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
using Galatee.Silverlight.ServiceCaisse;
using Galatee.Silverlight.ServiceAccueil;
namespace Galatee.Silverlight.Caisse
{
    public partial class FrmDemandeTimbreValidation : ChildWindow
    {
        public FrmDemandeTimbreValidation()
        {
            InitializeComponent();
            RetourneTypeTimbre();
        }
        public FrmDemandeTimbreValidation(int numDemande)
        {
            InitializeComponent();
            RetourneTypeTimbre();
            ChargeDetailDEvis(numDemande);
        }
        List<ServiceAccueil.CsElementAchatTimbre> lstObjetDevis = new List<ServiceAccueil.CsElementAchatTimbre>();
        CsDemande laDetailDemande = null;
        private void ChargeDetailDEvis(int IdDemandeDevis)
        {

            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.ChargerDetailDemandeAsync(IdDemandeDevis, string.Empty);
            client.ChargerDetailDemandeCompleted += (ssender, args) =>
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
                    this.dtg_EtatCaisse.ItemsSource = null;
                    this.dtg_EtatCaisse.ItemsSource = laDetailDemande.LstEltTimbre;
                    this.Txt_NumDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.Txt_UserCreat.Text = laDetailDemande.LaDemande.USERCREATION;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }
        //List<ServiceAccueil.CsElementAchatTimbre> lstObjetDevis = new List<ServiceAccueil.CsElementAchatTimbre>();
        //CsDemande laDetailDemande = null;
        //private void ChargeDetailDEvis(int IdDemandeDevis)
        //{

        //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
        //    client.GetDemandeByNumIdDemandeAsync(IdDemandeDevis);
        //    client.GetDemandeByNumIdDemandeCompleted += (ssender, args) =>
        //    {
        //        if (args.Cancelled || args.Error != null)
        //        {
        //            LayoutRoot.Cursor = Cursors.Arrow;
        //            string error = args.Error.Message;
        //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
        //            return;
        //        }
        //        if (args.Result == null)
        //        {
        //            LayoutRoot.Cursor = Cursors.Arrow;
        //            Message.ShowError(Silverlight.Resources.Devis.Languages.AucunesDonneesTrouvees, Silverlight.Resources.Devis.Languages.txtDevis);
        //            return;
        //        }
        //        else
        //        {
        //            laDetailDemande = args.Result;
        //            this.dtg_EtatCaisse.ItemsSource = null;
        //            this.dtg_EtatCaisse.ItemsSource = laDetailDemande.LstEltTimbre ;
        //            this.Txt_NumDemande.Text = laDetailDemande.LaDemande.NUMDEM;
        //            this.Txt_UserCreat .Text = laDetailDemande.LaDemande.USERCREATION  ;
        //        }
        //        LayoutRoot.Cursor = Cursors.Arrow;
        //    };
        //}
      
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void RetourneTypeTimbre()
        {
             CaisseServiceClient service = new  CaisseServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Caisse"));
             service.RetouneTypeTimbreCompleted  += (s, args) =>
            {
                if (args != null && args.Cancelled)
                    return;

            };
             service.RetouneTypeTimbreAsync();
            service.CloseAsync();
        }
        private CsDemande GetDemandeDevisFromScreen(CsDemande pDemandeDevis, bool isTransmettre)
        {
         
            if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
            pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
            CsDemandeDetailCout lstEltDEvis = new CsDemandeDetailCout
                {
                     CENTRE = pDemandeDevis.LaDemande.CENTRE ,
                     FK_IDCENTRE = pDemandeDevis.LaDemande.FK_IDCENTRE ,
                     FK_IDDEMANDE = pDemandeDevis.LstEltTimbre.First().FK_IDDEMANDE ,
                     MONTANTHT = pDemandeDevis.LstEltTimbre.Sum(t=>t.MONTANT),
                     NUMDEM = pDemandeDevis.LstEltTimbre.First().NUMDEM ,
                     MONTANTTAXE = 0,
                     COPER = SessionObject.Enumere.CoperACT,
                     FK_IDCOPER = SessionObject.LstDesCopers.FirstOrDefault(t => t.CODE == SessionObject.Enumere.CoperACT).PK_ID,
                     FK_IDTAXE = SessionObject.LstDesTaxe .FirstOrDefault(t => t.CODE == "00").PK_ID,
                     NATURE = "99",
                     REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00"),
                     USERCREATION = pDemandeDevis.LstEltTimbre.First().USERCREATION ,
                     DATECREATION = System.DateTime.Today.Date  
                };
            pDemandeDevis.LstEltTimbre = null;
            pDemandeDevis.LstCoutDemande = new List<CsDemandeDetailCout>();
            pDemandeDevis.LstCoutDemande.Add(lstEltDEvis);
            return pDemandeDevis;
        }
        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {
                // Get Devis informations from screen
                if (demandedevis != null)
                    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                else
                    demandedevis = GetDemandeDevisFromScreen(null, false);
                // Get DemandeDevis informations from screen
                if (demandedevis != null)
                {
                    if (IsTransmetre)
                        demandedevis.LaDemande.ETAPEDEMANDE = null;
                    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                    demandedevis.LaDemande.CENTRE = SessionObject.LePosteCourant.CODECENTRE;
                    demandedevis.LaDemande.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value;
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.ValiderAchatimbreDemandeCompleted += (ss, b) =>
                    {
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        Message.ShowInformation("Demande transmise avec succès","Achat de timbre");
                         
                        this.DialogResult = false;

                    };
                    client.ValiderAchatimbreDemandeAsync(demandedevis);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur est survenue a la validation de la demande", "ValiderDemandeInitailisation");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ValiderInitialisation(laDetailDemande, false );
        }
    }
}

