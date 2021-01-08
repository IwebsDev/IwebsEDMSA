using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.ServiceScelles;
using Galatee.Silverlight.Resources.Scelles;
using Galatee.Silverlight.Tarification.Helper;

namespace Galatee.Silverlight.Scelles
{
    public partial class UcScelleCompteurBta1 : ChildWindow
    {
        public CsRechercheCompteur lecompteur;
        List<CsMarque_Modele> ListdesModelesfonctMarq = new List<CsMarque_Modele>();
        List<CsRemiseScelleByAg> ListdesScelles = new List<CsRemiseScelleByAg>();
        private CsCompteurBta ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache, ID_Diametre;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsCompteurBta> listForInsertOrUpdate = new List<CsCompteurBta>();
        public UcScelleCompteurBta1()
        {
            InitializeComponent();
            RemplirListeCmbDesEtatCompteursExistant();
            RemplirListeCmbDesModelesMarqueExistant();
            ChargerDiametreCompteur();
            ChargerProduit();
            ChargerTypeCompteur();
            ListeScelleExistant();
            //ChargerListDesSite();
            ModeExecution = SessionObject.ExecMode.Creation;
        }
        public UcScelleCompteurBta1(CsCompteurBta pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var CompteurBt = new CsCompteurBta();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(CompteurBt, pObject as CsCompteurBta);
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                // dtgrdlReceptionScelle.ItemsSource = donnesDatagrid;
                RemplirListeCmbDesEtatCompteursExistant();
                RemplirListeCmbDesModelesMarqueExistant();
                ChargerDiametreCompteur();
                ChargerProduit();
                ChargerTypeCompteur();
                ListeScelleExistant();
                //ChargerListDesSite();
                //if (dataGrid != null) 
                //    donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsLotMagasinGeneral>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                      
                        Cbo_Diametre.SelectedItem = SessionObject.LstCalibreCompteur .FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDCALIBRECOMPTEUR );
                        Cbo_typeCmpt.SelectedItem = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDTYPECOMPTEUR);
                        List<CsRefEtatCompteur> lstetat = (List<CsRefEtatCompteur>)this.Cbo_Etat_cmpt.ItemsSource;
                        if (lstetat != null)
                            Cbo_Etat_cmpt.SelectedItem = lstetat.FirstOrDefault(t => t.EtatCompteur_ID == ObjetSelectionnee.EtatCompteur_ID);
                        //List<ServiceAccueil.CsCentre> lsCentre = (List<ServiceAccueil.CsCentre>)this.Cbo_Centre.ItemsSource;
                        ////if (lsCentre != null)
                        ////    Cbo_Centre.SelectedItem = lsCentre.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.CodeCentre);
                      
