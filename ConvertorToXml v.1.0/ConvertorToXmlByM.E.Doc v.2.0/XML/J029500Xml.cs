using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ConvertorToXmlByM.E.Doc_v._2._0.XML
{
    public class J029500Xml : BaseXml
    {
        public J029500Xml(DateTime period, DataSet dataSet, Dictionary<string, string> selectedColumn) : base(period, "J02", "950")
        {
            List<Store> stores = GenerateData(dataSet, selectedColumn);

            foreach (var store in stores)
            {
                Body.Add(new TableBody(store.StoreCode, store.ProductCode, "",
                    Math.Round(store.Received / 1000, 3).ToString(CultureInfo.InvariantCulture),
                    Math.Round(store.Implemented / 1000, 3).ToString(CultureInfo.InvariantCulture),
                    t1Rxxxxg12: Math.Round(store.ReplenishmentApplication / 1000, 3).ToString(CultureInfo.InvariantCulture)));
            }
        }

        private List<Store> GenerateData(DataSet dataSet, Dictionary<string, string> selectedColumn)
        {
            List<Store> stores = new List<Store>();
            List<(string edrpou, string act)> сounterpartyInvoiceNumber = new List<(string edrpou, string act)>();
            foreach (var dataRow in dataSet.Tables[0].Select())
            {
                DateTime? date = dataRow[selectedColumn["RegistrationDate"]] as DateTime?;
                string registrationNumber = dataRow[selectedColumn["RegistrationNumber"]].ToString();

                if (date == null || string.IsNullOrEmpty(registrationNumber) ||
                    date.Value.Year.ToString() != PeriodYear || date.Value.Month.ToString() != PeriodMonth) continue;

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
                            stores.Add(new Store(exciseWarehouseFrom, productCode));
                        Store temp = stores.First(s =>
                            s.StoreCode == exciseWarehouseFrom && s.ProductCode == productCode);
                        if (string.IsNullOrEmpty(edrpou))
                            temp.ReplenishmentApplication += volumeLiters;
                        else
                            temp.Implemented += volumeLiters;
                    }
                    else if (mobileExciseWarehouseFrom != string.Empty)
                    {
                        if (!stores.Any(s => s.StoreCode == mobileExciseWarehouseFrom && s.ProductCode == productCode))
                            stores.Add(new Store(mobileExciseWarehouseFrom, productCode));
                        stores.First(s => s.StoreCode == mobileExciseWarehouseFrom && s.ProductCode == productCode)
                            .Implemented += volumeLiters;
                    }
                }
                if (exciseWarehouseTo != string.Empty)
                {
                    if (!stores.Any(s => s.StoreCode == exciseWarehouseTo && s.ProductCode == productCode))
                        stores.Add(new Store(exciseWarehouseTo, productCode));
                    stores.First(s => s.StoreCode == exciseWarehouseTo && s.ProductCode == productCode).Received +=
                        volumeLiters;
                }
                else if (mobileExciseWarehouseTo != string.Empty)
                {
                    if (!stores.Any(s => s.StoreCode == mobileExciseWarehouseTo && s.ProductCode == productCode))
                        stores.Add(new Store(mobileExciseWarehouseTo, productCode));
                    stores.First(s => s.StoreCode == mobileExciseWarehouseTo && s.ProductCode == productCode)
                        .Received += volumeLiters;
                }
            }

            return stores;
        }

        protected override void WriteBody(XmlWriter writer)
        {
            writer.WriteStartElement("DECLARBODY");
            for (int i = 0; i < Body.Count; i++)
            {
                string rownum = (i + 1).ToString();

                WriteElementWithAttribute(writer, "T1RXXXXG1S", Body[i].T1Rxxxxg1, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG2S", Body[i].T1Rxxxxg2, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG3S", Body[i].T1Rxxxxg3, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG4", Body[i].T1Rxxxxg4, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG5", Body[i].T1Rxxxxg5, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG6", Body[i].T1Rxxxxg6, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG7", Body[i].T1Rxxxxg7, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG8", Body[i].T1Rxxxxg8, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG9", Body[i].T1Rxxxxg9, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG10", Body[i].T1Rxxxxg10, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG11", Body[i].T1Rxxxxg11, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG12", Body[i].T1Rxxxxg12, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG13", Body[i].T1Rxxxxg13, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG14", Body[i].T1Rxxxxg14, "ROWNUM", rownum);
                WriteElementWithAttribute(writer, "T1RXXXXG15", Body[i].T1Rxxxxg15, "ROWNUM", rownum);
            }

            //WriteElement(writer, "R01G9", obj.R01G9);
            //WriteElement(writer, "R01G10", obj.R01G10);
            //WriteElement(writer, "R01G11", obj.R01G11);

            writer.WriteEndElement();
        }
    }
}
