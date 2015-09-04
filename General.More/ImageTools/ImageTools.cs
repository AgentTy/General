using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace General
{
    public class ImageTools
    {

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

    }
}
