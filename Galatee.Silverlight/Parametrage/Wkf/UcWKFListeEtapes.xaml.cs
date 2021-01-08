using Galatee.Silverlight.Classes;
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
    public partial class UcWKFListeEtapes : ChildWindow
    {
        ObservableCollection<CsEtape> donnesDatagrid = new ObservableCollection<CsEtape>();
        ObservableCollection<ObjETAPEWKF> _LesEtapes = new ObservableCollection<ObjETAPEWKF>();
        List<CsOperation> _LsOperations;
        List<CsFormulaire> _LesFormulaires;

        public UcWKFListeEtapes()
        {
            try
            {
                InitializeComponent();

                Translate();
                GetData();

                BtnRechercher.IsEnabled = false;
                ModifierButton.IsEnabled = AjouterButton.IsEnabled = SupprimerButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ListeEtapes);
            }
        }

        private void Translate()
        {
            try
            {
                dtgrdEtape.Columns[0].Header = Languages.Code;
                dtgrdEtape.Columns[1].Header = Languages.ColonneNom;
                dtgrdEtape.Columns[2].Header = Languages.Description;
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

        public ObservableCollection<CsEtape> DonnesDatagrid
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
                client.SelectAllOperationCompleted += (ssender, args) =>
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
                    _LsOperations = new List<CsOperation>();                    
                    cmbOperation.Items.Clear();                    
                    if (args.Result != null)
                    {
                        foreach (var item in args.Result)
                        {
                            _LsOperations.Add(item);                     
                        }                        
                    }
                    cmbOperation.DisplayMemberPath = "NOM";
                    cmbOperation.SelectedValuePath = "PK_ID";
                    cmbOperation.ItemsSource = _LsOperations.Where(o => !o.FK_ID_PARENTOPERATION.HasValue || o.FK_ID_PARENTOPERATION.Value == Guid.Empty);

                    //Chargement de la liste des formulaires
                    client.SelectAllFormulaireCompleted += (fsender, fargs) =>
                        {
                            LoadingManager.EndLoading(back);
                            if (fargs.Cancelled || fargs.Error != null)
                            {
                                string error = fargs.Error.Message;
                                Message.Show(error, Languages.ListeCodePoste);
                                return;
                            }
                            if (fargs.Result == null)
                            {
                                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                                return;
                            }
                            _LesFormulaires = new List<CsFormulaire>();
                            foreach (var item in fargs.Result)
                            {
                                _LesFormulaires.Add(item);
                            }
                        };
                    client.SelectAllFormulaireAsync();
                };
                client.SelectAllOperationAsync();
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
                Message.Show(ex.Message, Languages.ListeEtapes);
            }
        }

        private void dtgrdParametre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModifierButton.IsEnabled = SupprimerButton.IsEnabled = (null != dtgrdEtape.SelectedItem 
                && 1 == dtgrdEtape.SelectedItems.Count);
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    ObjetSelectionne = dtgrdParametre.SelectedItem as CsOperation;
            //    SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsOperation;
            //    SessionObject.gridUtilisateur = dtgrdParametre;
            //}
            //catch (Exception ex)
            //{
            //    Message.Show(ex.Message, Languages.LibelleCodePoste);
            //}
        }




        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null != cmbSousOperation.SelectedValue)
            {
                //chargement des étapes
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                int back = LoadingManager.BeginLoading("Chargement des étapes");
                client.SelectAllEtapesByOperationIdCompleted += (esender, args) =>
                    {
                        LoadingManager.EndLoading(back);
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

                        donnesDatagrid = new ObservableCollection<CsEtape>();
                        _LesEtapes = new ObservableCollection<ObjETAPEWKF>();
                        foreach (CsEtape etape in args.Result.OrderBy(t=>t.NOM ).ToList())
                        {
                            donnesDatagrid.Add(etape);
                            ObjETAPEWKF o = new ObjETAPEWKF()
                            {
                                LEtape =etape                                
                            };
                            var leForm = _LesFormulaires.Where(f => f.PK_ID == etape.FK_IDFORMULAIRE).FirstOrDefault();
                            o.FormulaireAssocie = (null != leForm) ? leForm.FORMULAIRE1 : string.Empty;
                            _LesEtapes.Add(o);
                        }

                        dtgrdEtape.ItemsSource = _LesEtapes.OrderBy(t=>t.LEtape.CODE);
                    };
                client.SelectAllEtapesByOperationIdAsync(Guid.Parse(cmbSousOperation.SelectedValue.ToString()));
            }
            else
            {
                Message.ShowError(new Exception("Veuillez selectionner une opération"), "Liste des étapes d'une opération");
            }
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            //Ajoute d'une étape à l'opération
            if (null != cmbSousOperation.SelectedValue)
            {
                UcWKFEtape ucForm = new UcWKFEtape(Guid.Parse(cmbSousOperation.SelectedValue.ToString()));                
                ucForm.Closing += ucForm_Closing;
                ucForm.Show();
            }
            else
            {
                Message.ShowError("Veuillez selectionnez une opération", "Configuration Etape");
            }
        }

        void ucForm_Closing(object sender, CancelEventArgs e)
        {
            //On recharge le datagrid
            Button_Click(sender, new RoutedEventArgs());
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            if (null != dtgrdEtape.SelectedItem && 1 == dtgrdEtape.SelectedItems.Count)
            {
                ObjETAPEWKF etape = dtgrdEtape.SelectedItem as ObjETAPEWKF;
                if (null != etape)
                {
                    UcWKFEtape ucForm = new UcWKFEtape(etape.LEtape, SessionObject.ExecMode.Modification, Guid.Parse(cmbSousOperation.SelectedValue.ToString()));
                    ucForm.Closing += ucForm_Closing;
                    ucForm.Show();
                }
            }
        }

        private void cmbOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != cmbOperation.SelectedItem)
            {
                cmbSousOperation.IsEnabled = true;
                CsOperation operation = cmbOperation.SelectedItem as CsOperation;
                //Filtre
                if (null != _LsOperations && _LsOperations.Count > 0 && null != operation)
                {
                    cmbSousOperation.DisplayMemberPath = "NOM";
                    cmbSousOperation.SelectedValuePath = "PK_ID";
                    cmbSousOperation.ItemsSource = _LsOperations.Where (o => o.FK_ID_PARENTOPERATION == operation.PK_ID).OrderBy(t=>t.NOM )
                        .ToList();
                }
            }
        }

        private void cmbSousOperation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != cmbSousOperation.SelectedItem)
            {
                BtnRechercher.IsEnabled = true;
                ModifierButton.IsEnabled = AjouterButton.IsEnabled = SupprimerButton.IsEnabled = true;
            }
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {

        }
       
    }

    public class ObjETAPEWKF
    {
        public CsEtape LEtape { get; set; }
        public string FormulaireAssocie { get; set; }
    }
}

