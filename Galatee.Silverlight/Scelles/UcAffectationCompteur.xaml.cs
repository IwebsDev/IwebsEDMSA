using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
    public partial class UcAffectationCompteur : ChildWindow
    {
        Galatee.Silverlight.ServiceAccueil.CsDemandeBase laDemandeSelect = null;
        CsDemande laDetailDemande = null;
        private ServiceAccueil.CsCompteurBta ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public ServiceAccueil.CsCompteurBta ObjetSelectionneScelle { get; set; }
        List<ServiceAccueil.CsCompteurBta> ListdesScelles = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsCompteurBta> DonnesDatagridMargasinV = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsCompteurBta> DonnesDatagridMargasinV_Affecte = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsCompteurBta> ListSaisie = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsCompteurBta> listForInsertOrUpdate = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsCompteurBta> listForInsertOrUpdate1 = new List<ServiceAccueil.CsCompteurBta>();
        List<ServiceAccueil.CsUtilisateur> lstAllUser = new List<ServiceAccueil.CsUtilisateur>();
        List<ServiceAccueil.CsUtilisateur> lstCentreUser = new List<ServiceAccueil.CsUtilisateur>();
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        public UcAffectationCompteur()
        {
            InitializeComponent();
            //RemplirListeCmbDeMotifsExistant();
            //ChargeListeUser();
            //Rdb_RmScelle.IsChecked = true;
            ModeExecution = SessionObject.ExecMode.Creation;
          ///  Dateremise.PI = DateTime.Now;
            GetData();
            ChargerListDesSite();
            ChargerProduit();
        }




        public UcAffectationCompteur(Galatee.Silverlight.ServiceScelles.CsRemiseScelles pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                //Translate();
                var margaVirtuelle = new Galatee.Silverlight.ServiceAccueil.CsCompteurBta();
                if (pObject != null)
                 //   ObjetSelectionnee = Utility.ParseObject(Remise, pObject as CsCompteurBta);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                //RemplirListeCmbDeMotifsExistant();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                    }
                 }

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                   // btn_ajout.IsEnabled = true;
                }
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                {
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetData()
        {
            try
            {

                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();
                ChargerListeCompteur();
                //ChargerListeCompteurInDisponible();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerListeCompteurDisponible()
        {
            BuzyActions();
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
            client.SelectAllCompteurCompleted += (ssender, args) =>
            {
                UnBuzyActions();
                if (args.Cancelled || args.Error != null)
                {
                    string error = args.Error.Message;
                    Message.ShowError(error, Languages.LibelleReceptionScelle);
                    return;
                }
                if (args.Result == null)
                {
                    Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                    return;
                }
                DonnesDatagridMargasinV.Clear();
                if (args.Result != null)
                    foreach (var item in args.Result)
                    {
                        if (item.StatutCompteur == "Non_Affecté" && item.EtatCompteur_ID != 2) // compteur non défectueux
                        {
                            item.ETAT = "0"; //Non affecté à un magasin virtuel
                            DonnesDatagridMargasinV.Add(item);
                        }
                        //else
                        //    DonnesDatagridMargasinV_Affecte.Add(item);
                    }
                dgCompteur.ItemsSource = DonnesDatagridMargasinV.OrderBy(c => c.NUMERO).ToList();
                //dgMargasinVirtuelle.ItemsSource = DonnesDatagridMargasinV_Affecte.OrderBy(c => c.NUMERO).ToList();
            };
            client.SelectAllCompteurAsync();
        }
        private void ChargerListeCompteur()
        {
            try
            {
                BuzyActions();
                AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllCompteurPourAffectationCompleted += (ssender, args) =>
                {
                    UnBuzyActions();
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleReceptionScelle);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    DonnesDatagridMargasinV.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            if (item.StatutCompteur == "Non_Affecté" && item.EtatCompteur_ID != 2) // compteur non défectueux
                            {
                                item.ETAT = "0"; //Non affecté à un magasin virtuel
                                DonnesDatagridMargasinV.Add(item);
                            }
                        }
                    dgCompteur.ItemsSource = DonnesDatagridMargasinV.OrderBy(c => c.NUMERO).ToList();

                    DonnesDatagridMargasinV_Affecte.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            if (item.StatutCompteur == "Affecté")
                            {
                                item.ETAT = "1"; //Déjà affecté à un magasin virtuel
                                DonnesDatagridMargasinV_Affecte.Add(item);
                            }
                        }
                    dgMargasinVirtuelle.ItemsSource = DonnesDatagridMargasinV_Affecte.OrderBy(c => c.NUMERO).ToList();
                };
                client.SelectAllCompteurPourAffectationAsync();
            }
            catch (Exception EX)
            {
                Message.ShowInformation(EX.Message, "Erreur");
            }
        }

        private void UnBuzyActions()
        {
            btn_Rafraichir.IsEnabled = true;
            OKButton.IsEnabled = true;
            desableProgressBar();
        }

        private void BuzyActions()
        {
            btn_Rafraichir.IsEnabled = false;
            OKButton.IsEnabled = false;
            allowProgressBar();
        }
        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }
        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }
        private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    List<ServiceAccueil. CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
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



        private List<ServiceAccueil.CsCompteurBta> GetInformationsFromScreen()
        {

            try
            {
                var listObjetForInsertOrUpdate = new List<ServiceAccueil.CsCompteurBta>();

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {

                   
                        ListSaisie.Clear();
                        ListSaisie.AddRange((List<ServiceAccueil.CsCompteurBta>)dgMargasinVirtuelle.ItemsSource);
                        foreach (ServiceAccueil.CsCompteurBta element in ListSaisie)
                        {
                            if (element.ETAT == "0") //si l'élément vient d'être ajouté
                            {
                                element.FK_IDCENTRE = ((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID;
                                element.FONCTIONNEMENT = "1";
                                element.StatutCompteur = "Affecté";
                                element.ETAT = "Affecté";
                                element.DATECREATION = DateTime.Now;
                                element.USERCREATION = UserConnecte.matricule;
                                element.NUMERO = element.Numero_Compteur;
                                listObjetForInsertOrUpdate.Add(element);
                            }
                        }
                  }
                  return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
                return null;
            }
        }
        private List<ServiceAccueil.CsCompteurBta> GetInformationsFromScreen1()
        {

            try
            {
                var listObjetForInsertOrUpdate = new List<ServiceAccueil.CsCompteurBta>();

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {


                    ListSaisie.Clear();
                    ListSaisie.AddRange((List<ServiceAccueil.CsCompteurBta>)dgCompteur.ItemsSource);
                    foreach (ServiceAccueil.CsCompteurBta element in ListSaisie)
                    {
                        if (element.ETAT == "1") //si c'est un élément rétiré du magasin
                        {

                            element.FK_IDCENTRE = ((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID;
                            element.FONCTIONNEMENT = "1";
                            element.StatutCompteur = "Non_Affecté";
                            element.ETAT = "Non_Affecté";
                            element.DATECREATION = DateTime.Now;
                            element.NUMERO = element.Numero_Compteur;
                            listObjetForInsertOrUpdate.Add(element);
                        }
                    }
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
                return null;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem == null )
                {
                    Message.ShowInformation("Sélectionner le centre", "Affectation");
                        return ;
                }

                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        listForInsertOrUpdate1 = GetInformationsFromScreen1();
                        //var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                        AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));

                        //if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        //if (listForInsertOrUpdate != null)
                        //{
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {

                                //if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                                //{
                                BuzyActions();
                                    service.InsertMargasinVirtuelleCompleted += (snder, insertR) =>
                                    {
                                        UnBuzyActions();
                                        if (insertR.Cancelled ||
                                            insertR.Error != null)
                                        {
                                            Message.ShowError(insertR.Error.Message, Languages.Commune);
                                            return;
                                        }
                                        if (insertR.Result==0)
                                        {
                                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Commune);
                                            return;
                                        }
                                        //UpdateParentList(listForInsertOrUpdate[0]);
                                        OnEvent(null);
                                        DialogResult = true;
                                    };
                                    service.InsertMargasinVirtuelleAsync(listForInsertOrUpdate,listForInsertOrUpdate1);
                                //}
                            }
                            //if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            //{
                            //    service.UpdateMargasinVirtuelleCompleted += (snder, UpdateR) =>
                            //    {
                            //        if (UpdateR.Cancelled ||
                            //            UpdateR.Error != null)
                            //        {
                            //            Message.ShowError(UpdateR.Error.Message, Languages.Commune);
                            //            return;
                            //        }
                            //        if (UpdateR.Result==0)
                            //        {
                            //            Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Commune);
                            //            return;
                            //        }
                            //       // UpdateRemise(listForInsertOrUpdate[0]);
                            //        OnEvent(null);
                            //        DialogResult = true;
                            //    };
                            //    service.UpdateMargasinVirtuelleAsync(listForInsertOrUpdate);
                            //}
                        //}
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Commune);
            }
        }

     
  
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void Charger_Click(object sender, RoutedEventArgs e)
        {
                //ObjetSelectionneScelle = dgCompteur.SelectedItem as CsCompteurBta;


            if (chb_SaisiNombreCompteurSouhaite.IsChecked == true)
            {
                if (!string.IsNullOrWhiteSpace(txt_NombreCompteurSouhaite.Text))
                {
                    //on recupere le nmbre selectionné ,soit x
                    int NombreDeScelleSouhaiter = 0;
                    int NumDepart=0;
                    int NumFin=0;
                    if (int.TryParse(txt_NombreCompteurSouhaite.Text, out NombreDeScelleSouhaiter) && int.TryParse(txt_DebutNumCompteur.Text, out NumDepart) && int.TryParse(txt_FinNumCompteur.Text, out NumFin) )
                    {
                        if (NombreDeScelleSouhaiter > 0 && NumDepart  > 0  && NumFin > 0)
                        {
                           if (NumFin>NumDepart)
	                       {
                               var DataSourcedgScelle = ((List<ServiceAccueil.CsCompteurBta>)dgCompteur.ItemsSource) ;
                                var DataSourcedgRemis = ((List<ServiceAccueil.CsCompteurBta>)dgMargasinVirtuelle.ItemsSource);
                            
                                var ElementAAffecter = DataSourcedgScelle.Where(c => int.Parse(c.Numero_Compteur.Trim()) >= NumDepart && int.Parse(c.Numero_Compteur.Trim()) <= NumFin).Take(NombreDeScelleSouhaiter);
                                var ListElementAAffecter = ElementAAffecter != null ? ElementAAffecter.ToList() : null;
                                ListElementAAffecter.AddRange(DataSourcedgRemis != null ? DataSourcedgRemis.ToList() : new List<ServiceAccueil.CsCompteurBta>());

                                if (ListElementAAffecter != null)
                                {

                                    dgMargasinVirtuelle.ItemsSource = null;
                                    dgMargasinVirtuelle.ItemsSource = ListElementAAffecter.OrderBy(c => c.NUMERO).ToList();

                                    var ElementRestantAAffecter = DataSourcedgScelle.Where(s => !ListElementAAffecter.Contains(s));
                                    dgCompteur.ItemsSource = null;
                                    dgCompteur.ItemsSource = ElementRestantAAffecter.OrderBy(c => c.NUMERO).ToList();
                                    return;
                                }
                                else
                                {
                                    Message.ShowWarning("Aucun éléments affecté", "Information");
                                    return;
                                } 
	                        }else
	                        {
                               Message.ShowWarning("Veuillez vous assurer que le numéro de départ soit inférieur au numéro de fin", "Information");
                                    return;
	                        }
                        }
                        else
                        {
                            Message.ShowWarning("Veuillez saisir des valeurs supperieurs à 0 dans les differents champs", "Information");
                            return;
                        }
                    }
                    else
                    {
                        Message.ShowWarning("Veuillez saisir une valeur numérique", "Information");
                        return;
                    }
                }
                else
                {
                    Message.ShowWarning("Veuillez saisir le nombre de scelles souhaité", "Information");
                    return;
                }
            }




                Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<ServiceAccueil.CsCompteurBta>(dgCompteur, dgMargasinVirtuelle);
               
            
          
        }

        private void Decharger_Click(object sender, RoutedEventArgs e)
        {

            Galatee.Silverlight.Shared.CommonMethode.TransfertDataGrid<ServiceAccueil.CsCompteurBta>(dgMargasinVirtuelle, dgCompteur);
        }
        private void chargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    dgCompteur.ItemsSource = null;
                    dgMargasinVirtuelle.ItemsSource = DonnesDatagridMargasinV.OrderBy(c => c.NUMERO).ToList();
                    dgCompteur.ItemsSource = null;
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void DechargerTout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    dgMargasinVirtuelle.ItemsSource = null;
                    dgCompteur.ItemsSource = null;
                    dgCompteur.ItemsSource = DonnesDatagridMargasinV.OrderBy(c => c.NUMERO).ToList();
                   
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    var MyItemsSource = DonnesDatagridMargasinV_Affecte;

                    ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        //FilterParCentreEtParProduit();
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
                        this.txtSite.Text = csSite.CODE ?? string.Empty;
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

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    FilterParCentreEtParProduit();
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }

        private void FilterParCentreEtParProduit()
        {
            var MyItemsSource = DonnesDatagridMargasinV_Affecte;

            if (Cbo_Centre.SelectedValue != null)
            {
                int id_centre = ((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID;
                MyItemsSource = MyItemsSource != null ? MyItemsSource.Where(t => t.FK_IDCENTRE == id_centre).OrderBy(c => c.NUMERO).ToList() : new List<ServiceAccueil.CsCompteurBta>();
                dgMargasinVirtuelle.ItemsSource = MyItemsSource;
            }
            if (Cbo_Produit.SelectedValue != null)
            {
                int ID_produi = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                if (dgCompteur.ItemsSource != null)
                    dgCompteur.ItemsSource = DonnesDatagridMargasinV.Where(t => t.FK_IDPRODUIT == ID_produi).OrderBy(c => c.NUMERO).ToList();

                ItemsSource = MyItemsSource != null ? MyItemsSource.Where(t => t.FK_IDPRODUIT == ID_produi).OrderBy(c => c.NUMERO).ToList() : new List<ServiceAccueil.CsCompteurBta>();
                dgMargasinVirtuelle.ItemsSource = ItemsSource;
            }
        }

        private void chb_SaisiNombreScelleSouhaite_Checked(object sender, RoutedEventArgs e)
        {
            txt_NombreCompteurSouhaite.Visibility = Visibility.Visible;
             
            lbl_Debut.Visibility = Visibility.Visible;
            txt_DebutNumCompteur.Visibility = Visibility.Visible;

            lbl_Fin.Visibility = Visibility.Visible;
            txt_FinNumCompteur.Visibility = Visibility.Visible;

            dgCompteur.IsEnabled = false;
        }

        private void chb_SaisiNombreScelleSouhaite_Unchecked(object sender, RoutedEventArgs e)
        {
            txt_NombreCompteurSouhaite.Visibility = Visibility.Collapsed;

            lbl_Debut.Visibility = Visibility.Collapsed;
            txt_DebutNumCompteur.Visibility = Visibility.Collapsed;

            lbl_Fin.Visibility = Visibility.Collapsed;
            txt_FinNumCompteur.Visibility = Visibility.Collapsed;

            dgCompteur.IsEnabled = true;

        }
        //private void dgLotScelle_MouseRightButtonDown(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        ObjetSelectionneLot = dgLotScelle.SelectedItem as CsTbLot;
        //        if (txtNombredeScelle.Text != null)
        //        {
        //            int lot = int.Parse(txtNombredeScelle.Text.Trim());
        //            lot = (int)ObjetSelectionneLot.Nombre_scelles_reçu + lot;
        //            txtNombredeScelle.Text = lot.ToString();
                
        //        }
        //        else txtNombredeScelle.Text = ObjetSelectionneLot.Nombre_scelles_reçu.ToString();
        //        //SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsCentre;
        //        //SessionObject.gridUtilisateur = dtgrdParametre;
        //    }
        //    catch (Exception ex)
        //    {
        //        //var dialogResult = new DialogResult(ex.Message, Languages.LibelleNationalite, false, true, false);
        //        //dialogResult.Closed += new EventHandler(DialogResultOk);
        //        //dialogResult.Show();
        //        throw ex;

        //    }
        //}




        public List<ServiceAccueil.CsCompteurBta> ItemsSource { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FilterParCentreEtParProduit();
        }
    }
}

