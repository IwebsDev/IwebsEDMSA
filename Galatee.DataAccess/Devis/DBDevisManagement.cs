using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Galatee.Structure;
//using Galatee.structure;
using System.Data.Linq;
using System.Data.Common;
using Galatee.Entity.Model;

namespace Galatee.DataAccess
{


    public class DBDevisManagement 
    {
        /*
         
        public static bool CreateDevisDemandeDevisDocumentsAndDeposit(ObjDEVIS pDevis, ObjDEMANDEDEVIS pDemandedevis,
                                                               ObjDOCUMENTSCANNE pAutorisation,
                                                               ObjDOCUMENTSCANNE pPreuve, ObjDEPOSIT pDeposit,
                                                               ObjMATRICULE pAgent, int pTailleClient, int idEtapeSuivi,List<ObjAPPAREILS> pListAppareils)
        {
            SqlCommand command = null;
            string numeroDevis = string.Empty;
            bool resultValidationTransaction = false;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                // Inserer le Devis 
                if (command != null)
                {
                    if (pDevis != null)
                    {
                        if (pPreuve != null)
                        {
                            pDevis.IdOwnerShip = pPreuve.Id;
                            //pDevis.IdEtapeDevis = DBETAPEDEVIS.GetByIdTypeDevisNumEtape(pDevis.IdTypeDevis, (int)Enumere.EtapeDevis.Accueil).Id;
                            // Inserer preuve scannée
                            var resultInsertPreuve = DBDOCUMENTSCANNE.InsertDocumentScanne(command, pPreuve);
                            if (resultInsertPreuve)
                            {
                                var resultInsertDevis = DBDEVIS.InsertDevis(pDevis, command, out numeroDevis);
                                if (resultInsertDevis)
                                {
                                    pDevis.NumDevis = numeroDevis;
                                    if (pDemandedevis != null)
                                    {
                                        pDemandedevis.NumDevis = numeroDevis;
                                        pDevis.OriginalNumDevis = numeroDevis;
                                        pDemandedevis.Client = int.Parse(numeroDevis).ToString().PadLeft(pTailleClient, '0');
                                        // Inserer DemandeDevis
                                        var resultInsertDemandeDevis = DBDEMANDEDEVIS.InsertDemandeDevis(pDemandedevis, command);
                                        if (resultInsertDemandeDevis)
                                        {
                                            pDeposit.CENTRE = pDevis.Centre;
                                            pDeposit.CLIENT = pDemandedevis.Client;
                                            pDeposit.ORDRE = pDemandedevis.OrdreClient;
                                            pDeposit.NOM = pDemandedevis.Nom;
                                            pDeposit.NUMDEVIS = numeroDevis;

                                            if (string.IsNullOrEmpty(pDeposit.RECEIPT))
                                            {
                                                if (pAutorisation != null)
                                                {
                                                    pDeposit.IDLETTER = pAutorisation.Id;
                                                    // Inserer Documents Scannés
                                                    DBDOCUMENTSCANNE.InsertDocumentScanne(command, pAutorisation);
                                                    // Inserer Deposit
                                                    DBDEPOSIT.InsertDeposit(command, pDeposit);
                                                }
                                            }
                                            else
                                                DBDEPOSIT.UpdateDeposit(command, pDeposit);

                                            // Insertion Appareils
                                            if (pListAppareils != null && pListAppareils.Count > 0 && pDevis.CodeProduit == Enumere.Electricite)
                                            {
                                                List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
                                                ObjAPPAREILSDEVIS AppareilDevis = null;
                                                foreach (ObjAPPAREILS appareil in pListAppareils)
                                                {
                                                    AppareilDevis = new ObjAPPAREILSDEVIS();
                                                    AppareilDevis.CodeAppareil = appareil.CodeAppareil;
                                                    AppareilDevis.Nbre = appareil.Nombre;
                                                    AppareilDevis.Puissance = appareil.Puissance;
                                                    AppareilDevis.NumDevis = numeroDevis;
                                                    lAppareilsDevis.Add(AppareilDevis);
                                                }

                                                DBAPPAREILSDEVIS.Insert(lAppareilsDevis, command);
                                            }
                                            if (pDevis.IdEtapeDevis != null)
                                            {
                                                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(numeroDevis, (int)pDevis.IdEtapeDevis);
                                                DateTime? date, currentDate;
                                                currentDate = DateTime.Now.Date;
                                                TimeSpan difference = new TimeSpan();
                                                double delai;

                                                date = pDevis.DateEtape;
                                                if (date != null) difference = (currentDate.Value.Date - date.Value.Date);
                                                delai = difference.TotalDays;

                                                ObjSUIVIDEVIS newSuivi =
                                                    DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(numeroDevis, idEtapeSuivi);

                                                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                                                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.IdEtapeDevis);

                                                if (suivi != null && suivi.NumDevis == pDevis.NumDevis)
                                                {
                                                    if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                                        (pDevis.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                                         && !string.IsNullOrEmpty(pDevis.MotifRejet) &&
                                                         etape.NumEtape > next.NumEtape))
                                                    {
                                                        suivi.Commentaire += "\r\n* " + pDevis.MotifRejet;
                                                        // cas de rejet du devis
                                                    }

                                                    suivi.Duree = Convert.ToInt32(delai);
                                                    suivi.Agent = pAgent.Matricule;
                                                    DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                                                }
                                                else
                                                {
                                                    suivi = new ObjSUIVIDEVIS();
                                                    suivi.Duree = Convert.ToInt32(delai);
                                                    suivi.Agent = pAgent.Matricule;
                                                    suivi.NumDevis = pDevis.NumDevis;
                                                    suivi.IdEtape = idEtapeSuivi;
                                                    DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                                                }

                                                if (pDevis.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                                                {
                                                    suivi.Agent = null;
                                                    suivi.Commentaire = null;
                                                    suivi.Duree = null;
                                                    if (pDevis.IdEtapeDevis != null)
                                                        suivi.IdEtape = (int)pDevis.IdEtapeDevis;
                                                    DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                                                }
                                            }
                                            resultValidationTransaction = Transaction.CommitTransaction(command);
                                            pDevis.OriginalNumDevis = pDevis.NumDevis;
                                            pDemandedevis.OriginalNumDevis = pDevis.NumDevis;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static List<ObjDEVIS> GetDevisByEtapeDevisId(int pEtapeId)
        {
            try
            {
                return DBDEVIS.GetByIdEtapeDevis(pEtapeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDevisDemandeDevisAndDocuments(ObjDEVIS pDevis, ObjDEMANDEDEVIS pDemandedevis, ObjDOCUMENTSCANNE pAutorisation, ObjDOCUMENTSCANNE pPreuve, ObjDEPOSIT pDeposit, ObjMATRICULE pAgent, int idEtapeSuivi, List<ObjAPPAREILSDEVIS> pListAppareilsDevis)
        {
            SqlCommand command = null;
            bool resultValidationTransaction = false;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if(command != null)
                {
                    if (pDevis != null)
                    {

                        if (pAutorisation != null && pAutorisation.IsUpdate)
                            DBDOCUMENTSCANNE.UpdateDocumentScanne(command, pAutorisation);

                        if (pPreuve != null && pPreuve.IsUpdate)
                            DBDOCUMENTSCANNE.UpdateDocumentScanne(command, pPreuve);

                        // Cas ou le deposit est modifié 
                        if (pAutorisation != null && pAutorisation.IsToRemove)
                        {
                            pDeposit.IDLETTER = null;
                            DBDEPOSIT.UpdateDeposit(command, pDeposit);
                            DBDOCUMENTSCANNE.DeleteDocumentScanne(command, pAutorisation);
                        }
                        // Mise à jour des appareils devis
                        if (pListAppareilsDevis != null && pListAppareilsDevis.Count > 0)
                        {
                            if (DBAPPAREILSDEVIS.Delete(pDevis.NumDevis, command))
                                DBAPPAREILSDEVIS.Insert(pListAppareilsDevis, command);
                        }

                        var resultUpdateDevis = DBDEVIS.Update(command, pDevis);
                        if (resultUpdateDevis && pDemandedevis != null)
                        {
                            var resultUpdateDemandeDevis = DBDEMANDEDEVIS.Update(pDemandedevis, command);
                            if (resultUpdateDemandeDevis)
                            {


                                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(pDevis.NumDevis, idEtapeSuivi);
                                DateTime date = new DateTime(), currentDate;
                                currentDate = DateTime.Now.Date;
                                TimeSpan difference;
                                double delai;

                                if (pDevis.DateEtape != null) date = (DateTime)pDevis.DateEtape;
                                difference = (currentDate.Date - date.Date);
                                delai = difference.TotalDays;

                                if (pDevis.IdEtapeDevis != null)
                                {
                                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(pDevis.NumDevis, (int)pDevis.IdEtapeDevis);

                                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.IdEtapeDevis);

                                    if (suivi != null && suivi.NumDevis == pDevis.NumDevis)
                                    {
                                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                            (pDevis.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                             && !string.IsNullOrEmpty(pDevis.MotifRejet) && etape.NumEtape > next.NumEtape))
                                        {
                                            suivi.Commentaire += "\r\n* " + pDevis.MotifRejet; // cas de rejet du devis
                                        }

                                        suivi.Duree = Convert.ToInt32(delai);
                                        suivi.Agent = pAgent.Matricule;
                                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                                    }
                                    else
                                    {
                                        suivi = new ObjSUIVIDEVIS();
                                        suivi.Duree = Convert.ToInt32(delai);
                                        suivi.Agent = pAgent.Matricule;
                                        suivi.NumDevis = pDevis.NumDevis;
                                        suivi.IdEtape = idEtapeSuivi;
                                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                                    }

                                    if (pDevis.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                                    {
                                        suivi.Agent = null;
                                        suivi.Commentaire = null;
                                        suivi.Duree = null;
                                        if (pDevis.IdEtapeDevis != null) suivi.IdEtape = (int)pDevis.IdEtapeDevis;
                                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                                    }
                                }
                            }
                        }
                        resultValidationTransaction = Transaction.CommitTransaction(command);
                    }
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static CsInformationsDevis GetDemandeDevisForUpdateOrConsult(CsCriteresDevis pCriteresDevis)
        {
            CsInformationsDevis InformationsDevis = null;
            try
            {
                if (pCriteresDevis != null && !string.IsNullOrEmpty(pCriteresDevis.NumeroDevis))
                {
                    InformationsDevis = new CsInformationsDevis();
                    // Get informations of Devis
                    InformationsDevis.Devis = DBDEVIS.GetByNumDevis(pCriteresDevis.NumeroDevis);

                    if (InformationsDevis.Devis != null)
                    {
                        // Get informations of Etape suivante 
                        InformationsDevis.EtapeDevisSuivante = DBService.GetEtapeSuivante(InformationsDevis.Devis);

                        // Get informations of Etape suivante 
                        InformationsDevis.EtapeCourante = DBETAPEDEVIS.GetById(InformationsDevis.Devis.IdEtapeDevis);

                        // Get informations of Etape Rejet 
                        InformationsDevis.EtapeRejet = DBService.GetEtapeRejet(InformationsDevis.Devis);

                        // Get informations of Etape intermédiaire 
                        InformationsDevis.EtapeIntermediaire = DBService.GetEtapeIntermediaire(InformationsDevis.Devis);

                        // Get informations of Travaux
                        InformationsDevis.TravauxDevis = DBTRAVAUXDEVIS.SelectTravaux(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre);

                        // Get informations of ChefEquipe
                        if (InformationsDevis.TravauxDevis != null)
                            InformationsDevis.ChefEquipe = new DBAdmUsers().GetByIdUser(InformationsDevis.TravauxDevis.MatriculeChefEquipe);

                        // Get informations of Travaux
                        InformationsDevis.ControleTravaux = DBCONTROLETRAVAUX.SelectControles(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre);

                        // Get informations of DemandeDevis
                        InformationsDevis.DemandeDevis = DBDEMANDEDEVIS.GetByNumDevis(InformationsDevis.Devis.NumDevis);

                        // Get informations of Deposit
                        InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(pCriteresDevis.NumeroDevis);

                        // Get informations of Appareils
                        InformationsDevis.ListAppareilsDevis = DBAPPAREILSDEVIS.GetByNumDevis(pCriteresDevis.NumeroDevis);

                        // Get informations of ElementDevis
                        InformationsDevis.ListElementsDevis = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre,true);

                        // Get informations of detailsElementsDevis
                        InformationsDevis.ListDetailsElementsDevis = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre, false);

                        // Get informations of ElementsDevisForValidationRemiseStock
                        InformationsDevis.ListElementsDevisForValidationRemiseStock = DBELEMENTSDEVIS.SelectElementsDevisByNumDevisForValidationRemiseStock(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre, false);
                    }

                    // Get informations document autorisation scannée
                    if (InformationsDevis.Deposit != null && (Guid)InformationsDevis.Deposit.IDLETTER != Guid.Empty)
                        InformationsDevis.DocumentAutorisation = DBDOCUMENTSCANNE.GetDocumentScanneById((Guid)InformationsDevis.Deposit.IDLETTER);

                    // Get informations document preuve scannée
                    if (pCriteresDevis.IdDocumentPreuvePropriete != null && (Guid)pCriteresDevis.IdDocumentPreuvePropriete != Guid.Empty)
                        InformationsDevis.DocumentPreuvePropriete = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentPreuvePropriete);

                    // Get informations document schema
                    if (pCriteresDevis.IdDocumentSchema != null && (Guid)pCriteresDevis.IdDocumentSchema != Guid.Empty)
                        InformationsDevis.DocumentSchema = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentSchema);

                    // Get informations document manuscrit
                    if (pCriteresDevis.IdDocumentManuscrit != null && (Guid)pCriteresDevis.IdDocumentManuscrit != Guid.Empty)
                        InformationsDevis.DocumentManuscrit = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentManuscrit);
                }
                return InformationsDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CsInformationsDevis GetDevisControleTravauxDepositAndAmountPaid(string pNumDevis)
        {
            CsInformationsDevis InformationsDevis = null;
            try
            {
                if (!string.IsNullOrEmpty(pNumDevis))
                {
                     InformationsDevis = new CsInformationsDevis();
                    // Get informations of Devis
                     InformationsDevis.Devis = DBDEVIS.GetByNumDevis(pNumDevis);

                    if (InformationsDevis.Devis != null)
                    {
                        // Get informations of Etape suivante 
                        InformationsDevis.EtapeCourante = DBETAPEDEVIS.GetById(InformationsDevis.Devis.IdEtapeDevis);
                        // Get informations of Controle Travaux
                        InformationsDevis.ControleTravaux = DBCONTROLETRAVAUX.SelectControles(InformationsDevis.Devis.NumDevis, (byte)InformationsDevis.Devis.Ordre);
                        // Get informations of Deposit
                        InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(InformationsDevis.Devis.NumDevis);
                        // Get Amount paid
                       InformationsDevis.AmountPaid = DBService.GetAmountPaid(InformationsDevis.Devis.NumDevis);
                    }
                }
                return InformationsDevis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDevis(ObjDEVIS entity, CsUserConnecte agent)
        {
            SqlCommand command = null;
            bool resultUpdateDevis = false;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (entity != null && agent != null)
                    {
                        ObjETAPEDEVIS nextEtape = new ObjETAPEDEVIS();
                        entity.DateEtape = DateTime.Now;
                        nextEtape = DBService.GetEtapeSuivante(entity);
                        int idEtapeSuivi = (int)entity.IdEtapeDevis;
                        entity.IdEtapeDevis = nextEtape.Id;

                        DBDEVIS.Update(command, entity);
                        ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                        DateTime date, currentDate;
                        currentDate = DateTime.Now.Date;
                        TimeSpan difference;
                        double delai;

                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;

                        ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                        ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                        ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                        if (suivi != null && suivi.NumDevis == entity.NumDevis)
                        {
                            if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                            {
                                suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                            }

                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                        }
                        else
                        {
                            suivi = new ObjSUIVIDEVIS();
                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            suivi.NumDevis = entity.NumDevis;
                            suivi.IdEtape = idEtapeSuivi;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }
                        if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                        {
                            suivi.Agent = null;
                            suivi.Commentaire = null;
                            suivi.Duree = null;
                            suivi.IdEtape = (int)entity.IdEtapeDevis;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }
                        Transaction.CommitTransaction(command);
                        resultUpdateDevis = true;
                        entity.OriginalNumDevis = entity.NumDevis;
                    }
                }
                return resultUpdateDevis;
            }
            catch (Exception ex)
            {
                Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevis(ObjDEVIS entity, CsUserConnecte agent, int idEtapeSuivi)
        {
            SqlCommand command = null;
            bool resultUpdateDevis = false;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (entity != null && agent != null)
                    {
                        DBDEVIS.Update(command, entity);
                        ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                        DateTime date, currentDate;
                        currentDate = DateTime.Now.Date;
                        TimeSpan difference;
                        double delai;

                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;

                        ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                        ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                        ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                        if (suivi != null && suivi.NumDevis == entity.NumDevis)
                        {
                            if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                            {
                                suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                            }

                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                        }
                        else
                        {
                            suivi = new ObjSUIVIDEVIS();
                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            suivi.NumDevis = entity.NumDevis;
                            suivi.IdEtape = idEtapeSuivi;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }
                        if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                        {
                            suivi.Agent = null;
                            suivi.Commentaire = null;
                            suivi.Duree = null;
                            suivi.IdEtape = (int)entity.IdEtapeDevis;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }
                        Transaction.CommitTransaction(command);
                        resultUpdateDevis = true;
                        entity.OriginalNumDevis = entity.NumDevis;
                    }
                }
                return resultUpdateDevis;
            }
            catch (Exception ex)
            {
                Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool AffecterDevis(ObjDEVIS entity, ObjDEVISPIA pia, CsUserConnecte agent,bool pIsReaffecter)
        {
            SqlCommand command = null;
            ObjETAPEDEVIS nextEtape = null;
            bool resultUpdateDevis = false;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (entity != null && agent != null && pia != null)
                    {
                        entity.DateEtape = DateTime.Now;
                        if (!pIsReaffecter)
                            nextEtape = DBService.GetEtapeSuivante(entity);
                        int idEtapeSuivi = (int)entity.IdEtapeDevis;
                        if (nextEtape != null)
                            entity.IdEtapeDevis = nextEtape.Id;

                        DBDEVIS.Update(command, entity);

                        DBDEVISPIA.Delete(command, entity);
                        DBDEVISPIA.Insert(command, pia);

                        ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                        DateTime date, currentDate;
                        currentDate = DateTime.Now.Date;
                        TimeSpan difference;
                        double delai;

                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;

                        ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                        ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                        ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                        if (suivi != null && suivi.NumDevis == entity.NumDevis)
                        {
                            if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                            {
                                suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                            }

                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                        }
                        else
                        {
                            suivi = new ObjSUIVIDEVIS();
                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.matricule;
                            suivi.NumDevis = entity.NumDevis;
                            suivi.IdEtape = idEtapeSuivi;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }

                        if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                        {
                            suivi.Agent = null;
                            suivi.Commentaire = null;
                            suivi.Duree = null;
                            suivi.IdEtape = (int)entity.IdEtapeDevis;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }

                        entity.OriginalNumDevis = entity.NumDevis;
                        Transaction.CommitTransaction(command);
                        resultUpdateDevis = true;
                    }
                }
                return resultUpdateDevis;
            }
            catch (Exception ex)
            {
                Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention)
        {
            CsCriteresDevis ParametresDistance = new CsCriteresDevis();
            try
            {
                DB_ParametresGeneraux dbParamGeneraux = new DB_ParametresGeneraux();
                if (!string.IsNullOrEmpty(pMaxi))
                {
                    var ParamMaxi = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxi);
                    if (ParamMaxi != null)
                        ParametresDistance.Maxi = decimal.Parse(ParamMaxi.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pSeuil))
                {
                    var ParamSeuil = dbParamGeneraux.SelectParametresGenerauxByCode(pSeuil);
                    if (ParamSeuil != null)
                        ParametresDistance.Seuil = decimal.Parse(ParamSeuil.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pMaxiSubvention))
                {
                    var ParamMaxiSubvention = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxiSubvention);
                    if (ParamMaxiSubvention != null)
                        ParametresDistance.MaxiSubvention = decimal.Parse(ParamMaxiSubvention.LIBELLE);
                }
                return ParametresDistance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateElementsDevis(ObjDEMANDEDEVIS pDemandedevis, ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjDOCUMENTSCANNE doc, ObjDOCUMENTSCANNE manuscrit, ObjMATRICULE agent, int idEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (_lElements != null)
                        DBELEMENTSDEVIS.DeleteElementsDevis(command, entity.NumDevis, (byte)entity.Ordre);

                    if ((entity.IdSchema != null && (Guid)entity.IdSchema != Guid.Empty) || (entity.IdManuscrit != null && entity.IdManuscrit != Guid.Empty))
                    {
                        if (doc.IsNew)
                            DBDOCUMENTSCANNE.InsertDocumentScanne(command, doc);
                        else
                            DBDOCUMENTSCANNE.UpdateDocumentScanne(command, doc);

                        if (manuscrit.IsNew)
                            DBDOCUMENTSCANNE.InsertDocumentScanne(command, manuscrit);
                        else
                            DBDOCUMENTSCANNE.UpdateDocumentScanne(command, manuscrit);

                        var resultUpdateDevis = DBDEVIS.Update(command, entity);
                        if (resultUpdateDevis && pDemandedevis != null)
                            DBDEMANDEDEVIS.Update(pDemandedevis, command);
                    }
                    else
                    {
                        var resultUpdateDevis = DBDEVIS.Update(command, entity);
                        if (resultUpdateDevis && pDemandedevis != null)
                            DBDEMANDEDEVIS.Update(pDemandedevis, command);
                        if (doc.IsToRemove)
                            DBDOCUMENTSCANNE.DeleteDocumentScanne(command, doc);

                        if (manuscrit.IsToRemove)
                            DBDOCUMENTSCANNE.DeleteDocumentScanne(command, manuscrit);
                    }


                    if (_lElements != null && _lElements.Count > 0)
                        DBELEMENTSDEVIS.InsertElementsDevis(_lElements, command);

                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai = 0;
                    if (entity.DateEtape != null)
                    {
                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;
                    }
                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                    if (suivi != null && suivi.NumDevis == entity.NumDevis)
                    {
                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                            (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                            && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                        {
                            suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                        }

                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                    }
                    else
                    {
                        suivi = new ObjSUIVIDEVIS();
                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        suivi.NumDevis = entity.NumDevis;
                        suivi.IdEtape = idEtapeSuivi;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                    {
                        suivi.Agent = null;
                        suivi.Commentaire = null;
                        suivi.Duree = null;
                        suivi.IdEtape = (int)entity.IdEtapeDevis;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
               return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisElementsDocument(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjDOCUMENTSCANNE doc, ObjMATRICULE agent, int idEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;

            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (_lElements != null)
                        DBELEMENTSDEVIS.DeleteElementsDevis(command, entity.NumDevis, (byte)entity.Ordre);

                    if (entity.IdSchema != null && entity.IdSchema != Guid.Empty)
                    {
                        if (doc.IsNew)
                            DBDOCUMENTSCANNE.InsertDocumentScanne(command, doc);
                        else
                            DBDOCUMENTSCANNE.UpdateDocumentScanne(command, doc);
                        DBDEVIS.Update(command, entity); //Position liée aux contraintes d'intégrité
                    }
                    else
                    {
                        DBDEVIS.Update(command, entity); //Position liée aux contraintes d'intégrité
                        if (doc.OriginalId != Guid.Empty && doc.IsToRemove)
                            DBDOCUMENTSCANNE.DeleteDocumentScanne(command, doc);
                    }


                    if (_lElements != null)
                        DBELEMENTSDEVIS.InsertElementsDevis(_lElements, command);

                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai = 0;
                    if (entity.DateEtape != null)
                    {
                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;
                    }

                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                    if (suivi != null && suivi.NumDevis == entity.NumDevis)
                    {
                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                            (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                            && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                        {
                            suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                        }

                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                    }
                    else
                    {
                        suivi = new ObjSUIVIDEVIS();
                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        suivi.NumDevis = entity.NumDevis;
                        suivi.IdEtape = idEtapeSuivi;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                    {
                        suivi.Agent = null;
                        suivi.Commentaire = null;
                        suivi.Duree = null;
                        suivi.IdEtape = (int)entity.IdEtapeDevis;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisValidationEtude(List<ObjDEVIS> pListeDevis)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    ObjETAPEDEVIS etape;
                    foreach (ObjDEVIS _devis in pListeDevis)
                    {
                        var DevisEnBase = DBDEVIS.GetByNumDevis(_devis.NumDevis);
                        DevisEnBase.IsPose = _devis.IsPose;
                        DevisEnBase.IsFourniture = _devis.IsFourniture;
                        DevisEnBase.DateEtape = DateTime.Now;

                        ObjETAPEDEVIS next = DBService.GetEtapeSuivante(DevisEnBase);
                        DevisEnBase.IdEtapeDevis = next.Id;

                        etape = new ObjETAPEDEVIS();
                        etape = DBETAPEDEVIS.GetById(DevisEnBase.IdEtapeDevis);
                        command.Parameters.Clear();
                        DBDEVIS.Update(command, DevisEnBase);
                        command.ExecuteNonQuery();
                        if (etape.NumEtape == (int)Enumere.EtapeDevis.Encaissement)
                        {
                            ObjDEPOSIT deposit = DBDEPOSIT.SearchByNumDevis(DevisEnBase.NumDevis);
                            if (deposit != null)
                            {
                                deposit.TOTAL = (decimal)DevisEnBase.MontantTTC;
                                deposit.MONTANTTVA = (decimal)(DevisEnBase.MontantTTC - DevisEnBase.MontantHT);
                                DBDEPOSIT.UpdateDeposit(command, deposit);
                            }
                        }
                    }
                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
               return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisValidationDossier(List<ObjDEVIS> pListeDevis)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    ObjETAPEDEVIS etape;
                    foreach (ObjDEVIS _devis in pListeDevis)
                    {
                        etape = new ObjETAPEDEVIS();
                        etape = DBETAPEDEVIS.GetById(_devis.IdEtapeDevis);
                        command.Parameters.Clear();
                        DBDEVIS.Update(command, _devis);
                        if (etape.NumEtape == (int)Enumere.EtapeDevis.Encaissement)
                        {
                            ObjDEPOSIT deposit = DBDEPOSIT.SearchByNumDevis(_devis.NumDevis);
                            if (deposit != null)
                            {
                                deposit.TOTAL = (decimal)_devis.MontantTTC;
                                deposit.MONTANTTVA = (decimal)(_devis.MontantTTC - _devis.MontantHT);
                                DBDEPOSIT.UpdateDeposit(command, deposit);
                            }
                        }
                    }
                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static List<CsEtatBonDeSortie> EditerDevisPourBonDeSortie(string pNumDevis, byte pOrdre, bool pIsSummary,string pMatricule)
        {
            try
            {
                return DBETATSDEVIS.EditerDevisPourBonDeSortie(pNumDevis, pOrdre, pIsSummary,pMatricule);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEtatBonTravaux> EditerDevisPourBonTravaux(string pNumDevis, byte pOrdre)
        {
            try
            {
                return DBETATSDEVIS.EditerDevisPourBonTravaux(pNumDevis, pOrdre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDevisValidation(ObjDEVIS pDevis, ObjDEPOSIT pDeposit, ObjMATRICULE pAgent, int pIdEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    ObjETAPEDEVIS etape = new ObjETAPEDEVIS();

                    etape = DBETAPEDEVIS.GetById(pDevis.IdEtapeDevis);
                    command.Parameters.Clear();
                    DBDEVIS.Update(command, pDevis);
                    command.ExecuteNonQuery();
                    if (etape.NumEtape == (int)Enumere.EtapeDevis.Encaissement)
                    {
                        pDeposit.TOTAL = (decimal)pDevis.MontantTTC;
                        pDeposit.MONTANTTVA = (decimal)(pDevis.MontantTTC - pDevis.MontantHT);
                        DBDEPOSIT.UpdateDeposit(command, pDeposit);
                    }

                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(pDevis.NumDevis, pIdEtapeSuivi);
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai;

                    date = (DateTime)pDevis.DateEtape;
                    difference = (currentDate.Date - date.Date);
                    delai = difference.TotalDays;

                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(pDevis.NumDevis, (int)pDevis.IdEtapeDevis);

                    ObjETAPEDEVIS step = DBETAPEDEVIS.GetById(pIdEtapeSuivi);
                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.IdEtapeDevis);

                    if (suivi != null && suivi.NumDevis == pDevis.NumDevis)
                    {
                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                            (pDevis.IdEtapeDevis != pIdEtapeSuivi && newSuivi != null
                            && !string.IsNullOrEmpty(pDevis.MotifRejet) && step.NumEtape > next.NumEtape))
                        {
                            suivi.Commentaire += "\r\n* " + pDevis.MotifRejet; // cas de rejet du devis
                        }

                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = pAgent.Matricule;
                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                    }
                    else
                    {
                        suivi = new ObjSUIVIDEVIS();
                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = pAgent.Matricule;
                        suivi.NumDevis = pDevis.NumDevis;
                        suivi.IdEtape = pIdEtapeSuivi;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    if (pDevis.IdEtapeDevis != pIdEtapeSuivi && newSuivi == null)
                    {
                        suivi.Agent = null;
                        suivi.Commentaire = null;
                        suivi.Duree = null;
                        suivi.IdEtape = (int)pDevis.IdEtapeDevis;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;

            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisTravaux(ObjDEVIS entity, ObjTRAVAUXDEVIS travaux, bool isChefExists, bool isElementsNeeded, ObjMATRICULE agent, int idEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    command.Parameters.Clear();
                    DBDEVIS.Update(command, entity);

                    if (isChefExists)
                        DBTRAVAUXDEVIS.UpdateTravaux(command, travaux);
                    else
                        DBTRAVAUXDEVIS.InsertTravaux(command, travaux);
                    if (isElementsNeeded)
                    {
                        //byte ordre = _devis.Ordre;
                        //ordre--;

                        //Si le devis est ramené à "Etablissement procès verbal" pour reprise des travaux sans modification du devis,
                        //1- il faut marquer l'ordre précédent (dans TRAVAUXDEVIS) pour ne pas en tenir compte dans le bilan final de remboursement
                        ObjTRAVAUXDEVIS oldTravaux = DBTRAVAUXDEVIS.SelectTravaux(entity.NumDevis, (byte)entity.Ordre);
                        if ((oldTravaux != null) && (!string.IsNullOrEmpty(oldTravaux.MatriculeChefEquipe)))
                        {
                            oldTravaux.IsUsedForBilan = false;
                            DBTRAVAUXDEVIS.UpdateTravaux(command, oldTravaux);
                        }

                        //2- ensuite il faut recopier les éléments du devis et les mettre à l'ordre courant (le nouvel ordre) 
                        //   en gardant une copie pour l'ordre précédent
                        List<ObjELEMENTDEVIS> elements = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(entity.NumDevis, (byte)entity.Ordre, true);
                        List<ObjELEMENTDEVIS> elements2 = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(entity.NumDevis, (byte)entity.Ordre, false);

                        List<ObjELEMENTDEVIS> newElements = new List<ObjELEMENTDEVIS>();
                        if ((elements != null) && (elements.Count > 0))
                        {
                            ObjELEMENTDEVIS elt;
                            foreach (ObjELEMENTDEVIS st in elements)
                            {
                                elt = new ObjELEMENTDEVIS();
                                elt = st;
                                //elt.Ordre++;
                                newElements.Add(elt);
                            }
                        }

                        if ((elements2 != null) && (elements2.Count > 0))
                        {
                            ObjELEMENTDEVIS elt;
                            foreach (ObjELEMENTDEVIS st in elements2)
                            {
                                elt = new ObjELEMENTDEVIS();
                                elt = st;
                                //elt.Ordre++;
                                newElements.Add(elt);
                            }
                        }
                        DBELEMENTSDEVIS.InsertElementsDevis(newElements, command);

                    }



                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai;

                    date = (DateTime)entity.DateEtape;
                    difference = (currentDate.Date - date.Date);
                    delai = difference.TotalDays;

                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                    if (suivi != null && suivi.NumDevis == entity.NumDevis)
                    {
                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                            (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                            && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                        {
                            suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                        }

                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                    }
                    else
                    {
                        suivi = new ObjSUIVIDEVIS();
                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        suivi.NumDevis = entity.NumDevis;
                        suivi.IdEtape = idEtapeSuivi;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                    {
                        suivi.Agent = null;
                        suivi.Commentaire = null;
                        suivi.Duree = null;
                        suivi.IdEtape = (int)entity.IdEtapeDevis;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisProcesVerbal(ObjDEMANDEDEVIS _dem, ObjDEVIS entity, ObjTRAVAUXDEVIS travaux, bool isChefExists, bool isElementsNeeded, ObjMATRICULE agent, int idEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;
            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    command.Parameters.Clear();
                    if (isChefExists)
                        DBTRAVAUXDEVIS.UpdateTravaux(command, travaux);
                    else
                        DBTRAVAUXDEVIS.InsertTravaux(command, travaux);
                    if (isElementsNeeded)
                    {
                        //byte ordre = _devis.Ordre;
                        //ordre--;

                        //Si le devis est ramené à "Etablissement procès verbal" pour reprise des travaux sans modification du devis,
                        //1- il faut marquer l'ordre précédent (dans TRAVAUXDEVIS) pour ne pas en tenir compte dans le bilan final de remboursement
                        ObjTRAVAUXDEVIS oldTravaux = DBTRAVAUXDEVIS.SelectTravaux(entity.NumDevis, (byte)entity.Ordre);
                        if ((oldTravaux != null) && (!string.IsNullOrEmpty(oldTravaux.MatriculeChefEquipe)))
                        {
                            oldTravaux.IsUsedForBilan = false;
                            DBTRAVAUXDEVIS.UpdateTravaux(command, oldTravaux);
                        }

                        //2- ensuite il faut recopier les éléments du devis et les mettre à l'ordre courant (le nouvel ordre) 
                        //   en gardant une copie pour l'ordre précédent
                        List<ObjELEMENTDEVIS> elements = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(entity.NumDevis, (byte)entity.Ordre, true);
                        List<ObjELEMENTDEVIS> elements2 = DBELEMENTSDEVIS.SelectElementsDevisByNumDevis(entity.NumDevis, (byte)entity.Ordre, false);

                        List<ObjELEMENTDEVIS> newElements = new List<ObjELEMENTDEVIS>();
                        if ((elements != null) && (elements.Count > 0))
                        {
                            ObjELEMENTDEVIS elt;
                            foreach (ObjELEMENTDEVIS st in elements)
                            {
                                elt = new ObjELEMENTDEVIS();
                                elt = st;
                                //elt.Ordre++;
                                newElements.Add(elt);
                            }
                        }

                        if ((elements2 != null) && (elements2.Count > 0))
                        {
                            ObjELEMENTDEVIS elt;
                            foreach (ObjELEMENTDEVIS st in elements2)
                            {
                                elt = new ObjELEMENTDEVIS();
                                elt = st;
                                //elt.Ordre++;
                                newElements.Add(elt);
                            }
                        }
                        DBELEMENTSDEVIS.InsertElementsDevis(newElements, command);
                    }

                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                    DateTime date, currentDate;
                    currentDate = DateTime.Now.Date;
                    TimeSpan difference;
                    double delai;

                    date = (DateTime)entity.DateEtape;
                    difference = (currentDate.Date - date.Date);
                    delai = difference.TotalDays;

                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                    if (suivi != null && suivi.NumDevis == entity.NumDevis)
                    {
                        if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                            (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                            && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                        {
                            suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                        }

                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                    }
                    else
                    {
                        suivi = new ObjSUIVIDEVIS();
                        suivi.Duree = Convert.ToInt32(delai);
                        suivi.Agent = agent.Matricule;
                        suivi.NumDevis = entity.NumDevis;
                        suivi.IdEtape = idEtapeSuivi;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                    {
                        suivi.Agent = null;
                        suivi.Commentaire = null;
                        suivi.Duree = null;
                        suivi.IdEtape = (int)entity.IdEtapeDevis;
                        DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                    }

                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static List<CsEtatProcesVerbal> EditerDevisPourProcesVerbal(string pNumDevis, byte pOrdre)
        {
            try
            {
                return DBETATSDEVIS.EditerDevisPourProcesVerbal(pNumDevis, pOrdre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CsEtatProcesVerbal> EditerDevisPourBonControle(string pNumDevis, byte pOrdre)
        {
            try
            {
                return DBETATSDEVIS.EditerDevisPourBonControle(pNumDevis, pOrdre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool UpdateDevisValidation(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjMATRICULE agent, int idEtapeSuivi)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;


            try
            {
                command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                if (command != null)
                {
                    if (DBELEMENTSDEVIS.UpdateElementsDevisValide(_lElements, command))
                    {
                        ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, idEtapeSuivi);
                        DateTime date, currentDate;
                        currentDate = DateTime.Now.Date;
                        TimeSpan difference;
                        double delai;

                        date = (DateTime)entity.DateEtape;
                        difference = (currentDate.Date - date.Date);
                        delai = difference.TotalDays;

                        ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByNumDevisEtape(entity.NumDevis, (int)entity.IdEtapeDevis);

                        ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                        ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.IdEtapeDevis);

                        if (suivi != null && suivi.NumDevis == entity.NumDevis)
                        {
                            if (next.NumEtape == (int)Enumere.EtapeDevis.Annulé ||
                                (entity.IdEtapeDevis != idEtapeSuivi && newSuivi != null
                                && !string.IsNullOrEmpty(entity.MotifRejet) && etape.NumEtape > next.NumEtape))
                            {
                                suivi.Commentaire += "\r\n* " + entity.MotifRejet; // cas de rejet du devis
                            }

                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.Matricule;
                            DBSUIVIDEVIS.UpdateSuiviDevis(command, suivi);
                        }
                        else
                        {
                            suivi = new ObjSUIVIDEVIS();
                            suivi.Duree = Convert.ToInt32(delai);
                            suivi.Agent = agent.Matricule;
                            suivi.NumDevis = entity.NumDevis;
                            suivi.IdEtape = idEtapeSuivi;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }

                        if (entity.IdEtapeDevis != idEtapeSuivi && newSuivi == null)
                        {
                            suivi.Agent = null;
                            suivi.Commentaire = null;
                            suivi.Duree = null;
                            suivi.IdEtape = (int)entity.IdEtapeDevis;
                            DBSUIVIDEVIS.InsertSuiviDevis(command, suivi);
                        }
                        resultValidationTransaction = Transaction.CommitTransaction(command);
                    }
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        public static bool UpdateDevisComplementaire(ObjDEVIS devis, ObjDEPOSIT deposit, List<ObjELEMENTDEVIS> elements)
        {
            bool resultValidationTransaction = false;
            SqlCommand command = null;

            try
            {
                if (command != null)
                {
                    command = Transaction.OpenTransaction(Enumere.DataBase.Galadb);
                    DBDEVIS.Update(command, devis);
                    command.ExecuteNonQuery();
                    DBDEPOSIT.UpdateDeposit(command, deposit);
                    DBELEMENTSDEVIS.InsertElementsDevis(elements, command);
                    resultValidationTransaction = Transaction.CommitTransaction(command);
                }
                return resultValidationTransaction;
            }
            catch (Exception ex)
            {
                if (!resultValidationTransaction)
                    Transaction.RollBackTransaction(command);
                throw ex;
            }
        }

        */

