using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galatee.Entity.Model;
using Galatee.Structure;
using System.Data.SqlClient;

namespace Galatee.DataAccess
{
    public class DBScelle
    {

        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        private string ConnectionString;

        public DBScelle()
        {
            try
            {
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<CsCompteurBta> SelectAllCompteurInDisponible()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    DataTable dt = ScelleProcedures.ReturnecompteursInDisponibles(null, null, string.Empty, string.Empty);
                    //var dc= context.DCANALISATION.Select(d => d.COMPTEUR.NUMERO).ToList();
                    //List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt).Distinct().Where(c => !dc.Contains(c.NUMERO)).ToList();
                    var rslt = Entities.GetEntityListFromQuery<CsCompteurBta>(dt).Distinct();
                    List<CsCompteurBta> l = rslt != null ? rslt.ToList() : new List<CsCompteurBta>();
                    //var MAGASINVIRTUEL = context.MAGASINVIRTUEL.Where(m => !context.DCANALISATION.Select(c => c.FK_IDMAGAZINVIRTUEL).Contains(m.PK_ID)).ToList();

                    //foreach (var item in l)
                    //{
                    //    var c_Mv = MAGASINVIRTUEL.Where(m => m.FK_TBCOMPTEUR == item.PK_ID);
                    //    if (c_Mv!=null && c_Mv.Count()>0)
                    //    {
                    //        var obj = c_Mv.OrderByDescending(o => o.DATECREATION);
                    //        if (obj != null && obj.Count() > 0)
                    //        {
                    //            item.FK_IDCENTRE = obj.First().FK_IDCENTRE;
                    //        } 
                    //    }
                    //}
                    //l.ForEach(c => c.FK_IDCENTRE = context.MAGASINVIRTUEL.Where(m => m.FK_TBCOMPTEUR == c.PK_ID).OrderByDescending(o => o.DATECREATION).First().FK_IDCENTRE);
                    return l;
                }
                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());


                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCompteurBta> SelectAllCompteur()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    DataTable dt = ScelleProcedures.SCELLES_CompteurBta_RETOURNE();
                    List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt).Distinct().ToList();
                    //var MAGASINVIRTUEL = context.MAGASINVIRTUEL.Where(m => !context.DCANALISATION.Select(c => c.FK_IDMAGAZINVIRTUEL).Contains(m.PK_ID)).ToList();

