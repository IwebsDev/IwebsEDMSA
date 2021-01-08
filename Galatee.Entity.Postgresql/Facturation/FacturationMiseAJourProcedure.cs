using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Galatee.Entity.Model
{
    public static partial class FacturationProcedure
    {
      public static List<string> retourneMoisComptable(string statut, string dateDebut, string dateFin)
       {
           try
           {
               DateTime? dateDebut_=DateTime.Parse(dateDebut);
               DateTime? dateFin_=DateTime.Parse(dateFin);
               using (galadbEntities context = new galadbEntities())
               {
                   var ARRETE = context.ARRETE;
                   IEnumerable<string> ListeArrete = from arreter in ARRETE
                                                     where arreter.STATUT == statut &&
                                                           arreter.DATEDEBUT == dateDebut_ &&
                                                           arreter.DATEFIN == dateFin_
                                                     select arreter.ANNMOIS.Substring(4, 2) + "/" + arreter.ANNMOIS.Substring(1, 4);
                     return ListeArrete.ToList();                                           ;

               }
           }
           catch (Exception)
           {
               
               throw;
           }
       }


         public static List<CsLotri> retourneListeAMaj()
         {
           try
           {
               List<CsLotri> ListeLotri = new List<CsLotri>();
               #region recup info lotri

                   using (galadbEntities context = new galadbEntities())
                   {

                       var ENTFAC = context.ENTFAC;
                       var LOTRI = context.LOTRI;
                       var USER = context.ADMUTILISATEUR ;

                        ListeLotri =(from entfac in ENTFAC
                                        join lotri in LOTRI on entfac.LOTRI equals lotri.NUMLOTRI
                                        join user in USER on lotri.USERCREATION equals user.MATRICULE 
                                        where entfac.TOPMAJ == null &&
                                            //!new List<string>() { "00100001", "00100002", "00100003" }.Contains(lotri.NUMLOTRI) &&
                                            lotri.NUMLOTRI != "00100001" && lotri.NUMLOTRI != "00100002" && lotri.NUMLOTRI != "00100003" &&
                                            lotri.JET != null &&
                                            (lotri.ETATFAC5 != "M" || lotri.ETATFAC5 == null) &&
                                            (lotri.ETATFAC4 != "S" || lotri.ETATFAC5 == null)
                                        orderby lotri.NUMLOTRI, lotri.JET
                                        select new CsLotri()
                                        {
                                            NUMLOTRI=lotri.NUMLOTRI, 
                                            JET= lotri.JET,
                                            ETATFAC1= lotri.ETATFAC1,
                                            ETATFAC5= lotri.ETATFAC5,
                                            NOMUSER =user.LIBELLE ,
                                            PK_ID = lotri.PK_ID,
                                            TOURNEE = lotri.TOURNEE ,
                                            CATEGORIECLIENT = lotri.CATEGORIECLIENT ,
                                            CENTRE = lotri.CENTRE ,
                                            PERIODE = lotri.PERIODE ,
                                            PERIODICITE = lotri.PERIODICITE ,
                                            FK_IDCENTRE = lotri.FK_IDCENTRE 

                                        }).Distinct().ToList();
                   }
                   return ListeLotri;

                    #endregion
           }
           catch (Exception)
           {
               
               throw;
           }
         }

         public static DataTable  ChargerEvenementLot(CsLotri leLots)
         {
             try
             {
                 #region recup info evenement
                 using (galadbEntities context = new galadbEntities())
                 {
                     var evts = context.EVENEMENT;
                     IEnumerable<object> query = (from x in evts
                                   from y in x.PROFAC 
                                   where x.LOTRI == leLots.NUMLOTRI
                                   select new
                                   {
                                       x.LOTRI,
                                       x.CENTRE,
                                       x.CLIENT,
                                       x.ORDRE,
                                       y.TOURNEE
                                   });
                     return Galatee.Tools.Utility.ListToDataTable<object>(query);
                 }

                 #endregion
             }
             catch (Exception)
             {

                 throw;
             }
         }
         public static List<CsLotri> ChargerLotriNonMisAJours(List<int> lstCentre)
         {
             try
             {
                 List<CsLotri> ListeLotri = new List<CsLotri>();
                 #region recup info lotri
                    
                 using (galadbEntities context = new galadbEntities())
                 {

                     var ENTFAC = context.PROFAC ;
                     var LOTRI = context.LOTRI;
                     ListeLotri = (from entfac in ENTFAC
                                   join lotri in LOTRI on new { entfac.LOTRI, entfac.PRODUIT } equals
                                   new { LOTRI = lotri.NUMLOTRI, lotri.PRODUIT }
                                   where
                                   lstCentre.Contains(entfac.FK_IDCENTRE) && !entfac.FACTURE.Contains("*") && entfac.TOPMAJ == null
                                   orderby entfac.LOTRI, entfac.JET
                                   select new CsLotri
                                   {
                                       NUMLOTRI = entfac.LOTRI,
                                       JET = entfac.JET,
                                       CENTRE = entfac.CENTRE,
                                       FK_IDCENTRE = entfac.FK_IDCENTRE,
                                       ETATFAC4 = lotri.ETATFAC4,
                                       TOURNEE = lotri.TOURNEE,
                                       PRODUIT = lotri.PRODUIT,
                                       FK_IDTOURNEE = lotri.FK_IDTOURNEE,
                                       USERCREATION = lotri.USERCREATION,
                                       USERMODIFICATION = lotri.USERMODIFICATION,
                                       NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == lotri.USERCREATION).LIBELLE,
                                       PERIODE = entfac.PERIODE,
                                       TOPMAJ = entfac.TOPMAJ
                                   }).Distinct().ToList();
                 }
                 return ListeLotri;

                 #endregion
             }
             catch (Exception ex)
             {

                 throw;
             }
         }

         public static List<CsLotri> ChargerLotriNonMisAJoursEdition(List<int> lstCentre)
         {
             try
             {
                 List<CsLotri> ListeLotri = new List<CsLotri>();
                 #region recup info lotri

                 using (galadbEntities context = new galadbEntities())
                 {

                     var ENTFAC = context.ENTFAC;
                     var LOTRI = context.LOTRI;
                     ListeLotri = (from entfac in ENTFAC
                                   join lotri in LOTRI on entfac.LOTRI equals lotri.NUMLOTRI
                                   where
                                   lstCentre.Contains(entfac.FK_IDCENTRE) 
                                   orderby entfac.LOTRI, entfac.JET
                                   select new CsLotri
                                   {
                                       NUMLOTRI = entfac.LOTRI,
                                       JET = entfac.JET,
                                       CENTRE = entfac.CENTRE,
                                       FK_IDCENTRE = entfac.FK_IDCENTRE,
                                       ETATFAC4 = lotri.ETATFAC4,
                                       TOURNEE = lotri.TOURNEE,
                                       PRODUIT = lotri.PRODUIT,
                                       FK_IDTOURNEE = lotri.FK_IDTOURNEE,
                                       USERCREATION = lotri.USERCREATION,
                                       USERMODIFICATION = lotri.USERMODIFICATION,
                                       NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == lotri.USERCREATION).LIBELLE,
                                       PERIODE = entfac.PERIODE,
                                       TOPMAJ = entfac.TOPMAJ
                                   }).Distinct().ToList();

                     
                 }
                 return ListeLotri;

                 #endregion
             }
             catch (Exception ex)
             {

                 throw;
             }
         }


         public static List<CsLotri> ChargerLotriFromEntfac(List<int> lstCentre)
         {
             try
             {
                 List<CsLotri> ListeLotri = new List<CsLotri>();
                 #region recup info lotri
                 using (galadbEntities context = new galadbEntities())
                 {

                     var ENTFAC = context.ENTFAC;
                     var LOTRI = context.LOTRI;
                     ListeLotri = (from entfac in ENTFAC
                                   join lotri in LOTRI on entfac.LOTRI equals lotri.NUMLOTRI
                                   where lstCentre.Contains(entfac.FK_IDCENTRE)
                                   orderby entfac.LOTRI, entfac.JET
                                   select new CsLotri
                                   {
                                       NUMLOTRI = entfac.LOTRI,
                                       JET = entfac.JET,
                                       FACTURE = entfac.FACTURE ,
                                       USERCREATION = lotri.USERCREATION,
                                       USERMODIFICATION = lotri.USERMODIFICATION,

                                   }).Distinct().ToList();
                 }
                 return ListeLotri;

                 #endregion
             }
             catch (Exception ex)
             {

                 throw;
             }
         }

         public static List<CsDirecteur> retourneListeDirecteur()
         {
             //var context = ;
             
             //if ((command is ABO07Entities)!=null)
             //{
             //    context = (ABO07Entities)command;
             //}
             //else
             //{
             //    context = (galadbEntities)command;
             //}

             //if ((command is galadbEntities)!=null)
             //{
             //    context=(galadbEntities)command;
             //}
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                     {
                         //var Directeur = context.DIRECTEUR;
                         //IEnumerable<CsDirecteur> ListeDirecteur = from directeur in Directeur
                         //                                          select new CsDirecteur
                         //                                                             {
                         //                                                               CENTRE      =directeur.CENTRE      , 
                         //                                                               LIBELLE     =directeur.LIBELLE     ,  
                         //                                                               DR          =directeur.DR          , 
                         //                                                               SITE        =directeur.SITE        , 
                         //                                                               ADR1        =directeur.ADR1        , 
                         //                                                               ADR2        =directeur.ADR2        , 
                         //                                                               CPOS        =directeur.CPOS        , 
                         //                                                               BUREAU      =directeur.BUREAU      , 
                         //                                                               TELRENS     =directeur.TELRENS     ,  
                         //                                                               TELDEP      =directeur.TELDEP      ,   
                         //                                                               PRODUIT1    =directeur.PRODUIT1    , 
                         //                                                               PRODUIT2    =directeur.PRODUIT2    , 
                         //                                                               PRODUIT3    =directeur.PRODUIT3    , 
                         //                                                               PRODUIT4    =directeur.PRODUIT4    , 
                         //                                                               PRODUIT5    =directeur.PRODUIT5    , 
                         //                                                               PRODUIT6    =directeur.PRODUIT6    , 
                         //                                                               PRODUIT7    =directeur.PRODUIT7    , 
                         //                                                               PRODUIT8    =directeur.PRODUIT8    , 
                         //                                                               PRODUIT9    =directeur.PRODUIT9    , 
                         //                                                               PRODUIT10   =directeur.PRODUIT10   ,
                         //                                                               PRODOPTION1 =directeur.PRODOPTION1 , 
                         //                                                               PRODOPTION2 =directeur.PRODOPTION2 , 
                         //                                                               PRODOPTION3 =directeur.PRODOPTION3 , 
                         //                                                               PRODOPTION4 =directeur.PRODOPTION4 , 
                         //                                                               PRODOPTION5 =directeur.PRODOPTION5 , 
                         //                                                               PRODOPTION6 =directeur.PRODOPTION6 , 
                         //                                                               PRODOPTION7 =directeur.PRODOPTION7 , 
                         //                                                               PRODOPTION8 =directeur.PRODOPTION8 , 
                         //                                                               PRODOPTION9 =directeur.PRODOPTION9 , 
                         //                                                               PRODOPTION10=directeur.PRODOPTION10,
                         //                                                               NUMID       =directeur.NUMID       , 
                         //                                                               AVAUTO      =directeur.AVAUTO      ,
                         //                                                               FRAISAUTO   =directeur.FRAISAUTO   ,
                         //                                                               FACTURE     =directeur.FACTURE     ,
                         //                                                               DMAJ        =directeur.DMAJ        ,
                         //                                                               TRANS       =directeur.TRANS       ,
                         //                                                             };
                         //                 return ListeDirecteur.ToList();
                         return new List<CsDirecteur>();
                          }
   #endregion

             }
             catch (Exception ex )
             {
                 
                 throw ex;
             }

              

         }

         public static List<ENTFAC> retourneListeEntFac(string lotri, galadbEntities pcontext)
         {
             try
             {
                 #region handle
                 return pcontext.ENTFAC.Where(p => p.LOTRI.Equals(lotri)&& (string.IsNullOrEmpty(p.TOPMAJ) || p.TOPMAJ =="0")).ToList();
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }

         }
      
         public static List<CsClientLotri> RetourneClientsLot(string lot)
         {


             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                     var ClientLotris = context.EVENEMENT;
                     IEnumerable<CsClientLotri> ListeLotri = from clotris in ClientLotris
                                                             where clotris.LOTRI == lot && clotris.STATUS!=10
                                                             select new CsClientLotri
                                                                        {
                                                                            CENTRE = clotris.CENTRE,
                                                                            PRODUIT = clotris.PRODUIT,
                                                                            POINT = clotris.POINT,
                                                                            //FK_IDEVENEMENT = clotris.PK_IDEVENT,
                                                                            ORDRE = clotris.ORDRE,
                                                                            CLIENT = clotris.CLIENT,
                                                                            PERIODE = clotris.PERIODE,  
                                                                                                                                                       
                                                                        };
                     return ListeLotri.ToList();
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }



         }

         public static List<CsProduitFacture> RetourneListeProFac(string lotri, string jet, string centre, string client, string ordre, string lienfac)
         {
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                     var Profac = context.PROFAC;
                     IEnumerable<CsProduitFacture> ListeProfac = from fac in Profac
                                                                where 
                                                                    fac.LOTRI==lotri && 
                                                                    fac.JET==jet && 
                                                                    fac.CENTRE==centre && 
                                                                    fac.CLIENT==client && 
                                                                    fac.ORDRE==ordre && 
                                                                    fac.LIENFAC==lienfac
                                                                    && fac.TOPMAJ == "0"
                                                                select new CsProduitFacture
                                                             {
                                                                LOTRI             = fac.LOTRI               ,
                                                                JET               = fac.JET                 ,
                                                                CENTRE            = fac.CENTRE              ,
                                                                CLIENT            = fac.CLIENT              ,
                                                                ORDRE             = fac.ORDRE               ,
                                                                LIENFAC           = fac.LIENFAC             ,
                                                                FACTURE           = fac.FACTURE             ,
                                                                TOURNEE           = fac.TOURNEE             ,
                                                                ORDTOUR           = fac.ORDTOUR             ,
                                                                PRODUIT           = fac.PRODUIT             ,
                                                                COMPTEUR          = fac.COMPTEUR            ,
                                                                REGLAGECOMPTEUR = fac.REGLAGECOMPTEUR,
                                                                COEFLECT          = fac.COEFLECT            ,
                                                                POINT             = fac.POINT               ,
                                                                PUISSANCE         = fac.PUISSANCE           ,
                                                                DERPERF           = fac.DERPERF             ,
                                                                DERPERFN          = fac.DERPERFN            ,
                                                                REGCONSO          = fac.REGCONSO            ,
                                                                REGFAC            = fac.REGFAC              ,
                                                                TFAC              = fac.TFAC                ,
                                                                LIENRED           = fac.LIENRED             ,
                                                                CONSOFAC          = fac.CONSOFAC            ,
                                                                //DATEEVT           = fac.DATEEVT,
                                                                PERIODE           = fac.PERIODE             ,
                                                                AINDEX            = fac.AINDEX              ,
                                                                NINDEX            = fac.NINDEX              ,
                                                                CAS               = fac.CAS                 ,
                                                                CONSO             = fac.CONSO               ,
                                                                TOTPROHT          = fac.TOTPROHT            ,
                                                                TOTPROTAX         = fac.TOTPROTAX           ,
                                                                TOTPROTTC         = fac.TOTPROTTC           ,
                                                                ADERPERF          = fac.ADERPERF            ,
                                                                ADERPERFN         = fac.ADERPERFN           ,
                                                                REGIMPUTE         = fac.REGIMPUTE           ,
                                                                TYPECOMPTEUR      = fac.TYPECOMPTEUR,
                                                                REGROU            = fac.REGROU              ,
                                                                DEVPRE            = fac.DEVPRE              ,
                                                                NBREDTOT          = fac.NBREDTOT            ,
                                                                STATUS            = fac.STATUS              ,
                                                                EVENEMENT         = fac.EVENEMENT           ,
                                                                PUISSANCEINSTALLEE=fac.PUISSANCEINSTALLEE   ,
                                                                COEFCOMPTAGE      = fac.COEFCOMPTAGE        ,
                                                                BRANCHEMENT       = fac.BRANCHEMENT         ,
                                                                COEFK1            = fac.COEFK1              ,
                                                                COEFK2            = fac.COEFK2              ,
                                                                PERTESACTIVES     = fac.PERTESACTIVES       ,
                                                                PERTESREACTIVES   = fac.PERTESREACTIVES     ,
                                                                COEFFAC           = fac.COEFFAC             ,

                                                             };
                     return ListeProfac.ToList();
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static List<CsRedevanceFacture> RetourneListeRedFac(string lotri, string jet, string centre, string client, string ordre, string lienfac)
         {
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                     var Redfac = context.REDFAC;
                     IEnumerable<CsRedevanceFacture> ListeProfac = from fac in Redfac
                                                                 where
                                                                     fac.LOTRI == lotri &&
                                                                     fac.JET == jet &&
                                                                     fac.CENTRE == centre &&
                                                                     fac.CLIENT == client &&
                                                                     fac.ORDRED == ordre &&
                                                                     fac.LIENFAC == lienfac &&
                                                                    fac.TOPMAJ=="0"
                                                                   select new CsRedevanceFacture
                                                                 {
                                                                    LOTRI          =fac.LOTRI                  ,
                                                                    JET            =fac.JET                    ,
                                                                    CENTRE         =fac.CENTRE                 ,
                                                                    CLIENT         =fac.CLIENT                 ,
                                                                    ORDRED         =fac.ORDRED                 ,
                                                                    LIENRED        =fac.LIENRED                ,
                                                                    FACTURE        =fac.FACTURE                ,
                                                                    REDEVANCE      =fac.REDEVANCE              ,
                                                                    TRANCHE        =fac.TRANCHE                ,
                                                                    //ORDRED         =fac.ORDRED                 ,
                                                                    QUANTITE       =fac.QUANTITE               ,
                                                                    UNITE          =fac.UNITE                  ,
                                                                    BARPRIX        =fac.BARPRIX                ,
                                                                    TAXE           =fac.TAXE                   ,
                                                                    CTAX           =fac.CTAX                   ,
                                                                    DAPP           =fac.DAPP                   ,
                                                                    CRITERE        =fac.CRITERE                ,
                                                                    VARIANTE       =fac.VARIANTE               ,
                                                                    PARAM1         =fac.PARAM1                 ,
                                                                    PARAM2         =fac.PARAM2                 ,
                                                                    PARAM3         =fac.PARAM3                 ,
                                                                    PARAM4         =fac.PARAM4                 ,
                                                                    PARAM5         =fac.PARAM5                 ,
                                                                    PARAM6         =fac.PARAM6                 ,
                                                                    NBJOUR         =fac.NBJOUR                 ,
                                                                    DEBUTAPPLICATION=fac.DEBUTAPPLICATION      ,
                                                                    FINAPPLICATION =fac.FINAPPLICATION         ,
                                                                    LIENFAC        =fac.LIENFAC                ,
                                                                    TOPMAJ         =fac.TOPMAJ                 ,
                                                                    PERIODE        =fac.PERIODE                ,
                                                                    PRODUIT        =fac.PRODUIT                ,
                                                                    FORMULE        =fac.FORMULE                ,
                                                                    BARBORNEDEBUT  =fac.BARBORNEDEBUT          ,
                                                                    BARBORNEFIN    =fac.BARBORNEFIN            ,
                                                                     

                                                                 };
                     return ListeProfac.ToList();
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static CsCanalisation RecupererCanalisation(string centre, string client, string produit, int? point)
         {
              try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                       IEnumerable<object> query = null;
                       var _Ag = context.AG ;
                            query =
                            from _LeAg in _Ag
                            from _LeClient in _LeAg.CLIENT1
                            from _LeAbon in _LeClient.ABON
                            from _canal in _LeAbon.CANALISATION 
                            where _LeClient.CENTRE == centre &&
                                  _LeClient.REFCLIENT == client &&
                                  _LeAbon.PRODUIT1.CODE == produit &&
                                  _canal.POINT == point
                            select new
                            {
                                CLIENT = _LeClient.REFCLIENT ,
                                PRODUIT = _canal.PRODUIT1.CODE ,
                                POINT = _canal.POINT,
                                BRANCHEMENT = _canal.BRANCHEMENT,
                                COMPTEUR = _canal.COMPTEUR,
                                //PROPRIO = _canal.PROPRIO,
                                TCOMPT = _canal.COMPTEUR.TYPECOMPTEUR ,
                                MCOMPT = _canal.COMPTEUR.MARQUE ,
                                _canal.REGLAGECOMPTEUR  , 
                                COEFLECT = _canal.COMPTEUR.COEFLECT,
                                //CADCOMPT = _canal.CADCOMPT ,
                                ANNEEFAB = _canal.COMPTEUR.ANNEEFAB,
                                //ETATCOMPT = _canal.ETATCOMPT,
                                SURFACTURATION = _canal.SURFACTURATION,
                                DEBITANNUEL = _canal.DEBITANNUEL,
                                //MISEENSERVICE = _canal.DEPOSE ,
                                //FINSERVICE = _canal.POSE ,
                                //FONCTIONNEMENT = _canal.FONCTIONNEMENT,
                                //PLOMBAGE = _canal.PLOMBAGE,

                            };
                            DataTable dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                            return Entities.GetEntityFromQuery <CsCanalisation>(dt);
                 }
                 #endregion
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static List<CsEvenement> RecupererEvenements(string centre, string client, string produit, int? point, string lotri, int? status)
         {
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                     var Evenements = context.EVENEMENT;
                     IEnumerable<CsEvenement> ListeEv = from ev in Evenements
                                                                   where
                                                                       ev.LOTRI == lotri &&
                                                                       ev.CENTRE == centre &&
                                                                       ev.CLIENT == client &&
                                                                       ev.PRODUIT == produit &&
                                                                       ev.POINT == point &&
                                                                       ev.STATUS== Galatee.Structure.Enumere.EvenementFacture
                                                                    select new CsEvenement
                                                                   {
                                                                       CENTRE = ev.CENTRE,
                                                                       CLIENT = ev.CLIENT,
                                                                       PRODUIT = ev.PRODUIT,
                                                                       POINT = ev.POINT,
                                                                       NUMEVENEMENT = ev.NUMEVENEMENT,
                                                                       ORDRE = ev.ORDRE,
                                                                       COMPTEUR = ev.COMPTEUR,
                                                                       DATEEVT = ev.DATEEVT,
                                                                       PERIODE = ev.PERIODE,
                                                                       FACPER = ev.FACPER,
                                                                       QTEAREG = ev.QTEAREG,
                                                                       CODEEVT = ev.CODEEVT,
                                                                       INDEXEVT = ev.INDEXEVT,
                                                                       //.CAS = (Convertevan.CAS = (Convert  ,
                                                                       ENQUETE = ev.ENQUETE,
                                                                       CONSO = ev.CONSO,
                                                                       CONSONONFACTUREE = ev.CONSONONFACTUREE,
                                                                       LOTRI = ev.LOTRI,
                                                                       FACTURE = ev.FACTURE,
                                                                       SURFACTURATION = ev.SURFACTURATION,
                                                                       STATUS = ev.STATUS,
                                                                       TYPECONSO = ev.TYPECONSO,
                                                                       REGLAGECOMPTEUR = ev.REGLAGECOMPTEUR,
                                                                       MATRICULE = ev.MATRICULE,
                                                                   };
                     return ListeEv.ToList();
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static int? RecupererSommeProfac(string lotri, string jet, string regroup)
         {
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
                     var Profac = context.PROFAC;
                     int? SommeConsoPPDIV = (from profac in Profac
                                            where
                                               profac.LOTRI == lotri &&
                                               profac.JET == jet &&
                                               profac.REGROU == regroup
                                            select profac.CONSOFAC).Sum();
return SommeConsoPPDIV;
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static void MiseAJourEvenements(List<CsEvenement> events, CsProduitFacture profac, object command)
         {
             try
             {
                 #region Déclaration de variables

                    galadbEntities context = (galadbEntities)command;

                 #endregion

                 #region handle



                         foreach (var item in events)
	                    {
		                      foreach (var Event in context.EVENEMENT.Where(ev=> ev.CENTRE==item.CENTRE &&
                                                                                  ev.CLIENT==item.CLIENT &&
                                                                                  ev.PRODUIT==item.PRODUIT && 
                                                                                  ev.POINT==item.POINT && 
                                                                                  ev.NUMEVENEMENT==item.NUMEVENEMENT
                                                                          )
                                                                    )
	                                    {
		                                    Event.STATUS = item.STATUS;
		                                    Event.FACPER = item.FACPER;
		                                    Event.QTEAREG = item.QTEAREG;
		                                    Event.FACTURE =item.FACTURE;
		                                    Event.CONSONONFACTUREE =item.CONSONONFACTUREE;
		                                    Event.SURFACTURATION = item.SURFACTURATION;
		                                    Event.TYPECONSO = item.TYPECONSO;
		                                    Event.LOTRI = item.LOTRI;
                                            Event.MATRICULE = item.MATRICULE;
	                                    }
	                    }
                   
                 
                 #endregion
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

      

         public static void InsererLclient(List<LCLIENT> LignesCompte, object command)
         {
             try
             {
                 #region Déclaration de variables

                 galadbEntities context = (galadbEntities)command;

                 #endregion

                 #region handle

                     foreach (var LClient in LignesCompte )
                     {
                         context.LCLIENT.Add(LClient);
                     }
                 
                 #endregion
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static void MiseAJour_Entfac_ProFac_RedFac_Annul(CsEnteteFacture entfacs, object command)
         {
             try
             {
                 #region Déclaration de variables

                 galadbEntities context = (galadbEntities)command;

                 #endregion

                 #region handle

                 foreach (var Entfac in context.ENTFAC.Where(entfac => entfac.LIENFAC == entfacs.LIENFAC &&
                                                                         entfac.PERIODE == entfacs.PERIODE &&
                                                                         entfac.FACTURE == entfacs.FACTURE
                                                                         
                                                           ))
                     {
                         Entfac.TOPMAJ = "1";
                     }





                 foreach (var Profac in context.PROFAC.Where(entfac =>   entfac.LIENFAC == entfacs.LIENFAC &&
                                                                         entfac.PERIODE == entfacs.PERIODE &&
                                                                         entfac.FACTURE == entfacs.FACTURE

                                                           ))
                 {
                     Profac.TOPMAJ = "1";
                 }




                 foreach (var Redfac in context.REDFAC.Where(entfac => entfac.CENTRE == entfacs.CENTRE &&
                                                                         entfac.CLIENT == entfacs.CLIENT &&
                                                                         entfac.LOTRI == entfacs.LOTRI &&
                                                                         entfac.JET == entfacs.JET 

                                                           ))
                 {
                     Redfac.TOPMAJ = "1";
                 }






                  foreach (var Annul in context.ANNUL.Where(entfac => entfac.CENTRE == entfacs.CENTRE &&
                                                                         entfac.CLIENT == entfacs.CLIENT &&
                                                                         entfac.ORDRE == entfacs.ORDRE &&
                                                                         entfac.LIENFAC == entfacs.LIENFAC &&
                                                                         entfac.FACTURE == entfacs.FACTURE

                                                           ))
                 {
                     Annul.TOPMAJ = "1";
                 }
                 #endregion
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static void MajLotri_Exig_Action(CsLotri lot, string action, int? nombre, decimal? montant, string matricule, galadbEntities context)
         {
             try
             {
                 #region handle

                    foreach (var item in context.LOTRI.Where(lotri=>lotri.NUMLOTRI==lot.NUMLOTRI && lotri.JET==lot.JET))
                    {
                        item.ETATFAC5 = "M";
                        item.JET = lot.JET;
                    }

                    //foreach (var item in context.ACTION.Where(act=>act.LOT==lot.NUMLOTRI && act.PERIODE==lot.PERIODE && act.ACTION1==action && act.JET==lot.JET && act.DATE1==DateTime.Now && act.NOMBRE1==nombre && act.MONTANT1==montant && act.MATRICULE==matricule && act.STATUT== )
                    //context.ACTION.Add(new ACTION
                    //{
                    //    LOT = lot.NUMLOTRI,
                    //    PERIODE = lot.PERIODE,
                    //    ACTION1 = action,
                    //    JET = lot.JET,
                    //    DATE1 = DateTime.Now,
                    //    NOMBRE1 = nombre,
                    //    MONTANT1 = montant,
                    //    MATRICULE = matricule,
                    //    STATUT = "0"
                    //});
                 //   context.LIBELLEACTIONFACTURATION.Add(new LIBELLEACTIONFACTURATION { ACTION=action, CENTRE=lot.CENTRE, DATECREATION=DateTime.Now, =lot. });
                 //@LOTRI,@periode,@action,@jet,Getdate(),@nombre,@montant,@matricule,@status)

                 //context.exi

                 #endregion
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static List<CsEvenement> RetourneEvenementsHistorique(out DateTime? datePrec, out string dateAbon, string ag, string periode, int? point, string produit, string centre, string ordre)
         {
             try
             {
                 #region handle
                 using (galadbEntities context = new galadbEntities())
                 {
          
                     IEnumerable<object> query = null;
                     //var _CLIENT = context.CLIENT;
                             var _Ag = context.AG ;
                             query =
                             from _LeAg in _Ag
                             from _LeClient in _LeAg.CLIENT1 
                             from _LeAbon in _LeClient.ABON
                             from _lecanal in _LeAbon.CANALISATION
                             from _leEvt in _lecanal.EVENEMENT
                             where 
                                _LeClient.ORDRE == ordre &&
                                _LeClient.CENTRE == centre &&
                                _LeClient.REFCLIENT  == ag &&
                                _LeAbon.PRODUIT1.CODE  == produit &&
                                _leEvt.POINT == point &&
                                _leEvt.PERIODE == periode &&
                                _leEvt.STATUS == Galatee.Structure.Enumere.EvenementMisAJour
                             select new 
                             {
                                 CODEEVT = _leEvt.CODEEVT,
                                 CAS = _leEvt.CAS,
                                 DATEEVT = _leEvt.DATEEVT,
                                 CONSO = _leEvt.CONSO,
                                 TYPECONSO = _leEvt.TYPECONSO,
                                 CONSONONFACTUREE = _leEvt.CONSONONFACTUREE,
                                 SURFACTURATION = _leEvt.SURFACTURATION,
                                 QTEAREG = _leEvt.QTEAREG,
                                 DATEMODIFICATION = _LeAbon.DABONNEMENT 
                             };
                             DataTable dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                             List<CsEvenement> lstEvt = Entities.GetEntityListFromQuery<CsEvenement>(dt);
                             datePrec = lstEvt.Max(even => even.DATEEVT);
                             dateAbon = lstEvt.Single().DATEMODIFICATION.ToString();
                             return lstEvt;





                     //datePrec = Evenements.Max(even=>even.DATEEVT);
                     //dateAbon=Abon.Single( abon=> abon.ORDRE == ordre && abon.CENTRE == centre && abon.CLIENT == ag && abon.PRODUIT == produit).DABONNEMENT.ToString();
                     //IEnumerable<CsEvenement> ListeEv = from ev in Evenements
                     //                                   where
                     //                                       ev.ORDRE == ordre &&
                     //                                       ev.CENTRE == centre &&
                     //                                       ev.CLIENT == ag &&
                     //                                       ev.PRODUIT == produit &&
                     //                                       ev.POINT == point &&
                     //                                       ev.PERIODE == periode &&
                     //                                       ev.STATUS == Galatee.Structure.Enumere.EvenementMisAJour
                     //                                   select new CsEvenement
                     //                                   {
                     //                                       CODEEVT=            ev.CODEEVT            ,
                     //                                       CAS =               ev.CAS                ,
                     //                                       DATEEVT=            ev.DATEEVT            ,
                     //                                       CONSO =             ev.CONSO              ,
                     //                                       TYPECONSO =         ev.TYPECONSO          ,
                     //                                       CONSONONFACTUREE=   ev.CONSONONFACTUREE   ,
                     //                                       SURFACTURATION  =   ev.SURFACTURATION     ,
                     //                                       QTEAREG =           ev.QTEAREG                                                            
                     //                                   };
                     //return ListeEv.ToList();
                 }
                 #endregion

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public static void MajHistorique(CsHistorique historiqueClient, int? debitAnnuel, object command)
         {
             try
             {
                 #region Déclaration de variables

                 galadbEntities context = (galadbEntities)command;
                 var Historique = context.HISTORIQUE.Where(histo => histo.CENTRE == historiqueClient.CENTRE &&
                                                                histo.CLIENT == historiqueClient.CLIENT &&
                                                                histo.PRODUIT == historiqueClient.PRODUIT &&
                                                                histo.POINT == historiqueClient.POINT &&
                                                                histo.ORDRE == historiqueClient.ORDRE &&
                                                                histo.PERIODE == historiqueClient.PERIODE); 

                 var Canalisation =context.CANALISATION.Where(can =>can.CENTRE1.CODE  == historiqueClient.CENTRE &&
                                                                    //can.CLIENT == historiqueClient.CLIENT &&
                                                                    can.PRODUIT1.CODE  == historiqueClient.PRODUIT &&
                                                                    can.POINT == historiqueClient.POINT );
                var nbrHistorique = Historique.Count();
               

                 #endregion

                 #region handle

                 

                    if (nbrHistorique>0)
                    {
                        //MAJ HISTORIQUE
                        foreach (var item in Historique)
                        {
                            item.CONSO = historiqueClient.CONSO;
                            item.CONSOFAC = historiqueClient.CONSOFAC;
                            //item.CUMPER = historiqueClient.CUMPER;
                        }
                                             
                    }
                    else
                    {
                        //INSERTION NEW HISTORIQUE
                        Historique.ToList().Add(new HISTORIQUE {CENTRE = historiqueClient.CENTRE, 
                                                                PRODUIT = historiqueClient.PRODUIT, 
                                                                CLIENT = historiqueClient.CLIENT, 
                                                                POINT = historiqueClient.POINT, 
                                                                ORDRE = historiqueClient.ORDRE, 
                                                                PERIODE = historiqueClient.PERIODE, 
                                                                CONSO = historiqueClient.CONSO,
                                                                CONSOFAC = historiqueClient.CONSOFAC,
                                                                //ci=historiqueClient.CUMPER
                        });
                     
                    }
                    
                 #endregion
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static bool MiseAJourTableStatistiques(List<CsEnteteFacture> entfacs, List<CsProduitFacture> tousLesProfacs, List<CsRedevanceFacture> tousLesRedFacs)
         {
             try
             {
                 using (ABO07Entities context = new ABO07Entities())
                 using (var scope = new TransactionScope())
                 {
                     try
                     {
                         #region handle

                             foreach (var item in Entities.ConvertObject<CENTFAC, CsEnteteFacture>(entfacs))
                             {
                                 context.CENTFAC.Add(item);
                             }
                         
                             foreach (var item in Entities.ConvertObject<CPROFAC, CsProduitFacture>(tousLesProfacs))
                             {
                                 context.CPROFAC.Add(item);
                             }
                             foreach (var item in Entities.ConvertObject<CREDFAC, CsRedevanceFacture>(tousLesRedFacs))
                             {
                                 context.CREDFAC.Add(item);
                             }

                         #endregion
                             context.SaveChanges();
                         scope.Complete();
                     }
                     catch(Exception ex)
                     {
                         throw ex;
                     }
                 }

                 return true;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static void CommitTransaction(object command)
         {
             try
             {
                 #region Déclaration de variables

                 galadbEntities context = (galadbEntities)command;

                 #endregion

                 #region handle

                 context.SaveChanges();

                 #endregion
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public static DataTable RetourneEvenementEntfact(CsLotri leLot)
         {
             try
             {
                 using (galadbEntities pcontext = new galadbEntities())
                 {
                    
                         var _lstEvenement = pcontext.EVENEMENT  ;
                         IEnumerable<object> query = from x in _lstEvenement
                                                     from y in x.PROFAC 
                                                     where
                                                         x.LOTRI == leLot.NUMLOTRI &&
                                                         y.JET  == leLot.JET &&
                                                         x.CENTRE == leLot.CENTRE &&
                                                         x.FK_IDCENTRE == leLot.FK_IDCENTRE
                                                     select new
                                                     {
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.PRODUIT,
                                                         x.POINT,
                                                         x.NUMEVENEMENT,
                                                         x.ORDRE,
                                                         x.COMPTEUR,
                                                         x.DATEEVT,
                                                         x.PERIODE,
                                                         x.CODEEVT,
                                                         x.INDEXEVT,
                                                         x.CAS,
                                                         x.ENQUETE,
                                                         x.CONSO,
                                                         x.CONSONONFACTUREE,
                                                         x.LOTRI,
                                                         x.FACTURE,
                                                         x.SURFACTURATION,
                                                         x.STATUS,
                                                         x.TYPECONSO,
                                                         x.REGLAGECOMPTEUR ,
                                                         x.TYPETARIF,
                                                         x.FORFAIT,
                                                         x.CATEGORIE,
                                                         x.CODECONSO,
                                                         x.PROPRIO,
                                                         x.MODEPAIEMENT,
                                                         x.MATRICULE,
                                                         x.FACPER,
                                                         x.QTEAREG,
                                                         x.DERPERF,
                                                         x.DERPERFN,
                                                         x.CONSOFAC,
                                                         x.REGIMPUTE,
                                                         x.REGCONSO,
                                                         x.COEFLECT,
                                                         x.COEFCOMPTAGE,
                                                         x.PUISSANCE,
                                                         x.TYPECOMPTAGE,
                                                         x.TYPECOMPTEUR,
                                                         x.COEFK1,
                                                         x.COEFK2,
                                                         x.COEFFAC,
                                                         x.USERCREATION,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERMODIFICATION,
                                                         x.PK_ID,
                                                         x.FK_IDCANALISATION,
                                                         x.FK_IDABON,
                                                         x.FK_IDCOMPTEUR,
                                                         x.FK_IDCENTRE,
                                                         x.FK_IDPRODUIT,
                                                         x.ESTCONSORELEVEE,
                                                         x.COMMENTAIRE,
                                                         x.FK_IDTOURNEE,
                                                         x.TOURNEE,
                                                         x.ORDTOUR,
                                                         x.PERFAC,
                                                         x.CONSOMOYENNEPRECEDENTEFACTURE,
                                                         x.DATERELEVEPRECEDENTEFACTURE,
                                                         x.CASPRECEDENTEFACTURE,
                                                         x.INDEXPRECEDENTEFACTURE,
                                                         x.PERIODEPRECEDENTEFACTURE,
                                                         x.ORDREAFFICHAGE,
                                                         x.NOUVEAUCOMPTEUR,
                                                         x.PUISSANCEINSTALLEE
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneEntfact(CsLotri leLot, bool IsLotIsole)
         {
             try
             {
                 using (galadbEntities pcontext = new galadbEntities())
                 {
                     if (!IsLotIsole)
                     {
                         var _lstLEntfact = pcontext.ENTFAC;
                         IEnumerable<object> query = from x in _lstLEntfact
                                                     where
                                                         x.LOTRI == leLot.NUMLOTRI &&
                                                         x.JET == leLot.JET   &&
                                                         x.CENTRE == leLot.CENTRE &&
                                                         x.FK_IDCENTRE == leLot.FK_IDCENTRE
                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.DENABON,
                                                         x.NOMABON,
                                                         x.DENMAND,
                                                         x.NOMMAND,
                                                         x.ADRMAND1,
                                                         x.ADRMAND2,
                                                         x.CPOS,
                                                         x.BUREAU,
                                                         x.BANQUE,
                                                         x.GUICHET,
                                                         x.COMPTE,
                                                         x.RIB,
                                                         x.CODCONSO,
                                                         x.CATEGORIECLIENT,
                                                         x.REGROUPEMENT,
                                                         x.REGEDIT,
                                                         x.AG,
                                                         x.COMMUNE,
                                                         x.QUARTIER,
                                                         x.RUE,
                                                         x.NOMRUE,
                                                         x.NUMRUE,
                                                         x.COMPRUE,
                                                         x.ETAGE,
                                                         x.PORTE,
                                                         x.CADR,
                                                         x.TOURNEE,
                                                         x.ORDTOUR,
                                                         x.NBFAC,
                                                         x.FACTURE,
                                                         x.MES,
                                                         x.TOTFHT,
                                                         x.TOTFTAX,
                                                         x.TOTFTTC,
                                                         x.TOPE,
                                                         x.PERIODE,
                                                         x.EXIG,
                                                         x.COPER,
                                                         x.MODEPAIEMENT,
                                                         x.ANCIENREPORT,
                                                         x.TOTALNONARRONDI,
                                                         x.LIENFAC,
                                                         x.TOPMAJ,
                                                         x.SECTEUR,
                                                         x.DRESABON,
                                                         x.REFERENCEATM,
                                                         x.CODEPROFIL,
                                                         x.PK_ID,
                                                         x.FK_IDCLIENT,
                                                         x.DFAC,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDCATEGORIECLIENT,
                                                         x.FK_IDTOURNEE,
                                                         x.FK_IDCOMMUNE,
                                                         x.FK_IDCENTRE,
                                                         x.ISFACTUREEMAIL,
                                                         x.ISFACTURESMS,
                                                         x.EMAIL,
                                                         x.TELEPHONE,
                                                         //x.FK_IDABON ,
                                                         LIBELLECENTRE = x.CENTRE1.LIBELLE
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                     else
                     {
                         var _lstLEntfact = pcontext.ENTFAC;
                         IEnumerable<object> query = from x in _lstLEntfact
                                                     where
                                                        x.LOTRI == leLot.NUMLOTRI &&
                                                        x.JET == leLot.JET &&
                                                        x.CENTRE == leLot.CENTRE &&
                                                        x.FK_IDCENTRE == leLot.FK_IDCENTRE &&
                                                        (x.USERCREATION == leLot.USERCREATION || x.USERMODIFICATION == leLot.USERMODIFICATION)
                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.DENABON,
                                                         x.NOMABON,
                                                         x.DENMAND,
                                                         x.NOMMAND,
                                                         x.ADRMAND1,
                                                         x.ADRMAND2,
                                                         x.CPOS,
                                                         x.BUREAU,
                                                         x.BANQUE,
                                                         x.GUICHET,
                                                         x.COMPTE,
                                                         x.RIB,
                                                         x.CODCONSO,
                                                         x.CATEGORIECLIENT,
                                                         x.REGROUPEMENT,
                                                         x.REGEDIT,
                                                         x.AG,
                                                         x.COMMUNE,
                                                         x.QUARTIER,
                                                         x.RUE,
                                                         x.NOMRUE,
                                                         x.NUMRUE,
                                                         x.COMPRUE,
                                                         x.ETAGE,
                                                         x.PORTE,
                                                         x.CADR,
                                                         x.TOURNEE,
                                                         x.ORDTOUR,
                                                         x.NBFAC,
                                                         x.FACTURE,
                                                         x.MES,
                                                         x.TOTFHT,
                                                         x.TOTFTAX,
                                                         x.TOTFTTC,
                                                         x.TOPE,
                                                         x.PERIODE,
                                                         x.EXIG,
                                                         x.COPER,
                                                         x.MODEPAIEMENT,
                                                         x.ANCIENREPORT,
                                                         x.TOTALNONARRONDI,
                                                         x.LIENFAC,
                                                         x.TOPMAJ,
                                                         x.SECTEUR,
                                                         x.DRESABON,
                                                         x.REFERENCEATM,
                                                         x.CODEPROFIL,
                                                         x.PK_ID,
                                                         x.FK_IDCLIENT,
                                                         x.DFAC,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDCATEGORIECLIENT,
                                                         x.FK_IDTOURNEE,
                                                         x.FK_IDCOMMUNE,
                                                         x.FK_IDCENTRE,
                                                         x.ISFACTUREEMAIL,
                                                         x.ISFACTURESMS,
                                                         x.EMAIL,
                                                         x.TELEPHONE,
    
                                                         LIBELLECENTRE = x.CENTRE1.LIBELLE
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                 }


             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneRedfact(CsLotri leLot, bool IsLotIsole)
         {
             try
             {
                 using (galadbEntities pcontext = new galadbEntities())
                 {
                     if (!IsLotIsole)
                     {
                         var _lstLRedfact = pcontext.REDFAC;
                         IEnumerable<object> query = from x in _lstLRedfact
                                                     where
                                                        x.LOTRI == leLot.NUMLOTRI  &&
                                                        x.JET == leLot.JET
                                                        &&
                                                        x.CENTRE == leLot.CENTRE &&
                                                        x.FK_IDCENTRE == leLot.FK_IDCENTRE
                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.FACTURE,
                                                         x.LIENRED,
                                                         x.REDEVANCE,
                                                         x.TRANCHE,
                                                         x.ORDRED,
                                                         x.QUANTITE,
                                                         x.UNITE,
                                                         x.BARPRIX,
                                                         x.TAXE,
                                                         x.CTAX,
                                                         x.DAPP,
                                                         x.CRITERE,
                                                         x.VARIANTE,
                                                         x.TOTREDHT,
                                                         x.TOTREDTAX,
                                                         x.TOTREDTTC,
                                                         x.PARAM1,
                                                         x.PARAM2,
                                                         x.PARAM3,
                                                         x.PARAM4,
                                                         x.PARAM5,
                                                         x.PARAM6,
                                                         x.NBJOUR,
                                                         x.DEBUTAPPLICATION,
                                                         x.FINAPPLICATION,
                                                         x.LIENFAC,
                                                         x.TOPMAJ,
                                                         x.PERIODE,
                                                         x.PRODUIT,
                                                         x.FORMULE,
                                                         x.TOPANNUL,
                                                         x.BARBORNEDEBUT,
                                                         x.BARBORNEFIN,
                                                         x.PK_ID,
                                                         x.FK_IDENTFAC,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDPRODUIT,
                                                         x.FK_IDCENTRE,
                                                         x.FK_IDABON
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                     else
                     {
                         var _lstRedfact = pcontext.REDFAC;
                         IEnumerable<object> query = from x in _lstRedfact
                                                     where
                                                        x.LOTRI == leLot.NUMLOTRI 
                                                        &&
                                                        x.JET == leLot.JET &&
                                                        x.CENTRE == leLot.CENTRE &&
                                                        x.FK_IDCENTRE == leLot.FK_IDCENTRE &&
                                                        (x.USERCREATION == leLot.USERCREATION || x.USERMODIFICATION == leLot.USERMODIFICATION)

                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.FACTURE,
                                                         x.LIENRED,
                                                         x.REDEVANCE,
                                                         x.TRANCHE,
                                                         x.ORDRED,
                                                         x.QUANTITE,
                                                         x.UNITE,
                                                         x.BARPRIX,
                                                         x.TAXE,
                                                         x.CTAX,
                                                         x.DAPP,
                                                         x.CRITERE,
                                                         x.VARIANTE,
                                                         x.TOTREDHT,
                                                         x.TOTREDTAX,
                                                         x.TOTREDTTC,
                                                         x.PARAM1,
                                                         x.PARAM2,
                                                         x.PARAM3,
                                                         x.PARAM4,
                                                         x.PARAM5,
                                                         x.PARAM6,
                                                         x.NBJOUR,
                                                         x.DEBUTAPPLICATION,
                                                         x.FINAPPLICATION,
                                                         x.LIENFAC,
                                                         x.TOPMAJ,
                                                         x.PERIODE,
                                                         x.PRODUIT,
                                                         x.FORMULE,
                                                         x.TOPANNUL,
                                                         x.BARBORNEDEBUT,
                                                         x.BARBORNEFIN,
                                                         x.PK_ID,
                                                         x.FK_IDENTFAC,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDPRODUIT,
                                                         x.FK_IDCENTRE,
                                                         x.FK_IDABON
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static DataTable RetourneProfact(CsLotri leLot, bool IsLotIsole)
         {
             try
             {
                 using (galadbEntities pcontext = new galadbEntities())
                 {
                     if (!IsLotIsole)
                     {
                         var _lstLRedfact = pcontext.PROFAC;
                         IEnumerable<object> query = from x in _lstLRedfact
                                                     where
                                                         x.LOTRI == leLot.NUMLOTRI &&
                                                         x.JET == leLot.JET
                                                         &&
                                                         x.CENTRE == leLot.CENTRE &&
                                                         x.FK_IDCENTRE == leLot.FK_IDCENTRE
                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.TOURNEE,
                                                         x.ORDTOUR,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.FACTURE,
                                                         x.PRODUIT,
                                                         x.COMPTEUR,
                                                         x.REGLAGECOMPTEUR,
                                                         x.COEFLECT,
                                                         x.POINT,
                                                         x.PUISSANCE,
                                                         x.DERPERF,
                                                         x.DERPERFN,
                                                         x.REGCONSO,
                                                         x.REGFAC,
                                                         x.TFAC,
                                                         x.LIENRED,
                                                         x.CONSOFAC,
                                                         //x.DATEEVT ,
                                                         x.PERIODE,
                                                         x.AINDEX,
                                                         x.NINDEX,
                                                         x.CAS,
                                                         x.CONSO,
                                                         x.TOTPROHT,
                                                         x.TOTPROTAX,
                                                         x.TOTPROTTC,
                                                         x.ADERPERF,
                                                         x.ADERPERFN,
                                                         x.REGIMPUTE,
                                                         x.TYPECOMPTEUR,
                                                         x.REGROU,
                                                         x.DEVPRE,
                                                         x.NBREDTOT,
                                                         x.STATUS,
                                                         x.LIENFAC,
                                                         x.EVENEMENT,
                                                         x.TOPMAJ,
                                                         x.TOPANNUL,
                                                         x.PUISSANCEINSTALLEE,
                                                         x.COEFCOMPTAGE,
                                                         x.BRANCHEMENT,
                                                         x.COEFK1,
                                                         x.COEFK2,
                                                         x.PERTESACTIVES,
                                                         x.PERTESREACTIVES,
                                                         x.COEFFAC,
                                                         x.PK_ID,
                                                         x.FK_IDENTFAC,
                                                         x.FK_IDEVENEMENT,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDPRODUIT,
                                                         x.FK_IDCAS,
                                                         x.FK_IDCENTRE,
                                                         x.FK_IDABON
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                     else
                     {
                         var _lstProffact = pcontext.PROFAC;
                         IEnumerable<object> query = from x in _lstProffact
                                                     where
                                                        x.LOTRI == leLot.NUMLOTRI &&
                                                        x.JET == leLot.JET &&
                                                        x.CENTRE == leLot.CENTRE &&
                                                        x.FK_IDCENTRE == leLot.FK_IDCENTRE &&
                                                       ( x.USERCREATION == leLot.USERCREATION || x.USERMODIFICATION == leLot.USERMODIFICATION )
                                                     select new
                                                     {
                                                         x.LOTRI,
                                                         x.JET,
                                                         x.TOURNEE,
                                                         x.ORDTOUR,
                                                         x.CENTRE,
                                                         x.CLIENT,
                                                         x.ORDRE,
                                                         x.FACTURE,
                                                         x.PRODUIT,
                                                         x.COMPTEUR,
                                                         x.REGLAGECOMPTEUR,
                                                         x.COEFLECT,
                                                         x.POINT,
                                                         x.PUISSANCE,
                                                         x.DERPERF,
                                                         x.DERPERFN,
                                                         x.REGCONSO,
                                                         x.REGFAC,
                                                         x.TFAC,
                                                         x.LIENRED,
                                                         x.CONSOFAC,
                                                         //x.DATEEVT ,
                                                         x.PERIODE,
                                                         x.AINDEX,
                                                         x.NINDEX,
                                                         x.CAS,
                                                         x.CONSO,
                                                         x.TOTPROHT,
                                                         x.TOTPROTAX,
                                                         x.TOTPROTTC,
                                                         x.ADERPERF,
                                                         x.ADERPERFN,
                                                         x.REGIMPUTE,
                                                         x.TYPECOMPTEUR,
                                                         x.REGROU,
                                                         x.DEVPRE,
                                                         x.NBREDTOT,
                                                         x.STATUS,
                                                         x.LIENFAC,
                                                         x.EVENEMENT,
                                                         x.TOPMAJ,
                                                         x.TOPANNUL,
                                                         x.PUISSANCEINSTALLEE,
                                                         x.COEFCOMPTAGE,
                                                         x.BRANCHEMENT,
                                                         x.COEFK1,
                                                         x.COEFK2,
                                                         x.PERTESACTIVES,
                                                         x.PERTESREACTIVES,
                                                         x.COEFFAC,
                                                         x.PK_ID,
                                                         x.FK_IDENTFAC,
                                                         x.FK_IDEVENEMENT,
                                                         x.DATECREATION,
                                                         x.DATEMODIFICATION,
                                                         x.USERCREATION,
                                                         x.USERMODIFICATION,
                                                         x.FK_IDPRODUIT,
                                                         x.FK_IDCAS,
                                                         x.FK_IDCENTRE,
                                                         x.FK_IDABON
                                                     };
                         return Galatee.Tools.Utility.ListToDataTable<object>(query);
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public static void CreationLotIsole(List<CsEvenement> lstEvenement, galadbEntities _LeContextInter, galadbEntities pContext)
         {
             CsEvenement lsEvt = lstEvenement.First();
             string numlotri = lsEvt.LOTRI;
             LOTRI _leLotriIsole = _LeContextInter.LOTRI.FirstOrDefault(l => l.USERCREATION == lsEvt.USERCREATION &&
                                                                             l.CENTRE == lsEvt.CENTRE &&
                                                                             numlotri == l.NUMLOTRI &&
                                                                             l.PRODUIT == lsEvt.PRODUIT);
             LOTRI NewLotri = null;
             if (_leLotriIsole == null)
             {
                 NewLotri = new LOTRI
                 {
                     BASE = "S",
                     CATEGORIECLIENT = "99",
                     CENTRE = lsEvt.CENTRE,
                     FK_IDCENTRE = lsEvt.FK_IDCENTRE,
                     DATECREATION = DateTime.Now,
                     DATEMODIFICATION = DateTime.Now,
                     FK_IDCATEGORIECLIENT = null,
                     FK_IDPRODUIT = lsEvt.FK_IDPRODUIT,
                     FK_IDRELEVEUR = null,
                     NUMLOTRI = lsEvt.LOTRI,
                     PERIODE = DateTime.Now.Year + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()),
                     PERIODICITE = "01",
                     PRODUIT = lsEvt.PRODUIT,
                     RELEVEUR = "99",
                     TOURNEE = "000",
                     USERCREATION = lsEvt.USERCREATION,
                     USERMODIFICATION = lsEvt.USERCREATION,
                 };
             }
             else
             {
                 _leLotriIsole.JET = null;
                 _leLotriIsole.DFAC = null;
                 _leLotriIsole.ETATFAC1 = null;
                 _leLotriIsole.ETATFAC2 = null;
                 _leLotriIsole.ETATFAC5 = null;

             }
             _LeContextInter.Dispose();

             if (NewLotri != null)
                 Entities.InsertEntity<LOTRI>(NewLotri, pContext);
             else
                 Entities.UpdateEntity<LOTRI>(_leLotriIsole, pContext);

             pContext.SaveChanges();
         }

    }
}