        //#region  DEVIS

        ////public static bool CreateDevisDemandeDevisDocumentsAndDeposit(ObjDEVIS pDevis, ObjDEMANDEDEVIS pDemandedevis,
        ////                                                       ObjDOCUMENTSCANNE pAutorisation,
        ////                                                       ObjDOCUMENTSCANNE pPreuve, ObjDEPOSIT pDeposit,
        ////                                                       ObjMATRICULE pAgent, int pTailleClient, int idEtapeSuivi,List<ObjAPPAREILS> pListAppareils)
        ////{
        ////    string numeroDevis ;
        ////    try
        ////    {
        ////        using (galadbEntities context = new galadbEntities())
        ////        {
        ////            if (pDevis != null)
        ////            {
        ////                if (pPreuve != null)
        ////                {
        ////                    pDevis.IDOWNERSHIP = pPreuve.PK_ID;
        ////                    //pDevis.IdEtapeDevis = DBETAPEDEVIS.GetByIdTypeDevisNumEtape(pDevis.IdTypeDevis, (int)Enumere.EtapeDevis.Accueil).Id;
        ////                    // Inserer preuve scannée
                            
        ////                    var resultInsertPreuve = DBDOCUMENTSCANNE.InsertDocumentScanne(context, pPreuve);
        ////                    if (resultInsertPreuve)
        ////                    {
        ////                        DEVIS LeDevis = DBDEVIS.GETDevis(pDevis, out numeroDevis);
        ////                        if (LeDevis!= null )
        ////                        {
        ////                            if (pListAppareils != null && pListAppareils.Count > 0 && pDevis.CODEPRODUIT == Enumere.Electricite)
        ////                            {
        ////                                List<ObjAPPAREILSDEVIS> lAppareilsDevis = new List<ObjAPPAREILSDEVIS>();
        ////                                ObjAPPAREILSDEVIS AppareilDevis = null;
        ////                                foreach (ObjAPPAREILS appareil in pListAppareils)
        ////                                {
        ////                                    AppareilDevis = new ObjAPPAREILSDEVIS();
        ////                                    AppareilDevis.FK_IDCODEAPPAREIL = appareil.PK_ID;
        ////                                    AppareilDevis.CODEAPPAREIL = appareil.CODEAPPAREIL;
        ////                                    AppareilDevis.NBRE = appareil.NOMBRE;
        ////                                    AppareilDevis.PUISSANCE = appareil.PUISSANCE;
        ////                                    AppareilDevis.NUMDEVIS = numeroDevis;
        ////                                    AppareilDevis.DATECREATION = DateTime.Now;
        ////                                    AppareilDevis.USERCREATION = pAgent.MATRICULE;
        ////                                    AppareilDevis.FK_IDDEVIS = pDevis.PK_ID;
        ////                                    lAppareilsDevis.Add(AppareilDevis);
        ////                                }
        ////                                //LeDevis.APPAREILSDEVIS = DBAPPAREILSDEVIS.GetAppareil(lAppareilsDevis);
        ////                            }
        ////                            if (pDevis.FK_IDETAPEDEVIS != null)
        ////                            {
        ////                                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, (int)pDevis.FK_IDETAPEDEVIS);
        ////                                DateTime? date, currentDate;
        ////                                currentDate = DateTime.Now.Date;
        ////                                TimeSpan difference = new TimeSpan();
        ////                                double delai;

