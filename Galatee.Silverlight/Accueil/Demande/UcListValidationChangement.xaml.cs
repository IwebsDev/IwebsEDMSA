using Galatee.Silverlight.Classes;
using Galatee.Silverlight.ServiceAccueil ;
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

namespace Galatee.Silverlight.Accueil
{
    public partial class UcListValidationChangement : ChildWindow
    {
        private int? etapeDevis = null;
        private string _formeName = null;
        private string NameSpace = "Galatee.Silverlight.Accueil.";
        List<ServiceAccueil.CsDemandeBase> lstDemandeRec;
        public UcListValidationChangement()
        {
            InitializeComponent();
        }
        public UcListValidationChangement(List<ServiceAccueil.CsDemandeBase> lstDemande)
        {
            InitializeComponent();
            lstDemandeRec = new List<ServiceAccueil.CsDemandeBase>(lstDemande);
            this.dtgListeAccueil.ItemsSource = lstDemande;
        }
        
        private void form_Closed(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AfficherBouton(bool pValue)
        {
            try
            {
                this.Btn_Consultation.IsEnabled = pValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ActiverControleCourrant()
        {
            try
            {
                if (!this.IsEnabled)
                    this.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void ChildWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.Now - lastClick).Ticks < 2500000)
                ViewButton_Click(null, null);
            lastClick = DateTime.Now;

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.dtgListeAccueil.SelectedItem != null)
            {
                Galatee.Silverlight.Devis.FrmValiderChangementCompteur detailForm = new Galatee.Silverlight.Devis.FrmValiderChangementCompteur(((CsDemandeBase)this.dtgListeAccueil.SelectedItem).PK_ID);
                detailForm.Show();
            }
            else
            {
                Message.ShowInformation("Sélectionner une demande", "Info");
                return;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //ModifierDevis();
        }

        private void dtgListeAccueil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_Fermer_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
        CsDemande laDetailDemande = null;
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            client.GetDemandeByNumIdDemandeAsync(IdDemandeDevis);
            client.GetDemandeByNumIdDemandeCompleted += (ssender, args) =>
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
                    laDetailDemande = args.Result;

                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }

   

 

    }
}

