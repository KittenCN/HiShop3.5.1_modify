namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    public class UserStatisticeChart : IHttpHandler
    {
        public MemoryStream ShowChart_MS = new MemoryStream();

        public int FunMaxNum(string[] num)
        {
            decimal num2 = Convert.ToDecimal(num[0]);
            for (int i = 1; i < num.Length; i++)
            {
                if (Convert.ToDecimal(num[i]) > num2)
                {
                    num2 = Convert.ToDecimal(num[i]);
                }
            }
            return Convert.ToInt32(num2);
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string chartType = string.Empty;
                string xValues = string.Empty;
                string yValues = string.Empty;
                int? width = null;
                if ((!string.IsNullOrEmpty(context.Request.Params["ChartType"]) && !string.IsNullOrEmpty(context.Request.Params["XValues"])) && !string.IsNullOrEmpty(context.Request.Params["YValues"]))
                {
                    chartType = context.Request.QueryString["ChartType"];
                    xValues = context.Request.QueryString["XValues"];
                    yValues = context.Request.QueryString["YValues"];
                }
                if (!string.IsNullOrEmpty(context.Request.Params["Width"]))
                {
                    width = new int?(int.Parse(context.Request.Params["Width"]));
                }
                this.ShowChar(chartType, xValues, yValues, width);
                context.Response.ClearContent();
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(this.ShowChart_MS.ToArray());
            }
            catch
            {
            }
        }

        public void ShowChar(string ChartType, string XValues, string YValues, int? width)
        {
            if (ChartType != null)
            {
                bool flag;
                string str3 = ChartType;
                string str = XValues;
                string str2 = YValues;
                string str4 = "false";
                if (str4 == null)
                {
                    flag = false;
                }
                else
                {
                    try
                    {
                        flag = Convert.ToBoolean(str4);
                    }
                    catch
                    {
                        flag = false;
                    }
                }
                if ((str != null) && (str2 != null))
                {
                    Color white;
                    string str5;
                    string[] strArray = str2.Split("|".ToCharArray());
                    int num = 6;
                    int num2 = this.FunMaxNum(strArray);
                    if (num2 < 5)
                    {
                        num = num2 + 1;
                    }
                    if (flag)
                    {
                        white = Color.White;
                    }
                    else
                    {
                        white = Color.FromArgb(0xff, 0xfd, 0xf4);
                    }
                    Bitmap bitmap = null;
                    if (((str5 = str3) != null) && (str5 == "bar"))
                    {
                        CustomChart.BarGraph graph = new CustomChart.BarGraph(white) {
                            VerticalLabel = "",
                            VerticalTickCount = num,
                            ShowLegend = false,
                            ShowData = true,
                            Height = 200,
                            Width = width.HasValue ? width.Value : 600
                        };
                        graph.CollectDataPoints(str.Split("|".ToCharArray()), str2.Split("|".ToCharArray()));
                        bitmap = graph.Draw();
                    }
                    bitmap.Save(this.ShowChart_MS, ImageFormat.Png);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

