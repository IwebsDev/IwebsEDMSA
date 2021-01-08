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
    public partial class FrmDemandeTimbre : ChildWindow
    {

        public FrmDemandeTimbre()
        {
            InitializeComponent();
            RetourneTypeTimbre();
        }

 
        List<ServiceAccueil.CsElementAchatTimbre> lstObjetDevis = new List<ServiceAccueil.CsElementAchatTimbre>();
        CsDemande laDetailDemande = null;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            btn_Transmetre.IsEnabled = false;
            ValiderInitialisation(laDetailDemande, true );
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
                    demandedevis.LaDemande.CLIENT = "00000000000";
                    demandedevis.LaDemande.ORDRE = "01";
                    if (SessionObject.LePosteCourant.CODECENTRE == SessionObject.Enumere.Generale)
                    {
                      
                        demandedevis.LaDemande.CENTRE = "011";
                        demandedevis.LaDemande.FK_IDCENTRE = 63;
                    }
                    else
                    {
                        demandedevis.LaDemande.CENTRE = SessionObject.LePosteCourant.CODECENTRE;
                        demandedevis.LaDemande.FK_IDCENTRE = SessionObject.LePosteCourant.FK_IDCENTRE.Value ;
                    }
                    Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                    client.CreeDemandeCompleted += (ss, b) =>
                    {
                        DialogResult = true;
                        if (b.Cancelled || b.Error != null)
                        {
                            string error = b.Error.Message;
                            Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                            return;
                        }
                        if (b.Result != null)
                        {
                            Message.ShowInformation("La demande a été créée avec succès. Numéro de votre demande : " + b.Result.NUMDEM,
                                 Silverlight.Resources.Devis.Languages.txtDevis);
                        }
                        else
                            Message.ShowError("Une erreur s'est produite à la création de la demande ", "CreeDemande");
                    };
                    client.CreeDemandeAsync(demandedevis, true);
                }
            }
            catch (Exception ex)
            {
                Message.ShowError("Une erreur est survenu suite à la validation", "Validation demande");
            }
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
                pDemandeDevis.LaDemande.TYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.AchatTimbre ).CODE;
                pDemandeDevis.LaDemande.FK_IDTYPEDEMANDE = SessionObject.LstTypeDemande.FirstOrDefault(t => t.CODE == SessionObject.Enumere.AchatTimbre).PK_ID;
            }
            if (pDemandeDevis.LaDemande == null) pDemandeDevis.LaDemande = new CsDemandeBase();
            pDemandeDevis.LaDemande.DATEMODIFICATION = DateTime.Now;
            pDemandeDevis.LstEltTimbre = lstObjetDevis;

            return pDemandeDevis;
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
                cbo_TypeTimbre.ItemsSource = args.Result;
                cbo_TypeTimbre.DisplayMemberPath = "LIBELLE";

            };
             service.RetouneTypeTimbreAsync();
            service.CloseAsync();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.cbo_TypeTimbre.SelectedItem != null && !string.IsNullOrEmpty(this.Txt_Nombre.Text))
            {
                ServiceAccueil.CsElementAchatTimbre leElt = new ServiceAccueil.CsElementAchatTimbre()
                {
                    CODE = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).CODE,
                    DESIGNATION = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).LIBELLE,
                    PRIXUNITAIRE = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).MONTANT,
                    QUANTITE = int.Parse(Txt_Nombre.Text),
                    MONTANT = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).MONTANT * int.Parse(Txt_Nombre.Text),
                    USERCREATION = UserConnecte.matricule ,
                    DATECREATION = System.DateTime.Today.Date  ,
                    FK_IDTIMBRE = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).PK_ID 
                };
                ServiceAccueil.CsElementAchatTimbre leEltRech = lstObjetDevis.FirstOrDefault(t => t.CODE == leElt.CODE);
                if (leEltRech != null)
                {
                    lstObjetDevis.Remove(leEltRech);
                    lstObjetDevis.Add(leElt);
                }
                else
                    lstObjetDevis.Add(leElt);

                this.dtg_EtatCaisse.ItemsSource = null;
                this.dtg_EtatCaisse.ItemsSource = lstObjetDevis;
            }

        }

        private void cbo_TypeTimbre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbo_TypeTimbre.SelectedItem != null)
                this.Txt_Montant.Text = ((CsTypeTimbre)cbo_TypeTimbre.SelectedItem).MONTANT.Value.ToString(SessionObject.FormatMontant); 
        }

     
    }
}

