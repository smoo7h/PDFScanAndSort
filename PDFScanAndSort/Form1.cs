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
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFScanAndSort
{
    public partial class Form1 : Form
    {
        public Card currSelectedImg;

        public Card currentCard;

        public string CurrentFullPDF;
     //   public Card currSelectedCard;

        public List<Models.Card> cards;

        public List<Models.Record> records;

        public List<Models.Application> applications;

        //Current folder directory containing PDFs
        public string PDFFolder = "";
        //Current selected pdf in excel folder directory
        public int CurrentDocNum = 0;
        //List of excel sheets
        public FileInfo[] CurrentDoc;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\PDFScanAndSort\\Configs\\";



            PDFFolder = Utils.ConfigSettings.ReadSetting("PDFFolder");

            //create remp folder

            string p = Path.GetTempPath() + "ScannedPDFs\\";
            Directory.CreateDirectory(p);


            //initalize list DB view
            try
            {
                InitializeDataBaseListView();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Cannot connect to GIBS " + ex.Message );
            }

            cards = new List<Models.Card>();
            records = GridHelper.GetRecords();
            applications = new List<Models.Application>();
            RefreshApplicationGUI();

        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
        }

        public void resetControls()
        {
            
            clearAllCards();
            cards.Clear();
            records.Clear();
            applications.Clear();
            foreach (Card c in cards)
            {
                c.Dispose();
            }

            cards = new List<Models.Card>();
            records = GridHelper.GetRecords();
            applications = new List<Models.Application>();
         
            //clear txt boxes
            foreach (Control c in xtraScrollableControl4.Controls)
            {
                if (c.GetType() == typeof(TextEdit))
                {
                    c.Text = "";
                }
            }

            ((ColumnView)gridControl1.Views[0]).FindFilterText = "";

            CurrentFullPDF = "";

            RefreshApplicationGUI();
        }

        private void RefreshApplicationGUI()
        {

            //add new application button

            SimpleButton cmdAddApplication = new SimpleButton();
            cmdAddApplication.Text = "Add New Application";
            cmdAddApplication.Dock = DockStyle.Bottom;
            cmdAddApplication.Click += cmdAddApplication_Click;

            xtraScrollableControl1.Controls.Add(cmdAddApplication);

            //create apps in the UI
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

                //add the add and subtract buttons 

                FlowLayoutPanel btnContainer = new FlowLayoutPanel();
                btnContainer.Width = 42;
                btnContainer.Height = 111;
                btnContainer.AutoSize = false;
                btnContainer.AutoScroll = false;
                btnContainer.BorderStyle = BorderStyle.FixedSingle;
                btnContainer.WrapContents = true;

                SimpleButton newbtn = new SimpleButton();
                newbtn.Text = "+";
                newbtn.Width = 33;
                newbtn.Height = 48;
                newbtn.Click += newbtn_Click;



                SimpleButton removebtn = new SimpleButton();   
                removebtn.Text= "-";
                removebtn.Width = 33;
                removebtn.Height = 48;
                removebtn.Click += removebtn_Click;


                btnContainer.Controls.Add(newbtn);
                btnContainer.Controls.Add(removebtn);
                panelLong.Controls.Add(btnContainer);


            }


        }

        void cmdAddApplication_Click(object sender, EventArgs e)
        {
            NewAppWizard wizard = new NewAppWizard();
            wizard.ShowDialog();
            PDFScanAndSort.Models.Application app = wizard.Application;
            wizard.Dispose();




            GroupControl gc = new GroupControl();
            gc.Text = app.Name;
            gc.Dock = DockStyle.Top;
            gc.Width = 446;
            gc.Height = 165;
            xtraScrollableControl1.Controls.Add(gc);
            xtraScrollableControl1.Controls.SetChildIndex(gc, 0);


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


            app.Name = app.Name;
            applications.Add(app);

            for (int i = 0; i < app.NumberOfPages; i++)
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


                Page page = new Page();
                page.Card = picture;
                
                picture.Page = page;
                picture.Page.PageNumber = i;
                cards.Add(picture);

                //  card.PageNumber = i;
                page.Application = app;
                // page.PictureBox = picture;
                app.Pages.Add(page);

              

            }


            

            //add the add and subtract buttons 

            FlowLayoutPanel btnContainer = new FlowLayoutPanel();
            btnContainer.Width = 42;
            btnContainer.Height = 111;
            btnContainer.AutoSize = false;
            btnContainer.AutoScroll = false;
            btnContainer.BorderStyle = BorderStyle.FixedSingle;
            btnContainer.WrapContents = true;

            SimpleButton newbtn = new SimpleButton();
            newbtn.Text = "+";
            newbtn.Width = 33;
            newbtn.Height = 48;
            newbtn.Click += newbtn_Click;



            SimpleButton removebtn = new SimpleButton();
            removebtn.Text = "-";
            removebtn.Width = 33;
            removebtn.Height = 48;
            removebtn.Click += removebtn_Click;


           
            btnContainer.Controls.Add(newbtn);
            btnContainer.Controls.Add(removebtn);
            panelLong.Controls.Add(btnContainer);



            
        }

        void removebtn_Click(object sender, EventArgs e)
        {

            //make sure there is a blank card 2 remove from the end
            if ((sender as SimpleButton).Parent.Parent.Controls.Count > 2 && ((sender as SimpleButton).Parent.Parent.Controls[(sender as SimpleButton).Parent.Parent.Controls.IndexOf((sender as SimpleButton).Parent) - 1].Controls[0] as Card).ImageLocation == null)
                {
                //remove the card
                    RemoveCard(((sender as SimpleButton).Parent.Parent as FlowLayoutPanel));
                }

        }

        void newbtn_Click(object sender, EventArgs e)
        {
            //add a new page and card to the application
            Card newcard = AddBlankCard(((sender as SimpleButton).Parent.Parent as FlowLayoutPanel));
            newcard.Height = 75;
            newcard.Width = 70;
            newcard.Page = new Page();
            newcard.Page.Card = newcard;
            newcard.Page.Application  = ((newcard.Parent.Parent as FlowLayoutPanel).Controls[0].Controls[0] as Card).Page.Application;
            newcard.Page.Application.Pages.Add(newcard.Page);
            newcard.Parent.Parent.Controls.SetChildIndex(newcard.Parent, newcard.Parent.Parent.Controls.Count - 2);
            (newcard.Parent as FlowLayoutPanel).Height = 111;

            


        }

        void Form1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {      
            //display data into text box summery when focused row is changed 

            txtFastNumber.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST").ToString() : "";

            txtFirstName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName").ToString() : "";

            txtLastName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName").ToString() : "";

            txtStreetAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1").ToString() : "";

            txtFullAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.FullAddress") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.FullAddress").ToString() : "";

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
                ((ColumnView)gridControl1.Views[0]).Columns.AddField("Address.FullAddress");
                ((ColumnView)gridControl1.Views[0]).Columns.AddField("Address.City.Name");
                ((ColumnView)gridControl1.Views[0]).Columns.AddField("LDC.Name");
                ((ColumnView)gridControl1.Views[0]).Columns.AddField("ApplicationType");
                ((ColumnView)gridControl1.Views[0]).Columns.AddField("Oid");
                //((ColumnView)gridControl1.Views[0]).Columns.AddField("SocialHousing");

                //show the find panel
                ((ColumnView)gridControl1.Views[0]).ShowFindPanel();
                ((ColumnView)gridControl1.Views[0]).OptionsFind.AlwaysVisible = true;

                //add event listener for when row changes
                ((ColumnView)gridControl1.Views[0]).FocusedRowChanged += Form1_FocusedRowChanged;


              

     


        }

        public bool Validate()
        {
            foreach (Control item in xtraScrollableControl4.Controls)
            {
                if (item.GetType() == typeof(TextEdit))
                {
                    if (item.Name != "txtFastNumber" && item.Text == "")
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        private void cmdScanDoc_Click(object sender, EventArgs e)
        {

                //Sort pdf files by date
                CurrentDoc = new DirectoryInfo(PDFFolder)
                    .GetFiles("*.pdf")
                    .OrderByDescending(f => f.CreationTime)
                    .ToArray();

                if (Directory.GetFiles(PDFFolder).Length == 0)
                {
                    MessageBox.Show("You do not have any PDF's to import");
                    return;
                }
                

                CurrentFullPDF = CurrentDoc[0].FullName;

            List<Dictionary<int, string>> tiffLocations = new List<Dictionary<int, string>>();
            if (PDFFolder != "")
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

                //remoce blanks from borrom
                RemoveAllFromBottomAndAddBlank();

                //try to apply a name to the filter based on the txt in the app 

                try
                {
                    string name = GridHelper.GetClientName(applications);

                    ((ColumnView)gridControl1.Views[0]).FindFilterText = name;

                    if (name != "")
                    {
                        txtFastNumber.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST").ToString() : "";

            txtFirstName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName").ToString() : "";

            txtLastName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName").ToString() : "";

            txtStreetAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1").ToString() : "";

            txtFullAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.FullAddress") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.FullAddress").ToString() : "";

            txtCity.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name").ToString() : "";

            txtLDC.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("LDC.Name") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("LDC.Name").ToString() : "";

            txtAppType.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("ApplicationType") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("ApplicationType").ToString() : "";
                    }


                }
                catch (Exception ex)
                {

                  
                }

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


            top.Page = bottomPage;
            bottom.Page = topPage;

            object p = bottom.Parent;

            bottom.Parent = top.Parent;

            top.Parent = (Control)p;

            RemoveAllFromBottomAndAddBlank();
          
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

        private void cmdClearData_Click(object sender, EventArgs e)
        {
                //Iterate to next excel sheet in array

         
                resetControls();
                //cmdScanDoc.PerformClick();
            
        }

        private void cmdImport_Click(object sender, EventArgs e)
        {

            if (Validate() == false)
            {
                MessageBox.Show("Please select an application from the list");
                return;
            }

            //create pdfs
            createPDFs();

            //save the pdf to gibs
            savePDFToFolder();
            
            //attach PDF file paths to GIBS

            XPODataHelper xpd = new XPODataHelper();

            string oid = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Oid") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Oid").ToString() : "";

            xpd.SaveFileToDataBase(oid,this.applications);
            /////Cleanup 
            //delete local PDF file and TIFF files 


            DeleteCurrentPDF(CurrentFullPDF);

            //clear form
            resetControls();

            MessageBox.Show("Successfuly Imported =D");

            //delete the old PDF
            ClearTempFiles();


        }

        private void DeleteCurrentPDF(string location)
        {
            try
            {
                File.Delete(location);
            }
            catch (Exception ex )
            {

                MessageBox.Show(ex.Message);
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

        public void RemoveCard(FlowLayoutPanel panel)
        {

            Card card = panel.Controls[panel.Controls.Count - 2].Controls[0] as Card;


            panel.Controls.RemoveAt(panel.Controls.Count - 2);

            //remove page 
            Page p = card.Page;
            PDFScanAndSort.Models.Application app = card.Page.Application;

            app.Pages.Remove(p);
            p.Application = null;
            card.Page = null;
            card.Dispose();

        }

        public Card AddBlankCard(FlowLayoutPanel fp)
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


            return picture;

        }

        public void RemoveAllBlankCardsFromBottom()
        {

            List<Card> cardList = new List<Card>();

            foreach (Control control in fLPItemNotFound.Controls)
            {
                foreach (var item in control.Controls)
                {
                    if (item.GetType() == typeof(Card) && (item as Card).ImageLocation == null)
                    {
                        cardList.Add(item as Card);
                    }
                }
            }

            foreach (Card c in cardList)
            {
                fLPItemNotFound.Controls.Remove(c.Parent);
            }

        }

        public void RemoveAllFromBottomAndAddBlank()
        {
            RemoveAllBlankCardsFromBottom();
            AddBlankCard(this.fLPItemNotFound);
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

        private List<string> savePDFToFolder()
        {
            List<string> saveList = new List<string>();
 
            string pdfOutFolderLocaton = Utils.ConfigSettings.ReadSetting("DestinationFolder");
            string userDestFolder = "";

            if (!Directory.Exists(pdfOutFolderLocaton))
            {
                MessageBox.Show("directory " + pdfOutFolderLocaton + " Does not exist please create it");
                return null;
            }

            string LDCDir = pdfOutFolderLocaton + "\\" + this.txtLDC.Text;

            //check if LDC folder exists 
            if (!Directory.Exists(LDCDir))
            {
                //create it if it doesnt exist 
                Directory.CreateDirectory(LDCDir);
            }
            
            //check and create user folder 
            if (!Directory.Exists(LDCDir +"\\" +txtFullAddress.Text.Replace('.',' ')))
            {
                //create it if it doesnt exist 
                Directory.CreateDirectory(LDCDir + "\\" + txtFullAddress.Text.Replace('.', ' '));
            }
            //set fodler to variable
            userDestFolder = LDCDir + "\\" + txtFullAddress.Text.Replace('.', ' ');
            //filter the app list 
            applications.RemoveAll(item => item.Pages[0].Card.ImageLocation == null);

            //save origional untouched PDF to folder
            File.Copy(CurrentFullPDF, userDestFolder + "\\" + txtFullAddress.Text.Replace('.', ' ') + "-rawscan.pdf", true);

            saveList.Add(userDestFolder + "\\" + txtFullAddress.Text.Replace('.', ' ') + "-rawscan.pdf");

            foreach (var item in applications)
            {
                //save PDF to director
                File.Copy(item.PDFLocation, userDestFolder + "\\"  + Path.GetFileName(item.PDFLocation), true);
                saveList.Add(userDestFolder + "\\" + Path.GetFileName(item.PDFLocation));
            }

            return saveList;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MessageBox.Show("Current Directory " + Utils.ConfigSettings.ReadSetting("PDFFolder"));

            //Folder browse to write config txt file.
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Utils.ConfigSettings.AddUpdateAppSettings("PDFFolder", folderBrowserDialog1.SelectedPath);

                PDFFolder = Utils.ConfigSettings.ReadSetting("PDFFolder");
            }
        }

        public void ClearTempFiles()
        {
            string p = Path.GetTempPath() + "ScannedPDFs\\";

            try
            {
                  Directory.Delete(Path.GetTempPath() + "ScannedPDFs\\", true);
                  Directory.CreateDirectory(p);
            }
            catch (Exception e)
            {
                
              
            }
        
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //update destination folder 


            MessageBox.Show("Current Directory " + Utils.ConfigSettings.ReadSetting("DestinationFolder"));

            //Folder browse to write config txt file.
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Utils.ConfigSettings.AddUpdateAppSettings("DestinationFolder", folderBrowserDialog1.SelectedPath);
            }


        }
     
        

    }
}
