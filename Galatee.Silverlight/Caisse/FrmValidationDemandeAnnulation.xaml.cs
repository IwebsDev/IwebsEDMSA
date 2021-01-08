using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceCaisse;
using System.Globalization;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Caisse;
using Galatee.Silverlight.ServiceAdministration;


namespace Galatee.Silverlight.Caisse
{
    public partial class FrmValidationDemandeAnnulation : ChildWindow
    {
        List<CsReglement> _ListeDesReglementAnnuler = new List<CsReglement>();
        List<CsReglement> _LstrefReglement = new List<CsReglement>();
        List<CsUtilisateur> LstCaissier = new List<CsUtilisateur>();
        List<CsLclient> _ListeDesReglementDuRecu = new List<CsLclient>();

        List<CsUtilisateur> _ListeDesUtilisateurs = new List<CsUtilisateur>();
        List<CsReglement> _ListeDesReglement;
        int TailleRefcli = SessionObject.Enumere.TailleCentre +
                           SessionObject.Enumere.TailleClient +
                           SessionObject.Enumere.TailleOrdre;
        string ServerMode = SessionObject.ServerEtatInit;

        public FrmValidationDemandeAnnulation()
        {
            InitializeComponent();
            try
            {
                //translate();
                //NumCaisse = UserConnecte.numcaisse;
                MatriculeCaisse = UserConnecte.matricule;
            }
            catch (Exception x)
            {
                Message.ShowError(x, Langue.errorTitle);
            }
        }
        void translate()
        {
            try
            {
                this.dtg_FactureAnnule.Columns[0].Header = Langue.Mode_paiement;
                this.dtg_FactureAnnule.Columns[1].Header = Langue.Montant_recu;
                this.OKButton.Content = Langue.Btn_ok;
                this.CancelButton.Content = Langue.Btn_annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string MatriculeCaisse = UserConnecte.matricule;
        //string NumCaisse = UserConnecte.numcaisse;
        List<CsLclient> ListeDesReglementADupliquer = new List<CsLclient>();

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<CsLclient> lstRecuAnnuler = ((List<CsLclient>)this.dtg_FactureAnnule.ItemsSource).Where(t => t.Selectionner).ToList();
                List<CsLclient> _lstReglement = _ListeDesReglementDuRecu.Where(t =>t.MATRICULE ==lstRecuAnnuler.First().MATRICULE &&  lstRecuAnnuler.Select(y=>y.ACQUIT).Contains(t.ACQUIT )).ToList();
                validerAnnulation(_lstReglement);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }
        public void validerAnnulation(List<CsLclient> _lstReglement)
        {
            try
            {
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.ValiderAnnuleEncaissementAsync(Galatee.Silverlight.Caisse.MethodeGenerics.RetourneDistincAcquitement(_lstReglement));
                srv.ValiderAnnuleEncaissementCompleted  += (factor, es) =>
                {
                    if (es.Error != null || es.Cancelled)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp! :" + es.Error.Message, "Erreur");
                        return;
                    }
                    if (es.Result == null)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp!", "Erreur");
                        return;
                    }
                EditionDeRecu(_lstReglement);
                this.OKButton.IsEnabled = false;
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void LanceImpression(string key, Dictionary<string, string> parametresRDLC, List<Galatee.Silverlight.ServiceCaisse.CsReglementRecu> res, bool IsBordero)
        {
            try
            {

                if (!IsBordero)
                    Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "recu", "Caisse", true);
                else
                    Utility.ActionDirectOrientation<ServicePrintings.CsReglementRecu, ServiceCaisse.CsReglementRecu>(res, parametresRDLC, SessionObject.CheminImpression, "BorderoRegroupe", "Caisse", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void EditionDeRecu(List<CsLclient> _ListEdition)
        {
            try
            {

                List<CsReglementRecu> _ListeDesRecu = new List<CsReglementRecu>();
                List<CsClient> _lstClientRecu = Galatee.Silverlight.Caisse.MethodeGenerics.RetourneClientFromFacture(_ListeDesReglementDuRecu);
                foreach (CsClient item in _lstClientRecu)
                    RetourneEtatClient(item);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            dtg_FactureAnnule.ItemsSource = null;
            ListeDesReglementADupliquer.Clear();
        }
        private void RemplireDataGrid(List<CsLclient> _LstReglement)
        {
            try
            {
                var _lstfacture = (from p in _LstReglement
                                   group new { p } by new { p.FK_IDHABILITATIONCAISSE,  p.NOMCAISSIERE, p.ACQUIT, p.DENR, p.MATRICULE, p.MOTIFANNULATION, p.IsDEMANDEANNULATION, p.CAISSE,p.NUMDEM  } into pResult
                                   select new
                                   {
                                       pResult.Key.ACQUIT,
                                       pResult.Key.DENR,
                                       pResult.Key.MATRICULE,
                                       pResult.Key.MOTIFANNULATION,
                                       pResult.Key.IsDEMANDEANNULATION,
                                       pResult.Key.NOMCAISSIERE,
                                       pResult.Key.CAISSE,
                                       pResult.Key.FK_IDHABILITATIONCAISSE,
                                       pResult.Key.NUMDEM ,
                                       MONTANT = (decimal?)pResult.Sum(o => o.p.MONTANT)
                                   }).ToList();

                foreach (var item in _lstfacture)
                {
                    CsUtilisateur leUSer = _ListeDesUtilisateurs.FirstOrDefault(p => p.MATRICULE == item.MATRICULE);

                    CsLclient leReglement = new CsLclient()
                    {
                        //REFFERENCECLIENT = item.REFFERENCECLIENT,
                        ACQUIT = item.ACQUIT,
                        //NOM = item.NOM,
                        DENR = item.DENR,
                        NUMDEM  = item.NUMDEM ,
                        MONTANT = item.MONTANT,
                        MOTIFANNULATION = item.MOTIFANNULATION,
                        IsDEMANDEANNULATION = item.IsDEMANDEANNULATION,
                        MATRICULE = item.MATRICULE,
                        CAISSE = item.CAISSE,
                        FK_IDHABILITATIONCAISSE = item.FK_IDHABILITATIONCAISSE,
                        NOMCAISSIERE = (leUSer != null) ? (leUSer.LIBELLE + "(" + item.MATRICULE + ")") : item.MATRICULE
                    };
                    ListeDesReglementADupliquer.Add(leReglement);
                }
                dtg_FactureAnnule.ItemsSource = null;
                dtg_FactureAnnule.ItemsSource = ListeDesReglementADupliquer;
            }
            catch (Exception ex)
            {

            }
        }
        private void FrmDuplicatEncaissement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerDonneeDuSite();
            }
            catch (Exception EX)
            {
                Message.ShowError(EX, Langue.errorTitle);
            }
        }
        void RetourneReglementEncaisse(List<int> lesCentreCaisse)
        {
            try
            {
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.RetourneEncaissementPourValidationAnnulationAsync(lesCentreCaisse);
                srv.RetourneEncaissementPourValidationAnnulationCompleted += (insers, resultins) =>
                {
                    try
                    {
                        if (resultins.Cancelled || resultins.Error != null)
                        {
                            string error = resultins.Error.Message;
                            Message.ShowInformation(error, Langue.errorTitle);
                            return;
                        }
                        _ListeDesReglementDuRecu = resultins.Result;
                        if (_ListeDesReglementDuRecu != null && _ListeDesReglementDuRecu.Count != 0)
                            RemplireDataGrid(_ListeDesReglementDuRecu);
                        else
                            Message.ShowError(Langue.msg_pas_de_facture, Langue.informationTitle);

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Langue.informationTitle);
                    }
                };

            }
            catch (Exception es)
            {

                Message.ShowError(es.Message, Langue.informationTitle + "=> RetourneReglementEncaisse");
            }

        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                List<Galatee.Silverlight.ServiceAccueil.CsCentre> _lesCentre = new List<ServiceAccueil.CsCentre>();
                List<int> lesCentreCaisse = new List<int>();

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    _lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                    RetourneReglementEncaisse(lesCentreCaisse);
                    RetourneCaisseHabille(lesCentreCaisse);
                    return;

                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    _lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _lesCentre)
                        lesCentreCaisse.Add(item.PK_ID);
                    RetourneReglementEncaisse(lesCentreCaisse);
                    RetourneCaisseHabille(lesCentreCaisse);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<CsHabilitationCaisse> _listDesCaisseOuverte = new List<CsHabilitationCaisse>();
        private void RetourneCaisseHabille(List<int> LstCentreCaisse)
        {
            try
            {
                CaisseServiceClient service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint(this));
                service.RetourneCaisseNonClotureAsync(LstCentreCaisse);
                service.RetourneCaisseNonClotureCompleted += (sender, es) =>
                {
                    try
                    {
                        if (es.Cancelled || es.Error != null || es.Result == null)
                        {
                            Message.ShowError("Erreur d'annulation du reçu. Veuillez réessayer svp !", Langue.errorTitle);
                            return;
                        }
                        _listDesCaisseOuverte = es.Result;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //LstCaissier=   ChargerCaissier(_LstrefReglement.Where(p => p.ISDEMANDEANNULATION == true).ToList());
                List<object> _LstCaissiere = ClasseMEthodeGenerique.RetourneListeObjet(_listDesCaisseOuverte);
                //List<string> lstcolonne = new List<string>();
                Dictionary<string, string> lstcolonne = new Dictionary<string, string>();
                lstcolonne.Add("CENTRE", "CENTRE");
                lstcolonne.Add("NUMCAISSE", "CAISSE");
                lstcolonne.Add("NOMCAISSE", "NOM");
                Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstCaissiere, lstcolonne, false, "Liste des caissiers");
                ctr.Show();
                ctr.Closed += new EventHandler(galatee_OkClickedCaissiere);

            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Langue.LibelleModule);
            }
        }
        void galatee_OkClickedCaissiere(object sender, EventArgs e)
        {
            try
            {
                CsHabilitationCaisse _LeCaisseSelect = new CsHabilitationCaisse();
                Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
                if (ctrs.MyObject != null)
                {
                    _LeCaisseSelect = (CsHabilitationCaisse)ctrs.MyObject;
                    this.txt_Caissiere.Text = _LeCaisseSelect.MATRICULE;
                    if (ListeDesReglementADupliquer != null && ListeDesReglementADupliquer.Count != 0)
                        ListeDesReglementADupliquer.Clear();
                    RemplireDataGrid(_ListeDesReglementDuRecu.Where(p => p.MATRICULE == _LeCaisseSelect.MATRICULE).ToList());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void dtg_FactureAnnule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //CsLclient _leRecuSelect = (CsLclient)this.dtg_FactureAnnule.SelectedItem;
                //if (_leRecuSelect != null)
                //    _leRecuSelect.Selectionner = _leRecuSelect.Selectionner ? false : true;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "=> dtg_FactureAnnule_SelectionChanged");
            }
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;
                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    this.dtg_FactureDetail.ItemsSource = null;
                    this.dtg_FactureDetail.ItemsSource = _ListeDesReglementDuRecu.Where(t =>t.MATRICULE ==SelectedObject.MATRICULE &&  t.ACQUIT == SelectedObject.ACQUIT).ToList(); ;
                }
                else
                {
                    SelectedObject.Selectionner = false;
                    this.dtg_FactureDetail.ItemsSource = null;
                }
            }
        }
        private void ChildWindow_MouseMove_1(object sender, MouseEventArgs e)
        {

        }




