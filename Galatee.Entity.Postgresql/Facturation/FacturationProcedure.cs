using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections ;
using Galatee.Structure;
//using Galatee.DataAccess;

namespace Galatee.Entity.Model
{
  public  static  partial class FacturationProcedure
   {

      public static int[] StatusEvenementNonFacture()
      {
          int[] strArray =
                {
                  Enumere.EvenementFacture 
                };
          return strArray;

      }
      public static int[] StatusEvenementFacture()
      {
          int[] strArray =
                {
                  Enumere.EvenementFacture,Enumere.EvenementMisAJour ,Enumere.EvenementPurger  
                };
          return strArray;

      }
       #region Calcul
      public static DataTable ChargerDesEvenementClientLot(CsClient  leClient,string numlot)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  int[] LstStatut = StatusEvenementNonFacture();
                  var _LeEvt = context.EVENEMENT;
                  var _Entetfact = context.ENTFAC ;
                  IEnumerable<object> query =
                  from x in _LeEvt
                  join y in _Entetfact on new { x.FK_IDCENTRE, x.CENTRE , x.CLIENT ,x.ORDRE ,x.PERIODE ,x.LOTRI  }
                        equals new { y.FK_IDCENTRE, y.CENTRE, y.CLIENT, y.ORDRE, y.PERIODE, y.LOTRI } into _pTemp
                  from p in _pTemp.DefaultIfEmpty()
                  where (x.LOTRI == numlot &&
                         (x.FK_IDCENTRE == leClient.FK_IDCENTRE) &&
                         (x.CENTRE == leClient.CENTRE) &&
                         (x.CLIENT == leClient.REFCLIENT ) &&
                         (x.ORDRE == leClient.ORDRE) &&
                         LstStatut.Contains(x.STATUS.Value))
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
                      x.REGLAGECOMPTEUR,
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
                      x.FK_IDCENTRE,
                      x.FK_IDPRODUIT,
                      x.FK_IDCANALISATION,
                      x.FK_IDABON,
                      x.FK_IDCOMPTEUR,
                      x.TOURNEE,
                      x.FK_IDTOURNEE,
                      x.ESTCONSORELEVEE,
                      x.CASPRECEDENTEFACTURE,
                      x.CONSOMOYENNEPRECEDENTEFACTURE,
                      x.DATERELEVEPRECEDENTEFACTURE,
                      x.INDEXPRECEDENTEFACTURE,
                      x.PERIODEPRECEDENTEFACTURE,
                      MONTANT = p.TOTFTTC 
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
      public static DataTable ChargerDesEvenementPourRejet(CsLotri leLotri)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  int[] LstStatut = StatusEvenementNonFacture();
                  var _LeEvt = context.EVENEMENT;
                  var _Entetfact = context.ENTFAC ;
                  IEnumerable<object> query =
                  from x in _LeEvt
                  join y in _Entetfact on new { x.FK_IDCENTRE, x.CENTRE , x.CLIENT ,x.ORDRE ,x.PERIODE ,x.LOTRI  }
                        equals new { y.FK_IDCENTRE, y.CENTRE, y.CLIENT, y.ORDRE, y.PERIODE, y.LOTRI } into _pTemp
                  from p in _pTemp.DefaultIfEmpty()
                  where (x.LOTRI == leLotri.NUMLOTRI &&
                         (x.FK_IDCENTRE == 0 || x.FK_IDCENTRE == leLotri.FK_IDCENTRE) &&
                         (x.FK_IDTOURNEE == 0 || x.FK_IDTOURNEE == leLotri.FK_IDTOURNEE) &&
                         LstStatut.Contains(x.STATUS.Value))
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
                      x.REGLAGECOMPTEUR,
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
                      x.FK_IDCENTRE,
                      x.FK_IDPRODUIT,
                      x.FK_IDCANALISATION,
                      x.FK_IDABON,
                      x.FK_IDCOMPTEUR,
                      x.TOURNEE,
                      x.FK_IDTOURNEE,
                      x.ESTCONSORELEVEE,
                      x.CASPRECEDENTEFACTURE,
                      x.CONSOMOYENNEPRECEDENTEFACTURE,
                      x.DATERELEVEPRECEDENTEFACTURE,
                      x.INDEXPRECEDENTEFACTURE,
                      x.PERIODEPRECEDENTEFACTURE,
                      MONTANT=  p.TOTFTTC 
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable ChargerDesEvenement(CsLotri leLotri)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  int[] LstStatut = StatusEvenementNonFacture();
                  var _LeEvt = context.EVENEMENT;
                  IEnumerable<object> query =
                  from x in _LeEvt
                  where (x.LOTRI == leLotri.NUMLOTRI && 
                         (x.FK_IDCENTRE ==0 || x.FK_IDCENTRE == leLotri.FK_IDCENTRE ) &&
                         (x.FK_IDTOURNEE == 0 || x.FK_IDTOURNEE == leLotri.FK_IDTOURNEE ) &&
                         LstStatut.Contains(x.STATUS.Value))
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
                      x.REGLAGECOMPTEUR,
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
                      x.FK_IDCENTRE,
                      x.FK_IDPRODUIT,
                      x.FK_IDCANALISATION,
                      x.FK_IDABON ,
                      x.FK_IDCOMPTEUR ,
                      x.TOURNEE ,
                      x.FK_IDTOURNEE ,
                      x.ESTCONSORELEVEE,
                      x.CASPRECEDENTEFACTURE,
                      x.CONSOMOYENNEPRECEDENTEFACTURE,
                      x.DATERELEVEPRECEDENTEFACTURE,
                      x.INDEXPRECEDENTEFACTURE,
                      x.PERIODEPRECEDENTEFACTURE,
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable verifieSaisieTotal(List<CsLotri> pLot)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  string lot = pLot.First().NUMLOTRI ;
                  string periode = pLot.First().PERIODE  ;
                  List<int> lstIdCentre = pLot.Select(t => t.FK_IDCENTRE ).Distinct().ToList();

                  var _LEvt = context.EVENEMENT ;
                  IEnumerable<object> query =
                  (from x in _LEvt
                   where (x.LOTRI ==lot && x.PERIODE == periode && lstIdCentre.Contains(x.FK_IDCENTRE) && x.STATUS == Enumere.EvenementCree)
                  select new
                  {
                       NUMLOTRI= x.LOTRI ,
                      x.PERIODE,
                  }).Take(1);
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static DataTable verifieSimulation(List<CsLotri> pLot)
      {
          try
          {
              string lot = pLot.First().NUMLOTRI;
              string periode = pLot.First().PERIODE;
              string usercreation = pLot.First().USERCREATION ;
             
              List<int> lstIdCentre = pLot.Select(t => t.FK_IDCENTRE).Distinct().ToList();
              using (galadbEntities context = new galadbEntities())
              {
                  var _Lotri = context.LOTRI;
                  var _LEntfac = context.ENTFAC;
                  IEnumerable<object> query = null;

                  if (!CommonProcedures.IsLotIsole(lot))
                  {
                      query =
                     from x in _LEntfac
                     where (x.LOTRI == lot && x.PERIODE == periode && lstIdCentre.Contains(x.FK_IDCENTRE) && x.FACTURE.Contains("*"))
                     select new
                     {
                         NUMLOTRI = x.LOTRI,
                         x.PERIODE,
                         x.CENTRE,
                         LIBELLECENTRE = x.CENTRE1.LIBELLE,
                         NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION).LIBELLE
                     };
                  }
                  else
                  {
                      query =
                      from x in _LEntfac
                      where (x.LOTRI == lot && x.USERCREATION == usercreation  && x.FACTURE.Contains("*"))
                      select new
                      {
                          NUMLOTRI = x.LOTRI,
                          x.PERIODE,
                          x.CENTRE,
                          LIBELLECENTRE = x.CENTRE1.LIBELLE,
                          NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION).LIBELLE
                      };
                  }
                  return Galatee.Tools.Utility.ListToDataTable(query);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }
       public static DataTable ChargerDetailLotri(List<int> pListeIdCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _Lotri = context.LOTRI;
                   var _LEntfac = context.ENTFAC ;
                   IEnumerable<object> query =
                   from x in _Lotri
                   join y in _LEntfac on new { x.FK_IDCENTRE, x.PERIODE, x.NUMLOTRI }
                       equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI =y.LOTRI  } into _pTemp
                   from p in _pTemp.DefaultIfEmpty()

