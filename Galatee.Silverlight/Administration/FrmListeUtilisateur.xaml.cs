using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Galatee.Silverlight.ServiceAdministration;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using System.Threading.Tasks;

namespace Galatee.Silverlight.Administration
{
    public partial class FrmListeUtilisateur : ChildWindow, INotifyPropertyChanged
    {
        public FrmListeUtilisateur()
        {
            InitializeComponent();
            try
            {
                GetListeProfil();
                ChargerDonneeDuSite();
                txt_Matricule.MaxLength = SessionObject.Enumere.TailleMatricule;
                this.DataContext = SelectedUser;

                Translate();
                List<ContextMenuItem> ContextMenuItem;
                var AdministrationNamespace = "Galatee.Silverlight.Administration.";
           
                ContextMenuItem = new List<ContextMenuItem>()
                     {
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuCreate,ModeExcecution=SessionObject.ExecMode.Creation,Title =Galatee.Silverlight.Resources.Langue.Ajout+ " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur },
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuConsult,ModeExcecution=SessionObject.ExecMode.Consultation,Title =Galatee.Silverlight.Resources.Langue.Consultation + " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur },
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuModify,ModeExcecution=SessionObject.ExecMode.Modification,Title =Galatee.Silverlight.Resources.Langue.Modification + " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur },
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcReinitialisePwd",Label=Galatee.Silverlight.Resources.Langue.ContextMenuChangePassword,ModeExcecution=SessionObject.ExecMode.ChangementMotDePasse,Title =Galatee.Silverlight.Resources.Langue.Réinitialiser + " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur },
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcSupprimerUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuDelete,ModeExcecution=SessionObject.ExecMode.Suppression,Title =Galatee.Silverlight.Resources.Langue.Delete+ " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur },
                        new ContextMenuItem(){ Code=AdministrationNamespace+"UcGererUser",Label=Galatee.Silverlight.Resources.Langue.ContextMenuMuter,ModeExcecution=SessionObject.ExecMode.Muter,Title =Galatee.Silverlight.Resources.Langue.Muter+ " " + Galatee.Silverlight.Resources.Administration.Langue.Utilisateur }
                     };
                SessionObject.MenuContextuelItem = ContextMenuItem;
               
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        List<int> IdDesCentre = new List<int>();
        List<ServiceAccueil.CsSite> lstSite = new List<ServiceAccueil.CsSite>();
        List<ServiceAccueil.CsCentre> lesCentre = new List<ServiceAccueil.CsCentre>();
        private void ChargerDonneeDuSite()
        {
            try
            {
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
                        this.btn_Site.IsEnabled = false;
                    }
                    else
                        this.btn_Site.IsEnabled = true;

                    if (lesCentre.Count == 1)
                    {
                        this.Txt_Centre.Text = lesCentre.First().CODE;
                        txt_libellecentre.Text = lesCentre.First().LIBELLE;
                        this.Txt_Centre.Tag = lesCentre.First().PK_ID;
                        this.btn_Centre.IsEnabled = false;
                    }
                    else
                        this.btn_Centre.IsEnabled = true;

                    foreach (ServiceAccueil.CsCentre item in lesCentre)
                        IdDesCentre.Add(item.PK_ID);
                    GetListeUtilisateur(IdDesCentre);
                };
                service.ListeDesDonneesDesSiteAsync(false);
                service.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void GetListeProfil()
        {
            try
            {
                if (SessionObject.ListeDesProfils != null && SessionObject.ListeDesProfils.Count != 0)
                {
                    ProfilListe = SessionObject.ListeDesProfils;
                    return;
                }
                AdministrationServiceClient admClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                admClient.RetourneListeAllProfilUserCompleted += (sen, result) =>
                {
                    if (result.Cancelled || result.Error != null)
                    {
                        string error = result.Error.Message;
                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "GetAllRole");
                        return;
                    }

                    if (result.Result != null && result.Result.Count > 0)
                    {
                        SessionObject.ListeDesProfils = result.Result;
                        ProfilListe = result.Result;
                    }
                };
                admClient.RetourneListeAllProfilUserAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void Translate()
        {
            try
            {
                lvwResultat.Columns[1].Header = Galatee.Silverlight.Resources.Langue.lblCentre;
                lvwResultat.Columns[2].Header = Galatee.Silverlight.Resources.Langue.dg_matricule;
                lvwResultat.Columns[3].Header = Galatee.Silverlight.Resources.Langue.dg_loginName;
                lvwResultat.Columns[4].Header = Galatee.Silverlight.Resources.Langue.dg_nomprenom;
                lvwResultat.Columns[5].Header = Galatee.Silverlight.Resources.Langue.dg_statuscompte;
                lvwResultat.Columns[6].Header = Galatee.Silverlight.Resources.Langue.dg_jobtitle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        List<CsUtilisateur> donnesDatagrid = new List<CsUtilisateur>();

        ObservableCollection<CsUtilisateur> donnesSelected = new ObservableCollection<CsUtilisateur>();

        #region INotifyPropertyChanged Membres

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string nompropriete)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nompropriete));
        }
        #endregion

