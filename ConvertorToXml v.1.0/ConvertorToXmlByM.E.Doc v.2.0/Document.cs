using System;

namespace ConvertorToXmlByM.E.Doc_v._2._0
{
    public class Document
    {
        public string ProductCode = "";
        public string ProductDesc = "";
        public string Unit = "";
        public double CountProduct;
        public string PrivilegeCode = "";
        public double FromTaxable;
        public string PayerName = "";
        public DateTime? СrossingDate;
        public string Act = "";
        public string Type = "";

        public Document() { }

        public Document(string productCode = "2707109000", string productDesc = "Бензол", string unit = "л",
            double countProduct = 0, string privilegeCode = "14020030", double fromTaxable = 0,
            string payerName = "", DateTime? сrossingDate = null, string act = "", string type = "митна декларація")
        {
            ProductCode = productCode;
            ProductDesc = productDesc;
            Unit = unit;
            CountProduct = countProduct;
            PrivilegeCode = privilegeCode;
            FromTaxable = fromTaxable;
            PayerName = payerName;
            СrossingDate = сrossingDate;
            Act = act;
            Type = type;
        }
    }
}
