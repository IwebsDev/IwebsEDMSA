using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Galatee.Entity.Model.Reports;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBReports
    {
        SqlConnection sqlConnection;
        SqlConnection sqlConnectionAbo07;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;
        private string Abo07ConnectionString;

        public DBReports()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
                Abo07ConnectionString = Session.GetSqlConnexionStringAbo07();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CsDemandeBase> ReturneDevisDevisValiderByDateCentre(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {
            try
            {
                List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
                foreach (int item in lstIdCende)
                    lstDemande.AddRange(ReturneDemandeValiderSpx(item, dtDebut, dtFin, typedemande, produit));
                return lstDemande;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDemandeBase> ReturneDevisTravauxRealiseByDateCentre(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {
            try
            {
                List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
                foreach (int item in lstIdCende)
                    lstDemande.AddRange(ReturneDemandeTerminerSpx(item, dtDebut, dtFin, typedemande, produit));
                return lstDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsDemandeBase> ReturneDevisDemandeByTypeDemande(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DEMANDE = context.DEMANDE;
                    query =
                  from _LaDemande in _DEMANDE
                  from y in _LaDemande.DCLIENT
                  from z in _LaDemande.DAG
                  where lstIdCende.Contains(_LaDemande.FK_IDCENTRE) &&
                   _LaDemande.DATECREATION >= dtDebut
                   && _LaDemande.DATECREATION <= dtFin
                   && (_LaDemande.PRODUIT == produit || string.IsNullOrEmpty(produit))
                   //&& (_LaDemande.TYPEDEMANDE == typedemande)
                  select new
                  {
                      _LaDemande.NUMDEM,
                      _LaDemande.CENTRE,
                      _LaDemande.NUMPERE,
                      _LaDemande.TYPEDEMANDE,
                      _LaDemande.DPRRDV,
                      _LaDemande.DPRDEV,
                      _LaDemande.DPREX,
                      _LaDemande.DREARDV,
                      _LaDemande.DREADEV,
                      _LaDemande.DREAEX,
                      _LaDemande.HRDVPR,
                      _LaDemande.FDEM,
                      _LaDemande.FREP,
                      _LaDemande.NOMPERE,
                      _LaDemande.NOMMERE,
                      _LaDemande.MATRICULE,
                      _LaDemande.STATUT,
                      _LaDemande.DCAISSE,
                      _LaDemande.NCAISSE,
                      _LaDemande.EXDAG,
                      _LaDemande.EXDBRT,
                      _LaDemande.PRODUIT,
                      _LaDemande.EXCL,
                      _LaDemande.CLIENT,
                      _LaDemande.EXCOMPT,
                      _LaDemande.COMPTEUR,
                      _LaDemande.EXEVT,
                      _LaDemande.CTAXEG,
                      _LaDemande.DATED,
                      _LaDemande.REFEM,
                      _LaDemande.ORDRE,
                      _LaDemande.TOPEDIT,
                      _LaDemande.FACTURE,
                      _LaDemande.DATEFLAG,
                      _LaDemande.USERCREATION,
                      _LaDemande.DATECREATION,
                      _LaDemande.DATEMODIFICATION,
                      _LaDemande.DATEFIN,
                      _LaDemande.USERMODIFICATION,
                      _LaDemande.ETAPEDEMANDE,
                      _LaDemande.PK_ID,
                      _LaDemande.FK_IDCENTRE,
                      _LaDemande.FK_IDADMUTILISATEUR,
                      _LaDemande.FK_IDTYPEDEMANDE,
                      _LaDemande.FK_IDPRODUIT,
                      _LaDemande.REGLAGECOMPTEUR,
                      LIBELLETYPEDEMANDE = _LaDemande.TYPEDEMANDE1.LIBELLE,
                      LIBELLECENTRE = _LaDemande.CENTRE1.LIBELLE,
                      LIBELLEPRODUIT = _LaDemande.PRODUIT1.LIBELLE,
                      NOMCLIENT = y.NOMABON,
                      z.TOURNEE,
                      COMMUNE = z.COMMUNE1.LIBELLE,
                      QUARTIER = z.QUARTIER1.LIBELLE,
                      RUES = z.RUES.LIBELLE,
                  };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsDemandeBase>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsDemandeBase> ReturneDemandeEnAttenteDeLiaison(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
                foreach (int item in lstIdCende)
                    lstDemande.AddRange(ReturneDemandeEnAttenteDeLiaisonSpx(item, dtDebut, dtFin, typedemande, produit));
                return lstDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDemandeBase> ReturneDemandeEnAttenteDeLiaisonSpx(int IdCentre, DateTime dtDebut, DateTime dtFin,  List<string> typedemande, string produit)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);
                string Tdem = DBBase.RetourneStringListeObject(typedemande);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_DEMANDE_NONLIE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@dtDebut", SqlDbType.DateTime).Value = dtDebut;
                cmd.Parameters.Add("@dtFin", SqlDbType.DateTime).Value = dtFin;
                cmd.Parameters.Add("@typedemande", SqlDbType.VarChar, int.MaxValue ).Value = Tdem;
                cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = produit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);

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


        public List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisation(List<int> lstIdCende, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                List<CsDemandeBase> lstDemande = new List<CsDemandeBase>();
                foreach (int item in lstIdCende)
                    lstDemande.AddRange(ReturneDemandeEnAttenteDeRealisationSpx(item, dtDebut, dtFin, typedemande, produit));
                return lstDemande;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDemandeBase> ReturneDemandeEnAttenteDeRealisationSpx(int IdCentre, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                string Tdem = DBBase.RetourneStringListeObject(typedemande);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_DEMANDE_NONREALISE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@dtDebut", SqlDbType.DateTime).Value = dtDebut;
                cmd.Parameters.Add("@dtFin", SqlDbType.DateTime).Value = dtFin;
                cmd.Parameters.Add("@typedemande", SqlDbType.VarChar, int.MaxValue).Value = Tdem;
                cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = produit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);

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

        public List<CsDemandeBase> ReturneRegistreDemande(List<int> IdCentre, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                string LesCentre = DBBase.RetourneStringListeObject(IdCentre);
                string lesTdem = DBBase.RetourneStringListeObject(typedemande);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_JOURNALDEMANDE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.VarChar, int.MaxValue).Value = LesCentre;
                cmd.Parameters.Add("@dtDebut", SqlDbType.DateTime).Value = dtDebut;
                cmd.Parameters.Add("@dtFin", SqlDbType.DateTime).Value = dtFin;
                cmd.Parameters.Add("@typedemande", SqlDbType.VarChar, int.MaxValue).Value = lesTdem;
                cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = produit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);

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


        public List<CsDemandeBase> ReturneDemandeTerminerSpx(int IdCentre, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                string lstTypeDemande = DBBase.RetourneStringListeObject(typedemande);

                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_TRAVAUXREALISE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@dtDebut", SqlDbType.DateTime).Value = dtDebut;
                cmd.Parameters.Add("@dtFin", SqlDbType.DateTime).Value = dtFin;
                cmd.Parameters.Add("@typedemande", SqlDbType.VarChar, 2).Value = lstTypeDemande;
                cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = produit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);

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
        public List<CsDemandeBase> ReturneDemandeValiderSpx(int IdCentre, DateTime dtDebut, DateTime dtFin, List<string> typedemande, string produit)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                string lstTypeDemande = DBBase.RetourneStringListeObject(typedemande);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_DEVISVALIDE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.VarChar ,int.MaxValue ).Value = IdCentre;
                cmd.Parameters.Add("@dtDebut", SqlDbType.DateTime).Value = dtDebut;
                cmd.Parameters.Add("@dtFin", SqlDbType.DateTime).Value = dtFin;
                cmd.Parameters.Add("@typedemande", SqlDbType.VarChar, int.MaxValue).Value = lstTypeDemande;
                cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = produit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDemandeBase>(dt);

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


        public List<CsCanalisation> ReturnecompteursDisponibles(List<int> lstCentre, int? idCalibreCompteur, string produit, string EtatCompteur)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Magazin = context.MAGASINVIRTUEL;
                    query =
                  from x in _Magazin
                  where lstCentre.Contains(x.CENTRE.PK_ID )
                        && (x.FK_IDCALIBRECOMPTEUR == idCalibreCompteur || idCalibreCompteur == null)
                  && (x.PRODUIT.CODE == produit || string.IsNullOrEmpty(produit))
                  && x.ETAT == "Affecté"
                  select new
                  {
                      x.NUMERO,
                      CENTRE = x.CENTRE.CODESITE,
                      CODECALIBRECOMPTEUR = x.CALIBRECOMPTEUR1.LIBELLE,
                      LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                      x.COEFLECT,
                      x.COEFCOMPTAGE,
                      x.CADRAN,
                      x.ANNEEFAB,
                      INFOCOMPTEUR = "Disponible",
                      x.ETAT,
                      x.FONCTIONNEMENT,
                      x.PLOMBAGE,
                      x.USERCREATION,
                      //x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERMODIFICATION
                  };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsCanalisation>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCanalisation> ReturnecompteursAttribue(List<int> lstCentre, int? idCalibreCompteur, string produit, string EtatCompteur, DateTime? debut, DateTime? fin)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                  /*  IEnumerable<object> query = null;
                    var _Magazin = context.MAGASINVIRTUEL;
                    query =
                  from x in _Magazin*/

                    IEnumerable<object> query =
                  from x in context.MAGASINVIRTUEL 
                  join dc in context.DCANALISATION on  x.PK_ID equals dc.FK_IDMAGAZINVIRTUEL

                  where lstCentre.Contains(x.CENTRE.PK_ID )
                        && (x.FK_IDCALIBRECOMPTEUR == idCalibreCompteur || idCalibreCompteur == null)
                  && (x.PRODUIT.CODE == produit || string.IsNullOrEmpty(produit))
                  && x.ETAT != "Affecté"
                  && (debut == null || (dc.DATECREATION >= debut && dc.DATECREATION < fin))
                  select new
                  {
                      CENTRE = x.CENTRE.CODESITE,
                      CODECALIBRECOMPTEUR = x.CALIBRECOMPTEUR1.LIBELLE,
                      LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                      x.NUMERO,
                      x.COEFLECT,
                      INFOCOMPTEUR = "Attribuer",
                      x.COEFCOMPTAGE,
                      x.CADRAN,
                      x.ANNEEFAB,
                      x.ETAT,
                      x.FONCTIONNEMENT,
                      x.PLOMBAGE,
                      x.USERCREATION,
                      dc.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERMODIFICATION,
                      dc.NUMDEM
                  };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsCanalisation>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsLclient> ReturneListeDesImpayes(Dictionary<string, List<int>> lstCentre, List<int> lstIdCategorie, List<int> lstIdTournee, bool IsDetail)
        {

            try
            {
                List<CsLclient> lesImpaye = new List<CsLclient>();
                List<int> lstIdCentre = new List<int>();
                foreach (var item in lstCentre)
                    lstIdCentre.AddRange (item.Value);  
                

                foreach (int item in lstIdCentre)
                {
                    if (lstIdCategorie != null && lstIdCategorie.Count != 0)
                    {
                        foreach (int categ in lstIdCategorie)
                        {

                            if (lstIdTournee != null && lstIdTournee.Count != 0)
                            {
                                foreach (int tourn in lstIdTournee)
                                    lesImpaye.AddRange(ReturneListeDesImpayesSpx(item, categ, tourn, IsDetail));
                            }
                            else
                                lesImpaye.AddRange(ReturneListeDesImpayesSpx(item, categ, null, IsDetail));
                        }
                    }
                    else
                    {
                        if (lstIdTournee != null && lstIdTournee.Count != 0)
                        {
                            foreach (int tourn in lstIdTournee)
                                lesImpaye.AddRange(ReturneListeDesImpayesSpx(item, null, tourn, IsDetail));
                        }
                        else
                            lesImpaye.AddRange(ReturneListeDesImpayesSpx(item, null, null, IsDetail));
                    }
                }
                return lesImpaye;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> ReturneListeDesImpayesSpx(int IdCentre, int? IdCategorie, int? IdTournee, bool IsDetail)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                if (!IsDetail)
                cmd.CommandText = "SPX_RPT_IMPAYES_CENTRE";
                else
                    cmd.CommandText = "SPX_RPT_IMPAYES_CENTRE_DETAILS";

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

        public List<CsRedevanceFacture> ReturneVentePeriodeAnnee(List<int> lstIdCentre, string periode, string Annee, bool Isrecap)
        {

            try
            {
                List<CsRedevanceFacture> lesVente = new List<CsRedevanceFacture>();
                if (Isrecap)
                {
                    foreach (int item in lstIdCentre)
                        lesVente.AddRange(ReturneVentePeriodeAnneeRecapSpx(item, periode, Annee));
                }
                else
                {
                    foreach (int item in lstIdCentre)
                        lesVente.AddRange(ReturneVentePeriodeAnneeDetailSpx(item, periode, Annee));
                }
                return lesVente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRedevanceFacture> ReturneVentePeriodeAnneeRecapSpx(int IdCentre, string periode, string Annee)
        {

            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_VENTE_PERIODE_RECAP";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = string.IsNullOrEmpty(periode) ? null : periode;
                cmd.Parameters.Add("@Annee", SqlDbType.VarChar, 4).Value = string.IsNullOrEmpty(Annee) ? null : Annee;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);

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
        public List<CsRedevanceFacture> ReturneVentePeriodeAnneeDetailSpx(int IdCentre, string periode, string Annee)
        {

            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_VENTE_PERIODE_DETAIL";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@Annee", SqlDbType.VarChar, 4).Value = Annee;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);

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

        public List<CsEvenement> ReturneNombreMoyenDeFacturation(string CodeSite, string Periode)
        {
            List<CsEvenement> lesHistorique = new List<CsEvenement>();
            lesHistorique.AddRange(ReturneNombreMoyenDeFacturationSpx(CodeSite, Periode));
            return lesHistorique;
        }
        public List<CsEvenement> ReturneNombreMoyenDeFacturationSpx(string CodeSite, string Periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_NOMBRE_DE_JOURS";
                cmd.Parameters.Add("@codeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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


        public List<CsEvenement> ReturneListeDesCasReturneListeDesCas(List<int> lstIdCentre, List<int?> lstIdTournee, List<string> lstCas, string Periode, string Lotri)
        {
            List<CsEvenement> lesHistorique = new List<CsEvenement>();
            foreach (int item in lstIdCentre)
            {
                foreach (int items in lstIdTournee)
                {
                    foreach (string itemCas in lstCas)
                    {

                    }
                }
            }
            return lesHistorique;
        }
        public List<CsEvenement> ReturneListeDesCas(int IdCentre, int? IdTournee, string Periode, string Lotri)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_IMPAYES_CENTRE_CATEGORIE_CENTRE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@IdCategorie", SqlDbType.Int).Value = IdTournee;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
                cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 6).Value = Lotri;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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

        public List<CsEvenement> ReturneConsNull(Dictionary<string, List<int>> lstSiteCentre,string Periode )
        {
            List<CsEvenement> lesHistorique = new List<CsEvenement>();
            foreach (var item in lstSiteCentre)
                    lesHistorique.AddRange(ReturneConsNullSpx(item.Value , Periode));
            return lesHistorique;
        }
        public List<CsRedevanceFacture> ReturneVenteCummule(List<int> lstIdCentre, string Periode, bool IsSatistique)
        {
            return ReturneVenteCummuleSpx(lstIdCentre, Periode, IsSatistique);
        }
        public List<CsRedevanceFacture> ReturneVenteCummuleSpx(List<int> lstIdCentre, string Periode, bool IsSatistique)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);
                string lstId = DBBase.RetourneStringListeObject(lstIdCentre);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_VENTECUMMULE";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lstId;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
                cmd.Parameters.Add("@IsSatistique", SqlDbType.Bit).Value = IsSatistique;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsRedevanceFacture>(dt);

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


        public List<CsEvenement> ReturneConsNullSpx(List<int>lstCentre, string Periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                string lesCentre = DBBase.RetourneStringListeObject(lstCentre);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_LISTE_CONSO_NULL";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lesCentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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

        public List<CsEvenement> ReturneActionFacturation(List<int> lstIdCentre, string Periode, string Lotri)
        {
            List<CsEvenement> lesHistorique = new List<CsEvenement>();
            foreach (int item in lstIdCentre)
                lesHistorique.AddRange(ReturneActionFacturationSpx(item, Periode, Lotri));
            return lesHistorique;
        }
        public List<CsEvenement> ReturneActionFacturationSpx(int IdCentre, string Periode, string Lotri)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_SUIVIELOT";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = IdCentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, 6).Value = Periode;
                cmd.Parameters.Add("@Lotri", SqlDbType.VarChar, 8).Value = Lotri;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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


        public List<CsEvenement> ReturneCompteurParProduit(List<int> lstIdCentre, List<int> lstIdProduit)
        {
            return ReturneCompteurParProduitSpx(lstIdCentre, lstIdProduit);
        }
        public List<CsEvenement> ReturneCompteurParProduitSpx(List<int> IdCentre, List<int> IdProduit)
        {
            try
            {

                string lstIdCentre = DBBase.RetourneStringListeObject(IdCentre);
                string lstIdProduit = DBBase.RetourneStringListeObject(IdProduit);

                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_COMPTEUR_PRODUIT";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lstIdCentre;
                cmd.Parameters.Add("@IDPRODUITLIST", SqlDbType.VarChar, int.MaxValue).Value = lstIdProduit;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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

        public List<CsEvenement> ReturneCompteurParProduitPeriode(Dictionary<string, List<int>> lesDeCentre, string periode, bool IsStat)
        {
            List<CsEvenement> lesHistorique = new List<CsEvenement>();
            foreach (var item in lesDeCentre)
                lesHistorique.AddRange(ReturneCompteurParProduitPeriodeSpx(item.Key, item.Value, periode, IsStat));
            return lesHistorique;
        }
        public List<CsEvenement> ReturneCompteurParProduitPeriodeSpx(string CodeSite, List<int> lstIdCentre, string periode,bool IsStat)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_COMPTEUR_FACTURE";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar ,3).Value = CodeSite;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar).Value = periode;
                cmd.Parameters.Add("@IsStat", SqlDbType.Bit).Value = IsStat;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt).Where(y=>lstIdCentre.Contains(y.FK_IDCENTRE)).ToList();

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

        public List<CsEvenement> ReturneFactureIsole(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin)
        {
            return ReturneFactureIsoleSpx(lstIdCentre, DateDebut, DateFin);
        }
        public List<CsEvenement> ReturneFactureIsoleSpx(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                string lsIdCentre = DBBase.RetourneStringListeObject(lstIdCentre);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_LISTE_FACTURE_ISOLE";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lsIdCentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsEvenement>(dt);

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

        public List<CsEvenement> ReturneAnnulationFacture(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin)
        {
            return  ReturneAnnulationFactureSpx(lstIdCentre, DateDebut, DateFin);
        }
        public List<CsEvenement> ReturneAnnulationFactureTop(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin,int Top)
        {
            return  ReturneAnnulationFactureSpxTop(lstIdCentre, DateDebut, DateFin,Top);
        }
        public List<CsEvenement> ReturneAnnulationFactureSpx(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                string LstIdCentre = DBBase.RetourneStringListeObject(lstIdCentre);
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_LISTE_ANNULATION";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar ,int.MaxValue ).Value = LstIdCentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    var result=Entities.GetEntityListFromQuery<CsEvenement>(dt);
                    using (galadbEntities ctx=new galadbEntities())
                    {
                        var LCLIENT=ctx.LCLIENT.Where(l=>lstIdCentre.Contains( l.FK_IDCENTRE));
                        result.ForEach(e => e.COMMENTAIRE = LCLIENT.FirstOrDefault(c => c.NDOC == e.FACTURE && c.REFEM == e.PERIODE && c.TOP1 == "1").DC);
                    }
                    return result;
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
        public List<CsEvenement> ReturneAnnulationFactureSpxTop(List<int> lstIdCentre, DateTime DateDebut, DateTime DateFin,int Top)
        {
            try
            {
                string LstIdCentre = DBBase.RetourneStringListeObject(lstIdCentre);
                if(Top==1)
                cn = new SqlConnection(Abo07ConnectionString);
                else
                    cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                if(Top==1)
                cmd.CommandText = "SPX_RPT_LISTE_ANNULATION";
                else
                    cmd.CommandText = "SPX_RPT_LISTE_ANNULATION_TOP";

                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar ,int.MaxValue ).Value = LstIdCentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    var result=Entities.GetEntityListFromQuery<CsEvenement>(dt);
                    //using (galadbEntities ctx=new galadbEntities())
                    //{
                    //    #region Mise à jour de sens comptable
                    //    var fac_annul=result.Select(r => r.FACTURE).Distinct();
                    //    var per_annul=result.Select(r => r.PERIODE).Distinct();
                    //    //var LCLIENT = ctx.LCLIENT.Where(l => result.Select(r => new { r.PERIODE, r.FACTURE }).Contains(new { PERIODE = l.REFEM, FACTURE = l.NDOC }));
                    //    var LCLIENT = ctx.LCLIENT.Where(l => fac_annul.Contains(l.NDOC)).Where(l => per_annul.Contains(l.REFEM));
                    //    var TRANSCAISB = ctx.TRANSCAISB.Where(l => fac_annul.Contains(l.NDOC)).Where(l => per_annul.Contains(l.REFEM));
                    //    //var TRANSCAISB = ctx.TRANSCAISB.Where(l => result.Select(r => new { r.PERIODE, r.FACTURE }).Contains(new { PERIODE = l.REFEM, FACTURE = l.NDOC }));
                    //    if (LCLIENT!=null && LCLIENT.Count()>0)
                    //    {
                    //        foreach (var item in LCLIENT)
                    //        {
                    //            var anul = result.FirstOrDefault(an => an.PERIODE == item.REFEM && an.FACTURE == item.NDOC);
                    //            int index = result.IndexOf(anul);
                    //            anul.COMMENTAIRE = item.DC;
                    //            result[index] = anul;
                    //        }
                    //    }
                    //    if (TRANSCAISB != null && TRANSCAISB.Count() > 0)
                    //    {
                    //        foreach (var item in TRANSCAISB)
                    //        {
                    //            var anul = result.FirstOrDefault(an => an.PERIODE == item.REFEM && an.FACTURE == item.NDOC);
                    //            int index = result.IndexOf(anul);
                    //            anul.COMMENTAIRE = item.DC;
                    //            result[index] = anul;
                    //        }
                    //    }
                    //    #endregion
                    //}
                    return result;
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

        public List<CsDetailCampagne> ReturneAvisEmisTotal(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.DETAILCAMPAGNE;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where _LaDetCamp.FK_IDCENTRE == IdCentre
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin

                   select new
                   {
                       _LaDetCamp.IDCOUPURE,
                       _LaDetCamp.CLIENT,
                       _LaDetCamp.CAMPAGNE.MATRICULEPIA,
                       NOMABON = _LaDetCamp.CAMPAGNE.ADMUTILISATEUR.LIBELLE
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsDetailCampagne>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        //public List<CsDetailCampagne> ReturneAvisEmis(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _DETCAMP = context.DETAILCAMPAGNE;
        //            query =
        //          (from _LaDetCamp in _DETCAMP
        //           where _LaDetCamp.FK_IDCENTRE == IdCentre
        //            && _LaDetCamp.DATECREATION >= DateDebut
        //            && _LaDetCamp.DATECREATION <= DateFin
        //           group _LaDetCamp by new
        //           {
        //               NOMABON = _LaDetCamp.CAMPAGNE.ADMUTILISATEUR.LIBELLE,
        //               CLIENT = _LaDetCamp.CLIENT
        //           } into pTemp3
        //           let NOMBRE = pTemp3.Distinct().Count()
        //           select new
        //           {
        //               pTemp3.Key.NOMABON,
        //               pTemp3.Key.CLIENT,
        //               NOMBREAVISEMIS = pTemp3.Count()

        //           }).Distinct();
        //            DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
        //            if (dt != null)
        //                return Galatee.Tools.Utility.GetEntityListFromQuery<CsDetailCampagne>(dt);
        //            else
        //                return null;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<CsDetailCampagne> ReturneAvisCoupe(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.INDEXCAMPAGNE;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where _LaDetCamp.FK_IDCENTRE == IdCentre
                    && (string.IsNullOrEmpty(Matricule) || _LaDetCamp.CAMPAGNE.MATRICULEPIA == Matricule)
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin
                   group _LaDetCamp by new
                   {
                       LIBELLECOUPURE = _LaDetCamp.TYPECOUPURE.LIBELLE,
                       NOMABON = _LaDetCamp.CAMPAGNE.ADMUTILISATEUR.LIBELLE,
                       CLIENT = _LaDetCamp.CLIENT,
                       NOMAGENT = _LaDetCamp.CAMPAGNE.ADMUTILISATEUR.LIBELLE

                   } into pTemp3
                   let NOMBRE = pTemp3.Distinct().Count()
                   select new
                   {
                       pTemp3.Key.NOMAGENT,
                       pTemp3.Key.NOMABON,
                       pTemp3.Key.CLIENT,
                       pTemp3.Key.LIBELLECOUPURE,
                       NOMBREAVISCOUPE = NOMBRE
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsDetailCampagne>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCAMPAGNE> ReturneCampagne(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.CAMPAGNE;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where _LaDetCamp.FK_IDCENTRE == IdCentre
                    && (Matricule == _LaDetCamp.MATRICULEPIA || string.IsNullOrEmpty(Matricule))
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin
                   select new
                   {
                       _LaDetCamp.IDCOUPURE,
                       _LaDetCamp.CENTRE,
                       _LaDetCamp.MONTANT,
                       _LaDetCamp.MATRICULEPIA,
                       _LaDetCamp.PERIODE_RELANCABLE,
                       _LaDetCamp.DATE_EXIGIBILITE,
                       _LaDetCamp.PREMIERE_TOURNEE,
                       _LaDetCamp.DERNIERE_TOURNEE,
                       _LaDetCamp.DEBUT_ORDTOUR,
                       _LaDetCamp.FIN_ORDTOUR,
                       _LaDetCamp.MONTANT_RELANCABLE,
                       _LaDetCamp.DEBUT_CATEGORIE,
                       _LaDetCamp.FIN_CATEGORIE,
                       _LaDetCamp.NOMBRE_CLIENT,
                       _LaDetCamp.NOMBRE_FACTURE,
                       _LaDetCamp.DEBUT_AG,
                       _LaDetCamp.FIN_AG,
                       _LaDetCamp.PK_ID,
                       _LaDetCamp.DATECREATION,
                       _LaDetCamp.DATEMODIFICATION,
                       _LaDetCamp.USERCREATION,
                       _LaDetCamp.USERMODIFICATION,
                       _LaDetCamp.FK_IDCENTRE,
                       _LaDetCamp.FK_IDMATRICULE,
                       AGENTPIA = _LaDetCamp.ADMUTILISATEUR.LIBELLE
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsCAMPAGNE>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDetailCampagne> RetourneClientResilieSuiteCampagne(int IdCentre, string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RPT_RESILIESUITECAMPAGNE";

            cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = IdCentre;
            cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
            cmd.Parameters.Add("@matricule", SqlDbType.VarChar).Value = Matricule;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                //if (reader.Read())
                dt.Load(reader);

                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);

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

        public List<CsLclient> RetourneFactureGenere(List<int> IdCentre, List<string> Periode)
        {
            try
            {
                string idcentre = DBBase.RetourneStringListeObject(IdCentre);
                string lesPeriode = DBBase.RetourneStringListeObject(Periode);

                cn = new SqlConnection(Abo07ConnectionString );
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_FACTURATION_PERIODE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, int.MaxValue).Value = lesPeriode;
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public List<CsLclient> RetourneFactureRecouvre(List<int> IdCentre, List<string> Periode)
        {
            try
            {
                string idcentre = DBBase.RetourneStringListeObject(IdCentre);
                string lesPeriode = DBBase.RetourneStringListeObject(Periode);

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_ENCAISSEMENTPERIODE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.VarChar, int.MaxValue).Value = idcentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar, int.MaxValue).Value = lesPeriode;
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public CsLclient RetourneDernierPeriode(int IdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LCLIENT = context.LCLIENT;
                    query =
                    (from _LeLCLIENT in _LCLIENT
                     where IdCentre == _LeLCLIENT.FK_IDCENTRE
                     orderby _LeLCLIENT.REFEM
                     select new
                     {
                         _LeLCLIENT.REFEM
                     }).OrderByDescending(t => t.REFEM).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt).FirstOrDefault();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCampagneGc> ReturneCampagneSgc(string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.CAMPAGNESGC;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where (Matricule == _LaDetCamp.MATRICULEGESTIONNAIRE || string.IsNullOrEmpty(Matricule))
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin
                   select new
                   {
                       _LaDetCamp.NUMEROCAMPAGNE,
                       _LaDetCamp.REGROUPEMENT,
                       _LaDetCamp.MONTANT,
                       _LaDetCamp.MATRICULEGESTIONNAIRE,
                       _LaDetCamp.MATRICULECREATEURCAMPAGNE,
                       _LaDetCamp.PERIODE,
                       _LaDetCamp.FK_IDREGROUPEMENT,
                       _LaDetCamp.FK_IDMATRICULE,
                       _LaDetCamp.STATUS,
                       _LaDetCamp.PK_ID,
                       _LaDetCamp.DATECREATION,
                       _LaDetCamp.DATEMODIFICATION,
                       _LaDetCamp.USERCREATION,
                       _LaDetCamp.USERMODIFICATION,
                       _LaDetCamp.MONTANTPAYER,
                       NOMAGENT = _LaDetCamp.ADMUTILISATEUR.LIBELLE,
                       LIBELLEREGROUPEMENT = _LaDetCamp.REGROUPEMENT1.NOM
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsCampagneGc>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDetailCampagne> RetourneMontantEncaisseSgc(List<CsCampagneGc> lesCampagnes)
        {
            try
            {
                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
                foreach (CsCampagneGc laCampage in lesCampagnes)
                {
                    lstPaiementCampagne.AddRange(new DBClientAccess().PaiementCampagneSgc(laCampage));
                    lstPaiementCampagne.ForEach(t => t.NOMAGENT = laCampage.NOMAGENT);
                }
                var lstCampDist = (from t in lstPaiementCampagne
                                   group new { t } by new { t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT, t.NOMABON, t.NOMAGENT } into pResult
                                   select new
                                   {
                                       pResult.Key.CENTRE,
                                       pResult.Key.CLIENT,
                                       pResult.Key.ORDRE,
                                       pResult.Key.NOMABON,
                                       pResult.Key.NOMAGENT,
                                       pResult.Key.FK_IDCLIENT,
                                       MONTANTEREGLE = pResult.Sum(y => y.t.MONTANTEREGLE),
                                       MONTANTFRAIS = pResult.Sum(y => y.t.MONTANTFRAIS),
                                   });

                //List<CsDetailCampagne> lstFriasCampagne = new DBClientAccess().FraisCampagnePayerSgc(lesCampagnes);
                List<CsDetailCampagne> lesPaiementClient = new List<CsDetailCampagne>();
                foreach (var item in lstCampDist)
                {
                    //CsDetailCampagne leFrais = lstFriasCampagne.FirstOrDefault(t => t.FK_IDCLIENT == item.FK_IDCLIENT);
                    CsDetailCampagne leDetCampagne = new CsDetailCampagne();
                    leDetCampagne.CENTRE = item.CENTRE;
                    leDetCampagne.CLIENT = item.CLIENT;
                    leDetCampagne.ORDRE = item.ORDRE;
                    leDetCampagne.NOMABON = item.NOMABON;
                    leDetCampagne.NOMAGENT = item.NOMAGENT;
                    leDetCampagne.MONTANTEREGLE = item.MONTANTEREGLE;
                    leDetCampagne.MONTANTFRAIS = item.MONTANTFRAIS; ;
                    //leDetCampagne.DATEREGLEMENT = leFrais != null ? leFrais.DATEREGLEMENT : null;
                    lesPaiementClient.Add(leDetCampagne);
                }
                return lesPaiementClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsDetailCampagne> RetourneMontantEncaisseFraisSgc(List<CsCampagneGc> lesCampagnes)
        {
            try
            {
                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
                lstPaiementCampagne.AddRange(new DBClientAccess().FraisCampagnePayerSgc(lesCampagnes));
                var lstCampDist = (from t in lstPaiementCampagne
                                   group new { t } by new { t.FK_IDCENTRE, t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT, t.NOMABON, t.IDCOUPURE } into pResult
                                   select new
                                   {
                                       pResult.Key.FK_IDCENTRE,
                                       pResult.Key.CENTRE,
                                       pResult.Key.CLIENT,
                                       pResult.Key.ORDRE,
                                       pResult.Key.IDCOUPURE,
                                       pResult.Key.NOMABON,
                                       pResult.Key.FK_IDCLIENT,
                                       MONTANTFRAIS = pResult.Sum(y => y.t.MONTANT),

                                   });

                List<CsDetailCampagne> lesPaiementClient = new List<CsDetailCampagne>();
                foreach (var item in lstCampDist)
                {
                    CsDetailCampagne leDetCampagne = new CsDetailCampagne();
                    leDetCampagne.CENTRE = item.CENTRE;
                    leDetCampagne.CLIENT = item.CLIENT;
                    leDetCampagne.ORDRE = item.ORDRE;
                    leDetCampagne.NOMABON = item.NOMABON;
                    leDetCampagne.NOMAGENT = lesCampagnes.FirstOrDefault(t => t.NUMEROCAMPAGNE == item.IDCOUPURE).NOMAGENT;
                    leDetCampagne.MONTANTFRAIS = item.MONTANTFRAIS;
                    lesPaiementClient.Add(leDetCampagne);
                }
                return lesPaiementClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CsUtilisateur> RetourneGestionnaire()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Gest = context.AFFECTATIONGESTIONAIRE;
                    query =
                  (from x in _Gest
                   select new
                   {
                       x.ADMUTILISATEUR.MATRICULE,
                       x.ADMUTILISATEUR.LIBELLE,
                       x.ADMUTILISATEUR.PK_ID,
                       CODE = x.ADMUTILISATEUR.MATRICULE
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsUtilisateur>(dt);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsDetailCampagne> RetournePreavisSgc(List<CsCampagneGc> lesCampagnes)
        {
            try
            {
                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
                foreach (CsCampagneGc laCampage in lesCampagnes)
                {
                    lstPaiementCampagne.AddRange(new DBClientAccess().PreavisSgc(laCampage));
                    lstPaiementCampagne.ForEach(t => t.NOMAGENT = laCampage.NOMAGENT);
                }
                var lstCampDist = (from t in lstPaiementCampagne
                                   group new { t } by new { t.CENTRE, t.CLIENT, t.ORDRE, t.FK_IDCLIENT, t.NOMABON, t.NOMAGENT } into pResult
                                   select new
                                   {
                                       pResult.Key.CENTRE,
                                       pResult.Key.CLIENT,
                                       pResult.Key.ORDRE,
                                       pResult.Key.NOMABON,
                                       pResult.Key.NOMAGENT,
                                       pResult.Key.FK_IDCLIENT,
                                       MONTANTFRAIS = pResult.Sum(y => y.t.MONTANTFRAIS),
                                   });

                List<CsDetailCampagne> lesPaiementClient = new List<CsDetailCampagne>();
                foreach (var item in lstCampDist)
                {
                    CsDetailCampagne leDetCampagne = new CsDetailCampagne();
                    leDetCampagne.CENTRE = item.CENTRE;
                    leDetCampagne.CLIENT = item.CLIENT;
                    leDetCampagne.ORDRE = item.ORDRE;
                    leDetCampagne.NOMABON = item.NOMABON;
                    leDetCampagne.NOMAGENT = item.NOMAGENT;
                    leDetCampagne.MONTANTFRAIS = item.MONTANTFRAIS; ;
                    lesPaiementClient.Add(leDetCampagne);
                }
                return lesPaiementClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsCampagneGc> ReturneCampagneMandatSgc(string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.CAMPAGNEGC;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where (Matricule == _LaDetCamp.MATRICULEGESTIONNAIRE || string.IsNullOrEmpty(Matricule))
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin
                   select new
                   {
                       _LaDetCamp.NUMEROCAMPAGNE,
                       _LaDetCamp.REGROUPEMENT,
                       _LaDetCamp.MONTANT,
                       _LaDetCamp.MATRICULEGESTIONNAIRE,
                       _LaDetCamp.MATRICULECREATEURCAMPAGNE,
                       _LaDetCamp.PERIODE,
                       _LaDetCamp.FK_IDREGROUPEMENT,
                       _LaDetCamp.FK_IDMATRICULE,
                       _LaDetCamp.STATUS,
                       _LaDetCamp.PK_ID,
                       _LaDetCamp.DATECREATION,
                       _LaDetCamp.DATEMODIFICATION,
                       _LaDetCamp.USERCREATION,
                       _LaDetCamp.USERMODIFICATION,
                       _LaDetCamp.MONTANTPAYER,
                       NOMAGENT = _LaDetCamp.ADMUTILISATEUR.LIBELLE,
                       LIBELLEREGROUPEMENT = _LaDetCamp.REGROUPEMENT1.NOM
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsCampagneGc>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CsDetailCampagne> RetournePaiementMandatementSgc(List<CsMandatementGc> lesCampagnes)
        {
            try
            {
                List<CsDetailCampagne> lstPaiementCampagne = new List<CsDetailCampagne>();
                foreach (CsMandatementGc laCampage in lesCampagnes)
                {
                    lstPaiementCampagne.AddRange(new DBClientAccess().PaiementMandatementSg(laCampage));
                    lstPaiementCampagne.ForEach(t => t.NOMAGENT = laCampage.NOMGESTIONNAIRE);
                }
                return lstPaiementCampagne;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CsMandatementGc> ReturneMandatatementSgc(string Matricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DETCAMP = context.MANDATEMENTGC;
                    query =
                  (from _LaDetCamp in _DETCAMP
                   where (Matricule == _LaDetCamp.CAMPAGNEGC.MATRICULEGESTIONNAIRE || string.IsNullOrEmpty(Matricule))
                    && _LaDetCamp.DATECREATION >= DateDebut
                    && _LaDetCamp.DATECREATION <= DateFin
                   select new
                   {
                       _LaDetCamp.MONTANT,
                       _LaDetCamp.NUMEROMANDATEMENT,
                       _LaDetCamp.FK_IDCAMPAGNA,
                       _LaDetCamp.PK_ID,
                       NOMGESTIONNAIRE = _LaDetCamp.CAMPAGNEGC.ADMUTILISATEUR.LIBELLE
                   }).Distinct();
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    if (dt != null)
                        return Galatee.Tools.Utility.GetEntityListFromQuery<CsMandatementGc>(dt);
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsMandatementGc> RetourneMandatementTaux(List<string> Periode, string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LMandatement = context.MANDATEMENTGC;
                    query =
                    from _LeMandat in _LMandatement
                    where
                        //Periode.Contains(_LeMandat.DATECREATION.Year.ToString() + _LeMandat.DATECREATION.Month.ToString("00"))
                        //&& 
                    (_LeMandat.CAMPAGNEGC.MATRICULEGESTIONNAIRE == Matricule || string.IsNullOrEmpty(Matricule))
                    group _LeMandat by new
                    {
                        NOMGESTIONNAIRE = _LeMandat.CAMPAGNEGC.ADMUTILISATEUR.LIBELLE,
                    } into pTemp3
                    let MONTANT = pTemp3.Sum(x => x.MONTANT)
                    select new
                    {
                        pTemp3.Key.NOMGESTIONNAIRE,
                        MONTANT
                    };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsMandatementGc>(dt).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsMandatementGc> RetourneMandatementTotal(List<string> Periode, string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LMandatement = context.MANDATEMENTGC;
                    query =
                    from _LeMandat in _LMandatement
                    where
                        //Periode.Contains(_LeMandat.DATECREATION.Year.ToString() + _LeMandat.DATECREATION.Month.ToString("00"))
                        //&& 
                    (_LeMandat.CAMPAGNEGC.MATRICULEGESTIONNAIRE == Matricule || string.IsNullOrEmpty(Matricule))

                    select new
                    {
                        _LeMandat.MONTANT
                    };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsMandatementGc>(dt).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsMandatementGc> RetourneTauxPaiementTotal(List<string> Periode, string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LMandatement = context.PAIEMENTCAMPAGNEGC;
                    query =
                    from _LeMandat in _LMandatement
                    where
                    (_LeMandat.MANDATEMENTGC.CAMPAGNEGC.MATRICULEGESTIONNAIRE == Matricule || string.IsNullOrEmpty(Matricule))
                    select new
                    {
                        _LeMandat.MONTANT
                    };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsMandatementGc>(dt).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsMandatementGc> RetourneTauxPaiement(List<string> Periode, string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _LMandatement = context.PAIEMENTCAMPAGNEGC;
                    query =
                    from _LeMandat in _LMandatement
                    where
                        //Periode.Contains(_LeMandat.DATECREATION.Year.ToString() + _LeMandat.DATECREATION.Month.ToString("00"))
                        //&&
                    (_LeMandat.MANDATEMENTGC.CAMPAGNEGC.MATRICULEGESTIONNAIRE == Matricule || string.IsNullOrEmpty(Matricule))
                    group _LeMandat by new
                    {
                        NOMGESTIONNAIRE = _LeMandat.MANDATEMENTGC.CAMPAGNEGC.ADMUTILISATEUR.LIBELLE,
                        //PERIODE = _LeMandat.DATECREATION.Year.ToString() + _LeMandat.DATECREATION.Month.ToString("00")
                    } into pTemp3
                    let MONTANT = pTemp3.Sum(x => x.MONTANT)
                    select new
                    {
                        pTemp3.Key.NOMGESTIONNAIRE,
                        //pTemp3.Key.PERIODE,
                        MONTANT
                    };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsMandatementGc>(dt).ToList();
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> ReturneEmissionProduitRegroupement(List<int> IdRegroupement, string PeriodeDebut, string periodefin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            if (IdRegroupement != null)
            {
                foreach (int item in IdRegroupement)
                    lesEmission.AddRange(ReturneEmissionProduitSpx(item, PeriodeDebut, periodefin));
            }
            else
                lesEmission.AddRange(ReturneEmissionProduitSpx(null, PeriodeDebut, periodefin));

            return lesEmission;
        }
        public List<CsLclient> ReturneEmissionProduitSpx(int? IdRegroupement, string PeriodeDebut, string periodefin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_EMISSIONREGROUPEMENT";
                cmd.Parameters.Add("@IdRegroupement", SqlDbType.Int).Value = IdRegroupement;
                cmd.Parameters.Add("@PeriodeDebut", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PeriodeDebut) ? null : PeriodeDebut;
                cmd.Parameters.Add("@PeriodeFin", SqlDbType.VarChar).Value = string.IsNullOrEmpty(periodefin) ? null : periodefin;
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
                    //return null;
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

        public List<CsLclient> ReturneAvanceSurCommation(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneAvanceSurCommationSpx(item, DateDebut, DateFin));
            return lesEmission.Where(t => t.MONTANT != 0).ToList();
        }
        public List<CsLclient> ReturneAvanceSurCommationSpx(int Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_AVANCEPARCENTRE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

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
                    //return null;
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

        public List<CsLclient> ReturneEncaissementReversement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneEncaissementReversementSpx(item, DateDebut, DateFin));
            return lesEmission.Where(t => t.MONTANT != 0).ToList();
        }
        public List<CsLclient> ReturneEncaissementReversementSpx(int Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_ENCAISSEMTREVERSEMENT";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

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
                    //return null;
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


        public List<CsLclient> ReturneEncaissementModePaiement(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneEncaissementModePaiementSpx(item, DateDebut, DateFin));
            return lesEmission.Where(t => t.MONTANT != 0).ToList();
        }
        public List<CsLclient> ReturneEncaissementModePaiementSpx(int Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_ENCAISSEMTMODESOURCE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

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
                    //return null;
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


        public List<CsStatFactRecap> ReturneVente(List<int> Idcentre, string PeriodeDebut, string periodefin)
        {
            List<CsStatFactRecap> lesEmission = new List<CsStatFactRecap>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneVenteSpx(item, PeriodeDebut, periodefin));
            return lesEmission;
        }
        public List<CsStatFactRecap> ReturneVenteSpx(int Idcentre, string PeriodeDebut, string periodefin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_VENTE";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PeriodeDebut) ? null : PeriodeDebut;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsStatFactRecap>(dt);

                }
                catch (Exception ex)
                {
                    //return null;
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


        public List<CsLclient> ReturneClientPrepayeSansAchatPeriode(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneClientPrepayeSansAchatPeriodeSpx(item, DateDebut, DateFin));
            return lesEmission;
        }
        public List<CsLclient> ReturneClientPrepayeSansAchatPeriodeSpx(int Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_PREPAID_CLIENTSANSACHAT";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

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
                    //return null;
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

        public List<CsLclient> ReturneClientPrepayeJamaisAchat(List<int> Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            List<CsLclient> lesEmission = new List<CsLclient>();
            foreach (int item in Idcentre)
                lesEmission.AddRange(ReturneClientPrepayeJamaisAchatSpx(item, DateDebut, DateFin));
            return lesEmission;
        }
        public List<CsLclient> ReturneClientPrepayeJamaisAchatSpx(int Idcentre, DateTime? DateDebut, DateTime? DateFin)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_PREPAID_CLIENTJAMISACHAT";
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;

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
                    //return null;
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

        public List<CsComptabilisation> ReturneCompabilisationRecap(List<int> Idcentre, string periode, bool IsGrouper)
        {
            try
            {
                List<CsComptabilisation> lstFacture = new List<CsComptabilisation>();
                foreach (int item in Idcentre)
                {
                    lstFacture.AddRange(ReturneCompabilisationRecapEntfac(item, periode));
                    lstFacture.AddRange(ReturneCompabilisationRecapRedfac(item, periode));
                }
                List<CsCompteSpecifique> ListeCompteSpecifique = new DbInterfaceComptable().RetourneCompteSpecifique();
                List<CsOperationComptable> ListeOperationComptable = new DbInterfaceComptable().RetourneOperationComptable();
                List<CsOperationComptable> LstOperationCompableSelect = ListeOperationComptable.Where(t => t.CODE == "03").ToList();
                List<CsCentre> lstCentre = new DBAccueil().ChargerLesDonneesDesSite(false);
                List<CsTypeCompte> ListeTypeCompte = new DbInterfaceComptable().RetourneTypeCompte();
                List<CsCentreCompte> ListeCentreParametrage = new DbInterfaceComptable().RetourneParamCentre();
                List<CsCategorieClient> LstCategorie = new DBAccueil().RetourneCategorie();
                List<CsRedevance> ListeRedevance = new DBCalcul().ChargerRedevance();

                string LeSite = string.Empty;
                List<CsComptabilisation> lstFactureSite = new List<CsComptabilisation>();
                foreach (CsOperationComptable OperationComptat in LstOperationCompableSelect)
                {
                    var lstFactureDistnctCentre = lstFacture.Select(t => new { t.FK_IDCENTRE, t.CENTRE }).Distinct().ToList();
                    foreach (var lesCentre in lstFactureDistnctCentre)
                    {
                        CsCentre leCentre = lstCentre.FirstOrDefault(y => y.PK_ID == lesCentre.FK_IDCENTRE);
                        LeSite = leCentre.LIBELLESITE;
                        List<int> lstCoperCompte = new List<int>();
                        List<CsCompteSpecifique> lstCompteSpecifiqueOperation = ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).ToList();
                        lstCoperCompte.AddRange(ListeCompteSpecifique.Where(t => t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID).Select(t => t.FK_IDTYPE_COMPTE.Value).Distinct().ToList());
                        List<CsTypeCompte> lstTypeCompte = ListeTypeCompte.Where(t => lstCoperCompte.Contains(t.PK_ID)).ToList();
                        List<CsComptabilisation> lesFactureOperation = lstFacture.Where(t => t.FK_IDCENTRE == lesCentre.FK_IDCENTRE).ToList();
                        CsCentreCompte leCentreCaisse = ListeCentreParametrage.FirstOrDefault(t => t.CODECENTRE == lesCentre.CENTRE && t.DC == "V");
                        //List<CsRedevance> ListeRedevance = new DBCalcul().ChargerRedevance();

                        var lstFactureDistnctCategorie = lstFacture.Select(t => new { t.CATEGORIE }).Distinct().ToList();

                        foreach (var categ in lstFactureDistnctCategorie)
                        {
                            List<CsComptabilisation> lstFactureCategorie = lesFactureOperation.Where(t => t.CATEGORIE == categ.CATEGORIE).ToList();
                            foreach (CsTypeCompte TypeCompte in lstTypeCompte)
                            {
                                string SousCompte = "000000";
                                string NumeroPiece = string.Empty;
                                if (TypeCompte.SOUSCOMPTE == "CENTRE")
                                    SousCompte = leCentre.TYPECENTRE == "AG" ? "TAG" + leCentre.CODE : "TCI" + leCentre.CODE;

                                List<CsCompteSpecifique> lesCompte = new List<CsCompteSpecifique>();
                                if (TypeCompte.AVECFILTRE == false)
                                {
                                    #region SANSFILTRE
                                    CsCompteSpecifique leCompteSpe = ListeCompteSpecifique.FirstOrDefault(t => t.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && t.FK_IDOPERATIONCOMPTA == OperationComptat.PK_ID);
                                    if (leCompteSpe != null)
                                    {
                                        CsComptabilisation LigneComptable = new CsComptabilisation();
                                        string TypeValeur = leCompteSpe.VALEURMONTANT;
                                        string caseSwitch = TypeValeur;
                                        switch (caseSwitch)
                                        {
                                            case "MONTANTTTC":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTTC);
                                                }
                                                break;
                                            case "MONTANTHT":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTHT);
                                                }
                                                break;
                                            case "MONTANTTVA":
                                                {
                                                    if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                    else
                                                        LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPER == leCompteSpe.COPERASSOCIE).Sum(t => t.MONTANTTAXE);
                                                }
                                                break;
                                        }
                                        if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                            LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                        LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                        LigneComptable.COMPTEANNEXE1 = SousCompte;
                                        LigneComptable.CENTRE = lesCentre.CENTRE;
                                        LigneComptable.CATEGORIE = categ.CATEGORIE;
                                        LigneComptable.REDEVANCE = categ.CATEGORIE + LstCategorie.FirstOrDefault(o => o.CODE == categ.CATEGORIE).LIBELLE;


                                        LigneComptable.DC = leCompteSpe.DC;
                                        if (LigneComptable.DC == Enumere.Debit)
                                            LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                        else
                                            LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;
                                        if (LigneComptable.MONTANT != 0)
                                            lstFactureSite.Add(LigneComptable);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region CATEGORIE
                                    if (TypeCompte.TABLEFILTRE == "CATEGORIE" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {

                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                        if (leCompteSpe != null)
                                        {
                                            CsComptabilisation LigneComptable = new CsComptabilisation();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPER == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.COPERINITAL == OperationComptat.CODE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.CATEGORIE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                            LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.COMPTEANNEXE1 = SousCompte;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                            else
                                                LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;

                                            LigneComptable.CENTRE = lesCentre.CENTRE;
                                            LigneComptable.CATEGORIE = categ.CATEGORIE;
                                            if (LigneComptable.COMPTGENE.Contains("443"))
                                                LigneComptable.REDEVANCE = "T04 TVA 18% Electricit.";
                                            else
                                                LigneComptable.REDEVANCE = categ.CATEGORIE + LstCategorie.FirstOrDefault(o => o.CODE == categ.CATEGORIE).LIBELLE;
                                            if (LigneComptable.MONTANT != 0)
                                                lstFactureSite.Add(LigneComptable);
                                        }

                                    }
                                    #endregion
                                    #region PRODUIT
                                    if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctProduit = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                        foreach (var Produit in lstFactureDistnctProduit)
                                        {
                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                            if (leCompteSpe != null)
                                            {
                                                CsComptabilisation LigneComptable = new CsComptabilisation();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.PRODUIT == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.COMPTEANNEXE1 = SousCompte;
                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                                else
                                                    LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;
                                                if (LigneComptable.MONTANT != 0)
                                                    lstFactureSite.Add(LigneComptable);

                                            }
                                        }
                                    }
                                    #endregion
                                    #region REDEVANCE
                                    if (TypeCompte.TABLEFILTRE == "REDEVANCE" &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE1) &&
                                        string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                    {
                                        var lstFactureDistnctRedevance = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                        foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                        {
                                            List<string> Code = new List<string>();
                                            Code.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE).ToString());
                                            var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();
                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID && c.LSTVALEURFILTRE.Contains(VALEURFILTRE));
                                            if (leCompteSpe != null)
                                            {
                                                CsComptabilisation LigneComptable = new CsComptabilisation();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPERINITAL == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE && u.COPER == leCompteSpe.COPERASSOCIE && u.REDEVANCE == VALEURFILTRE).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;

                                                LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.COMPTEANNEXE1 = SousCompte;

                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                                else
                                                    LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;
                                                LigneComptable.CENTRE = lesCentre.CENTRE;
                                                LigneComptable.CATEGORIE = categ.CATEGORIE;
                                                LigneComptable.REDEVANCE = "R" + Redevance.REDEVANCE + ListeRedevance.FirstOrDefault(o => o.CODE == Redevance.REDEVANCE).LIBELLE;
                                                if (LigneComptable.MONTANT != 0)
                                                    lstFactureSite.Add(LigneComptable);
                                            }
                                        }
                                    }
                                }
                                    #endregion
                                #region PRODUIT & CATEGORIE
                                if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                    TypeCompte.TABLEFILTRE1 == "CATEGORIE" &&
                                    string.IsNullOrEmpty(TypeCompte.TABLEFILTRE2))
                                {
                                    var lstFactureDistnctProduit = lstFactureCategorie.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                    foreach (var Produit in lstFactureDistnctProduit)
                                    {

                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();


                                        List<string> Code1 = new List<string>();
                                        Code1.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE1).ToString());
                                        var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();
                                        var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                                    && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                                    && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1));
                                        if (leCompteSpe != null)
                                        {
                                            CsComptabilisation LigneComptable = new CsComptabilisation();
                                            string TypeValeur = leCompteSpe.VALEURMONTANT;
                                            string caseSwitch = TypeValeur;
                                            switch (caseSwitch)
                                            {
                                                case "MONTANTTTC":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                        u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                        u.CATEGORIE == VALEURFILTRE1
                                                                                                        ).Sum(t => t.MONTANTTTC);

                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                            u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                            u.PRODUIT == VALEURFILTRE &&
                                                                                                            u.CATEGORIE == VALEURFILTRE1
                                                                                                            ).Sum(t => t.MONTANTTTC);
                                                    }
                                                    break;
                                                case "MONTANTHT":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                        u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                        u.PRODUIT == VALEURFILTRE &&
                                                                                                        u.CATEGORIE == VALEURFILTRE1
                                                                                                        ).Sum(t => t.MONTANTHT);

                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                             u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                             u.PRODUIT == VALEURFILTRE &&
                                                                                                             u.DC == TypeCompte.DC &&
                                                                                                             u.CATEGORIE == VALEURFILTRE1
                                                                                                             ).Sum(t => t.MONTANTHT);
                                                    }
                                                    break;
                                                case "MONTANTTVA":
                                                    {
                                                        if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                    u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                    u.PRODUIT == VALEURFILTRE &&
                                                                                                    u.CATEGORIE == VALEURFILTRE1
                                                                                                    ).Sum(t => t.MONTANTTAXE);
                                                        else
                                                            LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                            u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                            u.PRODUIT == VALEURFILTRE &&
                                                                                                            u.CATEGORIE == VALEURFILTRE1 &&
                                                                                                            u.DC == TypeCompte.DC
                                                                                                            ).Sum(t => t.MONTANTTAXE);
                                                    }
                                                    break;
                                            }
                                            if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                            LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                            LigneComptable.COMPTEANNEXE1 = SousCompte;
                                            LigneComptable.CENTRE = lesCentre.CENTRE;
                                            LigneComptable.CATEGORIE = categ.CATEGORIE;

                                            LigneComptable.DC = leCompteSpe.DC;
                                            if (LigneComptable.DC == Enumere.Debit)
                                                LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                            else
                                                LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;
                                            LigneComptable.CENTRE = lesCentre.CENTRE;
                                            LigneComptable.CATEGORIE = categ.CATEGORIE;
                                            if (LigneComptable.MONTANT != 0)
                                                lstFactureSite.Add(LigneComptable);
                                        }
                                    }

                                }

                                #endregion
                                #region PRODUIT & REDEVANCE & CATEGORIE
                                if (TypeCompte.TABLEFILTRE == "PRODUIT" &&
                                    TypeCompte.TABLEFILTRE1 == "REDEVANCE" &&
                                    TypeCompte.TABLEFILTRE2 == "CATEGORIE")
                                {
                                    var lstFactureDistnctProduit = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.PRODUIT }).Distinct().ToList();
                                    foreach (var Produit in lstFactureDistnctProduit)
                                    {

                                        List<string> Code = new List<string>();
                                        Code.Add(GetPropValue(Produit, TypeCompte.TABLEFILTRE).ToString());
                                        var VALEURFILTRE = String.Join(" ", Code.ToArray()).Trim();

                                        var lstFactureDistnctRedevance = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE).Select(t => new { t.REDEVANCE }).Distinct().ToList();
                                        foreach (var Redevance in lstFactureDistnctRedevance.Where(t => !string.IsNullOrEmpty(t.REDEVANCE)).ToList())
                                        {
                                            List<string> Code1 = new List<string>();
                                            Code1.Add(GetPropValue(Redevance, TypeCompte.TABLEFILTRE1).ToString());
                                            var VALEURFILTRE1 = String.Join(" ", Code1.ToArray()).Trim();

                                            List<string> Code2 = new List<string>();
                                            Code2.Add(GetPropValue(categ, TypeCompte.TABLEFILTRE2).ToString());
                                            var VALEURFILTRE2 = String.Join(" ", Code2.ToArray()).Trim();

                                            var leCompteSpe = lstCompteSpecifiqueOperation.FirstOrDefault(c => c.FK_IDTYPE_COMPTE == TypeCompte.PK_ID
                                                                                              && c.LSTVALEURFILTRE.Contains(VALEURFILTRE)
                                                                                              && c.LSTVALEURFILTRE1.Contains(VALEURFILTRE1)
                                                                                              && c.LSTVALEURFILTRE2.Contains(VALEURFILTRE2)
                                                                                              );
                                            if (leCompteSpe != null)
                                            {
                                                CsComptabilisation LigneComptable = new CsComptabilisation();
                                                string TypeValeur = leCompteSpe.VALEURMONTANT;
                                                string caseSwitch = TypeValeur;
                                                switch (caseSwitch)
                                                {
                                                    case "MONTANTTTC":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                               u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                               u.PRODUIT == VALEURFILTRE &&
                                                                                                               u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                               u.CATEGORIE == VALEURFILTRE2
                                                                                                               ).Sum(t => t.MONTANTTTC);
                                                            else
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                    //u.COPER  == leCompteSpe.COPERASSOCIE &&
                                                                                                          u.PRODUIT == VALEURFILTRE &&
                                                                                                          u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                          u.CATEGORIE == VALEURFILTRE2
                                                                                                          ).Sum(t => t.MONTANTTTC);
                                                        }
                                                        break;
                                                    case "MONTANTHT":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                    //u.COPERINITAL == leCompteSpe.COPERASSOCIE && 
                                                                                                               u.PRODUIT == VALEURFILTRE &&
                                                                                                               u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                               u.CATEGORIE == VALEURFILTRE2
                                                                                                               ).Sum(t => t.MONTANTHT);
                                                            else
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                    //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                u.PRODUIT == VALEURFILTRE &&
                                                                                                                u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                u.CATEGORIE == VALEURFILTRE2
                                                                                                                ).Sum(t => t.MONTANTHT);
                                                        }
                                                        break;
                                                    case "MONTANTTVA":
                                                        {
                                                            if (!string.IsNullOrEmpty(TypeCompte.VALEURMONTANT) && TypeCompte.VALEURMONTANT == "TOUS")
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                                                             u.COPERINITAL == leCompteSpe.COPERASSOCIE &&
                                                                                                             u.PRODUIT == VALEURFILTRE &&
                                                                                                             u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                             u.CATEGORIE == VALEURFILTRE2
                                                                                                             ).Sum(t => t.MONTANTTAXE);
                                                            else
                                                                LigneComptable.MONTANT = lstFacture.Where(u => u.FK_IDCENTRE == lesCentre.FK_IDCENTRE &&
                                                                    //u.COPER == leCompteSpe.COPERASSOCIE &&
                                                                                                                u.PRODUIT == VALEURFILTRE &&
                                                                                                                u.REDEVANCE == VALEURFILTRE1 &&
                                                                                                                u.CATEGORIE == VALEURFILTRE2
                                                                                                                ).Sum(t => t.MONTANTTAXE);
                                                        }
                                                        break;
                                                }
                                                if (leCompteSpe.CENTREIMPUTATION == "CENTRE")
                                                {
                                                    if (leCentreCaisse == null)
                                                        throw new Exception(string.Format("Le centre {0} doit être paramétré dans la table COMPTA_CENTRE", lesCentre.CENTRE));

                                                    LigneComptable.CENTREIMPUTATION = leCentreCaisse.CI;
                                                }
                                                LigneComptable.COMPTGENE = leCompteSpe.COMPTE.Trim();
                                                LigneComptable.COMPTEANNEXE1 = SousCompte;
                                                LigneComptable.CENTRE = lesCentre.CENTRE;
                                                LigneComptable.CATEGORIE = categ.CATEGORIE;
                                                LigneComptable.REDEVANCE = "R" + Redevance.REDEVANCE + ListeRedevance.FirstOrDefault(o => o.CODE == Redevance.REDEVANCE).LIBELLE;

                                                LigneComptable.DC = leCompteSpe.DC;
                                                if (LigneComptable.DC == Enumere.Debit)
                                                    LigneComptable.MONTANTDEBIT = LigneComptable.MONTANT;
                                                else
                                                    LigneComptable.MONTANTCREDIT = LigneComptable.MONTANT;
                                                if (LigneComptable.MONTANT != 0)
                                                    lstFactureSite.Add(LigneComptable);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                                #endregion
                    }
                }
                if (IsGrouper)
                {
                    lstFactureSite.ForEach(y => y.CENTRE = "Facturation");
                    lstFactureSite.ForEach(y => y.COPER = "Facturation" + periode);
                }
                lstFactureSite.ForEach(y => y.ORIGINE = LeSite);
                return lstFactureSite;

            }
            catch (Exception ex)
            {
                new ErrorManager().WriteInLogFile(this, ex.Message);
                throw ex;
            }
        }





        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public List<CsComptabilisation> ReturneCompabilisationRecapEntfac(int Idcentre, string periode)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_FACTURE_ENTFAC";
                cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.ForEach(t => t.IsDebit = true);
                    lstEntfact.ForEach(t => t.DC = Enumere.Debit);
                    lstEntfact.ForEach(t => t.COPERINITAL = Enumere.FactureEmissionGeneral);
                    return lstEntfact;
                }
                catch (Exception ex)
                {
                    //return null;
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
        public List<CsComptabilisation> ReturneCompabilisationRecapRedfac(int Idcentre, string periode)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CPT_FACTURE_REDFAC";
                cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                }
                catch (Exception ex)
                {
                    //return null;
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
        public List<CsStatFact> ReturneStatistique(string CodeSite, string periode, string Produit,bool IsStat)
        {
            List<CsStatFact> lesEmission = new List<CsStatFact>();
            if (Produit == Enumere.Electricite)
                lesEmission.AddRange(ReturneStatistiqueBT(CodeSite, periode, IsStat));
            else
                lesEmission.AddRange(ReturneStatistiqueMT(CodeSite, periode, IsStat));
            return lesEmission;
        }
        public List<CsStatFact> ReturneStatistiqueMT(string CodeSite, string periode,bool IsStat)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_STAT_FACTMT";
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@IsStat", SqlDbType.Bit).Value = IsStat;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsStatFact>(dt);
                }
                catch (Exception ex)
                {
                    //return null;
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

        public List<CsStatFact> ReturneStatistiqueBT(string CodeSite, string periode, bool IsStat)
        {
            try
            {
                cn = new SqlConnection(Abo07ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_STAT_FACTBT";
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@IsStat", SqlDbType.Bit).Value = IsStat;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsStatFact>(dt);
                }
                catch (Exception ex)
                {
                    //return null;
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
        public bool ReturneFichierPersonnel(string periode, string CheminImpression)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_PERS_CATEGORIEEDMPERSONNEL";
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsFichierPersonnel> lstFactureAgent = Entities.GetEntityListFromQuery<CsFichierPersonnel>(dt);
                    if (lstFactureAgent != null && lstFactureAgent.Count != 0)
                        return ExportDataPersonnel(lstFactureAgent, CheminImpression, periode);
                    else return false;
                }
                catch (Exception ex)
                {
                    //return false;
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

        public bool ReturneFichierPersonnelDirecteur(string periode, string CheminImpression)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_PERS_CATEGORIEEDMDIRECTEUR";
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsFichierPersonnel> lstFactureAgent = Entities.GetEntityListFromQuery<CsFichierPersonnel>(dt);
                    if (lstFactureAgent != null && lstFactureAgent.Count != 0)
                        return ExportDataDirecteur(lstFactureAgent, CheminImpression, periode);
                    else return false;
                }
                catch (Exception ex)
                {
                    return false;
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

        public bool ExportDataPersonnel(List<CsFichierPersonnel> Fichiere, string CheminFichier, string periode)
        {
            List<CsFichierPersonnel> DetailFichier = Fichiere;
            string leChemin = CheminFichier.Replace('[', '\\');
            string CheminLog = leChemin + "\\FICHIEINTEGRATIONPERSONNEL" + periode + ".FFP";
            System.IO.StreamWriter monstream = new System.IO.StreamWriter(CheminLog, true);
            try
            {
                if (DetailFichier.Count > 0)
                {
                    foreach (CsFichierPersonnel UnImpayee in DetailFichier)
                    {
                        string[] Split = UnImpayee.MONTANT.ToString().Split(',');
                        //string Montant = Split[0].PadLeft(12, '0') + "." + Split[1];
                        string Montant = Split[0].PadLeft(12, '0');
                        monstream.WriteLine("01|" + UnImpayee.MATRICULE.PadLeft(6, '0') + "|" + UnImpayee.PERIODE + "|5053|" + Montant + "|" + UnImpayee.FACTURE);
                    }
                    monstream.Close();
                }
                return true;
            }
            catch (Exception io)
            {
                if (monstream != null)
                    monstream.Close();
                return false;
            }
        }
        public bool ExportDataDirecteur(List<CsFichierPersonnel> Fichiere, string CheminFichier, string periode)
        {
            List<CsFichierPersonnel> DetailFichier = Fichiere;
            string leChemin = CheminFichier.Replace('[', '\\');
            string CheminLog = leChemin + "\\FICHIEINTEGRATIONDIRECTEUR" + periode + ".TXT";
            System.IO.StreamWriter monstream = new System.IO.StreamWriter(CheminLog, true);
            try
            {
                if (DetailFichier.Count > 0)
                {
                    foreach (CsFichierPersonnel UnImpayee in DetailFichier)
                    {
                        monstream.WriteLine(UnImpayee.MATRICULE + "|" + "5053" + "|" + UnImpayee.MONTANT + "|" +
                       "FA" + UnImpayee.FACTURE + "|" + System.DateTime.Today.Year + System.DateTime.Today.Month.ToString("00"));
                    }
                    monstream.Close();
                }
                return true;
            }
            catch (Exception io)
            {
                if (monstream != null)
                    monstream.Close();

                return false;
            }
        }

        public List<CsComptabilisation> ReturneStatiqueDesVente(List<int> Idcentre, string periode, bool IsGrouper)
        {
            List<CsComptabilisation> lstFacture = new List<CsComptabilisation>();
            foreach (int item in Idcentre)
            {
                lstFacture.AddRange(ReturneCompabilisationRecapStatEntfac(item, periode));
                lstFacture.AddRange(ReturneCompabilisationRecapStatRedfac(item, periode));
            }
            return lstFacture;
        }
        public List<CsComptabilisation> ReturneCompabilisationRecapStatEntfac(int Idcentre, string periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_STATFACTURE_ENTFAC";
                cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    List<CsComptabilisation> lstEntfact = Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
                    lstEntfact.ForEach(t => t.IsDebit = true);
                    lstEntfact.ForEach(t => t.DC = Enumere.Debit);
                    lstEntfact.ForEach(t => t.COPERINITAL = Enumere.FactureEmissionGeneral);
                    return lstEntfact;
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
        public List<CsComptabilisation> ReturneCompabilisationRecapStatRedfac(int Idcentre, string periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_STATFACTURE_REDFAC";
                cmd.Parameters.Add("@MoisComptable", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@IdCentre", SqlDbType.Int).Value = Idcentre;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsComptabilisation>(dt);
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


        //public List<CsStatFact> ReturneStatistiqueVente(List<int> Idcentre, string periode, string Produit)
        //{
        //    List<CsStatFact> lesEmission = new List<CsStatFact>();
        //    if (Produit == Enumere.Electricite)
        //    {
        //        foreach (int item in Idcentre)
        //            lesEmission.AddRange(ReturneStatistiqueBTStat(item, periode));
        //    }
        //    else
        //    {
        //        foreach (int item in Idcentre)
        //            lesEmission.AddRange(ReturneStatistiqueMTStat(item, periode));
        //    }
        //    return lesEmission;
        //}

        public List<CsStatFact> ReturneStatistiqueMTStat(int Idcentre, string periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_STAT_FACTMTSTAT";
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
                cmd.Parameters.Add("@idcentre", SqlDbType.Int).Value = Idcentre;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsStatFact>(dt);
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

        public List<CsTranscaisse> ReturneEncaissementParMoisComptable(List<int> lstIdCentre, string Periode)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                string lsIdCentre = DBBase.RetourneStringListeObject(lstIdCentre);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 1800;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_ENCAISSEMENTCUMMULE";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = lsIdCentre;
                cmd.Parameters.Add("@Periode", SqlDbType.VarChar ,6).Value = Periode;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsTranscaisse>(dt);

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
        public List<CsTournee> RetourneTourneeParPIA(string codeSite)
        {
            //cmd.CommandText = "SPX_RPT_Dis_Zone";

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_TOURNEEPIA";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = codeSite;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsTournee>(dt);
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
        public List<CsDetailCampagne> ReturneAvisEmis(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                string Matricule = DBBase.RetourneStringListeObject(IdMatricule);

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_AVISEMIS";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@IdPia", SqlDbType.VarChar, int.MaxValue).Value = Matricule;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public List<CsDetailCampagne> ReturneAvisCoupeType(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                string Matricule = DBBase.RetourneStringListeObject(IdMatricule);

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_AVISCOUPE";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@IdPia", SqlDbType.VarChar, int.MaxValue).Value = Matricule;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

        public List<CsDetailCampagne> ReturneAvisRepose(string CodeSite, List<int> IdMatricule, DateTime DateDebut, DateTime DateFin)
        {
            try
            {
                string Matricule = DBBase.RetourneStringListeObject(IdMatricule);

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_AVISREMIS";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
                cmd.Parameters.Add("@IdPia", SqlDbType.VarChar, int.MaxValue).Value = Matricule;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }



        public List<CsDonnesStatistiqueDemande> RetourneDonnesStatistiqueDemande(string codesite, string codeproduit, string codetypedemande, string periode)
        {
            try
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        //string Matricule = DBBase.RetourneStringListeObject(IdMatricule);

                        SqlParameter pcodeSite = new SqlParameter("@codeSite", codesite);
                        SqlParameter pproduit = new SqlParameter("@produit", codeproduit);
                        SqlParameter ptypedemande = new SqlParameter("@typedemande", codetypedemande);
                        SqlParameter pperiode = new SqlParameter("@periode", periode);
                        var rlt = context.Database.SqlQuery<CsDonnesStatistiqueDemande>("exec SPX_RPT_TRAITEMENT_DEMANDE_CLIENTEL @codeSite,@produit,@typedemande,@periode", pcodeSite, pproduit, ptypedemande, pperiode);
                        var retune= rlt != null ? rlt.ToList() : new List<CsDonnesStatistiqueDemande>();
                        return retune;
                    }
               

                    //cn = new SqlConnection(ConnectionString);
                    //cmd = new SqlCommand();
                    //cmd.Connection = cn;
                    //cmd.CommandTimeout = 18000;
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "SPX_RPT_TRAITEMENT_DEMANDE_CLIENTEL";
                    //cmd.Parameters.Add("@codeSite", SqlDbType.VarChar, 3).Value = codesite;
                    //cmd.Parameters.Add("@produit", SqlDbType.VarChar, 2).Value = codeproduit;
                    //cmd.Parameters.Add("@typedemande",  SqlDbType.VarChar, 2).Value = codetypedemande;
                    //cmd.Parameters.Add("@periode",SqlDbType.VarChar, 2).Value = periode;
                    //DBBase.SetDBNullParametre(cmd.Parameters);
                    //try
                    //{
                    //    if (cn.State == ConnectionState.Closed)
                    //        cn.Open();
                    //    SqlDataReader reader = cmd.ExecuteReader();
                    //    DataTable dt = new DataTable();
                    //    dt.Load(reader);
                    //    return Entities.GetEntityListFromQuery<CsDonnesStatistiqueDemande>(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception(cmd.CommandText + ":" + ex.Message);
                }
                finally
                {
                    //if (cn.State == ConnectionState.Open)
                    //    cn.Close(); // Fermeture de la connection 
                    //cmd.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }


        public List<CsStatistiqueTravaux_Brt_Ext> RetourneStatistiqueTravaux_Brt_Ext(string codeproduit,string periode)
        {
            try
            {
                PRODUIT p = new PRODUIT();
                using (galadbEntities ctx = new galadbEntities())
                {
                    p = ctx.PRODUIT.FirstOrDefault(pr => pr.CODE == codeproduit);
                }
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_TRAVAUXBRANCHEMENTS_EXTENTIONS";
                cmd.Parameters.Add("@produit", SqlDbType.Int).Value = p.PK_ID;
                cmd.Parameters.Add("@periode", SqlDbType.VarChar, 6).Value = periode;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    var result= Entities.GetEntityListFromQuery<CsStatistiqueTravaux_Brt_Ext>(dt);
                   
                        result.ForEach(c => c.ID_PRODUIT = p.PK_ID);
                        result.ForEach(c => c.LIBELLE_PRODUIT = p.LIBELLE);
                    
                    return result;
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
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
        }

    }
}