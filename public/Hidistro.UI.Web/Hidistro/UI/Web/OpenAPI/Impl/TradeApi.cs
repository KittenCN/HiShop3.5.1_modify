namespace Hidistro.UI.Web.OpenAPI.Impl
{
    using global::Hishop.Open.Api;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Vshop;
    using global::Hishop.Plugins;
    using global::Hishop.Weixin.Pay;
    using global::Hishop.Weixin.Pay.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class TradeApi : ITrade
    {
        public string ChangLogistics(string tid, string company_name, string out_sid)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(tid);
            if ((orderInfo == null) || string.IsNullOrEmpty(orderInfo.OrderId))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_not_Exists, "tid");
            }
            if ((((orderInfo.OrderStatus == OrderStatus.Refunded) || (orderInfo.OrderStatus == OrderStatus.Returned)) || (orderInfo.OrderStatus == OrderStatus.Closed)) || (((orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid) && ((orderInfo.Gateway != "hishop.plugins.payment.podrequest") || (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay))) && (orderInfo.OrderStatus != OrderStatus.SellerAlreadySent)))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_Print, "orderstatue");
            }
            ExpressCompanyInfo info2 = ExpressHelper.FindNode(company_name);
            orderInfo.ExpressCompanyAbb = info2.Kuaidi100Code;
            orderInfo.ExpressCompanyName = info2.Name;
            orderInfo.IsPrinted = true;
            orderInfo.ShipOrderNumber = out_sid;
            if (!OrderHelper.UpdateOrder(orderInfo))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Print_Faild, "order");
            }
            string format = "{{\"logistics_change_response\":{{\"shipping\":{{\"is_success\":{0}}}}}}}";
            return string.Format(format, "true");
        }

        public bool CheckHasNext(int totalrecord, int pagesize, int pageindex)
        {
            int num = pagesize * pageindex;
            return (totalrecord > num);
        }

        private string ConvertTrades(OrderInfo orderInfo)
        {
            trade_list_model _model = new trade_list_model {
                tid = orderInfo.OrderId,
                buyer_memo = orderInfo.Remark,
                seller_memo = orderInfo.ManagerRemark,
                seller_flag = orderInfo.ManagerMark.HasValue ? Convert.ToInt16(orderInfo.ManagerMark).ToString() : "0",
                discount_fee = orderInfo.AdjustedDiscount,
                status = EnumDescription.GetEnumDescription(orderInfo.OrderStatus, 1),
                close_memo = orderInfo.CloseReason,
                created = new DateTime?(orderInfo.OrderDate),
                modified = new DateTime?(orderInfo.UpdateDate),
                pay_time = orderInfo.PayDate,
                consign_time = orderInfo.ShippingDate,
                end_time = orderInfo.FinishDate,
                buyer_uname = orderInfo.Username,
                buyer_email = orderInfo.EmailAddress,
                buyer_nick = orderInfo.RealName,
                receiver_name = orderInfo.ShipTo
            };
            string fullRegion = RegionHelper.GetFullRegion(orderInfo.RegionId, "-");
            if (!string.IsNullOrEmpty(fullRegion))
            {
                string[] strArray = fullRegion.Split(new char[] { '-' });
                _model.receiver_state = strArray[0];
                if (strArray.Length >= 2)
                {
                    _model.receiver_city = strArray[1];
                }
                if (strArray.Length >= 3)
                {
                    _model.receiver_district = strArray[2];
                }
                if (strArray.Length >= 4)
                {
                    _model.receiver_town = strArray[3];
                }
            }
            _model.receiver_address = orderInfo.Address;
            _model.receiver_mobile = orderInfo.CellPhone;
            _model.receiver_zip = orderInfo.ZipCode;
            _model.invoice_fee = orderInfo.Tax;
            _model.invoice_title = orderInfo.InvoiceTitle;
            _model.payment = orderInfo.GetTotal();
            _model.storeId = "0";
            foreach (LineItemInfo info in orderInfo.LineItems.Values)
            {
                string str2 = Globals.HtmlEncode(info.SKUContent);
                trade_itme_model item = new trade_itme_model {
                    sku_id = info.SkuId,
                    sku_properties_name = str2,
                    num_id = info.ProductId.ToString(),
                    num = info.Quantity,
                    title = info.ItemDescription,
                    outer_sku_id = info.SKU,
                    pic_path = info.ThumbnailsUrl,
                    price = decimal.Round(info.ItemAdjustedPrice, 2),
                    refund_status = EnumDescription.GetEnumDescription(info.OrderItemsStatus, 1)
                };
                _model.orders.Add(item);
            }
            return JsonConvert.SerializeObject(_model);
        }

        public string ConvertTrades(DataSet dstrades)
        {
            List<trade_list_model> list = new List<trade_list_model>();
            foreach (DataRow row in dstrades.Tables[0].Rows)
            {
                trade_list_model item = new trade_list_model();
                foreach (DataRow row2 in row.GetChildRows("OrderRelation"))
                {
                    string str = Globals.HtmlEncode(row2["SKUContent"].ToString());
                    trade_itme_model _model2 = new trade_itme_model {
                        sku_id = (string) row2["SkuId"],
                        sku_properties_name = str,
                        num_id = row2["ProductId"].ToString(),
                        num = (int) row2["Quantity"],
                        title = (string) row2["ItemDescription"],
                        outer_sku_id = (string) row2["SKU"],
                        pic_path = (row2["ThumbnailsUrl"] != DBNull.Value) ? ((string) row2["ThumbnailsUrl"]) : "",
                        price = (decimal) row2["ItemAdjustedPrice"],
                        refund_status = EnumDescription.GetEnumDescription((OrderStatus) Enum.Parse(typeof(OrderStatus), row2["OrderItemsStatus"].ToString()), 1)
                    };
                    item.orders.Add(_model2);
                }
                item.tid = (string) row["OrderId"];
                if (row["Remark"] != DBNull.Value)
                {
                    item.buyer_memo = row["Remark"].ToString();
                }
                if (row["ManagerRemark"] != DBNull.Value)
                {
                    item.seller_memo = row["ManagerRemark"].ToString();
                }
                if (row["ManagerMark"] != DBNull.Value)
                {
                    item.seller_flag = row["ManagerMark"].ToString();
                }
                item.discount_fee = (decimal) row["AdjustedDiscount"];
                item.status = EnumDescription.GetEnumDescription((OrderStatus) Enum.Parse(typeof(OrderStatus), row["OrderStatus"].ToString()), 1);
                if ((row["Gateway"].ToString() == "hishop.plugins.payment.podrequest") && (item.status == EnumDescription.GetEnumDescription((OrderStatus) Enum.Parse(typeof(OrderStatus), Convert.ToInt16(OrderStatus.WaitBuyerPay).ToString()), 1)))
                {
                    item.status = EnumDescription.GetEnumDescription((OrderStatus) Enum.Parse(typeof(OrderStatus), Convert.ToInt16(OrderStatus.BuyerAlreadyPaid).ToString()), 1);
                }
                if (row["CloseReason"] != DBNull.Value)
                {
                    item.close_memo = (string) row["CloseReason"];
                }
                item.created = new DateTime?(DateTime.Parse(row["OrderDate"].ToString()));
                if (row["UpdateDate"] != DBNull.Value)
                {
                    item.modified = new DateTime?(DateTime.Parse(row["UpdateDate"].ToString()));
                }
                if (row["PayDate"] != DBNull.Value)
                {
                    item.pay_time = new DateTime?(DateTime.Parse(row["PayDate"].ToString()));
                }
                if (row["ShippingDate"] != DBNull.Value)
                {
                    item.consign_time = new DateTime?(DateTime.Parse(row["ShippingDate"].ToString()));
                }
                if (row["FinishDate"] != DBNull.Value)
                {
                    item.end_time = new DateTime?(DateTime.Parse(row["FinishDate"].ToString()));
                }
                item.buyer_uname = (string) row["Username"];
                if (row["EmailAddress"] != DBNull.Value)
                {
                    item.buyer_email = (string) row["EmailAddress"];
                }
                if (row["RealName"] != DBNull.Value)
                {
                    item.buyer_nick = (string) row["RealName"];
                }
                if (row["ShipTo"] != DBNull.Value)
                {
                    item.receiver_name = (string) row["ShipTo"];
                }
                string fullRegion = RegionHelper.GetFullRegion(Convert.ToInt32(row["RegionId"]), "-");
                if (!string.IsNullOrEmpty(fullRegion))
                {
                    string[] strArray = fullRegion.Split(new char[] { '-' });
                    item.receiver_state = strArray[0];
                    if (strArray.Length >= 2)
                    {
                        item.receiver_city = strArray[1];
                    }
                    if (strArray.Length >= 3)
                    {
                        item.receiver_district = strArray[2];
                    }
                    else if (strArray.Length >= 2)
                    {
                        item.receiver_district = strArray[1];
                    }
                    if (strArray.Length >= 4)
                    {
                        item.receiver_town = strArray[3];
                    }
                }
                item.receiver_address = (string) row["Address"];
                item.receiver_mobile = (string) row["CellPhone"];
                item.receiver_zip = (string) row["ZipCode"];
                item.invoice_fee = (decimal) row["Tax"];
                item.invoice_title = (string) row["InvoiceTitle"];
                item.payment = (decimal) row["OrderTotal"];
                item.storeId = "0";
                list.Add(item);
            }
            return JsonConvert.SerializeObject(list);
        }

        public string GetIncrementSoldTrades(DateTime start_modified, DateTime end_modified, string status, string buyer_uname, int page_no, int page_size)
        {
            string format = "{{\"trades_sold_get_response\":{{\"total_results\":\"{0}\",\"has_next\":\"{1}\",\"trades\":{2}}}}}";
            OrderQuery entity = new OrderQuery {
                PageSize = 40,
                PageIndex = 1,
                Status = OrderStatus.All,
                UserName = buyer_uname,
                SortBy = "UpdateDate",
                SortOrder = SortAction.Desc,
                StartDate = new DateTime?(start_modified),
                EndDate = new DateTime?(end_modified)
            };
            OrderStatus all = OrderStatus.All;
            if (!string.IsNullOrEmpty(status))
            {
                EnumDescription.GetEnumValue<OrderStatus>(status, ref all);
            }
            entity.Status = all;
            if (page_no > 0)
            {
                entity.PageIndex = page_no;
            }
            if (page_size > 0)
            {
                entity.PageSize = page_size;
            }
            Globals.EntityCoding(entity, true);
            int records = 0;
            DataSet tradeOrders = OrderHelper.GetTradeOrders(entity, out records);
            string str2 = this.ConvertTrades(tradeOrders);
            bool flag = this.CheckHasNext(records, entity.PageSize, entity.PageIndex);
            return string.Format(format, records, flag, str2);
        }

        public string GetSoldTrades(DateTime? start_created, DateTime? end_created, string status, string buyer_uname, int page_no, int page_size)
        {
            string format = "{{\"trades_sold_get_response\":{{\"total_results\":\"{0}\",\"has_next\":\"{1}\",\"trades\":{2}}}}}";
            OrderQuery entity = new OrderQuery {
                PageSize = 40,
                PageIndex = 1,
                Status = OrderStatus.All,
                UserName = buyer_uname,
                SortBy = "OrderDate",
                SortOrder = SortAction.Desc
            };
            if (start_created.HasValue)
            {
                entity.StartDate = start_created;
            }
            if (end_created.HasValue)
            {
                entity.EndDate = end_created;
            }
            OrderStatus all = OrderStatus.All;
            if (!string.IsNullOrEmpty(status))
            {
                EnumDescription.GetEnumValue<OrderStatus>(status, ref all);
            }
            entity.Status = all;
            if (page_no > 0)
            {
                entity.PageIndex = page_no;
            }
            if (page_size > 0)
            {
                entity.PageSize = page_size;
            }
            Globals.EntityCoding(entity, true);
            int records = 0;
            DataSet tradeOrders = OrderHelper.GetTradeOrders(entity, out records);
            string str2 = this.ConvertTrades(tradeOrders);
            bool flag = this.CheckHasNext(records, entity.PageSize, entity.PageIndex);
            return string.Format(format, records, flag, str2);
        }

        public string GetTrade(string tid)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(tid);
            if ((orderInfo == null) || string.IsNullOrEmpty(orderInfo.OrderId))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_not_Exists, "tid");
            }
            string format = "{{\"trade_get_response\":{{\"trade\":{0}}}}}";
            string str2 = this.ConvertTrades(orderInfo);
            return string.Format(format, str2);
        }

        public string SendLogistic(string tid, string company_name, string out_sid)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(tid);
            if ((orderInfo == null) || string.IsNullOrEmpty(orderInfo.OrderId))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_not_Exists, "tid");
            }
            if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Success))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_Send, "group order");
            }
            if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_Send, "orderstatue");
            }
            if (string.IsNullOrEmpty(out_sid))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Missing_Required_Arguments, "out_sid");
            }
            ExpressCompanyInfo info2 = ExpressHelper.FindNode(company_name);
            if (info2 == null)
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Company_not_Exists, "company_name");
            }
            orderInfo.ExpressCompanyAbb = info2.Kuaidi100Code;
            orderInfo.ExpressCompanyName = info2.Name;
            orderInfo.ShipOrderNumber = out_sid;
            if (!OrderHelper.SendGoods(orderInfo))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_Status_Send, "send good");
            }
            Express.SubscribeExpress100(orderInfo.ExpressCompanyAbb, out_sid);
            SendNoteInfo note = new SendNoteInfo {
                NoteId = Globals.GetGenerateId(),
                OrderId = orderInfo.OrderId,
                Operator = orderInfo.UserId.ToString(),
                Remark = "接口发货成功"
            };
            OrderHelper.SaveSendNote(note);
            if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
            {
                if (orderInfo.Gateway == "hishop.plugins.payment.ws_wappay.wswappayrequest")
                {
                    PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                    if (paymentMode != null)
                    {
                        PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                    }
                }
                if (orderInfo.Gateway == "hishop.plugins.payment.weixinrequest")
                {
                    PayClient client;
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    if (masterSettings.EnableSP)
                    {
                        client = new PayClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
                    }
                    else
                    {
                        client = new PayClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
                    }
                    DeliverInfo deliver = new DeliverInfo {
                        TransId = orderInfo.GatewayOrderId,
                        OutTradeNo = orderInfo.OrderId,
                        OpenId = MemberHelper.GetMember(orderInfo.UserId).OpenId
                    };
                    client.DeliverNotify(deliver);
                }
            }
            orderInfo.OnDeliver();
            string format = "{{\"logistics_send_response\":{{\"shipping\":{{\"is_success\":{0}}}}}}}";
            return string.Format(format, "true");
        }

        public string UpdateTradeMemo(string tid, string memo, int flag)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(tid);
            if ((orderInfo == null) || string.IsNullOrEmpty(orderInfo.OrderId))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.Trade_not_Exists, "tid");
            }
            if (flag > 0)
            {
                orderInfo.ManagerMark = new OrderMark?((OrderMark) flag);
            }
            orderInfo.ManagerRemark = Globals.HtmlEncode(memo);
            if (!OrderHelper.SaveRemark(orderInfo))
            {
                return OpenApiErrorMessage.ShowErrorMsg(OpenApiErrorCode.System_Error, "save remark");
            }
            string format = "{{\"trade_memo_update_response\":{{\"trade\":{{\"tid\":\"{0}\",\"modified\":\"{1}\"}}}}}}";
            return string.Format(format, orderInfo.OrderId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}

