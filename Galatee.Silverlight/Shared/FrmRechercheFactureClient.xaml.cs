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
using Galatee.Silverlight.Resources.Recouvrement;

namespace Galatee.Silverlight.Shared
{
    public partial class FrmRechercheFactureClient : ChildWindow
    {
        public FrmRechercheFactureClient()
        {
            InitializeComponent();
            this.Title = Galatee.Silverlight.Resources.Langue.lbl_RechercheClient;
            groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheRefClient;
        }
        bool isAjoutFacture = false;
        List<int> lstidCentre = new List<int>();
        public FrmRechercheFactureClient(bool _isAjoufacture)
        {
            InitializeComponent();
            this.Title = Galatee.Silverlight.Resources.Langue.lbl_RechercheClient;
            groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheRefClient;
            isAjoutFacture = _isAjoufacture;
        }
        public bool isOkClick
        { get; set; }
        UcRechercheParReference _ReferenceClient;
        UcRechercheParNumFacture _billPeriod;
        int _rang;

        public string CustomerRef
        { get; set; }
        public string CustomerName
        { get; set; }
        public List<Galatee.Silverlight.ServiceCaisse.CsLclient> lstFacture = new List<ServiceCaisse.CsLclient>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (routeNumb.IsChecked == true || amount.IsChecked == true || name.IsChecked==true )
            {
                if (amount.IsChecked == true)
                {
                    _rang = 2;
                    content.Header = "";
                    groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheMontant ;

                }
                if (name.IsChecked == true)
                {
                    _rang = 3;
                    content.Header = "";
                    groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheNom  ;
                }
                if (routeNumb.IsChecked == true)
                {
                    _rang = 1;
                    content.Header = "";
                    groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheRefClient ;


                }
                UcRechercheParReference routeNumber = new UcRechercheParReference(_rang, lstidCentre,this);
                _ReferenceClient = routeNumber;
                _billPeriod = null;
                container.Child = _ReferenceClient;
            }

            if (bill.IsChecked == true)
            {
                UcRechercheParNumFacture searchuc = new UcRechercheParNumFacture(this);
                _billPeriod = searchuc;
                _ReferenceClient = null;
                content.Header = "";
                groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheNumFact ;
                container.Child = _billPeriod;
                
                
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _rang = 1;
            UcRechercheParReference routeNumber = new UcRechercheParReference(_rang, lstidCentre,this);
            _ReferenceClient = routeNumber;
            //content.Header = "Search by contract number";
            content.Header = "";
            groupBox1.Header = Galatee.Silverlight.Resources.Langue.gbo_RechercheRefClient;
            container.Child = _ReferenceClient;
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_ReferenceClient != null && _ReferenceClient.selectedClient != null && !isAjoutFacture)
                {
                    CustomerRef = _ReferenceClient.selectedClient.CENTRE + _ReferenceClient.selectedClient.REFCLIENT + _ReferenceClient.selectedClient.ORDRE;
                    CustomerName = _ReferenceClient.selectedClient.NOMABON;
                }
                else if (_ReferenceClient.listFactureSelectionne != null && _ReferenceClient.listFactureSelectionne.Count != 0 && isAjoutFacture)
                    lstFacture = _ReferenceClient.listFactureSelectionne;
            }
            catch (Exception ex )
            {
                string error = ex.Message;
            }
            finally
            {
             //   this.DialogResult = true;
            }
        }
    }
}