                   where (pListeIdCentre.Contains(x.FK_IDCENTRE))
                   select new
                   {
                       x.NUMLOTRI,
                       p.JET,
                       p.FACTURE,
                       x.PERIODE,
                       x.CENTRE,
                       x.PRODUIT,
                       x.CATEGORIECLIENT,
                       x.PERIODICITE,
                       x.EXIG,
                       x.DFAC,
                       x.ETATFAC1,
                       x.ETATFAC2,
                       x.ETATFAC3,
                       x.ETATFAC4,
                       x.ETATFAC5,
                       x.ETATFAC6,
                       x.ETATFAC7,
                       x.ETATFAC8,
                       x.ETATFAC9,
                       x.ETATFAC10,
                       x.TOURNEE,
                       x.RELEVEUR,
                       x.BASE,
                       x.PK_ID,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.FK_IDPRODUIT,
                       x.FK_IDCATEGORIECLIENT,
                       x.FK_IDRELEVEUR,
                       x.FK_IDCENTRE,
                       x.FK_IDTOURNEE,
                       LIBELLECENTRE = x.CENTRE1.LIBELLE,
                       NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION).LIBELLE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerDetailLotriPourDefacturation(List<int>  LstIdCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _Lotri = context.LOTRI;
                   IEnumerable<object> query =
                   from x in _Lotri
                   where (x.ETATFAC3  ==Enumere.DemandeDefacturation)
                   && LstIdCentre.Contains(x.FK_IDCENTRE )
                   select new
                   {
                       x.NUMLOTRI,
                       x.JET,
                       x.PERIODE,
                       x.CENTRE,
                       x.PRODUIT,
                       x.CATEGORIECLIENT,
                       x.PERIODICITE,
                       x.EXIG,
                       x.DFAC,
                       x.ETATFAC1,
                       x.ETATFAC2,
                       x.ETATFAC3,
                       x.ETATFAC4,
                       x.ETATFAC5,
                       x.ETATFAC6,
                       x.ETATFAC7,
                       x.ETATFAC8,
                       x.ETATFAC9,
                       x.ETATFAC10,
                       x.TOURNEE,
                       x.RELEVEUR,
                       x.BASE,
                       x.PK_ID,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.FK_IDPRODUIT,
                       x.FK_IDCATEGORIECLIENT,
                       x.FK_IDRELEVEUR,
                       x.FK_IDCENTRE ,
                       x.FK_IDTOURNEE,
                       NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == x.USERCREATION).LIBELLE 

                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerNiveauTarifCentre(CsCentre pCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {

                   var _LaVariable = context.VARIABLETARIF;
                   var _LVariante = context.VARIANTE;
                   IEnumerable<object> query =
                   from x in _LaVariable
                   join j in _LVariante on new { x.REDEVANCE1.PRODUIT , x.REDEVANCE, x.CENTRE }
                               equals new { j.PRODUIT, j.REDEVANCE, j.CENTRE } into pTemp2
                   from p in pTemp2.DefaultIfEmpty()
                   where (x.CENTRE == pCentre.CODE && x.FK_IDCENTRE == pCentre.PK_ID )
                   select new
                   {        x.REDEVANCE1.PRODUIT ,
                            x.REDEVANCE1.FK_IDPRODUIT ,
                            x.REDEVANCE ,
                            x.REGION ,
                            x.SREGION ,
                            x.CENTRE ,
                            x.COMMUNE ,
                            x.ORDREEDITION ,
                            x.DATEAPPLICATION ,
                            x.RECHERCHETARIF ,
                            x.MODECALCUL ,
                            x.MODEAPPLICATION ,
                            x.LIBELLECOMPTABLE ,
                            x.COMPTECOMPTABLE ,
                            x.ESTANALYTIQUE ,
                            x.GENERATIONANOMALIE ,
                            x.FORMULE ,
                            x.PK_ID ,
                            x.DATECREATION ,
                            x.DATEMODIFICATION ,
                            x.USERCREATION ,
                            x.USERMODIFICATION ,
                            x.FK_IDREDEVANCE ,
                            x.FK_IDCENTRE ,
                            x.FK_IDMODEAPPLICATION ,
                            x.FK_IDMODECALCUL ,
                            x.FK_IDRECHERCHETARIF,
                            p.CRITERE,
                            p.NUMVARIANTE,
                            p.PARAM1,
                            p.PARAM2,
                            p.PARAM3,
                            p.PARAM4,
                            p.PARAM5,
                            p.PARAM6
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerAllCtarcompParPeriode()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {

                   var _LeCtarcompt = context.CTARCOMPPARPERIODE ;
                   IEnumerable<object> query =
                   from t in _LeCtarcompt
                   select new
                   {
                       t.PK_ID,
                       t.RECHERCHETARIF,
                       t.FK_IDCENTRETARIF,
                       t.FK_IDPRODUIT,
                       t.FK_IDABON,
                       t.LOTRI,
                       t.PERIODE,
                       t.CTARCOMP
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerAllNiveauTarifCentre()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {

                   var _LaVariable = context.VARIABLETARIF;
                   var _LVariante = context.VARIANTE;
                   IEnumerable<object> query =
                   from x in _LaVariable
                   join j in _LVariante on new { x.REDEVANCE1.PRODUIT, x.REDEVANCE, x.CENTRE }
                               equals new { j.PRODUIT, j.REDEVANCE, j.CENTRE } into pTemp2
                   from p in pTemp2.DefaultIfEmpty()
                   select new
                   {
                       x.REDEVANCE1.PRODUIT,
                       x.REDEVANCE1.FK_IDPRODUIT,
                       x.REDEVANCE,
                       x.REGION,
                       x.SREGION,
                       x.CENTRE,
                       x.COMMUNE,
                       x.ORDREEDITION,
                       x.DATEAPPLICATION,
                       x.RECHERCHETARIF,
                       x.MODECALCUL,
                       x.MODEAPPLICATION,
                       x.LIBELLECOMPTABLE,
                       x.COMPTECOMPTABLE,
                       x.ESTANALYTIQUE,
                       x.GENERATIONANOMALIE,
                       x.FORMULE,
                       x.PK_ID,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.FK_IDREDEVANCE,
                       x.FK_IDCENTRE,
                       x.FK_IDMODEAPPLICATION,
                       x.FK_IDMODECALCUL,
                       x.FK_IDRECHERCHETARIF,
                       p.CRITERE,
                       p.NUMVARIANTE,
                       p.PARAM1,
                       p.PARAM2,
                       p.PARAM3,
                       p.PARAM4,
                       p.PARAM5,
                       p.PARAM6
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerNiveauTarifCommune()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LaGoeges = context.GEOGES;
                   var _LVartar = context.VARIABLETARIF;
                   var _LVariante = context.VARIANTE;

                   IEnumerable<object> query =
                   from x in _LaGoeges
                   join y in _LVartar on x.COMMUNE equals y.COMMUNE into pTemp
                   from p in pTemp.DefaultIfEmpty()
                   join j in _LVariante on new { p.REDEVANCE1.FK_IDPRODUIT  , p.REDEVANCE }
                    equals new { j.FK_IDPRODUIT , j.REDEVANCE } into pTemp2
                   from t in pTemp2.DefaultIfEmpty()
                   select new
                   {
                            p.REDEVANCE1.PRODUIT ,
                            p.REDEVANCE1.FK_IDPRODUIT ,
                            p.REDEVANCE ,
                            p.REGION ,
                            p.SREGION ,
                            p.CENTRE ,
                            p.COMMUNE ,
                            p.ORDREEDITION ,
                            p.DATEAPPLICATION ,
                            p.RECHERCHETARIF ,
                            p.MODECALCUL ,
                            p.MODEAPPLICATION ,
                            p.LIBELLECOMPTABLE ,
                            p.COMPTECOMPTABLE ,
                            p.ESTANALYTIQUE ,
                            p.GENERATIONANOMALIE ,
                            p.FORMULE ,
                            p.PK_ID ,
                            p.DATECREATION ,
                            p.DATEMODIFICATION ,
                            p.USERCREATION ,
                            p.USERMODIFICATION ,
                            p.FK_IDREDEVANCE ,
                            p.FK_IDCENTRE ,
                            p.FK_IDMODEAPPLICATION ,
                            p.FK_IDMODECALCUL ,
                            p.FK_IDRECHERCHETARIF,
                       t.CRITERE,
                       t.NUMVARIANTE ,
                       t.PARAM1,
                       t.PARAM2,
                       t.PARAM3,
                       t.PARAM4,
                       t.PARAM5,
                       t.PARAM6,
                       //p.FEUILLE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerNiveauTarifNationnal()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {

                   var _LaVariable = context.VARIABLETARIF;
                   var _LVariante = context.VARIANTE;
                   var  query =
                   from x in _LaVariable
                   join j in _LVariante on new { x.REDEVANCE1.FK_IDPRODUIT , x.REDEVANCE }
                               equals new { j.FK_IDPRODUIT , j.REDEVANCE } into pTemp2
                   from p in pTemp2.DefaultIfEmpty()
                   where x.CENTRE == Enumere.Generale
                   select new
                   {
                       x.REDEVANCE1.PRODUIT,
                       x.REDEVANCE1.FK_IDPRODUIT,
                       x.REDEVANCE,
                       x.REGION,
                       x.SREGION,
                       x.CENTRE,
                       x.COMMUNE,
                       x.ORDREEDITION,
                       x.DATEAPPLICATION,
                       x.RECHERCHETARIF,
                       x.MODECALCUL,
                       x.MODEAPPLICATION,
                       x.LIBELLECOMPTABLE,
                       x.COMPTECOMPTABLE,
                       x.ESTANALYTIQUE,
                       x.GENERATIONANOMALIE,
                       x.FORMULE,
                       x.PK_ID,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION,
                       x.FK_IDREDEVANCE,
                       x.FK_IDCENTRE,
                       x.FK_IDMODEAPPLICATION,
                       x.FK_IDMODECALCUL,
                       x.FK_IDRECHERCHETARIF
                      
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerVariableTarification()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LaVariable = context.VARIABLETARIF;

                   IEnumerable<object> query =
                   from x in _LaVariable
                   select new
                   {
                       x.CENTRE,
                       x.COMMUNE,
                       x.REDEVANCE1.PRODUIT,
                       x.REDEVANCE,
                       //x.ORDREDEVANCE,
                       //x.FEUILLE,
                       //x.DAPP,
                       x.RECHERCHETARIF,
                       x.MODECALCUL ,
                 
                       x.FORMULE,
                       //x.TYPETARIF,
                       //x.CAPP,
                       //x.LIBRED,
                       //x.MOIS,
                       x.PK_ID,
                       //x.TRANS
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable  ChargerTarifFacturation(int idCentre)
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeTarif= context.TARIFFACTURATION ;

                       IEnumerable<object> query =
                       from x in _LeTarif
                       where x.FK_IDCENTRE == idCentre || idCentre ==0
                       select new
                       {
                           x.PRODUIT,
                           x.REDEVANCE,
                           x.REGION,
                           x.SREGION,
                           x.CENTRE,
                           x.COMMUNE,
                           x.RECHERCHETARIF,
                           x.CTARCOMP,
                           x.DEBUTAPPLICATION,
                           x.FINAPPLICATION,
                           x.PERDEB,
                           x.PERFIN,
                           x.TAXE,
                           x.UNITE,
                           x.MONTANTANNUEL,
                           x.MINIVOL,
                           x.MINIVAL,
                           x.FORFVOL,
                           x.FORFVAL,
                           x.DATECREATION,
                           x.DATEMODIFICATION,
                           x.USERCREATION,
                           x.USERMODIFICATION,
                           x.FK_IDPRODUIT,
                           x.FK_IDCENTRE,
                           x.FK_IDVARIABLETARIF,
                           x.FK_IDTAXE,
                           x.FK_IDUNITECOMPTAGE,
                           x.PK_ID
                       };
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerAllTarifFacturation()
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeTarif = context.TARIFFACTURATION;

                       IEnumerable<object> query =
                       from x in _LeTarif
                       select new
                       {
                           x.PRODUIT,
                           x.REDEVANCE,
                           x.REGION,
                           x.SREGION,
                           x.CENTRE,
                           x.COMMUNE,
                           x.RECHERCHETARIF,
                           x.CTARCOMP,
                           x.DEBUTAPPLICATION,
                           x.FINAPPLICATION,
                           x.PERDEB,
                           x.PERFIN,
                           x.TAXE,
                           x.UNITE,
                           x.MONTANTANNUEL,
                           x.MINIVOL,
                           x.MINIVAL,
                           x.FORFVOL,
                           x.FORFVAL,
                           x.DATECREATION,
                           x.DATEMODIFICATION,
                           x.USERCREATION,
                           x.USERMODIFICATION,
                           x.FK_IDPRODUIT,
                           x.FK_IDCENTRE,
                           x.FK_IDVARIABLETARIF,
                           x.FK_IDTAXE,
                           x.FK_IDUNITECOMPTAGE,
                           x.PK_ID
                       };
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerDetailTarifFacturation(int idCentre)
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeTarif = context.DETAILTARIFFACTURATION;

                       IEnumerable<object> query =
                       from x in _LeTarif
                       where x.TARIFFACTURATION.FK_IDCENTRE == idCentre || idCentre ==0
                       select new
                       {
                           x.FK_IDTARIFFACTURATION,
                           x.FK_IDREDEVANCE,
                           x.NUMEROTRANCHE,
                           x.QTEANNUELMAXI,
                           x.PRIXUNITAIRE
                       };
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerAllDetailTarifFacturation()
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeTarif = context.DETAILTARIFFACTURATION;

                       IEnumerable<object> query =
                       from x in _LeTarif
                       select new
                       {
                           x.FK_IDTARIFFACTURATION,
                           x.FK_IDREDEVANCE,
                           x.NUMEROTRANCHE,
                           x.QTEANNUELMAXI,
                           x.PRIXUNITAIRE
                       };
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneLienRedevance()
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeLien = context.LIENREDEVANCE ;

                       IEnumerable<object> query =
                       from x in _LeLien
                       select new
                       {
                           x.FK_IDREDEVANCE,
                           x.FK_IDREDEVANCEPARAMETRE,
                           x.DATECREATION,
                           x.DATEMODIFICATION,
                           x.USERCREATION,
                           x.USERMODIFICATION
                       };
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable  RetourneCentreParId( int idCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LeCentre = context.CENTRE ;

                   IEnumerable<object> query =
                   from x in _LeCentre
                   where x.PK_ID  == idCentre
                   select new
                   {
                        x. CODE ,
                        x. LIBELLE ,
                        x. TYPECENTRE ,
                        x. CODESITE ,
                        x. ADRESSE ,
                        x. TELRENSEIGNEMENT ,
                        x. TELDEPANNAGE ,
                        x. NUMEROAUTOCLIENT ,
                        x. GESTIONAUTOAVANCECONSO ,
                        x. GESTIONAUTOFRAIS ,
                        x. NUMEROFACTUREPARCLIENT ,
                        x. DATECREATION ,
                        x.DATEMODIFICATION ,
                        x. USERCREATION ,
                        x. USERMODIFICATION ,
                        x. PK_ID ,
                        x. FK_IDCODESITE ,
                        x. FK_IDTYPECENTRE ,
                        x. FK_IDNIVEAUTARIF,
                        NIVEAUTARIF =  x.NIVEAUTARIF.CODE 

                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneConstitution(int IdRecherche)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LaConstitution = context.CTARCOMP ;

                   IEnumerable<object> query =
                   from x in _LaConstitution
                   where x.FK_IDRECHERCHETARIF == IdRecherche
                   select new
                   {
                       x.FK_IDRECHERCHETARIF,
                       x.ORDRE,
                       x.FK_IDCONTENANTCRITERETARIF,
                       x.DATECREATION,
                       x.DATEMODIFICATION,
                       x.USERCREATION,
                       x.USERMODIFICATION 
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneContenantTarif(int idContenantTarif)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LaContenantTarif = context.CONTENANTCRITERETARIF ;

                   IEnumerable<object> query =
                   from x in _LaContenantTarif
                   where x.PK_ID  == idContenantTarif
                   select new
                   {
                       x.PK_ID,
                       x.TABLEREFERENCE,
                       x.LIBELLE,
                       x.TABLEDONNEES,
                       x.COLONNEDONNEES,
                       x.TAILLE,
                       x.AVECPRODUIT
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       //public static DataTable RetournePagerieClientLot(string pCentre, string pClient, string pProduit, int pPoint, string plotri)
       //{
       //    try
       //    {
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            var _LaPagerie = context.PAGERI;

       //            IEnumerable<object> query =
       //            from x in _LaPagerie
       //            where x.CENTRE == pCentre &&
       //                  x.CLIENT == pClient &&
       //                  x.PRODUIT == pProduit &&
       //                  x.POINT == pPoint &&
       //                  x.LOTRI == plotri
       //            select new
       //            {
       //                x.CENTRE,
       //                x.CLIENT,
       //                x.LOTRI,
       //                x.POINT,
       //                x.PRODUIT,
       //                x.TOURNEE,
       //                x.ORDTOUR,
       //                x.PERIODICITE,
       //                x.PK_ID,
       //                x.STATUT,
       //                x.TOPEDIT
       //            };
       //            return Galatee.Tools.Utility.ListToDataTable(query);
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       //public static DataTable RetournePagerieAFacturer(string pLotri, string pCentre, string pTourne, string pProduit, string pCategorie, string pFrequence)
       //{
       //    try
       //    {
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            string vide = "";
       //            string Centre = string.IsNullOrEmpty(pCentre) ? "" : pCentre;
       //            string Tourne = string.IsNullOrEmpty(pTourne) ? "" : pTourne;
       //            string Produit = string.IsNullOrEmpty(pProduit) ? "" : pProduit;
       //            string Categorie = string.IsNullOrEmpty(pCategorie) ? "" : pCategorie;
       //            string Frequence = string.IsNullOrEmpty(pFrequence) ? "" : pFrequence;

       //            var _LaPagerie = context.PAGERI;
       //            IEnumerable<object> query =
       //            from x in _LaPagerie
       //            where x.LOTRI == pLotri &&
       //                 (x.CENTRE == pCentre || Centre == vide) &&
       //                  (x.TOURNEE == pTourne || Tourne == vide) &&
       //                  (x.PRODUIT == pProduit || Produit == vide) &&
       //                  (x.CATEGORIECLIENT == pCategorie || Categorie == vide) &&
       //                  (x.PERIODICITE == pFrequence || Frequence == vide)
       //            select new
       //            {
                       
       //                x.CENTRE,
       //                x.CLIENT,
       //                x.LOTRI,
       //                x.POINT,
       //                x.PRODUIT,
       //                x.TOURNEE,
       //                x.ORDTOUR,
       //                x.PERIODICITE,
       //                x.PK_ID,
       //                x.STATUT,
       //                x.TOPEDIT
       //            };
       //            return Galatee.Tools.Utility.ListToDataTable(query);
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       //public static DataTable RetourneEvenementNonSaisieFromPagerie(string pLotri, string pCentre, string pTourne, string pProduit, string pCategorie, string pFrequence)
       //{
       //    try
       //    {
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            string vide = "";
       //            string Centre = string.IsNullOrEmpty(pCentre) ? "" : pCentre;
       //            string Tourne = string.IsNullOrEmpty(pTourne) ? "" : pTourne;
       //            string Produit = string.IsNullOrEmpty(pProduit) ? "" : pProduit;
       //            string Categorie = string.IsNullOrEmpty(pCategorie) ? "" : pCategorie;
       //            string Frequence = string.IsNullOrEmpty(pFrequence) ? "" : pFrequence;


       //            var _LaPagerie = context.PAGERI;
       //            IEnumerable<object> query =
       //            from x in _LaPagerie
       //            where x.LOTRI == pLotri &&
       //                 (x.CENTRE == pCentre || Centre == vide) &&
       //                  (x.TOURNEE == pTourne || Tourne == vide) &&
       //                  (x.PRODUIT == pProduit || Produit == vide) &&
       //                  (x.CATEGORIECLIENT == pCategorie || Categorie == vide) &&
       //                  (x.PERIODICITE == pFrequence || Frequence == vide) &&
       //                  (x.LOTRI == pLotri && x.CAS  == "##")

       //            select new
       //            {
       //                x.EVENEMENT.CENTRE,
       //                x.EVENEMENT.CLIENT,
       //                x.EVENEMENT.PRODUIT,
       //                x.EVENEMENT.POINT,
       //                x.EVENEMENT.NUMEVENEMENT,
       //                x.EVENEMENT.ORDRE,
       //                x.EVENEMENT.COMPTEUR,
       //                x.EVENEMENT.DATEEVT,
       //                x.EVENEMENT.PERIODE,
       //                x.EVENEMENT.CODEEVT,
       //                x.EVENEMENT.INDEXEVT,
       //                x.EVENEMENT.CAS,
       //                x.EVENEMENT.ENQUETE,
       //                x.EVENEMENT.CONSO,
       //                x.EVENEMENT.CONSONONFACTUREE,
       //                x.EVENEMENT.LOTRI,
       //                x.EVENEMENT.FACTURE,
       //                x.EVENEMENT.SURFACTURATION,
       //                x.EVENEMENT.STATUS,
       //                x.EVENEMENT.TYPECONSO,
       //                x.EVENEMENT.DIAMETRE,
       //                x.EVENEMENT.TYPETARIF,
       //                x.EVENEMENT.FORFAIT,
       //                x.EVENEMENT.CATEGORIE,
       //                x.EVENEMENT.CODECONSO,
       //                x.EVENEMENT.PROPRIO,
       //                x.EVENEMENT.ETATCOMPTEUR,
       //                x.EVENEMENT.MODEPAIEMENT,
       //                x.EVENEMENT.MATRICULE,
       //                x.EVENEMENT.FACPER,
       //                x.EVENEMENT.QTEAREG,
       //                x.EVENEMENT.DERPERF,
       //                x.EVENEMENT.DERPERFN,
       //                x.EVENEMENT.CONSOFAC,
       //                x.EVENEMENT.REGIMPUTE,
       //                x.EVENEMENT.REGCONSO,
       //                x.EVENEMENT.COEFLECT,
       //                x.EVENEMENT.COEFCOMPTAGE,
       //                x.EVENEMENT.PUISSANCE,
       //                x.EVENEMENT.TYPECOMPTAGE,
       //                x.EVENEMENT.TYPECOMPTEUR,
       //                x.EVENEMENT.COEFK1,
       //                x.EVENEMENT.COEFK2,
       //                x.EVENEMENT.COEFFAC,
       //                x.EVENEMENT.USERCREATION,
       //                x.EVENEMENT.DATECREATION,
       //                x.EVENEMENT.DATEMODIFICATION,
       //                x.EVENEMENT.USERMODIFICATION,
       //                x.EVENEMENT.PK_ID,
       //                x.EVENEMENT.FK_IDCANALISATION,
       //                x.EVENEMENT.FK_IDABON,
       //                x.EVENEMENT.FK_IDCOMPTEUR,
       //                x.EVENEMENT.FK_IDCENTRE,
       //                x.EVENEMENT.FK_IDPRODUIT,
       //                x.EVENEMENT.ESTCONSORELEVEE
       //            };
       //            return Galatee.Tools.Utility.ListToDataTable(query);
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       //public static DataTable RetourneEvenementNonSaisieFromPagisol(string pLotri, int pidCentre, string usercreation)
       //{
       //    try
       //    {
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            int[] LstStatut = StatusEvenement(false);

       //            var _LaPagerie = context.PAGISOL ;
       //            IEnumerable<object> query =
       //            from x in _LaPagerie
       //            where
       //              (x.FK_IDCENTRE  == pidCentre) &&
       //              ((x.LOTRI == pLotri && LstStatut.Contains(x.EVENEMENT.STATUS.Value)))
       //            select new
       //            {
       //                x.EVENEMENT.CENTRE,
       //                x.EVENEMENT.CLIENT,
       //                x.EVENEMENT.PRODUIT,
       //                x.EVENEMENT.POINT,
       //                x.EVENEMENT.NUMEVENEMENT,
       //                x.EVENEMENT.ORDRE,
       //                x.EVENEMENT.COMPTEUR,
       //                x.EVENEMENT.DATEEVT,
       //                x.EVENEMENT.PERIODE,
       //                x.EVENEMENT.CODEEVT,
       //                x.EVENEMENT.INDEXEVT,
       //                x.EVENEMENT.CAS,
       //                x.EVENEMENT.ENQUETE,
       //                x.EVENEMENT.CONSO,
       //                x.EVENEMENT.CONSONONFACTUREE,
       //                x.EVENEMENT.LOTRI,
       //                x.EVENEMENT.FACTURE,
       //                x.EVENEMENT.SURFACTURATION,
       //                x.EVENEMENT.STATUS,
       //                x.EVENEMENT.TYPECONSO,
       //                x.EVENEMENT.REGLAGECOMPTEUR,
       //                x.EVENEMENT.TYPETARIF,
       //                x.EVENEMENT.FORFAIT,
       //                x.EVENEMENT.CATEGORIE,
       //                x.EVENEMENT.CODECONSO,
       //                x.EVENEMENT.PROPRIO,
       //                x.EVENEMENT.MODEPAIEMENT,
       //                x.EVENEMENT.MATRICULE,
       //                x.EVENEMENT.FACPER,
       //                x.EVENEMENT.QTEAREG,
       //                x.EVENEMENT.DERPERF,
       //                x.EVENEMENT.DERPERFN,
       //                x.EVENEMENT.CONSOFAC,
       //                x.EVENEMENT.REGIMPUTE,
       //                x.EVENEMENT.REGCONSO,
       //                x.EVENEMENT.COEFLECT,
       //                x.EVENEMENT.COEFCOMPTAGE,
       //                x.EVENEMENT.PUISSANCE,
       //                x.EVENEMENT.TYPECOMPTAGE,
       //                x.EVENEMENT.TYPECOMPTEUR,
       //                x.EVENEMENT.COEFK1,
       //                x.EVENEMENT.COEFK2,
       //                x.EVENEMENT.COEFFAC,
       //                x.EVENEMENT.USERCREATION,
       //                x.EVENEMENT.DATECREATION,
       //                x.EVENEMENT.DATEMODIFICATION,
       //                x.EVENEMENT.USERMODIFICATION,
       //                x.EVENEMENT.PK_ID,
       //                x.EVENEMENT.FK_IDCANALISATION,
       //                x.EVENEMENT.FK_IDABON,
       //                x.EVENEMENT.FK_IDCOMPTEUR,
       //                x.EVENEMENT.FK_IDCENTRE,
       //                x.EVENEMENT.FK_IDPRODUIT,
       //                x.EVENEMENT.ESTCONSORELEVEE,
       //                x.ISCONSOSEUL 
       //            };
       //            return Galatee.Tools.Utility.ListToDataTable(query);
       //        }
   
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       public static DataTable RetourneEvenementAFacturerFromPageriePartiel(string pLotri, string pCentre, string pTourne, string pProduit, string pCategorie, string pFrequence)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {

                   int[] LstStatut = StatusEvenement(true );

                   var _LaPagerie = context.EVENEMENT ;
                   IEnumerable<object> query =
                   from x in _LaPagerie
                   where x.LOTRI == pLotri
                   &&(x.CENTRE == pCentre || string.IsNullOrEmpty(pCentre))
                   &&(x.TOURNEE == pTourne || string.IsNullOrEmpty(pTourne)) &&
                     (x.PRODUIT == pProduit || string.IsNullOrEmpty(pProduit)) &&
                     (x.CATEGORIE  == pCategorie || string.IsNullOrEmpty(pCategorie)) &&
                     (x.PERFAC  == pFrequence || string.IsNullOrEmpty(pFrequence))&&
                     (x.LOTRI == pLotri )
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
                       x.REGLAGECOMPTEUR,
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
                       x.ESTCONSORELEVEE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneEvenementAFacturerFromPagerieDefinitif(string pLotri, int pidCentre, int? pidTourne, int? pidProduit, int? pidCategorie, string  pFrequence)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   int[] LstStatut = StatusEvenement(false );
                   List<int?> StatusIndex = new List<int?>() { 3, 4, 99 };
                   string categorie = context.CATEGORIECLIENT.FirstOrDefault(t => t.PK_ID == pidCategorie).CODE;
                   var _LeEvent = context.EVENEMENT ;
                   var query =
                   from x in _LeEvent
                   where
                     (x.FK_IDCENTRE == pidCentre)
                   && (x.FK_IDTOURNEE == pidTourne) &&
                     (x.FK_IDPRODUIT == pidProduit) &&
                     (x.CATEGORIE == categorie) &&
                     (x.PERFAC == pFrequence) &&
                     (x.LOTRI == pLotri) &&
                     !StatusIndex.Contains(x.STATUS )
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
                       x.REGLAGECOMPTEUR,
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
                       x.TOURNEE ,
                       x.ORDTOUR ,
                       FK_IDCLIENT = x.ABON.CLIENT1.PK_ID,
                       x.ESTCONSORELEVEE,
                       x.CASPRECEDENTEFACTURE,
                       x.CONSOMOYENNEPRECEDENTEFACTURE,
                       x.DATERELEVEPRECEDENTEFACTURE,
                       x.INDEXPRECEDENTEFACTURE,
                       x.PERIODEPRECEDENTEFACTURE,
                       /**/
                       x.ABON.DRES,
                       x.ABON.PERFAC,
                       x.ABON.DABONNEMENT,
                       x.PUISSANCEINSTALLEE,
                       /**/
                       x.ABON.CLIENT1.DENABON,
                       x.ABON.CLIENT1.NOMABON,
                       x.ABON.CLIENT1.DENMAND,
                       x.ABON.CLIENT1.ADRMAND1,
                       x.ABON.CLIENT1.ADRMAND2,
                       x.ABON.CLIENT1.CPOS,
                       x.ABON.CLIENT1.BUREAU,
                       x.ABON.CLIENT1.BANQUE,
                       x.ABON.CLIENT1.GUICHET,
                       x.ABON.CLIENT1.COMPTE,
                       x.ABON.CLIENT1.REGROUPEMENT,
                       x.ABON.CLIENT1.REGEDIT,
                       x.ABON.CLIENT1.AG.COMMUNE,
                       x.ABON.CLIENT1.AG.QUARTIER,
                       x.ABON.CLIENT1.AG.RUE,
                       x.ABON.CLIENT1.FK_IDCATEGORIE,

                       x.ABON.CLIENT1.ISFACTUREEMAIL,
                       x.ABON.CLIENT1.ISFACTURESMS,
                       x.ABON.CLIENT1.TELEPHONE,
                       x.ABON.CLIENT1.EMAIL,
                       x.ABON.CLIENT1.AG.FK_IDTOURNEE,
                       x.ABON.CLIENT1.AG.FK_IDCOMMUNE,

                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       //public static DataTable RetourneEvenementAFacturerFromPagisol(string pLotri, int pidCentre, string usercreation)
       //{
       //   try
       //    {
       //        using (galadbEntities context = new galadbEntities())
       //        {
       //            int[] LstStatut = StatusEvenement(false);

       //            var _LaPagerie = context.PAGISOL ;
       //            IEnumerable<object> query =
       //            from x in _LaPagerie
       //            where
       //              (x.FK_IDCENTRE  == pidCentre) &&
       //              ((x.LOTRI == pLotri && LstStatut.Contains(x.EVENEMENT.STATUS.Value)))
       //            select new
       //            {
       //                x.EVENEMENT.CENTRE,
       //                x.EVENEMENT.CLIENT,
       //                x.EVENEMENT.PRODUIT,
       //                x.EVENEMENT.POINT,
       //                x.EVENEMENT.NUMEVENEMENT,
       //                x.EVENEMENT.ORDRE,
       //                x.EVENEMENT.COMPTEUR,
       //                x.EVENEMENT.DATEEVT,
       //                x.EVENEMENT.PERIODE,
       //                x.EVENEMENT.CODEEVT,
       //                x.EVENEMENT.INDEXEVT,
       //                x.EVENEMENT.CAS,
       //                x.EVENEMENT.ENQUETE,
       //                x.EVENEMENT.CONSO,
       //                x.EVENEMENT.CONSONONFACTUREE,
       //                x.EVENEMENT.LOTRI,
       //                x.EVENEMENT.FACTURE,
       //                x.EVENEMENT.SURFACTURATION,
       //                x.EVENEMENT.STATUS,
       //                x.EVENEMENT.TYPECONSO,
       //                x.EVENEMENT.REGLAGECOMPTEUR,
       //                x.EVENEMENT.TYPETARIF,
       //                x.EVENEMENT.FORFAIT,
       //                x.EVENEMENT.CATEGORIE,
       //                x.EVENEMENT.CODECONSO,
       //                x.EVENEMENT.PROPRIO,
       //                x.EVENEMENT.MODEPAIEMENT,
       //                x.EVENEMENT.MATRICULE,
       //                x.EVENEMENT.FACPER,
       //                x.EVENEMENT.QTEAREG,
       //                x.EVENEMENT.DERPERF,
       //                x.EVENEMENT.DERPERFN,
       //                x.EVENEMENT.CONSOFAC,
       //                x.EVENEMENT.REGIMPUTE,
       //                x.EVENEMENT.REGCONSO,
       //                x.EVENEMENT.COEFLECT,
       //                x.EVENEMENT.COEFCOMPTAGE,
       //                x.EVENEMENT.PUISSANCE,
       //                x.EVENEMENT.TYPECOMPTAGE,
       //                x.EVENEMENT.TYPECOMPTEUR,
       //                x.EVENEMENT.COEFK1,
       //                x.EVENEMENT.COEFK2,
       //                x.EVENEMENT.COEFFAC,
       //                x.EVENEMENT.USERCREATION,
       //                x.EVENEMENT.DATECREATION,
       //                x.EVENEMENT.DATEMODIFICATION,
       //                x.EVENEMENT.USERMODIFICATION,
       //                x.EVENEMENT.PK_ID,
       //                x.EVENEMENT.FK_IDCANALISATION,
       //                x.EVENEMENT.FK_IDABON,
       //                x.EVENEMENT.FK_IDCOMPTEUR,
       //                x.EVENEMENT.FK_IDCENTRE,
       //                x.EVENEMENT.FK_IDPRODUIT,
       //                x.EVENEMENT.TOURNEE,
       //                x.EVENEMENT.ORDTOUR,
       //                FK_IDCLIENT = x.EVENEMENT.ABON.CLIENT1.PK_ID,
       //                x.EVENEMENT.ESTCONSORELEVEE,
       //                x.EVENEMENT.CASPRECEDENTEFACTURE,
       //                x.EVENEMENT.CONSOMOYENNEPRECEDENTEFACTURE,
       //                x.EVENEMENT.DATERELEVEPRECEDENTEFACTURE,
       //                x.EVENEMENT.INDEXPRECEDENTEFACTURE,
       //                x.EVENEMENT.PERIODEPRECEDENTEFACTURE,
       //                x.EVENEMENT.ABON.DRES,
       //                x.EVENEMENT.ABON.PERFAC,
       //                x.EVENEMENT.ABON.DABONNEMENT,
       //                x.EVENEMENT.PUISSANCEINSTALLEE ,
       //                x.EVENEMENT.ABON.CLIENT1.DENABON,
       //                x.EVENEMENT.ABON.CLIENT1.NOMABON,
       //                x.EVENEMENT.ABON.CLIENT1.DENMAND,
       //                x.EVENEMENT.ABON.CLIENT1.ADRMAND1,
       //                x.EVENEMENT.ABON.CLIENT1.ADRMAND2,
       //                x.EVENEMENT.ABON.CLIENT1.CPOS,
       //                x.EVENEMENT.ABON.CLIENT1.BUREAU,
       //                x.EVENEMENT.ABON.CLIENT1.BANQUE,
       //                x.EVENEMENT.ABON.CLIENT1.GUICHET,
       //                x.EVENEMENT.ABON.CLIENT1.COMPTE,
       //                x.EVENEMENT.ABON.CLIENT1.REGROUPEMENT,
       //                x.EVENEMENT.ABON.CLIENT1.REGEDIT,
       //                x.EVENEMENT.ABON.CLIENT1.AG.COMMUNE,
       //                x.EVENEMENT.ABON.CLIENT1.AG.QUARTIER,
       //                x.EVENEMENT.ABON.CLIENT1.AG.RUE,
       //                x.EVENEMENT.ABON.CLIENT1.FK_IDCATEGORIE,
       //                x.EVENEMENT.ABON.CLIENT1.ISFACTUREEMAIL,
       //                x.EVENEMENT.ABON.CLIENT1.ISFACTURESMS,
       //                x.EVENEMENT.ABON.CLIENT1.TELEPHONE,
       //                x.EVENEMENT.ABON.CLIENT1.EMAIL,
       //                x.EVENEMENT.ABON.CLIENT1.AG.FK_IDTOURNEE,
       //                x.EVENEMENT.ABON.CLIENT1.AG.FK_IDCOMMUNE,
       //            };
       //            return Galatee.Tools.Utility.ListToDataTable(query);
       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}
       public static DataTable RetourneEvenementADefacturerFacturerFromPagerie(CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<int?> StatusIndex = new List<int?>() { 3 };

                   var _LeEvent = context.EVENEMENT;
                   var query =
                   from x in _LeEvent
                   where
                     (x.FK_IDCENTRE == leLot.FK_IDCENTRE )
                   && (x.FK_IDTOURNEE == leLot.FK_IDTOURNEE  ) &&
                     (x.FK_IDPRODUIT == leLot.FK_IDPRODUIT) &&
                     (x.CATEGORIE == leLot.CATEGORIECLIENT) &&
                     (x.PERFAC == leLot.PERIODICITE) &&
                     (x.LOTRI == leLot.NUMLOTRI )
                     && StatusIndex.Contains(x.STATUS.Value)
                   select new
                   {
                        x. CENTRE ,
                        x. CLIENT ,
                        x. PRODUIT ,
                        x.  POINT ,
                        x.  NUMEVENEMENT ,
                        x. ORDRE ,
                        x. COMPTEUR ,
                        x. DATEEVT ,
                        x. PERIODE ,
                        x. CODEEVT ,
                        x. INDEXEVT ,
                        x. CAS ,
                        x. ENQUETE ,
                        x. CONSO ,
                        x. CONSONONFACTUREE ,
                        x. LOTRI ,
                        x. FACTURE ,
                        x. SURFACTURATION ,
                        x. STATUS ,
                        x. TYPECONSO ,
                        x. REGLAGECOMPTEUR ,
                        x. TYPETARIF ,
                        x. FORFAIT ,
                        x. CATEGORIE ,
                        x. CODECONSO ,
                        x. PROPRIO ,
                        x. STATUTCOMPTEUR ,
                        x. MODEPAIEMENT ,
                        x. MATRICULE ,
                        x. FACPER ,
                        x. QTEAREG ,
                        x. DERPERF ,
                        x. DERPERFN ,
                        x. CONSOFAC ,
                        x. REGIMPUTE ,
                        x. REGCONSO ,
                        x. COEFLECT ,
                        x. COEFCOMPTAGE ,
                        x. PUISSANCE ,
                        x. TYPECOMPTAGE ,
                        x. TYPECOMPTEUR ,
                        x. COEFK1 ,
                        x. COEFK2 ,
                        x. COEFKR1 ,
                        x. COEFKR2 ,
                        x. COEFFAC ,
                        x. USERCREATION ,
                        x.  DATECREATION ,
                        x. DATEMODIFICATION ,
                        x. USERMODIFICATION ,
                        x.  PK_ID ,
                        x.  FK_IDCANALISATION ,
                        x.  FK_IDABON ,
                        x.  FK_IDCOMPTEUR ,
                        x.  FK_IDCENTRE ,
                        x.  FK_IDPRODUIT ,
                        x. ESTCONSORELEVEE ,
                        x. FK_IDTOURNEE ,
                        x. TOURNEE ,
                        x. ORDTOUR ,
                        x. PERFAC ,
                        x. CONSOMOYENNEPRECEDENTEFACTURE ,
                        x. DATERELEVEPRECEDENTEFACTURE ,
                        x. CASPRECEDENTEFACTURE ,
                        x. INDEXPRECEDENTEFACTURE ,
                        x. PERIODEPRECEDENTEFACTURE ,
                        x. ORDREAFFICHAGE ,
                        x. NOUVEAUCOMPTEUR ,
                        x. PUISSANCEINSTALLEE ,
                        x. QTEAREGPRECEDENT ,
                        x. ISCONSOSEULE ,
                        x. ISEXONERETVA ,
                        x. DEBUTEXONERATIONTVA ,
                        x. FINEXONERATIONTVA ,
                        x. COMMENTAIRE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       
       public static DataTable RetourneEvenementADefacturerFacturerIsole(CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   int[] LstStatut = StatusEvenement(false );

                   var _LePagisol = context.EVENEMENT ;
                   IEnumerable<object> query =
                   from x in _LePagisol
                   where x.LOTRI == leLot.NUMLOTRI  &&
                        (x.CENTRE == leLot.CENTRE ) &&
                        (x.USERCREATION == leLot.USERCREATION ) &&
                        !LstStatut.Contains(x.STATUS.Value )
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
                       x.REGLAGECOMPTEUR,
                       x.TYPETARIF,
                       x.FORFAIT,
                       x.CATEGORIE,
                       x.CODECONSO,
                       x.PROPRIO,
                       x.STATUTCOMPTEUR,
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
                       x.COEFKR1,
                       x.COEFKR2,
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
                       x.PUISSANCEINSTALLEE,
                       x.QTEAREGPRECEDENT,
                       x.ISCONSOSEULE,
                       x.ISEXONERETVA,
                       x.DEBUTEXONERATIONTVA,
                       x.FINEXONERATIONTVA,
                       x.COMMENTAIRE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneEvenementMiseAJours(CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<int?> StatusIndex = new List<int?>() { 4 };

                   var _LeEvent = context.EVENEMENT;
                   var query =
                   from x in _LeEvent
                   where
                     (x.FK_IDCENTRE == leLot.FK_IDCENTRE)
                   && (x.FK_IDTOURNEE == leLot.FK_IDTOURNEE) &&
                     (x.FK_IDPRODUIT == leLot.FK_IDPRODUIT) &&
                     (x.CATEGORIE == leLot.CATEGORIECLIENT) &&
                     (x.PERFAC == leLot.PERIODICITE) &&
                     (x.LOTRI == leLot.NUMLOTRI)
                     && StatusIndex.Contains(x.STATUS.Value)
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
                       x.REGLAGECOMPTEUR,
                       x.TYPETARIF,
                       x.FORFAIT,
                       x.CATEGORIE,
                       x.CODECONSO,
                       x.PROPRIO,
                       x.STATUTCOMPTEUR,
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
                       x.COEFKR1,
                       x.COEFKR2,
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
                       x.PUISSANCEINSTALLEE,
                       x.QTEAREGPRECEDENT,
                       x.ISCONSOSEULE,
                       x.ISEXONERETVA,
                       x.DEBUTEXONERATIONTVA,
                       x.FINEXONERATIONTVA,
                       x.COMMENTAIRE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneEvenementMiseAJourIsole(CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   List<int?> StatusIndex = new List<int?>() { 4 };

                   var _LePagisol = context.EVENEMENT;
                   IEnumerable<object> query =
                   from x in _LePagisol
                   where x.LOTRI == leLot.NUMLOTRI &&
                        (x.CENTRE == leLot.CENTRE) &&
                        (x.USERCREATION == leLot.USERCREATION) &&
                        StatusIndex.Contains(x.STATUS.Value)
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
                       x.REGLAGECOMPTEUR,
                       x.TYPETARIF,
                       x.FORFAIT,
                       x.CATEGORIE,
                       x.CODECONSO,
                       x.PROPRIO,
                       x.STATUTCOMPTEUR,
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
                       x.COEFKR1,
                       x.COEFKR2,
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
                       x.PUISSANCEINSTALLEE,
                       x.QTEAREGPRECEDENT,
                       x.ISCONSOSEULE,
                       x.ISEXONERETVA,
                       x.DEBUTEXONERATIONTVA,
                       x.FINEXONERATIONTVA,
                       x.COMMENTAIRE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static int[] StatusEvenement(bool IsPartiel)
       {

           if (!IsPartiel)
           {
               int[] strArray =
                {
                  Enumere.EvenementCree ,  Enumere.EvenementReleve ,Enumere.EvenementDefacture,Enumere.EvenementRejeter
                };
               return strArray;
           }
           else
           {
               int[] strArray =
                {
                    Enumere.EvenementReleve ,Enumere.EvenementDefacture,Enumere.EvenementRejeter
                };
               return strArray;
           }

       }
       public static AG  RetourneInfoAg(string centre, string client, string ordre,galadbEntities pcontext)
       {
           return  pcontext.AG.FirstOrDefault(t => t.CENTRE == centre && t.CLIENT == client); ;
       }
       public static CLIENT   RetourneInfoClient(string centre, string client, string ordre,galadbEntities pcontext)
       {
           return pcontext.CLIENT.FirstOrDefault(t => t.CENTRE == centre && t.REFCLIENT  == client && t.ORDRE == ordre ); ;

       }
       public static DataTable RetourneEvenementAFacture(string pCentre, string pClient, int pPoint, string pProduit, string pLotri)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   int[] LstStatut = StatusEvenement(false );
                   IEnumerable<object> query = null;
                   var _EVENEMENT = context.EVENEMENT;
                   query =
                   from x in _EVENEMENT
                   where x.CENTRE == pCentre &&
                         x.CLIENT == pClient &&
                         x.POINT == pPoint &&
                         (x.PRODUIT == pProduit) &&
                         (x.LOTRI == pLotri || LstStatut.Contains(x.STATUS.Value))
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
                       x.REGLAGECOMPTEUR,
                       x.TYPETARIF,
                       x.FORFAIT,
                       x.CATEGORIE,
                       x.CODECONSO,
                       x.PROPRIO,
                       x.STATUTCOMPTEUR,
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
                       x.COEFKR1,
                       x.COEFKR2,
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
                       x.PUISSANCEINSTALLEE,
                       x.QTEAREGPRECEDENT,
                       x.ISCONSOSEULE,
                       x.ISEXONERETVA,
                       x.DEBUTEXONERATIONTVA,
                       x.FINEXONERATIONTVA,
                       x.COMMENTAIRE
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static List<EVENEMENT> RecupererEvenements(int? fk_idEvt,galadbEntities pcontext)
       {
           try
           {
               List<EVENEMENT> _lst = pcontext.EVENEMENT.Where(p => p.PK_ID == fk_idEvt).ToList();
               return _lst;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static List<ENTFAC> ListeFactureADefacturerGeneral(CsLotri _LeLotri, galadbEntities leContext)
       {
           try
           {
             
                   List<ENTFAC>  _LstEntfact = leContext.ENTFAC.Where(y=>y.CENTRE == _LeLotri.CENTRE &&
                                                                         y.LOTRI == _LeLotri.NUMLOTRI &&
                                                                         y.FK_IDCATEGORIECLIENT == _LeLotri.FK_IDCATEGORIECLIENT &&
                                                                         y.FK_IDTOURNEE  == _LeLotri.FK_IDTOURNEE ).ToList();
                   return _LstEntfact;
               
              
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static List<ENTFAC> ListeFactureADefacturerIsole(CsLotri _LeLotri, galadbEntities leContext)
       {
           try
           {

               List<ENTFAC> _LstEntfact = leContext.ENTFAC.Where(y => y.CENTRE == _LeLotri.CENTRE &&
                                                                     y.LOTRI == _LeLotri.NUMLOTRI &&
                                                                     y.USERCREATION  == _LeLotri.USERCREATION ).ToList();
               return _LstEntfact;


           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneProfact(CsLotri _LeLotri)
       {
               try
               {
                   using(galadbEntities leContext = new galadbEntities() )
	                {
                       var _LstEntfact = leContext.ENTFAC;
                       IEnumerable<object> query =
                       from _LeEntfact in _LstEntfact
                       from y in _LeEntfact.PROFAC 
                       where
                            (_LeEntfact.CENTRE == _LeLotri.CENTRE) &&
                            (_LeEntfact.LOTRI == _LeLotri.NUMLOTRI) &&
                            (_LeEntfact.FK_IDCATEGORIECLIENT == _LeLotri.FK_IDCATEGORIECLIENT) &&
                            (_LeEntfact.TOURNEE == _LeLotri.TOURNEE) 
                        select new 
                        {
                            y.LOTRI,
                            y.JET,
                            y.TOURNEE,
                            y.ORDTOUR,
                            y.CENTRE,
                            y.CLIENT,
                            y.ORDRE,
                            y.FACTURE,
                            y.PRODUIT,
                            y.COMPTEUR,
                            y.REGLAGECOMPTEUR,
                            y.COEFLECT,
                            y.POINT,
                            y.PUISSANCE,
                            y.DERPERF,
                            y.DERPERFN,
                            y.REGCONSO,
                            y.REGFAC,
                            y.TFAC,
                            y.LIENRED,
                            y.CONSOFAC,
                            //y.DATEEVT,
                            y.PERIODE,
                            y.AINDEX,
                            y.NINDEX,
                            y.CAS,
                            y.CONSO,
                            y.TOTPROHT,
                            y.TOTPROTAX,
                            y.TOTPROTTC,
                            y.ADERPERF,
                            y.ADERPERFN,
                            y.REGIMPUTE,
                            y.TYPECOMPTEUR,
                            y.REGROU,
                            y.DEVPRE,
                            y.NBREDTOT,
                            y.STATUS,
                            y.LIENFAC,
                            y.EVENEMENT,
                            y.TOPMAJ,
                            y.TOPANNUL,
                            y.PUISSANCEINSTALLEE,
                            y.COEFCOMPTAGE,
                            y.BRANCHEMENT,
                            y.COEFK1,
                            y.COEFK2,
                            y.PERTESACTIVES,
                            y.PERTESREACTIVES,
                            y.COEFFAC,
                            y.PK_ID,
                            y.FK_IDENTFAC,
                            y.FK_IDEVENEMENT,
                            y.DATECREATION,
                            y.DATEMODIFICATION,
                            y.USERCREATION,
                            y.USERMODIFICATION,
                            y.FK_IDPRODUIT,
                            y.FK_IDCENTRE
                        };
                        return Galatee.Tools.Utility.ListToDataTable(query);


	                }
                  
                        //select _LeEntfact.PROFAC).ToList();
               }
               catch (Exception ex)
               {
                   throw ex;
               }
       }
       public static DataTable RetourneRedfact(CsLotri _LeLotri)
       {
           try
           {
               using (galadbEntities leContext = new galadbEntities())
               {
                   //var _LstEntfact = leContext.ENTFAC.Where(y =>
                   //                                (y.CENTRE == _LeLotri.CENTRE) &&
                   //                                (y.LOTRI == _LeLotri.NUMLOTRI) &&
                   //                                (y.FK_IDCATCLI == _LeLotri.FK_IDCATEGOCLI) &&
                   //                                (y.TOURNEE == _LeLotri.TOURNEE)).ToList();

                   var _LstEntfact = leContext.ENTFAC;
                   IEnumerable<object> query =
                       from x in _LstEntfact
                       from y in x.REDFAC  
                        where
                        (x.CENTRE == _LeLotri.CENTRE) &&
                        (x.LOTRI == _LeLotri.NUMLOTRI) &&
                        (x.FK_IDCATEGORIECLIENT == _LeLotri.FK_IDCATEGORIECLIENT) &&
                        (x.TOURNEE == _LeLotri.TOURNEE)
                        select new 
                        {
                                y.LOTRI ,
                                y. JET ,
                                y. CENTRE ,
                                y. CLIENT ,
                                y. FACTURE ,
                                y. LIENRED ,
                                y. REDEVANCE ,
                                y. TRANCHE ,
                                y. ORDRED ,
                                y. QUANTITE ,
                                y. UNITE ,
                                y.  BARPRIX ,
                                y.  TAXE ,
                                y. CTAX ,
                                y. DAPP ,
                                y. CRITERE ,
                                y. VARIANTE ,
                                y.  TOTREDHT ,
                                y.  TOTREDTAX ,
                                y.  TOTREDTTC ,
                                y. PARAM1 ,
                                y. PARAM2 ,
                                y. PARAM3 ,
                                y. PARAM4 ,
                                y. PARAM5 ,
                                y. PARAM6 ,
                                y. NBJOUR ,
                                y. DEBUTAPPLICATION ,
                                y. FINAPPLICATION ,
                                y. LIENFAC ,
                                y. TOPMAJ ,
                                y. PERIODE ,
                                y. PRODUIT ,
                                y. FORMULE ,
                                y. TOPANNUL ,
                                y. BARBORNEDEBUT ,
                                y. BARBORNEFIN ,
                                y. PK_ID ,
                                y. FK_IDENTFAC ,
                                y. DATECREATION ,
                                y. DATEMODIFICATION ,
                                y. USERCREATION ,
                                y. USERMODIFICATION ,
                                y. FK_IDPRODUIT ,
                                y. FK_IDCENTRE
                        };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static CsStatFacturation ValiderDemandeDefacturation(CsLotri _LeLot, galadbEntities pContext)
       {
           try
           {
                   CsStatFacturation _laStat = new CsStatFacturation();
                   LOTRI _LeLotri = Entities.ConvertObject<LOTRI, CsLotri>(_LeLot);
                   Entities.UpdateEntity(_LeLotri, pContext);
                   return _laStat;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static CsStatFacturation ValiderDefacturation(List<EVENEMENT> _LstEvtDefact, List<ENTFAC> _LstFacture, CsLotri _LeLot, string Action)
       {
           try
           {
               using (galadbEntities pContext = new galadbEntities())
               {
                   CsStatFacturation _laStat = new CsStatFacturation();
                   LOTRI _LeLotri = Entities.ConvertObject<LOTRI, CsLotri>(_LeLot);
                   if (!string.IsNullOrEmpty(_LeLotri.NUMLOTRI))
                   { _LeLotri.ETATFAC5 = null; _LeLotri.DFAC = null; }
                   foreach (EVENEMENT item in _LstEvtDefact)
                   {
                       item.DERPERF = null;
                       item.DERPERFN = null;
                       item.CONSOFAC = null;
                       if (Action == Enumere.Defacturation)
                           item.STATUS = Enumere.EvenementDefacture;
                       else if (Action == Enumere.DestructionSimulation)
                           item.STATUS = 1;
                   }
                   Entities.DeleteEntity(_LstFacture, pContext);
                   Entities.UpdateEntity(_LstEvtDefact, pContext);
                   Entities.UpdateEntity(_LeLotri, pContext);
                   _laStat.Montant = _LstFacture.Sum(p => p.TOTFTTC);
                   _laStat.NombreCalcule = _LstFacture.Count();
                   pContext.SaveChanges();
                   return _laStat;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static CsStatFacturation ValiderDefacturation(List<EVENEMENT> _LstEvtDefact, List<ENTFAC> _LstFacture, List<PROFAC > _LstFactureProf, List<REDFAC> _LstFactureRed, CsLotri _LeLot,List<ANOMALIEFACTURATION> lstAnofac, string Action)
       {
           try
           {
               using (galadbEntities pContext = new galadbEntities())
               {
                   CsStatFacturation _laStat = new CsStatFacturation();
                   LOTRI _LeLotri = Entities.ConvertObject<LOTRI, CsLotri>(_LeLot);
                   if (!string.IsNullOrEmpty(_LeLotri.NUMLOTRI))
                   { _LeLotri.ETATFAC5 = null; _LeLotri.DFAC = null; _LeLotri.ETATFAC3 = null; }
                   foreach (EVENEMENT item in _LstEvtDefact)
                   {
                       item.DERPERF = null;
                       item.DERPERFN = null;
                       item.CONSOFAC = null;
                       if (Action == Enumere.Defacturation)
                           item.STATUS = Enumere.EvenementDefacture;
                       else if (Action == Enumere.DestructionSimulation)
                           item.STATUS = 1;
                   }
                   if (_LeLotri.JET == "01")
                       _LeLotri.JET = null;
                   else if (!string.IsNullOrEmpty( _LeLotri.JET) && _LeLotri.JET != "01" )
                       _LeLotri.JET = (int.Parse(_LeLotri.JET) - 1).ToString("00");

                   Entities.DeleteEntity(_LstFactureRed, pContext);
                   Entities.DeleteEntity(_LstFactureProf, pContext);
                   Entities.DeleteEntity(_LstFacture, pContext);
                   Entities.UpdateEntity(_LstEvtDefact, pContext);
                   Entities.UpdateEntity(_LeLotri, pContext);
                   Entities.DeleteEntity (lstAnofac, pContext);
                   
                   _laStat.Montant = _LstFacture.Sum(p => p.TOTFTTC);
                   _laStat.NombreCalcule = _LstFacture.Count();
                   pContext.SaveChanges();
                   return _laStat;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneFactureClient(int pk_IdClient)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _lstLclient = context.LCLIENT;
                   IEnumerable<object> query = from l in _lstLclient
                                               where
                                                     l.DC == Enumere.Debit &&
                                                     l.COPER != Enumere.CoperRNA &&
                                                     l.FK_IDCLIENT == pk_IdClient
                                               select new
                                               {
                                                   l.PK_ID,
                                                   l.CENTRE,
                                                   l.CLIENT,
                                                   l.ORDRE,
                                                   l.REFEM,
                                                   l.NDOC,
                                                   l.COPER,
                                                   l.DENR,
                                                   l.EXIG,
                                                   l.MONTANT,
                                                   l.CAPUR,
                                                   l.CRET,
                                                   l.MODEREG,
                                                   l.DC,
                                                   l.ORIGINE,
                                                   l.CAISSE,
                                                   l.ECART,
                                                   l.MOISCOMPT,
                                                   l.TOP1,
                                                   l.EXIGIBILITE,
                                                   l.FRAISDERETARD,
                                                   l.REFERENCEPUPITRE,
                                                   l.IDLOT,
                                                   l.DATEVALEUR,
                                                   l.REFERENCE,
                                                   l.REFEMNDOC,
                                                   l.ACQUIT,
                                                   l.MATRICULE,
                                                   l.TAXESADEDUIRE,
                                                   l.DATEFLAG,
                                                   l.MONTANTTVA,
                                                   l.IDCOUPURE,
                                                   l.AGENT_COUPURE,
                                                   l.RDV_COUPURE,
                                                   l.NUMCHEQ,
                                                   l.OBSERVATION_COUPURE,
                                                   l.USERCREATION,
                                                   l.DATECREATION,
                                                   l.DATEMODIFICATION,
                                                   l.USERMODIFICATION,
                                                   l.BANQUE,
                                                   l.GUICHET,
                                                   l.FK_IDCENTRE,
                                                   l.FK_IDADMUTILISATEUR,
                                                   l.FK_IDCOPER,
                                                   l.FK_IDLIBELLETOP,
                                                   l.FK_IDCLIENT,
                                                   l.POSTE,
                                                   l.FK_IDPOSTE

                                               };
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               };
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       public static DataTable ChargerLotriDejaFacture(List<int> lstCentreHabil,string matricule)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                               join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI } 
                                               where lstCentreHabil.Contains(t.FK_IDCENTRE) && !string.IsNullOrEmpty( y.TOPMAJ) 
                                               select new
                                               {
                                                   t.NUMLOTRI,
                                                   y.JET,
                                                   t.PERIODE,
                                                   t.CENTRE,
                                                   t.PRODUIT,
                                                   t.CATEGORIECLIENT,
                                                   t.PERIODICITE,
                                                   t.EXIG,
                                                   t.DFAC,
                                                   t.ETATFAC1,
                                                   t.ETATFAC2,
                                                   t.ETATFAC3,
                                                   t.ETATFAC4,
                                                   t.ETATFAC5,
                                                   t.ETATFAC6,
                                                   t.ETATFAC7,
                                                   t.ETATFAC8,
                                                   t.ETATFAC9,
                                                   t.ETATFAC10,
                                                   t.TOURNEE,
                                                   t.RELEVEUR,
                                                   t.BASE,
                                                   t.DATECREATION,
                                                   t.DATEMODIFICATION,
                                                   t.USERCREATION,
                                                   t.USERMODIFICATION,
                                                   t.FK_IDPRODUIT,
                                                   t.FK_IDCATEGORIECLIENT,
                                                   t.FK_IDCENTRE,
                                                   t.FK_IDRELEVEUR,
                                                   t.FK_IDTOURNEE,
                                                   LIBELLECENTRE = t.CENTRE1.LIBELLE ,
                                                   NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE
                                               }).Distinct();
                   IEnumerable<object> query1 = (from t in context.LOTRI
                                                join y in context.ENTFAC on new { t.FK_IDCENTRE, t.NUMLOTRI }
                                                 equals new { FK_IDCENTRE = y.FK_IDCENTRE, NUMLOTRI = y.LOTRI }
                                                where lstCentreHabil.Contains(t.FK_IDCENTRE) && !string.IsNullOrEmpty(y.TOPMAJ) &&
                                                (new string[] {"00002", "00001", "00003" }.Contains(t.NUMLOTRI.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre)))) 
                                                && (y.USERCREATION == matricule || y.USERMODIFICATION == matricule)

                                                select new
                                                {
                                                    t.NUMLOTRI,
                                                    y.JET,
                                                    t.PERIODE,
                                                    t.CENTRE,
                                                    t.PRODUIT,
                                                    t.CATEGORIECLIENT,
                                                    t.PERIODICITE,
                                                    t.EXIG,
                                                    t.DFAC,
                                                    t.ETATFAC1,
                                                    t.ETATFAC2,
                                                    t.ETATFAC3,
                                                    t.ETATFAC4,
                                                    t.ETATFAC5,
                                                    t.ETATFAC6,
                                                    t.ETATFAC7,
                                                    t.ETATFAC8,
                                                    t.ETATFAC9,
                                                    t.ETATFAC10,
                                                    t.TOURNEE,
                                                    t.RELEVEUR,
                                                    t.BASE,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERCREATION,
                                                    t.USERMODIFICATION,
                                                    t.FK_IDPRODUIT,
                                                    t.FK_IDCATEGORIECLIENT,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDRELEVEUR,
                                                    t.FK_IDTOURNEE,
                                                    LIBELLECENTRE = t.CENTRE1.LIBELLE,
                                                    NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                                }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerLotriDejaMiseAJour(List<int> lstCentreHabil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                               join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI } 
                                               where lstCentreHabil.Contains(t.FK_IDCENTRE) && y.TOPMAJ == "1"
                                               select new
                                               {
                                                   t.NUMLOTRI,
                                                   y.JET,
                                                   y.FACTURE,
                                                   t.PERIODE,
                                                   t.CENTRE,
                                                   t.PRODUIT,
                                                   t.CATEGORIECLIENT,
                                                   t.PERIODICITE,
                                                   t.EXIG,
                                                   t.DFAC,
                                                   t.ETATFAC1,
                                                   t.ETATFAC2,
                                                   t.ETATFAC3,
                                                   t.ETATFAC4,
                                                   t.ETATFAC5,
                                                   t.ETATFAC6,
                                                   t.ETATFAC7,
                                                   t.ETATFAC8,
                                                   t.ETATFAC9,
                                                   t.ETATFAC10,
                                                   t.TOURNEE,
                                                   t.RELEVEUR,
                                                   t.BASE,
                                                   t.PK_ID,
                                                   t.DATECREATION,
                                                   t.DATEMODIFICATION,
                                                   t.USERCREATION,
                                                   t.USERMODIFICATION,
                                                   t.FK_IDPRODUIT,
                                                   t.FK_IDCATEGORIECLIENT,
                                                   t.FK_IDCENTRE,
                                                   t.FK_IDRELEVEUR,
                                                   t.FK_IDTOURNEE,
                                                   LIBELLECENTRE = t.CENTRE1.LIBELLE ,
                                                   NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerLotriPourCalcul(List<int> lstCentreHabil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                                join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                 equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI } into _pTemp
                                                from p in _pTemp.DefaultIfEmpty()
                                                where lstCentreHabil.Contains(t.FK_IDCENTRE)
                                                select new
                                                {
                                                    t.NUMLOTRI,
                                                    p.JET,
                                                    t.PERIODE,
                                                    t.CENTRE,
                                                    t.PRODUIT,
                                                    t.CATEGORIECLIENT,
                                                    t.PERIODICITE,
                                                    t.EXIG,
                                                    t.DFAC,
                                                    t.ETATFAC1,
                                                    t.ETATFAC2,
                                                    t.ETATFAC3,
                                                    t.ETATFAC4,
                                                    t.ETATFAC5,
                                                    t.ETATFAC6,
                                                    t.ETATFAC7,
                                                    t.ETATFAC8,
                                                    t.ETATFAC9,
                                                    t.ETATFAC10,
                                                    t.TOURNEE,
                                                    t.RELEVEUR,
                                                    t.BASE,
                                                    t.PK_ID ,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERCREATION,
                                                    t.USERMODIFICATION,
                                                    t.FK_IDPRODUIT,
                                                    t.FK_IDCATEGORIECLIENT,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDRELEVEUR,
                                                    t.FK_IDTOURNEE,
                                                    LIBELLECENTRE = t.CENTRE1.LIBELLE,
                                                    NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                                }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerLotri(List<int> lstCentreHabil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                               join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI } into _pTemp
                                               from p in _pTemp.DefaultIfEmpty()
                                               where lstCentreHabil.Contains(t.FK_IDCENTRE)
                                               select new
                                               {
                                                   t.NUMLOTRI,
                                                   p.JET,
                                                   t.PERIODE,
                                                   t.CENTRE,
                                                   t.PRODUIT,
                                                   t.CATEGORIECLIENT,
                                                   t.PERIODICITE,
                                                   t.EXIG,
                                                   t.DFAC,
                                                   t.ETATFAC1,
                                                   t.ETATFAC2,
                                                   t.ETATFAC3,
                                                   t.ETATFAC4,
                                                   t.ETATFAC5,
                                                   t.ETATFAC6,
                                                   t.ETATFAC7,
                                                   t.ETATFAC8,
                                                   t.ETATFAC9,
                                                   t.ETATFAC10,
                                                   t.TOURNEE,
                                                   t.RELEVEUR,
                                                   t.BASE,
                                                   t.PK_ID,
                                                   t.DATECREATION,
                                                   t.DATEMODIFICATION,
                                                   t.USERCREATION,
                                                   t.USERMODIFICATION,
                                                   t.FK_IDPRODUIT,
                                                   t.FK_IDCATEGORIECLIENT,
                                                   t.FK_IDCENTRE,
                                                   t.FK_IDRELEVEUR,
                                                   t.FK_IDTOURNEE,
                                                   LIBELLECENTRE = t.CENTRE1.LIBELLE ,
                                                   NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable ChargerLotriFacturation(List<int> lstCentreHabil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                                join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                 equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI } into _pTemp
                                                from p in _pTemp.DefaultIfEmpty()
                                                where lstCentreHabil.Contains(t.FK_IDCENTRE) 
                                                && (context.EVENEMENT.Any(u => u.FK_IDCENTRE == t.FK_IDCENTRE && u.LOTRI == t.NUMLOTRI && (new int[] { Enumere.EvenementReleve, Enumere.EvenementDefacture, Enumere.EvenementRejeter }).Contains(u.STATUS.Value))) 
                                                select new
                                                {
                                                    t.NUMLOTRI,
                                                    p.JET,
                                                    t.PERIODE,
                                                    t.CENTRE,
                                                    t.PRODUIT,
                                                    t.CATEGORIECLIENT,
                                                    t.PERIODICITE,
                                                    t.EXIG,
                                                    t.DFAC,
                                                    t.ETATFAC1,
                                                    t.ETATFAC2,
                                                    t.ETATFAC3,
                                                    t.ETATFAC4,
                                                    t.ETATFAC5,
                                                    t.ETATFAC6,
                                                    t.ETATFAC7,
                                                    t.ETATFAC8,
                                                    t.ETATFAC9,
                                                    t.ETATFAC10,
                                                    t.TOURNEE,
                                                    t.RELEVEUR,
                                                    t.BASE,
                                                    t.PK_ID,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERCREATION,
                                                    t.USERMODIFICATION,
                                                    t.FK_IDPRODUIT,
                                                    t.FK_IDCATEGORIECLIENT,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDRELEVEUR,
                                                    t.FK_IDTOURNEE,
                                                    LIBELLECENTRE = t.CENTRE1.LIBELLE,
                                                    NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                                }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable ChargerLotriDefacturation(List<int> lstCentreHabil)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query = (from t in context.LOTRI
                                                join y in context.ENTFAC on new { t.FK_IDCENTRE, t.PERIODE, t.NUMLOTRI }
                                                 equals new { FK_IDCENTRE = y.FK_IDCENTRE, PERIODE = y.PERIODE, NUMLOTRI = y.LOTRI }
                                                //into _pTemp
                                                //from p in _pTemp.DefaultIfEmpty()
                                                where lstCentreHabil.Contains(t.FK_IDCENTRE)
                                                select new
                                                {
                                                    t.NUMLOTRI,
                                                    y.JET,
                                                    t.PERIODE,
                                                    t.CENTRE,
                                                    t.PRODUIT,
                                                    t.CATEGORIECLIENT,
                                                    t.PERIODICITE,
                                                    t.EXIG,
                                                    t.DFAC,
                                                    t.ETATFAC1,
                                                    t.ETATFAC2,
                                                    t.ETATFAC3,
                                                    t.ETATFAC4,
                                                    t.ETATFAC5,
                                                    t.ETATFAC6,
                                                    t.ETATFAC7,
                                                    t.ETATFAC8,
                                                    t.ETATFAC9,
                                                    t.ETATFAC10,
                                                    t.TOURNEE,
                                                    t.RELEVEUR,
                                                    t.BASE,
                                                    t.PK_ID,
                                                    t.DATECREATION,
                                                    t.DATEMODIFICATION,
                                                    t.USERCREATION,
                                                    t.USERMODIFICATION,
                                                    t.FK_IDPRODUIT,
                                                    t.FK_IDCATEGORIECLIENT,
                                                    t.FK_IDCENTRE,
                                                    t.FK_IDRELEVEUR,
                                                    t.FK_IDTOURNEE,
                                                    LIBELLECENTRE = t.CENTRE1.LIBELLE,
                                                    NOMUSER = context.ADMUTILISATEUR.FirstOrDefault(x => x.MATRICULE == t.USERCREATION).LIBELLE

                                                }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       #endregion
       #region Edition

       public static List<String> RetourneListeDePeriodes()
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   string vide = "";

                   var Entfac = context.ENTFAC;
                   IEnumerable<object> query =
                   from x in Entfac

                   select x.PERIODE.Distinct();            
                   
                   return (List<String>)query;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }



       public static List<CsTrancheRedevence> RetourneTranchesRedevance(int idRedevance)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var tranche = context.TRANCHEREDEVANCE;
                   IEnumerable<object> query =
                   from x in tranche
                   where x.FK_IDREDEVANCE == idRedevance
                   select new { x.FK_IDREDEVANCE, x.GRATUIT, x.LIBELLE, x.ORDRE, x.PK_ID, x.REDEVANCE };

                   DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                   return Entities.GetEntityListFromQuery<CsTrancheRedevence>(dt);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       public static List<String> RetourneListeDeJets(string LotRI)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   string vide = "";

                   var Entfac = context.ENTFAC;
                   IEnumerable<object> query =
                   from x in Entfac
                   where x.LOTRI == LotRI
                   select x.JET.Distinct();            
                   
                   return (List<String>)query;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneClientDuneBorne(string centre, string client, string lotRi, string periode)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   string vide = "";

                   var Entfac = context.ENTFAC;
                   IEnumerable<object> query;
                   if (!string.IsNullOrWhiteSpace(lotRi))
                   {
                      query =
                                                   from x in Entfac
                                                   where x.CENTRE==centre &&
                                                   x.CLIENT  ==client && x.LOTRI==lotRi
                                                   select new { x.CLIENT, x.TOURNEE , x.ORDTOUR, x.TOTFHT };                   }
                   else 
                   {
                       if (!string.IsNullOrWhiteSpace(periode))
                       {
                           query =
                                                     from x in Entfac
                                                     where x.CENTRE == centre && x.CLIENT  == client && x.PERIODE  == periode
                                                     select new { x.CLIENT, x.TOURNEE , x.ORDTOUR, x.TOTFHT };
                       }
                       else
                       {
                           query =
                                                     from x in Entfac
                                                     where x.CENTRE == centre && x.CLIENT  == client
                                                     select new { x.CLIENT, x.TOURNEE , x.ORDTOUR, x.TOTFHT };
                       }
                   }

                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }



       public static DataTable RetourneEntfacRegroupement( List<string>  regroupement, List<string> LstPeriode,List<string> Produit)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {
                   var _lstLEntfact = pcontext.ENTFAC;
                   IEnumerable<object> query = (from x in _lstLEntfact
                                               from y in x.PROFAC 
                                               where
                                                  (LstPeriode.Contains(x.PERIODE)) &&
                                                  (regroupement.Contains (x.REGROUPEMENT)) &&
                                                  (Produit.Contains(y.PRODUIT)) 
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
                                                   //x.COMMUNE,
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
                                                   y.PRODUIT ,
                                                   LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                   COMMUNE = x.COMMUNE1.LIBELLE, 
                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneEntfact(CsLotri leLotSelect, string centreTournee,
            string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
            string centreStop, string clientStop, string periodeSelectionne, bool IsLotIsole)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {
               if (!IsLotIsole)
               {
                   var _lstLEntfact = pcontext.ENTFAC;
                   IEnumerable<object> query = (from x in _lstLEntfact
                                               from y in x.PROFAC 
                                               where
                                                  (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI)) &&
                                                  (x.JET == leLotSelect.JET || string.IsNullOrEmpty(leLotSelect.JET)) &&
                                                  (x.CENTRE == leLotSelect.CENTRE || string.IsNullOrEmpty(leLotSelect.CENTRE)) &&
                                                  (x.FK_IDCENTRE == leLotSelect.FK_IDCENTRE || leLotSelect.FK_IDCENTRE == 0) &&
                                                  (string.IsNullOrEmpty(clientReprise) || x.CLIENT   == clientReprise) &&
                                                  (string.IsNullOrEmpty(tourneeDebut) || y.TOURNEE == tourneeDebut)
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
                                                   //x.COMMUNE,
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
                                                   LIBELLECENTRE = x.CENTRE1.LIBELLE, 
                                                   COMMUNE = x.COMMUNE1.LIBELLE, 
                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
               else
               {
                   var _lstLEntfact = pcontext.ENTFAC;
                   IEnumerable<object> query = (from x in _lstLEntfact
                                               from y in x.PROFAC
                                               where
                                                  (x.LOTRI == leLotSelect.NUMLOTRI   ) &&
                                                  (x.JET == leLotSelect.JET  ) &&
                                                  (string.IsNullOrEmpty(clientReprise) || x.CLIENT == clientReprise) &&
                                                  (string.IsNullOrEmpty(tourneeDebut) || y.TOURNEE == tourneeDebut) &&
                                                  (x.USERCREATION == leLotSelect.USERCREATION ||
                                                  x.USERMODIFICATION == leLotSelect.USERMODIFICATION) 
                                                  && string.IsNullOrEmpty( x.TOPMAJ) 
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
                                                   //x.COMMUNE,
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
                                                   LIBELLECENTRE = x.CENTRE1.LIBELLE,
                                                   COMMUNE = x.COMMUNE1.LIBELLE, 

                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
               }
               
    
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneAnnomalieFactures(CsLotri leLotSelect, string centreTournee,
      string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
      string centreStop, string clientStop, string periodeSelectionne, bool IsLotIsole)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {

                   var _lstLEntfact = pcontext.PROFAC ;
                   var _lstAnnomalie = pcontext.ANOMALIEFACTURATION;
                   IEnumerable<object> query = (from x in _lstAnnomalie
                                               join z in _lstAnnomalie on x.FK_IDABON  equals z.FK_IDABON 
                                               where
                                                  (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI))  
                                               select new
                                               {
                                                   z.LOTRI,
                                                   z.CENTRE,
                                                   z.CLIENT,
                                                   z.ORDRE,
                                                   z.COMPTEUR,
                                                   z.CAUSE
                                               }).Distinct();
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneControleFactures(CsLotri leLotSelect)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {

                   var _lstLEntfact = pcontext.PROFAC;
                   var _lstAnnomalie = pcontext.ANOMALIEFACTURATION;
                   IEnumerable<object> query = from x in _lstAnnomalie
                                               join z in _lstAnnomalie on x.FK_IDABON equals z.FK_IDABON
                                               where
                                                    x.LOTRI == leLotSelect.NUMLOTRI 
                                               select new
                                               {
                                                   z.LOTRI,
                                                   z.CENTRE,
                                                   z.CLIENT,
                                                   z.ORDRE,
                                                   z.COMPTEUR,
                                                   z.CAUSE
                                               };
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }








       public static DataTable RetourneRedfactRegroupement(List<string> regroupement, List<string> LstPeriode, List<string> Produit)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {
              
                       var _lstLRedfact = pcontext.REDFAC ;
                       IEnumerable<object> query = from x in _lstLRedfact
                                                   where
                                                         (LstPeriode.Contains(x.PERIODE)) &&
                                                  (regroupement.Contains(x.ENTFAC.REGROUPEMENT)) &&
                                                  (Produit.Contains(x.PRODUIT)) 
                                                   select new
                                                   {
                                                       x.LOTRI,
                                                       x.JET,
                                                       x.CENTRE,
                                                       x.CLIENT,
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
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                   
                   }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneRedfact(CsLotri leLotSelect, string centreTournee,
            string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
            string centreStop, string clientStop,string periodeSelectionne,bool IsLotIsole)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {
                   if (!IsLotIsole)
                   {
                       var _lstLRedfact = pcontext.REDFAC ;
                       IEnumerable<object> query = from x in _lstLRedfact
                                                   where
                                                         (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI)) &&
                                                         (x.JET == leLotSelect.JET || string.IsNullOrEmpty(leLotSelect.JET)) &&
                                                         (x.CENTRE == leLotSelect.CENTRE || string.IsNullOrEmpty(leLotSelect.CENTRE)) &&
                                                         (x.FK_IDCENTRE == leLotSelect.FK_IDCENTRE || leLotSelect.FK_IDCENTRE == 0) 
                                                   select new
                                                   {
                                                       x.LOTRI,
                                                       x.JET,
                                                       x.CENTRE,
                                                       x.CLIENT,
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
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                   }
                   else
                   {
                       var _lstRedfact = pcontext.REDFAC ;
                       IEnumerable<object> query = from x in _lstRedfact
                                                   where
                                                         (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI)) &&
                                                         (x.JET == leLotSelect.JET || string.IsNullOrEmpty(leLotSelect.JET)) &&
                                                         (x.CENTRE == leLotSelect.CENTRE || string.IsNullOrEmpty(leLotSelect.CENTRE)) &&
                                                         (x.FK_IDCENTRE == leLotSelect.FK_IDCENTRE || leLotSelect.FK_IDCENTRE == 0) &&
                                                         (x.USERCREATION == leLotSelect.USERCREATION || x.USERMODIFICATION == leLotSelect.USERMODIFICATION)
                                                          && string.IsNullOrEmpty(x.TOPMAJ)

                                                   select new
                                                   {
                                                       x.LOTRI,
                                                       x.JET,
                                                       x.CENTRE,
                                                       x.CLIENT,
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

       public static DataTable RetourneProfactRegroupement(List<string> regroupement, List<string> LstPeriode, List<string> Produit)
       {
           try
           {
               using (galadbEntities pcontext = new galadbEntities())
               {
                  
                       var _lstLRedfact = pcontext.PROFAC;
                       IEnumerable<object> query = from x in _lstLRedfact
                                                   where
                                                    (LstPeriode.Contains(x.PERIODE)) &&
                                                  (regroupement.Contains(x.ENTFAC.REGROUPEMENT )) &&
                                                  (Produit.Contains(x.PRODUIT)) 
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
                                                       //x.DATEEVT,
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
                                                       LIBELLEPRODUIT = x.PRODUIT1.LIBELLE,
                                                       x.FK_IDCENTRE,
                                                       x.TYPECOMPTAGE,
                                                       x.DATEEVT 
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                 
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }



       public static DataTable RetourneProfact(CsLotri leLotSelect, string centreTournee,string tourneeDebut, string tourneeFin,
                                               string centreReprise, string clientReprise,string centreStop, string clientStop, 
                                               string periodeSelectionne,bool IsLotIsole)
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
                                                         (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI)) &&
                                                         (x.JET == leLotSelect.JET || string.IsNullOrEmpty(leLotSelect.JET)) &&
                                                         (x.CENTRE == leLotSelect.CENTRE || string.IsNullOrEmpty(leLotSelect.CENTRE)) &&
                                                         (x.FK_IDCENTRE == leLotSelect.FK_IDCENTRE || leLotSelect.FK_IDCENTRE == 0)
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
                                                       //x.DATEEVT,
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
                                                       LIBELLEPRODUIT = x.PRODUIT1.LIBELLE ,
                                                       x.FK_IDCENTRE,
                                                       x.TYPECOMPTAGE,
                                                       x.DATEEVT 
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                   }
                   else
                   {
                       var _lstProffact = pcontext.PROFAC ;
                       IEnumerable<object> query = from x in _lstProffact
                                                   where
                  (x.LOTRI == leLotSelect.NUMLOTRI || string.IsNullOrEmpty(leLotSelect.NUMLOTRI)) &&
                                                         (x.JET == leLotSelect.JET || string.IsNullOrEmpty(leLotSelect.JET)) &&
                                                         (x.CENTRE == leLotSelect.CENTRE || string.IsNullOrEmpty(leLotSelect.CENTRE)) &&
                                                         (x.FK_IDCENTRE == leLotSelect.FK_IDCENTRE || leLotSelect.FK_IDCENTRE == 0) &&
                                                         (x.USERCREATION == leLotSelect.USERCREATION || x.USERMODIFICATION == leLotSelect.USERMODIFICATION)
                                                         && string.IsNullOrEmpty(x.TOPMAJ)

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
                                                       LIBELLEPRODUIT = x.PRODUIT1.LIBELLE,
                                                       x.FK_IDCENTRE,
                                                       x.TYPECOMPTAGE ,
                                                       x.DATEEVT 
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

       public static bool ValiderAnnulationFacture(CsEnteteFacture  laFacture)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   return true;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static CsLafactureClient RetourneFactureDetailPourAnnulation(CsEnteteFacture laFacture)
       {
           try
           {
               CsLafactureClient leDetailFactureClient = new Structure.CsLafactureClient();
               using (ABO07Entities c = new ABO07Entities())
               {
                   CENTFAC laFactureClient = c.CENTFAC.FirstOrDefault(t => t.PK_ID == laFacture.PK_ID);
                   leDetailFactureClient._LeEntatfac = Entities.ConvertObject<CsEnteteFacture, Galatee.Entity.Model.CENTFAC>(laFactureClient);
                   leDetailFactureClient._LstProfact = Entities.ConvertObject<CsProduitFacture, Galatee.Entity.Model.CPROFAC>(laFactureClient.CPROFAC.ToList());
                   leDetailFactureClient._LstRedFact = Entities.ConvertObject<CsRedevanceFacture, Galatee.Entity.Model.CREDFAC>(laFactureClient.CREDFAC.ToList()); 
               }
               return leDetailFactureClient;
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }
       public static DataTable retourneActionFact(CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _leAction = context.ACTION;
                   var _libAction = context.ACTIONFACTURATION;
                   IEnumerable<object> query =
                   from x in _leAction
                   join y in _libAction on x.ACTION1 equals y.CODE  
                   where (x.LOT == leLot.NUMLOTRI)
                   select new
                   {
                       x.LOT,
                       x.PERIODE,
                       ACTION1 = y.LIBELLE ,
                       x.JET,
                       x.SSACTION,
                       x.DATE1,
                       x.NOMBRE1,
                       x.MONTANT1,
                       x.NOMBRE2,
                       x.MONTANT2,
                       x.NOMBRE3,
                       x.MONTANT3,
                       x.MATRICULE,
                       x.PRODUIT,
                       x.STATUT,
                       x.LIBELLE,
                       x.PK_ID
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       #endregion
       #region Liste Redevance

       //Charger liste des lots reçu
       public static List<CsRedevance> LoadAllRedevance()
       {
           List<CsRedevance> ListeRedevance = new List<CsRedevance>();
           try
           {
               using (galadbEntities Context = new galadbEntities())
               {
                   //List<CsTrancheRedevence> dt = null;

                   foreach (var item in Context.REDEVANCE.ToList())
                   {
                       CsRedevance Redevance = new CsRedevance();
                       Redevance.PK_ID = item.PK_ID;
                       Redevance.CODE = item.CODE;
                       Redevance.DATECREATION = item.DATECREATION;
                       Redevance.DATEMODIFICATION = item.DATEMODIFICATION;
                       Redevance.FK_IDPRODUIT = item.FK_IDPRODUIT;
                       Redevance.FK_IDTYPELIENREDEVANCE = item.FK_IDTYPELIENREDEVANCE;
                       Redevance.FK_IDTYPEREDEVANCE = item.FK_IDTYPEREDEVANCE;
                       Redevance.FK_IDPRODUIT = item.FK_IDPRODUIT;
                       Redevance.LIBELLE = item.LIBELLE;
                       Redevance.PRODUIT = item.PRODUIT;
                       Redevance.USERCREATION = item.USERCREATION;
                       Redevance.USERMODIFICATION = item.USERMODIFICATION;
                       //Redevance.LIENREDEVANCE = item.LIENREDEVANCE;
                       Redevance.PRODUIT = item.PRODUIT;
                       //Redevance.TRANCHEREDEVANCE = item.PRODUIT;
                       Redevance.TYPELIEN = item.TYPELIENREDEVANCE.CODE ;
                       //Redevance.TYPEREDEVANCE = item.TYPEREDEVANCE;
                       //Redevance.VARIABLETARIF = item.VARIABLETARIF;

                       Redevance.TRANCHEREDEVANCE = new List<CsTrancheRedevence>();
                       //dt = RetourneTranchesRedevance(item.PK_ID);
                       //Redevance.TRANCHEREDEVANCE.AddRange(dt);


                       //foreach (var item_ in item.TRANCHEREDEVANCE)
                       //{
                       //    CsTrancheRedevence TrancheRedevence = new CsTrancheRedevence();
                       //    TrancheRedevence.FK_IDREDEVANCE = item_.FK_IDREDEVANCE;
                       //    TrancheRedevence.LIBELLE = item_.LIBELLE;
                       //    TrancheRedevence.GRATUIT = item_.GRATUIT;
                       //    TrancheRedevence.ORDRE = item_.ORDRE;
                       //    TrancheRedevence.REDEVANCE = Redevance;

                       //    Redevance.TRANCHEREDEVANCE.Add(TrancheRedevence);
                       //}
                       ListeRedevance.Add(Redevance);
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

           return ListeRedevance;
       }

       ////Transaction de mise à jour 
       //public static bool SaveRedevance(List<CsRedevance> ListeRedevanceScelleToUpdate, List<CsRedevance> ListeRedevanceScelleToInserte, List<CsRedevance> ListeRedevanceScelleToDelete)
       //{
       //    bool IsSaved = false;
       //    List<REDEVANCE> ListeRedevanceScelleToUpdate_ = new List<REDEVANCE>();
       //    List<REDEVANCE> ListeRedevanceScelleToInserte_ = new List<REDEVANCE>();
       //    List<REDEVANCE> ListeRedevanceScelleToDelete_ = new List<REDEVANCE>();

       //    try
       //    {

       //        using (galadbEntities Context = new galadbEntities())
       //        {
       //            convertCsRedevance(ListeRedevanceScelleToUpdate, ListeRedevanceScelleToUpdate_, Context);
       //            convertCsRedevance(ListeRedevanceScelleToInserte, ListeRedevanceScelleToInserte_, Context);
       //            //convertCsRedevance(ListeRedevanceScelleToDelete, ListeRedevanceScelleToDelete_);


       //            //Entities.UpdateEntity<REDEVANCE>(ListeRedevanceScelleToUpdate_, Context);
       //            //foreach (var item in ListeRedevanceScelleToUpdate_)
       //            //{
       //            //    Entities.DeleteEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //    Entities.InsertEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //}

       //            //Entities.InsertEntity<REDEVANCE>(ListeRedevanceScelleToInserte_, Context);
       //            //foreach (var item in ListeRedevanceScelleToInserte_)
       //            //{
       //            //    Entities.DeleteEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //    Entities.InsertEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //}
       //            //foreach (var item in ListeRedevanceScelleToDelete_)
       //            //{
       //            //    Entities.DeleteEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //    Entities.DeleteEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
       //            //}
       //            //Entities.DeleteEntity<REDEVANCE>(ListeRedevanceScelleToDelete_, Context);
       //            //TRANCHEREDEVENCE TRANCHEREDEVENCE = new TRANCHEREDEVENCE();

       //            //TRANCHEREDEVENCE.FK_IDREDEVENCE = 3;
       //            //TRANCHEREDEVENCE.GRATUIT = "1";
       //            //TRANCHEREDEVENCE.LIBELLE = "Tranche test";

       //            //Context.TRANCHEREDEVENCE.Add(TRANCHEREDEVENCE);
       //            Context.SaveChanges();
       //            IsSaved = true;

       //        }
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }

       //    return IsSaved;
       //}

       ////Convertir une liste de CsRedevance en une liste de REDEVANCE
       //private static void convertCsRedevance(List<CsRedevance> ListeRedevanceScelle, List<REDEVANCE> ListeRedevanceScelle_, galadbEntities Context)
       //{
       //    foreach (var item in ListeRedevanceScelle)
       //    {

       //        if (item.PK_ID > 0)
       //        {
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).USERCREATION = "99999";
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).CENTRE = !string.IsNullOrWhiteSpace(item.CODECENTRE) ? item.CENTRE : string.Empty;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).PRODUIT = !string.IsNullOrWhiteSpace(item.CODEPRODUIT) ? item.CENTRE : string.Empty;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).CODE = !string.IsNullOrWhiteSpace(item.CODE) ? item.CODE : string.Empty;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).LIBELLE = !string.IsNullOrWhiteSpace(item.LIBELLE) ? item.LIBELLE : string.Empty;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).FK_IDCENTRE = item.FK_IDCENTRE > 0 ? Context.CENTRE.FirstOrDefault(c => c.CODECENTRE == item.CENTRE).PK_ID : 0;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).FK_IDPRODUIT = item.FK_IDPRODUIT > 0 ? Context.PRODUIT.FirstOrDefault(p => p.CODE == item.PRODUIT).PK_ID : 0;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).PK_ID = item.PK_ID;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).DATECREATION = item.DATECREATION != null ? item.DATECREATION.Value : DateTime.Now;
       //            Context.REDEVANCE.FirstOrDefault(r => r.PK_ID == item.PK_ID).DATEMODIFICATION = item.DATEMODIFICATION != null ? item.DATEMODIFICATION.Value : DateTime.Now;
       //        }
       //        else
       //        {
       //            REDEVANCE Redevance = new REDEVANCE();

       //            Redevance.USERCREATION = "99999";
       //            Redevance.CENTRE = !string.IsNullOrWhiteSpace(item.CODECENTRE) ? item.CENTRE : string.Empty;
       //            Redevance.PRODUIT = !string.IsNullOrWhiteSpace(item.CODEPRODUIT) ? item.CENTRE : string.Empty;
       //            Redevance.CODE = !string.IsNullOrWhiteSpace(item.CODE) ? item.CODE : string.Empty;
       //            Redevance.LIBELLE = !string.IsNullOrWhiteSpace(item.LIBELLE) ? item.LIBELLE : string.Empty;
       //            Redevance.FK_IDCENTRE = item.FK_IDCENTRE > 0 ? Context.CENTRE.FirstOrDefault(c => c.CODECENTRE == item.CENTRE).PK_ID : 0;
       //            Redevance.FK_IDPRODUIT = item.FK_IDPRODUIT > 0 ? Context.PRODUIT.FirstOrDefault(p => p.CODE == item.PRODUIT).PK_ID : 0;
       //            Redevance.PK_ID = item.PK_ID;
       //            Redevance.DATECREATION = item.DATECREATION != null ? item.DATECREATION.Value : DateTime.Now;
       //            Redevance.DATEMODIFICATION = item.DATEMODIFICATION != null ? item.DATEMODIFICATION.Value : DateTime.Now;

       //            Context.REDEVANCE.Add(Redevance);

       //        }

       //        var OLD_TRANCHEREDEVANCE = Context.TRANCHEREDEVENCE.Where(t => t.FK_IDREDEVENCE == item.PK_ID);
       //        foreach (var item_ in OLD_TRANCHEREDEVANCE)
       //        {
       //            Context.TRANCHEREDEVENCE.Remove(item_);
       //        }

       //        foreach (var scelle in item.TrancheRedevences)
       //        {
       //            TRANCHEREDEVENCE TRANCHEREDEVENCE = new TRANCHEREDEVENCE();

       //            TRANCHEREDEVENCE.FK_IDREDEVENCE = item.PK_ID;
       //            TRANCHEREDEVENCE.GRATUIT = "1";
       //            TRANCHEREDEVENCE.LIBELLE = scelle.LIBELLE;

       //            Context.TRANCHEREDEVENCE.Add(TRANCHEREDEVENCE);
       //        }

       //        //ListeRedevanceScelle_.Add(Redevance);
       //    }
       //}

       #endregion

       public static ENTFAC RetourneEntfactFromIdEvenement(CsEvenement leEvenement,galadbEntities contxt)
       {
           try
           {
               ENTFAC leEntFact = contxt.ENTFAC.FirstOrDefault(t => t.PROFAC.FirstOrDefault(y => y.FK_IDEVENEMENT == leEvenement.PK_ID && y.FK_IDENTFAC == t.PK_ID) != null);
               return leEntFact;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static ENTFAC RetourneEntfactFromEvenement(CsEvenement leEvenement, galadbEntities contxt)
       {
           try
           {
               ENTFAC leEntFact = contxt.ENTFAC.FirstOrDefault(t => t.CENTRE == leEvenement.CENTRE && 
                   t.CLIENT == leEvenement.CLIENT && t.ORDRE == leEvenement.ORDRE &&
                   leEvenement.FK_IDCENTRE == t.FK_IDCENTRE && t.LOTRI == leEvenement.LOTRI);
               return leEntFact;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool? ValiderCreationFactureIsole(List<CsEvenement> lstEvenement)
       {

           try
           {
               galadbEntities _LeContextInter = new galadbEntities();
               List<EVENEMENT> _LstEvenement = new List<EVENEMENT>();
               List<EVENEMENT> _LstEvenementSupprimer = new List<EVENEMENT>();
               using (galadbEntities pContext = new galadbEntities())
               {
                   if (lstEvenement != null)
                   {   
                        
                       bool EvenementPerideExiste = false;
                       int FK_IDCENTRE = lstEvenement.First().FK_IDCENTRE;
                       string CENTRE = lstEvenement.First().CENTRE;
                       string CLIENT = lstEvenement.First().CLIENT;
                       string ORDRE = lstEvenement.First().ORDRE;
                       string PERIODE = lstEvenement.First().PERIODE;
                       string LOTRI = lstEvenement.First().LOTRI ;

                       //string Lot1 =CENTRE + "00002";
                       //string Lot2 =CENTRE + "00001";
                       //List<string> lotriIsole = new List<string>();
                       //lotriIsole.Add(Lot1);
                       //lotriIsole.Add(Lot2);


                       List<Galatee.Entity.Model.EVENEMENT> EvtPeriode = pContext.EVENEMENT.Where(t =>
                           t.CENTRE == CENTRE && t.CLIENT == CLIENT && t.ORDRE == ORDRE && t.FK_IDCENTRE == FK_IDCENTRE &&
                           t.PERIODE == PERIODE && LOTRI == t.LOTRI && (t.STATUS != Enumere.EvenementSupprimer && t.STATUS != Enumere.EvenementAnnule )).ToList();
                       if (EvtPeriode != null && EvtPeriode.Count != 0)
                       {
                           foreach (CsEvenement item in lstEvenement)
                           {
                               EVENEMENT leEVtPoint = EvtPeriode.FirstOrDefault(t => t.POINT == item.POINT);
                               if (leEVtPoint != null)
                               {
                                   leEVtPoint.INDEXEVT = item.INDEXEVT;
                                   leEVtPoint.CONSO = item.CONSO;
                                   leEVtPoint.DATEEVT = item.DATEEVT;
                                   leEVtPoint.CAS = item.CAS;
                                   leEVtPoint.ISCONSOSEULE = item.ISCONSOSEULE;
                                   //leEVtPoint.INDEXPRECEDENTEFACTURE = item.INDEXPRECEDENTEFACTURE;
                                   //leEVtPoint.PERIODEPRECEDENTEFACTURE = item.PERIODEPRECEDENTEFACTURE;
                                   //leEVtPoint.DATERELEVEPRECEDENTEFACTURE = item.DATERELEVEPRECEDENTEFACTURE; 

                               }
                           }

                       }
                       else
                       {
                           foreach (CsEvenement item in lstEvenement)
                           {
                             

                               int nuevt = item.NUMEVENEMENT;
                               List<EVENEMENT> LMaxEvt = _LeContextInter.EVENEMENT.Where(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && t.POINT == item.POINT ).ToList();
                               if (LMaxEvt != null && LMaxEvt.Count != 0)
                               {
                                   nuevt = LMaxEvt.Max(t => t.NUMEVENEMENT);
                                   item.NUMEVENEMENT = nuevt + 1;
                               }
                               EVENEMENT _LEvenement = Entities.ConvertObject<EVENEMENT, CsEvenement>(item);
                               _LstEvenement.Add(_LEvenement);

                               if (lstEvenement.FirstOrDefault(t =>!string.IsNullOrEmpty( t.LOTASUPPRIMER ))!= null)
                               {
                                   EVENEMENT LeEvtSupp = pContext.EVENEMENT.FirstOrDefault(t => t.CENTRE == item.CENTRE && t.CLIENT == item.CLIENT && 
                                                                                                       t.ORDRE == item.ORDRE && t.FK_IDCENTRE == item.FK_IDCENTRE && 
                                                                                                       t.POINT == item.POINT && t.LOTRI == item.LOTASUPPRIMER &&
                                                                                                       (t.STATUS == Enumere.EvenementReleve ||
                                                                                                        t.STATUS == Enumere.EvenementCree ||
                                                                                                        t.STATUS == Enumere.EvenementDefacture ||
                                                                                                        t.STATUS == Enumere.EvenementRejeter
                                                                                                        ));
                                   if (LeEvtSupp != null && !string.IsNullOrEmpty( LeEvtSupp .CLIENT)) 
                                      LeEvtSupp.STATUS = Enumere.EvenementSupprimer;
                               }
                           }

                           Entities.InsertEntity<EVENEMENT>(_LstEvenement, pContext);
                       }
                       //gestion de lotri
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
                   }
                  pContext.SaveChanges();
                  return true;
               }
              
           }
           catch (Exception e)
           {
               throw e;
           }
       }
       public static DataTable RetourneLesDernierEvtFacture(int fk_idcentre, string centre, string client, string Ordre, string produit, int? point)
        {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                    
             
                   int[] LstStatut = StatusEvenement(false);

                   var _LeEvenement = context.EVENEMENT ;
                   IEnumerable<object> query =
                   (from x in _LeEvenement
                   where x.FK_IDCENTRE == fk_idcentre &&
                        (x.CENTRE == centre) &&
                        (x.CLIENT  ==client) &&
                        (x.ORDRE   ==Ordre) &&
                        (x.PRODUIT   ==produit) &&
                        (x.POINT    ==point) &&
                        (x.STATUS == Enumere.EvenementFacture ||
                         x.STATUS == Enumere.EvenementMisAJour ||
                         x.STATUS == Enumere.EvenementPurger)
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
                       x.REGLAGECOMPTEUR,
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
                       x.ESTCONSORELEVEE
                   });
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneEvenementDePose(int fk_idcentre, string centre, string client, string Ordre, string produit, int? point)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _LeEvenement = context.EVENEMENT;
                   IEnumerable<object> query =
                   (from x in _LeEvenement
                    where x.FK_IDCENTRE == fk_idcentre &&
                         (x.CENTRE == centre) &&
                         (x.CLIENT == client) &&
                         (x.ORDRE == Ordre) &&
                         (x.PRODUIT == produit) &&
                         (x.POINT == point) &&
                         (x.CAS == Enumere.CasPoseCompteur )
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
                        x.REGLAGECOMPTEUR,
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
                        x.ESTCONSORELEVEE
                    });
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneCProfact(List<CsEnteteFacture > leCentafac)
       {
           try
           {
               List<int> lstIdEntfact = new List<int>();
               foreach (CsEnteteFacture item in leCentafac)
                   lstIdEntfact.Add(item.PK_ID);
               List<PRODUIT> lstProduit = new List<PRODUIT>();
               using(galadbEntities ctx = new galadbEntities())
	            {
		          lstProduit = ctx.PRODUIT.ToList();
	            }
               using (ABO07Entities pcontext = new ABO07Entities())
               {
                  
                       var _lstLRedfact = pcontext.CPROFAC;
                       IEnumerable<object> query = from x in _lstLRedfact
                                                   where lstIdEntfact.Contains(x.FK_IDCENTFAC)
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
                                                        FK_IDENTFAC=  x.FK_IDCENTFAC ,
                                                        x.FK_IDEVENEMENT,
                                                        x.DATECREATION,
                                                        x.DATEMODIFICATION,
                                                        x.USERCREATION,
                                                        x.USERMODIFICATION,
                                                        x.FK_IDPRODUIT,                                                       
                                                        x.FK_IDCENTRE,
                                                        x.TYPECOMPTAGE,
                                                        x.DATEEVT,


                                                       //x.CENTFAC.LOTRI,
                                                       //x.CENTFAC.TOURNEE,
                                                       //x.CENTFAC.ORDTOUR,
                                                       //x.CENTRE,
                                                       //x.CLIENT,
                                                       //x.ORDRE,
                                                       //x.FACTURE,
                                                       //x.PRODUIT,
                                                       //x.COMPTEUR,
                                                       //x.REGLAGECOMPTEUR ,
                                                       //x.COEFLECT,
                                                       //x.POINT,
                                                       //x.PUISSANCE,
                                                       //x.DERPERF,
                                                       //x.DERPERFN,
                                                       //x.TFAC,
                                                       //x.LIENRED,
                                                       //x.CONSOFAC,
                                                       //x.DATEEVT ,
                                                       //x.PERIODE,
                                                       //x.AINDEX,
                                                       //x.NINDEX,
                                                       //x.CAS,
                                                       //x.CONSO,
                                                       //x.TOTPROHT,
                                                       //x.TOTPROTAX,
                                                       //x.TOTPROTTC,
                                                       //x.ADERPERF,
                                                       //x.ADERPERFN,
                                                       //x.REGIMPUTE,
                                                       //x.REGROU,
                                                       //x.DEVPRE,
                                                       //x.LIENFAC,
                                                       //x.EVENEMENT,
                                                       //x.TOPANNUL,
                                                       //x.PUISSANCEINSTALLEE,
                                                       //x.COEFCOMPTAGE,
                                                       //x.BRANCHEMENT,
                                                       //x.COEFK1,
                                                       //x.COEFK2,
                                                       //x.PERTESACTIVES,
                                                       //x.PERTESREACTIVES,
                                                       //x.COEFFAC,
                                                       //x.PK_ID,
                                                       //FK_IDENTFAC = x.FK_IDCENTFAC,
                                                       //x.DATECREATION,
                                                       //x.DATEMODIFICATION,
                                                       //x.USERCREATION,
                                                       //x.USERMODIFICATION,
                                                       //x.FK_IDPRODUIT,
                                                       ////LIBELLEPRODUIT =lstProduit.FirstOrDefault(t=>t.PK_ID== x.FK_IDPRODUIT).LIBELLE ,
                                                       //x.FK_IDCENTRE,
                                                       //x.TYPECOMPTAGE,
                                                       //x.TYPECOMPTEUR ,
                                                       
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                   }
              
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneCRedfact(List<CsEnteteFacture> leCentafac)
       {
           try
           {
               List<int> lstIdEntfact = new List<int>();
               foreach (CsEnteteFacture item in leCentafac)
                   lstIdEntfact.Add(item.PK_ID);
               using (ABO07Entities pcontext = new ABO07Entities())
               {
                    var _lstLRedfact = pcontext.CREDFAC;
                       IEnumerable<object> query = from x in _lstLRedfact
                                                   where lstIdEntfact.Contains(x.FK_IDCENTFAC)

                                                   select new
                                                   {
                                                       x.CENTFAC.LOTRI,
                                                       x.CENTRE,
                                                       x.CLIENT,
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
                                                       x.PERIODE,
                                                       x.PRODUIT,
                                                       x.FORMULE,
                                                       x.TOPANNUL,
                                                       x.BARBORNEDEBUT,
                                                       x.BARBORNEFIN,
                                                       x.PK_ID,
                                                     FK_IDENTFAC=  x.FK_IDCENTFAC,
                                                       x.DATECREATION,
                                                       x.DATEMODIFICATION,
                                                       x.USERCREATION,
                                                       x.USERMODIFICATION,
                                                       x.FK_IDPRODUIT,
                                                       x.FK_IDCENTRE,
                                                   };
                       return Galatee.Tools.Utility.ListToDataTable<object>(query);
                 
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static List<CENTFAC> ListeFactureABO07(int idcentre, string centre, string client, string ordre, string periode, string numFacture,ABO07Entities c)
       {

           //RECUPE DE L'ENTETE DE FACTURE
           #region RECUPE DE L'ENTETE DE FACTURE
           var query_ = (from d in c.CENTFAC
                         where (d.CENTRE == centre &&
                                d.CLIENT == client &&
                                d.ORDRE == ordre &&
                                d.FK_IDCENTRE == idcentre &&
                                (d.FACTURE == numFacture || string.IsNullOrEmpty(numFacture)) &&
                                (d.PERIODE == periode || string.IsNullOrEmpty(periode)))
                         select d);


           return (List<CENTFAC>)query_.ToList();

           #endregion
       }
       public static DataTable ListeFactureABO07(int idcentre, string centre, string client, string ordre, string periode, string numFacture)
       {

           //RECUPE DE L'ENTETE DE FACTURE
           #region RECUPE DE L'ENTETE DE FACTURE
           using (ABO07Entities pcontext = new ABO07Entities())
           {
               var _lstLEntfact = pcontext.CENTFAC;
               IEnumerable<object> query = (from x in _lstLEntfact
                                            from y in x.CPROFAC
                                           where x.FK_IDCENTRE == idcentre && x.CENTRE == centre && x.CLIENT == client && x.ORDRE == ordre 
                                           select new
                                           {
                                               x.CENTRE,
                                               x.CLIENT,
                                               x.ORDRE,
                                               x.FACTURE,
                                               x.PERIODE,
                                               x.REGROUPEMENT,
                                               x.CODCONSO,
                                               x.CATEGORIECLIENT,
                                               x.DFAC,
                                               x.TOTFTAX,
                                               x.TOTFHT,
                                               x.TOURNEE,
                                               //x.MOISCOMPTA,
                                               x.COMMUNE,
                                               //x.DATEENC,
                                               //x.TYPEENC,
                                               x.LOTRI,
                                               x.COPER,
                                               x.MODEPAIEMENT,
                                               x.ANCIENREPORT,
                                               x.TOTFTTC,
                                               x.LIENFAC,
                                               x.SECTEUR,
                                               x.QUARTIER,
                                               x.ORDTOUR,
                                               x.NOMABON,
                                               x.RUE,
                                               x.PORTE ,
                                               x.NOMRUE,
                                               x.DRESABON,
                                               //x.DATEFLAG,
                                               x.USERCREATION,
                                               x.DATECREATION,
                                               x.USERMODIFICATION,
                                               x.DATEMODIFICATION,
                                               x.PK_ID,
                                               x.FK_IDCENTRE,
                                               x.FK_IDREGROUPEMENT,
                                               x.FK_IDCODECONSOMMATEUR,
                                               x.FK_IDCATEGORIECLIENT,
                                               x.FK_IDMODEPAIEMENT,
                                               x.FK_IDTOURNEE,
                                               x.FK_IDCOMMUNE,
                                               x.FK_IDCOPER,
                                               x.FK_IDSECTEUR,
                                               x.FK_IDQUARTIER,
                                               y.FK_IDEVENEMENT,
                                               y.FK_IDABON,
                                               x.FK_IDRUE,
                                               x.ISFACTUREEMAIL,
                                               x.EMAIL,
                                               x.ISFACTURESMS,
                                               x.TELEPHONE,
                                               x.FK_IDCLIENT,
                                               y.PRODUIT,
                                               x.EXIG ,
                                               x.ADRMAND1 ,
                                               x.ADRMAND2 

                                           }).Distinct();
               return Galatee.Tools.Utility.ListToDataTable<object>(query);
           #endregion

           }
       }

       public static DataTable RetournePaiementFacture(CsEnteteFacture laFacture)
        {
            try
            {

                List<CsLclient> LstEncaissement = new List<CsLclient>();
                galadbEntities context = new galadbEntities();
                IEnumerable<object> query = null;
                var _TRANSCAISSE = context.TRANSCAISSE ;
                query =
                from _LeTRANSCAISSE in _TRANSCAISSE
                where (_LeTRANSCAISSE.FK_IDCENTRE == laFacture.FK_IDCENTRE  &&
                       _LeTRANSCAISSE.CENTRE == laFacture.CENTRE  &&
                       _LeTRANSCAISSE.CLIENT == laFacture.CLIENT  &&
                       _LeTRANSCAISSE.ORDRE == laFacture.ORDRE &&
                       _LeTRANSCAISSE.REFEM  == laFacture.PERIODE  &&
                       _LeTRANSCAISSE.NDOC  == laFacture.FACTURE  &&
                       _LeTRANSCAISSE.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISSE.TOPANNUL != "O" || _LeTRANSCAISSE.TOPANNUL == null))
                orderby _LeTRANSCAISSE.DTRANS, _LeTRANSCAISSE.DATEENCAISSEMENT
                group _LeTRANSCAISSE by new
                {
                    CENTRE = _LeTRANSCAISSE.CENTRE,
                    CLIENT = _LeTRANSCAISSE.CLIENT,
                    ORDRE = _LeTRANSCAISSE.ORDRE,
                    CAISSE = _LeTRANSCAISSE.CAISSE,
                    ACQUIT = _LeTRANSCAISSE.ACQUIT,
                    NDOC = _LeTRANSCAISSE.NDOC,
                    REFEM = _LeTRANSCAISSE.REFEM,
                    DTRANS = _LeTRANSCAISSE.DTRANS,
                    DENR = _LeTRANSCAISSE.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISSE.DTRANS

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.DENR,
                    pTemp.Key.DTRANS,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                };


                IEnumerable<object> query1 = null;
                var _TRANSCAISB = context.TRANSCAISB;
                query1 =
                from _LeTRANSCAISB in _TRANSCAISB
                where (_LeTRANSCAISB.FK_IDCENTRE == laFacture.FK_IDCENTRE   &&
                       _LeTRANSCAISB.CENTRE == laFacture.CENTRE  &&
                       _LeTRANSCAISB.CLIENT == laFacture.CLIENT   &&
                       _LeTRANSCAISB.ORDRE == laFacture.ORDRE &&
                       _LeTRANSCAISB.REFEM == laFacture.PERIODE  &&
                       _LeTRANSCAISB.NDOC  == laFacture.FACTURE &&
                       _LeTRANSCAISB.COPER != Enumere.CoperTimbre &&
                       (_LeTRANSCAISB.TOPANNUL != "O" || _LeTRANSCAISB.TOPANNUL == null))
                orderby _LeTRANSCAISB.DTRANS, _LeTRANSCAISB.DATEENCAISSEMENT
               group _LeTRANSCAISB by new
                {
                    CENTRE = _LeTRANSCAISB.CENTRE,
                    CLIENT = _LeTRANSCAISB.CLIENT,
                    ORDRE = _LeTRANSCAISB.ORDRE,
                    CAISSE = _LeTRANSCAISB.CAISSE,
                    ACQUIT = _LeTRANSCAISB.ACQUIT,
                    NDOC = _LeTRANSCAISB.NDOC,
                    REFEM = _LeTRANSCAISB.REFEM,
                    DTRANS = _LeTRANSCAISB.DTRANS,
                    DENR = _LeTRANSCAISB.DTRANS,
                    DATEENCAISSEMENT = _LeTRANSCAISB.DTRANS

                } into pTemp
                let MONTANT = pTemp.Sum(x => x.MONTANT)
                select new
                {
                    pTemp.Key.CENTRE,
                    pTemp.Key.CLIENT,
                    pTemp.Key.ORDRE,
                    pTemp.Key.CAISSE,
                    pTemp.Key.ACQUIT,
                    pTemp.Key.NDOC,
                    pTemp.Key.REFEM,
                    pTemp.Key.DENR,
                    pTemp.Key.DTRANS,
                    MONTANT,
                    pTemp.Key.DATEENCAISSEMENT,
                };
                var result = query.Union(query1);
                return Galatee.Tools.Utility.ListToDataTable(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
       }

       public static  void  SupprimeAnnomalie(string lotri)
       {
           try
           {
               try
               {
                   List<ANOMALIEFACTURATION> _LesAnomalie = new List<ANOMALIEFACTURATION>();
                   using (galadbEntities context = new galadbEntities())
                   {
                      _LesAnomalie = context.ANOMALIEFACTURATION.Where(t=>t.LOTRI == lotri ).ToList() ;
                   }
                   if (_LesAnomalie != null && _LesAnomalie.Count != 0)
                       Entities.DeleteEntity<ANOMALIEFACTURATION>(_LesAnomalie);
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static DataTable RetourneLesteAnomalie( string lotri)
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LesAnomalie = context.ANOMALIEFACTURATION ;

                       IEnumerable<object> query =
                       (from x in _LesAnomalie
                       where x.LOTRI == lotri 
                       select new
                       {
                           x.CAUSE,
                           x.SOLUTION,
                           x.LOTRI,
                           x.COMPTEUR,
                           x.CENTRE,
                           x.CLIENT,
                           x.ORDRE,
                           x.FK_IDCENTRE,
                           x.PERIODE,
                           x.FK_IDPRODUIT,
                           x.FK_IDABON
                       }).Distinct();
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneLesJet(string lotri,string user)
       {
           try
           {
               try
               {
                   using (galadbEntities context = new galadbEntities())
                   {
                       var _LeEntfact = context.ENTFAC ;

                       IEnumerable<object> query =
                       (from x in _LeEntfact
                        where x.LOTRI  == lotri && x.USERCREATION == user 
                        select new
                        {
                           x.JET ,
                           x.LOTRI 
                        }).Distinct();
                       return Galatee.Tools.Utility.ListToDataTable(query);
                   }
               }
               catch (Exception ex)
               {
                   throw ex;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool  PurgeDeLot(List<EVENEMENT> _LstEvtDefact, List<ENTFAC> _LstFacture, List<PROFAC> _LstFactureProf, List<REDFAC> _LstFactureRed, List<CsLotri> _LeLot, List<ANOMALIEFACTURATION> lstAnofac)
       {
           try
           {
               int res = -1;
               using (galadbEntities pContext = new galadbEntities())
               {
                   CsStatFacturation _laStat = new CsStatFacturation();
                   List<LOTRI> _LeLotri = Entities.ConvertObject<LOTRI, CsLotri>(_LeLot);

                   Entities.DeleteEntity(_LstFactureRed, pContext);
                   Entities.DeleteEntity(_LstFactureProf, pContext);
                   Entities.DeleteEntity(_LstFacture, pContext);
                   Entities.UpdateEntity(_LstEvtDefact, pContext);
                   Entities.DeleteEntity(_LeLotri, pContext);
                   Entities.DeleteEntity(lstAnofac, pContext);
                   pContext.SaveChanges();
                   return res == -1 ?false : true ;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       public static DataTable retourneFactureDecroissance(List<int> lstIdCentre, CsLotri leLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _leEntfact = context.ENTFAC ;
                   IEnumerable<object> query =
                   from x in _leEntfact
                   where x.LOTRI == leLot.NUMLOTRI && lstIdCentre.Contains(x.FK_IDCENTRE)
                   select new
                   {
                       x.CENTRE ,
                       x.CLIENT,
                       x.ORDRE,
                       x.NOMABON ,
                       x.TOURNEE ,
                       x.ADRMAND1,
                       x.TOTFTTC,
                       x.TOTFHT ,
                       x.TOTFTAX ,
                       x.LOTRI ,
                       x.PERIODE ,
                       x.FACTURE ,
                       DENMAND = x.CENTRE + " " + x.CLIENT + " " + x.ORDRE 
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneClientLot( string Client,List<int> lstIdCentre)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _leAg = context.AG  ;
                   IEnumerable<object> query =
                   from t in _leAg
                   from x in t.CLIENT1 
                   from y in x.ABON
                   where x.REFCLIENT == Client && lstIdCentre.Contains(y.FK_IDCENTRE) && y.DRES ==null 
                   select new
                   {
                       x.CENTRE ,
                       x.REFCLIENT ,
                       x.ORDRE,
                       x.PK_ID ,
                       x.NOMABON ,
                       x.ADRMAND1,
                       x.CATEGORIE ,
                       y.PRODUIT ,
                       y.DRES ,
                       t.TOURNEE ,
                       t.FK_IDTOURNEE ,
                       t.FK_IDCENTRE ,
                       x.FK_IDCATEGORIE,
                       y.FK_IDPRODUIT,
                       FK_IDABON =  y.PK_ID 
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable VerifieClientExisteLot(CsClient  Client,string NumLot)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   var _leEvt = context.EVENEMENT ;
                   IEnumerable<object> query =
                   from x in _leEvt
                   where x.FK_IDCENTRE  == Client.FK_IDCENTRE &&
                         x.CENTRE   == Client.CENTRE  &&
                         x.CLIENT   == Client.REFCLIENT  &&
                         x.ORDRE   == Client.ORDRE  &&
                         x.LOTRI == NumLot &&
                         !(new int[] { Enumere.EvenementAnnule, Enumere.EvenementSupprimer, Enumere.EvenementPurger, Enumere.EvenementFacture, Enumere.EvenementMisAJour }).Contains(x.STATUS.Value) 
                   select new
                   {
                       x.CENTRE,
                   };
                   return Galatee.Tools.Utility.ListToDataTable(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable RetourneEvenement(CsClient Client)
       {
           try
           {
                  int[] LstStatut = StatusEvenementFacture();

               using (galadbEntities context = new galadbEntities())
               {
                       var _leAg = context.AG;
                       var query =
                       from t in _leAg
                       from b in t.BRT 
                       from x in t.CLIENT1
                       from y in x.ABON
                       from h in y.CANALISATION
                       where x.FK_IDCENTRE  == Client.FK_IDCENTRE &&
                             x.CENTRE == Client.CENTRE && 
                             x.REFCLIENT == Client.REFCLIENT &&
                             y.PRODUIT == Client.PRODUIT &&
                             h.DEPOSE == null 
                       select new
                       {
                           x.CENTRE,
                           x.REFCLIENT,
                           x.ORDRE,
                           y.PRODUIT ,
                           y.TYPETARIF ,
                           t.TOURNEE ,
                           t.ORDTOUR ,
                           PUISSANCESOUSCRITE= y.PUISSANCE,
                           COMPTEUR =h.COMPTEUR.NUMERO,
                           h.REGLAGECOMPTEUR ,
                           STATUTCOMPTEUR=  h.COMPTEUR.STATUTCOMPTEUR.CODE  ,
                           LIBELLETYPECOMPTEUR =h.COMPTEUR.TYPECOMPTEUR1.LIBELLE ,
                           h.COMPTEUR.TYPECOMPTEUR ,
                           h.COMPTEUR.COEFLECT ,
                           y.TYPECOMPTAGE ,
                           y.PERFAC ,
                           h.PROPRIO ,
                           x.CATEGORIE ,
                           y.FORFAIT ,
                           x.CODECONSO ,
                           FK_IDABON= y.PK_ID,
                           y.FK_IDCENTRE ,
                           FK_IDCLIENT =x.PK_ID ,
                           y.FK_IDPRODUIT ,
                           FK_IDCANALISATION = h.PK_ID,
                           h.FK_IDCOMPTEUR ,
                           h.POINT ,
                           b.PUISSANCEINSTALLEE ,
                           y.DEBUTEXONERATIONTVA  ,
                           y.FINEXONERATIONTVA,
                           NOMABON = y.CLIENT1.NOMABON,
                                            //RELEVEUR = t.TOURNEE1.RELEVEUR.ADMUTILISATEUR.LIBELLE,
                            CADRAN = h.COMPTEUR.CADRAN,
                            RUE = y.CLIENT1.AG.RUE,
                            PORTE = t.PORTE,
                            COMPTEURAFFICHER = h.COMPTEUR.NUMERO  
                       };

                     var _leEvtFacture = context.EVENEMENT.Where(t=>t.FK_IDCENTRE == Client.FK_IDCENTRE && 
                                                             t.CENTRE == Client.CENTRE &&
                                                             t.CLIENT == Client.REFCLIENT &&
                                                             t.ORDRE == Client.ORDRE &&
                                                             t.PRODUIT == Client.PRODUIT &&
                                                             LstStatut.Contains(t.STATUS.Value));

                     var _leEvt = context.EVENEMENT.Where(t => t.FK_IDCENTRE == Client.FK_IDCENTRE &&
                                          t.CENTRE == Client.CENTRE &&
                                          t.CLIENT == Client.REFCLIENT &&
                                          t.ORDRE == Client.ORDRE &&
                                          t.PRODUIT == Client.PRODUIT );
                     var query1 =
                     from t in query
                     join y in _leEvtFacture on new { t.FK_IDABON, t.POINT } equals new { y.FK_IDABON, y.POINT }
                     //join z in _leEvt on new { t.FK_IDABON, t.POINT } equals new { z.FK_IDABON, z.POINT }
                     where y.NUMEVENEMENT == _leEvt.Where(j=>j.POINT == t.POINT).Max(u=>u.NUMEVENEMENT)
                     select new
                     {
                         t.CENTRE,
                         t.REFCLIENT,
                         t.ORDRE,
                         t.PRODUIT,
                         t.TYPETARIF,
                         t.TOURNEE,
                         t.ORDTOUR,
                         t.PUISSANCESOUSCRITE ,
                         t.COMPTEUR,
                         t.REGLAGECOMPTEUR,
                         t.STATUTCOMPTEUR,
                         t.TYPECOMPTEUR,
                         t.COEFLECT,
                         t.TYPECOMPTAGE,
                         t.PERFAC,
                         t.PROPRIO,
                         t.CATEGORIE,
                         t.FORFAIT,
                         t.CODECONSO,
                         t.FK_IDABON ,
                         t.FK_IDCENTRE,
                         t.FK_IDCLIENT ,
                         t.FK_IDPRODUIT,
                         t.FK_IDCANALISATION ,
                         t.FK_IDCOMPTEUR,
                         t.POINT,
                         t.PUISSANCEINSTALLEE,
                         t.DEBUTEXONERATIONTVA,
                         t.FINEXONERATIONTVA,
                         DATERELEVEPRECEDENTEFACTURE= y.DATEEVT ,
                         PERIODEPRECEDENTEFACTURE= y.PERIODE  ,
                         CASPRECEDENTEFACTURE =y.CAS ,
                         CONSOMOYENNEPRECEDENTEFACTURE = y.CONSOMOYENNEPRECEDENTEFACTURE ,
                         INDEXPRECEDENTEFACTURE = y.INDEXEVT,
                         CAS = "##",
                         REFERENCE = t.CENTRE + " " + t.REFCLIENT  + " " + t.ORDRE,
                         t.NOMABON ,
                         t.CADRAN ,
                         t.RUE ,
                         t.PORTE ,
                         t.COMPTEURAFFICHER 
                     };
                     return Galatee.Tools.Utility.ListToDataTable(query1);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
      
   }
}
