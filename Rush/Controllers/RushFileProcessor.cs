using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rush.Models;

namespace Rush.Controllers
{
    public class RushFileProcessor 
    {
        #region enums

        public enum FileMode
        {
            Copy = 1,
            Move = 2
        }

        #endregion

        #region private properties

        public FileTypesInfo FileTypes { get; private set; }
        public string[] Sources { get; private set; }
        public string Destination { get; private set; }
        public FileMode Mode { get; private set; }
        public bool Overwrite { get; private set; }
        public OrganizeOrder Order { get; private set; }
        #endregion

        

        #region methods

        public RushFileProcessor(FileTypesInfo fileTypes, string[] sources, string destination, FileMode fileMode,
            bool overwrite,OrganizeOrder order)
        {
            FileTypes = fileTypes;
            Sources = sources;
            Destination = destination;
            Mode = fileMode;
            Overwrite = overwrite;
            Order = order;
        }


        #endregion

        public void Process()
        {
            
        }
    }
}
