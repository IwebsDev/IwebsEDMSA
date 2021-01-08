using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Inova.Tools.Utilities;
namespace Galatee.Structure 

{
    [DataContract]
   public class CsTbLot
    {
        [DataMember]
        public string lot_ID { get; set; }
        [DataMember]
        public int typeReception { get; set; }
        [DataMember]
        public DateTime DateReception { get; set; }
        [DataMember]
        public string Numero_depart { get; set; }
        [DataMember]
        public string Numero_fin { get; set; }
        [DataMember]
        public Nullable<int> Nombre_scelles_reçu { get; set; }
        [DataMember]
        public int   Nombre_scelles_lot { get; set; }
        [DataMember]
        public Nullable<int> provenance_Scelle_ID { get; set; }
        [DataMember]
        public Nullable<int> agence_centre_Appartenance { get; set; }
        [DataMember]
        public  Nullable<int> agence_centre_Origine { get; set; }
        [DataMember]
        public  Nullable<int> Origine_ID { get; set; }
        [DataMember]
        public Nullable<int> Status_lot_ID { get; set; }
        [DataMember]
        public Nullable<int> lot_Couleur_ID { get; set; } 
        [DataMember]
        public Nullable<DateTime> Date_Derniere_Modif { get; set; }
        [DataMember]
        public int TypeDeLot { get; set; }
        [DataMember]
        public  Nullable< int> Matricule_Creation { get; set; }
        [DataMember]
        public Nullable <int> Matricule_AgentModification { get; set; }
        [DataMember]
        public string Libelle_Couleur { get; set; }
        [DataMember]
        public string Libelle_Satut { get; set; }
     
        
    }
}