        ////                                date = pDevis.DATEETAPE;
        ////                                if (date != null) difference = (currentDate.Value.Date - date.Value.Date);
        ////                                delai = difference.TotalDays;

        ////                                ObjSUIVIDEVIS newSuivi =
        ////                                    DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, idEtapeSuivi);

        ////                                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        ////                                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.FK_IDETAPEDEVIS);

        ////                                if (suivi != null && suivi.NUMDEVIS == pDevis.NUMDEVIS)
        ////                                {
        ////                                    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        ////                                        (pDevis.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        ////                                         && !string.IsNullOrEmpty(pDevis.MOTIFREJET) &&
        ////                                         etape.NUMETAPE > next.NUMETAPE))
        ////                                    {
        ////                                        suivi.COMMENTAIRE += "\r\n* " + pDevis.MOTIFREJET;
        ////                                        // cas de rejet du devis
        ////                                    }

        ////                                    suivi.DUREE = Convert.ToInt32(delai);
        ////                                    suivi.MATRICULEAGENT = pAgent.MATRICULE;
        ////                                    suivi.USERMODIFICATION = pAgent.MATRICULE;
        ////                                    suivi.DATEMODIFICATION = DateTime.Now;
        ////                                    LeDevis.SUIVIDEVIS.Add(DBSUIVIDEVIS.GetSuiviDevis(suivi));
        ////                                }
        ////                                else
        ////                                {
        ////                                    suivi = new ObjSUIVIDEVIS();
        ////                                    suivi.DUREE = Convert.ToInt32(delai);
        ////                                    suivi.MATRICULEAGENT = pAgent.MATRICULE;
        ////                                    suivi.NUMDEVIS = pDevis.NUMDEVIS;
        ////                                    suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        ////                                    suivi.USERCREATION = pAgent.MATRICULE;
        ////                                    suivi.DATECREATION = DateTime.Now;
        ////                                    suivi.FK_IDDEVIS = pDevis.PK_ID;
        ////                                    LeDevis.SUIVIDEVIS.Add(DBSUIVIDEVIS.GetSuiviDevis(suivi));

