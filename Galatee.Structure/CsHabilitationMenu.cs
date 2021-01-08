using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsHabilitationMenu : CsPrint
    {
        [DataMember]  public int IDFONCTION { get; set; }
        [DataMember]  public string  LIBELLEFONCTION { get; set; }
        [DataMember]  public int IDPROFIL { get; set; }
        [DataMember]  public string LIBELLEPROFIL { get; set; }
        [DataMember]  public  Nullable<int>  IDMENU { get; set; }
        [DataMember]  public string LIBELLEMENU { get; set; }

        [DataMember]  public string LIBELLEFROMULAIRE { get; set; }
        [DataMember]  public string NOMUTILISATEUR { get; set; }
        [DataMember]  public string MATRICULE { get; set; }
        [DataMember]  public string LIBELLECENTRE { get; set; }
        [DataMember]  public DateTime  DATEDEBUTVALIDITE { get; set; }
        [DataMember]  public DateTime?  DATEFINVALIDITE { get; set; }

        [DataMember]  public string  DDEBUTVALIDITE { get; set; }
        [DataMember]  public string  DFINVALIDITE { get; set; }
            

    }
}
