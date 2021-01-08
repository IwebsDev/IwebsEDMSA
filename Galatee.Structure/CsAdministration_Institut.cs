using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
   public class CsAdministration_Institut
    {
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public string NOMMANDATAIRE { get; set; }
      [DataMember] public string PRENOMMANDATAIRE { get; set; }
      [DataMember] public string RANGMANDATAIRE { get; set; }
      [DataMember] public string NOMSIGNATAIRE { get; set; }
      [DataMember] public string PRENOMSIGNATAIRE { get; set; }
      [DataMember] public string RANGSIGNATAIRE { get; set; }
      [DataMember] public Nullable<int> FK_IDCLIENT { get; set; }
      [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
      [DataMember] public string NOMABON { get; set; }
    }
}
