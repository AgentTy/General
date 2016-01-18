using System;
using General;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Data;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Threading;
using iTextSharp;
using iTextSharp.text;
using General.Model;

namespace General.Drawing.Labels
{

    /// <summary>
    /// Ambassador Address Label � Avery 5160
    /// </summary>
    public class Avery5160
    {

        #region Constants for Avery Address Label 5160
        //Constants for Avery Address Label 5160
        private const double PAPER_SIZE_WIDTH_96 = 816; //8.5" x 96
        private const double PAPER_SIZE_HEIGHT_96 = 1056; //11" x 96
        private const double PAPER_SIZE_WIDTH_72 = 612; //8.5" x 72
        private const double PAPER_SIZE_HEIGHT_72 = 792; //11" x 72

        private const double LABEL_WIDTH_96 = 252; //2.625" x 96
        private const double LABEL_WIDTH_72 = 189; //2.625" x 72
        private const double LABEL_HEIGHT_96 = 96; //1" x 96
        private const double LABEL_HEIGHT_72 = 72; //1" x 72

        private const double SIDE_MARGIN_96 = 18.24; //0.19" x 96
        private const double SIDE_MARGIN_72 = 13.68; //0.19" x 72
        private const double TOP_MARGIN_96 = 48; //0.5" x 96
        private const double TOP_MARGIN_72 = 36; //0.5" x 72
        private const double HORIZONTAL_GAP_96 = 12.48; //0.13" x 96
        private const double HORIZONTAL_GAP_72 = 9.36; //0.13" x 72

        private const double LABELS_PER_SHEET = 30; //3 columns of 10 labels
        #endregion

        #region Private Document Prep Functions
        private FixedPage CreatePageXPS()
        {
            //Create new page
            FixedPage page = new FixedPage();
            //Set background
            page.Background = Brushes.White;
            //Set page size (Letter size)
            page.Width = PAPER_SIZE_WIDTH_96;
            page.Height = PAPER_SIZE_HEIGHT_96;
            return page;
        }

        private iTextSharp.text.Document CreatePagePDF(string strFilePath,iTextSharp.text.Rectangle rectPaperSize, float marginLeft, float marginRight, float marginTop, float marginBottom)
        {
            iTextSharp.text.Document objPDFDoc = new iTextSharp.text.Document(rectPaperSize,marginLeft,marginRight,marginTop,marginBottom);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(objPDFDoc,
                         new FileStream(strFilePath, FileMode.Create));
            objPDFDoc.Open();
            return objPDFDoc;
        }
        #endregion

