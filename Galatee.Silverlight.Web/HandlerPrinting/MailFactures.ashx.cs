using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Net.Mail;
using System.Net;
using System.IO;
using Galatee.Silverlight.Web.ServicePrintings;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Galatee.Silverlight.Web.HandlerPrinting
{
    /// <summary>
    /// Description résumée de MailFactures
    /// </summary>
    public class MailFactures : IHttpHandler
    {
        bool IsEmail = false;
        bool IsSms = false;

        public void ProcessRequest(HttpContext context)
        {
            PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());
            string retourMail = "Echec envoi mail";

            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            string message = System.Text.Encoding.UTF8.GetString(inputbytes);
            // Toutes les clés sont concatenées dans "message",
            // il faut donc les separer et utliser chaque clé pour envoyer la facture
            string[] TousLesParametres = message.Split('/');


            foreach (string chaine in TousLesParametres)
            {

                string[] Unparametre = chaine.Split('|');
                SessionObject.machine = Unparametre[3];
                SessionObject.portService = Unparametre[4];
                string from =  Unparametre[5];
                string subject =  Unparametre[6];
                string body =  Unparametre[7];
                string smtp =  Unparametre[8];
                string portSmtp =  Unparametre[9];
                string sslEnabled =  Unparametre[10];
                string login =  Unparametre[11];
                string password =  Unparametre[12];
                
                Utilitys objetUtil = new Utilitys();
                string filePath = objetUtil.LaunchPDFgeneration(false, Unparametre[1], Unparametre[0], Unparametre[2]);
                if (objetUtil.parameter != null)
                    {
                        if (objetUtil.parameter.ContainsKey("pismail"))
                        {

                            try
                            {
                                //objetUtil.LaunchPrinting(false, Unparametre[1], string.Empty, Unparametre[0], Unparametre[2], true);
                   
                                        if (!string.IsNullOrWhiteSpace(objetUtil.parameter["pismail"]))
                                        {
                                            printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Debut lancement Mail");

                                            //SendMail(objetUtil.parameter["pismail"], filePath);
                                            retourMail = EnvoiMail(objetUtil.parameter["pismail"], filePath, from, subject, body, smtp, portSmtp, sslEnabled, login, password);

                                            printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Fin lancement Mail");

                                        }
                                       
                    
                            }
                            catch (Exception ex)
                            {
                                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), retourMail + " " + ex.Message);
                                throw ex;
                            }
                        }
                    }
            }
            printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), retourMail);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void SendMailTest(string address,string filePath)
        {
            string MonAdresse = "GalateeTeam@inova.ci";
            MailAddress from = new MailAddress (MonAdresse);
            MailAddress to = new MailAddress (address);
            MailMessage mailMsg = new MailMessage(from,to);

            mailMsg.Subject = "Votre facture d'electricité";
            mailMsg.Attachments.Add(new Attachment(filePath));
            mailMsg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";

            SmtpClient client = new SmtpClient("iv-srv-003");
            client.Send(mailMsg);
        }
        //private void SendMail(string address, string filePath)
        //{
        //    //string MonAdresse = "GalateeTeam@inova.ci";
        //    string MonAdresse = "syllaibrahimbenyaya@gmail.com";
        //    MailAddress from = new MailAddress(MonAdresse);
        //    MailAddress to = new MailAddress(address);
        //    MailMessage mailMsg = new MailMessage(from, to);

        //    mailMsg.Subject = "Votre facture d'electricité";
        //    mailMsg.Attachments.Add(new Attachment(filePath));
        //    mailMsg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";

        //    ////SmtpClient client = new SmtpClient("iv-srv-003");
        //    ////SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        //    //SmtpClient smtp = new SmtpClient();
        //    //smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
        //    //smtp.Port = 587;

        //    //smtp.Credentials = new System.Net.NetworkCredential("ludkonan@gmail.com", "kouakouasso");
        //    ////smtp.Credentials = new System.Net.NetworkCredential("ibrahimbenyayasylla@gmail.com", "algoman03");
        //    //smtp.EnableSsl = true;
        //    //smtp.Send(mailMsg);




        //    //SmtpClient stpc = new SmtpClient("smtp.gmail.com", 587);
        //    SmtpClient stpc = new SmtpClient("smtp.gmail.com", 465);
        //    stpc.Credentials = new System.Net.NetworkCredential("syllaibrahimbenyaya@gmail.com", "algoman03");
        //    stpc.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    stpc.EnableSsl = true;
        //    stpc.UseDefaultCredentials = true;
        //    stpc.Send(mailMsg);

        //    //client.Send(mailMsg);
        //}
        //public string SendMail(string address, string filePath)
        //{
        //    MailMessage msg = new MailMessage();

        //    msg.From = new MailAddress("syllaibrahimbenyaya@gmail.com");
        //    msg.To.Add(address);
        //    msg.Subject = "Hello world! " + DateTime.Now.ToString();
        //    msg.Body = "hi to you ... :)";
        //    msg.Attachments.Add(new Attachment(filePath));

        //    SmtpClient client = new SmtpClient();
        //    client.UseDefaultCredentials = true;
        //    client.Host = "smtp.live.com";
        //    client.Port = 465;
        //    client.EnableSsl = false;
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //client.Credentials = new NetworkCredential("lesyllaibrahimbenyaya@hotmail.fr", "algoman03");
        //    client.Timeout = 20000;
        //    try
        //    {
        //        client.Send(msg);
        //        return "Mail has been successfully sent!";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Fail Has error" + ex.Message;
        //    }
        //    finally
        //    {
        //        msg.Dispose();
        //    }
        //}
        private void SendMail(string address, string filePath)
        {
            PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());

            try
            {

                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Envoi Mail debut");

                //String userName = "iwebsmail@edm-sa.com.ml";
                //String password = "projetiwebs";

                String userName = "s2syllab@inova.ci";
                String password = "P@$$w0rd";

                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(address));
                msg.From = new MailAddress(userName);
                msg.Subject = "Votre facture d'electricité";
                msg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";
                msg.IsBodyHtml = true;

                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Fichier joint debut");
                msg.Attachments.Add(new Attachment(filePath));
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Fichier joint fin");


                //msg.Subject = "Test Office 365 Account";
                //msg.Body = "Testing email using Office 365 account.";


                //SmtpClient smtpServer = new SmtpClient("10.100.2.56");
                //smtpServer.Port = 25;
                //smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpServer.UseDefaultCredentials = true;
                //smtpServer.Credentials = new System.Net.NetworkCredential(userName, password);
                //smtpServer.Timeout = 900;
                //smtpServer.EnableSsl = false;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.office365.com";
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.Port = 587;
                client.EnableSsl = true;
                client.Timeout = 300;
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Envoi Mail ok Debut");

                client.Send(msg);
                //smtpServer.Send(msg);
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), "Envoi Mail ok Fin");
            }
            catch (Exception ex)
            {
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                
            }

        }





        public string EnvoiMail(string adresse, string file, string from, string subject, string body, string smtp, string portSmtp, string sslEnabled, string login, string password)
        {
            PrintingsServiceClient printError = new PrintingsServiceClient(new Utilitys().Protocole(), new Utilitys().EndPoint());

            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = null;

/*                string from = System.Configuration.ConfigurationManager.AppSettings["MailSender"];
                string subject = System.Configuration.ConfigurationManager.AppSettings["MailSubject"];
                string body = System.Configuration.ConfigurationManager.AppSettings["MailBody"];
                string smtp = System.Configuration.ConfigurationManager.AppSettings["MailSmtp"];
                string port = System.Configuration.ConfigurationManager.AppSettings["MaMailPortilSmtp"];
                string sslEnabled = System.Configuration.ConfigurationManager.AppSettings["MailSslEnabled"];
                string login = System.Configuration.ConfigurationManager.AppSettings["MailLogin"];
                string password = System.Configuration.ConfigurationManager.AppSettings["MailPassword"];
                */

                mail.Subject = subject;
                mail.Body = body;

                mail.IsBodyHtml = true;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.From = new MailAddress(from);


                if (IsEMailAddress(adresse))
                    mail.To.Add(adresse);


                Attachment maPieceJointe = new Attachment(file);
                mail.Attachments.Add(maPieceJointe);

                if (!string.IsNullOrEmpty(smtp))
                    smtpServer = new SmtpClient(smtp);
                if (!string.IsNullOrEmpty(portSmtp))
                    smtpServer.Port = int.Parse(portSmtp);
                if (!string.IsNullOrEmpty(sslEnabled))
                    smtpServer.EnableSsl = bool.Parse(sslEnabled);

                smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpServer.UseDefaultCredentials = true;
                smtpServer.Credentials = new System.Net.NetworkCredential(login, password);

                smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                smtpServer.SendAsync(mail, mail);

                return "Transmission mail OK";

            }
            catch (Exception ex)
            {
                printError.SetErrorsFromSilverlightWebPrinting(GetCurrentMethod(), ex.Message);
                return "Echec transmission mail " + ex.Message;

            }
        }


        private static void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;
            mail.Dispose();
        }

        private static bool IsEMailAddress(string s)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(s, "^([\\w-]+\\.)*?[\\w-]+@[\\w-]+\\.([\\w-]+\\.)*?[\\w]+$");
        }





        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(2);

            return sf.GetMethod().Name;
        }
















    }
}