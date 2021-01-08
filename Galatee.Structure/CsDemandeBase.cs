using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDemandeBase : CsPrint
    {

      [DataMember]  public string NUMDEM { get; set; }
      [DataMember]  public string CENTRE { get; set; }
      [DataMember]  public string NUMPERE { get; set; }
      [DataMember]  public string TYPEDEMANDE { get; set; }
      [DataMember]  public Nullable<System.DateTime> DPRRDV { get; set; }
      [DataMember]  public Nullable<System.DateTime> DPRDEV { get; set; }
      [DataMember]  public Nullable<System.DateTime> DPREX { get; set; }
      [DataMember]  public Nullable<System.DateTime> DREARDV { get; set; }
      [DataMember]  public Nullable<System.DateTime> DREADEV { get; set; }
      [DataMember]  public Nullable<System.DateTime> DREAEX { get; set; }
      [DataMember]  public string HRDVPR { get; set; }
      [DataMember]  public string FDEM { get; set; }
      [DataMember]  public string FREP { get; set; }
      [DataMember]  public string NOMPERE { get; set; }
      [DataMember]  public string NOMMERE { get; set; }
      [DataMember]  public string MATRICULE { get; set; }
      [DataMember]  public string STATUT { get; set; }
      [DataMember]  public Nullable<System.DateTime> DCAISSE { get; set; }
      [DataMember]  public string NCAISSE { get; set; }
      [DataMember]  public string EXDAG { get; set; }
      [DataMember]  public string EXDBRT { get; set; }
      [DataMember]  public string PRODUIT { get; set; }
      [DataMember]  public string EXCL { get; set; }
      [DataMember]  public string CLIENT { get; set; }
      [DataMember]  public string EXCOMPT { get; set; }
      [DataMember]  public string COMPTEUR { get; set; }
      [DataMember]  public string EXEVT { get; set; }
      [DataMember]  public string CTAXEG { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATED { get; set; }
      [DataMember]  public string REFEM { get; set; }
      [DataMember]  public string ORDRE { get; set; }
      [DataMember]  public string TOPEDIT { get; set; }
      [DataMember]  public string FACTURE { get; set; }
      [DataMember]  public string OPERATIONDIVERSE { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEFLAG { get; set; }
      [DataMember]  public string USERCREATION { get; set; }
      [DataMember]  public System.DateTime DATECREATION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
      [DataMember]  public string USERMODIFICATION { get; set; }
      [DataMember]  public Nullable<int> ETAPEDEMANDE { get; set; }
      [DataMember]  public int PK_ID { get; set; }
      [DataMember]  public int FK_IDCENTRE { get; set; }
      [DataMember]  public Nullable<int> FK_IDCLIENT { get; set; }
      [DataMember]  public Nullable<int> FK_IDADMUTILISATEUR { get; set; }
      [DataMember]  public int FK_IDTYPEDEMANDE { get; set; }
      [DataMember]  public Nullable<int> FK_IDPRODUIT { get; set; }
      [DataMember]  public Nullable<bool> TRANSMIS { get; set; }
      [DataMember]  public string STATUTDEMANDE { get; set; }
      [DataMember]  public string ANNOTATION { get; set; }
      [DataMember]  public Nullable<System.Guid> FICHIERJOINT { get; set; }
      [DataMember]  public Nullable<bool> ISSUPPRIME { get; set; }
      [DataMember]  public string USERSUPPRESSION { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATESUPPRESSION { get; set; }
      [DataMember]  public  bool ISFOURNITURE { get; set; }
      [DataMember]  public  bool ISPOSE { get; set; }
      [DataMember]  public  bool ISEXTENSION { get; set; }
      [DataMember]  public  bool ISCONTROLE { get; set; }
      [DataMember]  public  bool ISPRESTATION { get; set; }
      [DataMember]  public  bool ISMUTATION { get; set; }
      [DataMember]  public  bool ISETALONNAGE { get; set; }
      [DataMember]  public  bool ISMETREAFAIRE { get; set; }
      [DataMember]  public  bool ISDEMANDEREJETERINIT { get; set; }
      [DataMember]  public  bool ISDEVISCOMPLEMENTAIRE { get; set; }
      [DataMember]  public  bool ISBONNEINITIATIVE { get; set; }
      [DataMember]  public  bool ISEDM { get; set; }
      [DataMember]  public  bool ISCOMMUNE { get; set; }
      [DataMember]  public  int  NOMBREDEFOYER { get; set; }
      [DataMember]  public  bool ISPASSERCAISSE { get; set; }
      [DataMember]  public  bool ISDEVISHT { get; set; }
      [DataMember]  public  bool ISPASDEFACTURE { get; set; }
      [DataMember]  public  decimal ? ANCIENNEPUISSANCE { get; set; }
        
      [DataMember]  public  string  TYPECOMPTAGE { get; set; }
      [DataMember]  public  int ? FK_IDTYPECOMPTAGE { get; set; }
      [DataMember]  public  int ? FK_IDPUISSANCESOUSCRITE { get; set; }
      [DataMember]  public  int ? FK_IDREGLAGECOMPTEUR { get; set; }

      [DataMember]  public Nullable<bool> ISDEFINITIF { get; set; }
      [DataMember]  public Nullable<bool> ISPROVISOIR { get; set; }
      [DataMember]  public Nullable<int> FK_IDDEMANDE { get; set; }

      [DataMember]  public int? CODEREGLAGECOMPTEUR { get; set; }
      [DataMember]  public string REGLAGECOMPTEUR { get; set; }
      [DataMember]  public Nullable<decimal>  PUISSANCESOUSCRITE { get; set; }
      [DataMember]  public Nullable<System.DateTime> DATEFIN{ get; set; }
      [DataMember]  public string MOTIF { get; set; }
      [DataMember]  public Nullable<int> FK_IDTYPECLIENT { get; set; }

      [DataMember] public string LIBELLE { get; set; }
      [DataMember] public string NOMCLIENT { get; set; }
      [DataMember] public string ADRESSE1CLIENT { get; set; }
      [DataMember] public string ADRESSE2CLIENT { get; set; }
      [DataMember] public string LIBELLETDEM { get; set; }
      [DataMember] public string LIBELLESTATUT { get; set; }
      [DataMember] public string LIBELLEETAPEDEMANDE { get; set; }

      [DataMember] public bool IsSELECT { get; set; }
      [DataMember] public bool ISNEW { get; set; }
      [DataMember] public bool ISCHANGECOMPTEUR { get; set; }

      [DataMember] public string LIBELLECENTRE { get; set; }
      [DataMember] public string LIBELLETYPEDEMANDE { get; set; }
      [DataMember] public string LIBELLEPRODUIT { get; set; }
      [DataMember] public string SITE { get; set; }
      [DataMember] public string LIBELLESITE { get; set; }
      [DataMember] public string LIBELLETACHE { get; set; }
      [DataMember] public string INITIERPAR { get; set; }
      [DataMember] public string LIBELLECOMMUNE { get; set; }
      [DataMember] public string LIBELLEQUARTIER { get; set; }
      [DataMember] public string LIBELLERUES { get; set; }

      [DataMember] public string LIBELLEETAPEENCOURS { get; set; }
      [DataMember] public string LIBELLEETAPESUIVANTE { get; set; }
      [DataMember] public int FK_IDETAPEENCOURE{ get; set; }
      [DataMember] public int FK_IDETAPESUIVANTE{ get; set; }
      [DataMember] public int FK_IDMAGAZINVIRTUEL{ get; set; }
      [DataMember] public string DIAMBRT { get; set; }
      // REPORT
      [DataMember] public string CATEGORIE { get; set; }
      [DataMember] public string TOURNEE { get; set; }
      [DataMember] public string CODECONSO { get; set; }
      [DataMember] public string CODEELECTRIQUE { get; set; }
      [DataMember] public string LONGITUDE { get; set; }
      [DataMember] public string LATITUDE { get; set; }
      [DataMember] public string COMMUNE { get; set; }
      [DataMember] public string QUARTIER { get; set; }
      [DataMember] public string RUE { get; set; }
      [DataMember] public string PORTE { get; set; }

      [DataMember] public bool ISSUPPRIMERCOUT { get; set; }


      [DataMember] public string PRESTATAIRE { get; set; }


    }
}









