   M S B A M L   `   `   `  ����  ConvertorToXmlByM.E.Doc)    #ConvertorToXmlByM.E.Doc.Views.Shell   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3b+http://schemas.microsoft.com/xaml/behaviors  5   	   Y UPrism.Unity.Wpf, Version=7.2.0.1422, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59S OPrism.Wpf, Version=7.2.0.1422, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59%mvvmhttp://prismlibrary.com/   5   	     � Title$  Shell��5	   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�66   !  Prism.Mvvm.ViewModelLocator   AutoWireViewModel . *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5   
      	EventName$ Loaded��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6$   ��  �� Path$ LoadedCommand�5      	 � 	Resources	 5      
 �� 
TargetType �� BasedOn �� Property �� Value��%?  '��       7��'�� O     7���� 5   
   "
 ��6   8   .�5      ��  � ��$
 Wrap=�6-   5      �� 5   
   "
 ��6   8   .�5      ��  � ��$	 150��6)   �� 5       � ��$	 225��6)   �� 5       �@��
IsEditable . 6+   �� 5       � �� 6,   �� *ColumnNames+5      5      .��5      �  � RowDefinitions 5   
   �� 5       $
��Auto �6   �� 5!      $
��Auto �6   �� 5"      .+�5$   
   	�  	� 	GroupName$ typeDocument��6   $��Доп. 1-1��6/   $��0��6B    d� 	IsChecked$
 TrueY�6O   $��J029500��6�   $
��Left=�6�   ��6`   �� $ NavigateCommand�6�   	� 5%   
   $ typeDocument��6   $��	Доп. 4��6/   $��1��6@   $��J029540��6v   $
��Left=�6�   ��6M   �� $ NavigateCommand�6�   �� 5&   
   $��2��6   !  Prism.Regions.RegionManager   
RegionName$ ContentRegionShell��6&   $��10q�6Y   5'      5(      