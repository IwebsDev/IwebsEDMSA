using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class RELEVEUR
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string NUMRELEVEUR { get; set; }
        [DataMember]
        public string MATRICULE { get; set; }
        [DataMember]
        public int FERMEQUOT { get; set; }
        [DataMember]
        public int FERMEREAL { get; set; }
        [DataMember]
        public string PORTABLE { get; set; }
        [DataMember]
        public string FONCTION { get; set; }
        [DataMember]
        public string TOURNEE { get; set; }
    }
}
