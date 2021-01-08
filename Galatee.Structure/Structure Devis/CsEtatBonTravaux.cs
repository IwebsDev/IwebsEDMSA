using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtatBonTravaux : CsPrint
    {
        [DataMember]
        public int DevisId { get; set; }
        [DataMember]
        public string NumeroDevis { get; set; }
        [DataMember]
        public string Route { get; set; }
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public string Commune { get; set; }
        [DataMember]
        public string NumLot { get; set; }
        [DataMember]
        public string Quartier { get; set; }
        [DataMember]
        public string NumTel { get; set; }
        [DataMember]
        public string Rue { get; set; }
        [DataMember]
        public string NumPoteauProche { get; set; }
        [DataMember]
        public DateTime? DateValidation { get; set; }
        [DataMember]
        public string TypeDevis { get; set; }
        [DataMember]
        public string Produit { get; set; }
        [DataMember]
        public string Centre { get; set; }
        [DataMember]
        public decimal? TotalTTC { get; set; }
        [DataMember]
        public string NumDevis { get; set; }
        [DataMember]
        public int Ordre { get; set; }
        [DataMember]
        public DateTime? DateDebutTrvx { get; set; }
        [DataMember]
        public DateTime? DateFinTrvx { get; set; }
        [DataMember]
        public string Matricule { get; set; }
        [DataMember]
        public string ChefEquipe { get; set; }
        [DataMember]
        public string ProcesVerbal { get; set; }
        [DataMember]
        public string NumeroCTR { get; set; }
        [DataMember]
        public string MeterSize { get; set; }
        [DataMember]
        public string TYPECTR { get; set; }
        [DataMember]
        public string MarqueCTR { get; set; }
        [DataMember]
        public string Adresse { get; set; }
        [DataMember]
        public int? IndexPoseCTR { get; set; }
        [DataMember]
        public DateTime? AnneeFabricationCTR { get; set; }
        [DataMember]
        public string NearestRoute { get; set; }
        [DataMember]
        public string NumeroGPS { get; set; }
        [DataMember]
        public System.String EmplacementCRT { get; set; }

    }
}
