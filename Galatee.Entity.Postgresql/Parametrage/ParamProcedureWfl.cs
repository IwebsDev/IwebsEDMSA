using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;

namespace Galatee.Entity.Model
{
    public static partial class ParamProcedure
    {
        #region WORKFLOW

        public static DataTable LISTE_WKF_OPERATION_PARENT()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from OPs in context.OPERATION
                  where OPs.FK_ID_PARENTOPERATION == Guid.Empty || OPs.FK_ID_PARENTOPERATION == null
                  select new
                  {
                      OPs.PK_ID,
                      OPs.CODE,
                      OPs.NOM,
                      OPs.DESCRIPTION,
                      OPs.FK_ID_PRODUIT,
                      OPs.FK_ID_PARENTOPERATION,
                      OPs.FK_IDFORMULAIRE,
                      OPs.FORMULAIRE,
                      OPs.CODE_TDEM,
                      PRODUITNAME = string.Empty,
                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ALLSOUSOPERATIONS(Guid FK_IDOperationParent)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from OPs in context.OPERATION
                  where OPs.FK_ID_PARENTOPERATION == FK_IDOperationParent
                  select new
                  {
                      OPs.PK_ID,
                      OPs.CODE,
                      OPs.NOM,
                      OPs.DESCRIPTION,
                      OPs.FK_ID_PRODUIT,
                      OPs.FK_ID_PARENTOPERATION,
                      OPs.FK_IDFORMULAIRE,
                      OPs.FORMULAIRE,
                      OPs.CODE_TDEM,
                      PRODUITNAME = string.Empty
                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ALLOPERATIONSBYDEMANDE(string NUMDEM)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from OPs in context.OPERATION
                  where OPs.CODE_TDEM == NUMDEM 
                  select new
                  {
                      OPs.PK_ID,
                      OPs.CODE,
                      OPs.NOM,
                      OPs.DESCRIPTION,
                      OPs.FK_ID_PRODUIT,
                      OPs.FK_ID_PARENTOPERATION,
                      OPs.FK_IDFORMULAIRE,
                      OPs.FORMULAIRE,
                      OPs.CODE_TDEM,
                      PRODUITNAME = string.Empty
                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LISTE_WKF_ALLOPERATIONS()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from OPs in context.OPERATION
                  select new
                  {
                      OPs.PK_ID,
                      OPs.CODE,
                      OPs.NOM,
                      OPs.DESCRIPTION,
                      OPs.FK_ID_PRODUIT,
                      OPs.FK_ID_PARENTOPERATION,
                      OPs.FK_IDFORMULAIRE,
                      OPs.FORMULAIRE,
                      OPs.CODE_TDEM,
                      PRODUITNAME = string.Empty
                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_CONFIGURATIONWORKFLOWCENTRE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from OPs in context.vwCONFIGURATIONWORKFLOWCENTRE
                  select new
                  {
                      OPs.PK_ID,
                      OPs.CENTREID,
                      OPs.CODECENTRE,
                      OPs.CODESITE,
                      OPs.FK_IDCODESITE,
                      OPs.FK_IDFORMULAIRE,
                      OPs.FORMULAIRE,
                      OPs.LIBELLE,
                      OPs.NOM,
                      OPs.OPERATIONID,
                      OPs.WORKFLOWNAME
                  };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ETAPES()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.ETAPE
                    select new
                    {
                        OPs.PK_ID,
                        OPs.CODE,
                        OPs.NOM,
                        OPs.DESCRIPTIONETAPE,
                        OPs.CONTROLEETAPE,
                        OPs.FK_IDMENU,
                        OPs.FK_IDFORMULAIRE,
                        OPs.FK_IDOPERATION,
                        OPs.ISMODIFICATION,
                        OPs.IS_TRAITEMENT_LOT
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ETAPESbyId(int idEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.ETAPE
                    where OPs.PK_ID == idEtape
                    select new
                    {
                        OPs.PK_ID,
                        OPs.CODE,
                        OPs.NOM,
                        OPs.DESCRIPTIONETAPE,
                        OPs.CONTROLEETAPE,
                        OPs.FK_IDMENU,
                        OPs.FK_IDFORMULAIRE,
                        OPs.FK_IDOPERATION,
                        OPs.ISMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ETAPES(Guid OperationId)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.ETAPE
                    where OPs.FK_IDOPERATION == OperationId
                    select new
                    {
                        OPs.PK_ID,
                        OPs.CODE,
                        OPs.NOM,
                        OPs.DESCRIPTIONETAPE,
                        OPs.CONTROLEETAPE,
                        OPs.FK_IDMENU,
                        OPs.FK_IDFORMULAIRE,
                        OPs.FK_IDOPERATION,
                        OPs.ISMODIFICATION 
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_GROUPES_VALIDATION_BY_ID(Guid  IdGrpValidation)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.GROUPE_VALIDATION
                    where OPs.PK_ID == IdGrpValidation
                    select new
                    {
                        OPs.PK_ID,
                        OPs.GROUPENAME,
                        OPs.DESCRIPTION,
                        OPs.EMAILDIFFUSION,
                        OPs.UNESEULEVALIDATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public static DataTable LISTE_WKF_GROUPES_VALIDATION()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.GROUPE_VALIDATION
                    select new
                    {
                        OPs.PK_ID,
                        OPs.GROUPENAME,
                        OPs.DESCRIPTION,
                        OPs.EMAILDIFFUSION,
                        OPs.UNESEULEVALIDATION,
                        OPs.VALEURSPECIFIQUE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public static DataTable LISTE_WKF_HABILITATION_GROUPE_VALIDATION(Guid pkGrp)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.RHABILITATIONGROUPEVALIDATION
                    from U in context.ADMUTILISATEUR
                        .Where(usr => usr.PK_ID == OPs.FK_IDADMUTILISATEUR)
                    where OPs.FK_IDGROUPE_VALIDATION == pkGrp
                    select new
                    {
                        OPs.PK_ID,
                        OPs.DATE_CREATION_HABILITATION,
                        OPs.DATE_DERNIEREMODIFICATION,
                        OPs.DATE_FIN_VALIDITE,
                        OPs.DATE_HABILITATION,
                        OPs.FK_IDADMUTILISATEUR,
                        OPs.FK_IDGROUPE_VALIDATION,
                        OPs.MATRICULE_USER_CREATION,
                        OPs.MATRICULE_USER_MODIFICATION,
                        OPs.RANG,
                        OPs.ESTRESPONSABLE,
                        LOGINNAME = U.LOGINNAME,
                        LIBELLE = U.LIBELLE,
                        EMAIL = U.E_MAIL,
                        OPs.ESTCONSULTATION ,

                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.WORKFLOW
                    from TB in context.TABLETRAVAIL
                        .Where(t => t.PK_ID == OPs.FK_IDTABLE_TRAVAIL)
                        .DefaultIfEmpty() //Left outer join
                    select new
                    {
                        OPs.PK_ID,
                        OPs.WORKFLOWNAME,
                        OPs.CODE,
                        OPs.DESCRIPTION,
                        OPs.FK_IDTABLE_TRAVAIL,
                        TABLENOM = TB.NOM,
                        TABLENAME = TB.TABLE_NAME,
                        TABLEDESCRIPTION = TB.DESCRIPTION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_FORMULAIRE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from OPs in context.FORMULAIRE
                    select new
                    {
                        OPs.PK_ID,
                        OPs.FORMULAIRE1,
                        OPs.FULLNAMECONTROLE,
                        OPs.CREATIONDEMANDE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_TABLETRAVAIL()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.TABLETRAVAIL
                    select new
                    {
                        TB.PK_ID,
                        TB.NOM,
                        TB.TABLE_NAME,
                        TB.DESCRIPTION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_ETAPESWORFKLOW(Guid pRaffectationWkfCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RAFFECTATIONETAPEWORKFLOW
                    from step in context.ETAPE.Where(stp => stp.PK_ID == TB.FK_IDETAPE)
                    where TB.FK_RWORKFLOWCENTRE == pRaffectationWkfCentre
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_RWORKFLOWCENTRE,
                        TB.ORDRE,
                        TB.FK_IDETAPE,
                        TB.CONDITION,
                        TB.FROMCONDITION,
                        TB.FK_IDRETAPEWORKFLOWORIGINE,
                        CODEETAPE = step.CODE,
                        LIBELLEETAPE = step.NOM,
                        TB.USEAFFECTATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        public static DataTable LISTE_WKF_ETAPESWORFKLOWDETOURNE(Guid pOrigineAffetation)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RAFFECTATIONETAPEWORKFLOW
                    from step in context.ETAPE.Where(stp => stp.PK_ID == TB.FK_IDETAPE)
                    where TB.FK_IDRETAPEWORKFLOWORIGINE == pOrigineAffetation
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_RWORKFLOWCENTRE,
                        TB.ORDRE,
                        TB.FK_IDETAPE,
                        TB.CONDITION,
                        TB.FROMCONDITION,
                        TB.FK_IDRETAPEWORKFLOWORIGINE,
                        CODEETAPE = step.CODE,
                        LIBELLEETAPE = step.NOM
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_CONDITION(Guid pAffEtapeWKF)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.CONDITIONBRANCHEMENT
                    where TB.FK_IDRAFFECTATIONWKF == pAffEtapeWKF
                    select new
                    {
                        TB.PK_ID,
                        TB.NOM,
                        TB.COLONNENAME,
                        TB.FK_IDETAPEFAUSE,
                        TB.FK_IDETAPEVRAIE,
                        TB.FK_IDRAFFECTATIONWKF,
                        TB.OPERATEUR,
                        TB.FK_IDTABLETRAVAIL,
                        TB.VALUE,
                        TB.PEUT_TRANSMETTRE_SI_FAUX
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_RAFFECTATION_WKF_CENTRE(Guid pKIDWKF, int CpKID, Guid OpPKID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RWORFKLOWCENTRE
                    where TB.FK_IDWORKFLOW == pKIDWKF && TB.FK_IDCENTRE == CpKID && TB.FK_IDOPERATION == OpPKID
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDCENTRE,
                        TB.FK_IDOPERATION,
                        TB.FK_IDWORKFLOW
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_DEMANDE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.DEMANDE_WORFKLOW
                    select new
                    {
                        TB.PK_ID,
                        TB.ALLCENTRE,
                        TB.CODE,
                        TB.DATECREATION,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDETAPEPRECEDENTE,
                        TB.FK_IDETAPESUIVANTE,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.FK_IDOPERATION,
                        TB.FK_IDRWORKLOW,
                        TB.FK_IDSTATUS,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDWORKFLOW,
                        TB.MATRICULEUSERCREATION,
                        TB.MATRICULEUSERMODIFICATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.FK_IDETAPECIRCUIT,
                        TB.CODE_DEMANDE_TABLETRAVAIL
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_DEMANDE(List<string> pIdDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.DEMANDE_WORFKLOW
                    where pIdDemande.Contains(TB.FK_IDLIGNETABLETRAVAIL)
                    select new
                    {
                        TB.PK_ID,
                        TB.ALLCENTRE,
                        TB.CODE,
                        TB.DATECREATION,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDETAPEPRECEDENTE,
                        TB.FK_IDETAPESUIVANTE,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.FK_IDOPERATION,
                        TB.FK_IDRWORKLOW,
                        TB.FK_IDSTATUS,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDWORKFLOW,
                        TB.MATRICULEUSERCREATION,
                        TB.MATRICULEUSERMODIFICATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.FK_IDETAPECIRCUIT,
                        TB.CODE_DEMANDE_TABLETRAVAIL,
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable LISTE_WKF_DEMANDE(string CodeDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.DEMANDE_WORFKLOW 
                    where TB.CODE == CodeDemande
                    select new
                    {
                        TB.PK_ID,
                        TB.ALLCENTRE,
                        TB.CODE,
                        TB.DATECREATION,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDETAPEPRECEDENTE,
                        TB.FK_IDETAPESUIVANTE,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.FK_IDOPERATION,
                        TB.FK_IDRWORKLOW,
                        TB.FK_IDSTATUS,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDWORKFLOW,
                        TB.MATRICULEUSERCREATION,
                        TB.MATRICULEUSERMODIFICATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.FK_IDETAPECIRCUIT,
                        TB.CODE_DEMANDE_TABLETRAVAIL,
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LISTE_WKF_DEMANDE_Code_Matricule(string CodeDemande,string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    int id_centre=context.ADMUTILISATEUR.FirstOrDefault(u=>u.MATRICULE==Matricule).FK_IDCENTRE;
                    IEnumerable<object> query =
                    from TB in context.DEMANDE_WORFKLOW
                    where TB.CODE == CodeDemande && TB.FK_IDCENTRE == id_centre
                    select new
                    {
                        TB.PK_ID,
                        TB.ALLCENTRE,
                        TB.CODE,
                        TB.DATECREATION,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDETAPEPRECEDENTE,
                        TB.FK_IDETAPESUIVANTE,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.FK_IDOPERATION,
                        TB.FK_IDRWORKLOW,
                        TB.FK_IDSTATUS,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDWORKFLOW,
                        TB.MATRICULEUSERCREATION,
                        TB.MATRICULEUSERMODIFICATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.FK_IDETAPECIRCUIT,
                        TB.CODE_DEMANDE_TABLETRAVAIL,
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_DEMANDEBYNUM_DEMANDE(string CodeDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.DEMANDE_WORFKLOW
                    where TB.CODE_DEMANDE_TABLETRAVAIL == CodeDemande
                    select new
                    {
                        TB.PK_ID,
                        TB.ALLCENTRE,
                        TB.CODE,
                        TB.DATECREATION,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDETAPEPRECEDENTE,
                        TB.FK_IDETAPESUIVANTE,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.FK_IDOPERATION,
                        TB.FK_IDRWORKLOW,
                        TB.FK_IDSTATUS,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDWORKFLOW,
                        TB.MATRICULEUSERCREATION,
                        TB.MATRICULEUSERMODIFICATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.FK_IDETAPECIRCUIT,
                        TB.CODE_DEMANDE_TABLETRAVAIL,
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LISTE_WKF_STATUS()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.STATUSDEMANDE
                    select new
                    {
                        TB.PK_ID,
                        TB.STATUS
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_CIRCUIT_DEMANDE(Guid pkIDDmd)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COPIE_DMD_CIRCUIT
                    from step in context.ETAPE.Where(stp => stp.PK_ID == TB.FK_IDETAPE)
                    where TB.FK_IDDEMANDE == pkIDDmd
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_RWORKFLOWCENTRE,
                        TB.ORDRE,
                        TB.FK_IDETAPE,
                        TB.CONDITION,
                        TB.FROMCONDITION,
                        TB.FK_IDRETAPEWORKFLOWORIGINE,
                        CODEETAPE = step.CODE,
                        LIBELLEETAPE = step.NOM,
                        TB.USEAFFECTATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_CIRCUIT_COPIE_CONDITION(Guid pkIDCopieDmdCircuit)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COPIE_DMD_CONDITIONBRANCHEMENT
                    where TB.FK_IDCOPIE_DMD_CIRCUIT == pkIDCopieDmdCircuit
                    select new
                    {
                        TB.PK_ID,
                        TB.NOM,
                        TB.COLONNENAME,
                        TB.FK_IDETAPEFAUSE,
                        TB.FK_IDETAPEVRAIE,
                        TB.FK_IDCOPIE_DMD_CIRCUIT,
                        TB.OPERATEUR,
                        TB.FK_IDTABLETRAVAIL,
                        TB.VALUE,
                        TB.PEUT_TRANSMETTRE_SI_FAUX
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable LISTE_WKF_CIRCUIT_DEMANDE(string CodeDmd)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COPIE_DMD_CIRCUIT
                    from step in context.ETAPE.Where(stp => stp.PK_ID == TB.FK_IDETAPE)
                    where TB.CODE_DEMANDE == CodeDmd
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_RWORKFLOWCENTRE,
                        TB.ORDRE,
                        TB.FK_IDETAPE,
                        TB.CONDITION,
                        TB.FROMCONDITION,
                        TB.FK_IDRETAPEWORKFLOWORIGINE,
                        CODEETAPE = step.CODE,
                        LIBELLEETAPE = step.NOM,
                        TB.USEAFFECTATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_DASHBOARD_DEMANDE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.VW_DASHBORDDEMANDE                   
                    select new
                    {
                        TB.ALERTE,
                        TB.CONTROLEETAPE,
                        TB.DUREE,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_IDMENU,
                        TB.FK_IDOPERATION,
                        TB.FK_IDWORKFLOW,
                        TB.IDCIRCUIT,
                        TB.IDETAPE,
                        TB.NOM,
                        TB.NOMBREDEMANDE,
                        TB.NOMOPERATION,
                        TB.ORDRE,
                        TB.WORKFLOWNAME,
                        TB.IS_TRAITEMENT_LOT,
                        TB.FK_IDDEMANDE,
                        TB.STATUTDEMANDE
                       

                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_VW_JOURNAL_DEMANDE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.VW_JOURNAL_DEMANDE
                    join dm in context.DEMANDE on TB.CODE_DEMANDE_TABLETRAVAIL equals dm.NUMDEM
                    join dcli in context.DCLIENT on dm.NUMDEM equals dcli.NUMDEM into _Dclient
                    from v in _Dclient.DefaultIfEmpty()
                    where (dm.ISSUPPRIME == null || dm.ISSUPPRIME == false)
                    select new
                    {
                        TB.ALERTE,
                        TB.CODE,
                        TB.CONTROLEETAPE,
                        TB.DATECREATION,
                        TB.DATEDERNIEREMODIFICATION,
                        TB.DUREE,
                        TB.ETAPEPRECEDENTE,
                        TB.FK_IDCENTRE,
                        TB.FK_IDETAPE,
                        TB.FK_IDETAPEACTUELLE,
                        TB.FK_IDGROUPEVALIDATIOIN,
                        TB.FK_IDMENU,
                        TB.FK_IDOPERATION,
                        TB.FK_IDSTATUS,
                        TB.FK_IDWORKFLOW,
                        TB.IDCIRCUIT,
                        TB.IDETAPE,
                        TB.NOM,
                        TB.MATRICULEUSERCREATION,
                        TB.ORDRE,
                        TB.RAFFECTATIONETAPE,
                        TB.CODECENTRE,
                        TB.CODESITE,
                        TB.ALLCENTRE,
                        TB.IDCENTRE,
                        TB.IDSITE,
                        TB.LIBELLESITE,
                        TB.LIBELLECENTRE,
                        TB.FK_IDTABLETRAVAIL,
                        TB.FK_IDLIGNETABLETRAVAIL,
                        TB.CODE_DEMANDE_TABLETRAVAIL,
                        TB.ISMODIFICATION ,
                        v.NOMABON 
                        
                    };
                    IEnumerable<object> query1 =
                   from TB in context.VW_JOURNAL_DEMANDE
                   where !context.DEMANDE.Any(t=>t.NUMDEM == TB.CODE_DEMANDE_TABLETRAVAIL )

                   select new
                   {
                       TB.ALERTE,
                       TB.CODE,
                       TB.CONTROLEETAPE,
                       TB.DATECREATION,
                       TB.DATEDERNIEREMODIFICATION,
                       TB.DUREE,
                       TB.ETAPEPRECEDENTE,
                       TB.FK_IDCENTRE,
                       TB.FK_IDETAPE,
                       TB.FK_IDETAPEACTUELLE,
                       TB.FK_IDGROUPEVALIDATIOIN,
                       TB.FK_IDMENU,
                       TB.FK_IDOPERATION,
                       TB.FK_IDSTATUS,
                       TB.FK_IDWORKFLOW,
                       TB.IDCIRCUIT,
                       TB.IDETAPE,
                       TB.NOM,
                       TB.MATRICULEUSERCREATION,
                       TB.ORDRE,
                       TB.RAFFECTATIONETAPE,
                       TB.CODECENTRE,
                       TB.CODESITE,
                       TB.ALLCENTRE,
                       TB.IDCENTRE,
                       TB.IDSITE,
                       TB.LIBELLESITE,
                       TB.LIBELLECENTRE,
                       TB.FK_IDTABLETRAVAIL,
                       TB.FK_IDLIGNETABLETRAVAIL,
                       TB.CODE_DEMANDE_TABLETRAVAIL,
                       TB.ISMODIFICATION,
                       NOMABON = ""

                   };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_JOURNAL_DEMANDE_WORKFLOW(string codeDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.JOURNAL_DEMANDE_WORKFLOW
                    where TB.CODE_DEMANDE == codeDemande
                    select new
                    {
                        TB.CODE_DEMANDE,
                        TB.DATEACTION,
                        TB.FK_IDDEMANDE,
                        TB.LIBELLEACTION,
                        TB.MATRICULEUSERACTION,
                        TB.OBSERVATIONS,
                        TB.PK_ID
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_VW_OPERATION_FORMULAIRE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.VW_OPERATION_FORMULAIRE
                    select new
                    {
                        TB.PK_ID,
                        TB.CODE,
                        TB.NOM,
                        TB.DESCRIPTION,
                        TB.FK_ID_PARENTOPERATION,
                        TB.FK_ID_PRODUIT,
                        TB.FK_IDFORMULAIRE,
                        TB.FORMULAIRE,
                        TB.FULLNAMECONTROLE,
                        TB.CREATIONDEMANDE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_AFFECTATION_DEMANDE_TRAITEMENT()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.AFFECTATIONDEMANDEUSER
                    select new
                    {
                        TB.PK_ID,
                        TB.CODEDEMANDE,
                        TB.CENTREID,
                        TB.FK_IDETAPE,
                        TB.MATRICULEUSER,
                        TB.OPERATIONID,
                        TB.WORKFLOWID,
                        TB.FK_IDETAPEFROM,
                        TB.MATRICULEUSERCREATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable LISTE_WKF_AFFECTATION_DEMANDE_TRAITEMENT(string Matricule, int IdEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.AFFECTATIONDEMANDEUSER
                    where TB.MATRICULEUSER == Matricule && TB.FK_IDETAPE == IdEtape
                    select new
                    {
                        TB.PK_ID,
                        TB.CODEDEMANDE,
                        TB.CENTREID,
                        TB.FK_IDETAPE,
                        TB.MATRICULEUSER,
                        TB.OPERATIONID,
                        TB.WORKFLOWID,
                        TB.FK_IDETAPEFROM,
                        TB.MATRICULEUSERCREATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public static DataTable LISTE_WKF_AFFECTATION_ETAPEUSER(List<int> LstIdDemande, string Matricule)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.AFFECTATIONDEMANDEUSER
                    join x in  context.VW_JOURNAL_DEMANDE on TB.CODEDEMANDE equals x.CODE_DEMANDE_TABLETRAVAIL 
                    where LstIdDemande.Contains(TB.FK_IDDEMANDE.Value) && TB.MATRICULEUSER == Matricule  
                    select new
                    {
                        x.ALERTE,
                        x.CODE,
                        x.CONTROLEETAPE,
                        x.DATECREATION,
                        x.DATEDERNIEREMODIFICATION,
                        x.DUREE,
                        x.ETAPEPRECEDENTE,
                        x.FK_IDCENTRE,
                        x.FK_IDETAPE,
                        x.FK_IDETAPEACTUELLE,
                        x.FK_IDGROUPEVALIDATIOIN,
                        x.FK_IDMENU,
                        x.FK_IDOPERATION,
                        x.FK_IDSTATUS,
                        x.FK_IDWORKFLOW,
                        x.IDCIRCUIT,
                        x.IDETAPE,
                        x.NOM,
                        x.MATRICULEUSERCREATION,
                        x.ORDRE,
                        x.RAFFECTATIONETAPE,
                        x.CODECENTRE,
                        x.CODESITE,
                        x.ALLCENTRE,
                        x.IDCENTRE,
                        x.IDSITE,
                        x.LIBELLESITE,
                        x.LIBELLECENTRE,
                        x.FK_IDTABLETRAVAIL,
                        x.FK_IDLIGNETABLETRAVAIL,
                        x.CODE_DEMANDE_TABLETRAVAIL,
                        x.ISMODIFICATION
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public static DataTable LISTE_WKF_AFFECTATION_ETAPEUSER(int IdEtape, string Matricule)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query =
                   from TB in context.AFFECTATIONDEMANDEUSER
                   join x in context.VW_JOURNAL_DEMANDE on TB.CODEDEMANDE  equals x.CODE_DEMANDE_TABLETRAVAIL
                   where x.FK_IDETAPEACTUELLE ==IdEtape   && TB.MATRICULEUSER == Matricule
                   select new
                   {
                       x.ALERTE,
                       x.CODE,
                       x.CONTROLEETAPE,
                       x.DATECREATION,
                       x.DATEDERNIEREMODIFICATION,
                       x.DUREE,
                       x.ETAPEPRECEDENTE,
                       x.FK_IDCENTRE,
                       x.FK_IDETAPE,
                       x.FK_IDETAPEACTUELLE,
                       x.FK_IDGROUPEVALIDATIOIN,
                       x.FK_IDMENU,
                       x.FK_IDOPERATION,
                       x.FK_IDSTATUS,
                       x.FK_IDWORKFLOW,
                       x.IDCIRCUIT,
                       x.IDETAPE,
                       x.NOM,
                       x.MATRICULEUSERCREATION,
                       x.ORDRE,
                       x.RAFFECTATIONETAPE,
                       x.CODECENTRE,
                       x.CODESITE,
                       x.ALLCENTRE,
                       x.IDCENTRE,
                       x.IDSITE,
                       x.LIBELLESITE,
                       x.LIBELLECENTRE,
                       x.FK_IDTABLETRAVAIL,
                       x.FK_IDLIGNETABLETRAVAIL,
                       x.CODE_DEMANDE_TABLETRAVAIL,
                       x.ISMODIFICATION
                   };
                   return Galatee.Tools.Utility.ListToDataTable<object>(query);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public static DataTable LISTE_WKF_VW_JOURNAL_DEMANDE(List<int> LstIdDemande, string Matricule)
       {
           try
           {
               using (galadbEntities context = new galadbEntities())
               {
                   IEnumerable<object> query =
                   from TB in context.VW_JOURNAL_DEMANDE
                   join dm in context.DEMANDE on TB.CODE_DEMANDE_TABLETRAVAIL equals dm.NUMDEM
                   join dcli in context.DCLIENT on dm.NUMDEM equals dcli.NUMDEM into _Dclient
                   from v in _Dclient.DefaultIfEmpty()
                   where LstIdDemande.Contains(dm.PK_ID) && 
                   !context.AFFECTATIONDEMANDEUSER.Any(p => LstIdDemande.Contains(p.FK_IDDEMANDE.Value) && p.MATRICULEUSER == Matricule)
                   select new
                   {
                       TB.ALERTE,
                       TB.CODE,
                       TB.CONTROLEETAPE,
                       TB.DATECREATION,
                       TB.DATEDERNIEREMODIFICATION,
                       TB.DUREE,
                       TB.ETAPEPRECEDENTE,
                       TB.FK_IDCENTRE,
                       TB.FK_IDETAPE,
                       TB.FK_IDETAPEACTUELLE,
                       TB.FK_IDGROUPEVALIDATIOIN,
                       TB.FK_IDMENU,
                       TB.FK_IDOPERATION,
                       TB.FK_IDSTATUS,
                       TB.FK_IDWORKFLOW,
                       TB.IDCIRCUIT,
                       TB.IDETAPE,
                       TB.NOM,
                       TB.MATRICULEUSERCREATION,
                       TB.ORDRE,
                       TB.RAFFECTATIONETAPE,
                       TB.CODECENTRE,
                       TB.CODESITE,
                       TB.ALLCENTRE,
                       TB.IDCENTRE,
                       TB.IDSITE,
                       TB.LIBELLESITE,
                       TB.LIBELLECENTRE,
                       TB.FK_IDTABLETRAVAIL,
                       TB.FK_IDLIGNETABLETRAVAIL,
                       TB.CODE_DEMANDE_TABLETRAVAIL,
                       TB.ISMODIFICATION,
                       v.NOMABON 
                   };
                   IEnumerable<object> query1 =
                      from TB in context.VW_JOURNAL_DEMANDE
                      join dm in context.DEMANDE on TB.CODE_DEMANDE_TABLETRAVAIL equals dm.NUMDEM
                      where LstIdDemande.Contains(dm.PK_ID) &&
                      !context.AFFECTATIONDEMANDEUSER.Any(p => LstIdDemande.Contains(p.FK_IDDEMANDE.Value) && p.MATRICULEUSER == Matricule) && 
                      !context.DEMANDE.Any(p => p.NUMDEM  == TB.CODE_DEMANDE_TABLETRAVAIL ) 
                      select new
                      {
                          TB.ALERTE,
                          TB.CODE,
                          TB.CONTROLEETAPE,
                          TB.DATECREATION,
                          TB.DATEDERNIEREMODIFICATION,
                          TB.DUREE,
                          TB.ETAPEPRECEDENTE,
                          TB.FK_IDCENTRE,
                          TB.FK_IDETAPE,
                          TB.FK_IDETAPEACTUELLE,
                          TB.FK_IDGROUPEVALIDATIOIN,
                          TB.FK_IDMENU,
                          TB.FK_IDOPERATION,
                          TB.FK_IDSTATUS,
                          TB.FK_IDWORKFLOW,
                          TB.IDCIRCUIT,
                          TB.IDETAPE,
                          TB.NOM,
                          TB.MATRICULEUSERCREATION,
                          TB.ORDRE,
                          TB.RAFFECTATIONETAPE,
                          TB.CODECENTRE,
                          TB.CODESITE,
                          TB.ALLCENTRE,
                          TB.IDCENTRE,
                          TB.IDSITE,
                          TB.LIBELLESITE,
                          TB.LIBELLECENTRE,
                          TB.FK_IDTABLETRAVAIL,
                          TB.FK_IDLIGNETABLETRAVAIL,
                          TB.CODE_DEMANDE_TABLETRAVAIL,
                          TB.ISMODIFICATION,
                          NOMABON =""
                      };
                   return Galatee.Tools.Utility.ListToDataTable<object>(query.Union(query1 ));
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
        

        public static DataTable LISTE_COMMENTAIRE_REJET()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COMMENTAIRE_REJET
                    select new
                    {
                        TB.PK_ID,
                        TB.CODEDEMANDE,
                        TB.COMMENTAIRE,
                        TB.DATECOMMENTAIRE,
                        TB.FK_IDDEMANDE,
                        TB.PIECE_JOINTE,
                        TB.UTILISATEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_COMMENTAIRE_REJET(string  NumDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COMMENTAIRE_REJET
                    join us in context.ADMUTILISATEUR on TB.UTILISATEUR equals us.MATRICULE 
                    where TB.CODEDEMANDE == NumDemande
                    select new
                    {
                        TB.PK_ID,
                        TB.CODEDEMANDE,
                        TB.COMMENTAIRE,
                        TB.DATECOMMENTAIRE,
                        TB.FK_IDDEMANDE,
                        TB.PIECE_JOINTE,
                        TB.UTILISATEUR,
                        NOMUTILISATEUR= us.LIBELLE ,
                        TB.FK_IDETAPE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_COMMENTAIRE_REJET(Guid pkIDDemande)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.COMMENTAIRE_REJET
                    where TB.FK_IDDEMANDE == pkIDDemande
                    select new
                    {
                        TB.PK_ID,
                        TB.CODEDEMANDE,
                        TB.COMMENTAIRE,
                        TB.DATECOMMENTAIRE,
                        TB.FK_IDDEMANDE,
                        TB.PIECE_JOINTE,
                        TB.UTILISATEUR
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_WKF_DEMANDEWORKFLOW_PAR_ID_TABLETRAVAIL(int pkidTableTravail, int fkidEtape)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    string id = pkidTableTravail.ToString();
                    IEnumerable<object> query =
                        from dmd in context.DEMANDE_WORFKLOW
                        where dmd.FK_IDLIGNETABLETRAVAIL == id && dmd.FK_IDETAPEACTUELLE == fkidEtape
                        select new
                        {
                            dmd.PK_ID,
                            dmd.ALLCENTRE,
                            dmd.CODE,
                            dmd.DATECREATION,
                            dmd.FK_IDCENTRE,
                            dmd.FK_IDETAPEACTUELLE,
                            dmd.FK_IDETAPEPRECEDENTE,
                            dmd.FK_IDETAPESUIVANTE,
                            dmd.FK_IDLIGNETABLETRAVAIL,
                            dmd.FK_IDOPERATION,
                            dmd.FK_IDRWORKLOW,
                            dmd.FK_IDSTATUS,
                            dmd.FK_IDTABLETRAVAIL,
                            dmd.FK_IDWORKFLOW,
                            dmd.MATRICULEUSERCREATION,
                            dmd.MATRICULEUSERMODIFICATION,
                            dmd.DATEDERNIEREMODIFICATION,
                            dmd.FK_IDETAPECIRCUIT,
                            dmd.CODE_DEMANDE_TABLETRAVAIL
                        };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_RENVOI_REJET()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RENVOI_REJET
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDETAPE,
                        TB.FK_IDRAFFECTATION,
                        TB.FK_IDETAPEACTUELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_RENVOI_REJET(int FkIDEtapeActuelle)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RENVOI_REJET
                    where TB.FK_IDETAPEACTUELLE == FkIDEtapeActuelle
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDETAPE,
                        TB.FK_IDRAFFECTATION,
                        TB.FK_IDETAPEACTUELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable LISTE_RENVOI_REJET(Guid RaffID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                    from TB in context.RENVOI_REJET
                    where TB.FK_IDRAFFECTATION == RaffID
                    select new
                    {
                        TB.PK_ID,
                        TB.FK_IDETAPE,
                        TB.FK_IDRAFFECTATION,
                        TB.FK_IDETAPEACTUELLE
                    };
                    return Galatee.Tools.Utility.ListToDataTable<object>(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}
