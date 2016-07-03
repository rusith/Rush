using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Rush.Models;

namespace Rush.Windows
{
    public partial class LogWindow
    {
        private int _fileCount;
        private int _skipped;
        private List<LogItem> _log; 

        public LogWindow(int filecount,int skipped,List<LogItem> log )
        {
            _fileCount = filecount;
            _skipped = skipped;
            _log = log;
            InitializeComponent();
        }

        private void OnWindowActivated(object sender, EventArgs e)
        {
            SetupControls();
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
            InformationLabel.Content = string.Format("Copied {0} From {1}, Skipped {2} Files", (_fileCount - _skipped),
                _fileCount, _skipped);
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

    }
}
