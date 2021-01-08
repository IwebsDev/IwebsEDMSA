using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsSales : CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string CATCLI { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public int CONSOFAC { get; set; }
        [DataMember]
        public decimal TOTPROTAX { get; set; }
        [DataMember]
        public decimal TOTPROHT { get; set; }
        [DataMember]
        public string CODCONSO { get; set; }
        [DataMember]
        public string TOURNEE { get; set; }
        [DataMember]
        public string TARIF { get; set; }
        [DataMember]
        public string REDEVENCE { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string NumMois { get; set; }
        [DataMember]
        public string Mois { get; set; }
        [DataMember]
        public string Zone { get; set; }
        [DataMember]
        public string Categorie { get; set; }
        [DataMember]
        public string libelleCategorie { get; set; }
        [DataMember]
        public int QuantiteWater { get; set; }
        [DataMember]
        public int QuantiteElectricity { get; set; }
        [DataMember]
        public int QuantiteSewerage { get; set; }
        [DataMember]
        public decimal MontantWater { get; set; }
        [DataMember]
        public decimal MontantElectricity { get; set; }
        [DataMember]
        public decimal MontantSewerage { get; set; }
        [DataMember]
        public decimal MontantWaterElecSew { get; set; }
        [DataMember]
        public decimal TOTAL { get; set; }
        
    }
}
