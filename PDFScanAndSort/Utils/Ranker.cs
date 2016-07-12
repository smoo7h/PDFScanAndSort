using PDFScanAndSort.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFScanAndSort.Utils
{
    public class Ranker
    {
        public Ranker(List<Application> apps, List<Card> cards)
        {
            RanksList = new List<Rank>();
            Pages = new List<Page>();

            this.Applications = apps;
            this.Cards = cards;

            MakePageList();

        }

        public List<Card> Cards { get; set; }

        public List<Rank> RanksList { get; set; }

        public List<Application> Applications { get; set; }

        public List<Page> Pages { get; set; }

        public List<Rank> RankCards()
        {
            foreach (Page page in Pages)
            {
                foreach (String s in page.SearchStrings)
                {
                    foreach (Card c in Cards)
                    {
                        
                        if (c.PageText != null && c.PageText.Contains(s))
                        {
                            Rank r = new Rank();
                            r.card = c;
                            r.Page = page;
                            r.TextFound = s;
                            RanksList.Add(r);
                        }
                    }
                }
            }
            return RanksList;
        }

        public void MakePageList()
        {
            foreach (Application a in Applications)
            {
                foreach (Page p in a.Pages)
                {
                    Pages.Add(p);
                }
            }          
        }

    }
}
