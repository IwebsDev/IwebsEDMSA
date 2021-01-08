using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDscelle
    {
        [DataMember]  public string NUMDEM { get; set; }
        [DataMember]  public int FK_IDCENTRE { get; set; }
        [DataMember]  public int FK_IDACTIVITE { get; set; }
        [DataMember]  public int FK_IDCOULEURSCELLE { get; set; }
        [DataMember]  public int FK_IDDEMANDE { get; set; }
        [DataMember]  public int FK_IDAGENT { get; set; }
        [DataMember]  public int NOMBRE_DEM { get; set; }
        [DataMember]  public int NOMBRE_REC { get; set; }
        [DataMember]  public int PK_ID { get; set; }
        [DataMember]  public int FK_IDCENTREFOURNISSEUR { get; set; }
        [DataMember]  public string LIBELLECOULEUR { get; set; }
        [DataMember]  public string LIBELLEACTIVITE { get; set; }
        [DataMember]  public string LIBELLEAGENT { get; set; }
        [DataMember]  public string LIBELLESITEAGENT { get; set; }
        [DataMember]  public string LIBELLECENTREDESTINATAIRE { get; set; }
        [DataMember]  public string LIBELLECENTREFOURNISSEUR { get; set; }
        [DataMember]  public string MATRICULE { get; set; }

    }
}
