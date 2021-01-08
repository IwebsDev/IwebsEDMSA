using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Galatee.WorkflowManager
{

    public sealed class CodeEnvoyerEmail : CodeActivity<bool>
    {
        
        #region In Parametres

        public InArgument<string> Emails { get; set; }
        public InArgument<string> Subject { get; set; }
        public InArgument<string> Body { get; set; }

        #endregion

        #region Out Parametres

        public OutArgument<string> result { get; set; }

        #endregion

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override bool Execute(CodeActivityContext context)
        {
            try
            {
                string to = context.GetValue<string>(Emails);
                string subject = context.GetValue<string>(Subject);
                string body = context.GetValue<string>(Body);

                WKFSmtpInfo smtpInfo = WKFManager.GetInfoServeurDeMail();

                var mailMessage = new System.Net.Mail.MailMessage();
                mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.From = new System.Net.Mail.MailAddress(smtpInfo.From);
                var smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = smtpInfo.Host;
                smtp.Credentials = new System.Net.NetworkCredential(smtpInfo.UserName, smtpInfo.Password);
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
                result.Set(context, "Envoi de mail réussi!");
                return true;
            }
            catch (Exception ex)
            {
                result.Set(context, ex.Message);
                return false;
            }
        }
    }
}
