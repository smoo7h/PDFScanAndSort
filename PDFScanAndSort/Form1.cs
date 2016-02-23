using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using GIBS.Module.Models.Programs.HAP;
using PDFScanAndSort.Models;
using PDFScanAndSort.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFScanAndSort
{
    public partial class Form1 : Form
    {
        public Card currSelectedImg;

        public Card currentCard;

     //   public Card currSelectedCard;

        public List<Models.Card> cards;

        public List<Models.Record> records;

        public List<Models.Application> applications;

        //Current folder directory containing PDFs
        public string ExcelFolder = "";
        //Current selected pdf in excel folder directory
        public int CurrentDocNum = 0;
        //List of excel sheets
        public FileInfo[] CurrentDoc;



        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PDFScanAndSort\\Configs\\";
            //Check if a config txt file exists, if not create one  
            if (!Directory.Exists(@path) || !File.Exists(path + "ExcelFolder.txt"))
            {
                Directory.CreateDirectory(@path);
                TextWriter tw = new StreamWriter(path + "ExcelFolder.txt", false);
                tw.Dispose();
                tw.Close();
            }
            else
            {
                //If config txt file exists, read txt file  and set ExcelFolder string with path
                System.IO.StreamReader myFile = new System.IO.StreamReader(@path + "\\ExcelFolder.txt");
                ExcelFolder = myFile.ReadLine();
             
                myFile.Close();
            }


          

            //initalize list DB view
            InitializeDataBaseListView();

            cards = new List<Models.Card>();
            records = GridHelper.GetRecords();
            applications = new List<Models.Application>();
            RefreshApplicationGUI();

        
        }

        public void onLoad()
        {
            
            clearAllCards();
            cards.Clear();
            records.Clear();
            applications.Clear();
            cards = new List<Models.Card>();
            records = GridHelper.GetRecords();
            applications = new List<Models.Application>();

            RefreshApplicationGUI();
        }

        private void RefreshApplicationGUI()
        {

            var groupedAppList = records
            .GroupBy(u => u.Application)
            .Select(grp => grp.ToList())
            .ToList();

            foreach (var item in groupedAppList)
            {
                GroupControl gc = new GroupControl();
                gc.Text = item[0].Application;
                gc.Dock = DockStyle.Top;
                gc.Width = 446;
                gc.Height = 165;
                xtraScrollableControl1.Controls.Add(gc);


                FlowLayoutPanel panelLong = new FlowLayoutPanel();
                panelLong.Width = 442;
                panelLong.Height = 142;
                panelLong.AutoSize = false;
                panelLong.AutoScroll = true;
                panelLong.WrapContents = false;
                panelLong.Dock = DockStyle.Top;
                panelLong.HorizontalScroll.Value = 0;
                gc.Controls.Add(panelLong);

                //create application class

                PDFScanAndSort.Models.Application app = new PDFScanAndSort.Models.Application();
                app.Name = item[0].Application;
                applications.Add(app);
               

                int i = 1;
                foreach (var rr in item)
                {
                    FlowLayoutPanel pictureContainer = new FlowLayoutPanel();
                    pictureContainer.Width = 85;
                    pictureContainer.Height = 111;
                    pictureContainer.AutoSize = false;
                    pictureContainer.AutoScroll = false;
                    pictureContainer.BorderStyle = BorderStyle.FixedSingle;
                    panelLong.Controls.Add(pictureContainer);

                    Card picture = new Card();
                    picture.Width = 76;
                    picture.Height = 75;
                    picture.AutoSize = false;
                    //picture.Image = Bitmap.FromFile(item);
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureContainer.Controls.Add(picture);



                    picture.Visible = true;
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.DragEnter += picture_DragEnter;
                    picture.DragDrop += picture_DragDrop;
                    picture.MouseDown += picture_MouseDown;

                    picture.AllowDrop = true;


                    //CheckBox checkbox = new CheckBox();
                    //checkbox.AutoSize = false;
                    //checkbox.Width = 76;
                    //checkbox.Height = 22;
                    //checkbox.Text = "Page: " + i;
                    //pictureContainer.Controls.Add(checkbox);


                    Page page = new Page();
                    page.Card = picture;
                    page.SearchStrings = rr.SearchTermStringList;
                    picture.Page = page;
                    picture.Page.PageNumber = i;
                    cards.Add(picture);
                    
                    //  card.PageNumber = i;
                    page.Application = app;
                    // page.PictureBox = picture;
                    app.Pages.Add(page);

                    i++;

                    


                }


            }
        }

        void Form1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {      
            //display data into text box summery when focused row is changed 

            txtFastNumber.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST").ToString() : "";

            txtFirstName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName").ToString() : "";

            txtLastName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName").ToString() : "";

            txtStreetAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1").ToString() : "";

            txtCity.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name").ToString() : "";

            txtLDC.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("LDC.Name") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("LDC.Name").ToString() : "";

            txtAppType.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("ApplicationType") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("ApplicationType").ToString() : "";

        }

        public void InitializeDataBaseListView()
        {
            XPODataHelper datahelper = new XPODataHelper();

            IObjectSpace space = datahelper.Connect();

            // Create a Session object. 
            Session session1 = ((XPObjectSpace)space).Session;
            // Create an XPClassInfo object corresponding to the Person class. 
            XPClassInfo classInfo = session1.GetClassInfo(typeof(GIBS.Module.Models.Programs.HAP.HAPApplication));
            // Create an XPServerCollectionSource object. 
            XPServerCollectionSource xpServerCollectionSource1 = new XPServerCollectionSource(session1, classInfo);

            gridControl1.Dock = DockStyle.Fill;

            // Bind the grid control to the data source. 
            gridControl1.DataSource = xpServerCollectionSource1;

            gridControl1.MainView.HideEditor();

            //clear the columns
            ((ColumnView)gridControl1.Views[0]).Columns.Clear();

            //add searchable properties to grid
            ((ColumnView)gridControl1.Views[0]).Columns.AddVisible("FullName");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("Client.FirstName");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("Client.LastName");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("Client.FAST");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("Address.StreetAddress1");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("Address.City.Name");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("LDC.Name");
            ((ColumnView)gridControl1.Views[0]).Columns.AddField("ApplicationType");
            //((ColumnView)gridControl1.Views[0]).Columns.AddField("SocialHousing");
           
            //show the find panel
            ((ColumnView)gridControl1.Views[0]).ShowFindPanel();
            ((ColumnView)gridControl1.Views[0]).OptionsFind.AlwaysVisible = true;

            //add event listener for when row changes
            ((ColumnView)gridControl1.Views[0]).FocusedRowChanged += Form1_FocusedRowChanged;
     


        }

        private void cmdScanDoc_Click(object sender, EventArgs e)
        {

            if (ExcelFolder != "")
            {
                //Sort excel sheets by date
                CurrentDoc = new DirectoryInfo(ExcelFolder)
                    .GetFiles("*.pdf")
                    .OrderBy(f => f.CreationTime)
                    .ToArray();
            }


       //     string path = @"P:\Division-Office Admin-HR-IT-LEGAL-SECURITY-SAFETY\IT\copier1@greensaver.org_20160112_150327.pdf";
            //string path = @"C:\Users\matt\Documents\greensaver\greensaver\wp-content\uploads\2015\09\BlowerDoorWeb.pdf";

            List<Dictionary<int, string>> tiffLocations = new List<Dictionary<int, string>>();
            if (ExcelFolder != "")
            {
                tiffLocations = PDFFunctions.createTiffFiles(@CurrentDoc[0].DirectoryName + "\\" + CurrentDoc[0].Name);

                foreach (var card in tiffLocations[0])
                {
                    FlowLayoutPanel pictureContainer = new FlowLayoutPanel();
                    pictureContainer.Width = 77;
                    pictureContainer.Height = 90;
                    pictureContainer.AutoSize = false;
                    pictureContainer.AutoScroll = false;
                    pictureContainer.BorderStyle = BorderStyle.FixedSingle;
                    fLPItemNotFound.Controls.Add(pictureContainer);

                    Card picture = new Card();

                    picture.ImageLocation = @card.Value;

                    //Set low res path
                    foreach (var lowRes in tiffLocations[1])
                    {
                        string highResToLowResPath = picture.ImageLocation.Replace("highRes", "lowRes");
                        if (lowRes.Value == highResToLowResPath)
                        {
                            picture.ImageLocationLR = lowRes.Value;
                        }
                    }

                    picture.PageText = PDFFunctions.imageToText(@card.Value);

                    //add card to cardlist
                    cards.Add(picture);

                    picture.Width = 68;
                    picture.Height = 81;
                    picture.AutoSize = false;
                    picture.Image = Bitmap.FromFile(picture.ImageLocation);
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureContainer.Controls.Add(picture);

                    //init drang and drop events
                    picture.Visible = true;
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.DragEnter += picture_DragEnter;
                    picture.DragDrop += picture_DragDrop;
                    picture.MouseDown += picture_MouseDown;

                    picture.AllowDrop = true;
                }


                currSelectedImg = new Card();
                //Swap cards from fLPItemNotFound to proper cards
                cardSort();
                AddBlankCardToBottom();

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select excel folder path.");
            }
        }

        void picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ImageZoom iz = new ImageZoom((sender as PictureBox).Image);

                iz.ShowDialog();

                return;
            }

            currSelectedImg = sender as Card;

            currentCard = sender as Card;

            (sender as Card).DoDragDrop((sender as Card), DragDropEffects.Copy);

        }

        void picture_DragEnter(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.Copy;
        }

        void picture_DragDrop(object sender, DragEventArgs e)
        {
            //switch the card cards and change the page object on the card

            Card top = sender as Card;
            Card bottom = (Card)e.Data.GetData(typeof(Card));

            Page topPage = top.Page;
            Page bottomPage = bottom.Page;

            if (topPage != null)
            {
                topPage.Card = bottom;
            }

            if (bottomPage != null)
            {
                bottomPage.Card = top;
            }
            else
            {
               // GridHelper.OrderBottomPanel(this);
            }

            top.Page = bottomPage;
            bottom.Page = topPage;


            

            object p = bottom.Parent;

            bottom.Parent = top.Parent;

            top.Parent = (Control)p;

            if (top.Page == null)
            {
                if (fLPItemNotFound.Controls.Contains(top.Parent))
                {
                  // fLPItemNotFound.Controls.Remove(top.Parent);
                 //   AddBlankCardToBottom();
                    
                }
               // top.Parent.Parent = null;

                
                
            }

            if (bottom.Page == null)
            {

                if (fLPItemNotFound.Controls.Contains(bottom.Parent))
                {
                 //   fLPItemNotFound.Controls.Remove(bottom.Parent);
                 //   AddBlankCardToBottom();

                }

             //   bottom.Parent.Parent.Parent.Controls.Remove(bottom.Parent);
             //   AddBlankCardToBottom();
            }


           

            
        }

        private void cmdConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConfigForm configForm = new ConfigForm();

            configForm.ShowDialog();

            this.xtraScrollableControl1.Controls.Clear();

            cards = new List<Models.Card>();

            records.Clear();

            records = GridHelper.GetRecords();

            RefreshApplicationGUI();


        

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
        }

        private void cmdClearData_Click(object sender, EventArgs e)
        {

            AddBlankCard(this.fLPItemNotFound);


            if (ExcelFolder != "")
            {
                //Iterate to next excel sheet in array
                
              //  onLoad();
                //cmdScanDoc.PerformClick();
            }
        }

        private void cmdImport_Click(object sender, EventArgs e)
        {
            RemoveAllBlankCardsFromBottom();

          

            //create pdfs
           // createPDFs();

            //save the pdf to gibs

            //use the list of applications because they have the PDF location attached 

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PDFScanAndSort\\Configs\\";

            //Folder browse to write config txt file.
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ExcelFolder = folderBrowserDialog1.SelectedPath;

                TextWriter tw = new StreamWriter(path + "\\ExcelFolder.txt", false);
                tw.WriteLine(ExcelFolder);
                tw.Dispose();
                tw.Close();
            }
        }

        public void AddBlankCardToBottom()
        {

            int check = 0;
            if (fLPItemNotFound.Controls.Count == 0)
            {
                check++;
            }
            else
            {
                foreach (Control control in fLPItemNotFound.Controls)
                {
                    foreach (var item in control.Controls)
                    {
                        if (item.GetType() == typeof(Card))
                        {
                            if ((item as Card).Page == null)
                            {
                                check++;
                            }
                           
                        }
                    }
                   
                   
                  
                }

            }

            if (check == 1)
            {
                fLPItemNotFound.SuspendLayout();



                FlowLayoutPanel pictureContainer = new FlowLayoutPanel();
                pictureContainer.Width = 77;
                pictureContainer.Height = 90;
                pictureContainer.AutoSize = false;
                pictureContainer.AutoScroll = false;
                pictureContainer.BorderStyle = BorderStyle.FixedSingle;
            

                Card picture = new Card();

                picture.ImageLocation = null;

                //add card to cardlist
                cards.Add(picture);

                picture.Width = 68;
                picture.Height = 81;
                picture.AutoSize = false;

                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureContainer.Controls.Add(picture);

                //init drang and drop events
                picture.Visible = true;
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.DragEnter += picture_DragEnter;
                picture.DragDrop += picture_DragDrop;
                picture.MouseDown += picture_MouseDown;
                

                picture.AllowDrop = true;

                fLPItemNotFound.Controls.Add(pictureContainer);

                fLPItemNotFound.ResumeLayout(true);
              
            }

        }

        public void AddBlankCard(FlowLayoutPanel fp)
        {
            fp.SuspendLayout();



            FlowLayoutPanel pictureContainer = new FlowLayoutPanel();
            pictureContainer.Width = 77;
            pictureContainer.Height = 90;
            pictureContainer.AutoSize = false;
            pictureContainer.AutoScroll = false;
            pictureContainer.BorderStyle = BorderStyle.FixedSingle;


            Card picture = new Card();

            picture.ImageLocation = null;

            //add card to cardlist
            cards.Add(picture);

            picture.Width = 68;
            picture.Height = 81;
            picture.AutoSize = false;

            picture.BorderStyle = BorderStyle.FixedSingle;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureContainer.Controls.Add(picture);

            //init drang and drop events
            picture.Visible = true;
            picture.BorderStyle = BorderStyle.FixedSingle;
            picture.DragEnter += picture_DragEnter;
            picture.DragDrop += picture_DragDrop;
            picture.MouseDown += picture_MouseDown;


            picture.AllowDrop = true;

            fp.Controls.Add(pictureContainer);

            fp.ResumeLayout(true);
              

        }

        public void RemoveAllBlankCardsFromBottom()
        {

            foreach (Control control in fLPItemNotFound.Controls)
            {
                 if (control.Controls[0].GetType() == typeof(Card))
                 {
                        if ((control.Controls[0] as Card).Page == null)
                        {
                            fLPItemNotFound.Controls.Remove(control);
                
                        }
                       
                    }

              
                foreach (var item in control.Controls)
                {
                   
                }
                //if ((control.Controls[0] as Card).ImageLocation == null)
                //{

                   
                //}

            }
        }

        private void clearAllCards()
        {
            foreach (var item in cards)
            {
                item.Dispose();
            }

            xtraScrollableControl1.Controls.Clear();

            fLPItemNotFound.Controls.Clear();
        }

        private void cardSort()
        {
            Ranker ranker = new Ranker(this.applications, this.cards);

            ranker.RankCards();

            var grpApps = ranker.RanksList
          .GroupBy(u => u.Page)
          .Select(grp => grp.ToList())
          .ToList();

            foreach (var item in grpApps)
            {
                var grpPage = item
                   .GroupBy(u => u.card)
                   .Select(grp => grp.ToList())
                   .ToList();

                Card winningCard = null;
                int winningGrpnum = 0;
                int currentwinningGrpnum = 0;

                foreach (var grpItem in grpPage)
                {
                    currentwinningGrpnum = grpItem.Count;

                    if (winningGrpnum < currentwinningGrpnum)
                    {
                        winningGrpnum = currentwinningGrpnum;
                        winningCard = grpItem[0].card;
                    }

                }

                if (winningCard != null)
                {
                    GridHelper.SwapCards(item[0].Page.Card, winningCard);
                }

            }
        }

        private void createPDFs()
        {

            applications.RemoveAll(item => item.Pages[0].Card.ImageLocation == null);


            foreach (var item in applications)
            {
                Console.WriteLine("application name - " + item.Name );

           
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PDFScanAndSort\\FinishedPDFs\\"
                    + Path.GetFileName(@CurrentDoc[0].DirectoryName + "\\" + CurrentDoc[0].Name).Replace(".pdf", "") + "\\";

                string appName = @path + item.Name + ".pdf";


                if (!Directory.Exists(@path))
                {
                    Directory.CreateDirectory(@path);
                    item.tiffToPDF(appName);
                }
                else
                {
                    item.tiffToPDF(appName);
                }


                item.PDFLocation = appName;

            }

     //       GridHelper.OrderBottomPanel(this);
        }

        private void fLPItemNotFound_ControlAdded(object sender, ControlEventArgs e)
        {
        

        }

        private void fLPItemNotFound_ControlRemoved(object sender, ControlEventArgs e)
        {

        
        }

      
        

    }
}
