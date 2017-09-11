namespace Hishop.Plugins.Payment.BankUnion
{
    using Hishop.Plugins;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web;

    [Plugin("银联在线")]
    public class BankUnionRequest : PaymentRequest
    {
        private const string Gateway = "https://unionpaysecure.com/api/Pay.action";
        private const string Remark1 = "Bankunion";
        private readonly string v_amount;
        private readonly string v_date;
        private const string v_moneytype = "CNY";
        private readonly string v_notifyUrl;
        private readonly string v_oid;
        private readonly string v_returnUrl;

        public BankUnionRequest()
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_date = "";
            this.v_returnUrl = "";
            this.v_notifyUrl = "";
        }

        public BankUnionRequest(string orderId, decimal amount, string subject, string body, string buyerEmail, DateTime date, string showUrl, string returnUrl, string notifyUrl, string attach)
        {
            this.v_oid = "";
            this.v_amount = "";
            this.v_date = "";
            this.v_returnUrl = "";
            this.v_notifyUrl = "";
            this.v_oid = orderId;
            this.v_amount = Math.Round((decimal) (amount * 100M), 0).ToString();
            this.v_returnUrl = returnUrl;
            this.v_date = date.ToString("yyyyMMddHHmmss");
            this.v_notifyUrl = notifyUrl;
        }

        public static string GetUserIP()
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            switch (userHostAddress)
            {
                case null:
                case "":
                    userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    break;
            }
            if ((userHostAddress == null) || (userHostAddress == string.Empty))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            if (((userHostAddress.Length >= 20) || (userHostAddress == null)) || string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = "127.0.0.1";
            }
            return userHostAddress;
        }

        private string joinMapValue(SortedDictionary<string, string> map, char connector)
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in map)
            {
                builder.Append(pair.Key);
                builder.Append('=');
                if (pair.Value != null)
                {
                    builder.Append(pair.Value);
                }
                builder.Append(connector);
            }
            return builder.ToString();
        }

        public override void SendGoods(string tradeno, string logisticsName, string invoiceno, string transportType)
        {
        }

        public override void SendRequest()
        {
            QuickPayConf.merCode = this.Vmid;
            QuickPayConf.merName = this.VmName;
            QuickPayConf.securityKey = this.Key;
            this.ValueVo = new string[] { 
                QuickPayConf.version, QuickPayConf.charset, "01", "", QuickPayConf.merCode, QuickPayConf.merName, "", "", "", "", this.v_amount, "1", "0", "0", this.v_oid, this.v_amount, 
                "156", this.v_date, GetUserIP(), "", "", "", "", this.v_returnUrl, this.v_notifyUrl, ""
             };
            QuickPayConf.gateWay = "https://unionpaysecure.com/api/Pay.action";
            this.SubmitPaymentForm("");
        }

        protected override void SubmitPaymentForm(string formContent)
        {
            string s = new QuickPayUtils().createPayHtml(this.ValueVo);
            HttpContext.Current.Response.ContentType = "text/html;charset=" + QuickPayConf.charset;
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(QuickPayConf.charset);
            try
            {
                SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>(StringComparer.Ordinal);
                for (int i = 0; i < QuickPayConf.reqVo.Length; i++)
                {
                    dictionary.Add(QuickPayConf.reqVo[i], this.ValueVo[i]);
                }
                HttpContext.Current.Response.Write(s);
            }
            catch (Exception)
            {
            }
        }

        public void WriteError(string data, string error)
        {
            DataTable table = new DataTable {
                TableName = "BankUnion"
            };
            table.Columns.Add(new DataColumn("OperTime"));
            table.Columns.Add(new DataColumn("OperIP"));
            table.Columns.Add(new DataColumn("OperData"));
            table.Columns.Add(new DataColumn("Html"));
            DataRow row = table.NewRow();
            row["OperTime"] = DateTime.Now;
            row["OperIP"] = GetUserIP();
            row["OperData"] = data;
            row["html"] = error;
            table.Rows.Add(row);
            table.WriteXml(HttpContext.Current.Server.MapPath("/BankUnionErr.xml"));
        }

        public override string Description
        {
            get
            {
                return string.Empty;
            }
        }

        public override bool IsMedTrade
        {
            get
            {
                return false;
            }
        }

        [ConfigElement("商户密钥", Nullable=false)]
        public string Key { get; set; }

        public override string Logo
        {
            get
            {
                return string.Empty;
            }
        }

        protected override bool NeedProtect
        {
            get
            {
                return true;
            }
        }

        public override string ShortDescription
        {
            get
            {
                return string.Empty;
            }
        }

        private string[] ValueVo { get; set; }

        [ConfigElement("商户号", Nullable=false)]
        public string Vmid { get; set; }

        [ConfigElement("商户名称", Nullable=false)]
        public string VmName { get; set; }
    }
}

