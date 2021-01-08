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
using Galatee.Silverlight.ServiceFraude;
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Accueil
{
    public partial class UcInitialisationDemandeFraude : ChildWindow
    {
        private List<Galatee.Silverlight.ServiceAccueil.CsCentre> _listeDesCentreExistant = null;
        Galatee.Silverlight.ServiceFraude.CsClient Client = new Galatee.Silverlight.ServiceFraude.CsClient();
        CsClientFraude Clientfraude = new CsClientFraude();
        List<CsFraude> listForInsertOrUpdate = new List<CsFraude>();
        List<CsDenonciateur> listForInsertOrUpdate1 = new List<CsDenonciateur>();
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistantCentre = null;
         string NumeroDemandeFraude;
        public UcInitialisationDemandeFraude()
        {
            InitializeComponent();
            ChargerSourceControle();
            ChargerMoyenDenonciation();
            _listeDesCentreExistant = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
           // RemplirCentrePerimetre(_listeDesCentreExistant);

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
       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

   

        private void ActiverEnregistrerOuTransmettre()
        {
            try
            {
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

                      
                    }
                };
                client.SelectAllMoyenDenomciationAsync();
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



                #region information abonnement

                if (string.IsNullOrEmpty(this.txt_Abonne.Text))
                    throw new Exception("Remplir le Champs Abonne");

               
                if (Chek_Identifia.IsChecked == true)
                {

                    if (string.IsNullOrEmpty(this.txt_Contact.Text))
                        throw new Exception("Remplir le Champs  le Contact ");

                    if (Cbo_MoyenDenociation.SelectedItem == null)
                        throw new Exception("Selectionnez Moyen Denociation ");
                }
                if (string.IsNullOrEmpty(this.txt_Abonne.Text))
                    throw new Exception("Remplir le Champs  Abonne");
                if (Chek_Identifia.IsChecked == true)
                {

                    //if (string.IsNullOrEmpty(this.txt_LienAbonne.Text))
                    //    throw new Exception("Remplir le Champs  Abonnée ");
                    if (string.IsNullOrEmpty(this.txt_Identite.Text))
                        throw new Exception("Remplir le Champs  Abonnée ");
                    //if (string.IsNullOrEmpty(this.txt_Identite.Text))
                    //    throw new Exception("Remplir le Champs  Abonnée ");
                    //if (Cbo_MoyenDenociation.SelectedItem == null)
                    //    throw new Exception("Selectionnez Moyen Denociation ");
                }
                //if (Cbo_AnormalieCacheb.SelectedItem == null)
                //    throw new Exception("Selectionnez Anormalie Cache borne ");
                //if (Cbo_AnormlieCompteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Anormalie Compteur ");
                //if (Cbo_CalibreCompteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Compteur ");

                //if (Cbo_CalibreDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Dijoncteur ");
                //if (Cbo_Fils.SelectedItem == null)
                //    throw new Exception("Selectionnez Calibre Fils ");
                //if (Cbo_MarqueCmpt.SelectedItem == null)
                //    throw new Exception("Selectionnez marque Compteur ");
                //if (Cbo_MArqueDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez marque Dijoncteur ");

                //if (Cbo_NbresfilsDijoncteur.SelectedItem == null)
                //    throw new Exception("Selectionnez nombres de fils rque Dijoncteur ");
                //if (Cbo_ReglageCmpt.SelectedItem == null)
                //    throw new Exception("Selectionnez reglagle compteur");
                //if (Cbo_usage.SelectedItem == null)
                //    throw new Exception("Selectionnez usage");
                //if (Cbo_Produit.SelectedItem == null)
                //    throw new Exception("Selectionnez Produit");




                //if (string.IsNullOrEmpty(this.txt_Identite.Text))
                //    throw new Exception("remplir le coffre Fusile ");

                //if (string.IsNullOrEmpty(this.txt_certifiplombage.Text))
                //    throw new Exception("remplir le certifie plombage ");

                //if (string.IsNullOrEmpty(this.txt_refeplombs.Text))
                //    throw new Exception("remplir referend plomgs ");

                //if (string.IsNullOrEmpty(this.txt_reference_plombs.Text))
                //    throw new Exception("remplir referend plomgs ");
                //if (string.IsNullOrEmpty(this.DateAbonnemnt.SelectedDate.ToString()))
                //    throw new Exception("remplir la date ");
                //if (string.IsNullOrEmpty(this.DateBranchemnt.SelectedDate.ToString()))
                //    throw new Exception("remplir la date ");

                //if (string.IsNullOrEmpty(this.txt.Text))
                //        throw new Exception("remplir referend plomgs ");

                //if (((CsProduit)Cbo_Produit.SelectedItem).CODE != SessionObject.Enumere.ElectriciteMT)
                //{
                //    if (string.IsNullOrEmpty(this.txt_Reglage.Text))
                //        throw new Exception("Selectionnez le calibre ");
                //}
                #endregion
                //#region Adresse géographique
                //if (Cbo_Centre.SelectedItem == null)
                //    throw new Exception("Selectionnez Centre ");

                //if (string.IsNullOrEmpty(this.txtCentre.Text))
                //    throw new Exception("Séléctionnez le Centre ");

                ////if (string.IsNullOrEmpty(this.txt_Quartier.Text))
                ////    throw new Exception("Séléctionnez le quartier ");
                //#endregion

                return ReturnValue;

            }
            catch (Exception ex)
            {
                //this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
            }

        }

        private void NewButton1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcEnregistrementClient Newfrm1 = new UcEnregistrementClient(); ;
                Newfrm1.CallBack += Newfrm1_CallBack;
                Newfrm1.Show();
            }
            catch (Exception ex)
            {

                throw ex ;
            }

        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UcRechercheClient Newfrm = new UcRechercheClient(); ;
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
           txt_Abonne.Text = Client.NOMABON;
           txt_Abonne.Tag = Client.PK_ID;

          Clientfraude.Nomabon =Client.NOMABON;
          Clientfraude.Client = Client.REFCLIENT;
          Clientfraude.Centre = Client.CENTRE;
          Clientfraude.FK_IDCENTRE =(int) Client.FK_IDCENTRE;
          Clientfraude.Telephone=Client.TELEPHONE ;
          Clientfraude.Porte = Client.PORTE;
          Clientfraude.Ordre  = Client.ORDRE ;
          LaDemande.ClientFraude = Clientfraude;
            
        }

        private void Newfrm1_CallBack(object sender, CustumEventArgs e)
        {
            
            Clientfraude = e.Data as CsClientFraude;
            txt_Abonne.Text = Clientfraude.Nomabon;
           txt_Abonne.Tag = Clientfraude.PK_ID;
           Clientfraude.Ordre = Clientfraude.Ordre;

           LaDemande.ClientFraude = Clientfraude;


        }
 
        private CsFraude GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new CsFraude();
            try
            {
                if ((int)txt_Abonne.Tag == 0)
                {
                    var tFraude = new CsFraude
                    {
                        DateCreation = DateTime.Now.Date,
                        DateEtape = DateTime.Now.Date,
                        DateReclamation = DateTime.Now.Date,
                        FK_IDSOURCECONTROLE = ((CsSourceControle)Cbo_SourceControle.SelectedItem).PK_ID,
                        FicheTraitement = NumeroDemandeFraude,
                        FK_IDDENONCIATEUR = ((CsMoyenDenomciation)Cbo_MoyenDenociation.SelectedItem).PK_ID,

                    };
                    listObjetForInsertOrUpdate = tFraude;

                }

                else
                {
                    var tFraude = new CsFraude
                     {
                         DateCreation = DateTime.Now.Date,
                         DateEtape = DateTime.Now.Date,
                         DateReclamation = DateTime.Now.Date,
                         FK_IDCLIENTFRAUDE = (int)txt_Abonne.Tag,
                         FK_IDSOURCECONTROLE = ((CsSourceControle)Cbo_SourceControle.SelectedItem).PK_ID,
                         FicheTraitement = NumeroDemandeFraude,
                         FK_IDDENONCIATEUR = ((CsMoyenDenomciation)Cbo_MoyenDenociation.SelectedItem).PK_ID,


                     };
                    listObjetForInsertOrUpdate = tFraude;
                }
                
               
                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
               // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        private CsDenonciateur GetInformationsFromScreen1()
        {
            var listObjetForInsertOrUpdate1 = new CsDenonciateur();
            try
            {


                    if (Chek_Identifia.IsChecked == true)
                    {
                        var tDenonciateur = new CsDenonciateur
                        {
                        DateDenonciation = Convert.ToDateTime( dateDeniation.SelectedDate),
                        //Localisation = ((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).LIBELLE,
                        LienAvecAbonne = string.IsNullOrEmpty(txt_LienAbonne.Text) ? string.Empty : txt_LienAbonne.Text,
                        Contact = string.IsNullOrEmpty(txt_Contact.Text) ? string.Empty : txt_Contact.Text,
                        Nom = string.IsNullOrEmpty(txt_Identite.Text) ? string.Empty : txt_Identite.Text,
                        FK_IDMOYENDENONCIATION = ((CsMoyenDenomciation)Cbo_MoyenDenociation.SelectedItem).PK_ID,
                        //FK_IDLOCALISATION = ((ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID
                       
                        };
                        listObjetForInsertOrUpdate1=tDenonciateur;

                    }



                    return listObjetForInsertOrUpdate1;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        private void btnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            Enregistrement(LaDemande);

        }

        private void Chek_Identifia_Checked(object sender, RoutedEventArgs e)
        {
            if (Chek_Identifia.IsChecked == true)
            { txt_Identite.IsReadOnly = false;  
                txt_Contact.IsReadOnly = false;
               txt_LienAbonne.IsReadOnly=false;
                dateDeniation.IsEnabled=true;
            }
            if (Chek_Identifia.IsChecked == false)
            {  txt_Identite.IsReadOnly = true;
            txt_Contact.IsReadOnly = true;
            txt_LienAbonne.IsReadOnly = true;
            dateDeniation.IsEnabled = false;

            }
        }

        private void cheb_AbonneRepertoire_Checked(object sender, RoutedEventArgs e)
        {
            if (Rdbtn_AbnRepetorie.IsChecked == true)
            {
                btn_creerAbon.IsEnabled = false;
                btn_creerAbon.Visibility = Visibility.Collapsed;

                btn_rechecherAbon.IsEnabled = true;
                btn_rechecherAbon.Visibility = Visibility.Visible;
                txt_Abonne.Text = string.Empty;
                txt_Abonne.Tag = string.Empty;

            }
            if (Rdbtn_AbnnReporier.IsChecked == true)
            {

                btn_rechecherAbon.IsEnabled = true;
                btn_rechecherAbon.Visibility = Visibility.Collapsed;

                btn_creerAbon.IsEnabled = true;
                btn_creerAbon.Visibility = Visibility.Visible;
                txt_Abonne.Text = string.Empty;
                txt_Abonne.Tag = string.Empty;
               
            }

        }

        private void ValidationDemande(CsDemandeFraude _LaDemande)
        {
            try
            {
             
                _LaDemande.LaDemande = new Galatee.Silverlight.ServiceFraude.CsDemandeBase();
                Galatee.Silverlight.ServiceAccueil.CsTdem leTydemande =new  Galatee.Silverlight.ServiceAccueil.CsTdem();
                leTydemande = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DemandeFraude);

                _LaDemande.LaDemande.TYPEDEMANDE =(string) leTydemande.CODE;
                _LaDemande.LaDemande.FK_IDTYPEDEMANDE =(int) leTydemande.PK_ID;
                _LaDemande.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                _LaDemande.LaDemande.CENTRE = UserConnecte.Centre;
                _LaDemande.LaDemande.FK_IDCENTRE = UserConnecte.FK_IDCENTRE;
                _LaDemande.LaDemande.USERCREATION = UserConnecte.matricule;
                _LaDemande.LaDemande.DATECREATION = DateTime.Now;
         
                //Lancer la transaction de mise a jour en base
                FraudeServiceClient service1 = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));

              
                service1.ValiderDemandeInitailisationCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (!string.IsNullOrEmpty(res.Result))
                    {
                        string Retour = res.Result;
                        string[] coupe = Retour.Split('.');
                       Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], _LaDemande.LaDemande.FK_IDCENTRE, coupe[1], _LaDemande.LaDemande.FK_IDTYPEDEMANDE);
                    }
                    //if (Closed != null)
                    //    Closed(this, new EventArgs());
                    this.DialogResult = false;
                };
                service1.ValiderDemandeInitailisationAsync(_LaDemande);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }


        }

        private void Enregistrement(CsDemandeFraude DemandeFraude)
        {
            try
            {
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Langue.Fraude, Langue.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        DemandeFraude.Fraude = GetInformationsFromScreen();
                        DemandeFraude.Denonciateur = GetInformationsFromScreen1();

                        var service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            service.InsertFraudeDenociateurCompleted += (snder, insertR) =>
                            {
                                if (insertR.Cancelled ||
                                    insertR.Error != null)
                                {
                                    Message.ShowError(insertR.Error.Message, Langue.Fraude);
                                    return;
                                }
                                if (insertR.Result == 1)
                                {
                                    Message.ShowError(Langue.ErreurInsertionDonnees, Langue.Fraude);
                                    return;
                                }
                                //OnEvent(null);
                                DialogResult = true;
                            };
                            service.InsertFraudeDenociateurAsync(DemandeFraude);
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

        //#region situation geaographique
        //private void RemplirCentrePerimetre(List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentre)
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

        //            //if (lstSite != null)
        //            //    foreach (var item in lstSite)
        //            //    {
        //            //        Cbo_Site.Items.Add(item);
        //            //    }
        //            //Cbo_Site.SelectedValuePath = "PK_ID";
        //            //Cbo_Site.DisplayMemberPath = "LIBELLE";

        //            //if (lstSite != null && lstSite.Count == 1)
        //            //    Cbo_Site.SelectedItem = lstSite.First();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void Cbo_Centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (this.Cbo_Centre.SelectedItem != null)
        //        {
        //            Galatee.Silverlight.ServiceAccueil.CsCentre centre = Cbo_Centre.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsCentre;
        //            if (centre != null)
        //            {
        //                this.txtCentre.Text = centre.CODE ?? string.Empty;
        //                this.txtCentre.Tag = centre.PK_ID;
        //                string numIncrementiel = ((ServiceAccueil.CsCentre)(Cbo_Centre.SelectedItem)).NUMDEM.ToString();
        //                this.NumeroDemandeFraude = ((ServiceAccueil.CsCentre)(Cbo_Centre.SelectedItem)).CODE + numIncrementiel.PadLeft(10, '0');

        //               // RemplirCommuneParCentre(centre);
        //                //  RemplirProduitCentre(centre);
        //            }
        //            //  VerifierDonneesSaisiesInformationsDevis();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Langue.Fraude);
        //    }
        //}
        //#endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!VerifieChampObligation()) return ;
            LaDemande.Fraude = GetInformationsFromScreen();
            LaDemande.Denonciateur = GetInformationsFromScreen1();
            ValidationDemande(LaDemande);
        }

        private void Radit_Abonne_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}

