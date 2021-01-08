using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeLot
    {
        [DataMember] public string CODE { get; set; }
        [DataMember] public string CODETRAITEMENT { get; set; }
        [DataMember] public string CODECONTROLE { get; set; }
        [DataMember] public string LIBELLECODECONTROLE { get; set; }
        [DataMember] public string REFERENCE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string DC { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public int FK_IDCOPER { get; set; }
    }
}









