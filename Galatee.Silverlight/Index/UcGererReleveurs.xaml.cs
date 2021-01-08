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
    public partial class UcGererReleveurs : ChildWindow
    {
        public UcGererReleveurs()
        {
            InitializeComponent();
        }
        public  bool IsOkClick = false;
        bool _modification = false;
        public event EventHandler Closed;
        public CsReleveur _releveurCreation
        {
            set;
            get;
        }

        List<ServiceAccueil.CsSite> LstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();


        List<CParametre> LstUser;
        List<object> ListeReleveurObject = new List<object>();
        CsReleveur _releveur = new CsReleveur();
        List<CsReleveur> ListeReleveur = new  List<CsReleveur>();
        List<ServiceAdministration.CsUtilisateur> users;
        int Action = 0;

        public UcGererReleveurs(List<ServiceAccueil.CsSite> _LstSite, List<ServiceAccueil.CsCentre > _LstCentre, List<CsReleveur> _ListeReleveur, CsReleveur releveur, bool modification)
        {
            InitializeComponent();

            try
            {
                ListeReleveur = _ListeReleveur;
                LstSite = _LstSite;
                LstCentre = _LstCentre;
                _releveur = releveur;
                _modification = modification;

                if (LstSite.Count == 1)
                {
                    this.Txt_Site.Text = LstSite.First().CODE;
                    txt_LibelleSite.Text = LstSite.First().LIBELLE;
                    this.Txt_Site.Tag = LstSite.First().PK_ID;
                }
                if (_LstCentre.Count == 1)
                {
                    this.Txt_Centre.Text = _LstCentre.First().CODE;
                    txt_libellecentre.Text = _LstCentre.First().LIBELLE;
                    this.Txt_Centre.Tag = _LstCentre.First().PK_ID;
                }
               
                this.btn_Releveur .Visibility = System.Windows.Visibility.Collapsed;
                InitialisationChamp(releveur);
                

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        public UcGererReleveurs(List<ServiceAccueil.CsSite> _LstSite, List<ServiceAccueil.CsCentre> _LstCentre, List<ServiceAdministration.CsUtilisateur> _users, List<CsReleveur> _ListeReleveur, bool modification)
        {
            InitializeComponent();

            try
            {
                ListeReleveur = _ListeReleveur;
                LstSite = _LstSite;
                LstCentre = _LstCentre;
                _modification = modification;
                users = _users;

                if (LstSite.Count == 1)
                {
                    this.Txt_Site.Text = LstSite.First().CODE;
                    txt_LibelleSite.Text = LstSite.First().LIBELLE;
                    this.Txt_Site.Tag = LstSite.First().PK_ID;
                }
                if (_LstCentre.Count == 1)
                {
                    this.Txt_Centre.Text = _LstCentre.First().CODE;
                    txt_libellecentre.Text = _LstCentre.First().LIBELLE;
                    this.Txt_Centre.Tag = _LstCentre.First().PK_ID;
                }
                this.lb_LibelleAgent.Visibility = System.Windows.Visibility.Collapsed;
                Txt_Matricule.IsReadOnly = false;

            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void InitialisationChamp(CsReleveur releveur)
        {
            try
            {
                Txt_Centre.Text = releveur.CENTRE;
                Txt_Matricule.Text = releveur.MATRICULE;
                Txt_Quota.Text = string.IsNullOrEmpty(releveur.FERMEQUOT.ToString()) ? string.Empty : releveur.FERMEQUOT.ToString();
                Txt_NumTerminalSaisie.Text = string.IsNullOrEmpty(releveur.PORTABLE) ? string.Empty : releveur.PORTABLE;
                txt_libellecentre.Text = LstCentre.First(c => c.PK_ID  == releveur.FK_IDCENTRE ).LIBELLE;
                txt_LibelleSite.Text = LstCentre.First(c => c.PK_ID == releveur.FK_IDCENTRE).LIBELLESITE ;
                Txt_Site.Text = LstCentre.First(c => c.PK_ID == releveur.FK_IDCENTRE).CODESITE  ;
                Txt_codeReleveur.Text = string.IsNullOrEmpty(releveur.CODE) ? string.Empty : releveur.CODE;
                txt_NomReleveur.Text = string.IsNullOrEmpty(releveur.NOMRELEVEUR) ? string.Empty : releveur.NOMRELEVEUR; 
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
                IsOkClick = true;
                if (!_modification) // cas de la creation d'un releveur
                {
                    CsReleveur releveur = new CsReleveur();
                    releveur.CENTRE = Txt_Centre.Text;
                    releveur.FERMEQUOT = string.IsNullOrEmpty(Txt_Quota.Text) ? 0 : int.Parse(Txt_Quota.Text);
                    releveur.MATRICULE = string.IsNullOrEmpty( Txt_Matricule.Text)? string.Empty : Txt_Matricule.Text ;
                    releveur.PORTABLE = string.IsNullOrEmpty(Txt_NumTerminalSaisie.Text) ? string.Empty : Txt_NumTerminalSaisie.Text;
                    releveur.CODE = string.IsNullOrEmpty(Txt_codeReleveur.Text) ? string.Empty : Txt_codeReleveur.Text; // lbl_releveur.Content.ToString();
                    releveur.USERCREATION = UserConnecte.matricule;
                    releveur.DATECREATION = DateTime.Now.Date;
                    releveur.FK_IDCENTRE = (int)Txt_Centre.Tag;
                    releveur.FK_IDUSER = (int)Txt_Matricule.Tag;
                    if (ListeReleveur.FirstOrDefault(t => t.FK_IDCENTRE == releveur.FK_IDCENTRE && t.FK_IDUSER == releveur.FK_IDUSER ) == null)
                        InsererReleveur(releveur);
                    else
                    {
                        Message.ShowInformation("Ce releveur existe deja sur ce centre ", "Index");
                        return;
                    }
                }
                else // cas de modification
                {
                    _releveur.FERMEQUOT = string.IsNullOrEmpty(Txt_Quota.Text) ? 0 : int.Parse(Txt_Quota.Text);
                    _releveur.PORTABLE = string.IsNullOrEmpty(Txt_NumTerminalSaisie.Text) ? string.Empty : Txt_NumTerminalSaisie.Text;
                    _releveur.CODE = string.IsNullOrEmpty(Txt_codeReleveur.Text) ? string.Empty : Txt_codeReleveur.Text; // lbl_releveur.Content.ToString();
                    _releveur.USERMODIFICATION = UserConnecte.matricule;
                    _releveur.DATEMODIFICATION = DateTime.Now.Date;
                    CsReleveur leRel = ListeReleveur.FirstOrDefault(t => t.FK_IDCENTRE == _releveur.FK_IDCENTRE  && 
                                                                         t.CODE == _releveur.CODE &&
                                                                         t.PK_ID != _releveur.PK_ID );
                    if (leRel == null)
                        UpdateReleveur(_releveur);
                    else
                    {
                        Message.ShowInformation("Code releveur deja attribué", "Index");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstCentre.Where(t=>t.FK_IDCODESITE == (int)this.Txt_Site.Tag ).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste site");
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
                this.Txt_Centre.Text = param.CODE;
                txt_libellecentre.Text  = param.LIBELLE;
                this.Txt_Centre.Tag = param.PK_ID;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
            //ChargerListeTournee(ctrs.MySite.CENTRE, null);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.OKButton.Visibility = System.Windows.Visibility.Collapsed;
                //this.CancelButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void DialogClosed(object sender, EventArgs e)
       {
           try
           {
               DialogResult ctrs = sender as DialogResult;
               if (ctrs.Yes) // permet de tester si l'utilisateur a click sur Ok ou NON 
               {
               }
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

        CsReleveur  MajTourneeSelect(CsReleveur  LeReleveurSelect)
       {
           try
           {
               LeReleveurSelect.CENTRE = this.Txt_Centre.Text;
               //LeReleveurSelect.MATRICULE = this.l.Text;
               //LeReleveurSelect.RELEVEUR = this.txt.Text;
               LeReleveurSelect.FERMEQUOT = int.Parse(this.Txt_Quota.Text);
               LeReleveurSelect.PORTABLE = this.Txt_NumTerminalSaisie.Text;

               return LeReleveurSelect;
           }
           catch (Exception ex)
           {
               throw ex;
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
                           Message.ShowError("Erreur survenue lors de l'appel du service", "Erreur");
                           return;
                       }

                       if (args.Result == false)
                       {
                           Message.ShowError("Erreur d'insertion du releveur.Réessayer svp!", "Information");
                           return;
                       }
                       LeReleveur.NOMRELEVEUR = this.txt_NomReleveur.Text;
                       _releveurCreation = LeReleveur;
                       Closed(this, new EventArgs());
                       this.DialogResult = false;
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
                           Message.ShowError("Erreur survenue lors de l'appel du service.", "Erreur");
                           return;
                       }

                       if (args.Result == false)
                       {
                           Message.ShowError("Erreur survenue lors de la mise à jour du releveur.Réessayer svp!", "Information");
                           return;
                       }


                       ListeReleveur.RemoveAll(r => r.FK_IDCENTRE == LeReleveur.FK_IDCENTRE && r.CENTRE == LeReleveur.CENTRE && r.MATRICULE == LeReleveur.MATRICULE &&
                                                r.CODE == LeReleveur.CODE);
                       _releveurCreation = _releveur;
                       Closed(this, new EventArgs());
                       this.DialogResult = false;
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

       private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
       {
       }

       void galatee_OkClickedUser(object sender, EventArgs e)
       {
           try
           {
               MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
               ServiceAdministration.CsUtilisateur c = (ServiceAdministration.CsUtilisateur)ctrs.MyObject;
               this.Txt_Matricule.Text = c.MATRICULE;
               this.txt_NomReleveur.Text  = c.LIBELLE;
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
               List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(LstSite);
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

       private void Txt_NumTerminalSaisie_TextChanged(object sender, TextChangedEventArgs e)
       {

       }

       private void Txt_Matricule_TextChanged(object sender, TextChangedEventArgs e)
       {

       }

       private void btn_Releveur_Click(object sender, RoutedEventArgs e)
       {
           try
           {
               List<ServiceAdministration.CsUtilisateur> ListeUserAdmis = new List<ServiceAdministration.CsUtilisateur>();
               List<int> UserReleveurDuCentre = new List<int>();
               List<string> _LstColonneAffich = new List<string>();
               _LstColonneAffich.Add("MATRICULE");
               _LstColonneAffich.Add("LIBELLE");


               foreach (CsReleveur rel in ListeReleveur)
               {
                   if ((rel.FK_IDCENTRE == (int)this.Txt_Centre.Tag) && rel.FK_IDUSER > 0)
                       UserReleveurDuCentre.Add(rel.FK_IDUSER.Value);
               }

               //List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(users.Where(t => t.FK_IDCENTRE == (int)this.Txt_Centre.Tag).ToList());
               List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(users.Where(t => !UserReleveurDuCentre.Contains(t.PK_ID)).ToList());
               MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Liste réléveur");
               ctrl.Closed += new EventHandler(galatee_OkClickedreleveur);
               ctrl.Show();
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }

       void galatee_OkClickedreleveur(object sender, EventArgs e)
       {
           try
           {
               MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
               if (ctrs.isOkClick)
               {
                   ServiceAdministration.CsUtilisateur param = (ServiceAdministration.CsUtilisateur)ctrs.MyObject;//.VALEUR;
                   this.Txt_Matricule.Text = param.MATRICULE;
                   txt_NomReleveur.Text = param.LIBELLE;
                   this.Txt_Matricule.Tag = param.PK_ID;
               }
           }
           catch (Exception ex)
           {
               Message.ShowError(ex, "Erreur");
           }
       }
     
    }
}

