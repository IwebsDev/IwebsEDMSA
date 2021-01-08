using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Galatee.Structure

{
    [DataContract]
    public class CsAUDIT
    {
        [DataMember]
        public Guid AUDIT_ID { get; set; }
        [DataMember]
        public String MATRICULEAGENT { get; set; }
        [DataMember]
        public DateTime DATETRACE { get; set; }
        [DataMember]
        public String LIBELLE { get; set; }
        [DataMember]
        public String LIBELLEDETAILLE { get; set; }
        [DataMember]
        public String NOMMACHINE { get; set; }
        [DataMember]
        public Int32 TYPEACTION_ID { get; set; }
        [DataMember]
        public String CODEUO { get; set; }
        [DataMember]
        public Boolean? PERSISTANT { get; set; }

    }
}
