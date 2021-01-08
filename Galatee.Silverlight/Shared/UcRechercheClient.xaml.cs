using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;

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

namespace Galatee.Silverlight.Shared
{
    public partial class UcRechercheClient : ChildWindow
    {
        public event EventHandler Closed;
        public object MyObject { get; set; }

        public UcRechercheClient()
        {
            InitializeComponent();
            try
            {
                InitializeComponent();
                translate();
                this.Txt_Client.MaxLength = SessionObject.Enumere.TailleClient;
                this.Txt_CodeCentre.MaxLength = SessionObject.Enumere.TailleCentre;
                this.Txt_CodeProduit.MaxLength = SessionObject.Enumere.TailleCodeProduit;
                if (Rdb_Client != null)
                    Rdb_Client.IsChecked = true;
                if (Rdb_AdresseElectrique  != null)
                    Rdb_AdresseElectrique.IsChecked = false ;
                if (Rdb_StreetAdresse  != null)
                    Rdb_StreetAdresse.IsChecked = false;
                if (Rdb_Zone  != null)
                    Rdb_Zone.IsChecked = false;
                if (Rdb_Meter  != null)
                    Rdb_Meter.IsChecked = false;
                //this.tab12_Txt_LibelleModePaiement.Visibility = System.Windows.Visibility.Collapsed;

                //this.Txt_CodeSite.Visibility = Visibility.Collapsed  ;
                //this.Txt_LibelleSite .Visibility = Visibility.Collapsed ;
                //this.lbl_site.Visibility = Visibility.Collapsed;
                //this.btn_Site.Visibility = Visibility.Collapsed;
                choixOptionRecherche();
                ChargeTypeDemande(null);
                ChargerLaListeDesCommunes();
                ChargeQuartier();
                ChargeSecteur();
                ChargerTournee();



                ChargerPuissance();
                ChargerForfait();
                ChargerTarif();
                ChargerFrequence();
                ChargerMois();
                ChargerApplicationTaxe();
                ChargerLaListeDesCommunes();
                ChargeQuartier();
                ChargeSecteur();
                ChargeRue();
                ChargerTournee();
                ChargerDonneeDuSite();
                ChargerListeDeProduit();
                ChargerDiametre();
                ChargerMaterielBranchement();
                ChargerCategorie();
                ChargerFermable();
                ChargerNatureClient();
                ChargerCivilite();
                ChargerNationnalite();
                ChargerCodeRegroupement();
                ChargerCodeConsomateur();

                ChargerDiametreCompteur();
                ChargerCadran();
                ChargerMarque();
                ChargerTypeCompteur();

              
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> LstDeProduit = new List<CsProduit>();
        List<CsEvenement> lstEvenement;
        Galatee.Silverlight.ServiceAccueil.CsClientRechercher _LeClientSelect;
        CsClasseurClient _LeClasseur;
        CsCompteClient leClasseurClient;
        List<CsTarif> LstTarif;
        List<CsForfait> LstForfait;
        List<CsPuissance> LstPuissance;
        List<CsReglageCompteur > LstDiametre;
        List<CsTypeBranchement> LstDiametreBrt;
        List<CsMois> LstMois;
        List<CsCodeTaxeApplication> LstCodeApplicationTaxe;
        List<CsNationalite> LstDesNationalites;

        List<CsCommune> LstCommuneAll = new List<CsCommune>();
        List<CsCommune> LstCommuneSelect = new List<CsCommune>();
        List<CsQuartier> LstQuartierAll = new List<CsQuartier>();
        List<CsSecteur> LstSecteurAll =new List<CsSecteur>();
        List<CsRues > LstRueAll = new List<CsRues>();
        List<CsTcompteur> LstTypeCompteur = new List<CsTcompteur>();
        List<CsMarqueCompteur> LstMarque = new List<CsMarqueCompteur>();
        List<CsCadran> LstCadran = new List<CsCadran>();
        List<CsMaterielBranchement> LstDeMaterielBrt = new List<CsMaterielBranchement>();
        List<CsRegCli> LstCodeRegroupement = new List<CsRegCli>();



        List<CsFrequence> LstFrequence = new List<CsFrequence>();
        List<CsFermable> LstFermable = new List<CsFermable>();
        List<CsDenomination> LstCivilite = new List<CsDenomination>();
        List<CsCodeConsomateur> LstCodeConsomateur = new List<CsCodeConsomateur>();
        List<CsCategorieClient> LstCategorie = new List<CsCategorieClient>();
        List<CsNatureClient> LstNatureClient = new List<CsNatureClient>();
        List<CsRegCli> LstRegCli = new List<CsRegCli>();
        List<CParametre> LstModePayment = new List<CParametre>();


        List<CsLclient > LstFactureClient;
        List<CsLclient > LstReglementClient;
        List<CsDemandeBase> ListDemande;
        void translate()
        {
            this.label5.Content = Langue.lbl_name;
        }
        private void initControl()
        {
            try
            {
                List<string> ListOperation = new List<string>();
                ListOperation = SessionObject.TypeOperationClasseur().ToList();
            }
            catch (Exception ex)
            {

                Message.ShowError(ex.Message, Langue.lbl_Menu + "=>initControl");
            }
        }
        private void ChargerPuissance()
        {
            try
            {
                if (SessionObject.LstPuissance.Count != 0)
                    LstPuissance = SessionObject.LstPuissance;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerPuissanceCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstPuissance = args.Result;
                    };
                    service.ChargerPuissanceAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerForfait()
        {
            try
            {
                if (SessionObject.LstForfait.Count != 0)
                    LstForfait = SessionObject.LstForfait;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerForfaitCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;

                        SessionObject.LstForfait = args.Result;
                        LstForfait = SessionObject.LstForfait;

                    };
                    service.ChargerForfaitAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerTarif()
        {
            try
            {
                if (SessionObject.LstTarif.Count != 0)
                    LstTarif = SessionObject.LstTarif;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTarifCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstTarif = args.Result;
                        LstTarif = SessionObject.LstTarif;
                    };
                    service.ChargerTarifAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerFrequence()
        {
            if (SessionObject.LstFrequence.Count != 0)
                LstFrequence = SessionObject.LstFrequence;
            else
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTousFrequenceCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFrequence = args.Result;
                    LstFrequence = SessionObject.LstFrequence;

                };
                service.ChargerTousFrequenceAsync();
                service.CloseAsync();
            }
        }
        private void ChargerMois()
        {
            try
            {
                if (SessionObject.LstMois.Count != 0)
                    LstMois = SessionObject.LstMois;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerTousMoisCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstMois = args.Result;
                        LstMois = SessionObject.LstMois;

                    };
                    service.ChargerTousMoisAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerApplicationTaxe()
        {
            try
            {
                if (SessionObject.LstCodeApplicationTaxe.Count != 0)
                    LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.RetourneTousApplicationTaxeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstCodeApplicationTaxe = args.Result;
                        LstCodeApplicationTaxe = SessionObject.LstCodeApplicationTaxe;

                    };
                    service.RetourneTousApplicationTaxeAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerLaListeDesCommunes()
        {
            try
            {
                if (SessionObject.LstCommune.Count  != 0)
                    LstCommuneAll = SessionObject.LstCommune;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerCommuneCompleted  += (s, args) =>
                    {
                        if (args.Error != null && args.Cancelled)
                            return;
                        SessionObject.LstCommune = args.Result;
                    };
                    service.ChargerCommuneAsync ();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerLaListeDesCommunes");
            }

        }
        private void ChargeQuartier()
        {
            try
            {
                if (SessionObject.LstQuartier.Count != 0)
                    LstQuartierAll = SessionObject.LstQuartier;
                else
                {
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ChargerLesQartiersCompleted  += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstQuartier = args.Result;
                    };
                    service.ChargerLesQartiersAsync ();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargeQuartier");
            }

        }
        private void ChargeSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur.Count != 0)
                {
                    LstSecteurAll = SessionObject.LstSecteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesSecteursCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstSecteur = args.Result;
                };
                service.ChargerLesSecteursAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargeSecteur");

            }
        }
        private void ChargeRue()
        {
            try
            {
                if (SessionObject.LstRues .Count != 0)
                {
                    LstRueAll  = SessionObject.LstRues ;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargeRue");

            }
        }
        private void ChargerTournee()
        {
            try
            {
                if (SessionObject.LstRues.Count != 0)
                {
                    LstRueAll = SessionObject.LstRues;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerLesTourneesCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstZone = args.Result;
                };
                service.ChargerLesTourneesAsync ();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                 Message.ShowError(ex.Message, "ChargerTournee");

            }
        }
        List<CsSite> lstSite = new List<CsSite>();

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
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    LstCentre = SessionObject.LstCentre;
                    lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    if (lstSite != null)
                    {
                        List<CsSite> _LstSite = lstSite.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList();
                        if (_LstSite.Count == 1)
                        {
                            this.Txt_CodeSite.Text = _LstSite[0].CODE;
                            this.Txt_LibelleSite.Text = _LstSite[0].LIBELLE;
                            this.btn_Site.IsEnabled = false;
                            this.Txt_CodeSite.IsReadOnly = true;
                        }
                    }
                };
                service.ListeDesDonneesDesSiteAsync(false );
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }

        private void ChargerListeDeProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit .Count != 0)
                {
                    LstDeProduit = SessionObject.ListeDesProduit;
                    return;
                }
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
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

