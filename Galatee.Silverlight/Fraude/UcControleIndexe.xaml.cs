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
using Galatee.Silverlight.Resources.Fraude;
using System.Collections.ObjectModel;
using Galatee.Silverlight.ServiceFraude;
using System.Windows.Data;
using System.Globalization;

namespace Galatee.Silverlight.Fraude
{
    public partial class UcControleIndexe : ChildWindow
    {
        CsDemandeFraude LaDemande = new CsDemandeFraude();
        int EtapeActuelle;
        public UcControleIndexe()
        {
            InitializeComponent();
        }
        public UcControleIndexe(List<int> demande, int etape)
        {
            InitializeComponent();
            ChargeDonneDemande(demande.First());
            EtapeActuelle = etape;
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LaDemande.CompteurFraude.IsEcartIndex = (bool)ckbEcartAFacturer.IsChecked;
            Validationdemande(LaDemande);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargeDonneDemande(int pk_id)
        {

            FraudeServiceClient service = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude"));
            service.RetourDemandeFraudeCompleted += (s, args) =>
            {
                try
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;

                    if (LaDemande != null)
                    {
                        ///Information iwbes
                        txt_CompteurIwebs.Text = string.IsNullOrEmpty(LaDemande.Canalisation.NUMERO) ? string.Empty : LaDemande.Canalisation.NUMERO;
                        txt_marqueIwebs.Text = string.IsNullOrEmpty(LaDemande.Canalisation.MARQUE) ? string.Empty : LaDemande.Canalisation.MARQUE;
                        txt_CommunIwebs.Text = string.IsNullOrEmpty(LaDemande.Ag.COMMUNE) ? string.Empty : LaDemande.Ag.COMMUNE;
                        txt_quartierIwebs.Text = string.IsNullOrEmpty(LaDemande.Ag.QUARTIER) ? string.Empty : LaDemande.Ag.QUARTIER;
                        txt_RueIwbes.Text = string.IsNullOrEmpty(LaDemande.Ag.RUE) ? string.Empty : LaDemande.Ag.RUE;
                        txt_porteIwebs.Text = string.IsNullOrEmpty(LaDemande.Ag.PORTE) ? string.Empty : LaDemande.Ag.PORTE;
                        txt_TourneeIwebs.Text = string.IsNullOrEmpty(LaDemande.Ag.TOURNEE) ? string.Empty : LaDemande.Ag.TOURNEE;
                        txt_ordreTourne.Text = string.IsNullOrEmpty(LaDemande.Ag.ORDTOUR) ? string.Empty : LaDemande.Ag.ORDTOUR;
                        //Information Fraude
                        ///Information iwbes
                        txt_CompteurFrs.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Commune) ? string.Empty : LaDemande.ClientFraude.Commune;
                        txt_MarqueFrs.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.MARQUE) ? string.Empty : LaDemande.CompteurFraude.MARQUE;
                        txt_CommunFrs.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.MARQUE) ? string.Empty : LaDemande.CompteurFraude.MARQUE;
                        txt_quartierFrs.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Quartier) ? string.Empty : LaDemande.ClientFraude.Quartier;
                        txt_rueFrs.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Rue) ? string.Empty : LaDemande.ClientFraude.Rue;
                        txt_portedfrs.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Porte) ? string.Empty : LaDemande.ClientFraude.Porte;
                        txt_IndexFrs.Text = string.IsNullOrEmpty(LaDemande.CompteurFraude.IndexCompteur.ToString()) ? string.Empty : LaDemande.CompteurFraude.IndexCompteur.ToString();
                        //txt_ordreTourne.Text = string.IsNullOrEmpty(LaDemande.Ag.ORDTOUR) ? string.Empty : LaDemande.Ag.ORDTOUR;
                      
                        txt_Nom.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Nomabon) ? string.Empty : LaDemande.ClientFraude.Nomabon;
                        txt_refAbon.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Client) ? string.Empty : LaDemande.ClientFraude.Client; ;
                        txt_Centre.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Centre) ? string.Empty : LaDemande.ClientFraude.Centre; ;
                        //txt_telephone.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Telephone) ? string.Empty : LaDemande.ClientFraude.Telephone; ;
                        //txt_porte.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.Porte) ? string.Empty : LaDemande.ClientFraude.Porte; ;
                        //txt_ContactAbonne.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratAbonnement) ? string.Empty : LaDemande.ClientFraude.ContratAbonnement;
                        //txt_contarBrachement.Text = string.IsNullOrEmpty(LaDemande.ClientFraude.ContratBranchement) ? string.Empty : LaDemande.ClientFraude.ContratBranchement;
                        //txt_Numerotraitement.Text = string.IsNullOrEmpty(LaDemande.LaDemande.NUMDEM) ? string.Empty : LaDemande.LaDemande.NUMDEM;
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }

            };
            service.RetourDemandeFraudeAsync(pk_id);

        }

        private void Validationdemande(CsDemandeFraude LaDemande)
        {
            try
            {
                FraudeServiceClient Client = new FraudeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Fraude")); ;
                Client.ValiderDemandeControleIndexCompleted += (ss, b) =>
                {
                    if (b.Cancelled || b.Error != null)
                    {
                        string error = b.Error.Message;
                        Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                        return;
                    }
                    if (b.Result != null)
                    {
                        Message.Show("Affectation effectuée avec succès", "Information");
                        List<int> Listid = new List<int>();
                        Listid.Add(LaDemande.LaDemande.PK_ID);
                        EnvoyerDemandeEtapeSuivante(Listid);
                        this.DialogResult = true;
//ici


                    }
                    else
                        Message.ShowError("Erreur a la cloture de la demande", "Cloturedemande");
                };
                Client.ValiderDemandeControleIndexAsync(LaDemande);

            }
            catch (Exception ex)
            {
                this.DialogResult = false;
                Message.Show(ex.Message, "Transmit");
            }
        }
        private void EnvoyerDemandeEtapeSuivante(List<int> Listid)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            clientWkf.ExecuterActionSurDemandeParPkIDLigneCompleted += (wkf, wsen) =>
            {
                if (null != wsen && wsen.Cancelled)
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                if (string.Empty != wsen.Result && wsen.Result.StartsWith("ERR"))
                {
                    Message.ShowError("Echec de sortie materiel", Langue.lbl_Menu);
                    return;
                }
                else
                {
                    //Message.ShowInformation("Sortie materiel éffectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }
    }
}

