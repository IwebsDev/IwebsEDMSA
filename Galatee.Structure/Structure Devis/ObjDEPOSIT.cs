using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure

{
    [DataContract]
    public class ObjDEPOSIT
    {

        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public String CENTRE { get; set; }

        [DataMember]
        public String CLIENT { get; set; }

        [DataMember]
        public String ORDRE { get; set; }

        [DataMember]
        public Decimal? DEPOSIT { get; set; }

        [DataMember]
        public String RECEIPT { get; set; }

        [DataMember]
        public DateTime DATEENC { get; set; }

        [DataMember]
        public String NUMDEVIS { get; set; }

        [DataMember]
        public String NOM { get; set; }

        [DataMember]
        public Decimal? TOTAL { get; set; }

        [DataMember]
        public String IDENTITE { get; set; }

        [DataMember]
        public String TOPANNUL { get; set; }

        [DataMember]
        public Decimal? MONTANTTVA { get; set; }

        [DataMember]
        public String BANQUE { get; set; }

        [DataMember]
        public String COMPTE { get; set; }

        [DataMember]
        public Guid? IDLETTER { get; set; }

        [DataMember]
        public Boolean? ISCREATED { get; set; }

        [DataMember]
        public string DateEncaissementAffichee { get; set; }

        [DataMember]
        public Decimal RELIQUAT { get; set; }

        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        
    }

}
