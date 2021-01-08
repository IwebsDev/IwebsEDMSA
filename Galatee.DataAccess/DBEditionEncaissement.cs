using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections;
using Inova.Tools.Utilities;
//
using Galatee.Structure;
using Galatee.DataAccess;
using Galatee.Entity.Model;


namespace Galatee.DataAccess
{
    public class DBEditionEncaissement
    {
        List<COPER> Copers = new List<COPER>();
        List<LIBELLETOP> Tops = new List<LIBELLETOP>();
        List<MODEREG> Moderegs = new List<MODEREG>();
        List<CENTRE> Centres = new List<CENTRE>();
        List<CAISSE> Caisses = new List<CAISSE>();
        List<ADMUTILISATEUR> AdmUsers = new List<ADMUTILISATEUR>();

        public DBEditionEncaissement()
        {
            Copers = Entities.GetEntityListFromQuery<COPER>(CommonProcedures.RetourneTousCoper());
            Tops = Entities.GetEntityListFromQuery<LIBELLETOP>(CommonProcedures.RetourneTousLibelleTop());
            Moderegs = Entities.GetEntityListFromQuery<MODEREG>(CommonProcedures.RetourneTousModeReglement());
            Centres = Entities.GetEntityListFromQuery<CENTRE>(CommonProcedures.RetourneTousCentres());
            Caisses = Entities.GetEntityListFromQuery<CAISSE>(CommonProcedures.RetourneTousCaisse());
            AdmUsers = Entities.GetEntityListFromQuery<ADMUTILISATEUR>(CommonProcedures.RetourneTousUtilisateur());
        }

        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;

        public List<CParametre> RetourneListeDesBanques()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_LISTEBANQUE";

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<CParametre> rows = new List<CParametre>();
                //FillParametre(reader, rows, 0, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
     


       // #region   METHODES RELATIVES A L'ONGLET PRINTING

       // public List<CsEtatCaisse> RetourneEtatCaisse(string matricule)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_ETATDECAISSE_BYCOPER";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisse> rows = new List<CsEtatCaisse>();
       //         Fill(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // private List<CsEtatCaisse> Fill(SqlDataReader reader, List<CsEtatCaisse> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsEtatCaisse c = new CsEtatCaisse();
       //         c.modeReglement = (Convert.IsDBNull(reader["ModeReg"])) ? string.Empty : (System.String)reader["ModeReg"];
       //         c.orientation = (Convert.IsDBNull(reader["DC"])) ? string.Empty : (System.String)reader["DC"];
       //         c.debitespece = (Convert.IsDBNull(reader["debitespece"])) ? 0 : (System.Decimal)reader["debitespece"];
       //         c.creditespece = (Convert.IsDBNull(reader["creditspece"])) ? 0 : (System.Decimal)reader["creditspece"];
       //         c.debitchecque = (Convert.IsDBNull(reader["debitcheque"])) ? 0 : (System.Decimal)reader["debitcheque"];
       //         c.creditchecque = (Convert.IsDBNull(reader["creditcheque"])) ? 0 : (System.Decimal)reader["creditcheque"];
       //         c.libelleCoper = (Convert.IsDBNull(reader["libelle"])) ? string.Empty : (System.String)reader["libelle"];
       //         c.CodeOperation = (Convert.IsDBNull(reader["COPER"])) ? string.Empty : (System.String)reader["COPER"];
       //         c.libelleCaissiere = (Convert.IsDBNull(reader["caissiere"])) ? string.Empty : (System.String)reader["caissiere"];
       //         c.numeroCaisse = (Convert.IsDBNull(reader["Numcaisse"])) ? string.Empty : (System.String)reader["Numcaisse"];
       //         c.site = (Convert.IsDBNull(reader["siteEnc"])) ? string.Empty : (System.String)reader["siteEnc"];
       //         c.dateOperation = (Convert.IsDBNull(reader["DTRANS"])) ? string.Empty :DateTime.Parse(reader["DTRANS"].ToString()).ToShortDateString();
       //         rows.Add(c);
       //     }
       //     return rows;
       // }

       //     /// <summary>
       // /// Entete des etats de l'onglet 
       // /// PRINTING -->ITEM 2( CASH DESK LOG)
       // /// </summary>
       // public CsEnteteEtatCaisse RetourneEnteteEtatCaisse(string matricule)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_ETATCAISSE_ENTETE";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         CsEnteteEtatCaisse rows = new CsEnteteEtatCaisse();
       //         FillEnteteEtatCaisse(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // CsEnteteEtatCaisse FillEnteteEtatCaisse(SqlDataReader reader,CsEnteteEtatCaisse rows, int start, int pageLength)
       // {
       //     if (reader.Read())
       //     {
       //         rows.libelleCaissiere = (Convert.IsDBNull(reader["caissiere"])) ? string.Empty : (System.String)reader["caissiere"];
       //         rows.numeroCaisse = (Convert.IsDBNull(reader["Numcaisse"])) ? string.Empty : (System.String)reader["Numcaisse"];
       //         rows.site = (Convert.IsDBNull(reader["siteEnc"])) ? string.Empty : (System.String)reader["siteEnc"];
       //         rows.dateOperation = (Convert.IsDBNull(reader["DATETRANS"])) ? string.Empty : DateTime.Parse(reader["DATETRANS"].ToString()).ToShortDateString();
       //         rows.fondCaisse = (Convert.IsDBNull(reader["cashFloat"])) ? 0 : (System.Decimal)reader["cashFloat"];


