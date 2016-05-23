using Rush.Windows;

namespace Rush.Controllers
{

    public class RushController
    {

        #region Fields

        private readonly MainWindow _mainWindow = null;

        #endregion


        #region Constructors

        public RushController()
        {
            _mainWindow = new MainWindow();
        }

        #endregion


        #region Methods

        public void StartProgram()
        {
            if (_mainWindow != null)
                _mainWindow.Show();
        }

        #endregion

    }
}