        ////                                }

        ////                                if (pDevis.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        ////                                {
        ////                                    suivi.MATRICULEAGENT = null;
        ////                                    suivi.COMMENTAIRE = null;
        ////                                    suivi.DUREE = null;
        ////                                    if (pDevis.FK_IDETAPEDEVIS != null)
        ////                                        suivi.FK_IDETAPEDEVIS = (int)pDevis.FK_IDETAPEDEVIS;
        ////                                    suivi.USERCREATION = pAgent.MATRICULE;
        ////                                    suivi.DATECREATION = DateTime.Now;
        ////                                    suivi.FK_IDDEVIS = pDevis.PK_ID;
        ////                                    LeDevis.SUIVIDEVIS.Add(DBSUIVIDEVIS.GetSuiviDevis(suivi));

        ////                                }
        ////                            }

        ////                            if (pDemandedevis != null)
        ////                            {
        ////                                pDemandedevis.NUMDEVIS = LeDevis.NUMDEVIS ;
        ////                                pDemandedevis.CLIENT = LeDevis.NUMDEVIS.PadLeft(pTailleClient, '0');
        ////                                pDemandedevis.FK_IDDEVIS = pDevis.PK_ID;
        ////                                // Inserer DemandeDevis
        ////                                //Insert de devis et la demande
        ////                                var resultInsertDemandeDevis = DBDEMANDEDEVIS.InsertDemandeDevis(pDemandedevis,LeDevis, context);
        ////                                if (resultInsertDemandeDevis)
        ////                                {
        ////                                    if (pDeposit != null)
        ////                                    {
        ////                                        pDeposit.CENTRE = pDevis.CODECENTRE;
        ////                                        pDeposit.CLIENT = pDemandedevis.CLIENT;
        ////                                        pDeposit.ORDRE = pDemandedevis.ORDRECLIENT;
        ////                                        pDeposit.NOM = pDemandedevis.NOM;
        ////                                        pDeposit.NUMDEVIS = numeroDevis;

        ////                                        if (string.IsNullOrEmpty(pDeposit.RECEIPT))
        ////                                        {
        ////                                            if (pAutorisation != null)
        ////                                            {
        ////                                                pDeposit.IDLETTER = pAutorisation.PK_ID;
        ////                                                // Inserer Documents Scannés
        ////                                                DBDOCUMENTSCANNE.InsertDocumentScanne(context, pAutorisation);
        ////                                                // Inserer Deposit
        ////                                                DBDEPOSIT.InsertDeposit(context, pDeposit);
        ////                                            }
        ////                                        }
        ////                                        else
        ////                                            DBDEPOSIT.UpdateDeposit(context, pDeposit);
        ////                                    }
        ////                                    // Insertion Appareils
                                           
        ////                                    return System.Convert.ToBoolean(context.SaveChanges());
        ////                                }
        ////                            }
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        return false;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}



    
        //public static List<ObjDEVIS> GetDevisByEtapeDevisId(int pEtapeId)
        //{
        //    try
        //    {
        //        return DBDEVIS.GetByIdEtapeDevis(pEtapeId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisDemandeDevisAndDocuments(ObjDEVIS pDevis, ObjDEMANDEDEVIS pDemandedevis, ObjDOCUMENTSCANNE pAutorisation, ObjDOCUMENTSCANNE pPreuve, ObjDEPOSIT pDeposit, ObjMATRICULE pAgent, int idEtapeSuivi, List<ObjAPPAREILSDEVIS> pListAppareilsDevis)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (pDevis != null)
        //            {

        //                if (pAutorisation != null && pAutorisation.ISUPDATE)
        //                    DBDOCUMENTSCANNE.UpdateDocumentScanne(context, pAutorisation);

        //                if (pPreuve != null && pPreuve.ISUPDATE)
        //                    DBDOCUMENTSCANNE.UpdateDocumentScanne(context, pPreuve);

        //                // Cas ou le deposit est modifié 
        //                if (pAutorisation != null && pAutorisation.ISTOREMOVE)
        //                {
        //                    pDeposit.IDLETTER = null;
        //                    DBDEPOSIT.UpdateDeposit(context, pDeposit);
        //                    DBDOCUMENTSCANNE.DeleteDocumentScanne(context, pAutorisation);
        //                }
        //                // Mise à jour des appareils devis
        //                if (pListAppareilsDevis != null && pListAppareilsDevis.Count > 0)
        //                {
        //                    if (DBAPPAREILSDEVIS.Delete(pListAppareilsDevis, context))
        //                        DBAPPAREILSDEVIS.Insert(pListAppareilsDevis, context);
        //                }

