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
    public partial class ImageZoom : Form
    {
        public Image image;
        public ImageZoom(Image img)
        {
            image = img;
            InitializeComponent();
            pictureBox1.Image = img;
            
        }
    }
}
