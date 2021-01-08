using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsModereglement
    {
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public Nullable<decimal> ECARTPOS { get; set; }
        [DataMember] public Nullable<decimal> ECARTNEG { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public string TRANS { get; set; }
        [DataMember] public string COMPTE { get; set; }
        [DataMember] public string COMPTEANNEXE1 { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDCOPER { get; set; }

        [DataMember] public Nullable<decimal> MONTANT { get; set; }
        [DataMember] public Nullable<decimal> PERCU { get; set; }
        [DataMember] public Nullable<decimal> RENDU { get; set; }



    }

}









