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

namespace Galatee.Silverlight.Devis
{
    public partial class UcSaisiCompteur : ChildWindow
    {
        public CsRechercheCompteur lecompteur;
        List<CsMarque_Modele> ListdesModelesfonctMarq = new List<CsMarque_Modele>();
        List<CsRemiseScelleByAg> ListdesScelles = new List<CsRemiseScelleByAg>();
        private CsCompteurBta ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache, ID_Diametre;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public  List<CsCompteurBta> listForInsertOrUpdate = new List<CsCompteurBta>();
        public  bool isOkClick = false;
        public UcSaisiCompteur()
        {
            InitializeComponent();
            RemplirListeCmbDesEtatCompteursExistant();
            RemplirListeCmbDesModelesMarqueExistant();
            ChargerDiametreCompteur();
            ChargerTypeCompteur();
            ListeScelleExistant();
            this.txt_Cadran.Text = "6";
            this.txt_ANNEEFAB.MaxLength = 4;
            this.txt_NumCpteur .MaxLength = 20;
            ModeExecution = SessionObject.ExecMode.Creation;
        }
       public  ServiceAccueil.CsDemande laDemande ;
        public UcSaisiCompteur(ServiceAccueil.CsDemande lademande)
        {
            InitializeComponent();
            RemplirListeCmbDesEtatCompteursExistant();
            RemplirListeCmbDesModelesMarqueExistant();
            ChargerDiametreCompteur();
            ChargerTypeCompteur();
            ListeScelleExistant();
            this.txt_Cadran.Text = "6";
            this.txt_ANNEEFAB.MaxLength = 4;
            this.txt_NumCpteur.MaxLength = 20;
            ModeExecution = SessionObject.ExecMode.Creation;
            laDemande = lademande;
            txt_CodeProduit.Text = laDemande.LaDemande.LIBELLEPRODUIT;
 

        }
        public UcSaisiCompteur(CsCompteurBta pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
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
                RemplirListeCmbDesEtatCompteursExistant();
                RemplirListeCmbDesModelesMarqueExistant();
                ChargerDiametreCompteur();
                ChargerTypeCompteur();
                ListeScelleExistant();
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                      
                        Cbo_Diametre.SelectedItem = SessionObject.LstCalibreCompteur .FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDCALIBRECOMPTEUR );
                        Cbo_typeCmpt.SelectedItem = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDTYPECOMPTEUR);
                        List<CsRefEtatCompteur> lstetat = (List<CsRefEtatCompteur>)this.Cbo_Etat_cmpt.ItemsSource;
                        if (lstetat != null)
                            Cbo_Etat_cmpt.SelectedItem = lstetat.FirstOrDefault(t => t.EtatCompteur_ID == ObjetSelectionnee.EtatCompteur_ID);
                      
                        List<CsMarque_Modele> lstMaqmMdt = ListdesModelesfonctMarq;
                        if (lstMaqmMdt != null)
                        {
                            Cbo_Marque.SelectedItem =ObjetSelectionnee.LIBELLEMARQUE ;
                            Cbo_Modele.SelectedItem = lstMaqmMdt.FirstOrDefault(t => t.MARQUE_ID == ObjetSelectionnee.FK_IDMARQUECOMPTEUR);
                        }
                        txt_ANNEEFAB.Text = ObjetSelectionnee.ANNEEFAB;
                        txt_NumCpteur.Text = ObjetSelectionnee.Numero_Compteur;
   
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
                        ChargerDonneByProduit(laDemande.LaDemande.FK_IDPRODUIT.Value);
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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


        //private void ChargerProduit()
        //{
        //    try
        //    {
        //        if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
        //        {
        //            Cbo_Produit.ItemsSource = null;
        //            Cbo_Produit.DisplayMemberPath = "LIBELLE";
        //            Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;

