using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Galatee.Structure;

namespace Galatee.DataAccess
{
    public class DB_DEMCOUT : Galatee.DataAccess.Parametrage.DbBase
    {
        /// <summary>
        /// DB_DEMCOUT
        /// </summary>
        public DB_DEMCOUT()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_DEMCOUT
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_DEMCOUT(string ConnStr)
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

        #region Méthodes de mise à jour de la table DEMCOUT
        /// <summary>
        /// SelectTDEM_LIBELLE_TDEM
        /// </summary>
        /// <returns></returns>
        /// 
        public List<CsLibelle> SelectCoperLibelle100()
        {
            List<CsLibelle> COPER = new List<CsLibelle>();
            CsLibelle typedem;
            CsLibelle ObjtypedemVide = new CsLibelle();

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectCOPER_LIBELLE100;

            try
            {


                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SqlDataReader reader = cmd.ExecuteReader();
                CommitTransaction(cmd.Transaction);

                while (reader.Read())
                {
                    typedem = new CsLibelle();
                    typedem.CODE = reader.GetValue(0).ToString().Trim();
                    typedem.LIBELLE = reader.GetValue(1).ToString().Trim();
                    COPER.Add(typedem);
                }
                //Fermeture reader
                reader.Close();

                return COPER;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectTDEMLIBELLE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsTdemCout> SelectTDEM_LIBELLE_TDEM()
        {
            List<CsTdemCout> typedemande = new List<CsTdemCout>();
            CsTdemCout typedem;
            CsTdemCout ObjtypedemVide = new CsTdemCout();

            ObjtypedemVide.TDEM = string.Empty;
            ObjtypedemVide.LIBELLE = string.Empty;
            typedemande.Add(ObjtypedemVide);

            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTDEMLIBELLE;

            try
            {


                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SqlDataReader reader = cmd.ExecuteReader();
                CommitTransaction(cmd.Transaction);

                while (reader.Read())
                {
                    typedem = new CsTdemCout();
                    typedem.TDEM = reader.GetValue(0).ToString().Trim();
                    typedem.LIBELLE = reader.GetValue(1).ToString().Trim();
                    typedemande.Add(typedem);
                }
                //Fermeture reader
                reader.Close();

                return typedemande;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.SelectTDEMLIBELLE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// SelectAll_DEMCOUT
        /// </summary>
        /// <returns></returns>
        public DataSet SelectAll_DEMCOUT()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectDEMCOUT;

            try
            {
                StartTransaction(cn);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();

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
                throw new Exception(EnumProcedureStockee.SelectDEMCOUT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Delete_DEMCOUT
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Produit"></param>
        /// <param name="Tdem"></param>
        public bool Delete_DEMCOUT(string Centre,string Produit, string Tdem)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteDEMCOUT;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            cmd.Parameters.Add("@TDEM", SqlDbType.VarChar).Value = Tdem;

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                StartTransaction(cn);

                int status =0;
                status = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return status > 0;
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                return false;
                throw new Exception(EnumProcedureStockee.DeleteDEMCOUT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        /// MiseAJour_DEMCOUT
        /// </summary>
        /// <param name="row"></param>
        public void MiseAJour_DEMCOUT(List<CsTdemCout> rows)
        {
            foreach (CsTdemCout row in rows)
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.UpdateDEMCOUT.Trim();

                cmd.Parameters.Clear();

                try
                {


                    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    cmd.Parameters.Add("@TDEM", SqlDbType.VarChar).Value = row.TDEM;
                    cmd.Parameters.Add("@COPER1", SqlDbType.VarChar).Value = row.COPER1;
                    cmd.Parameters.Add("@OBLI1", SqlDbType.VarChar).Value = row.OBLI1;
                    cmd.Parameters.Add("@AUTO1", SqlDbType.VarChar).Value = row.AUTO1;
                    cmd.Parameters.Add("@MONTANT1", SqlDbType.Decimal).Value = row.MONTANT1;
                    cmd.Parameters.Add("@TAXE1", SqlDbType.VarChar).Value = row.TAXE1;
                    cmd.Parameters.Add("@COPER2", SqlDbType.VarChar).Value = row.COPER2;
                    cmd.Parameters.Add("@OBLI2", SqlDbType.VarChar).Value = row.OBLI2;
                    cmd.Parameters.Add("@AUTO2", SqlDbType.VarChar).Value = row.AUTO2;
                    cmd.Parameters.Add("@MONTANT2", SqlDbType.Decimal).Value = row.MONTANT2;
                    cmd.Parameters.Add("@TAXE2", SqlDbType.VarChar).Value = row.TAXE2;
                    cmd.Parameters.Add("@COPER3", SqlDbType.VarChar).Value = row.COPER3;
                    cmd.Parameters.Add("@OBLI3", SqlDbType.VarChar).Value = row.OBLI3;
                    cmd.Parameters.Add("@AUTO3", SqlDbType.VarChar).Value = row.AUTO3;
                    cmd.Parameters.Add("@MONTANT3", SqlDbType.Decimal).Value = row.MONTANT3;
                    cmd.Parameters.Add("@TAXE3", SqlDbType.VarChar).Value = row.TAXE3;
                    cmd.Parameters.Add("@COPER4", SqlDbType.VarChar).Value = row.COPER4;
                    cmd.Parameters.Add("@OBLI4", SqlDbType.VarChar).Value = row.OBLI4;
                    cmd.Parameters.Add("@AUTO4", SqlDbType.VarChar).Value = row.AUTO4;
                    cmd.Parameters.Add("@MONTANT4", SqlDbType.Decimal).Value = row.MONTANT4;
                    cmd.Parameters.Add("@TAXE4", SqlDbType.VarChar).Value = row.TAXE4;
                    cmd.Parameters.Add("@COPER5", SqlDbType.VarChar).Value = row.COPER5;
                    cmd.Parameters.Add("@OBLI5", SqlDbType.VarChar).Value = row.OBLI5;
                    cmd.Parameters.Add("@AUTO5", SqlDbType.VarChar).Value = row.AUTO5;
                    cmd.Parameters.Add("@MONTANT5", SqlDbType.Decimal).Value = row.MONTANT5;
                    cmd.Parameters.Add("@TAXE5", SqlDbType.VarChar).Value = row.TAXE5;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = row.ROWID;


                    DBBase.SetDBNullParametre(cmd.Parameters);

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    cmd.ExecuteNonQuery();

                    CommitTransaction(cmd.Transaction);
                }

                catch (Exception ex)
                {
                    RollBackTransaction(cmd.Transaction);
                    throw new Exception(EnumProcedureStockee.UpdateDEMCOUT + ":" + ex.Message);
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
                }
            }
        }
        /// <summary>
        /// Insertion_DEMCOUT
        /// </summary>
        /// <param name="row"></param>
        public void Insertion_DEMCOUT(List<CsTdemCout> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertDEMCOUT.Trim();
            cmd.Parameters.Clear();

            
           try
            {
                foreach (CsTdemCout row in rows)
                {

                 cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    cmd.Parameters.Add("@TDEM", SqlDbType.VarChar).Value = row.TDEM;
                    cmd.Parameters.Add("@COPER1", SqlDbType.VarChar).Value = row.COPER1;
                    cmd.Parameters.Add("@OBLI1", SqlDbType.VarChar).Value = row.OBLI1;
                    cmd.Parameters.Add("@AUTO1", SqlDbType.VarChar).Value = row.AUTO1;
                    cmd.Parameters.Add("@MONTANT1", SqlDbType.Decimal).Value = row.MONTANT1;
                    cmd.Parameters.Add("@TAXE1", SqlDbType.VarChar).Value = row.TAXE1;
                    cmd.Parameters.Add("@COPER2", SqlDbType.VarChar).Value = row.COPER2;
                    cmd.Parameters.Add("@OBLI2", SqlDbType.VarChar).Value = row.OBLI2;
                    cmd.Parameters.Add("@AUTO2", SqlDbType.VarChar).Value = row.AUTO2;
                    cmd.Parameters.Add("@MONTANT2", SqlDbType.Decimal).Value = row.MONTANT2;
                    cmd.Parameters.Add("@TAXE2", SqlDbType.VarChar).Value = row.TAXE2;
                    cmd.Parameters.Add("@COPER3", SqlDbType.VarChar).Value = row.COPER3;
                    cmd.Parameters.Add("@OBLI3", SqlDbType.VarChar).Value = row.OBLI3;
                    cmd.Parameters.Add("@AUTO3", SqlDbType.VarChar).Value = row.AUTO3;
                    cmd.Parameters.Add("@MONTANT3", SqlDbType.Decimal).Value = row.MONTANT3;
                    cmd.Parameters.Add("@TAXE3", SqlDbType.VarChar).Value = row.TAXE3;
                    cmd.Parameters.Add("@COPER4", SqlDbType.VarChar).Value = row.COPER4;
                    cmd.Parameters.Add("@OBLI4", SqlDbType.VarChar).Value = row.OBLI4;
                    cmd.Parameters.Add("@AUTO4", SqlDbType.VarChar).Value = row.AUTO4;
                    cmd.Parameters.Add("@MONTANT4", SqlDbType.Decimal).Value = row.MONTANT4;
                    cmd.Parameters.Add("@TAXE4", SqlDbType.VarChar).Value = row.TAXE4;
                    cmd.Parameters.Add("@COPER5", SqlDbType.VarChar).Value = row.COPER5;
                    cmd.Parameters.Add("@OBLI5", SqlDbType.VarChar).Value = row.OBLI5;
                    cmd.Parameters.Add("@AUTO5", SqlDbType.VarChar).Value = row.AUTO5;
                    cmd.Parameters.Add("@MONTANT5", SqlDbType.Decimal).Value = row.MONTANT5;
                    cmd.Parameters.Add("@TAXE5", SqlDbType.VarChar).Value = row.TAXE5;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    DBBase.SetDBNullParametre(cmd.Parameters);

                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    cmd.ExecuteNonQuery();
                }
                CommitTransaction(cmd.Transaction);
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.InsertDEMCOUT + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        /// <summary>
        /// Testunicite_DEMCOUT
        /// </summary>
        /// <param name="Centre"></param>
        /// <param name="Produit"></param>
        /// <param name="Tdem"></param>
        /// <returns></returns>
        public bool Testunicite_DEMCOUT(string Centre, string Produit, string Tdem)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectDEMCOUTByKey.Trim();
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
                cmd.Parameters.Add("@TDEM", SqlDbType.VarChar).Value = Tdem;
                DBBase.SetDBNullParametre(cmd.Parameters);
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
                RollBackTransaction(cmd.Transaction);
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
        /// <summary>
        /// StartTransaction
        /// </summary>
        /// <param name="_conn"></param>
        private void StartTransaction(SqlConnection _conn)
        {
            if ((_Transaction) && (_conn != null))
            {
                cmd.Transaction = this.BeginTransaction(_conn);
            }
        }
        /// <summary>
        /// CommitTransaction
        /// </summary>
        /// <param name="_pSqlTransaction"></param>
        private void CommitTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.Commit(_pSqlTransaction);
            }
        }
        /// <summary>
        /// RollBackTransaction
        /// </summary>
        /// <param name="_pSqlTransaction"></param>
        private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        {
            if ((_Transaction) && (_pSqlTransaction != null))
            {
                this.RollBack(_pSqlTransaction);
            }

        }
        #endregion
        
    }
}
