using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Galatee.Security;
using Galatee.Sms.Tools;
using Galatee.DataAccess;
using Galatee.Structure;

namespace Galatee.Sms.WsManagement
{
    public partial class GalateeSmsManagement : ServiceBase
    {
        private Boolean bServiceSendInProcess;
        private String m_PathSuiviServiceSms = "";
        private const String cstTitreErreurEnvoie = "ENVOIE SMS";
        private const String cstTitreErreurTraitement = "TRAITEMENT SMS";
        private const String cstTitreErreurReception = "LECTURE SMS";

        private EnoieReceptionSms mcSendReadSms;
        private static String mConnexionString = "Data Source={0};Initial Catalog={1};Integrated Security=false;User ID={2};Password={3}";

        public GalateeSmsManagement()
        {
            try
            {
                InitializeComponent();
                if (!EventLog.SourceExists("Galatee.Sms.WsManagement"))
                    EventLog.CreateEventSource("Galatee.Sms.WsManagement", "Application");
                eventLog.Source = "Galatee.Sms.WsManagement";
                eventLog.Log = "Application";
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // TODO : ajoutez ici le code pour démarrer votre service.
                bServiceSendInProcess = true;
                //bServiceTraiterInProcess = true;
                if (InitService()) return;
                //bServiceReadInProcess = false;
                bServiceSendInProcess = false;
                //bServiceTraiterInProcess = false;

            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                eventLog.WriteEntry("Le service Galatee.Sms.WsManagement s'est arrêté ");
            }
            catch (Exception ex)
            {
                eventLog.WriteEntry(ex.Message);
            }
        }