       //         return rows;
       //     }
       //     else
       //     return null;
       // }
        
       // public List<CsEtatCaisseByNature> RetourneEtatCaisseByNature(string matricule)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_ETATCAISSE_BYNATURE";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisseByNature> rows = new List<CsEtatCaisseByNature>();
       //         FillEnteteEtatCaisseByNature(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsEtatCaisseByNature> FillEnteteEtatCaisseByNature(SqlDataReader reader, List<CsEtatCaisseByNature> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsEtatCaisseByNature c = new CsEtatCaisseByNature();

       //         c.libelleNature = (Convert.IsDBNull(reader["libelleNature"])) ? string.Empty : (System.String)reader["libelleNature"];
       //         c.credit = (Convert.IsDBNull(reader["credit"])) ? 0 : (System.Decimal)reader["credit"];
       //         c.debit = (Convert.IsDBNull(reader["debit"])) ? 0 : (System.Decimal)reader["debit"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }

       // public List<CsEtatCaisseStandard> RetourneEtatCaisseStandard(string matricule)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_ETATCAISSE_STANDARD";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisseStandard> rows = new List<CsEtatCaisseStandard>();
       //         FillEnteteEtatCaissestandard(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsEtatCaisseStandard> FillEnteteEtatCaissestandard(SqlDataReader reader, List<CsEtatCaisseStandard> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsEtatCaisseStandard c = new CsEtatCaisseStandard();

       //         c.mode_paiement = (Convert.IsDBNull(reader["modeReglement"])) ? string.Empty : (System.String)reader["modeReglement"];
       //         c.MontantOut = (Convert.IsDBNull(reader["montantOut"])) ? 0 : (System.Decimal)reader["montantOut"];
       //         c.MontantIn = (Convert.IsDBNull(reader["montantIn"])) ? 0 : (System.Decimal)reader["montantIn"];
       //         c.balance = (Convert.IsDBNull(reader["balance"])) ? 0 : (System.Decimal)reader["balance"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }


       // public List<CsPrintingAllCashReg> RetournePrintAllCashReg()
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_ALL_CASHREG";
       //    // cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsPrintingAllCashReg> rows = new List<CsPrintingAllCashReg>();
       //         FillEtatPrintingAllCashReg(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }

       // //public List<CsPrinting_3_1> RetournePrint31ByCashier(string matricule)
       // //{
       // //    cn = new SqlConnection(ConnectionString);
       // //    cmd = new SqlCommand();
       // //    cmd.Connection = cn;
       // //    cmd.CommandTimeout = 360;
       // //    cmd.CommandType = CommandType.StoredProcedure;
       // //    cmd.CommandText = "SPX_ENC_PRINTING_3_BY_CASHIER";
       // //    cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;

       // //    try
       // //    {
       // //        if (cn.State == ConnectionState.Closed)
       // //            cn.Open();

       // //        SqlDataReader reader = cmd.ExecuteReader();

       // //        List<CsPrintingAllCashReg> rows = new List<CsPrintingAllCashReg>();
       // //        FillEtatPrinting31(reader, rows, 0, int.MaxValue);
       // //        reader.Close();
       // //        return rows;
       // //    }
       // //    catch (Exception ex)
       // //    {
       // //        throw new Exception(cmd.CommandText + ":" + ex.Message);
       // //    }
       // //    finally
       // //    {
       // //        if (cn.State == ConnectionState.Open)
       // //            cn.Close(); // Fermeture de la connection 
       // //            cmd.Dispose();
       // //    }
       // //}
       // List<CsPrintingAllCashReg> FillEtatPrintingAllCashReg(SqlDataReader reader, List<CsPrintingAllCashReg> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsPrintingAllCashReg c = new CsPrintingAllCashReg();

