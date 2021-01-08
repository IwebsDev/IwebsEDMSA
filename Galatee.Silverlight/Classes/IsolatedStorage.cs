using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
using System.IO;

namespace Galatee.Silverlight.Classes
{

    [DataContract]
  static   public class IsolatedStorage
    {
        //static public void writeUserPreferences(CsInfoServiceLocal _InfoLocal )
        //{
        //    try
        //    {
        //        var iStorage = IsolatedStorageSettings.ApplicationSettings; // niveau application
        //        if (!iStorage.Contains("AdresseLocal"))
        //        {
        //            iStorage.Remove("AdresseLocal");
        //            iStorage.Remove("PortLocal");
        //            iStorage["AdresseLocal"] = _InfoLocal.AdresseMachineLocal;
        //            iStorage["PortLocal"] = _InfoLocal.PortServiceLocal;
        //            iStorage.Save();
        //        }
        //        else
        //        {
        //                iStorage.Remove("AdresseLocal");
        //                iStorage.Remove("PortLocal");
        //                iStorage["AdresseLocal"] = _InfoLocal.AdresseMachineLocal;
        //                iStorage["PortLocal"] = _InfoLocal.PortServiceLocal;
        //                iStorage.Save();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //static public void writeUserInfo(string  _Info,string Libelle)
        //{
        //    try
        //    {
        //        var iStorage = IsolatedStorageSettings.ApplicationSettings; // niveau application
        //        if (!iStorage.Contains(Libelle))
        //        {
        //            iStorage[Libelle] = _Info;
        //            iStorage.Save();
        //        }
        //        else
        //        {
        //            iStorage.Remove(Libelle);
        //            iStorage[Libelle] = _Info;
        //            iStorage.Save();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //static public CsInfoServiceLocal  readUserPreferences()
        //{
        //    try
        //    {
        //        CsInfoServiceLocal objetLocal = new CsInfoServiceLocal();
        //        var iStorage = IsolatedStorageSettings.ApplicationSettings; // niveau application

        //        objetLocal.AdresseMachineLocal = iStorage.Contains("AdresseLocal") ? iStorage["AdresseLocal"].ToString() : string.Empty;
        //        objetLocal.PortServiceLocal = iStorage.Contains("PortLocal") ? iStorage["PortLocal"].ToString() : string.Empty;
        //        return objetLocal;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //static public string  readInfo(string Info)
        //  {
        //      try
        //      {
       

        //          string objetLocal = string.Empty;
        //          var iStorage = IsolatedStorageSettings.ApplicationSettings; // niveau application
        //          objetLocal = iStorage.Contains(Info) ? iStorage[Info].ToString() : string.Empty;
        //          return objetLocal;
        //      }
        //      catch (Exception ex)
        //      {
        //          throw ex;
        //      }
        //  }

        public static void storeMachine(string MachineName)
        {
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    using (IsolatedStorageFileStream stream = store.CreateFile("MachineNameStore.txt"))
                    {
                        // Store the person details in the file.
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(MachineName);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "MachineNameStore");// throw;
            }
        }

       public static string getMachine()
        {
            string Machine = string.Empty;
            try
            {
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (store.FileExists("MachineNameStore.txt"))
                    {
                        using (FileStream stream = store.OpenFile("MachineNameStore.txt", FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(stream);
                            Machine = reader.ReadLine();
                            reader.Close();
                        }
                    }
                }
                return Machine;
            }
            catch (Exception ex)
            {
                Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "getModuleSend");// throw;
                return null;
            }
        }

       public static void storeCentre(string LeCentre)
       {
           try
           {
               using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
               {
                   using (IsolatedStorageFileStream stream = store.CreateFile("CentreStore.txt"))
                   {
                       // Store the person details in the file.
                       StreamWriter writer = new StreamWriter(stream);
                       writer.Write(LeCentre);
                       writer.Close();
                   }
               }
           }
           catch (Exception ex)
           {
               Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "CentreStore");// throw;
           }
       }

       public static string getCentre()
       {
           string Centre = string.Empty;
           try
           {
               using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
               {
                   if (store.FileExists("MachineNameStore.txt"))
                   {
                       using (FileStream stream = store.OpenFile("MachineNameStore.txt", FileMode.Open))
                       {
                           StreamReader reader = new StreamReader(stream);
                           Centre = reader.ReadLine();
                           reader.Close();
                       }
                   }
               }
               return Centre;
           }
           catch (Exception ex)
           {
               Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "getModuleSend");// throw;
               return null;
           }
       }

       public static void storeIsWorkfloGp(string IsWkfGp)
       {
           try
           {
               using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
               {
                   using (IsolatedStorageFileStream stream = store.CreateFile("WkfStore.txt"))
                   {
                       // Store the person details in the file.
                       StreamWriter writer = new StreamWriter(stream);
                       writer.Write(IsWkfGp);
                       writer.Close();
                   }
               }
           }
           catch (Exception ex)
           {
               Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "WkfStore");// throw;
           }
       }

       public static string getIsWorkfloGp()
       {
           string Centre = string.Empty;
           try
           {
               using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
               {
                   if (store.FileExists("WkfStore.txt"))
                   {
                       using (FileStream stream = store.OpenFile("WkfStore.txt", FileMode.Open))
                       {
                           StreamReader reader = new StreamReader(stream);
                           Centre = reader.ReadLine();
                           reader.Close();
                       }
                   }
               }
               return Centre;
           }
           catch (Exception ex)
           {
               Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle + "getModuleSend");// throw;
               return null;
           }
       }

       public static void DeleteIsWorkfloGp()
       {
           try
           {
               using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForSite())
               {
                   foreach (string file in store.GetFileNames("WkfStore.txt"))
                   {
                       try
                       {
                           if (store.FileExists(file))
                               store.DeleteFile(file);

                       }
                       catch (Exception ex)
                       {
                           Message.Show(ex, Galatee.Silverlight.Resources.Langue.errorTitle);// throw;
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

       }
    }
}
