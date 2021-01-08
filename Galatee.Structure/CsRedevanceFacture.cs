using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsRedevanceFacture : CsPrint
    {

       [DataMember] public string LOTRI { get; set; }
       [DataMember] public string JET { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRE { get; set; }
       [DataMember] public string FACTURE { get; set; }
       [DataMember] public Nullable<int> LIENRED { get; set; }
       [DataMember] public string REDEVANCE { get; set; }
       [DataMember] public string TRANCHE { get; set; }
       [DataMember] public string ORDRED { get; set; }
       [DataMember] public Nullable<int> QUANTITE { get; set; }
       [DataMember] public string UNITE { get; set; }
       [DataMember] public Nullable<decimal> BARPRIX { get; set; }
       [DataMember] public Nullable<decimal> TAXE { get; set; }
       [DataMember] public string CTAX { get; set; }
       [DataMember] public Nullable<System.DateTime> DAPP { get; set; }
       [DataMember] public string CRITERE { get; set; }
       [DataMember] public Nullable<int> VARIANTE { get; set; }
       [DataMember] public Nullable<decimal> TOTREDHT { get; set; }
       [DataMember] public Nullable<decimal> TOTREDTAX { get; set; }
       [DataMember] public Nullable<decimal> TOTREDTTC { get; set; }
       [DataMember] public string PARAM1 { get; set; }
       [DataMember] public string PARAM2 { get; set; }
       [DataMember] public string PARAM3 { get; set; }
       [DataMember] public string PARAM4 { get; set; }
       [DataMember] public string PARAM5 { get; set; }
       [DataMember] public string PARAM6 { get; set; }
       [DataMember] public Nullable<int> NBJOUR { get; set; }
       [DataMember] public Nullable<System.DateTime> DEBUTAPPLICATION { get; set; }
       [DataMember] public Nullable<System.DateTime> FINAPPLICATION { get; set; }
       [DataMember] public string NATURE { get; set; }
       [DataMember] public string LIENFAC { get; set; }
       [DataMember] public string TOPMAJ { get; set; }
       [DataMember] public string PERIODE { get; set; }
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public string FORMULE { get; set; }
       [DataMember] public Nullable<int> TOPANNUL { get; set; }
       [DataMember] public Nullable<int> BARBORNEDEBUT { get; set; }
       [DataMember] public Nullable<int> BARBORNEFIN { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public Nullable<int> FK_IDENTFAC { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDABON { get; set; }

        /*Autre */
       [DataMember] public Nullable<int> POINT { get; set; }
       [DataMember] public int EVENEMENT { get; set; }
       [DataMember] public string LIBELLEREDEVANCE { get; set; }
       [DataMember] public string LIBELLETRANCHE { get; set; }
       [DataMember] public Nullable<decimal> DEBIT { get; set; }
       [DataMember] public Nullable<decimal> CREDIT { get; set; }
        




    }
}
