using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure.Rpnt
{
    [DataContract]
    public class CsElementsDeCampagneHTA
    {
        [DataMember]
        public System.Guid Campagne_ID { get; set; }

        [DataMember]
        public System.Guid Contrat_ID { get; set; }

        [DataMember]
        public System.DateTime DateSelection { get; set; }

        [DataMember]
        public string Raccordement_ID { get; set; }

        [DataMember]
        public string ReferenceClient { get; set; }

        [DataMember]
        public System.Nullable<System.Guid> Lot_ID { get; set; }

        [DataMember]
        public string Libelle_Lot { get; set; }

        [DataMember]
        public System.Nullable<int> StatutLot_ID { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateCreation { get; set; }

        [DataMember]
        public string MatriculeAgentCreation { get; set; }

        [DataMember]
        public string MatriculeAgentControleur { get; set; }

        [DataMember]
        public System.Nullable<int> NbreElementsDuLot { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateFermeture { get; set; }

        [DataMember]
        public string CodeUO { get; set; }

        [DataMember]
        public string Nom { get; set; }

        [DataMember]
        public string Prenoms { get; set; }

        [DataMember]
        public System.Nullable<System.Guid> ResultatControle_ID { get; set; }

        [DataMember]
        public System.Nullable<int> ResultatValue { get; set; }

        [DataMember]
        public string MatriculeAgentSaisie { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateControle { get; set; }

        [DataMember]
        public string Commentaires { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateSaisieResultatControle { get; set; }

        [DataMember]
        public string _Numero_Compteur { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> DateAffectationLot { get; set; }

        [DataMember]
        public int Methode_ID { get; set; }

        [DataMember]
        public int Debut_PerAA { get; set; }

        [DataMember]
        public int Debut_PerMM { get; set; }

        [DataMember]
        public int Fin_PerAA { get; set; }

        [DataMember]
        public int Fin_PerMM { get; set; }

        [DataMember]
        public double Difference { get; set; }

        [DataMember]
        public string _CodeExploitation { get; set; }

        [DataMember]
        public System.Nullable<System.DateTime> _DateDerniereSelectionEnCampagne { get; set; }
    }
}
