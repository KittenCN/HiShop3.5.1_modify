namespace Hishop.Plugins.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.IO;
    using System.Web;

    public class PayLog
    {
        public static void AppendLog(IDictionary<string, string> param, string sign, string url, string msg, LogType logtype)
        {
            CheckFileSize(logtype);
            using (StreamWriter writer = File.AppendText(GetLogPath + "pay_" + logtype.ToString() + ".txt"))
            {
                writer.WriteLine("时间：" + DateTime.Now.ToString());
                if ((param != null) && (param.Count > 0))
                {
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        writer.WriteLine(pair.Key + ":" + pair.Value);
                    }
                }
                writer.WriteLine("HishopUrl:" + url);
                writer.WriteLine("Hishopmsg:" + msg);
                writer.WriteLine("Hishopsign:" + sign);
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("");
            }
        }

        public static void AppendLog_Collection(NameValueCollection param, string sign, string url, string msg, LogType logtype)
        {
            CheckFileSize(logtype);
            using (StreamWriter writer = File.AppendText(GetLogPath + "pay_" + logtype.ToString() + ".txt"))
            {
                writer.WriteLine("时间：" + DateTime.Now.ToString());
                if (param != null)
                {
                    foreach (string str in param.AllKeys)
                    {
                        writer.WriteLine(str + ":" + param[str]);
                    }
                }
                writer.WriteLine("HishopUrl:" + url);
                writer.WriteLine("Hishopmsg:" + msg);
                writer.WriteLine("Hishopsign:", sign);
                writer.WriteLine("");
                writer.WriteLine("");
                writer.WriteLine("");
            }
        }

        public static void CheckFileSize(LogType logtype)
        {
            string path = GetLogPath + "pay_" + logtype.ToString() + ".txt";
            if (File.Exists(path))
            {
                FileStream stream = File.Open(path, FileMode.Open);
                if (stream.Length > 0x2800L)
                {
                    File.Delete(path);
                }
                stream.Close();
            }
        }

        public static void writeLog(IDictionary<string, string> param, string sign, string url, string msg, LogType logtype)
        {
            try
            {
                DataTable table = new DataTable {
                    TableName = "log"
                };
                table.Columns.Add(new DataColumn("HishopOperTime"));
                if (param != null)
                {
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        table.Columns.Add(new DataColumn(pair.Key));
                    }
                }
                table.Columns.Add(new DataColumn("HishopMsg"));
                table.Columns.Add(new DataColumn("HishopSign"));
                table.Columns.Add(new DataColumn("HishopUrl"));
                DataRow row = table.NewRow();
                row["HishopOperTime"] = DateTime.Now;
                if (param != null)
                {
                    foreach (KeyValuePair<string, string> pair in param)
                    {
                        row[pair.Key] = pair.Value;
                    }
                }
                row["HishopMsg"] = msg;
                row["HishopSign"] = sign;
                row["HishopUrl"] = url;
                table.Rows.Add(row);
                table.WriteXml(GetLogPath + "pay_" + logtype.ToString("G") + ".xml");
            }
            catch (Exception)
            {
            }
        }

        public static void writeLog_Collection(NameValueCollection param, string sign, string url, string msg, LogType logtype)
        {
            try
            {
                DataTable table = new DataTable {
                    TableName = "log"
                };
                table.Columns.Add(new DataColumn("HishopOperTime"));
                if (param != null)
                {
                    foreach (string str in param.AllKeys)
                    {
                        table.Columns.Add(new DataColumn(str));
                    }
                }
                table.Columns.Add(new DataColumn("HishopMsg"));
                table.Columns.Add(new DataColumn("HishopSign"));
                table.Columns.Add(new DataColumn("HishopUrl"));
                DataRow row = table.NewRow();
                row["HishopOperTime"] = DateTime.Now;
                if (param != null)
                {
                    foreach (string str in param.AllKeys)
                    {
                        row[str] = param[str];
                    }
                }
                row["HishopMsg"] = msg;
                row["HishopSign"] = sign;
                row["HishopUrl"] = url;
                table.Rows.Add(row);
                table.WriteXml(GetLogPath + "pay_" + logtype.ToString("G") + ".xml");
            }
            catch (Exception exception)
            {
                AppendLog(null, "", "", exception.Message, LogType.Alipay_Direct);
            }
        }

        private static string GetLogPath
        {
            get
            {
                string path = "";
                if (HttpContext.Current != null)
                {
                    path = HttpContext.Current.Server.MapPath("/log/");
                }
                else
                {
                    string str2 = "/log/";
                    str2 = str2.Replace("/", @"\");
                    if (str2.StartsWith(@"\"))
                    {
                        str2 = str2.TrimStart(new char[] { '\\' });
                    }
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, str2);
                }
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }
    }
}

