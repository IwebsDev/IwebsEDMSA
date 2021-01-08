using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
   public class CsDemandeReclamation
    {
        #region  collection object
       
        #endregion

        #region Object
        [DataMember] public CsReclamationRcl ReclamationRcl  { get; set; }
        [DataMember] public CsDemandeBase LaDemande { get; set; }
        [DataMember] public CsClient LeClient { get; set; }
        [DataMember] public List<ObjDOCUMENTSCANNE> DonneDeDemande { get; set; }
        #endregion
    }
}
