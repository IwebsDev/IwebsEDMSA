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
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.ServiceAccueil;

namespace Galatee.Silverlight.Devis
{
    public partial class UcSortieCompteurDetail : ChildWindow
    {

        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        int? EtapeActuelle;
        int fkiddemande = 0;
        int i = 0;
        string TypeDemande = string.Empty;
        ServiceAccueil.CsProgarmmation LeProgramme = new CsProgarmmation();
        public UcSortieCompteurDetail(ServiceAccueil.CsProgarmmation _leProgramme, int IdEtape)
        {
            this.EtapeActuelle = IdEtape;
            InitializeComponent();
            ChargeListeUser();
            LeProgramme = _leProgramme;
            ChargerDetailSortieCompteur(_leProgramme);
        }
  
        private void ChargeListeUser()
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service1 = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeAllUserCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    lstAllUser = res.Result;
                };
                service1.RetourneListeAllUserAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        void galatee_OkClickedbtn_AgtLivreur(object sender, EventArgs e)
        {
            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtAgt_Livreur.Text = utilisateur.MATRICULE;
                this.txtAgt_Livreur.Tag = utilisateur.PK_ID;
            }
        }
        private void btn_AgtLivreur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_AgtLivreur);
                    ctr.Show();
                }
                else
                {
                    Message.ShowInformation("Aucun utilisareur trouvée", "Information");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtAgt_Livreur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtAgt_Livreur.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_Livreur.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleAgentLivreur.Text = leuser.LIBELLE;
                        txtAgt_Livreur.Tag = leuser.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtAgt_Livreur.Focus();
                    }
                }
            }
        }

        void galatee_OkClickedbtn_AgtRecepteur(object sender, EventArgs e)
        {

            Galatee.Silverlight.MainView.UcListeGenerique ctrs = sender as Galatee.Silverlight.MainView.UcListeGenerique;
            if (ctrs.isOkClick)
            {
                ServiceAccueil.CsUtilisateur utilisateur = (ServiceAccueil.CsUtilisateur)ctrs.MyObject;
                this.txtAgt_Recepteur.Text = utilisateur.MATRICULE;
                this.txtAgt_Recepteur.Tag = utilisateur.PK_ID;
            }
        }
        private void btn_AgtRecepteur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser);
                    Galatee.Silverlight.MainView.UcListeGenerique ctr = new Galatee.Silverlight.MainView.UcListeGenerique(_LstObj, "MATRICULE", "LIBELLE", "");
                    ctr.Closed += new EventHandler(galatee_OkClickedbtn_AgtRecepteur);
                    ctr.Show();
                }
                else
                {
                    Message.ShowInformation("Aucun utilisareur trouvée", "Information");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void txtAgt_Recepteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_Recepteur.Text);
                    if (leuser != null)
                    {
                        this.txt_LibelleAgentRecepteur.Text = leuser.LIBELLE;
                        txtAgt_Recepteur.Tag = leuser.PK_ID;
                    }
                    else
                    {
                        Message.ShowInformation("Aucun utilisateur n'existe ", "Information");
                        this.txtAgt_Recepteur.Focus();
                    }
                }
            }
        }

 
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btn_transmetre.IsEnabled = false;
                if (!string.IsNullOrEmpty(txtAgt_Recepteur.Text) && !string.IsNullOrEmpty(txtAgt_Livreur.Text) &&
                 ((List<CsCanalisation>)dgDemande.ItemsSource).Count() > 0)
                    ValiderChoix(((List<CsCanalisation>)dgDemande.ItemsSource).Where(t => t.IsSelect).ToList());
                else
                {
                    Message.ShowWarning("Veuillez vous assurer que les agents récepteurs et livreurs sont selectionnés ", Langue.lbl_Menu);
                    this.btn_transmetre.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        private static void DesactiverProgrammation(List<int> Listid, List<int> ListidRejet, Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client)
        {
            client.DesactivationProgrammationCompleted += (s_s, b_) =>
            {
                if (b_.Cancelled || b_.Error != null)
                {
                    string error = b_.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
            };
            List<int> lstIddmd = ListidRejet;
            lstIddmd.AddRange(Listid);
            client.DesactivationProgrammationAsync(lstIddmd);
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid, ServiceWorkflow.WorkflowClient clientWkf, Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client)
        {
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
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid,EtapeActuelle.Value, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
                string.Empty);
        }
        private void RejeterDemandeEtapePrecedente(List<int> Listid, ServiceWorkflow.WorkflowClient clientWkf, Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client)
        {
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
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle.Value , SessionObject.Enumere.REJETER, UserConnecte.matricule,
                string.Empty);
        }

        private void ValiderChoix(List<CsCanalisation> lesCompteurs)
        {
            try
            {
                lesCompteurs.ForEach(u => u.USERCREATION = UserConnecte.matricule);
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.InsertSortieCompteurCompleted += (sr, res2) =>
                {
                    if (res2 != null && res2.Cancelled)
                        return;
                    if (string.IsNullOrEmpty(res2.Result))
                    {
                        Message.ShowInformation("Sortie compteur effectuée", Langue.lbl_Menu);
                        lesCompteurs.ForEach(t => t.COMMENTAIRE = this.txt_LibelleAgentLivreur.Text);
                        lesCompteurs.ForEach(t => t.ETATDUCOMPTEUR = this.txt_LibelleAgentRecepteur.Text);
                        lesCompteurs.ForEach(t => t.CODECALIBRECOMPTEUR = LeProgramme.LIBELLEEQUIPE);
                        lesCompteurs.ForEach(t => t.BRANCHEMENT = LeProgramme.DATEPROGRAMME.Value.ToShortDateString());
                        lesCompteurs.ForEach(t => t.FONCTIONNEMENT = LeProgramme.NUMPROGRAMME);

                        Utility.ActionDirectOrientation<ServicePrintings.CsCanalisation, CsCanalisation>(lesCompteurs, null, SessionObject.CheminImpression, "SortieCompteur1", "Devis", true);
                        DialogResult = true;
                    }
                    else
                    {
                        Message.ShowError("Sortie compteur échouée", Langue.lbl_Menu);
                        return;
                    }
                };
                service1.InsertSortieCompteurAsync(LeProgramme.NUMPROGRAMME, (int)this.txtAgt_Livreur.Tag, (int)this.txtAgt_Recepteur.Tag, EtapeActuelle.Value, lesCompteurs);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

 
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChargerDetailSortieCompteur(ServiceAccueil.CsProgarmmation leProgramme)
        {
            try
            {
                List<CsCanalisation> LstCannalisation = new List<CsCanalisation>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.GetDemandeByListeNumIdDemandeSortieCompteurCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        LstCannalisation = res.Result;
                        dgDemande.ItemsSource = null;
                        dgDemande.ItemsSource = LstCannalisation;

                        this.txt_Equipe.Text = leProgramme.LIBELLEEQUIPE;
                        this.txt_DateProgramme.Text = leProgramme.DATEPROGRAMME.Value.ToShortDateString();
                    }
                    else
                        Message.Show("Aucune demande trouvé", "Information");
                };
                service1.GetDemandeByListeNumIdDemandeSortieCompteurAsync(leProgramme.NUMPROGRAMME, EtapeActuelle);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void dgMyDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dg = (sender as DataGrid);
            var allObjects = dg.ItemsSource as List<CsCanalisation>;
            if (dg.SelectedItem != null)
            {
                CsCanalisation SelectedObject = (CsCanalisation)dg.SelectedItem;
                if (SelectedObject.IsSelect  == false)
                {
                    SelectedObject.IsSelect = true;
                    SelectedObject.ETATDUCOMPTEUR = "Livré";
                }
                else
                {
                    SelectedObject.IsSelect = false;
                    SelectedObject.ETATDUCOMPTEUR = "";
                }
            }
        }

        private void btn_tout_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> allObjects = this.dgDemande.ItemsSource as List<CsCanalisation>;
            if (allObjects != null && allObjects.Count != 0)
            {
                allObjects.ForEach(y => y.IsSelect = true);
                allObjects.ForEach(y => y.ETATDUCOMPTEUR = "Livré");
            }
        }

        private void btn_Rien_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> allObjects = this.dgDemande.ItemsSource as List<CsCanalisation>;
            if (allObjects != null && allObjects.Count != 0)
            {
                allObjects.ForEach(y => y.IsSelect = false );
                allObjects.ForEach(y => y.ETATDUCOMPTEUR = "");
            }
        }

        private void Btn_imprimer_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> lesCompteurs = (List<CsCanalisation>)dgDemande.ItemsSource ;
            if (lesCompteurs != null && lesCompteurs.Count != 0)
            {
                lesCompteurs.ForEach(t => t.COMMENTAIRE = this.txt_LibelleAgentLivreur.Text);
                lesCompteurs.ForEach(t => t.ETATDUCOMPTEUR = this.txt_LibelleAgentRecepteur.Text);
                lesCompteurs.ForEach(t => t.CODECALIBRECOMPTEUR = LeProgramme.LIBELLEEQUIPE);
                lesCompteurs.ForEach(t => t.BRANCHEMENT = LeProgramme.DATEPROGRAMME.Value.ToShortDateString());
                Utility.ActionDirectOrientation<ServicePrintings.CsCanalisation, CsCanalisation>(lesCompteurs, null, SessionObject.CheminImpression, "SortieCompteur1", "Devis", true);
            }
        }

  
    }
}

