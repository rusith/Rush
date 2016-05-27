﻿using System.Runtime.InteropServices;
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
            _mainWindow = new MainWindow(this);
        }

        #endregion


        #region Methods

        public void StartProgram()
        {
            if (_mainWindow != null)
                _mainWindow.Show();
        }

        public void ShowFileOrderHelpWindow()
        {
            var helpWindow = new FileOrderHelpWindow();
            helpWindow.ShowDialog();
        }

        #endregion

    }
}
