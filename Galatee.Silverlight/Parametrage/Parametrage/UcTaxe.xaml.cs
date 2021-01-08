using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcTaxe : ChildWindow
    {
        List<CsCtax> listForInsertOrUpdate = null;
        ObservableCollection<CsCtax> donnesDatagrid = new ObservableCollection<CsCtax>();
        private CsCtax ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcTaxe()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }
        public UcTaxe(CsCtax  pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var taxe = new CsCtax ();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(taxe , pObject as CsCtax );
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                RemplirListeDesTypeTaxe();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCtax>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        Txt_Taux.Text = ObjetSelectionnee.TAUX.ToString();
                        Dtp_Debut.SelectedDate = ObjetSelectionnee.DEBUTAPPLICATION;
                        Dtp_Fin.SelectedDate = ObjetSelectionnee.FINAPPLICATION ;
                        btnOk.IsEnabled = false;
                        //Txt_Code.IsReadOnly = true;
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot,false);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }
        private void GetDataNew()
        {
            //int back = 0;
            try
            {
                //back = LoadingManager.BeginLoading("Veuillez patienter s'il vous plaît, chargement des données en cours...");
                LayoutRoot.Cursor = Cursors.Wait;
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.GetAllCtaxCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Parametrage);
                        //LoadingManager.EndLoading(back);
                        return;
                    }
                    if (args.Result == null)
                    {
                        LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Commune);
                        //LoadingManager.EndLoading(back);
                        return;
                    }

                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        //ListeCommune = args.Result;
                    foreach (var pCommune in args.Result)
                    {
                        donnesDatagrid.Add(pCommune);
                    }
                    //DonnesDatagrid.OrderBy(p => p.PK_ID);
                    //DonnesDatagrid.Distinct();
                    //dtgrdParametre.ItemsSource = DonnesDatagrid;
                    //LoadingManager.EndLoading(back);
                    LayoutRoot.Cursor = Cursors.Arrow;
                };
                client.GetAllCtaxAsync();
            }
            catch (Exception ex)
            {
                LayoutRoot.Cursor = Cursors.Arrow;
                //LoadingManager.EndLoading(back);
                throw ex;
            }
        }
        private void UpdateParentList(CsCtax pTaxe)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {

                    GetDataNew();
                    //donnesDatagrid.Add(pCommune);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {

                    GetDataNew();
                    //var commune = donnesDatagrid.First(p => p.PK_ID == pCommune.PK_ID);
                    //donnesDatagrid.Remove(commune);
                    //donnesDatagrid.Add(pCommune);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Translate()
        {
            try
            {
                //Title = Languages.Commune;
                //btnOk.Content = Languages.OK;
                //Btn_Reinitialiser.Content = Languages.Annuler;
                //GboCodeDepart.Header = Languages.InformationsCodePoste;
                //lab_Code.Content = Languages.Code;
                //lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsCtax> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCtax>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var taxe = new CsCtax
                    {
                        CODE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        TAUX =Convert.ToDecimal( Txt_Taux.Text ),
                        TYPETAXE = ((CsTypeTaxe)CboTypeTaxe.SelectedItem).CODE,
                        FK_IDTYPETAXE  = ((CsTypeTaxe)CboTypeTaxe.SelectedItem).PK_ID ,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        DEBUTAPPLICATION=Dtp_Debut.SelectedDate.Value ,
                        FINAPPLICATION =  Dtp_Fin.SelectedDate,
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == taxe.CODE && p.CENTRE == taxe.CENTRE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(taxe);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.TAUX = Convert.ToDecimal(Txt_Taux.Text);
                    ObjetSelectionnee.TYPETAXE = ((CsTypeTaxe)CboTypeTaxe.SelectedItem).CODE;
                    ObjetSelectionnee.FK_IDTYPETAXE = ((CsTypeTaxe)CboTypeTaxe.SelectedItem).PK_ID;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    ObjetSelectionnee.DEBUTAPPLICATION= Dtp_Debut.SelectedDate.Value  ;
                    ObjetSelectionnee.FINAPPLICATION = Dtp_Fin.SelectedDate; 
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.UpdateCtaxCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Commune);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Commune);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCtaxAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCtaxCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Commune);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Commune);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCtaxAsync(listForInsertOrUpdate);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Commune);
            }
        }

        private void RemplirListeDesTypeTaxe()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.GetAllTypeTaxeAsync();
                client.GetAllTypeTaxeCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        this.CboTypeTaxe.ItemsSource = args.Result;
                        this.CboTypeTaxe.DisplayMemberPath = "LIBELLE";
                        this.CboTypeTaxe.SelectedValuePath = "PK_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsTypeTaxe ttaxe in CboTypeTaxe.ItemsSource)
                            {
                                if (ttaxe.PK_ID == ObjetSelectionnee.FK_IDTYPETAXE  )
                                {
                                    CboTypeTaxe.SelectedItem = ttaxe;
                                    break;
                                }
                            }
                            VerifierSaisie();

                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                      && !string.IsNullOrEmpty(Txt_Taux.Text) && CboTypeTaxe.SelectedItem != null )
                    btnOk.IsEnabled = true;
                else
                {
                    btnOk.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reinitialiser();
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Txt_Taux.Text = string.Empty;
                btnOk.IsEnabled = false;
                Txt_Code.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void On_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }
    }
}


