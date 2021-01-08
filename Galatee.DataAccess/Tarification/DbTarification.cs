using Galatee.Entity.Model;
using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.DataAccess
{
    public  class DbTarification
    {
        SqlConnection sqlConnection;
        SqlConnection sqlConnectionAbo07;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;
        private string Abo07ConnectionString;

        public DbTarification()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
               // Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Redevance
        public List<CsRedevance> LoadAllRedevance()
        {
            try
            {
                return TarificationProcedure.LoadAllRedevance();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeRedevence> LoadAllTypeRedevance()
        {
            try
            {
                return TarificationProcedure.LoadAllTypeRedevance();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeLienRedevence> ListeTypeLienRedevance()
        {
            try
            {
                return TarificationProcedure.ListeTypeLienRedevance();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTypeLienProduit> ListeTypeLienProduit()
        {
            try
            {
                return TarificationProcedure.ListeTypeLienProduit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      public int SaveRedevance(List<CsRedevance> ListeLotsScelleRecuToUpdate, List<CsRedevance> ListeLotsScelleRecuToInserte, List<CsRedevance> ListeLotsScelleRecuToDelete)
        {
            try
            {
                return TarificationProcedure.SaveRedevance(ListeLotsScelleRecuToUpdate, ListeLotsScelleRecuToInserte, ListeLotsScelleRecuToDelete);
            }
            catch (Exception ex)
            {
                throw ex;
                //return 0;
            }
            //return true;
        }


        public bool CheickCodeRedevanceExist(string Code)
        {
            try
            {
                return TarificationProcedure.CheickCodeRedevanceExist(Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        #endregion

        #region Recherche Tarif
   

        public List<CsRechercheTarif> LoadAllRechercheTarif()
        {
            try
            { 
                //Stephen 29-01-2019
                //return TarificationProcedure.LoadAllRechercheTarif();
                return this.RetourneAllRechercheTarif();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SaveRechercheTarif(List<CsRechercheTarif> ListeLotsScelleRecuToUpdate, List<CsRechercheTarif> ListeLotsScelleRecuToInserte, List<CsRechercheTarif> ListeLotsScelleRecuToDelete)
        {
            try
            {
                return TarificationProcedure.SaveRechercheTarif(ListeLotsScelleRecuToUpdate, ListeLotsScelleRecuToInserte, ListeLotsScelleRecuToDelete);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }

        public List<CsContenantCritereTarif> LoadAllContenantCritereTarif()
        {
            try
            {
                return TarificationProcedure.LoadAllContenantCritereTarif();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Variable de tarification
        public int SaveVariableTarif(List<CsVariableDeTarification> ListeVariableDeTarificationToUpdate_, List<CsVariableDeTarification> ListeVariableDeTarificationToInserte_, List<CsVariableDeTarification> ListeVariableDeTarificationToDelete_)
        {
            try
            {
                return TarificationProcedure.SaveVariableTarif(ListeVariableDeTarificationToUpdate_, ListeVariableDeTarificationToInserte_, ListeVariableDeTarificationToDelete_);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        #endregion

        #region TarifFacturation

        //Charger liste des Tarif Facturation
        public List<CsTarifFacturation> LoadAllTarifFacturation(int? idCentre, int? idProduit, int? idRedevance, int? idCodeRecherche)
        {
            try
            {
                List<CsCommune> lstCommune = new DBAccueil().ChargerLesCommune();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesCentre();
                List<CsRechercheTarif> lstRchercheTarif = new DBRECHERCHETARIF().SelectAllRechercheTarif();
                List<CsRedevance> lstRedevance = new DBREDEVANCE().GetAll();
                CsCentre leCentre = lstCentre.FirstOrDefault(t => t.PK_ID == idCentre);
                if (leCentre != null)
                {
                    if (leCentre.CODENIVEAUTARIF == Enumere.NiveauTarif_Nat)
                    {
                        CsCentre leCentreGeneral = lstCentre.FirstOrDefault(t => t.CODE == Enumere.Generale);
                        if (leCentreGeneral != null)
                            idCentre = leCentreGeneral.PK_ID;
                    }
                }
                return TarificationProcedure.LoadAllTarifFacturation(idCentre, idProduit, idRedevance, idCodeRecherche, lstCommune, lstRchercheTarif, lstRedevance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Charger liste des Tarif Facturation
        //public  List<CsTarifFacturation> LoadAllTarifFacturation()
        //{
        //      try
        //        {
        //            List<CsCommune> lstCommune = new DBAccueil().ChargerLesCommune();
        //            List<CsRechercheTarif> lstRchercheTarif = new DBRECHERCHETARIF().SelectAllRechercheTarif();
        //            List<CsRedevance> lstRedevance = new DBREDEVANCE().GetAll();
        //            return TarificationProcedure.LoadAllTarifFacturation(lstRedevance, lstRchercheTarif, lstCommune);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //}

        public List<CsDetailTarifFacturation > LoadAllDetailTarifFacturation()
        {
            try
            {
                return TarificationProcedure.LoadAllDetailTarifFacturation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SaveTarifFacturation(CsTarifFacturation Tarification, int Action)
        {
            try
            {
                return TarificationProcedure.SaveTarifFacturation(Tarification, Action);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        public bool CreateTarif(List<CsTarifFacturation> Tarification)
        {
            try
            {
                return TarificationProcedure.CreateTarif(Tarification);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return true;
        }
        //public int SaveTarifFacturation(List<CsTarifFacturation> ListeVariableDeTarificationToUpdate_, List<CsTarifFacturation> ListeVariableDeTarificationToInserte_, List<CsTarifFacturation> ListeVariableDeTarificationToDelete_)
        //{
        //    try
        //    {
        //        return TarificationProcedure.SaveTarifFacturation(ListeVariableDeTarificationToUpdate_, ListeVariableDeTarificationToInserte_, ListeVariableDeTarificationToDelete_);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //return true;
        //}
        //public bool DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre, int? produit)
        //{
        //    try
        //    {
        //        return TarificationProcedure.DuplicationTarifVersCentre(AncienIdCentre, NouveauIdCentre, produit);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
       public bool DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre, int? produit)
        {
            try
            {
                try
                {
                    cn = new SqlConnection(ConnectionString);
                    int resultat = -1;
                    cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 3000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SPX_TARIF_DUPLICATION";
                    cmd.Parameters.Add("@AncienIdCentre", SqlDbType.Int).Value = AncienIdCentre;
                    cmd.Parameters.Add("@NouveauIdCentre", SqlDbType.Int).Value = NouveauIdCentre;
                    cmd.Parameters.Add("@produitId", SqlDbType.Int).Value = produit;

                    DBBase.SetDBNullParametre(cmd.Parameters);
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                            cn.Open();
                        resultat = cmd.ExecuteNonQuery();
                        return resultat < 0 ? false : true;
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
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                   
        public List<CsUniteComptage > LoadAllUniteComptage()
        {
              try
                {
                    return TarificationProcedure.LoadAllUniteComptage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        #endregion

        #region Generation de Ctarcomp

        public List<CsTarifFacturation> LoadTarifGenerer(string FK_IDRECHERCHETARIF, string FK_IDVARIABLETARIF, string Produit)
            {
                try
                {
                    return TarificationProcedure.LoadTarifGenerer(FK_IDRECHERCHETARIF, FK_IDVARIABLETARIF, Produit);
                    //return null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        #endregion

        public List<CsLclient> ReturneListeDesImpayesSpx(int IdCentre, int? IdCategorie, int? IdTournee)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_IMPAYES_CENTRE_CATEGORIE_CENTRE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@IdCategorie", SqlDbType.Int).Value = IdCategorie;
                cmd.Parameters.Add("@IdTournee", SqlDbType.Int).Value = IdTournee;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt);
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

    #region ADO .Net from Entity : Stephen 29-01-2019

        public List<CsRechercheTarif> RetourneAllRechercheTarif()
        {
            List<CsRechercheTarif> ListeRedevance = new List<CsRechercheTarif>();
            try
            {
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("RECHERCHETARIF");
                List<CsRechercheTarif> lstTarif = Galatee.Tools.Utility.GetEntityListFromQuery<CsRechercheTarif>(dt);

                DataTable dts = DB_ParametresGeneraux.SelectAllDonneReference("CTARCOMP");
                List<CsCtarcomp> lstCtarcompt = Galatee.Tools.Utility.GetEntityListFromQuery<CsCtarcomp>(dts);

                foreach (CsRechercheTarif item in lstTarif)
                    item.CTARCOMP = lstCtarcompt.Where(t => t.FK_IDRECHERCHETARIF == item.PK_ID).ToList();

                return lstTarif;
            }
            catch (Exception ex)
            {
            }
            return ListeRedevance;
        }
        public List<CsVariableDeTarification> LoadAllVariableTarif()
        {
            try
            {
                //return TarificationProcedure.LoadAllVariableTarif();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("VARIABLETARIF");
                //return Galatee.Tools.Utility.GetEntityListFromQuery<CsVariableDeTarification>(dt);
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsVariableDeTarification> _LstItem = new List<CsVariableDeTarification>();
                _LstItem = db.LoadAllVariableTarif();
                foreach (CsVariableDeTarification i in _LstItem)
                    i.REDEVANCE_RECHERCHE = "CODE REDEVENCE(" + i.CODEREDEVENCE + ")-CODE RECHERCHE(" + i.CODERECHERCHE;
                return _LstItem;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsModeCalcul> LoadAllModeCalcule()
        {
            try
            {
                //return TarificationProcedure.LoadAllModeCalcule();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MODECALCUL");
                return Galatee.Tools.Utility.GetEntityListFromQuery<CsModeCalcul>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsModeApplicationTarif> LoadAllModeApplicationTarif()
        {
            try
            {
                //return TarificationProcedure.LoadAllModeApplicationTarif();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MODEAPPLICATIONTARIF");
                return Galatee.Tools.Utility.GetEntityListFromQuery<CsModeApplicationTarif>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    #endregion



    }
}
