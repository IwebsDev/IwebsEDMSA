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
    public partial class UcValidationSortieMateriel : ChildWindow
    {
        List<CsUtilisateur> lstAllUser = new List<CsUtilisateur>();
        List<CsUtilisateur> lstCentreUser = new List<CsUtilisateur>();
        List<CsSortieMateriel> lstSortie = new List<CsSortieMateriel>();
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsDemandeBase> listeDemandeSelectionees = new List<CsDemandeBase>();
        List<CsCanalisation> LstDemandeValide = new List<CsCanalisation>();
        string numdemade = null;
        CsDemande LaDemande = new CsDemande();
        int EtapeActuelle;

        int iddemande = 0;
        int i = 0;
        public UcValidationSortieMateriel(int lstdem)
        {
            InitializeComponent();

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();

            ChargeListeUser();
            RechercheDemande(lstdem);
            ChargeEquipe(UserConnecte.Centre);
            ChargeProgrammation();
        }
        public UcValidationSortieMateriel(List<int> demande, int etape)
        {
            InitializeComponent();
            this.EtapeActuelle = etape;

            LstDemandeValide = new List<CsCanalisation>();
            lstAllUser = new List<CsUtilisateur>();
            lstSortie = new List<CsSortieMateriel>();
            listeDemandeSelectionees = new List<CsDemandeBase>();
            LstDemandeValide = new List<CsCanalisation>();
            ChargeListeUser();
            ChargeEquipe(UserConnecte.Centre);
            RechercheTypedemande(etape);

            //ChargeProgrammation();
            //RechercheDemande(demande);
        }
        private void RechercheTypedemande(int idetape)
        {
            try
            {

                //Lancer la transaction de mise a jour en base
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

        private void RechercheDemande(int lstdem)
        {
            try
            {
                List<int> lstint = new List<int>();
                lstint.Add(lstdem);
                List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeByIdCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {

                        iddemande = LstDemande.FirstOrDefault().PK_ID;
                        RetourneDetailDemande(LstDemande.FirstOrDefault());

                    }
                };
                service1.RetourneListeDemandeByIdAsync(lstint);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RetourneDetailDemande(CsDemandeBase laDemandeSelect)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetDemandeByNumIdDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    LaDemande = args.Result;
                    LoadingManager.EndLoading(res);


                    ListeSortieMateriel(LaDemande);
                    ListeSortieAutreMateriel(LaDemande);
                    dgDemande.ItemsSource = LaDemande.LstCanalistion;


                };
                service.GetDemandeByNumIdDemandeAsync(laDemandeSelect.PK_ID);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LstDemandeValide = new List<CsCanalisation>();
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_transmetre.IsEnabled = false;
            this.CancelButton.IsEnabled = false;
            EnregistreDemande(true);
        }
        private void btn_Enregistre_Click_1(object sender, RoutedEventArgs e)
        {
            EnregistreDemande(false);
        }
        private void EnregistreDemande(bool IsTransmettre)
        {
            if (this.txtAgt_Livreur.Tag != null && this.txtAgt_Livreur.Tag != null)
            {


                List<CsDemande> ListDmd = new List<CsDemande>();
                List<CsDemande> ListDmdRejet = new List<CsDemande>();
                List<int> Listid = new List<int>();
                List<int> ListidRejet = new List<int>();
                foreach (var item in ListDemande)
                {
                    var can = LstDemandeValide.FirstOrDefault(d => d.FK_IDDEMANDE == item.LaDemande.PK_ID);
                    if (can != null)
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
                EditionSortieMateriel(ListDmd);
                ValiderChoix(LstDemandeValide);
            }
        }
        private void EditionSortieMateriel(List<CsDemande> lesDemandes)
        {
            List<CsEditionDevis> LstDesRubriqueDevis = new List<CsEditionDevis>();
            foreach (var item in lesDemandes)
            {
                foreach (ObjELEMENTDEVIS leObjeDevis in item.EltDevis.Where(t=>t.FK_IDMATERIELDEVIS != null && t.FK_IDMATERIELDEVIS != 0).ToList())
                {
                    CsEditionDevis LaRubriqueDevis = new CsEditionDevis();
                    LaRubriqueDevis.CENTRE = item.LaDemande.CENTRE;
                    LaRubriqueDevis.PRODUIT = item.LaDemande.LIBELLEPRODUIT;
                    LaRubriqueDevis.TYPEDEMANDE = item.LaDemande.LIBELLETYPEDEMANDE;
                    LaRubriqueDevis.COMMUNUE = item.Ag.LIBELLECOMMUNE;
                    LaRubriqueDevis.QUARTIER = item.Ag.LIBELLEQUARTIER;
                    LaRubriqueDevis.NOM = item.LeClient.NOMABON;
                    LaRubriqueDevis.NUMDEMANDE = item.LaDemande.NUMDEM;
                    LaRubriqueDevis.LATITUDE = item.Branchement.LATITUDE;
                    LaRubriqueDevis.LONGITUDE = item.Branchement.LONGITUDE;
                    LaRubriqueDevis.DESIGNATION = leObjeDevis.DESIGNATION;
                    LaRubriqueDevis.QUANTITE = Convert.ToDecimal(leObjeDevis.QUANTITE);
                    LstDesRubriqueDevis.Add(LaRubriqueDevis);
                }
            }
            Utility.ActionDirectOrientation<ServicePrintings.CsEditionDevis, CsEditionDevis>(LstDesRubriqueDevis, null, SessionObject.CheminImpression, "ValidationMateriel", "Devis", true);
        }

        private void EnvoyerDemandeEtapeSuivante(List<int> Listid, ServiceWorkflow.WorkflowClient clientWkf)
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



        private void ValiderChoix(List<CsCanalisation> lstdemande)
        {
            //ServiceWorkflow.WorkflowClient clientWkf = new ServiceWorkflow.WorkflowClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Workflow"));

            //AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            //service1.InsertValidationSortieMaterielCompleted += (sr, res2) =>
            //{
            //    if (res2 != null && res2.Cancelled)
            //        return;
            //    if (res2.Result == true)
            //    {
            //        Message.ShowInformation("Validation Sortie matériel effectuée", Langue.lbl_Menu);
            //        EnvoyerDemandeEtapeSuivante(LstDemandeValide.Select(t=>t.FK_IDDEMANDE.Value).ToList(), clientWkf);
            //    }
            //    else
            //    {
            //        Message.ShowError("Validation Sortie matériel échouée", Langue.lbl_Menu);
            //    }
            //};
            //service1.InsertValidationSortieMaterielAsync(LstDemandeValide,(int)this.txtAgt_Recepteur.Tag);
            //service1.CloseAsync();



        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {

        }
        CsCanalisation demandecheck;
        CsCanalisation leDetailDemande = new CsCanalisation();
        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            demandecheck = ((CheckBox)sender).Tag as CsCanalisation;

            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != null && _LaDemandeSelect.PK_ID != 0)
            {
                LstDemandeValide.Add(demandecheck);
            }

            MiseAjourElementDevis();

        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            demandecheck = ((CheckBox)sender).Tag as CsCanalisation;

            CsCanalisation _LaDemandeSelect = new CsCanalisation();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsCanalisation)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect.PK_ID != 0)
                LstDemandeValide = LstDemandeValide.Where(c => c.PK_ID != demandecheck.PK_ID).ToList();
            MiseAjourElementDevis();

        }

        private void MiseAjourElementDevis()
        {
            var ListeDemande = ListDemande.Where(d => LstDemandeValide.Select(c => c.FK_IDDEMANDE).Contains(d.LaDemande.PK_ID));
            List<ObjELEMENTDEVIS> ElementPrisEnConte = new List<ObjELEMENTDEVIS>();
            foreach (var item in ListeDemande)
                ElementPrisEnConte.AddRange(item.EltDevis.Where(t => t.FK_IDMATERIELDEVIS  != 0 && t.FK_IDMATERIELDEVIS  != null).ToList());

            dgAutreMateriel.ItemsSource = RetournListeElementAggreger(ElementPrisEnConte);
        }
        private List<ObjELEMENTDEVIS> RetournListeElementAggreger(List<ObjELEMENTDEVIS> ElementDeDevis)
        {
            List<ObjELEMENTDEVIS> ElementDeDevisAggrege = (from e in ElementDeDevis
                                                           group new
                                                           {
                                                               e.NUMDEVIS,
                                                               e.NUMDEM,
                                                               e.ORDRE,
                                                               e.NUMFOURNITURE,
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
                                                               e.FK_IDFOURNITURE,
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
                                                                   NUMFOURNITURE = groupe.Key.NUMFOURNITURE,
                                                                   DESIGNATION = groupe.Key.DESIGNATION,
                                                                   QUANTITE = groupe.Sum(g => g.QUANTITE),
                                                                   QUANTITEALIVRET = groupe.Sum(g => g.QUANTITE),
                                                                   QUANTITELIVRET = groupe.Sum(g => g.QUANTITE),
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
                if (res2.Result.Count > 0)
                {
                    lstSortie = res2.Result;
                    demande.LstCanalistion.ForEach(c => c.ISLIVRE = false);
                    demande.LstCanalistion.ForEach(c => c.ISRECU = false);
                    foreach (CsSortieMateriel sortie in lstSortie)
                    {
                        foreach (CsCanalisation canal in demande.LstCanalistion)
                        {
                            if (sortie.FK_IDDEMANDE  == canal.FK_IDDEMANDE )
                                canal.ISLIVRE = true;

                            if (sortie.FK_IDDEMANDE  == canal.FK_IDDEMANDE  && sortie.FK_IDRECEPTEUR != 0)
                                LstDemandeValide.Add(canal);
                        }
                    }
                    LstDemandeValide = new List<CsCanalisation>();
                }
                else
                {
                    Message.ShowError("Sortie matériel échouée", Langue.lbl_Menu);
                    LstDemandeValide = new List<CsCanalisation>();
                }

            };
            service1.RetourneListeSortieMaterielLivreAsync(demande.LaDemande.PK_ID);
            service1.CloseAsync();

        }
        private void ListeSortieAutreMateriel(CsDemande demande)
        {
            try
            {
                dgAutreMateriel.ItemsSource = demande.EltDevis;
            }
            catch
            {

            }
        }

        private void dgDemande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_Rechercher_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Tag  != null)
            {
                RetourneListeDemandeAvalider();
            }
            else
            {
                Message.Show("Veuillez choisir l'agent de récepteure", "Information");
            }
        }

        private void RetourneListeDemandeAvalider()
        {
            AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service1.RetourneListeDemandeAvaliderCompleted += (sr, res) =>
            {
                if (res != null && res.Cancelled)
                    return;
                LstDemande = new List<CsDemandeBase>();
                foreach (var item in ListDemande)
	            {
                    if ( res.Result.Select(d=>d.PK_ID).Contains(item.LaDemande.PK_ID))
                    {
                        LstDemande.Add(item.LaDemande);
                    } 
	            }

                if (LstDemande != null && LstDemande.Count != 0 && btnRecherche.IsEnabled == true)
                {
                    RetourneDetailDemande(LstDemande);
                }
            };
            service1.RetourneListeDemandeAvaliderAsync((int)this.txtAgt_Recepteur.Tag);
            service1.CloseAsync();
        }


        private void RetourneDetailDemande(List<CsDemandeBase> laDemandeSelect)
        {
            int res = LoadingManager.BeginLoading(Langue.En_Cours);

            try
            {

                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.GetDemandeByListeNumIdDemandeCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    ListDemande = args.Result;

                    List<CsCanalisation> ListeDeCanalisation = new List<CsCanalisation>();
                    foreach (var item in ListDemande)
                    {
                        if (item.LstCanalistion != null && item.LstCanalistion.Count != 0)
                        {
                            item.EltDevis.ForEach(e => e.QUANTITELIVRET = e.QUANTITEALIVRET);
                            ListeDeCanalisation.Add(item.LstCanalistion.First());
                            ListeSortieMateriel(item);
                        }
                    }

                    ListeSortieAutreMateriel(LaDemande);

                    LoadingManager.EndLoading(res);
                    //if (btn_Rechercher.IsEnabled == true)
                    //{
                        dgDemande.ItemsSource = ListeDeCanalisation;
                    //}
                        btnRecherche.IsEnabled = true;


                };
                service.GetDemandeByListeNumIdDemandeAsync(laDemandeSelect.Select(d => d.PK_ID).ToList());
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                LstDemandeValide = new List<CsCanalisation>();
                LoadingManager.EndLoading(res);
                throw ex;
            }
        }


        public List<CsDemande> ListDemande = new List<CsDemande>();

        private void btnRecherche_Click(object sender, RoutedEventArgs e)
        {
            if (dtProgram.SelectedDate != null)
            {

                //List<CsProgarmmation> IdDemande = lstprogrammation.Where(p => p.ISCOMPTEURLIVRE == true && p.DATEPROGRAMME == dtProgram.SelectedDate).ToList();
                List<CsProgarmmation> IdDemande = lstprogrammation.Where(p =>p.DATEPROGRAMME == dtProgram.SelectedDate).ToList();
                if (cboEquipe.SelectedItem != null)
                {
                    Guid IdEquipe = ((CsGroupe)cboEquipe.SelectedItem).ID;
                    IdDemande = IdDemande.Where(p => p.FK_IDEQUIPE == IdEquipe).ToList();
                }
                if (IdDemande != null)
                {
                    RechercheDemande(IdDemande.Select(r => r.FK_IDDEMANDE.Value).ToList());
                }
                else
                {
                    Message.Show("Aucune demande trouvé", "Information");
                }
            }
            else
            {
                Message.Show("Veuillez sélectionner une date", "Information");
            }
        }

        public List<CsProgarmmation> lstprogrammation = new List<CsProgarmmation>();
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
        private void ChargeProgrammation()
        {
            try
            {
                List<CsGroupe> lstequipe = new List<CsGroupe>();
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneProgrammationCompleted += (sr, res) =>
                {
                    if (res != null && res.Cancelled)
                        return;
                    lstprogrammation = res.Result;
                };
                service1.RetourneProgrammationAsync();
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }

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
        private void txtAgt_Recepteur_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.txtAgt_Recepteur.Text.Length == SessionObject.Enumere.TailleMatricule)
            {
                if (lstAllUser != null && lstAllUser.Count() > 0)
                {
                    ServiceAccueil.CsUtilisateur leuser = lstAllUser.FirstOrDefault(t => t.MATRICULE == this.txtAgt_Livreur.Text);
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

