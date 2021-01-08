using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using Galatee.Structure;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{
    public class DBSECTEUR /*: Galatee.DataAccess.Parametrage.DbBase*/
    {
        
        public List<CsSecteur> SelectAllSecteur()
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsSecteur>(ParamProcedure.PARAM_SECTEUR_RETOURNE());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CsSecteur> SelectAllSecteurById(int pId)
        {
            try
            {
                return Entities.GetEntityListFromQuery<CsSecteur>(ParamProcedure.PARAM_SECTEUR_RETOURNEById(pId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(CsSecteur pSecteur)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(List<CsSecteur> pSecteurCollection)
        {
            try
            {
                return Entities.DeleteEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(CsSecteur pSecteur)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(List<CsSecteur> pSecteurCollection)
        {
            try
            {
                return Entities.UpdateEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(CsSecteur pSecteur)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteur));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Insert(List<CsSecteur> pSecteurCollection)
        {
            try
            {
                return Entities.InsertEntity<Galatee.Entity.Model.SECTEUR>(Entities.ConvertObject<Galatee.Entity.Model.SECTEUR, CsSecteur>(pSecteurCollection));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
