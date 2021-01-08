using Galatee.Silverlight.ServiceAccueil;
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

namespace Galatee.Silverlight.Devis
{
    public partial class UcValidationDemandeScelle : ChildWindow
    {

        int EtapeActuelle;
        List<CsDscelle> lademande = new List<CsDscelle>();
        List<CsActivite> lstAllActivite = new List<CsActivite>();
        List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur> lstAllUser = new List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur>();

        public UcValidationDemandeScelle(List<int> demande, int etape)
        {
            InitializeComponent();

            this.EtapeActuelle = etape;

            ChargeDonneDemande(demande.First());
           
            ActiveInfoAutreCentreForunisseur(false);
            chk_IsMagazinGeneral.IsChecked = true;
            AfficherInfoDemandeur();
        }

        private List<CsCentre> _listeDesCentreExistant = null;
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> CentreHabiliter = new List<ServiceAccueil.CsCentre>();
        public UcValidationDemandeScelle()
        {
            InitializeComponent();

            ActiveInfoAutreCentreForunisseur(false);
            chk_IsMagazinGeneral.IsChecked = true;
            AfficherInfoDemandeur();
        }

        private void ActiveInfoAutreCentreForunisseur(bool visible)
        {
            Visibility visibilite = visible == true ? Visibility.Visible : Visibility.Collapsed;

            txtCentreFourniseur.Visibility = visibilite;
            txtSite_Fournisseur.Visibility = visibilite;
            lbl_CentreFournisseur.Visibility = visibilite;
            //cboCentreFournisseur.Visibility = visibilite;
            //txtSite_Fournisseur.Visibility = visibilite;
            //Cbo_Site_Fournisseur.Visibility = visibilite;
            lbl_Site_Fournisseur.Visibility = visibilite;
        }

        private void AfficherInfoDemandeur()
        {
            //ChargerListDesCentreHabiliter();
            ChargerListDesSite();
            ChargeListeUser();
            //txtDemandeur.Text = UserConnecte.nomUtilisateur;
            //txtDemandeur.Tag = UserConnecte.PK_ID;
        }


