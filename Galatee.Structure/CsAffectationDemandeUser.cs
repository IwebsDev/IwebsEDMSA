using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAffectationDemandeUser
    {
        [DataMember]
        public System.Guid PK_ID { get; set; }
        [DataMember]
        public string CODEDEMANDE { get; set; }
        [DataMember]
        public string MATRICULEUSER { get; set; }
        [DataMember]
        public int FK_IDETAPE { get; set; }
        [DataMember]
        public int FK_IDUSERAFFECTER { get; set; }
        [DataMember]
        public System.Guid OPERATIONID { get; set; }
        [DataMember]
        public System.Guid WORKFLOWID { get; set; }
        [DataMember]
        public int CENTREID { get; set; }
        [DataMember]
        public int FK_IDETAPEFROM { get; set; }
        [DataMember]
        public int FK_IDDEMANDE { get; set; }
        [DataMember]
        public string MATRICULEUSERCREATION { get; set; }
    }
}
