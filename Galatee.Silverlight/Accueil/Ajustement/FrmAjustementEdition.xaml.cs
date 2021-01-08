using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil ;
//using Galatee.Silverlight.ServicePrintings;
using Galatee.Silverlight.Shared;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmAjustementEdition : ChildWindow
    {
        public FrmAjustementEdition()
        {
            InitializeComponent();
            Translate();
        }

        List<CsTypeLot> LstTypeLot = new List<CsTypeLot>();
        List<string > LstMoisCompte = new List<string >();
        List<CsLotCompteClient> LsDeLotCompte = new List<CsLotCompteClient>();

        CsCentre LeCentreSelect = new CsCentre();
        CsTypeLot LeTypeSelect = new CsTypeLot();
        string MoisSelect = string.Empty;
        private void Translate()
        {
            this.chk_Transaction.Content = Langue.lbl_transaction;
            this.Chk_Erreur .Content = Langue.lbl_erreur ;
            this.Chk_CompteClient .Content = Langue.lbl_CompteClient ;
            this.Chk_Batch.Content = Langue.lbl_listebatch;
            this.Chk_ClientRejeter.Content = Langue.lbl_CompteRejeter;
            this.lbl_batch.Content = Langue.lbl_NumBatch;
            this.Chk_batchrejet.Content = Langue.lbl_BatchRejete;
            this.lbl_Origine .Content = Langue.lbl_Origine ;
            this.lbl_type .Content = Langue.lbl_type ;
            this.lbl_moisComptable.Content = Langue.lbl_MoisComptable;
 
        }

        private void ChargerTypeLot()
        {
            try
            {
                if (SessionObject.ListeTypeLot != null && SessionObject.ListeTypeLot.Count != 0)
                {
                    List<CsTypeLot> LstTypeLot = SessionObject.ListeTypeLot;
                    if (LstTypeLot != null && LstTypeLot.Count != 0)
                    {
                        cbo_UpdateTypeBatchDeb.ItemsSource = null;
                        cbo_UpdateTypeBatchDeb.ItemsSource = LstTypeLot;
                        cbo_UpdateTypeBatchDeb.DisplayMemberPath = "LIBELLE";
                    }
                }
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneTypeLotCompleted += (es, argss) =>
                    {
                        List<CsTypeLot> LstTypeLot = new List<CsTypeLot>();
                        if (argss != null && argss.Cancelled)
                            return;
                        LstTypeLot.AddRange(argss.Result);
                        SessionObject.ListeTypeLot = argss.Result;
                        if (LstTypeLot != null && LstTypeLot.Count != 0)
                        {
                            cbo_UpdateTypeBatchDeb.ItemsSource = null;
                            cbo_UpdateTypeBatchDeb.ItemsSource = LstTypeLot;
                            cbo_UpdateTypeBatchDeb.DisplayMemberPath = "LIBELLE";
                        }
                    };
                    service1.RetourneTypeLotAsync();
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsSite> lstSite = new List<CsSite>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (LstCentre != null)
                    {
                        List<CsCentre> leCentreOrigine = SessionObject.LstCentre.Where(t => t.PK_ID == SessionObject.LePosteCourant.FK_IDCENTRE).ToList();
                        if (leCentreOrigine != null && leCentreOrigine.Count != 0)
                        {
                            this.Cbo_UpdateOrigineDeb.ItemsSource = leCentreOrigine.OrderBy(t => t.LIBELLE);
                            Cbo_UpdateOrigineDeb.DisplayMemberPath = "LIBELLE";
                            this.Cbo_UpdateOrigineDeb.SelectedItem = leCentreOrigine.First();
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
        public void ChargerCompteClient(string Origine, string TypeLot)
        {
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDesTypeLotCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                LsDeLotCompte = res.Result;
                if (LsDeLotCompte != null && LsDeLotCompte.Count != 0)
                {
                    foreach (CsLotCompteClient item in LsDeLotCompte)
                    {
                        item.FK_IDMATRICULE = UserConnecte.PK_ID;
                        item.MATRICULE = UserConnecte.matricule;
                        item.TOP1 = SessionObject.Enumere.TopGuichet;
                        item.FK_IDLIBELLETOP = SessionObject.LstDesLibelleTop.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TopGuichet).PK_ID;
                        if (LstMoisCompte.FirstOrDefault(p => p == item.MOISCOMPTABLE) == null)
                            LstMoisCompte.Add(item.MOISCOMPTABLE);
                    }
                }
                if (LstMoisCompte != null && LstMoisCompte.Count != 0)
                {
                    this.Cbo_MoisComptableDeb.ItemsSource = LstMoisCompte.OrderByDescending(p => p).ToList();
                    this.Cbo_MoisComptableDeb.SelectedIndex = 0;
                }
            };
            service1.RetourneListeDesTypeLotAsync(Origine, TypeLot);
            service1.CloseAsync();

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            List<ServiceAccueil.CsLotCompteClient> _ListeDeLotCompte = new List<ServiceAccueil.CsLotCompteClient>();
            string _NumLotDeb = this.Txt_NumeroLotDeb.Text ;
            string _NumLotFin = this.Txt_NumeroLotFin.Text;
            if (string.IsNullOrEmpty(_NumLotFin) && LsDeLotCompte != null )
                _ListeDeLotCompte = LsDeLotCompte.Where(p => (string.IsNullOrEmpty(_NumLotDeb) || p.NUMEROLOT == _NumLotDeb) &&
                                                             (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE ) &&
                                                             (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                             (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect)).ToList();
            else if (!string.IsNullOrEmpty(_NumLotDeb) && !string.IsNullOrEmpty(_NumLotFin) && LsDeLotCompte != null )
                _ListeDeLotCompte = LsDeLotCompte.Where(p => (int.Parse(p.NUMEROLOT) >= int.Parse(_NumLotDeb)) &&
                                                             (int.Parse(p.NUMEROLOT) <= int.Parse(_NumLotFin)) &&
                                                             (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE) &&
                                                             (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                             (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect)).ToList();
            if (_ListeDeLotCompte != null && _ListeDeLotCompte.Count != 0)
                EditionLot(_ListeDeLotCompte);
            else
                Message.ShowInformation(Langue.msglotNonTrouve, Langue.lbl_Menu);
        }
        void DialogClosed(object sender, EventArgs e)
        {
            DialogResult ctrs = sender as DialogResult;
            if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
            {
                ctrs.DialogResult = false;
                return;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Txt_NumeroLotDeb.MaxLength = SessionObject.Enumere.TailleNumeroLigneBatch;
            this.Txt_NumeroLotFin.MaxLength = SessionObject.Enumere.TailleNumeroLigneBatch;
            ChargerDonneeDuSite ();
            ChargerTypeLot();
        }

        private void Cbo_UpdateOrigine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             LeCentreSelect = (CsCentre  )this.Cbo_UpdateOrigineDeb.SelectedItem;
        }

        private void cbo_UpdateTypeBatchDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeTypeSelect = (CsTypeLot)this.cbo_UpdateTypeBatchDeb.SelectedItem;
            LeCentreSelect = (CsCentre  )this.Cbo_UpdateOrigineDeb.SelectedItem;
            ChargerCompteClient(LeCentreSelect.CODE, LeTypeSelect.CODE);
        }

        private void Cbo_MoisComptableDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoisSelect = this.Cbo_MoisComptableDeb.SelectedValue.ToString();
        }

        private void Txt_NumeroLotDeb_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_NumeroLotFin.Text = this.Txt_NumeroLotDeb.Text;
        }
        private void EditionLot(List<CsLotCompteClient> ListLot)
        {
            try
            {
                if (this.chk_Transaction.IsChecked == true)
                    EditerDetailLot(ListLot);
                if (this.Chk_CompteClient.IsChecked == true)
                    EditerCompteMisAjour(ListLot);
                if (this.Chk_Batch.IsChecked == true)
                {
                    string key = Utility.getKey();
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    //param.Add(Utility.setParam("pTypeRecu"), OperationCaisse);
                    Utility.ActionDirectOrientation<ServicePrintings.CsLotCompteClient, ServiceAccueil.CsLotCompteClient>(ListLot, param, SessionObject.CheminImpression , "AjustementTransaction", "Accueil", true);
                    //Utility.ActionImpressionDirect(this.TxtImprimante.Text, key, "AjustementTransaction", "Accueil");

                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditerDetailLot(List<CsLotCompteClient> LesLot)
        {
            List<ServiceAccueil.CsDetailLot> ListeDetail = new List<ServiceAccueil.CsDetailLot>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneDetailLotCompleted  += (es, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ListeDetail.AddRange(args.Result);
                Dictionary<string, string> param = new Dictionary<string, string>();
                //param.Add(Utility.setParam("pTypeRecu"), OperationCaisse);
                Utility.ActionDirectOrientation<ServicePrintings.CsDetailLot, ServiceAccueil.CsDetailLot>(ListeDetail, param, SessionObject.CheminImpression, "AjustementDetailLot", "Accueil", false);
                this.DialogResult = true;


                //string key = Utility.getKey();
                //var service = new ParametrageClient(Utility.Protocole(), Utility.EndPoint("Parametrage"));
                //service.EditerListeBanqueCompleted += (snder, print) =>
                //{
                //    if (print.Cancelled || print.Error != null)
                //    {
                //        Message.ShowError(print.Error.Message, Languages.Banque);
                //        return;
                //    }
                //    if (!print.Result)
                //    {
                //        Message.ShowError(Languages.ErreurImpressionDonnees, Languages.Banque);
                //        return;
                //    }
                //    Utility.ActionImpressionDirect(null, key, "Banque", "Parametrage");
                //};
                //service.EditerListeBanqueAsync(key, dictionaryParam);


            };
            service.RetourneDetailLotAsync(LesLot);
        
        }
        public void EditerCompteMisAjour(List<CsLotCompteClient> LesLot)
        {
            List<ServiceAccueil.CsLclient> ListeCompte = new List<ServiceAccueil.CsLclient>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCompteAjusteCompleted  += (es, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                ListeCompte.AddRange(args.Result);
                Dictionary<string, string> param = new Dictionary<string, string>();
                //param.Add(Utility.setParam("pTypeRecu"), OperationCaisse);
                Utility.Action<ServicePrintings.CsLclient, ServiceAccueil.CsLclient>(ListeCompte, param, SessionObject.CheminImpression, "AjustementCompteClient", "Accueil");
                this.DialogResult = true;

            };
            service.RetourneCompteAjusteAsync(LesLot);

        }

        private void Btn_Printer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

