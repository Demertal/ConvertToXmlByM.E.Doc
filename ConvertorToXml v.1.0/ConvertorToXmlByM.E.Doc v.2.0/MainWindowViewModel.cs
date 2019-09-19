using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;
using ExcelDataReader;
using Microsoft.Win32;

namespace ConvertorToXmlByM.E.Doc_v._2._0
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _filename = "";

        public RelayCommand StartWorkCommand { get; }

        public RelayCommand LoadFileCommand { get; }

        private DataSet _data;

        public DataView Data => _data?.Tables[0].DefaultView;

        private DataColumnCollection _columnName;

        public DataColumnCollection ColumnName
        {
            get => _columnName;
            set
            {
                _columnName = value;
                OnPropertyChanged("ColumnName");
            }
        }

        public Dictionary<string, string> SelectedColumn { get; set; }

        private DateTime? _period;
        public DateTime? Period
        {
            get => _period;
            set
            {
                _period = value;
                OnPropertyChanged("Period");
            }
        }

        public MainWindowViewModel()
        {
            Period = DateTime.Now;
            StartWorkCommand = new RelayCommand(obj => StartWork());
            LoadFileCommand = new RelayCommand(obj => LoadFile());
        }

        private void StartWork()
        {
            try
            {
                string fileName = "XML_AN.d4_" + DateTime.Now + ".xml";
                string tempFileName = fileName;
                fileName = "";
                for (int i = 0; i < tempFileName.Length; i++)
                {
                    if (tempFileName[i] == ':') i++;
                    fileName += tempFileName[i];
                }

                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady != true || d.DriveType != DriveType.Fixed || d.Name == "C:\\") continue;
                    string tempFileDirect = d.Name + fileName;
                    fileName = tempFileDirect;
                    break;
                }

                FileStream fs = File.Create(fileName);
                fs.Close();
                WriteXml(new PackXml(Period.Value, _data, SelectedColumn), fileName);
                MessageBox.Show("Результат сохранен в файл " + fileName, "Успех!", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void LoadFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel файл|*.xlsx;*.xls",
                    InitialDirectory = @"D:\",
                    ShowReadOnly = true,
                    CheckFileExists = true,
                    CheckPathExists = true
                };
                if (openFileDialog.ShowDialog() != true) return;
                _filename = openFileDialog.FileName;

                SelectedColumn = new Dictionary<string, string>(5)
                {
                    {"VolumeLiters", ""},
                    {"PayerName", ""},
                    {"AmountOfExciseTax", ""},
                    {"DocumentNumber", ""},
                    {"DateCrossingBorder", ""}
                };

                using (var stream = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        _data = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = tableReader => new ExcelDataTableConfiguration
                            {
                                EmptyColumnNamePrefix = "Column",
                                UseHeaderRow = true,
                                FilterColumn = (rowReader, columnIndex) => true
                            }
                        });

                        ColumnName = _data.Tables[0].Columns;
                        SelectedColumn["PayerName"] = "Имя плательщика";
                        SelectedColumn["VolumeLiters"] = "Объем в литрах";
                        SelectedColumn["AmountOfExciseTax"] = "Сумма АН, не упл. по операциям, не подл. н/о, грн.";
                        SelectedColumn["DocumentNumber"] = "Номер документа ГТД";
                        SelectedColumn["DateCrossingBorder"] = "Дата пересечения границы";
                        OnPropertyChanged("Data");
                        OnPropertyChanged("ColumnName");
                        OnPropertyChanged("SelectedColumn");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void WriteHead(PackXml obj, XmlWriter writer)
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
        private void WriteBody(PackXml obj, XmlWriter writer)
        {
            writer.WriteStartElement("DECLARBODY");
            for (int i = 0; i < obj.Body.Count; i++)
            {
                writer.WriteStartElement("T1RXXXXG2S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG2S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG3S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG3S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG4S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG4S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG5");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG5);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG6");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG6);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG7");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG7);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG8");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG8);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG9");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG9);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG10");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG10);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG11");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG11);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG12S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG12S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG13S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG13S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG14D");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG14D);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG15S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG15S);
                writer.WriteEndElement();
                writer.WriteStartElement("T1RXXXXG16S");
                writer.WriteAttributeString("ROWNUM", (i + 1).ToString());
                writer.WriteString(obj.Body[i].T1RXXXXG16S);
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
        private void WriteXml(PackXml obj, string fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};
            XmlWriter writer = XmlWriter.Create(fileName, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("DECLAR");
            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "J0295401.xsd");
            WriteHead(obj, writer);
            WriteBody(obj, writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
