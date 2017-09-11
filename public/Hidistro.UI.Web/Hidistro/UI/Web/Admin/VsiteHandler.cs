namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Linq;
    public class VsiteHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request.Form["actionName"];
            string s = string.Empty;
            switch (str)
            {
                case "Vote":
                    s = JsonConvert.SerializeObject(StoreHelper.GetVoteList());
                    goto Label_0547;

                case "Category":
                    s = JsonConvert.SerializeObject(from item in CatalogHelper.GetMainCategories() select new { CateId = item.CategoryId, CateName = item.Name });
                    goto Label_0547;

                case "Activity":
                {
                    Array values = Enum.GetValues(typeof(LotteryActivityType));
                    List<EnumJson> list2 = new List<EnumJson>();
                    foreach (Enum enum2 in values)
                    {
                        EnumJson json = new EnumJson {
                            Name = enum2.ToShowText(),
                            Value = enum2.ToString()
                        };
                        list2.Add(json);
                    }
                    s = JsonConvert.SerializeObject(list2);
                    goto Label_0547;
                }
                case "ActivityList":
                {
                    string str3 = context.Request.Form["acttype"];
                    LotteryActivityType type = (LotteryActivityType) Enum.Parse(typeof(LotteryActivityType), str3);
                    if (type == LotteryActivityType.SignUp)
                    {
                        s = JsonConvert.SerializeObject(from item in VShopHelper.GetAllActivity() select new { ActivityId = item.ActivityId, ActivityName = item.Name });
                    }
                    goto Label_0547;
                }
                case "AccountTime":
                {
                    s = s + "{";
                    BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                        RequestTime = "",
                        CheckTime = "",
                        StoreName = "",
                        PageIndex = 1,
                        PageSize = 1,
                        SortOrder = SortAction.Desc,
                        SortBy = "RequestTime",
                        RequestEndTime = "",
                        RequestStartTime = "",
                        IsCheck = "1",
                        UserId = context.Request.Form["UserID"]
                    };
                    Globals.EntityCoding(entity, true);
                    DataTable data = (DataTable) DistributorsBrower.GetBalanceDrawRequest(entity, null).Data;
                    if (data.Rows.Count <= 0)
                    {
                        s = s + "\"Time\":\"\"";
                        break;
                    }
                    if (!(data.Rows[0]["MerchantCode"].ToString().Trim() != context.Request.Form["merchantcode"].Trim()))
                    {
                        s = s + "\"Time\":\"\"";
                        break;
                    }
                    s = s + "\"Time\":\"" + data.Rows[0]["RequestTime"].ToString() + "\"";
                    break;
                }
                case "ProdSelect":
                {
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
                    {
                        DataTable products = ProductHelper.GetProducts(masterSettings.DistributorProducts);
                        if ((products != null) && (products.Rows.Count > 0))
                        {
                            List<SelectProduct> list3 = new List<SelectProduct>();
                            foreach (DataRow row in products.Rows)
                            {
                                SelectProduct product = new SelectProduct {
                                    productid = row["productid"].ToString(),
                                    ProductName = row["ProductName"].ToString(),
                                    ThumbnailUrl60 = row["ThumbnailUrl60"].ToString()
                                };
                                list3.Add(product);
                            }
                            s = JsonConvert.SerializeObject(list3);
                        }
                    }
                    goto Label_0547;
                }
                case "ProdDel":
                {
                    s = s + "{";
                    string str4 = context.Request.Form["productid"];
                    SiteSettings settings = SettingsManager.GetMasterSettings(false);
                    if (!string.IsNullOrEmpty(settings.DistributorProducts) && settings.DistributorProducts.Contains(str4))
                    {
                        string str5 = "";
                        foreach (string str6 in settings.DistributorProducts.Split(new char[] { ',' }))
                        {
                            if (!str6.Equals(str4))
                            {
                                str5 = str5 + str6 + ",";
                            }
                        }
                        if (str5.Length > 0)
                        {
                            str5 = str5.Substring(0, str5.Length - 1);
                        }
                        settings.DistributorProducts = str5;
                        SettingsManager.Save(settings);
                    }
                    s = s + "\"status\":\"ok\"" + "}";
                    goto Label_0547;
                }
                default:
                    goto Label_0547;
            }
            s = s + "}";
        Label_0547:
            context.Response.Write(s);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private class EnumJson
        {
            public string Name { get; set; }

            public string Value { get; set; }
        }

        private class SelectProduct
        {
            public string productid { get; set; }

            public string ProductName { get; set; }

            public string ThumbnailUrl60 { get; set; }
        }
    }
}

