using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsBalanceAgee
    {
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRE { get; set; }
       [DataMember] public string TYPECLIENT { get; set; }
       [DataMember] public string CATEGORIE { get; set; }
       [DataMember] public string NOMABON { get; set; }
       [DataMember] public decimal  MONTANTAVANT { get; set; }
       [DataMember] public decimal  MONTANTM1 { get; set; }
       [DataMember] public decimal  MONTANTM2 { get; set; }
       [DataMember] public decimal  MONTANTM3 { get; set; }
       [DataMember] public decimal  MONTANTM4 { get; set; }
       [DataMember] public decimal  MONTANTM5 { get; set; }
       [DataMember] public decimal  MONTANTM6 { get; set; }
       [DataMember] public decimal  MONTANTM7 { get; set; }
    }
}
