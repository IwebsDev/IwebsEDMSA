using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
       [DataContract]
   public class CsFactureFraude
    {
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string Refem { get; set; }
        [DataMember] public string NumeroFacture { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public Nullable<System.DateTime> DateEnregistrement { get; set; }
        [DataMember] public Nullable<decimal> Montant { get; set; }
        [DataMember] public Nullable<System.DateTime> Exigibilite { get; set; }
        [DataMember] public string Origine { get; set; }
        [DataMember] public string MoisComptable { get; set; }
        [DataMember] public Nullable<decimal> FraisRetard { get; set; }
        [DataMember] public Nullable<decimal> MontantTVA { get; set; }
        [DataMember] public byte OrdreTraitement { get; set; }
        [DataMember] public Nullable<System.DateTime> DateValidation { get; set; }
        [DataMember] public Nullable<System.DateTime> DateAnnulation { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDCLIENTFRAUDE { get; set; }
        [DataMember] public int FK_IDFRAUDE { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDCOPER { get; set; }
    } 
}
