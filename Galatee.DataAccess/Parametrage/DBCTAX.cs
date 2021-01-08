
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
    public class DBCTAX: Galatee.DataAccess.Parametrage.DbBase

  {

      public bool Delete(CsCtax entity)
      {
          try
          {
              return Entities.DeleteEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

      public bool Delete(List<CsCtax> entityCollection)
      {
          try
          {
              return Entities.DeleteEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entityCollection));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

      public List<CsCtax> GetAll()
      {
          try
          {
              //return Entities.GetEntityListFromQuery<CsCtax>(ParamProcedure.PARAM_CTAX_RETOURNE());
              DB_ParametresGeneraux db = new DB_ParametresGeneraux();
              List<CsCtax> _LstItem = new List<CsCtax>();
              _LstItem = db.RetourneCTax();
              return _LstItem;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public CsCtax GetByCENTRECTAXDEBUTAPPLICATION(CsCtax entity)
      {
          try
          {
              return Entities.GetEntityFromQuery<CsCtax>(ParamProcedure.PARAM_CTAX_RETOURNEById(entity));
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }

      public bool Insert(CsCtax entity)
      {
          try
          {
              return Entities.InsertEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

      public bool Insert(List<CsCtax> entityCollection)
      {
          try
          {
              return Entities.InsertEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entityCollection));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

      public bool Update(CsCtax entity)
      {
          try
          {
              return Entities.UpdateEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entity));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

      public bool Update(List<CsCtax> entityCollection)
      {
          try
          {
              return Entities.UpdateEntity<Galatee.Entity.Model.TAXE>(Entities.ConvertObject<Galatee.Entity.Model.TAXE, CsCtax>(entityCollection));
          }
          catch (Exception e)
          {
              throw e;
          }
      }

  }
} 