        private void ChargeDonneDemande(int pk_id)
        {

            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneListeDemandeScelleCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    lademande = args.Result;

                    RenseignerInformationDemande();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur ChargeDonneDemande");
                }

            };
            service.RetourneListeDemandeScelleAsync(pk_id);

        }
        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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

        private void RenseignerInformationDemande()
        {
            if (lademande != null && lademande.Count()>0)
            {

                    this.txtCouleurScelle.Tag = lademande.FirstOrDefault().FK_IDCOULEURSCELLE;
                    this.txtCouleurScelle.Text  = lademande.FirstOrDefault().LIBELLECOULEUR ;

                    txtServiceDemandeur.Tag = lademande.FirstOrDefault().FK_IDACTIVITE;
                    txtServiceDemandeur.Text  = lademande.FirstOrDefault().LIBELLEACTIVITE ;

                    Txt_NumDemande.Text = lademande.FirstOrDefault().NUMDEM;
                    txtDemandeur.Text = "( " + lademande.FirstOrDefault().MATRICULE + " )" + lademande.FirstOrDefault().LIBELLEAGENT;
                    txtSite.Text = lademande.FirstOrDefault().LIBELLESITEAGENT;
                    txtCentreDemandeur.Text = lademande.FirstOrDefault().LIBELLECENTREDESTINATAIRE;
                    txtCentreDemandeur.Tag = lademande.FirstOrDefault().FK_IDCENTRE ;

                    txtCentreFourniseur.Text = lademande.FirstOrDefault().LIBELLECENTREFOURNISSEUR;

                    chk_IsMagazinGeneral.IsChecked = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR == SessionObject.Enumere.IDGenerale ? true : false;
                    cboCentreFournisseur.SelectedValue = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR;
                
                    ServiceAccueil.CsCentre lCentreFournisseurDemande=SessionObject.LstCentre.FirstOrDefault (c => c.PK_ID == lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR);
                    if (lCentreFournisseurDemande != null )
                    {
                        txtCentreFourniseur.Text = lCentreFournisseurDemande.LIBELLE ; 
                        txtSite_Fournisseur.Text = lCentreFournisseurDemande.LIBELLESITE  ; 

                    }
                    var lstCentre_Demande = SessionObject.LstCentre.Where(c => c.PK_ID == lademande.FirstOrDefault().FK_IDCENTRE);
                    var Centre_Demande = lstCentre_Demande != null ? lstCentre_Demande.FirstOrDefault() : null;

                    txtNombre.Text = lademande.FirstOrDefault().NOMBRE_DEM.ToString();

                    string NombreScelle = lademande.FirstOrDefault().NOMBRE_DEM != null ? lademande.FirstOrDefault().NOMBRE_DEM.ToString() : string.Empty;
                    string Couleur = lademande.FirstOrDefault().LIBELLECOULEUR != null ? lademande.FirstOrDefault().LIBELLECOULEUR : string.Empty;

                    int IdCentreRecuperationDeLot = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR;
            }
        }

        void ChargerListDesCentreHabiliter()
        {
            List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LstCentre = args.Result;
                        SessionObject.LstCentre = LstCentre;
                        if (LstCentre.Count != 0)
                        {
                            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetreTotal(LstCentre, UserConnecte.listeProfilUser);

                            CentreHabiliter = lesCentre;

                            //cboCentreDemandeur.ItemsSource = CentreHabiliter;
                            //cboCentreDemandeur.DisplayMemberPath = "LIBELLE";
                            //cboCentreDemandeur.SelectedValuePath = "PK_ID";
                            RenseignerInformationDemande();
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur ChargerListDesCentreHabiliter");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur ChargerListDesCentreHabiliter");
            }
        }

        void ChargerListDesSite()
        {
            try
            {
                SessionObject.ModuleEnCours = "Accueil";
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
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
                        }
                        else
                        {
                            Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur ChargerListDesSite");
                    }
                };
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur ChargerListDesSite");
            }
        }

      private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));
            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de validation", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de validation", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                     Message.ShowInformation("Demande validée","Demande");
                     this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        private void RemplirCentreDuSiteFournisseur(int pIdSite, int pIdcentre)
        {
            try
            {
                cboCentreFournisseur.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            cboCentreFournisseur.Items.Add(item);
                        }
                    //cboCentreDemandeur.ItemsSource = lesCentreDuPerimetreAction;
                    cboCentreFournisseur.SelectedValuePath = "PK_ID";
                    cboCentreFournisseur.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.cboCentreFournisseur.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.cboCentreFournisseur.SelectedItem = _listeDesCentreExistant.First();

                    RenseignerInformationDemande();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void transmettreDemande()
        {
            try
            {

                this.OKButton.IsEnabled = false;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.TransmissionDemandeCompleted += (s, args) =>
                {
                    try
                    {

                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors du traitement, il se peut que votre opération ait échoué", "Erreur");
                            return;
                        }
                        if (string.IsNullOrEmpty(args.Result))
                            Message.Show("Demande transmise avec succès", "Information");
                        else
                            Message.ShowError(args.Result, "Information");
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                //service.InsertAffectionScelleAsync(lademande.FirstOrDefault().FK_IDDEMANDE, UserConnecte.PK_ID, EtapeActuelle, UserConnecte.matricule, ListLotAffecter_Selectionner);
                service.TransmissionDemandeAsync(lademande.FirstOrDefault().NUMDEM,EtapeActuelle, UserConnecte.matricule);
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            transmettreDemande();
            //List<int> Listid = new List<int>();
            //Listid.Add(lademande.First().FK_IDDEMANDE);
            //EnvoyerDemandeEtapeSuivante(Listid);
            //this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void txtSite_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ActiveInfoAutreCentreForunisseur(false );
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ActiveInfoAutreCentreForunisseur(true);
        }

        private void Cbo_Site_Fournisseur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                if (this.Cbo_Site_Fournisseur.SelectedItem != null)
                {
                    var csSite = Cbo_Site_Fournisseur.SelectedItem as CsSite;
                    if (csSite != null)
                    {
                        //this.txtSite_Fournisseur.Text = csSite.CODESITE ?? string.Empty;
                        RemplirCentreDuSiteFournisseur(csSite.PK_ID, 0);

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void Btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            //CsDemande laDetailDemande = new CsDemande();


            //Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande, true);
        }
    }
}


