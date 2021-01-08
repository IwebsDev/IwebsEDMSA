using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFraisTimbre
    {
      [DataMember]  public Nullable<decimal> BORNEINF { get; set; }
      [DataMember]  public Nullable<decimal> BORNESUP { get; set; }
      [DataMember]  public Nullable<decimal> FRAIS { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
    }
}
