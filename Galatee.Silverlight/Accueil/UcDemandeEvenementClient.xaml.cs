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
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeEvenementClient : UserControl
    {
        public  CsDemande LaDemande = new CsDemande();
        List<CParametre> LstBranche = new List<CParametre>();
        List<CParametre> LstQuartier = new List<CParametre>();
        List<CsEvenement> LsDernierEvenement = new List<CsEvenement>();
        List<CsEvenement> LstEvenement = new List<CsEvenement>();
        List<CsEvenement> LstEvenementCree = new List<CsEvenement>();
        List<CsPagisol> LstPagisolCree = new List<CsPagisol>();
        CsPagisol LePagisolCree = new CsPagisol();
        CsCanalisation LeCompteurSelect ;
        CsEvenement LeEvenementSelect;
        List<CsLclient > LeEvenementRemboursementAvance;
        int? IndexInit = 0;
        int MaxNumEvt = 0;
        DateTime? DateDernierEvt = null;
        bool IsSupprimerEvtNonSaisie = false;
        CsEvenement _LeEvtNonSaisie = new CsEvenement();
        public List<CsEvenement> MonEvenement
        {
            get { return LsDernierEvenement; }
            set { LsDernierEvenement = value; }
        }

        List<CsCanalisation> CanalisationClientRecherche = new List<CsCanalisation>();
        public List<CsCanalisation> MaCanalisation
        {
            get { return CanalisationClientRecherche; }
            set { CanalisationClientRecherche = value; }
        }
        List<CsCasind> LsDesCas = new List<CsCasind>();
        CsAbon AbonementRecherche = new CsAbon();
        CsClient ClientRecherche = new CsClient();
        DateTime? DateResil = null;
        string Periode = string.Empty;

        int indexSelect = 0;
        public UcDemandeEvenementClient()
        {
            InitializeComponent();
        }
        public UcDemandeEvenementClient(CsDemande _LaDemande)
        {
            try
            {
                InitializeComponent();
                LaDemande = _LaDemande;
                //Initctrl();
 
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        void Initctrl()
        {
            try
            {
               if (LaDemande.LstCanalistion == null) LaDemande.LstCanalistion = new List<CsCanalisation>();

                this.Txt_PeriodeEnCour.MaxLength = 7;
                this.Txt_CasEnCour.MaxLength = SessionObject.Enumere.TailleCas;
                this.checkBox1.IsChecked = true;
                #region REABONEMENT
                if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.Reabonnement)
                {
                }
                #endregion
                #region RESILIATION
                if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.Resiliation)
                {
                    LePagisolCree = new CsPagisol();
                    this.rdb_PasRetraitCompteur.IsChecked = true;
                    RetourneInfoCanalisation(LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT, null);
                    RetourneListeDesCas();
                    DateResil = LaDemande.Abonne.DRES;
                    RetourneInfoAbon(LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.PRODUIT);
                    this.Txt_FinPeriode.Text = ClasseMEthodeGenerique.DernierJourDuMois(int.Parse(LaDemande.Abonne.DRES.ToString().Substring(3, 2)), int.Parse(LaDemande.Abonne.DRES.ToString().Substring(6, 4)));
                    this.Txt_DebutPeriode.Text = "01" + "/" + LaDemande.Abonne.DRES.ToString().Substring(3, 2).PadLeft(2, '0') + "/" + LaDemande.Abonne.DRES.ToString().Substring(6, 4);
                    this.Txt_PeriodeEnCour.Text =  LaDemande.Abonne.DRES.ToString().Substring(3, 2).PadLeft(2, '0') + "/" + LaDemande.Abonne.DRES.ToString().Substring(6, 4);
                }
                #endregion
                #region FACTURE MANUELLE
                if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.FactureManuelle ||
                    LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.AvoirConsomation)
                {
                    LePagisolCree = new CsPagisol();
                    RetourneListeDesCas();
                    RetourneInfoCanalisation(LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.PRODUIT, null);
                    RetourneInfoAbon(LaDemande.LaDemande.FK_IDCENTRE, LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE, LaDemande.LaDemande.PRODUIT);
                    RetourneInfoClient (LaDemande.LaDemande.FK_IDCENTRE ,LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
                }
                #endregion
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        private void RetourneListeDesCas()
        {
            if (SessionObject.LsDesCas.Count != 0)
            {
                LsDesCas = SessionObject.LsDesCas;
                return;
            }
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneListeDesCasCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LsDesCas = args.Result;
                 SessionObject.LsDesCas = LsDesCas ;

            };
            service.RetourneListeDesCasAsync();
            service.CloseAsync();
        }
        private void RetourneInfoCanalisation(int fk_idcentre,string centre, string client,string produit,int? point)
        {
            CanalisationClientRecherche = new List<CsCanalisation>();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneCanalisationCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                CanalisationClientRecherche = args.Result;
                if (CanalisationClientRecherche.Count != 0)
                {
                    foreach (CsCanalisation item in CanalisationClientRecherche)
                        item.ORDRE   = LaDemande.LaDemande.ORDRE ;

                    RetourneEvenementCanalisation(CanalisationClientRecherche);
                }
            };
            service.RetourneCanalisationAsync(fk_idcentre,centre, client, produit, point);
            service.CloseAsync();

        }
        public static int[] StatusEvenementNonFacture()
        {
            int[] strArray =
                {
                 SessionObject.Enumere.EvenementCree ,SessionObject.Enumere.EvenementDefacture,SessionObject.Enumere.EvenementAnnule ,SessionObject.Enumere.EvenementSupprimer
                };
            return strArray;

        }
        private void RetourneEvenementCanalisation(List<CsCanalisation> LstCanalisation)
        {
            try
            {
                LstEvenement = new List<CsEvenement>();
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneEvenementDeLaCanalisationCompleted += (s, args) =>
                {
                    try
                    {
                       int[] LstStatut = StatusEvenementNonFacture();
                        if (args != null && args.Cancelled)
                            return;
                        LstEvenement = args.Result;
                        MaxNumEvt = LstEvenement.Max(t => t.NUMEVENEMENT);

                        _LeEvtNonSaisie = LstEvenement.FirstOrDefault(p => LstStatut.Contains(p.STATUS.Value));
                        if (_LeEvtNonSaisie == null)
                        {
                            foreach (CsCanalisation item in LstCanalisation)
                            {
                                LeEvenementSelect = ClasseMEthodeGenerique.DernierEvenement(LstEvenement, LaDemande.LaDemande.PRODUIT);
                                if (LeEvenementSelect != null )
                                {
                                       item.INDEXEVT = (LeEvenementSelect.INDEXEVT == null ? 0 : LeEvenementSelect.INDEXEVT);
                                       item.CONSO = (LeEvenementSelect.CONSO == null ? 0 : LeEvenementSelect.CONSO); ;
                                       this.Txt_IndexAnc.Text = string.IsNullOrEmpty(LeEvenementSelect.INDEXEVT.ToString()) ? string.Empty : LeEvenementSelect.INDEXEVT.ToString();
                                       item.INFOCOMPTEUR = LeEvenementSelect.COMPTEUR;
                                }
                                if (item.ETATCOMPT == SessionObject.Enumere.CompteurActifValeur) item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurActif;
                                else item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurInactifValeur;
                            }
                            AfficherInformation(LstCanalisation);
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_LeClientEvenementNonSaisie , MessageBoxControl.MessageBoxButtons.OkCancel   , MessageBoxControl.MessageBoxIcon.Question );
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                if (w.Result == MessageBoxResult.OK)
                                {
                                    IsSupprimerEvtNonSaisie = true;
                                    LstEvenement.Remove(_LeEvtNonSaisie);
                                    MaxNumEvt = LstEvenement.Max(t => t.NUMEVENEMENT);
                                    foreach (CsCanalisation item in LstCanalisation)
                                    {
                                        LeEvenementSelect = ClasseMEthodeGenerique.DernierEvenement(LstEvenement, LaDemande.LaDemande.PRODUIT);
                                        if (LeEvenementSelect != null)
                                        {
                                            item.INDEXEVT = (LeEvenementSelect.INDEXEVT == null ? 0 : LeEvenementSelect.INDEXEVT);
                                            item.CONSO = (LeEvenementSelect.CONSO == null ? 0 : LeEvenementSelect.CONSO); ;
                                            item.INFOCOMPTEUR = LeEvenementSelect.COMPTEUR;
                                            this.Txt_IndexAnc.Text = string.IsNullOrEmpty(LeEvenementSelect.INDEXEVT.ToString()) ? string.Empty : LeEvenementSelect.INDEXEVT.ToString();
                                        }
                                        if (item.ETATCOMPT == SessionObject.Enumere.CompteurActifValeur) item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurActif;
                                        else item.LIBELLEETATCOMPTEUR = SessionObject.Enumere.CompteurInactifValeur;
                                    }
                                    AfficherInformation(LstCanalisation);

                                }
                            };
                            w.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowWarning(ex.Message, Langue.Msg_LeClientEstEnFacturation);
                    }

                };
                service.RetourneEvenementDeLaCanalisationAsync(LstCanalisation);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Initctrl();
        }

        private void RetourneInfoAbon(int fk_idcentre,string centre, string client, string ordre, string produit)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneAbonCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    
                    AbonementRecherche = args.Result.FirstOrDefault(p => p.PRODUIT    == produit);
                    if (AbonementRecherche != null && !string.IsNullOrEmpty(AbonementRecherche.CENTRE ))
                    {
                        if (LaDemande.Abonne != null)
                        {
                            AbonementRecherche.NUMDEM = LaDemande.LaDemande.NUMDEM;
                            if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.Resiliation )
                                AbonementRecherche.DRES  = LaDemande.Abonne.DRES;
                            LaDemande.Abonne = AbonementRecherche;
                        }
                        //SetDemandeFromAbon(AbonementRecherche, ref LaDemande);
                        //if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.FactureManuelle ||
                        //    LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.Resiliation ||
                        //    LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.AvoirConsomation)
                        //{
                        //    this.Txt_PeriodeEnCour.IsReadOnly = false;
                        //    this.Txt_PeriodeEnCour.Background = new SolidColorBrush(Colors.Cyan);
                        //    this.Txt_PeriodeEnCour.Focus();
                        //}
                    }
                }
            };
            service.RetourneAbonAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();
        
        }
        private void RetourneInfoClient(int fk_idcentre,string centre, string client, string ordre)
        {
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneClientCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                if (args.Result != null)
                {
                    ClientRecherche  = args.Result;
                    LaDemande.LeClient = ClientRecherche;
                }
            };
            service.RetourneClientAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();

        }

        private void RafraichireLst()
        {
                //dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = LsDernierEvenement;
                //dataGrid1.SelectedItem = LsDernierEvenement[Index];
        }
        private void RemplireOngletEvenement(CsCanalisation _LeEvtSelect, CsEvenement _leDernierEvt)
        {
                //LaDemande.DRES = DateResil;
                //this.Txt_PeriodeEnCour.Text = (!string.IsNullOrEmpty(LaDemande.DRES)) ? LaDemande.DRES.Substring(3, 7) : string.Empty;
                //if (string.IsNullOrEmpty(this.Txt_PeriodeEnCour.Text))
                //this.Txt_PeriodeEnCour.Text = (!string.IsNullOrEmpty(LaDemande.PERIODE)) ? LaDemande.PERIODE : string.Empty;
                //this.Txt_point.Text = _LeEvtSelect.POINT.ToString();

                 if (!string.IsNullOrEmpty(_leDernierEvt.DERPERF))
                    this.Txt_periodeFactureF.Text =ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_leDernierEvt.DERPERF);

                 if (!string.IsNullOrEmpty(_leDernierEvt.DERPERFN ))
                     this.Txt_periodeFactureN.Text = ClasseMEthodeGenerique.FormatPeriodeMMAAAA(_leDernierEvt.DERPERFN);

                    this.Txt_IndexFacture.Text = string.IsNullOrEmpty(_leDernierEvt.INDEXEVT.ToString()) ? string.Empty : _leDernierEvt.INDEXEVT.ToString();
                    this.Txt_ConsoFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CONSO.ToString()) ? string.Empty : _leDernierEvt.CONSO.ToString();
                    this.Txt_CasFacture.Text = string.IsNullOrEmpty(_leDernierEvt.CAS) ? string.Empty : _leDernierEvt.CAS;

        }
        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LeCompteurSelect = new CsCanalisation();
            indexSelect = this.dataGrid1.SelectedIndex;

            if (this.dataGrid1.SelectedIndex >= 0)
            {
                LeCompteurSelect = (CsCanalisation)this.dataGrid1.SelectedItem;
                LeEvenementSelect = ClasseMEthodeGenerique.DernierEvenementFacture(LstEvenement, LaDemande.LaDemande .PRODUIT);
                if (LeEvenementSelect == null)
                    LeEvenementSelect = ClasseMEthodeGenerique.DernierEvenement(LstEvenement, LaDemande.LaDemande.PRODUIT);
                
                DateDernierEvt = LeEvenementSelect.DATEEVT;
                IndexInit = LeEvenementSelect.INDEXEVT;
                RemplireOngletEvenement(LeCompteurSelect, LeEvenementSelect);


            }
        }
        private void    CreeEvenement(CsEvenement _LeEvt,CsCanalisation _LeCompteur)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) &&
                    !string.IsNullOrEmpty(this.Txt_PeriodeEnCour .Text) &&
                    !string.IsNullOrEmpty(this.Txt_DateRelEncour.Text) &&
                    !string.IsNullOrEmpty(this.Txt_CasEnCour.Text))
                {
                    _LeEvt.NUMDEM = LaDemande.LaDemande.NUMDEM;
                    _LeEvt.INDEXEVT = string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) ? 0 : int.Parse(this.Txt_IndexSaisi.Text);
                    _LeEvt.CONSO = string.IsNullOrEmpty(this.Txt_ConsoEnCours.Text) ? 0 : int.Parse(this.Txt_ConsoEnCours.Text);
                    _LeEvt.DATEEVT = null;
                    _LeEvt.FACTURE = null;
                    _LeEvt.ENQUETE = string.Empty;
                    _LeEvt.FACPER = string.Empty;
                    _LeEvt.DERPERF  = string.Empty;
                    _LeEvt.DERPERFN = string.Empty;
                    _LeEvt.REGCONSO  = null;
                    _LeEvt.REGIMPUTE  = null;
                    _LeEvt.CONSOFAC = 0;
                    _LeEvt.MATRICULE = UserConnecte.matricule;
                    _LeEvt.COMPTEUR = _LeCompteur.NUMERO;
                    _LeEvt.TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR ;
                    _LeEvt.COEFLECT = _LeCompteur.COEFLECT;
                    _LeEvt.COEFCOMPTAGE = _LeCompteur.COEFCOMPTAGE;
                    _LeEvt.COEFLECT = _LeCompteur.COEFLECT;
                    _LeEvt.CAS = this.Txt_CasEnCour.Text;
                    _LeEvt.PERIODE = ClasseMEthodeGenerique.FormatPeriodeAAAAMM(this.Txt_PeriodeEnCour.Text);

                    _LeEvt.INDEXEVTPRECEDENT  = string.IsNullOrEmpty( this.Txt_IndexFacture .Text)? 0: int.Parse( this.Txt_IndexFacture .Text);
                    if (!string.IsNullOrEmpty(this.Txt_DateRelEncour.Text))
                        _LeEvt.DATEEVT = DateTime.Parse(this.Txt_DateRelEncour.Text);

                    if (LaDemande.LaDemande.TYPEDEMANDE  == SessionObject.Enumere.Resiliation)
                        _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeResiliation;

                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                        LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AvoirConsomation)
                        _LeEvt.CODEEVT = SessionObject.Enumere.EvenementCodeFactureIsole;

                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                        _LeEvt.LOTRI = LaDemande.LaDemande.CENTRE + SessionObject.Enumere.LotriTermination;
                    else
                        _LeEvt.LOTRI = LaDemande.LaDemande.CENTRE + SessionObject.Enumere.LotriManuel;

                    _LeEvt.STATUS = SessionObject.Enumere.EvenementReleve ;

                    _LeEvt.NUMEVENEMENT = MaxNumEvt + 1;
                    _LeEvt.USERCREATION = UserConnecte.matricule;
                    _LeEvt.USERMODIFICATION = UserConnecte.matricule; 
                    _LeEvt.DATECREATION = System.DateTime.Now.Date;
                    _LeEvt.DATEMODIFICATION = System.DateTime.Now.Date;

                    CsEvenement _LeEvenement = LstEvenementCree.FirstOrDefault(p => p.COMPTEUR == LeCompteurSelect.NUMERO);
                    if (_LeEvenement != null)
                        LstEvenementCree.Remove(_LeEvenement);
                    LstEvenementCree.Add(_LeEvt);
               }
                
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        
        }
        private void CreePagisol(CsPagisol _lePagisol)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) &&
                    //!string.IsNullOrEmpty(this.Txt_ConsoEnCours.Text) &&
                    !string.IsNullOrEmpty(this.Txt_DateRelEncour.Text) &&
                    !string.IsNullOrEmpty(this.Txt_CasEnCour.Text))
                {
                    _lePagisol.AIED = int.Parse(this.Txt_IndexFacture.Text );
                    _lePagisol.NIED = !string.IsNullOrEmpty(this.Txt_IndexSaisi.Text) ? int.Parse(this.Txt_IndexSaisi.Text): 0;
                    if (LaDemande.LeClient != null && !string.IsNullOrEmpty(LaDemande.LeClient.CATEGORIE))
                        _lePagisol.CATEGORIECLIENT = LaDemande.LeClient.CATEGORIE ;

                    _lePagisol.DDEB =  System.DateTime.Now;
                    _lePagisol.DFIN  =  System.DateTime.Now;
                    _lePagisol.CAS = this.Txt_CasEnCour.Text  ;
                    _lePagisol.CENTRE  = LaDemande.LaDemande.CENTRE ;
                    _lePagisol.CLIENT  = LaDemande.LaDemande.CLIENT ;
                    _lePagisol.POINT   = LeCompteurSelect.POINT ;
                    _lePagisol.PRODUIT = LaDemande.LaDemande.PRODUIT;
                    _lePagisol.TFAC = SessionObject.Enumere.FacturationEstimerAvecRegul;
                    if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                        _lePagisol.LOTRI = LaDemande.LaDemande.CENTRE + SessionObject.Enumere.LotriTermination;
                    else
                        _lePagisol.LOTRI = LaDemande.LaDemande.CENTRE + SessionObject.Enumere.LotriManuel;

                    _lePagisol.STATUT = SessionObject.Enumere.PagerieNonEnquetable;
                    if (LaDemande.Ag != null &&  !string.IsNullOrEmpty(LaDemande.Ag.ORDTOUR))
                        _lePagisol.ORDTOUR = LaDemande.Ag.ORDTOUR;
                    _lePagisol.PERFAC = "";
                    if (LaDemande.Abonne != null && !string.IsNullOrEmpty(LaDemande.Abonne.PERFAC ))
                        _lePagisol.FREQUENCE  = LaDemande.Abonne.PERFAC;
                    _lePagisol.QTEFAC =string.IsNullOrEmpty(this.Txt_ConsoEnCours.Text) ?0: int.Parse(this.Txt_ConsoEnCours.Text) ;
                    _lePagisol.STATUT = "";
                    _lePagisol.TOPEDIT = "";
                    if (LaDemande.Ag != null && !string.IsNullOrEmpty(LaDemande.Ag.TOURNEE ))
                        _lePagisol.TOURNEE = LaDemande.Ag.TOURNEE;
                    _lePagisol.USERCREATION = UserConnecte.matricule;
                    _lePagisol.USERMODIFICATION = UserConnecte.matricule;
                    _lePagisol.DATECREATION = System.DateTime.Now.Date;
                    _lePagisol.DATEMODIFICATION  = System.DateTime.Now.Date;
                    CsPagisol _LePag= LstPagisolCree.FirstOrDefault(p => p.CENTRE  == LeEvenementSelect .CENTRE &&
                                                                              p.CLIENT  == LeEvenementSelect.CLIENT &&
                                                                              p.POINT   == LeEvenementSelect.POINT  );
                    if (_LePag != null)
                        LstPagisolCree.Remove(_LePag);
                    LstPagisolCree.Add(_lePagisol);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void Txt_ConsoEnCours_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.Txt_ConsoEnCours.Text))
                //LaDemande.Evenement_CONSO  = int.Parse(this.Txt_ConsoEnCours.Text);
        }
        private void EnregisterCanalisation(CsDemande _LaDemande)
        {
            if (_LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.Resiliation)
                {
                      if (this.rdb_RetraitCompteur.IsChecked == true)
                          foreach (CsCanalisation item in _LaDemande.LstCanalistion)
                          {
                              item.ETATCOMPT = SessionObject.Enumere.CompteurInactifValeur;
                              item.NUMDEM = LaDemande.LaDemande.NUMDEM;
                          }
                }
        }
        private void EnregisterEvenement(CsDemande _LaDemande)
        {
            if (LstEvenementCree != null && LstEvenementCree.Count != 0)
                _LaDemande.LstEvenement = LstEvenementCree;
           
        }
        private void EnregisterPagisol(CsDemande _LaDemande)
        {
            if (LstPagisolCree != null && LstPagisolCree.Count != 0)
                _LaDemande.LstPagisol  = LstPagisolCree;
        }
        private  void EnregisterLclient(CsDemande _Lademande)
       { 

           LeEvenementRemboursementAvance = new List<CsLclient >();
           CsLclient _LaFacture = new CsLclient();
           _LaFacture.CENTRE   = LaDemande.LaDemande.CENTRE;
           _LaFacture.CLIENT  = LaDemande.LaDemande.CLIENT;
           _LaFacture.ORDRE  = LaDemande.LaDemande.ORDRE;
           _LaFacture.MATRICULE  = UserConnecte.matricule;
           _LaFacture.DC = SessionObject.Enumere.Debit;

           _LaFacture.REFEM = System.DateTime.Now.Date.ToShortDateString().Substring(6, 4) + System.DateTime.Now.Date.ToShortDateString().Substring(3, 2);
           _LaFacture.DENR = System.DateTime.Now.Date;
           _LaFacture.EXIGIBILITE = System.DateTime.Now.Date;
           _LaFacture.TOP1  = SessionObject.Enumere.TopGuichet;

           _LaFacture.COPER = SessionObject.Enumere.CoperRCD;
           _LaFacture.NATURE = "00";
           _LaFacture.MONTANT = LaDemande.Abonne.AVANCE;

           _LaFacture.USERCREATION = UserConnecte.matricule;
           _LaFacture.USERMODIFICATION = UserConnecte.matricule;
           _LaFacture.DATECREATION =  System.DateTime.Now;
           _LaFacture.DATEMODIFICATION =  System.DateTime.Now;

            if(_LaFacture.MONTANT != null && _LaFacture.MONTANT !=0)
           LeEvenementRemboursementAvance.Add(_LaFacture);
       }
        public void ValiderDemande(CsDemande _LaDemande)
        {
            try
            {
                EnregisterCanalisation(_LaDemande);
                EnregisterEvenement(_LaDemande);
                if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                    EnregisterLclient(LaDemande);
                if (_LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                    _LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                    EnregisterPagisol(_LaDemande);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void EnregistrerInfo(CsDemande _LaDemande)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                {
                    BackToZero(int.Parse(this.Txt_IndexSaisi.Text), int.Parse(this.Txt_IndexAnc.Text));

                    if (IsSupprimerEvtNonSaisie && _LeEvtNonSaisie != null)
                    {
                        AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service1.SupprimeEvtNonFactureCompleted += (ss, argss) =>
                        {
                            if (argss != null && argss.Cancelled)
                                return;
                        };
                        service1.SupprimeEvtNonFactureAsync(_LeEvtNonSaisie.FK_IDCENTRE ,_LeEvtNonSaisie.CENTRE, _LeEvtNonSaisie.CLIENT, _LeEvtNonSaisie.ORDRE, _LeEvtNonSaisie.PRODUIT, _LeEvtNonSaisie.POINT);
                        service1.CloseAsync();

                    }
                    foreach (CsEvenement item in LstEvenementCree)
                        IsSaisieValider(item, LsDesCas); 
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
 
        private void Txt_IndexSaisi_TextChanged(object sender, TextChangedEventArgs e)
       {
           try
           {
               if (!string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
               {
                   Txt_IndexNouv.Text = this.Txt_IndexSaisi.Text;
                   int? conso = int.Parse(this.Txt_IndexSaisi.Text) - IndexInit;
                   LeCompteurSelect.INDEXEVT = int.Parse(this.Txt_IndexSaisi.Text);
                   //if (conso > 0)
                   //{
                       LeCompteurSelect.CONSO = conso;
                       this.Txt_ConsoEnCours.Text = LeCompteurSelect.CONSO.ToString();
                   //}
                   CreeEvenement(LeEvenementSelect, LeCompteurSelect);
                   if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                       CreePagisol(LePagisolCree);

               }
               else
               {
                   Txt_IndexNouv.Text = this.Txt_IndexSaisi.Text;
                   LeCompteurSelect.CONSO = int.Parse(this.Txt_ConsoFacture.Text);
                   this.Txt_ConsoEnCours.Text = LeCompteurSelect.CONSO.ToString();
                   LeCompteurSelect.INDEXEVT = int.Parse(this.Txt_ConsoFacture.Text);
                   CreeEvenement(LeEvenementSelect, LeCompteurSelect);
                   if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                       LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                       CreePagisol(LePagisolCree);

               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        CsEvenement LeEvtSelect = new CsEvenement();
        void IsSaisieValider(CsEvenement LaSaisie, List<CsCasind> LstCas)
        {
            LeEvtSelect = new CsEvenement();
            LeEvtSelect = LaSaisie;
            CsCasind LeCasRecherche = LstCas.FirstOrDefault(p => p.CODE == LaSaisie.CAS);
            if (LeCasRecherche == null)
            {
                Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_CasInexistant, "Erreur");
                dataGrid1.IsEnabled = false;
                return;
            }
            else
            {
                // Saisie d'index
                if (LeCasRecherche.SAISIEINDEX  == SessionObject.Enumere.CodeObligatoire)
                {
                    if (string.IsNullOrEmpty(this.Txt_IndexSaisi.Text))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiConsoObligatoire, "Alert");
                        dataGrid1.IsEnabled = false;
                        return;

                    }
                }
                if (LeCasRecherche.SAISIEINDEX == SessionObject.Enumere.CodeInterdit)
                {
                    if (!string.IsNullOrEmpty(this.Txt_IndexSaisi .Text))
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Index .Langue .msg_SaisiConsoInterdite, "Alert");
                        dataGrid1.IsEnabled = false;
                        return;

                    }
                }
                //
                // Saisie de la consomation
                if (LeCasRecherche.SAISIECONSO  == SessionObject.Enumere.CodeObligatoire)
                {
                    //if (LaSaisie.CONSO != null && string.IsNullOrEmpty(this.Txt_Consomation.Text))
                    //{
                    //    Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiConsoObligatoire, "Alert");
                    //    dataGrid1.IsEnabled = false;
                    //    return;
                    //}
                }
                else if (LeCasRecherche.SAISIECONSO == SessionObject.Enumere.CodeInterdit)
                {
                    ////if (!string.IsNullOrEmpty(LaSaisie.CONSO.ToString()))
                    //if (!string.IsNullOrEmpty(this.Txt_Consomation.Text.ToString()))
                    //{
                    //    Message.ShowInformation(Galatee.Silverlight.Resources.Index.Langue.msg_SaisiConsoInterdite, "Alert");
                    //    dataGrid1.IsEnabled = false;
                    //    return;
                    //}
                }


            }
            IsCasValider(LaSaisie);
        }
        void BackToZero(int ? indexSaisi,int ? indexPrecedent )
        {
            try
            {
                if (indexSaisi < indexPrecedent)
                {
                    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_RetourAZero, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    ws.OnMessageBoxClosed += (l, results) =>
                    {
                        if (ws.Result == MessageBoxResult.No)
                        {
                            this.Txt_CasEnCour.Text = "82";
                            this.Txt_CasEnCour.IsReadOnly = true;
                            this.Txt_IndexSaisi.Text = this.Txt_IndexSaisi.Text;
                            this.Txt_ConsoEnCours.Text = IndexInit.ToString();
                        }
                        else if (ws.Result == MessageBoxResult.OK)
                        {
                            this.Txt_CasEnCour.Text = "04";
                            this.Txt_CasEnCour.IsReadOnly = true;
                            if (CanalisationClientRecherche[0].CADRAN == 0)
                            {
                                Message.ShowWarning(Langue.msgCadranIndeterminer, Langue.lbl_Menu);
                                this.Txt_IndexSaisi.Text = string.Empty;
                                this.Txt_ConsoEnCours.Text = string.Empty;
                                return;
                            }
                            else
                            {
                                int Roue = int.Parse( CanalisationClientRecherche[0].CADRAN.Value.ToString());
                                int initval = 9;
                                int? Indexmax = int.Parse(initval.ToString().PadLeft(Roue,'9'));
                                this.Txt_ConsoEnCours.Text = ((Indexmax - indexPrecedent) + (int.Parse(this.Txt_IndexSaisi.Text) + 1)).ToString();
                                this.Txt_IndexSaisi.Text = this.Txt_IndexSaisi.Text;
                            }
                        }
                    };
                    ws.Show();
                    return;
                }
                else
                {
                    this.Txt_CasEnCour.IsReadOnly = false;
                    this.Txt_CasEnCour.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        
        }
        void IsCasValider(CsEvenement LaSaise)
        {

            if (LaSaise.CAS == "00" && LaSaise.CONSOMOYENNE > LaSaise.CONSO)
            {
                var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_ConsoFaible, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                ws.OnMessageBoxClosed += (l, results) =>
                {
                    if (ws.Result == MessageBoxResult.No)
                    {
                        LeEvtSelect.CAS = string.Empty;
                        LeEvtSelect.INDEXEVT = null;
                        LeEvtSelect.CONSO = null;
                        LeEvtSelect.IsSaisi = false;
                        dataGrid1.IsEnabled = false;
                    }
                    else if (ws.Result == MessageBoxResult.OK)
                    {
                        LaSaise.CAS = "84";
                        ValiderDemande(LaDemande);
                    }
                };
                ws.Show();
                return;
            }
            //if (LaSaise.CAS != "00")
            //{
            //    var ws = new MessageBoxControl.MessageBoxChildWindow("Information", Galatee.Silverlight.Resources.Index.Langue.msg_Confirmation, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
            //    ws.OnMessageBoxClosed += (l, results) =>
            //    {
            //        if (ws.Result == MessageBoxResult.No)
            //        {
            //            LeEvtSelect.CAS = string.Empty;
            //            LeEvtSelect.INDEXEVT = null;
            //            LeEvtSelect.CONSO = null;
            //            LeEvtSelect.IsSaisi = false;
            //            dataGrid1.IsEnabled = false;
            //        }
            //        else if (ws.Result == MessageBoxResult.OK)
            //        {
            //            ValiderDemande(LaDemande);
            //        }
            //    };
            //    ws.Show();
            //    return;
            //}
            ValiderDemande(LaDemande);
        }
       private void btn_cas_Click(object sender, RoutedEventArgs e)
       {

           btn_cas.IsEnabled = false;
           List<object> _LstCas = ClasseMEthodeGenerique.RetourneListeObjet(LsDesCas);
           UcListeGenerique ctr = new UcListeGenerique(_LstCas, "NUMCAS", "LIBELLE", Langue.lbl_ListeCommune);
           ctr.Closed += new EventHandler(galatee_OkClicked);
           ctr.Show();
       }
       void galatee_OkClicked(object sender, EventArgs e)
       {
           try
           {
               UcListeGenerique ctrs = sender as UcListeGenerique;
               if (ctrs.GetisOkClick)
               {
                   CsCasind _lstCas = (CsCasind)ctrs.MyObject;
                   if (_lstCas != null)
                       this.Txt_CasEnCour.Text = _lstCas.CODE ;
               }
               this.btn_cas.IsEnabled = true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        public void AfficherInformation(List<CsCanalisation> _LstCanalisation)
        {
            try
            {
                dataGrid1.ItemsSource = _LstCanalisation;
                dataGrid1.SelectedItem = _LstCanalisation[0];
            } 
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void Txt_PeriodeEnCour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.FactureManuelle ||
                LaDemande.LeTypeDemande.CODE == SessionObject.Enumere.AvoirConsomation)
            {
                if (this.Txt_PeriodeEnCour.Text.Length == 7)
                    if (ClasseMEthodeGenerique.IsFormatPeriodeValide(Txt_PeriodeEnCour.Text))
                    {
                        this.Txt_FinPeriode.Text = ClasseMEthodeGenerique.DernierJourDuMois(int.Parse(Txt_PeriodeEnCour.Text.Substring(0, 2)), int.Parse(Txt_PeriodeEnCour.Text.Substring(3, 4)));
                        this.Txt_DebutPeriode.Text = "01" + "/" + Txt_PeriodeEnCour.Text.Substring(0, 2).PadLeft(2, '0') + "/" + Txt_PeriodeEnCour.Text.Substring(3, 4);
                    }
            }
        }

        private void Txt_DateRelEncour_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_DateRelEncour.Text.Length == SessionObject.Enumere.TailleDate)
                {
                    DateTime? pDateFin =ClasseMEthodeGenerique.IsDateValider(this.Txt_DateRelEncour.Text);
                    if (ClasseMEthodeGenerique.IsDateSaisieValide(pDateFin, DateDernierEvt))
                    {
                        CreeEvenement(LeEvenementSelect, LeCompteurSelect);
                        if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                             LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                            CreePagisol(LePagisolCree);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_DateFinInferieurDateDebut, MessageBoxControl.MessageBoxButtons.YesNo  , MessageBoxControl.MessageBoxIcon.Question );
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            if (w.Result == MessageBoxResult.OK)
                            {
                                CreeEvenement(LeEvenementSelect, LeCompteurSelect);
                                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                                     LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                                    CreePagisol(LePagisolCree);
                            }
                            else
                            {
                                this.Txt_DateRelEncour.Text = string.Empty;
                                this.Txt_DateRelEncour.Focus();
                            }
                        };
                        w.Show();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Txt_CasEnCour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CasEnCour.Text.Length == SessionObject.Enumere.TailleCas)
            {
                CreeEvenement(LeEvenementSelect, LeCompteurSelect);
                if (LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.FactureManuelle ||
                    LaDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation)
                    CreePagisol(LePagisolCree);
            }
        }

        private void Txt_IndexSaisi_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.Msg_ModificationFacture, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            w.OnMessageBoxClosed += (_, result) =>
            {
                if (w.Result == MessageBoxResult.No)
                    this.checkBox1.IsChecked = true;
            };
            w.Show(); 
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {

        }
   
    }
}
