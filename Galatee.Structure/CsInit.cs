using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsInit
    {
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public DateTime ? DMAJ { get; set; }
        [DataMember]
        public string NTABLE { get; set; }
        [DataMember]
        public string ZONE { get; set; }
        [DataMember]
        public string CONTENU { get; set; }
        [DataMember]
        public string OBLIG { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public string TRANS { get; set; }
        [DataMember]
        public byte[] ROWID { get; set; }

    }

}
