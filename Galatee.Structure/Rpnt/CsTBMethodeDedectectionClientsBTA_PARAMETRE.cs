using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
       [DataContract]
   public class CsTBMethodeDedectectionClientsBTA_PARAMETRE
    {
             [DataMember]
        public int MethDectParamatre_ID { get; set; }
             [DataMember]
        public Nullable<int> MethDection { get; set; }
             [DataMember]
        public Nullable<int> Parametre { get; set; }

        //     [DataMember]
        //public virtual CsREFMETHODEDEDETECTIONCLIENTSBTA REFMETHODEDEDETECTIONCLIENTSBTA { get; set; }
        //     [DataMember]
        //public virtual CsTBPARAMETRE TBPARAMETRE { get; set; }
    }
}
