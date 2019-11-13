using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using ConvertorToXmlByM.E.Doc.XML;
using ExcelDataReader;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;

namespace ConvertorToXmlByM.E.Doc.ViewModels
{
    class J029500ViewModel : ViewModelBase
    {
        private string _filename = "";

        public DelegateCommand StartWorkCommand { get; }

        public DelegateCommand LoadFileCommand { get; }

        private DataSet _data;

        public DataView Data => _data?.Tables[0].DefaultView;

        private string _xmlFilename = "";
        public string XmlFilename
        {
            get => _xmlFilename;
            set
            {
                _xmlFilename = value;
                RaisePropertyChanged();
            }
        }

        private DataColumnCollection _columnName;

        public DataColumnCollection ColumnName
        {
            get => _columnName;
            set
            {
                _columnName = value;
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        public J029500ViewModel()
        {
            Period = DateTime.Now;
            StartWorkCommand = new DelegateCommand(StartWork);
            LoadFileCommand = new DelegateCommand(LoadFile);
        }

        private void StartWork()
        {
            string fileName = "XML_AN.d1.1_" + DateTime.Now + ".xml";
            string tempFileName = fileName;
            fileName = "";
            for (int i = 0; i < tempFileName.Length; i++)
            {
                if (tempFileName[i] == ':') i++;
                fileName += tempFileName[i];
            }
            try
            {
                J029500Xml j029500 = new J029500Xml(Period.Value, _data, SelectedColumn, XmlFilename);
                _data = new DataSet();
                DataTable temp = new DataTable();
                temp.Columns.Add("Склад");
                temp.Columns.Add("Код УКТ ЗЕД");
                temp.Columns.Add("Остаток на начало");
                temp.Columns.Add("Полученно");
                temp.Columns.Add("Реализованно");
                temp.Columns.Add("Заявка на поступление");

                foreach (var store in j029500.Body)
                {
                    temp.Rows.Add(store.T1Rxxxxg1, store.T1Rxxxxg2, store.T1Rxxxxg3, store.T1Rxxxxg4, store.T1Rxxxxg5, store.T1Rxxxxg12);
                }

                _data.Tables.Add(temp);
                RaisePropertyChanged("Data");

                FileStream fs = File.Create(fileName);
                fs.Close();
                j029500.Write(fileName);

                MessageBox.Show("Результат сохранен в файл " + fileName, "Успех!", MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
            }
            catch (Exception e)
            {
                File.Delete(fileName);
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void LoadFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel и Xml файлы|*.xlsx;*.xls;*.xml",
                    InitialDirectory = @"D:\",
                    ShowReadOnly = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = true
                };
                if (openFileDialog.ShowDialog() != true) return;

                foreach (var fileName in openFileDialog.FileNames)
                {
                    if (fileName.Contains(".xml"))
                        XmlFilename = fileName;
                    else if (fileName.Contains(".xlsx") || fileName.Contains(".xls"))
                        _filename = fileName;
                }

                SelectedColumn = new Dictionary<string, string>(5)
                {
                    {"EDRPOU", "ЄДРПОУ" },
                    {"Act", "Номер" },
                    {"Direction", "Напрямок" },
                    {"RegistrationDate", "Дата та час реєстрації"},
                    {"RegistrationNumber", "Реєстраційний номер"},
                    {"ProductСode", "Код УКТ ЗЕД (АН,РК-1)"},
                    {"VolumeLiters", "Обсяг (АН,РК-1) (в л)"},
                    {"ExciseWarehouseFrom", "Акцизний склад, з якого відвантажено паливо"},
                    {"MobileExciseWarehouseFrom", "Пересувний акцизний склад, з якого відвантажно паливо - Реєстраційний номер"},
                    {"ExciseWarehouseTo", "Акцизний склад, на який отримано паливо"},
                    {"MobileExciseWarehouseTo", "Пересувний акцизний склад, на який отримано паливо - Реєстраційний номер"}
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

                        RaisePropertyChanged(nameof(Data));
                        RaisePropertyChanged(nameof(ColumnName));
                        RaisePropertyChanged(nameof(SelectedColumn));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        #region INavigationAware

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
