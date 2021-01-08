using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Hosting;

namespace WcfService
{
    [DataContract]
    public class Errors
    {
        [DataMember]
        public int ErrorNumber { get; set; }

        [DataMember]
        public string ErrorLabel { get; set; }
    }

    public static class ErrorManager
    {
        public static void LogException(object pModule, Exception ex)
        {
            try
            {
                if (ex.InnerException != null)
                    LogException(pModule, ex.InnerException);
                ErrorManager.WriteInLogFile(pModule, ex.Message);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void WriteInLogFile(object pModule, string pError)
        {
            try
            {
                pError = DateTime.Now.ToString() + " | " + GetCurrentMethod()+ " | "+ pError;
                WriteInLogFile(pModule.GetType().Name, pError,HostingEnvironment.MapPath("~/App_Data"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteInLogFileFromWeb(object pModule, string methodName, string pError)
        {
            try
            {
                pError = DateTime.Now.ToString() + " | " + methodName + " | " + pError;
                WriteInLogFile(pModule.GetType().Name, pError, HostingEnvironment.MapPath("~/App_Data"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(3); // 3 correspond au niveau de la méthode dans le StackTrace

            return sf.GetMethod().Name;
        }

        private static string CreateTraceFile(string pModule, string pChemin)
        {
            try
            {
                var info = new FileInfo(pChemin);
                if (info.DirectoryName != null)
                {
                    string sPathSuivi = pChemin + string.Format(@"\LOG GALATEE\{0}", pModule);
                    if (Directory.Exists(sPathSuivi) == false)
                        Directory.CreateDirectory(sPathSuivi);
                    var pCheminFichierLog = String.Format(sPathSuivi + @"\Log_Galatee_Du_{0}.txt", DateTime.Now.ToString("ddMMyyyy"));
                    if (File.Exists(pCheminFichierLog) == false)
                    {
                        File.AppendAllText(pCheminFichierLog,"");
                    }
                    return pCheminFichierLog;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                //return string.Empty;
                throw ex;
            }
        }

        public static void WriteInLogFile(string pModule, string pError, string pChemin)
        {
            try
            {
                var cheminFichierLog = CreateTraceFile(pModule, pChemin);
                if (!string.IsNullOrEmpty(cheminFichierLog))
                {
                    WriteLog(pError, cheminFichierLog);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void WriteLog(string message, string pCheminFichier)
        {
            try
            {
                var fileStream = new FileStream(pCheminFichier, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                var sb = new StringBuilder();
                using (var w = new StreamReader(fileStream))
                {
                    sb.Append(w.ReadToEnd());
                }
                fileStream.Close();
                fileStream.Dispose();
                var fileStream2 = new FileStream(pCheminFichier, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                using (var w = new StreamWriter(fileStream2))
                {
                    sb.AppendLine(message);
                    w.WriteLine(sb.ToString());
                }
                fileStream2.Close();
                fileStream2.Dispose();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
