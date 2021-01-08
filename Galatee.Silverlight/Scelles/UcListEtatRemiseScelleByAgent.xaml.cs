using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.ServiceScelles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Scelles
{
    public partial class UcListEtatRemiseScelleByAgent : ChildWindow
    {
        List<ServiceAccueil.CsUtilisateur> lstAllUser = new List<ServiceAccueil.CsUtilisateur>();
        Galatee.Silverlight.ServiceAccueil.CsDemandeBase laDemandeSelect = null;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        ObservableCollection<CsRetourScelles> donnesDatagridScelle = new ObservableCollection<CsRetourScelles>();
        ObservableCollection<CsRemiseScelleByAg> donnesDatagridRemise = new ObservableCollection<CsRemiseScelleByAg>();
        int StatutScelle;
        public UcListEtatRemiseScelleByAgent()
        {
            InitializeComponent();
            ChargerListDesSite();
            ChargeListeUser();

            RemplirListeCmbDesStatutScelles();
         
        }
        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;

                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        //private void GetDatascelle(int PK_Agt,int SatutScelle)
        //{
        //    try
        //    {
        //        //UcReceptionLotScellesMagasinGeneral ctrl = new UcReceptionLotScellesMagasinGeneral();
        //        //ctrl.Closed += new EventHandler(RafraichirList);
        //        //ctrl.Show();
        //        IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
        //        client.SelectAllRetourScelleCompleted += (ssender, args) =>
        //        {
        //            if (args.Cancelled || args.Error != null)
        //            {
        //                string error = args.Error.Message;
        //                Message.ShowError(error, Languages.LibelleReceptionScelle);
        //                return;
        //            }
        //            if (args.Result == null)
        //            {
        //                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
        //                return;
        //            }

        //            donnesDatagridScelle.Clear();
        //            if (args.Result != null)
        //            {
        //                foreach (var item in args.Result.Where(x => x.Status_ID == SatutScelle && x.Receveur_Mat == PK_Agt.ToString().Trim()))
        //                {
        //                    //if(item.DateReception==DateTime.Now)
        //                    donnesDatagridScelle.Add(item);
        //                }
        //            }
        //            dgScelleR.ItemsSource = donnesDatagridScelle;

        //        };
        //        client.SelectAllRetourScelleAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirListeCmbDesStatutScelles()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllStatutsScellesCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Scelles);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        this.Cbo_Motif.ItemsSource = args.Result;
                        this.Cbo_Motif.DisplayMemberPath = "Status";
                        this.Cbo_Motif.SelectedValuePath = "Status_ID";
                            foreach (CsRefStatutsScelles OgSll in Cbo_Motif.ItemsSource)
                            {
                                if (OgSll.Status_ID !=null && OgSll.Status_ID != SessionObject.Enumere.StatusScelleDisponible )
                                {
                                    Cbo_Motif.SelectedItem = OgSll;
                                    break;
                                }
                            }
                        
                    }
                };
                client.SelectAllStatutsScellesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ListeScelleExistant(int PK_Agt,int SatutScelle)
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListScelleByStatusCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    donnesDatagridRemise.Clear();
                    if (args.Result != null)
                    {
                        foreach (var item in args.Result )
                            donnesDatagridRemise.Add(item);
                    }
                    dgScelle.ItemsSource = donnesDatagridRemise;

                };
                client.RetourneListScelleByStatusAsync(PK_Agt, SatutScelle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCentrePerimetre(List<ServiceAccueil.CsCentre> lstCentre, List<CsSite> lstSite)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                        Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();

        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();

        void ChargerListDesSite()
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
                    RemplirCentrePerimetre(lesCentre, lesSite);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            RemplirCentrePerimetre(lesCentre, lesSite);
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        //RemplirCommuneParCentre(centre);
                        //RemplirProduitCentre(centre);
                    }
                    //VerifierDonneesSaisiesInformationsDevis();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Caisse);
            }
        }

        private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Site.SelectedItem != null)
                {
                    var csSite = Cbo_Site.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        this.txtSite.Text = csSite.CODE  ?? string.Empty;
                        if (laDemandeSelect != null)
                        {
                            if (laDemandeSelect.FK_IDCENTRE != 0)
                                RemplirCentreDuSite(csSite.PK_ID, laDemandeSelect.FK_IDCENTRE);
                        }
                        else
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
                //VerifierDonneesSaisiesInformationsDevis();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.txt);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
           // int id_satut = (strin)Cbo_Motif.SelectedValuePath;
            if (this.Cbo_Motif.SelectedItem != null)
            {
                this.dgScelle.ItemsSource = null;
                StatutScelle = ((CsRefStatutsScelles)Cbo_Motif.SelectedItem).Status_ID;
                ListeScelleExistant((int)txtAgt_M.Tag, StatutScelle);
            }

            //if (((CsRefStatutsScelles)Cbo_Motif.SelectedItem).Status == "Remis")
            //{
            //     StatutScelle =2 ;
            //     GetDatascelle((int)txtAgt_M.Tag, StatutScelle);
                
            //}
            //if (((CsRefStatutsScelles)Cbo_Motif.SelectedItem).Status == "Abîmé")
            //{
            //    dgScelle.Visibility = Visibility.Collapsed;
            //    dgScelleR.Visibility = Visibility.Visible;
            //    StatutScelle = 0;
            //    GetDatascelle((int)txtAgt_M.Tag, StatutScelle);
               
              
            //}
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_SearchAgt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c => c.CENTRE == UserConnecte.Centre).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelleAg);
                    ctr.Show();

                }
                else
                {
                    Message.ShowInformation("  ", "Information");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtAgt_M_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_M.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_M.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleAgentScelle.Text = leuser.LIBELLE;
                        txtAgt_M.Tag = leuser.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtAgt_M.Focus();
                    }
                }
            }
        }

        void galatee_OkClickedbtn_SearchScelleAg(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtAgt_M.Text = utilisateur.LIBELLE;
                this.txtAgt_M.Tag = utilisateur.PK_ID;

            }

        }

        Dictionary<string, string> param = null;
        List<CsRemiseScelleByAg> lstDonnee = new List<CsRemiseScelleByAg>();
        private void Imprimer()
        {
            param = new Dictionary<string, string>();
            try
            {
                if (dgScelle.ItemsSource != null )
                {
                    foreach (CsRemiseScelleByAg item in donnesDatagridRemise)
                        lstDonnee.Add(item);
                    lstDonnee.ForEach(t => t.lot_ID = this.txt_LibelleAgentScelle.Text);
                    if (this.Cbo_Motif.SelectedItem != null )
                    lstDonnee.ForEach(t => t.libelleStatus  =((CsRefStatutsScelles) this.Cbo_Motif.SelectedItem ).Status );

                }
                Galatee.Silverlight.Shared.FrmOptionEditon ctrl = new Shared.FrmOptionEditon();
                ctrl.Closed += ctrl_Closed;
                this.IsEnabled = false;
                ctrl.Show();

            }
            catch (Exception ex)
            {

            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            Galatee.Silverlight.Shared.FrmOptionEditon ctrs = sender as Galatee.Silverlight.Shared.FrmOptionEditon;
            if (ctrs.IsOptionChoisi)
            {
                
                if (ctrs.OptionSelect == SessionObject.EnvoiPrinter)
                    Utility.ActionDirectOrientation<ServicePrintings.CsRemiseScelleByAg, ServiceScelles.CsRemiseScelleByAg>(lstDonnee, param, SessionObject.CheminImpression, "ScelleRemis", "Report", true);
                else if (ctrs.OptionSelect == SessionObject.EnvoiExecl)
                    Utility.ActionExportation<ServicePrintings.CsRemiseScelleByAg, ServiceScelles.CsRemiseScelleByAg>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "ScelleRemis", "Report", true, "xlsx");

                else if (ctrs.OptionSelect == SessionObject.EnvoiWord)
                    Utility.ActionExportation<ServicePrintings.CsRemiseScelleByAg, ServiceScelles.CsRemiseScelleByAg>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "ScelleRemis", "Report", true, "doc");

                else if (ctrs.OptionSelect == SessionObject.EnvoiPdf)
                    Utility.ActionExportation<ServicePrintings.CsRemiseScelleByAg, ServiceScelles.CsRemiseScelleByAg>(lstDonnee, param, string.Empty, SessionObject.CheminImpression, "ScelleRemis", "Report", true, "pdf");
            }
        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            Imprimer();
        }

    }
}

