using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Collections.ObjectModel;

namespace Galatee.Tools
{
    public static class Utility
    {

        //public static CsUserConnecte User = null;
        /// <summary>
        /// Isertion de donées en batch dans une table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pListObject"></param>
        /// <param name="pDestinationTableName"></param>
        /// <param name="pSqlcommand"> Est null quand la transaction n'est gérée </param>
        /// <param name="pConnexion"></param>
        /// <returns></returns>
        /// 

        public static bool InsertionEnBloc<T>(IEnumerable<T> pListObject, string pDestinationTableName, SqlCommand pSqlcommand)
        {
            var colList = new List<string>();
            SqlBulkCopy sqlBulkInsert = null;
            DataTable pDtSource = null;
            try
            {
                // Recupérer le schemas de la table dans la base de données

                pSqlcommand.Parameters.Clear();
                pSqlcommand.CommandType = CommandType.Text;
                pSqlcommand.CommandText = String.Format("SELECT TOP 1 * FROM {0}", pDestinationTableName);
                var reader = pSqlcommand.ExecuteReader();
                colList.Clear();
                int i;
                for (i = 0; i <= reader.FieldCount - 1; i++)
                {
                    colList.Add(reader.GetName(i).ToUpper());
                }
                reader.Close();
                reader.Dispose();
                // Constituer la souce de données
                pDtSource = ListToDataTable(pListObject);

                // Insertion en bloc
                if (pSqlcommand.Transaction != null)
                    sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection, SqlBulkCopyOptions.Default, pSqlcommand.Transaction);
                else
                {
                    sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
                }
                foreach (DataColumn col in pDtSource.Columns)
                {
                    if (colList.Contains(col.ColumnName.ToUpper()))
                    {
                        sqlBulkInsert.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                }
                sqlBulkInsert.DestinationTableName = pDestinationTableName;
                sqlBulkInsert.BulkCopyTimeout = 30;
                sqlBulkInsert.BatchSize = 5000;
                sqlBulkInsert.WriteToServer(pDtSource);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pDtSource != null) pDtSource.Dispose();
            }
        }

        public static bool InsertionEnBlocOptimiser<T>(IEnumerable<T> pListObject, string pDestinationTableName, SqlCommand pSqlcommand)
        {
            var colList = new List<string>();
            SqlBulkCopy sqlBulkInsert = null;
            DataTable pDtSource = null;
            try
            {
                // Recupérer le schemas de la table dans la base de données

                pSqlcommand.Parameters.Clear();
                pSqlcommand.CommandType = CommandType.Text;
                pSqlcommand.CommandText = String.Format("SELECT TOP 1 * FROM {0}", pDestinationTableName);
                var reader = pSqlcommand.ExecuteReader();
                colList.Clear();
                int i;
                for (i = 0; i <= reader.FieldCount - 1; i++)
                {
                    colList.Add(reader.GetName(i).ToUpper());
                }
                reader.Close();
                reader.Dispose();
                // Constituer la souce de données
                pDtSource = ListToDataTable(pListObject, colList);

                // Insertion en bloc
                if (pSqlcommand.Transaction != null)
                    sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection, SqlBulkCopyOptions.Default, pSqlcommand.Transaction);
                else
                {
                    sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
                }
                foreach (DataColumn col in pDtSource.Columns)
                {
                    if (colList.Contains(col.ColumnName.ToUpper()))
                    {
                        sqlBulkInsert.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                }
                sqlBulkInsert.DestinationTableName = pDestinationTableName;
                sqlBulkInsert.BulkCopyTimeout = 30;
                sqlBulkInsert.BatchSize = 5000;
                sqlBulkInsert.WriteToServer(pDtSource);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (pDtSource != null) pDtSource.Dispose();
            }
        }


        public static List<T> GetEntityListFromQuery<T>(DataTable pTable) where T : new()
        {
            string nonColon = string.Empty;
            List<string> ColListName = new List<string>();
            try
            {
                var ColList = pTable.Columns;
                foreach (DataColumn col in ColList)
                    ColListName.Add(col.ColumnName.ToUpper());
                List<T> results = new List<T>();
                foreach (DataRow row in pTable.Rows)
                {
                    T obj = new T();
                    var properties = obj.GetType().GetProperties();

                    foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                    {
                        if (ColListName.Contains(f.Name.ToUpper()))
                        {
                            nonColon = f.Name;
                            var o = row[f.Name];
                            if (o.GetType() != typeof(DBNull)) f.SetValue(obj, o, null);
                        }
                    }
                    results.Add(obj);
                }

                return results.ToList();
            }
            catch (Exception ex)
            {
                string le = nonColon;
                throw ex;
            }
        }

        //public static bool InsertionEnBloc<T>(IEnumerable<T> pListObject, string pDestinationTableName, SqlCommand pSqlcommand)
        //{
        //    var colList = new List<string>();
        //    SqlBulkCopy sqlBulkInsert = null;
        //    DataTable pDtSource = null;
        //    try
        //    {
        //        // Recupérer le schemas de la table dans la base de données
        //        pSqlcommand.Parameters.Clear();
        //        pSqlcommand.CommandType = CommandType.Text;
        //        pSqlcommand.CommandText = String.Format("SELECT TOP 1 * FROM {0}", pDestinationTableName);
        //        var reader = pSqlcommand.ExecuteReader();
        //        pDtSource = new DataTable();
        //        pDtSource.TableName = pDestinationTableName;
        //        pDtSource.Load(reader);
        //        reader.Close();
        //        reader.Dispose();
        //        // Constituer la souce de données
        //        pDtSource.Clear();
        //        var DataSet = new DataSet();
        //        DataSet.Tables.Add(pDtSource);

