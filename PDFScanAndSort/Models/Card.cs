using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Card
    {
        public Card()
        {

        }

        public string ImageLocation { get; set; }

        public string PageText { get; set; }

        public Page CurrentPage { get; set; }

        public List<Rank> PageRanks { get; set; }

       // public int PageNumber { get; set; }

        public Application Application { get; set; }



    }
}
