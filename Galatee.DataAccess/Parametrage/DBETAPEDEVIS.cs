
#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.ComponentModel;
using Galatee.Structure;
using System.Data.SqlClient;
using Galatee.DataAccess;
using Galatee.Structure;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess
{
	[DataObject]
    public partial class DBETAPEDEVIS /*: Galatee.DataAccess.Parametrage.DbBase*/
	{
        //public bool Transaction { get; set; }

        //private SqlCommand cmd = null;
        //private SqlConnection cn = null;

        //private void StartTransaction(SqlConnection _conn)
        //{
        //    if ((Transaction) && (_conn != null))
        //    {
        //        cmd.Transaction = this.BeginTransaction(_conn);
        //    }
        //}

        //private void CommitTransaction(SqlTransaction _pSqlTransaction)
        //{
        //    if ((Transaction) && (_pSqlTransaction != null))
        //    {
        //        this.Commit(_pSqlTransaction);
        //    }
        //}

        //private void RollBackTransaction(SqlTransaction _pSqlTransaction)
        //{
        //    if ((Transaction) && (_pSqlTransaction != null))
        //    {
        //        this.RollBack(_pSqlTransaction);
        //    }
        //}

        //public static List<ObjETAPEDEVIS> Fill(IDataReader reader, List<ObjETAPEDEVIS> rows, int start, int pageLength)
        //{
        //    // advance to the starting row
        //    for (int i = 0; i < start; i++)
        //    {
        //        if (!reader.Read())
        //            return rows; // not enough rows, just return
        //    }

        //    for (int i = 0; i < pageLength; i++)
        //    {
        //        if (!reader.Read())
        //            break; // we are done

        //        var c = new Galatee.Structure.ObjETAPEDEVIS();
        //        c.PK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
        //        c.FK_IDTYPEDEVIS = (Convert.IsDBNull(reader["IdTypeDevis"])) ? (int)0 : (System.Int32)reader["IdTypeDevis"];
        //        c.FK_CODEPRODUIT = (Convert.IsDBNull(reader["CodeProduit"])) ? string.Empty : (System.String)reader["CodeProduit"];
        //        c.NUMETAPE = (Convert.IsDBNull(reader["NumEtape"])) ? (int)0 : (System.Int32)reader["NumEtape"];
        //        c.FK_IDTACHEDEVIS = (Convert.IsDBNull(reader["IdTacheDevis"])) ? (int)0 : (System.Int32)reader["IdTacheDevis"];
        //        c.FK_IDTACHESUIVANTE = (Convert.IsDBNull(reader["IdTacheSuivante"])) ? (int)0 : (System.Int32)reader["IdTacheSuivante"];
        //        if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //            c.FK_IDTACHEINTERMEDIAIRE = (System.Int32)reader["IdTacheIntermediaire"];
        //        else
        //            c.FK_IDTACHEINTERMEDIAIRE = null;
        //        if (!Convert.IsDBNull(reader["IdTacheRejet"]))
        //            c.FK_IDTACHEREJET = (System.Int32)reader["IdTacheRejet"];
        //        else
        //            c.FK_IDTACHEREJET = null;
        //        if (!Convert.IsDBNull(reader["IdTacheSaut"]))
        //            c.FK_IDTACHESAUT = (System.Int32)reader["IdTacheSaut"];
        //        else
        //            c.FK_IDTACHESAUT = null;
        //        if (!Convert.IsDBNull(reader["DelaiExecutionEtape"]))
        //            c.DELAIEXECUTIONETAPE = (byte)reader["DelaiExecutionEtape"];
        //        else
        //            c.DELAIEXECUTIONETAPE = null;
        //        if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //            c.FK_IDTACHEINTERMEDIAIRE = (byte)reader["IdTacheIntermediaire"];
        //        else
        //            c.FK_IDTACHEINTERMEDIAIRE = null;
               
        //        c.FK_MENUID = (Convert.IsDBNull(reader["MenuID"])) ? (int)0 : (System.Int32)reader["MenuID"];
        //        c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
        //        if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
        //            c.DATEMODIFICATION = null;
        //        else
        //            c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
        //        c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
        //        c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
        //        rows.Add(c);
        //    }
        //    return rows;
        //}

        // public static List<ObjETAPEDEVIS> FillTache(IDataReader reader, List<ObjETAPEDEVIS> rows, int start, int pageLength)
        //{
        //    // advance to the starting row
        //    for (int i = 0; i < start; i++)
        //    {
        //        if (!reader.Read())
        //            return rows; // not enough rows, just return
        //    }

        //    for (int i = 0; i < pageLength; i++)
        //    {
        //        if (!reader.Read())
        //            break; // we are done

        //        var c = new Galatee.Structure.ObjETAPEDEVIS();
        //        c.PK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
        //        c.FK_IDTYPEDEVIS = (Convert.IsDBNull(reader["IdTypeDevis"])) ? (int)0 : (System.Int32)reader["IdTypeDevis"];
        //        c.FK_CODEPRODUIT = (Convert.IsDBNull(reader["CodeProduit"])) ? string.Empty : (System.String)reader["CodeProduit"];
        //        c.NUMETAPE = (Convert.IsDBNull(reader["NumEtape"])) ? (int)0 : (System.Int32)reader["NumEtape"];
        //        c.FK_IDTACHEDEVIS = (Convert.IsDBNull(reader["IdTacheDevis"])) ? (int)0 : (System.Int32)reader["IdTacheDevis"];
        //        c.FK_IDTACHESUIVANTE = (Convert.IsDBNull(reader["IdTacheSuivante"])) ? (int)0 : (System.Int32)reader["IdTacheSuivante"];
        //        if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //            c.FK_IDTACHEINTERMEDIAIRE = (System.Int32)reader["IdTacheIntermediaire"];
        //        else
        //            c.FK_IDTACHEINTERMEDIAIRE = null;
        //        if (!Convert.IsDBNull(reader["IdTacheRejet"]))
        //            c.FK_IDTACHEREJET = (System.Int32)reader["IdTacheRejet"];
        //        else
        //            c.FK_IDTACHEREJET = null;
        //        if (!Convert.IsDBNull(reader["IdTacheSaut"]))
        //            c.FK_IDTACHESAUT = (System.Int32)reader["IdTacheSaut"];
        //        else
        //            c.FK_IDTACHESAUT = null;
        //        if (!Convert.IsDBNull(reader["DelaiExecutionEtape"]))
        //            c.DELAIEXECUTIONETAPE = (byte)reader["DelaiExecutionEtape"];
        //        else
        //            c.DELAIEXECUTIONETAPE = null;
        //        if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //            c.FK_IDTACHEINTERMEDIAIRE = (byte)reader["IdTacheIntermediaire"];
        //        else
        //            c.FK_IDTACHEINTERMEDIAIRE = null;

        //        c.FK_MENUID = (Convert.IsDBNull(reader["MenuID"])) ? (int)0 : (System.Int32)reader["MenuID"];
        //        c.LIBELLETACHE = (Convert.IsDBNull(reader["LibelleTache"])) ? string.Empty : (System.String)reader["LibelleTache"];
        //        c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
        //        if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
        //            c.DATEMODIFICATION = null;
        //        else
        //            c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
        //        c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
        //        c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
        //        rows.Add(c);
        //    }
        //    return rows;
        //}

        // public static List<ObjETAPEDEVIS> FillForDisplay(IDataReader reader, List<ObjETAPEDEVIS> rows, int start, int pageLength)
        // {
        //     // advance to the starting row
        //     for (int i = 0; i < start; i++)
        //     {
        //         if (!reader.Read())
        //             return rows; // not enough rows, just return
        //     }

        //     for (int i = 0; i < pageLength; i++)
        //     {
        //         if (!reader.Read())
        //             break; // we are done

        //         var c = new Galatee.Structure.ObjETAPEDEVIS();
        //         c.PK_ID = (Convert.IsDBNull(reader["Id"])) ? (int)0 : (System.Int32)reader["Id"];
        //         c.FK_IDTYPEDEVIS = (Convert.IsDBNull(reader["IdTypeDevis"])) ? (int)0 : (System.Int32)reader["IdTypeDevis"];
        //         c.FK_CODEPRODUIT = (Convert.IsDBNull(reader["CodeProduit"])) ? string.Empty : (System.String)reader["CodeProduit"];
        //         c.NUMETAPE = (Convert.IsDBNull(reader["NumEtape"])) ? (int)0 : (System.Int32)reader["NumEtape"];
        //         c.FK_IDTACHEDEVIS = (Convert.IsDBNull(reader["IdTacheDevis"])) ? (int)0 : (System.Int32)reader["IdTacheDevis"];
        //         c.FK_IDTACHESUIVANTE = (Convert.IsDBNull(reader["IdTacheSuivante"])) ? (int)0 : (System.Int32)reader["IdTacheSuivante"];
        //         if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //             c.FK_IDTACHEINTERMEDIAIRE = (System.Int32)reader["IdTacheIntermediaire"];
        //         else
        //             c.FK_IDTACHEINTERMEDIAIRE = null;
        //         if (!Convert.IsDBNull(reader["IdTacheRejet"]))
        //             c.FK_IDTACHEREJET = (System.Int32)reader["IdTacheRejet"];
        //         else
        //             c.FK_IDTACHEREJET = null;
        //         if (!Convert.IsDBNull(reader["IdTacheSaut"]))
        //             c.FK_IDTACHESAUT = (System.Int32)reader["IdTacheSaut"];
        //         else
        //             c.FK_IDTACHESAUT = null;
        //         if (!Convert.IsDBNull(reader["DelaiExecutionEtape"]))
        //             c.DELAIEXECUTIONETAPE = (byte)reader["DelaiExecutionEtape"];
        //         else
        //             c.DELAIEXECUTIONETAPE = null;
        //         if (!Convert.IsDBNull(reader["IdTacheIntermediaire"]))
        //             c.FK_IDTACHEINTERMEDIAIRE = (byte)reader["IdTacheIntermediaire"];
        //         else
        //             c.FK_IDTACHEINTERMEDIAIRE = null;

        //         c.FK_MENUID = (Convert.IsDBNull(reader["MenuID"])) ? (int)0 : (System.Int32)reader["MenuID"];
        //         c.LIBELLETACHE = (Convert.IsDBNull(reader["LibelleTache"])) ? string.Empty : (System.String)reader["LibelleTache"];
        //         c.LIBELLETACHEINTERMEDIAIRE = (Convert.IsDBNull(reader["LibelleTacheIntermediaire"])) ? string.Empty : (System.String)reader["LibelleTacheIntermediaire"];
        //         c.LIBELLEPRODUIT = (Convert.IsDBNull(reader["LibelleProduit"])) ? string.Empty : (System.String)reader["LibelleProduit"];
        //         c.LIBELLETACHEREJET = (Convert.IsDBNull(reader["LibelleTacheRejet"])) ? string.Empty : (System.String)reader["LibelleTacheRejet"];
        //         c.LIBELLETACHESUIVANTE = (Convert.IsDBNull(reader["LibelleTacheSuivante"])) ? string.Empty : (System.String)reader["LibelleTacheSuivante"];
        //         c.LIBELLETACHESAUT = (Convert.IsDBNull(reader["LibelleTacheSaut"])) ? string.Empty : (System.String)reader["LibelleTacheSaut"];
        //         c.LIBELLETYPEDEVIS = (Convert.IsDBNull(reader["LibelleTypeDevis"])) ? string.Empty : (System.String)reader["LibelleTypeDevis"];
        //         c.DATECREATION = (Convert.IsDBNull(reader["DATECREATION"])) ? DateTime.MinValue : (System.DateTime)reader["DATECREATION"];
        //         if (Convert.IsDBNull(reader["DATEMODIFICATION"]))
        //             c.DATEMODIFICATION = null;
        //         else
        //             c.DATEMODIFICATION = (System.DateTime)reader["DATEMODIFICATION"];
        //         c.USERCREATION = (Convert.IsDBNull(reader["USERCREATION"])) ? string.Empty : (System.String)reader["USERCREATION"];
        //         c.USERMODIFICATION = (Convert.IsDBNull(reader["USERMODIFICATION"])) ? null : (System.String)reader["USERMODIFICATION"];
        //         rows.Add(c);
        //     }
        //     return rows;
        // }

         //public static List<ObjETAPEDEVIS> GetEtapeDevisForDisplay()
         //{
         //    ObjETAPEDEVIS row = new ObjETAPEDEVIS();

         //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
         //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNE_POUR_AFFICHAGE", connection);
         //    command.CommandType = CommandType.StoredProcedure;

         //    try
         //    {
         //        if (connection.State == ConnectionState.Closed)
         //            connection.Open();

         //        SqlDataReader reader = command.ExecuteReader();

         //        List<ObjETAPEDEVIS> tmp = new List<ObjETAPEDEVIS>();
         //        FillForDisplay(reader, tmp, 0, int.MaxValue);
         //        reader.Close();
         //        return tmp;
         //    }
         //    catch (Exception ex)
         //    {
         //        throw new Exception(ex.Message);
         //    }
         //    finally
         //    {
         //        if (connection.State == ConnectionState.Open)
         //            connection.Close();
         //        command.Dispose();
         //    }
         //}

        //public static ObjETAPEDEVIS GetById(int? id)
        //{
        //    ObjETAPEDEVIS row = new ObjETAPEDEVIS();

        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEById", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", id));
        //    param.Direction = ParameterDirection.Input;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> tmp = new List<ObjETAPEDEVIS>();
        //        Fill(reader, tmp, 0, int.MaxValue);
        //        reader.Close();

        //        if (tmp.Count == 1)
        //        {
        //            row = tmp[0];
        //        }
        //        else if (tmp.Count == 0)
        //        {
        //            row = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //    return row;
        //}

        //public static ObjETAPEDEVIS GetByMenuID(int? pMenuId)
        //{
        //    ObjETAPEDEVIS row = new ObjETAPEDEVIS();
        //    int MainMenuIdReaffecter = 1700202;
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByMenuId", connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    if ((int)pMenuId == MainMenuIdReaffecter)
        //        pMenuId = MainMenuIdReaffecter-1;
        //    SqlParameter param = command.Parameters.Add(new SqlParameter("@Id", pMenuId));
        //    param.Direction = ParameterDirection.Input;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> tmp = new List<ObjETAPEDEVIS>();
        //        Fill(reader, tmp, 0, int.MaxValue);
        //        reader.Close();

        //        if (tmp.Count> 0)
        //        {
        //            row = tmp[0];
        //        }
        //        else if (tmp.Count == 0)
        //        {
        //            row = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //    return row;
        //}

        //public static List<ObjETAPEDEVIS> GetByCodeProduit(string codeProduit)
        //{
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByCodeProduit", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@CodeProduit", SqlDbType.VarChar, 2).Value = codeProduit;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();
        //        DBBase.SetDBNullParametre(command.Parameters);

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByCodeFonction(System.String codeFonction)
        //{
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
          
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByCodeFonction", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@CodeFonction", SqlDbType.VarChar, 3).Value = codeFonction;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTypeDevis(System.Int32? idTypeDevis)
        //{
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevis", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTypeDevis", SqlDbType.Int).Value = idTypeDevis;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);
                
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        FillTache(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTacheDevis(System.Int32? idTacheDevis)
        //{
            
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTacheDevis", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTacheDevis", SqlDbType.Int).Value = idTacheDevis;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();
        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTacheSuivante(System.Int32? idTacheSuivante)
        //{
           
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTacheSuivante", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTacheSuivante", SqlDbType.Int).Value = idTacheSuivante;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTacheRejet(System.Int32? idTacheRejet)
        //{
           
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTacheRejet", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTacheRejet", SqlDbType.Int).Value = idTacheRejet;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTacheSaut(System.Int32? idTacheSaut)
        //{
            
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());

        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTacheSaut", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTacheSaut", SqlDbType.Int).Value = idTacheSaut;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<ObjETAPEDEVIS> GetByIdTacheIntermediaire(System.Int32? idTacheIntermediaire)
        //{
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTacheIntermediaire", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTacheIntermediaire", SqlDbType.Int).Value = idTacheIntermediaire;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        DBBase.SetDBNullParametre(command.Parameters);

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> rows = new List<ObjETAPEDEVIS>();
        //        Fill(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static ObjETAPEDEVIS GetByIdTypeDevisCodeProduitIdTache(System.Int32? idTypeDevis, System.String codeProduit, System.Int32? idTacheCourante)
        //{
        //    ObjETAPEDEVIS row = new ObjETAPEDEVIS();

           
        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
            
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisCodeProduitIdTache", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    command.Parameters.Add("@IdTypeDevis", SqlDbType.Int).Value = idTypeDevis;
        //    command.Parameters.Add("@CodeProduit", SqlDbType.VarChar, 2).Value = codeProduit;
        //    command.Parameters.Add("@IdTacheDevis", SqlDbType.Int).Value = idTacheCourante;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();
        //        DBBase.SetDBNullParametre(command.Parameters);

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> tmp = new List<ObjETAPEDEVIS>();
        //        FillTache(reader, tmp, 0, int.MaxValue);
        //        reader.Close();

        //        if (tmp.Count == 1)
        //        {
        //            row = tmp[0];
        //        }
        //        else if (tmp.Count == 0)
        //        {
        //            row = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //    return row;
        //}
        
        //public static ObjETAPEDEVIS GetByIdTypeDevisNumEtape(System.Int32? idTypeDevis, System.Int32? numEtape)
        //{
        //    ObjETAPEDEVIS row = new ObjETAPEDEVIS();

        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
        //    SqlCommand command = new SqlCommand("SPX_PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisNumEtape", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    SqlParameter param = command.Parameters.Add(new SqlParameter("@IdTypeDevis", idTypeDevis));
        //    param.Direction = ParameterDirection.Input;

        //    SqlParameter param1 = command.Parameters.Add(new SqlParameter("@NumEtape", numEtape));
        //    param1.Direction = ParameterDirection.Input;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();
        //        DBBase.SetDBNullParametre(command.Parameters);

        //        //Object datareader
        //        SqlDataReader reader = command.ExecuteReader();

        //        List<ObjETAPEDEVIS> tmp = new List<ObjETAPEDEVIS>();
        //        FillTache(reader, tmp, 0, int.MaxValue);
        //        reader.Close();

        //        if (tmp.Count == 1)
        //        {
        //            row = tmp[0];
        //        }
        //        else if (tmp.Count == 0)
        //        {
        //            row = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        //Fermeture base
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //    return row;
        //}
        //public bool Delete(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    int number = 0;
        //    foreach (ObjETAPEDEVIS entity in pEtapeDevisCollection)
        //    {
        //        if (Delete(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //public bool Update(ObjETAPEDEVIS pEtapeDevis)
        //{
        //    cn = new SqlConnection(Session.GetSqlConnexionString());
        //    cmd = new SqlCommand
        //    {
        //        Connection = cn,
        //        CommandType = CommandType.StoredProcedure,
        //        CommandText = "SPX_PARAM_ETAPEDEVIS_UPDATE"
        //    };
        //    cmd.Parameters.Clear();

        //    try
        //    {
        //        cmd.Parameters.AddWithValue("@Id", pEtapeDevis.PK_ID);
        //        cmd.Parameters.AddWithValue("@IdTypeDevis", pEtapeDevis.FK_IDTYPEDEVIS);
        //        cmd.Parameters.AddWithValue("@CodeProduit", pEtapeDevis.FK_CODEPRODUIT);
        //        cmd.Parameters.AddWithValue("@NumEtape", pEtapeDevis.NUMETAPE);
        //        cmd.Parameters.AddWithValue("@IdTacheDevis", pEtapeDevis.FK_IDTACHEDEVIS);
        //        cmd.Parameters.AddWithValue("@IdTacheSuivante", pEtapeDevis.FK_IDTACHESUIVANTE);
        //        cmd.Parameters.AddWithValue("@IdTacheIntermediaire", pEtapeDevis.FK_IDTACHEINTERMEDIAIRE);
        //        cmd.Parameters.AddWithValue("@IdTacheRejet", pEtapeDevis.FK_IDTACHEREJET);
        //        cmd.Parameters.AddWithValue("@IdTacheSaut", pEtapeDevis.FK_IDTACHESAUT);
        //        cmd.Parameters.AddWithValue("@DelaiExecutionEtape", pEtapeDevis.DELAIEXECUTIONETAPE);
        //        cmd.Parameters.AddWithValue("@MenuID", pEtapeDevis.FK_MENUID);
        //        cmd.Parameters.AddWithValue("@DATECREATION", pEtapeDevis.DATECREATION);
        //        cmd.Parameters.AddWithValue("@DATEMODIFICATION", pEtapeDevis.DATEMODIFICATION);
        //        cmd.Parameters.AddWithValue("@USERCREATION", pEtapeDevis.USERCREATION);
        //        cmd.Parameters.AddWithValue("@USERMODIFICATION", pEtapeDevis.USERMODIFICATION);
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

        //public bool Delete(ObjETAPEDEVIS pEtapeDevis)
        //{
        //    try
        //    {
        //        cn = new SqlConnection(Session.GetSqlConnexionString());
        //        cmd = new SqlCommand
        //        {
        //            Connection = cn,
        //            CommandType = CommandType.StoredProcedure,
        //            CommandText = "SPX_PARAM_ETAPEDEVIS_SUPPRIMER"
        //        };
        //        cmd.Parameters.Clear();
        //        cmd.Parameters.AddWithValue("@Id", pEtapeDevis.PK_ID);
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
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close();
        //        cmd.Dispose();
        //    }
        //}

        //public bool Update(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    int number = 0;
        //    foreach (ObjETAPEDEVIS entity in pEtapeDevisCollection)
        //    {
        //        if (Update(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //public bool Insert(ObjETAPEDEVIS pEtapeDevis)
        //{
        //    cn = new SqlConnection(Session.GetSqlConnexionString());
        //    cmd = new SqlCommand
        //    {
        //        Connection = cn,
        //        CommandType = CommandType.StoredProcedure,
        //        CommandText = "SPX_PARAM_ETAPEDEVIS_INSERER"
        //    };
        //    cmd.Parameters.Clear();

        //    try
        //    {
        //        cmd.Parameters.AddWithValue("@IdTypeDevis", pEtapeDevis.FK_IDTYPEDEVIS);
        //        cmd.Parameters.AddWithValue("@CodeProduit", pEtapeDevis.FK_CODEPRODUIT);
        //        cmd.Parameters.AddWithValue("@NumEtape", pEtapeDevis.NUMETAPE);
        //        cmd.Parameters.AddWithValue("@IdTacheDevis", pEtapeDevis.FK_IDTACHEDEVIS);
        //        cmd.Parameters.AddWithValue("@IdTacheSuivante", pEtapeDevis.FK_IDTACHESUIVANTE);
        //        cmd.Parameters.AddWithValue("@IdTacheIntermediaire", pEtapeDevis.FK_IDTACHEINTERMEDIAIRE);
        //        cmd.Parameters.AddWithValue("@IdTacheRejet", pEtapeDevis.FK_IDTACHEREJET);
        //        cmd.Parameters.AddWithValue("@IdTacheSaut", pEtapeDevis.FK_IDTACHESAUT);
        //        cmd.Parameters.AddWithValue("@DelaiExecutionEtape", pEtapeDevis.DELAIEXECUTIONETAPE);
        //        cmd.Parameters.AddWithValue("@MenuID", pEtapeDevis.FK_MENUID);
        //        cmd.Parameters.AddWithValue("@DATECREATION", pEtapeDevis.DATECREATION);
        //        cmd.Parameters.AddWithValue("@DATEMODIFICATION", pEtapeDevis.DATEMODIFICATION);
        //        cmd.Parameters.AddWithValue("@USERCREATION", pEtapeDevis.USERCREATION);
        //        cmd.Parameters.AddWithValue("@USERMODIFICATION", pEtapeDevis.USERMODIFICATION);

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

        //public bool Insert(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    int number = 0;
        //    foreach (ObjETAPEDEVIS entity in pEtapeDevisCollection)
        //    {
        //        if (Insert(entity))
        //        {
        //            number++;
        //        }
        //    }
        //    return number != 0;
        //}

        //public static List<CSMenuGalatee> GetAdmMenus()
        //{
        //    CSMenuGalatee row = new CSMenuGalatee();

        //    SqlConnection connection = new SqlConnection(Session.GetSqlConnexionString());
        //    SqlCommand command = new SqlCommand("SPX_DEVIS_AdmMenus_RETOURNE", connection);
        //    command.CommandType = CommandType.StoredProcedure;

        //    try
        //    {
        //        if (connection.State == ConnectionState.Closed)
        //            connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();

        //        List<CSMenuGalatee> tmp = new List<CSMenuGalatee>();
        //        FillMenus(reader, tmp, 0, int.MaxValue);
        //        reader.Close();
        //        return tmp;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //        command.Dispose();
        //    }
        //}

        //public static List<CSMenuGalatee> FillMenus(IDataReader reader, List<CSMenuGalatee> rows, int start, int pageLength)
        //{
        //    // advance to the starting row
        //    for (int i = 0; i < start; i++)
        //    {
        //        if (!reader.Read())
        //            return rows; // not enough rows, just return
        //    }

        //    for (int i = 0; i < pageLength; i++)
        //    {
        //        if (!reader.Read())
        //            break; // we are done

        //        var c = new Galatee.Structure.CSMenuGalatee();
        //        c.MenuID = (Convert.IsDBNull(reader["MenuId"])) ? (int)0 : (System.Int32)reader["MenuId"];
        //        c.MenuText = (Convert.IsDBNull(reader["MenuText"])) ? string.Empty : (System.String)reader["MenuText"];
        //        rows.Add(c);
        //    }
        //    return rows;
        //}

        //public bool Insert(ObjETAPEDEVIS pEtapeDevis)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevis));
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //public bool Insert(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.InsertEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Update(ObjETAPEDEVIS pEtapeDevis)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public bool Update(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{
        //    try
        //    {
        //        return Entities.UpdateEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(ObjETAPEDEVIS pEtapeDevis)
        //{

        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevis));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool Delete(List<ObjETAPEDEVIS> pEtapeDevisCollection)
        //{

        //    try
        //    {
        //        return Entities.DeleteEntity<Galatee.Entity.Model.ETAPEDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ETAPEDEVIS, ObjETAPEDEVIS>(pEtapeDevisCollection));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static List<ObjETAPEDEVIS> GetEtapeDevisForDisplay()
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNE_POUR_AFFICHAGE());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ObjETAPEDEVIS GetById(ObjDEVIS pdevis)
        {
            try
            {
                if (pdevis != null)
                    return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEById(pdevis));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static ObjETAPEDEVIS GetById(int? id)
        {
            try
            {
                if (id != null)
                    return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEById((int)id));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjETAPEDEVIS GetByMenuID(int? pMenuId)
        {
            int MainMenuIdReaffecter = 1700202;
            try
            {
                if (pMenuId != null)
                {
                    if ((int)pMenuId == MainMenuIdReaffecter)
                        pMenuId = MainMenuIdReaffecter - 1;
                    return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByMenuId((int)pMenuId));
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByCodeProduit(int pProduitId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEById(pProduitId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTypeDevis(System.Int32? idTypeDevis)
        {
            try
            {
                if (idTypeDevis != null)
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevis((int)idTypeDevis));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTacheSuivante(int? idTacheSuivante)
        {
            try
            {
                if (idTacheSuivante != null)
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTacheDevis((int)idTacheSuivante));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTacheRejet(System.Int32? idTacheRejet)
        {
            try
            {
                if (idTacheRejet != null)
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTacheRejet((int)idTacheRejet));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTacheSaut(System.Int32? idTacheSaut)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTacheSaut((int)idTacheSaut));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTacheIntermediaire(int? idTacheIntermediaire)
        {
            try
            {
                if (idTacheIntermediaire != null)
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTacheIntermediaire((int)idTacheIntermediaire));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjETAPEDEVIS GetByIdTypeDevisCodeProduitIdTache(System.Int32? idTypeDevis, int IdProduit, int? idTacheCourante)
        {
            try
            {
                if (idTacheCourante != null)
                    return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisIdProduitIdTache((int)idTypeDevis, IdProduit, (int)idTacheCourante));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjETAPEDEVIS GetByIdTypeDevisNumEtape(System.Int32? idTypeDevis, System.Int32? numEtape)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisNumEtape((int)idTypeDevis,  (int)numEtape));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CSMenuGalatee> GetAdmMenus()
        {
            List<CSMenuGalatee> ListeDeRetour = new List<Structure.CSMenuGalatee>();
            try
            {
                DataTable table = CommonProcedures.RetourneMenuParModule(Enumere.LibellemoduleDevis);
                var collection = Entities.GetEntityListFromQuery<CSMenuGalatee>(table);
                foreach (var item in collection)
                {
                    if (!(item.MenuID.ToString().Substring(0, 2) == "17" && !string.IsNullOrEmpty(item.FormName)))
                        continue;
                    ListeDeRetour.Add(item);
                }
                return ListeDeRetour;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjETAPEDEVIS> GetByIdTacheDevis(System.Int32? IdTacheDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTacheDevis((int)IdTacheDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjETAPEDEVIS GetByIdTypeDevisIdProduitIdTache(System.Int32? idTypeDevis, int IdProduit, int? idTacheCourante)
        {
            try
            {
                if (idTacheCourante != null)
                    return Entities.GetEntityFromQuery<ObjETAPEDEVIS>(ParamProcedure.PARAM_ETAPEDEVIS_RETOURNEByIdTypeDevisIdProduitIdTache((int)idTypeDevis, IdProduit, (int)idTacheCourante));
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}