using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
         [DataContract]
   public class CsTBClient
    {
             [DataMember]
             public string Branchementt { get; set; }
             [DataMember]
             public string Nom { get; set; }
             [DataMember]
             public string Prenom { get; set; }
             [DataMember]
             public DateTime Periode { get; set; }
             [DataMember]
             public decimal ConsoAttendu { get; set; }
             [DataMember]
             public decimal ConsoFacture { get; set; }
             [DataMember]
             public decimal DifferenceConso { get; set; }
    }
}
