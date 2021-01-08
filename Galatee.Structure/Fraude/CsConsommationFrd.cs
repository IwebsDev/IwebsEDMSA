using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
        [DataContract]
    public class CsConsommationFrd : CsPrint
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public bool IsEstimationParAppareil { get; set; }
        [DataMember]
        public int ConsommationEstimee { get; set; }
        [DataMember]
        public int ConsommationRetrogradation { get; set; }
        [DataMember]
        public int ConsommationDejaFacturee { get; set; }
        [DataMember]
        public int ConsommationAFacturer { get; set; }
        [DataMember]
        public int ConsommationMensuelleAFacturer { get; set; }
        [DataMember]
        public int MontantHTConsommation { get; set; }
        [DataMember]
        public int MontantHTPrestationEDM { get; set; }
        [DataMember]
        public int MontantHTPrestationRemboursable { get; set; }
        [DataMember]
        public int MontantHTRegularisationDevis { get; set; }
        [DataMember]
        public Nullable<decimal> TauxTVA { get; set; }
        [DataMember]
        public int NombreMoisAFacturer { get; set; }
        [DataMember]
        public int MontantFactureTTC { get; set; }
        [DataMember]
        public bool IsFactureAuForfait { get; set; }
        [DataMember]
        public bool IsFactureAnnulee { get; set; }
        [DataMember]
        public int OrdreTraitement { get; set; }
        [DataMember]
        public string CodeTVA { get; set; }
        [DataMember]
        public int MontantTVAConsommation { get; set; }
        [DataMember]
        public int FK_IDFRAUDE { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
    }
}
