namespace Hidistro.Core
{
    using Hidistro.Core.Enums;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web;

    public static class ResourcesHelper
    {
        private static FileClass[] accessoryFileClass = new FileClass[] { FileClass.rar, FileClass.txt, FileClass.doc, FileClass.doc, FileClass.doc, FileClass.htm, FileClass.html, FileClass.zip };
        private static FileClass[] flasFileClass;
        private static FileClass[] imageFileClass = new FileClass[] { FileClass.jpg, FileClass.gif, FileClass.bmp, FileClass.png };
        private static FileClass[] mediaFileClass;

        static ResourcesHelper()
        {
            FileClass[] classArray3 = new FileClass[6];
            classArray3[0] = FileClass.wmv;
            classArray3[1] = FileClass.mid;
            classArray3[2] = FileClass.mp3;
            classArray3[4] = FileClass.rmvb;
            classArray3[5] = FileClass.xv;
            mediaFileClass = classArray3;
            flasFileClass = new FileClass[] { FileClass.swf, FileClass.f4v };
        }

        public static bool CheckPostedFile(HttpPostedFile postedFile, string dir = "image")
        {
            if ((postedFile != null) && (postedFile.ContentLength != 0))
            {
                int result = 0;
                int.TryParse(GetFileClassCode(postedFile), out result);
                FileClass[] imageFileClass = ResourcesHelper.imageFileClass;
                string str = dir;
                if (str != null)
                {
                    if (!(str == "image"))
                    {
                        if (str == "file")
                        {
                            imageFileClass = accessoryFileClass;
                        }
                        else if (str == "media")
                        {
                            imageFileClass = mediaFileClass;
                        }
                        else if (str == "flash")
                        {
                            imageFileClass = flasFileClass;
                        }
                    }
                    else
                    {
                        imageFileClass = ResourcesHelper.imageFileClass;
                    }
                }
                foreach (FileClass class2 in imageFileClass)
                {
                    if ((int)class2 == result)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void CreateThumbnail(string sourceFilename, string destFilename, int width, int height)
        {
            Image image = Image.FromFile(sourceFilename);
            if ((image.Width <= width) && (image.Height <= height))
            {
                File.Copy(sourceFilename, destFilename, true);
                image.Dispose();
            }
            else
            {
                int num = image.Width;
                int num2 = image.Height;
                float num3 = ((float) height) / ((float) num2);
                if ((((float) width) / ((float) num)) < num3)
                {
                    num3 = ((float) width) / ((float) num);
                }
                width = (int) (num * num3);
                height = (int) (num2 * num3);
                Image image2 = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(image2);
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(0, 0, num, num2), GraphicsUnit.Pixel);
                EncoderParameters encoderParams = new EncoderParameters();
                EncoderParameter parameter = new EncoderParameter(Encoder.Quality, 100L);
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
                image2.Save(destFilename, encoder, encoderParams);
                encoderParams.Dispose();
                parameter.Dispose();
                image.Dispose();
                image2.Dispose();
                graphics.Dispose();
            }
        }

        public static void DeleteImage(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    Globals.DelImgByFilePath(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + imageUrl));
                }
                catch
                {
                }
            }
        }

        public static string GenerateFilename(string extension)
        {
            return (Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + extension);
        }

        public static string GetFileClassCode(string FileUrl)
        {
            string str = "";
            try
            {
                FileStream input = new FileStream(FileUrl, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(input);
                str = reader.ReadByte().ToString();
                str = str + reader.ReadByte().ToString();
                reader.Close();
                input.Close();
            }
            catch
            {
                return "";
            }
            return str;
        }

        public static string GetFileClassCode(HttpPostedFile postedFile)
        {
            int contentLength = postedFile.ContentLength;
            byte[] buffer = new byte[contentLength];
            postedFile.InputStream.Read(buffer, 0, contentLength);
            MemoryStream input = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(input);
            string str = "";
            try
            {
                str = reader.ReadByte().ToString();
                str = str + reader.ReadByte().ToString();
            }
            catch
            {
            }
            reader.Close();
            input.Close();
            return str;
        }
    }
}

