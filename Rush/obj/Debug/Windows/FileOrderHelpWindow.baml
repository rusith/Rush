   M S B A M L   `   `   `  ����T  PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5       Rush&    Rush.Windows.FileOrderHelpWindow   HControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  How to setup file order��5   	   $��421.5��6)   $	��586��68    � 
ResizeMode$ NoResize=�6D   .��5      � $��0,0,2,0q�6   .+�5   
   �� $��How to  Setup File Order��6   $
��Left=�63   $��44��6N   $��10,0,0,0q�6Z   $	��Top=�6l   $	��254��6�   $��20(�6�   $
��Bold"�6�   �� 5   
   $���� Enter the Order of folders to be created in the destinations folder .
 <file> tag represents the file. (this tag should be the last tag)
 <artist> tag represent the artist of the media file
 <album> tag represents the album of the media file
 <year> tag represents the year of the media file
 <genre> tag represents the genre of the media media file the order text can only have these 5 tags��6   $
��Left=�5   ,   $	��106��6G   $��	10,34,0,0q�6T   $	��Top=�6g   $	��558��6   �� 5   
   -   5         ��Name$ CloseButton��5         SquareButtonStyle#���   6&   $��Close��6R   $
��Left=�6b   $��493,349,0,0q�6}   $	��Top=�6�   $��75��6�    � ToolTip$ Close Help Window��6�   6�   �� 5   
   $��How To Setup File Name��6   $
��Left=�61   $��44��6L   $��
10,137,0,0q�6X   $	��Top=�6l   $	��254��6�   $��20(�6�   $
��Bold"�6�   �� 5   
   $����To setup the file name enter variables inside File tag.
 variables you can use :
     [title] -> title of the medial file
     [album] -> album of the file
     [track] -> track number of the file
     [artist] -> artist of the file
     [count] -> (current number of files of the directory)+1 
     ["text"] -> represents the text inside  "" in this example 'text'
 if the tag info is not available its replaced with empty string.
 example : <File[title]["-"][artist]>��6   $
��Left=�5   7   $	��173��6R   $��
10,171,0,0q�6_   $	��Top=�6s   $	��558��6�   5      5      5      