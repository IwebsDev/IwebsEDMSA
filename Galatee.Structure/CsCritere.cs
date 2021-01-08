using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsCritere
    {
       [DataMember] public int FK_IDCENTRE       { get; set; }
       [DataMember] public DateTime DATEDEBUT        { get; set; }
       [DataMember] public DateTime DATEFIN        { get; set; }
       [DataMember] public string  NOMBREOCCURANCE       { get; set; }
    }
}









