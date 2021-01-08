using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsEvenement : CsPrint 
    {
            [DataMember]public string CENTRE { get; set; }
            [DataMember]public string CLIENT { get; set; }
            [DataMember]public string PRODUIT { get; set; }
            [DataMember]public int POINT { get; set; }
            [DataMember]public int NUMEVENEMENT { get; set; }
            [DataMember]public string ORDRE { get; set; }
            [DataMember]public string COMPTEUR { get; set; }
            [DataMember]public Nullable<System.DateTime> DATEEVT { get; set; }
            [DataMember]public string PERIODE { get; set; }
            [DataMember]public string CODEEVT { get; set; }
            [DataMember]public Nullable<int> INDEXEVT { get; set; }
            [DataMember]public string CAS { get; set; }
            [DataMember]public string ENQUETE { get; set; }
            [DataMember]public Nullable<int> CONSO { get; set; }
            [DataMember]public Nullable<int> CONSONONFACTUREE { get; set; }
            [DataMember]public string LOTRI { get; set; }
            [DataMember]public string FACTURE { get; set; }
            [DataMember]public Nullable<int> SURFACTURATION { get; set; }
            [DataMember]public Nullable<int> STATUS { get; set; }
            [DataMember]public Nullable<int> TYPECONSO { get; set; }
            [DataMember]public string REGLAGECOMPTEUR { get; set; }
            [DataMember]public string TYPETARIF { get; set; }
            [DataMember]public string FORFAIT { get; set; }
            [DataMember]public string CATEGORIE { get; set; }
            [DataMember]public string CODECONSO { get; set; }
            [DataMember]public string PROPRIO { get; set; }
            [DataMember]public string ETATCOMPTEUR { get; set; }
            [DataMember]public string MODEPAIEMENT { get; set; }
            [DataMember]public string MATRICULE { get; set; }
            [DataMember]public string FACPER { get; set; }
            [DataMember]public Nullable<int> QTEAREG { get; set; }
            [DataMember]public string DERPERF { get; set; }
            [DataMember]public string DERPERFN { get; set; }
            [DataMember]public Nullable<int> CONSOFAC { get; set; }
            [DataMember]public Nullable<int> REGIMPUTE { get; set; }
            [DataMember]public Nullable<int> REGCONSO { get; set; }
            [DataMember]public Nullable<decimal> COEFLECT { get; set; }
            [DataMember]public Nullable<int> COEFCOMPTAGE { get; set; }
            [DataMember]public Nullable<decimal> PUISSANCE { get; set; }
            [DataMember]public string TYPECOMPTAGE { get; set; }
            [DataMember]public string TYPECOMPTEUR { get; set; }
            [DataMember]public Nullable<decimal> COEFK1 { get; set; }
            [DataMember]public Nullable<decimal> COEFK2 { get; set; }

            [DataMember]public string DEBUTEXONERATIONTVA { get; set; }
            [DataMember]public string FINEXONERATIONTVA { get; set; }
            [DataMember]public bool  ISEXONERETVA { get; set; }

            
            [DataMember]public Nullable<decimal> COEFKR1 { get; set; }
            [DataMember]public Nullable<decimal> COEFKR2 { get; set; }

            [DataMember]public Nullable<int> COEFFAC { get; set; }
            [DataMember]public string USERCREATION { get; set; }
            [DataMember]public System.DateTime DATECREATION { get; set; }
            [DataMember]public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
            [DataMember]public string USERMODIFICATION { get; set; }
            [DataMember]public int PK_ID { get; set; }
            [DataMember]public int? FK_IDCANALISATION { get; set; }
            [DataMember]public int? FK_IDABON { get; set; }
            [DataMember]public int? FK_IDCOMPTEUR { get; set; }
            [DataMember]public int FK_IDCENTRE { get; set; }
            [DataMember]public int FK_IDPRODUIT { get; set; }
            [DataMember]public Nullable<bool> ESTCONSORELEVEE { get; set; }
            [DataMember]public string COMMENTAIRE { get; set; }
            [DataMember]public Nullable<int> FK_IDTOURNEE { get; set; }
            [DataMember]public string TOURNEE { get; set; }
            [DataMember]public string ORDTOUR { get; set; }
            [DataMember]public string PERFAC { get; set; }
            [DataMember]public Nullable<int> CONSOMOYENNEPRECEDENTEFACTURE { get; set; }
            [DataMember]public Nullable<System.DateTime> DATERELEVEPRECEDENTEFACTURE { get; set; }
            [DataMember]public string CASPRECEDENTEFACTURE { get; set; }
            [DataMember]public Nullable<int> INDEXPRECEDENTEFACTURE { get; set; }
            [DataMember]public string PERIODEPRECEDENTEFACTURE { get; set; }
            [DataMember]public Nullable<byte> ORDREAFFICHAGE { get; set; }


           [DataMember] public string CASPAGERI { get; set; }
           [DataMember] public string CASPAGISOL { get; set; }
           [DataMember] public string CASEVENEMENT { get; set; }
           [DataMember] public bool? IsFromPageri { get; set; }
           [DataMember] public bool? IsFromPagisol { get; set; }
           [DataMember] public int? CONSOPRECEDENT { get; set; }
           [DataMember] public bool? IsFromEvennt { get; set; }
           [DataMember] public string NOMABON { get; set; }
           [DataMember] public string REFERENCE { get; set; }

        
            [DataMember]public bool IsSaisi { get; set; }   
            [DataMember]public int? STATUSPAGERIE { get; set; }   
            [DataMember] public bool IsFacture { get; set; }   
            // autres champs foreign key
            [DataMember]public string PERIODEPRECEDENT { get; set; }
            [DataMember]public string OLD_RELEVEUR { get; set; }
            [DataMember]public string RELEVEUR { get; set; }
            [DataMember]public string OLD_CENTRE { get; set; }
            [DataMember]public int? CONSOFACPRECEDENT { get; set; }
            [DataMember]public string OLD_IDTOURNEE { get; set; }
            [DataMember] public decimal? FACTTOT { get; set; }
            [DataMember]public int ORDRESAISIE { get; set; }
            [DataMember] public int FK_IDPAGERI { get; set; }
            [DataMember] public int FK_IDPAGISOL { get; set; }
            [DataMember] public string NOMPIA { get; set; }
            [DataMember] public string LIBELLECASPRECEDENT { get; set; }
            [DataMember] public string LIBELLEPRODUIT { get; set; }
            [DataMember] public string LIBELLERELEVEUR { get; set; }
            [DataMember] public string LIBELLETYPECOMPTEUR { get; set; }
            [DataMember] public string LIBELLEETATCOMPTEUR { get; set; }
            [DataMember] public int  NOMBRECLIENTLOT { get; set; }
            [DataMember] public string PERIODEPREC { get; set; }
            [DataMember] public string NUMDEM { get; set; }
            [DataMember] public string DERPERFNPREC { get; set; }
            [DataMember] public string DERPERFPREC { get; set; }
            [DataMember]  public Nullable<int> QTEAREGPRECEDENT { get; set; }
            [DataMember]  public Nullable<int> REGIMPUTEPREC { get; set; }
            [DataMember]  public int CONSOSAISIE { get; set; }
            [DataMember]  public Nullable<System.DateTime> DATERESIL { get; set; }

            [DataMember]  public Nullable<decimal> PERTESACTIVES { get; set; }
            [DataMember]  public Nullable<decimal> PERTESREACTIVES { get; set; }
            [DataMember]  public Nullable<int> FK_IDCAS { get; set; }
            [DataMember]  public  int FK_IDCLIENT { get; set; }
            [DataMember]  public Nullable<decimal> PUISSANCEINSTALLEE { get; set; }
            [DataMember]  public Nullable<decimal> PUISSANCEUTILISEE { get; set; }
            [DataMember]  public Nullable<System.DateTime> DRES { get; set; }
            [DataMember]  public Nullable<System.DateTime> DABONNEMENT { get; set; }
            [DataMember]  public string CATEGORIECLIENT { get; set; }
            [DataMember]  public string PROPRIETAIRE { get; set; }
            [DataMember]  public Nullable<int> FK_IDCATEGORIE { get; set; }
            [DataMember]  public Nullable<int> FK_IDRELEVEUR { get; set; }
            [DataMember]  public bool   ISCONSOSEULE { get; set; }
            [DataMember]  public string NOUVCOMPTEUR { get; set; }
            [DataMember]  public string COMPTEURAFFICHER { get; set; }
            [DataMember]  public bool  ISEVTPOSETROUVE { get; set; }
            [DataMember]  public Nullable<decimal> MONTANT { get; set; }
            [DataMember]  public byte?  NOUVEAUCADRAN { get; set; }
        
        // TSP
            [DataMember] public string RUE { get; set; }
            [DataMember] public string PORTE { get; set; }
            [DataMember] public Nullable<byte> CADRAN { get; set; }
            [DataMember] public Nullable<int> FK_IDSTATUTTRANSFERT { get; set; }
            [DataMember] public string CASTRANSFERT{ get; set; }
            [DataMember] public string NOUVEAUCOMPTEUR { get; set; }
            [DataMember] public string ROUE { get; set; }
            [DataMember] public string LOCALISATION { get; set; }
            [DataMember] public int   INDEXEVTPRECEDENT { get; set; }
            [DataMember] public int IDCANALISATION { get; set; }
            [DataMember] public string CASPRECEDENT { get; set; }
            [DataMember] public int CONSOMOYENNE { get; set; }

        // TSP

            [DataMember] public string DENABON { get; set; }
            [DataMember] public string DENMAND { get; set; }
            [DataMember] public string ADRMAND1 { get; set; }
            [DataMember] public string ADRMAND2 { get; set; }
            [DataMember] public string CPOS { get; set; }
            [DataMember] public string BUREAU { get; set; }
            [DataMember] public string BANQUE { get; set; }
            [DataMember] public string GUICHET { get; set; }
            [DataMember] public string COMPTE { get; set; }
            [DataMember] public string REGROUPEMENT { get; set; }
            [DataMember] public string REGEDIT { get; set; }
            [DataMember] public string COMMUNE { get; set; }
            [DataMember] public string QUARTIER { get; set; }
            [DataMember] public string ETAGE { get; set; }
            [DataMember] public int? NBFAC { get; set; }
            [DataMember] public int? NBREJOUR { get; set; }

            [DataMember] public bool  ISFACTUREEMAIL { get; set; }
            [DataMember] public bool  ISFACTURESMS { get; set; }
            [DataMember] public string TELEPHONE { get; set; }
            [DataMember] public string EMAIL { get; set; }
            [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
            [DataMember] public bool  ISEVENEMENTNONFACURE { get; set; }
            [DataMember] public bool ISAJOUTLOT { get; set; }
            [DataMember] public string LIBELLEREGLAGECOMPTEUR { get; set; }
            [DataMember] public string LIBELLEMARQUE { get; set; }
            [DataMember] public string LOTASUPPRIMER { get; set; }
            [DataMember]public Nullable<int> FK_IDDEMANDE { get; set; }
            [DataMember] public int? IDETAPE { get; set; }

        
    }

}









