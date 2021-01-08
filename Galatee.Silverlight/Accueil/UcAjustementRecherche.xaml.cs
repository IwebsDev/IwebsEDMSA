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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil ;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcAjustementRecherche : ChildWindow
    {
        public event EventHandler Closed;
        List<CsLclient> _LeCompteParOreientation = new List<CsLclient>();
        CsLclient LeClientLot = new CsLclient();
        string   Dc;
        public CsLclient   MyClientDetailLot
        {
            get { return LeClientLot; }
            set { LeClientLot = value; } 
        }
        public UcAjustementRecherche()
        {
            InitializeComponent();
        }

        public UcAjustementRecherche(List<CsLclient  >_LeCptParCoper )
        {
            InitializeComponent();
            _LeCompteParOreientation = _LeCptParCoper;

            this.Txt_Centre.MaxLength = SessionObject.Enumere.TailleCentre;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            this.Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                getCloseAfterSelection();
                this.DialogResult = true;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            initctrl();
            this.Txt_Centre.Text = string.IsNullOrEmpty(_LeCompteParOreientation[0].CENTRE) ? string.Empty : _LeCompteParOreientation[0].CENTRE;
            if (!string.IsNullOrEmpty(this.Txt_Centre.Text))
                this.Txt_Centre.IsReadOnly = true;

            this.Txt_Client.Text = string.IsNullOrEmpty(_LeCompteParOreientation[0].CLIENT) ? string.Empty : _LeCompteParOreientation[0].CLIENT;
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.IsReadOnly = true;

            this.Txt_Ordre.Text = string.IsNullOrEmpty(_LeCompteParOreientation[0].ORDRE) ? string.Empty : _LeCompteParOreientation[0].ORDRE;
            if (!string.IsNullOrEmpty(this.Txt_Ordre.Text))
                this.Txt_Ordre.IsReadOnly = true;

            this.Txt_NomClient.Text = string.IsNullOrEmpty(_LeCompteParOreientation.First().NOM) ? string.Empty : _LeCompteParOreientation.First().NOM;

            this.dataGrid1.ItemsSource = null;
            this.dataGrid1.ItemsSource = _LeCompteParOreientation;
        }

        private void btn_Recherche_Click(object sender, RoutedEventArgs e)
        {
           if( !string.IsNullOrEmpty(this.Txt_refem .Text )&& 
               !string.IsNullOrEmpty(this.Txt_Ndoc  .Text ))
           {
               CsLclient   _LafactureRechercher = _LeCompteParOreientation.FirstOrDefault(p => p.NDOC == this.Txt_Ndoc.Text && p.REFEM == this.Txt_refem.Text);
                if (_LafactureRechercher != null )
                {
                 this.Txt_refem .Text = _LafactureRechercher.REFEM ;   
                 this.Txt_Ndoc.Text = _LafactureRechercher.NDOC  ;   
                }
           }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //CsReglement  _LaFacture = (CsReglement)this.dataGrid1.SelectedItem;
            CsLclient _LaFacture = (CsLclient)this.dataGrid1.SelectedItem; 
            this.Txt_refem.Text = _LaFacture.REFEM;
            this.Txt_Ndoc .Text = _LaFacture.NDOC ;
        }
        void getCloseAfterSelection()
        {
            if (Closed != null)
            {
                CsLclient _LaFacture = (CsLclient)this.dataGrid1.SelectedItem;
                LeClientLot = _LaFacture;
                Closed(this, new EventArgs());
            }
        }
        void initctrl()
        {
            this.btn_Recherche.Content = Langue.btn_search;
            this.lbl_NumFacture.Content = Langue.lbl_NumFact;

            this.dataGrid1.Columns[0].Header = Langue.lbl_route;
            this.dataGrid1.Columns[1].Header = Langue.lbl_NumFact ;
            this.dataGrid1.Columns[2].Header = Langue.lbl_coper ;
            this.dataGrid1.Columns[3].Header = Langue.lbl_type;
            this.dataGrid1.Columns[4].Header = Langue.lbl_dateSaisie ;
            this.dataGrid1.Columns[6].Header = Langue.lbl_Montant ;
        }
    }
}

