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
    public partial class FrmDemandeTimbreVerification : ChildWindow
    {
        public FrmDemandeTimbreVerification()
        {
            InitializeComponent();
            RetourneTypeTimbre();
        }
        public FrmDemandeTimbreVerification(int numDemande)
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
                    this.dtg_EtatCaisse.ItemsSource = laDetailDemande.LstEltTimbre ;
                    this.Txt_NumDemande.Text = laDetailDemande.LaDemande.NUMDEM;
                    this.Txt_UserCreat.Text = laDetailDemande.LaDemande.USERCREATION ;
                }
                LayoutRoot.Cursor = Cursors.Arrow;
            };
        }
      
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
            if (pDemandeDevis == null)
            {
                pDemandeDevis = new CsDemande();
                pDemandeDevis.LaDemande = new CsDemandeBase();
                pDemandeDevis.LaDemande.DATECREATION = DateTime.Now;
                pDemandeDevis.LaDemande.USERCREATION = UserConnecte.matricule;
                pDemandeDevis.LaDemande.FK_IDADMUTILISATEUR = UserConnecte.PK_ID;
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == "60").CODE;
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == "60").PK_ID;
            }
            if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
            pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
            return pDemandeDevis;
        }

        private void ValiderInitialisation(CsDemande demandedevis, bool IsTransmetre)
        {

            try
            {

                List<CsDemandeBase> laDema = new List<CsDemandeBase>();
                laDema.Add(demandedevis.LaDemande);
                Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(laDema, true, this);


                //// Get Devis informations from screen
                //if (demandedevis != null)
                //    demandedevis = GetDemandeDevisFromScreen(demandedevis, false);
                //else
                //    demandedevis = GetDemandeDevisFromScreen(null, false);
                //// Get DemandeDevis informations from screen
                //if (demandedevis != null)
                //{
                //    if (IsTransmetre)
                //        demandedevis.LaDemande.ETAPEDEMANDE = null;
                //    demandedevis.LaDemande.MATRICULE = UserConnecte.matricule;
                //    demandedevis.LaDemande.CENTRE = SessionObject.LePosteCourant.CODECENTRE;
                //    demandedevis.LaDemande.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value;
                //    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                //    client.ValiderDemandeInitailisationCompleted += (ss, b) =>
                //    {
                //        if (b.Cancelled || b.Error != null)
                //        {
                //            string error = b.Error.Message;
                //            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                //            return;
                //        }
                //        if (IsTransmetre)
                //        {
                //            List<string> codes = new List<string>();
                //            codes.Add(laDetailDemande.InfoDemande.CODE);
                //            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true , this);
                //            this.DialogResult = true;
                //        }
                //        this.DialogResult = false;

                //    };
                //    client.ValiderDemandeInitailisationAsync(demandedevis);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ValiderInitialisation(laDetailDemande,false );
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            List<string> codes = new List<string>();
            codes.Add(laDetailDemande.InfoDemande.CODE);
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.TransmettreDemande(codes, true , this);
            this.DialogResult = true;
        }
    }
}

