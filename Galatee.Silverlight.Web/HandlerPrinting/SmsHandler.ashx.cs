using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Galatee.Silverlight.Web.HandlerPrinting
{
    /// <summary>
    /// Summary description for SmsHandler
    /// </summary>
    public class SmsHandler : IHttpHandler
    {
        bool IsEmail = false;
        bool IsSms = false;

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            context.Response.Write("");
            int bytelength = context.Request.ContentLength;
            byte[] inputbytes = context.Request.BinaryRead(bytelength);
            string message = System.Text.Encoding.UTF8.GetString(inputbytes);
            // Toutes les clés sont concatenées dans "message",
            // il faut donc les separer et utliser chaque clé pour envoyer la facture
            //string[] TousLesParametres = message.Split('/');
            //foreach (string chaine in TousLesParametres)
            //{
                string[] Unparametre = message.Split('|');
                try
                {
                    if (!string.IsNullOrWhiteSpace(Unparametre[1]))
                    {
                        string message_sms = Unparametre[0];
                        message_sms = HttpUtility.UrlEncode(message_sms, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                        string msgerreur = "Un problème est survenu,veillez rééssayer svp";
                        SendSampleSMS(message_sms, "inova01 ", "zAag2014", "INOVA SI", Unparametre[1], ref msgerreur);
                        context.Response.Write("400");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            //}
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        public bool SendSampleSMS(String MessageSMS, string Username, string password, string sender,
            string mobile, ref String errorMsg)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("http://api.infobip.com/api/v3/sendsms/plain?user=" + Username + "&password=" + password +
                    "&sender=" + sender + "&SMSText=" + MessageSMS + "&GSM=" + mobile);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                //Console.WriteLine(result);
                result = result.Replace('\n', ' ');
                sr.Close();
                myResponse.Close();

                errorMsg = String.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }
    }
}