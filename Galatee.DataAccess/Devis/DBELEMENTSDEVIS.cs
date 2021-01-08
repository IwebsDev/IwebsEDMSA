using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Galatee.Structure;
using Galatee.Entity.Model;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBELEMENTSDEVIS : DBBase
    {
 

        public static bool InsertElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertElementsDevis(List<ObjELEMENTDEVIS> entityCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertElementsDevis(List<ObjELEMENTDEVIS> entityCollection, galadbEntities pCommand)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateOrInsertElementsDevis(List<ObjELEMENTDEVIS> entityCollection, ObjMATRICULE pAgent, galadbEntities pCommand)
        {
            List<ObjELEMENTDEVIS> ListEltDevisForUpdate = new List<ObjELEMENTDEVIS>();
            List<ObjELEMENTDEVIS> ListEltDevisForInsert = new List<ObjELEMENTDEVIS>();
            bool result = false;
            try
            {
                foreach (ObjELEMENTDEVIS eltDevis in entityCollection)
                {
                   var dt = DevisProcedures.DEVIS_ELEMENTDEVIS_RetourneByDevisIdOrdreFournitureId(eltDevis.FK_IDDEMANDE, eltDevis.ORDRE, eltDevis.FK_IDFOURNITURE.Value );
                   if (dt != null && dt.Rows.Count > 0)
                   {
                       ObjELEMENTDEVIS obj = Entities.GetEntityFromQuery<ObjELEMENTDEVIS>(dt);
                       eltDevis.PK_ID = obj.PK_ID;
                       eltDevis.USERMODIFICATION = pAgent.MATRICULE;
                       eltDevis.DATEMODIFICATION = DateTime.Now;
                       ListEltDevisForUpdate.Add(eltDevis);
                   }
                   else
                   {
                       eltDevis.USERCREATION = pAgent.MATRICULE;
                       eltDevis.DATECREATION = DateTime.Now;
                       ListEltDevisForInsert.Add(eltDevis);
                   }
                }
                if (ListEltDevisForInsert.Count > 0)
                   result=  Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection), pCommand);
                if (ListEltDevisForUpdate.Count > 0)
                   result=  Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection), pCommand);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateOrInsertElementsDeDevis(List<ObjELEMENTDEVIS> entityCollection, ObjMATRICULE pAgent)
        {
            List<ObjELEMENTDEVIS> ListEltDevisForUpdate = new List<ObjELEMENTDEVIS>();
            List<ObjELEMENTDEVIS> ListEltDevisForInsert = new List<ObjELEMENTDEVIS>();
            bool result = false;
            try
            {
                foreach (ObjELEMENTDEVIS eltDevis in entityCollection)
                {
                    var dt = DevisProcedures.DEVIS_ELEMENTDEVIS_RetourneByDevisIdOrdreFournitureId(eltDevis.FK_IDDEMANDE , eltDevis.ORDRE, eltDevis.FK_IDFOURNITURE.Value 
                        );
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ObjELEMENTDEVIS obj = Entities.GetEntityFromQuery<ObjELEMENTDEVIS>(dt);
                        eltDevis.PK_ID = obj.PK_ID;
                        eltDevis.USERCREATION = obj.USERCREATION;
                        eltDevis.DATECREATION = obj.DATECREATION;
                        eltDevis.USERMODIFICATION = pAgent.MATRICULE;
                        eltDevis.DATEMODIFICATION = DateTime.Now;
                        ListEltDevisForUpdate.Add(eltDevis);
                    }
                    else
                    {
                        eltDevis.USERCREATION = pAgent.MATRICULE;
                        eltDevis.DATECREATION = DateTime.Now;
                        ListEltDevisForInsert.Add(eltDevis);
                    }
                }
                if (ListEltDevisForInsert.Count > 0)
                    result = Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection));
                if (ListEltDevisForUpdate.Count > 0)
                    result = Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection));
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertElementsDevis(ObjELEMENTDEVIS entity, galadbEntities pCommand)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevis(ObjELEMENTDEVIS entity, galadbEntities pCommand)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevis(List<ObjELEMENTDEVIS> entityCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevis(List<ObjELEMENTDEVIS> entityCollection, galadbEntities pCommand)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entityCollection), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteElementsDevis(ObjELEMENTDEVIS entity)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteElementsDevis(ObjELEMENTDEVIS entity, galadbEntities pCommand)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(entity),pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevisConsomme(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(_lElements));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevisRemisEnStock(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(_lElements));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevisValide(List<ObjELEMENTDEVIS> _lElements)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(_lElements));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevisValide(List<ObjELEMENTDEVIS> _lElements, galadbEntities pCommande)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(_lElements), pCommande);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteElementsDevis(galadbEntities pCommand, ObjELEMENTDEVIS pElementDevis)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(pElementDevis), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteElementsDevis(galadbEntities pCommand, List<ObjELEMENTDEVIS> pListElementDevis)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.ELEMENTDEVIS>(Entities.ConvertObject<Galatee.Entity.Model.ELEMENTDEVIS, ObjELEMENTDEVIS>(pListElementDevis), pCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<ObjELEMENTDEVIS> SelectElementsDevisByDevisId(int pDevisId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(DevisProcedures.DEVIS_ELEMENTDEVIS_SelByDevisById(pDevisId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjELEMENTDEVIS> SelectElementsDevisByDevisId(int pDevisId, int ordre, bool isSummary)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(DevisProcedures.DEVIS_ELEMENTDEVIS_SelByDevisId(pDevisId, ordre, isSummary));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjELEMENTDEVIS> SelectElementsDevisByDevisIdForValidationRemiseStock(int pDevisId, int ordre, bool isSummary)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(DevisProcedures.DEVIS_ELEMENTDEVIS_ValidationRemiseStock_SelByDevisId(pDevisId, ordre, isSummary));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<ObjELEMENTDEVIS> SelectElementsDevisConsommeByDevisId(int pDevisId, int ordre)
        {
            try
            {
                return Entities.GetEntityListFromQuery<ObjELEMENTDEVIS>(DevisProcedures.DEVIS_ELEMENTDEVIS_Consomme_SelByDevisId(pDevisId, ordre));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
