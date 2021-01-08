using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjSUIVIDEVIS
    {
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public string NUMDEM { get; set; }
      [DataMember]  public int FK_IDETAPE { get; set; }
      [DataMember]  public Nullable<int> DUREE { get; set; }
      [DataMember]  public string MATRICULEAGENT { get; set; }
      [DataMember]  public string COMMENTAIRE { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int FK_IDDEMANDE { get; set; }
      [DataMember]  public string LIBELLEETAPE { get; set; }
    }
}
