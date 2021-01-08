using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.ServiceCaisse;

namespace Galatee.Silverlight.Shared
{
    public partial class UcRechercheParReference : UserControl
    {
        public CsClient selectedClient
            { get; set; }

        public ChildWindow _child
        { get;set ;}

        int compare;//=string.Empty;
        int _rang;
        List<int> lstidCentre = new List<int>();
        public List<CsLclient> listFactureSelectionne = new List<CsLclient>();
        public UcRechercheParReference()
        {
            InitializeComponent();
            ChargerListDesSite();
        }

        public UcRechercheParReference(int rang,List<int> idLstCentre, ChildWindow w)
        {
            InitializeComponent();
            ChargerListDesSite();
            lstidCentre = idLstCentre;
            _rang = rang;
            _child = w;
        }
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<int?> lesIdCentre = new List<int?>();
        void ChargerListDesSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        lesIdCentre.Add(item.PK_ID); 
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                            foreach (ServiceAccueil.CsCentre item in lesCentre)
                                lesIdCentre.Add(item.PK_ID);
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
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox1.ItemsSource =SessionObject.LoadComboBoxData();
            comboBox1.SelectedItem = comboBox1.Items[0]; 

            //choice datagrid 

            if (_rang == 1 || _rang == 3)
            {
                dgrdroute.Visibility = System.Windows.Visibility.Visible;
                dgrdroute.Columns[0].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_center;
                dgrdroute.Columns[1].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_client ;
                dgrdroute.Columns[2].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Ordre ;
                dgrdroute.Columns[3].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;


                dgrdamount.Visibility = System.Windows.Visibility.Collapsed;
                if (_rang == 1)
                    this.label1.Content = Galatee.Silverlight.Resources.Langue.lblReference;
                else
                    this.label1.Content = Galatee.Silverlight.Resources.Langue.lbl_Nom;
            }
            else
                if (_rang == 2)
                {
                    dgrdroute.Visibility = System.Windows.Visibility.Visible;
                    this.label1.Content = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Montant ;

                    dgrdroute.Columns[0].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_center;
                    dgrdroute.Columns[1].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_client;
                    dgrdroute.Columns[2].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Ordre;
                    dgrdroute.Columns[3].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;

                    dgrdamount.Columns[5].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_coper;
                    dgrdamount.Columns[6].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Montant ;
                }

        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
           compare = (comboBox1.Items[0] == comboBox1.SelectedItem ? (int)SessionObject.sens.egal : (comboBox1.Items[1] == comboBox1.SelectedItem ? (int)SessionObject.sens.sup : (int)SessionObject.sens.inf));
           // continue code
        }

