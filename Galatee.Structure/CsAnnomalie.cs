using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsAnnomalie : CsPrint 
    {
        [DataMember]   public int PK_ID { get; set; }
        [DataMember]   public string CAUSE { get; set; }
        [DataMember]   public string SOLUTION { get; set; }
        [DataMember]   public string LOTRI { get; set; }
        [DataMember]   public string COMPTEUR { get; set; }
        [DataMember]   public string CENTRE { get; set; }
        [DataMember]   public string CLIENT { get; set; }
        [DataMember]   public string ORDRE { get; set; }
        [DataMember]   public int FK_IDLOT { get; set; }
        [DataMember]   public int FK_IDCLIENT { get; set; }
    }
}