        public CsUtilisateur SelectedUser
        {
            get;
            set;
        }

        private string text = string.Empty;

        List<CsSite> SiteListe = new List<CsSite>();
        List<CsCentre> CentreListe = new List<CsCentre>();
        List<CsProfil> ProfilListe = new List<CsProfil>();
        void GetListeUtilisateur(List<int> lstCentre)
        {
            try
            {

                AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                client.RetourneListeAllUserPerimetreCompleted += (ss, res) =>
                {
                    if (res.Cancelled || res.Error != null)
                    {
                        string error = res.Error.Message;
                        Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        desableProgressBar();
                        return;
                    }

                    if (res.Result == null || res.Result.Count == 0)
                    {
                        Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                        desableProgressBar();
                        return;
                    }
                    SessionObject.ListeDesUtilisateurs = res.Result;              

                    foreach (CsUtilisateur item in res.Result)
                    {
                        item.CENTREAFFICHER = item.CENTRE + "  " + item.LIBELLECENTRE;
                        if (item.PERIMETREACTION == 1)
                            item.LIBELLEPERIMETREACTION = "Centre";
                        else if (item.PERIMETREACTION == 2)
                            item.LIBELLEPERIMETREACTION = "Site";
                        else if (item.PERIMETREACTION == 3)
                            item.LIBELLEPERIMETREACTION = "Globale";

                    }
                    List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
                    List<int> lstCentreHabil = new List<int>();
                    List<CsUtilisateur> _lstUserProfil  = new List<CsUtilisateur>();

                    foreach (var item in lstCentreProfil)
                        lstCentreHabil.Add(item.PK_ID);
                    if (UserConnecte.matricule != "99999")
                        _lstUserProfil = res.Result.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList();
                    else
                        _lstUserProfil = res.Result;

                    donnesDatagrid = _lstUserProfil;
                    var listeTemp = donnesDatagrid.Select(t => new { t.PASSE ,t.PK_ID, t.CENTREAFFICHER, t.CENTRE, t.MATRICULE, t.LOGINNAME, t.LIBELLE, t.LIBELLESTATUSCOMPTE, t.LIBELLEPERIMETREACTION }).Distinct().ToList();
                    List<CsUtilisateur> lstSourceDatagrid = new List<CsUtilisateur>();

                    foreach (var item in listeTemp)
                        lstSourceDatagrid.Add(new CsUtilisateur { PASSE = item.PASSE, CENTREAFFICHER = item.CENTREAFFICHER, PK_ID = item.PK_ID, CENTRE = item.CENTRE, MATRICULE = item.MATRICULE, LOGINNAME = item.LOGINNAME, LIBELLE = item.LIBELLE, LIBELLESTATUSCOMPTE = item.LIBELLESTATUSCOMPTE, LIBELLEPERIMETREACTION = item.LIBELLEPERIMETREACTION });
                

                    foreach (CsUtilisateur item in lstSourceDatagrid)
                    {
                        int toto = 0;
                        CsUtilisateur usrtemp = new CsUtilisateur();

                        foreach (CsUtilisateur usr in donnesDatagrid)
                        {
                            if (usr.LOGINNAME == item.LOGINNAME)
                            {
                                toto += 1;
                                usrtemp = usr;
                            }
                        }

                        if (toto > 1)
                        {
                            item.LIBELLEFONCTION = "Multiple";
                        }
                        else
                            if (toto == 1)
                            {
                                item.LIBELLEFONCTION = usrtemp.LIBELLEFONCTION;
                            }

                    }

                    //lvwResultat.ItemsSource = _lstUserProfil;
                    lvwResultat.ItemsSource = lstSourceDatagrid;

                    if (_lstUserProfil != null)
                        lvwResultat.SelectedItem = _lstUserProfil[0];
                };
                client.RetourneListeAllUserPerimetreAsync(lstCentre);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        void desableProgressBar()
        {
            progressBar1.IsIndeterminate = false;
            progressBar1.Visibility = Visibility.Collapsed;
        }

        void allowProgressBar()
        {
            progressBar1.IsEnabled = true;
            progressBar1.Visibility = Visibility.Visible;
            progressBar1.IsIndeterminate = true;
        }

        private void lvwResultat_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

        }

        private void lvwResultat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvwResultat.SelectedItem == null)
                return;

