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
    public partial class UcCoperDemande : ChildWindow
    {
        List<CsCoutDemande> listForInsertOrUpdate = null;
        ObservableCollection<CsCoutDemande> donnesDatagrid = new ObservableCollection<CsCoutDemande>();
        private CsCoutDemande ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;

        public UcCoperDemande()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Languages.CoperDemande);

            }
        }

        public UcCoperDemande(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsCoutDemande();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsCoutDemande);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                ChargerDonneeDuSite();
                ChargerProduit();
                RemplirListeDesCOPERExistant();
                RemplirListeDesTAXIExistant();
                RemplirListeDesTDEMExistant();
                ChargerDiametreCompteur();
                ChargerCategorie();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCoutDemande>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Montant.Text = ObjetSelectionnee.MONTANT.ToString();
                        CboTAXE.SelectedItem = ObjetSelectionnee.TAXE;
                        CboProduit.SelectedItem = ObjetSelectionnee.PRODUIT;
                        if(ObjetSelectionnee.AUTOMATIQUE==true )
                        CheckAuto.IsChecked = true;
                        if (ObjetSelectionnee.OBLIGATOIRE == true)
                        CheckObl.IsChecked = true;
                           

                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.CoperDemande);
            }
        }

        private void Translate()
        {
            try
            {
            //    Title = Languages.Forfait;
            //    btnOk.Content = Languages.OK;
            //    Btn_Reinitialiser.Content = Languages.Annuler;
            //    GboCodeDepart.Header = Languages.InformationsCodePoste;
            //    lab_Code.Content = Languages.Code;
            //    lab_Libelle.Content = Languages.Libelle;
           }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        


        List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    this.CboCentre.ItemsSource = SessionObject.LstCentre;
                    this.CboCentre.DisplayMemberPath = "LIBELLE";
                    this.CboCentre.SelectedValuePath = "PK_ID";
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentre);

                    this.CboCentre.ItemsSource = SessionObject.LstCentre;
                    this.CboCentre.DisplayMemberPath = "LIBELLE";
                    this.CboCentre.SelectedValuePath = "PK_ID";
                    //int idCentreSelect = ObjetSelectionnee.FK_IDCENTRE;
                    //if (ObjetSelectionnee != null && ObjetSelectionnee.FK_IDCENTRE != null)
                    //{
                    //    CboCentre.SelectedItem = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == idCentreSelect);
                    //    CboCentre.IsEnabled = false;
                    //}


                    this.CboSite.ItemsSource = lstSite;
                    this.CboSite.DisplayMemberPath = "LIBELLE";
                    this.CboSite.SelectedValuePath = "PK_ID";
                    //CboCentre.SelectedItem = lstSite.FirstOrDefault(t => t.PK_ID == SessionObject.LstCentre.FirstOrDefault(j => j.PK_ID == ObjetSelectionnee.FK_IDCENTRE).FK_IDCODESITE);
                    //CboSite.IsEnabled = false;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(SessionObject.LstCentre);

                    this.CboCentre.ItemsSource = SessionObject.LstCentre;
                    this.CboCentre.DisplayMemberPath = "LIBELLE";
                    this.CboCentre.SelectedValuePath = "PK_ID";

                    //if (ObjetSelectionnee != null && ObjetSelectionnee.FK_IDCENTRE != null)
                    //{
                    //    CboCentre.SelectedItem = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDCENTRE);
                    //    CboCentre.IsEnabled = false;
                    //}

                    this.CboSite.ItemsSource = lstSite;
                    this.CboSite.DisplayMemberPath = "LIBELLE";
                    this.CboSite.SelectedValuePath = "PK_ID";
                    CboCentre.SelectedItem = lstSite.FirstOrDefault(t => t.PK_ID == SessionObject.LstCentre.FirstOrDefault(j => j.PK_ID == ObjetSelectionnee.FK_IDCENTRE).FK_IDCODESITE );
                    CboSite.IsEnabled = false;

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count > 0)
                {
                    this.CboProduit.ItemsSource = SessionObject.ListeDesProduit;
                    this.CboProduit.DisplayMemberPath = "LIBELLE";
                    this.CboProduit.SelectedValuePath = "PK_ID";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesProduitCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = args.Result;

                    this.CboProduit.ItemsSource = SessionObject.ListeDesProduit;
                    this.CboProduit.DisplayMemberPath = "LIBELLE";
                    this.CboProduit.SelectedValuePath = "PK_ID";
                };
                service.ListeDesProduitAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirListeDesTAXIExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAll_CTAXCompleted += (ssender, args) =>
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
                        this.CboTAXE.ItemsSource = args.Result;
                        this.CboTAXE.DisplayMemberPath = "LIBELLE";

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                            (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCtax Tarif in CboTAXE.ItemsSource)
                            {
                                if (Tarif.PK_ID == ObjetSelectionnee.FK_IDPRODUIT)
                                {
                                    CboTAXE.SelectedItem = Tarif;
                                    break;
                                }
                            }
                            CboTAXE.IsEnabled = false;
                        }
                    }
                };
                client.SelectAll_CTAXAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesCOPERExistant()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCoperCompleted += (ssender, args) =>
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
                        this.CboCoper.DisplayMemberPath = "LIBELLE";
                        this.CboCoper.ItemsSource = args.Result;

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                            (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsCoper Coper in CboCoper.ItemsSource)
                            {
                                if (Coper.PK_ID == ObjetSelectionnee.FK_IDCOPER)
                                {
                                    CboCoper.SelectedItem = Coper;
                                    break;
                                }
                            }

                            CboCoper.IsEnabled = false;
                        }
                    }
                };
                client.SelectAllCoperAsync();
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
                        this.CboTDEM.ItemsSource = args.Result;
                        this.CboTDEM.DisplayMemberPath = "LIBELLE";
                        this.CboTDEM.SelectedValuePath = "PK_ID";

                        if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                            (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsTdem Tdem in CboTDEM.ItemsSource)
                            {
                                if (Tdem.PK_ID == ObjetSelectionnee.FK_IDTYPEDEMANDE)
                                {
                                    CboTDEM.SelectedItem = Tdem;
                                    break;
                                }
                            }
                            CboTDEM.IsEnabled = false;
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

        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie != null && SessionObject.LstCategorie.Count != 0)
                {
                    this.CboCATEG.ItemsSource = null;
                    this.CboCATEG.DisplayMemberPath = "LIBELLE";
                    this.CboCATEG.ItemsSource = SessionObject.LstCategorie;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    this.CboCATEG.ItemsSource = null;
                    this.CboCATEG.DisplayMemberPath = "LIBELLE";
                    this.CboCATEG.ItemsSource = SessionObject.LstCategorie;
                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    this.CboDIAMETRE .ItemsSource = null;
                    this.CboDIAMETRE.DisplayMemberPath = "LIBELLE";
                    this.CboDIAMETRE.ItemsSource = SessionObject.LstReglageCompteur;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                    this.CboDIAMETRE.ItemsSource = null;
                    this.CboDIAMETRE.DisplayMemberPath = "LIBELLE";
                    this.CboDIAMETRE.ItemsSource = SessionObject.LstReglageCompteur;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
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
                VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.CoperDemande);
            }
        }

        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCoperDemandeCompleted += (ssender, args) =>
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
                    donnesDatagrid.Clear();
                    if (args.Result != null)
                        foreach (var item in args.Result.OrderBy(t=>t.PRODUIT ).ThenBy(j=>j.TYPEDEMANDE).ThenBy(k=>k.REGLAGECOMPTEUR))
                        {
                            donnesDatagrid.Add(item);
                        }
                    //dtgrdParametre.ItemsSource = donnesDatagrid;
                };
                client.SelectAllCoperDemandeAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateParentList(CsCoutDemande pCoutcoper)
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

        private Boolean AutoChek(String Auto)
        {


            if(Auto=="1")
           return true;
            else
            return false;
        }
        private String Auto()
        {

            if (CheckAuto.IsChecked == true)
                return "1";
            else
                return "0";
        }
        private String OBLIG()
        {

            if (CheckObl.IsChecked == true)
                return "1";
            else
                return "0";
        }
        private List<CsCoutDemande> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCoutDemande>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var CoperDemande = new CsCoutDemande
                    {

                        AUTOMATIQUE =  CheckAuto.IsChecked,
                        OBLIGATOIRE =  CheckObl.IsChecked,
                        MONTANT = Decimal.Parse(Txt_Montant.Text),
                        FK_IDCENTRE = ((ServiceAccueil.CsCentre)CboCentre.SelectedItem).PK_ID,
                        FK_IDPRODUIT = ((ServiceAccueil.CsProduit)CboProduit.SelectedItem).PK_ID,
                        FK_IDTYPEDEMANDE = ((CsTdem)CboTDEM.SelectedItem).PK_ID,
                        FK_IDREGLAGECOMPTEUR  = ((ServiceAccueil.CsReglageCompteur )CboDIAMETRE.SelectedItem).PK_ID,
                        FK_IDCOPER = ((CsCoper)CboCoper.SelectedItem).PK_ID,
                        FK_IDTAXE = ((CsCtax)CboTAXE.SelectedItem).PK_ID,
                        CENTRE = ((ServiceAccueil.CsCentre)CboCentre.SelectedItem).CODE,
                        PRODUIT = ((ServiceAccueil.CsProduit)CboProduit.SelectedItem).CODE,
                        TAXE = ((CsCtax)CboTAXE.SelectedItem).CODE,
                        COPER = ((CsCoper)CboCoper.SelectedItem).CODE,
                        TYPEDEMANDE = ((CsTdem)CboTDEM.SelectedItem).CODE,
                        REGLAGECOMPTEUR = ((ServiceAccueil.CsReglageCompteur)CboDIAMETRE.SelectedItem).CODE,
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule
                    };
                    if (donnesDatagrid.FirstOrDefault(p => p.PK_ID == CoperDemande.PK_ID) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(CoperDemande);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {


                    ObjetSelectionnee.AUTOMATIQUE  =  CheckAuto.IsChecked;
                    ObjetSelectionnee.OBLIGATOIRE  =  CheckObl.IsChecked;
                          ObjetSelectionnee.MONTANT = Decimal.Parse(Txt_Montant.Text);
                          ObjetSelectionnee.FK_IDCENTRE = ((ServiceAccueil.CsCentre)CboCentre.SelectedItem).PK_ID;
                          ObjetSelectionnee.FK_IDPRODUIT = ((ServiceAccueil.CsProduit)CboProduit.SelectedItem).PK_ID;
                          ObjetSelectionnee.FK_IDTYPEDEMANDE = ((CsTdem)CboTDEM.SelectedItem).PK_ID;
                          ObjetSelectionnee.FK_IDCOPER = ((CsCoper)CboCoper.SelectedItem).PK_ID;
                         ObjetSelectionnee. FK_IDTAXE = ((CsCtax)CboTAXE.SelectedItem).PK_ID;
                         ObjetSelectionnee.FK_IDREGLAGECOMPTEUR  = ((ServiceAccueil.CsReglageCompteur)CboDIAMETRE.SelectedItem).PK_ID;

                         ObjetSelectionnee.CENTRE = ((ServiceAccueil.CsCentre)CboCentre.SelectedItem).CODE;
                         ObjetSelectionnee.PRODUIT = ((ServiceAccueil.CsProduit)CboProduit.SelectedItem).CODE;
                         ObjetSelectionnee. TAXE = ((CsCtax)CboTAXE.SelectedItem).CODE;
                          ObjetSelectionnee.COPER = ((CsCoper)CboCoper.SelectedItem).CODE;
                          ObjetSelectionnee.REGLAGECOMPTEUR = ((ServiceAccueil.CsReglageCompteur)CboDIAMETRE.SelectedItem).CODE;

                          ObjetSelectionnee.TYPEDEMANDE  = ((CsTdem)CboTDEM.SelectedItem).CODE;
                          ObjetSelectionnee.DATECREATION = DateTime.Now;
                          ObjetSelectionnee.USERCREATION = UserConnecte.matricule;

                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Coutcoper);
                return null;
            }
        }

        private void VerifierSaisie()
        {
            try
            {
                if (CboCoper.SelectedItem != null && CboProduit.SelectedItem != null && CboTDEM.SelectedItem != null && (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && CboCentre.SelectedItem != null && !string.IsNullOrEmpty(Txt_Montant.Text))
                    BtnOk.IsEnabled = true;

                else
                {
                    BtnOk.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Reinitialiser()
        {
            try
            {
                CboTDEM.SelectedItem = string.Empty;
                CboProduit.SelectedItem = string.Empty;
                CboCentre.SelectedItem = string.Empty;
                CboCoper.SelectedItem = string.Empty;
                CboTAXE.SelectedItem = string.Empty;
                BtnOk.IsEnabled = false;
                Txt_Montant.Text = string.Empty; ;
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
                Message.ShowError(ex.Message, Languages.Forfait);
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Caisse, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
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
                                service.InsertCoperDemandeCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertCoperDemandeAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCoperDemandeCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.CoperDemande);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.CoperDemande);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCoperDemandeAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.CoperDemande);
            }
        }
    

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        List<ServiceAccueil.CsProduit> lstProduitCentre = new List<ServiceAccueil.CsProduit>();
        private void CboCentre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CboCentre.SelectedItem != null)
            {
                lstProduitCentre = ((ServiceAccueil.CsCentre)CboCentre.SelectedItem).LESPRODUITSDUSITE;
                CboProduit.DisplayMemberPath  = "LIBELLE";
                CboProduit.ItemsSource = lstProduitCentre;

                if (lstProduitCentre == null || lstProduitCentre.Count == 0)
                    CboProduit.ItemsSource = SessionObject.ListeDesProduit ;
            }
        }

        private void CboTDEM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CboSite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.CboCentre.DisplayMemberPath = "LIBELLE";
            this.CboCentre.ItemsSource = (SessionObject.LstCentre.Where(t=>t.FK_IDCODESITE == ((ServiceAccueil.CsSite )CboSite.SelectedItem ).PK_ID ).ToList()); 
        }

        private void CboProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.CboDIAMETRE.DisplayMemberPath = "LIBELLE";
            this.CboDIAMETRE.ItemsSource = (SessionObject.LstReglageCompteur.Where(t => t.FK_IDPRODUIT  == ((ServiceAccueil.CsProduit )CboProduit.SelectedItem).PK_ID).ToList()); 
        }
    }
}

