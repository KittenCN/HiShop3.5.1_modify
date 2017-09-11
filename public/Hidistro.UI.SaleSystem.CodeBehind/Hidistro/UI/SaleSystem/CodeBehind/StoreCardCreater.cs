namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using ThoughtWorks.QRCode.Codec;

    public class StoreCardCreater
    {
        private string CodeUrl = "";
        private int ReferralId;
        private JObject resultObj;
        private string SetJson = "";
        private string StoreLogo = "";
        private string StoreName = "";
        private string UserHead = "";
        private int userId;
        private string UserName = "";

        public StoreCardCreater(string _SetJson, string _UserHeadPath, string _StoreLogoPath, string _CodeUrl, string _UserName, string _StoreName, int _ReferralId, int _userid)
        {
            this.SetJson = _SetJson;
            this.UserHead = _UserHeadPath;
            this.StoreLogo = _StoreLogoPath;
            this.CodeUrl = _CodeUrl;
            this.UserName = _UserName;
            this.StoreName = _StoreName;
            this.ReferralId = _ReferralId;
            this.userId = _userid;
        }

        public static Bitmap CombinImage(Bitmap QRimg, Image Logoimg, int logoW)
        {
            Bitmap image = new Bitmap(QRimg.Width + 20, QRimg.Height + 20);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.White);
            graphics.DrawImage(QRimg, 10, 10, QRimg.Width, QRimg.Height);
            graphics.DrawImage(Logoimg, (image.Width - logoW) / 2, (image.Height - logoW) / 2, logoW, logoW);
            return image;
        }

        public bool CreadCard(out string imgUrl)
        {
            Image image;
            bool flag = false;
            if ((this.resultObj == null) || (this.resultObj["BgImg"] == null))
            {
                imgUrl = "掌柜名片模板未设置，无法生成名片！";
                return flag;
            }
            imgUrl = "生成失败";
            Bitmap qRimg = null;
            if (this.CodeUrl.Contains("weixin.qq.com"))
            {
                qRimg = this.getNetImg(this.CodeUrl);
            }
            else
            {
                qRimg = new QRCodeEncoder { QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeScale = 8, QRCodeVersion = 8, QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M }.Encode(this.CodeUrl);
            }
            int num = int.Parse(this.resultObj["DefaultHead"].ToString());
            if (string.IsNullOrEmpty(this.UserHead) || (!this.UserHead.ToLower().StartsWith("http") && !System.IO.File.Exists(Globals.MapPath(this.UserHead))))
            {
                this.UserHead = "/Utility/pics/imgnopic.jpg";
            }
            if (!this.StoreLogo.ToLower().StartsWith("http") && !System.IO.File.Exists(Globals.MapPath(this.StoreLogo)))
            {
                this.StoreLogo = "/Utility/pics/headLogo.jpg";
            }
            switch (num)
            {
                case 2:
                    this.UserHead = "";
                    break;

                case 1:
                    this.UserHead = this.StoreLogo;
                    break;
            }
            if (this.UserHead.ToLower().StartsWith("http"))
            {
                image = this.getNetImg(this.UserHead);
            }
            else if (System.IO.File.Exists(Globals.MapPath(this.UserHead)))
            {
                image = Image.FromFile(Globals.MapPath(this.UserHead));
            }
            else
            {
                image = new Bitmap(100, 100);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(new Rectangle(0, 0, image.Width, image.Width));
            Bitmap bitmap2 = new Bitmap(image.Width, image.Width);
            using (Graphics graphics = Graphics.FromImage(bitmap2))
            {
                graphics.SetClip(path);
                graphics.DrawImage(image, 0, 0, image.Width, image.Width);
            }
            image.Dispose();
            qRimg = CombinImage(qRimg, bitmap2, 80);
            Bitmap bitmap3 = new Bitmap(480, 0x2df);
            Graphics graphics2 = Graphics.FromImage(bitmap3);
            graphics2.SmoothingMode = SmoothingMode.HighQuality;
            graphics2.CompositingQuality = CompositingQuality.HighQuality;
            graphics2.InterpolationMode = InterpolationMode.High;
            graphics2.Clear(Color.White);
            Bitmap b = new Bitmap(100, 100);
            if ((this.resultObj["BgImg"] != null) && System.IO.File.Exists(Globals.MapPath(this.resultObj["BgImg"].ToString())))
            {
                b = (Bitmap) Image.FromFile(Globals.MapPath(this.resultObj["BgImg"].ToString()));
                b = GetThumbnail(b, 0x2df, 480);
            }
            graphics2.DrawImage(b, 0, 0, 480, 0x2df);
            Font font = new Font("微软雅黑", (float) ((((int) this.resultObj["myusernameSize"]) * 6) / 5));
            Font font2 = new Font("微软雅黑", (float) ((((int) this.resultObj["shopnameSize"]) * 6) / 5));
            graphics2.DrawImage(bitmap2, (int) (((decimal) this.resultObj["posList"][0]["left"]) * 480M), (((int) this.resultObj["posList"][0]["top"]) * 0x2df) / 490, (int) (((decimal) this.resultObj["posList"][0]["width"]) * 480M), (int) (((decimal) this.resultObj["posList"][0]["width"]) * 480M));
            StringFormat format = new StringFormat(StringFormatFlags.DisplayFormatControl);
            string str = this.resultObj["myusername"].ToString().Replace("{{昵称}}", "$");
            string str2 = this.resultObj["shopname"].ToString().Replace("{{店铺名称}}", "$");
            string[] strArray = str.Split(new char[] { '$' });
            string[] strArray2 = str2.Split(new char[] { '$' });
            graphics2.DrawString(strArray[0], font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), (float) ((int) (((decimal) this.resultObj["posList"][1]["left"]) * 480M)), (float) ((((int) this.resultObj["posList"][1]["top"]) * 0x2df) / 490), format);
            if (strArray.Length > 1)
            {
                SizeF ef = graphics2.MeasureString(" ", font);
                SizeF ef2 = graphics2.MeasureString(strArray[0], font);
                graphics2.DrawString(this.UserName, font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["nickNameColor"].ToString())), (((int) (((decimal) this.resultObj["posList"][1]["left"]) * 480M)) + ef2.Width) - ef.Width, (float) ((((int) this.resultObj["posList"][1]["top"]) * 0x2df) / 490), format);
                SizeF ef3 = graphics2.MeasureString(this.UserName, font);
                graphics2.DrawString(strArray[1], font, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["myusernameColor"].ToString())), ((((int) (((decimal) this.resultObj["posList"][1]["left"]) * 480M)) + ef2.Width) - (ef.Width * 2f)) + ef3.Width, (float) ((((int) this.resultObj["posList"][1]["top"]) * 0x2df) / 490), format);
            }
            graphics2.DrawString(strArray2[0], font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), (float) ((int) (((decimal) this.resultObj["posList"][2]["left"]) * 480M)), (float) ((((int) this.resultObj["posList"][2]["top"]) * 0x2df) / 490));
            if (strArray2.Length > 1)
            {
                SizeF ef4 = graphics2.MeasureString(" ", font2);
                SizeF ef5 = graphics2.MeasureString(strArray2[0], font2);
                graphics2.DrawString(this.StoreName, font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["storeNameColor"].ToString())), (((int) (((decimal) this.resultObj["posList"][2]["left"]) * 480M)) + ef5.Width) - ef4.Width, (float) ((((int) this.resultObj["posList"][2]["top"]) * 0x2df) / 490), format);
                SizeF ef6 = graphics2.MeasureString(this.StoreName, font2);
                graphics2.DrawString(strArray2[1], font2, new SolidBrush(ColorTranslator.FromHtml(this.resultObj["shopnameColor"].ToString())), ((((int) (((decimal) this.resultObj["posList"][2]["left"]) * 480M)) + ef5.Width) - (ef4.Width * 2f)) + ef6.Width, (float) ((((int) this.resultObj["posList"][2]["top"]) * 0x2df) / 490), format);
            }
            graphics2.DrawImage(qRimg, (int) (((decimal) this.resultObj["posList"][3]["left"]) * 480M), (((int) this.resultObj["posList"][3]["top"]) * 0x2df) / 490, (int) (((decimal) this.resultObj["posList"][3]["width"]) * 480M), (int) (((decimal) this.resultObj["posList"][3]["width"]) * 480M));
            qRimg.Dispose();
            if (this.ReferralId == 0)
            {
                bitmap3.Save(Globals.MapPath(string.Concat(new object[] { Globals.GetStoragePath(), "/DistributorCards/MemberCard", this.userId, ".jpg" })), ImageFormat.Jpeg);
                imgUrl = string.Concat(new object[] { Globals.GetStoragePath(), "/DistributorCards/MemberCard", this.userId, ".jpg" });
            }
            else
            {
                bitmap3.Save(Globals.MapPath(string.Concat(new object[] { Globals.GetStoragePath(), "/DistributorCards/StoreCard", this.ReferralId, ".jpg" })), ImageFormat.Jpeg);
                imgUrl = string.Concat(new object[] { Globals.GetStoragePath(), "/DistributorCards/StoreCard", this.ReferralId, ".jpg" });
            }
            bitmap3.Dispose();
            return true;
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

        private Bitmap getNetImg(string imgUrl)
        {
            try
            {
                Random random = new Random();
                if (imgUrl.Contains("?"))
                {
                    imgUrl = imgUrl + "&aid=" + random.NextDouble();
                }
                else
                {
                    imgUrl = imgUrl + "?aid=" + random.NextDouble();
                }
                Stream responseStream = WebRequest.Create(imgUrl).GetResponse().GetResponseStream();
                Image image = Image.FromStream(responseStream);
                responseStream.Close();
                responseStream.Dispose();
                return (Bitmap) image;
            }
            catch (Exception exception)
            {
                Globals.Debuglog("1:" + exception.ToString(), "_Debuglogttt.txt");
                return new Bitmap(100, 100);
            }
        }

        private string getSpaceFill(string ystring)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ystring.Length; i++)
            {
                if (ystring[i] > '\x007f')
                {
                    builder.Append("　");
                }
                else
                {
                    builder.Append(" ");
                }
            }
            return builder.ToString();
        }

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
            EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, numArray);
            parameters.Param[0] = parameter;
            image.Dispose();
            return bitmap;
        }

        public bool ReadJson()
        {
            this.resultObj = JsonConvert.DeserializeObject(this.SetJson) as JObject;
            bool flag = false;
            if ((((this.resultObj != null) && (this.resultObj["writeDate"] != null)) && ((this.resultObj["posList"] != null) && (this.resultObj["DefaultHead"] != null))) && ((this.resultObj["myusername"] != null) && (this.resultObj["shopname"] != null)))
            {
                flag = true;
            }
            return flag;
        }
    }
}

