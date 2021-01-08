using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsComptDispo
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
        [DataMember]
        public int FK_IDDIAMETRE { get; set; }
        [DataMember]
        public int FK_IDMCOMPT { get; set; }
        [DataMember]
        public int FK_IDTCOMPT { get; set; }
        [DataMember]
        public string COMPTEUR { get; set; }
        [DataMember]
        public string DIAMETRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string MCOMPT { get; set; }
        [DataMember]
        public string TCOMPT { get; set; }
        [DataMember]
        public string CADCOMPT { get; set; }
        [DataMember]
        public int? SORTIES { get; set; }
        [DataMember]
        public string ETATCOMPT { get; set; }
        [DataMember]
        public string ANNEEFAB { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }
}









