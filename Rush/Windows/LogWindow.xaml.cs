using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Rush.Models;

namespace Rush.Windows
{
    public partial class LogWindow
    {
        private readonly List<FileInformation> _files; 
        private readonly List<LogItem> _log;
        private readonly DirectoryInfo _destinationFolder;
        private readonly HashSet<string> _souces;

        public LogWindow(List<FileInformation> files ,List<LogItem> log,DirectoryInfo destination,HashSet<string> soureces)
        {
            _files = files;
            _log = log;
            _destinationFolder = destination;
            _souces = soureces;
            InitializeComponent();
        }

        private void OnComboBoxChanged(object sender, EventArgs e)
        {
            var selected = TypeComboBox.SelectedItem as string;
            if (selected == "All")
            {
                Log.ItemsSource = _log;
            }
            else
            {
                var type = LogItem.StringToType(TypeComboBox.SelectedItem as string);
                Log.ItemsSource = _log.Where(l => l.MessageType == type).ToList();
            }
        }

        private void SetupControls()
        {
            var processed = _files.Count(i => i.Processed);
            var copied = _files.Count(i => i.Copied);
            var deleted = _files.Count(i => i.SourceDeleted);
            InformationLabel.Content = string.Format("Processed {0} Files , Copied {1} Files ,Deleted {2} Files, From {3} Files",
                processed, copied,deleted,_files.Count);
            TypeComboBox.Items.Add("All");
            var types= _log.GroupBy(l => l.MessageType).Select(g=>g);
            foreach (var type in types)
            {
                TypeComboBox.Items.Add(LogItem.TypeToString(type.Key));
            }
            TypeComboBox.SelectedIndex = 0;
            TypeComboBox.SelectionChanged += OnComboBoxChanged;
            Log.ItemsSource = _log;
        }

        private void OnSaveToAFileButtonClick(object sender, RoutedEventArgs e)
        {
            if (_log == null || _log.Count <= 0)
                return;
            var fileDialog = new SaveFileDialog
            {
                Title = "Select an location to save log file",
                FileName = "RushLog.log",
                DefaultExt = "log",
                InitialDirectory = _destinationFolder.FullName,
                AddExtension = true
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK) return;
            var str = new StringBuilder();
            str.AppendLine(string.Format("Rush Media File Organizer Log File {0} ", DateTime.Now))
                .AppendLine(string.Format("Destination Folder : {0}", _destinationFolder.FullName))
                .AppendLine("Source Folders : ");
            foreach (var folder in _souces)
            {
                str.AppendLine("\t" + folder);
            }
            var processed = _files.Count(i => i.Processed);
            var copied = _files.Count(i => i.Copied);
            var deleted = _files.Count(i => i.SourceDeleted);
            str.AppendFormat("Processed {0} Files , Copied {1} Files ,Deleted {2} Files, From {3} Files",
                processed, copied, deleted, _files.Count);
            str.Append(Environment.NewLine + Environment.NewLine + Environment.NewLine);
            var groups = from f in _log
                group f by f.MessageType
                into gro
                select gro;
            foreach (var group in groups)
            {
                str.AppendFormat(Environment.NewLine + Environment.NewLine+"__________________________________{0}__________________________" +Environment.NewLine+Environment.NewLine,
                    LogItem.TypeToString(@group.Key).ToUpper());
                foreach (var i in @group)
                {
                    str.AppendLine(i.Message);
                }
            }
            str.Append("End");
            var text = str.ToString();
            var file = new FileInfo(fileDialog.FileNames[0]);
            var bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            var stream = file.OpenWrite();
            stream.Write(bytes,0,bytes.Length);
            stream.Close();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            SetupControls();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
