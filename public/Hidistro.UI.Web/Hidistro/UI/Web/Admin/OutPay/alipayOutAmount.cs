namespace Hidistro.UI.Web.Admin.OutPay
{
    using Hidistro.ControlPanel.OutPay.App;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    public class alipayOutAmount : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string partner = masterSettings.Alipay_Pid;
            string str2 = masterSettings.Alipay_Key;
            string str3 = masterSettings.Alipay_mid;
            string str4 = masterSettings.Alipay_mName;
            string str5 = "utf-8";
            Core.setConfig(partner, "MD5", str2, str5);
            string str6 = Globals.HostPath(HttpContext.Current.Request.Url) + "/admin/OutPay/AliPaynotifyAmount_url.aspx";
            string str7 = DateTime.Now.ToString("yyyyMMdd");
            string str8 = DateTime.Now.ToString("yyyyMMddHHmmssff");
            decimal num = 0M;
            int num2 = 0;
            string str9 = "";
            str9 = base.Request.Form["Paydata"];
            if (string.IsNullOrEmpty(str9))
            {
                base.Response.Write("<span style='line-height:40px;color:red;padding:10px'>付款申请参数不能为空！</span>");
                base.Response.End();
            }
            string s = "";
            string[] strArray = str9.Split(new char[] { '|' });
            foreach (string str11 in strArray)
            {
                string[] strArray2 = str11.Split(new char[] { '^' });
                if (strArray2.Length == 5)
                {
                    decimal result = 0M;
                    if (decimal.TryParse(strArray2[3], out result))
                    {
                        num += result;
                        num2++;
                    }
                    else
                    {
                        if (strArray2[1].Length < 4)
                        {
                            string[] strArray4 = new string[] { "<span style='line-height:40px;color:red;padding:10px'>第", (num2 + 1).ToString(), "帐户名不正确,请检查：", str11, "</span>" };
                            s = string.Concat(strArray4);
                        }
                        else
                        {
                            string[] strArray5 = new string[] { "<span style='line-height:40px;color:red;padding:10px'>第", (num2 + 1).ToString(), "项付款参数有误,请检查：", str11, "</span>" };
                            s = string.Concat(strArray5);
                        }
                        break;
                    }
                }
            }
            if (num2 != strArray.Length)
            {
                base.Response.Write(s);
                base.Response.End();
            }
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("_input_charset", str5);
            sParaTemp.Add("service", "batch_trans_notify");
            sParaTemp.Add("notify_url", str6);
            sParaTemp.Add("email", str3);
            sParaTemp.Add("account_name", str4);
            sParaTemp.Add("pay_date", str7);
            sParaTemp.Add("batch_no", str8);
            sParaTemp.Add("batch_fee", num.ToString());
            sParaTemp.Add("batch_num", num2.ToString());
            sParaTemp.Add("detail_data", str9);
            string str12 = Core.BuildRequest(sParaTemp, "get", "确认");
            base.Response.Write(str12);
        }
    }
}

