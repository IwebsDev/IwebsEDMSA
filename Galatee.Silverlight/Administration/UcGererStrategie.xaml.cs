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
using Galatee.Silverlight.Security;
using Galatee.Silverlight.Library;

namespace Galatee.Silverlight.Administration
{
    public partial class UcGererStrategie : ChildWindow
    {
        public UcGererStrategie()
        {
            InitializeComponent();
        }
        ServiceAdministration.CsStrategieSecurite StrategySelected;
        DataGrid gridView = null;

        public UcGererStrategie(string _sens)
        {
            InitializeComponent();
        }

        public UcGererStrategie(object[] _user, SessionObject.ExecMode[] pExecMode, DataGrid[] grid)
        {
            InitializeComponent();
            try
            {
                translate();
                StrategySelected = new ServiceAdministration.CsStrategieSecurite();
                StrategySelected = _user[0] as ServiceAdministration.CsStrategieSecurite;
                GetData(pExecMode[0], StrategySelected);
                gridView = grid[0];
                Tag = pExecMode[0];
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }
        void translate()
        {
            //this.lbl_label.Content =Langue.l
            try
            {
                //this.lbl_standby.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeMiniMotPasse;
                //this.lbl_lockingcounter.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_LongeuPasse;
                //this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NombreEchec;
                //this.lbl_lockingcounter.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeDeveilleSession;
                //this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_reinitCompteurPasse;
                //this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbreMiniCaractereMajusc;
                //this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbreMiniCaractereMinus;
                //this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbrMinuChiffre;

                this.lbl_minpwdlimit.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeMiniMotPasse;
                this.lbl_maxpwdlimit.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeMaxiMotPasse;
                this.lbl_minlenght.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_LongeuPasse;
                this.ckcreversible.Content = Galatee.Silverlight.Resources.Administration.Langue.chk_Revers;
                this.lbl_locksession.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_Vérouillage;
                this.lbl_connexfail.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NombreEchec;
                this.lbl_standby.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeDeveilleSession;
                this.lbl_lockingcounter.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_DureeVerouillageCompte;
                this.lbl_LockedAccont.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_reinitCompteurPasse;
                this.chck_login_username.Content = Galatee.Silverlight.Resources.Administration.Langue.Chk_pasPasse;
                this.lbl_uppercase.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbreMiniCaractereMajusc;
                this.lbl_lowercase.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbreMiniCaractereMinus;
                this.lbl_digit.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_NbrMinuChiffre;
                this.lbl_specialchar.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_CaractereMini;
                this.lbl_saved.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_HistoriqueNbMotPasse;
                this.lbl_label.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_lbl_label;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        void GetData(SessionObject.ExecMode pSens, ServiceAdministration.CsStrategieSecurite pStrategy)
        {
            try
            {
                AdministrationServiceClient stragClient = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                stragClient.GetAllStrategieSecuriteCompleted += (sbrch, argbrch) =>
                {
                    if (argbrch.Cancelled || argbrch.Error != null)
                    {
                        string error = argbrch.Error.Message;
                        Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }

                    try
                    {
                        if (pSens == SessionObject.ExecMode.Consultation || pSens == SessionObject.ExecMode.Modification)
                        {
                            ckstatusStrat.IsChecked = pStrategy.ACTIF;
                            RecupererInfoStrategy(pStrategy);
                            if (pSens == SessionObject.ExecMode.Consultation)
                            {
                                Btn_Cancel.Content = Btn_OK.Content;
                                Btn_OK.Visibility = Visibility.Collapsed;
                                desactiverControles(false);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }
                };
                stragClient.GetAllStrategieSecuriteAsync();
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
                //lbl_branch.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_branche;
                //lbl_cash.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_num_çaisse;
                //lbl_çreation_date.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_date_creation;
                //lbl_from.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_period_from;
                //lbl_id.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_matricule;
                //lbl_last_modif_date.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_date_dernier_modif;
                //lbl_login.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_login;
                //lbl_PeriodValidite.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_period_validite;
                //lbl_pwd.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_password;
                //lbl_pwd_conf.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_password_conf;
                //lbl_role.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_profil;
                //lbl_to.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_period_to;
                //lbl_userStatus.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_status_user;
                //lblPerimAction.Content = Galatee.Silverlight.Resources.Administration.Langue.lbl_perimetre_action;
                //headerProfil.Header = Galatee.Silverlight.Resources.Administration.Langue.header_profil;
                //headerRef.Header = Galatee.Silverlight.Resources.Administration.Langue.header_reference;
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void RecupererInfoStrategy(ServiceAdministration.CsStrategieSecurite pSecurity)
        {
            try
            {
                txt_strayegy.Text = pSecurity.LIBELLE;
                txt_locksession1.Text = pSecurity.TOUCHEVERROUILLAGESESSION.Split('-')[0];
                txt_locksession2.Text = pSecurity.TOUCHEVERROUILLAGESESSION.Split('-')[1];

                NumUpDown_saved.Value = Convert.ToDouble(pSecurity.HISTORIQUENOMBREPASSWORD);
                NumUpDown_connexfail.Value = Convert.ToDouble(pSecurity.NOMBREMAXIMALECHECSOUVERTURESESSION);
                NumUpDown_LockedAccont.Value = Convert.ToDouble(pSecurity.DUREEVERROUILLAGECOMPTE);
                NumUpDown_lockingcounter.Value = Convert.ToDouble(pSecurity.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES);
                NumUpDown_lowercase.Value = Convert.ToDouble(pSecurity.NOMBREMINIMALCARACTERESMINISCULES);
                NumUpDown_maxpwdlimit.Value = Convert.ToDouble(pSecurity.DUREEMAXIMALEPASSWORD);
                NumUpDown_minlenght.Value = Convert.ToDouble(pSecurity.LONGUEURMINIMALEPASSWORD);
                NumUpDown_minpwdlimit.Value = Convert.ToDouble(pSecurity.DUREEMINIMALEPASSWORD);
                NumUpDown_specialchar.Value = Convert.ToDouble(pSecurity.NOMBREMINIMALCARACTERENONALPHABETIQUES);
                NumUpDown_standby.Value = Convert.ToDouble(pSecurity.DUREEVEUILLESESSION);
                NumUpDown_uppercase.Value = Convert.ToDouble(pSecurity.NOMBREMINIMALCARACTERESMAJUSCULES);
                NumUpDown_digit.Value = Convert.ToDouble(pSecurity.NOMBREMINIMALCARACTERESCHIFFRES);
                chck_login_username.IsChecked = pSecurity.NEPASCONTENIRNOMCOMPTE ? true : false;
                ckstatusStrat.IsChecked = pSecurity.ACTIF;
                ckcreversible.IsChecked = pSecurity.CHIFFREMENTREVERSIBLEPASSWORD ? true : false;

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

        void GererUtilisateur(SessionObject.ExecMode pSens, ServiceAdministration.CsStrategieSecurite pSecuritySelected)
        {
            ServiceAdministration.CsStrategieSecurite security = new ServiceAdministration.CsStrategieSecurite();
            var _security = new ServiceAdministration.CsStrategieSecurite();
            try
            {

                if (txt_locksession2.Text == txt_locksession1.Text)
                    throw new Exception(Galatee.Silverlight.Resources.Administration.Langue.MsgErrVerouill);
                if (pSens != SessionObject.ExecMode.Creation)
                {
                    security.PK_IDSTRATEGIESECURITE = pSecuritySelected.PK_IDSTRATEGIESECURITE;
                    security.PK_ID = pSecuritySelected.PK_ID;
                }
                if (pSens == SessionObject.ExecMode.Modification)
                {
                    security.PK_ID = pSecuritySelected.PK_ID;
                    security.PK_IDSTRATEGIESECURITE = pSecuritySelected.PK_IDSTRATEGIESECURITE;
                }

                security.LIBELLE = txt_strayegy.Text;
                security.HISTORIQUENOMBREPASSWORD = Convert.ToInt16(NumUpDown_saved.Value);
                security.DUREEMINIMALEPASSWORD = Convert.ToInt16(NumUpDown_minpwdlimit.Value);
                security.DUREEMAXIMALEPASSWORD = Convert.ToInt16(NumUpDown_maxpwdlimit.Value);
                security.LONGUEURMINIMALEPASSWORD = Convert.ToInt16(NumUpDown_minlenght.Value);
                security.CHIFFREMENTREVERSIBLEPASSWORD = ckcreversible.IsChecked.Value;
                security.TOUCHEVERROUILLAGESESSION = txt_locksession1.Text + "-" + txt_locksession2.Text;
                security.NOMBREMAXIMALECHECSOUVERTURESESSION = Convert.ToInt16(NumUpDown_connexfail.Value);
                security.DUREEVEUILLESESSION = Convert.ToInt16(NumUpDown_standby.Value);
                security.DUREEVERROUILLAGECOMPTE = Convert.ToInt16(NumUpDown_lockingcounter.Value);
                security.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = Convert.ToInt16(NumUpDown_LockedAccont.Value);
                security.NEPASCONTENIRNOMCOMPTE = chck_login_username.IsChecked.Value;
                security.NOMBREMINIMALCARACTERESMAJUSCULES = Convert.ToInt16(NumUpDown_uppercase.Value);
                security.NOMBREMINIMALCARACTERESMINISCULES = Convert.ToInt16(NumUpDown_lowercase.Value);
                security.NOMBREMINIMALCARACTERESCHIFFRES = Convert.ToInt16(NumUpDown_digit.Value);
                security.NOMBREMINIMALCARACTERENONALPHABETIQUES = Convert.ToInt16(NumUpDown_specialchar.Value);
                security.ACTIF = ckstatusStrat.IsChecked.Value;
                _security = Utility.ParseObject(_security, security);

                //Controle de l'unicité de la stratégie active

                List<Galatee.Silverlight.ServiceAuthenInitialize.CsStrategieSecurite> strategie = new List<Galatee.Silverlight.ServiceAuthenInitialize.CsStrategieSecurite>();

                if (pSens == SessionObject.ExecMode.Creation)
                {
                    _security.USERCREATION = UserConnecte.matricule;
                    _security.DATECREATION = DateTime.Now.Date;
                    //_security.PK_ID = new Guid();
                    //_security.PK_IDSTRATEGIESECURITE = new Guid();
                    InsertAdmStrategieSecurite(_security);
                }
                else
                    if (pSens == SessionObject.ExecMode.Modification)
                        if (!Utility.HasSamePropertyValue(security, ParseValueToModification(pSecuritySelected))) // permet de savoir si un composant a été modifié sur l'IHM en vu
                        {
                            _security.USERCREATION = pSecuritySelected.USERCREATION;
                            _security.DATECREATION = pSecuritySelected.DATECREATION;
                            _security.USERMODIFICATION = UserConnecte.matricule;
                            _security.DATEMODIFICATION = DateTime.Now.Date;

                            if (Verification(gridView, security))
                                UpdateAdmStrategieSecurite(_security);
                        }
                        else
                            Message.Show(Galatee.Silverlight.Resources.Langue.AucuneModificationIhm, Galatee.Silverlight.Resources.Langue.informationTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ServiceAdministration.CsStrategieSecurite ParseValueToModification(ServiceAdministration.CsStrategieSecurite pUserSelected)
        {
            try
            {
                ServiceAdministration.CsStrategieSecurite security = new ServiceAdministration.CsStrategieSecurite();

                security.LIBELLE = txt_strayegy.Text;
                security.HISTORIQUENOMBREPASSWORD = pUserSelected.HISTORIQUENOMBREPASSWORD;
                security.DUREEMINIMALEPASSWORD = pUserSelected.DUREEMINIMALEPASSWORD;
                security.DUREEMAXIMALEPASSWORD = pUserSelected.DUREEMAXIMALEPASSWORD;
                security.LONGUEURMINIMALEPASSWORD = pUserSelected.LONGUEURMINIMALEPASSWORD;
                security.CHIFFREMENTREVERSIBLEPASSWORD = pUserSelected.CHIFFREMENTREVERSIBLEPASSWORD;
                security.TOUCHEVERROUILLAGESESSION = pUserSelected.TOUCHEVERROUILLAGESESSION;
                security.NOMBREMAXIMALECHECSOUVERTURESESSION = pUserSelected.NOMBREMAXIMALECHECSOUVERTURESESSION;
                security.DUREEVEUILLESESSION = pUserSelected.DUREEVEUILLESESSION;
                security.DUREEVERROUILLAGECOMPTE = pUserSelected.DUREEVERROUILLAGECOMPTE;
                security.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES = pUserSelected.REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES;
                security.NEPASCONTENIRNOMCOMPTE = pUserSelected.NEPASCONTENIRNOMCOMPTE;
                security.NOMBREMINIMALCARACTERESMAJUSCULES = pUserSelected.NOMBREMINIMALCARACTERESMAJUSCULES;
                security.NOMBREMINIMALCARACTERESMINISCULES = pUserSelected.NOMBREMINIMALCARACTERESMINISCULES;
                security.NOMBREMINIMALCARACTERESCHIFFRES = pUserSelected.NOMBREMINIMALCARACTERESCHIFFRES;
                security.NOMBREMINIMALCARACTERENONALPHABETIQUES = pUserSelected.NOMBREMINIMALCARACTERENONALPHABETIQUES;
                security.ACTIF = pUserSelected.ACTIF;
                return security;
            }
            catch (Exception)
            {
                return new ServiceAdministration.CsStrategieSecurite();
            }
        }

        void UpdateAdmStrategieSecurite(ServiceAdministration.CsStrategieSecurite pStrategyUpdate)
        {

            try
            {
                AdministrationServiceClient majStrategy = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                majStrategy.UpdateStrategieSecuriteCompleted += (majs, resultmaj) =>
                {
                    if (resultmaj.Cancelled || resultmaj.Error != null)
                    {
                        string error = resultmaj.Error.Message;
                        Message.Show(error, Galatee.Silverlight.Resources.Langue.informationTitle);
                        return;
                    }

                    if (resultmaj.Result == false)
                    {
                        Message.Show(Galatee.Silverlight.Resources.Administration.Langue.MsgErrorMajUser, Galatee.Silverlight.Resources.Langue.errorTitle);
                        return;
                    }
                    else
                    {
                        Message.Show(Galatee.Silverlight.Resources.Langue.updateSuccess, Galatee.Silverlight.Resources.Langue.ConfirmationTitle);
                        MiseAJourDonnees(pStrategyUpdate, gridView);
                        this.DialogResult = true;
                    }



                };
                majStrategy.UpdateStrategieSecuriteAsync(pStrategyUpdate);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void MiseAJourDonnees(ServiceAdministration.CsStrategieSecurite pStrategy, DataGrid grid)
        {
            try
            {
                List<Galatee.Silverlight.ServiceAdministration.CsStrategieSecurite> _Strategy = grid.ItemsSource as List<Galatee.Silverlight.ServiceAdministration.CsStrategieSecurite>;
                foreach (Galatee.Silverlight.ServiceAdministration.CsStrategieSecurite _strategy in _Strategy)
                {
                    if (pStrategy.PK_IDSTRATEGIESECURITE == _strategy.PK_IDSTRATEGIESECURITE)
                    {
                        _Strategy.Remove(_strategy);
                        _Strategy.Add(pStrategy);
                        break;
                    }

                }
                grid.ItemsSource = null;
                grid.ItemsSource = _Strategy;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        void InsertAdmStrategieSecurite(ServiceAdministration.CsStrategieSecurite pStrategy)
        {
            try
            {
                AdministrationServiceClient insertStrategy = new AdministrationServiceClient(Utility.ProtocoleFacturation(), Utility.EndPoint("Administration"));
                insertStrategy.InsertStrategieSecuriteCompleted += (inserS, resultIns) =>
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
                        InitialiserDonneeDataGrid(resultIns.Result, gridView);
                        this.DialogResult = true;
                    }

                };
                insertStrategy.InsertStrategieSecuriteAsync(pStrategy);
            }
            catch (Exception ex)
            {

                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        void InitialiserDonneeDataGrid(List<ServiceAdministration.CsStrategieSecurite> pStrategy, DataGrid usergrid)
        {
            try
            {
                usergrid.ItemsSource = null;
                usergrid.ItemsSource = pStrategy;
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
                  GererUtilisateur((SessionObject.ExecMode)Tag, StrategySelected);
            }
            catch (Exception ex)
            {
                Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
            }

        }

        private bool Verification(DataGrid gridView, ServiceAdministration.CsStrategieSecurite pStrategySelected)
        {
            try
            {
                List<ServiceAdministration.CsStrategieSecurite> stragtegies = gridView.Tag as List<ServiceAdministration.CsStrategieSecurite>;
                foreach (ServiceAdministration.CsStrategieSecurite s in stragtegies)
                {
                    if (s.PK_IDSTRATEGIESECURITE == pStrategySelected.PK_IDSTRATEGIESECURITE)
                        s.ACTIF = pStrategySelected.ACTIF;
                }

                int totalActive = stragtegies.Where(s => s.ACTIF == true).Count();

                if (totalActive == 0 || totalActive > 1)
                {
                    Message.ShowWarning("Au plus une stratégie de sécurité doit être active", "Stratégie sécurité");
                    foreach (ServiceAdministration.CsStrategieSecurite s in stragtegies)
                    {
                        if (s.PK_IDSTRATEGIESECURITE == pStrategySelected.PK_IDSTRATEGIESECURITE)
                            s.ACTIF = !pStrategySelected.ACTIF;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Galatee.Silverlight.Shared.ClasseMEthodeGenerique.FermetureEcran(this);

        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_locksession1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox t = (TextBox)sender;
                // t.Text =Key.;

            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
            }
        }

        private void numericTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}

