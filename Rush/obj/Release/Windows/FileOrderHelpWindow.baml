   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls   Rush&    Rush.Windows.FileOrderHelpWindow   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  How to setup file order��$��421.5��$	��586�� � 
ResizeMode$ NoResize=�.��� $��0,0,2,0q�.+��� $��How to  Setup File Order��$
��Left=�$��44��$��10,0,0,0q�$	��Top=�$	��254��$��20(�$
��Bold"��� $���� Enter the Order of folders to be created in the destinations folder .
 <file> tag represents the file. (this tag should be the last tag)
 <artist> tag represent the artist of the media file
 <album> tag represents the album of the media file
 <year> tag represents the year of the media file
 <genre> tag represents the genre of the media media file the order text can only have these 5 tags��$
��Left=�$	��106��$��	10,34,0,0q�$	��Top=�$	��558���� -    ��Name$ CloseButton��   SquareButtonStyle#���   $��Close��$
��Left=�$��493,349,0,0q�$	��Top=�$��75�� � ToolTip$ Close Help Window���� $��How To Setup File Name��$
��Left=�$��44��$��
10,137,0,0q�$	��Top=�$	��254��$��20(�$
��Bold"��� $����To setup the file name enter variables inside File tag.
 variables you can use :
     [title] -> title of the medial file
     [album] -> album of the file
     [track] -> track number of the file
     [artist] -> artist of the file
     [count] -> (current number of files of the directory)+1 
     ["text"] -> represents the text inside  "" in this example 'text'
 if the tag info is not available its replaced with empty string.
 example : <File[title]["-"][artist]>��$
��Left=�$	��173��$��
10,171,0,0q�$	��Top=�$	��558��