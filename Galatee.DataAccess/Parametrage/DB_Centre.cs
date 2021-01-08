using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using Inova.Tools.Utilities;
using Galatee.Structure;
using System.Globalization;
using Galatee.Entity.Model;
using System.Threading.Tasks;


namespace Galatee.DataAccess
{
    public class DB_Centre : Galatee.DataAccess.Parametrage.DbBase
    {
        /*
        private string ConnectionString;

        public DB_Centre()
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

        public DB_Centre(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        private SqlConnection cn = null;

        private bool _Transaction;

        public bool Transaction
        {
            get { return _Transaction; }
            set { _Transaction = value; }

        }

        private SqlCommand cmd = null;

        public List<CsCentre> SelectAllCentre()
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
                              CommandText = EnumProcedureStockee.SelectCENTRE
                          };
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCentre>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(EnumProcedureStockee.SelectCENTRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public List<CsCentre> SelectCentreByCodeSite(string pCodeSite)
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
                    CommandText = "SPX_PARAM_CENTRE_RETOURNEByCODESITE"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CODESITE", pCodeSite);
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCentre>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw  ex;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public CsCentre SelectCentreByCodeSiteCodeCentre(string pCodeSite, string pCodeCentre)
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
                    CommandText = "SPX_PARAM_CENTRE_RETOURNEByCodeCentreCODESITE"
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CodeCentre", pCodeCentre);
                cmd.Parameters.AddWithValue("@CODESITE", pCodeSite);
                IDataReader reader = cmd.ExecuteReader();
                var rows = new List<CsCentre>();
                Fill(reader, rows, int.MinValue, int.MaxValue);
                reader.Close();
                return rows.Count > 0 ? rows[0] : null;
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
        }

        public bool Delete(CsCentre pCentre)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.DeleteCENTRE
                };
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CodeCentre", pCentre.PK_CodeCentre);
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
                throw new Exception(EnumProcedureStockee.DeleteCENTRE + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                cn.Close();
                cmd.Dispose();
            }
        }

        public bool Delete(List<CsCentre> pCentreCollection)
        {
            int number = 0;
            foreach (CsCentre entity in pCentreCollection)
            {
                if (Delete(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public static List<CsCentre> Fill(IDataReader reader, List<CsCentre> rows, int start, int pageLength)
		{
			// advance to the starting row
			for (int i = 0; i < start; i++)
			{
				if (! reader.Read() )
					return rows; // not enough rows, just return
			}

			for (int i = 0; i < pageLength; i++)
			{
				if (!reader.Read())
					break; // we are done

                var c = new CsCentre();
                c.PK_CodeCentre = (Convert.IsDBNull(reader["CodeCentre"])) ? string.Empty : (System.String)reader["CodeCentre"];
                c.OriginalCodeCentre = (Convert.IsDBNull(reader["CodeCentre"])) ? string.Empty : (System.String)reader["CodeCentre"];
                c.Libelle = (Convert.IsDBNull(reader["LIBELLE"])) ? string.Empty : (System.String)reader["LIBELLE"];
                c.CodeType = (Convert.IsDBNull(reader["CodeType"])) ? string.Empty : (System.String)reader["CodeType"];
                c.CodeSite = (Convert.IsDBNull(reader["CodeSite"])) ? string.Empty : (System.String)reader["CodeSite"];
                c.LIBELLESITE = (Convert.IsDBNull(reader["LIBELLESITE"])) ? string.Empty : (System.String)reader["LIBELLESITE"];
                c.LIBELLETYPECENTRE = (Convert.IsDBNull(reader["LIBELLETYPECENTRE"])) ? string.Empty : (System.String)reader["LIBELLETYPECENTRE"];
                c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
                c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? string.Empty : (System.String)reader["USERMODIFICATION"];
                c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? (DateTime?)null : (System.DateTime)reader["DATECREATION"];
                c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? (DateTime?)null : (System.DateTime)reader["DATEMODIFICATION"];
				rows.Add(c);
			}
			return rows;
		}

        public bool Update(CsCentre pCentre)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                          {
                              Connection = cn,
                              CommandType = CommandType.StoredProcedure,
                              CommandText = EnumProcedureStockee.UpdateCENTRE
                          };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CodeCentre", pCentre.PK_CodeCentre);
                    cmd.Parameters.AddWithValue("@OriginalCodeCentre", pCentre.OriginalCodeCentre);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCentre.Libelle);
                    cmd.Parameters.AddWithValue("@CodeType", pCentre.CodeType);
                    cmd.Parameters.AddWithValue("@CodeSite", pCentre.CodeSite);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCentre.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCentre.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCentre.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCentre.USERMODIFICATION);
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

        public bool Update(List<CsCentre> pCsCentreCollection)
        {
            int number = 0;
            foreach (CsCentre entity in pCsCentreCollection)
            {
                if (Update(entity))
                {
                    number++;
                }
            }
            return number != 0;
        }

        public bool Insert(CsCentre pCentre)
        {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = EnumProcedureStockee.InsertCENTRE
                };
                cmd.Parameters.Clear();

                try
                {
                    cmd.Parameters.AddWithValue("@CodeCentre", pCentre.PK_CodeCentre);
                    cmd.Parameters.AddWithValue("@CodeSite", pCentre.CodeSite);
                    cmd.Parameters.AddWithValue("@CODETYPE", pCentre.CodeType);
                    cmd.Parameters.AddWithValue("@LIBELLE", pCentre.Libelle);
                    cmd.Parameters.AddWithValue("@DATECREATION", pCentre.DATECREATION);
                    cmd.Parameters.AddWithValue("@DATEMODIFICATION", pCentre.DATEMODIFICATION);
                    cmd.Parameters.AddWithValue("@USERCREATION", pCentre.USERCREATION);
                    cmd.Parameters.AddWithValue("@USERMODIFICATION", pCentre.USERMODIFICATION);
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

        public bool Insert(List<CsCentre> pCentreCollection)
        {
            int number = 0;
            foreach (CsCentre entity in pCentreCollection)
            {
                if (Insert(entity))
                {
                    number++;
                }
            }
            return number != 0;
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

        */

