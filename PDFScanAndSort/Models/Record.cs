using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Record
    {
        int pageNumber;
        string searchTerms;
        string application;

        public Record(int pageNumber, string searchTerms, string application)
        {
            this.pageNumber = pageNumber;
            this.searchTerms = searchTerms;
            this.application = application;

        }

        public int PageNumber { get { return pageNumber; } }

        public string SearchTerms
        {
            get { return searchTerms; }
            set { searchTerms = value; }
        }

        public string Application
        {
            get { return application; }
            set { application = value; }
        }


    }
}
