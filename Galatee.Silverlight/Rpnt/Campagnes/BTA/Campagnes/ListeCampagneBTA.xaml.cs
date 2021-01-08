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
//using Galatee.Silverlight.ServiceRpnt;
//using Galatee.Silverlight.Rpnt.Common;
//using Galatee.Silverlight.Rpnt.Campagnes.BTA.Campagnes;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class ListeCampagneBTA : ChildWindow
    {
        ObservableCollection<Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA> listeCampagne = new ObservableCollection<Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA>();
        ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsCentre> ListExploitation = new ObservableCollection<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA C_BTA = new Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA();
        List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO> Conteneur_listeCampagne = new List<Galatee.Silverlight.ServiceRecouvrement.CsCampagnesBTAAccessiblesParLUO>();


        public ListeCampagneBTA()
        {
            InitializeComponent();
            dgCampagne.ItemsSource = listeCampagne;

            LoadExploitation();
        }


        public void LoadExploitation()
        {
            if (SessionObject.LstCentre.Count() > 0)
            {
                ListExploitation.Clear();
                foreach (var item in SessionObject.LstCentre)
                {
                    ListExploitation.Add(item);
                }
                LoadCampagne();
            }
            //else
            //{
            //    Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient service = new Galatee.Silverlight.ServiceRecouvrement.RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
            //    int handler = LoadingManager.BeginLoading("Recuperation des données ...");
            //    service.GetExploitationByUOAsync(new CsREFUNITEORGANISATIONNELLE());
            //    service.GetExploitationByUOCompleted += (er, res) =>
            //    {
            //        try
            //        {
            //            if (res.Error != null || res.Cancelled)
            //                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
            //            else
            //                if (res.Result != null)
            //                {
            //                    SessionObject.LstCentre = res.Result;
            //                    ListExploitation.Clear();
            //                    foreach (var item in SessionObject.LstCentre)
            //                    {
            //                        ListExploitation.Add(item);
            //                    }
            //                    LoadCampagne();
            //                }
            //                else
            //                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
            //                        "Erreur");

            //            LoadingManager.EndLoading(handler);
            //        }
            //        catch (Exception)
            //        {

            //            throw;
            //        }
            //    };
            //}

        }


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
                                Conteneur_listeCampagne = res.Result;
                                Load_Campagne();
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

        private void Load_Campagne()
        {
            listeCampagne.Clear();
            foreach (var item in Conteneur_listeCampagne)
            {
                C_BTA = new Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA();
                string code_centre = item.CodeCentre != null ? item.CodeCentre.ToString() : string.Empty;
                C_BTA.CODEEXPLOITATION = item.fk_idCentre.ToString();
                C_BTA.CAMPAGNE_ID = item.Campagne_ID != null ? item.Campagne_ID : Guid.NewGuid();
                C_BTA.CODECENTRE = code_centre;
                if (code_centre != string.Empty)
                {
                    C_BTA.LIBELLECENTRE = ListExploitation.First(c => Convert.ToInt16(c.CODE) == Convert.ToInt16(code_centre)).LIBELLE != null ? ListExploitation.First(c => Convert.ToInt16(c.CODE) == Convert.ToInt16(code_centre)).LIBELLE : "";
                }
                else
                {
                    C_BTA.LIBELLECENTRE = string.Empty;
                }
                //C_BTA.METHODE = new CsREFMETHODEDEDETECTIONCLIENTSBTA();
                //C_BTA.METHODE.METHODE_ID = item.METHODE != null ? (item.METHODE.METHODE_ID != null ? item.METHODE.METHODE_ID : int.MinValue) : int.MinValue;
                C_BTA.DATECREATION = item.DateCreation != null ? item.DateCreation : DateTime.MinValue;
                C_BTA.DATEDEBUTCONTROLES = item.DateDebutControles != null ? item.DateDebutControles : DateTime.MinValue;
                C_BTA.DATEFINPREVUE = item.DateFinPrevue != null ? item.DateFinPrevue : DateTime.MinValue;
                C_BTA.DATEMODIFICATION = item.DateModification != null ? item.DateModification : DateTime.MinValue;
                C_BTA.LIBELLE_CAMPAGNE = item.Libelle_Campagne != null ? item.Libelle_Campagne : "";
                C_BTA.LIBELLEEXPLOITATION = C_BTA.LIBELLECENTRE;
                C_BTA.MATRICULEAGENTCREATION = item.MatriculeAgentCreation != null ? item.MatriculeAgentCreation : "";
                C_BTA.MATRICULEAGENTDERNIEREMODIFICATION = item.MatriculeAgentDerniereModification != null ? item.MatriculeAgentDerniereModification : "";
                C_BTA.NBREELEMENTS = item.NbreElements != null ? item.NbreElements : 0;
                C_BTA.STATUT = item.Statut_ID == 1 ? " Actif ": " Inactif ";
                C_BTA.STATUT_ID = item.Statut_ID != null ? item.Statut_ID : 0;
                //C_BTA.NBRLOTS = item.NBRLOTS != null ? item.NBRLOTS : 0;
                //C_BTA.POULATIONNONAFFECTES = item.POULATIONNONAFFECTES != null ? item.POULATIONNONAFFECTES : 0;
                //C_BTA.LISTEBRANCHEMENT = new ObservableCollection<CsBrt>();
                //C_BTA.LISTELOT = new ObservableCollection<CsTBLOTDECONTROLEBTA>();
                //if (item.LISTEBRANCHEMENT != null)
                //{
                //    foreach (var item_ in item.LISTEBRANCHEMENT)
                //    {
                //        C_BTA.LISTEBRANCHEMENT.Add(item_);
                //    }
                //}
                //if (item.LISTELOT != null)
                //{
                //    foreach (var item_ in item.LISTELOT)
                //    {
                //        C_BTA.LISTELOT.Add(item_);
                //    }
                //}


                listeCampagne.Add(C_BTA);
                //txtnbrbranch.Text = ListeBrtCampagne.Count().ToString();
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        ContextMenu ctxmn = null;
        MenuItem mnitem;
        private void dg_campagne_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dg_campagne_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (((DataGrid)sender).SelectedItem != null)
            //{
                ctxmn = new ContextMenu();

                mnitem = new MenuItem();
                mnitem.Header = "Consultation";
                ctxmn.Items.Add(mnitem);
                mnitem.Click += new RoutedEventHandler(mnitem_Click);

                mnitem = new MenuItem();
                mnitem.Header = "Modification";
                ctxmn.Items.Add(mnitem);
                mnitem.Click += new RoutedEventHandler(mnitem_Click);

                ctxmn.IsOpen = true;
                ctxmn.HorizontalOffset = e.GetPosition(null).X;
                ctxmn.VerticalOffset = e.GetPosition(null).Y;
            //}
        }

        void mnitem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnitem = (MenuItem)sender;

            switch (mnitem.Header.ToString())
            {
                case "Consultation":
                    new Galatee.Silverlight.Recouvrement.InitiationCampagne((Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA)dgCampagne.SelectedItem,true).Show();
                    break;
                case "Modification":
                    new Galatee.Silverlight.Recouvrement.InitiationCampagne((Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA)dgCampagne.SelectedItem, false).Show();
                    //new ConsultationCampagneControle(false, dgCampagne.SelectedItem).Show();
                    break;
                default:
                    break;
            }
        }

        void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            dgCampagne.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dg_campagne_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Galatee.Silverlight.Recouvrement.SelectionClientBTA FRM = new Galatee.Silverlight.Recouvrement.SelectionClientBTA(this.Conteneur_listeCampagne);
            FRM.MyHandler += FRM_MethodeAbonnee;
            FRM.Show();
            
        }

        private void FRM_MethodeAbonnee(object sender, Rpnt.Helper.BranchementClientEventArgs e)
        {
            LoadCampagne();
        }

       
    }
}

