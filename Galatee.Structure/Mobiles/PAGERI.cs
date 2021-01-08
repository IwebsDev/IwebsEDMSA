using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class PAGERI
    {
        [DataMember]
        public string NUMLOTRI { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string REFCLIENT { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public int POINT { get; set; }
        [DataMember]
        public string CASRELEVE { get; set; }
        [DataMember]
        public string ORDTOUR { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string NUMTOURNEE { get; set; }
        [DataMember]
        public int? STATUSRELEVE { get; set; }
        [DataMember]
        public string STATUSTSP { get; set; }
    }
}
