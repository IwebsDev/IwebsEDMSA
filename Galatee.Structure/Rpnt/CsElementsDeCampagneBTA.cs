using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    public class CsElementsDeCampagneBTA
    {
        [DataMember]
        public System.Guid Campagne_ID { get; set; }
        
        [DataMember]
        public int ElementsDeCampagneBTA_ID { get; set; }
        
        [DataMember]
        public int Contrat_ID { get; set; }

        [DataMember]
        public System.DateTime DateSelection { get; set; }

        //[DataMember]
        //public string Branchement_ID { get; set; }

        [DataMember]
        public string ReferenceClient { get; set; }

        //[DataMember]
        //public System.Nullable<System.Guid> Lot_ID { get; set; }

        [DataMember]
        public string Libelle_Centre { get; set; }

        //[DataMember]
        //public System.Nullable<int> StatutLot_ID { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateCreation { get; set; }

        [DataMember]
        public string MatriculeAgentCreation { get; set; }

        [DataMember]
        public string MatriculeAgentControleur { get; set; }

        //[DataMember]
        //public System.Nullable<int> NbreElementsDuLot { get; set; }

        //[DataMember]
        //public System.Nullable<System.DateTime> DateFermeture { get; set; }

        //[DataMember]
        //public System.Nullable<int> Critere_TypeClient { get; set; }

        //[DataMember]
        //public System.Nullable<int> Critere_GroupeDeFacturation { get; set; }

        //[DataMember]
        //public string Critere_TypeTarif { get; set; }

        //[DataMember]
        //public System.Nullable<System.Guid> Critere_IdTournee { get; set; }

        //[DataMember]
        //public System.Nullable<int> Critere_TypeCompteur { get; set; }

        //[DataMember]
        //public System.Nullable<System.DateTime> DateAffectationLot { get; set; }

        [DataMember]
        public string Nom { get; set; }

        //[DataMember]
        //public string Prenoms { get; set; }

        //[DataMember]
        //public System.Nullable<System.Guid> ResultatControle_ID { get; set; }

        //[DataMember]
        //public System.Nullable<int> ResultatValue { get; set; }

        //[DataMember]
        //public string MatriculeAgentSaisie { get; set; }

        //[DataMember]
        //public System.Nullable<System.DateTime> DateSaisieResultatControle { get; set; }

        //[DataMember]
        //public System.Nullable<System.DateTime> DateControle { get; set; }

        //[DataMember]
        //public string Commentaires { get; set; }

        //[DataMember]
        //public System.Nullable<int> GroupeDeFacturation { get; set; }

        //[DataMember]
        //public System.Nullable<int> TypeClient { get; set; }

        //[DataMember]
        //public System.Nullable<System.Guid> Tournee_Id { get; set; }

        //[DataMember]
        //public System.Nullable<int> TypeCompteur_ID { get; set; }

        //[DataMember]
        //public int StatutContrat { get; set; }

        //[DataMember]
        //public System.Nullable<int> TypeContratID { get; set; }
    }
}
