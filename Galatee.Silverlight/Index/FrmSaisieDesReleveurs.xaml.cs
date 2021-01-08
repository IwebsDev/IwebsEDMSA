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
using Galatee.Silverlight.ServiceFacturation ;
using Galatee.Silverlight.MainView ;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmSaisieDesReleveurs : ChildWindow
    {
        public FrmSaisieDesReleveurs()
        {
            InitializeComponent();
            try
            {
               btn_Centre.IsEnabled = false;
               ChargerDonneeDuSite();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        List<CsCentre> LstSite;
        List<CParametre> LstUser;
        List<Galatee.Silverlight.ServiceAdministration.CsUtilisateur> ListeUtilisateurReleveur = new List<Galatee.Silverlight.ServiceAdministration.CsUtilisateur>();
        List<object> ListeReleveurObject = new List<object>();
        List<ServiceAdministration.CsUtilisateur> ListeUser = new List<ServiceAdministration.CsUtilisateur>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        int Action = 0;
        List<CsReleveur> ListeReleveur = new List<CsReleveur>();
        List<CsReleveur> ListeReleveurActif = new List<CsReleveur>();

        List<int> IdDesCentre = new List<int>();
        private void ChargerDonneeDuSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                     lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                     if (lstSite.Count == 1)
                     {
                         this.Txt_Site.Text = lstSite.First().CODE;
                         txt_LibelleSite.Text  = lstSite.First().LIBELLE;
                         this.Txt_Site.Tag = lstSite.First().PK_ID;
                         this.btn_Centre.IsEnabled = true;

                     }
                     if (lesCentre.Count == 1)
                     {
                         this.Txt_Centre.Text = lesCentre.First().CODE;
                         txt_libellecentre.Text  = lesCentre.First().LIBELLE;
                         this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                        
                     }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeUtilisateur(IdDesCentre);
                    ChargerListeReleveur(IdDesCentre);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_Site.Text = lstSite.First().CODE;
                        txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_Site.Tag = lstSite.First().PK_ID;
                        this.btn_Centre.IsEnabled = true;

                    }
                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE ;
                        txt_libellecentre.Text  = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                    }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeReleveur(IdDesCentre);
                    ChargerListeUtilisateur(IdDesCentre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<int> lstIdReleveur = new List<int>();
        private void ChargerListeReleveur(List<int> lstIdCentre)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
                service.RetourneReleveurCentre_Async(lstIdCentre,UserConnecte.PK_ID);
                service.RetourneReleveurCentre_Completed  += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null || args.Result.Count <= 0)
                        {
                            Message.ShowError("Aucun releveur retourné par le système.", "Information");
                            return;
                        }
                        ListeReleveur = args.Result.OrderBy(r=>r.MATRICULE).ThenBy(r=>r.DATECREATION).ToList();

                        ListeReleveurActif = ListeReleveur.Where(t => t.SUPPRIMER == null || t.SUPPRIMER == false) != null ? ListeReleveur.Where(t => t.SUPPRIMER == null || t.SUPPRIMER == false).ToList() : new List<CsReleveur>();
                        ListeUser = ListeUtilisateurReleveur.Where(l => !lstIdReleveur.Contains(l.PK_ID)).ToList();
                        if (lesCentre.Count == 1)
                        {
                            this.Txt_Centre.Text = lesCentre.First().CODE;
                            txt_libellecentre.Text = lesCentre.First().LIBELLE;
                            this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                            if (ListeReleveur != null && ListeReleveur.Count != 0)
                            {
                                this.dataGrid1.ItemsSource = null;
                                this.dataGrid1.ItemsSource = ListeReleveur.Where(t => t.FK_IDCENTRE == lesCentre.First().PK_ID && (t.SUPPRIMER == null || t.SUPPRIMER == false));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }
                };

                this.btn_Creation.Visibility = System.Windows.Visibility.Visible;
                this.btn_modification.Visibility = System.Windows.Visibility.Visible;
                this.btn_suppression.Visibility = System.Windows.Visibility.Visible;

                this.OKButton.Visibility = System.Windows.Visibility.Collapsed;
                this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
                this.dataGrid1.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void ChargerListeUtilisateur(List<int> lstCentre)
        {
            try
            {
                if (SessionObject.ListeDesUtilisateurs != null && SessionObject.ListeDesUtilisateurs.Count != 0)
                {
                    List<ServiceAdministration.CsUtilisateur> lstUtilisateur = SessionObject.ListeDesUtilisateurs.Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
                    //ListeUtilisateurReleveur.AddRange(lstUtilisateur.Where(t => t.FONCTION == SessionObject.CodeFonctionReleveur));
                    ListeUtilisateurReleveur.AddRange(lstUtilisateur);
                    return;
                }
                Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient client = new Galatee.Silverlight.ServiceAdministration.AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }
                    SessionObject.ListeDesUtilisateurs = res.Result;
                    List<ServiceAdministration.CsUtilisateur> lstUtilisateur = SessionObject.ListeDesUtilisateurs.Where(t => lstCentre.Contains(t.FK_IDCENTRE)).ToList();
                    //ListeUtilisateurReleveur.AddRange(lstUtilisateur.Where(t => t.FONCTION == SessionObject.CodeFonctionReleveur));
                    ListeUtilisateurReleveur.AddRange(lstUtilisateur);
                };
                client.RetourneListeAllUserAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult resultat = new DialogResult("", false);//, "Warming", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                resultat.Closed += new EventHandler(DialogClosed);
                resultat.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;

            try
            {
                this.btn_Creation.Visibility = System.Windows.Visibility.Visible;
                this.btn_modification.Visibility = System.Windows.Visibility.Visible;
                this.btn_suppression.Visibility = System.Windows.Visibility.Visible;

                this.OKButton.Visibility = System.Windows.Visibility.Collapsed;
                this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;

                this.dataGrid1.IsEnabled = true;
                if (ListeReleveur.Count != 0)
                    this.dataGrid1.SelectedItem = ListeReleveur[0];
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }

        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lesCentre.Where(t=>t.FK_IDCODESITE  ==(int)this.Txt_Site .Tag).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += new EventHandler(galatee_OkClickedCentre);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedCentre(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsCentre param = (ServiceAccueil.CsCentre)ctrs.MyObject;//.VALEUR;
                    this.Txt_Centre.Text = param.CODE;
                    txt_libellecentre.Text = param.LIBELLE;
                    this.Txt_Centre.Tag = param.PK_ID;

                    if (ListeReleveur != null && ListeReleveur.Count != 0)
                    {
                        this.dataGrid1.ItemsSource = null;
                        this.dataGrid1.ItemsSource = ListeReleveur.Where(t => t.FK_IDCENTRE == param.PK_ID && (t.SUPPRIMER==null || t.SUPPRIMER==false) );
                    }
                }

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void btn_Site_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lstSite);
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Site");
                ctrl.Closed += new EventHandler(galatee_OkClickedSite);
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void galatee_OkClickedSite(object sender, EventArgs e)
        {
            try
            {
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                    this.Txt_Site.Text = param.CODE;
                    txt_LibelleSite.Text = param.LIBELLE;
                    this.Txt_Site.Tag = param.PK_ID;
                    this.btn_Centre.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.OKButton.Visibility = System.Windows.Visibility.Collapsed;
                this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (this.dataGrid1.SelectedItem != null )
                {
                    this.Txt_Matricule.Text = string.IsNullOrEmpty(((CsReleveur)this.dataGrid1.SelectedItem).MATRICULE) ? string.Empty : ((CsReleveur)this.dataGrid1.SelectedItem).MATRICULE;
                    this.Txt_Releveur.Text = string.IsNullOrEmpty(((CsReleveur)this.dataGrid1.SelectedItem).CODE) ? string.Empty : ((CsReleveur)this.dataGrid1.SelectedItem).CODE;
                    this.txt_NomReleveur.Text = string.IsNullOrEmpty(((CsReleveur)this.dataGrid1.SelectedItem).NOMRELEVEUR) ? string.Empty : ((CsReleveur)this.dataGrid1.SelectedItem).NOMRELEVEUR;
                    this.Txt_Quota.Text = string.IsNullOrEmpty(((CsReleveur)this.dataGrid1.SelectedItem).FERMEQUOT.ToString()) ? string.Empty : ((CsReleveur)this.dataGrid1.SelectedItem).FERMEQUOT.ToString();
                    this.Txt_NumTerminalSaisie.Text = string.IsNullOrEmpty(((CsReleveur)this.dataGrid1.SelectedItem).PORTABLE) ? string.Empty : ((CsReleveur)this.dataGrid1.SelectedItem).PORTABLE;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

       public  void SelectAction(int _Action)
        {
            try
            {
                if (_Action == 1)
                {
                    this.dataGrid1.IsEnabled = true;
                    this.Txt_Matricule.Text = string.Empty;
                    this.Txt_Matricule.IsReadOnly = false;

                    this.Txt_Releveur.Text = string.Empty;
                    this.Txt_Releveur.IsReadOnly = false;

                    this.Txt_NumTerminalSaisie.Text = string.Empty;
                    this.Txt_NumTerminalSaisie.IsReadOnly = false;

                    this.Txt_Quota.Text = string.Empty;
                    this.Txt_Quota.IsReadOnly = false;


                    this.dataGrid1.IsEnabled = false;


                }
                else if (_Action == 2)
                {
                    this.Txt_Matricule.IsReadOnly = false;
                    this.Txt_Releveur.IsReadOnly = false;
                    this.Txt_NumTerminalSaisie.IsReadOnly = false;
                    this.Txt_Quota.IsReadOnly = false;

                }

                this.btn_Creation.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_modification.Visibility = System.Windows.Visibility.Collapsed;
                this.btn_suppression.Visibility = System.Windows.Visibility.Collapsed;

                this.OKButton.Visibility = System.Windows.Visibility.Visible;
                this.CancelButton.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                throw ex;
            }
       }

       void DialogClosed(object sender, EventArgs e)
       {
           try
           {
               DialogResult ctrs = sender as DialogResult;
               if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
               {
                   //MisaJourTourne(Action, LeReleveurSelect);
                   ctrs.DialogResult = false;
                   return;
               }
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void btn_Creation_Click(object sender, RoutedEventArgs e)
       {
           try
           {

               UcGererReleveurs u = new UcGererReleveurs(lstSite, lesCentre, ListeUtilisateurReleveur, ListeReleveur, false);
               u.Closed += new EventHandler(galatee_OkclikcCreation);
               u.Show();
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void btn_modification_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               if (dataGrid1.SelectedItem == null)
                   return;
               CsReleveur releveur = (CsReleveur)dataGrid1.SelectedItem;
               UcGererReleveurs u = new UcGererReleveurs(lstSite,lesCentre, ListeReleveur, releveur, true);
               u.Closed += new EventHandler(galatee_OkclikcModification);
               u.Show();
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void btn_suppression_Click(object sender, RoutedEventArgs e)
       {

           try
           {
              var w = new MessageBoxControl.MessageBoxChildWindow("Suppression Releveur", "Voulez vous executer cette action", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
               w.OnMessageBoxClosed += (_, result) =>
               {
                   if (w.Result == MessageBoxResult.OK)
                   {
                       if (dataGrid1.SelectedItem == null)
                           return;
                       CsReleveur releveur = (CsReleveur)dataGrid1.SelectedItem;

                       releveur.USERMODIFICATION = UserConnecte.matricule;
                       releveur.DATEMODIFICATION = DateTime.Now.Date;
                       releveur.SUPPRIMER = true;
                       SupprimerReleveur(releveur);
                   }
                   else
                   {
                       DialogResult = false;
                   }
               };
               w.Show ();
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void MisaJourTourne(int Action,CsReleveur  LeReleveur)
       {
           try
           {
               if (Action == 2)
                   UpdateReleveur(MajTourneeSelect(LeReleveur));
               if (Action == 1)
                   InsererReleveur(MajTourneeSelect(LeReleveur));
               if (Action == 3)
                   SupprimerReleveur(LeReleveur);
           }
           catch (Exception ex)
           {
               
               throw ex;
           }
       }

       CsReleveur  MajTourneeSelect(CsReleveur  LeReleveurSelect)
       {
           try
           {
               LeReleveurSelect.CENTRE = this.Txt_Centre.Text;
               LeReleveurSelect.MATRICULE = this.Txt_Matricule.Text;
               LeReleveurSelect.CODE = this.Txt_Releveur.Text;
               LeReleveurSelect.FERMEQUOT = int.Parse(this.Txt_Quota.Text);
               LeReleveurSelect.PORTABLE = this.Txt_NumTerminalSaisie.Text;

               return LeReleveurSelect;
           }
           catch (Exception ex)
           {
               
               throw;
           }
       }

       private void InsererReleveur(CsReleveur LeReleveur)
       {
           try
           {
               FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
               service.InsererUnReleveurCompleted += (s, args) =>
               {
                   try
                   {
                       if (args != null && args.Cancelled)
                       {
                           Message.ShowError("", "");
                           return;
                       }

                       if (args.Result == false)
                       {
                           Message.ShowError("Erreur d'insertion du releveur.Réessayer svp!", "Information");
                           return;
                       }

                      
                   }
                   catch (Exception ex)
                   {
                       Message.ShowError(ex, "Erreur");
                   }

               };
               service.InsererUnReleveurAsync(LeReleveur);
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void UpdateReleveur(CsReleveur LeReleveur)
       {
           try
           {
               FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
               service.UpdateUnReleveurCompleted += (s, args) =>
               {
                   try
                   {
                       if (args != null && args.Cancelled)
                       {
                           Message.ShowError("Erreur survenue lors de la mise à jour du releveur.Réessayer svp!", "Erreur");
                           return;
                       }

                       if (args.Result == false)
                       {
                           Message.ShowError("Erreur d'insertion du releveur.Réessayer svp!", "Information");
                           return;
                       }

                   }
                   catch (Exception ex)
                   {
                       Message.ShowError(ex, "Erreur");
                   }

               };
               service.UpdateUnReleveurAsync(LeReleveur);
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void SupprimerReleveur(CsReleveur  LeReleveur)
       {
           try
           {
               FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));
               service.SupprimerUnReleveurCompleted += (s, args) =>
               {
                   try
                   {
                       if (args != null && args.Cancelled)
                       {
                           Message.ShowError("Erreur survenue lors de la suppression du releveur.Réessayer svp!", "Erreur");
                           return;
                       }

                       if (args.Result == false)
                       {
                           Message.ShowError("Erreur survenue lors de la suppression du releveur.Réessayer svp!", "Information");
                           return;
                       }

                       ListeReleveur.RemoveAll(c =>c.FK_IDCENTRE == LeReleveur.FK_IDCENTRE &&  c.CENTRE == LeReleveur.CENTRE && c.CODE == LeReleveur.CODE && c.MATRICULE == LeReleveur.MATRICULE);
                       Txt_Centre_TextChanged(null, null);

                       //ChargerListeReleveur(this.Txt_Centre.Text, null);
                   }
                   catch (Exception ex)
                   {
                       Message.ShowError(ex, "Erreur");
                   }

               };
               service.SupprimerUnReleveurAsync(LeReleveur);
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
       {
           try
           {
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       private void btn_UserId_Click(object sender, RoutedEventArgs e)
       {
           try
           {

               List<string> _LstColonneAffich = new List<string>();
               _LstColonneAffich.Add("MATRICULE");
               _LstColonneAffich.Add("LIBELLE");
               List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeUtilisateurReleveur.Where(t => t.CENTRE == this.Txt_Centre.Text).ToList());
               MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "LISTE DES UTILISATEURS");
               ctrl.Closed += new EventHandler(galatee_OkClickedUser);
               ctrl.Show();
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       void galatee_OkClickedUser(object sender, EventArgs e)
       {
           try
           {
               MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
               CsUtilisateur c = (CsUtilisateur)ctrs.MyObject;
               this.Txt_Matricule.Text = c.MATRICULE;
               
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       void galatee_OkclikcCreation(object sender, EventArgs e)
       {
           try
           {
               UcGererReleveurs  rel= sender as UcGererReleveurs;
               if (rel.IsOkClick)
               {
                   if (!ListeReleveurActif.Select(r=>r.MATRICULE).Contains( rel._releveurCreation.MATRICULE))
                   {
                       ListeReleveur.Add(rel._releveurCreation);
                       Txt_Centre.Text = rel._releveurCreation.CENTRE;
                       Txt_Centre_TextChanged(null, null);
                       if (ListeReleveur != null && ListeReleveur.Count != 0)
                       {
                           this.dataGrid1.ItemsSource = null;
                           this.dataGrid1.ItemsSource = ListeReleveur.Where(t => t.FK_IDCENTRE == rel._releveurCreation.FK_IDCENTRE && (t.SUPPRIMER == null || t.SUPPRIMER == false));
                       }
                   }
                   else
                   {
                       Message.ShowWarning("Ce réléveur est déja actif sur ce cente", "Alert");
                   }
               }
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       void galatee_OkclikcModification(object sender, EventArgs e)
       {
           try
           {
               UcGererReleveurs rel = sender as UcGererReleveurs;
               Txt_Centre.Text = ListeReleveur.First().CENTRE;
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

    }
}

