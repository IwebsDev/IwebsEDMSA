using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsBalance : CsPrint
    {
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string CATEGORIE { get; set; }
        [DataMember]
        public string LIBELLECATEGORIE { get; set; }
        [DataMember]
        public string TYPECLIENT { get; set; }
        [DataMember]
        public bool STATUT { get; set; }
        [DataMember]
        public decimal SOLDE { get; set; }
        [DataMember]
        public int PASSAGE { get; set; }

        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string NOMABON { get; set; }
        [DataMember]
        public string ADRESSE { get; set; }
        [DataMember]
        public bool IsSelect { get; set; }

        [DataMember]
        public decimal PERIODE1 { get; set; }
        [DataMember]
        public decimal PERIODE2 { get; set; }
        [DataMember]
        public decimal PERIODE3 { get; set; }
        [DataMember]
        public decimal PERIODE4 { get; set; }
        [DataMember]
        public decimal PERIODE5 { get; set; }
        [DataMember]
        public decimal PERIODE6 { get; set; }
        [DataMember]
        public decimal PERIODE7 { get; set; }
        [DataMember]
        public decimal TOTAL { get; set; }
    }
}

   
  //      [DataMember]
  //      public decimal PERIODE1 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE2 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE3 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE4 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE5 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE6 { get; set; }
  //      [DataMember]
  //      public decimal PERIODE7 { get; set; }
  //      [DataMember]
  //      public decimal TOTAL { get; set; }