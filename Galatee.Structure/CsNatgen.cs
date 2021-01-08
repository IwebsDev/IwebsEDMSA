using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsNatgen
    {
         [DataMember] public string CENTRE { get; set; }
       [DataMember] public string AFFECT { get; set; }
       [DataMember] public string CLIPRO { get; set; }
       [DataMember] public string COPER { get; set; }
       [DataMember] public string NATURE { get; set; }
       [DataMember] public string TRANS { get; set; }
       [DataMember] public string CATCLI { get; set; }
       [DataMember] public string CODETARIF { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCOPER { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
    }

}









