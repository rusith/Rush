
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxOptions = System.Windows.Forms.MessageBoxOptions;

namespace Rush.Windows
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
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
                if (folderDialogResult != System.Windows.Forms.DialogResult.OK) continue;
                var selectedDir = new DirectoryInfo(folderDialog.SelectedPath);
                if (!selectedDir.Exists)
                {
                    var notExistResult = MessageBox.Show(
                    "The selected directory not exist in the given path. do you want to create that folder ?",
                    "Folder dose not exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    if (notExistResult != System.Windows.Forms.DialogResult.Yes) continue;
                    try
                    {
                        selectedDir.Create();
                        if (!selectedDir.Exists)
                            throw new Exception("Faild to create the folder");
                    }
                    catch (Exception)
                    {
                        var cannotCreateDialogResult =
                            MessageBox.Show(
                                string.Format(
                                    "Cannot create director ({0}) . \nDo you want to try another location?",
                                    selectedDir.FullName), "Cannot Create Folder.", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        if (cannotCreateDialogResult == System.Windows.Forms.DialogResult.Yes)
                            continue;
                        return;
                    }
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
    }
}
