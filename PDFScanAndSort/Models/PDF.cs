using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Models
{
    public class PDF
    {

        public Application Application { get; set; }

        public string SaveLocation { get; set; }

        public Guid GibsLocationOID { get; set; }

        public List<Card> Cards { get; set; }




    }
}
