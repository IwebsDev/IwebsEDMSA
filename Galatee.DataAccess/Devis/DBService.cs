
#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Galatee.DataAccess;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using System.Diagnostics;
using Galatee.Structure;
using Galatee.Entity.Model;

#endregion

namespace Galatee.DataAccess 
{
	///<summary>
	/// This class is the Data Access Logic Component implementation for the <see cref="Produit"/> business entity.
	///</summary>
    public class DBService
	{
        //public static ObjETAPEDEVIS GetEtapeSuivanteCaisse(DEVIS devis)
        //{
        //    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
        //    //ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis);
        //    if ((etape == null) || (etape.PK_ID <= 0))
        //        //throw new Exception("Unknown step");
        //        throw new Exception("Etape inconnue");

        //    ObjETAPEDEVIS next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHESUIVANTE);
        //    if (next == null)
        //        //throw new Exception("Next step not configured.");
        //        throw new Exception("Etape suivante non paramétrée.");
        //    return next;
        //}
        public static ObjETAPEDEVIS GetEtapeSuivante_(ObjDEVIS devis)
        {
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
            //ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis);
            if ((etape == null) || (etape.PK_ID <= 0))
                //throw new Exception("Unknown step");
                throw new Exception("Etape inconnue");

            ObjETAPEDEVIS next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHESUIVANTE);
            if (next == null)
                //throw new Exception("Next step not configured.");
                throw new Exception("Etape suivante non paramétrée.");
            return next;
        }
        public static ObjETAPEDEVIS GetEtapeSuivante(ObjDEVIS devis)
        {



            //ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis);
            if ((etape == null) || (etape.PK_ID <= 0))
                //throw new Exception("Unknown step");
                throw new Exception("Etape inconnue");

            ObjETAPEDEVIS next = new ObjETAPEDEVIS();
            next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHESUIVANTE);
            next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHESUIVANTE);
          //next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.FK_IDTACHEDEVIS);
            if (next == null)
                //throw new Exception("Next step not configured.");
                throw new Exception("Etape suivante non paramétrée.");
            return next;
        }

        public static ObjETAPEDEVIS GetEtapeInit(CsDemande devis)
        {

            ObjETAPEDEVIS next = new ObjETAPEDEVIS();
            next = DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.LaDemande.FK_IDTYPEDEMANDE, devis.LaDemande.FK_IDPRODUIT.Value ,devis.LaDemande.FK_IDTYPEDEMANDE);
            if (next == null)
                throw new Exception("Etape suivante non paramétrée.");
            return next;
        }

        public static ObjETAPEDEVIS GetEtapeIntermediaire(ObjDEVIS devis)
        {
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
            if ((etape == null) || (etape.PK_ID <= 0))
                //throw new Exception("Unknown step");
                throw new Exception("Etape inconnue");
            if (etape.IDTACHEINTERMEDIAIRE != null)
                return DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHEINTERMEDIAIRE);
            else
                return null;
        }

        public static ObjETAPEDEVIS GetEtapeRejet(ObjDEVIS devis)
        {
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
            if ((etape == null) || (etape.PK_ID <= 0))
                //throw new Exception("Unknown step");
                throw new Exception("Etape inconnue");
            if (etape.IDTACHEREJET != null)
                return DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHEREJET);
            else
                return null;
        }

        public static ObjETAPEDEVIS GetEtapeSaut(ObjDEVIS devis)
        {
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(devis.FK_IDETAPEDEVIS);
            if ((etape == null) || (etape.PK_ID <= 0))
                throw new Exception("L'étape du devis n'est pas déterminée.");

            return DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.IDTACHESAUT);
        }

        public static ObjETAPEDEVIS GetEtapeAnnulation(ObjDEVIS devis)
        {
            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetByIdTypeDevisNumEtape(devis.FK_IDTYPEDEVIS, (int)Enumere.EtapeDevis.Annulé);
            if ((etape == null) || (etape.PK_ID <= 0))
                throw new Exception("L'étape d'annulation n'est pas paramétrée pour ce type devis.");
                //throw new Exception("Cancellation step unknown for this type of application.");

            return DBETAPEDEVIS.GetByIdTypeDevisIdProduitIdTache(devis.FK_IDTYPEDEVIS, devis.FK_IDPRODUIT, etape.FK_IDTACHEDEVIS);
        }

        public static void SetDBNullParametre(SqlParameterCollection parameters)
        {
            foreach (SqlParameter Parameter in parameters)
            {
                if (Parameter.Value == null)
                {
                    Parameter.Value = DBNull.Value;
                }
            }
        }
	}
}