using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;
namespace Galatee.Structure
{
     [DataContract]
  public  class CsRegularisation
    {
          [DataMember] 
        public int PK_ID { get; set; }
          [DataMember]
          public string Libelle { get; set; }
          [DataMember]
          public decimal PrixUnitaire { get; set; }
          [DataMember]
          public int FK_IDPHASECOMPTEUR { get; set; }
          [DataMember]
          public Nullable<bool> EstModifiable { get; set; }
          [DataMember]
          public int MontantHT { get; set; }
          [DataMember]
          public int MontantTva { get; set; }
          [DataMember]
          public int MontantTTC { get; set; }
          [DataMember]
          public int Quantite { get; set; }
    
    }
}
