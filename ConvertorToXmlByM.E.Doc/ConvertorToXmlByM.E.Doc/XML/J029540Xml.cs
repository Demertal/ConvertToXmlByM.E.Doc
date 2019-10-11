using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ConvertorToXmlByM.E.Doc.XML
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
                    t1Rxxxxg10: Math.Round(document.FromTaxable).ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg12: document.PayerName, t1Rxxxxg14: document.СrossingDate.ToString(),
                    t1Rxxxxg15: document.Act, t1Rxxxxg16: document.Type));
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
                double countProduct = (double) dataRow[selectedColumn["VolumeLiters"]];
                double fromTaxable = (double)dataRow[selectedColumn["FromTaxable"]];

                if (documents.All(d => d.Act != act))
                    documents.Add(new Document(сrossingDate: сrossingDate, payerName: payerName, act: act));
                Document document = documents.First(d => d.Act == act);
                document.CountProduct += countProduct;
                document.FromTaxable += fromTaxable;
            }

            return documents;
        }

        protected override void WriteBody(XmlWriter writer)
        {
            writer.WriteStartElement("DECLARBODY");
            for (int i = 0; i < Body.Count; i++)
            {
                string rownum = (i + 1).ToString();

                WriteElementWithAttribute(writer, "T1RXXXXG2S", Body[i].T1Rxxxxg2, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG3S", Body[i].T1Rxxxxg3, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG4S", Body[i].T1Rxxxxg4, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG5", Body[i].T1Rxxxxg5, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG6", Body[i].T1Rxxxxg6, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG7", Body[i].T1Rxxxxg7, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG8", Body[i].T1Rxxxxg8, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG9", Body[i].T1Rxxxxg9, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG10", Body[i].T1Rxxxxg10, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG11", Body[i].T1Rxxxxg11, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG12S", Body[i].T1Rxxxxg12, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG13S", Body[i].T1Rxxxxg13, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG14D", Body[i].T1Rxxxxg14, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG15S", Body[i].T1Rxxxxg15, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG16S", Body[i].T1Rxxxxg16, "ROWNUM", rownum);
            }

            WriteElement(writer, "R01G9", R01G9);
            WriteElement(writer, "R01G10", R01G10);
            WriteElement(writer, "R01G11", R01G11);

            writer.WriteEndElement();
        }
    }
}
