using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Structure;
using System.Data;
using Galatee.Entity.Model;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using System.IO;


namespace Galatee.DataAccess
{
    public class DbInterfaceComptable
    {

        SqlConnection sqlConnection;
        SqlConnection sqlConnectionAbo07;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;
        private string Abo07ConnectionString;

        public DbInterfaceComptable()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
                Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CsSite> RetourneListeDeSite()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousSites();
                return Entities.GetEntityListFromQuery<CsSite>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCaisse> RetourneCaisse()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCaisse();
                return Entities.GetEntityListFromQuery<CsCaisse>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCoper> RetourneOperation()
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.CommonProcedures.RetourneTousCoper();
                return Entities.GetEntityListFromQuery<CsCoper>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetourneAllOperation(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
        {
            try
            {
                DataTable dt = Galatee.Entity.Model.InterfaceComptableProcedure.RetourneAllOperationClient(IdCentre, lstIdcaisse, OperationSelect, DateCaisseDebut, DateCaisseFin, Date);
                return Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> Retournefacture(List<CsOperationComptable> lesOperationCpt, List<int> IdCentre, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
        {
            try
            {
                string LeSite = Site;
                List<CsComptabilisation> lstEcriture = new List<CsComptabilisation>();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);

                CsOperationComptable OpComptable = lesOperationCpt.FirstOrDefault(u => u.CODE == Enumere.FactureTravaux);
                if (OpComptable != null)
                {
                    if (Site == Enumere.Generale)
                    {
                        if (lstCentre.FirstOrDefault(o => o.CODE == Enumere.Generale) != null)
                            lstEcriture.AddRange(RetournefactureTrvxSpx(lstCentre.FirstOrDefault(o => o.CODE == Enumere.Generale).PK_ID, DateCaisseDebut, DateCaisseFin));
                    }
                    else
                        lstEcriture.AddRange(RetournefactureTrvxSpx(IdCentre.First(), DateCaisseDebut, DateCaisseFin));

                }
                foreach (CsOperationComptable Operation in lesOperationCpt.Where(p=>p.CODE != Enumere.FactureTravaux).ToList())
                {
                    foreach (int item in IdCentre)
                    {
                        if (Operation.CODE == Enumere.FraisRemise ||
                            Operation.CODE == Enumere.FraisEtalonnage ||
                            Operation.CODE == Enumere.FraisChequeImpaye)
                            lstEcriture.AddRange(RetournefactureFactureAutreSpx(Operation.CODE, item, Operation.COPERIDENTIFIANT, DateCaisseDebut, DateCaisseFin));
                    }
                }
                Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> TheDatas = new Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>();
                List<string> lstOperatGnrale = new List<string>();
                lstOperatGnrale.Add(Enumere.FactureAnnulation);
                lstOperatGnrale.Add(Enumere.FactureEmissionGeneral);
                List<CsOperationComptable> LstOperation = lesOperationCpt.Where(j => lstOperatGnrale.Contains(j.CODE )).ToList();
                if (LstOperation.Count == 0)
                {
                    List<CsEcritureComptable> lstEcritureComptable = ComptabilisationAutreFacture(lstEcriture, lesOperationCpt, IdCentre, matricule, Site);
                    TheDatas.Add(lstEcriture, lstEcritureComptable);
                }
                else
                {
                    List<CsComptabilisation> lstFacture = new List<CsComptabilisation>();
                    string MoisCpt = DateCaisseDebut.Value.Year.ToString() + DateCaisseDebut.Value.Month.ToString("00");
                    string MoisCptAfficher = DateCaisseDebut.Value.Year.ToString() + (DateCaisseDebut.Value.Month + 1).ToString("00");
                    foreach (int item in IdCentre)
                    {
                        lstFacture.AddRange(new DBReports().ReturneCompabilisationRecapEntfac(item, MoisCpt));
                        lstFacture.AddRange(new DBReports().ReturneCompabilisationRecapRedfac(item, MoisCpt));
                    }
                    List<CsEcritureComptable> lstEcritureComptable = ComptabilisationFactureEmissionGneral(IdCentre, lstFacture, matricule, Site, MoisCptAfficher);
                    TheDatas.Add(lstFacture.Where(i=>i.DC=="D").ToList(), lstEcritureComptable);
                }
                return TheDatas;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
        {
            try
            {
                List<CsComptabilisation> lstEcriture = new List<CsComptabilisation>();
                foreach (CsOperationComptable Operation in lesOperationCpt)
                {
                    foreach (CsCaisse item in lstCaisse)
                    {
                        if (Operation.CODE == Enumere.EncaissementBranchement)
                            lstEcriture.AddRange(RetourneEncaissementFactureTrvxSpx(Operation.COPERIDENTIFIANT, item.PK_ID, DateCaisseDebut, DateCaisseFin));
                        else if (Operation.CODE == Enumere.EncaissementFacture)
                            lstEcriture.AddRange(RetourneEncaissementFactureSpx(Operation.COPERIDENTIFIANT, item.PK_ID, DateCaisseDebut, DateCaisseFin));
                        else if (Operation.CODE == Enumere.EncaisseFraisRemise || Operation.CODE == Enumere.EncaisseEtalonnage)
                            lstEcriture.AddRange(RetourneEncaissementAutreFactureSpx(Operation.CODE, item.PK_ID, DateCaisseDebut, DateCaisseFin, Operation.COPERIDENTIFIANT));
                    }
                }

                Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> TheDatas = new Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>();
                List<CsEcritureComptable> lstEcritureComptable = ComptabilisationEncaissement(lstEcriture, lesOperationCpt, lstCaisse, matricule, Site);
                TheDatas.Add(lstEcriture, lstEcritureComptable);
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<CsEcritureComptable> ComptabilisationAutreFacture(List<CsComptabilisation> lstFacture, List<CsOperationComptable> LstOperationCompableSelect, List<int> lstIdCentre, string matricule, string Site)
        public List<CsEcritureComptable> ComptabilisationAutreFacture(List<CsComptabilisation> lstFacture2, List<CsOperationComptable> LstOperationCompableSelect, List<int> lstIdCentre, string matricule, string Site)
        {
            string CompteL = string.Empty;
            try
            {
                List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();
                List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
                List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
                List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
                List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);


                List<CsComptabilisation> lstFacture = new List<CsComptabilisation>();
                CsComptabilisation cpt = null;

                foreach (CsComptabilisation st in lstFacture2)
                {
                    cpt = new CsComptabilisation();
                    cpt = st;
                    if (cpt.COPER == Enumere.CoperPRE)
                        cpt.COPER = Enumere.CoperTRV;
                    lstFacture.Add(cpt);
                }


                string LeSite = string.Empty;
                List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();

                var lstFactureDistnctDate = lstFacture.Select(t => new { t.DATECREATION }).Distinct().ToList();
                foreach (var idCentre in lstIdCentre)
                {
                    CsCentre lesCentre = lstCentre.FirstOrDefault(y => y.PK_ID == idCentre);
                    foreach (var lesDate in lstFactureDistnctDate.OrderBy(t => t.DATECREATION).ToList())
                    {
                        foreach (CsOperationComptable OperationComptat in LstOperationCompableSelect)
                        {
                            List<int> lstCoperCompte = new List<int>();
                            List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
                            lstCoperCompte.AddRange(ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
                            List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();
                            List<CsComptabilisation> lesFactureOperation = lstFacture.Where(t => t.FK_IDCENTRE == lesCentre.PK_ID && t.DATECREATION == lesDate.DATECREATION && t.COPERINITAL == OperationComptat.COPERIDENTIFIANT).ToList();

                            if (lesFactureOperation.Count == 0) continue;
                            string LibelleActivite = string.Empty;
                            string CodeCentre = lesCentre.CODE;
                            if (lesCentre.CODESITE == Enumere.CodeSiteScaBT ||
                                    lesCentre.CODESITE == Enumere.CodeSiteScaMT)
                                CodeCentre = Enumere.Generale;

                            CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == CodeCentre && t.DC == "V");
                            if (OperationComptat.LIBELLECOMPTABLE == "VENTE")
                                    LibelleActivite = leCentreCaisse.LIBELLEACTIVITE;

                            foreach (CsTypeCompte TypeCompte in lstTypeCompte)
                            {

                                string SousCompte = "000000";
                                string NumeroPiece = string.Empty;
                                NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;

                                if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                    SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "TCI" + lesCentre.CODE;

                                List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
                                if (TypeCompte.AVECFILTRE == false)
                                {
                                    if (TypeCompte.SOUSCOMPTE == "CAISSE")
                                    {
                                        var lstFactureDistnctDateCaisse = lesFactureOperation.Select(t => new { t.DATECREATION }).Distinct().ToList();
                                        foreach (var item in lstFactureDistnctDateCaisse)
                                        {
                                            var lstFactureDistnctCaisse = lesFactureOperation.Select(t => new { t.FK_IDCAISSE, t.FK_IDCENTRECAISSE, t.CAISSE }).Distinct().ToList();
                                            foreach (var items in lstFactureDistnctCaisse)
                                            {
                                                List<CsComptabilisation> lesFactureOperationCaisseDate = lesFactureOperation.Where(p => p.FK_IDCAISSE == items.FK_IDCAISSE && p.DATECREATION == item.DATECREATION).ToList();
                                                CsCentre lCentre = lstCentre.FirstOrDefault(t => t.PK_ID == items.FK_IDCENTRECAISSE);
                                                SousCompte = leCentreCaisse.CODECOMPTA + items.CAISSE;

                                                #region SANSFILTRE
                                                CsCompteSpecifique leCompteSpe = ListeCompteSpecifique.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID);
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {
                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                                else
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                                else
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lesFactureOperationCaisseDate.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                                        SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "TCI" + lesCentre.CODE;

                                                    LigneComptable.SITE = lesCentre.CODESITE;
                                                    LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
                                                    LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                    LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                                    LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                    LigneComptable.DC = leCompteSpe.DC;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                    else
                                                        LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                                    LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                    LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                                    LigneComptable.INITIAL = Enumere.INITIAL;
                                                    LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                    LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                    LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                    LigneComptable.NO = Enumere.NO;
                                                    LigneComptable.ZERO = Enumere.ZERO;
                                                    LigneComptable.ALPHA = Enumere.ALPHA;
                                                    LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                    LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                    LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                                    LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                    LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                    LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                                    LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                    LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                    LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                    LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.SITE = Site;
                                                    LigneComptable.CENTRE = lesCentre.CODE;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;
                                                    LigneComptable.CENTREIMPUTATION = "00000";
                                                    if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    {
                                                        if (lesCentre.TYPECENTRE == "AG")
                                                        {
                                                            CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                            if (leCCaisse != null)
                                                                LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                        }
                                                        else
                                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                    }
                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    else
                                    {
                                        #region SANSFILTRE
                                        CsCompteSpecifique leCompteSpe = ListeCompteSpecifique.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID);
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }

                                            if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                                SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "TCI" + lesCentre.CODE;



                                            LigneComptable.SITE = lesCentre.CODESITE;
                                            LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.NUMPIECE = NumeroPiece;
                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                            else
                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                            LigneComptable.INITIAL = Enumere.INITIAL;
                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                            LigneComptable.NO = Enumere.NO;
                                            LigneComptable.ZERO = Enumere.ZERO;
                                            LigneComptable.ALPHA = Enumere.ALPHA;
                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                            LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.SITE = Site;
                                            LigneComptable.CENTRE = lesCentre.CODE;
                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                         " " + LigneComptable.DATEOPERATION;

                                            LigneComptable.CENTREIMPUTATION = "00000";
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                            {
                                                if (lesCentre.TYPECENTRE == "AG")
                                                {
                                                    CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                    if (leCCaisse != null)
                                                        LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                }
                                                else
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                            }
                                            else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    #region CATEGORIE
                                    if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctCategorie = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                        foreach (var Categorie in lstFactureDistnctCategorie)
                                        {
                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                            if (leCompteSpe != null)
                                            {
                                                CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }

                                                LigneComptable.SITE = lesCentre.CODESITE;
                                                LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
                                                LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;

                                                LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                LigneComptable.SOUSCOMPTE = SousCompte;

                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                else
                                                    LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                                LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                                LigneComptable.NUMPIECE = NumeroPiece;
                                                LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                LigneComptable.INITIAL = Enumere.INITIAL;
                                                LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                LigneComptable.NO = Enumere.NO;
                                                LigneComptable.ZERO = Enumere.ZERO;
                                                LigneComptable.ALPHA = Enumere.ALPHA;
                                                LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                                LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                LigneComptable.USERCREATION = matricule;
                                                LigneComptable.USERMODIFICATION = matricule;
                                                LigneComptable.SITE = Site;
                                                LigneComptable.CENTRE = lesCentre.CODE;
                                                LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                             " " + LigneComptable.DATEOPERATION;

                                                LigneComptable.CENTREIMPUTATION = "00000";
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                                {
                                                    if (lesCentre.TYPECENTRE == "AG")
                                                    {
                                                        CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                        if (leCCaisse != null)
                                                            LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                    }
                                                    else
                                                        LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                }
                                                else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                if (LigneComptable.MONTANT != 0)
                                                {
                                                    CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && LigneComptable.DATECREATION == t.DATECREATION);
                                                    if (laLigneCompte != null)
                                                    {
                                                        laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                        if (LigneComptable.DC == Enumere.Debit)
                                                            laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                        else
                                                            laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                    }
                                                    else
                                                        ListeLigneComptable.Add(LigneComptable);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region PRODUIT
                                    if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctProduit = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                        foreach (var Produit in lstFactureDistnctProduit)
                                        {
                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                            if (leCompteSpe != null)
                                            {
                                                CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }
                                                LigneComptable.SITE = lesCentre.CODESITE;
                                                LigneComptable.CENTRE = lesCentre != null ? (lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE) : string.Empty;
                                                LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                                LigneComptable.NUMPIECE = NumeroPiece;
                                                LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                LigneComptable.SOUSCOMPTE = SousCompte;
                                                LigneComptable.LIBELLEOPERATION = OperationComptat.LIBELLE;

                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                else
                                                    LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                                LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                LigneComptable.DEBIT1 = LigneComptable.DEBIT;

                                                LigneComptable.INITIAL = Enumere.INITIAL;
                                                LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                LigneComptable.NO = Enumere.NO;
                                                LigneComptable.ZERO = Enumere.ZERO;
                                                LigneComptable.ALPHA = Enumere.ALPHA;
                                                LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                                LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                                LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                LigneComptable.USERCREATION = matricule;
                                                LigneComptable.USERMODIFICATION = matricule;
                                                LigneComptable.SITE = Site;
                                                LigneComptable.CENTRE = lesCentre.CODE;
                                                LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                             " " + LigneComptable.DATEOPERATION;
                                                LigneComptable.CENTREIMPUTATION = "00000";
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                                {
                                                    if (lesCentre.TYPECENTRE == "AG")
                                                    {
                                                        CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                        if (leCCaisse != null)
                                                            LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                    }
                                                    else
                                                        LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                }
                                                else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                if (LigneComptable.MONTANT != 0)
                                                {
                                                    CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                    if (laLigneCompte != null)
                                                    {
                                                        laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                        if (LigneComptable.DC == Enumere.Debit)
                                                            laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                        else
                                                            laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                    }
                                                    else
                                                        ListeLigneComptable.Add(LigneComptable);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region REDEVANCE
                                    if (TypeCompte.TABLEFILTRE == "REDEVANCE" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctRedevance = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                        foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                        {
                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                            if (leCompteSpe != null)
                                            {
                                                CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }

                                                LigneComptable.SITE = lesCentre.CODESITE;
                                                LigneComptable.CENTRE = lesCentre != null ? (lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE) : string.Empty;
                                                LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                                LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                LigneComptable.SOUSCOMPTE = SousCompte;
                                                LigneComptable.NUMPIECE = NumeroPiece;
                                                LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                LigneComptable.LIBELLEOPERATION = OperationComptat.LIBELLE;

                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                else
                                                    LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                                LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                LigneComptable.DEBIT1 = LigneComptable.DEBIT;

                                                LigneComptable.INITIAL = Enumere.INITIAL;
                                                LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                LigneComptable.NO = Enumere.NO;
                                                LigneComptable.ZERO = Enumere.ZERO;
                                                LigneComptable.ALPHA = Enumere.ALPHA;
                                                LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                                LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                                LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                LigneComptable.USERCREATION = matricule;
                                                LigneComptable.USERMODIFICATION = matricule;
                                                LigneComptable.SITE = Site;
                                                LigneComptable.CENTRE = lesCentre.CODE;
                                                LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                             " " + LigneComptable.DATEOPERATION;

                                                LigneComptable.CENTREIMPUTATION = "00000";
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                                {
                                                    if (lesCentre.TYPECENTRE == "AG")
                                                    {
                                                        CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                        if (leCCaisse != null)
                                                            LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                    }
                                                    else
                                                        LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                }
                                                else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                if (LigneComptable.MONTANT != 0)
                                                {
                                                    CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                    if (laLigneCompte != null)
                                                    {
                                                        laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                        if (LigneComptable.DC == Enumere.Debit)
                                                            laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                        else
                                                            laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                    }
                                                    else
                                                        ListeLigneComptable.Add(LigneComptable);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    #region PRODUIT & CATEGORIE
                                    if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                        TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctProduit = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                        foreach (var Produit in lstFactureDistnctProduit)
                                        {

                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();


                                            var lstFactureDistnctCategorie = lesFactureOperation.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                            foreach (var Categorie in lstFactureDistnctCategorie)
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE1).ToString());
                                                var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();
                                                var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                                            && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                                            && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {
                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                                u.PRODUIT == VALEURFILTRE &&
                                                                                                                u.CATEGORIE == VALEURFILTRE1
                                                                                                                ).Sum(t => t.MONTANTTTC);

                                                                else
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                    u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                    u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.PRODUIT == VALEURFILTRE &&
                                                                                                                    u.CATEGORIE == VALEURFILTRE1
                                                                                                                    ).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                                u.PRODUIT == VALEURFILTRE &&
                                                                                                                u.CATEGORIE == VALEURFILTRE1
                                                                                                                ).Sum(t => t.MONTANTHT);

                                                                else
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                     u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                     u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                     u.PRODUIT == VALEURFILTRE &&
                                                                                                                     u.DC == TypeCompte.DC &&
                                                                                                                     u.CATEGORIE == VALEURFILTRE1
                                                                                                                     ).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                            u.DATECREATION == lesDate.DATECREATION &&
                                                                                                            u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                            u.PRODUIT == VALEURFILTRE &&
                                                                                                            u.CATEGORIE == VALEURFILTRE1
                                                                                                            ).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                    u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                    u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.PRODUIT == VALEURFILTRE &&
                                                                                                                    u.CATEGORIE == VALEURFILTRE1 &&
                                                                                                                    u.DC == TypeCompte.DC
                                                                                                                    ).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    LigneComptable.SITE = lesCentre.CODESITE;
                                                    LigneComptable.CENTRE = lesCentre != null ? (lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE) : string.Empty;
                                                    LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                    LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                    LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;

                                                    LigneComptable.LIBELLEOPERATION = OperationComptat.LIBELLE;

                                                    LigneComptable.DC = leCompteSpe.DC;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                    else
                                                        LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                                    LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                    LigneComptable.DEBIT1 = LigneComptable.DEBIT;

                                                    LigneComptable.INITIAL = Enumere.INITIAL;
                                                    LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                    LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                    LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                    LigneComptable.NO = Enumere.NO;
                                                    LigneComptable.ZERO = Enumere.ZERO;
                                                    LigneComptable.ALPHA = Enumere.ALPHA;
                                                    LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                    LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                    LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                                    LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                    LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                                    LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                    LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                    LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                    LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                    LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.SITE = Site;
                                                    LigneComptable.CENTRE = lesCentre.CODE;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;
                                                    LigneComptable.CENTREIMPUTATION = "00000";
                                                    if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                                    {
                                                        if (lesCentre.TYPECENTRE == "AG")
                                                        {
                                                            CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                            if (leCCaisse != null)
                                                                LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                        }
                                                        else
                                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                    }
                                                    else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                        LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    #endregion
                                    #region PRODUIT & REDEVANCE & CATEGORIE
                                    if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                        TypeCompte.TABLEFILTRE1 == "REDEVANCE" &&
                                        TypeCompte.TABLEFILTRE2 == "CATEGORIE")
                                    {
                                        var lstFactureDistnctProduit = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                        foreach (var Produit in lstFactureDistnctProduit)
                                        {

                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();

                                            var lstFactureDistnctRedevance = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                            foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE1).ToString());
                                                var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();

                                                var lstFactureDistnctCategorie = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID && u.DATECREATION == lesDate.DATECREATION).Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                                foreach (var Categorie in lstFactureDistnctCategorie)
                                                {
                                                    List<string> Code2 = new List<string>();
                                                    Code2.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE2).ToString());
                                                    var VALEURFILTRE2 = String.Join(" ", Code2.ToArray()).Trim();

                                                    var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                                      && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                                      && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1)
                                                                                                      && c.LSTVALEURFILTRE2.Contains(VALEURFILTRE2)
                                                                                                      );
                                                    if (leCompteSpe != null)
                                                    {
                                                        CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                        string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                        string caseSwitch = TypeValeur;
                                                        switch (caseSwitch)
                                                        {
                                                            case "MONTANTTTC":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                       u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                       u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                                       u.PRODUIT == VALEURFILTRE &&
                                                                                                                       u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                       u.CATEGORIE == VALEURFILTRE2
                                                                                                                       ).Sum(t => t.MONTANTTTC);
                                                                    else
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                  u.DATECREATION == lesDate.DATECREATION &&
                                                                            //u.COPER  == leCompteSpe.COPERASSOCIE &&
                                                                                                                  u.PRODUIT == VALEURFILTRE &&
                                                                                                                  u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                  u.CATEGORIE == VALEURFILTRE2
                                                                                                                  ).Sum(t => t.MONTANTTTC);
                                                                }
                                                                break;
                                                            case "MONTANTHT":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                       u.DATECREATION == lesDate.DATECREATION &&
                                                                            //u.COPERINITAL == leCompteSpe.COPERASSOCIE && 
                                                                                                                       u.PRODUIT == VALEURFILTRE &&
                                                                                                                       u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                       u.CATEGORIE == VALEURFILTRE2
                                                                                                                       ).Sum(t => t.MONTANTHT);
                                                                    else
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                        u.DATECREATION == lesDate.DATECREATION &&
                                                                            //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                                        u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                        u.CATEGORIE == VALEURFILTRE2
                                                                                                                        ).Sum(t => t.MONTANTHT);
                                                                }
                                                                break;
                                                            case "MONTANTTVA":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                     u.DATECREATION == lesDate.DATECREATION &&
                                                                                                                     u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                                     u.PRODUIT == VALEURFILTRE &&
                                                                                                                     u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                     u.CATEGORIE == VALEURFILTRE2
                                                                                                                     ).Sum(t => t.MONTANTTAXE);
                                                                    else
                                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.PK_ID &&
                                                                                                                        u.DATECREATION == lesDate.DATECREATION &&
                                                                            //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                                        u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                        u.CATEGORIE == VALEURFILTRE2
                                                                                                                        ).Sum(t => t.MONTANTTAXE);
                                                                }
                                                                break;
                                                        }

                                                        LigneComptable.SITE = lesCentre.CODESITE;
                                                        LigneComptable.CENTRE = lesCentre.CODE;
                                                        LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                                        LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                                        LigneComptable.NUMPIECE = NumeroPiece;

                                                        LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                                        LigneComptable.SOUSCOMPTE = SousCompte;

                                                        LigneComptable.LIBELLEOPERATION = OperationComptat.LIBELLE;
                                                        LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                                        LigneComptable.DC = leCompteSpe.DC;
                                                        if (LigneComptable.DC == Enumere.Debit)
                                                            LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                                        else
                                                            LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                                        LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                                        LigneComptable.DEBIT1 = LigneComptable.DEBIT;

                                                        LigneComptable.INITIAL = Enumere.INITIAL;
                                                        LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                                        LigneComptable.PROVENACE = Enumere.PROVENACE;
                                                        LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                                        LigneComptable.NO = Enumere.NO;
                                                        LigneComptable.ZERO = Enumere.ZERO;
                                                        LigneComptable.ALPHA = Enumere.ALPHA;
                                                        LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                                        LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                                        LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                                        LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                                        LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                                        LigneComptable.DATEOPERATION = lesDate.DATECREATION.Value.ToShortDateString();
                                                        LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                                        LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                                        LigneComptable.DATECREATION = lesDate.DATECREATION.Value;
                                                        LigneComptable.DATEMODIFICATION = DateTime.Now;
                                                        LigneComptable.USERCREATION = matricule;
                                                        LigneComptable.USERMODIFICATION = matricule;
                                                        LigneComptable.SITE = Site;
                                                        LigneComptable.CENTRE = lesCentre.CODE;
                                                        LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                        LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                     " " + LigneComptable.DATEOPERATION;
                                                        LigneComptable.CENTREIMPUTATION = "00000";
                                                        if (leCompteSpe.CENTREIMPUTATION == "CENTRES")
                                                        {
                                                            if (lesCentre.TYPECENTRE == "AG")
                                                            {
                                                                CsCentreCompte leCCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale && t.DC == "V");
                                                                if (leCCaisse != null)
                                                                    LigneComptable.CENTREIMPUTATION = leCCaisse.CI;
                                                            }
                                                            else
                                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                        }
                                                        else if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                        if (LigneComptable.MONTANT != 0)
                                                        {
                                                            CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                            if (laLigneCompte != null)
                                                            {
                                                                laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                                if (LigneComptable.DC == Enumere.Debit)
                                                                    laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                                else
                                                                    laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                            }
                                                            else
                                                                ListeLigneComptable.Add(LigneComptable);
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                return ListeLigneComptable;
            }
            catch (Exception ex)
            {
                string cpt = CompteL;
                throw ex;
            }
        }
        //public List<CsEcritureComptable> ComptabilisationEncaissement(List<CsComptabilisation> lstEncaissement, List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, string matricule, string Site)
        //{
        //    string CompteL = string.Empty;
        //    try
        //    {
        //        List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();
        //        List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
        //        List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
        //        List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
        //        List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
        //        List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
        //        string LeSite = string.Empty;
        //        List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();

