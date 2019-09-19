using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Application = Microsoft.Office.Interop.Excel.Application;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;

namespace ConvertorToXml_v._1._0
{
    public partial class Form1 : Form
    {

        public string NameExcel;
        public Range range_PayerName;           //Имя плательщика
        public Range range_VolumeLiters;        //Объем в литрах
        public Range range_AmountOfExciseTax;   //Сумма акцизного налога, не уплаченная по операциям, не подлежащим налогообложению, грн
        public Range range_Document_Number;     //Номер документа ГТД
        public Range range_DateCrossingBorder; //Дата пересечения границы
        public Range range_Result;              //Итог
        public Form1()
        {
            InitializeComponent();
        }

        public void GetNameExcel()
        {
            NameExcel = OpenExcel.FileName;
        }

        //Получение местонахождения необходимых столбцов
        public void GetRange(Microsoft.Office.Interop.Excel.Worksheet OS)
        {
            string string_PayerName = "Имя плательщика";
            string string_VolumeLiters = "Объем в литрах";
            string string_AmountOfExciseTax = "Сумма акцизного налога, не уплаченная по операциям, не подлежащим налогообложению, грн";
            string string_Document_Number = "Номер документа ГТД";
            string string_DateCrossingBorder = "Дата пересечения границы";
            string string_Result = "Итого";
            range_PayerName = OS.UsedRange.Find(string_PayerName);
            range_VolumeLiters = OS.UsedRange.Find(string_VolumeLiters);
            range_AmountOfExciseTax = OS.UsedRange.Find(string_AmountOfExciseTax);
            range_Document_Number = OS.UsedRange.Find(string_Document_Number);
            range_DateCrossingBorder = OS.UsedRange.Find(string_DateCrossingBorder);
            range_Result = OS.UsedRange.Find(string_Result);
        }

        [Serializable]
        public class PackXml : Form1
        {
            public int rownum;
            public string TIN;
            public string C_DOC;
            public string C_DOC_SUB;
            public string C_DOC_VER;
            public string C_DOC_TYPE;
            public string C_DOC_CNT;
            public string C_REG;
            public string C_RAJ;
            public string PERIOD_MONTH;
            public string PERIOD_TYPE;
            public string PERIOD_YEAR;
            public string C_STI_ORIG;
            public string C_DOC_STAN;
            public string LINKED_DOCS;
            public string D_FILL;
            public string SOFTWARE;
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
            }
            public Body[] body; //Загружаемый массив основных данных
            public string R01G9;
            public string R01G10;
            public string R01G11;

            //Создание загружаемой таблицы
            public PackXml(int rownum, int count, List<uint> items, Microsoft.Office.Interop.Excel.Worksheet OS, Form1 obj)
            {
                this.rownum = rownum;
                TIN = "00191075";
                C_DOC = "J02";
                C_DOC_SUB = "954";
                C_DOC_VER = "6";
                C_DOC_TYPE = "0";
                C_DOC_CNT = "0";
                C_REG = "";
                C_RAJ = "";
                PERIOD_TYPE = "1";
                Сhoice_Period(obj);
                C_STI_ORIG = "";
                C_DOC_STAN = "1";
                D_FILL = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                SOFTWARE = "";
                body = new Body[this.rownum];
                double result = 0;
                for (int i = obj.range_VolumeLiters.Row+1, j = 0, l = 0; i < count; i++)
                {
                    if (l == items.Count)
                    {
                        body[j].T1RXXXXG2S = "2707109000";
                        body[j].T1RXXXXG3S = "Бензол";
                        body[j].T1RXXXXG4S = "л";
                        body[j].T1RXXXXG5 = OS.Cells[i, obj.range_VolumeLiters.Column].Value.ToString();
                        body[j].T1RXXXXG8 = "14020030";
                        double temp_g10 = OS.Cells[i, obj.range_AmountOfExciseTax.Column].Value;
                        body[j].T1RXXXXG10 = Math.Round(temp_g10, 2).ToString();
                        result += OS.Cells[i, obj.range_AmountOfExciseTax.Column].Value;
                        body[j].T1RXXXXG12S = OS.Cells[i, obj.range_PayerName.Column].Value.ToString();
                        body[j].T1RXXXXG14D = OS.Cells[i, obj.range_DateCrossingBorder.Column].Value.ToString();
                        body[j].T1RXXXXG15S = OS.Cells[i, obj.range_Document_Number.Column].Value.ToString();
                        body[j].T1RXXXXG16S = "митна декларація";
                        j++;
                    }
                    else
                    {
                        if (items[l] != i)
                        {
                            body[j].T1RXXXXG2S = "2707109000";
                            body[j].T1RXXXXG3S = "Бензол";
                            body[j].T1RXXXXG4S = "л";
                            body[j].T1RXXXXG5 = OS.Cells[i, obj.range_VolumeLiters.Column].Value.ToString();
                            body[j].T1RXXXXG8 = "14020030";
                            double temp_g10 = OS.Cells[i, obj.range_AmountOfExciseTax.Column].Value;
                            body[j].T1RXXXXG10 = Math.Round(temp_g10, 2).ToString();
                            result += OS.Cells[i, obj.range_AmountOfExciseTax.Column].Value;
                            body[j].T1RXXXXG12S = OS.Cells[i, obj.range_PayerName.Column].Value.ToString();
                            body[j].T1RXXXXG14D = OS.Cells[i, obj.range_DateCrossingBorder.Column].Value.ToString();
                            body[j].T1RXXXXG15S = OS.Cells[i, obj.range_Document_Number.Column].Value.ToString();
                            body[j].T1RXXXXG16S = "митна декларація";
                            j++;
                        }
                        else l++;
                    }
                }
                R01G10 = result.ToString();
            }

