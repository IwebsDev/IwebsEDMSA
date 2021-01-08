using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight
{
    /// <summary>
    /// Constantes et enumeration GalateeDevis
    /// </summary>
    /// 

    public class GalateeManager
    {

        #region Constantes

        public const string Const_Eau = "01";
        public const string Const_Electricite = "03";


        #endregion



        #region Methodes

        public static void ShowMessageBox(string msg)
        {
            MessageBox.Show(msg, "GALATEE", MessageBoxButton.OK);
        }
        public static bool ShowQuestionBox(string msg)
        {

            return (MessageBox.Show(msg, "GALATEE", MessageBoxButton.OKCancel) == MessageBoxResult.OK);
        }

        #endregion



        #region Enumeration
        /// <summary>
        /// 
        /// </summary>
        public enum EnumEtapeDevis
        {
            Annulé = -1,                 //indice -1
            DossierCloturé = 0,          //indice 0
            Accueil,
            AffectationTache,
            Survey,
            ValidationTechnique,
            Facturation,
            ValidationEtude,
            VerificationParticipation,
            EditionDevis,
            ValidationDevis,
            Encaissement,
            AnalyseDossier,
            ValidationDossier,
            BonDeSortie,
            ProcesVerbal,
            DecisionControle,
            Controle,
            ValidationControleCloture


        }

        public enum EnumReedition
        {
            BonDeSortie,
            BonTravaux,
            ProcesVerbal,
            Devis,
            BonControle,
            RapportControle,
            BonEntreeStock,
            Bilan,
            Cancellation,
            DevisComplementaire
        }



        public enum EditiqueEnum
        {
            BonDeSortie,
            BonTravaux,
            ProcesVerbal,
            Devis,
            BonControle,
            RapportControle,
            BonEntreeStock,
            Bilan,
            Devis_Eng,
            BonDeSortie_Eng,
            BonTravaux_Eng,
            ProcesVerbal_Eng,
            Bilan_Eng,
            Repartition,
            Devis_Separation,
            DevisSubvention


        }


        public enum EnumEtatLogin
        {
            Valide,
            Inconnu,
            MotDePasseLoginIncompatible,
            ProblemeDeValidation
        }


        public enum EnumTypeDevis
        {
            ConnectionElectricity = 1,
            ExtensionElectricity,
            ConnectionEau,
            SeparationEau,
            ConnectionCashPower,
            ExtensionEau,
            SeparationElectricity
        }


        /// <summary>
        /// 
        /// </summary>
        public enum EnumExecutionMode
        {
            Creation,
            Modification,
            Consultation
        } 

        #endregion
    }
}
