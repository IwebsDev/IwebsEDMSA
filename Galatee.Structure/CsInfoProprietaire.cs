using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsInfoProprietaire
    {
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public string NOM { get; set; }
      [DataMember]  public string PRENOM { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATENAISSANCE { get; set; }
      [DataMember]  public string NUMEROPIECEIDENTITE { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
      [DataMember]  public Nullable<int> FK_IDDEMANDE { get; set; }
      [DataMember]  public Nullable<int> FK_IDPIECEIDENTITE { get; set; }
      [DataMember]  public Nullable<int> FK_IDNATIONNALITE { get; set; }
      [DataMember]  public string FAX { get; set; }
      [DataMember]  public string BOITEPOSTALE { get; set; }
      [DataMember]  public string EMAIL { get; set; }
      [DataMember]  public string TELEPHONEMOBILE { get; set; }
      [DataMember]  public string TELEPHONEFIXE { get; set; }

      [DataMember]  public Nullable<int> FK_IDCLIENT { get; set; }
      [DataMember]  public string LIBELLEPIECE { get; set; }
      [DataMember]  public string LIBELLENATIONNALITE { get; set; }
    }
}
