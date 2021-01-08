using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCtax : CsPrint 
    {
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CODE { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public decimal TAUX { get; set; }
       [DataMember] public string TYPETAXE { get; set; }
       [DataMember] public string COMPTE { get; set; }
       [DataMember] public System.DateTime DEBUTAPPLICATION { get; set; }
       [DataMember] public Nullable<System.DateTime> FINAPPLICATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDTYPETAXE { get; set; }
       [DataMember] public int PK_ID { get; set; }
      [DataMember]  public string LIBELLETYPETAXE { get; set; }


    }

}