        //        //DataSet.Tables[pDtSource.TableName].ChildRelations.Clear();
        //        //DataSet.Tables[pDtSource.TableName].ParentRelations.Clear();
        //        DataSet.Tables[pDtSource.TableName].Constraints.Clear();
        //        pDtSource.Columns.Remove("ROWID");

        //        pDtSource.Merge(ListToDataTable(pListObject), false, MissingSchemaAction.Ignore);
        //        // Insertion en bloc
        //        if (pSqlcommand.Transaction != null)
        //            sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection, SqlBulkCopyOptions.Default, pSqlcommand.Transaction);
        //        else
        //        {
        //            sqlBulkInsert = new SqlBulkCopy(pSqlcommand.Connection.ConnectionString, SqlBulkCopyOptions.UseInternalTransaction);
        //        }
        //        sqlBulkInsert.DestinationTableName = pDestinationTableName;
        //        sqlBulkInsert.BulkCopyTimeout = 30;
        //        sqlBulkInsert.BatchSize = 5000;
        //        sqlBulkInsert.WriteToServer(pDtSource);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        //throw ex;
        //        foreach (DataRow error in pDtSource.GetErrors())
        //        {
        //            System.Diagnostics.Debug.WriteLine(error.RowError);
        //        }
        //        return false;
        //    }
        //    finally
        //    {
        //        if (pDtSource != null)
        //            pDtSource.Dispose();
        //    }
        //}

