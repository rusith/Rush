using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            var dupis = _files.Select(fi => fi.Duplicate).Distinct().ToList().ToList();
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
