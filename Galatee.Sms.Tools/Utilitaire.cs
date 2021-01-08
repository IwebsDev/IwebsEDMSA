using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Text;
using System.Linq;
using System.Collections;
using System.IO.Compression;


namespace Galatee.Sms.Tools
{
    public class Utilitaire
    {
        static String sChaine = "Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        public static char SeparateurDeFichier = '|';
        private static String[] tFactures = null;
        private static String[] tLignesCompsantLaFacture = null;

        public const int iNombreMaxSms = 160;
       
        [DllImport("Kernel32.dll")]
        public static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            [In, Out] System.Text.StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
            );

        [DllImport("Kernel32.dll")]
        public static extern bool WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName
            );


        public static string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }

        public static bool ValiderParametresApp(String pDataBaseName, String pServerName, out string pErreur)
        {
            System.Configuration.Configuration m_config;
            string sChaine = string.Empty;
            pErreur = string.Empty;
            try
            {
                String sPath = Assembly.GetExecutingAssembly().Location;
                m_config = ConfigurationManager.OpenExeConfiguration(sPath);
                m_config.AppSettings.Settings.Remove("BDEnvoiFactureCIEConnectionString");
                //constituer la chaine
                sChaine = GetSqlConnexionStringBuilder(pDataBaseName, pServerName).ConnectionString;
                if (!Utilitaire.EstSqlConnexionStringValide(sChaine, out pErreur))
                    throw new Exception(pErreur);
                else
                {
                    // Crypter la chain
                    sChaine = Crypteur.CrypterText(sChaine);
                    m_config.AppSettings.Settings.Add("BDEnvoiFactureCIEConnectionString", sChaine);
                    m_config.Save(ConfigurationSaveMode.Modified);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static SqlConnectionStringBuilder GetSqlConnexionStringBuilder(String pDataBaseName, String pServerName)
        {
            try
            {
                var StrBuild = new SqlConnectionStringBuilder
                {
                    DataSource = pServerName,
                    InitialCatalog = pDataBaseName,
                    PersistSecurityInfo = true,
                    UserID = Galatee.Security.Connexion.GalaUserId,
                    Password = Galatee.Security.Connexion.GalaUserPassword
                };
                return StrBuild;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool EstSqlConnexionStringValide(string connectionString, out string Erreur)
        {
            Erreur = "";
            try
            {
                var Connexion = new SqlConnection(connectionString);
                Connexion.Open();
                Connexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                Erreur = ex.Message;
                return false;
            }
        }




        public static bool SendMail(string pFrom, string pTo, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, out string pErreur)
        {
            MailMessage message = null;
            string errorMessage = "";
            pErreur = string.Empty;
            try
            {
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.Body = pBody;
                message.Subject = pSubject;
                var client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };

                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;

                client.Send(message);
                return true;
            }
            catch (/*Exception ex*/ReflectionTypeLoadException ex)
            {

                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                errorMessage = sb.ToString();

                pErreur = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
            }
        }
        
        public static bool SendMail(string pFrom, string pTo, string pCc, string pAttachement, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, out string pErreur)
        {
            SmtpClient client = null;
            MailMessage message = null;
            try
            {
                GC.Collect();
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.Body = pBody;
                message.Subject = pSubject;
                message.Attachments.Add(new Attachment(pAttachement));
                client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
                GC.Collect();
            }
        }

        public static bool SendMail(string pFrom, string pTo, string pAttachement, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, out string pErreur)
        {
            SmtpClient client = null;
            MailMessage message = null;
            try
            {
                GC.Collect();
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.Body = pBody;
                message.Subject = pSubject;
                if(pAttachement!=null)
                    message.Attachments.Add(new Attachment(pAttachement));

                client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
                GC.Collect();
            }
        }

        public static bool SendMail(string pFrom, string pTo, string[] pAttachement, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, out string pErreur)
        {
            SmtpClient client = null;
            MailMessage message = null;
            try
            {
                GC.Collect();
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.Body = pBody;
                message.Subject = pSubject;

                foreach (String sFile in pAttachement)
                  message.Attachments.Add(new Attachment(sFile));

                client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
                GC.Collect();
            }
        }

        public static bool SendMail(string pFrom, string pTo, string pCc, string pAttachement, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, bool pAck, out string pErreur)
        {
            MailMessage message = null;
            try
            {
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.CC.Add(new MailAddress(pCc));
                message.Body = pBody;
                message.Subject = pSubject;
                message.Attachments.Add(new Attachment(pAttachement));
                var client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
            }
        }

        public static bool SendMailwithoutAttachement(string pFrom, string pTo, string pCc, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, out string pErreur)
        {
            MailMessage message = null;
            try
            {
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.CC.Add(new MailAddress(pCc));
                message.Body = pBody;
                message.Subject = pSubject;
                var client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                message.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
            }
        }

        public static bool SendMailwithoutCc(string pFrom, string pTo, string pAttachement, string pBody, string pSubject, string pSmtpClient, int pPort, bool pEnableSsl, string pUser, string pPassword, bool pAck, out string pErreur)
        {
            MailMessage message = null;
            try
            {
                pErreur = string.Empty;
                message = new MailMessage { From = new MailAddress(pFrom) };
                message.To.Add(new MailAddress(pTo));
                message.Body = pBody;
                message.Subject = pSubject;
                message.Attachments.Add(new Attachment(pAttachement));
                //If akn is requiered
                if (pAck)
                {
                    message.Headers.Add("Disposition-Notification-To", pFrom);
                }
                var client = new SmtpClient(pSmtpClient, pPort)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = pEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(pUser, pPassword)
                };
                //validate the certificate
                ServicePointManager.ServerCertificateValidationCallback =
                    (s, certificate, chain, sslPolicyErrors) => true;
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
            finally
            {
                if (message != null) message.Dispose();
            }
        }

        public static string TestConnexion(String sHote, String sEmail, String sUser, Boolean bUseSSL, String sMotDePasse, int iPort)
        {

            string Erreur = string.Empty;
            string OutMessage = string.Empty;

            try
            {

                if (
                    SendMail(sEmail, sEmail,
                                       "Cet E-Mail est un mail de test de configuration du serveur SMTP. ",
                                       "Configuration serveur SMTP", sHote, iPort, bUseSSL, sUser, sMotDePasse, out Erreur))
                {
                    OutMessage = "Test de connexion au serveur SMTP réalisé avec succès";
                }
                else if (!string.IsNullOrEmpty(Erreur))
                {
                    OutMessage = Erreur;
                }
                return OutMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static String GetConnectionString()
        {

            int InitApp = 0;
            string BaseDonnee = "";
            String Serveur = "";
            GetAppSettings(ref InitApp, ref Serveur, ref  BaseDonnee);
            sChaine = string.Format(sChaine, Serveur, BaseDonnee, Galatee.Security.Connexion.GalaUserId, Galatee.Security.Connexion.GalaUserPassword);
            return sChaine;
        }

        public static void GetAppSettings(ref int iAppInit, ref String sHost, ref String sBase)
        {
            String sPath = Assembly.GetExecutingAssembly().Location;
            System.Configuration.Configuration m_config = ConfigurationManager.OpenExeConfiguration(sPath);

            try
            {
                iAppInit = Convert.ToInt32(m_config.AppSettings.Settings["Application_Initialisee"].Value);
            }
            catch
            {
                iAppInit = 0;
            }
            try
            {
                sHost = Convert.ToString(m_config.AppSettings.Settings["HostName"].Value);
            }
            catch
            {
                sHost = "";
            }

            //try
            //{
            //    eModeCon = (eModeConnexion)Convert.ToInt32(m_config.AppSettings.Settings["ModeConnexion"].Value);
            //}
            //catch
            //{
            //    eModeCon = 0;
            //}
            try
            {
                sBase = Convert.ToString(m_config.AppSettings.Settings["Base"].Value);
            }
            catch
            {
                sBase = "";
            }

            //try
            //{
            //    sUser = Convert.ToString(m_config.AppSettings.Settings["UserName"].Value);
            //}
            //catch
            //{
            //    sUser = "";
            //}
            //try
            //{
            //    sPass = Convert.ToString(m_config.AppSettings.Settings["PassCrypt"].Value);
            //}
            //catch
            //{
            //    sPass = "";
            //}
        }

        public static bool OpenTransation(out SqlCommand CommandEnvoiDeFacture, out SqlTransaction TransactionEnvoiDeFacture, out SqlConnection ConnectionEnvoiDeFacture)
        {
            try
            {
                ConnectionEnvoiDeFacture = new SqlConnection(Utilitaire.GetConnectionString());
                ConnectionEnvoiDeFacture.Open();
                TransactionEnvoiDeFacture =
                    ConnectionEnvoiDeFacture.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                CommandEnvoiDeFacture = new SqlCommand();
                CommandEnvoiDeFacture = ConnectionEnvoiDeFacture.CreateCommand();
                CommandEnvoiDeFacture.CommandTimeout = 5 * 60;
                CommandEnvoiDeFacture.Transaction = TransactionEnvoiDeFacture;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool OpenTransation(out SqlCommand CommandEnvoiDeFacture, out SqlTransaction TransactionEnvoiDeFacture, string pConnectionEnvoiDeFacture)
        {
            try
            {
                var connectionEnvoiDeFacture = new SqlConnection(pConnectionEnvoiDeFacture);
                connectionEnvoiDeFacture.Open();
                TransactionEnvoiDeFacture =
                    connectionEnvoiDeFacture.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                CommandEnvoiDeFacture = new SqlCommand();
                CommandEnvoiDeFacture = connectionEnvoiDeFacture.CreateCommand();
                CommandEnvoiDeFacture.CommandTimeout = 5 * 60;
                CommandEnvoiDeFacture.Transaction = TransactionEnvoiDeFacture;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValiderTransation(bool pValide, ref SqlCommand CommandEnvoiDeFacture, ref SqlTransaction TransactionEnvoiDeFacture, ref SqlConnection ConnectionEnvoiDeFacture)
        {
            try
            {
                if (pValide)
                {
                    if (TransactionEnvoiDeFacture != null) TransactionEnvoiDeFacture.Commit();
                    if (ConnectionEnvoiDeFacture.State != ConnectionState.Closed)
                        ConnectionEnvoiDeFacture.Close();
                    return true;
                }
                if (TransactionEnvoiDeFacture != null) TransactionEnvoiDeFacture.Rollback();
                if (ConnectionEnvoiDeFacture.State != ConnectionState.Closed)
                    ConnectionEnvoiDeFacture.Close();
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValiderTransation(bool pValide, ref SqlCommand CommandEnvoiDeFacture, ref SqlTransaction TransactionEnvoiDeFacture)
        {
            try
            {
                if (pValide)
                {
                    if (TransactionEnvoiDeFacture != null) TransactionEnvoiDeFacture.Commit();
                    if (CommandEnvoiDeFacture != null && CommandEnvoiDeFacture.Connection.State != ConnectionState.Open)
                        CommandEnvoiDeFacture.Connection.Close();
                    return true;
                }
                if (TransactionEnvoiDeFacture != null) TransactionEnvoiDeFacture.Rollback();
                if (CommandEnvoiDeFacture != null && CommandEnvoiDeFacture.Connection.State != ConnectionState.Open)
                    CommandEnvoiDeFacture.Connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static String Compress(FileInfo fileToCompress)
        {
            String pFichier="";
            try
            {

                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                    {
                        using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                                pFichier = fileToCompress.DirectoryName + "\\" + string.Format("{0}.zip", fileToCompress.FullName);
                                //Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                //    fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                            }
                        }
                    }
                }

                return pFichier;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }


        }

        public static String CompressionFichier(String pFichier, int pCapaciteReception)
        {

            try
            {
                //GC.Collect();
                if (File.Exists(pFichier))
                {
                    var infofile = new FileInfo(pFichier);
                    var directoryPath = infofile.DirectoryName;
                    string sNomFichier = infofile.Name;
                    var sCapacite = infofile.Length;
                    var sExt = infofile.Extension;
                    var sNomFichierZip = sNomFichier.Replace(sExt, "");
                    sCapacite = sCapacite / 1024;
                    if (Convert.ToInt32(sCapacite) >= pCapaciteReception)
                    {
                        //pFichier= Compress(infofile);
                        using (var s = new ZipOutputStream(File.Create(String.Format("{0}\\{1}.zip", directoryPath, sNomFichierZip))))
                        {
                            s.SetLevel(9);
                            var buffer = new byte[4096];
                            var entry = new ZipEntry(sNomFichierZip + sExt) { DateTime = DateTime.Now };
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(pFichier))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                }
                                while (sourceBytes > 0);

                                fs.Close();
                                fs.Dispose();

                            }

                            s.Finish();
                            s.Close();
                            s.Dispose();
                            pFichier = infofile.DirectoryName + "\\" + string.Format("{0}.zip", sNomFichierZip);
                        }
                    }
                }
                return pFichier;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //GC.Collect();
            }
        }
       
        private String CompressionDossier(String pFichier)
        {
            try
            {
                if (Directory.Exists(pFichier) == true)
                {
                    //System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream();
                    using (var s = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(String.Format("{0}.zip", pFichier))))
                    {
                        s.SetLevel(9);
                        var buffer = new byte[4096];
                        var entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(pFichier) { DateTime = DateTime.Now };
                        s.PutNextEntry(entry);
                        using (StreamReader fs = new StreamReader(pFichier))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read();
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }
                        s.Finish();
                        s.Close();
                        s.Dispose();
                        pFichier = string.Format("{0}.zip", pFichier);
                    }
                    //}
                }
                return pFichier;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetDBNullParametre(SqlParameterCollection parameters)
        {
            foreach (SqlParameter Parameter in parameters)
            {
                if (Parameter.Value == null)
                {
                    Parameter.Value = DBNull.Value;
                }
            }
        }

    }


    public enum StatutEnvoiSms
    {
        NonEnvoye = 0,
        Envoye
    };

    public enum EtapeProcessus
    {
        Etape1 = 1,
        Etape2,
        Etape3
    };
}