        private void ChargerDiametre()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur .Count != 0)
                {
                    LstDiametre = SessionObject.LstDiametreCompteur;
                    return;
                }
                //AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                //service.ChargerDiamentreBranchementCompleted += (s, args) =>
                //{
                //    if ((args != null && args.Cancelled) || (args.Error != null))
                //        return;
                //    SessionObject.LstDiametreBrt = args.Result;
                //};
                //service.ChargerDiamentreBranchementAsync();
                //service.CloseAsync();
            }
            catch (Exception es)
            {

                Message.ShowError(es.Message, "ChargerDiametre");

            }

        }
        private void ChargerMaterielBranchement()
        {
            try
            {
                //if (SessionObject.LstDiametreBrt .Count != 0)
                //{
                //    LstDiametreBrt  = SessionObject.LstDiametreBrt ;
                //    return;
                //}
                //AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                //service.RetourneMaterielBranchementCompleted += (s, args) =>
                //{
                //    if ((args != null && args.Cancelled) || (args.Error != null))
                //        return;
                //    SessionObject.LstDeMaterielBrt = args.Result;
                //    LstDiametreBrt = SessionObject.LstDiametreBrt;

                //};
                //service.RetourneMaterielBranchementAsync();
                //service.CloseAsync();
            }
            catch (Exception es)
            {
                Message.ShowError(es.Message, "ChargerMaterielBranchement");

            }

        }

        private void ChargerCategorie()
        {
            try
            {
                if (SessionObject.LstCategorie.Count != 0)
                {
                    LstCategorie = SessionObject.LstCategorie;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCategorieCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCategorie = args.Result;
                    LstCategorie = SessionObject.LstCategorie;

                };
                service.RetourneCategorieAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerCategorie");
            }
        }
        private void ChargerFermable()
        {
            try
            {
                if (SessionObject.LstFermable.Count != 0)
                {
                    LstFermable = SessionObject.LstFermable;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneFermableCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstFermable = args.Result;
                    LstFermable = SessionObject.LstFermable;

                };
                service.RetourneFermableAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerFermable");

            }
        }
        private void ChargerNatureClient()
        {
            try
            {
                if (SessionObject.LstNatureClient .Count != 0)
                {
                    LstNatureClient = SessionObject.LstNatureClient;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneNatureCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstNatureClient = args.Result;
                    LstNatureClient = SessionObject.LstNatureClient;
                };
                service.RetourneNatureAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerNatureClient");
            }
        }
        private void ChargerCivilite()
        {
            try
            {
                if (SessionObject.LstCivilite.Count != 0)
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
                Message.ShowError(ex.Message, "ChargerCivilite");

            }
        }
        private void ChargerNationnalite()
        {
            try
            {
                if (SessionObject.LstDesNationalites.Count != 0)
                {
                    LstDesNationalites = SessionObject.LstDesNationalites;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneNationnaliteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDesNationalites = args.Result;
                    LstDesNationalites = SessionObject.LstDesNationalites;

                };
                service.RetourneNationnaliteAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerNationnalite");

            }
        }
        private void ChargerCodeRegroupement()
        {
            try
            {
                if (SessionObject.LstCodeRegroupement.Count != 0)
                {
                    LstCodeRegroupement = SessionObject.LstCodeRegroupement;
                    return;
                }
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
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerCodeRegroupement");

            }

        }
        private void ChargerCodeConsomateur()
        {
            try
            {
                if (SessionObject.LstCodeConsomateur.Count != 0)
                {
                    LstCodeConsomateur = SessionObject.LstCodeConsomateur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneCodeConsomateurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCodeConsomateur = args.Result;
                    LstCodeConsomateur = SessionObject.LstCodeConsomateur;

                };
                service.RetourneCodeConsomateurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerCodeConsomateur");
            }
        }

        private void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstDiametreCompteur.Count != 0)
                {
                    LstDiametre  = SessionObject.LstDiametreCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerDiametreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstDiametreCompteur = args.Result;
                    LstDiametre = SessionObject.LstDiametreCompteur;

                };
                service.ChargerDiametreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDiametreCompteur");
            }
        }
        private void ChargerCadran()
        {
            try
            {
                if (SessionObject.LstCadran .Count != 0)
                {
                    LstCadran = SessionObject.LstCadran;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutCadranCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCadran = args.Result;
                    LstCadran = SessionObject.LstCadran;

                };
                service.RetourneToutCadranAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerCadran");
            }
        }
        private void ChargerMarque()
        {
            try
            {
                if (SessionObject.LstMarque.Count  != 0)
                {
                    LstMarque = SessionObject.LstMarque;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RetourneToutMarqueCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstMarque = args.Result;
                    LstMarque = SessionObject.LstMarque;

                };
                service.RetourneToutMarqueAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerMarque");
            }
        }
        private void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur.Count != 0)
                {
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                    LstTypeCompteur = SessionObject.LstTypeCompteur;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerTypeCompteur");

            }
        }
     

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void choixOptionRecherche()
        {
            try
            {
                if (Rdb_Client!= null && Rdb_Client.IsChecked == true)
                {
                    this.Txt_Client.IsEnabled = true;
                    this.Txt_Client.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                    this.Txt_CodeProduit.IsEnabled = true;

                    this.Txt_Ordre.IsEnabled = true;
                    this.Txt_Ordre.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                    this.Txt_NomClient.IsEnabled = true;
                    this.Txt_NomClient.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                    this.Txt_Zone.IsEnabled = false;
                    this.Txt_Zone.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Sequence.IsEnabled = false;
                    this.Txt_Sequence.Text = string.Empty;
                    this.Txt_Sequence.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Compteur.IsEnabled = false;
                    this.Txt_Compteur.Text = string.Empty;
                    this.Txt_Compteur.Background = new SolidColorBrush(Colors.Transparent);

                }
                else if (Rdb_Zone!= null && Rdb_Zone.IsChecked == true)
                {
                    this.Txt_Client.IsEnabled = false;
                    this.Txt_Client.Text = string.Empty;
                    this.Txt_Client.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_CodeProduit.IsEnabled = false;
                    this.Txt_CodeProduit.Text = string.Empty;
                    this.Txt_CodeProduit.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Ordre.IsEnabled = false;
                    this.Txt_Ordre.Text = string.Empty;
                    this.Txt_Ordre.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_NomClient.IsEnabled = false;
                    this.Txt_NomClient.Text = string.Empty;
                    this.Txt_NomClient.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Compteur.IsEnabled = false;
                    this.Txt_Compteur.Text = string.Empty;
                    this.btn_Produit.Background = new SolidColorBrush(Colors.Transparent);


                    this.btn_Produit.IsEnabled = false;
                    this.btn_Produit.Background = new SolidColorBrush(Colors.Transparent);


                    this.Txt_Zone.IsEnabled = true;
                    this.Txt_Zone.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));
                    

                }
                else if (this.Rdb_Meter!= null && this.Rdb_Meter.IsChecked == true)
                {
                    this.Txt_Client.IsEnabled = false;
                    this.Txt_Client.Text = string.Empty;
                    this.Txt_Client.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_CodeProduit.IsEnabled = false;
                    this.Txt_CodeProduit.Text = string.Empty;
                    this.Txt_CodeProduit.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Ordre.IsEnabled = false;
                    this.Txt_Ordre.Text = string.Empty;
                    this.Txt_Ordre.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_NomClient.IsEnabled = false;
                    this.Txt_NomClient.Text = string.Empty;
                    this.Txt_NomClient.Background = new SolidColorBrush(Colors.Transparent);


                    this.btn_Produit.IsEnabled = false;


                    this.Txt_Compteur.IsEnabled = true;
                    this.Txt_Compteur.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                    this.btn_Produit.IsEnabled = false;
                    this.Txt_Zone.IsEnabled = false;
                    this.Txt_Zone.Text  = string.Empty ;
                    this.Txt_Zone.Background = new SolidColorBrush(Colors.Transparent);


                }
                else if (this.Rdb_StreetAdresse != null && this.Rdb_StreetAdresse.IsChecked == true)
                {
                    this.Txt_Client.IsEnabled = false;
                    this.Txt_Client.Text = string.Empty;
                    this.Txt_Client.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_CodeProduit.IsEnabled = false;
                    this.Txt_CodeProduit.Text = string.Empty;
                    this.Txt_CodeProduit.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Ordre.IsEnabled = false;
                    this.Txt_Ordre.Text = string.Empty;
                    this.Txt_Ordre.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_NomClient.IsEnabled = false;
                    this.Txt_NomClient.Text = string.Empty;
                    this.Txt_NomClient.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Zone.IsEnabled = false;
                    this.Txt_Zone.Text = string.Empty;
                    this.Txt_Zone.Background = new SolidColorBrush(Colors.Transparent);

                    this.Txt_Compteur  .IsEnabled = false;
                    this.Txt_Compteur.Text = string.Empty;
                    this.Txt_Compteur.Background = new SolidColorBrush(Colors.Transparent);

                    this.btn_Produit.IsEnabled = false;


                    this.Txt_Sequence.IsEnabled = true;
                    this.Txt_Sequence.Background = new SolidColorBrush(Color.FromArgb(100, 173, 216, 230));

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        private List<CsLclient> FormateListe(List<CsLclient> _LeCompteClient)
        {
            List<CsLclient> _LstFactureFinal = new List<CsLclient>();
            if (_LeCompteClient != null && ((_LeCompteClient != null && _LeCompteClient.Count != 0)))
            {
                List<CsLclient> _LstFacture = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Debit && p.TOP1 != SessionObject.Enumere.TopCaisse).ToList();
                List<CsLclient> _LstEncaissement = _LeCompteClient.Where(p => p.DC == SessionObject.Enumere.Credit ).ToList();
                foreach (var item in _LstFacture)
                {
                    _LstFactureFinal.Add(item);
                    _LstFactureFinal.AddRange(TransLClient(_LstEncaissement.Where(p => p.CENTRE == item.CENTRE && p.CLIENT == item.CLIENT && p.ORDRE == item.ORDRE
                                                                     && p.REFEM  == item.REFEM && p.NDOC == item.NDOC ).ToList()));
                }
  
            }
            return _LstFactureFinal;
        }
        private List<CsLclient> TransLClient(List<CsLclient> _LeTranscaisse)
        {
            List<CsLclient> _LeReglt = new List<CsLclient>();
            foreach (var item in _LeTranscaisse )
            {
                item.REFEM = string.Empty ;
                item.NDOC = string.Empty ;
                item.ACQUIT = string.Empty ;
                _LeReglt.Add(item);
            }
            return _LeReglt;
        }

      
       
      
       
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            initControl();
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        void galatee_OkClicked(object sender, EventArgs e)
        {
            //UcListeDesCentres ctrs = sender as UcListeDesCentres;
            //this.Txt_CodeCentre.Text = ctrs.MySite.CODE;
            ////this.Txt_LibelleCentre.Text = ctrs.MySite.LIBELLE;
            //LstDeProduit = ClasseMEthodeGenerique.RetourneListeDeProduitDuSite(ctrs.MySite);
            //if (LstDeProduit.Count == 1)
            //{
            //    this.Txt_CodeProduit.Text = LstDeProduit[0].CODEPRODUIT ;
            //    //this.Txt_LibelleProduit.Text = LstDeProduit[0].LIBELLE;
            //    this.btn_Produit.IsEnabled = false;

            //}

            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsCentre _LaCateg = (CsCentre)ctrs.MyObject;
                this.Txt_CodeCentre.Text = _LaCateg.CODE;
            }
        }

        private void btn_Produit_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (LstDeProduit != null && LstDeProduit.Count != 0)
                {
                    List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(LstDeProduit);
                    UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODEPRODUIT", "LIBELLE", "Liste");
                    ctr.Closed += new EventHandler(galatee_OkClickedProduit);
                    ctr.Show();
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }
        void galatee_OkClickedProduit(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            if (ctrs.GetisOkClick)
            {
                CsProduit _Leproduit = (CsProduit)ctrs.MyObject;
                this.Txt_CodeProduit.Text = _Leproduit.CODE ;
                this.Txt_LibelleProduit.Text = _Leproduit.LIBELLE;
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!string.IsNullOrEmpty(this.Txt_CodeCentre.Text))
                {
                    Galatee.Silverlight.ServiceAccueil.CsClientRechercher _LeClient = new Galatee.Silverlight.ServiceAccueil.CsClientRechercher();
                    _LeClient.CENTRE = string.IsNullOrEmpty(this.Txt_CodeCentre.Text) ? null : this.Txt_CodeCentre.Text;
                    _LeClient.CLIENT = string.IsNullOrEmpty(this.Txt_Client.Text) ? null : this.Txt_Client.Text;
                    _LeClient.ORDRE = string.IsNullOrEmpty(this.Txt_Ordre.Text) ? null : this.Txt_Ordre.Text;
                    _LeClient.NOMABON = string.IsNullOrEmpty(this.Txt_NomClient.Text) ? null : this.Txt_NomClient.Text;
                    _LeClient.TOURNEE = string.IsNullOrEmpty(this.Txt_Zone.Text) ? null : this.Txt_Zone.Text;
                    _LeClient.SEQUENCE = string.IsNullOrEmpty(this.Txt_Sequence.Text) ? null : this.Txt_Sequence.Text;
                    _LeClient.NUMCOMPTEUR = string.IsNullOrEmpty(this.Txt_Compteur.Text) ? null : this.Txt_Compteur.Text;
                    _LeClient.PRODUIT = string.IsNullOrEmpty(this.Txt_CodeProduit.Text) ? null : this.Txt_CodeProduit.Text;
                    _LeClient.FK_IDCENTRE = IdCentreClient; 
                    if (this.Rdb_Client.IsChecked == true)
                        _LeClient.OptionRecherche = 1;
                    if (this.Rdb_Zone .IsChecked == true)
                        _LeClient.OptionRecherche = 2;
                    if (this.Rdb_Meter .IsChecked == true)
                        _LeClient.OptionRecherche = 3;
                    if (this.Rdb_StreetAdresse .IsChecked == true)
                        _LeClient.OptionRecherche = 4;

                    RechercheClient(_LeClient);
                }
                else
                    Message.ShowError("Selectionner le Centre du client", Langue.lbl_Menu);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        //private void RetourneClasseurClient(CsClientRechercher _LeClientRechercher)
        //{
        //    int res = LoadingManager.BeginLoading(Langue.En_Cours);
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation (), Utility.EndPoint("Accueil"));
        //    service.RetourneClasseurClientCompleted += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        this.tab3_txt_Centre.Text = _LeClientRechercher.CENTRE ;
        //        this.tab3_txt_Client.Text = _LeClientRechercher.CLIENT ;
        //        this.tab3_txt_Ordre.Text = _LeClientRechercher.ORDRE ;

        //        this.tab1_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab1_txt_Client.Text = _LeClientRechercher.CLIENT;
        //        this.tab_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        this.tab2_txt_centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab2_txt_client.Text = _LeClientRechercher.CLIENT;
        //        this.tab2_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        this.tab6_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab6_txt_Client.Text = _LeClientRechercher.CLIENT;

        //        this.tab5_Txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab5_txt_Client.Text = _LeClientRechercher.CLIENT;



        //        this.tab4_txt_Centre.Text = _LeClientRechercher.CENTRE;
        //        this.tab4_txt_Client .Text = _LeClientRechercher.CLIENT;
        //        this.tab4_txt_Ordre.Text = _LeClientRechercher.ORDRE;

        //        _LeClasseur = new CsClasseurClient();
        //        _LeClasseur = args.Result;
        //        if (_LeClasseur != null)
        //        {
        //            if (_LeClasseur.Ag != null )
        //                RemplireOngletAdresse(_LeClasseur.Ag);
        //            else
        //                RemplireOngletAdresse(new CsAg ());

        //            if (_LeClasseur.LeClient != null)
        //                RemplireOngletClient(_LeClasseur.LeClient);
        //            else
        //                RemplireOngletClient(new CsClient());

        //            if (_LeClasseur.LstAbonnement != null && _LeClasseur.LstAbonnement.Count != 0)
        //                RemplireOngletAbon(_LeClasseur.LstAbonnement);
        //            else
        //                RemplireOngletAbon(new List<CsAbon>());

        //            if (_LeClasseur.LstBranchement != null && _LeClasseur.LstBranchement.Count != 0)
        //                RemplireOngletbranchement(_LeClasseur.LstBranchement);
        //            else
        //                RemplireOngletbranchement(new List<CsBrt>());

        //            if (_LeClasseur.LstCanalistion != null && (_LeClasseur.LstCanalistion.Count != 0))                                           
        //                RemplireOngletCanalisation(_LeClasseur.LstCanalistion);
        //            else
        //                RemplireOngletCanalisation(new List<CsCanalisation>());
                        


        //            if (_LeClasseur.LeCompteClient  != null)
        //            {
        //                decimal _totalDebit = 0;
        //                decimal _totalCredit = 0;

        //                _totalDebit = decimal.Parse(_LeClasseur.LeCompteClient.ToutLClient.Where(t=>t.DC == SessionObject.Enumere.Debit).Sum(p => p.MONTANT).ToString());
        //                //this.tab4_txt_TotalDebit.Text = Math.Abs(_totalDebit).ToString("N2");
        //                this.tab4_txt_TotalDebit.Text = ThousandsSeparator.formate(_totalDebit);

        //                _totalCredit = _LeClasseur.LeCompteClient.ToutLClient.Where(t => t.DC == SessionObject.Enumere.Credit).Sum(p => decimal.Parse(p.MONTANT.ToString()));
        //                //this.tab4_txt_TotalCredit.Text = Math.Abs(_totalCredit).ToString("N2");
        //                this.tab4_txt_TotalCredit.Text = ThousandsSeparator.formate(_totalCredit);
        //                 tab4_txt_balance.Text =ThousandsSeparator.formate( (_totalDebit - _totalCredit));

        //                 LstReglementClient = new List<CsTranscaisse >();
        //                 LstFactureClient = new List<CsLclient >();
        //                 if (_LeClasseur.LeCompteClient.LstFacture != null)
        //                     LstFactureClient = _LeClasseur.LeCompteClient.LstFacture ;
        //                 if (_LeClasseur.LeCompteClient.LstReglement  != null)
        //                     LstReglementClient  = _LeClasseur.LeCompteClient.LstReglement;
        //                 RemplirTypeAction(0);
        //            }
        //        }
        //        LoadingManager.EndLoading(res);
        //    };
        //    service.RetourneClasseurClientAsync(_LeClientRechercher);
        //    service.CloseAsync();
        //}

           
        List<CsTdem> LstTypeDemande = new List<CsTdem>();
        private void ChargeTypeDemande(string typeDemande)
        {
            try
            {

                if (SessionObject.LstTypeDemande != null && SessionObject.LstTypeDemande.Count != 0)
                    LstTypeDemande = SessionObject.LstTypeDemande;
                else
                {
                    AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service1.RetourneOptionDemandeCompleted += (sr, res) =>
                    {
                        if (res != null && res.Cancelled)
                            return;
                        LstTypeDemande = res.Result;
                        SessionObject.LstTypeDemande = LstTypeDemande;
                    };
                    service1.RetourneOptionDemandeAsync(typeDemande);
                    service1.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }
        private string RetourneLibelleStatutDemande(CsDemandeBase _laDemande)
        {
            string Libelle = string.Empty;
            if (_laDemande.STATUT == "1")
                Libelle = Langue.lib_Statut_EnAttente;

            else if (_laDemande.STATUT == "11")
                Libelle = Langue.lib_Statut_EnCaisse;

            else if (_laDemande.STATUT == "2")
                Libelle = Langue.lib_Statut_DejaEncCaisse;

            else if (_laDemande.STATUT == "3")
                Libelle = Langue.lib_Statut_terminer;

            return Libelle;

        }
       
       
        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
         
       
        private void btn_ClearField_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Txt_Client.Text = string.Empty;
                this.Txt_Ordre.Text = string.Empty;
                this.Txt_NomClient.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }
        int IdCentreClient = 0;
        private void Txt_CodeCentre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                LstCommuneSelect = new List<CsCommune>();
                if (!string.IsNullOrEmpty(Txt_CodeCentre.Text) && Txt_CodeCentre.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsCentre _LeCentreClient = ClasseMEthodeGenerique.RetourneObjectFromList(LstCentre, this.Txt_CodeCentre.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeCentreClient.LIBELLE))
                    {
                        this.Txt_LibelleCentre.Text = _LeCentreClient.LIBELLE;
                        IdCentreClient = _LeCentreClient.PK_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }
        private void Txt_CodeProduit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeProduit.Text) && this.Txt_CodeProduit.Text.Length == SessionObject.Enumere.TailleCodeProduit)
                {
                    CsProduit _LeProduit = ClasseMEthodeGenerique.RetourneObjectFromList(LstDeProduit, this.Txt_CodeProduit.Text, LstDeProduit[0].CODE );
                    if (!string.IsNullOrEmpty(_LeProduit.CODE))
                    
                        this.Txt_LibelleProduit.Text = _LeProduit.LIBELLE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);
            }
        }

        private void Txt_Client_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Txt_Client.Text))
                this.Txt_Client.Text = this.Txt_Client.Text.PadLeft(SessionObject.Enumere.TailleClient, '0');
        }

        private void Rdb_Zone_Checked(object sender, RoutedEventArgs e)
        {
            choixOptionRecherche();
        }
        private void Rdb_Client_Checked(object sender, RoutedEventArgs e)
        {
            choixOptionRecherche();
        }
        private void Rdb_Meter_Checked(object sender, RoutedEventArgs e)
        {
            choixOptionRecherche();
        }

        private void Rdb_StreetAdresse_Checked(object sender, RoutedEventArgs e)
        {
            choixOptionRecherche();
        }

        private void RechercheClient(Galatee.Silverlight.ServiceAccueil.CsClientRechercher _LeClient)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.RechercherClientCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;

                    List<Galatee.Silverlight.ServiceAccueil.CsClientRechercher> _LstClient = new List<Galatee.Silverlight.ServiceAccueil.CsClientRechercher>();
                    _LstClient = args.Result.OrderBy(t=>t.CENTRE).OrderBy(u=>u.CLIENT ).OrderBy(h=>h.ORDRE ).ToList();
                    if (_LstClient != null && _LstClient.Count != 0)
                    {
                        LoadingManager.EndLoading(res);
                        dataGrid1.ItemsSource = null;
                        System.Windows.Data.PagedCollectionView view = new System.Windows.Data.PagedCollectionView(_LstClient);
                        dataGrid1.ItemsSource = view;
                        datapager.Source = view;
                        dataGrid1.SelectedItem = _LstClient[0];
                    }
                    else
                    {
                     
                        Message.Show(Langue.MsgEltInexistent, Langue.lbl_Menu);
                        LoadingManager.EndLoading(res);
                    }
                };
                service.RechercherClientAsync(_LeClient);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LoadingManager.EndLoading(res);
                string error = ex.Message;
            }

        }

        private void tab4_dataGrid3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            try
            {
                List<CsSite> _LstSite = new List<CsSite>();
                foreach (var item in LstCentre.Select(s => new { s.CODESITE, s.LIBELLESITE }).Distinct().OrderBy(p => p.CODESITE).ToList())
                {
                    _LstSite.Add(new CsSite { CODE = item.CODESITE, LIBELLE = item.LIBELLESITE });
                }
                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(_LstSite);
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkClicked_site);
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void galatee_OkClicked_site(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsSite _LaCateg = (CsSite)ctrs.MyObject;
            this.Txt_CodeCentre.Text = _LaCateg.CODE;
            //this.Txt_LibelleSite.Text = _LaCateg.LIBELLE;
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<object> _LstObject = ClasseMEthodeGenerique.RetourneListeObjet(lstSite.OrderBy(p => p.CODE).ToList());
                UcListeGenerique ctr = new UcListeGenerique(_LstObject, "CODE", "LIBELLE", "Liste");
                ctr.Closed += new EventHandler(galatee_OkSiteClicked);
                ctr.Show();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void galatee_OkSiteClicked(object sender, EventArgs e)
        {
            UcListeGenerique ctrs = sender as UcListeGenerique;
            CsSite _LeSite = (CsSite)ctrs.MyObject;
            this.Txt_CodeSite.Text = _LeSite.CODE;
        }

        private void Txt_CodeSite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Txt_CodeSite.Text) && Txt_CodeSite.Text.Length == SessionObject.Enumere.TailleCentre)
                {
                    CsSite _LeSiteClient = ClasseMEthodeGenerique.RetourneObjectFromList(lstSite, this.Txt_CodeSite.Text, "CODE");
                    if (!string.IsNullOrEmpty(_LeSiteClient.LIBELLE))                    
                        this.Txt_LibelleSite.Text = _LeSiteClient.LIBELLE;



                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.lbl_Menu);

            }
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid1.SelectedItem!=null)
            {
                getCloseAfterSelection();
            }
        }
        //CsClient leclientselectionne = new CsClient();
        private void    getCloseAfterSelection()
        {
            try
            {
                if (Closed != null)
                {
                    var selectedItem=(CsClientRechercher)dataGrid1.SelectedItem;
                    leclientselectionne = new CsClient { CENTRE = selectedItem.CENTRE, REFCLIENT = selectedItem.CLIENT, ORDRE = selectedItem.ORDRE, NOMABON = selectedItem.NOMABON, FK_IDCENTRE = selectedItem.FK_IDCENTRE };
                   Closed(this, new EventArgs());
                   this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       


        //void EditerCompteClient()
        //{

        //    string key = Utility.getKey();
        //    BranchementClientRecherche = new List<CsBrt>();
        //    AcceuilServiceClient service = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint(this));
        //    service.EditionCompteClientCompleted  += (s, args) =>
        //    {
        //        if (args != null && args.Cancelled)
        //            return;
        //        //Utility.ActionImpressionDirect(this.Txt_Imprimante.Text, key, NomRapport, "Index");
        //    };
        //    service.EditionCompteClientAsync(ToutLeCompteClientRecherche, key);
        //    service.CloseAsync();



        //}

        public CsClient leclientselectionne { get; set; }
    }
}
