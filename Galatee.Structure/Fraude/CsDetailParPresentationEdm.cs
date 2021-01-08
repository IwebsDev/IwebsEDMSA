using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
        [DataContract] 
  public class CsDetailParPresentationEdm
    {
            [DataMember]
            public int PK_ID { get; set; }
            [DataMember]
            public int NombrePrestation { get; set; }
            [DataMember]
            public decimal PrixUnitaire { get; set; }
            [DataMember]
            public int FK_IDCONSOMMATION { get; set; }
            [DataMember]
            public int FK_IDPRESTATIONEDM { get; set; }
            [DataMember]
            public int FK_IDPRODUIT { get; set; }
    
    }
}
