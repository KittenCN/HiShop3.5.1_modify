namespace Hidistro.UI.Web.Admin.distributor
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class DistributorLogoUpload : Page
    {
        protected HtmlForm form1;

        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            Image image = b;
            ImageFormat rawFormat = image.RawFormat;
            int width = 0;
            int height = 0;
            int num3 = image.Width;
            int num4 = image.Height;
            if ((num4 > destHeight) || (num3 > destWidth))
            {
                if ((num3 * destHeight) < (num4 * destWidth))
                {
                    width = destWidth;
                    height = (destWidth * num4) / num3;
                }
                else
                {
                    height = destHeight;
                    width = (num3 * destHeight) / num4;
                }
            }
            else
            {
                width = destWidth;
                height = destHeight;
            }
            Bitmap bitmap = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(image, new Rectangle((destWidth - width) / 2, (destHeight - height) / 2, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            graphics.Dispose();
            EncoderParameters parameters = new EncoderParameters();
            long[] numArray = new long[] { 100L };
            EncoderParameter parameter = new EncoderParameter(Encoder.Quality, numArray);
            parameters.Param[0] = parameter;
            image.Dispose();
            return bitmap;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.QueryString["delimg"] != null)
            {
                base.Response.Write("0");
                base.Response.End();
            }
            int num = int.Parse(base.Request.QueryString["imgurl"]);
            try
            {
                if (num < 1)
                {
                    HttpPostedFile file = base.Request.Files["Filedata"];
                    string str = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);
                    string str2 = "/Storage/data/DistributorLogoPic/";
                    string str3 = Path.GetExtension(file.FileName).ToLower();
                    switch (str3)
                    {
                        case ".gif":
                        case ".jpg":
                        case ".png":
                        case ".jpeg":
                        {
                            string str4 = str + str3;
                            string s = Globals.UploadFileAndCheck(file, Globals.MapPath(str2 + str4));
                            if (s == "1")
                            {
                                try
                                {
                                    Bitmap b = new Bitmap(Globals.MapPath(str2 + str4));
                                    if ((b.Height > 200) || (b.Width > 200))
                                    {
                                        b = GetThumbnail(b, 200, 200);
                                    }
                                    b.Save(Globals.MapPath("/Utility/pics/headLogo.jpg"), ImageFormat.Jpeg);
                                    b.Dispose();
                                }
                                catch (Exception)
                                {
                                }
                                base.Response.StatusCode = 200;
                                base.Response.Write(str + "|/Storage/data/DistributorLogoPic/" + str4);
                                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                                string distributorLogoPic = masterSettings.DistributorLogoPic;
                                distributorLogoPic = base.Server.MapPath(distributorLogoPic);
                                masterSettings.DistributorLogoPic = "/Storage/data/DistributorLogoPic/" + str4;
                                SettingsManager.Save(masterSettings);
                            }
                            else
                            {
                                base.Response.Write(s);
                                base.Response.End();
                            }
                            return;
                        }
                    }
                    base.Response.StatusCode = 500;
                    base.Response.Write("服务器错误");
                    base.Response.End();
                }
                else
                {
                    base.Response.Write("0");
                }
            }
            catch (Exception)
            {
                base.Response.StatusCode = 500;
                base.Response.Write("服务器错误");
                base.Response.End();
            }
            finally
            {
                base.Response.End();
            }
        }
    }
}

