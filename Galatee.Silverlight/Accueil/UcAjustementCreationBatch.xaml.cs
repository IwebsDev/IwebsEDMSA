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
using System.Collections.ObjectModel;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.Classes;



namespace Galatee.Silverlight.Accueil
{
    public partial class UcAjustementCreationBatch : UserControl
    {
        public UcAjustementCreationBatch()
        {
            InitializeComponent();
        }
        bool LotExit = false;
        public bool  EtatMonLot
        {
            get { return LotExit; }
            set { LotExit = value; }
        }
        CsLotCompteClient LeLot = new CsLotCompteClient();
        List<CsLotCompteClient> ListDeLot = new  List<CsLotCompteClient>();
        public CsLotCompteClient MonLot
        {
            get { return LeLot; }
            set { LeLot = value; }
        }
        private void ChargerTypeLot()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneCodeControleCompleted += (s, args) =>
            {
                List<CsCodeControle> LstCodeControle = new List<CsCodeControle>();
                if (args != null && args.Cancelled)
                    return;
                LstCodeControle.AddRange(args.Result);
                if (LstCodeControle != null && LstCodeControle.Count != 0)
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                                CsCodeControle  _LeCodeControle = LstCodeControle.First(p => p.CODECONTROLE  == item.CODECONTROLE);
                                if (_LeCodeControle != null)
                                    item.LIBELLECODECONTROLE   = _LeCodeControle.LIBELLE;
                            }
                            cbo_TypeBatch.ItemsSource = null;
                            cbo_TypeBatch.ItemsSource = LstTypeLot;
                            cbo_TypeBatch.DisplayMemberPath = "LIBELLE";
                        }
                    };
                    service1.RetourneTypeLotAsync();
                    service1.CloseAsync();
                }
            };
            service.RetourneCodeControleAsync();
            service.CloseAsync();
        }
        void Translate()
        {
            this.lbl_TypeBatch.Content = Langue.lbl_TypeBatch;
            this.lbl_Origine.Content = Langue.lbl_Origine;
            this.lbl_Caisse.Content = Langue.lbl_Caisse;
            this.lbl_NumBatch.Content = Langue.lbl_NumBatch;
            this.lbl_Controle.Content = Langue.lbl_Controle;
            this.lbl_Montant.Content = Langue.lbl_MontantBatch;
            this.lbl_MoisCompta.Content = Langue.lbl_MoisComptable;
            this.lbl_DateBatch.Content = Langue.lbl_DateBatch;
            this.btn_AutreInfo.Content = Langue.lbl_autreInfo; 
        }
        private  void ChargerOrigine()
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourneOrigineCompleted += (es, args) =>
            {
                List<CsOrigineLot> LstOrigine = new List<CsOrigineLot>();
                if (args != null && args.Cancelled)
                    return;
                LstOrigine.AddRange(args.Result);
                Cbo_Origine.ItemsSource = null;
                Cbo_Origine.ItemsSource = LstOrigine;
                Cbo_Origine.DisplayMemberPath = "LIBELLE";

            };
            service.RetourneOrigineAsync();
            service.CloseAsync();
        }
        public void VerifieLotCompteClient(string Origine,string NumeroLot,string TypeLot)
        {
            
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDesTypeLotCompleted  += (sr, res) =>
            {
                LeLot = new CsLotCompteClient();
                ListDeLot = new List<CsLotCompteClient>();
                if (res != null && res.Cancelled)
                    return;
                ListDeLot = res.Result;
                if (ListDeLot != null && ListDeLot.Count != 0)
                {
                    LeLot = ListDeLot.First(p => p.NUMEROLOT  == NumeroLot);
                    if (LeLot != null)
                    LotExit = true;
                }

            };
            service1.RetourneListeDesTypeLotAsync(Origine, TypeLot);
            service1.CloseAsync();
        
        }
        private void cbo_TypeBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CsTypeLot _LeTypeLotSelect = (CsTypeLot)this.cbo_TypeBatch.SelectedItem;
            if (_LeTypeLotSelect != null)
                this.Txt_CodeControle.Text = _LeTypeLotSelect.LIBELLECODECONTROLE;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerOrigine();
            ChargerTypeLot();
            this.Txt_NumCaisse.Text = UserConnecte.numcaisse;
            this.Txt_NumCaisse.IsReadOnly = true;
            this.Cbo_MoisComptable.ItemsSource = ClasseMEthodeGenerique.RetourneListeDesMoisComptable();
        }
    }
}
