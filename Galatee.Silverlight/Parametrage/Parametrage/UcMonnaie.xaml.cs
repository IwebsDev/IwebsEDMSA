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
    public partial class UcMonnaie : ChildWindow
    {
        List<CsMonnaie> listForInsertOrUpdate = null;
        ObservableCollection<CsMonnaie> donnesDatagrid = new ObservableCollection<CsMonnaie>();
        private CsMonnaie ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcMonnaie()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }
        public UcMonnaie(CsMonnaie pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var Monnaie = new CsMonnaie();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(Monnaie, pObject as CsMonnaie);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                RemplirListeDesCentreExistant();
                RemplirListeSupport();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsMonnaie>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.VALEUR.Value.ToString("N2") ?? string.Empty;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ?? string.Empty;
                        btnOk.IsEnabled = false;
                        //Txt_Code.IsReadOnly = true;
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot,false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void UpdateParentList(CsMonnaie pMonnaie)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    donnesDatagrid.Add(pMonnaie);
                    donnesDatagrid.OrderBy(p => p.SUPPORT);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    var Monnaie = donnesDatagrid.First(p => p.VALEUR == pMonnaie.VALEUR && p.CENTRE == pMonnaie.CENTRE && p.SUPPORT == pMonnaie.SUPPORT);
                    donnesDatagrid.Remove(Monnaie);
                    donnesDatagrid.Add(pMonnaie);
                    donnesDatagrid.OrderBy(p => p.SUPPORT);
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
                //Title = Languages.Monnaie;
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

        private List<CsMonnaie> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsMonnaie>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var monnaie = new CsMonnaie
                    {
                        VALEUR = decimal.Parse(Txt_Code.Text) ,
                        SUPPORT = ((CsLibelle)CboSupport.SelectedItem).CODE,
                        CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE,
                        LIBELLE = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CENTRE == monnaie.CENTRE && p.SUPPORT == monnaie.SUPPORT && p.VALEUR == monnaie.VALEUR) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(monnaie);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.VALEUR = decimal.Parse(Txt_Code.Text);
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE;
                    ObjetSelectionnee.SUPPORT = ((CsLibelle)CboSupport.SelectedItem).CODE;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
                return null;
            }
        }

        private List<CsLibelle> ConstruireListeSupport()
        {
            List<CsLibelle> ListeSupport = new List<CsLibelle>();

            CsLibelle Piece = new CsLibelle();
            Piece.CODE = "0";
            Piece.LIBELLE = Languages.Piece;
            ListeSupport.Add(Piece);

            CsLibelle Billet = new CsLibelle();
            Billet.CODE = "1";
            Billet.LIBELLE = Languages.Billet;
            ListeSupport.Add(Billet);

            return ListeSupport;
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Monnaie, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertMonnaieCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Monnaie);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Monnaie);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertMonnaieAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateMonnaieCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Monnaie);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Monnaie);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateMonnaieAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Monnaie);
            }
        }

        private void RemplirListeDesCentreExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        this.CboCentre.ItemsSource = args.Result;
                        this.CboCentre.DisplayMemberPath = "LIBELLE";
                        this.CboCentre.SelectedValuePath = "PK_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCentre centre in CboCentre.ItemsSource)
                            {
                                if (centre.PK_ID == ObjetSelectionnee.FK_IDCENTRE)
                                {
                                    CboCentre.SelectedItem = centre;
                                    break;
                                }
                            }
                            CboCentre.IsEnabled = false;
                        }
                    }
                };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeSupport()
        {
            try
            {
                CboSupport.ItemsSource = ConstruireListeSupport();
                CboSupport.SelectedValuePath = "PK_ID";
                CboSupport.DisplayMemberPath = "LIBELLE";

                if (ObjetSelectionnee != null)
                {
                    foreach (CsLibelle Libelle in CboSupport.ItemsSource)
                    {
                        if (Libelle.CODE == ObjetSelectionnee.SUPPORT)
                        {
                            CboSupport.SelectedItem = Libelle;
                            break;
                        }
                    }
                    CboSupport.IsEnabled = false;
                }
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
                    && CboCentre.SelectedItem != null && CboSupport.SelectedItem != null)
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
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
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
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }

        private void OnComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Monnaie);
            }
        }
    }
}


