using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsSecteur : CsPrint
    {
      [DataMember]public string CODEQUARTIER { get; set; }
      [DataMember]public string CODE{ get; set; }
      [DataMember]public string LIBELLE { get; set; }
      [DataMember]public string TRANS { get; set; }
      [DataMember]public int PK_ID { get; set; }
      [DataMember]public int FK_IDQUARTIER { get; set; }
      [DataMember]public System.DateTime DATECREATION { get; set; }
      [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]public string USERCREATION { get; set; }
      [DataMember]public string USERMODIFICATION { get; set; }
      [DataMember]public string LIBELLEQUARTIER { get; set; }
      [DataMember]public string LIBELLECOMMUNE { get; set; }
      [DataMember]public string LIBELLECENTRE { get; set; }
    }
 }









