using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCoper :CsPrint
    {
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBCOURT { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public string COMPTGENE { get; set; }
      [DataMember]  public string DC { get; set; }
      [DataMember]  public string CTRAIT { get; set; }
      [DataMember]  public Nullable<System.DateTime> DMAJ { get; set; }
      [DataMember]  public string TRANS { get; set; }
      [DataMember]  public string COMPTEANNEXE1 { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public Nullable<bool> ISOD { get; set; }

      [DataMember]  public bool IsSelect { get; set; }


    }

}









