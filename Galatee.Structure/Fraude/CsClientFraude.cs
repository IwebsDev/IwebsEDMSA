using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract] 
    public class CsClientFraude : CsPrint
    {
        
         [DataMember] public int PK_ID { get; set; }
         [DataMember] public Nullable<int> FK_IDSITE { get; set; }
         [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }
         [DataMember] public string Centre { get; set; }
         [DataMember] public string Client { get; set; }
         [DataMember] public string Ordre { get; set; }
         [DataMember] public string IdentificationUnique { get; set; } 
         [DataMember] public string Nomabon { get; set; }
         [DataMember] public string Email { get; set; }
         [DataMember] public string Telephone { get; set; }
         [DataMember] public string Commune { get; set; }
         [DataMember] public string Quartier { get; set; }
         [DataMember] public string Rue { get; set; }
         [DataMember] public string Porte { get; set; }
         [DataMember] public string Tournee { get; set; }
         [DataMember] public string OrdreTournee { get; set; }
         [DataMember] public string Secteur { get; set; }
         [DataMember] public string ContratAbonnement { get; set; }
         [DataMember] public Nullable<System.DateTime> DateContratAbonnement { get; set; }
         [DataMember] public string ContratBranchement { get; set; }
         [DataMember] public Nullable<System.DateTime> DateContratBranchement { get; set; }
         [DataMember] public Nullable<decimal> PuissanceSouscrite { get; set; }
         [DataMember] public Nullable<decimal> PuissanceInstallee { get; set; }
         [DataMember]public Nullable<int> FK_IDCOMMUNE { get; set; }
         [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
         [DataMember] public Nullable<int> FK_SECTEUR { get; set; }
         [DataMember] public Nullable<int> FK_RUE { get; set; }
    
    }
}
