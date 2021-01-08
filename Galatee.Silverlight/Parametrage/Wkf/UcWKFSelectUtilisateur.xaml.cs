using Galatee.Silverlight.ServiceAdministration;
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

namespace Galatee.Silverlight.Parametrage
{
    public partial class UcWKFSelectUtilisateur : ChildWindow, INotifyPropertyChanged
    {
        ObservableCollection<CustomHabilitation> __donnesDatagrid = new ObservableCollection<CustomHabilitation>();
        private DataGrid dataGrid = null;
        List<CsUtilisateur> lsUtilisateursSelected;
        public List<CsUtilisateur> LesUtilisateursSelectionnes
        {
            get { return lsUtilisateursSelected; }
        }

        public UcWKFSelectUtilisateur(DataGrid dtGrid)
        {
            try
            {
                if (dataGrid != null) __donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CustomHabilitation>;
                InitializeComponent();
                GetData();                

                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Translate()
        {
            try
            {
                lvwResultat.Columns[1].Header = Galatee.Silverlight.Resources.Langue.lblCentre;
                lvwResultat.Columns[2].Header = Galatee.Silverlight.Resources.Langue.dg_matricule;
                lvwResultat.Columns[3].Header = Galatee.Silverlight.Resources.Langue.dg_loginName;
                lvwResultat.Columns[4].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;
                lvwResultat.Columns[5].Header = Galatee.Silverlight.Resources.Langue.dg_statuscompte;
                lvwResultat.Columns[6].Header = Galatee.Silverlight.Resources.Langue.dg_jobtitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<CsUtilisateur> donnesDatagrid = new List<CsUtilisateur>();

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        List<CsUtilisateur> _lstUserProfil = new List<CsUtilisateur>();
        void GetData()
        {
            try
            {
                int back = LoadingManager.BeginLoading("Chargement des utilisateurs ...");
                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        LoadingManager.EndLoading(back);
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                        LoadingManager.EndLoading(back);
                        return;
                    }

                    foreach (CsUtilisateur item in res.Result)
                    {
                        item.CENTREAFFICHER = item.CENTRE + "  " + item.LIBELLECENTRE;
                        if (item.PERIMETREACTION == 1)
                            item.LIBELLEPERIMETREACTION = "Centre";
                        else if (item.PERIMETREACTION == 2)
                            item.LIBELLEPERIMETREACTION = "Site";
                        else if (item.PERIMETREACTION == 3)
                            item.LIBELLEPERIMETREACTION = "Globale";

                    }

                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<int> lstCentreHabil = new List<int>();
                    foreach (var item in lstCentreProfil)
                        lstCentreHabil.Add(item.PK_ID);
                    _lstUserProfil = res.Result.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList();

                    donnesDatagrid = _lstUserProfil;
                    lvwResultat.ItemsSource = _lstUserProfil;

                    if (_lstUserProfil != null)
                        lvwResultat.SelectedItem = _lstUserProfil[0];

                    LoadingManager.EndLoading(back);
                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //On selectionne les utilisateurs
            if (null != lvwResultat.SelectedItems && lvwResultat.SelectedItems.Count > 0)
            {
                lsUtilisateursSelected = new List<CsUtilisateur>();
                List<CustomHabilitation> Habils = new List<CustomHabilitation>();
                foreach (var obj in lvwResultat.SelectedItems)
                {
                    CsUtilisateur u = obj as CsUtilisateur;
                    lsUtilisateursSelected.Add(u);
                    lsUtilisateursSelected.ForEach(c => c.ESTCONSULTATION =IsConsultation.IsChecked.Value);

                    Habils.Add(new CustomHabilitation()
                    {
                        FK_IDADMUTILISATEUR = u.PK_ID,
                        LOGINNAME = u.LOGINNAME,
                        LIBELLE = u.LIBELLE,
                        EMAIL = u.E_MAIL,
                        MATRICULE = u.MATRICULE,
                        ESTCONSULTATION = IsConsultation.IsChecked.Value 
                    });
                }

                UpdateParentList(Habils);
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void UpdateParentList(List<CustomHabilitation> pListeObjet)
        {
            try
            {
                if (pListeObjet != null && pListeObjet.Count > 0)
                    foreach (var item in pListeObjet)
                    {
                        __donnesDatagrid.Add(item);
                    }                
                DialogResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Afficher_Click(object sender, RoutedEventArgs e)
        {
            List<CsUtilisateur> lesUser = new List<CsUtilisateur>();
            if (!string.IsNullOrEmpty(this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text))
            {
                lesUser = _lstUserProfil.Where(t => t.MATRICULE == this.txt_Matricule.Text).ToList();
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(_lstUserProfil);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }
            if (!string.IsNullOrEmpty(this.txt_Nom.Text) && string.IsNullOrEmpty(this.txt_Matricule.Text))
            {
                lesUser = _lstUserProfil.Where(t => t.LIBELLE.Contains(this.txt_Nom.Text.ToUpper())).ToList();
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(_lstUserProfil);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }
            if (!string.IsNullOrEmpty(this.txt_Matricule.Text) && !string.IsNullOrEmpty(this.txt_Nom.Text))
            {
                lesUser = _lstUserProfil.Where(t => t.MATRICULE == this.txt_Matricule.Text && t.LIBELLE.Contains(this.txt_Nom.Text.ToUpper())).ToList();
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(_lstUserProfil);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }      
            RafraichirDatagrig(lesUser);

        }
     void RafraichirDatagrig(List<CsUtilisateur> _lstUser)
        {
            lvwResultat.ItemsSource = null ;
            donnesDatagrid = _lstUser;
            lvwResultat.ItemsSource = _lstUser;

            if (_lstUserProfil != null)
                lvwResultat.SelectedItem = _lstUser[0];
        }

     private void txt_Matricule_TextChanged_1(object sender, TextChangedEventArgs e)
     {
         if (string.IsNullOrEmpty(this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text))
             RafraichirDatagrig(_lstUserProfil);
     }

     private void txt_Nom_TextChanged_1(object sender, TextChangedEventArgs e)
     {
         if (string.IsNullOrEmpty(this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text))
             RafraichirDatagrig(_lstUserProfil);
     }

     private void IsConsultation_Checked(object sender, RoutedEventArgs e)
     {

     }

    }

}

