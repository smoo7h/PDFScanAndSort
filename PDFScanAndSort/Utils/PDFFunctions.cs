using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PDFScanAndSort.Utils
{
    class PDFFunctions
    {
        public static Dictionary<int, string> PDFToImage(string file, string outputPath, int dpi)
        {


            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Ghostscript.NET.Rasterizer.GhostscriptRasterizer rasterizer = null;
            Ghostscript.NET.GhostscriptVersionInfo vesion = new Ghostscript.NET.GhostscriptVersionInfo(new System.Version(0, 0, 0), path + @"\gsdll64.dll", string.Empty, Ghostscript.NET.GhostscriptLicense.GPL);

            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            using (rasterizer = new Ghostscript.NET.Rasterizer.GhostscriptRasterizer())
            {
                rasterizer.Open(file, vesion, false);

                for (int i = 1; i <= rasterizer.PageCount; i++)
                {
                    string pageFilePath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(@file).Replace(".pdf", "") + "-p" + i.ToString() + ".tiff");

                    using (System.Drawing.Image img = rasterizer.GetPage(dpi, dpi, i))
                    {
                        
                            if (File.Exists(pageFilePath))
                            {
                              

                                //img.Save(pageFilePath, System.Drawing.Imaging.ImageFormat.Tiff);
                                dictionary.Add(i, pageFilePath);

                            }
                            else
                            {
                                img.Save(pageFilePath, System.Drawing.Imaging.ImageFormat.Tiff);
                                dictionary.Add(i, pageFilePath);
                                
                            }
                       
                            
                      
                    }
                    
                }
                rasterizer.Close();
                rasterizer.Dispose();

             
            }
            return dictionary;
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public static string imageToText(string tiffPath)
        {
            string text = "";
            int stop = 0;
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(tiffPath))
                    {
                        var i = 1;
                        using (var page = engine.Process(img))
                        {
                            text = page.GetText();
                            Debug.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();
                                do
                                {
                                    if (i % 2 == 0)
                                    {
                                        do
                                        {
                                            text = text + iter.GetText(PageIteratorLevel.Word);
                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));
                                    }
                                    i++;
                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));

                                iter.Dispose();
                            }
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unexpected Error: " + e.Message);
                Debug.WriteLine("Details: ");
                Debug.WriteLine(e.ToString());
            }
            
            return text;
        }

        public static List<Dictionary<int, string>> createTiffFiles(string path)
        {
            //Declare pdf
            string pdfPath = path;

            //Declare .tiff outputs
            string outputHighRes = "";
            string outputLowRes = "";

            //Get pdf file name for directory name
            string pdfName = Path.GetFileName(pdfPath);
            //Path to temp
            string @tempPath = Path.GetTempPath() + "ScannedPDFs\\" + pdfName;

            //Create PDF specific directory(in project folder)
            if (!Directory.Exists(tempPath + pdfName))
            {
                Directory.CreateDirectory(tempPath + pdfName);
            }

            //Create high-res .tiff specific directory and declare high res folder(in pdf folder)
            if (!Directory.Exists(tempPath + "\\highRes\\"))
            {
                Directory.CreateDirectory(tempPath + "\\highRes\\");
                outputHighRes = tempPath + "\\highRes\\";
            }
            else
            {
                outputHighRes = tempPath + "\\highRes\\";
            }

            //Create low-res .tiff specific directory and declare low res folder(in pdf folder)
            if (!Directory.Exists(tempPath + "\\lowRes\\"))
            {
                Directory.CreateDirectory(tempPath + "\\lowRes\\");
                outputLowRes = tempPath + "\\lowRes\\";
            }
            else
            {
                outputLowRes = tempPath + "\\lowRes\\";
            }

            //Create high res tiffs
            Dictionary<int, string> highResTiffLocations = new Dictionary<int, string>();
            highResTiffLocations = PDFFunctions.PDFToImage(pdfPath, outputHighRes, 300);

            //Create low res tiffs
            Dictionary<int, string> lowResTiffLocations = new Dictionary<int, string>();
            lowResTiffLocations = PDFFunctions.PDFToImage(pdfPath, outputLowRes, 100);

            List<Dictionary<int, string>> dictionaries = new List<Dictionary<int, string>>();
            dictionaries.Add(highResTiffLocations);
            dictionaries.Add(lowResTiffLocations);

            return dictionaries;


        }
    }
}
