using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
//using Galatee.Silverlight.ServiceExecuterActionWorkflow;
using System.Globalization;
using Galatee.Silverlight.Workflow;
using Galatee.Silverlight.ServiceWorkflow;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Accueil
{
    public partial class FrmReeditonProgramme : ChildWindow
    {

        //List<DemandeWorkflowInformation> LstLesDemandes;
        List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation> LstLesDemandes;
        int TypeEdition = 1;
        bool IsConsultationSeul = false;
        public FrmReeditonProgramme(string TypeEtat)
        {
            try
            {
                InitializeComponent();
                Translate();
                SessionObject.IsChargerDashbord = false;
                prgBar.Visibility = System.Windows.Visibility.Collapsed;
                if (TypeEtat == SessionObject.ReeditionProgramme) TypeEdition = 1;
                else if (TypeEtat == SessionObject.ReeditionSortieCompteur) TypeEdition=2;
                else if (TypeEtat == SessionObject.ReeditionSortieMateriel) TypeEdition = 3;
                ChargeEquipe(UserConnecte.Centre);
                ChargerListDesSite();
                //On cache dabord le bouton
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tableau de bord, Fenetre demande Liste");
            }
        }
        #region Fonctions

        void Translate()
        {
            CancelButton.Content = Languages.Annuler;
        }

        void GetData(List<int> LesCentreHabilite,string NumeroProgramme, DateTime? DateDebut, DateTime? DateFin, string IdEquipe)
        {
            LstLesDemandes = new List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation>();
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            {
                client.ChargerListeProgramReeditionAsync(LesCentreHabilite,NumeroProgramme, DateDebut, DateFin, IdEquipe, TypeEdition);
                client.ChargerListeProgramReeditionCompleted += (sender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }

                    LstLesDemandes = new List<Galatee.Silverlight.ServiceAccueil.CsProgarmmation>();
                    LstLesDemandes = args.Result;
                    dtgrdParametre.ItemsSource = LstLesDemandes;
                    dtgrdParametre.ItemsSource = LstLesDemandes.OrderBy(t => t.DATETRANSMISSION).ToList();

                };
            }
        }
        #endregion
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        void ChargerListDesSite( )
        {
            try
            {

                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    SessionObject.ModuleEnCours = "Accueil";
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    _listeDesCentreExistant = lesCentre;
             

                    //GetData(_listeDesCentreExistant.Select(p => p.PK_ID).ToList(), null, null, null, FKEtape, IsSortieCompteur);
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteAsync(false);
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCentre = args.Result;
                        if (SessionObject.LstCentre.Count != 0)
                        {
                            SessionObject.ModuleEnCours = "Accueil";
                            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            _listeDesCentreExistant = lesCentre;
                            //GetData(_listeDesCentreExistant.Select(p => p.PK_ID).ToList(), null, null, null, null, IsSortieCompteur);

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
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
            this.DialogResult = false;
        }
        #region Gestion DataGrid

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ActiverOuDesactiverBouton();
        }
     
        #endregion
        private void ConsulterButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                if (dtgrdParametre.SelectedItem != null )
                {
                    DemandeWorkflowInformation SelectedObject = (DemandeWorkflowInformation)dtgrdParametre.SelectedItem;
                    Galatee.Silverlight.Devis.UcConsultationDevis detailForm = new Galatee.Silverlight.Devis.UcConsultationDevis(int.Parse(SelectedObject.FK_IDLIGNETABLETRAVAIL));
                    detailForm.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
          
        }
        
        private void EditerButton_Click(object sender, RoutedEventArgs e)
        {
            //ON récupère la demande sélectionnée, 
            try
            {
                //this.EditerButton.IsEnabled = false;
                //this.CancelButton.IsEnabled = false;

                if (this.dtgrdParametre.ItemsSource != null)
                {
                    ServiceAccueil.CsProgarmmation lePgmSelect = ((List<ServiceAccueil.CsProgarmmation>)this.dtgrdParametre.ItemsSource).FirstOrDefault(o => o.IsSelect);
                    if (lePgmSelect != null && lePgmSelect.FK_IDEQUIPE != null)
                    {
                        if (TypeEdition == 1|| 
                            TypeEdition == 2)
                        {
                            Galatee.Silverlight.Devis.UcReeditionPgrammeCompteur ctrl = new Galatee.Silverlight.Devis.UcReeditionPgrammeCompteur(lePgmSelect, TypeEdition);
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                        else  
                        {
                            Galatee.Silverlight.Devis.FrmReeditionSortieMateriel ctrl = new Galatee.Silverlight.Devis.FrmReeditionSortieMateriel(lePgmSelect);
                            ctrl.Closed += ctrl_Closed;
                            ctrl.Show();
                        }
                     }
                 
                }
         
            }
            catch (Exception ex)
            {
                //Message.ShowError("Impossible de traiter la demande. Détails de l'erreur : " + ex.Message, "Traiter une demande");
            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private void dtgrdParametre_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dmdRow = e.Row.DataContext as DemandeWorkflowInformation;
            if (dmdRow != null)
            {
                if (dmdRow.DATEFINTRAITEMENT.HasValue)
                {
                    int delai = ((DateTime)dmdRow.DATEFINTRAITEMENT.Value - DateTime.Today)
                        .Days;
                    if (delai <= 0)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Red);
                        e.Row.Foreground = SolidColorBrush;
                    }
                    return;
                }
               else  if (dmdRow.ESTAFFECTE)
                {
                    if (dmdRow.UTILISATEURAFFECTE == UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Green);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        return;

                    }
                    else if (dmdRow.UTILISATEURAFFECTE != UserConnecte.matricule)
                    {
                        SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.LightGray);
                        e.Row.Foreground = SolidColorBrush;
                        e.Row.FontWeight = FontWeights.Bold;
                        e.Row.IsEnabled = false;
                        return;

                    }
                   
                }
               else  if (dmdRow.FK_IDSTATUS.HasValue && dmdRow.FK_IDSTATUS.Value == (int)WorkflowManager.STATUSDEMANDE.Rejetee)
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Blue);
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Bold;
                }
                else
                {
                    SolidColorBrush SolidColorBrush = new SolidColorBrush(Colors.Black );
                    e.Row.Foreground = SolidColorBrush;
                    e.Row.FontWeight = FontWeights.Normal;

                }
              
            }
        }
        void ucform_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            //if (lesCentre != null && lesCentre.Count != 0)
            //    GetData(lesCentre.Select(t => t.PK_ID).ToList());
        }

        void cw_Closed(object sender, EventArgs e)
        {
            //On recharge les demandes
            //this.IsEnabled = true;
            //this.EditerButton.IsEnabled = true ;
            //this.CancelButton.IsEnabled = true;
            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetreTotal(SessionObject.LstCentre , UserConnecte.listeProfilUser);
             //if (lesCentre != null && lesCentre.Count  !=0)
             //    GetData(lesCentre.Select(k=>k.PK_ID).ToList());
            //GetData();
        }
        DateTime lastClick = DateTime.Now;
        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<ServiceAccueil.CsProgarmmation >;
            if (dg.SelectedItem != null)
            {
                ServiceAccueil.CsProgarmmation SelectedObject = (ServiceAccueil.CsProgarmmation)dg.SelectedItem;
                checkerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(SelectedObject) as CheckBox);
                SelectedObject.IsSelect = true;
                foreach (ServiceAccueil.CsProgarmmation item in (dg.ItemsSource as List<ServiceAccueil.CsProgarmmation>).Where(t => t.NUMPROGRAMME != SelectedObject.NUMPROGRAMME).ToList())
                {
                    if (checkSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox))
                    {
                        DechekerSelectedItem((CheckBox)this.dtgrdParametre.Columns[0].GetCellContent(item) as CheckBox);
                        item.IsSelect = false;
                    }
                }
                if ((DateTime.Now - lastClick).Ticks < 2500000)
                {
                    if (IsConsultationSeul)
                        ConsulterButton_Click(null, null);
                    else
                        EditerButton_Click(null, null);
                }
                lastClick = DateTime.Now;

            }
        }
        bool checkSelectedItem(CheckBox check)
        {
            if (check != null)
            {
                CheckBox chk = check;
                return chk.IsChecked.Value;
            }
            else return false;
        }

        void checkerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = true;
                EditerButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void DechekerSelectedItem(CheckBox check)
        {
            try
            {
                CheckBox chk = check;
                chk.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SessionObject.IsChargerDashbord = true ;
        }
        private void ChargeEquipe(string p)
        {
            try
            {
                List<Galatee.Silverlight.ServiceAccueil.CsGroupe> lstequipe = new List<Galatee.Silverlight.ServiceAccueil.CsGroupe>();
                //Lancer la transaction de Mise à jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeGroupeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstequipe = res.Result;
                    cboEquipe.ItemsSource = lstequipe;
                    cboEquipe.SelectedValuePath = "ID";
                    cboEquipe.DisplayMemberPath = "LIBELLE";

                };
                service1.RetourneListeGroupeAsync(UserConnecte.Centre);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        List<int> lstCentreSiteDist = new List<int>();
        List<int> lstCentreSelect= new List<int>();
        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {

            GetData(_listeDesCentreExistant.Select(p => p.PK_ID).ToList(), this.Txt_NumeroProgramme.Text,
                                                      (this.dtProgramDeb.SelectedDate == null ? null : this.dtProgramDeb.SelectedDate),
                                                      (this.dtProgramFin.SelectedDate == null ? null : this.dtProgramFin.SelectedDate),
                                                      (this.cboEquipe.SelectedItem == null ? null : ((ServiceAccueil.CsGroupe)this.cboEquipe.SelectedItem).ID.ToString()));


        }
    }
}

