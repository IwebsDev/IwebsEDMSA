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
using Galatee.Silverlight.ServiceFacturation ;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Resources.Index ;
using System.ComponentModel;


namespace Galatee.Silverlight.Facturation
{
    public partial class FrmIndexCreationDeLot : ChildWindow,INotifyPropertyChanged
    {
        List<ServiceAccueil.CsProduit> LstProduitSelect = new List<ServiceAccueil.CsProduit>();
        List<ServiceAccueil.CsCentre> LstCentreSelect = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsCategorieClient> LstCategorieSelect = new List<ServiceAccueil.CsCategorieClient>();
        List<ServiceAccueil.CsFrequence> LstFrequenceSelect = new List<ServiceAccueil.CsFrequence>();
        List<ServiceAccueil.CsTournee> LstZone = new List<ServiceAccueil.CsTournee>();
        List<ServiceAccueil.CsTournee> LstZoneCentre = new List<ServiceAccueil.CsTournee>();

        ObservableCollection<ServiceAccueil.CsCentre> LstCentreObs = new ObservableCollection<ServiceAccueil.CsCentre>();
        ObservableCollection<ServiceAccueil.CsSite> LstSiteObs = new ObservableCollection<ServiceAccueil.CsSite>();
        ObservableCollection<ServiceAccueil.CsProduit> LsteProduitObs = new ObservableCollection<ServiceAccueil.CsProduit>();
        ObservableCollection<ServiceAccueil.CsCategorieClient> LstCategorieObs;
        ObservableCollection<ServiceAccueil.CsFrequence> LstFrequenceObs;
        ObservableCollection<ServiceAccueil.CsTournee> LstTourneeObs = new ObservableCollection<ServiceAccueil.CsTournee>();

