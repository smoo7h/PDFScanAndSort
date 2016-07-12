using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFScanAndSort.Models;

namespace PDFScanAndSort
{
    public partial class NewAppWizard : Form
    {
        public NewAppWizard()
        {
            InitializeComponent();
        }

        public PDFScanAndSort.Models.Application Application { get; set; }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text == "")
            {
                MessageBox.Show("Please Add An Application Name");
               
            }
            else
            {

                PDFScanAndSort.Models.Application app = new PDFScanAndSort.Models.Application();
                app.Name = textEdit1.Text;
                app.NumberOfPages = Convert.ToInt32(textEdit2.Text);
                this.Application = app;
                
                this.Close();
            }


        }
    }
}
