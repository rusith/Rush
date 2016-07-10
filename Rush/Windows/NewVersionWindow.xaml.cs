using System.Diagnostics;
using System.Windows;

namespace Rush.Windows
{
    public partial class NewVersionWindow
    {
        private readonly string _downloadPath;
        private readonly string _html;
        public NewVersionWindow(string downloadPath,string html)
        {
            _downloadPath = downloadPath;
            _html = html;
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            WebView.NavigateToString(_html);
        }

        private void OnDownloadButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start(_downloadPath);
            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
