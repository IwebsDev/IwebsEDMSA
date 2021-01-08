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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using Galatee.Silverlight.Resources.Devis;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Shared;

namespace Galatee.Silverlight.Devis
{
    public partial class UcValidationDapannage : ChildWindow
    {
        private ObjDEVIS ObjetDevisSelectionne = null;
        private ObjDEVIS MyDevis = new ObjDEVIS();
        public Galatee.Silverlight.SessionObject.ExecMode ExecMode {get;set;}
        private CsDemandeBase laDemandeSelect = null;
        private CsDemande laDetailDemande = new CsDemande();

        public bool IsForAnalyse { get; set; }

        public UcValidationDapannage()
        {
            InitializeComponent();
        }
        public UcValidationDapannage(int idDevis)
        {
            InitializeComponent();
            ChargeDetailDEvis(idDevis);

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
                    RenseignerInformationsDevis(laDetailDemande);
                    RenseignerInformationsFournitureDevis(laDetailDemande);
                    LayoutRoot.Cursor = Cursors.Arrow;
                }
                LayoutRoot.Cursor = Cursors.Arrow;



            };
            client.GetDevisByNumIdDevisAsync(IdDemandeDevis);
        }
      
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_transmetre.IsEnabled = false;
                EnregisterOuTransmetre(false);
            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, Languages.txtDevis);
            }
        }
        private void EnregisterOuTransmetre(bool isTransmetre)
        {
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            if (laDetailDemande.EltDevis != null && laDetailDemande.EltDevis.Count != 0)
            {
                int idTrv = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperTRV).PK_ID;
                ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID == laDetailDemande.LaDemande.FK_IDCENTRE);

                CsDemandeDetailCout leCoutduDevis = new CsDemandeDetailCout();
                if (laDetailDemande.Depannage.ISPERSONNEEXTERIEUR==true )
                {
                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(SessionObject.Enumere.Generale) ? null : SessionObject.Enumere.Generale;
                    leCoutduDevis.CLIENT = string.IsNullOrEmpty("00000000000") ? null : "00000000000";
                    leCoutduDevis.ORDRE = "00";
                }
                else
                {
                    leCoutduDevis.CENTRE = string.IsNullOrEmpty(SessionObject.Enumere.Generale) ? null : SessionObject.Enumere.Generale;
                    leCoutduDevis.CLIENT = string.IsNullOrEmpty(leCentre.COMPTEECLAIRAGEPUBLIC) ? null : leCentre.COMPTEECLAIRAGEPUBLIC;
                    leCoutduDevis.ORDRE = "01";
                }
                leCoutduDevis.FK_IDCENTRE = laDetailDemande.LaDemande.FK_IDCENTRE;
                leCoutduDevis.NUMDEM = string.IsNullOrEmpty(laDetailDemande.LaDemande.NUMDEM) ? string.Empty : laDetailDemande.LaDemande.NUMDEM;
                leCoutduDevis.COPER = SessionObject.Enumere.CoperFactureTrvxEtDivers;
                leCoutduDevis.FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperFactureTrvxEtDivers).PK_ID;
                leCoutduDevis.FK_IDTAXE = laDetailDemande.EltDevis.First().FK_IDTAXE.Value;
                leCoutduDevis.FK_IDDEMANDE = laDetailDemande.LaDemande.PK_ID;
                leCoutduDevis.MONTANTHT = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTHT  ));
                leCoutduDevis.MONTANTTAXE = (decimal?)Math.Ceiling((double)laDetailDemande.EltDevis.Where(t => t.FK_IDCOPER == idTrv).Sum(h => h.MONTANTTAXE ));
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
                    LaRubriqueDevis.NOM = item.NOM;
                    LaRubriqueDevis.CENTRE = laDetailDemande.LaDemande.CENTRE;
                    LaRubriqueDevis.PRODUIT = laDetailDemande.LaDemande.LIBELLEPRODUIT;
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

            }
            bool? IsDevisEditer = laDetailDemande.Depannage.ISPERSONNEEXTERIEUR ;

            laDetailDemande.EltDevis = null;
            laDetailDemande.Depannage  = null;
            AcceuilServiceClient clientDevis = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            clientDevis.ValiderDemandeCompleted += (ss, b) =>
            {
                this.btn_transmetre.IsEnabled = true;
                if (b.Cancelled || b.Error != null)
                {
                    string error = b.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                if (isTransmetre == true)
                {
                    if (IsDevisEditer == true )
                        Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "DevisEp", "Accueil", true);
                    //List<string> codes = new List<string>();
                    //codes.Add(laDetailDemande.InfoDemande.CODE);
                    //Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);

                    AcceuilServiceClient clientDeviss = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    clientDeviss.ClotureValiderDemandeCompleted += (sss, bd) =>
                    {
                        if (bd.Cancelled || bd.Error != null)
                        {
                            string error = bd.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (bd.Result == true)
                        {

                            List<string> codes = new List<string>();
                            codes.Add(laDetailDemande.InfoDemande.CODE);
                            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, false, this);
                        }
                        else
                        {
                            Message.ShowError("Erreur a la cloture de la demande", Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                    };
                    clientDeviss.ClotureValiderDemandeAsync(laDetailDemande);
                }
            };
            clientDevis.ValiderDemandeAsync(laDetailDemande);
                   
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        void Translate()
        {

        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int resultLoding = 0;
        }

        private void AfficherOuMasquer(TabItem pTabItem, bool pValue)
        {
            try
            {
                pTabItem.Visibility = pValue ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RenseignerInformationsDevis(CsDemande laDemande)
        {
            try
            {
                if (laDemande != null)
                {
                   ServiceAccueil.CsCentre leCentre = SessionObject.LstCentre.FirstOrDefault(t => t.PK_ID  == laDemande.LaDemande.FK_IDCENTRE );

                    Txt_NumeroDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.NUMDEM) ? laDemande.LaDemande.NUMDEM : string.Empty;
                    Txt_Ordre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.ORDRE.ToString()) ? laDemande.LaDemande.ORDRE.ToString() : string.Empty;
                    Txt_CodeSite.Text = !string.IsNullOrEmpty(leCentre.CODESITE) ? leCentre.CODESITE  : string.Empty;
                    Txt_CodeCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.CENTRE) ? laDemande.LaDemande.CENTRE : string.Empty;
                    Txt_LibelleCentre.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLECENTRE) ? laDemande.LaDemande.LIBELLECENTRE : string.Empty;
                    Txt_LibelleSite.Text = !string.IsNullOrEmpty(leCentre.LIBELLESITE) ? leCentre.LIBELLESITE : string.Empty;
                    Txt_LibelleTypeDevis.Text = !string.IsNullOrEmpty(laDemande.LaDemande.LIBELLETYPEDEMANDE) ? laDemande.LaDemande.LIBELLETYPEDEMANDE : string.Empty;

                    Txt_Commune.Text = !string.IsNullOrEmpty(laDemande.Depannage.LACOMMUNE) ? laDemande.Depannage.LACOMMUNE : string.Empty;
                    Txt_Quartier.Text = !string.IsNullOrEmpty(laDemande.Depannage.LEQUARTIER ) ? laDemande.Depannage.LEQUARTIER  : string.Empty;
                    Txt_Secteur.Text = !string.IsNullOrEmpty(laDemande.Depannage.LESECTEUR) ? laDemande.Depannage.LESECTEUR : string.Empty;
                    Txt_Rue.Text = !string.IsNullOrEmpty(laDemande.Depannage.LARUE ) ? laDemande.Depannage.LARUE  : string.Empty;
                    Txt_Commentaire.Text = !string.IsNullOrEmpty(laDemande.Depannage.PROCESVERBAL) ? laDemande.Depannage.PROCESVERBAL : string.Empty;
                    txt_TypePanneTraite.Text = !string.IsNullOrEmpty(laDemande.Depannage.PANNETRAITE) ? laDemande.Depannage.PANNETRAITE : string.Empty;
                    txt_TypePanne.Text = !string.IsNullOrEmpty(laDemande.Depannage.TYPEDEPANNE) ? laDemande.Depannage.TYPEDEPANNE : string.Empty;
                    Txt_Commentaire1.Text = !string.IsNullOrEmpty(laDemande.Depannage.DESCRIPTIONPANNE) ? laDemande.Depannage.DESCRIPTIONPANNE : string.Empty;
                    
                    
                    this.Txt_EtapeCourante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Title = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_ACTUELLE) ? laDemande.InfoDemande.ETAPE_ACTUELLE : string.Empty;
                    this.Txt_EtapeSuivante.Text = !string.IsNullOrEmpty(laDemande.InfoDemande.ETAPE_SUIVANTE) ? laDemande.InfoDemande.ETAPE_SUIVANTE : string.Empty; 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void RenseignerInformationsFournitureDevis( CsDemande lademande)
        {
            try
            {
                if (lademande != null && (lademande.EltDevis != null && lademande.EltDevis.Count > 0))
                {
                    dataGridForniture.ItemsSource = null;
                    dataGridForniture.ItemsSource = lademande.EltDevis;
                
                    this.Txt_TotalHt.Text = lademande.EltDevis.Sum(t=>t.MONTANTHT ).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTtc.Text = lademande.EltDevis.Sum(t => t.MONTANTTTC ).Value .ToString(SessionObject.FormatMontant);
                    this.Txt_TotalTva.Text = lademande.EltDevis.Sum(t => t.MONTANTTAXE ).Value .ToString(SessionObject.FormatMontant);
                    AfficherOuMasquer(tabItemFournitures, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Erreur au chargement des fournitures", "Validation devis");
            }
        }
 
        private void btn_transmetre_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            EnregisterOuTransmetre(true);
        }

        private void RejeterButton_Click(object sender, RoutedEventArgs e)
        {
            Shared.ClasseMEthodeGenerique.RejeterDemande(laDetailDemande);
            this.DialogResult = false;
        }

     
    }
}

