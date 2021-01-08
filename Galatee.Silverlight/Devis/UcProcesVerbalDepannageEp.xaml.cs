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
using Galatee.Silverlight.ServiceAccueil ;
using Galatee.Silverlight.Resources.Devis;
using System.IO;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcProcesVerbalDepannageEp : ChildWindow
    {
        public ObjDEVIS DevisSelectionne { get; set; }
        public List<ObjELEMENTDEVIS> Elements { get; set; }
        public ObjTRAVAUXDEVIS Travaux { get; set; }
        public string ProcesVerbal { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();
        private CsDemande laDetailDemandeRechercher = new CsDemande();
        public DateTime? dtp_DateDepose = new DateTime();
        ObservableCollection<ObjELEMENTDEVIS> donnesDatagrid = new ObservableCollection<ObjELEMENTDEVIS>();
        private List<ObjELEMENTDEVIS> ListeFournitureExistante = null;
        public List<ObjELEMENTDEVIS> MyElements { get; set; }
        List<CsOrganeScellable> ListeOrganeScellable = new List<CsOrganeScellable>();
        List<CsOrganeScelleDemande> ListeOrganeScelleDemande = new List<CsOrganeScelleDemande>();
        List<CsScelle> ListeScelle = new List<CsScelle>();
        List<CsScelle> ListeScelleToRemove = new List<CsScelle>();
        List<CsTypePanne> lstTypeDetailPanne = new List<CsTypePanne>();

        public UcProcesVerbalDepannageEp()
        {
            InitializeComponent();
        }
        public UcProcesVerbalDepannageEp(int pIdDevis)
        {
            try
            {
                InitializeComponent();
                ChargeDetailDEvis(pIdDevis);
                ChargerDonneeDuSite();
                RemplirListeMateriel();
                RemplirTypePanne();
                RemplirVehiculeDePannage();
                this.tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                this.tabSaisieIndex.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemFacture.Visibility = System.Windows.Visibility.Collapsed;
                this.tabItemClient.Visibility = System.Windows.Visibility.Collapsed;
                //this.rbt_DepMT.Visibility = System.Windows.Visibility.Collapsed;
                //this.rbt_DepConventionnel.Visibility = System.Windows.Visibility.Collapsed;
                //this.rbt_DepPrepaid.Visibility = System.Windows.Visibility.Collapsed;
                //this.rbt_DepReseau.Visibility = System.Windows.Visibility.Collapsed;
                //this.rbt_DepEp.Visibility = System.Windows.Visibility.Collapsed;

                this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Collapsed;
                this.TxtOrdre.Visibility = System.Windows.Visibility.Collapsed;
                this.label_Reference.Visibility = System.Windows.Visibility.Collapsed;
                btn_RechercheClient.Visibility = System.Windows.Visibility.Collapsed;

                this.label_NomClient.Visibility = System.Windows.Visibility.Visible;
                //txt_nomClient.Visibility = System.Windows.Visibility.Collapsed;

                this.label_NumVehicule.Visibility = System.Windows.Visibility.Collapsed;
                txt_NumeroVehicule.Visibility = System.Windows.Visibility.Collapsed;


            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        List<int> lesCentrePerimetre = new List<int>();

        private void ChargerDonneeDuSite()
        {
            try
            {
                SessionObject.ModuleEnCours = "Accueil";
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count > 0)
                {
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(o=>o.CODE).ToList());
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(o => o.CODE).ToList());

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirInfoDepannage(CsDepannage leDepannage)
        {
            try
            {
                txt_ModeRecueil1.Text = string.IsNullOrEmpty(leDepannage.MODERECUEIL) ? string.Empty : leDepannage.MODERECUEIL;
                txt_Commune1 .Text = string.IsNullOrEmpty(leDepannage.LACOMMUNE ) ? string.Empty : leDepannage.LACOMMUNE ;
                txt_Quartier1.Text = string.IsNullOrEmpty(leDepannage.LEQUARTIER) ? string.Empty : leDepannage.LEQUARTIER;
                txt_NumSecteur1.Text = string.IsNullOrEmpty(leDepannage.LESECTEUR) ? string.Empty : leDepannage.LESECTEUR;
                txt_NumRue1.Text = string.IsNullOrEmpty(leDepannage.LARUE) ? string.Empty : leDepannage.LARUE;
                txt_TypePanne.Text = string.IsNullOrEmpty(leDepannage.TYPEDEPANNE) ? string.Empty : leDepannage.TYPEDEPANNE;
                Txt_Commentaire1.Text = string.IsNullOrEmpty(leDepannage.DESCRIPTIONPANNE) ? string.Empty : leDepannage.DESCRIPTIONPANNE;
                Txt_Nom_Declarant.Text = string.IsNullOrEmpty(leDepannage.NOM_DECLARANT) ? string.Empty : leDepannage.NOM_DECLARANT;
                Txt_Porte1.Text = string.IsNullOrEmpty(leDepannage.PORTE) ? string.Empty : leDepannage.PORTE;
                chk_Est_Client.IsChecked = !leDepannage.ISPERSONNEEXTERIEUR ;
                RemplirTypeDetailPanne(leDepannage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirTypePanne()
        {
            try
            {
                List<CsTypePanne> lstTypePanne = new List<CsTypePanne>();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneTypePanneAsync();
                service.RetourneTypePanneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstTypePanne = args.Result;

                    //Cbo_TypedePanne.ItemsSource = lstTypePanne.OrderBy(t => t.LIBELLE).ToList();
                    //Cbo_TypedePanne.IsEnabled = true;
                    //Cbo_TypedePanne.DisplayMemberPath = "LIBELLE";

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirVehiculeDePannage()
        {
            try
            {
                List<CsVehicule> lstVehicule = new List<CsVehicule>();
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneVehiculeAsync ();
                service.RetourneVehiculeCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstVehicule = args.Result;

                    Cbo_Vehicule .ItemsSource = lstVehicule.OrderBy(t => t.LIBELLE).ToList();
                    Cbo_Vehicule.IsEnabled = true;
                    Cbo_Vehicule.DisplayMemberPath = "LIBELLE";

                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RemplirTypeDetailPanne(CsDepannage leDepannage)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneDetailTypePanneAsync();
                service.RetourneDetailTypePanneCompleted += (s, args) =>
                {
                    if (args.Error != null && args.Cancelled)
                        return;
                    lstTypeDetailPanne = args.Result;
                    Cbo_TypeDetaildePanne.ItemsSource = lstTypeDetailPanne.Where(u => u.ID_TYPE_RECLAMATION == leDepannage.FK_IDTYPEDEPANNE).OrderBy(t => t.LIBELLE).ToList();
                    Cbo_TypeDetaildePanne.IsEnabled = true;
                    Cbo_TypeDetaildePanne.DisplayMemberPath = "LIBELLE";
                    Cbo_TypeDetaildePanne.SelectedItem = null;
                };
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.GetDevisByNumIdDevisCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    laDetailDemande = args.Result;
                    laDemandeSelect = laDetailDemande.LaDemande;
                    this.Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemandeSelect.NUMDEM) ? laDemandeSelect.NUMDEM : string.Empty;
                    if (laDetailDemande.Depannage != null)
                        RemplirInfoDepannage(laDetailDemande.Depannage);
                    this.tabControle1.SelectedItem = this.tabItemInfoDemande;
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageEp)
                        rbt_DepEclairagePublic.Visibility = System.Windows.Visibility.Collapsed;
                    ShowControleDeposeCompteur(true);
                }
            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }

        private void ShowControleDeposeCompteur(bool p)
        {
            Visibility State=p?Visibility.Visible:Visibility.Collapsed;
            
        }

 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChkRemiseEnStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Convert.ToBoolean(ChkRemiseEnStock.IsChecked))
                //{
                //    UcRemiseEnStock form = new UcRemiseEnStock(laDetailDemande);
                //    form.Closed += new EventHandler(formRemiseEnStock_Closed);
                //    form.Show();
                //}
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void formRemiseEnStock_Closed(object sender, EventArgs e)
        {
            try
            {
                var form = (UcRemiseEnStock)sender;
                if (form != null)
                {
                    Elements = form.Elements;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DtpDebutTravaux_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                {
                    if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                    {
                        if (DtpDebutTravaux.SelectedDate.Value.Date > DtpFinTravaux.SelectedDate.Value.Date)
                            throw new Exception("La date de début de travaux ne peut pas être supérieur à la date de fin travaux !");
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                DtpDebutTravaux.ClearValue(DatePicker.SelectedDateProperty);
                DtpDebutTravaux.Focus();
            }
        }

        private void DtpFinTravaux_CalendarClosed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DtpDebutTravaux.SelectedDate != null && DtpDebutTravaux.SelectedDate.Value != null)
                {
                    if (DtpFinTravaux.SelectedDate != null && DtpFinTravaux.SelectedDate.Value != null)
                    {
                        if (DtpFinTravaux.SelectedDate.Value.Date < DtpDebutTravaux.SelectedDate.Value.Date)
                            throw new Exception("La date de début de travaux ne peut pas être inférieur à la date de fin travaux !");
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                DtpFinTravaux.ClearValue(DatePicker.SelectedDateProperty);
                DtpFinTravaux.Focus();
            }
        }

        private void Cbo_DiametreCompteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }
        private void btn_Rejeter_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
        }

        private ObjDOCUMENTSCANNE SaveFile(byte[] pStream, int pTypeDocument, ObjDOCUMENTSCANNE pDocumentScane)
        {
            try
            {
                //Récupération du contenu.
                if (pDocumentScane == null)
                {
                    pDocumentScane = new ObjDOCUMENTSCANNE { CONTENU = pStream, PK_ID = Guid.NewGuid(), DATECREATION = DateTime.Now, USERCREATION = UserConnecte.matricule };
                    pDocumentScane.OriginalPK_ID = pDocumentScane.PK_ID;
                    pDocumentScane.ISNEW = true;
                }
                else
                    pDocumentScane.CONTENU = pStream;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
            return pDocumentScane;
        }
        private void GetInformationFromChildWindowImageAutorisation(object sender, EventArgs e)
        {
            try
            {
                var form = (UcImageScanne)sender;
                if (form != null)
                {
                    if (form.DialogResult == true /*&& form.ImageScannee != null*/)
                    {
                        //this.lnkLetter.Tag = form.ImageScannee;
                        //SaveFile(form.ImageScannee, (int)SessionObject.TypeDocumentScanneDevis.Lettre);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }
        private void RemplirListeMateriel()
        {
            try
            {
                AcceuilServiceClient Serviceclient = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                Serviceclient.SelectAllMaterielCompleted += (ss, bc) =>
                {
                    try
                    {
                        if (bc.Cancelled || bc.Error != null)
                        {
                            string error = bc.Error.Message;
                            if (LayoutRoot != null)
                                LayoutRoot.Cursor = Cursors.Arrow;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (bc.Result != null)
                            ListeFournitureExistante = bc.Result;
                            
                    }
                    catch (Exception ex)
                    {
                        if (LayoutRoot != null)
                            LayoutRoot.Cursor = Cursors.Arrow;
                        Message.ShowError(ex.Message, Languages.txtDevis);
                    }
                };
                Serviceclient.SelectAllMaterielAsync();

            }
            catch (Exception ex)
            {
                if (LayoutRoot != null)
                    LayoutRoot.Cursor = Cursors.Arrow;
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        private void btn_Devis_Click(object sender, RoutedEventArgs e)
        {
            var MyLstFourniture = this.ListeFournitureExistante;
            if (MyLstFourniture != null)
            {
                UcListeDesignation frm = new UcListeDesignation(this.ListeFournitureExistante, MyElements);
                if (frm != null)
                {
                    frm.Closed += new EventHandler(frm_Closed);
                    frm.Show();
                }
            }
            else
            {
                Message.ShowInformation("Aucun élément de fourniture coreespondant", "Information");
            }
        }
        void frm_Closed(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var form = ((UcListeDesignation)sender);
                    if (form != null && form.DialogResult.Value)
                    {
                        this.MyElements = form.MyElements;
                        this.dataGridElementDevis.ItemsSource = null;
                        this.dataGridElementDevis.ItemsSource = this.MyElements;
                        if (this.MyElements != null && this.MyElements.Count != 0)
                        {
                            this.txt_MontantHt.Text = this.MyElements.Sum(t => t.MONTANTHT).Value .ToString(SessionObject.FormatMontant);
                            this.txt_MontantTaxe.Text = this.MyElements.Sum(t => t.MONTANTTAXE).Value .ToString(SessionObject.FormatMontant);
                            this.txt_MontantTTc.Text = this.MyElements.Sum(t => t.MONTANTTTC).Value .ToString(SessionObject.FormatMontant);
                        }
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Transmetre_Click(object sender, RoutedEventArgs e)
        {
            EnregisterOuTransmetre(true);
        }
        private void EnregisterOuTransmetre(bool IsEnregister)
        {
            try
            {
               
                if (DtpDebutTravaux.SelectedDate == null || DtpFinTravaux.SelectedDate == null)
                    throw new Exception("Veuillez renseigner les dates de debute et de fin de travaux");
                if ((DateTime.Parse(DtpFinTravaux.SelectedDate.Value.ToString())) < DateTime.Parse(DtpDebutTravaux.SelectedDate.Value.ToString()))
                    throw new Exception("Date de fin inférieure à la date début travaux");
                //if (this.Cbo_TypedePanne.SelectedItem ==null )
                //    throw new Exception("Sélectionner le type de panne");

                if ((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageClient  ||
                    laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannagePrepayer) &&
                    rbt_DepConventionnel.IsChecked == true)
                {
                    if (laDetailDemandeRechercher == null)
                        throw new Exception("Veuillez rehercher le client");

                    if (dg_compteur.ItemsSource == null)
                        throw new Exception("Veuillez selectionner le nouveau compteur");

                    if (laDetailDemande.LstCanalistion != null && laDetailDemandeRechercher.LstCanalistion.Count != 0)
                        laDetailDemandeRechercher.LstCanalistion.Clear();
                    laDetailDemande.LstCanalistion = (List<CsCanalisation>)dg_compteur.ItemsSource;
                }
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageEp &&
                  IsFactureAfaire.IsChecked == true && laDetailDemande.Depannage.ISPERSONNEEXTERIEUR == true  )
                { 
                     if (string.IsNullOrEmpty( this.txt_nomClient.Text))
                        throw new Exception("Veuillez saisir le nom du client");
                }

                if (laDetailDemande.TravauxDevis == null)
                    laDetailDemande.TravauxDevis = new ObjTRAVAUXDEVIS();

                laDetailDemande.TravauxDevis.FK_IDDEVIS = laDetailDemande.LaDemande.PK_ID;
                //laDetailDemande.TravauxDevis.FK_IDTYPEDEPANNE = ((CsTypePanne)this.Cbo_TypedePanne.SelectedItem).ID; 
                laDetailDemande.TravauxDevis.NUMDEVIS = laDetailDemande.LaDemande.NUMDEM;
                laDetailDemande.TravauxDevis.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                laDetailDemande.TravauxDevis.ORDRE = int.Parse(laDetailDemande.LaDemande.ORDRE);
                laDetailDemande.TravauxDevis.PROCESVERBAL  =string.IsNullOrEmpty(this.TxtCommentaire.Text)? string .Empty : this.TxtCommentaire.Text;
                laDetailDemande.TravauxDevis.DATEDEBUTTRVX = Convert.ToDateTime(this.DtpDebutTravaux.SelectedDate.Value);
                laDetailDemande.TravauxDevis.DATEFINTRVX = Convert.ToDateTime(this.DtpFinTravaux.SelectedDate.Value);
                laDetailDemande.TravauxDevis.DATECREATION = System.DateTime.Today.Date ;
                laDetailDemande.TravauxDevis.USERCREATION = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.MATRICULECHEFEQUIPE = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.MATRICULEREGLEMENT = UserConnecte.matricule;
                laDetailDemande.TravauxDevis.NOMCHEFEQUIPE = UserConnecte.nomUtilisateur;
                laDetailDemande.TravauxDevis.DATEREGLEMENT = System.DateTime.Today;
                laDetailDemande.TravauxDevis.DATEPREVISIONNELLE = System.DateTime.Today;

               
                if (this.MyElements != null && this.MyElements.Count != 0)
                {
                    if (laDetailDemande.EltDevis == null)
                        laDetailDemande.EltDevis = new List<ObjELEMENTDEVIS>();
                    laDetailDemande.EltDevis = this.MyElements;
                    laDetailDemande.EltDevis.ForEach(el => el.NUMDEM = this.laDetailDemande.LaDemande.NUMDEM);
                    laDetailDemande.EltDevis.ForEach(el => el.FK_IDDEMANDE = this.laDetailDemande.LaDemande.PK_ID);
                    laDetailDemande.EltDevis.ForEach(el => el.USERCREATION = UserConnecte.matricule);
                    laDetailDemande.EltDevis.ForEach(el => el.DATECREATION = System.DateTime.Now);
                    #region Depannage MT && EP
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageMT ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageEp )
                    {
                        List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
                        if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
                        {
                            int idTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                            ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDetailDemande.LaDemande.FK_IDCENTRE);

                            CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();

                            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageMT)
                            {
                                leCoutduDevis.CENTRE = laDetailDemandeRechercher.Abonne.CENTRE;
                                leCoutduDevis.CLIENT = laDetailDemandeRechercher.Abonne.CLIENT;
                                leCoutduDevis.ORDRE = laDetailDemandeRechercher.Abonne.ORDRE;
                                leCoutduDevis.FK_IDCENTRE = laDetailDemandeRechercher.Abonne.FK_IDCENTRE;
                            }
                            else
                            {
                                if (laDetailDemande.Depannage.ISPERSONNEEXTERIEUR == true || 
                                    laDetailDemande.Depannage.ISEDM  == true  )
                                {
                                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(SessionObject.Enumere.Generale) ? null : SessionObject.Enumere.Generale;
                                    leCoutduDevis.CLIENT = string.IsNullOrEmpty("00000000000") ? null : "00000000000";
                                    leCoutduDevis.ORDRE = "00";
                                    leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                                    laDetailDemande.EltDevis.ForEach(el => el.NOM = string.IsNullOrEmpty(this.txt_nomClient.Text)? string.Empty :this.txt_nomClient.Text );
                                }
                                else
                                {
                                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(SessionObject.Enumere.Generale) ? null : SessionObject.Enumere.Generale;
                                    leCoutduDevis.CLIENT = string.IsNullOrEmpty(leCentre.COMPTEECLAIRAGEPUBLIC) ? null : leCentre.COMPTEECLAIRAGEPUBLIC;
                                    leCoutduDevis.FK_IDCENTRE  = string.IsNullOrEmpty(leCentre.COMPTEECLAIRAGEPUBLIC ) ? 0 : leCentre.PK_ID ;
                                    leCoutduDevis.ORDRE = "01";
                                }
                            }
                            leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                            leCoutduDevis.COPER = SessionObject.Enumere.CoperFactureTrvxEtDivers;
                            leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperFactureTrvxEtDivers).PK_ID;
                            leCoutduDevis.FK_IDTAXE = laDetailDemande.EltDevis.First().FK_IDTAXE.Value;
                            leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                            leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTHT));
                            leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTTAXE));
                            leCoutduDevis.REFEM = DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00");
                            leCoutduDevis.DATECREATION = DateTime.Now;
                            leCoutduDevis.USERCREATION = UserConnecte.matricule;
                            if (laDetailDemande.LstCoutDemande == null)
                            {
                                laDetailDemande.LstCoutDemande = new List<CsDemandeDetailCout>();
                                laDetailDemande.LstCoutDemande.Add(leCoutduDevis);
                            }
                            else
                                laDetailDemande.LstCoutDemande.Add(leCoutduDevis);


                            decimal montantTotal = laDetailDemande.EltDevis.Sum(t => (decimal)(t.MONTANTTTC));
                            foreach (ObjELEMENTDEVIS item in laDetailDemande.EltDevis)
                            {
                                CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageMT)
                                {
                                    LaRubriqueDevis.NOM = laDetailDemandeRechercher.LeClient.NOMABON;
                                    LaRubriqueDevis.PRODUIT = laDetailDemandeRechercher.Abonne.LIBELLEPRODUIT;
                                }
                                else
                                {
                                    LaRubriqueDevis.NOM = item.NOM;
                                    LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                                }

                                LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                                LaRubriqueDevis.TYPEDEMANDE = laDetailDemande.LaDemande.LIBELLETYPEDEMANDE;
                                LaRubriqueDevis.COMMUNUE = laDetailDemande.Depannage.LACOMMUNE;
                                LaRubriqueDevis.QUARTIER = laDetailDemande.Depannage.LEQUARTIER;
                                LaRubriqueDevis.NUMDEMANDE = laDetailDemande.LaDemande.NUMDEM;
                                LaRubriqueDevis.DESIGNATION = item.DESIGNATION;
                                LaRubriqueDevis.QUANTITE = Convert.ToDecimal(item.QUANTITE);
                                LaRubriqueDevis.PRIXUNITAIRE = item.PRIX_UNITAIRE.Value;
                                LaRubriqueDevis.MONTANTHT = (decimal)(item.MONTANTTTC);
                                LaRubriqueDevis.TOTALDEVIS = montantTotal;
                                LstDesRubriqueDevis.Add(LaRubriqueDevis);
                            }
                            Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "Devis", "Accueil", true);
                        }

                    }
                    #endregion
                }

                #region Sylla 17/04/2017
                //Depannage 
                laDetailDemande.Depannage.HEUREDEBUT = this.DtpDebutTravaux.SelectedDate != null ? this.DtpDebutTravaux.SelectedDate : null;
                laDetailDemande.Depannage.HEUREFIN = this.DtpFinTravaux.SelectedDate != null ? this.DtpFinTravaux.SelectedDate : null;
                laDetailDemande.Depannage.ISPROVISOIR = this.Rdb_IsProvisoire.IsChecked == true ? true : false;
                laDetailDemande.Depannage.ISDEFINITIF = this.Rdb_IsDéfinitif.IsChecked == true ? true : false;
                laDetailDemande.Depannage.NOM_CLIENT_DEPANE = txt_nomClient.Text;
                laDetailDemande.Depannage.SIEGE_DEFAUT = Txt_Siege_Defaut.Text;
                laDetailDemande.Depannage.CAUSE_DEFAUT = Txt_Cause_Defaut.Text;
                laDetailDemande.Depannage.POSTE = txt_Poste.Text;
                laDetailDemande.Depannage.ISRESEAU = (rbt_DepReseau.IsChecked==true || rbt_DepEclairagePublic.IsChecked==true)?true:false;
                laDetailDemande.Depannage.ISBRANCHEMENT = (rbt_DepConventionnel.IsChecked == true || rbt_DepMT.IsChecked == true || rbt_DepPrepaid.IsChecked == true) ? true : false;
                laDetailDemande.Depannage.DESCRIPTIONPANNE = TxtCommentaire.Text;
                //laDetailDemande.Depannage.DEPART = txt_depart.Text;
                if(Cbo_TypeDetaildePanne.SelectedItem!=null)
                    laDetailDemande.Depannage.FK_IDTYPEDEPANNE_TRAITE = ((CsTypePanne)(Cbo_TypeDetaildePanne.SelectedItem)).ID;
                if (this.Cbo_Vehicule.SelectedItem != null)
                {
                    laDetailDemande.Depannage.IMMATRICULATION = this.txt_MatriculeVehicule.Text;
                }
                //laDetailDemande.Depannage.ISPERSONNEEXTERIEUR = (rbt_DepConventionnel.IsChecked == true || rbt_DepMT.IsChecked == true || rbt_DepPrepaid.IsChecked == true) ? true : false;

                //
                #endregion

                this.DateFin = Convert.ToDateTime(this.DtpFinTravaux.SelectedDate.Value);
                    AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    clientDevis.ValiderDemandeCompleted += (ss, b) =>
                    {
                        this.btn_Transmetre.IsEnabled = false;
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        //if (IsEnregister && b.Result == true)
                        if (IsEnregister)
                        {
                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true, this);

                            #region DepannageConventionnel
                            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageClient  && rbt_DepConventionnel.IsChecked == true)
                            {
                                laDetailDemandeRechercher.LaDemande = laDetailDemande.LaDemande;
                                laDetailDemande.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ChangementCompteur).CODE;
                                laDetailDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ChangementCompteur).PK_ID;
                                laDetailDemandeRechercher.LeClient.DATECREATION = System.DateTime.Now;
                                laDetailDemandeRechercher.LeClient.USERCREATION = UserConnecte.matricule;
                                laDetailDemandeRechercher.LeClient.DATEMODIFICATION = null;
                                laDetailDemandeRechercher.LeClient.USERMODIFICATION = null;

                                laDetailDemandeRechercher.Branchement.DATECREATION = System.DateTime.Now;
                                laDetailDemandeRechercher.Branchement.USERCREATION = UserConnecte.matricule;
                                laDetailDemandeRechercher.Branchement.DATEMODIFICATION = null;
                                laDetailDemandeRechercher.Branchement.USERMODIFICATION = null;

                                laDetailDemandeRechercher.Abonne.DATECREATION = System.DateTime.Now;
                                laDetailDemandeRechercher.Abonne.USERCREATION = UserConnecte.matricule;
                                laDetailDemandeRechercher.Abonne.DATEMODIFICATION = null;
                                laDetailDemandeRechercher.Abonne.USERMODIFICATION = null;

                                laDetailDemandeRechercher.Ag.DATECREATION = System.DateTime.Now;
                                laDetailDemandeRechercher.Ag.USERCREATION = UserConnecte.matricule;
                                laDetailDemandeRechercher.Ag.DATEMODIFICATION = null;
                                laDetailDemandeRechercher.Ag.USERMODIFICATION = null;

                                laDetailDemandeRechercher.LaDemande.REGLAGECOMPTEUR = laDetailDemandeRechercher.LstCanalistion.First().REGLAGECOMPTEUR;
                                laDetailDemandeRechercher.LaDemande.FK_IDREGLAGECOMPTEUR = laDetailDemandeRechercher.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                                laDetailDemandeRechercher.LstCanalistion = null;
                                laDetailDemandeRechercher.LstEvenement = null;
                                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                                client.ValiderDemandeInitailisationCompleted += (ssd, bd) =>
                                {
                                    if (bd.Cancelled || bd.Error != null)
                                    {
                                        string error = bd.Error.Message;
                                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                        return;
                                    }
                                    string numDemande = string.Empty;
                                    string Client = string.Empty;
                                    string Retour = bd.Result;
                                    string[] coupe = Retour.Split('.');
                                    Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], laDetailDemandeRechercher.LaDemande.FK_IDCENTRE, coupe[1], laDetailDemandeRechercher.LaDemande.FK_IDTYPEDEMANDE);
                                    numDemande = coupe[1];
                                };
                                client.ValiderDemandeInitailisationAsync(laDetailDemandeRechercher);
                            }
                            #endregion
                            #region Depannage prepaye avec chg cpt
                            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannagePrepayer && rbt_DepConventionnel.IsChecked == true)
                            {
                                laDetailDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.ChangementCompteur).PK_ID;
                                laDetailDemande.LaDemande.CENTRE = laDetailDemandeRechercher.Abonne.CENTRE;
                                laDetailDemande.LaDemande.CLIENT = laDetailDemandeRechercher.Abonne.CLIENT;
                                laDetailDemande.LaDemande.ORDRE = laDetailDemandeRechercher.Abonne.ORDRE;
                                laDetailDemande.LaDemande.PRODUIT = laDetailDemandeRechercher.Abonne.PRODUIT;
                                laDetailDemande.LaDemande.FK_IDPRODUIT = laDetailDemandeRechercher.Abonne.FK_IDPRODUIT;
                                laDetailDemande.LaDemande.MATRICULE = UserConnecte.matricule;
                                laDetailDemande.LaDemande.FK_IDCENTRE = laDetailDemandeRechercher.Abonne.FK_IDCENTRE;
                                laDetailDemandeRechercher.LaDemande = laDetailDemande.LaDemande;

                                foreach (var leCompteur in (List<CsCanalisation>)dg_compteur.ItemsSource)
                                {
                                    CsEvenement leEvtPose = new CsEvenement();
                                    leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                                    leEvtPose.CENTRE = laDetailDemandeRechercher.LaDemande.CENTRE;
                                    leEvtPose.CLIENT = laDetailDemandeRechercher.LaDemande.CLIENT;
                                    leEvtPose.ORDRE = laDetailDemandeRechercher.LaDemande.ORDRE;
                                    leEvtPose.PRODUIT = laDetailDemandeRechercher.LaDemande.PRODUIT;
                                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                                    {
                                        leCompteur.POINT = 1;
                                        leEvtPose.POINT = 1;
                                    }
                                    else
                                    {
                                        leEvtPose.POINT = leCompteur.POINT;

                                    }

                                    leEvtPose.FK_IDPRODUIT = laDetailDemandeRechercher.LaDemande.FK_IDPRODUIT.Value;
                                    leEvtPose.FK_IDCENTRE = laDetailDemandeRechercher.LaDemande.FK_IDCENTRE;
                                    leEvtPose.MATRICULE = laDetailDemandeRechercher.LaDemande.MATRICULE;



                                    leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                                    leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                                    leEvtPose.COMPTEUR = leCompteur.NUMERO;
                                    leEvtPose.TYPETARIF = laDetailDemandeRechercher.Abonne.TYPETARIF;
                                    leEvtPose.CATEGORIE = laDetailDemandeRechercher.LeClient.CATEGORIE;
                                    leEvtPose.USERCREATION = UserConnecte.matricule;
                                    leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                                    leEvtPose.DATECREATION = System.DateTime.Now;
                                    leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                                    leEvtPose.CAS = leCompteur.CAS;
                                    leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
                                    leEvtPose.FK_IDABON = null;

                                    leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                                    leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                                    leEvtPose.DATEEVT = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? DtpPose : DtpDePose;
                                    leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                                    leEvtPose.PERIODE = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodePose.Text) : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
                                    leEvtPose.PUISSANCE = laDetailDemandeRechercher.Abonne.PUISSANCE;
                                    leEvtPose.TYPECOMPTAGE = laDetailDemandeRechercher.Branchement.TYPECOMPTAGE;

                                    //leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                                    //leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;

                                    if (laDetailDemande.LstEvenement != null && laDetailDemande.LstEvenement.Count != 0)
                                    {
                                        CsEvenement _LaCan = laDetailDemandeRechercher.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                                        if (_LaCan != null)
                                        {
                                            _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                            _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                            _LaCan.PERIODE = leEvtPose.PERIODE;
                                        }
                                        else
                                            laDetailDemandeRechercher.LstEvenement.Add(leEvtPose);
                                    }
                                    else
                                    {
                                        laDetailDemandeRechercher.LstEvenement = new List<CsEvenement>();
                                        laDetailDemandeRechercher.LstEvenement.Add(leEvtPose);
                                    }
                                }
                                foreach (var leCompteur in (List<CsCanalisation>)dg_AncienCompteur.ItemsSource)
                                {
                                    CsEvenement leEvtPose = new CsEvenement();
                                    leEvtPose.NUMDEM = laDetailDemande.LaDemande.NUMDEM;
                                    leEvtPose.CENTRE = laDetailDemandeRechercher.LaDemande.CENTRE;
                                    leEvtPose.CLIENT = laDetailDemandeRechercher.LaDemande.CLIENT;
                                    leEvtPose.ORDRE = laDetailDemandeRechercher.LaDemande.ORDRE;
                                    leEvtPose.PRODUIT = laDetailDemandeRechercher.LaDemande.PRODUIT;
                                    if (laDetailDemande.LaDemande.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                                    {
                                        leCompteur.POINT = 1;
                                        leEvtPose.POINT = 1;
                                    }
                                    else
                                    {
                                        leEvtPose.POINT = leCompteur.POINT;

                                    }

                                    leEvtPose.FK_IDPRODUIT = laDetailDemandeRechercher.LaDemande.FK_IDPRODUIT.Value;
                                    leEvtPose.FK_IDCENTRE = laDetailDemandeRechercher.LaDemande.FK_IDCENTRE;
                                    leEvtPose.MATRICULE = laDetailDemandeRechercher.LaDemande.MATRICULE;



                                    leEvtPose.REGLAGECOMPTEUR = leCompteur.REGLAGECOMPTEUR;
                                    leEvtPose.TYPECOMPTEUR = leCompteur.TYPECOMPTEUR;
                                    leEvtPose.COMPTEUR = leCompteur.NUMERO;
                                    leEvtPose.TYPETARIF = laDetailDemandeRechercher.Abonne.TYPETARIF;
                                    leEvtPose.CATEGORIE = laDetailDemandeRechercher.LeClient.CATEGORIE;
                                    leEvtPose.USERCREATION = UserConnecte.matricule;
                                    leEvtPose.USERMODIFICATION = UserConnecte.matricule;
                                    leEvtPose.DATECREATION = System.DateTime.Now;
                                    leEvtPose.DATEMODIFICATION = System.DateTime.Now;
                                    leEvtPose.CAS = leCompteur.CAS;
                                    leEvtPose.FK_IDCANALISATION = leCompteur.PK_ID;
                                    leEvtPose.FK_IDABON = null;

                                    leEvtPose.CODEEVT = SessionObject.Enumere.EvenementCodeNormale;
                                    leEvtPose.STATUS = SessionObject.Enumere.EvenementReleve;
                                    leEvtPose.DATEEVT = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? DtpPose : DtpDePose;
                                    leEvtPose.INDEXEVT = leCompteur.INDEXEVT;
                                    leEvtPose.PERIODE = leEvtPose.CAS == SessionObject.Enumere.CasPoseCompteur ? Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodePose.Text) : Shared.ClasseMEthodeGenerique.FormatPeriodeAAAAMM(TxtperiodeDepose.Text);
                                    leEvtPose.PUISSANCE = laDetailDemandeRechercher.Abonne.PUISSANCE;
                                    leEvtPose.TYPECOMPTAGE = laDetailDemandeRechercher.Branchement.TYPECOMPTAGE;

                                    //leEvtPose.COEFK1 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE1;
                                    //leEvtPose.COEFK2 = SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.PRODUIT == laDetailDemande.Branchement.PRODUIT && t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE) == null ? null : SessionObject.LstPuissanceInstalle.FirstOrDefault(t => t.VALEUR == laDetailDemande.Branchement.PUISSANCEINSTALLEE).KPERTEACTIVE2;

                                    if (laDetailDemandeRechercher.LstEvenement != null && laDetailDemandeRechercher.LstEvenement.Count != 0)
                                    {
                                        CsEvenement _LaCan = laDetailDemandeRechercher.LstEvenement.FirstOrDefault(p => p.CAS == leEvtPose.CAS && p.POINT == leEvtPose.POINT);
                                        if (_LaCan != null)
                                        {
                                            _LaCan.DATEEVT = leEvtPose.DATEEVT;
                                            _LaCan.INDEXEVT = leCompteur.INDEXEVT;
                                            _LaCan.PERIODE = leEvtPose.PERIODE;
                                        }
                                        else
                                            laDetailDemandeRechercher.LstEvenement.Add(leEvtPose);
                                    }
                                    else
                                    {
                                        laDetailDemandeRechercher.LstEvenement = new List<CsEvenement>();
                                        laDetailDemandeRechercher.LstEvenement.Add(leEvtPose);
                                    }
                                }
                                if (laDetailDemandeRechercher.LstCanalistion != null && laDetailDemandeRechercher.LstCanalistion.Count != 0) laDetailDemandeRechercher.LstCanalistion.Clear();
                                laDetailDemandeRechercher.LstCanalistion = (List<CsCanalisation>)dg_compteur.ItemsSource;
                                AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                                clientDeviss.ClotureValiderDemandeCompleted += (sss, bd) =>
                                {
                                    if (bd.Cancelled || bd.Error != null)
                                    {
                                        string error = bd.Error.Message;
                                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                        return;
                                    }
                                };
                                clientDeviss.ClotureValiderDemandeAsync(laDetailDemandeRechercher);
                            }
                            #endregion
                            #region Depanage Ep commune
                            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageEp && 
                                laDetailDemande.Depannage.ISCOMMUNE == true  )
                            {
                                AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                                clientDeviss.ClotureValiderDemandeCompleted += (sss, bdm) =>
                                {
                                    if (bdm.Cancelled || bdm.Error != null)
                                    {
                                        string error = bdm.Error.Message;
                                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                                        return;
                                    }
                                };
                                clientDeviss.ClotureValiderDemandeAsync(laDetailDemande);
                            }
                            #endregion

                            //#region DepannageEntretient
                            //if (this.rbt_DepReseau.IsChecked==true )
                            //{
                            //    laDetailDemandeRechercher.LaDemande = laDetailDemande.LaDemande;
                            //    laDetailDemande.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageMaintenance ).CODE;
                            //    laDetailDemande.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.DepannageMaintenance).PK_ID;
                            //    laDetailDemandeRechercher.LeClient.DATECREATION = System.DateTime.Now;
                            //    laDetailDemandeRechercher.LeClient.USERCREATION = UserConnecte.matricule;
                            //    laDetailDemandeRechercher.LeClient.DATEMODIFICATION = null;
                            //    laDetailDemandeRechercher.LeClient.USERMODIFICATION = null;

                            //    laDetailDemandeRechercher.Branchement.DATECREATION = System.DateTime.Now;
                            //    laDetailDemandeRechercher.Branchement.USERCREATION = UserConnecte.matricule;
                            //    laDetailDemandeRechercher.Branchement.DATEMODIFICATION = null;
                            //    laDetailDemandeRechercher.Branchement.USERMODIFICATION = null;

                            //    laDetailDemandeRechercher.Abonne.DATECREATION = System.DateTime.Now;
                            //    laDetailDemandeRechercher.Abonne.USERCREATION = UserConnecte.matricule;
                            //    laDetailDemandeRechercher.Abonne.DATEMODIFICATION = null;
                            //    laDetailDemandeRechercher.Abonne.USERMODIFICATION = null;

                            //    laDetailDemandeRechercher.Ag.DATECREATION = System.DateTime.Now;
                            //    laDetailDemandeRechercher.Ag.USERCREATION = UserConnecte.matricule;
                            //    laDetailDemandeRechercher.Ag.DATEMODIFICATION = null;
                            //    laDetailDemandeRechercher.Ag.USERMODIFICATION = null;

                            //    laDetailDemandeRechercher.LaDemande.REGLAGECOMPTEUR = laDetailDemandeRechercher.LstCanalistion.First().REGLAGECOMPTEUR;
                            //    laDetailDemandeRechercher.LaDemande.FK_IDREGLAGECOMPTEUR = laDetailDemandeRechercher.LstCanalistion.First().FK_IDREGLAGECOMPTEUR;

                            //    laDetailDemandeRechercher.LstCanalistion = null;
                            //    laDetailDemandeRechercher.LstEvenement = null;
                            //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                            //    client.ValiderDemandeInitailisationCompleted += (ssd, bd) =>
                            //    {
                            //        if (bd.Cancelled || bd.Error != null)
                            //        {
                            //            string error = bd.Error.Message;
                            //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            //            return;
                            //        }
                            //        string numDemande = string.Empty;
                            //        string Client = string.Empty;
                            //        string Retour = bd.Result;
                            //        string[] coupe = Retour.Split('.');
                            //        Shared.ClasseMEthodeGenerique.InitWOrkflow(coupe[0], laDetailDemandeRechercher.LaDemande.FK_IDCENTRE, coupe[1], laDetailDemandeRechercher.LaDemande.FK_IDTYPEDEMANDE);
                            //        numDemande = coupe[1];
                            //    };
                            //    client.ValiderDemandeInitailisationAsync(laDetailDemandeRechercher);
                            //}
                            //#endregion
                        }
                        this.DialogResult = false;
                    };
                    clientDevis.ValiderDemandeAsync(laDetailDemande);
                    this.DialogResult = true;
            }
            catch (Exception ex)
            {
                this.btn_Transmetre.IsEnabled = true;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }

        private void Cbo_TypePanne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (this.Cbo_TypedePanne.SelectedItem != null)
            //{
            //    this.txt_TypePanne.Text = ((CsTypePanne)this.Cbo_TypedePanne.SelectedItem).CODE;
            //}
        }

        private void Cbo_Vehicule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Cbo_Vehicule.SelectedItem != null)
            {
                this.txt_MatriculeVehicule.Text = ((CsVehicule )this.Cbo_Vehicule.SelectedItem).IMMATRICULATION ;
            }
        }
        private void rbt_RuptureSimple_Checked(object sender, RoutedEventArgs e)
        {
            txt_NumScelleRompu.IsReadOnly = false;
            txt_NumNouveauScelle.IsReadOnly = true;
        }

        private void rbt_RuptureSimple_Unchecked_1(object sender, RoutedEventArgs e)
        {
            txt_NumScelleRompu.IsReadOnly = true;
        }

        private void rbt_NouveauScelle_Checked_1(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = false;
            txt_NumScelleRompu.IsReadOnly = true;
            btn_ListScelle.IsEnabled = true;
        }

        private void rbt_NouveauScelle_Unchecked_1(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = true;
            btn_ListScelle.IsEnabled = false;
        }

        private void rbt_AuneAction_Checked(object sender, RoutedEventArgs e)
        {
            txt_NumNouveauScelle.IsReadOnly = true;
            txt_NumScelleRompu.IsReadOnly = true;
        }

        private void btn_Ajout_Click(object sender, RoutedEventArgs e)
        {
            if (dg_composantScellable.SelectedItem != null)
            {
                int nombre = 0;
                int.TryParse(txt_NombreScelle.Text, out nombre);
                CsOrganeScelleDemande OrganeScelleDemande = new CsOrganeScelleDemande();
                OrganeScelleDemande.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                OrganeScelleDemande.FK_IDORGANE_SCELLABLE = ((CsOrganeScellable)dg_composantScellable.SelectedItem).PK_ID;
                OrganeScelleDemande.LIBELLEORGANE_SCELLABLE = ListeOrganeScellable.FirstOrDefault(o => o.PK_ID == OrganeScelleDemande.FK_IDORGANE_SCELLABLE).LIBELLE;
                OrganeScelleDemande.NOMBRE = (nombre != 0 ? nombre : 0);

                if (rbt_RuptureSimple.IsChecked == true)
                {
                    if (!string.IsNullOrWhiteSpace(txt_NumScelleRompu.Text))
                        OrganeScelleDemande.NUM_SCELLE = txt_NumScelleRompu.Text;
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le numero du scelle rompu", "Information");
                        return;
                    }
                    //OrganeScelleDemande.NUM_SCELLE = !string.IsNullOrWhiteSpace(txt_NumScelleRompu.Text) ? txt_NumScelleRompu.Text : string.Empty;
                }
                if (rbt_NouveauScelle.IsChecked == true)
                {
                    if (!string.IsNullOrWhiteSpace(txt_NumNouveauScelle.Text))
                        OrganeScelleDemande.NUM_SCELLE = txt_NumNouveauScelle.Text;
                    else
                    {
                        Message.ShowWarning("Veuillez saisir le numero du nouveau scelle ", "Information");
                        return;
                    }
                }
                if (rbt_AuneAction.IsChecked == true)
                {
                    OrganeScelleDemande.NUM_SCELLE = string.Empty;
                }
                if (dg_Scellage.ItemsSource != null)
                {
                    var ScelleDejaLie = ((List<CsOrganeScelleDemande>)dg_Scellage.ItemsSource).Select(o => o.NUM_SCELLE);
                    if (!ScelleDejaLie.Contains(txt_NumScelleRompu.Text))
                    {
                        ListeOrganeScelleDemande.Add(OrganeScelleDemande);
                        dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
                    }
                }
                else
                {
                    ListeOrganeScelleDemande.Add(OrganeScelleDemande);
                    dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
                }
                this.txt_NumNouveauScelle.Text = string.Empty;
                this.txt_NumNouveauScelle.Tag = null;
            }
            else
            {
                Message.ShowWarning("Veuillez sélectionner un composant", "Information");
            }
        }

        private void btn_Supprimer_Click_1(object sender, RoutedEventArgs e)
        {
            if (dg_Scellage.SelectedItem != null)
            {
                CsOrganeScelleDemande OrganeScelleDemande = (CsOrganeScelleDemande)dg_Scellage.SelectedItem;
                ListeOrganeScelleDemande.Remove(OrganeScelleDemande);
                dg_Scellage.ItemsSource = ListeOrganeScelleDemande.OrderBy(c => c.NOMBRE).ToList();
            }
        }

        private void btn_ListScelle_Click(object sender, RoutedEventArgs e)
        {
            this.btn_ListScelle.IsEnabled = false;
            if (ListeScelle.Count != 0)
            {
                var ListeScelleValide = ListeScelle.Where(s => !ListeOrganeScelleDemande.Select(o => o.NUM_SCELLE).Contains(s.Numero_Scelle)).OrderBy(u => u.Numero_Scelle).ToList();
                if (ListeScelleValide != null && ListeScelleValide.Count() > 0)
                {
                    Dictionary<string, string> _LstColonneAffich = new Dictionary<string, string>();
                    _LstColonneAffich.Add("Numero_Scelle", "Numero_Scelle");

                    List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeScelleValide.ToList());
                    MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Scelle");
                    ctrl.Closed += new EventHandler(galatee_OkClickedBtnScelle);
                    ctrl.Show();
                }
                else
                {
                    Message.ShowInformation("Plus de scellés disponible en stock veuillez vous approvisionner", "Information");
                }
            }
        }

        void galatee_OkClickedBtnScelle(object sender, EventArgs e)
        {
            this.btn_ListScelle.IsEnabled = true;
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsScelle Scelle = (CsScelle)ctrs.MyObject;
                this.txt_NumNouveauScelle.Text = Scelle.Numero_Scelle;
                this.txt_NumNouveauScelle.Tag = Scelle;

            }

        }

        private void lnkLetter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lnkLetter.Tag == null)
                {
                    var openDialog = new OpenFileDialog();
                    //openDialog.Filter = "Text Files (*.txt)|*.txt";
                    openDialog.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp) | *.jpg; *.jpeg; *.png; *.bmp";
                    openDialog.Multiselect = true;
                    bool? userClickedOK = openDialog.ShowDialog();
                    if (userClickedOK == true)
                    {
                        if (openDialog.Files != null && openDialog.Files.Count() > 0 && openDialog.File != null)
                        {
                            FileStream stream = openDialog.File.OpenRead();
                            var memoryStream = new MemoryStream();
                            stream.CopyTo(memoryStream);
                            lnkLetter.Tag = memoryStream.GetBuffer();
                            var formScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Creation);
                            formScanne.Closed += new EventHandler(GetInformationFromChildWindowImageAutorisation);
                            formScanne.Show();
                        }
                    }
                }

                else
                {
                    MemoryStream memoryStream = new MemoryStream(lnkLetter.Tag as byte[]);
                    var ucImageScanne = new UcImageScanne(memoryStream, SessionObject.ExecMode.Modification);
                    ucImageScanne.Show();
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Devis.Languages.txtDevis);
            }
        }
        DateTime DtpPose = new DateTime();
        DateTime DtpDePose = new DateTime();
        private void dtpPose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpPose.SelectedDate != null && dtpPose.SelectedDate.Value != null && dg_compteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_compteur.ItemsSource;
                lesCompteur.ForEach(t => t.POSE = dtpPose.SelectedDate);
                LoadCompteur(lesCompteur);
                DtpPose = dtpPose.SelectedDate.Value;
                this.TxtperiodePose.Text = dtpPose.SelectedDate.Value.Month.ToString("00") + "/" + dtpPose.SelectedDate.Value.Year;
            }
        }

        private void dtpDepose_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (dtpDepose.SelectedDate != null && dtpDepose.SelectedDate.Value != null && dg_AncienCompteur.ItemsSource != null)
            {
                List<CsCanalisation> lesCompteur = (List<CsCanalisation>)dg_AncienCompteur.ItemsSource;
                lesCompteur.ForEach(t => t.DEPOSE = dtpDepose.SelectedDate);
                LoadCompteur(lesCompteur);
                DtpDePose = dtpDepose.SelectedDate.Value;
                this.TxtperiodeDepose.Text = dtpDepose.SelectedDate.Value.Month.ToString("00") + "/" + dtpDepose.SelectedDate.Value.Year;

            }
        }
        private void LoadListeOrganeScellable(int FK_IDTDEM, int FK_IDPRODUIT)
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeOrganeScellableCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListeOrganeScellable = args.Result;
                    dg_composantScellable.ItemsSource = ListeOrganeScellable;
                    return;
                };
                service.LoadListeOrganeScellableAsync(FK_IDTDEM, FK_IDPRODUIT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadListeScelle()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeScellesCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                    if (args.Result.Count == 0)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }

                    ListeScelle = args.Result;
                    if (rbt_NouveauScelle.IsChecked == true)
                        btn_ListScelle.IsEnabled = true;
                    return;
                };
                service.LoadListeScellesAsync(UserConnecte.PK_ID, laDetailDemande.LaDemande.FK_IDTYPEDEMANDE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadListeScelleMt()
        {
            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.LoadListeScellesDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    if (args.Result == null)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }
                    if (args.Result.Count == 0)
                    {
                        //tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
                        return;
                    }

                    ListeScelle = args.Result;
                    if (rbt_NouveauScelle.IsChecked == true)
                        btn_ListScelle.IsEnabled = true;
                    return;
                };
                service.LoadListeScellesDemandeAsync(UserConnecte.PK_ID, laDetailDemande.LaDemande.FK_IDTYPEDEMANDE, 5);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadCompteur(List<CsCanalisation> list)
        {
            if (((laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AugmentationPuissance ||
                laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DimunitionPuissance) &&
                laDetailDemande.LaDemande.ISCHANGECOMPTEUR == true)
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonementExtention
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementMt
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonement
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.BranchementAbonnementEp
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.AbonnementSeul
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur
                || laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Reabonnement)
            {
                if (list.First().PRODUIT == SessionObject.Enumere.ElectriciteMT)
                {
                    DataReferenceManager.CodificationCompteurMt(list);
                    string typeComptage = SessionObject.LstTypeComptage.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.TYPECOMPTAGE) != null ?
                        SessionObject.LstTypeComptage.FirstOrDefault(t => t.CODE == laDetailDemande.LaDemande.TYPECOMPTAGE).LIBELLE : string.Empty;
                    this.dg_compteur.Columns[2].Header = "TYPE COMPTAGE";
                    list.ForEach(t => t.LIBELLEREGLAGECOMPTEUR = typeComptage);
                    list.ForEach(t => t.INDEXEVT = 0);
                }
                dg_compteur.ItemsSource = list.Where(c => c.CAS == SessionObject.Enumere.CasPoseCompteur).ToList();
                List<CsCanalisation> lstNouvCompt = list.Where(c => c.CAS == SessionObject.Enumere.CasPoseCompteur).ToList();
                if (lstNouvCompt != null && lstNouvCompt.Count != 0)
                    lstNouvCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasPoseCompteur);

                //dg_AncienCompteur.IsEnabled = false;
                //this.dg_AncienCompteur.IsReadOnly = true;
                //TxtperiodePose.Text = string.IsNullOrEmpty(lstNouvCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstNouvCompt.First().PERIODE);
            }
        }

        private void ChargerCompteurADeposer(CsDemandeBase laDemande)
        {
            AcceuilServiceClient client = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.RetourneAncienCompteurCompleted += (ssender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    string error = args.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (args.Result == null)
                {
                    LayoutRoot.Cursor = Cursors.Arrow;
                    Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                else
                {
                    List<CsCanalisation> lesAncienCompteur = new List<CsCanalisation>();
                    lesAncienCompteur = args.Result;
                    lesAncienCompteur.ForEach(t => t.ANCCOMPTEUR = "000");
                    if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Resiliation ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.Etalonage ||
                        laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.ChangementCompteur)
                    {
                        if (lesAncienCompteur.Count == 0)
                        {
                            Message.ShowInformation("Ce client n'a pas de compteur", "Resiliation");
                            return;
                        }

                        dg_AncienCompteur.ItemsSource = lesAncienCompteur;
                        List<CsCanalisation> lstAncCompt = lesAncienCompteur;
                        if (lstAncCompt != null && lstAncCompt.Count != 0)
                            lstAncCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasDeposeCompteur);

                        TxtperiodeDepose.Text = string.IsNullOrEmpty(lstAncCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstAncCompt.First().PERIODE);
                    }
                }
            };
            client.RetourneAncienCompteurAsync(laDemande);
        }
        private void IsCompteurChanger_Checked(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageClient)
            {
                this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Visible;
                this.TxtOrdre.Visibility = System.Windows.Visibility.Visible;
                this.label_Reference.Visibility = System.Windows.Visibility.Visible;
                btn_RechercheClient.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannagePrepayer )
                {
                    this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Visible;
                    this.TxtOrdre.Visibility = System.Windows.Visibility.Visible;
                    this.label_Reference.Visibility = System.Windows.Visibility.Visible;
                    btn_RechercheClient.Visibility = System.Windows.Visibility.Visible;
                }
                this.tabSaisieIndex.Visibility = System.Windows.Visibility.Visible;
                tabitemScelle.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void IsCompteurChanger_Unchecked(object sender, RoutedEventArgs e)
        {
            if (laDetailDemande.LaDemande.TYPEDEMANDE != SessionObject.Enumere.DepannageClient)
            {
                this.tabSaisieIndex.Visibility = System.Windows.Visibility.Collapsed;
                tabitemScelle.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
 
        private void IsFactureAfaire_Checked(object sender, RoutedEventArgs e)
        {
            this.tabItemFacture.Visibility = System.Windows.Visibility.Visible;
            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageMT)
            {
                this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Visible;
                this.TxtOrdre.Visibility = System.Windows.Visibility.Visible;
                this.label_Reference.Visibility = System.Windows.Visibility.Visible;
                btn_RechercheClient.Visibility = System.Windows.Visibility.Visible;
            }
            if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannageEp)
            {
                Accueil.FrmPayeurFactureEP ctr = new Accueil.FrmPayeurFactureEP();
                ctr.Closed += ctr_Closed;
                ctr.Show();
            }
        }
        void ctr_Closed(object sender, EventArgs e)
        {
            Accueil.FrmPayeurFactureEP ctrs = sender as Accueil.FrmPayeurFactureEP;
            if (ctrs.IsOKclick == true)
            {
                if ( ctrs.IsEdm.IsChecked == true)
                    laDetailDemande.Depannage.ISEDM  = true;
                else if (ctrs.IsCommune.IsChecked == true)
                    laDetailDemande.Depannage.ISCOMMUNE  = true;
                else if (ctrs.IsTiers.IsChecked == true)
                {
                    laDetailDemande.Depannage.ISPERSONNEEXTERIEUR = true;
                    this.label_NomClient.Visibility = System.Windows.Visibility.Visible;
                    txt_nomClient.Visibility = System.Windows.Visibility.Visible;

                    this.label_NumVehicule.Visibility = System.Windows.Visibility.Visible;
                    txt_NumeroVehicule.Visibility = System.Windows.Visibility.Visible;
                }
            }

        }
        private void IsFactureAfaire_Unchecked(object sender, RoutedEventArgs e)
        {
            this.tabItemFacture.Visibility = System.Windows.Visibility.Collapsed ;

        }

        private void IsPanneNonRegle_Unchecked(object sender, RoutedEventArgs e)
        {
            this.rbt_DepMT.Visibility = System.Windows.Visibility.Collapsed;
            this.rbt_DepConventionnel.Visibility = System.Windows.Visibility.Collapsed;
            this.rbt_DepPrepaid.Visibility = System.Windows.Visibility.Collapsed;
            this.rbt_DepReseau.Visibility = System.Windows.Visibility.Collapsed;
            this.IsFactureAfaire.IsChecked = true ;
            this.IsFactureAfaire.Visibility = System.Windows.Visibility.Visible;
        }

        private void IsPanneNonRegle_Checked(object sender, RoutedEventArgs e)
        {
            this.rbt_DepMT.Visibility = System.Windows.Visibility.Visible;
            this.rbt_DepConventionnel.Visibility = System.Windows.Visibility.Visible;
            this.rbt_DepPrepaid.Visibility = System.Windows.Visibility.Visible;
            this.rbt_DepReseau.Visibility = System.Windows.Visibility.Visible;
            this.IsFactureAfaire.IsChecked  = false ;
            this.IsFactureAfaire.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void rdb_ClientConnu_Checked(object sender, RoutedEventArgs e)
        {
            this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Visible;
            this.TxtOrdre.Visibility = System.Windows.Visibility.Visible;
            this.label_Reference.Visibility = System.Windows.Visibility.Visible  ;
          
          
        }

        private void rdb_ClientNonConnu_Checked(object sender, RoutedEventArgs e)
        {
            this.Txt_ReferenceClient.Visibility = System.Windows.Visibility.Collapsed;
            this.TxtOrdre.Visibility = System.Windows.Visibility.Collapsed;
            this.label_Reference.Visibility = System.Windows.Visibility.Collapsed;
          
        }

        private void btn_RechercheClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                    ChargerClientFromReference(this.Txt_ReferenceClient.Text);
                else
                {
                    Message.Show("La reference saisie n'est pas correcte", "Infomation");
                }
            }
            catch (Exception ex)
            {
                Message.ShowInformation(ex.Message, "Demande");
            }
        }

        private void ChargerClientFromReference(string ReferenceClient)
        {
            try
            {
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.RetourneClientByReferenceCompleted += (s, args) =>
                {
                    if ((args != null && args.Cancelled) || (args.Error != null))
                        return;
                    if (args.Result != null && args.Result.Count > 1)
                    {
                        List<object> _Listgen =Shared.ClasseMEthodeGenerique.RetourneListeObjet(args.Result);
                        Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_Listgen, "CENTRE", "LIBELLESITE", "Liste des site");
                        ctr.Show();
                        ctr.Closed += new EventHandler(galatee_OkClickedChoixClient);
                    }
                    else
                    {
                        CsClient leClient = args.Result.First();
                        leClient.TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE ;
                        VerifieExisteDemande(leClient);
                    }
                };
                service.RetourneClientByReferenceAsync(ReferenceClient, lesCentrePerimetre);
                service.CloseAsync();

            }
            catch (Exception)
            {
                Message.ShowError("Erreur au chargement des données", "Demande");
            }
        }
        private void galatee_OkClickedChoixClient(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                CsClient _UnClient = (CsClient)ctrs.MyObject;
                _UnClient.TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE ;
                VerifieExisteDemande(_UnClient);
            }
        }
        private void VerifieExisteDemande(CsClient leClient)
        {

            try
            {
                if (!string.IsNullOrEmpty(Txt_ReferenceClient.Text) && Txt_ReferenceClient.Text.Length == SessionObject.Enumere.TailleClient)
                {
                    string OrdreMax = string.Empty;
                    AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    service.RetourneDemandeClientTypeCompleted += (s, args) =>
                    {
                        if (args != null && args.Cancelled)
                            return;
                        if (args.Result != null)
                        {
                            if (args.Result.DATEFIN == null && args.Result.ISSUPPRIME != true)
                            {
                                Message.ShowInformation("Il existe une demande numero " + args.Result.NUMDEM + " sur ce client", "Accueil");
                                return;
                            }
                        }
                        ChargeDetailDEvis(leClient);
                    };
                    service.RetourneDemandeClientTypeAsync(leClient);
                    service.CloseAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private void ChargeDetailDEvis(CsClient leclient)
        {

            try
            {
                leclient.TYPEDEMANDE = laDetailDemande.LaDemande.TYPEDEMANDE ;
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                client.GeDetailByFromClientCompleted += (ssender, args) =>
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        string error = args.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (args.Result == null)
                    {
                        Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    else
                    {
                        laDetailDemandeRechercher = new CsDemande();
                        laDetailDemandeRechercher = args.Result;
                        this.TxtOrdre.Text = laDetailDemandeRechercher.LeClient.ORDRE;
                        if (laDetailDemande.LaDemande.TYPEDEMANDE == SessionObject.Enumere.DepannagePrepayer)
                        {
                            dg_AncienCompteur.ItemsSource = laDetailDemandeRechercher.LstCanalistion ;
                            List<CsCanalisation> lstAncCompt = laDetailDemandeRechercher.LstCanalistion;
                            if (lstAncCompt != null && lstAncCompt.Count != 0)
                                lstAncCompt.ForEach(t => t.CAS = SessionObject.Enumere.CasDeposeCompteur);

                            TxtperiodeDepose.Text = string.IsNullOrEmpty(lstAncCompt.First().PERIODE) ? string.Empty : Shared.ClasseMEthodeGenerique.FormatPeriodeMMAAAA(lstAncCompt.First().PERIODE);
                        }

                    }
                };
                client.GeDetailByFromClientAsync(leclient);
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des donnéés", "Demande");
            }
        }

        CsReglageCompteur leReglageCompteur = null;
        List<CsCompteur> LstCompteur = new List<CsCompteur>();

        private void IsSelectionCtr_Checked(object sender, RoutedEventArgs e)
        {
            leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == laDetailDemandeRechercher.LstCanalistion.First().REGLAGECOMPTEUR);
            List<CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t => t.REGLAGEMINI >= leReglageCompteur.REGLAGEMINI &&
                                                                                                      t.REGLAGEMAXI <= leReglageCompteur.REGLAGEMAXI &&
                                                                                                      t.FK_IDPRODUIT == laDetailDemandeRechercher.Abonne.FK_IDPRODUIT).ToList();

            List<int> lesIdCalibre = new List<int>();
            foreach (CsCalibreCompteur item in LeCalibreEquivalant)
                lesIdCalibre.Add(item.PK_ID);

            UcDetailCompteur ctr = new UcDetailCompteur(laDetailDemande.LaDemande, LstCompteur.Where(t =>t.CODEPRODUIT == laDetailDemandeRechercher.Abonne.PRODUIT && lesIdCalibre.Contains(t.FK_IDCALIBRECOMPTEUR.Value)).ToList());
            ctr.Closed += new EventHandler(galatee_Check);
            ctr.Show();
        }
        List<CsCanalisation> lesCanalisationACree = new List<CsCanalisation>();

        void galatee_Check(object sender, EventArgs e)
        {

            UcDetailCompteur ctrs = sender as UcDetailCompteur;
            if (ctrs.isOkClick)
            {
                int i = 1;
                List<CsCompteur> _LesCompteurs = (List<CsCompteur>)ctrs.MyObject;

                foreach (CsCompteur _LeCompteur in _LesCompteurs)
                {
                    CsCanalisation canal = new CsCanalisation()
                    {
                        CENTRE = laDetailDemandeRechercher.Abonne.CENTRE ,
                        CLIENT = laDetailDemandeRechercher.Abonne.CLIENT ,
                        NUMDEM = laDetailDemande.LaDemande.NUMDEM,
                        PRODUIT = laDetailDemandeRechercher.Abonne.PRODUIT,
                        PROPRIO = "1",
                        MARQUE = _LeCompteur.MARQUE,
                        TYPECOMPTEUR = _LeCompteur.TYPECOMPTEUR,
                        NUMERO = _LeCompteur.NUMERO,
                        INFOCOMPTEUR = _LeCompteur.NUMERO,
                        FK_IDCENTRE = laDetailDemandeRechercher.Abonne.FK_IDCENTRE ,
                        FK_IDPRODUIT = laDetailDemandeRechercher.Abonne.FK_IDPRODUIT ,
                        FK_IDMAGAZINVIRTUEL = _LeCompteur.PK_ID,
                        //FK_IDCOMPTEUR  = _LeCompteur.PK_ID,
                        FK_IDTYPECOMPTEUR = _LeCompteur.FK_IDTYPECOMPTEUR,
                        FK_IDMARQUECOMPTEUR = _LeCompteur.FK_IDMARQUECOMPTEUR,
                        FK_IDCALIBRE = _LeCompteur.FK_IDCALIBRECOMPTEUR,
                        FK_IDREGLAGECOMPTEUR = leReglageCompteur.PK_ID,
                        FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID,
                        LIBELLETYPECOMPTEUR = _LeCompteur.NUMERO,
                        LIBELLEMARQUE = _LeCompteur.LIBELLEMARQUE,
                        LIBELLEREGLAGECOMPTEUR = _LeCompteur.LIBELLECALIBRE,
                        POSE = System.DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        USERMODIFICATION = UserConnecte.matricule,
                        DATECREATION = System.DateTime.Now,
                        DATEMODIFICATION = System.DateTime.Now,
                        FK_IDPROPRIETAIRE = 1,
                        CAS = SessionObject.Enumere.CasPoseCompteur
                    };
                    lesCanalisationACree.Add(canal);
                }
                this.dg_compteur.ItemsSource = null;
                this.dg_compteur.ItemsSource = lesCanalisationACree;
            }
        }
        private void loadCompteur(List<string> codeSite)
        {
            AcceuilServiceClient service2 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service2.RetourneListeCompteurMagasinCompleted += (sr2, res2) =>
            {

                if (res2 != null && res2.Cancelled)
                    return;
                LstCompteur = res2.Result;

            };
            service2.RetourneListeCompteurMagasinAsync(codeSite);
            service2.CloseAsync();
        }

        private void IsEltDevis_Checked(object sender, RoutedEventArgs e)
        {
            tabItemFacture.Visibility = System.Windows.Visibility.Visible;
            this.tabControle1.SelectedItem = tabItemFacture;
        }

        private void IsEltDevis_Unchecked(object sender, RoutedEventArgs e)
        {
            tabItemFacture.Visibility = System.Windows.Visibility.Collapsed ;
            this.tabControle1.SelectedItem = tabItemInfoDemande;

        }

        private void DtpDebutTravaux_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date =DtpDebutTravaux.SelectedDate!=null? DtpDebutTravaux.SelectedDate.Value.Date:DateTime.Now.Date;
            if (txt_HeureDebut.Value!=null)
            {
                date = date.AddHours(txt_HeureDebut.Value.Value.Hour);
                date = date.AddMinutes(txt_HeureDebut.Value.Value.Minute);
                date = date.AddMinutes(txt_HeureDebut.Value.Value.Millisecond); 
            }
            DtpDebutTravaux.SelectedDate = date;
        }

        private void DtpFinTravaux_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime date = DtpFinTravaux.SelectedDate != null ? DtpFinTravaux.SelectedDate.Value.Date : DateTime.Now.Date;
            if (txt_HeureFin.Value != null)
            {
                date = date.AddHours(txt_HeureFin.Value.Value.Hour);
                date = date.AddMinutes(txt_HeureFin.Value.Value.Minute);
                date = date.AddMinutes(txt_HeureFin.Value.Value.Millisecond);
            }
            DtpFinTravaux.SelectedDate = date;
        }

        private void txt_HeureDebut_ValueChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            DateTime date = DtpDebutTravaux.SelectedDate != null ? DtpDebutTravaux.SelectedDate.Value.Date : DateTime.Now.Date;
            if (txt_HeureDebut.Value != null)
            {
                date = date.AddHours(txt_HeureDebut.Value.Value.Hour);
                date = date.AddMinutes(txt_HeureDebut.Value.Value.Minute);
                date = date.AddMinutes(txt_HeureDebut.Value.Value.Millisecond);
            }
            DtpDebutTravaux.SelectedDate = date;
        }

        private void txt_HeureFin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            DateTime date = DtpFinTravaux.SelectedDate != null ? DtpFinTravaux.SelectedDate.Value.Date : DateTime.Now.Date;
            if (txt_HeureFin.Value != null)
            {
                date = date.AddHours(txt_HeureFin.Value.Value.Hour);
                date = date.AddMinutes(txt_HeureFin.Value.Value.Minute);
                date = date.AddMinutes(txt_HeureFin.Value.Value.Millisecond);
            }
            DtpFinTravaux.SelectedDate = date;
        }
    }
}

