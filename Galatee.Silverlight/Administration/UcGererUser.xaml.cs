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
using Galatee.Silverlight.ServiceAdministration;
using Galatee.Silverlight.Library;
using Galatee.Silverlight.Resources.Administration;
//using Galatee.Silverlight.ServiceAuthenInitialize;
using Galatee.Silverlight.Security;
using Galatee.Silverlight.Resources;
/*treeview*/
using System.Collections.ObjectModel;
using Galatee.Silverlight.Classes;
using UpdateControls.XAML;
using Galatee.Silverlight.Library.Models;
using Galatee.Silverlight.Library.ViewModels;
using System.Text.RegularExpressions;

namespace Galatee.Silverlight.Administration
{
    public partial class UcGererUser : ChildWindow
    {
        public UcGererUser()
        {
            InitializeComponent();
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;

        }

        ServiceAdministration.CsUtilisateur userSelected;
        //les données du user modifier
        List<CsProfil> lesProfilDuUser = new List<CsProfil>();
        List<CsProfil> lesProfilDuUserInit = new List<CsProfil>();
        List<CsProfil> lesProfilDuUserFinal = new List<CsProfil>();
        List<CsSite> leSiteDuUser = new List<CsSite>();
        List<CsCentre> leCentreDuUser = new List<CsCentre>();

        List<CsSite> SiteListe = new List<CsSite>();
        List<CsCentre> CentreListe = new List<CsCentre>();
        List<CsProfil> ProfilListe = new List<CsProfil>();

        DataGrid gridView = null;
        string CodeCaisse = string.Empty;
        SessionObject.ExecMode _Action;
        CsFonction _LaFonctionSelect;
        CsProfil _LeProfilSelect;

        List<CsCentreDuProfil> lesParamProfilsUser = new List<CsCentreDuProfil>();
        List<CsCentreDuProfil> lesParamProfilsUserInit = new List<CsCentreDuProfil>();
        List<CsCentreDuProfil> lesParamProfilsUserFinal = new List<CsCentreDuProfil>();


