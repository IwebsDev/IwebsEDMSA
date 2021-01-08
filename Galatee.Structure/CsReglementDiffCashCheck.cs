using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Galatee.Structure
{
    [DataContract]
    public class CsReglementDiffCashCheck
    {
        [DataMember]
         public string modeRegl { get; set; }
        [DataMember]
         public string Libelle { get; set; }
        
       
    }

}









