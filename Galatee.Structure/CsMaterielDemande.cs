using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsMaterielDemande : CsPrint
    {
      [DataMember] public string CODE { get; set; }
      [DataMember] public string LIBELLE { get; set; }
      [DataMember] public Nullable<decimal> COUTUNITAIRE_FOURNITURE { get; set; }
      [DataMember] public Nullable<decimal> COUTUNITAIRE_POSE { get; set; }
      [DataMember] public System.DateTime DATECREATION { get; set; }
      [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember] public string USERCREATION { get; set; }
      [DataMember] public string USERMODIFICATION { get; set; }
      [DataMember] public int PK_ID { get; set; }
      [DataMember] public Nullable<bool> ISDISTANCE { get; set; }
      [DataMember] public Nullable<bool> ISCOMPTEUR { get; set; }
      [DataMember] public Nullable<bool> ISGENERE { get; set; }
      [DataMember] public Nullable<decimal> COUTUNITAIRE { get; set; } //Stephen : 13-02-2019

    }
 }









