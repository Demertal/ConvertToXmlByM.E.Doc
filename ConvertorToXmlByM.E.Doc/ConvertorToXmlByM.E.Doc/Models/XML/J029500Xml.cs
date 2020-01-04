using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ConvertorToXmlByM.E.Doc.Properties;

namespace ConvertorToXmlByM.E.Doc.Models.XML
{
    public class J029500Xml : BaseXml
    {
        private readonly Dictionary<string, double> _taxRates = new Dictionary<string, double>
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
            List<Store> stores = GenerateData(dataSet, selectedColumn);

            SetBalanceStores(stores, xmlFileName);

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

        private List<Store> GenerateData(DataSet dataSet, Dictionary<string, string> selectedColumn)
        {
            var rows = from d in dataSet.Tables[0].Select()
                where !string.IsNullOrEmpty(d[selectedColumn[Resources.RegistrationNumber]].ToString())
                where d[selectedColumn[Resources.RegistrationDate]] is DateTime date &&
                      date.Year == int.Parse(PeriodYear) &&
                      date.Month == int.Parse(PeriodMonth)
                select new
                {
                    edrpou = d[selectedColumn[Resources.EDRPOU]].ToString().Trim(),
                    act = d[selectedColumn[Resources.Act]].ToString().Trim(),
                    direction = d[selectedColumn[Resources.Direction]].ToString().Trim(),
                    storageFrom = d[selectedColumn[Resources.ExciseWarehouseFrom]].ToString().Trim(),
                    storageMobileFrom = d[selectedColumn[Resources.MobileExciseWarehouseFrom]].ToString().Trim(),
                    storageTo = d[selectedColumn[Resources.ExciseWarehouseTo]].ToString().Trim(),
                    storageMobileTo = d[selectedColumn[Resources.MobileExciseWarehouseTo]].ToString().Trim(),
                    productCode1 = d[selectedColumn[Resources.ProductСode1]].ToString().Trim(),
                    volumeLiters1 = (double) d[selectedColumn[Resources.VolumeLiters1]],
                    productCode2 = d[selectedColumn[Resources.ProductСode2]].ToString().Trim(),
                    volumeLiters2 = double.TryParse(d[selectedColumn[Resources.VolumeLiters2]].ToString(), out var tresult) ? (double?)tresult : null
                };

            List<Store> stores = new List<Store>();
            List<(string edrpou, string act)> сounterpartyInvoiceNumber = new List<(string, string)>();

            foreach (var row in rows) 
            {
                if(сounterpartyInvoiceNumber.Any(c => c.edrpou == row.edrpou && c.act == row.act)) continue;

                сounterpartyInvoiceNumber.Add((row.edrpou, row.act));

                if (row.direction == "Виданий")
                {
                    if (row.storageFrom != string.Empty)
                    {
                        if (string.IsNullOrEmpty(row.edrpou))
                            SetStoreReplenishmentApplication(stores, row.storageFrom, row.productCode1, row.volumeLiters1, row.productCode2,
                                row.volumeLiters2);
                        else
                            SetStoreImplemented(stores, row.storageFrom, row.productCode1, row.volumeLiters1, row.productCode2,
                                row.volumeLiters2);
                    }
                    else if (row.storageMobileFrom != string.Empty)
                    {
                        SetStoreImplemented(stores, row.storageMobileFrom, row.productCode1, row.volumeLiters1, row.productCode2,
                            row.volumeLiters2);
                    }
                }
                if (row.storageTo != string.Empty)
                {
                    SetStoreReceived(stores, row.storageTo, row.productCode1, row.volumeLiters1, row.productCode2,
                        row.volumeLiters2);
                }
                else if (row.storageMobileTo != string.Empty)
                {
                    SetStoreReceived(stores, row.storageMobileTo, row.productCode1, row.volumeLiters1, row.productCode2,
                        row.volumeLiters2);
                }
            }

            return stores;
        }

        private Store GetStore(List<Store> stores, string storeCode, string productCode)
        {
            Store store = stores.FirstOrDefault(s =>
                s.StoreCode == storeCode && s.ProductCode == productCode);

            if (store == null)
            {
                store = new Store(storeCode, productCode,
                    _taxRates.First(t => t.Key == productCode).Value);
                stores.Add(store);
            }

            return store;
        }

        private void SetStoreReceived(List<Store> stores, string storeCode, string productCode1, double volumeLiters1, string productCode2, double? volumeLiters2)
        {
            Store store = GetStore(stores, storeCode, productCode1);
            store.Received += volumeLiters1;

            if (string.IsNullOrEmpty(productCode2) || volumeLiters2 == null) return;

            store = GetStore(stores, storeCode, productCode2);
            store.Received += volumeLiters2.Value;
        }

        private void SetStoreImplemented(List<Store> stores, string storeCode, string productCode1, double volumeLiters1, string productCode2, double? volumeLiters2)
        {
            Store store = GetStore(stores, storeCode, productCode1);
            store.Implemented += volumeLiters1;

            if (string.IsNullOrEmpty(productCode2) || volumeLiters2 == null) return;

            store = GetStore(stores, storeCode, productCode2);
            store.Implemented += volumeLiters2.Value;
        }

        private void SetStoreReplenishmentApplication(List<Store> stores, string storeCode, string productCode1, double volumeLiters1, string productCode2, double? volumeLiters2)
        {
            Store store = GetStore(stores, storeCode, productCode1);
            store.ReplenishmentApplication += volumeLiters1;

            if (string.IsNullOrEmpty(productCode2) || volumeLiters2 == null) return;

            store = GetStore(stores, storeCode, productCode2);
            store.ReplenishmentApplication += volumeLiters2.Value;
        }

        private void SetBalanceStores(List<Store> stores, string xmlFileName)
        {
            XDocument document = XDocument.Load(xmlFileName);

            var declarGroup = from declar in document.Element("DECLAR")?.Element("DECLARBODY")?.Elements()
                group declar by declar.Attribute("ROWNUM")?.Value;

            var fmt = new NumberFormatInfo { NumberDecimalSeparator = ".", NegativeSign = "-" };

            foreach (var group in declarGroup)
            {
                double balance = double.Parse(group.Single(e => e.Name == "T1RXXXXG3").Value, fmt) +
                                 double.Parse(group.Single(e => e.Name == "T1RXXXXG4").Value, fmt) -
                                 double.Parse(group.Single(e => e.Name == "T1RXXXXG5").Value, fmt);

                if (balance < 0.001)
                    continue;

                var storeCode = group.Single(e => e.Name == "T1RXXXXG1S").Value;
                var productCode = group.Single(e => e.Name == "T1RXXXXG2S").Value;

                Store store = stores.SingleOrDefault(s => s.StoreCode == storeCode && s.ProductCode == productCode);
                if (store == null)
                {
                    store = new Store(storeCode, productCode, _taxRates.First(t => t.Key == productCode).Value);
                    stores.Add(store);
                }

                store.Balance = balance;
            }
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
