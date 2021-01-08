using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{ 
    [DataContract]
   public class CsLotMagasinGeneral
    {

         [DataMember] public string Id_LotMagasinGeneral { get; set; }
         [DataMember] public DateTime DateReception { get; set; }
         [DataMember] public int Matricule_AgentReception { get; set; }
         [DataMember] public  string Numero_depart { get; set; }
         [DataMember] public  string Numero_fin { get; set; }
         [DataMember] public Nullable<int> Nbre_Scelles { get; set; }
	     [DataMember] public Nullable <int> Origine_ID { get; set; }
	     [DataMember] public Nullable <int> CodeCentre { get; set; }
	     [DataMember] public Nullable <int> Couleur_ID { get; set; }
	     [DataMember] public Nullable <int> Fournisseur_ID { get; set; }
	     [DataMember] public Nullable <DateTime> Date_DerniereModif { get; set; }
	     [DataMember] public Nullable <int> Matricule_AgentDerniereModif { get; set; }
	     [DataMember] public Nullable <Guid> Id_Affectation { get; set; }
	     [DataMember] public Nullable <int> Activite_ID { get; set; }
	     [DataMember] public Nullable <int> StatutLot_ID { get; set; }

         //Autre
         [DataMember] public string Libelle_statue { get; set; }
         [DataMember] public string Libelle_Fournisseur { get; set; }
         [DataMember] public string Libelle_Origine { get; set; }
         [DataMember] public string Couleur_libelle { get; set; }
      
	


    }
    
}
