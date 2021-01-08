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
using Galatee.Silverlight.ServiceRecouvrement;
//using Galatee.Silverlight.ServicePrintings;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class FrmAutorisationPaiementFraisPoseSGC : ChildWindow
    {
        List<CsCentre> lstCentreCampagne = new List<CsCentre>();
        List<CsCAMPAGNE> lesCampagne = new List<CsCAMPAGNE>();
        ObservableCollection<CsDetailCampagne> lesClientCampagne = new ObservableCollection<CsDetailCampagne>();
        List<CsSite> lstSiteCampagne = new List<CsSite>();
        List<CsSite> lstAgentCampagne = new List<CsSite>();
        public FrmAutorisationPaiementFraisPoseSGC()
        {
            try
            {
                InitializeComponent();
                InitialiserControle();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
 
        private void InitialiserControle()
        {
            try
            {
                ChargerDonneeCentre();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        private void ChargerDonneeCentre()
        {
            try
            {
                List<int> lstIdCentreClient = new List<int>();
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (LstCentre != null)
                    {
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                            lstIdCentreClient.Add(item.PK_ID);
                    }
                    return;
                }
                //Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new AccesServiceWCF().GetAcceuilClient();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (LstCentre != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in _LstCentre)
                            lstIdCentreClient.Add(item.PK_ID);
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "LoadCentre");

            }
        }
        public event EventHandler Closed;

        string campaign = string.Empty;
        public bool IsGettingIdCoupure = false;
        public void ClosedEnventHandler()
        {
            if (this.Closed != null)
            {
                this.Closed(this, null);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
            //this.DialogResult = true;
        }
        private void InsererCompteClient(Galatee.Silverlight.ServiceAccueil.CsLclient leFrais)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient clients = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                clients.InsererFraisPoseAsync(leFrais);
                clients.InsererFraisPoseCompleted += (es, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "InsererCompteClient");
                            return;
                        }
                        if (result.Result == null)
                        {
                            Message.ShowInformation("Donnée non trouvé ", "InsererCompteClient");
                            return;
                        }
                        Message.ShowInformation(Galatee.Silverlight.Resources.Accueil.Langue.MsgOperationTerminee, Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Galatee.Silverlight.ServiceAccueil.CsLclient GetElementDeFrais(CsDetailCampagne Campagne)
        {
            Galatee.Silverlight.ServiceAccueil.CsLclient Frais = new Galatee.Silverlight.ServiceAccueil.CsLclient();
            try
            {
                Frais.CENTRE = Campagne.CENTRE;
                Frais.CLIENT = Campagne.CLIENT;
                Frais.ORDRE = Campagne.ORDRE;
                Frais.REFEM = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString("00");
                Frais.IDCOUPURE = Campagne.IDCOUPURE;
                Frais.COPER = SessionObject.Enumere.CoperFRP;
                Frais.DENR = DateTime.Today.Date;
                Frais.EXIGIBILITE = DateTime.Today.Date;
                Frais.DATECREATION = DateTime.Today.Date;
                Frais.DATEMODIFICATION = DateTime.Today.Date;
                Frais.DC = SessionObject.Enumere.Debit;
                Frais.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                Frais.MATRICULE = UserConnecte.matricule;
                Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString("00");
                Frais.MONTANT = Campagne.MONTANTFRAIS;
                Frais.TOP1 = "0";


                return Frais;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
     
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }


        void Recherche(CsCAMPAGNE laCampagneSelect, CsClient LeClientRechercheSelect)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                client.RetourneDonneeAnnulationFraisAsync(laCampagneSelect, LeClientRechercheSelect);
                client.RetourneDonneeAnnulationFraisCompleted += (ss, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            string error = args.Error.Message;
                            Message.ShowError("Erreur à l'exécution du service", "Recouvrement");
                            return;
                        }

                        if (args.Result == null || args.Result.Count == 0)
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "Recouvrement");
                            return;
                        }

                        List<CsDetailCampagne> detailcampagnes = new List<CsDetailCampagne>();
                        detailcampagnes = args.Result.Where(t=>t.ISNONENCAISSABLE != null ).ToList();
                        if (detailcampagnes.Count == 0 )
                        {
                            Message.ShowInformation("Aucune donnée trouvée", "Recouvrement");
                            return;
                        }
                        lesClientCampagne.Clear();
                        foreach (CsDetailCampagne item in detailcampagnes)
                            lesClientCampagne.Add(item);

                        this.lvwResultat.ItemsSource = null;
                        this.lvwResultat.ItemsSource = lesClientCampagne;

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        CsClient ClientRechercheSelect;
        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.txtCentreClient.Text) &&
                    !string.IsNullOrEmpty(this.txtReferenceClient.Text) &&
                    !string.IsNullOrEmpty(this.txtOrdeClient.Text))
                {
                    RetourneFraisPose(this.txtCentreClient.Text, this.txtReferenceClient.Text, this.txtOrdeClient.Text);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                    btnOk.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lvwResultat.ItemsSource != null)
                {
                    CsLclient lstClientSelect = new CsLclient();
                    lstClientSelect = lvwResultat.SelectedItem as CsLclient;
                    ValiderAutorisationFrais(lstClientSelect);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {

        }

        List<DataGridRow> Rows = new List<DataGridRow>();
        public static FrameworkElement SearchFrameworkElement(FrameworkElement parentFrameworkElement, string childFrameworkElementNameToSearch)
        {
            FrameworkElement childFrameworkElementFound = null;
            SearchFrameworkElement(parentFrameworkElement, ref childFrameworkElementFound, childFrameworkElementNameToSearch);
            return childFrameworkElementFound;
        }
        private static void SearchFrameworkElement(FrameworkElement parentFrameworkElement, ref FrameworkElement childFrameworkElementToFind, string childFrameworkElementName)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parentFrameworkElement);
            if (childrenCount > 0)
            {
                FrameworkElement childFrameworkElement = null;
                for (int i = 0; i < childrenCount; i++)
                {
                    childFrameworkElement = (FrameworkElement)VisualTreeHelper.GetChild(parentFrameworkElement, i);
                    if (childFrameworkElement != null && childFrameworkElement.Name.Equals(childFrameworkElementName))
                    {
                        childFrameworkElementToFind = childFrameworkElement;
                        return;
                    }
                    SearchFrameworkElement(childFrameworkElement, ref childFrameworkElementToFind, childFrameworkElementName);
                }
            }
        }
        private void ChangeSelectedItemColor()
        {
            try
            {
                //to get the current row binding value
                CsDetailCampagne currentRow = (CsDetailCampagne)lvwResultat.SelectedItem;

                //to read the currentRow
                DataGridRow selectedRow = Rows[lvwResultat.SelectedIndex];
                //color row
                var backgroundRectangle = SearchFrameworkElement(selectedRow, "BackgroundRectangle") as Rectangle;
                if (backgroundRectangle != null)
                {
                    backgroundRectangle.Fill = new SolidColorBrush(Colors.Cyan);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ValiderAutorisationFrais(CsLclient  Lst)
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.ValidateAutorisationCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }

                        if (ress.Result == false )
                        {
                            Message.ShowInformation("Erreur lors de l'insertion des index de campagne! Veuillez réessayer svp ", "Informations");
                            return;
                        }
                        if (ress.Result == true)
                        {
                            Message.ShowInformation("Mise à jour validée ", "Informations");
                            btnsearch_Click(null, null);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.ValidateAutorisationAsync(Lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnReinitialiser_Click(object sender, RoutedEventArgs e)
        {

        }
        private void RetourneFraisPose(string Centre,string Client,string Ordre )
        {
            try
            {
                RecouvrementServiceClient client = new RecouvrementServiceClient(Utility.ProtocoleIndex(), Utility.EndPoint("Recouvrement"));
                client.AutorisationDePaiementCompleted += (ss, ress) =>
                {
                    try
                    {
                        if (ress.Cancelled || ress.Error != null)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel service", "Informations");
                            return;
                        }

                        if (ress.Result != null && ress.Result.Count != 0)
                        {
                            lvwResultat.ItemsSource = null;
                            lvwResultat.ItemsSource = ress.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };
                client.AutorisationDePaiementAsync(Centre ,Client ,Ordre );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
    }
}

