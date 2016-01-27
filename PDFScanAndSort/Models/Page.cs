using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFScanAndSort.Models
{
    public class Page 
    {
       
     //   public PictureBox PictureBox { get; set; }

        public int PageNumber { get; set; }

        public List<string> SearchStrings { get; set; }

        public Application Application { get; set; }

        public Card Card { get; set; }

    }
}
