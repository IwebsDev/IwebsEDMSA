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
    public partial class UcRedevance : ChildWindow
    {
        List<CsRedevance> listForInsertOrUpdate = null;
        ObservableCollection<CsRedevance> donnesDatagrid = new ObservableCollection<CsRedevance>();
        private CsRedevance ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcRedevance()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }
        public UcRedevance(CsRedevance pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var Redevance = new CsRedevance();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(Redevance, pObject as CsRedevance);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                RemplirListeDesCentreExistant();
                RemplirProduit();
                RemplirNatureClient();
                RemplirLienRedevance();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsRedevance>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Redevance.Text = ObjetSelectionnee.NUMREDEVANCE ?? string.Empty;
                        TxtTranche.Text = ObjetSelectionnee.TRANCHE ?? string.Empty;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ?? string.Empty;
                        TxtParametre1.Text = ObjetSelectionnee.PARAM1 ?? string.Empty;
                        TxtParametre2.Text = ObjetSelectionnee.PARAM2 ?? string.Empty;
                        TxtParametre3.Text = ObjetSelectionnee.PARAM3 ?? string.Empty;
                        TxtParametre4.Text = ObjetSelectionnee.PARAM4 ?? string.Empty;
                        TxtParametre5.Text = ObjetSelectionnee.PARAM5 ?? string.Empty;
                        TxtParametre6.Text = ObjetSelectionnee.PARAM6 ?? string.Empty;
                        ChkEditable.IsChecked = ObjetSelectionnee.EDITEE == "1" ? true : false;
                        ChkExoneree.IsChecked = ObjetSelectionnee.EXONERATION == "1" ? true : false;
                        btnOk.IsEnabled = false;
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }

        private List<CsLibelle> ConstruireListeRedevance()
        {
            List<CsLibelle> ListeRedevance = new List<CsLibelle>();

            CsLibelle RedevanceNormale = new CsLibelle();
            RedevanceNormale.CODE = "0";
            RedevanceNormale.LIBELLE = Languages.RedevanceNormale;
            ListeRedevance.Add(RedevanceNormale);

            CsLibelle RedevanceParametreAutre = new CsLibelle();
            RedevanceParametreAutre.CODE = "1";
            RedevanceParametreAutre.LIBELLE =  Languages.RedevanceParametreAutre;
            ListeRedevance.Add(RedevanceParametreAutre);

            CsLibelle RedevanceAdmettantRedevanceEnParametre = new CsLibelle();
            RedevanceAdmettantRedevanceEnParametre.CODE = "2";
            RedevanceAdmettantRedevanceEnParametre.LIBELLE =  Languages.RedevanceAdmettantRedevanceEnParametre;
            ListeRedevance.Add(RedevanceAdmettantRedevanceEnParametre);

            CsLibelle RedevanceParametreAutreAdmettantRedevanceEnParametre = new CsLibelle();
            RedevanceParametreAutreAdmettantRedevanceEnParametre.CODE = "3";
            RedevanceParametreAutreAdmettantRedevanceEnParametre.LIBELLE =  Languages.RedevanceParametreAutreAdmettantRedevanceEnParametre;
            ListeRedevance.Add(RedevanceParametreAutreAdmettantRedevanceEnParametre);

            return ListeRedevance;
        }


        private void UpdateParentList(CsRedevance pRedevance)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    donnesDatagrid.Add(pRedevance);
                    donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    var Redevance = donnesDatagrid.First(p => p.PK_ID == pRedevance.PK_ID);
                    donnesDatagrid.Remove(Redevance);
                    donnesDatagrid.Add(pRedevance);
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
                //Title = Languages.Redevance;
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

        private List<CsRedevance> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsRedevance>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var redevance = new CsRedevance
                    {
                        PRODUIT = ((CsProduit)CboProduit.SelectedItem).CODE ?? null,
                        CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE ?? null,
                        NATURECLI = ((CsNatureClient)CboNatureClient.SelectedItem).CODE  ?? null,
                        TYPELIEN = ((CsLibelle)CboLienRedevance.SelectedItem).CODE ?? null,
                        PARAM1 = TxtParametre1.Text ?? null,
                        PARAM2 = TxtParametre2.Text ?? null,
                        PARAM3 = TxtParametre3.Text ?? null,
                        PARAM4 = TxtParametre4.Text ?? null,
                        PARAM5 = TxtParametre5.Text ?? null,
                        PARAM6 = TxtParametre6.Text ?? null,
                        LIBELLE = Txt_Libelle.Text ?? null,
                        TRANCHE = TxtTranche.Text ?? null,
                        EXONERATION = Convert.ToBoolean(ChkExoneree.IsChecked) == true ? "1" : "0",
                        EDITEE = Convert.ToBoolean(ChkEditable.IsChecked) == true ? "1" : "0",
                        NUMREDEVANCE = Txt_Redevance.Text,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (donnesDatagrid.FirstOrDefault(p => p.PK_ID == redevance.PK_ID ) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(redevance);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.PRODUIT = ((CsProduit)CboProduit.SelectedItem).CODE ?? null;
                    ObjetSelectionnee.CENTRE = ((CsCentre)CboCentre.SelectedItem).CODE ?? null;
                    ObjetSelectionnee.NATURECLI = ((CsNatureClient)CboNatureClient.SelectedItem).CODE  ?? null;
                    ObjetSelectionnee.TYPELIEN = ((CsLibelle)CboLienRedevance.SelectedItem).CODE ?? null;
                    ObjetSelectionnee.PARAM1 = TxtParametre1.Text ?? null;
                    ObjetSelectionnee.PARAM2 = TxtParametre2.Text ?? null;
                    ObjetSelectionnee.PARAM3 = TxtParametre3.Text ?? null;
                    ObjetSelectionnee.PARAM4 = TxtParametre4.Text ?? null;
                    ObjetSelectionnee.PARAM5 = TxtParametre5.Text ?? null;
                    ObjetSelectionnee.PARAM6 = TxtParametre6.Text ?? null;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text ?? null;
                    ObjetSelectionnee.TRANCHE = TxtTranche.Text ?? null;
                    ObjetSelectionnee.EXONERATION = Convert.ToBoolean(ChkExoneree.IsChecked) == true ? "1" : "0";
                    ObjetSelectionnee.EDITEE = Convert.ToBoolean(ChkEditable.IsChecked) == true ? "1" : "0";
                    ObjetSelectionnee.NUMREDEVANCE = Txt_Redevance.Text ?? string.Empty;
                    ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                    ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Redevance);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Redevance, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertRedevanceCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Redevance);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Redevance);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertRedevanceAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateRedevanceCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Redevance);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Redevance);
                                        return;
                                    }
                                    //UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateRedevanceAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Redevance);
            }
        }

        private void RemplirListeDesCentreExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
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

        private void RemplirProduit()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboProduit.ItemsSource = args.Result;
                        CboProduit.SelectedValuePath = "PK_ID";
                        CboProduit.DisplayMemberPath = "LIBELLE";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsProduit produit in CboProduit.ItemsSource)
                            {
                                if (produit.PK_ID == ObjetSelectionnee.FK_IDPRODUIT)
                                {
                                    CboProduit.SelectedItem = produit;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllProduitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirNatureClient()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                client.SelectAllNatureClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Redevance);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboNatureClient.ItemsSource = args.Result;
                        CboNatureClient.SelectedValuePath = "PK_ID";
                        CboNatureClient.DisplayMemberPath = "LIBELLE";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsNatureClient natureClient in CboNatureClient.ItemsSource)
                            {
                                if (natureClient.CODE  == ObjetSelectionnee.NATURECLI)
                                {
                                    CboNatureClient.SelectedItem = natureClient;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllNatureClientAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirLienRedevance()
        {
            try
            {
                CboLienRedevance.ItemsSource = ConstruireListeRedevance();
                CboLienRedevance.SelectedValuePath = "CODE";
                CboLienRedevance.DisplayMemberPath = "LIBELLE";

                if (ObjetSelectionnee != null)
                {
                    foreach (CsLibelle typeLien in CboLienRedevance.ItemsSource)
                    {
                        if (typeLien.CODE == ObjetSelectionnee.TYPELIEN)
                        {
                            CboLienRedevance.SelectedItem = typeLien;
                            break;
                        }
                    }
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
                if (!string.IsNullOrEmpty(TxtTranche.Text) && !string.IsNullOrEmpty(Txt_Redevance.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && CboCentre.SelectedItem != null && CboProduit.SelectedItem != null)
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
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
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
                Message.ShowError(ex.Message, Languages.Redevance);
            }
        }
    }
}


