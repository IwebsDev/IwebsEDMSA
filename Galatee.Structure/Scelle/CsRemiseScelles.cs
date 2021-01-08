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
 public class CsRemiseScelles
    {
        [DataMember]  public Guid Id_Remise { get; set; }
        [DataMember]  public DateTime Date_Remise { get; set; }
        [DataMember]  public int Matricule_User { get; set; }
        [DataMember]  public int Matricule_Receiver { get; set; }
        [DataMember]  public int Motif_ID { get; set; }
        [DataMember]  public Nullable< int> Nbre_Scelles { get; set; }
        [DataMember]  public Nullable<int> CodeCentre { get; set; }
        [DataMember]  public Nullable<int> TypeRemise { get; set; }
        [DataMember]  public string Libelle_Motif { get; set; }
        [DataMember]  public Nullable< Guid >Id_DetailRemise { get; set; }
        [DataMember]  public string Lot_Id { get; set; }
        [DataMember] public Nullable< Guid> Id_Scelle { get; set; }
       
    }
}
