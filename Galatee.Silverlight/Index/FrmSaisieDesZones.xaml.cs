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
using Galatee.Silverlight.ServiceFacturation;
using Galatee.Silverlight.MainView ;

namespace Galatee.Silverlight.Facturation
{
    public partial class FrmSaisieDesZones : ChildWindow
    {
        public FrmSaisieDesZones()
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
        List<object> ListeReleveurObject = new List<object>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        int Action = 0;
        List<CsTournee> ListeTournee;
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
                     }
                     if (lesCentre.Count == 1)
                     {
                         this.Txt_Centre.Text = lesCentre.First().CODE ;
                         txt_libellecentre.Text  = lesCentre.First().LIBELLE;
                         this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                     }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeTournee(IdDesCentre);
                    return;

                }

                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    if (args != null && args.Cancelled)
                        return;
                    SessionObject.LstCentre = args.Result;
                    List<int> IdDesCentre = new List<int>();
                    lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    lstSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                    if (lstSite.Count == 1)
                    {
                        this.Txt_Site.Text = lstSite.First().CODE;
                        txt_LibelleSite.Text = lstSite.First().LIBELLE;
                        this.Txt_Site.Tag = lstSite.First().PK_ID;
                    }
                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE  ;
                        txt_libellecentre.Text  = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                    }
                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    ChargerListeTournee(IdDesCentre);
                };
                service.ListeDesDonneesDesSiteAsync(true);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<int> lstIdReleveur = new List<int>();
        private void ChargerListeTournee(List<int> lstCentre)
        {
            ListeTournee = new List<CsTournee>();
            FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));

            service.ChargerListeDesTourneesCompleted += (s, args) =>
            {
                try
                {
                    if (args.Cancelled || args.Error != null)
                    {
                        Message.ShowError("Erreur survenue à l'appel du service.", "Erreur");
                        return;
                    }

                    if (args.Result == null || args.Result.Count == 0)
                    {
                        Message.ShowError("Aucune tournée retournée par le système.", "Info");
                        return;
                    }
                    ListeTournee = args.Result;
                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE ;
                        txt_libellecentre.Text = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                        if (ListeTournee != null && ListeTournee.Count != 0)
                        {
                            this.dataGrid1.ItemsSource = null;
                            this.dataGrid1.ItemsSource = ListeTournee.Where(t => t.FK_IDCENTRE == lesCentre.First().PK_ID).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message.ShowError(ex, "Erreur");
                }
            };
            service.ChargerListeDesTourneesAsync(lstCentre);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult resultat = new DialogResult("Confirmer vous la saisie", false);//, "Warming", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                resultat.Closed += new EventHandler(DialogClosed);
                resultat.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreru");
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

                //this.dataGrid1.IsEnabled = true;
                //if (ListeReleveur.Count != 0)
                //    this.dataGrid1.SelectedItem = ListeReleveur[0];
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
                _LstColonneAffich.Add("CODE ");
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
                ServiceAccueil.CsCentre param = (ServiceAccueil.CsCentre)ctrs.MyObject;//.VALEUR;
                this.Txt_Centre.Text = param.CODE ;
                txt_libellecentre.Text  = param.LIBELLE;
                this.Txt_Centre.Tag  = param.PK_ID ;
                if (ListeTournee != null && ListeTournee.Count != 0)
                {
                    this.dataGrid1.ItemsSource = null;
                    this.dataGrid1.ItemsSource = ListeTournee.Where(t => t.FK_IDCENTRE == param.PK_ID).ToList();
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
                ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                this.Txt_Site.Text = param.CODE;
                txt_LibelleSite.Text  = param.LIBELLE;
                this.Txt_Site.Tag = param.PK_ID;
                this.btn_Centre.IsEnabled = true;
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
                    this.Txt_zone.Text = string.IsNullOrEmpty(((CsTournee)this.dataGrid1.SelectedItem).CODE) ? string.Empty : ((CsTournee)this.dataGrid1.SelectedItem).CODE;
                    this.Txt_priorite.Text = string.IsNullOrEmpty(((CsTournee)this.dataGrid1.SelectedItem).PRIORITE) ? string.Empty : ((CsTournee)this.dataGrid1.SelectedItem).PRIORITE;
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
                    //this.Txt_Matricule.Text = string.Empty;
                    //this.Txt_Matricule.IsReadOnly = false;

                    //this.Txt_Releveur.Text = string.Empty;
                    //this.Txt_Releveur.IsReadOnly = false;

                    //this.Txt_NumTerminalSaisie.Text = string.Empty;
                    //this.Txt_NumTerminalSaisie.IsReadOnly = false;

                    //this.Txt_Quota.Text = string.Empty;
                    //this.Txt_Quota.IsReadOnly = false;


                    this.dataGrid1.IsEnabled = false;


                }
                else if (_Action == 2)
                {
                    //this.Txt_Matricule.IsReadOnly = false;
                    //this.Txt_Releveur.IsReadOnly = false;
                    //this.Txt_NumTerminalSaisie.IsReadOnly = false;
                    //this.Txt_Quota.IsReadOnly = false;

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
               UcGererTournees u = new UcGererTournees(lstSite, lesCentre, ListeTournee, false);
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
               UcGererTournees u = new UcGererTournees(lstSite, lesCentre, ListeTournee,(CsTournee )dataGrid1.SelectedItem, true );
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
               //LeReleveurSelect.CENTRE = this.Txt_Centre.Text;
               //LeReleveurSelect.MATRICULE = this.Txt_Matricule.Text;
               //LeReleveurSelect.CODE = this.Txt_Releveur.Text;
               //LeReleveurSelect.FERMEQUOT = int.Parse(this.Txt_Quota.Text);
               //LeReleveurSelect.PORTABLE = this.Txt_NumTerminalSaisie.Text;

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

                       //ListeReleveur.RemoveAll(c =>c.FK_IDCENTRE == LeReleveur.FK_IDCENTRE &&  c.CENTRE == LeReleveur.CENTRE && c.CODE == LeReleveur.CODE && c.MATRICULE == LeReleveur.MATRICULE);
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

               //List<string> _LstColonneAffich = new List<string>();
               //_LstColonneAffich.Add("MATRICULE");
               //_LstColonneAffich.Add("LIBELLE");
               //List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(ListeUser.Where(t => t.CENTRE  == this.Txt_Centre.Text).ToList());
               //MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "LISTE DES UTILISATEURS");
               //ctrl.Closed += new EventHandler(galatee_OkClickedUser);
               //ctrl.Show();
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
               //this.Txt_Matricule.Text = c.MATRICULE;
               
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
             Galatee.Silverlight.Facturation.UcGererTournees rel = sender as UcGererTournees;
               if (rel.IsOkClick)
               {
                   if ( !((List<CsTournee>)this.dataGrid1.ItemsSource).Select(t=>t.CODE).Contains( rel._TourneeCreation.CODE))
                   {

                       ListeTournee.Add(rel._TourneeCreation);
                       Txt_Centre.Text = rel._TourneeCreation.CENTRE;
                       Txt_Centre_TextChanged(null, null);
                       if (ListeTournee != null && ListeTournee.Count != 0)
                       {
                           this.dataGrid1.ItemsSource = null;
                           this.dataGrid1.ItemsSource = ListeTournee.Where(t => t.FK_IDCENTRE == rel._TourneeCreation.FK_IDCENTRE).ToList();
                       }
                   }
                   else
                   {
                       Message.ShowWarning("Cette tournées existe déja sur ce centre", "Erreur");
                       return;
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
               //Txt_Centre.Text = ListeReleveur.First().CENTRE;
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

    }
}