                        List<CsMarque_Modele> lstMaqmMdt = ListdesModelesfonctMarq;
                        if (lstMaqmMdt != null)
                        {
                            Cbo_Marque.SelectedItem =ObjetSelectionnee.LIBELLEMARQUE ;
                            Cbo_Modele.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MARQUE_ID == ObjetSelectionnee.FK_IDMARQUECOMPTEUR);
                        }
                        txt_ANNEEFAB.Text = ObjetSelectionnee.ANNEEFAB;
                        txt_NumCpteur.Text = ObjetSelectionnee.Numero_Compteur;
                        //if (ObjetSelectionnee.FONCTIONNEMENT == "1")
                        //    Chck_Fcnt.IsChecked = true;
                        //if (ObjetSelectionnee.CADRAN == 1)
                        //    Chck_Cadran.IsChecked = true;
                        if (ObjetSelectionnee.CapotMoteur_ID_Scelle1 != null)
                        {
                            txt_NumNouveauScelle_1.Text = ObjetSelectionnee.Numero_ScelleCapot_1.ToString();
                            rbt_NouveauScelle_1.IsChecked = true;
                        }
                        else
                            rbt_AuneAction_1.IsChecked = true;

                        if (ObjetSelectionnee.CapotMoteur_ID_Scelle2 != null)
                        {
                            txt_NumNouveauScelle_2.Text = ObjetSelectionnee.Numero_ScelleCapot_3.ToString();
                            rbt_NouveauScelle_2.IsChecked = true;
                        }
                        else
                            rbt_AuneAction_2.IsChecked = true;
                        if (ObjetSelectionnee.CapotMoteur_ID_Scelle3 != null)
                        {
                            txt_NumNouveauScelle_3.Text = ObjetSelectionnee.Numero_ScelleCapot_3.ToString();
                            rbt_NouveauScelle_3.IsChecked = true;
                        }
                        else
                            rbt_AuneAction_3.IsChecked = true;

                        if (ObjetSelectionnee.Cache_Scelle != null)
                        {
                            txt_NumNouveauScelle_Cache.Text = ObjetSelectionnee.Numero_Cache_3.ToString();
                            rbt_NouveauScelle_Cache.IsChecked = true;
                        }
                        else
                            rbt_AuneAction_Cache.IsChecked = true;
                       // DateCreation.D = ObjetSelectionnee.DATECREATION;
                        //Txt_Libelle.Text = ObjetSelectionnee.LIBELLE;
                        //btnOk.IsEnabled = false;
                        ////Txt_Code.IsReadOnly = true;
                    }
                }
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    AllInOne.ActivateControlsFromXaml(LayoutRoot, false);
                }
                //VerifierSaisie();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.Commune);
            }
        }

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
        private void Translate()
        {
            try
            {
                Title = Languages.Scelles;
                //btnOk.Content = Languages.OK;
                //Btn_Reinitialiser.Content = Languages.Annuler;
                //GboCodeDepart.Header = Languages.InformationsCodePoste;
                //lab_Code.Content = Languages.Code;
                //lab_Libelle.Content = Languages.Libelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
        //{
        //    try
        //    {
        //        Cbo_Centre.Items.Clear();
        //        if (_listeDesCentreExistant != null &&
        //            _listeDesCentreExistant.Count != 0)
        //        {
        //            List<ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
        //            if (lesCentreDuPerimetreAction != null)
        //                foreach (var item in lesCentreDuPerimetreAction)
        //                {
        //                    Cbo_Centre.Items.Add(item);
        //                }
        //            //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
        //            //Cbo_Centre.SelectedValuePath = "PK_ID";
        //            //Cbo_Centre.DisplayMemberPath = "LIBELLE";

        //            //if (pIdcentre != 0)
        //            //    this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
        //            //if (_listeDesCentreExistant.Count == 1)
        //            //    this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private void RemplirCentrePerimetre(List<ServiceAccueil.CsCentre> lstCentre, List<ServiceAccueil.CsSite> lstSite)
        //{
        //    try
        //    {
        //        Cbo_Centre.Items.Clear();
        //        if (_listeDesCentreExistant != null &&
        //            _listeDesCentreExistant.Count != 0)
        //        {
        //            if (lstCentre != null)
        //                foreach (var item in lstCentre)
        //                {
        //                    Cbo_Centre.Items.Add(item);
        //                }
        //            Cbo_Centre.SelectedValuePath = "PK_ID";
        //            Cbo_Centre.DisplayMemberPath = "LIBELLE";

        //            if (lstSite != null)
        //                foreach (var item in lstSite)
        //                {
        //                    Cbo_Site.Items.Add(item);
        //                }
        //            Cbo_Site.SelectedValuePath = "PK_ID";
        //            Cbo_Site.DisplayMemberPath = "LIBELLE";

        //            if (lstSite != null && lstSite.Count == 1)
        //                Cbo_Site.SelectedItem = lstSite.First();
                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        //List<ServiceAccueil.CsSite> lesSite = new List<ServiceAccueil.CsSite>();
        //void ChargerListDesSite()
        //{
        //    try
        //    {

        //        if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
        //        {
        //            lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
        //            lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
        //            _listeDesCentreExistant = lesCentre;
        //            RemplirCentrePerimetre(lesCentre, lesSite);
        //            return;
        //        }
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
        //        service.ListeDesDonneesDesSiteAsync(false);
        //        service.ListeDesDonneesDesSiteCompleted += (s, args) =>
        //        {
        //            try
        //            {
        //                if (args != null && args.Cancelled)
        //                    return;
        //                SessionObject.LstCentre = args.Result;
        //                if (SessionObject.LstCentre.Count != 0)
        //                {
        //                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
        //                    lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
        //                    _listeDesCentreExistant = lesCentre;
        //                    RemplirCentrePerimetre(lesCentre, lesSite);
        //                }
        //                else
        //                {
        //                    Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Message.ShowError(ex, "Erreur");
        //            }
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex, "Erreur");
        //    }
        //}

        private void RemplirListeCmbDesModelesMarqueExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneListMarque_ModeleCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        ListdesModelesfonctMarq = args.Result;

                    }
                };
                client.RetourneListMarque_ModeleAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RemplirListeCmbDesEtatCompteursExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneEtatCompteurCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {

                        this.Cbo_Etat_cmpt.ItemsSource = args.Result;
                        this.Cbo_Etat_cmpt.DisplayMemberPath = "Libelle_ETAT";
                        this.Cbo_Etat_cmpt.SelectedValuePath = "EtatCompteur_ID";

                        if (ObjetSelectionnee != null)
                        {
                            foreach (CsRefEtatCompteur marqModl in Cbo_Etat_cmpt.ItemsSource)
                            {
                                if (marqModl.EtatCompteur_ID != null)
                                {
                                    Cbo_Etat_cmpt.SelectedItem = marqModl;
                                    break;
                                }
                            }
                        }
                    }
                };
                client.RetourneEtatCompteurAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ListeScelleExistant()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SCELLES_RETOURNE_Pour_ScellageCptCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Languages.Quartier);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Languages.msgErreurChargementDonnees, Languages.Scelles);
                        return;
                    }
                    else
                    {
                        ListdesScelles = args.Result;
                    }
                };
                client.SCELLES_RETOURNE_Pour_ScellageCptAsync(UserConnecte.PK_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChargerDiametreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur != null && SessionObject.LstCalibreCompteur.Count != 0)
                {
                    Cbo_Diametre.ItemsSource = null;
                    Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_Diametre.ItemsSource = SessionObject.LstCalibreCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                    Cbo_Diametre.ItemsSource = null;
                    Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                    Cbo_Diametre.ItemsSource = SessionObject.LstCalibreCompteur;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                {
                    Cbo_Produit.ItemsSource = null;
                    Cbo_Produit.DisplayMemberPath = "LIBELLE";
                    Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                    Cbo_Produit.ItemsSource = null;
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

        private void ChargerTypeCompteur()
        {
            try
            {
                if (SessionObject.LstTypeCompteur != null && SessionObject.LstTypeCompteur.Count != 0)
                {
                    Cbo_typeCmpt.ItemsSource = null;
                    Cbo_typeCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_typeCmpt.ItemsSource = SessionObject.LstTypeCompteur;

                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                service.ChargerTypeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstTypeCompteur = args.Result;
                    Cbo_typeCmpt.ItemsSource = null;
                    Cbo_typeCmpt.DisplayMemberPath = "LIBELLE";
                    Cbo_typeCmpt.ItemsSource = SessionObject.LstTypeCompteur;
                };
                service.ChargerTypeAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void charqueModel(int MArque_id)
        {
            try
            {
                this.Cbo_Modele.ItemsSource = ListdesModelesfonctMarq.Where(c => c.MARQUE_ID == MArque_id).ToList();
                this.Cbo_Modele.DisplayMemberPath = "Libelle_Modele";
                this.Cbo_Modele.SelectedValuePath = "MODELE_ID";
                //foreach (CsMarque_Modele marqModl in Cbo_Modele.ItemsSource)
                //      {
                //          if (marqModl.MODELE_ID != null)
                //          {
                //              Cbo_Modele.SelectedItem = marqModl;

                //          }

                //      }
                //- Aucune sélection par défaut.
                this.Cbo_Modele.SelectedValue = string.Empty;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ChargerDonneByProduit(int ID_Produit)
        {
            try
            {
                this.Cbo_Marque.ItemsSource = ListdesModelesfonctMarq.Where(c => c.Produit_ID == ID_Produit).ToList();
                this.Cbo_Marque.DisplayMemberPath = "Libelle_MArque";
                this.Cbo_Marque.SelectedValuePath = "MARQUE_ID";
                this.Cbo_Marque.SelectedValue = string.Empty;

                this.Cbo_typeCmpt.ItemsSource = SessionObject.LstTypeCompteur.Where(c => c.FK_IDPRODUIT  == ID_Produit).ToList();
                this.Cbo_typeCmpt.DisplayMemberPath = "LIBELLE" ;
                this.Cbo_typeCmpt.SelectedValuePath = "PK_ID";
                this.Cbo_typeCmpt.SelectedValue = string.Empty;

       

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private List<CsCompteurBta> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsCompteurBta>();
            try
            {

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                  
                        var tCompteur = new CsCompteurBta();

                        tCompteur.Numero_Compteur = txt_NumCpteur.Text;
                        tCompteur.EtatCompteur_ID = ((CsRefEtatCompteur)Cbo_Etat_cmpt.SelectedItem).EtatCompteur_ID;
                        tCompteur.ANNEEFAB = txt_ANNEEFAB.Text;
                        tCompteur.FONCTIONNEMENT = "1";
                        //TYPE_COMPTEUR  = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).CODE ,
                        tCompteur.CADRAN = Convert.ToByte(this.txt_Cadran .Text );
                        tCompteur.FK_IDCALIBRECOMPTEUR = null;
                        if ( Cbo_Diametre.SelectedItem != null)
                            tCompteur.FK_IDCALIBRECOMPTEUR = ((ServiceAccueil.CsCalibreCompteur)Cbo_Diametre.SelectedItem).PK_ID  ;
                        tCompteur.MARQUE = ((CsMarque_Modele)Cbo_Marque.SelectedItem).CODE_Marque;
                        tCompteur.FK_IDMARQUECOMPTEUR = ((CsMarque_Modele)Cbo_Marque.SelectedItem).MARQUE_ID;
                        tCompteur.FK_IDTYPECOMPTEUR = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).PK_ID;
                        tCompteur.CapotMoteur_ID_Scelle1 = (Guid?)txt_NumNouveauScelle_1.Tag;
                        tCompteur.CapotMoteur_ID_Scelle2 = (Guid?)txt_NumNouveauScelle_2.Tag;
                        tCompteur.CapotMoteur_ID_Scelle3 = (Guid?)txt_NumNouveauScelle_3.Tag;
                        tCompteur.Cache_Scelle = (Guid?)txt_NumNouveauScelle_3.Tag;
                        tCompteur.DATECREATION = DateTime.Now;
                        tCompteur.USERCREATION = UserConnecte.matricule;
                        tCompteur.ETAT = "Non_Affecté";
                        tCompteur.StatutCompteur="Non_Affecté";
                        tCompteur.CODEPRODUIT = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).CODE;
                        tCompteur.FK_IDPRODUIT = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                        tCompteur.NUMERO = txt_NumCpteur.Text;
                        listObjetForInsertOrUpdate.Add(tCompteur);
                    
                }

                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                {
                    

                        ObjetSelectionnee.Numero_Compteur = txt_NumCpteur.Text;
                        ObjetSelectionnee.EtatCompteur_ID = ((CsRefEtatCompteur)Cbo_Etat_cmpt.SelectedItem).EtatCompteur_ID;
                        ObjetSelectionnee.ANNEEFAB = txt_ANNEEFAB.Text;
                        ObjetSelectionnee.FONCTIONNEMENT = "1";
                        ObjetSelectionnee.Type_Compteur = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).CODE ;
                        ObjetSelectionnee.CADRAN = Convert.ToByte(this.txt_Cadran.Text );
                        ObjetSelectionnee.FK_IDCALIBRECOMPTEUR  = ((ServiceAccueil.CsCalibreCompteur)Cbo_Diametre.SelectedItem).PK_ID;
                        ObjetSelectionnee.FK_IDMARQUECOMPTEUR = ((CsMarque_Modele)Cbo_Marque.SelectedItem).MARQUE_ID;
                        ObjetSelectionnee.FK_IDTYPECOMPTEUR = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).PK_ID;
                        ObjetSelectionnee.CapotMoteur_ID_Scelle1 = (Guid?)txt_NumNouveauScelle_1.Tag;
                        ObjetSelectionnee.CapotMoteur_ID_Scelle2 = (Guid?)txt_NumNouveauScelle_2.Tag;
                        ObjetSelectionnee.CapotMoteur_ID_Scelle3 = (Guid?)txt_NumNouveauScelle_3.Tag;
                        ObjetSelectionnee.Cache_Scelle = (Guid?)txt_NumNouveauScelle_3.Tag;
                        ObjetSelectionnee.DATEMODIFICATION = DateTime.Now;
                        ObjetSelectionnee.USERMODIFICATION = UserConnecte.matricule;
                        listObjetForInsertOrUpdate.Add(ObjetSelectionnee);
                    
                }
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Languages.EtatDuCompteur);
                return null;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Languages.Commune, Languages.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {

                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                            {
                                service.InsertCompteurCompleted += (snder, insertR) =>
                                {
                                    if (insertR.Cancelled ||
                                        insertR.Error != null)
                                    {
                                        Message.ShowError(insertR.Error.Message, Languages.EtatDuCompteur);
                                        return;
                                    }
                                    if (insertR.Result!=1)
                                    {
                                        Message.ShowError(Languages.ErreurInsertionDonnees, Languages.EtatDuCompteur);
                                        return;
                                    }
                                    ReinitControle();
                                    //OnEvent(null);
                                    //DialogResult = true;
                                };
                                service.InsertCompteurAsync(listForInsertOrUpdate);
                               }
                           
                            if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification)
                            {
                                service.UpdateCompteurCompleted += (snder, UpdateR) =>
                                {
                                    if (UpdateR.Cancelled ||
                                        UpdateR.Error != null)
                                    {
                                        Message.ShowError(UpdateR.Error.Message, Languages.EtatDuCompteur);
                                        return;
                                    }
                                    if (UpdateR.Result==0)
                                    {
                                        Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.EtatDuCompteur);
                                        return;
                                    }
                                    ReinitControle();
                                    //OnEvent(null);
                                    //DialogResult = true;
                                };
                                service.UpdateCompteurAsync(listForInsertOrUpdate);
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Commune);
            }
        }

        private void ReinitControle()
        {
            this.txt_NumCpteur.Text = string.Empty;
            this.txt_Cadran.Text = string.Empty;
            this.txt_NumNouveauScelle_1.Text = string.Empty;
            this.txt_NumNouveauScelle_2.Text = string.Empty;
            this.txt_NumNouveauScelle_3 .Text = string.Empty;
            this.txt_NumNouveauScelle_Cache .Text = string.Empty;
            this.Cbo_Diametre.SelectedItem = null; ;
            this.Cbo_Produit.SelectedItem = null; ;
            this.Cbo_Modele.SelectedItem = null; ;
            this.Cbo_Marque .SelectedItem = null; ;
            this.Cbo_Etat_cmpt .SelectedItem = null; ;
        }
        
        
        public void GriserReftoRadio_ScelleGauche()
        {
            txt_NumScelleRompu_1.IsEnabled = false;
            rbt_AuneAction_1.IsEnabled = false;
            rbt_NouveauScelle_1.IsEnabled = false;
            rbt_RuptureSimple_1.IsEnabled = false;
            txt_NumNouveauScelle_1.IsEnabled = false;

        }
        public void GriserReftoRadio_ScelleMilieu()
        {
            txt_NumScelleRompu_2.IsEnabled = false;
            rbt_AuneAction_2.IsEnabled = false;
            rbt_NouveauScelle_2.IsEnabled = false;
            rbt_RuptureSimple_2.IsEnabled = false;
            txt_NumNouveauScelle_2.IsEnabled = false;
        }
        public void GriserReftoRadio_ScelleDroite()
        {
            txt_NumScelleRompu_3.IsEnabled = false;
            rbt_AuneAction_3.IsEnabled = false;
            rbt_NouveauScelle_3.IsEnabled = false;
            rbt_RuptureSimple_3.IsEnabled = false;
            txt_NumNouveauScelle_3.IsEnabled = false;
        }
        public void ActiverReftoRadio_ScelleGauche()
        {
            txt_NumScelleRompu_1.IsEnabled = true;
            rbt_AuneAction_1.IsEnabled = true;
            rbt_NouveauScelle_1.IsEnabled = true;
            rbt_RuptureSimple_1.IsEnabled = true;
            txt_NumNouveauScelle_1.IsEnabled = true;

        }
        public void ActiverReftoRadio_ScelleMilieu()
        {
            txt_NumScelleRompu_2.IsEnabled = true;
            rbt_AuneAction_2.IsEnabled = true;
            rbt_NouveauScelle_2.IsEnabled = true;
            rbt_RuptureSimple_2.IsEnabled = true;
            txt_NumNouveauScelle_2.IsEnabled = true;
        }
        public void ActiverReftoRadio_ScelleDroite()
        {
            txt_NumScelleRompu_3.IsEnabled = true;
            rbt_AuneAction_3.IsEnabled = true;
            rbt_NouveauScelle_3.IsEnabled = true;
            rbt_RuptureSimple_3.IsEnabled = true;
            txt_NumNouveauScelle_3.IsEnabled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }

        private void Cbo_Produit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cbo_Produit.SelectedValue != null)
                {
                    int ID_produi = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID;
                    //if (((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).CODE == SessionObject.Enumere.ElectriciteMT)
                    //{
                    //    //this.Cbo_Diametre.IsEnabled = false;
                    //    if (this.Cbo_typeCmpt.ItemsSource != null)
                    //        this.Cbo_typeCmpt.SelectedItem = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == "3"); ;
                    //    this.Cbo_typeCmpt.IsEnabled = false;
                    //    this.Cbo_Diametre.Visibility = System.Windows.Visibility.Collapsed;
                    //}
                    //else
                    //{
                    //    this.Cbo_typeCmpt.IsEnabled = true;
                    //    this.Cbo_Diametre.IsEnabled = true;
                    //    this.Cbo_Diametre.Visibility = System.Windows.Visibility.Visible ;

                    //}
                    ChargerDonneByProduit(ID_produi);
                }
            }
            catch (Exception ex)
            {

              Message.ShowError("Erreur au chargement des produits","Scelle");
            }
        }
        private void Cbo_Marque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (Cbo_Marque.SelectedItem == null)
                    return;
                if (Cbo_Marque.SelectedValue != null && Cbo_Produit.SelectedValue != null)
                {
                     ID_MArque = ((CsMarque_Modele)Cbo_Marque.SelectedItem).MARQUE_ID;
                    
                    charqueModel(ID_MArque);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Cbo_Modele_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (Cbo_Modele.SelectedValue != null)
                {
                    int ID_MOdele = ((CsMarque_Modele)Cbo_Modele.SelectedItem).MODELE_ID;
                    foreach (CsMarque_Modele allTS in ListdesModelesfonctMarq.Where(c => c.MODELE_ID == ID_MOdele && c.MARQUE_ID==   ID_MArque).ToList())
                    {
                        nbr_capot = (int)allTS.Nbre_scel_capot;
                        nb_cache = (int)allTS.Nbre_scel_cache;

                    }
                    DesactiverScelleMilieuEnfonctionDuTypeCompteur(nbr_capot);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

  

        public void DesactiverScelleMilieuEnfonctionDuTypeCompteur(int leTypeCompteur)
        {
            try
            {
                //- Type Monophasé ==> Désactiver la saisie du scellé 'Milieu'
                //- Type Triphasé ==> activer la saisie du scellé 'Milieu'
                if (leTypeCompteur == 1)
                {
                    GriserReftoRadio_ScelleMilieu();
                    GriserReftoRadio_ScelleDroite();
                    ActiverReftoRadio_ScelleGauche();
                }
                if (leTypeCompteur ==2)
                {

                    ActiverReftoRadio_ScelleGauche();
                    ActiverReftoRadio_ScelleDroite();
                    GriserReftoRadio_ScelleMilieu();
                }
                if (leTypeCompteur == 3)
                {
                    ActiverReftoRadio_ScelleGauche();
                    ActiverReftoRadio_ScelleDroite();
                    ActiverReftoRadio_ScelleMilieu();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        void galatee_OkClickedbtn_SearchScelle_1(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRemiseScelleByAg Scelle = (CsRemiseScelleByAg)ctrs.MyObject;
                this.txt_NumNouveauScelle_1.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle_1.Tag = Scelle.Id_Scelle;

            }

        }
        void galatee_OkClickedbtn_SearchScelle_2(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRemiseScelleByAg Scelle = (CsRemiseScelleByAg)ctrs.MyObject;
                this.txt_NumNouveauScelle_2.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle_2.Tag = Scelle.Id_Scelle;

            }

        }
        void galatee_OkClickedbtn_SearchScelle_3(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRemiseScelleByAg Scelle = (CsRemiseScelleByAg)ctrs.MyObject;
                this.txt_NumNouveauScelle_3.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle_3.Tag = Scelle.Id_Scelle;

            }

        }

        void galatee_OkClickedbtn_SearchScelle_Ca(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsRemiseScelleByAg Scelle = (CsRemiseScelleByAg)ctrs.MyObject;
                this.txt_NumNouveauScelle_Cache.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle_Cache.Tag = Scelle.Id_Scelle;

            }

        }



        private void btn_SearchScelle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListdesScelles.Count > 0)
                {
                    var ListeScelleValide = ListdesScelles.Where(s => s.Status_ID == SessionObject.Enumere.StatusScelleRemis);
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_1.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_1.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_2.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_2.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_3.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_2.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_Cache.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_Cache.Text).ToList();
                    if (ListeScelleValide != null && ListeScelleValide.Count() > 0)
                    {
                        List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "Matricule_Receiver", "Numero_Scelle", "Nbre_Scelles");
                        ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelle_1);
                        ctr.Show();

                    }
                  
                }

                else
                {
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsRemiseScelleByAg> affichierScelle()
        {
            try
            {
                List<CsRemiseScelleByAg> ListeScelleValides = new List<CsRemiseScelleByAg>();
               
                return ListeScelleValides;
            }
            catch (Exception)
            {
                
             return null;
            }
        }

        private void btn_Searchcelle_2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListdesScelles.Count != 0)
                {
                    var ListeScelleValide = ListdesScelles.Where(s => s.Status_ID == SessionObject.Enumere.StatusScelleRemis );
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_1.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_1.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_2.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_2.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_3.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_3.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_Cache.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_Cache.Text).ToList();
                    if (ListeScelleValide != null && ListeScelleValide.Count() > 0)
                    {
                        List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "Matricule_Receiver", "Numero_Scelle", "Nbre_Scelles");
                        ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelle_2);
                        ctr.Show();

                    }
                    else
                    {
                        Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                    }
                }

        
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_Searchcelle_3_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (ListdesScelles.Count > 0)
                {
                    var ListeScelleValide = ListdesScelles.Where(s => s.Status_ID ==SessionObject.Enumere.StatusScelleRemis );
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_1.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_1.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_2.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_2.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_3.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_3.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_Cache.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_Cache.Text).ToList();
                    if (ListeScelleValide != null && ListeScelleValide.Count() > 0)
                    {
                        List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "Matricule_Receiver", "Numero_Scelle", "Nbre_Scelles");
                        ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelle_3);
                        ctr.Show();

                    }
                    
                }
                else
                {
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }
        
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btn_SearchcelleCahe__Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ListdesScelles.Count >0)
                {
                    var ListeScelleValide = ListdesScelles.Where(s => s.Status_ID == SessionObject.Enumere.StatusScelleRemis );
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_1.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_1.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_2.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_2.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_3.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_3.Text).ToList();
                    if (!string.IsNullOrEmpty(txt_NumNouveauScelle_Cache.Text))
                        ListeScelleValide = ListeScelleValide.Where(c => c.Numero_Scelle != txt_NumNouveauScelle_Cache.Text).ToList();
                    if (ListeScelleValide != null && ListeScelleValide.Count() > 0)
                    {
                        List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "Matricule_Receiver", "Numero_Scelle", "Nbre_Scelles");
                        ctr.Closed += new EventHandler(galatee_OkClickedbtn_SearchScelle_Ca);
                        ctr.Show();

                    }
                  
                }
                else
                {
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void rbt_NouveauScelle_1_Checked(object sender, RoutedEventArgs e)
        {
            if (rbt_NouveauScelle_1.IsChecked == true)
                btn_SearchScelle_1.IsEnabled = true;
            else
            {
                btn_SearchScelle_1.IsEnabled = false;
                txt_NumNouveauScelle_1.Text = string.Empty;
            }
        }

        private void rbt_NouveauScelle_2_Checked(object sender, RoutedEventArgs e)
        {

            if (rbt_NouveauScelle_2.IsChecked == true)
                btn_Searchcelle_2.IsEnabled = true;
            else
            {
                btn_Searchcelle_2.IsEnabled = false;
                txt_NumNouveauScelle_2.Text = string.Empty;
            }
        }

        private void rbt_NouveauScelle_3_Checked(object sender, RoutedEventArgs e)
        {
            if (rbt_NouveauScelle_3.IsChecked == true)
                btn_SearchScelle_3.IsEnabled = true;
            else
            {
                btn_SearchScelle_3.IsEnabled = false;
                txt_NumNouveauScelle_3.Text = string.Empty;
            }

        }

        private void rbt_NouveauScelle_Cache_Checked(object sender, RoutedEventArgs e)
        {

            if (rbt_NouveauScelle_Cache.IsChecked == true)
                btn_ListScelle_Cache.IsEnabled = true;
            else
            {
                btn_ListScelle_Cache.IsEnabled = false;
                txt_NumNouveauScelle_Cache.Text = string.Empty;
            }
        }

        private void Cbo_typeCmpt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_typeCmpt.SelectedItem != null)
            {
                int IdProduit = ((ServiceAccueil.CsProduit)Cbo_Produit.SelectedItem).PK_ID; 
                string CodeTypeCompte = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).CODE;
            

                this.Cbo_Diametre.ItemsSource = null;
                this.Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                //this.Cbo_Diametre.ItemsSource = SessionObject.LstCalibreCompteur.Where(t => t.FK_IDPRODUIT == IdProduit && t.CODEPHASE == CodeTypeCompte).ToList();
                this.Cbo_Diametre.ItemsSource = SessionObject.LstCalibreCompteur.Where(t => t.FK_IDPRODUIT == IdProduit).ToList();
            }
        }



    }
}