       //         c.clientId = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
       //         c.clientName = (Convert.IsDBNull(reader["client_nom"])) ? string.Empty : (System.String)reader["client_nom"];
       //         c.nomCaissiere = (Convert.IsDBNull(reader["Nomcaissiere"])) ? string.Empty : (System.String)reader["Nomcaissiere"];
       //         c.numCaisse = (Convert.IsDBNull(reader["numCaisse"])) ? string.Empty : (System.String)reader["numCaisse"];
       //         c.codeOper = (Convert.IsDBNull(reader["libeCodeOper"])) ? string.Empty : (System.String)reader["libeCodeOper"];
       //         c.docNo = (Convert.IsDBNull(reader["NDOC"])) ? string.Empty : (System.String)reader["NDOC"];
       //         c.modeReglement = (Convert.IsDBNull(reader["mode_regl"])) ? string.Empty : (System.String)reader["mode_regl"];
       //         c.moutantIn = (Convert.IsDBNull(reader["mtIn"])) ? 0 : (System.Decimal)reader["mtIn"];
       //         c.moutantOut = (Convert.IsDBNull(reader["mtOut"])) ? 0 : (System.Decimal)reader["mtOut"];
       //         c.period = (Convert.IsDBNull(reader["REFEM"])) ? string.Empty : (System.String)reader["REFEM"];
       //         c.periodOperation = (Convert.IsDBNull(reader["dtrans"])) ? string.Empty :DateTime.Parse(reader["dtrans"].ToString()).ToShortDateString();
       //         c.receiptNo = (Convert.IsDBNull(reader["ACQUIT"])) ? string.Empty : (System.String)reader["ACQUIT"];
       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.centreLabel = (Convert.IsDBNull(reader["sitelabel"])) ? string.Empty : (System.String)reader["sitelabel"];
       //         c.collected = 0;
       //         c.lobCode = "";
       //         rows.Add(c);
       //     }
       //     return rows;
       // }


       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU REPRINT TRANSACTION LIST
       // ///                  PRINTING BY RECEIPT AND ISSUE REF 
       // ///                         FIRST ITEM PROCESS
       // /// </summary>
       // /// <returns></returns>
       // public List<CsPrintingAllCashReg> RetourneRePrintByReceiptAndIssueRef(string dateFrom, string dateTo,bool isAccountFilter)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = (isAccountFilter ? "SPX_ENC_REPRINTING_ALL_DESKLOG_BY_ACCOUNT_RECEIPISSUE" : "SPX_ENC_REPRINTING_ALL_DESKLOG_BY_TRANS_RECEIPISSUE");
       //     cmd.Parameters.Add("@DATEFROM", SqlDbType.VarChar, 10).Value = dateFrom;
       //     cmd.Parameters.Add("@DATETO", SqlDbType.VarChar, 10).Value = dateTo;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsPrintingAllCashReg> rows = new List<CsPrintingAllCashReg>();
       //         FillRePrintByReceiptAndIssueRef(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }

       // List<CsPrintingAllCashReg> FillRePrintByReceiptAndIssueRef(SqlDataReader reader, List<CsPrintingAllCashReg> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsPrintingAllCashReg c = new CsPrintingAllCashReg();

       //         c.clientId = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
       //         c.clientName = (Convert.IsDBNull(reader["client_nom"])) ? string.Empty : (System.String)reader["client_nom"];
       //         c.nomCaissiere = (Convert.IsDBNull(reader["Nomcaissiere"])) ? string.Empty : (System.String)reader["Nomcaissiere"];
       //         c.numCaisse = (Convert.IsDBNull(reader["numCaisse"])) ? string.Empty : (System.String)reader["numCaisse"];
       //         c.codeOper = (Convert.IsDBNull(reader["libCourt"])) ? string.Empty : (System.String)reader["libCourt"];
       //         c.docNo = (Convert.IsDBNull(reader["NDOC"])) ? string.Empty : (System.String)reader["NDOC"];
       //         c.modeReglement = (Convert.IsDBNull(reader["mode_regl"])) ? string.Empty : (System.String)reader["mode_regl"];
       //         c.moutantIn = (Convert.IsDBNull(reader["mtIn"])) ? 0 : (System.Decimal)reader["mtIn"];
       //         c.moutantOut = (Convert.IsDBNull(reader["mtOut"])) ? 0 : (System.Decimal)reader["mtOut"];
       //         c.period = (Convert.IsDBNull(reader["REFEM"])) ? string.Empty : (System.String)reader["REFEM"];
       //         c.periodOperation = (Convert.IsDBNull(reader["DTRANS"])) ? string.Empty : DateTime.Parse(reader["DTRANS"].ToString()).ToShortDateString();
       //         c.receiptNo = (Convert.IsDBNull(reader["ACQUIT"])) ? string.Empty : (System.String)reader["ACQUIT"];
       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.centreLabel = (Convert.IsDBNull(reader["Centrelabel"])) ? string.Empty : (System.String)reader["Centrelabel"];
       //         c.collected = 0;
       //         c.lobCode = (Convert.IsDBNull(reader["libCourt"])) ? string.Empty : (System.String)reader["libCourt"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }



       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU REPRINT TRANSACTION LIST
       // ///                   PRINTING BY RECEIPT AND ISSUE REF 
       // ///                          SECOND ITEM PROCESS
       // /// </summary>
       // /// <returns></returns>
       // public List<CsPrintingAllCashReg> RetourneRePrintByReceiptAndIssueRefByCashier(string matricule,string dateFrom, string dateTo, bool isAccountFilter)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = (isAccountFilter ? "SPX_ENC_REPRINTING_ALL_DESKLOG_BY_ACCOUNT_RECEIPISSUE_BYCASHIER" : "SPX_ENC_REPRINTING_ALL_DESKLOG_BY_TRANS_RECEIPISSUE_BYCASHIER");
       //     cmd.Parameters.Add("@DATEFROM", SqlDbType.VarChar, 10).Value = dateFrom;
       //     cmd.Parameters.Add("@DATETO", SqlDbType.VarChar, 10).Value = dateTo;
       //     cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 10).Value = matricule;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsPrintingAllCashReg> rows = new List<CsPrintingAllCashReg>();
       //         FillRePrintByReceiptAndIssueRefByCashier(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }

