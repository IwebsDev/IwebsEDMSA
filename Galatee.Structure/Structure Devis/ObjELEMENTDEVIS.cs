using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class ObjELEMENTDEVIS : CsPrint 
    {
 [DataMember] public int PK_ID { get; set; }

        [DataMember]  public string NUMDEVIS { get; set; }
        [DataMember]  public string NUMDEM { get; set; }
        [DataMember]  public int ORDRE { get; set; }
        [DataMember]  public string  NUMFOURNITURE { get; set; }
        [DataMember]  public string  CODEMATERIELDEVIS { get; set; }
    
        [DataMember]  public int? QUANTITE { get; set; }
        [DataMember]  public int? QUANTITEREMISENSTOCK { get; set; }
        [DataMember]  public int? QUANTITECONSOMMEE { get; set; }
        [DataMember]  public int? QUANTITELIVRET { get; set; }
        [DataMember]  public Nullable<int> QUANTITEALIVRET { get; set; }
        [DataMember]  public Nullable<int> FK_IDTYPEDEMANDE { get; set; }
        [DataMember]  public Nullable<int> QUANTITEPREVUE { get; set; }
        [DataMember]  public Nullable<int> QUANTITELIVREE { get; set; }

        [DataMember]  public Decimal? TAXE { get; set; }
        [DataMember]  public decimal COUT { get; set; }
        [DataMember]  public string DESIGNATION { get; set; }
        [DataMember]  public decimal PRIX { get; set; }
        [DataMember]  public decimal REMBOURSEMENT { get; set; }
        [DataMember]  public decimal MONTANT { get; set; }
        [DataMember]  public decimal MONTANTCONSOMME { get; set; }
        [DataMember]  public decimal MONTANTVALIDE { get; set; }
        [DataMember]  public Decimal? PRIX_UNITAIRE { get; set; }
        [DataMember]  public Boolean? ISSUMMARY { get; set; }
        [DataMember]  public Boolean? ISADDITIONAL { get; set; }
        [DataMember]  public Boolean? ISFORTRENCH { get; set; }
        [DataMember]  public Boolean? ISDEFAULT { get; set; }
        [DataMember]  public string UTILISE { get; set; }
        [DataMember]  public string COUTRECAP { get; set; }
        [DataMember]  public string TVARECAP { get; set; }
        [DataMember]  public string QUANTITERECAP { get; set; }
        [DataMember]  public string MontantRecap { get; set; }
        [DataMember]  public int REMISE { get; set; }
        [DataMember]  public int CONSOMME { get; set; }
        [DataMember]  public decimal COUTTOTAL { get; set; }
        [DataMember]  public DateTime? DATECREATION { get; set; }
        [DataMember]  public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]  public string USERCREATION { get; set; }
        [DataMember]  public string USERMODIFICATION { get; set; }
        [DataMember]  public decimal?  TAUXTAXE { get; set; }
        [DataMember]  public string NOM { get; set; }



        [DataMember]  public bool ISPM { get; set; }
        [DataMember]  public bool IsCOLORIE { get; set; }
        [DataMember]  public string  RUBRIQUE { get; set; }
        [DataMember]  public bool IsGENERE { get; set; }
        
        
        [DataMember]  public string CODECOPER { get; set; }
        [DataMember]  public Nullable<int> FK_IDCOPER { get; set; }
        [DataMember]  public int? FK_IDTAXE { get; set; }
        [DataMember]  public Nullable<int> FK_IDTDEM { get; set; }
        [DataMember]  public int? FK_IDFOURNITURE { get; set; }
        [DataMember]  public int? FK_IDCOUTCOPER { get; set; }
        [DataMember]  public int FK_IDDEMANDE { get; set; }
        [DataMember]  public int? FK_IDMATERIELDEVIS { get; set; }
        [DataMember]  public int? FK_IDTYPEMATERIEL { get; set; }

        


        [DataMember] public bool ISFOURNITURE { get; set; }
        [DataMember] public bool ISPOSE{ get; set; }
        [DataMember] public bool ISPRESTATION{ get; set; }
        [DataMember] public bool ISEXTENSION{ get; set; }

        [DataMember]  public decimal? COUTPOSE { get; set; }
        [DataMember]  public decimal? COUTFOURNITURE { get; set; }

        [DataMember]  public decimal? COUTUNITAIRE_POSE { get; set; }
        [DataMember]  public decimal? COUTUNITAIRE_FOURNITURE { get; set; }

        [DataMember]  public decimal? MONTANTHT { get; set; }
        [DataMember]  public decimal? MONTANTTAXE { get; set; }
        [DataMember]  public decimal? MONTANTTTC{ get; set; }

        [DataMember]  public string  CODE  { get; set; }
        [DataMember]  public string  LIBELLE  { get; set; }
        [DataMember]  public decimal COUTUNITAIRE { get; set; }
        [DataMember]  public int? FK_IDRUBRIQUEDEVIS { get; set; }
        [DataMember] public bool ISGENERE{ get; set; }
        [DataMember] public bool ISCOMPTEUR{ get; set; }

        [DataMember]  public string CLIENT { get; set; }
        [DataMember]  public string COMPTEUR { get; set; }
        [DataMember]  public string COMMUNE { get; set; }
        [DataMember]  public string QUARTIER { get; set; }
        [DataMember]  public string RUE { get; set; }
        [DataMember]  public string PORTE { get; set; }
        [DataMember]  public string TELEPHONE { get; set; }
        [DataMember]  public int? REGLAGE { get; set; }
        [DataMember]  public string LIBELLETYPEDEMANDE { get; set; }
        [DataMember]  public string ESTSUPRIMER { get; set; }

        

    }
}
