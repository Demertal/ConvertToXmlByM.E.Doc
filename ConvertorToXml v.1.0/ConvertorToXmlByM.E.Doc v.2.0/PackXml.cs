using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConvertorToXmlByM.E.Doc_v._2._0
{
    public struct Body
    {
        public string T1RXXXXG2S;
        public string T1RXXXXG3S;
        public string T1RXXXXG4S;
        public string T1RXXXXG5;
        public string T1RXXXXG6;
        public string T1RXXXXG7;
        public string T1RXXXXG8;
        public string T1RXXXXG9;
        public string T1RXXXXG10;
        public string T1RXXXXG11;
        public string T1RXXXXG12S;
        public string T1RXXXXG13S;
        public string T1RXXXXG14D;
        public string T1RXXXXG15S;
        public string T1RXXXXG16S;

        public Body(string t1Rxxxxg2S = "2707109000", string t1Rxxxxg3S = "Бензол", string t1Rxxxxg4S = "л",
            string t1Rxxxxg5 = "", string t1Rxxxxg6 = "", string t1Rxxxxg7 = "", string t1Rxxxxg8 = "14020030",
            string t1Rxxxxg9 = "", string t1Rxxxxg10 = "", string t1Rxxxxg11 = "", string t1Rxxxxg12S = "",
            string t1Rxxxxg13S = "", string t1Rxxxxg14D = "", string t1Rxxxxg15S = "",
            string t1Rxxxxg16S = "митна декларація")
        {
            T1RXXXXG2S = t1Rxxxxg2S;
            T1RXXXXG3S = t1Rxxxxg3S;
            T1RXXXXG4S = t1Rxxxxg4S;
            T1RXXXXG5 = t1Rxxxxg5;
            T1RXXXXG6 = t1Rxxxxg6;
            T1RXXXXG7 = t1Rxxxxg7;
            T1RXXXXG8 = t1Rxxxxg8;
            T1RXXXXG9 = t1Rxxxxg9;
            T1RXXXXG10 = t1Rxxxxg10;
            T1RXXXXG11 = t1Rxxxxg11;
            T1RXXXXG12S = t1Rxxxxg12S;
            T1RXXXXG13S = t1Rxxxxg13S;
            T1RXXXXG14D = t1Rxxxxg14D;
            T1RXXXXG15S = t1Rxxxxg15S;
            T1RXXXXG16S = t1Rxxxxg16S;
        }
    }

    [Serializable]
    public class PackXml
    {
        public string TIN = "00191075";
        public string C_DOC = "J02";
        public string C_DOC_SUB = "954";
        public string C_DOC_VER = "6";
        public string C_DOC_TYPE = "0";
        public string C_DOC_CNT = "0";
        public string C_REG = "";
        public string C_RAJ = "";
        public string PERIOD_TYPE = "1";
        public string PERIOD_MONTH;
        public string PERIOD_YEAR;
        public string C_STI_ORIG = "";
        public string C_DOC_STAN = "1";
        public string LINKED_DOCS = "";
        public string D_FILL;
        public string SOFTWARE = "";
        public List<Body> Body = new List<Body>();
        public string R01G9 = "";
        public string R01G10;
        public string R01G11 = "";

        public PackXml(DateTime period, DataSet dataSet, Dictionary<string, string> selectedColumn)
        {
            DateTime now = DateTime.Now;
            PERIOD_MONTH = period.Month.ToString();
            PERIOD_YEAR = period.Year.ToString();
            D_FILL = now.Day.ToString() + now.Month + now.Year;
            double result = 0;
            foreach (var dataRow in dataSet.Tables[0].Select())
            {
                result += (double) dataRow[selectedColumn["AmountOfExciseTax"]];
                Body.Add(new Body(t1Rxxxxg5: dataRow[selectedColumn["VolumeLiters"]].ToString(),
                    t1Rxxxxg10: Math.Round((double) dataRow[selectedColumn["AmountOfExciseTax"]]).ToString(),
                    t1Rxxxxg12S: dataRow[selectedColumn["PayerName"]].ToString(),
                    t1Rxxxxg14D: dataRow[selectedColumn["DateCrossingBorder"]].ToString(),
                    t1Rxxxxg15S: dataRow[selectedColumn["DocumentNumber"]].ToString()));
            }

            R01G10 = result.ToString();
        }
    }
}
