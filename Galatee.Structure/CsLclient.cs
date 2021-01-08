using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsLclient : CsPrint
    {
       [DataMember]public int PK_ID { get; set; }
       [DataMember]public string CENTRE { get; set; }
       [DataMember]public string CLIENT { get; set; }
       [DataMember]public string ORDRE { get; set; }
       [DataMember]public string REFEM { get; set; }
       [DataMember]public string NDOC { get; set; }
       [DataMember]public string NATURE { get; set; }
       [DataMember]public string COPER { get; set; }
       [DataMember]public System.DateTime DENR { get; set; }
       [DataMember]public Nullable<int> EXIG { get; set; }
       [DataMember]public Nullable<decimal> MONTANT { get; set; }
       [DataMember]public string CAPUR { get; set; }
       [DataMember]public string CRET { get; set; }
       [DataMember]public string MODEREG { get; set; }
       [DataMember]public string DC { get; set; }
       [DataMember]public string ORIGINE { get; set; }
       [DataMember]public string CAISSE { get; set; }
       [DataMember]public Nullable<decimal> ECART { get; set; }
       [DataMember]public string MOISCOMPT { get; set; }
       [DataMember]public string TOP1 { get; set; }
       [DataMember]public Nullable<System.DateTime> EXIGIBILITE { get; set; }
       [DataMember]public Nullable<decimal> FRAISDERETARD { get; set; }
       [DataMember]public Nullable<int> REFERENCEPUPITRE { get; set; }
       [DataMember]public Nullable<int> IDLOT { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEVALEUR { get; set; }
       [DataMember]public string REFERENCE { get; set; }
       [DataMember]public string REFEMNDOC { get; set; }
       [DataMember]public string ACQUIT { get; set; }
       [DataMember]public string MATRICULE { get; set; }
       [DataMember]public Nullable<decimal> TAXESADEDUIRE { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEFLAG { get; set; }
       [DataMember]public Nullable<decimal> MONTANTTVA { get; set; }
       [DataMember]public string IDCOUPURE { get; set; }
       [DataMember]public string AGENT_COUPURE { get; set; }
       [DataMember]public Nullable<System.DateTime> RDV_COUPURE { get; set; }
       [DataMember]public string NUMCHEQ { get; set; }
       [DataMember]public string OBSERVATION_COUPURE { get; set; }
       [DataMember]public string USERCREATION { get; set; }
       [DataMember]public Nullable<System.DateTime>  DATECREATION { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember]public string USERMODIFICATION { get; set; }
       [DataMember]public string BANQUE { get; set; }
       [DataMember]public string GUICHET { get; set; }
       [DataMember]public int FK_IDCENTRE { get; set; }
       [DataMember]public int FK_IDNATURE { get; set; }
       [DataMember]public int FK_IDADMUTILISATEUR { get; set; }
       [DataMember]public int FK_IDCOPER { get; set; }
       [DataMember]public int FK_IDLIBELLETOP { get; set; }
       [DataMember]public int FK_IDCLIENT { get; set; }
       [DataMember]public int FK_IDMOTIFCHEQUEINPAYE { get; set; }
       [DataMember] public Nullable<int>  FK_IDNAF { get; set; }
       [DataMember]public Nullable<System.DateTime> DATEENCAISSEMENT { get; set; }
       [DataMember]public Nullable<int> FK_IDMORATOIRE { get; set; }
       [DataMember]public Nullable<decimal> MONTANTPAYE { get; set; }
       [DataMember]public Nullable<decimal> FRAISTIMBRE { get; set; }
       [DataMember]public string CODE_WKF { get; set; }
       [DataMember]public int FK_IDREGROUPEMENT { get; set; }
       [DataMember] public Nullable<bool>  ISNONENCAISSABLE { get; set; }
       [DataMember] public bool  ISDECAISSEMENT { get; set; }
       [DataMember] public bool  ISPRESTATIONSEULEMENT { get; set; }
       [DataMember] public bool  ISREMBOURSEMENTAVANCE { get; set; }
       [DataMember] public string   TYPEDEMANDE { get; set; }
       [DataMember] public Nullable<System.DateTime>    DATETRANS { get; set; }



        #region Autre
        [DataMember] public string  TYPELOT { get; set; }
        [DataMember] public string NUMEROLOT { get; set; }
        [DataMember] public string LIBELLECOPER { get; set; }
        [DataMember] public string LIBELLENATURE { get; set; }
        [DataMember] public string LIBELLEMODREG { get; set; }
        [DataMember] public string LIBELLEREGROUPEMENT { get; set; }
        [DataMember] public string NOM { get; set; }
        [DataMember] public string ADRESSE { get; set; }
        [DataMember] public Nullable<decimal> SOLDEFACTURE { get; set; }
        [DataMember] public Nullable<decimal> MONTANTPAYPARTIEL { get; set; }
        [DataMember] public bool  IsGlobal { get; set; }
        [DataMember] public bool  IsExonerationTaxe { get; set; }
        [DataMember] public bool  Selectionner { get; set; }
        [DataMember] public Nullable<decimal> AVANCE { get; set; }
        [DataMember] public Nullable<decimal> MONTANTCREDIT { get; set; }
        [DataMember] public Nullable<decimal> MONTANTEXIGIBLE { get; set; }
        [DataMember] public Nullable<decimal> MONTANTNONEXIGIBLE { get; set; }
        [DataMember] public Nullable<decimal> SOLDEAPRESPAIEMENT { get; set; }
        [DataMember] public Nullable<decimal> SOLDECLIENT { get; set; }
        [DataMember] public bool  traiter { get; set; }
        [DataMember] public int NUMETAPE { get; set; }
        [DataMember] public string NUMDEM { get; set; }
        [DataMember] public string NUMDEVIS { get; set; }
        [DataMember] public string TOPANNUL { get; set; }
        [DataMember] public int? FK_IDETAPEDEVIS  { get; set; }
        [DataMember] public string SAISIPAR  { get; set; }
        [DataMember] public Nullable<decimal> PERCU  { get; set; }
        [DataMember] public Nullable<decimal> RENDU  { get; set; }
        [DataMember] public string POSTE { get; set; }
        [DataMember] public int FK_IDPOSTE { get; set; }

        
        [DataMember] public string PLACE { get; set; }
        [DataMember] public string NOMCAISSIERE { get; set; }
        [DataMember] public string LIBELLEAGENCE { get; set; }
        [DataMember] public string LIBELLESITE { get; set; }
        [DataMember] public string LIBELLEBANQUE { get; set; }
        [DataMember] public Nullable<int>  FK_IDTYPEDEVIS { get; set; }
        [DataMember] public Nullable<int>  FK_IDMODEREG { get; set; }
        [DataMember] public Nullable<int>  FK_IDHABILITATIONCAISSE { get; set; }
        [DataMember] public Nullable<int>  FK_IDCAISSIERE{ get; set; }
        [DataMember] public Nullable<int>  FK_IDAGENTSAISIE{ get; set; }
        [DataMember] public Nullable<int>  FK_IDPOSTECLIENT{ get; set; }
        [DataMember] public Nullable<int>  FK_IDLCLIENT{ get; set; }
        [DataMember] public Nullable<decimal> MONTANTNAF  { get; set; }
        [DataMember] public string REFFERENCECLIENT { get; set; }
        [DataMember] public string MOTIFANNULATION { get; set; }
        [DataMember] public bool  IsDEMANDEANNULATION { get; set; }
        [DataMember] public bool  IsREGLEMENTPARNAF { get; set; }
        [DataMember] public bool  IsREGLEMENTNAF { get; set; }
        [DataMember]public Nullable<System.DateTime>  DTRANS { get; set; }

        [DataMember] public string MOTIFDEMANDE { get; set; }
        [DataMember] public string MOTIFREJET { get; set; }
        [DataMember] public string STATUS { get; set; }
        [DataMember] public bool  IsPAIEMENTANTICIPE { get; set; }
        [DataMember] public string  DRES   { get; set; }
        [DataMember] public bool   ISPRECONTENTIEUX   { get; set; }
        [DataMember] public string LIBELLECATEGORIE   { get; set; }
        [DataMember] public string TOURNEE   { get; set; }
        [DataMember] public decimal  MONTANTEMIS   { get; set; }
        [DataMember] public decimal  MONTANTENCAISSE   { get; set; }
        [DataMember] public decimal  TAUXRECOUVREMENT   { get; set; }
        [DataMember] public int  NOMBRE   { get; set; }
        [DataMember] public string  NUMEROMANDAT   { get; set; }
        [DataMember] public string  NUMEROAVISCREDIT  { get; set; }
        [DataMember] public string  COMMUNE  { get; set; }
        [DataMember] public string  QUARTIER  { get; set; }
        [DataMember] public string  RUE  { get; set; }
        [DataMember] public string  PORTE  { get; set; }
        [DataMember] public string  PRODUIT  { get; set; }
        [DataMember] public string  CATEGORIE  { get; set; }

        #endregion
    }
}









