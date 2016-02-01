using DevExpress.XtraGrid;
using PDFScanAndSort.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace PDFScanAndSort.Utils
{
    public static class GridHelper
    {

        public static void AddRecords(List<Record> records, GridControl theGrid)
        {
            BindingList<Record> listDataSource = new BindingList<Record>();

            foreach (Record item in records)
            {
                listDataSource.Add(item);
            }
            
            theGrid.DataSource = listDataSource;
        }

        public static void AddNewRecord(Record record, GridControl theGrid)
        {
            BindingList<Record> listDataSource = theGrid.DataSource as BindingList<Record>;

            listDataSource.Add(record);

            theGrid.DataSource = listDataSource;
        }


        public static List<Record> GetRecords()
        {

            List<Record> recordList = new List<Record>();

            string readText = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\ScannedPDFs\\" + "RecordList.txt");

            recordList = JsonConvert.DeserializeObject<List<Record>>(readText);

            foreach (var item in recordList)
            {
                item.BuildStringList();
            }

            return recordList;

        }

        public static void SaveRecordFile(List<Record> recordList)
        {
            string json = JsonConvert.SerializeObject(recordList, Formatting.Indented);

            int stop = 0;

            System.IO.File.WriteAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\ScannedPDFs\\" + "RecordList.txt", json);

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"RecordList.txt", true))
            //{
            //    file.WriteLine(json);
            //}



        }

        public static void SwapCards(Card top, Card bottom)
        {

            Page topPage = top.Page;
            Page bottomPage = bottom.Page;

            if (topPage != null)
            {
                topPage.Card = bottom;
            }

            if (bottomPage != null)
            {
                bottomPage.Card = top;
            }

            top.Page = bottomPage;
            bottom.Page = topPage;

            object p = bottom.Parent;

            bottom.Parent = top.Parent;

            top.Parent = (Control)p;
        }

        public static void UpdateRankList(List<PDFScanAndSort.Models.Application> apps, List<Card> cards)
        {
            Ranker ranker = new Ranker(apps, cards);
            
            ranker.RankCards();

            var grpApps = ranker.RanksList
          .GroupBy(u => u.Page)
          .Select(grp => grp.ToList())
          .ToList();

            foreach (var item in grpApps)
            {
                var grpPage = item
                   .GroupBy(u => u.card)
                   .Select(grp => grp.ToList())
                   .ToList();

                Card winningCard = null;
                int winningGrpnum = 0;
                int currentwinningGrpnum = 0;

                foreach (var grpItem in grpPage)
                {
                    currentwinningGrpnum = grpItem.Count;

                    if (winningGrpnum < currentwinningGrpnum)
                    {
                        winningGrpnum = currentwinningGrpnum;
                        winningCard = grpItem[0].card;
                    }

                }

                if (winningCard != null)
                {
                    GridHelper.SwapCards(item[0].Page.Card, winningCard);
                }
            }

        }

    }
}
