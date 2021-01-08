using Galatee.DataAccess;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace WcfService.InterfaceComptable
{
     //NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InterfaceComptableService" in code, svc and config file together.
     //NOTE: In order to launch WCF Test Client for testing this service, please select InterfaceComptableService.svc or InterfaceComptableService.svc.cs at the Solution Explorer and start debugging.
     [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)] 
    public class InterfaceComptableService : IInterfaceComptableService
    {
         public List<CsCentre> RetourneListeDeSite()
         {
             try
             {
                 DBAccueil db = new DBAccueil();
                 return db.ChargerLesDonneesDesSite(false );
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsCaisse> RetourneCaisse()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneCaisse();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsCoper> RetourneCodeOperation()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneOperation();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsComptabilisation> RetourneAllOperationByCritere(int IdCentre, List<int> lstIdcaisse, List<string> OperationSelect, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, DateTime? Date)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneAllOperation(IdCentre, lstIdcaisse, OperationSelect, DateCaisseDebut, DateCaisseFin, Date);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> Retournefacture(List<CsOperationComptable> lesOperationCpt, List<int> IdCentre,DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.Retournefacture(lesOperationCpt,IdCentre, DateCaisseDebut, DateCaisseFin,matricule ,Site );
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>>  RetourneEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneEncaissement(lesOperationCpt, lstCaisse, DateCaisseDebut, DateCaisseFin,matricule,Site);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneFactureAutre(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneEncaissement(lesOperationCpt, lstCaisse, DateCaisseDebut, DateCaisseFin, matricule, Site);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneMiseAJourGrandCompte(CsOperationComptable OperationCpt, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneMiseAJourGrandCompte(OperationCpt, DateCaisseDebut, DateCaisseFin, matricule, Site);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsComptabilisation> RetourneEtatMiseAJourGrandCompte( DateTime? DateCaisseDebut, DateTime? DateCaisseFin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneEtatMiseAJourGrandCompte(DateCaisseDebut, DateCaisseFin);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }



         public Dictionary<List<CsComptabilisation>, List<CsEcritureComptable>> RetourneDEncaissement(List<CsOperationComptable> lesOperationCpt, List<CsCaisse> lstCaisse, DateTime? DateCaisseDebut, DateTime? DateCaisseFin, string matricule, string Site)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneDEncaissement(lesOperationCpt, lstCaisse, DateCaisseDebut, DateCaisseFin,matricule ,Site );

                 
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public bool RetourneFichierComptable(List<CsComptabilisation> LstEcriture)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneFichierComptable(LstEcriture);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return false;
             }
        }
         public List<CsBalance> RetourneBalanceAgee(string CodeSite, DateTime? Datefin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneBalanceAgeeSpx(CodeSite, Datefin);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsBalance> RetourneBalanceAuxilliaire(string CodeSite, DateTime? Datefin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 ErrorManager.WriteInLogFile(this, "debut" + System.DateTime.Now);
                 List<CsBalance> lats = db.RetourneBalanceAuxilliaireSpx(CodeSite, Datefin);
                 ErrorManager.WriteInLogFile(this, "Fin" + System.DateTime.Now);
                 return lats;
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }

         //public bool RetourneBalanceAuxilliaire_Block(string CodeSite, DateTime? Datefin, bool chk_Excel_IsChecked, string Libelle_Site, string CheminImpression, int Offset, string matricule,string ServerEndPointName,string ServerEndPointPort,string WebAppBaseAdreese)
         //{
         //    try
         //    {
         //        DbInterfaceComptable db = new DbInterfaceComptable();
         //        ErrorManager.WriteInLogFile(this, "debut" + System.DateTime.Now);
         //        List<CsBalance> lats = db.RetourneBalanceAuxilliaireSpx(CodeSite, Datefin);
         //        ErrorManager.WriteInLogFile(this, "Fin" + System.DateTime.Now);
         //        //return lats;




         //       string format = "pdf";
         //       if (chk_Excel_IsChecked == true) format= "xlsx";
         //       Dictionary<string, string> param = new Dictionary<string, string>();
         //       //param.Add("pDateFin", Datefin.ToString());
         //       //param.Add("pAgence", Libelle_Site);
         //       //List<ServiceAccueil.CsBalance> l = Utility.ConvertListType<ServiceAccueil.CsBalance, ServiceInterfaceComptable.CsBalance>(args.Result);
         //       ActionExportationWithSpliting(lats, param, string.Empty, CheminImpression, "BalanceAuxilliaire", "InterfaceComptable", true, format, Offset,matricule, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);

         //       return true;
         //    }
         //    catch (Exception ex)
         //    {
         //        ErrorManager.LogException(this, ex);
         //        return false;
         //    }
         //}
         //static int compteur;
         //static public void ActionExportationWithSpliting(List<CsBalance> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, int Offset, string matricule, string ServerEndPointName, string ServerEndPointPort, string WebAppBaseAdreese)
         //{
         //    // Parsing de originalArray vers Arrays
         //    //int loaderHandler = -1;
         //    //loaderHandler = LoadingManager.BeginLoading("Patientez ... ");
         //    try
         //    {

         //        string key = getKey(matricule);
         //        //Shared.ClasseMEthodeGenerique.SetMachineAndPortFromEndPoint(Utility.EndPoint("Printing"));

         //        SetFromWebPartBalanceWithSplit(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, key, Offset, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);

         //    }
         //    catch (Exception ex)
         //    {
         //        throw ex;
         //    }
         //}
         //public static string getKey(string matricule_param)
         //{
         //    try
         //    {
         //        //string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
         //        //int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf(@"clientbin");
         //        //string root = strBaseWebAddress.Substring(0, PositionOfClientBin);
         //        //string clientIpAddrss = root.Split(':')[1].Split('/')[2];
         //        //return clientIpAddrss;
         //        string matricule = matricule_param;
         //        string heure = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
         //        return matricule + heure;
         //        //return SessionObject.MachineName;
         //    }
         //    catch (Exception ex)
         //    {
         //        string error = ex.Message;
         //        return string.Empty;
         //    }

         //}
         //private static void SetFromWebPartBalanceWithSplit(List<CsBalance> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, string key, int Offset, string ServerEndPointName, string ServerEndPointPort, string WebAppBaseAdreese)
         //{
         //    var Reste = ListeDobjects.Count() % Offset;
         //    var Quotient = ListeDobjects.Count() / Offset;
         //    RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, key, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);
         //}
         //private static void RecursifSet(List<CsBalance> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, string key, int Offset, int Quotient, int Reste, string ServerEndPointName, string ServerEndPointPort,string  WebAppBaseAdreese)
         //{
         //   new WcfService.AcceuilService().setFromWebPartBalance(ListeDobjects.GetRange(compteur * Offset, (compteur == Quotient) ? Reste : Offset), key, param);
            
         //   compteur++;
         //   if (compteur < Quotient)
         //       RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, key, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);
         //   else
         //   {
         //       if (compteur == Quotient)
         //       {
         //           //Offset = Reste;
         //           RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, key, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);
         //       }
         //   }
         //   if (compteur > Quotient)
         //   {
         //       compteur = 0;
         //       SendToExportTypeEdition(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format, "1", ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese);

         //   }
         //}
         static void SendToExportTypeEdition(string NomFichier, string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape, string Format, string TypeEdition, string ServerEndPointName, string ServerEndPointPort, string WebAppBaseAdreese)
        {
            try
            {
                string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape + "|"  + TypeEdition;
                System.Net.WebClient objWebClient = new System.Net.WebClient();
                objWebClient.UploadStringCompleted += (send, resul) =>
                {
                    try
                    {
                        if (resul.Cancelled || resul.Error != null)
                        {
                            string error = resul.Error.Message;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
                objWebClient.UploadStringAsync(GetHTTPHandlerURIExportation_TYPEDEMANDE(NomFichier,rdlcUri, moduleNmae, key, printer, IsLandscape, Format,TypeEdition, ServerEndPointName, ServerEndPointPort, WebAppBaseAdreese), parametres);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
         static public Uri GetHTTPHandlerURIExportation_TYPEDEMANDE(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape, string Format, string TypeEdition, string ServerEndPointName, string ServerEndPointPort, string WebAppBaseAdreese)
        {
             var tableau = OperationContext.Current.Host.BaseAddresses.First().AbsoluteUri.Split('/');
                //string uri = tableau[0] + "//" + tableau[2].Split(':')[0] + ":2017/";
            string handler = "Main";
            //string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
            //int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
            //string root = uri;
            string root = WebAppBaseAdreese;
            Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}&TypeEdition={11}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, ServerEndPointName, ServerEndPointPort, NomFichier, TypeEdition));
            return UriWebService;
        }



         //public bool ActionExportFormatWithSpliting_image(List<CsImageFile> ListeDobjects, Dictionary<string, string> param, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format,string Matricule,string ServerEndPointName,string ServerEndPointPort)
         //{
         //    try
         //    {
         //        SetFromWebPartBalanceWithSplitGenerique(ListeDobjects, param, "", printer, rdlcUri, moduleNmae, IsLandscape, Format, 2000, ServerEndPointName, ServerEndPointPort);

         //       return true;
         //    }
         //    catch (Exception ex)
         //    {
         //        ErrorManager.LogException(this, ex);
         //        return false;
         //    }
         //}
         //private static void SetFromWebPartBalanceWithSplitGenerique(List<CsImageFile> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, int Offset,string ServerEndPointName,string ServerEndPointPort)
         //{
         //    var Reste = ListeDobjects.Count() % Offset;
         //    var Quotient = ListeDobjects.Count() / Offset;
         //    //Message.Show("RecursifSet debut " , "");
         //    RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort);
         //}
         //private static void RecursifSet(List<CsImageFile> ListeDobjects, Dictionary<string, string> param, string NomFichier, string printer, string rdlcUri, string moduleNmae, bool IsLandscape, string Format, int Offset, int Quotient, int Reste,string ServerEndPointName,string ServerEndPointPort)
         //{

         // AcceuilService svc=   new AcceuilService();
         //    List<CsPrint> ListeAImprimer = new List<CsPrint>();
         //    string keys = getKey("99999");
         //    CsPrint objVide = new CsPrint();
         //    foreach (P item in ListeDobjects)
         //    {
         //        objVide = ParseObject<CsPrint, CsImageFile>(objVide, item);
         //        ListeAImprimer.Add(objVide);
         //        objVide = new CsPrint();
         //    }
         //    //var list = (List<CsPrint>)ListeDobjects;
         //    svc.setFromWebPartWithSplit(ListeAImprimer.GetRange(compteur * Offset, (compteur == Quotient) ? Reste : Offset), keys, param);
            

         //                compteur++;
         //                if (compteur < Quotient)
         //                    RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort);
         //                else
         //                {
         //                    if (compteur == Quotient)
         //                    {
         //                        //Offset = Reste;
         //                        RecursifSet(ListeDobjects, param, NomFichier, printer, rdlcUri, moduleNmae, IsLandscape, Format, Offset, Quotient, Reste, ServerEndPointName, ServerEndPointPort);
         //                    }
         //                }
         //                if (compteur > Quotient)
         //                {
         //                    compteur = 0;
         //                    //Message.Show("SendToExport debut" , "");
         //                    SendToExport(NomFichier, rdlcUri, moduleNmae, keys, printer, IsLandscape, Format, ServerEndPointName, ServerEndPointPort);
         //                    //LoadingManager.EndLoading(loaderHandler);

         //                }
                  
         //}
         //static public T ParseObject<T, P>(T objetARemplir, P objetATransferer)
         //{
         //    int indexval = 0;
         //    try
         //    {
         //        T objetARemp = objetARemplir;
         //        // Recuperation des types
         //        PropertyInfo[] properties1 = objetARemplir.GetType().GetProperties();
         //        PropertyInfo[] properties2 = objetATransferer.GetType().GetProperties();

         //        // Test de l'unicité des deux types
         //        if (properties1.Length == properties2.Length)
         //        {
         //            // Remplacement des valeurs

         //            for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
         //            {
         //                indexval = indexval + 1;
         //                if (properties1[attrNum].GetType().Equals(properties2[attrNum].GetType()))
         //                {
         //                    object value2 = properties2[attrNum].GetValue(objetATransferer, null);
         //                    properties1[attrNum].SetValue(objetARemp, value2, null);
         //                }
         //            }

         //            return objetARemp;

         //        }
         //        else
         //            throw new Exception("Les types n'ont pas la meme structure");
         //    }
         //    catch (Exception ex)
         //    {
         //        int t = indexval;
         //        throw ex;
         //    }

         //}
         //static void SendToExport(string NomFichier, string rdlcUri, string moduleNmae, string key, string printer, bool IsLandscape, string Format,string ServerEndPointName,string ServerEndPointPort)
         //{
         //    //int res = LoadingManager.BeginLoading(Langue.En_Cours);
         //    try
         //    {
         //        string parametres = rdlcUri + "|" + moduleNmae + "|" + key + "|" + printer + "|" + IsLandscape;
         //        System.Net.WebClient objWebClient = new System.Net.WebClient();
         //        objWebClient.UploadStringCompleted += (send, resul) =>
         //        {
         //            try
         //            {
         //                if (resul.Cancelled || resul.Error != null)
         //                {
         //                    //LoadingManager.EndLoading(res);
         //                    string error = resul.Error.Message;
         //                    //Message.Show(error, Galatee.Silverlight.Resources.Langue.errorTitle);
         //                    return;
         //                }
         //                //LoadingManager.EndLoading(res);
         //            }
         //            catch (Exception ex)
         //            {
         //                //LoadingManager.EndLoading(res);
         //                //Message.ShowError(ex, Galatee.Silverlight.Resources.Langue.errorTitle);
         //            }
         //        };
         //        objWebClient.UploadStringAsync(GetHTTPHandlerURIExportation(NomFichier, rdlcUri, moduleNmae, key, printer, IsLandscape, Format, ServerEndPointName, ServerEndPointPort), parametres);
         //    }
         //    catch (Exception ex)
         //    {
         //        //LoadingManager.EndLoading(res);
         //        throw ex;
         //    }

         //}
         //static public Uri GetHTTPHandlerURIExportation(string NomFichier, string rdlcName, string moduleName, string key, string printer, bool IsLandscape, string Format,string ServerEndPointName,string ServerEndPointPort)
         //{
         //    //string handler = "Main";
         //    var tableau = OperationContext.Current.Host.BaseAddresses.First().AbsoluteUri.Split('/');
         //    string uri = tableau[0] + "//" + tableau[2].Split(':')[0] + ":2005/";
         //    string handler = "Main";
         //    //string strBaseWebAddress = App.Current.Host.Source.AbsoluteUri;
         //    //int PositionOfClientBin = App.Current.Host.Source.AbsoluteUri.ToLower().IndexOf("clientbin");
         //    string root = uri;
         //    Uri UriWebService = new Uri(String.Format(@"{0}" + "HandlerPrinting/" + handler + ".aspx?rdlc={1}&module={2}&key={3}&printer={4}&IsLandscape={5}&IsExport={6}&FormatExport={7}&machine={8}&port={9}&NomDuFichier={10}", root, rdlcName, moduleName, key, printer, IsLandscape, true, Format, SessionObject.ServerEndPointName, SessionObject.ServerEndPointPort, NomFichier));
         //    return UriWebService;
         //}







         #region Interfacage comptable Sylla
         public List<CsTypeFactureComptable> RetourneTypeFacture()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneTypeFacture();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsCompteSpecifique> RetourneCompteSpecifique()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneCompteSpecifique();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsTypeCompte > RetourneTypeCompte()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneTypeCompte();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsOperationComptable> RetourneOperationComptable()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneOperationComptable();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsBanqueCompte > RetourneBanqueCentre()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneBanqueCentre();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsCentreCompte > RetourneParamCentre()
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneParamCentre();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsEcritureComptable> IsOperationExiste(List<CsEcritureComptable> LigneComptable)
         {
             return new DbInterfaceComptable().IsOperationExiste(LigneComptable);
         }

         public bool InsertionLigneComptableGenerer(List<CsEcritureComptable> LigneComptable)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.InsertionLigneComptable(LigneComptable);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return false;
             }
         }
         public bool PurgeComptabilisation(List<int> IdOpertation,string CodeSite,DateTime? DateDebut,DateTime? DateFin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 foreach (int item in IdOpertation)
                     db.PurgeComptabilisation(item, CodeSite, DateDebut, DateFin);
                 return true;
                     
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return false;
             }
         }

 
         public List<CsLclient> RetourneAvanceSurConsomation(string CodeSite, bool IsResilier, DateTime? DateDebut, DateTime? DateFin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneAvanceSurConsomation(CodeSite, IsResilier, DateDebut,  DateFin);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         public List<CsClient> RetourneProvision(string CodeSite, List<string> lstCateg, List<string> lstProd, DateTime? DateDebut, DateTime? DateFin)
         {
             try
             {
                 DbInterfaceComptable db = new DbInterfaceComptable();
                 return db.RetourneProvision(CodeSite, lstCateg, lstProd, DateDebut, DateFin);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogException(this, ex);
                 return null;
             }
         }
         #endregion


     }
}
