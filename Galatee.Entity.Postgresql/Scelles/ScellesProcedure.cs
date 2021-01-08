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
    public static class ScelleProcedures
    {

        //public static DataTable RetourneListeActivite()
        //{
        //    //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _Activite = context.RefActivite;

        //            query =
        //            from p in _Activite


        //            select new
        //            {
        //                p.Activite_ID,
        //                p.Activite_Libelle

        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneListeCouleurScelle(int Activite_ID)
        //{
        //    //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _Couleur = context.RefCouleurlot;
        //            var _Tdem = context.RefActivite;
        //            var _Activitecouleur = context.ActiviteCouleur;

        //            query =
        //            from p in _Tdem
        //            join tp in _Activitecouleur on p.Activite_ID equals tp.Activite_ID
        //            join mk in _Couleur on tp.Couleur_ID equals mk.Couleur_ID

        //            where
        //                p.Activite_ID == Activite_ID

        //            select new
        //            {
        //                mk.Couleur_ID,
        //                mk.Couleur_libelle

        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneListeDemandeScelle(int fkdemande)
        //{
        //    //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _DSCELLE = context.DSCELLE;

        //            query =
        //            from p in _DSCELLE

        //            where
        //                p.FK_IDDEMANDE == fkdemande

        //            select new
        //            {
        //                p.NUMDEM,
        //                p.FK_IDAGENT,
        //                p.FK_IDCENTRE,
        //                p.FK_IDACTIVITE,
        //                p.FK_IDCOULEURSCELLE,
        //                p.FK_IDDEMANDE,
        //                p.NOMBRE_DEM,
        //                p.NOMBRE_REC
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneListeScelle()
        //{
        //    //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _SCELLE = context.tbLotMagasinGeneral;

        //            query =
        //            from p in _SCELLE

        //            where
        //                p.StatutLot_ID == 0

        //            select new
        //            {
        //                p.Id_LotMagasinGeneral,
        //                p.Couleur_ID,
        //                p.CodeCentre,
        //                p.Numero_depart,
        //                p.Numero_fin,
        //                p.Nbre_Scelles
        //            };
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static string InsertDemandeScelle(DEMANDE Demande, DSCELLE scelle)
        //{
        //    try
        //    {
        //        int id;
        //        DEMANDE DEM = new DEMANDE();
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            Entities.InsertEntity<DEMANDE>(Demande);

        //            scelle.FK_IDDEMANDE = Demande.PK_ID;
        //        }

        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            scelle.NUMDEM = Demande.NUMDEM;
        //            Entities.InsertEntity<DSCELLE>(scelle);
        //            ctontext.SaveChanges();
        //        }

        //        return Demande.PK_ID + "." + Demande.NUMDEM;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        var errorMessages = ex.EntityValidationErrors
        //               .SelectMany(x => x.ValidationErrors)
        //               .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        return string.Empty;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //        throw ex;
        //    }

        //}
  
        //public static bool InsertAffectionScelle(AffectationScelle AffectationScelle, List<DetailAffectationScelle> Lst_DetailAffectationScelle)
        //{
        //    try
        //    {
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            Entities.InsertEntity<AffectationScelle>(AffectationScelle);
        //            Entities.InsertEntity<DetailAffectationScelle>(Lst_DetailAffectationScelle);

        //            ctontext.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        var errorMessages = ex.EntityValidationErrors
        //               .SelectMany(x => x.ValidationErrors)
        //               .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        return false;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneListeDetailAffectationScelle(int IdDemande)
        //{
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _SCELLE = context.DetailAffectationScelle.Where(a => a.AffectationScelle.DEMANDE.PK_ID == IdDemande && a.EstLivre == false);

        //            query =
        //            from p in _SCELLE

        //            select p;
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool ValidationReception(List<DetailAffectationScelle> ListeScelle, List<Scelles> Scelle, tbLot Lot)
        //{
        //    try
        //    {
        //        using (galadbEntities ctontext = new galadbEntities())
        //        {
        //            Entities.InsertEntity<Scelles>(Scelle);
        //            Entities.InsertEntity<tbLot>(Lot);
        //            Entities.UpdateEntity<DetailAffectationScelle>(ListeScelle);

        //            ctontext.SaveChanges();
        //        }

        //        return true;
        //    }
        //    catch (DbEntityValidationException ex)
        //    {

        //        var errorMessages = ex.EntityValidationErrors
        //               .SelectMany(x => x.ValidationErrors)
        //               .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        return false;
        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

        //        throw ex;
        //    }
        //}

        //public static DataTable RetourneListeAffectationScelle(Guid IdAffectationScelle)
        //{
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            var _SCELLE = context.AffectationScelle.Where(a => a.Id_Affectation == IdAffectationScelle);

        //            query =
        //            from p in _SCELLE

        //            select p;
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}




        public static DataTable RetourneListeActivite()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Activite = context.RefActivite;

                    query =
                    from p in _Activite


                    select new
                    {
                        p.Activite_ID,
                        p.Activite_Libelle

                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeCouleurScelle(int Activite_ID)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = RetournCouleurParActivite(Activite_ID, context);
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IEnumerable<object> RetournCouleurParActivite(int Activite_ID, galadbEntities context)
        {
            IEnumerable<object> query = null;
            var _Couleur = context.RefCouleurlot;
            var _Tdem = context.RefActivite;
            var _Activitecouleur = context.ActiviteCouleur;

            query =
            from p in _Tdem
            join tp in _Activitecouleur on p.Activite_ID equals tp.Activite_ID
            join mk in _Couleur on tp.Couleur_ID equals mk.Couleur_ID

            where
                p.Activite_ID == Activite_ID

            select new
            {
                mk.Couleur_ID,
                mk.Couleur_libelle,
                p.Activite_ID,
                p.Activite_Libelle,
            };
            return query;
        }

        public static DataTable RetourneListeAllCouleurScelle()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Couleur = context.RefCouleurlot;
                    var _Tdem = context.RefActivite;
                    var _Activitecouleur = context.ActiviteCouleur;

                    query =
                    from p in _Tdem
                    join tp in _Activitecouleur on p.Activite_ID equals tp.Activite_ID
                    join mk in _Couleur on tp.Couleur_ID equals mk.Couleur_ID
                    select new
                    {
                        mk.Couleur_ID,
                        mk.Couleur_libelle,
                        p.Activite_ID,
                        p.Activite_Libelle,
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeDemandeScelle(int fkdemande)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _DSCELLE = context.DSCELLE;
                    query =
                    from p in _DSCELLE
                    where
                        p.FK_IDDEMANDE == fkdemande

                    select new
                    {
                        p.NUMDEM,
                        p.FK_IDAGENT,
                        p.FK_IDCENTRE,
                        p.FK_IDACTIVITE,
                        p.FK_IDCOULEURSCELLE,
                        p.FK_IDDEMANDE,
                        p.NOMBRE_DEM,
                        p.NOMBRE_REC,
                        LIBELLECOULEUR = p.RefCouleurlot.Couleur_libelle ,
                        p.FK_IDCENTREFOURNISSEUR,
                        LIBELLEAGENT = p.ADMUTILISATEUR.LIBELLE ,
                        LIBELLEACTIVITE = p.RefActivite.Activite_Libelle ,
                        LIBELLESITEAGENT = p.CENTRE.SITE.LIBELLE ,
                        LIBELLECENTREDESTINATAIRE = p.CENTRE.LIBELLE ,
                        LIBELLECENTREFOURNISSEUR = p.CENTRE1.LIBELLE ,
                        MATRICULE = p.ADMUTILISATEUR.MATRICULE 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeScelle()
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _SCELLE = context.tbLotMagasinGeneral;

                    query =
                    from p in _SCELLE

                    where
                        p.StatutLot_ID == 0
                    select new
                    {
                        p.Id_LotMagasinGeneral,
                        p.DateReception,
                        p.Matricule_AgentReception,
                        p.Numero_depart,
                        p.Numero_fin,
                        p.Nbre_Scelles,
                        p.StatutLot_ID,
                        p.Origine_ID,
                        p.CodeCentre,
                        p.Couleur_ID,
                        p.Fournisseur_ID,
                        p.Date_DerniereModif,
                        p.Matricule_AgentDerniereModif,
                        p.Id_Affectation,
                        p.Activite_ID,
                        Libelle_statue = p.RefStatutLotMagasinGeneral.Libelle_Statut,
                        Libelle_Fournisseur = p.RefFournisseurs.Fournisseur_Libelle,
                        Libelle_Origine = p.RefOrigineScelle.Origine_Libelle 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListeScelle(int IdCentreRecuperationDeLot)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    List<tbLot> _SCELLE = new List<tbLot>();
                    if (IdCentreRecuperationDeLot == context.CENTRE.FirstOrDefault(c => c.CODE==Enumere.Generale).PK_ID)
                    {
                        return RetourneListeScelle();
                    }
                    else
                    {
                        _SCELLE = context.tbLot.Where(l => l.CENTRE1.PK_ID == IdCentreRecuperationDeLot).ToList();
                        query =
                        from p in _SCELLE

                        where
                            p.Status_lot_ID == 0
                        //select p;
                        select new
                        {
                            Id_LotMagasinGeneral = p.lot_ID,
                            p.DateReception,
                            Matricule_AgentReception = p.Matricule_Creation,
                            p.Numero_depart,
                            p.Numero_fin,
                            Nbre_Scelles = p.Nombre_scelles_lot,
                            StatutLot_ID = p.Status_lot_ID,
                            p.Origine_ID,
                            CodeCentre = p.CENTRE1.PK_ID,
                            Couleur_ID = p.lot_Couleur_ID,
                            //p.Fournisseur_ID,
                            Date_DerniereModif = p.Date_Derniere_Modif,
                            Matricule_AgentDerniereModif = p.Matricule_AgentModification
                            //,p.Id_Affectation,
                            //p.Activite_ID,
                        };
                    }
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string InsertDemandeScelle(DEMANDE Demande, DSCELLE scelle)
        {
            try
            {
                int id;
                DEMANDE DEM = new DEMANDE();
                using (galadbEntities ctontext = new galadbEntities())
                {
                    Entities.InsertEntity<DEMANDE>(Demande);

                    scelle.FK_IDDEMANDE = Demande.PK_ID;
                }

                using (galadbEntities ctontext = new galadbEntities())
                {
                    scelle.NUMDEM = Demande.NUMDEM;
                    Entities.InsertEntity<DSCELLE>(scelle);
                    ctontext.SaveChanges();
                }

                return Demande.PK_ID + "." + Demande.NUMDEM;
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                       .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                return string.Empty;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                throw ex;
            }

        }





        public static DataTable RetourneCentre()
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    //IEnumerable<object> query = null;
                    var _SCELLE = context.CENTRE.ToList();

                    //query =
                    //from p in _SCELLE

                    //select p;
                    return Galatee.Tools.Utility.ListToDataTable(_SCELLE);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool InsertAffectionScelle(AffectationScelle AffectationScelle, List<DetailAffectationScelle> Lst_DetailAffectationScelle, List<tbLotMagasinGeneral> Lst_Lot, int IdCente)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {


                    Entities.InsertEntity<AffectationScelle>(AffectationScelle);
                    Entities.InsertEntityBloc<DetailAffectationScelle>(Lst_DetailAffectationScelle, ctontext.Database.Connection.ConnectionString );
                    if (IdCente == 1)
                    {
                        Lst_Lot.ForEach(c => c.Id_Affectation = null);
                        Entities.UpdateEntity<tbLotMagasinGeneral>(Lst_Lot);
                    }
                    else
                    {
                        List<tbLot> LsttbLot = new List<tbLot>();
                        foreach (var item in Lst_Lot)
                        {
                            tbLot tbLot = new tbLot();
                            tbLot.agence_centre_Appartenance = AffectationScelle.Code_Centre_Dest ;
                            tbLot.agence_centre_Origine = AffectationScelle.Code_Centre_Origine ;
                            tbLot.CENTRE = item.CENTRE;
                            tbLot.Date_Derniere_Modif = DateTime.Now;
                            tbLot.DateReception = item.DateReception;
                            tbLot.lot_Couleur_ID = item.Couleur_ID;
                            tbLot.lot_ID = item.Id_LotMagasinGeneral;
                            tbLot.Matricule_AgentModification = item.Matricule_AgentDerniereModif;
                            tbLot.Matricule_Creation = item.Matricule_AgentReception;
                            tbLot.Nombre_scelles_lot = item.Nbre_Scelles.Value;
                            tbLot.Nombre_scelles_reçu = item.Nbre_Scelles;
                            tbLot.Numero_depart = item.Numero_depart;
                            tbLot.Numero_fin = item.Numero_fin;
                            tbLot.Origine_ID = item.Origine_ID;
                            tbLot.Status_lot_ID = item.StatutLot_ID;

                            LsttbLot.Add(tbLot);
                        }

                        Entities.UpdateEntity<tbLot>(LsttbLot);
                    }


                    ctontext.SaveChanges();
                }

                return true;
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                       .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                return false;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                throw ex;
            }
        }

        public static DataTable RetourneListeDetailAffectationScelle(int IdDemande)
        {
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _SCELLE = context.DetailAffectationScelle.Where(a => a.AffectationScelle.DEMANDE.PK_ID == IdDemande && a.EstLivre == false);
                    query =
                    from p in _SCELLE

                    select p;
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ValidationReception(List<DetailAffectationScelle> ListeScelle, List<Scelles> Scelle, tbLot Lot)
        {
            try
            {
                using (galadbEntities ctontext = new galadbEntities())
                {
                    Lot.Date_Derniere_Modif = DateTime.Now; ;
                    Entities.InsertEntity<tbLot>(Lot);
                    Entities.UpdateEntity<DetailAffectationScelle>(ListeScelle);
                    Scelle.ForEach(s => s.lot_ID = Lot.lot_ID);
                    Scelle.ForEach(ds => ds.Date_creation_scelle = DateTime.Now);
                    Entities.InsertEntity<Scelles>(Scelle);
                    ctontext.SaveChanges();
                }


                return true;
            }
            catch (DbEntityValidationException ex)
            {

                var errorMessages = ex.EntityValidationErrors
                       .SelectMany(x => x.ValidationErrors)
                       .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                return false;
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                throw ex;
            }
        }

        public static DataTable RetourneListeAffectationScelle(Guid IdAffectationScelle)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _AffectationScelle = context.AffectationScelle;
                    query =
                    from p in _AffectationScelle
                    where
                     p.Id_Affectation == IdAffectationScelle 
                    select new
                    {
                        p.Id_Affectation,
                        p.Code_Centre_Origine,
                        p.Code_Centre_Dest,
                        p.Date_Transfert,
                        p.Id_Modificateur,
                        p.Nbre_Scelles,
                        p.Id_Demande,
                        p.NumScelleDepart,
                        p.NumScelleFin
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable RetourneListeAffectationScelle(List<Guid> LstIdAffectationScelle)
        {
            try
            {
                 using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _AffectationScelle = context.AffectationScelle;
                    query =
                    from p in _AffectationScelle
                    where 
                     LstIdAffectationScelle.Contains(p.Id_Affectation)
                    select new
                    {
                        p.Id_Affectation ,
                        p.Code_Centre_Origine ,
                        p.Code_Centre_Dest ,
                        p.Date_Transfert ,
                        p.Id_Modificateur ,
                        p.Nbre_Scelles ,
                        p.Id_Demande ,
                        p.NumScelleDepart ,
                        p.NumScelleFin 
                    };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable SCELLES_RetourScelleAgt_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.TbRetourScelles
                  join c in context.TbDetailRetourScelles on x.Id_Retour equals c.Id_Retour
                  join cn in context.Scelles on c.Id_Scelle equals cn.Id_Scelle

                  select new
                  {
                      x.Id_Retour,
                      x.CodeCentre,
                      x.Receveur_Mat,
                      x.Donneur_Mat,
                      x.Date_Retour,
                      x.Nbre_Scelles,
                      c.Id_Scelle,
                      c.Id_DetailRetour,
                      cn.Numero_Scelle,
                      cn.Status_ID,
                      libelle_Statut = cn.RefStatus.Status,
                      cn.RefCouleurlot.Couleur_libelle ,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_RemiseScelle_RETOURNE(int pk_ID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Scelles = context.Scelles;
                    var _tbDetailRemiseScelles = context.tbDetailRemiseScelles;
                    var _remiseScelle = context.tbRemiseScelles;
                    var _ActiviteScelle = context.ActiviteCouleur ;
                    query =
                          from p in _tbDetailRemiseScelles
                          join y in _ActiviteScelle on p.Scelles.Couleur_Scelle equals y.Couleur_ID 
                          where p.tbRemiseScelles.Matricule_Receiver == pk_ID  
                          select new
                          {
                              p.Scelles.Numero_Scelle ,
                              p.Scelles.Status_ID  ,
                              p.Scelles.RefStatus.Status   ,
                              p.tbRemiseScelles.Matricule_Receiver    ,
                              p.tbRemiseScelles.Nbre_Scelles     ,
                              p.tbRemiseScelles.CodeCentre      ,
                              p.Scelles.Id_Scelle       ,
                              p.tbRemiseScelles.Date_Remise      ,
                              p.Scelles.Couleur_Scelle       ,
                              y.Activite_ID ,
                              p.Scelles.RefCouleurlot.Couleur_libelle 
                          };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SCELLES_RETOURNE_Pour_ScellageCpt(int pk_ID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Scelles = context.Scelles;
                    var _tbDetailRemiseScelles = context.tbDetailRemiseScelles;
                    var _remiseScelle = context.tbRemiseScelles;
                    var _ActiviteScelle = context.ActiviteCouleur;
                    query =
                          (from p in _tbDetailRemiseScelles
                          join y in _ActiviteScelle on p.Scelles.Couleur_Scelle equals y.Couleur_ID
                          where p.tbRemiseScelles.Matricule_Receiver == pk_ID && y.Activite_ID == 1 &&
                          p.Scelles.Status_ID == 2
                          select new
                          {
                              p.Scelles.Numero_Scelle,
                              p.Scelles.Status_ID,
                              p.Scelles.RefStatus.Status,
                              p.tbRemiseScelles.Matricule_Receiver,
                              p.tbRemiseScelles.CodeCentre,
                              p.Scelles.Id_Scelle,
                              p.Scelles.Couleur_Scelle,
                              y.Activite_ID,
                              p.Scelles.RefCouleurlot.Couleur_libelle
                          }).Distinct();

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Status scelle
        public static DataTable SCELLES_StatusScelle_Modele_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefStatus
                  select new
                  {
                      x.Status,
                      x.Status_ID

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLE_Status_ModeleById(int id_Statuts)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.RefStatus
                     where
                       x.Status_ID == id_Statuts
                     select new
                     {
                         x.Status,
                         x.Status_ID
                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region Bobou


        #region MArgasin virtuelle
        public static DataTable SCELLES_MAGASINVIRTUEL_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.MAGASINVIRTUEL
                  select new
                  {
                      x.PK_ID,
                      x.PRODUIT,
                      x.MARQUE,
                      x.ANNEEFAB,
                      x.FONCTIONNEMENT,
                      x.CADRAN,
                      x.NUMERO,
                      x.ETAT,
                      x.FK_IDCALIBRECOMPTEUR ,
                      x.FK_IDMARQUECOMPTEUR,
                      x.FK_IDTYPECOMPTEUR,
                      x.USERCREATION,
                      x.USERMODIFICATION,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      libelleProduit = x.PRODUIT.LIBELLE,
                      libelleMArque = x.MARQUECOMPTEUR.LIBELLE,



                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_MAGASINVIRTUEL_RETOURNEById(int Pk_ID)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.MAGASINVIRTUEL
                     where
                       x.PK_ID == Pk_ID
                     select new
                     {
                         x.PK_ID,
                         x.PRODUIT,
                         x.MARQUE,
                         x.ANNEEFAB,
                         x.FONCTIONNEMENT,
                         x.CADRAN,
                         x.NUMERO,
                         x.FK_IDCALIBRECOMPTEUR ,
                         x.FK_IDMARQUECOMPTEUR,
                         x.FK_IDTYPECOMPTEUR,
                         x.USERCREATION,
                         x.USERMODIFICATION,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         libelleProduit = x.PRODUIT.LIBELLE,
                         libelleMArque = x.MARQUECOMPTEUR.LIBELLE,

                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region RefOrigineScelle
        public static DataTable SCELLES_RefOrigineScelle_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefOrigineScelle
                  select new
                  {
                      x.Origine_ID,
                      x.Origine_Libelle,
                      x.Longueur_ScelleID
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SCELLE_RefOrigineScelle_RETOURNEById(int pIdRefOrigineScelle)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.RefOrigineScelle
                     where
                       x.Origine_ID == pIdRefOrigineScelle
                     select new
                     {
                         x.Origine_ID,
                         x.Origine_Libelle,
                         x.Longueur_ScelleID
                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
        #endregion

        #region Vwrecherchecompteur
        public static DataTable SCELLES_vWRechercheCompteur_RETOURNE()
        {
            try
            {
                return new DataTable();
                //using (galadbEntities context = new galadbEntities())
                //{
                //    IEnumerable<object> query =
                //  from x in context.vWRechercheCompteur
                //  select new
                //  {
                //      x.Numero_Compteur,
                //      x.StatutCompteur,
                //      x.Type_Compteur_ID,
                //      x.EtatCompteur_ID,
                //      x.CapotMoteur_Numero_Scelle1,
                //      x.CapotMoteur_Couleur_Scelle1,
                //      x.CapotMoteur_Numero_Scelle2,
                //      x.CapotMoteur_Couleur_Scelle2,
                //      x.CapotMoteur_Numero_Scelle3,
                //      x.CapotMoteur_Couleur_Scelle3,
                //      x.TYPE_COMPTEUR,
                //      x.DIAMETRE,
                //      x.MARQUE,
                //      x.CADRAN,
                //      x.ANNEEFAB,
                //      x.FONCTIONNEMENT,
                //      x.CapotMoteur_DateDePoseScelle3,
                //      x.CodeCentre,
                //      x.Cache_Scelle,
                //      x.FK_IDDIAMETRECOMPTEUR,
                //      x.FK_IDMARQUECOMPTEUR,
                //      x.FK_IDTYPECOMPTEUR,
                //      x.Cache_Couleur_Scelle4,
                //      x.Cache_Scelle_ID,
                //      x.Cache_Scelle_Numero_Scelle4,
                //      x.Cache_sceller_DateDePoseScelle4,
                //      x.USERCREATION,
                //      x.DATECREATION,
                //      x.DATEMODIFICATION,
                //      x.USERMODIFICATION
                //  };

                    //return Galatee.Tools.Utility.ListToDataTable(query);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLE_vWRechercheCompteurById(string Numero_Compteur)
        {
            try
            {
                return new DataTable();
                //using (galadbEntities context = new galadbEntities())
                //{
                //    IEnumerable<object> query =
                //     from x in context.vWRechercheCompteur
                //     where
                //       x.Numero_Compteur == Numero_Compteur
                //     select new
                //     {
                //         x.Numero_Compteur,
                //         x.StatutCompteur,
                //         x.Type_Compteur_ID,
                //         x.EtatCompteur_ID,
                //         x.CapotMoteur_Numero_Scelle1,
                //         x.CapotMoteur_Couleur_Scelle1,
                //         x.CapotMoteur_Numero_Scelle2,
                //         x.CapotMoteur_Couleur_Scelle2,
                //         x.CapotMoteur_Numero_Scelle3,
                //         x.CapotMoteur_Couleur_Scelle3,
                //         x.TYPE_COMPTEUR,
                //         x.DIAMETRE,
                //         x.MARQUE,
                //         x.CADRAN,
                //         x.ANNEEFAB,
                //         x.FONCTIONNEMENT,
                //         x.CapotMoteur_DateDePoseScelle3,
                //         x.CodeCentre,
                //         x.Cache_Scelle,
                //         x.FK_IDDIAMETRECOMPTEUR,
                //         x.FK_IDMARQUECOMPTEUR,
                //         x.FK_IDTYPECOMPTEUR,
                //         x.Cache_Couleur_Scelle4,
                //         x.Cache_Scelle_ID,
                //         x.Cache_Scelle_Numero_Scelle4,
                //         x.Cache_sceller_DateDePoseScelle4,
                //         x.USERCREATION,
                //         x.DATECREATION,
                //         x.DATEMODIFICATION,
                //         x.USERMODIFICATION
                //     };

                //    return Galatee.Tools.Utility.ListToDataTable(query);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Fournisseurs
        public static DataTable SCELLES_RefFournisseurs_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefFournisseurs
                  select new
                  {
                      x.Fournisseur_ID,
                      x.Fournisseur_Libelle

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_RefFournisseurs_RETOURNEById(int pIdRefFournisseurs)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.RefFournisseurs
                     where
                       x.Fournisseur_ID == pIdRefFournisseurs
                     select new
                     {
                         x.Fournisseur_ID,
                         x.Fournisseur_Libelle

                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LotMagasinGeneral
        public static DataTable SCELLES_LotMagasinGeneral_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.tbLotMagasinGeneral
                  select new
                  {
                      x.Id_LotMagasinGeneral,
                      x.DateReception,
                      x.Matricule_AgentReception,
                      x.Numero_depart,
                      x.Numero_fin,
                      x.Nbre_Scelles,
                      x.StatutLot_ID,
                      x.Origine_ID,
                      x.CodeCentre,
                      x.Couleur_ID,
                      x.Fournisseur_ID,
                      x.Date_DerniereModif,
                      x.Matricule_AgentDerniereModif,
                      x.Id_Affectation,
                      x.Activite_ID,
                      Libelle_statue=x.RefStatutLotMagasinGeneral.Libelle_Statut,
                      Libelle_Fournisseur = x.RefFournisseurs.Fournisseur_Libelle ,
                      Libelle_Origine = x.RefOrigineScelle.Origine_Libelle ,
                      Couleur_libelle = x.RefCouleurlot.Couleur_libelle 

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_LotMagasinGeneral_RETOURNEById(string pIdLotMagasinGeneral)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.tbLotMagasinGeneral
                     where
                       x.Id_LotMagasinGeneral == pIdLotMagasinGeneral
                     select new
                     {
                         x.Id_LotMagasinGeneral,
                         x.DateReception,
                         x.Matricule_AgentReception,
                         x.Numero_depart,
                         x.Numero_fin,
                         x.Nbre_Scelles,
                         x.StatutLot_ID,
                         x.Origine_ID,
                         x.CodeCentre,
                         x.Couleur_ID,
                         x.Fournisseur_ID,
                         x.Date_DerniereModif,
                         x.Matricule_AgentDerniereModif,
                         x.Id_Affectation,
                         x.Activite_ID

                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CompteurBta

        public static DataTable SelectAllCompteurNonAffecte()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.TbCompteurBTA
                  join y in context.RefEtatCompteur on x.EtatCompteur_ID equals y.EtatCompteur_ID  
                  where x.StatutCompteur == "Non_Affecté"
                  select new
                  {
                             x. Numero_Compteur ,
                            x. Type_Compteur_ID ,
                            x. StatutCompteur ,
                            x. EtatCompteur_ID ,
                            x. CapotMoteur_ID_Scelle1 ,
                            x. CapotMoteur_ID_Scelle2 ,
                            x. CapotMoteur_ID_Scelle3 ,
                            x. Cache_Scelle ,
                            x. MARQUE ,
                            x. ANNEEFAB ,
                            x. FONCTIONNEMENT ,
                            x.CADRAN ,
                            x.FK_IDCALIBRECOMPTEUR  ,
                            x. FK_IDMARQUECOMPTEUR ,
                            x. FK_IDTYPECOMPTEUR ,
                            x. FK_IDPRODUIT ,
                            x. USERCREATION ,
                            x. DATECREATION ,
                            x. DATEMODIFICATION ,
                            x. USERMODIFICATION ,
                             CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                             CODEPRODUIT = x.PRODUIT.CODE,
                            Numero_ScelleCapot_1=x.Scelles1.Numero_Scelle,
                            Numero_ScelleCapot_2=x.Scelles2.Numero_Scelle,
                             Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                             Numero_Cache_3 = x.Scelles.Numero_Scelle ,
                            LIBELLEMARQUE=x.MARQUECOMPTEUR.LIBELLE,
                            LIBELLEPRODUIT = x.PRODUIT.LIBELLE ,
                            LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE ,
                             LIBELLEETATCOMPTEUR = y.Libelle_ETAT,
                             x.PK_ID
                  };
                  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_CompteurBta(CsCompteurBta leCompteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.TbCompteurBTA
                  where x.Numero_Compteur == leCompteur.Numero_Compteur && x.FK_IDMARQUECOMPTEUR == leCompteur.FK_IDMARQUECOMPTEUR 
                  select new
                  {
                      x.Numero_Compteur 
                     
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sylla 09/01/2017

        public static DataTable SCELLES_CompteurBtaNew(CsCompteur leCompteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.COMPTEUR
                  where x.NUMERO == leCompteur.NUMERO && x.FK_IDMARQUECOMPTEUR == leCompteur.FK_IDMARQUECOMPTEUR
                  select new
                  {
                      x.NUMERO

                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public static DataTable ReturnecompteursInDisponibles(List<int> lstCentre, int? idCalibreCompteur, string produit, string EtatCompteur)
        {

            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Magazin = context.MAGASINVIRTUEL;
                    query =
                  from z in _Magazin
                  join x in context.TbCompteurBTA on z.FK_TBCOMPTEUR equals x.PK_ID
                  join y in context.RefEtatCompteur on x.EtatCompteur_ID equals y.EtatCompteur_ID  
                  where 
                  //(lstCentre.Contains(z.CENTRE.PK_ID) || lstCentre==null) &&
                         (z.FK_IDCALIBRECOMPTEUR == idCalibreCompteur || idCalibreCompteur == null)
                  && (z.PRODUIT.CODE == produit || string.IsNullOrEmpty(produit))
                  && z.ETAT == "Affecté"
                  select new
                  {
                      x.Numero_Compteur,
                      x.Type_Compteur_ID,
                      x.StatutCompteur,
                      x.EtatCompteur_ID,
                      x.CapotMoteur_ID_Scelle1,
                      x.CapotMoteur_ID_Scelle2,
                      x.CapotMoteur_ID_Scelle3,
                      x.Cache_Scelle,
                      x.MARQUE,
                      x.ANNEEFAB,
                      x.FONCTIONNEMENT,
                      x.CADRAN,
                      x.FK_IDCALIBRECOMPTEUR,
                      x.FK_IDMARQUECOMPTEUR,
                      x.FK_IDTYPECOMPTEUR,
                      x.FK_IDPRODUIT,
                      x.USERCREATION,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERMODIFICATION,
                      CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                      CODEPRODUIT = x.PRODUIT.CODE,
                      Numero_ScelleCapot_1 = x.Scelles1.Numero_Scelle,
                      Numero_ScelleCapot_2 = x.Scelles2.Numero_Scelle,
                      Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                      Numero_Cache_3 = x.Scelles.Numero_Scelle,
                      LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                      LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE,
                      LIBELLEETATCOMPTEUR = y.Libelle_ETAT,
                      x.PK_ID
                      ,
                      FK_IDCENTRE = z.CENTRE.PK_ID
                  };
                    return Galatee.Tools.Utility.ListToDataTable(query);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_CompteurBta_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.TbCompteurBTA
                  join y in context.RefEtatCompteur on x.EtatCompteur_ID equals y.EtatCompteur_ID  
                  //join z in context.MAGASINVIRTUEL on x.PK_ID equals z.FK_TBCOMPTEUR
                  select new
                  {
                      x.Numero_Compteur,
                      x.Type_Compteur_ID,
                      x.StatutCompteur,
                      x.EtatCompteur_ID,
                      x.CapotMoteur_ID_Scelle1,
                      x.CapotMoteur_ID_Scelle2,
                      x.CapotMoteur_ID_Scelle3,
                      x.Cache_Scelle,
                      x.MARQUE,
                      x.ANNEEFAB,
                      x.FONCTIONNEMENT,
                      x.CADRAN,
                      x.FK_IDCALIBRECOMPTEUR,
                      x.FK_IDMARQUECOMPTEUR,
                      x.FK_IDTYPECOMPTEUR,
                      x.FK_IDPRODUIT,
                      x.USERCREATION,
                      x.DATECREATION,
                      x.DATEMODIFICATION,
                      x.USERMODIFICATION,
                      CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                      CODEPRODUIT = x.PRODUIT.CODE,
                      Numero_ScelleCapot_1 = x.Scelles1.Numero_Scelle,
                      Numero_ScelleCapot_2 = x.Scelles2.Numero_Scelle,
                      Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                      Numero_Cache_3 = x.Scelles.Numero_Scelle,
                      LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                      LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                      LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE,
                      LIBELLEETATCOMPTEUR = y.Libelle_ETAT ,
                      x.PK_ID 
                  };

                  return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_CompteurBta_RETOURNEById(string Numero_Compteur)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.TbCompteurBTA
                     where
                       x.Numero_Compteur == Numero_Compteur
                     select new
                     {
                         x.Numero_Compteur,
                         x.Type_Compteur_ID,
                         x.StatutCompteur,
                         x.EtatCompteur_ID,
                         x.CapotMoteur_ID_Scelle1,
                         x.CapotMoteur_ID_Scelle2,
                         x.CapotMoteur_ID_Scelle3,
                         x.Cache_Scelle,
                         x.MARQUE,
                         x.ANNEEFAB,
                         x.FONCTIONNEMENT,
                         x.CADRAN,
                         x.FK_IDCALIBRECOMPTEUR,
                         x.FK_IDMARQUECOMPTEUR,
                         x.FK_IDTYPECOMPTEUR,
                         x.FK_IDPRODUIT,
                         x.USERCREATION,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERMODIFICATION,
                         CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                         CODEPRODUIT = x.PRODUIT.CODE,
                         Numero_ScelleCapot_1 = x.Scelles1.Numero_Scelle,
                         Numero_ScelleCapot_2 = x.Scelles2.Numero_Scelle,
                         Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                         Numero_Cache_3 = x.Scelles.Numero_Scelle,
                         LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                         LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                         LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE ,
                         x.PK_ID 

                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_CompteurBta_RETOURNEByNumScelle(string Numero_Scelle)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.TbCompteurBTA
                     where
                      ( x.Scelles.Numero_Scelle == Numero_Scelle || 
                       x.Scelles1 .Numero_Scelle == Numero_Scelle || 
                       x.Scelles2 .Numero_Scelle == Numero_Scelle || 
                       x.Scelles3 .Numero_Scelle == Numero_Scelle  )
                     select new
                     {
                         x.Numero_Compteur,
                         x.Type_Compteur_ID,
                         x.StatutCompteur,
                         x.EtatCompteur_ID,
                         x.CapotMoteur_ID_Scelle1,
                         x.CapotMoteur_ID_Scelle2,
                         x.CapotMoteur_ID_Scelle3,
                         x.Cache_Scelle,
                         x.MARQUE,
                         x.ANNEEFAB,
                         x.FONCTIONNEMENT,
                         x.CADRAN,
                         x.FK_IDCALIBRECOMPTEUR,
                         x.FK_IDMARQUECOMPTEUR,
                         x.FK_IDTYPECOMPTEUR,
                         x.FK_IDPRODUIT,
                         x.USERCREATION,
                         x.DATECREATION,
                         x.DATEMODIFICATION,
                         x.USERMODIFICATION,
                         CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                         CODEPRODUIT = x.PRODUIT.CODE,
                         Numero_ScelleCapot_1 = x.Scelles1.Numero_Scelle,
                         Numero_ScelleCapot_2 = x.Scelles2.Numero_Scelle,
                         Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                         Numero_Cache_3 = x.Scelles.Numero_Scelle,
                         LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                         LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                         LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE,
                         x.PK_ID 
                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataTable RetourneListeCompteurMagasinCentre(string CodeSite)
        {
            //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
            try
            {

                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Compteur = context.MAGASINVIRTUEL;
                    var _Scellage = context.TbCompteurBTA ;
                    query =
                      from p in _Compteur
                      join x in _Scellage on new { p.NUMERO, p.MARQUE } equals new { NUMERO = x.Numero_Compteur, MARQUE = x.MARQUE }
                      where
                           p.ETAT.Equals(Enumere.CompteurAffecte)
                          && p.CENTRE.CODESITE == CodeSite

                      select new
                      {

                          x.Numero_Compteur,
                          x.Type_Compteur_ID,
                          x.StatutCompteur,
                          x.EtatCompteur_ID,
                          x.CapotMoteur_ID_Scelle1,
                          x.CapotMoteur_ID_Scelle2,
                          x.CapotMoteur_ID_Scelle3,
                          x.MARQUE,
                          x.ANNEEFAB,
                          x.FONCTIONNEMENT,
                          x.TYPECOMPTEUR,
                          x.CADRAN,
                          x.FK_IDCALIBRECOMPTEUR,
                          x.FK_IDMARQUECOMPTEUR,
                          x.FK_IDTYPECOMPTEUR,
                          x.USERCREATION,
                          x.USERMODIFICATION,
                          x.DATECREATION,
                          x.DATEMODIFICATION,

                          CALIBRE = x.CALIBRECOMPTEUR.LIBELLE,
                          CODEPRODUIT = x.PRODUIT.CODE,
                          Numero_ScelleCapot_1 = x.Scelles1.Numero_Scelle,
                          Numero_ScelleCapot_2 = x.Scelles2.Numero_Scelle,
                          Numero_ScelleCapot_3 = x.Scelles3.Numero_Scelle,
                          Numero_Cache_3 = x.Scelles.Numero_Scelle,
                          LIBELLEMARQUE = x.MARQUECOMPTEUR.LIBELLE,
                          LIBELLEPRODUIT = x.PRODUIT.LIBELLE,
                          LIBELLETYPECOMPTEUR = x.TYPECOMPTEUR.LIBELLE  
                      };
                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  Remise Scelles
        public static DataTable SCELLES_RemiseScelleAgt_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.tbRemiseScelles
                    join c in context.tbDetailRemiseScelles on x.Id_Remise equals c.Id_Remise
                  //   join ss in context.Scelles on ss.Id_Scelle equals c.Id_Scelle
                  select new
                  {
                      x.Id_Remise,
                      x.CodeCentre,
                      x.Matricule_Receiver,
                      x.Matricule_User,
                      x.Motif_ID,
                      x.Nbre_Scelles,
                      x.Date_Remise,
                      x.TypeRemise,
                      Libelle_Motif=x.RefMotif.Motif_libelle,
                     c.Id_Scelle,
                     c.Id_DetailRemise
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_RemiseScelleAgt_RETOURNEById(Guid id_remise)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.tbRemiseScelles
                     where
                       x.Id_Remise == id_remise
                     select new
                     {
                         x.Id_Remise,
                         x.CodeCentre,
                         x.Matricule_Receiver,
                         x.Matricule_User,
                         x.Motif_ID,
                         x.Nbre_Scelles,
                         x.Date_Remise,
                         x.TypeRemise,
                         
                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  Retour Scelles

        //public static DataTable SCELLES_RetourScelleAgt_RETOURNE()
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities()) 
        //        {
        //            IEnumerable<object> query =
        //          from x in context.TbRetourScelles
        //          join c in context.TbDetailRetourScelles on x.Id_Retour equals c.Id_Retour

        //          select new
        //          {
        //              x.Id_Retour,
        //              x.CodeCentre,
        //              x.Receveur_Mat,
        //              x.Donneur_Mat,
        //              x.Date_Retour,
        //              x.Nbre_Scelles,
        //             c.Id_Scelle,
        //             c.Id_DetailRetour
        //          };

        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static DataTable SCELLES_RetourScelleAgt_RETOURNEById(Guid id_retour)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.TbRetourScelles
                     where
                       x.Id_Retour == id_retour
                     select new
                     {
                         x.Id_Retour,
                         x.CodeCentre,
                         x.Receveur_Mat,
                         x.Donneur_Mat,
                         x.Date_Retour,
                         x.Nbre_Scelles,



                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Etat Compteur
        public static DataTable SCELLES_RefEtatCompteur_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefEtatCompteur
                  select new
                  {
                      x.EtatCompteur_ID,
                      x.Libelle_ETAT,
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Scelle
        public static DataTable SCELLES_Scelle_RETOURNE_ByCentre(int IdCentre)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.Scelles
                  where x.agence_centre_Appartenance ==IdCentre && x.Status_ID == 3
                  select new
                  {
                      x.Status_ID,
                      x.Date_creation_scelle,
                      x.Origine_scelle,
                      x.provenance_Scelle_ID,
                      x.agence_centre_Origine,
                      x.lot_ID,
                      x.Couleur_Scelle,
                      x.agence_centre_Appartenance,
                      x.DateDePose,
                      x.DateDeRupture,
                      x.TypeOrganeScelle,
                      x.Reference_OrganeOuBranchementOuRaccordement_Scelle,
                      x.Id_Scelle,
                      x.Numero_Scelle,
                      x.CodeCentre,
                      Libelle_Couleur = x.RefCouleurlot.Couleur_libelle,
                      Libelle_Satut = x.RefStatus.Status,

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SCELLES_Scelle_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.Scelles
                  select new
                  {
                      x.Status_ID,
                      x.Date_creation_scelle,
                      x.Origine_scelle,
                      x.provenance_Scelle_ID,
                      x.agence_centre_Origine,
                      x.lot_ID,
                      x.Couleur_Scelle,
                      x.agence_centre_Appartenance,
                      x.DateDePose,
                      x.DateDeRupture,
                      x.TypeOrganeScelle,
                      x.Reference_OrganeOuBranchementOuRaccordement_Scelle,
                      x.Id_Scelle,
                      x.Numero_Scelle,
                      x.CodeCentre,
                      Libelle_Couleur=x.RefCouleurlot.Couleur_libelle,
                      Libelle_Satut = x.RefStatus.Status,
                     
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SCELLES_Scelle_RETOURNEById(Guid Id_Scelle)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.Scelles
                     where
                       x.Id_Scelle == Id_Scelle
                  select new
                  {
                      x.Status_ID,
                      x.Date_creation_scelle,
                      x.Origine_scelle,
                      x.provenance_Scelle_ID,
                      x.agence_centre_Origine,
                      x.lot_ID,
                      x.Couleur_Scelle,
                      x.agence_centre_Appartenance,
                      x.DateDePose,
                      x.DateDeRupture,
                      x.TypeOrganeScelle,
                      x.Reference_OrganeOuBranchementOuRaccordement_Scelle,
                      x.Id_Scelle,
                      x.Numero_Scelle,
                      x.CodeCentre,
                       Libelle_Couleur=x.RefCouleurlot.Couleur_libelle,
                      Libelle_Satut = x.RefStatus.Status,


                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Marque_Modele
        public static DataTable SCELLES_Marque_Modele_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.MARQUE_MODELE
                  select new
                  {
                      x.PK_ID,
                      x.MODELE_ID,
                      Libelle_Modele = x.MODELE.Libelle_Modele ,
                      x.MARQUE_ID,
                      Libelle_MArque = x.MARQUECOMPTEUR.LIBELLE ,
                      x.Nbre_scel_capot,
                      x.Nbre_scel_cache,
                      x.Produit_ID,
                      Libelle_Produit = x.PRODUIT.LIBELLE ,
                      CODE_Marque = x.MARQUECOMPTEUR.CODE
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLE_Marque_ModeleById(int id_marque)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.MARQUE_MODELE
                     where
                       x.MARQUE_ID == id_marque
                     select new
                     {
                         x.PK_ID,
                         x.MODELE_ID,
                         Libelle_Modele = x.MODELE.Libelle_Modele  ,
                         x.MARQUE_ID,
                         Libelle_MArque = x.MARQUECOMPTEUR.LIBELLE,
                         x.Nbre_scel_capot,
                         x.Nbre_scel_cache,
                         x.Produit_ID,
                         Libelle_Produit = x.PRODUIT.LIBELLE,
                         CODE_Marque = x.MARQUECOMPTEUR.CODE
                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        //#region selectionnner les scelles d un utilisateur
        //public static DataTable SCELLES_RemiseScelle_RETOURNE(int pk_ID) 
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null; 
        //             var _Scelles = context.Scelles;
        //             var _tbDetailRemiseScelles = context.tbDetailRemiseScelles;
        //            var _remiseScelle=context.tbRemiseScelles;
        //            query =
        //                  from p in _tbDetailRemiseScelles
        //                  join cn in _remiseScelle on p.Id_Remise equals cn.Id_Remise
        //                  join can in _Scelles on p.Id_Scelle equals can.Id_Scelle
        //                  where cn.Matricule_Receiver == pk_ID
        //             select new
        //             {
        //                 can.Numero_Scelle,
        //                 can.Status_ID,
        //                 cn.Matricule_Receiver,
        //                 cn.Nbre_Scelles,
        //                 cn.CodeCentre,
        //                 can.Id_Scelle
        //             };

        //            return Galatee.Tools.Utility.ListToDataTable(query);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        #region Motif 
        public static DataTable SCELLES_Motif_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.RefMotif
                  select new
                  {
                      x.Motif_ID,
                      x.Motif_libelle,
                      x.TypeInterventionDuMotif
                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Lot
        public static DataTable SCELLES_Lot_RETOURNE()
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                  from x in context.tbLot
                  select new
                  {
                      x.lot_ID,
                      x.DateReception,
                      x.typeReception,
                      x.Numero_depart,
                      x.Numero_fin,
                      x.Nombre_scelles_reçu,
                      x.Nombre_scelles_lot,
                      x.Origine_ID,
                      x.Status_lot_ID,
                      x.lot_Couleur_ID,
                      x.provenance_Scelle_ID,
                      x.Matricule_AgentModification,
                      x.Matricule_Creation,
                      x.agence_centre_Appartenance,
                      x.agence_centre_Origine,
                      x.Date_Derniere_Modif,
                      Libelle_Couleur = x.RefCouleurlot.Couleur_libelle,
                      Libelle_Satut = x.RefStatusLotScelle.Status,
                      Numero_lot=x.Numero_depart+""+x.Numero_fin

                  };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable SCELLES_Lot_RETOURNEById(string pIdLot)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query =
                     from x in context.tbLot
                     where
                       x.lot_ID == pIdLot
                     select new
                     {
                         x.lot_ID,
                         x.DateReception,
                         x.typeReception,
                         x.Numero_depart,
                         x.Numero_fin,
                         x.Nombre_scelles_reçu,
                         x.Nombre_scelles_lot,
                         x.Origine_ID,
                         x.lot_Couleur_ID,
                         x.provenance_Scelle_ID,
                         x.Matricule_AgentModification,
                         x.Matricule_Creation,
                         x.agence_centre_Appartenance,
                         x.agence_centre_Origine,
                         x.Date_Derniere_Modif,
                         Libelle_Couleur = x.RefCouleurlot.Couleur_libelle,
                         Libelle_Satut = x.RefStatusLotScelle.Status,


                     };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Sylla

        //public static DataTable RetourneListeScelle(int IdCentreRecuperationDeLot)
        //{
        //    //cmd.CommandText = "SPX_GUI_RETOURNE_DABON";
        //    try
        //    {

        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            IEnumerable<object> query = null;
        //            List<tbLot> _SCELLE = new List<tbLot>();
        //            if (IdCentreRecuperationDeLot == 1)
        //            {
        //                return RetourneListeScelle();
        //            }
        //            else
        //            {
        //                _SCELLE = context.tbLot.Where(l => l.CENTRE1.PK_ID == IdCentreRecuperationDeLot).ToList();
        //                query =
        //                from p in _SCELLE

        //                where
        //                    p.Status_lot_ID == 0
        //                //select p;
        //                select new
        //                {
        //                    Id_LotMagasinGeneral = p.lot_ID,
        //                    p.DateReception,
        //                    Matricule_AgentReception = p.Matricule_Creation,
        //                    p.Numero_depart,
        //                    p.Numero_fin,
        //                    Nbre_Scelles = p.Nombre_scelles_lot,
        //                    StatutLot_ID = p.Status_lot_ID,
        //                    p.Origine_ID,
        //                    CodeCentre = p.CENTRE1.PK_ID,
        //                    Couleur_ID = p.lot_Couleur_ID,
        //                    //p.Fournisseur_ID,
        //                    Date_DerniereModif = p.Date_Derniere_Modif,
        //                    Matricule_AgentDerniereModif = p.Matricule_AgentModification
        //                    //,p.Id_Affectation,
        //                    //p.Activite_ID,
        //                };
        //            }
        //            return Galatee.Tools.Utility.ListToDataTable(query);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        #endregion

        #region Ludovic 050416
                
        public static DataTable RetourneListScelleByStatus(int pk_ID,int Status)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Scelles = context.Scelles;
                    var _tbDetailRemiseScelles = context.tbDetailRemiseScelles;
                    var _remiseScelle = context.tbRemiseScelles;
                    query =
                          from p in _tbDetailRemiseScelles
                          join cn in _remiseScelle on p.Id_Remise equals cn.Id_Remise
                          join can in _Scelles on p.Id_Scelle equals can.Id_Scelle
                          where cn.Matricule_Receiver == pk_ID && can.Status_ID == Status 
                          select new
                          {
                              can.Numero_Scelle,
                              can.Status_ID,
                              libelle_Statut = can.RefStatus.Status,
                              cn.Matricule_Receiver,
                              cn.Nbre_Scelles,
                              cn.CodeCentre,
                              can.Id_Scelle,
                              cn.Date_Remise,
                              can.RefCouleurlot.Couleur_libelle 
                          };

                    return Galatee.Tools.Utility.ListToDataTable(query);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable RetourneListScelleByCentre(int idAgence)

        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                    IEnumerable<object> query = null;
                    var _Scelles = context.Scelles;
                    var _tbDetailRemiseScelles = context.tbDetailRemiseScelles;
                    var _remiseScelle = context.tbRemiseScelles;
                    query =
                          from p in _tbDetailRemiseScelles
                          join cn in _remiseScelle on p.Id_Remise equals cn.Id_Remise
                          where cn.CodeCentre == idAgence
                          select new
                          {
                              p.tbLot.lot_ID ,
                              p.tbLot.RefCouleurlot.Couleur_libelle ,
                              p.tbRemiseScelles.Date_Remise 
                          };
                    return Galatee.Tools.Utility.ListToDataTable(query);
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
