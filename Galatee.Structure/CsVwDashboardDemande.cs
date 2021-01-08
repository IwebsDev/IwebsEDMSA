using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    public class CsVwDashboardDemande
    {
        public int IDETAPE { get; set; }
        public string NOM { get; set; }
        public Nullable<int> FK_IDETAPEACTUELLE { get; set; }
        public System.Guid IDCIRCUIT { get; set; }
        public Nullable<int> NOMBREDEMANDE { get; set; }
        public System.Guid FK_IDGROUPEVALIDATIOIN { get; set; }
        public System.Guid FK_IDWORKFLOW { get; set; }
        public System.Guid FK_IDOPERATION { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int ORDRE { get; set; }
        public int FK_IDETAPE { get; set; }
        public Nullable<int> DUREE { get; set; }
        public Nullable<int> ALERTE { get; set; }
        public string CONTROLEETAPE { get; set; }
        public Nullable<int> FK_IDMENU { get; set; }
        public string WORKFLOWNAME { get; set; }
        public string NOMOPERATION { get; set; }
        public System.Guid RAFFECTATIONETAPE { get; set; }
        public bool IS_TRAITEMENT_LOT { get; set; }
        public System.Guid FK_IDDEMANDE { get; set; }
        public int STATUTDEMANDE { get; set; }

    }
}
