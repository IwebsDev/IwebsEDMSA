using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsTypeTimbre : CsPrint 
    {
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public string CODE { get; set; }
      [DataMember] public string LIBELLE { get; set; }
      [DataMember] public Nullable<decimal> MONTANT { get; set; }
    }
}