        //                var resultUpdateDevis = DBDEVIS.Update(context, pDevis);
        //                if (resultUpdateDevis && pDemandedevis != null)
        //                {
        //                    var resultUpdateDemandeDevis = DBDEMANDEDEVIS.Update(pDemandedevis, context);
        //                    if (resultUpdateDemandeDevis)
        //                    {


        //                        ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, idEtapeSuivi);
        //                        DateTime date = new DateTime(), currentDate;
        //                        currentDate = DateTime.Now.Date;
        //                        TimeSpan difference;
        //                        double delai;

        //                        if (pDevis.DATEETAPE != null) date = (DateTime)pDevis.DATEETAPE;
        //                        difference = (currentDate.Date - date.Date);
        //                        delai = difference.TotalDays;

        //                        if (pDevis.FK_IDETAPEDEVIS != null)
        //                        {
        //                            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, (int)pDevis.FK_IDETAPEDEVIS);

        //                            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.FK_IDETAPEDEVIS);

        //                            if (suivi != null && suivi.FK_IDDEVIS == pDevis.PK_ID)
        //                            {
        //                                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                                    (pDevis.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                                     && !string.IsNullOrEmpty(pDevis.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                                {
        //                                    suivi.COMMENTAIRE += "\r\n* " + pDevis.MOTIFREJET; // cas de rejet du devis
        //                                }

        //                                suivi.DUREE = Convert.ToInt32(delai);
        //                                suivi.MATRICULEAGENT = pAgent.MATRICULE;
        //                                suivi.USERMODIFICATION = pAgent.MATRICULE;
        //                                suivi.DATEMODIFICATION = DateTime.Now;
        //                                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                            }
        //                            else
        //                            {
        //                                suivi = new ObjSUIVIDEVIS();
        //                                suivi.DUREE = Convert.ToInt32(delai);
        //                                suivi.MATRICULEAGENT = pAgent.MATRICULE;
        //                                suivi.NUMDEVIS = pDevis.NUMDEVIS;
        //                                suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                                suivi.USERCREATION = pAgent.MATRICULE;
        //                                suivi.DATECREATION = DateTime.Now;
        //                                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                            }

        //                        }
        //                    }
        //                }
        //                return System.Convert.ToBoolean(context.SaveChanges());
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsInformationsDevis GetDemandeDevisForUpdateOrConsult(CsCriteresDevis pCriteresDevis)
        //{
        //    CsInformationsDevis InformationsDevis = null;
        //    try
        //    {
        //        if (pCriteresDevis != null && !string.IsNullOrEmpty(pCriteresDevis.NumeroDevis))
        //        {
        //            InformationsDevis = new CsInformationsDevis();
        //            // Get informations of Devis
        //            InformationsDevis.Devis = DBDEVIS.GetByIdDevis(pCriteresDevis.IdDevis);

        //            if (InformationsDevis.Devis != null)
        //            {
        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeDevisSuivante = DBService.GetEtapeSuivante_(InformationsDevis.Devis);

        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeCourante = DBETAPEDEVIS.GetById(InformationsDevis.Devis.FK_IDETAPEDEVIS);

        //                // Get informations of Etape Rejet 
        //                InformationsDevis.EtapeRejet = DBService.GetEtapeRejet(InformationsDevis.Devis);

        //                // Get informations of Etape intermédiaire 
        //                InformationsDevis.EtapeIntermediaire = DBService.GetEtapeIntermediaire(InformationsDevis.Devis);

        //                // Get informations of Travaux
        //                InformationsDevis.TravauxDevis = DBTRAVAUXDEVIS.SelectTravaux(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE);

        //                // Get informations of ChefEquipe
        //                if (InformationsDevis.TravauxDevis != null)
        //                    InformationsDevis.ChefEquipe = new DBAdmUsers().GetByMatricule(InformationsDevis.TravauxDevis.MATRICULECHEFEQUIPE);

        //                // Get informations of Travaux
        //                InformationsDevis.ControleTravaux = DBCONTROLETRAVAUX.SelectControles(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE);

        //                // Get informations of DemandeDevis
        //                InformationsDevis.DemandeDevis = DBDEMANDEDEVIS.GetDemandeDevisByIdDevis(InformationsDevis.Devis.PK_ID);

        //                // Get informations of Deposit
        //                InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(pCriteresDevis.NumeroDevis);

        //                // Get informations of Appareils
        //                InformationsDevis.ListAppareilsDevis = DBAPPAREILSDEVIS.GetByNumDevis(pCriteresDevis.IdDevis);

        //                // Get informations of ElementDevis
        //                InformationsDevis.ListElementsDevis = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE, true);

        //                // Get informations of detailsElementsDevis
        //                InformationsDevis.ListDetailsElementsDevis = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE, false);

        //                // Get informations of ElementsDevisForValidationRemiseStock
        //                InformationsDevis.ListElementsDevisForValidationRemiseStock = DBELEMENTSDEVIS.SelectElementsDevisByDevisIdForValidationRemiseStock(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE, false);
        //            }

        //            // Get informations document autorisation scannée
        //            if (InformationsDevis.Deposit != null && InformationsDevis.Deposit.IDLETTER  != null && (Guid)InformationsDevis.Deposit.IDLETTER != Guid.Empty)
        //                InformationsDevis.DocumentAutorisation = DBDOCUMENTSCANNE.GetDocumentScanneById((Guid)InformationsDevis.Deposit.IDLETTER);

        //            // Get informations document preuve scannée
        //            if (pCriteresDevis.IdDocumentPreuvePropriete != null && pCriteresDevis.IdDocumentPreuvePropriete != null && (Guid)pCriteresDevis.IdDocumentPreuvePropriete != Guid.Empty)
        //                InformationsDevis.DocumentPreuvePropriete = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentPreuvePropriete);

        //            // Get informations document schema
        //            if (pCriteresDevis.IdDocumentSchema != null && pCriteresDevis.IdDocumentSchema != null && (Guid)pCriteresDevis.IdDocumentSchema != Guid.Empty)
        //                InformationsDevis.DocumentSchema = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentSchema);

        //            // Get informations document manuscrit
        //            if (pCriteresDevis.IdDocumentManuscrit != null && pCriteresDevis.IdDocumentManuscrit != null && (Guid)pCriteresDevis.IdDocumentManuscrit != Guid.Empty)
        //                InformationsDevis.DocumentManuscrit = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentManuscrit);
        //        }
        //        return InformationsDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<CsComptDispo> SelectCompteurDispo(string pDiametre, string pProduit, string pNumeroCompteur , string pAnneeFabrication)
        //{
        //    try
        //    {
        //        return Entities.GetEntityListFromQuery<CsComptDispo>(CommonProcedures.RetourneCompteurDispo(pDiametre, pProduit, pNumeroCompteur, pAnneeFabrication));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsInformationsDevis GetDevisDemandeDevisEtapeSuivante(int pDevisId)
        //{
        //    CsInformationsDevis InformationsDevis = null;
        //    try
        //    {
        //        //if (!string.IsNullOrEmpty(pNumeroDevis))
        //        //{
        //            InformationsDevis = new CsInformationsDevis();
        //            // Get informations of Devis
        //            InformationsDevis.Devis = DBDEVIS.GetByIdDevis(pDevisId);
        //            if (InformationsDevis.Devis != null)
        //            {
        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeDevisSuivante = DBService.GetEtapeSuivante_(InformationsDevis.Devis);
        //                // Get informations of DemandeDevis
        //                InformationsDevis.DemandeDevis = DBDEMANDEDEVIS.GetDemandeDevisByIdDevis(InformationsDevis.Devis.PK_ID);
        //            }
        //        //}
        //        return InformationsDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsInformationsDevis GetDevisDemandeDevisAppareilsForUpdateOrConsult(CsCriteresDevis pCriteresDevis )
        //{
        //    CsInformationsDevis InformationsDevis = null;
        //    try
        //    {
        //        if (pCriteresDevis != null && !string.IsNullOrEmpty(pCriteresDevis.NumeroDevis))
        //        {
        //            InformationsDevis = new CsInformationsDevis();
        //            // Get informations of Devis
        //            InformationsDevis.Devis = DBDEVIS.GetByNumDevis(pCriteresDevis.NumeroDevis);

        //            if (InformationsDevis.Devis != null)
        //            {
        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeDevisSuivante = DBService.GetEtapeSuivante(InformationsDevis.Devis);

        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeCourante = DBETAPEDEVIS.GetById(InformationsDevis.Devis.FK_IDETAPEDEVIS);

        //                // Get informations of Etape Rejet 
        //                InformationsDevis.EtapeRejet = DBService.GetEtapeRejet(InformationsDevis.Devis);

        //                // Get informations of Etape intermédiaire 
        //                InformationsDevis.EtapeIntermediaire = DBService.GetEtapeIntermediaire(InformationsDevis.Devis);

        //                // Get informations of DemandeDevis
        //                InformationsDevis.DemandeDevis = DBDEMANDEDEVIS.GetDemandeDevisByIdDevis(InformationsDevis.Devis.PK_ID);

        //                // Get informations of Deposit
        //                InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(pCriteresDevis.NumeroDevis);

        //                // Get informations of Appareils
        //                InformationsDevis.ListAppareilsDevis = DBAPPAREILSDEVIS.GetByNumDevis(pCriteresDevis.IdDevis);
        //            }

        //            // Get informations document autorisation scannée
        //            if (InformationsDevis.Deposit != null && InformationsDevis.Deposit.IDLETTER != null && (Guid)InformationsDevis.Deposit.IDLETTER != Guid.Empty)
        //                InformationsDevis.DocumentAutorisation = DBDOCUMENTSCANNE.GetDocumentScanneById((Guid)InformationsDevis.Deposit.IDLETTER);

        //            // Get informations document preuve scannée
        //            if (pCriteresDevis.IdDocumentPreuvePropriete != null && pCriteresDevis.IdDocumentPreuvePropriete  != null && (Guid)pCriteresDevis.IdDocumentPreuvePropriete != Guid.Empty)
        //                InformationsDevis.DocumentPreuvePropriete = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentPreuvePropriete);
        //        }
        //        return InformationsDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsInformationsDevis GetDevisDemandeDepositAppareilsAutorisationPreuveEtapeSuivante(CsCriteresDevis pCriteresDevis)
        //{
        //    CsInformationsDevis InformationsDevis = null;
        //    try
        //    {
        //        if (pCriteresDevis != null && !string.IsNullOrEmpty(pCriteresDevis.NumeroDevis))
        //        {
        //            InformationsDevis = new CsInformationsDevis();
        //            // Get informations of Devis
        //            InformationsDevis.Devis = DBDEVIS.GetByNumDevis(pCriteresDevis.NumeroDevis);

        //            if (InformationsDevis.Devis != null)
        //            {
        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeDevisSuivante = DBService.GetEtapeSuivante(InformationsDevis.Devis);

        //                // Get informations of DemandeDevis
        //                InformationsDevis.DemandeDevis = DBDEMANDEDEVIS.GetDemandeDevisByIdDevis(InformationsDevis.Devis.PK_ID);

        //                // Get informations of Deposit
        //                InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(pCriteresDevis.NumeroDevis);

        //                // Get informations of Appareils
        //                InformationsDevis.ListAppareilsDevis = DBAPPAREILSDEVIS.GetByNumDevis(pCriteresDevis.IdDevis);
        //            }

        //            // Get informations document autorisation scannée
        //            if (InformationsDevis.Deposit != null && InformationsDevis.Deposit.IDLETTER != null && (Guid)InformationsDevis.Deposit.IDLETTER != Guid.Empty)
        //                InformationsDevis.DocumentAutorisation = DBDOCUMENTSCANNE.GetDocumentScanneById((Guid)InformationsDevis.Deposit.IDLETTER);

        //            // Get informations document preuve scannée
        //            if (pCriteresDevis.IdDocumentPreuvePropriete != null && pCriteresDevis.IdDocumentPreuvePropriete  != null && (Guid)pCriteresDevis.IdDocumentPreuvePropriete != Guid.Empty)
        //                InformationsDevis.DocumentPreuvePropriete = DBDOCUMENTSCANNE.GetDocumentScanneById(pCriteresDevis.IdDocumentPreuvePropriete);
        //        }
        //        return InformationsDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsInformationsDevis GetDevisControleTravauxDepositAndAmountPaid(string pNumDevis)
        //{
        //    CsInformationsDevis InformationsDevis = null;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(pNumDevis))
        //        {
        //             InformationsDevis = new CsInformationsDevis();
        //            // Get informations of Devis
        //             InformationsDevis.Devis = DBDEVIS.GetByNumDevis(pNumDevis);

