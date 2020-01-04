using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using ConvertorToXmlByM.E.Doc.Models.XML;
using ConvertorToXmlByM.E.Doc.Properties;
using ExcelDataReader;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Regions;

namespace ConvertorToXmlByM.E.Doc.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    class J029500ViewModel : ViewModelBase
    {
        private string _filename = "";

        private DataSet _data;

        #region Properties

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

        private string _edrpou;
        public string Edrpou
        {
            get => _edrpou;
            set
            {
                _edrpou = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _act;
        public string Act
        {
            get => _act;
            set
            {
                _act = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _direction;
        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _registrationDate;
        public string RegistrationDate
        {
            get => _registrationDate;
            set
            {
                _registrationDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _registrationNumber;
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                _registrationNumber = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _productСode1;
        public string ProductСode1
        {
            get => _productСode1;
            set
            {
                _productСode1 = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _volumeLiters1;
        public string VolumeLiters1
        {
            get => _volumeLiters1;
            set
            {
                _volumeLiters1 = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _productСode2;
        public string ProductСode2
        {
            get => _productСode2;
            set
            {
                _productСode2 = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _volumeLiters2;
        public string VolumeLiters2
        {
            get => _volumeLiters2;
            set
            {
                _volumeLiters2 = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _exciseWarehouseFrom;
        public string ExciseWarehouseFrom
        {
            get => _exciseWarehouseFrom;
            set
            {
                _exciseWarehouseFrom = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _mobileExciseWarehouseFrom;
        public string MobileExciseWarehouseFrom
        {
            get => _mobileExciseWarehouseFrom;
            set
            {
                _mobileExciseWarehouseFrom = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _exciseWarehouseTo;
        public string ExciseWarehouseTo
        {
            get => _exciseWarehouseTo;
            set
            {
                _exciseWarehouseTo = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        private string _mobileExciseWarehouseTo;
        public string MobileExciseWarehouseTo
        {
            get => _mobileExciseWarehouseTo;
            set
            {
                _mobileExciseWarehouseTo = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CanWork));
            }
        }

        public bool CanWork => Period != null && !string.IsNullOrEmpty(Edrpou) && !string.IsNullOrEmpty(Act) &&
                               !string.IsNullOrEmpty(Direction) && !string.IsNullOrEmpty(RegistrationDate) &&
                               !string.IsNullOrEmpty(RegistrationNumber) && !string.IsNullOrEmpty(ProductСode1) &&
                               !string.IsNullOrEmpty(VolumeLiters1) && !string.IsNullOrEmpty(ProductСode2) &&
                               !string.IsNullOrEmpty(VolumeLiters2) && !string.IsNullOrEmpty(ExciseWarehouseFrom) &&
                               !string.IsNullOrEmpty(MobileExciseWarehouseFrom) &&
                               !string.IsNullOrEmpty(ExciseWarehouseTo) &&
                               !string.IsNullOrEmpty(MobileExciseWarehouseTo);

        #endregion

        #region DelegateCommand

        public DelegateCommand StartWorkCommand { get; }
        public DelegateCommand LoadFileCommand { get; }

        #endregion

        public J029500ViewModel()
        {
            Period = DateTime.Now;
            StartWorkCommand = new DelegateCommand(StartWork).ObservesCanExecute(() => CanWork);
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
                // ReSharper disable once PossibleInvalidOperationException
                J029500Xml j029500 = new J029500Xml(Period.Value, _data,
                    new Dictionary<string, string>(5)
                    {
                        {Resources.EDRPOU, Edrpou}, {Resources.Act, Act}, {Resources.Direction, Direction},
                        {Resources.RegistrationDate, RegistrationDate},
                        {Resources.RegistrationNumber, RegistrationNumber}, {Resources.ProductСode1, ProductСode1},
                        {Resources.VolumeLiters1, VolumeLiters1}, {Resources.ProductСode2, ProductСode2},
                        {Resources.VolumeLiters2, VolumeLiters2}, {Resources.ExciseWarehouseFrom, ExciseWarehouseFrom},
                        {Resources.MobileExciseWarehouseFrom, MobileExciseWarehouseFrom},
                        {Resources.ExciseWarehouseTo, ExciseWarehouseTo},
                        {Resources.MobileExciseWarehouseTo, MobileExciseWarehouseTo}
                    }, XmlFilename);
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
                RaisePropertyChanged(nameof(Data));

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

                        Edrpou = "ЄДРПОУ";
                        Act = "Номер";
                        Direction = "Напрямок";
                        RegistrationDate = "Дата та час реєстрації";
                        RegistrationNumber = "Реєстраційний номер";
                        ProductСode1 = "Код УКТ ЗЕД (АН,РК-1)";
                        VolumeLiters1 = "Обсяг (АН,РК-1) (в л)";
                        ProductСode2 = "Код УКТ ЗЕД (РК-2)";
                        VolumeLiters2 = "Обсяг (РК-2) (в л)";
                        ExciseWarehouseFrom = "Акцизний склад, з якого відвантажено паливо";
                        MobileExciseWarehouseFrom = "Пересувний акцизний склад, з якого відвантажно паливо - Реєстраційний номер";
                        ExciseWarehouseTo = "Акцизний склад, на який отримано паливо";
                        MobileExciseWarehouseTo = "Пересувний акцизний склад, на який отримано паливо - Реєстраційний номер";

                        RaisePropertyChanged(nameof(ColumnNames));
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
