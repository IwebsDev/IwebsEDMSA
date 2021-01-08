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
    public partial class UcCaisse : ChildWindow
    {
        List<CsCaisse> listForInsertOrUpdate = null;
        ObservableCollection<CsCaisse> donnesDatagrid = new ObservableCollection<CsCaisse>();
        private CsCaisse ObjetSelectionnee = null;
        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public UcCaisse()
        {
            try
            {
                InitializeComponent();
                Translate();
                ChargerDonneeDuSite();
                this.Txt_Code.MaxLength = 3;
            }
            catch (Exception ex)
            {

             Message.Show(ex.Message, Languages.Caisse);
        
            }
           

        }
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    this.btn_Centre.IsEnabled = true;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<ServiceAccueil.CsSite> _LstSite = lstSite ;
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_CodeSite.Tag = _LstSite[0].PK_ID;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_CodeCentre.Tag = _LstCentre[0].PK_ID;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                            this.btn_Centre.IsEnabled = false;
                            this.Txt_CodeCentre.IsReadOnly = true;
                        }
                    }
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    this.btn_Centre.IsEnabled = true;
                    LstCentre =  Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1 )
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.Txt_CodeSite.Tag  = _LstSite[0].PK_ID ;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<ServiceAccueil.CsCentre> _LstCentre = LstCentre.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE ;
                            this.Txt_CodeCentre.Tag = _LstCentre[0].PK_ID;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                            this.btn_Centre.IsEnabled = false;
                            this.Txt_CodeCentre.IsReadOnly = true;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstSite.Count > 0)
                {
                    this.btn_Site.IsEnabled = false;
                    List<object> _Listgen = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Paramétrage");
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                ServiceAccueil.CsSite leSite = (ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODE;
                this.Txt_LibelleSite.Text = leSite.LIBELLE;
                this.Txt_CodeSite.Tag = leSite.PK_ID;
                List<ServiceAccueil.CsCentre> lstCentreSite = LstCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList();
                if (lstCentreSite != null && lstCentreSite.Count == 1)
                {
                    this.Txt_CodeCentre.Text = lstCentreSite.First().CODE;
                    this.Txt_CodeCentre.Tag = lstCentreSite.First().PK_ID;
                    this.Txt_LibelleCentre.Text = lstCentreSite.First().LIBELLE;
                }
            }
            else
                this.btn_Site.IsEnabled = true;


        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LstCentre.Count > 0)
                {
                    this.btn_Centre.IsEnabled = false;
                    List<object> _Listgen = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.Where(t=>t.FK_IDCODESITE == (int)this.Txt_CodeSite.Tag ).ToList());
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CODE", "LIBELLE", "Centre");
                    ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Paramétrage");
            }

        }
        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Centre.IsEnabled = true;
                ServiceAccueil.CsCentre leCentre = (ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = leCentre.CODE;
                this.Txt_CodeCentre.Tag = leCentre.PK_ID;
                this.Txt_LibelleCentre.Text = leCentre.LIBELLE;
            }
            else
                this.btn_Centre.IsEnabled = true;
        }
        public UcCaisse(object[] pObjects, SessionObject.ExecMode[] pExecMode, DataGrid[] pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var categorieClient = new CsCaisse();
                if (pObjects[0] != null)
                    ObjetSelectionnee = Utility.ParseObject(categorieClient, pObjects[0] as CsCaisse);
                ModeExecution = pExecMode[0];
                dataGrid = pGrid[0];
                ChargerDonneeDuSite();
                if (dataGrid != null) donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsCaisse>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification ||
                    (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        Txt_Code.Text = ObjetSelectionnee.NUMCAISSE;
                        Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        btnV.IsEnabled = false;
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
                Message.Show(ex.Message, Languages.Banque);
            }
        }
  
        private void Translate()
        {
            try
            {
                //Title = Languages.Forfait;
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
    
  
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Reinitialiser();
            this.DialogResult = false;
        }
        private void VerifierSaisie()
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_Code.Text) && 
                    !string.IsNullOrEmpty(Txt_Libelle.Text) &&
                    (SessionObject.ExecMode)ModeExecution != SessionObject.ExecMode.Consultation
                    && !string.IsNullOrEmpty( Txt_CodeCentre.Text ))
                  btnV.IsEnabled=true;
                  
                else
                {
                    btnV.IsEnabled = false;
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
                Txt_Code.Text = string.Empty;
                Txt_Libelle.Text = string.Empty;
                btnV.IsEnabled = false;
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
                Message.ShowError(ex.Message, Languages.Forfait);
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
                Message.ShowError(ex.Message, Languages.Forfait);
            }
        }
        private void GetDataNew()
        {
            try
            {
                ParametrageClient client = new ParametrageClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Parametrage"));
                client.SelectAllCaisseCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.LibelleProduit);
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
                client.SelectAllCaisseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateParentList(CsCaisse pCaisse)
        {
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                    GetDataNew();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                    GetDataNew();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<CsCaisse> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCaisse>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                    var caisse = new CsCaisse
                    {
                        NUMCAISSE = Txt_Code.Text,
                        LIBELLE = Txt_Libelle.Text,
                        FK_IDCENTRE = (int)Txt_CodeCentre.Tag,
                        CENTRE = Txt_CodeCentre.Text ,
                        FONDCAISSE  =string.IsNullOrEmpty(this.Txt_FondDeCaisse.Text ) ? 0 : Convert.ToDecimal(this.Txt_FondDeCaisse.Text),
                        DATECREATION = DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        ACQUIT = "1"
                    };
                    if (!string.IsNullOrEmpty(Txt_Code.Text) && donnesDatagrid.FirstOrDefault(p => p.NUMCAISSE == caisse.NUMCAISSE && p.CENTRE == caisse.CENTRE) != null)
                    {
                        throw new Exception(Languages.CetElementExisteDeja);
                    }
                    listObjetForInsertOrUpdate.Add(caisse);
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    ObjetSelectionnee.NUMCAISSE = Txt_Code.Text;
                    ObjetSelectionnee.LIBELLE = Txt_Libelle.Text;
                    ObjetSelectionnee.FK_IDCENTRE = (int)Txt_CodeCentre.Tag;
                    ObjetSelectionnee.CENTRE = Txt_CodeCentre.Text;
                    ObjetSelectionnee.DATECREATION = DateTime.Now;
                    ObjetSelectionnee.USERCREATION = UserConnecte.matricule;
                    ObjetSelectionnee.FONDCAISSE  =string.IsNullOrEmpty(this.Txt_FondDeCaisse.Text ) ? 0 : Convert.ToDecimal(this.Txt_FondDeCaisse.Text);
                    listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Caisse);
                return null;
            }
        }
        private void btnV_Click(object sender, RoutedEventArgs e)
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
                                service.InsertCaisseCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.Caisse);
                                        return;
                                    }
                                    if (!insertR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.Caisse);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.InsertCaisseAsync(listForInsertOrUpdate);
                            }
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCaisseCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.Caisse);
                                        return;
                                    }
                                    if (!UpdateR.Result)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.Caisse);
                                        return;
                                    }
                                    UpdateParentList(listForInsertOrUpdate[0]);
                                    DialogResult = true;
                                };
                                service.UpdateCaisseAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Languages.Caisse);
            }
        }
    
    }
}

