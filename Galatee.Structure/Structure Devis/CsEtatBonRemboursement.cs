using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEtatBonRemboursement : CsPrint
    {
        [DataMember]
        public int DevisId { get; set; }
        [DataMember]
        public string RefClient { get; set; }
        [DataMember]
        public string NumDevis { get; set; }
        [DataMember]
        public int? ordre { get; set; }
        [DataMember]
        public string Nom { get; set; }
        [DataMember]
        public string Centre { get; set; }
        [DataMember]
        public string TypeDevis { get; set; }
        [DataMember]
        public string Produit { get; set; }
        [DataMember]
        public decimal? TotalARembourserHT { get; set; }
        [DataMember]
        public decimal? TotalARembourserTTC { get; set; }
        [DataMember]
        public decimal? TotalRegleHT { get; set; }
        [DataMember]
        public decimal? TotalRegleTTC { get; set; }
        [DataMember]
        public decimal? TotalARembourserHTOrdre { get; set; }
        [DataMember]
        public decimal? TotalARembourserTTCOrdre { get; set; }
        [DataMember]
        public decimal? TotalRegleHTOrdre { get; set; }
        [DataMember]
        public decimal? TotalRegleTTCOrdre { get; set; }
        [DataMember]
        public string DepositReceipt { get; set; }
        [DataMember]
        public string InvoiceReceipt { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal? MontantDevisHT { get; set; }
        [DataMember]
        public decimal? MontantDevisTTC { get; set; }
        [DataMember]
        public string MatriculeTranscaisb { get; set; }
        [DataMember]
        public decimal? MontantTranscaisb { get; set; }
        [DataMember]
        public string CaisseTranscaisb { get; set; }
        [DataMember]
        public string AcquitTranscaisb { get; set; }
        [DataMember]
        public System.String EmplacementCRT { get; set; }
    }
}
