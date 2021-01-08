using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Galatee.Structure;

namespace Galatee.Entity.Model
{
   public class ReclamationProcedure
    {
        public static DataTable RetourneListeModeReception()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _SousControle = context.MODERECEPTION;

                    query =
                    from p in _SousControle


                    select new
                    {
                        p.PK_ID,
                        p.LIBELLE

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeTypeReclamation()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RCL_TypeReclamation = context.RCL_TypeReclamation;

                    query =
                    from p in _RCL_TypeReclamation


                    select new
                    {
                        p.PK_ID,
                        p.Code ,
                        p.Libelle

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeValidation()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RCL_Validation = context.RCL_Validation;

                    query =
                    from p in _RCL_Validation


                    select new
                    {
                        PK_ID= p.id ,
                        p.Code ,
                        p.Libelle

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeReclamationbyDemande(int fk_IDDEMANDE)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RCL_Reclamation = context.RCL_Reclamation;
                    

                    query =
                    from p in _RCL_Reclamation
                    where
                       p.FK_IDDEMANDE == fk_IDDEMANDE

                    select new
                    {
                        p.CENTRE,
                        p.  PK_ID ,
                        p. Ordre ,
                        p. NumeroReclamation ,
                        p. Fk_IdTypeReclamation ,
                        p. NomClient ,
                        p.Prenoms,
                        p. Fk_IdClient ,
                        p. DateOuverture ,
                        p. DateRdv ,
                        p. AgentEmetteur ,
                        p. AgentRecepteur ,
                        p. AgentValidation ,
                        p. DateTransmission ,
                        p. DateRetourSouhaite ,
                        p. DateRetour ,
                        p. Observation ,
                        p. Fk_IdModeReception ,
                        p. Adresse ,
                        p. Email ,
                        p. NumeroTelephonePortable ,
                        p. NumeroTelephoneFixe ,
                        p. ObjetReclamation ,
                        p. ActionMenees ,
                        p. Fk_IdStatutReclamation ,
                        p. MotifReprise ,
                        p. NonConformite ,
                        p. LettreReponse ,
                        p. DateValidation ,
                        p. MotifRejet ,
                        p. Fk_IdCentre ,
                        p. Client ,
                        p. FK_IDDEMANDE ,
                        p. FK_IDCOMMUNE ,
                        p. FK_IDQUARTIER ,
                        p. FK_IDRUE ,
                        p. COMMUNE ,
                        p. FK_IDSECTEUR ,
                        p. SECTEUR ,
                        p. QUARTIER,
                        LIBELLETYPERECLAMATION = p.RCL_TypeReclamation.Libelle,
                        p.Fk_IdValidation
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeReclamation()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _RCL_Reclamation = context.RCL_Reclamation;

                    query =
                    from p in _RCL_Reclamation
                    select new
                    {
                        p.PK_ID,
                        p.Ordre,
                        p.NumeroReclamation,
                        p.Fk_IdTypeReclamation,
                        p.NomClient,
                        p.Fk_IdClient,
                        p.DateOuverture,
                        p.DateTransmission,
                        p.AgentEmetteur,
                        p.AgentRecepteur,
                        p.AgentValidation,
                        p.DateRdv,
                        p.DateRetourSouhaite,
                        p.DateRetour,
                        p.Observation,
                        p.Fk_IdModeReception,
                        p.Adresse,
                        p.Email,
                        p.NumeroTelephonePortable,
                        p.NumeroTelephoneFixe,
                        p.ObjetReclamation,
                        p.ActionMenees,
                        p.Fk_IdStatutReclamation,
                        p.MotifReprise,
                        p.NonConformite,
                        p.LettreReponse,
                        p.DateValidation,
                        p.MotifRejet,
                        p.Fk_IdCentre,

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void InsertionInitReclamation(CsDemandeReclamation pDemandeRecl, galadbEntities pContext)
        {

            try
            {
                galadbEntities contextinter = new galadbEntities();
                RCL_Reclamation _Reclamation = new RCL_Reclamation();

                List<DOCUMENTSCANNE> lstDocumentScan = new List<DOCUMENTSCANNE>();
                if (pDemandeRecl.DonneDeDemande != null && pDemandeRecl.DonneDeDemande.Count != 0)
                    lstDocumentScan = Entities.ConvertObject<DOCUMENTSCANNE, ObjDOCUMENTSCANNE>(pDemandeRecl.DonneDeDemande);

                
                if (pDemandeRecl.ReclamationRcl != null)
                    _Reclamation = Entities.ConvertObject<RCL_Reclamation, CsReclamationRcl>(pDemandeRecl.ReclamationRcl);

                DEMANDE _DEMANDE = new DEMANDE();
                if (pDemandeRecl.LaDemande != null)
                    _DEMANDE = Entities.ConvertObject<DEMANDE, CsDemandeBase>(pDemandeRecl.LaDemande);

                foreach (DOCUMENTSCANNE item in lstDocumentScan)
                {
                    if (pContext.DOCUMENTSCANNE.FirstOrDefault(d => d.PK_ID == item.PK_ID) == null)
                    {
                        item.PK_ID = Guid.NewGuid();
                        _DEMANDE.DOCUMENTSCANNE.Add(item);
                    }
                }

                //DCLIENT _Dclient= new DCLIENT();
                //if (pDemandeRecl.LeClient  != null)
                //    _Dclient = Entities.ConvertObject<DCLIENT, CsClient>(pDemandeRecl.LeClient);

                //_DEMANDE.DCLIENT.Add(_Dclient);

                if (_DEMANDE.PK_ID == 0)
                {
                    _DEMANDE.RCL_Reclamation.Add(_Reclamation);
                    Entities.InsertEntity<DEMANDE>(_DEMANDE, pContext);
                }
                else
                {
                    Entities.UpdateEntity<DEMANDE>(_DEMANDE, pContext);
                    Entities.UpdateEntity<RCL_Reclamation>(_Reclamation, pContext);
                }
                 
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }
        public static void ValiderReclamation(CsDemandeReclamation pDemandeRecl, galadbEntities pContext)
        {

            try
            {
                galadbEntities contextinter = new galadbEntities();
                RCL_Reclamation _Reclamation = new RCL_Reclamation();
                if (pDemandeRecl.ReclamationRcl != null)
                    _Reclamation = Entities.ConvertObject<RCL_Reclamation, CsReclamationRcl>(pDemandeRecl.ReclamationRcl);
                Entities.UpdateEntity<RCL_Reclamation>(_Reclamation, pContext);
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                //var errorMessages = ex.EntityValidationErrors
                //        .SelectMany(x => x.ValidationErrors)
                //        .Select(x => x.ErrorMessage);

                //// Join the list to a single string.
                //var fullErrorMessage = string.Join("; ", errorMessages);

                //// Combine the original exception message with the new one.
                //var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                ////return exceptionMessage;
                //// Throw a new DbEntityValidationException with the improved exception message.
                //throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                throw ex;

            }
        }

        public static DataTable RetourneReclamation(int fk_idcentre, string pCentre, string pClient, string ordre, string Numdemande)
        {
            try
            {
                // string ordreMax = string.Empty;
                using (galadbEntities context = new galadbEntities())
                {
                    var Reclamation = context.RCL_Reclamation;
                  
                        var query = (from _Reclamation in Reclamation
                                     where
                                     (_Reclamation.Fk_IdCentre == fk_idcentre)
                                     && (_Reclamation.NumeroReclamation == Numdemande )

                                     select new
                                     {
                                         _Reclamation.Ordre,
                                         _Reclamation.NomClient,
                                         _Reclamation.NumeroReclamation,
                                         _Reclamation.Client,
                                         _Reclamation.CENTRE,
                                         _Reclamation.Fk_IdCentre,
                                         _Reclamation.NumeroTelephoneFixe,
                                         _Reclamation.NumeroTelephonePortable,
                                         _Reclamation.Adresse,
                                         _Reclamation.Email,
                                         _Reclamation.DateOuverture,
                                         _Reclamation.DateRetourSouhaite,
                                         _Reclamation.DateRdv,
                                         _Reclamation.ObjetReclamation,
                                         _Reclamation.Observation,
                                         _Reclamation.AgentEmetteur,

                                     });

                        //if (query != null)
                        //    ordreMax = query.Max(cl => cl.ORDRE);
                        return Galatee.Tools.Utility.ListToDataTable(query);
                  

                        

                }
                //  return ordreMax;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
