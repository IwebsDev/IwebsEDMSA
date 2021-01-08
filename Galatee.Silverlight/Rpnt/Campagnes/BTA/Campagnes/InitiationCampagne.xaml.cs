using Galatee.Silverlight.ServiceRecouvrement;
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
//using Galatee.Silverlight.ServiceRpnt;

namespace Galatee.Silverlight.Recouvrement
{
    public partial class InitiationCampagne : ChildWindow
    {
        #region Variables

            int handler = 0;
            private Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA campagne;
            private bool IsConsultation;

            public List<ServiceAccueil.CsCentre> lesCentre { get; set; }
            public List<ServiceAccueil.CsSite> lesSite { get; set; }
            public List<ServiceAccueil.CsCentre> _listeDesCentreExistant { get; set; }

        #endregion

        #region Constructeur

            public InitiationCampagne()
            {
                InitializeComponent();
                LoadUI(); 
                //LoadCampagne();
            }
            public InitiationCampagne(Galatee.Silverlight.Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA campagne,bool IsConsultation)
            {
                InitializeComponent();
                InitialisationDesVariablesLocales(campagne, IsConsultation);
                LoadUI();
                //LoadCampagne();
            }

        #endregion

        #region Services

           
            private void LoadCampagne()
            {
                //if (!(SessionObject.campagne.Count() > 0))
                //{
                    //RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
                    //service.GetCampagneBTAControleAsync();
                    //service.GetCampagneBTAControleCompleted += (er, res) =>
                    //{
                    //    try
                    //    {
                    //        if (res.Error != null || res.Cancelled)
                    //            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                    //        else
                    //            if (res.Result != null)
                    //            {
                    //                SessionObject.campagne = res.Result;
                    //            }
                    //            else
                    //                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                    //                    "Erreur");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw ex;
                    //    }
                    //};
                //}

            }
            private void SaveCampagne()
            {
                if (string.IsNullOrWhiteSpace(txtcampagne.Text) || Cbo_Centre.SelectedItem==null || dtpdatedebut.SelectedDate==null || dtpdatefinprevu.SelectedDate==null)
                {
                     Message.Show("Tous les champs son obligatoire",
                                     "Notification");
                }
                else
                {
                    RecouvrementServiceClient service = new RecouvrementServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Recouvrement"));
                    handler = LoadingManager.BeginLoading("Savegarde des données ...");

                    CsCampagnesBTAAccessiblesParLUO CampBAT = new CsCampagnesBTAAccessiblesParLUO
                    {
                         //Campagne_ID=Guid.Parse("6F9619FF-8B86-D011-B42D-00C04FC964FA"),
                        Campagne_ID = Guid.NewGuid(),
                        CodeCentre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).CODE.ToString(),
                        DateCreation = dtpdatedreation.SelectedDate.Value,
                        DateDebutControles = dtpdatedebut.SelectedDate.Value,
                        DateFinPrevue = dtpdatefinprevu.SelectedDate.Value,
                        DateModification = DateTime.UtcNow.Date,
                        Libelle_Campagne = txtcampagne.Text,
                        MatriculeAgentCreation = UserConnecte.matricule,
                        MatriculeAgentDerniereModification = UserConnecte.matricule,
                        NbreElements = 0,
                        Statut_ID = 1,
                        fk_idCentre = ((Galatee.Silverlight.ServiceAccueil.CsCentre)Cbo_Centre.SelectedItem).PK_ID
                    };

                    service.InsertCampagneBTAAsync(CampBAT);
                    service.InsertCampagneBTACompleted += (er, res) =>
                    {
                        try
                        {
                            if (res.Error != null || res.Cancelled)
                                Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                            else
                            {
                                if (res.Result != false)
                                {
                                    Message.Show("L'enregistrement c'est bien passé",
                                         "Notification");
                                    //CsTBCAMPAGNECONTROLEBTA camp = new CsTBCAMPAGNECONTROLEBTA
                                    //{
                                    //    CAMPAGNE_ID = CampBAT.Campagne_ID,
                                    //    CODECENTRE = int.Parse(CampBAT.CodeCentre),
                                    //    CODEEXPLOITATION = CampBAT.CodeCentre,
                                    //    DATECREATION = CampBAT.DateCreation,
                                    //    DATEDEBUTCONTROLES = CampBAT.DateDebutControles,
                                    //    DATEFINPREVUE = CampBAT.DateFinPrevue,
                                    //    DATEMODIFICATION = CampBAT.DateModification,
                                    //    LIBELLE_CAMPAGNE = CampBAT.Libelle_Campagne,
                                    //    LIBELLECENTRE = ((CsCentre)cbxexploitation.SelectedItem).LIBELLE,
                                    //    LIBELLEEXPLOITATION = ((CsCentre)cbxexploitation.SelectedItem).LIBELLE,
                                    //    LISTEBRANCHEMENT = new List<CsBrt>(),
                                    //    LISTELOT = new List<CsTBLOTDECONTROLEBTA>(),
                                    //    MATRICULEAGENTCREATION = CampBAT.MatriculeAgentCreation,
                                    //    MATRICULEAGENTDERNIEREMODIFICATION = CampBAT.MatriculeAgentDerniereModification,
                                    //    METHODE = new CsREFMETHODEDEDETECTIONCLIENTSBTA(),
                                    //    NBREELEMENTS = 0,
                                    //    NBRLOTS = 0,
                                    //    PERIODE = string.Empty,
                                    //    POULATIONNONAFFECTES = 0,
                                    //    STATUT = string.Empty,
                                    //    STATUT_ID = int.MinValue

                                    //};
                                    //SessionObject.campagne.Add(camp);
                                }
                                else
                                {
                                    Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                                         "Erreur");
                                }
                            }
                            LoadingManager.EndLoading(handler);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    };
                }
            }
            //private void LoadExploitation()
            //{
               
