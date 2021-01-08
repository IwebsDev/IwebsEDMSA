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
using Galatee.Silverlight.Resources.Caisse ;
using Galatee.Silverlight.ServiceCaisse;


namespace Galatee.Silverlight.Caisse
{
    public partial class FrmValidationDemandeReversement : ChildWindow
    {
        public string CaisseSelectionee = string.Empty;
        public string MatriculeSelectionee = string.Empty;
        public string IsReversementCaisse = string.Empty;
        public event EventHandler success;

        public FrmValidationDemandeReversement()
        {
            InitializeComponent();
            RetourneDemandeReversementCentre();
            translate();

        }
        public FrmValidationDemandeReversement(string _IsReversementCaisse)
        {
            InitializeComponent();
            translate();
            RetourneDemandeReversementCentre();
            IsReversementCaisse = _IsReversementCaisse;

        }
        void translate()
        {
            try
            {
                this.OKButton.Content = Langue.btn_update;
                this.CancelButton.Content = Langue.Btn_annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }                                                                 
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtg_DemandeValidation.SelectedItem != null)
                        ValiderDemandeReversement((CsDemandeReversement)dtg_DemandeValidation.SelectedItem);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void ValiderDemandeReversement(CsDemandeReversement laDemande)
        {
            try
            {
                laDemande.STATUT = SessionObject.Enumere.EvenementMisAJour;
                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.MiseAJourDemandeReversementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == true)
                        Message.ShowInformation("Demande initiée", Langue.LibelleModule);
                  
                };
                service.MiseAJourDemandeReversementAsync(laDemande,2);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RetourneDemandeReversementCentre()
        {
            try
            {

                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneDemandeReversementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsDemandeReversement> lstCaisse = new List<CsDemandeReversement>();
                    if (args.Result != null && args.Result.Count != 0)
                    {
                        lstCaisse = args.Result;
                        dtg_DemandeValidation.ItemsSource  = null;
                        dtg_DemandeValidation.ItemsSource = lstCaisse;
 
                    }
                };
                service.RetourneDemandeReversementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void HabiliterCaisse(CsCaisse csCaisse)
        {
            try
            {
                CsHabilitationCaisse habilitationCaisse = new CsHabilitationCaisse
                {
                    //CODE = csCaisse.CODE,
                    //DATE_DEBUT = DateTime.Now,
                    //FK_IDCENTRE = csCaisse.FK_IDCENTRE,
                    //FK_NUMCAISSE = csCaisse.PK_ID,
                    //MATRICULE = UserConnecte.matricule,
                    //NUMCAISSE = csCaisse.NUMCAISSE,
                    //POSTE = SessionObject.LePosteCourant.NOMPOSTE,
                    //USERCREATION = UserConnecte.matricule ,
                    //DATECREATION = System.DateTime.Now ,
                    //FK_IDADMUTILISATEUR = UserConnecte.PK_ID 
                };

                //Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                //service.HabiliterCaisseCompleted += (s, args) =>
                //{
                //    if (args != null && args.Cancelled)
                //        return;
                    
                //        if (args.Result != 0)
                //            habilitationCaisse.PK_ID = args.Result;
                //     SessionObject.LaCaisseCourante  = habilitationCaisse;
                //};
                //service.HabiliterCaisseAsync(habilitationCaisse);
                //service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

