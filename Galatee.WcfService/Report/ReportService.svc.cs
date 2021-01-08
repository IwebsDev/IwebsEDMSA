using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.Collections;
using System.ServiceModel.Activation;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "ReportService" à la fois dans le code, le fichier svc et le fichier de configuration.
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    [DataContract]
    public class ReportService: IReportService
    {
       public List<CsDemandeBase> ReturneDevisTerminerDsLesDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           try
           {
               List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
               foreach (var p in produit)
               {
                   List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisDevisValiderByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, p);
                   foreach (CsDemandeBase item in lsDemande)
                       if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) <= 15)
                           lesDemandeFin.Add(item); 
               }
               return lesDemandeFin;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public List<CsDemandeBase> ReturneDevisTerminerDsLesDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           try
           {
               List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
               List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisDevisValiderByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, produit);
               foreach (CsDemandeBase item in lsDemande)
                   if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) <= 15)
                       lesDemandeFin.Add(item);
               return lesDemandeFin;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public List<CsDemandeBase> ReturneDevisTerminerHorsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           foreach (var p in produit)
           {

               List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisDevisValiderByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, p);
               foreach (CsDemandeBase item in lsDemande)
                   if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) > 15)
                       lesDemandeFin.Add(item); 
           }
           return lesDemandeFin;
       }
       public List<CsDemandeBase> ReturneDevisTerminerHorsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisDevisValiderByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, produit);
           foreach (CsDemandeBase item in lsDemande)
               if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) > 15)
                   lesDemandeFin.Add(item);
           return lesDemandeFin;
       }

       public List<CsDemandeBase> ReturneTravauxRealiserDsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           foreach (var p in produit)
           {
               List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, p);
               foreach (CsDemandeBase item in lsDemande)
                   if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) <= 15)
                       lesDemandeFin.Add(item); 
           }
           return lesDemandeFin;
       }
       public List<CsDemandeBase> ReturneTravauxRealiserDsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, produit);
           foreach (CsDemandeBase item in lsDemande)
               if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) <= 15)
                   lesDemandeFin.Add(item);
           return lesDemandeFin;
       }
       public List<CsDemandeBase> ReturneTravauxRealiserHorsDelais(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, produit);
           foreach (CsDemandeBase item in lsDemande)
               if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) > 15)
                   lesDemandeFin.Add(item);
           return lesDemandeFin;
       }
       public List<CsDemandeBase> ReturneTravauxRealiserHorsDelais_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> lesDemandeFin = new List<CsDemandeBase>();
           foreach (var p in produit)
           {
               List<CsDemandeBase> lsDemande = new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, p);
               foreach (CsDemandeBase item in lsDemande)
                   if (item.DATEFIN != null && Galatee.Tools.Utility.JourOuvrer(item.DATECREATION, item.DATEFIN.Value) > 15)
                       lesDemandeFin.Add(item); 
           }
           return lesDemandeFin;
       }
       public List<CsDemandeBase> ReturneTravauxRealiser(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           return new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, produit);
       }
       public List<CsDemandeBase> ReturneTravauxRealiser_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> result = new List<CsDemandeBase>();
           foreach (var p in produit)
           {
               result.AddRange( new DBReports().ReturneDevisTravauxRealiseByDateCentre(lstIdCende, dtDebut, dtFin, typedemande, p));
           }
           return result;
       }
       public List<CsDemandeBase> ReturneDemandeParType(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           return new DBReports().ReturneDevisDemandeByTypeDemande(lstIdCende, dtDebut, dtFin, typedemande, produit);
       }

       public List<CsDemandeBase> ReturneDevisPayeEnInstanceDeLiaison_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
            List<CsDemandeBase> result= new List<CsDemandeBase>();
        foreach (var p in produit)
	    {
		    result.AddRange( new DBReports().ReturneDemandeEnAttenteDeLiaison(lstIdCende, dtDebut, dtFin, typedemande, p));
 
	    }           
           return result;
       }
       public List<CsDemandeBase> ReturneDevisPayeEnInstanceDeLiaison(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           return new DBReports().ReturneDemandeEnAttenteDeLiaison(lstIdCende, dtDebut, dtFin, typedemande, produit);
       }
       public List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisation(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           return new DBReports().ReturneDemandeEnAttenteDeRealisation(lstIdCende, dtDebut, dtFin, typedemande, produit);
       }
       public List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisation_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> result = new List<CsDemandeBase>();
           foreach (var p in produit)
           {
               result.AddRange(new DBReports().ReturneDemandeEnAttenteDeRealisation(lstIdCende, dtDebut, dtFin, typedemande, p));
           }
           return result;
       }
       public List<CsDemandeBase> ReturneRegistreDemande(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
       {
           return new DBReports().ReturneRegistreDemande(lstIdCende, dtDebut, dtFin, typedemande, produit);
       }
       public List<CsDemandeBase> ReturneRegistreDemande_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, List<string> produit)
       {
           List<CsDemandeBase> result = new List<CsDemandeBase>();
           foreach (var p in produit)
           {
               result.AddRange(new DBReports().ReturneRegistreDemande(lstIdCende, dtDebut, dtFin, typedemande, p));
           }
           return result;
       }
       public List<CsCanalisation> ReturnecompteursDisponiblesEnMagasin(List<int> lstCentre, int? idCalibreCompteur, string produit, string EtatCompteur, DateTime? debut, DateTime? fin)
       {
           if (EtatCompteur == "1")
               return new DBReports().ReturnecompteursDisponibles(lstCentre, idCalibreCompteur, produit, EtatCompteur);
           else if (EtatCompteur == "2")
               return new DBReports().ReturnecompteursAttribue(lstCentre, idCalibreCompteur, produit, EtatCompteur, debut, fin);
           else if (EtatCompteur == "3")
           {
               List<CsCanalisation> lstCompteur = new List<CsCanalisation>();
               //lstCompteur.AddRange(new DBReports().ReturnecompteursDisponibles(lstCentre, idCalibreCompteur, produit, EtatCompteur));
               //lstCompteur.AddRange(new DBReports().ReturnecompteursAttribue(lstCentre, idCalibreCompteur, produit, EtatCompteur));
               return lstCompteur;
           }
           else return null;
          
       }

       public List<CsLclient> ReturneListeDesImpayes(Dictionary<string, List<int>> lstCentre, List<int> lstIdCategorieClient, List<int> lstIdTournee,bool IsDetail)
       {
           return new DBReports().ReturneListeDesImpayes(lstCentre, lstIdCategorieClient, lstIdTournee, IsDetail);
       }

       public List<CsRedevanceFacture > ReturneVentePeriodeAnnee(List<int> lstIdCentre, string periode, string Annee,bool IsRecap)
       {
           return new DBReports().ReturneVentePeriodeAnnee(lstIdCentre, periode, Annee,IsRecap );
       }

       public List<CsEvenement> ReturneNombreMoyenDeFacturation(string CodeSite, string Periode)
       {
           return new DBReports().ReturneNombreMoyenDeFacturation(CodeSite, Periode);
       }
       public List<CsEvenement> ReturneConsNull(Dictionary<string, List<int>> lstSiteCentre, string Periode)
       {
           return new DBReports().ReturneConsNull(lstSiteCentre, Periode);
       }
       public  List<CsRedevanceFacture> ReturneVenteCummule(List<int> lstIdCentre, string Periode, bool IsSatistique)
       {
           return new DBReports().ReturneVenteCummule(lstIdCentre, Periode, IsSatistique);
       }
       public List<CsEvenement> ReturneActionFacturation(List<int> lstIdCentre, string Periode, string Lotri)
       {
           return new DBReports().ReturneActionFacturation(lstIdCentre, Periode, Lotri);
       }

       public List<CsEvenement> ReturneCompteurParProduit(List<int> lstIdCentre, List<int> lstIdProduit)
       {
           return new DBReports().ReturneCompteurParProduit(lstIdCentre, lstIdProduit);
       }
       public List<CsEvenement> ReturneCompteurParProduitPeriode(Dictionary<string, List<int>> lesDeCentre, string periode, bool IsStat)
       {
           return new DBReports().ReturneCompteurParProduitPeriode(lesDeCentre, periode, IsStat);
       }

       public List<CsEvenement> ReturneFactureIsole(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin)
       {
           return new DBReports().ReturneFactureIsole(lstIdCentre, DateDebut, DateFin);
       }

       public List<CsEvenement> ReturneAnnulationFacture(List<int> lstIdCentre,DateTime DateDebut,DateTime DateFin)
       {
           return new DBReports().ReturneAnnulationFacture(lstIdCentre, DateDebut, DateFin);
       }
       public List<CsEvenement> ReturneAnnulationFactureTop(List<int> lstIdCentre,DateTime DateDebut,DateTime DateFin,int Top)
       {
           return new DBReports().ReturneAnnulationFactureTop(lstIdCentre, DateDebut, DateFin,Top);
       }
       public List<CsDetailCampagne> ReturneAvisEmis(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
       {
           return new DBReports().ReturneAvisEmis(CodeSite, IdMatricule, DateDebut, DateFin);
       }
       public List<CsDetailCampagne> ReturneAvisCoupeType(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
       {
           return new DBReports().ReturneAvisCoupeType(CodeSite, IdMatricule, DateDebut, DateFin);
       }

       public List<CsDetailCampagne> ReturneAvisRepose(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
       {
           return new DBReports().ReturneAvisRepose(CodeSite, IdMatricule, DateDebut, DateFin);
       }


       //public List<CsDetailCampagne> ReturneAvisCoupeType(int IdCentre, List<string> Matricule, DateTime DateDebut, DateTime DateFin)
       //{
       //    //List<CsDetailCampagne> lstAvisEmis = new DBReports().ReturneAvisEmis(IdCentre, Matricule, DateDebut, DateFin);
       //    //List<CsDetailCampagne> lstAvisEmis = new List<CsDetailCampagne>();
       //    //List<CsDetailCampagne> lstAvisCoupe = new DBReports().ReturneAvisCoupe(IdCentre, Matricule, DateDebut, DateFin);
       //    //var lstCampDist = (from t in lstAvisCoupe
       //    //                   group new { t } by new { t.IDCOUPURE, t.NOMABON, t.LIBELLECOUPURE } into pResult
       //    //                   select new
       //    //                   {
       //    //                       pResult.Key.NOMABON,
       //    //                       pResult.Key.LIBELLECOUPURE,
       //    //                       NOMBREAVISCOUPE = (int)pResult.Where(y => y.t.IDCOUPURE == pResult.Key.IDCOUPURE).Count(),
       //    //                       NOMBREAVISEMIS = 0
       //    //                   });
       //    //List<CsDetailCampagne> LesAvisCoupe = new List<CsDetailCampagne>();
       //    //int TotalCoupe = 0;
       //    //foreach (var r in lstCampDist)
       //    //{
       //    //    CsDetailCampagne camp = new CsDetailCampagne();
       //    //    camp.NOMABON = r.NOMABON;
       //    //    camp.LIBELLECOUPURE = r.LIBELLECOUPURE;
       //    //    camp.NOMBREAVISCOUPE = r.NOMBREAVISCOUPE;
       //    //    camp.NOMBREAVISEMIS = lstAvisEmis.Count();
       //    //    camp.POURCENTAGE = (decimal)(((decimal)camp.NOMBREAVISCOUPE / (decimal)camp.NOMBREAVISEMIS) * 100);

       //    //    TotalCoupe = TotalCoupe + camp.NOMBREAVISCOUPE;
       //    //    LesAvisCoupe.Add(camp);
       //    //}
       //    //if (lstAvisEmis.Count > TotalCoupe)
       //    //{

       //    //    CsDetailCampagne camp = new CsDetailCampagne();
       //    //    camp.NOMABON = LesAvisCoupe.First().NOMABON;
       //    //    camp.LIBELLECOUPURE = "Avis non coupé";
       //    //    camp.NOMBREAVISCOUPE = lstAvisEmis.Count - TotalCoupe;
       //    //    camp.NOMBREAVISEMIS = lstAvisEmis.Count();
       //    //    camp.POURCENTAGE = (decimal)(((decimal)camp.NOMBREAVISCOUPE / (decimal)camp.NOMBREAVISEMIS) * 100);
       //    //    LesAvisCoupe.Add(camp);
       //    //}
       //    //return LesAvisCoupe;
       //    return new List<CsDetailCampagne>();
       //}

       public List<CsDetailCampagne> ReturneMontant(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           List<CsDetailCampagne> lstDetailCampagne = new List<CsDetailCampagne>();
           List<CsCAMPAGNE> lstCampagne = new DBReports().ReturneCampagne(IdCentre, Matricule, DateDebut, DateFin);
           if (lstCampagne != null && lstCampagne.Count != 0)
               lstDetailCampagne = RetourneMontantEncaisse(lstCampagne);

           return lstDetailCampagne;
       }

       public List<CsDetailCampagne> RetourneMontantEncaisse(List<CsCAMPAGNE> lesCampagnes)
       {
           try
           {
               List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
               foreach (CsCAMPAGNE laCampage in lesCampagnes)
               {
                   lstPaiementCampagne.AddRange(new DBClientAccess().PaiementCampagne(laCampage));
                   lstPaiementCampagne.ForEach(t => t.NOMAGENT = laCampage.AGENTPIA);
               }
               var lstCampDist = (from t in lstPaiementCampagne
                                  group new { t } by new { t.FK_IDCENTRE ,t.CENTRE ,t.CLIENT ,t.ORDRE ,t.FK_IDCLIENT  , t.NOMABON,t.NOMAGENT } into pResult
                                  select new
                                  {
                                      pResult.Key.FK_IDCENTRE  ,
                                      pResult.Key.CENTRE  ,
                                      pResult.Key.CLIENT  ,
                                      pResult.Key.ORDRE ,
                                      pResult.Key.NOMABON,
                                      pResult.Key.NOMAGENT ,
                                      pResult.Key.FK_IDCLIENT ,
                                      MONTANTEREGLE = pResult.Sum(y => y.t.MONTANT),

                                  });

               List<CsDetailCampagne> lstFriasCampagne = new DBClientAccess().FraisCampagnePayer(lesCampagnes);
               List<CsDetailCampagne> lesPaiementClient = new List<CsDetailCampagne>();
               foreach (var item in lstCampDist)
               {
                   CsDetailCampagne leFrais = lstFriasCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                   CsDetailCampagne leDetCampagne = new CsDetailCampagne();
                   leDetCampagne.CENTRE = item.CENTRE;
                   leDetCampagne.CLIENT = item.CLIENT;
                   leDetCampagne.ORDRE = item.ORDRE;
                   leDetCampagne.NOMABON = item.NOMABON;
                   leDetCampagne.NOMAGENT = item.NOMAGENT;
                   leDetCampagne.MONTANTEREGLE = item.MONTANTEREGLE.Value;
                   leDetCampagne.MONTANTFRAIS = leFrais != null ? leFrais.MONTANTFRAIS : 0;
                   leDetCampagne.DATEREGLEMENT = leFrais != null ? leFrais.DATEREGLEMENT : null;
                   lesPaiementClient.Add(leDetCampagne);
               }
               return lesPaiementClient;
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }
       }

       public List<CsDetailCampagne> RetourneMontantEncaisseFrais(List<CsCAMPAGNE> lesCampagnes)
       {
           try
           {
               List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
               lstPaiementCampagne.AddRange(new DBClientAccess().FraisCampagnePayer(lesCampagnes));
               var lstCampDist = (from t in lstPaiementCampagne
                                  group new { t } by new { t.FK_IDCENTRE, t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT, t.NOMABON,t.IDCOUPURE  } into pResult
                                  select new
                                  {
                                      pResult.Key.FK_IDCENTRE,
                                      pResult.Key.CENTRE,
                                      pResult.Key.CLIENT,
                                      pResult.Key.ORDRE,
                                      pResult.Key.IDCOUPURE ,
                                      pResult.Key.NOMABON,
                                      pResult.Key.FK_IDCLIENT,
                                      MONTANTFRAIS = pResult.Sum(y => y.t.MONTANT),

                                  });

               List<CsDetailCampagne> lesPaiementClient = new List<CsDetailCampagne>();
               foreach (var item in lstCampDist)
               {
                   CsDetailCampagne leDetCampagne = new CsDetailCampagne();
                   leDetCampagne.CENTRE = item.CENTRE;
                   leDetCampagne.CLIENT = item.CLIENT;
                   leDetCampagne.ORDRE = item.ORDRE;
                   leDetCampagne.NOMABON = item.NOMABON;
                   leDetCampagne.NOMAGENT = lesCampagnes.FirstOrDefault(t => t.IDCOUPURE == item.IDCOUPURE).AGENTPIA;
                   leDetCampagne.MONTANTFRAIS = item.MONTANTFRAIS;
                   lesPaiementClient.Add(leDetCampagne);
               }
               return lesPaiementClient;
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
               return null;
           }
       }

       //public List<CsDetailCampagne> ReturneAvisRepose(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
       //{

       //    List<CsDetailCampagne> lstDetailRemis = new List<CsDetailCampagne>();
       //    List<CsDetailCampagne> lstAvisCoupe = new DBReports().ReturneAvisCoupe(IdCentre, Matricule, DateDebut, DateFin);
       //    var lstCampDist = (from t in lstAvisCoupe
       //                       group new { t } by new { t.NOMABON, t.NOMAGENT } into pResult
       //                       select new
       //                       {
       //                           pResult.Key.NOMAGENT,
       //                           NOMBREAVISCOUPE = (int)pResult.Where(y => y.t.NOMAGENT == pResult.Key.NOMAGENT).Count(),
       //                       });

       //    List<CsCAMPAGNE> lstCampagne = new DBReports().ReturneCampagne(IdCentre, Matricule, DateDebut, DateFin);
       //    if (lstCampagne != null && lstCampagne.Count != 0)
       //        lstDetailRemis = RetourneMontantEncaisseFrais(lstCampagne);
       //    List<CsDetailCampagne> LesAvisCoupe = new List<CsDetailCampagne>();
       //    foreach (var r in lstCampDist)
       //    {
       //        CsDetailCampagne camp = new CsDetailCampagne();
       //        camp.NOMAGENT = r.NOMAGENT;
       //        camp.LIBELLECOUPURE = "CLIENT REMIS";
       //        camp.NOMBREAVISCOUPE = r.NOMBREAVISCOUPE;
       //        camp.NOMBREAVISEMIS = lstDetailRemis.Where(t => t.NOMAGENT == r.NOMAGENT).Count();
       //        camp.POURCENTAGE = (decimal)(((decimal)camp.NOMBREAVISEMIS / (decimal)camp.NOMBREAVISCOUPE) * 100);
       //        LesAvisCoupe.Add(camp);

       //        if (camp.NOMBREAVISCOUPE > camp.NOMBREAVISEMIS)
       //        {
       //            CsDetailCampagne camps = new CsDetailCampagne();
       //            camps.NOMAGENT = r.NOMAGENT;
       //            camps.LIBELLECOUPURE = "CLIENT NOM REMIS";
       //            camps.NOMBREAVISCOUPE = camp.NOMBREAVISCOUPE ;
       //            camps.NOMBREAVISEMIS = camp.NOMBREAVISCOUPE - camp.NOMBREAVISEMIS;
       //            camps.POURCENTAGE = (decimal)(((decimal)camps.NOMBREAVISEMIS / (decimal)camps.NOMBREAVISCOUPE) * 100);
       //            LesAvisCoupe.Add(camps);
       //        }
       //    }
       //    return LesAvisCoupe;
       //}


       public List<CsDetailCampagne> RetourneClientResilieSuiteCampagne(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           return new DBReports().RetourneClientResilieSuiteCampagne(IdCentre, Matricule, DateDebut, DateFin);
       }

       public List<CsLclient > RetourneTauxDeRecouvrement(List<int> IdCentre)
       {
           List<string> lstPeriode = DetermineListePeriode(1, 12, System.DateTime.Today);

           List<CsLclient> lesFacture = new DBReports().RetourneFactureGenere(IdCentre, lstPeriode);
           List<CsLclient> lesReglement = new DBReports().RetourneFactureRecouvre(IdCentre, lstPeriode);
           List<CsLclient> lesTaux = new List<CsLclient>();
           List<string> lstCateg = new List<string>();
           var lstCate = lesFacture.Select(p => p.LIBELLECATEGORIE).Distinct();
           foreach (var item in lstCate)
               lstCateg.Add(item);

           foreach (string item in lstPeriode)
           {
               foreach (string categ in lstCateg)
               {
                   CsLclient l = new CsLclient();
                   l.REFEM = item;
                   l.LIBELLESITE  = categ;
                   l.MONTANTEMIS = lesFacture.FirstOrDefault(i => i.REFEM == item && i.LIBELLECATEGORIE ==categ) != null ? lesFacture.FirstOrDefault(i => i.REFEM == item && i.LIBELLECATEGORIE ==categ).MONTANT.Value : 0;
                   l.MONTANTENCAISSE = lesReglement.FirstOrDefault(i => i.REFEM == item && i.LIBELLECATEGORIE ==categ) != null ? lesReglement.FirstOrDefault(i => i.REFEM == item && i.LIBELLECATEGORIE ==categ).MONTANT.Value : 0;
                   l.TAUXRECOUVREMENT = (l.MONTANTENCAISSE / l.MONTANTEMIS) * 100;
                   lesTaux.Add(l); 
               }
           }
           return lesTaux;
       }

       List<string> DetermineListePeriode(int frequence, int NombreOccurMor, DateTime DateDebut)
       {
           try
           {
               List<string> LstPeriode = new List<string>();
               DateTime now = new DateTime();
               if (DateDebut == System.DateTime.Today)
                   now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
               else
                   now = new DateTime(DateDebut.Year, DateDebut.Month, DateDebut.Day);

               for (int i = 1; i < NombreOccurMor; i++)
               {
                   DateTime laDate = new DateTime();
                    string datte = string.Empty;
                    if (frequence == 1)
                        laDate = now.AddMonths(-i);
                    LstPeriode.Add(laDate.Year.ToString() + laDate.Month.ToString("00"));
               }
               return LstPeriode;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public List<CsLclient> RetourneTauxDeEncaissement(List<int> IdCentre)
       {
           List<string> lstPeriode = DetermineListePeriode(1, 12, System.DateTime.Today);
           List<CsLclient> lesFactureRetourne = new List<CsLclient>();
           List<CsLclient> lesFacture = new DBReports().RetourneFactureGenere(IdCentre, lstPeriode);
           List<CsLclient> lesReglement = new DBReports().RetourneFactureRecouvre(IdCentre, lstPeriode);
           if (lesFacture != null && lesFacture.Count != 0)
           {
               CsLclient lafacturePeriode = lesFacture.First();
               lafacturePeriode.MONTANTENCAISSE = lesReglement.FirstOrDefault(t => t.REFEM == lesFacture.First().REFEM ) == null ? 0 :
                                          lesReglement.FirstOrDefault(t => t.REFEM == lesFacture.First().REFEM ).MONTANT.Value;
               lafacturePeriode.MONTANTEMIS = lafacturePeriode.MONTANT.Value;
               lafacturePeriode.TAUXRECOUVREMENT = (lafacturePeriode.MONTANTENCAISSE / lafacturePeriode.MONTANTEMIS) * 100;
               lesFactureRetourne.Add(lafacturePeriode);

               CsLclient AutreFacture = new CsLclient();
               AutreFacture.MONTANTEMIS = lesFacture.Where(t => t.REFEM != lesFacture.First().REFEM).Sum(y => y.MONTANT.Value);
               AutreFacture.MONTANTENCAISSE = lesReglement.Where (t => t.REFEM != lesFacture.First().REFEM ) == null ? 0 :
                           lesReglement.Where(t => t.REFEM != lesFacture.First().REFEM).Sum(y => y.MONTANT.Value);
               AutreFacture.REFEM = "AUTRE";
               AutreFacture.TAUXRECOUVREMENT = (AutreFacture.MONTANTENCAISSE / AutreFacture.MONTANTEMIS) * 100;

               lesFactureRetourne.Add(AutreFacture);
           }
           return lesFactureRetourne;
       }

       public List<CsDetailCampagne> MontantPaiementPreavis(string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           List<CsDetailCampagne> lstDetailCampagne = new List<CsDetailCampagne>();
           List<CsCampagneGc> lstCampagne = new DBReports().ReturneCampagneSgc(Matricule, DateDebut, DateFin);
           if (lstCampagne != null && lstCampagne.Count != 0)
               lstDetailCampagne = new DBReports().RetourneMontantEncaisseSgc(lstCampagne);
           return lstDetailCampagne;
       }

       public List<CsUtilisateur > RetourneGestionnaire()
       {
           return new DBReports().RetourneGestionnaire();
       }
       public List<CsDetailCampagne> ListePreavisPreavis(string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           List<CsDetailCampagne> lstDetailCampagne = new List<CsDetailCampagne>();
           List<CsCampagneGc> lstCampagne = new DBReports().ReturneCampagneSgc(Matricule, DateDebut, DateFin);
           if (lstCampagne != null && lstCampagne.Count != 0)
               lstDetailCampagne = new DBReports().RetournePreavisSgc(lstCampagne);
           return lstDetailCampagne;
       }

       public List<CsMandatementGc> ListeMandatement(string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           try
           {
               return new DBReports().ReturneMandatatementSgc(Matricule, DateDebut, DateFin);
           }
           catch (Exception ex)
           {
              ErrorManager.LogException(this, ex);
                return null;
           }
       }
       public List<CsDetailCampagne> ListePaiementMandatement(string Matricule, DateTime DateDebut, DateTime DateFin)
       {
           try
           {
               List<CsDetailCampagne> lstDetailCampagne = new List<CsDetailCampagne>();
               List<CsMandatementGc> lstCampagne = new DBReports().ReturneMandatatementSgc(Matricule, DateDebut, DateFin);
               if (lstCampagne != null && lstCampagne.Count != 0)
                   lstDetailCampagne = new DBReports().RetournePaiementMandatementSgc(lstCampagne);
               return lstDetailCampagne;
           }
           catch (Exception ex)
           {
               
                   ErrorManager.LogException(this, ex);
                return null;
           }
       }


       public List<CsMandatementGc > RetourneTauxDeMandatement(string Matricule)
       {
           List<string> lstPeriode = DetermineListePeriode(1, 12, System.DateTime.Today);
           List<CsMandatementGc> lesFactureRetourne = new List<CsMandatementGc>();
           List<CsMandatementGc> lesMandatClient = new DBReports().RetourneMandatementTaux(lstPeriode,Matricule);
           List<CsMandatementGc> lesMandatgeneral = new DBReports().RetourneMandatementTotal(lstPeriode, Matricule);
           if (lesMandatClient != null && lesMandatClient.Count != 0)
           {
               foreach (CsMandatementGc item in lesMandatClient)
               {
                   item.MONTANT  = item.MONTANT;
                   item.MONTANTTOTAL = lesMandatgeneral.Count  == 0 ? 0 :
                                          lesMandatgeneral.Sum(t => t.MONTANT);
                   item.TAUXRECOUVREMENT = (item.MONTANT / item.MONTANTTOTAL) * 100;
                   lesFactureRetourne.Add(item);
               }
           }
           return lesFactureRetourne;
       }

       public List<CsMandatementGc> RetourneTauxPaiement(string Matricule)
       {
           List<string> lstPeriode = DetermineListePeriode(1, 12, System.DateTime.Today);
           List<CsMandatementGc> lesFactureRetourne = new List<CsMandatementGc>();
           List<CsMandatementGc> lesMandatClient = new DBReports().RetourneTauxPaiement(lstPeriode, Matricule);
           List<CsMandatementGc> lesMandatgeneral = new DBReports().RetourneTauxPaiementTotal(lstPeriode, Matricule);
           if (lesMandatClient != null && lesMandatClient.Count != 0)
           {
               foreach (CsMandatementGc item in lesMandatClient)
               {
                   item.MONTANT = item.MONTANT;
                   item.MONTANTTOTAL = lesMandatgeneral.Count == 0 ? 0 :
                                          lesMandatgeneral.Sum(t => t.MONTANT);
                   item.TAUXRECOUVREMENT = (item.MONTANT / item.MONTANTTOTAL) * 100;
                   lesFactureRetourne.Add(item);
               }
           }
           return lesFactureRetourne;
       }

       public List<CsLclient> ReturneEmissionProduitRegroupement(List<int> IdRegroupement, string PeriodeDebut, string periodefin)
       {
           return new DBReports().ReturneEmissionProduitRegroupement(IdRegroupement,PeriodeDebut,periodefin);
       }
       public List<CsLclient> ReturneAvanceSurConso(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
       {
           return new DBReports().ReturneAvanceSurCommation(Idcentre, DateDebut, DateFin);
       }

       public List<CsLclient> ReturneEncaissementReversement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
       {
           return new DBReports().ReturneEncaissementReversement(Idcentre, DateDebut, DateFin);
       }
       public List<CsLclient> ReturneEncaissementModePaiement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
       {
           return new DBReports().ReturneEncaissementModePaiement(Idcentre, DateDebut, DateFin);
       }
       public List<CsStatFactRecap> ReturneVente(List<int> Idcentre, string PeriodeDebut, string periodefin)
       {
           return new DBReports().ReturneVente(Idcentre, PeriodeDebut, periodefin);
       }
       public List<CsLclient> ReturneClientPrepayeSansAchatPeriode(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
       {
           return new DBReports().ReturneClientPrepayeSansAchatPeriode(Idcentre, DateDebut, DateFin);
       }
       public List<CsLclient> ReturneClientPrepayeJamaisAchat(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
       {
           return new DBReports().ReturneClientPrepayeJamaisAchat(Idcentre, DateDebut, DateFin);
       }

       public List<CsComptabilisation> ReturneCompabilisationRecap(List<int> Idcentre, string periode,bool IsGroup)
       {
           return new DBReports().ReturneCompabilisationRecap(Idcentre, periode, IsGroup);
       }
       public List<CsStatFact> ReturneStatistique(string CodeSite, string periode, string Produit,bool IsStat)
       {
           return new DBReports().ReturneStatistique(CodeSite, periode, Produit,IsStat);
       }
       public void ReturneFichierPersonnel(string periode, string CheminImpression)
       {
           try
           {
               new DBReports().ReturneFichierPersonnel(periode, CheminImpression);
               //new DBReports().ReturneFichierPersonnelDirecteur(periode, CheminImpression);
           }
           catch (Exception ex)
           {
               ErrorManager.LogException(this, ex);
           }
       }
       public List<CsRedevance> ChargerRedevance()
       {
           return new DBCalcul().ChargerRedevance();
       }

       public List<CsComptabilisation> ReturneStatiqueDesVente(List<int> Idcentre, string periode, bool IsGrouper)
       {
           return new DBReports().ReturneStatiqueDesVente(Idcentre, periode, IsGrouper);
       }
       public List<CsTranscaisse > ReturneEncaissementParMoisComptable(  List<int>  lesDeCentre, string periode )
       {
           return new DBReports().ReturneEncaissementParMoisComptable(lesDeCentre, periode );
       }
       public List<CsTournee> ReturneTourneePIA(string CodeSite)
       {
           return new DBReports().RetourneTourneeParPIA(CodeSite);
       }



       //public List<CsDemandeBase> ReturneRegistreDemande_(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, list<string> produit)
       //{
       //    throw new NotImplementedException();
       //}
    }
}
