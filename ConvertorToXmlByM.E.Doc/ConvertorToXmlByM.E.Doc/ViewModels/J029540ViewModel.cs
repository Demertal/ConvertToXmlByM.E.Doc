using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using ConvertorToXmlByM.E.Doc.Models.XML;
using ExcelDataReader;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;

namespace ConvertorToXmlByM.E.Doc.ViewModels
{
    class J029540ViewModel : ViewModelBase
    {
        private string _filename = "";

        public DelegateCommand StartWorkCommand { get; }

        public DelegateCommand LoadFileCommand { get; }

        private DataSet _data;

        public DataView Data => _data?.Tables[0].DefaultView;

        private List<string> _columnNames;

        public List<string> ColumnNames
        {
            get => _columnNames;
            set
            {
                _columnNames = value;
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

        public J029540ViewModel()
        {
            Period = DateTime.Now;
            StartWorkCommand = new DelegateCommand(StartWork);
            LoadFileCommand = new DelegateCommand(LoadFile);
        }

        private void StartWork()
        {
            string fileName = "XML_AN.d4_" + DateTime.Now + ".xml";
            string tempFileName = fileName;
            fileName = "";
            for (int i = 0; i < tempFileName.Length; i++)
            {
                if (tempFileName[i] == ':') i++;
                fileName += tempFileName[i];
            }
            try
            {
                J029540Xml j029540 = new J029540Xml(Period.Value, _data, SelectedColumn);
                FileStream fs = File.Create(fileName);
                fs.Close();
                j029540.Write(fileName);

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
                    Filter = "Excel файл|*.xlsx;*.xls",
                    InitialDirectory = @"D:\",
                    ShowReadOnly = true,
                    CheckFileExists = true,
                    CheckPathExists = true
                };
                if (openFileDialog.ShowDialog() != true) return;
                _filename = openFileDialog.FileName;

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

                        ColumnNames = new List<string>();
                        foreach (DataColumn column in _data.Tables[0].Columns)
                        {
                            ColumnNames.Add(column.ColumnName);
                        }

                        SelectedColumn = new Dictionary<string, string>(5)
                        {
                            {"PayerName", "Имя плательщика"},
                            {"VolumeLiters", "Объем в литрах"},
                            {"FromTaxable", "Сумма АН, не упл. по операциям, не подл. н/о, грн."},
                            {"Act", "Номер документа ГТД"},
                            {"СrossingDate", "Дата пересечения границы"}
                        };

                        RaisePropertyChanged(nameof(ColumnNames));
                        RaisePropertyChanged(nameof(SelectedColumn));
                        RaisePropertyChanged(nameof(Data));
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