        private void dgrdroute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClient = dgrdroute.SelectedItem != null ? (CsClient)dgrdroute.SelectedItem : null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreference.Text))
            {
                btnSearch.IsEnabled = false;
                if (_rang == 1)
                {
                    CaisseServiceClient client = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                    client.RetourneClientsParReferenceCompleted += (send, args) =>
                        {
                            if (args.Cancelled || args.Error != null)
                            {
                                Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
                                btnSearch.IsEnabled = true;
                                return;
                            }

                            if (args.Result.Count != 0 && args.Result != null)
                            {
                                List<CsClient> lesClientsAfficher = new List<CsClient>();
                                lesClientsAfficher = args.Result.Where(t => lesIdCentre.Contains(t.FK_IDCENTRE)).ToList();
                                if (lesClientsAfficher.Count == 0)
                                {
                                    Message.ShowInformation("Ce client n'est pas de votre périmetre d'action", Galatee.Silverlight.Resources.Langue.errorTitle);
                                    return;
                                }

                                dgrdroute.ItemsSource = null;
                                PagedCollectionView view = new PagedCollectionView(lesClientsAfficher);
                                dgrdroute.ItemsSource = view;
                                datapager.Source = view;
                                btnSearch.IsEnabled = true;

                            }
                            else
                            {
                                Message.ShowInformation ("Aucune donnée trouvé", Galatee.Silverlight.Resources.Langue.errorTitle);
                                btnSearch.IsEnabled = true;
                            }
                        };
                    client.RetourneClientsParReferenceAsync(txtreference.Text, compare.ToString());
                }
                else
                    if (_rang == 2)
                    {
                        CaisseServiceClient Serviceclient = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                        Serviceclient.RetourneClientsParAmountCompleted += (s, result) =>
                            {

                                try
                                {
                                    if (result.Cancelled || result.Error != null)
                                    {
                                        Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);

                                        btnSearch.IsEnabled = true;
                                        return;
                                    }
                                    if (result.Result.Count != 0 && result.Result != null)
                                    {
                                        dgrdamount.ItemsSource = null;
                                        List<CsClient> lesClientsAfficher = new List<CsClient>();
                                        lesClientsAfficher = result.Result.Where(t => lesIdCentre.Contains(t.FK_IDCENTRE)).ToList();
                                        if (lesClientsAfficher.Count == 0)
                                        {
                                            Message.ShowInformation("Ce client n'est pas de votre périmetre d'action", Galatee.Silverlight.Resources.Langue.errorTitle);
                                            return;
                                        }
                                        var groupementdeclient = from x in lesClientsAfficher
                                                                 group x by new  {x.CENTRE, x.REFCLIENT , x.ORDRE,x.NOMABON  } into g
                                                                 select g;
                                        List<CsClient> ListClient = (from cl in groupementdeclient
                                                                     select new CsClient { CENTRE = cl.Key.CENTRE, REFCLIENT = cl.Key.REFCLIENT, ORDRE = cl.Key.ORDRE,NOMABON= cl.Key.NOMABON }).ToList();

                                        PagedCollectionView view = new PagedCollectionView(ListClient);
                                        dgrdroute.ItemsSource = view;
                                        datapager.Source = view;
                                    }
                                    else
                                        Message.ShowInformation("Aucune donnée trouvé", Galatee.Silverlight.Resources.Langue.errorTitle);

                                    btnSearch.IsEnabled = true;
                                }
                                catch (Exception ex )
                                {
                                 Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
                                }
                            };
                        Serviceclient.RetourneClientsParAmountAsync(Convert.ToDecimal(txtreference.Text), compare.ToString(), lstidCentre);
                    }
                    else
                        if (_rang == 3)
                          {
          
                        CaisseServiceClient Service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                        Service.RetourneClientsParNomsCompleted += (s, result) =>
                        {
                            try
                            {

                                if (result.Cancelled || result.Error != null)
                                {
                                    Message.ShowError(Galatee.Silverlight.Resources.Langue.wcf_error,Galatee.Silverlight.Resources.Langue.errorTitle);
                                    btnSearch.IsEnabled = true;
                                    return;
                                }
                                if (result.Result.Count != 0 && result.Result != null)
                                {

                                    List<CsClient> lesClientsAfficher = new List<CsClient>();
                                    lesClientsAfficher = result.Result.Where(t => lesIdCentre.Contains(t.FK_IDCENTRE)).ToList();
                                    if (lesClientsAfficher.Count == 0)
                                    {
                                        Message.ShowInformation("Ce client n'est pas de votre périmetre d'action", Galatee.Silverlight.Resources.Langue.errorTitle);
                                        return;
                                    }
                                    dgrdroute.ItemsSource = null;
                                    PagedCollectionView view = new PagedCollectionView(lesClientsAfficher);
                                    dgrdroute.ItemsSource = view;
                                    datapager.Source = view;
                                }
                                else
                                    Message.ShowInformation("Aucune donnée trouvé", Galatee.Silverlight.Resources.Langue.errorTitle);

                                btnSearch.IsEnabled = true;
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        };
                        Service.RetourneClientsParNomsAsync(txtreference.Text);
                    }

            }
            else
                Message.ShowInformation("there is no criteria for search ",Galatee.Silverlight.Resources.Langue.errorTitle);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtreference.Text))
                return;      
      
            if (dgrdamount.Visibility == System.Windows.Visibility.Visible)
                selectedClient = (CsClient)dgrdamount.SelectedItem;
            else
                if (dgrdroute.Visibility == System.Windows.Visibility.Visible)
                    selectedClient = (CsClient)dgrdroute.SelectedItem;

            _child.DialogResult = true;
            
        }

        private void dgrdamount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedClient = dgrdamount.SelectedItem != null ? (CsClient)dgrdamount.SelectedItem : null;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
             
      
            if (selectedClient != null)
            {
                btnView.IsEnabled = false;
                CaisseServiceClient Service = new CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                Service.RetourneListeFactureNonSoldeCompleted += (s, results) =>
                {
                    if (results.Cancelled || results.Error != null)
                    {
                        MessageBox.Show("error d'invocation du service");
                        btnView.IsEnabled = true;
                        return;
                    }

                    if (results.Result == null)
                    {
                        MessageBox.Show("no data match for those criteria");
                        btnView.IsEnabled = true;
                        return;
                    }
                    else
                    {
                        if (results.Result.Count > 0)
                        {
                            List<CsLclient > _lstFact = results.Result;
                            //if (_rang == 2)
                            //{
                            //    if (this.comboBox1.SelectedItem.ToString() == "=")
                            //        _lstFact = _lstFact.Where(p => p.MONTANT == Convert.ToDecimal(this.txtreference.Text)).ToList();
                            //    else if (this.comboBox1.SelectedItem.ToString() == ">=")
                            //        _lstFact = _lstFact.Where(p => p.MONTANT >= Convert.ToDecimal(this.txtreference.Text)).ToList();
                            //    else if (this.comboBox1.SelectedItem.ToString() == "<=")
                            //        _lstFact = _lstFact.Where(p => p.MONTANT <= Convert.ToDecimal(this.txtreference.Text)).ToList();
                            //}

                            _lstFact.ForEach(p => p.REFEMNDOC  = p.REFEM + "-" + p.NDOC);
                            _lstFact.ForEach(p => p.NOM = selectedClient.NOMABON );
                            List<CsClient> clients = new List<CsClient>();
                            UcConsulClientAccount consult = new UcConsulClientAccount(_lstFact);
                            consult.Closed += new EventHandler(UcConsulClientAccountClosed);
                            consult.Show();
                        }
                        else
                            Message.ShowInformation("Ce Client n'a pas de facture", "information");
                        btnView.IsEnabled = true;
                    }
                };
                Service.RetourneListeFactureNonSoldeAsync(selectedClient);
            }
            else
                MessageBox.Show("No Item selected in the Grid");

            //passage++;
        }

    
        void UcConsulClientAccountClosed(object sender, EventArgs e)
        {
            txtreference.Focus();
            UcConsulClientAccount ctrs = sender as UcConsulClientAccount;
            if (!ctrs.isCancelClick)
            {
                listFactureSelectionne = ctrs.FactureclientsSelect;
                _child.DialogResult = true;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _child.DialogResult = true;
        }

        private void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                btnOk_Click(null, null);
            }
        }

    }
}
