using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCprofac  : CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string CATCLI { get; set; }
        [DataMember]
        public string CATCLILABEL { get; set; }
        [DataMember]
        public string CODCONSO { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string MOIS { get; set; }
        [DataMember]
        public int? CONSOFAC { get; set; }
        [DataMember]
        public decimal PERCENT { get; set; }
        [DataMember]
        public decimal? TOTAL { get; set; }
        [DataMember]
        public int CONSOFAC1 { get; set; }
        [DataMember]
        public decimal PERCENT1 { get; set; }
        [DataMember]
        public decimal TOTAL1 { get; set; }
        [DataMember]
        public int CONSOFAC2 { get; set; }
        [DataMember]
        public decimal PERCENT2 { get; set; }
        [DataMember]
        public decimal TOTAL2 { get; set; }
        [DataMember]
        public int CONSOFAC3 { get; set; }
        [DataMember]
        public decimal PERCENT3 { get; set; }
        [DataMember]
        public decimal TOTAL3 { get; set; }
        [DataMember]
        public int CONSOFAC4 { get; set; }
        [DataMember]
        public decimal PERCENT4 { get; set; }
        [DataMember]
        public decimal TOTAL4 { get; set; }
        [DataMember]
        public int CONSOFAC5 { get; set; }
        [DataMember]
        public decimal PERCENT5 { get; set; }
        [DataMember]
        public decimal TOTAL5 { get; set; }
        [DataMember]
        public int CONSOFAC6 { get; set; }
        [DataMember]
        public decimal PERCENT6 { get; set; }
        [DataMember]
        public decimal TOTAL6 { get; set; }
        [DataMember]
        public int CONSOFAC7 { get; set; }
        [DataMember]
        public decimal PERCENT7 { get; set; }
        [DataMember]
        public decimal TOTAL7 { get; set; }
        [DataMember]      
        public int CONSOFAC8 { get; set; }
        [DataMember]
        public decimal PERCENT8 { get; set; }
        [DataMember]
        public decimal TOTAL8 { get; set; }
        [DataMember]
        public int CONSOFAC9 { get; set; }
        [DataMember]
        public decimal PERCENT9 { get; set; }
        [DataMember]
        public decimal TOTAL9 { get; set; }
        [DataMember]
        public int CONSOFAC10 { get; set; }
        [DataMember]
        public decimal PERCENT10 { get; set; }
        [DataMember]
        public decimal TOTAL10 { get; set; }
        [DataMember]
        public int CONSOFAC11 { get; set; }
        [DataMember]
        public decimal PERCENT11 { get; set; }
        [DataMember]
        public decimal TOTAL11 { get; set; }
        [DataMember]
        public int CONSOFAC12 { get; set; }
        [DataMember]
        public decimal PERCENT12 { get; set; }
        [DataMember]
        public decimal TOTAL12 { get; set; }
        [DataMember]
        public string PRODUCTLABEL { get; set; }
        [DataMember]
        public string DIAMETRELABEL { get; set; }
        [DataMember]
        public string CENTRELABEL { get; set; }
        [DataMember]
        public string DIAMETRE { get; set; }
        [DataMember]
        public string Tournee { get; set; }
        [DataMember]
        public string TARIF { get; set; }
        [DataMember]
        public string Localisation { get; set; }
        [DataMember]
        public int NumMois { get; set; }
        [DataMember]
        public string PRODUIT1 { get; set; }
        [DataMember]
        public string PRODUIT2 { get; set; }
        [DataMember]
        public string PRODUIT3 { get; set; }
        [DataMember]
        public string ANNEE { get; set; }
    }
}
