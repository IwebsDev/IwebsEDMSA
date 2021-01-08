using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
      [DataContract]
    public class CsRefOrigineScelle
    {
        [DataMember]
        public int Origine_ID { get; set; }
        [DataMember]
        public string Origine_Libelle { get; set; }
        [DataMember]
        public int Longueur_ScelleID { get; set; }

    }
}
