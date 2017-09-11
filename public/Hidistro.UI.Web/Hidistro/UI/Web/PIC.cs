namespace Hidistro.UI.Web
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public class PIC
    {
        private Bitmap _outBmp;

        public MemoryStream AddImageSignPic(Image img, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            Graphics graphics = null;
            Image image = null;
            ImageAttributes imageAttr = null;
            MemoryStream stream = null;
            MemoryStream stream2;
            try
            {
                graphics = Graphics.FromImage(img);
                image = new Bitmap(watermarkFilename);
                imageAttr = new ImageAttributes();
                ColorMap map = new ColorMap {
                    OldColor = Color.FromArgb(0xff, 0, 0xff, 0),
                    NewColor = Color.FromArgb(0, 0, 0, 0)
                };
                ColorMap[] mapArray = new ColorMap[] { map };
                imageAttr.SetRemapTable(mapArray, ColorAdjustType.Bitmap);
                float num = 0.5f;
                if ((watermarkTransparency >= 1) && (watermarkTransparency <= 10))
                {
                    num = ((float) watermarkTransparency) / 10f;
                }
                float[][] numArray3 = new float[5][];
                float[] numArray4 = new float[5];
                numArray4[0] = 1f;
                numArray3[0] = numArray4;
                float[] numArray5 = new float[5];
                numArray5[1] = 1f;
                numArray3[1] = numArray5;
                float[] numArray6 = new float[5];
                numArray6[2] = 1f;
                numArray3[2] = numArray6;
                float[] numArray7 = new float[5];
                numArray7[3] = num;
                numArray3[3] = numArray7;
                float[] numArray8 = new float[5];
                numArray8[4] = 1f;
                numArray3[4] = numArray8;
                float[][] newColorMatrix = numArray3;
                ColorMatrix matrix = new ColorMatrix(newColorMatrix);
                imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                int x = 0;
                int y = 0;
                switch (watermarkStatus)
                {
                    case 1:
                        x = (int) (img.Width * 0.01f);
                        y = (int) (img.Height * 0.01f);
                        break;

                    case 2:
                        x = ((int) (img.Width * 0.5f)) - (image.Width / 2);
                        y = (int) (img.Height * 0.01f);
                        break;

                    case 3:
                        x = ((int) (img.Width * 0.99f)) - image.Width;
                        y = (int) (img.Height * 0.01f);
                        break;

                    case 4:
                        x = (int) (img.Width * 0.01f);
                        y = ((int) (img.Height * 0.5f)) - (image.Height / 2);
                        break;

                    case 5:
                        x = ((int) (img.Width * 0.5f)) - (image.Width / 2);
                        y = ((int) (img.Height * 0.5f)) - (image.Height / 2);
                        break;

                    case 6:
                        x = ((int) (img.Width * 0.99f)) - image.Width;
                        y = ((int) (img.Height * 0.5f)) - (image.Height / 2);
                        break;

                    case 7:
                        x = (int) (img.Width * 0.01f);
                        y = ((int) (img.Height * 0.99f)) - image.Height;
                        break;

                    case 8:
                        x = ((int) (img.Width * 0.5f)) - (image.Width / 2);
                        y = ((int) (img.Height * 0.99f)) - image.Height;
                        break;

                    case 9:
                        x = ((int) (img.Width * 0.99f)) - image.Width;
                        y = ((int) (img.Height * 0.99f)) - image.Height;
                        break;
                }
                graphics.DrawImage(image, new Rectangle(x, y, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                foreach (ImageCodecInfo info2 in imageEncoders)
                {
                    if (info2.MimeType.IndexOf("jpeg") > -1)
                    {
                        encoder = info2;
                    }
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] numArray2 = new long[1];
                if ((quality < 0) || (quality > 100))
                {
                    quality = 80;
                }
                numArray2[0] = quality;
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, numArray2);
                encoderParams.Param[0] = parameter;
                stream = new MemoryStream();
                if (encoder != null)
                {
                    img.Save(stream, encoder, encoderParams);
                }
                stream2 = stream;
            }
            catch
            {
                stream = null;
                stream2 = stream;
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (img != null)
                {
                    img.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
                if (imageAttr != null)
                {
                    imageAttr.Dispose();
                }
            }
            return stream2;
        }

        public void Dispose()
        {
            if (this._outBmp != null)
            {
                this._outBmp.Dispose();
                this._outBmp = null;
            }
        }

        private static Size NewSize(int maxWidth, int maxHeight, int width, int height)
        {
            double num = 0.0;
            double num2 = 0.0;
            double num3 = Convert.ToDouble(width);
            double num4 = Convert.ToDouble(height);
            double num5 = Convert.ToDouble(maxWidth);
            double num6 = Convert.ToDouble(maxHeight);
            if ((num3 < num5) && (num4 < num6))
            {
                num = num3;
                num2 = num4;
            }
            else if ((num3 / num4) > (num5 / num6))
            {
                num = maxWidth;
                num2 = (num * num4) / num3;
            }
            else
            {
                num2 = maxHeight;
                num = (num2 * num3) / num4;
            }
            return new Size(Convert.ToInt32(num), Convert.ToInt32(num2));
        }

        public void SendSmallImage(string fileName, int maxHeight, int maxWidth)
        {
            Image image = null;
            this._outBmp = null;
            Graphics graphics = null;
            try
            {
                image = Image.FromFile(fileName);
                ImageFormat rawFormat = image.RawFormat;
                Size size = NewSize(maxWidth, maxHeight, image.Width, image.Height);
                this._outBmp = new Bitmap(size.Width, size.Height);
                graphics = Graphics.FromImage(this._outBmp);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                if (graphics != null)
                {
                    graphics.Dispose();
                }
            }
            catch
            {
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
            }
        }

        public static void SendSmallImage(string fileName, string newFile, int maxHeight, int maxWidth)
        {
            Image image = null;
            Bitmap bitmap = null;
            Graphics graphics = null;
            try
            {
                image = Image.FromFile(fileName);
                ImageFormat rawFormat = image.RawFormat;
                Size size = NewSize(maxWidth, maxHeight, image.Width, image.Height);
                bitmap = new Bitmap(size.Width, size.Height);
                graphics = Graphics.FromImage(bitmap);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                EncoderParameters encoderParams = new EncoderParameters();
                long[] numArray = new long[] { 100L };
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, numArray);
                encoderParams.Param[0] = parameter;
                ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo encoder = null;
                for (int i = 0; i < imageEncoders.Length; i++)
                {
                    if (imageEncoders[i].FormatDescription.Equals("JPEG"))
                    {
                        encoder = imageEncoders[i];
                        break;
                    }
                }
                if (encoder != null)
                {
                    bitmap.Save(newFile, encoder, encoderParams);
                }
                else
                {
                    bitmap.Save(newFile, rawFormat);
                }
            }
            catch
            {
            }
            finally
            {
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (image != null)
                {
                    image.Dispose();
                }
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }
        }

        public Bitmap OutBMP
        {
            get
            {
                return this._outBmp;
            }
        }
    }
}

