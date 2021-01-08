using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCentre : CsPrint
    {

    [DataMember] public string CODE { get; set; }
    [DataMember] public string LIBELLE { get; set; }
    [DataMember] public string TYPECENTRE { get; set; }
    [DataMember] public string CODESITE { get; set; }
    [DataMember] public string ADRESSE { get; set; }
    [DataMember] public string TELRENSEIGNEMENT { get; set; }
    [DataMember] public string TELDEPANNAGE { get; set; }
    [DataMember] public Nullable<bool> NUMEROAUTOCLIENT { get; set; }
    [DataMember] public Nullable<bool> GESTIONAUTOAVANCECONSO { get; set; }
    [DataMember] public Nullable<bool> GESTIONAUTOFRAIS { get; set; }
    [DataMember] public Nullable<bool> NUMEROFACTUREPARCLIENT { get; set; }
    [DataMember] public System.DateTime DATECREATION { get; set; }
    [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
    [DataMember] public string USERCREATION { get; set; }
    [DataMember] public string USERMODIFICATION { get; set; }
    [DataMember] public Nullable<int> NUMERODEMANDE { get; set; }
    [DataMember] public int PK_ID { get; set; }
    [DataMember] public int FK_IDCODESITE { get; set; }
    [DataMember] public Nullable<int> FK_IDTYPECENTRE { get; set; }
    [DataMember] public int FK_IDNIVEAUTARIF { get; set; }
    [DataMember] public Nullable<int> NUMEROFACTURE { get; set; }
    [DataMember] public Nullable<System.DateTime> DATEFIN { get; set; }
    [DataMember] public string COMPTEECLAIRAGEPUBLIC { get; set; }
    [DataMember] public string CODESOUSCOMPTE { get; set; }

        


    [DataMember]  public bool IsSelect { get; set; }
    [DataMember]  public string LIBELLESITE { get; set; }
    [DataMember]  public string LIBELLETYPECENTRE { get; set; }
    [DataMember]  public string  LIBELLENIVEAUTARIF { get; set; }
    [DataMember]  public string  CODENIVEAUTARIF { get; set; }
    [DataMember]  public string  NUMDEM { get; set; }
    [DataMember]  public List<CsProduit>  LESPRODUITSDUSITE { get; set; }

        

        
  
    }
}
