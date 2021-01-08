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
//using Galatee.Silverlight.serviceWeb;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Shared;
using System.IO;
namespace Galatee.Silverlight.Accueil
{
    public partial class FrmModicationClient : ChildWindow
    {
        #region Variables

            #region Listes
                List<CsFermable> LstFermable;
                List<CsDenomination > LstCivilite;
                List<CsRegCli> LstCodeRegroupement;
                List<CsCategorieClient> LstCategorie;
                List<CsNatureClient > LstNatureClient;
                List<CsNationalite> LstDesNationalites;
                List<CsCodeConsomateur> LstCodeConsomateur;
                List<CsModepaiement> LstModePaiement = new List<CsModepaiement>();
                List<CsModepaiement> LstDesModePaiement = new List<CsModepaiement>();
                List<CsCentre> LstCentre = new List<CsCentre>();
                List<CsProduit> ListeDesProduitDuSite = new List<CsProduit>();
                CsProduit LeProduitSelect = new CsProduit();


            #endregion

            #region Objets Globaux
        
            CsDevis LeDevis = new CsDevis();
            CsClient LeClient = new CsClient();
            CsClient  LeClientRecherche = new CsClient();
            CsDenomination lacivilite = new CsDenomination();
            public CsDemande LaDemande = new CsDemande();
            CsCentre LeCentreSelect = new CsCentre();
            bool IsUpate = false;

            #endregion

        #endregion

        #region Constructeurs

        
            public FrmModicationClient()
            {
                InitializeComponent();
                this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
                this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
            }

