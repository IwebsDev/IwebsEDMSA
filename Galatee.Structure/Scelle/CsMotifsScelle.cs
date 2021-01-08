using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
     [DataContract]
     public class CsMotifsScelle
    {
         [DataMember]
         public int Motif_ID { get; set; }
         [DataMember]
         public string Motif_libelle { get; set; }
         [DataMember]
         public int TypeInterventionDuMotif { get; set; }
    }
}
