using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure
{
    [DataContract]
    public class ObjTRAVAUXDEVIS
    {
        [DataMember] public String NUMDEM { get; set; }
        [DataMember] public String NUMDEVIS { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDDEVIS { get; set; }
        [DataMember] public int? FK_IDPRESTATAIRE { get; set; }
        [DataMember] public int? FK_IDTYPEDEPANNE { get; set; }
        [DataMember] public int? FK_IDVEHICULE { get; set; }
        [DataMember] public int ORDRE { get; set; }
        [DataMember] public String MATRICULECHEFEQUIPE { get; set; }
        [DataMember] public String NOMCHEFEQUIPE { get; set; }
        [DataMember] public String PROCESVERBAL { get; set; }
        [DataMember] public decimal? MONTANTREGLE { get; set; }
        [DataMember] public decimal? MONTANTREMBOURSEMENT { get; set; }
        [DataMember] public DateTime DATEPREVISIONNELLE { get; set; }
        [DataMember] public DateTime? DATEDEBUTTRVX { get; set; }
        [DataMember] public DateTime? DATEFINTRVX { get; set; }
        [DataMember] public String MATRICULEREGLEMENT { get; set; }
        [DataMember] public DateTime? DATEREGLEMENT { get; set; }
        [DataMember] public Boolean ISUSEDFORBILAN { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }	
        [DataMember] public string NBRCABLESECTION { get; set; }	
    }
}
