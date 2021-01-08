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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;


namespace Galatee.Silverlight.Accueil
{
    public partial class FrmAjustementSaisie : ChildWindow
    {
        public FrmAjustementSaisie()
        {
            InitializeComponent();
        }
        CsLotCompteClient LeLot;
        CsLclient _leClientLot;
        List<CsDetailLot> LeDetail = new List<CsDetailLot>();
        CsTypeLot _LeTypeLotSelect = new CsTypeLot();
        int? MaxIdLot = 0;

        bool IsLotExit = false;
        bool IsClientExist = false;
        bool IsClientSelect = false;
        CsNatgen Natgen;
        CsCoper  LeCoper;
        int InitValue = 0;
        int NbreDetail = 0;
        CsClient LeClient= new CsClient();
        List<CsLclient> LesFacturesClient = new List<CsLclient>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl1.SelectedItem == ogl_creation )
            {
                if (this.ogl_SaisieDetail.Visibility != System.Windows.Visibility.Visible)
                {
                    string _NumeroLot = this.Txt_NumeroBatch.Text;
                    string _TypeLot = ((CsTypeLot)this.cbo_TypeBatch.SelectedItem).CODE;
                    if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text ) && !string.IsNullOrEmpty(_NumeroLot)
                        && !string.IsNullOrEmpty(_TypeLot))
                        VerifieLotCompteClient(this.Txt_CodeCentre.Text, _NumeroLot, _TypeLot);
                    else
                        Message.ShowInformation(Langue.MsgChampOblige, Langue.lbl_Menu);
                    tabControl1.SelectedItem = ogl_SaisieDetail;
                }
                else
                {
                    tabControl1.SelectedItem = ogl_SaisieDetail;
                    return;
                }
               
            }
            if (tabControl1.SelectedItem == ogl_SaisieDetail )
            {
                decimal? MontantLot = 0;
                if (Dtg_ListeDetail.ItemsSource != null)
                {
                    foreach (CsDetailLot item in LeDetail)
                        MontantLot = MontantLot + item.MONTANT;

                    LeLot.MONTANT = MontantLot;
                    ValiderBacth(LeLot, LeDetail);
                    this.OKButton.Content = "Mise a jour";
                    this.DialogResult = true;
                }
                else
                {
                    
                    var w = new MessageBoxControl.MessageBoxChildWindow("Mise a jour", "", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                    };
                    w.Show();
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void translate()
        {

            //Gestion de la langue
            this.lbl_Annee.Content = Langue.lbl_annee;
            this.lbl_CentreClient.Content = Langue.lbl_center ;
            this.lbl_client.Content = Langue.lbl_client;
            this.lbl_coper.Content = Langue.lbl_coper;
            this.lbl_DateBatch.Content = Langue.lbl_DateBatch;
            this.lbl_datebatch1.Content = Langue.lbl_DateBatch;
            this.lbl_dateExigible.Content = Langue.lbl_dateExigible;
            this.lbl_datesaisie.Content = Langue.lbl_dateSaisie;
            this.lbl_DetailMontant.Content = Langue.lbl_MontantDetailBatch;
            this.lbl_Mois.Content = Langue.lbl_Mois;
            this.lbl_Montant.Content = Langue.lbl_Montant;
            this.lbl_montant1.Content = Langue.lbl_Montant;
            this.lbl_MontantBatch.Content = Langue.lbl_MontantBatch;
            this.lbl_NbrLigneDetails.Content = Langue.lbl_NbreLigneDetail;
            this.lbl_NumBatch.Content = Langue.lbl_NumBatch;
            this.lbl_Numlot1.Content = Langue.lbl_NumBatch;
            this.lbl_NumFact.Content = Langue.lbl_NumFact;
            this.lbl_ordre.Content = Langue.lbl_Ordre;
            this.lbl_Origine.Content = Langue.lbl_Origine;
            this.lbl_periode.Content = Langue.lbl_periode;
            this.lbl_type.Content =  Langue.lbl_type ;
            this.lbl_TypeBatch.Content =Langue.lbl_TypeBatch;
            this.lbl_periode.Content = Langue.lbl_periode;
            this.btn_Ajouter.Content = Langue.btn_ajouter;
            this.Btn_Delete.Content = Langue.btn_supprimer;
            this.btn_modifier.Content = Langue.btn_modifier;
            this.btn_Rechercher.Content = Langue.btn_search;
            this.Btn_Nouveau.Content = Langue.btn_nouveau ;
            //
            Dtg_ListeDetail.Columns[3].Header = Langue.lbl_periode;
            Dtg_ListeDetail.Columns[4].Header = Langue.lbl_NumFact;
            Dtg_ListeDetail.Columns[7].Header = Langue.lbl_dateSaisie;
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChargerDonneeDuSite();
            ChargerTypeLot();
            RetourneMaxIdLot();
            this.ogl_SaisieDetail.Visibility = System.Windows.Visibility.Collapsed;
            this.dtp_DateCreationLot.SelectedDateFormat = DatePickerFormat.Short;
            this.dtp_DateCreationLot.SelectedDate =  System.DateTime.Now;
            this.dtp_DateSaisiDetail.SelectedDate =  System.DateTime.Now;
            this.dtp_DateExigible.SelectedDate =  System.DateTime.Now;
            this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            this.Txt_Ordre.MaxLength = SessionObject.Enumere.TailleOrdre;
            this.Txt_NumeroBatch.MaxLength = SessionObject.Enumere.TailleNumeroLigneBatch;
            translate();
            this.OKButton.Content = "Suivant";
        }
        void initCtrl()
        {
            this.Txt_Client.Text= string.Empty ;
            this.Txt_Ordre.Text= string.Empty ;
            this.Txt_refem.Text= string.Empty;
            this.Txt_Ndoc.Text= string.Empty ;
            this.Txt_Montant.Text = string.Empty;
        }
        private void ChargerTypeLot()
        {
            try
            {
                if (SessionObject.ListeTypeLot != null && SessionObject.ListeTypeLot.Count != 0)
                {
                    List<CsTypeLot> LstTypeLot =  SessionObject.ListeTypeLot;
                    if (LstTypeLot != null && LstTypeLot.Count != 0)
                    {
                        cbo_TypeBatch.ItemsSource = null;
                        cbo_TypeBatch.ItemsSource = LstTypeLot;
                        cbo_TypeBatch.DisplayMemberPath = "LIBELLE";
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
                            cbo_TypeBatch.ItemsSource = null;
                            cbo_TypeBatch.ItemsSource = LstTypeLot;
                            cbo_TypeBatch.DisplayMemberPath = "LIBELLE";
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
                        this.Cbo_Centre.ItemsSource = null;
                        Cbo_Centre.ItemsSource = LstCentre.OrderBy(t=>t.LIBELLE);
                        Cbo_Centre.DisplayMemberPath = "LIBELLE";
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void ValiderBacth(CsLotCompteClient LeBatch, List<CsDetailLot> LeDetailLot)
        {
            CsSaisieDeMasse _LeLotDeSaisi = new CsSaisieDeMasse();
            _LeLotDeSaisi.LotCompteClient = LeBatch;
            _LeLotDeSaisi.LstDetailLot = LeDetailLot;
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.ValiderSaisieBactchCompleted += (sr, res) =>
            {
                var w = new MessageBoxControl.MessageBoxChildWindow("Mise a jour", Langue.MsgOperationTerminee, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                w.OnMessageBoxClosed += (_, result) =>
                {
                };
                w.Show();
            };
            service1.ValiderSaisieBactchAsync(_LeLotDeSaisi);
            service1.CloseAsync();
        }
        public void VerifieLotCompteClient(string Origine, string NumeroLot, string TypeLot)
        {
            List<CsLotCompteClient> ListDeLotInit = new List<CsLotCompteClient>();
            LeLot = new CsLotCompteClient();

            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDesTypeLotCompleted += (sr, res) =>
            {
                ListDeLotInit = new List<CsLotCompteClient>();
                if (res != null && res.Cancelled)
                    return;
                ListDeLotInit = res.Result;
                if (ListDeLotInit != null && ListDeLotInit.Count != 0)
                {
                    LeLot = ListDeLotInit.FirstOrDefault(p => p.NUMEROLOT == NumeroLot && p.ORIGINE == Origine);
                    if (LeLot != null)
                    {
                        if (LeLot.STATUS == "0")
                        {
                            IsLotExit = true;
                            RetourneDetailLot(LeLot.IDLOT);
                        }
                        else
                            Message.ShowInformation(Langue.msglotExit, "Saisi de lot");
                    }
                    else
                        RemplireDeuxiemeOnglet(IsLotExit);
                }
                else
                {
                    RemplireDeuxiemeOnglet(IsLotExit);
                    this.ogl_SaisieDetail.Visibility = System.Windows.Visibility.Visible;
                    this.tabControl1.SelectedItem = ogl_SaisieDetail;
                }

            };
            service1.RetourneListeDesTypeLotAsync(null ,null );
            service1.CloseAsync();
        }
        private void RetourneMaxIdLot()
        {
            List<CsLotCompteClient> ListDeLotInit = new List<CsLotCompteClient>();
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetourneMaxIDlotCompleted  += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                MaxIdLot  = res.Result;
            };
            service1.RetourneMaxIDlotAsync ();
            service1.CloseAsync();
        
        }
        private void RetourneDetailLot(int IdLot)
        {
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDesDetailLotCompleted  += (sr, res) =>
            {
                LeDetail = new List<CsDetailLot>();
                if (res != null && res.Cancelled)
                    return;
                LeDetail = res.Result;
                if (LeDetail != null)                
                    NbreDetail = LeDetail.Count;
                    
                
                RemplireDeuxiemeOnglet(IsLotExit);
            };
            service1.RetourneListeDesDetailLotAsync (IdLot);
            service1.CloseAsync();
        }

        private void RemplireDeuxiemeOnglet(bool IsLotExist)
        {
            try
            {
                decimal? MontantTotal = 0;
                if (IsLotExist)
                {
                    this.Txt_Origine.Text = LeLot.ORIGINE + "        " + LeLot.MOISCOMPTABLE + "         " + LeLot.TYPELOT;
                    this.Txt_Numlot2.Text = LeLot.NUMEROLOT;
                    this.Txt_BatchDate.Text = (LeLot.DATECREATION != null) ? LeLot.DATECREATION.ToString() :  System.DateTime.Now.ToShortDateString();
                    this.Txt_NombreClientBatch.Text = "0";
                    if (LeDetail != null && LeDetail.Count != 0)
                    {
                        foreach (CsDetailLot item in LeDetail)
                        {
                            MontantTotal = MontantTotal + item.MONTANT;
                            item.IsACTION = SessionObject.Enumere.IsUpdate;
                        }
                        this.Txt_BatchAmount.Text = LeLot.MONTANT.ToString();
                        this.Txt_MontantOnglet1.Text = MontantTotal.ToString();
                        this.Txt_MontantCourant.Text = MontantTotal.ToString();
                        this.Txt_NombreClientBatch.Text = LeDetail.Count.ToString();
                        Dtg_ListeDetail.ItemsSource = null;
                        Dtg_ListeDetail.ItemsSource = LeDetail;
                    }
                    LeLot.STATUS = "0";
                }
                else
                {
                   

                    string _NumeroLot = this.Txt_NumeroBatch.Text;
                    string _TypeLot = string.Empty;
                    CsTypeLot leLotSelect = new CsTypeLot();
                    if (this.cbo_TypeBatch.SelectedItem != null)
                        leLotSelect = ((CsTypeLot)this.cbo_TypeBatch.SelectedItem);

                   

                    this.Txt_Numlot2.Text = string.IsNullOrEmpty(this.Txt_NumeroBatch.Text) ? string.Empty : this.Txt_NumeroBatch.Text;
                    this.Txt_BatchAmount.Text = "0";
                    this.Txt_BatchDate.Text = string.IsNullOrEmpty(this.dtp_DateCreationLot.Text) ? string.Empty : dtp_DateCreationLot.Text;

                    LeLot = new CsLotCompteClient();
                    LeLot.TYPELOT = leLotSelect.CODE;
                    LeLot.DC  = leLotSelect.DC ;
                    LeLot.NUMEROLOT = _NumeroLot;
                    LeLot.DATECREATION =  System.DateTime.Now;
                    LeLot.MONTANT = 0;
                    LeLot.STATUS = "0";
                    LeLot.IDLOT = int.Parse((MaxIdLot + 1).ToString());
                    LeLot.USERCREATION = UserConnecte.matricule;
                    LeLot.USERMODIFICATION = UserConnecte.matricule;
                    LeLot.DATECREATION = System.DateTime.Now.Date;
                    LeLot.DATEMODIFICATION = System.DateTime.Now.Date;
                }
                this.Txt_Coper.Text = LeLot.TYPELOT.ToString().PadLeft(3, '0');
                this.Txt_Type.Text =Natgen!=null?(!string.IsNullOrWhiteSpace(Natgen.NATURE)?Natgen.NATURE:string.Empty):string.Empty;
                this.Txt_Type.Text ="01";
                this.OKButton.Content = "Creer";
                this.ogl_SaisieDetail.Visibility = System.Windows.Visibility.Visible;
                this.tabControl1.SelectedItem = ogl_SaisieDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void FillDataGrid(CsDetailLot _EltsFacture)
        {
            
                CsDetailLot _Elts = new CsDetailLot()
                {
                    IDLOT  = _EltsFacture.IDLOT,
                    NUMEROLIGNE = (LeDetail.Count != 0 ? (LeDetail.Max(p => int.Parse(p.NUMEROLIGNE) + 1)).ToString().PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0') : InitValue.ToString().PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0')),
                    CENTRE = _EltsFacture.CENTRE,
                    CLIENT = _EltsFacture.CLIENT,
                    ORDRE = _EltsFacture.ORDRE,
                    REFEM = _EltsFacture.REFEM,
                    NDOC = _EltsFacture.NDOC,
                    MONTANT = _EltsFacture.MONTANT,
                    COPER = _EltsFacture.COPER,
                    NATURE = _EltsFacture.NATURE,
                    MODEREG = _EltsFacture.MODEREG,
                    ACQUIT = _EltsFacture.ACQUIT,
                    DATEPIECE = _EltsFacture.DATEPIECE,
                    DATESAISIE = _EltsFacture.DATESAISIE,
                    ECART = _EltsFacture.ECART,
                    CODEERR = _EltsFacture.CODEERR,
                    SENS = _EltsFacture.SENS,
                    REFERENCE = _EltsFacture.REFERENCE,
                    MATRICULE = _EltsFacture.MATRICULE,
                    DATETRAIT = _EltsFacture.DATETRAIT,
                    REFEMNDOC = _EltsFacture.REFEMNDOC,
                    USERCREATION = _EltsFacture.USERCREATION ,
                    USERMODIFICATION =_EltsFacture.USERMODIFICATION ,
                    DATECREATION =_EltsFacture.DATECREATION ,
                    DATEMODIFICATION = _EltsFacture.DATEMODIFICATION 
                };
                _EltsFacture.IsACTION = SessionObject.Enumere.IsInsertion;

            LeDetail.Add(_EltsFacture);
            Dtg_ListeDetail.ItemsSource = null;
            List<CsDetailLot> _LstItemValide = LeDetail.Where(p => p.IsACTION != SessionObject.Enumere.IsDelete).ToList();
            Dtg_ListeDetail.ItemsSource = _LstItemValide;
            this.Txt_NombreClientBatch.Text = _LstItemValide.Count.ToString();
            decimal ? TotMontantCourant = 0;

            foreach (CsDetailLot item in _LstItemValide)
                TotMontantCourant = TotMontantCourant + item.MONTANT;
            this.Txt_MontantCourant.Text = TotMontantCourant.ToString();
        }
        void 
            UpdateDataGrid(CsDetailLot _EltsFacture)
        {
            CsDetailLot LaFactureRechercher = LeDetail.FirstOrDefault(p => p.IDLOT == _EltsFacture.IDLOT && p.NUMEROLIGNE == _EltsFacture.NUMEROLIGNE);
                if (LaFactureRechercher != null)
                {

                    LaFactureRechercher.CENTRE = _EltsFacture.CENTRE;
                    LaFactureRechercher.CLIENT = _EltsFacture.CLIENT;
                    LaFactureRechercher.ORDRE = _EltsFacture.ORDRE;
                    LaFactureRechercher.REFEM = string.IsNullOrEmpty(this.Txt_refem.Text) ? string.Empty : this.Txt_refem.Text;
                    LaFactureRechercher.NDOC = string.IsNullOrEmpty(this.Txt_Ndoc.Text) ? string.Empty : this.Txt_Ndoc.Text;
                    LaFactureRechercher.DATESAISIE = dtp_DateSaisiDetail.SelectedDate.Value.Date;
                    LaFactureRechercher.DATEPIECE = this.dtp_DateExigible.SelectedDate.Value.Date;
                    LaFactureRechercher.MONTANT = string.IsNullOrEmpty(this.Txt_Montant.Text) ? 0 : System.Convert.ToDecimal(this.Txt_Montant.Text);
                    LaFactureRechercher.COPER = _EltsFacture.COPER;
                    LaFactureRechercher.NATURE = _EltsFacture.NATURE;
                    LaFactureRechercher.MODEREG = _EltsFacture.MODEREG;
                    LaFactureRechercher.ACQUIT = _EltsFacture.ACQUIT;
                    LaFactureRechercher.ECART = _EltsFacture.ECART;
                    LaFactureRechercher.CODEERR = _EltsFacture.CODEERR;
                    LaFactureRechercher.SENS = _EltsFacture.SENS;
                    LaFactureRechercher.REFERENCE = _EltsFacture.REFERENCE;
                    LaFactureRechercher.MATRICULE = _EltsFacture.MATRICULE;
                    LaFactureRechercher.DATETRAIT = _EltsFacture.DATETRAIT;
                    LaFactureRechercher.REFEMNDOC = _EltsFacture.REFEMNDOC;

                }
            
            Dtg_ListeDetail.ItemsSource = null;
            List<CsDetailLot> _LstItemValide = LeDetail.Where(p => p.IsACTION != SessionObject.Enumere.IsDelete).ToList();
            Dtg_ListeDetail.ItemsSource = _LstItemValide;
            this.Txt_NombreClientBatch.Text = _LstItemValide.Count.ToString();
            decimal? TotMontantCourant = 0;
             
            foreach (CsDetailLot item in _LstItemValide)
                TotMontantCourant = TotMontantCourant + item.MONTANT;
            this.Txt_MontantCourant.Text = TotMontantCourant.ToString();
        }
        private void CreeObjetDetaile()
        {
            CsDetailLot _EltsFacture = new CsDetailLot();
            _EltsFacture.IDLOT = LeLot.IDLOT;
            _EltsFacture.TYPELOT  = LeLot.TYPELOT ; 
            _EltsFacture.CENTRE = ((CsCentre )Cbo_Centre.SelectedItem).CODE   ;
            _EltsFacture.CLIENT = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
            _EltsFacture.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
            _EltsFacture.REFEM = string.IsNullOrEmpty(this.Txt_refem.Text) ? string.Empty : this.Txt_refem.Text;
            _EltsFacture.NDOC = string.IsNullOrEmpty(this.Txt_Ndoc.Text) ? string.Empty : this.Txt_Ndoc.Text;
            _EltsFacture.DATESAISIE = dtp_DateSaisiDetail.SelectedDate.Value.Date;
            _EltsFacture.DATEPIECE = this.dtp_DateExigible.SelectedDate.Value.Date; 
            _EltsFacture.MONTANT  = string.IsNullOrEmpty(this.Txt_Montant .Text) ? 0 : System.Convert.ToDecimal(this.Txt_Montant .Text);
            _EltsFacture.COPER = ((CsTypeLot)this.cbo_TypeBatch.SelectedItem).CODE ;
            _EltsFacture.NATURE = string.IsNullOrEmpty(this.Txt_Type.Text) ? string.Empty : this.Txt_Type.Text;
            _EltsFacture.MODEREG = SessionObject.Enumere.ModePayementAjustement;
            if (_LeTypeLotSelect != null && !string.IsNullOrEmpty(_LeTypeLotSelect.DC))
                _EltsFacture.SENS = _LeTypeLotSelect.DC;
            _EltsFacture.NUMEROLIGNE  = NbreDetail.ToString().PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0');
            _EltsFacture.ECART = 0;
            _EltsFacture.USERCREATION = UserConnecte.matricule;
            _EltsFacture.MATRICULE  = UserConnecte.matricule;
            _EltsFacture.USERMODIFICATION = UserConnecte.matricule;
            _EltsFacture.DATECREATION = System.DateTime.Now.Date;
            _EltsFacture.DATEMODIFICATION  = System.DateTime.Now.Date;
            _EltsFacture.ECART = 0;
            _EltsFacture.FK_IDMATRICULE = UserConnecte.PK_ID ;
            _EltsFacture.FK_IDCENTRE = _leClientLot.FK_IDCENTRE;
            _EltsFacture.FK_IDCLIENT  = _leClientLot.FK_IDCLIENT;
            _EltsFacture.FK_IDCOPER = ((CsTypeLot)this.cbo_TypeBatch.SelectedItem).FK_IDCOPER;

             FillDataGrid(_EltsFacture);
            initCtrl();
        }
        private void UpdateObjetDetaile(CsDetailLot _EltsFacture)
        {
            _EltsFacture.IDLOT = LeLot.IDLOT;
            _EltsFacture.CENTRE = ((CParametre)Cbo_Centre.SelectedItem).CODE ;
            _EltsFacture.CLIENT = string.IsNullOrEmpty(this.Txt_Client.Text) ? string.Empty : this.Txt_Client.Text;
            _EltsFacture.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;
            _EltsFacture.REFEM = string.IsNullOrEmpty(this.Txt_refem.Text) ? string.Empty : this.Txt_refem.Text;
            _EltsFacture.NDOC = string.IsNullOrEmpty(this.Txt_Ndoc.Text) ? string.Empty : this.Txt_Ndoc.Text;
            _EltsFacture.DATESAISIE = dtp_DateSaisiDetail.SelectedDate.Value.Date;
            _EltsFacture.DATEPIECE = this.dtp_DateExigible.SelectedDate.Value.Date;
            _EltsFacture.MONTANT = string.IsNullOrEmpty(this.Txt_Montant.Text) ? 0 : System.Convert.ToDecimal(this.Txt_Montant.Text);
            _EltsFacture.COPER = string.IsNullOrEmpty(this.Txt_Coper.Text) ? string.Empty : this.Txt_Coper.Text;
            _EltsFacture.NATURE = string.IsNullOrEmpty(this.Txt_Type.Text) ? string.Empty : this.Txt_Type.Text;
            _EltsFacture.SENS = (LeCoper != null && !string.IsNullOrEmpty(LeCoper.CODE )) ? LeCoper.DC : string.Empty;

            UpdateDataGrid(_EltsFacture);
            initCtrl();


        }
        private void cbo_TypeBatch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }
        private void RetourneNature(string Coper)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneNatureParCoperCompleted  += (s, args) =>
            {
                Natgen = new CsNatgen();
                if (args != null && args.Cancelled)
                    return;
                Natgen = args.Result;
            };
            service.RetourneNatureParCoperAsync(Coper);
            service.CloseAsync();
        }

        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)    
        {
            LoadCompteClient(false);
        }

        private void LoadCompteClient(bool IsbtnRecherche)
        {
            try
            {
                CsCentre LeCentreSelect = (CsCentre)this.Cbo_Centre.SelectedItem;
                if (!string.IsNullOrEmpty(Txt_Client.Text) && this.Txt_Client.Text.Length == SessionObject.Enumere.TailleClient &&
                    !string.IsNullOrEmpty(this.Txt_Ordre.Text) && this.Txt_Ordre.Text.Length == SessionObject.Enumere.TailleOrdre)
                {
                    if (IsbtnRecherche && !string.IsNullOrEmpty(this.Txt_refem.Text) && this.Txt_refem.Text.Length == SessionObject.Enumere.TaillePeriode &&
                         !string.IsNullOrEmpty(this.Txt_Ndoc.Text) && this.Txt_Ndoc.Text.Length == SessionObject.Enumere.TailleFacture)
                        return;

                    VerifieExisteFacture(LeCentreSelect.PK_ID ,LeCentreSelect.CODE, this.Txt_Client.Text, this.Txt_Ordre.Text, this.Txt_refem.Text, this.Txt_Ndoc.Text, IsbtnRecherche);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(Langue.MsgFactureNonTrouve, Langue.lbl_Menu);
            }
        }

        //Recupération du compte client contenant la liste des transcaiss(Encaissement) en credit,la liste des Lclients(factures) en debit et la liste de toute les (Lclient)facture 
        //void  VerifieExisteFacture(int fk_idcentre,string centre, string client,string Ordre ,string refem,string Ndoc,bool IsRechercher)
        //{


        //    if (((CsTypeLot)cbo_TypeBatch.SelectedItem).DC == "D")
        //    { 
            
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //    service.RetourneListeFactureClientCompleted   += (s, args) =>
        //    {
        //        ToutLeCompteClientRecherche = new List<CsLclient>();
        //        if (args != null && args.Cancelled)
        //            return;
        //        ToutLeCompteClientRecherche = args.Result;
        //        if (ToutLeCompteClientRecherche != null && ToutLeCompteClientRecherche.Count != 0)
        //        {

        //            if (!IsRechercher)
        //            {
        //                //S'il y'à une facture en debit correspondant au critères charger info dans la grille du UI
        //                if (ToutLeCompteClientRecherche.FirstOrDefault(p => p.CENTRE == centre &&
        //                                                                 p.CLIENT == client &&
        //                                                                 p.ORDRE == Ordre &&
        //                                                                 p.REFEM == refem &&
        //                                                                 p.NDOC == Ndoc) != null)
        //                {
        //                    CsDetailLot _LeDetail = LeDetail.FirstOrDefault(p => p.CENTRE == centre &&
        //                                                    p.CLIENT == client &&
        //                                                    p.ORDRE == Ordre &&
        //                                                    p.REFEM == refem &&
        //                                                    p.NDOC == Ndoc &&
        //                                                    p.IsACTION != SessionObject.Enumere.IsDelete);
        //                    if (_LeDetail == null)
        //                        CreeObjetDetaile();
        //                    else
        //                        UpdateDataGrid(_LeDetail);
        //                }
        //                else
        //                    Message.ShowError(Langue.MsgFactureNonTrouve, Langue.lbl_Menu);
        //            }
        //            else
        //            {
        //                UcAjustementRecherche Ctrl = new UcAjustementRecherche(ToutLeCompteClientRecherche);
        //                Ctrl.Closed += new EventHandler(galatee_OkClicked);
        //                Ctrl.Show();
        //            }
        //        }
        //        else
        //        {
        //            Message.ShowError(Langue.MsgFactureNonTrouve, Langue.lbl_Menu);
        //        }
        //    };
        //    service.RetourneListeFactureClientAsync(fk_idcentre,centre, client, Ordre);
        //    service.CloseAsync();
            
        //    }
        //    else
        //    { }
        //}

        //Recupération du compte client contenant la liste des transcaiss(Encaissement) en credit,la liste des Lclients(factures) en debit et la liste de toute les (Lclient)facture 
        void VerifieExisteFacture(int fk_idcentre, string centre, string client, string Ordre, string refem, string Ndoc, bool IsRechercher)
        {
            if (LesFacturesClient != null && LesFacturesClient.Count != 0)
            {
                CsDetailLot _LeDetail = LeDetail.FirstOrDefault(p => p.CENTRE == centre &&
                                                p.CLIENT == client &&
                                                p.ORDRE == Ordre &&
                                                p.REFEM == refem &&
                                                p.NDOC == Ndoc &&
                                                p.IsACTION != SessionObject.Enumere.IsDelete);
                if (_LeDetail == null)
                    CreeObjetDetaile();
                else
                    UpdateDataGrid(_LeDetail); 
            }
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
        private void Txt_Montant_LostFocus(object sender, RoutedEventArgs e)
        {
            //CParametre LeCentreSelect = (CParametre)this.Cbo_Centre.SelectedItem;
            //if (!string.IsNullOrEmpty(Txt_Client.Text) && this.Txt_Client.Text.Length == SessionObject.Enumere.TailleCentre &&
            //    !string.IsNullOrEmpty(this.Txt_Ordre.Text) && this.Txt_Ordre .Text.Length == SessionObject.Enumere.TailleOrdre  
            //    &&
            //    !string.IsNullOrEmpty(this.Txt_refem.Text) && this.Txt_refem .Text.Length == SessionObject.Enumere.TaillePeriode  &&
            //    !string.IsNullOrEmpty(this.Txt_Ndoc.Text) && this.Txt_Ndoc.Text.Length == SessionObject.Enumere.TailleFacture
            //    ) 
               //VerifieExisteFacture(LeCentreSelect.VALEUR, this.Txt_Client.Text, this.Txt_Ordre.Text , this.Txt_refem.Text, this.Txt_Ndoc.Text);
        }
        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Dtg_ListeDetail.SelectedItem != null)
            {
                CsDetailLot _detLot = (CsDetailLot)Dtg_ListeDetail.SelectedItem;
                CsDetailLot _leDetait = LeDetail.FirstOrDefault(p => p.NUMEROLIGNE == _detLot.NUMEROLIGNE);
                _leDetait.IsACTION = SessionObject.Enumere.IsDelete;

                Dtg_ListeDetail.ItemsSource = null;
                Dtg_ListeDetail.ItemsSource = LeDetail.Where(p => p.IsACTION != SessionObject.Enumere.IsDelete);
                initCtrl();
            }
        }
        private void Dtg_ListeDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsClientSelect = true;
            CsDetailLot _LafactureSelect = (CsDetailLot)this.Dtg_ListeDetail.SelectedItem;
            if (_LafactureSelect != null)
                RenseigneChamp(_LafactureSelect);
        }
        private  void RenseigneChamp(CsDetailLot _LeDetailSelectionne)
        {
            this.Txt_Client.Text = _LeDetailSelectionne.CLIENT;
            this.Txt_Ordre.Text = _LeDetailSelectionne.ORDRE;
            this.Txt_refem.Text = _LeDetailSelectionne.REFEM;
            this.Txt_Ndoc.Text = _LeDetailSelectionne.NDOC;
            this.dtp_DateSaisiDetail.Text = _LeDetailSelectionne.DATESAISIE.ToString();
            this.Txt_Montant.Text = _LeDetailSelectionne.MONTANT.ToString();
        }
        private void Btn_Nouveau_Click(object sender, RoutedEventArgs e)
       {
           this.Dtg_ListeDetail.SelectedItem = null;
           initCtrl();
       }
        private void Txt_Ndoc_LostFocus(object sender, RoutedEventArgs e)
       {
           //if (ToutLeCompteClientRecherche != null && ToutLeCompteClientRecherche.Count != 0 && ToutLeCompteClientRecherche.FirstOrDefault(t => t.CLIENT == this.Txt_Client.Text && t.ORDRE == this.Txt_Ordre.Text) != null)
           //    LoadCompteClient(true);
       }
        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CsCentre leCentreSelect = (CsCentre)this.Cbo_Centre.SelectedItem;
                CsTypeLot leTypeSelect = (CsTypeLot)this.cbo_TypeBatch.SelectedItem;
                RetourneCompteFromCoper(leTypeSelect, leCentreSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetourneCompteFromCoper(CsTypeLot leTypeDuLot, CsCentre leCentreSelect)
        {
            try
            {
                #region Facture
                if (leTypeDuLot.DC == SessionObject.Enumere.Debit)
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneListeFactureClientCompleted += (es, argss) =>
                    {
                        if (argss != null && argss.Cancelled)
                            return;
                        LesFacturesClient = argss.Result;
                        if (LesFacturesClient != null && LesFacturesClient.Count != 0)
                        {
                            UcAjustementRecherche Ctrl = new UcAjustementRecherche(LesFacturesClient);
                            Ctrl.Closed += new EventHandler(galatee_OkClicked);
                            Ctrl.Show();
                        }
                    };
                    service1.RetourneListeFactureClientAsync(leCentreSelect.PK_ID, leCentreSelect.CODE, this.Txt_Client.Text, this.Txt_Ordre.Text);
                    service1.CloseAsync();
                }
                #endregion
                #region reglement
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneListeReglementCompleted += (es, argss) =>
                    {
                        if (argss != null && argss.Cancelled)
                            return;
                        LesFacturesClient = argss.Result;
                        if (LesFacturesClient != null && LesFacturesClient.Count != 0)
                        {
                            UcAjustementRecherche Ctrl = new UcAjustementRecherche(LesFacturesClient);
                            Ctrl.Closed += new EventHandler(galatee_OkClicked);
                            Ctrl.Show();
                        }
                        else
                        {
                            Message.Show(Langue.MsgFactureNonTrouve, "Recherche client");
                            return;
                        }
                    };
                    service1.RetourneListeReglementAsync(leCentreSelect.PK_ID, leCentreSelect.CODE, this.Txt_Client.Text, this.Txt_Ordre.Text);
                    service1.CloseAsync();
                #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        //private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        //{
        //    //Utiliser ToutLeCompteClientRecherche pour charger les facture du client
        //    ToutLeCompteClientDeOrientation = new List<CsLclient>();
        //    if (((CsTypeLot)cbo_TypeBatch.SelectedItem).DC=="D")
        //    {
        //        if (ToutLeCompteClientRecherche != null && 
        //            ToutLeCompteClientRecherche.FirstOrDefault(p=>p.CLIENT == this.Txt_Client.Text && p.NDOC == this.Txt_Ordre.Text )!= null )
        //        {
        //            if (ToutLeCompteClientRecherche.Count() > 0)
        //              foreach (var item in ToutLeCompteClientRecherche)
        //                {
        //                    CsLclient facture = new CsLclient();
                        
        //                        //ACQUITANTERIEUR =item.ACQUIT!=null?item.ACQUIT:string.Empty,
        //                        facture.NDOC = !string.IsNullOrWhiteSpace(item.NDOC) ? item.NDOC : string.Empty;
        //                        facture.REFEM = !string.IsNullOrWhiteSpace(item.REFEM) ? item.REFEM : string.Empty;
        //                        facture.COPER = !string.IsNullOrWhiteSpace(item.COPER) ? item.COPER : string.Empty;
        //                        facture.NATURE = !string.IsNullOrWhiteSpace(item.NATURE) ? item.NATURE : string.Empty;
        //                        facture.DATECREATION = item.DATECREATION;
        //                        facture.DC = !string.IsNullOrWhiteSpace(item.DC) ? item.DC : string.Empty;
        //                        facture.MONTANT  = item.MONTANT != null ? item.MONTANT.Value : decimal.MinValue;
        //                        facture.CENTRE = item.CENTRE;
        //                        facture.CLIENT = item.CLIENT;
        //                        facture.ORDRE = item.ORDRE;
        //                    ToutLeCompteClientDeOrientation.Add(facture);
        //                }
        //            if (ToutLeCompteClientDeOrientation != null && ToutLeCompteClientDeOrientation.Count != 0)
        //            {
        //                UcAjustementRecherche Ctrl = new UcAjustementRecherche(ToutLeCompteClientDeOrientation);
        //                Ctrl.Closed += new EventHandler(galatee_OkClicked);
        //                Ctrl.Show();
        //            }
        //        }
        //        else
        //        {
        //            LoadCompteClient(true);
        //        }
        //    }
        //    else
        //    {
        //        if (ToutLeCompteClientRecherche != null)
        //        {
        //            if (ToutLeCompteClientRecherche.Count() > 0)
        //                foreach (var item in ToutLeCompteClientRecherche)
        //                {
        //                    CsLclient facture = new CsLclient();
        //                    facture.NDOC = !string.IsNullOrWhiteSpace(item.NDOC) ? item.NDOC : string.Empty;
        //                    facture.REFEM = !string.IsNullOrWhiteSpace(item.REFEM) ? item.REFEM : string.Empty;
        //                    facture.COPER = !string.IsNullOrWhiteSpace(item.COPER) ? item.COPER : string.Empty;
        //                    facture.NATURE = !string.IsNullOrWhiteSpace(item.NATURE) ? item.NATURE : string.Empty;
        //                    facture.DATECREATION = item.DATECREATION;
        //                    facture.DC = !string.IsNullOrWhiteSpace(item.DC) ? item.DC : string.Empty;
        //                    facture.MONTANT  = item.MONTANT != null ? item.MONTANT.Value : decimal.MinValue;
        //                    facture.CENTRE = item.CENTRE;
        //                    facture.CLIENT = item.CLIENT;
        //                    facture.ORDRE = item.ORDRE;
        //                    ToutLeCompteClientDeOrientation.Add(facture);
        //                }
        //            if (ToutLeCompteClientDeOrientation != null && ToutLeCompteClientDeOrientation.Count != 0)
        //            {
        //                UcAjustementRecherche Ctrl = new UcAjustementRecherche(ToutLeCompteClientDeOrientation);
        //                Ctrl.Closed += new EventHandler(galatee_OkClicked);
        //                Ctrl.Show();
        //            }
        //        }
        //        else
        //        {
        //            LoadCompteClient(true);
        //        }
        //    }
           
        //}
        void galatee_OkClicked(object sender, EventArgs e)
        {
            UcAjustementRecherche ctrs = sender as UcAjustementRecherche;
            _leClientLot = new CsLclient();
             _leClientLot = ctrs.MyClientDetailLot;
            if (_leClientLot != null)
            {
                this.Txt_Client.Text = _leClientLot.CLIENT;
                this.Txt_Ordre .Text = _leClientLot.ORDRE ;
                this.Txt_refem .Text = _leClientLot.REFEM ;
                this.Txt_Ndoc .Text = _leClientLot.NDOC ;
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void Txt_Client_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private void Txt_Ordre_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Txt_refem_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Txt_Ndoc_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ogl_creation!=null)
            {
                if (tabControl1.SelectedItem == ogl_creation)
                {
                    this.OKButton.Content = "Suivant";
                    if (LeDetail != null && LeDetail.Where(p => p.IsACTION != SessionObject.Enumere.IsDelete).ToList().Count != 0)
                    {
                        this.cbo_TypeBatch.IsEnabled = false;
                        this.Txt_NumeroBatch.IsReadOnly = true;
                        this.Txt_MontantOnglet1.Text = LeDetail.Where(p => p.IsACTION != SessionObject.Enumere.IsDelete).Sum(t => t.MONTANT).Value.ToString(SessionObject.FormatMontant);
                    }
                    else
                    {
                        this.cbo_TypeBatch.IsEnabled = true;
                        this.Txt_NumeroBatch.IsReadOnly = false;
                    }


                }
            }

            if (ogl_SaisieDetail != null)
            {
                if (tabControl1.SelectedItem == ogl_SaisieDetail)
                {
                    this.OKButton.Content = "Créer";
                    if (LeDetail != null && LeDetail.Count != 0)
                        Cbo_Centre.SelectedItem = LstCentre.FirstOrDefault(t => t.PK_ID == LeDetail.Last().FK_IDCENTRE);
                }
            }
            
        }

        private void Txt_NumeroBatch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_NumeroBatch .Text))
                this.Txt_NumeroBatch.Text = this.Txt_NumeroBatch.Text.PadLeft(SessionObject.Enumere.TailleNumeroLigneBatch, '0');
        }

        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            //LoadCompteClient(false);
        }

        private void Txt_Ordre_LostFocus(object sender, RoutedEventArgs e)
        {
            //LoadCompteClient(false);
        }

        private void Txt_refem_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (ToutLeCompteClientRecherche != null && ToutLeCompteClientRecherche.Count != 0 && ToutLeCompteClientRecherche.FirstOrDefault(t => t.CLIENT == this.Txt_Client.Text && t.ORDRE == this.Txt_Ordre.Text) != null)
            //    LoadCompteClient(true);
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += ctr_Closed;
                this.IsEnabled = false;
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void ctr_Closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                CsSite _LeSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = _LeSite.CODE;
                this.Txt_CodeCentre.Text = string.Empty;
                this.Txt_CodeCentre.Tag = null;
                this.Txt_LibelleCentre.Text = string.Empty;
                LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSite.PK_ID).ToList();
                if (LstCentre != null && LstCentre.Count == 1)
                {
                    this.Txt_CodeCentre.Text = LstCentre.First().CODE;
                    this.Txt_LibelleCentre.Text = LstCentre.First().LIBELLE;
                    this.Txt_CodeCentre.Tag = LstCentre.First();
                }
            }
        }

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))
                    {
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;
                        LstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == _LeSiteClient.PK_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkCentreClicked);
                this.IsEnabled = false;
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void galatee_OkCentreClicked(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCentre _LaCateg = (CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = _LaCateg.CODE;
                this.Txt_CodeCentre.Tag = _LaCateg;
            }
        }
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        Txt_CodeCentre.Tag = _LeCentreClient;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }
    }
}

