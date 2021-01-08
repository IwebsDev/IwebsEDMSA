using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]

    public class CsCoutCoper : CsPrint
    {
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public Nullable<decimal> PUISSANCE { get; set; }
       [DataMember] public string TYPETARIF { get; set; }
       [DataMember] public string COPER { get; set; }
       [DataMember] public decimal MONTANT { get; set; }
       [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
       [DataMember] public string DIAMETRE { get; set; }
       [DataMember] public Nullable<bool> SUBVENTIONNEE { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDCOPER { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public Nullable<int> FK_IDTYPETARIF { get; set; }
       [DataMember] public Nullable<int> FK_IDDIAMETRECOMPTEUR { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }

       [DataMember] public string LIBELLEDIAMETRE { get; set; }
       [DataMember] public string LIBELLECOPER { get; set; }
       [DataMember] public string LIBELLETYPETARIF { get; set; }
       [DataMember] public string LIBELLEPRODUI { get; set; }
       [DataMember] public string LIBELLECENTRE { get; set; }
    }
 }









