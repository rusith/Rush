
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using Rush.Controllers;
using Rush.Extensions;
using Rush.Models;
using MahApps.Metro.Controls;

namespace Rush.Windows
{
    public partial class MainWindow
    {

        private readonly RushController _controller;
        private int _fileCount;
        private int _mp3Count;
        private int _m4ACount;
        private int _aacCount;
        private int _falcCount;
        private int _oggCount;
        private int _wmaCount;
        private OrganizeOrder _order;

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

        private async void SelectSourceFolder()
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
                    await this.ShowMessageAsync("cannot accept the location", "You have selected a drive . \nplease select a folder ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                    continue;
                }
                var selectedDir = new DirectoryInfo(folderDialog.SelectedPath);
                if (selectedDir.FullName == DestinationTextBox.Text)
                {
                    await this.ShowMessageAsync("Cannot add Folder", "Cannot add selected folder becouse it is selected as the destination folder.\nplease select deferent location and try again ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
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
                }
                break;
            }
        }

        private async void SelectDestinationFolder()
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
                if (
                    SourceLocationsComboBox.Items.OfType<ComboBoxItem>()
                        .Any(cbi => (string)cbi.ToolTip == directory.FullName))
                {
                    await this.ShowMessageAsync("Cannot add Folder", "Cannot add selected folder becouse it is contains in the source folders list.\nplease select deferent location and try again ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
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

        private  void Organize()
        {
            if (_fileCount < 1)
            {
                this.ShowMessageAsync("No Files", "No files present to organize in the selected folders.\nplease select another location that have supported file types ", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                return;
            }
            if (Mp3CheckBox.IsChecked == false && M4ACheckBox.IsChecked == false && AacCheckBox.IsChecked == false &&
                FalcCheckBox.IsChecked == false && OggCheckBox.IsChecked == false && WmaCheckBox.IsChecked == false)
            {
                this.ShowMessageAsync("File types not selected", "File types not selected.\nyou should select at least one file type to continue", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "OK" });
                return;
            }

            var sources = new HashSet<string>();
            foreach (var item in SourceLocationsComboBox.Items.OfType<ComboBoxItem>().Where(item => item != null))
            {
                sources.Add((string)item.ToolTip);
            }

            if(sources.Count<1)
                return;

            var fileMode = CopyRadioButton.IsChecked;

            var files = new HashSet<FileInformation>();
            foreach (var f in 
                    from sourceFolder in
                    sources select new DirectoryInfo(sourceFolder) 
                    into dir where dir.Exists
                    select dir.GetFilesUsingExtensions(new string[] {"mp3", "m4a", "aac", "falc", "ogg", "wma"}) 
                    into inFiles where inFiles.Count > 0 from f in inFiles select f)
            {
                files.Add(new FileInformation {SourceFile = f});
            }

            var destination = new DirectoryInfo(DestinationTextBox.Text);
            if (destination.Exists == false)
                Directory.CreateDirectory(destination.FullName);
            Height = 407;
            progressStack.Visibility = Visibility.Visible;

            var overwrite = OverwriteExistingCheckBox.IsChecked.GetValueOrDefault();
            Task.Factory.StartNew(() =>
            {
                progressbar.Dispatcher.Invoke(() =>
                {
                    progressbar.Minimum = 0;
                    progressbar.Maximum = files.Count;
                    progressbar.Value = 0;
                });

                TitleLabel.Dispatcher.Invoke(() =>
                {
                    TitleLabel.Content = "Processing Files";
                });

                int[] fileIndex = { 0 };
                foreach (var file in files)
                {
                    var file1 = file.SourceFile;
                    MessageLabel.Dispatcher.Invoke(() =>
                    {
                        MessageLabel.Content = file1.Name;
                    });

                    progressbar.Dispatcher.Invoke(() =>
                    {
                        progressbar.Value = fileIndex[0];
                    });

                    var taginfo = TagLib.File.Create(file1.FullName);
                    var newfile = destination.FullName;
                    foreach (var tag in _order.Order)
                    {
                        if (tag == OrderElement.Album)
                        {
                            var album = taginfo.Tag.Album;
                            if (string.IsNullOrWhiteSpace(album))
                                album = "UnknownAlbum";
                            newfile = Path.Combine(newfile, album.ToSafeFileName());
                        }
                        if (tag == OrderElement.Artist)
                        {
                            var artist = "UnknownArtist";
                            if (taginfo.Tag.Performers != null && taginfo.Tag.Performers.Length > 0 && taginfo.Tag.Performers.All(c => c.Length > 0))
                                artist = taginfo.Tag.Performers.Aggregate((c, n) => c + " & " + n).TrimEnd('&');
                            newfile = Path.Combine(newfile, artist.ToSafeFileName());
                        }
                        if (tag == OrderElement.Genre)
                        {
                            var genre = "UnknownGenre";
                            if (taginfo.Tag.Genres != null && taginfo.Tag.Genres.Length > 0 && taginfo.Tag.Genres.All(c => c.Length > 0))
                                genre = taginfo.Tag.Genres.Aggregate((c, n) => c + " & " + n).TrimEnd('&');
                            newfile = Path.Combine(newfile, genre.ToSafeFileName());
                        }
                        if (tag == OrderElement.Year)
                        {
                            var year = "UnknownYear";
                            if (taginfo.Tag.Year > 0)
                                year = taginfo.Tag.Year.ToString();
                            newfile = Path.Combine(newfile, year);
                        }

                        if (tag == OrderElement.File)
                        {
                            var fileName = file1.Name;
                            newfile = Path.Combine(newfile, fileName.ToSafeFileName());
                        }
                    }
                    file.DestinationFile=new FileInfo(newfile);
                    fileIndex[0] = fileIndex[0] + 1;
                }

                TitleLabel.Dispatcher.Invoke(() =>
                {
                    TitleLabel.Content = string.Format("{0}  Files",fileMode.GetValueOrDefault()?"Copying":"Moving");
                });

                
                fileIndex[0] = 0;
                foreach (var file in files)
                {
                    try
                    {
                        var sf = file.SourceFile;
                        MessageLabel.Dispatcher.Invoke(() =>
                        {
                            MessageLabel.Content = sf.Name;
                        });

                        progressbar.Dispatcher.Invoke(() =>
                        {
                            progressbar.Value = fileIndex[0];
                        });
                        fileIndex[0]++;
                        if (file.DestinationFile.Directory != null && !file.DestinationFile.Directory.Exists)
                            Directory.CreateDirectory(file.DestinationFile.Directory.FullName);
                        if (file.DestinationFile.Exists)
                            if (overwrite)
                            {
                                file.DestinationFile.Delete();
                            }
                            else
                            {
                                continue;
                            }
                        File.Copy(file.SourceFile.FullName, file.DestinationFile.FullName);
                    }
                    finally
                    {
                        if(!fileMode.GetValueOrDefault())
                            file.SourceFile.Delete();
                    }
                }

            });
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

        private async void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync("confirm exit", "Are you sure ? do you want to exit", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "Yes",NegativeButtonText = "No" });
            if (result == MessageDialogResult.Affirmative)
                System.Windows.Application.Current.Shutdown();
        }
    }
}