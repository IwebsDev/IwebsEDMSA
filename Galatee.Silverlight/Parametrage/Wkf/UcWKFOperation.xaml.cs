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
    //Ajouté par William CONTAYON 08/07/2015
    public partial class UcWKFOperation : ChildWindow
    {

        CsOperation ObjetSelectionne;
        public CsOperation Operation { get; set; }
        SessionObject.ExecMode _execMode = SessionObject.ExecMode.Creation; //Par défaut;
        List<CsProduit> _lstProduit;
        List<CsOperation> _lstOperationParent;
        Guid _opParentPKID;
        List<CsTdem> _lstTDem;
        List<CsFormulaire> _lsFormulaires;
        ObservableCollection<CsOperation> donnesDatagrid = new ObservableCollection<CsOperation>();
        private DataGrid dataGrid = null;

        public UcWKFOperation()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    //Chargement des données par défaut pour la création
                    GetDefaultData();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKFOperation(Guid opParent)
        {
            try
            {
                InitializeComponent();
                _opParentPKID = opParent;
                Translate();
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    //Chargement des données par défaut pour la création
                    GetDefaultData();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }


        public UcWKFOperation(DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                dataGrid = pGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsOperation>;

                Translate();
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    //Chargement des données par défaut pour la création
                    GetDefaultData();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKFOperation(CsOperation _objCsOp, SessionObject.ExecMode execMode)
        {
            try
            {
                InitializeComponent();
                ObjetSelectionne = _objCsOp;
                _execMode = execMode;
                if (_execMode == SessionObject.ExecMode.Modification || _execMode == SessionObject.ExecMode.Consultation)
                {
                    GetDefaultData();
                    if (_execMode == SessionObject.ExecMode.Consultation) AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                ShowDetailsOperation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }

        public UcWKFOperation(CsOperation _objCsOp, SessionObject.ExecMode execMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                dataGrid = pGrid;
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsOperation>;

                ObjetSelectionne = _objCsOp;
                _execMode = execMode;
                if (_execMode == SessionObject.ExecMode.Modification || _execMode == SessionObject.ExecMode.Consultation)
                {
                    GetDefaultData();
                    if (_execMode == SessionObject.ExecMode.Consultation) AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                ShowDetailsOperation();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.FenetreOperation);
            }
        }


        private void Translate()
        {
            try
            {
                Title = Languages.FenetreOperation;
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
                client.SelectAllProduitCompleted += (ssender, args) =>
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
                    cmbProduit.Items.Clear();
                    _lstProduit = new List<CsProduit>();
                    _lstProduit.Add(new CsProduit() { LIBELLE = "Aucun", PK_ID = 0 });
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            _lstProduit.Add(item);
                        }
                    cmbProduit.DisplayMemberPath = "LIBELLE";
                    cmbProduit.SelectedValuePath = "PK_ID";
                    cmbProduit.ItemsSource = _lstProduit;
                };
                client.SelectAllProduitAsync();

                //Chargement des opérations
                client.SelectAllOperationParentCompleted += (ssender, args) =>
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
                    cmbOperationParent.Items.Clear();
                    _lstOperationParent = new List<CsOperation>();
                    _lstOperationParent.Add(new CsOperation() { NOM = "Aucun", PK_ID = Guid.Empty });
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            _lstOperationParent.Add(item);
                        }
                    cmbOperationParent.DisplayMemberPath = "NOM";
                    cmbOperationParent.SelectedValuePath = "PK_ID";
                    cmbOperationParent.ItemsSource = _lstOperationParent;

                    if (null != _opParentPKID && _opParentPKID != Guid.Empty)
                    {
                        cmbOperationParent.SelectedValue = _opParentPKID;
                    }
                };
                client.SelectAllOperationParentAsync();

                //Chargement des formulaires
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
                    if (fargs.Result != null)
                        foreach (var item in fargs.Result.Where(f => f.CREATIONDEMANDE))
                        {
                            _lsFormulaires.Add(item);
                        }
                    cmbFormulaire.DisplayMemberPath = "FORMULAIRE1";
                    cmbFormulaire.SelectedValuePath = "PK_ID";
                    cmbFormulaire.ItemsSource = _lsFormulaires;
                };
                client.SelectAllFormulaireAsync();

                //Liste des tdem
                client.SelectAllDTEMCompleted += (tsender, targs) =>
                {
                    if (targs.Cancelled || targs.Error != null)
                    {
                        string error = targs.Error.Message;
                        Message.Show(error, Languages.ListeCodePoste);
                        return;
                    }
                    if (targs.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    cmbTDem.Items.Clear();
                    _lstTDem = new List<CsTdem>();
                    if (targs.Result != null)
                        foreach (var item in targs.Result)
                        {
                            _lstTDem.Add(item);
                        }
                    cmbTDem.DisplayMemberPath = "LIBELLE";
                    cmbTDem.SelectedValuePath = "PK_ID";
                    cmbTDem.ItemsSource = _lstTDem;

                    LoadingManager.EndLoading(back);
                };
                client.SelectAllDTEMAsync();
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
                txtDescription.Text = ObjetSelectionne.DESCRIPTION;
                cmbOperationParent.SelectedValue = ObjetSelectionne.FK_ID_PARENTOPERATION;
                cmbProduit.SelectedValue = ObjetSelectionne.FK_ID_PRODUIT;
                if (ObjetSelectionne.FK_IDFORMULAIRE.HasValue)
                {
                    cmbFormulaire.SelectedValue = ObjetSelectionne.FK_IDFORMULAIRE.Value;
                }
                if (string.Empty != ObjetSelectionne.CODE_TDEM)
                {
                    cmbTDem.SelectedValue = ObjetSelectionne.CODE_TDEM;
                }
            }
            if (_execMode == SessionObject.ExecMode.Consultation)
            {
                txtCode.IsEnabled = false;
                txtNom.IsEnabled = false;
                txtDescription.IsEnabled = false;
                cmbOperationParent.IsEnabled = false;
                cmbProduit.IsEnabled = false;
                cmbFormulaire.IsEnabled = false;
                cmbTDem.IsEnabled = false;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                int idFrm = (null != cmbFormulaire.SelectedValue) ? int.Parse(cmbFormulaire.SelectedValue.ToString()) : 0;
                string codeTdem = (null != cmbTDem.SelectedValue) ? cmbTDem.SelectedValue.ToString() : "0";
                int back = LoadingManager.BeginLoading("Enregistrement ...");
                if (_execMode == SessionObject.ExecMode.Creation)
                {
                    ObjetSelectionne = new CsOperation()
                    {
                        PK_ID = Guid.NewGuid(),
                        CODE = txtCode.Text,
                        DESCRIPTION = txtDescription.Text,
                        NOM = txtNom.Text,
                        FK_ID_PARENTOPERATION = (Guid.Parse(cmbOperationParent.SelectedValue.ToString()) != Guid.Empty) ?
                            Guid.Parse(cmbOperationParent.SelectedValue.ToString()) : Guid.Empty,
                        FK_ID_PRODUIT = (int.Parse(cmbProduit.SelectedValue.ToString()) != 0) ?
                            int.Parse(cmbProduit.SelectedValue.ToString()) : 0,
                        FK_IDFORMULAIRE = idFrm,
                        FORMULAIRE = (idFrm != 0) ? _lsFormulaires.Where(f => f.PK_ID == idFrm).First()
                            .FORMULAIRE1 : string.Empty,
                        CODE_TDEM = codeTdem
                    };
                    List<CsOperation> toInsert = new List<CsOperation>() { ObjetSelectionne };
                    client.InsertOperationCompleted += (ssender, insertR) =>
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
                        UpdateParentList(toInsert);

                        LoadingManager.EndLoading(back);
                    };
                    client.InsertOperationAsync(toInsert);
                }
                else if (_execMode == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionne.CODE = txtCode.Text;
                    ObjetSelectionne.DESCRIPTION = txtDescription.Text;
                    ObjetSelectionne.NOM = txtNom.Text;
                    ObjetSelectionne.FK_ID_PARENTOPERATION = (Guid.Parse(cmbOperationParent.SelectedValue.ToString()) != Guid.Empty) ?
                        Guid.Parse(cmbOperationParent.SelectedValue.ToString()) : Guid.Empty;
                    ObjetSelectionne.FK_ID_PRODUIT = (int.Parse(cmbProduit.SelectedValue.ToString()) != 0) ?
                        int.Parse(cmbProduit.SelectedValue.ToString()) : 0;
                    ObjetSelectionne.FK_IDFORMULAIRE = idFrm;
                    ObjetSelectionne.FORMULAIRE = (idFrm != 0) ? _lsFormulaires.Where(f => f.PK_ID == idFrm).First()
                            .FORMULAIRE1 : string.Empty;
                    ObjetSelectionne.CODE_TDEM = codeTdem;
                    List<CsOperation> toUpdate = new List<CsOperation>() { ObjetSelectionne };
                    client.UpdateOperationCompleted += (ssender, insertR) =>
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
                        UpdateParentList(toUpdate);

                        LoadingManager.EndLoading(back);
                    };
                    client.UpdateOperationAsync(toUpdate);
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

        private void UpdateParentList(List<CsOperation> pListeObjet)
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
                //throw ex;
            }
        }

    }
}

