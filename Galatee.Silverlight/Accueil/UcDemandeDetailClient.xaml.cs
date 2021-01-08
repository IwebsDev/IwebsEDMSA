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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil ;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcDemandeDetailClient : UserControl
    {

        List<CsDenomination > LstCivilite = new List<CsDenomination>() ;
        List<CsModepaiement> LstModePaiement = new List<CsModepaiement>();

        
        List<CsCodeConsomateur> LstCodeConsomateur = new List<CsCodeConsomateur>() ;
        List<CsCategorieClient> LstCategorie = new List<CsCategorieClient>() ;
        List<CsFermable> LstFermable = new List<CsFermable>();
        List<CsNatureClient > LstNatureClient = new List<CsNatureClient>();
        List<CsNationalite> LstDesNationalites = new List<CsNationalite>();
        List<CsRegCli> LstCodeRegroupement = new List<CsRegCli>();

        List<CParametre> LstDesModePaiement = new List<CParametre>();
        CsClient  LeClientRecherche = new CsClient();
         public CsDemande LaDemande = new CsDemande();
  
        bool IsUpate = false;
        void Translate()
        {

            // Gestion de la langue
            this.lbl_adresse.Content = Langue.lbl_adresse;
            this.lbl_categoie.Content = Langue.lbl_categoie;
            this.lbl_client.Content = Langue.lbl_client;
            this.lbl_CodeConsomateur.Content = Langue.lbl_consommation ;
            this.lbl_CodeRegroupement.Content = Langue.lbl_CodeRegroupement;
            this.lbl_CodeRelance.Content = Langue.lbl_CodeRelance;
            //this.lbl_denom.Content = Langue.lbl_denom;
            this.lbl_ModePayement.Content = Langue.lbl_ModePayement;
            this.lbl_Nationnalite.Content = Langue.lbl_Nationnalite;
            this.lbl_NomAgent.Content = Langue.lbl_name;
            this.btn_autresInfo.Content = Langue.lbl_autresInfo;
            //this.label4.Content = Langue.lbl_denom;
            this.label5.Content = Langue.lbl_name;
            this.rdb_Owner.Content = Langue.rdb_landlord ;
            this.rdb_Tenant.Content = Langue.rdb_tenant ;
            //
        }
        public UcDemandeDetailClient()
        {
            InitializeComponent();
            Translate();
        }
        public UcDemandeDetailClient(CsDemande _LaDemande,bool _IsUpdate )
        {
            InitializeComponent();
            Translate();
            LaDemande = _LaDemande;
            TypeDemande = _LaDemande.LaDemande.TYPEDEMANDE;
            IsUpate = _IsUpdate;
            if (LaDemande.LeClient == null)
            {
                LaDemande.LeClient = new CsClient();
                LaDemande.LeClient.CENTRE = LaDemande.LaDemande.CENTRE;
                LaDemande.LeClient.REFCLIENT = LaDemande.LaDemande.CLIENT;
                LaDemande.LeClient.ORDRE = LaDemande.LaDemande.ORDRE;
            }
            ChargerCategorie();
            ChargerCodeConsomateur();
            ChargerNationnalite();
            ChargerNatureClient();
            ChargerFermable();
            ChargerCivilite();
            ChargerModePaiement();

          
        }
        string TypeDemande = string.Empty;
        public UcDemandeDetailClient(CsClient  _leClient, string _typedemande)
        {
            InitializeComponent();
            Translate();    
            ChargerCategorie();
            ChargerCodeConsomateur();
            ChargerNationnalite();
            ChargerNatureClient();
            ChargerFermable();
            ChargerCivilite();
            TypeDemande = _typedemande;
            AfficherInformationClient(_leClient);
            RemplirLibelle();

        }
        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie.Count != 0)
                {
                    LstCategorie = SessionObject.LstCategorie;
                    if (LstCategorie != null && LstCategorie.Count != 0)
                    {
                        if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                            TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                        {
                            this.Txt_CodeCategorie.Text = LstCategorie[0].CODE;
                            this.Txt_LibelleCategorie .Text = LstCategorie[0].LIBELLE ;
                            this.Txt_CodeCategorie.Tag = LstCategorie[0].PK_ID;
                            EnregistrerDemande(LaDemande);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeCategorie.Text) && LstCategorie!= null )
                            {
                                CsCategorieClient _LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(LstCategorie, this.Txt_CodeCategorie.Text, "CODE");
                                if (_LaCategorie != null && !string.IsNullOrEmpty(_LaCategorie.LIBELLE))
                                    this.Txt_LibelleCategorie.Text = _LaCategorie.LIBELLE;
                            }
                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCategorieCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCategorie = args.Result;
                        LstCategorie = SessionObject.LstCategorie;
                        if (LstCategorie != null && LstCategorie.Count != 0)
                        {
                            if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                                TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                            {
                                this.Txt_CodeCategorie.Text = LstCategorie[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(LaDemande.LeClient.CATEGORIE))
                                {
                                    CsCategorieClient _LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(LstCategorie, this.Txt_CodeCategorie.Text, "CODE");
                                    if (_LaCategorie != null && !string.IsNullOrEmpty(_LaCategorie.LIBELLE))
                                        this.Txt_LibelleCategorie.Text = _LaCategorie.LIBELLE;
                                }

                            }
                        }
                    };
                    service.RetourneCategorieAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerFermable()
        {
            try
            {
                if (SessionObject.LstFermable.Count != 0)
                {
                    LstFermable = SessionObject.LstFermable;
                    if (LstFermable != null && LstFermable.Count != 0)
                    {
                        if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                             TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                        {
                            this.Txt_CodeFermableClient.Text = LstFermable[0].CODE;
                            this.Txt_LibelleFermable  .Text = LstFermable[0].LIBELLE ;
                            this.Txt_CodeFermableClient.Tag = LstFermable[0].PK_ID;
                            EnregistrerDemande(LaDemande);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text) && LstFermable!= null )
                            {
                                CsFermable _LeFermable = ClasseMEthodeGenerique.RetourneObjectFromList(LstFermable, this.Txt_CodeFermableClient.Text, "CODE");
                                if (_LeFermable != null && !string.IsNullOrEmpty(_LeFermable.LIBELLE))
                                    this.Txt_LibelleFermable.Text = _LeFermable.LIBELLE;
                            }

                        }
                    }
                
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneFermableCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstFermable = args.Result;
                        LstFermable = SessionObject.LstFermable;
                        if (LstFermable != null && LstFermable.Count != 0)
                        {
                            if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                                 TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                            {
                                this.Txt_CodeFermableClient.Text = LstFermable[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text) && LstFermable!= null )
                                {
                                    CsFermable _LeFermable = ClasseMEthodeGenerique.RetourneObjectFromList(LstFermable, this.Txt_CodeFermableClient.Text, "CODE");
                                    if (_LeFermable != null && !string.IsNullOrEmpty(_LeFermable.LIBELLE))
                                        this.Txt_LibelleFermable.Text = _LeFermable.LIBELLE;
                                }

                            }
                        }
                    };
                    service.RetourneFermableAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNatureClient()
        {
            try
            {
                if (SessionObject.LstNatureClient.Count != 0)
                {
                    LstNatureClient = SessionObject.LstNatureClient;
                    if (LstNatureClient != null && LstNatureClient.Count != 0)
                    {
                        if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                             TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                        {
                            this.Txt_CodeNatureClient.Text = LstNatureClient[0].CODE;
                            this.Txt_CodeNatureClient.Tag  = LstNatureClient[0].PK_ID ;
                            this.Txt_LibelleNatureClient .Text = LstNatureClient[0].LIBELLE ;
                            EnregistrerDemande(LaDemande);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text) && LstNatureClient != null )
                            {
                                CsNatureClient _LaNatureClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstNatureClient, this.Txt_CodeNatureClient.Text, "CODE");
                                if (!string.IsNullOrEmpty(_LaNatureClient.LIBELLE))
                                    this.Txt_LibelleNatureClient.Text = _LaNatureClient.LIBELLE;
                            }

                        }
                    }
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneNatureCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstNatureClient = args.Result;

                        LstNatureClient = SessionObject.LstNatureClient;
                        if (LstNatureClient != null && LstNatureClient.Count != 0)
                        {
                            if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                                 TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                            {
                                this.Txt_CodeNatureClient.Text = LstNatureClient[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text) )
                                {
                                    CsNatureClient _LaNatureClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstNatureClient, this.Txt_CodeNatureClient.Text, "CODE");
                                    if (!string.IsNullOrEmpty(_LaNatureClient.LIBELLE))
                                        this.Txt_LibelleNatureClient.Text = _LaNatureClient.LIBELLE;
                                }

                            }
                        }
                    };
                    service.RetourneNatureAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        private void ChargerModePaiement()
        {
            try
            {
                if (SessionObject.LstModePaiement != null && SessionObject.LstModePaiement.Count != 0)
                {
                    LstModePaiement = SessionObject.LstModePaiement;

                    if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                        TypeDemande == SessionObject.Enumere.AbonnementSeul ||
                           TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                    {
                        this.Txt_CodePaiement.Text = LstModePaiement[0].CODE;
                        this.Txt_LibelleModePaiement.Text = LstModePaiement[0].LIBELLE;
                        this.Txt_CodePaiement.Tag = LstModePaiement[0].PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodePaiement.Text))
                        {
                            CsModepaiement _LeModePaiement = ClasseMEthodeGenerique.RetourneObjectFromList(LstModePaiement, this.Txt_CodePaiement.Text, "CODE");
                            if (!string.IsNullOrEmpty(_LeModePaiement.LIBELLE))
                            {
                                this.Txt_CodePaiement.Text = _LeModePaiement.CODE;
                                this.Txt_LibelleModePaiement.Text = _LeModePaiement.LIBELLE;
                            }
                        }
                    }
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCodeModePayementCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstModePaiement = args.Result;
                    LstModePaiement = SessionObject.LstModePaiement;
                    if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                         TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                    {
                        this.Txt_CodePaiement.Text = LstModePaiement[0].CODE;
                        this.Txt_LibelleModePaiement.Text = LstModePaiement[0].LIBELLE;
                        this.Txt_CodePaiement.Tag = LstModePaiement[0].PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this.Txt_CodePaiement.Text))
                        {
                            CsModepaiement _LeModePaiement = ClasseMEthodeGenerique.RetourneObjectFromList(LstModePaiement, this.Txt_CodePaiement.Text, "CODE");
                            if (!string.IsNullOrEmpty(_LeModePaiement.LIBELLE))
                                this.Txt_CodePaiement.Text = _LeModePaiement.CODE ;
                        }
                    }
                };
                service.RetourneCodeModePayementAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerCivilite()
        {
            try
            {
                if (SessionObject.LstCivilite != null)
                {
                    LstCivilite = SessionObject.LstCivilite;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneListeDenominationAllCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCivilite = args.Result;
                    LstCivilite = SessionObject.LstCivilite;
                };
                service.RetourneListeDenominationAllAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerNationnalite()
        {
            try
            {
                if (SessionObject.LstDesNationalites.Count != 0)
                {
                    LstDesNationalites = SessionObject.LstDesNationalites;
                    if (LstDesNationalites != null && LstDesNationalites.Count != 0)
                    {
                        if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                             TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                        {
                            this.Txt_CodeNationalite.Text = LstDesNationalites[0].CODE;
                            this.Txt_Nationnalite.Text = LstDesNationalites[0].LIBELLE  ;
                            this.Txt_CodeNationalite.Tag = LstDesNationalites[0].PK_ID;

                            EnregistrerDemande(LaDemande);

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeNationalite.Text))
                            {
                                CsNationalite _LaNationalite = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                                if (!string.IsNullOrEmpty(_LaNationalite.LIBELLE))
                                    this.Txt_Nationnalite.Text = _LaNationalite.LIBELLE;
                            }

                        }
                    }
                
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneNationnaliteCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstDesNationalites = args.Result;

                        LstDesNationalites = SessionObject.LstDesNationalites;
                        if (LstDesNationalites != null && LstDesNationalites.Count != 0)
                        {
                            if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                                 TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                            {
                                this.Txt_CodeNationalite.Text = LstDesNationalites[0].CODE;
                                this.Txt_Nationnalite .Text = LstDesNationalites[0].LIBELLE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeNationalite.Text))
                                {
                                    CsNationalite _LaNationalite = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                                    if (!string.IsNullOrEmpty(_LaNationalite.LIBELLE))
                                        this.Txt_Nationnalite.Text = _LaNationalite.LIBELLE;
                                }

                            }
                        }
                    };
                    service.RetourneNationnaliteAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    LstCodeRegroupement = SessionObject.LstCodeRegroupement;
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeRegroupementCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeRegroupement = args.Result;
                    };
                    service.RetourneCodeRegroupementAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void ChargerCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                {
                    LstCodeConsomateur = SessionObject.LstCodeConsomateur;
                    if (LstCodeConsomateur != null && LstCodeConsomateur.Count != 0)
                    {
                        if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                             TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                        {
                            this.Txt_CodeConsomateur.Text = LstCodeConsomateur[0].CODE;
                            this.Txt_LibelleCodeConso.Text = LstCodeConsomateur[0].LIBELLE;
                            this.Txt_CodeConsomateur.Tag = LstCodeConsomateur[0].PK_ID;

                            EnregistrerDemande(LaDemande);

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodeConsomateur.Text))
                            {
                                CsCodeConsomateur _LeCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                                if (!string.IsNullOrEmpty(_LeCodeConso.LIBELLE))
                                    this.Txt_LibelleCodeConso.Text = _LeCodeConso.LIBELLE;
                            }
                        }
                    }
                
                }
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneCodeConsomateurCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeConsomateur = args.Result;
                        LstCodeConsomateur = SessionObject.LstCodeConsomateur;
                        if (LstCodeConsomateur != null && LstCodeConsomateur.Count != 0)
                        {
                            if ((TypeDemande == SessionObject.Enumere.BranchementAbonement ||
                                 TypeDemande == SessionObject.Enumere.BranchementSimple) && !IsUpate)
                            {
                                this.Txt_CodeConsomateur.Text = LstCodeConsomateur[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeConsomateur.Text))
                                {
                                    CsCodeConsomateur _LeCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                                    if (!string.IsNullOrEmpty(_LeCodeConso.LIBELLE))
                                        this.Txt_LibelleCodeConso.Text = _LeCodeConso.LIBELLE;
                                }
                            }
                        }
                    };
                    service.RetourneCodeConsomateurAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void InitialiseCtrl()
        {

            try
            {

                if (!IsUpate)
                {
                  
                    if (TypeDemande != SessionObject.Enumere.BranchementSimple
                        && TypeDemande != SessionObject.Enumere.BranchementAbonement
                        && TypeDemande != SessionObject.Enumere.AbonnementSeul )
                        RetourneInfoClient(LaDemande.LaDemande.FK_IDCENTRE , LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
                    if (TypeDemande == SessionObject.Enumere.AbonnementSeul)
                    {
                        if (TypeDemande == SessionObject.Enumere.AbonnementSeul && LaDemande.LaDemande.ORDRE != "01")
                            RetourneInfoClient(LaDemande.LaDemande.FK_IDCENTRE , LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, (int.Parse(LaDemande.LaDemande.ORDRE) - 1).ToString("00"));
                        else
                            RetourneInfoClient(LaDemande.LaDemande.FK_IDCENTRE , LaDemande.LaDemande.CENTRE, LaDemande.LaDemande.CLIENT, LaDemande.LaDemande.ORDRE);
                    }
                }
                AfficherInformationClient(LaDemande.LeClient);
            }
            catch (Exception EX)
            {
                Message.ShowError(EX.Message, Langue.lbl_Menu);
            }

        }
        private void RetourneInfoClient(int fk_idcentre,string centre, string client, string ordre)
        {
            LeClientRecherche = new CsClient();
            AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service.RetourneClientCompleted += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;
                LeClientRecherche = args.Result;
                if (LeClientRecherche != null )
                AfficherInformationClient(LeClientRecherche);
            };
            service.RetourneClientAsync(fk_idcentre,centre, client, ordre);
            service.CloseAsync();

        }
        private void AfficherInformationClient(CsClient _LeClient)
        {
            this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Addresse.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
            this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;
         
            if (_LeClient.PROPRIO != null && string.IsNullOrEmpty(_LeClient.PROPRIO))
                this.rdb_Tenant.IsChecked = true;
            else
                this.rdb_Owner.IsChecked = true;
            if (TypeDemande != SessionObject.Enumere.ModificationClient )
               this.Txt_NomClientAbon.Text = string.IsNullOrEmpty(LaDemande.Ag.NOMP) ? string.Empty : LaDemande.Ag.NOMP;
            if (string.IsNullOrEmpty(this.Txt_NomClientAbon.Text))
                this.Txt_NomClientAbon.Text =  (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);

            //this.Txt_CodeCivilite.Text = (string.IsNullOrEmpty(_LeClient.DENABON) ? string.Empty : _LeClient.DENABON);
            this.TxtSMSSend.Text = string.IsNullOrEmpty(LaDemande.Ag.TELEPHONE) ? string.Empty : LaDemande.Ag.TELEPHONE;
            this.Txt_telephone.Text = string.IsNullOrEmpty(LaDemande.Ag.TELEPHONE) ? string.Empty : LaDemande.Ag.TELEPHONE;
            if (string.IsNullOrEmpty(this.Txt_telephone.Text))
                this.Txt_telephone.Text = string.IsNullOrEmpty(LaDemande.LeClient.TELEPHONE) ? string.Empty : LaDemande.LeClient.TELEPHONE;
            this.Txt_Email.Text = string.IsNullOrEmpty(LaDemande.Ag.EMAIL) ? string.Empty : LaDemande.Ag.EMAIL;
            if (string.IsNullOrEmpty(this.Txt_Email.Text))
                this.Txt_Email.Text = string.IsNullOrEmpty(LaDemande.LeClient.EMAIL) ? string.Empty : LaDemande.LeClient.EMAIL;
            this.Txt_Addresse1.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
            this.Txt_adresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;

            this.Txt_CodeConsomateur.Text = string.IsNullOrEmpty(_LeClient.CODECONSO) ? string.Empty : _LeClient.CODECONSO;
            this.Txt_CodeCategorie.Text = string.IsNullOrEmpty(_LeClient.CATEGORIE) ? string.Empty : _LeClient.CATEGORIE;
            this.Txt_CodeFermableClient.Text = string.IsNullOrEmpty(_LeClient.CODERELANCE) ? string.Empty : _LeClient.CODERELANCE;
            this.Txt_CodeNatureClient.Text = string.IsNullOrEmpty(_LeClient.NATURE) ? string.Empty : _LeClient.NATURE;
            this.Txt_CodeNationalite.Text = string.IsNullOrEmpty(_LeClient.NATIONNALITE) ? string.Empty : _LeClient.NATIONNALITE;
            this.Txt_Numeronina.Text = string.IsNullOrEmpty(_LeClient.NUMEROIDCLIENT) ? string.Empty : _LeClient.NUMEROIDCLIENT;
            this.Txt_CodePaiement.Text = string.IsNullOrEmpty(_LeClient.MODEPAIEMENT) ? string.Empty : _LeClient.MODEPAIEMENT;
            

            if (TypeDemande == SessionObject.Enumere.Resiliation ||
                //TypeDemande == SessionObject.Enumere.Reabonnement ||
                TypeDemande == SessionObject.Enumere.ChangementCompteur  ||
                TypeDemande == SessionObject.Enumere.ModificationClient)
                IsControleInaActif(true);


        }
        private void IsControleInaActif(bool Etat)
        {
            this.btn_autresInfo.IsEnabled = !Etat;
            this.btn_Categorie.IsEnabled = !Etat;
            //this.btn_Civilite.IsEnabled = !Etat;
            //this.btn_CiviliteAgent.IsEnabled = !Etat;
            this.btn_CodeConsomateur.IsEnabled = !Etat;
            this.btn_CodeRegroupement.IsEnabled = !Etat;
            this.btn_Nationalite.IsEnabled = !Etat;
            this.btn_FermableClient.IsEnabled = !Etat;
            this.btn_NatureClient.IsEnabled = !Etat;
            this.btn_Categorie.IsEnabled = !Etat;

            this.Txt_Addresse.IsReadOnly = Etat;
            this.Txt_Client.IsReadOnly = Etat;
            this.Txt_Ordre.IsReadOnly = Etat;
            //this.Txt_CodeCivilite.IsReadOnly = Etat;
            //this.Txt_LibelleCivilite.IsReadOnly = Etat;
            this.Txt_NomClientAbon.IsReadOnly = Etat;
            this.Txt_Addresse1.IsReadOnly = Etat;
            this.Txt_adresse2.IsReadOnly = Etat;
            this.Txt_CodeConsomateur.IsReadOnly = Etat;
            this.Txt_LibelleCodeConso.IsReadOnly = Etat;
            this.Txt_CodeFermableClient.IsReadOnly = Etat;
            this.Txt_LibelleFermable.IsReadOnly = Etat;
            this.Txt_CodeCategorie.IsReadOnly = Etat;
            this.Txt_LibelleCategorie.IsReadOnly = Etat;
            this.Txt_CodeNatureClient.IsReadOnly = Etat;
            this.Txt_LibelleNatureClient.IsReadOnly = Etat;

            this.Txt_CodeRegroupement.IsReadOnly = Etat;
            this.Txt_LibelleGroupeCode.IsReadOnly = Etat;
            this.Txt_Nationnalite.IsReadOnly = Etat;
            this.Txt_CodeNationalite.IsReadOnly = Etat;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialiseCtrl();
        }

        private void btn_CodeConsomateur_Click(object sender, RoutedEventArgs e)
        {
            if (LstCodeConsomateur!= null && LstCodeConsomateur.Count != 0)
            {
                this.btn_CodeConsomateur.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCodeConsomateur );
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste des codes consomateur");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnConsomateur);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnConsomateur(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCodeConsomateur _LeCodeSelect = (CsCodeConsomateur)ctrs.MyObject;
                this.Txt_CodeConsomateur.Text = _LeCodeSelect.CODE;
                this.Txt_CodeConsomateur.Tag  = _LeCodeSelect.PK_ID ;
            }
            this.btn_CodeConsomateur.IsEnabled = true;
        }

        private void btn_FermableClient_Click(object sender, RoutedEventArgs e)
        {

            if (LstFermable!= null && LstFermable.Count != 0)
            {
                this.btn_FermableClient.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstFermable);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnFermableClient);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnFermableClient(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsFermable _LaFermable = (CsFermable)ctrs.MyObject;
                this.Txt_CodeFermableClient.Text = _LaFermable.CODE;
                this.Txt_CodeFermableClient.Tag  = _LaFermable.PK_ID ;
            }
            this.btn_FermableClient.IsEnabled = true;
        }

        private void btn_Categorie_Click(object sender, RoutedEventArgs e)
        {
            if (LstCategorie!= null &&  LstCategorie.Count != 0)
            {
                this.btn_Categorie.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCategorie);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnCategorie);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnCategorie(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCategorieClient _LaCateg = (CsCategorieClient)ctrs.MyObject;
                this.Txt_CodeCategorie.Text = _LaCateg.CODE;
                this.Txt_CodeCategorie.Tag  = _LaCateg.PK_ID ;
            }
            this.btn_Categorie.IsEnabled = true;
        }

        private void btn_NatureClient_Click(object sender, RoutedEventArgs e)
        {
            if (LstNatureClient != null && LstNatureClient.Count != 0)
            {
                this.btn_NatureClient.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstNatureClient);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnNatureClient);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnNatureClient(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsNatureClient _LaNature = (CsNatureClient)ctrs.MyObject;
                this.Txt_CodeNatureClient.Text = _LaNature.CODE;
                this.Txt_CodeNatureClient.Tag  = _LaNature.PK_ID ;
            }
            this.btn_NatureClient.IsEnabled = true;

        }

        private void btn_Nationalite_Click(object sender, RoutedEventArgs e)
        {

            if (LstDesNationalites!= null && LstDesNationalites.Count != 0)
            {
                this.btn_Nationalite.IsEnabled = false;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDesNationalites );
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnNationnalite);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnNationnalite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsNationalite _LaNationnalite = (CsNationalite)ctrs.MyObject;
                this.Txt_CodeNationalite.Text = _LaNationnalite.CODE;
                this.Txt_CodeNationalite.Tag  = _LaNationnalite.PK_ID ;
                this.Txt_Nationnalite.Text = _LaNationnalite.LIBELLE;
            }
            this.btn_Nationalite.IsEnabled = true;

        }

        private void btn_Civilite_Click(object sender, RoutedEventArgs e)
        {
            //if (LstCivilite != null && LstCivilite.Count != 0)
            //{
            //    this.btn_Civilite.IsEnabled = false;
            //    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCivilite);
            //    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //    ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
            //    ctr.Show();
            //}
        }
        void galatee_OkClickedBtnCivilite(object sender, EventArgs e)
        {
            //UcListeGenerique ctrs = sender as UcListeGenerique;
            //if (ctrs.isOkClick)
            //{
            //    CsDenomination _LaDenoSelect = (CsDenomination)ctrs.MyObject;
            //    this.Txt_CodeCivilite.Text = _LaDenoSelect.CODE;
            //    this.Txt_CodeCivilite.Tag  = _LaDenoSelect.PK_ID ;
            //}
            //this.btn_Civilite.IsEnabled = true;

        }

        private void btn_CiviliteAgent_Click(object sender, RoutedEventArgs e)
        {
            //if (LstCivilite!= null && LstCivilite.Count != 0)
            //{
            //    this.btn_Civilite.IsEnabled = false;
            //    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCivilite);
            //    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
            //    ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
            //    ctr.Show();
            //}
        }
        void galatee_OkClickedBtnCiviliteAgent(object sender, EventArgs e)
        {
            //UcListeGenerique ctrs = sender as UcListeGenerique;
            //if (ctrs.isOkClick)
            //{
            //    CsDenomination _LaDenoSelect = (CsDenomination)ctrs.MyObject;
            //    ////this.Txt_CodeCiviliteAgent.Text = _LaDenoSelect.CODE;
            //}
            //this.btn_CiviliteAgent.IsEnabled = true;
        }

        private void btn_CodeRegroupement_Click(object sender, RoutedEventArgs e)
        {
            if (LstCodeRegroupement!= null && LstCodeRegroupement.Count != 0)
            {
                List<object> _Lstobj = ClasseMEthodeGenerique.RetourneListeObjet(LstCodeRegroupement);
                UcListeGenerique ctr = new UcListeGenerique(_Lstobj, "REGCLI", "NOM", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClicked);
                ctr.Show();
            }
        }
        void galatee_OkClicked(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
            CsRegCli _LeRegcli = (CsRegCli)ctrs.MyObject;
            //this.Txt_CodeCiviliteAgent.Text = _LeRegcli.CODE ;
            //this.Txt_CodeCiviliteAgent.Tag  = _LeRegcli.PK_ID ;
            }
            this.btn_CodeRegroupement.IsEnabled = true;
        }

        private void btn_autresInfo_Click(object sender, RoutedEventArgs e)
        {
            UcAutreInformation ctr = new UcAutreInformation();
            ctr.Closed += new EventHandler(galatee_OkClickedbtn_autresInfo);
            ctr.Show();
        }
        void galatee_OkClickedbtn_autresInfo(object sender, EventArgs e)
        {
            UcAutreInformation ctrs = sender as UcAutreInformation;

          LaDemande.LeClient.NOMMERE = ctrs.txtNomMere.Text;
          LaDemande.LeClient.NOMPERE = ctrs.txtNompere.Text;
          LaDemande.LeClient.CNI = ctrs.txtNumPieceId.Text;
          LaDemande.LeClient.MOISNAIS = string.IsNullOrEmpty(ctrs.dtp_DateNaissance.Text) ? string.Empty : DateTime.Parse(ctrs.dtp_DateNaissance.Text).Month.ToString();
          LaDemande.LeClient.ANNAIS = string.IsNullOrEmpty(ctrs.dtp_DateNaissance.Text) ? string.Empty : DateTime.Parse(ctrs.dtp_DateNaissance.Text).Year.ToString();
        }

        public void  EnregistrerDemande(CsDemande _LaDemande)
        {
            try
            {

                _LaDemande.LeClient.NUMDEM = string.IsNullOrEmpty(_LaDemande.LaDemande.NUMDEM) ? string.Empty : _LaDemande.LaDemande.NUMDEM;
                _LaDemande.LeClient.CENTRE = string.IsNullOrEmpty(_LaDemande.LaDemande.CENTRE) ? string.Empty : _LaDemande.LaDemande.CENTRE;
                _LaDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(_LaDemande.LaDemande.CLIENT) ? string.Empty : _LaDemande.LaDemande.CLIENT;
                _LaDemande.LeClient.ORDRE = string.IsNullOrEmpty(_LaDemande.LaDemande.ORDRE) ? string.Empty : _LaDemande.LaDemande.ORDRE;

                //_LaDemande.LeClient.DENABON = string.IsNullOrEmpty(this.Txt_CodeCivilite.Text) ? string.Empty : this.Txt_CodeCivilite.Text;
                _LaDemande.LeClient.NOMABON = string.IsNullOrEmpty(this.Txt_NomClientAbon.Text) ? string.Empty : this.Txt_NomClientAbon.Text;
                _LaDemande.LeClient.ADRMAND1 = string.IsNullOrEmpty(this.Txt_Addresse1.Text) ? string.Empty : this.Txt_Addresse1.Text;
                _LaDemande.LeClient.ADRMAND2 = string.IsNullOrEmpty(this.Txt_adresse2.Text) ? string.Empty : this.Txt_adresse2.Text;
                _LaDemande.LeClient.PROPRIO = (rdb_Owner.IsChecked == true) ? "1" : "0";
                _LaDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.Txt_telephone.Text) ? string.Empty : this.Txt_telephone.Text;
                _LaDemande.LeClient.CODECONSO = string.IsNullOrEmpty(this.Txt_CodeConsomateur.Text) ? string.Empty : this.Txt_CodeConsomateur.Text;
                _LaDemande.LeClient.CATEGORIE = string.IsNullOrEmpty(this.Txt_CodeCategorie.Text) ? string.Empty : this.Txt_CodeCategorie.Text;
                _LaDemande.LeClient.CODERELANCE = string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text) ? string.Empty  : this.Txt_CodeFermableClient.Text;
                _LaDemande.LeClient.NATIONNALITE = string.IsNullOrEmpty(this.Txt_CodeNationalite.Text) ? string.Empty : this.Txt_CodeNationalite.Text;
                _LaDemande.LeClient.NATURE = string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text) ? string.Empty : this.Txt_CodeNatureClient.Text;
                _LaDemande.LeClient.MODEPAIEMENT = string.IsNullOrEmpty(this.Txt_CodePaiement.Text) ? string.Empty  : this.Txt_CodePaiement.Text;
                _LaDemande.LeClient.REGCLI = string.IsNullOrEmpty(this.Txt_CodeRegroupement.Text) ? string.Empty  : this.Txt_CodeRegroupement.Text;
                _LaDemande.LeClient.NUMEROIDCLIENT = this.Txt_Numeronina.Text;
                _LaDemande.LeClient.USERCREATION = UserConnecte.matricule;
                _LaDemande.LeClient.USERMODIFICATION = UserConnecte.matricule;
                _LaDemande.LeClient.DATECREATION = System.DateTime.Now;
                _LaDemande.LeClient.DATEMODIFICATION = System.DateTime.Now;
                _LaDemande.LeClient.ISFACTUREEMAIL = (this.chk_Email.IsChecked == true) ? true : false;
                _LaDemande.LeClient.ISFACTURESMS = (this.chk_SMS.IsChecked == true) ? true : false;
                _LaDemande.LeClient.EMAIL = string.IsNullOrEmpty(this.Txt_Email.Text) ? string.Empty : this.Txt_Email.Text;
                _LaDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.Txt_telephone.Text) ? string.Empty : this.Txt_telephone.Text;

                _LaDemande.LeClient.FK_IDCATEGORIE = this.Txt_CodeCategorie.Tag == null ? _LaDemande.LeClient.FK_IDCATEGORIE : int.Parse(this.Txt_CodeCategorie.Tag.ToString());
                _LaDemande.LeClient.FK_IDCENTRE = _LaDemande.LaDemande.FK_IDCENTRE;
                _LaDemande.LeClient.FK_IDCODECONSO = this.Txt_CodeConsomateur.Tag == null ? _LaDemande.LeClient.FK_IDCODECONSO : int.Parse(this.Txt_CodeConsomateur.Tag.ToString());
                _LaDemande.LeClient.FK_IDMODEPAIEMENT = this.Txt_CodePaiement.Tag == null ? _LaDemande.LeClient.FK_IDMODEPAIEMENT : int.Parse(this.Txt_CodePaiement.Tag.ToString());
                _LaDemande.LeClient.FK_IDNATIONALITE = this.Txt_CodeNationalite.Tag == null ? _LaDemande.LeClient.FK_IDNATIONALITE : int.Parse(this.Txt_CodeNationalite.Tag.ToString());
                _LaDemande.LeClient.FK_IDNATURECLIENT = this.Txt_CodeNatureClient.Tag == null ? _LaDemande.LeClient.FK_IDNATURECLIENT : int.Parse(this.Txt_CodeNatureClient.Tag.ToString());
                _LaDemande.LeClient.FK_IDREGCLI = this.Txt_CodeRegroupement.Tag == null ? _LaDemande.LeClient.FK_IDREGCLI : int.Parse(this.Txt_CodeRegroupement.Tag.ToString());
                _LaDemande.LeClient.FK_IDRELANCE = this.Txt_CodeFermableClient.Tag == null ? _LaDemande.LeClient.FK_IDRELANCE  : int.Parse(this.Txt_CodeFermableClient.Tag.ToString());

                if (_LaDemande.LeClient.FK_IDREGCLI == 0) _LaDemande.LeClient.FK_IDREGCLI = null;
                if (_LaDemande.LeClient.FK_IDMODEPAIEMENT == 0) _LaDemande.LeClient.FK_IDMODEPAIEMENT = 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            EnregistrerDemande(LaDemande);
        }

        private void Txt_CodeCategorie_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeCategorie.Text.Length == SessionObject.Enumere.TailleCodeCategorie
                    && LstCategorie.Count != 0)
                {

                    CsCategorieClient LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(LstCategorie, Txt_CodeCategorie.Text, "CODE");
                        if (!string.IsNullOrEmpty(LaCategorie.LIBELLE))
                        {
                            this.Txt_LibelleCategorie.Text = LaCategorie.LIBELLE;
                            this.Txt_CodeCategorie.Tag = LaCategorie.PK_ID;
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_CodeCategorie.Focus();
                            };
                            w.Show();
                        }
                }
            }
            catch (Exception ex )
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeConsomateur_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeConsomateur.Text.Length == SessionObject.Enumere.TailleCodeConso
                    &&  LstCodeConsomateur.Count != 0)
                {
                    CsCodeConsomateur _leCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeConso.LIBELLE ))
                    {
                        this.Txt_LibelleCodeConso.Text = _leCodeConso.LIBELLE;
                        this.Txt_CodeConsomateur.Tag  = _leCodeConso.PK_ID ;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeConsomateur.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeNationalite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeNationalite.Text.Length == SessionObject.Enumere.TailleCodeNationalite  &&
                    LstDesNationalites.Count !=0)
                {
                    CsNationalite _leCodeNation = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeNation.LIBELLE ))
                    {
                        this.Txt_Nationnalite.Text = _leCodeNation.LIBELLE;
                        this.Txt_CodeNationalite.Tag = _leCodeNation.PK_ID;
                        EnregistrerDemande(LaDemande);
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeNationalite.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeNatureClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Txt_CodeNatureClient.Text.Length == SessionObject.Enumere.TailleCodeTypeClient &&
                LstNatureClient.Count != 0)
            {
                CsNatureClient _leCodeNature = ClasseMEthodeGenerique.RetourneObjectFromList(LstNatureClient, this.Txt_CodeNatureClient.Text, "CODE");
                if (!string.IsNullOrEmpty(_leCodeNature.LIBELLE))
                {
                    this.Txt_LibelleNatureClient.Text = _leCodeNature.LIBELLE;
                    this.Txt_CodeNatureClient.Tag  = _leCodeNature.PK_ID ;
                    EnregistrerDemande(LaDemande);
                }
                else
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        this.Txt_CodeNatureClient.Focus();
                    };
                    w.Show();
                }
            }
        }

        private void Txt_CodeFermableClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeFermableClient.Text.Length == SessionObject.Enumere.TailleCodeRelance && 
                    LstFermable.Count != 0)
                {
                    CsFermable _Fermable = ClasseMEthodeGenerique.RetourneObjectFromList(LstFermable, this.Txt_CodeFermableClient.Text, "CODE");
                        if (!string.IsNullOrEmpty(_Fermable.LIBELLE ))
                        {
                            this.Txt_LibelleFermable.Text = _Fermable.LIBELLE;
                            this.Txt_CodeFermableClient.Tag  = _Fermable.PK_ID ;
                            EnregistrerDemande(LaDemande);
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_CodeFermableClient.Focus();
                            };
                            w.Show();
                        }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeRegroupement_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodeRegroupement.Text.Length == SessionObject.Enumere.TailleCodeRegroupement
                    && LstCodeRegroupement.Count != 0)
                {

                    CsRegCli  LeRegroupement = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeRegroupement,this.Txt_CodeRegroupement .Text, "CODE");
                    if (!string.IsNullOrEmpty(LeRegroupement.NOM ))
                    {
                        this.Txt_LibelleCategorie.Text = LeRegroupement.NOM;
                        this.Txt_CodeRegroupement.Tag = LeRegroupement.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCategorie.Focus();
                        };
                        w.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }

        private void Txt_NomClientAbon_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnregistrerDemande(LaDemande);
        }

        private void btn_Modepaiement_Click(object sender, RoutedEventArgs e)
        {
            if (SessionObject.LstModePaiement .Count != 0)
            {
                this.btn_Modepaiement.IsEnabled = true ;
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstModePaiement);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClickedBtnModePAie);
                ctr.Show();
            }
        }
        void galatee_OkClickedBtnModePAie(object sender, EventArgs e)
        {
            UcListeGenerique  ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsModepaiement leMode = (CsModepaiement)ctrs.MyObject;
                this.Txt_CodePaiement.Text = leMode.CODE;
                this.Txt_LibelleModePaiement .Text = leMode.LIBELLE ;
                this.btn_Modepaiement.IsEnabled = false ;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
             UcDomiciliationBanque ctr = new UcDomiciliationBanque();
             ctr.Closed += new EventHandler(galatee_OkClickedUcDomiciliationBanquet);
            ctr.Show();
        }
        void galatee_OkClickedUcDomiciliationBanquet(object sender, EventArgs e)
        {
            UcDomiciliationBanque ctrs = sender as UcDomiciliationBanque;
            LaDemande.LeClient.COMPTE = ctrs.Txt_Compte.Text;
            LaDemande.LeClient.BANQUE = ctrs.Txt_Banque.Text;
            LaDemande.LeClient.GUICHET = ctrs.Txt_Guichet.Text;
            LaDemande.LeClient.RIB = ctrs.Txt_Rib.Text;
        }

        private void Txt_CodeCivilite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite && (LstCivilite != null && LstCivilite.Count != 0))
                //{
                //    CsDenomination _laCivilite = LstCivilite.FirstOrDefault(c=>c.CODE==this.Txt_CodeCivilite.Text);
                //    if (_laCivilite != null && !string.IsNullOrEmpty(_laCivilite.LIBELLE))
                //    {
                //        this.Txt_LibelleCivilite.Text = _laCivilite.LIBELLE;
                //        this.Txt_CodeCivilite.Tag = _laCivilite.PK_ID;
                //        EnregistrerDemande(LaDemande);
                //    }
                //    else
                //    {
                //        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        w.OnMessageBoxClosed += (_, result) =>
                //        {
                //            this.Txt_CodeCivilite.Focus();
                //            this.Txt_CodeCivilite.Text = string.Empty;
                //            this.Txt_LibelleCivilite.Text = string.Empty;
                //        };
                //        w.Show();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_CodeCivilite_LostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    HandleLostFocus<CsDenomination>((TextBox)sender, this.Txt_LibelleCivilite, LstCivilite, SessionObject.Enumere.TailleCodeCivilite);
            //}
            //catch (Exception ex)
            //{
            //    Message.ShowError(ex.Message, Langue.lbl_Menu);
            //}
        }


        private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems,int Taille)
        {
            if (!string.IsNullOrEmpty(Code.Text) &&
                Code.Text.Length == Taille &&
                listItems.Count != 0)
            {
                Code.Text = Code.Text.PadLeft(Taille, '0');
            }
            else
            {
                Code.Text = string.Empty;
                Libelle.Text = string.Empty;
            }
        }

        private void RemplirLibelle()
        {
            if (this.Txt_CodeCategorie.Text.Length == SessionObject.Enumere.TailleCodeCategorie
                && LstCategorie.Count != 0)
            {

                CsCategorieClient LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(LstCategorie, Txt_CodeCategorie.Text, "CODE");
                if (!string.IsNullOrEmpty(LaCategorie.LIBELLE))
                    this.Txt_LibelleCategorie.Text = LaCategorie.LIBELLE;
            }
            //if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite && (LstCivilite != null && LstCivilite.Count != 0))
            //{
            //    CsDenomination _laCivilite = LstCivilite.FirstOrDefault(c => c.CODE == this.Txt_CodeCivilite.Text);
            //    if (_laCivilite != null && !string.IsNullOrEmpty(_laCivilite.LIBELLE))
            //        this.Txt_LibelleCivilite.Text = _laCivilite.LIBELLE;
            //}
            if (this.Txt_CodeNationalite.Text.Length == SessionObject.Enumere.TailleCodeNationalite &&
                LstDesNationalites.Count != 0)
            {
                CsNationalite _leCodeNation = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                if (!string.IsNullOrEmpty(_leCodeNation.LIBELLE))
                    this.Txt_Nationnalite.Text = _leCodeNation.LIBELLE;
            }
            if (this.Txt_CodeConsomateur.Text.Length == SessionObject.Enumere.TailleCodeConso
              && LstCodeConsomateur.Count != 0)
            {
                CsCodeConsomateur _leCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                if (!string.IsNullOrEmpty(_leCodeConso.LIBELLE))
                    this.Txt_LibelleCodeConso.Text = _leCodeConso.LIBELLE;
            }
            if (this.Txt_CodeFermableClient.Text.Length == SessionObject.Enumere.TailleCodeRelance &&
                 LstFermable.Count != 0)
            {
                CsFermable _Fermable = ClasseMEthodeGenerique.RetourneObjectFromList(LstFermable, this.Txt_CodeFermableClient.Text, "CODE");
                if (!string.IsNullOrEmpty(_Fermable.LIBELLE))
                    this.Txt_LibelleFermable.Text = _Fermable.LIBELLE;
            }
            if (this.Txt_CodeNatureClient.Text.Length == SessionObject.Enumere.TailleCodeTypeClient &&
                 LstNatureClient.Count != 0)
            {
                CsNatureClient _leCodeNature = ClasseMEthodeGenerique.RetourneObjectFromList(LstNatureClient, this.Txt_CodeNatureClient.Text, "CODE");
                if (!string.IsNullOrEmpty(_leCodeNature.LIBELLE))
                    this.Txt_LibelleNatureClient.Text = _leCodeNature.LIBELLE;
            }

        }

        private void Txt_CodePaiement_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.Txt_CodePaiement.Text.Length == 1
                    && LstModePaiement.Count != 0)
                {

                    CsModepaiement   LeModePaiement = ClasseMEthodeGenerique.RetourneObjectFromList(LstModePaiement,this.Txt_CodePaiement.Text, "CODE");
                    if (!string.IsNullOrEmpty(LeModePaiement.CODE  ))
                    {
                        this.Txt_LibelleModePaiement.Text = LeModePaiement.LIBELLE  ;
                        this.Txt_CodePaiement.Tag = LeModePaiement.PK_ID;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodePaiement.Focus();
                        };
                        w.Show();
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
