
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Rush.Controllers;
using Rush.Extensions;
using Rush.Models;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;

namespace Rush.Windows
{
    public partial class MainWindow
    {

        private readonly RushController _controller;
        private int _fileCount;
        private int mp3Count;
        private int m4ACount;
        private int aacCount;
        private int falcCount;
        private int oggCount;
        private int wmaCount;

        public MainWindow(RushController controller)
        {
            InitializeComponent();
            InitializeControls();
            _controller = controller;
        }

        private void InitializeControls()
        {
            OrderTextBox.Text = "<Artist><Album><File>";
        }

        private void SelectSourceFolder()
        {
            while (true)
            {
                var folderDialog = new FolderBrowserDialog
                {
                    Description = "select a new source folder to add source locations list"
                };
                var folderDialogResult = folderDialog.ShowDialog();
                if (folderDialogResult != System.Windows.Forms.DialogResult.OK) break;
                if (folderDialog.SelectedPath.Length < 4)
                {
                    MessageBox.Show("You have selected a drive . \nplease select a folder ",
                        "cannot accept the location",MessageBoxButtons.OK,MessageBoxIcon.Stop,MessageBoxDefaultButton.Button1);
                    continue;
                }
                var selectedDir = new DirectoryInfo(folderDialog.SelectedPath);
                
                if (!selectedDir.Exists)
                {
                    var notExistResult = MessageBox.Show(
                    "The selected directory not exist in the given path. \ndo you want to try another location?",
                    "Folder dose not exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (notExistResult == System.Windows.Forms.DialogResult.Yes)
                        continue;
                    return;
                }
                var have = false;
                foreach (ComboBoxItem cbi in SourceLocationsComboBox.Items)
                {
                    have= ((string) cbi.ToolTip == selectedDir.FullName) ;
                }

                if (!have)
                {
                    var comboBoxItem = new ComboBoxItem
                    {
                        ToolTip = selectedDir.FullName,
                        Tag = selectedDir.FullName,
                        IsSelected = true,
                        Content = selectedDir.Name
                    };
                    SourceLocationsComboBox.Items.Add(comboBoxItem);
                }
                break;
            }
        }

        private void SelectDestinationFolder()
        {
            while (true)
            {
                 var folderDialog = new FolderBrowserDialog
                {
                    Description = "Select Folder to use as destination location"
                };
                if (folderDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) break;
                var directory = new DirectoryInfo(folderDialog.SelectedPath);
                if (directory.FullName == DestinationTextBox.Text)
                    return;
                if (!directory.Exists)
                {
                    try
                    {
                        directory.Create();
                        if(!directory.Exists)
                            throw new Exception("cannot create directory");
                    }
                    catch (Exception)
                    {
                        var result =
                            MessageBox.Show(
                                "The selected folder dose not exists and cannot create it\nDo you want to try deferent location?",
                                "Cannot create the Directory.", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                            continue;
                        return;
                    }
                }
                var rushTestFile = new FileInfo(directory.FullName + "\\" + "rushTestFile.rush");
                try
                {
                    var stream = rushTestFile.Create();
                    stream.Close();

                    if (rushTestFile.Exists)
                        rushTestFile.Delete();
                    else
                        throw new Exception("cannot create file");
                }
                catch (Exception)
                {
                    var result =
                        MessageBox.Show(
                            "Cannot use this folder as the destination location.\nbecause this program cannot create or delete files on that location\nmake sure rush have read and write perditions on that folder",
                            "specified location is not accessible.\ndo you want to try another location?",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                        continue;
                    return;
                }
                DestinationTextBox.Text = directory.FullName;
                break;
            }
        }

        private void ValidateOrderInput()
        {
            var text = OrderTextBox.Text;
            var variables = Regex.Matches(text, "<.+?>");
            if (variables.Count < 1 )
            {
                NotifyOrderValidatation("Order Expression Must Contain One Field Value.",true);
                return;
            }
            var containsFile = false;
            foreach (Match match in variables)
            {
                containsFile = match.Value.ToLower() == "<file>";
                if(containsFile)
                    break;
            }
                
            
            if (!containsFile)
            {
                NotifyOrderValidatation("Order Expression Must Contain File Field Value.", true);
                return;
            }
            var order = new OrganizeOrder();
            for (var i = 0; i < variables.Count; i++)
            {
                var match = variables[i];
                var value = match.Value.Substring(1, match.Value.Length - 2).ToLower();
                switch (value)
                {
                    case "file":
                        if (i < variables.Count - 1)
                        {
                            NotifyOrderValidatation("File Must be the last Field", true);
                            OrderTextBox.Select(match.Index, match.Length);
                            return;
                        }
                        order.AddElement(OrderElement.File);
                        continue;
                    case "album":
                        if (!order.AddElement(OrderElement.Album))
                        {
                            OrderTextBox.Text= text.Remove(match.Index, match.Length);
                        }
                        continue;
                    case "artist":
                        if (!order.AddElement(OrderElement.Artist))
                        {
                            OrderTextBox.Text = text.Remove(match.Index, match.Length);
                        }
                        continue;
                    case "genre":
                        if (!order.AddElement(OrderElement.Genre))
                        {
                            OrderTextBox.Text = text.Remove(match.Index, match.Length);
                        }
                        continue;
                    case "year":
                        if (!order.AddElement(OrderElement.Year))
                        {
                            OrderTextBox.Text = text.Remove(match.Index, match.Length);
                        }
                        continue;
                }
            }
            var orderString = order.ToOrderString();
            if (orderString.Length < OrderTextBox.Text.Length)
            {
                NotifyOrderValidatation("Text Contains Unwanted Characters ", true);
                return;
            }
            NotifyOrderValidatation(order.IsEmpty()?"   ":order.ToString(), false);
        }

        private void NotifyOrderValidatation(string text, bool errorOrNotify)
        {
            if (string.IsNullOrEmpty(text))
                return;
            OrderValidationLabel.Foreground = (errorOrNotify ? Brushes.Tomato : Brushes.CornflowerBlue);
            OrderTextBox.Tag = !errorOrNotify;
            OrderTextBox.SelectionBrush = OrderValidationLabel.Foreground;

            OrderValidationLabel.Content = text;
            
        }

        private void AddNewItemToSourceFolders()
        {
            var beforeItemCOunt = SourceLocationsComboBox.Items.Count;
            SelectSourceFolder();
            var afterItemsCount = SourceLocationsComboBox.Items.Count;
            if (beforeItemCOunt != afterItemsCount && afterItemsCount > 0)
                SetFileTypeItemCounts();
        }

        private void SetFileTypeItemCounts()
        {
            var items = SourceLocationsComboBox.Items;
            var mp3 = new List<FileInfo>();
            var m4A = new List<FileInfo>();
            var aac = new List<FileInfo>();
            var falc = new List<FileInfo>();
            var ogg = new List<FileInfo>();
            var wma = new List<FileInfo>();
            foreach (var dirInfo 
                        in from ComboBoxItem item 
                        in items 
                        select item.Tag 
                        as string into location 
                        where string.IsNullOrWhiteSpace(location)==false 
                        select new DirectoryInfo(location) 
                        into dirInfo 
                        where dirInfo.Exists select dirInfo)
            {
                mp3.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "mp3" }));
                m4A.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "m4a" }));
                aac.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "aac" }));
                falc.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "falc" }));
                ogg.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "ogg" }));
                wma.AddRange(dirInfo.GetFilesUsingExtensions(new[] { "wma" }));
            }

            mp3 = mp3.Distinct().ToList();
            m4A = m4A.Distinct().ToList();
            aac = aac.Distinct().ToList();
            falc = falc.Distinct().ToList();
            ogg = ogg.Distinct().ToList();
            wma = wma.Distinct().ToList();

            mp3Count = mp3.Count;
            m4ACount = m4A.Count;
            aacCount = aac.Count;
            falcCount = falc.Count;
            oggCount = ogg.Count;
            wmaCount = wma.Count;
            var isEmpty = items.Count < 1;

            _fileCount = mp3Count + m4ACount + aacCount + falcCount + oggCount + wmaCount;

            Mp3CheckBox.Content = isEmpty? "mp3" : string.Format("mp3({0})", mp3Count);
            M4ACheckBox.Content = isEmpty ? "m4a" : string.Format("m4a({0})", m4ACount);
            AacCheckBox.Content = isEmpty ? "aac" : string.Format("aac({0})", aacCount);
            FalcCheckBox.Content = isEmpty ? "falc" : string.Format("falc({0})", falcCount);
            OggCheckBox.Content = isEmpty ? "ogg" : string.Format("ogg({0})", oggCount);
            WmaCheckBox.Content = isEmpty ? "wma" : string.Format("wma({0})", wmaCount);

            Mp3CheckBox.Tag = mp3Count;
            M4ACheckBox.Tag = m4ACount;
            AacCheckBox.Tag = aacCount;
            FalcCheckBox.Tag = falcCount;
            OggCheckBox.Tag = oggCount;
            WmaCheckBox.Tag = wmaCount;
        }

        private void Organize()
        {
            if (_fileCount < 1)
            {
                 MessageBox.Show("No files present to organize in the selected folders.\nplease select another location that have supported file types ", "No Files",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }
            if (Mp3CheckBox.IsChecked == false && M4ACheckBox.IsChecked == false && AacCheckBox.IsChecked == false &&
                FalcCheckBox.IsChecked == false && OggCheckBox.IsChecked == false && WmaCheckBox.IsChecked == false)
            {
                MessageBox.Show("File types not selected.\nyou should select at least one file type to continue", "File types not selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                return;
            }

            {
                var count = 0;
                if (Mp3CheckBox.IsChecked.GetValueOrDefault())
                    count += mp3Count;
                if (M4ACheckBox.IsChecked.GetValueOrDefault())
                    count += m4ACount;
                if (AacCheckBox.IsChecked.GetValueOrDefault())
                    count += aacCount;
                if (FalcCheckBox.IsChecked.GetValueOrDefault())
                    count += falcCount;
                if (OggCheckBox.IsChecked.GetValueOrDefault())
                    count += oggCount;
                if (WmaCheckBox.IsChecked.GetValueOrDefault())
                    count += wmaCount;

                if (count < 1)
                {
                    MessageBox.Show("There are no files with selected file types.\nplease select file types that have at least one file","No Files to process",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            OrganizeButton.IsEnabled = false;
            _controller.Organize();
        }

        private void OnAddNewSourceFolderButtonClick(object sender, RoutedEventArgs e)
        {
            AddNewItemToSourceFolders();
        }

        private void OnRemoveSelectedSourceFolderButtonClick(object sender, RoutedEventArgs e)
        {
            if (SourceLocationsComboBox.SelectedIndex <= -1) return;
            SourceLocationsComboBox.Items.RemoveAt(SourceLocationsComboBox.SelectedIndex);
            SetFileTypeItemCounts();
        }

        private void OnClearAllSourceFoldersButtonClick(object sender, RoutedEventArgs e)
        {
            if(SourceLocationsComboBox.Items.Count>0)
                SourceLocationsComboBox.Items.Clear();
            SetFileTypeItemCounts();
        }

        private void OnDestinationBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            SelectDestinationFolder();
        }

        private void OnOrderTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateOrderInput();
        }

        private void OnFileOrderHelpImagePreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _controller.ShowFileOrderHelpWindow();
        }

        private void OnOrganizeButtonClick(object sender, RoutedEventArgs e)
        {
            Organize();
        }

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            var confirmMessageResult = MessageBox.Show("Are you sure ? do you want to exit", "confirm exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (confirmMessageResult == System.Windows.Forms.DialogResult.Yes)
                System.Windows.Application.Current.Shutdown();
        }
    }
}