            List<CsUtilisateur> _lstUtil = (List<CsUtilisateur>)lvwResultat.ItemsSource;
            SelectedUser = lvwResultat.SelectedItem as CsUtilisateur;
            if (SelectedUser.ISSELECT )
                SelectedUser.ISSELECT = false;
            else
                SelectedUser.ISSELECT = true;

            List<CsUtilisateur> pUser = (lvwResultat.ItemsSource as List<CsUtilisateur>).Where(t => t.ISSELECT).ToList();
            if (pUser != null && pUser.Count > 1)
            {
                this.Btn_modifier.IsEnabled = false;
                this.Btn_nouveau.IsEnabled = false;
                this.Btn_muter.IsEnabled = false;
                this.Btn_Reinitialiser.IsEnabled = false;
                this.Btn_Supprimer.IsEnabled = false;
            }
            else
            {
                this.Btn_modifier.IsEnabled = true ;
                this.Btn_nouveau.IsEnabled = true ;
                this.Btn_muter.IsEnabled = true ;
                this.Btn_Reinitialiser.IsEnabled = true ;
                this.Btn_Supprimer.IsEnabled = true ;
            }

            //List<CsUtilisateur> lstDejaCoche = _lstUtil.Where(t => t.IsSELECT == true && t.PK_ID != SelectedUser.PK_ID).ToList();
            //if (lstDejaCoche != null && lstDejaCoche.Count != 0)
            //    lstDejaCoche.ForEach(t => t.IsSELECT = false);

            SessionObject.objectSelected = lvwResultat.SelectedItem as CsUtilisateur;
            SessionObject.gridUtilisateur = lvwResultat;

        }

