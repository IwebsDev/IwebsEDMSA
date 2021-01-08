
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Galatee.Structure;
using System.ComponentModel;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{

/// <summary>
///	This class is the base repository for the CRUD operations on the Inova.Xxxxxxxxxx.Common.CTAX objects.
/// </summary>
    [DataObject]
    public class DBENTREPRISE 
  {

        public bool Delete(CsEntreprise entity)
      {
          try
          {
              return Entities.DeleteEntity<Galatee.Entity.Model.ENTREPRISE>(Entities.ConvertObject<Galatee.Entity.Model.ENTREPRISE, CsEntreprise>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

        public CsEntreprise GetAll()
        {
          try
          {
              return Entities.GetEntityFromQuery<CsEntreprise>(CommonProcedures.RetourneInformationsEntreprise());
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

        public CsEntreprise GetById(CsEntreprise entity)
        {
          try
          {
              return Entities.GetEntityFromQuery<CsEntreprise>(ParamProcedure.PARAM_ENTREPRISE_RETOURNEById(entity.PK_ID));
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

        public bool Insert(CsEntreprise entity)
        {
          try
          {
              return Entities.InsertEntity<Galatee.Entity.Model.ENTREPRISE>(Entities.ConvertObject<Galatee.Entity.Model.ENTREPRISE, CsEntreprise>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
        }

        public bool Update(CsEntreprise entity)
        {
          try
          {
              return Entities.UpdateEntity<Galatee.Entity.Model.ENTREPRISE>(Entities.ConvertObject<Galatee.Entity.Model.ENTREPRISE, CsEntreprise>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
        }

  }
} 