        #region Public CreateDocument
        private System.Collections.ArrayList objData;
        private string _strFilePathXPS;
        private string _strFilePathPDF;
        public void CreateDocument(System.Collections.ArrayList data, string strFilePathXPS, string strFilePathPDF)
        {
            objData = data;
            _strFilePathXPS = strFilePathXPS;
            _strFilePathPDF = strFilePathPDF;
            Thread thread = new Thread(CreateDocument_STAThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
        #endregion

        private void CreateDocument_STAThread()
        {

            #region Prep
            System.Collections.ArrayList data = objData;
            //Create new document
            FixedDocument doc = new FixedDocument();
            //Set page size
            doc.DocumentPaginator.PageSize = new Size(PAPER_SIZE_WIDTH_96, PAPER_SIZE_HEIGHT_96);

            //Number of records
            double count = (double)data.Count;
            #endregion

            if (count > 0)
            {

                #region Declare Variables
                AveryBarcodeLabel label;

                //Determine number of pages to generate
                double pageCount = Math.Ceiling(count / LABELS_PER_SHEET);

                int dataIndex = 0;
                int currentColumn = 0;
                int currentPDFColumn = 0;
                int currentRow = 0;

                iTextSharp.text.pdf.PdfPTable objPDFTable = null;
                #endregion

                #region Open PDF Document
                iTextSharp.text.Rectangle rectPaperSize = new iTextSharp.text.Rectangle((float)PAPER_SIZE_WIDTH_72, (float)PAPER_SIZE_HEIGHT_72);
                iTextSharp.text.Document objPDFDoc = CreatePagePDF(_strFilePathPDF, rectPaperSize, (float)SIDE_MARGIN_72, (float)SIDE_MARGIN_72, (float)(TOP_MARGIN_72), 0);
                //objPDFDoc.
                #endregion

                #region Define PDF Column Widths
                //Define PDF Column Widths
                float[] columnWidth = new float[5];
                columnWidth[0] = (float)LABEL_WIDTH_72;
                columnWidth[1] = (float)HORIZONTAL_GAP_72;
                columnWidth[2] = (float)LABEL_WIDTH_72;
                columnWidth[3] = (float)HORIZONTAL_GAP_72;
                columnWidth[4] = (float)LABEL_WIDTH_72;
                #endregion

                for (int i = 0; i < pageCount; i++)
                {

                    #region Prep XPS Page
                    //Create page
                    PageContent page = new PageContent();
                    FixedPage fixedPage = this.CreatePageXPS();
                    #endregion

                    //Create labels
                    for (int j = 0; j < 30; j++)
                    {

                        #region Set currentRow
                        if (j % 10 == 0)
                        {
                            currentRow = 0;
                        }
                        else
                        {
                            currentRow++;
                        }
                        #endregion

                        #region Set curentColumn (Vertically)
                        if (j < 10)
                        {
                            currentColumn = 0;
                        }
                        else if (j > 19)
                        {
                            currentColumn = 2;
                        }
                        else
                        {
                            currentColumn = 1;
                        }
                        #endregion

                        #region Set currentPDFColumn (Horizontally)
                        if (j % 3 == 0)
                        {
                            currentPDFColumn = 0;
                        }
                        else if (j%3 == 1)
                        {
                            currentPDFColumn = 1;
                        }
                        else if (j % 3 == 2)
                        {
                            currentPDFColumn = 2;
                        }
                        #endregion

                        if (dataIndex < count)
                        {

                            #region Start a New Page When Necessary
                            if (dataIndex % 30 == 0 || dataIndex == 0)
                            {
                                if (objPDFTable != null)
                                {
                                    objPDFDoc.Add(objPDFTable);
                                    objPDFDoc.NewPage();
                                }
                                objPDFTable = new iTextSharp.text.pdf.PdfPTable(5);
                                objPDFTable.SetTotalWidth(columnWidth);
                                objPDFTable.LockedWidth = true;
                                //objPDFTable.SplitLate = false;
                                objPDFTable.SkipLastFooter = true;
                                objPDFTable.KeepTogether = true;
                                //objPDFTable.ExtendLastRow = true;
                            }
                            #endregion

                            #region Get Data and Fill Lines
                            label = new AveryBarcodeLabel();

                            if (data[dataIndex].GetType() == typeof(AveryBarcodeLabel))
                            {
                                label = (AveryBarcodeLabel)data[dataIndex];
                            }
                            else if (data[dataIndex].GetType() == typeof(AveryLabelDataModel))
                            {
                                label = new AveryBarcodeLabel((AveryLabelDataModel)data[dataIndex]);
                            }
                            else if(data[dataIndex].GetType() == typeof(PostalAddress))
                            {
                                PostalAddress objAddr = (PostalAddress)data[dataIndex];
                                label.Line1 = objAddr.Reference;
                                label.Line2 = objAddr.Address1;
                                if (!StringFunctions.IsNullOrWhiteSpace(objAddr.Address2))
                                    label.Line2 += " " + objAddr.Address2;
                                if (!StringFunctions.IsNullOrWhiteSpace(objAddr.Address3))
                                    label.Line2 += " " + objAddr.Address3;
                                label.Line3 = objAddr.ToLocationString() + " " + objAddr.PostalCode;
                            }
                            else if (data[dataIndex].GetType() == typeof(AddressBookEntry))
                            {
                                AddressBookEntry objAddr = (AddressBookEntry)data[dataIndex];
                                label.Line1 = objAddr.FullName;
                                label.Line2 = objAddr.Address1;
                                if (!StringFunctions.IsNullOrWhiteSpace(objAddr.Address2))
                                    label.Line2 += " " + objAddr.Address2;
                                if (!StringFunctions.IsNullOrWhiteSpace(objAddr.Address3))
                                    label.Line2 += " " + objAddr.Address3;
                                label.Line3 = objAddr.ToLocationString() + " " + objAddr.PostalCode;
                                label.Email = objAddr.Email;
                                label.Phone = objAddr.Phone;
                            }

                            /*
                         line1 = (string)data.Rows[dataIndex]["Name"];
                         line2 = (string)data.Rows[dataIndex]["Address"];
                         postalCode = (string)data.Rows[dataIndex]["PostalCode"];
                         line3 = (string)data.Rows[dataIndex]["City"] + " " + (string)data.Rows[dataIndex]["State"] + " " + postalCode;
                         */
                            #endregion

                            #region Draw Label Cell in PDF
                            //Create individual label
                            iTextSharp.text.pdf.PdfPTable tblCell;

                            if ((label.Phone != null && label.Phone.Valid) || (label.Email != null && label.Email.Valid) || (label.Date != null && label.Date != DateTime.MinValue && label.Date != DateTime.MaxValue))
                            {
                                #region Label With Phone/Email/Date
                                Font objFont = new Font();
                                objFont.Size = Font.DEFAULTSIZE - 4;

                                tblCell = new iTextSharp.text.pdf.PdfPTable(2);

                                if (label.Date != null && label.Date != DateTime.MinValue && label.Date != DateTime.MaxValue)
                                {
                                    iTextSharp.text.pdf.PdfPCell objLine1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line1, objFont));
                                    objLine1.Border = 0;
                                    tblCell.AddCell(objLine1);

                                    iTextSharp.text.pdf.PdfPCell objDate = new iTextSharp.text.pdf.PdfPCell(new Phrase("wd: " + label.Date.ToShortDateString(), objFont));
                                    objDate.HorizontalAlignment = 2; //Right
                                    objDate.Border = 0;
                                    tblCell.AddCell(objDate);
                                }
                                else
                                {
                                    iTextSharp.text.pdf.PdfPCell objLine1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line1, objFont));
                                    objLine1.Border = 0;
                                    objLine1.Colspan = 2;
                                    tblCell.AddCell(objLine1);
                                }

                                iTextSharp.text.pdf.PdfPCell objLine2 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line2, objFont));
                                objLine2.Border = 0;
                                objLine2.Colspan = 2;
                                tblCell.AddCell(objLine2);

                                if (label.Phone != null && label.Phone.Valid)
                                {
                                    iTextSharp.text.pdf.PdfPCell objLine3 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line3, objFont));
                                    objLine3.Border = 0;
                                    objLine3.NoWrap = true;
                                    tblCell.AddCell(objLine3);

                                    iTextSharp.text.pdf.PdfPCell objPhone = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Phone.ToString(), objFont));
                                    objPhone.HorizontalAlignment = 2; //Right
                                    objPhone.Border = 0;
                                    tblCell.AddCell(objPhone);
                                }
                                else
                                {
                                    iTextSharp.text.pdf.PdfPCell objLine3 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line3, objFont));
                                    objLine3.Border = 0;
                                    objLine3.Colspan = 2;
                                    tblCell.AddCell(objLine3);
                                }
                                
                                if (label.Email != null && label.Email.Valid)
                                {
                                    iTextSharp.text.pdf.PdfPCell objEmail = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Email.ToString(), objFont));
                                    objEmail.HorizontalAlignment = 2; //Right
                                    objEmail.Border = 0;
                                    objEmail.Colspan = 2;
                                    tblCell.AddCell(objEmail);
                                }
                                
                                #endregion
                            }
                            else
                            {
                                #region Standard Address Label
                                tblCell = new iTextSharp.text.pdf.PdfPTable(1);

                                Font objNameFont = new Font();
                                objNameFont.Size = Font.DEFAULTSIZE - 1;
                                iTextSharp.text.pdf.PdfPCell objLine1 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line1, objNameFont));
                                objLine1.Border = 0;
                                tblCell.AddCell(objLine1);

                                Font objAddrFont = new Font();
                                objAddrFont.Size = Font.DEFAULTSIZE - 3;
                                iTextSharp.text.pdf.PdfPCell objLine2 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line2, objAddrFont));
                                objLine2.Border = 0;
                                tblCell.AddCell(objLine2);

                                if (!StringFunctions.IsNullOrWhiteSpace(label.Line3))
                                {
                                    iTextSharp.text.pdf.PdfPCell objLine3 = new iTextSharp.text.pdf.PdfPCell(new Phrase(label.Line3, objAddrFont));
                                    objLine3.Border = 0;
                                    tblCell.AddCell(objLine3);
                                }

                                #endregion
                            }
                            iTextSharp.text.pdf.PdfPCell pCell = new iTextSharp.text.pdf.PdfPCell(tblCell);
                            pCell.FixedHeight = (float)(LABEL_HEIGHT_72);
                            pCell.Padding = 5;
                            pCell.Border = 0;
                            objPDFTable.AddCell(pCell);
                            #endregion

                            #region Add Spacer Cell
                            iTextSharp.text.pdf.PdfPCell objGap = new iTextSharp.text.pdf.PdfPCell();
                            objGap.Border = 0;

                            if (currentPDFColumn == 0)
                            {
                                objPDFTable.AddCell(objGap);
                            }
                            else if (currentPDFColumn == 1)
                            {
                                objPDFTable.AddCell(objGap);
                            }
                            #endregion

                            #region Set XPS Position and Add to Document
                            //Set label location
                            if (currentColumn == 0)
                            {
                                FixedPage.SetLeft(label, SIDE_MARGIN_96);
                            }
                            else if (currentColumn == 1)
                            {
                                FixedPage.SetLeft(label, SIDE_MARGIN_96 + LABEL_WIDTH_96 + HORIZONTAL_GAP_96);
                            }
                            else
                            {
                                FixedPage.SetLeft(label, SIDE_MARGIN_96 + LABEL_WIDTH_96 * 2 + HORIZONTAL_GAP_96 * 2);
                            }
                            FixedPage.SetTop(label, TOP_MARGIN_96 + currentRow * LABEL_HEIGHT_96);

                            //Add label object to page
                            fixedPage.Children.Add(label);
                            #endregion

                            #region Finish Last PDF Row By Adding Blanks
                            if (dataIndex == count - 1) //If I'm on the last cell
                            {
                                if (currentPDFColumn != 3)
                                {
                                    while (currentPDFColumn != 3)
                                    {
                                        currentPDFColumn++;
                                        objPDFTable.AddCell(objGap);
                                    }
                                }
                            }
                            #endregion

                            dataIndex++;
                        }
                    }

                    #region Finalize XPS Page
                    //Invoke Measure(), Arrange() and UpdateLayout() for drawing
                    fixedPage.Measure(new Size(PAPER_SIZE_WIDTH_96, PAPER_SIZE_HEIGHT_96));
                    fixedPage.Arrange(new Rect(new Point(), new Size(PAPER_SIZE_WIDTH_96, PAPER_SIZE_HEIGHT_96)));
                    fixedPage.UpdateLayout();
                    ((IAddChild)page).AddChild(fixedPage);
                    doc.Pages.Add(page);
                    #endregion

                }

                #region Finalize PDF Document
                objPDFDoc.Add(objPDFTable);
                objPDFDoc.Close();
                #endregion

            }

            #region Write XPS Document
            if (!StringFunctions.IsNullOrWhiteSpace(_strFilePathXPS))
            {
                if (File.Exists(_strFilePathXPS))
                    File.Delete(_strFilePathXPS);
                XpsDocument xpsd = new XpsDocument(_strFilePathXPS, FileAccess.ReadWrite);
                System.Windows.Xps.XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(doc);
                xpsd.Close();
            }
            #endregion

        }

    }
}
