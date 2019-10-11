using System;
using System.Collections.Generic;
using System.Xml;

namespace ConvertorToXmlByM.E.Doc_v._2._0.XML
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

        protected void WriteElement(XmlWriter writer, string key, string value)
        {
            writer.WriteStartElement(key);
            writer.WriteString(value);
            writer.WriteEndElement();
        }

        protected void WriteElementWithAttribute(XmlWriter writer, string keyElement, string valueElement,
            string keyAttribute, string valueAttribute)
        {
            writer.WriteStartElement(keyElement);
            writer.WriteAttributeString(keyAttribute, valueAttribute);
            writer.WriteString(valueElement);
            writer.WriteEndElement();
        }

        private void WriteHead(XmlWriter writer)
        {
            writer.WriteStartElement("DECLARHEAD");
            WriteElement(writer, "TIN", Tin);
            WriteElement(writer, "C_DOC", CDoc);
            WriteElement(writer, "C_DOC_SUB", CDocSub);
            WriteElement(writer, "C_DOC_VER", CDocVer);
            WriteElement(writer, "C_DOC_TYPE", CDocType);
            WriteElement(writer, "C_DOC_CNT", CDocCnt);
            WriteElement(writer, "C_REG", CReg);
            WriteElement(writer, "C_RAJ", CRaj);
            WriteElement(writer, "PERIOD_MONTH", PeriodMonth);
            WriteElement(writer, "PERIOD_TYPE", PeriodType);
            WriteElement(writer, "PERIOD_YEAR", PeriodYear);
            WriteElement(writer, "C_STI_ORIG", CStiOrig);
            WriteElement(writer, "C_DOC_STAN", CDocStan);
            WriteElement(writer, "LINKED_DOCS", LinkedDocs);
            WriteElement(writer, "D_FILL", DFill);
            WriteElement(writer, "SOFTWARE", Software);
            writer.WriteEndElement();
        }

        protected abstract void WriteBody(XmlWriter writer);

        public void Write(string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
            XmlWriter writer = XmlWriter.Create(fileName, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("DECLAR");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, CDoc + CDocSub + "01.xsd");
            WriteHead(writer);
            WriteBody(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
