using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
     [DataContract]
   public class CsAppareilUtiliserFrd
    {
            [DataMember] public int PK_ID { get; set; }
            [DataMember] public Nullable<int> NOMBRE { get; set; }
            [DataMember] public Nullable<int> PUISSANCEUNITAIRE { get; set; }
            [DataMember] public int FK_IDAUDITION { get; set; }
            [DataMember] public int FK_IDAPPAREIL { get; set; }
            [DataMember] public int FK_IDPRODUIT { get; set; }
            [DataMember] public int Consommation { get; set; }
            [DataMember] public int Journaliere { get; set; }
            [DataMember] public int Mensuelle { get; set; }
            [DataMember] public int TEMPSUTILISATION { get; set; }
            [DataMember] public int estimee { get; set; }
            [DataMember]  public string DESIGNATION { get; set; }
            [DataMember]
            public int CODEAPPAREIL { get; set; }
       

    }
}
