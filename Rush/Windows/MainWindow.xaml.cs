
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Rush.Models;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;

namespace Rush.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeControls();
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
            if(text.Length<1)return;
            var variables = Regex.Matches(text, "<.+?>");
            if (variables.Count < 1 )
            {
                NotifyOrderValidatation("Order Expression Must Contain One Field Value.",true);
                
                return;
            }
            var containsFile = false;
            foreach (Match match in variables)
            {
                containsFile=match.Value.ToLower() == "<file>";
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
                var value = match.Value.Substring(1, match.Value.Length - 2);
                switch (value.ToLower())
                {
                    case "file":
                    {
                        if (i < variables.Count - 1)
                        {
                            NotifyOrderValidatation("File Must be the last Field",true);
                            OrderTextBox.Select(match.Index,match.Length);
                            return;
                        }
                        order.AddElement(OrderElement.File);
                        break;
                    }

                }
            }
        }

        private void NotifyOrderValidatation(string text, bool errorOrNotify)
        {
            if (string.IsNullOrEmpty(text))
                return;
            OrderValidationLabel.Foreground = (errorOrNotify ? Brushes.Tomato : Brushes.CornflowerBlue);
            OrderTextBox.BorderBrush = OrderValidationLabel.Foreground;
            OrderTextBox.SelectionBrush = Brushes.Tomato;
            OrderValidationLabel.Content = text;
            
        }

        private void OnAddNewSourceFolderButtonClick(object sender, RoutedEventArgs e)
        {
            SelectSourceFolder();
        }

        private void OnRemoveSelectedSourceFolderButtonClick(object sender, RoutedEventArgs e)
        {
            if (SourceLocationsComboBox.SelectedIndex > -1)
            {
                SourceLocationsComboBox.Items.RemoveAt(SourceLocationsComboBox.SelectedIndex);
            }
        }

        private void OnClearAllSourceFoldersButtonClick(object sender, RoutedEventArgs e)
        {
            if(SourceLocationsComboBox.Items.Count>0)
                SourceLocationsComboBox.Items.Clear();
        }

        private void OnDestinationBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            SelectDestinationFolder();
        }

        private void OnOrderTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateOrderInput();
        }
    }
}
