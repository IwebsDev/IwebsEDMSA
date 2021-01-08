using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsAbon
    {

        [DataMember] public bool ISAUGMENTATIONPUISSANCE { get; set; }
        [DataMember] public bool ISDIMINUTIONPUISSANCE { get; set; }
        [DataMember] public Nullable<decimal> NOUVELLEPUISSANCE { get; set; }
        [DataMember] public string NATURECLIENT { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string NUMDEM { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string TYPETARIF { get; set; }
        [DataMember] public Nullable<decimal> PUISSANCE { get; set; }
        [DataMember] public string FORFAIT { get; set; }
        [DataMember] public string FORFPERSO { get; set; }
        [DataMember] public Nullable<decimal> AVANCE { get; set; }
        [DataMember] public Nullable<System.DateTime> DAVANCE { get; set; }
        [DataMember] public string REGROU { get; set; }
        [DataMember] public string PERFAC { get; set; }
        [DataMember] public string MOISFAC { get; set; }
        [DataMember] public Nullable<System.DateTime> DABONNEMENT { get; set; }
        [DataMember] public Nullable<System.DateTime> DRES { get; set; }
        [DataMember] public Nullable<System.DateTime> DATERACBRT { get; set; }
        [DataMember] public Nullable<int> NBFAC { get; set; }
        [DataMember] public string PERREL { get; set; }
        [DataMember] public string MOISREL { get; set; }
        [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
        [DataMember] public string RECU { get; set; }
        [DataMember] public Nullable<decimal> RISTOURNE { get; set; }
        [DataMember] public Nullable<int> CONSOMMATIONMAXI { get; set; }
        [DataMember] public Nullable<decimal> PUISSANCEUTILISEE { get; set; }
        [DataMember] public Nullable<int> COEFFAC { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDCLIENT { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public Nullable<int> FK_IDFORFAIT { get; set; }
        [DataMember] public int FK_IDMOISREL { get; set; }
        [DataMember] public int FK_IDMOISFAC { get; set; }
        [DataMember] public int FK_IDTYPETARIF { get; set; }
        [DataMember] public int FK_IDPERIODICITEFACTURE { get; set; }
        [DataMember] public int FK_IDPERIODICITERELEVE { get; set; }
        [DataMember] public int FK_IDDEMANDE { get; set; }
        [DataMember] public int FK_IDCATEGORIE { get; set; }
        [DataMember] public int? FK_IDTYPECOMPTAGE { get; set; }
        [DataMember] public string CATEGORIE { get; set; }
        [DataMember] public Nullable<bool> ESTEXONERETVA { get; set; }
        [DataMember] public string  DEBUTEXONERATIONTVA { get; set; }
        [DataMember] public string  FINEXONERATIONTVA { get; set; }
        [DataMember] public string  TYPECOMPTAGE { get; set; }
        [DataMember] public Nullable<bool> ISBORNEPOSTE { get; set; }
        
        /* Autre */
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLETARIF { get; set; }
        [DataMember] public string LIBELLEFORFAIT { get; set; }
        [DataMember] public string LIBELLEMOISFACT { get; set; }
        [DataMember] public string LIBELLEMOISIND { get; set; }
        [DataMember] public string LIBELLEFREQUENCE { get; set; }
        [DataMember] public string LIBELLETYPECOMPTAGE { get; set; }
        
        [DataMember] public string LIBELLEAPPLICATIONTAXE { get; set; }
        [DataMember] public int FK_IDCATEGORIECLIENT { get; set; }
        [DataMember] public string CATEGORIECLIENT { get; set; }
        [DataMember] public string NOMABON { get; set; }
        [DataMember] public int? NOMBREDEFOYER { get; set; }
        [DataMember]  public int FK_IDTOURNEE { get; set; }

        [DataMember] public bool ISMODIFIER { get; set; }

      
    }
}









