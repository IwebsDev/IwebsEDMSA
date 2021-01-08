using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
    public class CsMotifChequeImpaye
    {
       [DataMember]  public int PK_ID { get; set; }
       [DataMember]  public string CODE { get; set; }
       [DataMember]  public string LIBELLE { get; set; }
       [DataMember]  public byte POIDS { get; set; }
    }
}
