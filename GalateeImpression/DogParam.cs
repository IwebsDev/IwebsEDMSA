using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace GalateeImpression
{
    public class DogParam
    {
        private string _directory;
        //System.IO.FileStream m_streams = new System.IO.FileStream(@"C:\TEMP\Test.Bmp", System.IO.FileMode.Open, System.IO.FileAccess.Read);
        private static FileSystemWatcher _fsw = new FileSystemWatcher();

        public string Directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        public DogParam(string directory)
        {
            try
            {
                _directory = directory;
                _fsw.Path = _directory;
                _fsw.Filter = "*.PDF*";
                _fsw.NotifyFilter =  NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            
                _fsw.Created  += new FileSystemEventHandler(_fsw_Created);
                _fsw.EnableRaisingEvents = true;
                //processStarterLog.writeLog(" chemin"+_fsw.Path);
            }
            catch (Exception ex)
            {
                //processStarterLog.writeLog(ex.Message);
            }

        }

        DogParam()
        {
        }

       

        void _fsw_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                //MyTimer(5);
                //PrintFormPdfData(e.FullPath);

                bool ok = false;
                while (!ok)
                    ok = FichierEstLibre(e.FullPath);
                PrintFormPdfData(e.FullPath);
            }
            catch (Exception ex)
            {
                processStarterLog.writeLog(ex.Message);
            }
        }



        private bool FichierEstLibre(string fichier)
        {
            try
            {
                FileStream fs;
                fs = File.Open(fichier, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                if (fs != null)
                {
                    fs.Close();
                    return true;
                }
                else
                    return false;
            }
            catch (IOException)
            {
                return false;
            }
        }


        private void MyTimer(int secondes)
        {
            DateTime tempsdeb = DateTime.Now;
            TimeSpan diffTemps = new TimeSpan();
            diffTemps = DateTime.Now - tempsdeb;
            while (diffTemps.Seconds < secondes)
            {
                diffTemps = DateTime.Now - tempsdeb;
            }
        }

 

       ProcessStarter processStarterLog = new ProcessStarter();
        public static void EcrireFichier(string Message, string CheminLog)
        {

            string Buffer = "";
            FileInfo Fichier = new FileInfo(CheminLog);

            if (Fichier.Exists) // on verifie ke le fichier existe
            {
                StreamReader Lecture = new StreamReader(CheminLog, ASCIIEncoding.Default); // on ouvre le fichier
                Buffer = Lecture.ReadToEnd(); // on met la totalité du fichier dans une variable
                Lecture.Close(); // on ferme
            }

            if (Buffer == null || Buffer == "") // on verifie si y a kelke chose dans le fichier, si oui...
            {
                StreamWriter Ecriture = new StreamWriter(CheminLog, false, ASCIIEncoding.Default); // le boolean à false permet d'écraser le fichier existant
                Ecriture.Write(Message + "\r\n"); // on écrit la variable et sa valeur
                Ecriture.Close(); // on ferme
            }
            else // si non...
            {
                StreamWriter Ecriture = new StreamWriter(CheminLog, true, ASCIIEncoding.Default); // le boolean à false permet d'ajouter un ligne sans écraser le fichier
                Ecriture.Write(Message + "\r\n"); // on ajoute la variable plus la valeur (un saut a la ligne avant)
                Ecriture.Close(); // on ferme
            }
        }

        private void PrintFormPdfData(string tempFile)
        {

            try
            {
            
            //processStarterLog.writeLog("debut");
            byte[] formPdfData = System.IO.File.ReadAllBytes(tempFile);
            //processStarterLog.writeLog("transform bit");
            using (FileStream fs = new FileStream(tempFile, FileMode.Create))
            {
                fs.Write(formPdfData, 0, formPdfData.Length);
                fs.Flush();
                //processStarterLog.writeLog("fin filestream");
                fs.Close();
            }
            //string pdfArguments = string.Format("/h /p {0} \"Printer name\"", tempFile);
            string pdfArguments = String.Format(@"/p /h {0}", tempFile);

            /*
                / N - Lancement d'une nouvelle instance de lecteur même si une autre est déjà ouverte
                / S - Ne pas afficher l'écran de démarrage
                / O - Ne pas afficher la boîte de dialogue de fichier ouvert
                / H - Ouvrir comme une fenêtre réduite
                / P <filename> - Ouvrir et aller directement à la boîte de dialogue d'impression
                / T <nom du fichier> <nom de l'imprimante> <drivername> <portname> - Imprimer le fichier sur l'imprimante spécifiée.

            */
            processStarterLog.writeLog(tempFile);
            //string pdfPrinterLocation = @"C:\Program Files (x86)\Adobe\Reader 9.0\Reader\AcroRd32.exe";
            //processStarterLog.writeLog("appel de adobe");
            string pdfPrinterLocation = Microsoft.Win32.Registry.LocalMachine
                                                         .OpenSubKey("SOFTWARE")
                                                         .OpenSubKey("Microsoft")
                                                          .OpenSubKey("Windows")
                                                         .OpenSubKey("CurrentVersion")
                                                         .OpenSubKey("App Paths")
                                                          .OpenSubKey("AcroRd32.exe")
                                                        .GetValue(String.Empty).ToString();

                    using (ProcessStarter processStarter = new ProcessStarter("AcroRd32", pdfPrinterLocation, pdfArguments))
                    {
                        processStarter.Run();
                        MyTimer(10);
                        //processStarter.WaitForExit();
                        processStarter.Stop();
                        processStarter.KillAdobe(pdfPrinterLocation);
                        //FermerAdobe();
                        //processStarterLog.writeLog("fin fermeture");
                    }
            }
            catch (Exception ex)
            {
                 processStarterLog.writeLog("Exception PrintFormPdfData : " + ex.Message);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }







        private static void FermerAdobe()
        {
            try
            {
                Process[] allProcs = Process.GetProcesses();

                foreach (Process pr in allProcs)
                {
                    if (pr.ProcessName == "AcroRd32")
                        pr.Kill();
                }                
            }
            catch
            {
            }
        }











        private void PrintPDFs(string pdfFileName)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.Verb = "runas";
                proc.StartInfo.Verb = "print";

                //Define location of adobe reader/command line
                //switches to launch adobe in "print" mode
                proc.StartInfo.FileName =  @"C:\Program Files (x86)\Adobe\Reader 11.0\Reader\AcroRd32.exe";
                proc.StartInfo.Arguments = String.Format(@"/p /h {0}", pdfFileName);
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();

                proc.WaitForExit(10000);

                if (proc.HasExited == false)
                {
                    proc.EnableRaisingEvents = true;
                    proc.Close();
                    KillAdobe("AcroRd32");
                }

            }
            catch (Exception ex)
            {
                processStarterLog.writeLog(ex.Message);
            }
            finally
            {
                File.Delete(pdfFileName);
            }

        }

        //For whatever reason, sometimes adobe likes to be a stage 5 clinger.
        //So here we kill it with fire.
        private static bool KillAdobe(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses().Where(
                         clsProcess => clsProcess.ProcessName.StartsWith(name)))
            {
                clsProcess.Kill();
                return true;
            }
            return false;
        }





    }
}

