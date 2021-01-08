using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.DataAccess;
using Galatee.Structure;
using System.ServiceModel.Activation;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "InterfaceService" à la fois dans le code, le fichier svc et le fichier de configuration.
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class InterfaceService : IInterfaceService
    {
        #region VARIABLES GLOBALES

        int typeExport;
        int MoisComptaAuto;
        List<CsCentre> LCentres = new List<CsCentre>();
        List<CsJournal> LJournal = new List<CsJournal>();
        List<CsLibelle> LActiviteProduit = new List<CsLibelle>();
        List<CsCorrespondance> LCorrespondance = new List<CsCorrespondance>();
        List<CsComptable> LComptabilisation = new List<CsComptable>();
        List<CsNature> LNature = new List<CsNature>();
        List<CsSchema> LSchema = new List<CsSchema>();
        CsParametre Parametres = new CsParametre();
        List<string> LignesFichiersGen = new List<string>();
        string separator = ";";
        string MoisComptable = string.Empty;

        #endregion
        #region IInterfaceService Membres

        public List<Galatee.Structure.CsLibelle> RetourneTousMoisComptables()
        {
            try
            {
                return new DBCompta().RetourneTousMoisComptables();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         CsParametre RetourneParametresCompta()
        {
            try
            {
                return new DBCompta().RetourneParametresCompta().First();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         public List<CsLibelle> RetourneSousMenuCompta()
        {
            try
            {
                return new DBCompta().RetourneSousMenuCompta();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsCtax> SelectAll_CTAX()
        {
            throw new NotImplementedException();
        }

         List<CsCentre> RetourneTousDirecteur()
        {
            try
            {
                return new CommonDAL().GetCentre();
            }
            catch (Exception ex )
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsComptable> RetourneComptabilisation(string pMoisCompta, string pTypenC)
        {
            try
            {
                return new DBCompta().RetourneComptaDuMoisComptable(pMoisCompta, pTypenC);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsCorrespondance> RetourneCorrespondanceCompta()
        {
            try
            {
                return new DBCompta().RetourneCorrespondanceCompta();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsLibelle> RetourneActiviteProduits()
        {
            try
            {
                return new DBCompta().RetourneActiviteProduits();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsJournal> RetourneJournals()
        {
            try
            {
                return new DBCompta().RetourneJournals();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

         List<CsCompteCritere> RetourneCsCompteCriteres()
        {
            try
            {
                return new DBCompta().RetourneCsCompteCriteres();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        public bool? ComptabilisationFacturation(string pMoisCompta, string pImprimante,int pGestionMoisCompta,string fileName,string fileHeader,int pTypeEnc,int pTypeMenu)
        {
           
            try
            {
                MoisComptable = pMoisCompta;
                LCentres.AddRange(RetourneTousDirecteur());
                LJournal.AddRange(RetourneJournals());
                LActiviteProduit.AddRange(RetourneActiviteProduits());
                LCorrespondance.AddRange(RetourneCorrespondanceCompta());
                LComptabilisation.AddRange(RetourneComptabilisation(pMoisCompta, pTypeMenu.ToString()));
                LNature.AddRange(new CommonDAL().RetourneAllNature());
                LSchema.AddRange(new DBCompta().RetourneDonneesTableSchema());
                Parametres = RetourneParametresCompta();

                CsParametre paramGlobal = RetourneParametresCompta();
                typeExport = Convert.ToInt16(paramGlobal.TypeExportFichier);
                MoisComptaAuto = Convert.ToInt16(paramGlobal.GestionAutoMoisCompt);
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        bool? Constitution(List<CsComptable> pComptabilisation,string pMoisCompta, int pMoisComptaAuto, int pTypeExport, int pModeTrait, int pTypeEnc,int pTypeMenu,string pOperation)
        {
            int Inversion;
            List<string> SensRedevance = new List<string>();
            List<string> SensNature = new List<string>();
            List<CsComptable> comptabilisation = new List<CsComptable>();
            try
            {
                if (pModeTrait == 0)
                    return false;
                if (pTypeExport == Enumere.SATTI || pTypeExport == Enumere.AGRESSO)
                {
                    Inversion = 1;
                    if (pTypeMenu == Enumere.NORMALE)
                    {
                        SensRedevance.Add("C");
                        SensNature.Add("D");
                    }
                    else
                    {
                        SensRedevance.Add("D");
                        SensNature.Add("C");
                    }
                }
                else
                {
                    SensRedevance.Add("C");
                    SensNature.Add("D");
                    if (pTypeMenu == Enumere.NORMALE)
                        Inversion = 1;
                    else
                        Inversion = -1;
                }

                comptabilisation.AddRange(pComptabilisation);

                foreach (CsComptable compta in pComptabilisation)
                {
                    compta.REDHT = Inversion * compta.REDHT;
                    compta.REDTAXE = Inversion * compta.REDTAXE;
                    compta.CODEJOURNAL = "V";

                    if (compta.REDTAXE != 0)
                        ComptabilisationTaxe(compta,LNature, SensRedevance[0], 300, pOperation);
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        bool? ComptabilisationTaxe(CsComptable pComptabilisation, List<CsNature> pNature,string pSens, int pOperation,string pOrigineLigne)
        {
            int origine = pOperation;
            string CodeTaxeEncours = string.Empty;
            string CompteEncours = string.Empty;
            string CompteAnalEnCours = string.Empty;
            string Libelle = string.Empty;
            decimal? dTauxTaxe = 0;
            bool bTrouve = false;
            string tax = string.Empty;
            string resultCpt = string.Empty;
            List<CsCtax> taxes = new List<CsCtax>();
            try
            {
                taxes.AddRange(SelectAll_CTAX());
                if (pOperation == Enumere.FACTURATION || pOperation == Enumere.GUICHET)
                    origine = Enumere.FACTURATION;
                else
                    origine = Enumere.ENCAISSEMENT;

                for(int i = 0 ; i < taxes.Count ;i++)
                {
                    //if (pComptabilisation.CTAX == taxes[i].CTAX)
                    //{
                    //    bTrouve = true;
                    //    CodeTaxeEncours = taxes[i].CTAX;
                    //    CsNature nature = pNature.Where(n => n.NATURE == pComptabilisation.NATURE).FirstOrDefault();
                    //    if (nature.COMPTGENE.Contains('S')) // il n'y a un shéma comptable sur la nature
                    //    {
                    //        RecupererCompteTVASchema(pComptabilisation.CENTRE,nature.COMPTGENE.Substring(1), CompteEncours, CompteAnalEnCours);
                    //        if (string.IsNullOrEmpty(CompteEncours))
                    //            CompteEncours = taxes[i].CIMP;
                    //    }
                    //    else
                    //        CompteEncours = taxes[i].CIMP;
                    //    Libelle = taxes[i].LIBELLE;
                    //    dTauxTaxe = taxes[i].TAUX;
                    //    break;
                    //} 
                }

                tax = "T"+pComptabilisation.CTAX;
                if (Enumere.ORIGINE_LIGNE.DEMANDE.ToString() == pOrigineLigne)
                    pComptabilisation.REDTAXE = dTauxTaxe * pComptabilisation.REDHT;

                if (bTrouve)
                {
                    if (CompteEncours.Contains('S'))
                        ExploiterSchema(CompteEncours, pComptabilisation.NATURE, pComptabilisation.CENTRE, pComptabilisation.PRODUIT,
                                        pComptabilisation.MOISCOMPTA, pComptabilisation.REDTAXE, pSens, Libelle, tax, Parametres.CompteAnalytique, pComptabilisation.CODEJOURNAL);
                    else
                    {
                        ExploiterSchema(CompteEncours,resultCpt, pComptabilisation.NATURE, pComptabilisation.CENTRE, pComptabilisation.PRODUIT,
                                          pComptabilisation.MOISCOMPTA,"G", null, null);
                        EcritureLigneCompte(pSens, pComptabilisation.CENTRE, pComptabilisation.NATURE, pComptabilisation.PRODUIT, pComptabilisation.REDTAXE, resultCpt,
                                            CompteAnalEnCours, tax, Libelle, "", "", origine, "", pComptabilisation.CODEJOURNAL);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        private void EcritureLigneCompte(string pSens, string pCentre, string pNature, string pProduit, decimal? pMontant, string pCompteGeneral, string pCompteAnal, string pRedtax, 
                                         string pLibelle, string pCoper, string pModeReg, int pIfrom, string pSousCpt, string pCodeJrnal)
        {
            string szFactuCobOut = string.Empty;
            string szNomDB = string.Empty;
            string szTmpTable = string.Empty;

            decimal? szDebit = null;
            decimal? szCredit = null;
            string szDateCompta = DateTime.Now.ToShortDateString();
            string ligneFichier = string.Empty;

            try
            {

                if(pIfrom ==0 || pIfrom ==1)
                {
                    if (pIfrom == 0)
                        szFactuCobOut = "F";   // facture d'énergie 
                    else
                        szFactuCobOut = "O";  // facture autre produit
                }

                if (pIfrom == 2) // encaissement
                {
                    szFactuCobOut = "E";
                }

                if (Parametres.TypeExportFichier == Enumere.AGRESSO.ToString())
                    EcritureLigneCompteAGRESSO(pSens,pCentre,pNature,pProduit,pMontant,pCompteGeneral,pCompteAnal,pRedtax,pLibelle,pCoper,pModeReg,szNomDB,szTmpTable,szFactuCobOut);
                else if(Parametres.TypeExportFichier == Enumere.SATTI.ToString())
                        EcritureLigneCompteTypeCFA(pSens, pCentre, pNature, pProduit, pMontant, pCompteGeneral, pCompteAnal, pRedtax,pSousCpt ,pLibelle, pCoper,szNomDB, szTmpTable);
                else if (Parametres.TypeExportFichier == Enumere.ORACLEAPP.ToString() || Parametres.TypeExportFichier == Enumere.SIZAWATER.ToString())
                {
                    bool ret;
                    ret = EcritureLigneCompteORACLEAPP(pSens, pCentre, pNature, pProduit, pMontant, pCompteGeneral, pCompteAnal, pRedtax, pLibelle, pCoper, pModeReg, szTmpTable, szFactuCobOut, pCodeJrnal);
                    if (!ret)
                    {
                        if (!(pSens == "D"))
                        {
                            szDebit = pMontant;
                            szCredit = 0;
                        }
                        else
                        {
                            szCredit = pMontant;
                            szDebit = 0;
                        }

                        string ligne = "20" + separator + "0" + separator + pCompteGeneral + separator + szDateCompta + separator + szDebit.ToString() +
                                       separator + szCredit.ToString() + separator + MoisComptable + separator + szDateCompta + separator + string.Empty + separator +
                                       szDateCompta + separator + "0" + separator + pLibelle + separator + "0" + separator + "0" + separator + "0" + separator + pCompteGeneral;

                        LignesFichiersGen.Add(ligne); // sauvegarde des lignes du fichier de sortie

                        if (pCompteGeneral == "705100")
                        {
                            string _ligne = "20" + separator + "1" + separator + "01600" + separator + szDateCompta + separator + szDebit.ToString() +
                                          separator + szCredit.ToString() + separator + pLibelle + separator + szDateCompta + separator + string.Empty + separator + string.Empty + separator +
                                          string.Empty + separator + "0" + separator + pLibelle + separator + "0" + separator + "0" + separator + "0" + separator + pCompteGeneral;

                            LignesFichiersGen.Add(_ligne);
                        }

                    }
                }

                     
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private bool EcritureLigneCompteORACLEAPP(string pSens, string pCentre, string pNature, string pProduit, decimal? pMontant, string pCompteGeneral, string pCompteAnal, string pRedtax, string pLibelle, string pCoper, string pModeReg, string szTmpTable, string szFactuCobOut, string pCodeJrnal)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }

        private void EcritureLigneCompteTypeCFA(string pSens, string pCentre, string pNature, string pProduit, decimal? pMontant, string pCompteGeneral, string pCompteAnal, string pRedtax, string pSousCpt, string pLibelle, string pCoper, string szNomDB, string szTmpTable)
        {
            try
            {

            }
            catch (Exception ex )
            {
                string error = ex.Message;
            }
        }

        private void EcritureLigneCompteAGRESSO(string pSens, string pCentre, string pNature, string pProduit, decimal? pMontant, string pCompteGeneral, string pCompteAnal, string pRedtax, string pLibelle, string pCoper, string pModeReg, string szNomDB, string szTmpTable, string szFactuCobOut)
        {
            try
            {

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void ExploiterSchema(string CompteEncours, string pNature, string pCentre, string pProduit, string pMoisCompta, decimal? pRedtax, string pSens, string pLibelle, string pTax, string pComptAnal, string pCodeJour)
        {
            try
            {

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void ExploiterSchema(string CompteEncours, string pResult, string pNature, string pCentre, string pProduit, string pMoisCompta, string pString1, string pString2, string pString3)
        {
            try
            {

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void RecupererCompteTVASchema(string pCentre,string pNum, string pCompteTVA, string pCompteAnal) 
        {
            try
            {
                CsSchema _schema=   LSchema.FirstOrDefault(s => s.NUM == pNum && s.CENTRE == pCentre);
                if (!string.IsNullOrEmpty(_schema.GENE5))
                {
                    pCompteTVA = _schema.GENE5;
                    if (pCompteTVA.ToArray()[0].ToString() == Enumere.CPTCONST4.ToString())
                    {
                        foreach (CsJournal journal in LJournal)
                        {
                            if (journal.CodeAgence == pCentre)
                            {
                                pCompteAnal = "T" + journal.CodeService;
                                break;
                            }
                        }
                    }
                }
                else
                    pCompteAnal = string.Empty;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        #endregion
    }
}
