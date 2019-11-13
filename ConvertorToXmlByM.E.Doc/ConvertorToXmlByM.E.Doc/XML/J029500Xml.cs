using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ConvertorToXmlByM.E.Doc.XML
{
    public class J029500Xml : BaseXml
    {
        private Dictionary<string, double> _taxRates = new Dictionary<string, double>
        {
            {"2707109000", 195},
            {"2707309000", 195},
            {"2710124194", 213.5},
            {"2710192100", 21},
            {"2710194300", 139.5},
            {"2711129700", 52},
            {"2901100010", 52},
            {"2901100090", 213.5}
        };

        public J029500Xml(DateTime period, DataSet dataSet, Dictionary<string, string> selectedColumn, string xmlFileName) : base(period, "J02", "950")
        {
            List<Store> stores = GenerateData(dataSet, xmlFileName, selectedColumn);

            foreach (var store in stores)
            {
                Body.Add(new TableBody(store.StoreCode, store.ProductCode,
                    Math.Round(store.Balance, 3).ToString(CultureInfo.InvariantCulture),
                    Math.Round(store.Received / 1000, 3).ToString(CultureInfo.InvariantCulture),
                    Math.Round(store.Implemented / 1000, 3).ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg9: store.TaxRate.ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg12: Math.Round(store.ReplenishmentApplication / 1000, 3)
                        .ToString(CultureInfo.InvariantCulture)));
            }
        }

        private List<Store> GenerateData(DataSet dataSet, string xmlFileName, Dictionary<string, string> selectedColumn)
        {
            List<Store> stores = new List<Store>();
            List<(string edrpou, string act)> сounterpartyInvoiceNumber = new List<(string edrpou, string act)>();
            foreach (var dataRow in dataSet.Tables[0].Select().Where(d =>
                !string.IsNullOrEmpty(d[selectedColumn["RegistrationNumber"]].ToString()) &&
                d[selectedColumn["RegistrationDate"]] is DateTime date && date.Year == int.Parse(PeriodYear) &&
                date.Month == int.Parse(PeriodMonth))) 
            {
                string edrpou = dataRow[selectedColumn["EDRPOU"]].ToString();
                string act = dataRow[selectedColumn["Act"]].ToString();
                
                if(сounterpartyInvoiceNumber.Any(c => c.edrpou == edrpou && c.act == act)) continue;

                сounterpartyInvoiceNumber.Add((edrpou, act));

                string productCode = dataRow[selectedColumn["ProductСode"]].ToString();
                string direction = dataRow[selectedColumn["Direction"]].ToString();
                string exciseWarehouseFrom = dataRow[selectedColumn["ExciseWarehouseFrom"]].ToString();
                string mobileExciseWarehouseFrom = dataRow[selectedColumn["MobileExciseWarehouseFrom"]].ToString();
                string exciseWarehouseTo = dataRow[selectedColumn["ExciseWarehouseTo"]].ToString();
                string mobileExciseWarehouseTo = dataRow[selectedColumn["MobileExciseWarehouseTo"]].ToString();
                double volumeLiters = (double) dataRow[selectedColumn["VolumeLiters"]];

                if (direction == "Виданий")
                {
                    if (exciseWarehouseFrom != string.Empty)
                    {
                        if (!stores.Any(s => s.StoreCode == exciseWarehouseFrom && s.ProductCode == productCode))
                            stores.Add(new Store(exciseWarehouseFrom, productCode, _taxRates.First( t => t.Key == productCode).Value));
                        Store store = stores.First(s =>
                            s.StoreCode == exciseWarehouseFrom && s.ProductCode == productCode);
                        if (string.IsNullOrEmpty(edrpou))
                            store.ReplenishmentApplication += volumeLiters;
                        else
                            store.Implemented += volumeLiters;
                    }
                    else if (mobileExciseWarehouseFrom != string.Empty)
                    {
                        if (!stores.Any(s => s.StoreCode == mobileExciseWarehouseFrom && s.ProductCode == productCode))
                            stores.Add(new Store(mobileExciseWarehouseFrom, productCode, _taxRates.First(t => t.Key == productCode).Value));
                        Store store = stores.First(s =>
                            s.StoreCode == mobileExciseWarehouseFrom && s.ProductCode == productCode);
                        store.Implemented += volumeLiters;
                    }
                }
                if (exciseWarehouseTo != string.Empty)
                {
                    if (!stores.Any(s => s.StoreCode == exciseWarehouseTo && s.ProductCode == productCode))
                        stores.Add(new Store(exciseWarehouseTo, productCode, _taxRates.First(t => t.Key == productCode).Value));
                    stores.First(s => s.StoreCode == exciseWarehouseTo && s.ProductCode == productCode).Received +=
                        volumeLiters;
                }
                else if (mobileExciseWarehouseTo != string.Empty)
                {
                    if (!stores.Any(s => s.StoreCode == mobileExciseWarehouseTo && s.ProductCode == productCode))
                        stores.Add(new Store(mobileExciseWarehouseTo, productCode, _taxRates.First(t => t.Key == productCode).Value));
                    stores.First(s => s.StoreCode == mobileExciseWarehouseTo && s.ProductCode == productCode)
                        .Received += volumeLiters;
                }
            }

            XDocument document = XDocument.Load(xmlFileName);

            var declarGroup = from declar in document.Element("DECLAR").Element("DECLARBODY").Elements()
                group declar by declar.Attribute("ROWNUM").Value;

            foreach (var group in declarGroup)
            {
                double balance = double.Parse(group.Single(e => e.Name == "T1RXXXXG3").Value,
                                     NumberStyles.AllowDecimalPoint,
                                     new NumberFormatInfo {NumberDecimalSeparator = "."}) +
                                 double.Parse(group.Single(e => e.Name == "T1RXXXXG4").Value,
                                     NumberStyles.AllowDecimalPoint,
                                     new NumberFormatInfo {NumberDecimalSeparator = "."}) -
                                 double.Parse(group.Single(e => e.Name == "T1RXXXXG5").Value,
                                     NumberStyles.AllowDecimalPoint,
                                     new NumberFormatInfo {NumberDecimalSeparator = "."});

                if (balance < 0.001)
                    continue;

                var storeCode = group.Single(e => e.Name == "T1RXXXXG1S").Value;
                var productCode = group.Single(e => e.Name == "T1RXXXXG2S").Value;

                Store temp = stores.SingleOrDefault(s => s.StoreCode == storeCode && s.ProductCode == productCode);
                if (temp == null)
                {
                    temp = new Store(storeCode, productCode, _taxRates.First(t => t.Key == productCode).Value);
                    stores.Add(temp);
                }

                temp.Balance = balance;
            }
            return stores;
        }

        protected override void WriteBody(XElement declarBody)
        {
            for (int i = 0; i < Body.Count; i++)
            {
                XAttribute rownum = new XAttribute("ROWNUM", (i + 1).ToString());

                declarBody.Add(
                    new XElement("T1RXXXXG1S", Body[i].T1Rxxxxg1, rownum),
                    new XElement("T1RXXXXG2S", Body[i].T1Rxxxxg2, rownum),
                    new XElement("T1RXXXXG3", Body[i].T1Rxxxxg3, rownum),
                    new XElement("T1RXXXXG4", Body[i].T1Rxxxxg4, rownum),
                    new XElement("T1RXXXXG5", Body[i].T1Rxxxxg5, rownum),
                    new XElement("T1RXXXXG6", Body[i].T1Rxxxxg6, rownum),
                    new XElement("T1RXXXXG7", Body[i].T1Rxxxxg7, rownum),
                    new XElement("T1RXXXXG8", Body[i].T1Rxxxxg8, rownum),
                    new XElement("T1RXXXXG9", Body[i].T1Rxxxxg9, rownum),
                    new XElement("T1RXXXXG10", Body[i].T1Rxxxxg10, rownum),
                    new XElement("T1RXXXXG11", Body[i].T1Rxxxxg11, rownum),
                    new XElement("T1RXXXXG12", Body[i].T1Rxxxxg12, rownum),
                    new XElement("T1RXXXXG13", Body[i].T1Rxxxxg13, rownum),
                    new XElement("T1RXXXXG14", Body[i].T1Rxxxxg14, rownum),
                    new XElement("T1RXXXXG15", Body[i].T1Rxxxxg15, rownum)
                    );
            }
        }
    }
}
