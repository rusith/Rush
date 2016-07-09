using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Rush.Windows
{
    /// <summary>
    /// Interaction logic for ShareWindow.xaml
    /// </summary>
    public partial class ShareWindow 
    {
        public ShareWindow()
        {
            InitializeComponent();
        }

        private void OnFacebookClick(object sender, MouseButtonEventArgs e)
        {
            var url = string.Format("http://www.facebook.com/share.php?u={0}", "http://rusith.github.io/Rush/");
            Process.Start(url);
            Properties.Settings.Default.sharedOnce = true;
            Properties.Settings.Default.Save();
        }

        private void OnTwitterClick(object sender, MouseButtonEventArgs e)
        {
            var url = "http://twitter.com/share?url={0}&text={1}&via={2}";
            url = string.Format(url, "http://rusith.github.io/Rush/", "Organize your music library using Rush.", "Rush");

            Process.Start(url);
            if (Properties.Settings.Default.sharedOnce)
                return;
            Properties.Settings.Default.sharedOnce = true;
            Properties.Settings.Default.Save();
        }
    }
}
