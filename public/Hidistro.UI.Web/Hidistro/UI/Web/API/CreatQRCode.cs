namespace Hidistro.UI.Web.API
{
    using Hidistro.Core;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Web;
    using ThoughtWorks.QRCode.Codec;

    public class CreatQRCode : IHttpHandler
    {
        public static Image CombinImage(Image QRimg, Image Logoimg, int logoW, int WhiteSpace)
        {
            Bitmap image = new Bitmap(430, 430);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            graphics.DrawImage(QRimg, WhiteSpace, WhiteSpace, (int) (430 - (WhiteSpace * 2)), (int) (430 - (WhiteSpace * 2)));
            if (Logoimg != null)
            {
                graphics.DrawImage(Logoimg, (image.Width - logoW) / 2, (image.Height - logoW) / 2, logoW, logoW);
            }
            return image;
        }

        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180f, 90f);
            path.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - (cornerRadius * 2), rect.Y);
            path.AddArc((rect.X + rect.Width) - (cornerRadius * 2), rect.Y, cornerRadius * 2, cornerRadius * 2, 270f, 90f);
            path.AddLine(rect.Right, rect.Y + (cornerRadius * 2), rect.Right, (rect.Y + rect.Height) - (cornerRadius * 2));
            path.AddArc((int) ((rect.X + rect.Width) - (cornerRadius * 2)), (int) ((rect.Y + rect.Height) - (cornerRadius * 2)), (int) (cornerRadius * 2), (int) (cornerRadius * 2), 0f, 90f);
            path.AddLine(rect.Right - (cornerRadius * 2), rect.Bottom, rect.X + (cornerRadius * 2), rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 90f, 90f);
            path.AddLine(rect.X, rect.Bottom - (cornerRadius * 2), rect.X, rect.Y + (cornerRadius * 2));
            path.CloseFigure();
            return path;
        }

        private Image getNetImg(string imgUrl)
        {
            try
            {
                if (imgUrl.ToLower().StartsWith("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = (se, ert, chain, sslerror) => true;
                }
                Random random = new Random();
                if (imgUrl.Contains("?"))
                {
                    imgUrl = imgUrl + "&aid=&" + random.NextDouble();
                }
                else
                {
                    imgUrl = imgUrl + "?aid=&" + random.NextDouble();
                }
                Stream responseStream = WebRequest.Create(imgUrl).GetResponse().GetResponseStream();
                Image image = Image.FromStream(responseStream);
                responseStream.Close();
                return image;
            }
            catch (Exception exception)
            {
                Globals.Debuglog(imgUrl + ";读取网络图片异常" + exception.Message, "_Debuglog.txt");
                return new Bitmap(100, 100);
            }
        }

        public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image image = new Bitmap(newW, newH);
                Graphics graphics = Graphics.FromImage(image);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                graphics.Dispose();
                return image;
            }
            catch
            {
                return null;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string str = context.Request["code"];
            string str2 = context.Request["Combin"];
            string str3 = context.Request["Logo"];
            if (!string.IsNullOrEmpty(str))
            {
                Image qRimg = new QRCodeEncoder { QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeScale = 6, QRCodeVersion = 5, QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M }.Encode(str);
                Image image = null;
                if (!string.IsNullOrEmpty(str3))
                {
                    if (!str3.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(str3)))
                    {
                        image = Image.FromFile(context.Server.MapPath(str3));
                    }
                    else if (str3.ToLower().StartsWith("http"))
                    {
                        image = this.getNetImg(str3);
                    }
                }
                Bitmap bitmap = null;
                if (image != null)
                {
                    bitmap = new Bitmap(200, 200);
                    GraphicsPath path = CreateRoundedRectanglePath(new Rectangle(0, 0, 200, 200), 20);
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.SetClip(path);
                        graphics.Clear(Color.White);
                        path = CreateRoundedRectanglePath(new Rectangle(14, 14, 0xac, 0xac), 14);
                        graphics.SetClip(path);
                        graphics.DrawImage(image, 0, 0, 200, 200);
                    }
                    image.Dispose();
                }
                qRimg = CombinImage(qRimg, bitmap, 80, 30);
                MemoryStream stream = new MemoryStream();
                qRimg.Save(stream, ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(stream.ToArray());
                stream.Dispose();
                qRimg.Dispose();
            }
            else if (!string.IsNullOrEmpty(str2) && !string.IsNullOrEmpty(str3))
            {
                Image image3 = null;
                if (!str3.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(str3)))
                {
                    image3 = Image.FromFile(context.Server.MapPath(str3));
                }
                else if (str3.ToLower().StartsWith("http"))
                {
                    image3 = this.getNetImg(str3);
                }
                else
                {
                    context.Response.End();
                }
                Image image4 = null;
                if (!str2.ToLower().StartsWith("http") && System.IO.File.Exists(context.Server.MapPath(str2)))
                {
                    image4 = Image.FromFile(context.Server.MapPath(str2));
                }
                else if (str2.ToLower().StartsWith("http"))
                {
                    image4 = this.getNetImg(Globals.UrlDecode(str2));
                }
                else
                {
                    context.Response.End();
                }
                Bitmap bitmap2 = null;
                if (image3 != null)
                {
                    bitmap2 = new Bitmap(200, 200);
                    GraphicsPath path2 = CreateRoundedRectanglePath(new Rectangle(0, 0, 200, 200), 20);
                    using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                    {
                        graphics2.SetClip(path2);
                        graphics2.Clear(Color.White);
                        path2 = CreateRoundedRectanglePath(new Rectangle(14, 14, 0xac, 0xac), 14);
                        graphics2.SetClip(path2);
                        graphics2.DrawImage(image3, 0, 0, 200, 200);
                    }
                    image3.Dispose();
                }
                image4 = CombinImage(image4, bitmap2, 80, 0);
                MemoryStream stream2 = new MemoryStream();
                image4.Save(stream2, ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(stream2.ToArray());
                stream2.Dispose();
                image4.Dispose();
            }
            context.Response.Flush();
            context.Response.End();
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

