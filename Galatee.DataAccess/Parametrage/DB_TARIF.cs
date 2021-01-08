using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_TARIF /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        public DB_TARIF()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        public DB_TARIF(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        private string ConnectionString;// = string.Empty;
        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;
        #region Méthodes de mise à jour de la table TARIF

        public DataSet SelectAll_TARIF()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectTARIF;
            
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
                throw new Exception(EnumProcedureStockee.SelectTARIF + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
            }
        }

        public void Delete_TARIF(string Centre, string Produit, string Tarif)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeleteTARIF;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            cmd.Parameters.Add("@TARIF", SqlDbType.VarChar).Value = Tarif;

           
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
                throw new Exception(EnumProcedureStockee.DeleteTARIF + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public void MiseAJour_TARIF(List<CsTarif> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdateTARIF.Trim();

            try
            {
                foreach (CsTarif row in rows)
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@TARIF", SqlDbType.VarChar).Value = row.TARIF;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = row.ROWID;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                }
                CommitTransaction(cmd.Transaction);
               
            }
            
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.UpdateTARIF + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public void Insertion_TARIF(List<CsTarif> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertTARIF.Trim();

           
            cmd.Parameters.Clear();

            try
            {
                foreach (CsTarif row in rows)
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@TARIF", SqlDbType.VarChar).Value = row.TARIF;
                    cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    StartTransaction(cn);

                    cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
                }
                CommitTransaction(cmd.Transaction);
               
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception(EnumProcedureStockee.InsertTARIF + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Testunicite_TARIF(string Centre, string Produit, string Tarif)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectTARIFByKey.Trim();
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
                cmd.Parameters.Add("@TARIF", SqlDbType.VarChar).Value = Tarif;



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
        */
        //public bool Testunicite_TARIFByRowId(Byte[] RowId)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand();
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = Galate.Datacess.EnumProcedureStockee.SelectTARIFByRowId.Trim();
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
        //        RollBackTransaction(cmd.Transaction);
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
        /*
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

        #endregion
         */

        public List<CsTarif> SelectAllTarif()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTarif>(ParamProcedure.PARAM_TARIF_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsTarif> SelectTarifByTarifCentreProduit(CsTarif pTarif)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTarif>(ParamProcedure.PARAM_TARIFRETOURNEById(pTarif.PK_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTarif pTarifs)
        {
            try
            {
                return Entities.UpdateEntity <Galatee.Entity.Model.TYPETARIF>(Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifs));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTarif> pTarifsCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPETARIF>(Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTarif pTarifs)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPETARIF>(Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifs));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTarif> pTarifsCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPETARIF>(Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifsCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTarif pTarifs)
        {
            try
            {
                TYPETARIF letar = Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifs);
                return Entities.InsertEntity<Galatee.Entity.Model.TYPETARIF>(letar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTarif> pTarifsCollection)
        {
            try
            {
                var letar = Entities.ConvertObject<Galatee.Entity.Model.TYPETARIF, CsTarif>(pTarifsCollection);
                return Entities.InsertEntity<Galatee.Entity.Model.TYPETARIF>(letar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
         
}
