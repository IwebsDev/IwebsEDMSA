using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRegCli : CsPrint
    {
       [DataMember] public string CODE { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CATEGORIE { get; set; }

       [DataMember] public string NOM { get; set; }
       [DataMember] public string ADR1 { get; set; }
       [DataMember] public string ADR2 { get; set; }
       [DataMember] public string CODPOS { get; set; }
       [DataMember] public string BUREAU { get; set; }
       [DataMember] public string TRAITFAC { get; set; }
       [DataMember] public string TRANS { get; set; }
       [DataMember] public Nullable<int> REFERENCEPUPITRE { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public string LIBELLE { get; set; }

       [DataMember] public bool IsSelect { get; set; }
       [DataMember] public List<string > LstCentre { get; set; }
    }
}
