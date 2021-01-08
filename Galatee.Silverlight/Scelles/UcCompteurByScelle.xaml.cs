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
    public partial class UcCompteurByScelle : ChildWindow
    {
        public CsRechercheCompteur lecompteur;
        List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur> ListdesModelesfonctMarq = new List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur>();
        List<CsRemiseScelleByAg> ListdesScelles = new List<CsRemiseScelleByAg>();
        private CsCompteurBta ObjetSelectionnee = null;
        static int ID_MArque, nbr_capot, nb_cache, ID_Diametre;
        private List<ServiceAccueil.CsCentre> _listeDesCentreExistant = null;

        private Object ModeExecution = null;
        private DataGrid dataGrid = null;
        List<CsCompteurBta> listForInsertOrUpdate = new List<CsCompteurBta>();
        public UcCompteurByScelle()
        {
            InitializeComponent();
            //RemplirListeCmbDesEtatCompteursExistant();
            //RemplirListeCmbDesModelesMarqueExistant();
            ChargerProduit();
            ModeExecution = SessionObject.ExecMode.Creation;
            this.txt_NumCpteur.MaxLength = 15;
            //this.txt_ANNEEFAB.MaxLength = 4;
            //this.txt_Cadran.MaxLength = 1;
            //this.txt_Cadran.Text = "6";
            this.Rdb_NumCompt.IsChecked = true;
            SelectAllCompteurNonAffecte();
            //OKButton.IsEnabled = true;
        }
        public UcCompteurByScelle(CsCompteurBta pObject, SessionObject.ExecMode pExecMode, DataGrid pGrid)
        {
            try
            {
                InitializeComponent();
                Translate();
                var CompteurBt = new CsCompteurBta();
                if (pObject != null)
                    ObjetSelectionnee = Utility.ParseObject(CompteurBt, pObject as CsCompteurBta);

                this.txt_NumCpteur.MaxLength = 15;
                //this.txt_ANNEEFAB.MaxLength = 4;
                //this.txt_Cadran .MaxLength = 1;
                //this.txt_Cadran.Text  = "6";
                ModeExecution = pExecMode;
                dataGrid = pGrid;
                // dtgrdlReceptionScelle.ItemsSource = donnesDatagrid;
                //RemplirListeCmbDesEtatCompteursExistant();
                //RemplirListeCmbDesModelesMarqueExistant();
                ChargerProduit();
                //if (dataGrid != null) 
                //    donnesDatagrid = dataGrid.ItemsSource as ObservableCollection<CsLotMagasinGeneral>;
                if ((SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Modification || (SessionObject.ExecMode)ModeExecution == SessionObject.ExecMode.Consultation)
                {
                    if (ObjetSelectionnee != null)
                    {
                        //List<CsRefEtatCompteur> lstetat = (List<CsRefEtatCompteur>)this.Cbo_Etat_cmpt.ItemsSource;
                        //if (lstetat != null)
                        //    Cbo_Etat_cmpt.SelectedItem = lstetat.FirstOrDefault(t => t.EtatCompteur_ID == ObjetSelectionnee.EtatCompteur_ID);

                        //List<Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur> lstMaqmMdt = ListdesModelesfonctMarq;
                        //if (lstMaqmMdt != null)
                        //    Cbo_Marque.SelectedItem =ObjetSelectionnee.LIBELLEMARQUE ;

                        //txt_ANNEEFAB.Text = ObjetSelectionnee.ANNEEFAB;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<CsCompteurBta> leCptGenerale;
        private void SelectAllCompteurNonAffecte()
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.SelectAllCompteurNonAffecteCompleted += (ssender, args) =>
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
                        leCptGenerale = new List<CsCompteurBta>();
                        leCptGenerale =Shared.ClasseMEthodeGenerique.RetourneListCopy<CsCompteurBta>( args.Result);
                        this.dtg_CompteurSaisie.ItemsSource = null;
                        this.dtg_CompteurSaisie.ItemsSource = args.Result;
                    }
                };
                client.SelectAllCompteurNonAffecteAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    
        List<CsRefEtatCompteur> lstEtatCompteur = new List<CsRefEtatCompteur>();
 


        private void ChargerProduit()
        {
            try
            {
                if (SessionObject.ListeDesProduit != null && SessionObject.ListeDesProduit.Count != 0)
                {
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.ListeDesProduitCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    SessionObject.ListeDesProduit = res.Result;
                };
                service1.ListeDesProduitAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

 
        private void DeleteCompteur(CsCompteurBta listForCompteur)
        {
            try
            {

                var service = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));

                if (listForCompteur != null )
                {
                    service.DeleteCompteurCompleted += (snder, UpdateR) =>
                    {
                        if (UpdateR.Cancelled ||
                            UpdateR.Error != null)
                        {
                            Message.ShowError(UpdateR.Error.Message, Languages.EtatDuCompteur);
                            return;
                        }
                        if (UpdateR.Result == false)
                        {
                            Message.ShowError(Languages.ErreurMiseAJourDonnees, Languages.EtatDuCompteur);
                            return;
                        }
                        else
                        {
                        }
                    };
                    service.DeleteCompteurAsync(listForCompteur);

                }
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

        //private void Cbo_Marque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (Cbo_Marque.SelectedItem == null)
        //            return;
        //        if (Cbo_Marque.SelectedValue != null  )
        //            ID_MArque = ((Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur)Cbo_Marque.SelectedItem).PK_ID ;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //private void btn_Modifier_Click_1(object sender, RoutedEventArgs e)
        //{
        //    if (this.dtg_CompteurSaisie.SelectedItem != null)
        //    {
        //        ModeExecution = SessionObject.ExecMode.Creation ;
        //        ObjetSelectionnee = (CsCompteurBta)this.dtg_CompteurSaisie.SelectedItem;
        //        this.txt_ANNEEFAB.Text = ObjetSelectionnee.ANNEEFAB;
        //        this.txt_NumCpteur.Text = ObjetSelectionnee.Numero_Compteur;
        //        this.txt_Cadran.Text = ObjetSelectionnee.CADRAN.ToString();
        //        this.Cbo_Etat_cmpt.SelectedItem = lstEtatCompteur.FirstOrDefault(t => t.EtatCompteur_ID == ObjetSelectionnee.EtatCompteur_ID); ;
        //        this.Cbo_Marque.SelectedItem = ListdesModelesfonctMarq.FirstOrDefault(t => t.PK_ID == ObjetSelectionnee.FK_IDMARQUECOMPTEUR);
        //        OKButton.IsEnabled = true;
        //        btn_Annuler.IsEnabled = false;
        //        CancelButton.IsEnabled = false;
        //        btn_Modifier.IsEnabled = false;
        //        DeleteCompteur(ObjetSelectionnee);

        //    }
        //}
        //private void btn_Annuler_Click_1(object sender, RoutedEventArgs e)
        //{
        //    RazEcran();
        //    ModeExecution = SessionObject.ExecMode.Creation ;
        //}
        //private void btn_Modifier_Click_1(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (this.dtg_CompteurSaisie.SelectedItem != null)
        //        {
        //            ObjetSelectionnee = (CsCompteurBta)this.dtg_CompteurSaisie.SelectedItem;
        //            //if (ObjetSelectionnee.EtatCompteur_ID != 0)
        //            //{
        //            //    Message.ShowInformation("Ce Etat Compteur ne peut ètre modifier ", "info");
        //            //    ObjetSelectionnee = null;
        //            //    return;
        //            //}
        //            ModeExecution = SessionObject.ExecMode.Modification;
        //            if (ListdesModelesfonctMarq.Count != 0)
        //            {
        //                if (ObjetSelectionnee != null)
        //                {
        //                    this.Cbo_Marque.ItemsSource = ListdesModelesfonctMarq;
        //                    this.Cbo_Marque.DisplayMemberPath = "LIBELLE";
        //                    this.Cbo_Marque.SelectedValuePath = "PK_ID";

        //                    foreach (Galatee.Silverlight.ServiceAccueil.CsMarqueCompteur OgSll in Cbo_Marque.ItemsSource)
        //                    {
        //                        if (OgSll.PK_ID == ObjetSelectionnee.FK_IDMARQUECOMPTEUR)
        //                        {
        //                            Cbo_Marque.SelectedItem = OgSll;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
                   
        //            if (lstEtatCompteur.Count != 0)
        //            {
        //                if (ObjetSelectionnee != null)
        //                {
        //                    this.Cbo_Etat_cmpt.ItemsSource = lstEtatCompteur;
        //                    this.Cbo_Etat_cmpt.DisplayMemberPath = "Libelle_ETAT";
        //                    this.Cbo_Etat_cmpt.SelectedValuePath = "EtatCompteur_ID";
        //                    foreach (CsRefEtatCompteur OgSll in Cbo_Etat_cmpt.ItemsSource)
        //                    {
        //                        if (OgSll.EtatCompteur_ID == ObjetSelectionnee.EtatCompteur_ID)
        //                        {
        //                            Cbo_Etat_cmpt.SelectedItem = OgSll;
        //                            break;
        //                        }
        //                    }
        //                }

        //            }
        //            //if (SessionObject.LstDesActivitee.Count != 0)
        //            //{
        //            //    if (ObjetSelectionnee != null)
        //            //    {
        //            //        this.Cbo_Activite.ItemsSource = SessionObject.LstDesActivitee;
        //            //        this.Cbo_Activite.DisplayMemberPath = "Activite_Libelle";
        //            //        this.Cbo_Activite.SelectedValuePath = "Activite_ID";
        //            //        foreach (CsActivite Activite in Cbo_Activite.ItemsSource)
        //            //        {
        //            //            if (Activite.Activite_ID == ObjetSelectionnee.Activite_ID)
        //            //            {
        //            //                Cbo_Activite.SelectedItem = Activite;
        //            //                break;
        //            //            }
        //            //        }
        //            //    }
        //            //}
        //            List<CsLotMagasinGeneral> lstLotScelle = new List<CsLotMagasinGeneral>();
        //            //lstLotScelle.Add(new CsLotMagasinGeneral
        //            //{
        //            //    Numero_depart = ObjetSelectionnee.Numero_depart,
        //            //    Numero_fin = ObjetSelectionnee.Numero_fin

        //            //});
        //            //txt_NumeroDepart.Text = ObjetSelectionnee.Numero_depart;
        //            //txt_NumeroFin.Text = ObjetSelectionnee.Numero_fin;

        //            //dtgrdlReceptionScelle.ItemsSource = lstLotScelle;
        //            //btn_ajout.IsEnabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError("Erreur au chargement des données", "Erreur");
        //    }
        //}

        private void dtg_CompteurSaisie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Rdb_NumCompt_Checked(object sender, RoutedEventArgs e)
        {
            if (Rdb_NumCompt.IsChecked == true)
            {
                labe_Num.Content = "Numero de Compteur";
            }
            if (Rdb_NumScelle.IsChecked == true)
            {
                labe_Num.Content = "Numéro de Scellé";
            }
        }

        private void btn_Modifier_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.dtg_CompteurSaisie.SelectedItem != null)
            {
                CsCompteurBta leCompteurSelect = (CsCompteurBta )this.dtg_CompteurSaisie.SelectedItem;
                UcRepriseScelleCompteurBta ctrl = new UcRepriseScelleCompteurBta(leCompteurSelect, SessionObject.ExecMode.Modification, dtg_CompteurSaisie);
                ctrl.Closed += ctrl_Closed;
                ctrl.Show();
            }
        }

        void ctrl_Closed(object sender, EventArgs e)
        {
            UcRepriseScelleCompteurBta ctrs = sender as UcRepriseScelleCompteurBta;
            if (ctrs.IsOKclic==true  )
            {
                if (this.dtg_CompteurSaisie.ItemsSource != null)
                {

                    CsCompteurBta lecompteurModifier = leCptGenerale.FirstOrDefault(t => t.PK_ID  == ((CsCompteurBta)this.dtg_CompteurSaisie.SelectedItem).PK_ID );
                    if (lecompteurModifier != null)
                    {
                        leCptGenerale.Remove(lecompteurModifier);
                        leCptGenerale.Add(ctrs.leCompteurModier);
                        this.dtg_CompteurSaisie.ItemsSource = null;
                        this.dtg_CompteurSaisie.ItemsSource = leCptGenerale;
                    }
                }
            }
            else
            {
                this.dtg_CompteurSaisie.ItemsSource = null;
                this.dtg_CompteurSaisie.ItemsSource = leCptGenerale;
            }

        }

        private void btn_Annuler_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void RechercherCompteurByNumCptNumScelle(string NumCompteur,string NumScelle)
        {
            try
            {
                IScelleServiceClient client = new IScelleServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Scelles"));
                client.RetourneCompteurBtaByNumCptNumScelleCompleted += (ssender, args) =>
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
                        this.dtg_CompteurSaisie.ItemsSource = null;
                        this.dtg_CompteurSaisie.ItemsSource = args.Result;
                        leCptGenerale = args.Result;
                    }
                };
                client.RetourneCompteurBtaByNumCptNumScelleAsync(NumCompteur, NumScelle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RechercheCompteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Rdb_NumCompt.IsChecked == true)
                    RechercherCompteurByNumCptNumScelle(this.txt_NumCpteur.Text, string.Empty);
                else if (Rdb_NumScelle.IsChecked == true)
                    RechercherCompteurByNumCptNumScelle(string.Empty, this.txt_NumCpteur.Text);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void txt_NumCpteur_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_NumCpteur.Text))
            {
                this.dtg_CompteurSaisie.ItemsSource = null;
                this.dtg_CompteurSaisie.ItemsSource = leCptGenerale;
            }
        }
       
    }
}

