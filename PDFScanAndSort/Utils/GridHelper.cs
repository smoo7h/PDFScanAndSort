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

            //string readText = File.ReadAllText(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "RecordList.txt");

            string readText = File.ReadAllText("RecordList.txt");

            
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

            System.IO.File.WriteAllText("RecordList.txt", json);




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


            if (top.Page == null && top.ImageLocation == null)
            {
                top.Parent.Parent = null;
                top.Parent = null;
                top = null;
            }

            if (bottom.Page == null && bottom.ImageLocation == null)
            {
                bottom.Parent.Parent = null;
                bottom.Parent = null;
                bottom = null;
            }


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

        public static void OrderBottomPanel(Form1 form)
        {
            Control[] btmControlLst = form.Controls.Find("fLPItemNotFound", true);
            bool check = false;

            FlowLayoutPanel btmPanel = btmControlLst[0] as FlowLayoutPanel;

            foreach (var item in btmPanel.Controls)
            {
                if (((item as FlowLayoutPanel).Controls[0] as Card).ImageLocation == null)
                {
                   // btmPanel.Controls.SetChildIndex((item as Control), btmPanel.Controls.Count - 1);      
                  //  btmPanel.Controls.Remove((item as Control));
                    check = true;
                }
            }

          

        }

        public static string GetClientName(List<PDFScanAndSort.Models.Application> apps)
        {
            string result = "";
            int pFrom;
            int pTo;
            string[] appsWithNames = { "Building Owner Manager Application", "Resident Application" };

            foreach (var app in appsWithNames)
            {
                PDFScanAndSort.Models.Application a = apps.Find(item => item.Name == app);

                if (a.Pages[0].Card.PageText != null)
                {
                    string pText = a.Pages[0].Card.PageText;

                    if (app == "Building Owner Manager Application" && pText != null && pText != "")
                    {
                        pFrom = pText.IndexOf("tenant, ") + "tenant, ".Length;
                        pTo = pText.IndexOf(" (");

                        int words = pText.Substring(pFrom, pTo - pFrom).Trim().Split(' ').Length;

                        if (words == 2)
                        {
                            result = pText.Substring(pFrom, pTo - pFrom).Trim();
                        }

                    }
                    else if (app == "Resident Application" && pText != null && pText != "")
                    {
                        pFrom = pText.IndexOf("Dear ") + "Dear ".Length;
                        pTo = pText.LastIndexOf(",\n\nAttached");

                        int words = pText.Substring(pFrom, pTo - pFrom).Trim().Split(' ').Length;

                        if (words == 2)
                        {
                            result = pText.Substring(pFrom, pTo - pFrom).Trim();
                        }
                    }

                }
            }
            //add quote for some reason u have 2 

            if (result != "")
            {
                result = "\"" + result + "\"";
            }

            return result;
        }

        public static void RankCards(Form1 form)
        {

            Ranker ranker = new Ranker(form.applications, form.cards);

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
