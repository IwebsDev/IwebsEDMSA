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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.ServiceCaisse;

namespace Galatee.Silverlight.Shared
{
    public partial class UcConsulClientAccount : ChildWindow
    {
        public UcConsulClientAccount()
        {
            InitializeComponent();
            translate();
        }

        public CsClient selectedClient
        { get; set; }

        public bool isCancelClick
        { get; set; }

        int passage=0;
        public event EventHandler Closed;
        public ChildWindow _w;
        List<CsClient> clients = new List<CsClient>();
        List<CsLclient> Factureclients = new List<CsLclient>();
        public List<CsLclient> FactureclientsSelect = new List< CsLclient>();
        bool IsAjoutClient = false;
        void translate()
        {
            this.dgrdAccount.Columns[0].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Numerofact;
            this.dgrdAccount.Columns[1].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_coper ;
            this.dgrdAccount.Columns[3].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Montant;
            this.dgrdAccount.Columns[4].Header = Galatee.Silverlight.Resources.Accueil.Langue.lbl_Date ;
        }

        public UcConsulClientAccount(List<CsClient> listeClient, ChildWindow w)
        {

            InitializeComponent();
            translate();
            foreach(CsClient cl in listeClient)
                clients.Add(cl);
            _w = w;
        }

        public UcConsulClientAccount(List<CsLclient  > listeFactureClient)
        {
            InitializeComponent();
            translate();
            Factureclients = listeFactureClient;
            IsAjoutClient = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passage > 0)
                {
                    FactureclientsSelect = (dgrdAccount.ItemsSource  as List<CsLclient>).Where(t=>t.Selectionner).ToList();
                    isCancelClick = false;
                    if (Closed != null)
                    {
                        Closed(this, new EventArgs());
                        this.DialogResult = true;
                    }
                }
                passage++;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isCancelClick = true;
                if (Closed != null)
                {
                    Closed(this, new EventArgs());
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dgrdAccount.ItemsSource = Factureclients ;
                updateViewAfterSelection(Factureclients[0]);
            }
            catch (Exception es )
            {
                Message.Show(es, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
            
        }
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsLclient>;
            if (dg.SelectedItem != null)
            {
                CsLclient SelectedObject = (CsLclient)dg.SelectedItem;

                if (SelectedObject.Selectionner == false)
                {
                    SelectedObject.Selectionner = true;
                    SelectedObject.MONTANTPAYE = SelectedObject.SOLDEFACTURE;
                }
                else
                {
                    SelectedObject.Selectionner = false;
                    SelectedObject.MONTANTPAYE = null;
                }
            }
        }
        private void dgrdAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CsLclient clientSelected = (CsLclient)dgrdAccount.SelectedItem;
                updateViewAfterSelection(clientSelected);
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void updateViewAfterSelection(CsLclient clientSelected)
        {
            try
            {
                //lblAccount.Content = clientSelected.NumeroCompte;
                //lblCode.Content = clientSelected.CodeBank;
                //lblLibelle.Content = clientSelected.LibelleBank;
                lblNomabn.Content = clientSelected.NOM ;
                lblRef.Content = clientSelected.CENTRE + " " + clientSelected.CLIENT + " " + clientSelected.ORDRE ;
                //lblCode.Content = clientSelected.CodeBank;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}