        private void galatee_OkClickedRaisonAnnulation(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.Caisse.FrmMotifAction ctrs = sender as Galatee.Silverlight.Caisse.FrmMotifAction;
                if (ctrs.IsValide)
                {
                    foreach (CsLclient item in ctrs.lstReglementAnnuler)
                        item.MOTIFREJET = item.MOTIFANNULATION;

                    List<CsLclient> _lstReglement = ((List<CsLclient>)dtg_FactureAnnule.ItemsSource).Where(r => r.Selectionner == true).ToList();
                    validerRejetAnnulation(ctrs.lstReglementAnnuler);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void validerRejetAnnulation(List<CsLclient> _lstReglement)
        {
            try
            {
                this.DialogResult = true;
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.RejeterAnnuleEncaissementAsync(_lstReglement);
                srv.RejeterAnnuleEncaissementCompleted += (factor, es) =>
                {
                    if (es.Error != null || es.Cancelled)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp! :" + es.Error.Message, "Erreur");
                        return;
                    }
                    if (es.Result == null)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp!", "Erreur");
                        return;
                    }
                    this.OKButton.IsEnabled = false;
                };
            }
            catch (Exception)
            {

                throw;
            }
        }



        public void RetourneEtatClient(CsClient leClient)
        {
            try
            {
                this.DialogResult = true;
                CaisseServiceClient srv = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                srv.RetourneEtatClientAsync(leClient.PK_ID);
                srv.RetourneEtatClientCompleted += (factor, es) =>
                {
                    if (es.Error != null || es.Cancelled)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp! :" + es.Error.Message, "Erreur");
                        return;
                    }
                    if (es.Result == null)
                    {
                        Message.ShowError("L'insertion des encaissements a retournée une erreur. Réessayer svp!", "Erreur");
                        return;
                    }
                    decimal? MontantExigible = es.Result.Where(t => t.EXIGIBILITE < System.DateTime.Today.Date ).Sum(y => y.MONTANT);
                    decimal? MontantNonExigible = es.Result.Where(t => t.EXIGIBILITE > System.DateTime.Today.Date ).Sum(y => y.MONTANT);
                    decimal? Solde = MontantExigible + MontantNonExigible;
                    List<CsLclient> lstFactureDuClient = _ListeDesReglementDuRecu.Where(t => t.FK_IDCLIENT == leClient.PK_ID).ToList();

                    foreach (CsLclient item in lstFactureDuClient)
                    {
                        item.MONTANTEXIGIBLE = MontantExigible;
                        item.MONTANTNONEXIGIBLE = MontantNonExigible;
                        item.SOLDECLIENT = Solde;
                    }
                    List<CsReglementRecu> _ListeDesRecu = Galatee.Silverlight.Caisse.EditionRecu.ReorganiserReglement(lstFactureDuClient, SessionObject.Enumere.ActionRecuDuplicat);
                    int nbreDeCopie = 2;
                    for (int i = 1; i <= nbreDeCopie; i++)
                    {
                        string key = Utility.getKey();
                        Dictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("pTypeRecu", "RECU D'ANNULATION D'ENCAISSEMENT");
                        LanceImpression(key, param, _ListeDesRecu, false);
                    }
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                CsLclient leReglementSelect = (CsLclient)dtg_FactureAnnule.SelectedItem;
                if (leReglementSelect != null)
                {

                    List<CsLclient> _lstReglement = ((List<CsLclient>)dtg_FactureAnnule.ItemsSource).Where(r => r.Selectionner == true).ToList();
                    Galatee.Silverlight.Caisse.FrmMotifAction ctrl = new Galatee.Silverlight.Caisse.FrmMotifAction(_lstReglement);
                    ctrl.Closed += new EventHandler(galatee_OkClickedRaisonAnnulation);
                    ctrl.Show();
                }



            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

    }
}