            public FrmModicationClient(CsDemande _LaDemande)
            {
                this.Title = "Modification du client";
                InitializeComponent();
                Translate();
                LaDemande = _LaDemande;
                TypeDemande = LaDemande.LaDemande.TYPEDEMANDE;
                ChargerCategorie();
                ChargerCodeConsomateur();
                ChargerNationnalite();
                ChargerNatureClient();
                ChargerFermable();
                ChargerCivilite();
                ChargerDonneeDuSite();
                ChargerListeDeProduit();
                ChargerModePaiement();


                isUpdate = true;
                this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;


                LeClientRecherche = _LaDemande.LeClient ;
                _LaDemande.LaDemande.STATUTDEMANDE = null;

                this.Txt_CodeCentre.Text = string.IsNullOrEmpty(LeClientRecherche.CENTRE) ? string.Empty : LeClientRecherche.CENTRE;
                this.Txt_CodeProduit.Text = string.IsNullOrEmpty(_LaDemande.LaDemande.PRODUIT) ? string.Empty : _LaDemande.LaDemande.PRODUIT;
                this.Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                AfficherInformationClient(LeClientRecherche);
                RemplirLibelle();

                this.Txt_CodeCentre.IsReadOnly = true;
                this.Txt_CodeProduit.IsReadOnly = true;
                this.Txt_NumDemande.IsReadOnly = true;
                this.Txt_Client.IsReadOnly = true;
                this.btn_Rechercher.IsEnabled = false;
                this.btn_Centre.IsEnabled = false;
                this.btn_Produit.IsEnabled = false;
                this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

                this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
                this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;

                if (LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                    RetourneObjectScan(LaDemande.LaDemande);

            }
            ObjDOCUMENTSCANNE leObjectScan = new ObjDOCUMENTSCANNE();
            private void RetourneObjectScan(CsDemandeBase laDemande)
            {
                try
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ReturneObjetScanCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        leObjectScan = args.Result;
                        if (leObjectScan != null)
                        {
                            this.lnkLetter.Content = Langue.msgVoirPiecejointe;
                            this.lnkLetter.Tag = leObjectScan.CONTENU;
                            this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                            this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                        }
                    };
                    service.ReturneObjetScanAsync(laDemande);
                    service.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            string TypeDemande = string.Empty;
            bool isUpdate = false;
            public FrmModicationClient(string _typeDemande)
            {
                //Intialisation du des contrôles de l'UI
                TypeDemande = _typeDemande;
                if (LaDemande.LaDemande == null) LaDemande.LaDemande = new CsDemandeBase();
                if (LaDemande.LeClient == null) LaDemande.LeClient = new CsClient();
                InitializeComponent();
                ChargerCategorie();
                ChargerCodeConsomateur();
                ChargerNationnalite();
                ChargerNatureClient();
                ChargerFermable();
                ChargerCivilite();
                ChargerDonneeDuSite();
                ChargerListeDeProduit();
                ChargerModePaiement();
                Translate();
                this.lnkMotif.Visibility = System.Windows.Visibility.Collapsed;

                this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;

            }

        #endregion
            void Translate()
            {

                // Gestion de la langue
                this.lbl_categoie.Content = Langue.lbl_categoie;
                this.lbl_Client .Content = Langue.lbl_client;
                this.lbl_CodeConsomateur.Content = Langue.lbl_consommation;
                this.lbl_CodeRegroupement.Content = Langue.lbl_CodeRegroupement;
                this.lbl_CodeRelance.Content = Langue.lbl_CodeRelance;
                this.lbl_denom.Content = Langue.lbl_denom;
                this.lbl_ModePayement.Content = Langue.lbl_ModePayement;
                this.lbl_Nationnalite.Content = Langue.lbl_Nationnalite;
                this.lbl_NomAgent.Content = Langue.lbl_name;
                this.btn_autresInfo.Content = Langue.lbl_autresInfo;
                this.lbl_denom .Content = Langue.lbl_denom;
                this.lbl_Nom .Content = Langue.lbl_name;
                this.rdb_Owner.Content = Langue.rdb_landlord;
                this.rdb_tenant .Content = Langue.rdb_tenant;
                this.lbl_TypeClient.Content = Langue.lbl_Nature;

                //
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
                                this.Txt_LibelleCategorie.Text = LstCategorie[0].LIBELLE;
                                LeClient.CATEGORIE = LstCategorie[0].CODE;
                                this.Txt_CodeCategorie.Tag = LstCategorie[0].PK_ID;
                                EnregistrerDemande(LaDemande, false);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeCategorie.Text))
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
                                    LeClient.CATEGORIE = LstCategorie[0].CODE;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(LeClient.CATEGORIE))
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
                                this.Txt_LibelleFermable.Text = LstFermable[0].LIBELLE;
                                this.Txt_CodeFermableClient.Tag = LstFermable[0].PK_ID;
                                LeClient.CODERELANCE = LstFermable[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text))
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
                                    LeClient.CODERELANCE = LstFermable[0].CODE;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text))
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
                                this.Txt_LibelleNatureClient.Text = LstNatureClient[0].LIBELLE;
                                this.Txt_CodeNatureClient.Tag = LstNatureClient[0].PK_ID;
                                LeClient.NATURE = LstNatureClient[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text))
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
                                    LeClient.NATURE = LstNatureClient[0].CODE;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text))
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
                            EnregistrerDemande(LaDemande, false);
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
                            EnregistrerDemande(LaDemande, false);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(this.Txt_CodePaiement.Text))
                            {
                                CsModepaiement _LeModePaiement = ClasseMEthodeGenerique.RetourneObjectFromList(LstModePaiement, this.Txt_CodePaiement.Text, "CODE");
                                if (!string.IsNullOrEmpty(_LeModePaiement.LIBELLE))
                                    this.Txt_CodePaiement.Text = _LeModePaiement.CODE;
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
                                this.Txt_Nationnalite.Text = LstDesNationalites[0].LIBELLE;
                                this.Txt_CodeNationalite.Tag = LstDesNationalites[0].PK_ID;
                                LeClient.NATIONNALITE = LstDesNationalites[0].CODE;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(this.Txt_CodeNationalite.Text))
                                {
                                    CsNationalite _LaNationalite = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODENATIONNALITE");
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
                                    LeClient.NATIONNALITE = LstDesNationalites[0].CODE;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(this.Txt_CodeNationalite.Text))
                                    {
                                        CsNationalite _LaNationalite = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODENATIONNALITE");
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
                                LeClient.CODECONSO = LstCodeConsomateur[0].CODE;
                                this.Txt_CodeConsomateur.Tag = LstCodeConsomateur[0].PK_ID;
                                EnregistrerDemande(LaDemande, false);

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
                                    LeClient.CODECONSO = LstCodeConsomateur[0].CODE;
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
                    LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODESITE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1 )
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODESITE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    if (LstCentre != null)
                    {
                        List<CsCentre> _LstCentre = LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstCentre.Count == 1)
                        {
                            this.Txt_CodeCentre.Text = _LstCentre[0].CODE;
                            this.Txt_LibelleCentre.Text = _LstCentre[0].LIBELLE;
                        }
                        else
                        {
                            CsCentre _LeCentre = new CsCentre();
                            if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                                _LeCentre = LstCentre.FirstOrDefault(p => p.CODE == this.Txt_CodeCentre.Text);
                            if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                            {
                                this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                                this.btn_Centre.IsEnabled = false;
                                this.Txt_CodeCentre.IsReadOnly = true;
                            }
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
            private void ChargerListeDeProduit()
            {
                try
                {
                    if (SessionObject.ListeDesProduit.Count != 0)
                    {
                        ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                        if (ListeDesProduitDuSite != null)
                        {
                            if (ListeDesProduitDuSite.Count == 1)
                            {
                                this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                this.Txt_CodeProduit.Tag = ListeDesProduitDuSite[0].PK_ID;
                                this.btn_Produit.IsEnabled = false;
                            }
                            else
                            {
                                //CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                                //if (_LeProduit != null)
                                //{
                                //    this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                                //    this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                                //    this.btn_Produit.IsEnabled = false;
                                //    this.Txt_CodeProduit.IsReadOnly = true;
                                //}
                            }
                        }

                    }
                    else
                    {
                        AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service1.ListeDesProduitCompleted += (sr, res) =>
                        {
                            if (res != null && res.Cancelled)
                                return;
                            SessionObject.ListeDesProduit = res.Result;
                            ListeDesProduitDuSite = SessionObject.ListeDesProduit;
                            if (ListeDesProduitDuSite != null)
                            {
                                if (ListeDesProduitDuSite.Count == 1)
                                {
                                    this.Txt_CodeProduit.Text = ListeDesProduitDuSite[0].CODE;
                                    this.Txt_LibelleProduit.Text = ListeDesProduitDuSite[0].LIBELLE;
                                    this.btn_Produit.IsEnabled = false;
                                }
                                else
                                {
                                    CsProduit _LeProduit = ListeDesProduitDuSite.FirstOrDefault(p => p.CODE == LaDemande.LaDemande.PRODUIT);
                                    if (_LeProduit != null)
                                    {
                                        this.Txt_CodeProduit.Text = LaDemande.LaDemande.PRODUIT;
                                        this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                                        this.btn_Produit.IsEnabled = false;
                                        this.Txt_CodeProduit.IsReadOnly = true;
                                    }
                                }
                            }
                        };
                        service1.ListeDesProduitAsync();
                        service1.CloseAsync();
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
                    if (isUpdate)
                    {
                        if (!string.IsNullOrEmpty(LaDemande.LaDemande.ANNOTATION))
                            this.lnkMotif.Visibility = System.Windows.Visibility.Visible;
                    }
                  
                }
                catch (Exception EX)
                {
                    Message.ShowError(EX.Message, Langue.lbl_Menu);
                }

            }
            private void RetourneInfoClient(int fk_idcentre,string centre, string client, string ordre)
            {
                try
                {
                    int res = LoadingManager.BeginLoading(Langue.En_Cours);
                    LeClientRecherche = new CsClient();
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneClientCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        LeClientRecherche = args.Result;
                        if (LeClientRecherche != null)
                            AfficherInformationClient(LeClientRecherche);
                        else 
                        Message.ShowInformation("Aucune information trouvée", "Recherche de branchement");
                        LoadingManager.EndLoading(res);
                    };
                    service.RetourneClientAsync(fk_idcentre,centre, client, ordre);
                    service.CloseAsync();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

            }
            private void AfficherInformationClient(CsClient _LeClient)
            {
                if (TypeDemande != SessionObject.Enumere.ModificationClient)
                {
                    this.Txt_Client.Text = string.IsNullOrEmpty(LaDemande.LaDemande.CLIENT) ? string.Empty : LaDemande.LaDemande.CLIENT;
                    this.Txt_Ordre.Text = string.IsNullOrEmpty(LaDemande.LaDemande.ORDRE) ? string.Empty : LaDemande.LaDemande.ORDRE;
                }
                else
                {
                    this.Txt_Client.Text = string.IsNullOrEmpty(_LeClient.REFCLIENT) ? string.Empty : _LeClient.REFCLIENT;
                    this.Txt_Ordre.Text = string.IsNullOrEmpty(_LeClient.ORDRE ) ? string.Empty : _LeClient.ORDRE ;
                }
                if (_LeClient.PROPRIO != null && string.IsNullOrEmpty(_LeClient.PROPRIO))
                    this.rdb_tenant .IsChecked = true;
                else
                    this.rdb_Owner.IsChecked = true;

                this.Txt_NomClientAbon.Text = string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON;
                if (string.IsNullOrEmpty(this.Txt_NomClientAbon.Text))
                    this.Txt_NomClientAbon.Text = (string.IsNullOrEmpty(_LeClient.DENABON) ? string.Empty : _LeClient.DENABON) + (string.IsNullOrEmpty(_LeClient.NOMABON) ? string.Empty : _LeClient.NOMABON);
                this.Txt_telephone.Text = string.IsNullOrEmpty(_LeClient.TELEPHONE) ? string.Empty : _LeClient.TELEPHONE;
                this.Txt_Addresse1.Text = string.IsNullOrEmpty(_LeClient.ADRMAND1) ? string.Empty : _LeClient.ADRMAND1;
                this.Txt_adresse2.Text = string.IsNullOrEmpty(_LeClient.ADRMAND2) ? string.Empty : _LeClient.ADRMAND2;

                this.Txt_CodeConsomateur.Text = string.IsNullOrEmpty(_LeClient.CODECONSO) ? string.Empty : _LeClient.CODECONSO;
                this.Txt_CodeCategorie.Text = string.IsNullOrEmpty(_LeClient.CATEGORIE) ? string.Empty : _LeClient.CATEGORIE;
                this.Txt_CodeFermableClient.Text = string.IsNullOrEmpty(_LeClient.CODERELANCE) ? string.Empty : _LeClient.CODERELANCE;
                this.Txt_CodeNatureClient.Text = string.IsNullOrEmpty(_LeClient.NATURE) ? string.Empty : _LeClient.NATURE;
                this.Txt_CodeNationalite.Text = string.IsNullOrEmpty(_LeClient.NATIONNALITE) ? string.Empty : _LeClient.NATIONNALITE;
                this.Txt_CodePaiement.Text = string.IsNullOrEmpty(_LeClient.MODEPAIEMENT) ? string.Empty : _LeClient.MODEPAIEMENT;

                if (TypeDemande == SessionObject.Enumere.Resiliation ||
                    TypeDemande == SessionObject.Enumere.Reabonnement)
                    ActionControle(true);
                else if (TypeDemande == SessionObject.Enumere.ModificationClient)
                    ActionControle(false);

            }
            private void ActionControle(bool Etat)
            {
                this.btn_autresInfo.IsEnabled = !Etat;
                this.btn_Categorie.IsEnabled = !Etat;
                this.btn_Civilite.IsEnabled = !Etat;
                this.btn_CiviliteAgent.IsEnabled = !Etat;
                this.btn_CodeConsomateur.IsEnabled = !Etat;
                this.btn_CodeRegroupement.IsEnabled = !Etat;
                this.btn_Nationalite.IsEnabled = !Etat;
                this.btn_FermableClient.IsEnabled = !Etat;
                this.btn_NatureClient.IsEnabled = !Etat;
                this.btn_Categorie.IsEnabled = !Etat;

                this.Txt_Client.IsReadOnly = Etat;
                this.Txt_Ordre.IsReadOnly = Etat;
                this.Txt_CodeCivilite.IsReadOnly = Etat;
                this.Txt_LibelleCivilite.IsReadOnly = Etat;
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

     

            private void btn_CodeConsomateur_Click(object sender, RoutedEventArgs e)
            {
                if (LstCodeConsomateur.Count != 0)
                {
                    this.btn_CodeConsomateur.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCodeConsomateur);
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
                }
                this.btn_CodeConsomateur.IsEnabled = true;
            }

            private void btn_FermableClient_Click(object sender, RoutedEventArgs e)
            {

                if (LstFermable.Count != 0)
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
                }
                this.btn_FermableClient.IsEnabled = true;
            }

            private void btn_Categorie_Click(object sender, RoutedEventArgs e)
            {
                if (LstCategorie.Count != 0)
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
                }
                this.btn_Categorie.IsEnabled = true;
            }

            private void btn_NatureClient_Click(object sender, RoutedEventArgs e)
            {
                if (LstNatureClient.Count != 0)
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
                }
                this.btn_NatureClient.IsEnabled = true;

            }

            private void btn_Nationalite_Click(object sender, RoutedEventArgs e)
            {

                if (LstDesNationalites.Count != 0)
                {
                    this.btn_Nationalite.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDesNationalites);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODENATIONNALITE", "LIBELLE", "Liste");
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
                }
                this.btn_Nationalite.IsEnabled = true;

            }

            private void btn_Civilite_Click(object sender, RoutedEventArgs e)
            {
                if (LstCivilite.Count != 0)
                {
                    this.btn_Civilite.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCivilite);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
                    ctr.Show();
                }
            }
            void galatee_OkClickedBtnCivilite(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsDenomination _LaDenoSelect = (CsDenomination)ctrs.MyObject;
                    this.Txt_CodeCivilite.Text = _LaDenoSelect.CODE;
                }
                this.btn_Civilite.IsEnabled = true;

            }

            private void btn_CiviliteAgent_Click(object sender, RoutedEventArgs e)
            {
                if (LstCategorie.Count != 0)
                {
                    this.btn_Civilite.IsEnabled = false;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCivilite);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnCivilite);
                    ctr.Show();
                }
            }
            void galatee_OkClickedBtnCiviliteAgent(object sender, EventArgs e)
            {
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsDenomination _LaDenoSelect = (CsDenomination)ctrs.MyObject;
                    this.Txt_CodeCiviliteAgent.Text = _LaDenoSelect.CODE;
                }
                this.btn_CiviliteAgent.IsEnabled = true;
            }

            private void btn_CodeRegroupement_Click(object sender, RoutedEventArgs e)
            {
                if (LstCodeRegroupement != null && LstCodeRegroupement.Count != 0)
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
                    this.Txt_CodeCiviliteAgent.Text = _LeRegcli.CODE ;
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

                LeClient.NOMMERE = ctrs.txtNomMere.Text;
                LeClient.NOMPERE = ctrs.txtNompere.Text;
                LeClient.CNI = ctrs.txtNumPieceId.Text;
                LeClient.MOISNAIS = string.IsNullOrEmpty(ctrs.dtp_DateNaissance.Text) ? string.Empty : DateTime.Parse(ctrs.dtp_DateNaissance.Text).Month.ToString();
                LeClient.ANNAIS = string.IsNullOrEmpty(ctrs.dtp_DateNaissance.Text) ? string.Empty : DateTime.Parse(ctrs.dtp_DateNaissance.Text).Year.ToString();
            }
            public void EnregistrerDemande(CsDemande _LaDemande, bool Isgeneral)
            {
                try
                {
                    if (Isgeneral)
                    {
                        if (LaDemande.LeClient != null) LeClient = LaDemande.LeClient;
                        else LeClient = new CsClient();
                        AfficherInformationClient(LeClient);
                    }
                   LaDemande.LeClient = LeClientRecherche;
                   LaDemande.LeClient.NUMDEM = string.IsNullOrEmpty(_LaDemande.LaDemande.NUMDEM) ? string.Empty : _LaDemande.LaDemande.NUMDEM;
                   LaDemande.LeClient.CENTRE = string.IsNullOrEmpty(_LaDemande.LaDemande.CENTRE) ? string.Empty : _LaDemande.LaDemande.CENTRE;
                   LaDemande.LeClient.REFCLIENT = string.IsNullOrEmpty(_LaDemande.LaDemande.CLIENT) ? string.Empty : _LaDemande.LaDemande.CLIENT;
                   LaDemande.LeClient.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? string.Empty : this.Txt_Ordre.Text;

                   LaDemande.LeClient.DENABON = string.IsNullOrEmpty(this.Txt_CodeCivilite.Text) ? string.Empty : this.Txt_CodeCivilite.Text;
                   LaDemande.LeClient.NOMABON = string.IsNullOrEmpty(this.Txt_NomClientAbon.Text) ? string.Empty : this.Txt_NomClientAbon.Text;
                   LaDemande.LeClient.ADRMAND1 = string.IsNullOrEmpty(this.Txt_Addresse1.Text) ? string.Empty : this.Txt_Addresse1.Text;
                   LaDemande.LeClient.ADRMAND2 = string.IsNullOrEmpty(this.Txt_adresse2.Text) ? string.Empty : this.Txt_adresse2.Text;
                   LaDemande.LeClient.PROPRIO = (rdb_Owner.IsChecked == true) ? "1" : "0";
                   LaDemande.LeClient.TELEPHONE = string.IsNullOrEmpty(this.Txt_telephone.Text) ? string.Empty : this.Txt_telephone.Text;
                   LaDemande.LeClient.CODECONSO = string.IsNullOrEmpty(this.Txt_CodeConsomateur.Text) ? string.Empty : this.Txt_CodeConsomateur.Text;
                   LaDemande.LeClient.CATEGORIE = string.IsNullOrEmpty(this.Txt_CodeCategorie.Text) ? string.Empty : this.Txt_CodeCategorie.Text;
                   LaDemande.LeClient.CODERELANCE = string.IsNullOrEmpty(this.Txt_CodeFermableClient.Text) ? "0" : this.Txt_CodeFermableClient.Text;
                   LaDemande.LeClient.NATIONNALITE = string.IsNullOrEmpty(this.Txt_CodeNationalite.Text) ? string.Empty : this.Txt_CodeNationalite.Text;
                   LaDemande.LeClient.NATURE = string.IsNullOrEmpty(this.Txt_CodeNatureClient.Text) ? string.Empty : this.Txt_CodeNatureClient.Text;
                   LaDemande.LeClient.MODEPAIEMENT = string.IsNullOrEmpty(this.Txt_CodePaiement.Text) ? "0" : this.Txt_CodePaiement.Text;
                   LaDemande.LeClient.REGCLI = string.IsNullOrEmpty(this.Txt_CodeRegroupement.Text) ? "0000" : this.Txt_CodeRegroupement.Text;



                   LaDemande.LeClient.USERCREATION = UserConnecte.matricule;
                   LaDemande.LeClient.USERMODIFICATION = UserConnecte.matricule;
                   LaDemande.LeClient.DATECREATION =  System.DateTime.Now;
                   LaDemande.LeClient.DATEMODIFICATION =  System.DateTime.Now;

                    LaDemande.LaDemande.STATUT = SessionObject.Enumere.DemandeStatusEnAttente;
                    LaDemande.LaDemande.TYPEDEMANDE = TypeDemande;
                    LaDemande.LaDemande.CLIENT = LeClientRecherche.REFCLIENT;
                    LaDemande.LaDemande.ORDRE = LeClientRecherche.ORDRE;
                    LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                    LaDemande.LaDemande.USERMODIFICATION = UserConnecte.matricule;
                    LaDemande.LaDemande.DATECREATION =  System.DateTime.Now;
                    LaDemande.LaDemande.DATEMODIFICATION =  System.DateTime.Now;

                    if (lnkLetter.Tag != null)
                    {
                        leDoc = SaveFile((byte[])lnkLetter.Tag, 1, null);
                        //_Lademande.LeDocumentScanne = leDoc;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private void UserControl_LostFocus(object sender, RoutedEventArgs e)
            {
                EnregistrerDemande(LaDemande, false);
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
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }

            private void Txt_CodeConsomateur_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (this.Txt_CodeConsomateur.Text.Length == SessionObject.Enumere.TailleCodeConso
                        && LstCodeConsomateur.Count != 0)
                    {
                        CsCodeConsomateur _leCodeConso = ClasseMEthodeGenerique.RetourneObjectFromList(LstCodeConsomateur, this.Txt_CodeConsomateur.Text, "CODE");
                        if (!string.IsNullOrEmpty(_leCodeConso.LIBELLE))
                        {
                            this.Txt_LibelleCodeConso.Text = _leCodeConso.LIBELLE;
                            this.Txt_LibelleCodeConso.Tag = _leCodeConso.PK_ID;
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
                    if (this.Txt_CodeNationalite.Text.Length == SessionObject.Enumere.TailleCodeNationalite &&
                        LstDesNationalites.Count != 0)
                    {
                        CsNationalite _leCodeNation = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                        if (!string.IsNullOrEmpty(_leCodeNation.LIBELLE))
                        {
                            this.Txt_Nationnalite.Text = _leCodeNation.LIBELLE;
                            this.Txt_Nationnalite.Tag = _leCodeNation.PK_ID;
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
                        this.Txt_LibelleNatureClient.Tag = _leCodeNature.PK_ID;
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
                        if (!string.IsNullOrEmpty(_Fermable.LIBELLE))
                        {
                            this.Txt_LibelleFermable.Text = _Fermable.LIBELLE;
                            this.Txt_LibelleFermable.Tag = _Fermable.PK_ID;
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
                //GetInformationFromSream(LaDemande);

            }

            private void Txt_NomClientAbon_TextChanged(object sender, TextChangedEventArgs e)
            {
                EnregistrerDemande(LaDemande, false);
            }

            private void btn_Modepaiement_Click(object sender, RoutedEventArgs e)
            {
                if (SessionObject.LstModePaiement.Count != 0)
                {
                    this.btn_Modepaiement.IsEnabled = true;
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(SessionObject.LstModePaiement);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedBtnModePAie);
                    ctr.Show();
                }
            }
            void galatee_OkClickedBtnModePAie(object sender, EventArgs e)
            {
                UcListeTa ctrs = sender as UcListeTa;
                //this.Txt_CodePaiement.Text = ctrs.MyElt.VALEUR.Substring(2, 4);
                //this.Txt_LibelleModePaiement.Text = ctrs.MyElt.LIBELLE;
                this.btn_Modepaiement.IsEnabled = true;
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
                LeClient.COMPTE = ctrs.Txt_Compte.Text;
                LeClient.BANQUE = ctrs.Txt_Banque.Text;
                LeClient.GUICHET = ctrs.Txt_Guichet.Text;
                LeClient.RIB = ctrs.Txt_Rib.Text;
            }

            private void Txt_CodeCivilite_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite && (LstCivilite != null && LstCivilite.Count != 0))
                    {
                        CsDenomination _laCivilite = LstCivilite.FirstOrDefault(c => c.CODE == this.Txt_CodeCivilite.Text);
                        if (_laCivilite != null && !string.IsNullOrEmpty(_laCivilite.LIBELLE))
                        {
                            this.Txt_LibelleCivilite.Text = _laCivilite.LIBELLE;
                            this.Txt_LibelleCivilite.Tag = _laCivilite.PK_ID;
                            lacivilite = _laCivilite;
                            EnregistrerDemande(LaDemande, false);
                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_CodeCivilite.Focus();
                                this.Txt_CodeCivilite.Text = string.Empty;
                                this.Txt_LibelleCivilite.Text = string.Empty;
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

            private void Txt_CodeCivilite_LostFocus(object sender, RoutedEventArgs e)
            {
                try
                {
                    HandleLostFocus<CsDenomination>((TextBox)sender, this.Txt_LibelleCivilite, LstCivilite, SessionObject.Enumere.TailleCodeCivilite);
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }


            private void HandleLostFocus<T>(TextBox Code, TextBox Libelle, List<T> listItems, int Taille)
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

            //private void Txt_CodeCivilite_TextChanged(object sender, TextChangedEventArgs e)
            //{
            //    try
            //    {
            //        if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite && (LstCivilite != null && LstCivilite.Count != 0))
            //        {
            //            CsDenomination _laCivilite = LstCivilite.FirstOrDefault(c => c.CODE == this.Txt_CodeCivilite.Text);
            //            if (_laCivilite != null && !string.IsNullOrEmpty(_laCivilite.LIBELLE))
            //            {
            //                this.Txt_LibelleCivilite.Text = _laCivilite.LIBELLE;
            //                lacivilite = _laCivilite;
            //                EnregistrerDemande(LaDemande, false);
            //            }
            //            else
            //            {
            //                var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //                w.OnMessageBoxClosed += (_, result) =>
            //                {
            //                    this.Txt_CodeCivilite.Focus();
            //                    this.Txt_CodeCivilite.Text = string.Empty;
            //                    this.Txt_LibelleCivilite.Text = string.Empty;
            //                };
            //                w.Show();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Message.ShowError(ex.Message, Langue.lbl_Menu);
            //    }
            //}

            //private void Txt_CodeCivilite_LostFocus(object sender, RoutedEventArgs e)
            //{
            //    try
            //    {
            //        HandleLostFocus<CsDenomination>((TextBox)sender, this.Txt_LibelleCivilite, LstCivilite, SessionObject.Enumere.TailleCodeCivilite);
            //    }
            //    catch (Exception ex)
            //    {
            //        Message.ShowError(ex.Message, Langue.lbl_Menu);
            //    }
            //}

            private void Txt_CodePaiement_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (this.Txt_CodePaiement.Text.Length == 1
                        && LstModePaiement.Count != 0)
                    {

                        CsModepaiement LeModePaiement = ClasseMEthodeGenerique.RetourneObjectFromList(LstModePaiement, this.Txt_CodePaiement.Text, "CODE");
                        if (!string.IsNullOrEmpty(LeModePaiement.CODE))
                        {
                            this.Txt_LibelleModePaiement.Text = LeModePaiement.LIBELLE;
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

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {

            }
            public event EventHandler Closed;

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                EnregistrerDemande(LaDemande,false );
                ValidationDemande(LaDemande);
                if (Closed != null)
                    Closed(this, new EventArgs());
            }

            private void Txt_CodeCiviliteAgent_TextChanged(object sender, TextChangedEventArgs e)
            {

            }
            ObjDOCUMENTSCANNE leDoc = new ObjDOCUMENTSCANNE();

        private void ValidationDemande(CsDemande _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderDemandeWithObjectCompleted += (sr, res) =>
                {

                    string Messages = string.Empty;
                    //Si la date de d'encaissement n'est pas renseigné c-a-d que la demende est en attente
                    if (_LaDemande.LaDemande.STATUT == SessionObject.Enumere.DemandeStatusEnAttente)
                        //Msg de confirmation de l'enregistremet
                        Messages = Langue.MsgRequestSaved;
                    //Si la date d'encaissement est renseigné 
                    else
                        Messages = Langue.MsgOperationTerminee;

                    Message.ShowInformation(Messages, Langue.lbl_Menu);
                    this.DialogResult = false;
                };
                service1.ValiderDemandeWithObjectAsync(_LaDemande, leDoc);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void lnkLetter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lnkLetter.Tag == null)
                {
                    var openDialog = new OpenFileDialog();
                    //openDialog.Filter = "Text Files (*.txt)|*.txt";
                    openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                    openDialog.Multiselect = true;
                    bool? userClickedOK = openDialog.ShowDialog();
                    if (userClickedOK == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            lnkLetter.Tag = memoryStream.GetBuffer();
                            var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                            formScanne.Show();

                            this.btn_Supprime.Visibility = System.Windows.Visibility.Visible;
                            this.btn_Modifier.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }

                else
                {
                    MemoryStream memoryStream = new MemoryStream(lnkLetter.Tag as byte[]);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private ObjDOCUMENTSCANNE SaveFile(byte[] pStream, int pTypeDocument, ObjDOCUMENTSCANNE pDocumentScane)
        {
            try
            {
                //Récupération du contenu.
                if (pDocumentScane == null)
                {
                    pDocumentScane = new ObjDOCUMENTSCANNE { CONTENU = pStream, PK_ID = Guid.NewGuid(), DATECREATION = DateTime.Now, USERCREATION = UserConnecte.matricule };
                    pDocumentScane.OriginalPK_ID = pDocumentScane.PK_ID;
                    pDocumentScane.ISNEW = true;
                    if (LaDemande.LaDemande != null)
                        LaDemande.LaDemande.FICHIERJOINT = pDocumentScane.PK_ID;
                }
                else
                    pDocumentScane.CONTENU = pStream;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }

            return pDocumentScane;
        }
        private void GetInformationFromChildWindowImageAutorisation(object sender, EventArgs e)
        {
            try
            {
                var form = (UcImageScanne)sender;
                if (form != null)
                {
                    if (form.DialogResult == true /*&& form.ImageScannee != null*/)
                    {
                        this.lnkLetter.Content = Langue.msgVoirPiecejointe;
                        //this.lnkLetter.Tag = form.ImageScannee;
                        //SaveFile(form.ImageScannee, (int)SessionObject.TypeDocumentScanneDevis.Lettre);
                    }
                }
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
                    List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                    UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODESITE", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedSite);
                    ctr.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }

        }
        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                this.btn_Site.IsEnabled = true;
                CsSite leSite = (CsSite)ctrs.MyObject;
                this.Txt_CodeSite.Text = leSite.CODESITE;
                this.LstCentre = LstCentre.Where(t => t.FK_IDCODESITE == leSite.PK_ID).ToList();

            }
            else
                this.btn_Centre.IsEnabled = true;


        }
        private void lnkMotif_Click(object sender, RoutedEventArgs e)
        {
            Message.ShowInformation(LaDemande.LaDemande.ANNOTATION, "Motif réjet");

        }
            private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                    if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                    {
                        this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                        LaDemande.LaDemande.CENTRE = _LeCentre.CODE; ;
                        LaDemande.LaDemande.FK_IDCENTRE = _LeCentre.PK_ID;
                        this.Txt_NumDemande.Text = _LeCentre.CODE + _LeCentre.NUMDEM;
                        LaDemande.LaDemande.NUMDEM = this.Txt_NumDemande.Text;
                        LaDemande.LeCentre = LeCentreSelect;
                        LaDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                    }
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCentre.Focus();
                        };
                        w.Show();
                    }
                }
            }

            private void btn_Centre_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (LstCentre.Count > 0)
                    {
                        this.btn_Centre.IsEnabled = false;
                        List<object> _Listgen = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre);
                        UcListeGenerique ctr = new UcListeGenerique(_Listgen, "CODE", "LIBELLE", Langue.lbl_ListeCentre);
                        ctr.Closed += new EventHandler(galatee_OkClickedCentre);
                        ctr.Show();
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }

            }
            void galatee_OkClickedCentre(object sender, EventArgs e)
            {
                this.btn_Centre.IsEnabled = true;
                LeCentreSelect = new CsCentre();
                UcListeGenerique ctrs = sender as UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    CsCentre leCentre = (CsCentre)ctrs.MyObject;
                    LeCentreSelect = leCentre;
                    this.Txt_CodeCentre.Text = leCentre.CODE;

                    string numIncrementiel = LeCentreSelect.NUMDEM.ToString();
                    if (LeCentreSelect.NUMDEM.ToString().Length >= 10)
                        numIncrementiel = LeCentreSelect.NUMDEM.ToString().Substring(numIncrementiel.Length - 9, 9);
                    this.Txt_NumDemande.Text = LeCentreSelect.CODE + numIncrementiel.PadLeft(10, '0');
                    LaDemande.LaDemande.FK_IDCENTRE = LeCentreSelect.PK_ID;

                }
            }

            private void btn_Produit_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    List<object> _LstProduit = ClasseMEthodeGenerique.RetourneListeObjet(ListeDesProduitDuSite);
                    UcListeGenerique ctr = new UcListeGenerique(_LstProduit, "CODE", "LIBELLE", Langue.lbl_ListeProduit);
                    ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                    ctr.Show();
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Langue.lbl_Menu);
                }
            }
            void galatee_OkClickedProduit(object sender, EventArgs e)
            {
                try
                {
                    UcListeGenerique ctrs = sender as UcListeGenerique;
                    if (ctrs.isOkClick)
                    {
                        LeProduitSelect = (CsProduit)ctrs.MyObject;
                        this.Txt_CodeProduit.Text = LeProduitSelect.CODE;

                    }
                    btn_Produit.IsEnabled = true;
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }


            private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
            {
                try
                {
                    if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                    {
                        LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                        CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                        if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                        {
                            this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                            LaDemande.LeProduit = LeProduitSelect;
                            LaDemande.LaDemande.FK_IDPRODUIT = _LeProduitSelect.PK_ID;

                        }
                        else
                        {
                            var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            w.OnMessageBoxClosed += (_, result) =>
                            {
                                this.Txt_CodeProduit.Focus();
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
            private void btn_Rechercher_Click_1(object sender, RoutedEventArgs e)
            {

                VerifieExisteDemande(this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text, LaDemande.LaDemande.FK_IDCENTRE, TypeDemande);
            }
            private void VerifieExisteDemande(string centre, string client, string Ordre, int idCentre, string tdem)
            {

                try
                {
                    if (!string.IsNullOrEmpty(Txt_Client.Text) && Txt_Client.Text.Length == SessionObject.Enumere.TailleClient)
                    {

                        LaDemande.LaDemande.CLIENT = Txt_Client.Text;
                        string OrdreMax = string.Empty;
                        AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                        service.RetourneDemandeClientTypeCompleted += (s, args) =>
                        {
                            if (args != null && args.Cancelled)
                                return;
                            if (args.Result != null)
                            {
                                if (args.Result.STATUT != SessionObject.Enumere.DemandeStatusPriseEnCompte || args.Result.ISSUPPRIME == false)
                                {
                                    Message.ShowError("Il existe déja une demande de ce type sur ce client", "Accueil");
                                    return;
                                }
                            }
                            RetourneInfoClient(idCentre,this.Txt_CodeCentre.Text, this.Txt_Client.Text, this.Txt_Ordre.Text);
                        };
                        service.RetourneDemandeClientTypeAsync(centre, client, Ordre, idCentre, tdem);
                        service.CloseAsync();
                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

            }
            void RemplirLibelle()
            {
                if (this.Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsCentre _LeCentre = ClasseMEthodeGenerique.RetourneObjectFromList<CsCentre>(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                    if (_LeCentre != null && !string.IsNullOrEmpty(_LeCentre.CODE))
                        this.Txt_LibelleCentre.Text = _LeCentre.LIBELLE;
                }
                if (this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    LaDemande.LaDemande.PRODUIT = this.Txt_CodeProduit.Text;
                    CsProduit _LeProduitSelect = ClasseMEthodeGenerique.RetourneObjectFromList(ListeDesProduitDuSite, this.Txt_CodeProduit.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeProduitSelect.LIBELLE))
                    {
                        this.Txt_LibelleProduit.Text = _LeProduitSelect.LIBELLE;
                    }
                }
                if (this.Txt_CodeCategorie.Text.Length == SessionObject.Enumere.TailleCodeCategorie
                    && LstCategorie.Count != 0)
                {

                    CsCategorieClient LaCategorie = ClasseMEthodeGenerique.RetourneObjectFromList(LstCategorie, Txt_CodeCategorie.Text, "CODE");
                    if (!string.IsNullOrEmpty(LaCategorie.LIBELLE))
                        this.Txt_LibelleCategorie.Text = LaCategorie.LIBELLE;
                }
                if (this.Txt_CodeCivilite.Text.Length == SessionObject.Enumere.TailleCodeCivilite && (LstCivilite != null && LstCivilite.Count != 0))
                {
                    CsDenomination _laCivilite = LstCivilite.FirstOrDefault(c => c.CODE == this.Txt_CodeCivilite.Text);
                    if (_laCivilite != null && !string.IsNullOrEmpty(_laCivilite.LIBELLE))
                    {
                        this.Txt_LibelleCivilite.Text = _laCivilite.LIBELLE;
                    }
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
                if (this.Txt_CodeNationalite.Text.Length == SessionObject.Enumere.TailleCodeNationalite &&
                  LstDesNationalites.Count != 0)
                {
                    CsNationalite _leCodeNation = ClasseMEthodeGenerique.RetourneObjectFromList(LstDesNationalites, this.Txt_CodeNationalite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeNation.LIBELLE))
                        this.Txt_Nationnalite.Text = _leCodeNation.LIBELLE;
                }
                if (this.Txt_CodeNatureClient.Text.Length == SessionObject.Enumere.TailleCodeTypeClient &&
        LstNatureClient.Count != 0)
                {
                    CsNatureClient _leCodeNature = ClasseMEthodeGenerique.RetourneObjectFromList(LstNatureClient, this.Txt_CodeNatureClient.Text, "CODE");
                    if (!string.IsNullOrEmpty(_leCodeNature.LIBELLE))
                        this.Txt_LibelleNatureClient.Text = _leCodeNature.LIBELLE;
                }
            }

            private void ChildWindow_Loaded_1(object sender, RoutedEventArgs e)
            {
                InitialiseCtrl();
            }

            private void Txt_Ordre_LostFocus(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrEmpty(this.Txt_Ordre.Text))
                    this.Txt_Ordre.Text = this.Txt_Ordre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
            }
            private void Txt_CodeCentre_LostFocus(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                    this.Txt_CodeCentre.Text = this.Txt_CodeCentre.Text.PadLeft(SessionObject.Enumere.TailleOrdre, '0');
            }
            private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                    this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
            }


            private void btn_Supprimer_click(object sender, RoutedEventArgs e)
            {
                if (lnkLetter.Tag != null)
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.msgSuppressionFichier, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                        {
                            if (LaDemande != null && LaDemande.LaDemande != null && LaDemande.LaDemande.FICHIERJOINT != new Guid("00000000-0000-0000-0000-000000000000"))
                                LaDemande.LaDemande.FICHIERJOINT = new Guid("00000000-0000-0000-0000-000000000000");

                            this.btn_Supprime.Visibility = System.Windows.Visibility.Collapsed;
                            this.btn_Modifier.Visibility = System.Windows.Visibility.Collapsed;
                            lnkLetter.Tag = null;
                            this.lnkLetter.Content = "Motif de la modification";
                        }
                    };
                    w.Show();
                }
                else
                    Message.ShowInformation(Langue.msgAucunfichier, Langue.lbl_Menu);

            }

            private void btn_Modifier_click(object sender, RoutedEventArgs e)
            {
                try
                {
                    var openDialog = new OpenFileDialog();
                    //openDialog.Filter = "Text Files (*.txt)|*.txt";
                    openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                    openDialog.Multiselect = true;
                    bool? userClickedOK = openDialog.ShowDialog();
                    if (userClickedOK == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            lnkLetter.Tag = memoryStream.GetBuffer();
                            var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            //var formScanne = new UcImageScanne(stream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                            formScanne.Show();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                }
            }

           

            private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
            {
                if (this.Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsSite _LeSite = ClasseMEthodeGenerique.RetourneObjectFromList<CsSite>(lstSite, this.Txt_CodeSite.Text, "CODESITE");
                    if (_LeSite != null && !string.IsNullOrEmpty(_LeSite.CODESITE))
                        this.Txt_LibelleSite.Text = _LeSite.LIBELLE;
                    else
                    {
                        var w = new MessageBoxControl.MessageBoxChildWindow(Langue.lbl_Menu, Langue.MsgEltInexistent, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        w.OnMessageBoxClosed += (_, result) =>
                        {
                            this.Txt_CodeCentre.Focus();
                        };
                        w.Show();
                    }
                }


            }
                 
    }
}

