using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DB_TCOMPT : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        public DB_TCOMPT()
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
        public DB_TCOMPT(string ConnStr)
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

        public List<CsTcompt> SelectAllTcompt()
        {
            cn = new SqlConnection(ConnectionString);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_TCOMPT_RETOURNE"
                };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsTcompt>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception("SPX_PARAM_Tcompt_RETOURNE" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsTcompt pTcompt)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "SPX_PARAM_TCOMPT_SUPPRIMER"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CENTRE", pTcompt.CENTRE);
                cmd.Parameters.AddWithValue("@PRODUIT", pTcompt.PRODUIT);
                cmd.Parameters.AddWithValue("@TYPE", pTcompt.TYPE);
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
            }
            catch (Exception ex)
            {
                RollBackTransaction(cmd.Transaction);
                throw new Exception("SPX_PARAM_Tcompt_SUPPRIMER" + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsTcompt> pCategorieBranchemenCollection)
        {
            int number = 0;
            foreach (CsTcompt entity in pCategorieBranchemenCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsTcompt> Fill(IDataReader reader, List<CsTcompt> rows, int start, int pageLength)
        {
            // advance to the starting row
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows; // not enough rows, just return
            }

            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break; // we are done

                var c = new CsTcompt();
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.OriginalCENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.PRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? string.Empty : (System.String)reader["PRODUIT"];
                c.OriginalPRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? string.Empty : (System.String)reader["PRODUIT"];
                c.TYPE = (Convert.IsDBNull(reader["TYPE"])) ? string.Empty : (System.String)reader["TYPE"];
                c.OriginalTYPE = (Convert.IsDBNull(reader["TYPE"])) ? string.Empty : (System.String)reader["TYPE"];
                c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? null : (System.String)reader["LIBELLE"];
                c.TRANS = (Convert.IsDBNull(reader["TRANS"])) ? null : (System.String)reader["TRANS"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
                if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
                    c.DATEMODIFICATION = null;
                else
                    c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
                rows.Add(c);
            }
            return rows;
        }

        public bool Update(CsTcompt pTcompt)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TCOMPT_UPDATE"
            };
            cmd.Parameters.Clear();

            try
            {
                 cmd.Parameters.AddWithValue("@CENTRE", pTcompt.CENTRE);
                 cmd.Parameters.AddWithValue("@OriginalCENTRE", pTcompt.OriginalCENTRE);
                 cmd.Parameters.AddWithValue("@PRODUIT", pTcompt.PRODUIT);
                 cmd.Parameters.AddWithValue("@OriginalPRODUIT", pTcompt.OriginalPRODUIT);
                 cmd.Parameters.AddWithValue("@TYPE", pTcompt.TYPE);
                 cmd.Parameters.AddWithValue("@OriginalTYPE", pTcompt.OriginalTYPE);
                 cmd.Parameters.AddWithValue("@LIBELLE", pTcompt.LIBELLE);
                 cmd.Parameters.AddWithValue("@TRANS", pTcompt.TRANS);
                 cmd.Parameters.AddWithValue("@DATECREATION", pTcompt.DATECREATION);
                 cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTcompt.DATEMODIFICATION);
                 cmd.Parameters.AddWithValue("@USERCREATION", pTcompt.USERCREATION);
                 cmd.Parameters.AddWithValue("@USERMODIFICATION", pTcompt.USERMODIFICATION);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);
                SetDBNullParametre(cmd.Parameters);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
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

        public bool Update(List<CsTcompt> pTcomptCollection)
        {
            int number = 0;
            foreach (CsTcompt entity in pTcomptCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsTcompt pTcompt)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = "SPX_PARAM_TCOMPT_INSERER"
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@CENTRE", pTcompt.CENTRE);
                cmd.Parameters.AddWithValue("@PRODUIT", pTcompt.PRODUIT);
                cmd.Parameters.AddWithValue("@TYPE", pTcompt.TYPE);
                cmd.Parameters.AddWithValue("@LIBELLE", pTcompt.LIBELLE);
                cmd.Parameters.AddWithValue("@TRANS", pTcompt.TRANS);
                cmd.Parameters.AddWithValue("@DATECREATION", pTcompt.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pTcompt.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pTcompt.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pTcompt.USERMODIFICATION);

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                StartTransaction(cn);

                SetDBNullParametre(cmd.Parameters);
                int rowsAffected = cmd.ExecuteNonQuery();
                CommitTransaction(cmd.Transaction);
                return Convert.ToBoolean(rowsAffected);
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

        public bool Insert(List<CsTcompt> pTcomptCollection)
        {
            int number = 0;
            foreach (CsTcompt entity in pTcomptCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        #region Méthodes de mise à jour de la table TCOMPT

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

        public List<CsTcompteur > SelectAllTcompt()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsTcompteur>(ParamProcedure.PARAM_TCOMPT_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsTcompteur pTcompt)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsTcompteur> pTcomptCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TYPECOMPTEUR >(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsTcompteur pTcompt)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsTcompteur> pTcomptCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TYPECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsTcompteur  pTcompt)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcompt));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsTcompteur> pTcomptCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TYPECOMPTEUR>(Entities.ConvertObject<Galatee.Entity.Model.TYPECOMPTEUR, CsTcompteur>(pTcomptCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
