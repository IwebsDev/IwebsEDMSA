using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAdministration;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmListeImport : ChildWindow
    {
        public aImportFichier ObjetSelectionne { get; set; }
        ObservableCollection<aImportFichier> donnesDatagrid = new ObservableCollection<aImportFichier>();
        public FrmListeImport()
        {
            InitializeComponent();
            this.DataContext = ObjetSelectionne;

            //var Namespace = "Galatee.Silverlight.Administration.";
            //var ContextMenuItem = new List<ContextMenuItem>()
            // {
            //    new ContextMenuItem(){ Code=Namespace+"FrmImportFichier",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " d'import" },
            //    new ContextMenuItem(){ Code=Namespace+"FrmImportFichier",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " d'import"  },
            //    new ContextMenuItem(){ Code=Namespace+"FrmImportFichier",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " d'import"  },
                
            // };

            //SessionObject.MenuContextuelItem = ContextMenuItem;
            ChargeGrid();
        }
        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public ObservableCollection<aImportFichier> DonnesDatagrid
        {
            get { return donnesDatagrid; }
            set
            {
                if (value == donnesDatagrid)
                    return;
                donnesDatagrid = value;
                NotifyPropertyChanged("DonnesDatagrid");
            }
        }
        private void ChargeGrid()
        {
            List<aImportFichier> LstImport = new List<aImportFichier>();

            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.GetAllImportCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                LstImport = res.Result;
                if (LstImport != null && LstImport.Count > 0)
                {
                    dgImport.ItemsSource = LstImport;

                }

            };
            service1.GetAllImportAsync();
            service1.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Valider.IsEnabled = false;
            Executer_Click(null, null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgImport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dgImport.SelectedItem as aImportFichier;
                SessionObject.objectSelected = dgImport.SelectedItem as aImportFichier;
                SessionObject.gridUtilisateur = dgImport;
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.PeriodiciteFacturation);
            }
        }


        private void dgImport_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
               // Message.ShowError(ex.Message, Languages.Banque);
            }
        }
        private void Executer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (dgImport.SelectedItem != null)
                {
                    aImportFichier objetselectionne = (aImportFichier)dgImport.SelectedItem;

                    if (!string.IsNullOrWhiteSpace( objetselectionne.FICHIER) && !string.IsNullOrWhiteSpace( objetselectionne.REPERTOIRE))
                    {
                        ImportDepuisFicher(objetselectionne);
                        this.btn_Valider.IsEnabled = true;
                    }
                    else if (!string.IsNullOrEmpty(objetselectionne.PROVIDER) && !string.IsNullOrEmpty(objetselectionne.SERVER) && !string.IsNullOrEmpty(objetselectionne.BASEDEDONNE) && !string.IsNullOrEmpty(objetselectionne.UTILISATEUR) && !string.IsNullOrEmpty(objetselectionne.MOTDEPASSE))
                    {
                        ImportDepuisBaseDeDonnee(objetselectionne);

                    }

            
                }
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        private void ImportDepuisBaseDeDonnee(aImportFichier objetselectionne)
        {
            //List<string> listImport = null;
            List<aImportFichierColonne> list = new List<aImportFichierColonne>();
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.ImportDepuisBaseDeDonneeCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                //listImport = res.Result;
                //if (listImport != null && listImport.Count > 0)
                //{

                //    Message.ShowInformation("Synchronisation réussie", "Paramétrage");
                //}
                //else
                //{
                    //if (listImport == null)
                    //{

                        //Message.ShowInformation("Fichier non trouvé. Vérifier le paramétrage", "Paramétrage");
                    //}
                if (res.Result == false)
                { 
                    Message.ShowInformation("Synchronisation echoué. Aucune nouvelle ligne", "Paramétrage");
                }
                else
                {
                    Message.ShowInformation("Synchronisation réussie", "Paramétrage");
                }
            };
            service1.ImportDepuisBaseDeDonneeAsync(objetselectionne);
            service1.CloseAsync();
        }

        private static void ImportDepuisFicher(aImportFichier objetselectionne)
        {
            
            List<string> listImport = null;
            List<aImportFichierColonne> list = new List<aImportFichierColonne>();
            AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            service1.ExexcImporterFichierCompleted += (sr, res) =>
            {
                
                if (res != null && res.Cancelled)
                    return;
                listImport = res.Result;
                if (listImport != null && listImport.Count > 0)
                {
                    UcResultatSynchroAgent ctrl = new UcResultatSynchroAgent(listImport);
                    ctrl.Show();
                }
                else
                {
                    Message.ShowInformation("Synchronisation echouée", "Paramétrage");

                    //if (listImport == null)
                    //{

                    //    Message.ShowInformation("Fichier non trouvé. Vérifier le paramétrage", "Paramétrage");
                    //}
                    //if (listImport != null && listImport.Count == 0)
                    //    Message.ShowInformation("Synchronisation réussie. Aucune nouvelle ligne", "Paramétrage");
                }

            };
            service1.ExexcImporterFichierAsync(objetselectionne);
            service1.CloseAsync();
        }

        private void dgImport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MenuContextuelSupprimer.IsEnabled = MenuContextuelExecuter.IsEnabled = dgImport.SelectedItems.Count > 0;
        //        //MenuContextuelModifier.UpdateLayout();
        //        MenuContextuel.UpdateLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //       // Message.ShowError(ex.Message, Languages.RechercheTarif);
        //    }
        //}

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgImport.SelectedItems != null)
                {
                    MenuContextuel.IsEnabled = (this.dgImport.SelectedItems.Count == 1);
                }
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.EtapeDevis);
            }
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dgImport.SelectedItem as aImportFichier;
                SessionObject.objectSelected = dgImport.SelectedItem as aImportFichier;
                SessionObject.gridUtilisateur = dgImport;
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FrmImportFichier form = new FrmImportFichier();
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (FrmImportFichier)sender;
                if (form != null && form.DialogResult == true)
                {
                    ChargeGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgImport.SelectedItem != null)
                {
                    var objetselectionne = (aImportFichier)dgImport.SelectedItem;
                    FrmImportFichier form = new FrmImportFichier(objetselectionne, SessionObject.ExecMode.Modification);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        private void Editer_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    Imprimer();
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Languages.RechercheTarif);
            //}
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgImport.SelectedItem != null)
                {
                    var objetselectionne = (aImportFichier)dgImport.SelectedItem;
                    bool result = false;
                    AdministrationServiceClient service1 = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    service1.SuppressImportFichierCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        result = res.Result;
                        if (result == true)
                        {
                            ChargeGrid();
                            MessageBox.Show("Supprimé");
                            
                        }
                        else
                            MessageBox.Show("Echec");

                    };
                    service1.SuppressImportFichierAsync(objetselectionne);
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Echec");
            }
        }

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgImport.SelectedItem != null)
                {
                    var objetselectionne = (aImportFichier)dgImport.SelectedItem;
                    FrmImportFichier form = new FrmImportFichier(objetselectionne, SessionObject.ExecMode.Consultation);
                    form.Closed += form_Closed;
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dgImport.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.RechercheTarif);
            }
        }

        #endregion
    }
}

