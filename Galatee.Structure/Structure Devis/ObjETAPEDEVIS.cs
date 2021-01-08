using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com
namespace Galatee.Structure
{
    [DataContract]
    public class ObjETAPEDEVIS : CsPrint
    {
        [DataMember]
        public Int32 PK_ID { get; set; }

        [DataMember]
        public string PRODUIT { get; set; }

        [DataMember]
        public Int32 FK_IDTYPEDEVIS { get; set; }

        [DataMember]
        public int FK_IDPRODUIT { get; set; }

        [DataMember]
        public Int32 NUMETAPE { get; set; }

        [DataMember]
        public Int32 FK_IDTACHEDEVIS { get; set; }

        [DataMember]
        public Int32 IDTACHESUIVANTE { get; set; }

        [DataMember]
        public Int32? IDTACHEINTERMEDIAIRE { get; set; }

        [DataMember]
        public Int32? IDTACHEREJET { get; set; }

        [DataMember]
        public Int32? IDTACHESAUT { get; set; }

        [DataMember]
        public String CodeFonction { get; set; }

        [DataMember]
        public int? DELAIEXECUTIONETAPE { get; set; }

        [DataMember]
        public Int32 MENUID { get; set; }

        [DataMember]
        public string LIBELLETACHE { get; set; }

        [DataMember]
        public string LIBELLETYPEDEVIS { get; set; }

        [DataMember]
        public string LIBELLEPRODUIT { get; set; }

        [DataMember]
        public string LIBELLETACHESUIVANTE { get; set; }

        [DataMember]
        public string LIBELLETACHEREJET { get; set; }

        [DataMember]
        public string LIBELLETACHESAUT { get; set; }

        [DataMember]
        public string LIBELLETACHEINTERMEDIARE { get; set; }
        
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