        public static bool OpenTransation(string pConnexion, ref SqlCommand pCommand)
        {
            try
            {
                var connectionBd = new SqlConnection(pConnexion);
                connectionBd.Open();
                var transactionBd =
                   connectionBd.BeginTransaction(IsolationLevel.ReadCommitted);
                pCommand = new SqlCommand();
                pCommand = connectionBd.CreateCommand();
                pCommand.CommandTimeout = 30 * 60;
                pCommand.Transaction = transactionBd;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlCommand OpenTransation(string pConnexion)
        {
            try
            {
                var connectionBd = new SqlConnection(pConnexion);
                connectionBd.Open();
                var transactionBd =
                   connectionBd.BeginTransaction(IsolationLevel.ReadCommitted);
                var pCommand = connectionBd.CreateCommand();
                pCommand.CommandTimeout = 30 * 60;
                pCommand.Transaction = transactionBd;
                return pCommand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValiderTransation(bool pValide, ref SqlCommand pCommand)
        {
            try
            {
                if (pValide)
                {
                    if (pCommand.Transaction != null) pCommand.Transaction.Commit();
                    if (pCommand.Connection.State != ConnectionState.Closed)
                        pCommand.Connection.Close();
                    return true;
                }
                if (pCommand.Transaction != null) pCommand.Transaction.Rollback();
                if (pCommand.Connection.State != ConnectionState.Closed)
                    pCommand.Connection.Close();
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Dispose();
            }
        }

        public static bool Commit( SqlCommand pCommand)
        {
            try
            {
                if (pCommand.Transaction != null) pCommand.Transaction.Commit();
                if (pCommand.Connection.State != ConnectionState.Closed)
                    pCommand.Connection.Close();
                pCommand.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Rollback(SqlCommand pCommand)
        {
            try
            {
                if (pCommand.Transaction != null) pCommand.Transaction.Rollback();
                if (pCommand.Connection.State != ConnectionState.Closed)
                    pCommand.Connection.Close();
                pCommand.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValiderTransation(bool pValide, SqlCommand pCommand)
        {
            try
            {
                if (pValide)
                {
                    if (pCommand.Transaction != null) pCommand.Transaction.Commit();
                    if (pCommand.Connection.State != ConnectionState.Closed)
                        pCommand.Connection.Close();
                    pCommand.Dispose();
                    return true;
                }
                if (pCommand.Transaction != null) pCommand.Transaction.Rollback();
                if (pCommand.Connection.State != ConnectionState.Closed)
                    pCommand.Connection.Close();
                pCommand.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pCommand.Dispose();
            }
        }

        /// <summary>
        /// 
        /// Convert a list of object to a datatable 
        /// so that it could be pass as parameter to local
        /// report embedded sources
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="varlist"></param>
        /// <returns></returns>
        /// 
        public static DataTable ListToDataTable<T>(IEnumerable<T> varlist)
        {
            var dtReturn = new DataTable();
            string NomCollum = string.Empty;
            // column names 
            try
            {
                PropertyInfo[] oProps = null;

                if (varlist == null) return dtReturn;

                foreach (var rec in varlist)
                {
                    // Use reflection to get property names, to create table, Only first time, others will follow 
                    if (oProps == null)
                    {
                        oProps = rec.GetType().GetProperties();
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;
                            NomCollum = pi.Name;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }

                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }

                    var dr = dtReturn.NewRow();

                    foreach (var pi in oProps)
                    {
                        NomCollum = pi.Name;
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }

                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                string tt = NomCollum;
                throw ex;
            }
        }
       public static DataTable ListToDataTable<T>(IEnumerable<T> varlist,List<string>Destination)
        {
            var dtReturn = new DataTable();
            string NomCollum = string.Empty;
            // column names 
            try
            {
                PropertyInfo[] oProps = null;

                if (varlist == null) return dtReturn;

                foreach (var rec in varlist)
                {
                    // Use reflection to get property names, to create table, Only first time, others will follow 
                    if (oProps == null)
                    {
                        oProps = rec.GetType().GetProperties();
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;
                            NomCollum = pi.Name;
                            string ValRech = Destination.FirstOrDefault(o => o == pi.Name);
                            if (ValRech == null) continue;
                            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }

                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                    }

                    var dr = dtReturn.NewRow();

                    foreach (var pi in oProps)
                    {
                        NomCollum = pi.Name;
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }

                    dtReturn.Rows.Add(dr);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                string tt = NomCollum;
              throw ex;
            }
        }

       public static  BasicHttpBinding Protocole()
       {
           try
           {
               BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
               binding.MaxReceivedMessageSize = Int32.MaxValue;
               binding.MaxBufferSize = Int32.MaxValue;
               return binding;
           }
           catch (Exception ex)
           {
               string error = ex.Message;
               return null;
           }
       }

       public static  EndpointAddress EndPoint(string address)
       {
           try
           {
               return new EndpointAddress(address);
           }
           catch (Exception ex)
           {
               string error = ex.Message;
               return null;
           }
       }

       static public T ParseObject<T, P>(T objetARemplir, P objetATransferer)
       {
           string colon = string.Empty;
           try
           {
               T objetARemp = objetARemplir;
               // Recuperation des types
               PropertyInfo[] properties1 = objetARemplir.GetType().GetProperties();
               PropertyInfo[] properties2 = objetATransferer.GetType().GetProperties();

               // Test de l'unicité des deux types
               //if (properties1.Length == properties2.Length)
               //{
                   // Remplacement des valeurs
                   for (int attrNum = 0; attrNum < properties2.Length; attrNum++)
                   {
                       if (properties1[attrNum].GetType().Equals(properties2[attrNum].GetType()))
                       {
                           colon = properties2[attrNum].GetType().Name;
                           object value2 = properties2[attrNum].GetValue(objetATransferer, null);
                           properties1[attrNum].SetValue(objetARemp, value2, null);
                       }
                   }

                   return objetARemp;

               //}
               //else
               //    throw new Exception("Les types n'ont pas la meme structure");
           }
           catch (Exception ex)
           {
               string lac = colon;
               throw ex;
           }

       }

       /// <summary>
       /// Convertir une liste d'objets de type P en une liste 
       /// d'objet de type T
       /// NB Les types T et P doivent avoir le meme nombre de propriétés de meme type 
       /// </summary>
       /// <typeparam name="T">Type de retour</typeparam>
       /// <typeparam name="P">Type de destination</typeparam>
       /// <param name="ListeDobjects">Liste source</param>
       /// <returns></returns>
       public static List<T> ConvertListType<T, P>(List<P> ListeDobjects) where T : new()
       {
           // Parsing de originalArray vers Arrays
           List<T> ListeAImprimer = new List<T>();
           //for (int objNum = 0; objNum < ListeDobjects.Count; objNum++)
           T objVide = new T();
           foreach (P item in ListeDobjects)
           {
               objVide = ConvertEntity<T, P>(item);
               ListeAImprimer.Add(objVide);
               objVide = new T();
           }
           return ListeAImprimer;
       }

       static public T ConvertEntity<T, P>(P objetATransferer) where T : new()
       {
           string Chm = string.Empty;
           string ch = string.Empty ;
           try
           {
               List<P> pliste = new List<P>();
               List<string> ColListName = new List<string>();
               pliste.Add(objetATransferer);

               DataTable table = ListToDataTable(pliste);
               var ColList = table.Columns;
               foreach (DataColumn col in ColList)
                   ColListName.Add(col.ColumnName.ToUpper());
               List<T> results = new List<T>();
               foreach (DataRow row in table.Rows)
               {
                   T obj = new T();
                   var properties = obj.GetType().GetProperties();

                   foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                   {
                       Chm = f.Name;
                       if (ColListName.Contains(f.Name.ToUpper()))
                       {
                           var o = row[f.Name];
                           if (o.GetType() != typeof(DBNull)) f.SetValue(obj, o, null);
                       }
                   }
                   results.Add(obj);
               }

               return results.FirstOrDefault();
           }
           catch (Exception ex)
           {
               ch = Chm;
               throw ex;
           }

       }

       public static IEnumerable<T> GetEntityFromQuery<T>(IEnumerable<object> pObjects) where T : new()
       {
           List<string> ColListName = new List<string>();
           try
           {
               DataTable table = ListToDataTable(pObjects);
               var ColList = table.Columns;
               foreach (DataColumn col in ColList)
                   ColListName.Add(col.ColumnName.ToUpper());
               T obj = new T();
               List<T> results = new List<T>();
               foreach (DataRow row in table.Rows)
               {
                   var properties = obj.GetType().GetProperties();

                   foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                   {
                       if (ColListName.Contains(f.Name.ToUpper()))
                       {
                           var o = row[f.Name];
                           if (o.GetType() != typeof(DBNull)) f.SetValue(obj, o, null);
                       }
                   }
                   results.Add(obj);
               }

               return results;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static IEnumerable<T> GetEntityFromQuery<T>(DataTable pTable) where T : new()
       {
           List<string> ColListName = new List<string>();
           try
           {
               var ColList = pTable.Columns;
               foreach (DataColumn col in ColList)
                   ColListName.Add(col.ColumnName.ToUpper());
               List<T> results = new List<T>();
               foreach (DataRow row in pTable.Rows)
               {
                   T obj = new T();
                   var properties = obj.GetType().GetProperties();

                   foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                   {
                       if (ColListName.Contains(f.Name.ToUpper()))
                       {
                           var o = row[f.Name];
                           if (o.GetType() != typeof(DBNull)) f.SetValue(obj, o, null);
                       }
                   }
                   results.Add(obj);
               }

               return results;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool IsGuid(string guid)
       {
           try
           {
               if (!string.IsNullOrEmpty(guid))
               {
                   var regex = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
                   return regex.IsMatch(guid);
               }
               return false;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool IsEmail(string email)
       {
           try
           {
               var regex = new Regex("^[a-z0-9._-]+@[a-z0-9.-]{2,}[.][a-z]{2,3}$");
               return regex.IsMatch(email);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static bool? SendMail(string address,string Sujet, string Message)
       {
           try
           {
               string MonAdresse = "GalateeTeam@inova.ci";

               MailAddress from = new MailAddress(MonAdresse);
               MailAddress to = new MailAddress(address);
               MailMessage mailMsg = new MailMessage(from, to);

               mailMsg.Subject = Sujet;
               //mailMsg.Attachments.Add(new Attachment(filePath));
               mailMsg.Body = Message;

               SmtpClient client = new SmtpClient("outlook.office365.com");
               client.Send(mailMsg);
               return true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static void EnvoiMail(List<string> destinataires, string message,string Emetteur,string Password,string ServerSmtp,int Port)
       {

           try
           {
               MailMessage mail = new MailMessage();
               SmtpClient smtpServer = new SmtpClient(ServerSmtp);

               string login = Emetteur;
               string password = "Password";

               //mail.To.Add("zgbale@inova.ci");
               //mail.To.Add("iakeita@edm-sa.com.ml");
               //mail.To.Add("iwebsmail@edm-sa.com.ml");
               //mail.To.Add("bmakadji@edm-sa.com.ml");
               if (destinataires != null && destinataires.Count > 0)
               {
                   foreach (string to in destinataires)
                   {
                       if (IsEMailAddress(to))
                           mail.To.Add(new MailAddress(to.Trim()));
                   }
               }

               mail.Subject = "Demande de mise à jour de compte rendu travaux";

               string text = "Des cas de compte rendu de remplacement de compteur non saisi ont été constatés lors de la saisie des relevés. " +
                           "<br/>Nous vous prions de prendre les dispositions nécessaires pour que ces comptes rendus soient saisis, " +
                           "afin de permettre la prise en compte des abonnements concernés dans la facturation. " +
                           "<br/>La liste des compteurs concernés est accessible dans le module ACCUEIL, au niveau du menu \"<font color=#336699> <i><b>Impression => Liste des compteurs remplacés.</b></i></font>\"";


               mail.Body = message;

               mail.IsBodyHtml = true;
               mail.BodyEncoding = System.Text.Encoding.UTF8;
               mail.SubjectEncoding = System.Text.Encoding.UTF8;

               mail.From = new MailAddress(login);

               smtpServer.Port = Port;

               smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
               smtpServer.UseDefaultCredentials = true;
               smtpServer.Credentials = new System.Net.NetworkCredential(login, password);
               smtpServer.EnableSsl = false;


               smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

               smtpServer.SendAsync(mail, mail);
           }
           catch (Exception ex)
           {
               //string dd = ex.Message;
           }
       }

       public static void EnvoiMail(List<string> destinataires, string message)
       {

           try
           {
               MailMessage mail = new MailMessage();
               SmtpClient smtpServer = new SmtpClient("10.100.2.56");

               string login = "iwebsmail@edm-sa.com.ml";
               string password = "projetiwebs";

               //mail.To.Add("zgbale@inova.ci");
               //mail.To.Add("iakeita@edm-sa.com.ml");
               //mail.To.Add("iwebsmail@edm-sa.com.ml");
               //mail.To.Add("bmakadji@edm-sa.com.ml");
               if (destinataires != null && destinataires.Count > 0)
               {
                   foreach (string to in destinataires)
                   {
                       if (IsEMailAddress(to))
                           mail.To.Add(new MailAddress(to.Trim()));
                   }
               }

               mail.Subject = "Demande de mise à jour de compte rendu travaux";

               string text = "Des cas de compte rendu de remplacement de compteur non saisi ont été constatés lors de la saisie des relevés. " +
                           "<br/>Nous vous prions de prendre les dispositions nécessaires pour que ces comptes rendus soient saisis, " +
                           "afin de permettre la prise en compte des abonnements concernés dans la facturation. " +
                           "<br/>La liste des compteurs concernés est accessible dans le module ACCUEIL, au niveau du menu \"<font color=#336699> <i><b>Impression => Liste des compteurs remplacés.</b></i></font>\"";


               mail.Body = message;

               mail.IsBodyHtml = true;
               mail.BodyEncoding = System.Text.Encoding.UTF8;
               mail.SubjectEncoding = System.Text.Encoding.UTF8;

               mail.From = new MailAddress(login);

               smtpServer.Port = 25;

               smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
               smtpServer.UseDefaultCredentials = true;
               smtpServer.Credentials = new System.Net.NetworkCredential(login, password);
               smtpServer.EnableSsl = false;


               smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

               smtpServer.SendAsync(mail, mail);
           }
           catch (Exception ex)
           {
               //string dd = ex.Message;
           }
       }

       //public static void EnvoiMail(List<string> destinataires, string message)
       //{

       //    try
       //    {
       //        MailMessage mail = new MailMessage();
       //        SmtpClient smtpServer = new SmtpClient("10.100.2.54");

       //        string login = "iwebsmail@edm-sa.com.ml";
       //        string password = "projetiwebs";

       //        //mail.To.Add("zgbale@inova.ci");
       //        //mail.To.Add("iakeita@edm-sa.com.ml");
       //        //mail.To.Add("iwebsmail@edm-sa.com.ml");
       //        //mail.To.Add("bmakadji@edm-sa.com.ml");
       //        if (destinataires != null && destinataires.Count > 0)
       //        {
       //            foreach (string to in destinataires)
       //            {
       //                if (IsEMailAddress(to))
       //                    mail.To.Add(new MailAddress(to.Trim()));
       //            }
       //        }

       //        mail.Subject = "Demande de mise à jour de compte rendu travaux";

       //        string text = "Des cas de compte rendu de remplacement de compteur non saisi ont été constatés lors de la saisie des relevés. " +
       //                    "<br/>Nous vous prions de prendre les dispositions nécessaires pour que ces comptes rendus soient saisis, " +
       //                    "afin de permettre la prise en compte des abonnements concernés dans la facturation. " +
       //                    "<br/>La liste des compteurs concernés est accessible dans le module ACCUEIL, au niveau du menu \"<font color=#336699> <i><b>Impression => Liste des compteurs remplacés.</b></i></font>\"";


       //        mail.Body = message;

       //        mail.IsBodyHtml = true;
       //        mail.BodyEncoding = System.Text.Encoding.UTF8;
       //        mail.SubjectEncoding = System.Text.Encoding.UTF8;

       //        mail.From = new MailAddress(login);

       //        smtpServer.Port = 25;

       //        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
       //        smtpServer.UseDefaultCredentials = true;
       //        smtpServer.Credentials = new System.Net.NetworkCredential(login, password);
       //        smtpServer.EnableSsl = false;


       //        smtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

       //        smtpServer.SendAsync(mail, mail);
       //    }
       //    catch (Exception ex)
       //    {
       //        //string dd = ex.Message;
       //    }
       //}

       private static void SendCompletedCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
       {
           MailMessage mail = e.UserState as MailMessage;
           mail.Dispose();
       }

       private static bool IsEMailAddress(string s)
       {
           return Regex.IsMatch(s, "^([\\w-]+\\.)*?[\\w-]+@[\\w-]+\\.([\\w-]+\\.)*?[\\w]+$");
       }

       public  static void SendMail()
       {
           try
           {
               string MonAdresse = "iwebsmail@edm-sa.com.ml";
               MailAddress from = new MailAddress(MonAdresse);
               //MailAddress to = new MailAddress("armand.kouakou@inova.ci");
               //MailMessage mailMsg = new MailMessage(from, to);

               MailMessage mailMsg = new MailMessage();
               mailMsg.From = new MailAddress(MonAdresse);
               mailMsg.To.Add(new MailAddress("ibdembele@edm-sa.com.ml"));
               //mailMsg.To.Add(new MailAddress("armand.kouakou@inova.ci"));

               mailMsg.Subject = "Votre facture d'electricité";
               //mailMsg.Attachments.Add(new Attachment(filePath));
               mailMsg.Body = "Bonjour, vous trouverez ci-joint votre facture, merci de ne plus fraudez et de payer regulièrement \nAu plaisir.";

               SmtpClient stpc = new SmtpClient("10.100.2.56", 25);
               stpc.Credentials = new System.Net.NetworkCredential("iwebsmail@edm-sa.ml.com", "projetiwebs");
               stpc.EnableSsl = false;
               stpc.UseDefaultCredentials = false;
               stpc.Send(mailMsg);
           }
           catch (Exception ex)
           {
           }

       }
     

       public static bool IsNumeric(this string valueToCheck)
       {
           try
           {
               Regex regex = new Regex(@"^[-+]?\d*[.,]?\d*$");
               return regex.IsMatch(valueToCheck);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
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

       public static List<T> RetourneListCopy<T>(List<T> _LaListe) where T : new()
       {
           try
           {
               List<T> results = new List<T>();
               foreach (T item in _LaListe)
               {
                   T obj = new T();
                   var properties11 = obj.GetType().GetProperties();
                   var properties = item.GetType().GetProperties();
                   foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                   {
                       PropertyInfo res = properties11.FirstOrDefault(p => p.Name.ToUpper() == f.Name.ToUpper());
                       if (res != null)
                       {
                           var o = f.GetValue(item, null);
                           f.SetValue(obj, o, null);
                       }
                   }
                   results.Add(obj);
               }

               return results.ToList();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public static T RetourneCopyObjet<T>(T objectInit) where T : new()
       {
           try
           {
               T results = new T();

               T obj = new T();
               var properties11 = obj.GetType().GetProperties();
               var properties = objectInit.GetType().GetProperties();
               foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
               {
                   PropertyInfo res = properties11.FirstOrDefault(p => p.Name.ToUpper() == f.Name.ToUpper());
                   if (res != null)
                   {
                       var o = f.GetValue(objectInit, null);
                       f.SetValue(obj, o, null);
                   }
               }
               results = obj;

               return results;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static string DecryptLicense(string input)
       {

           try
           {
               byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159 }; // La clé est codée sur 192 bits
               string Salt = Convert.ToBase64String(_SaltByte);
               byte[] encryptedBytes = Convert.FromBase64String(input);
               byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
               string decryptedString = string.Empty;
               using (var aes = new AesManaged())
               {
                   Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                   aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                   aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                   aes.Key = rfc.GetBytes(aes.KeySize / 8);
                   aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                   using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                   {
                       using (MemoryStream decryptedStream = new MemoryStream())
                       {
                           CryptoStream decryptor =
                               new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                           decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                           decryptor.Flush();
                           decryptor.Close();

                           byte[] decryptBytes = decryptedStream.ToArray();
                           decryptedString =
                               UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                       }
                   }
               }
               return decryptedString;
           }
           catch (Exception ex)
           {
               return null;
           }
       }

       public static  string converti(int  chiffre)
       {
           int centaine, dizaine, unite, reste, y;
           bool dix = false;
           string lettre = "";
           //strcpy(lettre, "");

           reste = chiffre / 1;

           for (int i = 1000000000; i >= 1; i /= 1000)
           {
               y = reste / i;
               if (y != 0)
               {
                   centaine = y / 100;
                   dizaine = (y - centaine * 100) / 10;
                   unite = y - (centaine * 100) - (dizaine * 10);
                   switch (centaine)
                   {
                       case 0:
                           break;
                       case 1:
                           lettre += "cent ";
                           break;
                       case 2:
                           if ((dizaine == 0) && (unite == 0)) lettre += "deux cents ";
                           else lettre += "deux cent ";
                           break;
                       case 3:
                           if ((dizaine == 0) && (unite == 0)) lettre += "trois cents ";
                           else lettre += "trois cent ";
                           break;
                       case 4:
                           if ((dizaine == 0) && (unite == 0)) lettre += "quatre cents ";
                           else lettre += "quatre cent ";
                           break;
                       case 5:
                           if ((dizaine == 0) && (unite == 0)) lettre += "cinq cents ";
                           else lettre += "cinq cent ";
                           break;
                       case 6:
                           if ((dizaine == 0) && (unite == 0)) lettre += "six cents ";
                           else lettre += "six cent ";
                           break;
                       case 7:
                           if ((dizaine == 0) && (unite == 0)) lettre += "sept cents ";
                           else lettre += "sept cent ";
                           break;
                       case 8:
                           if ((dizaine == 0) && (unite == 0)) lettre += "huit cents ";
                           else lettre += "huit cent ";
                           break;
                       //case 9:
                       //    if ((dizaine == 0) && (unite == 0)) lettre += "neuf cents ";
                       //    else lettre += "neuf cent ";
                   }// endSwitch(centaine)

                   switch (dizaine)
                   {
                       case 0:
                           break;
                       case 1:
                           dix = true;
                           break;
                       case 2:
                           lettre += "vingt ";
                           break;
                       case 3:
                           lettre += "trente ";
                           break;
                       case 4:
                           lettre += "quarante ";
                           break;
                       case 5:
                           lettre += "cinquante ";
                           break;
                       case 6:
                           lettre += "soixante ";
                           break;
                       case 7:
                           dix = true;
                           lettre += "soixante ";
                           break;
                       case 8:
                           lettre += "quatre-vingt ";
                           break;
                       //case 9:
                       //    dix = true;
                       //    lettre += "quatre-vingt ";
                   } // endSwitch(dizaine)

                   switch (unite)
                   {
                       case 0:
                           if (dix) lettre += "dix ";
                           break;
                       case 1:
                           if (dix) lettre += "onze ";
                           else lettre += "un ";
                           break;
                       case 2:
                           if (dix) lettre += "douze ";
                           else lettre += "deux ";
                           break;
                       case 3:
                           if (dix) lettre += "treize ";
                           else lettre += "trois ";
                           break;
                       case 4:
                           if (dix) lettre += "quatorze ";
                           else lettre += "quatre ";
                           break;
                       case 5:
                           if (dix) lettre += "quinze ";
                           else lettre += "cinq ";
                           break;
                       case 6:
                           if (dix) lettre += "seize ";
                           else lettre += "six ";
                           break;
                       case 7:
                           if (dix) lettre += "dix-sept ";
                           else lettre += "sept ";
                           break;
                       case 8:
                           if (dix) lettre += "dix-huit ";
                           else lettre += "huit ";
                           break;
                       //case 9:
                       //    if (dix) lettre += "dix-neuf ";
                       //    else lettre += "neuf ";
                   } // endSwitch(unite)

                   switch (i)
                   {
                       case 1000000000:
                           if (y > 1) lettre += "milliards ";
                           else lettre += "milliard ";
                           break;
                       case 1000000:
                           if (y > 1) lettre += "millions ";
                           else lettre += "million ";
                           break;
                       //case 1000:
                       //    lettre += "mille ";
                   }
               } // end if(y!=0)
               reste -= y * i;
               dix = false;
           } // end for
           if (lettre.Length == 0) lettre += "zero";

           return lettre;
       }

       public static int JourOuvrer(DateTime dtmStart, DateTime dtmEnd)
       {
           // This function includes the start and end date in the count if they fall on a weekday
           int NbreJour = 0;
           int dowStart = ((int)dtmStart.DayOfWeek == 0 ? 7 : (int)dtmStart.DayOfWeek);
           int dowEnd = ((int)dtmEnd.DayOfWeek == 0 ? 7 : (int)dtmEnd.DayOfWeek);
           TimeSpan tSpan = dtmEnd - dtmStart;
           if (dowStart <= dowEnd)
               NbreJour =(((tSpan.Days / 7) * 5) + Math.Max((Math.Min((dowEnd + 1), 6) - dowStart), 0));
           else
               NbreJour =(((tSpan.Days / 7) * 5) + Math.Min((dowEnd + 6) - Math.Min(dowStart, 6), 5));

           return NbreJour;
       }
       public static bool BulkUpdateData<T>(IEnumerable<T> pListObject, string TableName, string idTable, SqlCommand command)
       {
           DataTable dataTable;
           DataRow newDataRow;

           DataTable pDtSource = null;

           pDtSource = ListToDataTable(pListObject);

           Collection<DataRow> dataRowsList = new Collection<DataRow>();
           DataRow dataRowsObjet;

           DataRow[] rows = pDtSource.Select();

           foreach (DataRow item in rows)
           {
               dataRowsObjet = item;

               dataRowsList.Add(dataRowsObjet);
           }

           try
           {
               //Indication du nom de l'entité
               TableName = string.Format("{0}", TableName.ToLower().Trim());
               dataTable = new DataTable(TableName);

               #region /*Formattage de la requete de mise à jour*/

               StringBuilder strCreateTableStatement = new StringBuilder("CREATE TABLE #TmpTable(");
               StringBuilder strUpdateTargetTableStatement = new StringBuilder("UPDATE target_table SET ");
               string strTableKey = string.Format("{0}", idTable);

               foreach (DataColumn dataColumn in dataRowsList.First().Table.Columns)
               {
                   //retirer la clé primaire de la table parmi les champs à mettre à jour
                   if (!dataColumn.ColumnName.Equals(strTableKey, StringComparison.InvariantCultureIgnoreCase))
                       strUpdateTargetTableStatement.Append(string.Format(" target_table.{0} = source_table.{0},", dataColumn.ColumnName));

                   if (dataColumn.DataType == typeof(System.Guid))
                   {
                       strCreateTableStatement.Append(string.Format("{0} UNIQUEIDENTIFIER NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Guid));
                   }
                   else if (dataColumn.DataType == typeof(System.Int32))
                   {
                       strCreateTableStatement.Append(string.Format("{0} INT NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Int32));
                   }
                   else if (dataColumn.DataType == typeof(System.DateTime))
                   {
                       strCreateTableStatement.Append(string.Format("{0} DATETIME NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.DateTime));
                   }
                   else if (dataColumn.DataType == typeof(System.Decimal))
                   {
                       strCreateTableStatement.Append(string.Format("{0} money NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Decimal));
                   }
                   else if (dataColumn.DataType == typeof(System.Boolean))
                   {
                       strCreateTableStatement.Append(string.Format("{0} BIT NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Boolean));
                   }
                   else
                   {
                       strCreateTableStatement.Append(string.Format("{0} VARCHAR(MAX) NULL,", dataColumn.ColumnName));
                       dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.String));
                   }
               }

               //Pour chacune des lignes à insérer
               foreach (DataRow dataRow in dataRowsList)
               {
                   //on créé une nouvelle ligne
                   newDataRow = dataTable.NewRow();

                   //on parcourt les colonnes pour en affecter les valeurs
                   foreach (DataColumn item in dataRow.Table.Columns)
                   {
                       newDataRow[item.ColumnName] = item.DataType == typeof(System.DateTime) ? DateTime.Now : dataRow[item.ColumnName];
                   }

                   dataTable.Rows.Add(newDataRow);
               }

               strCreateTableStatement.Remove(strCreateTableStatement.Length - 1, 1);
               strUpdateTargetTableStatement.Remove(strUpdateTargetTableStatement.Length - 1, 1);
               strCreateTableStatement.Append(")");

               strUpdateTargetTableStatement.Append(string.Format(" FROM #TmpTable source_table INNER JOIN {0} target_table ON target_table.{1} = source_table.{1};  DROP TABLE #TmpTable;", dataTable.TableName, idTable));

               #endregion /*Formattage de la requete de mise à jour*/
                   try
                   {

                       //Création de table temporaire

                       command.CommandText = strCreateTableStatement.ToString();
                       command.ExecuteNonQuery();

                       //Insertion dans la table temporaire


                       using (var bulkcopy = new SqlBulkCopy(command.Connection, SqlBulkCopyOptions.TableLock, command.Transaction))
                       {
                           bulkcopy.BulkCopyTimeout = 30000;
                           bulkcopy.DestinationTableName = "#TmpTable";
                           bulkcopy.WriteToServer(dataTable);
                           bulkcopy.Close();
                       }

                       //Mise à jour dans la table de production
                       command.CommandTimeout = 30000;
                       command.CommandText = strUpdateTargetTableStatement.ToString();

                       //Exxécution
                       command.ExecuteNonQuery();
                   }
                   catch (SqlException sqlException)
                   {
                       return false;
                   }
                   catch (Exception ex)
                   {
                       return false;
                       // Handle exception properly
                   }
                   finally
                   {
                       command.Dispose();
                   }
           }
           catch (Exception ex)
           {
               return false;
           }
           return true;
       }


       public static void UpdateData(List<string> pValeurAModifer, List<string> pProprieteJointure, DataTable DataTable,
           string TabelName, string RequeteTableTemp,string Connexion)
       {
           SqlCommand command = new SqlCommand();
           SqlConnection conn = new SqlConnection(Connexion);
                   try
                   {
                       command.Connection = conn;
                       if (conn.State == ConnectionState.Closed)
                           conn.Open();
                       //Creating temp table on database
                       command.CommandText = RequeteTableTemp;
                       command.ExecuteNonQuery();

                       //Bulk insert into temp table
                       using (SqlBulkCopy bulkcopy = new SqlBulkCopy(command.Connection ))
                       {
                           bulkcopy.BulkCopyTimeout = 660;
                           bulkcopy.DestinationTableName = "#TmpTable";
                           bulkcopy.WriteToServer(DataTable);
                           bulkcopy.Close();
                       }

                       // Updating destination table, and dropping temp table
                      
                       command.CommandTimeout = 300;
                       command.CommandText = UpdateCommand(TabelName, pValeurAModifer, pProprieteJointure) + " ;DROP TABLE #TmpTable;";
                       command.ExecuteNonQuery();
                   }
                   catch (Exception ex)
                   {
                       throw ex;
                   }
                   finally
                   {
                       command.Dispose();
                   }
       }

       static string UpdateCommand(string tableName,List<string> pValeurAModifer,List<string> pProprieteJointure)
        {
            string sql = "UPDATE A SET ";
            string whereClause = string.Empty;
            bool firstLap = false;

            try
            {
                foreach (var f in pValeurAModifer)
                    sql += "A." + f + " = " + "T." + f + " , ";

                int lastCommaIndex = sql.LastIndexOf(',');
                sql = sql.Substring(0, lastCommaIndex);// +" FROM " + tableName;

                foreach (string  w in pProprieteJointure)
                {
                    if (!firstLap)
                        sql += " FROM " + tableName + " AS A INNER JOIN #TmpTable AS T ON  ";

                    sql +="A." + w + " = " + "T." + w + " AND ";
                    firstLap = true;
                }

                if (firstLap)
                {
                    int lastIndex = sql.LastIndexOf("AND");
                    sql = sql.Substring(0, lastIndex);
                }
                return sql;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static  void BulkInsertManyToManyRelationship(DataTable TablePere, string NomTable, Dictionary<string, DataTable> TableFille,string NomCleEtranger, SqlCommand cmd)
       {
           // Connect to the database.

               
               //SqlTransaction transaction = null;
               try
               {
                   //transaction = connection.BeginTransaction(); // Use one transaction to put all the data in the database
                   // Create 1.000 products.
                   int lastIdentity = 0;
                   if (cmd.Connection.State == ConnectionState.Closed)
                       cmd.Connection.Open();
                   BulkInsertTable(NomTable, TablePere, cmd.Connection,cmd.Transaction );
                   lastIdentity = GetIdentity(NomTable, cmd.Connection, cmd.Transaction);

                   foreach (var item in TableFille)
                   {
                       // Add all products to all customers, this will create 1.000.000 records
                       string TableName = item.Key ;
                        foreach (DataRow rows in item.Value.Rows)
                            rows[NomCleEtranger] = lastIdentity;
                        BulkInsertTable(TableName, item.Value, cmd.Connection, cmd.Transaction);
                   }
                   //transaction.Commit();
               }
               catch (Exception ex)
               {
                   throw ex;
               }
       }
       public static  DataTable GetSchemaInfo(string tableName, SqlConnection connection, SqlTransaction transaction)
       {
           DataTable dataTable = new DataTable();
           using (SqlCommand selectSchemaCommand = connection.CreateCommand())
           {
               selectSchemaCommand.CommandText = string.Format("set fmtonly on; select * from {0}", tableName);
               selectSchemaCommand.Transaction = transaction;
               using (var adapter = new SqlDataAdapter(selectSchemaCommand)) // Get only the schema information for table [Sale]
               {
                   adapter.FillSchema(dataTable, SchemaType.Source);
               }
           }
           return dataTable;
       }
       public static  int GetIdentity(string tableName, SqlConnection connection, SqlTransaction transaction)
       {
           int identity = 0;
           // Get the last customer identity
           using (SqlCommand sqlCommand = connection.CreateCommand())
           {
               sqlCommand.CommandText = string.Format("SELECT IDENT_CURRENT('{0}')", tableName);
               sqlCommand.Transaction = transaction;
               identity = Convert.ToInt32(sqlCommand.ExecuteScalar());
           }
           return identity;
       }
       public static  void BulkInsertTable(string tableName, DataTable dataTable, SqlConnection connection, SqlTransaction transaction)
       {
           using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction)) // Lock the table
           {
               sqlBulkCopy.DestinationTableName = tableName;
               sqlBulkCopy.BulkCopyTimeout = 3600;
               sqlBulkCopy.WriteToServer(dataTable);
           }
       }



       public static IList<T> ToList<T>(this DataTable table, Dictionary<string, string> mappings) where T : new()
       {
           IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
           IList<T> result = new List<T>();

           foreach (var row in table.Rows)
           {
               var item = CreateItemFromRow<T>((DataRow)row, properties, mappings);
               result.Add(item);
           }

           return result;
       }

       private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
       {
           T item = new T();
           foreach (var property in properties)
           {
               property.SetValue(item, row[property.Name], null);
           }
           return item;
       }

       private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties, Dictionary<string, string> mappings) where T : new()
       {
           T item = new T();
           foreach (var property in properties)
           {
               if (mappings.ContainsKey(property.Name))
                   property.SetValue(item, row[mappings[property.Name]], null);
           }
           return item;
       }
       public static string DernierJourDuMois(int mois, int annee)
       {
           try
           {
               DateTime now = DateTime.Now;
               int nbDays = DateTime.DaysInMonth(annee, mois);
               return new DateTime(annee, mois, nbDays, 23, 59, 59, 999).ToShortDateString();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
    }
}