       // List<CsPrintingAllCashReg> FillRePrintByReceiptAndIssueRefByCashier(SqlDataReader reader, List<CsPrintingAllCashReg> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsPrintingAllCashReg c = new CsPrintingAllCashReg();

       //         c.clientId = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
       //         c.clientName = (Convert.IsDBNull(reader["client_nom"])) ? string.Empty : (System.String)reader["client_nom"];
       //         c.nomCaissiere = (Convert.IsDBNull(reader["Nomcaissiere"])) ? string.Empty : (System.String)reader["Nomcaissiere"];
       //         c.numCaisse = (Convert.IsDBNull(reader["numCaisse"])) ? string.Empty : (System.String)reader["numCaisse"];
       //         c.codeOper = (Convert.IsDBNull(reader["libCourt"])) ? string.Empty : (System.String)reader["libCourt"];
       //         c.docNo = (Convert.IsDBNull(reader["NDOC"])) ? string.Empty : (System.String)reader["NDOC"];
       //         c.modeReglement = (Convert.IsDBNull(reader["mode_regl"])) ? string.Empty : (System.String)reader["mode_regl"];
       //         c.moutantIn = (Convert.IsDBNull(reader["mtIn"])) ? 0 : (System.Decimal)reader["mtIn"];
       //         c.moutantOut = (Convert.IsDBNull(reader["mtOut"])) ? 0 : (System.Decimal)reader["mtOut"];
       //         c.period = (Convert.IsDBNull(reader["REFEM"])) ? string.Empty : (System.String)reader["REFEM"];
       //         c.periodOperation = (Convert.IsDBNull(reader["DTRANS"])) ? string.Empty : DateTime.Parse(reader["DTRANS"].ToString()).ToShortDateString();
       //         c.receiptNo = (Convert.IsDBNull(reader["ACQUIT"])) ? string.Empty : (System.String)reader["ACQUIT"];
       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.centreLabel = (Convert.IsDBNull(reader["Centrelabel"])) ? string.Empty : (System.String)reader["Centrelabel"];
       //         c.collected = 0;
       //         c.lobCode = (Convert.IsDBNull(reader["libCourt"])) ? string.Empty : (System.String)reader["libCourt"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }



       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU NEW EDTION OF ACCOUNTANCY
       // ///                      REPORT_DETAILS
       // /// </summary>
       // /// <returns></returns>
       // public List<CsNewEditionAccount> RetourneNewEditnAccReprtDtl(string matricule, string dateComptable)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_EDITION_ACCOUNT_REPORT_DETAILS";
       //     cmd.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 10).Value = matricule;
       //     cmd.Parameters.Add("@DATECOMP", SqlDbType.VarChar, 10).Value = dateComptable;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsNewEditionAccount> rows = new List<CsNewEditionAccount>();
       //         NewEditnAccReprtDtl(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }

       //void NewEditnAccReprtDtl(SqlDataReader reader, List<CsNewEditionAccount> rows, int start, int pageLength)
       // {
       //     //for (int i = 0; i < start; i++)
       //     //{
       //     //    if (!reader.Read())
       //     //        return rows;
       //     //}
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsNewEditionAccount c = new CsNewEditionAccount();

       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.coperNom = (Convert.IsDBNull(reader["libCourt"])) ? string.Empty : (System.String)reader["libCourt"];
       //         c.coperNum = (Convert.IsDBNull(reader["COPER"])) ? string.Empty : (System.String)reader["COPER"];
       //         c.creditAccount = (Convert.IsDBNull(reader["creditAcc"])) ? 0 : (System.Decimal)reader["creditAcc"];
       //         c.debitAccount = (Convert.IsDBNull(reader["debitAcc"])) ? 0 : (System.Decimal)reader["debitAcc"];
       //         c.modeReg = (Convert.IsDBNull(reader["MODEREG"])) ? string.Empty : (System.String)reader["MODEREG"];
       //         c.modeRegNom = (Convert.IsDBNull(reader["mode_regl"])) ? string.Empty : (System.String)reader["mode_regl"];
       //         c.montant = (Convert.IsDBNull(reader["montant"])) ? 0 : (System.Decimal)reader["montant"];
       //         c.natureLabel = (Convert.IsDBNull(reader["natureName"])) ?  string.Empty : (System.String)reader["natureName"];
       //         c.natureNum = (Convert.IsDBNull(reader["NATURE"])) ? string.Empty : (System.String)reader["NATURE"];
       //         c.dateCompt = (Convert.IsDBNull(reader["dtrans"])) ? string.Empty : (System.String)reader["dtrans"];
       //         c.centreLabel = (Convert.IsDBNull(reader["centreLabel"])) ? string.Empty : (System.String)reader["centreLabel"];
       //         c.numCaissier = (Convert.IsDBNull(reader["numCaisse"])) ? string.Empty : (System.String)reader["numCaisse"];
       //         rows.Add(c);
       //     }
       //     //return rows;
       // }


       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU DETAIL BANK LIST
       // /// </summary>
        
