//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galatee.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class C_A_MIG_FROM_TRANSCAISB_SCA_BT
    {
        public string CENTRE { get; set; }
        public string CLIENT { get; set; }
        public string ORDRE { get; set; }
        public string CAISSE { get; set; }
        public string ACQUIT { get; set; }
        public string MATRICULE { get; set; }
        public string NDOC { get; set; }
        public string REFEM { get; set; }
        public Nullable<decimal> MONTANT { get; set; }
        public string DC { get; set; }
        public string COPER { get; set; }
        public Nullable<decimal> PERCU { get; set; }
        public Nullable<decimal> RENDU { get; set; }
        public string MODEREG { get; set; }
        public string PLACE { get; set; }
        public Nullable<System.DateTime> DTRANS { get; set; }
        public Nullable<System.DateTime> DEXIG { get; set; }
        public string BANQUE { get; set; }
        public string GUICHET { get; set; }
        public string ORIGINE { get; set; }
        public Nullable<decimal> ECART { get; set; }
        public string TOPANNUL { get; set; }
        public string MOTIFANNULATION { get; set; }
        public string CRET { get; set; }
        public string MOISCOMPT { get; set; }
        public string TOP1 { get; set; }
        public string TOURNEE { get; set; }
        public string NUMDEM { get; set; }
        public Nullable<System.DateTime> DATEVALEUR { get; set; }
        public Nullable<System.DateTime> DATEFLAG { get; set; }
        public string NUMCHEQ { get; set; }
        public string SAISIPAR { get; set; }
        public Nullable<System.DateTime> DATEENCAISSEMENT { get; set; }
        public string CANCELLATION { get; set; }
        public string USERCREATION { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDLCLIENT { get; set; }
        public Nullable<int> FK_IDHABILITATIONCAISSE { get; set; }
        public Nullable<int> FK_IDMODEREG { get; set; }
        public int FK_IDLIBELLETOP { get; set; }
        public Nullable<int> FK_IDCAISSIERE { get; set; }
        public Nullable<int> FK_IDAGENTSAISIE { get; set; }
        public int FK_IDCOPER { get; set; }
        public Nullable<int> FK_IDPOSTECLIENT { get; set; }
        public Nullable<int> FK_IDNAF { get; set; }
        public string POSTE { get; set; }
        public Nullable<System.DateTime> DATETRANS { get; set; }
        public string BANQUECAISSE { get; set; }
        public string AGENCEBANQUE { get; set; }
        public Nullable<bool> ESTSYNCHRONISE { get; set; }
        public Nullable<int> FK_IDMORATOIRE { get; set; }
    }
}
