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
using Galatee.Workflow;
using Galatee.WorkflowManager;

namespace Galatee.Silverlight.Workflow
{
    public class DemandeWorkflowInformation
    {
        public int IDETAPE { get; set; }
        public string NOM { get; set; }
        public Nullable<int> FK_IDETAPEACTUELLE { get; set; }
        public System.Guid IDCIRCUIT { get; set; }
        public string CODE { get; set; }
        public Nullable<System.DateTime> DATECREATION { get; set; }
        public Nullable<int> ETAPEPRECEDENTE { get; set; }
        public string MATRICULEUSERCREATION { get; set; }
        public System.Guid FK_IDGROUPEVALIDATIOIN { get; set; }
        public System.Guid FK_IDWORKFLOW { get; set; }
        public System.Guid FK_IDOPERATION { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int ORDRE { get; set; }
        public int FK_IDETAPE { get; set; }
        public Nullable<int> DUREE { get; set; }
        public Nullable<int> ALERTE { get; set; }
        public string CONTROLEETAPE { get; set; }
        public Nullable<int> FK_IDMENU { get; set; }
        public Nullable<int> FK_IDSTATUS { get; set; }
        public Nullable<System.DateTime> DATEDERNIEREMODIFICATION { get; set; }
        public string STATUT { get; set; }
        public string NOMOPERATION { get; set; }
        public Nullable<bool> ALLCENTRE { get; set; }
        public Nullable<int> IDCENTRE { get; set; }
        public string CODECENTRE { get; set; }
        public string LIBELLECENTRE { get; set; }
        public string LIBELLEPRODUIT { get; set; }
        public string CODESITE { get; set; }
        public string LIBELLESITE { get; set; }
        public int IDSITE { get; set; }
        public string COMBINAISON_FKETAPE_FKOPERATION { get; set; }
        public Nullable<int> FK_IDTABLETRAVAIL { get; set; }
        public string FK_IDLIGNETABLETRAVAIL { get; set; }
        public Nullable<DateTime> DATEFINTRAITEMENT { get; set; }
        public string UTILISATEURAFFECTE { get; set; }
        public bool ESTAFFECTE { get; set; }
        public string ETAPEAFFECTEE { get; set; }
        public string CODE_DEMANDE_TABLETRAVAIL { get; set; }
        public Nullable<bool> MODIFICATION { get; set; }
        public Nullable<System.Guid> FK_IDETAPECIRCUIT { get; set; }
        public bool CanBeProcessed { get; set; }
        public bool IS_TRAITEMENT_LOT { get; set; }
        public bool IsSelect { get; set; }
        public string NOMABON { get; set; }
        public string LIBELLECOMMUNE { get; set; }
        public string LIBELLEQUARTIER { get; set; }
        public string QUARTIER { get; set; }
        public string COMMUNE { get; set; }
    }


    public static class EnumStatutDemande
    {
        public static string RecupererLibelleStatutDemande(STATUSDEMANDE statut)
        {
            switch (statut)
            {
                case STATUSDEMANDE.Annulee :
                    return "Annulée";
                case STATUSDEMANDE.EnAttenteValidation :
                    return "En attente de validation";
                case STATUSDEMANDE.Initiee :
                    return "Initiée";
                case STATUSDEMANDE.Rejetee :
                    return "Rejetée";
                case STATUSDEMANDE.Suspendue :
                    return "Suspendue";
                case STATUSDEMANDE.Terminee :
                    return "Terminée (validée)";
                default: return "Initiée";
            }
        }
    }
}
