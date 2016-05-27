using System.Windows;
namespace Rush.Windows
{
    public partial class FileOrderHelpWindow
    {
        public FileOrderHelpWindow()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