        void ChargerListDesSite()
        {
            try
            {
                if (SessionObject.LstCentre != null && SessionObject.LstCentre.Count != 0)
                {
                    List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(SessionObject.LstCentre.Where(p => p.CODE != SessionObject.Enumere.Generale).ToList(), UserConnecte.listeProfilUser);
                    List<ServiceAccueil.CsSite> lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);

                    CentreListe.AddRange(Shared.ClasseMEthodeGenerique.ConvertTypeCentre(lesCentre));
                    SiteListe.AddRange(Utility.ConvertListType<CsSite, ServiceAccueil.CsSite>(lesSite));

                    Cbo_centre.ItemsSource = CentreListe;
                    Cbo_centre.SelectedValuePath = "CODE";
                    Cbo_centre.DisplayMemberPath = "LIBELLE";

                    List<CsSite> ListeDesSite = new List<CsSite>();
                    ListeDesSite.Add(new CsSite());
                    ListeDesSite.AddRange(SiteListe.Where(p => p.CODE != "000").ToList());
                    cbo_site.ItemsSource = ListeDesSite;
                    cbo_site.SelectedValuePath = "CODE";
                    cbo_site.DisplayMemberPath = "LIBELLE";
                    return;
                }
                Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient service = new Galatee.Silverlight.ServiceAccueil.AcceuilServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Accueil"));
                service.ListeDesDonneesDesSiteCompleted += (s, args) =>
                {
                    try
                    {
                        List<ServiceAccueil.CsCentre> LstCentre = new List<ServiceAccueil.CsCentre>();
                        if (args != null && args.Cancelled)
                            return;
                        LstCentre = args.Result;
                        SessionObject.LstCentre = LstCentre;
                        if (LstCentre.Count != 0)
                        {
                            List<ServiceAccueil.CsCentre> lesCentre = Shared.ClasseMEthodeGenerique.RetourCentreByPerimetre(LstCentre, UserConnecte.listeProfilUser);
                            List<ServiceAccueil.CsSite> lesSite = Shared.ClasseMEthodeGenerique.RetourneSiteByCentre(lesCentre);
                            CentreListe.AddRange(Shared.ClasseMEthodeGenerique.ConvertTypeCentre(lesCentre));
                            SiteListe.AddRange(Utility.ConvertListType<CsSite, ServiceAccueil.CsSite>(lesSite));

                            Cbo_centre.ItemsSource = CentreListe;
                            Cbo_centre.SelectedValuePath = "CODE";
                            Cbo_centre.DisplayMemberPath = "LIBELLE";

                            cbo_site.ItemsSource = SiteListe.Where(p => p.CODE != "000").ToList();
                            cbo_site.SelectedValuePath = "CODE";
                            cbo_site.DisplayMemberPath = "LIBELLE";
                        }
                        else
                        {
                            Message.ShowInformation("Aucun centre trouvé en base.", "Erreur");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex, "Erreur");
                    }

                };
                service.ListeDesDonneesDesSiteAsync(false);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, "Erreur");
            }
        }
        void GetListeProfil()
        {
            try
            {
                //if (SessionObject.ListeDesProfils != null && SessionObject.ListeDesProfils.Count != 0)
                //{
                //    ProfilListe = SessionObject.ListeDesProfils;
                //    return;
                //}
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

        public UcGererUser(string _sens)
        {
            InitializeComponent();
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;

        }
        SessionObject.ExecMode mpe = new SessionObject.ExecMode();
        public UcGererUser(object[] _user, SessionObject.ExecMode[] pExecMode, DataGrid[] grid)
        {
            InitializeComponent();
            Translate();
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;

            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;

            mpe = pExecMode[0];
            if (pExecMode[0] == SessionObject.ExecMode.Modification)
            {

                ActionClick((ServiceAdministration.CsUtilisateur)_user[0], SessionObject.ExecMode.Modification, Shared.ClasseMEthodeGenerique.ConvertTypeSite(SessionObject.LstCentre), Shared.ClasseMEthodeGenerique.ConvertTypeCentre(SessionObject.LstCentre), SessionObject.ListeDesProfils);
                return;
            }

            if (pExecMode[0] == SessionObject.ExecMode.Consultation)
            {
                ActionClick((ServiceAdministration.CsUtilisateur)_user[0], SessionObject.ExecMode.Consultation, Shared.ClasseMEthodeGenerique.ConvertTypeSite(SessionObject.LstCentre), Shared.ClasseMEthodeGenerique.ConvertTypeCentre(SessionObject.LstCentre), SessionObject.ListeDesProfils);
                return;
            }
            if (pExecMode[0] == SessionObject.ExecMode.Creation)
            {
                rdbActive.IsChecked = true;
            }
            if (pExecMode[0] == SessionObject.ExecMode.Muter )
            {
                ActionClick((ServiceAdministration.CsUtilisateur)_user[0], SessionObject.ExecMode.Muter , Shared.ClasseMEthodeGenerique.ConvertTypeSite(SessionObject.LstCentre), Shared.ClasseMEthodeGenerique.ConvertTypeCentre(SessionObject.LstCentre), SessionObject.ListeDesProfils);
                return;
            }
            ChargerListDesSite();
            GetListeProfil();
            GetAllFonction();
            try
            {
                this.Btn_OK.IsEnabled = false;
                this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date ;

                if (!SessionObject.Enumere.IsUtilisateurCreeParAgent)
                    this.txtMatri_Agent.IsReadOnly = true;
                else
                    txtName.IsReadOnly = true;

                userSelected = new ServiceAdministration.CsUtilisateur();
                userSelected = _user[0] as ServiceAdministration.CsUtilisateur;
                _Action = pExecMode[0];
                gridView = grid[0];
                Tag = pExecMode[0];
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcGererUser(string operat, int un)
        {
            InitializeComponent();
            Translate();
            mpe = SessionObject.ExecMode.Creation ;
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;
            GetDataAll();
            this.Cbo_centre.IsEnabled = false;
            if (operat == "1" && un == 1) //nouveau
            {

                try
                {
                    this.Btn_OK.IsEnabled = false;
                    this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date ;
                    this.rdbActive.IsChecked = true;
                    if (!SessionObject.Enumere.IsUtilisateurCreeParAgent)
                    {
                        this.txtMatri_Agent.IsReadOnly = true;
                    }
                    else
                        txtName.IsReadOnly = true;
                }
                catch (Exception ex)
                {
                    Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                }
            }
        }

        private void ActionClick(Galatee.Silverlight.ServiceAdministration.CsUtilisateur _user, SessionObject.ExecMode pExecMode, List<CsSite> lstsite, List<CsCentre> lstcentre, List<CsProfil> lstprofil)
        {
            try
            {
                IsUpadate = true;
                GetAllFonction();
                GetDataGeneral(lstsite, lstcentre, lstprofil);
                mpe = pExecMode;
                if (pExecMode == SessionObject.ExecMode.Muter ) //mutation
                {
                    try
                    {
                        this.Btn_OK.IsEnabled = false;
                        this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date ;

                        if (!SessionObject.Enumere.IsUtilisateurCreeParAgent)
                        {
                            this.txtMatri_Agent.IsReadOnly = true;
                        }

                        if (_user.LOGINNAME != null)
                        {

                            ChargerDonneesUtilisateur(_user.LOGINNAME, pExecMode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                }
                if (pExecMode == SessionObject.ExecMode.Modification ) //modification
                {
                    try
                    {
                        this.Btn_OK.IsEnabled = false;
                        this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date ;

                        if (SessionObject.Enumere.IsUtilisateurCreeParAgent)
                        {
                            this.txtMatri_Agent.IsReadOnly = true;
                        }

                        if (_user.LOGINNAME != null)
                        {

                            ChargerDonneesUtilisateur(_user.LOGINNAME, pExecMode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                }
                if (pExecMode == SessionObject.ExecMode.Consultation ) //consultation
                {
                    try
                    {
                        this.Btn_OK.IsEnabled = false;
                        this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date ;

                        if (SessionObject.Enumere.IsUtilisateurCreeParAgent)
                            this.txtMatri_Agent.IsReadOnly = true;

                        if (_user.LOGINNAME != null)
                            ChargerDonneesUtilisateurConsult(_user.LOGINNAME, pExecMode);
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public UcGererUser(Galatee.Silverlight.ServiceAdministration.CsUtilisateur _user, SessionObject.ExecMode  pExecMode, List<CsProfil> lstprofil)
        {
            InitializeComponent();
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;
            GetListeProfil();
            Translate();

            mpe = pExecMode;
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

                    List<CsSite> lstsite = Shared.ClasseMEthodeGenerique.ConvertTypeSite(SessionObject.LstCentre);
                    List<CsCentre> lstcentre = Shared.ClasseMEthodeGenerique.ConvertTypeCentre(SessionObject.LstCentre);
                    ActionClick(_user, pExecMode, lstsite, lstcentre, ProfilListe);
                }
            };
            admClient.RetourneListeAllProfilUserAsync();

        }



        void chargerChampUtilisateur(SessionObject.ExecMode pExecMode)
        {
            if (userSelected != null )
            {
                //griser mot de passe
                this.txtPwd.IsEnabled = false;
                this.txtPdwConf.IsEnabled = false;

                //remplir les champs texte
                this.txtEmail.Text = (userSelected.E_MAIL == null) ? "" : userSelected.E_MAIL;
                this.txtLogin.Text = userSelected.LOGINNAME;
                this.txtMatri_Agent.Text = userSelected.MATRICULE;
                this.txtName.Text = userSelected.LIBELLE;
                this.txtTelephone.Text = (userSelected.TELEPHONE == null) ? "" : userSelected.TELEPHONE;
                this.dtpDate_creation.SelectedDate = (userSelected.DATECREATION == null) ? DateTime.Now : userSelected.DATECREATION;

                if (userSelected.FK_IDSTATUS == SessionObject.Enumere.UserAcitveAccount)
                {
                    rdbActive.IsChecked = true;
                    rdbInactive.IsChecked = false;
                    rdbLocked.IsChecked = false;
                }
                else
                {
                    if (userSelected.FK_IDSTATUS == SessionObject.Enumere.UserInAcitveAccount)
                    {
                        rdbActive.IsChecked = false;
                        rdbInactive.IsChecked = true;
                        rdbLocked.IsChecked = false;

                    }
                    else
                    {
                        if (userSelected.FK_IDSTATUS == SessionObject.Enumere.UserLockeAcitveAccount)
                        {
                            rdbActive.IsChecked = false;
                            rdbInactive.IsChecked = false;
                            rdbLocked.IsChecked = true;

                        }
                    }

                }


                //cbo_site.IsEnabled = true;
                if (pExecMode == SessionObject.ExecMode.Modification)
                {
                    Cbo_centre.IsEnabled = false;
                    cbo_site.IsEnabled = false;
                }
                CsCentre ctre = new CsCentre();
                ctre = CentreListe.Where(s => s.PK_ID == userSelected.FK_IDCENTRE).FirstOrDefault();

                CsSite ste = new CsSite();
                ste = SiteListe.Where(s => s.PK_ID == ctre.FK_IDCODESITE).FirstOrDefault();

                cbo_site.SelectedItem = ste;
                Cbo_centre.SelectedItem = ctre;


                //set init user profil 
                if (userSelected.LESPROFILSUTILISATEUR == null) return;
                    lesProfilDuUserInit.AddRange(userSelected.LESPROFILSUTILISATEUR);
                lesProfilDuUser = userSelected.LESPROFILSUTILISATEUR;
                dtgprofil.ItemsSource = lesProfilDuUser;

                ActiverOK(SessionObject.ExecMode.Modification);

            }
        }

        public UcGererUser(Galatee.Silverlight.ServiceAdministration.CsUtilisateur _user, SessionObject.ExecMode[] pExecMode)
        {
            InitializeComponent();
            txtMatri_Agent.MaxLength = SessionObject.Enumere.TailleMatricule;

            Translate();
            try
            {
                this.Btn_OK.IsEnabled = false;
                this.dtpDate_creation.SelectedDate = System.DateTime.Today.Date;
                this.rdbActive.IsChecked = true;
                if (!SessionObject.Enumere.IsUtilisateurCreeParAgent)

                    this.txtMatri_Agent.IsReadOnly = true;
                else
                    txtName.IsReadOnly = true;
                 
                GetData(pExecMode[1], _user);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        bool IsUpadate = false;

        private void ChargerDonneesUtilisateurConsult(string lOGINNAME, SessionObject.ExecMode pExecMode)
        {
            ChargerDonneesUtilisateur(lOGINNAME, pExecMode);
            GriserTousLesChamps();
            //griser les champs

        }

        void GriserTousLesChamps()
        {
            this.txtEmail.IsReadOnly  = true ;
            this.txtLogin.IsReadOnly = true;
            this.txtMatri_Agent.IsReadOnly = true;
            this.txtName.IsReadOnly = true;
            this.txtPdwConf.IsEnabled  = false;
            this.txtPwd.IsEnabled = false;
            this.txtTelephone.IsReadOnly = true;
            this.txtEmail.IsReadOnly = true;

            this.rdbActive.IsEnabled = false;
            this.rdbInactive.IsEnabled = false;
            this.rdbLocked.IsEnabled = false;

            this.Cbo_centre.IsEnabled = false;
            this.cbo_site.IsEnabled = false;
            this.ckcChangepwd.IsEnabled = false;

            this.dtpkFromValid.IsEnabled = false;
            this.dtpkToValid.IsEnabled = false;
            this.dtp_lastModif.IsEnabled = false;

            this.btnParametrerProfil.IsEnabled = false;
            this.dtgprofil.IsReadOnly  = true ;
            this.Btn_OK.IsEnabled = false;
            //this.Cbo_centre.IsEnabled = false;
        }


        public void ChargerDonneesUtilisateur(string loginuser, SessionObject.ExecMode  pExecMode)
        {
            Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur _utilisateur = new Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur();

            try
            {
                //client.GetNatureByLibelleCourtAsync(SessionObject.Enumere.LibNatureCheqImpaye);
                Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient getlogin = new Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
                getlogin.GetByLoginNameCompleted += (send, result) =>
                {
                    try
                    {
                        if (result.Cancelled || result.Error != null)
                        {
                            string error = result.Error.Message;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                            return;
                        }

                        if (result.Result == null)
                        {
                            return;
                        }

                        Galatee.Silverlight.ServiceAdministration.CsUtilisateur leUtilisateurCourant = new Galatee.Silverlight.ServiceAdministration.CsUtilisateur();
                        leUtilisateurCourant = ConvertionUtilisateurAdminSVC(result.Result);

                        userSelected = new ServiceAdministration.CsUtilisateur();
                        userSelected = leUtilisateurCourant;
                        if (userSelected != null)
                        {
                            chargerChampUtilisateur(pExecMode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                getlogin.GetByLoginNameAsync(loginuser);


            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public static ServiceAdministration.CsUtilisateur ConvertionUtilisateurAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsUtilisateur user)
        {
            ServiceAdministration.CsUtilisateur _user = new ServiceAdministration.CsUtilisateur();
            if (user != null)
            {
                try
                {
                    _user.USERCREATION = user.USERCREATION;
                    _user.PK_ID = user.PK_ID;
                    _user.BRANCHE = user.BRANCHE;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEDEBUTVALIDITE = user.DATEDEBUTVALIDITE;
                    _user.DATEDERNIERECONNEXION = DateTime.Now;
                    _user.DATEDERNIEREMODIFICATION = user.DATEDERNIEREMODIFICATION;
                    _user.DATEDERNIEREMODIFICATIONPASSWORD = user.DATEDERNIEREMODIFICATIONPASSWORD;
                    _user.DATEDERNIERVERROUILLAGE = user.DATEDERNIERVERROUILLAGE;
                    _user.DATEFINVALIDITE = user.DATEFINVALIDITE;
                    _user.DERNIERECONNEXIONREUSSIE = user.DERNIERECONNEXIONREUSSIE;
                    _user.LIBELLE = user.LIBELLE;
                    _user.ESTSUPPRIMER = user.ESTSUPPRIMER;
                    _user.E_MAIL = user.E_MAIL;
                    _user.EsADMIN = user.EsADMIN;
                    _user.CENTRE = user.CENTRE;
                    _user.FONCTION = user.FONCTION;
                    _user.INITUSERPASSWORD = user.INITUSERPASSWORD;
                    _user.LIBELLEFONCTION = user.LIBELLEFONCTION;
                    _user.LOGINNAME = user.LOGINNAME;
                    _user.NOMBREECHECSOUVERTURESESSION = user.NOMBREECHECSOUVERTURESESSION;
                    _user.PASSE = user.PASSE;
                    _user.PERIMETREACTION = user.PERIMETREACTION;
                    _user.MATRICULE = user.MATRICULE;
                    _user.PK_ID = user.PK_ID;
                    _user.LIBELLESTATUSCOMPTE = user.LIBELLESTATUSCOMPTE;
                    _user.FK_IDCENTRE = user.FK_IDCENTRE;
                    _user.FK_IDANCIENCENTRE  = user.FK_IDCENTRE;
                    _user.FK_IDFONCTION = user.FK_IDFONCTION;
                    _user.FK_IDSTATUS = user.FK_IDSTATUS;
                    _user.PK_ID = user.PK_ID;
                    _user.TELEPHONE = user.TELEPHONE;
                    if (user.LESPROFILSUTILISATEUR  != null)
                    {
                        foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsProfil item in user.LESPROFILSUTILISATEUR)
                        {
                            if (_user.LESPROFILSUTILISATEUR == null) _user.LESPROFILSUTILISATEUR = new List<CsProfil>();
                            _user.LESPROFILSUTILISATEUR.Add(ConvertionProfilAdminSVC(item));
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        public static ServiceAdministration.CsProfil ConvertionProfilAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsProfil user)
        {
            ServiceAdministration.CsProfil _user = new ServiceAdministration.CsProfil();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.CODE = user.CODE;
                    _user.CODEFONCTION = user.CODEFONCTION;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEMODIFICATION = user.DATEMODIFICATION;
                    _user.FK_IDFONCTION = user.FK_IDFONCTION;
                    _user.LIBELLE = user.LIBELLE;
                    _user.MODULE = user.MODULE;
                    _user.USERCREATION = user.USERCREATION;
                    _user.USERMODIFICATION = user.USERMODIFICATION;
                    _user.FK_IDPROFIL = user.FK_IDPROFIL;
                    if (user.LESCENTRESPROFIL != null && user.LESCENTRESPROFIL.Count > 0)
                    {
                        foreach (Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil item in user.LESCENTRESPROFIL)
                        {
                            if (_user.LESCENTRESPROFIL == null)
                                _user.LESCENTRESPROFIL = new List<ServiceAdministration.CsCentreProfil>();
                            _user.LESCENTRESPROFIL.Add(ConvertionCentreProfilAdminSVC(item));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        public static ServiceAdministration.CsCentreProfil ConvertionCentreProfilAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsCentreProfil user)
        {
            ServiceAdministration.CsCentreProfil _user = new ServiceAdministration.CsCentreProfil();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.DATEDEBUTVALIDITE = user.DATEDEBUTVALIDITE;
                    _user.DATEFINVALIDITE = user.DATEFINVALIDITE;
                    _user.FK_IDADMUTILISATEUR = user.FK_IDADMUTILISATEUR;
                    _user.FK_IDCENTRE = user.FK_IDCENTRE;
                    _user.FK_IDPROFIL = user.FK_IDPROFIL;
                }

                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return _user;
        }

        public static ServiceAdministration.CsCentre ConvertionCentreAdminSVC(Galatee.Silverlight.ServiceAuthenInitialize.CsCentre user)
        {
            ServiceAdministration.CsCentre _user = new ServiceAdministration.CsCentre();

            if (user != null)
            {
                try
                {
                    _user.PK_ID = user.PK_ID;
                    _user.ADRESSE = user.ADRESSE;
                    _user.CODE = user.CODE;
                    _user.CODESITE = user.CODESITE;
                    _user.DATECREATION = user.DATECREATION;
                    _user.DATEMODIFICATION = user.DATEMODIFICATION;
                    _user.FK_IDCODESITE = user.FK_IDCODESITE;
                    _user.FK_IDNIVEAUTARIF = user.FK_IDNIVEAUTARIF;
                    _user.FK_IDTYPECENTRE = user.FK_IDTYPECENTRE;
                    _user.GESTIONAUTOAVANCECONSO = user.GESTIONAUTOAVANCECONSO;
                    _user.GESTIONAUTOFRAIS = user.GESTIONAUTOFRAIS;
                    //_user.LESPRODUITSDUSITE = user.LESPRODUITSDUSITE;
                    //_user.GESTIONAUTOAVANCECONSO = user.GESTIONAUTOAVANCECONSO;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return _user;
        }

        private CsAgent RetourneAgentRech(String agentRechMat)
        {
            CsAgent trouveAgent = new CsAgent();

            if (SessionObject.ListeDesAgents != null)
            {

                foreach (CsAgent ag in SessionObject.ListeDesAgents)
                {
                    if (ag.MATRICULE == agentRechMat)
                    {
                        trouveAgent = ag;
                    }
                }
            }


            return trouveAgent;

        }

        void GetDataGeneral(List<CsSite> lstsite, List<CsCentre> lstcentre, List<CsProfil> lstprofil)
        {
            try
            {
                // obtenir la liste des sites
                CentreListe.AddRange(lstcentre);

                SiteListe.AddRange(lstsite);

                // obtenir la liste des centres
                //List<CsCentre> ListeCentre = new List<CsCentre>();
                //ListeCentre.Add(new CsCentre { CODE = " ", LIBELLE = " " });
                //ListeCentre.AddRange(CentreListe);

                Cbo_centre.ItemsSource = CentreListe;
                Cbo_centre.SelectedValuePath = "CODE";
                Cbo_centre.DisplayMemberPath = "LIBELLE";

                //cbo_site.ItemsSource = argbrch.Result.Where(p => p.CODESITE != "000").ToList();

                //List<CsSite> ListeSite = new List<CsSite>();
                //ListeSite.Add(new CsSite { CODE = "  ", LIBELLE = " " });
                //ListeSite.AddRange(SiteListe);

                cbo_site.ItemsSource = SiteListe;
                cbo_site.SelectedValuePath = "CODE";
                cbo_site.DisplayMemberPath = "LIBELLE";

                List<CsProfil> lalist = new List<CsProfil>();
                lalist.AddRange(lstprofil);
                ProfilListe = lalist;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void GetDataPlus(SessionObject.ExecMode sens, ServiceAdministration.CsUtilisateur user)
        {
            GetAllFonction();
            try
            {

                if (sens == SessionObject.ExecMode.Creation)
                {
                    this.rdbActive.IsChecked = true;
                    this.rdbInactive.IsEnabled = false;
                    this.rdbLocked.IsEnabled = false;
                    this.txtMatri_Agent.Focus();
                    //this.rdbCentre.IsChecked = true;
                }

                IsUpadate = true;
                try
                {

                    lesProfilDuUser = user.LESPROFILSUTILISATEUR;
                    // obtenir la liste des centres
                    List<CsCentre> ListeCentre = new List<CsCentre>();
                    //ListeCentre.Add(new CsCentre {CODE="  ",LIBELLE=" " });
                    ListeCentre.AddRange( SessionObject.ListeDesCentreAdm);
                    Cbo_centre.ItemsSource = ListeCentre;
                    Cbo_centre.SelectedValuePath = "CODE";
                    Cbo_centre.DisplayMemberPath = "LIBELLE";

                    //cbo_site.ItemsSource = argbrch.Result.Where(p => p.CODESITE != "000").ToList();

                    List<CsSite> ListeSite = new List<CsSite>();
                    //ListeSite.Add(new CsSite { CODE = " ", LIBELLE = " " });
                    ListeSite.AddRange( SessionObject.ListeDesSitesAdm);
                    cbo_site.ItemsSource = ListeSite;
                    cbo_site.SelectedValuePath = "CODE";
                    cbo_site.DisplayMemberPath = "LIBELLE";

                }
                catch (Exception ex)
                {
                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void GetData(SessionObject.ExecMode sens, ServiceAdministration.CsUtilisateur user)
        {
            GetAllFonction();
            try
            {

                if (sens == SessionObject.ExecMode.Creation)
                {
                    this.rdbActive.IsChecked = true;
                    this.rdbInactive.IsEnabled = false;
                    this.rdbLocked.IsEnabled = false;
                    this.txtMatri_Agent.Focus();
                    //this.rdbCentre.IsChecked = true;
                }
                if (sens == SessionObject.ExecMode.Modification)
                {
                    this.rdbActive.IsEnabled = true;
                    this.rdbInactive.IsEnabled = true;
                    this.rdbLocked.IsEnabled = true;
                    //this.rdbCentre.IsChecked = true;
                }
                //if (sens == SessionObject.ExecMode.Modification)
                IsUpadate = true;
                AdministrationServiceClient admClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                //admClient.SELECT_All_FonctionCompleted += (sen, result) =>
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

                        AdministrationServiceClient brchClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                        brchClient.GetAllSiteCompleted += (sbrch, argbrch) =>
                        {
                            if (argbrch.Cancelled || argbrch.Error != null)
                            {
                                string error = argbrch.Error.Message;
                                Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "GetAllBranche");
                                return;
                            }
                            AdministrationServiceClient SiteClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                            SiteClient.RetourneListeDesCentreCompleted += (scentre, rcentre) =>
                            {
                                try
                                {
                                    if (rcentre.Cancelled || rcentre.Error != null)
                                    {
                                        string error = rcentre.Error.Message;
                                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "RetourneListeDesCentre");
                                        return;
                                    }

                                    // obtenir la liste des sites
                                    CentreListe.AddRange(rcentre.Result);
                                    lesProfilDuUser = user.LESPROFILSUTILISATEUR;
                                    SiteListe.AddRange(argbrch.Result);
                                    //

                                    // obtenir la liste des centres
                                    List<CsCentre> ListeCentre = new List<CsCentre>();
                                    ListeCentre.Add(new CsCentre {CODE=" ", LIBELLE=" " });
                                    ListeCentre.AddRange(rcentre.Result);
                                    Cbo_centre.ItemsSource = ListeCentre;
                                    Cbo_centre.SelectedValuePath = "CODE";
                                    Cbo_centre.DisplayMemberPath = "LIBELLE";
                                    List<CsSite> ListeSite = new List<CsSite>();
                                    ListeSite.Add(new CsSite{CODE=" ", LIBELLE=" "} );
                                    ListeSite.AddRange(argbrch.Result);
                                    cbo_site.ItemsSource = ListeSite;
                                    //cbo_site.ItemsSource = argbrch.Result ;
                                    cbo_site.SelectedValuePath = "CODE";
                                    cbo_site.DisplayMemberPath = "LIBELLE";

                                    List<CsProfil> lalist = new List<CsProfil>();
                                    lalist.AddRange(result.Result);
                                    ProfilListe = lalist;

                                    if (sens == SessionObject.ExecMode.Consultation || sens == SessionObject.ExecMode.Modification)
                                    {
                                        RecupererInfoUser(user, SiteListe, CentreListe, lesProfilDuUser);
                                        txtMatri_Agent.IsReadOnly = false;
                                        txtLogin.IsReadOnly = false;
                                        if (sens == SessionObject.ExecMode.Consultation)
                                        {
                                            Btn_Cancel.Content = Btn_OK.Content;
                                            Btn_OK.Visibility = Visibility.Collapsed;
                                            desactiverControles(false);
                                        }
                                        txtPdwConf.IsEnabled = txtPwd.IsEnabled = false;

                                        if (sens == SessionObject.ExecMode.Modification)
                                        {
                                            Cbo_centre.IsEnabled = false;
                                            cbo_site.IsEnabled = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                                }
                            };
                            SiteClient.RetourneListeDesCentreAsync();
                        };
                        brchClient.GetAllSiteAsync();
                    }
                };
                admClient.RetourneListeAllProfilUserAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void GetDataAll()
        {
            GetAllFonction();
            
            try
            {

                //IsUpadate = true;
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

                        AdministrationServiceClient brchClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                        brchClient.GetAllSiteCompleted += (sbrch, argbrch) =>
                        {
                            if (argbrch.Cancelled || argbrch.Error != null)
                            {
                                string error = argbrch.Error.Message;
                                Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "GetAllBranche");
                                return;
                            }
                            AdministrationServiceClient SiteClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                            SiteClient.RetourneListeDesCentreCompleted += (scentre, rcentre) =>
                            {
                                try
                                {
                                    if (rcentre.Cancelled || rcentre.Error != null)
                                    {
                                        string error = rcentre.Error.Message;
                                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, "RetourneListeDesCentre");
                                        return;
                                    }

                                    // obtenir la liste des sites
                                    CentreListe.AddRange(rcentre.Result);

                                    SiteListe.AddRange(argbrch.Result);

                                    // obtenir la liste des centres
                                    //List<CsCentre> ListeCentre = new List<CsCentre>();
                                    //ListeCentre.Add(new CsCentre { CODE = "  ", LIBELLE = " " });
                                    //ListeCentre.AddRange(rcentre.Result);

                                    Cbo_centre.ItemsSource = CentreListe;
                                    Cbo_centre.SelectedValuePath = "CODE";
                                    Cbo_centre.DisplayMemberPath = "LIBELLE";

                                    //List<CsSite> ListeSite = new List<CsSite>();
                                    //ListeSite.Add(new CsSite {CODE=" ",LIBELLE=" "  });
                                    //ListeSite.AddRange(argbrch.Result);

                                    cbo_site.ItemsSource = SiteListe;
                                    cbo_site.SelectedValuePath = "CODE";
                                    cbo_site.DisplayMemberPath = "LIBELLE";

                                    List<CsProfil> lalist = new List<CsProfil>();

                                    lalist.AddRange(result.Result);
                                    // obtenir la liste
                                    ProfilListe = lalist;
                                }
                                catch (Exception ex)
                                {
                                    Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                                }
                            };
                            SiteClient.RetourneListeDesCentreAsync();
                        };
                        brchClient.GetAllSiteAsync();
                    }
                };
                admClient.RetourneListeAllProfilUserAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void Translate()
        {
            try
            {
                lbl_branch.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_branche;
                lbl_çreation_date.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_date_creation;
                lbl_last_modif_date.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_date_dernier_modif;
                lbl_login.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_login;
                lbl_Centre.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_centre;
                lbl_pwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_password;
                lbl_pwd_conf.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_password_conf;
                lbl_role.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_profil;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void RecupererInfoUser(ServiceAdministration.CsUtilisateur user, List<CsSite> branche, List<CsCentre> centre, List<CsProfil> profils)
        {

            try
            {
                //txtID.Text = string.IsNullOrEmpty(user.MATRICULE) ? string.Empty : user.MATRICULE;
                txtLogin.Text = string.IsNullOrEmpty(user.LOGINNAME) ? string.Empty : user.LOGINNAME;
                txtName.Text = string.IsNullOrEmpty(user.LIBELLE) ? string.Empty : user.LIBELLE;
                txtEmail.Text = string.IsNullOrEmpty(user.E_MAIL) ? string.Empty : user.E_MAIL;
                txtMatri_Agent.Text = string.IsNullOrEmpty(user.MATRICULE) ? string.Empty : user.MATRICULE;

                ServiceAdministration.CsUtilisateur users = SessionObject.objectSelected as ServiceAdministration.CsUtilisateur;

                ckcChangepwd.IsChecked = users.INITUSERPASSWORD == null ? false : users.INITUSERPASSWORD;
                CsCentre _centre = CentreListe.First(p => p.PK_ID == users.FK_IDCENTRE);
                Cbo_centre.SelectedItem = _centre;

                cbo_site.SelectedItem = SiteListe.FirstOrDefault(p => p.PK_ID == _centre.FK_IDCODESITE);

                rdbActive.IsChecked = users.FK_IDSTATUS == SessionObject.Enumere.UserAcitveAccount;
                rdbInactive.IsChecked = users.FK_IDSTATUS == SessionObject.Enumere.UserInAcitveAccount;
                rdbLocked.IsChecked = users.FK_IDSTATUS == SessionObject.Enumere.UserLockeAcitveAccount || users.FK_IDSTATUS == SessionObject.Enumere.UserLockeSessionAccount || users.FK_IDSTATUS > 3;

                dtpDate_creation.Text = users.DATECREATION == null ? string.Empty : users.DATECREATION.ToString();
                dtp_lastModif.Text = users.DATEDERNIEREMODIFICATION == null ? string.Empty : users.DATEDERNIEREMODIFICATION.ToString();
                dtpkFromValid.Text = users.DATEDEBUTVALIDITE == null ? string.Empty : users.DATEDEBUTVALIDITE.ToString();
                dtpkToValid.Text = users.DATEFINVALIDITE == null ? string.Empty : users.DATEFINVALIDITE.ToString();
                dtp_lastModif.IsEnabled = dtpDate_creation.IsEnabled = false;

                IsUpadate = false;

                dtgprofil.ItemsSource = profils;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void desactiverControles(bool etat)
        {
            AllInOne.ActivateControlsFromXaml(LayoutRoot, etat, typeof(Button));
        }

        void GererUtilisateur(SessionObject.ExecMode sens, ServiceAdministration.CsUtilisateur pUserSelected)
        {
            ServiceAdministration.CsUtilisateur user = new ServiceAdministration.CsUtilisateur();

            try
            {
                user.E_MAIL = txtEmail.Text;
                user.TELEPHONE = txtTelephone.Text;
                user.MATRICULE = txtMatri_Agent.Text;
                user.LIBELLE = txtName.Text;
                user.CENTRE = (Cbo_centre.SelectedItem as CsCentre).CODE;
                user.FK_IDCENTRE = (Cbo_centre.SelectedItem as CsCentre).PK_ID;
                user.FK_IDANCIENCENTRE  = pUserSelected.FK_IDCENTRE;
                user.INITUSERPASSWORD = (ckcChangepwd.IsChecked == true) ? true : false;

                if (sens == SessionObject.ExecMode.Creation)
                    Securities.CheckConfirmPassword(txtPwd.Password, txtPdwConf.Password);

                if (rdbActive.IsChecked == true)
                {
                    user.FK_IDSTATUS = SessionObject.Enumere.UserAcitveAccount;
                    user.LIBELLESTATUSCOMPTE = Galatee.Silverlight.Resources.Administration.Langue.RdbuttonActive;
                    user.NOMBREECHECSOUVERTURESESSION = 0;
                }

                else if (rdbInactive.IsChecked == true)
                {
                    user.FK_IDSTATUS = SessionObject.Enumere.UserInAcitveAccount;
                    user.LIBELLESTATUSCOMPTE = Galatee.Silverlight.Resources.Administration.Langue.RdbuttonInActive;
                }
                else if (rdbLocked.IsChecked == true)
                {
                    user.FK_IDSTATUS = SessionObject.Enumere.UserLockeAcitveAccount;
                    user.LIBELLESTATUSCOMPTE = Galatee.Silverlight.Resources.Administration.Langue.RdbuttonLocked;
                }
                user.DATEDEBUTVALIDITE = string.IsNullOrEmpty(dtpkFromValid.Text) ? null : dtpkFromValid.SelectedDate;
                user.DATEFINVALIDITE = string.IsNullOrEmpty(dtpkToValid.Text) ? null : dtpkToValid.SelectedDate;

                user.LOGINNAME = txtLogin.Text;

                user.LESPROFILSUTILISATEUR = new List<CsProfil>();
                user.LESPROFILSUTILISATEUR = (List<CsProfil>)dtgprofil.ItemsSource;
                //if (leUserSelect != null && !string.IsNullOrEmpty(leUserSelect.NOM))
                //    user.PERIMETREACTION = leUserSelect.PERIMETREACTION;
                if (sens == SessionObject.ExecMode.Creation)
                {
                    user.USERCREATION = UserConnecte.matricule;
                    user.DATECREATION = DateTime.Now;
                    user.ESTSUPPRIMER = false;
                    user.DATEDERNIEREMODIFICATION = DateTime.Now; //user.DateCreation;
                    Security.CsStrategieSecurite security = new Security.CsStrategieSecurite();
                    security = Utility.ParseObject<Security.CsStrategieSecurite, ServiceAuthenInitialize.CsStrategieSecurite>(security, SessionObject.securiteActive);
                    user.PASSE = Cryptage.GetPasswordToBeSaved(security, txtLogin.Text, txtPwd.Password);
                    user.DATECREATION = DateTime.Now;
                    MiseAJourInsertion (user);
                }
                else if (sens == SessionObject.ExecMode.Modification || sens == SessionObject.ExecMode.Muter)
                {
                    Security.CsStrategieSecurite security = new Security.CsStrategieSecurite();
                    this.cbo_site.IsEnabled = false;
                    this.Cbo_centre.IsEnabled = false;
                    user.DATECREATION = pUserSelected.DATECREATION;// string.IsNullOrEmpty(dtpDate_creation.Text) ? null : dtpDate_creation.SelectedDate;
                    user.DATEDERNIEREMODIFICATION = string.IsNullOrEmpty(dtp_lastModif.Text) ? null : dtp_lastModif.SelectedDate;
                    user.PASSE = pUserSelected.PASSE;
                    //user.PASSE = Cryptage.GetPasswordToBeSaved(security, txtLogin.Text, txtPwd.Password);
                    user.ESTSUPPRIMER = pUserSelected.ESTSUPPRIMER;
                    user.USERCREATION = pUserSelected.USERCREATION;
                    user.DATECREATION = pUserSelected.DATECREATION;
                    user.PK_ID = pUserSelected.PK_ID;
                     
                    if (!Utility.HasSamePropertyValue(user, ParseValueToModification(pUserSelected))) // permet de savoir si un composant a été modifié sur l'IHM en vu
                    {                                                                                 // de déclencher le processus de modification
                        user.DATEDERNIEREMODIFICATION = DateTime.Now;

                        if (user.INITUSERPASSWORD.Value)
                            user.DATEDERNIEREMODIFICATIONPASSWORD = DateTime.Now;
                        else
                            user.DATEDERNIEREMODIFICATIONPASSWORD = pUserSelected.DATEDERNIEREMODIFICATIONPASSWORD;

                        if (pUserSelected.FK_IDSTATUS != user.FK_IDSTATUS) // permet de savoir si le compte etait verrouillé par un échec de connexion et vient d'etre réactive
                            if ((pUserSelected.FK_IDSTATUS >= SessionObject.Enumere.UserLockeAcitveAccount) && user.FK_IDSTATUS == SessionObject.Enumere.UserAcitveAccount && user.INITUSERPASSWORD.Value == true)
                            {
                                user.FK_IDSTATUS = SessionObject.Enumere.UserLockeAcitveAccount;
                            }
                        user.USERCREATION = UserConnecte.matricule;
                        user.DATECREATION = DateTime.Now;
                        //user.PASSE = pUserSelected.PASSE;
                        UpdateUtilisateur (user, lesProfilDuUserInit);
                    }
                    else
                        Message.ShowError(Galatee.Silverlight.Resources.Langue.AucuneModificationIhm, Galatee.Silverlight.Resources.Langue.informationTitle);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        private ServiceAdministration.CsUtilisateur ParseValueToModification(ServiceAdministration.CsUtilisateur pUserSelected)
        {
            try
            {
                ServiceAdministration.CsUtilisateur _user = new ServiceAdministration.CsUtilisateur();
                _user.MATRICULE = pUserSelected.MATRICULE;
                _user.LIBELLE = pUserSelected.LIBELLE;
                _user.CENTRE = pUserSelected.CENTRE;
                _user.INITUSERPASSWORD = pUserSelected.INITUSERPASSWORD;
                _user.FK_IDSTATUS = pUserSelected.FK_IDSTATUS;
                _user.PERIMETREACTION = pUserSelected.PERIMETREACTION;
                _user.LIBELLESTATUSCOMPTE = pUserSelected.LIBELLESTATUSCOMPTE;

                _user.DATEDEBUTVALIDITE = pUserSelected.DATEDEBUTVALIDITE;
                _user.DATEFINVALIDITE = pUserSelected.DATEFINVALIDITE;
                _user.FONCTION = pUserSelected.FONCTION;

                _user.BRANCHE = pUserSelected.BRANCHE;
                _user.LIBELLEFONCTION = pUserSelected.LIBELLEFONCTION;
                _user.LOGINNAME = pUserSelected.LOGINNAME;
                _user.DATECREATION = pUserSelected.DATECREATION;
                _user.DATEDERNIEREMODIFICATION = pUserSelected.DATEDERNIEREMODIFICATION;
                _user.PASSE = pUserSelected.PASSE;
                _user.USERCREATION = pUserSelected.USERCREATION;
                return _user;
            }
            catch (Exception ex)
            {
                return new ServiceAdministration.CsUtilisateur();
            }

        }


        void UpdateUtilisateur(ServiceAdministration.CsUtilisateur user, List<CsProfil> anciensProfil)
        {
            try
            {
                AdministrationServiceClient majclient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                majclient.InsertUpdateUserCompleted += (majs, resultmaj) =>
                {
                    if (resultmaj.Cancelled || resultmaj.Error != null)
                    {
                        string error = resultmaj.Error.Message;
                        Message.Show(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }
                    else
                    {
                        this.Btn_OK.IsEnabled = true;
                        this.DialogResult = true;
                    }
                };
                majclient.InsertUpdateUserAsync(user, anciensProfil);
            }
            catch (Exception ex)
            {
                this.Btn_OK.IsEnabled = true;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void MiseAJourInsertion(ServiceAdministration.CsUtilisateur user)
        {
            try
            {
                AdministrationServiceClient insertUser = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                insertUser.saveUserProfilCentreCompleted += (inserS, resultIns) =>
                {
                    if (resultIns.Cancelled || resultIns.Error != null)
                    {
                        string error = resultIns.Error.Message;
                        Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorInsertUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (resultIns.Result == null || resultIns.Result.Count == 0)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorInsertUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    else
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.insertSuccess, Galatee.Silverlight.Resources.Langue.ConfirmationTitle);

                        //List<ServiceAdministration.CsUtilisateur> lstusers = resultIns.Result;

                        //InitialiserDonneeDataGrid(lstusers, gridView);

                        //InitialiserDonneeDataGrid(resultIns.Result, gridView);
                        this.Btn_OK.IsEnabled = true;
                        this.DialogResult = true;
                    }

                };
                insertUser.saveUserProfilCentreAsync(user); //, profils);
            }
            catch (Exception ex)
            {
                this.Btn_OK.IsEnabled = true;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void VerifieUserExiste(string Matricule)
        {
            try
            {
                AdministrationServiceClient insertUser = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                insertUser.VerifieUserExistCompleted += (inserS, resultIns) =>
                {
                    if (resultIns.Cancelled || resultIns.Error != null)
                    {
                        string error = resultIns.Error.Message;
                        Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorInsertUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    if (resultIns.Result == null )
                        GererUtilisateur(SessionObject.ExecMode.Creation, userSelected); 
                    else
                    {
                        userSelected.PK_ID = resultIns.Result.PK_ID;
                        userSelected.ESTSUPPRIMER = false;
                        GererUtilisateur(SessionObject.ExecMode.Modification, userSelected);
                    }

                };
                insertUser.VerifieUserExistAsync(Matricule); //, profils);
            }
            catch (Exception ex)
            {
                this.Btn_OK.IsEnabled = true;
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }


        void InitialiserDonneeDataGrid(List<ServiceAdministration.CsUtilisateur> users, DataGrid usergrid)
        {
            try
            {
                foreach (ServiceAdministration.CsUtilisateur item in users)
                {
                    if (item.PERIMETREACTION == 1)
                        item.LIBELLEPERIMETREACTION = "Agence";
                    else if (item.PERIMETREACTION == 2)
                        item.LIBELLEPERIMETREACTION = "Banque";
                    else if (item.PERIMETREACTION == 3)
                        item.LIBELLEPERIMETREACTION = "Globale";

                    item.CENTREAFFICHER = item.CENTRE + "  " + item.LIBELLECENTRE;
                }
                usergrid.ItemsSource = null;
                usergrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dtpkFromValid.SelectedDate>dtpkToValid.SelectedDate)
                {
                    Message.ShowError("La date de départ doit être inférieur à la date de fin", "");
                    return;
                }
                this.Btn_OK.IsEnabled = false;

                if (!IsUpadate)
                {
                     if (SessionObject.ListeDesUtilisateurs != null && SessionObject.ListeDesUtilisateurs.Count != 0)
                        {
                            if (SessionObject.ListeDesUtilisateurs.FirstOrDefault(t => t.MATRICULE == txtMatri_Agent.Text) != null)
                            {
                                Message.ShowError(Galatee.Silverlight.Resources.Administration.Langue.msgMatriculeExist, Galatee.Silverlight.Resources.Administration.Langue.libModule);
                                this.Btn_OK.IsEnabled = true ;
                                return;
                            }
                            if (SessionObject.ListeDesUtilisateurs.FirstOrDefault(t => t.LOGINNAME == txtLogin.Text) != null)
                            {
                                Message.ShowError(Galatee.Silverlight.Resources.Administration.Langue.msgloginExist, Galatee.Silverlight.Resources.Administration.Langue.libModule);
                                this.Btn_OK.IsEnabled = true;
                                return;
                            }
                        }
                     VerifieUserExiste(userSelected.MATRICULE);
                    //GererUtilisateur(SessionObject.ExecMode.Creation, userSelected);
                }
                if (IsUpadate)
                {
                    var w = new MessageBoxControl.MessageBoxChildWindow(Galatee.Silverlight.Resources.Langue.ConfirmationTitle, "Voulez-vous continuer la modification de cet utilisateur", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                    w.OnMessageBoxClosed += (_, result) =>
                    {
                        if (w.Result == MessageBoxResult.OK)
                            GererUtilisateur(SessionObject.ExecMode.Modification, userSelected);
                    };
                    w.Show();
                }
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }

        }

        private void RechAgentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SessionObject.ListeDesAgents != null)
                {
                    if (this.txtMatri_Agent.Text == "")
                    {
                        Message.Show(" Veuillez saisir le matricule de l'Agent! ", Galatee.Silverlight.Resources.Langue.Error_Title);
                    }
                    else
                    {
                        CsAgent leAgentSelect = RetourneAgentRech(this.txtMatri_Agent.Text);
                        if (leAgentSelect != null)
                        {
                            if (leAgentSelect.MATRICULE != null && leAgentSelect.MATRICULE != "" && leAgentSelect.NOM != null && leAgentSelect.NOM != "")
                            {
                                {
                                    this.txtName.Text = leAgentSelect.NOM + " " + leAgentSelect.PRENOM ;
                                    this.txtMatri_Agent.Text = leAgentSelect.MATRICULE;
                                }
                            }
                            else 
                            {
                                Message.Show(" Les informations du matricule " + this.txtMatri_Agent.Text + " sont introuvables !", Galatee.Silverlight.Resources.Langue.Error_Title);
                            }
                        }
                        else
                        {
                            Message.Show(" L'Agent de matricule " + this.txtMatri_Agent.Text + " est introuvable !", Galatee.Silverlight.Resources.Langue.Error_Title);
                        }
                    }
                }
                else
                    Message.Show(" Veuillez fermer et recharger la page ...", Galatee.Silverlight.Resources.Langue.Error_Title);


            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        private void txtMatri_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ActiverOK(SessionObject.ExecMode sens)
        {
            try
            {
                if (sens == SessionObject.ExecMode.Creation)
                {
                    if (!string.IsNullOrEmpty(this.txtLogin.Text) &&
                       !string.IsNullOrEmpty(this.txtName.Text) &&
                       !string.IsNullOrEmpty(this.txtPwd.Password) &&
                       !string.IsNullOrEmpty(this.txtPdwConf.Password) &&
                       (this.Cbo_centre.SelectedValue != null || !string.IsNullOrEmpty(this.Cbo_centre.SelectedItem.ToString())) &&
                       !string.IsNullOrEmpty(this.txtMatri_Agent.Text) &&
                       !string.IsNullOrEmpty(this.txtName.Text) &&
                       IsEMailAddress(this.txtEmail.Text))
                        this.Btn_OK.IsEnabled = true;
                }
                else if (sens == SessionObject.ExecMode.Modification)
                {
                    if (!string.IsNullOrEmpty(this.txtLogin.Text) &&
                        !string.IsNullOrEmpty(this.txtName.Text) &&
                        //(this.Cbo_centre.SelectedValue != null) &&
                        !string.IsNullOrEmpty(this.txtMatri_Agent.Text) &&
                        //!string.IsNullOrEmpty(this.txtName.Text) &&
                        IsEMailAddress(this.txtEmail.Text) &&
                        !string.IsNullOrEmpty(this.txtTelephone.Text))
                        this.Btn_OK.IsEnabled = true;
                    else
                    {
                        //if (!(lesProfilSelect.Count > 0))
                        //{
                        //    Message.ShowError("Veuillez paramétrer au moins un profil . ", Galatee.Silverlight.Resources.Langue.errorTitle);

                        //}
                        //if (!(IsEMailAddress(this.txtEmail.Text)))
                        //{
                        //    Message.ShowError("Veuillez renseigner correctement l'adresse email .", Galatee.Silverlight.Resources.Langue.errorTitle);

                        //}
                        this.Btn_OK.IsEnabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            if (IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Modification);
            }
            //ActiverOK(_Action);
            //
        }

        private void Cbo_centre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ActiverOK(_Action);

            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            //if (IsUpadate)
            //{
            //    ActiverOK(SessionObject.ExecMode.Modification);
            //}

        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ActiverOK(_Action);

            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            if (IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Modification);
            }

        }

        private void txtTelephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ActiverOK(_Action);

            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            if (IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Modification);
            }

        }

        private void txtPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //ActiverOK(_Action);

            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            //if (IsUpadate)
            //{
            //    ActiverOK(SessionObject.ExecMode.Modification);
            //}

        }

        private void txtPdwConf_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //ActiverOK(_Action);
            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            //if (IsUpadate)
            //{
            //    ActiverOK(SessionObject.ExecMode.Modification);
            //}

        }

        private void cbo_branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ////ActiverOK(_Action);
                //if (IsUpadate)
                //    return;

                //if (!IsUpadate)
                //{
                //    ActiverOK(SessionObject.ExecMode.Creation);
                //}
                ////if (IsUpadate)
                ////{
                ////    ActiverOK(SessionObject.ExecMode.Modification);
                ////}

                if (cbo_site.SelectedItem != null)
                {
                    if (mpe == SessionObject.ExecMode.Creation )
                         Cbo_centre.IsEnabled = true;

                    CsSite site = cbo_site.SelectedItem as CsSite;
                    List<CsCentre> _centres = Cbo_centre.ItemsSource as List<CsCentre>;
                    _centres = null;
                    _centres = CentreListe.Where(c => c.FK_IDCODESITE == site.PK_ID).ToList();
                    if (_centres != null && _centres.Count > 0)
                    {
                        Cbo_centre.ItemsSource = _centres;
                        Cbo_centre.SelectedItem = _centres.First();
                    }
                }
                else
                    Cbo_centre.SelectedItem = null;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ActiverOK(_Action);
            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            if (IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Modification);
            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ActiverOK(_Action);

            if (!IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Creation);
            }
            if (IsUpadate)
            {
                ActiverOK(SessionObject.ExecMode.Modification);
            }
        }

        #region chnage new

        List<CsFonction> DonnnesProfils = new List<CsFonction>();
        void GetAllFonction()
        {
            try
            {
                if (SessionObject.ListeFonction != null && SessionObject.ListeFonction.Count != 0)
                    DonnnesProfils = SessionObject.ListeFonction;
                else
                {
                    AdministrationServiceClient client = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                    client.SELECT_All_FonctionCompleted += (ss, res) =>
                    {
                        try
                        {
                            if (res.Cancelled || res.Error != null)
                            {
                                string error = res.Error.Message;
                                return;
                            }

                            if (res.Result == null || res.Result.Count == 0)
                            {
                                return;
                            }
                            SessionObject.ListeFonction = res.Result;
                            DonnnesProfils.AddRange(res.Result);
                        }
                        catch (Exception ex)
                        {
                        }

                    };
                    client.SELECT_All_FonctionAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        Galatee.Silverlight.ServiceAdministration.CsUtilisateur leUserSelect = new ServiceAdministration.CsUtilisateur();
        private void FrmUserProfilParam_Closed(object sender, EventArgs e)
        {
            try
            {
                this.IsEnabled = true ;
                FrmUserProfilParam ctrs = sender as FrmUserProfilParam;
                if (ctrs.isOkClick)
                {
                    leUserSelect = (Galatee.Silverlight.ServiceAdministration.CsUtilisateur)ctrs.userselect;
                    dtgprofil.ItemsSource = leUserSelect.LESPROFILSUTILISATEUR;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Btn_Param_Profil(object sender, RoutedEventArgs e)
        {
        

            CsSite pSite = (cbo_site.SelectedItem as CsSite);
            CsCentre pCentre = (Cbo_centre.SelectedItem as CsCentre);

            if (pSite != null && pSite.PK_ID.ToString() != null)
            {
                if (pCentre != null && pCentre.CODE != null)
                {
                    try
                    {
                        if (userSelected == null)
                        {
                            userSelected = new ServiceAdministration.CsUtilisateur();
                            userSelected.E_MAIL = string.IsNullOrEmpty( txtEmail.Text) ? string.Empty : txtEmail.Text ;
                            userSelected.TELEPHONE = string.IsNullOrEmpty(txtTelephone.Text) ? string.Empty : txtTelephone.Text;
                            userSelected.MATRICULE = string.IsNullOrEmpty(txtMatri_Agent.Text) ? string.Empty : txtMatri_Agent.Text;
                            userSelected.LIBELLE = string.IsNullOrEmpty(txtName.Text) ? string.Empty : txtName.Text;
                            userSelected.CENTRE = (Cbo_centre.SelectedItem as CsCentre).CODE;
                        }
                        var FrmUserProfilParam = new FrmUserProfilParam(pSite, pCentre, CentreListe, SiteListe, ProfilListe, DonnnesProfils, lesParamProfilsUser, userSelected);
                        FrmUserProfilParam.Closed += new EventHandler(FrmUserProfilParam_Closed);
                        this.IsEnabled = false;
                        FrmUserProfilParam.Show();

                    }
                    catch (Exception ex)
                    {
                        //Message.ShowError(ex.Message, Languages.txtDevis);
                    }
                }
                else
                {
                    Message.Show(" Veuillez choisir un site et un centre pour l'utilisateur à créer. ", Galatee.Silverlight.Resources.Langue.Error_Title);
                }
            }
            else
            {
                Message.Show(" Veuillez choisir un site et un centre pour l'utilisateur à créer. ", Galatee.Silverlight.Resources.Langue.Error_Title);
            }

        }

        private void dataGrid_Profil_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        #endregion chnage new

        private static bool IsEMailAddress(string s)
        {
            return Regex.IsMatch(s, "^([\\w-]+\\.)*?[\\w-]+@[\\w-]+\\.([\\w-]+\\.)*?[\\w]+$");
        }

    }
}

