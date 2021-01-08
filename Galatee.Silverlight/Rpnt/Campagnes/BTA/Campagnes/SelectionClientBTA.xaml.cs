//using Galatee.Silverlight.ServiceRpnt;
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
using System.Collections.ObjectModel;
using Galatee.Silverlight.Rpnt.Common;
using System.Windows.Data;
using Galatee.Silverlight.Rpnt.Helper;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class SelectionClientBTA : ChildWindow
    {

        public delegate void BranchementClientEventHandler(object sender, BranchementClientEventArgs e);
        public event BranchementClientEventHandler MyHandler;


        protected virtual void OnEvent(BranchementClientEventArgs e)
        {
            if (MyHandler != null)
                MyHandler(this, e);
        }

        #region Variables

        List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> Conteneur_listeCampagne = new List<ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>();
        ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsBrt> ListeBranchement = new ObservableCollection<Galatee.Silverlight.ServiceRecouvrement.CsBrt>();

        #endregion

        #region Constructers
        
        public SelectionClientBTA()
        {
            InitializeComponent();


            LoadCampagne();
        }
        public SelectionClientBTA(List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> Conteneur_listeCampagne)
        {
            InitializeComponent();

            LoadCampagne();
            //Load_Campagne(Conteneur_listeCampagne);
        }

        #endregion

        #region Service
        public void LoadCampagne()
        {
            //if (SessionObject.campagne.Count() > 0)
            //{
            //    Load_Campagne();
            //}
            //else
            //{
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
            service.GetCampagneBTAControleAsync();
            service.GetCampagneBTAControleCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
                            Load_Campagne(res.Result);
                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");

                    LoadingManager.EndLoading(handler);
                }
                catch (Exception)
                {

                    throw;
                }
            };
            //}

        }
        public void LoadBranchement(List<Galatee.Silverlight.ServiceRecouvrement.CsClient> ListeClientSelection)
        {
            //Remplir le grille des branchement avec(jointure entre branchement et client)
            //Activation de busy indictor
            //busyIndicatorBranchement.IsBusy = true;

            //Handle
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            //int handler = LoadingManager.BeginLoading("Recuperation des factures ...");
            service.GetBranchementBTAControleAsync(ListeClientSelection);
            service.GetBranchementBTAControleCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != null)
                        {
                            //bool Exist = false;
                            CleanAllBranchement();
                            List<Galatee.Silverlight.ServiceRecouvrement.CsBrt> LISTEBRANCHEMENT = res.Result;
                            ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListElementsCamp=new List<ServiceRecouvrement.CsElementsDeCampagneBTA>();
                            foreach (var item in LISTEBRANCHEMENT)
	                        {
                                Galatee.Silverlight.ServiceRecouvrement.CsElementsDeCampagneBTA ElementsDeCampagneBTA = new ServiceRecouvrement.CsElementsDeCampagneBTA();

                                ElementsDeCampagneBTA.Campagne_ID = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).Campagne_ID;
                                ElementsDeCampagneBTA.Contrat_ID = item.PK_ID;
                                ElementsDeCampagneBTA.DateCreation = DateTime.Now;
                                ElementsDeCampagneBTA.MatriculeAgentCreation = UserConnecte.matricule;
                                ElementsDeCampagneBTA.DateSelection = DateTime.Now;
                                ElementsDeCampagneBTA.ReferenceClient = item.CLIENT;
                                ElementsDeCampagneBTA.Nom = item.NOMABON;
                                ElementsDeCampagneBTA.Libelle_Centre = item.CENTRE;
                                ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListElementsCamp.Add(ElementsDeCampagneBTA); 
	                        }

                            dgbranchement.ItemsSource = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListElementsCamp;
                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");
                    //LoadingManager.EndLoading(handler);
                }
                catch (Exception)
                {

                    throw;
                }
            };

            //Desactivation du busy indicator
            //busyIndicatorBranchement.IsBusy = false;
        }
        private void SaveCampagne()
        {
            //Association de campagne à liste de branchement selection
            Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            int handler = LoadingManager.BeginLoading("traitement des données ...");
            List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> listecampageneTosave = (List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>)dgCampagne.ItemsSource;
          
            service.SaveCampagneElementAsync(listecampageneTosave);
            service.SaveCampagneElementCompleted += (er, res) =>
            {
                try
                {
                    if (res.Error != null || res.Cancelled)
                        Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    else
                        if (res.Result != false)
                        {
                            Message.Show("Modification enrégistrer avec succès",
                                "Erreur");

                            Conteneur_listeCampagne.Clear();
                            Conteneur_listeCampagne = listecampageneTosave;
                            //CallServiceToLoadCamp();

                        }
                        else
                            Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                "Erreur");

                    LoadingManager.EndLoading(handler);
                }
                catch (Exception)
                {
                    throw;
                }
            };
        }


        #endregion

        #region Event Handler
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (dgCampagne.SelectedItem != null)
            {
                SaveCampagne();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnEvent(null);
            this.DialogResult = false;
        }
        private void btnrechclient_Click(object sender, RoutedEventArgs e)
        {
            if (dgCampagne.SelectedItem!=null)
            {
                var SelectedCampagne = (Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem;

                Galatee.Silverlight.Recouvrement.AffecteClientCampagne FRMClient = new Galatee.Silverlight.Recouvrement.AffecteClientCampagne(SelectedCampagne.fk_idCentre.Value);
                FRMClient.MethodeAbonnee += FRMClient_MethodeAbonnee;
                FRMClient.Show();
            }
            else
            {
                Message.Show("Veuillez Sélectionner une campagne ", "Info");
                return;
            }
        }

        private void FRMClient_MethodeAbonnee(object sender, Galatee.Silverlight.Rpnt.Helper.BranchementClientEventArgs e)
        {

            if (e.ListeClientEligibleSellection!=null)
            {
                LoadBranchement((List<Galatee.Silverlight.ServiceRecouvrement.CsClient>)e.ListeClientEligibleSellection);
                //((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).Methode_ID = ((Galatee.Silverlight.ServiceRecouvrement.CsRefMethodesDeDetectionClientsBTA)e.MethodeDetection).Methode_ID;

                //var Mth_Detection = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).Methode_ID;
                //((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).PERIODE = e.PeriodeDepart;
                //var PeriodeDepart = ((Galatee.Silverlight.ServiceRecouvrement.CsRefMethodesDeDetectionClientsBTA)dgCampagne.SelectedItem).PERIODE;
            }
            
        }

        private void dgbranchement_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void dgbranchement_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                ctxmn = new ContextMenu();

                mnitem = new MenuItem();
                mnitem.Header = "Consultation";
                ctxmn.Items.Add(mnitem);
                mnitem.Click += new RoutedEventHandler(mnitem_Click);

                //mnitem = new MenuItem();
                //mnitem.Header = "Modification";
                //ctxmn.Items.Add(mnitem);
                //mnitem.Click += new RoutedEventHandler(mnitem_Click);

                mnitem = new MenuItem();
                mnitem.Header = "Supprimer";
                ctxmn.Items.Add(mnitem);
                mnitem.Click += new RoutedEventHandler(mnitem_Click);

                ctxmn.IsOpen = true;
                ctxmn.HorizontalOffset = e.GetPosition(null).X;
                ctxmn.VerticalOffset = e.GetPosition(null).Y;
            }
        }

        private void dgbranchement_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }
        private void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            dgbranchement.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgCampagne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCampagne.SelectedItem!=null)
            {
                var SelectedCampagne =(Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO) dgCampagne.SelectedItem;

                var AgentCreateur = SessionObject.ListeDesAgents.FirstOrDefault(a => a.MATRICULE == SelectedCampagne.MatriculeAgentCreation);
                if(AgentCreateur!=null)
                    txtagcrea.Text=AgentCreateur.NOM+" "+AgentCreateur.PRENOM+ " ( "+ AgentCreateur.MATRICULE + " )";

                txtdateCrea.Text = SelectedCampagne.DateCreation.Date.ToString();

                var CentreCampagne=SessionObject.LstCentre.FirstOrDefault(c=> c.PK_ID==SelectedCampagne.fk_idCentre);
                if(CentreCampagne!=null)
                    txtExploitaion.Text = CentreCampagne.LIBELLE + " ( " + SelectedCampagne.CodeCentre + " )";

                txtlibcamp.Text = SelectedCampagne.Libelle_Campagne;

                txtnbrbranch.Text = SelectedCampagne.NbreElements.ToString();

                txtstatut.Text = SelectedCampagne.Statut_ID == 0 ? "Inactif" : "Actif";


                dgbranchement.ItemsSource = ((Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO)dgCampagne.SelectedItem).ListElementsCamp;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //new CreationLotManuelle().Show();
            new CreationLotBTA((List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>)dgCampagne.ItemsSource).Show();

        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region Menu contextuel
        ContextMenu ctxmn = null;
        MenuItem mnitem;
        void mnitem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnitem = (MenuItem)sender;

            switch (mnitem.Header.ToString())
            {
                case "Consultation":
                    //TODO: gestion d'écrant de consultation client
                   
                    break;
                case "Supprimer":
                    
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Methode

        public void CleanAllBranchement()
        {
            ListeBranchement.Clear();
        }

        private void Load_Campagne(List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> Conteneur_listeCampagne)
        {
            this.Conteneur_listeCampagne = new List<ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>();
            this.Conteneur_listeCampagne = Conteneur_listeCampagne;


            foreach (var item in this.Conteneur_listeCampagne)
            {
                var CentreCampagne = SessionObject.LstCentre.FirstOrDefault(c => c.PK_ID == item.fk_idCentre);
                item.CodeCentre = CentreCampagne.LIBELLE + " ( " + item.CodeCentre + " )";
            }

            dgCampagne.ItemsSource = this.Conteneur_listeCampagne;
        }

        #endregion

    }
}

