using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class cClient
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string NOMABON { get; set; }
        [DataMember]
        public string COMMUNE { get; set; }
        [DataMember]
        public string QUARTIER { get; set; }
        [DataMember]
        public string RUE { get; set; }
        [DataMember]
        public string PORTE { get; set; }
        [DataMember]
        public string TOURNEE { get; set; }
        [DataMember]
        public string COMPTEUR { get; set; }
        [DataMember]
        public decimal SOLDE { get; set; }
        [DataMember]
        public string CATEGORIECLIENT { get; set; }
        [DataMember]
        public string CNI { get; set; }
        [DataMember]
        public string TELEPHONE { get; set; }

        
        //public override string ToString()
        //{
        //    return (string.IsNullOrEmpty(NOMABON)) ? CLIENT : NOMABON;
        //}
    }
    ////[CollectionDataContract]
    ////public class cClientCollection : List<cClient>
    ////{
 
    ////}
}