        private Boolean CreateFichierTrace()
        {
            try
            {
                String sExePath = System.Reflection.Assembly.GetExecutingAssembly().Location.TrimEnd('\\');
                System.IO.FileInfo info = new System.IO.FileInfo(sExePath);
                sExePath = info.DirectoryName.TrimEnd('\\');
                String sPathSuivi = sExePath + @"\Suivi_Galatee_SMS";

                m_PathSuiviServiceSms = sPathSuivi + String.Format(@"\FichierSuiviServiceSmsGalateeDu_{0}.txt", DateTime.Now.ToString("ddMMyyyy"));
                if (System.IO.Directory.Exists(sPathSuivi) == false)
                    System.IO.Directory.CreateDirectory(sPathSuivi);
                return true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Galatee.Sms.WsManagement", ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        internal Boolean InitService()
        {
            try
            {
                ModemSettings.Settings.Read();
                //mAdapterSms = new SmsTableAdapter();

                mConnexionString = String.Format(mConnexionString,
                                                 ModemSettings.Settings.Option.mServeurSQL,
                                                 ModemSettings.Settings.Option.mBaseDonnees,
                                                 Galatee.Security.Connexion.GalaUserId,
                                                 Galatee.Security.Connexion.GalaUserPassword);

                mcSendReadSms = new EnoieReceptionSms();


                mcSendReadSms.InitiPort(ModemSettings.Settings.Port.PortName,
                                        ModemSettings.Settings.Port.BaudRate,
                                        ModemSettings.Settings.Port.DataBits);

                //bServiceReadInProcess = false;
                bServiceSendInProcess = false;
                //bServiceTraiterInProcess = false;
                //Teste d'accès à la base de données
                SqlConnection con = new SqlConnection(mConnexionString);

                try
                {
                    con.Open();
                }
                catch
                {
                    EventLog.WriteEntry("Galatee.Sms.WsManagement",
                        "Service démarré avec erreur d'accès à la base de donnnées",
                        EventLogEntryType.Error);
                    return false;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
                // Fin teste

                timer.Enabled = true;
                EventLog.WriteEntry("Galatee.Sms.WsManagement", "Service démarré avec succès", EventLogEntryType.Information);
                return CreateFichierTrace();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Galatee.Sms.WsManagement", "Service démarré avec erreur:" + ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        private void EnvoieSms()
        {
            bServiceSendInProcess = true;
            timer.Interval = 60 * 60 * 1000;
            String sTitreErreur = "";
            String sMessageErreur = "";

            CErreurAppli erreur = new CErreurAppli();
            CreateFichierTrace();
            try
            {
                var ListSmsToSend = DBEservice.GetSmsByStatutEnvoiNombreEnvoi((int)StatutEnvoiSms.NonEnvoye, ModemSettings.Settings.Option.iNbreTraitementSms);
                //Recuperer les SMS à envoyer
                if (ListSmsToSend != null && ListSmsToSend.Count > 0)
                {
                    String sRetourSend;
                    foreach (CsSms mSms in ListSmsToSend)
                    {
                        sRetourSend = "";
                        if (string.IsNullOrEmpty(mSms.DESTINATEUR))
                        {
                            sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            sMessageErreur = String.Format("Echec d'envoie du SMS Num:{0}", mSms.SMSID) + " Numéro de téléphone, Messsage inexistant";
                            Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", sMessageErreur,
                                                                 m_PathSuiviServiceSms);
                            continue;
                        }
                        if (string.IsNullOrEmpty(mSms.DESTINATEUR))
                        {
                            sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            sMessageErreur = String.Format("Echec d'envoie du SMS Num:{0}", mSms.SMSID) + " Numéro de téléphone inexistant";
                            Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSms", sMessageErreur,
                                                                 m_PathSuiviServiceSms);
                            continue;
                        }
                        if (string.IsNullOrEmpty(mSms.MESSAGE))
                        {
                            sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            sMessageErreur = String.Format("Echec d'envoie du SMS Num:{0}", mSms.SMSID) + " Messsage inexistant";
                            Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", sMessageErreur,
                                                                 m_PathSuiviServiceSms);
                            continue;
                        }
                        if (mSms.MESSAGE.Length > Utilitaire.iNombreMaxSms) // Nombre maximum de caractère dans un SMS normal Ajouter le 12/02/2013 par ATO
                        {
                            erreur.Set(eCodeErreur.ECHEC_ENVOIE_SMS_MESSAGE_TROP_LONG);
                            continue;
                        }
                        if (!mcSendReadSms.EnvoieReceptionSynchrone(ref erreur,false, mSms.DESTINATEUR, mSms.MESSAGE, out sRetourSend))
                        {
                            //Ecrire erreur dans table ou fichier
                            sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                         DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            sMessageErreur = String.Format("Echec d'envoie du SMS Num:{0}", mSms.SMSID + " " + sRetourSend);
                            Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", sMessageErreur, m_PathSuiviServiceSms);

                            mSms.STATUTENVOI = (int)StatutEnvoiSms.NonEnvoye;
                            mSms.NOMBREENVOI = mSms.NOMBREENVOI == null ? 0 : 1;
                            if (!DBEservice.Update(mSms))
                            {
                                //Ecrire erreur dans table ou fichier
                                sMessageErreur = String.Format("Message Numéro {0} non enregistré. \nCause: {1} ",
                                                               mSms.SMSID, erreur.MessageErreur);

                                sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                             DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", sMessageErreur,
                                                                     m_PathSuiviServiceSms);

                            }
                            continue;
                        }
                        else
                        {
                            mSms.STATUTENVOI = (int)StatutEnvoiSms.Envoye;
                            mSms.NOMBREENVOI = mSms.NOMBREENVOI == null ? 0 : 1;
                            mSms.DATEEMISSION = DateTime.Now;
                            if (!DBEservice.Update(mSms))
                            {
                                //Ecrire erreur dans table ou fichier
                                sMessageErreur = String.Format("Message Numéro {0} non enregistré. \nCause: {1} ",
                                                               mSms.SMSID, erreur.MessageErreur);

                                sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                             DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                                Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", sMessageErreur,
                                                                     m_PathSuiviServiceSms);

                            }
                            continue;
                        }
                    }

                }
                return;
            }
            catch (Exception ex)
            {
                erreur.Set(eCodeErreur.ERROR, ex.Message);
                //Ecrire erreur dans table ou fichier
                sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie,
                                                             DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", ex.Message, m_PathSuiviServiceSms);

                return;
            }
            finally
            {
                bServiceSendInProcess = false;
                timer.Interval = 1000;
            }
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CErreurAppli erreur = new CErreurAppli();
            String sTitreErreur = "";
            CreateFichierTrace();
            try
            {
                if (bServiceSendInProcess == false)
                {
                    //if (InitService()) return;
                    EnvoieSms();
                }
            }
            catch (Exception ex)
            {

                erreur.Set(eCodeErreur.ERROR, ex.Message);
                //Ecrire erreur dans table ou fichier
                sTitreErreur = String.Format("{0} {1}", cstTitreErreurEnvoie, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                Utilitaire.WritePrivateProfileString(sTitreErreur, "ErreurServiceSmsGalatee", ex.Message, m_PathSuiviServiceSms);
                return;
            }
        }
        
    }
}
