using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ConvertorToXmlByM.E.Doc.Models.XML
{
    public class J029540Xml : BaseXml
    {
        public J029540Xml(DateTime period, DataSet dataSet, Dictionary<string, string> selectedColumn) : base(period, "J02", "954")
        {
            double result = 0;
            List<Document> documents = GenerateData(dataSet, selectedColumn);
            foreach (var document in documents)
            {
                result += document.FromTaxable;

                Body.Add(new TableBody("", document.ProductCode, document.ProductDesc, document.Unit,
                    Math.Round(document.CountProduct, 3).ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg8: document.PrivilegeCode,
                    t1Rxxxxg9: Math.Round(document.FromTaxable).ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg11: document.PayerName, t1Rxxxxg13: document.СrossingDate.ToString(),
                    t1Rxxxxg14: document.Act, t1Rxxxxg15: document.Type));
            }

            R01G10 = result.ToString(CultureInfo.InvariantCulture);
        }

        private List<Document> GenerateData(DataSet dataSet, Dictionary<string, string> selectedColumn)
        {
            List<Document> documents = new List<Document>();
            foreach (var dataRow in dataSet.Tables[0].Select())
            {
                DateTime? сrossingDate = dataRow[selectedColumn["СrossingDate"]] as DateTime?;
                string payerName = dataRow[selectedColumn["PayerName"]].ToString();
                string act = dataRow[selectedColumn["Act"]].ToString();
                double countProduct = dataRow[selectedColumn["VolumeLiters"]] is DBNull ? 0 : (double) dataRow[selectedColumn["VolumeLiters"]];
                double fromTaxable = dataRow[selectedColumn["FromTaxable"]] is DBNull ? 0 : (double) dataRow[selectedColumn["FromTaxable"]];

                if (documents.All(d => d.Act != act))
                    documents.Add(new Document(сrossingDate: сrossingDate, payerName: payerName, act: act));
                Document document = documents.First(d => d.Act == act);
                document.CountProduct += countProduct;
                document.FromTaxable += fromTaxable;
            }

            return documents;
        }

        protected override void WriteBody(XElement declarBody)
        {
            for (int i = 0; i < Body.Count; i++)
            {
                XAttribute rownum = new XAttribute("ROWNUM", (i + 1).ToString());

                declarBody.Add(
                    new XElement("T1RXXXXG2S", Body[i].T1Rxxxxg2, rownum),
                    new XElement("T1RXXXXG3S", Body[i].T1Rxxxxg3, rownum),
                    new XElement("T1RXXXXG4S", Body[i].T1Rxxxxg4, rownum),
                    new XElement("T1RXXXXG5", Body[i].T1Rxxxxg5, rownum),
                    new XElement("T1RXXXXG6", Body[i].T1Rxxxxg6, rownum),
                    new XElement("T1RXXXXG7", Body[i].T1Rxxxxg7, rownum),
                    new XElement("T1RXXXXG8S", Body[i].T1Rxxxxg8, rownum),
                    new XElement("T1RXXXXG9", Body[i].T1Rxxxxg9, rownum),
                    new XElement("T1RXXXXG10", Body[i].T1Rxxxxg10, rownum),
                    new XElement("T1RXXXXG11S", Body[i].T1Rxxxxg11, rownum),
                    new XElement("T1RXXXXG12S", Body[i].T1Rxxxxg12, rownum),
                    new XElement("T1RXXXXG13D", Body[i].T1Rxxxxg13, rownum),
                    new XElement("T1RXXXXG14S", Body[i].T1Rxxxxg14, rownum),
                    new XElement("T1RXXXXG15S", Body[i].T1Rxxxxg15, rownum),
                    new XElement("T1RXXXXG16S", Body[i].T1Rxxxxg16, rownum));
            }

            declarBody.Add(
                new XElement("R01G9", R01G9),
                new XElement("R01G10", R01G10),
                new XElement("R01G11", R01G11)
                );
        }
    }
}
