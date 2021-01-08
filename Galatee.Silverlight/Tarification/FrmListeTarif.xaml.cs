using Galatee.Silverlight.MainView;
using Galatee.Silverlight.ServiceTarification;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Tarification.Helper;
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

namespace Galatee.Silverlight.Tarification
{
    public partial class FrmListeTarif : ChildWindow
    {

        #region Services

        #region Load

        public void LoadAllTarifFacturation( int? idCentre,int? idProduit,int? idRedevance,int? idCodeRecherche)
        {
            try
            {
                ListeTarifFacturation.Clear();
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllTarifFacturationAsync( idCentre, idProduit, idRedevance, idCodeRecherche);
                service.LoadAllTarifFacturationCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                SessionObject.ListeTarifFacturation = res.Result;
                                //LoadAllDetailTarifFacturation(SessionObject.ListeTarifFacturation);
                                foreach (var item in res.Result)
                                    ListeTarifFacturation.Add(item);

                                LoadDatagrid(ListeTarifFacturation.OrderBy(t=>t.CTARCOMP).ToList());
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs","Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void LoadAllDetailTarifFacturation(List<CsTarifFacturation> lstTatif)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                service.LoadAllDetailTarifFacturationAsync();
                service.LoadAllDetailTarifFacturationCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                foreach (var item in lstTatif)
                                {
                                    item.DETAILTARIFFACTURATION = res.Result.Where(t => t.FK_IDTARIFFACTURATION == item.PK_ID).ToList();
                                    item.LIBELLERECHERCHETARIF = SessionObject.ListeRechercheTarif .FirstOrDefault(r => r.CODE == item.RECHERCHETARIF).LIBELLE;
                                    item.LIBELLEREDEVANCE = SessionObject.ListeRedevence.FirstOrDefault(r => r.CODE == item.REDEVANCE).LIBELLE;
                                    ListeTarifFacturation.Add(item);
                                }
                                LoadDatagrid(ListeTarifFacturation.ToList());
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs", "Erreur");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void LoadAllRedevance()
        {
            try
            {
                if (SessionObject.ListeRedevence.Count != 0)
                {
                    ListeRedevence = SessionObject.ListeRedevence;

                    return;
                }
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllRedevanceAsync();
                service.LoadAllRedevanceCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                SessionObject.ListeRedevence = res.Result;
                                foreach (var item in SessionObject.ListeRedevence)
                                {
                                    ListeRedevence.Add(item);
                                }
                                SessionObject.ListeRedevence = ListeRedevence;
                                //InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRedevence);
                                //dgListeRedevence.ItemsSource = view;
                                //datapager.Source = view;
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };

                //    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void LoadAllRechercheTarif()
        {
            try
            {
                if (SessionObject.ListeRechercheTarif.Count != 0)
                {
                    ListeRechercheTarif = SessionObject.ListeRechercheTarif;

                    return;
                }
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Chargement des donnée ...");
                service.LoadAllRechercheTarifAsync();
                service.LoadAllRechercheTarifCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                foreach (var item in res.Result)
                                {
                                    ListeRechercheTarif.Add(item);
                                }
                                SessionObject.ListeRechercheTarif = ListeRechercheTarif;
                                //InitCentre_Redev_RechTarif_ModeCalc_ModeApp();
                                //System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(ListeRechercheTarif);
                                //dgListeRechercheTarif.ItemsSource = view;
                                //datapager.Source = view;
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };

                //    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit.Count != 0)
                {
                    LstDeProduit = SessionObject.ListeDesProduit;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    LstDeProduit = SessionObject.ListeDesProduit;

                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerListeDeProduit");
            }
        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    LstCentre = SessionObject.LstCentre;
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite.First().CODE;
                            this.Txt_LibelleSite.Text = _LstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
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
                    LstCentre = SessionObject.LstCentre;
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite.First().CODE;
                            this.Txt_LibelleSite.Text = _LstSite.First().LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }
        #endregion

        #region Update

        public void Save(CsTarifFacturation TarifFacturationoUpdate, int Action)
        {
            try
            {
                TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                int handler = LoadingManager.BeginLoading("Mise à jour des données ...");
                service.SaveTarifFacturationAsync(TarifFacturationoUpdate, Action);
                service.SaveTarifFacturationCompleted += (er, res) =>
                {
                    try
                    {
                        if (res.Error != null || res.Cancelled)
                            Message.Show("Erreur dans le traitement : " + res.Error.InnerException.ToString(), "Erreur");
                        else
                            if (res.Result != null)
                            {
                                if (res.Result)
                                    LoadDatagrid(ListeTarifFacturation.ToList());
                                else
                                {
                                    Message.Show("Sauvegarde non effectuée avec succès, il se peut que vos modifications n'aient pas été prises en compte",
                                    "Info");
                                }
                            }
                            else
                                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                    "Erreur");
                        LoadingManager.EndLoading(handler);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Variables
      
        ObservableCollection<CsTarifFacturation> ListeTarifFacturation = new ObservableCollection<CsTarifFacturation>();

        public List<CsTarifFacturation> TarifFacturationoUpdate = new List<CsTarifFacturation>();
        public List<CsTarifFacturation> TarifFacturationoInserte = new List<CsTarifFacturation>();
        public List<CsTarifFacturation> TarifFacturationoDelete = new List<CsTarifFacturation>();

        List<CsRedevance> ListeRedevence = new List<CsRedevance>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = new List<Galatee.Silverlight.ServiceAccueil.CsCentre>();
        public static List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = new List<Galatee.Silverlight.ServiceAccueil.CsSite>();
        List<CsRechercheTarif> ListeRechercheTarif = new List<CsRechercheTarif>();

        List<Galatee.Silverlight.ServiceAccueil.CsProduit> LstDeProduit = new List<Galatee.Silverlight.ServiceAccueil.CsProduit>();


        FrmTarifFacturation Newfrm = new FrmTarifFacturation();

        #endregion

        #region EventHandler

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
          Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string PK_ID = ((Button)e.OriginalSource).Tag.ToString();
            int tarifselectionne_PK_ID = int.Parse(PK_ID);
            CsTarifFacturation tarifselectionne = ListeTarifFacturation.FirstOrDefault(t => t.PK_ID == tarifselectionne_PK_ID) ;
            FrmTarifFacturation Updatefrm = new FrmTarifFacturation(tarifselectionne, true);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (tarifselectionne.PK_ID != 0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeTarifFacturation.Remove(tarifselectionne);
                TarifFacturationoInserte.Remove(tarifselectionne);
            }
            Updatefrm.Show();
        }

       
        
       

        //private void chb_Duplication_Checked(object sender, RoutedEventArgs e)
        //{

        //        DeleteButton.Visibility = Visibility.Collapsed; 
        //        NewButton.Visibility = Visibility.Collapsed;

        //        lbl_site.Visibility = Visibility.Visible;
        //        cbo_Site.Visibility = Visibility.Visible;
        //        lbl_centreCible.Visibility = Visibility.Visible;
        //        cbo_centreCible.Visibility = Visibility.Visible;
        //        btn_Dupli_Tarif_Centre.Visibility = Visibility.Visible;
        //}
        //private void chb_Duplication_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    DeleteButton.Visibility = Visibility.Visible;
        //    NewButton.Visibility = Visibility.Visible;

        //    lbl_site.Visibility = Visibility.Collapsed;
        //    cbo_Site.Visibility = Visibility.Collapsed;
        //    lbl_centreCible.Visibility = Visibility.Collapsed;
        //    cbo_centreCible.Visibility = Visibility.Collapsed;
        //    btn_Dupli_Tarif_Centre.Visibility = Visibility.Collapsed;
        //}


        void Updatefrm_CallBack(object sender, CustumEventArgs e)
        {
            CsTarifFacturation TarifFacturationoUpdate = new CsTarifFacturation();
            TarifFacturationoUpdate =(CsTarifFacturation)e.Bag;
            Save(TarifFacturationoUpdate,2);

                //if (TarifFacturationoUpdate.PK_ID.Contains(((CsTarifFacturation)e.Bag).PK_ID))
                //{
                //    //CsTarifFacturation LotsScelleRecuToUpdate_ = TarifFacturationoUpdate.FirstOrDefault(l => l.PK_ID == ((CsTarifFacturation)e.Bag).PK_ID);
                //    //int indexOfLotsScelleRecuToUpdate_ = TarifFacturationoUpdate.IndexOf(LotsScelleRecuToUpdate_);
                //    //TarifFacturationoUpdate[indexOfLotsScelleRecuToUpdate_] = (CsTarifFacturation)e.Bag;

                //    CsTarifFacturation LotsScelleRecuToUpdate = ListeTarifFacturation.FirstOrDefault(l => l.PK_ID == ((CsTarifFacturation)e.Bag).PK_ID);
                //    int indexOfLotsScelleRecuToUpdate = ListeTarifFacturation.IndexOf(LotsScelleRecuToUpdate);
                //    ListeTarifFacturation[indexOfLotsScelleRecuToUpdate] = (CsTarifFacturation)e.Bag;

                //    SessionObject.ListeTarifFacturation = ListeTarifFacturation.ToList();
                //}
                //else
                //{
                //    TarifFacturationoUpdate.Add((CsTarifFacturation)e.Bag);
                //    Save(TarifFacturationoUpdate, new List<CsTarifFacturation>(), new List<CsTarifFacturation>());


                //    CsTarifFacturation LotsScelleRecuToUpdate = ListeTarifFacturation.FirstOrDefault(l => l.PK_ID == ((CsTarifFacturation)e.Bag).PK_ID);
                //    int indexOfLotsScelleRecuToUpdate = ListeTarifFacturation.IndexOf(LotsScelleRecuToUpdate);
                //    ListeTarifFacturation[indexOfLotsScelleRecuToUpdate] = (CsTarifFacturation)e.Bag;

                //    SessionObject.ListeTarifFacturation = ListeTarifFacturation.ToList();
                //}
                LoadDatagrid(ListeTarifFacturation.ToList());

        }
        void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
            this.IsEnabled = true ;
            CsTarifFacturation TarifFacturationoUpdate = new CsTarifFacturation();
            TarifFacturationoUpdate = (CsTarifFacturation)e.Bag;
            Save(TarifFacturationoUpdate, 2);


            ListeTarifFacturation.Add((CsTarifFacturation)e.Bag);

            SessionObject.ListeTarifFacturation = ListeTarifFacturation.ToList();
            LoadDatagrid(ListeTarifFacturation.OrderBy(t=>t.CTARCOMP).ToList());

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            new FrmTarifFacturation((CsTarifFacturation)dgListeTarifFacturation.SelectedItem).Show();
            //new FrmTarifFacturation((CsTarifFacturation)dgListeTarifFacturation.SelectedItem).Show();
        }
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            FrmGenerationCtarcomp Newfrm = new FrmGenerationCtarcomp();
            Newfrm.CallBack += Newfrm_CallBack;
            this.IsEnabled = false;
            Newfrm.Show();
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            FrmTarifFacturation Updatefrm = new FrmTarifFacturation((CsTarifFacturation)dgListeTarifFacturation.SelectedItem, true);
            CsTarifFacturation TarifFacturation = ((CsTarifFacturation)dgListeTarifFacturation.SelectedItem);
            //On verifie que l'element actuelement selectionner a jamais été insérer en base en s'assurant que PK_ID est different de 0
            if (TarifFacturation.PK_ID!=0)
            {
                Updatefrm.CallBack += Updatefrm_CallBack;
            }
            else
            {
                //Sinon on la considere comme une nouvelle insertion
                Updatefrm.CallBack += Newfrm_CallBack;
                ListeTarifFacturation.Remove(TarifFacturation);
                TarifFacturationoInserte.Remove(TarifFacturation);
            }
            Updatefrm.Show();
           
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
            messageBox.OnMessageBoxClosed += (_, result) =>
            {
                if (messageBox.Result == MessageBoxResult.OK)
                {
                    CsTarifFacturation LotsScelleRecuToDelete = (CsTarifFacturation)dgListeTarifFacturation.SelectedItem;
                    Save(LotsScelleRecuToDelete,3);


                    ListeTarifFacturation.Remove(LotsScelleRecuToDelete);

                    SessionObject.ListeTarifFacturation = ListeTarifFacturation.ToList();
                    LoadDatagrid(ListeTarifFacturation.ToList());
                }
                else
                {
                    return;
                }
            };
            messageBox.Show();
        }

        private void dgListeTarifFacturation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgListeTarifFacturation.SelectedItem != null)
            {
                CsTarifFacturation TarifFacturation = (CsTarifFacturation)dgListeTarifFacturation.SelectedItem;
                var DetailDataSource = ListeTarifFacturation.Where(t => t.LIBELLECENTRE == TarifFacturation.LIBELLECENTRE &&
                t.FK_IDCENTRE == TarifFacturation.FK_IDCENTRE &&
                t.LIBELLEPRODUIT == TarifFacturation.LIBELLEPRODUIT &&
                t.MODEAPPLICATION == TarifFacturation.MODEAPPLICATION &&
                t.RECHERCHETARIF == TarifFacturation.RECHERCHETARIF &&
                t.REDEVANCE == TarifFacturation.REDEVANCE);
                if (DetailDataSource != null)
                {
                    dg_Detail.ItemsSource = DetailDataSource.ToList();
                }

            }

        }

        private void ChildWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (TarifFacturationoUpdate.Count > 0 || TarifFacturationoInserte.Count > 0 || TarifFacturationoDelete.Count > 0)
            //{
            //    Save(TarifFacturationoUpdate, TarifFacturationoInserte, TarifFacturationoDelete);
            //}
        }


        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += ctr_ClosedCentre;
                this.IsEnabled = false; 
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void ctr_ClosedCentre(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsCentre _LaCateg = (Galatee.Silverlight.ServiceAccueil.CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = _LaCateg.CODE;
                LstDeProduit = _LaCateg.LESPRODUITSDUSITE;
                LstDeProduit = LstCentre.First().LESPRODUITSDUSITE;
                if (LstDeProduit != null && LstDeProduit.Count == 1)
                {
                    this.Txt_CodeProduit.Text = LstDeProduit.First().CODE;
                    this.Txt_CodeProduit.Tag = LstDeProduit.First().PK_ID;
                    this.Txt_LibelleProduit.Text = LstDeProduit.First().LIBELLE;
                }
            }
        }
     
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_LibelleCentre1.Text = _LeCentreClient.LIBELLE;
                        Txt_CodeCentre.Tag = _LeCentreClient.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Centre inexistant dans pour ce site", "Centre");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tarif");
            }
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += ctr_ClosedCtr;
                this.IsEnabled = false;
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void ctr_ClosedCtr(object sender, EventArgs e)
        {
            this.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.Txt_CodeCentre.Text = string.Empty;
                this.Txt_LibelleCentre1.Text = string.Empty;
                this.Txt_CodeCentre.Tag = null;
                Galatee.Silverlight.ServiceAccueil.CsSite _LeSite = (Galatee.Silverlight.ServiceAccueil.CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = _LeSite.CODE;
                this.Txt_CodeSite.Tag = _LeSite.PK_ID;
                LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
                if (LstCentre != null && LstCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = LstCentre.First().CODE;
                    LstDeProduit = LstCentre.First().LESPRODUITSDUSITE;

                    if (LstDeProduit != null && LstDeProduit.Count == 1)
                    {
                        this.Txt_CodeProduit.Text = LstDeProduit.First().CODE;
                        this.Txt_CodeProduit.Tag = LstDeProduit.First().PK_ID;
                        this.Txt_LibelleProduit.Text = LstDeProduit.First().LIBELLE;
                    }
                }
            }
        }
        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    Galatee.Silverlight.ServiceAccueil.CsSite _LeSiteClient =lstSite.FirstOrDefault(t=>t.CODE ==  this.Txt_CodeSite.Text );
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteClient.PK_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tarification");

            }
        }


        private void btn_Redevence_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (this.Txt_CodeProduit.Tag != null)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRedevence.Where(t => t.FK_IDPRODUIT == (int)this.Txt_CodeProduit.Tag).OrderBy(p => p.CODE).ToList());
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += ctr_ClosedRedevane;
                    this.IsEnabled = false;
                    ctr.Show();
                }
                else
                {
                    Message.ShowInformation("Selectionner le produit", "Tarification");
                    return;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void ctr_ClosedRedevane(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRedevance _LaRedevance = (CsRedevance)ctrs.MyObject;
                this.Txt_CodeRedevence.Text = _LaRedevance.CODE;
                this.Txt_CodeRedevence.Tag = _LaRedevance.PK_ID;
            }
        }
        
        int IdRedevance = 0;
        private void Txt_CodeRedevence_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeRedevence.Text) && Txt_CodeRedevence.Text.Length == 2)
                {
                    CsRedevance _LaRedevance = ClasseMEthodeGenerique.RetourneObjectFromList(ListeRedevence.Where(t => t.FK_IDPRODUIT == (int)this.Txt_CodeProduit.Tag).ToList(), this.Txt_CodeRedevence.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaRedevance.LIBELLE))
                    {
                        this.Txt_LibelleRedevence.Text = _LaRedevance.LIBELLE;
                        IdRedevance = _LaRedevance.PK_ID;
                        this.Txt_CodeRedevence.Tag = _LaRedevance.PK_ID;

                        //this.csVariableDeTarification.FK_IDREDEVANCE = IdRedevance;

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tarif");
            }
        }

        private void btn_RechercheTarif_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(ListeRechercheTarif.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += ctr_ClosedRechercheTarif;
                this.IsEnabled = false;
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void ctr_ClosedRechercheTarif(object sender, EventArgs e)
        {
            this.IsEnabled = true ;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRechercheTarif _LaRechercheTarif = (CsRechercheTarif)ctrs.MyObject;
                this.Txt_CodeRechercheTarif.Text = _LaRechercheTarif.CODE;
                this.Txt_CodeRechercheTarif.Tag = _LaRechercheTarif.PK_ID;
            }
        }
      
        int IdRechercheTarif = 0;
        private void Txt_CodeRechercheTarif_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeRechercheTarif.Text) && Txt_CodeRechercheTarif.Text.Length == 3)
                {
                    CsRechercheTarif _LaRechercheTarif = ClasseMEthodeGenerique.RetourneObjectFromList(ListeRechercheTarif, this.Txt_CodeRechercheTarif.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LaRechercheTarif.LIBELLE))
                    {
                        this.Txt_LibelleRechercheTarif.Text = _LaRechercheTarif.LIBELLE;
                        IdRechercheTarif = _LaRechercheTarif.PK_ID;
                        this.Txt_CodeRechercheTarif.Tag = _LaRechercheTarif.PK_ID;

                        //this.csVariableDeTarification.FK_IDRECHERCHETARIF = IdRechercheTarif;

                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Tarif");
            }
        }


        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (LstDeProduit != null && LstDeProduit.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDeProduit);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += ctr_ClosedProduit;
                    this.IsEnabled = false;
                    ctr.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void ctr_ClosedProduit(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                Galatee.Silverlight.ServiceAccueil.CsProduit _Leproduit = (Galatee.Silverlight.ServiceAccueil.CsProduit)ctrs.MyObject;
                this.Txt_CodeProduit.Text = _Leproduit.CODE;
                this.Txt_CodeProduit.Tag = _Leproduit.PK_ID;
                this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
           
        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) && this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    Galatee.Silverlight.ServiceAccueil.CsProduit _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeProduit, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduit.CODE))
                    {
                        this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                        this.Txt_CodeProduit.Tag = _LeProduit.PK_ID ;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Devis");
            }
        }


        #endregion

        #region Constructeurs

        public FrmListeTarif()
        {
            InitializeComponent();
            InitializeComponentData();

            //LoadAllTarifFacturation();
            ChargerDonneeDuSite();
            LoadAllRedevance();
            LoadAllRechercheTarif();
            ChargerListeDeProduit();
            LoadAllUniteComptage();
            DeleteButton.Visibility = Visibility.Visible;
            NewButton.Visibility = Visibility.Visible;

            //lbl_site.Visibility = Visibility.Collapsed;
            //cbo_Site.Visibility = Visibility.Collapsed;
            //lbl_centreCible.Visibility = Visibility.Collapsed;
            //cbo_centreCible.Visibility = Visibility.Collapsed;
            //btn_Dupli_Tarif_Centre.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Menue Contextuel

        ContextMenu ctxmn = null;
        MenuItem mnitem;

        void mnitem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnitem = (MenuItem)sender;

            switch (mnitem.Header.ToString())
            {
                case "Créer":
                     FrmGenerationCtarcomp Newfrm = new FrmGenerationCtarcomp();
                        Newfrm.CallBack += Newfrm_CallBack;
                        Newfrm.Show();
                    break;
                case "Dupliquer sur autre centre":
                    {
                        if (lbl_id_centre.Content != null )
                        {
                            new FrmTarifDuplication(int.Parse(lbl_id_centre.Content.ToString())).Show();
                        }
                        else
                        {
                            Message.ShowInformation("Sélectionnez le centre origine", "Tarification");
                            return;
                        }
                        break;
                    }
                //case "Consultation":
                //    new FrmTarifFacturation((CsTarifFacturation)dgListeTarifFacturation.SelectedItem).Show();
                //    break;
                //case "Modification":
                //    FrmTarifFacturation Updatefrm = new FrmTarifFacturation((CsTarifFacturation)dgListeTarifFacturation.SelectedItem, true);
                //    Updatefrm.CallBack += Updatefrm_CallBack;
                //    Updatefrm.Show();
                //    break;
                case "Supprimer":
                    var messageBox = new MessageBoxControl.MessageBoxChildWindow("Attention", "Êtes-vous sûr de vouloir supprimer la ligne?", MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Information);
                    messageBox.OnMessageBoxClosed += (_, result) =>
                    {
                        if (messageBox.Result == MessageBoxResult.OK)
                        {
                            CsTarifFacturation LotsScelleRecuToDelete = (CsTarifFacturation)dgListeTarifFacturation.SelectedItem;
                            TarifFacturationoDelete.Add(LotsScelleRecuToDelete);
                            ListeTarifFacturation.Remove(LotsScelleRecuToDelete);
                        }
                        else
                        {
                            return;
                        }
                    };
                    messageBox.Show();
                    break;
                default:
                    break;
            }
        }

        void Row_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            dgListeTarifFacturation.SelectedItem = ((sender) as DataGridRow).DataContext;
        }

        private void dgListeTarifFacturation_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void dgListeTarifFacturation_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ctxmn = new ContextMenu();

            mnitem = new MenuItem();
            mnitem.Header = "Créer";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);
            
            mnitem = new MenuItem();
            mnitem.Header = "Dupliquer sur autre centre";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            //mnitem = new MenuItem();
            //mnitem.Header = "Consultation";
            //ctxmn.Items.Add(mnitem);
            //mnitem.Click += new RoutedEventHandler(mnitem_Click);

            //mnitem = new MenuItem();
            //mnitem.Header = "Modification";
            //ctxmn.Items.Add(mnitem);
            //mnitem.Click += new RoutedEventHandler(mnitem_Click);

            mnitem = new MenuItem();
            mnitem.Header = "Supprimer";
            ctxmn.Items.Add(mnitem);
            mnitem.Click += new RoutedEventHandler(mnitem_Click);

            ctxmn.IsOpen = true;
            ctxmn.HorizontalOffset = e.GetPosition(null).X;
            ctxmn.VerticalOffset = e.GetPosition(null).Y;
        }

        private void dgListeTarifFacturation_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseRightButtonDown += new MouseButtonEventHandler(Row_MouseRightButtonDown);
        }

        #endregion

        #region Methodes  d'interface

        private void InitializeComponentData()
        {
            this.dgListeTarifFacturation.ItemsSource = ListeTarifFacturation;
        }
        private void LoadDatagrid(List<CsTarifFacturation> ListeTarifFacturation)
        {
            var listTarif = (from t in ListeTarifFacturation
                             orderby t.LIBELLECENTRE, t.LIBELLEPRODUIT, t.CTARCOMP
                             select t).ToList();
            var datasource = (from t in listTarif
                              group new
                              {
                                  PK_ID = t.PK_ID, 
                                  LIBELLECENTRE = t.LIBELLECENTRE,
                                  CENTRE = t.CENTRE,
                                  DATECREATION = t.DATECREATION,
                                  DATEMODIFICATION = t.DATEMODIFICATION,
                                  LIBELLECOMMUNE = t.LIBELLECOMMUNE,
                                  COMMUNE = t.COMMUNE,
                                  USERCREATION = t.USERCREATION,
                                  USERMODIFICATION = t.USERMODIFICATION,
                                  CTARCOMP = t.CTARCOMP,
                                  DEBUTAPPLICATION = t.DEBUTAPPLICATION,
                                  FINAPPLICATION = t.FINAPPLICATION,
                                  FK_IDCENTRE = t.FK_IDCENTRE,
                                  FK_IDPRODUIT = t.FK_IDPRODUIT,
                                  FK_IDTAXE = t.FK_IDTAXE,
                                  FK_IDUNITECOMPTAGE = t.FK_IDUNITECOMPTAGE,
                                  FK_IDVARIABLETARIF = t.FK_IDVARIABLETARIF,
                                  FORFVAL = t.FORFVAL,
                                  MINIVAL = t.MINIVAL,
                                  MINIVOL = t.MINIVOL,
                                  MONTANTANNUEL = t.MONTANTANNUEL,
                                  PERDEB = t.PERDEB,
                                  PERFIN = t.PERFIN,
                                  LIBELLEPRODUIT = t.LIBELLEPRODUIT,
                                  PRODUIT = t.PRODUIT,
                                  LIBELLERECHERCHETARIF = t.LIBELLERECHERCHETARIF,
                                  RECHERCHETARIF = t.RECHERCHETARIF,
                                  LIBELLEREDEVANCE = t.LIBELLEREDEVANCE,
                                  REDEVANCE = t.REDEVANCE,
                                  REGION = t.REGION,
                                  SREGION = t.SREGION,
                                  TAXE = t.TAXE,
                                  UNITE = t.UNITE,
                                  MODEAPPLICATION = t.MODEAPPLICATION
                              } by new
                              {
                                  LIBELLECENTRE = t.LIBELLECENTRE,
                                  FK_IDCENTRE = t.FK_IDCENTRE,
                                  CENTRE=t.CENTRE,
                                  LIBELLEPRODUIT = t.LIBELLEPRODUIT,
                                 PRODUIT=t.PRODUIT,
                                  MODEAPPLICATION = t.MODEAPPLICATION,
                                  RECHERCHETARIF = t.RECHERCHETARIF,
                                  LIBELLERECHERCHETARIF = t.LIBELLERECHERCHETARIF,
                                  REDEVANCE = t.REDEVANCE,
                                  LIBELLEREDEVANCE = t.LIBELLEREDEVANCE

                              } into groupetarif
                              select new CsTarifFacturation
                              {
                                  LIBELLECENTRE = groupetarif.Key.LIBELLECENTRE, 
                                  CENTRE=groupetarif.Key.CENTRE,
                                  PRODUIT=groupetarif.Key.PRODUIT,
                                  FK_IDCENTRE = groupetarif.Key.FK_IDCENTRE,
                                  LIBELLEPRODUIT = groupetarif.Key.LIBELLEPRODUIT,
                                  MODEAPPLICATION = groupetarif.Key.MODEAPPLICATION,
                                  RECHERCHETARIF = groupetarif.Key.RECHERCHETARIF,
                                  LIBELLERECHERCHETARIF = groupetarif.Key.LIBELLERECHERCHETARIF,
                                  REDEVANCE = groupetarif.Key.REDEVANCE,
                                  LIBELLEREDEVANCE = groupetarif.Key.LIBELLEREDEVANCE,
                                  //DETAIL = groupetarif

                              }
                            ).ToList();
            var collection = (from t in datasource orderby t.LIBELLECENTRE, t.LIBELLEPRODUIT select t).ToList();
            System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(collection);
            dgListeTarifFacturation.ItemsSource = view;
            datapager.Source = view;
        }

        #endregion

        private void LoadAllUniteComptage()
        {
            try
            {
                if (SessionObject.ListeUniteComptage.Count != 0)
                    return;
                else
                {
                    TarificationServiceClient service = new TarificationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Tarification"));
                    service.LoadAllUniteComptageCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.ListeUniteComptage = args.Result;

                    };
                    service.LoadAllUniteComptageAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int? idCentre = this.Txt_CodeCentre.Tag != null ? (int?)this.Txt_CodeCentre.Tag : null;
            int? idProduit = this.Txt_CodeProduit.Tag != null ? (int?)this.Txt_CodeProduit.Tag : null;
            int? idRedevance = this.Txt_CodeRedevence.Tag != null ? (int?)this.Txt_CodeRedevence.Tag : null;
            int? idCodeRecherche = this.Txt_CodeRechercheTarif.Tag != null ? (int?)this.Txt_CodeRechercheTarif.Tag : null; 

                LoadAllTarifFacturation(idCentre,idProduit,idRedevance,idCodeRecherche );

            //List<CsTarifFacturation> datasource = ListeTarifFacturation.ToList();
            //if (!string.IsNullOrWhiteSpace(Txt_CodeCentre.Text))
            //{
            //    datasource = datasource.Where(t => t.CENTRE == Txt_CodeCentre.Text) != null ? datasource.Where(t => t.CENTRE == Txt_CodeCentre.Text).ToList() : null;
            //}
            //if (!string.IsNullOrWhiteSpace(Txt_CodeRedevence.Text))
            //{
            //    datasource = datasource.Where(t => t.REDEVANCE == Txt_CodeRedevence.Text) != null ? datasource.Where(t => t.REDEVANCE == Txt_CodeRedevence.Text).ToList() : null;
            //}
            //if (!string.IsNullOrWhiteSpace(Txt_CodeRechercheTarif.Text))
            //{
            //    datasource = datasource.Where(t => t.RECHERCHETARIF == Txt_CodeRechercheTarif.Text) != null ? datasource.Where(t => t.RECHERCHETARIF == Txt_CodeRechercheTarif.Text).ToList() : null;
            //}
            //if (!string.IsNullOrWhiteSpace(Txt_CodeProduit.Text))
            //{
            //    datasource = datasource.Where(t => t.PRODUIT == Txt_CodeProduit.Text) != null ? datasource.Where(t => t.PRODUIT == Txt_CodeProduit.Text).ToList() : null;
            //}
            //if (datasource!=null)
            //{
            //    LoadDatagrid(datasource.ToList());
            //}
            
        }


       

      

       

       

       

       

    }
}

