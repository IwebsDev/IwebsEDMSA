using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtatProcesVerbal : CsPrint
    {
        [DataMember]
        public int DevisId { get; set; }
        [DataMember]
        public System.String Route { get; set; }
        [DataMember]        
        public  System.String OriginalRoute {get;set;}
        [DataMember]
        public  System.String Nom {get;set;}
        [DataMember]
        public  System.String Commune {get;set;}
        [DataMember]
        public  System.String NumLot {get;set;}
        [DataMember]
        public  System.String Quartier {get;set;}
        [DataMember]
        public  System.String NumTel {get;set;}
        [DataMember]
        public  System.String Rue {get;set;}
        [DataMember]
        public  System.String NumPoteauProche {get;set;}
        [DataMember]
        public DateTime? DateValidation { get; set; }
        [DataMember]
        public  System.String TypeDevis {get;set;}
        [DataMember]
        public  System.String Produit {get;set;}
        [DataMember]
        public  System.String Centre {get;set;}
        [DataMember]
        public  System.Decimal? TotalTTC {get;set;}
        [DataMember]
        public  System.String NumDevis {get;set;}
        [DataMember]
        public  int? Ordre {get;set;}
        [DataMember]
        public  DateTime? DateDebutTrvx {get;set;}
        [DataMember]
        public DateTime? DateFinTrvx { get; set; }
        [DataMember]
        public  System.String Matricule {get;set;}
        [DataMember]
        public  System.String Chefequipe {get;set;}
        [DataMember]
        public  System.String ProcesVerbal {get;set;}
        [DataMember]
        public  System.String NumeroCTR {get;set;}
        [DataMember]
        public  System.String MeterSize {get;set;}
        [DataMember]
        public  System.String TYPECTR {get;set;}
        [DataMember]
        public  System.String MarqueCTR {get;set;}
        [DataMember]
        public  System.String Adresse {get;set;}
        [DataMember]
        public  int? IndexPoseCTR {get;set;}
        [DataMember]
        public  int? AnneeFabricationCTR {get;set;}
        [DataMember]
        public  System.String NearestRoute {get;set;}
        [DataMember]
        public  System.String NumeroGPS {get;set;}
        [DataMember]
        public System.String MatriculeChefEquipe { get; set; }
        [DataMember]
        public System.String NomChefEquipe { get; set; }
        [DataMember]
        public System.String METMOYCONTROLE { get; set; }
        [DataMember]
        public System.DateTime? DATECONTROLE { get; set; }
        [DataMember]
        public System.String VOLUMETERTRVX { get; set; }
        [DataMember]
        public System.String DEGRADATIONVOIE { get; set; }
        [DataMember]
        public int? NOTE { get; set; }
        [DataMember]
        public System.String EmplacementCRT { get; set; }
        
    }
}
