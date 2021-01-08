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
using Galatee.Silverlight.ServiceAccueil ;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;



namespace Galatee.Silverlight
{
    public class DataReferenceManager
    {
        public static string ReaffectationFormeName = "UcListReAffectation";
        public const string Eau = "01";
        public const string Electricite = "03";
        public const string ElectriciteMT = "04";
        public const string Prepaye = "05";
        public const string ProduitInconnu = "99";
        public const string CentreInconnu = "000";
        public const string CommuneInconnue = "00000";
        public const string RueInconnue = "00000";
        public const string QuartierInconnu = "00000";
        //Voir Table TA num = 12
        public const string BranchementSociaux = "00011";


        public const string FormatMontant = "N2";
        public const string ParametreDistanceMaxiElectricite = "000209";
        public const string ParametreDistanceMaxiEau = "000210";
        public const string ParametreSeuilDistanceAdditionnelleElectricite = "000211";
        public const string ParametreSeuilDistanceAdditionnelleEau = "000212";
        public const string ParametreDistanceMaxiEauSubvention = "000216";
        public const string ParametreGestionSaisieCompteur = "000230";
        //#region Produits

     

        

        public static bool ConvertVisibilityToBoolean(System.Windows.Visibility visibilite)
        {
            if (visibilite == Visibility.Visible)
                return true;
            else
                return false;
        }

        public static System.Windows.Visibility ConvertBooleanToVisibility(bool visibilite)
        {
            if (visibilite)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }


        public static void Control_Enabled(Control elt, bool Activer)
        {
            if (!Activer)
            {
                //SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, 194, 194, 194));
                elt.Background = brush;
                elt.IsEnabled = false;
            }
            else
            {
                SolidColorBrush brush = new SolidColorBrush(Colors.White);
                elt.Background = brush;
                elt.IsEnabled = true;
            }
        }
      


        //public event PropertyChangedEventHandler PropertyChanged;

        //private void Notifier(string Proriete)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(Proriete));
        //    }
        //}

        public enum EtapeDevis
        {
            Annulé = -1,                 //indice -1
            DossierCloturé = 0,          //indice 0
            Accueil = 1,
            AffectationTache = 2,
            Survey = 3,
            ValidationTechnique = 4,
            Facturation = 5,
            ValidationEtude = 6,
            VerificationParticipation = 7,
            EditionDevis = 8,
            ValidationDevis = 9,
            Encaissement = 10,
            AnalyseDossier = 11,
            ValidationDossier = 12,
            BonDeSortie = 13,
            ProcesVerbal = 14,
            DecisionControle = 15,
            Controle = 16,
            ValidationControleCloture = 17,
            AnnulationDevis = 18,
            SaisieDevisComplementaire = 19 
        }

        public enum Reedition
        {
            BonDeSortie = 1,
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
        public enum TypeDevis
        {
            ConnectionElectricity = 1,
            ExtensionElectricity,
            ConnectionEau,
            SeparationEau,
            ConnectionCashPower,
            ExtensionEau,
            SeparationElectricity
        }

        public static bool IsDecimal(string pChaine)
        {
            try
            {
                if (!string.IsNullOrEmpty(pChaine))
                {
                    var regex = new Regex(@"[\d]{1,4}([.,][\d]{1,2})?");
                    return regex.IsMatch(pChaine);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int RetourneTypeDedition(int pMenuId)
        {
            switch (pMenuId)
            {
                case 1800101:
                    return 4;
                case 1800102:
                    return 1;
                case 1800103:
                    return 2;
                case 1800104:
                    return 3;
                case 1800105:
                    return 7;
                case 1800106:
                    return 8;
                default:
                    return 0;
            }
        }
        public static List<CsCanalisation> CodificationCompteurMt(List<CsCanalisation> lstCannalisation)
        {
            try
            {
                foreach (CsCanalisation item in lstCannalisation)
                {
                    if (item.PRODUIT == SessionObject.Enumere.ElectriciteMT)
                    {
                        if (item.POINT == 1)
                        {
                            item.NUMERO = "AHPO-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptagePoint;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptagePoint).PK_ID;
                            item.LIBELLETYPECOMPTEUR  = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptagePoint).LIBELLE ;
                        }
                        else if (item.POINT == 2)
                        {
                            item.NUMERO = "AHPL-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptagePleinne;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptagePleinne).PK_ID;
                            item.LIBELLETYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptagePleinne).LIBELLE;

                        }

                        else if (item.POINT == 3)
                        {
                            item.NUMERO = "AHCR-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptageCreuse;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageCreuse).PK_ID;
                            item.LIBELLETYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageCreuse).LIBELLE;

                        }

                        else if (item.POINT == 4)
                        {
                            item.NUMERO = "REAC-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptageReactif;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageReactif).PK_ID;
                            item.LIBELLETYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageReactif).LIBELLE;

                        }

                        else if (item.POINT == 5)
                        {
                            item.NUMERO = "HORA-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptageHoraire;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageHoraire).PK_ID;
                            item.LIBELLETYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageHoraire).LIBELLE;
                        }

                        else if (item.POINT == 6)
                        {
                            item.NUMERO = "MAXI-" + item.NUMERO;
                            item.TYPECOMPTEUR = SessionObject.Enumere.TypeComptageMaximetre;
                            item.FK_IDTYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageMaximetre).PK_ID;
                            item.LIBELLETYPECOMPTEUR = SessionObject.LstTypeCompteur.FirstOrDefault(t => t.CODE == SessionObject.Enumere.TypeComptageMaximetre).LIBELLE;

                        }
                    }
                }
                return lstCannalisation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
