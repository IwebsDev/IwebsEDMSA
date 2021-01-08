using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Galatee.Structure;

namespace Galatee.Entity.Model
{
    public class InterfaceProcedures
    {

        //#region Interface eclipse Sylla 17/08/2016

        //public static bool EnvoieDonneesClientAEclipse(CsClient client, CsAbon abon, CsAg ag, CsBrt brt, CsCompteur compteur, bool Modification)
        //{
        //    try
        //    {
        //        using (EclipseEntities Context = new EclipseEntities())
        //        {
        //            //Insertion de données dans eclipse
        //            #region LEGAL_ENTITY

        //            LEGAL_ENTITY LEGAL_ENTITY = InsertLegal_Entity(client, ag, Context);
        //            #endregion

        //            #region AGR
        //            AGR AGR = InsertAgr(Context, ref LEGAL_ENTITY);
        //            #endregion

        //            #region LOCATION
        //            LOCATION LOCATION = InsertLocation(client, Context, LEGAL_ENTITY);
        //            #endregion

        //            #region RDP
        //            InsertRdp(brt, Context, AGR, LOCATION);

        //            #endregion

        //            #region METER
        //            InsertMeter(Context);
        //            #endregion

        //        }
        //        return true;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        //return exceptionMessage;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseAP_DP(CsAbon abon, CsCompteur Nouveaucompteur, CsCompteur Aciencompteur, bool ChangementCompteur)
        //{
        //    try
        //    {
        //        using (EclipseEntities Context = new EclipseEntities())
        //        {

        //            //Insertion de données dans eclipse

        //            #region METER
        //            METER METER = new METER();
        //            METER NEWMETER = new METER();
        //            //METER = Context.METER.FirstOrDefault(m => m.MSNO == Aciencompteur.NUMERO);
        //            //NEWMETER = Context.METER.FirstOrDefault(m => m.MSNO == Nouveaucompteur.NUMERO);
        //            #region Code finale
        //            if (!ChangementCompteur)
        //            {
        //                //METER.MRL_ID=abon.PUISSANCE;
        //                //METER.MOD_TS=Aciencompteur.DATEMODIFICATION;
        //                //METER.CHANGE_DATE=Aciencompteur.DATEMODIFICATION;

        //            }
        //            else
        //            {
        //                //Retourner l’ancien compteur 
        //                //METER.METER_STATUS_ID=2;
        //                //METER.STATUS_CATEGORY_ID=102;
        //                //METER.LOCATION_ID=54840823400;
        //                //METER.RDP_ID= NULL;
        //                //METER.MOD_TS=Aciencompteur.DATEMODIFICATION;
        //                //METER.CHANGE_DATE=Aciencompteur.DATEMODIFICATION;

        //                //Assigner le nouveau compteur 
        //                //NEWMETER.MRL_ID= 50503400;
        //                //NEWMETER.METER_STATUS_ID=1;
        //                //NEWMETER.STATUS_CATEGORY_ID=101;
        //                //NEWMETER.LOCATION_ID=YY00123456;
        //                //NEWMETER.RDP_ID= YY00123456;
        //                //NEWMETER.INST_DATE=Nouveaucompteur.DATEMODIFICATION;
        //                //NEWMETER.MOD_TS=Nouveaucompteur.DATEMODIFICATION;
        //                //NEWMETER.CHANGE_DATE=Nouveaucompteur.DATEMODIFICATION;

        //            }

        //            #endregion

        //            #region Code test


        //            #endregion


        //            Entities.InsertEntity_Eclipse<METER>(METER, Context);
        //            Context.SaveChanges();
        //            #endregion

        //        }
        //        return true;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        //return exceptionMessage;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseResiliation(CsAbon abon, CsCompteur Nouveaucompteur, CsCompteur Aciencompteur, bool ChangementCompteur)
        //{
        //    try
        //    {
        //        using (EclipseEntities Context = new EclipseEntities())
        //        {

        //            //Insertion de données dans eclipse

        //            #region METER
        //            //METER METER = new METER();
        //            //LOCATION LOCATION = Context.LOCATION.FirstOrDefault(l=>l.LOCATION_ID==METER.LOCATION_ID);
        //            //RDP RDP = Context.RDP.FirstOrDefault(r => r.LOCATION_ID == LOCATION.LOCATION_ID);

        //            #region Code finale

        //            ////Retourner le compteur 
        //            //METER.METER_STATUS_ID=2;
        //            //METER.STATUS_CATEGORY_ID=102;
        //            //METER.LOCATION_ID=54840823400;
        //            //METER.RDP_ID= NULL;
        //            //METER.MOD_TS='16/08/2016';
        //            //METER.CHANGE_DATE='16/08/2016';


