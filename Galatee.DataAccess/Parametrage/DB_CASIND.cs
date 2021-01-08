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
    /// DB_CASIND
    /// </summary>
    public class DB_CASIND /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
    //    /// <summary>
    //    /// DB_CASIND
    //    /// </summary>
    //    public DB_CASIND()
    //    {
    //        ConnectionString = Session.GetSqlConnexionString();
    //    }
    //    /// <summary>
    //    /// DB_CASIND
    //    /// </summary>
    //    /// <param name="ConnStr"></param>
    //    public DB_CASIND(string ConnStr)
    //    {
    //        ConnectionString = ConnStr;
    //    }
    //    /// <summary>
    //    /// ConnectionString
    //    /// </summary>
    //    private string ConnectionString;
    //    private SqlConnection cn = null;
    //    /// <summary>
    //    /// _Transaction
    //    /// </summary>
    //    private bool _Transaction;
    //    /// <summary>
    //    /// Transaction
    //    /// </summary>
    //    public bool Transaction
    //    {
    //        get { return _Transaction; }
    //        set { _Transaction = value; }

    //    }

    //    private SqlCommand cmd = null;

        //public static List<CsCasind> Fill(IDataReader reader, List<CsCasind> rows, int start, int pageLength)
        //{
        //    // advance to the starting row
        //    for (int i = 0; i < start; i++)
        //    {
        //        if (!reader.Read())
        //            return rows; // not enough rows, just return
        //    }

        //    for (int i = 0; i < pageLength; i++)
        //    {
        //        //if (!reader.Read())
        //        //    break; // we are done

        //        //CsCasind c = new CsCasind();
        //        //c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
        //        //c.OriginalCENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
        //        //c.CAS = (Convert.IsDBNull(reader["CAS"])) ? string.Empty : (System.String)reader["CAS"];
        //        //c.OriginalCAS = (Convert.IsDBNull(reader["CAS"])) ? string.Empty : (System.String)reader["CAS"];

        //        //if (Convert.IsDBNull(reader["CASGEN1"]))
        //        //    c.CASGEN1 = null;
        //        //else
        //        //    c.CASGEN1 = (short)reader["CASGEN1"];

        //        //if (Convert.IsDBNull(reader["CASGEN2"]))
        //        //    c.CASGEN2 = null;
        //        //else
        //        //    c.CASGEN2 = (short)reader["CASGEN2"];
        //        //if (Convert.IsDBNull(reader["CASGEN3"]))
        //        //    c.CASGEN3 = null;
        //        //else
        //        //    c.CASGEN3 = (short)reader["CASGEN3"];
        //        //if (Convert.IsDBNull(reader["CASGEN4"]))
        //        //    c.CASGEN4 = null;
        //        //else
        //        //    c.CASGEN4 = (short)reader["CASGEN4"];
        //        //if (Convert.IsDBNull(reader["CASGEN5"]))
        //        //    c.CASGEN5 = null;
        //        //else
        //        //    c.CASGEN5 = (short)reader["CASGEN5"];
        //        //if (Convert.IsDBNull(reader["CASGEN6"]))
        //        //    c.CASGEN6 = null;
        //        //else
        //        //    c.CASGEN6 = (short)reader["CASGEN6"];
        //        //if (Convert.IsDBNull(reader["CASGEN7"]))
        //        //    c.CASGEN7 = null;
        //        //else
        //        //    c.CASGEN7 = (short)reader["CASGEN7"];
        //        //if (Convert.IsDBNull(reader["CASGEN8"]))
        //        //    c.CASGEN8 = null;
        //        //else
        //        //    c.CASGEN8 = (short)reader["CASGEN8"];
        //        //if (Convert.IsDBNull(reader["CASGEN9"]))
        //        //    c.CASGEN9 = null;
        //        //else
        //        //    c.CASGEN9 = (short)reader["CASGEN9"];
        //        //if (Convert.IsDBNull(reader["CASGEN10"]))
        //        //    c.CASGEN10 = null;
        //        //else
        //        //    c.CASGEN10 = (short)reader["CASGEN10"];

        //        //c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? null : (System.DateTime?)reader["DATECREATION"];
        //        //c.DATEMODIFICATION = (Convert.IsDBNull(reader["DATEMODIFICATION"])) ? null : (System.DateTime?)reader["DATEMODIFICATION"];
        //        //c.ELSAISI1 = (Convert.IsDBNull(reader["ELSAISI1"])) ? string.Empty : (System.String)reader["ELSAISI1"];
        //        //c.ELSAISI2 = (Convert.IsDBNull(reader["ELSAISI2"])) ? null : (System.String)reader["ELSAISI2"];
        //        //c.ELSAISI3 = (Convert.IsDBNull(reader["ELSAISI3"])) ? null : (System.String)reader["ELSAISI3"];
        //        //c.ENQUETE = (Convert.IsDBNull(reader["ENQUETE"])) ? null : (System.String)reader["ENQUETE"];
        //        //c.FACTURE = (Convert.IsDBNull(reader["FACTURE"])) ? null : (System.String)reader["FACTURE"];
        //        //c.LIBCOURT = (Convert.IsDBNull(reader["LIBCOURT"])) ? null : (System.String)reader["LIBCOURT"];
        //        //c.LIBELLE = (Convert.IsDBNull(reader["LIBELLE"])) ? null : (System.String)reader["LIBELLE"];
        //        //c.LIBFAC = (Convert.IsDBNull(reader["LIBFAC"])) ? null : (System.String)reader["LIBFAC"];
        //        //c.TYPEMSGTSP = (Convert.IsDBNull(reader["TYPEMSGTSP"])) ? null : (System.String)reader["TYPEMSGTSP"];
        //        //c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
        //        //c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
        //        //c.LIBELLECENTRE = (Convert.IsDBNull(reader["LIBELLECENTRE"])) ? string.Empty : (System.String)reader["LIBELLECENTRE"];
        //        //rows.Add(c);
        //    }
        //    return rows;
        //}

        //public List<CsCasind> SelectAllCasDeReleve()
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "SPX_PARAM_CASIND_RETOURNE";

        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        StartTransaction(cn);

        //        IDataReader reader = cmd.ExecuteReader();
        //        var rows = new List<CsCasind>();
        //        Fill(reader, rows, int.MinValue, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception("SPX_PARAM_CASIND_RETOURNE" + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        //public List<CsCasind> SelectAllCasDeReleveByCentreCas(CsCasind pCasind)
        //{
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand
        //        {
        //            Connection = cn,
        //            CommandType = CommandType.StoredProcedure,
        //            CommandText = "SPX_PARAM_CASIND_RETOURNEByCENTRECAS"
        //        };
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@CENTRE", pCasind.FK_CENTRE);
        //        cmd.Parameters.AddWithValue("@CAS", pCasind.PK_CAS);

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        StartTransaction(cn);

        //        IDataReader reader = cmd.ExecuteReader();
        //        var rows = new List<CsCasind>();
        //        Fill(reader, rows, int.MinValue, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception("SPX_PARAM_CASIND_RETOURNEByCENTRECAS" + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        //public List<CsCasind> SelectAllCasDeReleveEcrasable()
        //{
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand
        //        {
        //            Connection = cn,
        //            CommandType = CommandType.StoredProcedure,
        //            CommandText = "SPX_PARAM_CASIND_RETOURNE_CASECRASABLE"
        //        };
        //        cmd.Parameters.Clear();

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        StartTransaction(cn);

        //        IDataReader reader = cmd.ExecuteReader();
        //        var rows = new List<CsCasind>();
        //        Fill(reader, rows, int.MinValue, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception("SPX_PARAM_CASIND_RETOURNE_CASECRASABLE" + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}

        //public bool Delete(CsCasind pCasind)
        //{
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand
        //        {
        //            Connection = cn,
        //            CommandType = CommandType.StoredProcedure,
        //            CommandText = "SPX_PARAM_CASIND_SUPPRIMER"
        //        };
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@CENTRE", pCasind.FK_CENTRE);
        //        cmd.Parameters.AddWithValue("@CAS", pCasind.PK_CAS);
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        int rowsAffected = cmd.ExecuteNonQuery();
        //        CommitTransaction(cmd.Transaction);
        //        return Convert.ToBoolean(rowsAffected);
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception("SPX_PARAM_CASIND_SUPPRIMER" + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close();
        //        cmd.Dispose();
        //    }
        //}

        //public bool Delete(List<CsCasind> pCasindCollection)
        //{
        //    int number = 0;
        //    foreach (CsCasind entity in pCasindCollection)
        //    {
        //        if (Delete(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //public bool Update(CsCasind pCasind)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand
        //    {
        //        Connection = cn,
        //        CommandType = CommandType.StoredProcedure,
        //        CommandText = "SPX_PARAM_CASIND_UPDATE"
        //    };
        //    cmd.Parameters.Clear();

        //    try
        //    {
        //        cmd.Parameters.AddWithValue("CENTRE", pCasind.FK_CENTRE);
        //        cmd.Parameters.AddWithValue("OriginalCENTRE", pCasind.OriginalCENTRE);
        //        cmd.Parameters.AddWithValue("CAS", pCasind.PK_CAS);
        //        cmd.Parameters.AddWithValue("OriginalCAS", pCasind.OriginalCAS);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN2", pCasind.CASGEN2);
        //        cmd.Parameters.AddWithValue("CASGEN3", pCasind.CASGEN3);
        //        cmd.Parameters.AddWithValue("CASGEN4", pCasind.CASGEN4);
        //        cmd.Parameters.AddWithValue("CASGEN5", pCasind.CASGEN5);
        //        cmd.Parameters.AddWithValue("CASGEN6", pCasind.CASGEN6);
        //        cmd.Parameters.AddWithValue("CASGEN7", pCasind.CASGEN7);
        //        cmd.Parameters.AddWithValue("CASGEN8", pCasind.CASGEN8);
        //        cmd.Parameters.AddWithValue("CASGEN9", pCasind.CASGEN9);
        //        cmd.Parameters.AddWithValue("CASGEN10", pCasind.CASGEN10);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("ELSAISI1", pCasind.ELSAISI1);
        //        cmd.Parameters.AddWithValue("ELSAISI2", pCasind.ELSAISI2);
        //        cmd.Parameters.AddWithValue("ELSAISI3", pCasind.ELSAISI3);
        //        cmd.Parameters.AddWithValue("ENQUETE", pCasind.ENQUETE);
        //        cmd.Parameters.AddWithValue("FACTURE", pCasind.FACTURE);
        //        cmd.Parameters.AddWithValue("LIBCOURT", pCasind.LIBCOURT);
        //        cmd.Parameters.AddWithValue("LIBELLE", pCasind.LIBELLE);
        //        cmd.Parameters.AddWithValue("LIBFAC", pCasind.LIBFAC);
        //        cmd.Parameters.AddWithValue("TYPEMSGTSP", pCasind.TYPEMSGTSP);
        //        cmd.Parameters.AddWithValue("TRANS", pCasind.TRANS);
        //        cmd.Parameters.AddWithValue("DATECREATION", pCasind.DATECREATION);
        //        cmd.Parameters.AddWithValue("DATEMODIFICATION", pCasind.DATEMODIFICATION);
        //        cmd.Parameters.AddWithValue("USERCREATION", pCasind.USERCREATION);
        //        cmd.Parameters.AddWithValue("USERMODIFICATION", pCasind.USERMODIFICATION);

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        SetDBNullParametre(cmd.Parameters);
        //        int rowsAffected = cmd.ExecuteNonQuery();
        //        CommitTransaction(cmd.Transaction);
        //        return Convert.ToBoolean(rowsAffected);
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
        //}

        //public bool Update(List<CsCasind> pCasindCollection)
        //{
        //    int number = 0;
        //    foreach (CsCasind entity in pCasindCollection)
        //    {
        //        if (Update(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //public bool Insert(CsCasind pCasind)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand
        //    {
        //        Connection = cn,
        //        CommandType = CommandType.StoredProcedure,
        //        CommandText = "SPX_PARAM_QUARTIER_INSERER"
        //    };
        //    cmd.Parameters.Clear();

        //    try
        //    {
        //        cmd.Parameters.AddWithValue("CENTRE", pCasind.FK_CENTRE);
        //        cmd.Parameters.AddWithValue("CAS", pCasind.PK_CAS);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN2", pCasind.CASGEN2);
        //        cmd.Parameters.AddWithValue("CASGEN3", pCasind.CASGEN3);
        //        cmd.Parameters.AddWithValue("CASGEN4", pCasind.CASGEN4);
        //        cmd.Parameters.AddWithValue("CASGEN5", pCasind.CASGEN5);
        //        cmd.Parameters.AddWithValue("CASGEN6", pCasind.CASGEN6);
        //        cmd.Parameters.AddWithValue("CASGEN7", pCasind.CASGEN7);
        //        cmd.Parameters.AddWithValue("CASGEN8", pCasind.CASGEN8);
        //        cmd.Parameters.AddWithValue("CASGEN9", pCasind.CASGEN9);
        //        cmd.Parameters.AddWithValue("CASGEN10", pCasind.CASGEN10);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("CASGEN1", pCasind.CASGEN1);
        //        cmd.Parameters.AddWithValue("ELSAISI1", pCasind.ELSAISI1);
        //        cmd.Parameters.AddWithValue("ELSAISI2", pCasind.ELSAISI2);
        //        cmd.Parameters.AddWithValue("ELSAISI3", pCasind.ELSAISI3);
        //        cmd.Parameters.AddWithValue("ENQUETE", pCasind.ENQUETE);
        //        cmd.Parameters.AddWithValue("FACTURE", pCasind.FACTURE);
        //        cmd.Parameters.AddWithValue("LIBCOURT", pCasind.LIBCOURT);
        //        cmd.Parameters.AddWithValue("LIBELLE", pCasind.LIBELLE);
        //        cmd.Parameters.AddWithValue("LIBFAC", pCasind.LIBFAC);
        //        cmd.Parameters.AddWithValue("TYPEMSGTSP", pCasind.TYPEMSGTSP);
        //        cmd.Parameters.AddWithValue("TRANS", pCasind.TRANS);
        //        cmd.Parameters.AddWithValue("DATECREATION", pCasind.DATECREATION);
        //        cmd.Parameters.AddWithValue("DATEMODIFICATION", pCasind.DATEMODIFICATION);
        //        cmd.Parameters.AddWithValue("USERCREATION", pCasind.USERCREATION);
        //        cmd.Parameters.AddWithValue("USERMODIFICATION", pCasind.USERMODIFICATION);

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);

        //        SetDBNullParametre(cmd.Parameters);
        //        int rowsAffected = cmd.ExecuteNonQuery();
        //        CommitTransaction(cmd.Transaction);
        //        return Convert.ToBoolean(rowsAffected);
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
        //}

        //public bool Insert(List<CsCasind> pCasindCollection)
        //{
        //    int number = 0;
        //    foreach (CsCasind entity in pCasindCollection)
        //    {
        //        if (Insert(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //#region Méthodes de mise à jour de la table CASIND
        ///// <summary>
        ///// Selectcasind_CASECRASABLE
        ///// </summary>
        ///// <returns></returns>
        //public List<CsCasind> Selectcasind_CASECRASABLE()
        //{
        //    List<CsCasind> casecrasable = new List<CsCasind>();
        //    CsCasind cas;

        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.SelectCASINDCasEcrasable;

        //    try
        //    {
        //        StartTransaction(cn);
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        CommitTransaction(cmd.Transaction);

        //        while (reader.Read())
        //        {
        //            cas = new CsCasind();
        //            cas.CENTRE = reader.GetValue(0).ToString().Trim();
        //            cas.CAS = reader.GetValue(1).ToString().Trim();
        //            cas.LIBELLE = reader.GetValue(2).ToString().Trim();
        //            cas.ENQUETE = reader.GetValue(3).ToString().Trim();
        //            cas.FACTURE= reader.GetValue(4).ToString().Trim();
        //            cas.LIBFAC = reader.GetValue(5).ToString().Trim();
        //            cas.CASGEN1 = Convert.ToInt16(string.IsNullOrEmpty(reader.GetValue(6).ToString().Trim())) ;
        //            cas.CASGEN2 = Convert.ToInt16(reader.GetValue(7).ToString().Trim());
        //            cas.CASGEN3 = Convert.ToInt16(reader.GetValue(8).ToString().Trim());
        //            cas.CASGEN4 = Convert.ToInt16(reader.GetValue(9).ToString().Trim());
        //            cas.CASGEN5 = Convert.ToInt16(reader.GetValue(10).ToString().Trim());
        //            cas.CASGEN6 = Convert.ToInt16(reader.GetValue(11).ToString().Trim());
        //            cas.CASGEN7 = Convert.ToInt16(reader.GetValue(12).ToString().Trim());
        //            cas.CASGEN8 = Convert.ToInt16(reader.GetValue(13).ToString().Trim());
        //            cas.CASGEN9 = Convert.ToInt16(reader.GetValue(14).ToString().Trim());
        //            cas.CASGEN10 = Convert.ToInt16(reader.GetValue(15).ToString().Trim());
        //            cas.ELSAISI1 = reader.GetValue(16).ToString().Trim();
        //            cas.ELSAISI2 = reader.GetValue(17).ToString().Trim();
        //            cas.ELSAISI3 = reader.GetValue(18).ToString().Trim();
        //            //cas.DMAJ = Convert.ToDateTime(reader.GetValue(19).ToString().Trim());
        //            //cas.TRANS = reader.GetValue(20).ToString().Trim();
        //            cas.TYPEMSGTSP = reader.GetValue(21).ToString().Trim();
        //            casecrasable.Add(cas);
        //        }
        //        //Fermeture reader
        //        reader.Close();

        //        return casecrasable;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.SelectCASINDCasEcrasable + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //            cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// SelectAll_CASIND
        ///// </summary>
        ///// <returns></returns>
        //public DataSet SelectAll_CASIND()
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.SelectCASIND;

        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = cmd;
        //        DataSet ds = new DataSet();
        //        adapter.Fill(ds);
        //        CommitTransaction(cmd.Transaction);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.SelectCASIND + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// Selectcasind_CASGEN_BY_CAS
        ///// </summary>
        ///// <param name="Cas"></param>
        ///// <returns></returns>
        //public List<CsCasind> Selectcasind_CASGEN_BY_CAS(string Cas)
        //{
        //    List<CsCasind> Listecasgen = new List<CsCasind>();
        //    CsCasind casgen;

        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.SelectCASIND_CASGENS;
        //    cmd.Parameters.Clear();

        //    cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = Cas;
        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        CommitTransaction(cmd.Transaction);

        //        while (reader.Read())
        //        {
        //            casgen = new CsCasind();
        //            casgen.CASGEN1 = Convert.ToInt16(reader.GetValue(0).ToString().Trim());
        //            casgen.CASGEN2 = Convert.ToInt16(reader.GetValue(1).ToString().Trim());
        //            casgen.CASGEN3 = Convert.ToInt16(reader.GetValue(2).ToString().Trim());
        //            casgen.CASGEN4 = Convert.ToInt16(reader.GetValue(3).ToString().Trim());
        //            casgen.CASGEN5 = Convert.ToInt16(reader.GetValue(4).ToString().Trim());
        //            casgen.CASGEN6 = Convert.ToInt16(reader.GetValue(5).ToString().Trim());
        //            casgen.CASGEN7 = Convert.ToInt16(reader.GetValue(6).ToString().Trim());
        //            casgen.CASGEN8 = Convert.ToInt16(reader.GetValue(7).ToString().Trim());
        //            casgen.CASGEN9 = Convert.ToInt16(reader.GetValue(8).ToString().Trim());
        //            casgen.CASGEN10 = Convert.ToInt16(reader.GetValue(9).ToString().Trim());
        //            Listecasgen.Add(casgen);
        //        }
        //        //Fermeture reader
        //        reader.Close();

        //        return Listecasgen;
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.SelectCASIND_CASGENS + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// Selectcasind_LIBELLE_BY_CAS
        ///// </summary>
        ///// <param name="CAS"></param>
        ///// <returns></returns>
        //public string Selectcasind_LIBELLE_BY_CAS(string CAS)
        //{
        //    string Libelle = string.Empty;

        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.SelectCASIND_LIBELLE_BY_CAS;
        //    cmd.Parameters.Clear();

        //    cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = CAS;
        //    try
        //    {

        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();
        //        StartTransaction(cn);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        CommitTransaction(cmd.Transaction);

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
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.SelectCASIND_CASGENS + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// MiseAJour_CASIND
        ///// </summary>
        ///// <param name="row"></param>
        //public void MiseAJour_CASIND(List<CsCasind> rows)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.UpdateCASIND.Trim();


        //    try
        //    {
        //        foreach (CsCasind row in rows)
        //        {
        //            cmd.Parameters.Clear();

        //            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
        //            cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = row.CAS;
        //            cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
        //            cmd.Parameters.Add("@ENQUETE", SqlDbType.VarChar).Value = row.ENQUETE;
        //            cmd.Parameters.Add("@FACTURE", SqlDbType.VarChar).Value = row.FACTURE;
        //            cmd.Parameters.Add("@LIBFAC", SqlDbType.VarChar).Value = row.LIBFAC;
        //            cmd.Parameters.Add("@CASGEN1", SqlDbType.SmallInt).Value = row.CASGEN1;
        //            cmd.Parameters.Add("@CASGEN2", SqlDbType.SmallInt).Value = row.CASGEN2;
        //            cmd.Parameters.Add("@CASGEN3", SqlDbType.SmallInt).Value = row.CASGEN3;
        //            cmd.Parameters.Add("@CASGEN4", SqlDbType.SmallInt).Value = row.CASGEN4;
        //            cmd.Parameters.Add("@CASGEN5", SqlDbType.SmallInt).Value = row.CASGEN5;
        //            cmd.Parameters.Add("@CASGEN6", SqlDbType.SmallInt).Value = row.CASGEN6;
        //            cmd.Parameters.Add("@CASGEN7", SqlDbType.SmallInt).Value = row.CASGEN7;
        //            cmd.Parameters.Add("@CASGEN8", SqlDbType.SmallInt).Value = row.CASGEN8;
        //            cmd.Parameters.Add("@CASGEN9", SqlDbType.SmallInt).Value = row.CASGEN9;
        //            cmd.Parameters.Add("@CASGEN10", SqlDbType.SmallInt).Value = row.CASGEN10;
        //            cmd.Parameters.Add("@ELSAISI1", SqlDbType.VarChar).Value = row.ELSAISI1;
        //            cmd.Parameters.Add("@ELSAISI2", SqlDbType.VarChar).Value = row.ELSAISI2;
        //            cmd.Parameters.Add("@ELSAISI3", SqlDbType.VarChar).Value = row.ELSAISI3;
        //            cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
        //            cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
        //            cmd.Parameters.Add("@LIBCOURT", SqlDbType.VarChar).Value = row.LIBCOURT;
        //            cmd.Parameters.Add("@TYPEMSGTSP", SqlDbType.VarChar).Value = row.TYPEMSGTSP;
        //            cmd.Parameters.Add("@ROWID", SqlDbType.Timestamp).Value = row.ROWID;

        //            DBBase.SetDBNullParametre(cmd.Parameters);

        //            if (cn.State == ConnectionState.Closed)
        //                cn.Open();

        //            StartTransaction(cn);

        //            cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
        //        }
        //        CommitTransaction(cmd.Transaction);
        //    }

        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.UpdateCASIND + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// Insertion_CASIND
        ///// </summary>
        ///// <param name="row"></param>
        //public void Insertion_CASIND(List<CsCasind> rows)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.InsertCASIND.Trim();


        //    try
        //    {
        //        foreach (CsCasind row in rows)
        //        {
        //            cmd.Parameters.Clear();

        //            cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = row.CENTRE;
        //            cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = row.CAS;
        //            cmd.Parameters.Add("@LIBELLE", SqlDbType.VarChar).Value = row.LIBELLE;
        //            cmd.Parameters.Add("@ENQUETE", SqlDbType.VarChar).Value = row.ENQUETE;
        //            cmd.Parameters.Add("@FACTURE", SqlDbType.VarChar).Value = row.FACTURE;
        //            cmd.Parameters.Add("@LIBFAC", SqlDbType.VarChar).Value = row.LIBFAC;
        //            cmd.Parameters.Add("@CASGEN1", SqlDbType.SmallInt).Value = row.CASGEN1;
        //            cmd.Parameters.Add("@CASGEN2", SqlDbType.SmallInt).Value = row.CASGEN2;
        //            cmd.Parameters.Add("@CASGEN3", SqlDbType.SmallInt).Value = row.CASGEN3;
        //            cmd.Parameters.Add("@CASGEN4", SqlDbType.SmallInt).Value = row.CASGEN4;
        //            cmd.Parameters.Add("@CASGEN5", SqlDbType.SmallInt).Value = row.CASGEN5;
        //            cmd.Parameters.Add("@CASGEN6", SqlDbType.SmallInt).Value = row.CASGEN6;
        //            cmd.Parameters.Add("@CASGEN7", SqlDbType.SmallInt).Value = row.CASGEN7;
        //            cmd.Parameters.Add("@CASGEN8", SqlDbType.SmallInt).Value = row.CASGEN8;
        //            cmd.Parameters.Add("@CASGEN9", SqlDbType.SmallInt).Value = row.CASGEN9;
        //            cmd.Parameters.Add("@CASGEN10", SqlDbType.SmallInt).Value = row.CASGEN10;
        //            cmd.Parameters.Add("@ELSAISI1", SqlDbType.VarChar).Value = row.ELSAISI1;
        //            cmd.Parameters.Add("@ELSAISI2", SqlDbType.VarChar).Value = row.ELSAISI2;
        //            cmd.Parameters.Add("@ELSAISI3", SqlDbType.VarChar).Value = row.ELSAISI3;
        //            cmd.Parameters.Add("@DMAJ", SqlDbType.DateTime).Value = row.DMAJ;
        //            cmd.Parameters.Add("@TRANS", SqlDbType.VarChar).Value = row.TRANS;
        //            cmd.Parameters.Add("@LIBCOURT", SqlDbType.VarChar).Value = row.LIBCOURT;
        //            cmd.Parameters.Add("@TYPEMSGTSP", SqlDbType.VarChar).Value = row.TYPEMSGTSP;

        //            DBBase.SetDBNullParametre(cmd.Parameters);

        //            if (cn.State == ConnectionState.Closed)
        //                cn.Open();

        //            StartTransaction(cn);

        //            cmd.ExecuteNonQuery(); // Exécution de la procédure stockée
        //        }

        //        CommitTransaction(cmd.Transaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.InsertCASIND + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        ///// <summary>
        ///// Testunicite_CASIND
        ///// </summary>
        ///// <param name="Centre"></param>
        ///// <param name="Cas"></param>
        ///// <returns></returns>
        //public bool Testunicite_CASIND(string Centre, string Cas)
        //{
        //    bool Result = false;
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);
        //        cmd = new SqlCommand();
        //        cmd.Connection = cn;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = EnumProcedureStockee.SelectCASINDByKey.Trim();
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
        //        cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = Cas;

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
        ///// <summary>
        ///// Delete_CASIND
        ///// </summary>
        ///// <param name="Centre"></param>
        ///// <param name="Cas"></param>
        //public void Delete_CASIND(string Centre, string Cas)
        //{
        //    cn = new SqlConnection(ConnectionString);
        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = EnumProcedureStockee.DeleteCASIND;
        //    cmd.Parameters.Clear();

        //    cmd.Parameters.Add("@CENTRE", SqlDbType.VarChar).Value = Centre;
        //    cmd.Parameters.Add("@CAS", SqlDbType.VarChar).Value = Cas;

        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        StartTransaction(cn);

        //        cmd.ExecuteNonQuery();
        //        CommitTransaction(cmd.Transaction);
        //    }
        //    catch (Exception ex)
        //    {
        //        RollBackTransaction(cmd.Transaction);
        //        throw new Exception(EnumProcedureStockee.DeleteCASIND + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close();
        //        cmd.Dispose();
        //    }
        //}

        //#endregion

        //#region Méthodes de gestion de transaction
        ///// <summary>
        ///// StartTransaction
        ///// </summary>
        ///// <param name="_conn"></param>
        //private void StartTransaction(SqlConnection _conn)
        //{
        //    if ((_Transaction) && (_conn != null))
        //    {
        //        cmd.Transaction = this.BeginTransaction(_conn);
        //    }
        //}
        ///// <summary>
        ///// CommitTransaction
        ///// </summary>
        ///// <param name="_pSqlTransaction"></param>
        //private void CommitTransaction(SqlTransaction _pSqlTransaction)
        //{
        //    if ((_Transaction) && (_pSqlTransaction != null))
        //    {
        //        this.Commit(_pSqlTransaction);
        //    }
        //}
        ///// <summary>
        ///// RollBackTransaction
        ///// </summary>
        ///// <param name="_pSqlTransaction"></param>
        //private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        //{
        //    if ((_Transaction) && (_pSqlTransaction != null))
        //    {
        //        this.RollBack(_pSqlTransaction);
        //    }

        //}

        //#endregion

        public List<CsCasind> SelectAllCasDeReleve()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCasind>(ParamProcedure.PARAM_CASIND_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCasind> SelectAllCasDeReleveByCentreCas(CsCasind pCasind)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCasind>(ParamProcedure.PARAM_CASIND_RETOURNEByCas(pCasind.PK_ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCasind> SelectAllCasDeReleveEcrasable()
        {
            List<CsCasind> ListeRetour = new List<CsCasind>();
            try
            {
               var ls = Entities.GetEntityListFromQuery<CsCasind>(ParamProcedure.PARAM_CASIND_RETOURNE());
               if (ls != null && ls.Count > 0)
               {
                   foreach (var item in ls)
                   {
                       if (Tools.Utility.IsNumeric(item.CODE ))
                       {
                           if (int.Parse(item.CODE) > Enumere.Cas80)
                               ListeRetour.Add(item);
                       }
                   }
               }
               return ListeRetour;
                //return Entities.GetEntityListFromQuery<CsCasind>(ParamProcedure.PARAM_CASIND_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsCasind pCasind)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasind));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCasind> pCasindCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasindCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCasind pCasind)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasind));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCasind> pCasindCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasindCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCasind pCasind)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasind));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCasind> pCasindCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.CASIND>(Entities.ConvertObject<Galatee.Entity.Model.CASIND, CsCasind>(pCasindCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
