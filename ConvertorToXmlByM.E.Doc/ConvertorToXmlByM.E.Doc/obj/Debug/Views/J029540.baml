   M S B A M L   `   `   `  ����O  KCustomControlLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY@clr-namespace:CustomControlLibrary;assembly=CustomControlLibraryCustomControlLibrary  5       ConvertorToXmlByM.E.Doc+   %ConvertorToXmlByM.E.Doc.Views.J029540   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Y UPrism.Unity.Wpf, Version=7.2.0.1422, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59S OPrism.Wpf, Version=7.2.0.1422, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59%mvvmhttp://prismlibrary.com/   5      Ocustom@clr-namespace:CustomControlLibrary;assembly=CustomControlLibrary   5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  . .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5       � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Выбрать файл��6   $��0��6L   $��0��6Y   $	��120��6i   ��6(   �� *LoadFileCommand+6i   �� 5   
   $#��Имя плательщика��6   $��1��6+   $��0��68   �� 5   
   $��1��6   $��1��6     �� SelectedValue 60   �� *SelectedColumn[PayerName]+�� 5   
   $ ��Объем в литрах��6   $��2��6*   $��0��67   �� 5   
   $��2��6   $��1��6     60   �� *SelectedColumn[VolumeLiters]+�� 5   
   $Z��TСумма АН, не упл. по операциям, не подл. н/о, грн.��6   $��3��6N   $��0��6[   �� 5    
   $��3��6   $��1��6     60   �� *SelectedColumn[FromTaxable]+�� 5!   
   $*��$Номер документа ГТД��6   $��4��6/   $��0��6<   �� 5"   
   $��4��6   $��1��6     60   �� *SelectedColumn[Act]+�� 5#   
   $4��.Дата пересечения границы��6   $��5��64   $��0��6A   �� 5$   
   $��5��6   $��1��6     60   �� *SelectedColumn[СrossingDate]+�� 5%   
   $��Сформировать��6   $��6��6M   $��0��6Z   $	��Top=�6j   $	��120��6�   ��6(   �� *StartWorkCommand+6�   +   %CustomControlLibrary.ExtendedDataGrid  5&   
   &   System.Windows.Controls.DataGrid   CanUserSortColumns .     CanUserDeleteRows .    ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ SizeToHeader 5'   "   $��0��6=   $��2��6J   $��7��6Z   $	��500��6k   $	��800��6z   $	��500��6�      AutoGenerateColumns . ��5&   "   �� *Data+ �� UpdateSourceTrigger$ PropertyChanged=�5'   �   (  "System.Windows.Controls.DatePicker  5(   
   -   'CustomControlLibrary.DatePickerCalendar	   IsMonthYear	 . /   )CustomControlLibrary.DatePickerDateFormat
   
DateFormat$
 MM/yyyy��6b   $��6��6�   $��1��6�   $��25��5)      $	��Top=�6!      SelectedDate 5(      �� *Period+5)   !   5*      5+      