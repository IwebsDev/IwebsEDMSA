using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTournee :CsPrint 
    {

      [DataMember]  public string CENTRE { get; set; }
      [DataMember]  public string RELEVEUR { get; set; }
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public string LOCALISATION { get; set; }
      [DataMember]  public string PRIORITE { get; set; }
      [DataMember]  public string MATRICULEPIA { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public Nullable<bool> SUPPRIMER { get; set; }
      [DataMember]  public int FK_IDCENTRE { get; set; }
      [DataMember]  public Nullable<int> FK_IDRELEVEUR { get; set; }
      [DataMember]  public Nullable<int> FK_IDADMUTILISATEUR { get; set; }
      [DataMember]  public Nullable<int> FK_IDLOCALISATION { get; set; }

      [DataMember]  public string NOMRELEVEUR { get; set; }
      [DataMember]  public string NOMAGENTPIA { get; set; }
      [DataMember]  public Nullable<int> FK_IDTOURNEE{ get; set; }
      [DataMember]  public bool IsSelect { get; set; }
      [DataMember]  public string IDCOUPURE { get; set; }

      [DataMember]  public string TOURNEDEBUT { get; set; }
      [DataMember]  public string TOURNEFIN { get; set; }

        
      [DataMember]  public DateTime ? DATEDEBUT { get; set; }
      [DataMember]  public DateTime ? DATEFIN { get; set; }
    }
}
