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
    public partial class UcWKFListeOperations : ChildWindow, INotifyPropertyChanged
    {

        #region Model CsOperation Modifié

        public class m_CsOperation
        {
            public System.Guid PK_ID { get; set; }
            public string CODE { get; set; }
            public string NOM { get; set; }
            public string DESCRIPTION { get; set; }
            public Nullable<System.Guid> FK_ID_PARENTOPERATION { get; set; }
            public Nullable<int> FK_ID_PRODUIT { get; set; }
            public string FORMULAIRE { get; set; }
            public Nullable<int> FK_IDFORMULAIRE { get; set; }
            public string PRODUITNAME { get; set; }
            public List<m_CsOperation> SOUS_OPERATION { get; set; }
            public string CODE_TDEM { get; set; }
        }

        #endregion

        public CsOperation ObjetSelectionne { get; set; }
        ObservableCollection<m_CsOperation> donnesDatagrid = new ObservableCollection<m_CsOperation>();
        ObservableCollection<m_CsOperation> _sousDonnesDatagrid = new ObservableCollection<m_CsOperation>();

        public UcWKFListeOperations()
        {
            try
            {
                InitializeComponent();
                Translate();

                this.DataContext = ObjetSelectionne;

                GetData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.ListeOperation);
            }
        }


        private void Translate()
        {
            try
            {
                dtgrdParametre.Columns[0].Header = Languages.Code;
                dtgrdParametre.Columns[1].Header = Languages.Libelle;
                dtgrdParametre.Columns[2].Header = Languages.Description;
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

        public ObservableCollection<m_CsOperation> DonnesDatagrid
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

        public ObservableCollection<m_CsOperation> SousDonnesDatagrid
        {
            get { return _sousDonnesDatagrid; }
            set
            {
                if (value == _sousDonnesDatagrid)
                    return;
                _sousDonnesDatagrid = value;
                NotifyPropertyChanged("SousDonnesDatagrid");
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
                    DonnesDatagrid.Clear();
                    if (args.Result != null)
                    {
                        List<m_CsOperation> _lesOperations = new List<m_CsOperation>();
                        List<CsOperation> normalOperation = args.Result;
                        foreach (var item in args.Result.Where(i => !i.FK_ID_PARENTOPERATION.HasValue || i.FK_ID_PARENTOPERATION.Value == Guid.Empty))
                        {
                            var Enfants = args.Result.Where(o => o.FK_ID_PARENTOPERATION == item.PK_ID);
                            List<m_CsOperation> mEnfants = new List<m_CsOperation>();
                            Enfants.ToList().ForEach((CsOperation op) =>
                            {
                                mEnfants.Add(new m_CsOperation()
                                {
                                    PK_ID = op.PK_ID,
                                    DESCRIPTION = op.DESCRIPTION,
                                    CODE = op.CODE,
                                    FK_ID_PARENTOPERATION = op.FK_ID_PARENTOPERATION,
                                    FK_ID_PRODUIT = op.FK_ID_PRODUIT,
                                    FK_IDFORMULAIRE = op.FK_IDFORMULAIRE,
                                    PRODUITNAME = op.PRODUITNAME,
                                    FORMULAIRE = op.FORMULAIRE, 
                                    CODE_TDEM = op.CODE_TDEM,
                                    NOM = op.NOM
                                });
                            });
                            _lesOperations.Add(new m_CsOperation()
                            {
                                PK_ID = item.PK_ID,
                                DESCRIPTION = item.DESCRIPTION,
                                CODE = item.CODE,
                                FK_ID_PARENTOPERATION = item.FK_ID_PARENTOPERATION,
                                FK_ID_PRODUIT = item.FK_ID_PRODUIT,
                                FK_IDFORMULAIRE = item.FK_IDFORMULAIRE,
                                PRODUITNAME = item.PRODUITNAME,
                                NOM = item.NOM,
                                FORMULAIRE = item.FORMULAIRE,
                                CODE_TDEM = item.CODE_TDEM,
                                SOUS_OPERATION = mEnfants
                            });
                        }
                        //On les ajoutes aux datagrid
                        foreach (var item in _lesOperations)
                        {
                            donnesDatagrid.Add(item);
                        }
                    }
                    dtgrdParametre.ItemsSource = DonnesDatagrid; LoadingManager.EndLoading(back);
                };
                client.SelectAllOperationAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ActiverOuDesactiverBouton()
        {
            btnAjouter.IsEnabled = BtnModifier.IsEnabled = btnSupprimer.IsEnabled = (null != dtgrdParametre.SelectedItem
                && dtgrdParametre.SelectedItems.Count == 1);
        }

        private void ActiverOuDesactiverBoutonSousOperation()
        {
            btnAjouter_Copy.IsEnabled = BtnModifier_Copy.IsEnabled = btnSupprimer_Copy.IsEnabled = (null != dtgrdParametre2.SelectedItem
                && dtgrdParametre2.SelectedItems.Count == 1);
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
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (m_CsOperation)dtgrdParametre.SelectedItem;
                    //On récupère ses sous opérations
                    var enfants = objetselectionne.SOUS_OPERATION;
                    _sousDonnesDatagrid = new ObservableCollection<m_CsOperation>(enfants);
                    dtgrdParametre2.ItemsSource = SousDonnesDatagrid;
                }

                ActiverOuDesactiverBouton();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }
        private void dtgrdParametre_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ObjetSelectionne = dtgrdParametre.SelectedItem as CsOperation;
                SessionObject.objectSelected = dtgrdParametre.SelectedItem as CsOperation;
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
                            var selected = dtgrdParametre.SelectedItem as m_CsOperation;
                            var _csOp = new CsOperation()
                            {
                                PK_ID = selected.PK_ID,
                                DESCRIPTION = selected.DESCRIPTION,
                                CODE = selected.CODE,
                                FK_ID_PARENTOPERATION = selected.FK_ID_PARENTOPERATION,
                                FK_ID_PRODUIT = selected.FK_ID_PRODUIT,
                                FK_IDFORMULAIRE = selected.FK_IDFORMULAIRE,
                                PRODUITNAME = selected.PRODUITNAME,
                                FORMULAIRE = selected.FORMULAIRE,
                                NOM = selected.NOM
                            };
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteOperationCompleted += (del, argDel) =>
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
                                };
                                delete.DeleteOperationAsync(new List<CsOperation>() { _csOp });
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

        private void SupprimerSousOp()
        {
            try
            {
                if (DonnesDatagrid.Count == 0)
                    throw new Exception(Languages.AucuneDonneeASupprimer);
                if (dtgrdParametre2.SelectedItem != null && dtgrdParametre2.SelectedItems.Count == 1)
                {

                    var w = new MessageBoxControl.MessageBoxChildWindow(Languages.Parametrage, Languages.QuestionSuppressionDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            var selected = dtgrdParametre2.SelectedItem as m_CsOperation;
                            var _csOp = new CsOperation()
                            {
                                PK_ID = selected.PK_ID,
                                DESCRIPTION = selected.DESCRIPTION,
                                CODE = selected.CODE,
                                FK_ID_PARENTOPERATION = selected.FK_ID_PARENTOPERATION,
                                FK_ID_PRODUIT = selected.FK_ID_PRODUIT,
                                FK_IDFORMULAIRE = selected.FK_IDFORMULAIRE,
                                PRODUITNAME = selected.PRODUITNAME,
                                FORMULAIRE = selected.FORMULAIRE,
                                NOM = selected.NOM
                            };
                            if (selected != null)
                            {
                                ParametrageClient delete = new ParametrageClient(Utility.Protocole(),
                                                                                 Utility.EndPoint("Parametrage"));
                                delete.DeleteOperationCompleted += (del, argDel) =>
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
                                    SousDonnesDatagrid.Remove(selected);
                                };
                                delete.DeleteOperationAsync(new List<CsOperation>() { _csOp });
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
                UcWKFOperation form = new UcWKFOperation(dtgrdParametre);
                form.Closed += form_Closed;
                form.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        void form_Closed(object sender, EventArgs e)
        {
            GetData();
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (m_CsOperation)dtgrdParametre.SelectedItem;
                    var _csOp = new CsOperation()
                    {
                        PK_ID = objetselectionne.PK_ID,
                        DESCRIPTION = objetselectionne.DESCRIPTION,
                        CODE = objetselectionne.CODE,
                        FK_ID_PARENTOPERATION = objetselectionne.FK_ID_PARENTOPERATION,
                        FK_ID_PRODUIT = objetselectionne.FK_ID_PRODUIT,
                        FK_IDFORMULAIRE = objetselectionne.FK_IDFORMULAIRE,
                        PRODUITNAME = objetselectionne.PRODUITNAME,
                        FORMULAIRE = objetselectionne.FORMULAIRE,
                        NOM = objetselectionne.NOM,
                        CODE_TDEM =objetselectionne.CODE_TDEM
                    };
                    UcWKFOperation form = new UcWKFOperation(_csOp, SessionObject.ExecMode.Modification, dtgrdParametre);
                    form.Closed += form_Closed;
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

        private void Consulter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre.SelectedItem != null)
                {
                    var objetselectionne = (m_CsOperation)dtgrdParametre.SelectedItem;
                    var _csOp = new CsOperation()
                    {
                        PK_ID = objetselectionne.PK_ID,
                        DESCRIPTION = objetselectionne.DESCRIPTION,
                        CODE = objetselectionne.CODE,
                        FK_ID_PARENTOPERATION = objetselectionne.FK_ID_PARENTOPERATION,
                        FK_ID_PRODUIT = objetselectionne.FK_ID_PRODUIT,
                        FK_IDFORMULAIRE = objetselectionne.FK_IDFORMULAIRE,
                        PRODUITNAME = objetselectionne.PRODUITNAME,
                        FORMULAIRE = objetselectionne.FORMULAIRE,
                        NOM = objetselectionne.NOM,
                        CODE_TDEM = objetselectionne.CODE_TDEM
                    };
                    UcWKFOperation form = new UcWKFOperation(_csOp, SessionObject.ExecMode.Consultation);
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
                ///*MenuContextuelListeSousOp.IsEnabled = */ MenuContextuelConsulter.IsEnabled = MenuContextuelModifier.IsEnabled = MenuContextuelConsulter.IsEnabled = MenuContextuelSupprimer.IsEnabled = dtgrdParametre.SelectedItems.Count > 0;
                //MenuContextuelModifier.UpdateLayout();
                //MenuContextuel.UpdateLayout();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void btnAjouter_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (null != dtgrdParametre.SelectedItem != null && dtgrdParametre.SelectedItems.Count == 1)
                {
                    try
                    {
                        var op = dtgrdParametre.SelectedItem as m_CsOperation;
                        UcWKFOperation form = new UcWKFOperation(op.PK_ID);
                        form.Show();
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Languages.FenetreOperation);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Veuillez selectionner une opération parente", "Opération");
                return;
            }
        }

        private void BtnModifier_Copy_Click(object sender, RoutedEventArgs e)
        {
            //Modification sous opération
            try
            {
                if (dtgrdParametre2.SelectedItem != null)
                {
                    var objetselectionne = (m_CsOperation)dtgrdParametre2.SelectedItem;
                    var _csOp = new CsOperation()
                    {
                        PK_ID = objetselectionne.PK_ID,
                        DESCRIPTION = objetselectionne.DESCRIPTION,
                        CODE = objetselectionne.CODE,
                        FK_ID_PARENTOPERATION = objetselectionne.FK_ID_PARENTOPERATION,
                        FK_ID_PRODUIT = objetselectionne.FK_ID_PRODUIT,
                        FK_IDFORMULAIRE = objetselectionne.FK_IDFORMULAIRE,
                        PRODUITNAME = objetselectionne.PRODUITNAME,
                        FORMULAIRE = objetselectionne.FORMULAIRE,
                        NOM = objetselectionne.NOM
                    };
                    UcWKFOperation form = new UcWKFOperation(_csOp, SessionObject.ExecMode.Modification, dtgrdParametre2);
                    form.Show();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.FenetreOperation);
            }
        }

        private void btnModifier_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtgrdParametre2.SelectedItem != null)
                {
                    SupprimerSousOp();
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Fonction);
            }
        }

        private void dtgrdParametre2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActiverOuDesactiverBoutonSousOperation();
        }

        private void dtgrdParametre2_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        //private void MenuContextuelListeSousOp_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (dtgrdParametre.SelectedItem != null)
        //        {
        //            var objetselectionne = (CsOperation)dtgrdParametre.SelectedItem;
        //            UcWKFListeSousOperations form = new UcWKFListeSousOperations(objetselectionne);
        //            form.Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.Show(ex.Message, Languages.Fonction);
        //    }
        //}

        #endregion


    }
}

