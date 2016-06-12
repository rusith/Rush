using System.Collections.Generic;
using Rush.Models;
using Rush.Windows;

namespace Rush.Controllers
{

    public class RushController
    {

        #region Fields

        private readonly MainWindow _mainWindow ;

        #endregion

        #region Properties

        public HashSet<string> Sources { get; set; }
        public string Destination { get; set; }
        public OrganizeOrder Order { get; set; }

        #endregion

        #region Constructors

        public RushController()
        {
            _mainWindow = new MainWindow(this);
        }

        #endregion

        #region Methods
        
        public void StartProgram()
        {
            _mainWindow?.Show();
        }

        public void Organize()
        {
            if (Sources != null && Sources.Count > 0 && (!string.IsNullOrEmpty(Destination)) && Order != null)
            {

            }

        }

        public void ShowFileOrderHelpWindow()
        {
            var helpWindow = new FileOrderHelpWindow();
            helpWindow.ShowDialog();
        }

        #endregion

    }
}
