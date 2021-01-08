using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Parametrage;
using Galatee.Silverlight.ServiceParametrage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UcWKFEtape : ChildWindow
    {

        CsEtape ObjetSelectionne;
        public CsEtape Etape { get; set; }
        Guid OperationGUID;
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation; //Par défaut;                
        List<CsFormulaire> _lsFormulaires;
        ObservableCollection<CsEtape> donnesDatagrid = new ObservableCollection<CsEtape>();
        private DataGrid dataGrid = null;

        public UcWKFEtape(Guid OpGUID)
        {
            try
            {
                InitializeComponent();
                OperationGUID = OpGUID;

                Translate();
                GetDefaultData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Configuration Etape");
            }
        }

        public UcWKFEtape(DataGrid pGrid, Guid OpGUID)
        {
            try
            {
                InitializeComponent();
                dataGrid = pGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsEtape>;
                OperationGUID = OpGUID;

                Translate();
                //Chargement des données par défaut pour la création
                GetDefaultData();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKFEtape(CsEtape objEtape, SessionObject.ExecMode execMode, Guid OpGUID)
        {
            try
            {
                InitializeComponent();
                ObjetSelectionne = objEtape;
                _execMode = execMode;
                OperationGUID = OpGUID;
                Translate();
                if (_execMode == SessionObject.ExecMode.Modification || _execMode == SessionObject.ExecMode.Consultation)
                {
                    GetDefaultData();
                }
                ShowDetailsOperation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        #region Fonctions

        private void Translate()
        {
            try
            {
                OKButton.Content = Languages.OK;
                CancelButton.Content = Languages.Annuler;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDefaultData()
        {
            try
            {
                int back = LoadingManager.BeginLoading("Chargement des données par défaut");
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllFormulaireCompleted += (fsender, fargs) =>
                {
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
                    cmbFormulaire.Items.Clear();
                    _lsFormulaires = new List<CsFormulaire>();
                    _lsFormulaires.Add(new CsFormulaire() { PK_ID = 0, FORMULAIRE1 = "Aucun" });
                    if (fargs.Result != null)
                        foreach (var item in fargs.Result.OrderBy(t => t.FORMULAIRE1).ToList())
                        {
                            _lsFormulaires.Add(item);
                        }
                    cmbFormulaire.DisplayMemberPath = "FORMULAIRE1";
                    cmbFormulaire.SelectedValuePath = "PK_ID";
                    cmbFormulaire.ItemsSource = _lsFormulaires.OrderBy(u=>u.FORMULAIRE1 ).ToList();

                    LoadingManager.EndLoading(back);
                };
                client.SelectAllFormulaireAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowDetailsOperation()
        {
            if (null != ObjetSelectionne)
            {
                txtCode.Text = ObjetSelectionne.CODE;
                txtNom.Text = ObjetSelectionne.NOM;
                txtDescription.Text = ObjetSelectionne.DESCRIPTIONETAPE;
                if (ObjetSelectionne.FK_IDFORMULAIRE.HasValue)
                {
                    cmbFormulaire.SelectedValue = ObjetSelectionne.FK_IDFORMULAIRE.Value;
                }
                chkTraitementParLot.IsChecked = ObjetSelectionne.IS_TRAITEMENT_LOT;
            }
            if (_execMode == SessionObject.ExecMode.Consultation)
            {
                txtCode.IsEnabled = false;
                txtNom.IsEnabled = false;
                txtDescription.IsEnabled = false;
                cmbFormulaire.IsEnabled = false;
                cmbFormulaire.IsEnabled = false;
            }
        }


        #endregion


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (null != cmbFormulaire.SelectedValue)
            {
                try
                {
                    ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                    int idFrm = int.Parse(cmbFormulaire.SelectedValue.ToString());
                    bool? checkboxSelected = chkboxConsultationSeulement.IsChecked;
                    int back = LoadingManager.BeginLoading("Enregistrement ...");
                    if (_execMode == SessionObject.ExecMode.Creation)
                    {
                        ObjetSelectionne = new CsEtape()
                        {
                            PK_ID = 0,
                            CODE = txtCode.Text,
                            DESCRIPTIONETAPE = txtDescription.Text,
                            NOM = txtNom.Text,
                            FK_IDOPERATION = OperationGUID,
                            FK_IDFORMULAIRE = idFrm,
                            MODIFICATION = !checkboxSelected,
                            CONTROLEETAPE = (0 != idFrm) ? _lsFormulaires.Where(f => f.PK_ID == idFrm)
                                .First().FULLNAMECONTROLE : string.Empty,
                            IS_TRAITEMENT_LOT = chkTraitementParLot.IsChecked.Value
                        };
                        List<CsEtape> toInsert = new List<CsEtape>() { ObjetSelectionne };
                        client.InsertEtapeCompleted += (ssender, insertR) =>
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
                            Etape = ObjetSelectionne;
                            DialogResult = true;
                            UpdateParentList(toInsert);

                            LoadingManager.EndLoading(back);
                        };
                        client.InsertEtapeAsync(toInsert);
                    }
                    else if (_execMode == SessionObject.ExecMode.Modification)
                    {
                        ObjetSelectionne.CODE = txtCode.Text;
                        ObjetSelectionne.DESCRIPTIONETAPE = txtDescription.Text;
                        ObjetSelectionne.NOM = txtNom.Text;
                        ObjetSelectionne.FK_IDOPERATION = OperationGUID;
                        ObjetSelectionne.FK_IDFORMULAIRE = idFrm;
                        ObjetSelectionne.CONTROLEETAPE = (idFrm != 0) ? _lsFormulaires.Where(f => f.PK_ID == idFrm).First()
                                .FULLNAMECONTROLE : string.Empty;
                        ObjetSelectionne.IS_TRAITEMENT_LOT = chkTraitementParLot.IsChecked.Value;
                        List<CsEtape> toUpdate = new List<CsEtape>() { ObjetSelectionne };
                        client.UpdateEtapeCompleted += (ssender, insertR) =>
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
                            Etape = ObjetSelectionne;
                            DialogResult = true;
                            UpdateParentList(toUpdate);

                            LoadingManager.EndLoading(back);
                        };
                        client.UpdateEtapeAsync(toUpdate);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else Message.ShowError(new Exception("Veuillez selectionner un formulaire"), "Configuration Etape");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void UpdateParentList(List<CsEtape> pListeObjet)
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
                        donnesDatagrid.Remove(item);
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

    }
}

