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
    public partial class UcWKFListeGroupeValidation : ChildWindow, INotifyPropertyChanged
    {
        public CsGroupeValidation ObjetSelectionne { get; set; }
        ObservableCollection<CsGroupeValidation> donnesDatagrid = new ObservableCollection<CsGroupeValidation>();
        Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> lsDatas;

        public UcWKFListeGroupeValidation()
        {
            try
            {
                InitializeComponent();

                Translate();
                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.ColonneNom;
                dtgrdParametre.Columns[1].Header = Languages.Description;
                dtgrdParametre.Columns[2].Header = Languages.EmailDiffusionGroupe;
                Title = Languages.LibelleCodePoste;
                GroupBox.Header = Languages.ElementDansTable;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public ObservableCollection<CsGroupeValidation> DonnesDatagrid
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

        private void GetData()
        {
            int back = 0;
            try
            {
                back = LoadingManager.BeginLoading("Chargement des données en cours...");
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllGroupeValidationCompleted += (ssender, args) =>
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
                    DonnesDatagrid.Clear();
                    lsDatas = new Dictionary<CsGroupeValidation, List<CsRHabilitationGrouveValidation>>();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            DonnesDatagrid.Add(item.Key);
                            lsDatas.Add(item.Key, item.Value);
                        }
                    dtgrdParametre.ItemsSource = DonnesDatagrid; 
                    LoadingManager.EndLoading(back);
                };
                client.SelectAllGroupeValidationAsync();
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
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsGroupeValidation;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsGroupeValidation;
                SessionObject.gridUtilisateur = dtgrdParametre;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.LibelleCodePoste);
            }
        }


        private void Supprimer()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre.SelectedItem as CsGroupeValidation;
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteGroupeValidationCompleted += (del, argDel) =>
                                {
                                    if (argDel.Cancelled || argDel.Error != null)
                                    {
                                        Message.Show(argDel.Error.Message, Languages.Parametrage);
                                        return;
                                    }
                                    if (argDel.Result == false)
                                    {
                                        Message.Show(Languages.ErreurSuppressionDonnees, Languages.Parametrage);
                                        return;
                                    }
                                    DonnesDatagrid.Remove(selected);
                                    GetData();
                                };
                                delete.DeleteGroupeValidationAsync(new List<CsGroupeValidation>() { selected });
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



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        #region "Gestion MenuContextuel"

        private void Creer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcWKFGroupeValidation form = new UcWKFGroupeValidation(dtgrdParametre);
                form.Show();
                GetData();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsGroupeValidation)dtgrdParametre.SelectedItem;
                    //On recupere le KeyValuePair du grouve de validation
                    KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> d = lsDatas.Where(item => item.Key.PK_ID == objetselectionne.PK_ID).FirstOrDefault();

                    UcWKFGroupeValidation form = new UcWKFGroupeValidation(d, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Show();
                    GetData();
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

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (CsGroupeValidation)dtgrdParametre.SelectedItem;
                    //On recupere le KeyValuePair du grouve de validation
                    KeyValuePair<CsGroupeValidation, List<CsRHabilitationGrouveValidation>> d = lsDatas.Where(item => item.Key.PK_ID == objetselectionne.PK_ID)
                        .FirstOrDefault();
                    UcWKFGroupeValidation form = new UcWKFGroupeValidation(d, SessionObject.ExecMode.Consultation, dtgrdParametre);
                    form.Show();
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
                MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
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
}