        //            return;
        //        }
        //        Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //        service1.ListeDesProduitCompleted += (sr, res) =>
        //        {
        //            if (res != null && res.Cancelled)
        //                return;
        //            SessionObject.ListeDesProduit = res.Result;
        //            Cbo_Produit.ItemsSource = null;
        //            Cbo_Produit.DisplayMemberPath = "LIBELLE";
        //            Cbo_Produit.ItemsSource = SessionObject.ListeDesProduit;
        //        };
        //        service1.ListeDesProduitAsync();
        //        service1.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
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
                        tCompteur.LIBELLEETATCOMPTEUR = ((CsRefEtatCompteur)Cbo_Etat_cmpt.SelectedItem).Libelle_ETAT;
                        tCompteur.ANNEEFAB = txt_ANNEEFAB.Text;
                        tCompteur.FONCTIONNEMENT = "1";
                        tCompteur.CADRAN = Convert.ToByte(this.txt_Cadran .Text );
                        tCompteur.FK_IDCALIBRECOMPTEUR = null;
                        if (Cbo_Diametre.SelectedItem != null)
                        {
                            tCompteur.FK_IDCALIBRECOMPTEUR = ((ServiceAccueil.CsCalibreCompteur)Cbo_Diametre.SelectedItem).PK_ID;
                            tCompteur.CALIBRE  = ((ServiceAccueil.CsCalibreCompteur)Cbo_Diametre.SelectedItem).LIBELLE ;
                        }
                        tCompteur.MARQUE = ((CsMarque_Modele)Cbo_Marque.SelectedItem).CODE_Marque;
                        tCompteur.LIBELLEMARQUE  = ((CsMarque_Modele)Cbo_Marque.SelectedItem).Libelle_MArque ;
                        tCompteur.FK_IDMARQUECOMPTEUR = ((CsMarque_Modele)Cbo_Marque.SelectedItem).MARQUE_ID;
                        tCompteur.FK_IDTYPECOMPTEUR = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).PK_ID;
                        tCompteur.TYPECOMPTEUR = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).CODE ;
                        tCompteur.LIBELLETYPECOMPTEUR = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).LIBELLE ;
                    
                        tCompteur.DATECREATION = DateTime.Now;
                        tCompteur.USERCREATION = UserConnecte.matricule;
                        tCompteur.ETAT = "Non_Affecté";
                        tCompteur.StatutCompteur="Non_Affecté";
                        tCompteur.CODEPRODUIT = laDemande.LaDemande.PRODUIT ;
                        tCompteur.FK_IDPRODUIT = laDemande.LaDemande.FK_IDPRODUIT.Value  ;
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
                isOkClick = true;
                this.DialogResult = false;

                //ServiceAccueil.CsCanalisation canal = new ServiceAccueil.CsCanalisation()
                //{
                //    CENTRE = laDemande.LaDemande.CENTRE ,
                //    CLIENT = laDemande.LaDemande.CLIENT,
                //    NUMDEM = laDemande.LaDemande.NUMDEM,
                //    PRODUIT = laDemande.LaDemande.PRODUIT,
                //    PROPRIO = "1",
                //    FK_IDPRODUIT = laDemande.LaDemande.FK_IDPRODUIT.Value,
                //    FK_IDDEMANDE = laDemande.LaDemande.PK_ID,
                //    FK_IDCENTRE = laDemande.LaDemande.FK_IDCENTRE,
                //    POSE = System.DateTime.Now,
                //    USERCREATION = laDemande.LaDemande.USERCREATION,
                //    USERMODIFICATION = laDemande.LaDemande.USERCREATION,
                //    DATECREATION = System.DateTime.Now,
                //    DATEMODIFICATION = System.DateTime.Now,
                //    FK_IDPROPRIETAIRE = 1,
                //};
                //if (laDemande.LaDemande.FK_IDREGLAGECOMPTEUR != null)
                //    canal.FK_IDREGLAGECOMPTEUR = laDemande.LaDemande.FK_IDREGLAGECOMPTEUR;
                //if (laDemande.LstCanalistion == null )
                //    laDemande.LstCanalistion = new List<ServiceAccueil.CsCanalisation>();

                //if (laDemande.LstCanalistion.Count == 0)
                //    laDemande.LstCanalistion.Add(canal);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.Commune);
            }
        }

  

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.FermetureEcran(this);
        }
     
        private void Cbo_Marque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                if (Cbo_Marque.SelectedItem == null)
                    return;
                if (Cbo_Marque.SelectedValue != null )
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
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        Galatee.Silverlight.ServiceAccueil.CsReglageCompteur leReglageCompteur = null;

        private void Cbo_typeCmpt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Cbo_typeCmpt.SelectedItem != null)
            {
                int IdProduit =laDemande.LaDemande.FK_IDPRODUIT.Value ; 
                string CodeTypeCompte = ((ServiceAccueil.CsTcompteur)Cbo_typeCmpt.SelectedItem).CODE;

                this.Cbo_Diametre.ItemsSource = null;
                this.Cbo_Diametre.DisplayMemberPath = "LIBELLE";
                this.Cbo_Diametre.ItemsSource = SessionObject.LstCalibreCompteur.Where(t => t.FK_IDPRODUIT == IdProduit).ToList();

                leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDemande.LaDemande.REGLAGECOMPTEUR);
                List<Galatee.Silverlight.ServiceAccueil.CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t => t.REGLAGEMINI >= leReglageCompteur.REGLAGEMINI &&
                                                                                                          t.REGLAGEMAXI <= leReglageCompteur.REGLAGEMAXI &&
                                                                                                          t.FK_IDPRODUIT == laDemande.LaDemande.FK_IDPRODUIT).ToList();

                List<ServiceAccueil.CsCalibreCompteur> lesCalibres=SessionObject.LstCalibreCompteur.Where(t=>LeCalibreEquivalant.Select(u=>u.PK_ID).Contains (t.PK_ID)).ToList(); 
                if (lesCalibres != null && lesCalibres.Count != 0)
                {
                    int lePremier = lesCalibres.First().PK_ID ;
                    this.Cbo_Diametre.SelectedItem  = SessionObject.LstCalibreCompteur.FirstOrDefault (t => t.PK_ID  == lePremier);
                }
            }
        }

        private void btn_Ajouter_Click(object sender, RoutedEventArgs e)
        {

            listForInsertOrUpdate = GetInformationsFromScreen();
            this.dtg_CompteurSaisie.ItemsSource = null;
            this.dtg_CompteurSaisie.ItemsSource = listForInsertOrUpdate;
        }
    }
}

