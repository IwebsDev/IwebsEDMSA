using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using Galatee.DataAccess;
using System.ServiceModel.Activation;
//using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Hosting;
using System.Data.SqlClient;

namespace WcfService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "FacturationService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FacturationService : IFacturationService
    {

        string from = string.Empty;
        string subject = string.Empty;
        string body = string.Empty;
        string smtp = string.Empty;
        string portSmtp = string.Empty;
        string sslEnabled = string.Empty;
        string login = string.Empty;
        string password = string.Empty;

        #region 

        public bool? EnvoyerFacturesNew(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc, string Matricule, string Serveur, string Port, string PortWeb)
        {
            try
            {

                DbFacturation db = new DbFacturation();
                //return new DbFacturation().RetourneFacturesAbo07(facturesAEnvoyer, Rdlc);

                List<CsFactureClient> lstGenerale = db.RetourneFacturesAbo07(facturesAEnvoyer, Rdlc); ;
                List<CsFactureClient> lesClient = RetourneDistinctClientFacture(lstGenerale);
                foreach (CsFactureClient item in lesClient)
                {
                    try
                    {
                        ErrorManager.WriteInLogFile(this, item.Client + " " + item.EMAIL);

                        Dictionary<string, string> param = new Dictionary<string, string>();

                        List<CsFactureClient> lstDetailClient = lstGenerale.Where(t => t.Centre == item.Centre && t.Client == item.Client).ToList();
                        string refclient = item.Centre + item.Client + item.Ordre;
                        string periode = FormatPeriodeMMAAAA(lstDetailClient.First().Periode);
                        string montant = Convert.ToDecimal(lstDetailClient.First().TotFTTC).ToString("#,0.");
                        //string exigible = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(item.detail.First().dateExige);
                        string nomAbon = item.NomAbon;
                        string telephone = string.Empty;
                        param.Add("TypeEdition", "Facture");

                        if (lstDetailClient != null && lstDetailClient.Count > 0)
                        {
                            if (lstDetailClient.First().ISFACTURE != null && lstDetailClient.First().ISFACTURE.Value == true)
                            {
                                LireParametresMail();
                                param.Add("pismail", lstDetailClient.FirstOrDefault(c => c.EMAIL != null && c.EMAIL != "").EMAIL);
                                new WcfService.Printings.PrintingsService().ActionMail<CsFactureClient, CsFactureClient>(lstDetailClient, param, "FactureSimpleMail", "Facturation", Matricule, Serveur, Port, PortWeb, from, subject, body, smtp, portSmtp, sslEnabled, login, password);
                            }
                            if (lstDetailClient.First().ISSMS != null && lstDetailClient.First().ISSMS.Value == true)
                            {
                                #region Envoi de sms
                                //telephone = lstDetailClient.FirstOrDefault(c => c.TELEPHONE != null && c.TELEPHONE != "").TELEPHONE;
                                //if (!string.IsNullOrWhiteSpace(telephone))
                                //{
                                //    string message_sms1 = "Chere:" + nomAbon + "(" + refclient + "), nous vous informons de la disponibilité de votre facture du :" + periode + " de " + montant + "FCFA.";
                                //    string message_sms2 = "Vous pouvez vous présenter à nos guichets ou utiliser le service Orange Money pour réglement .Merci de votre fidélité";
                                //    Utility.SendToSmsHandler(message_sms1, telephone);
                                //    Utility.SendToSmsHandler(message_sms2, telephone);

                                //}
                                #endregion
                            }
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorManager.WriteInLogFile(this, ex.Message);
                        continue;
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
   
        #endregion
        #region Facturation
        public bool verifieSimulation(List<CsLotri> pLot)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.verifieSimulation(pLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public bool verifieSaisieTotal(List<CsLotri> pLot)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.verifieSaisieTotal(pLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsLotri> ChargerLotriPourCalcul(Dictionary<string, List<int>> lstSiteCentre, string matricule, bool IsLotCourant,string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                ErrorManager.WriteInLogFile (this,"Debut " + System.DateTime.Now );
                List<CsLotri> l = db.ChargerLotriPourCalcul(lstSiteCentre, matricule, IsLotCourant, Periode);
                ErrorManager.WriteInLogFile(this, "Debut " + System.DateTime.Now);
                return l;

            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsLotri> ChargerLotriPourMiseAJour(List<int> lstCentreHabil, string matricule)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourMiseAJour(lstCentreHabil, matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsLotri> ChargerLotri(List<int> lstCentreHabil)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerLotri(lstCentreHabil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsLotri> ChargerLotriDejaFacture(List<int> lstCentreHabil, string Matricule)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerLotriDejaFacture(lstCentreHabil, Matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriDejaMiseAJour(List<int> lstCentreHabil, string Matricule)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerLotriDejaFacture(lstCentreHabil, Matricule);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEvenement> ChargerEvenementLot(CsLotri leLots)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerEvenementLot(leLots);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriNonMisAJours(List<int> lstCentre)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriNonMisAJours(lstCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourEditionIndex(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourEditionIndex(lstSiteCentre, IsLotCourant, Periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourSaisie(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant, string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourSaisie(lstSiteCentre, IsLotCourant, Periode, UserConnect);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourEdition(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant, string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                ErrorManager.WriteInLogFile (this, "Debut " + System.DateTime.Now );
                List<CsLotri> l = db.ChargerLotriPourEdition(lstSiteCentre, UserConnect, IsLotCourant, Periode);
                ErrorManager.WriteInLogFile(this, "Fin " + System.DateTime.Now);
                return l;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourRejetClient(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsLotCourant,string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourRejetClient(lstSiteCentre, UserConnect, IsLotCourant, Periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourDefacturation(Dictionary<string, List<int>> lstSiteCentre, string UserConnect, bool IsValidation )
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourDefacturation(lstSiteCentre, UserConnect,IsValidation );
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerLotriPourEnquete(Dictionary<string, List<int>> lstSiteCentre, bool IsLotCourant, string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerLotriPourEnquete(lstSiteCentre, IsLotCourant, Periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerDetailLotri(List<int> lstIdCentre, string Matricule)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                List<CsLotri> lstRetour = new List<CsLotri>();
                List<CsLotri> lstLot = db.ChargerDetailLotri(lstIdCentre);
                foreach (CsLotri item in lstLot)
                {
                    if (IsLotIsole(item.NUMLOTRI) && item.USERCREATION != Matricule)
                        continue;
                    lstRetour.Add(item);
                }
                return lstRetour;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEvenement> ChargerDesEvenementPourRejet(List<CsLotri> lstLot)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ChargerDesEvenementPourRejet(lstLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsEvenement> ChargerDesEvenement(List<CsLotri> lstLot)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ChargerDesEvenement(lstLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }


        public List<CsEvenement> ChargerDesEvenementClientLot(CsClient leClient, string NumeroLot)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ChargerDesEvenementClientLot(leClient, NumeroLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsLotri> ChargerDetailLotriPourDefacturation(List<int> LstIdCentre)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ChargerDetailLotriPourDefacturation(LstIdCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public bool ValiderRejetDefacturation(List<CsLotri> _ListDesLots)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.RejetDemandeDefacturerLot(_ListDesLots);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public CParametre ReturnCparam()
        {
            try
            {
                return new CParametre();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #region Edition de facture

        public List<string> ListeDeJet(string LotRi)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneListeDeJets(LotRi);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<string> ListeDePeriode()
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneListeDePeriodes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<string> ListeDeFormat()
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneListeDePeriodes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsEnteteFacture> RetourneClientDuneBorne(string centre, string client, string lotRi, string periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneClientDuneBorne(centre, client, lotRi, periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        private bool IsLotIsole(string Numlot)
        {
            if (Numlot == null)
                return false;
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureAnnulatinIndex, FactureResiliationIndex }.Contains(Numlot.Substring(Enumere.TailleCentre, (Enumere.TailleNumeroBatch - Enumere.TailleCentre))))
                return true;
            else
                return false;
        }

        public List<CsFactureClient> RetourneFacturesRegroupement(string regroupementDebut, string regroupementFin, List<string> LstPeriode, List<string> Produit, int? idcentre, string rdlc)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstFacture = new List<CsFactureClient>();
                lstFacture = db.RetourneFacturesRegroupement(regroupementDebut, regroupementFin, LstPeriode, Produit, idcentre, rdlc);
                return lstFacture;
            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsFactureClient> RetourneFacturesPeriode(List<CsCentre> lstCentre, string periodeDebut, string PeriodeFin, string centreTournee,
            string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
            string centreStop, string clientStop, string rdl)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstFacture = new List<CsFactureClient>();
                lstFacture = db.RetourneFacturesPeriode(lstCentre, periodeDebut, PeriodeFin, centreTournee, tourneeDebut, tourneeFin,
                centreReprise, clientReprise, centreStop, clientStop, rdl, false);
                if (lstFacture == null)
                    return new List<CsFactureClient>();
                else
                    return lstFacture;

            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsFactureClient> RetourneFactures(CsLotri leLotSelect, CsTournee laTournee,
             CsClient leClient  ,bool DejaMiseAjour, string rdl)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstFacture = new List<CsFactureClient>();
                lstFacture = db.RetourneFactures(leLotSelect, laTournee,leClient, DejaMiseAjour, IsLotIsole(leLotSelect.NUMLOTRI), rdl);
                if (lstFacture == null)
                    return new List<CsFactureClient>();
                else
                    return lstFacture;

            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsAnnomalie> RetourneAnnomalieFactures(CsLotri leLotSelect, string centreTournee,
                    string tourneeDebut, string tourneeFin, string centreReprise, string clientReprise,
                   string centreStop, string clientStop, string periodeSelectionne, string rdl)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsAnnomalie> lstFacture = new List<CsAnnomalie>();
                lstFacture = db.RetourneAnnomalieFactures(leLotSelect, centreTournee, tourneeDebut, tourneeFin,
                centreReprise, clientReprise, centreStop, clientStop, periodeSelectionne, IsLotIsole(leLotSelect.NUMLOTRI));
                return lstFacture;
            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsAnnomalie> RetourneControleFactures(CsLotri leLotSelect)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsAnnomalie> lstFacture = new List<CsAnnomalie>();
                lstFacture = db.RetourneControleFactures(leLotSelect);
                return lstFacture;
            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsFactureClient> retourneDuplicatas(string centre, string client, string ordre)
        {
            try
            {
                DBEservice db = new DBEservice();
                return db.ListeDesFactures(centre, client, ordre, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsEnteteFacture> retourneFactureAnnulation(int idCentre, string centre, string client, string ordre)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                //return db.ListeDesFacturesAnnulation(idCentre, centre, client, ordre, string.Empty, string.Empty); ZEG 04/01/2021
                return db.FacturesPourAnnulation(idCentre, centre, client, ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEnteteFacture> retourneFacturePourDuplicat(int idCentre, string centre, string client, string ordre)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ListeDesFacturesAnnulation(idCentre, centre, client, ordre, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool VerifierPaiementFacture(CsEnteteFacture laFacture)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.VerifierPaiementFacture(laFacture);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        //public bool ValiderAnnulationFacture(CsEnteteFacture laFacture)
        //{
        //    try
        //    {
        //        DBCalcul db = new DBCalcul();
        //        return db.AnnulationFactureSpx(laFacture.PK_ID, laFacture.MATRICULE);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogException(this, ex);
        //        return true;
        //    }
        //}

        public List<CsLotri> retourneListeAMaj(string lotri, byte onlyLotriNum)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.retourneListeAMaj();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<string> retourneMoisComptable(string statut, string dateDebut, string dateFin)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                //return db.retourneMoisComptable(statut, dateDebut,dateFin);
                string moisComptable = DateTime.Today.Month.ToString("00") + "/" + DateTime.Today.Year;
                string moisComptable1 = (DateTime.Today.Month - 1).ToString("00") + "/" + DateTime.Today.Year;
                List<string> lstMoisComptable = new List<string>();
                lstMoisComptable.Add(moisComptable);
                lstMoisComptable.Add(moisComptable1);
                return lstMoisComptable;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public CsStatFacturation MiseAjourLots(List<CsLotri> lots)
        {

            try
            {
                return new DbFacturation().MiseAjourLots(lots, false);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }




        public bool? EditerFactures(CsLotri leLotSelect, CsTournee laTournee,
                    CsClient leClient, bool DejaMiseAjour, string Rdlc, Dictionary<string, string> param, string CheminImpression, string Matricule, string Serveur, string Port, string PortWeb)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Début édition  lot " + leLotSelect.NUMLOTRI); /** ZEG 29/08/2017 **/

                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstClient = new List<CsFactureClient>();
                List<CsFactureClient> lstFacture = db.RetourneFactures(leLotSelect, laTournee, leClient, DejaMiseAjour, IsLotIsole(leLotSelect.NUMLOTRI), Rdlc);


                var lesClient = lstFacture.Select(y => new { y.Centre, y.Client, y.Ordre }).Distinct();
                foreach (var item in lesClient)
                    lstClient.Add(new CsFactureClient() { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });



                int Passage = 0;
                string[] tableau = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V" };
                while (lstClient.Where(o => o.ISFACTURE != true).Count() != 0)
                {

                    //string NomFichier = Rdlc + tableau[Passage];  ZEG 29/08/2017
                    string NomFichier = Rdlc + leLotSelect.NUMLOTRI + tableau[Passage];
                    List<string> clientSelectionne = lstClient.Where(m => m.ISFACTURE != true).Take(100).Select(o => o.Client).ToList();
                    List<CsFactureClient> factureAEditer = lstFacture.Where(p => clientSelectionne.Contains(p.Client)).ToList();
                    factureAEditer.ForEach(y => y.fk_idClient = clientSelectionne.Count.ToString());


                    new WcfService.Printings.PrintingsService().PrintingFromService<CsFactureClient, CsFactureClient>(factureAEditer, param, NomFichier, CheminImpression, "pdf", Rdlc, "Facturation", Matricule, Serveur, Port, PortWeb);

                    lstClient.Where(p => clientSelectionne.Contains(p.Client)).ToList().ForEach(p => p.ISFACTURE = true);
                    Passage++;
                }
                ErrorManager.WriteInLogFile(this, "Fin édition lot " + leLotSelect.NUMLOTRI); /** ZEG 29/08/2017 **/

                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, "Problème édition lot " + leLotSelect.NUMLOTRI + " => " + ex.Message); /** ZEG 29/08/2017 **/
                return false;
            }
        }




        #endregion

        #region Calcul facture
        public List<CsFactureBrut> CalculeDuLotGeneral(List<CsLotri> _ListDesLots, bool IsSimulation)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.CalculeDuLot(_ListDesLots, IsSimulation);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public CsStatFacturation DefacturerLot(List<CsLotri> _ListDesLots, string Action)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.DefacturerLot(_ListDesLots, Action);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool DemandeDefacturerLot(List<CsLotri> _ListDesLots)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.DemandeDefacturerLot(_ListDesLots);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        public bool ValiderFacturation(List<CsFactureBrut> laFacturation, bool IsFactureResil)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                //laFacturation
                return db.ValiderFacturation(laFacturation, IsFactureResil);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }

        public List<CsLotri> ChargerLotriFromEntfac(List<int> lstCentre)
        {
            try
            {
                DBCalcul db = new DBCalcul();
                return db.ChargerLotriFromEntfac(lstCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        #endregion
        public List<CsAction> retourneActionFact(CsLotri leLot)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.retourneActionFact(leLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsFactureClient> ExposeFactureClient()
        { return new List<CsFactureClient>(); }
        public bool RetirerClientLotFact(List<CsEvenement> lesEvenementLot)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetirerClientLotFact(lesEvenementLot);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #region Redevence

        public List<CsRedevance> LoadAllRedevance()
        {
            List<CsRedevance> ListeLotScelle = new List<CsRedevance>();
            try
            {
                ListeLotScelle = new DbFacturation().LoadAllRedevance();
                return ListeLotScelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveRedevance(List<CsRedevance> ListeRedevanceToUpdate, List<CsRedevance> ListeRedevanceToInserte, List<CsRedevance> ListeRedevanceToDelete)
        {
            List<CsRedevance> ListeRedevanceToUpdate_ = (List<CsRedevance>)ListeRedevanceToUpdate;
            List<CsRedevance> ListeRedevanceToInserte_ = (List<CsRedevance>)ListeRedevanceToInserte;
            List<CsRedevance> ListeRedevanceToDelete_ = (List<CsRedevance>)ListeRedevanceToDelete;

            try
            {
                return new DbFacturation().SaveRedevance(ListeRedevanceToUpdate_, ListeRedevanceToInserte_, ListeRedevanceToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

        #endregion
        #region Index
        static Dictionary<string, List<CsEnregRI>> dicosReleveIndex = new Dictionary<string, List<CsEnregRI>>();
        static Dictionary<string, string> parametreReleveIndex = new Dictionary<string, string>();

        public CsTableReference ChargerTableDeReference(int Num, string Code, string Centre)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.ChargerTableDeReference(Num, Code, Centre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsProduit> RetourneProduitService(int NumEntreTa, string CodeTable, string centre)
        {
            try
            {
                DBAccueil db = new DBAccueil();
                return new List<CsProduit>();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsCentre> ListeDesDonneesDesSite()
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.ChargerLesDonneesDesSite(false);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTournee> ChargerListeDesTournees()
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerLesTournees();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> IsBatchExistDansPagerie(string _NumLot)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.IsBatchExistDansPagerie(_NumLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public int? ConstruireLot(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule)
        {
            try
            {
                int _NombreDeClient = 0;
                _NombreDeClient = new DBIndex().ConstruireLotSansEdition(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periode, Matricule);
                return _NombreDeClient;

            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ConstruireLotAvecEdition(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule)
        {
            try
            {
                List<CsEvenement> lesEvenement = new List<CsEvenement>();
                lesEvenement = new DBIndex().ConstruireLotAvecEdition(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periode, Matricule);
                return lesEvenement;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ChargerListeDesEvenements(List<CsLotri> _lstLotri, string sequence)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesEvenements(_lstLotri, sequence);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ChargerListeDesEvenementsNonFacture(List<CsCanalisation> leCompteur)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesEvenementsNonFacture(leCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ChargerListeTransfert(List<CsLotri> _lstLotri, string sequence)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesEvenements(_lstLotri, sequence);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsCasind> RetourneListeDesCas(string centre, string cas)
        {
            try
            {
                return new DBAccueil().RetourneListeDesCas();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool DechargementTsp(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.DechargementTsp(LstEvt);

            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        public bool InsererTransferTsp(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.InsererTransferTsp(LstEvt);

            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsEvenement> ChargerListeDesEvenementsTsp(List<CsLotri> _lstLotri, string sequence)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesEvenementsTsp(_lstLotri, sequence);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsEvenement> ChargerListeDesTransfertsp(CsLotri _lstLotri)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesTransfertsp(_lstLotri);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsLotri> ChargerDistinctLotriTransfertsp(List<int> isCentre)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerDistinctLotTsp(isCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }



        public CsEvenement MisAJourEvenement(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                if (db.MisAJourEvenement(LstEvt))
                    return LstEvt.First();

                return null;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool RepriseIndex(CsEvenement LstEvt)
        {

            try
            {
                return new DBIndex().RepriseIndex(LstEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }




        public bool MisAJourEvenementSynchoTSP(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.MisAJourEvenementSynchoTSP(LstEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public CsEvenement MisAJourEvenementIndex(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                if (db.MisAJourEvenementIndex(LstEvt))
                    return LstEvt.First();

                return null;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool ValiderEnquete(List<CsEvenement> LstEvt)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.MisAJourEvenement(LstEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsCanalisation> RetourneCanalisation(int Fk_IDcentre, string Centre, string Client, string produit, int? point)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.RetourneCanalisation(Fk_IDcentre, Centre, Client, produit, point);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public CsCanalisation RetourneCanalisationbyIdEvenement(int? Fk_IdCompteur)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.RetourneCanalisationbyIdEvenement(Fk_IdCompteur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsEvenement RetourneEvenementPose(CsEvenement leEvt)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.RetourneEvenementPose(leEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool VerificationNumeroCompteur(CsEvenement leEvt)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.VerificationNumeroCompteur(leEvt);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        public CsSaisiHorsLot RetourneInfoClientHorsLot(int Fk_IDcentre, string Centre, string Client, string produit, string tournee)
        {
            try
            {
                CsSaisiHorsLot horslot = new CsSaisiHorsLot();
                horslot.Compteurs = RetourneCanalisation(Fk_IDcentre, Centre, Client, produit, null);
                foreach (var item in horslot.Compteurs)
                {
                    CsSaisiIndexIndividuel indexcompteur = RetourneEvenementNonFact(item);
                    indexcompteur.POINT = item.POINT;
                    horslot.IndexInfo.Add(indexcompteur);
                }
                return horslot;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public CsSaisiIndexIndividuel RetourneEvenementNonFact(CsCanalisation LaCanalisation)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.RetourneEvenementNonFact(LaCanalisation);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsSaisiIndexIndividuel> RetourneListEvenementNonFact(List<CsCanalisation> LesCanalisation)
        {
            try
            {
                List<CsSaisiIndexIndividuel> lstIndexIndividuel = new List<CsSaisiIndexIndividuel>();
                DBIndex db = new DBIndex();
                foreach (CsCanalisation item in LesCanalisation)
                {
                    lstIndexIndividuel.Add(db.RetourneEvenementNonFact(item));
                }
                //System.Threading.Tasks.Parallel.ForEach(LesCanalisation, item =>
                //    {
                //    lstIndexIndividuel.Add(db.RetourneEvenementNonFact(item));
                //    });
                return lstIndexIndividuel;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }



        public List<CsEvenement> EditerListeDesCasConfirmer(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> Cas)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.EditerListeDesCasConfirmer(Lotri, LesCentre, Latournee, Cas);

                //List<CsPrint> listePrint = new List<CsPrint>();
                //listePrint.AddRange(ListeDesReleve);
                //Printings.PrintingsService printService = new Printings.PrintingsService();
                //printService.setFromWebPart(listePrint, key, null);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> EditerListeDesCas(string Lotri, List<int> LesCentre, List<int?> Latournee, List<CsCasind> Cas)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.EditerListeDesCas(Lotri, LesCentre, Latournee, Cas);


                //List<CsPrint> listePrint = new List<CsPrint>();
                //listePrint.AddRange(ListeDesReleve);
                //Printings.PrintingsService printService = new Printings.PrintingsService();
                //printService.setFromWebPart(listePrint, key, null);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> EditerLotGenerale(List<CsLotri> LstLotri, int typeRequete)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.EditerLotGenerale(LstLotri, typeRequete);


                //List<CsPrint> listePrint = new List<CsPrint>();
                //listePrint.AddRange(ListeDesReleve);

                //Printings.PrintingsService printService = new Printings.PrintingsService();
                //return printService.setFromWebPart(listePrint, key, null);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }

        }



        public List<CsLotri> RetourneListeLotNonTraite(List<int> lsCentrePerimetre)
        {
            try
            {
                return new DBIndex().RetournerListeLotNonSaisi(lsCentrePerimetre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ListeDesEnquete(List<CsLotri> loSelectionnee)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ListeDesEnquete(loSelectionnee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> ListeDesEnqueteCasOnze(List<CsLotri> loSelectionnee)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ListeDesEnqueteCasOnze(loSelectionnee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }


        public List<CsCasind> ListeDesCas(string pLotri, List<int> LesCentre, List<int?> lesTournee, string typeCas)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ListeDesCas(pLotri, LesCentre, lesTournee, typeCas);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool? UpdateUneTournee(CsTournee LaTournee)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.UpdateUneTournee(LaTournee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;

            }
        }
        public bool? InsererUneTournee(CsTournee LaTournee)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.InsererUneTournee(LaTournee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool SupprimerUneTournee(CsTournee LaTournee)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.SupprimerUneTournee(LaTournee);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return true;
            }
        }

        public bool? CreationClientIndexHorsLot(CsEvenement ev)
        {
            try
            {
                return new DBIndex().CreationClientIndexHorsLot(ev);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsReleveur> RetourneReleveurCentre(List<int> Liscentre)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.RetourneReleveurCentre(Liscentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsReleveur> RetourneReleveurCentre_(List<int> Liscentre,int id_user)
        {
            try
            {
                DBIndex db = new DBIndex();
                List<CsReleveur> result = new List<CsReleveur>();
                //List<CsReleveur> res = null;
                foreach (var item in Liscentre)
	            {
                   result.AddRange(db.RetourneReleveurCentre_(item, id_user));

 /*                   res = new List<CsReleveur>();
                    res = db.RetourneReleveurCentre_(item, id_user);
                    if (res != null && res.Count > 0)
                    {
                        foreach (CsReleveur it in res)
                        {
                            if (!result.Contains(it))
                                result.Add(it);
                        }
                    }*/
	            }
                return result;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool UpdateUnReleveur(CsReleveur LeReleveur)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.UpdateUnReleveur(LeReleveur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return true;
            }
        }
        public bool InsererUnReleveur(CsReleveur LeReleveur)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.InsererUnReleveur(LeReleveur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return true;
            }
        }
        public bool SupprimerUnReleveur(CsReleveur LeReleveur)
        {

            try
            {
                DBIndex db = new DBIndex();
                return db.SupprimerUnReleveur(LeReleveur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return true;
            }
        }
        public List<CsUtilisateur> RetourneAllUser()
        {
            try
            {
                DBAdmUsers db = new DBAdmUsers();
                return db.GetAllUser();
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        //Edition
        public bool setListRiInWebPart(string key, List<CsEnregRI> objectList, Dictionary<string, string> parameters)
        {

            try
            {
                if (dicosReleveIndex.ContainsKey(key))
                    dicosReleveIndex.Remove(key);
                dicosReleveIndex.Add(key, objectList);
                parametreReleveIndex = parameters;
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsEnregRI> getListRiFromWebPart(string key)
        {
            try
            {
                return dicosReleveIndex[key];
            }
            catch (Exception ex)
            {

                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool DeleteListRiFromWebPart(string key)
        {
            try
            {
                dicosReleveIndex.Remove(key);
                parametreReleveIndex.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        public string MiseAJourEvenementTSP(string CONSO, string DATECREATION, string IsSaisi, string STATUS, string STATUSPAGERIE, string MATRICULE, string IsFromPageri, string CAS, string INDEXEVT, string DATEEVT, string PK_ID)
        {
            DBIndex db = new DBIndex();
            List<CsEvenement> lstevt = db.GetEvenement(PK_ID);
            lstevt.First().CONSO = int.Parse(CONSO);
            lstevt.First().DATECREATION = DateTime.Parse(DATECREATION);
            lstevt.First().IsSaisi = IsSaisi == "true" ? true : false;
            lstevt.First().STATUS = int.Parse(STATUS);
            lstevt.First().STATUSPAGERIE = int.Parse(STATUSPAGERIE);
            lstevt.First().MATRICULE = MATRICULE;
            lstevt.First().IsFromPageri = IsFromPageri == "true" ? true : false;
            lstevt.First().CAS = CAS;
            lstevt.First().INDEXEVT = int.Parse(INDEXEVT);
            lstevt.First().DATEEVT = DateTime.Parse(DATEEVT);
            MisAJourEvenement(lstevt);
            return "ok";
        }

        public List<CsEvenement> RetourneListeLotSelonCritere(string NumLot, string Centre, string Tournee)
        {
            try
            {
                List<CsLotri> lotSelonCritere = new DBIndex().RetourneListeLotSelonCritere(NumLot, Centre, Tournee);
                return ChargerListeDesEvenementsParReleveurs(lotSelonCritere, string.Empty);
                //return new List<CsEvenement> { new CsEvenement { LOTRI = "01200002" } };
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
                //
            }
        }

        public List<CsEvenement> ChargerListeDesEvenementsParReleveurs(List<CsLotri> _lstLotri, string sequence)
        {
            try
            {

                DBIndex db = new DBIndex();
                return db.ChargerListeDesEvenements(_lstLotri, sequence);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsLotri> RetourneListeLotNonTraiteParReleveur(string NumPortable)
        {
            try
            {
                var lotDureleveur = new DBIndex().RetourneListeLotNonTraiteParReleveur(NumPortable);
                return lotDureleveur;
                //return ChargerListeDesEvenementsParReleveurs(lotDureleveur, string.Empty);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool? InsererEvenement(CsEvenement ev)
        {
            try
            {
                return new DBIndex().InsererEvenement(ev);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTournee> ChargerTourneeReleveur(int Fk_IdReleveur)
        {
            try
            {
                return new DBIndex().ChargerTourneeReleveur(Fk_IdReleveur);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsTournee> ChargerAllTourneeReleveur(List<int> lstIdCentre)
        {
            try
            {
                 var rslt=new DBIndex().ChargerAllTourneeReleveur(lstIdCentre);
                 foreach (var item in rslt)
                 {
                     if (item.DATEFIN!=null || item.DATEFIN<DateTime.Now)
                     {
                         item.NOMRELEVEUR = string.Empty;
                         item.MATRICULEPIA = string.Empty;
                     }
                 }
                 return rslt;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool ValiderAffectation(List<CsTournee> lstTourne)
        {
            try
            {
                return new DBIndex().ValiderAffectation(lstTourne);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        public List<CsLotri> ChargerLotriPourDelete(string matricule, string DebutLot, string Finlot)
        {
            try
            {
                return new DBIndex().ChargerLotriPourDelete(matricule, DebutLot, Finlot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public bool VerificationEtatDuLot(List<CsLotri> lesLot)
        {
            DBIndex db = new DBIndex();
            bool result = true;
            foreach (CsLotri item in lesLot)
            {
                if (!db.VerifierSaisie(item))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool DeleteLotri(List<CsLotri> lesLot)
        {
            try
            {
                return new DBIndex().SupprimeEvtLotri(lesLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public bool EffacerEvtLotri(string UserConnecter)
        {
            try
            {
                return new DBIndex().EffacerEvtLotri(UserConnecter);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        #region Suivie de la facturation
        public List<CsEvenement> ChargerCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerCasFacture(lstSiteCentre, Lotri, Periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEvenement> ChargerDetailCasFacture(Dictionary<string, List<int>> lstSiteCentre, string Lotri, string Periode, List<string> Cas)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerDetailCasFacture(lstSiteCentre, Lotri, Periode, Cas);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEvenement> ChargerClientNonConstituer(Dictionary<string, List<int>> lstSiteCentre,string Periode)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ChargerClientNonConstituer(lstSiteCentre,Periode);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEvenement> EtatStatistique(Dictionary<string, List<int>> lstSiteCentre, string Periode, string Lotri, string TypeEtat)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.EtatStatistique(lstSiteCentre, Periode, Lotri, TypeEtat);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public bool SaveAffectationTourne(List<CsTournee> ListeDesTourneAAffecter)
        {
            try
            {
                return new DbFacturation().SaveAffectationTourne(ListeDesTourneAAffecter);
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return false;
            }
        }
        #endregion
     
        #endregion
        #region Envoi de facture par mail

        public List<CsEnteteFacture> ListeDesClientPourEnvoieMail(List<int> CentreClient, List<string> lstPeriode, bool sms, bool email)
        {
            try
            {
                DBEservice db = new DBEservice();
                return db.ListeDesClientPourEnvoieMail(CentreClient, lstPeriode, sms, email);
            }
            catch (Exception ex)
            {

                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public static List<CsFactureClient> RetourneDistinctClientFacture(List<CsFactureClient> _LstFacture)
        {
            try
            {
                List<CsFactureClient> lstClientResult = new List<CsFactureClient>();
                var lstClientDistinct = _LstFacture.Select(t => new { t.Centre, t.Client, t.Ordre }).Distinct().ToList();
                foreach (var item in lstClientDistinct)
                    lstClientResult.Add(new CsFactureClient { Centre = item.Centre, Client = item.Client, Ordre = item.Ordre });
                return lstClientResult;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string FormatPeriodeMMAAAA(string periode)
        {
            if (periode.Length == 6)
                return periode.Substring(4, 2).PadLeft(2, '0') + "/" + periode.Substring(0, 4);
            else return string.Empty;
        }
        public List<CsFactureClient> EnvoyerFactures(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return new DbFacturation().RetourneFacturesAbo07(facturesAEnvoyer, Rdlc);

            }
            catch { return null; }
        }
        public bool? EnvoyerFacturesNew(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc,string Matricule)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                //return new DbFacturation().RetourneFacturesAbo07(facturesAEnvoyer, Rdlc);

                 List<CsFactureClient> lstGenerale = db.RetourneFacturesAbo07(facturesAEnvoyer, Rdlc);;
                 List<CsFactureClient> lesClient = RetourneDistinctClientFacture(lstGenerale);
                            foreach (CsFactureClient item in lesClient)
                            {
                                Dictionary<string, string> param = new Dictionary<string, string>();

                                List<CsFactureClient> lstDetailClient = lstGenerale.Where(t => t.Centre == item.Centre && t.Client == item.Client).ToList();
                                string refclient = item.Centre + item.Client + item.Ordre;
                                string periode = FormatPeriodeMMAAAA(lstDetailClient.First().Periode);
                                string montant = Convert.ToDecimal(lstDetailClient.First().TotFTTC).ToString("#,0.");
                                //string exigible = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(item.detail.First().dateExige);
                                string nomAbon = item.NomAbon;
                                string telephone = string.Empty;
                                param.Add("TypeEdition", "Facture");
                                if (lstDetailClient.First().ISFACTURE.Value == true)
                                {
                                    param.Add("pismail", lstDetailClient.FirstOrDefault(c => c.EMAIL != null && c.EMAIL != "").EMAIL);
                                    new WcfService.Printings.PrintingsService().ActionMail<CsFactureClient, CsFactureClient>(lstDetailClient, param, "FactureSimpleMail", "Facturation", Matricule);
                                }
                                if (lstDetailClient.First().ISSMS.Value == true)
                                {
                                    #region Envoi de sms
                                    //telephone = lstDetailClient.FirstOrDefault(c => c.TELEPHONE != null && c.TELEPHONE != "").TELEPHONE;
                                    //if (!string.IsNullOrWhiteSpace(telephone))
                                    //{
                                    //    string message_sms1 = "Chere:" + nomAbon + "(" + refclient + "), nous vous informons de la disponibilité de votre facture du :" + periode + " de " + montant + "FCFA.";
                                    //    string message_sms2 = "Vous pouvez vous présenter à nos guichets ou utiliser le service Orange Money pour réglement .Merci de votre fidélité";
                                    //    Utility.SendToSmsHandler(message_sms1, telephone);
                                    //    Utility.SendToSmsHandler(message_sms2, telephone);

                                    //}
                                    #endregion
                                }

                            }

                            return true;

            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }

        //public List<CsFactureClient> EnvoyerFactures(List<CsEnteteFacture> facturesAEnvoyer, string Rdlc)
        //{
        //    try
        //    {
        //        DbFacturation db = new DbFacturation();
        //        return new DbFacturation().RetourneFacturesAbo07(facturesAEnvoyer, Rdlc);


        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}

        //public List<CsFactureClient> EnvoyerFactures(List<CsEnteteFacture> facturesAEnvoyer, string key, Dictionary<string, string> parametresRDLC)
        //{
        //    try
        //    {
        //        DBEservice db = new DBEservice();
        //        List<CsPrint> listeAImprimer = new List<CsPrint>();
        //        List<string> listeDeCles = new List<string>();
        //        Dictionary<string, string> DicoKeyMail = new Dictionary<string, string>(); // contient une clé pour l'impression associée a un mail
        //        List<CsRedevance> lesredevence = db.ListeDesREdevance();
        //        List<CsProduit> lesProduit = db.ListeDesProduits();
        //        List<CsCategorieClient> lescateClient = db.ListeDeCategorieClient();
        //        List<CsUniteComptage> lesunites = new DBUNITECOMPTAGE().SelectAllUniteComptage();


        //        List<CsFactureClient> laFactures = new List<CsFactureClient>();

        //        //foreach (CsEnvoiMail facture in facturesAEnvoyer)
        //        //{
        //        //    List<CsFactureClient> laFactures = ListeDesFactures(facture.centre, facture.client, facture.ordre, facture.periode, facture.numfact);
        //        //    listeAImprimer.AddRange (laFactures);
        //        //}

        //        //    Printings.PrintingsService service = new Printings.PrintingsService();
        //        //  return   service.setFromWebPart(listeAImprimer, key, parametresRDLC);

        //        foreach (CsEnvoiMail facture in facturesAEnvoyer)
        //        {
        //            List<CsFactureClient> laFacturescl = new List<CsFactureClient>();
        //            laFacturescl = ListeDesFactures(facture.centre, facture.client, facture.ordre, facture.periode, facture.numfact);
        //            laFactures.AddRange(laFacturescl);
        //        }
        //        return laFactures;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}
        //public List<CsFactureClient> ListeDesFactures(string centre, string client, string ordre, string periode, string numFacture)
        //{
        //    try
        //    {
        //        DBEservice db = new DBEservice();
        //        List<CsFactureClient> Retour = new List<CsFactureClient>();
        //        List<CsFactureClient> list = db.ListeDesFactures(centre, client, ordre, periode, numFacture);
        //        foreach (var item in list)
        //        {
        //            if (item.ISFACTURE || item.ISSMS)
        //            {
        //                Retour.Add(item);
        //            }
        //        }
        //        return Retour;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}


        public List<CsFactureClient> RetourneFactures(List<CsEnteteFacture> LstFacture, string rdl, string key)
        {

            try
            {
                DbFacturation db = new DbFacturation();
                List<CsFactureClient> lstFacture = new List<CsFactureClient>();
                lstFacture = db.RetourneFacturesAbo07(LstFacture, rdl);
                return lstFacture;
                //List<CsPrint> listeDeFactures = new List<CsPrint>();
                //foreach (CsFactureClient facture in lstFacture)
                //    listeDeFactures.Add(facture);

                //Printings.PrintingsService printService = new Printings.PrintingsService();
                //return printService.setFromWebPart(listeDeFactures, key, null );
            }
            catch (Exception ex)
            {

                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        //public List<CsEvenement> RetourneDernierEvenementDeLaCanalisation(CsAbon leAbonnement)
        //{
        //    try
        //    {
        //        DBAccueil db = new DBAccueil();
        //        return db.RetourneDernierEvenementDeLaCanalisation(leAbonnement);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message);
        //        return null;
        //    }
        //}
        public List<CsAbon> RetourneAbon(int fk_idcentre, string Centre, string Client, string Ordre)
        {
            try
            {

                DBAccueil db = new DBAccueil();
                return db.RetourneAbon(fk_idcentre, Centre, Client, Ordre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public List<CsAnnomalie> RetourneLesteAnomalie(string lotri, int idCentre)
        {
            try
            {

                DBCalcul db = new DBCalcul();
                return db.RetourneLesteAnomalie(lotri, idCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public void SupprimeAnnomalie(string lotri, int idCentre)
        {
            try
            {

                DBCalcul db = new DBCalcul();
                db.SupprimeAnnomalie(lotri, idCentre);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
            }
        }
        public bool PurgeDeLot(List<CsLotri> leslotri)
        {
            try
            {

                DBCalcul db = new DBCalcul();
                return db.PurgeDeLot(leslotri);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsEnteteFacture> retourneFactureDecroissance(List<int> lstIdCentre, CsLotri leLot)
        {
            try
            {

                DbFacturation db = new DbFacturation();
                return db.retourneFactureDecroissance(lstIdCentre, leLot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsClient> VerifieClient(List<CsLotri> lesLot, string Client)
        {
            try
            {

                DbFacturation db = new DbFacturation();
                return db.VerifieClient(lesLot, Client);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool VerifieExisteLotClient(CsClient LeClient, string Numlot)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.VerifieExisteLotClient(LeClient, Numlot);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsEvenement> RetourneEvenementClient(CsClient LeClient)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneEvenement(LeClient);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        public List<CsEvenement> RetourneEvenementCorrectionIndex(CsClient LeClient)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.RetourneEvenementCorrectionIndex(LeClient);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool? InsererLstEvenement(List<CsEvenement> pEvenement)
        {
            try
            {
                DBIndex db = new DBIndex();
                return db.InsererLstEvenement(pEvenement);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return false;
            }
        }
        public List<CsComparaisonFacture> ComparaisonFacture(List<int> LstCentre, string PerideDebut, string Lotri1, string PeriodeFin, string Lotri2, bool IsMT)
        {
            try
            {
                DbFacturation db = new DbFacturation();
                return db.ControleFacturationSpx(LstCentre,PerideDebut,Lotri1,PeriodeFin,Lotri2,IsMT);
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }
        #endregion

        private void LireParametresMail()
        {
            from = System.Configuration.ConfigurationManager.AppSettings["MailSender"];
            subject = System.Configuration.ConfigurationManager.AppSettings["MailSubject"];
            body = System.Configuration.ConfigurationManager.AppSettings["MailBody"];
            smtp = System.Configuration.ConfigurationManager.AppSettings["MailSmtp"];
            portSmtp = System.Configuration.ConfigurationManager.AppSettings["MailPort"];
            sslEnabled = System.Configuration.ConfigurationManager.AppSettings["MailSslEnabled"];
            login = System.Configuration.ConfigurationManager.AppSettings["MailLogin"];
            password = System.Configuration.ConfigurationManager.AppSettings["MailPassword"];

            login = DecrypterText(login);
            password = DecrypterText(password);

        }







        private static System.Security.Cryptography.SymmetricAlgorithm _mCSP = new System.Security.Cryptography.TripleDESCryptoServiceProvider();

        //Ces valeurs ne doivent pas être modifiées sinon les éléments cryptés avec la clé et le vecteur ci-dessous ne seront pas décryptables
        //Les valeurs ci-dessous sont définies de manières arbitraires
        private static byte[] _Cle = new byte[24] { 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
        private static byte[] _Vecteur = new byte[8] { 130, 206, 100, 99, 35, 128, 72, 225 };

        #region Cryptage de Chaine

        //Si on n'a pas besoin de decrypter la donnée cryptée alors on utilise le hachage
        public static string HasherText(string TextAHasher)
        {
            try
            {
                System.Security.Cryptography.SHA1CryptoServiceProvider SHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider(); ;

                // Convertit le tring en tableau de Bytes
                byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(TextAHasher);

                // Execute le hachage, retourne un tableau de bytes
                byte[] bytHash = SHA1.ComputeHash(bytValue);

                SHA1.Clear();

                // Return a base 64 encoded string of the Hash value
                return Convert.ToBase64String(bytHash);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        //Si on a besoin de decrypter la donnée cryptée alors on utilise le cryptage      
        public static string CrypterText(string DonneeACrypter)
        {
            try
            {
                if (DonneeACrypter == null)
                    DonneeACrypter = "";

                System.Security.Cryptography.ICryptoTransform ct; //Une interface utilisée pour pouvoir appeler la méthode CreateEncryptor sur les fournisseurs de services, qui renverront un objet encryptor 
                MemoryStream ms;
                System.Security.Cryptography.CryptoStream cs;
                byte[] byt;

                ct = _mCSP.CreateEncryptor(_Cle, _Vecteur);

                byt = System.Text.Encoding.UTF8.GetBytes(DonneeACrypter); //convertir la chaîne originale en un tableau d'octets

                ms = new MemoryStream(); //la création d'un flux dans lequel écrire les octets cryptés
                cs = new System.Security.Cryptography.CryptoStream(ms, ct, System.Security.Cryptography.CryptoStreamMode.Write); //mode dans lequel vous voulez créer cette classe (lecture, écriture, etc.). 
                cs.Write(byt, 0, byt.Length); //écrire les données dans le flux de mémoire en utilisant la méthode Write de l'objet CryptoStream. C'est elle qui exécute concrètement le cryptage et, à mesure que chaque bloc de données est crypté, les informations sont écrites dans l'objet MemoryStream.
                cs.FlushFinalBlock(); //pour vérifier que toutes les données ont été écrites dans l'objet MemoryStream

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static string DecrypterText(string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                    return string.Empty;

                System.Security.Cryptography.ICryptoTransform ct;
                MemoryStream ms;
                System.Security.Cryptography.CryptoStream cs;
                byte[] byt;

                ct = _mCSP.CreateDecryptor(_Cle, _Vecteur);

                byt = Convert.FromBase64String(Value);

                ms = new MemoryStream();
                cs = new System.Security.Cryptography.CryptoStream(ms, ct, System.Security.Cryptography.CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        //Méthode d'origine
        #region Cryptage de fichiers

        public bool CrypterFichier(string unfichier)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            //Encoding leCode;
            StreamReader sr = new StreamReader(unfichier, System.Text.Encoding.Default);
            //string ligne=sr1.ReadLine();


            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".cry");  // Ouverture du fichier Crypté
            FileInfo fi = new FileInfo(nouveauNomDuFichierCrypte(unfichier));  // Ouverture du fichier Crypté

            StreamWriter sw = fi.CreateText();

            //			FileStream fs = fi.Create();

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {

                //Utilisation de la méthode Crypter() :
                resultat = CrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                //				EcrireLigne( resultat,ref fs );
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush(); sw.Close();// Fichier crypté

            return true;
        }
        public bool DecrypterFichier(string unfichier)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            StreamReader sr = File.OpenText(unfichier);
            //CrypteurSYGES cryptage = new CrypteurSYGES();

            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier décrypté
            string NewNomFichier = this.NomDuFichierDecrypte(unfichier);
            if (NewNomFichier == "")
                return false;
            //FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier décrypté

            //StreamWriter sw = fi.CreateText();
            StreamWriter sw = new StreamWriter(NewNomFichier, false, System.Text.Encoding.Default);

            //			FileStream fs = fi.Create();

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {
                //Utilisation de la méthode Crypter() :
                resultat = DecrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush(); sw.Close();// Fichier crypté
            return true;
        }
        public bool DecrypterFichier(string unfichier, string pathResult)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            StreamReader sr = File.OpenText(unfichier);
            //StreamReader sr = new StreamReader(unfichier, Encoding.Default);
            //Creation d'un objet de la classe pour utiliser les méthodes.
            //CrypteurSYGES cryptage = new CrypteurSYGES();

            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier décrypté
            string NewNomFichier = this.NomDuFichierDecrypte(unfichier);
            if (NewNomFichier == "")
                return false;

            if (!Directory.Exists(pathResult))
                return false;
            NewNomFichier = Path.GetDirectoryName(pathResult) + Path.DirectorySeparatorChar + NewNomFichier;

            //FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier décrypté

            //StreamWriter sw = fi.CreateText();

            StreamWriter sw = new StreamWriter(NewNomFichier, false, System.Text.Encoding.Default);

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {
                //Utilisation de la méthode Crypter() :
                resultat = DecrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush(); sw.Close();// Fichier crypté
            return true;
        }


        public void EcrireLigne(string Ligne, ref StreamWriter sw)
        {
            sw.WriteLine(Ligne);
        }

        #region "Gestion de l'extension des fichiers cryptés"
        public string ExtensionDeFichierCrypte()
        {
            return ".cry";
        }


        public bool EstUnFichierCrypte(string FileName)
        {
            return true;
        }



        public string nouveauNomDuFichierCrypte(string ActualFileName)
        {
            return ActualFileName + ExtensionDeFichierCrypte();
        }



        public string AjoutExtensionFichierCrypte(string ActualFileName)
        {
            return nouveauNomDuFichierCrypte(ActualFileName);
        }



        public bool PossedeExtensionDeFichierCrypte(string FileNameToTest)
        {
            string CryptedExtansion = ExtensionDeFichierCrypte();

            //Contrôle des variables en entrée
            if ((FileNameToTest == null) || (FileNameToTest.Length == 0))
                return false;

            //Traitements
            string TestName = FileNameToTest.ToUpper();
            if (TestName.IndexOf(CryptedExtansion.ToUpper()) < (TestName.Length - CryptedExtansion.Length))
                //if (FileNameToTest.IndexOf(CryptedExtansion) < 0)
                return false;

            //Fin de la fonction
            return true;
        }




        public string NomDuFichierDecrypte(string CryptedFileName)
        {
            if (CryptedFileName == null)
                return "";
            if (!this.PossedeExtensionDeFichierCrypte(CryptedFileName))
                return "";

            string NewName = CryptedFileName.Replace(this.ExtensionDeFichierCrypte(), "");

            if ((NewName == CryptedFileName) || (NewName == ""))
                return "";
            else
                return NewName;

        }
        #endregion

        #endregion
    }
}
