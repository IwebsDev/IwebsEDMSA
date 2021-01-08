using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFichierPersonnel : CsPrint
    {
        [DataMember]public string MATRICULE { get; set; }
        [DataMember]public string RUBRIQUE { get; set; }
        [DataMember]public decimal MONTANT { get; set; }
        [DataMember]public string MOISCPT { get; set; }
        [DataMember]public string FACTURE { get; set; }
        [DataMember]public string PERIODE { get; set; }
        [DataMember]public bool IsDirecteur { get; set; }
    }
}
