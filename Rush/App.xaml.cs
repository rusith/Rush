using System.Windows;
using Rush.Controllers;

namespace Rush
{
    public partial class App 
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {

            var rushController = new RushController();
            rushController.StartProgram();
        }
    }
}
