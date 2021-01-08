using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTdem
    {
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public string DEMOPTION1 { get; set; }
      [DataMember]  public string DEMOPTION2 { get; set; }
      [DataMember]  public string DEMOPTION3 { get; set; }
      [DataMember]  public string DEMOPTION4 { get; set; }
      [DataMember]  public string DEMOPTION5 { get; set; }
      [DataMember]  public string DEMOPTION6 { get; set; }
      [DataMember]  public string DEMOPTION7 { get; set; }
      [DataMember]  public string DEMOPTION8 { get; set; }
      [DataMember]  public string DEMOPTION9 { get; set; }
      [DataMember]  public string DEMOPTION10 { get; set; }
      [DataMember]  public string DEMOPTION11 { get; set; }
      [DataMember]  public string DEMOPTION12 { get; set; }
      [DataMember]  public string DEMOPTION13 { get; set; }
      [DataMember]  public string DEMOPTION14 { get; set; }
      [DataMember]  public string DEMOPTION15 { get; set; }
      [DataMember]  public string DEMOPTION16 { get; set; }
      [DataMember]  public string DEMOPTION17 { get; set; }
      [DataMember]  public string DEMOPTION18 { get; set; }
      [DataMember]  public string DEMOPTION19 { get; set; }
      [DataMember]  public string DEMOPTION20 { get; set; }
      [DataMember]  public string EVT1 { get; set; }
      [DataMember]  public string EVT2 { get; set; }
      [DataMember]  public string EVT3 { get; set; }
      [DataMember]  public string EVT4 { get; set; }
      [DataMember]  public string EVT5 { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public int FK_IDPRODUIT { get; set; }
      [DataMember]  public bool  ISSELECT { get; set; }
    }
}
