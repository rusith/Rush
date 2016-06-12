using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
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
    public partial class DuplicatesWindow
    {
        private readonly ObservableCollection<FileInformation> _files;
        public DuplicatesWindow(ref ObservableCollection<FileInformation> files )
        {
            InitializeComponent();
            _files = files;
            SetupTreeView();
        }


        private void SetupTreeView()
        {
            var dupis = _files.Select(fi => fi.Duplicate).ToList().Distinct().ToList();
            var res = dupis.GroupBy(d => d.SourceFile.Name).ToList();
            foreach (var g in res)
            {
                var twi = new TreeViewItem {Header = g.Key};
                foreach (var item in g)
                {
                    twi.Items.Add(item.SourceFile.FullName);
                }
                MainTreeView.Items.Add(twi);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
