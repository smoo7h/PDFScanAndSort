using System;
using System.Collections.Generic;
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


        public string Name { get; set; }

        public List<Page> Pages { get; set; }

        public string PDFLocation { get; set; }

      //  public List<Card> Cards { get; set; }


        public void tiffToPDF(string resultPDF)
        {
            // creation of the document with a certain size and certain margins  
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 0, 0, 0, 0);

            // creation of the different writers  
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(resultPDF, System.IO.FileMode.Create));
            
            document.Open();
            foreach (Page page in Pages)
            {
                if (page.Card.ImageLocationLR != null)
                {
                    // load the tiff image and count the total pages  
                    System.Drawing.Bitmap bm = new System.Drawing.Bitmap(page.Card.ImageLocationLR);
                    int total = bm.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    for (int k = 0; k < total; ++k)
                    {
                        bm.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, k);
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(bm, System.Drawing.Imaging.ImageFormat.Bmp);
                        // scale the image to fit in the page  
                        img.ScalePercent(72f / img.DpiX * 100);
                        img.SetAbsolutePosition(0, 0);
                        cb.AddImage(img);
                        document.NewPage();
                    } 
                }
            }


            document.Close();
            document.Dispose();
            writer.Dispose();
        }


    }
}
