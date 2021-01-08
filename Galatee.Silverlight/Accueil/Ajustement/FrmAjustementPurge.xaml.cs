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
    public partial class FrmAjustementPurge : ChildWindow
    {
        public FrmAjustementPurge()
        {
            InitializeComponent();
        }
        List<CsOrigineLot> LstOrigine = new List<CsOrigineLot>();
        List<CsTypeLot> LstTypeLot = new List<CsTypeLot>();
        List<string > LstMoisCompte = new List<string >();
        List<CsLotCompteClient> LsDeLotCompte = new List<CsLotCompteClient>();
        List<CsLotCompteClient> _ListeDeLotCompte = new List<CsLotCompteClient>();


        CsCentre LeCentreSelect = new CsCentre();
        CsTypeLot LeTypeSelect = new CsTypeLot();
        string MoisSelect = string.Empty;
        private void ChargerTypeLot()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCodeControleCompleted += (s, args) =>
                {
                    List<CsCodeControle> LstCodeControle = new List<CsCodeControle>();
                    if (args != null && args.Cancelled)
                        return;
                    LstCodeControle.AddRange(args.Result);
                    if (LstCodeControle != null && LstCodeControle.Count != 0)
                    {
                        AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service1.RetourneTypeLotCompleted += (es, argss) =>
                        {
                            List<CsTypeLot> LstTypeLot = new List<CsTypeLot>();
                            if (argss != null && argss.Cancelled)
                                return;
                            LstTypeLot.AddRange(argss.Result);
                            if (LstTypeLot != null && LstTypeLot.Count != 0)
                            {
                                foreach (CsTypeLot item in LstTypeLot)
                                {
                                    CsCodeControle _LeCodeControle = LstCodeControle.FirstOrDefault(p => p.CODECONTROLE == item.CODECONTROLE);
                                    if (_LeCodeControle != null)
                                        item.LIBELLECODECONTROLE = _LeCodeControle.LIBELLE;

                                }
                                cbo_UpdateTypeBatchDeb.ItemsSource = null;
                                cbo_UpdateTypeBatchDeb.ItemsSource = LstTypeLot;
                                cbo_UpdateTypeBatchDeb.DisplayMemberPath = "LIBELLE";
                            }
                        };
                        service1.RetourneTypeLotAsync();
                        service1.CloseAsync();
                    }
                };
                service.RetourneCodeControleAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerOrigine()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneOrigineCompleted += (es, args) =>
            {
                List<CsOrigineLot> LstOrigine = new List<CsOrigineLot>();
                if (args != null && args.Cancelled)
                    return;
                LstOrigine.AddRange(args.Result);
                Cbo_UpdateOrigineDeb.ItemsSource = null;
                Cbo_UpdateOrigineDeb.ItemsSource = LstOrigine;
                Cbo_UpdateOrigineDeb.DisplayMemberPath = "LIBELLE";

                this.Cbo_UpdateOrigineDeb.ItemsSource = null;
                Cbo_UpdateOrigineDeb.ItemsSource = LstOrigine;
                Cbo_UpdateOrigineDeb.DisplayMemberPath = "LIBELLE";

                if (LstOrigine != null && LstOrigine.Count == 1)
                {
                    Cbo_UpdateOrigineDeb.SelectedItem = LstOrigine[0];
                    Cbo_UpdateOrigineDeb.SelectedItem = LstOrigine[0];
                    this.Cbo_UpdateOrigineDeb.IsEnabled = false;
                    this.Cbo_UpdateOrigineDeb.IsEnabled = false;

                }
            };
            service.RetourneOrigineAsync();
            service.CloseAsync();
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
                        if (LstMoisCompte.FirstOrDefault(p => p == item.MOISCOMPTABLE) == null)
                            LstMoisCompte.Add(item.MOISCOMPTABLE);
                    }
                }
                this.Cbo_MoisComptableDeb.ItemsSource = LstMoisCompte.OrderByDescending(p => p).ToList();
            };
            service1.RetourneListeDesTypeLotAsync(Origine, TypeLot);
            service1.CloseAsync();

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string _NumLotDeb = this.Txt_NumeroLotDeb.Text;
                string _NumLotFin = this.Txt_NumeroLotFin.Text;
                if (string.IsNullOrEmpty(_NumLotFin))
                    _ListeDeLotCompte = LsDeLotCompte.Where(p => (string.IsNullOrEmpty(_NumLotDeb) || p.NUMEROLOT == _NumLotDeb) &&
                                                                 (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE  ) &&
                                                                 (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                                 (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect) &&
                                                                 p.STATUS != "0").ToList();
                else if (!string.IsNullOrEmpty(_NumLotDeb) && !string.IsNullOrEmpty(_NumLotFin))
                    _ListeDeLotCompte = LsDeLotCompte.Where(p => (int.Parse(p.NUMEROLOT) >= int.Parse(_NumLotDeb)) &&
                                                                 (int.Parse(p.NUMEROLOT) <= int.Parse(_NumLotFin)) &&
                                                                 (LeCentreSelect == null || p.ORIGINE == LeCentreSelect.CODE) &&
                                                                 (LeTypeSelect == null || p.TYPELOT == LeTypeSelect.CODE) &&
                                                                 (string.IsNullOrEmpty(MoisSelect) || p.MOISCOMPTABLE == MoisSelect) &&
                                                                 p.STATUS != "0").ToList();
                if (_ListeDeLotCompte != null && _ListeDeLotCompte.Count != 0)
                {
                    this.Txt_NombreBatch.Text = _ListeDeLotCompte.Count.ToString();
                    this.Txt_NombreBatch.IsReadOnly = true;

                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu,this.Txt_NombreBatch.Text + "   Batchs found, do you want to delete then?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                            EpurerBach(LsDeLotCompte);
                    };
                    w.Show();
                }
                else
                    Message.ShowInformation("Aucun element trouvé", Langue.lbl_Menu);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EpurerBach(List<CsLotCompteClient> _LeLot)
        {
            try
            {
                List<CsLclient> ListeCompte = new List<CsLclient>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.PurgeLotCompleted += (es, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    this.DialogResult = true;
                };
                service.PurgeLotAsync(_LeLot);
            }
            catch (Exception ex)
            {
                throw ex ;
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
            ChargerOrigine();
            ChargerTypeLot();
        }

        private void Cbo_UpdateOrigine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             LeCentreSelect = (CsCentre )this.Cbo_UpdateOrigineDeb.SelectedItem;
        }

        private void cbo_UpdateTypeBatchDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeTypeSelect = (CsTypeLot)this.cbo_UpdateTypeBatchDeb.SelectedItem;
            LeCentreSelect = (CsCentre )this.Cbo_UpdateOrigineDeb.SelectedItem;
            ChargerCompteClient(LeCentreSelect.CODE , LeTypeSelect.CODE);
        }

        private void Cbo_MoisComptableDeb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MoisSelect = this.Cbo_MoisComptableDeb.SelectedValue.ToString();
        }

        private void Txt_NumeroLotDeb_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Txt_NumeroLotFin.Text = this.Txt_NumeroLotDeb.Text;
        }

        private void Txt_NumeroLotDeb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NumeroLotDeb.Text))
            {
                this.Txt_NumeroLotDeb.Text = this.Txt_NumeroLotDeb.Text.PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0');
                if (string.IsNullOrEmpty(this.Txt_NumeroLotFin.Text))
                    this.Txt_NumeroLotFin.Text = this.Txt_NumeroLotDeb.Text;
            }
        }
      
    }
}