        bool isAllClick = false;
        bool isUnChecking = false;
        bool isNothingClick = false;
        List<int> LstCentrePagerieLot = new List<int>();
        public FrmIndexCreationDeLot()
        {
            try
            {
                InitializeComponent();
                ChargerTournee();
                this.Txt_batch.MaxLength = SessionObject.Enumere.TailleNumeroBatch;
                this.Txt_Period.MaxLength = 7;
                this.OKButton.IsEnabled = false;
                prgBar.Visibility = Visibility.Collapsed;
                LstTourneeObs.Clear();
                translate();
                this.dtg_zone.ItemsSource = null;
                this.dtg_Categorie.ItemsSource = null;
            }
            catch (Exception ex)
            {

                Message.ShowInformation(ex.Message, "Erreur");
            }
        }
        public void translate()
        {
            try
            {
                this.btn_rienCategorie.Content = this.btn_rienCentre.Content = this.btn_rienFrenquence.Content = this.btn_rienProduit.Content = this.btn_rienReleveur.Content = this.btn_RienTourne.Content = Langue.btn_Rien;
                this.btn_ToutCategorie.Content = this.btn_ToutCentre.Content = this.btn_ToutFrequence.Content = this.btn_ToutProduit.Content = this.btn_ToutReleveur.Content = this.btn_ToutTournee.Content = Langue.btn_Tout;

                //this.lbl_batch.Content = Langue.btn_batch;
                this.lbl_De.Content = Langue.ConnecteurDe;
                this.lbl_A.Content = Langue.Connecteur;
                this.lbl_periode.Content = Langue.lbl_Periode;
                this.gbo_ListLot.Header  = Langue.gbo_ListLot;
                this.gbo_Specification.Header = Langue.gbo_Specification;
                this.gboEdition.Header = Langue.gboEdition;
                this.dtg_Centre.Columns[1].Header = Langue.lbl_Centre;
                this.dtg_produit.Columns[1].Header = Langue.lbl_produit;
                this.dtg_Categorie.Columns[1].Header = Langue.lbl_Categorie;
                this.dtg_frequence.Columns[1].Header = Langue.lbl_frequence;
                this.dtg_zone.Columns[1].Header = Langue.lbl_tournee;
                this.dtg_zone.Columns[2].Header = Langue.lbl_Centre;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        bool VerifierSaisiDonnee()
        {
            bool returnValue = true ;
            try
            {
                List<ServiceAccueil.CsCentre> lesCentreSelect = new List<ServiceAccueil.CsCentre>();
                if (dtg_Centre.ItemsSource != null )
                lesCentreSelect = ((ObservableCollection<ServiceAccueil.CsCentre>)dtg_Centre.ItemsSource).Where(t => t.IsSelect == true).ToList();

                List<ServiceAccueil.CsProduit> lesProdSelect = new List<ServiceAccueil.CsProduit>();
                if (dtg_produit.ItemsSource != null )
                lesProdSelect = ((ObservableCollection<ServiceAccueil.CsProduit>)dtg_produit.ItemsSource).Where(t => t.IsSelect == true).ToList();

                List<ServiceAccueil.CsCategorieClient> lesCategSelect = new List<ServiceAccueil.CsCategorieClient>();
                if ( dtg_Categorie.ItemsSource != null )
                 lesCategSelect = ((ObservableCollection<ServiceAccueil.CsCategorieClient>)dtg_Categorie.ItemsSource).Where(t => t.IsSelect == true).ToList();


                List<ServiceAccueil.CsFrequence> lesFrequenceSelect = new List<ServiceAccueil.CsFrequence>();
                if (dtg_frequence.ItemsSource != null )
                lesFrequenceSelect = ((ObservableCollection<ServiceAccueil.CsFrequence>)dtg_frequence.ItemsSource).Where(t => t.IsSelect == true).ToList();

                List<ServiceAccueil.CsTournee> lstTourneSelect = new List<ServiceAccueil.CsTournee>();
                if (dtg_frequence.ItemsSource != null)
                lstTourneSelect = ((ObservableCollection<ServiceAccueil.CsTournee>)dtg_zone.ItemsSource).Where(t => t.IsSelect == true).ToList();

                if (lesCentreSelect.Count == 0)
                {
                    Message.ShowInformation("Centre non sélectionné", "Error");
                    this.OKButton.IsEnabled = true;
                    returnValue = false;
                }
                else if (lesProdSelect.Count == 0)
                {
                    Message.ShowInformation("Produit non sélectionné", "Error");
                    this.OKButton.IsEnabled = true;
                    returnValue = false;

                }
                else if (lesFrequenceSelect.Count == 0)
                {
                    Message.ShowInformation("Fréquence non sélectionnée", "Error");
                    this.OKButton.IsEnabled = true;
                    returnValue = false;

                }
                else if (lesCategSelect.Count == 0)
                {
                    Message.ShowInformation("Categorie non sélectionnée", "Error");
                    this.OKButton.IsEnabled = true;
                    returnValue = false;

                }
                else if (!ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Period.Text))
                {
                    Message.ShowInformation("Période saisie incorrecte", "Error");
                    this.OKButton.IsEnabled = true;
                    returnValue = false;

                }
                return returnValue;
            }
            catch (Exception ex )
            {
                this.OKButton.IsEnabled = true;
                throw ex;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_Period.Text))
                {
                    if (this.Txt_Period.Text.Length == 7)
                    {
                        if (!string.IsNullOrEmpty(this.Txt_batch.Text)) this.OKButton.IsEnabled = true;
                        if (!ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Period.Text))
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Index.Langue.libelleModule, Galatee.Silverlight.Resources.Index.Langue.MsgFormatInvalide, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_Period.Focus();
                                return;
                            };
                            w.Show();
                        }
                    }
                    else
                    {
                        Message.ShowInformation("Le format de la période n'est pas correct", "Facturation");
                        this.Txt_Period.Focus();
                        return;
                    }
                }
                List<int> lstIdCentre = new List<int>();
                if (ClasseMEthodeGenerique.IsDateValide(this.Txt_Period.Text))
                {
                  foreach (ServiceAccueil.CsCentre  item in lesCentre)
	                  lstIdCentre.Add(item.PK_ID );
                  VerifieSiBacthExist(this.Txt_batch.Text, lstIdCentre);
                }
                else
                {
                    OKButton.IsEnabled = true;
                    Message.ShowError("Le format de la période n'est pas valide. Veuillez réessayer svp!", "Information");
                }
            }
            catch (Exception ex)
            {
                this.OKButton.IsEnabled = false;
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "=>OKButton_Click");
            }
        }
        void DialogClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerListDesSite();
                ChargerDonneeReference ();
            }
            catch (Exception ex)
            {
                var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Index.Langue.libelleModule , ex.Message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                w.OnMessageBoxClosed += (_, result) =>
                {
                };
                w.Show();
            }
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        List<int> lesidCentre = new List<int>();
        void ChargerListDesSite()
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
                            LstCentre= args.Result;
                            SessionObject.LstCentre = LstCentre;
                            if (LstCentre.Count != 0)
                            {
                                LsteProduitObs = new ObservableCollection<ServiceAccueil.CsProduit>();
                                lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(LstCentre, UserConnecte.listeProfilUser);
                                lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);

                                foreach (ServiceAccueil.CsCentre items in lesCentre)
                                {
                                    lesidCentre.Add(items.PK_ID);
                                }

                                foreach (ServiceAccueil.CsSite item in lesSite)
                                    LstSiteObs.Add(item);

                                dtg_Site.ItemsSource = null;
                                dtg_Site.ItemsSource = LstSiteObs;
                                if (LstSiteObs.Count == 1)
                                {
                                    dtg_Site.SelectedIndex = 0;
                                    this.dtg_Site.IsReadOnly = true;
                                }


                              
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
                    service.ListeDesDonneesDesSiteAsync(false );
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
        }
        private void ChargerFrequence()
        {
            try
            {
                if (SessionObject.LstFrequence != null && SessionObject.LstFrequence.Count != 0)
                {
                    SessionObject.LstFrequence.ForEach(t => t.IsSelect = false);
                    LstFrequenceObs = new ObservableCollection<ServiceAccueil.CsFrequence>();
                    foreach (ServiceAccueil.CsFrequence item in SessionObject.LstFrequence)
                        LstFrequenceObs.Add(item);

                    dtg_frequence.ItemsSource = null;
                    dtg_frequence.ItemsSource = LstFrequenceObs;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTousFrequenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFrequence = args.Result;
                    LstFrequenceObs = new ObservableCollection<ServiceAccueil.CsFrequence>();
                    foreach (ServiceAccueil.CsFrequence item in SessionObject.LstFrequence)
                        LstFrequenceObs.Add(item);

                    dtg_frequence.ItemsSource = null;
                    dtg_frequence.ItemsSource = LstFrequenceObs;
                };
                service.ChargerTousFrequenceAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie != null && SessionObject.LstCategorie.Count != 0)
                {
                    SessionObject.LstCategorie.ForEach(y => y.IsSelect = false);
                    LstCategorieObs = new ObservableCollection<ServiceAccueil.CsCategorieClient>();
                    foreach (ServiceAccueil.CsCategorieClient item in SessionObject.LstCategorie)
                        LstCategorieObs.Add(item);
                        dtg_Categorie  .ItemsSource = null;
                        dtg_Categorie.ItemsSource = LstCategorieObs;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;

                    if (SessionObject.LstCategorie != null && SessionObject.LstCategorie.Count != 0)
                    {
                        LstCategorieObs = new ObservableCollection<ServiceAccueil.CsCategorieClient>();
                        foreach (ServiceAccueil.CsCategorieClient item in SessionObject.LstCategorie)
                            LstCategorieObs.Add(item);
                        dtg_Categorie.ItemsSource = null;
                        dtg_Categorie.ItemsSource = LstCategorieObs;
                    }
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ChargerDonneeReference()
        {
            ChargerCategorie(); 
            ChargerFrequence();
        }
        private void ConstruireLot(List<CsCentre> LstCentre, List<CsProduit > LstProduit, List<CsCategorieClient > LstCategorie, List<CsFrequence > LstPeriodicite, List<CsTournee > LstTournee, string Lotri, string periode, string Matricule)
        {
            try
            {
                bool _IsAvecEdition = false;
                if (Chk_AvecEdition.IsChecked == true)
                    ConstruireLotriAvecEdition(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, this.Txt_batch.Text, periode, UserConnecte.matricule, _IsAvecEdition);
                else
                    ConstruireLotri(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, this.Txt_batch.Text, periode, UserConnecte.matricule, _IsAvecEdition);

            }
            catch (Exception ex)
            {
                OKButton.IsEnabled = true;
              throw ex;
            }
        }
        private void SupprimeEvenement()
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.EffacerEvtLotriCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                };
                service.EffacerEvtLotriAsync(UserConnecte.matricule );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ConstruireLotriAvecEdition(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule, bool IsAvecEdition)
        {
            prgBar.Visibility = Visibility.Visible;
            try
            {
                int? _NombreDeClient = 0;
                string key = Utility.getKey();

                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Facturation"));
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ConstruireLotAvecEditionCompleted += (s, args) =>
                {
                    try
                    {
                        OKButton.IsEnabled = true;

                        if (args.Error != null && args.Cancelled)
                        {
                            prgBar.Visibility = Visibility.Collapsed;
                            Message.ShowError("Erreur d'invocation du service.", "Erreur + => ConstruireLotri");
                            return;
                        }

                        if (args.Result == null)
                        {
                            prgBar.Visibility = Visibility.Collapsed;
                            Message.ShowError("Erreur survenue lors de la création du Lot . Veuillez réesayer svp!", "Erreur + => ConstruireLotri");
                            return;
                        }
                        List<CsEvenement> lesEvenement = args.Result;
                        prgBar.Visibility = Visibility.Collapsed;

                        if (lesEvenement != null && lesEvenement.Count != 0)
                            _NombreDeClient = lesEvenement.Count;
                        else
                            _NombreDeClient = 0;

                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch + "  " + _NombreDeClient.ToString() + "  " + Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch1 +
                                                                "\n\r" + "      " + Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch11, Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                       
                            if (LstProduit.First().CODE == SessionObject.Enumere.ElectriciteMT)
                                Utility.ActionDirectOrientation<ServicePrintings.CsEvenement , ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "IndexMt", "Index", true);
                            else
                                Utility.ActionDirectOrientation<ServicePrintings.CsEvenement, ServiceFacturation.CsEvenement>(args.Result, null, SessionObject.CheminImpression, "Index", "Index", true);
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        prgBar.Visibility = Visibility.Collapsed;
                        Message.ShowError(ex.InnerException.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                        OKButton.IsEnabled = true;
                    }
                };
                service.ConstruireLotAvecEditionAsync(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periode, Matricule);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = Visibility.Collapsed;
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ConstruireLotri(List<CsCentre> LstCentre, List<CsProduit> LstProduit, List<CsCategorieClient> LstCategorie, List<CsFrequence> LstPeriodicite, List<CsTournee> LstTournee, string Lotri, string periode, string Matricule, bool IsAvecEdition)
        {
            prgBar.Visibility = Visibility.Visible ;
            try
            {
                int? _NombreDeClient = 0;
                string key = Utility.getKey();

                Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Facturation"));


                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ConstruireLotCompleted += (s, args) =>
                {
                    try
                    {
                        OKButton.IsEnabled = true;

                        if (args.Error != null && args.Cancelled)
                        {
                            prgBar.Visibility = Visibility.Collapsed ;
                            Message.ShowError("Erreur d'invocation du service.", "Erreur + => ConstruireLotri");
                            return;
                        }

                        if (args.Result == null)
                        {
                            prgBar.Visibility = Visibility.Collapsed;
                            Message.ShowError("Erreur survenue lors de la création du Lot . Veuillez réesayer svp!", "Erreur + => ConstruireLotri");
                            return;
                        }
                        _NombreDeClient = args.Result;
                        prgBar.Visibility = Visibility.Collapsed;

                        Message.ShowInformation( Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch + "  " + _NombreDeClient.ToString() + "  " + Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch1 +
                                                                "\n\r" + "      " + Galatee.Silverlight.Resources.Index.Langue.MsgCreationBatch11,Galatee.Silverlight.Resources.Index.Langue.libelleModule);
                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        prgBar.Visibility = Visibility.Collapsed;
                        Message.ShowError(ex.InnerException.Message , Galatee.Silverlight.Resources.Langue.errorTitle);
                        OKButton.IsEnabled = true;
                    }
                };
                service.ConstruireLotAsync(LstCentre, LstProduit, LstCategorie, LstPeriodicite, LstTournee, Lotri, periode, Matricule);
            }
            catch (Exception ex)
            {
                prgBar.Visibility = Visibility.Collapsed;
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ReinitialiseControle()
        {
            if (dtg_Centre.ItemsSource != null)
            {
                var lesCentre= ((ObservableCollection<ServiceAccueil.CsCentre >)dtg_Centre.ItemsSource).ToList();
                lesCentre.ForEach(t => t.IsSelect = false);
            }

            if (dtg_produit.ItemsSource != null)
            {
                dtg_produit.ItemsSource = null;
                LsteProduitObs.Clear();
            }

            if (dtg_Categorie.ItemsSource != null)
            {
                var lesCateg = ((ObservableCollection<ServiceAccueil.CsCategorieClient >)dtg_Categorie.ItemsSource).ToList();
                lesCateg.ForEach(t => t.IsSelect = false);
            }
            if (dtg_frequence.ItemsSource != null)
            {
                var lesFrequence = ((ObservableCollection<ServiceAccueil.CsFrequence >)dtg_frequence.ItemsSource).ToList();
                lesFrequence.ForEach(t => t.IsSelect = false);
            }
            if (dtg_zone.ItemsSource != null)
            {
                dtg_zone.ItemsSource = null;
                LstZone.ForEach(t => t.IsSelect = false);
            }
            this.Txt_Period.Text = string.Empty;
            this.Txt_batch.Text = string.Empty;
            this.OKButton.IsEnabled = false;
        }
        private void ChargerTournee()
        {
            if (SessionObject.LstZone.Count != 0)
            {
                LstZone = SessionObject.LstZone;
                 SessionObject.LstZone.ForEach(t=>t.IsSelect= false );
                LstZone.ForEach(t => t.IsSelect = false);
                return;
            }
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.ChargerLesTourneesCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                SessionObject.LstZone = args.Result;

                LstZone = SessionObject.LstZone;
                LstZone.ForEach(t => t.IsSelect = false);

            };
            service.ChargerLesTourneesAsync(lesidCentre);
            service.CloseAsync();
        }

        private void VerifieSiBacthExist(string _Numbatch,List<int> lstidCentreSite)
        {
            try
            {
                this.OKButton.IsEnabled = false;

                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.IsBatchExistDansPagerieCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            OKButton.IsEnabled = true;
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }

                      List<CsEvenement >  LstEvtLot = args.Result;
                      if (LstEvtLot.FirstOrDefault(t => lstidCentreSite.Contains(t.FK_IDCENTRE)) != null)
                      {
                          OKButton.IsEnabled = true;
                          Message.ShowError(Langue.MsgLotExiste, "Erreur");
                          return;
                      }
                      else
                      {
                          if (VerifierSaisiDonnee())
                          {
                              List<ServiceFacturation.CsCentre> lstCentreSelect = new List<ServiceFacturation.CsCentre>();
                              var lesCentreSelect = ((ObservableCollection<ServiceAccueil.CsCentre>)dtg_Centre.ItemsSource).Where(t => t.IsSelect == true).ToList();
                              foreach (var item in lesCentreSelect)
                              {
                                  lstCentreSelect.Add(new CsCentre()
                                  {
                                      CODE = item.CODE,
                                      CODESITE = item.CODESITE,
                                      PK_ID = item.PK_ID,
                                      FK_IDCODESITE = item.FK_IDCODESITE
                                  });
                              }
                              List<ServiceFacturation.CsProduit> lstProduitSelect = new List<ServiceFacturation.CsProduit>();
                              var lesProdSelect = ((ObservableCollection<ServiceAccueil.CsProduit>)dtg_produit.ItemsSource).Where(t => t.IsSelect == true).ToList();
                              foreach (var item in lesProdSelect)
                              {
                                  lstProduitSelect.Add(new CsProduit()
                                  {
                                      CODE = item.CODE,
                                      PK_ID = item.PK_ID,
                                      LIBELLE = item.LIBELLE,
                                      FK_IDCENTRE  = item.FK_IDCENTRE 
                                  });
                              }
                              List<ServiceFacturation.CsCategorieClient> lstCategSelect = new List<ServiceFacturation.CsCategorieClient>();
                              var lesCategSelect = ((ObservableCollection<ServiceAccueil.CsCategorieClient>)dtg_Categorie.ItemsSource).Where(t => t.IsSelect == true).ToList();
                              foreach (var item in lesCategSelect)
                              {
                                  lstCategSelect.Add(new CsCategorieClient()
                                  {
                                      CODE = item.CODE,
                                      PK_ID = item.PK_ID,
                                      LIBELLE = item.LIBELLE,
                                  });
                              }
                              List<ServiceFacturation.CsFrequence> lstFrequenceSelect = new List<ServiceFacturation.CsFrequence>();
                              var lesFrequenceSelect = ((ObservableCollection<ServiceAccueil.CsFrequence>)dtg_frequence.ItemsSource).Where(t => t.IsSelect == true).ToList();
                              foreach (var item in lesFrequenceSelect)
                              {
                                  lstFrequenceSelect.Add(new CsFrequence()
                                  {
                                      CODE = item.CODE,
                                      PK_ID = item.PK_ID,
                                      LIBELLE = item.LIBELLE,
                                  });
                              }
                              List<ServiceFacturation.CsTournee> lstTourneSelect = new List<ServiceFacturation.CsTournee>();
                              var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsTournee>)dtg_zone .ItemsSource).Where(t=>t.IsSelect==true).ToList();
                              foreach (var item in lesFactureSelect)
                              {
                                  lstTourneSelect.Add(new CsTournee() {
                                   CODE = item.CODE,
                                   PK_ID = item.PK_ID,
                                   CENTRE = item.CENTRE ,
                                   FK_IDCENTRE = item.FK_IDCENTRE ,
                                   FK_IDRELEVEUR  = item.FK_IDRELEVEUR 
                                  });
                              }
                              ConstruireLot(lstCentreSelect, lstProduitSelect, lstCategSelect, lstFrequenceSelect, lstTourneSelect, this.Txt_batch.Text, ClasseMEthodeGenerique.ReformatePeriode(this.Txt_Period.Text), UserConnecte.matricule);
                          }
                      }


                    }
                    catch (Exception ex)
                    {
                        OKButton.IsEnabled = true;
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.IsBatchExistDansPagerieAsync(_Numbatch);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

       private void Chk_Tournee_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               if (Chk_Tournee.IsChecked == true)
               {
                   LstZoneCentre = null;
                   foreach (ServiceAccueil.CsCentre _lesite in LstCentreSelect)
                   {
                       LstZoneCentre = LstZone.Where(p => p.FK_IDCENTRE == _lesite.PK_ID).OrderBy(t => t.CENTRE).ThenBy(s => s.CODE).ToList();
                       foreach (ServiceAccueil.CsTournee item in LstZoneCentre)
                           LstTourneeObs.Add(item);
                       dtg_zone.ItemsSource = null;
                       dtg_zone.ItemsSource = LstTourneeObs;
                   }
               }
               else
               {
                   dtg_zone.ItemsSource = null;
               }
           }
           catch (Exception ex)
           {
             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
           }
       }

       private void Txt_batch_TextChanged(object sender, TextChangedEventArgs e)
       {
           try
           {
               if (this.Txt_batch.Text.Length  == SessionObject.Enumere.TailleNumeroBatch)
                   if (!string.IsNullOrEmpty(this.Txt_Period .Text)) this.OKButton.IsEnabled = true;
           }
           catch (Exception ex)
           {
             Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
           }
       }

       private void Txt_Period_TextChanged(object sender, TextChangedEventArgs e)
       {
           if (this.Txt_Period.Text.Length == 7)
           {
               if (!string.IsNullOrEmpty(this.Txt_batch.Text)) this.OKButton.IsEnabled = true;
               if (! ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Period.Text))
               {
                   var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Index.Langue.libelleModule, Galatee.Silverlight.Resources.Index.Langue.MsgFormatInvalide, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                   w.OnMessageBoxClosed += (_, result) =>
                   {
                       this.Txt_Period.Focus();
                   };
                   w.Show();
               }
           }

       }
       private void btn_ToutSite_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               foreach (ServiceAccueil.CsSite  item in LstSiteObs)
                   item.IsSelect = true;

           }
           catch (Exception ex)
           {
               Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
           }
       }

       private void btn_rienSite_Click(object sender, RoutedEventArgs e)
       {
           foreach (ServiceAccueil.CsSite item in LstSiteObs)
           {
               if (item.IsSelect == true)
                   item.IsSelect = false;
           }
       }
        private void btn_ToutCentre_Click(object sender, RoutedEventArgs e)
        {
            string leCentre = "";
            try
            {
                List<ServiceAccueil.CsTournee> lesTourneeSelect = new List<ServiceAccueil.CsTournee>();
                foreach (ServiceAccueil.CsCentre item in LstCentreObs)
                {
                    leCentre = item.CODE;
                    item.IsSelect = true;
                    if (LstZone.Where(t => t.FK_IDCENTRE == item.PK_ID) != null && 
                        LstZone.Where(t => t.FK_IDCENTRE == item.PK_ID).ToList().Count != 0)
                    lesTourneeSelect.AddRange( LstZone.Where(t => t.FK_IDCENTRE == item.PK_ID).ToList());

                }
                LsteProduitObs.Clear();
                foreach (ServiceAccueil.CsProduit item in LstCentreObs.First().LESPRODUITSDUSITE)
                    LsteProduitObs.Add(item);

                dtg_produit.ItemsSource = null;
                dtg_produit.ItemsSource = LsteProduitObs;

                LstTourneeObs.Clear();
                foreach (ServiceAccueil.CsTournee item in lesTourneeSelect)
                    LstTourneeObs.Add(item);
                dtg_zone.ItemsSource = null;
                dtg_zone.ItemsSource = LstTourneeObs;
            }
            catch (Exception ex)
            {
                string c = leCentre;
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    
        private void btn_rienCentre_Click(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsCentre item in LstCentreObs)
            {
                if (item.IsSelect == true)
                    item.IsSelect = false;
            }
            LstTourneeObs.Clear();
            dtg_zone.ItemsSource = null;
            dtg_zone.ItemsSource = LstTourneeObs;
        }

        private void btn_ToutProduit_Click(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsProduit item in LsteProduitObs)
            {
                if (item.IsSelect == false)
                    item.IsSelect = true;
            }
        }

        private void btn_rienProduit_Click(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsProduit item in LsteProduitObs)
            {
                if (item.IsSelect == true  )
                    item.IsSelect = false ;
            }
        }

        private void btn_ToutCategorie_Click(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsCategorieClient item in LstCategorieObs)
            {
                if (item.IsSelect == false)
                    item.IsSelect = true;
            }
        }

        private void btn_rienCategorie_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsCategorieClient item in LstCategorieObs)
            {
                if (item.IsSelect == true )
                    item.IsSelect = false ;
            }
        }

        private void btn_ToutFrequence_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsFrequence   item in LstFrequenceObs )
            {
                if (item.IsSelect == false )
                    item.IsSelect = true;
            }
        }

        private void btn_rienFrenquence_Click(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsFrequence item in LstFrequenceObs)
            {
                if (item.IsSelect == true)
                    item.IsSelect = false;
            }
        }

        private void dtg_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtg_Site.SelectedItem != null)
            {
                ServiceAccueil.CsSite leSite = LstSiteObs.FirstOrDefault(t => t.PK_ID == ((ServiceAccueil.CsSite)dtg_Site.SelectedItem).PK_ID);
                if (leSite != null)
                {
                    if (leSite.IsSelect)
                    { 
                        leSite.IsSelect = false;
                        foreach (var item in LstCentreObs.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList())
                           item.IsSelect = false;
                    }
                    else
                    {
                        leSite.IsSelect = true;
                        if (lesCentre != null && lesCentre.Count != 0)
                        {
                            foreach (var items in LstSiteObs.Where(t => t.PK_ID != leSite.PK_ID).ToList())
                                items.IsSelect = false;

                            LstCentreObs.Clear();
                            foreach (var item in lesCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList())
                                LstCentreObs.Add(item);


                            dtg_Centre.ItemsSource = null;
                            dtg_Centre.ItemsSource = LstCentreObs;
                        }
                    }

                }
            }
        }

        private void btn_ToutTournee_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsTournee item in LstTourneeObs)
            {
                if (item.IsSelect == false)
                    item.IsSelect = true;
            }
        }

        private void btn_RienTourne_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (ServiceAccueil.CsTournee item in LstTourneeObs)
            {
                if (item.IsSelect == true )
                    item.IsSelect = false ;
            }
        }

        private void dgMyDataGridSite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection <ServiceAccueil.CsSite>)dg.ItemsSource).ToList();
            lesFactureSelect.Where(t=>t.PK_ID !=((ServiceAccueil.CsSite)dg.SelectedItem).PK_ID ).ToList().ForEach(t => t.IsSelect = false);
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsSite SelectedObject = (ServiceAccueil.CsSite)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
        private void chk_Site_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_Site.ItemsSource != null)
                {
                    var lstSite = ((ObservableCollection<ServiceAccueil.CsSite>)dtg_Site.ItemsSource).ToList();
                    if (lstSite != null && this.dtg_Site.SelectedItem != null)
                    {
                        ServiceAccueil.CsSite leSiteSelect = (ServiceAccueil.CsSite)this.dtg_Site.SelectedItem;

                        var lstSiteAutre = lstSite.Where(t => t.PK_ID != leSiteSelect.PK_ID); ;
                        bool IsSiteCocher =Shared.ClasseMEthodeGenerique. checkSelectedItem((CheckBox)this.dtg_Site.Columns[0].GetCellContent(dtg_Site.SelectedItem as ServiceAccueil.CsSite) as CheckBox);
                        if (IsSiteCocher)
                        {
                            LstCentreObs.Clear();
                            foreach (var item in lesCentre.Where(t => t.FK_IDCODESITE == leSiteSelect.PK_ID).ToList())
                            {
                                item.IsSelect = false;
                                LstCentreObs.Add(item);
                            }
                            dtg_Centre.ItemsSource = null;
                            dtg_Centre.ItemsSource = LstCentreObs;

                            LsteProduitObs.Clear();
                            dtg_produit.ItemsSource = null;
                            dtg_produit .ItemsSource = LsteProduitObs;

                            LstTourneeObs.Clear();
                            dtg_zone.ItemsSource = null;
                            dtg_zone.ItemsSource = LstTourneeObs;
                        }
                        foreach (var item in lstSiteAutre)
                        {
                            bool IsSiteCoche = Shared.ClasseMEthodeGenerique.checkSelectedItem((CheckBox)this.dtg_Site.Columns[0].GetCellContent(item) as CheckBox);
                            if (IsSiteCoche)
                                Shared.ClasseMEthodeGenerique.DecheckerSelectedItem((CheckBox)this.dtg_Site.Columns[0].GetCellContent(item) as CheckBox);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }
        private void chk_Site_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_Site.ItemsSource != null)
                {
                    var lstSite = ((ObservableCollection<ServiceAccueil.CsSite>)dtg_Site.ItemsSource).ToList();
                    if (lstSite != null && this.dtg_Site.SelectedItem != null)
                    {
                        ServiceAccueil.CsSite leSiteSelect = (ServiceAccueil.CsSite)this.dtg_Site.SelectedItem;
                        bool IsSiteCocher =Shared.ClasseMEthodeGenerique . checkSelectedItem((CheckBox)this.dtg_Site.Columns[0].GetCellContent(dtg_Site.SelectedItem as ServiceAccueil.CsSite) as CheckBox);
                        if (!IsSiteCocher)
                        {
                            LstCentreObs.Clear();
                            dtg_Centre.ItemsSource = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Erreur");
                throw;
            }
        }

        private void dgMyDataGridCentre_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsCentre >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsCentre SelectedObject = (ServiceAccueil.CsCentre)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false ;
            }
        }
        private void chk_Centre_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_Centre.ItemsSource != null)
                {
                    var lstCentre = ((ObservableCollection<ServiceAccueil.CsCentre>)dtg_Centre.ItemsSource).ToList();
                    if (lstCentre != null && this.dtg_Centre.SelectedItem != null)
                    {
                        ServiceAccueil.CsCentre leCentreSelect = (ServiceAccueil.CsCentre)this.dtg_Centre.SelectedItem;
                        if (leCentreSelect == null)
                        {
                            Message.ShowInformation("Sélectionner le centre", "Index");
                            return;
                        }
                        var lstSiteAutre = lstCentre.Where(t => t.PK_ID != leCentreSelect.PK_ID); ;
                        bool IsSiteCocher = Shared.ClasseMEthodeGenerique.checkSelectedItem((CheckBox)this.dtg_Centre.Columns[0].GetCellContent(leCentreSelect) as CheckBox);
                        if (IsSiteCocher)
                        {
                            if (dtg_produit.ItemsSource != null)
                            {
                                var lesProduitSelect = ((ObservableCollection<ServiceAccueil.CsProduit>)dtg_produit.ItemsSource).ToList();
                                foreach (ServiceAccueil.CsProduit item in leCentreSelect.LESPRODUITSDUSITE)
                                {
                                    if (lesProduitSelect.FirstOrDefault(t => t.PK_ID == item.PK_ID) != null) continue;
                                    LsteProduitObs.Add(item);
                                }
                            }
                            else
                            {
                                foreach (ServiceAccueil.CsProduit item in leCentreSelect.LESPRODUITSDUSITE)
                                    LsteProduitObs.Add(item);
                            }
                            dtg_produit.ItemsSource = null;
                            dtg_produit.ItemsSource = LsteProduitObs;
                            if (LstZone != null && LstZone.Count != 0)
                                LstZoneCentre.AddRange(LstZone.Where(p => p.FK_IDCENTRE == leCentreSelect.PK_ID).OrderBy(t => t.CENTRE).ThenBy(s => s.CODE).ToList());

                            LstTourneeObs.Clear();
                            foreach (ServiceAccueil.CsTournee item in LstZoneCentre)
                                LstTourneeObs.Add(item);
                            dtg_zone.ItemsSource = null;
                            dtg_zone.ItemsSource = LstTourneeObs;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }
        private void chk_Centre_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_Centre.ItemsSource != null)
                {
                    var lstSite = ((ObservableCollection<ServiceAccueil.CsCentre>)dtg_Centre.ItemsSource).ToList();
                    if (lstSite != null && this.dtg_Centre.SelectedItem != null)
                    {
                        ServiceAccueil.CsCentre leCentreSelect = (ServiceAccueil.CsCentre)this.dtg_Centre.SelectedItem;
                        if (leCentreSelect == null)
                        {
                            Message.ShowInformation("Sélectionner le centre", "Index");
                            return;
                        }
                        bool IsSiteCocher = Shared.ClasseMEthodeGenerique.checkSelectedItem((CheckBox)this.dtg_Centre.Columns[0].GetCellContent(leCentreSelect) as CheckBox);
                        if (LstCentreSelect.FirstOrDefault(p => p.CODE == leCentreSelect.CODE && p.PK_ID == leCentreSelect.PK_ID) != null)
                            LstCentreSelect.Remove(leCentreSelect);

                        List<ServiceAccueil.CsTournee> LstTourneeSupp = LstZoneCentre.Where(p => p.FK_IDCENTRE == leCentreSelect.PK_ID).ToList();
                        List<int> LstTourneeObsupp = new List<int>();
                        if (LstTourneeSupp != null && LstTourneeSupp.Count != 0)
                        {
                            foreach (ServiceAccueil.CsTournee item in LstTourneeSupp)
                                LstTourneeObsupp.Add(item.PK_ID);
                            LstZoneCentre = LstZoneCentre.Where(t => !LstTourneeObsupp.Contains(t.PK_ID)).ToList();
                            LstTourneeObs.Clear();

                            foreach (ServiceAccueil.CsTournee item in LstZoneCentre)
                                LstTourneeObs.Add(item);
                            dtg_zone.ItemsSource = null;
                            dtg_zone.ItemsSource = LstTourneeObs;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }


        private void dgMyDataGridProduit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsProduit  >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsProduit SelectedObject = (ServiceAccueil.CsProduit)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false ;

            }
        }
        private void chk_Produit_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_produit.ItemsSource != null)
                {
                    var lstProduit = ((ObservableCollection<ServiceAccueil.CsProduit>)dtg_produit.ItemsSource).ToList();
                    if (lstProduit != null && this.dtg_produit.SelectedItem != null)
                    {
                        ServiceAccueil.CsProduit leProduitSelect = (ServiceAccueil.CsProduit)this.dtg_produit.SelectedItem;
                        if (leProduitSelect == null)
                        {
                            Message.ShowInformation("Sélectionner le centre", "Index");
                            return;
                        }
                        leProduitSelect.IsSelect = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }
        private void chk_Produit_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_produit.ItemsSource != null)
                {
                    var lstProduit = ((ObservableCollection<ServiceAccueil.CsProduit>)dtg_produit.ItemsSource).ToList();
                    if (lstProduit != null && this.dtg_produit.SelectedItem != null)
                    {
                        ServiceAccueil.CsProduit leProduitSelect = (ServiceAccueil.CsProduit)this.dtg_produit.SelectedItem;
                        if (leProduitSelect == null)
                        {
                            Message.ShowInformation("Sélectionner le produit", "Index");
                            return;
                        }
                        leProduitSelect.IsSelect = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }

        private void dgMyDataGridCategorie_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsCategorieClient >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsCategorieClient SelectedObject = (ServiceAccueil.CsCategorieClient)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;

            }
        }
        private void chk_Categorie_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_Categorie.ItemsSource != null)
                {
                    var lstCategorie = ((ObservableCollection<ServiceAccueil.CsCategorieClient>)dtg_Categorie.ItemsSource).ToList();
                    if (lstCategorie != null && this.dtg_Categorie.SelectedItem != null)
                    {
                        ServiceAccueil.CsCategorieClient laCategorieSelect = (ServiceAccueil.CsCategorieClient)this.dtg_Categorie.SelectedItem;
                        if (laCategorieSelect == null)
                        {
                            Message.ShowInformation("Sélectionner la catégorie", "Index");
                            return;
                        }
                        laCategorieSelect.IsSelect = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
            }
        }
        private void chk_Categorie_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (dtg_Categorie.ItemsSource != null)
            {
                var lstCateg = ((ObservableCollection<ServiceAccueil.CsCategorieClient>)dtg_Categorie.ItemsSource).ToList();
                if (lstCateg != null && this.dtg_Categorie.SelectedItem != null)
                {
                    ServiceAccueil.CsCategorieClient laCategorieSelect = (ServiceAccueil.CsCategorieClient)this.dtg_Categorie.SelectedItem;
                    if (laCategorieSelect == null)
                    {
                        Message.ShowInformation("Sélectionner la catégorie", "Index");
                        return;
                    }
                    laCategorieSelect.IsSelect = false;
                }
            }
        }


        private void dgMyDataGridFrequence_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsFrequence >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsFrequence SelectedObject = (ServiceAccueil.CsFrequence)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
        private void chk_Frequence_Checked_1(object sender, RoutedEventArgs e)
        {
            if (dtg_frequence.ItemsSource != null)
            {
                var lstFrequence = ((ObservableCollection<ServiceAccueil.CsFrequence>)dtg_frequence.ItemsSource).ToList();
                if (lstFrequence != null && this.dtg_frequence.SelectedItem != null)
                {
                    ServiceAccueil.CsFrequence laFrequenceSelect = (ServiceAccueil.CsFrequence)this.dtg_frequence.SelectedItem;
                    if (laFrequenceSelect == null)
                    {
                        Message.ShowInformation("Sélectionner la fréquence", "Index");
                        return;
                    }
                    laFrequenceSelect.IsSelect = true;
                }
            }
        }
        private void chk_Frequence_Unchecked_1(object sender, RoutedEventArgs e)
        {
            if (dtg_frequence.ItemsSource != null)
            {
                var lstProduit = ((ObservableCollection<ServiceAccueil.CsFrequence>)dtg_frequence.ItemsSource).ToList();
                if (lstProduit != null && this.dtg_frequence.SelectedItem != null)
                {
                    ServiceAccueil.CsFrequence laFrequenceSelect = (ServiceAccueil.CsFrequence)this.dtg_frequence.SelectedItem;
                    if (laFrequenceSelect == null)
                    {
                        Message.ShowInformation("Sélectionner la fréquence", "Index");
                        return;
                    }
                    laFrequenceSelect.IsSelect = false;
                }
            }
        }

        private void dgMyDataGridTournee_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var lesFactureSelect = ((ObservableCollection<ServiceAccueil.CsTournee >)dg.ItemsSource).ToList();
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsTournee SelectedObject = (ServiceAccueil.CsTournee)dg.SelectedItem;
                if (SelectedObject.IsSelect == false)
                    SelectedObject.IsSelect = true;
                else
                    SelectedObject.IsSelect = false;
            }
        }
        private void chk_Tournee_Checked_1(object sender, RoutedEventArgs e)
        {
            //if (dtg_zone.ItemsSource != null)
            //{
            //    var lstTournee = ((ObservableCollection<ServiceAccueil.CsTournee>)dtg_zone.ItemsSource).ToList();
            //    if (lstTournee != null && this.dtg_zone.SelectedItem != null)
            //    {
            //        ServiceAccueil.CsTournee laTournee = (ServiceAccueil.CsTournee)this.dtg_zone.SelectedItem;
            //        if (laTournee == null)
            //        {
            //            Message.ShowInformation("Sélectionner la tournée", "Index");
            //            return;
            //        }
            //        laTournee.IsSelect = true;
            //    }
            //}
        }
        private void chk_Tournee_Unchecked_1(object sender, RoutedEventArgs e)
        {
            //if (dtg_zone.ItemsSource != null)
            //{
            //    var lstProduit = ((ObservableCollection<ServiceAccueil.CsTournee>)dtg_zone.ItemsSource).ToList();
            //    if (lstProduit != null && this.dtg_frequence.SelectedItem != null)
            //    {
            //        ServiceAccueil.CsTournee laTournee = (ServiceAccueil.CsTournee)this.dtg_zone.SelectedItem;
            //        if (laTournee == null)
            //        {
            //            Message.ShowInformation("Sélectionner la tournéé", "Index");
            //            return;
            //        }
            //        laTournee.IsSelect = false;
            //    }
            //}
        }

        public static bool IsLotIsole(string NUMLOTRI)
        {
            string FactureIsoleIndex = "00002";
            string FactureResiliationIndex = "00001";
            string FactureAnnulatinIndex = "00003";
            if (new string[] { FactureIsoleIndex, FactureResiliationIndex, FactureAnnulatinIndex }.Contains(NUMLOTRI.Substring(SessionObject.Enumere.TailleCentre, (SessionObject.Enumere.TailleNumeroBatch - SessionObject.Enumere.TailleCentre))))
                return true;
            else
                return false;
        }

        private void AfficherLotPourSuppression(string Matricule,string debut ,string fin)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.ChargerLotriPourDeleteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args.Cancelled || args.Error != null)
                        {
                            OKButton.IsEnabled = true;
                            Message.ShowError("Erreur d'invocation du service.", "Erreur");
                            return;
                        }
                        List<CsLotri> LstLotInit = args.Result;
                        List<CsLotri> LstLot = args.Result;

                        List<CsLotri> listAfficher = new List<CsLotri>();
                        if (LstLot != null && LstLot.Count != 0)
                        {
                            List<CsLotri> lstLotDist = new List<CsLotri>();
                            var lstCentreDistnct = LstLot.Where(p=>!IsLotIsole(p.NUMLOTRI)).Select(t => new { t.NUMLOTRI  }).Distinct().ToList();
                            foreach (var item in lstCentreDistnct)
                            {
                                List<CsLotri> lstLot = LstLot.Where(t => t.NUMLOTRI == item.NUMLOTRI).ToList();
                                foreach (CsLotri  items in lstLot)
                                {
                                    if (lstLot.FirstOrDefault(t=>!string.IsNullOrEmpty( t.ETATFAC2)) == null ) 
                                        items.ETATFAC2 = items.NUMLOTRI;
                                    listAfficher.Add(items);
                                }
                            }
                            UcListeDeLotri ctrl = new UcListeDeLotri(listAfficher);
                            ctrl.Show();
                        }
                        else
                        {
                            Message.ShowInformation("Aucun lot trouvé", "Facturation");
                        }

                    }
                    catch (Exception ex)
                    {
                        OKButton.IsEnabled = true;
                        Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                service.ChargerLotriPourDeleteAsync(Matricule, debut,fin );
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_AfficherLot_Click_1(object sender, RoutedEventArgs e)
        {
            AfficherLotPourSuppression(UserConnecte.matricule, this.txt_Numlotdebt.Text, this.txt_numLotFin.Text);
        }

        private void Txt_Period_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void VerifierPeriode()
        {
            if (!string.IsNullOrEmpty(this.Txt_Period.Text))
            {
                if (this.Txt_Period.Text.Length == 7)
                {
                    if (!string.IsNullOrEmpty(this.Txt_batch.Text)) this.OKButton.IsEnabled = true;
                    if (!ClasseMEthodeGenerique.IsFormatPeriodeValide(this.Txt_Period.Text))
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Index.Langue.libelleModule, Galatee.Silverlight.Resources.Index.Langue.MsgFormatInvalide, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_Period.Focus();
                        };
                        w.Show();
                    }
                }
                else
                {
                    Message.ShowInformation("Le format de la période n'est pas correct", "Facturation");
                    this.Txt_Period.Focus();
                    return;
                }
            }
        }




    }
}

