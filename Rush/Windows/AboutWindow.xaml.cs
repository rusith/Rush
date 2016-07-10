using Rush.Common;

namespace Rush.Windows
{
    public partial class AboutWindow 
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void CloseButtonOnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void OnWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            HeadLabel.Content = string.Format("Music File Organizer v{0}", RushConfiguration.Config.Version);
        }
    }
}