        //        var lstEncaissementDistnctDate = lstEncaissement.Select(t => new { t.DATECAISSE }).Distinct().ToList();
        //        foreach (var Caisse in lstCaisse.OrderBy(t => t.PK_ID).ToList())
        //        {
        //            CsCentre lesCentre = lstCentre.FirstOrDefault(t => t.PK_ID == Caisse.FK_IDCENTRE);
        //            foreach (var lesDate in lstEncaissementDistnctDate.OrderBy(t => t.DATECAISSE).ToList())
        //            {
        //                foreach (CsOperationComptable OperationComptat in lesOperationCpt)
        //                {
        //                    var lstEncaissementDistnctCentreClient = lstEncaissement.Select(t => new { t.CENTRE }).Distinct().ToList();
        //                    List<int> lstCoperCompte = new List<int>();
        //                    List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
        //                    lstCoperCompte.AddRange(lstCompteSpecifiqueOperation.Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
        //                    List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();

        //                    string LibelleActivite = string.Empty;
        //                    string SensOperation = string.Empty;

        //                    if (OperationComptat.LIBELLECOMPTABLE   == "DECAISSEMENT")
        //                        SensOperation = "D";
        //                    else if (OperationComptat.LIBELLECOMPTABLE == "VENTE")
        //                        SensOperation = "V";
        //                    else if (OperationComptat.LIBELLECOMPTABLE == "ENCAISSEMENT")
        //                        SensOperation = "C";

        //                    CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Caisse.CENTRE && t.DC == SensOperation);
        //                    if (leCentreCaisse != null)
        //                        LibelleActivite = leCentreCaisse.LIBELLEACTIVITE;

        //                    foreach (var centreClient in lstEncaissementDistnctCentreClient)
        //                    {
        //                        foreach (CsTypeCompte TypeCompte in lstTypeCompte)
        //                        {
        //                            CsCentreCompte lesCentreCompte = new CsCentreCompte();
        //                            string SousCompte = "000000";
        //                            string NumeroPiece = string.Empty;
        //                            NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;

        //                            CompteL = TypeCompte.CODE;
        //                            if (TypeCompte.SOUSCOMPTE == "CENTRE")
        //                            {
        //                                CsCentre leCentreClient = lstCentre.FirstOrDefault(t => t.CODE == centreClient.CENTRE);
        //                                SousCompte = leCentreClient.TYPECENTRE == "AG" ? "TAG" + leCentreClient.CODE : "TCI" + leCentreClient.CODE;
        //                            }
        //                            else if (TypeCompte.SOUSCOMPTE == "CENCAI")
        //                            {
        //                                if (lesCentre.CODESITE == Enumere.Generale)
        //                                    SousCompte = "TAG010";
        //                                else
        //                                    SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODESITE : "TCI" + lesCentre.CODESITE;

        //                            }
        //                            else if (TypeCompte.SOUSCOMPTE == null) SousCompte = "000000";
        //                            else  SousCompte = leCentreCaisse.CODECOMPTA + Caisse.NUMCAISSE;

        //                            List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
        //                            if (TypeCompte.AVECFILTRE == false)
        //                            {
        //                                #region SANSFILITRE
        //                                CsCompteSpecifique leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID);
        //                                if (leCompteSpe != null)
        //                                {
        //                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                    string caseSwitch = TypeValeur;
        //                                    switch (caseSwitch)
        //                                    {
        //                                        case "MONTANTTTC":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTTC);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
        //                                            }
        //                                            break;
        //                                        case "MONTANTHT":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTHT);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
        //                                            }
        //                                            break;
        //                                        case "MONTANTTVA":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTAXE);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
        //                                            }
        //                                            break;
        //                                    }
        //                                    LigneComptable.SITE = lesCentre.CODESITE;
        //                                    LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
        //                                    LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
        //                                    LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;

        //                                    LigneComptable.FILIERE = leCompteSpe.FILIERE;
        //                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                    LigneComptable.SOUSCOMPTE = SousCompte;

        //                                    LigneComptable.DC = leCompteSpe.DC;
        //                                    if (LigneComptable.DC == Enumere.Debit)
        //                                        LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
        //                                    else
        //                                        LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

        //                                    LigneComptable.CREDIT1 = LigneComptable.CREDIT;
        //                                    LigneComptable.DEBIT1 = LigneComptable.DEBIT;

        //                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                    LigneComptable.NUMPIECE = NumeroPiece;
        //                                    LigneComptable.INITIAL = Enumere.INITIAL;
        //                                    LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
        //                                    LigneComptable.PROVENACE = Enumere.PROVENACE;
        //                                    LigneComptable.NEGATIVE = Enumere.NEGATIVE;
        //                                    LigneComptable.NO = Enumere.NO;
        //                                    LigneComptable.ZERO = Enumere.ZERO;
        //                                    LigneComptable.ALPHA = Enumere.ALPHA;
        //                                    LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
        //                                    LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
        //                                    LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
        //                                    LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
        //                                    LigneComptable.LOCALISATION  = leCompteSpe.LOC ;
        //                                    LigneComptable.LIBRE = leCompteSpe.LIBRE;
        //                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                    LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
        //                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                    LigneComptable.DATEMODIFICATION = DateTime.Now;
        //                                    LigneComptable.USERCREATION = matricule;
        //                                    LigneComptable.USERMODIFICATION = matricule;
        //                                    LigneComptable.SITE = Site;
        //                                    LigneComptable.CENTRE = lesCentre.CODE;
        //                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                 " " + LigneComptable.DATEOPERATION;

        //                                    if (LigneComptable.MONTANT != 0)
        //                                    {
        //                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && 
        //                                                                                                                    t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                    t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                    t.DC == LigneComptable.DC );
        //                                        if (laLigneCompte != null)
        //                                        {
        //                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                            if (LigneComptable.DC == Enumere.Debit)
        //                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                            else
        //                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                        }
        //                                        else
        //                                            ListeLigneComptable.Add(LigneComptable);
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                            else
        //                            {
        //                                #region CATEGORIE
        //                                if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstEncaissementDistnctCategorie = lstEncaissement.Where(u => u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE).Select(t => new { t.CATEGORIE }).Distinct().ToList();
        //                                    foreach (var Categorie in lstEncaissementDistnctCategorie.Where(o=>!string.IsNullOrEmpty(o.CATEGORIE)))
        //                                    {
        //                                        List<string> Code = new List<string>();
        //                                        Code.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
        //                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
                                                     
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && 
        //                                                                u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && 
        //                                                                u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && 
        //                                                                u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }

        //                                            LigneComptable.SITE = lesCentre.CODESITE;
        //                                            LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
        //                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
        //                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;


        //                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

        //                                            LigneComptable.DC = leCompteSpe.DC;
        //                                            if (LigneComptable.DC == Enumere.Debit)
        //                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
        //                                            else
        //                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

        //                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
        //                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;

        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.INITIAL = Enumere.INITIAL;
        //                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
        //                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
        //                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
        //                                            LigneComptable.NO = Enumere.NO;
        //                                            LigneComptable.ZERO = Enumere.ZERO;
        //                                            LigneComptable.ALPHA = Enumere.ALPHA;
        //                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
        //                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
        //                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
        //                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
        //                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.SITE = Site;
        //                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;
        //                                            LigneComptable.CENTRE = lesCentre.CODE;
        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                                                                            t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                            t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                            t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region MODREG
        //                                if (TypeCompte.TABLEFILTRE == "MODEREG" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstEncaissementDistnctModreg = lstEncaissement.Where(u => u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE).Select(t => new { t.MODEREG }).Distinct().ToList();
        //                                    foreach (var Modreg in lstEncaissementDistnctModreg.Where(o=>!string.IsNullOrEmpty( o.MODEREG)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
        //                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }

        //                                            LigneComptable.SITE = lesCentre.CODESITE;
        //                                            LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
        //                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
        //                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;


        //                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

        //                                            LigneComptable.DC = leCompteSpe.DC;
        //                                            if (LigneComptable.DC == Enumere.Debit)
        //                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
        //                                            else
        //                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

        //                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
        //                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;

        //                                            LigneComptable.INITIAL = Enumere.INITIAL;
        //                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
        //                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
        //                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
        //                                            LigneComptable.NO = Enumere.NO;
        //                                            LigneComptable.ZERO = Enumere.ZERO;
        //                                            LigneComptable.ALPHA = Enumere.ALPHA;
        //                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
        //                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
        //                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
        //                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
        //                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.SITE = Site;
        //                                            LigneComptable.CENTRE = lesCentre.CODE;
        //                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;

        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                                                                            t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                            t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                            t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region MODREG & TYPECENTRE
        //                                if (TypeCompte.TABLEFILTRE == "MODEREG" &&
        //                                    TypeCompte.TABLEFILTRE1 == "TYPECENTRE" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstFactureDistnctModreg = lstEncaissement.Where(u => u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE).Select(t => new { t.MODEREG }).Distinct().ToList();
        //                                    foreach (var Modreg in lstFactureDistnctModreg.Where(i=>!string.IsNullOrEmpty(i.MODEREG)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
        //                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID &&
        //                                                                                                    c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
        //                                                                                                    c.LSTVALEURFILTRE1.Contains(lesCentre.TYPECENTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                        u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                        u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                            u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                            u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                        u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                        u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                            u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                            u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE &&
        //                                                                                                    u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                    u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                            u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                            u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (LigneComptable.MONTANT == 0) continue;
        //                                            LigneComptable.SITE = lesCentre != null ? lesCentre.CODESITE : string.Empty;
        //                                            LigneComptable.SITE = lesCentre.CODESITE;
        //                                            LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
        //                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
        //                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;


        //                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

        //                                            LigneComptable.DC = leCompteSpe.DC;
        //                                            if (LigneComptable.DC == Enumere.Debit)
        //                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
        //                                            else
        //                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

        //                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
        //                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;

        //                                            LigneComptable.INITIAL = Enumere.INITIAL;
        //                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
        //                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
        //                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
        //                                            LigneComptable.NO = Enumere.NO;
        //                                            LigneComptable.ZERO = Enumere.ZERO;
        //                                            LigneComptable.ALPHA = Enumere.ALPHA;
        //                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
        //                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
        //                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
        //                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
        //                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.SITE = Site;
        //                                            LigneComptable.CENTRE = lesCentre.CODE;
        //                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;

        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;
        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                     t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                     t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                     t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region COPER & CATEGORIE
        //                                if (TypeCompte.TABLEFILTRE == "COPER" &&
        //                                    TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstFactureDistnctCoper = lstEncaissement.Where(u => u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE).Select(t => new { t.COPER }).Distinct().ToList();
        //                                    foreach (var Modreg in lstFactureDistnctCoper.Where(k=>!string.IsNullOrEmpty(k.COPER )))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();

        //                                        var lstFactureDistnctCateg = lstEncaissement.Where(u => u.FK_IDCAISSE == Caisse.PK_ID && u.DATECAISSE == lesDate.DATECAISSE).Select(t => new { t.CATEGORIE }).Distinct().ToList();
        //                                        foreach (var categ in lstFactureDistnctCateg.Where (k=>!string.IsNullOrEmpty(k.CATEGORIE)))
        //                                        {

