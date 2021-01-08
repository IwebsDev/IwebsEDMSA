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
    public partial class UcSaisiCompteursMT : ChildWindow
    {
        public CsRechercheCompteur lecompteur;
        List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur> ListdesModelesfonctMarq = new List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur>();
        List<CsRemiseScelleByAg> ListdesScelles = new List<CsRemiseScelleByAg>();
        private CsCompteurBta ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache, ID_Diametre;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        public  List<CsCompteurBta> listForInsertOrUpdate = new List<CsCompteurBta>();
        public  bool isOkClick = false;
        public UcSaisiCompteursMT()
        {
            InitializeComponent();
            RemplirListeCmbDesEtatCompteursExistant();
            RemplirListeCmbDesModelesMarqueExistant();
            ListeScelleExistant();
            this.txt_Cadran.Text = "6";
            this.txt_ANNEEFAB.MaxLength = 4;
            this.txt_NumCpteur .MaxLength = 20;
            ModeExecution = SessionObject.ExecMode.Creation;
        }
       public  ServiceAccueil.CsDemande laDemande ;
        public UcSaisiCompteursMT(ServiceAccueil.CsDemande lademande)
        {
            InitializeComponent();
            RemplirListeCmbDesEtatCompteursExistant();
            RemplirListeCmbDesModelesMarqueExistant();
            ListeScelleExistant();
            this.txt_Cadran.Text = "6";
            this.txt_ANNEEFAB.MaxLength = 4;
            this.txt_NumCpteur.MaxLength = 20;
            ModeExecution = SessionObject.ExecMode.Creation;
            laDemande = lademande;
            txt_CodeProduit.Text = laDemande.LaDemande.LIBELLEPRODUIT;

            this.txt_PuissanceSouscrite.Text = lademande.LaDemande.PUISSANCESOUSCRITE.ToString();
            this.txt_PuissanceInstalle.Text = lademande.Branchement.PUISSANCEINSTALLEE.ToString();
            this.txt_TypeComptage.Text = lademande.LaDemande.TYPECOMPTAGE ;
            this.txt_NombreDeTransformateur.Text = lademande.Branchement.NOMBRETRANSFORMATEUR.ToString();
            if (lademande.TravauxDevis != null)
            {
                this.TxtSectionCable.Text = string.IsNullOrEmpty(lademande.TravauxDevis.NBRCABLESECTION) ? string.Empty :
                lademande.TravauxDevis.NBRCABLESECTION;
            }
        }
        public UcSaisiCompteursMT(CsCompteurBta pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
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
                ListeScelleExistant();
                 
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
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.RetourneToutMarqueCompleted += (ssender, args) =>
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
                        this.Cbo_Marque.ItemsSource = ListdesModelesfonctMarq;
                        this.Cbo_Marque.DisplayMemberPath = "LIBELLE";
                        this.Cbo_Marque.SelectedValuePath = "PK_ID";
                        this.Cbo_Marque.SelectedValue = string.Empty;

                    }
                };
                client.RetourneToutMarqueAsync();
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
        List<CsCompteurBta> listObjetForInsertOrUpdate = null;
        private List<CsCompteurBta> GetInformationsFromScreen()
        {
            listObjetForInsertOrUpdate = new List<CsCompteurBta>();
            try
            {
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Creation)
                {
                        var tCompteur = new CsCompteurBta();
                        tCompteur.Numero_Compteur = txt_NumCpteur.Text;
                        tCompteur.EtatCompteur_ID = ((CsRefEtatCompteur)Cbo_Etat_cmpt.SelectedItem).EtatCompteur_ID;
                        tCompteur.LIBELLEETATCOMPTEUR  = ((CsRefEtatCompteur)Cbo_Etat_cmpt.SelectedItem).Libelle_ETAT ;
                        tCompteur.ANNEEFAB = txt_ANNEEFAB.Text;
                        tCompteur.FONCTIONNEMENT = "1";
                        tCompteur.CADRAN = Convert.ToByte(this.txt_Cadran.Text);
                        tCompteur.FK_IDCALIBRECOMPTEUR = null;
                        tCompteur.FK_IDTYPECOMPTEUR = 1;
                        tCompteur.MARQUE = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).CODE ;
                        tCompteur.LIBELLEMARQUE = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).LIBELLE ;
                        tCompteur.FK_IDMARQUECOMPTEUR = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).PK_ID ;
                        tCompteur.DATECREATION = DateTime.Now;
                        tCompteur.USERCREATION = UserConnecte.matricule;
                        tCompteur.ETAT = "Non_Affecté";
                        tCompteur.StatutCompteur = "Non_Affecté";
                        tCompteur.CODEPRODUIT = SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).CODE;
                        tCompteur.FK_IDPRODUIT = SessionObject.ListeDesProduit.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ElectriciteMT).PK_ID;
                        tCompteur.NUMERO = txt_NumCpteur.Text;
                        listObjetForInsertOrUpdate.Add(tCompteur);
                    
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
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message,"Demande");
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
            }
            catch (Exception ex)
            {

                throw ex;
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

