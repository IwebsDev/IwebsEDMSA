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
    public partial class FrmCreeCaisse : ChildWindow
    {
        public string CaisseSelectionee = string.Empty;
        public string MatriculeSelectionee = string.Empty;
        public string IsOperationCaisse = string.Empty;
        public event EventHandler success;

        public FrmCreeCaisse()
        {
            InitializeComponent();
            this.txt_RaisonDemande.Visibility = System.Windows.Visibility.Collapsed;
            this.lbl_RaisonDemande.Visibility = System.Windows.Visibility.Collapsed;

            this.Cbo_ListeCaisse.ItemsSource = SessionObject.ListeCaisse;
            this.Cbo_ListeCaisse.DisplayMemberPath = "NUMCAISSE";
            this.Cbo_ListeCaisse.SelectedValuePath = "NUMCAISSE";
            IsOperationCaisse = "OUI";
            translate();

        }
        public FrmCreeCaisse(string _IsOperationCaisse)
        {
            InitializeComponent();
            translate();
            if (_IsOperationCaisse == "NON")
            {
                this.txt_RaisonDemande.Visibility = System.Windows.Visibility.Collapsed;
                this.lbl_RaisonDemande.Visibility = System.Windows.Visibility.Collapsed;
            }
                RetourneCaisseHabileCentre();
                IsOperationCaisse = _IsOperationCaisse;
        }
        void translate()
        {
            try
            {
                this.lbl_Caisse.Content = Langue.Caisse;
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
                if(Cbo_ListeCaisse.SelectedItem!=null)
                    if (IsOperationCaisse == "OUI")
                        HabiliterCaisse((CsCaisse)Cbo_ListeCaisse.SelectedItem);
                    else if (IsOperationCaisse == "NON")
                    {
                        frmEtatCaisse ctrl = new frmEtatCaisse((CsHabilitationCaisse)Cbo_ListeCaisse.SelectedItem);
                        ctrl.Show();
                    }
                    else
                    {
                        ValiderDemandeReversement((CsHabilitationCaisse)Cbo_ListeCaisse.SelectedItem);

                    }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Langue.errorTitle);
            }
        }

        private void ValiderDemandeReversement(CsHabilitationCaisse laCaisse)
        {
            try
            {
                CsDemandeReversement laDemande = new CsDemandeReversement() 
                { 
                     FK_IDHABILCAISSE = laCaisse.PK_ID ,
                     USERCREATION = UserConnecte.matricule ,
                     DATECREATION = System.DateTime.Now ,
                     DATEMODIFICATION = System.DateTime.Now ,
                     RAISONDEMANDE = this.txt_RaisonDemande.Text ,
                     STATUT = SessionObject.Enumere.EvenementCree 
                };
                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.MiseAJourDemandeReversementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == true)
                        Message.ShowInformation("Demande initiée", Langue.LibelleModule);
                  
                };
                service.MiseAJourDemandeReversementAsync(laDemande,1);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void RetourneCaisseHabileCentre()
        {
            try
            {
                CsCentre  Centre = new CsCentre 
                {
                 CODE   = SessionObject.LePosteCourant.CODECENTRE ,
                 PK_ID = SessionObject.LePosteCourant.FK_IDCENTRE.Value                    
                };

                Galatee.Silverlight.ServiceCaisse.CaisseServiceClient service = new Galatee.Silverlight.ServiceCaisse.CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
                service.RetourneCaisseHabiliterCentreCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    List<CsHabilitationCaisse> lstCaisse = new List<CsHabilitationCaisse>();
                    if (args.Result != null && args.Result.Count != 0)
                    {
                        lstCaisse = args.Result;
                        lstCaisse.ForEach(t => t.NOMCAISSE = string.Format("{0} ({1})", t.NOMCAISSE, t.MATRICULE));

                      Cbo_ListeCaisse.ItemsSource = null ;
                      Cbo_ListeCaisse.ItemsSource = lstCaisse;
                      Cbo_ListeCaisse.DisplayMemberPath = "NOMCAISSE";
 
                    }
                };
                service.RetourneCaisseHabiliterCentreAsync(Centre );
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
                    CENTRE = csCaisse.CENTRE,
                    DATE_DEBUT = DateTime.Now,
                    FK_IDCENTRE = csCaisse.FK_IDCENTRE,
                    FK_IDCAISSE = csCaisse.PK_ID,
                    MATRICULE = UserConnecte.matricule,
                    NUMCAISSE = csCaisse.NUMCAISSE,
                    POSTE = SessionObject.LePosteCourant.NOMPOSTE,
                    FK_IDCAISSIERE = UserConnecte.PK_ID 
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
          
                    ////this.Cbo_ListeCaisse.ItemsSource = SessionObject.ListeCaisse ;
                    ////this.Cbo_ListeCaisse.DisplayMemberPath = "NUMCAISSE" ;
                    ////this.Cbo_ListeCaisse.SelectedValuePath = "NUMCAISSE";
               
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