        //            if (InformationsDevis.Devis != null)
        //            {
        //                // Get informations of Etape suivante 
        //                InformationsDevis.EtapeCourante = DBETAPEDEVIS.GetById(InformationsDevis.Devis.FK_IDETAPEDEVIS);
        //                // Get informations of Travaux
        //                InformationsDevis.TravauxDevis = DBTRAVAUXDEVIS.SelectTravaux(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE);
        //                // Get informations of Controle Travaux
        //                InformationsDevis.ControleTravaux = DBCONTROLETRAVAUX.SelectControles(InformationsDevis.Devis.PK_ID, (int)InformationsDevis.Devis.ORDRE);
        //                // Get informations of Deposit
        //                InformationsDevis.Deposit = DBDEPOSIT.SearchByNumDevis(InformationsDevis.Devis.NUMDEVIS);
        //                // Get Amount paid
        //                InformationsDevis.AmountPaid = DBService.GetAmountPaid(InformationsDevis.Devis);
        //            }
        //        }
        //        return InformationsDevis;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevis(ObjDEVIS entity, CsUserConnecte agent)
        //{
        //    try
        //    {
        //       using (galadbEntities context = new galadbEntities())
        //        {
        //            if (entity != null && agent != null)
        //            {
        //                ObjETAPEDEVIS nextEtape = new ObjETAPEDEVIS();
        //                entity.DATEETAPE = DateTime.Now;
        //                nextEtape = DBService.GetEtapeSuivante_(entity);
        //                int idEtapeSuivi = (int)entity.FK_IDETAPEDEVIS;
        //                entity.FK_IDETAPEDEVIS = nextEtape.PK_ID;

        //                DBDEVIS.Update(context, entity);
        //                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //                DateTime date, currentDate;
        //                currentDate = DateTime.Now.Date;
        //                TimeSpan difference;
        //                double delai;

        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;

        //                ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //                if (suivi != null && suivi.FK_IDDEVIS == entity.PK_ID)
        //                {
        //                    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                        (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                        && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                    {
        //                        suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                    }

        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.USERMODIFICATION = agent.matricule;
        //                    suivi.DATEMODIFICATION = DateTime.Now;
        //                    DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                }
        //                else
        //                {
        //                    suivi = new ObjSUIVIDEVIS();
        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.NUMDEVIS = entity.NUMDEVIS;
        //                    suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //                {
        //                    suivi.MATRICULEAGENT = null;
        //                    suivi.COMMENTAIRE = null;
        //                    suivi.DUREE = null;
        //                    suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                return System.Convert.ToBoolean(context.SaveChanges());
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevis(ObjDEVIS entity, CsUserConnecte agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (entity != null && agent != null)
        //            {
        //                DBDEVIS.Update(context, entity);
        //                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //                DateTime date, currentDate;
        //                currentDate = DateTime.Now.Date;
        //                TimeSpan difference;
        //                double delai;

        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;

        //                ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //                if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
        //                {
        //                    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                        (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                        && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                    {
        //                        suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                    }

        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.USERMODIFICATION = agent.matricule;
        //                    suivi.DATEMODIFICATION = DateTime.Now;
        //                    DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                }
        //                else
        //                {
        //                    suivi = new ObjSUIVIDEVIS();
        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.NUMDEVIS = entity.NUMDEVIS;
        //                    suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.FK_IDDEVIS = (int)entity.PK_ID;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //                {
        //                    suivi.MATRICULEAGENT = null;
        //                    suivi.COMMENTAIRE = null;
        //                    suivi.DUREE = null;
        //                    suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    suivi.FK_IDDEVIS = (int)entity.PK_ID;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                return System.Convert.ToBoolean(context.SaveChanges());
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool AffecterDevis(ObjDEVIS entity, ObjDEVISPIA pia, CsUserConnecte agent,bool pIsReaffecter)
        //{
        //    ObjETAPEDEVIS nextEtape = null;
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (entity != null && agent != null && pia != null)
        //            {
        //                entity.DATEETAPE = DateTime.Now;
        //                if (!pIsReaffecter)
        //                    nextEtape = DBService.GetEtapeSuivante_(entity);
        //                int idEtapeSuivi = (int)entity.FK_IDETAPEDEVIS;
        //                if (nextEtape != null)
        //                    entity.FK_IDETAPEDEVIS = nextEtape.PK_ID;

        //                DBDEVIS.Update(context, entity);

        //                //DBDEVISPIA.Delete(context, pia);
        //                //DBDEVISPIA.Insert(context, pia);
        //                pia.FK_IDUSER = context.ADMUTILISATEUR.FirstOrDefault(u => u.MATRICULE == pia.USERCREATION).PK_ID;
        //                DBDEVISPIA.InsertOrUpdate(context, pia);

        //                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //                DateTime date, currentDate;
        //                currentDate = DateTime.Now.Date;
        //                TimeSpan difference;
        //                double delai;

        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;

        //                ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //                if (suivi != null && suivi.FK_IDDEVIS == entity.PK_ID)
        //                {
        //                    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                        (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                        && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                    {
        //                        suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                    }

        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.USERMODIFICATION = agent.matricule;
        //                    suivi.DATEMODIFICATION = DateTime.Now;
        //                    DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                }
        //                else
        //                {
        //                    suivi = new ObjSUIVIDEVIS();
        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.NUMDEVIS = entity.NUMDEVIS;
        //                    suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }

        //                if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //                {
        //                    suivi.MATRICULEAGENT = null;
        //                    suivi.COMMENTAIRE = null;
        //                    suivi.DUREE = null;
        //                    suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //               return System.Convert.ToBoolean(context.SaveChanges());
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention)
        //{
        //    CsCriteresDevis ParametresDistance = new CsCriteresDevis();
        //    try
        //    {
        //        DB_ParametresGeneraux dbParamGeneraux = new DB_ParametresGeneraux();
        //        if (!string.IsNullOrEmpty(pMaxi))
        //        {
        //            var ParamMaxi = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxi);
        //            if (ParamMaxi != null)
        //                ParametresDistance.Maxi = decimal.Parse(ParamMaxi.LIBELLE);
        //        }
        //        if (!string.IsNullOrEmpty(pSeuil))
        //        {
        //            var ParamSeuil = dbParamGeneraux.SelectParametresGenerauxByCode(pSeuil);
        //            if (ParamSeuil != null)
        //                ParametresDistance.Seuil = decimal.Parse(ParamSeuil.LIBELLE);
        //        }
        //        if (!string.IsNullOrEmpty(pMaxiSubvention))
        //        {
        //            var ParamMaxiSubvention = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxiSubvention);
        //            if (ParamMaxiSubvention != null)
        //                ParametresDistance.MaxiSubvention = decimal.Parse(ParamMaxiSubvention.LIBELLE);
        //        }
        //        return ParametresDistance;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateElementsDevis(ObjDEMANDEDEVIS pDemandedevis, ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjDOCUMENTSCANNE doc, ObjDOCUMENTSCANNE manuscrit, ObjMATRICULE agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (_lElements != null && _lElements.Count >0)
        //                DBELEMENTSDEVIS.DeleteElementsDevis(context, _lElements);

        //            if ((entity.IDSCHEMA != null && (Guid)entity.IDSCHEMA != Guid.Empty) || (entity.IDMANUSCRIT != null && entity.IDMANUSCRIT != Guid.Empty))
        //            {
        //                if (doc.ISNEW)
        //                    DBDOCUMENTSCANNE.InsertDocumentScanne(context, doc);
        //                else
        //                    DBDOCUMENTSCANNE.UpdateDocumentScanne(context, doc);

        //                if (manuscrit.ISNEW)
        //                    DBDOCUMENTSCANNE.InsertDocumentScanne(context, manuscrit);
        //                else
        //                    DBDOCUMENTSCANNE.UpdateDocumentScanne(context, manuscrit);

        //                var resultUpdateDevis = DBDEVIS.Update(context, entity);
        //                if (resultUpdateDevis && pDemandedevis != null)
        //                    DBDEMANDEDEVIS.Update(pDemandedevis, context);
        //            }
        //            else
        //            {
        //                var resultUpdateDevis = DBDEVIS.Update(context, entity);
        //                if (resultUpdateDevis && pDemandedevis != null)
        //                    DBDEMANDEDEVIS.Update(pDemandedevis, context);
        //                if (doc.ISTOREMOVE)
        //                    DBDOCUMENTSCANNE.DeleteDocumentScanne(context, doc);

        //                if (manuscrit.ISTOREMOVE)
        //                    DBDOCUMENTSCANNE.DeleteDocumentScanne(context, manuscrit);
        //            }


        //            if (_lElements != null && _lElements.Count > 0)
        //                DBELEMENTSDEVIS.InsertElementsDevis(_lElements, context);

        //            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //            DateTime date, currentDate;
        //            currentDate = DateTime.Now.Date;
        //            TimeSpan difference;
        //            double delai = 0;
        //            if (entity.DATEETAPE != null)
        //            {
        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;
        //            }
        //            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //            if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
        //            {
        //                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                    (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                    && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                {
        //                    suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                }
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.USERMODIFICATION = agent.MATRICULE;
        //                suivi.DATEMODIFICATION = DateTime.Now;
        //                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //            }
        //            else
        //            {
        //                suivi = new ObjSUIVIDEVIS();
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.NUMDEVIS = entity.NUMDEVIS;
        //                suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //            {
        //                suivi.MATRICULEAGENT = null;
        //                suivi.COMMENTAIRE = null;
        //                suivi.DUREE = null;
        //                suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }
        //            return System.Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisElementsDocument(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjDOCUMENTSCANNE doc, ObjMATRICULE agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            //if (_lElements != null && _lElements.Count >0)
        //            //    DBELEMENTSDEVIS.DeleteElementsDevis(context, _lElements);

        //            if (entity.IDSCHEMA != null && entity.IDSCHEMA != Guid.Empty)
        //            {
        //                if (doc.ISNEW)
        //                    DBDOCUMENTSCANNE.InsertDocumentScanne(context, doc);
        //                else
        //                    DBDOCUMENTSCANNE.UpdateDocumentScanne(context, doc);
        //                DBDEVIS.Update(context, entity); //Position liée aux contraintes d'intégrité
        //            }
        //            else
        //            {
        //                DBDEVIS.Update(context, entity); //Position liée aux contraintes d'intégrité
        //                if (doc.OriginalPK_ID != Guid.Empty && doc.ISTOREMOVE)
        //                    DBDOCUMENTSCANNE.DeleteDocumentScanne(context, doc);
        //            }

        //            if (_lElements != null && _lElements.Count > 0)
        //                //DBELEMENTSDEVIS.InsertElementsDevis(_lElements, context);
        //                DBELEMENTSDEVIS.UpdateOrInsertElementsDevis(_lElements, agent, context);

        //            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //            DateTime date, currentDate;
        //            currentDate = DateTime.Now.Date;
        //            TimeSpan difference;
        //            double delai = 0;
        //            if (entity.DATEETAPE != null)
        //            {
        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;
        //            }

        //            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //            if (suivi != null && suivi.FK_IDDEVIS == entity.PK_ID)
        //            {
        //                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                    (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                    && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                {
        //                    suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                }

        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.USERMODIFICATION = agent.MATRICULE;
        //                suivi.DATEMODIFICATION = DateTime.Now;
        //                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //            }
        //            else
        //            {
        //                suivi = new ObjSUIVIDEVIS();
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.NUMDEVIS = entity.NUMDEVIS;
        //                suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //            {
        //                suivi.MATRICULEAGENT = null;
        //                suivi.COMMENTAIRE = null;
        //                suivi.DUREE = null;
        //                suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }
        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisValidationEtude(List<ObjDEVIS> pListeDevis)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            ObjETAPEDEVIS etape;
        //            foreach (ObjDEVIS _devis in pListeDevis)
        //            {
        //                var DevisEnBase = DBDEVIS.GetByNumDevis(_devis.NUMDEVIS);
        //                DevisEnBase.ISPOSE = _devis.ISPOSE;
        //                DevisEnBase.ISFOURNITURE = _devis.ISFOURNITURE;
        //                DevisEnBase.DATEETAPE = DateTime.Now;
        //                ObjETAPEDEVIS next = DBService.GetEtapeSuivante_(DevisEnBase);
        //                DevisEnBase.FK_IDETAPEDEVIS = next.PK_ID;

        //                etape = new ObjETAPEDEVIS();
        //                etape = DBETAPEDEVIS.GetById(DevisEnBase.FK_IDETAPEDEVIS);
                      
