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
    public partial class UcSortieMateriel : ChildWindow
    {

        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        List<CsSortieMateriel> lstSortie = new List<CsSortieMateriel>();
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsDemandeBase> listeDemandeSelectionees = new List<CsDemandeBase>();
        List<CsCanalisation> LstDemandeValide = new List<CsCanalisation>();
        List<CsSortieAutreMateriel> lstAutreMateriel = new List<CsSortieAutreMateriel>();
        int? EtapeActuelle;
        int fkiddemande = 0;
        int i = 0;
        CsDemande LaDemande = new CsDemande();
        List<CsDemande> ListDemande = new List<CsDemande>();

        public UcSortieMateriel(int demande)
        {
            fkiddemande = demande;
            InitializeComponent();
           


            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();

            List<int> lstdema = new List<int>();
            lstdema.Add(demande);
            ChargeListeUser();
        }
        List<int> lesCentrePerimetre = new List<int>();
        ServiceAccueil.CsProgarmmation LeProgramme = new CsProgarmmation();
        public UcSortieMateriel(ServiceAccueil.CsProgarmmation _leProgramme, int etape)
        {
            this.EtapeActuelle = etape;
            InitializeComponent();

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();
            LeProgramme = _leProgramme;
            LeProgramme = _leProgramme;
            this.txt_DateProgramme.Text = LeProgramme.DATEPROGRAMME.Value.ToShortDateString();
            this.txt_Equipe.Text = LeProgramme.LIBELLEEQUIPE;
            ChargeListeUser();
            ChargerDetailSortieMateriel(LeProgramme);
        }
        public UcSortieMateriel(ServiceAccueil.CsProgarmmation _leProgramme)
        {
            this.EtapeActuelle = null;
            InitializeComponent();

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();
            LeProgramme = _leProgramme;
            this.txt_DateProgramme.Text = LeProgramme.DATEPROGRAMME.Value.ToShortDateString();
            this.txt_Equipe.Text = LeProgramme.LIBELLEEQUIPE;
            LeProgramme = _leProgramme;
            ChargeListeUser();
            ChargerDetailSortieMateriel(LeProgramme);
        }
        CsCanalisation leDetailDemande = new CsCanalisation();
        CsCanalisation demandecheck = new CsCanalisation();
        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> lesElementsDesDevisSelect = new List<CsCanalisation>();
            demandecheck = ((CheckBox)sender).Tag as CsCanalisation;
            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
            {
                List<CsCanalisation> lesDemandeSelect = ((List<CsCanalisation>)this.dgDemande.ItemsSource).Where(t => t.IsSelect == true).ToList();
                foreach (CsCanalisation item in lesDemandeSelect)
                    lesElementsDesDevisSelect.AddRange( LstCannalisation.Where(t => t.FK_IDDEMANDE == item.FK_IDDEMANDE).ToList());

            }
           var reglemntParModereg = (from p in lesElementsDesDevisSelect
                                          group new { p } by new { p.LIBELLEMATERIEL, p.FK_IDMATERIELDEVIS   } into pResult
                                          select new
                                          {
                                              pResult.Key.LIBELLEMATERIEL ,
                                              pResult.Key.FK_IDMATERIELDEVIS,
                                              QUANTITE = (int)pResult.Where(t => t.p.FK_IDMATERIELDEVIS  == pResult.Key.FK_IDMATERIELDEVIS).Sum(o => o.p.QUANTITE )
                                          });
           List<CsCanalisation> lstEltDevisAffiche = new List<CsCanalisation>();
           foreach (var item in reglemntParModereg)
               lstEltDevisAffiche.Add(new CsCanalisation() { LIBELLEMATERIEL = item.LIBELLEMATERIEL ,QUANTITE =item.QUANTITE   });

           dgAutreMateriel.ItemsSource = null;
           dgAutreMateriel.ItemsSource = lstEltDevisAffiche;

        }
        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            CsCanalisation demandecheck = ((CheckBox)sender).Tag as CsCanalisation;


            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
            {
                LstDemandeValide = LstDemandeValide.Where(c => c.PK_ID != demandecheck.PK_ID).ToList();
            }

            MiseAjourElementDevis();
        }

        private void MiseAjourElementDevis()
        {
            var ListeDemande = ListDemande.Where(d =>  LstDemandeValide.Select(c => c.FK_IDDEMANDE).Contains(d.LaDemande.PK_ID));
            List<ObjELEMENTDEVIS> ElementPrisEnConte = new List<ObjELEMENTDEVIS>();
            foreach (var item in ListeDemande)
                 ElementPrisEnConte.AddRange(item.EltDevis.Where(t =>t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null && t.MONTANTTAXE != 0).ToList());

          dgAutreMateriel .ItemsSource=  RetournListeElementAggreger(ElementPrisEnConte);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Tag != null && this.txtAgt_Livreur.Tag != null && ((List<CsCanalisation>)dgDemande.ItemsSource).Count() > 0)
            {
                ValiderChoix(((List<CsCanalisation>)this.dgDemande.ItemsSource).Where(t => t.IsSelect == true).ToList());
            }
            else
                Message.ShowWarning("Veuillez vous assurer que les agens récepteur et livreurs sont selectionné ", Langue.lbl_Menu);
        }

        private void ValiderElementdevis(List<CsDemande> Lstdemande, List<int> Listid, List<CsDemande> LstdemandeRejet, List<int> ListidRejet, List<CsCanalisation> LstDemandeValide)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            LaDemande.EltDevis = (List<ObjELEMENTDEVIS>)dgAutreMateriel.ItemsSource;
            Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient client = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            client.MiseAJourElementDevisCompleted += (ss, b) =>
            {
                if (b.Cancelled || b.Error != null)
                {
                    string error = b.Error.Message;
                    Message.ShowError(error, Silverlight.Resources.Devis.Languages.txtDevis);
                    return;
                }
                EnvoyerDemandeEtapeSuivante(Listid, clientWkf, client);
                RejeterDemandeEtapePrecedente(ListidRejet, clientWkf, client);
                DesactiverProgrammation(Listid, ListidRejet, client);
            };
            client.MiseAJourElementDevisAsync(Lstdemande);
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
                    this.DialogResult = true;
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle.Value , SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
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
                    this.DialogResult = true;
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle.Value , SessionObject.Enumere.REJETER, UserConnecte.matricule,
                string.Empty);
        }

        private void ValiderChoix(List<CsCanalisation> lstdemande)
        {
            lstdemande.ForEach(u => u.USERCREATION = UserConnecte.matricule);
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.InsertSortieMaterielCompleted += (sr, res2) =>
            {
                if (res2 != null && res2.Cancelled)
                    return;
                if (string.IsNullOrEmpty(res2.Result))
                {
                    Message.ShowInformation("Sortie matériel effectuée", Langue.lbl_Menu);
                    EditionSortieMateriel(lstdemande.Select(d => d.FK_IDDEMANDE.Value).ToList());
                }
                else
                    Message.ShowError(res2.Result, "Demande");
            };
            service1.InsertSortieMaterielAsync((int)this.txtAgt_Livreur.Tag, (int)this.txtAgt_Recepteur.Tag, EtapeActuelle.Value, lstdemande, false);
            service1.CloseAsync();
        }
        private void EditionSortieMateriel(List<int> demandes)
        {
            try
            {
                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneElementDEvisFromIdDemandeCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    res.Result.ForEach(t => t.NUMFOURNITURE = LeProgramme.LIBELLEEQUIPE );
                    res.Result.ForEach(t => t.QUANTITERECAP = this.txt_LibelleAgentLivreur.Text);
                    res.Result.ForEach(t => t.MontantRecap = this.txt_LibelleAgentRecepteur.Text);

                    res.Result.ForEach(t => t.COMMUNE  = LeProgramme.LIBELLEEQUIPE);
                    res.Result.ForEach(t => t.NUMDEVIS = LeProgramme.DATEPROGRAMME.Value.ToShortDateString());
                    res.Result.ForEach(t => t.USERMODIFICATION = LeProgramme.NUMPROGRAMME);

                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(res.Result, null, SessionObject.CheminImpression, "SortieMateriel", "Devis", true);

                };
                service1.RetourneElementDEvisFromIdDemandeAsync(demandes);
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

        private void RetourneDetailDemande(List<CsDemandeBase>  laDemandeSelect)
        {
            try
            {
                
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetDemandeByListeNumIdDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListDemande = args.Result;

                    if (ListDemande!= null &&  ListDemande.Count > 0)
                    {
                        List<CsCanalisation> ListeDeCanalisation = new List<CsCanalisation>();
                        foreach (var item in ListDemande)
                        {
                            if (item.LstCanalistion != null && item.LstCanalistion.Count != 0)
                            {
                                item.LstCanalistion.ForEach(t => t.NOMCLIENT = item.LeClient.NOMABON);
                                item.EltDevis.ForEach(e => e.QUANTITEALIVRET = e.QUANTITE);
                                ListeDeCanalisation.Add(item.LstCanalistion.First());
                                ListeSortieMateriel(item);
                            }
                        }
                        ListeSortieAutreMateriel(LaDemande);
                        dgDemande.ItemsSource = ListeDeCanalisation;
                    }
                    else
                    {
                        Message.Show("Aucune demande trouvé", "Information");
                    }
                    
                };
                service.GetDemandeByListeNumIdDemandeAsync(laDemandeSelect.Select(d=>d.PK_ID).ToList());
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LstDemandeValide = new List<CsCanalisation>();
                throw ex;
            }
        }
        private List<ObjELEMENTDEVIS> RetournListeElementAggreger(List<ObjELEMENTDEVIS> ElementDeDevis)
        {
            List<ObjELEMENTDEVIS> ElementDeDevisAggrege = (from e in ElementDeDevis
                                                           group new
                                                           {
                                                               e.NUMDEVIS,
                                                               e.NUMDEM,
                                                               e.ORDRE,
                                                               e.QUANTITE,
                                                               e.QUANTITEREMISENSTOCK,
                                                               e.QUANTITECONSOMMEE,
                                                               e.QUANTITELIVRET,
                                                               e.QUANTITEALIVRET,
                                                               e.TAXE,
                                                               e.COUT,
                                                               e.DESIGNATION,
                                                               e.PRIX,
                                                               e.REMBOURSEMENT,
                                                               e.MONTANT,
                                                               e.MONTANTCONSOMME,
                                                               e.MONTANTVALIDE,
                                                               e.PRIX_UNITAIRE,
                                                               e.ISSUMMARY,
                                                               e.ISADDITIONAL,
                                                               e.ISFORTRENCH,
                                                               e.ISDEFAULT,
                                                               e.UTILISE,
                                                               e.COUTRECAP,
                                                               e.TVARECAP,
                                                               e.QUANTITERECAP,
                                                               e.MontantRecap,
                                                               e.REMISE,
                                                               e.CONSOMME,
                                                               e.COUTTOTAL,
                                                               e.DATECREATION,
                                                               e.DATEMODIFICATION,
                                                               e.USERCREATION,
                                                               e.USERMODIFICATION,
                                                               e.CODECOPER,
                                                               e.FK_IDCOPER,
                                                               e.FK_IDTAXE,
                                                               e.FK_IDTDEM,
                                                               e.FK_IDMATERIELDEVIS ,
                                                               e.FK_IDCOUTCOPER,
                                                               e.FK_IDDEMANDE
                                                           } by new
                                                           {
                                                               e.DESIGNATION,
                                                               e.NUMFOURNITURE
                                                           }
                                                               into groupe
                                                               select new ObjELEMENTDEVIS
                                                               {
                                                                   NUMFOURNITURE=groupe.Key.NUMFOURNITURE,
                                                                   DESIGNATION = groupe.Key.DESIGNATION,
                                                                   QUANTITE = groupe.Sum(g => g.QUANTITE),
                                                                   QUANTITEALIVRET = groupe.Sum(g => g.QUANTITEALIVRET)

                                                               }).ToList();

            return ElementDeDevisAggrege;
        }
      
        private void ListeSortieMateriel(CsDemande demande)
        {

            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service1.RetourneListeSortieMaterielLivreCompleted += (sr, res2) =>
            {
                if (res2 != null && res2.Cancelled)
                    return;
                if (res2.Result.Count() > 0)
                {
                    lstSortie = res2.Result;
                    demande.LstCanalistion.ForEach(c => c.ISLIVRE = false);
                    foreach (CsSortieMateriel sortie in lstSortie)
                    {
                        foreach (CsCanalisation canal in demande.LstCanalistion)
                        {
                            //if (sortie.FK_IDCANALISATION == canal.PK_ID)
                            //{
                                canal.ISLIVRE = true;
                                LstDemandeValide.Add(canal);
                            //}
                        }

                    }

                    LstDemandeValide = LstDemandeValide.Where(c => c.PK_ID != 0).ToList();
                    LstDemandeValide = new List<CsCanalisation>();

                }
                else
                {
                    demande.LstCanalistion.ForEach(c => c.ISLIVRE = false);
                    LstDemandeValide = new List<CsCanalisation>();
                }

            };
            service1.RetourneListeSortieMaterielLivreAsync(demande.LaDemande.PK_ID);
            service1.CloseAsync();

        }
        private void ListeSortieAutreMateriel(CsDemande demande)
        {
            dgAutreMateriel.ItemsSource = demande.EltDevis;
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAutreMateriel.SelectedItem != null)
                {
                    CsSortieAutreMateriel lAutreSelect = (CsSortieAutreMateriel)dgAutreMateriel.SelectedItem;

                    dgAutreMateriel.ItemsSource = null;
                    lstAutreMateriel = lstAutreMateriel.Where(c => c.LIBELLE != lAutreSelect.LIBELLE && c.NOMBRE != lAutreSelect.NOMBRE).ToList();
                    dgAutreMateriel.ItemsSource = lstAutreMateriel;
                }
            }
            catch
            {
            }
        }
        List<CsCanalisation> LstCannalisation = new List<CsCanalisation>();
        private void ChargerDetailSortieMateriel(ServiceAccueil.CsProgarmmation leProgramme)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.GetDemandeByListeNumIdDemandeSortieMaterielCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    if (res.Result != null && res.Result.Count != 0)
                    {
                        LstCannalisation = res.Result;
                        List<CsCanalisation> lstElementDatagrid = new List<CsCanalisation>();
                        var DistinctCannalisation = res.Result.Select(u => new { u.FK_IDDEMANDE, u.NUMDEM, u.NUMERO, u.NOMCLIENT, u.CENTRE, u.CLIENT, u.ORDRE }).Distinct();
                        foreach (var item in DistinctCannalisation)
                            lstElementDatagrid.Add(new CsCanalisation() { FK_IDDEMANDE = item.FK_IDDEMANDE, NUMDEM = item.NUMDEM, NUMERO = item.NUMERO, NOMCLIENT = item.NOMCLIENT, CENTRE = item.CENTRE, CLIENT = item.CLIENT, ORDRE = item.ORDRE });

                        dgDemande.ItemsSource = null;
                        dgDemande.ItemsSource = lstElementDatagrid;
                    }
                    else
                        Message.Show("Aucune demande trouvé", "Information");
                };
                service1.GetDemandeByListeNumIdDemandeSortieMaterielAsync(leProgramme.NUMPROGRAMME, EtapeActuelle);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c => c.CENTRE == UserConnecte.Centre).ToList());
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
                    List<object> _LstObj = Galatee.Silverlight.Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstAllUser.Where(c => c.CENTRE == UserConnecte.Centre).ToList());
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
                AgregerMaterielDevis();
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
            AgregerMaterielDevis();
         
        }

        private void btn_Rien_Click(object sender, RoutedEventArgs e)
        {
            List<CsCanalisation> allObjects = this.dgDemande.ItemsSource as List<CsCanalisation>;
            if (allObjects != null && allObjects.Count != 0)
            {
                allObjects.ForEach(y => y.IsSelect = false );
                allObjects.ForEach(y => y.ETATDUCOMPTEUR = "");
            }
            AgregerMaterielDevis();
        }

        private void AgregerMaterielDevis()
        {
            List<CsCanalisation> lesElementsDesDevisSelect = new List<CsCanalisation>();
            List<CsCanalisation> lesDemandeSelect = ((List<CsCanalisation>)this.dgDemande.ItemsSource).Where(t => t.IsSelect == true).ToList();
            foreach (CsCanalisation item in lesDemandeSelect)
                lesElementsDesDevisSelect.AddRange(LstCannalisation.Where(t => t.FK_IDDEMANDE == item.FK_IDDEMANDE).ToList());

            var reglemntParModereg = (from p in lesElementsDesDevisSelect
                                      group new { p } by new { p.LIBELLEMATERIEL, p.FK_IDMATERIELDEVIS } into pResult
                                      select new
                                      {
                                          pResult.Key.LIBELLEMATERIEL,
                                          pResult.Key.FK_IDMATERIELDEVIS,
                                          QUANTITE = (int)pResult.Where(t => t.p.FK_IDMATERIELDEVIS == pResult.Key.FK_IDMATERIELDEVIS).Sum(o => o.p.QUANTITE)
                                      });
            List<CsCanalisation> lstEltDevisAffiche = new List<CsCanalisation>();
            foreach (var item in reglemntParModereg)
                lstEltDevisAffiche.Add(new CsCanalisation() { LIBELLEMATERIEL = item.LIBELLEMATERIEL, QUANTITE = item.QUANTITE });

            dgAutreMateriel.ItemsSource = null;
            dgAutreMateriel.ItemsSource = lstEltDevisAffiche;
        
        }
    }
}

