using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace  Galatee.Structure

{
    [DataContract]
    public class CsAbonneBTA
    {


        [DataMember]
        public String CodeExploitation { get; set; }
        [DataMember]
        public String ReferenceClient { get; set; }
        [DataMember]
        public int? TypeClient { get; set; }
        [DataMember]
        public string TYPABON_ID { get; set; }
        //[DataMember]
        //public Char? TYPABON_ID { get; set; }
        [DataMember]
        public String Nom { get; set; }
        [DataMember]
        public String Prenoms { get; set; }
        [DataMember]
        public Guid Contrat_ID { get; set; }
        [DataMember]
        public String Branchement_ID { get; set; }
        [DataMember]
        public Guid? ResultatDernierControle_ID { get; set; }
        [DataMember]
        public int StatutContrat { get; set; }
        [DataMember]
        public int? GroupeDeFacturation { get; set; }
        [DataMember]
        public Double? PuissanceSouscrite { get; set; }
        [DataMember]
        public int? TypeContratID { get; set; }
        [DataMember]
        public String TypeTarif_ID { get; set; }
        [DataMember]
        public string UsageAbonnement_ID { get; set; }
        //public Char? UsageAbonnement_ID { get; set; }
        [DataMember]
        public DateTime? DateAbonnement { get; set; }
        [DataMember]
        public DateTime? DateStatutContrat { get; set; }
        [DataMember]
        public DateTime? DateProchainControlePostFraude { get; set; }
        [DataMember]
        public Guid? ConstatDuControlePostFraude_ID { get; set; }

        [DataMember]
        public String Numero_Compteur { get; set; }
        [DataMember]
        public int? TypeBranchement_ID { get; set; }
        [DataMember]
        public Guid? Tournee_Id { get; set; }
        [DataMember]
        public int? ResultatDernierControle_Value { get; set; }
        [DataMember]
        public DateTime? DateDernierControle { get; set; }
        [DataMember]
        public String MatriculeAgentDernierControle { get; set; }

    }
}
