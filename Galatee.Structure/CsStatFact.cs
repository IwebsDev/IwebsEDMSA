using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsStatFact  : CsPrint
    {
        [DataMember]public string CENTRE { get; set; }
        [DataMember]public string CATEGORIE { get; set; }
        [DataMember] public Int64  CONSOMATIONACTIVE { get; set; }
        [DataMember] public Int64  CONSOMATIONFACTURE { get; set; }
        [DataMember] public Int64  CONSOMATIONREACTIVE { get; set; }
        [DataMember] public decimal MONTANT { get; set; }
        [DataMember] public decimal MAJORATION { get; set; }
        [DataMember] public decimal MINORATION { get; set; }
        [DataMember] public decimal PENALITE { get; set; }
        [DataMember] public decimal PRIMEFIXE { get; set; }
        [DataMember] public decimal ENTRETIEN { get; set; }
        [DataMember] public decimal ECLAIRAGEPUBLIQUE { get; set; }
        [DataMember] public decimal MONTANTHT { get; set; }
        [DataMember] public decimal MONTANTTVA { get; set; }
        [DataMember] public decimal MONTANTTC { get; set; }
        [DataMember] public Int64 NOMBRE { get; set; }
         
    }
}
