using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFraixParticipation
    {
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public string REF_CLIENT { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT { get; set; }
      [DataMember]  public Nullable<bool> ESTEXONERE { get; set; }
      [DataMember]  public byte[] PREUVE { get; set; }
      [DataMember]  public Nullable<int> FK_IDDEMANDE { get; set; }
      [DataMember]  public int FK_IDCLIENT { get; set; }
      [DataMember]  public string CENTRE { get; set; }
      [DataMember]  public string ORDRE { get; set; }
      [DataMember]  public string NOM { get; set; }
    }
}