        //            ////Mise à jour des info d'addresse géo
        //            //LOCATION.LOCATION_STATUS_ID = 3;
        //            //LOCATION.MOD_TS = DateTime.Now.Date;

        //            ////Mise à jur des info de branchement
        //            //RDP.RDP_STATUS_ID = 3;
        //            //RDP.MOD_TS = DateTime.Now.Date;


        //            #endregion

        //            #region Code test


        //            #endregion

        //            //List<METER> List_METER = new List<METER>();
        //            //List_METER.Add(METER);
        //            //Entities.UpdateEntity_Eclipse<METER>(List_METER, Context);

        //            //List<LOCATION> List_LOCATION = new List<LOCATION>();
        //            //List_LOCATION.Add(LOCATION);
        //            //Entities.UpdateEntity_Eclipse<LOCATION>(List_LOCATION, Context);


        //            //List<RDP> List_RDP = new List<RDP>();
        //            //List_RDP.Add(RDP);
        //            //Entities.UpdateEntity_Eclipse<RDP>(List_RDP, Context);

        //            Context.SaveChanges();
        //            #endregion

        //        }
        //        return true;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        //return exceptionMessage;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //    }
        //}
        //public static bool EnvoieDonneesClientAEclipseMutation(CsClient Nouveauclient, CsClient Ancienclient, CsAbon abon, CsAg ag, CsBrt brt, CsCompteur compteur, bool Modification)
        //{
        //    try
        //    {
        //        using (EclipseEntities Context = new EclipseEntities())
        //        {
        //            //Insertion de données dans eclipse
        //            #region LEGAL_ENTITY

        //            LEGAL_ENTITY LEGAL_ENTITY = InsertLegal_Entity(Nouveauclient, ag, Context);
        //            #endregion

        //            #region AGR
        //            AGR AGR = InsertAgr(Context, ref LEGAL_ENTITY);
        //            #endregion

        //            #region LOCATION
        //            LOCATION LOCATION = InsertLocation(Nouveauclient, Context, LEGAL_ENTITY);
        //            #endregion

        //            LEGAL_ENTITY ANCIEN_LEGAL_ENTITY = Context.LEGAL_ENTITY.FirstOrDefault(l => l.LEGAL_ENTITY_REF == Ancienclient.REFCLIENT);
        //            ANCIEN_LEGAL_ENTITY.LEGAL_ENTITY_STATUS_ID = 0;

        //            List<LEGAL_ENTITY> LISTE_ANCIEN_LEGAL_ENTITY = new List<Model.LEGAL_ENTITY>();
        //            LISTE_ANCIEN_LEGAL_ENTITY.Add(ANCIEN_LEGAL_ENTITY);
        //            Entities.UpdateEntity_Eclipse<LEGAL_ENTITY>(LISTE_ANCIEN_LEGAL_ENTITY, Context);

        //            AGR AGR_EXISTANT = Context.AGR.FirstOrDefault(a => a.LEGAL_ENTITY_ID == ANCIEN_LEGAL_ENTITY.LEGAL_ENTITY_ID);
        //            AGR_EXISTANT.AGR_STATUS_ID = 0;
        //            List<AGR> LISTE_AGR = new List<Model.AGR>();
        //            LISTE_AGR.Add(AGR_EXISTANT);
        //            Entities.UpdateEntity_Eclipse<AGR>(LISTE_AGR, Context);

        //            Context.SaveChanges();

        //        }
        //        return true;
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        //return exceptionMessage;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //    }
        //}

        ////private static void InsertMeter(EclipseEntities Context)
        ////{
        ////    METER METER = new METER();

        ////    #region Code finale


        ////    #endregion

        ////    #region Code test

        ////    #endregion


        ////    Entities.InsertEntity_Eclipse<METER>(METER, Context);
        ////    Context.SaveChanges();
        ////}

        ////private static void InsertRdp(CsBrt brt, EclipseEntities Context, AGR AGR, LOCATION LOCATION)
        ////{
        ////    RDP RDP = new RDP();

        ////    #region Code finale


        ////    #endregion

        ////    #region Code test
        ////    RDP.RDP_ID = LOCATION.LOCATION_ID;
        ////    RDP.RDP_STATUS_ID = 1;
        ////    RDP.RES_ID = 1;
        ////    RDP.UTIL_ID = 340;
        ////    RDP.RDP_DISP_METHOD_ID = 1;
        ////    RDP.RESP_OPERATOR_ID = 123456789;
        ////    RDP.TRF_ID = 10503400;
        ////    RDP.SG_ID = 20573400;
        ////    RDP.LOCATION_ID = LOCATION.LOCATION_ID;
        ////    RDP.RDP_MEASURE_TYPE_ID = 1;
        ////    RDP.MRL_ID = LOCATION.LOCATION_ID;
        ////    RDP.RDP_TYPE_ID = 1;
        ////    RDP.RDP_REF = "RDP" + LOCATION.LOCATION_ID.ToString();
        ////    //decimal LATITUDE = -1;
        ////    //if (decimal.TryParse(brt.LATITUDE, out LATITUDE))
        ////    //    RDP.GPS_LATITUDE = LATITUDE;
        ////    RDP.GPS_LATITUDE = 51236479;
        ////    //decimal LONGITUDE = -1;
        ////    //if (decimal.TryParse(brt.LONGITUDE, out LONGITUDE))
        ////    //    RDP.GPS_LONGITUDE = LONGITUDE;
        ////    RDP.GPS_LONGITUDE = 98756321;
        ////    RDP.INST_DATE = DateTime.Now;
        ////    RDP.AGR_ID = AGR.AGR_ID;
        ////    //RDP.REP_STDATE = brt.DRES;
        ////    RDP.REP_STDATE = DateTime.Now.Date;


        ////    #endregion


        ////    Entities.InsertEntity_Eclipse<RDP>(RDP, Context);
        ////    Context.SaveChanges();
        ////    RDP = Context.RDP.OrderByDescending(l => l.RDP_ID).First();
        ////}

        ////private static LOCATION InsertLocation(CsClient client, EclipseEntities Context, LEGAL_ENTITY LEGAL_ENTITY)
        ////{
        ////    LOCATION LOCATION = new LOCATION();

        ////    #region Code finale
        ////    //LOCATION.LOCATION_TYPE_ID = 1;
        ////    //LOCATION.ADDR1 = ag.COMMUNE + "-" + ag.QUARTIER;
        ////    //LOCATION.ADDR2 = ag.PORTE;
        ////    //LOCATION.ADDR3 = ag.RUE;
        ////    //LOCATION.LOCATION_REF = client.TOURNEE + client.ORDTOUR;
        ////    //LOCATION.AREA_ID = client.FK_IDCENTRE;
        ////    //LOCATION.LOCATION_STATUS_ID = 1;
        ////    //LOCATION.OWNER_LEGAL_ENTITY_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        ////    //LOCATION.RESP_OPERATOR_ID = 123456789;

        ////    #endregion

        ////    #region Code test
        ////    LOCATION.LOCATION_TYPE_ID = 1;
        ////    LOCATION.ADDR1 = LEGAL_ENTITY.ADDR1;
        ////    LOCATION.ADDR2 = LEGAL_ENTITY.ADDR2;
        ////    LOCATION.ADDR3 = LEGAL_ENTITY.ADDR3;
        ////    //LOCATION.LOCATION_REF = client.TOURNEE + client.ORDTOUR;            
        ////    LOCATION.LOCATION_REF = "test";
        ////    //LOCATION.AREA_ID = client.FK_IDCENTRE;
        ////    LOCATION.AREA_ID = 40523400;
        ////    LOCATION.LOCATION_STATUS_ID = 1;
        ////    LOCATION.OWNER_LEGAL_ENTITY_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        ////    LOCATION.RESP_OPERATOR_ID = 123456789;


        ////    #endregion


        ////    Entities.InsertEntity_Eclipse<LOCATION>(LOCATION, Context);
        ////    Context.SaveChanges();
        ////    LOCATION = Context.LOCATION.OrderByDescending(l => l.LOCATION_ID).First();

        ////    return LOCATION;
        ////}

        //private static AGR InsertAgr(EclipseEntities Context, ref LEGAL_ENTITY LEGAL_ENTITY)
        //{
        //    AGR AGR = new AGR();
        //    #region Code finale
        //    //AGR.AGR_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        //    //AGR.AGR_TYPE_ID = 1;
        //    //AGR.LEGAL_ENTITY_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        //    //AGR.AREF = "AGR" + LEGAL_ENTITY.LEGAL_ENTITY_REF;
        //    //AGR.AGR_STATUS_ID = LEGAL_ENTITY.LEGAL_ENTITY_STATUS_ID;
        //    //AGR.UTIL_ID = 340;
        //    //AGR.LIABILITY_TYPE_ID = 3;
        //    //AGR.CREDIT_CONTROL_VALUE = 0;
        //    //AGR.CREDIT_CONTROLLED = 0;
        //    //AGR.RESP_OPERATOR_ID = 123456789;
        //    //AGR.CREDIT_CONTROLLED_DATE = null;

        //    #endregion

        //    #region Code test
        //    AGR.AGR_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        //    AGR.AGR_TYPE_ID = 1;
        //    AGR.LEGAL_ENTITY_ID = LEGAL_ENTITY.LEGAL_ENTITY_ID;
        //    AGR.AREF = "AGR" + LEGAL_ENTITY.LEGAL_ENTITY_REF;
        //    AGR.AGR_STATUS_ID = LEGAL_ENTITY.LEGAL_ENTITY_STATUS_ID;
        //    AGR.UTIL_ID = 340;
        //    AGR.LIABILITY_TYPE_ID = 3;
        //    AGR.CREDIT_CONTROL_VALUE = 0;
        //    AGR.CREDIT_CONTROLLED = 0;
        //    AGR.RESP_OPERATOR_ID = 123456789;
        //    AGR.CREDIT_CONTROLLED_DATE = null;

        //    #endregion


        //    Entities.InsertEntity_Eclipse<AGR>(AGR, Context);
        //    Context.SaveChanges();
        //    AGR = Context.AGR.OrderByDescending(l => l.AGR_ID).First();

        //    return AGR;
        //}

        //private static LEGAL_ENTITY InsertLegal_Entity(CsClient client, CsAg ag, EclipseEntities Context)
        //{
        //    LEGAL_ENTITY LEGAL_ENTITY = new LEGAL_ENTITY();

        //    #region Code Finale
        //    //LEGAL_ENTITY.LEGAL_ENTITY_NAME = client.NOMABON;
        //    //LEGAL_ENTITY.ADDR1 = ag.COMMUNE +"-"+ag.QUARTIER;
        //    //LEGAL_ENTITY.ADDR2 = ag.PORTE;
        //    //LEGAL_ENTITY.ADDR3 = ag.RUE;
        //    //LEGAL_ENTITY.LEGAL_ENTITY_REF = client.REFCLIENT;
        //    //LEGAL_ENTITY.TEL = client.TELEPHONE;
        //    //LEGAL_ENTITY.LEGAL_ENTITY_TYPE_ID = 1;
        //    //LEGAL_ENTITY.LEGAL_ENTITY_STATUS_ID = 1;
        //    ////long MATRICULE = -1;
        //    ////if(long.TryParse( user.MATRICULE,out MATRICULE))
        //    ////    LEGAL_ENTITY.RESP_OPERATOR_ID = MATRICULE ;
        //    //LEGAL_ENTITY.RESP_OPERATOR_ID = 123456789;
        //    #endregion

        //    #region Code de test
        //    LEGAL_ENTITY.LEGAL_ENTITY_NAME = "test";
        //    LEGAL_ENTITY.ADDR1 = "test";
        //    LEGAL_ENTITY.ADDR2 = "test";
        //    LEGAL_ENTITY.ADDR3 = "test";
        //    LEGAL_ENTITY.LEGAL_ENTITY_REF = "test";
        //    LEGAL_ENTITY.TEL = "test";
        //    LEGAL_ENTITY.LEGAL_ENTITY_TYPE_ID = 1;
        //    LEGAL_ENTITY.LEGAL_ENTITY_STATUS_ID = 1;
        //    LEGAL_ENTITY.RESP_OPERATOR_ID = 123456789;

        //    #endregion


        //    Entities.InsertEntity_Eclipse<LEGAL_ENTITY>(LEGAL_ENTITY, Context);
        //    Context.SaveChanges();
        //    LEGAL_ENTITY = Context.LEGAL_ENTITY.OrderByDescending(l => l.LEGAL_ENTITY_ID).First();

        //    return LEGAL_ENTITY;
        //}

        //private static void UpdateMeter(CsAbon abon, CsCompteur compteur, METER METER)
        //{
        //    //METER.MRL_ID = abon.PUISSANCE;
        //    //METER.MOD_TS = compteur.DATEMODIFICATION;
        //    //METER.CHANGE_DATE = compteur.DATEMODIFICATION;

        //    //METER.METER_STATUS_ID = 2;
        //    //METER.STATUS_CATEGORY_ID = 102;
        //    //METER.LOCATION_ID = 54840823400;
        //    //METER.RDP_ID = NULL;
        //    //METER.MOD_TS = compteur.DATEMODIFICATION;
        //    //METER.INST_DATE = compteur.DATEMODIFICATION;
        //    //METER.CHANGE_DATE = compteur.DATEMODIFICATION;

        //}

        //#endregion

    }
}
