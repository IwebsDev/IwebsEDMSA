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


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmAjustementUpdate : ChildWindow
    {
        public FrmAjustementUpdate()
        {
            InitializeComponent();
        }
        List<CsOrigineLot> LstOrigine = new List<CsOrigineLot>();
        List<CsTypeLot> LstTypeLot = new List<CsTypeLot>();
        List<string > LstMoisCompte = new List<string >();
        List<CsLotCompteClient> LsDeLotCompte = new List<CsLotCompteClient>();

        CsCentre LeCentreSelect = new CsCentre();
        CsTypeLot LeTypeSelect = new CsTypeLot();
        string MoisSelect = string.Empty;
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
                if (LsDeLotCompte != null)
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
                this.Cbo_MoisComptableDeb.ItemsSource = LstMoisCompte.OrderByDescending(p => p).ToList();
                this.Cbo_MoisComptableDeb.SelectedIndex  = 0;

            };
            service1.RetourneListeDesTypeLotAsync(Origine, TypeLot);
            service1.CloseAsync();

        }
        void Translate()
        {
            this.lbl_Origine.Content = Langue.lbl_Origine;
            this.lbl_Type.Content = Langue.lbl_type;
            this.lbl_MoisCompta.Content = Langue.lbl_MoisComptable;
            this.lbl_Batch.Content = Langue.lbl_Batch;
            this.lbl_Batch1.Content = Langue.lbl_Batch;
            this.lbl_Parametre.Content = Langue.lbl_Parametre;
            this.lbl_Parametre1.Content = Langue.lbl_Parametre;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<CsLotCompteClient> _ListeDeLotCompte = new List<CsLotCompteClient>();
                string _NumLotDeb = this.Txt_NumeroLotDeb.Text;
                string _NumLotFin = this.Txt_NumeroLotFin.Text;
                if (string.IsNullOrEmpty(_NumLotFin))
                    _ListeDeLotCompte = LsDeLotCompte.Where(p => (string.IsNullOrEmpty(_NumLotDeb) || p.NUMEROLOT == _NumLotDeb) &&
                                                                 (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE) &&
                                                                 (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                                 (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect) &&
                                                                 p.STATUS == "0").ToList();
                else if (!string.IsNullOrEmpty(_NumLotDeb) && !string.IsNullOrEmpty(_NumLotFin))
                    _ListeDeLotCompte = LsDeLotCompte.Where(p => (int.Parse(p.NUMEROLOT) >= int.Parse(_NumLotDeb)) &&
                                                                 (int.Parse(p.NUMEROLOT) <= int.Parse(_NumLotFin)) &&
                                                                 (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE) &&
                                                                 (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                                 (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect) &&
                                                                 p.STATUS == "0").ToList();
                if (_ListeDeLotCompte != null && _ListeDeLotCompte.Count != 0)
                {
                    foreach (CsLotCompteClient item in _ListeDeLotCompte)
                        item.STATUS = "1";

                    ValiderMiseAJour(_ListeDeLotCompte);
                }
                else
                {
                    Message.ShowError(Langue.msglotNonTrouve, "");
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Txt_NumeroLotDeb.MaxLength = SessionObject.Enumere.TailleNumeroLigneBatch;
                this.Txt_NumeroLotFin.MaxLength = SessionObject.Enumere.TailleNumeroLigneBatch;
                ChargerDonneeDuSite();
                ChargerTypeLot();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Cbo_UpdateOrigine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cbo_UpdateTypeBatchDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeTypeSelect = (CsTypeLot)this.cbo_UpdateTypeBatchDeb.SelectedItem;
            LeCentreSelect = (CsCentre )this.Cbo_UpdateOrigineDeb.SelectedItem;
            ChargerCompteClient(LeCentreSelect.CODE , LeTypeSelect.CODE);
        }

        private void Cbo_MoisComptableDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MoisSelect = this.Cbo_MoisComptableDeb.SelectedValue.ToString();
        }

        private void Txt_NumeroLotDeb_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_NumeroLotFin.Text = this.Txt_NumeroLotDeb.Text;
        }
        private void  ValiderMiseAJour(List<CsLotCompteClient> ListLot)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.ValiderMiseAJourBatchCompleted  += (es, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result == true)
                    Message.ShowInformation(Langue.MsgOperationTerminee,"Mise a jour");
                else
                    Message.ShowInformation(Langue.msg_error_Maj, "Mise a jour");

            };
            service.ValiderMiseAJourBatchAsync(ListLot);
            service.CloseAsync();
        }

        private void Txt_NumeroLotDeb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_NumeroLotDeb.Text))
                {
                    this.Txt_NumeroLotDeb.Text = this.Txt_NumeroLotDeb.Text.PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0');
                    if (string.IsNullOrEmpty(this.Txt_NumeroLotFin.Text))
                        this.Txt_NumeroLotFin.Text = this.Txt_NumeroLotDeb.Text;
                }
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }
    }
}

