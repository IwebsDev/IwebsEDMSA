using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsStatFacturation
    {

       [DataMember] public int NombreCalcule { get; set; }
       [DataMember] public decimal?  Montant { get; set; }
       [DataMember] public int NombreRejete { get; set; }
       [DataMember] public int VolumeCalcule { get; set; }
    }
}









