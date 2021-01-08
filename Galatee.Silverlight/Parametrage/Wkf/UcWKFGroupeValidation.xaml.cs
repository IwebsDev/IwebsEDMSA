using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
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
    //WCO le 10/07/2015
    public partial class UcWKFGroupeValidation : ChildWindow, INotifyPropertyChanged
    {
        ObservableCollection<CsGroupeValidation> donnesDatagrid = new ObservableCollection<CsGroupeValidation>();
        ObservableCollection<CustomHabilitation> lsHabilitations = new ObservableCollection<CustomHabilitation>();
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation;
        KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> leGroupe;
        private DataGrid dataGrid = null;
        CustomHabilitation ObjetSelectionne;        
        List<string> LstDesUserExistantDejaDansLeGrpDeValdtion = new List<string>();

        public UcWKFGroupeValidation()
        {
            try
            {
                InitializeComponent();

                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcWKFGroupeValidation(DataGrid dtGrid)
        {
            try
            {
                InitializeComponent();
                dataGrid = dtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsGroupeValidation>;
                
                Translate();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcWKFGroupeValidation(KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lGroupeValidation,
            SessionObject.ExecMode exeMode)
        {
            try
            {
                InitializeComponent();

                Translate();
                leGroupe = lGroupeValidation;

                _execMode = exeMode;

                ShowDataDetail();
                if (_execMode == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }

                //Affichage des détails
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcWKFGroupeValidation(KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lGroupeValidation,
            SessionObject.ExecMode exeMode, DataGrid dtGrid)
        {
            try
            {
                InitializeComponent();
                dataGrid = dtGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsGroupeValidation>;
                
                Translate();
                leGroupe = lGroupeValidation;

                _execMode = exeMode;
                ShowDataDetail();

                if (_execMode == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }

                //Affichage des détails
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }


        public ObservableCollection<CsGroupeValidation> DonnesDataGrid
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

        public ObservableCollection<CustomHabilitation> LesHabilitations
        {
            get { return lsHabilitations; }
            set
            {
                if (value == lsHabilitations)
                    return;
                lsHabilitations = value;
                NotifyPropertyChanged("LesHabilitations");
            }
        }

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion



        private void Translate()
        {
            try
            {                
                dtgrdParametre.Columns[0].Header = Galatee.Silverlight.Resources.Langue.dg_matricule;
                dtgrdParametre.Columns[1].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;
                dtgrdParametre.Columns[2].Header = Galatee.Silverlight.Resources.Langue.dg_loginName;
                dtgrdParametre.Columns[3].Header = Languages.lab_EMail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ShowDataDetail()
        {
            if (null != leGroupe.Key && null != leGroupe.Value)
            {
                txtNom.Text = leGroupe.Key.GROUPENAME;
                txtEmail.Text = leGroupe.Key.EMAILDIFFUSION != null ? leGroupe.Key.EMAILDIFFUSION : string.Empty;
                txtDescription.Text = leGroupe.Key.DESCRIPTION != null ? leGroupe.Key.DESCRIPTION : string.Empty;
                chkAllUserValidation.IsChecked = !leGroupe.Key.UNESEULEVALIDATION;
                chkSpecifique.IsChecked = leGroupe.Key.VALEURSPECIFIQUE == 1;

                lsHabilitations = new ObservableCollection<CustomHabilitation>();
                leGroupe.Value.ForEach((CsRHabilitationGrouveValidation Hab) =>
                {
                    lsHabilitations.Add(new CustomHabilitation()
                    {
                        PK_ID = Hab.PK_ID,
                        DATE_CREATION_HABILITATION = Hab.DATE_CREATION_HABILITATION,
                        DATE_DERNIEREMODIFICATION = Hab.DATE_DERNIEREMODIFICATION,
                        DATE_FIN_VALIDITE = Hab.DATE_FIN_VALIDITE,
                        DATE_HABILITATION = Hab.DATE_HABILITATION,
                        EMAIL = Hab.EMAIL,
                        FK_IDADMUTILISATEUR = Hab.FK_IDADMUTILISATEUR,
                        FK_IDGROUPE_VALIDATION = Hab.FK_IDGROUPE_VALIDATION,
                        MATRICULE_USER_MODIFICATION = Hab.MATRICULE_USER_MODIFICATION,
                        LIBELLE = Hab.LIBELLE,
                        LOGINNAME = Hab.LOGINNAME,
                        MATRICULE_USER_CREATION = Hab.MATRICULE_USER_CREATION,
                        RANG = Hab.RANG,
                        MATRICULE = Hab.LOGINNAME,
                        ESTCONSULTATION = Hab.ESTCONSULTATION.Value
                    });
                });
                //Une fois fais, on set notre datasource
                dtgrdParametre.ItemsSource = lsHabilitations.OrderBy(c => c.RANG);                    
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    CsGroupeValidation grp = new CsGroupeValidation()
                    {
                        PK_ID = Guid.NewGuid(),
                        GROUPENAME = txtNom.Text,
                        DESCRIPTION = txtDescription.Text,
                        EMAILDIFFUSION = txtEmail.Text,
                        UNESEULEVALIDATION = !chkAllUserValidation.IsChecked.Value,
                        VALEURSPECIFIQUE = this.chkSpecifique.IsChecked.Value ? 1 : 0
                    };
                    List<CsRHabilitationGrouveValidation> lsHab = new List<CsRHabilitationGrouveValidation>();
                    var customHabs = dtgrdParametre.ItemsSource as IOrderedEnumerable<CustomHabilitation>;
                    if (null != customHabs)
                    {
                        foreach (var objHab in customHabs)
                        {
                            lsHab.Add(new CsRHabilitationGrouveValidation()
                            {
                                PK_ID = Guid.NewGuid(),
                                FK_IDGROUPE_VALIDATION = grp.PK_ID,
                                DATE_CREATION_HABILITATION = DateTime.Today.Date,
                                DATE_FIN_VALIDITE = null,
                                DATE_HABILITATION = DateTime.Today.Date,
                                EMAIL = objHab.EMAIL,
                                FK_IDADMUTILISATEUR = objHab.FK_IDADMUTILISATEUR,
                                DATE_DERNIEREMODIFICATION = null,
                                LIBELLE = objHab.LIBELLE,
                                LOGINNAME = objHab.LOGINNAME,
                                MATRICULE_USER_CREATION = UserConnecte.matricule,
                                MATRICULE_USER_MODIFICATION = string.Empty,
                                ESTCONSULTATION = objHab.ESTCONSULTATION
                            });
                        }
                    }

                    leGroupe = new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(grp, lsHab);
                    Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> toInsert = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                    toInsert.Add(grp, lsHab);
                    client.InsertGroupeValidationCompleted += (ssender, insertR) =>
                    {
                        if (insertR.Cancelled ||
                                insertR.Error != null)
                        {
                            Message.ShowError(insertR.Error.Message, Languages.FenetreOperation);
                            return;
                        }
                        if (!insertR.Result)
                        {
                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.FenetreOperation);
                            return;
                        }
                        DialogResult = true;
                        UpdateParentList(toInsert.Keys.ToList());
                    };
                    client.InsertGroupeValidationAsync(toInsert);
                }
                else if (_execMode == SessionObject.ExecMode.Modification)
                {
                    CsGroupeValidation grp = new CsGroupeValidation()
                    {
                        PK_ID = leGroupe.Key.PK_ID,
                        GROUPENAME = txtNom.Text,
                        DESCRIPTION = txtDescription.Text,
                        EMAILDIFFUSION = txtEmail.Text,
                        UNESEULEVALIDATION = !chkAllUserValidation.IsChecked.Value,
                        VALEURSPECIFIQUE = this.chkSpecifique.IsChecked.Value ? 1 : 0
                    };

                    //La modification consiste à tout delete et recréer
                    List<CsRHabilitationGrouveValidation> lsHab = new List<CsRHabilitationGrouveValidation>();
                    var customHabs = dtgrdParametre.ItemsSource as IOrderedEnumerable<CustomHabilitation>;
                    foreach (var objHab in customHabs)
                    {
                        lsHab.Add(new CsRHabilitationGrouveValidation()
                        {
                            PK_ID = Guid.NewGuid(),
                            FK_IDGROUPE_VALIDATION = grp.PK_ID,
                            DATE_CREATION_HABILITATION = DateTime.Today.Date,
                            DATE_FIN_VALIDITE = null,
                            DATE_HABILITATION = DateTime.Today.Date,
                            EMAIL = objHab.EMAIL,
                            FK_IDADMUTILISATEUR = objHab.FK_IDADMUTILISATEUR,
                            DATE_DERNIEREMODIFICATION = null,
                            LIBELLE = objHab.LIBELLE,
                            LOGINNAME = objHab.LOGINNAME,
                            MATRICULE_USER_CREATION = UserConnecte.matricule,
                            MATRICULE_USER_MODIFICATION = string.Empty,
                            ESTCONSULTATION=objHab.ESTCONSULTATION
                        });
                    }

                    leGroupe = new KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>(grp, lsHab);
                    Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> toUpdate = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                    toUpdate.Add(grp, lsHab);
                    client.UpdateGroupeValidationCompleted += (ssender, insertR) =>
                    {
                        if (insertR.Cancelled ||
                                insertR.Error != null)
                        {
                            Message.ShowError(insertR.Error.Message, Languages.FenetreOperation);
                            return;
                        }
                        if (!insertR.Result)
                        {
                            Message.ShowError(Languages.ErreurInsertionDonnees, Languages.FenetreOperation);
                            return;
                        }
                        DialogResult = true;
                        UpdateParentList(toUpdate.Keys.ToList());
                    };
                    client.UpdateGroupeValidationAsync(toUpdate);
                }
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

        private void Supprimer()
        {
            try
            {
                if (lsHabilitations.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CustomHabilitation;
                            if (selected != null)
                            {
                                lsHabilitations.Remove(selected);
                                dtgrdParametre.ItemsSource = lsHabilitations.OrderBy(c => c.RANG);                    
                            }
                        }
                        else
                        {
                            return;
                        }
                    };
                    w.Show();
                }
                else
                {
                    throw new Exception(Languages.SelectionnerUnElement);
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Parametrage);
            }
        }

        private void UpdateParentList(List<CsGroupeValidation> pListeObjet)
        {
            try
            {
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    if (pListeObjet != null && pListeObjet.Count > 0)
                        foreach (var item in pListeObjet)
                        {
                            donnesDatagrid.Add(item);
                        }
                }
                else if (_execMode == SessionObject.ExecMode.Modification)
                {
                    foreach (var item in pListeObjet)
                    {
                        var toDelete = donnesDatagrid.Where(g => g.PK_ID == item.PK_ID)
                            .FirstOrDefault();
                        donnesDatagrid.Remove(toDelete);
                        donnesDatagrid.Add(item);
                    }
                }
                donnesDatagrid.OrderBy(p => p.PK_ID);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void dtgrdParametre_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //aTa0 o = lvwResultat.SelectedItem as aTa0;
                string Ucname = string.Empty;
                //    if(int.Parse(o.CODE) > 0 && int.Parse(o.CODE)  < 1000)
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CustomHabilitation;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CustomHabilitation;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }



        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserAgentsPicker FormUserAgentPicker = new UserAgentsPicker();

                FormUserAgentPicker.Closed += new EventHandler(FormUserAgentPicker_Closed);
                FormUserAgentPicker.RecherchePourGroupeValidation = true;
                FormUserAgentPicker.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FormUserAgentPicker_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UserAgentsPicker)sender;
                if (form != null)
                {
                    if (form.DialogResult == true && form.AgentsSelectionnes != null && form.AgentsSelectionnes.Count > 0)
                    {
                        var agent = form.AgentsSelectionnes;
                        if (agent != null)
                        {
                            LstDesUserExistantDejaDansLeGrpDeValdtion = new List<string>();
                            foreach (var u in agent)
                            {

                                if (lsHabilitations != null)
                                {
                                    var List_Id_User = lsHabilitations.Select(s => s.FK_IDADMUTILISATEUR);
                                    if (!List_Id_User.Contains(u.PK_ID))
                                    {
                                        int rang = 0;
                                        lsHabilitations.Add(new CustomHabilitation()
                                        {
                                            FK_IDADMUTILISATEUR = u.PK_ID,
                                            LOGINNAME = u.LOGINNAME,
                                            LIBELLE = u.LIBELLE,
                                            EMAIL = u.E_MAIL,
                                            RANG = rang += 1,
                                            MATRICULE = u.MATRICULE,
                                            ESTCONSULTATION = form.UserEnConsultation

                                        });

                                        dtgrdParametre.ItemsSource = lsHabilitations.OrderBy(g => g.RANG);
                                    }
                                    else
                                    {
                                        LstDesUserExistantDejaDansLeGrpDeValdtion.Add(u.LIBELLE + " (" + u.MATRICULE + " )");
                                    }
                                }

                            }

                            if (LstDesUserExistantDejaDansLeGrpDeValdtion.Count > 0)
                            {
                                string info = "les utilisateurs suivants existent déja :\n" + string.Join("\n\t ;", LstDesUserExistantDejaDansLeGrpDeValdtion);
                                Message.Show(info, Languages.Fonction);
                            }

                        }

                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, "Paramétrage");
            }
        }





   /*     private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Appel de la fenetre de sélection des utilisateurs

                List<CustomHabilitation> refList = new List<CustomHabilitation>();
                UcWKFSelectUtilisateur form = new UcWKFSelectUtilisateur(dtgrdParametre);
                form.Show();
                form.Closed += form_Closed;                
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            var lsUsrs = ((UcWKFSelectUtilisateur)sender).LesUtilisateursSelectionnes;
            if (null != lsUsrs)
            {
                LstDesUserExistantDejaDansLeGrpDeValdtion = new List<string>();
                foreach (var u in lsUsrs)
                {
                    
                    var List_Id_User = leGroupe.Value.Select(s => s.FK_IDADMUTILISATEUR);
                    if (!List_Id_User.Contains(u.PK_ID))
                    {
                        int rang = 0;
                        lsHabilitations.Add(new CustomHabilitation()
                        {
                            FK_IDADMUTILISATEUR = u.PK_ID,
                            LOGINNAME = u.LOGINNAME,
                            LIBELLE = u.LIBELLE,
                            EMAIL = u.E_MAIL,
                            RANG = rang += 1,
                            MATRICULE = u.MATRICULE,
                            ESTCONSULTATION = u.ESTCONSULTATION

                        });

                        dtgrdParametre.ItemsSource = lsHabilitations.OrderBy(g => g.RANG);
                    }
                    else
                    {
                        LstDesUserExistantDejaDansLeGrpDeValdtion.Add(u.LIBELLE + " (" + u.MATRICULE + " )");
                    }

                }
                if (LstDesUserExistantDejaDansLeGrpDeValdtion.Count>0)
                {
                    string info="les utilisateur suivant existe déja :\n" +string.Join("\n\t ;", LstDesUserExistantDejaDansLeGrpDeValdtion);
                    Message.Show(info , Languages.Fonction);
                }

            }
        }
    * */

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CustomHabilitation)dtgrdParametre.SelectedItem;
                    UcWKFSetUtilisateurRang form = new UcWKFSetUtilisateurRang(objetselectionne, dtgrdParametre);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    Supprimer();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void MenuContextuel_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuContextuelModifier.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                MenuContextuelModifier.UpdateLayout();
                MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        #endregion

    }


    public class CustomHabilitation
    {
        public System.Guid PK_ID { get; set; }
        public int RANG { get; set; }
        public int FK_IDADMUTILISATEUR { get; set; }
        public string MATRICULE { get; set; }
        public System.Guid FK_IDGROUPE_VALIDATION { get; set; }
        public System.DateTime DATE_HABILITATION { get; set; }
        public Nullable<System.DateTime> DATE_FIN_VALIDITE { get; set; }
        public string MATRICULE_USER_CREATION { get; set; }
        public Nullable<System.DateTime> DATE_DERNIEREMODIFICATION { get; set; }
        public string MATRICULE_USER_MODIFICATION { get; set; }
        public System.DateTime DATE_CREATION_HABILITATION { get; set; }
        public string LOGINNAME { get; set; }
        public string LIBELLE { get; set; }
        public string EMAIL { get; set; }
        public bool  ESTCONSULTATION { get; set; }
    }
}

