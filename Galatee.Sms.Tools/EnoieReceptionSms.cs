using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace Galatee.Sms.Tools
{
    public class EnoieReceptionSms
    {
        private const char Crf = '\r';
        private SerialPort port;
        private IList<ShortMessage> _mMessagesLus;
        private static readonly Object _thisLock = new Object();
        private const string debutCmdAt = "AT";
        private const string debutCmdAtCmgf = "AT+CMGF=1";
        private const string debutCmdAtCmgl = "AT+CMGL=\"ALL\"";
        private const string debutCmdAtCmgd = "AT+CMGD";


        public IList<ShortMessage> MMessagesLus
        {
            get { return _mMessagesLus; }
        }

        public EnoieReceptionSms()
        {
            port = new SerialPort();
        }

        private void ParseMessages(string input)
        {
            try
            {
                IList<ShortMessage> messages = new List<ShortMessage>();
                Regex r = new Regex(@"\+CMGL: (\d+),""(.+)"",""(.+)"",(.*),""(.+)""\r\n(.+)\r\n");
                Match m = r.Match(input);
                while (m.Success)
                {
                    ShortMessage msg = new ShortMessage();
                    msg.Index = int.Parse(m.Groups[1].Value);
                    msg.Status = m.Groups[2].Value;
                    msg.Sender = m.Groups[3].Value;
                    msg.Alphabet = m.Groups[4].Value;
                    msg.Sent = m.Groups[5].Value;
                    msg.Message = m.Groups[6].Value;
                    messages.Add(msg);
                    m = m.NextMatch();
                }
                _mMessagesLus = messages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #region "Communication"

        public void InitiPort(string portName)
        {
            try
            {
                port.PortName = portName;
                port.BaudRate = 115200;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.ReadTimeout = 300;
                port.WriteTimeout = 300;
                //port.Encoding = Encoding.GetEncoding("iso-8859-1");
                //port.Encoding = Encoding.ASCII;
                port.Encoding = Encoding.UTF8;

                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        public void InitiPort(string portName, int pBaudeRate, int pDataBit)
        {
            try
            {
                port.PortName = portName;
                port.BaudRate = pBaudeRate;
                port.DataBits = pDataBit;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.ReadTimeout = 300;
                port.WriteTimeout = 300;
                ////port.Encoding = Encoding.GetEncoding("iso-8859-1");
                //port.Encoding = Encoding.ASCII;
                port.Encoding = Encoding.UTF8;

                return;
            }
            catch (Exception)
            {
                return;
            }

        }

        private void OuvrirPort()
        {
            try
            {
                if (port.IsOpen)
                    return;

                port.Open();
                port.DtrEnable = true;
                port.RtsEnable = true;
            }
            catch
            {
                return;
            }
        }

        public void ClosePort()
        {
            try
            {
                if (port.IsOpen)
                    port.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private String ReadResponse(int timeout)
        {
            string buffer = string.Empty;
            try
            {
                int maxWait = timeout;
                int iTry = 1;
                int iWaitTime = 0;
                TimeSpan waitSpan = new TimeSpan(0, 0, 0, 0, 10);
                while (iWaitTime <= maxWait)
                {
                    try
                    {
                        buffer += port.ReadExisting();

                        if (buffer.EndsWith("\r\nOK\r\n") || buffer.EndsWith("\r\nERROR\r\n"))
                        {
                            return buffer;
                        }
                        System.Threading.Thread.Sleep(waitSpan);
                        iWaitTime = iTry*10;
                        iTry++;
                    }
                    catch (TimeoutException)
                    {
                    }
                }
                buffer = "";
                return buffer;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string ExecCommand(string command, int responseTimeout, ref CErreurAppli erreurAppli)
        {
            try
            {
                port.DiscardOutBuffer();
                port.DiscardInBuffer();
                port.Write(command + Crf);

                string input = ReadResponse(responseTimeout);
                if ((input.Length == 0) || (!input.EndsWith("\r\nOK\r\n")))
                    erreurAppli.Set(eCodeErreur.ECHEC_LECTURE_SMS);
                else
                {
                    erreurAppli.Set(eCodeErreur.NO_ERROR);
                }
                return input;
            }
            catch (Exception ex)
            {
                erreurAppli.Set(eCodeErreur.ECHEC_LECTURE_SMS, ex.Message);
                return "";
            }
        }       

        //private Boolean ReadSms(ref CErreurAppli erreurAppli, TraitementSMS.TraitmentSms mTraitementSMS)
        //{
        //    try
        //    {
        //        string input = "";
        //        int iWaitTime =0;

        //        if (port == null)
        //        {
        //            erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
        //            return false;
        //        }
               
        //        IList<ShortMessage> messages = null;
        //        try
        //        {
        //            OuvrirPort();
        //            if (port.IsOpen == false)
        //            {
        //                erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
        //                return false;
        //            }
					
        //            iWaitTime = 300;
        //            ExecCommand("AT", iWaitTime, ref erreurAppli);
        //            if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
        //            {
        //                erreurAppli.Set(eCodeErreur.AUCUN_MODEM_CONNECTE_AU_PORT);
        //                return false;
        //            }

        //            iWaitTime = 500;
        //            ExecCommand("AT+CMGF=1", iWaitTime, ref erreurAppli);
        //            if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
        //            {
        //                erreurAppli.Set(eCodeErreur.ECHEC_MODIFICATION_FORMAT_SMS);
        //                return false;
        //            }

        //            iWaitTime = 5000;
        //           //input = ExecCommand("AT+CMGL=\"REC UNREAD\"", iWaitTime, ref erreurAppli);
        //            input = ExecCommand("AT+CMGL=\"ALL\"", iWaitTime, ref erreurAppli);
        //            if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
        //            {
        //                return false;
        //            }

        //            ParseMessages(input);
        //            iWaitTime = 500;
        //            CErreurAppli erreurDelete=new CErreurAppli();

        //            //Traitement Enregistrement et supression SMS:
        //            if (_mMessagesLus.Count != 0)
        //            {
        //                foreach (ShortMessage shortMessage in _mMessagesLus)
        //                {

        //                    //DateTime mDateSent = Convert.ToDateTime(shortMessage.Sent,mCuture);
        //                    DateTime mDateSent = DateTime.Now; // Voir comment formter la date du modem et recupérer cette date
        //                    erreurAppli = mTraitementSMS.EnregistrerSMS(shortMessage.Sender, mDateSent, shortMessage.Message);

        //                    if (erreurAppli.m_eCode == eCodeErreur.NO_ERROR)
        //                        input = ExecCommand(String.Format("AT+CMGD={0}", shortMessage.Index), iWaitTime,
        //                                            ref erreurDelete);
        //                }
        //            }
        //            //Fin Traitement

        //            //  if( _mMessagesLus.Count != 0)
        //            //{
        //            //    foreach (ShortMessage shortMessage in _mMessagesLus)
        //            //    {
        //            //        input = ExecCommand(String.Format("AT+CMGD={0}", shortMessage.Index), iWaitTime, ref erreurDelete);
        //            //    }
        //            //}
               
        //            erreurAppli.Set(eCodeErreur.NO_ERROR);
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            ClosePort();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //Inscrire l'erreur dans
        //        erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
        //        return false;
        //    }
        //}

        private Boolean ReadSms(ref CErreurAppli erreurAppli)
        {
            try
            {
                string input = "";
                int iWaitTime = 0;

                if (port == null)
                {
                    erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
                    return false;
                }

                IList<ShortMessage> messages = null;
                try
                {
                    OuvrirPort();
                    if (port.IsOpen == false)
                    {
                        erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
                        return false;
                    }

                    iWaitTime = 300;
                    ExecCommand(debutCmdAt, iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        erreurAppli.Set(eCodeErreur.AUCUN_MODEM_CONNECTE_AU_PORT);
                        return false;
                    }

                    iWaitTime = 500;
                    ExecCommand(debutCmdAtCmgf, iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        erreurAppli.Set(eCodeErreur.ECHEC_MODIFICATION_FORMAT_SMS);
                        return false;
                    }

                    iWaitTime = 5000;
                    //input = ExecCommand("AT+CMGL=\"REC UNREAD\"", iWaitTime, ref erreurAppli);
                    input = ExecCommand(debutCmdAtCmgl, iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        return false;
                    }

                    ParseMessages(input);
                    iWaitTime = 500;
                    CErreurAppli erreurDelete = new CErreurAppli();

                    //Traitement Enregistrement et supression SMS:
                    if (_mMessagesLus.Count != 0)
                    {
                        foreach (ShortMessage shortMessage in _mMessagesLus)
                        {

                            //DateTime mDateSent = Convert.ToDateTime(shortMessage.Sent,mCuture);
                            DateTime mDateSent = DateTime.Now; // Voir comment formter la date du modem et recupérer cette date
                            //erreurAppli = mTraitementSMS.EnregistrerSMS(shortMessage.Sender, mDateSent, shortMessage.Message);

                            if (erreurAppli.m_eCode == eCodeErreur.NO_ERROR)
                                input = ExecCommand(String.Format("{0}={1}",debutCmdAtCmgd, shortMessage.Index), iWaitTime,
                                                    ref erreurDelete);
                        }
                    }
                    //Fin Traitement

                    //  if( _mMessagesLus.Count != 0)
                    //{
                    //    foreach (ShortMessage shortMessage in _mMessagesLus)
                    //    {
                    //        input = ExecCommand(String.Format("AT+CMGD={0}", shortMessage.Index), iWaitTime, ref erreurDelete);
                    //    }
                    //}

                    erreurAppli.Set(eCodeErreur.NO_ERROR);
                    return true;
                }
                catch (Exception ex)
                {
                    erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
                    return false;
                }
                finally
                {
                    ClosePort();
                }

            }
            catch (Exception ex)
            {
                //Inscrire l'erreur dans
                erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
                return false;
            }
        }

        private Boolean SendSms(ref CErreurAppli erreurAppli, String pPhoneNumber, String pMessageToSend, out String sRetour)
        {
            sRetour = "";
            int iWaitTime = 0;
            try
            {
                if (port == null)
                {
                    erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
                    return false;
                }

                try
                {
                    OuvrirPort();
                    if (port.IsOpen == false)
                    {
                        erreurAppli.Set(eCodeErreur.MODEM_NON_INITIALISE);
                        return false;
                    }

                    iWaitTime = 300;
                    ExecCommand("AT", iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        erreurAppli.Set(eCodeErreur.AUCUN_MODEM_CONNECTE_AU_PORT);
                        return false;
                    }

                    iWaitTime = 500;
                    ExecCommand("AT+CMGF=1", iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        erreurAppli.Set(eCodeErreur.ECHEC_MODIFICATION_FORMAT_SMS);
                        return false;
                    }

                    //Envoie de la première commande: numéro de téléphone
                    iWaitTime = 300;
                    ExecCommand("AT+CMGS=" + pPhoneNumber, iWaitTime, ref erreurAppli);

                    //Envoie de la première commande: message
                   
                    iWaitTime = 60*1000;
                   
                    ExecCommand(pMessageToSend + (char) 26, iWaitTime, ref erreurAppli);
                    if (erreurAppli.eCode != eCodeErreur.NO_ERROR)
                    {
                        erreurAppli.Set(eCodeErreur.ECHEC_ENVOIE_SMS);
                        return false;
                    }
                    erreurAppli.Set(eCodeErreur.NO_ERROR);
                    return true;
                }
                catch (Exception ex)
                {
                    erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
                    return false;
                }
                finally
                {
                    ClosePort();
                }

            }
            catch (Exception ex)
            {
                erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
                sRetour = ex.Message;
                return false;
            }
        }

        public Boolean EnvoieReceptionSynchrone(ref CErreurAppli erreurAppli, Boolean isLecture, String pPhoneNumber, String pMessageToSend, out String sRetour)
        {
            sRetour = "";
            try
            {
                lock (_thisLock)
                {
                    return isLecture ? ReadSms(ref erreurAppli) : SendSms(ref erreurAppli, pPhoneNumber, pMessageToSend, out sRetour);
                }

            }
            catch (Exception ex)
            {
                erreurAppli.Set(eCodeErreur.ERROR, ex.Message);
                return false;
            }
        }

        #endregion
    }
}
