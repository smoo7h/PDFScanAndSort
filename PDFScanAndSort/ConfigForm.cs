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
        List<Record> r;

        public ConfigForm()
        {
            InitializeComponent();

            r = GridHelper.GetRecords();


         
            GridHelper.AddRecords(r, this.gridConfig);

            //refresh app list
            reFreshAppList();

            //for testing 
            this.lstBoxApplications.SelectedValueChanged += lstBoxApplications_SelectedValueChanged;


          

        }

        private void CmdAddColumn_Click(object sender, EventArgs e)
        {
            Record newrecord = new Record(Convert.ToInt32(txtPageNumber.Text),txtSearchTerms.Text, txtApplication.Text);

            GridHelper.AddNewRecord(newrecord, this.gridConfig);
            r.Add(newrecord);

            GridHelper.SaveRecordFile(r);

            this.lstBoxApplications.Items.Clear();

            reFreshAppList();

            GridHelper.SaveRecordFile(r);

        }

        private void lstBoxApplications_SelectedValueChanged(object sender, EventArgs e)
        {   
                BinaryOperator bOper = new BinaryOperator("Application", lstBoxApplications.SelectedValue);

                //  CriteriaOperator cri = BinaryOperator
                ColumnFilterInfo d = new ColumnFilterInfo(bOper);

                ((ColumnView)gridConfig.Views[0]).Columns[0].FilterInfo = d;

         

        }

      

        public  void reFreshAppList()
        {
            var groupedCustomerList = r
              .GroupBy(u => u.Application)
              .Select(grp => grp.ToList())
              .ToList();

            foreach (var item in groupedCustomerList)
            {
                lstBoxApplications.Items.Add(item[0].Application);
            }

        }
        


      
    }
}
