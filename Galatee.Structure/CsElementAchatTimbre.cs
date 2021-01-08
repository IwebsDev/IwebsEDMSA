using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    public class CsElementAchatTimbre : CsPrint
    {
       [DataMember] public string NUMDEM { get; set; }
       [DataMember] public Nullable<int> QUANTITE { get; set; }
       [DataMember] public Nullable<decimal> TAXE { get; set; }
       [DataMember] public Nullable<decimal> MONTANT { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDDEMANDE { get; set; }
       [DataMember] public int FK_IDTIMBRE { get; set; }
       [DataMember] public Nullable<int> FK_IDCOPER { get; set; }
       [DataMember] public Nullable<int> FK_IDTAXE { get; set; }


       [DataMember] public string CODE { get; set; }
       [DataMember] public string DESIGNATION { get; set; }
       [DataMember] public Nullable<decimal> PRIXUNITAIRE { get; set; }

        

    }
}
