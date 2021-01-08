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
using System.ComponentModel;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcEnregistrementClient : ChildWindow, INotifyPropertyChanged                           
    {
        List<CsClientFraude> listForInsertOrUpdate = new List<CsClientFraude>();
        private List<Galatee.Silverlight.ServiceAccueil.CsCommune> _listeDesCommuneExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil .CsCommune> _listeDesCommuneExistantCentre = null;
        private List<Galatee.Silverlight.ServiceAccueil .CsCentre> _listeDesCentreExistant = null;
        private string Tdem = null;
        private List<Galatee.Silverlight.ServiceAccueil .CsRues> _listeDesRuesExistant = null;
        private List<Galatee.Silverlight.ServiceAccueil.CsQuartier> _listeDesQuartierExistant = null;
        #region EventHandling

        public delegate void MethodeEventHandler(object sender, CustumEventArgs e);
        public event MethodeEventHandler CallBack;
        CustumEventArgs MyEventArg = new CustumEventArgs();

        public virtual void OnEvent(CustumEventArgs e)
        {
            if (CallBack != null)
                CallBack(this, e);
        }

        #endregion

        public UcEnregistrementClient()
        {
            InitializeComponent();
            //ChargerDonneeDuSite();
            RemplirCommune();
            RemplirSecteur();
            _listeDesCentreExistant = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
            RemplirCentrePerimetre(_listeDesCentreExistant);
        }


        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private List<CsClientFraude> GetInformationsFromScreen()
        {
            var listObjetForInsertOrUpdate = new List<CsClientFraude>();
            try
            {
                var sClientFraude = new CsClientFraude
               {
                   Nomabon =txt_Nom.Text,
                   Client=txt_refclient.Text,
                   IdentificationUnique=txt_IdentUnique.Text,
                   Commune=((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).LIBELLE,
                   Centre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).CODE,
                   Quartier = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).LIBELLE,
                   Rue = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).LIBELLE,
                   Secteur = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).LIBELLE,
                   FK_IDCOMMUNE = ((Galatee.Silverlight.ServiceAccueil.CsCommune)Cbo_Commune.SelectedItem).PK_ID,
                   FK_IDQUARTIER = ((Galatee.Silverlight.ServiceAccueil.CsQuartier)Cbo_Quartier.SelectedItem).PK_ID,
                   FK_RUE = ((Galatee.Silverlight.ServiceAccueil.CsRues)Cbo_Rue.SelectedItem).PK_ID,
                   FK_SECTEUR = ((Galatee.Silverlight.ServiceAccueil.CsSecteur)Cbo_Secteur.SelectedItem).PK_ID,
                   FK_IDCENTRE = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID,
                   //FK_IDSITE = (int) this.Txt_CodeSite.Tag,
                   Email=txt_email.Text,
                  ContratAbonnement=txt_ContactAbonne.Text,
                  ContratBranchement=txt_contarBrachement.Text,
                  Porte=txt_porte.Text,
                  Telephone=txt_telephone.Text,
                  Ordre="01",
                 

                };
                listObjetForInsertOrUpdate.Add(sClientFraude);

                return listObjetForInsertOrUpdate;
            }
            catch (Exception ex)
            {
                // Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.EtatDuCompteur);
                return null;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!VerifieChampObligation())
                    return;
                var messageBox = new MessageBoxControl.MessageBoxChildWindow(Langue.Fraude, Langue.QuestionEnregistrerDonnees, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                messageBox.OnMessageBoxClosed += (_, result) =>
                {
                    if (messageBox.Result == MessageBoxResult.OK)
                    {
                        listForInsertOrUpdate = GetInformationsFromScreen();
                        var service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));

                        if (listForInsertOrUpdate != null && listForInsertOrUpdate.Count > 0)
                        {
                            service.InsertClientFraudeCompleted += (snder, insertR) =>
                            {
                                if (insertR.Cancelled ||
                                    insertR.Error != null)
                                {
                                    Message.ShowError(insertR.Error.Message, Langue.Fraude);
                                    return;
                                }
                              
                                    MyEventArg.Data = insertR.Result;
                                    OnEvent(MyEventArg);
                                    this.DialogResult = false;
                                return;
                            };
                            service.InsertClientFraudeAsync(listForInsertOrUpdate);
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
                Message.Show(ex.Message, Langue.ErreurInsertionDonnees);
            }
        }

 
        public string GetNumDevis(List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentrePerimetre)
        {
            string NumeroDepartDevis = "1";
            try
            {
                string pNumeroDevis = string.Empty;
                int idCentre = (int)UserConnecte.FK_IDCENTRE;
                var devisenBase = LstCentrePerimetre.FirstOrDefault(t => t.PK_ID == idCentre);
                if (devisenBase != null && !string.IsNullOrWhiteSpace(devisenBase.CODE))
                {
                     int NUMERODEMANDE = Int32.Parse(devisenBase.NUMDEM);
                    string CodeSite = devisenBase.CODE;
                    int numDevis = (NUMERODEMANDE + 1);
                    if (numDevis > 0)
                    {
                        pNumeroDevis = numDevis.ToString().PadLeft(7, '0');
                        NumeroDepartDevis = devisenBase.CODE + CodeSite + pNumeroDevis;
                    }
                    NUMERODEMANDE++;

                }

                return NumeroDepartDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void txt_refclient_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_IdentUnique.Text != null )
            {
                FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
                service.RetourneClientFraudeCompleted += (snder, insertR) =>
                {
                    if (insertR.Cancelled || insertR.Error != null)
                    {
                        Message.ShowError(insertR.Error.Message, Langue.Fraude);
                        return;
                    }
                    if (insertR.Result ==null)
                    {
                        Message.ShowError(Langue.ErreurInsertionDonnees, Langue.Fraude);
                        return;
                    }
                    if (insertR.Result !=null)
                    {
                        foreach (CsClientFraude item in insertR.Result)
                        {
                        txt_Nom.Text = item.Nomabon;
                        txt_Nom.Tag = item.PK_ID;
                        txt_refclient.Text=item.Client;
                        txt_IdentUnique.Text=item.IdentificationUnique;
                        txt_email.Text=item.Email;
                        txt_ContactAbonne.Text=item.ContratAbonnement;
                        txt_contarBrachement.Text=item.ContratBranchement;
                        txt_porte.Text=item.Porte;
                        txt_telephone.Text = item.Telephone;
                        }
                    }
                };
                service.RetourneClientFraudeAsync(txt_IdentUnique.Text,(int) UserConnecte.FK_IDCENTRE);
            }

            else
            {
                return;
            }
        }

        private void txt_Nom_LostFocus(object sender, RoutedEventArgs e)
        {
            txt_refclient.Text = GetNumDevis(_listeDesCentreExistant);

        }
        private bool VerifieChampObligation()
        {
            try
            {
                bool ReturnValue = true;



                #region information abonnement

                if (string.IsNullOrEmpty(this.txt_Nom.Text))
                    throw new Exception("Remplir le champs Nom ");

                if (string.IsNullOrEmpty(this.txt_IdentUnique.Text))
                    throw new Exception("remplir le champs Identification Unique ");

                if (string.IsNullOrEmpty(this.txt_email.Text))
                    throw new Exception("remplir le champs Email ");

                if (string.IsNullOrEmpty(this.txt_ContactAbonne.Text))
                    throw new Exception("remplir le champs Contact Abonne ");

                if (string.IsNullOrEmpty(this.txt_contarBrachement.Text))
                    throw new Exception("remplir contact Brachement ");


                if (string.IsNullOrEmpty(this.txt_telephone.Text))
                    throw new Exception("remplir telephone ");

                //if (string.IsNullOrEmpty(this.txt_porte.Text))
                //    throw new Exception("remplir le champs porte ");

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
                #region Adresse géographique

                if (Cbo_Centre.SelectedItem == null)
                    throw new Exception("Selectionnez Centre");

                if (Cbo_Quartier.SelectedItem == null)
                    throw new Exception("Selectionnez Centre");


                if (Cbo_Commune.SelectedItem == null)
                    throw new Exception("Selectionnez Centre");


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
                //this.BtnTRansfert.IsEnabled = true;
                this.OKButton.IsEnabled = true;
                Message.ShowInformation(ex.Message, "Accueil");
                return false;
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

        private void RemplirCommuneParCentre(Galatee.Silverlight.ServiceAccueil.CsCentre centre)
        {
            try
            {

                if (_listeDesCommuneExistant != null && _listeDesCommuneExistant.Count > 0)
                    _listeDesCommuneExistantCentre = _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID )!= null ? _listeDesCommuneExistant.Where(c => c.FK_IDCENTRE == centre.PK_ID).ToList() : new List<Galatee.Silverlight.ServiceAccueil.CsCommune>();
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

        private void RemplirQuartier(int pCommuneId)
        {
            List<Galatee.Silverlight.ServiceAccueil.CsQuartier> ListeQuartierFiltres = new List<Galatee.Silverlight.ServiceAccueil.CsQuartier>();
            List<Galatee.Silverlight.ServiceAccueil.CsQuartier> QuartierParDefaut = null;
            _listeDesQuartierExistant = SessionObject.LstQuartier;

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
            _listeDesRuesExistant = SessionObject.LstRues;
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
    }
}

