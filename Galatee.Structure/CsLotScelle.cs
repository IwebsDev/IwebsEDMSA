using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsLotScelle
    {
        [DataMember] public string Id_LotMagasinGeneral { get; set; }
        [DataMember] public DateTime DateReception { get; set; }
        [DataMember] public int Matricule_AgentReception { get; set; }
        [DataMember] public string Numero_depart { get; set; }
        [DataMember] public string Numero_fin { get; set; }
        [DataMember] public int Nbre_Scelles { get; set; }
        [DataMember] public int StatutLot_ID { get; set; }
        [DataMember] public int Origine_ID { get; set; }
        [DataMember] public int CodeCentre { get; set; }
        [DataMember] public int Couleur_ID { get; set; }
        [DataMember] public int Fournisseur_ID { get; set; }
        [DataMember] public DateTime Date_DerniereModif { get; set; }
        [DataMember] public int Matricule_AgentDerniereModif { get; set; }
        [DataMember] public Guid Id_Affectation { get; set; }
        [DataMember] public Nullable<int> Activite_ID { get; set; }


        [DataMember] public string Libelle_Fournisseur { get; set; }
        [DataMember] public string Libelle_Couleur { get; set; }
        [DataMember] public string Libelle_Origine { get; set; }

        [DataMember] public string Etat { get; set; }
        [DataMember] public bool  IsSelect { get; set; }

    }
}
