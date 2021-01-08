using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Galatee.Structure;
using System.Collections;
using System.ServiceModel.Activation;
using System.IO;
using System.Drawing;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Windows.Markup.Localizer;
using System.Threading;
namespace WcfService.Printings
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "PrintingsService" à la fois dans le code, le fichier svc et le fichier de configuration.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class PrintingsService : IPrintingsService
    {

        #region Sylla 11-05-2017
        public void ActionMail<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string matricule, string serveur, string port, string PortWeb, string from, string subject, string body, string smtp, string portSmtp, string sslEnabled, string login, string password) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            string key = getKey(matricule);
            //loaderHandler = LoadingManager.BeginLoading("Envoi en cours... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Galatee.Tools.Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Structure.CsPrint> list = new List<Galatee.Structure.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Structure.CsPrint);

            //PrintingsServiceClient service = new PrintingsServiceClient(Utility.Protocole(), Utility.EndPoint("Printing"));
            setFromWebPart(list, key, param);
            SendToMailHandler(rdlcUri, moduleNmae, key, serveur, port, PortWeb, from, subject, body, smtp, portSmtp, sslEnabled, login, password);


        }

        static void SendToMailHandler(string rdlcUri, string moduleNmae, string key, string serveur, string port, string PortWeb, string from, string subject, string body, string smtp, string portSmtp, string sslEnabled, string login, string password)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + serveur + "|" + port + "|" + from + "|" + subject + "|" + body + "|" + smtp + "|" + portSmtp + "|" + sslEnabled + "|" + login + "|" + password;
                System.Net.WebClient objWebClient = new System.Net.WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            //Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }


                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = objWebClient.BaseAddress;
                        int PositionOfClientBin = objWebClient.BaseAddress.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        //ShowPreview(root,keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        //Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }

                };
                var tableau = OperationContext.Current.Host.BaseAddresses.First().AbsoluteUri.Split('/');
                string uri = tableau[0] + "//" + tableau[2].Split(':')[0] + ":" + PortWeb + "/";
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURIForEmail(uri), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Sylla 14/04/2017


        public string getKey(string matricule)
        {
            try
            {
                //string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
                //int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
                //string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                //string clientIpAddrss = root.Split(':')[1].Split('/')[2];
                //return clientIpAddrss;
                //string matricule = matricule;
                string heure = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                return matricule + heure;
                //return SessionObject.MachineName;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return string.Empty;
            }

        }
        public void ActionMail<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string rdlcUri, string moduleNmae, string matricule) where T : new()
        {
            // Parsing de originalArray vers Arrays
            List<T> ListeAImprimer = new List<T>();
            string key = getKey(matricule);
            //loaderHandler = LoadingManager.BeginLoading("Envoi en cours... ");
            T objVide = new T();
            foreach (P item in ListeDobjects)
            {
                objVide = Galatee.Tools.Utility.ParseObject<T, P>(objVide, item);
                ListeAImprimer.Add(objVide);
                objVide = new T();
            }

            List<Galatee.Structure.CsPrint> list = new List<Galatee.Structure.CsPrint>();
            foreach (T item in ListeAImprimer)
                list.Add(item as Galatee.Structure.CsPrint);

            //PrintingsServiceClient service = new PrintingsServiceClient(Utility.Protocole(), Utility.EndPoint("Printing"));
            setFromWebPart(list, key, param);
            SendToMailHandler(rdlcUri, moduleNmae, key);


        }
        static public Uri GetPreviewHTTPHandlerURIForEmail(string AbsoluteUri)
        {
            string handler = "MailFactures.ashx";
            //string strBaseWebAddress = AbsoluteUri;
            //int PositionOfClientBin = AbsoluteUri.ToLower().IndexOf("clientbin");
            //string root = strBaseWebAddress.Substring(0, PositionOfClientBin);

            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler, AbsoluteUri));

            return UriWebService;
        }
        static void SendToMailHandler(string rdlcUri, string moduleNmae, string key)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key;
                System.Net.WebClient objWebClient = new System.Net.WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        string keyPreview = resul.Result;

                        if (string.IsNullOrEmpty(keyPreview))
                        {
                            //Message.Show(Langue.preview_error_smg, Langue.msg_error_title);
                            return;
                        }


                        // construire le chemin de la page aspx servant d'apercu d'impression
                        string strBaseWebAddress = objWebClient.BaseAddress;
                        int PositionOfClientBin = objWebClient.BaseAddress.ToLower().IndexOf("clientbin");
                        string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
                        //string urlAspx = String.Format(@"{0}" + "HandlerPrinting/PreviewPrinting.aspx?key={1}", root, keyPreview);

                        //ShowPreview(root,keyPreview);
                        //ShowPreviewFrame(urlAspx);

                    }
                    catch (Exception ex)
                    {
                        //Message.Show(ex.Message, Galatee.Silverlight.Resources.Langue.errorTitle);
                    }

                };
                var tableau = OperationContext.Current.Host.BaseAddresses.First().AbsoluteUri.Split('/');
                string uri = tableau[0] + "//" + tableau[2].Split(':')[0] + ":17207/";
                objWebClient.UploadStringAsync(GetPreviewHTTPHandlerURIForEmail(uri), parametres);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public List<CsImageFile> Upload(CsImageFile image)
        {
             string filePath;
            filePath = image.ImagePath
                //+ ConfigurationManager.AppSettings["PictureUploadDirectory"] 
                   + "\\" + image.ImageName;
            filePath = filePath.Replace('[', '\\');
            //Bitmap MyImage = BitmapImage2Bitmap(ParaImage);
            //MyImage.Save(filePath, ImageFormat.Jpeg);
            byte[] b=(byte[])image.Imagestream.Clone();
            Image t = LoadImage(b);
            t.Save(filePath);
            Thread.Sleep(2000);
            //List<CsImageFile> obj = new List<CsImageFile>();
            // obj.Add(image);
             return null;
             
        }
        public Image LoadImage(byte[] bytes)
        {
            ////data:image/gif;base64,
            ////this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==");

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
        #endregion
        //Dictionnaire des parametres pour le Dataset 
        public static Hashtable parametres = new Hashtable();
        //Dictionnaire des données a imprimer
        public static Hashtable dicosCsPrints = new Hashtable();        

        /// <summary>
        /// Enregistre la liste des objets participants au report dans un hashtable static
        /// </summary>
        /// <param name="ObjectTable">Un hashtable constitué de la clé de l'user et de la liste des donnée à imprimer</param>
        /// <param name="key">La clé de l'utilisateur qui lance l'impression</param>        
        /// <param name="parameters">Les parametres additionnels de report</param>
        /// <returns>True pour reussite et false pour echec</returns>
        public bool? setFromWebPart(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter)
        {
            try
            {
                //ErrorManager.WriteInLogFile(this, key);

                if (dicosCsPrints.Contains(key))
                {
                    dicosCsPrints.Remove(key);
                }
                dicosCsPrints.Add(key, ObjectTable);

                if (parametres.Contains(key))
                    parametres.Remove(key);
                parametres.Add(key, parameter);
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message );
                return null;
            }
        }
        //public string setFromWebPartWithSplit(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter)
        //{
        //    try
        //    {
        //        ErrorManager.WriteInLogFile(this, "Lecture debut");
        //        ErrorManager.WriteInLogFile(this, key);

        //        if (WcfService.Printings.PrintingsService.dicosCsPrints.Contains(key))
        //        {
        //            var data = (List<CsPrint>)WcfService.Printings.PrintingsService.dicosCsPrints[key];
        //            data.AddRange(ObjectTable);
        //            WcfService.Printings.PrintingsService.dicosCsPrints[key] = data;
        //        }
        //        else
        //        {
        //            WcfService.Printings.PrintingsService.dicosCsPrints.Add(key, ObjectTable);
        //        }


        //        if (!parametres.Contains(key))
        //            parametres.Add(key, parameter);

        //        return "ok";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.WriteInLogFile(this, ex.Message );
        //        return null;
        //    }
        //}
        public string  setFromWebPartStrint(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter)
        {
            try
            {

                ErrorManager.WriteInLogFile(this, "Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                if (dicosCsPrints.Contains(key))
                    dicosCsPrints.Remove(key);
                dicosCsPrints.Add(key, ObjectTable);

                if (parametres.Contains(key))
                    parametres.Remove(key);
                parametres.Add(key, parameter);
                return "ok";
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return ex.Message;
            }
        }
        public List<CsPrint> GetCsPrintFromWebPart(string key)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                List<CsPrint> csprints = new List<CsPrint>();
                csprints = dicosCsPrints[key] as List<CsPrint>;

                if (csprints == null )
                    ErrorManager.WriteInLogFile(this, "Lecture Non OK");
                else 
                   ErrorManager.WriteInLogFile(this, "Lecture OK");

                return csprints;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {
            //    if (dicosCsPrints.ContainsKey(key))
            //        dicosCsPrints.Remove(key);
            //}
        }
        public List<CsImageFile > GetCsPrintFromWebPartOjetScanne(string key)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                List<CsImageFile> csprints = new List<CsImageFile>();
                csprints = dicosCsPrints[key] as List<CsImageFile>;

                if (csprints == null)
                    ErrorManager.WriteInLogFile(this, "Lecture Non OK");
                else
                    ErrorManager.WriteInLogFile(this, "Lecture OK");

                return csprints;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {
            //    if (dicosCsPrints.ContainsKey(key))
            //        dicosCsPrints.Remove(key);
            //}
        }

        public List<CsBalance> GetCsPrintFromWebPartBalanceAgee(string key)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Balance Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                List<CsBalance> csprints = new List<CsBalance>();
                csprints = dicosCsPrints[key] as List<CsBalance>;

                if (csprints == null)
                    ErrorManager.WriteInLogFile(this, "Balance Lecture Non OK");
                else
                    ErrorManager.WriteInLogFile(this, "Balance Lecture OK");

                return csprints;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {
            //    if (dicosCsPrints.ContainsKey(key))
            //        dicosCsPrints.Remove(key);
            //}
        }

        public Dictionary<string, string> getParameters(string key)
        {
            try
            {
                return parametres[key] as Dictionary<string, string>;
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
            //finally {

            //    if (parametres.ContainsKey(key))
            //        parametres.Remove(key);
            //}
        }
        public string setFromWebPartWithSplit(List<CsPrint> ObjectTable, string key, Dictionary<string, string> parameter)
        {
            try
            {
                ErrorManager.WriteInLogFile(this, "Lecture debut");
                ErrorManager.WriteInLogFile(this, key);

                if (WcfService.Printings.PrintingsService.dicosCsPrints.Contains(key))
                {
                    var data = (List<CsPrint>)WcfService.Printings.PrintingsService.dicosCsPrints[key];
                    data.AddRange(ObjectTable);
                    WcfService.Printings.PrintingsService.dicosCsPrints[key] = data;
                }
                else
                {
                    WcfService.Printings.PrintingsService.dicosCsPrints.Add(key, ObjectTable);
                }


                if (!WcfService.Printings.PrintingsService.parametres.Contains(key))
                    WcfService.Printings.PrintingsService.parametres.Add(key, parameter);

                return "ok";
            }
            catch (Exception ex)
            {
                ErrorManager.WriteInLogFile(this, ex.Message);
                return null;
            }
        }

        public bool? SetErrorsFromSilverlightWebPrinting(string methodInvoquante,string Error)
        {
            try
            {
                ErrorManager.WriteInLogFileFromWeb(this,methodInvoquante,Error);
                return true;
            }
            catch(Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsCampagnesBTAAccessiblesParLUO> Stat_LoadCampgne()
        {
            try
            {
                return new List<CsCampagnesBTAAccessiblesParLUO>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationCaisse> EtatCaisse()
        {
            try
            {
                return new List<CsHabilitationCaisse>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsDemandeBase> DemandeBase()
        {
            try
            {
                return new List<CsDemandeBase>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsReglementRecu> ReportListeEncaissements()
        {
            try
            {
                return new List<CsReglementRecu>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }

        public List<CsHabilitationProgram> ListeCsHabilitationProgram()
        {
            try
            {
                return new List<CsHabilitationProgram>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CustumEclairagePublic> GetCustumEclairagePublic()
        {
            try
            {
                return new List<CustumEclairagePublic>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  List<CsDetailCampagne> GetDetailCampagne()
        {
            try
            {
                return new List<CsDetailCampagne>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsHabilitationMetier> EtatHabilitationMetier()
        {
            try
            {
                return new List<CsHabilitationMetier>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsHabilitationMenu> EtatHabilitationMenu()
        {
            try
            {
                return new List<CsHabilitationMenu>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsAnnomalie > RetourneAnomalie()
        {
            try
            {
                return new List<CsAnnomalie>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogException(this, ex);
                return null;
            }
        }
        public List<CsEditionDevis> EditionDevis()
        {
            return new List<CsEditionDevis>();
        }
        public List<CsOrdreTravail> EditerOT()
        {
            return new List<CsOrdreTravail>();
        }
        public List<CsEvenement> EditerEnquete()
        {
            return new List<CsEvenement>();
        }
        public List<CsAvisDeCoupureClient> EditerAvisClient()
        {
            return new List<CsAvisDeCoupureClient>();
        }
             public List<ObjELEMENTDEVIS> EditerSortiMateriel()
        {
            return new List<ObjELEMENTDEVIS>();
        }
        public List<CsContrat > EditerContrat()
        {
            return new List<CsContrat>();
        }
        public List<CsDetailCampagnePrecontentieux> EditerCampagnePrecontentieux()
        {
            return new List<CsDetailCampagnePrecontentieux>();
        }
      public List<CsCanalisation> EditerCannalisation()
        {
            return new List<CsCanalisation>();
        }
      public List<CsLclient > EditerImpayeCategorie()
        {
            return new List<CsLclient>();
        }
      public List<CsEnteteFacture> EditerCsEnteteFacture()
      {
          return new List<CsEnteteFacture>();
      }
      public List<CsProduitFacture> EditerCsProduitFacture()
        {
            return new List<CsProduitFacture>();
        }
      public List<CsRedevanceFacture > EditerCsRedevanceFacture()
        {
            return new List<CsRedevanceFacture>();
        }
      public List<CsMandatementGc> EditerCsMandatementGc()
        {
            return new List<CsMandatementGc>();
        }
      public List<CsMaterielDemande> EditerCsMaterielDemande()
        {
            return new List<CsMaterielDemande>();
        }
      public List<CsCtax> EditerCsCtax()
        {
            return new List<CsCtax>();
        }
      public List<CsCaisse> EditerCsCaisse()
        {
            return new List<CsCaisse>();
        }
      public List<CsCoutDemande > EditerCsCoutDemande()
        {
            return new List<CsCoutDemande>();
        }
      public List<CsRemiseScelleByAg> EditerCsRemiseScelleByAg()
        {
            return new List<CsRemiseScelleByAg>();
        }
      public List<CsProgarmmation> EditerCsProgarmmation()
      {
          return new List<CsProgarmmation>();
      }
      public List<CsDepannage> EditerCsDepannage()
      {
          return new List<CsDepannage>();

      }
      public List<CsReclamationRcl> EditerCsReclamationRcl()
      {
          return new List<CsReclamationRcl>();

      }
      public List<cStatistiqueReclamation> EditercStatistiqueReclamation()
      {
          return new List<cStatistiqueReclamation>();

      }
      public List<CsEditionFactureFd> RetourneCsEditionFactureFd()
      {
          return new List<CsEditionFactureFd>();
      }
      public List<CsComptabilisation> RetourneCsComptabilisation()
      {
          return new List<CsComptabilisation>();
      }
      public List<CsEcritureComptable> RetourneCsEcritureComptable()
      {
          return new List<CsEcritureComptable>();
      }
      public List<CsStatFact> RetourneStatFact()
      {
          return new List<CsStatFact>();
      }
      public List<CsStatFactRecap> RetourneCsStatFactRecap()
      {
          return new List<CsStatFactRecap>();
      }

     public  List<CsFichierPersonnel> RetourneCsFichierPersonnel()
      {
          return new List<CsFichierPersonnel>();
      }
     public List<CsCompteurBta > RetourneCsCompteurBta()
      {
          return new List<CsCompteurBta>();
      }
     public List<CsBalanceAgee> RetourneCsBalanceAgee()
     {
         return new List<CsBalanceAgee>();
     }
     public List<CsBalance> RetourneCsBalance()
     {
         return new List<CsBalance>();
     }
     public List<CsClientRechercher> RetourneCsClientRechercher()
     {
         return new List<CsClientRechercher>();
     }
     public List<CsTranscaisse> RetourneCsTranscaisse()
     {
         return new List<CsTranscaisse>();
     }
     public List<CsTournee> RetourneCsTournee()
     {
         return new List<CsTournee>();
     }
     public List<CsComparaisonFacture> RetourneCsComparaisonFacture()
     {
         return new List<CsComparaisonFacture>();
     }
     public List<CsClient> RetourneCsClient()
     {
         return new List<CsClient>();
     }
     public List<CsDonnesStatistiqueDemande> RetourneCsDonnesStatistiqueDemande()
     {
         return new List<CsDonnesStatistiqueDemande>();
     }
     public List<CsStatistiqueTravaux_Brt_Ext> RetourneStatistiqueTravaux_Brt_Ext()
     {
         return new List<CsStatistiqueTravaux_Brt_Ext>();
     }




     public void PrintingFromService<T, P>(List<P> ListeDobjects, Dictionary<string, string> param, string NomFichier, string CheminImpression, string Format, string rdlcUri, string moduleNmae, string matricule, string serveur, string port, string PortWeb) where T : new()
     {
         try
         {
             // Parsing de originalArray vers Arrays
             List<T> ListeAImprimer = new List<T>();
             string key = getKey(matricule);
             //loaderHandler = LoadingManager.BeginLoading("Envoi en cours... ");
             T objVide = new T();
             foreach (P item in ListeDobjects)
             {
                 objVide = Galatee.Tools.Utility.ParseObject<T, P>(objVide, item);
                 ListeAImprimer.Add(objVide);
                 objVide = new T();
             }

             List<Galatee.Structure.CsPrint> list = new List<Galatee.Structure.CsPrint>();
             foreach (T item in ListeAImprimer)
                 list.Add(item as Galatee.Structure.CsPrint);

             setFromWebPart(list, key, param);

             SendToExport(NomFichier, rdlcUri, moduleNmae, key, CheminImpression, false, Format, serveur, port, PortWeb);


         }
         catch (Exception ex)
         {
             ErrorManager.WriteInLogFile(this, "Problème impression => " + ex.Message);
         }
     }

     static void SendToExport(string NomFichier, string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape, string Format, string ServerEndPointName, string ServerEndPointPort, string hostPort)
     {
         try
         {
             string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape;
             System.Net.WebClient objWebClient = new System.Net.WebClient();
             objWebClient.UploadStringCompleted += (send, resul) =>
             {
                 try
                 {
                     if (resul.Cancelled || resul.Error != null)
                     {
                         return;
                     }
                 }
                 catch (Exception ex)
                 {
                 }
             };
             objWebClient.UploadStringAsync(GetHTTPHandlerURIExportation(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format, ServerEndPointName, ServerEndPointPort, hostPort), parametres);
         }
         catch (Exception ex)
         {
             throw ex;
         }

     }
     //static public Uri GetHTTPHandlerURIExportation(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape, string Format, string ServerEndPointName, string ServerEndPointPort, string hostPort)
     //{
     //    string handler = "Main";
     //    var tableau = OperationContext.Current.Host.BaseAddresses.First().AbsoluteUri.Split('/');
     //    string root = tableau[0] + "//" + tableau[2].Split(':')[0] + ":" + hostPort + "/";
     //    Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, ServerEndPointName, ServerEndPointPort, NomFichier));
     //    return UriWebService;
     //}


     static public Uri GetHTTPHandlerURIExportation(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape, string Format, string ServerEndPointName, string ServerEndPointPort, string hostPort)
     {

         string handler = "Main";
         string root = "http://" + ServerEndPointName + ":" + hostPort + "/";
         Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, ServerEndPointName, ServerEndPointPort, NomFichier));
         return UriWebService;
     }






    }
}
