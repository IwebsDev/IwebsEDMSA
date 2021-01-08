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
    public partial class UcListInitialisation : ChildWindow
    {
        private int? etapeDevis = null;
        private string _formeName = null;
        private string NameSpace = "Galatee.Silverlight.Accueil.";
        List<ServiceAccueil.CsDemandeBase> lstDemandeRec;
        public UcListInitialisation()
        {
            InitializeComponent();
        }
       public UcListInitialisation(List<ServiceAccueil.CsDemandeBase> lstDemande)
        {
            InitializeComponent();
            lstDemandeRec = new List<ServiceAccueil.CsDemandeBase>(lstDemande);
            this.dtgListeAccueil.ItemsSource = lstDemande;
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Creer();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private void Creer()
        {

            try
            {
                ChildWindow form = null;
                try
                {
                    _formeName = "UcInitialisation";
                    if (!string.IsNullOrEmpty(_formeName))
                        form = ContextMenuManagement.CreateChildWindow(NameSpace + _formeName, Galatee.Silverlight.Resources.Devis.Languages.ttlCreationDevis, SessionObject.ExecMode.Creation);
                    if (form != null)
                    {
                        form.Closed += new EventHandler(form_Closed);
                        form.Show();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        //private void ModifierDevis()
        //{
        //    ChildWindow form = null;
        //    try
        //    {
        //        if (dtgListeAccueil.SelectedItem != null)
        //        {    
        //           ServiceAccueil.CsDemandeBase l = (ServiceAccueil.CsDemandeBase)dtgListeAccueil.SelectedItem;
        //           ServiceAccueil.CsDemandeBase laDemandeSelect = lstDemandeRec.FirstOrDefault(t => t.NUMDEM == l.NUMDEM);
        //            UcInitialisation ctrl = new UcInitialisation(laDemandeSelect.PK_ID, laDemandeSelect.FK_IDTYPEDEMANDE);
        //            ctrl.Closed += new EventHandler(form_Closed);
        //            ctrl.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(((CsDemandeBase)this.dtgListeAccueil.SelectedItem).PK_ID);
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

   

 

    }
}

