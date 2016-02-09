using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFScanAndSort.Models
{
    public class Card : PictureBox
    {
       

        public string ImageLocation { get; set; }

        public string ImageLocationLR { get; set; }

        public string PageText { get; set; }

        public Page Page { get; set; }

        public List<Rank> PageRanks { get; set; }

        //public PictureBox PictureBox { get; set; }

       // public int PageNumber { get; set; }

    //    public Application Application { get; set; }



    }
}
