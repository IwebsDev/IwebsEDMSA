using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtatDevis : CsPrint
    {
        [DataMember]
        public int DevisId { get; set; }
        [DataMember]
        public DateTime? DateReglement { get; set; }
        [DataMember]
        public string DateCreation { get; set; }
        [DataMember]
        public string AG { get; set; }
        [DataMember]
        public string DCAISSE { get; set; }
        [DataMember]
        public string DEMANDE { get; set; }
        [DataMember]
        public string DATEBRANCHEMENT { get; set; }
        [DataMember]
        public string DateLimite { get; set; }
        [DataMember]
        public string TypeDevis { get; set; }
        [DataMember]
        public string Etape { get; set; }
        [DataMember]
        public string Montant { get; set; }
        [DataMember]
        public string Produit { get; set; }
        [DataMember]
        public string Centre { get; set; }
        [DataMember]
        public string Designation { get; set; }
        [DataMember]
        public int? Quantite { get; set; }
        [DataMember]
        public decimal? Prix_Unitaire { get; set; }
        [DataMember]
        public float? montantHT { get; set; }
        [DataMember]
        public float? montantTTC { get; set; }
        [DataMember]
        public float? totalTTC { get; set; }
        [DataMember]
        public string Numdevis { get; set; }
        [DataMember]
        public byte? Ordre { get; set; }
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public int? QuantiteRemisEnStock { get; set; }
        [DataMember]
        public decimal? RembourserHT { get; set; }
        [DataMember]
        public decimal? RembourserTTC { get; set; }
        [DataMember]
        public decimal? TotalRemboursableHT { get; set; }
        [DataMember]
        public decimal? TotalRemboursableTTC { get; set; }
        [DataMember]
        public decimal? Payment { get; set; }
        [DataMember]
        public string NumeroCTR { get; set; }
        [DataMember]
        public string MeterSize { get; set; }
        [DataMember]
        public string NumPoteauProche { get; set; }
        [DataMember]
        public string TYPECTR { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public decimal? DEPOSIT { get; set; }
        [DataMember]
        public float? Balance { get; set; }
        [DataMember]
        public string NomAgent { get; set; }
        [DataMember]
        public string MatriculeAgent { get; set; }

    }
}
