using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeCompte
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public Nullable<bool> AVECFILTRE { get; set; }
        [DataMember] public string TABLEFILTRE { get; set; }
        [DataMember] public string TABLEFILTRE1 { get; set; }
        [DataMember] public string TABLEFILTRE2 { get; set; }
        [DataMember] public string DC { get; set; }
        [DataMember] public bool SIGNE { get; set; }
        [DataMember] public string COPERASSOCIE { get; set; }
        [DataMember] public string VALEURMONTANT { get; set; }
        [DataMember] public string SOUSCOMPTE { get; set; }

    }
}
