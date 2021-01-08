using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
  public static class InterfaceComptableProcedure
    {
      private static bool ConvertDateTimeInDate(DateTime? dateDebutEnvoye, DateTime? dateDenr)
      {
          return Convert.ToDateTime(dateDebutEnvoye).ToShortDateString() == Convert.ToDateTime(dateDenr).ToShortDateString();
      }

      #region Interfacage Comptable Sylla



      private static DataTable RetourneFactureByCritere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
      {
          using (galadbEntities context = new galadbEntities())
          {

              IEnumerable<object> query;
              query = FatureByCritere(IdCentre, OperationSelect, DateCaisseDebut, DateCaisseFin, Date, context);
              return Galatee.Tools.Utility.ListToDataTable(query);
          }
      }

      private static IEnumerable<object> FatureByCritere(int IdCentre, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date, galadbEntities context)
      {
          IEnumerable<object> query;
          query = (from t in context.LCLIENT
                   join p in context.ABON on new { t.FK_IDCLIENT } equals new { p.FK_IDCLIENT }
                   where t.FK_IDCENTRE == IdCentre && OperationSelect.Contains(t.COPER) && p.DRES == null 
                   && ((DateCaisseDebut != null && DateCaisseFin != null && DateCaisseDebut <= t.DENR && DateCaisseFin >= t.DENR)
                   || (DateCaisseDebut != null && DateCaisseFin == null && DateCaisseDebut <= t.DENR)
                   || (DateCaisseDebut == null && DateCaisseFin != null && DateCaisseFin > t.DENR)) &&
                       (Date != null && Date == t.DENR )
                   select new
                   {
                       CENTRE = t.CENTRE,
                       CLIENT = t.CLIENT,
                       ORDRE = t.ORDRE,
                       MONTANT = t.MONTANT.Value,
                       ORIGINE = t.ORIGINE,
                       CAISSE = t.CAISSE,
                       PK_COPER = t.COPER,
                       DATECAISSE = t.DENR,
                       DATECREATION = t.DATECREATION,
                       NDOC = t.NDOC,
                       MODEREG = t.MODEREG,
                       PRODUIT = p.PRODUIT,
                       ISFACTURE = true
                   });
          if (query != null)
          {
              query = query.ToList();
          }
          else
          {
              query = null;
          }
          return query;
      }



      private static DataTable RetournReglementByCrytere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
      {

          using (galadbEntities context = new galadbEntities())
          {

              IEnumerable<object> query;
              query = ReglementByCrytere(IdCentre, lstIdcaisse, OperationSelect,DateCaisseDebut,DateCaisseFin, Date, context);
              return Galatee.Tools.Utility.ListToDataTable(query);
          }
      }

      private static IEnumerable<object> ReglementByCrytere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date, galadbEntities context)
      {
          IEnumerable<object> query;
          query = (from t in context.TRANSCAISB
                   join p in context.ABON on new { t.LCLIENT.FK_IDCLIENT } equals new { p.FK_IDCLIENT }
                   where t.HABILITATIONCAISSE.FK_IDCENTRE == IdCentre && 
                         lstIdcaisse.Contains(t.HABILITATIONCAISSE.FK_IDCAISSE) && 
                         OperationSelect.Contains(t.COPER)  && 
                        ((DateCaisseDebut != null && DateCaisseFin != null && 
                          DateCaisseDebut <= t.HABILITATIONCAISSE.DATE_DEBUT && 
                          DateCaisseFin >= t.HABILITATIONCAISSE.DATE_DEBUT)
                       || (DateCaisseDebut != null && 
                           DateCaisseFin == null && 
                           DateCaisseDebut <= t.HABILITATIONCAISSE.DATE_DEBUT)
                       || (DateCaisseDebut == null && 
                           DateCaisseFin != null && 
                           DateCaisseFin > t.HABILITATIONCAISSE.DATE_DEBUT)) 
                       //    &&
                       //(Date != null && Date == t.HABILITATIONCAISSE.DATE_DEBUT && p.DRES == null)
                   select new
                   {
                       CENTRE = t.CENTRE,
                       CLIENT = t.CLIENT,
                       ORDRE = t.ORDRE,
                       MONTANT = t.MONTANT.Value,
                       ORIGINE = t.ORIGINE,
                       CAISSE = t.CAISSE,
                       PK_COPER = t.COPER,
                       DATECAISSE = t.DATECREATION,
                       DATECREATION = t.DATECREATION,
                       NDOC = t.NDOC,
                       MODEREG = t.MODEREG,
                       PRODUIT = p.PRODUIT,
                       ISFACTURE = false

                   });
          //query = from x in CompteClient
          //        select new 
          //        {
          //            CENTRE        = x.CENTRE,
          //            CLIENT        = x.CLIENT,
          //            ORDRE         = x.ORDRE,
          //            MONTANT       = x.MONTANT.Value,
          //            ORIGINE       = x.ORIGINE,
          //            CAISSE        = x.CAISSE,
          //            PK_COPER      = x.COPER,
          //            DATECAISSE    = x.DTRANS,
          //            DATECREATION  = x.DATECREATION,
          //            NDOC=x.NDOC,
          //            NATURE=x.NATURE
          //        };
          if (query != null)
          {
              query = query.ToList();
          }
          else
          {
              query = null;
          }
          return query;
      }

      public static DataTable RetourneAllOperationClient(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  string OperSelect = OperationSelect.First();
                  var COPER_DE_DEBIT = context.COPER.Where(t => OperationSelect.Contains(t.CODE) && t.DC == Enumere.Debit);
                  bool IsDebite = COPER_DE_DEBIT != null ? true : false;

                  var COPER_DE_CREDITE = context.COPER.Where(t => OperationSelect.Contains(t.CODE)&& t.DC == Enumere.Credit);
                  bool IsCredit = COPER_DE_CREDITE != null ? true : false;

                  COPER leCoper = context.COPER.FirstOrDefault(t => t.CODE == OperSelect);
                  if (IsDebite && IsCredit)
                  {
                      var ToutLesCoper = COPER_DE_DEBIT.Select(t => t.CODE).ToList();
                      ToutLesCoper.AddRange(COPER_DE_CREDITE.Select(t => t.CODE).ToList());
                      var Facture = RetourneFactureByCritere(IdCentre, lstIdcaisse,COPER_DE_DEBIT .Select(t => t.CODE).ToList(), DateCaisseDebut, DateCaisseFin, Date);
                      var Reglement = RetournReglementByCrytere(IdCentre, lstIdcaisse, COPER_DE_CREDITE.Select(t => t.CODE).ToList(), DateCaisseDebut, DateCaisseFin, Date);
                      var TouteLesOperation = Facture;
                      TouteLesOperation.Merge(Reglement);

                      return TouteLesOperation;

                  }
                  else if (IsDebite)
                  {
                      return RetourneFactureByCritere(IdCentre, lstIdcaisse, COPER_DE_DEBIT.Select(t =>t.CODE).ToList(), DateCaisseDebut, DateCaisseFin, Date);

                  }
                  else if (IsCredit)
                  {
                      return RetournReglementByCrytere(IdCentre, lstIdcaisse, COPER_DE_CREDITE.Select(t =>t.CODE).ToList(), DateCaisseDebut, DateCaisseFin, Date);

                  }
                  return null;
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      private static DataTable RetourneFactureEtReglementByCritere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
      {
          throw new NotImplementedException();
      }

      public static List<CsTypeFactureComptable> RetourneTypeFacture()
      {
          try
          {
              using (galadbEntities Context = new galadbEntities())
              {
                  //#region Récupe données à Manipuler depuis la BD
                  //var TYPE_FACTURE_COMPTABILISE = Context.TYPE_FACTURE_COMPTABILISE;
                  //var COPERS = Context.COPER_TYPE_FACTURE_COMPTABILISE;
                  //var TYPE_COMPTE_TYPE_FACTURE_COMPTABLE = Context.TYPE_COMPTE_TYPE_FACTURE_COMPTABLE;
                  //#endregion

                  //#region Convertion des type de facture comptable en structure
                  //List<CsTypeFactureComptable> ListeTypeFacture = Entities.ConvertObject<CsTypeFactureComptable, TYPE_FACTURE_COMPTABILISE>(TYPE_FACTURE_COMPTABILISE != null ? TYPE_FACTURE_COMPTABILISE.ToList() : new List<TYPE_FACTURE_COMPTABILISE>());
                  //#endregion

                  //#region Récupérartion des des code opération(COPER) lié à chaque Type de facture comptable
                  //foreach (var item in ListeTypeFacture)
                  //{
                  //    var LISTCOPER = COPERS.Where(c => c.FK_IDTYPE_FACTUE_COMPTABILISER == item.PK_ID);
                  //    item.LISTCOPER = LISTCOPER != null ? LISTCOPER.Select(c => c.COPER.CODE).ToList() : new List<string>();
                  //    //item.DC = TYPE_COMPTE_TYPE_FACTURE_COMPTABLE.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == item.PK_ID).DC;f
                  //}
                  //#endregion

                  //#region Récupérartion des comptes (COPER) et des Type de compte(en indiquant la direction:debit ou credit) lié à chaque Type de facture comptable
                  //foreach (var item in ListeTypeFacture)
                  //{
                  //    //Récupérartion des comptes (COPER)
                  //    var LISTCOPER = COPERS.Where(c => c.FK_IDTYPE_FACTUE_COMPTABILISER == item.PK_ID);

                  //    item.LISTCOPER = LISTCOPER != null ? LISTCOPER.Select(c => c.COPER.CODE).ToList() : new List<string>();

                  //    //Récupérartion des Type de compte(en indiquant la direction:debit ou credit)
                  //    var LISTTYPECOMPTE = TYPE_COMPTE_TYPE_FACTURE_COMPTABLE.Where(c => c.FK_IDTYPE_FACTUE_COMPTABILISER == item.PK_ID);
                  //    item.LISTTYPECOMPTE = LISTTYPECOMPTE != null ? Entities.ConvertObject<CsTypeCompte, TYPE_COMPTE>(LISTTYPECOMPTE.Select(c => c.TYPE_COMPTE).ToList()) : new List<CsTypeCompte>();
                  //    //En indiquant la direction:debit ou credit
                  //    item.LISTTYPECOMPTE.ForEach(t => t.DC = LISTTYPECOMPTE.FirstOrDefault(c => c.FK_IDTYPE_FACTUE_COMPTABILISER == item.PK_ID && c.FK_IDTYPE_COMPTE == c.PK_ID).DC);
                  //}
                  //#endregion

                  //return ListeTypeFacture;

                  return null;
              }
          }
          catch (Exception ex)
          {
              return null;
          }
      }

      public static List<CsCompteSpecifique> RetourneCompteSpecifique()
      {
          try
          {
              using (galadbEntities Context = new galadbEntities())
              {
                  #region Récupe données à Manipuler depuis la BD
                  var COMPTE_SPECIFIQUE = Context.COMPTA_COMPTE_SPECIFIQUE ;
                  #endregion

                  #region Convertion des type de facture comptable en structure
                  List<CsCompteSpecifique> ListeCompteSpecifique = Entities.ConvertObject<CsCompteSpecifique, COMPTA_COMPTE_SPECIFIQUE >(COMPTE_SPECIFIQUE != null ? COMPTE_SPECIFIQUE.ToList() : new List<COMPTA_COMPTE_SPECIFIQUE >());
                  #endregion
                  foreach (CsCompteSpecifique item in ListeCompteSpecifique)
                  {
                      if (!string.IsNullOrEmpty(item.VALEURFILTRE))
                      {
                          item.LSTVALEURFILTRE = new List<string>();
                          string[] valeur = item.VALEURFILTRE.Split(';');
                          foreach (var items in valeur)
                              item.LSTVALEURFILTRE.Add(items);
                      }
                      if (!string.IsNullOrEmpty(item.VALEURFILTRE1))
                      {
                          item.LSTVALEURFILTRE1 = new List<string>();
                          string[] valeur = item.VALEURFILTRE1.Split(';');
                          foreach (var items in valeur)
                              item.LSTVALEURFILTRE1.Add(items);
                      }
                      if (!string.IsNullOrEmpty(item.VALEURFILTRE2))
                      {
                          item.LSTVALEURFILTRE2 = new List<string>();
                          string[] valeur = item.VALEURFILTRE2.Split(';');
                          foreach (var items in valeur)
                              item.LSTVALEURFILTRE2.Add(items);
                      }
                  }
                  return ListeCompteSpecifique;
              }
          }
          catch (Exception)
          {

              throw;
          }
      }

      public static bool InsertionLigneComptable(List<CsEcritureComptable> LigneComptable)
      {
          try
          {
              List<COMPTA_ECRITURECOMPTABLE> ECRITURE_COMPTABLE = new List<COMPTA_ECRITURECOMPTABLE>();
              using (galadbEntities Context = new galadbEntities())
              {
                  LigneComptable.ForEach(u => u.CREDIT1 = u.CREDIT);
                  LigneComptable.ForEach(u => u.DEBIT1  = u.DEBIT);
                  ECRITURE_COMPTABLE.AddRange(Entities.ConvertObject<COMPTA_ECRITURECOMPTABLE, CsEcritureComptable>(LigneComptable));
                  Entities.InsertEntity<COMPTA_ECRITURECOMPTABLE>(ECRITURE_COMPTABLE, Context);
                  return Context.SaveChanges() > 0 ? true : false;
              }
          }
          catch (Exception ex)
          {

              throw ex;
          }
      }
      public static List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable)
      {
          try
          {
              List<COMPTA_ECRITURECOMPTABLE> ECRITURE_COMPTABLE = new List<COMPTA_ECRITURECOMPTABLE>();
              using (galadbEntities Context = new galadbEntities())
              {
                  foreach (CsEcritureComptable item in LigneComptable)
                  {
                      if (!string.IsNullOrEmpty(item.CAISSE))
                      {
                          if (Context.COMPTA_ECRITURECOMPTABLE.FirstOrDefault(t => t.SITE == item.SITE && t.CENTRE == item.CENTRE
                                                                     && t.CAISSE == item.CAISSE
                                                                     && t.FK_IDOPERATION == item.FK_IDOPERATION
                                                                     && t.DATEOPERATION == item.DATEOPERATION) != null)
                              item.IsGenere = true;
                      }
                      else
                      {
                          if (Context.COMPTA_ECRITURECOMPTABLE.FirstOrDefault(t => t.SITE == item.SITE
                                                                        && t.CENTRE == item.CENTRE
                                                                        && t.FK_IDOPERATION == item.FK_IDOPERATION
                                                                        && t.DATEOPERATION == item.DATEOPERATION) != null)
                              item.IsGenere = true;
                      }


                  }
                  return LigneComptable;
              }
          }
          catch (Exception ex)
          {

              throw ex;
          }
      }
      public static List<CsOperationComptable> RetourneOperationComptable()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var _Operation = context.COMPTA_OPERATIONCOMPTABLE  ;
                  IEnumerable<object> query =
                  from x in _Operation
                  select new
                  {
                      x.CODE,
                      x.LIBELLE,
                      x.PK_ID ,
                      x.COPERIDENTIFIANT ,
                      x.FK_IDCOPER ,
                      x.COPER.DC ,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.LIBELLECOMPTABLE 
                  };
                  DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                  return  Entities.GetEntityListFromQuery<CsOperationComptable>(dt);
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public static List<CsTypeCompte> RetourneTypeCompte()
      {
          try
          {
                  using (galadbEntities context = new galadbEntities())
                  {
                      var _TypeCompte = context.COMPTA_TYPE_COMPTE ;
                      IEnumerable<object> query =
                      from x in _TypeCompte
                      select new
                      {
                          x.CODE,
                          x.LIBELLE,
                          x.PK_ID,
                          x.AVECFILTRE,
                          x.TABLEFILTRE,
                          x.TABLEFILTRE1,
                          x.TABLEFILTRE2,
                          x.SIGNE,
                          x.SOUSCOMPTE ,
                          x.VALEURMONTANT,
                          x.USERCREATION,
                          x.USERMODIFICATION,
                          x.DATECREATION,
                          x.DATEMODIFICATION
                      };
                      DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                      return Entities.GetEntityListFromQuery<CsTypeCompte>(dt);
                  }
          }
          catch (Exception ex)
          {

              throw;
          }
      }

      public static List<CsBanqueCompte> RetourneBanqueCentre()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var _TBanqueCompte = context.COMPTA_BANQUECENTRE;
                  IEnumerable<object> query =
                  from x in _TBanqueCompte
                  select new
                  {
                      x.PK_ID,
                      x.BANQUE,
                      x.CODECENTRE,
                      x.COMPTE
                  };
                  DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                  return Entities.GetEntityListFromQuery<CsBanqueCompte>(dt);
                  return new List<CsBanqueCompte>();
              }
          }
          catch (Exception ex)
          {

              throw;
          }
      }

      public static List<CsCentreCompte > RetourneParamCentre()
      {
          try
          {
              using (galadbEntities context = new galadbEntities())
              {
                  var _TBanqueCompte = context.COMPTA_CENTRE ;
                  IEnumerable<object> query =
                  from x in _TBanqueCompte
                  select new
                  {
                      x.PK_ID,
                      x.CODEACTIVITE ,
                      x.CODECENTRE,
                      x.DC ,
                      x.LIBELLEACTIVITE ,
                      x.CODECOMPTA ,
                      x.CI
                  };
                  DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                  return Entities.GetEntityListFromQuery<CsCentreCompte>(dt);
              }
          }
          catch (Exception ex)
          {
              throw;
          }
      }

    
      #endregion



  
  }
}
