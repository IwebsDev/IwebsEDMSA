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
using Galatee.Silverlight.ServiceAccueil;
using Galatee.Silverlight.Resources.Accueil;
using Galatee.Silverlight.Shared;
using Galatee.Silverlight.MainView;
using Galatee.Silverlight.Workflow;

namespace Galatee.Silverlight.Devis
{
    public partial class UcLiasonCompteur : ChildWindow
    {
        List<CsCentre> LstCentre = new List<CsCentre>();
        List<CsProduit> LstDeProduit = new List<CsProduit>();
        List<CsSite> lstSite = new List<CsSite>();
        List<CsDemandeBase> LstDemande = new List<CsDemandeBase>();
        List<CsCompteur> LstCompteur = new List<CsCompteur>();
        List<CsDemande> listeDemandeSelectionees = new List<CsDemande>();
        CsReglageCompteur leReglageCompteur = null;
        CsCalibreCompteur leCalibreCompteur = null;
        List<CsDemandeBase> LstDemandeValide = new List<CsDemandeBase>();
        int EtapeActuelle;

        int i = 0;
        public UcLiasonCompteur()
        {
            InitializeComponent();
        }


        public UcLiasonCompteur(List<int> demandes)
        {
            InitializeComponent();
            ChargerDonneeDuSite();
            RechercheDemande(demandes);
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
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(y=>y.CODE).ToList());
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> LstCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<Galatee.Silverlight.ServiceAccueil.CsSite> lstSite = ClasseMEthodeGenerique.RetourneSiteByCentre(LstCentre);
                    foreach (Galatee.Silverlight.ServiceAccueil.CsCentre item in LstCentre)
                        lesCentrePerimetre.Add(item.PK_ID);
                    loadCompteur(lstSite.Select(y => y.CODE).ToList());

                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UcLiasonCompteur(List<int> demandes, int fkIdEtape)
        {
            InitializeComponent();
      
            ChargerDonneeDuSite();
            RechercheDemande(demandes);
            ChargerReglageCompteur();
            EtapeActuelle = fkIdEtape;
        }
        private void ChargerCalibreCompteur()
        {
            try
            {
                if (SessionObject.LstCalibreCompteur .Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerCalibreCompteurCompleted  += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCalibreCompteur = args.Result;
                };
                service.ChargerCalibreCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void ChargerReglageCompteur()
        {
            try
            {
                if (SessionObject.LstReglageCompteur.Count != 0)
                    return;
                AcceuilServiceClient service = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ChargerReglageCompteurCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstReglageCompteur = args.Result;
                };
                service.ChargerReglageCompteurAsync();
                service.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.btn_Transmetre.IsEnabled = false;
            List<CsDemandeBase> lesDemandeSelect = ((List<CsDemandeBase>)dgDemande.ItemsSource).Where(t => !string.IsNullOrEmpty(t.COMPTEUR )).ToList();
            ValiderChoix(lesDemandeSelect);
        }

        private void ValiderDemande()
        {
        }

        private void ValiderChoix(List<CsDemandeBase> lesDemandeSelect)
        {
            try
            {
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.InsertLiaisonCompteurCompleted += (sr, res) =>
                {
                    this.btn_Transmetre.IsEnabled = true;
                    if (res != null && res.Cancelled)
                        return;
                    List<CsDemandeBase> resultat = res.Result;
                    if (resultat != null && resultat.Count != 0)
                    {
                        Utility.ActionDirectOrientation<ServicePrintings.CsDemandeBase, CsDemandeBase>(resultat, null, SessionObject.CheminImpression, "LiaisonCompteur", "Accueil", true);
                        Message.ShowInformation("Liaisons de compteurs effectuées", Langue.lbl_Menu);
                        this.DialogResult = true;
                    }
                    else
                        Message.ShowError("Aucun compteur trouvé", Langue.lbl_Menu);
                };
                service1.InsertLiaisonCompteurAsync(lesDemandeSelect);
                service1.CloseAsync();
            }
            catch
            {

            }

        }
       
      
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            LstDemande = LstDemande.Where(c => c.REFEM == txtRefClient.Text && c.NUMDEM == txtNumDemande.Text && c.NOMCLIENT == txtNomClient.Text).ToList();
        
        }

        private void RechercheDemande(List<int> demandes)
        {
            try
            {

                //Lancer la transaction de Mise à jour en base
                AcceuilServiceClient service1 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service1.RetourneListeDemandeByIdCompleted += (sr, res) =>
                {

                    if (res != null && res.Cancelled)
                        return;
                    LstDemande = res.Result;
                    if (LstDemande != null && LstDemande.Count != 0)
                    {
                        dgDemande.ItemsSource = LstDemande;

                    }
                    else
                        Message.ShowError("Aucune données trouvées", Langue.lbl_Menu);


                };
                service1.RetourneListeDemandeByIdAsync(demandes);
                service1.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        private void dgDemande_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        CsDemandeBase demandecheck;
        CsDemande leDetailDemande = new CsDemande();
        List<CsCanalisation> lesCanalisationACree=new List<CsCanalisation>();
        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            demandecheck = ((CheckBox)sender).Tag as CsDemandeBase;
            CsDemandeBase _LaDemandeSelect = new CsDemandeBase();
            if (this.dgDemande.SelectedItem != null)
                _LaDemandeSelect = (CsDemandeBase)this.dgDemande.SelectedItem;
            if (_LaDemandeSelect != null)
            {
                //LstDemandeValide.Add(_LaDemandeSelect);
                //if (lesCanalisationACree.Count > 0)
                //{
                //    List<CsCompteur> lstcompt = new List<CsCompteur>();
                //    List<CsCompteur> lstcompt2 = new List<CsCompteur>();
                //    foreach (CsCanalisation can in lesCanalisationACree)
                //            lstcompt.Add(LstCompteur.Where(x => x.PK_ID == int.Parse(can.FK_IDMAGAZINVIRTUEL.ToString())).FirstOrDefault());

                //    foreach (CsCompteur cpt in LstCompteur)
                //    {
                //        if (lstcompt != null && lstcompt.Count != 0 )
                //        {
                //            if (lstcompt.Where(x => x.PK_ID == cpt.PK_ID).FirstOrDefault() == null)
                //                lstcompt2.Add(cpt);
                //        }
                //    }
                //    LstCompteur = lstcompt2;
                //}
                //List<CsCompteur> lesCompteurDispo = new List<CsCompteur>();
                //lesCompteurDispo = LstCompteur;
                if (this.dgDemande.ItemsSource  != null )
                {
                    List<CsDemandeBase> lstDemandelie =((List<CsDemandeBase>)this.dgDemande.ItemsSource).Where(t=>!string.IsNullOrEmpty(t.COMPTEUR )).ToList(); 
                    if (lstDemandelie != null && lstDemandelie.Count  != 0)
                    {
                        List<int> lstIdCompteur = lstDemandelie.Select(o => o.FK_IDMAGAZINVIRTUEL).ToList();
                        LstCompteur = LstCompteur.Where(j => !lstIdCompteur.Contains(j.PK_ID)).ToList();
                    }
                }
                if (_LaDemandeSelect.PRODUIT != SessionObject.Enumere.ElectriciteMT)
                {
                    leReglageCompteur = SessionObject.LstReglageCompteur.FirstOrDefault(t => t.CODE == _LaDemandeSelect.REGLAGECOMPTEUR);
                    if (leReglageCompteur != null && leReglageCompteur.PK_ID != 0)
                    {
                        string  CodePhase = "2";
                        if (_LaDemandeSelect.REGLAGECOMPTEUR != null && _LaDemandeSelect.REGLAGECOMPTEUR.Substring(0, 1) == "4")
                            CodePhase = "4";

                        List<CsCalibreCompteur> LeCalibreEquivalant = SessionObject.LstCalibreCompteur.Where(t =>
                                                                                                                  t.CODEPHASE  == CodePhase &&
                                                                                                                  t.REGLAGEMAXI >= leReglageCompteur.REGLAGEMAXI &&
                                                                                                                  t.FK_IDPRODUIT == _LaDemandeSelect.FK_IDPRODUIT).ToList();

                        List<int> lesIdCalibre = LeCalibreEquivalant.Select(u => u.PK_ID).ToList();
                        List<CsCompteur> lstcompteurCritere = LstCompteur.Where(t =>t.FK_IDCALIBRECOMPTEUR != null && t.CODESITE == _LaDemandeSelect.SITE && t.CODEPRODUIT == _LaDemandeSelect.PRODUIT && lesIdCalibre.Contains(t.FK_IDCALIBRECOMPTEUR.Value)).ToList();
                        UcDetailCompteur ctr = new UcDetailCompteur(_LaDemandeSelect, lstcompteurCritere);
                        ctr.Closed += new EventHandler(galatee_Check);
                        ctr.Show();
                    }
                }
            }
            
        }
        void galatee_Check(object sender, EventArgs e)
        {

            UcDetailCompteur ctrs = sender as UcDetailCompteur;
            if (ctrs.isOkClick)
            {
                int i=1;
                List<CsCompteur> _LesCompteurs = (List<CsCompteur>)ctrs.MyObject;
              
                foreach (CsCompteur _LeCompteur in _LesCompteurs)
                {
                    CsCanalisation canal = new CsCanalisation()
                    {
                        CENTRE = demandecheck.CENTRE,
                        CLIENT = demandecheck.CLIENT,
                        NUMDEM = demandecheck.NUMDEM,
                        PRODUIT = demandecheck.PRODUIT,
                        PROPRIO = "1",
                        MARQUE=_LeCompteur.MARQUE,
                        TYPECOMPTEUR=_LeCompteur.TYPECOMPTEUR,
                        NUMERO=_LeCompteur.NUMERO,
                        INFOCOMPTEUR = _LeCompteur.NUMERO,
                        FK_IDCENTRE = demandecheck.FK_IDCENTRE,
                        FK_IDPRODUIT = int.Parse(demandecheck.FK_IDPRODUIT.ToString()),
                        FK_IDMAGAZINVIRTUEL = _LeCompteur.PK_ID,
                        //FK_IDCOMPTEUR  = _LeCompteur.PK_ID,
                        FK_IDTYPECOMPTEUR = _LeCompteur.FK_IDTYPECOMPTEUR,
                        FK_IDMARQUECOMPTEUR = _LeCompteur.FK_IDMARQUECOMPTEUR,
                        FK_IDCALIBRE = _LeCompteur.FK_IDCALIBRECOMPTEUR ,
                        FK_IDDEMANDE = demandecheck.PK_ID,
                        LIBELLETYPECOMPTEUR = _LeCompteur.NUMERO,
                        POSE  = System.DateTime.Now,
                        USERCREATION = UserConnecte.matricule,
                        USERMODIFICATION = UserConnecte.matricule,
                        DATECREATION = System.DateTime.Now,
                        DATEMODIFICATION = System.DateTime.Now,
                        FK_IDPROPRIETAIRE = 1,
                    };
                    if (leReglageCompteur != null)
                        canal.FK_IDREGLAGECOMPTEUR = leReglageCompteur.PK_ID;

                    lesCanalisationACree.Add(canal);

                    if (i == _LesCompteurs.Count)
                    {
                        LstDemande.FirstOrDefault(c => c.NUMDEM == ((CsDemandeBase)this.dgDemande.SelectedItem).NUMDEM).COMPTEUR += _LeCompteur.NUMERO;
                        LstDemande.FirstOrDefault(c => c.FK_IDMAGAZINVIRTUEL  == ((CsDemandeBase)this.dgDemande.SelectedItem).FK_IDMAGAZINVIRTUEL ).FK_IDMAGAZINVIRTUEL  += _LeCompteur.PK_ID ;
                    }
                    else
                    {
                        LstDemande.FirstOrDefault(c => c.NUMDEM == ((CsDemandeBase)this.dgDemande.SelectedItem).NUMDEM).COMPTEUR += _LeCompteur.NUMERO + ", ";
                        LstDemande.FirstOrDefault(c => c.FK_IDMAGAZINVIRTUEL == ((CsDemandeBase)this.dgDemande.SelectedItem).FK_IDMAGAZINVIRTUEL).FK_IDMAGAZINVIRTUEL += _LeCompteur.PK_ID ;

                    }


                i++;
                }
                //dgDemande.ItemsSource = null;
                dgDemande.ItemsSource= LstDemande;
            }
        }
       
        private void loadCompteur(List<string> lstCodeSite)
        {
            AcceuilServiceClient service2 = new AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
            service2.RetourneListeCompteurMagasinCompleted += (sr2, res2) =>
            {

                if (res2 != null && res2.Cancelled)
                    return;
                LstCompteur = res2.Result;

            };
            service2.RetourneListeCompteurMagasinAsync(lstCodeSite);
            service2.CloseAsync();
        }

        private void Cbo_Compteur_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {

            if (dgDemande.SelectedItem != null)
            {
                CsDemandeBase laDemandeSelect = (CsDemandeBase)dgDemande.SelectedItem;
                laDemandeSelect.IsSELECT = false;

                LstDemande.FirstOrDefault(c => c.NUMDEM == laDemandeSelect.NUMDEM).COMPTEUR = string.Empty;

                //dgDemande.ItemsSource = null;
                dgDemande.ItemsSource = LstDemande;
                lesCanalisationACree = lesCanalisationACree.Where(c => c.NUMDEM != laDemandeSelect.NUMDEM).ToList();
            }
        }

      
    }

    
}

