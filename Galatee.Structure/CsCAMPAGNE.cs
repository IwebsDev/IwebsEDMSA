using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCAMPAGNE
    {
      [DataMember]  public string IDCOUPURE { get; set; }
      [DataMember]  public string CENTRE { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT { get; set; }
      [DataMember]  public string MATRICULEPIA { get; set; }
      [DataMember]  public string PERIODE_RELANCABLE { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATE_EXIGIBILITE { get; set; }
      [DataMember]  public string PREMIERE_TOURNEE { get; set; }
      [DataMember]  public string DERNIERE_TOURNEE { get; set; }
      [DataMember]  public string DEBUT_ORDTOUR { get; set; }
      [DataMember]  public string FIN_ORDTOUR { get; set; }
      [DataMember]  public Nullable<decimal> MONTANT_RELANCABLE { get; set; }
      [DataMember]  public string DEBUT_CATEGORIE { get; set; }
      [DataMember]  public string FIN_CATEGORIE { get; set; }
      [DataMember]  public string NOMBRE_CLIENT { get; set; }
      [DataMember]  public string NOMBRE_FACTURE { get; set; }
      [DataMember]  public string DEBUT_AG { get; set; }
      [DataMember]  public string FIN_AG { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int FK_IDCLIENT { get; set; }
      [DataMember]  public int FK_IDCENTRE { get; set; }
      [DataMember]  public int FK_IDMATRICULE { get; set; }


      [DataMember]  public int? INDEX { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATECOUPURE { get; set; }
      [DataMember]  public int FK_IDSITE { get; set; }
      [DataMember]  public string CODESITE { get; set; }
      [DataMember]  public string LIBELLECENTRE { get; set; }
      [DataMember]  public string LIBELLESITE { get; set; }
      [DataMember]  public string AGENTPIA { get; set; }
      [DataMember]  public bool  IsSelect { get; set; }

      [DataMember]  public List<CsDetailCampagne > DetailleCampagne { get; set; }
      [DataMember]  public List<CsBrt> BRT { get; set; }

    }
}
