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
using Galatee.Silverlight.Resources.Fraude;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceFraude;
using System.Windows.Data;
using System.Globalization;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcConsultation : ChildWindow
    {
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        private string Tdem = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        CsDemandeFraude listForInsertOrUpdate = new CsDemandeFraude();
        private List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> _listeDesReglageCompteurExistant = null;
        public List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> listAppareilsSelectionnes = null;
        private List<CsActionSurCompteur> ListActionEntreprise = new List<CsActionSurCompteur>();
        private List<CsMArqueDisjoncteur> ListMArqueDisjoncteur = new List<CsMArqueDisjoncteur>();
        private List<CsTypeFraude> ListTypeFraude = new List<CsTypeFraude>();
        private List<CsPhaseCompteur> ListPhaseCompteur = new List<CsPhaseCompteur>();
        private List<CsUsage> ListUsager = new List<CsUsage>();
        private List<CsDecisionfrd> ListDecisionfrd = new List<CsDecisionfrd>();
        private List<CsSourceControle> ListSourceControle = new List<CsSourceControle>();
        private List<CsMoyenDenomciation> ListMoyenDenomciation = new List<CsMoyenDenomciation>();
        private List<CsAppareilRecenseFrd> ListAppareilRecenseFrd = new List<CsAppareilRecenseFrd>();
        private List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> lappareils = new List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS>();
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int ConsoDejaFacturee, nombreMois, estime;
        ObservableCollection<CsAppareilUtiliserFrd> DonnesDatagrid = new ObservableCollection<CsAppareilUtiliserFrd>();
       
        int EtapeActuelle;
        public UcConsultation()
        {
            InitializeComponent();
        }

        public UcConsultation(List<int> demande, int etape)
        {
            InitializeComponent();
            EtapeActuelle = etape;
            ChargeDonneDemande(demande.First());
            ChargerProduit();
            ChargerTypeFraude();
            ChargerPhaseCompteur();
            ChargerMarqueDijoncteur();
            ChargerTypeCompteur();
            ChargerActionSurCompteur();
            ChargerMarqueCompteur();
            ChargerReglageCompteur();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerCalibreCompteur();
            ChargerCalibreCompteur_();
            ChargerUsage();
            ChargerDecision();
            ChargerSourceControle();
            ChargerMoyenDenonciation();
            _listeDesCentreExistant = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
            RemplirCentrePerimetre(_listeDesCentreExistant);
        }
        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                {
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.SelectedValuePath = "PK_ID";
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.SelectedValuePath = "PK_ID";
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerMarqueCompteur()
        {
            try
            {
                if (SessionObject.LstMarque != null && SessionObject.LstMarque.Count != 0)
                {
                    Cbo_MarqueCmpt.ItemsSource = null;
                    Cbo_MarqueCmpt.SelectedValuePath = "PK_ID";
                    Cbo_MarqueCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_MarqueCmpt.ItemsSource = SessionObject.LstMarque;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneToutMarqueCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.LstMarque = res.Result;
                    Cbo_MarqueCmpt.ItemsSource = null;
                    Cbo_MarqueCmpt.SelectedValuePath = "PK_ID";
                    Cbo_MarqueCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_MarqueCmpt.ItemsSource = SessionObject.LstMarque;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerMarqueDijoncteur()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllMarqueDisjoncteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_MArqueDijoncteur.ItemsSource = null;
                        Cbo_MArqueDijoncteur.DisplayMemberPath = "Libelle";
                        Cbo_MArqueDijoncteur.SelectedValuePath = "PK_ID";
                        Cbo_MArqueDijoncteur.ItemsSource = args.Result;
                        ListMArqueDisjoncteur = args.Result;
                        

                    }
                };
                client.SelectAllMarqueDisjoncteurAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur != null && SessionObject.LstTypeCompteur.Count != 0)
                {
                    Cbo_typeCompteur.ItemsSource = null;
                    Cbo_typeCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_typeCompteur.ItemsSource = SessionObject.LstTypeCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                    Cbo_typeCompteur.ItemsSource = null;
                    Cbo_typeCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_typeCompteur.ItemsSource = SessionObject.LstTypeCompteur;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerCalibreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count != 0)
                {
                    Cbo_CalibreCompteur.ItemsSource = null;
                    Cbo_CalibreCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreCompteur.ItemsSource = SessionObject.LstCalibreCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                    Cbo_CalibreCompteur.ItemsSource = null;
                    Cbo_CalibreCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreCompteur.ItemsSource = SessionObject.LstCalibreCompteur;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerCalibreCompteur_()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count != 0)
                {
                    Cbo_CalibreDijoncteur.ItemsSource = null;
                    Cbo_CalibreDijoncteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreDijoncteur.ItemsSource = SessionObject.LstCalibreCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                    Cbo_CalibreDijoncteur.ItemsSource = null;
                    Cbo_CalibreDijoncteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreDijoncteur.ItemsSource = SessionObject.LstCalibreCompteur;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerCalibreDisjoncteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count != 0)
                {
                    Cbo_CalibreCompteur.ItemsSource = null;
                    Cbo_CalibreCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreCompteur.ItemsSource = SessionObject.LstCalibreCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                    Cbo_CalibreCompteur.ItemsSource = null;
                    Cbo_CalibreCompteur.DisplayMemberPath = "LIBELLE";
                    Cbo_CalibreCompteur.ItemsSource = SessionObject.LstCalibreCompteur;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerReglageCompteur()
        {
            try
            {
                if (SessionObject.LstReglageCompteur != null && SessionObject.LstReglageCompteur.Count != 0)
                {
                    Cbo_ReglageCmpt.ItemsSource = null;
                    Cbo_ReglageCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_ReglageCmpt.ItemsSource = SessionObject.LstReglageCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerRegalgeCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                    Cbo_ReglageCmpt.ItemsSource = null;
                    Cbo_ReglageCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_ReglageCmpt.ItemsSource = SessionObject.LstReglageCompteur;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerQualiteExpert()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllQualiteExpertCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_QualiteExpert.ItemsSource = null;
                        Cbo_QualiteExpert.DisplayMemberPath = "Libelle";
                        Cbo_QualiteExpert.SelectedValuePath = "PK_ID";
                        Cbo_QualiteExpert.ItemsSource = args.Result;


                    }
                };
                client.SelectAllQualiteExpertAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerTypeFraude()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllTypeFraudeCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_AnormlieCompteur.ItemsSource = null;
                        Cbo_AnormlieCompteur.DisplayMemberPath = "Libelle";
                        Cbo_AnormlieCompteur.SelectedValuePath = "PK_ID";
                        Cbo_AnormlieCompteur.ItemsSource = args.Result.Where(c => c.FK_IDORGANEFRAUDE == 2).ToList();
                        ListTypeFraude = args.Result;
                        //Cbo_AnormlieCompteur.SelectedItem = args.Result.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIECOMPTEUR);

                        Cbo_AnorBranchmnt.ItemsSource = null;
                        Cbo_AnorBranchmnt.DisplayMemberPath = "Libelle";
                        Cbo_AnorBranchmnt.SelectedValuePath = "PK_ID";
                        //Cbo_AnorBranchmnt.ItemsSource = args.Result;
                        Cbo_AnorBranchmnt.ItemsSource = args.Result.Where(c => c.FK_IDORGANEFRAUDE == 1).ToList();
                       // Cbo_AnorBranchmnt.SelectedItem = args.Result.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIEBRANCHEMENT);


                        Cbo_AnormalieCacheb.ItemsSource = null;
                        Cbo_AnormalieCacheb.DisplayMemberPath = "Libelle";
                        Cbo_AnormalieCacheb.SelectedValuePath = "PK_ID";
                        Cbo_AnormalieCacheb.ItemsSource = args.Result.Where(c => c.FK_IDORGANEFRAUDE == 3).ToList(); ;
                      //  Cbo_AnormalieCacheb.SelectedItem = args.Result.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIECACHEBORNE);

                    }
                };
                client.SelectAllTypeFraudeAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //private void ChargerTypeDisjoncteur()
        //{
        //    try
        //    {
        //        FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
        //        client.SelectAllTypeDisjoncteurCompleted += (ssender, args) =>
        //        {
        //            if (args.Cancelled || args.Error != null)
        //            {
        //                string error = args.Error.Message;
        //                Message.ShowError(error, "");
        //                return;
        //            }
        //            if (args.Result == null)
        //            {
        //                //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
        //                Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
        //                return;
        //            }
        //            if (args.Result != null)
        //            {


        //                Cbo_MArqueDijoncteur.ItemsSource = null;
        //                Cbo_MArqueDijoncteur.DisplayMemberPath = "LIBELLE";
        //                Cbo_MArqueDijoncteur.SelectedValuePath = "PK_ID";
        //                Cbo_MArqueDijoncteur.ItemsSource = args.Result;


        //            }
        //        };
        //        client.SelectAllTypeDisjoncteurAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        private void ChargerPhaseCompteur()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllPhaseCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_NbresfilsDijoncteur.ItemsSource = null;
                        Cbo_NbresfilsDijoncteur.DisplayMemberPath = "LIBELLE";
                        Cbo_NbresfilsDijoncteur.SelectedValuePath = "PK_ID";
                        Cbo_NbresfilsDijoncteur.ItemsSource = args.Result;

                        Cbo_Fils.ItemsSource = null;
                        Cbo_Fils.DisplayMemberPath = "LIBELLE";
                        Cbo_Fils.SelectedValuePath = "PK_ID";
                        Cbo_Fils.ItemsSource = args.Result;
                        ListPhaseCompteur = args.Result;

                    }
                };
                client.SelectAllPhaseCompteurAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerActionSurCompteur()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllActionSurCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_ActionEntreprise.ItemsSource = null;
                        Cbo_ActionEntreprise.DisplayMemberPath = "LIBELLE";
                        Cbo_ActionEntreprise.SelectedValuePath = "PK_ID";
                        Cbo_ActionEntreprise.ItemsSource = args.Result;
                        ListActionEntreprise = args.Result;


                    }
                };
                client.SelectAllActionSurCompteurAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerUsage()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllUsageCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_usage.ItemsSource = null;
                        Cbo_usage.DisplayMemberPath = "LIBELLE";
                        Cbo_usage.SelectedValuePath = "PK_ID";
                        Cbo_usage.ItemsSource = args.Result;
                        ListUsager = args.Result;

                    }
                };
                client.SelectAllUsageAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region situation geaographique
        private void RemplirCentrePerimetre(List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentre)
        {
            try
            {
                Cbo_Centre.Items.Clear();
                if (_listeDesCentreExistant != null &&
                    _listeDesCentreExistant.Count != 0)
                {
                    if (lstCentre != null)
                        foreach (var item in lstCentre)
                        {
                            Cbo_Centre.Items.Add(item);
                        }
                    Cbo_Centre.SelectedValuePath = "PK_ID";
                    Cbo_Centre.DisplayMemberPath = "LIBELLE";

                    //if (lstSite != null)
                    //    foreach (var item in lstSite)
                    //    {
                    //        Cbo_Site.Items.Add(item);
                    //    }
                    //Cbo_Site.SelectedValuePath = "PK_ID";
                    //Cbo_Site.DisplayMemberPath = "LIBELLE";

                    //if (lstSite != null && lstSite.Count == 1)
                    //    Cbo_Site.SelectedItem = lstSite.First();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.Cbo_Centre.SelectedItem != null)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCentre;
                    if (centre != null)
                    {
                        this.txtCentre.Text = centre.CODE ?? string.Empty;
                        this.txtCentre.Tag = centre.PK_ID;
                        RemplirCommuneParCentre(centre);
                        //  RemplirProduitCentre(centre);
                    }
                    //  VerifierDonneesSaisiesInformationsDevis();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }
        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
                    Cbo_Commune.ItemsSource = null;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";
                    Cbo_Commune.ItemsSource = SessionObject.LstCommune;
                    _listeDesCommuneExistant = SessionObject.LstCommune;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCommuneAsync();
                service.ChargerCommuneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    SessionObject.LstCommune = args.Result;
                    Cbo_Commune.ItemsSource = null;
                    Cbo_Commune.SelectedValuePath = "PK_ID";
                    Cbo_Commune.DisplayMemberPath = "LIBELLE";
                    Cbo_Commune.ItemsSource = SessionObject.ListeDesProduit;
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirCommuneParCentre(Galatee.Silverlight.ServiceAccueil.CsCentre centre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList();
                //!= null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList() : new List<Galatee.Silverlight.ServiceAccueil.CsCommune>();
                txt_Commune.Text = string.Empty;
                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                Cbo_Commune.IsEnabled = true;
                Cbo_Commune.SelectedValuePath = "PK_ID";
                Cbo_Commune.DisplayMemberPath = "LIBELLE";

                Cbo_Commune.ItemsSource = _listeDesCommuneExistantCentre;
                if (_listeDesCommuneExistantCentre.Count > 0)
                {
                    if (_listeDesCommuneExistantCentre.Count == 1)
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre[0];
                }
                else
                {
                    Message.ShowError("Aucune commune associé à ce centre", "Info");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesQuartierExistant()
        {
            try
            {

                if (SessionObject.LstQuartier != null && SessionObject.LstQuartier.Count != 0)
                {
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    Cbo_Quartier.ItemsSource = _listeDesQuartierExistant;
                    Cbo_Quartier.IsEnabled = true;
                    Cbo_Quartier.SelectedValuePath = "PK_ID";
                    Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                    return;
                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesQartiersAsync();
                service.ChargerLesQartiersCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstQuartier = args.Result;
                    _listeDesQuartierExistant = SessionObject.LstQuartier;
                    Cbo_Quartier.ItemsSource = _listeDesQuartierExistant;
                    Cbo_Quartier.IsEnabled = true;
                    Cbo_Quartier.SelectedValuePath = "PK_ID";
                    Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirQuartier(int pCommuneId)
        {
            List<Galatee.Silverlight.ServiceAccueil.CsQuartier> ListeQuartierFiltres = new List<Galatee.Silverlight.ServiceAccueil.CsQuartier>();
            List<Galatee.Silverlight.ServiceAccueil.CsQuartier> QuartierParDefaut = null;

            this.txt_Quartier.Text = string.Empty;
            try
            {
                QuartierParDefaut = _listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId).ToList();
                if (QuartierParDefaut != null && QuartierParDefaut.Count > 0)
                    ListeQuartierFiltres.AddRange(QuartierParDefaut);
                ListeQuartierFiltres.AddRange(_listeDesQuartierExistant.Where(q => q.FK_IDCOMMUNE == pCommuneId && q.CODE != DataReferenceManager.QuartierInconnu).ToList());

                if (ListeQuartierFiltres.Count > 0)
                    //foreach (var item in ListeQuartierFiltres)
                    //{
                    //    Cbo_Quartier.Items.Add(item);
                    //}
                    Cbo_Quartier.ItemsSource = null;
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;
                Cbo_Quartier.SelectedValuePath = "PK_ID";
                Cbo_Quartier.DisplayMemberPath = "LIBELLE";
                Cbo_Quartier.ItemsSource = ListeQuartierFiltres;

                //Cbo_Quartier.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirRues(int pIdCommune)
        {
            List<Galatee.Silverlight.ServiceAccueil.CsRues> ListeRuesFiltrees = new List<Galatee.Silverlight.ServiceAccueil.CsRues>();
            List<Galatee.Silverlight.ServiceAccueil.CsRues> RueParDefaut = null;
            this.txt_NumRue.Text = string.Empty;
            try
            {
                RueParDefaut = _listeDesRuesExistant.Where(q => q.CODE == DataReferenceManager.RueInconnue).ToList();
                if (RueParDefaut != null && RueParDefaut.Count > 0)
                    ListeRuesFiltrees.AddRange(RueParDefaut);
                ListeRuesFiltrees.AddRange(_listeDesRuesExistant.Where(q => q.PK_ID == pIdCommune && q.CODE != DataReferenceManager.RueInconnue).ToList());

                Cbo_Rue.ItemsSource = null;
                Cbo_Rue.ItemsSource = ListeRuesFiltrees;
                Cbo_Rue.SelectedValuePath = "PK_ID";
                Cbo_Rue.DisplayMemberPath = "LIBELLE";
                //Cbo_Rue.IsEnabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeDesRuesExistant()
        {
            try
            {

                if (SessionObject.LstRues != null && SessionObject.LstRues.Count != 0)
                {
                    _listeDesRuesExistant = SessionObject.LstRues;
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerLesRueDesSecteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstRues = args.Result;
                    _listeDesRuesExistant = SessionObject.LstRues;
                };
                service.ChargerLesRueDesSecteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirSecteur()
        {
            try
            {
                if (SessionObject.LstSecteur != null && SessionObject.LstSecteur.Count != 0)
                {
                    Cbo_Secteur.Items.Clear();
                    Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                    Cbo_Secteur.SelectedValuePath = "PK_ID";
                    Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                    return;
                }
                else
                {

                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.ChargerLesSecteursCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        SessionObject.LstSecteur = args.Result;
                        Cbo_Secteur.Items.Clear();
                        Cbo_Secteur.ItemsSource = SessionObject.LstSecteur;
                        Cbo_Secteur.SelectedValuePath = "PK_ID";
                        Cbo_Secteur.DisplayMemberPath = "LIBELLE";
                        return;
                    };
                    service.ChargerLesSecteursAsync();
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Cbo_Commune_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Commune.SelectedItem != null)
                {
                    Galatee.Silverlight.ServiceAccueil.CsCommune commune = Cbo_Commune.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCommune;
                    if (commune != null)
                    {
                        Cbo_Commune.SelectedItem = commune;
                        txt_Commune.Text = commune.CODE ?? string.Empty;
                        RemplirQuartier(commune.PK_ID);
                        RemplirRues(commune.PK_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }

        private void Cbo_Quartier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Quartier.SelectedItem != null)
                {
                    var quartier = Cbo_Quartier.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsQuartier;
                    if (quartier != null)
                    {
                        txt_Quartier.Text = quartier.CODE ?? string.Empty;
                        List<ServiceAccueil.CsSecteur> lstSecteur = SessionObject.LstSecteur.Where(t => t.FK_IDQUARTIER == quartier.PK_ID).ToList();
                        this.Cbo_Secteur.ItemsSource = lstSecteur;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }

        private void Cbo_Rue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Rue.SelectedItem != null)
                {
                    var Secteur = Cbo_Rue.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsRues;
                    if (Secteur != null)
                        txt_NumRue.Text = Secteur.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }

        private void Cbo_Secteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Secteur.SelectedItem != null)
                {
                    var Secteur = Cbo_Secteur.SelectedItem as ServiceAccueil.CsSecteur;
                    if (Secteur != null)
                        txt_NumSecteur.Text = Secteur.CODE ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }
        #endregion


        private void check_maisonhabite_Checked(object sender, RoutedEventArgs e)
        {
            if (check_maisonhabite.IsChecked == true)
            {
                txt_NombreHabitant.IsReadOnly = false;
            }
            else
                txt_NombreHabitant.IsReadOnly = true;


        }

        private void check_abnmotif_Checked(object sender, RoutedEventArgs e)
        {
            if (check_maisonhabite.IsChecked == true)
            {
                txt_abnmotif.IsReadOnly = false;
            }
            else
                txt_abnmotif.IsReadOnly = true;
        }

        private void ChargerDecision()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllDecisionCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_Decision.ItemsSource = null;
                        Cbo_Decision.DisplayMemberPath = "Libelle";
                        Cbo_Decision.SelectedValuePath = "PK_ID";
                        Cbo_Decision.ItemsSource = args.Result;
                        ListDecisionfrd = args.Result;
                        //if( LaDemande.Fraude.FK_IDDECISIONFRAUDE!=null)
                        //Cbo_Decision.SelectedItem = ListDecisionfrd.FirstOrDefault(t => t.PK_ID == LaDemande.Fraude.FK_IDDECISIONFRAUDE);


                    }
                };
                client.SelectAllDecisionAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void txtConsommationDejaFacturee_TextChanged(object sender, TextChangedEventArgs e)
        {
            int dejaFacturée = 0, estiméeEquipement = 0, retrogradation = 0;
            if (this.txtConsommationDejaFacturee.Text != string.Empty)
                dejaFacturée = int.Parse(this.txtConsommationDejaFacturee.Text.Trim());
            if (this.txtConsommationEstimeeEquipement.Text != string.Empty)
                estiméeEquipement = int.Parse(this.txtConsommationEstimeeEquipement.Text.Trim());
            if (this.txtRetrogradation.Text != string.Empty)
                retrogradation = int.Parse(this.txtRetrogradation.Text.Trim());

            this.txtTotalEstime.Text = (estiméeEquipement + retrogradation).ToString();
            this.txtConsommationAFacturer.Text = (estiméeEquipement + retrogradation - dejaFacturée).ToString();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Message.Show("Affectation effectuée avec succès", "Information");
            List<int> Listid = new List<int>();
            Listid.Add(LaDemande.LaDemande.PK_ID);
            EnvoyerDemandeEtapeSuivante(Listid);
            this.DialogResult = true;

        }
        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel éffectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargeDonneDemande(int pk_id)
        {

            FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
            service.RetourDemandeFraudeCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;

                    if (LaDemande != null)
                    {

                        //--Initalisation
                       
                        
                        //----Infor Abonnée------------------------//

                        txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Nomabon) ? string.Empty : LaDemande.ClientFraude.Nomabon;
                        txt_refclient.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Client) ? string.Empty : LaDemande.ClientFraude.Client; ;
                        txt_emailIn.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Email) ? string.Empty : LaDemande.ClientFraude.Email; ;
                        txt_telephone.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Telephone) ? string.Empty : LaDemande.ClientFraude.Telephone; ;
                        txt_porteIn.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Porte) ? string.Empty : LaDemande.ClientFraude.Porte; ;
                        txt_ContactAbonne.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratAbonnement) ? string.Empty : LaDemande.ClientFraude.ContratAbonnement;
                        txt_contarBrachement.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratBranchement) ? string.Empty : LaDemande.ClientFraude.ContratBranchement;
                        Cbo_Centre.SelectedItem = _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDCENTRE);
                        Cbo_Commune.SelectedItem = _listeDesCommuneExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDCOMMUNE);
                        Cbo_Quartier.SelectedItem = _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDQUARTIER);
                        Cbo_Rue.SelectedItem = _listeDesRuesExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_SECTEUR);
                        Cbo_Secteur.SelectedItem = SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_RUE);
                        DateAbonnemnt.SelectedDate = LaDemande.ClientFraude.DateContratAbonnement == null ? null : LaDemande.ClientFraude.DateContratAbonnement;
                        DateBranchemnt.SelectedDate = LaDemande.ClientFraude.DateContratBranchement == null ? null : LaDemande.ClientFraude.DateContratBranchement; ;

                        //----Info Controle----------------------------------//

                        txt_Numerotraitement.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                        txt_FichControle.Text = string.IsNullOrEmpty(LaDemande.Controle.FicheControle) ? string.Empty : LaDemande.Controle.FicheControle;
                        txt_Nomexpert.Text = string.IsNullOrEmpty(LaDemande.Controle.NomExpert) ? string.Empty : LaDemande.Controle.NomExpert;
                        DateControle.SelectedDate = LaDemande.Fraude.DateCreation;
                        txt_courantAdmn.Text = string.IsNullOrEmpty(LaDemande.Controle.CourantAdmissibleParCable.ToString()) ? string.Empty : LaDemande.Controle.CourantAdmissibleParCable.ToString();
                        txt_ordinateur.Text = string.IsNullOrEmpty(LaDemande.Controle.Ordonnateur) ? string.Empty : LaDemande.Controle.CourantAdmissibleParCable.ToString();
                        txt_Commissarial.Text = string.IsNullOrEmpty(LaDemande.Controle.CommissariatPolicePresent) ? string.Empty : LaDemande.Controle.CommissariatPolicePresent;
                        chck_fraudAve.IsChecked = LaDemande.Fraude.IsFraudeConfirmee;
                        chck_abonne.IsChecked = LaDemande.Controle.IsAbonneOuRepresentantPresent;
                        chck_convocation.IsChecked = LaDemande.Controle.IsConvocationRemise;
                        chck_anomalie.IsChecked = LaDemande.Controle.IsFraudeAveree;

                        //--- info Compteur-------------------------------//

                        Cbo_Produit.SelectedItem = SessionObject.ListeDesProduit.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDPRODUIT);
                        Cbo_MarqueCmpt.SelectedItem = SessionObject.LstMarque.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDMARQUECOMPTEUR);
                        Cbo_typeCompteur.SelectedItem = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDTYPECOMPTEUR);
                        Cbo_CalibreCompteur.SelectedItem = SessionObject.LstCalibreCompteur.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDCALIBRECOMPTEUR);
                        Cbo_ReglageCmpt.SelectedItem = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDREGLAGE); ;
                        Cbo_Fils.SelectedItem = ListPhaseCompteur.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDPHASECOMPTEUR);
                        Cbo_MArqueDijoncteur.SelectedItem = ListMArqueDisjoncteur.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDMARQUEDISJONCTEUR);
                        Cbo_AnorBranchmnt.SelectedItem = ListTypeFraude.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIEBRANCHEMENT);
                        Cbo_AnormalieCacheb.SelectedItem = ListTypeFraude.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIECACHEBORNE);
                        Cbo_AnormlieCompteur.SelectedItem = ListTypeFraude.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDANOMALIECOMPTEUR);
                        Cbo_ActionEntreprise.SelectedItem = ListActionEntreprise.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDACTIONSURCOMPTEUR);
                        Cbo_usage.SelectedItem = ListUsager.FirstOrDefault(t => t.PK_ID == LaDemande.CompteurFraude.FK_IDUSAGEPRODUIT);
                        txt_numero.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.NumeroCompteur) ? string.Empty : LaDemande.CompteurFraude.NumeroCompteur;
                        txt_numero.Tag = LaDemande.CompteurFraude.PK_ID;
                        txt_certifiplombage.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.CertificatPlombage) ? string.Empty : LaDemande.CompteurFraude.CertificatPlombage;
                        txt_refeplombs.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.RefPlombCompteur) ? string.Empty : LaDemande.CompteurFraude.RefPlombCompteur;
                        txt_CoffreFusile.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.RefPlombCoffretFusible) ? string.Empty : LaDemande.CompteurFraude.RefPlombCoffretFusible;
                        txtCoffreSeruite.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.RefPlombCoffretSecurite) ? string.Empty : LaDemande.CompteurFraude.RefPlombCoffretSecurite;
                        ListAppareilRecenseFrd = LaDemande.AppareilRecenseFrd;
                        lConertirList(LaDemande.AppareilRecenseFrd);

                        //---Audition

                         Check_Abnpropriete.IsChecked = LaDemande.AuditionFraude.IsProprietaire;
                        check_Abnpenalite.IsChecked = LaDemande.AuditionFraude.IsDejaPenaliseSurCompteur;
                        check_DemandeVerifie.IsChecked = LaDemande.AuditionFraude.IsDemandeVerificationDejaEmise;
                        check_Reception.IsChecked = LaDemande.AuditionFraude.IsAccuseReceptionDemande;
                        check_maisonhabite.IsChecked = LaDemande.AuditionFraude.IsMaisonHabitee;
                        check_abnmotif.IsChecked = LaDemande.AuditionFraude.IsDejaDepanne;
                        check_Autrefacture.IsChecked = LaDemande.AuditionFraude.IsFacturePenaliteDejaRecue;
                        check_certifie.IsChecked = LaDemande.AuditionFraude.IsCertificatPlombageRecu;
                        check_Nouvelle_Acqui.IsChecked = LaDemande.AuditionFraude.IsNewAppareilAcquis;
                        daterdv.SelectedDate = LaDemande.AuditionFraude.DateRendezVous;
                        dateAudition.SelectedDate = LaDemande.AuditionFraude.DateRendezVous;
                        txt_nomrepond.Text = LaDemande.AuditionFraude.NomRepondant;
                        txt_NombreHabitant.Text = LaDemande.AuditionFraude.NombreHabitant.ToString();
                        txt_abnmotif.Text = LaDemande.AuditionFraude.MotifDepannage.ToString();

                        //--------- Consommation deja facturée

                        txtConsommationDejaFacture.Text = LaDemande.ConsommationFrd.ConsommationDejaFacturee.ToString();

                        //--
                        foreach (CsMoisDejaFactures item in LaDemande.MoisDejaFactures)
                        {
                            ConsoDejaFacturee = Convert.ToInt32(item.ConsoDejaFacturee) + ConsoDejaFacturee;
                            nombreMois++;
                        }
                        GetData(LaDemande.AppareilUtiliserFrd);
                        txtConsommationDejaFacturee.Text = ConsoDejaFacturee.ToString();
                        txtConsommationAFacturer.Text = (Convert.ToInt32(txtTotalEstime.Text) - ConsoDejaFacturee).ToString();
                        Cbo_Decision.SelectedItem = ListDecisionfrd.FirstOrDefault(t => t.PK_ID == LaDemande.Fraude.FK_IDDECISIONFRAUDE);
                        this.ckbFraudeConfirmée.IsChecked=LaDemande.Fraude.IsFraudeConfirmee;
                        this.txtConsommationAFacturer.Text=  LaDemande.ConsommationFrd.ConsommationAFacturer.ToString();
                        this.txtConsommationEstimeeEquipement.Text=  LaDemande.ConsommationFrd.ConsommationEstimee.ToString();
                        this.txtRetrogradation.Text= LaDemande.ConsommationFrd.ConsommationRetrogradation.ToString() ;
                        this.txtConsommationDejaFacturee.Text=LaDemande.ConsommationFrd.ConsommationDejaFacturee.ToString() ;
                      
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeFraudeAsync(pk_id);

        }

        private void GetData(List<CsAppareilUtiliserFrd> _AppareilUtiliserFrd)
        {
            try
            {
                //UcRemisesScelles ctrl = new UcRemisesScelles();
                //ctrl.Closed += new EventHandler(RafraichirList);
                //ctrl.Show();

                DonnesDatagrid.Clear();
                if (_AppareilUtiliserFrd != null && _AppareilUtiliserFrd.Count > 0)
                {
                    foreach (var item in _AppareilUtiliserFrd)
                    {
                        item.estimee = item.Mensuelle * nombreMois;
                        estime = item.estimee + estime;
                        DonnesDatagrid.Add(item);
                    }
                }
                dtgrdAnnalyse.ItemsSource = DonnesDatagrid;
                txtConsommationEstimeeEquipement.Text = estime.ToString();
                this.txtTotalEstime.Text = ((float)estime).ToString();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChargerSourceControle()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllSourceControleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_SourceControle.ItemsSource = null;
                        Cbo_SourceControle.DisplayMemberPath = "Libelle";
                        Cbo_SourceControle.SelectedValuePath = "PK_ID";
                        Cbo_SourceControle.ItemsSource = args.Result;
                        ListSourceControle = args.Result;

                    }
                };
                client.SelectAllSourceControleAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ChargerMoyenDenonciation()
        {
            try
            {
                FraudeServiceClient client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                client.SelectAllMoyenDenomciationCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, "");
                        return;
                    }
                    if (args.Result == null)
                    {
                        //Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Parametrage);
                        Message.ShowError(Galatee.Silverlight.Resources.Devis.Languages.msgErreurChargementDonnees, "");
                        return;
                    }
                    if (args.Result != null)
                    {
                        Cbo_MoyenDenociation.ItemsSource = null;
                        Cbo_MoyenDenociation.DisplayMemberPath = "Libelle";
                        Cbo_MoyenDenociation.SelectedValuePath = "PK_ID";
                        Cbo_MoyenDenociation.ItemsSource = args.Result;
                        ListMoyenDenomciation = args.Result;

                    }
                };
                client.SelectAllMoyenDenomciationAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region Liste des Appareils
        private void lConertirList(List<CsAppareilRecenseFrd> lisapprail)
        {
            foreach (CsAppareilRecenseFrd Item in lisapprail)
            {
                var appareils = new Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS
                {

                    PK_ID = Item.FK_IDAPPAREIL,
                    PUISSANCE = (int)Item.PUISSANCEUNITAIRE,
                    NOMBRE = (int)Item.NOMBRE,
                    DISPLAYLABEL = Item.OBSERVATION,
                    CODEAPPAREIL = Item.CODEAPPAREIL.ToString(),

                };
                lappareils.Add(appareils);
            }
            RemplirListeAppareils(lappareils);
        }
        private void RemplirListeAppareils(List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> lappareils)
        {
            try
            {
                int sommePuissance = 0;
                decimal intensite = 0;

                Cbo_ListeAppareils.Items.Clear();
                foreach (var item in lappareils)
                {
                    sommePuissance = sommePuissance + (item.NOMBRE * item.PUISSANCE);
                    Cbo_ListeAppareils.Items.Add(item);
                }

                Cbo_ListeAppareils.SelectedValuePath = "CODEAPPAREIL";
                Cbo_ListeAppareils.DisplayMemberPath = "DISPLAYLABEL";
                listAppareilsSelectionnes = lappareils;
                Cbo_ListeAppareils.SelectedIndex = 0;

                Galatee.Silverlight.ServiceAccueil.CsProduit leProduitSelect = Cbo_Produit.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsProduit;

                if (sommePuissance != 0)
                    intensite = sommePuissance / 220;
                //if (leProduitSelect != null)
                //{
                //    List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> _listeDesDiametrePuissance = _listeDesReglageCompteurExistant.Where(p => p.REGLAGE >= intensite && p.FK_IDPRODUIT == leProduitSelect.PK_ID).ToList();
                //    if (_listeDesDiametrePuissance == null || _listeDesDiametrePuissance.Count == 0)
                //    {
                //        Cbo_Diametre.ItemsSource = null;
                //        Cbo_Diametre.ItemsSource = _listeDesReglageCompteurExistant.Where(p => p.FK_IDPRODUIT == leProduitSelect.PK_ID);
                //        Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                //        return;
                //    }

                //    Cbo_Diametre.ItemsSource = null;
                //    Cbo_Diametre.ItemsSource = _listeDesDiametrePuissance;
                //    Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_ListeAppareils_Click(object sender, RoutedEventArgs e)
        {
            List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> listeAppareil = null;
            try
            {
                var UcListAppareils = new Galatee.Silverlight.Devis.UcListAppareils();
                if (Cbo_ListeAppareils.Items.Count > 0)
                {
                    listeAppareil = new List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS>();
                    foreach (Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS appareil in Cbo_ListeAppareils.Items)
                        listeAppareil.Add(appareil);
                }
                UcListAppareils.AppareilsSelectionnes = listeAppareil;
                UcListAppareils.Closed += new EventHandler(UcListAppareils_Closed);
                UcListAppareils.Show();

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.Fraude);
            }
        }
        private void UcListAppareils_Closed(object sender, EventArgs e)
        {
            try
            {
                var lappareils = ((Galatee.Silverlight.Devis.UcListAppareils)sender).AppareilsSelectionnes;
                if (lappareils != null && lappareils.Count > 0)
                {
                    RemplirListeAppareils(lappareils);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