            //        if (SessionObject.LstCentre.Count > 0)
            //        {

            //            cbxexploitation.ItemsSource = SessionObject.LstCentre;
            //            cbxexploitation.DisplayMemberPath = "LIBELLE";
            //            cbxexploitation.SelectedValuePath = "CODE";
            //            OKButton.IsEnabled = true;
            //        }
            //        else
            //        {
            //            LoadCentre();
            //        }
                
            //}
            void LoadExploitation()
            {
                try
                {

                    if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                    {
                        lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                        lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                        _listeDesCentreExistant = lesCentre;
                        RemplirCentrePerimetre(lesCentre, lesSite);
                        return;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
                    service.ListeDesDonneesDesSiteAsync(false);
                    service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                    {
                        try
                        {
                            if (args != null && args.Cancelled)
                                return;
                            SessionObject.LstCentre = args.Result;
                            if (SessionObject.LstCentre.Count != 0)
                            {
                                lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                                lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                                _listeDesCentreExistant = lesCentre;
                                RemplirCentrePerimetre(lesCentre, lesSite);
                            }
                            else
                            {
                                Message.ShowInformation("Aucun site trouvé en base.", "Erreur");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Message.ShowError(ex, "Erreur");
                        }
                    };
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            }

            private void RemplirCentrePerimetre(List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentre, List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite)
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

                        if (lstSite != null)
                            foreach (var item in lstSite)
                            {
                                Cbo_Site.Items.Add(item);
                            }
                        Cbo_Site.SelectedValuePath = "PK_ID";
                        Cbo_Site.DisplayMemberPath = "LIBELLE";

                        if (lstSite != null && lstSite.Count == 1)
                            Cbo_Site.SelectedItem = lstSite.First();

                        RenseignerLesChanmp();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private void RemplirCentreDuSite(int pIdSite, int pIdcentre)
            {
                try
                {
                    Cbo_Centre.Items.Clear();
                    if (_listeDesCentreExistant != null &&
                        _listeDesCentreExistant.Count != 0)
                    {
                        List<Galatee.Silverlight.ServiceAccueil.CsCentre> lesCentreDuPerimetreAction = _listeDesCentreExistant.Where(p => p.FK_IDCODESITE == pIdSite).ToList();
                        if (lesCentreDuPerimetreAction != null)
                            foreach (var item in lesCentreDuPerimetreAction)
                            {
                                Cbo_Centre.Items.Add(item);
                            }
                        //Cbo_Centre.ItemsSource = lesCentreDuPerimetreAction;
                        Cbo_Centre.SelectedValuePath = "PK_ID";
                        Cbo_Centre.DisplayMemberPath = "LIBELLE";

                        if (pIdcentre != 0)
                            this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First(t => t.PK_ID == pIdcentre);
                        if (_listeDesCentreExistant.Count == 1)
                            this.Cbo_Centre.SelectedItem = _listeDesCentreExistant.First();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            private void LoadCentre()
            {
                //RpntServiceClient service = new RpntServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Rpnt"));
                //handler = LoadingManager.BeginLoading("Recuperation des données ...");
                //CsREFUNITEORGANISATIONNELLE UO = new CsREFUNITEORGANISATIONNELLE();

                //service.GetExploitationByUOAsync(UO);
                //service.GetExploitationByUOCompleted += (er, res) =>
                //{
                //    try
                //    {
                //        if (res.Error != null || res.Cancelled)
                //            Message.Show("Erreur dans le traitement des méthode de dectection : " + res.Error.InnerException.ToString(), "Erreur");
                //        else
                //        {
                //            if (res.Result != null)
                //            {
                //                SessionObject.centre = res.Result;
                //                cbxexploitation.ItemsSource = SessionObject.centre;
                //                cbxexploitation.DisplayMemberPath = "LIBELLE";
                //                OKButton.IsEnabled = true;
                //                //cbxexploitation.SelectedValuePath = "CODEEXPLOITATION";
                //            }
                //            else
                //            {
                //                Message.Show("Une erreur s'est produite, veuillez consultez le journal des erreurs",
                //                     "Erreur");
                //            }
                //        }
                //        LoadingManager.EndLoading(handler);
                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }
                //};
            }

        #endregion

        #region Event Handler

            private void OKButton_Click(object sender, RoutedEventArgs e)
            {
                SaveCampagne();
                this.DialogResult = true;
            }
            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                LoadingManager.EndLoading(handler);
                this.DialogResult = false;
            }


            private void Cbo_Site_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    if (this.Cbo_Site.SelectedItem != null)
                    {
                        var csSite = Cbo_Site.SelectedItem as Galatee.Silverlight.ServiceAccueil.CsSite;
                        if (csSite != null)
                        {
                            this.txtSite.Text = csSite.CODE ?? string.Empty;

                            RemplirCentreDuSite(csSite.PK_ID, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
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

                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                }
            }
        #endregion

        #region Methode UI
            private void LoadUI()
            {
               
                InitialisationDesControleUI();
                //OKButton.IsEnabled = false;
                LoadExploitation();

            }

            private void InitialisationDesControleUI()
            {
                Cbo_Site.IsEnabled = !this.IsConsultation;
                Cbo_Centre.IsEnabled = !this.IsConsultation;

                dtpdatedreation.IsEnabled = false;
                txtcampagne.IsReadOnly = this.IsConsultation;

                dtpdatedebut.IsEnabled = !this.IsConsultation;
                dtpdatefinprevu.IsEnabled = !this.IsConsultation;

                OKButton.IsEnabled = !this.IsConsultation;
            }
            private void InitialisationDesVariablesLocales(Rpnt.ViewsModels.CsTBCAMPAGNECONTROLEBTA campagne, bool IsConsultation)
            {
                this.campagne = campagne;
                this.IsConsultation = IsConsultation;
            }
            private void RenseignerLesChanmp()
            {
                if (this.campagne != null)
                {
                    ServiceAccueil.CsCentre CentreDeLaCampagne = lesCentre.Single(s => s.PK_ID == int.Parse(this.campagne.CODEEXPLOITATION));
                    ServiceAccueil.CsSite SiteDeLaCampagne = lesSite.Single(s => s.PK_ID == CentreDeLaCampagne.FK_IDCODESITE);
                    
                    this.Cbo_Site.SelectedItem = SiteDeLaCampagne;
                    this.Cbo_Centre.SelectedItem = CentreDeLaCampagne;

                    dtpdatedreation.SelectedDate = this.campagne.DATECREATION;
                    txtcampagne.Text = this.campagne.LIBELLE_CAMPAGNE;

                    dtpdatedebut.SelectedDate = this.campagne.DATEDEBUTCONTROLES;
                    dtpdatefinprevu.SelectedDate = this.campagne.DATEFINPREVUE;
                }
                else
                {
                    dtpdatedreation.Text = DateTime.UtcNow.Date.ToString();
                }
            }

        #endregion

    }
}