       // public List<CsDetailBankList> RetourneDetailBankList(string matricule, string bank, string guichet)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_DETAIL_BANK_LIST";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@BANK", SqlDbType.VarChar, 50).Value = bank;
       //     cmd.Parameters.Add("@CASHIER", SqlDbType.VarChar, 50).Value = guichet;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsDetailBankList> rows = new List<CsDetailBankList>();
       //         FillDetailBankList(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsDetailBankList> FillDetailBankList(SqlDataReader reader, List<CsDetailBankList> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsDetailBankList c = new CsDetailBankList();

       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.client = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
       //         c.libelleBank = (Convert.IsDBNull(reader["bankLabel"])) ? string.Empty : (System.String)reader["bankLabel"];
       //         c.matricule = (Convert.IsDBNull(reader["MATRICULE"])) ? string.Empty : (System.String)reader["MATRICULE"];
       //         c.finalMontant = (Convert.IsDBNull(reader["finalMontant"])) ? 0 : (System.Decimal)reader["finalMontant"];
       //         c.receiptNo = (Convert.IsDBNull(reader["ACQUIT"])) ? string.Empty : (System.String)reader["ACQUIT"];
       //         c.caisse = (Convert.IsDBNull(reader["caisse"])) ? string.Empty : (System.String)reader["caisse"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }

       // /// <summary>
       // /// ENTETE DES ETATS DU SOUS MENU DETAIL BANK LIST
       // /// </summary>
      
       // public CsEntetePrintingDetailedBankList RetourneEnteteDetailBankList(string matricule, string bank, string guichet)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_HEADER_DETAIL_BANK_LIST";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@BANK", SqlDbType.VarChar, 50).Value = bank;
       //     cmd.Parameters.Add("@CASHIER", SqlDbType.VarChar, 50).Value = guichet;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         CsEntetePrintingDetailedBankList rows = new CsEntetePrintingDetailedBankList();
       //         FillCsEntetePrintDtldBnkLst(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // CsEntetePrintingDetailedBankList FillCsEntetePrintDtldBnkLst(SqlDataReader reader, CsEntetePrintingDetailedBankList rows, int start, int pageLength)
       // {
       //     if (reader.Read())
       //     {
       //         rows.bankLabel = (Convert.IsDBNull(reader["bklabel"])) ? string.Empty : (System.String)reader["bklabel"];
       //         rows.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         rows.compte = (Convert.IsDBNull(reader["COMPTE"])) ? string.Empty : (System.String)reader["COMPTE"];
       //         rows.libelleSite = (Convert.IsDBNull(reader["sitelabel"])) ? string.Empty : (System.String)reader["sitelabel"];
       //         rows.numeroCaisse = (Convert.IsDBNull(reader["CAISSE"])) ? string.Empty : (System.String)reader["CAISSE"];
       //         return rows;
       //     }
       //     else
       //     return null;
       // }


       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU NEW EDITION OF  BANK LIST
       // /// </summary>

       // public List<CsDetailBankList> RetourneNewBankList(string matricule, string bank, string guichet,string date)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_EDITION_BANK_LIST";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@BANK", SqlDbType.VarChar, 50).Value = bank;
       //     cmd.Parameters.Add("@CASHIER", SqlDbType.VarChar, 50).Value = guichet;
       //     cmd.Parameters.Add("@DATE", SqlDbType.VarChar, 50).Value = date; 

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsDetailBankList> rows = new List<CsDetailBankList>();
       //         FillNewBankList(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsDetailBankList> FillNewBankList(SqlDataReader reader, List<CsDetailBankList> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsDetailBankList c = new CsDetailBankList();

       //         c.centre = (Convert.IsDBNull(reader["CENTRE"])) ? string.Empty : (System.String)reader["CENTRE"];
       //         c.client = (Convert.IsDBNull(reader["CLIENT"])) ? string.Empty : (System.String)reader["CLIENT"];
       //         c.libelleBank = (Convert.IsDBNull(reader["bankLabel"])) ? string.Empty : (System.String)reader["bankLabel"];
       //         c.matricule = (Convert.IsDBNull(reader["MATRICULE"])) ? string.Empty : (System.String)reader["MATRICULE"];
       //         c.finalMontant = (Convert.IsDBNull(reader["finalMontant"])) ? 0 : (System.Decimal)reader["finalMontant"];
       //         c.receiptNo = (Convert.IsDBNull(reader["ACQUIT"])) ? string.Empty : (System.String)reader["ACQUIT"];
       //         c.caisse = (Convert.IsDBNull(reader["caisse"])) ? string.Empty : (System.String)reader["caisse"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }


       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU NEW EDITION OF CASH DESK LOG
       // ///                      PRINTING BY NATURE
       // /// </summary>
      
