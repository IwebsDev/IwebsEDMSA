using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMonnaie : CsPrint
    {
        [DataMember]  public int PK_ID { get; set; }
        [DataMember]  public int FK_IDCENTRE { get; set; }
        [DataMember]  public string CENTRE { get; set; }
        [DataMember]  public string SUPPORT { get; set; }
        [DataMember]  public decimal? VALEUR { get; set; }
        [DataMember]  public string LIBELLE { get; set; }
        [DataMember]  public DateTime? DATECREATION { get; set; }
        [DataMember]  public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]  public string USERCREATION { get; set; }
        [DataMember]  public string USERMODIFICATION { get; set; }
        [DataMember]  public string LIBELLESUPPORT { get; set; }
        [DataMember]  public string LIBELLECENTRE { get; set; }
        [DataMember]  public string DISPLAYVALUE { get; set; }
    }
}
