using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Galatee.Structure
{
    [DataContract]
    public class CsPersonePhysique :CsPrint
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public Nullable<System.DateTime> DATENAISSANCE { get; set; }
       [DataMember] public string NUMEROPIECEIDENTITE { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
       [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
       [DataMember] public Nullable<int> FK_IDPIECEIDENTITE { get; set; }
       [DataMember] public string NOMABON { get; set; }
       [DataMember] public Nullable<int> FK_IDCLIENT { get; set; }
    }
}
