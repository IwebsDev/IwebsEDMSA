using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    /// <summary>
    /// DB_BANQUES2
    /// </summary>
    public class DB_PARAMABONUTILISE /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        /*
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        public DB_PARAMABONUTILISE()
        {
            ConnectionString = Session.GetSqlConnexionString();
        }
        /// <summary>
        /// DB_BANQUES2
        /// </summary>
        /// <param name="ConnStr"></param>
        public DB_PARAMABONUTILISE(string ConnStr)
        {
            ConnectionString = ConnStr;
        }
        /// <summary>
        /// ConnectionString
        /// </summary>
        private string ConnectionString;
        private SqlConnection cn = null;

        private bool _Transaction;
        /// <summary>
        /// Transaction
        /// </summary>
        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;


        public List<CsParamAbonUtilise> SelectAllParamAbonUtilise()
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
                    CommandText = EnumProcedureStockee.SelectPARAMABONUTILISE
                };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsParamAbonUtilise>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectPARAMABONUTILISE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool Delete(CsParamAbonUtilise pParamAbonUtilise)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeletePARAMABONUTILISE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CENTRE", pParamAbonUtilise.CENTRE);
                cmd.Parameters.AddWithValue("@CLECAL", pParamAbonUtilise.CLECAL);
                cmd.Parameters.AddWithValue("@PRODUIT", pParamAbonUtilise.PRODUIT);
                cmd.Parameters.AddWithValue("@PARAM", pParamAbonUtilise.PARAM);
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
                throw new Exception(EnumProcedureStockee.DeletePARAMABONUTILISE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            int number = 0;
            foreach (CsParamAbonUtilise entity in pParamAbonUtiliseCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Update(CsParamAbonUtilise pParamAbonUtilise)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.UpdatePARAMABONUTILISE
            };
            cmd.Parameters.Clear();

            try
            {

                cmd.Parameters.AddWithValue("@CENTRE",pParamAbonUtilise.CENTRE);
                cmd.Parameters.AddWithValue("@OriginalCENTRE",pParamAbonUtilise.OriginalCENTRE);
                cmd.Parameters.AddWithValue("@CLECAL",pParamAbonUtilise.CLECAL);
                cmd.Parameters.AddWithValue("@OriginalCLECAL",pParamAbonUtilise.OriginalCLECAL);
                cmd.Parameters.AddWithValue("@PRODUIT",pParamAbonUtilise.PRODUIT);
                cmd.Parameters.AddWithValue("@OriginalPRODUIT",pParamAbonUtilise.OriginalPRODUIT);
                cmd.Parameters.AddWithValue("@PARAM",pParamAbonUtilise.PARAM);
                cmd.Parameters.AddWithValue("@OriginalPARAM",pParamAbonUtilise.OriginalPARAM);
                cmd.Parameters.AddWithValue("@CODE",pParamAbonUtilise.CODE);
                cmd.Parameters.AddWithValue("@VALDEF",pParamAbonUtilise.VALDEF);
                cmd.Parameters.AddWithValue("@STATUT",pParamAbonUtilise.STATUT);
                cmd.Parameters.AddWithValue("@DEBUTAPPLICATION", pParamAbonUtilise.DEBUTAPPLICATION);
                cmd.Parameters.AddWithValue("@FINAPPLICATION", pParamAbonUtilise.FINAPPLICATION);
                cmd.Parameters.AddWithValue("@DATECREATION", pParamAbonUtilise.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pParamAbonUtilise.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION",pParamAbonUtilise.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION",pParamAbonUtilise.USERMODIFICATION);

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

        public bool Update(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            int number = 0;
            foreach (CsParamAbonUtilise entity in pParamAbonUtiliseCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        private bool Insert(CsParamAbonUtilise pParamAbonUtilise)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand
            {
                Connection = cn,
                CommandType = CommandType.StoredProcedure,
                CommandText = EnumProcedureStockee.InsertPARAMABONUTILISE
            };
            cmd.Parameters.Clear();

            try
            {
                cmd.Parameters.AddWithValue("@CENTRE", pParamAbonUtilise.CENTRE);
                cmd.Parameters.AddWithValue("@CLECAL", pParamAbonUtilise.CLECAL);
                cmd.Parameters.AddWithValue("@PRODUIT", pParamAbonUtilise.PRODUIT);
                cmd.Parameters.AddWithValue("@PARAM", pParamAbonUtilise.PARAM);
                cmd.Parameters.AddWithValue("@CODE", pParamAbonUtilise.CODE);
                cmd.Parameters.AddWithValue("@VALDEF", pParamAbonUtilise.VALDEF);
                cmd.Parameters.AddWithValue("@STATUT", pParamAbonUtilise.STATUT);
                cmd.Parameters.AddWithValue("@DEBUTAPPLICATION", pParamAbonUtilise.DEBUTAPPLICATION);
                cmd.Parameters.AddWithValue("@FINAPPLICATION", pParamAbonUtilise.FINAPPLICATION);
                cmd.Parameters.AddWithValue("@DATECREATION", pParamAbonUtilise.DATECREATION);
                cmd.Parameters.AddWithValue("@DATEMODIFICATION", pParamAbonUtilise.DATEMODIFICATION);
                cmd.Parameters.AddWithValue("@USERCREATION", pParamAbonUtilise.USERCREATION);
                cmd.Parameters.AddWithValue("@USERMODIFICATION", pParamAbonUtilise.USERMODIFICATION);

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

        public bool Insert(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            int number = 0;
            foreach (CsParamAbonUtilise entity in pParamAbonUtiliseCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsParamAbonUtilise> Fill(IDataReader reader, List<CsParamAbonUtilise> rows, int start, int pageLength)
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

                var c = new CsParamAbonUtilise();
                c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.OriginalCENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
                c.CLECAL = (Convert.IsDBNull(reader["CLECAL"])) ? string.Empty : (System.String)reader["CLECAL"];
                c.OriginalCLECAL = (Convert.IsDBNull(reader["CLECAL"])) ? string.Empty : (System.String)reader["CLECAL"];
                c.PRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? string.Empty : (System.String)reader["PRODUIT"];
                c.OriginalPRODUIT = (Convert.IsDBNull(reader["PRODUIT"])) ? string.Empty : (System.String)reader["PRODUIT"];
                c.PARAM = (Convert.IsDBNull(reader["PARAM"])) ? string.Empty : (System.String)reader["PARAM"];
                c.OriginalPARAM = (Convert.IsDBNull(reader["PARAM"])) ? string.Empty : (System.String)reader["PARAM"];
                c.CODE = (Convert.IsDBNull(reader["CODE"])) ? null : (System.String)reader["CODE"];
                c.VALDEF = (Convert.IsDBNull(reader["VALDEF"])) ? null : (System.String)reader["VALDEF"];
                c.STATUT = (Convert.IsDBNull(reader["STATUT"])) ? null : (System.String)reader["STATUT"];
                if (Convert.IsDBNull(reader["DEBUTAPPLICATION"]))
                    c.DEBUTAPPLICATION = null;
                else
                    c.DEBUTAPPLICATION = (System.DateTime)reader["DEBUTAPPLICATION"];
                if (Convert.IsDBNull(reader["FINAPPLICATION"]))
                    c.DEBUTAPPLICATION = null;
                else
                    c.FINAPPLICATION = (System.DateTime)reader["FINAPPLICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
                if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
                    c.DATECREATION = null;
                else
                    c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
                rows.Add(c);
            }
            return rows;
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
        */

        public List<CsParamAbonUtilise> SelectAllParamAbonUtilise()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsParamAbonUtilise>(ParamProcedure.PARAM_PARAMABONUTILISE_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsParamAbonUtilise pParamAbonUtilise)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtilise));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtiliseCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Update(CsParamAbonUtilise pParamAbonUtilise)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtilise));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtiliseCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Insert(CsParamAbonUtilise pParamAbonUtilise)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtilise));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsParamAbonUtilise> pParamAbonUtiliseCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.PARAMABONUTILISE>(Entities.ConvertObject<Galatee.Entity.Model.PARAMABONUTILISE, CsParamAbonUtilise>(pParamAbonUtiliseCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
