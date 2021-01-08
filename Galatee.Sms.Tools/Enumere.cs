using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASCEnvoiFacturesCIETools
{
    public class Enumere
    {
        #region StatusCompteUtilisateur 

        public enum StatusCompteUtilisateur
        {
            Actif = 1,
            Inactif,
            Verrouille,
            VerrouilleOuvertureSession
        };

        #endregion

        #region StatusEnvoiMail

        public enum StatusEnvoiMail
        {
            NonEnvoyer = 0,
            Envoyer
        };

        #endregion

        #region StatusEnvoiSms

        public enum StatusEnvoiSms
        {
            NonEnvoyer = 0,
            Envoyer
        };

        #endregion

        #region StatusRejet

        public enum StatusRejet
        {
            PasRejeter =0,
            Rejeter = 1
        };

        #endregion

        #region "Operation de Validation" 
        public enum OperationValidation
        { 
            valider=0,
            rejeter,
            envoyer
        }
        #endregion

        #region EtapeWorkflow

        public enum EtapeWorkflow
        {
            Importer = 1,
            Valider,
            Envoyer
        };

        #endregion

        #region StatutRecherche

        public enum StatutRecherche
        {
            ValideEnvoye = 0,
            AValiderAEnvoyer
        };

        #endregion

    }
}