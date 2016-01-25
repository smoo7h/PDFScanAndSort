using DevExpress.XtraGrid;
using PDFScanAndSort.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Utils
{
    public static class GridHelper
    {

        public static void AddNewRecords(List<Record> records, GridControl theGrid)
        {
            BindingList<Record> listDataSource = new BindingList<Record>();

            foreach (Record item in records)
            {
                listDataSource.Add(item);
            }
            
            theGrid.DataSource = listDataSource;
        }

        public static List<Record> GetRecords()
        {

            List<Record> recordList = new List<Record>();


            return recordList;

        }

        public static void SaveRecordFile(List<Record> recordList)
        {

          

        }





    }
}
