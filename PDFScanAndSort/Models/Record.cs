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
            this.searchTermStringList = new List<string>();

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

        private List<string> searchTermStringList;
        public List<string> SearchTermStringList
        {
            get { return searchTermStringList; }
            set { searchTermStringList = value; }
        }

        public void BuildStringList()
        {
            if (searchTerms.Length > 0)
            {
                searchTermStringList.Clear();
                string[] terms = searchTerms.Split(',');

                foreach (string s in terms)
                {
                    searchTermStringList.Add(s.Trim());
                }
            }
        }

    }
}
