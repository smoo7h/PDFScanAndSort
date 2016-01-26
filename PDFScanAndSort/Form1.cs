using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using GIBS.Module.Models.Programs.HAP;
using PDFScanAndSort.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFScanAndSort
{
    public partial class Form1 : Form
    {
        public PictureBox currSelectedImg;

        public List<Models.Card> cards;

        public List<Models.Record> records;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            //initalize list DB view
            InitializeDataBaseListView();

            cards = new List<Models.Card>();

            records = GridHelper.GetRecords();
            RefreshGrid();
        
        }

        private void RefreshGrid()
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

                    PictureBox picture = new PictureBox();
                    picture.Width = 76;
                    picture.Height = 75;
                    picture.AutoSize = false;
                    //picture.Image = Bitmap.FromFile(item);
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureContainer.Controls.Add(picture);



                    picture.Visible = true;
                    picture.BorderStyle = BorderStyle.FixedSingle;
                    picture.DragEnter += pb_DragEnter;
                    picture.DragDrop += pb_DragDrop;
                    picture.MouseDown += pb_MouseDown;

                    picture.AllowDrop = true;


                    CheckBox checkbox = new CheckBox();
                    checkbox.AutoSize = false;
                    checkbox.Width = 76;
                    checkbox.Height = 22;
                    checkbox.Text = "Page: " + i;
                    pictureContainer.Controls.Add(checkbox);
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
            string path = @"P:\Division-Office Admin-HR-IT-LEGAL-SECURITY-SAFETY\IT\Luca's Legacy\TestPDFRead\copier1@greensaver.org_20160112_150327.pdf";
            List<Dictionary<int, string>> tiffLocations = PDFFunctions.createTiffFiles(path);

            foreach (var item in tiffLocations[0])
            {

                Models.Card card = new Models.Card();
            //    card.PageText = PDFFunctions.imageToText(@item.Value);
                card.ImageLocation = @item.Value;
                cards.Add(card);
            }

            foreach (var card in cards)
            {
                FlowLayoutPanel pictureContainer = new FlowLayoutPanel();
                pictureContainer.Width = 77;
                pictureContainer.Height = 90;
                pictureContainer.AutoSize = false;
                pictureContainer.AutoScroll = false;
                pictureContainer.BorderStyle = BorderStyle.FixedSingle;
                fLPItemNotFound.Controls.Add(pictureContainer);

                PictureBox picture = new PictureBox();
                picture.Width = 68;
                picture.Height = 81;
                picture.AutoSize = false;
                picture.Image = Bitmap.FromFile(card.ImageLocation);
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureContainer.Controls.Add(picture);



                picture.Visible = true;
                picture.BorderStyle = BorderStyle.FixedSingle;
                picture.DragEnter += pb_DragEnter;
                picture.DragDrop += pb_DragDrop;
                picture.MouseDown += pb_MouseDown;

                picture.AllowDrop = true;
            }


            currSelectedImg = new PictureBox();

        }

        private void cmdConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConfigForm configForm = new ConfigForm();

            configForm.ShowDialog();

            this.xtraScrollableControl1.Controls.Clear();

            cards = new List<Models.Card>();

            records.Clear();

            records = GridHelper.GetRecords();
            RefreshGrid();


        

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
        }

        void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
              //  Form2 f2 = new Form2((sender as PictureBox).Image);

               // f2.ShowDialog();

                return;
            }

            //throw new NotImplementedException();
            currSelectedImg = sender as PictureBox;

            (sender as PictureBox).DoDragDrop((sender as PictureBox).Image, DragDropEffects.Copy);


        }

        void pb_DragDrop(object sender, DragEventArgs e)
        {
            currSelectedImg.Image = (sender as PictureBox).Image;

            (sender as PictureBox).Image = (Image)e.Data.GetData(DataFormats.Bitmap);
        }

        void pb_DragEnter(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.Copy;
        }
      
    }
}
