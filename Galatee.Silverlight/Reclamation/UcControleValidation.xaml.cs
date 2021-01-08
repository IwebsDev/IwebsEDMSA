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
    public partial class UcControleValidation : ChildWindow
    {
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        Galatee.Silverlight.ServiceFraude.CsClient Client = new Galatee.Silverlight.ServiceFraude.CsClient();
        CsDemandeReclamation LaDemande = new CsDemandeReclamation();
        public ObservableCollection<ObjDOCUMENTSCANNE> LstPiece = new ObservableCollection<ObjDOCUMENTSCANNE>();
        bool isPreuveSelectionnee = false;

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

        public UcControleValidation()
        {
            InitializeComponent();
           
        }
        public UcControleValidation(int demande, bool IsConsultation=false)
        {
            InitializeComponent();
            this.IsConsultation = IsConsultation;
            this.demande = demande;
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargeDonneDemande(demande);
            ChargerAppreciation();

        }
        int EtapeActuelle;

        public UcControleValidation(List<int> demandes, int fkIdEtape)
        {
            InitializeComponent();
            this.demande = demandes.First();
            RemplirCommune();
            RemplirListeDesQuartierExistant();
            RemplirSecteur();
            RemplirListeDesRuesExistant();
            ChargerDonneeDuSite();
            ChargeDonneDemande(demandes.First());
            ChargerAppreciation();
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
        private void ChargerAppreciation()
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.SelectAllValidationCompleted += (ssender, args) =>
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
                        Cbo_Apreciation.ItemsSource = null;
                        Cbo_Apreciation.DisplayMemberPath = "Libelle";
                        Cbo_Apreciation.SelectedValuePath = "PK_ID";
                        Cbo_Apreciation.ItemsSource = args.Result;

                        ChargeDonneDemande(this.demande);
                    }
                };
                client.SelectAllValidationAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;
                if (string.IsNullOrEmpty(this.Date_Retour.SelectedDate.ToString()))
                    throw new Exception("Selectionnez Date retour réel");

                if (this.Date_Retour.SelectedDate < DateTime.Now.Date)
                    throw new Exception("La Date retour réel ne doit peut pas être inférieur à la date actuel");

                if (string.IsNullOrEmpty(this.txt_lettrereponse.Text))
                    throw new Exception("Veuillez saisir la lettre de réponse ");

                if (this.Cbo_Apreciation.SelectedItem == null )
                    throw new Exception("Séléctionnez l'appréciation ");
                

                return ReturnValue;

            }
            catch (Exception ex)
            {
               // this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Reclamation");
                return false;
            }

        }


        private void ValidationDemande(CsDemandeReclamation _LaDemande)
        {
            try
            {
                //Lancer la transaction de mise a jour en base
                _LaDemande.LaDemande.DATEFIN = System.DateTime.Today;
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
                    Message.ShowInformation("Opération effectuée avec succès", "Reclamation");
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
                        Txt_AgentTraiteur.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.NOMAGENTRECEPTEUR) ? string.Empty : LaDemande.ReclamationRcl.NOMAGENTRECEPTEUR + " ("+ LaDemande.ReclamationRcl.AgentRecepteur +")";
                        Txt_ActionMennes.Text = string.IsNullOrEmpty(LaDemande.ReclamationRcl.ActionMenees) ? string.Empty : LaDemande.ReclamationRcl.ActionMenees;
                        Dtp_DateTraitement.SelectedDate = LaDemande.ReclamationRcl.DateTransmission == null ? null  : LaDemande.ReclamationRcl.DateTransmission;
                        Date_Retour.SelectedDate = DateTime.Now;
                        this.tbControleClient.SelectedItem = this.tabItemValidation;

                        #region DocumentScanne
                        if (LaDemande.DonneDeDemande != null && LaDemande.DonneDeDemande.Count != 0)
                        {
                            //isPreuveSelectionnee = true;
                            LstPiece.Clear();
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

                        if (this.IsConsultation == true)
                        {
                            //Cbo_Apreciation.SelectedValue =Cbo_Apreciation.ItemsSource!=null?((List<CsRclValidation>)Cbo_Apreciation.ItemsSource).FirstOrDefault(c=>c.PK_ID LaDemande.ReclamationRcl.Fk_IdValidation;
                            Cbo_Apreciation.SelectedValue =LaDemande.ReclamationRcl.Fk_IdValidation;
                            Date_Retour.SelectedDate = LaDemande.ReclamationRcl.DateRetour;
                            check_NonConforme.IsChecked = LaDemande.ReclamationRcl.NonConformite;
                            txt_lettrereponse.Text = LaDemande.ReclamationRcl.LettreReponse;
                            OKButton.Visibility = Visibility.Collapsed;
                            CancelButton.Visibility = Visibility.Collapsed;
                        }
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
        private void dgListePiece_CurrentCellChanged(object sender, EventArgs e)
        {
            dgListePiece.BeginEdit();
        }
        private void Recupere(CsDemandeReclamation LaDemande)
        {
            LaDemande.ReclamationRcl.NonConformite = check_NonConforme.IsChecked;
            LaDemande.ReclamationRcl.DateRetour = Convert.ToDateTime(Date_Retour.SelectedDate);
            LaDemande.ReclamationRcl.DateValidation  = Convert.ToDateTime(Date_Retour.SelectedDate);
            LaDemande.ReclamationRcl.LettreReponse = txt_lettrereponse.Text;
            LaDemande.ReclamationRcl.Fk_IdValidation  =((CsRclValidation) this.Cbo_Apreciation.SelectedItem ).PK_ID ;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return;
            Recupere(LaDemande);
            ValidationDemande(LaDemande);
        }
        private void hyperlinkButtonPropScannee__Click(object sender, RoutedEventArgs e)
        {
            MemoryStream memoryStream = new MemoryStream(((HyperlinkButton)sender).Tag as byte[]);
            var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
            ucImageScanne.Show();
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






        private void EnvoyerDemandeEtapeprecedente(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de validation", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de validation", Galatee.Silverlight.Resources.Accueil.Langue.lbl_Menu);
                    return;
                }
                else
                {
                    Message.ShowInformation("Validation effectuée avec succès", Langue.lbl_Menu);
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.REJETER, UserConnecte.matricule,
                string.Empty);
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


        public Galatee.Silverlight.ServiceAccueil.CsDemande LaDemandeD { get; set; }

        public int demande { get; set; }

        public bool IsConsultation = false;
    }
}