        //                                            List<string> Code11 = new List<string>();
        //                                            Code11.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE1).ToString());
        //                                            var VALEURFILTRE1 = String.Join(" ", Code11.ToArray()).Trim();

        //                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID &&
        //                                                                                                        c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
        //                                                                                                        c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
        //                                            if (leCompteSpe != null)
        //                                            {
        //                                                CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                                string caseSwitch = TypeValeur;
        //                                                switch (caseSwitch)
        //                                                {
        //                                                    case "MONTANTTTC":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                            u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                            u.COPER == VALEURFILTRE &&
        //                                                                                                            u.CATEGORIE == VALEURFILTRE1).Sum(t => t.MONTANTTTC);

        //                                                        }
        //                                                        break;
        //                                                    case "MONTANTHT":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE && u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                            u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);


        //                                                        }
        //                                                        break;
        //                                                    case "MONTANTTVA":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissement.Where(u => u.CENTRE == centreClient.CENTRE &&
        //                                                                                                        u.FK_IDCAISSE == Caisse.PK_ID &&
        //                                                                                                        u.DATECAISSE == lesDate.DATECAISSE &&
        //                                                                                                        u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);

        //                                                        }
        //                                                        break;
        //                                                }
        //                                                LigneComptable.SITE = lesCentre != null ? lesCentre.CODESITE : string.Empty;
        //                                                LigneComptable.SITE = lesCentre.CODESITE;
        //                                                LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
        //                                                LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
        //                                                LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;


        //                                                LigneComptable.FILIERE = leCompteSpe.FILIERE;
        //                                                LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                                LigneComptable.SOUSCOMPTE = SousCompte;
        //                                                LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

        //                                                LigneComptable.DC = leCompteSpe.DC;
        //                                                if (LigneComptable.DC == Enumere.Debit)
        //                                                    LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
        //                                                else
        //                                                    LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

        //                                                LigneComptable.CREDIT1 = LigneComptable.CREDIT;
        //                                                LigneComptable.DEBIT1 = LigneComptable.DEBIT;
        //                                                LigneComptable.NUMPIECE = NumeroPiece;

        //                                                LigneComptable.INITIAL = Enumere.INITIAL;
        //                                                LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
        //                                                LigneComptable.PROVENACE = Enumere.PROVENACE;
        //                                                LigneComptable.NEGATIVE = Enumere.NEGATIVE;
        //                                                LigneComptable.NO = Enumere.NO;
        //                                                LigneComptable.ZERO = Enumere.ZERO;
        //                                                LigneComptable.ALPHA = Enumere.ALPHA;
        //                                                LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
        //                                                LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
        //                                                LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
        //                                                LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
        //                                                LigneComptable.LIBRE = leCompteSpe.LIBRE;
        //                                                LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                                LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
        //                                                LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                                LigneComptable.DATEMODIFICATION = DateTime.Now;
        //                                                LigneComptable.USERCREATION = matricule;
        //                                                LigneComptable.USERMODIFICATION = matricule;
        //                                                LigneComptable.SITE = Site;
        //                                                LigneComptable.CENTRE = lesCentre.CODE;
        //                                                LigneComptable.LOCALISATION = leCompteSpe.LOC;

        //                                                LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                                LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                             " " + LigneComptable.DATEOPERATION;
        //                                                if (LigneComptable.MONTANT != 0)
        //                                                {
        //                                                    CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                     t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                     t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                     t.DC == LigneComptable.DC);
        //                                                    if (laLigneCompte != null)
        //                                                    {
        //                                                        laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                        if (LigneComptable.DC == Enumere.Debit)
        //                                                            laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                        else
        //                                                            laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                    }
        //                                                    else
        //                                                        ListeLigneComptable.Add(LigneComptable);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return ListeLigneComptable;
        //    }
        //    catch (Exception ex)
        //    {
        //        string cpt = CompteL;
        //        throw ex;
        //    }
        //}


        //public List<CsEcritureComptable> ComptabilisationEncaissement(List<CsComptabilisation> lstEncaissement, List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, string matricule, string Site)
        //{
        //    string CompteL = string.Empty;
        //    try
        //    {
        //        List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();
        //        List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
        //        List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
        //        List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
        //        List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
        //        List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
        //        string LeSite = string.Empty;
        //        List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();

        //        var lstEncaissementDistnctCaisse = lstEncaissement.Select(t => new { t.FK_IDCAISSE }).Distinct().ToList();
        //        foreach (var LesCaisseEncaisse in lstEncaissementDistnctCaisse)
        //        {
        //            CsCaisse Caisse = lstCaisse.FirstOrDefault(o => o.PK_ID == LesCaisseEncaisse.FK_IDCAISSE);
        //            CsCentre lesCentre = lstCentre.FirstOrDefault(t => t.PK_ID == Caisse.FK_IDCENTRE);

        //            List<CsComptabilisation> lstEncaissementCritereCaisse = lstEncaissement.Where(o => o.FK_IDCAISSE == Caisse.PK_ID).ToList();
        //            if (lstEncaissementCritereCaisse == null || lstEncaissementCritereCaisse.Count == 0) continue;


        //            var lstEncaissementDistnctDate = lstEncaissement.Select(t => new { t.DATECAISSE }).Distinct().ToList();
        //            foreach (var lesDate in lstEncaissementDistnctDate.OrderBy(t => t.DATECAISSE).ToList())
        //            {
        //                foreach (CsOperationComptable OperationComptat in lesOperationCpt)
        //                {
        //                    List<CsComptabilisation> lstEncaissementCritere = lstEncaissementCritereCaisse.Where(o => o.DATECAISSE == lesDate.DATECAISSE).ToList();
        //                    if (lstEncaissementCritere == null || lstEncaissementCritere.Count == 0) continue;

        //                    List<int> lstCoperCompte = new List<int>();
        //                    List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
        //                    lstCoperCompte.AddRange(lstCompteSpecifiqueOperation.Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
        //                    List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();

        //                    string LibelleActivite = string.Empty;
        //                    string SensOperation = string.Empty;

        //                    if (OperationComptat.LIBELLECOMPTABLE == "DECAISSEMENT")
        //                        SensOperation = "D";
        //                    else if (OperationComptat.LIBELLECOMPTABLE == "VENTE")
        //                        SensOperation = "V";
        //                    else if (OperationComptat.LIBELLECOMPTABLE == "ENCAISSEMENT")
        //                        SensOperation = "C";

        //                    CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Caisse.CENTRE && t.DC == SensOperation);
        //                    if (leCentreCaisse != null)
        //                        LibelleActivite = leCentreCaisse.LIBELLEACTIVITE;

        //                    var lstEncaissementDistnctCentreClient = lstEncaissementCritere.Select(t => new { t.CENTRE }).Distinct().ToList();
        //                    foreach (var centreClient in lstEncaissementDistnctCentreClient)
        //                    {

        //                        List<CsComptabilisation> lstEncaissementCritereCentre = lstEncaissementCritere.Where(o => o.CENTRE == centreClient.CENTRE).ToList();
        //                        if (lstEncaissementCritereCentre == null || lstEncaissementCritereCentre.Count == 0) continue;

        //                        foreach (CsTypeCompte TypeCompte in lstTypeCompte)
        //                        {

        //                            List<CsCompteSpecifique> lesComptesSpecDuType = lstCompteSpecifiqueOperation.Where(i => i.FK_IDTYPE_COMPTE == TypeCompte.PK_ID).ToList();
        //                            if (lesComptesSpecDuType == null || lesComptesSpecDuType.Count == 0) continue;

        //                            CsCentreCompte lesCentreCompte = new CsCentreCompte();
        //                            string SousCompte = "000000";
        //                            string NumeroPiece = string.Empty;
        //                            NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;

        //                            CompteL = TypeCompte.CODE;
        //                            if (TypeCompte.SOUSCOMPTE == "CENTRE")
        //                            {
        //                                CsCentre leCentreClient = lstCentre.FirstOrDefault(t => t.CODE == centreClient.CENTRE);
        //                                SousCompte = leCentreClient.TYPECENTRE == "AG" ? "TAG" + leCentreClient.CODE : "TCI" + leCentreClient.CODE;
        //                            }
        //                            else if (TypeCompte.SOUSCOMPTE == "CENCAI")
        //                            {
        //                                if (lesCentre.CODESITE == Enumere.Generale)
        //                                    SousCompte = "TAG010";
        //                                else
        //                                    SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "TCI" + lesCentre.CODE;
        //                                    //SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODESITE : "TCI" + lesCentre.CODESITE;

        //                            }
        //                            else if (TypeCompte.SOUSCOMPTE == null) SousCompte = "000000";
        //                            else SousCompte = leCentreCaisse.CODECOMPTA + Caisse.NUMCAISSE;

        //                            List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
        //                            if (TypeCompte.AVECFILTRE == false)
        //                            {
        //                                #region SANSFILITRE
        //                                CsCompteSpecifique leCompteSpe = lesComptesSpecDuType.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID);
        //                                if (leCompteSpe != null)
        //                                {
        //                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                    string caseSwitch = TypeValeur;
        //                                    switch (caseSwitch)
        //                                    {
        //                                        case "MONTANTTTC":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTTC);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
        //                                            }
        //                                            break;
        //                                        case "MONTANTHT":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTHT);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
        //                                            }
        //                                            break;
        //                                        case "MONTANTTVA":
        //                                            {
        //                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTAXE);
        //                                                else
        //                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
        //                                            }
        //                                            break;
        //                                    }
        //                                    if (LigneComptable.MONTANT == 0) continue;
        //                                    LigneComptable.SOUSCOMPTE = SousCompte;
        //                                    LigneComptable.NUMPIECE = NumeroPiece;
        //                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                    LigneComptable.USERCREATION = matricule;
        //                                    LigneComptable.USERMODIFICATION = matricule;
        //                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                 " " + LigneComptable.DATEOPERATION;

        //                                    LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
        //                                    if (LigneComptable.MONTANT != 0)
        //                                    {
        //                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                                                                    t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                    t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                    t.DC == LigneComptable.DC);
        //                                        if (laLigneCompte != null)
        //                                        {
        //                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                            if (LigneComptable.DC == Enumere.Debit)
        //                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                            else
        //                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                        }
        //                                        else
        //                                            ListeLigneComptable.Add(LigneComptable);
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                            else
        //                            {
        //                                #region CATEGORIE
        //                                if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstEncaissementDistnctCategorie = lstEncaissementCritereCentre.Select(t => new { t.CATEGORIE }).Distinct().ToList();
        //                                    foreach (var Categorie in lstEncaissementDistnctCategorie.Where(o => !string.IsNullOrEmpty(o.CATEGORIE)))
        //                                    {
        //                                        List<string> Code = new List<string>();
        //                                        Code.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
        //                                        var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {

        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (LigneComptable.MONTANT == 0) continue;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);

        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                                                                            t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                            t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                            t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region MODREG
        //                                else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstEncaissementDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
        //                                    foreach (var Modreg in lstEncaissementDistnctModreg.Where(o => !string.IsNullOrEmpty(o.MODEREG)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
        //                                        var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (LigneComptable.MONTANT == 0) continue;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);

        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                                                                            t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                                                                            t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                                                                            t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region COPER & CATEGORIE
        //                                else if (TypeCompte.TABLEFILTRE == "COPER" &&
        //                                   TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
        //                                   string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstFactureDistnctCoper = lstEncaissementCritereCentre.Select(t => new { t.COPER }).Distinct().ToList();
        //                                    foreach (var Modreg in lstFactureDistnctCoper.Where(k => !string.IsNullOrEmpty(k.COPER)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();

        //                                        var lstFactureDistnctCateg = lstEncaissementCritereCentre.Select(t => new { t.CATEGORIE }).Distinct().ToList();
        //                                        foreach (var categ in lstFactureDistnctCateg.Where(k => !string.IsNullOrEmpty(k.CATEGORIE)))
        //                                        {

        //                                            List<string> Code11 = new List<string>();
        //                                            Code11.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE1).ToString());
        //                                            var VALEURFILTRE1 = String.Join(" ", Code11.ToArray()).Trim();

        //                                            var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
        //                                                                                                        c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
        //                                            if (leCompteSpe != null)
        //                                            {
        //                                                CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                                string caseSwitch = TypeValeur;
        //                                                switch (caseSwitch)
        //                                                {
        //                                                    case "MONTANTTTC":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == VALEURFILTRE &&
        //                                                                                                            u.CATEGORIE == VALEURFILTRE1).Sum(t => t.MONTANTTTC);

        //                                                        }
        //                                                        break;
        //                                                    case "MONTANTHT":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);


        //                                                        }
        //                                                        break;
        //                                                    case "MONTANTTVA":
        //                                                        {
        //                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                                LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);

        //                                                        }
        //                                                        break;
        //                                                }
        //                                                if (LigneComptable.MONTANT == 0) continue;
        //                                                LigneComptable.SOUSCOMPTE = SousCompte;
        //                                                LigneComptable.NUMPIECE = NumeroPiece;
        //                                                LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                                LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                                LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                                LigneComptable.USERCREATION = matricule;
        //                                                LigneComptable.USERMODIFICATION = matricule;
        //                                                LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                                LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                                LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                             " " + LigneComptable.DATEOPERATION;