       // public List<CsEtatCaisseStandard> RetourneNewEditCashLogByNature(string matricule,string date)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_EDITION_CASHDESK_BYNATURE";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = date;
            
       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisseStandard> rows = new List<CsEtatCaisseStandard>();
       //         FillNewEditCashLogByNature(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsEtatCaisseStandard> FillNewEditCashLogByNature(SqlDataReader reader, List<CsEtatCaisseStandard> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsEtatCaisseStandard c = new CsEtatCaisseStandard();

       //         c.mode_paiement = (Convert.IsDBNull(reader["natureLabel"])) ? string.Empty : (System.String)reader["natureLabel"];
       //         c.MontantOut = (Convert.IsDBNull(reader["montantOut"])) ? 0 : (System.Decimal)reader["montantOut"];
       //         c.MontantIn = (Convert.IsDBNull(reader["montantIn"])) ? 0 : (System.Decimal)reader["montantIn"];
       //         c.balance = (Convert.IsDBNull(reader["balance"])) ? 0 : (System.Decimal)reader["balance"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }


       // /// <summary>
       // /// METHODES RELATIVES AUX ETATS DU SOUS MENU NEW EDITION OF CASH DESK LOG
       // ///                         PRINTING BY CODE OPERATION
       // /// </summary>

       // public List<CsEtatCaisse> RetourneNewEditionCashDeskLogByCode(string matricule, string date)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_EDITION_CASHDESK_BYCOPER";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = date;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisse> rows = new List<CsEtatCaisse>();
       //         Fill(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }

       // /// <summary>
       // ///     METHODES RELATIVES AUX ETATS DU SOUS MENU NEW EDITION OF CASH DESK LOG
       // ///                         PRINTING STANDARD
       // /// </summary>

       // public List<CsEtatCaisseStandard> RetourneNewEditionCashDeskLogStandard(string matricule, string date)
       // {
       //     cn = new SqlConnection(ConnectionString);
       //     cmd = new SqlCommand();
       //     cmd.Connection = cn;
       //     cmd.CommandTimeout = 360;
       //     cmd.CommandType = CommandType.StoredProcedure;
       //     cmd.CommandText = "SPX_ENC_PRINTING_EDITION_CASHDESK_STANDARD";
       //     cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 5).Value = matricule;
       //     cmd.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = date;

       //     try
       //     {
       //         if (cn.State == ConnectionState.Closed)
       //             cn.Open();

       //         SqlDataReader reader = cmd.ExecuteReader();

       //         List<CsEtatCaisseStandard> rows = new List<CsEtatCaisseStandard>();
       //         FillEnteteNewEditionCashDeskLogStandard(reader, rows, 0, int.MaxValue);
       //         reader.Close();
       //         return rows;
       //     }
       //     catch (Exception ex)
       //     {
       //         throw new Exception(cmd.CommandText + ":" + ex.Message);
       //     }
       //     finally
       //     {
       //         if (cn.State == ConnectionState.Open)
       //             cn.Close(); // Fermeture de la connection 
       //         cmd.Dispose();
       //     }
       // }
       // List<CsEtatCaisseStandard> FillEnteteNewEditionCashDeskLogStandard(SqlDataReader reader, List<CsEtatCaisseStandard> rows, int start, int pageLength)
       // {
       //     for (int i = 0; i < start; i++)
       //     {
       //         if (!reader.Read())
       //             return rows;
       //     }
       //     for (int i = 0; i < pageLength; i++)
       //     {
       //         if (!reader.Read())
       //             break;

       //         CsEtatCaisseStandard c = new CsEtatCaisseStandard();

       //         c.mode_paiement = (Convert.IsDBNull(reader["modeReglement"])) ? string.Empty : (System.String)reader["modeReglement"];
       //         c.MontantOut = (Convert.IsDBNull(reader["montantOut"])) ? 0 : (System.Decimal)reader["montantOut"];
       //         c.MontantIn = (Convert.IsDBNull(reader["montantIn"])) ? 0 : (System.Decimal)reader["montantIn"];
       //         c.balance = (Convert.IsDBNull(reader["balance"])) ? 0 : (System.Decimal)reader["balance"];
       //         rows.Add(c);
       //     }
       //     return rows;
       // }

       // #endregion 

        #region METHODES RELATIVES A L'ONGLET OPERATION




