using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTypePanne : CsPrint
    {
        
      [DataMember]  public int ID { get; set; }
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public int ID_TYPE_RECLAMATION { get; set; }
      [DataMember]  public bool EST_SUPPRIME { get; set; }
      [DataMember]  public string CREER_PAR { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATE_CREATION { get; set; }
      [DataMember]  public string DERNIER_UTILISATEUR { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATE_MODIFICATION { get; set; }
    }
}
