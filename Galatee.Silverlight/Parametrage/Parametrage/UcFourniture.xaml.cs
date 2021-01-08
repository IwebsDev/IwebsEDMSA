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
    public partial class UcFourniture : ChildWindow
    {
        List<ObjFOURNITURE > listForInsertOrUpdate = null;
        ObservableCollection<ObjFOURNITURE> donnesDatagrid = new ObservableCollection<ObjFOURNITURE>();
        private ObjFOURNITURE ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public List<ObjTYPEDEVIS> _listeDesTypeDevisExistant { get; set; }
      
     
        public UcFourniture()
        {
            InitializeComponent();
        }

        public UcFourniture(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new ObjFOURNITURE();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as ObjFOURNITURE);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                RemplirProduit();
                RemplirListeDesTDEMExistant();
                RemplireMateriel();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<ObjFOURNITURE >;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_CoutFournituure.Text = Convert.ToString(ObjetSelectionnee.COUTUNITAIRE_FOURNITURE);
                        Check_Add.IsChecked = ObjetSelectionnee.ISADDITIONAL;
                        Check_Default.IsChecked = ObjetSelectionnee.ISDEFAULT;
                        txt_Quatite.Text =  Convert.ToString(ObjetSelectionnee.QUANTITY);
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    ObjetSelectionnee = null;
                    Txt_CoutFournituure.Text = "";
                    Txt_CoutPose.Text = "";
                    Check_Add.IsChecked = false;
                    Check_Default.IsChecked = false;
                    txt_Quatite.Text = " ";
                    CboProduit.SelectedIndex=0;
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Diametrecompteur);
            }
        }

        private void Translate()
        {
            try
            {
                Title = Languages.Appareils;
                btnOk.Content = Languages.OK;
                CancelButton.Content = Languages.Annuler;

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
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllProduitCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.Show(error, Languages.Produit);
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

        private void RemplirListeDesTDEMExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllDTEMCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Parametrage.Languages.lblModule);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        this.CboTypeDevis.ItemsSource = args.Result;
                        this.CboTypeDevis.DisplayMemberPath = "LIBELLE";

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                            (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                            if (ObjetSelectionnee != null)
                            {
                                foreach (CsTdem  Tdem in CboTypeDevis.ItemsSource)
                                {
                                    if (Tdem.PK_ID == ObjetSelectionnee.FK_IDTYPEDEMANDE)
                                    {
                                        CboTypeDevis.SelectedItem = Tdem;
                                        break;
                                    }
                                }
                            }
                    }
                };
                client.SelectAllDTEMAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplireMateriel()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllMaterielCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.CoperDemande);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    else
                    {
                        CboMateriel.ItemsSource = args.Result;
                        CboMateriel.DisplayMemberPath = "LIBELLE";
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsMaterielDemande typeDe in CboMateriel.ItemsSource)
                            {
                                if (typeDe.PK_ID == ObjetSelectionnee.FK_IDMATERIELDEVIS )
                                {
                                    CboMateriel.SelectedItem = typeDe;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.SelectAllMaterielAsync();
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
                if (CboTypeDevis.SelectedItem != null && CboMateriel.SelectedItem != null && !string.IsNullOrEmpty(txt_Quatite.Text ) && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && Check_Default.IsChecked != null )
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

        private void OnComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fourniture);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllFournitureCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Fourniture);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        return;
                    }
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result)
                        {
                            donnesDatagrid.Add(item);
                        }
                   
                };
                client.SelectAllFournitureAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateParentList(ObjFOURNITURE  pFourniture)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {

                    GetDataNew();
                       }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {

                    GetDataNew();
                         }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ObjFOURNITURE > GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<ObjFOURNITURE>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var Fourniture = new ObjFOURNITURE
                    {
                        FK_IDPRODUIT = ((CsProduit)CboProduit.SelectedItem).PK_ID,
                        QUANTITY=Convert.ToInt32(txt_Quatite.Text),
                        CODEPRODUIT = ((CsProduit)CboProduit.SelectedItem).CODE,
                        FK_IDTYPEDEMANDE = ((CsTdem)CboTypeDevis.SelectedItem).PK_ID,
                        FK_IDMATERIELDEVIS = ((CsMaterielDemande )CboMateriel.SelectedItem).PK_ID,
                        //ISADDITIONAL=bool.Parse(Check_Add.IsChecked.ToString()),
                        //ISDEFAULT=bool.Parse(Check_Default.IsChecked.ToString()),
                        ISADDITIONAL = false ,
                        ISDEFAULT = true,
                        ISDISTANCE = bool.Parse(Check_Distance.IsChecked.ToString()),
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (CboMateriel.SelectedItem != null && CboTypeDevis.SelectedItem != null  && donnesDatagrid.FirstOrDefault(p => p.CODE == Fourniture.CODE && p.CODEPRODUIT == Fourniture.CODEPRODUIT && 
                                                                                               p.FK_IDMATERIELDEVIS == ((CsMaterielDemande)CboMateriel.SelectedItem).PK_ID &&
                                                                                               p.FK_IDTYPEDEMANDE == ((CsTdem)CboTypeDevis.SelectedItem).PK_ID) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(Fourniture);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                        ObjetSelectionnee.QUANTITY = Convert.ToInt32(txt_Quatite.Text); 
                        ObjetSelectionnee.FK_IDPRODUIT = ((CsProduit)CboProduit.SelectedItem).PK_ID;
                        ObjetSelectionnee.FK_IDTYPEDEMANDE = ((CsTdem)CboTypeDevis.SelectedItem).PK_ID;
                        ObjetSelectionnee.FK_IDMATERIELDEVIS = ((CsMaterielDemande )CboMateriel.SelectedItem).PK_ID;
                        //ObjetSelectionnee.ISADDITIONAL=bool.Parse(Check_Add.IsChecked.ToString());
                        //ObjetSelectionnee. ISDEFAULT=bool.Parse(Check_Default.IsChecked.ToString());
                        ObjetSelectionnee.ISDISTANCE = bool.Parse(Check_Distance.IsChecked.ToString());
                        ObjetSelectionnee.DATECREATION = DateTime.Now;
                        ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                  listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Fourniture);
                return null;
            }
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Fourniture, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertFournitureCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Fourniture);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Fourniture);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertFournitureAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateFournitureCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Fourniture);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Fourniture);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateFournitureAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Fourniture);
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CboMateriel_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (this.CboMateriel.SelectedItem != null)
            {
                this.Txt_CoutFournituure.Text = ((CsMaterielDemande)this.CboMateriel.SelectedItem).COUTUNITAIRE_FOURNITURE.Value.ToString(SessionObject.FormatMontant);
                this.Txt_CoutPose.Text = ((CsMaterielDemande)this.CboMateriel.SelectedItem).COUTUNITAIRE_POSE.Value.ToString(SessionObject.FormatMontant);
            }
        }
   
       

      

      

        
    }
}

