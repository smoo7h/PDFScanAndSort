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
            XPClassInfo classInfo = session1.GetClassInfo(typeof(HAPApplication));
            // Create an XPServerCollectionSource object. 
            XPServerCollectionSource xpServerCollectionSource1 =
              new XPServerCollectionSource(session1, classInfo);
            // Create a grid control. 

            gridControl1.Dock = DockStyle.Fill;

            // this.Controls.Add(gridControl1);
            // Bind the grid control to the data source. 
            gridControl1.DataSource = xpServerCollectionSource1;

            gridControl1.MainView.HideEditor();
            
            ((ColumnView)gridControl1.Views[0]).Columns.Clear();

            ((ColumnView)gridControl1.Views[0]).Columns.AddVisible("FullName");
            ((ColumnView)gridControl1.Views[0]).ShowFindPanel();

         //   ((ColumnView)gridControl1.Views[0]).




        //    ((ColumnView)gridControl1.Views[0]).Appearance.
           
             
        
        
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {

        }

      
    }
}
