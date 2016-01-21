using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Rank
    {

        public Card card { get; set; }

        public int NumberOfKeysFound { get; set; }

        public Page Page { get; set; }


    }
}
