using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure.Rpnt;
using System.Collections.ObjectModel;

namespace Galatee.Structure.Rpnt.ViewModels
{
    public class ViewModelGestionClientBTA
    {
        #region Liste
            public ObservableCollection<CsTBCAMPAGNECONTROLEBTA> ListeCampagne { get; set; }
            //public ObservableCollection<CsREFSTATUt> ListeStatut { get; set; }
            //public ObservableCollection<CsREFEXPLOITATION> ListeExploitation { get; set; }
            public ObservableCollection<CsTBBRANCHEMENT> ListeBranchement { get; set; }
        #endregion

        #region Champs
            //public string LibelleCampagne { get; set; }
            //public string LibelleExploitation { get; set; }
            //public string DateCreation { get; set; }
            //public string AgentCreation { get; set; }
            //public string LibelleStaut { get; set; }
            //public string NombreBranchement { get; set; }
        #endregion
                public ViewModelGestionClientBTA()
        {
        }
    }
}
