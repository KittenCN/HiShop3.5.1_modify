namespace Hidistro.UI.Web
{
    using Hidistro.Core;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    public class VerifyCodeImage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                string str = Globals.CreateVerifyCode(4);
                int maxValue = 0x2d;
                int num2 = str.Length * 20;
                Bitmap image = new Bitmap(num2 - 3, 40);
                Graphics graphics = Graphics.FromImage(image);
                graphics.Clear(Color.AliceBlue);
                graphics.DrawRectangle(new Pen(Color.Gray, 0f), 0, 0, image.Width - 1, image.Height - 3);
                Random random = new Random();
                Pen pen = new Pen(Color.LightGray, 0f);
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(0, image.Width);
                    int y = random.Next(0, image.Height);
                    graphics.DrawRectangle(pen, x, y, 1, 1);
                }
                char[] chArray = str.ToCharArray();
                StringFormat format = new StringFormat(StringFormatFlags.NoClip) {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                Color[] colorArray = new Color[] { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.DarkCyan, Color.Purple, Color.DarkGreen };
                for (int j = 0; j < chArray.Length; j++)
                {
                    int index = random.Next(7);
                    random.Next(4);
                    Font font = new Font("Microsoft Sans Serif", 17f, FontStyle.Bold);
                    Brush brush = new SolidBrush(colorArray[index]);
                    Point point = new Point(14, 11);
                    float angle = random.Next(-maxValue, maxValue);
                    graphics.TranslateTransform((float) point.X, (float) point.Y);
                    graphics.RotateTransform(angle);
                    graphics.DrawString(chArray[j].ToString(), font, brush, 1f, 10f, format);
                    graphics.RotateTransform(-angle);
                    graphics.TranslateTransform(2f, (float) -point.Y);
                }
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Gif);
                base.Response.ClearContent();
                base.Response.ContentType = "image/gif";
                base.Response.BinaryWrite(stream.ToArray());
                graphics.Dispose();
                image.Dispose();
            }
            catch
            {
            }
        }
    }
}

