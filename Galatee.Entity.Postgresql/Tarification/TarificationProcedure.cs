using Galatee.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Entity.Model
{
    public static partial class TarificationProcedure
    {

        #region  Redevance

        //Chargement des Types de redevence
        public static List<CsTypeRedevence> LoadAllTypeRedevance()
        {
            List<CsTypeRedevence> ListeTypeRedevence = new List<CsTypeRedevence>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.TYPEREDEVANCE.ToList())
                    {
                        CsTypeRedevence TypeRedevence = new CsTypeRedevence();
                        TypeRedevence.PK_ID = item.PK_ID;
                        TypeRedevence.CODE = item.CODE;
                        TypeRedevence.DATECREATION = item.DATECREATION;
                        TypeRedevence.DATEMODIFICATION = item.DATEMODIFICATION;
                        TypeRedevence.LIBELLE = item.LIBELLE;
                        TypeRedevence.USERCREATION = item.USERCREATION;
                        TypeRedevence.USERMODIFICATION = item.USERMODIFICATION;

                        ListeTypeRedevence.Add(TypeRedevence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeTypeRedevence;
        }

        //Chargement des Types de lien redevence
        public static List<CsTypeLienRedevence> ListeTypeLienRedevance()
        {
            List<CsTypeLienRedevence> ListeTypeLienRedevence = new List<CsTypeLienRedevence>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.TYPELIENREDEVANCE.ToList())
                    {
                        CsTypeLienRedevence TypeLienRedevence = new CsTypeLienRedevence();
                        TypeLienRedevence.PK_ID = item.PK_ID;
                        TypeLienRedevence.CODE = item.CODE;
                        TypeLienRedevence.DATECREATION = item.DATECREATION;
                        TypeLienRedevence.DATEMODIFICATION = item.DATEMODIFICATION;
                        TypeLienRedevence.LIBELLE = item.LIBELLE;
                        TypeLienRedevence.USERCREATION = item.USERCREATION;
                        TypeLienRedevence.USERMODIFICATION = item.USERMODIFICATION;

                        ListeTypeLienRedevence.Add(TypeLienRedevence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeTypeLienRedevence;
        }
        public static List<CsTypeLienProduit> ListeTypeLienProduit()
        {
            List<CsTypeLienProduit> ListeTypeLienProduit = new List<CsTypeLienProduit>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.TYPELIENPRODUIT.ToList())
                    {
                        CsTypeLienProduit TypeLienRedevence = new CsTypeLienProduit();
                        TypeLienRedevence.PK_ID = item.PK_ID;
                        TypeLienRedevence.FK_IDPRODUIT = item.FK_IDPRODUIT;
                        TypeLienRedevence.FK_IDTYPELIEN = item.FK_IDTYPELIEN;
                       

                        ListeTypeLienProduit.Add(TypeLienRedevence);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeTypeLienProduit;
        }

        //Charger liste des lots reçu
        public static List<CsRedevance> LoadAllRedevance()
        {
            List<CsRedevance> ListeRedevance = new List<CsRedevance>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {

                    foreach (var item in Context.REDEVANCE.ToList())
                    {
                        CsRedevance Redevance = new CsRedevance();
                        Redevance.PK_ID = item.PK_ID;
                        Redevance.CODE = item.CODE;
                        Redevance.DATECREATION = item.DATECREATION;
                        Redevance.DATEMODIFICATION = item.DATEMODIFICATION;
                        Redevance.FK_IDPRODUIT = item.FK_IDPRODUIT;
                        Redevance.LIBELLEPRODUIT = item.PRODUIT1.LIBELLE ;
                        Redevance.FK_IDTYPELIENREDEVANCE = item.FK_IDTYPELIENREDEVANCE;
                        Redevance.FK_IDTYPEREDEVANCE = item.FK_IDTYPEREDEVANCE;
                        Redevance.TYPELIEN = item.TYPELIENREDEVANCE.LIBELLE ;
                        Redevance.LIBELLE = item.LIBELLE;
                        Redevance.PRODUIT = item.PRODUIT;
                        Redevance.USERCREATION = item.USERCREATION;
                        Redevance.USERMODIFICATION = item.USERMODIFICATION;
                        Redevance.NATURECLI = item.TYPEREDEVANCE.LIBELLE ;
                        Redevance.PARAM1 = item.TYPEREDEVANCE.CODE;
                        Redevance.PARAM2 = item.TYPELIENREDEVANCE.CODE ;
                        Redevance.TRANCHEREDEVANCE = new List<CsTrancheRedevence>();
                        //foreach (var item_ in item.TRANCHEREDEVANCE)
                        //{
                        //    CsTrancheRedevence TrancheRedevence = new CsTrancheRedevence();
                        //    TrancheRedevence.FK_IDREDEVANCE = item_.FK_IDREDEVANCE;
                        //    TrancheRedevence.LIBELLE = item_.LIBELLE;
                        //    TrancheRedevence.GRATUIT = item_.GRATUIT.Value;
                        //    TrancheRedevence.ORDRE = item_.ORDRE;
                        //    TrancheRedevence.PK_ID = item_.PK_ID;
                        //    Redevance.TRANCHEREDEVANCE.Add(TrancheRedevence);
                        //}
                        ListeRedevance.Add(Redevance);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeRedevance;
        }

        public static int SaveRedevance(List<CsRedevance> ListeRedevanceScelleToUpdate, List<CsRedevance> ListeRedevanceScelleToInserte, List<CsRedevance> ListeRedevanceScelleToDelete)
        {
            //bool IsSaved = false;
            int PK_ID = 0;
            List<REDEVANCE> ListeRedevanceScelleToUpdate_ = new List<REDEVANCE>();
            List<REDEVANCE> ListeRedevanceScelleToInserte_ = new List<REDEVANCE>();
            List<REDEVANCE> ListeRedevanceScelleToDelete_ = new List<REDEVANCE>();

            try
            {

                using (galadbEntities Context = new galadbEntities())
                {
                    convertCsRedevance(ListeRedevanceScelleToUpdate, ListeRedevanceScelleToUpdate_);
                    convertCsRedevance(ListeRedevanceScelleToInserte, ListeRedevanceScelleToInserte_);
                    convertCsRedevance(ListeRedevanceScelleToDelete, ListeRedevanceScelleToDelete_);


                    Entities.UpdateEntity<REDEVANCE>(ListeRedevanceScelleToUpdate_, Context);
                    foreach (var item in ListeRedevanceScelleToUpdate_)
                    {
                        foreach (var item_ in item.TRANCHEREDEVANCE)
                        {
                            if (item_.PK_ID == 0)
                            {
                                Context.TRANCHEREDEVANCE.Add(item_);
                            }
                            else
                            {
                                Entities.UpdateEntity<TRANCHEREDEVANCE>(item_, Context);
                            }
                        }
                    }

                    Entities.InsertEntity<REDEVANCE>(ListeRedevanceScelleToInserte_, Context);
                    //foreach (var item in ListeRedevanceScelleToInserte_)
                    //{
                    //    Entities.DeleteEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
                    //    Entities.InsertEntity<TRANCHEREDEVENCE>(item.TRANCHEREDEVENCE.ToList(), Context);
                    //}

                    Entities.DeleteEntity<REDEVANCE>(ListeRedevanceScelleToDelete_, Context);
                    foreach (var item in ListeRedevanceScelleToDelete_)
                    {
                        Entities.DeleteEntity<TRANCHEREDEVANCE>(item.TRANCHEREDEVANCE.ToList(), Context);
                    }
                    Context.SaveChanges();

                    if (ListeRedevanceScelleToUpdate_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToUpdate_.First().PK_ID;
                    }
                    if (ListeRedevanceScelleToInserte_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToInserte_.First().PK_ID;
                    }
                    if (ListeRedevanceScelleToDelete_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToDelete_.First().PK_ID;
                    }
                    //IsSaved = true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PK_ID;
        }


        //Convertir une liste de CsRedevance en une liste de REDEVANCE
        private static void convertCsRedevance(List<CsRedevance> ListeRedevanceScelle, List<REDEVANCE> ListeRedevanceScelle_)
        {
            foreach (var item in ListeRedevanceScelle)
            {

                REDEVANCE Redevance = new REDEVANCE();

                Redevance.USERCREATION = "99999";
                Redevance.PRODUIT = !string.IsNullOrWhiteSpace(item.PRODUIT) ? item.PRODUIT : string.Empty;
                Redevance.CODE = !string.IsNullOrWhiteSpace(item.CODE) ? item.CODE : string.Empty;
                Redevance.LIBELLE = !string.IsNullOrWhiteSpace(item.LIBELLE) ? item.LIBELLE : string.Empty;
                Redevance.FK_IDPRODUIT = item.FK_IDPRODUIT;
                Redevance.PK_ID = item.PK_ID;
                Redevance.DATECREATION = item.DATECREATION != null ? item.DATECREATION.Value : DateTime.Now;
                Redevance.DATEMODIFICATION = item.DATEMODIFICATION != null ? item.DATEMODIFICATION.Value : DateTime.Now;
                Redevance.FK_IDTYPELIENREDEVANCE = item.FK_IDTYPELIENREDEVANCE;
                Redevance.FK_IDTYPEREDEVANCE = item.FK_IDTYPEREDEVANCE;


                foreach (var tranche in item.TRANCHEREDEVANCE)
                {
                    TRANCHEREDEVANCE TRANCHEREDEVENCE = new TRANCHEREDEVANCE();

                    TRANCHEREDEVENCE.FK_IDREDEVANCE = item.PK_ID;
                    TRANCHEREDEVENCE.GRATUIT = tranche.GRATUIT;
                    TRANCHEREDEVENCE.LIBELLE = tranche.LIBELLE;
                    TRANCHEREDEVENCE.ORDRE = tranche.ORDRE;
                    TRANCHEREDEVENCE.PK_ID = tranche.PK_ID;
                    Redevance.TRANCHEREDEVANCE.Add(TRANCHEREDEVENCE);
                }

                ListeRedevanceScelle_.Add(Redevance);
            }
        }


        //Verifier que le code redevence passé n'exite pas en base pour eviter les redevenece avce le même code
        public static bool CheickCodeRedevanceExist(string Code)
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                   return Context.REDEVANCE.FirstOrDefault(r => r.CODE == Code) == null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Recherche Tarif
        //Charger liste des  Recherche Tarif


        public static DataTable GetAllRechercheTarif()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                 from rechTarif in context.RECHERCHETARIF 
                 select new
                 {
                    rechTarif.PK_ID,
                    rechTarif.CODE,
                    rechTarif.DATECREATION,
                    rechTarif.DATEMODIFICATION,
                    rechTarif.LIBELLE,
                    rechTarif.USERCREATION,
                    rechTarif.USERMODIFICATION,
                 };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetAllCtarcomp()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                     IEnumerable<object> query =
                     from ctarCompt in context.CTARCOMP 
                     select new
                     {
                         ctarCompt.DATECREATION,
                         ctarCompt.DATEMODIFICATION,
                         ctarCompt.FK_IDCONTENANTCRITERETARIF,
                         ctarCompt.ORDRE,
                         ctarCompt.FK_IDRECHERCHETARIF,
                         ctarCompt.USERCREATION,
                         ctarCompt.USERMODIFICATION,
                         LIBELLECONTENANTCRITERETARIF= ctarCompt.CONTENANTCRITERETARIF.LIBELLE 
                     };
                     return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetAllVariableDeTarif()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from x in context.VARIABLETARIF 
                    select new
                    {
                        x.REDEVANCE,
                        x.REGION,
                        x.SREGION,
                        x.CENTRE,
                        x.COMMUNE,
                        x.ORDREEDITION,
                        x.DATEAPPLICATION,
                        x.RECHERCHETARIF,
                        x.MODECALCUL,
                        x.MODEAPPLICATION,
                        x.LIBELLECOMPTABLE,
                        x.COMPTECOMPTABLE,
                        x.ESTANALYTIQUE,
                        x.GENERATIONANOMALIE,
                        x.FORMULE,
                        x.PK_ID,
                        x.DATECREATION,
                        x.DATEMODIFICATION,
                        x.USERCREATION,
                        x.USERMODIFICATION,
                        x.FK_IDREDEVANCE,
                        x.FK_IDCENTRE,
                        x.FK_IDMODEAPPLICATION,
                        x.FK_IDMODECALCUL,
                        x.FK_IDRECHERCHETARIF
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<CsRechercheTarif> LoadAllRechercheTarif()
        {
            List<CsRechercheTarif> ListeRedevance = new List<CsRechercheTarif>();
            try
            {
                DataTable dt = GetAllRechercheTarif();
                List<CsRechercheTarif> lstTarif = Galatee.Tools.Utility.GetEntityListFromQuery<CsRechercheTarif>(dt);

                DataTable dts = GetAllCtarcomp();
                List<CsCtarcomp> lstCtarcompt = Galatee.Tools.Utility.GetEntityListFromQuery<CsCtarcomp>(dts);

                foreach (CsRechercheTarif item in lstTarif)
                    item.CTARCOMP = lstCtarcompt.Where(t => t.FK_IDRECHERCHETARIF == item.PK_ID).ToList();

                return lstTarif;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeRedevance;
        }

        //public static List<CsRechercheTarif> LoadAllRechercheTarif()
        //{
        //    List<CsRechercheTarif> ListeRedevance = new List<CsRechercheTarif>();
        //    try
        //    {
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            foreach (var item in Context.RECHERCHETARIF.ToList())
        //            {
        //                CsRechercheTarif RechercheTarif = new CsRechercheTarif();
        //                RechercheTarif.PK_ID = item.PK_ID;
        //                RechercheTarif.CODE = item.CODE;
        //                RechercheTarif.DATECREATION = item.DATECREATION;
        //                RechercheTarif.DATEMODIFICATION = item.DATEMODIFICATION;
        //                RechercheTarif.LIBELLE = item.LIBELLE;
        //                RechercheTarif.USERCREATION = item.USERCREATION;
        //                RechercheTarif.USERMODIFICATION = item.USERMODIFICATION;

        //                RechercheTarif.CTARCOMP = new List<CsCtarcomp>();

        //                foreach (var item_ in item.CTARCOMP)
        //                {
        //                    CsCtarcomp Ctarcomp = new CsCtarcomp();
        //                    Ctarcomp.DATECREATION = item_.DATECREATION;
        //                    Ctarcomp.DATEMODIFICATION = item_.DATEMODIFICATION;
        //                    Ctarcomp.FK_IDCONTENANTCRITERETARIF = item_.FK_IDCONTENANTCRITERETARIF;
        //                    var CONTENANTCRITERETARIF=Context.CONTENANTCRITERETARIF.FirstOrDefault(c=>c.PK_ID==item_.FK_IDCONTENANTCRITERETARIF);
        //                    Ctarcomp.LIBELLECONTENANTCRITERETARIF=CONTENANTCRITERETARIF!=null?CONTENANTCRITERETARIF.LIBELLE:string.Empty;
        //                    Ctarcomp.ORDRE = item_.ORDRE;
        //                    Ctarcomp.FK_IDRECHERCHETARIF = item_.FK_IDRECHERCHETARIF;
        //                    Ctarcomp.ORDRE = item_.ORDRE;
        //                    Ctarcomp.USERCREATION = item_.USERCREATION;
        //                    Ctarcomp.USERMODIFICATION = item_.USERMODIFICATION;

        //                    RechercheTarif.CTARCOMP.Add(Ctarcomp);
        //                }
        //                ListeRedevance.Add(RechercheTarif);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ListeRedevance;
        //}

        //Chargement des Contenant Critere Tarif

        public static DataTable GetAllContenantTarif()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from contenatTarf in context.CONTENANTCRITERETARIF
                    select new
                    {
                        contenatTarf.PK_ID,
                        contenatTarf.AVECPRODUIT,
                        contenatTarf.COLONNEDONNEES,
                        contenatTarf.TABLEDONNEES,
                        contenatTarf.LIBELLE,
                        contenatTarf.TABLEREFERENCE,
                        contenatTarf.TAILLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<CsContenantCritereTarif> LoadAllContenantCritereTarif()
        {
            DataTable dt = GetAllContenantTarif();
            List<CsContenantCritereTarif> ListeTypeRedevence = Galatee.Tools.Utility.GetEntityListFromQuery<CsContenantCritereTarif>(dt);
            return ListeTypeRedevence;
        }

        //Transaction de mise à jour 
        //public static List<CsContenantCritereTarif> LoadAllContenantCritereTarif()
        //{
        //    List<CsContenantCritereTarif> ListeTypeRedevence = new List<CsContenantCritereTarif>();
        //    try
        //    {
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            foreach (var item in Context.CONTENANTCRITERETARIF.ToList())
        //            {
        //                CsContenantCritereTarif ContenantCritereTarif = new CsContenantCritereTarif();
        //                ContenantCritereTarif.PK_ID = item.PK_ID;
        //                ContenantCritereTarif.AVECPRODUIT = item.AVECPRODUIT;
        //                ContenantCritereTarif.COLONNEDONNEES = item.COLONNEDONNEES;
        //                ContenantCritereTarif.TABLEDONNEES = item.TABLEDONNEES;
        //                ContenantCritereTarif.LIBELLE = item.LIBELLE;
        //                ContenantCritereTarif.TABLEREFERENCE = item.TABLEREFERENCE;
        //                ContenantCritereTarif.TAILLE = item.TAILLE;

        //                ListeTypeRedevence.Add(ContenantCritereTarif);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ListeTypeRedevence;
        //}

        //Transaction de mise à jour 
        public static int SaveRechercheTarif(List<CsRechercheTarif> ListeRedevanceScelleToUpdate, List<CsRechercheTarif> ListeRedevanceScelleToInserte, List<CsRechercheTarif> ListeRedevanceScelleToDelete)
        {
            //bool IsSaved = false;
            int PK_ID = 0;
            List<RECHERCHETARIF> ListeRedevanceScelleToUpdate_ = new List<RECHERCHETARIF>();
            List<RECHERCHETARIF> ListeRedevanceScelleToInserte_ = new List<RECHERCHETARIF>();
            List<RECHERCHETARIF> ListeRedevanceScelleToDelete_ = new List<RECHERCHETARIF>();

            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    convertCsRechecheTarif(ListeRedevanceScelleToUpdate, ListeRedevanceScelleToUpdate_);
                    convertCsRechecheTarif(ListeRedevanceScelleToInserte, ListeRedevanceScelleToInserte_);
                    convertCsRechecheTarif(ListeRedevanceScelleToDelete, ListeRedevanceScelleToDelete_);


                    Entities.UpdateEntity<RECHERCHETARIF>(ListeRedevanceScelleToUpdate_, Context);

                    Entities.InsertEntity<RECHERCHETARIF>(ListeRedevanceScelleToInserte_, Context);
                   
                    Entities.DeleteEntity<RECHERCHETARIF>(ListeRedevanceScelleToDelete_, Context);

                    Context.SaveChanges();
                    if (ListeRedevanceScelleToUpdate_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToUpdate_.First().PK_ID;
                    }
                    if (ListeRedevanceScelleToInserte_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToInserte_.First().PK_ID;
                    }
                    if (ListeRedevanceScelleToDelete_.Count > 0)
                    {
                        PK_ID = ListeRedevanceScelleToDelete_.First().PK_ID;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PK_ID;
        }


        //Convertir une liste de CsRedevance en une liste de REDEVANCE
        private static void convertCsRechecheTarif(List<CsRechercheTarif> ListeRedevanceScelle, List<RECHERCHETARIF> ListeRedevanceScelle_)
        {
            foreach (var item in ListeRedevanceScelle)
            {

                RECHERCHETARIF Redevance = new RECHERCHETARIF();

                Redevance.USERCREATION = "99999";
                Redevance.USERMODIFICATION = "9999";
                Redevance.CODE = !string.IsNullOrWhiteSpace(item.CODE) ? item.CODE : string.Empty;
                Redevance.LIBELLE = !string.IsNullOrWhiteSpace(item.LIBELLE) ? item.LIBELLE : string.Empty;
                Redevance.PK_ID = item.PK_ID;
                Redevance.DATECREATION = item.DATECREATION != null ? item.DATECREATION.Value : DateTime.Now;
                Redevance.DATEMODIFICATION = item.DATEMODIFICATION != null ? item.DATEMODIFICATION.Value : DateTime.Now;



                foreach (var ctarcomp in item.CTARCOMP)
                {
                    CTARCOMP CTARCOMP = new CTARCOMP();

                    CTARCOMP.DATECREATION =DateTime.Now;
                    CTARCOMP.DATEMODIFICATION = DateTime.Now;
                    CTARCOMP.FK_IDCONTENANTCRITERETARIF = ctarcomp.FK_IDCONTENANTCRITERETARIF;
                    CTARCOMP.FK_IDRECHERCHETARIF = ctarcomp.FK_IDRECHERCHETARIF;
                    CTARCOMP.ORDRE = ctarcomp.ORDRE;
                    CTARCOMP.USERCREATION = "99999";
                    CTARCOMP.USERMODIFICATION = "99999";

                    Redevance.CTARCOMP.Add(CTARCOMP);
                }

                ListeRedevanceScelle_.Add(Redevance);
            }
        }

        #endregion

        #region Variable de Tarification

        //Charger liste des Variables de Tarification
        //public static List<CsVariableDeTarification> LoadAllVariableTarif()
        //{
        //    List<CsVariableDeTarification> ListeVariableDeTarification = new List<CsVariableDeTarification>();
        //    try
        //    {
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            foreach (var item in Context.VARIABLETARIF.ToList())
        //            {
        //                CsVariableDeTarification VariableDeTarification = new CsVariableDeTarification();
        //                VariableDeTarification.REDEVANCE_RECHERCHE="CODE REDEVENCE("+item.REDEVANCE1.CODE+")-CODE RECHERCHE("+item.RECHERCHETARIF1.CODE;
        //                VariableDeTarification.PK_ID = item.PK_ID;
        //                VariableDeTarification.CENTRE = item.CENTRE;
        //                VariableDeTarification.DATECREATION = item.DATECREATION;
        //                VariableDeTarification.DATEMODIFICATION = item.DATEMODIFICATION;
        //                VariableDeTarification.COMMUNE = item.COMMUNE;
        //                VariableDeTarification.USERCREATION = item.USERCREATION;
        //                VariableDeTarification.USERMODIFICATION = item.USERMODIFICATION;
        //                VariableDeTarification.COMPTECOMPTABLE = item.COMPTECOMPTABLE;
        //                VariableDeTarification.DATEAPPLICATION = item.DATEAPPLICATION;
        //                VariableDeTarification.ESTANALYTIQUE = item.ESTANALYTIQUE;
        //                VariableDeTarification.FK_IDCENTRE = item.FK_IDCENTRE;
        //                VariableDeTarification.FK_IDMODEAPPLICATION = item.FK_IDMODEAPPLICATION;
        //                VariableDeTarification.FK_IDMODECALCUL = item.FK_IDMODECALCUL;
        //                VariableDeTarification.FK_IDRECHERCHETARIF = item.FK_IDRECHERCHETARIF;
        //                VariableDeTarification.FK_IDREDEVANCE = item.FK_IDREDEVANCE;
        //                VariableDeTarification.FORMULE = item.FORMULE;
        //                VariableDeTarification.GENERATIONANOMALIE = item.GENERATIONANOMALIE;
        //                VariableDeTarification.LIBELLECOMPTABLE = item.LIBELLECOMPTABLE;
        //                VariableDeTarification.MODEAPPLICATION = item.MODEAPPLICATION;
        //                VariableDeTarification.MODECALCUL = item.MODECALCUL;
        //                VariableDeTarification.ORDREEDITION = item.ORDREEDITION;
        //                VariableDeTarification.RECHERCHETARIF = item.RECHERCHETARIF;
        //                VariableDeTarification.REDEVANCE = item.REDEVANCE;
        //                VariableDeTarification.REGION = item.REGION;
        //                VariableDeTarification.SREGION = item.SREGION;


        //                ListeVariableDeTarification.Add(VariableDeTarification);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return ListeVariableDeTarification;
        //}

        //Charger liste des Variables de Tarification
        public static List<CsVariableDeTarification> LoadAllVariableTarif()
        {
            List<CsVariableDeTarification> ListeVariableDeTarification = new List<CsVariableDeTarification>();
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from VariableDeTarification in context.VARIABLETARIF 
                    select new
                    {
                        REDEVANCE_RECHERCHE="CODE REDEVENCE("+VariableDeTarification.REDEVANCE1.CODE+")-CODE RECHERCHE("+VariableDeTarification.RECHERCHETARIF1.CODE,
                        VariableDeTarification.PK_ID,
                        VariableDeTarification.CENTRE,
                        VariableDeTarification.DATECREATION,
                        VariableDeTarification.DATEMODIFICATION,
                        VariableDeTarification.COMMUNE,
                        VariableDeTarification.USERCREATION,
                        VariableDeTarification.USERMODIFICATION,
                        VariableDeTarification.COMPTECOMPTABLE,
                        VariableDeTarification.DATEAPPLICATION,
                        VariableDeTarification.ESTANALYTIQUE,
                        VariableDeTarification.FK_IDCENTRE,
                        VariableDeTarification.FK_IDMODEAPPLICATION,
                        VariableDeTarification.FK_IDMODECALCUL,
                        VariableDeTarification.FK_IDRECHERCHETARIF,
                        VariableDeTarification.FK_IDREDEVANCE,
                        VariableDeTarification.REDEVANCE1.FK_IDPRODUIT,
                        PRODUIT= VariableDeTarification.REDEVANCE1.PRODUIT1.CODE ,
                        VariableDeTarification.FORMULE,
                        VariableDeTarification.GENERATIONANOMALIE,
                        VariableDeTarification.LIBELLECOMPTABLE,
                        VariableDeTarification.MODEAPPLICATION,
                        VariableDeTarification.MODECALCUL,
                        VariableDeTarification.ORDREEDITION,
                        VariableDeTarification.RECHERCHETARIF,
                        VariableDeTarification.REDEVANCE,
                        VariableDeTarification.REGION,
                        VariableDeTarification.SREGION
                    };
                    DataTable dt = Galatee.Tools.Utility.ListToDataTable(query);
                  return   ListeVariableDeTarification = Galatee.Tools.Utility.GetEntityListFromQuery<CsVariableDeTarification>(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Chargement de l liste des Modes de calcule
        public static List<CsModeCalcul> LoadAllModeCalcule()
        {
            List<CsModeCalcul> ListeModeCalcul = new List<CsModeCalcul>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.MODECALCUL.ToList())
                    {
                        CsModeCalcul ModeCalcul = new CsModeCalcul();
                        ModeCalcul.PK_ID = item.PK_ID;
                        ModeCalcul.DATECREATION = item.DATECREATION;
                        ModeCalcul.DATEMODIFICATION = item.DATEMODIFICATION;
                        ModeCalcul.USERCREATION = item.USERCREATION;
                        ModeCalcul.USERMODIFICATION = item.USERMODIFICATION;
                        ModeCalcul.CODE = item.CODE;
                        ModeCalcul.LIBELLE = item.LIBELLE;
                       


                        ListeModeCalcul.Add(ModeCalcul);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeModeCalcul;
        }

        //Chargement de l liste des Modes d'application
        public static List<CsModeApplicationTarif> LoadAllModeApplicationTarif()
        {
            List<CsModeApplicationTarif> ListeModeApplicationTarif = new List<CsModeApplicationTarif>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.MODEAPPLICATIONTARIF.ToList())
                    {
                        CsModeApplicationTarif ModeApplicationTarif = new CsModeApplicationTarif();
                        ModeApplicationTarif.PK_ID = item.PK_ID;
                        ModeApplicationTarif.DATECREATION = item.DATECREATION;
                        ModeApplicationTarif.DATEMODIFICATION = item.DATEMODIFICATION;
                        ModeApplicationTarif.USERCREATION = item.USERCREATION;
                        ModeApplicationTarif.USERMODIFICATION = item.USERMODIFICATION;
                        ModeApplicationTarif.CODE = item.CODE;
                        ModeApplicationTarif.LIBELLE = item.LIBELLE;



                        ListeModeApplicationTarif.Add(ModeApplicationTarif);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeModeApplicationTarif;
        }
          //Transaction de mise à jour 
        public static int SaveVariableTarif(List<CsVariableDeTarification> ListeVariableDeTarificationToUpdate, List<CsVariableDeTarification> ListeVariableDeTarificationToInserte, List<CsVariableDeTarification> ListeVariableDeTarificationToDelete)
        {
            //bool IsSaved = false;
            int PK_ID = 0;
            List<VARIABLETARIF> ListeVariableDeTarificationToUpdate_ = new List<VARIABLETARIF>();
            List<VARIABLETARIF> ListeVariableDeTarificationToInserte_ = new List<VARIABLETARIF>();
            List<VARIABLETARIF> ListeVariableDeTarificationToDelete_ = new List<VARIABLETARIF>();

            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    convertCsVariableDeTarification(ListeVariableDeTarificationToUpdate, ListeVariableDeTarificationToUpdate_);
                    convertCsVariableDeTarification(ListeVariableDeTarificationToInserte, ListeVariableDeTarificationToInserte_);
                    convertCsVariableDeTarification(ListeVariableDeTarificationToDelete, ListeVariableDeTarificationToDelete_);


                    Entities.UpdateEntity<VARIABLETARIF>(ListeVariableDeTarificationToUpdate_, Context);

                    Entities.InsertEntity<VARIABLETARIF>(ListeVariableDeTarificationToInserte_, Context);

                    Entities.DeleteEntity<VARIABLETARIF>(ListeVariableDeTarificationToDelete_, Context);

                    Context.SaveChanges();

                    if (ListeVariableDeTarificationToUpdate_.Count > 0)
                    {
                        PK_ID = ListeVariableDeTarificationToUpdate_.First().PK_ID;
                    }
                    if (ListeVariableDeTarificationToInserte_.Count > 0)
                    {
                        PK_ID = ListeVariableDeTarificationToInserte_.First().PK_ID;
                    }
                    if (ListeVariableDeTarificationToDelete_.Count > 0)
                    {
                        PK_ID = ListeVariableDeTarificationToDelete_.First().PK_ID;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PK_ID;
        }
       
        //Convertir une liste de CsRedevance en une liste de REDEVANCE
        private static void convertCsVariableDeTarification(List<CsVariableDeTarification> ListeVariableDeTarificationToUpdate, List<VARIABLETARIF> ListeVariableDeTarificationToUpdate_)
        {
            foreach (var item in ListeVariableDeTarificationToUpdate)
            {

                VARIABLETARIF VariableDeTarification = new VARIABLETARIF();

                VariableDeTarification.PK_ID = item.PK_ID;
                VariableDeTarification.CENTRE = item.CENTRE;
                VariableDeTarification.DATECREATION = item.DATECREATION;
                VariableDeTarification.DATEMODIFICATION = item.DATEMODIFICATION;
                VariableDeTarification.COMMUNE = item.COMMUNE;
                VariableDeTarification.USERCREATION = item.USERCREATION;
                VariableDeTarification.USERMODIFICATION = item.USERMODIFICATION;
                VariableDeTarification.COMPTECOMPTABLE = item.COMPTECOMPTABLE;
                VariableDeTarification.DATEAPPLICATION = item.DATEAPPLICATION;
                VariableDeTarification.ESTANALYTIQUE = item.ESTANALYTIQUE;
                VariableDeTarification.FK_IDCENTRE = item.FK_IDCENTRE;
                VariableDeTarification.FK_IDMODEAPPLICATION = item.FK_IDMODEAPPLICATION;
                VariableDeTarification.FK_IDMODECALCUL = item.FK_IDMODECALCUL;
                VariableDeTarification.FK_IDRECHERCHETARIF = item.FK_IDRECHERCHETARIF;
                VariableDeTarification.FK_IDREDEVANCE = item.FK_IDREDEVANCE;
                VariableDeTarification.FORMULE = item.FORMULE;
                VariableDeTarification.GENERATIONANOMALIE = item.GENERATIONANOMALIE;
                VariableDeTarification.LIBELLECOMPTABLE = item.LIBELLECOMPTABLE;
                VariableDeTarification.MODEAPPLICATION = item.MODEAPPLICATION;
                VariableDeTarification.MODECALCUL = item.MODECALCUL;
                VariableDeTarification.ORDREEDITION = item.ORDREEDITION;
                VariableDeTarification.RECHERCHETARIF = item.RECHERCHETARIF;
                VariableDeTarification.REDEVANCE = item.REDEVANCE;
                VariableDeTarification.REGION = item.REGION;
                VariableDeTarification.SREGION = item.SREGION;

                ListeVariableDeTarificationToUpdate_.Add(VariableDeTarification);
            }
        }
        
        #endregion

        #region TarifFacturation
        private static DataTable GetAllTarif(int? idCentre, int? idProduit, int? idRedevance, int? idCodeRecherche)
        {

            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query =
                from TarifFacturation in context.TARIFFACTURATION 
                where (TarifFacturation.FK_IDCENTRE == idCentre || idCentre == null ) && 
                      (TarifFacturation.FK_IDPRODUIT == idProduit || idProduit == null ) &&
                      (TarifFacturation.VARIABLETARIF.REDEVANCE1.PK_ID == idRedevance || idRedevance == null) &&
                      (TarifFacturation.VARIABLETARIF.RECHERCHETARIF1.PK_ID == idCodeRecherche || idCodeRecherche == null) &&
                      (TarifFacturation.FINAPPLICATION == null || TarifFacturation.FINAPPLICATION > System.DateTime.Today )
                select new
                {
                        TarifFacturation.PK_ID ,
                        TarifFacturation.CENTRE ,
                        LIBELLECENTRE =  TarifFacturation.CENTRE1.LIBELLE ,
                        TarifFacturation.DATECREATION ,
                        TarifFacturation.DATEMODIFICATION ,
                        TarifFacturation.COMMUNE,
                        //LIBELLECOMMUNE = lstCommune.FirstOrDefault(c => c.CODE == TarifFacturation.COMMUNE).LIBELLE,
                        TarifFacturation.USERCREATION ,
                        TarifFacturation.USERMODIFICATION ,
                        TarifFacturation.CTARCOMP  ,
                        TarifFacturation.DEBUTAPPLICATION ,
                        TarifFacturation.FINAPPLICATION  ,
                        TarifFacturation.FK_IDCENTRE  ,
                        TarifFacturation.FK_IDPRODUIT,
                        TarifFacturation.FK_IDTAXE ,
                        TarifFacturation.FK_IDUNITECOMPTAGE,
                        TarifFacturation.FK_IDVARIABLETARIF,
                        //TarifFacturation.VARIABLETARIF.MODEAPPLICATION,
                        TarifFacturation.FORFVAL,
                        TarifFacturation.MINIVAL,
                        TarifFacturation.MINIVOL,
                        TarifFacturation.MONTANTANNUEL,
                        TarifFacturation.PERDEB,
                        TarifFacturation.PERFIN ,
                        TarifFacturation.PRODUIT,
                        LIBELLEPRODUIT         = TarifFacturation.PRODUIT1.LIBELLE ,
                        TarifFacturation.RECHERCHETARIF,
                        //LIBELLERECHERCHETARIF = lstRechercheTarif.FirstOrDefault(r => r.CODE == TarifFacturation.RECHERCHETARIF).LIBELLE,
                        TarifFacturation.REDEVANCE ,
                        //LIBELLEREDEVANCE = lstRedevance.FirstOrDefault(r => r.CODE == TarifFacturation.REDEVANCE).LIBELLE,
                        TarifFacturation.REGION  ,
                        TarifFacturation.SREGION ,
                        TAXE = TarifFacturation.TAXE,
                        LIBELLETAXE = TarifFacturation.TAXE1.LIBELLE ,
                        TarifFacturation.UNITE          
                };
                return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }
        private static DataTable GetAllDatailTarif()
        {
            using (galadbEntities context = new galadbEntities())
            {
                IEnumerable<object> query =
                from DetailTarifFacturation in context.DETAILTARIFFACTURATION 
                select new
                {
                     DetailTarifFacturation.PK_ID ,
                     DetailTarifFacturation.FK_IDREDEVANCE,
                     DetailTarifFacturation.FK_IDTARIFFACTURATION,
                     DetailTarifFacturation.NUMEROTRANCHE,
                     DetailTarifFacturation.PRIXUNITAIRE,
                     DetailTarifFacturation.QTEANNUELMAXI,
                     DetailTarifFacturation.DATECREATION,
                     DetailTarifFacturation.DATEMODIFICATION,
                     DetailTarifFacturation.USERCREATION,
                     DetailTarifFacturation.USERMODIFICATION ,
                };
                return Galatee.Tools.Utility.ListToDataTable(query);
            }
        }


        public static List<CsTarifFacturation> LoadAllTarifFacturation(int? idCentre, int? idProduit, int? idRedevance, int? idCodeRecherche, List<CsCommune> lstCommune, List<CsRechercheTarif> lstRchercheTarif, List<CsRedevance> lstRedevance)
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    DataTable dt = GetAllTarif(idCentre,idProduit,   idRedevance,   idCodeRecherche);
                    List<CsTarifFacturation> ListeTarifFacturation = Galatee.Tools.Utility.GetEntityListFromQuery<CsTarifFacturation>(dt);

                    DataTable dts = GetAllDatailTarif();
                    List<CsDetailTarifFacturation> ListeDetailTarifFacturation = Galatee.Tools.Utility.GetEntityListFromQuery<CsDetailTarifFacturation>(dts);
                    Parallel.ForEach(ListeTarifFacturation, item =>
                                    {
                                        item.LIBELLECOMMUNE = lstCommune.FirstOrDefault(c => c.CODE == item.COMMUNE).LIBELLE;
                                        item.LIBELLERECHERCHETARIF = lstRchercheTarif.FirstOrDefault(r => r.CODE == item.RECHERCHETARIF).LIBELLE;
                                        item.LIBELLEREDEVANCE = lstRedevance.FirstOrDefault(r => r.CODE == item.REDEVANCE).LIBELLE;
                                        item.DETAILTARIFFACTURATION = ListeDetailTarifFacturation.Where(t => t.FK_IDTARIFFACTURATION == item.PK_ID).ToList();
                                    });
                    return ListeTarifFacturation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static List<CsDetailTarifFacturation> LoadAllDetailTarifFacturation()
        {
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    DataTable dts = GetAllDatailTarif();
                    List<CsDetailTarifFacturation> ListeDetailTarifFacturation = Galatee.Tools.Utility.GetEntityListFromQuery<CsDetailTarifFacturation>(dts);
                    return ListeDetailTarifFacturation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //public static int SaveTarifFacturation(List<CsTarifFacturation> ListeVariableDeTarificationToUpdate, List<CsTarifFacturation> ListeVariableDeTarificationToInserte, List<CsTarifFacturation> ListeVariableDeTarificationToDelete)
        //{
        //    int PK_ID = 0;
        //    List<TARIFFACTURATION> ListeVariableDeTarificationToUpdate_ = new List<TARIFFACTURATION>();
        //    List<TARIFFACTURATION> ListeVariableDeTarificationToInserte_ = new List<TARIFFACTURATION>();
        //    List<TARIFFACTURATION> ListeVariableDeTarificationToDelete_ = new List<TARIFFACTURATION>();

        //    try
        //    {
        //        using (galadbEntities Context = new galadbEntities())
        //        {
        //            convertCsTarifFacturation(ListeVariableDeTarificationToUpdate, ListeVariableDeTarificationToUpdate_);
        //            convertCsTarifFacturation(ListeVariableDeTarificationToInserte, ListeVariableDeTarificationToInserte_);
        //            convertCsTarifFacturation(ListeVariableDeTarificationToDelete, ListeVariableDeTarificationToDelete_);

        //            Entities.InsertEntity<TARIFFACTURATION>(ListeVariableDeTarificationToInserte_, Context);
        //            if (ListeVariableDeTarificationToUpdate_.Count != 0)
        //            {
        //                List<DETAILTARIFFACTURATION> lstDetailTarif = new List<DETAILTARIFFACTURATION>();
        //                List<DETAILTARIFFACTURATION> lstDetailTarifInsert = new List<DETAILTARIFFACTURATION>();
        //                //Entities.UpdateEntity<TARIFFACTURATION>(ListeVariableDeTarificationToUpdate_, Context);
        //                foreach (TARIFFACTURATION item in ListeVariableDeTarificationToUpdate_)
        //                {
        //                    lstDetailTarif.AddRange(item.DETAILTARIFFACTURATION.Where(t => t.PK_ID != 0));
        //                    lstDetailTarifInsert.AddRange(item.DETAILTARIFFACTURATION.Where(t => t.PK_ID == 0));
        //                }
        //                if (lstDetailTarif.Count != 0)
        //                    Entities.UpdateEntity<DETAILTARIFFACTURATION>(lstDetailTarif, Context);

        //                if (lstDetailTarifInsert.Count != 0)
        //                    Entities.InsertEntity<DETAILTARIFFACTURATION>(lstDetailTarifInsert, Context);

        //            }

        //            if (ListeVariableDeTarificationToDelete_.Count != 0)
        //            {
        //                List<DETAILTARIFFACTURATION> lstDetailTarif = new List<DETAILTARIFFACTURATION>();
        //                foreach (TARIFFACTURATION item in ListeVariableDeTarificationToUpdate_)
        //                    lstDetailTarif.AddRange(item.DETAILTARIFFACTURATION);
        //                Entities.DeleteEntity<DETAILTARIFFACTURATION>(lstDetailTarif, Context);
        //                Entities.DeleteEntity<TARIFFACTURATION>(ListeVariableDeTarificationToDelete_, Context);
        //            }

        //            Context.SaveChanges();
        //            if (ListeVariableDeTarificationToUpdate_.Count > 0)
        //            {
        //                PK_ID = ListeVariableDeTarificationToUpdate_.First().PK_ID;
        //            }
        //            if (ListeVariableDeTarificationToInserte_.Count > 0)
        //            {
        //                PK_ID = ListeVariableDeTarificationToInserte_.First().PK_ID;
        //            }
        //            if (ListeVariableDeTarificationToDelete_.Count > 0)
        //            {
        //                PK_ID = ListeVariableDeTarificationToDelete_.First().PK_ID;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return PK_ID;
        //}

          //Transaction de mise à jour 
        public static bool SaveTarifFacturation(CsTarifFacturation Tarification,int Action)
        {
            try
            {
                int resultat = -1;
                using (galadbEntities Context = new galadbEntities())
                {
                    if (Action == 1)
                    {
                        TARIFFACTURATION leTarif = Galatee.Tools.Utility.ConvertEntity<TARIFFACTURATION, CsTarifFacturation>(Tarification);
                        List<DETAILTARIFFACTURATION> DetailleTarif = Galatee.Tools.Utility.ConvertListType<DETAILTARIFFACTURATION, CsDetailTarifFacturation>(Tarification.DETAILTARIFFACTURATION.ToList());
                        leTarif.DETAILTARIFFACTURATION = DetailleTarif;
                        Entities.InsertEntity<TARIFFACTURATION>(leTarif, Context);
                    }
                   else  if (Action == 2)
                    {
                        TARIFFACTURATION leTarif = Galatee.Tools.Utility.ConvertEntity<TARIFFACTURATION, CsTarifFacturation>(Tarification);
                        TARIFFACTURATION leTarifAnc = Context.TARIFFACTURATION.FirstOrDefault(t => t.FK_IDCENTRE == leTarif.FK_IDCENTRE && t.FK_IDPRODUIT == leTarif.FK_IDPRODUIT &&
                                                                                                  t.FK_IDVARIABLETARIF == leTarif.FK_IDVARIABLETARIF);
                        if (leTarifAnc != null)
                            leTarifAnc.FINAPPLICATION = System.DateTime.Today;

                        List<DETAILTARIFFACTURATION> DetailleTarif = Galatee.Tools.Utility.ConvertListType<DETAILTARIFFACTURATION, CsDetailTarifFacturation>(Tarification.DETAILTARIFFACTURATION.ToList());
                        leTarif.DETAILTARIFFACTURATION = DetailleTarif.Where(t => t.QTEANNUELMAXI != 0 && t.PRIXUNITAIRE != 0).ToList();
                        leTarif.PK_ID = 0;
                        Entities.InsertEntity<TARIFFACTURATION>(leTarif, Context);   


                        //TARIFFACTURATION leTarif = Galatee.Tools.Utility.ConvertEntity<TARIFFACTURATION, CsTarifFacturation>(Tarification);
                        //Entities.UpdateEntity <TARIFFACTURATION>(leTarif, Context);

                        //List<DETAILTARIFFACTURATION> DetailleTarifInsert = Galatee.Tools.Utility.ConvertListType<DETAILTARIFFACTURATION, CsDetailTarifFacturation>(Tarification.DETAILTARIFFACTURATION.ToList());
                        //if (DetailleTarifInsert != null && DetailleTarifInsert.Count != 0)
                        //{
                        //    foreach (DETAILTARIFFACTURATION item in DetailleTarifInsert)
                        //    {
                        //        DETAILTARIFFACTURATION leDetTarifAnc = Context.DETAILTARIFFACTURATION.FirstOrDefault(t => t.FK_IDTARIFFACTURATION  == leTarif.PK_ID  && t.FK_IDREDEVANCE  == item.FK_IDREDEVANCE  &&
                        //                                                          t.NUMEROTRANCHE == item.NUMEROTRANCHE);
                        //        if (leDetTarifAnc != null)
                        //            Entities.UpdateEntity<DETAILTARIFFACTURATION>(leDetTarifAnc, Context);
                        //        else
                        //        {
                        //            item.FK_IDTARIFFACTURATION = leTarif.PK_ID;
                        //            Entities.InsertEntity<DETAILTARIFFACTURATION>(item, Context);
                        //        }
                        //    }
                        //}
                    }
                    else if (Action == 3)
                    {
                        TARIFFACTURATION leTarif = Galatee.Tools.Utility.ConvertEntity<TARIFFACTURATION, CsTarifFacturation>(Tarification);
                        TARIFFACTURATION leTarifAnc = Context.TARIFFACTURATION.FirstOrDefault(t => t.FK_IDCENTRE == leTarif.FK_IDCENTRE && t.FK_IDPRODUIT == leTarif.FK_IDPRODUIT &&
                                                                                                  t.FK_IDVARIABLETARIF == leTarif.FK_IDVARIABLETARIF);
                        if (leTarifAnc != null)
                            leTarifAnc.FINAPPLICATION = System.DateTime.Today;

                    }
                    resultat= Context.SaveChanges();
                    return resultat < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       
        public static bool  CreateTarif(List<CsTarifFacturation> Tarification)
        {
            try
            {
                int resultat = -1;

                

                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (CsTarifFacturation item in Tarification)
                    {
                        TARIFFACTURATION leTarifAnc = Context.TARIFFACTURATION.FirstOrDefault(t => t.FK_IDCENTRE == item.FK_IDCENTRE && t.FK_IDPRODUIT == item.FK_IDPRODUIT &&
                                                                                                 t.FK_IDVARIABLETARIF == item.FK_IDVARIABLETARIF);
                        if (leTarifAnc != null)
                            leTarifAnc.FINAPPLICATION = System.DateTime.Today;

                        TARIFFACTURATION leTarif = Galatee.Tools.Utility.ConvertEntity<TARIFFACTURATION, CsTarifFacturation>(item);
                        List<DETAILTARIFFACTURATION> DetailleTarif = Galatee.Tools.Utility.ConvertListType<DETAILTARIFFACTURATION, CsDetailTarifFacturation>(item.DETAILTARIFFACTURATION.ToList());
                        leTarif.DETAILTARIFFACTURATION = DetailleTarif.Where(t=>t.QTEANNUELMAXI != 0 && t.PRIXUNITAIRE != 0 ).ToList();
                        

                        Entities.InsertEntity<TARIFFACTURATION>(leTarif, Context);   
                    }
                    resultat= Context.SaveChanges();
                    return resultat < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

        public static bool DuplicationTarifVersCentre(int AncienIdCentre, int NouveauIdCentre,int? produit)
        {
            try
            {
                  List<CsTarifFacturation> lstTarif = Entities.GetEntityListFromQuery<CsTarifFacturation>(GetAllTarif(AncienIdCentre, produit, null, null));
                    List<CsDetailTarifAnnuel> lstDetTarif = Entities.GetEntityListFromQuery<CsDetailTarifAnnuel >(GetAllDatailTarif());
                    List<CsVariableTarif> lesVariableTarfication = Entities.GetEntityListFromQuery<CsVariableTarif>(GetAllVariableDeTarif());

                    galadbEntities ctxInter = new galadbEntities();
                    var ListeVariable = lesVariableTarfication.Where(t => t.FK_IDCENTRE == AncienIdCentre).ToList();
                    var leCentre = ctxInter.CENTRE.FirstOrDefault(c => c.PK_ID == NouveauIdCentre);
                    foreach (var item in ListeVariable)
                    {
                        item.FK_IDCENTRE = leCentre.PK_ID;
                        item.CENTRE = leCentre.CODE;
                    }
                    List<TARIFFACTURATION> lstTariffac = new List<TARIFFACTURATION>();
                    List<VARIABLETARIF> tarvar = Entities.ConvertObject<VARIABLETARIF, CsVariableTarif>(ListeVariable);
                    using(galadbEntities Context = new galadbEntities())
	                {
		                    Entities.InsertEntity<VARIABLETARIF>(tarvar,Context);
                            foreach (CsTarifFacturation item in lstTarif)
                            {
                                var laVariableTarif = tarvar.FirstOrDefault(t => t.RECHERCHETARIF == item.RECHERCHETARIF && t.REDEVANCE == item.REDEVANCE);
                                if (laVariableTarif == null) continue;
                                TARIFFACTURATION leTarif =  Entities.ConvertObject<TARIFFACTURATION, CsTarifFacturation >(item);
                                leTarif.FK_IDCENTRE = NouveauIdCentre;
                                leTarif.CENTRE = leCentre.CODE;
                                leTarif.FK_IDVARIABLETARIF = laVariableTarif.PK_ID;
                                leTarif.DETAILTARIFFACTURATION = Entities.ConvertObject<DETAILTARIFFACTURATION , CsDetailTarifAnnuel>(lstDetTarif.Where(t=>t.FK_IDTARIFANNUEL == item.PK_ID ).ToList());
                                lstTariffac.Add(leTarif);
                                Entities.InsertEntity<TARIFFACTURATION>(leTarif, Context);
                            }
                        int rest =   Context.SaveChanges();
                        return rest==0?false :true ;
	                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void convertCsTarifFacturation(List<CsTarifFacturation> ListeVariableDeTarificationToUpdate, List<TARIFFACTURATION> ListeVariableDeTarificationToUpdate_)
        {
            foreach (var item in ListeVariableDeTarificationToUpdate)
            {

                TARIFFACTURATION TarifFacturation = new TARIFFACTURATION();

                TarifFacturation.PK_ID = item.PK_ID;
                TarifFacturation.CENTRE =!string.IsNullOrWhiteSpace(item.CENTRE)?item.CENTRE:string.Empty;
                //TarifFacturation.LIBELLECENTRE = item.CENTRE1.LIBELLE;
                TarifFacturation.DATECREATION = item.DATECREATION!=null?item.DATECREATION:new DateTime();
                TarifFacturation.DATEMODIFICATION = item.DATEMODIFICATION;
                TarifFacturation.COMMUNE = !string.IsNullOrWhiteSpace(item.COMMUNE) ? item.COMMUNE : string.Empty;
                //TarifFacturation.LIBELLECOMMUNE = Context.COMMUNE.FirstOrDefault(c => c.CODE == item.COMMUNE).LIBELLE;
                TarifFacturation.USERCREATION = !string.IsNullOrWhiteSpace(item.USERCREATION) ? item.USERCREATION : string.Empty;
                TarifFacturation.USERMODIFICATION = !string.IsNullOrWhiteSpace(item.USERMODIFICATION)?item.USERMODIFICATION:string.Empty;
                TarifFacturation.CTARCOMP = !string.IsNullOrWhiteSpace(item.CTARCOMP) ? item.CTARCOMP : string.Empty; 
                TarifFacturation.DEBUTAPPLICATION = item.DEBUTAPPLICATION!=null?item.DEBUTAPPLICATION:new DateTime();
                TarifFacturation.FINAPPLICATION = item.FINAPPLICATION;
                TarifFacturation.FK_IDCENTRE = item.FK_IDCENTRE!=null?item.FK_IDCENTRE:0;
                TarifFacturation.FK_IDPRODUIT = item.FK_IDPRODUIT != null ? item.FK_IDPRODUIT : 0;
                TarifFacturation.FK_IDTAXE =item.FK_IDTAXE != null ? item.FK_IDTAXE : 0 ;
                TarifFacturation.FK_IDUNITECOMPTAGE =item.FK_IDUNITECOMPTAGE != null ? item.FK_IDUNITECOMPTAGE : 0 ;
                TarifFacturation.FK_IDVARIABLETARIF = item.FK_IDVARIABLETARIF != null ? item.FK_IDVARIABLETARIF : 0;
                //TarifFacturation.MODEAPPLICATION = Context.VARIABLETARIF.FirstOrDefault(v => v.PK_ID == item.FK_IDVARIABLETARIF).MODEAPPLICATION;
                TarifFacturation.FORFVAL = item.FORFVAL;
                TarifFacturation.MINIVAL = item.MINIVAL;
                TarifFacturation.MINIVOL = item.MINIVOL;
                TarifFacturation.MONTANTANNUEL = item.MONTANTANNUEL;
                TarifFacturation.PERDEB =!string.IsNullOrWhiteSpace( item.PERDEB)?item.PERDEB:string.Empty;
                TarifFacturation.PERFIN = !string.IsNullOrWhiteSpace(item.PERFIN) ? item.PERFIN : string.Empty;
                TarifFacturation.PRODUIT = !string.IsNullOrWhiteSpace(item.PRODUIT) ? item.PRODUIT : string.Empty;
                //TarifFacturation.LIBELLEPRODUIT = item.PRODUIT1.LIBELLE;
                TarifFacturation.RECHERCHETARIF = !string.IsNullOrWhiteSpace(item.RECHERCHETARIF) ? item.RECHERCHETARIF : string.Empty;
                //TarifFacturation.LIBELLERECHERCHETARIF = Context.RECHERCHETARIF.FirstOrDefault(r => r.CODE == item.RECHERCHETARIF).LIBELLE;
                TarifFacturation.REDEVANCE = !string.IsNullOrWhiteSpace(item.REDEVANCE) ? item.REDEVANCE : string.Empty;
                //TarifFacturation.LIBELLEREDEVANCE = Context.REDEVANCE.FirstOrDefault(r => r.CODE == item.REDEVANCE).LIBELLE;
                TarifFacturation.REGION = !string.IsNullOrWhiteSpace(item.REGION) ? item.REGION : string.Empty;
                TarifFacturation.SREGION = !string.IsNullOrWhiteSpace(item.SREGION) ? item.SREGION : string.Empty;
                TarifFacturation.TAXE = !string.IsNullOrWhiteSpace(item.TAXE) ? item.TAXE : string.Empty;
                TarifFacturation.UNITE = !string.IsNullOrWhiteSpace(item.UNITE) ? item.UNITE : string.Empty;


                TarifFacturation.DETAILTARIFFACTURATION = new List<DETAILTARIFFACTURATION>();

                foreach (var item_ in item.DETAILTARIFFACTURATION)
                {
                    DETAILTARIFFACTURATION DetailTarifFacturation = new DETAILTARIFFACTURATION();
                    DetailTarifFacturation.FK_IDREDEVANCE = item_.FK_IDREDEVANCE!=null?item_.FK_IDREDEVANCE:0;
                    DetailTarifFacturation.FK_IDTARIFFACTURATION = TarifFacturation.PK_ID;
                    DetailTarifFacturation.PK_ID = item_.PK_ID;
                    DetailTarifFacturation.NUMEROTRANCHE = item_.NUMEROTRANCHE!=null?item_.NUMEROTRANCHE:new byte();
                    DetailTarifFacturation.PRIXUNITAIRE = item_.PRIXUNITAIRE;
                    DetailTarifFacturation.QTEANNUELMAXI = item_.QTEANNUELMAXI!=null?item_.QTEANNUELMAXI:0;

                    TarifFacturation.DETAILTARIFFACTURATION.Add(DetailTarifFacturation);
                }


                ListeVariableDeTarificationToUpdate_.Add(TarifFacturation);
            }
        }

        public static List<CsUniteComptage> LoadAllUniteComptage()
        {
            List<CsUniteComptage> ListeTarifFacturation = new List<CsUniteComptage>();
            try
            {
                using (galadbEntities Context = new galadbEntities())
                {
                    foreach (var item in Context.UNITECOMPTAGE.Take(500).ToList())
                    {
                        CsUniteComptage TarifFacturation     = new CsUniteComptage();
                        TarifFacturation.PK_ID                  = item.PK_ID                ;
                        TarifFacturation.CODE        = item.CODE;
                        TarifFacturation.LIBELLE                = item.LIBELLE      ;
                        TarifFacturation.DATECREATION           = item.DATECREATION         ;
                        TarifFacturation.DATEMODIFICATION       = item.DATEMODIFICATION     ;
                        TarifFacturation.USERCREATION           = item.USERCREATION         ;

                        ListeTarifFacturation.Add(TarifFacturation);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListeTarifFacturation;
        }

        #endregion

        #region Generation de Ctarcomp

        public static List<CsTarifFacturation> LoadTarifGenerer(string FK_IDRECHERCHETARIF, string FK_IDVARIABLETARIF,string Produit)
        {
            try
            {
                List<CsTarifFacturation> ListeTarifFacturation = new List<CsTarifFacturation>();

                using (galadbEntities Context = new galadbEntities())
                {
                    List<string> cmb = new List<string>();

                    int PK_IDRECHERCHETARIF = int.Parse(FK_IDRECHERCHETARIF);

                    //Identification des info lié à la recherche
                    RECHERCHETARIF RECHERCHETARIF = Context.RECHERCHETARIF.FirstOrDefault(r => r.PK_ID == PK_IDRECHERCHETARIF);

                    if (RECHERCHETARIF!=null)
                    {
                        List<List<string>> ListChampCombinaison = new List<List<string>>();

                        foreach (var item in RECHERCHETARIF.CTARCOMP.OrderBy(c=>c.ORDRE))
                        {
                            string TABLEREFERENCE = item.CONTENANTCRITERETARIF.TABLEREFERENCE;
                            bool IsLieProduit = item.CONTENANTCRITERETARIF.AVECPRODUIT;
                            if (IsLieProduit )
                                ListChampCombinaison.Add(GetCodeValueFromTableAvecProduit(TABLEREFERENCE,Produit, Context));
                            else 
                            ListChampCombinaison.Add(GetCodeValueFromTable(TABLEREFERENCE, Context));
                        }

                        int i = 1;
                        cmb = DoCombinaison(ListChampCombinaison[0], ListChampCombinaison[1]);
                        while (ListChampCombinaison.Count() > i + 1)
                        {
                            cmb = DoCombinaison(cmb, ListChampCombinaison[i + 1]);
                        }


                        int IDVARIABLETARIF=int.Parse(FK_IDVARIABLETARIF);
                        VARIABLETARIF VARIABLETARIF = Context.VARIABLETARIF.FirstOrDefault(v => v.PK_ID == IDVARIABLETARIF);
                        List<RECHERCHETARIF> lstRechtarif = Context.RECHERCHETARIF.Where(r => r.CODE == VARIABLETARIF.RECHERCHETARIF).ToList();
                        List<REDEVANCE > lstRedevance = Context.REDEVANCE.Where(r => r.CODE == VARIABLETARIF.REDEVANCE ).ToList();

                        foreach (var item in cmb)
                        {
                            CsTarifFacturation TarifFacturation = new CsTarifFacturation();
                            //TarifFacturation.PK_ID = item.PK_ID;
                            TarifFacturation.CENTRE = VARIABLETARIF.CENTRE;
                            TarifFacturation.LIBELLECENTRE = VARIABLETARIF.CENTRE1.LIBELLE;
                            TarifFacturation.DATECREATION = DateTime.Now;
                            TarifFacturation.DATEMODIFICATION = DateTime.Now;
                            TarifFacturation.COMMUNE = string.IsNullOrEmpty(VARIABLETARIF.COMMUNE) ? "00000" : VARIABLETARIF.COMMUNE;
                            //TarifFacturation.LIBELLECOMMUNE = Context.COMMUNE.FirstOrDefault(c => c.CODE == VARIABLETARIF.COMMUNE).LIBELLE;
                            TarifFacturation.USERCREATION = VARIABLETARIF.USERCREATION;
                            TarifFacturation.USERMODIFICATION = VARIABLETARIF.USERMODIFICATION;
                            TarifFacturation.CTARCOMP = item;
                            TarifFacturation.DEBUTAPPLICATION = DateTime.Now;
                            //TarifFacturation.FINAPPLICATION = item.FINAPPLICATION;
                            TarifFacturation.FK_IDCENTRE = VARIABLETARIF.FK_IDCENTRE;
                            TarifFacturation.FK_IDPRODUIT = VARIABLETARIF.REDEVANCE1.PRODUIT1.PK_ID;
                            //TarifFacturation.FK_IDTAXE = VARIABLETARIF.FK_IDTAXE;
                            //TarifFacturation.FK_IDUNITECOMPTAGE = VARIABLETARIF.FK_IDUNITECOMPTAGE;
                            TarifFacturation.FK_IDVARIABLETARIF = VARIABLETARIF.PK_ID;
                            TarifFacturation.MODEAPPLICATION = VARIABLETARIF.MODEAPPLICATION;
                            //Context.VARIABLETARIF.FirstOrDefault(v => v.PK_ID == VARIABLETARIF.PK_ID).MODEAPPLICATION;
                            TarifFacturation.FORFVAL = 0;
                            TarifFacturation.MINIVAL = 0L;
                            TarifFacturation.MINIVOL = 0;
                            TarifFacturation.MONTANTANNUEL = 0;
                            TarifFacturation.PERDEB = string.Empty;
                            TarifFacturation.PERFIN = string.Empty;
                            TarifFacturation.PRODUIT = VARIABLETARIF.REDEVANCE1.PRODUIT;
                            TarifFacturation.LIBELLEPRODUIT = VARIABLETARIF.REDEVANCE1.PRODUIT1.LIBELLE;
                            TarifFacturation.RECHERCHETARIF = VARIABLETARIF.RECHERCHETARIF;
                            TarifFacturation.LIBELLERECHERCHETARIF =lstRechtarif.FirstOrDefault(r => r.CODE == VARIABLETARIF.RECHERCHETARIF).LIBELLE;
                            TarifFacturation.REDEVANCE = VARIABLETARIF.REDEVANCE;
                            TarifFacturation.LIBELLEREDEVANCE = lstRedevance.FirstOrDefault(r => r.CODE == VARIABLETARIF.REDEVANCE).LIBELLE;
                            TarifFacturation.REGION = VARIABLETARIF.REGION;
                            TarifFacturation.SREGION = VARIABLETARIF.SREGION;
                            //TarifFacturation.TAXE = Context.TAXE.FirstOrDefault(t => t.CODE == VARIABLETARIF.TAXE) != null ? Context.TAXE.FirstOrDefault(t => t.CODE == item.TAXE).LIBELLE : string.Empty;
                            //TarifFacturation.UNITE = VARIABLETARIF.UNITE;


                            TarifFacturation.DETAILTARIFFACTURATION = new List<CsDetailTarifFacturation>();

                            foreach (var item_ in VARIABLETARIF.REDEVANCE1.TRANCHEREDEVANCE)
                            {
                                CsDetailTarifFacturation DetailTarifFacturation = new CsDetailTarifFacturation();
                                DetailTarifFacturation.FK_IDREDEVANCE = item_.FK_IDREDEVANCE;
                                DetailTarifFacturation.FK_IDTARIFFACTURATION = TarifFacturation.PK_ID;
                                DetailTarifFacturation.NUMEROTRANCHE = 1;
                                DetailTarifFacturation.PRIXUNITAIRE = 0;
                                DetailTarifFacturation.QTEANNUELMAXI =0;

                                TarifFacturation.DETAILTARIFFACTURATION.Add(DetailTarifFacturation);
                            }

                            ListeTarifFacturation.Add(TarifFacturation);
                            continue;
                        }

                    }
                }
                return ListeTarifFacturation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static List<string> DoCombinaison(List<string> ListeToConcat, List<string> ListeUsedToConcat)
        {
            List<string> RetourCtarComp = new List<string>();
            foreach (var itemToConcat in ListeToConcat)
            {
                foreach (var itemUsedToConcat in ListeUsedToConcat)
                {
                    RetourCtarComp.Add(itemToConcat + itemUsedToConcat);
                }
            }

            return RetourCtarComp;
        }
        private static List<string> GetCodeValueFromTable(string TABLEREFERENCE, galadbEntities Context)
        {
            List<string> ListeCode = new List<string>();

            dynamic TABLE = Context.GetType().InvokeMember(
                TABLEREFERENCE.Trim(),
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                null, Context, null);


            foreach (var item in TABLE)
            {               
                ListeCode.Add(item.CODE);
            }

            

            return ListeCode;
        }

        private static List<string> GetCodeValueFromTableAvecProduit(string TABLEREFERENCE,string  PRODUIT, galadbEntities Context)
        {
            List<string> ListeCode = new List<string>();

            dynamic TABLE = Context.GetType().InvokeMember(
                TABLEREFERENCE.Trim(),
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                null, Context, null);


            foreach (var item in TABLE)
            {
                 if (item.PRODUIT ==PRODUIT)
                   ListeCode.Add(item.CODE);
            }



            return ListeCode;
        }


        #endregion



    }


}
