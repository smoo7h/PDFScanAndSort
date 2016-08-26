using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Application
    {
        public Application()
        {
          //  this.Cards = new List<Card>();

            this.Pages = new List<Page>();

        }

        public string ServerLink { get; set; }

        public string Name { get; set; }

        public List<Page> Pages { get; set; }

        public string PDFLocation { get; set; }

        public int NumberOfPages { get; set; }

      //  public List<Card> Cards { get; set; }


        public void tiffToPDF(string resultPDF)
        {
            // creation of the document with a certain size and certain margins  
            using (iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0))
            {
                using (FileStream fs = new System.IO.FileStream(resultPDF, System.IO.FileMode.Create))
                {
                    // creation of the different writers  
                    using (iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs))
                    {

                        document.Open();
                        foreach (Page page in Pages)
                        {
                            if (page.Card.ImageLocationLR != null)
                            {
                                // load the tiff image and count the total pages  

                                int total = 0;
                                using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(page.Card.ImageLocationLR))
                                {
                                     total = bm2.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
                                    // bm2 = null;
                                    bm2.Dispose();
                                }
                                   



                                    for (int k = 0; k < total; ++k)
                                    {
                                        System.Drawing.Bitmap bm = new System.Drawing.Bitmap(page.Card.ImageLocationLR);
                                        iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;

                                        bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);


                                        img.ScaleToFitHeight = false;

                                        img.ScalePercent(70f / img.DpiX * 100);

                                        // scale the image to fit in the page  
                                       img.SetAbsolutePosition(-22, 25);


                                        cb.AddImage(img);

                                        document.NewPage();

                                        img = null;
                                        
                                        cb.ClosePath();
                                        bm.Dispose();
                                    }

                                   
                                  
                                
                            }
                        }

                      

                        document.Close();
                        document.Dispose();
                        writer.Dispose();
                       
                        fs.Close();
                        fs.Dispose();

                    }
                }
            }
        }


    }
}