         public List<CsCentre> SelectAllCentre()
        {
            try
            {
                List<CsCentre> lstCentre = Entities.GetEntityListFromQuery<CsCentre>(ParamProcedure.PARAM_CENTRE_RETOURNE());
                List<CsProduit> lstProduitCentre = Entities.GetEntityListFromQuery<CsProduit>(ParamProcedure.PARAM_PRODUIT_CENTRE());
                foreach (CsCentre item in lstCentre)             
                    item.LESPRODUITSDUSITE = lstProduitCentre.Where(t => t.FK_IDCENTRE == item.PK_ID).ToList();
                return lstCentre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         public List<CsCentre> SelectCentreBySiteId(int pId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCentre>(ParamProcedure.PARAM_CENTRE_RETOURNEBySiteId(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public CsCentre SelectCentreByCodeSiteCodeCentre(int pCodeSite, int pCodeCentre)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCentre>(ParamProcedure.PARAM_CENTRE_RETOURNEByIdSiteIdCentre(pCodeSite, pCodeCentre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCentre pCentre)
        {
            try
            {
                pCentre.DATEFIN = System.DateTime.Today;
                return Entities.UpdateEntity <Galatee.Entity.Model.CENTRE>(Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(pCentre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCentre> pCentreCollection)
        {
            try
            {
                pCentreCollection.ForEach(t => t.DATEFIN = System.DateTime.Today);
                return Entities.UpdateEntity <Galatee.Entity.Model.CENTRE>(Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(pCentreCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCentre pCentre)
        {
            try
            {
                //return Entities.UpdateEntity<Galatee.Entity.Model.CENTRE>(Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(pCentre));

                List<int> lstIdCentreInit = new List<int>();
                List<int> lstIdProduitAnc = new List<int>();
                List<int> lstIdProduitNouv = new List<int>();

                List<int> lstIdProduitAJouter = new List<int>();
                List<int> lstIdProduitSupprimer = new List<int>();
                List<int> lstIdProduitNonModifier = new List<int>();

                List<CsProduit> lstProduitAsupp = new List<CsProduit>();
                List<CsProduit> lstProduitAjout = new List<CsProduit>();

                List<CsProduit> lstProduitCentre = Entities.GetEntityListFromQuery<CsProduit>(ParamProcedure.PARAM_PRODUIT_CENTRE(pCentre.FK_IDCODESITE));

                foreach (CsProduit items in lstProduitCentre)
                    lstIdProduitAnc.Add(items.FK_IDPRODUIT);

                foreach (CsProduit items in pCentre.LESPRODUITSDUSITE)
                    lstIdProduitNouv.Add(items.FK_IDPRODUIT);

                lstIdProduitAJouter = lstIdProduitNouv.Where(t => !lstIdProduitAnc.Contains(t)).ToList();
                lstProduitAjout = pCentre.LESPRODUITSDUSITE.Where(t => lstIdProduitAJouter.Contains(t.FK_IDPRODUIT)).ToList();

                // Supprimer
                lstIdProduitSupprimer = lstIdProduitAnc.Where(t => !lstIdProduitNouv.Contains(t)).ToList();
                lstProduitAsupp = lstProduitCentre.Where(t => lstIdProduitSupprimer.Contains(t.FK_IDPRODUIT)).ToList();
                lstProduitAsupp.ForEach(t => t.DATEFIN = System.DateTime.Today);
                //

                CENTRE leCentre = new CENTRE();
                leCentre = Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(pCentre);
                leCentre.PRODUITCENTRE = Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE, CsProduit>(pCentre.LESPRODUITSDUSITE);
                int result = -1;
                using (galadbEntities ctx = new galadbEntities())
                {
                    Entities.UpdateEntity<Galatee.Entity.Model.CENTRE>(leCentre, ctx);
                    Entities.InsertEntity<Galatee.Entity.Model.PRODUITCENTRE>(Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE, CsProduit>(lstProduitAsupp), ctx);
                    Entities.InsertEntity<Galatee.Entity.Model.PRODUITCENTRE>(Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE, CsProduit>(lstProduitAjout), ctx);
                    result = ctx.SaveChanges();
                }
                return result == -1 ? false : true;
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCentre> pCsCentreCollection)
        {
            try
            {

                 
                galadbEntities ctx = new galadbEntities();
                int result = -1;
                foreach (CsCentre item in pCsCentreCollection)
                {

                    List<int> lstIdCentreInit = new List<int>();
                    List<int> lstIdProduitAnc = new List<int>();
                    List<int> lstIdProduitNouv = new List<int>();

                    List<int> lstIdProduitAJouter = new List<int>();
                    List<int> lstIdProduitSupprimer = new List<int>();
                    List<int> lstIdProduitNonModifier = new List<int>();

                    List<PRODUITCENTRE> lstProduitAsupp = new List<PRODUITCENTRE>();
                    List<PRODUITCENTRE> lstProduitAjout = new List<PRODUITCENTRE>();

                    List<CsProduit> lstProduitCentre = Entities.GetEntityListFromQuery<CsProduit>(ParamProcedure.PARAM_PRODUIT_CENTRE(item.PK_ID ));

                    foreach (CsProduit items in lstProduitCentre)
                        lstIdProduitAnc.Add(items.FK_IDPRODUIT);

                    foreach (CsProduit items in item.LESPRODUITSDUSITE)
                        lstIdProduitNouv.Add(items.FK_IDPRODUIT);

                    lstIdProduitAJouter = lstIdProduitNouv.Where(t => !lstIdProduitAnc.Contains(t)).ToList();
                    lstProduitAjout = Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE, CsProduit>(item.LESPRODUITSDUSITE.Where(t => lstIdProduitAJouter.Contains(t.FK_IDPRODUIT)).ToList()) ;

                    // Supprimer
                    lstIdProduitSupprimer = lstIdProduitAnc.Where(t => !lstIdProduitNouv.Contains(t)).ToList();
                    lstProduitAsupp =  Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE, CsProduit>(lstProduitCentre.Where(t => lstIdProduitSupprimer.Contains(t.FK_IDPRODUIT)).ToList()) ;
                    lstProduitAsupp.ForEach(t => t.DATEFIN = System.DateTime.Today);
                    //

                    CENTRE leCentre = new CENTRE();
                    leCentre = Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(item);
                   

                    Entities.UpdateEntity<Galatee.Entity.Model.CENTRE>(leCentre, ctx);
                    if (lstProduitAsupp != null && lstProduitAsupp.Count != 0)
                        Entities.UpdateEntity<Galatee.Entity.Model.PRODUITCENTRE>(lstProduitAsupp, ctx);

                    if (lstProduitAjout != null && lstProduitAjout.Count != 0)
                        Entities.InsertEntity<Galatee.Entity.Model.PRODUITCENTRE>(lstProduitAjout, ctx);


                }
                result=  ctx.SaveChanges();
                return result == -1 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCentre pCentre)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CENTRE>(Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(pCentre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCentre> pCentreCollection)
        {
            try
            {
                List<CENTRE> lstCentre = new List<CENTRE>();
                foreach (CsCentre item in pCentreCollection)
                {
                    CENTRE leCentre = new CENTRE();
                    leCentre = Entities.ConvertObject<Galatee.Entity.Model.CENTRE, CsCentre>(item);
	                leCentre.PRODUITCENTRE   = Entities.ConvertObject<Galatee.Entity.Model.PRODUITCENTRE , CsProduit >(item.LESPRODUITSDUSITE );
                    lstCentre.Add(leCentre);
                }
                return Entities.InsertEntity<Galatee.Entity.Model.CENTRE>(lstCentre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
