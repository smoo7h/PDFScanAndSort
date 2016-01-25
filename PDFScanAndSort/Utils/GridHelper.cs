﻿using DevExpress.XtraGrid;
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

            string readText = File.ReadAllText(@"RecordList.txt");

            recordList = JsonConvert.DeserializeObject<List<Record>>(readText);


            return recordList;

        }

        public static void SaveRecordFile(List<Record> recordList)
        {
            string json = JsonConvert.SerializeObject(recordList, Formatting.Indented);

            int stop = 0;

            System.IO.File.WriteAllText(@"RecordList.txt", json);

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"RecordList.txt", true))
            //{
            //    file.WriteLine(json);
            //}



        }





    }
}