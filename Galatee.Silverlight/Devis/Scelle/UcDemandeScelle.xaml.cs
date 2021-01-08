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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeScelle : ChildWindow
    {
        private List<CsCentre> _listeDesCentreExistant = null;
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> CentreHabiliter = new List<ServiceAccueil.CsCentre>();
        List<CsActivite> lstAllActivite = new List<CsActivite>();
        List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur> lstAllUser = new List<Galatee.Silverlight.ServiceAccueil.CsUtilisateur>();

        public UcDemandeScelle()
        {
            InitializeComponent();

            ActiveInfoAutreCentreForunisseur(false);
            chk_IsMagazinGeneral.IsChecked = true;
            AfficherInfoDemandeur();
            RemplirListeDesTypeDemandeExistant();
        }
        public UcDemandeScelle(int idDemande)
        {
            InitializeComponent();

            ActiveInfoAutreCentreForunisseur(false);
            chk_IsMagazinGeneral.IsChecked = true;
            AfficherInfoDemandeur();
            RemplirListeDesTypeDemandeExistant();
        }
        private void ActiveInfoAutreCentreForunisseur(bool visible)
        {
            Visibility visibilite = visible == true ? Visibility.Visible : Visibility.Collapsed;
            lbl_CentreFournisseur.Visibility = visibilite;
            cboCentreFournisseur.Visibility = visibilite;
            txtSite_Fournisseur.Visibility = visibilite;
            Cbo_Site_Fournisseur.Visibility = visibilite;
            lbl_Site_Fournisseur.Visibility = visibilite;
        }

        private void AfficherInfoDemandeur()
        {
            //ChargerListDesCentreHabiliter();
            ChargerListDesSite();
            ChargerService();

            txtDemandeur.Text = UserConnecte.nomUtilisateur;
            txtDemandeur.Tag = UserConnecte.PK_ID;
        }
        List<CsDscelle> lademande = new List<CsDscelle>(); 
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
        private void RenseignerInformationDemande()
        {
            if (lademande != null && lademande.Count() > 0)
            {
                cboCouleur.SelectedValue = lademande.FirstOrDefault().FK_IDCOULEURSCELLE;
                Txt_NumDemande.Text = lademande.FirstOrDefault().NUMDEM;
                txtDemandeur.Text = lademande.FirstOrDefault().LIBELLEAGENT;
                txtSite.Text = lademande.FirstOrDefault().LIBELLESITEAGENT;
                //txtCentreDemandeur.Text = lademande.FirstOrDefault().LIBELLECENTREDESTINATAIRE;
                //txtCentreDemandeur.Tag = lademande.FirstOrDefault().FK_IDCENTRE;

                //txtCentreFourniseur.Text = lademande.FirstOrDefault().LIBELLECENTREFOURNISSEUR;

                chk_IsMagazinGeneral.IsChecked = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR == SessionObject.Enumere.IDGenerale ? true : false;
                cboCentreFournisseur.SelectedValue = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR;

                ServiceAccueil.CsCentre lCentreFournisseurDemande = SessionObject.LstCentre.FirstOrDefault(c => c.PK_ID == lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR);
                if (lCentreFournisseurDemande != null)
                {
                    //txtCentreFourniseur.Text = lCentreFournisseurDemande.LIBELLE;
                    txtSite_Fournisseur.Text = lCentreFournisseurDemande.LIBELLESITE;

                }
                var lstCentre_Demande = SessionObject.LstCentre.Where(c => c.PK_ID == lademande.FirstOrDefault().FK_IDCENTRE);
                var Centre_Demande = lstCentre_Demande != null ? lstCentre_Demande.FirstOrDefault() : null;

                txtNombre.Text = lademande.FirstOrDefault().NOMBRE_DEM.ToString();
                string NombreScelle = lademande.FirstOrDefault().NOMBRE_DEM != null ? lademande.FirstOrDefault().NOMBRE_DEM.ToString() : string.Empty;
                string Couleur = lademande.FirstOrDefault().LIBELLECOULEUR != null ? lademande.FirstOrDefault().LIBELLECOULEUR : string.Empty;

                int IdCentreRecuperationDeLot = lademande.FirstOrDefault().FK_IDCENTREFOURNISSEUR;
            }
        }

        private void ChargerService()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneListeActiviteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        lstAllActivite = args.Result;
                        cboServiceDemandeur.ItemsSource = args.Result;
                        cboServiceDemandeur.DisplayMemberPath = "Activite_Libelle";
                        cboServiceDemandeur.SelectedValuePath = "Activite_ID";

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.RetourneListeActiviteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }
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

        private void RemplirCentrePerimetre(List<CsCentre> lstCentre, List<CsSite> lstSite)
        {
            try
            {
                cboCentreDemandeur.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            cboCentreDemandeur.Items.Add(item);
                        }
                    cboCentreDemandeur.SelectedValuePath = "PK_ID";
                    cboCentreDemandeur.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null)
                        foreach (var item in lstSite)
                        {
                            Cbo_Site.Items.Add(item);
                            Cbo_Site_Fournisseur.Items.Add(item);
                        }
                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    Cbo_Site_Fournisseur.SelectedValuePath = "PK_ID";
                    Cbo_Site_Fournisseur.DisplayMemberPath = "LIBELLE";

                    if (lstSite != null && lstSite.Count == 1)
                    {
                        Cbo_Site.SelectedItem = lstSite.First();
                        Cbo_Site_Fournisseur.SelectedItem = lstSite.First();

                    }

                }
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
                cboCentreDemandeur.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                    if (lesCentreDuPerimetreAction != null)
                        foreach (var item in lesCentreDuPerimetreAction)
                        {
                            cboCentreDemandeur.Items.Add(item);
                        }
                    //cboCentreDemandeur.ItemsSource = lesCentreDuPerimetreAction;
                    cboCentreDemandeur.SelectedValuePath = "PK_ID";
                    cboCentreDemandeur.DisplayMemberPath = "LIBELLE";

                    if (pIdcentre != 0)
                        this.cboCentreDemandeur.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                    if (_listeDesCentreExistant.Count == 1)
                        this.cboCentreDemandeur.SelectedItem = _listeDesCentreExistant.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeDesTypeDemandeExistant()
        {
            try
            {
                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    return;

                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneOptionDemandeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstTypeDemande = res.Result;
                };
                service1.RetourneOptionDemandeAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

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
                        this.txtSite.Text = csSite.CODE ?? string.Empty;
                            RemplirCentreDuSite(csSite.PK_ID, 0);

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Txt_NumDemande.Text) && cboCentreDemandeur.SelectedValue != null && cboServiceDemandeur.SelectedValue != null && cboCouleur.SelectedValue != null && !string.IsNullOrEmpty(txtNombre.Text))
            {
                CsDscelle scelle = new CsDscelle()
                {
                    NUMDEM = Txt_NumDemande.Text,
                    FK_IDCENTRE = int.Parse(cboCentreDemandeur.SelectedValue.ToString()),
                    FK_IDACTIVITE = int.Parse(cboServiceDemandeur.SelectedValue.ToString()),
                    FK_IDCOULEURSCELLE = int.Parse(cboCouleur.SelectedValue.ToString()),
                    FK_IDAGENT = int.Parse(txtDemandeur.Tag.ToString()),
                    NOMBRE_DEM = int.Parse(txtNombre.Text),
                    FK_IDCENTREFOURNISSEUR =chk_IsMagazinGeneral.IsChecked==false? int.Parse(cboCentreFournisseur.SelectedValue.ToString()):SessionObject.Enumere.IDGenerale
                };
                Galatee.Silverlight.ServiceAccueil.CsTdem leTydemande = SessionObject.LstTypeDemande.FirstOrDefault(t=>t.CODE == SessionObject.Enumere.DemandeScelle);
                CsDemandeBase lademande = new CsDemandeBase()
                {
                    NUMDEM = Txt_NumDemande.Text,
                    TYPEDEMANDE = leTydemande.CODE ,
                    FK_IDTYPEDEMANDE = leTydemande.PK_ID ,
                    FK_IDADMUTILISATEUR = UserConnecte.PK_ID,
                    CENTRE = ((ServiceAccueil.CsCentre)cboCentreDemandeur.SelectedItem).CODE,
                    FK_IDCENTRE = ((ServiceAccueil.CsCentre)cboCentreDemandeur.SelectedItem).PK_ID,
                    USERCREATION = UserConnecte.matricule,
                    DATECREATION = DateTime.Now

                };

             AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.InsertDemandeScelleCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;

                        if (!string.IsNullOrWhiteSpace( args.Result))
                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " +args.Result ,
                              Silverlight.Resources.Devis.Languages.txtDevis);
                        else
                            Message.ShowError("Erreur d'enregistrement", Langue.lbl_Menu);
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.InsertDemandeScelleAsync(lademande, scelle);


                this.DialogResult = true;
            }


        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void cboServiceDemandeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboServiceDemandeur.SelectedValue != null)
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneListeCouleurScelleCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args != null && args.Cancelled)
                                return;
                            cboCouleur.ItemsSource = args.Result;
                            cboCouleur.DisplayMemberPath = "Couleur_libelle";
                            cboCouleur.SelectedValuePath = "Couleur_ID";
                            if (args.Result != null && args.Result.Count == 1)
                                cboCouleur.SelectedItem  = args.Result.First();
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, "Erreur");
                        }

                    };
                    service.RetourneListeCouleurScelleAsync(int.Parse(cboServiceDemandeur.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void cboCentreDemandeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cboCentreDemandeur.SelectedValue != null)
            {
                string numIncrementiel = ((ServiceAccueil.CsCentre)(cboCentreDemandeur.SelectedItem)).NUMDEM.ToString();
                this.Txt_NumDemande.Text = ((ServiceAccueil.CsCentre)(cboCentreDemandeur.SelectedItem)).CODE + numIncrementiel.PadLeft(10, '0');

            }
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
                        this.txtSite_Fournisseur.Text = csSite.CODE ?? string.Empty;
                        RemplirCentreDuSiteFournisseur(csSite.PK_ID, 0);

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
    }
}

