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
    public partial class UcSortieMaterielExtension : ChildWindow
    {

        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        List<CsSortieMateriel> lstSortie = new List<CsSortieMateriel>();
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsDemandeBase> listeDemandeSelectionees = new List<CsDemandeBase>();
        List<CsCanalisation> LstDemandeValide = new List<CsCanalisation>();
        List<CsSortieAutreMateriel> lstAutreMateriel = new List<CsSortieAutreMateriel>();
        int EtapeActuelle;
        int fkiddemande = 0;
        int i = 0;
        CsDemande LaDemande = new CsDemande();
        List<CsDemande> ListDemande = new List<CsDemande>();

        public UcSortieMaterielExtension(int demande)
        {
            fkiddemande = demande;
            InitializeComponent();
            //ChargerDonneeDuSite();


            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();

            List<int> lstdema = new List<int>();
            lstdema.Add(demande);
            RechercheDemande(lstdema);
            ChargeListeUser();
        }
        List<int> lesCentrePerimetre = new List<int>();

        List<int> lstIdDemande = new List<int>();
        public UcSortieMaterielExtension(List<int> demande, int etape)
        {
            this.EtapeActuelle = etape;
            InitializeComponent();

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            lstAutreMateriel = new List<CsSortieAutreMateriel>();
            lstIdDemande = demande;
            ChargeListeUser();
            ChargeEquipe(UserConnecte.Centre);
            RechercheTypedemande(etape);
        }
        private void RechercheTypedemande(int idetape)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneTypeDemandeFromIdEtapeWkfCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    CsTdem leType = new CsTdem();
                    leType = res.Result;
                    ChargeProgrammation(leType.CODE);

                };
                service1.RetourneTypeDemandeFromIdEtapeWkfAsync(idetape);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void ChargeProgrammation(string tdem)
        {
            try
            {
                List<CsGroupe> lstequipe = new List<CsGroupe>();
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneProgrammationCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstprogrammation = res.Result.Where(t => t.TYPEDEMANDE == tdem).ToList();


                };
                service1.RetourneProgrammationAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private void RechercheDemande(List<int> pk_id)
        {
            try
            {
                List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeByIdCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        RetourneDetailDemande(LstDemande);

                    }
                };
                service1.RetourneListeDemandeByIdAsync(pk_id);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        CsCanalisation leDetailDemande = new CsCanalisation();
        CsCanalisation demandecheck = new CsCanalisation();
        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            demandecheck = ((CheckBox)sender).Tag as CsCanalisation;
            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
                LstDemandeValide.Add(_LaDemandeSelect);

            MiseAjourElementDevis();

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
                    ElementPrisEnConte.AddRange(item.EltDevis.Where(t =>t.ISEXTENSION == true && t.ISFOURNITURE == true && t.FK_IDMATERIELDEVIS != 0 && t.FK_IDMATERIELDEVIS != null).ToList());

          dgAutreMateriel .ItemsSource=  RetournListeElementAggreger(ElementPrisEnConte);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Tag != null && this.txtAgt_Livreur.Tag != null && ((List<CsCanalisation>)dgDemande.ItemsSource).Count() > 0)
            {
                List<CsDemande> ListDmd = new List<CsDemande>();
                List<CsDemande> ListDmdRejet = new List<CsDemande>();
                List<int> Listid = new List<int>();
                List<int> ListidRejet = new List<int>();
                foreach (var item in ListDemande)
	            {
                    var can=LstDemandeValide.FirstOrDefault(d => d.FK_IDDEMANDE==item.LaDemande.PK_ID);
                    if (can!=null)
                    {
                        item.EltDevis.ForEach(d => d.QUANTITEALIVRET = d.QUANTITE);
                        ListDmd.Add(item);
                        Listid.Add(item.LaDemande.PK_ID);
                    }
                    else
                    {
                        ListDmdRejet.Add(item);
                        ListidRejet.Add(item.LaDemande.PK_ID);
                    }
	            }
                List<ObjELEMENTDEVIS > lesElementDevis = new List<ObjELEMENTDEVIS>();
                foreach (CsDemande item in ListDmd)
                    lesElementDevis.AddRange(item.EltDevis.Where(t => t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0).ToList());
                //ValiderElementdevis(ListDmd, Listid, ListDmdRejet, ListidRejet, LstDemandeValide);
                ValiderChoix(LstDemandeValide, lesElementDevis, ListDmd, Listid, ListDmdRejet, ListidRejet);
            }
            else
            {
                Message.ShowWarning("Veuillez vous assurer que les agens récepteur et livreurs sont selectionné ", Langue.lbl_Menu);
            }
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
                {
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.TRANSMETTRE, UserConnecte.matricule,
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
                    //Message.ShowInformation("Sortie materiel effectuée", Langue.lbl_Menu);

                   
                    this.DialogResult = true;
                }
            };
            clientWkf.ExecuterActionSurDemandeParPkIDLigneAsync(Listid, EtapeActuelle, SessionObject.Enumere.REJETER, UserConnecte.matricule,
                string.Empty);
        }

        private void ValiderChoix(List<CsCanalisation> lstdemande, List<ObjELEMENTDEVIS> lesObjetDevis, List<CsDemande> Lstdemande, List<int> Listid, List<CsDemande> LstdemandeRejet, List<int> ListidRejet)
        {
            ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.Protocole(), Utility.EndPoint("Workflow"));
            lstAutreMateriel = new List<CsSortieAutreMateriel>();

            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.Protocole(), Utility.EndPoint("Accueil"));
            service1.InsertSortieMaterielCompleted += (sr, res2) =>
            {
                if (res2 != null && res2.Cancelled)
                    return;
                if (string.IsNullOrEmpty(res2.Result))
                {
                    Message.ShowInformation("Sortie matériel effectuée", Langue.lbl_Menu);
                    EnvoyerDemandeEtapeSuivante(lstdemande.Select(d => d.FK_IDDEMANDE.Value).ToList(), clientWkf, service1);
                    List<ObjELEMENTDEVIS> lesElemtEditer = lesObjetDevis;
                    lesElemtEditer.ForEach(t => t.NUMDEVIS = this.dtProgram.SelectedDate.Value.ToShortDateString());
                    lesElemtEditer.ForEach(t => t.NUMFOURNITURE = ((CsGroupe)this.cboEquipe.SelectedItem).LIBELLE);
                    lesElemtEditer.ForEach(t => t.QUANTITERECAP = this.txt_LibelleAgentLivreur.Text);
                    lesElemtEditer.ForEach(t => t.MontantRecap = this.txt_LibelleAgentRecepteur.Text);
                    Utility.ActionDirectOrientation<ServicePrintings.ObjELEMENTDEVIS, ObjELEMENTDEVIS>(lesElemtEditer, null, SessionObject.CheminImpression, "SortieMateriel", "Devis", true);
                }
                else
                {
                    Message.ShowError("Sortie compteur échouée", Langue.lbl_Menu);
                    return;
                }
            };
            service1.InsertSortieMaterielAsync((int)this.txtAgt_Livreur.Tag, (int)this.txtAgt_Recepteur.Tag, EtapeActuelle, LstDemandeValide, true);
            service1.CloseAsync();
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

                        //ListeSortieMateriel(LaDemande);
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

        private void ChargeEquipe(string p)
        {
            try
            {
                List<CsGroupe> lstequipe = new List<CsGroupe>();
                //Lancer la transaction de mise a jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeGroupeCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstequipe = res.Result;
                    cboEquipe.ItemsSource = lstequipe;
                    cboEquipe.SelectedValuePath = "ID";
                    cboEquipe.DisplayMemberPath = "LIBELLE";

                };
                service1.RetourneListeGroupeAsync(UserConnecte.Centre);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UcAutreMateriel ctr = new UcAutreMateriel(fkiddemande);
            ctr.Closed += new EventHandler(galatee_Check);
            ctr.Show();
        }


        void galatee_Check(object sender, EventArgs e)
        {

            UcAutreMateriel ctrs = sender as UcAutreMateriel;
            if (ctrs.isOkClick)
            {
                int i = 1;
                lstAutreMateriel.Add((CsSortieAutreMateriel)ctrs.MyObject);

                dgDemande.ItemsSource = null;
                dgAutreMateriel.ItemsSource = lstAutreMateriel;
            }
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

        private void btnRecherche_Click(object sender, RoutedEventArgs e)
        {
            if (dtProgram.SelectedDate !=null)
            {

                List<CsProgarmmation> IdDemande = lstprogrammation.Where(p => p.ESTACTIF == true && p.ISPRESTATION == false &&  p.DATEPROGRAMME == dtProgram.SelectedDate).ToList();
                if (cboEquipe.SelectedItem != null)
                {
                    Guid IdEquipe = ((CsGroupe)cboEquipe.SelectedItem).ID;
                    IdDemande = IdDemande.Where(p => p.FK_IDEQUIPE == IdEquipe).ToList(); 
                }
                if (IdDemande != null)
                    RechercheDemande(IdDemande.Where(u => u.ISMATERIELEXTLIVRE == false || u.ISMATERIELEXTLIVRE == null).Select(r => r.FK_IDDEMANDE.Value).ToList());
                else
                    Message.Show("Aucune demande trouvé", "Information");
            }
            else
                Message.Show("Veuillez sélectionner une date", "Information");
        }


        public List<CsProgarmmation> lstprogrammation = new List<CsProgarmmation>();

        private void ChargeListeUser()
        {
            try
            {

                //Lancer la transaction de mise a jour en base
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
    }
}