        //public bool ClosureOfCashRegister(CsHabilitationCaisse laCaissehabil)
        //{
        //    //cmd.CommandText = "SPX_ENC_CASH_CLOSURE";
        //    try
        //    {
        //        List<TRANSCAISSE> trcaisse = CaisseProcedures.ObtenirTransCaiss(laCaissehabil.NUMCAISSE, laCaissehabil.MATRICULE);
        //        List<TRANSCAISB> trcaisseb = ObtenirTransCaissB(trcaisse);
        //        List<OPENINGDAY> openday = CaisseProcedures.ObtenirOpenDay(laCaissehabil.MATRICULE);

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            Entities.InsertEntity<TRANSCAISB>(trcaisseb, context);
        //            Entities.DeleteEntity<TRANSCAISSE>(trcaisse, context);
        //            Entities.UpdateEntity<OPENINGDAY>(openday, context);

        //            var laCaisse = context.CAISSE.FirstOrDefault(c => c.NUMCAISSE == laCaissehabil.NUMCAISSE);
        //            var habilitationCaisse = context.HABILITATIONCAISSE.FirstOrDefault(h => h.DATE_FIN == null && h.MATRICULE == laCaissehabil.MATRICULE && h.NUMCAISSE == laCaissehabil.NUMCAISSE );
        //            if (laCaisse != null && habilitationCaisse != null)
        //            {
        //                laCaisse.ISATTRIBUER = false;
        //                habilitationCaisse.DATE_FIN = DateTime.Now;
        //                habilitationCaisse.FONDCAIS = laCaisse.FONDCAIS;
        //                habilitationCaisse.MONTANTENCAISSE  = laCaissehabil.MONTANTENCAISSE  ;
        //                habilitationCaisse.MONTANTREVERSE  = laCaissehabil.MONTANTREVERSE  ;
        //                habilitationCaisse.ECART = laCaissehabil.ECART; 
        //            }

        //            context.SaveChanges();
        //            return true;
        //        }
        //        //return CaisseProcedures.FermetureCaisse(Caisse, matricule);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        #region METHODES RELATIVES A L'ONGLET RECEIPT


        //public List<CsReglement> RetourneListeRecuPourAnnulation(string MatriculeConect)
        //{

        //    cn = new SqlConnection(ConnectionString);

        //    cmd = new SqlCommand();
        //    cmd.Connection = cn;
        //    cmd.CommandTimeout = 360;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "SPX_ENC_RECUAANNULLE";
        //    cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 5).Value = MatriculeConect;

        //    try
        //    {
        //        if (cn.State == ConnectionState.Closed)
        //            cn.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        List<CsReglement> rows = new List<CsReglement>();
        //        FillListeRecu(reader, rows, 0, int.MaxValue);
        //        reader.Close();
        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(cmd.CommandText + ":" + ex.Message);
        //    }
        //    finally
        //    {
        //        if (cn.State == ConnectionState.Open)
        //            cn.Close(); // Fermeture de la connection 
        //        cmd.Dispose();
        //    }
        //}
        private List<CsReglement> FillListeRecu(SqlDataReader reader, List<CsReglement> rows, int start, int pageLength)
        {
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows;
            }
            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break;

