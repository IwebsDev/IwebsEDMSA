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
using Galatee.Silverlight.Tarification.Helper;
using Galatee.Silverlight.Fraude;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Resources.Fraude;
//using Galatee.Silverlight.ServiceReclamation;
using Galatee.Silverlight.ServiceAccueil;
using System.Collections.ObjectModel;
using System.IO;


namespace Galatee.Silverlight.Accueil
{
    public partial class UcMotifReprise : ChildWindow
    {
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        Galatee.Silverlight.ServiceFraude.CsClient Client = new Galatee.Silverlight.ServiceFraude.CsClient();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();

        CsDemandeReclamation LaDemande = new CsDemandeReclamation();
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        protected virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
        }
        public UcMotifReprise()
        {
            InitializeComponent();
           
        }
        public UcMotifReprise(int demande)
        {
            InitializeComponent();

            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed;

            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargeDonneDemande(demande);

        }
        int EtapeActuelle;

        public UcMotifReprise(List<int> demandes, int fkIdEtape)
        {
            this.demande = demandes.First();
            InitializeComponent();

            this.labMotif.Visibility = System.Windows.Visibility.Collapsed;
            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Collapsed;

            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargeDonneDemande(demandes.First());
            EtapeActuelle = fkIdEtape;

        }
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre.Count != 0)
                {
                    _listeDesCentreExistant = SessionObject.LstCentre;
                    return;
                }
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    _listeDesCentreExistant = SessionObject.LstCentre;
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "ChargerDonneeDuSite");

            }
        }

        private CsReclamationRcl GetInformationsFromScreen(CsReclamationRcl laReclamation)
        {
            var listObjetForInsertOrUpdate = new CsReclamationRcl();
            try
            {
                laReclamation.ActionMenees = Txt_ActionMennes.Text;
                laReclamation.DateTransmission = Dtp_DateTraitement.SelectedDate.Value;
                laReclamation.AgentRecepteur  = UserConnecte.matricule;
                return laReclamation;
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
                #region Adresse géographique

                if (string.IsNullOrEmpty(this.Txt_ActionMennes.Text))
                    throw new Exception("Entrer l'action ménée ");
                #endregion

                return ReturnValue;

            }
            catch (Exception ex)
            {
               // this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Fraude");
                return false;
            }

        }

        //private void ValidationDemande(CsDemandeReclamation _LaDemande)
        //{
        //    try
        //    {

        //        _LaDemande.LaDemande = new Galatee.Silverlight.ServiceReclamation.CsDemandeBase();
        //        Galatee.Silverlight.ServiceAccueil.CsTdem leTydemande = new Galatee.Silverlight.ServiceAccueil.CsTdem();
        //        leTydemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DemandeFraude);

        //        _LaDemande.LaDemande.TYPEDEMANDE = (string)leTydemande.CODE;
        //        _LaDemande.LaDemande.FK_IDTYPEDEMANDE = (int)leTydemande.PK_ID;
        //        _LaDemande.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
        //        _LaDemande.LaDemande.CENTRE = UserConnecte.Centre;
        //        _LaDemande.LaDemande.FK_IDCENTRE = UserConnecte.FK_IDCENTRE;
        //        _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
        //        _LaDemande.LaDemande.DATECREATION = DateTime.Now;

        //        //Lancer la transaction de mise a jour en base
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        service1.ValiderInitReclamationCompleted += (sr, res) =>
        //        {
        //            if (res != null && res.Cancelled)
        //                return;
        //            if (!string.IsNullOrEmpty(res.Result))
        //            {
        //                string Retour = res.Result;
        //                string[] coupe = Retour.Split('.');
        //                Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
        //            }
        //            //if (Closed != null)
        //            //    Closed(this, new EventArgs());
        //            this.DialogResult = false;
        //        };
        //        service1.ValiderInitReclamationAsync(_LaDemande);
        //        service1.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }


        //}

        private void ValidationDemande(CsDemandeReclamation _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderReclamationCompleted += (sr, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeSuivante(Listid);
                    }
                    else
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                service1.ValiderReclamationAsync(_LaDemande);
                service1.CloseAsync();

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
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec ", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    Message.ShowInformation("Demande transmise avec succès", "Reclamation");
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }

        private void ChargeDonneDemande(int pk_id)
        {
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service.RetourDemandeReclamationCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;

                    if (LaDemande != null)
                    {
                        Txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NomClient) ? string.Empty : LaDemande.ReclamationRcl.NomClient;
                        Txt_Portable.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroTelephonePortable) ? string.Empty : LaDemande.ReclamationRcl.NumeroTelephonePortable;
                        Txt_DateOuverture .Text = LaDemande.ReclamationRcl.DateOuverture== null  ? string.Empty : LaDemande.ReclamationRcl.DateOuverture.Value.ToShortDateString ();
                        Txt_NumeroFixe.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroTelephoneFixe) ? string.Empty : LaDemande.ReclamationRcl.NumeroTelephoneFixe;
                        Txt_TypeReclamation.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.LIBELLETYPERECLAMATION) ? string.Empty : LaDemande.ReclamationRcl.LIBELLETYPERECLAMATION;
                        Txt_Adress.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Adresse) ? string.Empty : LaDemande.ReclamationRcl.Adresse;
                        Txt_Email.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Email) ? string.Empty : LaDemande.ReclamationRcl.Email;
                        Txt_Object.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.ObjetReclamation) ? string.Empty : LaDemande.ReclamationRcl.ObjetReclamation;
                        Txt_Observation.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Observation) ? string.Empty : LaDemande.ReclamationRcl.Observation;
                        Txt_Client.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.Client) ? string.Empty : LaDemande.ReclamationRcl.Client;
                        Txt_DateRendezVous.Text = LaDemande.ReclamationRcl.DateRdv == null ? string.Empty : LaDemande.ReclamationRcl.DateRdv.Value.ToShortDateString();
                        Txt_DateRetourSouhaiter.Text = LaDemande.ReclamationRcl.DateRetourSouhaite == null ? string.Empty : LaDemande.ReclamationRcl.DateRetourSouhaite.Value.ToShortDateString();
                        Dtp_DateTraitement.SelectedDate = System.DateTime.Today;
                        Txt_NumDemande.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NumeroReclamation) ? string.Empty : LaDemande.ReclamationRcl.NumeroReclamation;
                        Txt_EtablirPar.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NOMAGENTCREATION) ? string.Empty : LaDemande.ReclamationRcl.NOMAGENTCREATION + " (" + LaDemande.ReclamationRcl.AgentEmetteur + ")";
                        Txt_AgentTraiteur.Text = UserConnecte.nomUtilisateur +" (" + UserConnecte.matricule + ")";

                        this.tbControleClient.SelectedItem = this.tabItemtraitement;

                        Txt_MotifRejet.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.MotifRejet) ? string.Empty : LaDemande.ReclamationRcl.MotifRejet;

                        if (!string.IsNullOrEmpty(LaDemande.ReclamationRcl.MotifRejet))
                        {
                            this.labMotif.Visibility = System.Windows.Visibility.Visible;
                            this.Txt_MotifRejet.Visibility = System.Windows.Visibility.Visible;
                        }



                        #region DocumentScanne
                        if (LaDemande.DonneDeDemande != null && LaDemande.DonneDeDemande.Count != 0)
                        {
                            //isPreuveSelectionnee = true;
                            foreach (var item in LaDemande.DonneDeDemande)
                            {
                                LstPiece.Add(item);
                            }
                            dgListePiece.ItemsSource = this.LstPiece;
                        }
                        //else
                        //{
                        //    isPreuveSelectionnee = false;
                        //}
                        #endregion
                        //txtCentre.Text   = _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.Fk_IdCentre)!= null ?
                        //    _listeDesCentreExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.Fk_IdCentre).LIBELLE :string.Empty ;

                        //txt_Commune.Text = _listeDesCommuneExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDCOMMUNE) != null ?
                        //    _listeDesCommuneExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDCOMMUNE).LIBELLE : string.Empty ;

                        //txt_Quartier.Text = _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDQUARTIER) != null ?
                        //    _listeDesQuartierExistant.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDQUARTIER).LIBELLE : string.Empty;

                        //txt_NumSecteur.Text = SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDSECTEUR) != null ?
                        //    SessionObject.LstSecteur.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDSECTEUR).LIBELLE : string.Empty;

                        //txt_NumRue.Text =SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDRUE)!= null ?
                        //    SessionObject.LstRues.FirstOrDefault(t => t.PK_ID == LaDemande.ReclamationRcl.FK_IDRUE).LIBELLE : string.Empty ;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeReclamationAsync(pk_id);

        }
        private void Recupere(CsDemandeReclamation LaDemande)
        {
            LaDemande.ReclamationRcl = GetInformationsFromScreen(LaDemande.ReclamationRcl );
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return;
            Recupere(LaDemande);
            ValidationDemande(LaDemande);
        }
        private void EnvoyerDemandeEtapeprecedente(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Demande transmise avec succès", Langue.lbl_Menu);
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.REJETER, UserConnecte.matricule,
                string.Empty);
        }





        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceAccueil.CsDemandeBase dem = new ServiceAccueil.CsDemandeBase();
            ServiceAccueil.CsDemande laDemandeD = new ServiceAccueil.CsDemande();
            laDemandeD.InfoDemande = new CsInfoDemandeWorkflow();
            dem.NUMDEM = Txt_NumDemande.Text;
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneInfoDemandeWkfCompleted += (sr, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, "Rejet");
                        return;
                    }
                    if (b.Result != null)
                    {
                        laDemandeD.InfoDemande = b.Result;

                        Galatee.Silverlight.Workflow.UcWKFMotifRejet ucMotif = new Galatee.Silverlight.Workflow.UcWKFMotifRejet(laDemandeD.InfoDemande);
                        ucMotif.Closed += new EventHandler(ucMotif_OK);
                        ucMotif.Show();
                    }
                    else
                        Message.ShowError("Erreur à la mise à jour de la demande", "Rejet");
                };
                service1.RetourneInfoDemandeWkfAsync(dem);
                service1.CloseAsync();


            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, "Erreur");
                return;
            }
        }



        private void ucMotif_OK(object sender, EventArgs e)
        {
            try
            {
                Galatee.Silverlight.Workflow.UcWKFMotifRejet ctrs = sender as Galatee.Silverlight.Workflow.UcWKFMotifRejet;
                if (ctrs.DialogResult.Value)
                {
                    Recupere(LaDemande);
                    LaDemande.ReclamationRcl.MotifRejet = ctrs.txtMotif.Text;
                    MiseAJourRejet(LaDemande);
                    this.DialogResult = false;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private void MiseAJourRejet(CsDemandeReclamation _LaDemande)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ValiderReclamationCompleted += (sr, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, "Rejet");
                        return;
                    }
                    if (b.Result != null)
                    {
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeprecedente(Listid);
                    }
                    else
                        Message.ShowError("Erreur à la mise à jour de la demande", "Rejet");
                };
                service1.ValiderReclamationAsync(_LaDemande);
                service1.CloseAsync();

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Erreur");
            }
        }








        //private void ChargerModeReception()
        //{
        //    try
        //    {
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        client.SelectAllModeReceptionCompleted += (ssender, args) =>
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
        //                Cbo_ModeReception.ItemsSource = null;
        //                Cbo_ModeReception.DisplayMemberPath = "Libelle";
        //                Cbo_ModeReception.SelectedValuePath = "PK_ID";
        //                Cbo_ModeReception.ItemsSource = args.Result;


        //            }
        //        };
        //        client.SelectAllModeReceptionAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //private void ChargerTypeReclamation()
        //{
        //    try
        //    {
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        client.SelectAllTypeReclamationRclCompleted += (ssender, args) =>
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
        //                Cbo_TypeReclamation.ItemsSource = null;
        //                Cbo_TypeReclamation.DisplayMemberPath = "Libelle";
        //                Cbo_TypeReclamation.SelectedValuePath = "PK_ID";
        //                Cbo_TypeReclamation.ItemsSource = args.Result;


        //            }
        //        };
        //        client.SelectAllTypeReclamationRclAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        #region situation geaographique
        private void RemplirCommune()
        {
            try
            {
                if (SessionObject.LstCommune != null && SessionObject.LstCommune.Count != 0)
                {
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
                    _listeDesCommuneExistant = SessionObject.LstCommune;

                };
                service.CloseAsync();
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
                };
                service.CloseAsync();
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
        #endregion

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Galatee.Silverlight.Fraude.UcRechercheClient Newfrm = new Galatee.Silverlight.Fraude.UcRechercheClient(); ;
                Newfrm.CallBack += Newfrm_CallBack;
                Newfrm.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void Newfrm_CallBack(object sender, CustumEventArgs e)
        {
             Client = e.Data as Galatee.Silverlight.ServiceFraude.CsClient;
             Txt_Nom.Text = Client.NOMABON;
             Txt_Nom.Tag = Client.ORDRE;
             Txt_Portable.Text = Client.TELEPHONE;
             Txt_NumeroFixe.Text = Client.TELEPHONE;
             Txt_Adress.Text = Client.ADRMAND1;
            //Clientfraude.Nomabon = Client.NOMABON;
            //Clientfraude.Client = Client.REFCLIENT;
            //Clientfraude.Centre = Client.CENTRE;
            //Clientfraude.FK_IDCENTRE = (int)Client.FK_IDCENTRE;
            //Clientfraude.Telephone = Client.TELEPHONE;
            //Clientfraude.Porte = Client.PORTE;
            //Clientfraude.Ordre = Client.ORDRE;
            //LaDemande.ClientFraude = Clientfraude;

        }

        private void Chk_EstClient_Checked(object sender, RoutedEventArgs e)
        {
            //if (Chk_EstClient.IsChecked == true)
            // Btn_RechercherClient.IsEnabled = true;
        }


        public int demande { get; set; }
    }
}

