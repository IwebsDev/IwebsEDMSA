using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Inova.Tools.Utilities;
using Galatee.Structure;


namespace Galatee.DataAccess
{
    public class DB_TA : Galatee.DataAccess.Parametrage.DbBase
    {
        public DB_TA()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        public DB_TA(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        private string ConnectionString;
        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;


        public void DeleteTa(string Centre, string Code, short Numero)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteTa;
            cmd.Parameters.Clear();

            SqlParameter param1 = cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar);
            param1.Direction = ParameterDirection.Input;
            SqlParameter param2 = cmd.Parameters.Add("@CODE", SqlDbType.VarChar);
            param2.Direction = ParameterDirection.Input;
            SqlParameter param3 = cmd.Parameters.Add("@NUM", SqlDbType.SmallInt);
            param3.Direction = ParameterDirection.Input;
            // recuperer les informations de la ligne selectionnée
            param1.Value = Centre;
            param2.Value = Code;
            param3.Value = Numero;


            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);

            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectTaCodeLibelleByNum + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public DataSet SELECT_ALL_By_NUMTABLE(int NUM)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTa;
            SqlParameter param = new SqlParameter("@NUM", SqlDbType.SmallInt);
            param.Direction = ParameterDirection.Input;
            param.Value = NUM;
            cmd.Parameters.Add(param);


            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                CommitTransaction(cmd.Transaction);

