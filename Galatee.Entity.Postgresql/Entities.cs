using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;
using System.Configuration;
//using System.Data.EntityClient;
using System.Data.SqlClient;
//using System.Data.Entity.Core.EntityClient;
using System.Data.EntityClient;
using System.Security.Cryptography;
using System.IO;
//using System.Transactions;

namespace Galatee.Entity.Model
{
    public class Entities 
    {
        public static bool InsertEntity<T>(T entity) where T : class
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        context.Set<T>().Add(entity);
                        context.SaveChanges();
                        return true;
                    }
                    
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertEntity<T>(T entity,galadbEntities pContext) where T : class
        {
            try
            {
                    if (pContext.Entry(entity).State == EntityState.Detached)
                        pContext.Set<T>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool InsertEntity_Fraude<T>(T entity, Fraude.FRAUDES_Entities pContext) where T : class
        {
            try
            {
                    if (pContext.Entry(entity).State == EntityState.Detached)
                        pContext.Set<T>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool InsertEntity<T>(List<T> pEntity) where T : class
        {
            try
            {
                   using (galadbEntities context = new galadbEntities())
                    {
                        foreach (T _entity in pEntity)
                        {
                            if (context.Entry(_entity).State == EntityState.Detached)
                                context.Set<T>().Add(_entity);
                        }
                        context.SaveChanges();
                        return true;
                     }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertEntity<T>(List<T> pEntity,galadbEntities pContext) where T : class
        {
            try
            {
                foreach (T _entity in pEntity)
                {
                    if (pContext.Entry(_entity).State == EntityState.Detached)
                        pContext.Set<T>().Add(_entity);
                }
                        return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertEntityAbo07<T>(List<T> pEntity, ABO07Entities  pContext) where T : class
        {
            try
            {
         
                foreach (T _entity in pEntity)
                {
                    if (pContext.Entry(_entity).State == EntityState.Detached)
                        pContext.Set<T>().Add(_entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertEntityAbo07<T>(T entity, ABO07Entities pContext) where T : class
        {
            try
            {
                if (pContext.Entry(entity).State == EntityState.Detached)
                    pContext.Set<T>().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool RemoveInsertEntity<T>(List<T> pEntity) where T : class
        {
            try
            {
                    using (galadbEntities context = new galadbEntities())
                    {
                        foreach (T _entity in pEntity) // suppression des habilitation dans la base
                        {
                            if (context.Entry(_entity).State == EntityState.Detached)
                            {
                                context.Set<T>().Attach(_entity);
                                context.Set<T>().Remove(_entity);
                            }
                        }
                        context.SaveChanges();

                        foreach (T _entity in pEntity) // suppression des habilitation dans la base
                        {
                            if (context.Entry(_entity).State == EntityState.Detached)
                            {
                                context.Set<T>().Attach(_entity);
                                context.Set<T>().Remove(_entity);
                            }
                        }
                        context.SaveChanges();

                        return true;
                    }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(T entity) where T : class
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        context.Set<T>().Attach(entity);
                        context.Entry(entity).State = EntityState.Modified;
                        context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(List<T> pEntity) where T : class
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (T _entity in pEntity)
                    {
                        if (context.Entry(_entity).State == EntityState.Detached)
                        {
                            context.Set<T>().Attach(_entity);
                            context.Entry(_entity).State = EntityState.Modified;
                        }
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(List<T> pEntity, galadbEntities pContext) where T : class
        {
            try
            {
                    foreach (T _entity in pEntity)
                    {
                        try
                        {
                            if (pContext.Entry(_entity).State == EntityState.Detached)
                                pContext.Set<T>().Attach(_entity);
                            pContext.Entry(_entity).State = EntityState.Modified;
                        }
                        catch 
                        {
                            continue;
                        }
                    }
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(T pEntity, galadbEntities pContext) where T : class
        {
            try
            {
                if (pContext.Entry(pEntity).State == EntityState.Detached)
                        pContext.Set<T>().Attach(pEntity);

                        pContext.Entry(pEntity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(List<T> pEntity, ABO07Entities pContext) where T : class
        {
            try
            {
                foreach (T _entity in pEntity)
                {
                    if (pContext.Entry(_entity).State == EntityState.Detached)
                        pContext.Set<T>().Attach(_entity);
                    pContext.Entry(_entity).State = EntityState.Modified;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateEntity<T>(T pEntity, ABO07Entities  pContext) where T : class
        {
            try
            {
                if (pContext.Entry(pEntity).State == EntityState.Detached)
                    pContext.Set<T>().Attach(pEntity);

                pContext.Entry(pEntity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool DeleteEntity<T>(T entity) where T : class
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        context.Set<T>().Attach(entity);
                        context.Set<T>().Remove(entity);
                        context.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteEntity<T>(List<T> pEntity) where T : class
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    foreach (T _entity in pEntity)
                    {
                        if (context.Entry(_entity).State == EntityState.Detached)
                        {
                            context.Set<T>().Attach(_entity);
                            context.Set<T>().Remove(_entity);
                        }
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteEntity<T>(List<T> pEntity, galadbEntities pContext) where T : class
        {
            try
            {
                    foreach (T _entity in pEntity)
                    {
                        if (pContext.Entry(_entity).State == EntityState.Detached)
                            pContext.Set<T>().Attach(_entity);
                            pContext.Set<T>().Remove(_entity);
                    }
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteEntity<T>(T pEntity, galadbEntities pContext) where T : class
        {
            try
            {
                if (pContext.Entry(pEntity).State == EntityState.Detached)
                    {
                        pContext.Set<T>().Attach(pEntity);
                        pContext.Set<T>().Remove(pEntity);
                    }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T ConvertObject<T,P>(P pSource) where T : new()
        {
            try
            {
                return Galatee.Tools.Utility.ConvertEntity<T, P>(pSource);
            }
            catch (Exception ex)
            {
              throw ex;
            }
        }

        public static List<T> ConvertObject<T, P>(List<P> pSource) where T : new()
        {
            try
            {
                List<T> liste = new List<T>();
                foreach( P p in pSource)
                    liste.Add(Galatee.Tools.Utility.ConvertEntity<T, P>(p));

                return liste;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> GetEntityListFromQuery<T>(DataTable pTable) where T : new()
        {
            string nonColone = string.Empty;
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
                        nonColone = f.Name;
                        if (ColListName.Contains(f.Name.ToUpper()))
                        {
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
                string cl = nonColone;
                throw ex;
            }
        }

        public static T GetEntityFromQuery<T>(DataTable pTable) where T : new()
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

                    foreach (var f in properties) // Récuperation des valeurs des proprietes de l'objet
                    {
                        if (ColListName.Contains(f.Name.ToUpper()))
                        {
                            var o = row[f.Name];
                            if (o.GetType() != typeof(DBNull)) f.SetValue(obj, o, null);
                        }
                    }
                    results.Add(obj);
                }

                return results.FirstOrDefault() ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> DataTableToCsvFormat(DataTable dt)
        {
            List<string> sb = new List<string>();

            //var columnNames = dt.Columns.Cast<DataColumn>().Select(column => "\"" + column.ColumnName.Replace("\"", "\"\"") + "\"").ToArray();
            //sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                sb.Add(string.Join(";", fields));
            }

            return sb;
        }
        //#region ECLIPSE
        //public static bool InsertEntity_Eclipse<T>(List<T> pEntity, EclipseEntities pContext) where T : class
        //{
        //    try
        //    {

        //        foreach (T _entity in pEntity)
        //        {
        //            if (pContext.Entry(_entity).State == EntityState.Detached)
        //                pContext.Set<T>().Add(_entity);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool InsertEntity_Eclipse<T>(T entity, EclipseEntities pContext) where T : class
        //{
        //    try
        //    {
        //        if (pContext.Entry(entity).State == EntityState.Detached)
        //            pContext.Set<T>().Add(entity);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateEntity_Eclipse<T>(List<T> pEntity, EclipseEntities pContext) where T : class
        //{
        //    try
        //    {
        //        foreach (T _entity in pEntity)
        //        {
        //            if (pContext.Entry(_entity).State == EntityState.Detached)
        //                pContext.Set<T>().Attach(_entity);
        //            pContext.Entry(_entity).State = EntityState.Modified;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateEntity_Eclipse<T>(T pEntity, EclipseEntities pContext) where T : class
        //{
        //    try
        //    {
        //        if (pContext.Entry(pEntity).State == EntityState.Detached)
        //            pContext.Set<T>().Attach(pEntity);

        //        pContext.Entry(pEntity).State = EntityState.Modified;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static bool InsertEntityBloc<T>(List<T> pEntity, string connectionString) where T : class
        {
            try
            {
                var sbCopy = new SqlBulkCopy(connectionString);
                sbCopy.DestinationTableName = typeof(T).Name;
                DataTable dt = Galatee.Tools.Utility.ListToDataTable<T>(pEntity);
                sbCopy.WriteToServer(dt);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //#endregion
    
        #region Gestion chaîne de connection

        public const string GaladbConnectionStringName = "galadbEntities";
        public const string Abo07ConnectionStringName = "ABO07Entities";
        public const string RpntConnectionStringName = "RPNTEntities";
        static string sUserId = "user id";
        static string sPassword = "password";

        static string BaseOracle = "ORACLE";
        static string BaseSqlServer = "SQL SERVER";
        static string BasePostgresql = "POSTGRESQL";

        public static string GetGaladbConnectionString()
        {
            try
            {
                return GetEntityConnexionString(GaladbConnectionStringName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetAbo07ConnectionString()
        {
            try
            {
                return GetEntityConnexionString(Abo07ConnectionStringName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetRpntConnectionString()
        {
            try
            {
                return GetEntityConnexionString(RpntConnectionStringName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetEntityConnexionString(string pConnectionStringName)
        {
            try
            {
                var myEntityConStringBuld = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[pConnectionStringName].ConnectionString);
                if (myEntityConStringBuld != null)
                    return ConnectionStringBuilder(myEntityConStringBuld.ConnectionString.Split(';'));
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string Decrypt(string input)
        {

            try
            {
                //byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159 }; // La clé est codée sur 192 bits
                byte[] _SaltByte = new byte[30] { 2, 32, 123, 70, 80, 21, 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
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
        public static string GetConnexionGaladb()
        {
            string connexionname = string.Empty;
            try
            {
                string DecryptValue = string.Empty;
                foreach (ConnectionStringSettings aValue in ConfigurationManager.ConnectionStrings)
                {
                    if (aValue.Name =="GALADB")
                    {
                         DecryptValue = Decrypt(aValue.ConnectionString);
                         break;
                    }
                }
                return DecryptValue;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GetConnexionAbo07()
        {
            string connexionname = string.Empty;
            try
            {
                string DecryptValue = string.Empty;
                foreach (ConnectionStringSettings aValue in ConfigurationManager.ConnectionStrings)
                {
                    if (aValue.Name == "ABO07")
                    {
                        DecryptValue = Decrypt(aValue.ConnectionString);
                        break;
                    }
                }
                return DecryptValue;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private static string ConnectionStringBuilder(string[] pPropertyArray)
        {
            string ConnectionString = string.Empty;
            try
            {
                foreach (string item in pPropertyArray)
                {
                    string value = string.Empty;
                    if (item.ToUpper().Contains(sUserId.ToUpper()))
                    {
                        value = sUserId + "=" + Galatee.Security.Connexion.GalaUserId;
                        ConnectionString = !string.IsNullOrEmpty(ConnectionString) ? ConnectionString + ";" + value : ConnectionString + value;
                        continue;
                    }
                    if (item.ToUpper().Contains(sPassword.ToUpper()))
                    {
                        value = sPassword + "=" + Galatee.Security.Connexion.GalaUserPassword + ";";
                        ConnectionString = !string.IsNullOrEmpty(ConnectionString) ? ConnectionString + ";" + value : ConnectionString + value;
                        continue;
                    }
                    ConnectionString = !string.IsNullOrEmpty(ConnectionString) ? ConnectionString + ";" + item : ConnectionString + item; 
                }
                return ConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private static string EntityConnectionStringBuilder(string pServerName,string pDataBaseName, Galatee.Structure.CsLibelle pDataBaseType)
        //{
        //    try
        //    {
        //        if (pDataBaseType.CODE.ToUpper() == BaseOracle.ToUpper())
        //        {

        //        }
        //        if (pDataBaseType.CODE.ToUpper() == BaseSqlServer.ToUpper())
        //        {
        //            string sChaine = "Data Source={0};Initial Catalog={1};User ID={2};Password={3}";
        //            sChaine = string.Format(sChaine, pServerName, pDataBaseName, Galatee.Security.Connexion.GalaUserId, Galatee.Security.Connexion.GalaUserPassword);
        //            SqlConnection Connexion = new SqlConnection(sChaine);
        //            try
        //            {
        //                Connexion.Open();
        //            }
        //            catch //(Exception ex)
        //            {
        //                result = false;
        //                //throw ex;
        //            }
        //            finally
        //            {
        //                Connexion.Close();
        //            }
        //        }
        //        if (pDataBaseType.CODE.ToUpper() == BasePostgresql.ToUpper())
        //        {

        //        }
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}

        public static EntityConnection GetEntityConnectionString(string pConnectionStringName)
        {
            try
            {
                return new EntityConnection(GetEntityConnexionString(pConnectionStringName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool TesterEntityConnection(EntityConnection pEntityConnection, out string pErreur)
        {
            pErreur = string.Empty;
            try
            {
                pEntityConnection.Open();
                pEntityConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                pErreur = ex.Message;
                return false;
            }
        }

        public static bool TesterEntityConnection()
        {
            try
            {
                var pEntityConnection = GetEntityConnectionString(GaladbConnectionStringName);

                pEntityConnection.Open();
                pEntityConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion          
    }
}
