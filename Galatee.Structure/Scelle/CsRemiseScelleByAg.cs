using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
 public   class CsRemiseScelleByAg :CsPrint 
    {

         [DataMember] public Guid Id_Scelle { get; set; }
        [DataMember] public string Numero_Scelle { get; set; }
        [DataMember] public int Status_ID { get; set; }
        [DataMember] public int Matricule_Receiver { get; set; }
        [DataMember] public int Nbre_Scelles { get; set; }
        [DataMember] public int CodeCentre { get; set; }


        [DataMember] public string Couleur_libelle { get; set; }
        [DataMember] public string libelleStatus { get; set; }
        [DataMember] public DateTime  Date_Remise { get; set; }
        [DataMember] public string lot_ID { get; set; }
        [DataMember] public int? Couleur_Scelle { get; set; }

         
         
       
    }
}
