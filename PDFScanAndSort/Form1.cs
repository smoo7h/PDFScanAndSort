using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
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
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

            XPODataHelper datahelper = new XPODataHelper();

            IObjectSpace space = datahelper.Connect();

            // Create a Session object. 
            Session session1 = ((XPObjectSpace)space).Session;
            // Create an XPClassInfo object corresponding to the Person class. 
            XPClassInfo classInfo = session1.GetClassInfo(typeof(GIBS.Module.Models.Programs.Program));
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
            //show the find panel
            ((ColumnView)gridControl1.Views[0]).ShowFindPanel();
            ((ColumnView)gridControl1.Views[0]).OptionsFind.AlwaysVisible = true;

            //add event listener for when row changes
            ((ColumnView)gridControl1.Views[0]).FocusedRowChanged += Form1_FocusedRowChanged;
     
        
        
        }

        void Form1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {      
            //display data into text box summery when focused row is changed 

            txtFastNumber.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FAST").ToString() : "";

            txtFirstName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.FirstName").ToString() : "";

            txtLastName.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Client.LastName").ToString() : "";

            txtStreetAddress.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.StreetAddress1").ToString() : "";

            txtCity.Text = ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name") != null ? ((ColumnView)gridControl1.Views[0]).GetFocusedRowCellValue("Address.City.Name").ToString() : "";


        }

        private void cmdScanDoc_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\gianluca.gallo\Desktop\Working Space\copier1@greensaver.org_20160112_150327.pdf";
            List<Dictionary<int, string>> tiffLocations = PDFFunctions.createTiffFiles(path);

            List<Models.Card> cards = new List<Models.Card>();

            foreach (var item in tiffLocations[0])
            {
                
                Models.Card card = new Models.Card();
                card.PageText = PDFFunctions.imageToText(@item.Value);
                card.ImageLocation = @item.Value;
                cards.Add(card);
            }
        }

     

       

      
    }
}
