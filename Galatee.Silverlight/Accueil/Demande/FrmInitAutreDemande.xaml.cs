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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmInitAutreDemande : ChildWindow
    {
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode { get; set; }
        private ServiceAccueil.CsDemandeBase laDemandeSelect = null;
        private ServiceAccueil.CsDemande laDetailDemande = new ServiceAccueil.CsDemande();
        public ObservableCollection<CsTypeDOCUMENTSCANNE> LstTypeDocument = new ObservableCollection<CsTypeDOCUMENTSCANNE>();
        public FrmInitAutreDemande()
        {
            InitializeComponent();
        }
        string LeTypeDemande = string.Empty;
        public FrmInitAutreDemande(string Tdem)
        {
            InitializeComponent();
            LeTypeDemande = Tdem;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ValiderInitialisation(laDetailDemande, false  );
        }

        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        private byte[] image;

        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                 
                     CsDemandeBase laDemande = new CsDemandeBase();

                    laDemande.DATECREATION = DateTime.Now;
                    laDemande.USERCREATION = UserConnecte.matricule;
                    laDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    laDemande.TYPEDEMANDE = LeTypeDemande;
                    laDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDemande.CLIENT  = laDetailDemande.Abonne.CLIENT ;
                    laDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDemande.TYPEDEMANDE = LeTypeDemande;
                    laDemande.DATECREATION = DateTime.Now;
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(LstPiece);
                #endregion
                laDetailDemande.LaDemande = laDemande;

                    laDetailDemande.LaDemande.ETAPEDEMANDE = (int)DataReferenceManager.EtapeDevis.Accueil;
                    if (IsTransmetre)
                        laDetailDemande.LaDemande.ETAPEDEMANDE = null;
                    laDetailDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (IsTransmetre)
                        {
                            string Retour = b.Result;
                            string[] coupe = Retour.Split('.');
                            Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], demandedevis.LaDemande.FK_IDCENTRE, coupe[1], demandedevis.LaDemande.FK_IDTYPEDEMANDE);
                        }
                    };
                    client.ValiderDemandeInitailisationAsync(laDetailDemande);
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur c'est produite a la validation ", "ValiderDemandeInitailisation");
            }
        }

        private CsDemande GetDemandeDevisFromScreen( bool isTransmettre)
        {
            try
            {
               
                    CsDemandeBase laDemande = new CsDemandeBase();

                    laDemande.DATECREATION = DateTime.Now;
                    laDemande.USERCREATION = UserConnecte.matricule;
                    laDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                    laDemande.TYPEDEMANDE = LeTypeDemande;
                    laDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDemande.CLIENT  = laDetailDemande.Abonne.CLIENT ;
                    laDemande.CENTRE = laDetailDemande.Abonne.CENTRE;
                    laDemande.TYPEDEMANDE = LeTypeDemande;
                    laDemande.DATECREATION = DateTime.Now;
                #region Doc Scanne
                if (laDetailDemande.ObjetScanne == null) laDetailDemande.ObjetScanne = new List<ObjDOCUMENTSCANNE>();
                laDetailDemande.ObjetScanne.AddRange(LstPiece);
                #endregion
                laDetailDemande.LaDemande = laDemande;

                return laDetailDemande;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

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
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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
        private void ChargerTypeDocument()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeDocumentCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;

                    foreach (var item in args.Result)
                    {
                        LstTypeDocument.Add(item);
                    }
                    cbo_typedoc.ItemsSource = LstTypeDocument;
                    cbo_typedoc.DisplayMemberPath = "LIBELLE";
                    cbo_typedoc.SelectedValuePath = "PK_ID";
                };
                service.ChargerTypeDocumentAsync();
                service.CloseAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    CsCentre centre = Cbo_Centre.SelectedItem as CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        RemplirProduitCentre(centre);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
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
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }
        private void RemplirProduitCentre(CsCentre pCentre)
        {
            try
            {
                Cbo_Produit.ItemsSource = null;
                Cbo_Produit.ItemsSource = pCentre.LESPRODUITSDUSITE;
                Cbo_Produit.SelectedValuePath = "PK_ID";
                Cbo_Produit.DisplayMemberPath = "LIBELLE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Cbo_Site.SelectedItem != null && Cbo_Centre.SelectedItem != null && Cbo_Produit.SelectedItem != null)
            {
                if (txt_Ref_Branchement.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    CsClient leClient = new CsClient();
                    leClient.FK_IDCENTRE = (int)this.txtCentre.Tag;
                    leClient.CENTRE = this.txtCentre.Text;
                    leClient.REFCLIENT = this.txt_Ref_Branchement.Text;
                    leClient.PRODUIT = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).CODE;
                    leClient.TYPEDEMANDE = LeTypeDemande;

                    RetourneOrdre(leClient);
                }
            }
            else
            {
                Message.Show("Veuillez vous assurer que le site le centre et le produit soit selectionné", "Infomation");
            }
        }
        private void RetourneOrdre(ServiceAccueil.CsClient leClient)
        {
            try
            {
                string OrdreMax = string.Empty;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneOrdreMaxCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    OrdreMax = args.Result;
                    if (OrdreMax != null)
                    {
                        leClient.ORDRE = OrdreMax;
                        ChargeDetailDEvis(leClient);
                    }
                };
                service.RetourneOrdreMaxAsync(leClient.FK_IDCENTRE.Value, leClient.CENTRE, leClient.REFCLIENT, leClient.PRODUIT);
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargeDetailDEvis(ServiceAccueil.CsClient leclient)
        {

            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {

                        laDetailDemande = new ServiceAccueil . CsDemande();
                        laDetailDemande = args.Result;
                        RenseignerInformationsAbonnement(laDetailDemande.Abonne );
                        RemplireOngletClient(laDetailDemande.LeClient);
                      }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RenseignerInformationsAbonnement(CsAbon  leAbon)
        {
            try
            {
                if (leAbon != null && leAbon != null )
                {
                    this.Txt_CodePuissanceUtilise.Text = laDetailDemande.Branchement.PUISSANCEINSTALLEE.ToString();
                    this.Txt_CodeTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? laDetailDemande.Abonne.TYPETARIF : string.Empty;
                    this.Txt_CodePussanceSoucrite.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.PUISSANCE.Value.ToString()) ? laDetailDemande.Abonne.PUISSANCE.Value.ToString() : string.Empty;

                    if (laDetailDemande.Abonne.PUISSANCE != null)
                        this.Txt_CodePussanceSoucrite.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCE.ToString()).ToString("N2");
                    if (laDetailDemande.Abonne.PUISSANCEUTILISEE != null)
                        this.Txt_CodePuissanceUtilise.Text = Convert.ToDecimal(laDetailDemande.Abonne.PUISSANCEUTILISEE.Value).ToString("N2");
                    this.Txt_CodeRistoune.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.RISTOURNE.ToString()) ? string.Empty : Convert.ToDecimal(laDetailDemande.Abonne.RISTOURNE.Value).ToString("N2");

                    this.Txt_CodeForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.FORFAIT) ? string.Empty : laDetailDemande.Abonne.FORFAIT;
                    this.Txt_LibelleForfait.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFORFAIT) ? string.Empty : laDetailDemande.Abonne.LIBELLEFORFAIT;

                    this.Txt_CodeTarif.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.TYPETARIF) ? string.Empty : laDetailDemande.Abonne.TYPETARIF;
                    this.Txt_LibelleTarif.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLETARIF) ? laDetailDemande.Abonne.LIBELLETARIF : string.Empty;

                    this.Txt_CodeFrequence.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.PERFAC) ? string.Empty : laDetailDemande.Abonne.PERFAC;
                    this.Txt_LibelleFrequence.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEFREQUENCE) ? laDetailDemande.Abonne.LIBELLEFREQUENCE : string.Empty;

                    this.Txt_CodeMoisIndex.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISREL) ? string.Empty : laDetailDemande.Abonne.MOISREL;
                    this.Txt_LibelleMoisIndex.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISIND) ? laDetailDemande.Abonne.LIBELLEMOISIND : string.Empty;

                    this.Txt_CodeMoisFacturation.Text = string.IsNullOrEmpty(laDetailDemande.Abonne.MOISFAC) ? string.Empty : laDetailDemande.Abonne.MOISFAC;
                    this.Txt_LibMoisFact.Text = !string.IsNullOrEmpty(laDetailDemande.Abonne.LIBELLEMOISFACT) ? laDetailDemande.Abonne.LIBELLEMOISFACT : string.Empty;
                    this.Txt_DateAbonnement.Text = (laDetailDemande.Abonne.DABONNEMENT == null) ? DateTime.Now.ToShortDateString() : Convert.ToDateTime(laDetailDemande.Abonne.DABONNEMENT.Value).ToShortDateString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplireOngletClient(CsClient _LeClient)
        {
            try
            {
                if (_LeClient != null)
                {
                    this.txt_NomClient.Text = (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty :  _LeClient.NOMABON);
                    this.txt_Telephone1.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                    this.txt_Adresse1.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                    this.txt_NumNina.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
                    this.tab12_Txt_LibelleCodeConso.Text = string.IsNullOrEmpty(_LeClient.LIBELLECODECONSO) ? string.Empty : _LeClient.LIBELLECODECONSO;
                    this.tab12_Txt_LibelleCategorie.Text = string.IsNullOrEmpty(_LeClient.LIBELLECATEGORIE) ? string.Empty : _LeClient.LIBELLECATEGORIE;
                    this.tab12_Txt_LibelleEtatClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLERELANCE) ? string.Empty : _LeClient.LIBELLERELANCE;
                    this.tab12_Txt_LibelleTypeClient.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATURECLIENT) ? string.Empty : _LeClient.LIBELLENATURECLIENT;
                    this.tab12_Txt_Nationnalite.Text = string.IsNullOrEmpty(_LeClient.LIBELLENATIONALITE) ? string.Empty : _LeClient.LIBELLENATIONALITE;
                    this.tab12_Txt_Datecreate.Text = string.IsNullOrEmpty(_LeClient.DATECREATION.ToString()) ? string.Empty : Convert.ToDateTime(_LeClient.DATECREATION).ToShortDateString();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Btn_Transmettre_Click(object sender, RoutedEventArgs e)
        {
            ValiderInitialisation(laDetailDemande, true);

        }

        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

