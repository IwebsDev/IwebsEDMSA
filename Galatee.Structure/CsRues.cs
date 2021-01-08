using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class CsRues : CsPrint
    {


       //[DataMember] public string CENTRE { get; set; }
       //[DataMember] public string COMMUNE { get; set; }
       [DataMember]public string CODE { get; set; }
       [DataMember] public string LIBELLE { get; set; }
       [DataMember] public string SECTEUR { get; set; }
       [DataMember] public string COMPRUE { get; set; }
       [DataMember] public string NUMRUE { get; set; }
       [DataMember] public string TRANS { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       //[DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDSECTEUR { get; set; }


       //public string OriginalPK_FK_CENTRE { get; set; }
       //[DataMember]
       //public string OriginalPK_RUE { get; set; }
       //[DataMember]
       //public string OriginalPK_FK_COMMUNE { get; set; }
       //[DataMember]
       //public string LIBELLECENTRE { get; set; }
       //[DataMember]
       //public string LIBELLECOMMUNE { get; set; }
    }
}
