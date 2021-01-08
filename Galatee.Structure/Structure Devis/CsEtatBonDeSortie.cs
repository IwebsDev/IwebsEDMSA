using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtatBonDeSortie : CsPrint
    {
        [DataMember]
        public int DevisId { get; set; }
        [DataMember]
        public DateTime? DateReglement { get; set; }
        [DataMember]
        public string TypeDevis { get; set; }
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
        public decimal? montantHT { get; set; }
        [DataMember]
        public decimal? montantTTC { get; set; }
        [DataMember]
        public decimal? totalTTC { get; set; }
        [DataMember]
        public string numdevis { get; set; }
        [DataMember]
        public int ordre { get; set; }
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
        public string Payment { get; set; }
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
        public decimal? Balance { get; set; }
        [DataMember]
        public string NomAgent { get; set; }
        [DataMember]
        public string MatriculeAgent { get; set; }
        [DataMember]
        public decimal? Taxe { get; set; }
        [DataMember]
        public string PoteauProche { get; set; }
        [DataMember]
        public string Commune { get; set; }
        [DataMember]
        public string Quartier { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Telephone { get; set; }
        [DataMember]
        public System.String EmplacementCRT { get; set; }

        [DataMember]
        public bool ? Isdefault { get; set; }
        #region INFORMATIONS ENTREPRISE

        [DataMember]
        public string NOMENTREPRISE { get; set; }
        [DataMember]
        public string SIGLE { get; set; }
        [DataMember]
        public string SLOGAN { get; set; }
        [DataMember]
        public string ADRESSEPRINCIPALE { get; set; }
        [DataMember]
        public string ADRESSESECONDAIRE { get; set; }
        [DataMember]
        public string TELEPHONEPRINCIPAL { get; set; }
        [DataMember]
        public string TELEPHONESECONDAIRE { get; set; }
        [DataMember]
        public string FAXPRINCIPALE { get; set; }
        [DataMember]
        public string FAXSECONDAIRE { get; set; }
        [DataMember]
        public string EMAILPRINCIPALE { get; set; }
        [DataMember]
        public string EMAILSECONDAIRE { get; set; }
        [DataMember]
        public string ACTIVITE { get; set; }
        [DataMember]
        public string PAYS { get; set; }
        [DataMember]
        public string SITEINTERNET { get; set; }
        [DataMember]
        public byte[] LOGO { get; set; } 

        #endregion
    }
}
