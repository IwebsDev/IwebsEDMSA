using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsRubriqueDevis : CsPrint 
    {
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public string CODE { get; set; }
      [DataMember]  public string LIBELLE { get; set; }
      [DataMember]  public int? FK_IDPRODUIT { get; set; }
      [DataMember]  public string CODEPRODUIT { get; set; }
      [DataMember]  public bool  IsSelect { get; set; }
    }
}