                return ds;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectTa + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsTa> SELECT_CodeLibelle_By_NUM(int NUM)
        {
            CsTa _ta;
            List<CsTa> ListeTA = new List<CsTa>();

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTaCodeLibelleByNum;
            SqlParameter param = new SqlParameter("@NUM", SqlDbType.SmallInt);
            param.Direction = ParameterDirection.Input;
            param.Value = NUM;
            cmd.Parameters.Add(param);

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                    _ta = new CsTa();
                    //_ta.CODE = reader.GetValue(0).ToString().Trim();
                    _ta.LIBELLE = reader.GetValue(1).ToString().Trim();
                    ListeTA.Add(_ta);
                }
                //Fermeture reader
                reader.Close();
                return ListeTA;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTaCodeLibelleByNum + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public CsTa SELECT_Exploitation(string Code)
        {
            CsTa _ta = null;

            cn = new SqlConnection(ConnectionString); 
            SqlCommand command = new SqlCommand("SPX_TA_SELECT_VALEUR_58_By_CODE", cn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.Add("@Code", SqlDbType.VarChar, 6).Value = Code;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                    _ta = new CsTa();
                    //_ta.CODE = reader.GetValue(0).ToString().Trim();
                    _ta.LIBELLE = reader.GetValue(1).ToString().Trim();
                }
                //Fermeture reader
                reader.Close();
                return _ta;
            }
            catch (SqlException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_TA_SELECT_VALEUR_58_By_CODE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                command.Dispose();
            }
        }

        public string SELECT_Produit_By_CodeProduit(string CodeProduit)
        {
            string Libelle = string.Empty;

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTaByCodeProduit;

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODE", SqlDbType.VarChar).Value = CodeProduit;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                //if (!reader.HasRows)
                //{
                //    reader.Close();
                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                //}
                while (reader.Read())
                {
                    Libelle = reader.GetValue(0).ToString().Trim();
                }
                //Fermeture reader
                reader.Close();
                return Libelle;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTaByCodeProduit + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        //public string SELECT_Produits()
        //{
        //    string Libelle = string.Empty;

        //    cn = new SqlConnection(ConnectionString);

        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.SelectTaAllProduit;

        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Libelle = reader.GetValue(0).ToString().Trim();
        //        }
        //        //Fermeture reader
        //        reader.Close();
        //        return Libelle;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(EnumProcedureStockee.SelectTaByCodeProduit + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        public int SELECT_Libelle_Verifie_UtilisationAjufin()
        {
            int Libelle = 0;

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTaLibelleVerifieUtilisationAJUFIN;

            try
            {

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                    Libelle = int.Parse(reader.GetValue(0).ToString().Trim());
                }
                reader.Close();
                return Libelle;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTaByCodeProduit + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        public List<CsZone> SELECT_INIT_SELECT_COLUMNS_BY_NTABLE(int NTable)
        {
            CsZone _col;
            List<CsZone> columns = new List<CsZone>();
            //Ajout de chaine vide
            CsZone _colvide = new CsZone();
            _colvide.Code = string.Empty;
            _colvide.Libelle = string.Empty;
            _colvide.Table = string.Empty;
            columns.Add(_colvide);

            //Objet connection
            SqlConnection connection = new SqlConnection(ConnectionString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_INIT_SELECT_COLUMNS_BY_NTABLE", connection);
            command.CommandType = CommandType.StoredProcedure;


            SqlParameter param = command.Parameters.Add(new SqlParameter("@NTable", NTable));
            param.Direction = ParameterDirection.Input;

            //Ouverture
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            try
            {
                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();

                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                    _col = new CsZone();
                    _col.Code = reader.GetValue(0).ToString().Trim();
                    _col.Libelle = reader.GetValue(1).ToString().Trim();
                    _col.Table = reader.GetValue(2).ToString().Trim();
                    if (reader.GetValue(3).ToString().Trim() != string.Empty)
                        _col.ColId = int.Parse(reader.GetValue(3).ToString().Trim());
                    columns.Add(_col);
                }
                //Fermeture reader
                reader.Close();

                return columns;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                connection.Close();
            }
        }


        public List<CsZone> SELECT_INIT_SELECT_COLUMNS()
        {
            CsZone _col;
            List<CsZone> columns = new List<CsZone>();

            //Objet connection
            SqlConnection connection = new SqlConnection(ConnectionString);
            //Objet Command
            SqlCommand command = new SqlCommand("SPX_INIT_SELECT_COLUMNS", connection);
            command.CommandType = CommandType.StoredProcedure;


            //SqlParameter param = command.Parameters.Add(new SqlParameter("@Table", Table));
            //param.Direction = ParameterDirection.Input;

            //Ouverture
            connection.Open();

            try
            {
                //Object datareader
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();

                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                    _col = new CsZone();
                    _col.Code = reader.GetValue(0).ToString().Trim();
                    _col.Libelle = reader.GetValue(1).ToString().Trim();
                    _col.Table = reader.GetValue(2).ToString().Trim();
                    if (reader.GetValue(3).ToString().Trim() != string.Empty)
                        _col.ColId = int.Parse(reader.GetValue(3).ToString().Trim());
                    columns.Add(_col);
                }
                //Fermeture reader
                reader.Close();

                return columns;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                //Fermeture base
                connection.Close();
            }
        }


        public void Update(List<aTa> rows)
        {
            foreach (aTa row in rows)
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.UpdateTa.Trim();

                cmd.Parameters.Clear();
                try
                {

                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@CODE", SqlDbType.SmallInt).Value = row.CODE;
                    cmd.Parameters.Add("@NUM", SqlDbType.SmallInt).Value = row.NUM;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value =  row.ROWID;

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);
                    cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                    CommitTransaction(cmd.Transaction);

                }

                catch (Exception ex)
                {
                    RollBackTransaction(cmd.Transaction);
                    throw ex;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
        }

        public void Insert(List<aTa> row)
        {
            foreach (aTa ta in row)
            {
                int rowsAffected = -1;
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.InsertTa.Trim();

                cmd.Parameters.Clear();
                try
                {
                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = ta.CENTRE;
                    cmd.Parameters.Add("@NUM", SqlDbType.SmallInt).Value = ta.NUM;
                    cmd.Parameters.Add("@CODE", SqlDbType.SmallInt).Value = ta.CODE;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = ta.TRANS;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = ta.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = ta.DMAJ;

                    DBBase.SetDBNullParametre(cmd.Parameters);     

                    if (cn.State == ConnectionState.Closed)
                           cn.Open();

                  

                    
                    StartTransaction(cn);
                    rowsAffected = cmd.ExecuteNonQuery();
                    //cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                    CommitTransaction(cmd.Transaction);

                }
                catch (Exception ex)
                {
                    RollBackTransaction(cmd.Transaction);
                    throw ex;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
        }


        public bool Testunicite(string Centre, string Code, int Numero)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectTaByKey.Trim();
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@CODE", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.Add("@NUM", SqlDbType.SmallInt).Value = Numero;



                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Result = true;
                }
                reader.Close();
                CommitTransaction(cmd.Transaction);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
            return Result;
        }

        public CsTa SELECT_Exploitation()
        {
            bool ErreurSql = false;
            CsTa _ta = new CsTa();
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTaExploitation;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    //throw new Exception("Aucune colonne trouvée pour cette table.");
                }
                while (reader.Read())
                {
                   // _ta.CODE = reader.GetValue(0).ToString().Trim();
                    _ta.LIBELLE = reader.GetValue(1).ToString().Trim();
                }
                //Fermeture reader
                reader.Close();
                return _ta;
            }
            catch (SqlException)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectTaCodeLibelleByNum + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }


        //public bool TestuniciteRowid(Byte[] RowId)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand();
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = Galate.Datacess.EnumProcedureStockee.SelectTaByRowid.Trim();
               
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = RowId;



        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            Result = true;
        //        }
        //        reader.Close();
        //        CommitTransaction(cmd.Transaction);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //    return Result;
        //}


        private void StartTransaction(SqlConnection _conn)
        {
            if ((_Transaction) && (_conn != null))
            {
                cmd.Transaction = this.BeginTransaction(_conn);
            }
        }

        private void CommitTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.Commit(_pSqlTransaction);
            }
        }

        private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.RollBack(_pSqlTransaction);
            }

        }
    }
}
