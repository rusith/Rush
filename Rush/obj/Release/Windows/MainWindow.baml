   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  ? ;Rush, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Rush.ConvertersRush.Converters    Rush.Windows.MainWindow   Hcontrols;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ,vcclr-namespace:Rush.Converters     -   (   "MahApps.Metro.Controls.MetroWindow   Name$  MetroWindow�� � Title$
 Rush��$	��316��$	��316��$	��645��$	��645�� � Icon$% /Rush;component/Images/logo.PNG��   RightWindowCommands +   %MahApps.Metro.Controls.WindowCommands  .@��� -   $��UpdateButton��$��update�� � ToolTip$D >Check For Update . This Will Open Homepage if update available���� -   $��AboutButton��$��about���� -   $
��home��$ Goto Homepage���� -   $��Facebook��$��15��$ Share On Facebook������  �� ImageSource$. (/Rush;component/Images/facebook_logo.jpg���� -   $��Twitter��$��15��$ Share On Twitter������ $- '/Rush;component/Images/twitter_logo.png�� � 	Resources (  "Rush.Converters.ListSelectedToBool   ListSelectedToBoolConverterA  ;Rush.Converters.ComboBoxAndButtonTooltipMultiValueConverter / +ComboBoxAndButtonTooltipMultiValueConverter(  "Rush.Converters.IntToBoolConverter  IntToBoolConverter* $Rush.Converters.BoolToColorConverter  BoolToColorConverter+  %Rush.Converters.StringToBoolConverter  StringToBoolConverter-  'Rush.Converters.OrganizeButtonConverter  OrganizeButtonConverter��%Z   &	        &	      &	 
     &	      &	      &	                  .���� $
��Auto=�$
��Auto=�.��� $��5,10,0,0q�.+��� .+��� $��10,5,0,0q�.+��� $
��Bold"�.��	Locations��  �� Orientation$ 
Horizontal=�$��10,0,0,0q�.+��� .��Source Locations�� -   $��SourceLocationsComboBox��$&  Folders For Get media FIles From��$��5,0,0,0q�$	��339��$��2,1,1,1q����� *Tag+ �� 	Converter# [ 	 �� RelativeSource	 �� *Self+

 � Tag
 �� *Items.Count+# [  �� Mode$ OneWay=�	 �� *Self+�� -   $��AddSourceFolderButton��  SquareButtonStyle#���  $��5,0,0,0q�$ Add New Location��.��Add�� -	   $��Remove��#���  $��5,0,0,0q�{��� *SelectedIndex+# [   �� ElementName$ SourceLocationsComboBox�� `�  `� 	Converter# [ .1��� $ SourceLocationsComboBox�� �� Path$ SelectedIndex��� $ SourceLocationsComboBox��$ SelectedItem��� -
   $"��SelectAllSourceFoldersButton��#���  $��5,0,0,0q�$ Clear All Locations��{��� *Items.Count+$ SourceLocationsComboBox��$ OneWay=�# [ .��Clear�� $ 
Horizontal=�$��10,5,0,0q�.+��� .��Destination�� -   $��DestinationTextBox��*	   $MahApps.Metro.Controls.TextBoxHelper 	  	Watermark$ Destination Folder��$��35,0,0,0q�$	��339��$��2,1,1,1q�$ The Destination Folder�� �� 
IsReadOnly . ���� *Tag+# [ 	 �� *Self+
 �� *Text+# [ 	 �� *Self+�� -   $��DestinationBrowseButton��#���  $��5,0,0,0q�$% Select The Destination Location��.��Browse�� $��10,5,0,0q�.+��� $
��Bold"�.��
File Types�� $ 
Horizontal=�$��15,5,0,0q�.+��� -   $��Mp3CheckBox��$	��mp3��{��� *Tag+# [ 	 �� *Self+�� -   $��M4ACheckBox��$	��m4a��$��10,0,0,0q�{��� *Tag+# [ 	 �� *Self+�� -   $��AacCheckBox��$	��aac��$��10,0,0,0q�{��� *Tag+# [ 	 �� *Self+�� -   $��FalcCheckBox��$
��falc��$��10,0,0,0q�{��� *Tag+# [ 	 �� *Self+�� -   $��OggCheckBox��$	��ogg��$��10,0,0,0q�{��� *Tag+# [ 	 �� *Self+�� -   $��WmaCheckBox��$	��wma��$��10,0,0,0q�{��� *Tag+# [ 	 �� *Self+�� $��10,5,0,0q�.+��� $
��Bold"�.��
File Order�� $ 
Horizontal=�$��10,0,0,0q�.+��� -   $��OrderTextBox��$��2,1,1,1q�$��0,0,5,0q�$	��533��$g aEnter the Order of the files you want in this input box (click help button for more informations)������ *Tag+# [ 	 �� *Self+
 �� *Text+# [ 	 �� *Self+�� -   $��FileOrderHelpImage��$%��/Rush;component/Images/help.png��$��26��$��36���� $��20,5,0,0q�.+��� -   $��OrderValidationLabel��
���Gc��$��11(��� $��0,10,0,0q�.+��� $ 
Horizontal=�$��RightToLeft=�.+��� -    ��Name$ OrganizeButton��#���  $��10,0,0,0q�$��29��$��Organize��{�`� # [ .1��� $ SourceLocationsComboBox��$	 Tag��� $ DestinationTextBox��$	 Tag��� $ OrderTextBox��$	 Tag��� -   $ 
ExitButton��#���  $��10,0,0,0q�$��57�� �� IsCancel . .��Exit�� -   $��MoveCheckbox��$��57��$
��Bold"�$��
290,0,10,0q�$ enable Moving files��.��Move�� -    ��Name$ OverwriteExistingCheckBox��$
��Bold"�$	��137��$��18�� d� 	IsChecked$
 TrueY�$��Segoe UI)�.��Overwrite Existing