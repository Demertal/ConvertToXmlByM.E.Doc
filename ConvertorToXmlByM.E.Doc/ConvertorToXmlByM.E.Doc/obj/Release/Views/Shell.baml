   M S B A M L   `   `   `  ����   ConvertorToXmlByM.E.Doc)    #ConvertorToXmlByM.E.Doc.Views.Shell   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Y UPrism.Unity.Wpf, Version=7.2.0.1367, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59S OPrism.Wpf, Version=7.2.0.1367, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity    � Title$  Shell�� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=�!  Prism.Mvvm.ViewModelLocator   AutoWireViewModel . .  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Loaded��.  (System.Windows.Interactivity.TriggerBase   Actions. -  'Prism.Interactivity.InvokeCommandAction     Command ��  �� Path$ LoadedCommand�	 � 	Resources	 
 �� 
TargetType �� BasedOn �� Property �� Value��%$  '��       7��'�� *     7���� "
 ��8   .���  � ��$
 Wrap=��� "
 ��8   .���  � ��$	 150����  � ��$	 225����  �@��
IsEditable . ��  �@��DisplayMemberPath$ Caption����  �@��SelectedValuePath$ Caption����  � �� �� *
ColumnName+.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� .+�	�  	� 	GroupName$ typeDocument��$��Доп. 1-1��$��0�� d� 	IsChecked$
 TrueY�$��J029500��$
��Left=����� $ NavigateCommand�	� $ typeDocument��$��	Доп. 4��$��1��$��J029540��$
��Left=����� $ NavigateCommand��� $��2��!  Prism.Regions.RegionManager   
RegionName$ ContentRegionShell��$��10q