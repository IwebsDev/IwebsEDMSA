using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_PUISSANCE /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        public DB_PUISSANCE()
        {
           try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        public DB_PUISSANCE(string ConnStr)
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

        #region Méthodes de mise à jour de la table PUISSANCE

        public DataSet SelectAll_PUISSANCE()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.SelectPUISSANCE;
           
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
                throw new Exception(EnumProcedureStockee.SelectPUISSANCE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                    cmd.Dispose();
            }
        }

        public void Delete_PUISSANCE(string Centre, string Produit, string Commune, string CodeTarif, Decimal Puissance, DateTime DebutApplication)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.DeletePUISSANCE;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = Puissance;
            cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = CodeTarif;
            cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = DebutApplication;
            cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = Commune;

           
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
                throw new Exception(EnumProcedureStockee.DeletePUISSANCE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public void MiseAJour_PUISSANCE(List<CsPuissance> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.UpdatePUISSANCE.Trim();


            try
            {
                foreach (CsPuissance row in rows)
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = row.PUISSANCE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@FRAISDERETARD", SqlDbType.Decimal).Value = row.FRAISDERETARD;
                    cmd.Parameters.Add("@CONSOMMATIONMAXI", SqlDbType.Int).Value = row.CONSOMMATIONMAXI;
                    cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = row.CODETARIF;
                    cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = row.DEBUTAPPLICATION;
                    cmd.Parameters.Add("@FINAPPLICATION", SqlDbType.DateTime).Value = row.FINAPPLICATION;
                    //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = row.COMMUNE;
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
                throw new Exception(EnumProcedureStockee.UpdatePUISSANCE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public void Insertion_PUISSANCE(List<CsPuissance> rows)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = EnumProcedureStockee.InsertPUISSANCE.Trim();

            

            try
            {
                foreach (CsPuissance row in rows)
                {
                    cmd.Parameters.Clear();

                    //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
                    //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
                    //cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = row.PUISSANCE;
                    cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
                    cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
                    cmd.Parameters.Add("@FRAISDERETARD", SqlDbType.Decimal).Value = row.FRAISDERETARD;
                    cmd.Parameters.Add("@CONSOMMATIONMAXI", SqlDbType.Int).Value = row.CONSOMMATIONMAXI;
                    cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = row.CODETARIF;
                    cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = row.DEBUTAPPLICATION;
                    cmd.Parameters.Add("@FINAPPLICATION", SqlDbType.DateTime).Value = row.FINAPPLICATION;
                    //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = row.COMMUNE;

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
                throw new Exception(EnumProcedureStockee.InsertPUISSANCE + ":" + ex.Message);
            }
            finally
            {

                if (cn.State == ConnectionState.Open)
                cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Testunicite_PUISSANCE(string Centre, string Produit, string Commune, string CodeTarif, Decimal Puissance, DateTime DebutApplication)
        {
            bool Result = false;
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumProcedureStockee.SelectPUISSANCEByKey.Trim();
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
                cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
                cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = Puissance;
                cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = CodeTarif;
                cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = DebutApplication;
                cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = Commune;

                

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

        public CsPuissance SelectAll_PUISSANCE()
        {
            try
            {
                return null;
                    //Entities.GetEntityFromQuery<CsPuissance>(DevisProcedures.DEVIS_CONTROLETRAVAUX_RETOURNEByNumDevis(pNumDevis, pOrdre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete_PUISSANCE(string Centre, string Produit, string Commune, string CodeTarif, Decimal Puissance, DateTime DebutApplication)
        {
            //cn = new SqlConnection(ConnectionString);
            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = EnumProcedureStockee.DeletePUISSANCE;
            //cmd.Parameters.Clear();

            //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
            //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = Produit;
            //cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = Puissance;
            //cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = CodeTarif;
            //cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = DebutApplication;
            //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = Commune;


            //try
            //{

            //    if (cn.State == ConnectionState.Closed)
            //        cn.Open();
            //    StartTransaction(cn);
            //    cmd.ExecuteNonQuery();
            //    CommitTransaction(cmd.Transaction);

            //}
            //catch (Exception ex)
            //{
            //    RollBackTransaction(cmd.Transaction);
            //    throw new Exception(EnumProcedureStockee.DeletePUISSANCE + ":" + ex.Message);
            //}
            //finally
            //{
            //    if (cn.State == ConnectionState.Open)
            //        cn.Close();
            //    cmd.Dispose();
            //}
        }

        public void MiseAJour_PUISSANCE(List<CsPuissance> rows)
        {
            //cn = new SqlConnection(ConnectionString);
            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = EnumProcedureStockee.UpdatePUISSANCE.Trim();


            //try
            //{
            //    foreach (CsPuissance row in rows)
            //    {
            //        cmd.Parameters.Clear();

            //        //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
            //        //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
            //        //cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = row.PUISSANCE;
            //        cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
            //        cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
            //        cmd.Parameters.Add("@FRAISDERETARD", SqlDbType.Decimal).Value = row.FRAISDERETARD;
            //        cmd.Parameters.Add("@CONSOMMATIONMAXI", SqlDbType.Int).Value = row.CONSOMMATIONMAXI;
            //        cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = row.CODETARIF;
            //        cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = row.DEBUTAPPLICATION;
            //        cmd.Parameters.Add("@FINAPPLICATION", SqlDbType.DateTime).Value = row.FINAPPLICATION;
            //        //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = row.COMMUNE;
            //        cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = row.ROWID;

            //        DBBase.SetDBNullParametre(cmd.Parameters);

            //        if (cn.State == ConnectionState.Closed)
            //            cn.Open();
            //        StartTransaction(cn);

            //        cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
            //    }

            //    CommitTransaction(cmd.Transaction);

            //}

            //catch (Exception ex)
            //{
            //    RollBackTransaction(cmd.Transaction);
            //    throw new Exception(EnumProcedureStockee.UpdatePUISSANCE + ":" + ex.Message);
            //}
            //finally
            //{
            //    if (cn.State == ConnectionState.Open)
            //        cn.Close(); // Fermeture de la connection 
            //    cmd.Dispose();
            //}
        }

        public void Insertion_PUISSANCE(List<CsPuissance> rows)
        {
            //cn = new SqlConnection(ConnectionString);
            //cmd = new SqlCommand();
            //cmd.Connection = cn;
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = EnumProcedureStockee.InsertPUISSANCE.Trim();



            //try
            //{
            //    foreach (CsPuissance row in rows)
            //    {
            //        cmd.Parameters.Clear();

            //        //cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
            //        //cmd.Parameters.Add("@PRODUIT", SqlDbType.VarChar).Value = row.PRODUIT;
            //        //cmd.Parameters.Add("@PUISSANCE", SqlDbType.Decimal).Value = row.PUISSANCE;
            //        cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
            //        cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
            //        cmd.Parameters.Add("@FRAISDERETARD", SqlDbType.Decimal).Value = row.FRAISDERETARD;
            //        cmd.Parameters.Add("@CONSOMMATIONMAXI", SqlDbType.Int).Value = row.CONSOMMATIONMAXI;
            //        cmd.Parameters.Add("@CODETARIF", SqlDbType.VarChar).Value = row.CODETARIF;
            //        cmd.Parameters.Add("@DEBUTAPPLICATION", SqlDbType.DateTime).Value = row.DEBUTAPPLICATION;
            //        cmd.Parameters.Add("@FINAPPLICATION", SqlDbType.DateTime).Value = row.FINAPPLICATION;
            //        //cmd.Parameters.Add("@COMMUNE", SqlDbType.VarChar).Value = row.COMMUNE;

            //        DBBase.SetDBNullParametre(cmd.Parameters);

            //        if (cn.State == ConnectionState.Closed)
            //            cn.Open();
            //        StartTransaction(cn);
            //        cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
            //    }

            //    CommitTransaction(cmd.Transaction);

            //}
            //catch (Exception ex)
            //{
            //    RollBackTransaction(cmd.Transaction);
            //    throw new Exception(EnumProcedureStockee.InsertPUISSANCE + ":" + ex.Message);
            //}
            //finally
            //{

            //    if (cn.State == ConnectionState.Open)
            //        cn.Close(); // Fermeture de la connection 
            //    cmd.Dispose();
            //}
        }

        public List<CsPuissance> GetPuissanceParProduit(string pProduit)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsPuissance>(DevisProcedures.DEVIS_PUISSANCE_GetByProduit(pProduit));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsPuissance> GetPuissanceParProduitId(int pProduitId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsPuissance>(DevisProcedures.DEVIS_PUISSANCE_GetByProduitId(pProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
