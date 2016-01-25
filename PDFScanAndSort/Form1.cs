﻿using DevExpress.ExpressApp;
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

            //initalize list DB view
            InitializeDataBaseListView();
        

        
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

       

      
    }
}
