using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class EVENEMENT
    {
        [DataMember]
        public int POINT { get; set; }
        [DataMember]
        
        public string CENTRE { get; set; }
        [DataMember]

        public string LOT { get; set; }
        [DataMember]

        public string PRODUIT { get; set; }
        [DataMember]

        public string REFCLIENT { get; set; }
        [DataMember]

        public int NUMEVENEMENT { get; set; }
        [DataMember]

        public string ORDRE { get; set; }
        [DataMember]

        public int? INDEXEVT { get; set; }
        [DataMember]

        public string CAS { get; set; }
        [DataMember]

        public string DERPERF { get; set; }
        [DataMember]

        public string DERPERFN { get; set; }
        [DataMember]

        public int? CONSO { get; set; }
        [DataMember]

        public int? STATUS { get; set; }
        [DataMember]

        public DateTime? DATEEVT { get; set; }
        [DataMember]

        public string PERIODE { get; set; }
    }
}
