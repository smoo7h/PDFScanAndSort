using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Page
    {

        public int PageNumber { get; set; }

        public List<string> SearchStrings { get; set; }

        public Application Application { get; set; }

    }
}
