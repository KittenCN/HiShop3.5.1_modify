namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Goods.ascx;
    using System;

    [PrivilegeCheck(Privilege.AddProductType)]
    public class AddSpecification : AdminPage
    {
        protected SpecificationView specificationView;

        protected AddSpecification() : base("m02", "spp07")
        {
        }

        private void btnFilish_Click(object server, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ProductTypes.aspx"), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            if ((!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true")) && int.TryParse(base.Request["ValueId"], out num))
            {
                if (ProductTypeHelper.DeleteAttributeValue(num))
                {
                    if (!string.IsNullOrEmpty(base.Request["ImageUrl"]))
                    {
                        StoreHelper.DeleteImage(base.Request["ImageUrl"]);
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{\"Status\":\"true\"}");
                        base.Response.End();
                    }
                    else
                    {
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{\"Status\":\"true\"}");
                        base.Response.End();
                    }
                }
                else
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{\"Status\":\"false\"}");
                    base.Response.End();
                }
            }
            if (!string.IsNullOrEmpty(base.Request["isAjax"]) && (base.Request["isAjax"] == "true"))
            {
                string str = base.Request["Mode"].ToString();
                string str2 = "";
                string str3 = "false";
                string str6 = str;
                if (str6 != null)
                {
                    int num2;
                    if (!(str6 == "Add"))
                    {
                        if (!(str6 == "AddSkuItemValue"))
                        {
                            return;
                        }
                    }
                    else
                    {
                        num2 = 0;
                        str2 = "参数缺少";
                        if (int.TryParse(base.Request["AttributeId"], out num2))
                        {
                            string str4 = "";
                            str2 = "属性名称不允许为空！";
                            if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
                            {
                                str4 = Globals.HtmlEncode(base.Request["ValueName"].ToString());
                                AttributeValueInfo attributeValue = new AttributeValueInfo {
                                    ValueStr = str4,
                                    AttributeId = num2
                                };
                                int num3 = 0;
                                str2 = "添加属性值失败";
                                num3 = ProductTypeHelper.AddAttributeValue(attributeValue);
                                if (num3 > 0)
                                {
                                    str2 = num3.ToString();
                                    str3 = "true";
                                }
                            }
                        }
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{\"Status\":\"" + str3 + "\",\"msg\":\"" + str2 + "\"}");
                        base.Response.End();
                        return;
                    }
                    str2 = "参数缺少";
                    if (int.TryParse(base.Request["AttributeId"], out num2))
                    {
                        string str5 = "";
                        str2 = "规格值名不允许为空！";
                        if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
                        {
                            str5 = Globals.HtmlEncode(base.Request["ValueName"].ToString().Replace("+", "").Replace(",", ""));
                            str2 = "规格值名长度不允许超过50个字符";
                            if (str5.Length < 50)
                            {
                                AttributeValueInfo info2 = new AttributeValueInfo {
                                    ValueStr = str5,
                                    AttributeId = num2
                                };
                                int num4 = 0;
                                str2 = "添加规格值失败";
                                num4 = ProductTypeHelper.AddAttributeValue(info2);
                                if (num4 > 0)
                                {
                                    str2 = num4.ToString();
                                    str3 = "true";
                                }
                            }
                        }
                    }
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{\"Status\":\"" + str3 + "\",\"msg\":\"" + str2 + "\"}");
                    base.Response.End();
                }
            }
        }
    }
}

