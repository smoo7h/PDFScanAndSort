using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class Application
    {
        public Application()
        {
          //  this.Cards = new List<Card>();

            this.Pages = new List<Page>();

        }


        public string Name { get; set; }

        public List<Page> Pages { get; set; }

      //  public List<Card> Cards { get; set; }




    }
}
