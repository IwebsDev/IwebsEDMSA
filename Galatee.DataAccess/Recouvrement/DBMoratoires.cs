using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using Galatee.Entity.Model;
using System.Threading.Tasks;
namespace Galatee.DataAccess
{
    public class DBMoratoires
    {
        private string ConnectionString;
        private SqlCommand cmd = null;
        private SqlConnection cn = null;
        SqlTransaction transaction = null;
        bool isRollback = false;

        public DBMoratoires()
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


        public bool? InsertionMoratoires(List<CsLclient> moratoires,List<CsLclient> lstFacture)
        {

            try
            {
                return InsertDetailsMoratoires(moratoires, lstFacture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? InsertChecqueImpayes(List<CsLclient> LignecompteClient)
        {
            try
            {
                foreach (CsLclient item in LignecompteClient)
                    item.NDOC = AccueilProcedures.NumeroFacture(item.FK_IDCENTRE);
                using (galadbEntities context = new galadbEntities())
                {
                    // insertion des impayes dans le compte client
                    List<LCLIENT> lesFact = ObtenirLclientChq(LignecompteClient);
                    Entities.InsertEntity<LCLIENT>(lesFact, context);
                    context.SaveChanges();
                    //InsererMauvaisPayeur(lstImpaye);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<aCampagne> RechercherCampagneParCoupure(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            List<aCampagne> lCampagnes = new List<aCampagne>();
            //cmd = new SqlCommand("SPX_LCLIENT_CAMPAGNE_SELECT_BY_IDCOUPURE", cn);

            try
            {
                DataTable dt = RecouvProcedures.RetourneCampagneCoupureIdCoupureNonSaisi(Campagne, leClientRech);
                lCampagnes = Entities.GetEntityListFromQuery<aCampagne>(dt);

                foreach (var c in lCampagnes)
                {
                    //aCampagne campagne = new aCampagne();
                    aCampagne camp = RechercherIndexParClient(c.IdCoupure, c.Centre, c.Client, c.Ordre);
                    if (camp != null)
                    {
                        if (c.DateRDV != null)
                            c.Observation = "RDV le " + c.DateRDV.Value.ToShortDateString();
                        else
                        {
                        }
                        c.IndexO = camp.IndexO;
                        c.IndexE = camp.IndexE;
                        c.DateCoupure = camp.DateCoupure;
                    }
                    //aSituation situation = RechercherImpayeCampagne(c.Centre, c.Client, c.Ordre, c.IdCoupure);

                    //if (situation != null)
                    //{
                    //    c.SoldeInitial = situation.SoldeInitial;
                    //    c.NbreFactImpayes = situation.NbreFactImpayes;
                    //    c.SoldeCourant = situation.SoldeCourant;
                    //    c.NbreFactRegle = situation.NbreFactRegle;
                    //    c.Frais = situation.Frais;
                    //    c.DateRemise = situation.DateRemise;

                    //    aParam critere = RechercherCritereCampagne(c.IdCoupure);
                    //    c.PeriodeRelance = critere.PeriodeRelance;
                    //    c.DateExigibilite = critere.DateExigibilite;
                    //    c.DebutTournee = critere.DebutTournee;
                    //    c.FinTournee = critere.FinTournee;
                    //    c.DebutOrdTournee = critere.DebutOrdTournee;
                    //    c.FinOrdTournee = critere.FinOrdTournee;
                    //    c.MontantRelance = critere.MontantRelance.Value;
                    //}

                }
                return lCampagnes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private bool InsererMauvaisPayeur(List<CsLclient> leChequeImpaye)
        {
            try
            {
                List<CsClient> lstClientResult = new List<CsClient>();
                var lstClientDistinct = leChequeImpaye.Select(t => new { t.CENTRE , t.CLIENT , t.ORDRE,t.FK_IDCLIENT   }).Distinct().ToList();
                foreach (var item in lstClientDistinct)
                    lstClientResult.Add(new CsClient { CENTRE = item.CENTRE, REFCLIENT = item.CLIENT, ORDRE = item.ORDRE, PK_ID  =item.FK_IDCLIENT });
                List<MAUVAISPAYEUR> lesMovaisPayeur = new List<MAUVAISPAYEUR>();
                foreach (CsClient item in lstClientResult)
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<LCLIENT> lstClient = context.LCLIENT.Where(c => c.FK_IDCLIENT == item.PK_ID &&
                                                         c.COPER == Enumere.CoperChqImp && 
                                                         !context.MAUVAISPAYEUR.Any(t=>t.FK_IDCLIENT == item.PK_ID)).ToList();
                        if (lstClient.Count >= 1)
                        {
                            MAUVAISPAYEUR x = new MAUVAISPAYEUR() 
                            {
                                FK_IDCLIENT = item.PK_ID ,
                                DATECREATION = System.DateTime.Now ,
                                DATEMODIFICATION =System.DateTime.Now ,
                                USERCREATION = leChequeImpaye.First().MATRICULE 
                            };
                            lesMovaisPayeur.Add(x);
                        }
                    } 
                }
                Entities.InsertEntity<MAUVAISPAYEUR>(lesMovaisPayeur);
                

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<LCLIENT> ObtenirLclientChq(List<CsLclient> LignecompteClient)
        {
            try
            {
                List<LCLIENT> client = new List<LCLIENT>();
                foreach (var c in LignecompteClient)
                {
                    LCLIENT cl = new LCLIENT()
                    {
                        CENTRE = c.CENTRE,
                        CLIENT = c.CLIENT,
                        ORDRE = c.ORDRE,
                        REFEM = c.REFEM,
                        NDOC = c.NDOC,
                        COPER = c.COPER,
                        DENR = c.DENR,
                        EXIG = c.EXIG,
                        MONTANT = c.MONTANT,
                        CAPUR = c.CAPUR,
                        CRET = c.CRET,
                        MODEREG = c.MODEREG,
                        DC = c.DC,
                        ORIGINE = c.ORIGINE,
                        CAISSE = c.CAISSE,
                        ECART = c.ECART,
                        MOISCOMPT = c.MOISCOMPT,
                        TOP1 = c.TOP1,
                        EXIGIBILITE = c.EXIGIBILITE,
                        FRAISDERETARD = c.FRAISDERETARD,
                        REFERENCEPUPITRE = c.REFERENCEPUPITRE,
                        IDLOT = c.IDLOT,
                        DATEVALEUR = c.DATEVALEUR,
                        REFERENCE = c.REFERENCE,
                        REFEMNDOC = c.REFEMNDOC,
                        ACQUIT = c.ACQUIT,
                        MATRICULE = c.MATRICULE,
                        TAXESADEDUIRE = c.TAXESADEDUIRE,
                        DATEFLAG = c.DATEFLAG,
                        MONTANTTVA = c.MONTANTTVA,
                        IDCOUPURE = c.IDCOUPURE,
                        AGENT_COUPURE = c.AGENT_COUPURE,
                        RDV_COUPURE = c.RDV_COUPURE,
                        NUMCHEQ = c.NUMCHEQ,
                        OBSERVATION_COUPURE = c.OBSERVATION_COUPURE,
                        USERCREATION = c.USERCREATION,
                        DATECREATION = c.DATECREATION,
                        DATEMODIFICATION = c.DATEMODIFICATION,
                        USERMODIFICATION = c.USERMODIFICATION,
                        BANQUE = c.BANQUE,
                        GUICHET = c.GUICHET,
                        FK_IDCLIENT = c.FK_IDCLIENT,
                        FK_IDCENTRE = c.FK_IDCENTRE,
                        FK_IDADMUTILISATEUR = c.FK_IDADMUTILISATEUR,
                        FK_IDCOPER = c.FK_IDCOPER,
                        FK_IDLIBELLETOP = c.FK_IDLIBELLETOP,
                        POSTE = c.POSTE,
                        FK_IDMOTIFCHEQUEINPAYE = c.FK_IDMOTIFCHEQUEINPAYE 
                    };
                    client.Add(cl);
                }
                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAvisDeCoupureClient > REEDITION_ETAT_AVIS_DE_COUPURE(CsCAMPAGNE laCampagne, bool IsListe)
        {
            try
            {
                return  RetourneDetailCampagneFromCampagne(laCampagne);
                //foreach (CsDetailCampagne item in leDetail)
                //{
                //    List<CsLclient> LstFactureImplique = RetourneFactureImpliqueCampagne(item);
                //    List<CsLclient> lstfactureEditer = new List<CsLclient>();
                //    //foreach (var c in LstFactureImplique.Take(5).ToList())
                //        foreach (var c in LstFactureImplique.ToList())
                //    {
                //        //Parallel.ForEach(LstFactureImplique.Take(5).ToList(), c =>
                //        //{
                //            if (c != null)
                //            {
                //                if (c.COPER != Enumere.CoperRNA && c.DC == Enumere.Debit)
                //                    c.SOLDEFACTURE = FonctionCaisse.RetourneSoldeDocument(c.FK_IDCENTRE, c.CENTRE, c.CLIENT, c.ORDRE, c.NDOC, c.REFEM);
                //                lstfactureEditer.Add(c);
                //            }
                //        //});
                //    }
                //    if(LstFactureImplique.Count != 0)
                //        lstAvisAEditer.AddRange(ConvertToDisconnexionObjetReprint(item, laCampagne, lstfactureEditer, IsListe));
                //}
                //return lstAvisAEditer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<aDisconnection> ETAT_AVIS_DE_COUPUREGC(CsAvisCoupureEdition aviscoupure, aDisconnection dis, bool IsListe)
        //{
        //    //return new List<aDisconnection>();
        //    try
        //    {
        //        List<CsLclient> _lstFactureMiseAJour = new List<CsLclient>();
        //        List<CsClient> clientAvisCampagne = new List<CsClient>();
        //        List<int?> tempPkiClient = new List<int?>();
        //        DataTable dt = RecouvProcedures.RetourneElementAvisCoupureGC(aviscoupure);
        //        List<CsClient> clientsAvis = Entities.GetEntityListFromQuery<CsClient>(dt);
        //        List<CsRegCli> lstRegroupementAfficher = new List<CsRegCli>();
        //        foreach (CsRegCli item in aviscoupure.ListeRegroupement)
        //        {
        //            List<CsClient> clientsAvisRegroupement = clientsAvis.Where(t => t.FK_IDREGROUPEMENT == item.PK_ID).ToList();
        //            //if (aviscoupure.ClientResilie == true) clientsAvis = clientsAvis.Where(t => t.DRES != null).ToList();

        //            List<int> lstIdRegcli = new List<int>();
        //            lstIdRegcli.Add(item.PK_ID);
        //            List<CsLclient> lstFactureClient = new DBEncaissement().RetourneListeFactureNonSoldeByRegroupementId(lstIdRegcli);
        //            lstFactureClient.ForEach(t => t.FK_IDREGROUPEMENT = item.PK_ID);
        //            if (lstFactureClient == null || lstFactureClient.Count == 0) continue;
        //            if (aviscoupure.SoldeClient != null && aviscoupure.SoldeClient > 0)
        //            {
        //                if (lstFactureClient.Where(t => t.EXIGIBILITE <= aviscoupure.Exigible).Sum(y => y.SOLDEFACTURE) >= aviscoupure.SoldeClient)
        //                {
        //                    foreach (CsClient leClient in clientsAvis)
        //                    {
        //                        _lstFactureMiseAJour.AddRange(lstFactureClient);
        //                        leClient.TOTALFACUTREDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).ToList().Count;
        //                        leClient.SOLDEDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID && t.EXIGIBILITE <= aviscoupure.Exigible).Sum(t => t.SOLDEFACTURE);
        //                        leClient.MATRICULE = aviscoupure.Matricule;
        //                        leClient.SOLDE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).Sum(t => t.SOLDEFACTURE);
        //                        aviscoupure.MontantRelancable = aviscoupure.SoldeClient;
        //                        clientAvisCampagne.Add(leClient);
        //                    }
        //                }
        //            }
        //            if (aviscoupure.MontantPeriode != null && aviscoupure.MontantPeriode != 0 &&
        //                aviscoupure.Periode != null && !string.IsNullOrEmpty(aviscoupure.Periode))
        //            {
        //                if (lstFactureClient.First().SOLDECLIENT > 0)
        //                {
        //                    _lstFactureMiseAJour.AddRange(lstFactureClient.Where(t => t.REFEM == aviscoupure.Periode && t.SOLDEFACTURE >= aviscoupure.MontantPeriode && t.EXIGIBILITE <= aviscoupure.Exigible));
        //                    foreach (CsClient leClient in clientsAvis)
        //                    {
        //                        leClient.TOTALFACUTREDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).ToList().Count;
        //                        leClient.SOLDEDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID && t.EXIGIBILITE <= aviscoupure.Exigible).Sum(t => t.SOLDEFACTURE);
        //                        leClient.MATRICULE = aviscoupure.Matricule;
        //                        leClient.SOLDE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).Sum(t => t.SOLDEFACTURE);
        //                        aviscoupure.MontantRelancable = aviscoupure.MontantPeriode;
        //                        clientAvisCampagne.Add(leClient);
        //                    }
        //                }
        //            }
        //            if (aviscoupure.SoldeClient != null && aviscoupure.SoldeClient != 0 &&
        //                aviscoupure.TotalClient != null && aviscoupure.TotalClient != 0)
        //            {
        //                if (lstFactureClient.First().SOLDECLIENT > 0)
        //                {
        //                    if (lstFactureClient.Count >= aviscoupure.TotalClient && lstFactureClient.Where(t => t.EXIGIBILITE <= aviscoupure.Exigible).Sum(t => t.SOLDEFACTURE) >= aviscoupure.SoldeClient)
        //                        _lstFactureMiseAJour.AddRange(lstFactureClient);

        //                    foreach (CsClient leClient in clientsAvis)
        //                    {
        //                        leClient.TOTALFACUTREDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).ToList().Count;
        //                        leClient.SOLDEDUE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID && t.EXIGIBILITE <= aviscoupure.Exigible).Sum(t => t.SOLDEFACTURE);
        //                        leClient.MATRICULE = aviscoupure.Matricule;
        //                        leClient.SOLDE = lstFactureClient.Where(t => t.FK_IDCLIENT == leClient.PK_ID).Sum(t => t.SOLDEFACTURE);
        //                        aviscoupure.MontantRelancable = aviscoupure.SoldeClient;
        //                        clientAvisCampagne.Add(leClient);
        //                    }
        //                }
        //            }
        //            if (aviscoupure.TotalFacture != null && aviscoupure.TotalFacture != 0)
        //                if (clientAvisCampagne.Count == aviscoupure.TotalFacture) return null;
        //            lstRegroupementAfficher.Add(item);
        //        }



        //        // retourner l'identifiant de coupure
        //        List<CsClient> ListeCampagneAInserer = new List<CsClient>();
        //        if (!aviscoupure.IsReedition)
        //        {
        //            string idcoupure = CommonProcedures.RetourneIdCampagneCoupure(aviscoupure.Centre);
        //            if (idcoupure.Length != 16)
        //                idcoupure = aviscoupure.Site + aviscoupure.Centre + idcoupure;
        //            aviscoupure.IdCoupure = idcoupure;
        //            List<LCLIENT> Comptes = Entities.ConvertObject<LCLIENT, CsLclient>(_lstFactureMiseAJour);
        //            aviscoupure.TotalClient = clientAvisCampagne.Count;
        //            aviscoupure.TotalFacture = _lstFactureMiseAJour.Count();
        //            CAMPAGNE camp = RetourneCampagne(idcoupure, aviscoupure);
        //            camp.DETAILCAMPAGNE = RetourneDetailCampagne(clientAvisCampagne, idcoupure);
        //            //if (!RecouvProcedures.SaveCampagne(camp, _lstFactureMiseAJour))
        //            //    throw new Exception("Erreur lors de la mise a jour de la campgane");
        //        }
        //        if (IsListe)
        //            return ConvertToDisconnexionObjetSGC(clientAvisCampagne, aviscoupure);
        //        else
        //            return ConvertToDisconnexionObjetRegroupement(aviscoupure, _lstFactureMiseAJour);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
   
        private void MiseAjourLclientCampagneDetailCampage(List<CsAvisDeCoupureClient> lstAvis, CsAvisCoupureEdition aviscoupure)
        {
            try
            {
                string idcoupure = CommonProcedures.RetourneIdCampagneCoupure(aviscoupure);
                if (idcoupure.Length != 10)
                {
                    string Valeur = idcoupure.Substring(idcoupure.Length - 10, 10);
                    idcoupure = aviscoupure.Site + aviscoupure.Centre + Valeur;
                }
                else
                    idcoupure = aviscoupure.Site + aviscoupure.Centre + idcoupure;
                aviscoupure.IdCoupure = idcoupure;
                 
                CsMapperCampagne  camp = RetourneCampagne(idcoupure, aviscoupure);
                List<CsMapperDetailCampagne> detailCampagne = RetourneDetailCampagne(lstAvis, idcoupure);

                foreach (CsMapperDetailCampagne item in detailCampagne)
                {
                    item.IDCOUPURE = idcoupure;
                    item.DATECREATION  = System.DateTime.Today;
                    item.USERCREATION = aviscoupure.Matricule ;
                }

                SqlCommand laCommande = DBBase.InitTransaction(ConnectionString);
                try
                {
                    SaveCampagneBulk(camp, detailCampagne, lstAvis, cmd);
                    laCommande.Transaction.Commit();
                }
                catch (Exception ex)
                {
                    laCommande.Transaction.Rollback();
                    throw ex;
                }
                //if (!RecouvProcedures.SaveCampagne(camp, lstAvis))
                //    throw new Exception("Erreur lors de la mise a jour de la campgane");
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void SaveCampagneBulk(CsMapperCampagne camp ,List<CsMapperDetailCampagne> detailCampagne, List<CsAvisDeCoupureClient> lesFacture, SqlCommand pContext)
        {
            try
            {
                    List<CsMapperLclientCoupure> lesFactures = Galatee.Tools.Utility.ConvertListType<CsMapperLclientCoupure, CsAvisDeCoupureClient>(lesFacture);
                    foreach (var item in lesFactures)
                    {
                        item.IDCOUPURE = camp.IDCOUPURE;
                        item.PK_ID = item.FK_IDLCLIENT;
                        //item.DATEMODIFICATION = System.DateTime.Today;
                        //item.USERCREATION = camp.USERMODIFICATION;
                    }
                    pContext.CommandTimeout = 18000;
                    Dictionary<string, DataTable> TableFille = new Dictionary<string, DataTable>();
                    List<CsMapperCampagne> lstPere = new List<CsMapperCampagne>();
                    lstPere.Add(camp);
                    DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(lstPere);
                    DataTable TableFille1 = Galatee.Tools.Utility.ListToDataTable(detailCampagne);

                    TableFille.Add("DETAILCAMPAGNE", TableFille1);
                    Galatee.Tools.Utility.BulkInsertManyToManyRelationship(TablePere, "CAMPAGNE", TableFille, "FK_IDCAMPAGNE", pContext);
                    UpdateEvtfacteBulk(lesFactures, pContext);


                    //BulkUpdateData(lesFacture, "LCLIENT", "FK_IDLCLIENT", pContext);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEvtfacteBulk(List<CsMapperLclientCoupure> _LesFacture, SqlCommand pContext)
        {
            try
            {
                List<string> ProprieteAModifier = new List<string>();
                ProprieteAModifier.Add("IDCOUPURE");
                //ProprieteAModifier.Add("DATEMODIFICATION");

                List<string> ProprieteDeJointure = new List<string>();
                ProprieteDeJointure.Add("PK_ID");

                string Sufixe = string.Empty;
                var properties = _LesFacture.First().GetType().GetProperties();

                int NbrPropertie = properties.Count();
                int Passage = 0;
                foreach (var f in properties) // Récuperation des valeurs des propriete de l'objet
                {
                    Passage += 1;
                    var type = f.PropertyType.FullName;
                    string TypeVal = type.ToString();
                    if (type.Contains("System.String"))
                        TypeVal = "Varchar(30)";
                    else if (type.Contains("System.Int32") || type.Contains("System.Int16"))
                        TypeVal = "int";
                    else if (type.Contains("System.Boolean"))
                        TypeVal = "bit";
                    else if (type.Contains("System.DateTime"))
                        TypeVal = "datetime";
                    else if (type.Contains("System.Decimal"))
                        TypeVal = "numeric(38, 10)";
                    else if (type.Contains("System.Byte"))
                        TypeVal = "tinyint";

                    if (Passage == NbrPropertie)
                        Sufixe += f.Name + " " + TypeVal + " NULL ";
                    else
                        Sufixe += f.Name + " " + TypeVal + " NULL " + " ,";
                }
                string CreationTableTemp = "CREATE TABLE #TmpTable( " + Sufixe + ")";

                DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(_LesFacture);
                Galatee.Tools.Utility.UpdateData(ProprieteAModifier, ProprieteDeJointure, TablePere, "LCLIENT", CreationTableTemp, ConnectionString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool BulkUpdateData<T>(IEnumerable<T> pListObject, string TableName, string idTable, SqlCommand command)
        {
            DataTable dataTable;
            DataRow newDataRow;

            DataTable pDtSource = null;

            pDtSource = Galatee.Tools.Utility.ListToDataTable(pListObject);

            System.Collections.ObjectModel.Collection<DataRow> dataRowsList = new System.Collections.ObjectModel.Collection<DataRow>();
            DataRow dataRowsObjet;

            DataRow[] rows = pDtSource.Select();

            foreach (DataRow item in rows)
            {
                dataRowsObjet = item;

                dataRowsList.Add(dataRowsObjet);
            }

            try
            {
                //Indication du nom de l'entité
                TableName = string.Format("{0}", TableName.ToLower().Trim());
                dataTable = new DataTable(TableName);

                #region /*Formattage de la requete de mise à jour*/

                StringBuilder strCreateTableStatement = new StringBuilder("CREATE TABLE #TmpTable(");
                StringBuilder strUpdateTargetTableStatement = new StringBuilder("UPDATE target_table SET ");
                string strTableKey = string.Format("{0}", idTable);

                foreach (DataColumn dataColumn in dataRowsList.First().Table.Columns)
                {
                    //retirer la clé primaire de la table parmi les champs à mettre à jour
                    if (!dataColumn.ColumnName.Equals(strTableKey, StringComparison.InvariantCultureIgnoreCase))
                        strUpdateTargetTableStatement.Append(string.Format(" target_table.{0} = source_table.{0},", dataColumn.ColumnName));

                    if (dataColumn.DataType == typeof(System.Guid))
                    {
                        strCreateTableStatement.Append(string.Format("{0} UNIQUEIDENTIFIER NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Guid));
                    }
                    else if (dataColumn.DataType == typeof(System.Int32))
                    {
                        strCreateTableStatement.Append(string.Format("{0} INT NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Int32));
                    }
                    else if (dataColumn.DataType == typeof(System.DateTime))
                    {
                        strCreateTableStatement.Append(string.Format("{0} DATETIME NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.DateTime));
                    }
                    else if (dataColumn.DataType == typeof(System.Decimal))
                    {
                        strCreateTableStatement.Append(string.Format("{0} money NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Decimal));
                    }
                    else if (dataColumn.DataType == typeof(System.Boolean))
                    {
                        strCreateTableStatement.Append(string.Format("{0} BIT NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.Boolean));
                    }
                    else
                    {
                        strCreateTableStatement.Append(string.Format("{0} VARCHAR(MAX) NULL,", dataColumn.ColumnName));
                        dataTable.Columns.Add(dataColumn.ColumnName, typeof(System.String));
                    }
                }

                //Pour chacune des lignes à insérer
                foreach (DataRow dataRow in dataRowsList)
                {
                    //on créé une nouvelle ligne
                    newDataRow = dataTable.NewRow();

                    //on parcourt les colonnes pour en affecter les valeurs
                    foreach (DataColumn item in dataRow.Table.Columns)
                    {
                        newDataRow[item.ColumnName] = item.DataType == typeof(System.DateTime) ? DateTime.Now : dataRow[item.ColumnName];
                    }

                    dataTable.Rows.Add(newDataRow);
                }

                strCreateTableStatement.Remove(strCreateTableStatement.Length - 1, 1);
                strUpdateTargetTableStatement.Remove(strUpdateTargetTableStatement.Length - 1, 1);
                strCreateTableStatement.Append(")");

                strUpdateTargetTableStatement.Append(string.Format(" FROM #TmpTable source_table INNER JOIN {0} target_table ON target_table.{1} = source_table.{1};  DROP TABLE #TmpTable;", dataTable.TableName, idTable));

                #endregion /*Formattage de la requete de mise à jour*/
                try
                {

                    //Création de table temporaire

                    command.CommandText = strCreateTableStatement.ToString();
                    command.ExecuteNonQuery();

                    //Insertion dans la table temporaire


                    using (var bulkcopy = new SqlBulkCopy(command.Connection, SqlBulkCopyOptions.TableLock, command.Transaction))
                    {
                        bulkcopy.BulkCopyTimeout = 30000;
                        bulkcopy.DestinationTableName = "#TmpTable";
                        bulkcopy.WriteToServer(dataTable);
                        bulkcopy.Close();
                    }

                    //Mise à jour dans la table de production
                    command.CommandTimeout = 30000;
                    command.CommandText = strUpdateTargetTableStatement.ToString();

                    //Exxécution
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlException)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                    // Handle exception properly
                }
                finally
                {
                    command.Dispose();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private CsMapperCampagne   RetourneCampagne(string idcoupure, CsAvisCoupureEdition coupure)
        {
            try
            {
                string tourneeDebut = string.Empty;
                string tourneeFin = string.Empty;
                string CategorieFin = string.Empty;
                string CategorieDebut = string.Empty;
                //galadbEntities contextInter = new galadbEntities();
                CsMapperCampagne camp = new CsMapperCampagne();
                camp.CENTRE = coupure.Centre;
                camp.MONTANT_RELANCABLE = coupure.MontantRelancable;
                camp.PERIODE_RELANCABLE = coupure.Periode;
                camp.DATE_EXIGIBILITE = coupure.Exigible;
                camp.PREMIERE_TOURNEE = coupure.TourneeDebut;
                camp.DERNIERE_TOURNEE = coupure.TourneeFin ;
                camp.DEBUT_CATEGORIE = coupure.CategorieDebut;
                camp.MONTANT_RELANCABLE = coupure.SoldeMinimum;
                camp.FIN_CATEGORIE = coupure.CategorieFin;
                camp.FIN_AG = coupure.MatriculeFin;
                camp.NOMBRE_CLIENT = coupure.NombreTotalDeClient.ToString();
                camp.NOMBRE_FACTURE = coupure.NombreFactureTotalClient.ToString();
                camp.DEBUT_AG = coupure.MatriculeDebut;
                camp.MATRICULEPIA = coupure.AgentPia;
                camp.IDCOUPURE = idcoupure;
                //camp.FK_REGROUPEMENT  = coupure.idRegroupement== 0? null : (int?)coupure.idRegroupement  ;
                camp.DATECREATION = DateTime.Now.Date;
                camp.USERCREATION = coupure.Matricule;
                CsUtilisateur leUser = new DBAuthentification().GetUserByMatricule(camp.MATRICULEPIA);

                camp.FK_IDCENTRE = coupure.idCentre;
                camp.FK_IDMATRICULE = leUser.PK_ID;
                //contextInter.Dispose();
                return camp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsMapperDetailCampagne> RetourneDetailCampagne(List<CsAvisDeCoupureClient> clientAvisCampagne, string idcoupure)
        {
            try
            {
                List<CsMapperDetailCampagne> lstDetailCampagne = new List<CsMapperDetailCampagne>();
                lstDetailCampagne = Entities.ConvertObject<CsMapperDetailCampagne, CsAvisDeCoupureClient>(clientAvisCampagne);
                return lstDetailCampagne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<LCLIENT> RetournerCompteClient(CsClient cs, string idcoupure)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    List<LCLIENT> cmpteClient = context.LCLIENT.Where(l => l.CENTRE == cs.CENTRE && l.CLIENT == cs.REFCLIENT
                                                                             && l.ORDRE == cs.ORDRE && l.NDOC == cs.NDOC && cs.EXIGIBILITE == l.EXIGIBILITE).ToList();
                    cmpteClient.ForEach(f => f.IDCOUPURE = idcoupure);
                    cmpteClient.ForEach(f => f.AGENT_COUPURE = cs.AGENTRECOUVR);

                    return cmpteClient;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<aDisconnection> ConvertToDisconnexionObjetReprint(CsDetailCampagne clientAvisCampagne, CsCAMPAGNE avis, List<CsLclient> _lstfacture, bool isliste)
        {
            try
            {
                

                List<aDisconnection> listes = new List<aDisconnection>();
                if (isliste)
                {
                        aDisconnection a = new aDisconnection();
                        a.CENTRE = clientAvisCampagne.CENTRE;
                        a.CLIENT = clientAvisCampagne.CLIENT;
                        a.ORDRE = clientAvisCampagne.ORDRE;
                        a.TOURNEE = clientAvisCampagne.TOURNEE;
                        a.NOMABON = clientAvisCampagne.NOMABON;
                        a.Adresse = clientAvisCampagne.ADRESSE ;
                        a.SOLDEDUE = clientAvisCampagne.SOLDEDUE == null ? 0 : clientAvisCampagne.SOLDEDUE.Value;
                        a.BILLSDUE = clientAvisCampagne.NOMBREFACTURE == null ? 0 : clientAvisCampagne.NOMBREFACTURE.Value;
                        a.SOLDETOTAL = clientAvisCampagne.SOLDECLIENT.Value;
                        a.COMPTEURELECT = clientAvisCampagne.COMPTEUR;
                        a.NomControler = avis.AGENTPIA;
                        a.IdCoupure = clientAvisCampagne.IDCOUPURE .ToString();
                        listes.Add(a);
                    return listes;
                }
                foreach (var c in _lstfacture)
                {
                    aDisconnection a = new aDisconnection();
                    a.CENTRE = clientAvisCampagne.CENTRE;
                    a.CLIENT = clientAvisCampagne.CLIENT;
                    a.ORDRE =clientAvisCampagne.ORDRE;
                    a.TOURNEE = clientAvisCampagne.TOURNEE;
                    a.NOMABON = clientAvisCampagne.NOMABON;
                    a.CATCLI = clientAvisCampagne.CATEGORIECLIENT;
                    a.Adresse = clientAvisCampagne.ADRESSE;
                    a.SOLDEDUE = clientAvisCampagne.SOLDEDUE == null ? 0 : clientAvisCampagne.SOLDEDUE.Value;
                    a.BILLSDUE = clientAvisCampagne.NOMBREFACTURE == null ? 0 : clientAvisCampagne.NOMBREFACTURE.Value;
                    a.SOLDETOTAL = clientAvisCampagne.SOLDECLIENT == null ? 0 : clientAvisCampagne.SOLDECLIENT.Value;
                    a.COMPTEURELECT = clientAvisCampagne.COMPTEUR;
                    a.PERIODEFACTURE = c.REFEM;
                    a.NDOC = c.NDOC;
                    a.MONTANTFACTURE = c.MONTANT;
                    a.SOLDEFACTURE = c.SOLDEFACTURE;
                    a.NomControler = avis.AGENTPIA;
                    a.COMPTEURWATER = c.LIBELLENATURE;
                    a.IdCoupure = avis.IDCOUPURE.ToString();
                    a.ORDTOUR = clientAvisCampagne.ORDTOUR;
                    listes.Add(a);
                }
             return  listes.OrderBy(t => t.TOURNEE).ThenBy(y => y.ORDTOUR).ToList();

            }
            catch (Exception ex)
            {
                string cli = clientAvisCampagne.CLIENT;
                throw ex;
            }
        }

        public List<aDisconnection> ConvertToDisconnexionObjetRegroupement( CsAvisCoupureEdition avis, List<CsLclient> _lstfacture)
        {
            try
            {
                List<aDisconnection> lstRegroupement = new List<aDisconnection>();
                foreach (CsRegCli  item in avis.ListeRegroupement )
                {
                    aDisconnection a = new aDisconnection();
                    a.REGROUPEMENT = item.CODE;
                    a.LIBELLEREGROUPEMENT = item.NOM;
                    a.ADRESSEREGROUPEMENT = item.ADR1 + item.ADR2;
                    a.SOLDEDUE = _lstfacture.Where(y=>y.FK_IDREGROUPEMENT == item.PK_ID).Sum(t => t.SOLDEFACTURE.Value);
                    a.BILLSDUE = _lstfacture.Where(y => y.FK_IDREGROUPEMENT == item.PK_ID).Count();
                    a.SOLDETOTAL = _lstfacture.Where(y => y.FK_IDREGROUPEMENT == item.PK_ID).Sum(t => t.SOLDEFACTURE.Value);
                    a.NomControler = avis.NomAgentPia;
                    a.IdCoupure = avis.IdCoupure.ToString();
                    lstRegroupement.Add(a);
                }

                return lstRegroupement;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<aDisconnection> ConvertToDisconnexionObjet(List<CsClient> clientAvisCampagne, CsAvisCoupureEdition avis, List<CsLclient> _lstfacture)
        {
            try
            {
                List<aDisconnection> listes = new List<aDisconnection>();
                foreach (var c in clientAvisCampagne)
                {
                    List<CsLclient> lesFactureAEditer = _lstfacture.Where(t => t.CENTRE == c.CENTRE && t.CLIENT == c.REFCLIENT && t.ORDRE == c.ORDRE && t.FK_IDCENTRE == c.FK_IDCENTRE && t.SOLDEFACTURE > 50).ToList();
                    //if (lesFactureAEditer.Count > 5)
                    //    lesFactureAEditer = lesFactureAEditer.Take(5).ToList();
                    foreach (CsLclient item in lesFactureAEditer)
                    {
                        aDisconnection a = new aDisconnection();
                        a.CENTRE = c.CENTRE;
                        a.CLIENT = c.REFCLIENT;
                        a.ORDRE = c.ORDRE;
                        a.TOURNEE = c.TOURNEE;
                        a.NOMABON = c.NOMABON;
                        a.CATCLI = c.LIBELLECATEGORIE ;
                        a.REGROUPEMENT = c.REGROUPEMENT;
                        a.Adresse = c.ADRMAND1;
                        a.SOLDEDUE = c.SOLDEDUE == null ? 0 : c.SOLDEDUE.Value;
                        a.BILLSDUE = c.TOTALFACUTREDUE == null ? 0 : c.TOTALFACUTREDUE.Value;
                        a.SOLDETOTAL = c.SOLDE == null ? 0 : c.SOLDE.Value;
                        a.COMPTEURWATER = item.LIBELLENATURE;
                        a.COMPTEURELECT  = c.COMPTEUR;
                        a.PERIODEFACTURE = item.REFEM;
                        a.NDOC = item.NDOC;
                        a.MONTANTFACTURE = item.MONTANT;
                        a.SOLDEFACTURE = item.SOLDEFACTURE;
                        a.NomControler = avis.NomAgentPia;
                        a.IdCoupure = avis.IdCoupure.ToString();
                        a.RUE = c.RUE;
                        a.PORTE = c.PORTE ;
                        a.CODECONSO = c.CODECONSO;
                        a.ORDTOUR = c.ORDTOUR;
                        a.SOLDENAF = c.SOLDENAF;
                        listes.Add(a);
                    }
                }
                return listes.OrderBy(t=>t.TOURNEE ).ThenBy(y=>y.ORDTOUR ).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<aDisconnection> ConvertToDisconnexionObjetSGC(List<CsClient> clientAvisCampagne, CsAvisCoupureEdition avis)
        {
            try
            {
                List<aDisconnection> listes = new List<aDisconnection>();
                foreach (var c in clientAvisCampagne)
                {
                    aDisconnection a = new aDisconnection();
                    CsRegCli leRegroup = avis.ListeRegroupement.FirstOrDefault(t => t.PK_ID == c.FK_IDREGROUPEMENT);
                    if (leRegroup != null)
                    {
                        a.REGROUPEMENT = leRegroup.CODE;
                        a.LIBELLEREGROUPEMENT = leRegroup.NOM;
                        a.ADRESSEREGROUPEMENT = leRegroup.ADR1 + leRegroup.ADR2;
                    }
                    a.CENTRE = c.CENTRE;
                    a.CLIENT = c.REFCLIENT;
                    a.ORDRE = c.ORDRE;
                    a.TOURNEE = c.TOURNEE;
                    a.NOMABON = c.NOMABON;
                    //a.CATCLI = c.CATEGORIE;
                    a.Adresse = c.ADRMAND1;
                    a.SOLDEDUE = c.SOLDEDUE == null ? 0 : c.SOLDEDUE.Value;
                    a.BILLSDUE = c.TOTALFACUTREDUE == null ? 0 : c.TOTALFACUTREDUE.Value;
                    a.SOLDETOTAL = c.SOLDE == null ? 0 : c.SOLDE.Value;
                    a.COMPTEURELECT = c.COMPTEUR;
                    a.NomControler = avis.NomAgentPia;
                    a.IdCoupure = avis.IdCoupure.ToString();
                    listes.Add(a);
                }

                return listes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<aDisconnection> ConvertToDisconnexionObjet(List<CsClient> clientAvisCampagne, CsAvisCoupureEdition avis)
        {
            try
            {
                List<aDisconnection> listes = new List<aDisconnection>();
                foreach (var c in clientAvisCampagne)
                {
                    aDisconnection a = new aDisconnection();

                    a.CENTRE = c.CENTRE;
                    a.CLIENT = c.REFCLIENT;
                    a.ORDRE = c.ORDRE;
                    a.TOURNEE = c.TOURNEE;
                    a.NOMABON = c.NOMABON;
                    //a.CATCLI = c.CATEGORIE;
                    a.Adresse = c.ADRMAND1;
                    a.SOLDEDUE = c.SOLDEDUE == null ? 0 : c.SOLDEDUE.Value;
                    a.BILLSDUE = c.TOTALFACUTREDUE == null ? 0 : c.TOTALFACUTREDUE.Value;
                    a.SOLDETOTAL = c.SOLDE == null ? 0 : c.SOLDE.Value;
                    a.COMPTEURELECT = c.COMPTEUR;
                    a.NomControler = avis.NomAgentPia;
                    a.IdCoupure = avis.IdCoupure.ToString();
                    listes.Add(a);
                }

                return listes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CsUtilisateur> RetourneListeUtiliasteurPia(int IdCentre, string CodeFonction)
        {
            try
            {
                DataTable dt = CommonProcedures.RetourneListeUtiliasteurPia(IdCentre, CodeFonction);
                return Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTournee > RetourneTourneePIA(List<int> lstIdCentre)
        {
            //cmd.CommandText = "SPX_RPT_Dis_Zone";

            try
            {
                DataTable dt = CommonProcedures.RetourneListeTourneePia(lstIdCentre);
                return Entities.GetEntityListFromQuery<CsTournee>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsTournee> RetourneTourneeParPIA(string  codeSite)
        {
            //cmd.CommandText = "SPX_RPT_Dis_Zone";

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_TOURNEEPIA";
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
        public List<CsUtilisateur> RetournePIAAgence(string codeSite)
        {
            //cmd.CommandText = "SPX_RPT_Dis_Zone";

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_RECHERCHEPIAAGENCE";
                cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = codeSite;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsUtilisateur>(dt);
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
        
        public List<CsTournee> RetourneTourneeFromCampagne(List<CsCAMPAGNE> lstCampagne)
        {

            try
            {
                DataTable dt = RecouvProcedures.RetourneTourneeFromCampagne(lstCampagne);
                return Entities.GetEntityListFromQuery<CsTournee>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsLclient VerifierChecqueImpayes(string numChq, string banque)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_VERIFIECHEQUESAISI";
                cmd.Parameters.Add("@NumChq", SqlDbType.VarChar, 9).Value = numChq;
                cmd.Parameters.Add("@Banque", SqlDbType.VarChar, 10).Value = banque;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityFromQuery<CsLclient>(dt);
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
        public List<CsLclient> RetourneReglementDuChecqhe(string numChq, string banque, string guichet)
        {
            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_RETOURNEREGLEMENTCHEQUE";
                cmd.Parameters.Add("@NumChq", SqlDbType.VarChar, 9).Value = numChq;
                cmd.Parameters.Add("@Banque", SqlDbType.VarChar, 10).Value = banque;
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
                throw ex;
            }
        }
        public List<CsDetailCampagne> ListeDesChequeImpayes(List<int> lstIdCentre, DateTime? dateDebut, DateTime? dateFin, int NombreAuccurrance)
        {
            try
            {

                string LstIdCentre = DBBase.RetourneStringListeObject(lstIdCentre); 

                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_LISTEDESCHEQUEIMPAYES";
                cmd.Parameters.Add("@IDCENTRELIST", SqlDbType.VarChar, int.MaxValue).Value = LstIdCentre;
                cmd.Parameters.Add("@DATEDEBUT", SqlDbType.DateTime).Value = dateDebut;
                cmd.Parameters.Add("@DATEFIN", SqlDbType.DateTime).Value = dateFin;
                cmd.Parameters.Add("@OCCURRENCE", SqlDbType.Int).Value = NombreAuccurrance;
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
                throw ex;
            }
        }

        public bool? UpdateDetailMoratoire(List<CsDetailMoratoire> lstMoratoire)
        {
            try
            {
                using (galadbEntities cmd = new galadbEntities())
                {
                    foreach (var item in lstMoratoire)
	                {

                        DETAILMORATOIRE leMoratoire = cmd.DETAILMORATOIRE.FirstOrDefault(t => t.FK_IDMORATOIRE == item.IDMORATOIRE &&
                                                                                            //t.REFEM == item.REFEM &&
                                                                                            t.NDOC == item.NDOC );
                        if (leMoratoire != null)
                        {
                            leMoratoire.MONTANT = item.MONTANT;
                            leMoratoire.EXIGIBILITE = item.EXIGIBILITE;

                            LCLIENT leLClient = cmd.LCLIENT.FirstOrDefault(t => t.FK_IDMORATOIRE == item.IDMORATOIRE &&
                                                                                          //t.REFEM == item.REFEM &&
                                                                                          t.NDOC == item.NDOC);
                            if (leLClient != null)
                            {
                                leLClient.MONTANT = item.MONTANT;
                                leLClient.EXIGIBILITE = item.EXIGIBILITE;
                            }
                        }
                        else
                        {
                            item.NDOC  = AccueilProcedures.NumeroFacture(item.FK_IDCENTRE );
                            Entities.InsertEntity<LCLIENT>(ObtenirLclientMor(item), cmd);
                            Entities.InsertEntity<DETAILMORATOIRE >(Galatee.Tools.Utility.ConvertEntity<DETAILMORATOIRE,CsDetailMoratoire>(item), cmd);
                        }
	                }
                    cmd.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private LCLIENT ObtenirLclientMor(CsDetailMoratoire  moratoires)
        {
            try
            {
                    LCLIENT cmpte = new LCLIENT();

                    cmpte.CRET = moratoires.CRET;
                    cmpte.DATEVALEUR = moratoires.DATEVALEUR;
                    cmpte.DC = moratoires.DC;
                    cmpte.EXIGIBILITE = moratoires.EXIGIBILITE;
                    cmpte.CENTRE = moratoires.CENTRE;
                    cmpte.CLIENT = moratoires.CLIENT;
                    cmpte.COPER = moratoires.COPER;
                    cmpte.ORDRE = moratoires.ORDRE;
                    cmpte.TOP1 = moratoires.TOP1;
                    cmpte.FRAISDERETARD = moratoires.FRAISDERETARD;
                    cmpte.MOISCOMPT = moratoires.MOISCOMPT;
                    cmpte.MONTANT = moratoires.MONTANT;
                    cmpte.NDOC = moratoires.NDOC;
                    cmpte.REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00");
                    cmpte.MATRICULE = moratoires.MATRICULE;
                    cmpte.DENR = moratoires.DATECREATION.Value;

                    cmpte.USERCREATION = moratoires.USERCREATION;
                    cmpte.DATECREATION = moratoires.DATECREATION;
                    cmpte.USERCREATION = moratoires.MATRICULE;
                    cmpte.DATECREATION = moratoires.DATECREATION;
                    cmpte.FK_IDCLIENT = moratoires.FK_IDCLIENT;

                    // valorisation des propriétés foreign key

                    cmpte.FK_IDCENTRE = moratoires.FK_IDCENTRE;
                    cmpte.FK_IDCOPER = moratoires.FK_IDCOPER;
                    cmpte.FK_IDLIBELLETOP = moratoires.FK_IDLIBELLETOP;
                    cmpte.FK_IDADMUTILISATEUR = moratoires.FK_IDADMUTILISATEUR;

                    return cmpte;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool? InsertDetailsMoratoires(List<CsLclient> lstMoratoire, List<CsLclient> facture)
        {
            try
            {
                List<DETAILMORATOIRE> moratoirs = ObtenirDetailMoratoires(lstMoratoire);
                List<LCLIENT> comptes = ObtenirLclient(lstMoratoire.Where(t => t.COPER != "085").ToList(), moratoirs.Where(t => t.COPER != "085").ToList());
                MORATOIRE m = InsererMoratoire(lstMoratoire.First().FK_IDCENTRE, lstMoratoire.First().CENTRE, lstMoratoire.First().CLIENT, lstMoratoire.First().ORDRE, lstMoratoire.First().USERCREATION, lstMoratoire.First().ISPRECONTENTIEUX );

                m.DETAILMORATOIRE = moratoirs;
                m.LCLIENT = comptes;
                List<TRANSCAISB> lstEncaisseAInserer = new List<TRANSCAISB>();
                foreach (CsLclient  item in facture)
                {
                    item.DATECREATION = System.DateTime.Now;
                    item.USERCREATION = lstMoratoire.First().USERCREATION;
                    item.DATEMODIFICATION = System.DateTime.Now;
                    item.MONTANT = item.SOLDEFACTURE;
                    item.TOP1 = lstMoratoire.First().TOP1;
                    item.FK_IDLIBELLETOP = lstMoratoire.First().FK_IDLIBELLETOP;
                    item.COPER = lstMoratoire.First().COPER;
                    item.FK_IDCOPER = lstMoratoire.First().FK_IDCOPER;
                    item.NATURE = lstMoratoire.First().NATURE;
                    item.FK_IDNATURE = lstMoratoire.First().FK_IDNATURE;
                    TRANSCAISB Facture = ObtenirTranscaisse(item);
                    if (facture != null)
                        lstEncaisseAInserer.Add(Facture);
                }
                m.TRANSCAISB = lstEncaisseAInserer;
                using (galadbEntities cmd = new galadbEntities())
                {
                    Entities.InsertEntity<MORATOIRE>(m, cmd);
                    cmd.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<LCLIENT> ObtenirLclient(List<CsLclient> moratoires, List<DETAILMORATOIRE> lesDetailMor)
        {
            try
            {
                List<LCLIENT> comptes = new List<LCLIENT>();
                CsLclient leMor = moratoires.First();
                foreach (var m in lesDetailMor)
                {
                    LCLIENT cmpte = new LCLIENT();

                    cmpte.CRET = m.CRET;
                    cmpte.DATEVALEUR = m.DATEVALEUR;
                    cmpte.DC = m.DC;
                    cmpte.EXIGIBILITE = m.EXIGIBILITE;
                    cmpte.CENTRE = leMor.CENTRE;
                    cmpte.CLIENT = leMor.CLIENT;
                    cmpte.COPER = m.COPER;
                    cmpte.ORDRE = leMor.ORDRE;
                    cmpte.TOP1 = leMor.TOP1;
                    cmpte.FRAISDERETARD = m.FRAISDERETARD;
                    cmpte.MOISCOMPT = leMor.MOISCOMPT;
                    cmpte.MONTANT = m.MONTANT;
                    cmpte.NDOC = m.NDOC;
                    cmpte.REFEM = m.REFEM ;
                    cmpte.MATRICULE = leMor.MATRICULE;
                    cmpte.DENR = leMor.DATECREATION.Value;

                    cmpte.USERCREATION = m.USERCREATION;
                    cmpte.DATECREATION = m.DATECREATION;
                    cmpte.CAISSE = leMor.CAISSE;
                    cmpte.MODEREG = leMor.MODEREG;
                    cmpte.USERCREATION = leMor.MATRICULE;
                    cmpte.DATECREATION = m.DATECREATION;
                    cmpte.FK_IDCLIENT = leMor.FK_IDCLIENT;

                    // valorisation des propriétés foreign key

                    cmpte.FK_IDCENTRE = leMor.FK_IDCENTRE;
                    cmpte.FK_IDCOPER = leMor.FK_IDCOPER;
                    cmpte.FK_IDLIBELLETOP = leMor.FK_IDLIBELLETOP;
                    cmpte.FK_IDADMUTILISATEUR = leMor.FK_IDADMUTILISATEUR;

                    comptes.Add(cmpte);
                }
                return comptes;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private TRANSCAISB ObtenirTranscaisse(CsLclient LeMoratoires)
        {
            try
            {
                galadbEntities contextInter = new galadbEntities();
                TRANSCAISB cmpte = new TRANSCAISB();
                cmpte.CENTRE = LeMoratoires.CENTRE;
                cmpte.CLIENT = LeMoratoires.CLIENT;
                cmpte.ORDRE = LeMoratoires.ORDRE;
                cmpte.CAISSE = LeMoratoires.CAISSE;
                cmpte.ACQUIT = LeMoratoires.ACQUIT;
                cmpte.MATRICULE = LeMoratoires.MATRICULE;
                cmpte.NDOC = LeMoratoires.NDOC;
                cmpte.REFEM = LeMoratoires.REFEM;
                cmpte.MONTANT = LeMoratoires.MONTANT;
                cmpte.DC = LeMoratoires.DC;
                cmpte.COPER = LeMoratoires.COPER;
                cmpte.MODEREG = LeMoratoires.MODEREG;
                cmpte.DTRANS = System.DateTime.Today;
                cmpte.DEXIG = LeMoratoires.EXIGIBILITE;
                cmpte.BANQUE = LeMoratoires.BANQUE;
                cmpte.GUICHET = LeMoratoires.GUICHET;
                cmpte.ORIGINE = LeMoratoires.ORIGINE;
                cmpte.ECART = LeMoratoires.ECART;
                cmpte.CRET = LeMoratoires.CRET;
                cmpte.MOISCOMPT = LeMoratoires.MOISCOMPT;
                cmpte.TOP1 = LeMoratoires.TOP1;
                cmpte.DATEVALEUR = LeMoratoires.DATEVALEUR;
                cmpte.DATEFLAG = LeMoratoires.DATEFLAG;
                cmpte.DATEENCAISSEMENT = LeMoratoires.DATEENCAISSEMENT;
                cmpte.DATECREATION = LeMoratoires.DATECREATION.Value;
                cmpte.USERCREATION = LeMoratoires.USERCREATION;
                cmpte.DATEMODIFICATION = LeMoratoires.DATEMODIFICATION;
                cmpte.USERMODIFICATION = LeMoratoires.USERMODIFICATION;
                cmpte.FK_IDCENTRE = LeMoratoires.FK_IDCENTRE;
                cmpte.FK_IDLCLIENT = LeMoratoires.PK_ID;
                cmpte.FK_IDHABILITATIONCAISSE = LeMoratoires.FK_IDHABILITATIONCAISSE;
                cmpte.FK_IDMODEREG = LeMoratoires.FK_IDMODEREG; 
                cmpte.FK_IDLIBELLETOP = LeMoratoires.FK_IDLIBELLETOP;
                cmpte.FK_IDCAISSIERE = LeMoratoires.FK_IDCAISSIERE;
                cmpte.FK_IDAGENTSAISIE = LeMoratoires.FK_IDAGENTSAISIE;
                cmpte.FK_IDCOPER = LeMoratoires.FK_IDCOPER;
                cmpte.FK_IDPOSTECLIENT = LeMoratoires.FK_IDPOSTECLIENT;
                cmpte.FK_IDNAF = LeMoratoires.FK_IDNAF;
                cmpte.POSTE = LeMoratoires.POSTE;
                //cmpte.DATETRANS = LeMoratoires.DATETRANS;
                //cmpte.BANQUECAISSE = LeMoratoires.BANQUECAISSE;
                //cmpte.AGENCEBANQUE = LeMoratoires.AGENCEBANQUE;

                return cmpte;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<DETAILMORATOIRE> ObtenirDetailMoratoires(List<CsLclient> moratoires)
        {
            try
            {
                List<DETAILMORATOIRE> mList = new List<DETAILMORATOIRE>();
                int idCentre = moratoires[0].FK_IDCENTRE;
                foreach (var m in moratoires)
                {
                    DETAILMORATOIRE detail = new DETAILMORATOIRE();
                    detail.CRET = m.CRET;
                    detail.DATEVALEUR = m.DATEVALEUR;
                    detail.DC = m.DC;
                    detail.EXIGIBILITE = m.EXIGIBILITE;
                    detail.COPER = m.COPER;
                    detail.FRAISDERETARD = m.FRAISDERETARD;
                    detail.MONTANT = m.MONTANT;
                    detail.NDOC = AccueilProcedures.NumeroFacture(idCentre);
                    detail.REFEM = System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString("00");
                    detail.USERCREATION = m.MATRICULE;
                    detail.DATECREATION = m.DATECREATION.Value;
                    mList.Add(detail);
                }

                return mList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        string RetournerMaxIdMoratoire()
        {
            try
            {
                // prendre le idmoratoire max dans la table moratoire

                using (galadbEntities context = new galadbEntities())
                {
                    int? moratoire = context.MORATOIRE.Max(m => m.PK_ID);
                    if (moratoire == null) // le scena dans lequel on a aucun moratoire dans la base
                        moratoire = 0;

                    return (++moratoire).ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool? InsererMoratoire(string centre, string client, string ordre, galadbEntities cmd)
        {
            //cmd.CommandText="SPX_MOR_INSERTIONNOUVEAUMORATOIRE";
            try
            {
                MORATOIRE moratoire = new MORATOIRE();

                moratoire.CENTRE = centre;
                moratoire.CLIENT = client;
                moratoire.ORDRE = ordre;
                moratoire.STATUS = Enumere.StatusActive;
                moratoire.TOP1 = Enumere.moduleRecouvrement.ToString();

                return Entities.InsertEntity<MORATOIRE>(moratoire, cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private MORATOIRE InsererMoratoire(int? fk_idcentre, string centre, string client, string ordre, string matricule,bool IsPrecontentieux)
        {
            //cmd.CommandText="SPX_MOR_INSERTIONNOUVEAUMORATOIRE";
            try
            {
                galadbEntities contextInter = new galadbEntities();
                MORATOIRE moratoire = new MORATOIRE();

                moratoire.CENTRE = centre;
                moratoire.CLIENT = client;
                moratoire.ORDRE = ordre;
                moratoire.STATUS = Enumere.StatusActive;
                moratoire.TOP1 = Enumere.moduleRecouvrement.ToString();
                moratoire.FK_IDTOP1 = contextInter.LIBELLETOP.First(t => t.CODE == moratoire.TOP1).PK_ID;
                moratoire.FK_IDCENTRE = fk_idcentre;
                moratoire.USERCREATION = matricule;
                moratoire.DATECREATION = DateTime.Now.Date;
                moratoire.ISPRECONTENTIEUX = IsPrecontentieux;

                contextInter.Dispose();
                return moratoire;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CsDetailMoratoire> RetourneMoratoireDuClient(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = RecouvProcedures.RetourneMoratoireClient(centre, client, ordre);
                DataTable dts = RecouvProcedures.RetourneReglementMoratoireClient(centre, client, ordre);
                List<CsLclient> lstFactureMoratoire = Entities.GetEntityListFromQuery<CsLclient>(dts);
                List<CsDetailMoratoire> lstMoratoire = Entities.GetEntityListFromQuery<CsDetailMoratoire>(dt);
                foreach (CsDetailMoratoire item in lstMoratoire)
                {
                    string reffacture = string.Empty;
                    List<CsLclient> lstFacture = lstFactureMoratoire.Where(t => t.FK_IDMORATOIRE == item.IDMORATOIRE).ToList();
                    int i=0;
                    foreach (CsLclient items in lstFacture)
                    {
                        if (i == 6) break;
                        reffacture = reffacture + items.REFEM + "-" + items.NDOC + "  ";
                        i++;
                    }
                    item.REFEMNDOC = reffacture;
                    item.REFERENCE = lstFacture.Count().ToString();
                }
                return lstMoratoire;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailMoratoire> VerifiePaiementMoratoire(int IdMoratoire)
        {
            //cmd.CommandText = "SPX_RPT_Dis_Zone";

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_VERIFIEPAIEMENTMORATOIRE";
                cmd.Parameters.Add("@IdMoratoire", SqlDbType.Int).Value = IdMoratoire;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                   return  Entities.GetEntityListFromQuery<CsDetailMoratoire>(dt);
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

        public bool? SuppressionMoratoireDuClient(int Idmoratoire)
        {
            try
            {
                MORATOIRE morat = ObtenirMoratoire(Idmoratoire);
                morat.STATUS = Enumere.StatusSupprimer;

                List<LCLIENT> detailMorCmpte = ObtenirMoratoireCompteClient(Idmoratoire);
                List<TRANSCAISB> ReglementDuMoration = ObtenirReglementDuMoratoir(Idmoratoire);
                ReglementDuMoration.ForEach(t => t.TOPANNUL = "O");
                using (galadbEntities context = new galadbEntities())
                {
                    Entities.UpdateEntity<TRANSCAISB>(ReglementDuMoration, context);
                    Entities.UpdateEntity<MORATOIRE>(morat, context);
                    Entities.DeleteEntity<LCLIENT>(detailMorCmpte, context);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool MiseAJourMoratoire(int IdMoratoire)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_ANNULATIONMORATOIRE";
                cmd.Parameters.Add("@IdMoratoire", SqlDbType.Int).Value = IdMoratoire;
                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    int res = -1;
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    res = cmd.ExecuteNonQuery();
                    return res == -1 ? false : true;
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
        private List<LCLIENT> ObtenirMoratoireCompteClient(int IdMoratoire)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.LCLIENT.Where(l => l.FK_IDMORATOIRE == IdMoratoire).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<TRANSCAISB> ObtenirReglementDuMoratoir(int Idmoratoire)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.TRANSCAISB.Where(l => l.FK_IDMORATOIRE == Idmoratoire && l.COPER == Enumere.CoperMorSolde ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private MORATOIRE ObtenirMoratoire(int Idmoratoire)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    return context.MORATOIRE.First(c => c.PK_ID == Idmoratoire);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsCAMPAGNE> RetourneDonneesSaisieIndexAvisCoupure(List<int> lstCentre)
        {
            try
            {
                DataTable dt = RecouvProcedures.RetourneDonneesSaisieIndexAvisCoupure(lstCentre);
                return Entities.GetEntityListFromQuery<CsCAMPAGNE>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  
        public List<CsCAMPAGNE> RetourneDonneeCampagne(List<int> lstCentre)
        {
            try
            {
                DataTable dt = RecouvProcedures.RetourneElementAvisCoupure(lstCentre);
                return Entities.GetEntityListFromQuery<CsCAMPAGNE>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> RechercherClientCampagnePourRDV(CsCAMPAGNE campagne, CsClient leClientRech)
        {
            try
            {
                DataTable dt = new DBMoratoires().ReturnAvisCoupurePourSaisiRDV(campagne, leClientRech);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAvisDeCoupureClient > RetourneDetailCampagneFromCampagne(CsCAMPAGNE campagne )
        {
            try
            {
                DataTable dt = new DBMoratoires().ReturnAvisCoupure(campagne);
                List<CsAvisDeCoupureClient> lst = Entities.GetEntityListFromQuery<CsAvisDeCoupureClient>(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable ReturnAvisCoupure(CsCAMPAGNE campagne)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var detcampagne = context.DETAILCAMPAGNE;
                    IEnumerable<object> query =

                        (from x in detcampagne
                         where x.FK_IDCENTRE == campagne.FK_IDCENTRE && x.IDCOUPURE == campagne.IDCOUPURE 
                         select new
                         {
                                x.  IDCOUPURE ,
                                x.  CENTRE ,
                                x.  CLIENT ,
                                x.  ORDRE ,
                                x.  TOURNEE ,
                                x.REFEM ,
                                x.NDOC,
                                x.MONTANT,
                                x.ORDTOUR ,
                                x.SOLDEDUE ,
                                x.NOMBREFACTURE ,
                                x.PK_ID ,
                                x.DATECREATION ,
                                x.DATEMODIFICATION ,
                                x.USERCREATION ,
                                x.USERMODIFICATION ,
                                x.FK_IDCENTRE ,
                                x.FK_IDTOURNEE ,
                                x.FK_IDCATEGORIECLIENT ,
                                x.FK_IDCAMPAGNE ,
                                x.FK_IDCLIENT ,
                                x.SOLDECLIENT,
                                x.SOLDEFACTURE ,
                                x.COMPTEUR ,
                                x.ISAUTORISER ,
                                x.MOTIFAUTORISATION ,
                                x.FRAIS ,
                                x.ISANNULATIONFRAIS ,
                                x.MOTIFANNULATION,
                                ADRESSE = x.CLIENT1.ADRMAND1,
                                x.CLIENT1.NOMABON,
                                CATEGORIECLIENT = x.CATEGORIECLIENT1.LIBELLE,
                                LIBELLECOPER = context.COPER.FirstOrDefault(t=>t.CODE == x.COPER).LIBELLE 
                         });
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable ReturnAvisCoupurePourSaisiRDV(CsCAMPAGNE campagne, CsClient leClientRech)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    if (leClientRech != null)
                    {
                        var detcampagne = context.DETAILCAMPAGNE;
                        IEnumerable<object> query =

                            (from x in detcampagne
                             where x.FK_IDCENTRE == leClientRech.FK_IDCENTRE && x.CENTRE == leClientRech.CENTRE && x.ORDRE == leClientRech.ORDRE && (x.FRAIS != 0 || x.FRAIS != null)
                                 && !context.LCLIENT.Any(es => es.FK_IDCLIENT == x.FK_IDCLIENT && es.IDCOUPURE == campagne.IDCOUPURE && es.RDV_COUPURE != null)
                             select new
                             {
                                 x.IDCOUPURE,
                                 x.CENTRE,
                                 x.CLIENT,
                                 x.ORDRE,
                                 x.TOURNEE,
                                 x.ORDTOUR,
                                 CATEGORIECLIENT = x.CATEGORIECLIENT1.LIBELLE,
                                 x.SOLDEDUE,
                                 x.NOMBREFACTURE,
                                 x.DATECREATION,
                                 x.DATEMODIFICATION,
                                 x.USERCREATION,
                                 x.USERMODIFICATION,
                                 x.FK_IDCENTRE,
                                 x.FK_IDTOURNEE,
                                 x.FK_IDCATEGORIECLIENT,
                                 x.FK_IDCAMPAGNE,
                                 x.FK_IDCLIENT,
                                 x.CLIENT1.NOMABON,
                                 ADRESSE = x.CLIENT1.ADRMAND1,
                                 x.COMPTEUR

                             }).Distinct();
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
                    else
                    {
                        var detcampagne = context.DETAILCAMPAGNE;
                        IEnumerable<object> query =

                            (from x in detcampagne
                             where x.FK_IDCENTRE == campagne.FK_IDCENTRE && x.IDCOUPURE == campagne.IDCOUPURE
                             && !context.LCLIENT.Any(es => es.FK_IDCLIENT == x.FK_IDCLIENT && es.IDCOUPURE == campagne.IDCOUPURE && es.RDV_COUPURE != null)
                             select new
                             {
                                 x.IDCOUPURE,
                                 x.CENTRE,
                                 x.CLIENT,
                                 x.ORDRE,
                                 x.TOURNEE,
                                 x.ORDTOUR,
                                 CATEGORIECLIENT = x.CATEGORIECLIENT1.LIBELLE,
                                 x.SOLDEDUE,
                                 x.NOMBREFACTURE,
                                 x.DATECREATION,
                                 x.DATEMODIFICATION,
                                 x.USERCREATION,
                                 x.USERMODIFICATION,
                                 x.FK_IDCENTRE,
                                 x.FK_IDTOURNEE,
                                 x.FK_IDCATEGORIECLIENT,
                                 x.FK_IDCAMPAGNE,
                                 x.FK_IDCLIENT,
                                 x.CLIENT1.NOMABON,
                                 ADRESSE = x.CLIENT1.ADRMAND1,
                                 x.COMPTEUR

                             }).Distinct();
                        return Galatee.Tools.Utility.ListToDataTable(query);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CsLclient> RetourneFactureImpliqueCampagne(CsDetailCampagne Detailcampagne)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    var histo = context.LCLIENT;
                    IEnumerable<object> query =
                        from x in histo
                        where x.FK_IDCENTRE == Detailcampagne.FK_IDCENTRE && x.IDCOUPURE == Detailcampagne.IDCOUPURE && x.FK_IDCLIENT == Detailcampagne.FK_IDCLIENT
                        select new
                        {
                            x.PK_ID,
                            x.CENTRE,
                            x.CLIENT,
                            x.ORDRE,
                            x.REFEM,
                            x.NDOC,
                            x.COPER,
                            x.DENR,
                            x.EXIG,
                            x.MONTANT,
                            x.CAPUR,
                            x.CRET,
                            x.MODEREG,
                            x.DC,
                            x.ORIGINE,
                            x.CAISSE,
                            x.ECART,
                            x.MOISCOMPT,
                            x.TOP1,
                            x.EXIGIBILITE,
                            x.FRAISDERETARD,
                            x.REFERENCEPUPITRE,
                            x.IDLOT,
                            x.DATEVALEUR,
                            x.REFERENCE,
                            x.REFEMNDOC,
                            x.ACQUIT,
                            x.MATRICULE,
                            x.TAXESADEDUIRE,
                            x.DATEFLAG,
                            x.MONTANTTVA,
                            x.IDCOUPURE,
                            x.AGENT_COUPURE,
                            x.RDV_COUPURE,
                            x.NUMCHEQ,
                            x.OBSERVATION_COUPURE,
                            x.USERCREATION,
                            x.DATECREATION,
                            x.DATEMODIFICATION,
                            x.USERMODIFICATION,
                            x.BANQUE,
                            x.GUICHET,
                            x.FK_IDCENTRE,
                            x.FK_IDADMUTILISATEUR,
                            x.FK_IDCOPER,
                            x.FK_IDLIBELLETOP,
                            x.FK_IDCLIENT,
                            x.FK_IDPOSTE,
                            x.POSTE,
                            x.DATETRANS,
                            LIBELLECOPER = x.COPER1.LIBELLE,
                            LIBELLENATURE = x.COPER1.LIBCOURT,
                        };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                    return Entities.GetEntityListFromQuery<CsLclient>(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public aCampagne RechercherIndexParClient(string idCoupure, string centre, string client, string ordre)
        {
            aCampagne campagne = new aCampagne();
            //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_SELECT_BY_CLIENT", cn);

            try
            {
                DataTable dt = RecouvProcedures.RechercherIndexCampagneParClient(idCoupure, centre, client, ordre);
                return Entities.GetEntityFromQuery<aCampagne>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> RechercherCampagneSaisiIndexParCoupure(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {
                DataTable dtTotale = RecouvProcedures.RetourneCampagneCoupureIdCoupureNonSaisi( Campagne,  leClientRech);
                List<CsDetailCampagne> TotalCampagne = Entities.GetEntityListFromQuery<CsDetailCampagne>(dtTotale);
                return TotalCampagne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> RechercherCampagneIndexSaisi(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {
                DataTable dtTotale = RecouvProcedures.RetourneCampagneCoupureIdCoupureSaisi(Campagne, leClientRech);
                List<CsDetailCampagne> TotalCampagne = Entities.GetEntityListFromQuery<CsDetailCampagne>(dtTotale);
                return TotalCampagne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> RetourneDonneeAnnulationFrais(CsCAMPAGNE  campagne ,CsClient leClientRech)
        {
      
            try
            {
                DataTable dtTotale = RecouvProcedures.RetourneDonneeAnnulationFrais(campagne, leClientRech);
                List<CsDetailCampagne> TotalCampagne = Entities.GetEntityListFromQuery<CsDetailCampagne>(dtTotale);
                return TotalCampagne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ValidationAutorisationFrais(List<CsDetailCampagne> campagne)
        {
      
            try
            {
                return RecouvProcedures.ValidationAutorisationFrais(campagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool  ValidationAnnulationFrais(List<CsDetailCampagne> campagne)
        {
      
            try
            {
                //return RecouvProcedures.ValidationAnnulationFrais(campagne);
                return ValidationAnnulationFrais(campagne.First());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidationReposeCompteur(List<CsDetailCampagne> campagne)
        {

            try
            {
                bool IsResultat = true;
                foreach (CsDetailCampagne item in campagne)
                   IsResultat =  RecouvProcedures.ValidationReposeCompteur(item);

                return IsResultat;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> RetourneDonneeFraisCoupure(string idCoupure)
        {
            List<CsDetailCampagne> lCampagnes = new List<CsDetailCampagne>();
            try
            {
                DataTable dtTotale = RecouvProcedures.RetourneDonneeFraisCoupure(idCoupure);
                List<CsDetailCampagne> TotalCampagne = Entities.GetEntityListFromQuery<CsDetailCampagne>(dtTotale);
                return TotalCampagne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsLclient VerifieFraisDejaSaisi(CsDetailCampagne leClient)
        {
            //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_SELECT_BY_IDCOUPURE", cn);

            try
            {
                DataTable dt = RecouvProcedures.VerifieFraisDejaSaisi(leClient);
                return Entities.GetEntityFromQuery<CsLclient>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne> EditionClientAReposer(List<CsCAMPAGNE> lstCampagne)
        {
            //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_SELECT_BY_IDCOUPURE", cn);

            try
            {
                DataTable dt = RecouvProcedures.EditionClientAReposer(lstCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<aCampagne> RechercherIndexParCampagne(string idCoupure)
        {
            //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_SELECT_BY_IDCOUPURE", cn);

            try
            {
                DataTable dt = RecouvProcedures.RechercherIndexParCampagne(idCoupure);
                return Entities.GetEntityListFromQuery<aCampagne>(dt);
                //campagne.Id = Int64.Parse(reader.GetValue(0).ToString().Trim());
                //campagne.IdCoupure = reader.GetValue(1).ToString().Trim();
                //campagne.Centre = reader.GetValue(2).ToString().Trim();
                //campagne.Client = reader.GetValue(3).ToString().Trim();
                //campagne.Ordre = reader.GetValue(4).ToString().Trim();
                //if (reader.GetValue(5).ToString() != string.Empty)
                //    campagne.SoldeInitial = decimal.Parse(reader.GetValue(5).ToString().Trim());
                //if (reader.GetValue(6).ToString() != string.Empty)
                //    campagne.IndexO = int.Parse(reader.GetValue(6).ToString().Trim());
                //if (reader.GetValue(7).ToString() != string.Empty)
                //    campagne.IndexE = int.Parse(reader.GetValue(7).ToString().Trim());
                //campagne.Observation = reader.GetValue(8).ToString().Trim();
                //if (reader.GetValue(9).ToString() != string.Empty)
                //    campagne.DateCoupure = DateTime.Parse(reader.GetValue(9).ToString().Trim());
                //if (reader.GetValue(10).ToString() != string.Empty)
                //    campagne.DateRDV = DateTime.Parse(reader.GetValue(10).ToString().Trim());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsDetailCampagne> RetourneDonneeSaisiIndexCoupure(CsCAMPAGNE Campagne, CsClient leClientRech)
        {
            try
            {

                return RechercherCampagneSaisiIndexParCoupure(Campagne, leClientRech);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<CsDetailCampagne> RetourneDonneeAnnulationFrais(CsCAMPAGNE Campagne, CsClient leClientRech)
        //{
        //    try
        //    {

        //        return RechercherCampagneSaisiIndexParCoupure(Campagne, leClientRech);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        
        /*
        public bool InsererLclient(List<CsLclient> Lesfacture)
        {
            try
            {
                galadbEntities context = new galadbEntities();

                List<LIBELLETOP> lstLibelleTop = context.LIBELLETOP.ToList();
                List<COPER> lstCoper= context.COPER.ToList();
                List<ADMUTILISATEUR > lstUtilisateur = context.ADMUTILISATEUR.ToList();
                List<LCLIENT> _lesFacture = new List<LCLIENT>();
                List<DETAILCAMPAGNE> leClientCampagne = new List<DETAILCAMPAGNE>();
               
                foreach (CsLclient Lafacture in Lesfacture)
                {
                    DETAILCAMPAGNE x = context.DETAILCAMPAGNE.FirstOrDefault(t => t.FK_IDCLIENT == Lafacture.FK_IDCLIENT && t.IDCOUPURE == Lafacture.IDCOUPURE);
                    if (x != null)
                    {
                        x.FRAIS = Lafacture.MONTANT;
                        x.FK_IDTYPECOUPURE = Lafacture.FK_IDTYPEDEVIS;
                        leClientCampagne.Add(x);
                    }

                    Lafacture.EXIGIBILITE = System.DateTime.Today.Date;
                    Lafacture.FK_IDLIBELLETOP = context.LIBELLETOP.FirstOrDefault(t => t.CODE == Lafacture.TOP1).PK_ID;
                    Lafacture.FK_IDCOPER = context.COPER.FirstOrDefault(t => t.CODE == Lafacture.COPER).PK_ID;
                    Lafacture.FK_IDADMUTILISATEUR = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == Lafacture.MATRICULE).PK_ID; 
                    _lesFacture.Add(Galatee.Tools.Utility.ConvertEntity <LCLIENT, CsLclient>(Lafacture));
                    _lesFacture.ForEach(t => t.FK_IDMOTIFCHEQUEINPAYE = null);
                }
                context.Dispose();
                int res = -1;
                using (galadbEntities ctx = new galadbEntities())
                {
                    Entities.UpdateEntity<DETAILCAMPAGNE>(leClientCampagne,ctx );
                    Entities.InsertEntity<LCLIENT>(_lesFacture,ctx );
                   res= ctx.SaveChanges();
                }
                return res == -1 ? false : true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        */

        public List<CsTypeCoupure> SelectObservations()
        {
            //cmd = new SqlCommand("SPX_OBSERVATION_SELECT_ALL", cn);

            try
            {
                DataTable dt = AccueilProcedures.RetourneTypeCoupure();
                return Entities.GetEntityListFromQuery<CsTypeCoupure>(dt);


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private LCLIENT GetElementDeFrais(CsDetailCampagne Campagne)
        {
            LCLIENT Frais = new LCLIENT();
            try
            {
                Frais.CENTRE = Campagne.CENTRE;
                Frais.CLIENT = Campagne.CLIENT;
                Frais.ORDRE = Campagne.ORDRE;
                Frais.REFEM = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                Frais.IDCOUPURE = Campagne.IDCOUPURE;
                Frais.COPER = Enumere.CoperFRP;
                Frais.DENR = DateTime.Today.Date;
                Frais.EXIGIBILITE = DateTime.Today.Date;
                Frais.DATECREATION = DateTime.Now ;
                Frais.DATEMODIFICATION = null ;
                Frais.DC = Enumere.Debit;
                Frais.MATRICULE = Campagne.MATRICULE;
                Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                Frais.MONTANT = Campagne.MONTANTFRAIS;
                Frais.TOP1 = "0";
                Frais.NDOC = new DBAccueil().NumeroFacture(Campagne.FK_IDCENTRE);
                Frais.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                Frais.FK_IDLIBELLETOP = Campagne.FK_IDLIBELLETOP;
                Frais.FK_IDCOPER = Campagne.FK_IDCOPER;
                Frais.FK_IDADMUTILISATEUR = Campagne.FK_IDADMUTILISATEUR;
                Frais.ISNONENCAISSABLE = Campagne.ISNONENCAISSABLE  ;
                return Frais;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private TRANSCAISB  GetElementDeposit(LCLIENT  Campagne)
        {
            TRANSCAISB Deposit = new TRANSCAISB();
            try
            {
                Deposit.CENTRE = Campagne.CENTRE;
                Deposit.CLIENT = Campagne.CLIENT;
                Deposit.ORDRE = Campagne.ORDRE;
                Deposit.REFEM = Campagne.REFEM ;
                Deposit.COPER = Enumere.CoperFPA;
                Deposit.DTRANS  = DateTime.Today.Date;
                Deposit.DATECREATION = DateTime.Today.Date;
                Deposit.DATEMODIFICATION = DateTime.Today.Date;
                Deposit.DC = Enumere.Debit;
                Deposit.MATRICULE = Campagne.MATRICULE;
                Deposit.MOISCOMPT = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                Deposit.MONTANT = Campagne.MONTANT ;
                Deposit.TOP1 = "0";
                Deposit.NDOC = new DBAccueil().NumeroFacture(Campagne.FK_IDCENTRE);
                Deposit.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                Deposit.FK_IDLCLIENT  = Campagne.PK_ID ;
                Deposit.FK_IDLIBELLETOP = Campagne.FK_IDLIBELLETOP;
                Deposit.FK_IDCOPER = Campagne.FK_IDCOPER;
                return Deposit;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool? UpdateIndex(CsDetailCampagne campagne)
        {
            try
            {
                int result = -1;
                using (galadbEntities context = new galadbEntities())
                {

                        LCLIENT leFrais = context.LCLIENT.FirstOrDefault(p => p.FK_IDCLIENT == campagne.FK_IDCLIENT &&
                                                                            p.COPER == Enumere.CoperFRP &&
                                                                            p.IDCOUPURE == campagne.IDCOUPURE);
                        bool IsPaiement = false ;
                        if (leFrais != null)
                        {
                            TRANSCAISSE leReglement = context.TRANSCAISSE.FirstOrDefault(p => p.FK_IDLCLIENT == leFrais.PK_ID &&
                                                                                p.TOPANNUL != "O");
                            if (leReglement != null) IsPaiement = true;
                            TRANSCAISB leReglements = context.TRANSCAISB.FirstOrDefault(p => p.FK_IDLCLIENT == leFrais.PK_ID &&
                                                                                p.TOPANNUL != "O");
                            if (leReglements != null) IsPaiement = true;

                        }
                        List<DETAILCAMPAGNE> lstfac = context.DETAILCAMPAGNE.Where(t => t.IDCOUPURE == campagne.IDCOUPURE &&
                                                                                        t.FK_IDCLIENT == campagne.FK_IDCLIENT).ToList();

                        INDEXCAMPAGNE leIdxCampagne = context.INDEXCAMPAGNE.FirstOrDefault(p => p.FK_IDCLIENT  == campagne.FK_IDCLIENT  &&
                                                        p.IDCOUPURE == campagne.IDCOUPURE);

                        if (leFrais != null && lstfac.First().ISANNULATIONFRAIS!= true )
                        {
                            leFrais.MONTANT = campagne.FRAIS;
                            if (leFrais.MONTANT > campagne.FRAIS && IsPaiement)
                            {
                                TRANSCAISB leDeposite = new TRANSCAISB();
                                leDeposite = GetElementDeposit(leFrais);
                                leDeposite.MONTANT = campagne.FRAIS;
                                Entities.InsertEntity<TRANSCAISB>(leDeposite, context);
                            }
                        }
                        if (lstfac != null && lstfac.Count != 0)
                            lstfac.ForEach(t => t.FRAIS = campagne.FRAIS);

                        if (leIdxCampagne != null)
                        {
                            leIdxCampagne.FK_TYPECOUPURE = campagne.FK_TYPECOUPURE.Value;
                            leIdxCampagne.INDEXE = campagne.INDEX;
                            leIdxCampagne.DATECOUPURE = campagne.DATECOUPURE;
                        }
                        result = context.SaveChanges();
                        return result < 0 ? false : true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? InsertIndexSGC(CsDetailCampagne campagne)
        {
            try
            {
                //Objet Command
                //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_INSERT", cn);
                INDEXCAMPAGNESGC index = null;
                index = new INDEXCAMPAGNESGC();
                index.IDCOUPURE = campagne.IDCOUPURE;
                index.CENTRE = campagne.CENTRE;
                index.CLIENT = campagne.CLIENT;
                index.ORDRE = campagne.ORDRE;
                index.MONTANT = campagne.SOLDECLIENT;
                index.INDEXE = campagne.INDEX;
                index.INDEXO = campagne.INDEX;
                index.CODEOBSERVATION = campagne.TYPECOUPURE;
                index.DATECOUPURE = campagne.DATECOUPURE;
                index.USERCREATION = campagne.MATRICULE;
                index.DATECREATION = DateTime.Now.Date;
                // renseigner les foreign key

                index.FK_TYPECOUPURE = campagne.FK_TYPECOUPURE.Value;
                index.FK_IDCAMPAGNE = campagne.FK_IDCAMPAGNE;
                index.FK_IDOBSERVATION = campagne.FK_IDOBSERVATION;

                int result = -1;

                List<LCLIENT> FraisPose = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {

                    CsLclient lstFrais = this.VerifieFraisDejaSaisi(campagne);
                    if (lstFrais != null && !string.IsNullOrEmpty(lstFrais.CLIENT))
                        Entities.InsertEntity<INDEXCAMPAGNESGC>(index, context);
                    else
                    {
                        List<DETAILCAMPAGNE> lstfac = context.DETAILCAMPAGNE.Where(t => t.IDCOUPURE == campagne.IDCOUPURE && t.FK_IDCLIENT == campagne.FK_IDCLIENT).ToList();

                        CLIENT leClient = context.CLIENT.FirstOrDefault(t => t.CENTRE == index.CENTRE && t.REFCLIENT == index.CLIENT && t.ORDRE == index.ORDRE);
                        campagne.FK_IDCLIENT = leClient.PK_ID;
                        campagne.FK_IDCENTRE = leClient.FK_IDCENTRE;

                        lstfac.ForEach(t => t.FRAIS = campagne.FRAIS);
                        Entities.InsertEntity<INDEXCAMPAGNESGC>(index, context); ;
                        FraisPose.Add(GetElementDeFrais(campagne));
                        Entities.InsertEntity<LCLIENT>(FraisPose, context);
                    }
                    result = context.SaveChanges();
                    return result < 0 ? false : true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? InsertIndex(CsDetailCampagne campagne)
        {
            try
            {
                //Objet Command
                //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_INSERT", cn);
                INDEXCAMPAGNE index = null;
                    index = new INDEXCAMPAGNE();
                    index.IDCOUPURE = campagne.IDCOUPURE;
                    index.CENTRE = campagne.CENTRE;
                    index.CLIENT = campagne.CLIENT;
                    index.ORDRE = campagne.ORDRE;
                    index.MONTANT = campagne.SOLDECLIENT;
                    index.INDEXE = campagne.INDEX;
                    index.INDEXO = campagne.INDEX;
                    index.CODEOBSERVATION = campagne.TYPECOUPURE;
                    index.DATECOUPURE = campagne.DATECOUPURE;
                    index.USERCREATION = campagne.MATRICULE;
                    index.DATECREATION = DateTime.Now.Date;
                    // renseigner les foreign key

                    index.FK_TYPECOUPURE = campagne.FK_TYPECOUPURE.Value ;
                    index.FK_IDCAMPAGNE = campagne.FK_IDCAMPAGNE;
                    index.FK_IDOBSERVATION = campagne.FK_IDOBSERVATION ;
                    index.FK_IDCENTRE = campagne.FK_IDCENTRE;
                    index.FK_IDCLIENT = campagne.FK_IDCLIENT;

                int result = -1;

                List<LCLIENT> FraisPose = new List<LCLIENT>();
                using (galadbEntities context = new galadbEntities())
                {
          
                     //CsLclient  lstFrais = this.VerifieFraisDejaSaisi(campagne);
                     LCLIENT LeFrais = context.LCLIENT.FirstOrDefault(t => t.IDCOUPURE == index.IDCOUPURE && t.FK_IDCLIENT == index.FK_IDCLIENT && ( t.COPER==Enumere.CoperFRP || t.COPER =="074"));
                     if (LeFrais != null )
                     //if (lstFrais != null && !string.IsNullOrEmpty(lstFrais.CLIENT))
                     {
                         LeFrais.MONTANT = index.MONTANT;
                         LeFrais.DATEMODIFICATION = System.DateTime.Now;
                         INDEXCAMPAGNE leIdx = context.INDEXCAMPAGNE.FirstOrDefault(t => t.FK_IDCAMPAGNE == index.FK_IDCAMPAGNE && t.FK_IDCLIENT == index.FK_IDCLIENT);
                         if (leIdx != null)
                         {
                             leIdx.INDEXE = index.INDEXE;
                             leIdx.INDEXO  = index.INDEXE;
                             leIdx.DATECOUPURE = index.DATECOUPURE;
                             leIdx.FK_TYPECOUPURE  = index.FK_TYPECOUPURE ;
                             leIdx.FK_IDOBSERVATION  = index.FK_IDOBSERVATION ;
                         }
                         else 
                         Entities.InsertEntity<INDEXCAMPAGNE>(index, context);
                     }
                     else
                     {
                         List<DETAILCAMPAGNE> lstfac = context.DETAILCAMPAGNE.Where(t => t.IDCOUPURE == campagne.IDCOUPURE && t.FK_IDCLIENT == campagne.FK_IDCLIENT).ToList();
                         lstfac.ForEach(t => t.FRAIS = campagne.FRAIS);
                         lstfac.ForEach(t => t.FK_IDTYPECOUPURE = campagne.FK_TYPECOUPURE);
                         Entities.InsertEntity<INDEXCAMPAGNE>(index, context); ;
                         FraisPose.Add(GetElementDeFrais(campagne));
                         Entities.InsertEntity<LCLIENT>(FraisPose, context);
                     }
                    result = context.SaveChanges();
                    return result < 0 ? false : true; 
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*
        public bool? InsertListIndex(List<CsDetailCampagne> campagnes)
        {
            try
            {
                //Objet Command
                //cmd = new SqlCommand("SPX_INDEXCAMPAGNE_INSERT", cn);
                List<INDEXCAMPAGNE> indexCampagne = new List<INDEXCAMPAGNE>();
                List<LCLIENT> FraisPose = new List<LCLIENT>();
                INDEXCAMPAGNE index = null;
                int result = -1;
                galadbEntities context = new galadbEntities();

                foreach (var campagne in campagnes)
                {
                    index = new INDEXCAMPAGNE();
                    index.IDCOUPURE = campagne.IDCOUPURE;
                    index.CENTRE = campagne.CENTRE;
                    index.CLIENT = campagne.CLIENT;
                    index.ORDRE = campagne.ORDRE;
                    index.MONTANT = campagne.SOLDECLIENT;
                    index.INDEXE = campagne.INDEX;
                    index.INDEXO = campagne.INDEX;
                    index.CODEOBSERVATION = campagne.TYPECOUPURE;
                    index.DATECOUPURE = campagne.DATECOUPURE;
                    index.USERCREATION = campagne.MATRICULE;
                    index.DATECREATION = DateTime.Now.Date;
                    // renseigner les foreign key

                    index.FK_IDCAMPAGNE = campagne.FK_IDCAMPAGNE;
                    index.FK_IDOBSERVATION = campagne.FK_IDOBSERVATION.Value ;
                    index.FK_IDCENTRE = campagne.FK_IDCENTRE;
                    index.FK_IDCLIENT = campagne.FK_IDCLIENT;
                    indexCampagne.Add(index);

                    DETAILCAMPAGNE leClient = Galatee.Tools.Utility.ConvertEntity<DETAILCAMPAGNE, CsDetailCampagne>(campagne);
                    CsLclient lstFrais = this.VerifieFraisDejaSaisi(campagne);
                    if (lstFrais != null && !string.IsNullOrEmpty(lstFrais.CLIENT))
                        Entities.InsertEntity<INDEXCAMPAGNE>(index, context);
                    else
                    {
                        Entities.InsertEntity<INDEXCAMPAGNE>(index, context); ;
                        if (campagne.MONTANTFRAIS != 0)
                        {
                            FraisPose.Add(GetElementDeFrais(campagne));
                            Entities.InsertEntity<LCLIENT>(FraisPose, context);
                        }
                        Entities.UpdateEntity<DETAILCAMPAGNE>(leClient, context);
                    }
                }
                result = context.SaveChanges();
                return result < 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        public bool SaveAffectationTourne( List<CsTournee> ListeDesTourneAAffecter)
        {
            try
            {
                return RecouvProcedures.SaveAffectationTourne(ListeDesTourneAAffecter);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SaveRDVCoupure(List<CsDetailCampagne> lstClientRDV)
        {
            try
            {
                foreach (CsDetailCampagne item in lstClientRDV)
                    RecouvProcedures.SaveRDVCoupure(item.FK_IDCLIENT, item.IDCOUPURE, item.DATERENDEZVOUS.Value);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool SaveRDVCoupureHorsCampagne(CsClient leClient, DateTime dateRdv)

        {
            try
            {
                RecouvProcedures.SaveRDVCoupureHorsCampagne(leClient, dateRdv);
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<CsDetailCampagne> RetourneCampagneRendezVousCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                DataTable dt = RecouvProcedures.RetourneCampagneRendezVousCoupure(lstCentre, idCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<CsDetailCampagne> RetourneCampagneReglementCoupure(List<int> lstCentre, string idCampagne)
        {
            try
            {
                DataTable dt = RecouvProcedures.RetourneCampagneReglementCoupure(lstCentre, idCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
 
        public List<CsDetailCampagne> ListeDesMoratoiresEmis(List<int> lstIdCentre, DateTime Datedebut, DateTime Datefin)
        {
            DataTable dt = RecouvProcedures.ListeDesMoratoiresEmis(lstIdCentre, Datedebut, Datefin);
            return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
        }
        public List<CsDetailCampagne> ListeDesMoratoiresNonRespecte(List<int> lstIdCentre, DateTime Datedebut, DateTime Datefin)
        {
            DataTable dt = RecouvProcedures.ListeDesMoratoiresNonRespecte(lstIdCentre, Datedebut, Datefin);
            return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt); 
        }
        public List<CsDetailCampagne> ListeDesMoratoiresRespecte(List<int> lstIdCentre, DateTime Datedebut, DateTime Datefin)
        {
            DataTable dt = RecouvProcedures.ListeDesMoratoiresNonRespecte(lstIdCentre, Datedebut, Datefin);
            return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
        }
        #region Sylla

        public List<CsAffectationGestionnaire> RemplirAffectation()
        {
            try
            {
                DataTable dt = RecouvProcedures.RemplirAffectation();
                List<CsAffectationGestionnaire> AffectationGestionnaire = Entities.GetEntityListFromQuery<CsAffectationGestionnaire>(dt);

                return AffectationGestionnaire;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? SaveAffection(List<CsRegCli> ListRegCliAffecter, int? ID_USER)
        {
            try
            {
                return RecouvProcedures.SaveAffection(ListRegCliAffecter, ID_USER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveCampagne(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER)
        {
            try
            {
                return RecouvProcedures.SaveCampagne(ListFacturation, csRegCli, ID_USER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string SaveCampagnePrecontencieux(List<CsDetailCampagnePrecontentieux > ListDEtail,int ID_USER,string matricule)
        {
            try
            {
                return RecouvProcedures.SaveCampagnePrecontentieux(ListDEtail, ID_USER, matricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Dico> SaveCampane(List<CsLclient> ListFacturation, CsRegCli csRegCli, int? ID_USER)
        {
            try
            {
                return RecouvProcedures.SaveCampane(ListFacturation, csRegCli, ID_USER);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsCampagneGc> RetournCampagneByRegcli(CsRegCli csRegCli, string periode)
        {
            try
            {
                return RecouvProcedures.RetournCampagneByRegcli(csRegCli, periode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CsCampagneGc VerifierCampagneExiste(CsRegCli csRegCli, string periode)
        {
            try
            {
                return RecouvProcedures.VerifierCampagneExiste(csRegCli, periode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<CsFacture> Remplirfacture(CsRegCli csRegCli, List<string> listperiode)
        //{
        //    try
        //    {
        //        DataTable dt = RecouvProcedures.Remplirfacture(csRegCli,listperiode);
        //        List<CsFacture> Facture = Entities.GetEntityListFromQuery<CsFacture>(dt);

        //        return Facture;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public List<CsCampagneGc> RemplirCampagne(string Matricule)
        {
            try
            {
                return RecouvProcedures.RemplirCampagne(Matricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CsCampagneGc RemplirCampagneById(int IdCampagne,string Statut)
        {
            try
            {
                return RecouvProcedures.RemplirCampagneById(IdCampagne,Statut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? SaveMandatement(List<CsDetailCampagneGc> ListMandatementGc, bool IsAvisCerdit)
        {
            try
            {
                return SaveMandatementGc(ListMandatementGc, IsAvisCerdit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  bool? SaveMandatementGc(List<CsDetailCampagneGc> ListMandatementGc, bool IsAvisCerdit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    //var NDOCS = ListMandatementGc.Select(l => l.NDOC);
                    var numero = IsAvisCerdit != true ? ListMandatementGc.First().NUMEROMANDATEMENT : "0000000000000";
                    var MANDATEMENTEXISTANT = context.MANDATEMENTGC.FirstOrDefault(m => m.NUMEROMANDATEMENT == numero);
                    int idCampagne = ListMandatementGc.FirstOrDefault().IDCAMPAGNEGC;
                    string NumeroDemande = context.CAMPAGNEGC.FirstOrDefault(t => t.PK_ID == idCampagne).NUMEROCAMPAGNE;

                    string Matricule = ListMandatementGc.FirstOrDefault().USERCREATION;
                    //var LCLIENTS = context.LCLIENT.Where(m => NDOCS.Contains(m.NDOC)).ToList();
                    if (MANDATEMENTEXISTANT == null || MANDATEMENTEXISTANT.NUMEROMANDATEMENT == "0000000000000")
                    {
                        MANDATEMENTGC Mand = new MANDATEMENTGC();
                        Mand.DATECREATION = System.DateTime.Now;
                        Mand.DATEMODIFICATION = System.DateTime.Now;
                        Mand.FK_IDCAMPAGNA = ListMandatementGc.First().IDCAMPAGNEGC;
                        //Mand.MONTANT = ListMandatementGc.Sum(t => t.MONTANT +t.MONTANTTVA);
                        Mand.NUMEROMANDATEMENT = numero;
                        Mand.USERCREATION = ListMandatementGc.First().USERCREATION;
                        Mand.USERMODIFICATION = ListMandatementGc.First().USERMODIFICATION;
                        foreach (CsDetailCampagneGc item_ in ListMandatementGc)
                        {
                            DETAILMANDATEMENTGC DetailMand = new DETAILMANDATEMENTGC();

                            DetailMand.CENTRE = item_.CENTRE;
                            DetailMand.CLIENT = item_.CLIENT;
                            DetailMand.DATECREATION = item_.DATECREATION;
                            DetailMand.DATEMODIFICATION = item_.DATEMODIFICATION;
                            DetailMand.MONTANT = item_.MONTANT;
                            //DetailMand.MONTANTTVA = LCLIENTS.FirstOrDefault(m => m.NDOC == item_.NDOC && m.CENTRE == item_.CENTRE && m.CLIENT == item_.CLIENT && m.ORDRE == item_.ORDRE).MONTANTTVA;
                            DetailMand.MONTANTTVA = context.LCLIENT.FirstOrDefault(t => t.PK_ID == item_.FK_IDLCLIENT).MONTANTTVA;
                            DetailMand.NDOC = item_.NDOC;
                            DetailMand.ORDRE = item_.ORDRE;
                            DetailMand.PERIODE = item_.PERIODE;
                            DetailMand.PK_ID = item_.PK_ID;
                            DetailMand.FK_IDCLIENT = item_.FK_IDCLIENT;
                            DetailMand.FK_IDLCLIENT = item_.FK_IDLCLIENT;
                            DetailMand.STATUS = item_.STATUS;
                            DetailMand.USERCREATION = item_.USERCREATION;
                            DetailMand.USERMODIFICATION = item_.USERMODIFICATION;

                            Mand.DETAILMANDATEMENTGC.Add(DetailMand);
                        }
                        Mand.MONTANT = Mand.DETAILMANDATEMENTGC.Sum(t => t.MONTANT);
                        Entities.InsertEntity<MANDATEMENTGC>(Mand, context);
                        context.SaveChanges();
                        new DbWorkFlow().ExecuterActionSurDemandeTransction(NumeroDemande, Enumere.TRANSMETTRE, Matricule, string.Empty, context);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception  ex)
            {
                return null;
            }

        }
        public bool? SavePaiement(List<CsPaiementGc> ListMandatementGc, CsDetailMandatementGc Facture_Payer_Partiellement)
        {
            try
            {
                return RecouvProcedures.SavePaiement(ListMandatementGc, Facture_Payer_Partiellement);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool? SaisiPaiement(decimal? Montant, int Id)
        {
            try
            {
                return RecouvProcedures.SaisiPaiement(Montant, Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient > RechercheDetailCampane(string numerocamp)
        {
            try
            {
                DataTable dt = RecouvProcedures.RechercheDetaileCampane(numerocamp);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public List<CsLclient> RechercheMandatemant(string numeroCampagne)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_RETOURNE_MANDATEMENT";
            cmd.Parameters.Add("@NumCampagne", SqlDbType.VarChar, 30).Value = numeroCampagne;

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
        public List<CsLclient> RechercheMiseAJour(string numeroCampagne)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_RETOURNE_MISEAJOUR";
            cmd.Parameters.Add("@NumCampagne", SqlDbType.VarChar, 30).Value = numeroCampagne;

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
        public List<CsLclient> RecherchePaiement(string numeroCampagne)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_RETOURNE_PAIEMENT";
            cmd.Parameters.Add("@NumCampagne", SqlDbType.VarChar, 30).Value = numeroCampagne;

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
        public List<CsDetailCampagne> ListeDesClientEnRDV(string CodeSite, DateTime? DateDebut, DateTime? DateFin)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUV_CLIENTRENDEZVOUS";
            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@DateDebut", SqlDbType.Date).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = DateFin;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return Entities.GetEntityListFromQuery<CsDetailCampagne >(dt);
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
        public List<CsDetailCampagne> RetourneDonneePaiementFraisCampagne(DateTime? DateDebut, DateTime? DateFin)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_REGLEMENTFRAIS";
            cmd.Parameters.Add("@DateDebut", SqlDbType.Date).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = DateFin;
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
        public List<CsDetailCampagne> RetourneDonneePaiementCampagne(DateTime? DateDebut, DateTime? DateFin)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_REGLEMENTCAMPAGNE";
            cmd.Parameters.Add("@DateDebut", SqlDbType.Date).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = DateFin;
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
        public List<CsDetailCampagne> RetourneDonneeClientAutorise(DateTime? DateDebut, DateTime? DateFin)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUVGC_CLIENTAUTORISER";
            cmd.Parameters.Add("@DateDebut", SqlDbType.Date).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.Date).Value = DateFin;
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
        public List<CsCampagneGc> RechercheCampane(string numerocamp)
        {
            try
            {
                DataTable dt = RecouvProcedures.RechercheCampane(numerocamp);
                return Entities.GetEntityListFromQuery<CsCampagneGc>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> MiseAjourCompt(List<CsDetailPaiementGc> LstDetailPaiemenent, int Id)
        {
            try
            {
                return MiseAjourComptScgc(LstDetailPaiemenent, Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SupprimeDoublons(string NumeroCampagne)
        {

            try
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 3000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RPT_DEMANDE_NONLIE";
                cmd.Parameters.Add("@NumeroCampagne", SqlDbType.VarChar, 20).Value = NumeroCampagne;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                      cmd.ExecuteNonQueryAsync ();
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
        public  List<CsLclient> MiseAjourComptScgc(List<CsDetailPaiementGc > LstDetailPaiemenent, int Id)
        {
            CAMPAGNEGC CAMPAGNEGC = new CAMPAGNEGC();
            PAIEMENTCAMPAGNEGC PAIEMENTGC = new PAIEMENTCAMPAGNEGC();
            try
            {
                cmd = new SqlCommand();
                cn = new SqlConnection(ConnectionString);
                cmd.Connection = cn;
                cmd.CommandTimeout = 600;
                cmd.Connection.Open();
                cmd.Transaction = cmd.Connection.BeginTransaction();
              

                using (galadbEntities context = new galadbEntities())
                {
                    int IdPaiement = LstDetailPaiemenent.FirstOrDefault().FK_IDPAIEMENTCAMPAGNEGC;
                    List<CsMapperTranscaisB> TRANSCAISB = new List<CsMapperTranscaisB>();
                    CAMPAGNEGC = context.CAMPAGNEGC.FirstOrDefault(c => c.PK_ID == Id);
                    MANDATEMENTGC MANDATEMENTGC = context.MANDATEMENTGC.FirstOrDefault(c => c.FK_IDCAMPAGNA == Id);
                    PAIEMENTGC = context.PAIEMENTCAMPAGNEGC.FirstOrDefault(c => c.PK_ID == IdPaiement);
                    ADMUTILISATEUR ADMUTILISATEUR = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == CAMPAGNEGC.USERCREATION);
                    MODEREG modreg = context.MODEREG.FirstOrDefault(u => u.CODE == "4");
                    COPER COPER = context.COPER.FirstOrDefault(c => c.CODE == "010");
                    List<CsLclient> clients = new List<CsLclient>();

                    string NumeroDemande = CAMPAGNEGC.NUMEROCAMPAGNE ;
                    string Matricule = MANDATEMENTGC.USERCREATION;

                    foreach (CsDetailPaiementGc item in LstDetailPaiemenent)
                    {
                        CsMapperTranscaisB transB = new CsMapperTranscaisB();
                        CLIENT LeClient = context.CLIENT.FirstOrDefault(l => l.PK_ID == item.FK_IDCLIENT);

                        transB.ACQUIT = MANDATEMENTGC.NUMEROMANDATEMENT.Substring(0, 9);
                        transB.CENTRE = item.CENTRE;
                        transB.CLIENT = item.CLIENT;
                        transB.ORDRE = item.ORDRE;
                        transB.NDOC = item.NDOC;
                        transB.REFEM = item.PERIODE ;
                        transB.MONTANT = item.MONTANT;
                        transB.DC = "C";
                        transB.COPER = COPER.CODE;
                        transB.PERCU = item.MONTANT;
                        transB.RENDU = 0;
                        transB.MODEREG = modreg.CODE;
                        transB.PLACE = "-";
                        transB.DTRANS = DateTime.Now;
                        transB.BANQUE = "------";
                        transB.GUICHET = "------";
                        transB.ORIGINE = item.CENTRE;
                        transB.ECART = 0;
                        transB.MOISCOMPT = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                        transB.TOP1 = "8";
                        transB.MATRICULE = CAMPAGNEGC.USERCREATION;
                        transB.DATEENCAISSEMENT = DateTime.Now;
                        transB.DATECREATION = DateTime.Now;
                        transB.USERCREATION = CAMPAGNEGC.USERCREATION;
                        transB.DATEVALEUR = DateTime.Now;

                        // valuer les foreign key
                        transB.FK_IDCENTRE = LeClient.FK_IDCENTRE ;
                        transB.FK_IDCOPER = COPER.PK_ID;
                        transB.FK_IDLIBELLETOP = context.LIBELLETOP.FirstOrDefault(l => l.CODE == "8").PK_ID;
                        transB.FK_IDMODEREG = modreg.PK_ID;
                        transB.FK_IDAGENTSAISIE = ADMUTILISATEUR.PK_ID;
                        transB.FK_IDLCLIENT = item.FK_IDLCLIENT ;

                        //PAIEMENT.EST_MIS_A_JOUR = true;
                        //LISTEPAIEMENT.Add(PAIEMENT);
                        TRANSCAISB.Add(transB);
                        //LCLIENTS.Add(LCLIENT);

                        CsLclient leclient = new CsLclient();
                        leclient.ACQUIT = PAIEMENTGC.NUMEROMANDATEMENT.Substring(0, 9); ;
                        leclient.NDOC = item.NDOC;
                        leclient.PK_ID = item.PK_ID;
                        leclient.MONTANT = item.MONTANT;
                        leclient.CENTRE = item.CENTRE;
                        leclient.CLIENT = item.CLIENT;
                        leclient.ORDRE = item.ORDRE;
                        leclient.REFEM = transB.REFEM;
                        leclient.NOM = LeClient.NOMABON;
                        leclient.NUMDEVIS = PAIEMENTGC.NumAvisCredit ;
                        leclient.NUMDEM = PAIEMENTGC.NUMEROMANDATEMENT;
                        clients.Add(leclient);

                    }
 
                    CAMPAGNEGC.STATUS = "0";
                    PAIEMENTGC.EST_MIS_A_JOUR = true;

                    Entities.UpdateEntity<PAIEMENTCAMPAGNEGC>(PAIEMENTGC, context);
                    Entities.UpdateEntity<CAMPAGNEGC>(CAMPAGNEGC, context);
                    context.SaveChanges();
                    
                    DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(TRANSCAISB);
                    Galatee.Tools.Utility.BulkInsertTable("TRANSCAISB", TablePere, cmd.Connection, cmd.Transaction);
                    cmd.Transaction.Commit();
                    new DbWorkFlow().ExecuterActionSurDemandeTransction(NumeroDemande, Enumere.TRANSMETTRE, Matricule, string.Empty, context);
                    SupprimeDoublons(NumeroDemande);
                    return clients;
                }
            }
            catch (Exception ex)
            {
                if (PAIEMENTGC != null && PAIEMENTGC.EST_MIS_A_JOUR == true  && CAMPAGNEGC != null && CAMPAGNEGC.STATUS =="0")
                {
                    using (galadbEntities ctx = new galadbEntities() )
                    {
                        CAMPAGNEGC.STATUS = "1";
                        PAIEMENTGC.EST_MIS_A_JOUR = false ;
                        Entities.UpdateEntity<PAIEMENTCAMPAGNEGC>(PAIEMENTGC, ctx);
                        Entities.UpdateEntity<CAMPAGNEGC>(CAMPAGNEGC, ctx); 
                    }
                }
                throw ex;
            }
        }
        #endregion
        public List<CsCampagneGc> ChargerCampagne( int IdRegroupement)
        {
            try
            {
                DataTable dt = RecouvProcedures.ChargerCampagne(IdRegroupement);
                return Entities.GetEntityListFromQuery<CsCampagneGc>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsDetailCampagne > ChargerClientPourSaisiIndex(int idCampagne)
        {
            try
            {
                DataTable dt = RecouvProcedures.ChargerClientPourSaisiIndex(idCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CsLclient> AutorisationDePaiement(string centre, string client, string ordre)
        {
            try
            {
                DataTable dt = RecouvProcedures.AutorisationDePaiement(centre, client, ordre);
                return Entities.GetEntityListFromQuery<CsLclient>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool  ValidateAutorisation(CsLclient laFacture)
        {
            try
            {
                return  RecouvProcedures.ValidateAutorisation(laFacture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Procedure stoke

        private DataTable CreationLotCampagne(string idtournee, string idcategerie, string idCodeConsomateur,
                                              string ReferenceDebut, string ReferenceFin, string OrdreTourneDebut, string OrdreTourneFin, bool? ResilierPrisEnCompte,
                                              DateTime? Exigibilite, int? NombreMaximunFacture, decimal? SoldeMinimum, string Periode, int? NombreTotalClient, int? IdPia, string matricule)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_CREATION_COUPURE";

            cmd.Parameters.Add("@idCodeconso", SqlDbType.VarChar,int.MaxValue).Value = idCodeConsomateur;
            cmd.Parameters.Add("@idTournee", SqlDbType.VarChar, int.MaxValue).Value = idtournee;
            cmd.Parameters.Add("@idCategorie", SqlDbType.VarChar, int.MaxValue).Value = idcategerie;

            cmd.Parameters.Add("@referenceDebut", SqlDbType.VarChar, 20).Value = ReferenceDebut;
            cmd.Parameters.Add("@referenceFin", SqlDbType.VarChar, 20).Value = ReferenceFin;
            cmd.Parameters.Add("@ordreTourneeDebut", SqlDbType.VarChar, 15).Value = OrdreTourneDebut;
            cmd.Parameters.Add("@ordreTourneeFin", SqlDbType.VarChar, 15).Value = OrdreTourneFin;
            cmd.Parameters.Add("@exclureResilie", SqlDbType.Bit).Value = ResilierPrisEnCompte;
            cmd.Parameters.Add("@dueDate", SqlDbType.DateTime).Value = Exigibilite;
            cmd.Parameters.Add("@nbreMinimumFacturesDues", SqlDbType.Int).Value = NombreMaximunFacture;
            cmd.Parameters.Add("@soldeMinimumFacturesDues", SqlDbType.Decimal).Value = SoldeMinimum;
            cmd.Parameters.Add("@periodeDue", SqlDbType.VarChar, 8).Value = Periode;
            cmd.Parameters.Add("@nbreTotalClientIn", SqlDbType.Int).Value = NombreTotalClient;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                //if (reader.Read())
                dt.Load(reader);

                return dt;

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

        private DataTable CreationLotCampagneRegroupe(int idRegroupement, string Client, string Ordre, bool? ResilierPrisEnCompte, string centre, DateTime? Exigibilite, decimal? Montantmini, bool IsClientParticulier, string periodeDebut, string periodeFin)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 1800000;
            cmd.CommandType = CommandType.StoredProcedure;
            //if (IsClientParticulier)
            //{
            //    cmd.CommandText = "SPX_CREATION_COUPURE_REGROUPE_PARTICULIER";
            //    cmd.Parameters.Add("@MontantMini", SqlDbType.Decimal).Value = Montantmini;
            //}
            //else
                cmd.CommandText = "SPX_CREATION_COUPURE_REGROUPE";

            cmd.Parameters.Add("@idRegroupement", SqlDbType.Int).Value = idRegroupement;
            cmd.Parameters.Add("@exclureResilie", SqlDbType.Bit).Value = ResilierPrisEnCompte;
            cmd.Parameters.Add("@CentreCoupure", SqlDbType.VarChar, 3).Value = centre;
            cmd.Parameters.Add("@ClientRech", SqlDbType.VarChar, 20).Value = Client;
            cmd.Parameters.Add("@OrdreRech", SqlDbType.VarChar, 3).Value = Ordre;
            cmd.Parameters.Add("@DateExig", SqlDbType.DateTime).Value = Exigibilite;
            cmd.Parameters.Add("@periodeDebut", SqlDbType.VarChar, 6).Value = periodeDebut;
            cmd.Parameters.Add("@periodeFin", SqlDbType.VarChar, 6).Value = periodeFin;


            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                //if (reader.Read())
                dt.Load(reader);

                return dt;

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

        public List<CsLclient> ETAT_AVIS_DE_COUPUREGC(CsAvisCoupureEdition aviscoupure, aDisconnection dis, bool IsPreavis)
        {
            try
            {
                List<CsLclient> lstDonneFinale = new List<CsLclient>();
                List<CsRegCli> leRegroupement = new List<CsRegCli>();
                leRegroupement = aviscoupure.ListeRegroupement;
                List<string > lstCentre = new List<string>();
                if (aviscoupure.Centre_Campagne != null && aviscoupure.Centre_Campagne.Count != 0)
                lstCentre = aviscoupure.Centre_Campagne.Select(u => u.CODE).ToList();

                int? NombreRestant = aviscoupure.NombreTotalDeClient;
                int? NombreTotalDuLot = aviscoupure.NombreTotalDeClient;
                if (aviscoupure.NombreFactureTotalClient == null) aviscoupure.NombreFactureTotalClient = 1;
                foreach (var item in leRegroupement)
                {
                    List<CsLclient> lstDonne = new List<CsLclient>();
                    if (lstCentre != null && lstCentre.Count != 0)
                    {

                        foreach (var items in lstCentre)
                        {
                            if (item.CODE == Enumere.RegroupementParticilier)
                            {
                                DataTable dt = CreationLotCampagneRegroupe(item.PK_ID, aviscoupure.referenceClientDebut, aviscoupure.OrdreTourneDebut,aviscoupure.ClientResilie, items, aviscoupure.Exigible, aviscoupure.MontantRelancable, true,aviscoupure.PeriodeDebut ,aviscoupure.PeriodeFin );
                                lstDonne.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dt));
                                if (lstDonne != null && lstDonne.Count != 0)
                                {
                                    lstDonne = lstDonne.Where(t => t.SOLDECLIENT >= aviscoupure.MontantRelancable).ToList();
                                    lstDonne.ForEach(t => t.REFEMNDOC = item.NOM);
                                    lstDonne.ForEach(t => t.POSTE = item.ADR1);
                                }
                            }
                            else
                            {
                                DataTable dt = CreationLotCampagneRegroupe(item.PK_ID, aviscoupure.referenceClientDebut, aviscoupure.OrdreTourneDebut, aviscoupure.ClientResilie, items, aviscoupure.Exigible, aviscoupure.MontantRelancable, false,aviscoupure.PeriodeDebut, aviscoupure.PeriodeFin);
                                lstDonne.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dt));

                                if (lstDonne != null && lstDonne.Count != 0)
                                {

                                    if (lstDonne.Sum(t => t.SOLDEFACTURE) < aviscoupure.MontantRelancable)
                                        return null;
                                    lstDonne.ForEach(t => t.REFEMNDOC = item.NOM);
                                    lstDonne.ForEach(t => t.POSTE = item.ADR1);
                                }
                            }
                        }
                    
                        if (lstDonne != null && lstDonne.Count != 0 && !IsPreavis)
                            SaveCampagne(lstDonne, item, 1);
                    }
                    else
                    {
                        if (item.CODE == Enumere.RegroupementParticilier)
                        {
                            DataTable dt = CreationLotCampagneRegroupe(item.PK_ID, aviscoupure.referenceClientDebut, aviscoupure.OrdreTourneDebut, false, string.Empty, aviscoupure.Exigible, aviscoupure.MontantRelancable, true, aviscoupure.PeriodeDebut, aviscoupure.PeriodeFin);
                            lstDonne.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dt));
                            if (lstDonne != null && lstDonne.Count != 0)
                            {
                                lstDonne = lstDonne.Where(t => t.SOLDECLIENT >= aviscoupure.MontantRelancable).ToList(); 
                                lstDonne.ForEach(t => t.REFEMNDOC = item.NOM);
                                lstDonne.ForEach(t => t.POSTE = item.ADR1);
                            }
                        }
                        else
                        {
                            DataTable dt = CreationLotCampagneRegroupe(item.PK_ID, aviscoupure.referenceClientDebut, aviscoupure.OrdreTourneDebut, false, string.Empty, aviscoupure.Exigible, aviscoupure.MontantRelancable, true, aviscoupure.PeriodeDebut, aviscoupure.PeriodeFin);
                            lstDonne.AddRange(Entities.GetEntityListFromQuery<CsLclient>(dt));
                            if (lstDonne != null && lstDonne.Count != 0)
                            {
                                if (lstDonne.Sum(t => t.SOLDEFACTURE) < aviscoupure.MontantRelancable)
                                    return null;
                                lstDonne.ForEach(t => t.REFEMNDOC = item.NOM);
                                lstDonne.ForEach(t => t.POSTE = item.ADR1);
                            }
                        }
                 
                        if (lstDonne != null && lstDonne.Count != 0 && !IsPreavis)
                            SaveCampagne(lstDonne, item, 1);
                    }
                    lstDonneFinale.AddRange(lstDonne);
                }
                return lstDonneFinale;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsAvisDeCoupureClient> ETAT_AVIS_DE_COUPURE(CsAvisCoupureEdition aviscoupure, aDisconnection dis, bool IsListe)
        {
            try
            {
                galadbEntities ctx = new galadbEntities();
                List<CATEGORIECLIENT> lstCateg = ctx.CATEGORIECLIENT.ToList();
                List<CsAvisDeCoupureClient> lstDonne = new List<CsAvisDeCoupureClient>();
                int? NombreRecu = 0;
                int? NombreRestant = aviscoupure.NombreTotalDeClient;
                int? NombreTotalDuLot = aviscoupure.NombreTotalDeClient;
                if (aviscoupure.NombreFactureTotalClient == null) aviscoupure.NombreFactureTotalClient = 1;

                string idtournee = DBBase.RetourneStringListeObject(aviscoupure.Tournees.Select(o => o.ToString()).ToList());
                string idCategorie = DBBase.RetourneStringListeObject(aviscoupure.Categories.Select(o => o.ToString()).ToList());
                string idCodeConso = DBBase.RetourneStringListeObject(aviscoupure.Consomateur.Select(o => o.ToString()).ToList());
                
                //foreach (int idtournee in aviscoupure.Tournees)
                //{
                    //foreach (int idCategorie in aviscoupure.Categories)
                    //{
                        //string LibelCateg = lstCateg.FirstOrDefault(t => t.PK_ID == idCategorie).LIBELLE;
                        //List<CsAvisDeCoupureClient> lstDuJet = new List<CsAvisDeCoupureClient>();
                        //if (aviscoupure.Consomateur == null || aviscoupure.Consomateur.Count == 0)
                        //{

                            List<CsAvisDeCoupureClient> lstDuJet = new List<CsAvisDeCoupureClient>();

                            DataTable dt = CreationLotCampagne(idtournee, idCategorie, null, aviscoupure.referenceClientDebut,
                                aviscoupure.referenceClientFin, aviscoupure.OrdreTourneDebut, aviscoupure.OrdreTourneFin, aviscoupure.ClientResilie,
                                aviscoupure.Exigible, aviscoupure.NombreFactureTotalClient, aviscoupure.SoldeMinimum, aviscoupure.Periode,
                                NombreRestant, aviscoupure.idAgentPia, aviscoupure.Matricule);
                            lstDuJet = Entities.GetEntityListFromQuery<CsAvisDeCoupureClient>(dt);
                            //foreach (CsAvisDeCoupureClient item in lstDuJet)
                            //{
                            //    item.FK_IDCATEGORIECLIENT = idCategorie;
                            //    item.FK_IDTOURNEE = idtournee;
                            //    item.CATEGORIE = LibelCateg;
                            //    item.NOMCONTROLER = aviscoupure.NomAgentPia + "(" + aviscoupure.AgentPia + ")";
                            //}
                            if (lstDuJet != null && lstDuJet.Count != 0 && NombreTotalDuLot != null)
                            {
                                NombreRecu = lstDuJet.First().NOMBRECLIENTTROUVE;
                                NombreRestant = NombreTotalDuLot - NombreRecu;
                            }
                            lstDonne.AddRange(lstDuJet);
                        //}
                        //else
                        //{
                        //    foreach (int idCodeConso in aviscoupure.Consomateur)
                        //    {
                        //        DataTable dt = CreationLotCampagne(idtournee, idCategorie, null, aviscoupure.referenceClientDebut,
                        //                        aviscoupure.referenceClientFin, aviscoupure.OrdreTourneDebut, aviscoupure.OrdreTourneFin, aviscoupure.ClientResilie,
                        //                        aviscoupure.Exigible, aviscoupure.NombreFactureTotalClient, aviscoupure.SoldeMinimum, aviscoupure.Periode,
                        //                        NombreRestant, aviscoupure.idAgentPia, aviscoupure.Matricule);
                        //        lstDuJet = Entities.GetEntityListFromQuery<CsAvisDeCoupureClient>(dt);
                        //        if (lstDuJet != null && lstDuJet.Count != 0 && NombreTotalDuLot != null)
                        //        {
                        //            NombreRecu = lstDuJet.First().NOMBRECLIENTTROUVE;
                        //            NombreRestant = NombreTotalDuLot - NombreRecu;
                        //        }
                        //        foreach (CsAvisDeCoupureClient item in lstDuJet)
                        //        {
                        //            item.FK_IDCATEGORIECLIENT = idCategorie;
                        //            item.FK_IDTOURNEE = idtournee;
                        //            item.CATEGORIE = LibelCateg;
                        //            item.NOMCONTROLER = aviscoupure.NomAgentPia;

                        //        }
                        //        lstDonne.AddRange(lstDuJet);
                        //    }
                        //}
                    //}
                //}
                if (lstDonne != null && lstDonne.Count != 0)
                    MiseAjourLclientCampagneDetailCampage(lstDonne, aviscoupure);
                lstDonne.ForEach(y => y.IDCOUPURE = aviscoupure.IdCoupure);
                return lstDonne;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        #endregion

        #region Sylla 20/06/2016
        public List<string> RecupererPeriodeDePlage(string PeriodeDebut, string PeriodeFin)
        {
            try
            {
                return RecouvProcedures.RecupererPeriodeDePlage(PeriodeDebut, PeriodeFin);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AnnulerCampagne(string numeroCampagne)
        {
            try
            {
                return RecouvProcedures.AnnulerCampagne(numeroCampagne);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public List<CsDetailCampagne> RechercheClientCampagneScgc(string CodeSite,string Centre, string Client, string Ordre, int TypeEdition)
        {
            cn = new SqlConnection(ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURSAISIFRAISSCGC";

            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(Centre) ? null : Centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(Client) ? null : Client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(Ordre) ? null : Ordre;

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

        public List<CsDetailCampagne> RechercheClientCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, string Centre, string Client, string Ordre, int TypeEdition)
        {
            cn = new SqlConnection(ConnectionString);

            int? idPiacampagne = null;
            if (IdPia != 0) idPiacampagne = IdPia;
            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (TypeEdition ==1)
            cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURSAISIINDEX";
            else if (TypeEdition == 2)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURSAISIFRAIS";
            else if (TypeEdition == 3)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURANNULATION";
            else if (TypeEdition == 4)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURRELANCE";
            else if (TypeEdition == 5)
                cmd.CommandText = "SPX_RECOUV_CLIENTAREMETRE";
            else if (TypeEdition == 6)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTINDEXSAISI";
            else if (TypeEdition == 7)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECLIENTPOURAUTORISATION";
            else if (TypeEdition == 8)
                cmd.CommandText = "SPX_RECOUVGC_CLIENTFRAIS";
            else if (TypeEdition == 9)
                cmd.CommandText = "SPX_RECOUVGC_CLIENTINDEX";

            
            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar ,3).Value = CodeSite;
            cmd.Parameters.Add("@NumCampagne", SqlDbType.VarChar, 20).Value =string.IsNullOrEmpty( IdCampagne)?null : IdCampagne ;
            cmd.Parameters.Add("@IdPia", SqlDbType.Int).Value = idPiacampagne;
            cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
            cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
            cmd.Parameters.Add("@Centre", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(Centre) ? null : Centre;
            cmd.Parameters.Add("@Client", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(Client) ? null : Client;
            cmd.Parameters.Add("@Ordre", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(Ordre) ? null : Ordre; 

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

        public List<CsCAMPAGNE> RechercheCampagne(string CodeSite, string IdCampagne, int IdPia, DateTime? DateDebut, DateTime? DateFin, int TypeEdition)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            if (TypeEdition == 1)
                cmd.CommandText = "SPX_RECOUV_RECHERCHECAMPAGNE";
            cmd.Parameters.Add("@CodeSite", SqlDbType.VarChar, 3).Value = CodeSite;
            cmd.Parameters.Add("@NumCampagne", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(IdCampagne) ? null : IdCampagne;
            cmd.Parameters.Add("@IdPia", SqlDbType.Int).Value = IdPia;
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
                return Entities.GetEntityListFromQuery<CsCAMPAGNE>(dt);
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

        private bool ValidationAnnulationFrais(CsDetailCampagne campagne)
        {
            cn = new SqlConnection(ConnectionString);

            cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandTimeout = 18000;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SPX_RECOUV_ANNULATIONFRAIS";
            if (cmd.Parameters != null && cmd.Parameters.Count != 0) cmd.Parameters.Clear();
            cmd.Parameters.Add("@IdCoupure", SqlDbType.VarChar, 20).Value = campagne.IDCOUPURE;
            cmd.Parameters.Add("@IdClient", SqlDbType.Int).Value = campagne.FK_IDCLIENT;
            cmd.Parameters.Add("@Matricule", SqlDbType.VarChar, 6).Value = campagne.USERMODIFICATION;
            cmd.Parameters.Add("@Motif", SqlDbType.VarChar, 50).Value = campagne.MOTIFANNULATION;
            cmd.Parameters.Add("@Montant", SqlDbType.Decimal).Value = campagne.FRAIS;

            DBBase.SetDBNullParametre(cmd.Parameters);
            try
            {
                int ret = -1;

                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                   ret = cmd.ExecuteNonQuery();
                return (ret > 0);
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

        #region Precontentieux
            private DataTable CreationLotCampagnePrecontentieux(int idcentre, DateTime? DateDebut, DateTime? DateFin, decimal SoldeDu)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAMPAGNE_PRECONTENTIEUX";

                cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;
                cmd.Parameters.Add("@DateDebut", SqlDbType.DateTime).Value = DateDebut;
                cmd.Parameters.Add("@DateFin", SqlDbType.DateTime).Value = DateFin;
                cmd.Parameters.Add("@SoldeDue", SqlDbType.Decimal).Value = SoldeDu;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    //if (reader.Read())
                    dt.Load(reader);

                    return dt;

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
            public List<CsDetailCampagnePrecontentieux> ClientSoldeSuitePrecontentieux(int idcentre, string IdCampagne)
            {
                cn = new SqlConnection(ConnectionString);

                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_CAMPAGNE_CLIENTSOLDE";

                cmd.Parameters.Add("@idCentre", SqlDbType.Int).Value = idcentre;
                cmd.Parameters.Add("@IdCampagne", SqlDbType.VarChar).Value = IdCampagne;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    //if (reader.Read())
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt);
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
            public List<CsDetailCampagnePrecontentieux> CREE_CAMPAGNE_PRECONTENTIEUX(int idcentre, DateTime? DateDebut, DateTime? DateFin, decimal SoldeDu, int Fk_idMatricule, string matricule)
            {
                try
                {
                    List<CsDetailCampagnePrecontentieux> lstDonne = new List<CsDetailCampagnePrecontentieux>();
                    DataTable dt = CreationLotCampagnePrecontentieux(idcentre, DateDebut, DateFin, SoldeDu);
                    lstDonne = Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt);

                    if (lstDonne != null && lstDonne.Count != 0)
                        SaveCampagnePrecontencieux(lstDonne, Fk_idMatricule, matricule);
                    return lstDonne;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsCAMPAGNE> RetourneCampagnePrecontentieux(List<int> lstCentre)
            {
                try
                {
                    DataTable dt = RecouvProcedures.RetourneCampagnePrecontentieux(lstCentre);
                    return Entities.GetEntityListFromQuery<CsCAMPAGNE>(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDetailCampagnePrecontentieux> RetourneDetailPrecontentieux(int idCampagne)
            {
                try
                {
                    DataTable dt = RecouvProcedures.RetourneDetailCampagnePrecontentieux(idCampagne);
                    return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public bool MajDetailPrecontentieux(List<CsDetailCampagnePrecontentieux> lstIdDetailCamp)
            {
                try
                {
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        List<DETAILCAMPAGNEPRENCONTENTIEUX> lstDetail = Galatee.Tools.Utility.ConvertListType<DETAILCAMPAGNEPRENCONTENTIEUX, CsDetailCampagnePrecontentieux>(lstIdDetailCamp);
                        return Entities.UpdateEntity<DETAILCAMPAGNEPRENCONTENTIEUX>(lstDetail);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public bool InsererDechargePrecontentieux(CsPrecontentieuxDechargement Decharge)
            {
                try
                {
                    int result = -1;
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        DECHARGEPRECONTENSIEUX laDecharge = new DECHARGEPRECONTENSIEUX();
                        List<AUTRECLIENTPRECONTENCIEUX> lstAutreClient = new List<AUTRECLIENTPRECONTENCIEUX>();

                        laDecharge = Entities.ConvertObject<DECHARGEPRECONTENSIEUX, CsPrecontentieuxDechargement>(Decharge);

                        if (Decharge.ListAutreClient != null && Decharge.ListAutreClient.Count != 0)
                            lstAutreClient = Entities.ConvertObject<AUTRECLIENTPRECONTENCIEUX, CsPrecontentieuxAutreClient>(Decharge.ListAutreClient.ToList());

                        if (laDecharge != null && laDecharge.CENTRE != string.Empty)
                            Entities.InsertEntity<DECHARGEPRECONTENSIEUX>(laDecharge, ctx);
                        if (lstAutreClient != null && lstAutreClient.Count != 0)
                            Entities.InsertEntity<AUTRECLIENTPRECONTENCIEUX>(lstAutreClient, ctx);
                        result = ctx.SaveChanges();
                        return result == -1 ? false : true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDetailCampagnePrecontentieux> RechercheAbonneLier(CsCAMPAGNE lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {


                        var query = (
                            from camp in context.DETAILCAMPAGNEPRENCONTENTIEUX
                            join y in context.AUTRECLIENTPRECONTENCIEUX on camp.FK_IDCLIENT equals y.FK_IDCLIENTPRECONTENTIEUX
                            where
                            camp.IDCAMPAGNE == lesCampagnes.IDCOUPURE
                            select new
                            {
                                y.CLIENT1.CENTRE,
                                CLIENT = y.CLIENT.REFCLIENT,
                                y.CLIENT.ORDRE,
                                y.CLIENT.NOMABON,
                                ADRESSE = y.CLIENT.ADRMAND1,
                                y.CLIENT.AG.RUES,
                                y.CLIENT.AG.PORTE,
                            });
                        DataTable dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt);

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDetailCampagnePrecontentieux> REEDITION_CAMPAGNE_PRECONTENTIEUX(string IdCampagne)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {


                        var query = (
                            from camp in context.DETAILCAMPAGNEPRENCONTENTIEUX
                            where
                            camp.IDCAMPAGNE == IdCampagne
                            select new
                            {
                                camp.IDCAMPAGNE,
                                camp.PK_ID,
                                camp.CENTRE,
                                camp.CLIENT,
                                camp.ORDRE,
                                camp.TOURNEE,
                                camp.ORDTOUR,
                                camp.CATEGORIE,
                                camp.SOLDEDUE,
                                camp.SOLDECLIENT,
                                camp.CLIENT1.AG.RUES,
                                camp.CLIENT1.AG.PORTE,
                                ADRESSE = camp.CLIENT1.ADRMAND1,
                                camp.USERCREATION,
                                camp.DATECREATION,
                                camp.DATEMODIFICATION,
                                camp.USERMODIFICATION,
                                camp.FK_IDCENTRE,
                                camp.FK_IDCLIENT,
                                camp.FK_IDTOURNEE,
                                camp.FK_IDCATEGORIE,
                                camp.FK_IDCAMPAGNE,
                                camp.ISINVITATIONEDITER,
                                camp.DATERESILIATION,
                                NOMABON = camp.CLIENT1.NOMABON,
                                camp.DATERDV
                            });
                        DataTable dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt);

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public CsDetailCampagnePrecontentieux RetourneClientByReferenceOrdrePrecontentieux(int idCentre, string client, string Ordre)
            {
                try
                {

                    using (galadbEntities context = new galadbEntities())
                    {
                        IEnumerable<object> query = null;
                        var _clientP = context.DETAILCAMPAGNEPRENCONTENTIEUX  ;
                        query =
                        from x in _clientP
                        where
                        x.CLIENT  == client &&
                        x.ORDRE == Ordre &&
                        idCentre == x.FK_IDCENTRE
                        select new
                        {
                            x.IDCAMPAGNE,
                            x.PK_ID,
                            x.CENTRE,
                            x.CLIENT,
                            x.ORDRE,
                            x.CLIENT1.NOMABON,
                            ADRESSE=x.CLIENT1.ADRMAND1 ,
                            x.CLIENT1.AG.RUE,
                            x.CLIENT1.AG.PORTE,
                            x.TOURNEE,
                            x.ORDTOUR,
                            x.CATEGORIE,
                            x.SOLDEDUE,
                            x.SOLDECLIENT,
                            x.USERCREATION,
                            x.DATECREATION,
                            x.DATEMODIFICATION,
                            x.USERMODIFICATION,
                            x.FK_IDCENTRE,
                            x.FK_IDCLIENT,
                            x.FK_IDTOURNEE,
                            x.FK_IDCATEGORIE,
                            x.FK_IDCAMPAGNE,
                            x.ISINVITATIONEDITER,
                            x.DATERESILIATION,
                            x.DATERDV 
                        };
                        DataTable dt= Galatee.Tools.Utility.ListToDataTable(query);
                        return Entities.GetEntityFromQuery<CsDetailCampagnePrecontentieux>(dt);

                    };
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        #endregion

            public void Insert_INDEXCAMPAGNE(CsDetailCampagne LeIdxCampage, SqlCommand cmds)
            {
                cmds.CommandTimeout = 1800;
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "SPX_RECOUV_INSERT_INDEXCAMPAGNE";
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                cmds.Parameters.Add("@IDCOUPURE", SqlDbType.VarChar, 16).Value = LeIdxCampage.IDCOUPURE;
                cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = LeIdxCampage.CENTRE;
                cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = LeIdxCampage.CLIENT;
                cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = LeIdxCampage.ORDRE;
                cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = LeIdxCampage.MONTANT;
                cmds.Parameters.Add("@INDEXO", SqlDbType.Int).Value = LeIdxCampage.INDEX;
                cmds.Parameters.Add("@INDEXE", SqlDbType.Int).Value = LeIdxCampage.INDEX;
                cmds.Parameters.Add("@CODEOBSERVATION", SqlDbType.VarChar, 2).Value = LeIdxCampage.TYPECOUPURE;
                cmds.Parameters.Add("@DATECOUPURE", SqlDbType.DateTime).Value = LeIdxCampage.DATECOUPURE;
                cmds.Parameters.Add("@DATERDV", SqlDbType.DateTime).Value = LeIdxCampage.DATERDV;
                cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 6).Value = LeIdxCampage.USERCREATION;
                cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = LeIdxCampage.FK_IDCENTRE;
                cmds.Parameters.Add("@FK_IDCAMPAGNE", SqlDbType.Int).Value = LeIdxCampage.FK_IDCAMPAGNE;
                cmds.Parameters.Add("@FK_IDOBSERVATION", SqlDbType.Int).Value = LeIdxCampage.FK_IDOBSERVATION;
                cmds.Parameters.Add("@FK_TYPECOUPURE", SqlDbType.Int).Value = LeIdxCampage.FK_TYPECOUPURE;
                cmds.Parameters.Add("@COMPTEUR", SqlDbType.VarChar, 20).Value = LeIdxCampage.COMPTEUR;
                cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = LeIdxCampage.FK_IDCLIENT;

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
            public void Update_Detail(CsDetailCampagne LeIdxCampage, SqlCommand cmds)
            {
                cmds.CommandTimeout = 1800;
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "SPX_RECOUV_UPDATE_DETAILCAMPAGNE";
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();


                cmds.Parameters.Add("@IDCOUPURE", SqlDbType.VarChar, 16).Value = LeIdxCampage.IDCOUPURE;
                cmds.Parameters.Add("@ISAUTORISER", SqlDbType.Bit).Value = LeIdxCampage.ISAUTORISER;
                cmds.Parameters.Add("@MOTIFAUTORISATION", SqlDbType.Bit).Value = LeIdxCampage.MOTIFAUTORISATION;
                cmds.Parameters.Add("@FRAIS", SqlDbType.Decimal).Value = LeIdxCampage.FRAIS;
                cmds.Parameters.Add("@ISANNULATIONFRAIS", SqlDbType.Bit).Value = LeIdxCampage.ISANNULATIONFRAIS;
                cmds.Parameters.Add("@MOTIFANNULATION", SqlDbType.VarChar, 50).Value = LeIdxCampage.MOTIFANNULATION;
                cmds.Parameters.Add("@DATERDV", SqlDbType.DateTime).Value = LeIdxCampage.DATERDV;
                cmds.Parameters.Add("@RELANCE", SqlDbType.Int).Value = LeIdxCampage.RELANCE;
                cmds.Parameters.Add("@FK_IDTYPECOUPURE", SqlDbType.Int).Value = LeIdxCampage.FK_IDTYPECOUPURE;
                cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = LeIdxCampage.FK_IDCLIENT;
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
            public void InsertOrUpdateLCLIENT(CsLclient Lclient, SqlCommand cmds)
            {
                cmds.CommandTimeout = 1800;
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "SPX_RECOUV_INSERTORUPDATE_LCLIENT";
                if (cmds.Parameters != null && cmds.Parameters.Count != 0) cmds.Parameters.Clear();

                int? IdUser = null;
                if (Lclient.FK_IDMOTIFCHEQUEINPAYE != 0) IdUser = Lclient.FK_IDMOTIFCHEQUEINPAYE;

                int? IdMoratoire = null;
                if (Lclient.FK_IDMORATOIRE != 0) IdUser = Lclient.FK_IDMORATOIRE;

                int? IdMotifChq = null;
                if (Lclient.FK_IDMOTIFCHEQUEINPAYE != 0) IdMotifChq = Lclient.FK_IDMOTIFCHEQUEINPAYE;

                int? idPoste = null;
                if (Lclient.FK_IDPOSTE != 0) idPoste = Lclient.FK_IDPOSTE;


                cmds.Parameters.Add("@CENTRE", SqlDbType.VarChar, 3).Value = Lclient.CENTRE;
                cmds.Parameters.Add("@CLIENT", SqlDbType.VarChar, 20).Value = Lclient.CLIENT;
                cmds.Parameters.Add("@ORDRE", SqlDbType.VarChar, 2).Value = Lclient.ORDRE;
                cmds.Parameters.Add("@REFEM", SqlDbType.VarChar, 6).Value = Lclient.REFEM;
                cmds.Parameters.Add("@NDOC", SqlDbType.VarChar, 6).Value = Lclient.NDOC;
                cmds.Parameters.Add("@COPER", SqlDbType.VarChar, 3).Value = Lclient.COPER;
                cmds.Parameters.Add("@DENR", SqlDbType.DateTime).Value = Lclient.DENR;
                cmds.Parameters.Add("@EXIG", SqlDbType.Int).Value = Lclient.EXIG;
                cmds.Parameters.Add("@MONTANT", SqlDbType.Decimal).Value = Lclient.MONTANT;
                cmds.Parameters.Add("@CAPUR", SqlDbType.VarChar, 1).Value = Lclient.CAPUR;
                cmds.Parameters.Add("@CRET", SqlDbType.VarChar, 1).Value = Lclient.CRET;
                cmds.Parameters.Add("@MODEREG", SqlDbType.VarChar, 2).Value = Lclient.MODEREG;
                cmds.Parameters.Add("@DC", SqlDbType.VarChar, 1).Value = Lclient.DC;
                cmds.Parameters.Add("@ORIGINE", SqlDbType.VarChar, 3).Value = Lclient.ORIGINE;
                cmds.Parameters.Add("@CAISSE", SqlDbType.VarChar, 3).Value = Lclient.CAISSE;
                cmds.Parameters.Add("@ECART", SqlDbType.Decimal).Value = Lclient.ECART;
                cmds.Parameters.Add("@MOISCOMPT", SqlDbType.VarChar, 6).Value = Lclient.MOISCOMPT;
                cmds.Parameters.Add("@TOP1", SqlDbType.VarChar, 2).Value = Lclient.TOP1;
                cmds.Parameters.Add("@EXIGIBILITE", SqlDbType.DateTime).Value = Lclient.EXIGIBILITE;
                cmds.Parameters.Add("@FRAISDERETARD", SqlDbType.Decimal).Value = Lclient.FRAISDERETARD;
                cmds.Parameters.Add("@REFERENCEPUPITRE", SqlDbType.Int).Value = Lclient.REFERENCEPUPITRE;
                cmds.Parameters.Add("@IDLOT", SqlDbType.Int).Value = Lclient.IDLOT;
                cmds.Parameters.Add("@DATEVALEUR", SqlDbType.DateTime).Value = Lclient.DATEVALEUR;
                cmds.Parameters.Add("@REFERENCE", SqlDbType.VarChar, 6).Value = Lclient.REFERENCE;
                cmds.Parameters.Add("@REFEMNDOC", SqlDbType.VarChar, 12).Value = Lclient.REFEMNDOC;
                cmds.Parameters.Add("@ACQUIT", SqlDbType.VarChar, 9).Value = Lclient.ACQUIT;
                cmds.Parameters.Add("@MATRICULE", SqlDbType.VarChar, 20).Value = Lclient.MATRICULE;
                cmds.Parameters.Add("@TAXESADEDUIRE", SqlDbType.Decimal).Value = Lclient.TAXESADEDUIRE;
                cmds.Parameters.Add("@DATEFLAG", SqlDbType.DateTime).Value = Lclient.DATEFLAG;
                cmds.Parameters.Add("@MONTANTTVA", SqlDbType.Decimal).Value = Lclient.MONTANTTVA;
                cmds.Parameters.Add("@IDCOUPURE", SqlDbType.VarChar, 16).Value = Lclient.IDCOUPURE;
                cmds.Parameters.Add("@AGENT_COUPURE", SqlDbType.VarChar, 20).Value = Lclient.AGENT_COUPURE;
                cmds.Parameters.Add("@RDV_COUPURE", SqlDbType.DateTime).Value = Lclient.RDV_COUPURE;
                cmds.Parameters.Add("@NUMCHEQ", SqlDbType.VarChar, 10).Value = Lclient.NUMCHEQ;
                cmds.Parameters.Add("@OBSERVATION_COUPURE", SqlDbType.VarChar, 512).Value = Lclient.OBSERVATION_COUPURE;
                cmds.Parameters.Add("@USERCREATION", SqlDbType.VarChar, 20).Value = Lclient.USERCREATION;
                cmds.Parameters.Add("@USERMODIFICATION", SqlDbType.VarChar, 20).Value = Lclient.USERMODIFICATION;
                cmds.Parameters.Add("@BANQUE", SqlDbType.VarChar, 6).Value = Lclient.BANQUE;
                cmds.Parameters.Add("@GUICHET", SqlDbType.VarChar, 6).Value = Lclient.GUICHET;
                cmds.Parameters.Add("@FK_IDCENTRE", SqlDbType.Int).Value = Lclient.FK_IDCENTRE;
                //cmds.Parameters.Add("@FK_IDADMUTILISATEUR", SqlDbType.Int).Value = IdUser;
                cmds.Parameters.Add("@FK_IDADMUTILISATEUR", SqlDbType.Int).Value = Lclient.FK_IDADMUTILISATEUR;
                cmds.Parameters.Add("@FK_IDCOPER", SqlDbType.Int).Value = Lclient.FK_IDCOPER;
                cmds.Parameters.Add("@FK_IDLIBELLETOP", SqlDbType.Int).Value = Lclient.FK_IDLIBELLETOP;
                cmds.Parameters.Add("@FK_IDCLIENT", SqlDbType.Int).Value = Lclient.FK_IDCLIENT;
                cmds.Parameters.Add("@FK_IDPOSTE", SqlDbType.Int).Value = idPoste;
                cmds.Parameters.Add("@POSTE", SqlDbType.VarChar, 50).Value = Lclient.POSTE;
                cmds.Parameters.Add("@DATETRANS", SqlDbType.DateTime).Value = Lclient.DATETRANS;
                cmds.Parameters.Add("@ISNONENCAISSABLE", SqlDbType.Bit).Value = Lclient.ISNONENCAISSABLE;
                cmds.Parameters.Add("@FK_IDMORATOIRE", SqlDbType.Int).Value = IdMoratoire;
                cmds.Parameters.Add("@FK_IDMOTIFCHEQUEINPAYE", SqlDbType.Int).Value = IdMotifChq;

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

            public void InsertIndex(CsDetailCampagne campagne, SqlCommand laCommande)
            {
                try
                {
                    Update_Detail(campagne, laCommande);
                    Insert_INDEXCAMPAGNE(campagne, laCommande);
                    CsLclient laFacture = GetElementDeFraisCs(campagne);
                    InsertOrUpdateLCLIENT(laFacture, laCommande);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public string InsertListIndex(List<CsDetailCampagne> campagnes)
            {
                SqlCommand laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);

                try
                {
                    foreach (CsDetailCampagne item in campagnes)
                    {
                        //laCommande = new SqlCommand();
                        InsertIndex(item, laCommande);
                        laCommande.Transaction.Commit();
                    }
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
                        laCommande.Connection.Close(); // Fermeture de la connection 
                    laCommande.Dispose();
                }
            }

            private CsLclient GetElementDeFraisCs(CsDetailCampagne Campagne)
            {
                CsLclient Frais = new CsLclient();
                try
                {
                    Frais.CENTRE = Campagne.CENTRE;
                    Frais.CLIENT = Campagne.CLIENT;
                    Frais.ORDRE = Campagne.ORDRE;
                    Frais.REFEM = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                    Frais.IDCOUPURE = Campagne.IDCOUPURE;
                    Frais.COPER = Enumere.CoperFRP;
                    Frais.DENR = DateTime.Today.Date;
                    Frais.EXIGIBILITE = DateTime.Today.Date;
                    Frais.DATECREATION = DateTime.Now;
                    Frais.DATEMODIFICATION = null;
                    Frais.DC = Enumere.Debit;
                    Frais.MATRICULE = Campagne.MATRICULE;
                    Frais.MOISCOMPT = DateTime.Today.Date.Year.ToString("0000") + DateTime.Today.Date.Month.ToString("00");
                    Frais.MONTANT = Campagne.MONTANTFRAIS;
                    Frais.TOP1 = "8";
                    Frais.NDOC = new DBAccueil().NumeroFacture(Campagne.FK_IDCENTRE);
                    Frais.FK_IDCENTRE = Campagne.FK_IDCENTRE;
                    Frais.FK_IDCLIENT = Campagne.FK_IDCLIENT;
                    Frais.FK_IDLIBELLETOP = Campagne.FK_IDLIBELLETOP;
                    Frais.FK_IDCOPER = Campagne.FK_IDCOPER;
                    Frais.FK_IDADMUTILISATEUR = Campagne.FK_IDADMUTILISATEUR;
                    Frais.ISNONENCAISSABLE = Campagne.ISNONENCAISSABLE;
                    return Frais;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            public string InsererLclient(List<CsLclient> Lesfacture)
            {
                SqlCommand laCommande = new SqlCommand();
                laCommande = DBBase.InitTransaction(ConnectionString);

                try
                {
                    foreach (CsLclient item in Lesfacture)
                    {
                        //laCommande = new SqlCommand();
                        //laCommande = DBBase.InitTransaction(ConnectionString);

                        CsDetailCampagne detailCampagne = new CsDetailCampagne
                        {
                            IDCOUPURE = item.IDCOUPURE,
                            ISAUTORISER = false,
                            MOTIFAUTORISATION = string.Empty,
                            FRAIS = item.MONTANT,
                            ISANNULATIONFRAIS = false,
                            MOTIFANNULATION = string.Empty,
                            DATERDV = null,
                            RELANCE = 0,
                            FK_IDTYPECOUPURE = item.FK_IDTYPEDEVIS,
                            FK_IDCLIENT = item.FK_IDCLIENT
                        };

                        Update_Detail(detailCampagne, laCommande);
                        InsertOrUpdateLCLIENT(item, laCommande);
                        laCommande.Transaction.Commit();
                    }
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
                        laCommande.Connection.Close(); // Fermeture de la connection 
                    laCommande.Dispose();
                }
            }

        #region 11-05-2017
            public List<CsLclient> MiseAjourComptAjustement(List<CsDetailLot> LstDetailPaiemenent, int Id)
            {
                LOTCOMPTECLIENT CAMPAGNEGC = new LOTCOMPTECLIENT();
                DETAILLOT PAIEMENTGC = new DETAILLOT();
                try
                {
                    cmd = new SqlCommand();
                    cn = new SqlConnection(ConnectionString);
                    cmd.Connection = cn;
                    cmd.CommandTimeout = 600;
                    cmd.Connection.Open();
                    cmd.Transaction = cmd.Connection.BeginTransaction();


                    using (galadbEntities context = new galadbEntities())
                    {
                        int IdPaiement = LstDetailPaiemenent.FirstOrDefault().FK_IDLOTCOMPECLIENT;
                        List<CsMapperTranscaisB> TRANSCAISB = new List<CsMapperTranscaisB>();
                        CAMPAGNEGC = context.LOTCOMPTECLIENT.FirstOrDefault(c => c.PK_ID == Id);
                        //MANDATEMENTGC MANDATEMENTGC = context.MANDATEMENTGC.FirstOrDefault(c => c.FK_IDCAMPAGNA == Id);
                        PAIEMENTGC = context.DETAILLOT.FirstOrDefault(c => c.PK_ID == IdPaiement);
                        ADMUTILISATEUR ADMUTILISATEUR = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == CAMPAGNEGC.USERCREATION);
                        MODEREG modreg = context.MODEREG.FirstOrDefault(u => u.CODE == "4");
                        COPER COPER = context.COPER.FirstOrDefault(c => c.CODE == "010");
                        //LCLIENT facture = context.LCLIENT.FirstOrDefault(c => c.CODE == "010");
                        List<CsLclient> clients = new List<CsLclient>();

                        //string NumeroDemande = CAMPAGNEGC.NUMEROCAMPAGNE;
                        //string Matricule = MANDATEMENTGC.USERCREATION;

                        foreach (CsDetailLot item in LstDetailPaiemenent)
                        {
                            LCLIENT facture = context.LCLIENT.FirstOrDefault(c => c.NDOC == item.NDOC);
                            CsMapperTranscaisB transB = new CsMapperTranscaisB();
                            CLIENT LeClient = context.CLIENT.FirstOrDefault(l => l.PK_ID == item.FK_IDCLIENT);

                            //transB.ACQUIT = MANDATEMENTGC.NUMEROMANDATEMENT.Substring(0, 9);
                            transB.CENTRE = item.CENTRE;
                            transB.CLIENT = item.CLIENT;
                            transB.ORDRE = item.ORDRE;
                            transB.NDOC = item.NDOC;
                            transB.REFEM = item.DATECREATION.Month.ToString().PadLeft(2);
                            transB.MONTANT = item.MONTANT_AJUSTEMENT;
                            transB.DC = "C";
                            transB.COPER = COPER.CODE;
                            transB.PERCU = item.MONTANT_AJUSTEMENT;
                            transB.RENDU = 0;
                            transB.MODEREG = modreg.CODE;
                            transB.PLACE = "-";
                            transB.DTRANS = DateTime.Now;
                            transB.BANQUE = "------";
                            transB.GUICHET = "------";
                            transB.ORIGINE = item.CENTRE;
                            transB.ECART = 0;
                            transB.MOISCOMPT = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                            transB.TOP1 = "8";
                            transB.MATRICULE = CAMPAGNEGC.USERCREATION;
                            transB.DATEENCAISSEMENT = DateTime.Now;
                            transB.DATECREATION = DateTime.Now;
                            transB.USERCREATION = CAMPAGNEGC.USERCREATION;
                            transB.DATEVALEUR = DateTime.Now;

                            // valuer les foreign key
                            transB.FK_IDCENTRE = LeClient.FK_IDCENTRE;
                            transB.FK_IDCOPER = COPER.PK_ID;
                            transB.FK_IDLIBELLETOP = context.LIBELLETOP.FirstOrDefault(l => l.CODE == "8").PK_ID;
                            transB.FK_IDMODEREG = modreg.PK_ID;
                            transB.FK_IDAGENTSAISIE = ADMUTILISATEUR.PK_ID;
                            transB.FK_IDLCLIENT = facture.PK_ID;

                            //PAIEMENT.EST_MIS_A_JOUR = true;
                            //LISTEPAIEMENT.Add(PAIEMENT);
                            TRANSCAISB.Add(transB);
                            //LCLIENTS.Add(LCLIENT);

                            CsLclient leclient = new CsLclient();
                            //leclient.ACQUIT = PAIEMENTGC.NUMEROMANDATEMENT.Substring(0, 9); 
                            leclient.NDOC = item.NDOC;
                            leclient.PK_ID = item.PK_ID;
                            leclient.MONTANT = item.MONTANT_AJUSTEMENT;
                            leclient.CENTRE = item.CENTRE;
                            leclient.CLIENT = item.CLIENT;
                            leclient.ORDRE = item.ORDRE;
                            leclient.REFEM = transB.REFEM;
                            leclient.NOM = LeClient.NOMABON;
                            //leclient.NUMDEVIS = PAIEMENTGC.NumAvisCredit;
                            //leclient.NUMDEM = PAIEMENTGC.NUMEROMANDATEMENT;
                            clients.Add(leclient);

                        }

                        CAMPAGNEGC.STATUS = "2";
                        //PAIEMENTGC.STATUT = false;

                        //Entities.UpdateEntity<PAIEMENTCAMPAGNEGC>(PAIEMENTGC, context);
                        Entities.UpdateEntity<LOTCOMPTECLIENT>(CAMPAGNEGC, context);
                        context.SaveChanges();

                        DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(TRANSCAISB);
                        Galatee.Tools.Utility.BulkInsertTable("TRANSCAISB", TablePere, cmd.Connection, cmd.Transaction);
                        cmd.Transaction.Commit();
                        //new DbWorkFlow().ExecuterActionSurDemandeTransction(NumeroDemande, Enumere.TRANSMETTRE, Matricule, string.Empty, context);
                        //SupprimeDoublons(NumeroDemande);
                        return clients;
                    }
                }
                catch (Exception ex)
                {
                    if (CAMPAGNEGC.STATUS == "2")
                    {
                        using (galadbEntities ctx = new galadbEntities())
                        {
                            CAMPAGNEGC.STATUS = "1";
                            //PAIEMENTGC.EST_MIS_A_JOUR = false;
                            //Entities.UpdateEntity<PAIEMENTCAMPAGNEGC>(PAIEMENTGC, ctx);
                            Entities.UpdateEntity<LOTCOMPTECLIENT>(CAMPAGNEGC, ctx);
                        }
                    }
                    throw ex;
                }
            }
            public List<CsLotComptClient> LoadAllAjustement()
            {
                try
                {
                    return RecouvProcedures.LoadAllAjustement();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public int SaveAjustement(List<CsLotComptClient> ListeAjustementToUpdate_, List<CsLotComptClient> ListeAjustementToInserte_, List<CsLotComptClient> ListeAjustementToDelete_)
            {
                try
                {
                    return RecouvProcedures.SaveAjustement(ListeAjustementToUpdate_, ListeAjustementToInserte_, ListeAjustementToDelete_);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //return 0;
                }
            }
        #region 05/06/2017
            public List<string> MiseAJourCategorie7(List<string> lines, string Matricule)
            {
                try
                {
                    List<string> AgentInconuCommeCommeClient = new List<string>();
                    
                    using (galadbEntities context = new galadbEntities())
                    {
                        ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext.CommandTimeout = 600000;

                        cmd = new SqlCommand();
                        cn = new SqlConnection(ConnectionString+";Connection Timeout=600000");
                        cmd.Connection = cn;
                        cmd.CommandTimeout = 600000;
                        cmd.Connection.Open();
                        cmd.Transaction = cmd.Connection.BeginTransaction();
                        string entete = lines.First();
                        string pied = lines.Last();
                        TRAITEMENT_FICHIER FICHIER_DEJA_TRAITE = context.TRAITEMENT_FICHIER.FirstOrDefault(t => t.ID_FICHIER == entete + "|" + pied);

                      if (FICHIER_DEJA_TRAITE==null)
	                    {
		                      MODEREG modreg = context.MODEREG.FirstOrDefault(u => u.CODE == "4");
                            COPER COPER = context.COPER.FirstOrDefault(c => c.CODE == "010");
                            ADMUTILISATEUR ADMUTILISATEUR = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == Matricule);
                            List<CsMapperTranscaisB> TRANSCAISS = new List<CsMapperTranscaisB>();
                            TRAITEMENT_FICHIER Info_De_Traitement_De_Fichier = new TRAITEMENT_FICHIER();
                            //List<REJET_DE_TRAITEMENT> List_De_Rejet_Du_Traitement_de_Fichier = new List<REJET_DE_TRAITEMENT>();
                            REJET_DE_TRAITEMENT Rejet_De_Traitement=new REJET_DE_TRAITEMENT();


                            //Info de traitement du fichier
                            Info_De_Traitement_De_Fichier.ID_FICHIER = lines.First() + "|" + lines.Last();
                            Info_De_Traitement_De_Fichier.DATE_CREATION = DateTime.Now;
                            Info_De_Traitement_De_Fichier.DATE_MODIFICATION = DateTime.Now;
                            Info_De_Traitement_De_Fichier.DESCIPTION = "Mise à jour des client de la catégorie 7";
                            Info_De_Traitement_De_Fichier.USER_CREATION = Matricule;
                            Info_De_Traitement_De_Fichier.USER_MODIFICATION = Matricule;


                            lines.Remove(lines.First());
                            lines.Remove(lines.Last());
                            foreach (var item in lines)
                            {
                                List<string> line = item.Split('|').ToList();
                                string matriculeClient = line[0].Substring(2, 5);
                                CsMapperTranscaisB transB = new CsMapperTranscaisB();
                                CLIENT LeClient=new CLIENT();
                                Rejet_De_Traitement = new REJET_DE_TRAITEMENT();
                                string NUMERO_FACTURE = line[5];
                                string PERIODE = line[1];
                                LCLIENT Facture = context.LCLIENT.FirstOrDefault(f => f.NDOC == NUMERO_FACTURE && f.REFEM == PERIODE);
                                //Gestion des element non conforme au régle de traitement(Stockage des éléments rejétées)
                                if (Facture != null )
                                {
                                    if (Facture.MONTANT.Value == decimal.Parse(line[4]))
                                    {
                                        LeClient = context.CLIENT.FirstOrDefault(l => l.MATRICULE == matriculeClient && l.PK_ID == Facture.FK_IDCLIENT);
                                        if (LeClient == null)
                                        {
                                            //Alimenté la table de rejet avec comm motif "client introuvable"
                                            Rejet_De_Traitement.NUMERO_DE_LIGNE = lines.IndexOf(item);
                                            Rejet_De_Traitement.MOTIF_DE_REJET = "client introuvable";
                                            Rejet_De_Traitement.DONNE = item;

                                            Info_De_Traitement_De_Fichier.REJET_DE_TRAITEMENT.Add(Rejet_De_Traitement);
                                            AgentInconuCommeCommeClient.Add(matriculeClient);
                                            continue;
                                        } 
                                    } 
                                    else
                                    {
                                        //Alimenté la table de rejet avec comm motif "montant de facture non conforme"
                                        Rejet_De_Traitement.NUMERO_DE_LIGNE = lines.IndexOf(item);
                                        Rejet_De_Traitement.MOTIF_DE_REJET = "montant de facture non conforme";
                                        Rejet_De_Traitement.DONNE = item;

                                        Info_De_Traitement_De_Fichier.REJET_DE_TRAITEMENT.Add(Rejet_De_Traitement);
                                        AgentInconuCommeCommeClient.Add(matriculeClient);
                                        continue;
                                    }
                                }
                                else
                                {
                                    //Alimenté la table de rejet avec comm motif "facture inexistant"
                                    Rejet_De_Traitement.NUMERO_DE_LIGNE = lines.IndexOf(item);
                                    Rejet_De_Traitement.MOTIF_DE_REJET = "facture inexistant";
                                    Rejet_De_Traitement.DONNE = item;

                                    Info_De_Traitement_De_Fichier.REJET_DE_TRAITEMENT.Add(Rejet_De_Traitement);
                                    AgentInconuCommeCommeClient.Add(matriculeClient);
                                    continue;
                                }
                            
                                    transB.ACQUIT = "";
                                    transB.CENTRE = LeClient != null ? LeClient.CENTRE : string.Empty;
                                    transB.CLIENT = LeClient != null ? LeClient.REFCLIENT : string.Empty;
                                    transB.ORDRE = LeClient != null ? LeClient.ORDRE : string.Empty;
                                    transB.NDOC = line[5];
                                    transB.REFEM = line[1];
                                    transB.MONTANT = decimal.Parse(line[4]);
                                    transB.DC = "C";
                                    transB.COPER = line[2];
                                    transB.PERCU = decimal.Parse(line[4]);
                                    transB.RENDU = 0;
                                    transB.MODEREG = modreg.CODE;
                                    transB.PLACE = "-";
                                    transB.DTRANS = DateTime.Now;
                                    transB.BANQUE = "------";
                                    transB.GUICHET = "------";
                                    transB.ORIGINE = LeClient != null ? LeClient.CENTRE : string.Empty;
                                    transB.ECART = 0;
                                    transB.MOISCOMPT = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
                                    transB.TOP1 = "8";
                                    transB.MATRICULE = Matricule;
                                    transB.DATEENCAISSEMENT = DateTime.Now;
                                    transB.DATECREATION = DateTime.Now;
                                    transB.USERCREATION = Matricule;
                                    transB.DATEVALEUR = DateTime.Now;

                                    // valuer les foreign key
                                    transB.FK_IDCENTRE = LeClient != null ? LeClient.FK_IDCENTRE : 0;
                                    transB.FK_IDCOPER = COPER.PK_ID;
                                    transB.FK_IDLIBELLETOP = context.LIBELLETOP.FirstOrDefault(l => l.CODE == "8").PK_ID;
                                    transB.FK_IDMODEREG = modreg.PK_ID;
                                    transB.FK_IDAGENTSAISIE = ADMUTILISATEUR.PK_ID;
                                    transB.FK_IDLCLIENT = Facture != null ? Facture.PK_ID : 0;
                                    TRANSCAISS.Add(transB);

                            }

                            Info_De_Traitement_De_Fichier.NOMBRE_DE_LIGNE_REJETE = Info_De_Traitement_De_Fichier.REJET_DE_TRAITEMENT.Count();
                            Info_De_Traitement_De_Fichier.NOMBRE_DE_LIGNE_TRAITE =lines.Count() - Info_De_Traitement_De_Fichier.REJET_DE_TRAITEMENT.Count();
                            context.TRAITEMENT_FICHIER.Add(Info_De_Traitement_De_Fichier);
                            context.SaveChanges();

                            DataTable TablePere = Galatee.Tools.Utility.ListToDataTable(TRANSCAISS);
                            Galatee.Tools.Utility.BulkInsertTable("TRANSCAISB", TablePere, cmd.Connection, cmd.Transaction);
                            cmd.Transaction.Commit();

                            return AgentInconuCommeCommeClient;
	                    }
                      return new List<string>();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        #endregion
        #endregion

        #region ADO .Net from Entity : Stephen 26-01-2019

            public List<CsMotifChequeImpaye> RetourneMotifChequeImpaye()
            {
                try
                {
                    //DataTable dt = CommonProcedures.ListeDesMotifRejetCheque();
                    DataTable dt = DB_ParametresGeneraux.SelectAllDonneReference("MOTIFCHEQUEIMPAYE");
                    return Entities.GetEntityListFromQuery<CsMotifChequeImpaye>(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        #endregion


    }
}
