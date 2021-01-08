using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBClientAccess
    {
            static string Langue = Thread.CurrentThread.CurrentUICulture.Name;
            private string ConnectionString;
            private SqlCommand cmd = null;
            private SqlConnection cn = null;
            SqlTransaction transaction = null;
            bool isRollback = false;
            public DBClientAccess()
            {
            try
            {
                //ConnectionString = GalateeConfig.ConnectionStrings[Enumere.ConnexionGALADB].ConnectionString;
                ConnectionString = Session.GetSqlConnexionString();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           }
            public List<CsCentre> SelectCentreCampagne()
            {

                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        if (context.CAMPAGNE.Select(c => c.CENTRE1) != null)
                        {
                            var query = context.CAMPAGNE.Select(c => new { c.CENTRE1.PK_ID, c.CENTRE1.CODE, c.CENTRE1.LIBELLE });
                            var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);

                            return Entities.GetEntityListFromQuery<CsCentre>(dt).Distinct().ToList();
                        }
                        return new List<CsCentre>();
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }

                //CTab300 centre;
                //List<CTab300> centres = new List<CTab300>();

                ////Objet cn
                //SqlConnection cn = new SqlConnection(ConnectionString);
                ////Objet Command
                //SqlCommand cmd = new SqlCommand("SPX_CAMPAGNE_GetCentres", cn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandTimeout = 18000;

                //try
                //{

                //    //Ouverture
                //    cn.Open();

                //    //Object datareader
                //    SqlDataReader reader = cmd.ExecuteReader();

                //    if (!reader.HasRows)
                //    {
                //        reader.Close();

                //       // throw new Exception(Translate(1, Langue));
                //    }
                //    while (reader.Read())
                //    {
                //        centre = new CTab300();
                //        centre.Code = reader.GetValue(0).ToString().Trim();
                //        centre.Libelle =new TAB300Access().SelectLibelleCentre(centre.Code);
                //        centres.Add(centre);
                //    }
                //    //Fermeture reader
                //    reader.Close();
                //    return centres;
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message);
                //}
                //finally
                //{
                //    //Fermeture base
                //    cn.Close();
                //}
            }


            public List<CsDetailCampagne> RetourneClientAutoriser(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lstIdCampagne = new List<string>();
                        foreach (CsCAMPAGNE item in lesCampagnes)
                            lstIdCampagne.Add(item.IDCOUPURE);

                        var query = (
                            from camp in context.CAMPAGNE
                            from det in camp.DETAILCAMPAGNE
                            where
                            lstIdCampagne.Contains(det.IDCOUPURE) && (det.ISAUTORISER == true || det.ISANNULATIONFRAIS == true )
                            orderby det.IDCOUPURE, det.CENTRE, det.CLIENT, det.ORDRE
                            select new CsDetailCampagne 
                            {
                                LIBELLECENTRE  = det.CENTRE1.LIBELLE,
                                DEBUTORDTOURNEE  = camp.DEBUT_ORDTOUR,
                                FINORDTOURNEE  = camp.FIN_ORDTOUR,
                                DEBUTTOURNEE  = camp.PREMIERE_TOURNEE,
                                FINTOURNEE = camp.DERNIERE_TOURNEE,
                                ORDTOUR  = det.ORDTOUR ,
                                EXIGIBILITE  = camp.DATE_EXIGIBILITE.Value,
                                NOMAGENT  = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == camp.MATRICULEPIA) != null ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == camp.MATRICULEPIA).LIBELLE : string.Empty,
                                NOMABON  = det.CLIENT1.NOMABON,
                                CLIENT  = det.CLIENT,
                                CENTRE  = det.CENTRE,
                                ORDRE  = det.ORDRE,
                                TOURNEE  = det.TOURNEE,
                                RUE  = det.CLIENT1.AG.RUE,
                                PORTE  = det.CLIENT1.AG.PORTE,
                                QUARTIER  = det.CLIENT1.AG.QUARTIER1.LIBELLE,
                                SOLDEDUE  = det.SOLDEDUE.Value,
                                NOMBREFACTURE  = det.NOMBREFACTURE.Value ,
                                IDCOUPURE  = camp.IDCOUPURE,
                                COMPTEUR  = det.COMPTEUR,
                                FK_IDCLIENT = det.FK_IDCLIENT
                            }).Distinct();
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();
                        return query.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > RechercherSuiviCampagne(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {

                        List<string> lesIdCampagne = new List<string>();
                        foreach (var item in lesCampagnes)
                            lesIdCampagne.Add(item.IDCOUPURE);

                        var query = (
                            from camp in context.CAMPAGNE 
                            from det in camp.DETAILCAMPAGNE
                            where
                            (lesIdCampagne.Contains(det.IDCOUPURE))
                            orderby det.IDCOUPURE, det.CENTRE, det.CLIENT, det.ORDRE
                            select new CsDetailCampagne 
                            {

                                LIBELLECENTRE = det.CENTRE1.LIBELLE,
                                CENTRE = det.CENTRE,
                                CLIENT = det.CLIENT,
                                ORDRE = det.ORDRE,
                                NOMABON = det.CLIENT1.NOMABON,
                                ORDTOUR = det.ORDTOUR,
                                TOURNEE = det.TOURNEE,
                                RUE = det.CLIENT1.AG.RUE,
                                QUARTIER = det.CLIENT1.AG.QUARTIER1.LIBELLE,
                                COMPTEUR = det.COMPTEUR,
                                NOMAGENT = context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == camp.MATRICULEPIA) != null ? context.ADMUTILISATEUR.FirstOrDefault(t => t.MATRICULE == camp.MATRICULEPIA).LIBELLE : string.Empty,
                                DEBUTORDTOURNEE = camp.DEBUT_ORDTOUR,
                                FINORDTOURNEE = camp.FIN_ORDTOUR,
                                DEBUTTOURNEE = camp.PREMIERE_TOURNEE,
                                FINTOURNEE = camp.DERNIERE_TOURNEE,
                                FK_IDCLIENT = det.FK_IDCLIENT ,
                                IDCOUPURE = det.IDCOUPURE ,
                                ISANNULATIONFRAIS = det.ISANNULATIONFRAIS ,
                                ISAUTORISER = det.ISAUTORISER ,
                                MOTIFANNULATION = det.MOTIFANNULATION ,
                                MOTIFAUTORISATION = det.MOTIFAUTORISATION ,
                                NOMBREFACTURE = det.NOMBREFACTURE ,
                                SOLDEDUE = det.SOLDEDUE ,
                                DATERDV = det.DATERDV.Value 
                            }).Distinct();
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();
                         return query.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > IndexCampagneSaisie(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lesIdCampagne = new List<string>();
                        foreach (var item in lesCampagnes)
                            lesIdCampagne.Add(item.IDCOUPURE);

                        var query = (
                            from camp in context.INDEXCAMPAGNE 
                            join det in context.DETAILCAMPAGNE on camp.IDCOUPURE equals det.IDCOUPURE 
                            where
                            (lesIdCampagne.Contains(camp.IDCOUPURE))
                            orderby camp.IDCOUPURE, camp.CENTRE, camp.CLIENT, camp.ORDRE
                            select new CsDetailCampagne 
                            {
                                CLIENT  = camp.CLIENT,
                                CENTRE  = camp.CENTRE,
                                ORDRE  = camp.ORDRE,
                                INDEX  = camp.INDEXE ,
                                DATECOUPURE = camp.DATECOUPURE,
                                FK_IDCLIENT = camp.FK_IDCLIENT ,
                                COMPTEUR = det.COMPTEUR,
                                OBSERVATION = camp.OBSERVATION.LIBELLE 
                            });
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();
                        return query.ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDetailCampagne> MandatementSg(CsCampagneGc lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from camp in context.MANDATEMENTGC 
                            where
                               camp.FK_IDCAMPAGNA  == lesCampagnes.PK_ID
                            select new
                            {
                                camp. MONTANT ,
                                IDCOUPURE= camp. NUMEROMANDATEMENT 
                            });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne> PaiementMandatementSg(CsMandatementGc  lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from camp in context.PAIEMENTCAMPAGNEGC 
                            where
                               camp.FK_IDMANDATEMANT  == lesCampagnes.PK_ID
                            select new
                            {
                                camp.MONTANT,
                                IDCOUPURE = camp.NUMEROMANDATEMENT,
                                DATEREGLEMENT =  camp.DATECREATION 
                            });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne> PreavisSgc(CsCampagneGc lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from camp in context.DETAILCAMPAGNESGC
                            where
                               camp.IDCAMPAGNEGC == lesCampagnes.PK_ID 
                            orderby camp.FK_IDCLIENT
                            select new
                            {
                                camp.CLIENT,
                                camp.CENTRE,
                                camp.ORDRE,
                                camp.CLIENT1.NOMABON,
                                camp.FK_IDCLIENT,
                                MONTANTFRAIS = camp.MONTANT,
                            });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne> PaiementCampagneSgc(CsCampagneGc lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from camp in context.DETAILCAMPAGNESGC 
                            join y in context.TRANSCAISSE on camp.FK_IDLCLIENT  equals y.FK_IDLCLIENT
                            where
                               camp.IDCAMPAGNEGC   == lesCampagnes.PK_ID   &&
                                //y.DTRANS >= lesCampagnes.DATECREATION &&
                                string.IsNullOrEmpty(y.TOPANNUL) &&
                                y.NDOC != "TIMBRE"
                            orderby camp.FK_IDCLIENT
                            select new
                            {
                                camp.CLIENT,
                                camp.CENTRE,
                                camp.ORDRE,
                                camp.CLIENT1.NOMABON,
                                camp.FK_IDCLIENT,
                              MONTANTFRAIS=  camp.MONTANT,
                              MONTANTEREGLE =  y.MONTANT ,
                                camp.PK_ID
                            });

                        var query1 = (
                             from camp in context.DETAILCAMPAGNESGC
                             join y in context.TRANSCAISSE on camp.FK_IDLCLIENT equals y.FK_IDLCLIENT
                             where
                                camp.IDCAMPAGNEGC == lesCampagnes.PK_ID &&
                                 y.DTRANS >= lesCampagnes.DATECREATION &&
                                 string.IsNullOrEmpty(y.TOPANNUL) &&
                                 y.NDOC != "TIMBRE"
                             orderby camp.FK_IDCLIENT
                             select new
                             {
                                 camp.CLIENT,
                                 camp.CENTRE,
                                 camp.ORDRE,
                                 camp.CLIENT1.NOMABON,
                                 camp.FK_IDCLIENT,
                                 MONTANTFRAIS = camp.MONTANT,
                                 MONTANTEREGLE = y.MONTANT,
                                 camp.PK_ID
                             });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
                        return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne> FraisCampagnePayerSgc(List<CsCampagneGc> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lesIdCampagne = new List<string>();
                        foreach (var item in lesCampagnes)
                            lesIdCampagne.Add(item.NUMEROCAMPAGNE );

                        var query = (
                            from camp in context.LCLIENT
                            from trans in camp.TRANSCAISB
                            where
                              (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                                string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                            select new CsDetailCampagne
                            {
                                FK_IDCLIENT = camp.FK_IDCLIENT,
                                MONTANTFRAIS = camp.MONTANT,
                                DATEREGLEMENT = trans.DTRANS,
                                IDCOUPURE = camp.IDCOUPURE
                            });
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();

                        var query1 = (
                                       from camp in context.LCLIENT
                                       from trans in camp.TRANSCAISSE
                                       where
                                         (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                                          string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                                       select new CsDetailCampagne
                                       {
                                           FK_IDCLIENT = camp.FK_IDCLIENT,
                                           MONTANTFRAIS = camp.MONTANT,
                                           DATEREGLEMENT = trans.DTRANS,
                                           IDCOUPURE = camp.IDCOUPURE

                                       });
                        List<CsDetailCampagne> Liste2 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste2 = query.ToList();

                        return query.Union(query1).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > PaiementCampagne(CsCAMPAGNE lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from camp in context.DETAILCAMPAGNE
                            join y in context.TRANSCAISSE on camp.FK_IDLCLIENT equals y.FK_IDLCLIENT
                            where
                               camp.IDCOUPURE == lesCampagnes.IDCOUPURE &&
                                y.DTRANS >= lesCampagnes.DATECREATION &&
                                string.IsNullOrEmpty(y.TOPANNUL) &&
                                y.NDOC != "TIMBRE"
                            orderby camp.FK_IDCLIENT
                            select new
                            {
                                camp.CLIENT,
                                camp.CENTRE,
                                camp.ORDRE,
                                camp.CLIENT1.NOMABON ,
                                camp.FK_IDCLIENT,
                                camp.MONTANT,
                                camp.PK_ID
                            });

                        var query1 = (
                             from camp in context.DETAILCAMPAGNE
                             join y in context.TRANSCAISB on camp.FK_IDLCLIENT equals y.FK_IDLCLIENT
                             where
                                camp.IDCOUPURE == lesCampagnes.IDCOUPURE &&
                                camp.IDCOUPURE == lesCampagnes.IDCOUPURE &&
                                y.DTRANS >= lesCampagnes.DATECREATION &&
                                string.IsNullOrEmpty(y.TOPANNUL) &&
                                y.NDOC != "TIMBRE"
                             orderby camp.FK_IDCLIENT
                             select new
                             {
                                 camp.CLIENT,
                                 camp.CENTRE,
                                 camp.ORDRE,
                                 camp.CLIENT1.NOMABON,
                                 camp.FK_IDCLIENT,
                                 camp.MONTANT,
                                 camp.PK_ID
                             });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
                        return Entities.GetEntityListFromQuery<CsDetailCampagne >(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > FraisCampagnePayer(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lesIdCampagne = new List<string>();
                        foreach (var item in lesCampagnes)
                            lesIdCampagne.Add(item.IDCOUPURE);

                        var query = (
                            from camp in context.LCLIENT
                            from trans in camp.TRANSCAISB
                            where
                              (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                                string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                            select new CsDetailCampagne 
                            {
                                FK_IDCLIENT = camp.FK_IDCLIENT,
                                MONTANTFRAIS = camp.MONTANT,
                                DATEREGLEMENT = trans.DTRANS,
                                IDCOUPURE  = camp.IDCOUPURE ,
                                 
                            });
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();

                        var query1 = (
                                       from camp in context.LCLIENT
                                       from trans in camp.TRANSCAISSE
                                       where
                                         (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                                          string.IsNullOrEmpty(trans.TOPANNUL) && trans.NDOC != "TIMBRE"
                                       select new CsDetailCampagne 
                                       {
                                           FK_IDCLIENT = camp.FK_IDCLIENT,
                                           MONTANTFRAIS = camp.MONTANT,
                                           DATEREGLEMENT = trans.DTRANS,
                                           IDCOUPURE = camp.IDCOUPURE 
                                          
                                       });
                        List<CsDetailCampagne> Liste2 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste2 = query.ToList();

                        return query.Union(query1).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > FraisCampagneNonPayer(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lesIdCampagne = new List<string>();
                        foreach (var item in lesCampagnes)
                            lesIdCampagne.Add(item.IDCOUPURE);

                        var query = (
                            from camp in context.LCLIENT
                            where
                              (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP && 
                              !context.TRANSCAISB.Any(t=>t.FK_IDLCLIENT == camp.PK_ID && string.IsNullOrEmpty(t.TOPANNUL) && t.NDOC !="TIMBRE" )
                            select new CsDetailCampagne 
                            {
                                FK_IDCLIENT = camp.FK_IDCLIENT,
                                MONTANTFRAIS  = camp.MONTANT,
                            });
                        List<CsDetailCampagne> Liste1 = new List<CsDetailCampagne>();
                        if (query != null)
                            Liste1 = query.ToList();

                        var query1 = (
                                       from camp in context.LCLIENT
                                       from trans in camp.TRANSCAISSE
                                       where
                              (lesIdCampagne.Contains(camp.IDCOUPURE)) && camp.COPER == Enumere.CoperFRP &&
                              !context.TRANSCAISB.Any(t => t.FK_IDLCLIENT == camp.PK_ID && string.IsNullOrEmpty(t.TOPANNUL) && t.NDOC != "TIMBRE")
                                       select new CsDetailCampagne 
                                       {
                                           FK_IDCLIENT = camp.FK_IDCLIENT,
                                           MONTANTFRAIS = camp.MONTANT,
                                       });
                        List<CsDetailCampagne > Liste2 = new List<CsDetailCampagne >();
                        if (query != null)
                            Liste2 = query.ToList();

                        return query.Union(query1).ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagne > EditerCLientaPoser(string Campagne)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                           from cl in context.LCLIENT
                           from tr in cl.TRANSCAISSE
                           join camp in context.CAMPAGNE on cl.IDCOUPURE equals camp.IDCOUPURE
                           from det in camp.DETAILCAMPAGNE
                           where
                            (cl.RDV_COUPURE != null || cl.COPER1.CODE == "076")
                           && (cl.IDCOUPURE == Campagne)
                           orderby cl.CENTRE, cl.IDCOUPURE, cl.CLIENT, cl.ORDRE
                           group new { cl.FK_IDCLIENT,tr.MONTANT, FRAIS = cl.MONTANT, cl.COPER }
                                   by new
                                   {
                                       LIBELLECENTRE= cl.CENTRE1.LIBELLE,
                                       cl.FK_IDCLIENT,
                                       cl.CLIENT,
                                       cl.CENTRE,
                                       cl.CLIENT1.NOMABON,
                                       cl.ORDRE,
                                       cl.IDCOUPURE,
                                       cl.CLIENT1.AG.TOURNEE,
                                       cl.CLIENT1.AG.RUE,
                                       cl.CLIENT1.AG.PORTE,
                                       cl.CLIENT1.AG.QUARTIER1.LIBELLE,
                                       camp.MATRICULEPIA,
                                       camp.DATE_EXIGIBILITE,
                                       camp.DEBUT_ORDTOUR,
                                       camp.PREMIERE_TOURNEE,
                                       camp.DERNIERE_TOURNEE,
                                       camp.FIN_ORDTOUR,
                                       det.SOLDEDUE,
                                       det.NOMBREFACTURE ,
                                       cl.OBSERVATION_COUPURE
                                   } into result
                           let SoldeCourant = result.Sum(x => x.MONTANT)
                           let SoldFrais = result.Where(x => x.COPER == "076").Sum(x => x.FRAIS)
                           let NbreFactRegleImpayes = result.Count()
                           select new CsDetailCampagne 
                           {
                               LIBELLECENTRE  = result.Key.LIBELLECENTRE,
                               OBSERVATION = result.Key.OBSERVATION_COUPURE,
                               DEBUTORDTOURNEE  = result.Key.DEBUT_ORDTOUR,
                               FINORDTOURNEE  = result.Key.FIN_ORDTOUR,
                               DEBUTTOURNEE = result.Key.PREMIERE_TOURNEE,
                               FINTOURNEE = result.Key.DERNIERE_TOURNEE,
                               EXIGIBILITE = result.Key.DATE_EXIGIBILITE.Value,
                               NOMAGENT = result.Key.MATRICULEPIA,
                               MONTANTFRAIS = SoldFrais,
                               NOMABON = result.Key.NOMABON,
                               CLIENT = result.Key.CLIENT,
                               CENTRE = result.Key.CENTRE,
                               ORDRE = result.Key.ORDRE,
                               TOURNEE = result.Key.TOURNEE,
                               RUE = result.Key.RUE,
                               PORTE = result.Key.PORTE,
                               QUARTIER = result.Key.LIBELLE,
                               //SOLDEDUE = result.Key.SOLDEDUE,
                               NOMBREFACTURE = result.Key.NOMBREFACTURE.Value,
                               //SoldeCourant = SoldeCourant.Value,
                               NOMBREFACTUREREGLE = NbreFactRegleImpayes
                           });
                        return query.ToList();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public List<CsDetailCampagne> RetourneDetailCampagne(List<CsCAMPAGNE> lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        List<string> lstIdCampagne = new List<string>();
                        lstIdCampagne.AddRange(lesCampagnes.Select(t => t.IDCOUPURE)); 

                        var query = (
                           from cl in context.DETAILCAMPAGNE 
                           where
                                 lstIdCampagne.Contains(cl.IDCOUPURE)
                           select new
                           {
                               cl.IDCOUPURE,
                               cl.CENTRE,
                               cl.CLIENT,
                               cl.ORDRE,
                               cl.COPER,
                               cl.MONTANT,
                               cl.EXIGIBILITE,
                               cl.TOURNEE,
                               cl.ORDTOUR,
                               cl.CATEGORIECLIENT,
                               cl.SOLDEDUE,
                               cl.NOMBREFACTURE,
                               cl.SOLDECLIENT,
                               cl.COMPTEUR,
                               cl.ISAUTORISER,
                               cl.MOTIFAUTORISATION,
                               NOMABON = cl.CLIENT1.NOMABON,
                               cl.FRAIS,
                               cl.ISANNULATIONFRAIS,
                               cl.MOTIFANNULATION,
                               cl.DATERDV,
                               cl.USERCREATION,
                               cl.USERMODIFICATION,
                               cl.FK_IDLCLIENT,
                               cl.FK_IDCENTRE,
                               cl.FK_IDCLIENT,
                               cl.FK_IDTOURNEE,
                               cl.FK_IDCATEGORIECLIENT,
                               cl.FK_IDCAMPAGNE,
                               cl.RELANCE,
                               cl.FK_IDTYPECOUPURE
                           }).Distinct();
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public List<CsClient> LoadClientCampgne(string Idcoupure)
            {
                using (galadbEntities contex=new galadbEntities())
                {
                   List<CLIENT> client = contex.LCLIENT.Where(l=>l.IDCOUPURE==Idcoupure).Select(l=>l.CLIENT1).Distinct().ToList();
                    if (client!=null)
                    {
                        var  CsClient= from c in client
                                            from a in c.ABON
                                            select new CsClient
                                            {
                                              CENTRE= c.CENTRE,REFCLIENT= c.REFCLIENT,ORDRE= c.ORDRE,PK_ID = c.PK_ID, NOMABON= c.NOMABON
                                            };
                        if (CsClient!=null)
                        {
                            return CsClient.ToList();
                        }
                    }
                    return new List<CsClient>();

                }
            }
        
            public List<CsDetailCampagne> ListeDesClientEnRDC(List<CsCAMPAGNE> lesCampagne)
            {
                DataTable dt = RecouvProcedures.ListeDesClientEnRDC(lesCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);

            }

            public List<CsDetailCampagne> ListeDesClientIndexSaisi(List<CsCAMPAGNE> lesCampagne)
            {
                List<CsDetailCampagne> lstCampagne = new List<CsDetailCampagne>();
                foreach (CsCAMPAGNE item in lesCampagne)
                {
                    DataTable dt = RecouvProcedures.ListeDesClientIndexSaisi(item);
                    lstCampagne.AddRange(Entities.GetEntityListFromQuery<CsDetailCampagne>(dt));
                }
                return lstCampagne;
            }
            public List<CsDetailCampagne> ListeDesClientRelance(List<CsCAMPAGNE> lesCampagne)
            {
                DataTable dt = RecouvProcedures.ListeDesClientRelance(lesCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }
            public List<CsDetailCampagne> ListeDesClientFraisSaisi(List<CsCAMPAGNE> lesCampagne)
            {
                DataTable dt = RecouvProcedures.ListeDesClientFraisSaisi(lesCampagne);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }

            public List<CsDetailCampagne> ListeDesMauvaisPayer(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
            {
                DataTable dt = RecouvProcedures.ListeDesMauvaisPayer(lstIdCentre, Datedebut, Datefin);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }

            public List<CsDetailCampagne> ListeDesMoratoiresNonRespecte(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin)
            {
                DataTable dt = RecouvProcedures.ListeDesMoratoiresNonRespecte(lstIdCentre, Datedebut, Datefin);
                return Entities.GetEntityListFromQuery<CsDetailCampagne>(dt);
            }

            public List<CsDetailMoratoire> ListeDesMoratoiresEmis(List<int> lstIdCentre, DateTime? Datedebut, DateTime? Datefin, bool IsPrecontentieux)
            {
                DataTable dt = new DataTable();
                if (!IsPrecontentieux )
                 dt = RecouvProcedures.ListeDesMoratoiresEmis(lstIdCentre, Datedebut, Datefin);
                else
                  dt = RecouvProcedures.ListeDesMoratoiresEmisPrecontentieux(lstIdCentre, Datedebut, Datefin);
                List<CsDetailMoratoire> lstMore = Entities.GetEntityListFromQuery<CsDetailMoratoire>(dt).Where(t=>t.STATUS != 9 ).ToList();
                foreach (CsDetailMoratoire item in lstMore)
                {
                     DataTable dtd = RecouvProcedures.ListeDesReglementMoratoires(item.FK_IDLCLIENT );
                     List<CsDetailMoratoire> lstRegl = Entities.GetEntityListFromQuery<CsDetailMoratoire>(dtd);
                     if (lstRegl != null && lstRegl.Count != 0)
                         item.MONTANTPAYE = lstRegl.Sum(t => t.MONTANT); 
                }
                return lstMore;
            }
            public decimal ? RetourneSoldeClient(CsDetailCampagne leClient)
            {
                try
                {
                    return FonctionCaisse.RetourneSoldeClient(leClient.FK_IDCENTRE, leClient.CENTRE, leClient.CLIENT, leClient.ORDRE);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public bool  UpdateDetailCampagneSuiteRelance(List<CsDetailCampagne> lstDetCampagne)
            {
                try
                {
                    int res = -1;
                    using (galadbEntities ctx = new galadbEntities())
                    {
                        foreach (CsDetailCampagne item in lstDetCampagne)
                        {
                            List<DETAILCAMPAGNE> lesDet = ctx.DETAILCAMPAGNE.Where(t => t.FK_IDCLIENT == item.FK_IDCLIENT && item.IDCOUPURE == t.IDCOUPURE).ToList();
                            if (lesDet.Count != 0)
                            {
                               if ( lesDet.First().RELANCE != null )
                                lesDet.ForEach(t => t.RELANCE = t.RELANCE + 1);
                               else
                                 lesDet.ForEach(t => t.RELANCE = 1);
                            }
                        }
                       res=  ctx.SaveChanges();
                    }
                    return res == -1 ? false : true;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            public List<CsObservation> RemplirObservation()
            {
                try
                {
                    DataTable dt = Galatee.Entity.Model.RecouvProcedures.RemplirObservation();
                    return Entities.GetEntityListFromQuery<CsObservation>(dt);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public List<CsDetailCampagnePrecontentieux> PaiementCampagnePrecontantieux(CsCAMPAGNE lesCampagnes)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        DateTime ladateCampagne = Convert.ToDateTime(lesCampagnes.DATECREATION.ToShortDateString());
                        var query = (
                            from camp in context.DETAILCAMPAGNEPRENCONTENTIEUX
                            join y in context.TRANSCAISSE on new { camp.FK_IDCENTRE, camp.CENTRE, camp.CLIENT, camp.ORDRE } equals
                                                             new { y.FK_IDCENTRE, y.CENTRE, y.CLIENT, y.ORDRE }
                            where
                               camp.IDCAMPAGNE  == lesCampagnes.IDCOUPURE  &&
                                y.DTRANS >= ladateCampagne &&
                                string.IsNullOrEmpty(y.TOPANNUL) &&
                                y.NDOC != "TIMBRE"
                            orderby camp.FK_IDCLIENT
                            select new
                            {
                                camp.CENTRE,
                                camp.CLIENT,
                                camp.ORDRE,
                                camp.TOURNEE,
                                camp.ORDTOUR,
                                camp.CATEGORIE,
                                camp.SOLDEDUE,
                                camp.USERCREATION,
                                camp.DATECREATION,
                                camp.DATEMODIFICATION,
                                camp.USERMODIFICATION,
                                camp.FK_IDCENTRE,
                                camp.FK_IDCLIENT,
                                camp.FK_IDTOURNEE,
                                camp.FK_IDCATEGORIE,
                                camp.FK_IDCAMPAGNE,
                                camp.CLIENT1.NOMABON,
                                camp.ISINVITATIONEDITER,
                                camp.DATERDV,
                                camp.DATERESILIATION ,
                                ADRESSE = camp.CLIENT1.ADRMAND1,
                                camp.CLIENT1.AG.RUE,
                                camp.CLIENT1.AG.PORTE ,
                                CODESITE = camp.CENTRE1.SITE.CODE,
                                FK_IDSITE = camp.CENTRE1.FK_IDCODESITE,
                                LIBELLECENTRE = camp.CENTRE1.LIBELLE,
                                LIBELLESITE = camp.CENTRE1.SITE.LIBELLE,
                                SOLDECLIENT = y.MONTANT,
                                DENR = y.DTRANS,
                                camp.PK_ID
                            });

                        var query1 = (
                                        from camp in context.DETAILCAMPAGNEPRENCONTENTIEUX
                                        join y in context.TRANSCAISB on new { camp.FK_IDCENTRE, camp.CENTRE, camp.CLIENT, camp.ORDRE } equals
                                         new { y.FK_IDCENTRE, y.CENTRE, y.CLIENT, y.ORDRE }
                                        where
                                           camp.IDCAMPAGNE == lesCampagnes.IDCOUPURE &&
                                           y.DTRANS >= lesCampagnes.DATECREATION &&
                                           string.IsNullOrEmpty(y.TOPANNUL) &&
                                           y.NDOC != "TIMBRE"
                                        orderby camp.FK_IDCLIENT
                                        select new
                                        {
                                            camp.CENTRE,
                                            camp.CLIENT,
                                            camp.ORDRE,
                                            camp.TOURNEE,
                                            camp.ORDTOUR,
                                            camp.CATEGORIE,
                                            camp.SOLDEDUE,
                                            camp.USERCREATION,
                                            camp.DATECREATION,
                                            camp.DATEMODIFICATION,
                                            camp.USERMODIFICATION,
                                            camp.FK_IDCENTRE,
                                            camp.FK_IDCLIENT,
                                            camp.FK_IDTOURNEE,
                                            camp.FK_IDCATEGORIE,
                                            camp.FK_IDCAMPAGNE,
                                            camp.CLIENT1.NOMABON,
                                            camp.ISINVITATIONEDITER,
                                            camp.DATERDV,
                                            camp.DATERESILIATION,
                                            ADRESSE = camp.CLIENT1.ADRMAND1,
                                            camp.CLIENT1.AG.RUE,
                                            camp.CLIENT1.AG.PORTE,
                                            CODESITE = camp.CENTRE1.SITE.CODE,
                                            FK_IDSITE = camp.CENTRE1.FK_IDCODESITE,
                                            LIBELLECENTRE = camp.CENTRE1.LIBELLE,
                                            LIBELLESITE = camp.CENTRE1.SITE.LIBELLE,
                                            SOLDECLIENT = y.MONTANT,
                                            DENR = y.DTRANS,
                                            camp.PK_ID
                                        });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
                        return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<CsDetailCampagnePrecontentieux> RechercherAbonnemtPrepayePrecontentieux(CsDetailCampagnePrecontentieux leClient)
            {
                try
                {
                    using (galadbEntities context = new galadbEntities())
                    {
                        var query = (
                            from cli in context.CLIENT 
                            from  abon in cli.ABON 

                            where
                                 cli.AG.PORTE.Contains(leClient.RUE) && cli.AG.PORTE.Contains(leClient.PORTE) 
                                 //&& cli.CENTRE != leClient.CENTRE && cli.REFCLIENT != leClient.CLIENT && cli.ORDRE != leClient.ORDRE 
                            select new
                            {
                                cli.CENTRE,
                                abon.CLIENT,
                                cli.ORDRE,
                                cli.AG .TOURNEE,
                                cli.AG.ORDTOUR,
                                cli.CATEGORIE,
                                cli.NOMABON,
                                ADRESSE = cli.ADRMAND1,
                                cli.AG.RUE,
                                cli.AG.PORTE,
                                CODESITE = cli.CENTRE1.SITE.CODE,
                                FK_IDSITE = cli.CENTRE1.FK_IDCODESITE,
                                LIBELLECENTRE = cli.CENTRE1.LIBELLE,
                                LIBELLESITE = cli.CENTRE1.SITE.LIBELLE,
                            });
                        var dt = Galatee.Tools.Utility.ListToDataTable<object>(query);
                        return Entities.GetEntityListFromQuery<CsDetailCampagnePrecontentieux>(dt).Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            public List<CsRegCli> RetourneRegroupementGestionnaires(int IdGestionnaire)
            {
                cn = new SqlConnection(ConnectionString);
                cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandTimeout = 18000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPX_RECOUV_REGROUPEMENTGESTIONNAIRE";
                cmd.Parameters.Add("@IDGESTIONNAIRE", SqlDbType.Int ).Value = IdGestionnaire;

                DBBase.SetDBNullParametre(cmd.Parameters);
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    return Entities.GetEntityListFromQuery<CsRegCli>(dt);
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

    }
    
}
