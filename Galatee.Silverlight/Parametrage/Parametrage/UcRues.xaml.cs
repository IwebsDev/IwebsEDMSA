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
    public partial class UcRues : ChildWindow
    {
        List<CsRues> listForInsertOrUpdate = null;
        ObservableCollection<CsRues> donnesDatagrid = new ObservableCollection<CsRues>();
        private CsRues ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcRues()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }
        public UcRues(CsRues pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var Rue = new CsRues();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(Rue, pObject as CsRues);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                //RemplirListeDesCentreExistant();
                RemplirSecteur();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRues>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ;
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        private void UpdateParentList(CsRues pRue)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    donnesDatagrid.Add(pRue);
                    donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    var Rues = donnesDatagrid.First(p => p.PK_ID == pRue.PK_ID );
                    donnesDatagrid.Remove(Rues);
                    donnesDatagrid.Add(pRue);
                    donnesDatagrid.OrderBy(p => p.PK_ID);
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
                //Title = Languages.Rue;
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

        private List<CsRues> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsRues>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var rue = new CsRues
                    {
                        CODE  = Txt_Code.Text,
                        FK_IDSECTEUR = ((CsSecteur)CboSecture.SelectedItem).PK_ID,
                      //FK_IDCENTRE = ((CsCentre)CboCentre.SelectedItem).PK_ID,
                       
                        LIBELLE  = Txt_Libelle.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE == rue.CODE && p.FK_IDSECTEUR == rue.FK_IDSECTEUR) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(rue);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE  = Txt_Libelle.Text;
                    ObjetSelectionnee.FK_IDSECTEUR = ((CsSecteur)CboSecture.SelectedItem).PK_ID;
                    //ObjetSelectionnee.FK_IDCENTRE = ((CsCentre)CboCentre.SelectedItem).PK_ID;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Rue);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Rue, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertRuesCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Rue);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Rue);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertRuesAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateRuesCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Rue);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Rue);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateRuesAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Rue);
            }
        }

        //private void RemplirListeDesCentreExistant()
        //{
        //    try
        //    {
        //        ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
        //        client.SelectAllCentreCompleted += (ssender, args) =>
        //        {
        //            if (args.Cancelled || args.Error != null)
        //            {
        //                string error = args.Error.Message;
        //                Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //                return;
        //            }
        //            if (args.Result == null)
        //            {
        //                Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                return;
        //            }
        //            //else
        //            //{
        //            //    this.CboCentre.ItemsSource = args.Result;
        //            //    this.CboCentre.DisplayMemberPath = "PK_ID";
        //            //    this.CboCentre.SelectedValuePath = "LIBELLE";

        //            //    if (ObjetSelectionnee != null)
        //            //    {
        //            //        foreach (CsCentre centre in CboCentre.ItemsSource)
        //            //        {
        //            //            if (centre.PK_ID == ObjetSelectionnee.FK_IDCENTRE)
        //            //            {
        //            //                CboCentre.SelectedItem = centre;
        //            //                break;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //        };
        //        client.SelectAllCentreAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        private void RemplirSecteur()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllSecteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Rue);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboSecture.ItemsSource = args.Result;
                        CboSecture.SelectedValuePath = "PK_ID";
                        CboSecture.DisplayMemberPath = "LIBELLE";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsSecteur secteur in CboSecture.ItemsSource)
                            {
                                if (secteur.PK_ID == ObjetSelectionnee.PK_ID)
                                {
                                    CboSecture.SelectedItem = secteur;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllSecteurAsync();
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
                    && CboSecture.SelectedItem != null && CboSecture.SelectedItem != null)
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
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
                Message.ShowError(ex.Message, Languages.Rue);
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
                Message.ShowError(ex.Message, Languages.Rue);
            }
        }
    }
}


