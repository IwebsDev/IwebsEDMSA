using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;
using Galatee.Structure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace Galatee.DataAccess
{
     public class DBRECLAMATION
    {
        private string ConnectionString;
        private SqlCommand cmd = null;
        public SqlCommand Cmd
        {
            get { return cmd; }
            set { cmd = value; }
        }
        private SqlConnection cn = null;
        public DBRECLAMATION()
        {
            try
            {
                //ConnectionString = Session.GetSqlConnexionString();
                //Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                throw;
            }
        }
         public List<CsModeReception> SelectAllModeReception()
         {
             try
             {
                 List<CsModeReception> _lstModeReception = new List<CsModeReception>();
                 DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneListeModeReception();
                 _lstModeReception = Entities.GetEntityListFromQuery<CsModeReception>(dt);

                 return _lstModeReception;

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<CsRclValidation> SelectAllValidation()
         {
             try
             {
                 List<CsRclValidation> _lstValidation = new List<CsRclValidation>();
                 DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneListeValidation();
                 _lstValidation = Entities.GetEntityListFromQuery<CsRclValidation>(dt);

                 return _lstValidation;

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }
         public List<CsReclamationRcl> SelectAllReclamationRcl()
         {
             try
             {
                 List<CsReclamationRcl> _lstReclamationRcl = new List<CsReclamationRcl>();
                 DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneListeReclamation();
                 _lstReclamationRcl = Entities.GetEntityListFromQuery<CsReclamationRcl>(dt);

                 return _lstReclamationRcl;

             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

         public string ValiderInitReclamation(CsDemandeReclamation LaDemande)
         {
             try
             {


                 string DemandeID = string.Empty;
                 bool Resultat = false;
                 int resultTransaction = -1;
                 using (galadbEntities transaction = new galadbEntities())
                 {

                     try
                     {

                        
                         LaDemande.LaDemande.NUMDEM = AccueilProcedures.GetNumDevis(LaDemande.LaDemande);
                         LaDemande.ReclamationRcl.NumeroReclamation = LaDemande.LaDemande.NUMDEM;
                         LaDemande.ReclamationRcl.FK_IDDEMANDE = LaDemande.LaDemande.PK_ID;
                         //LaDemande.LeClient.NUMDEM  = LaDemande.LaDemande.NUMDEM;

                         ReclamationProcedure.InsertionInitReclamation(LaDemande, transaction);
                         resultTransaction = transaction.SaveChanges();
                         if (resultTransaction != -1)
                         {
                             LaDemande.LaDemande.PK_ID = transaction.DEMANDE.FirstOrDefault(d => d.NUMDEM == LaDemande.LaDemande.NUMDEM && d.CENTRE == LaDemande.LaDemande.CENTRE).PK_ID;
                             if (LaDemande.LaDemande.PK_ID == 0)
                             {
                                 using (galadbEntities tctx = new galadbEntities())
                                 {
                                     DEMANDE laDem = tctx.DEMANDE.FirstOrDefault(t => t.NUMDEM == LaDemande.LaDemande.NUMDEM);
                                     if (laDem != null)
                                         DemandeID = laDem.PK_ID + "." + LaDemande.LaDemande.NUMDEM;
                                 };
                             }
                             else
                                 DemandeID = LaDemande.LaDemande.PK_ID + "." + LaDemande.LaDemande.NUMDEM;

                         }
                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }

                 };
                 return DemandeID;
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message);
             }
         }

         public bool  ValiderReclamation(CsDemandeReclamation LaDemande)
         {
             try
             {


                 string DemandeID = string.Empty;
                 int resultTransaction = -1;
                 using (galadbEntities transaction = new galadbEntities())
                 {
                     try
                     {
                         ReclamationProcedure.ValiderReclamation(LaDemande, transaction);
                         resultTransaction = transaction.SaveChanges();
                         return resultTransaction == -1 ? false : true;
                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }

                 };
             }
             catch (Exception ex)
             {
                 throw new Exception(ex.Message);
             }
         }

         public CsDemandeReclamation RetourneLaDemande(int fk_demande)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_DEMANDE";
             CsDemandeReclamation _LaDemande = new CsDemandeReclamation();

             try
             {
                 galadbEntities transaction = new galadbEntities();
                 CsReclamationRcl _Reclamation = new CsReclamationRcl();
                 List<ADMUTILISATEUR> lstUser = transaction.ADMUTILISATEUR.ToList();
                 DEMANDE _DEMANDE = new DEMANDE();
                 _LaDemande.LaDemande = Entities.ConvertObject<CsDemandeBase, DEMANDE>(transaction.DEMANDE.FirstOrDefault(p => p.PK_ID == fk_demande));
                  
                 try
                 {
                     DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneListeReclamationbyDemande(fk_demande);
                     _Reclamation = Entities.GetEntityFromQuery<CsReclamationRcl>(dt);
                     if (!string.IsNullOrEmpty( _Reclamation.AgentEmetteur))
                         _Reclamation.NOMAGENTCREATION  = lstUser.FirstOrDefault(t=>t.MATRICULE ==_Reclamation.AgentEmetteur)!= null ? 
                              lstUser.FirstOrDefault(t=>t.MATRICULE ==_Reclamation.AgentEmetteur).LIBELLE : string.Empty ;

                     if (!string.IsNullOrEmpty(_Reclamation.AgentRecepteur ))
                         _Reclamation.NOMAGENTRECEPTEUR  = lstUser.FirstOrDefault(t => t.MATRICULE == _Reclamation.AgentRecepteur) != null ?
                              lstUser.FirstOrDefault(t => t.MATRICULE == _Reclamation.AgentRecepteur).LIBELLE : string.Empty;

                     if (!string.IsNullOrEmpty(_Reclamation.AgentValidation ))
                         _Reclamation.NOMAGENTVALIDATEUR = lstUser.FirstOrDefault(t => t.MATRICULE == _Reclamation.AgentValidation) != null ?
                              lstUser.FirstOrDefault(t => t.MATRICULE == _Reclamation.AgentValidation).LIBELLE : string.Empty;


                     _LaDemande.ReclamationRcl = _Reclamation;


                     DataTable dtDoc = DevisProcedures.DEVIS_DOCUMENTSCANNE_RETOURNEByIdDemande(_LaDemande.LaDemande.PK_ID);
                     _LaDemande.DonneDeDemande = Entities.GetEntityListFromQuery<ObjDOCUMENTSCANNE>(dtDoc);

                 }
                 catch (Exception)
                 {

                     _LaDemande.ReclamationRcl = null;
                 }
                 transaction.Dispose();
                 return _LaDemande;

             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public List<cStatistiqueReclamation> RetourStatistiqueReclamation(List<int> lstIdCentre, DateTime datedebut, DateTime dateFin)
         {

             try
             {
                 List<cStatistiqueReclamation> lesStat = new List<cStatistiqueReclamation>();
                     lesStat.AddRange(RetourStatistiqueReclamationSpx(datedebut, dateFin, lstIdCentre));
                 return lesStat;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public List<CsReclamationRcl> RetourneReclamation(int fk_idcentre, string centre, string client, string ordre, string numeroDemande)
         {
             //cmd.CommandText = "SPX_GUI_RETOURNE_ORDREMAX";
             try
             {

                 List<CsReclamationRcl> _ReclamationRcl = new List<CsReclamationRcl>();
                 DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneReclamation(fk_idcentre, centre, client, ordre, numeroDemande);
                 _ReclamationRcl = Entities.GetEntityListFromQuery<CsReclamationRcl>(dt);

                 return _ReclamationRcl;

             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }


         public List<cStatistiqueReclamation> RetourStatistiqueReclamationSpx(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre)
         {

             try
             {
                 cn = new SqlConnection(ConnectionString);
                 string lesCentres = DBBase.RetourneStringListeObject(lstidcentre);

                 cmd = new SqlCommand();
                 cmd.Connection = cn;
                 cmd.CommandTimeout = 3000;
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandText = "SPX_RECLAMATION_STATISTIQUERECLAMATION";
                 cmd.Parameters.Add("@dateDebut", SqlDbType.DateTime ).Value = dateDebut;
                 cmd.Parameters.Add("@dateFin", SqlDbType.DateTime).Value = dateFin;
                 cmd.Parameters.Add("@centre", SqlDbType.VarChar).Value = lesCentres;

                 DBBase.SetDBNullParametre(cmd.Parameters);
                 try
                 {
                     if (cn.State == ConnectionState.Closed)
                         cn.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     DataTable dt = new DataTable();
                     dt.Load(reader);
                     return Entities.GetEntityListFromQuery<cStatistiqueReclamation>(dt);

                 }
                 catch (Exception ex)
                 {
                     return null;
                     throw new Exception(cmd.CommandText + ":" + ex.Message);
                 }
                 finally
                 {
                     if (cn.State == ConnectionState.Open)
                         cn.Close(); // Fermeture de la connection 
                     cmd.Dispose();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public List<CsReclamationRcl> ReclamationParAgentSpx(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre)
         {

             try
             {
                 cn = new SqlConnection(ConnectionString);
                 string lesCentres = DBBase.RetourneStringListeObject(lstidcentre);
                 cmd = new SqlCommand();
                 cmd.Connection = cn;
                 cmd.CommandTimeout = 3000;
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandText = "SPX_RECLAMATION_PARAGENT";
                 cmd.Parameters.Add("@dateDebut", SqlDbType.DateTime).Value = dateDebut;
                 cmd.Parameters.Add("@dateFin", SqlDbType.DateTime).Value = dateFin;
                 cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lesCentres;

                 DBBase.SetDBNullParametre(cmd.Parameters);
                 try
                 {
                     if (cn.State == ConnectionState.Closed)
                         cn.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     DataTable dt = new DataTable();
                     dt.Load(reader);
                     return Entities.GetEntityListFromQuery<CsReclamationRcl>(dt);

                 }
                 catch (Exception ex)
                 {
                     return null;
                     throw new Exception(cmd.CommandText + ":" + ex.Message);
                 }
                 finally
                 {
                     if (cn.State == ConnectionState.Open)
                         cn.Close(); // Fermeture de la connection 
                     cmd.Dispose();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public List<cStatistiqueReclamation> SuiviTauxTraitementSpx(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre)
         {

             try
             {
                 cn = new SqlConnection(ConnectionString);
                 string lesCentres = DBBase.RetourneStringListeObject(lstidcentre);
                 cmd = new SqlCommand();
                 cmd.Connection = cn;
                 cmd.CommandTimeout = 3000;
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandText = "SPX_RECLAMATION_TAUXTRAITEMENT";
                 cmd.Parameters.Add("@dateDebut", SqlDbType.DateTime).Value = dateDebut;
                 cmd.Parameters.Add("@dateFin", SqlDbType.DateTime).Value = dateFin;
                 cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lesCentres;

                 DBBase.SetDBNullParametre(cmd.Parameters);
                 try
                 {
                     if (cn.State == ConnectionState.Closed)
                         cn.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     DataTable dt = new DataTable();
                     dt.Load(reader);
                     return  Entities.GetEntityListFromQuery<cStatistiqueReclamation>(dt);
                  

                 }
                 catch (Exception ex)
                 {
                     return null;
                     throw new Exception(cmd.CommandText + ":" + ex.Message);
                 }
                 finally
                 {
                     if (cn.State == ConnectionState.Open)
                         cn.Close(); // Fermeture de la connection 
                     cmd.Dispose();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public List<CsReclamationRcl> ListDesReclamationSpx(DateTime dateDebut, DateTime dateFin, List<int> lstidcentre, List<int> EtatReclamant)
         {

             try
             {
                 string listeTypeReclamation = DBBase.RetourneStringListeObject(EtatReclamant); ;
                 //foreach (var item in EtatReclamant)
                 //   {
                 //       listeTypeReclamation=item+","+ listeTypeReclamation;
                 //   }
                 cn = new SqlConnection(ConnectionString);
                 string lesCentres = DBBase.RetourneStringListeObject(lstidcentre);
                 cmd = new SqlCommand();
                 cmd.Connection = cn;
                 cmd.CommandTimeout = 3000;
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.CommandText = "SPX_RECLAMATION_LISTERECLAMATION";
                 cmd.Parameters.Add("@dateDebut", SqlDbType.DateTime).Value = dateDebut;
                 //cmd.Parameters.Add("@TypeReclamantion", SqlDbType.Int).Value = EtatReclamant;
                 cmd.Parameters.Add("@TypeReclamantion", SqlDbType.VarChar).Value = listeTypeReclamation;
                 cmd.Parameters.Add("@dateFin", SqlDbType.DateTime).Value = dateFin;
                 cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lesCentres;

                 DBBase.SetDBNullParametre(cmd.Parameters);
                 try
                 {
                     if (cn.State == ConnectionState.Closed)
                         cn.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     DataTable dt = new DataTable();
                     dt.Load(reader);
                     return Entities.GetEntityListFromQuery<CsReclamationRcl>(dt);

                 }
                 catch (Exception ex)
                 {
                     return null;
                     throw new Exception(cmd.CommandText + ":" + ex.Message);
                 }
                 finally
                 {
                     if (cn.State == ConnectionState.Open)
                         cn.Close(); // Fermeture de la connection 
                     cmd.Dispose();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

    #region ADO .Net from Entity : Stephen 28-01-2019
         //TriggerMenuView
         public List<CsTypeReclamationRcl> SelectAllTypeReclamationRcl()
         {
             try
             {
                 //DataTable dt = Galatee.Entity.Model.ReclamationProcedure.RetourneListeTypeReclamation();
                 DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("Rcl.RCL_TypeReclamation");
                 return Entities.GetEntityListFromQuery<CsTypeReclamationRcl>(dt);
             }
             catch (Exception ex)
             {

                 throw ex;
             }
         }

     
    #endregion

    }
}
