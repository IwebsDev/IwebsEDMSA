using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceParametrage;
using Galatee.Silverlight.Resources.Parametrage;
namespace Galatee.Silverlight.Parametrage
{
    public partial class UcCentre : ChildWindow
    {
        List<CsCentre> listForInsertOrUpdate = null;
        ObservableCollection<CsCentre> donnesDatagrid = new ObservableCollection<CsCentre>();
        private CsCentre ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsProduit> listProduit = new List<CsProduit>();
        List<CsProduit> listProduitCentre = new List<CsProduit>();

        public UcCentre()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }
        public UcCentre(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var centre = new CsCentre();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(centre, pObjects[0] as CsCentre);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCentre>;
                RemplirSite();
                RemplirTypeCentre();
                RemplirListeProduit();
                RemplirNiveautarif();
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.CODE ?? string.Empty;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE ?? string.Empty;
                        Txt_Adresse.Text = ObjetSelectionnee.ADRESSE  ?? string.Empty;
                        btnOk.IsEnabled = false;

                        Cbo_Produit.ItemsSource = null;
                        Cbo_Produit.ItemsSource = ObjetSelectionnee.LESPRODUITSDUSITE;
                        Cbo_Produit.SelectedValuePath  = "LIBELLE";

                        dtg_Produit.ItemsSource = null;
                        dtg_Produit.ItemsSource = ObjetSelectionnee.LESPRODUITSDUSITE;
                    }
                }
                if ((SessionObject.ExecMode) ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    ObjetSelectionnee = new CsCentre();
                    ObjetSelectionnee.LESPRODUITSDUSITE = new List<CsProduit>();
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void RemplirSite()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllSitesCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Centre);
                    }

                    Cbo_Site.Items.Clear();
                    foreach (var item in args.Result)
                    {
                        Cbo_Site.Items.Add(item);
                    }

                    Cbo_Site.SelectedValuePath = "PK_ID";
                    Cbo_Site.DisplayMemberPath = "LIBELLE";

                    if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsSite site in Cbo_Site.Items)
                            {
                                if (site.PK_ID == ObjetSelectionnee.FK_IDCODESITE)
                                {
                                    Cbo_Site.SelectedItem = site;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllSitesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
       private void RemplirNiveautarif()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllNiveauTarifCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Centre);
                    }

                    Cbo_NiveauTarif.Items.Clear();
                    foreach (var item in args.Result)
                    {
                        Cbo_NiveauTarif.Items.Add(item);
                    }

                    Cbo_NiveauTarif.SelectedValuePath = "PK_ID";
                    Cbo_NiveauTarif.DisplayMemberPath = "LIBELLE";

                    if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsNiveauTarif  nivo in Cbo_NiveauTarif.Items)
                            {
                                if (nivo.PK_ID == ObjetSelectionnee.FK_IDNIVEAUTARIF)
                                {
                                    Cbo_NiveauTarif.SelectedItem = nivo;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllNiveauTarifAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirTypeCentre()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllTypeCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Centre);
                    }

                    Cbo_TypeCentre.Items.Clear();
                    foreach (var item in args.Result)
                    {
                        Cbo_TypeCentre.Items.Add(item);
                    }

                    Cbo_TypeCentre.SelectedValuePath = "PK_ID";
                    Cbo_TypeCentre.DisplayMemberPath = "LIBELLE";

                    if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                    {
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsTypeCentre typeCentre in Cbo_TypeCentre.Items)
                            {
                                if (typeCentre.PK_ID == ObjetSelectionnee.FK_IDTYPECENTRE)
                                {
                                    Cbo_TypeCentre.SelectedItem = typeCentre;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllTypeCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeProduit()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitAsync();
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Centre);
                    }
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.ItemsSource = args.Result;
                    Cbo_Produit.SelectedValuePath = "PK_ID";
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    listProduit = args.Result;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCentreCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Centre);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var pCentre in args.Result)
                        {
                            donnesDatagrid.Add(pCentre);
                        }
                    //dtgrdParametre.ItemsSource = DonnesDatagrid;
                };
                client.SelectAllCentreAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsCentre pCentre)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    GetDataNew();
                    //donnesDatagrid.Add(pCentre);
                    //donnesDatagrid.OrderBy(p => p.PK_ID);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    GetDataNew();
                    //var Centre = donnesDatagrid.First(p => p.CO == pCentre.OriginalCodeCentre);
                    //donnesDatagrid.Remove(Centre);
                    //donnesDatagrid.Add(pCentre);
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
                Title = Languages.Centre;
                btnOk.Content = Languages.OK;
                Btn_Annuler.Content = Languages.Annuler;
                GboCentre.Header = Languages.InformationsCentre;
                lab_Centre.Content = Languages.Centre;
                lab_Site.Content = Languages.Site;
                lab_Libelle.Content = Languages.Libelle;
                lab_TypeCentre.Content = Languages.LibelleTypeCentre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsCentre> GetInformationsFromScreen()
        {
            if (Cbo_Site.SelectedItem == null)
            {
                 Message.ShowInformation("Selectionnez le site", "Centre");
                 return null;
            }
            if (Cbo_NiveauTarif.SelectedItem == null)
            {
                Message.ShowInformation("Selectionnez le niveau de tarif", "Centre");
                return null;
            }
            if (Cbo_TypeCentre.SelectedItem == null)
            {
                Message.ShowInformation("Selectionnez le type de centre", "Centre");
                return null;
            }
            if (string.IsNullOrEmpty(this.Txt_Code.Text))
            {
                Message.ShowInformation("Saisir le code centre", "Centre");
                return null;
            }
            if (string.IsNullOrEmpty(this.Txt_Libelle.Text))
            {
                Message.ShowInformation("Saisir le libelle centre", "Centre");
                return null;
            }
            var listObjetForInsertOrUpdate = new List<CsCentre>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Centre = new CsCentre();
                    Centre.CODE = Txt_Code.Text;
                    Centre.LIBELLE = Txt_Libelle.Text;
                    Centre.FK_IDCODESITE = ((CsSite)Cbo_Site.SelectedItem).PK_ID;
                    Centre.FK_IDTYPECENTRE = ((CsTypeCentre)Cbo_TypeCentre.SelectedItem).PK_ID;
                    Centre.TYPECENTRE = ((CsTypeCentre)Cbo_TypeCentre.SelectedItem).CODE;
                    Centre.FK_IDNIVEAUTARIF = ((CsNiveauTarif)Cbo_NiveauTarif.SelectedItem).PK_ID;
                    Centre.CODENIVEAUTARIF = ((CsNiveauTarif)Cbo_NiveauTarif.SelectedItem).CODE;
                    Centre.ADRESSE = Txt_Adresse.Text;
                    if(Cbo_Site.SelectedItem != null)
                    {
                        Centre.LIBELLESITE = ((CsSite) Cbo_Site.SelectedItem).LIBELLE;
                        Centre.CODESITE = ((CsSite)Cbo_Site.SelectedItem).CODE;
                    }
                    if (Cbo_TypeCentre.SelectedItem != null)
                        Centre.LIBELLETYPECENTRE = ((CsTypeCentre)Cbo_TypeCentre.SelectedItem).LIBELLE;
                       
                    Centre.DATECREATION = DateTime.Now;
                    Centre.USERCREATION = UserConnecte.matricule;
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.CODE != null && p.CODESITE != null && p.CODE == Centre.CODE && p.CODESITE == Centre.CODESITE) != null)
                        throw new Exception(Languages.CetElementExisteDeja);
                    List<CsProduit> ListProduit = (List<CsProduit>)this.dtg_Produit.ItemsSource;
                    Centre.LESPRODUITSDUSITE = ListProduit;
                    listObjetForInsertOrUpdate.Add(Centre);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.CODE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLESITE = Txt_Libelle.Text;
                    var selectedItem = (CsSite)Cbo_Site.SelectedItem;
                    if (selectedItem != null)
                        ObjetSelectionnee.FK_IDCODESITE = selectedItem.PK_ID;
                    ObjetSelectionnee.LIBELLESITE = ((CsSite)Cbo_Site.SelectedItem).LIBELLE;
                    ObjetSelectionnee.CODESITE = ((CsSite)Cbo_Site.SelectedItem).CODE;
                    var csTypeCentre = (CsTypeCentre)Cbo_TypeCentre.SelectedItem;
                    if (csTypeCentre != null)
                        ObjetSelectionnee.FK_IDTYPECENTRE = csTypeCentre.PK_ID;
                    ObjetSelectionnee.LIBELLETYPECENTRE = ((CsTypeCentre)Cbo_TypeCentre.SelectedItem).LIBELLE;
                    ObjetSelectionnee.ADRESSE = Txt_Adresse.Text;

                    var nivo = (CsNiveauTarif)Cbo_NiveauTarif.SelectedItem;
                    if (nivo != null)
                    {
                        ObjetSelectionnee.FK_IDNIVEAUTARIF = ((CsNiveauTarif)Cbo_NiveauTarif.SelectedItem).PK_ID;
                        ObjetSelectionnee.CODENIVEAUTARIF = ((CsNiveauTarif)Cbo_NiveauTarif.SelectedItem).CODE;
                    }

                    List<CsProduit> ListProduit = (List<CsProduit>)this.dtg_Produit.ItemsSource;
                    ObjetSelectionnee.LESPRODUITSDUSITE = ListProduit;

                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Centre);
                return null;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Centre, Languages.QuestionEnregistrerDonnees , MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertCentreCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Centre);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.Show(Languages.ErreurInsertionDonnees, Languages.Centre);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertCentreAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCentreCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.Show(UpdateR.Error.Message, Languages.Centre);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Centre);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCentreAsync(listForInsertOrUpdate);
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
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && !string.IsNullOrEmpty(Txt_Libelle.Text) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation)
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
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void Reinitialiser()
        {
            try
            {
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                Cbo_Site.SelectedItem = null;
                Cbo_TypeCentre.SelectedItem = null;
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
                Message.ShowError(ex.Message, Languages.Centre);
            }
        }

        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {
            if (Cbo_Produit.SelectedItem != null)
            {
                CsProduit leProduit = new CsProduit();
                CsProduit leproduitSelect = (CsProduit)Cbo_Produit.SelectedItem ;
                //if (ObjetSelectionnee.LESPRODUITSDUSITE != null )
                //    leProduit = ObjetSelectionnee.LESPRODUITSDUSITE.FirstOrDefault(t => t.FK_IDPRODUIT == leproduitSelect.PK_ID);
                //else
                //    leProduit = listProduit.FirstOrDefault(t => t.FK_IDPRODUIT == leproduitSelect.PK_ID);
                //if (leProduit == null)
                //{
               

                    leproduitSelect.FK_IDPRODUIT = leproduitSelect.PK_ID;
                    if ((SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Creation)
                    {
                        leproduitSelect.FK_IDCENTRE = ObjetSelectionnee.PK_ID;
                        listProduitCentre.AddRange(ObjetSelectionnee.LESPRODUITSDUSITE);
                    }
                    leproduitSelect.PK_ID = 0;
                    leproduitSelect.DATECREATION = System.DateTime.Today;
                    leproduitSelect.USERCREATION = UserConnecte.matricule;
                    listProduitCentre.Add(leproduitSelect);
                    dtg_Produit.ItemsSource = null;
                    dtg_Produit.ItemsSource = listProduitCentre;
                //}
            }
        }

        private void btn_Supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            if (dtg_Produit.SelectedItem != null)
            {
                CsProduit leproduitSelect = (CsProduit)dtg_Produit.SelectedItem;
                CsProduit leProduit = ObjetSelectionnee.LESPRODUITSDUSITE.FirstOrDefault(t => t.FK_IDPRODUIT == leproduitSelect.FK_IDPRODUIT);
                if (leProduit != null)
                {
                    ObjetSelectionnee.LESPRODUITSDUSITE.Remove(leproduitSelect);
                    dtg_Produit.ItemsSource = null;
                    dtg_Produit.ItemsSource = ObjetSelectionnee.LESPRODUITSDUSITE;
                }
            }
        }
    }
}


