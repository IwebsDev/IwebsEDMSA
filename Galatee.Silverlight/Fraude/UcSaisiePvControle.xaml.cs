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
using Galatee.Silverlight.ServiceFraude;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcSaisiePvControle : ChildWindow
    {

        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        private string Tdem = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsReglageCompteur> _listeDesReglageCompteurExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> listAppareilsSelectionnes = null;
        CsDemandeFraude listForInsertOrUpdate = new CsDemandeFraude();
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle;
        //private CsCompteur
        public UcSaisiePvControle()
        {
            InitializeComponent();
            ChargerQualiteExpert();
            ChargerProduit();
            ChargerTypeCompteur();
            //ChargerDiametreCompteur();
            ChargerReglageCompteur();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
         
        }
        public UcSaisiePvControle(List<int> demande, int etape)
        {
            EtapeActuelle = etape;
            InitializeComponent();
            ChargeDonneDemande(demande.First());
            ChargerQualiteExpert();
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
            _listeDesCentreExistant = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
            RemplirCentrePerimetre(_listeDesCentreExistant);
        }
          //this.RemplirUsageElectricite(ctr);
          //  this.RemplirCalibre(ctr);
          //  this.RemplirNombreFils(ctr);
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                        //Cbo_AnormlieCompteur.ItemsSource = args.Result;

                        Cbo_AnorBranchmnt.ItemsSource = null;
                        Cbo_AnorBranchmnt.DisplayMemberPath = "Libelle";
                        Cbo_AnorBranchmnt.SelectedValuePath = "PK_ID";
                        //Cbo_AnorBranchmnt.ItemsSource = args.Result;
                        Cbo_AnorBranchmnt.ItemsSource = args.Result.Where(c => c.FK_IDORGANEFRAUDE == 1).ToList();


                        Cbo_AnormalieCacheb.ItemsSource = null;
                        Cbo_AnormalieCacheb.DisplayMemberPath = "Libelle";
                        Cbo_AnormalieCacheb.SelectedValuePath = "PK_ID";
                        Cbo_AnormalieCacheb.ItemsSource = args.Result.Where(c => c.FK_IDORGANEFRAUDE == 3).ToList(); ;

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


                    }
                };
                client.SelectAllUsageAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private CsFraude GetInformationsFromScreenFrd()
        {
            var listObjetForInsertOrUpdate = new CsFraude();
            try
            {

                LaDemande.Fraude.IsFraudeConfirmee = (bool)chck_fraudAve.IsChecked;
                return LaDemande.Fraude;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        private CsClientFraude GetInformationsFromScreenClientFrd()
        {
            var listObjetForInsertOrUpdate = new CsClientFraude();
            try
            {

                var sClientFraude = new CsClientFraude
                {
                    Nomabon=txt_Nom.Text,
                    PK_ID=Convert.ToInt32(  txt_Nom.Tag),
                    Client=txt_refclient.Text,
                    IdentificationUnique=txt_IdentUnique.Text,
                    Commune =((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).LIBELLE,
                    Quartier = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).LIBELLE,
                    Rue = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).LIBELLE,
                    Secteur = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).LIBELLE,
                    Centre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).CODE,
                    Email=txt_email.Text,
                    Telephone=txt_telephone.Text,
                    Porte=txt_porte.Text,
                    ContratAbonnement = string.IsNullOrEmpty(txt_ContactAbonne.Text) ? string.Empty : txt_ContactAbonne.Text,
                    ContratBranchement = string.IsNullOrEmpty(txt_contarBrachement.Text) ? string.Empty : txt_contarBrachement.Text,
                    DateContratAbonnement = DateAbonnemnt.SelectedDate != null ? null : DateAbonnemnt.SelectedDate,
                    DateContratBranchement = DateBranchemnt.SelectedDate !=null? null:DateBranchemnt.SelectedDate,
                    FK_IDCENTRE = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID,
                    FK_IDCOMMUNE = ((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).PK_ID,
                    FK_IDQUARTIER = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).PK_ID,
                    FK_RUE = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).PK_ID,
                    FK_SECTEUR = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).PK_ID,
                    Ordre= LaDemande.ClientFraude.Ordre,
                   
                };
                listObjetForInsertOrUpdate=sClientFraude ;
                

                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        private CsControle GetInformationsFromScreenControle()
        {
            var listObjetForInsertOrUpdate = new CsControle();
            try
            {

                var sControle = new CsControle
                {
                    CommissariatPolicePresent = txt_Commissarial.Text,
                    FicheControle = txt_FichControle.Text,
                    FK_IDQUALITEEXPERT = ((CsQualiteExpert)Cbo_QualiteExpert.SelectedItem).PK_ID,
                    DateControle = Convert.ToDateTime(DateControle.SelectedDate),
                   NomExpert=txt_Nomexpert.Text,
                   // CourantAdmissibleParCable=Convert.ToInt32( txt_courantAdmn.Text),
                   IsAbonneOuRepresentantPresent=(bool)chck_abonne.IsChecked,
                   IsConvocationRemise=(bool)chck_convocation.IsChecked,
                   Ordonnateur=txt_ordinateur.Text,
                   IsFraudeAveree = (bool)chck_anomalie.IsChecked,
                   IsAnomalieReconnue = (bool)chck_anomalie.IsChecked
                };
                listObjetForInsertOrUpdate = sControle;


                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }
        
        private CsControleur GetInformationsFromScreenControleur()
        {
            var listObjetForInsertOrUpdate = new CsControleur();
            try
            {



                var sControleur = new CsControleur
                {
                   FK_IDUSERCONTROLEUR=UserConnecte.PK_ID,
                   
                };
                listObjetForInsertOrUpdate = sControleur;


                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;



                #region information abonnement 

                    if (string.IsNullOrEmpty(this.DateControle.SelectedDate.ToString()))
                        throw new Exception("Selectionnez Date Controle");
                    if (string.IsNullOrEmpty(this.txt_numero.Text))
                        throw new Exception("Selectionnez le numero compteur");
                    if (Cbo_ActionEntreprise.SelectedItem == null)
                        throw new Exception("Selectionnez Action Entreprise ");
                    if (Cbo_typeCompteur.SelectedItem == null)
                        throw new Exception("Selectionnez Type Compteur ");
                    if (Cbo_AnorBranchmnt.SelectedItem == null)
                        throw new Exception("Selectionnez Anormalie Branchment ");
                    if (Cbo_AnormalieCacheb.SelectedItem == null)
                        throw new Exception("Selectionnez Anormalie Cache borne ");
                    if (Cbo_AnormlieCompteur.SelectedItem == null)
                        throw new Exception("Selectionnez Anormalie Compteur ");
                    if (Cbo_CalibreCompteur.SelectedItem == null)
                        throw new Exception("Selectionnez Calibre Compteur ");
                    if (Cbo_CalibreDijoncteur.SelectedItem == null)
                        throw new Exception("Selectionnez Calibre Dijoncteur ");
                    if (Cbo_Fils.SelectedItem == null)
                        throw new Exception("Selectionnez Calibre Fils ");
                    if (Cbo_MarqueCmpt.SelectedItem == null)
                        throw new Exception("Selectionnez marque Compteur ");
                    if (Cbo_MArqueDijoncteur.SelectedItem == null)
                        throw new Exception("Selectionnez marque Dijoncteur ");

                    if (Cbo_NbresfilsDijoncteur.SelectedItem == null)
                        throw new Exception("Selectionnez nombres de fils rque Dijoncteur ");
                    if (Cbo_ReglageCmpt.SelectedItem == null)
                        throw new Exception("Selectionnez reglagle compteur");
                    if (Cbo_usage.SelectedItem == null)
                        throw new Exception("Selectionnez usage");
                    if (Cbo_ListeAppareils.SelectedItem == null)
                        throw new Exception("Selectionnez la liste des appareils");

                   if (Cbo_Produit.SelectedItem == null)
                        throw new Exception("Selectionnez Produit");

                    if (string.IsNullOrEmpty(this.txt_CoffreFusile.Text))
                        throw new Exception("remplir le coffre Fusile ");

                    if (string.IsNullOrEmpty(this.txt_certifiplombage.Text))
                        throw new Exception("remplir le certifie plombage ");

                    if (string.IsNullOrEmpty(this.txt_refeplombs.Text))
                        throw new Exception("remplir referend plomgs ");

                    if (string.IsNullOrEmpty(this.txt_reference_plombs.Text))
                        throw new Exception("remplir referend plomgs ");
                    if (string.IsNullOrEmpty(this.DateAbonnemnt.SelectedDate.ToString()))
                        throw new Exception("remplir la date ");
                    if (string.IsNullOrEmpty(this.DateBranchemnt.SelectedDate.ToString()))
                        throw new Exception("remplir la date ");
                   
                //if (string.IsNullOrEmpty(this.txt.Text))
                //        throw new Exception("remplir referend plomgs ");

                    //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                    //{
                    //    if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                    //        throw new Exception("Selectionnez le calibre ");
                    //}
                    #endregion

                    #region Adresse géographique

                    if (Cbo_Centre.SelectedItem == null)
                        throw new Exception("Selectionnez Centre");

                    if (Cbo_Quartier.SelectedItem == null)
                        throw new Exception("Selectionnez Quartier");

                    if (Cbo_Commune.SelectedItem == null)
                        throw new Exception("Selectionnez Commune");


                    if (Cbo_Rue.SelectedItem == null)
                        throw new Exception("Selectionnez Rue");

                    if (Cbo_Secteur.SelectedItem == null)
                        throw new Exception("Selectionnez Secteur");

                    if (string.IsNullOrEmpty(this.txtCentre.Text))
                        throw new Exception("Séléctionnez le Centre ");

                    if (string.IsNullOrEmpty(this.txt_Commune.Text))
                        throw new Exception("Séléctionnez la commune ");

                    if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                        throw new Exception("Séléctionnez le quartier ");

                  
                    #endregion
              
                return ReturnValue;

            }
            catch (Exception ex)
            {
                this.BtnTRansfert.IsEnabled = true;
               //this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Fraude");
                return false;
            }

        }
        private CsCompteurFraude GetInformationsFromScreenCompteurFraude()
        {
            var listObjetForInsertOrUpdate = new CsCompteurFraude();
            try
            {
                var sCompteurFraude = new CsCompteurFraude
                {
                    NumeroCompteur = string.IsNullOrEmpty(txt_numero.Text) ? string.Empty : txt_numero.Text,
                    FK_IDTYPECOMPTEUR = ((Galatee.Silverlight.ServiceAccueil.CsTcompteur)Cbo_typeCompteur.SelectedItem).PK_ID,
                    FK_IDCALIBRECOMPTEUR = ((Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur)Cbo_CalibreCompteur.SelectedItem).PK_ID,
                    CertificatPlombage = string.IsNullOrEmpty(txt_certifiplombage.Text) ? string.Empty : txt_certifiplombage.Text,
                    FK_IDPRODUIT = ((Galatee.Silverlight.ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID,
                    FK_IDREGLAGE = ((Galatee.Silverlight.ServiceAccueil.CsReglageCompteur)Cbo_ReglageCmpt.SelectedItem).PK_ID,
                    FK_IDANOMALIECACHEBORNE = ((CsTypeFraude)Cbo_AnormalieCacheb.SelectedItem).PK_ID,
                    FK_IDANOMALIEBRANCHEMENT = ((CsTypeFraude)Cbo_AnorBranchmnt.SelectedItem).PK_ID,
                    FK_IDANOMALIECOMPTEUR = ((CsTypeFraude)Cbo_AnormlieCompteur.SelectedItem).PK_ID,
                    FK_IDACTIONSURCOMPTEUR = ((CsActionSurCompteur)Cbo_ActionEntreprise.SelectedItem).PK_ID,
                    RefPlombCoffretFusible = string.IsNullOrEmpty(txt_CoffreFusile.Text) ? string.Empty : txt_CoffreFusile.Text,
                    RefPlombCoffretSecurite = string.IsNullOrEmpty(txtCoffreSeruite.Text) ? string.Empty : txtCoffreSeruite.Text,
                    RefPlombCompteur=txt_refeplombs.Text,
                    PRODUIT = ((Galatee.Silverlight.ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).CODE,
                    TYPECOMPTEUR = ((Galatee.Silverlight.ServiceAccueil.CsTcompteur)Cbo_typeCompteur.SelectedItem).CODE,
                    FK_IDUSAGEPRODUIT = ((CsUsage)Cbo_usage.SelectedItem).PK_ID,
                    USAGEPRODUIT = ((CsUsage)Cbo_usage.SelectedItem).CODE,
                    FK_IDCLIENTFRAUDE = Convert.ToInt32(txt_Nom.Tag),
                    FK_IDPHASECOMPTEUR = ((CsPhaseCompteur)Cbo_Fils.SelectedItem).PK_ID,
                    FK_IDMARQUECOMPTEUR = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_MarqueCmpt.SelectedItem).PK_ID,
                    FK_IDMARQUEDISJONCTEUR = ((CsMArqueDisjoncteur)Cbo_MArqueDijoncteur.SelectedItem).PK_ID,
                    MARQUE = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_MarqueCmpt.SelectedItem).CODE,
                    IndexCompteur=Convert.ToInt32( txt_Index.Text),
                    NumeroPince = txt_Numbrepince.Text,
                };
                listObjetForInsertOrUpdate = sCompteurFraude;


                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Langue.RecuperationEchec);
                throw ex;
                return null;
            }
        }


        private List<CsAppareilRecenseFrd> GetInformationsFromScreenControle(List<Galatee.Silverlight.ServiceAccueil.ObjAPPAREILS> lappareils)
        {
            var listObjetForInsertOrUpdate = new  List<CsAppareilRecenseFrd>();
            try
            {


                foreach (var item in lappareils)
                {
                    var sAppareilRecenseFrd = new CsAppareilRecenseFrd
                    {
                        PUISSANCEUNITAIRE = item.PUISSANCE,
                        NOMBRE = item.NOMBRE,
                        FK_IDAPPAREIL = item.PK_ID,
                        FK_IDPRODUIT = ((Galatee.Silverlight.ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID,
                        OBSERVATION=item.DISPLAYLABEL,
                        CODEAPPAREIL= Convert.ToInt32( item.CODEAPPAREIL),
                    };
                    listObjetForInsertOrUpdate.Add(sAppareilRecenseFrd);

                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        //private List<CsDemandeFraude> GetInformationsFromScreenClientFrd()
        //{
        //    var listObjetForInsertOrUpdate = new List<CsClientFraude>();
        //    try
        //    {



        //        var sClientFraude = new CsClientFraude
        //        {
        //            Nomabon = txt_Nom.Text,
        //            Client = txt_refclient.Text,
        //            IdentificationUnique = txt_IdentUnique.Text,
        //            Commune = ((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).LIBELLE,
        //            Quartier = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).LIBELLE,
        //            Rue = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).LIBELLE,
        //            Secteur = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).LIBELLE,
        //            Centre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).LIBELLE,
        //            Email = txt_email.Text,
        //            Telephone = txt_telephone.Text,
        //            Porte = txt_porte.Text,
        //            ContratAbonnement = txt_ContactAbonne.Text,
        //            ContratBranchement = txt_contarBrachement.Text,
        //            DateContratAbonnement = Convert.ToDateTime(DateAbonnemnt.SelectedDate),
        //            DateContratBranchement = Convert.ToDateTime(DateBranchemnt.SelectedDate),



        //        };
        //        listObjetForInsertOrUpdate.Add(sClientFraude);


        //        return listObjetForInsertOrUpdate;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
        //        return null;
        //    }
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Recupere( LaDemande);
            Enregistrer(LaDemande);
        }

        private void Enregistrer( CsDemandeFraude LaDemande)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Langue.Fraude, Langue.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = LaDemande;
                        var service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                        if (listForInsertOrUpdate != null)
                        {
                            service.InsertControleFraudeCompleted += (snder, insertR) =>
                            {
                                if (insertR.Cancelled ||
                                    insertR.Error != null)
                                {
                                    Message.ShowError(insertR.Error.Message, Langue.Fraude);
                                    return;
                                }
                                if (insertR.Result == false)
                                {
                                    Message.ShowError(Langue.ErreurInsertionDonnees, Langue.Fraude);
                                    return;
                                }
                                //OnEvent(null);
                                DialogResult = true;
                            };
                            service.InsertControleFraudeAsync(listForInsertOrUpdate);
                        }

                        else
                        {
                            return;
                        }
                    }

                };
                messageBox.Show();
            }

            catch (Exception ex)
            {
                //Message.Show(ex.Message, Languages.Commune);
            }
        
        
        
        }

        private void Recupere(CsDemandeFraude LaDemande)
        {
          
            LaDemande.Fraude = GetInformationsFromScreenFrd();
            LaDemande.ClientFraude = GetInformationsFromScreenClientFrd();
            LaDemande.Controle = GetInformationsFromScreenControle();
            LaDemande.Controleur = GetInformationsFromScreenControleur();
            LaDemande.CompteurFraude = GetInformationsFromScreenCompteurFraude();
            LaDemande.AppareilRecenseFrd = GetInformationsFromScreenControle(listAppareilsSelectionnes);
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
                    Cbo_Commune.ItemsSource = SessionObject.LstCommune;
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


        #region Liste des Appareils
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
                    txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Nomabon) ? string.Empty : LaDemande.ClientFraude.Nomabon;
                    txt_Nom.Tag = string.IsNullOrEmpty(LaDemande.ClientFraude.PK_ID.ToString()) ? string.Empty : LaDemande.ClientFraude.PK_ID.ToString();
                    txt_refclient.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Client) ? string.Empty : LaDemande.ClientFraude.Client; ;
                    txt_email.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Email) ? string.Empty : LaDemande.ClientFraude.Email; ;
                    txt_telephone.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Telephone) ? string.Empty : LaDemande.ClientFraude.Telephone; ;
                    txt_porte.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Porte) ? string.Empty : LaDemande.ClientFraude.Porte; ;
                    txt_ContactAbonne.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratAbonnement) ? string.Empty : LaDemande.ClientFraude.ContratAbonnement;
                    txt_contarBrachement.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratBranchement) ? string.Empty : LaDemande.ClientFraude.ContratBranchement;
                    txt_Numerotraitement.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                    txt_FichControle.Text = txt_Numerotraitement.Text.ToString() + "- 1";
                    Cbo_Centre.SelectedItem = _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDCENTRE);
                    Cbo_Commune.SelectedItem = _listeDesCommuneExistantCentre.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDCOMMUNE);
                    Cbo_Quartier.SelectedItem = _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_IDQUARTIER);
                    Cbo_Secteur.SelectedItem = SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_SECTEUR);
                    Cbo_Rue.SelectedItem = SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == LaDemande.ClientFraude.FK_RUE);
                    if (LaDemande.Abon != null && LaDemande.Abon.DABONNEMENT != null )
                        DateAbonnemnt.SelectedDate = LaDemande.Abon.DABONNEMENT; 
                    txt_ContactAbonne.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratAbonnement) ? string.Empty : LaDemande.ClientFraude.ContratAbonnement;
                    txt_contarBrachement.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratBranchement) ? string.Empty : LaDemande.ClientFraude.ContratBranchement;
                    if (LaDemande.Canalisation!=null)
                    DateBranchemnt.SelectedDate = LaDemande.Canalisation.POSE;
                      
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeFraudeAsync(pk_id);

        }

        private void Validationdemande(CsDemandeFraude LaDemande)
        {
            try
            {
                FraudeServiceClient Client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude")); ;
                Client.ValiderDemandeControleCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        Message.Show("Affectation effectuée avec succès", "Information");
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeSuivante(Listid);
                        this.DialogResult = true;

                      
                       
                    }
                    else
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                Client.ValiderDemandeControleAsync(LaDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Transmit");
            }
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

        private void BtnTRansfert_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return;
            Recupere(LaDemande);
            Validationdemande( LaDemande);

        }
    }
}

