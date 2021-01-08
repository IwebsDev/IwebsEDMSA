using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRexo
    {
        [DataMember]
        public string PK_FK_CENTRE { get; set; }
         [DataMember]
        public string PK_FK_PRODUIT { get; set; }
         [DataMember]
         public string PK_REGCLI { get; set; }
         [DataMember]
         public string   EXFAV { get; set; }
         [DataMember]
         public string   EXFDOS { get; set; }
         [DataMember]
         public string   EXFPOL { get; set; }
         [DataMember]
         public string   TRAITFAC { get; set; }
         [DataMember]
         public DateTime? DATECREATION { get; set; }
         [DataMember]
         public DateTime? DATEMODIFICATION { get; set; }
         [DataMember]
         public string USERCREATION { get; set; }
         [DataMember]
         public string USERMODIFICATION { get; set; }
    }

}