        //                DBDEVIS.Update(context, DevisEnBase);
        //                if (etape.NUMETAPE == (int)Enumere.EtapeDevis.Encaissement)
        //                {
        //                    ObjDEPOSIT deposit = DBDEPOSIT.SearchByNumDevis(DevisEnBase.NUMDEVIS);
        //                    if (deposit != null)
        //                    {
        //                        deposit.TOTAL = (decimal)DevisEnBase.MONTANTTTC;
        //                        deposit.MONTANTTVA = (decimal)(DevisEnBase.MONTANTTTC - DevisEnBase.MONTANTHT);
        //                        deposit.USERMODIFICATION = _devis.USERMODIFICATION;
        //                        deposit.DATEMODIFICATION = DateTime.Now;
        //                        DBDEPOSIT.UpdateDeposit(context, deposit);
        //                    }
        //                }
        //            }
        //            return Convert.ToBoolean(context.SaveChanges());
        //       }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisValidationDossier(List<ObjDEVIS> pListeDevis)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            ObjETAPEDEVIS etape;
        //            foreach (ObjDEVIS _devis in pListeDevis)
        //            {
        //                etape = new ObjETAPEDEVIS();
        //                etape = DBETAPEDEVIS.GetById(_devis.FK_IDETAPEDEVIS);
        //                DBDEVIS.Update(context, _devis);
        //                if (etape.NUMETAPE == (int)Enumere.EtapeDevis.Encaissement)
        //                {
        //                    ObjDEPOSIT deposit = DBDEPOSIT.SearchByNumDevis(_devis.NUMDEVIS);
        //                    if (deposit != null)
        //                    {
        //                        deposit.TOTAL = (decimal)_devis.MONTANTTTC;
        //                        deposit.MONTANTTVA = (decimal)(_devis.MONTANTTTC - _devis.MONTANTHT);
        //                        DBDEPOSIT.UpdateDeposit(context, deposit);
        //                    }
        //                }
        //            }
        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<CsEtatBonDeSortie> EditerDevisPourBonDeSortie(int pNumDevis, int pOrdre, bool pIsSummary,string pMatricule)
        //{
        //    try
        //    {
        //        return DBETATSDEVIS.EditerDevisPourBonDeSortie(pNumDevis, pOrdre, pIsSummary,pMatricule);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<CsEtatBonTravaux> EditerDevisPourBonTravaux(int pNumDevis, byte pOrdre)
        //{
        //    try
        //    {
        //        return DBETATSDEVIS.EditerDevisPourBonTravaux(pNumDevis, pOrdre);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisValidation(ObjDEVIS pDevis, ObjDEPOSIT pDeposit, ObjMATRICULE pAgent, int pIdEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            ObjETAPEDEVIS etape = new ObjETAPEDEVIS();

        //            etape = DBETAPEDEVIS.GetById(pDevis.FK_IDETAPEDEVIS);
        //            DBDEVIS.Update(context, pDevis);
        //            if (etape.NUMETAPE == (int)Enumere.EtapeDevis.Encaissement)
        //            {
        //                pDeposit.TOTAL = (decimal)pDevis.MONTANTTTC;
        //                pDeposit.MONTANTTVA = (decimal)(pDevis.MONTANTTTC - pDevis.MONTANTHT);
        //                DBDEPOSIT.UpdateDeposit(context, pDeposit);
        //            }

        //            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, pIdEtapeSuivi);
        //            DateTime date, currentDate;
        //            currentDate = DateTime.Now.Date;
        //            TimeSpan difference;
        //            double delai;

        //            date = (DateTime)pDevis.DATEETAPE;
        //            difference = (currentDate.Date - date.Date);
        //            delai = difference.TotalDays;

        //            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(pDevis.PK_ID, (int)pDevis.FK_IDETAPEDEVIS);

        //            ObjETAPEDEVIS step = DBETAPEDEVIS.GetById(pIdEtapeSuivi);
        //            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(pDevis.FK_IDETAPEDEVIS);

        //            if (suivi != null && suivi.NUMDEVIS == pDevis.NUMDEVIS)
        //            {
        //                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                    (pDevis.FK_IDETAPEDEVIS != pIdEtapeSuivi && newSuivi != null
        //                    && !string.IsNullOrEmpty(pDevis.MOTIFREJET) && step.NUMETAPE > next.NUMETAPE))
        //                {
        //                    suivi.COMMENTAIRE += "\r\n* " + pDevis.MOTIFREJET; // cas de rejet du devis
        //                }

        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = pAgent.MATRICULE;
        //                suivi.USERMODIFICATION = pAgent.MATRICULE;
        //                suivi.DATEMODIFICATION = DateTime.Now;
        //                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //            }
        //            else
        //            {
        //                suivi = new ObjSUIVIDEVIS();
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = pAgent.MATRICULE;
        //                suivi.NUMDEVIS = pDevis.NUMDEVIS;
        //                suivi.FK_IDETAPEDEVIS = pIdEtapeSuivi;
        //                suivi.USERCREATION = pAgent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            if (pDevis.FK_IDETAPEDEVIS != pIdEtapeSuivi && newSuivi == null)
        //            {
        //                suivi.MATRICULEAGENT = null;
        //                suivi.COMMENTAIRE = null;
        //                suivi.DUREE = null;
        //                suivi.FK_IDETAPEDEVIS = (int)pDevis.FK_IDETAPEDEVIS;
        //                suivi.USERCREATION = pAgent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }
        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisTravaux(ObjDEVIS entity, ObjTRAVAUXDEVIS travaux, bool isChefExists, bool isElementsNeeded, ObjMATRICULE agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            DBDEVIS.Update(context, entity);

        //            travaux.USERCREATION = agent.MATRICULE;
        //            travaux.DATECREATION = DateTime.Now;
        //            travaux.FK_IDDEVIS = entity.PK_ID;

        //            if (isChefExists)
        //                DBTRAVAUXDEVIS.UpdateTravaux(context, travaux);
        //            else
        //                DBTRAVAUXDEVIS.InsertTravaux(context, travaux);
        //            if (isElementsNeeded)
        //            {
        //                //byte ordre = _devis.Ordre;
        //                //ordre--;

        //                //Si le devis est ramené à "Etablissement procès verbal" pour reprise des travaux sans modification du devis,
        //                //1- il faut marquer l'ordre précédent (dans TRAVAUXDEVIS) pour ne pas en tenir compte dans le bilan final de remboursement
        //                ObjTRAVAUXDEVIS oldTravaux = DBTRAVAUXDEVIS.SelectTravaux(entity.PK_ID, (int)entity.ORDRE);
        //                if ((oldTravaux != null) && (!string.IsNullOrEmpty(oldTravaux.MATRICULECHEFEQUIPE)))
        //                {
        //                    oldTravaux.ISUSEDFORBILAN = false;
        //                    oldTravaux.USERMODIFICATION = agent.MATRICULE;
        //                    oldTravaux.DATEMODIFICATION = DateTime.Now;
        //                    DBTRAVAUXDEVIS.UpdateTravaux(context, oldTravaux);
        //                }

        //                //2- ensuite il faut recopier les éléments du devis et les mettre à l'ordre courant (le nouvel ordre) 
        //                //   en gardant une copie pour l'ordre précédent
        //                List<ObjELEMENTDEVIS> elements = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(entity.PK_ID, (int)entity.ORDRE, true);
        //                List<ObjELEMENTDEVIS> elements2 = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(entity.PK_ID, (int)entity.ORDRE, false);

        //                List<ObjELEMENTDEVIS> newElements = new List<ObjELEMENTDEVIS>();
        //                if ((elements != null) && (elements.Count > 0))
        //                {
        //                    ObjELEMENTDEVIS elt;
        //                    foreach (ObjELEMENTDEVIS st in elements)
        //                    {
        //                        elt = new ObjELEMENTDEVIS();
        //                        elt = st;
        //                        //elt.Ordre++;
        //                        elt.USERMODIFICATION = agent.USERMODIFICATION;
        //                        elt.DATEMODIFICATION = DateTime.Now;
        //                        newElements.Add(elt);
        //                    }
        //                }

        //                if ((elements2 != null) && (elements2.Count > 0))
        //                {
        //                    ObjELEMENTDEVIS elt;
        //                    foreach (ObjELEMENTDEVIS st in elements2)
        //                    {
        //                        elt = new ObjELEMENTDEVIS();
        //                        elt = st;
        //                        //elt.Ordre++;
        //                        elt.USERMODIFICATION = agent.USERMODIFICATION;
        //                        elt.DATEMODIFICATION = DateTime.Now;
        //                        newElements.Add(elt);
        //                    }
        //                }
        //                DBELEMENTSDEVIS.InsertElementsDevis(newElements, context);

        //            }

        //            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //            DateTime date, currentDate;
        //            currentDate = DateTime.Now.Date;
        //            TimeSpan difference;
        //            double delai;

        //            date = (DateTime)entity.DATEETAPE;
        //            difference = (currentDate.Date - date.Date);
        //            delai = difference.TotalDays;

        //            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //            if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
        //            {
        //                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                    (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                    && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                {
        //                    suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                }

        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.USERMODIFICATION = agent.MATRICULE;
        //                suivi.DATEMODIFICATION = DateTime.Now;
        //                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //            }
        //            else
        //            {
        //                suivi = new ObjSUIVIDEVIS();
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.NUMDEVIS = entity.NUMDEVIS;
        //                suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                suivi.FK_IDDEVIS = entity.PK_ID;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //            {
        //                suivi.MATRICULEAGENT = null;
        //                suivi.COMMENTAIRE = null;
        //                suivi.DUREE = null;
        //                suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisProcesVerbal(ObjDEMANDEDEVIS _dem, ObjDEVIS entity, ObjTRAVAUXDEVIS travaux, bool isChefExists, bool isElementsNeeded, ObjMATRICULE agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            DBDEVIS.Update(context, entity);
        //            DBDEMANDEDEVIS.Update(_dem, context);

        //            if (isChefExists)
        //            {
        //                travaux.USERMODIFICATION = agent.MATRICULE;
        //                travaux.DATEMODIFICATION = DateTime.Now;
        //                DBTRAVAUXDEVIS.UpdateTravaux(context, travaux);
        //            }
        //            else
        //            {
        //                travaux.USERCREATION = agent.MATRICULE;
        //                travaux.DATECREATION= DateTime.Now;
        //                DBTRAVAUXDEVIS.InsertTravaux(context, travaux);
        //            }
        //            if (isElementsNeeded)
        //            {
        //                //byte ordre = _devis.Ordre;
        //                //ordre--;

        //                //Si le devis est ramené à "Etablissement procès verbal" pour reprise des travaux sans modification du devis,
        //                //1- il faut marquer l'ordre précédent (dans TRAVAUXDEVIS) pour ne pas en tenir compte dans le bilan final de remboursement
        //                ObjTRAVAUXDEVIS oldTravaux = DBTRAVAUXDEVIS.SelectTravaux(entity.PK_ID, (int)entity.ORDRE);
        //                if ((oldTravaux != null) && (!string.IsNullOrEmpty(oldTravaux.MATRICULECHEFEQUIPE)))
        //                {
        //                    oldTravaux.ISUSEDFORBILAN = false;
        //                    oldTravaux.USERMODIFICATION = agent.MATRICULE;
        //                    oldTravaux.DATEMODIFICATION = DateTime.Now;
        //                    DBTRAVAUXDEVIS.UpdateTravaux(context, oldTravaux);
        //                }

        //                //2- ensuite il faut recopier les éléments du devis et les mettre à l'ordre courant (le nouvel ordre) 
        //                //   en gardant une copie pour l'ordre précédent
        //                List<ObjELEMENTDEVIS> elements = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(entity.PK_ID, (int)entity.ORDRE, true);
        //                List<ObjELEMENTDEVIS> elements2 = DBELEMENTSDEVIS.SelectElementsDevisByDevisId(entity.PK_ID, (int)entity.ORDRE, false);

        //                List<ObjELEMENTDEVIS> newElements = new List<ObjELEMENTDEVIS>();
        //                if ((elements != null) && (elements.Count > 0))
        //                {
        //                    ObjELEMENTDEVIS elt;
        //                    foreach (ObjELEMENTDEVIS st in elements)
        //                    {
        //                        elt = new ObjELEMENTDEVIS();
        //                        elt = st;
        //                        elt.USERMODIFICATION = agent.USERMODIFICATION;
        //                        elt.DATEMODIFICATION = DateTime.Now;
        //                        newElements.Add(elt);
        //                    }
        //                }

        //                if ((elements2 != null) && (elements2.Count > 0))
        //                {
        //                    ObjELEMENTDEVIS elt;
        //                    foreach (ObjELEMENTDEVIS st in elements2)
        //                    {
        //                        elt = new ObjELEMENTDEVIS();
        //                        elt = st;
        //                        elt.USERMODIFICATION = agent.USERMODIFICATION;
        //                        elt.DATEMODIFICATION = DateTime.Now;
        //                        newElements.Add(elt);
        //                    }
        //                }
        //                DBELEMENTSDEVIS.InsertElementsDevis(newElements, context);
        //            }

        //            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //            DateTime date, currentDate;
        //            currentDate = DateTime.Now.Date;
        //            TimeSpan difference;
        //            double delai;

        //            date = (DateTime)entity.DATEETAPE;
        //            difference = (currentDate.Date - date.Date);
        //            delai = difference.TotalDays;

        //            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //            if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
        //            {
        //                if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                    (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                    && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                {
        //                    suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                }

        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.USERMODIFICATION = agent.USERMODIFICATION;
        //                suivi.DATEMODIFICATION = DateTime.Now;
        //                DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //            }
        //            else
        //            {
        //                suivi = new ObjSUIVIDEVIS();
        //                suivi.DUREE = Convert.ToInt32(delai);
        //                suivi.MATRICULEAGENT = agent.MATRICULE;
        //                suivi.NUMDEVIS = entity.NUMDEVIS;
        //                suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }

        //            if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //            {
        //                suivi.MATRICULEAGENT = null;
        //                suivi.COMMENTAIRE = null;
        //                suivi.DUREE = null;
        //                suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                suivi.USERCREATION = agent.MATRICULE;
        //                suivi.DATECREATION = DateTime.Now;
        //                DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //            }
        //            UpdateObjetDemande(entity, context);
        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void  UpdateObjetDemande(ObjDEVIS _LeObjDevis, galadbEntities leContext)
        //{
        //    try
        //    {
        //        foreach (var dcompteur in leContext.DCANALISATION.Where(dcanal => dcanal.DEMANDE.NUMDEVIS == _LeObjDevis.NUMDEVIS))
        //        {
        //            dcompteur.COMPTEUR.NUMERO = _LeObjDevis.NUMEROCTR;
        //            dcompteur.COMPTEUR.MARQUE  = _LeObjDevis.IDMARQUECTR;
        //            dcompteur.COMPTEUR.TYPECOMPTEUR  = _LeObjDevis.IDTYPECTR;
        //            dcompteur.COMPTEUR.ANNEEFAB = _LeObjDevis.DATEFABRICATIONCTR.Value.Year.ToString();
        //        }
        //        foreach (var dcompteur in leContext.DEVENEMENT.Where(dcanal => dcanal.DEMANDE.NUMDEVIS == _LeObjDevis.NUMDEVIS))
        //        {
        //            dcompteur.DATEEVT = _LeObjDevis.DATEPOSECTR;
        //            dcompteur.INDEXEVT = _LeObjDevis.INDEXPOSECTR;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw ex;
        //    }
        
        //}

        //public static List<CsEtatProcesVerbal> EditerDevisPourProcesVerbal(int pIdDevis, byte pOrdre)
        //{
        //    try
        //    {
        //        return DBETATSDEVIS.EditerDevisPourProcesVerbal(pIdDevis, pOrdre);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static List<CsEtatProcesVerbal> EditerDevisPourBonControle(int pIdDevis, byte pOrdre)
        //{
        //    try
        //    {
        //        return DBETATSDEVIS.EditerDevisPourBonControle(pIdDevis, pOrdre);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisValidation(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjMATRICULE agent, int idEtapeSuivi)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (DBDEVIS.Update(context, entity))
        //            {
        //                if (DBELEMENTSDEVIS.UpdateElementsDevisValide(_lElements, context))
        //                {
        //                    ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //                    DateTime date, currentDate;
        //                    currentDate = DateTime.Now.Date;
        //                    TimeSpan difference;
        //                    double delai;

        //                    date = (DateTime)entity.DATEETAPE;
        //                    difference = (currentDate.Date - date.Date);
        //                    delai = difference.TotalDays;

        //                    ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //                    ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                    ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //                    if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
        //                    {
        //                        if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                            (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                            && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                        {
        //                            suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                        }

        //                        suivi.DUREE = Convert.ToInt32(delai);
        //                        suivi.MATRICULEAGENT = agent.MATRICULE;
        //                        suivi.USERMODIFICATION = agent.USERMODIFICATION;
        //                        suivi.DATEMODIFICATION = DateTime.Now;
        //                        DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                    }
        //                    else
        //                    {
        //                        suivi = new ObjSUIVIDEVIS();
        //                        suivi.DUREE = Convert.ToInt32(delai);
        //                        suivi.MATRICULEAGENT = agent.MATRICULE;
        //                        suivi.NUMDEVIS = entity.NUMDEVIS;
        //                        suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                        suivi.USERCREATION = agent.MATRICULE;
        //                        suivi.DATECREATION = DateTime.Now;
        //                        DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                    }

        //                    if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //                    {
        //                        suivi.MATRICULEAGENT = null;
        //                        suivi.COMMENTAIRE = null;
        //                        suivi.DUREE = null;
        //                        suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                        suivi.USERCREATION = agent.MATRICULE;
        //                        suivi.DATECREATION = DateTime.Now;
        //                        DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                    }
        //                    new DBAccueil().ValiderDemandeByDevis(entity, context);
        //                    return Convert.ToBoolean(context.SaveChanges());
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateDevisComplementaire(ObjDEVIS devis, ObjDEPOSIT deposit, List<ObjELEMENTDEVIS> elements)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            DBDEVIS.Update(context, devis);
        //            DBDEPOSIT.UpdateDeposit(context, deposit);
        //            DBELEMENTSDEVIS.InsertElementsDevis(elements, context);
        //            return Convert.ToBoolean(context.SaveChanges());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static bool UpdateAnnulationDevis(ObjDEVIS entity, CsUserConnecte agent)
        //{
        //    try
        //    {
        //        using (galadbEntities context = new galadbEntities())
        //        {
        //            if (entity != null && agent != null)
        //            {
        //                ObjETAPEDEVIS nextEtape = new ObjETAPEDEVIS();
        //                entity.DATEETAPE = DateTime.Now;
        //                nextEtape = DBService.GetEtapeAnnulation(entity);
        //                int idEtapeSuivi = (int)entity.FK_IDETAPEDEVIS;
        //                entity.FK_IDETAPEDEVIS = nextEtape.PK_ID;

        //                DBDEVIS.Update(context, entity);
        //                ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
        //                DateTime date, currentDate;
        //                currentDate = DateTime.Now.Date;
        //                TimeSpan difference;
        //                double delai;

        //                date = (DateTime)entity.DATEETAPE;
        //                difference = (currentDate.Date - date.Date);
        //                delai = difference.TotalDays;

        //                ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

        //                ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
        //                ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

        //                if (suivi != null && suivi.FK_IDDEVIS == entity.PK_ID)
        //                {
        //                    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
        //                        (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
        //                        && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
        //                    {
        //                        suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
        //                    }

        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.USERMODIFICATION = agent.matricule;
        //                    suivi.DATEMODIFICATION = DateTime.Now;
        //                    DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
        //                }
        //                else
        //                {
        //                    suivi = new ObjSUIVIDEVIS();
        //                    suivi.DUREE = Convert.ToInt32(delai);
        //                    suivi.MATRICULEAGENT = agent.matricule;
        //                    suivi.NUMDEVIS = entity.NUMDEVIS;
        //                    suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
        //                {
        //                    suivi.MATRICULEAGENT = null;
        //                    suivi.COMMENTAIRE = null;
        //                    suivi.DUREE = null;
        //                    suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
        //                    suivi.FK_IDDEVIS = entity.PK_ID;
        //                    suivi.USERCREATION = agent.matricule;
        //                    suivi.DATECREATION = DateTime.Now;
        //                    DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
        //                }
        //                return System.Convert.ToBoolean(context.SaveChanges());
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //#endregion

         
 
        public static CsCriteresDevis GetParametresDistance(string pMaxi, string pSeuil, string pMaxiSubvention)
        {
            CsCriteresDevis ParametresDistance = new CsCriteresDevis();
            try
            {
                DB_ParametresGeneraux dbParamGeneraux = new DB_ParametresGeneraux();
                if (!string.IsNullOrEmpty(pMaxi))
                {
                    var ParamMaxi = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxi);
                    if (ParamMaxi != null)
                        ParametresDistance.Maxi = decimal.Parse(ParamMaxi.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pSeuil))
                {
                    var ParamSeuil = dbParamGeneraux.SelectParametresGenerauxByCode(pSeuil);
                    if (ParamSeuil != null)
                        ParametresDistance.Seuil = decimal.Parse(ParamSeuil.LIBELLE);
                }
                if (!string.IsNullOrEmpty(pMaxiSubvention))
                {
                    var ParamMaxiSubvention = dbParamGeneraux.SelectParametresGenerauxByCode(pMaxiSubvention);
                    if (ParamMaxiSubvention != null)
                        ParametresDistance.MaxiSubvention = decimal.Parse(ParamMaxiSubvention.LIBELLE);
                }
                return ParametresDistance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool UpdateDevisValidation(ObjDEVIS entity, List<ObjELEMENTDEVIS> _lElements, ObjMATRICULE agent, int idEtapeSuivi)
        {
            try
            {
                using (galadbEntities context = new galadbEntities())
                {
                        if (DBELEMENTSDEVIS.UpdateElementsDevisValide(_lElements, context))
                        {
                            ObjSUIVIDEVIS suivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, idEtapeSuivi);
                            DateTime date, currentDate;
                            currentDate = DateTime.Now.Date;
                            TimeSpan difference;
                            double delai;

                            date = (DateTime)entity.DATEETAPE;
                            difference = (currentDate.Date - date.Date);
                            delai = difference.TotalDays;

                            ObjSUIVIDEVIS newSuivi = DBSUIVIDEVIS.GetSuiviDevisByDevisIdEtape(entity.PK_ID, (int)entity.FK_IDETAPEDEVIS);

                            ObjETAPEDEVIS etape = DBETAPEDEVIS.GetById(idEtapeSuivi);
                            ObjETAPEDEVIS next = DBETAPEDEVIS.GetById(entity.FK_IDETAPEDEVIS);

                            //if (suivi != null && suivi.NUMDEVIS == entity.NUMDEVIS)
                            //{
                            //    if (next.NUMETAPE == (int)Enumere.EtapeDevis.Annulé ||
                            //        (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi != null
                            //        && !string.IsNullOrEmpty(entity.MOTIFREJET) && etape.NUMETAPE > next.NUMETAPE))
                            //    {
                            //        suivi.COMMENTAIRE += "\r\n* " + entity.MOTIFREJET; // cas de rejet du devis
                            //    }

                            //    //suivi.DUREE = Convert.ToInt32(delai);
                            //    //suivi.MATRICULEAGENT = agent.MATRICULE;
                            //    //suivi.USERMODIFICATION = agent.USERMODIFICATION;
                            //    //suivi.DATEMODIFICATION = DateTime.Now;
                            //    //DBSUIVIDEVIS.UpdateSuiviDevis(context, suivi);
                            //}
                            //else
                            //{
                            //    //suivi = new ObjSUIVIDEVIS();
                            //    //suivi.DUREE = Convert.ToInt32(delai);
                            //    //suivi.MATRICULEAGENT = agent.MATRICULE;
                            //    //suivi.NUMDEVIS = entity.NUMDEVIS;
                            //    //suivi.FK_IDETAPEDEVIS = idEtapeSuivi;
                            //    //suivi.USERCREATION = agent.MATRICULE;
                            //    //suivi.DATECREATION = DateTime.Now;
                            //    //DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
                            //}

                            if (entity.FK_IDETAPEDEVIS != idEtapeSuivi && newSuivi == null)
                            {
                                //suivi.MATRICULEAGENT = null;
                                //suivi.COMMENTAIRE = null;
                                //suivi.DUREE = null;
                                //suivi.FK_IDETAPEDEVIS = (int)entity.FK_IDETAPEDEVIS;
                                //suivi.USERCREATION = agent.MATRICULE;
                                //suivi.DATECREATION = DateTime.Now;
                                //DBSUIVIDEVIS.InsertSuiviDevis(context, suivi);
                            }
                            new DBAccueil().ValiderDemandeByDevis(entity, context);
                            return Convert.ToBoolean(context.SaveChanges());
                        }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
 