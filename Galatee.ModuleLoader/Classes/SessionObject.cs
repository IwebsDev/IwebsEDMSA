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
using Galatee.ModuleLoader.ServiceAuthenInitialize;
using Galatee.ModuleLoader.ServiceCaisse;
using System.Collections.Generic;

namespace Galatee.ModuleLoader
{
    public static class SessionObject
    {
        public static ServiceAuthenInitialize.EnumereWrap Enumere;
        public static ServiceAuthenInitialize.EnumProcedureStockee EnumereProcedureStockee;
        public static string EtatCaisse;
        public static ServiceCaisse.CsOpenningDay CaisseOverte;
        public static string NatureByLibelleCourtFraisCheqImpaye = string.Empty;
        public static string NatureByLibelleCourtCheqImpaye = string.Empty;
        public static string moisComptable="201211";

        public static List<string> Imprimantes = new List<string>();
        //public static List<Galatee.Silverlight.ServiceCaisse.CParametre> Imprimantes = new List<Galatee.Silverlight.ServiceCaisse.CParametre>();

        public  enum sens
        {
            sup = 1,
            inf = -1,
            egal = 0
        }

        public enum frequenceMoratoire
        {
            Month = 1,
            ForNight = 2,
            Week = 3
        }

        public static string[] LoadComboBoxData()
        {
            string[] strArray =
                {

                    "=",
                    ">=",
                    "<="
                };
            return strArray;

        }

        public static string[] Cret()
        {
            string[] strArray =
                {

                    "A","B","C","D","E","F",
                    "G","H","I","J","K","L"
                };
            return strArray;

        }

        public static string[] TypeOperationClasseur()
        {
            string[] strArray =
                {
                    "ALL","PAYMENT","BILL","UNPAID"
                };
            return strArray;

        }

        // creation du dictionnaire des tables TA0 pour la creation dynamique des child window
        // par HGB 26/12/2012
        public static Dictionary<int, string>  getTableDetails()
        {
           Dictionary<int, string> codeToWindow = new Dictionary<int, string>() { 
          
            {0, "FrmGeneric"},
            {1001, "UcINIT"},
            {1002, "UcRue"},
            {1003, "UcREGROU"},
            {1005, "UcDIACOMP"},
            {1006, "UcTCOMPT"},
            {1007, "UcPuissance"},
            {1010, "UcTARIF"},
            {1011, "UcFORFAIT"},
            {1014, "UcBANQUES2"},
            {1016, "UcCASIND"},
            {1023, "UcCTAX"},
            {1025, "UcMATRICULE"},
            {1034, "UcTDEM"},
            {1038, "UcREGCLI"},
            {1039, "FrmClientRegrp"},
            {1041, "UcGEOGES"},
            {1044, "UcQUARTIER"},
            {1046, "UcMESSAGE"},
            {1048, ""},
            {1049, ""},
            {1053, "UcSPESITE"},
            {1054, ""},
            {1055, "UcREGEXO"},
            {1058, "UcDIRECTEUR"},
            {1059, "UcDEMCOUT"},
            {1060, "UcMODEREG"},
            {1061, "UcFRAISTIMB"},
            {1062, "UcFRAISHP"},
            {1063, ""},
            {1064, "UcCOPER"},
            {1065, "UcNATURE"},
            {1066, "UcDOMBANC"},
            {1067, "UcCOPEROD"},
            {1068, "UcARRETE"},
            {1069, ""},
            {1070, "UcTAXCOMP"},
            {1071, "UcIMPRIM"},
            {1072, "UcREDEVANCE"},
            {1073, "UcNATGEN"},
            {1074, "UcSCHEMAS"},
            {1075, "UcAJUFIN"},
            {1076, "UcFRAISCONTENTIEUX"},
            {1077, "UcLIBRUBACTION"},
            {1078, "UcSECURITEMATRICULE"},
            {1079, "UcMONNAIE"},
            {1080, "UcDEFPARAMABON"},
            {1081, "UcPARAMABONUTILISE"},
            {1082, "UcCENTREENCAISSABLE"},
            {1083, "UcTYPELOT"},
            {1084, "UcCONTROLELIGNE"},
            {1085, "UcCONTROLESECONDAIRE"},
            {1086, "UcCONTROLEPIECE"},
            {1087, ""},
            {1088, ""},
            {1089, ""},
            {1090, ""},
            {1091, ""},
            {1094, ""},
            {1100, "UcMULTIMONNAIES"},
            {1101, "UcCodeControle"},
            {1102, "UcOrigineLot"},
            {1107, "UcSECTEUR"},
            {1113, "UcCOUTPUISSANCE"},
            {1116, "UcTYPEBRANCHEMENT"},
            {1122, "UcPUISPERTES"},
            {1123, "UcCOMPTAGE"},
            {1124, "UcTRANSFOCOMPTAGE"},
            {2000, "FrmGenericDevis"},
            {2001, "UcTypeDevis"},
            {2002, "UcAppareils"},
            {2003, "UcFourniture"},
            {2004, "UcCaracteristiques"},
            {2005, "UcEtapeDevis"},
            {2006, "UcETAPERECLAMATION"},
            {2007, "UcTYPERECLAMATION"},
            {2008, "UcETAPEFONCTION"},
            {2009, "UcPRESTATAIRE"},
            {2010, "UcStatutReclamation"},
            {2011, "UcGroupeReclamation"},
            {2012, "UcGroupProgram"},
            {2013, "UcProgram"},
            {2014, "UcHabilitationProgram"},
            {2015, "UcProduit"},
            {2016, "UcMotifRejet"},
            {2017, "UcFonction"},
            {2018, "UcCentre"},
            {2019, "UcSite"},
            {2020, "UcMotifRejet"},
            {2021, "UcEtapeFonctionCheque"},
            {2047, "UcTypeCentre"},
            {2048, "UcHabilitationCheque"}
            //putain quelle tache fastidieuz   26/12/2012 HGB 
        };
           return codeToWindow;
    }
       
    }
}