                CsReglement c = new CsReglement();
                c.ACQUIT = (Convert.IsDBNull(reader["acquit"])) ? String.Empty : (System.String)reader["acquit"];
                c.CAISSE = (Convert.IsDBNull(reader["caisse"])) ? String.Empty : (System.String)reader["caisse"];
               // c.MatriculeCaiss = (Convert.IsDBNull(reader["matricule"])) ? String.Empty : (System.String)reader["matricule"];
                //c.REFFERENCEACQUIT = c.FK_CAISSE + c.ACQUIT + c.MatriculeCaiss;
                rows.Add(c);
            }
            return rows;
        }

        public List<CsReglement> RetourneListePaiementPourAnnulation(string caisse, string acquit, string MatriculeConect)
        {

            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_ENC_PAIEMENTDURECU";
            cmd.Parameters.Add("@Caisse", SqlDbType.VarChar, 3).Value = caisse;
            cmd.Parameters.Add("@Acquit", SqlDbType.VarChar, 9).Value = acquit;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar, 5).Value = MatriculeConect;

            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                List<CsReglement> rows = new List<CsReglement>();
                FillListePaiement(reader, rows, 0, int.MaxValue);
                reader.Close();
                return rows;
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }
        private List<CsReglement> FillListePaiement(SqlDataReader reader, List<CsReglement> rows, int start, int pageLength)
        {
            for (int i = 0; i < start; i++)
            {
                if (!reader.Read())
                    return rows;
            }
            for (int i = 0; i < pageLength; i++)
            {
                if (!reader.Read())
                    break;

                CsReglement c = new CsReglement();
               // c.CENTRE = (Convert.IsDBNull(reader["CENTRE"])) ? String.Empty : (System.String)reader["CENTRE"];
                //c.CLIENT = (Convert.IsDBNull(reader["CLIENT"])) ? String.Empty : (System.String)reader["CLIENT"];
                //c.ORDRE = (Convert.IsDBNull(reader["ORDRE"])) ? String.Empty : (System.String)reader["ORDRE"];
                c.REFEM = (Convert.IsDBNull(reader["REFEM"])) ? String.Empty : (System.String)reader["REFEM"];
                c.NDOC = (Convert.IsDBNull(reader["NDOC"])) ? String.Empty : (System.String)reader["NDOC"];
                //c.NATURE = (Convert.IsDBNull(reader["NATURE"])) ? String.Empty : (System.String)reader["NATURE"];
                c.DC = (Convert.IsDBNull(reader["DC"])) ? String.Empty : (System.String)reader["DC"];
                c.MONTANTPAYE = (Convert.IsDBNull(reader["montant"])) ? 0 : (System.Decimal)reader["montant"];
                c.PERCU = (Convert.IsDBNull(reader["PERCU"])) ? 0 : (System.Decimal)reader["PERCU"];
                c.RENDU = (Convert.IsDBNull(reader["RENDU"])) ? 0 : (System.Decimal)reader["RENDU"];
               // c.MODEREG = (Convert.IsDBNull(reader["MODEREG"])) ? String.Empty : (System.String)reader["MODEREG"];
                //c.TOPANNUL = (Convert.IsDBNull(reader["topannul"])) ? String.Empty : (System.String)reader["topannul"];
                c.MOISCOMPT = (Convert.IsDBNull(reader["MOISCOMPT"])) ? String.Empty : (System.String)reader["MOISCOMPT"];
               // c.COPER = (Convert.IsDBNull(reader["COPER"])) ? String.Empty : (System.String)reader["COPER"];
                c.NUMDEM = (Convert.IsDBNull(reader["NUMDEM"])) ? String.Empty : (System.String)reader["NUMDEM"];
                //c.DATEENCAISSEMENT = (Convert.IsDBNull(reader["DATEENCAISSEMENT"])) ? String.Empty : (System.String)reader["DATEENCAISSEMENT"];
               // c.MatriculeCaiss = (Convert.IsDBNull(reader["Matricule"])) ? String.Empty : (System.String)reader["Matricule"];
                c.NUMDEVIS = (Convert.IsDBNull(reader["NUMDEVIS"])) ? String.Empty : (System.String)reader["NUMDEVIS"];


                rows.Add(c);
            }
            return rows;
        }
        #endregion

        #region OPENING DAY

        /// <summary>
        /// Opening day for the cashier's operations
        /// </summary>
        /// <param name="matriCashier"></param>

        public bool InsererOpenDay(DateTime? date, string matriculeCaissier, string numcaisse, string matriculeOperateurOuverture, string raison, string matriculeClerk, bool CaisseEstManuel)
        {
            if (!CaisseEstManuel)
                CaisseProcedures.OuvertureCaisseEnLigne(date, matriculeCaissier, matriculeOperateurOuverture, numcaisse, raison);
            //cmd.CommandText = "SPX_ENC_OPENING_DAY";
            else
                CaisseProcedures.OuvertureCaisseSaisiManuel(date, matriculeCaissier,matriculeClerk, matriculeOperateurOuverture, numcaisse, raison);
            //{
            //    cmd.CommandText = "SPX_ENC_OPENING_BILLING_CLERK_DAY";
            //    cmd.Parameters.Add("@SAISI_PAR", SqlDbType.VarChar, 5).Value = matriculeClerk;

            //}
            //string numcaisse = RetourneNumCaisse(matriculeCaisse);
            cmd.Parameters.Add("@DATE", SqlDbType.VarChar, 10).Value = date;
            cmd.Parameters.Add("@MATRICULE_OPERATEUR", SqlDbType.VarChar, 5).Value = matriculeOperateurOuverture;
            cmd.Parameters.Add("@MATRICULE_CAISSIERE", SqlDbType.VarChar, 5).Value = matriculeCaissier;
            cmd.Parameters.Add("@REAZON", SqlDbType.VarChar, 500).Value = raison;
            cmd.Parameters.Add("@NUM_CAISSE", SqlDbType.VarChar, 3).Value = numcaisse;
            try
            {
                int Nombre = 0;
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    Nombre = (Convert.IsDBNull(reader["NOMBRE"])) ? 0 : int.Parse(reader["NOMBRE"].ToString());
                if (Nombre > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false ;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close(); // Fermeture de la connection 
                cmd.Dispose();
            }
        }

        public bool ValideOuvertureCaisse(DateTime? date, string matriculeCaisse,string numcaiss, string matriculeClerk, string raison, string saisipar, bool CaisseEstManuel)
        {
            try
            {
                return InsererOpenDay(date, matriculeCaisse,numcaiss, matriculeClerk, raison, saisipar, CaisseEstManuel);
                //return InsererOpenDay(date, matriculeCaisse, matriculeOperateur, raison, saisipar, CaisseEstManuel, RetourneNumCaisse(matriculeCaisse));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Opening day for the clerk's operations
        /// </summary>
        /// <param name="matriCashier"></param>
        #endregion

    }
}


