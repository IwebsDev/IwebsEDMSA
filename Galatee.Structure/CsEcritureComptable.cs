using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    public class CsEcritureComptable
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string ORIGINE { get; set; }
        [DataMember] public string PK_COPER { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string LIBCOURT { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string COMPTGENE { get; set; } 
        [DataMember] public string DC { get; set; }
        [DataMember] public string CTRAIT { get; set; }
        [DataMember] public string CAISSE { get; set; }
        [DataMember] public System.DateTime ? DMAJ { get; set; }
        [DataMember] public string TRANS { get; set; }
        [DataMember] public string COMPTEANNEXE1 { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime? DATECREATION { get; set; }
        [DataMember] public System.DateTime? DATECAISSE { get; set; }
        [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public decimal MONTANT { get; set; }
    }
}