                    //foreach (var item in l)
                    //{
                    //    var c_Mv = MAGASINVIRTUEL.Where(m => m.FK_TBCOMPTEUR == item.PK_ID);
                    //    if (c_Mv!=null && c_Mv.Count()>0)
                    //    {
                    //        var obj = c_Mv.OrderByDescending(o => o.DATECREATION);
                    //        if (obj != null && obj.Count() > 0)
                    //        {
                    //            item.FK_IDCENTRE = obj.First().FK_IDCENTRE;
                    //        } 
                    //    }
                    //}
                    //l.ForEach(c => c.FK_IDCENTRE = context.MAGASINVIRTUEL.Where(m => m.FK_TBCOMPTEUR == c.PK_ID).OrderByDescending(o => o.DATECREATION).First().FK_IDCENTRE);
                    return l;
                }
                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());


                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDscelle> RetourneListeDemandeScelle(int fk_dem)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_SELECT_DSCELLE";
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = fk_dem;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsDscelle>(dt);
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



        public List<CsLotScelle> RetourneListeScelle()
        {
            try
            {
                List<CsLotScelle> _lstScelle = new List<CsLotScelle>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeScelle();
                _lstScelle = Entities.GetEntityListFromQuery<CsLotScelle>(dt);

                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsDetailAffectationScelle> RetourneListeDetailAffectationScelle(int IdDemande)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_SELECT_DETAILSCELLE";
            cmd.Parameters.Add("@IDDEMANDE", SqlDbType.Int).Value = IdDemande;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsDetailAffectationScelle>(dt);
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



        public List<CsCentre> RetourneCentre()
        {
            try
            {
                List<CsCentre> _lstScelle = new List<CsCentre>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneCentre();
                _lstScelle = Entities.GetEntityListFromQuery<CsCentre>(dt);

                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public CsAffectationScelle RetourneListeAffectationScelle(Guid IdAffectationScelle)
        {
            try
            {
                CsAffectationScelle _lstScelle = new CsAffectationScelle();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeAffectationScelle(IdAffectationScelle);
                _lstScelle = Entities.GetEntityFromQuery<CsAffectationScelle>(dt);

                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsAffectationScelle> RetourneListeAffectationScelle(List<Guid> IdAffectationScelle)
        {
            try
            {
                List<CsAffectationScelle> _lstScelle = new List<CsAffectationScelle>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeAffectationScelle(IdAffectationScelle);
                _lstScelle = Entities.GetEntityListFromQuery<CsAffectationScelle>(dt);

                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /*
        public string InsertDemandeScelle(CsDemandeBase lademande, CsDscelle dscelle)
        {
            try
            {
                lademande.NUMDEM = AccueilProcedures.GetNumDevis(lademande);
                DEMANDE demande = Entities.ConvertObject<DEMANDE, CsDemandeBase>(lademande);
                DSCELLE scelle = Entities.ConvertObject<DSCELLE, CsDscelle>(dscelle);
                return Galatee.Entity.Model.ScelleProcedures.InsertDemandeScelle(demande, scelle);
                //return string.Empty ;
            }
            catch (Exception ex)
            {
                return string.Empty;
                throw ex;
            }
        }
         * */




        public string InsertDemandeScelle(CsDemandeBase lademande, CsDscelle dscelle)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

            try
            {
                lademande.NUMDEM = new DBAccueil().NumeroDemande(lademande.FK_IDCENTRE);
                new DBAccueil().InsertOrUpdateDemande(lademande, laCommande);
                dscelle.FK_IDDEMANDE = lademande.PK_ID;
                dscelle.NUMDEM = lademande.NUMDEM;
                new DBAccueil().InsertOrUpdateDscelle(dscelle, laCommande);
                string CodeWkF = lademande.SITE + lademande.CENTRE + DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day + DateTime.Today.Hour +
                                  DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                new DBAccueil().CreeDemande(lademande.PK_ID, lademande.NUMDEM, CodeWkF, laCommande);
                laCommande.Transaction.Commit();
                return lademande.NUMDEM;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();

                new ErrorManager().WriteInLogFile(this, ex.Message);
                return string.Empty;
                throw ex;
            }
        }




        /*
                public bool InsertAffectionScelle(int id_lademande, int IdUser, List<CsLotScelle> LstLotScelle)
                {
                    try
                    {
                        //Chargé les info lié a la demande 
                        var demande = RetourneListeDemandeScelle(id_lademande).FirstOrDefault();

                        //Chargé les anciens en stock 
                        List<CsLotScelle> AncientbLotMagasinGeneral = RetourneListeScelle(demande.FK_IDCENTREFOURNISSEUR);

                        //Chargé l'utilisateur courant 
                        var User = new DBAdmUsers().GetAll().FirstOrDefault(u => u.PK_ID == IdUser);

                        //Convertion des lot en tbLotMagasinGeneral à débité
                        //LstLotScelle.ForEach(l => l.Id_Affectation = null);
                        List<tbLotMagasinGeneral> tbLotMagasinGeneral = Entities.ConvertObject<tbLotMagasinGeneral, CsLotScelle>(LstLotScelle);

                        CsMapperAffectationScelle AffectationScelle = new CsMapperAffectationScelle();
                        List<CsMapperDetailAffectationScelle> Lst_DetailAffectationScelle = new List<CsMapperDetailAffectationScelle>();

                        int NombreLotAffecter = 0;

                        //Mise à jour des date de modification
                        tbLotMagasinGeneral.ForEach(t => t.Date_DerniereModif = DateTime.Now);

                        //Alimentation des infos d'affectation
                        AffectationScelle.Code_Centre_Dest = demande.FK_IDCENTRE;
                        AffectationScelle.Code_Centre_Origine = demande.FK_IDCENTREFOURNISSEUR ;
                        AffectationScelle.Date_Transfert = DateTime.Now;
                        AffectationScelle.Id_Affectation = Guid.NewGuid();
                        AffectationScelle.Id_Modificateur = IdUser;
                        AffectationScelle.Id_Demande = id_lademande;


                        //Pour chaque element de lot à débité
                        foreach (var item in tbLotMagasinGeneral)
                        {
                            //Recupération de l'id du lot
                            string idlot = item.Id_LotMagasinGeneral;

                            //Recuperation de l'ancien numero de depart du lot actuel
                            string Num_Depart = AncientbLotMagasinGeneral.FirstOrDefault(l => l.Id_LotMagasinGeneral == idlot).Numero_depart;

                            //Recuperation de l'ancienne taille du numero de depart du lot actuel
                            var NombrePositionAncienNumDepart = Num_Depart.ToString().Length;

                            #region Sylla 04/04/2016

                            //Recuperation du nouveau nombre de scelle a affecté pour le lot actuel courant en fesant la difference entre le "item.Numero_depart' et l'ancien numero de depart
                            //int NombreScelleAffecterALot =int.Parse(item.Numero_depart) - int.Parse( Num_Depart );

                            // Modification apporté le 04/04/2016 par BSY(Je supose que le nombre de scelles demandé est toujours éguale au nombre de scelles à affecté)
                            int NombreScelleAffecterALot = demande.NOMBRE_DEM;

                            #endregion


                            //Incrementation du nombre de scelle a affecté pour le lot pour avoir le nombre totale en fin de boucle
                            NombreLotAffecter = NombreLotAffecter + NombreScelleAffecterALot;

                            //Génération des détail d'affectation en bouclant a partir du numero de depart de l'ancient lot qui est incrémenté jusko nombre de scelle a affecté
                            //for (int i = int.Parse(Num_Depart); i == int.Parse(Num_Depart) + NombreScelleAffecterALot; i++)
                            for (int i = int.Parse(Num_Depart); i <= int.Parse(Num_Depart) + (NombreScelleAffecterALot - 1); i++)
                            {
                                CsMapperDetailAffectationScelle DetailAffectationScelle = new CsMapperDetailAffectationScelle();

                                DetailAffectationScelle.Centre_Origine_Scelle = item.CodeCentre;
                                DetailAffectationScelle.Date_Reception = DateTime.Now;
                                DetailAffectationScelle.Id_Affectation = AffectationScelle.Id_Affectation;
                                DetailAffectationScelle.LotAppartenance_Id = item.Id_LotMagasinGeneral;
                                DetailAffectationScelle.Id_DetailAffectationScelle = Guid.NewGuid();
                                DetailAffectationScelle.Nuemro_Scelle = i.ToString().PadLeft(NombrePositionAncienNumDepart, '0');
                                DetailAffectationScelle.EstLivre = false;
                                Lst_DetailAffectationScelle.Add(DetailAffectationScelle);
                            }
                        }
                        AffectationScelle.Nbre_Scelles = NombreLotAffecter;
                        AffectationScelle.NumScelleDepart = Lst_DetailAffectationScelle.First().Nuemro_Scelle;
                        AffectationScelle.NumScelleFin = Lst_DetailAffectationScelle.Last().Nuemro_Scelle;

                        return InsertAffectionScelle(AffectationScelle, Lst_DetailAffectationScelle, tbLotMagasinGeneral, demande);
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw ex;
                    }
                }
                public  bool InsertAffectionScelle(CsMapperAffectationScelle  AffectationScelle, List<CsMapperDetailAffectationScelle > Lst_DetailAffectationScelle, List<tbLotMagasinGeneral> Lst_Lot, CsDscelle laDemande)
                {
                     cmd = new SqlCommand();
                  try
                  {
                        cn = new SqlConnection(ConnectionString);
                        cmd.Connection = cn;
                        cmd.CommandTimeout = 3000;
                        cmd.Connection.Open();
                        cmd.Transaction = cmd.Connection.BeginTransaction();

                        List<CsMapperAffectationScelle> lstAffectation = new List<CsMapperAffectationScelle>();
                        DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(Lst_DetailAffectationScelle);
                        Galatee.Tools.Utility.BulkInsertTable("scelle.DetailAffectationScelle",TablePere, cmd.Connection, cmd.Transaction);

                        lstAffectation.Add(AffectationScelle);
                        DataTable Table = Galatee.Tools.Utility.ListToDataTable(lstAffectation);
                        Galatee.Tools.Utility.BulkInsertTable("scelle.AffectationScelle", Table, cmd.Connection, cmd.Transaction);

                        using (galadbEntities ctontext = new galadbEntities())
                        {

                            if (laDemande.FK_IDCENTREFOURNISSEUR  == 1)
                            {
                                Lst_Lot.ForEach(c => c.Id_Affectation = null);
                                Entities.UpdateEntity<tbLotMagasinGeneral>(Lst_Lot);
                            }
                            else
                            {
                                List<tbLot> LsttbLot = new List<tbLot>();
                                foreach (var item in Lst_Lot)
                                {
                                    tbLot tbLot = new tbLot();
                                    tbLot.agence_centre_Appartenance = laDemande.FK_IDCENTRE ;
                                    //Le centre d'origine est mit est le mm que le centre du lot pour le moment
                                    tbLot.agence_centre_Origine = laDemande.FK_IDCENTREFOURNISSEUR ;
                                    tbLot.CENTRE = item.CENTRE;
                                    tbLot.Date_Derniere_Modif = DateTime.Now;
                                    tbLot.DateReception = item.DateReception;
                                    tbLot.lot_Couleur_ID = item.Couleur_ID;
                                    tbLot.lot_ID = item.Id_LotMagasinGeneral;
                                    tbLot.Matricule_AgentModification = item.Matricule_AgentDerniereModif;
                                    tbLot.Matricule_Creation = item.Matricule_AgentReception;
                                    tbLot.Nombre_scelles_lot = item.Nbre_Scelles.Value;
                                    tbLot.Nombre_scelles_reçu = item.Nbre_Scelles;
                                    tbLot.Numero_depart = item.Numero_depart;
                                    tbLot.Numero_fin = item.Numero_fin;
                                    tbLot.Origine_ID = item.Origine_ID;
                                    tbLot.Status_lot_ID = 1;

                                    LsttbLot.Add(tbLot);
                                }

                                Entities.UpdateEntity<tbLot>(LsttbLot);
                            }

                            ctontext.SaveChanges();
                            cmd.Transaction.Commit();
                        }
                        return true;
                    }
                    catch (Exception  ex)
                    {
                        cmd.Transaction.Rollback();
                        throw ex;
                    }
                }
        */
        //public bool InsertAffectionScelle(int id_lademande, int IdUser, List<CsLotScelle> LstLotScelle)
        //{

        //    try
        //    {
        //        CsDscelle  demande = RetourneListeDemandeScelle(id_lademande).FirstOrDefault();
        //        int NombreTotal = demande.NOMBRE_DEM;
        //        int? nbrAInsere = null;
        //        int nbrInsere = 0;
        //        foreach (CsLotScelle item in LstLotScelle)
        //        {
        //           nbrInsere = InsertAffectionScelleSPX(id_lademande, IdUser, item.Id_LotMagasinGeneral, int.Parse(item.Numero_depart), int.Parse(item.Numero_fin),nbrAInsere,item.Nbre_Scelles );
        //           if (nbrAInsere < NombreTotal)
        //               nbrAInsere = NombreTotal - nbrInsere;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        //public int InsertAffectionScelleSPX(int laDemande, int IdUser, string  IdLot,int NumDepart,int NumFin,int? NombreAInserer,int NombreRestant)
        //{
        //    int resultat = -1;
        //    try
        //    {
        //        cn = new SqlConnection(ConnectionString);

        //        cmd = new SqlCommand();
        //        cmd.Connection = cn;
        //        cmd.CommandTimeout = 3000;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "SPX_SCELLE_AFFECTATION";
        //        cmd.Parameters.Add("@id_demande", SqlDbType.Int).Value = laDemande;
        //        cmd.Parameters.Add("@id_User", SqlDbType.Int).Value = IdUser;
        //        cmd.Parameters.Add("@id_idlot", SqlDbType.VarChar,255).Value = IdLot;
        //        cmd.Parameters.Add("@NumDepart", SqlDbType.Int ).Value = NumDepart;
        //        cmd.Parameters.Add("@NumFin", SqlDbType.Int).Value = NumFin;
        //        cmd.Parameters.Add("@NonbreAInserer", SqlDbType.Int).Value = NombreAInserer;
        //        cmd.Parameters.Add("@NombreRestant", SqlDbType.Int).Value = NombreRestant;

        //        DBBase.SetDBNullParametre(cmd.Parameters);
        //        try
        //        {
        //            if (cn.State == ConnectionState.Closed)
        //                cn.Open();
        //            resultat = cmd.ExecuteNonQuery ();
        //            int NombreInserer = 0;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                NombreInserer = (Convert.IsDBNull(reader["NOMNREINSERER"])) ? 0 : (int)reader["NOMNREINSERER"];
        //            }
        //            return NombreInserer;
        //        }
        //        catch (Exception ex)
        //        {
        //            return 0 ;
        //        }
        //        finally
        //        {
        //            if (cn.State == ConnectionState.Open)
        //                cn.Close(); // Fermeture de la connection 
        //            cmd.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public string ValidationReception(List<CsDetailAffectationScelle> ListeScelle, string MatAgent, int idetapeActuelle, string numdem)
        //{
        //    try
        //    {
        //        List<DetailAffectationScelle> DetailAffectationScelle = Entities.ConvertObject<DetailAffectationScelle, CsDetailAffectationScelle>(ListeScelle);
        //        List<CsMapperScelles> Scelles = new List<CsMapperScelles>();
        //        tbLot lot = new tbLot();

        //        CsAffectationScelle lAffectation = RetourneListeAffectationScelle(ListeScelle.FirstOrDefault().Id_Affectation.Value);
        //        var Id_Demande_ = RetourneListeAffectationScelle(lAffectation.Id_Affectation).Id_Demande.Value;
        //        CsDscelle dscelle_ = RetourneListeDemandeScelle(Id_Demande_).FirstOrDefault();


        //        lot.Date_Derniere_Modif = DateTime.Now;
        //        lot.DateReception = DateTime.Now;
        //        lot.lot_Couleur_ID = dscelle_.FK_IDCOULEURSCELLE;
        //        lot.Matricule_AgentModification = int.Parse(MatAgent);
        //        lot.Matricule_Creation = int.Parse(MatAgent);
        //        lot.Nombre_scelles_lot = dscelle_.NOMBRE_DEM;
        //        lot.Nombre_scelles_reçu = dscelle_.NOMBRE_REC;
        //        lot.Numero_depart = ListeScelle.Select(s => int.Parse(s.Nuemro_Scelle)).Min().ToString();
        //        lot.Numero_fin = ListeScelle.Select(s => int.Parse(s.Nuemro_Scelle)).Max().ToString();
        //        lot.lot_ID = lot.Numero_depart + "_" + lot.Numero_fin;
        //        lot.Origine_ID = null;
        //        lot.provenance_Scelle_ID = null;
        //        lot.Status_lot_ID = 0;
        //        lot.agence_centre_Appartenance = dscelle_.FK_IDCENTRE;
        //        lot.agence_centre_Origine = dscelle_.FK_IDCENTREFOURNISSEUR;
        //        var Id_Demande = RetourneListeAffectationScelle(ListeScelle.First().Id_Affectation.Value).Id_Demande.Value;
        //        CsDscelle dscelle = RetourneListeDemandeScelle(Id_Demande).FirstOrDefault();

        //        foreach (CsDetailAffectationScelle item in ListeScelle.Where(o => o.EstLivre == true).ToList())
        //        {
        //            //if (item.EstLivre == true)
        //            //{
        //            CsMapperScelles scelle = new CsMapperScelles();
        //            int codeCentre = item.Centre_Origine_Scelle.Value;
        //            scelle.Id_Scelle = Guid.NewGuid();
        //            scelle.agence_centre_Origine = item.Centre_Origine_Scelle;
        //            scelle.CodeCentre = codeCentre;
        //            scelle.Couleur_Scelle = dscelle.FK_IDCOULEURSCELLE;
        //            scelle.Date_creation_scelle = DateTime.Now;
        //            scelle.Date_creation_scelle = DateTime.Now;
        //            scelle.Numero_Scelle = item.Nuemro_Scelle;
        //            scelle.Origine_scelle = item.Centre_Origine_Scelle;
        //            scelle.provenance_Scelle_ID = item.Centre_Origine_Scelle;
        //            scelle.Status_ID = 3;
        //            scelle.agence_centre_Appartenance = dscelle.FK_IDCENTRE;
        //            Scelles.Add(scelle);
        //            //}
        //        }
        //        ListeScelle.ForEach(ds => ds.Date_Reception = DateTime.Now);
        //        ValidationReception(DetailAffectationScelle, Scelles, lot, numdem, idetapeActuelle, MatAgent);
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}


        public string ValidationReception(string MatAgent, string numdem, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_VALIDER_RECPETION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMDEMANDE", SqlDbType.VarChar, 20).Value = numdem;
            cmds.Parameters.Add("@IDAGENT", SqlDbType.Int).Value = int.Parse(MatAgent);
            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                int result = -1;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                result = cmds.ExecuteNonQuery();
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }


        public string ValidationReception(string NumDemande, int idEtapeActuelle, string matricule)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

            try
            {
                laCommande.CommandTimeout = 180;
                ValidationReception(matricule, NumDemande, laCommande);
                new DBAccueil().TransmettreDemande(NumDemande, idEtapeActuelle, matricule, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }



        //public string ValidationReception(List<DetailAffectationScelle> ListeScelle, List<CsMapperScelles> Scelle, tbLot Lot, string NumDemande, int idEtapeActuelle, string matricule)
        //{
        //    SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

        //    try
        //    {
        //        laCommande.CommandTimeout = 180;
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            Lot.Date_Derniere_Modif = DateTime.Now; ;
        //            Entities.InsertEntity<tbLot>(Lot, ctontext);
        //            Entities.UpdateEntity<DetailAffectationScelle>(ListeScelle, ctontext);
        //            Scelle.ForEach(s => s.lot_ID = Lot.lot_ID);
        //            Scelle.ForEach(ds => ds.Date_creation_scelle = DateTime.Now);

        //            DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(Scelle);
        //            Galatee.Tools.Utility.BulkInsertTable("scelle.Scelles", TablePere, laCommande.Connection, laCommande.Transaction);
        //            new DBAccueil().TransmettreDemande(NumDemande, idEtapeActuelle, matricule, laCommande);
        //            ctontext.SaveChanges();
        //        }
        //        laCommande.Transaction.Commit();
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        laCommande.Transaction.Rollback();
        //        return ex.Message;
        //    }
        //    finally
        //    {
        //        if (laCommande.Connection.State == ConnectionState.Open)
        //            laCommande.Connection.Close();
        //        laCommande.Dispose();
        //    }
        //}


        public void InsertAffectionScelle(CsMapperAffectationScelle AffectationScelle, List<CsMapperDetailAffectationScelle> Lst_DetailAffectationScelle, List<CsLotScelle> Lst_Lot, CsDscelle laDemande, SqlCommand laCommande)
        {
            try
            {
                List<CsMapperAffectationScelle> lstAffectation = new List<CsMapperAffectationScelle>();
                DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(Lst_DetailAffectationScelle);
                Galatee.Tools.Utility.BulkInsertTable("scelle.DetailAffectationScelle", TablePere, laCommande.Connection, laCommande.Transaction);

                lstAffectation.Add(AffectationScelle);
                DataTable Table = Galatee.Tools.Utility.ListToDataTable(lstAffectation);
                Galatee.Tools.Utility.BulkInsertTable("scelle.AffectationScelle", Table, laCommande.Connection, laCommande.Transaction);
                foreach (CsLotScelle item in Lst_Lot)
                {
                    CsTbLot tbLot = new CsTbLot();
                    tbLot.agence_centre_Appartenance = laDemande.FK_IDCENTRE;
                    //Le centre d'origine est mit est le mm que le centre du lot pour le moment
                    tbLot.agence_centre_Origine = laDemande.FK_IDCENTREFOURNISSEUR;
                    tbLot.Date_Derniere_Modif = DateTime.Now;
                    tbLot.DateReception = item.DateReception;
                    tbLot.lot_Couleur_ID = item.Couleur_ID;
                    tbLot.lot_ID = item.Id_LotMagasinGeneral;
                    tbLot.Matricule_AgentModification = item.Matricule_AgentDerniereModif;
                    tbLot.Matricule_Creation = item.Matricule_AgentReception;
                    tbLot.Nombre_scelles_lot = item.Nbre_Scelles;
                    tbLot.Nombre_scelles_reçu = AffectationScelle.Nbre_Scelles;
                    tbLot.Numero_depart = item.Numero_depart;
                    tbLot.Numero_fin = item.Numero_fin;
                    tbLot.Origine_ID = item.Origine_ID;
                    tbLot.Status_lot_ID = 1;
                    UpdateLotScelle(tbLot, laDemande.FK_IDCENTREFOURNISSEUR, item.Id_LotMagasinGeneral, laCommande);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void TraiterCompteurNonAffecte(CsCompteurBta compteur, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_TBCOMPTEURBTA_UPDATE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMERO_COMPTEUR", SqlDbType.VarChar, 20).Value = compteur.Numero_Compteur;
            cmds.Parameters.Add("@fk_idmarquecompteur", SqlDbType.Int).Value = compteur.FK_IDMARQUECOMPTEUR;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void TraiterCompteurAffecte(CsCompteurBta compteur, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_MAGASINVIRTUEL_INSERT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = compteur.NUMERO;
            cmds.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = compteur.MARQUE;
            cmds.Parameters.Add("@CADRAN", SqlDbType.TinyInt).Value = compteur.CADRAN;
            cmds.Parameters.Add("@ANNEEFAB", SqlDbType.VarChar, 4).Value = compteur.ANNEEFAB;
            cmds.Parameters.Add("@FONCTIONNEMENT", SqlDbType.VarChar, 1).Value = compteur.FONCTIONNEMENT;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = compteur.USERCREATION;
            cmds.Parameters.Add("@DATECREATION", SqlDbType.DateTime).Value = compteur.DATECREATION;
            cmds.Parameters.Add("@FK_IDTYPECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDTYPECOMPTEUR;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = compteur.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDMARQUECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDMARQUECOMPTEUR;
            cmds.Parameters.Add("@FK_IDCALIBRECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDCALIBRECOMPTEUR;
            cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = compteur.FK_IDCENTRE;
            cmds.Parameters.Add("@FK_TBCOMPTEUR", SqlDbType.Int).Value = compteur.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }



        public void MiseAJourMagasin(CsCompteurBta compteur)
        {
            SqlCommand cmds = DBBase.InitTransaction(ConnectionString);

            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_MAGASINVIRTUEL_UPDATE";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@NUMERO", SqlDbType.VarChar, 20).Value = compteur.NUMERO;
            cmds.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = compteur.MARQUE;
            cmds.Parameters.Add("@CADRAN", SqlDbType.TinyInt).Value = compteur.CADRAN;
            cmds.Parameters.Add("@ANNEEFAB", SqlDbType.VarChar, 4).Value = compteur.ANNEEFAB;
            cmds.Parameters.Add("@FONCTIONNEMENT", SqlDbType.VarChar, 1).Value = compteur.FONCTIONNEMENT;
            cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 6).Value = compteur.USERMODIFICATION;
            cmds.Parameters.Add("@DATEMODIFICATION", SqlDbType.DateTime).Value = compteur.DATEMODIFICATION;
            cmds.Parameters.Add("@FK_IDTYPECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDTYPECOMPTEUR;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = compteur.FK_IDPRODUIT;
            cmds.Parameters.Add("@FK_IDMARQUECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDMARQUECOMPTEUR;
            cmds.Parameters.Add("@FK_IDCALIBRECOMPTEUR", SqlDbType.Int).Value = compteur.FK_IDCALIBRECOMPTEUR;
            cmds.Parameters.Add("@FK_TBCOMPTEUR", SqlDbType.Int).Value = compteur.PK_ID;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
                cmds.Transaction.Commit();
            }
            catch (Exception ex)
            {
                cmds.Transaction.Rollback();
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }

            finally
            {
                if (cmds.Connection.State == ConnectionState.Open)
                    cmds.Connection.Close();
                cmds.Dispose();
            }

        }


        public void UpdateLotScelle(CsTbLot leLot, int FK_IDCENTREFOURNISSEUR, string IdLotMagasin, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_UPDATELOT";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@lot_ID", SqlDbType.VarChar, 255).Value = leLot.lot_ID;
            cmds.Parameters.Add("@typeReception", SqlDbType.Int).Value = leLot.typeReception;
            cmds.Parameters.Add("@DateReception", SqlDbType.DateTime).Value = leLot.DateReception;
            cmds.Parameters.Add("@Matricule_Creation", SqlDbType.Int).Value = leLot.Matricule_Creation;
            cmds.Parameters.Add("@Numero_depart", SqlDbType.VarChar, 15).Value = leLot.Numero_depart;
            cmds.Parameters.Add("@Numero_fin", SqlDbType.VarChar, 15).Value = leLot.Numero_fin;
            cmds.Parameters.Add("@Nombre_scelles_reçu", SqlDbType.Int).Value = leLot.Nombre_scelles_reçu;
            cmds.Parameters.Add("@Nombre_scelles_lot", SqlDbType.Int).Value = leLot.Nombre_scelles_lot;
            cmds.Parameters.Add("@provenance_Scelle_ID", SqlDbType.Int).Value = leLot.provenance_Scelle_ID;
            cmds.Parameters.Add("@Origine_ID", SqlDbType.Int).Value = leLot.Origine_ID;
            cmds.Parameters.Add("@Status_lot_ID", SqlDbType.Int).Value = leLot.Status_lot_ID;
            cmds.Parameters.Add("@lot_Couleur_ID", SqlDbType.Int).Value = leLot.lot_Couleur_ID;
            cmds.Parameters.Add("@Date_Derniere_Modif", SqlDbType.DateTime).Value = leLot.Date_Derniere_Modif;
            cmds.Parameters.Add("@TypeDeLot", SqlDbType.Int).Value = leLot.TypeDeLot;
            cmds.Parameters.Add("@Matricule_AgentModification", SqlDbType.Int).Value = leLot.Matricule_AgentModification;
            cmds.Parameters.Add("@agence_centre_Appartenance", SqlDbType.Int).Value = leLot.agence_centre_Appartenance;
            cmds.Parameters.Add("@agence_centre_Origine", SqlDbType.Int).Value = leLot.agence_centre_Origine;
            cmds.Parameters.Add("@FK_IDFOURNISSEUR", SqlDbType.Int).Value = FK_IDCENTREFOURNISSEUR;
            cmds.Parameters.Add("@Id_LotMagasinGeneral", SqlDbType.VarChar, 100).Value = IdLotMagasin;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }


        public void UpdateLotScelleMagasin(CsLotScelle leLot, int Nombre, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_UPDATELOTMAGASIN";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();



            cmds.Parameters.Add("@Numero_depart", SqlDbType.VarChar, 15).Value = int.Parse(leLot.Numero_depart) + Nombre;
            cmds.Parameters.Add("@Numero_fin", SqlDbType.VarChar, 15).Value = leLot.Numero_fin;
            cmds.Parameters.Add("@Nombre_scelles_lot", SqlDbType.Int).Value = int.Parse(leLot.Numero_fin) - int.Parse(leLot.Numero_depart);
            cmds.Parameters.Add("@Id_LotMagasinGeneral", SqlDbType.VarChar, 100).Value = leLot.Id_LotMagasinGeneral;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }

        public void ValiderAffectation(CsLotScelle leLot, int id_lademande, SqlCommand cmds)
        {
            cmds.CommandTimeout = 1800;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_VALIDER_AFFECTATION";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@Numero_depart", SqlDbType.VarChar, 15).Value = leLot.Numero_depart;
            cmds.Parameters.Add("@Numero_fin", SqlDbType.VarChar, 15).Value = leLot.Numero_fin;
            cmds.Parameters.Add("@NombreScelleAAffecter", SqlDbType.Int).Value = leLot.Nbre_Scelles;

            cmds.Parameters.Add("@Origine_ID", SqlDbType.Int).Value = leLot.Origine_ID;
            cmds.Parameters.Add("@Status_lot_ID", SqlDbType.Int).Value = leLot.StatutLot_ID;
            cmds.Parameters.Add("@lot_Couleur_ID", SqlDbType.Int).Value = leLot.Couleur_ID;
            cmds.Parameters.Add("@Matricule_AgentModification", SqlDbType.Int).Value = leLot.Matricule_AgentDerniereModif;
            cmds.Parameters.Add("@FK_IDFOURNISSEUR", SqlDbType.Int).Value = leLot.Fournisseur_ID;
            cmds.Parameters.Add("@Id_LotMagasinGeneral", SqlDbType.VarChar, 100).Value = leLot.Id_LotMagasinGeneral;
            cmds.Parameters.Add("@FK_IDDEMANDE", SqlDbType.Int).Value = id_lademande;
            DBBase.SetDBNullParametre(cmds.Parameters);
            try
            {
                string IdDemande = string.Empty;
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }




        public string InsertAffectionScelle(int id_lademande, string NUMDEM, int IdUser, int idEtapeActuelle, string Matricule, List<CsLotScelle> LstLotScelle)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
            try
            {
                foreach (var item in LstLotScelle)
                    ValiderAffectation(item, id_lademande, laCommande);

                new DBAccueil().TransmettreDemande(NUMDEM, idEtapeActuelle, Matricule, laCommande);
                laCommande.Transaction.Commit();
                return "";
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return ex.Message;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close();
                laCommande.Dispose();
            }
        }


        //public string InsertAffectionScelle(int id_lademande, int IdUser, int idEtapeActuelle, string Matricule, List<CsLotScelle> LstLotScelle)
        //{


        //    //Chargé les info lié a la demande 
        //    var demande = RetourneListeDemandeScelle(id_lademande).FirstOrDefault();

        //    ////Chargé les anciens en stock 
        //    //List<CsLotScelle> AncientbLotMagasinGeneral = RetourneListeScelle(demande.FK_IDCENTREFOURNISSEUR);

        //    SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

        //    try
        //    {


        //        CsMapperAffectationScelle AffectationScelle = new CsMapperAffectationScelle();
        //        List<CsMapperDetailAffectationScelle> Lst_DetailAffectationScelle = new List<CsMapperDetailAffectationScelle>();

        //        int NombreLotAffecter = 0;

        //        //Mise à jour des date de modification
        //        LstLotScelle.ForEach(t => t.Date_DerniereModif = DateTime.Now);

        //        //Alimentation des infos d'affectation
        //        AffectationScelle.Code_Centre_Dest = demande.FK_IDCENTRE;
        //        AffectationScelle.Code_Centre_Origine = demande.FK_IDCENTREFOURNISSEUR;
        //        AffectationScelle.Date_Transfert = DateTime.Now;
        //        AffectationScelle.Id_Affectation = Guid.NewGuid();
        //        AffectationScelle.Id_Modificateur = IdUser;
        //        AffectationScelle.Id_Demande = id_lademande;


        //        //Pour chaque element de lot à débité
        //        foreach (var item in LstLotScelle)
        //        {
        //            //Recupération de l'id du lot
        //            //string idlot = item.Id_LotMagasinGeneral;

        //            //Recuperation de l'ancien numero de depart du lot actuel
        //            //string Num_Depart = AncientbLotMagasinGeneral.FirstOrDefault(l => l.Id_LotMagasinGeneral == idlot).Numero_depart;

        //            //Recuperation de l'ancienne taille du numero de depart du lot actuel
        //            var NombrePositionAncienNumDepart = item.Numero_depart.ToString().Length;

        //            #region Sylla 04/04/2016

        //            //Recuperation du nouveau nombre de scelle a affecté pour le lot actuel courant en fesant la difference entre le "item.Numero_depart' et l'ancien numero de depart
        //            //int NombreScelleAffecterALot = int.Parse(item.Numero_depart) - int.Parse(Num_Depart);

        //            // Modification apporté le 04/04/2016 par BSY(Je supose que le nombre de scelles demandé est toujours éguale au nombre de scelles à affecté)
        //            //int NombreScelleAffecterALot = demande.NOMBRE_DEM; => FAUX, ZEG 04/07/2018

        //            //ZEG 04/07/2018
        //            int NombreScelleAffecterALot = item.Nbre_Scelles;

        //            #endregion


        //            //Incrementation du nombre de scelle a affecté pour le lot pour avoir le nombre totale en fin de boucle
        //            NombreLotAffecter = NombreLotAffecter + NombreScelleAffecterALot;

        //            //Génération des détail d'affectation en bouclant a partir du numero de depart de l'ancient lot qui est incrémenté jusko nombre de scelle a affecté
        //            //for (int i = int.Parse(Num_Depart); i == int.Parse(Num_Depart) + NombreScelleAffecterALot; i++)
        //            for (int i = int.Parse(item.Numero_depart); i <= int.Parse(item.Numero_depart) + (NombreScelleAffecterALot - 1); i++)
        //            {
        //                CsMapperDetailAffectationScelle DetailAffectationScelle = new CsMapperDetailAffectationScelle();

        //                DetailAffectationScelle.Centre_Origine_Scelle = item.CodeCentre;
        //                DetailAffectationScelle.Date_Reception = DateTime.Now;
        //                DetailAffectationScelle.Id_Affectation = AffectationScelle.Id_Affectation;
        //                DetailAffectationScelle.LotAppartenance_Id = item.Id_LotMagasinGeneral;
        //                DetailAffectationScelle.Id_DetailAffectationScelle = Guid.NewGuid();
        //                DetailAffectationScelle.Nuemro_Scelle = i.ToString().PadLeft(NombrePositionAncienNumDepart, '0');
        //                DetailAffectationScelle.EstLivre = false;
        //                Lst_DetailAffectationScelle.Add(DetailAffectationScelle);
        //            }
        //        }
        //        AffectationScelle.Nbre_Scelles = NombreLotAffecter;
        //        AffectationScelle.NumScelleDepart = Lst_DetailAffectationScelle.First().Nuemro_Scelle;
        //        AffectationScelle.NumScelleFin = Lst_DetailAffectationScelle.Last().Nuemro_Scelle;

        //        InsertAffectionScelle(AffectationScelle, Lst_DetailAffectationScelle, LstLotScelle, demande, laCommande);

        //        new DBAccueil().TransmettreDemande(demande.NUMDEM, idEtapeActuelle, Matricule, laCommande);
        //        laCommande.Transaction.Commit();
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        laCommande.Transaction.Rollback();
        //        return ex.Message;
        //    }
        //    finally
        //    {
        //        if (laCommande.Connection.State == ConnectionState.Open)
        //            laCommande.Connection.Close();
        //        laCommande.Dispose();
        //    }
        //}

        /*
        public bool ValidationReception(List<DetailAffectationScelle> ListeScelle, List<CsMapperScelles> Scelle, tbLot Lot)
        {
            cmd = new SqlCommand();
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.Connection.Open();
                cmd.Transaction = cmd.Connection.BeginTransaction();
               using (galadbEntities ctontext = new galadbEntities())
                {
                    Lot.Date_Derniere_Modif = DateTime.Now; ;
                    Entities.InsertEntity<tbLot>(Lot, ctontext);
                    Entities.UpdateEntity<DetailAffectationScelle>(ListeScelle.Take(20).ToList(), ctontext);
                    Scelle.ForEach(s => s.lot_ID = Lot.lot_ID);
                    Scelle.ForEach(ds => ds.Date_creation_scelle = DateTime.Now);

                    DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(Scelle);
                    Galatee.Tools.Utility.BulkInsertTable("scelle.Scelles", TablePere, cmd.Connection, cmd.Transaction);
                    ctontext.SaveChanges();
                }
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                cmd.Transaction.Rollback();
                throw ex;
            }
        }
        */

        #region  Statuts Scelle

        public List<CsRefStatutsScelles> SelectAllStatutScelle()
        {
            try
            {
                List<CsRefStatutsScelles> _lstStatutScelle = new List<CsRefStatutsScelles>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_StatusScelle_Modele_RETOURNE();
                _lstStatutScelle = Entities.GetEntityListFromQuery<CsRefStatutsScelles>(dt);


                return _lstStatutScelle;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Bobou

        public List<CsCouleurActivite> RetourneListeCouleurScelle(int Activite_ID)
        {
            try
            {
                List<CsCouleurActivite> _lstCouleur = new List<CsCouleurActivite>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeCouleurScelle(Activite_ID);
                _lstCouleur = Entities.GetEntityListFromQuery<CsCouleurActivite>(dt);

                return _lstCouleur.ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region LotMagasinGeneral

        public bool ValiderListeSaisieSelonDonnees(Dictionary<string, string> ListeSaisie, int OrigineLotsDeLaListe)
        {
            try
            {
                if (ListeSaisie == null || ListeSaisie.Count == 0)
                {
                    return false;
                }

                bool result = true;
                string NumeroDebut = "";
                string NumeroFin = "";
                List<CsLotMagasinGeneral> LotsTemp;
                List<CsLotMagasinGeneral> ListLotScelle;
                foreach (KeyValuePair<string, string> curItem in ListeSaisie)
                {
                    NumeroDebut = curItem.Key;
                    NumeroFin = curItem.Value;

                    //- Test numéro début
                    galadbEntities leContext = new galadbEntities();
                    //DataTable dt = ScelleProcedures.SCELLES_LotMagasinGeneral_RETOURNE();
                    ListLotScelle = SelectAllLotMagasinGeneral();
                    LotsTemp = ListLotScelle.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroDebut))
                                                                     && (Int32.Parse(NumeroDebut) <= Int32.Parse(l.Numero_fin))
                                                                     && l.Origine_ID == OrigineLotsDeLaListe).ToList();
                    if (LotsTemp != null && LotsTemp.Count > 0)
                    {
                        result = false;
                        break;
                    }

                    //- Test numéro fin
                    LotsTemp = ListLotScelle.Where(l => (Int32.Parse(l.Numero_depart) <= Int32.Parse(NumeroFin))
                                                                     && (Int32.Parse(NumeroFin) <= Int32.Parse(l.Numero_fin))
                                                                     && l.Origine_ID == OrigineLotsDeLaListe).ToList();
                    if (LotsTemp != null && LotsTemp.Count > 0)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLotMagasinGeneral> SelectAllLotMagasinGeneral()
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_SELECT_MAGASIN";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotMagasinGeneral>(dt);
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

        public List<CsLotMagasinGeneral> SelectAllLotMagasinGeneralByid(string LotMagasinGeneral_Id)
        {
            try
            {


                List<CsLotMagasinGeneral> _lstLotMagasinGeneral = new List<CsLotMagasinGeneral>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_LotMagasinGeneral_RETOURNEById(LotMagasinGeneral_Id);
                _lstLotMagasinGeneral = Entities.GetEntityListFromQuery<CsLotMagasinGeneral>(dt);

                return _lstLotMagasinGeneral.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(CsLotMagasinGeneral cLotMagasinGeneral)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsLotMagasinGeneral SelectLotMagasinGeneralByCoperDemandeId(string cLotMagasinGeneral)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsLotMagasinGeneral>(ScelleProcedures.SCELLES_LotMagasinGeneral_RETOURNEById(cLotMagasinGeneral));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsLotMagasinGeneral> cLotMagasinGeneral)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsLotMagasinGeneral cLotMagasinGeneral)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsLotMagasinGeneral> cLotMagasinGeneral)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsLotMagasinGeneral cLotMagasinGeneral)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsLotMagasinGeneral> cLotMagasinGeneral)
        {
            try
            {
                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
                Galatee.Tools.Utility.InsertionEnBloc<CsLotMagasinGeneral>(cLotMagasinGeneral, "scelle.tbLotMagasinGeneral", laCommande);
                laCommande.Transaction.Commit();
                return true;
                //return Entities.InsertEntity<Galatee.Entity.Model.tbLotMagasinGeneral>(Entities.ConvertObject<Galatee.Entity.Model.tbLotMagasinGeneral, CsLotMagasinGeneral>(cLotMagasinGeneral));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region CompteurBta
        public List<CsCompteurBta> SelectAllCompteurNonAffecte()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
                DataTable dt = ScelleProcedures.SelectAllCompteurNonAffecte();
                List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
                return l;

                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool VerifieCompteurExiste(CsCompteurBta leCompteur)
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
                DataTable dt = ScelleProcedures.SCELLES_CompteurBta(leCompteur);
                List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
                return (l.Count != 0) ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Sylla 09/01/2017
        public bool VerifieCompteurExistenew(CsCompteur leCompteur)
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNE());
                DataTable dt = ScelleProcedures.SCELLES_CompteurBtaNew(leCompteur);
                List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
                return (l.Count != 0) ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<CsCompteurBta> SelectAllCompteurByid(string CompteurBta_Id)
        {
            try
            {


                List<CsCompteurBta> _lstCompteur = new List<CsCompteurBta>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_CompteurBta_RETOURNEById(CompteurBta_Id);
                _lstCompteur = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);

                return _lstCompteur.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(CsCompteurBta sCompteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCompteurBta SelectCompteurByCoperDemandeId(string sCompteur)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_CompteurBta_RETOURNEById(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsCompteurBta sCompteur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsCompteurBta sCompteur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsCompteurBta> sCompteur)
        {
            try
            {

                return Entities.InsertEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region MAGASINVIRTUEL
        //public int InsertCompteurscelle(List<CsCompteurBta> sCompteur)
        //{
        //    using (galadbEntities context = new galadbEntities())
        //    {
        //        try
        //        {
        //            List<TbCompteurBTA> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
        //            List<MAGASINVIRTUEL> listcompteurMAgasin = (Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(sCompteur));
        //           MettreAJourLeScellePoseSurOrgane(sCompteur);
        //            Entities.InsertEntity<TbCompteurBTA>(listCompteurBta, context);
        //            Entities.InsertEntity<MAGASINVIRTUEL>(listcompteurMAgasin, context);

        //            return context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }


        //}

        //public int InsertCompteurscelle(List<CsCompteurBta> sCompteur)
        //{
        //    using (galadbEntities context = new galadbEntities())
        //    {
        //        try
        //        {
        //            List<TbCompteurBTA> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
        //            MettreAJourLeScellePoseSurOrgane(sCompteur);
        //            Entities.InsertEntity<TbCompteurBTA>(listCompteurBta, context);

        //            return context.SaveChanges();
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }

        //    }
        //}

        public string InsertCompteurscelle(List<CsCompteurBta> sCompteur)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

            try
            {
                foreach (CsCompteurBta item in sCompteur)
                    InsertOrUpdateCompteurEtScellage(item, laCommande);
                laCommande.Transaction.Commit();
                return string.Empty;
            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return ex.Message;
            }
            finally
            {
               
                laCommande.Dispose();
            }
        }
        public void InsertOrUpdateCompteurEtScellage(CsCompteurBta leCompteur, SqlCommand cmds)
        {
            cmds.CommandTimeout = 3000;
            cmds.CommandType = CommandType.StoredProcedure;
            cmds.CommandText = "SPX_SCELLE_SAISIE_ET_SCELLAGE_COMPTEUR";
            if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

            cmds.Parameters.Add("@Numero_Compteur", SqlDbType.VarChar, 20).Value = leCompteur.NUMERO;
            //cmds.Parameters.Add("@Type_Compteur_ID", SqlDbType.Int).Value = leCompteur.Type_Compteur;
            cmds.Parameters.Add("@StatutCompteur", SqlDbType.VarChar, 75).Value = leCompteur.StatutCompteur;
            cmds.Parameters.Add("@EtatCompteur_ID", SqlDbType.Int ).Value = leCompteur.EtatCompteur_ID;
            cmds.Parameters.Add("@CapotMoteur_ID_Scelle1", SqlDbType.UniqueIdentifier).Value = leCompteur.CapotMoteur_ID_Scelle1;
            cmds.Parameters.Add("@CapotMoteur_ID_Scelle2", SqlDbType.UniqueIdentifier).Value = leCompteur.CapotMoteur_ID_Scelle2;
            cmds.Parameters.Add("@CapotMoteur_ID_Scelle3", SqlDbType.UniqueIdentifier).Value = leCompteur.CapotMoteur_ID_Scelle3;
            cmds.Parameters.Add("@Cache_Scelle", SqlDbType.UniqueIdentifier).Value = leCompteur.Cache_Scelle;
            cmds.Parameters.Add("@MARQUE", SqlDbType.VarChar, 2).Value = leCompteur.MARQUE;
            cmds.Parameters.Add("@ANNEEFAB", SqlDbType.VarChar, 4).Value = leCompteur.ANNEEFAB;
            cmds.Parameters.Add("@TYPE_COMPTEUR", SqlDbType.VarChar, 50).Value = leCompteur.Type_Compteur;
            cmds.Parameters.Add("@CADRAN", SqlDbType.TinyInt).Value = leCompteur.CADRAN;
            cmds.Parameters.Add("@FK_IDCALIBRECOMPTEUR ", SqlDbType.Int).Value = leCompteur.FK_IDCALIBRECOMPTEUR;
            cmds.Parameters.Add("@FK_IDMARQUECOMPTEUR", SqlDbType.Int).Value = leCompteur.FK_IDMARQUECOMPTEUR;
            cmds.Parameters.Add("@FK_IDTYPECOMPTEUR", SqlDbType.Int).Value = leCompteur.FK_IDTYPECOMPTEUR;
            cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = leCompteur.USERCREATION;
            cmds.Parameters.Add("@FK_IDPRODUIT", SqlDbType.Int).Value = leCompteur.FK_IDPRODUIT;

            DBBase.SetDBNullParametre(cmds.Parameters);

            try
            {
                if (cmds.Connection.State == ConnectionState.Closed)
                    cmds.Connection.Open();
                cmds.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(cmds.CommandText + ":" + ex.Message);
            }
        }
        public List<CsCompteurBta> SelectAllCompteurPourAffectation()
        {
            SqlConnection laConnection = DBBase.InitConnection(ConnectionString);
            try
            {
                List<CsCompteurBta> lesCompteur = new List<CsCompteurBta>();
                lesCompteur.AddRange(SelectAllCompteurInDisponible(laConnection));
                lesCompteur.AddRange(SelectAllCompteur(laConnection));
                return lesCompteur;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (laConnection.State == ConnectionState.Open)
                    laConnection.Close(); // Fermeture de la connection 
                laConnection.Dispose();
            }
        }
        public List<CsCompteurBta> SelectAllCompteurInDisponible(SqlConnection laConnection)
        {

            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_LISTECOMPTEUR_AFFECTES";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
            //try
            //{
            //    using (galadbEntities context = new galadbEntities())
            //    {
            //        DataTable dt = ScelleProcedures.ReturnecompteursInDisponibles(null, null, string.Empty, string.Empty);
            //        var rslt = Entities.GetEntityListFromQuery<CsCompteurBta>(dt).Distinct();
            //        List<CsCompteurBta> l = rslt != null ? rslt.ToList() : new List<CsCompteurBta>();
            //        return l;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public List<CsCompteurBta> SelectAllCompteur(SqlConnection laConnection)
        {
            cmd = new SqlCommand();
            cmd.Connection = laConnection;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_LISTECOMPTEUR_NONAFFECTES";

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (laConnection.State == ConnectionState.Closed)
                    laConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception(cmd.CommandText + ":" + ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        
        }


        public int UpdateCompteurscelleSuiteModif(List<CsCompteurBta> sCompteur)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    //List<TbCompteurBTA> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
                    //List<MAGASINVIRTUEL> listcompteurMAgasin = (Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(sCompteur));
                    MettreAJourLeScellePoseSurOrganeSuiteModif(sCompteur, context);
                    //Entities.UpdateEntity<TbCompteurBTA>(listCompteurBta, context);
                    return context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }


        }
        public int UpdateCompteurscelle(List<CsCompteurBta> sCompteur)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    List<TbCompteurBTA> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
                    List<MAGASINVIRTUEL> listcompteurMAgasin = (Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(sCompteur));
                    MettreAJourLeScellePoseSurOrgane(sCompteur);
                    Entities.UpdateEntity<TbCompteurBTA>(listCompteurBta, context);
                    return context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }


        }
        public bool VerifierEtatCompteur(CsCompteurBta sCompteur)
        {
            try
            {

                using (galadbEntities ctx = new galadbEntities())
                {
                    TbCompteurBTA leCompt = ctx.TbCompteurBTA.FirstOrDefault(o => o.PK_ID == sCompteur.PK_ID);
                    if (leCompt != null)
                    {
                        MAGASINVIRTUEL leMagasin = ctx.MAGASINVIRTUEL.FirstOrDefault(t => t.FK_TBCOMPTEUR == leCompt.PK_ID);
                        if (leMagasin != null && leMagasin.ETAT != Enumere.CompteurAffecte)
                            return true;

                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void MettreAJourLeScellePoseSurOrganeSuiteModif(List<CsCompteurBta> sCompteur, galadbEntities leContext)
        {
            try
            {
                List<CsScelle> ListScelles = RetourneScellesListe();
                if (sCompteur != null && sCompteur.Count > 0)
                {
                    foreach (CsCompteurBta item in sCompteur)
                    {
                        bool majMag = false;
                        CsCompteurBta magasin = new CsCompteurBta();

                        TbCompteurBTA leCompt = new TbCompteurBTA();
                        leCompt = leContext.TbCompteurBTA.FirstOrDefault(o => o.PK_ID == item.PK_ID);
                        if (leCompt != null)
                        {
                            leCompt.Numero_Compteur = item.Numero_Compteur;
                            leCompt.MARQUE = item.MARQUE;
                            leCompt.ANNEEFAB = item.ANNEEFAB;
                            leCompt.FK_IDPRODUIT = item.FK_IDPRODUIT;
                            leCompt.CADRAN = item.CADRAN;
                            leCompt.FK_IDCALIBRECOMPTEUR = item.FK_IDCALIBRECOMPTEUR;
                            leCompt.FK_IDMARQUECOMPTEUR = item.FK_IDMARQUECOMPTEUR;
                            leCompt.EtatCompteur_ID = item.EtatCompteur_ID;
                            leCompt.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR;
                            leCompt.TYPE_COMPTEUR = item.Type_Compteur;

                            leCompt.Cache_Scelle = item.Cache_Scelle;
                            leCompt.CapotMoteur_ID_Scelle1 = item.CapotMoteur_ID_Scelle1;
                            leCompt.CapotMoteur_ID_Scelle2 = item.CapotMoteur_ID_Scelle2;
                            leCompt.CapotMoteur_ID_Scelle3 = item.CapotMoteur_ID_Scelle3;

                            Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(item));


                            if (!string.IsNullOrEmpty(item.ANCNUMEROCRT) && item.ANCNUMEROCRT != item.Numero_Compteur)
                            {
                                MAGASINVIRTUEL leMagasin = leContext.MAGASINVIRTUEL.FirstOrDefault(t => t.FK_TBCOMPTEUR == leCompt.PK_ID);
                                if (leMagasin != null)
                                {
                                    magasin.NUMERO = leCompt.Numero_Compteur;
                                    magasin.MARQUE = leCompt.MARQUE;
                                    magasin.FK_IDMARQUECOMPTEUR = leCompt.FK_IDMARQUECOMPTEUR;
                                    magasin.FK_IDPRODUIT = item.FK_IDPRODUIT;
                                    magasin.FK_IDCALIBRECOMPTEUR = leCompt.FK_IDCALIBRECOMPTEUR;
                                    magasin.ANNEEFAB = leCompt.ANNEEFAB;
                                    magasin.CADRAN = leCompt.CADRAN;
                                    magasin.TYPECOMPTEUR = item.TYPECOMPTEUR;
                                    magasin.USERMODIFICATION = item.USERMODIFICATION;
                                    magasin.PK_ID = leMagasin.PK_ID;
                                    if (item.FK_IDTYPECOMPTEUR != null)
                                        magasin.FK_IDTYPECOMPTEUR = item.FK_IDTYPECOMPTEUR.Value;

                                    majMag = true;
                                }

                            }
                        }

                        if (item.CapotMoteur_ID_Scelle1 != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            //Scelles leScellePose = leContext.Scelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle1);
                            CsScelle leScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle1);
                            if (leScellePose != null)
                            {
                                leScellePose.DateDePose = DateTime.Now;
                                leScellePose.Status_ID = 5;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));

                                if (item.AncCapotMoteur_ID_Scelle1 != null && item.CapotMoteur_ID_Scelle1 != item.AncCapotMoteur_ID_Scelle1)
                                {
                                    CsScelle leAncScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle1);
                                    if (leAncScellePose != null)
                                    {
                                        leAncScellePose.DateDePose = null;
                                        leAncScellePose.Status_ID = 2;
                                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                                    }
                                }
                            }
                        }
                        if (item.CapotMoteur_ID_Scelle2 != null)
                        {

                            //Recuperer le scelle correspondant depuis la base
                            CsScelle leScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle2);
                            if (leScellePose != null)
                            {
                                leScellePose.DateDePose = DateTime.Now;
                                leScellePose.Status_ID = 5;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                                if (item.AncCapotMoteur_ID_Scelle2 != null && item.CapotMoteur_ID_Scelle1 != item.AncCapotMoteur_ID_Scelle2)
                                {
                                    CsScelle leAncScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle2);
                                    if (leAncScellePose != null)
                                    {
                                        leAncScellePose.DateDePose = null;
                                        leAncScellePose.Status_ID = 2;
                                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                                    }
                                }
                            }
                        }
                        if (item.CapotMoteur_ID_Scelle3 != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            CsScelle leScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle3);
                            if (leScellePose != null)
                            {
                                leScellePose.DateDePose = DateTime.Now;
                                leScellePose.Status_ID = 5;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                                if (item.AncCapotMoteur_ID_Scelle3 != null && item.CapotMoteur_ID_Scelle3 != item.AncCapotMoteur_ID_Scelle3)
                                {
                                    CsScelle leAncScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle3);
                                    if (leAncScellePose != null)
                                    {
                                        leAncScellePose.DateDePose = null;
                                        leAncScellePose.Status_ID = 2;
                                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                                    }
                                }
                            }
                        }
                        if (item.Cache_Scelle != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            CsScelle leScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.Cache_Scelle);
                            if (leScellePose != null)
                            {
                                leScellePose.DateDePose = DateTime.Now;
                                leScellePose.Status_ID = 5;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                                if (item.AncCache_Scelle != null && item.Cache_Scelle != item.AncCache_Scelle)
                                {
                                    CsScelle leAncScellePose = ListScelles.FirstOrDefault(o => o.Id_Scelle == (Guid)item.AncCache_Scelle);
                                    if (leAncScellePose != null)
                                    {
                                        leAncScellePose.DateDePose = null;
                                        leAncScellePose.Status_ID = 2;
                                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                                    }
                                }
                            }
                        }


                        if (majMag)
                            MiseAJourMagasin(magasin);

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void MettreAJourLeScellePoseSurOrgane(List<CsCompteurBta> sCompteur)
        {
            try
            {
                List<CsScelle> ListScelles = RetourneScellesListe();
                if (sCompteur != null && sCompteur.Count > 0)
                {
                    foreach (CsCompteurBta item in sCompteur)
                    {
                        CsScelle leScellePose = new CsScelle();
                        CsScelle leAncScellePose = new CsScelle();
                        if (item.CapotMoteur_ID_Scelle1 != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle1);
                            leScellePose.DateDePose = DateTime.Now;
                            leScellePose.Status_ID = 5;
                            Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                            if (item.AncCapotMoteur_ID_Scelle1 != null && item.CapotMoteur_ID_Scelle1 != item.AncCapotMoteur_ID_Scelle1)
                            {
                                leAncScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle1);
                                leScellePose.DateDePose = null;
                                leScellePose.Status_ID = 2;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                            }
                        }
                        if (item.CapotMoteur_ID_Scelle2 != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle2);
                            leScellePose.Status_ID = 5;
                            Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                            if (item.AncCapotMoteur_ID_Scelle2 != null && item.CapotMoteur_ID_Scelle2 != item.AncCapotMoteur_ID_Scelle2)
                            {
                                leAncScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle2);
                                leScellePose.DateDePose = null;
                                leScellePose.Status_ID = 2;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                            }
                        }
                        if (item.CapotMoteur_ID_Scelle3 != null)
                        {
                            leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.CapotMoteur_ID_Scelle3);
                            leScellePose.Status_ID = 5;
                            Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));

                            if (item.AncCapotMoteur_ID_Scelle3 != null && item.CapotMoteur_ID_Scelle3 != item.AncCapotMoteur_ID_Scelle3)
                            {
                                leAncScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.AncCapotMoteur_ID_Scelle3);
                                leScellePose.DateDePose = null;
                                leScellePose.Status_ID = 2;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                            }
                        }
                        if (item.Cache_Scelle != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.Cache_Scelle);
                            leScellePose.Status_ID = 5;
                            Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                            if (item.AncCache_Scelle != null && item.Cache_Scelle != item.AncCache_Scelle)
                            {
                                leAncScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)item.AncCache_Scelle);
                                leScellePose.DateDePose = null;
                                leScellePose.Status_ID = 2;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leAncScellePose));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool InsertMAGASINVIRTUEL(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.MAGASINVIRTUEL>(Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(sCompteur));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Margasin virtuelle 03/03/2016

        public List<CsCompteurBta> SelectAllMargasinVirtuelle()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsCompteurBta>(ScelleProcedures.SCELLES_MAGASINVIRTUEL_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCompteurBta> SelectAlllMargasinVirtuelleByid(int CompteurBta_Id)
        {
            try
            {


                List<CsCompteurBta> _lstCompteur = new List<CsCompteurBta>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_MAGASINVIRTUEL_RETOURNEById(CompteurBta_Id);
                _lstCompteur = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);

                return _lstCompteur.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*
        public int InsertMargasinVirtuelle(List<CsCompteurBta> sCompteur, List<CsCompteurBta> sCompteur1)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    List<MAGASINVIRTUEL> listcompteurMAgasin = new List<MAGASINVIRTUEL>();
                    List<MAGASINVIRTUEL> listcompteurMAgasin1 = new List<MAGASINVIRTUEL>();

                    List<MAGASINVIRTUEL> listcompteurMAgasin1ToDelete = new List<MAGASINVIRTUEL>();
                    List<TbCompteurBTA> listcTbCompteurBTA = new List<TbCompteurBTA>();
                    var TbCompteurBTA = context.TbCompteurBTA;
                    var MAGASINVIRTUEL = context.MAGASINVIRTUEL;
                    //List<TbCompteurBTA> listcompteur = Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur);
                    //Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(listcompteur, context);
                    foreach (CsCompteurBta item in sCompteur)
                    {
                        TbCompteurBTA leCompteur = TbCompteurBTA.FirstOrDefault(u => u.Numero_Compteur == item.NUMERO && item.MARQUE == u.MARQUE);
                        MAGASINVIRTUEL leCompteur1 = MAGASINVIRTUEL.FirstOrDefault(u => u.NUMERO == item.NUMERO && item.MARQUE == u.MARQUE);
                        if (leCompteur1 == null)
                        {
                            if (leCompteur != null)
                            {
                                leCompteur.StatutCompteur = "Affecté";
                                MAGASINVIRTUEL leCptMagasin = Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(item);
                                leCptMagasin.FK_TBCOMPTEUR = leCompteur.PK_ID;
                                listcompteurMAgasin.Add(leCptMagasin);
                            }

                        }
                        else
                        {
                            if (item.FK_IDCENTRE != null)
                                leCompteur1.FK_IDCENTRE = item.FK_IDCENTRE.Value;
                            listcompteurMAgasin1.Add(leCompteur1);
                        }
                    }

                    foreach (var item in sCompteur1)
                    {
                        MAGASINVIRTUEL leCompteur1 = MAGASINVIRTUEL.FirstOrDefault(u => u.NUMERO == item.NUMERO && item.MARQUE == u.MARQUE);
                        if (leCompteur1 != null)
                            listcompteurMAgasin1ToDelete.Add(leCompteur1);
                        TbCompteurBTA leCompteur = TbCompteurBTA.FirstOrDefault(u => u.Numero_Compteur == item.NUMERO && item.MARQUE == u.MARQUE);
                        leCompteur.StatutCompteur = "Non_Affecté";
                        listcTbCompteurBTA.Add(leCompteur);

                    }
                    Entities.InsertEntity<MAGASINVIRTUEL>(listcompteurMAgasin, context);
                    Entities.UpdateEntity<MAGASINVIRTUEL>(listcompteurMAgasin1, context);
                    Entities.DeleteEntity<MAGASINVIRTUEL>(listcompteurMAgasin1ToDelete, context);

                    Entities.UpdateEntity<TbCompteurBTA>(listcTbCompteurBTA, context);

                    return context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }


        }
        */

        public int InsertMargasinVirtuelle(List<CsCompteurBta> sCompteur, List<CsCompteurBta> sCompteur1)
        {
            SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);

            try
            {

                foreach (CsCompteurBta cpt in sCompteur)
                {
                    TraiterCompteurAffecte(cpt, laCommande);
                }

                foreach (CsCompteurBta cpt in sCompteur1)
                {
                    TraiterCompteurNonAffecte(cpt, laCommande);
                }
                laCommande.Transaction.Commit();

                return 1;

            }
            catch (Exception ex)
            {
                laCommande.Transaction.Rollback();
                return 0;
            }
            finally
            {
                if (laCommande.Connection.State == ConnectionState.Open)
                    laCommande.Connection.Close(); // Fermeture de la connection 
                laCommande.Dispose();
            }

        }



        public int UpdateMargasinVirtuelle(List<CsCompteurBta> sCompteur)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    List<MAGASINVIRTUEL> listcompteurMAgasin = (Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(sCompteur));
                    MettreAJourMargasinVirtuelle(sCompteur);
                    Entities.UpdateEntity<MAGASINVIRTUEL>(listcompteurMAgasin, context);

                    return context.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }


        }

        public void MettreAJourMargasinVirtuelle(List<CsCompteurBta> sCompteur)
        {
            try
            {
                List<CsCompteurBta> ListCompteurScelle = SelectAllCompteur();
                if (sCompteur != null && sCompteur.Count > 0)
                {

                    foreach (CsCompteurBta item in sCompteur)
                    {



                        CsCompteurBta leSatutCompteurBta = new CsCompteurBta();
                        if (item.Numero_Compteur != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            //leSatutCompteurBta = ListCompteurScelle.FirstOrDefault(s => s.Numero_Compteur == item.NUMERO);

                            leSatutCompteurBta.StatutCompteur = "Affecté";
                            leSatutCompteurBta.DATECREATION = DateTime.Now;

                            Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(leSatutCompteurBta));
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void MettreDeleteMargasinVirtuelle(CsCompteurBta sCompteur)
        {
            try
            {
                List<CsCompteurBta> ListCompteurScelle = SelectAllCompteur();



                CsCompteurBta leSatutCompteurBta = new CsCompteurBta();

                //Recuperer le scelle correspondant depuis la base
                //leSatutCompteurBta = ListCompteurScelle.FirstOrDefault(s => s.Numero_Compteur == sCompteur.NUMERO);

                leSatutCompteurBta.StatutCompteur = "Non_Affecté";
                //leSatutCompteurBta.DATECREATION = DateTime.Now;
                leSatutCompteurBta.DATEMODIFICATION = DateTime.Now;

                Entities.UpdateEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(leSatutCompteurBta));


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public bool DeleteMagV(CsCompteurBta Margasin)
        {
            try
            {
                MettreDeleteMargasinVirtuelle(Margasin);
                return Entities.DeleteEntity<Galatee.Entity.Model.MAGASINVIRTUEL>(Entities.ConvertObject<Galatee.Entity.Model.MAGASINVIRTUEL, CsCompteurBta>(Margasin));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool DeleteMargasinVirtuelle(List<CsCompteurBta> sCompteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TbCompteurBTA>(Entities.ConvertObject<Galatee.Entity.Model.TbCompteurBTA, CsCompteurBta>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region VwRechercheCompteur
        public List<CsRechercheCompteur> SelectAllRechercheCompteur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRechercheCompteur>(ScelleProcedures.SCELLES_vWRechercheCompteur_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsRechercheCompteur RechercheCompteurByid(string Numero_compteur)
        {
            try
            {


                List<CsRechercheCompteur> _lstOrigineScelle = new List<CsRechercheCompteur>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLE_vWRechercheCompteurById(Numero_compteur);
                _lstOrigineScelle = Entities.GetEntityListFromQuery<CsRechercheCompteur>(dt);


                return _lstOrigineScelle.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Marque_Modele

        public List<CsMarque_Modele> RetourneListeMarque_ModeleByid(int Id_marque)
        {
            try
            {
                List<CsMarque_Modele> _lstMarque_Modele = new List<CsMarque_Modele>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLE_Marque_ModeleById(Id_marque);
                _lstMarque_Modele = Entities.GetEntityListFromQuery<CsMarque_Modele>(dt);

                return _lstMarque_Modele.ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region LIstedesScellesRemis

        public List<CsRemiseScelleByAg> RetourneListeRemiScellesAg(int pk_ID)
        {
            try
            {
                List<CsRemiseScelleByAg> _lstScelle = new List<CsRemiseScelleByAg>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_RemiseScelle_RETOURNE(pk_ID);
                _lstScelle = Entities.GetEntityListFromQuery<CsRemiseScelleByAg>(dt);


                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsRemiseScelleByAg> SCELLES_RETOURNE_Pour_ScellageCpt(int pk_ID)
        {
            try
            {
                List<CsRemiseScelleByAg> _lstScelle = new List<CsRemiseScelleByAg>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_RETOURNE_Pour_ScellageCpt(pk_ID);
                _lstScelle = Entities.GetEntityListFromQuery<CsRemiseScelleByAg>(dt).OrderBy(u => u.Numero_Scelle).ToList();


                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        #endregion

        #region scelle

        public List<CsScelle> LoadListeScelle(int idAgentMatricule, int fk_TypeDemande)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_SCELLE_LISTEDESCELLEPARAGENT";
            cmd.Parameters.Add("@IDAGENT", SqlDbType.Int).Value = idAgentMatricule;
            cmd.Parameters.Add("@TYPEDEMANDE", SqlDbType.Int).Value = fk_TypeDemande;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<Structure.CsScelle>(dt);
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
        public List<CsScelle> RetourneScellesListeByAgence(int IdCentre)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 360;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_SCELLE_LISTEDESCELLEPOURAFFECTATION";
            cmd.Parameters.Add("@IDCENTRE", SqlDbType.Int).Value = IdCentre;
            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<Structure.CsScelle>(dt);
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

        public List<CsScelle> RetourneScellesListe()
        {
            try
            {
                List<CsScelle> _lstscelle = new List<CsScelle>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_Scelle_RETOURNE();
                _lstscelle = Entities.GetEntityListFromQuery<CsScelle>(dt);
                return _lstscelle;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Update(CsScelle sScelle)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(sScelle));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  Remise scelle

        public List<CsRemiseScelles> SelectAllRemiseScelle()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRemiseScelles>(ScelleProcedures.SCELLES_RemiseScelleAgt_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRemiseScelles> SelectAllRemiseScelleByid(Guid Id_remise)
        {
            try
            {


                List<CsRemiseScelles> _lstremiseScelle = new List<CsRemiseScelles>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_RemiseScelleAgt_RETOURNEById(Id_remise);
                _lstremiseScelle = Entities.GetEntityListFromQuery<CsRemiseScelles>(dt);

                return _lstremiseScelle.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(CsRemiseScelles sCompteur)
        {
            try
            {
                MettreAJourDELETELeScelleRemis(sCompteur);
                MettreAJourDeleteLeLotRemis(sCompteur);
                Entities.DeleteEntity<Galatee.Entity.Model.tbDetailRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbDetailRemiseScelles, CsRemiseScelles>(sCompteur));

                return Entities.DeleteEntity<Galatee.Entity.Model.tbRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsRemiseScelles SelectRemiseScellesId(Guid sCompteur)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsRemiseScelles>(ScelleProcedures.SCELLES_RemiseScelleAgt_RETOURNEById(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public bool Delete(List<CsRemiseScelles> sCompteur)
        //{
        //    try
        //    {
        //        MettreAJourDELETELeScelleRemis(sCompteur);
        //        MettreAJourDeleteLeLotRemis(sCompteur);
        //        Entities.DeleteEntity<Galatee.Entity.Model.tbDetailRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbDetailRemiseScelles, CsRemiseScelles>(sCompteur));
        //        return Entities.DeleteEntity<Galatee.Entity.Model.tbRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sCompteur));

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool Update(CsRemiseScelles sRemise)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.tbRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sRemise));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsRemiseScelles> sRemise)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.tbRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sRemise));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsRemiseScelles sRemise)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.tbRemiseScelles>(Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sRemise));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Insert(List<CsRemiseScelles> sremise)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    List<tbRemiseScelles> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.tbRemiseScelles, CsRemiseScelles>(sremise));
                    List<tbDetailRemiseScelles> listDetailremise = (Entities.ConvertObject<Galatee.Entity.Model.tbDetailRemiseScelles, CsRemiseScelles>(sremise));
                    listDetailremise = MettreAJourDetailRemiseScelles(sremise);
                    if (listDetailremise.Count == 0)
                        listDetailremise = (Entities.ConvertObject<Galatee.Entity.Model.tbDetailRemiseScelles, CsRemiseScelles>(sremise));

                    List<Guid> lstIdScelle = sremise.Select(u => u.Id_Scelle.Value).ToList();
                    List<Scelles> lstScelleAMAJ = context.Scelles.Where(y => lstIdScelle.Contains(y.Id_Scelle)).ToList();
                    lstScelleAMAJ.ForEach(o => o.Status_ID = 2);


                    List<string> lstIdLot = sremise.Select(u => u.Lot_Id).ToList();
                    List<tbLot> lstLotAMAJ = context.tbLot.Where(y => lstIdLot.Contains(y.lot_ID)).ToList();
                    lstLotAMAJ.ForEach(o => o.Status_lot_ID = 1);

                    //MettreAJourLeScelleRemis(sremise, listDetailremise);
                    //MettreAJourLeLotRemis(sremise);

                    Entities.InsertEntity<tbRemiseScelles>(listCompteurBta.First(), context);
                    listDetailremise.ForEach(c => c.Id_Remise = listCompteurBta.First().Id_Remise);
                    Entities.InsertEntity<tbDetailRemiseScelles>(listDetailremise, context);

                    return context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void MettreAJourLeScelleRemis(List<CsRemiseScelles> sRemise, List<tbDetailRemiseScelles> listDetailremise)
        {
            try
            {
                CsScelle leScellePose = new CsScelle();
                List<CsScelle> ListScelles = RetourneScellesListe();
                if (sRemise != null && sRemise.Count > 0)
                {
                    foreach (CsRemiseScelles item in sRemise)
                    {

                        if (item.Id_Scelle != Guid.Empty)
                        {
                            foreach (tbDetailRemiseScelles items in listDetailremise)
                            {

                                if (item.Lot_Id == items.Lot_Id)
                                {
                                    foreach (CsScelle itemScel in ListScelles)
                                    {
                                        if (items.Id_Scelle == itemScel.Id_Scelle)
                                        {
                                            //Recuperer le scelle correspondant depuis la base
                                            leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)itemScel.Id_Scelle);
                                            leScellePose.Status_ID = 2;
                                            Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                                        }
                                    }
                                }
                            }

                        }
                    }


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }
        public List<tbDetailRemiseScelles> MettreAJourDetailRemiseScelles(List<CsRemiseScelles> listDetailremise)
        {
            try
            {
                CsScelle leScellePose = new CsScelle();
                List<CsScelle> ListScelles = RetourneScellesListe();
                List<tbDetailRemiseScelles> ListDetaille = new List<tbDetailRemiseScelles>();

                if (listDetailremise != null && listDetailremise.Count > 0)
                {
                    foreach (CsRemiseScelles item in listDetailremise)
                    {
                        if (item.Id_Scelle == Guid.Empty)
                        {
                            if (!string.IsNullOrEmpty(item.Lot_Id))
                            {
                                foreach (CsScelle items in ListScelles)
                                {
                                    //Recupere les identifiant des scllés poour un lot Donnée depuis la base
                                    if (item.Lot_Id == items.lot_ID)
                                    {
                                        leScellePose = ListScelles.FirstOrDefault(s => s.lot_ID == item.Lot_Id);
                                        item.Id_Scelle = leScellePose.Id_Scelle;
                                        var DetailRemise = new tbDetailRemiseScelles
                                        {
                                            Id_Remise = item.Id_Remise,
                                            Id_Scelle = items.Id_Scelle,
                                            Lot_Id = item.Lot_Id,
                                            Id_DetailRemise = Guid.NewGuid()
                                        };
                                        ListDetaille.Add(DetailRemise);
                                        //leScellePose.Status_ID = 4;
                                        //Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                                    }
                                }
                            }
                        }
                    }


                }
                return ListDetaille;

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }
        public void MettreAJourDeleteLeLotRemis(CsRemiseScelles sRemise)
        {
            try
            {
                CsTbLot lelot = new CsTbLot();
                List<CsTbLot> ListLots = RetourneLotDeScelle();

                if (sRemise.Lot_Id != null)
                {
                    //Recuperer le scelle correspondant depuis la base
                    lelot = ListLots.FirstOrDefault(s => s.lot_ID == sRemise.Lot_Id.ToString());
                    lelot.Status_lot_ID = 0;
                    Entities.UpdateEntity<Galatee.Entity.Model.tbLot>(Entities.ConvertObject<Galatee.Entity.Model.tbLot, CsTbLot>(lelot));
                }





            }
            catch (Exception ex)
            {

                throw ex;

            }

        }
        public void MettreAJourDELETELeScelleRemis(CsRemiseScelles sRemise)
        {
            try
            {
                CsScelle leScellePose = new CsScelle();
                List<CsScelle> ListScelles = RetourneScellesListe();

                foreach (CsScelle itemScel in ListScelles)
                {
                    if (sRemise.Id_Scelle == itemScel.Id_Scelle)
                    {
                        //Recuperer le scelle correspondant depuis la base
                        leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)itemScel.Id_Scelle);
                        leScellePose.Status_ID = 3;
                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                    }
                }

            }

            catch (Exception ex)
            {

                throw ex;

            }

        }

        public void MettreAJourLeLotRemis(List<CsRemiseScelles> sRemise)
        {
            try
            {
                CsTbLot lelot = new CsTbLot();
                List<CsTbLot> ListLots = RetourneLotDeScelle();
                if (sRemise != null && sRemise.Count > 0)
                {
                    foreach (CsRemiseScelles item in sRemise)
                    {

                        if (item.Lot_Id != null)
                        {
                            //Recuperer le scelle correspondant depuis la base
                            lelot = ListLots.FirstOrDefault(s => s.lot_ID == item.Lot_Id.ToString());
                            lelot.Status_lot_ID = 1;
                            Entities.UpdateEntity<Galatee.Entity.Model.tbLot>(Entities.ConvertObject<Galatee.Entity.Model.tbLot, CsTbLot>(lelot));
                        }
                    }


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

        }

        #endregion

        #region  Retour scelle

        public List<CsRetourScelles> SelectAllRetourScelle()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsRetourScelles>(ScelleProcedures.SCELLES_RetourScelleAgt_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRetourScelles> SelectAllRetrousScelleByid(Guid Id_remise)
        {
            try
            {


                List<CsRetourScelles> _lstretourScelle = new List<CsRetourScelles>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_RetourScelleAgt_RETOURNEById(Id_remise);
                _lstretourScelle = Entities.GetEntityListFromQuery<CsRetourScelles>(dt);

                return _lstretourScelle.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(CsRetourScelles sRetour)
        {
            try
            {
                MettreAJourDeteleLeScelleRetours(sRetour);
                Entities.DeleteEntity<Galatee.Entity.Model.TbDetailRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbDetailRetourScelles, CsRetourScelles>(sRetour));

                return Entities.DeleteEntity<Galatee.Entity.Model.TbRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sRetour));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsRetourScelles SelectRetoursScellesId(Guid sCompteur)
        {
            try
            {
                return Entities.GetEntityFromQuery<CsRetourScelles>(ScelleProcedures.SCELLES_RetourScelleAgt_RETOURNEById(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsRetourScelles> sCompteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.TbRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sCompteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsRetourScelles sRemise)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TbRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sRemise));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsRetourScelles> sRemise)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.TbRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sRemise));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsRetourScelles sRemise)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.TbRetourScelles>(Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sRemise));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(List<CsRetourScelles> sretour)
        {
            using (galadbEntities context = new galadbEntities())
            {
                try
                {
                    List<TbRetourScelles> listCompteurBta = (Entities.ConvertObject<Galatee.Entity.Model.TbRetourScelles, CsRetourScelles>(sretour));
                    List<TbDetailRetourScelles> listDetailremise = (Entities.ConvertObject<Galatee.Entity.Model.TbDetailRetourScelles, CsRetourScelles>(sretour));
                    MettreAJourLeScelleRetours(sretour);
                    Entities.InsertEntity<TbRetourScelles>(listCompteurBta, context);
                    Entities.InsertEntity<TbDetailRetourScelles>(listDetailremise, context);
                    return context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void MettreAJourLeScelleRetours(List<CsRetourScelles> sretour)
        {
            try
            {
                CsScelle leScellePose = new CsScelle();
                List<CsScelle> ListScelles = RetourneScellesListe();
                if (sretour != null && sretour.Count > 0)
                {
                    foreach (CsRetourScelles item in sretour)
                    {
                        foreach (CsScelle itemScel in ListScelles)
                        {
                            if (item.Id_Scelle == itemScel.Id_Scelle)
                            {
                                //Recuperer le scelle correspondant depuis la base
                                leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)itemScel.Id_Scelle);
                                leScellePose.Status_ID = item.Statut_Scelle;
                                Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                            }
                        }
                    }


                }
            }

            catch (Exception ex)
            {

                throw ex;

            }

        }

        public void MettreAJourDeteleLeScelleRetours(CsRetourScelles sretour)
        {
            try
            {
                CsScelle leScellePose = new CsScelle();
                List<CsScelle> ListScelles = RetourneScellesListe();

                foreach (CsScelle itemScel in ListScelles)
                {
                    if (sretour.Id_Scelle == itemScel.Id_Scelle)
                    {
                        //Recuperer le scelle correspondant depuis la base
                        leScellePose = ListScelles.FirstOrDefault(s => s.Id_Scelle == (Guid)itemScel.Id_Scelle);
                        leScellePose.Status_ID = 2;
                        Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
                    }
                }
            }



            catch (Exception ex)
            {

                throw ex;

            }

        }

        //public List<TbDetailRetourScelles> MettreAJourDetailRetoursScelles(List<CsRetourScelles> listDetailremise)
        //{
        //    try
        //    {
        //        CsScelle leScellePose = new CsScelle();
        //        List<CsScelle> ListScelles = RetourneScellesListe();
        //        List<TbDetailRetourScelles> ListDetaille = new List<TbDetailRetourScelles>();

        //        if (listDetailremise != null && listDetailremise.Count > 0)
        //        {
        //            foreach (CsRetourScelles item in listDetailremise)
        //            {

        //            item.Id_Scelle = leScellePose.Id_Scelle;
        //            var DetailRemise = new TbDetailRetourScelles
        //            {
        //                //Id_Remise = item.Id_Remise,
        //                //Id_Scelle = items.Id_Scelle,
        //                //Lot_Id = item.Lot_Id,
        //                //Id_DetailRemise = Guid.NewGuid()
        //            };
        //            ListDetaille.Add(DetailRemise);
        //            //leScellePose.Status_ID = 4;
        //            //Entities.UpdateEntity<Galatee.Entity.Model.Scelles>(Entities.ConvertObject<Galatee.Entity.Model.Scelles, CsScelle>(leScellePose));
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        return ListDetaille;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;

        //    }

        //}

        //public void MettreAJourLeLotRetours(List<CsRetourScelles> sRemise)
        //{
        //    try
        //    {
        //        CsTbLot lelot = new CsTbLot();
        //        List<CsTbLot> ListLots = RetourneLotDeScelle();
        //        if (sRemise != null && sRemise.Count > 0)
        //        {
        //            foreach (CsRemiseScelles item in sRemise)
        //            {

        //                if (item.Lot_Id != null)
        //                {
        //                    //Recuperer le scelle correspondant depuis la base
        //                    lelot = ListLots.FirstOrDefault(s => s.lot_ID == item.Lot_Id.ToString());
        //                    lelot.Status_lot_ID = 1;
        //                    Entities.UpdateEntity<Galatee.Entity.Model.tbLot>(Entities.ConvertObject<Galatee.Entity.Model.tbLot, CsTbLot>(lelot));
        //                }
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;

        //    }

        //}

        #endregion

        #region  Motifs
        public List<CsMotifsScelle> SelectAllMotifs()
        {
            try
            {
                List<CsMotifsScelle> _lstMotifs = new List<CsMotifsScelle>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_Motif_RETOURNE();
                _lstMotifs = Entities.GetEntityListFromQuery<CsMotifsScelle>(dt);


                return _lstMotifs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region tbLot

        public List<CsTbLot> RetourneLotDeScelle()
        {
            try
            {
                List<CsTbLot> _lstscelle = new List<CsTbLot>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_Lot_RETOURNE();
                _lstscelle = Entities.GetEntityListFromQuery<CsTbLot>(dt);


                return _lstscelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<CsTbLot> RetourneLotDeScelleAffectation(int IdCentre)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 360;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_SCELLE_LISTEDESLOTSCELLEPOURAFFECTATION";
                cmd.Parameters.Add("@IDCENTRE", SqlDbType.Int).Value = IdCentre;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<Structure.CsTbLot>(dt);
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

        #endregion

        #endregion

        #region Sylla

        public List<CsLotScelle> RetourneListeScelle(int IdCentreRecuperationDeLot)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 3000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.CommandText = "SPX_SCELLE_SELECT_LOTSCELLE";
            cmd.Parameters.Add("@IdCentreRecuperationDeLot", SqlDbType.Int).Value = IdCentreRecuperationDeLot;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsLotScelle>(dt);
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


        #endregion
        #region Ludovic 050416
        public List<CsRemiseScelleByAg> RetourneListScelleByStatus(int pk_ID, int Status)
        {
            try
            {
                List<CsRemiseScelleByAg> _lstScelle = new List<CsRemiseScelleByAg>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListScelleByStatus(pk_ID, Status);
                _lstScelle = Entities.GetEntityListFromQuery<CsRemiseScelleByAg>(dt);
                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsRemiseScelleByAg> RetourneListScelleByCentre(int idCentre)
        {
            try
            {
                List<CsRemiseScelleByAg> _lstScelle = new List<CsRemiseScelleByAg>();
                DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListScelleByCentre(idCentre);
                _lstScelle = Entities.GetEntityListFromQuery<CsRemiseScelleByAg>(dt);
                return _lstScelle;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsCompteurBta> RetourneCompteurBtaByNumCptNumScelle(string NumeroCompteur, string NumeroScelle)
        {
            try
            {
                if (!string.IsNullOrEmpty(NumeroScelle))
                {
                    DataTable dt = ScelleProcedures.SCELLES_CompteurBta_RETOURNEByNumScelle(NumeroScelle);
                    List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
                    return l;
                }
                else if (!string.IsNullOrEmpty(NumeroCompteur))
                {
                    DataTable dt = ScelleProcedures.SCELLES_CompteurBta_RETOURNEById(NumeroCompteur);
                    List<CsCompteurBta> l = Entities.GetEntityListFromQuery<CsCompteurBta>(dt);
                    return l;
                }
                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCompteurBta> RetourneCompteurMagasinVirtuel(string codeAgence)
        {
            try
            {
                DataTable dt = ScelleProcedures.RetourneListeCompteurMagasinCentre(codeAgence);
                return Entities.GetEntityListFromQuery<CsCompteurBta>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region ADO .Net from Entity : Stephen 26-01-2019
        //scelle.
        public List<CsActivite> RetourneListeActivite()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeActivite();
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("scelle.RefActivite");
                return Entities.GetEntityListFromQuery<CsActivite>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsRefOrigineScelle RefOrigineScelleByid(int Id_OrigineScelle)
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLE_RefOrigineScelle_RETOURNEById(Id_OrigineScelle);
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("scelle.RefOrigineScelle");
                return Entities.GetEntityFromQuery<CsRefOrigineScelle>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCouleurActivite> RetourneListAllCouleurScelle()//TriggerMenuView 29-01
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ScelleProcedures.RetourneListeAllCouleurScelle();
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsCouleurActivite> _LstCouleurActivite = new List<CsCouleurActivite>();
                _LstCouleurActivite = db.RetourneListeAllCouleurScelle();
                return _LstCouleurActivite;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsMarque_Modele> RetourneListeMarque_Modele()
        {
            try
            {
                //DataTable dt = Galatee.Entity.Model.ScelleProcedures.SCELLES_Marque_Modele_RETOURNE();
                //DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MARQUE_MODELE");
                //return Entities.GetEntityListFromQuery<CsMarque_Modele>(dt);
                DB_ParametresGeneraux db = new DB_ParametresGeneraux();
                List<CsMarque_Modele> _LstItem = new List<CsMarque_Modele>();
                _LstItem = db.RetourneMarqueModeleScelle();
                return _LstItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRefFournisseurs> SelectAllRefFournisseurs()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsRefFournisseurs>(ScelleProcedures.SCELLES_RefFournisseurs_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("scelle.RefFournisseurs");
                return Entities.GetEntityListFromQuery<CsRefFournisseurs>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsRefOrigineScelle> SelectAllRefOrigineScelle()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsRefOrigineScelle>(ScelleProcedures.SCELLES_RefOrigineScelle_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("scelle.RefOrigineScelle");
                return Entities.GetEntityListFromQuery<CsRefOrigineScelle>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region EtatCompteur
        public List<CsRefEtatCompteur> SelectAllEtatCompteur()
        {
            try
            {
                //return Entities.GetEntityListFromQuery<CsRefEtatCompteur>(ScelleProcedures.SCELLES_RefEtatCompteur_RETOURNE());
                DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("scelle.RefEtatCompteur");
                return Entities.GetEntityListFromQuery<CsRefEtatCompteur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        #endregion




    }

}