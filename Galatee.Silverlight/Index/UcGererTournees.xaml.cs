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
    public partial class UcGererTournees : ChildWindow
    {
        public UcGererTournees()
        {
            InitializeComponent();
        }
        public  bool IsOkClick = false;
        bool _modification = false;
        public event EventHandler Closed;
        public CsTournee _TourneeCreation
        {
            set;
            get;
        }

        List<ServiceAccueil.CsSite> LstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();


        CsTournee LaTournee = new CsTournee();
        List<CsTournee> ListeTournee = new List<CsTournee>();
        List<ServiceAdministration.CsUtilisateur> users;
        int Action = 0;

        public UcGererTournees(List<ServiceAccueil.CsSite> _LstSite, List<ServiceAccueil.CsCentre> _LstCentre, List<CsTournee> _ListeTournee, CsTournee _LaTournee, bool modification)
        {
            InitializeComponent();

            try
            {
                ListeTournee = _ListeTournee;
                LstSite = _LstSite;
                LstCentre = _LstCentre;
                LaTournee = _LaTournee;
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
                InitialisationChamp(LaTournee);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        public UcGererTournees(List<ServiceAccueil.CsSite> _LstSite, List<ServiceAccueil.CsCentre> _LstCentre,  List<CsTournee> _ListeTournee, bool modification)
        {
            InitializeComponent();

            try
            {
                LstSite = _LstSite;
                LstCentre = _LstCentre;
                _modification = modification;

                if (LstSite.Count == 1)
                {
                    this.Txt_Site.Text = LstSite.First().CODE;
                    txt_LibelleSite.Text = LstSite.First().LIBELLE;
                    this.Txt_Site.Tag = LstSite.First().PK_ID;
                }
                if (_LstCentre.Count == 1)
                {
                    this.Txt_Centre.Text = _LstCentre.First().CODE ;
                    txt_libellecentre.Text = _LstCentre.First().LIBELLE;
                    this.Txt_Centre.Tag = _LstCentre.First().PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        private void InitialisationChamp(CsTournee  tournee)
        {
            try
            {
                Txt_Centre.Text = tournee.CENTRE;
                txt_libellecentre.Text = LstCentre.First(c => c.PK_ID == tournee.FK_IDCENTRE).LIBELLE;
                txt_LibelleSite.Text = LstCentre.First(c => c.PK_ID == tournee.FK_IDCENTRE).LIBELLESITE;
                Txt_Site.Text = LstCentre.First(c => c.PK_ID == tournee.FK_IDCENTRE).CODESITE;
                Txt_codeTournee .Text = string.IsNullOrEmpty(tournee.CODE) ? string.Empty : tournee.CODE;
                this.Txt_Centre.Tag = tournee.FK_IDCENTRE ;
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
                    CsTournee Tournee = new CsTournee();
                    Tournee.CENTRE = Txt_Centre.Text;
                    Tournee.PRIORITE = string.IsNullOrEmpty(Txt_Priorite.Text) ? string.Empty : Txt_Priorite.Text;
                    Tournee.CODE = string.IsNullOrEmpty(Txt_codeTournee.Text) ? string.Empty : Txt_codeTournee.Text; 
                    Tournee.USERCREATION = UserConnecte.matricule;
                    Tournee.DATECREATION = DateTime.Now.Date;
                    Tournee.FK_IDCENTRE = (int)Txt_Centre.Tag;
                    if (string.IsNullOrEmpty( Tournee.CODE))
                    {
                        Message.ShowInformation("Vous devez saisir le code obligatoirement", "Index");
                        return;
                    }
                    if (ListeTournee.FirstOrDefault(t => t.FK_IDCENTRE == LaTournee.FK_IDCENTRE && t.CODE == LaTournee.CODE ) == null)
                        InsererTournee(Tournee);
                    else
                    {
                        Message.ShowInformation("Ce releveur existe deja sur ce centre ", "Index");
                        return;
                    }
                }
                else // cas de modification
                {
                    LaTournee.PRIORITE = string.IsNullOrEmpty(Txt_Priorite.Text) ? string.Empty : Txt_Priorite.Text;
                    LaTournee.CODE = string.IsNullOrEmpty(Txt_codeTournee.Text) ? string.Empty : Txt_codeTournee.Text;
                    LaTournee.USERMODIFICATION = UserConnecte.matricule;
                    LaTournee.DATEMODIFICATION = DateTime.Now.Date;
                    CsTournee laTourn = ListeTournee.FirstOrDefault(t => t.FK_IDCENTRE == LaTournee.FK_IDCENTRE &&
                                                                         t.CODE == LaTournee.CODE &&
                                                                         t.PK_ID != LaTournee.PK_ID);
                    if (laTourn == null)
                        UpdateTournee(LaTournee);
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
                this.Txt_Centre.Text = param.CODE ;
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

        private void InsererTournee(CsTournee LaTournee)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));

                service.InsererUneTourneeCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel du service", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur d'insertion du releveur.Réessayer svp!", "Information");
                            return;
                        }

                        if (args.Result == true)
                        {
                            Message.ShowInformation("Insertion validée.", "Information");
                            this.DialogResult = true;
                            //return;
                        }

                        _TourneeCreation = LaTournee;
                        ListeTournee.Add(LaTournee);
                        Closed(this, new EventArgs());
                        this.DialogResult = false;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.InsererUneTourneeAsync(LaTournee);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        private void UpdateTournee(CsTournee LeTournee)
        {
            try
            {
                FacturationServiceClient service = new FacturationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Facturation"));

                service.UpdateUneTourneeCompleted += (s, args) =>
                {
                    try
                    {
                        if (args != null && args.Cancelled)
                        {
                            Message.ShowError("Erreur survenue lors de l'appel du service.", "Erreur");
                            return;
                        }

                        if (args.Result == null)
                        {
                            Message.ShowError("Erreur survenue lors de la mise à jour de la tournée.Réessayer svp!", "Information");
                            return;
                        }

                        if (args.Result == true)
                        {
                            Message.ShowInformation("Insertion de la tournée validée.", "Information");
                            this.DialogResult = true;
                            //return;
                        }
                        _TourneeCreation = LeTournee;
                        ListeTournee.RemoveAll(r => r.CENTRE == LaTournee.CENTRE && r.CODE == LaTournee.CODE &&
                                                 r.RELEVEUR == LaTournee.RELEVEUR);
                        ListeTournee.Add(LeTournee);

                        Closed(this, new EventArgs());
                        this.DialogResult = false;
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.UpdateUneTourneeAsync(LeTournee);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

       private void Txt_Centre_TextChanged(object sender, TextChangedEventArgs e)
       {
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


     
    }
}

