using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;

namespace General.Drawing
{
    public class Rendering
    {

        #region GetImage
        public static ImageHandler GetImage(int intWidth, int intHeight, float fltDPI)
        {
            return GetImage(intWidth, intHeight, fltDPI, Brushes.White);
        }

        public static ImageHandler GetImage(int intWidth, int intHeight, float fltDPI, Brush objBackgroundColor)
        {

            //Create Stage
            Bitmap objImage = new Bitmap(intWidth, intHeight);
            objImage.SetResolution(fltDPI, fltDPI);
            Graphics objStage = System.Drawing.Graphics.FromImage(objImage);

            //Define Bounds
            Rectangle objBounds = new Rectangle(0, 0, intWidth, intHeight);

            //Draw Background Color
            objStage.FillRectangle(objBackgroundColor, objBounds);

            //Return ImageHandler
            ImageHandler objImageHandler = new ImageHandler();
            objImageHandler.Image = objImage;
            objImageHandler.Stage = objStage;
            objImageHandler.Bounds = objBounds;
            return objImageHandler;

        }
        #endregion

        #region SaveJPEG
        public static void SaveJPEG(ImageHandler objImageHandler, string strFileName)
        {
            //Set Output Type
            ImageCodecInfo objImageCodecInfo;
            objImageCodecInfo = Tools.GetEncoderInfo("image/jpeg");
            
            //Save
            objImageHandler.Image.Save(strFileName, objImageCodecInfo, null);
        }

        public static void SaveJPEG(ImageHandler objImageHandler, string strFileName, int intSizePercent)
        {
            //Set Output Type
            ImageCodecInfo objImageCodecInfo;
            objImageCodecInfo = Tools.GetEncoderInfo("image/jpeg");

            Image objNewImage = Tools.Resize(objImageHandler.Image, intSizePercent);

            //Save
            objNewImage.Save(strFileName, objImageCodecInfo, null);
        }
        #endregion

        #region SavePDF
        public static string SavePDF(string strInputFile, string strOutputFile)
        {
            iTextSharp.text.Document doc = null;
            try
            {
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(strInputFile);
                iTextSharp.text.Rectangle rectDocSize = new iTextSharp.text.Rectangle(img.Width, img.Height);
                doc = new iTextSharp.text.Document(rectDocSize);

                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(strOutputFile, FileMode.Create));
                doc.Open();
                //doc.Add(new iTextSharp.text.Paragraph("GIF"));
                doc.Add(img);
            }
            catch (iTextSharp.text.DocumentException dex)
            {
                throw dex;
            }
            catch (IOException ioex)
            {
                throw ioex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(doc != null)
                    doc.Close();
            }
            return strOutputFile;            
        }
        #endregion

    }
}
