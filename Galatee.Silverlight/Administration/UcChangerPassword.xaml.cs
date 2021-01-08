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
//using Galatee.Silverlight.Security;
//using Galatee.Silverlight.Resources.Administration;
using Galatee.Silverlight.Resources;

namespace Galatee.Silverlight.Administration
{
    public partial class UcChangerPassword : ChildWindow
    {
        public UcChangerPassword()
        {
            InitializeComponent();
        }

        DataGrid gridView = null;
        ServiceAdministration.CsUtilisateur userSelected;
        ServiceAuthenInitialize.CsUtilisateur Unuser;
        string MsgPwdChange = string.Empty;
        bool TenirCompteAncienPwd = false;
        int passage = 0;

        public bool IsPasswordChange
        {
            set;
            get;
        }

        public UcChangerPassword(ServiceAdministration.CsUtilisateur pUser, ServiceAuthenInitialize.CsUtilisateur Leuser, string pMsgPwdChange)
        {
            try
            {
                InitializeComponent();
                userSelected = pUser;
                Unuser = Leuser;
                MsgPwdChange = pMsgPwdChange;
                InitialiseComposants(pUser);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        public UcChangerPassword(ServiceAdministration.CsUtilisateur pUser,ServiceAuthenInitialize.CsUtilisateur Leuser, string pMsgPwdChange, bool pTenirCompteAncienPwd)
        {
            try
            {
                InitializeComponent();
                userSelected = pUser;
                Unuser = Leuser;
                //txt_old.Visibility = System.Windows.Visibility.Collapsed;
                //lbl_old.Visibility = System.Windows.Visibility.Collapsed; 
                TenirCompteAncienPwd = pTenirCompteAncienPwd;
                MsgPwdChange = pMsgPwdChange;
                InitialiseComposants(pUser);
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void InitialiseComposants(ServiceAdministration.CsUtilisateur pUser)
        {
            txt_Login.Text = string.Format("{0} ({1})", pUser.LIBELLE , pUser.MATRICULE );
            Translate();
        }

         private void Translate()
        {
            try
            {
                lbl_login.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_login;
                lbl_new_pwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_new_pwd;
                //lbl_confirm.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_old_pwd;
                lbl_password_conf.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_password_conf;
            }
            catch (Exception ex)
            {
                Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Security.CsStrategieSecurite security = new Security.CsStrategieSecurite();
                //Utility.ParseObject<Security.CsStrategieSecurite, ServiceAuthenInitialize.CsStrategieSecurite>(security, SessionObject.securiteActive);

                security.ACTIF = SessionObject.securiteActive.ACTIF;
                security.CHIFFREMENTREVERSIBLEPASSWORD = SessionObject.securiteActive.CHIFFREMENTREVERSIBLEPASSWORD;
                security.DUREEMAXIMALEPASSWORD = SessionObject.securiteActive.DUREEMAXIMALEPASSWORD;
                security.DUREEMINIMALEPASSWORD = SessionObject.securiteActive.DUREEMINIMALEPASSWORD;
                security.DUREEVEUILLESESSION = SessionObject.securiteActive.DUREEVEUILLESESSION;
                security.DUREEVERROUILLAGECOMPTE = SessionObject.securiteActive.DUREEVERROUILLAGECOMPTE;
                security.HISTORIQUENOMBREPASSWORD = SessionObject.securiteActive.HISTORIQUENOMBREPASSWORD;
                security.LIBELLE = SessionObject.securiteActive.LIBELLE;
                security.LONGUEURMINIMALEPASSWORD = SessionObject.securiteActive.LONGUEURMINIMALEPASSWORD;
                security.NEPASCONTENIRNOMCOMPTE = SessionObject.securiteActive.NEPASCONTENIRNOMCOMPTE;
                security.NOMBREMAXIMALECHECSOUVERTURESESSION = SessionObject.securiteActive.NOMBREMAXIMALECHECSOUVERTURESESSION;
                security.NOMBREMINIMALCARACTERESCHIFFRES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESCHIFFRES;
                security.NOMBREMINIMALCARACTERESMAJUSCULES = SessionObject.securiteActive.NOMBREMINIMALCARACTERESMAJUSCULES;
                security.NOMBREMINIMALCARACTERENONALPHABETIQUES = SessionObject.securiteActive.NOMBREMINIMALCARACTERENONALPHABETIQUES;
                security.PK_IDSTRATEGIESECURITE = SessionObject.securiteActive.PK_IDSTRATEGIESECURITE;
                security.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = SessionObject.securiteActive.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES;
                security.TOUCHEVERROUILLAGESESSION = SessionObject.securiteActive.TOUCHEVERROUILLAGESESSION;




                Security.CsUtilisateur user = new Security.CsUtilisateur();

                user.PK_ID  = userSelected.PK_ID ;
                user.BRANCHE = userSelected.BRANCHE;
                user.DATECREATION = userSelected.DATECREATION ;
                user.DATEDEBUTVALIDITE = userSelected.DATEDEBUTVALIDITE ;
                user.DATEDERNIERECONNEXION = userSelected.DATEDERNIERECONNEXION ;
                user.DATEDERNIEREMODIFICATION = userSelected.DATEDERNIEREMODIFICATION ;
                user.DATEDERNIEREMODIFICATIONPASSWORD = userSelected.DATEDERNIEREMODIFICATIONPASSWORD ;
                user.DATEDERNIERVERROUILLAGE = userSelected.DATEDERNIERVERROUILLAGE ;
                user.DATEFINVALIDITE = userSelected.DATEFINVALIDITE ;
                user.DERNIERECONNEXIONREUSSIE = userSelected.DERNIERECONNEXIONREUSSIE ;
                user.LIBELLE = userSelected.LIBELLE;
                user.ESTSUPPRIMER = userSelected.ESTSUPPRIMER ;
                user.E_MAIL = userSelected.E_MAIL ;
                user.EsADMIN = userSelected.EsADMIN;
                user.CENTRE = userSelected.CENTRE;
                user.FONCTION = userSelected.FONCTION    ;
                user.INITUSERPASSWORD = userSelected.INITUSERPASSWORD ;
                user.LIBELLEFONCTION = userSelected.LIBELLEFONCTION;
                user.LOGINNAME = userSelected.LOGINNAME ;
                user.NOMBREECHECSOUVERTURESESSION = userSelected.NOMBREECHECSOUVERTURESESSION ;
                user.USERCREATION = userSelected.USERCREATION;
                user.PASSE = userSelected.PASSE;
                user.PERIMETREACTION = userSelected.PERIMETREACTION ;
                user.MATRICULE = userSelected.MATRICULE ;
                //user.Prenoms = userSelected.Prenoms;
                //user.RoleDisplayName = userSelected.RO;
                //user.RoleID = userSelected.RoleID;
                user.LIBELLESTATUSCOMPTE = userSelected.LIBELLESTATUSCOMPTE;
                user.PK_ID = userSelected.PK_ID;
                user.FK_IDSTATUS = userSelected.FK_IDSTATUS;


                //Utility.ParseObject(user, userSelected);

                bool isToUpate = Security.Securities.ChangePassword(user, security,user.PASSE, txt_new.Password, txt_confirm.Password,TenirCompteAncienPwd);
                if (isToUpate)
                {
                   // var admUser = Utility.ParseObject(new CsUtilisateur(),user);

                    ServiceAdministration.CsUtilisateur admUser = new ServiceAdministration.CsUtilisateur();
                    admUser.DATEDERNIEREMODIFICATIONPASSWORD  = DateTime.Now;// userSelected.DateDerniereModificationPassword;
                    admUser.PK_ID = user.PK_ID;
                    admUser.BRANCHE  = user.BRANCHE;
                    admUser.DATECREATION  = user.DATECREATION;
                    admUser.DATEDEBUTVALIDITE  = user.DATEDEBUTVALIDITE;
                    admUser.DATEDERNIERECONNEXION  = user.DATEDERNIERECONNEXION;
                    admUser.DATEDERNIEREMODIFICATION = user.DATEDERNIEREMODIFICATION;
                    //admUser.DateDerniereModificationPassword = user.DateDerniereModificationPassword;
                    admUser.DATEDERNIERVERROUILLAGE  = user.DATEDERNIERVERROUILLAGE;
                    admUser.DATEFINVALIDITE  = user.DATEFINVALIDITE;
                    admUser.DERNIERECONNEXIONREUSSIE  = user.DERNIERECONNEXIONREUSSIE;
                    admUser.LIBELLE = user.LIBELLE;
                    admUser.ESTSUPPRIMER  = user.ESTSUPPRIMER;
                    admUser.E_MAIL  = user.E_MAIL;
                    admUser.EsADMIN = user.EsADMIN;
                    admUser.CENTRE   = user.CENTRE;
                    admUser.FONCTION    = user.FONCTION;
                    admUser.INITUSERPASSWORD  = false;
                    admUser.LIBELLEFONCTION = user.LIBELLEFONCTION;
                    admUser.LOGINNAME  = user.LOGINNAME;
                    admUser.NOMBREECHECSOUVERTURESESSION  = user.NOMBREECHECSOUVERTURESESSION;
                    admUser.USERCREATION = user.USERCREATION;
                    admUser.PASSE = user.PASSE;
                    admUser.PERIMETREACTION  = user.PERIMETREACTION;
                    admUser.MATRICULE  = user.MATRICULE;
                    admUser.LIBELLESTATUSCOMPTE = user.LIBELLESTATUSCOMPTE;
                    admUser.PK_ID = user.PK_ID;
                    admUser.FK_IDCENTRE = userSelected.FK_IDCENTRE;
                    admUser.FK_IDFONCTION  = userSelected.FK_IDFONCTION  ;
                    admUser.FK_IDSTATUS  = userSelected.FK_IDSTATUS ;


                    AdministrationServiceClient adm = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));

                    adm.UpdateUserCompleted += (updates, resultupt) =>
                    {
                        if (resultupt.Cancelled || resultupt.Error != null)
                        {
                            string error = resultupt.Error.Message;
                            Message.ShowError(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                            return;
                        }

                        if (resultupt.Result == false)
                        {
                            Message.ShowInformation(Galatee.Silverlight.Resources.Langue.updateError, Galatee.Silverlight.Resources.Langue.errorTitle);
                            IsPasswordChange = false;
                            return;
                        }
                        else
                        {
                            var ws = new MessageBoxControl.MessageBoxChildWindow("", Galatee.Silverlight.Resources.Langue.ConfirmationModifMotDePasse, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            
                            IsPasswordChange = true;
                            ws.OnMessageBoxClosed += (l, results) =>
                            {
                                if (ws.Result == MessageBoxResult.OK)
                                {
                                        //ObtenirDonneesConnection( Unuser);
                                    this.DialogResult = true;

                                }
                            };
                            ws.Show();
                        }
                    };
                    adm.UpdateUserAsync(admUser,true );
                }
                
            }
            catch (Exception ex)
            {
                Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
                OKButton_Click(null,null );
        }
        //void ObtenirDonneesConnection(ServiceAuthenInitialize.CsUtilisateur _leUser)
        //{

        //    try
        //    {

        //        UserConnecte.PK_ID = _leUser.PK_ID;
        //        UserConnecte.matricule = _leUser.MATRICULE;
        //        UserConnecte.Centre = _leUser.CENTRE;
        //        UserConnecte.nomUtilisateur = _leUser.LIBELLE;
        //        UserConnecte.codefontion = _leUser.FONCTION;
        //        UserConnecte.LibelleFonction = _leUser.LIBELLEFONCTION;
        //        UserConnecte.PerimetreAction = _leUser.PERIMETREACTION;
        //        UserConnecte.LibelleCentre = _leUser.LIBELLECENTRE;
        //        UserConnecte.FK_IDFONCTION = _leUser.FK_IDFONCTION;
        //        UserConnecte.listeProfilUser = _leUser.LESPROFILSUTILISATEUR;
        //        // construction du menu des modules relatifs au profil du userconnecte

        //            Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient proxy = new Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //            proxy.GetListeModuleCompleted += (sender, results) =>
        //            {
        //                if (results.Cancelled || results.Error != null)
        //                {
        //                    //desableProgressBar();
        //                    Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                    CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                    passage = 0;
        //                    LayoutRoot.Cursor = Cursors.Wait;
        //                    return;
        //                }

        //                if (results.Result == null || results.Result.Count == 0)
        //                {
        //                    //desableProgressBar();
        //                    Message.Show(Galatee.Silverlight.Resources.Langue.wcf_no_profile_module, Galatee.Silverlight.Resources.Langue.informationTitle);
        //                    CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                    passage = 0;
        //                    LayoutRoot.Cursor = Cursors.Wait;
        //                    return;
        //                }

        //                /// call MVVM to create module treeNode
        //                /// 
        //                List<ServiceAuthenInitialize.CsDesktopGroup>  modules = results.Result;

        //                Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient cls = new Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //                cls.RetourneListImpranteCompleted += (send, aa) =>
        //                {
        //                    if (aa.Cancelled || aa.Error != null)
        //                    {
        //                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                        Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                        //desableProgressBar();
        //                        passage = 0;
        //                        LayoutRoot.Cursor = Cursors.Wait;
        //                        return;
        //                        // 
        //                    }

        //                    if (aa.Result == null || aa.Result.Count == 0)
        //                    {
        //                        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //                        Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
        //                        //desableProgressBar();
        //                        passage = 0;
        //                        LayoutRoot.Cursor = Cursors.Wait;
        //                        return;
        //                    }
        //                    List<ServiceAuthenInitialize.CParametre> imprimante = new List<ServiceAuthenInitialize.CParametre>();
        //                    imprimante = aa.Result;
        //                    foreach (ServiceAuthenInitialize.CParametre impr in imprimante)
        //                        SessionObject.Imprimantes.Add(impr.LIBELLE);



        //                    // enumeree de la procedure stockees 

        //                    Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient prox = new Galatee.Silverlight.ServiceAuthenInitialize.AuthentInitializeServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Initialisation"));
        //                    prox.returnEnumProcedureStockeeCompleted += (sa, resa) =>
        //                    {

        //                        if (resa.Cancelled || resa.Error != null)
        //                        {
        //                            string error = resa.Error.Message;
        //                            Message.Show(Galatee.Silverlight.Resources.Langue.wcf_error, Galatee.Silverlight.Resources.Langue.errorTitle);
        //                            LayoutRoot.Cursor = Cursors.Wait;
        //                            return;
        //                        }

        //                        if (resa.Result == null)
        //                        {
        //                            Message.Show(Galatee.Silverlight.Resources.Langue.msgNodata, Galatee.Silverlight.Resources.Langue.informationTitle);
        //                            LayoutRoot.Cursor = Cursors.Wait;
        //                            return;
        //                        }

        //                        SessionObject.EnumereProcedureStockee = resa.Result;

        //                        //completedAllRequest = true;
        //                        //desableProgressBar();
        //                        LayoutRoot.Cursor = Cursors.Wait;
        //                        //success(this, new EventArgs());
        //                        this.DialogResult = true;

        //                    };
        //                    prox.returnEnumProcedureStockeeAsync();
        //                };
        //                cls.RetourneListImpranteAsync();
        //            };
        //            proxy.GetListeModuleAsync(_leUser);

        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        Message.ShowError(ex.Message, Galatee.Silverlight.Resources.Langue.informationTitle);// throw;
        //        CancelButton.IsEnabled = OKButton.IsEnabled = true;
        //        passage = 0;
        //        LayoutRoot.Cursor = Cursors.Wait;
        //        //desableProgressBar();
        //    }
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

    

        
    }
}