            private void Сhoice_Period(Form1 obj)
            {
                PERIOD_MONTH = obj.MonthUpDown.Value.ToString();
                PERIOD_YEAR = obj.YearUpDown.Value.ToString();
            }
        }

        static void Writer_Head(PackXml obj, XmlWriter writer)
        {
            writer.WriteStartElement("DECLARHEAD");
            writer.WriteStartElement("TIN");
            writer.WriteString(obj.TIN);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC");
            writer.WriteString(obj.C_DOC);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC_SUB");
            writer.WriteString(obj.C_DOC_SUB);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC_VER");
            writer.WriteString(obj.C_DOC_VER);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC_TYPE");
            writer.WriteString(obj.C_DOC_TYPE);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC_CNT");
            writer.WriteString(obj.C_DOC_CNT);
            writer.WriteEndElement();
            writer.WriteStartElement("C_REG");
            writer.WriteString(obj.C_REG);
            writer.WriteEndElement();
            writer.WriteStartElement("C_RAJ");
            writer.WriteString(obj.C_RAJ);
            writer.WriteEndElement();
            writer.WriteStartElement("PERIOD_MONTH");
            writer.WriteString(obj.PERIOD_MONTH);
            writer.WriteEndElement();
            writer.WriteStartElement("PERIOD_TYPE");
            writer.WriteString(obj.PERIOD_TYPE);
            writer.WriteEndElement();
            writer.WriteStartElement("PERIOD_YEAR");
            writer.WriteString(obj.PERIOD_YEAR);
            writer.WriteEndElement();
            writer.WriteStartElement("C_STI_ORIG");
            writer.WriteString(obj.C_STI_ORIG);
            writer.WriteEndElement();
            writer.WriteStartElement("C_DOC_STAN");
            writer.WriteString(obj.C_DOC_STAN);
            writer.WriteEndElement();
            writer.WriteStartElement("LINKED_DOCS");
            writer.WriteString(obj.LINKED_DOCS);
            writer.WriteEndElement();
            writer.WriteStartElement("D_FILL");
            writer.WriteString(obj.D_FILL);
            writer.WriteEndElement();
            writer.WriteStartElement("SOFTWARE");
            writer.WriteString(obj.SOFTWARE);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        static void Writer_Body(PackXml obj, XmlWriter writer)
        {
            writer.WriteStartElement("DECLARBODY");
            for (int i = 0; i < obj.rownum; i++)
            {
                writer.WriteStartElement("T1RXXXXG2S");
                writer.WriteAttributeString("ROWNUM", (i+1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG2S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG3S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG3S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG4S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG4S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG5");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG5);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG6");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG6);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG7");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG7);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG8");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG8);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG9");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG9);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG10");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG10);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG11");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG11);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG12S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG12S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG13S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG13S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG14D");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG14D);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG15S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG15S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG16S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.body[i].T1RXXXXG16S);
                writer.WriteEndElement();
            }
            writer.WriteStartElement("R01G9");
            writer.WriteString(obj.R01G9);
            writer.WriteEndElement();
            writer.WriteStartElement("R01G10");
            writer.WriteString(obj.R01G10);
            writer.WriteEndElement();
            writer.WriteStartElement("R01G11");
            writer.WriteString(obj.R01G11);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        static void Serialize_XmlWriter(PackXml obj, string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(fileName, settings); 
            writer.WriteStartDocument();
            writer.WriteStartElement("DECLAR");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null , "J0295401.xsd");
            Writer_Head(obj, writer);
            Writer_Body(obj, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        //Получение кол-ва скрытых стро\столбцов(строки (true) или столбцы (false))
        public static List<uint> GetHiddenRowsOrCols(string fileName, string sheetName, bool detectRows)
        {
            List<uint> itemList = new List<uint>();
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);

                if (theSheet == null) return itemList;
                WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                DocumentFormat.OpenXml.Spreadsheet.Worksheet ws = wsPart.Worksheet;
                if (detectRows)
                {
                    itemList = ws.Descendants<Row>().Where(r => r.Hidden != null && r.Hidden.Value).Select(r => r.RowIndex.Value).ToList();
                }
                else
                {
                    var cols = ws.Descendants<Column>().Where(c => c.Hidden != null && c.Hidden.Value);
                    foreach (Column item in cols)
                    {
                        for (uint i = item.Min.Value; i <= item.Max.Value; i++)
                        {
                            itemList.Add(i);
                        }
                    }
                }
            }
        return itemList;
    }

        private void Main_Work()
        {

            int rownum = 0;
            if (OpenExcel.ShowDialog() != DialogResult.OK) return;
            GetNameExcel();

            var fileName = string.Format("{0}\\fileNameHere", Directory.GetCurrentDirectory());
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", fileName);

            var adapter = new OleDbDataAdapter("SELECT * FROM [workSheetNameHere$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "anyNameHere");

            System.Data.DataTable data = ds.Tables["anyNameHere"];

            Application objExcel = new Application();
            try
            {

                //Открываем книгу.    
                Workbook objWorkBook = objExcel.Workbooks.Open(NameExcel, 0, false, 5, "", "", false,
                    XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                //Выбираем таблицу(лист).
                Worksheet objWorkSheet = (Worksheet) objWorkBook.Sheets[1];
                string temp = objWorkSheet.Name;
                objExcel.Quit();
                List<uint> items = GetHiddenRowsOrCols(NameExcel, temp, true);

                objWorkBook = objExcel.Workbooks.Open(NameExcel, 0, false, 5, "", "", false,
                    XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                objWorkSheet = (Worksheet)objWorkBook.Sheets[1];

                GetRange(objWorkSheet);
                int count = range_VolumeLiters.Row + 1;
                int i = 0;
                do
                {
                    if (i == items.Count) rownum++;
                    else
                    {
                        if (items[i] != count)
                        {
                            rownum++;
                        }
                        else i++;
                    }

                    count++;
                } while (count != range_Result.Row);

                PackXml src = new PackXml(rownum, count, items, objWorkSheet, this);
                string file_name = "XML_AN.d4_" + DateTime.Now.ToString() + ".xml";
                string temp_file_name = file_name;
                file_name = "";
                for (i = 0; i < temp_file_name.Length; i++)
                {
                    if (temp_file_name[i] == ':') i++;
                    file_name += temp_file_name[i];
                }

                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady != true || d.DriveType != DriveType.Fixed || d.Name == "C:\\") continue;
                    string tempFileDirect = d.Name + file_name;
                    file_name = tempFileDirect;
                    break;
                }

                FileStream fs = File.Create(file_name);
                fs.Close();
                Serialize_XmlWriter(src, file_name);
                MessageBox.Show("Результат сохранен в файл " + file_name, "Успех!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!");
            }
            finally
            {
                objExcel.Quit();
            }
        }
        private void Button_Start_Click(object sender, EventArgs e)
        {
            Main_Work();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MonthUpDown.Value = DateTime.Now.Month;
            YearUpDown.Value = DateTime.Now.Year;
        }
    }
}
