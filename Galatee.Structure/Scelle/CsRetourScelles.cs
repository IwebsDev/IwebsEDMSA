using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Inova.Tools.Utilities;

namespace Galatee.Structure
{

    [DataContract]
 public class CsRetourScelles
    
    {
        [DataMember] public Guid Id_Retour { get; set; }
        [DataMember] public DateTime Date_Retour { get; set; }
        [DataMember] public string Receveur_Mat { get; set; }
        [DataMember] public string Donneur_Mat { get; set; }
        [DataMember]  public Nullable<int> Nbre_Scelles { get; set; }
        [DataMember] public Nullable<int> CodeCentre { get; set; }
        [DataMember] public Nullable<Guid> Id_DetailRetour { get; set; }
        [DataMember] public int Motif_Retour { get; set; }
        [DataMember] public int Statut_Scelle { get; set; }
        [DataMember] public Nullable< Guid> Id_Scelle { get; set; }
        [DataMember] public int Status_ID { get; set; }
        [DataMember] public string  Numero_Scelle { get; set; }
        [DataMember] public string  Couleur_libelle { get; set; }
        
       
       
    }
}
