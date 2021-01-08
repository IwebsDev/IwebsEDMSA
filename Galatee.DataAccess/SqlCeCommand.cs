using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.DataAccess
{
    public static class SqlCeCommand
    {
    //    public static bool InsertEntities<T>(T entity) where T : new()
    //    {
    //        try
    //        {
    //            string commandText = InsertCommand<T>();
    //            using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //            {
    //                cn.Open();

    //                using (var cmd = cn.CreateCommand())
    //                {
    //                    cmd.CommandText = commandText;
    //                    CommandeParamValues(cmd, entity);
    //                    cmd.ExecuteNonQuery();
    //                }
    //                return true;
    //            }
    //        }
    //        catch (System.Data.SqlClient.SqlException ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool InsertEntities<T>(List<T> pListeGeneric) where T : new()
    //    {
    //        try
    //        {
    //            string commandText = InsertCommand<T>();

    //            foreach (T generic in pListeGeneric)
    //            {
    //                int Result = 0;
    //                using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //                {
    //                    cn.Open();

    //                    using (var cmd = cn.CreateCommand())
    //                    {
    //                        cmd.CommandText = commandText;
    //                        CommandeParamValues(cmd, generic);

    //                        Result = cmd.ExecuteNonQuery();
    //                    }
    //                }
    //            }
    //        }
    //        catch (System.Data.SqlClient.SqlException ex)
    //        {
    //            throw ex;
    //        }

    //        return true;
    //    }

    //    public static bool DeleteEntity<T>() where T : new()
    //    {
    //        try
    //        {
    //            string table = typeof(T).Name;
    //            string commmandText = "DELETE FROM " + table;

    //            using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //            {
    //                cn.Open();
    //                using (var cmd = cn.CreateCommand())
    //                {
    //                    cmd.CommandText = commmandText;
    //                    cmd.ExecuteNonQuery();
    //                }
    //                return true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool DeleteEntity<T>(Dictionary<string, object> pSearchProperties)
    //    {
    //        try
    //        {
    //            string commandText = DeleteCommand<T>(pSearchProperties);

    //            using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //            {
    //                cn.Open();
    //                using (var cmd = cn.CreateCommand())
    //                {
    //                    cmd.CommandText = commandText;
    //                    CommandeParamValuesDelete(cmd, pSearchProperties);
    //                    cmd.ExecuteNonQuery();
    //                }
    //                return true;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool UpdateEntity<T>(T entity, List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties) where T : new()
    //    {
    //        try
    //        {
    //            string table = typeof(T).Name;
    //            string sql = UpdateCommand<T>(pTargetedProperties, pSearchProperties);

    //            using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //            {
    //                cn.Open();
    //                using (var cmd = cn.CreateCommand())
    //                {
    //                    cmd.CommandText = sql;
    //                    CommandeParamValuesUpdate(cmd, entity, pTargetedProperties, pSearchProperties);
    //                    cmd.ExecuteNonQuery();
    //                }
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool UpdateEntity<T>(T entity, List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties, System.Data.SqlServerCe.SqlCeConnection cn) where T : new()
    //    {
    //        try
    //        {
    //            string table = typeof(T).Name;
    //            string sql = UpdateCommand<T>(pTargetedProperties, pSearchProperties);
    //            using (var cmd = cn.CreateCommand())
    //            {
    //                cmd.CommandText = sql;
    //                CommandeParamValuesUpdate(cmd, entity, pTargetedProperties, pSearchProperties);
    //                cmd.ExecuteNonQuery();
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool UpdateEntity<T>(List<T> entityList, List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties) where T : new()
    //    {
    //        try
    //        {
    //            string table = typeof(T).Name;
    //            string commandText = UpdateCommand<T>(pTargetedProperties, pSearchProperties);

    //            foreach (var item in entityList)
    //            {
    //                using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //                {
    //                    cn.Open();
    //                    using (var cmd = cn.CreateCommand())
    //                    {
    //                        cmd.CommandText = commandText;
    //                        CommandeParamValuesUpdate(cmd, item, pTargetedProperties, pSearchProperties);
    //                        cmd.ExecuteNonQuery();
    //                    }
    //                }
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static bool UpdateEntity<T>(List<T> entityList, List<string> pTargetedProperties, List<string> pSearchProperties) where T : new()
    //    {
    //        try
    //        {
    //            string table = typeof(T).Name;
    //            Dictionary<string, object> pSearchPropertiesDico = null;
    //            int result = 0;
    //            foreach (var item in entityList)
    //            {

    //                using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //                {
    //                    cn.Open();
    //                    using (var cmd = cn.CreateCommand())
    //                    {
    //                        pSearchPropertiesDico = new Dictionary<string, object>();
    //                        BuildDicoItem(pSearchPropertiesDico, pSearchProperties, item);
    //                        string commandText = UpdateCommand<T>(pTargetedProperties, pSearchPropertiesDico);
    //                        cmd.CommandText = commandText;
    //                        CommandeParamValuesUpdate(cmd, item, pTargetedProperties, pSearchPropertiesDico);
    //                        result = cmd.ExecuteNonQuery();
    //                    }
    //                }
    //            }
    //            return (result <= 0 ? false : true);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    private static void BuildDicoItem<T>(Dictionary<string, object> pSearchPropertiesDico, List<string> pSearchProperties, T item) where T : new()
    //    {
    //        try
    //        {
    //            foreach (var items in pSearchProperties)
    //            {
    //                object o = GetEntityPropertyValue(item, items);
    //                pSearchPropertiesDico.Add(items, o);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public static IEnumerable<T> Where<T>(Dictionary<string, object> pSearchProperties) where T : new()
    //    {

    //        string table = typeof(T).Name;
    //        var properties = typeof(T).GetProperties();
    //        string sql = pSearchProperties == null ? WhereCommand<T>() : WhereCommand<T>(pSearchProperties);
    //        //sql = "SELECT RELEVEUR,NOM FROM MODULE WHERE RELEVEUR = @WpRELEVEUR ";
    //        using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //        {
    //            cn.Open();
    //            using (var cmd = cn.CreateCommand())
    //            {
    //                cmd.CommandText = sql;
    //                if (pSearchProperties != null)
    //                    CommandeParamValuesWhere(cmd, pSearchProperties);
    //                using (var reader = cmd.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        var element = new T();

    //                        foreach (var f in properties)
    //                        {
    //                            var o = reader[f.Name];
    //                            if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //                        }
    //                        yield return element;
    //                    }
    //                }

    //            }
    //            cn.Close();
    //        }
    //    }

    //    public static IEnumerable<T> Where<T>(Dictionary<string, object> pSearchProperties, Dictionary<string, object> pSearchWithDiffValue) where T : new()
    //    {

    //        string table = typeof(T).Name;
    //        var properties = typeof(T).GetProperties();
    //        string sql = WhereCommand<T>(pSearchProperties, pSearchWithDiffValue);
    //        //sql = "SELECT RELEVEUR,NOM FROM MODULE WHERE RELEVEUR = @WpRELEVEUR ";
    //        using (var cn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //        {
    //            cn.Open();
    //            using (var cmd = cn.CreateCommand())
    //            {
    //                cmd.CommandText = sql;
    //                CommandeParamValuesWhere(cmd, pSearchProperties, pSearchWithDiffValue);
    //                using (var reader = cmd.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        var element = new T();

    //                        foreach (var f in properties)
    //                        {
    //                            var o = reader[f.Name];
    //                            if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //                        }
    //                        yield return element;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public static T FirsOrDefault<T>(Dictionary<string, object> pSearchProperties) where T : new()
    //    {
    //        return Where<T>(pSearchProperties).FirstOrDefault();
    //    }

    //    //public static string UpdateCommand<T>(T entity, List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties) where T : new()
    //    //{
    //    //        string tableName = typeof(T).Name;
    //    //        string sql = "UPDATE SET ";
    //    //        try
    //    //        {
    //    //            var properties = typeof(T).GetProperties();
    //    //            var element = new T();
    //    //            int propertiesCount = properties.Count();
    //    //            int increment = 0;
    //    //            pTargetedProperties.ForEach(c => c.ToUpper());

    //    //            foreach (var f in properties)
    //    //            {
    //    //                increment++;
    //    //                if (increment != propertiesCount)
    //    //                {
    //    //                    if (pTargetedProperties.Contains(f.Name.ToUpper()))
    //    //                    {
    //    //                        sql += f.Name.ToUpper() + " = " + (GetEntityPropertyValue(entity,f.Name.ToUpper()) as string) + " , ";
    //    //                    }
    //    //                }
    //    //                else
    //    //                {
    //    //                    if (pTargetedProperties.Contains(f.Name.ToUpper()))
    //    //                    {
    //    //                        sql += f.Name.ToUpper() + " = " + (GetEntityPropertyValue(entity, f.Name.ToUpper()) as string) + " , ";
    //    //                    }

    //    //                    int lastCommaIndex = sql.LastIndexOf(',');
    //    //                    sql = sql.Substring(0, lastCommaIndex) + " FROM " + tableName;
    //    //                    sql = CreateWhereClause(sql, pSearchProperties);
    //    //                }
    //    //            }

    //    //            return sql;
    //    //        }
    //    //        catch (Exception ex)
    //    //        {
    //    //            throw ex;
    //    //        }
    //    //}

    //    static string UpdateCommand<T>(T entity, List<string> pTargetedProperties) where T : new()
    //    {
    //        string tableName = typeof(T).Name;
    //        string sql = "UPDATE SET ";
    //        try
    //        {
    //            var properties = typeof(T).GetProperties();
    //            var element = new T();
    //            int propertiesCount = properties.Count();
    //            int increment = 0;
    //            pTargetedProperties.ForEach(c => c.ToUpper());

    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                if (increment != propertiesCount)
    //                {
    //                    if (pTargetedProperties.Contains(f.Name.ToUpper()))
    //                    {
    //                        sql += f.Name.ToUpper() + " = " + (GetEntityPropertyValue(entity, f.Name.ToUpper()) as string) + " , ";
    //                    }
    //                }
    //                else
    //                {
    //                    int lastCommaIndex = sql.LastIndexOf(',');
    //                    sql = sql.Substring(0, lastCommaIndex);
    //                }
    //            }

    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string CreateWhereClause(string sql, Dictionary<string, object> pSearchProperties)
    //    {
    //        try
    //        {
    //            string whereClause = string.Empty;
    //            bool firstLap = false;
    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //            {
    //                if (!firstLap)
    //                    whereClause = " WHERE ";

    //                whereClause += w.Key + " = " + w.Value + " , ";
    //                firstLap = true;
    //            }

    //            if (firstLap)
    //            {
    //                int lastIndex = whereClause.LastIndexOf(',');
    //                whereClause.Substring(0, lastIndex);
    //            }
    //            sql += whereClause;
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static object GetEntityPropertyValue<T>(T entity, string pProperty) where T : new()
    //    {
    //        try
    //        {
    //            // column names 
    //            System.Reflection.PropertyInfo[] oProps = null;
    //            oProps = null;
    //            if (oProps == null)
    //            {
    //                oProps = entity.GetType().GetProperties();
    //                foreach (var pi in oProps)
    //                {
    //                    if (pi.Name.Equals(pProperty))
    //                    {
    //                        object o = pi.GetValue(entity, null) ?? DBNull.Value;
    //                        return o;
    //                    }
    //                }
    //            }
    //            return null;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static bool CommandeParamValues(System.Data.SqlServerCe.SqlCeCommand pCmd, System.Data.SqlClient.SqlDataReader pReader, List<string> pParam)
    //    {
    //        try
    //        {
    //            int i = 0;
    //            foreach (string cas in pParam)
    //            {
    //                try
    //                {
    //                    string readerParam = pParam[i].Substring(2);
    //                    pCmd.Parameters.AddWithValue(pParam[i++], pReader[readerParam]);
    //                }
    //                catch (Exception ex)
    //                {
    //                    throw ex;
    //                }
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static bool CommandeParamValues<T>(System.Data.SqlServerCe.SqlCeCommand pCmd, T pEntity) where T : new()
    //    {
    //        // column names 
    //        System.Reflection.PropertyInfo[] oProps = null;
    //        try
    //        {
    //            string dollar = "@p";
    //            oProps = null;
    //            if (oProps == null)
    //            {
    //                oProps = pEntity.GetType().GetProperties();
    //                foreach (var pi in oProps)
    //                {
    //                    var o = pi.GetValue(pEntity, null) ?? DBNull.Value;
    //                    pCmd.Parameters.AddWithValue(dollar + pi.Name, o);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return true;
    //    }

    //    static bool CommandeParamValuesUpdate<T>(System.Data.SqlServerCe.SqlCeCommand pCmd, T pEntity, List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties) where T : new()
    //    {
    //        // column names 
    //        try
    //        {
    //            string dollarSet = "@p";
    //            string dollarWhere = "@Wp";
    //            foreach (var prop in pTargetedProperties)
    //            {
    //                var o = GetEntityPropertyValue(pEntity, prop);
    //                pCmd.Parameters.AddWithValue(dollarSet + prop, o);
    //            }

    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //                pCmd.Parameters.AddWithValue(dollarWhere + w.Key, w.Value);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static bool CommandeParamValuesDelete(System.Data.SqlServerCe.SqlCeCommand pCmd, Dictionary<string, object> pSearchProperties)
    //    {
    //        try
    //        {
    //            string dollarWhere = "@Wp";
    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //                pCmd.Parameters.AddWithValue(dollarWhere + w.Key, w.Value);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static bool CommandeParamValuesWhere(System.Data.SqlServerCe.SqlCeCommand pCmd, Dictionary<string, object> pSearchProperties)
    //    {
    //        try
    //        {
    //            string dollarWhere = "@Wp";
    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //                pCmd.Parameters.AddWithValue(dollarWhere + w.Key, w.Value);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static bool CommandeParamValuesWhere(System.Data.SqlServerCe.SqlCeCommand pCmd, Dictionary<string, object> pSearchProperties, Dictionary<string, object> pSearchWithDiffValue)
    //    {
    //        try
    //        {
    //            string dollarWhere = "@Wp";
    //            if (pSearchProperties != null)
    //                foreach (KeyValuePair<string, object> w in pSearchProperties)
    //                    pCmd.Parameters.AddWithValue(dollarWhere + w.Key, w.Value);

    //            if (pSearchWithDiffValue != null)
    //                foreach (KeyValuePair<string, object> w in pSearchWithDiffValue)
    //                    pCmd.Parameters.AddWithValue(dollarWhere + w.Key, w.Value);

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }


    //    public static IEnumerable<T> DataBaseToEntity<T>(string connectionstring, string sql) where T : new()
    //    {
    //        var properties = typeof(T).GetProperties();

    //        using (var conn = new System.Data.SqlClient.SqlConnection(connectionstring))
    //        {
    //            using (var comm = new System.Data.SqlClient.SqlCommand(sql, conn))
    //            {
    //                conn.Open();
    //                using (var reader = comm.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        var element = new T();

    //                        foreach (var f in properties)
    //                        {
    //                            var o = reader[f.Name];
    //                            if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //                        }
    //                        yield return element;
    //                    }
    //                }
    //                conn.Close();
    //            }
    //        }
    //    }

    //    public static IEnumerable<T> GetEntitiesFromDataBase<T>() where T : new()
    //    {
    //        var properties = typeof(T).GetProperties();

    //        using (var conn = new System.Data.SqlServerCe.SqlCeConnection(Global.ChaineConnexionLocalDb))
    //        {
    //            using (var comm = conn.CreateCommand())
    //            {
    //                conn.Open();
    //                string sql = WhereCommand<T>();
    //                comm.CommandText = sql;
    //                using (var reader = comm.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        var element = new T();

    //                        foreach (var f in properties)
    //                        {
    //                            var o = reader[f.Name];
    //                            if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //                        }
    //                        yield return element;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    public static bool Pull<T>(string connectionstring, string sql) where T : new()
    //    {
    //        try
    //        {
    //            var properties = typeof(T).GetProperties();

    //            using (var conn = new System.Data.SqlClient.SqlConnection(connectionstring))
    //            {
    //                using (var comm = new System.Data.SqlClient.SqlCommand(sql, conn))
    //                {
    //                    conn.Open();
    //                    using (var reader = comm.ExecuteReader())
    //                    {
    //                        while (reader.Read())
    //                        {
    //                            var element = new T();

    //                            foreach (var f in properties)
    //                            {
    //                                var o = reader[f.Name];
    //                                if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //                            }
    //                            //yield return element;
    //                        }
    //                    }
    //                    conn.Close();
    //                }
    //            }
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {

    //            throw ex;
    //        }
    //    }

    //    #region DataDefinitionLanguage

    //    static string InsertCommand<T>(List<string> pParamArray) where T : new()
    //    {
    //        //        cmd.CommandText = "INSERT INTO CASIND(CENTRE, CAS, LIBELLE, ENQUETE, FACTURE, LIBFAC,CASGEN1 " +
    //        //         ") VALUES (@pCENTRE, @CAS, @LIBELLE, @ENQUETE, @FACTURE, @LIBFAC, @CASGEN1)";

    //        string tableName = typeof(T).Name;
    //        string sql = "INSERT INTO " + tableName + "(";
    //        string values = "  VALUES(";
    //        string sign = "@p";
    //        try
    //        {
    //            var properties = typeof(T).GetProperties();
    //            var element = new T();
    //            int propertiesCount = properties.Count();
    //            int increment = 0;

    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                    values += sign + f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + ")";
    //                    values += sign + f.Name.ToUpper() + ")";
    //                }

    //                pParamArray.Add(sign + f.Name.ToUpper());
    //                //if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
    //            }

    //            return sql + values;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string InsertCommand<T>() where T : new()
    //    {
    //        //        cmd.CommandText = "INSERT INTO CASIND(CENTRE, CAS, LIBELLE, ENQUETE, FACTURE, LIBFAC,CASGEN1 " +
    //        //         ") VALUES (@pCENTRE, @CAS, @LIBELLE, @ENQUETE, @FACTURE, @LIBFAC, @CASGEN1)";

    //        string tableName = typeof(T).Name;
    //        string sql = "INSERT INTO " + tableName + "(";
    //        string values = "  VALUES(";
    //        string sign = "@p";

    //        try
    //        {
    //            var properties = typeof(T).GetProperties();
    //            var element = new T();
    //            int propertiesCount = properties.Count();
    //            int increment = 0;

    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                    values += sign + f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + ")";
    //                    values += sign + f.Name.ToUpper() + ")";
    //                }

    //            }

    //            return sql + values;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string InsertCommand(object t)
    //    {
    //        //        cmd.CommandText = "INSERT INTO CASIND(CENTRE, CAS, LIBELLE, ENQUETE, FACTURE, LIBFAC,CASGEN1 " +
    //        //         ") VALUES (@pCENTRE, @CAS, @LIBELLE, @ENQUETE, @FACTURE, @LIBFAC, @CASGEN1)";
    //        string namespaces = t.GetType().Namespace;
    //        Type ControlType = null;
    //        string tableName = t.GetType().UnderlyingSystemType.FullName.Substring(namespaces.Length + 1);
    //        //obj = Activator.CreateInstance(ControlType);

    //        //string tableName = t.GetType().FullName.Substring(namespaces.Length + 1);
    //        string sql = "INSERT INTO " + tableName + "(";
    //        string values = "  VALUES(";
    //        string sign = "@p";

    //        try
    //        {
    //            ControlType = t.GetType();
    //            //ControlType = Type.GetType(t.UnderlyingSystemType.FullName);
    //            //ControlType = Assembly.GetExecutingAssembly().GetType(t.UnderlyingSystemType.FullName);
    //            //var element = Activator.CreateInstance(ControlType);
    //            var properties = t.GetType().GetProperties();
    //            int propertiesCount = properties.Count();
    //            int increment = 0;

    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                    values += sign + f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + ")";
    //                    values += sign + f.Name.ToUpper() + ")";
    //                }

    //            }

    //            return sql + values;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string UpdateCommand<T>(List<string> pTargetedProperties, Dictionary<string, object> pSearchProperties)
    //    {
    //        string signSet = "@p";
    //        string signWh = "@Wp";
    //        string tableName = typeof(T).Name;
    //        string sql = "UPDATE " + tableName + " SET ";
    //        string whereClause = string.Empty;
    //        bool firstLap = false;

    //        try
    //        {
    //            foreach (var f in pTargetedProperties)
    //                sql += f + " = " + signSet + f + " , ";

    //            int lastCommaIndex = sql.LastIndexOf(',');
    //            sql = sql.Substring(0, lastCommaIndex);// +" FROM " + tableName;

    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //            {
    //                if (!firstLap)
    //                    sql += " WHERE ";

    //                sql += w.Key + " = " + signWh + w.Key + " AND ";
    //                firstLap = true;
    //            }

    //            if (firstLap)
    //            {
    //                int lastIndex = sql.LastIndexOf("AND");
    //                sql = sql.Substring(0, lastIndex);
    //            }
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string DeleteCommand<T>(Dictionary<string, object> pSearchProperties)
    //    {
    //        string signWh = "@Wp";
    //        string tableName = typeof(T).Name;
    //        string sql = "DELETE FROM " + tableName;
    //        string whereClause = string.Empty;
    //        bool firstLap = false;

    //        try
    //        {
    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //            {
    //                if (!firstLap)
    //                    sql += " WHERE ";

    //                sql += w.Key + " = " + signWh + w.Key + " AND ";
    //                firstLap = true;
    //            }

    //            if (firstLap)
    //            {
    //                int lastIndex = sql.LastIndexOf("AND");
    //                sql = sql.Substring(0, lastIndex);
    //            }
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string WhereCommand<T>(Dictionary<string, object> pSearchProperties)
    //    {
    //        string signWh = "@Wp";
    //        string tableName = typeof(T).Name;
    //        string sql = "SELECT ";// +tableName;
    //        string whereClause = string.Empty;
    //        bool firstLap = false;
    //        var properties = typeof(T).GetProperties();
    //        int propertiesCount = properties.Count();
    //        int increment = 0;

    //        try
    //        {
    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                //if(!firstLap)
    //                //    sql += f.Name.ToUpper() + ", ";

    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + " FROM " + tableName;
    //                }
    //            }
    //            foreach (KeyValuePair<string, object> w in pSearchProperties)
    //            {
    //                if (!firstLap)
    //                    sql += " WHERE ";

    //                sql += w.Key + " = " + signWh + w.Key + " AND ";
    //                firstLap = true;
    //            }

    //            if (firstLap)
    //            {
    //                int lastIndex = sql.LastIndexOf("AND");
    //                sql = sql.Substring(0, lastIndex);
    //            }
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string WhereCommand<T>(Dictionary<string, object> pSearchProperties, Dictionary<string, object> pSearchWithDiffProp)
    //    {
    //        string signWh = "@Wp";
    //        string tableName = typeof(T).Name;
    //        string sql = "SELECT ";// +tableName;
    //        string whereClause = string.Empty;
    //        bool firstLap = false;
    //        var properties = typeof(T).GetProperties();
    //        int propertiesCount = properties.Count();
    //        int increment = 0;

    //        try
    //        {
    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                //if(!firstLap)
    //                //    sql += f.Name.ToUpper() + ", ";

    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + " FROM " + tableName;
    //                }
    //            }
    //            if (pSearchProperties != null)
    //                foreach (KeyValuePair<string, object> w in pSearchProperties)
    //                {
    //                    if (!firstLap)
    //                        sql += " WHERE ";

    //                    sql += w.Key + " = " + signWh + w.Key + " AND ";
    //                    firstLap = true;
    //                }

    //            if (pSearchWithDiffProp != null)
    //                foreach (KeyValuePair<string, object> w in pSearchWithDiffProp)
    //                {
    //                    if (pSearchProperties == null)
    //                        if (!firstLap)
    //                            sql += " WHERE ";

    //                    sql += w.Key + " != " + signWh + w.Key + " AND ";
    //                    firstLap = true;
    //                }

    //            if (firstLap)
    //            {
    //                int lastIndex = sql.LastIndexOf("AND");
    //                sql = sql.Substring(0, lastIndex);
    //            }
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    static string WhereCommand<T>()
    //    {
    //        string signSet = "@p";
    //        string signWh = "@Wp";
    //        string tableName = typeof(T).Name;
    //        string sql = "SELECT ";// +tableName;
    //        string whereClause = string.Empty;
    //        bool firstLap = false;
    //        var properties = typeof(T).GetProperties();
    //        int propertiesCount = properties.Count();
    //        int increment = 0;

    //        try
    //        {
    //            foreach (var f in properties)
    //            {
    //                increment++;
    //                //if(!firstLap)
    //                //    sql += f.Name.ToUpper() + ", ";

    //                if (increment != propertiesCount)
    //                {
    //                    sql += f.Name.ToUpper() + ", ";
    //                }
    //                else
    //                {
    //                    sql += f.Name.ToUpper() + " FROM " + tableName;
    //                }
    //            }
    //            return sql;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    #endregion

    }
}