        private void myDataGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string Ucname = string.Empty;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }

        private void dataGrid_Profil_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_nouveau_Click(object sender, RoutedEventArgs e)
        {
            //nouveau User 

            try
            {                
                String alloper = "1";//nouveau User 
                int one = 1;
                var FrmGererUser = new UcGererUser(alloper, one);
                FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                this.IsEnabled = false;
                FrmGererUser.Show();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.txtDevis);
            }

        }

        void RechagerListeUtilisateur()
        {
            GetListeUtilisateur(IdDesCentre);
        }

        private void FrmGererUser_Closed(object sender, EventArgs e)
        {
            try
            {
                if (this.Btn_nouveau.IsEnabled == false)
                   this.Btn_nouveau.IsEnabled = true ;

                if (this.Btn_modifier.IsEnabled == false)
                    this.Btn_modifier.IsEnabled = true;

                if (this.Btn_muter.IsEnabled == false)
                    this.Btn_muter.IsEnabled = true;

                if (this.Btn_Reinitialiser.IsEnabled == false)
                    this.Btn_Reinitialiser.IsEnabled = true;
                
                
                RechagerListeUtilisateur();
                this.IsEnabled = true ;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_modifier_Click(object sender, RoutedEventArgs e)
        {
           
            //modifier User 
            CsUtilisateur pUser = (lvwResultat.SelectedItem as CsUtilisateur);

            if (pUser != null && pUser.PK_ID.ToString() != null && pUser.LOGINNAME != null)
            {
                try
                {
                    var FrmGererUser = new UcGererUser(pUser, SessionObject.ExecMode.Modification, ProfilListe);
                    FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                    this.IsEnabled = false;
                    FrmGererUser.Show();

                }
                catch (Exception ex)
                {
                    //Message.ShowError(ex.Message, Languages.txtDevis);
                }
            }
            else
            {
                if (this.Btn_nouveau.IsEnabled == false)
                    this.Btn_nouveau.IsEnabled = true;

                if (this.Btn_modifier.IsEnabled == false)
                    this.Btn_modifier.IsEnabled = true;

                if (this.Btn_muter.IsEnabled == false)
                    this.Btn_muter.IsEnabled = true;

                if (this.Btn_Reinitialiser.IsEnabled == false)
                    this.Btn_Reinitialiser.IsEnabled = true;
                Message.Show(" Veuillez choisir l'utilisateur à modifier. ", Galatee.Silverlight.Resources.Langue.Error_Title);
            }
        }

        private void Btn_muter_Click(object sender, RoutedEventArgs e)
        {
            Btn_muter.IsEnabled = false;
            CsUtilisateur pUser = (lvwResultat.SelectedItem as CsUtilisateur);
            if (pUser != null && pUser.PK_ID.ToString() != null && pUser.LOGINNAME != null)
            {
                try
                {
                    var FrmGererUser = new UcGererUser(pUser, SessionObject.ExecMode.Muter,ProfilListe);
                    FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                    FrmGererUser.Show();

                }
                catch (Exception ex)
                {
                    //Message.ShowError(ex.Message, Languages.txtDevis);
                }
            }
            else
            {
                Message.Show(" Veuillez choisir l'utilisateur à modifier. ", Galatee.Silverlight.Resources.Langue.Error_Title);
            }
        }

        private void Btn_Consultation_Click(object sender, RoutedEventArgs e)
        {
            //modifier User 
            CsUtilisateur pUser = (lvwResultat.SelectedItem as CsUtilisateur);

            if (pUser != null && pUser.PK_ID.ToString() != null && pUser.LOGINNAME != null)
            {
                try
                {
                    var FrmGererUser = new UcGererUser(pUser, SessionObject.ExecMode.Consultation , ProfilListe);
                    FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                    FrmGererUser.Show();

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message,"Administration");
                }
            }
            else
            {
                Message.Show(" Veuillez choisir l'utilisateur à modifier. ", Galatee.Silverlight.Resources.Langue.Error_Title);
            }
        }

        private void Btn_Reinitialiser_Click(object sender, RoutedEventArgs e)
        {
            Btn_Reinitialiser.IsEnabled = false;
            CsUtilisateur pUser = (lvwResultat.SelectedItem as CsUtilisateur);
            if (pUser != null && pUser.PK_ID.ToString() != null && pUser.LOGINNAME != null)
            {
                try
                {
                    var FrmGererUser = new UcReinitialisePwd(pUser, SessionObject.ExecMode.Suppression  );
                    FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                    FrmGererUser.Show();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Message.Show(" Veuillez choisir l'utilisateur à modifier. ", Galatee.Silverlight.Resources.Langue.Error_Title);
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
                ctrl.Closed += ctrl_ClosedSite;
                this.IsEnabled = false;
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void ctrl_ClosedSite(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true;
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    this.Txt_Centre.Text = string.Empty;
                    this.Txt_Centre.Tag = null;
                    this.txt_libellecentre.Text = string.Empty;

                    ServiceAccueil.CsSite param = (ServiceAccueil.CsSite)ctrs.MyObject;//.VALEUR;
                    this.Txt_Site.Text = param.CODE;
                    txt_LibelleSite.Text = param.LIBELLE;
                    this.Txt_Site.Tag = param.PK_ID;
                    this.btn_Centre.IsEnabled = true;
                    if (param.CODE == SessionObject.Enumere.Generale) isgeneral = true;
                    else isgeneral = false;
                    List<ServiceAccueil.CsCentre> lstCEnt = lesCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList();
                    if (lstCEnt != null && lstCEnt.Count != 0)
                    {
                        if (lstCEnt.Count == 1)
                        {
                            this.Txt_Centre.Text = lstCEnt.First().CODE;
                            txt_libellecentre.Text = lstCEnt.First().LIBELLE;
                            this.Txt_Centre.Tag = lstCEnt.First().PK_ID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        bool isgeneral = false;

        private void btn_Centre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> _LstColonneAffich = new List<string>();
                _LstColonneAffich.Add("CODE");
                _LstColonneAffich.Add("LIBELLE");
                List<object> obj = Shared.ClasseMEthodeGenerique.RetourneListeObjet(lesCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList());
                MainView.UcListeGenerique ctrl = new MainView.UcListeGenerique(obj, _LstColonneAffich, false, "Lots");
                ctrl.Closed += ctrl_ClosedCentre;
                this.IsEnabled = false;
                ctrl.Show();
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }

        void ctrl_ClosedCentre(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true ;
                MainView.UcListeGenerique ctrs = sender as MainView.UcListeGenerique;
                if (ctrs.isOkClick)
                {
                    ServiceAccueil.CsCentre param = (ServiceAccueil.CsCentre)ctrs.MyObject;//.VALEUR;
                    this.Txt_Centre.Text = param.CODE;
                    txt_libellecentre.Text = param.LIBELLE;
                    this.Txt_Centre.Tag = param.PK_ID;
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }


        private void btn_Afficher_Click(object sender, RoutedEventArgs e)
        {
            List<CsUtilisateur> lesUser = new List<CsUtilisateur>();
            if (!string.IsNullOrEmpty( this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text) )
            {
                lesUser = SessionObject.ListeDesUtilisateurs.Where(t=>t.MATRICULE ==this.txt_Matricule.Text).ToList() ;
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(SessionObject.ListeDesUtilisateurs);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }
            if (!string.IsNullOrEmpty(this.txt_Nom.Text) && string.IsNullOrEmpty(this.txt_Matricule.Text))
            {
                lesUser = SessionObject.ListeDesUtilisateurs.Where(t => t.LIBELLE.Contains(this.txt_Nom.Text.ToUpper())).ToList();
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(SessionObject.ListeDesUtilisateurs);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }
            if (!string.IsNullOrEmpty(this.txt_Matricule.Text) && !string.IsNullOrEmpty(this.txt_Nom.Text))
            {
                lesUser = SessionObject.ListeDesUtilisateurs.Where(t => t.MATRICULE == this.txt_Matricule.Text && t.LIBELLE.Contains(this.txt_Nom.Text.ToUpper())).ToList();
                if (lesUser.Count == 0)
                {
                    Message.ShowInformation("Aucun éléments trouvé pour les critère", "Administration");
                    RafraichirDatagrig(SessionObject.ListeDesUtilisateurs);
                }
                else
                    RafraichirDatagrig(lesUser);
                return;
            }
      
            if (isgeneral)
            {
                  lesUser = SessionObject.ListeDesUtilisateurs;
                    RafraichirDatagrig(lesUser);
                return ;
            }
           if (this.Txt_Centre.Tag != null && this.Txt_Site.Tag != null)
            {
                List<ServiceAccueil.CsCentre> lstCentre = SessionObject.LstCentre.Where(t => t.PK_ID == (int)this.Txt_Centre.Tag).ToList();
                var lstuserCentre = from x in lstCentre
                                    join y in SessionObject.ListeDesUtilisateurs on x.PK_ID equals y.FK_IDCENTRE
                                    select y;
                foreach (var item in lstuserCentre)
                    lesUser.Add(item);
            }
           else if (this.Txt_Centre.Tag == null && this.Txt_Site.Tag != null)
            {
                List<ServiceAccueil.CsCentre> lstCentre = SessionObject.LstCentre.Where(t => t.FK_IDCODESITE == (int)this.Txt_Site.Tag).ToList();
                var lstuserCentre = from x in lstCentre
                                    join y in SessionObject.ListeDesUtilisateurs on x.PK_ID equals y.FK_IDCENTRE
                                    select y;
                foreach (var item in lstuserCentre)
                    lesUser.Add(item);
            }
           RafraichirDatagrig(lesUser);

        }

        void RafraichirDatagrig(List<CsUtilisateur> _lstUser)
        {

            foreach (CsUtilisateur item in _lstUser)
            {
                item.CENTREAFFICHER = item.CENTRE + "  " + item.LIBELLECENTRE;
                if (item.PERIMETREACTION == 1)
                    item.LIBELLEPERIMETREACTION = "Centre";
                else if (item.PERIMETREACTION == 2)
                    item.LIBELLEPERIMETREACTION = "Site";
                else if (item.PERIMETREACTION == 3)
                    item.LIBELLEPERIMETREACTION = "Globale";

            }
            List<Galatee.Silverlight.ServiceAccueil.CsCentre> lstCentreProfil = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre, UserConnecte.listeProfilUser);
            List<int> lstCentreHabil = new List<int>();
            List<CsUtilisateur> _lstUserProfil = new List<CsUtilisateur>();

            foreach (var item in lstCentreProfil)
                lstCentreHabil.Add(item.PK_ID);
            if (UserConnecte.matricule != "99999")
                _lstUserProfil = _lstUser.Where(t => lstCentreHabil.Contains(t.FK_IDCENTRE)).ToList();
            else
                _lstUserProfil = _lstUser;

            donnesDatagrid = _lstUserProfil;
            var listeTemp = donnesDatagrid.Select(t => new {t.PASSE , t.PK_ID, t.CENTREAFFICHER, t.CENTRE, t.MATRICULE, t.LOGINNAME, t.LIBELLE, t.LIBELLESTATUSCOMPTE, t.LIBELLEPERIMETREACTION }).Distinct().ToList();
            List<CsUtilisateur> lstSourceDatagrid = new List<CsUtilisateur>();

            foreach (var item in listeTemp)
                lstSourceDatagrid.Add(new CsUtilisateur {PASSE=item.PASSE , CENTREAFFICHER = item.CENTREAFFICHER, PK_ID = item.PK_ID, CENTRE = item.CENTRE, MATRICULE = item.MATRICULE, LOGINNAME = item.LOGINNAME, LIBELLE = item.LIBELLE, LIBELLESTATUSCOMPTE = item.LIBELLESTATUSCOMPTE, LIBELLEPERIMETREACTION = item.LIBELLEPERIMETREACTION });

            foreach (CsUtilisateur item in lstSourceDatagrid)
            {
                int toto = 0;
                CsUtilisateur usrtemp = new CsUtilisateur();

                foreach (CsUtilisateur usr in donnesDatagrid)
                {
                    if (usr.LOGINNAME == item.LOGINNAME)
                    {
                        toto += 1;
                        usrtemp = usr;
                    }
                }

                if (toto > 1)
                {
                    item.LIBELLEFONCTION = "Multiple";
                }
                else
                    if (toto == 1)
                    {
                        item.LIBELLEFONCTION = usrtemp.LIBELLEFONCTION;
                    }

            }
            lvwResultat.ItemsSource = lstSourceDatagrid;
            if (_lstUserProfil != null && _lstUserProfil.Count != 0)
                lvwResultat.SelectedItem = _lstUserProfil[0];
        }

        private void Btn_Editer_Click(object sender, RoutedEventArgs e)
        {
            List<CsUtilisateur> pUser = (lvwResultat.ItemsSource as List<CsUtilisateur>).Where(t => t.ISSELECT).ToList();

            if (pUser != null)
            {
                FrOPtionEdition OptionEdition = new FrOPtionEdition(pUser);
                OptionEdition.Show();
            }
            else
                Message.Show("Sélectionner un utilisateur", "Edition");
            //List<CsHabilitationMenu> lesHabil = new List<CsHabilitationMenu>();
            //CsUtilisateur pUser = (lvwResultat.SelectedItem as CsUtilisateur);
            //List<int> leId = new List<int>();
            //leId.Add(pUser.PK_ID);
            //RetourneHabilitationUser(leId);
        }

      

        private void RetourneHabilitationUser(List<int> ListidClient)
        {
            AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
            client.RetourneProfilUtilisateurCompleted += (ss, res) =>
            {
                if (res.Cancelled || res.Error != null)
                {
                    string error = res.Error.Message;
                    Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                    return;
                }
                if (res.Result == null )
                {
                    Message.ShowInformation(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
                    return;
                }

                Utility.ActionDirectOrientation<ServicePrintings.CsHabilitationMenu, ServiceAdministration.CsHabilitationMenu>(res.Result, null, SessionObject.CheminImpression, "ReportHabillitationMenuUser", "Administration", true);

            };
            client.RetourneProfilUtilisateurAsync(ListidClient);

        }

        private void Btn_Supprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Btn_Supprimer.IsEnabled = false;
                var FrmGererUser = new UcSupprimerUser(this.lvwResultat.SelectedItem as CsUtilisateur,lvwResultat);
                FrmGererUser.Closed += new EventHandler(FrmGererUser_Closed);
                FrmGererUser.Show();
            }
            catch (Exception ex)
            {
                //Message.ShowError(ex.Message, Languages.txtDevis);
            }
        }

        private void txt_Matricule_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text))
                RafraichirDatagrig(SessionObject.ListeDesUtilisateurs);
        }

        private void txt_Nom_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_Matricule.Text) && string.IsNullOrEmpty(this.txt_Nom.Text))
                RafraichirDatagrig(SessionObject.ListeDesUtilisateurs);
        }

    }
}