        //                                                LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
        //                                                if (LigneComptable.MONTANT != 0)
        //                                                {
        //                                                    CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                     t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                     t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                     t.DC == LigneComptable.DC);
        //                                                    if (laLigneCompte != null)
        //                                                    {
        //                                                        laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                        if (LigneComptable.DC == Enumere.Debit)
        //                                                            laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                        else
        //                                                            laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                    }
        //                                                    else
        //                                                        ListeLigneComptable.Add(LigneComptable);
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region MODREG & TYPECENTRE
        //                                else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
        //                                    TypeCompte.TABLEFILTRE1 == "TYPECENTRE" &&
        //                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
        //                                {
        //                                    var lstFactureDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
        //                                    foreach (var Modreg in lstFactureDistnctModreg.Where(i => !string.IsNullOrEmpty(i.MODEREG)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
        //                                        var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE != null && c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
        //                                                                                                  c.LSTVALEURFILTRE1 != null && c.LSTVALEURFILTRE1.Contains(lesCentre.TYPECENTRE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (LigneComptable.MONTANT == 0) continue;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                     t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                     t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                     t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                                #region MODREG & TYPECENTRE & CENTRECAIS
        //                                else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
        //                                    TypeCompte.TABLEFILTRE1 == "TYPECENTRE" &&
        //                                    TypeCompte.TABLEFILTRE2 == "CENTRECAIS")
        //                                {
        //                                    var lstFactureDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
        //                                    foreach (var Modreg in lstFactureDistnctModreg.Where(i => !string.IsNullOrEmpty(i.MODEREG)))
        //                                    {
        //                                        List<string> Code1 = new List<string>();
        //                                        Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
        //                                        var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
        //                                        var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE != null && c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
        //                                                                                                   c.LSTVALEURFILTRE1 != null && c.LSTVALEURFILTRE1.Contains(lesCentre.TYPECENTRE) &&
        //                                                                                                   c.LSTVALEURFILTRE2 != null && c.LSTVALEURFILTRE2.Contains(lesCentre.CODE));
        //                                        if (leCompteSpe != null)
        //                                        {
        //                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
        //                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
        //                                            string caseSwitch = TypeValeur;
        //                                            switch (caseSwitch)
        //                                            {
        //                                                case "MONTANTTTC":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTHT":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                        leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);

        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTHT);
        //                                                    }
        //                                                    break;
        //                                                case "MONTANTTVA":
        //                                                    {
        //                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);
        //                                                        else
        //                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
        //                                                                                                            u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
        //                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
        //                                                                                                            u.DC == TypeCompte.DC).Sum(t => t.MONTANTTAXE);
        //                                                    }
        //                                                    break;
        //                                            }
        //                                            if (LigneComptable.MONTANT == 0) continue;
        //                                            LigneComptable.SOUSCOMPTE = SousCompte;
        //                                            LigneComptable.NUMPIECE = NumeroPiece;
        //                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
        //                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
        //                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
        //                                            LigneComptable.USERCREATION = matricule;
        //                                            LigneComptable.USERMODIFICATION = matricule;
        //                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
        //                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
        //                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
        //                                                                         " " + LigneComptable.DATEOPERATION;

        //                                            LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
        //                                            if (LigneComptable.MONTANT != 0)
        //                                            {
        //                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
        //                                                                     t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
        //                                                                     t.DATECREATION == LigneComptable.DATECREATION &&
        //                                                                     t.DC == LigneComptable.DC);
        //                                                if (laLigneCompte != null)
        //                                                {
        //                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
        //                                                    if (LigneComptable.DC == Enumere.Debit)
        //                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
        //                                                    else
        //                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
        //                                                }
        //                                                else
        //                                                    ListeLigneComptable.Add(LigneComptable);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                                #endregion
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return ListeLigneComptable;
        //    }
        //    catch (Exception ex)
        //    {
        //        string cpt = CompteL;
        //        throw ex;
        //    }
        //}


        public List<CsEcritureComptable> ComptabilisationEncaissement(List<CsComptabilisation> lstEncaissement, List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, string matricule, string Site)
        {
            string CompteL = string.Empty;
            try
            {
                List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();
                List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
                List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
                List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
                List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
                string LeSite = string.Empty;
                List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();

                var lstEncaissementDistnctCaisse = lstEncaissement.Select(t => new { t.FK_IDCAISSE }).Distinct().ToList();
                foreach (var LesCaisseEncaisse in lstEncaissementDistnctCaisse)
                {
                    CsCaisse Caisse = lstCaisse.FirstOrDefault(o => o.PK_ID == LesCaisseEncaisse.FK_IDCAISSE);
                    CsCentre lesCentre = lstCentre.FirstOrDefault(t => t.PK_ID == Caisse.FK_IDCENTRE);

                    List<CsComptabilisation> lstEncaissementCritereCaisse = lstEncaissement.Where(o => o.FK_IDCAISSE == Caisse.PK_ID).ToList();
                    if (lstEncaissementCritereCaisse == null || lstEncaissementCritereCaisse.Count == 0) continue;


                    var lstEncaissementDistnctDate = lstEncaissement.Select(t => new { t.DATECAISSE }).Distinct().ToList();
                    foreach (var lesDate in lstEncaissementDistnctDate.OrderBy(t => t.DATECAISSE).ToList())
                    {
                        foreach (CsOperationComptable OperationComptat in lesOperationCpt)
                        {
                            if (OperationComptat == null)
                                continue;

                            List<CsComptabilisation> lstEncaissementCritere = lstEncaissementCritereCaisse.Where(o => o.DATECAISSE == lesDate.DATECAISSE).ToList();
                            if (lstEncaissementCritere == null || lstEncaissementCritere.Count == 0) continue;

                            List<int> lstCoperCompte = new List<int>();
                            List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
                            lstCoperCompte.AddRange(lstCompteSpecifiqueOperation.Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
                            List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();

                            string LibelleActivite = string.Empty;
                            string SensOperation = string.Empty;

                            if (OperationComptat.LIBELLECOMPTABLE == "DECAISSEMENT")
                                SensOperation = "D";
                            else if (OperationComptat.LIBELLECOMPTABLE == "VENTE")
                                SensOperation = "V";
                            else if (OperationComptat.LIBELLECOMPTABLE == "ENCAISSEMENT")
                                SensOperation = "C";

                            CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Caisse.CENTRE && t.DC == SensOperation);
                            if (leCentreCaisse != null)
                                LibelleActivite = leCentreCaisse.LIBELLEACTIVITE;

                            var lstEncaissementDistnctCentreClient = lstEncaissementCritere.Select(t => new { t.CENTRE }).Distinct().ToList();
                            foreach (var centreClient in lstEncaissementDistnctCentreClient)
                            {

                                List<CsComptabilisation> lstEncaissementCritereCentre = lstEncaissementCritere.Where(o => o.CENTRE == centreClient.CENTRE).ToList();
                                if (lstEncaissementCritereCentre == null || lstEncaissementCritereCentre.Count == 0) continue;

                                foreach (CsTypeCompte TypeCompte in lstTypeCompte)
                                {

                                    List<CsCompteSpecifique> lesComptesSpecDuType = lstCompteSpecifiqueOperation.Where(i => i.FK_IDTYPE_COMPTE == TypeCompte.PK_ID).ToList();
                                    if (lesComptesSpecDuType == null || lesComptesSpecDuType.Count == 0) continue;

                                    CsCentreCompte lesCentreCompte = new CsCentreCompte();
                                    string SousCompte = "000000";
                                    string NumeroPiece = string.Empty;
                                    NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;

                                    CompteL = TypeCompte.CODE;

                                    if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                    {
                                        CsCentre leCentreClient = lstCentre.FirstOrDefault(t => t.CODE == centreClient.CENTRE);
                                        SousCompte = leCentreClient.TYPECENTRE == "AG" ? "TAG" + leCentreClient.CODE : "TCI" + leCentreClient.CODE;

                                    }
                                    else if (TypeCompte.SOUSCOMPTE == "CENCAI")
                                    {
                                        if (lesCentre.CODESITE == Enumere.Generale)
                                            SousCompte = "TAG010";
                                        else if (lesCentre.TYPECENTRE == "CE" && TypeCompte.TABLEFILTRE2 == "CENTRECAIS")
                                            SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "BCI" + lesCentre.CODE;
                                        else
                                            SousCompte = lesCentre.TYPECENTRE == "AG" ? "TAG" + lesCentre.CODE : "TCI" + lesCentre.CODE;

                                    }
                                    else if (TypeCompte.SOUSCOMPTE == null) SousCompte = "000000";
                                    else SousCompte = leCentreCaisse.CODECOMPTA + Caisse.NUMCAISSE;

                                    List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
                                    if (TypeCompte.AVECFILTRE == false)
                                    {
                                        #region SANSFILITRE
                                        CsCompteSpecifique leCompteSpe = lesComptesSpecDuType.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID);
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            if (LigneComptable.MONTANT == 0) continue;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.NUMPIECE = NumeroPiece;
                                            LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                            LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                         " " + LigneComptable.DATEOPERATION;

                                            LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                                                                            t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                                                                            t.DATECREATION == LigneComptable.DATECREATION &&
                                                                                                                            t.DC == LigneComptable.DC);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region CATEGORIE
                                        if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
                                            string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                            string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                        {
                                            var lstEncaissementDistnctCategorie = lstEncaissementCritereCentre.Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                            foreach (var Categorie in lstEncaissementDistnctCategorie.Where(o => !string.IsNullOrEmpty(o.CATEGORIE)))
                                            {
                                                List<string> Code = new List<string>();
                                                Code.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE).ToString());
                                                var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                                var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {

                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                        u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    if (LigneComptable.MONTANT == 0) continue;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;

                                                    LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);

                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                                                                                    t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                                                                                    t.DATECREATION == LigneComptable.DATECREATION &&
                                                                                                                                    t.DC == LigneComptable.DC);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region MODREG
                                        else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
                                            string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                            string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                        {
                                            var lstEncaissementDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
                                            foreach (var Modreg in lstEncaissementDistnctModreg.Where(o => !string.IsNullOrEmpty(o.MODEREG)))
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
                                                var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
                                                var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {
                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.MODEREG == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    if (LigneComptable.MONTANT == 0) continue;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;

                                                    LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);

                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                                                                                    t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                                                                                    t.DATECREATION == LigneComptable.DATECREATION &&
                                                                                                                                    t.DC == LigneComptable.DC);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region COPER & CATEGORIE
                                        else if (TypeCompte.TABLEFILTRE == "COPER" &&
                                           TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
                                           string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                        {
                                            var lstFactureDistnctCoper = lstEncaissementCritereCentre.Select(t => new { t.COPER }).Distinct().ToList();
                                            foreach (var Modreg in lstFactureDistnctCoper.Where(k => !string.IsNullOrEmpty(k.COPER)))
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
                                                var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();

                                                var lstFactureDistnctCateg = lstEncaissementCritereCentre.Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                                foreach (var categ in lstFactureDistnctCateg.Where(k => !string.IsNullOrEmpty(k.CATEGORIE)))
                                                {

                                                    List<string> Code11 = new List<string>();
                                                    Code11.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE1).ToString());
                                                    var VALEURFILTRE1 = String.Join(" ", Code11.ToArray()).Trim();

                                                    var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
                                                                                                                c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
                                                    if (leCompteSpe != null)
                                                    {
                                                        CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                        string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                        string caseSwitch = TypeValeur;
                                                        switch (caseSwitch)
                                                        {
                                                            case "MONTANTTTC":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == VALEURFILTRE &&
                                                                                                                    u.CATEGORIE == VALEURFILTRE1).Sum(t => t.MONTANTTTC);

                                                                }
                                                                break;
                                                            case "MONTANTHT":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);


                                                                }
                                                                break;
                                                            case "MONTANTTVA":
                                                                {
                                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                        LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);

                                                                }
                                                                break;
                                                        }
                                                        if (LigneComptable.MONTANT == 0) continue;
                                                        LigneComptable.SOUSCOMPTE = SousCompte;
                                                        LigneComptable.NUMPIECE = NumeroPiece;
                                                        LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                                        LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                                        LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                                        LigneComptable.USERCREATION = matricule;
                                                        LigneComptable.USERMODIFICATION = matricule;
                                                        LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                                        LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                        LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                     " " + LigneComptable.DATEOPERATION;

                                                        LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
                                                        if (LigneComptable.MONTANT != 0)
                                                        {
                                                            CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                             t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                             t.DATECREATION == LigneComptable.DATECREATION &&
                                                                             t.DC == LigneComptable.DC);
                                                            if (laLigneCompte != null)
                                                            {
                                                                laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                                if (LigneComptable.DC == Enumere.Debit)
                                                                    laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                                else
                                                                    laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                            }
                                                            else
                                                                ListeLigneComptable.Add(LigneComptable);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region MODREG & TYPECENTRE
                                        else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
                                            TypeCompte.TABLEFILTRE1 == "TYPECENTRE" &&
                                            string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                        {
                                            var lstFactureDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
                                            foreach (var Modreg in lstFactureDistnctModreg.Where(i => !string.IsNullOrEmpty(i.MODEREG)))
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
                                                var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
                                                var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE != null && c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
                                                                                                          c.LSTVALEURFILTRE1 != null && c.LSTVALEURFILTRE1.Contains(lesCentre.TYPECENTRE));
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {
                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);

                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);

                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
                                                                                                                    u.DC == TypeCompte.DC).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
                                                                                                                    u.DC == TypeCompte.DC).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    if (LigneComptable.MONTANT == 0) continue;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;

                                                    LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                             t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                             t.DATECREATION == LigneComptable.DATECREATION &&
                                                                             t.DC == LigneComptable.DC);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region MODREG & TYPECENTRE & CENTRECAIS
                                        else if (TypeCompte.TABLEFILTRE == "MODEREG" &&
                                            TypeCompte.TABLEFILTRE1 == "TYPECENTRE" &&
                                            TypeCompte.TABLEFILTRE2 == "CENTRECAIS")
                                        {
                                            var lstFactureDistnctModreg = lstEncaissementCritereCentre.Select(t => new { t.MODEREG }).Distinct().ToList();
                                            foreach (var Modreg in lstFactureDistnctModreg.Where(i => !string.IsNullOrEmpty(i.MODEREG)))
                                            {
                                                List<string> Code1 = new List<string>();
                                                Code1.Add(GetPropValue(Modreg, TypeCompte.TABLEFILTRE).ToString());
                                                var VALEURFILTRE = String.Join(" ", Code1.ToArray()).Trim();
                                                var leCompteSpe = lesComptesSpecDuType.FirstOrDefault(c => c.LSTVALEURFILTRE != null && c.LSTVALEURFILTRE.Contains(VALEURFILTRE) &&
                                                                                                           c.LSTVALEURFILTRE1 != null && c.LSTVALEURFILTRE1.Contains(lesCentre.TYPECENTRE) &&
                                                                                                           c.LSTVALEURFILTRE2 != null && c.LSTVALEURFILTRE2.Contains(lesCentre.CODE));
                                                if (leCompteSpe != null)
                                                {
                                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                    string caseSwitch = TypeValeur;
                                                    switch (caseSwitch)
                                                    {
                                                        case "MONTANTTTC":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);

                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTTC);
                                                            }
                                                            break;
                                                        case "MONTANTHT":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTHT);

                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
                                                                                                                    u.DC == TypeCompte.DC).Sum(t => t.MONTANTHT);
                                                            }
                                                            break;
                                                        case "MONTANTTVA":
                                                            {
                                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                            leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG)).Sum(t => t.MONTANTTAXE);
                                                                else
                                                                    LigneComptable.MONTANT = lstEncaissementCritereCentre.Where(u => u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                    u.COPERINITAL == OperationComptat.COPERIDENTIFIANT &&
                                                                                                                    leCompteSpe.LSTVALEURFILTRE.Contains(u.MODEREG) &&
                                                                                                                    u.DC == TypeCompte.DC).Sum(t => t.MONTANTTAXE);
                                                            }
                                                            break;
                                                    }
                                                    if (LigneComptable.MONTANT == 0) continue;
                                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                                    LigneComptable.NUMPIECE = NumeroPiece;
                                                    LigneComptable.CAISSE = Caisse.NUMCAISSE;
                                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                                    LigneComptable.DATEOPERATION = lesDate.DATECAISSE.Value.ToShortDateString();
                                                    LigneComptable.USERCREATION = matricule;
                                                    LigneComptable.USERMODIFICATION = matricule;
                                                    LigneComptable.DATECREATION = lesDate.DATECAISSE.Value;
                                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                                 " " + LigneComptable.DATEOPERATION;

                                                    LigneComptable = CompleteEcriture(LigneComptable, lesCentre, leCompteSpe);
                                                    if (LigneComptable.MONTANT != 0)
                                                    {
                                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE &&
                                                                             t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE &&
                                                                             t.DATECREATION == LigneComptable.DATECREATION &&
                                                                             t.DC == LigneComptable.DC);
                                                        if (laLigneCompte != null)
                                                        {
                                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                            if (LigneComptable.DC == Enumere.Debit)
                                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                            else
                                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                        }
                                                        else
                                                            ListeLigneComptable.Add(LigneComptable);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }
                return ListeLigneComptable;
            }
            catch (Exception ex)
            {
                string cpt = CompteL;
                throw ex;
            }
        }













        private CsEcritureComptable CompleteEcriture(CsEcritureComptable LigneComptable, CsCentre lesCentre, CsCompteSpecifique leCompteSpe)
        {

            LigneComptable.SITE = lesCentre != null ? lesCentre.CODESITE : string.Empty;
            LigneComptable.SITE = lesCentre.CODESITE;
            LigneComptable.CENTRE = lesCentre.LIBELLE.Length >= 3 ? lesCentre.LIBELLE.Substring(0, 3) : lesCentre.LIBELLE;
            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
            LigneComptable.FILIERE = leCompteSpe.FILIERE;
            LigneComptable.DC = leCompteSpe.DC;
            if (LigneComptable.DC == Enumere.Debit)
                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
            else
                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
            LigneComptable.INITIAL = Enumere.INITIAL;
            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
            LigneComptable.PROVENACE = Enumere.PROVENACE;
            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
            LigneComptable.NO = Enumere.NO;
            LigneComptable.ZERO = Enumere.ZERO;
            LigneComptable.ALPHA = Enumere.ALPHA;
            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
            LigneComptable.LIBRE = leCompteSpe.LIBRE;
            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
            LigneComptable.DATEMODIFICATION = DateTime.Now;
            LigneComptable.LOCALISATION = leCompteSpe.LOC;

            return LigneComptable;

        }


        public List<CsEcritureComptable> ComptabilisationGC(List<CsComptabilisation> lstEncaissement, CsOperationComptable OperationComptat, string matricule, string Site)
        {
            string CompteL = string.Empty;
            try
            {
                List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();
                List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
                List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
                List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
                List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
                string LeSite = string.Empty;
                List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();

                var lstEncaissementDistnctMandat = lstEncaissement.Select(t => new { t.NUMEROMANDATEMENT  }).Distinct().ToList();
                foreach (var lesMandat in lstEncaissementDistnctMandat.OrderBy(t => t.NUMEROMANDATEMENT).ToList())
                {
                    List<CsComptabilisation> lstEncaissementMandatement = lstEncaissement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT).ToList();
                    var lstEncaissementDistnctCentreClient = lstEncaissementMandatement.Select(t => new { t.CENTRE }).Distinct().ToList();
                    List<int> lstCoperCompte = new List<int>();
                    List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
                    lstCoperCompte.AddRange(lstCompteSpecifiqueOperation.Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
                    List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();

                    CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == Enumere.Generale  && t.DC == "C");

                    string LibelleActivite = string.Empty;
                    foreach (var centreClient in lstEncaissementDistnctCentreClient)
                    {
                        List<CsComptabilisation> lstEncaissementCentre = lstEncaissementMandatement.Where(u => u.CENTRE == centreClient.CENTRE).ToList();
                        foreach (CsTypeCompte TypeCompte in lstTypeCompte)
                        {
                            CsCentreCompte lesCentreCompte = new CsCentreCompte();
                            string SousCompte = "000000";
                            string NumeroPiece = string.Empty;

                            CompteL = TypeCompte.CODE;
                            if (TypeCompte.SOUSCOMPTE == "CENTRE")
                            {
                                CsCentre leCentreClient = lstCentre.FirstOrDefault(t => t.CODE == centreClient.CENTRE);
                                SousCompte = leCentreClient.TYPECENTRE == "AG" ? "TAG" + leCentreClient.CODE : "TCI" + leCentreClient.CODE;
                            }
                            NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;

                            List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
                            if (TypeCompte.AVECFILTRE == false)
                            {
                                #region SANSFILITRE
                                CsCompteSpecifique leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID);
                                if (leCompteSpe != null)
                                {
                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                    string caseSwitch = TypeValeur;
                                    switch (caseSwitch)
                                    {
                                        case "MONTANTTTC":
                                            {
                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u =>u.NUMEROMANDATEMENT ==lesMandat.NUMEROMANDATEMENT && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTTC);
                                                else
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                            }
                                            break;
                                        case "MONTANTHT":
                                            {
                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTHT);
                                                else
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                            }
                                            break;
                                        case "MONTANTTVA":
                                            {
                                                if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && u.COPERINITAL == OperationComptat.COPERIDENTIFIANT).Sum(t => t.MONTANTTAXE);
                                                else
                                                    LigneComptable.MONTANT = lstEncaissementMandatement.Where(u => u.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                            }
                                            break;
                                    }

                                    if (leCompteSpe.SOUSCOMPTE == "CENCAI")
                                    SousCompte = "TAG010";

                                    LigneComptable.SITE = "000";
                                    LigneComptable.CENTRE = "000";
                                    LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                    LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                    LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                    LigneComptable.SOUSCOMPTE = SousCompte;

                                    LigneComptable.DC = leCompteSpe.DC;
                                    if (LigneComptable.DC == Enumere.Debit)
                                        LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                    else
                                        LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                    LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                    LigneComptable.DEBIT1 = LigneComptable.DEBIT;

                                    LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;
                                    LigneComptable.NUMPIECE = NumeroPiece;
                                    LigneComptable.INITIAL = Enumere.INITIAL;
                                    LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                    LigneComptable.PROVENACE = Enumere.PROVENACE;
                                    LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                    LigneComptable.NO = Enumere.NO;
                                    LigneComptable.ZERO = Enumere.ZERO;
                                    LigneComptable.ALPHA = Enumere.ALPHA;
                                    LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                    LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                    LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                    LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                    LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                    LigneComptable.DATEOPERATION = lstEncaissementMandatement.FirstOrDefault().DATECAISSE.Value.ToShortDateString();
                                    LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                    LigneComptable.DATECREATION = lstEncaissementMandatement.FirstOrDefault().DATECAISSE.Value;
                                    LigneComptable.DATEMODIFICATION = DateTime.Now;
                                    LigneComptable.USERCREATION = matricule;
                                    LigneComptable.USERMODIFICATION = matricule;
                                    LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                    LigneComptable.NUMEROMANDATEMENT  = lesMandat.NUMEROMANDATEMENT ;
                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = leCentreCaisse.LIBELLEACTIVITE;

                                    //LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = "MISE A JOUR SCA " + lesMandat.NUMEROMANDATEMENT;
                                    //LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                    //                             " " + lesMandat.NUMEROMANDATEMENT;
                                    LigneComptable.DESCRIPTIONLIGNE = leCentreCaisse.LIBELLEACTIVITE + " " + lesMandat.NUMEROMANDATEMENT;
                                    if (LigneComptable.MONTANT != 0)
                                    {
                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t =>t.NUMEROMANDATEMENT == lesMandat .NUMEROMANDATEMENT && t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && LigneComptable.DATECREATION == t.DATECREATION);
                                        if (laLigneCompte != null)
                                        {
                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                            else
                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                        }
                                        else
                                            ListeLigneComptable.Add(LigneComptable);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region CATEGORIE
                                if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                {
                                    var lstEncaissementDistnctCategorie = lstEncaissementCentre.Select(t => new { t.CATEGORIE }).Distinct().ToList();
                                    foreach (var Categorie in lstEncaissementDistnctCategorie)
                                    {
                                        List<CsComptabilisation> lstEncaissementCategorie = lstEncaissementCentre.Where(u => u.CATEGORIE == Categorie.CATEGORIE).ToList();
                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(Categorie, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(leCompteSpe.VALEURMONTANT) && leCompteSpe.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstEncaissementCategorie.Where(u => u.COPERINITAL == OperationComptat.COPERIDENTIFIANT && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            if (leCompteSpe.SOUSCOMPTE == "CENCAI")
                                                SousCompte = "TAG010";

                                            LigneComptable.SITE = "000";
                                            LigneComptable.CENTRE = "000";
                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.FK_IDOPERATION = OperationComptat.PK_ID;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                            else
                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;

                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                            LigneComptable.NUMPIECE = NumeroPiece;

                                            LigneComptable.INITIAL = Enumere.INITIAL;
                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                            LigneComptable.NO = Enumere.NO;
                                            LigneComptable.ZERO = Enumere.ZERO;
                                            LigneComptable.ALPHA = Enumere.ALPHA;
                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                            LigneComptable.DATEOPERATION = lstEncaissementCentre.FirstOrDefault().DATECAISSE.Value.ToShortDateString();
                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATECREATION = lstEncaissementCentre.FirstOrDefault().DATECAISSE.Value;
                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.SITE = "000";
                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                            LigneComptable.NUMEROMANDATEMENT = lesMandat.NUMEROMANDATEMENT ;
                                            LigneComptable.DESCRIPSTIONOPERATION =LigneComptable.LIBELLEOPERATION =  leCentreCaisse.LIBELLEACTIVITE;
                                            //LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = "MISE A JOUR SCA " + lesMandat.NUMEROMANDATEMENT;
                                            //LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                            //                             " " + lesMandat.NUMEROMANDATEMENT;
                                            LigneComptable.DESCRIPTIONLIGNE = leCentreCaisse.LIBELLEACTIVITE + " " + lesMandat.NUMEROMANDATEMENT ;
                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.NUMEROMANDATEMENT == lesMandat.NUMEROMANDATEMENT && t.COMPTE == LigneComptable.COMPTE && t.SOUSCOMPTE == LigneComptable.SOUSCOMPTE && LigneComptable.DATECREATION == t.DATECREATION);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }

                }
                return ListeLigneComptable;
            }
            catch (Exception ex)
            {
                string cpt = CompteL;
                throw ex;
            }
        }
        public List<CsEcritureComptable> ComptabilisationFactureEmissionGneral(List<int> Idcentre, List<CsComptabilisation> lstFacture, string matricule, string Site, string MoisCpt)
        {

            List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
            List<CsOperationComptable> ListeOperationComptable = new DbInterfaceComptable().RetourneOperationComptable();
            List<CsOperationComptable> LstOperationCompableSelect = ListeOperationComptable.Where(t => t.CODE == "03").ToList();
            List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
            List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
            List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
            List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
            string LeSite = string.Empty;
            List<CsEcritureComptable> ListeLigneComptable = new List<CsEcritureComptable>();

            string DateComptat = Galatee.Tools.Utility.DernierJourDuMois(int.Parse(MoisCpt.Substring(4, 2)), int.Parse(MoisCpt.Substring(0, 4)));

            foreach (CsOperationComptable OperationComptat in LstOperationCompableSelect)
            {
                var lstFactureDistnctCentre = lstFacture.Select(t => new { t.FK_IDCENTRE, t.CENTRE }).Distinct().ToList();
                foreach (var lesCentre in lstFactureDistnctCentre)
                {
                    CsCentre leCentre = lstCentre.FirstOrDefault(y => y.PK_ID == lesCentre.FK_IDCENTRE);
                    LeSite = leCentre.LIBELLESITE;
                    List<int> lstCoperCompte = new List<int>();
                    List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
                    lstCoperCompte.AddRange(ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
                    List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();
                    List<CsComptabilisation> lesFactureOperation = lstFacture.Where(t => t.FK_IDCENTRE == lesCentre.FK_IDCENTRE).ToList();
                    List<CsRedevance> ListeRedevance = new DBCalcul().ChargerRedevance();

                    string LibelleActivite = string.Empty;
                
                    CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == lesCentre.CENTRE && t.DC == "V");
                    if (leCentreCaisse != null)
                    {
                        if (leCentre.CODESITE == Enumere.CodeSiteScaMT || leCentre.CODESITE == Enumere.CodeSiteScaBT)
                        {
                            if (leCentre.CODESITE == Enumere.CodeSiteScaMT)
                            LibelleActivite = "VENTES GALATEE SCA" + " MT";
                            else
                                LibelleActivite = "VENTES GALATEE SCA" ;
                        }
                        else
                            LibelleActivite = leCentreCaisse.LIBELLEACTIVITE;
                    }

                    var lstFactureDistnctCategorie = lstFacture.Select(t => new { t.CATEGORIE }).Distinct().ToList();
                    foreach (var categ in lstFactureDistnctCategorie)
                    {
                        List<CsComptabilisation> lstFactureCategorie = lesFactureOperation.Where(t => t.CATEGORIE == categ.CATEGORIE).ToList();
                        foreach (CsTypeCompte TypeCompte in lstTypeCompte)
                        {
                            string SousCompte = "000000";
                            if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                SousCompte = leCentre.TYPECENTRE == "AG" ? "TAG" + leCentre.CODE : "TCI" + leCentre.CODE;

                            string NumeroPiece = string.Empty;
                            if (leCentre.CODESITE == Enumere.CodeSiteScaMT)
                                NumeroPiece = leCentreCaisse.CODEACTIVITE + " " +"SCA MT";
                            else if (leCentre.CODESITE == Enumere.CodeSiteScaBT)
                                    NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + "SCA";
                            else  NumeroPiece = leCentreCaisse.CODEACTIVITE + " " + leCentreCaisse.CODECOMPTA;
                          

                            List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
                            if (TypeCompte.AVECFILTRE == false)
                            {
                                #region SANSFILTRE
                                CsCompteSpecifique leCompteSpe = ListeCompteSpecifique.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID);
                                if (leCompteSpe != null)
                                {
                                    CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                    string TypeValeur = leCompteSpe.VALEURMONTANT;
                                    string caseSwitch = TypeValeur;
                                    switch (caseSwitch)
                                    {
                                        case "MONTANTTTC":
                                            {
                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                else
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                            }
                                            break;
                                        case "MONTANTHT":
                                            {
                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                else
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                            }
                                            break;
                                        case "MONTANTTVA":
                                            {
                                                if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                else
                                                    LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                            }
                                            break;
                                    }
                                    LigneComptable.SITE = leCentre.CODESITE;
                                    LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                    LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                    LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                    LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                    LigneComptable.SOUSCOMPTE = SousCompte;
                                    LigneComptable.NUMPIECE = NumeroPiece;

                                    LigneComptable.DC = leCompteSpe.DC;
                                    if (LigneComptable.DC == Enumere.Debit)
                                        LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                    else
                                        LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                    LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                    LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                    LigneComptable.INITIAL = Enumere.INITIAL;
                                    LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                    LigneComptable.PROVENACE = Enumere.PROVENACE;
                                    LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                    LigneComptable.NO = Enumere.NO;
                                    LigneComptable.ZERO = Enumere.ZERO;
                                    LigneComptable.ALPHA = Enumere.ALPHA;
                                    LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                    LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                    LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                    LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                    LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                    LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                    LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                    LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                    LigneComptable.DATECREATION = DateTime.Now;
                                    LigneComptable.DATEMODIFICATION = DateTime.Now;
                                    LigneComptable.USERCREATION = matricule;
                                    LigneComptable.USERMODIFICATION = matricule;
                                    LigneComptable.SITE = Site;
                                    LigneComptable.CENTRE = leCentre.CODE;
                                    LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                    LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                 " " + DateComptat;
                                    LigneComptable.CENTREIMPUTATION = "00000";
                                    if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                        LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                    if (LigneComptable.MONTANT != 0)
                                    {
                                        CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                        if (laLigneCompte != null)
                                        {
                                            laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                            else
                                                laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                        }
                                        else
                                            ListeLigneComptable.Add(LigneComptable);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region CATEGORIE
                                if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                {

                                    List<string> Code = new List<string>();
                                    Code.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE).ToString());
                                    var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                    var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                    if (leCompteSpe != null)
                                    {
                                        CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                        string TypeValeur = leCompteSpe.VALEURMONTANT;
                                        string caseSwitch = TypeValeur;
                                        switch (caseSwitch)
                                        {
                                            case "MONTANTTTC":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                }
                                                break;
                                            case "MONTANTHT":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                }
                                                break;
                                            case "MONTANTTVA":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                }
                                                break;
                                        }
                                        LigneComptable.SITE = leCentre.CODESITE;
                                        LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                        LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                        LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                        LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                        LigneComptable.SOUSCOMPTE = SousCompte;
                                        LigneComptable.NUMPIECE = NumeroPiece;

                                        LigneComptable.DC = leCompteSpe.DC;
                                        if (LigneComptable.DC == Enumere.Debit)
                                            LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                        else
                                            LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                        LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                        LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                        LigneComptable.INITIAL = Enumere.INITIAL;
                                        LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                        LigneComptable.PROVENACE = Enumere.PROVENACE;
                                        LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                        LigneComptable.NO = Enumere.NO;
                                        LigneComptable.ZERO = Enumere.ZERO;
                                        LigneComptable.ALPHA = Enumere.ALPHA;
                                        LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                        LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                        LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                        LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                        LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                        LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                        LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                        LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                        LigneComptable.DATECREATION = DateTime.Now;
                                        LigneComptable.DATEMODIFICATION = DateTime.Now;
                                        LigneComptable.USERCREATION = matricule;
                                        LigneComptable.USERMODIFICATION = matricule;
                                        LigneComptable.SITE = Site;
                                        LigneComptable.CENTRE = leCentre.CODE;
                                        LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                        LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                     " " + DateComptat;
                                        LigneComptable.CENTREIMPUTATION = "00000";
                                        if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                        if (LigneComptable.MONTANT != 0)
                                        {
                                            CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                            if (laLigneCompte != null)
                                            {
                                                laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                else
                                                    laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                            }
                                            else
                                                ListeLigneComptable.Add(LigneComptable);
                                        }
                                    }

                                }
                                #endregion
                                #region PRODUIT
                                if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                {
                                    var lstFactureDistnctProduit = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                    foreach (var Produit in lstFactureDistnctProduit)
                                    {
                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            LigneComptable.SITE = leCentre.CODESITE;
                                            LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.NUMPIECE = NumeroPiece;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                            else
                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                            LigneComptable.INITIAL = Enumere.INITIAL;
                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                            LigneComptable.NO = Enumere.NO;
                                            LigneComptable.ZERO = Enumere.ZERO;
                                            LigneComptable.ALPHA = Enumere.ALPHA;
                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;

                                            LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATECREATION = DateTime.Now;
                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.SITE = Site;
                                            LigneComptable.CENTRE = leCentre.CODE;
                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                         " " + DateComptat;
                                            LigneComptable.CENTREIMPUTATION = "00000";
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #region REDEVANCE
                                if (TypeCompte.TABLEFILTRE == "REDEVANCE" &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                {
                                    var lstFactureDistnctRedevance = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                    foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                    {
                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            LigneComptable.SITE = leCentre.CODESITE;
                                            LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.NUMPIECE = NumeroPiece;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                            else
                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                            LigneComptable.INITIAL = Enumere.INITIAL;
                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                            LigneComptable.NO = Enumere.NO;
                                            LigneComptable.ZERO = Enumere.ZERO;
                                            LigneComptable.ALPHA = Enumere.ALPHA;
                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                            LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATECREATION = DateTime.Now;
                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.SITE = Site;
                                            LigneComptable.CENTRE = leCentre.CODE;
                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                         " " + DateComptat;
                                            LigneComptable.CENTREIMPUTATION = "00000";
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }
                                    }
                                }
                            }
                                #endregion
                                #region PRODUIT & CATEGORIE
                            if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
                                string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                            {
                                var lstFactureDistnctProduit = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                foreach (var Produit in lstFactureDistnctProduit)
                                {

                                    List<string> Code = new List<string>();
                                    Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                    var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();


                                    List<string> Code1 = new List<string>();
                                    Code1.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE1).ToString());
                                    var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();
                                    var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                                && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                                && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
                                    if (leCompteSpe != null)
                                    {
                                        CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                        string TypeValeur = leCompteSpe.VALEURMONTANT;
                                        string caseSwitch = TypeValeur;
                                        switch (caseSwitch)
                                        {
                                            case "MONTANTTTC":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                    u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                    u.PRODUIT == VALEURFILTRE &&
                                                                                                    u.CATEGORIE == VALEURFILTRE1
                                                                                                    ).Sum(t => t.MONTANTTTC);

                                                    else
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                        u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                        u.CATEGORIE == VALEURFILTRE1
                                                                                                        ).Sum(t => t.MONTANTTTC);
                                                }
                                                break;
                                            case "MONTANTHT":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                    u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                    u.PRODUIT == VALEURFILTRE &&
                                                                                                    u.CATEGORIE == VALEURFILTRE1
                                                                                                    ).Sum(t => t.MONTANTHT);

                                                    else
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                         u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                         u.PRODUIT == VALEURFILTRE &&
                                                                                                         u.DC == TypeCompte.DC &&
                                                                                                         u.CATEGORIE == VALEURFILTRE1
                                                                                                         ).Sum(t => t.MONTANTHT);
                                                }
                                                break;
                                            case "MONTANTTVA":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                u.PRODUIT == VALEURFILTRE &&
                                                                                                u.CATEGORIE == VALEURFILTRE1
                                                                                                ).Sum(t => t.MONTANTTAXE);
                                                    else
                                                        LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                        u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                        u.CATEGORIE == VALEURFILTRE1 &&
                                                                                                        u.DC == TypeCompte.DC
                                                                                                        ).Sum(t => t.MONTANTTAXE);
                                                }
                                                break;
                                        }
                                        LigneComptable.SITE = leCentre.CODESITE;
                                        LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                        LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                        LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                        LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                        LigneComptable.SOUSCOMPTE = SousCompte;
                                        LigneComptable.NUMPIECE = NumeroPiece;

                                        LigneComptable.DC = leCompteSpe.DC;
                                        if (LigneComptable.DC == Enumere.Debit)
                                            LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                        else
                                            LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                        LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                        LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                        LigneComptable.INITIAL = Enumere.INITIAL;
                                        LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                        LigneComptable.PROVENACE = Enumere.PROVENACE;
                                        LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                        LigneComptable.NO = Enumere.NO;
                                        LigneComptable.ZERO = Enumere.ZERO;
                                        LigneComptable.ALPHA = Enumere.ALPHA;
                                        LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                        LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                        LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                        LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                        LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                        LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                        LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                        LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                        LigneComptable.DATECREATION = DateTime.Now;
                                        LigneComptable.DATEMODIFICATION = DateTime.Now;
                                        LigneComptable.USERCREATION = matricule;
                                        LigneComptable.USERMODIFICATION = matricule;
                                        LigneComptable.SITE = Site;
                                        LigneComptable.CENTRE = leCentre.CODE;
                                        LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                        LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                     " " + DateComptat;
                                        LigneComptable.CENTREIMPUTATION = "00000";
                                        if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                        if (LigneComptable.MONTANT != 0)
                                        {
                                            CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                            if (laLigneCompte != null)
                                            {
                                                laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                else
                                                    laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                            }
                                            else
                                                ListeLigneComptable.Add(LigneComptable);
                                        }
                                    }
                                }

                            }

                            #endregion
                                #region PRODUIT & REDEVANCE & CATEGORIE
                            if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                TypeCompte.TABLEFILTRE1 == "REDEVANCE" &&
                                TypeCompte.TABLEFILTRE2 == "CATEGORIE")
                            {
                                var lstFactureDistnctProduit = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                foreach (var Produit in lstFactureDistnctProduit)
                                {

                                    List<string> Code = new List<string>();
                                    Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                    var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();

                                    var lstFactureDistnctRedevance = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                    foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                    {
                                        List<string> Code1 = new List<string>();
                                        Code1.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE1).ToString());
                                        var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();

                                        List<string> Code2 = new List<string>();
                                        Code2.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE2).ToString());
                                        var VALEURFILTRE2 = String.Join(" ", Code2.ToArray()).Trim();

                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                          && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                          && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1)
                                                                                          && c.LSTVALEURFILTRE2.Contains(VALEURFILTRE2)
                                                                                          );
                                        if (leCompteSpe != null)
                                        {
                                            CsEcritureComptable LigneComptable = new CsEcritureComptable();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                           u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                           u.PRODUIT == VALEURFILTRE &&
                                                                                                           u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                           u.CATEGORIE == VALEURFILTRE2
                                                                                                           ).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                //u.COPER  == leCompteSpe.COPERASSOCIE &&
                                                                                                      u.PRODUIT == VALEURFILTRE &&
                                                                                                      u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                      u.CATEGORIE == VALEURFILTRE2
                                                                                                      ).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                //u.COPERINITAL == leCompteSpe.COPERASSOCIE && 
                                                                                                           u.PRODUIT == VALEURFILTRE &&
                                                                                                           u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                           u.CATEGORIE == VALEURFILTRE2
                                                                                                           ).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                            u.PRODUIT == VALEURFILTRE &&
                                                                                                            u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                            u.CATEGORIE == VALEURFILTRE2
                                                                                                            ).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                         u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                         u.PRODUIT == VALEURFILTRE &&
                                                                                                         u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                         u.CATEGORIE == VALEURFILTRE2
                                                                                                         ).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                            u.PRODUIT == VALEURFILTRE &&
                                                                                                            u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                            u.CATEGORIE == VALEURFILTRE2
                                                                                                            ).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            LigneComptable.SITE = leCentre.CODESITE;
                                            LigneComptable.CENTRE = leCentre.LIBELLE.Length >= 3 ? leCentre.LIBELLE.Substring(0, 3) : leCentre.LIBELLE;
                                            LigneComptable.COMPTE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.ACTIVITE = leCompteSpe.ACTIVITE;
                                            LigneComptable.FILIERE = leCompteSpe.FILIERE;
                                            LigneComptable.SOUSCOMPTE = SousCompte;
                                            LigneComptable.NUMPIECE = NumeroPiece;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.DEBIT = LigneComptable.MONTANT.Value;
                                            else
                                                LigneComptable.CREDIT = LigneComptable.MONTANT.Value;
                                            LigneComptable.CREDIT1 = LigneComptable.CREDIT;
                                            LigneComptable.DEBIT1 = LigneComptable.DEBIT;
                                            LigneComptable.INITIAL = Enumere.INITIAL;
                                            LigneComptable.CODEINTERFACE = Enumere.CODEINTERFACE;
                                            LigneComptable.PROVENACE = Enumere.PROVENACE;
                                            LigneComptable.NEGATIVE = Enumere.NEGATIVE;
                                            LigneComptable.NO = Enumere.NO;
                                            LigneComptable.ZERO = Enumere.ZERO;
                                            LigneComptable.ALPHA = Enumere.ALPHA;
                                            LigneComptable.DEVISE = Enumere.DEVISCOMPTA;
                                            LigneComptable.SOCIETE = leCompteSpe.SOCIETE;
                                            LigneComptable.CENTREIMPUTATION = leCompteSpe.CENTREIMPUTATION;
                                            LigneComptable.NATUREIMMO = leCompteSpe.NATIMMO;
                                            LigneComptable.LOCALISATION = leCompteSpe.LOC;
                                            LigneComptable.LIBRE = leCompteSpe.LIBRE;
                                            LigneComptable.DATEOPERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATEGENERATION = DateTime.Now.ToShortDateString();
                                            LigneComptable.DATECREATION = DateTime.Now;
                                            LigneComptable.DATEMODIFICATION = DateTime.Now;
                                            LigneComptable.USERCREATION = matricule;
                                            LigneComptable.USERMODIFICATION = matricule;
                                            LigneComptable.SITE = Site;
                                            LigneComptable.CENTRE = leCentre.CODE;
                                            LigneComptable.DESCRIPSTIONOPERATION = LigneComptable.LIBELLEOPERATION = LibelleActivite;
                                            LigneComptable.DESCRIPTIONLIGNE = LibelleActivite +
                                                                         " " + DateComptat;
                                            LigneComptable.CENTREIMPUTATION = "00000";
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                            if (LigneComptable.MONTANT != 0)
                                            {
                                                CsEcritureComptable laLigneCompte = ListeLigneComptable.FirstOrDefault(t => t.COMPTE == LigneComptable.COMPTE && t.CENTRE == LigneComptable.CENTRE && LigneComptable.DATECREATION == t.DATECREATION);
                                                if (laLigneCompte != null)
                                                {
                                                    laLigneCompte.MONTANT = laLigneCompte.MONTANT + LigneComptable.MONTANT;
                                                    if (LigneComptable.DC == Enumere.Debit)
                                                        laLigneCompte.DEBIT = laLigneCompte.DEBIT + LigneComptable.MONTANT.Value;
                                                    else
                                                        laLigneCompte.CREDIT = laLigneCompte.CREDIT + LigneComptable.MONTANT.Value;
                                                }
                                                else
                                                    ListeLigneComptable.Add(LigneComptable);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                            #endregion
                }
            }
            return ListeLigneComptable;
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneDEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
        {
            try
            {
                List<CsComptabilisation> lstEcriture = new List<CsComptabilisation>();
                foreach (CsOperationComptable Operation in lesOperationCpt)
                {
                    foreach (CsCaisse item in lstCaisse)
                    {
                        if (Operation.CODE == Enumere.ComptaAchatTimbre)
                            lstEcriture.AddRange(RetourneAchatTimbreSpx(Operation.COPERIDENTIFIANT, item.PK_ID, DateCaisseDebut, DateCaisseFin));
                        else if (Operation.CODE == Enumere.RemboursementASC)
                            lstEcriture.AddRange(RetourneRemboursementSpx(Operation.COPERIDENTIFIANT, item.PK_ID, DateCaisseDebut, DateCaisseFin));
                    }
                }
                Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> TheDatas = new Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>();
                if (lesOperationCpt.FirstOrDefault(y => y.CODE == Enumere.RemboursementASC) != null)
                {
                    List<CsOperationComptable> lesOperation = RetourneOperationComptable();
                    lesOperationCpt.Add(lesOperation.FirstOrDefault(p => p.CODE == Enumere.ResiliationAbonnement));
                }
                List<CsEcritureComptable> lstEcritureComptable = ComptabilisationEncaissement(lstEcriture, lesOperationCpt, lstCaisse, matricule, Site);
                TheDatas.Add(lstEcriture, lstEcritureComptable);
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneMiseAJourGrandCompte(CsOperationComptable OperationCpt, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
        {
            try
            {
                List<CsComptabilisation> lstEcriture = new List<CsComptabilisation>();
                List<CsComptabilisation> lstMandatement = RetourneEncaissementMiseAJourGCMandatementSpx(OperationCpt.COPERIDENTIFIANT, DateCaisseDebut, DateCaisseFin);
                List<CsComptabilisation> lstAvisCredit = RetourneEncaissementMiseAJourGCAvisDeCrediSpx(OperationCpt.COPERIDENTIFIANT, DateCaisseDebut, DateCaisseFin);
                if (lstMandatement != null && lstMandatement.Count != null)
                    lstEcriture.AddRange(lstMandatement);
                if (lstAvisCredit != null && lstAvisCredit.Count != null)
                    lstEcriture.AddRange(lstAvisCredit);
                Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> TheDatas = new Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>();
                List<CsEcritureComptable> lstEcritureComptable = ComptabilisationGC(lstEcriture, OperationCpt, matricule, Site);
                TheDatas.Add(lstEcriture, lstEcritureComptable);
                return TheDatas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<CsComptabilisation> RetourneEtatMiseAJourGrandCompte(DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                List<CsComptabilisation> lstEcriture = new List<CsComptabilisation>();
                List<CsComptabilisation> lstMandatement = RetourneEncaissementMiseAJourGCMandatementSpx(string.Empty, DateCaisseDebut, DateCaisseFin);
                List<CsComptabilisation> lstAvisCredit = RetourneEncaissementMiseAJourGCAvisDeCrediSpx(string.Empty, DateCaisseDebut, DateCaisseFin);
                if (lstMandatement != null && lstMandatement.Count != null  )
                    lstEcriture.AddRange(lstMandatement);
                if (lstAvisCredit != null && lstAvisCredit.Count != null)
                    lstEcriture.AddRange(lstAvisCredit);
                lstEcriture.ForEach(y => y.DATECREATION = y.DATECAISSE);
                return lstEcriture;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<CsComptabilisation> RetournefactureRedfacSpx(int IdCentre, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, bool IsAnnulation)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                if (!IsAnnulation)
                {
                    string MoisCpt = DateCaisseDebut.Value.Year.ToString() + DateCaisseDebut.Value.Month.ToString("00");
                    cmd.CommandText = "SPX_CPT_FACTURE_REDFAC";
                    cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = MoisCpt;
                }
                else
                {
                    cmd.CommandText = "SPX_CPT_ANNULATION_REDFAC";
                    cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                    cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;
                }
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;


                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.ForEach(t => t.IsDebit = false);
                    lstEntfact.ForEach(t => t.DC = Enumere.Credit);
                    if (!IsAnnulation)
                        lstEntfact.ForEach(t => t.DATECREATION = Convert.ToDateTime(Galatee.Tools.Utility.DernierJourDuMois(DateCaisseDebut.Value.Month, DateCaisseDebut.Value.Year)));

                    return lstEntfact;

                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetournefactureEntfacSpx(int IdCentre, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, bool IsAnnulation)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                string MoisCpt = string.Empty;
                if (!IsAnnulation)
                {
                    MoisCpt = DateCaisseDebut.Value.Year.ToString() + DateCaisseDebut.Value.Month.ToString("00");
                    cmd.CommandText = "SPX_CPT_FACTURE_ENTFAC";
                    cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = MoisCpt;
                }
                else
                {
                    cmd.CommandText = "SPX_CPT_ANNULATION_ENTFAC";
                    cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                    cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;
                }
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;


                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.ForEach(t => t.IsDebit = true);
                    lstEntfact.ForEach(t => t.DC = Enumere.Debit);
                    if (!IsAnnulation)
                        lstEntfact.ForEach(t => t.DATECREATION = Convert.ToDateTime(Galatee.Tools.Utility.DernierJourDuMois(DateCaisseDebut.Value.Month, DateCaisseDebut.Value.Year)));
                    lstEntfact.ForEach(t => t.COPERINITAL = lstEntfact.First().COPER);
                    return lstEntfact;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetournefactureTrvxSpx(int IdCentre, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_FACTURETRAVAUX";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.Where(y => y.COPER == Enumere.CoperTRV || y.COPER == Enumere.CoperPRE).ToList().ForEach(t => t.MONTANTDEBIT = t.MONTANTTTC);
                    lstEntfact.Where(y => y.COPER != Enumere.CoperTRV && y.COPER != Enumere.CoperPRE).ToList().ForEach(t => t.MONTANTCREDIT = t.MONTANTHT);
                    lstEntfact.ForEach(t => t.MONTANT = (t.MONTANTHT + t.MONTANTTAXE));
                    lstEntfact.ForEach(t => t.COPERINITAL = Enumere.CoperTRV);
                    lstEntfact.ForEach(t => t.DC = Enumere.Debit);


                    return lstEntfact;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsComptabilisation> RetourneEncaissementFactureTrvxSpx(string CoperOperation, int IdCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_REGLEMENTFACTURETRAVAUX";
                cmd.Parameters.Add("@Idcaisse", SqlDbType.Int).Value = IdCaisse;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPER = (t.NDOC == "TIMBRE" ? "100" : "010"));
                    lstResult.ForEach(t => t.MONTANTHT = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTHT));
                    lstResult.ForEach(t => t.MONTANTTAXE = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTTAXE));
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetournefactureFactureAutreSpx(string CoperOperation, int IdCentre, string Coper, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_FACTUREAUTRE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;
                cmd.Parameters.Add("@Coper", SqlDbType.VarChar, 3).Value = Coper;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.ForEach(t => t.IsDebit = true);
                    lstEntfact.ForEach(t => t.DC = Enumere.Debit);
                    lstEntfact.ForEach(t => t.COPERINITAL = Coper);
                    return lstEntfact;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetourneEncaissementMiseAJourGCMandatementSpx(string CoperOperation, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_MISEAJOUR_MANDATEMENT";
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    lstResult.ForEach(t => t.ISMANDATEMENT = true);
                    
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetourneEncaissementMiseAJourGCAvisDeCrediSpx(string CoperOperation, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_MISEAJOUR_AVISDECREDIT";
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsComptabilisation> RetourneEncaissementAutreFactureSpx(string CoperOperation, int IdCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string coper)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_REGLEMENTAUTREFACTURE";
                cmd.Parameters.Add("@Idcaisse", SqlDbType.Int).Value = IdCaisse;
                cmd.Parameters.Add("@Coper", SqlDbType.VarChar, 3).Value = coper;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPER = (t.NDOC == "TIMBRE" ? "100" : t.COPER));
                    lstResult.ForEach(t => t.MONTANTHT = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTHT));
                    lstResult.ForEach(t => t.MONTANTTAXE = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTTAXE));
                    lstResult.ForEach(t => t.COPERINITAL = coper);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsComptabilisation> RetourneEncaissementFactureSpx(string CoperOperation, int IdCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_REGLEMENTFACTURE";
                cmd.Parameters.Add("@Idcaisse", SqlDbType.Int).Value = IdCaisse;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPER = (t.NDOC == "TIMBRE" ? "100" : t.COPER));
                    lstResult.ForEach(t => t.MONTANTHT = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTHT));
                    lstResult.ForEach(t => t.MONTANTTAXE = (t.NDOC == "TIMBRE" ? 0 : t.MONTANTTAXE));
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsComptabilisation> RetourneAchatTimbreSpx(string CoperOperation, int IdCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_ACHATTIMBRE";
                cmd.Parameters.Add("@Idcaisse", SqlDbType.Int).Value = IdCaisse;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsComptabilisation> RetourneRemboursementSpx(string CoperOperation, int IdCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_REMBOURSEMENT";
                cmd.Parameters.Add("@Idcaisse", SqlDbType.Int).Value = IdCaisse;
                cmd.Parameters.Add("@Coper", SqlDbType.VarChar, 3).Value = CoperOperation;
                cmd.Parameters.Add("@DateCaisseDebut", SqlDbType.DateTime).Value = DateCaisseDebut;
                cmd.Parameters.Add("@DateCaisseFin", SqlDbType.DateTime).Value = DateCaisseFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstResult = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstResult.ForEach(t => t.COPERINITAL = CoperOperation);
                    return lstResult;
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsBalance> RetourneBalanceAgeeSpx(string CodeSite, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 30800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_BALANCEAGEE";
                cmd.Parameters.Add("@codeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = Convert.ToDateTime(DateFin);

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsBalance>(dt);
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsBalance> RetourneBalanceAuxilliaireSpx(string CodeSite, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 60800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_BALANCEAGEEAUXILLIAIRE11";
                cmd.Parameters.Add("@codeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsBalance>(dt);
                }
                catch (Exception ex)
                {
                    new ErrorManager().WriteInLogFile(this, ex.Message);
                    
                    //throw new Exception(cmd.CommandText + ":" + ex.Message);

                    return null;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RetourneFichierComptable(List<CsComptabilisation> LstEcriture)
        {
            string nomColone = string.Empty;
            try
            {
                List<string> sb = new List<string>();
                foreach (var obj in LstEcriture)
                {
                    List<string> fields = new List<string>();

                    var properties = obj.GetType().GetProperties();

                    fields.Add("NEW|2023|-1|GALATEE");
                    foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                    {
                        nomColone = f.Name;
                        fields.Add(f.GetValue(obj, null).ToString());
                    }
                    sb.Add(string.Join("|", fields.ToArray()));
                    Galatee.Tools.Utility.EcrireFichier(string.Join("|", fields.ToArray()), new DB_ParametresGeneraux().SelectParametresGenerauxByCode("000409").LIBELLE + ".txt");

                }
                return true;
            }
            catch (Exception ex)
            {
                string col = nomColone;
                return false;
            }


        }




        #region Interfacage Comptable Sylla
        public List<CsTypeFactureComptable> RetourneTypeFacture()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneTypeFacture();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsCompteSpecifique> RetourneCompteSpecifique()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneCompteSpecifique();
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public List<CsOperationComptable> RetourneOperationComptable()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneOperationComptable();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsTypeCompte> RetourneTypeCompte()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneTypeCompte();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsBanqueCompte> RetourneBanqueCentre()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneBanqueCentre();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsCentreCompte> RetourneParamCentre()
        {
            try
            {
                return Galatee.Entity.Model.InterfaceComptableProcedure.RetourneParamCentre();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region INTERFACE COMPTAT
        public List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable)
        {
            return Galatee.Entity.Model.InterfaceComptableProcedure.IsOperationExiste(LigneComptable);

        }

        public bool InsertionLigneComptable(List<CsEcritureComptable> LigneComptable)
        {
            try
            {
                //string   _CheminListing = "C:\\TANGO\\Anofac.lst";
                if (Galatee.Entity.Model.InterfaceComptableProcedure.InsertionLigneComptable(LigneComptable))
                {
                    string oradb = Session.GetsqlString("ComptatEntities");
                    OracleConnection conn = new OracleConnection(oradb);
                    List<CsGL_Interface> GL_Interface = new List<CsGL_Interface>();
                    GL_Interface = RetourneGL_Interface(LigneComptable);
                    int i = 0;
                    foreach (CsGL_Interface item in GL_Interface)
                    {
                        //Galatee.Tools.Utility.EcrireFichier(i.ToString(), _CheminListing);
                        i++;
                        InsertTableGl_interface(item);
                    }
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static List<CsGL_Interface> RetourneGL_Interface(List<CsEcritureComptable> leEcritComptat)
        {
            List<CsGL_Interface> lstInterface = new List<CsGL_Interface>();
            foreach (CsEcritureComptable item in leEcritComptat)
            {
                CsGL_Interface ct = new CsGL_Interface();
                ct.STATUS = item.INITIAL;
                ct.LEDGER_ID = long.Parse(item.CODEINTERFACE);
                ct.SET_OF_BOOKS_ID = long.Parse(item.NEGATIVE);
                ct.USER_JE_SOURCE_NAME = item.PROVENACE;
                ct.USER_JE_CATEGORY_NAME = item.DESCRIPSTIONOPERATION;
                ct.ACCOUNTING_DATE = DateTime.Parse(item.DATEOPERATION);
                ct.CURRENCY_CODE = item.DEVISE;
                ct.DATE_CREATED = DateTime.Parse(item.DATEGENERATION);
                ct.CREATED_BY = long.Parse(item.ZERO);
                ct.ACTUAL_FLAG = item.ALPHA;
                ct.SEGMENT1 = item.SOCIETE;
                ct.SEGMENT2 = item.ACTIVITE;
                ct.SEGMENT3 = item.COMPTE;
                ct.SEGMENT4 = item.CENTREIMPUTATION;
                ct.SEGMENT5 = item.FILIERE;
                ct.SEGMENT6 = item.SOUSCOMPTE;
                ct.SEGMENT7 = item.LOCALISATION;
                ct.SEGMENT8 = item.NATUREIMMO;
                ct.SEGMENT9 = item.LIBRE;
                ct.ENTERED_DR = item.DEBIT;
                ct.ENTERED_CR = item.CREDIT;
                ct.ACCOUNTED_DR = item.DEBIT;
                ct.ACCOUNTED_CR = item.CREDIT;
                ct.REFERENCE1 = item.LIBELLEOPERATION;
                ct.REFERENCE4 = item.NUMPIECE;
                ct.REFERENCE7 = item.NO;
                ct.REFERENCE10 = item.DESCRIPTIONLIGNE;
                lstInterface.Add(ct);
            }
            return lstInterface;
        }
        public static void InsertTableGl_interface(CsGL_Interface leInterfacce)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("ComptatEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                // Perform insert using parameters (bind variables)
                cmd.CommandText = "Insert into GL.GL_INTERFACE (STATUS,LEDGER_ID,SET_OF_BOOKS_ID,USER_JE_SOURCE_NAME,USER_JE_CATEGORY_NAME," +
                                                             "ACCOUNTING_DATE,CURRENCY_CODE,DATE_CREATED,CREATED_BY,ACTUAL_FLAG,SEGMENT1," +
                                                             "SEGMENT2,SEGMENT3,SEGMENT4,SEGMENT5,SEGMENT6,SEGMENT7,SEGMENT8,SEGMENT9," +
                                                             "ENTERED_DR,ENTERED_CR,ACCOUNTED_DR,ACCOUNTED_CR,REFERENCE1,REFERENCE4," +
                                                             "REFERENCE7,REFERENCE10)" +
                                                     "values (:STATUS,:LEDGER_ID,:SET_OF_BOOKS_ID,:USER_JE_SOURCE_NAME,:USER_JE_CATEGORY_NAME," +
                                                             ":ACCOUNTING_DATE, :CURRENCY_CODE,:DATE_CREATED,:CREATED_BY,:ACTUAL_FLAG,:SEGMENT1," +
                                                             ":SEGMENT2,:SEGMENT3,:SEGMENT4,:SEGMENT5,:SEGMENT6,:SEGMENT7,:SEGMENT8,:SEGMENT9," +
                                                             ":ENTERED_DR,:ENTERED_CR,:ACCOUNTED_DR,:ACCOUNTED_CR,:REFERENCE1,:REFERENCE4," +
                                                             ":REFERENCE7,:REFERENCE10) ";

                // Here's one way to use parameters aka bind variables:
                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("STATUS", OracleDbType.Varchar2, leInterfacce.STATUS, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LEDGER_ID", OracleDbType.Long, leInterfacce.LEDGER_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SET_OF_BOOKS_ID", OracleDbType.Long, leInterfacce.SET_OF_BOOKS_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("USER_JE_SOURCE_NAME", OracleDbType.Varchar2, leInterfacce.USER_JE_SOURCE_NAME, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("USER_JE_CATEGORY_NAME", OracleDbType.Varchar2, leInterfacce.USER_JE_CATEGORY_NAME, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ACCOUNTING_DATE", OracleDbType.Date, Convert.ToDateTime(leInterfacce.ACCOUNTING_DATE.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CURRENCY_CODE", OracleDbType.Varchar2, leInterfacce.CURRENCY_CODE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("DATE_CREATED", OracleDbType.Date, Convert.ToDateTime(leInterfacce.DATE_CREATED.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CREATED_BY", OracleDbType.Long, leInterfacce.CREATED_BY, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ACTUAL_FLAG", OracleDbType.Varchar2, leInterfacce.ACTUAL_FLAG, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT1", OracleDbType.Varchar2, leInterfacce.SEGMENT1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT2", OracleDbType.Varchar2, leInterfacce.SEGMENT2, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT3", OracleDbType.Varchar2, leInterfacce.SEGMENT3, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT4", OracleDbType.Varchar2, leInterfacce.SEGMENT4, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT5", OracleDbType.Varchar2, leInterfacce.SEGMENT5, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT6", OracleDbType.Varchar2, leInterfacce.SEGMENT6, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT7", OracleDbType.Varchar2, leInterfacce.SEGMENT7, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT8", OracleDbType.Varchar2, leInterfacce.SEGMENT8, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SEGMENT9", OracleDbType.Varchar2, leInterfacce.SEGMENT9, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ENTERED_DR", OracleDbType.Decimal, leInterfacce.ENTERED_DR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ENTERED_CR", OracleDbType.Decimal, leInterfacce.ENTERED_CR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ACCOUNTED_DR", OracleDbType.Decimal, leInterfacce.ACCOUNTED_DR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ACCOUNTED_CR", OracleDbType.Decimal, leInterfacce.ACCOUNTED_CR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("REFERENCE1", OracleDbType.Varchar2, leInterfacce.REFERENCE1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("REFERENCE4", OracleDbType.Varchar2, leInterfacce.REFERENCE4, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("REFERENCE7", OracleDbType.Varchar2, leInterfacce.REFERENCE7, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("REFERENCE10", OracleDbType.Varchar2, leInterfacce.REFERENCE10, ParameterDirection.Input));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion
        #region INTERFACE ECLIPSE
        long IDLOCATION = 0;
        long IDCENTRE = 0;
        long IdPuissanceSouscrite = 0;
        long TrfID = 0;
        long IdOperateur = 123456;
        long IDAGR = 0;
        long IDRDP = 0;
        public List<CsOperator> SelectUserEclipse(string Matricule)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                // Perform insert using parameters (bind variables) 
                cmd.CommandText = "SELECT  OPERATOR_ID,OPERATOR_NAME from AFW.OPERATOR WHERE OPERATOR_NAME = : OPERATOR_NAME ";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("OPERATOR_NAME", OracleDbType.Varchar2, Matricule, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                List<CsOperator> lstOperation = new List<CsOperator>();
                while (reader.Read())
                {
                    CsOperator Operation = new CsOperator();
                    Operation.OPERATOR_ID = long.Parse(reader.GetValue(0).ToString());
                    Operation.OPERATOR_NAME = reader.GetValue(0).ToString();
                    lstOperation.Add(Operation);
                }
                return lstOperation;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public long SelectPuissaceEclipse(CsDemandeBase laDemande)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                // Perform insert using parameters (bind variables) sHE
                cmd.CommandText = "SELECT MRL_ID FROM VPS.MRL WHERE MAXLIMIT =:PUISSANCE";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("PUISSANCE", OracleDbType.Varchar2, laDemande.CODEREGLAGECOMPTEUR, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                long IDMRL = 0;
                while (reader.Read())
                {
                    IDMRL = long.Parse(reader.GetValue(0).ToString());
                }
                return IDMRL;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public long SelectCentreEclipse(CsDemandeBase leDemande)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                // Perform insert using parameters (bind variables) sHE
                cmd.CommandText = "SELECT AREA_ID FROM VPS.AREA WHERE AREA =:Libellecentre";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("Libellecentre", OracleDbType.Varchar2, leDemande.LIBELLECENTRE, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                long IDCENTRE = 0;
                while (reader.Read())
                {
                    IDCENTRE = long.Parse(reader.GetValue(0).ToString());
                }
                return IDCENTRE;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public long SelectIDLEGALENTITYEclipse(CsClient leClient)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                // Perform insert using parameters (bind variables) sHE
                cmd.CommandText = "SELECT LEGAL_ENTITY_ID  FROM VPS.LEGAL_ENTITY WHERE LEGAL_ENTITY_REF = :LEGAL_ENTITY_REF";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_REF", OracleDbType.Varchar2, leClient.CODESITE + " " + leClient.CENTRE + " " + leClient.REFCLIENT + " " + leClient.ORDRE, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long , IdOperateur , ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                long LEGAL_ENTITY_ID = 0;
                while (reader.Read())
                {
                    LEGAL_ENTITY_ID = long.Parse(reader.GetValue(0).ToString());
                }
                return LEGAL_ENTITY_ID;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public long SelectiDLOCATION(CsClient leClient)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                long IDLEGALENTITY = SelectIDLEGALENTITYEclipse(leClient);

                // Perform insert using parameters (bind variables)
                cmd.CommandText = "SELECT LOCATION_ID  FROM  VPS.LOCATION WHERE OWNER_LEGAL_ENTITY_ID= :OWNER_LEGAL_ENTITY_ID ";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("OWNER_LEGAL_ENTITY_ID", OracleDbType.Long, IDLEGALENTITY, ParameterDirection.Input));

                OracleDataReader reader = cmd.ExecuteReader();
                long IDLOCATIONC = 0;
                while (reader.Read())
                {
                    IDLOCATIONC = long.Parse(reader.GetValue(0).ToString());
                }
                return IDLOCATIONC;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public long SelectiAGR(CsClient leClient, CsAg leAg)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                long IDCONTRAT = SelectIDLEGALENTITYEclipse(leClient);
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                cmd.CommandText = "SELECT AGR_ID  FROM VPS.AGR WHERE LEGAL_ENTITY_ID = :LEGAL_ENTITY_ID";
                //AND RESP_OPERATOR_ID = :RESP_OPERATOR_ID ";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_ID", OracleDbType.Long, IDCONTRAT, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                long AGR_IG = 0;
                while (reader.Read())
                {
                    AGR_IG = long.Parse(reader.GetValue(0).ToString());
                }
                return AGR_IG;

            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public long SelectIDRDP(CsClient leClient)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                long IDCONTRAT = SelectiDLOCATION(leClient);
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;
                cmd.CommandText = "SELECT RDP_ID  FROM VPS.RDP WHERE LOCATION_ID = :LOCATION_ID";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IDCONTRAT, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                long ID_RDP = 0;
                while (reader.Read())
                {
                    ID_RDP = long.Parse(reader.GetValue(0).ToString());
                }
                return ID_RDP;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public string SelectIDMETER(CsClient leClient, string NumeCompteur)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                long IDLOCATION = SelectiDLOCATION(leClient);
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                cmd.CommandText = "SELECT MSNO  FROM VPS.METER WHERE LOCATION_ID = :LOCATION_ID AND MSNO = :NumeCompteur";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IDLOCATION, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MSNO", OracleDbType.Varchar2, NumeCompteur, ParameterDirection.Input));
                OracleDataReader reader = cmd.ExecuteReader();
                string compteur = string.Empty;
                while (reader.Read())
                {
                    compteur = reader.GetValue(0).ToString();
                }
                return compteur;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
        }
        public List<CsLiaisonCompteur> RetourneLiaisonCompteur(CsClient leClient)
        {
            string MessageDerreur = string.Empty;
            MessageDerreur = "OracleCommand debut";
            OracleCommand cmd = new OracleCommand();
            MessageDerreur = "OracleCommand fin";
            try
            {
                

                string oradb = Session.GetsqlString("EclipseEntities");
                MessageDerreur = "OracleConnection debut( " + oradb +" )";
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                MessageDerreur = "OracleConnection fin";

                new ErrorManager().WriteInLogFile(this, MessageDerreur);

                cmd.Connection = conn;
                cmd.CommandTimeout = 360;


                // Perform insert using parameters (bind variables)
                cmd.CommandText = "SELECT meter.location_id,LEGAL_ENTITY.legal_entity_id,location.OWNER_LEGAL_ENTITY_ID,agr.agr_id," +
                                  "rdp.rdp_id,meter_id,LEGAL_ENTITY_REF,LEGAL_ENTITY_NAME,MSNO ,meter.TRF_ID " +
                                  "FROM vps.location,vps.legal_entity,vps.meter,vps.agr,vps.rdp " +
                                  "WHERE location.OWNER_LEGAL_ENTITY_ID =  legal_entity.LEGAL_ENTITY_ID " +
                                  "and LOCATION.LOCATION_ID = meter.LOCATION_ID " +
                                  "and LOCATION.LOCATION_ID = rdp.LOCATION_ID " +
                                  "and LEGAL_ENTITY.LEGAL_ENTITY_ID=agr.LEGAL_ENTITY_ID " +
                                  "and agr.agr_id=rdp.agr_id and rdp.rdp_id=meter.rdp_id AND LEGAL_ENTITY_REF = :LEGAL_ENTITY_REF ";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_REF", OracleDbType.Varchar2, leClient.CENTRE + " " + leClient.CENTRE + " " + leClient.REFCLIENT + " " + leClient.ORDRE, ParameterDirection.Input));

                MessageDerreur = "OracleDataReader debut";
                OracleDataReader reader = cmd.ExecuteReader();
                MessageDerreur = "OracleDataReader fin";

                List<CsLiaisonCompteur> laLiaison = new List<CsLiaisonCompteur>();
                while (reader.Read())
                {
                    CsLiaisonCompteur Laison = new CsLiaisonCompteur();
                    Laison.LOCATION_ID = long.Parse(reader.GetValue(0).ToString());
                    Laison.LEGAL_ENTITY_ID = long.Parse(reader.GetValue(1).ToString());
                    Laison.OWNER_LEGAL_ENTITY_ID = long.Parse(reader.GetValue(2).ToString());
                    Laison.AGR_ID = long.Parse(reader.GetValue(3).ToString());
                    Laison.RDP_ID = long.Parse(reader.GetValue(4).ToString());
                    Laison.METER_ID = long.Parse(reader.GetValue(5).ToString());
                    Laison.LEGAL_ENTITY_REF = reader.GetValue(6).ToString();
                    Laison.LEGAL_ENTITY_NAME = reader.GetValue(7).ToString();
                    Laison.MSNO = reader.GetValue(8).ToString();
                    Laison.TRF_ID = long.Parse(reader.GetValue(9).ToString());
                    laLiaison.Add(Laison);
                }
                MessageDerreur = laLiaison.Count().ToString();



















                //cmd.CommandText = "SELECT LEGAL_ENTITY_REF FROM vps.legal_entity WHERE LEGAL_ENTITY_REF = :LEGAL_ENTITY_REF ";

                //// Create parameters to hold values from front-end
                //if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                //cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_REF", OracleDbType.Varchar2, leClient.CENTRE + " " + leClient.REFCLIENT + " " + leClient.ORDRE, ParameterDirection.Input));

                //MessageDerreur = "OracleDataReader debut";
                //OracleDataReader reader = cmd.ExecuteReader();
                //MessageDerreur = "OracleDataReader fin";

                //List<CsLiaisonCompteur> laLiaison = new List<CsLiaisonCompteur>();
                //while (reader.Read())
                //{
                //    CsLiaisonCompteur Laison= new CsLiaisonCompteur();
                //    Laison.LEGAL_ENTITY_REF = reader.GetValue(6).ToString();
                //    laLiaison.Add(Laison);
                //}
                //MessageDerreur = laLiaison.Count().ToString();



                return laLiaison;
            }
            catch (Exception ex)
            {
                //ex.Message = MessageDerreur;
                cmd.Dispose();
                throw new Exception(MessageDerreur+":"+ex.Message);
            }
        }


        public bool InsertTableLEGAL_ENTITY(CsClient leClient, CsAg leAg)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;


                // Perform insert using parameters (bind variables)
                cmd.CommandText = "INSERT INTO VPS.LEGAL_ENTITY(LEGAL_ENTITY_NAME,ADDR1,ADDR2,ADDR3,LEGAL_ENTITY_REF,TEL," +
                                                               "LEGAL_ENTITY_TYPE_ID,LEGAL_ENTITY_STATUS_ID,RESP_OPERATOR_ID)" +
                                                      "VALUES (:LEGAL_ENTITY_NAME,:ADDR1,:ADDR2,:ADDR3,:LEGAL_ENTITY_REF,:TEL," +
                                                               ":LEGAL_ENTITY_TYPE_ID,:LEGAL_ENTITY_STATUS_ID,:RESP_OPERATOR_ID) ";

                // Here's one way to use parameters aka bind variables:
                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_NAME", OracleDbType.Varchar2, leClient.NOMABON, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR1", OracleDbType.Varchar2, (leAg.LIBELLECOMMUNE + "  " + leAg.LIBELLEQUARTIER), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR2", OracleDbType.Varchar2, leAg.PORTE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR3", OracleDbType.Varchar2, leAg.RUE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_REF", OracleDbType.Varchar2, leClient.CODESITE + " " + leClient.CENTRE + " " + leClient.REFCLIENT + " " + leClient.ORDRE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TEL", OracleDbType.Varchar2, leClient.TELEPHONE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();

                //IDCONTRAT = long.Parse(cmd.Parameters[":IDCONTRAT"].Value.ToString());
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {

                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool InsertTableAGR(CsClient leClient, CsAg leAg)
        {
            try
            {

                long IDLEGALENTITYE = SelectIDLEGALENTITYEclipse(leClient);
                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                // Perform insert using parameters (bind variables)
                //cmd.CommandText = "INSERT INTO VPS.AGR(AGR_TYPE_ID,LEGAL_ENTITY_ID,AREF,AGR_STATUS_ID," +
                //                                      "UTIL_ID, LIABILITY_TYPE_ID, CREDIT_CONTROL_VALUE," +
                //                                      "CREDIT_CONTROLLED,CREDIT_CONTROLLED_DATE,RESP_OPERATOR_ID)" +
                //                                "VALUES (:AGR_TYPE_ID,:LEGAL_ENTITY_ID,:AREF,:AGR_STATUS_ID," +
                //                                       ":UTIL_ID,:LIABILITY_TYPE_ID,:CREDIT_CONTROL_VALUE," +
                //                                     ":CREDIT_CONTROLLED,:CREDIT_CONTROLLED_DATE,:RESP_OPERATOR_ID)";

                //if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                //cmd.Parameters.Add(new OracleParameter("AGR_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("LEGAL_ENTITY_ID", OracleDbType.Long, IDLEGALENTITYE, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("AREF", OracleDbType.Varchar2, leAg.TOURNEE + leAg.ORDTOUR, ParameterDirection.Input));

                ////cmd.Parameters.Add(new OracleParameter("AREF", OracleDbType.Varchar2, "AGR" + IDLEGALENTITYE.ToString(), ParameterDirection.Input)); // TO DO A reprendre avec le ID de la table meme
                //cmd.Parameters.Add(new OracleParameter("AGR_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("UTIL_ID", OracleDbType.Long, 340, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("LIABILITY_TYPE_ID", OracleDbType.Long, 3, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("CREDIT_CONTROL_VALUE", OracleDbType.Long, 0, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("CREDIT_CONTROLLED", OracleDbType.Long, 0, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("CREDIT_CONTROLLED_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));

cmd.CommandText="INSERT INTO VPS.AGR(AGR_TYPE_ID,LEGAL_ENTITY_ID,AREF,AGR_STATUS_ID,UTIL_ID, LIABILITY_TYPE_ID, CREDIT_CONTROL_VALUE,"+ 
                "CREDIT_CONTROLLED,CREDIT_CONTROLLED_DATE,RESP_OPERATOR_ID)"+
                "VALUES (1," + IDLEGALENTITYE + ",'" + leAg.TOURNEE + leAg.ORDTOUR + "',1, 400, 3, 0, 0, to_date('" + System.DateTime.Today.Day.ToString("00") + "-" + System.DateTime.Today.Month.ToString("00") + "-" + System.DateTime.Today.Year.ToString("00") + "', 'dd-mm-yy'), " + IdOperateur + ")";















                int rowsUpdated = cmd.ExecuteNonQuery();
                if (null != cmd)
                    cmd.Dispose();
                //UpdateTableAGR(leAg, leClient);
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool InsertTableLOCATION(CsDemandeBase laDemande, CsClient leClient, CsAg leAg)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                long IDCENTRE = SelectCentreEclipse(laDemande);
                long IDCONTRAT = SelectIDLEGALENTITYEclipse(leClient);

                // Perform insert using parameters (bind variables)
                //cmd.CommandText = "INSERT INTO VPS.LOCATION(LOCATION_TYPE_ID,LOCATION_STATUS_ID,ADDR1,ADDR2," +
                //                                            "ADDR3,LOCATION_REF,AREA_ID,OWNER_LEGAL_ENTITY_ID,RESP_OPERATOR_ID)" +
                //                                    "VALUES (:LOCATION_TYPE_ID,:LOCATION_STATUS_ID,:ADDR1,:ADDR2," +
                //                                            ":ADDR3,:LOCATION_REF,:AREA_ID,:OWNER_LEGAL_ENTITY_ID,:RESP_OPERATOR_ID)";

                cmd.CommandText = "INSERT INTO VPS.LOCATION(LOCATION_TYPE_ID,LOCATION_STATUS_ID,ADDR1,ADDR2," +
                                                           "ADDR3,AREA_ID,OWNER_LEGAL_ENTITY_ID,RESP_OPERATOR_ID)" +
                                                   "VALUES (:LOCATION_TYPE_ID,:LOCATION_STATUS_ID,:ADDR1,:ADDR2," +
                                                           ":ADDR3,:AREA_ID,:OWNER_LEGAL_ENTITY_ID,:RESP_OPERATOR_ID)";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.Parameters.Add(new OracleParameter("LOCATION_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR1", OracleDbType.Varchar2, (leAg.LIBELLECOMMUNE + "  " + leAg.LIBELLEQUARTIER), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR2", OracleDbType.Varchar2, leAg.PORTE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ADDR3", OracleDbType.Varchar2, leAg.RUE, ParameterDirection.Input));
                //cmd.Parameters.Add(new OracleParameter("LOCATION_REF", OracleDbType.Varchar2, leAg.TOURNEE + leAg.ORDTOUR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AREA_ID", OracleDbType.Long, IDCENTRE, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("OWNER_LEGAL_ENTITY_ID", OracleDbType.Long, IDCONTRAT, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool InsertTableRDP(CsDemandeBase ledemande, CsClient leClient, CsBrt leBranchement, CsAg leAg)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                if (ledemande.REGLAGECOMPTEUR != null && ledemande.REGLAGECOMPTEUR.Substring(0, 1) == "2")
                    TrfID = 503400;
                else
                    TrfID = 10503400;

                IDLOCATION = SelectiDLOCATION(leClient);
                IdPuissanceSouscrite = SelectPuissaceEclipse(ledemande);

                if (IDAGR == 0)
                    IDAGR = SelectiAGR(leClient, leAg);

                // Perform insert using parameters (bind variables)
                cmd.CommandText = "INSERT INTO VPS.RDP(RDP_STATUS_ID,RES_ID,UTIL_ID," +
                                                      "RDP_DISP_METHOD_ID,RESP_OPERATOR_ID,TRF_ID," +
                                                      "SG_ID,LOCATION_ID,RDP_MEASURE_TYPE_ID,MRL_ID," +
                                                      "RDP_TYPE_ID," +
                                                      "GPS_LATITUDE,GPS_LONGITUDE," +
                                                      "INST_DATE,AGR_ID,REP_STDATE)" +
                                                "VALUES(:RDP_STATUS_ID,:RES_ID,:UTIL_ID," +
                                                      ":RDP_DISP_METHOD_ID,:RESP_OPERATOR_ID,:TRF_ID," +
                                                      ":SG_ID,:LOCATION_ID,:RDP_MEASURE_TYPE_ID,:MRL_ID," +
                                                      ":RDP_TYPE_ID," +
                                                     ":GPS_LATITUDE,:GPS_LONGITUDE," +
                                                      ":INST_DATE,:AGR_ID,:REP_STDATE)";

                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("RDP_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RES_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("UTIL_ID", OracleDbType.Long, 340, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_DISP_METHOD_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_ID", OracleDbType.Long, TrfID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SG_ID", OracleDbType.Long, 20573400, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IDLOCATION, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_MEASURE_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MRL_ID", OracleDbType.Long, IdPuissanceSouscrite, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("GPS_LATITUDE", OracleDbType.Long, string.IsNullOrEmpty(leBranchement.LATITUDE) ? 0 : int.Parse(leBranchement.LATITUDE), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("GPS_LONGITUDE", OracleDbType.Long, string.IsNullOrEmpty(leBranchement.LONGITUDE) ? 0 : int.Parse(leBranchement.LONGITUDE), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("INST_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AGR_ID", OracleDbType.Long, IDAGR, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("REP_STDATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (null != cmd)
                    cmd.Dispose();
                UpdateTableRDP(leAg, leClient);
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool InsertTableMeter(CsDemandeBase laDemande, CsAbon leAbon, CsClient leClient, CsAg leAg, string NumeroCompteur)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                if (IdPuissanceSouscrite == 0)
                    IdPuissanceSouscrite = SelectPuissaceEclipse(laDemande);
                if (TrfID == 0)
                {
                    if (laDemande.REGLAGECOMPTEUR != null && laDemande.REGLAGECOMPTEUR.Substring(0, 1) == "2")
                        TrfID = 503400;
                    else
                        TrfID = 10503400;
                }
                if (IDLOCATION == 0)
                    IDLOCATION = SelectiDLOCATION(leClient);

                if (IDRDP == 0)
                    IDRDP = SelectIDRDP(leClient);

                // Perform insert using parameters (bind variables)
                cmd.CommandText = "INSERT INTO VPS.METER(METER_TYPE_ID,MRL_ID,METER_STATUS_ID,STATUS_CATEGORY_ID,MSNO,RES_ID,UTIL_ID,TKTYPE_ID" +
                                                    ",TRF_ID,SG_ID,LOCATION_ID ,ENCRYPTION_TYPE_ID,MANUFACTURER_ID,RDP_ID,INST_DATE,CHANGE_DATE," +
                                                     "RESP_OPERATOR_ID,MEASURE_TYPE_ID,TRF_TS,SG_TS,AMR_RATE_CHANNEL," +
                                                    "AMR_AUTH_LEVEL,AMR_CLIENT_TYPE,AMR_PROF_INTERVAL,METER_TAKE_ON_COMPLETE )" +
                                          "VALUES ( :METER_TYPE_ID,:MRL_ID,:METER_STATUS_ID,:STATUS_CATEGORY_ID,:MSNO,:RES_ID,:UTIL_ID,:TKTYPE_ID" +
                                                    ",:TRF_ID,:SG_ID,:LOCATION_ID,:ENCRYPTION_TYPE_ID,:MANUFACTURER_ID,:RDP_ID,:INST_DATE,:CHANGE_DATE," +
                                                    ":RESP_OPERATOR_ID,:MEASURE_TYPE_ID,:TRF_TS,:SG_TS,:AMR_RATE_CHANNEL," +
                                                    ":AMR_AUTH_LEVEL,:AMR_CLIENT_TYPE,:AMR_PROF_INTERVAL,:METER_TAKE_ON_COMPLETE )";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("METER_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MRL_ID", OracleDbType.Long, IdPuissanceSouscrite, ParameterDirection.Input)); // Voir la meilleur facon de faire
                cmd.Parameters.Add(new OracleParameter("METER_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("STATUS_CATEGORY_ID", OracleDbType.Long, 101, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MSNO", OracleDbType.Varchar2, NumeroCompteur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RES_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("UTIL_ID", OracleDbType.Long, 340, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TKTYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_ID", OracleDbType.Long, TrfID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SG_ID", OracleDbType.Long, 20573400, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IDLOCATION, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ENCRYPTION_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MANUFACTURER_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long, IDRDP, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("INST_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CHANGE_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MEASURE_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_TS", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SG_TS", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_RATE_CHANNEL", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_AUTH_LEVEL", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_CLIENT_TYPE", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_PROF_INTERVAL", OracleDbType.Long, 0, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("METER_TAKE_ON_COMPLETE", OracleDbType.Long, 0, ParameterDirection.Input));
                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }

        public bool UpdateTableMeter(CsDemandeBase laDemande, CsClient leClient, CsAg leAg, string compteur)
        {
            try
            {

                if (IdPuissanceSouscrite == 0)
                    IdPuissanceSouscrite = SelectPuissaceEclipse(laDemande);
                if (TrfID == 0)
                {
                    if (laDemande.REGLAGECOMPTEUR != null && laDemande.REGLAGECOMPTEUR.Substring(0, 1) == "2")
                        TrfID = 503400;
                    else
                        TrfID = 10503400;
                }
                if (IDLOCATION == 0)
                    IDLOCATION = SelectiDLOCATION(leClient);



                long StatuCompter = 1;
                if (laDemande.TYPEDEMANDE == Enumere.Resiliation)
                    StatuCompter = 2;

                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;


                // Perform insert using parameters (bind variables)
                cmd.CommandText = "UPDATE VPS.METER SET MRL_ID= :MRL_ID, METER_STATUS_ID= :METER_STATUS_ID,STATUS_CATEGORY_ID= :STATUS_CATEGORY_ID , " +
                                                     "RDP_ID= :RDP_ID ,TRF_ID = :TRF_ID," +
                                                     "CHANGE_DATE= :CHANGE_DATE)" +
                                                     "WHERE MSNO= :MSNO AND LOCATION_ID = :LOCATION_ID )";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("MRL_ID", OracleDbType.Long, IdPuissanceSouscrite, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("METER_STATUS_ID", OracleDbType.Long, StatuCompter, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("STATUS_CATEGORY_ID", OracleDbType.Long, 101, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long, IDLOCATION, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_ID", OracleDbType.Long, TrfID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CHANGE_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MSNO", OracleDbType.Varchar2, compteur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IDLOCATION, ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
        }
        public bool UpdateTableLOCATION(CsDemandeBase laDemande, CsAg leAg)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandTimeout = 360;

                cmd.CommandText = "UPDATE VPS.LOCATION SET LOCATION_STATUS_ID= :LOCATION_STATUS_ID,MOD_TS= :MOD_TS"
                                 + "WHERE LOCATION_REF= :LOCATION_REF";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.Parameters.Add(new OracleParameter("LOCATION_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MOD_TS", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_REF", OracleDbType.Varchar2, leAg.TOURNEE + " " + leAg.ORDTOUR, ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();
                IDLOCATION = long.Parse(cmd.Parameters[":IDLOCATION"].Value.ToString());

                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception)
            {
                cmd.Dispose();
                throw;
            }

        }
        public bool UpdateTableAGR(CsAg leAg, CsClient leClient)
        {
            try
            {
                IDAGR = SelectiAGR(leClient, leAg);
                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE VPS.AGR SET AREF= :AREF WHERE AGR_ID= :AGR_ID";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("AREF", OracleDbType.Varchar2, "AGR" + IDAGR.ToString(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AGR_ID", OracleDbType.Long, IDAGR, ParameterDirection.Input));
                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }

        }

        public bool UpdateTableRDP(CsAg leAg, CsClient leClient)
        {
            try
            {
                IDRDP = SelectIDRDP(leClient);
                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE VPS.RDP SET RDP_REF= :RDP_REF WHERE RDP_ID= :RDP_ID";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("RDP_REF", OracleDbType.Varchar2, "RDP" + IDRDP.ToString(), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long, IDRDP, ParameterDirection.Input));
                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }

        }

        public bool UpdateTableRDP_SUITEAPDP(CsDemandeBase ledemande, CsClient leClient)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                IDRDP = SelectIDRDP(leClient);
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                if (ledemande.REGLAGECOMPTEUR != null && ledemande.REGLAGECOMPTEUR.Substring(0, 1) == "2")
                    TrfID = 503400;
                else
                    TrfID = 10503400;

                IdPuissanceSouscrite = SelectPuissaceEclipse(ledemande);

                // Perform insert using parameters (bind variables)
                cmd.CommandText = "UPDATE VPS.RDP SET MRL_ID= :MRL_ID WHERE RDP_ID= :RDP_ID";

                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long, IDRDP, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MRL_ID", OracleDbType.Long, IdPuissanceSouscrite, ParameterDirection.Input));

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (null != cmd)
                    cmd.Dispose();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool UpdateTableChangementCompteur(CsDemandeBase ledemande, CsClient leClient,string NumeroCompteur)
        {
            try
            {
                bool Resultat = false;
                List<CsLiaisonCompteur> liaisonCpt = this.RetourneLiaisonCompteur(leClient);
                if (liaisonCpt != null && liaisonCpt.Count != 0)
                {
                    CsLiaisonCompteur laliaison = liaisonCpt.First();
                    if (DeliaisonCompteur(leClient, laliaison, 52110833400))
                      Resultat=  ReliaisonCompteur(leClient, laliaison, NumeroCompteur);
                }
                return Resultat;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }
        public bool DeliaisonCompteur(CsClient leClient,CsLiaisonCompteur laLiaison, long IdmagasinCentre)
        {
            try
            {
                OracleCommand cmd = new OracleCommand();
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;

                cmd.CommandText = "UPDATE VPS.METER SET  METER_STATUS_ID=METER_STATUS_ID,STATUS_CATEGORY_ID=STATUS_CATEGORY_ID,LOCATION_ID=LOCATION_ID," +
                 "RDP_ID= RDP_ID,MOD_TS=MOD_TS ,CHANGE_DATE=CHANGE_DATE  WHERE METER_ID=METER_ID";
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();

                cmd.Parameters.Add(new OracleParameter("METER_STATUS_ID", OracleDbType.Long,2, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("STATUS_CATEGORY_ID", OracleDbType.Long , 102, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, IdmagasinCentre, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long ,null , ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MOD_TS", OracleDbType.Date  ,System.DateTime.Today  , ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CHANGE_DATE", OracleDbType.Date  ,System.DateTime.Today  , ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("METER_ID", OracleDbType.Long, laLiaison.METER_ID, ParameterDirection.Input));
                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw;
            }

        }
        public bool ReliaisonCompteur(CsClient leClient, CsLiaisonCompteur laLiaison, string NumeroCompteur)
        {
            OracleCommand cmd = new OracleCommand();
            try
            {
                string oradb = Session.GetsqlString("EclipseEntities");
                OracleConnection conn = new OracleConnection(oradb);
                conn.Open();
                cmd.Connection = conn;
                // Perform insert using parameters (bind variables)
                cmd.CommandText = "INSERT INTO VPS.METER(METER_TYPE_ID,MRL_ID,METER_STATUS_ID,STATUS_CATEGORY_ID,MSNO,RES_ID,UTIL_ID,TKTYPE_ID" +
                                                    ",TRF_ID,SG_ID,LOCATION_ID ,ENCRYPTION_TYPE_ID,MANUFACTURER_ID,RDP_ID,INST_DATE,CHANGE_DATE," +
                                                     "RESP_OPERATOR_ID,MEASURE_TYPE_ID,TRF_TS,SG_TS,AMR_RATE_CHANNEL," +
                                                    "AMR_AUTH_LEVEL,AMR_CLIENT_TYPE,AMR_PROF_INTERVAL,METER_TAKE_ON_COMPLETE )" +
                                          "VALUES ( :METER_TYPE_ID,:MRL_ID,:METER_STATUS_ID,:STATUS_CATEGORY_ID,:MSNO,:RES_ID,:UTIL_ID,:TKTYPE_ID" +
                                                    ",:TRF_ID,:SG_ID,:LOCATION_ID,:ENCRYPTION_TYPE_ID,:MANUFACTURER_ID,:RDP_ID,:INST_DATE,:CHANGE_DATE," +
                                                    ":RESP_OPERATOR_ID,:MEASURE_TYPE_ID,:TRF_TS,:SG_TS,:AMR_RATE_CHANNEL," +
                                                    ":AMR_AUTH_LEVEL,:AMR_CLIENT_TYPE,:AMR_PROF_INTERVAL,:METER_TAKE_ON_COMPLETE )";

                // Create parameters to hold values from front-end
                if (cmd.Parameters.Count != 0) cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter("METER_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MRL_ID", OracleDbType.Long, IdPuissanceSouscrite, ParameterDirection.Input)); // Voir la meilleur facon de faire
                cmd.Parameters.Add(new OracleParameter("METER_STATUS_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("STATUS_CATEGORY_ID", OracleDbType.Long, 101, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MSNO", OracleDbType.Varchar2, NumeroCompteur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RES_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("UTIL_ID", OracleDbType.Long, 340, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TKTYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_ID", OracleDbType.Long, laLiaison.TRF_ID , ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SG_ID", OracleDbType.Long, 20573400, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("LOCATION_ID", OracleDbType.Long, laLiaison.LOCATION_ID , ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("ENCRYPTION_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MANUFACTURER_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RDP_ID", OracleDbType.Long, laLiaison.RDP_ID, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("INST_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("CHANGE_DATE", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("RESP_OPERATOR_ID", OracleDbType.Long, IdOperateur, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("MEASURE_TYPE_ID", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("TRF_TS", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("SG_TS", OracleDbType.Date, Convert.ToDateTime(System.DateTime.Today.ToShortDateString()), ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_RATE_CHANNEL", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_AUTH_LEVEL", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_CLIENT_TYPE", OracleDbType.Long, 1, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("AMR_PROF_INTERVAL", OracleDbType.Long, 0, ParameterDirection.Input));
                cmd.Parameters.Add(new OracleParameter("METER_TAKE_ON_COMPLETE", OracleDbType.Long, 0, ParameterDirection.Input));
                int rowsUpdated = cmd.ExecuteNonQuery();
                return (rowsUpdated == 0) ? false : true;
            }
            catch (Exception ex)
            {
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                // Cleanup objects
                if (null != cmd)
                    cmd.Dispose();
                if (null != cmd)
                    cmd.Dispose();
            }
        }

        public bool PurgeComptabilisation(int IdOperation, string CodeSite, DateTime? DateDebut, DateTime? DateFin)
        {

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CPT_PURGEMISEAJOUR";

            cmd.Parameters.Add("@IdOperation", SqlDbType.Int).Value = IdOperation;
            cmd.Parameters.Add("@Site", SqlDbType.VarChar, 6).Value = CodeSite;
            cmd.Parameters.Add("@DateOperationDebut", SqlDbType.DateTime).Value = DateDebut;
            cmd.Parameters.Add("@DateOperationFin", SqlDbType.DateTime).Value = DateFin;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                int Res = -1;
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                Res = cmd.ExecuteNonQuery();
                return Res==-1 ? false : true ;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        public List<CsLclient> RetourneAvanceSurConsomation(string CodeSite, bool IsResilier, DateTime? DateDebut, DateTime? DateFin)
        {

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CPT_AVANCECONSOMATION";
            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@IsResilier", SqlDbType.Bit).Value = IsResilier;
            cmd.Parameters.Add("@DateOperationDebut", SqlDbType.DateTime).Value = DateDebut;
            cmd.Parameters.Add("@DateOperationFin", SqlDbType.DateTime).Value = DateFin;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsClient > RetourneProvision(string CodeSite, List<string> lstCateg,List<string> lstProd, DateTime? DateDebut, DateTime? DateFin)
        {
            string lstCategorie = DBBase.RetourneStringListeObject(lstCateg);
            string lstProduit = DBBase.RetourneStringListeObject(lstProd);

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CPT_PROVISION";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Categorie", SqlDbType.VarChar, int.MaxValue).Value = lstCategorie;
            cmd.Parameters.Add("@Produit", SqlDbType.VarChar, int.MaxValue).Value = lstProduit;
            cmd.Parameters.Add("@DateOperationDebut", SqlDbType.DateTime).Value = DateDebut;
            cmd.Parameters.Add("@DateOperationFin", SqlDbType.DateTime).Value = DateFin;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsClient>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        #endregion

        #endregion

    }
}
