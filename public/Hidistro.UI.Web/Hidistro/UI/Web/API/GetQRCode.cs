namespace Hidistro.UI.Web.API
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;
    using ThoughtWorks.QRCode.Codec;

    public class GetQRCode : IHttpHandler
    {
        public static Image CombinImage(Image imgBack, Image img)
        {
            if ((img.Height != 0x41) || (img.Width != 0x41))
            {
                img = KiResizeImage(img, 250, 250, 0);
            }
            Graphics graphics = Graphics.FromImage(imgBack);
            graphics.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);
            graphics.DrawImage(img, ((imgBack.Width / 2) - (img.Width / 2)) + 10, ((imgBack.Width / 2) - (img.Width / 2)) + 0x55, 0x88, 0x88);
            GC.Collect();
            return imgBack;
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
            if (!string.IsNullOrEmpty(str))
            {
                Image img = new QRCodeEncoder { QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeScale = 4, QRCodeVersion = 8, QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M }.Encode(str);
                MemoryStream stream = new MemoryStream();
                img.Save(stream, ImageFormat.Png);
                Image imgBack = Image.FromFile(context.Server.MapPath("/Storage/master/QRcord.jpg"));
                MemoryStream stream2 = new MemoryStream();
                CombinImage(imgBack, img).Save(stream2, ImageFormat.Png);
                context.Response.ClearContent();
                context.Response.ContentType = "image/png";
                context.Response.BinaryWrite(stream2.ToArray());
                stream.Dispose();
                stream2.Dispose();
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

