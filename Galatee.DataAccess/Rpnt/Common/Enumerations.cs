using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galatee.DataAccess.Common
{
    public enum EnumCompteurIdentifiantTechniqueID
    {
        BET = 0,
        TypeAnomalieBTA = 1,
        TypeAnomalieHTA = 2
    }

    #region Enumérés utilisés par la Classe Messages 

    public enum EnumAppMessage
    {
        AucunMessageDisponible = 0,
        ConnexionImpossible = 1,
        ConnexionUtilisateur = 2,
        AppLibelleLong = 3,
        AppLibelleCourt = 4,
        UniteOrganisationnelle = 5,
        QuitterAppli = 6
    }

    #endregion

    #region Enumérés Concernant la synchronisation

    public enum EnumTypeSynchronisation
    {
        Totale = 0,
        Differentielle = 1
    }
    public enum EnumSynchroEtapeEnCours
    {
        DemandeInitiee = 0,
        Etape01_PRESynchro_Agents = 1,
        Etape02_PRESynchro_BranchementsBTA = 2,
        Etape03_PRESynchro_AbonnesBTA = 3,
        Etape04_PRESynchro_ConsommationsBTA = 4,
        //Etape05_PRESynchro_CompteursBTA = 5,
        Etape05_PRESynchro_BET = 5,
        //Etape07_PRESynchro_CompteursHTA = 7,
        Etape06_PRESynchro_RaccordementsHTA = 6,
        Etape07_PRESynchro_AbonnesHTA = 7,
        Etape08_PRESynchro_ConsommationsHTA = 8,
        PRESynchro_Terminee_OK = 89,
        Etape11_Agents = 11,
        Etape12_AbonnesBTA = 12,
        Etape13_CompteursBTA = 13,
        Etape14_BranchementsBTA = 14,
        Etape15_ConsommationsBTA = 15,
        Etape16_BET = 16,
        Etape17_CompteursHTA = 17,
        Etape18_AbonnesHTA = 18,
        Etape19_RaccordementsHTA = 19,
        Etape20_ConsommationsHTA = 20,
        SynchroEnCoursTerminee = 90,
        SynchronisationInterrompueParUser = 91,
        SynchronisationTerminee_CauseErreurAccesGesabel = 92,
        SynchronisationTerminee_CauseException = 93,
        PRESynchro_Interrompue_CauseException = 94,
        SynchronisationTerminee_CauseErreurAccesBaseSynchro = 95
    }
    public enum EnumSynchroCodeFinDeSynchro
    {
        TraitementOK = 0,
        TraitementOKAvecDesErreurs = 1,
        TraitementInterrompuParLUtilisateur = 2,
        TraitementInterrompuSuiteAUneException = 3
    }
    public enum EnumPeriodiciteSynchroAutomatique
    {
        Journalier = 1,
        Hebdomadaire = 2,
        Mensuel = 3
    }

    #endregion

    #region Enumérés Campagnes de contrôle

    public enum EnumStatutCampagne
    {
        Initialise = 1,
        RechercheDeClientsAControlerEnCours = 2,
        LotsDeControleEnCoursDeCreation = 3,
        LotsDeControleCrees = 4,
        EditionDesLotsDeControleEnCours = 5,
        EditionDesLotsDeControleTerminee = 6,
        SaisieDesCRDeControleEncours = 7,
        SaisieDesCRDeControleTerminee = 8,
        Cloture = 9
    }
    public enum EnumStatutLotDeControle
    {
        PopulationDuLotEnCours = 1,
        Edite = 2,
        CompteRenduDeControleEnCours = 3,
        CompteRenduTemine = 4
        //CompteRenduValide = 5
    }
    public enum EnumValeurResultatControle
    {
        RAS = 1,
        AnomaliesDetectees = 2
    }
    public enum EnumFamilleAnomalie
    {
        ASA = 3
    }
    public enum EnumTypeDeControle
    {
        EnCampagne = 1,
        Isole = 2,
        PostFraude = 3
    }

    #endregion

    #region Enumérés sur les Tables de référence

    public enum EnumCasBTAHTA
    {
        BTA = 1,
        HTA = 2
    }
    public enum EnumRefDonneesDeReference
    {
        FamilleAnomalieBTA = 1,
        TypeAnomalieBTA = 2,
        FamilleAnomalieHTA = 3,
        TypeAnomalieHTA = 4,
        Exploitations = 5,
        NiveauUnit = 6
    }
    public enum EnumRefTypeComptageHTA
    {
        HTAS = 1,
        HTAP = 2,
        HTBP = 3,
        HTBS = 4,
        HTASP = 5,
        HTBSP = 6
    }
    public enum EnumRefMethodeDeDetectionClientsBTA
    {
        MethodeDesChutesDeConso = 1,
        MethodeIncoherencePS_Consommation = 2,
        //MethodeDeLaPSSousEvaluee = 3
        //MethodeDesTendancesDeConso = 4
    }
    public enum EnumRefMethodeDeDetectionClientsHTA
    {
        MethodeDesChutesDeConso = 1
    }
    public enum EnumRefUsage
    {
        Domestique = '1',
        Professionnel = '2',
        EclairagePublic = '3'
    }

    #endregion

    #region Enumérés pour Adm

    public enum EnumStatutUser
    {
        Actif = 1,
        Inactif = 2,
        Supprime = 3,
        VerrouilleOuvertureSession = 4
    }

    #endregion

    #region Enumérés sur les paramètres

    public enum EnumParametreID
    {
        NbreCyclesFacturation = 1,
        CoeffiscientDeFoisonnement = 2,
        SeuilConsoSupAPS = 3,
        SeuilChutesDeConso = 4,
        NbreCyclesFacturationHT = 5
    }
    public enum EnumTypeValeurParametre
    {
        Entier = 1,
        Decimale = 2,
        Chaine = 3,
        Pourcentage = 4
    }

    #endregion

    #region Enumérés sur les Traces d'audit

    public enum EnumTypeActionAudite
    {
        Connexion = 1,
        Creation = 2,
        Consultation = 3,
        Modification = 4,
        Suppression = 5,
        Impression = 6
    }

    #endregion

    #region Enumérés sur les Algorithmes de recherche

    #endregion

    #region Enumérés sur les données importées

    public enum EnumTypeContratBTA
    {
        Standard = 1,
        Prepaiement = 2,
        Forfaitaire = 3
    }
    public enum EnumTypeCompteurBTA
    {
        Monophase = 1,
        Triphase = 3
    }
    public enum EnumTypeClientBTA
    {
        Particulier = 1,
        Societe = 2,
        Administration = 3
    }
    public enum EnumStatutContrat
    {
        Actif = 1,
        Resilie = 2
    }
    public enum EnumPeriodiciteFacturation
    {
        Mensuel = 1,
        Bimestriel = 2
    }

    #endregion

    #region Enuméré Statut BET
    public enum EnumStatutBET
    {
        Traité = 1,
        Pas_traité = 2
    }
    #endregion

    public static class GetValuesEnum
    {
        public static string GetLibelleEnumStatutUser(EnumStatutUser lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumStatutUser.Actif:
                    leLibelle = "Actif";
                    break;
                case EnumStatutUser.Inactif:
                    leLibelle = "Désactivé";
                    break;
                case EnumStatutUser.Supprime:
                    leLibelle = "Supprimé";
                    break;
                case EnumStatutUser.VerrouilleOuvertureSession:
                    leLibelle = "Verrouillé";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumStatutCampagne(EnumStatutCampagne lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumStatutCampagne.Initialise:
                    leLibelle = "Initialisé";
                    break;
                case EnumStatutCampagne.RechercheDeClientsAControlerEnCours:
                    leLibelle = "Sélection de clients en cours";
                    break;
                case EnumStatutCampagne.LotsDeControleEnCoursDeCreation:
                    leLibelle = "Lots de contrôle en cours";
                    break;
                case EnumStatutCampagne.LotsDeControleCrees:
                    leLibelle = "Lots de contrôle créés";
                    break;
                case EnumStatutCampagne.EditionDesLotsDeControleEnCours:
                    leLibelle = "Edition des lots de contrôle";
                    break;
                case EnumStatutCampagne.EditionDesLotsDeControleTerminee:
                    leLibelle = "Lots de contrôle édités";
                    break;
                case EnumStatutCampagne.SaisieDesCRDeControleEncours:
                    leLibelle = "Compte-rendu de contrôle en cours";
                    break;
                case EnumStatutCampagne.SaisieDesCRDeControleTerminee:
                    leLibelle = "Compte-rendu de contrôle terminé";
                    break;
                case EnumStatutCampagne.Cloture:
                    leLibelle = "Clôturé";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumStatutContrat(EnumStatutContrat lEnum)
        {
            string leLibelle = string.Empty;
            switch (lEnum)
            {
                case EnumStatutContrat.Actif:
                    leLibelle = "Actif";
                    break;
                case EnumStatutContrat.Resilie:
                    leLibelle = "Resilie";
                    break;
                default:
                    break;
            }
            return leLibelle;
        }
        public static string GetLibelleEnumStatutBET(EnumStatutBET lEnum)
        {
            string leLibelle = string.Empty;
            switch (lEnum)
            {
                case EnumStatutBET.Traité:
                    leLibelle = "Traité";
                    break;
                case EnumStatutBET.Pas_traité:
                    leLibelle = "Pas encore traité";
                    break;
                default:
                    break;
            }
            return leLibelle;
        }
        public static string GetLibelleEnumParametre(EnumParametreID leParametre)
        {
            string leLibelle = string.Empty;

            switch (leParametre)
            {
                case EnumParametreID.NbreCyclesFacturation:
                    leLibelle = "Nombre de cycles facturation";
                    break;
                case EnumParametreID.CoeffiscientDeFoisonnement:
                    leLibelle = "Seuil de Consommation inférieur à la Puissance souscrite";
                    break;
                case EnumParametreID.SeuilConsoSupAPS:
                    leLibelle = "Seuil de Consommation supérieur à la Puissance souscrite";
                    break;
                case EnumParametreID.SeuilChutesDeConso:
                    leLibelle = "Seuil de tolérance des tendances de consommation";
                    break;
                case EnumParametreID.NbreCyclesFacturationHT:
                    leLibelle = "Nombre de cycles facturation pour la HT";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumValeurResultatControle(EnumValeurResultatControle lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumValeurResultatControle.RAS:
                    leLibelle = "R.A.S";
                    break;
                case EnumValeurResultatControle.AnomaliesDetectees:
                    leLibelle = "Anomalies détectées";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumTypeValeurParametre(EnumTypeValeurParametre leType)
        {
            string leLibelle = string.Empty;

            switch (leType)
            {
                case EnumTypeValeurParametre.Entier:
                    leLibelle = "Nombre entier";
                    break;
                case EnumTypeValeurParametre.Decimale:
                    leLibelle = "Nombre décimal";
                    break;
                case EnumTypeValeurParametre.Chaine:
                    leLibelle = "Chaîne de caractères";
                    break;
                case EnumTypeValeurParametre.Pourcentage:
                    leLibelle = "Pourcentage ([0-100])";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumStatutLotDeControle(EnumStatutLotDeControle lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumStatutLotDeControle.PopulationDuLotEnCours:
                    leLibelle = "Lot de contrôle créé";
                    break;
                case EnumStatutLotDeControle.Edite:
                    leLibelle = "Bordereau de contrôle édité";
                    break;
                case EnumStatutLotDeControle.CompteRenduDeControleEnCours:
                    leLibelle = "Compte-rendu de contrôle en cours";
                    break;
                case EnumStatutLotDeControle.CompteRenduTemine:
                    leLibelle = "Saisie C.R. terminée";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumTypeActionAudite(EnumTypeActionAudite lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumTypeActionAudite.Connexion:
                    leLibelle = "Connexion / Déconnexion utilisateur";
                    break;
                case EnumTypeActionAudite.Creation:
                    leLibelle = "Création";
                    break;
                case EnumTypeActionAudite.Consultation:
                    leLibelle = "Consultation";
                    break;
                case EnumTypeActionAudite.Modification:
                    leLibelle = "Modification";
                    break;
                case EnumTypeActionAudite.Suppression:
                    leLibelle = "Suppression";
                    break;
                case EnumTypeActionAudite.Impression:
                    leLibelle = "Impression";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumTypeCompteurBTA(EnumTypeCompteurBTA lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumTypeCompteurBTA.Monophase:
                    leLibelle = "Monophasé";
                    break;
                case EnumTypeCompteurBTA.Triphase:
                    leLibelle = "Triphasé";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumTypeClientBTA(EnumTypeClientBTA lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            { 
                case EnumTypeClientBTA.Particulier:
                    leLibelle = "Particulier";
                    break;
                case EnumTypeClientBTA.Societe :
                    leLibelle = "Société";
                    break;
                case EnumTypeClientBTA.Administration:
                    leLibelle = "Administration";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumPeriodiciteFacturation(EnumPeriodiciteFacturation lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumPeriodiciteFacturation.Mensuel:
                    leLibelle = "Mensuel";
                    break;
                case EnumPeriodiciteFacturation.Bimestriel:
                    leLibelle = "Bimestriel";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumTypeContratBTA(EnumTypeContratBTA lEnum)
        {
            string leLibelle = string.Empty;

            switch (lEnum)
            {
                case EnumTypeContratBTA.Standard:
                    leLibelle = "Standard BT";
                    break;
                case EnumTypeContratBTA.Prepaiement:
                    leLibelle = "Prépayé";
                    break;
                case EnumTypeContratBTA.Forfaitaire:
                    leLibelle = "Forfaitaire";
                    break;
                default:
                    break;
            }

            return leLibelle;
        }
        public static string GetLibelleEnumCodeFinSynchronisation(EnumSynchroCodeFinDeSynchro leResultat)
        {
            string libelle = string.Empty;

            switch (leResultat)
            {
                case EnumSynchroCodeFinDeSynchro.TraitementOK:
                    libelle = "Ok";
                    break;
                case EnumSynchroCodeFinDeSynchro.TraitementOKAvecDesErreurs:
                    libelle = "Ok - Avec des rejets";
                    break;
                case EnumSynchroCodeFinDeSynchro.TraitementInterrompuParLUtilisateur:
                    libelle = "Interrompu par l'utilisateur";
                    break;
                case EnumSynchroCodeFinDeSynchro.TraitementInterrompuSuiteAUneException:
                    libelle = "KO";
                    break;
                default:
                    break;
            }

            return libelle;
        }
        public static string GetLibelleEnumPeriodiciteSynchoAutomatique(EnumPeriodiciteSynchroAutomatique lEnum)
        {
            string libelle = string.Empty;

            switch (lEnum)
            {
                case EnumPeriodiciteSynchroAutomatique.Journalier:
                    libelle = "Une fois par Jour";
                    break;
                case EnumPeriodiciteSynchroAutomatique.Hebdomadaire:
                    libelle = "Une fois par semaine";
                    break;
                case EnumPeriodiciteSynchroAutomatique.Mensuel:
                    libelle = "Une fois par mois";
                    break;
                default:
                    break;
            }

            return libelle;
        }
        public static string GetLibelleEnumEtapeEnCoursServiceSynchronisation(EnumSynchroEtapeEnCours lEtape)
        {
            string libelle = string.Empty;

            switch (lEtape)
            {
                case EnumSynchroEtapeEnCours.DemandeInitiee:
                    libelle = "Demande de synchronisation initiée";
                    break;
                case EnumSynchroEtapeEnCours.Etape01_PRESynchro_Agents:
                    libelle = "(1/20) - PRE Chargement des agents...";
                    break;
                case EnumSynchroEtapeEnCours.Etape02_PRESynchro_BranchementsBTA:
                    libelle = "(2/20) - PRE Chargement des Branchements BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape03_PRESynchro_AbonnesBTA:
                    libelle = "(3/20) - PRE Chargement des Abonnés BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape04_PRESynchro_ConsommationsBTA:
                    libelle = "(4/20) - PRE Chargement des consommations BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape05_PRESynchro_BET:
                    libelle = "(5/20) - PRE Chargement des BET...";
                    break;
                case EnumSynchroEtapeEnCours.Etape06_PRESynchro_RaccordementsHTA:
                    libelle = "(6/20) - PRE Chargement des raccordements HTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape07_PRESynchro_AbonnesHTA:
                    libelle = "(7/20) - PRE Chargement des abonnés HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape08_PRESynchro_ConsommationsHTA:
                    libelle = "(8/20) - PRE Chargement des consommations HTA...";
                    break;
                case EnumSynchroEtapeEnCours.PRESynchro_Terminee_OK:
                    libelle = "PRE Chargement terminé";
                    break;
                case EnumSynchroEtapeEnCours.Etape11_Agents:
                    libelle = "(11/20) - Synchronisation des agents de l'UO...";
                    break;
                case EnumSynchroEtapeEnCours.Etape12_AbonnesBTA:
                    libelle = "(12/20) - Synchronisation des Abonnés BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape13_CompteursBTA:
                    libelle = "(13/20) - Synchronisation des Compteurs BTA de PNT...";
                    break;
                case EnumSynchroEtapeEnCours.Etape14_BranchementsBTA:
                    libelle = "(14/20) - Synchronisation des Branchements BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape15_ConsommationsBTA:
                    libelle = "(15/20) - Synchronisation des consommations BTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape16_BET:
                    libelle = "(16/20) - Synchronisation des BET...";
                    break;
                case EnumSynchroEtapeEnCours.Etape17_CompteursHTA:
                    libelle = "(17/20) - Synchronisation des Compteurs HTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape18_AbonnesHTA:
                    libelle = "(18/20) - Synchronisation des abonnés HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape19_RaccordementsHTA:
                    libelle = "(19/20) - Synchronisation des raccordements HTA...";
                    break;
                case EnumSynchroEtapeEnCours.Etape20_ConsommationsHTA:
                    libelle = "(20/20) - Synchronisation des consommations HTA...";
                    break;
                case EnumSynchroEtapeEnCours.SynchroEnCoursTerminee:
                    libelle = "Synchronisation terminée";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationInterrompueParUser:
                    libelle = "Synchronisation interrompue par l'utilisateur";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseErreurAccesGesabel:
                    libelle = "Erreur d'accès aux bases Source. Synchronisation interrompue";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseException:
                    libelle = "Synchronisation interrompue suite à une exception";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseErreurAccesBaseSynchro:
                    libelle = "Erreur d'accès à la base de synchro. Synchronisation interrompue";
                    break;
                default:
                    break;
            }

            return libelle;
        }
        public static string GetLibelleEnumEtapeEnCoursListViewSynchroEnCours(EnumSynchroEtapeEnCours lEtape)
        {
            string libelle = "";

            switch (lEtape)
            {
                case EnumSynchroEtapeEnCours.DemandeInitiee:
                    libelle = "Initialisation de la demande";
                    break;
                case EnumSynchroEtapeEnCours.Etape01_PRESynchro_Agents:
                    libelle = "1 - PRE Chargement Agents";
                    break;
                case EnumSynchroEtapeEnCours.Etape02_PRESynchro_BranchementsBTA:
                    libelle = "2 - PRE Chargement Branchements";
                    break;
                case EnumSynchroEtapeEnCours.Etape03_PRESynchro_AbonnesBTA:
                    libelle = "3 - PRE Chargement Abonnés BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape04_PRESynchro_ConsommationsBTA:
                    libelle = "4 - PRE Chargement consos BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape05_PRESynchro_BET:
                    libelle = "5 - PRE Chargement BET";
                    break;
                case EnumSynchroEtapeEnCours.Etape06_PRESynchro_RaccordementsHTA:
                    libelle = "6 - PRE Chargement raccordements";
                    break;
                case EnumSynchroEtapeEnCours.Etape07_PRESynchro_AbonnesHTA:
                    libelle = "7 - PRE Chargement abonnés HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape08_PRESynchro_ConsommationsHTA:
                    libelle = "8 - PRE Chargement consos HTA";
                    break;
                case EnumSynchroEtapeEnCours.PRESynchro_Terminee_OK:
                    libelle = "Pre Chargement Terminé";
                    break;
                case EnumSynchroEtapeEnCours.Etape11_Agents:
                    libelle = "11 - Synchro des Agents";
                    break;
                case EnumSynchroEtapeEnCours.Etape12_AbonnesBTA:
                    libelle = "12 - Abonnés BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape13_CompteursBTA:
                    libelle = "13 - MAJ Compteurs BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape14_BranchementsBTA:
                    libelle = "14 - Branchements BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape15_ConsommationsBTA:
                    libelle = "15 - Consommations BTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape16_BET:
                    libelle = "16 - BET";
                    break;
                case EnumSynchroEtapeEnCours.Etape17_CompteursHTA:
                    libelle = "17 - Compteurs HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape18_AbonnesHTA:
                    libelle = "18 - Abonnés HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape19_RaccordementsHTA:
                    libelle = "19 - Raccordements HTA";
                    break;
                case EnumSynchroEtapeEnCours.Etape20_ConsommationsHTA:
                    libelle = "20 - Consommations HTA";
                    break;
                case EnumSynchroEtapeEnCours.SynchroEnCoursTerminee:
                    libelle = "Synchro terminée";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationInterrompueParUser:
                    libelle = "Synchro interrompue";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseErreurAccesGesabel:
                    libelle = "Arrêt - Erreur accès Base source";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseException:
                    libelle = "Arrêt - Exception enregistrée";
                    break;
                case EnumSynchroEtapeEnCours.PRESynchro_Interrompue_CauseException:
                    libelle = "Arrêt PRE Synchro - Exception enregistrée";
                    break;
                case EnumSynchroEtapeEnCours.SynchronisationTerminee_CauseErreurAccesBaseSynchro:
                    libelle = "Arrêt - Erreur accès Base synchro";
                    break;
                default:
                    break;
            }
            return libelle;
        }
        public static string GetLibelleEnumFamilleAnomalie(EnumFamilleAnomalie lEnum)
        {
            string leLibelle = string.Empty;
            switch (lEnum)
            {
                case EnumFamilleAnomalie.ASA:
                    leLibelle = "Anomalie sur Alimentation";
                    break;
                default:
                    break;
            }
            return leLibelle;
        }
        //public static string GetLibelleEnum__(lEnum)
        //{
        //    string leLibelle = string.Empty;


        //    return leLibelle;
        //}
    }
}
