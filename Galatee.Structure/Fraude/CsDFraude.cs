using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
        [DataContract] 
  public  class CsDFraude
    {
            [DataMember]
            public int PK_ID { get; set; }
            [DataMember]
            public byte[] NUMDEM { get; set; }
            [DataMember]
            public string TYPEDEMANDE { get; set; }
            [DataMember]
            public Nullable<int> FK_IDCENTRE { get; set; }
            [DataMember]
            public Nullable<int> FK_PRODUIT { get; set; }
            [DataMember]
            public Nullable<int> FK_CLIENTFRAUDE { get; set; }
            [DataMember]
            public Nullable<int> FK_FRAUDE { get; set; }
            [DataMember]
            public Nullable<int> FK_COMPTEURFRD { get; set; }
            [DataMember]
            public Nullable<int> FK_CONSOMMATION { get; set; }
            [DataMember]
            public Nullable<int> FK_IDTYPEDEMANDE { get; set; }
    }
}
