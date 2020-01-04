using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ConvertorToXmlByM.E.Doc.Models.XML
{
    public abstract class BaseXml
    {
        public string Tin = "00191075";
        public string CDoc;
        public string CDocSub;
        public string CDocVer;
        public string CDocType = "0";
        public string CDocCnt = "0";
        public string CReg = "";
        public string CRaj = "";
        public string PeriodType = "1";
        public string PeriodMonth;
        public string PeriodYear;
        public string CStiOrig = "";
        public string CDocStan = "1";
        public string LinkedDocs = "";
        public string DFill;
        public string Software = "";
        public List<TableBody> Body = new List<TableBody>();
        public string R01G9 = "";
        public string R01G10 = "";
        public string R01G11 = "";

        protected BaseXml(DateTime period, string cDoc, string cDocSub, string cDocVer = "7")
        {
            CDoc = cDoc;
            CDocSub = cDocSub;
            CDocVer = cDocVer;
            DateTime now = DateTime.Now;
            PeriodMonth = period.Month.ToString();
            PeriodYear = period.Year.ToString();
            DFill = now.Day.ToString() + now.Month + now.Year;
        }

        protected abstract void WriteBody(XElement declarBody);

        public void Write(string fileName)
        {
            XDocument xdoc = new XDocument();
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XElement declar = new XElement("DECLAR",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(xsi + "noNamespaceSchemaLocation", CDoc + CDocSub + "01.xsd"));

            XElement declarHead = new XElement("DECLARHEAD",
                new XElement("TIN", Tin),
                new XElement("C_DOC", CDoc),
                new XElement("C_DOC_SUB", CDocSub),
                new XElement("C_DOC_VER", CDocVer),
                new XElement("C_DOC_TYPE", CDocType),
                new XElement("C_DOC_CNT", CDocCnt),
                new XElement("C_REG", CReg),
                new XElement("C_RAJ", CRaj),
                new XElement("PERIOD_MONTH", PeriodMonth),
                new XElement("PERIOD_TYPE", PeriodType),
                new XElement("PERIOD_YEAR", PeriodYear),
                new XElement("C_STI_ORIG", CStiOrig),
                new XElement("C_DOC_STAN", CDocStan),
                new XElement("LINKED_DOCS", LinkedDocs),
                new XElement("D_FILL", DFill),
                new XElement("SOFTWARE", Software));

            XElement declarbody = new XElement("DECLARBODY");
            WriteBody(declarbody);

            declar.Add(declarHead, declarbody);

            xdoc.Add(declar);

            xdoc.Save(fileName);
        }
    }
}
