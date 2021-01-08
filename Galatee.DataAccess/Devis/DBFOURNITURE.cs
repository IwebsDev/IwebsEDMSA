using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Galatee.Structure;
using System.Data.SqlClient;
using System.Data;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBFOURNITURE : DBBase
    {

        public static List<ObjFOURNITURE> SelectFournituresByCodeProduitByIdTypeDevis(int pProduitId, int pIdTypeDevis, string pDiametre)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjFOURNITURE>(DevisProcedures.DEVIS_FOURNITURE_RETOURNEByCodeProduitByIdTypeDevisDiametre(pIdTypeDevis,pProduitId,pDiametre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjFOURNITURE SelectFournituresByNumFourniture(int pNumFourniture)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjFOURNITURE>(DevisProcedures.DEVIS_FOURNITURE_RETOURNEByNumFourniture(pNumFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjFOURNITURE> GetFournitureByIdTypeDevis(System.Int32 idTypeDevis)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjFOURNITURE>(DevisProcedures.DEVIS_FOURNITURE_RETOURNEByIdTypeDevis(idTypeDevis));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ObjFOURNITURE GetFournitureByNumFourniture(System.Int32 numFourniture)
        {
            try
            {
                return Entities.GetEntityFromQuery<ObjFOURNITURE>(DevisProcedures.DEVIS_FOURNITURE_RETOURNEByNumFourniture(numFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjFOURNITURE> GetAllFourniture()
        {
            try
            {
                DataTable dtf = DevisProcedures.DEVIS_FOURNITURE_RETOURNE();
                return Entities.GetEntityListFromQuery<ObjFOURNITURE>(dtf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteFourniture(ObjFOURNITURE pFourniture)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(pFourniture));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteFourniture(List<ObjFOURNITURE> entityCollection)
        {

            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertFourniture(ObjFOURNITURE entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertFourniture(List<ObjFOURNITURE> entityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateFourniture(ObjFOURNITURE entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateFourniture(List<ObjFOURNITURE> entityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.FOURNITURE>(Entities.ConvertObject<Galatee.Entity.Model.FOURNITURE, ObjFOURNITURE>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
