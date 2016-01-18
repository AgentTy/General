using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace General.Drawing
{
    public class Tools
    {

        #region Resize

        #region Percent
        public static Image Resize(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        #endregion

        #region Size
        public static Image Resize(Image imgPhoto, Size objSize)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = objSize.Width;
            int destHeight = objSize.Height;

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight,
                                     PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                    imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        #endregion

        #endregion

        #region SmartResize
        /*
         * I don't need this because Bitmap is just a type of Image
        public static Bitmap SmartResize(Bitmap objImage, Size objMaxSize)
        {
            if (objImage.Width > objMaxSize.Width || objImage.Height > objMaxSize.Height)
            {
                Size objSize;
                int intWidthOverrun = 0;
                int intHeightOverrun = 0;
                if (objImage.Width > objMaxSize.Width)
                    intWidthOverrun = objImage.Width - objMaxSize.Width;
                if (objImage.Height > objMaxSize.Height)
                    intHeightOverrun = objImage.Height - objMaxSize.Height;

                double dblRatio;
                double dblWidthRatio = (double)objMaxSize.Width / (double)objImage.Width;
                double dblHeightRatio = (double)objMaxSize.Height / (double)objImage.Height;
                if (dblWidthRatio < dblHeightRatio)
                    dblRatio = dblWidthRatio;
                else
                    dblRatio = dblHeightRatio;
                objSize = new Size((int)((double)objImage.Width * dblRatio), (int)((double)objImage.Height * dblRatio));

                Bitmap objNewImage = (Bitmap) Resize(objImage, objSize);

                objImage.Dispose();
                return objNewImage;
            }
            else
            {
                return (Bitmap) objImage.Clone();
            }
        }
        */

        public static Image SmartResize(Image objImage, Size objMaxSize)
        {
            if (objImage.Width > objMaxSize.Width || objImage.Height > objMaxSize.Height)
            {
                Size objSize;
                int intWidthOverrun = 0;
                int intHeightOverrun = 0;
                if (objImage.Width > objMaxSize.Width)
                    intWidthOverrun = objImage.Width - objMaxSize.Width;
                if (objImage.Height > objMaxSize.Height)
                    intHeightOverrun = objImage.Height - objMaxSize.Height;

                double dblRatio;
                double dblWidthRatio = (double)objMaxSize.Width / (double)objImage.Width;
                double dblHeightRatio = (double)objMaxSize.Height / (double)objImage.Height;
                if (dblWidthRatio < dblHeightRatio)
                    dblRatio = dblWidthRatio;
                else
                    dblRatio = dblHeightRatio;
                objSize = new Size((int)((double)objImage.Width * dblRatio), (int)((double)objImage.Height * dblRatio));

                Image objNewImage = Resize(objImage, objSize);

                objImage.Dispose();
                return objNewImage;
            }
            else
            {
                return (Image) objImage.Clone();
            }
        }
        #endregion

        #region GetBrush
        public static Brush GetBrush(string strColor)
        {
            try
            {
                if (strColor.Contains("#"))
                {
                    return (Brush)new SolidBrush(ColorTranslator.FromHtml(strColor));
                }
                else
                {
                    return (Brush)new SolidBrush(Color.FromName(strColor));
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region LoadImageFromURL
        public static Image LoadImageFromURL(string strURL)
        {
            //Load an image from the web and display that - if it fails load one from a local resource
            try
            {
                WebRequest request = WebRequest.Create(strURL);
                request.Credentials = CredentialCache.DefaultCredentials;

                Stream source = request.GetResponse().GetResponseStream();
                MemoryStream ms = new MemoryStream();

                byte[] data = new byte[256];
                int c = source.Read(data, 0, data.Length);

                while (c > 0)
                {
                    ms.Write(data, 0, c);
                    c = source.Read(data, 0, data.Length);
                }

                source.Close();
                ms.Position = 0;
                return new Bitmap(ms);

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion

        #region GetBlackAndWhite
        public static ColorMatrix GetBlackAndWhite()
        {
            ColorMatrix mtrx = new ColorMatrix(
               new float[][] {
                 new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                 new float[] { 0.588f, 0.588f, 0.588f, 0, 0}, 
                 new float[] { 0.111f, 0.111f, 0.111f, 0, 0 }, 
                 new float[] { 0, 0, 0, 1, 0 }, 
                 new float[] { 0, 0, 0, 0, 1}, 
                }
            );

            return mtrx;
        }
        #endregion GetBlackAndWhite

        #region GetSepia
        public static ColorMatrix GetSepia()
        {
            ColorMatrix mtrx = new ColorMatrix(
               new float[][] {
                 new float[] { 0.393f, 0.349f, 0.272f, 0, 0 },
                 new float[] { 0.769f, 0.686f, 0.534f, 0, 0}, 
                 new float[] { 0.189f, 0.168f, 0.131f, 0, 0 }, 
                 new float[] { 0, 0, 0, 1, 0 }, 
                 new float[] { 0, 0, 0, 0, 1}, 
                }
            );

            return mtrx;
        }
        #endregion GetSepia

        #region GetNegative
        public static ColorMatrix GetNegative()
        {
            ColorMatrix mtrx = new ColorMatrix(
               new float[][] {
                 new float[] { -1, 0, 0, 0, 0 },
                 new float[] { 0, -1, 0, 0, 0}, 
                 new float[] { 0, 0, -1, 0, 0 }, 
                 new float[] { 0, 0, 0, 1, 0 }, 
                 new float[] { 0, 0, 0, 0, 1}, 
                }
            );

            return mtrx;
        }
        #endregion GetNegative

        #region GetEncoderInfo
        public static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        #endregion

    }
}
