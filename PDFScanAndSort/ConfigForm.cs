using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using PDFScanAndSort.Models;
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
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

   

            List<Record> r = new List<Record>();
            r.Add(new Record(1,"Matt rec","Test App"));
            GridHelper.AddRecords(r, this.gridConfig);


            //for testing 
            this.lstBoxApplications.SelectedValueChanged += lstBoxApplications_SelectedValueChanged;

            lstBoxApplications.Items.Add("Test App");
            lstBoxApplications.Items.Add("Test App2");
          


        }

        private void CmdAddColumn_Click(object sender, EventArgs e)
        {
            GridHelper.AddNewRecord(new Record(Convert.ToInt32(txtPageNumber.Text), txtSearchTerms.Text, txtApplication.Text), this.gridConfig);

        }

        private void lstBoxApplications_SelectedValueChanged(object sender, EventArgs e)
        {   
                BinaryOperator bOper = new BinaryOperator("Application", lstBoxApplications.SelectedValue);

                //  CriteriaOperator cri = BinaryOperator
                ColumnFilterInfo d = new ColumnFilterInfo(bOper);

                ((ColumnView)gridConfig.Views[0]).Columns[0].FilterInfo = d;

         

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            

        }

        


      
    }
}
