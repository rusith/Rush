
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls.Dialogs;
using Rush.Extensions;
using Rush.Models;
using WinForms = System.Windows.Forms;


namespace Rush.Windows
{
    public partial class MainWindow
    {
        private int _fileCount;
        private int _mp3Count;
        private int _m4ACount;
        private int _aacCount;
        private int _falcCount;
        private int _oggCount;
        private int _wmaCount;
        private OrganizeOrder _order;
        private string _lastSource;

        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            OrderTextBox.Text = "<Artist><Album><File>";
        }

        private async void SelectSourceFolder()
        {
            while (true)
            {
                var folderDialog = new WinForms.FolderBrowserDialog
                {
                    Description = @"select a new source folder to add source locations list"
                };
                if (string.IsNullOrEmpty(_lastSource) == false)
                    folderDialog.SelectedPath = _lastSource;
                var folderDialogResult = folderDialog.ShowDialog();
                if (folderDialogResult != System.Windows.Forms.DialogResult.OK) break;
                if (folderDialog.SelectedPath.Length < 4)
                {
                    await this.ShowMessageAsync("cannot accept the location", "You have selected a drive . \nplease select a folder ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                    continue;
                }
                var selectedDir = new DirectoryInfo(folderDialog.SelectedPath);
                if (selectedDir.FullName == DestinationTextBox.Text)
                {
                    await this.ShowMessageAsync("Cannot add Folder", "Cannot add selected folder because it is selected as the destination folder.\nplease select deferent location and try again ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                    return;
                }

                if (!selectedDir.Exists)
                {
                    var result=await this.ShowMessageAsync("Folder dose not exists", "The selected directory not exist in the given path. \ndo you want to try another location?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes",NegativeButtonText = "No"});
                    if (result == MessageDialogResult.Affirmative)
                        continue;
                    return;
                }
                var have = false;
                foreach (ComboBoxItem cbi in SourceLocationsComboBox.Items)
                {
                    have = ((string)cbi.ToolTip == selectedDir.FullName);
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
                    _lastSource = selectedDir.FullName;
                }
                break;
            }
        }

        private async void SelectDestinationFolder()
        {
            while (true)
            {
                var folderDialog = new WinForms.FolderBrowserDialog
                {
                    Description = @"Select Folder to use as destination location"
                };
                if (folderDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) break;
                var directory = new DirectoryInfo(folderDialog.SelectedPath);
                if (directory.FullName == DestinationTextBox.Text)
                    return;
                if (
                    SourceLocationsComboBox.Items.OfType<ComboBoxItem>()
                        .Any(cbi => (string)cbi.ToolTip == directory.FullName))
                {
                    await this.ShowMessageAsync("Cannot add Folder", "Cannot add selected folder because it is contains in the source folders list.\nplease select deferent location and try again ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                    return;
                }
                if (!directory.Exists)
                {
                    try
                    {
                        directory.Create();
                        if (!directory.Exists)
                            throw new Exception("cannot create directory");
                    }
                    catch (Exception)
                    {

                        var result = await this.ShowMessageAsync("Cannot create the Directory.", "The selected folder dose not exists and cannot create it\nDo you want to try deferent location?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
                        if (result == MessageDialogResult.Affirmative)
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
                    var result = await this.ShowMessageAsync("specified location is not accessible.", "Cannot use this folder as the destination location.\nbecause this program cannot create or delete files on that location\nmake sure rush have read and write perditions on that folder", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
                    if (result == MessageDialogResult.Affirmative)
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
            var fileIndex = 0;
            var order = new OrganizeOrder();
            foreach (Match match in variables)
            {
                containsFile=Regex.IsMatch(match.Value, "<file.+?>", RegexOptions.IgnoreCase);
                if(!containsFile)
                    if (match.Value.Replace(">","").Replace("<","").ToLower() == "file")
                    {
                        containsFile = true;
                        fileIndex = variables.OfType<Match>().ToList().IndexOf(match);
                        break;
                    }
                        

                if (containsFile)
                {
                    var content = match.Value.Remove(0, Regex.Match(match.Value, "<file", RegexOptions.IgnoreCase).Length).Replace(">", "");
                    if (content.Length > 0)
                    {
                        var matches = Regex.Matches(text, "\\[.+?\\]");
                        if (matches.Count > 0)
                        {
                            var fileName=new FileNameTemplate();
                            foreach (var contentm in from Match tem in matches select tem.Value.Replace("]", "").Replace("[", ""))
                            {
                                if (contentm.StartsWith("\"") && contentm.EndsWith("\""))
                                {
                                    fileName.AddLiterel(contentm.Replace("\"",""));
                                    continue;
                                }
                                switch (contentm)
                                {
                                    case "artist":
                                        fileName.AddElement(FileNameItem.Artist);
                                        continue;
                                    case "album":
                                        fileName.AddElement(FileNameItem.Album);
                                        continue;
                                    case "track":
                                        fileName.AddElement(FileNameItem.Trak);
                                        continue;
                                    case "title":
                                        fileName.AddElement(FileNameItem.Title);
                                        continue;
                                    case "count":
                                        fileName.AddElement(FileNameItem.Count);
                                        continue;
                                }
                            }
                            order.FileNameTemplate = fileName;
                        }
                    }
                    fileIndex = variables.OfType<Match>().ToList().IndexOf(match);
                   break;
                }
            }
            if (!containsFile)
            {
                NotifyOrderValidatation("Order Expression Must Contain File Field Value.", true);
                return;
            }
            
            for (var i = 0; i < variables.Count; i++)
            {
                var match = variables[i];
                var value = fileIndex == i ? "file" : match.Value.Substring(1, match.Value.Length - 2).ToLower();
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
            var count = variables.Cast<Match>().Sum(v => v.Length);
            //var orderString = order.ToOrderString();
            if (count < OrderTextBox.Text.Length)
            {
                NotifyOrderValidatation("Text Contains Unwanted Characters ", true);
                return;
            }
            NotifyOrderValidatation(order.IsEmpty()?"   ":order.ToString(), false);
            _order = order;

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

            _mp3Count = mp3.Count;
            _m4ACount = m4A.Count;
            _aacCount = aac.Count;
            _falcCount = falc.Count;
            _oggCount = ogg.Count;
            _wmaCount = wma.Count;
            var isEmpty = items.Count < 1;

            _fileCount = _mp3Count + _m4ACount + _aacCount + _falcCount + _oggCount + _wmaCount;

            Mp3CheckBox.Content = isEmpty? "mp3" : string.Format("mp3({0})", _mp3Count);
            M4ACheckBox.Content = isEmpty ? "m4a" : string.Format("m4a({0})", _m4ACount);
            AacCheckBox.Content = isEmpty ? "aac" : string.Format("aac({0})", _aacCount);
            FalcCheckBox.Content = isEmpty ? "falc" : string.Format("falc({0})", _falcCount);
            OggCheckBox.Content = isEmpty ? "ogg" : string.Format("ogg({0})", _oggCount);
            WmaCheckBox.Content = isEmpty ? "wma" : string.Format("wma({0})", _wmaCount);

            Mp3CheckBox.Tag = _mp3Count;
            M4ACheckBox.Tag = _m4ACount;
            AacCheckBox.Tag = _aacCount;
            FalcCheckBox.Tag = _falcCount;
            OggCheckBox.Tag = _oggCount;
            WmaCheckBox.Tag = _wmaCount;
        }

        // ReSharper disable once FunctionComplexityOverflow
        private async void Organize()
        {
            if (_fileCount < 1)
            {
                await this.ShowMessageAsync("No Files", "No files present to organize in the selected folders.\nplease select another location that have supported file types ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                OrganizeButton.IsEnabled = true;
                return;
            }
            if (Mp3CheckBox.IsChecked == false && M4ACheckBox.IsChecked == false && AacCheckBox.IsChecked == false &&
                FalcCheckBox.IsChecked == false && OggCheckBox.IsChecked == false && WmaCheckBox.IsChecked == false)
            {
                await this.ShowMessageAsync("File types not selected", "File types not selected.\nyou should select at least one file type to continue", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                OrganizeButton.IsEnabled = true;
                return;
            }

            if (_order.FileNameTemplate.Template != null && _order.FileNameTemplate.Template.Count > 0)
            {
                if (!_order.FileNameTemplate.Template.Contains(FileNameItem.Count))
                {
                    var result = await this.ShowMessageAsync("Be careful!", "you are going to change the file name template . be careful. because if you set a more generic type name template some files (maybe most) will lose.add count variable to make sure all files in the folder have deferent file names.", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Continue", NegativeButtonText = "Stop" });
                    if (result == MessageDialogResult.Negative)
                        return;
                }
            }

            var sources = new HashSet<string>();
            foreach (var item in SourceLocationsComboBox.Items.OfType<ComboBoxItem>().Where(item => item != null))
            {
                sources.Add((string)item.ToolTip);
            }

            if (sources.Count < 1)
            {
                OrganizeButton.IsEnabled = true;
                ExitButton.Content = "Exit";
                return;
            }

            var fileTypes = new List<string>();
            if (Mp3CheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("mp3");
            if (M4ACheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("m4a");
            if (AacCheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("aac");
            if (FalcCheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("falc");
            if (OggCheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("ogg");
            if (WmaCheckBox.IsChecked.GetValueOrDefault())
                fileTypes.Add("wma");

            var files = new HashSet<FileInformation>();
            foreach (var f in
                    from sourceFolder in
                    sources
                    select new DirectoryInfo(sourceFolder)
                    into dir
                    where dir.Exists
                    select dir.GetFilesUsingExtensions(fileTypes.ToArray())
                    into inFiles
                    where inFiles.Count > 0
                    from f in inFiles
                    select f)
            {
                files.Add(new FileInformation { SourceFile = f });
            }

            var destination = new DirectoryInfo(DestinationTextBox.Text);
            try
            {
                if (destination.Exists == false)
                    Directory.CreateDirectory(destination.FullName);
            }
            catch (Exception)
            {
                await this.ShowMessageAsync("Cannot Create Destination Folder", "The Destination Folder You selected is not exists now. and i cannot create it. please select deferent location and try again", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                OrganizeButton.IsEnabled = true;
                return;
            }
            var overwrite = OverwriteExistingCheckBox.IsChecked.GetValueOrDefault();
            var progress = new ProcessWindow(_order, sources, destination,
                 files.ToList(), overwrite);

            progress.ShowDialog();
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
            var helpWindow = new FileOrderHelpWindow();
            helpWindow.Show();
        }

        private void OnOrganizeButtonClick(object sender, RoutedEventArgs e)
        {
            Organize();
        }

        private async void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync("confirm exit", "Are you sure ? do you want to exit", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes", NegativeButtonText = "No" });
            if (result == MessageDialogResult.Affirmative)
                Application.Current.Shutdown();
        }
    }
}