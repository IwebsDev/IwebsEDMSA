using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class ObjDEVIS : CsPrint
    {
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public string CODETAXE { get; set; }
        [DataMember]
        public int FK_IDTACHEDEVIS_CURRENT{ get; set; }

        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public int FK_IDSITE { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }

        [DataMember]
        public string NUMDEVIS { get; set; }

        [DataMember]
        public string CODECENTRE { get; set; }

        [DataMember]
        public string CODESITE { get; set; }

        [DataMember]
        public string CODEPRODUIT { get; set; }

        [DataMember]
        public int? FK_IDTYPEDEVIS { get; set; }

        [DataMember]
        public DateTime? DATEDECREATION { get; set; }

        [DataMember]
        public DateTime? DATEETAPE { get; set; }

        [DataMember]
        public int? FK_IDETAPEDEVIS { get; set; }

        [DataMember]
        public decimal? MONTANTHT { get; set; }

        [DataMember]
        public decimal? MONTANTTTC { get; set; }

        [DataMember]
        public decimal? MONTANTTOUTORDRE { get; set; }

        [DataMember]
        public string NUMEROCTR { get; set; }

        [DataMember]
        public string MOTIFREJET { get; set; }

        [DataMember]
        public DateTime? DATEREGLEMENT { get; set; }

        [DataMember]
        public string MATRICULECAISSE { get; set; }

        [DataMember]
        public int? ORDRE { get; set; }

        [DataMember]
        public System.Guid? IDSCHEMA { get; set; }

        [DataMember]
        public bool? ISFOURNITURE { get; set; }

        [DataMember]
        public bool? ISPOSE { get; set; }

        [DataMember]
        public bool? ISANALYSED { get; set; }

        [DataMember]
        public decimal? PUISSANCESOUSCRITE { get; set; }

        [DataMember]
        public string LIBELLETYPECOMPTEUR { get; set; }

        [DataMember]
        public string IDTYPECTR { get; set; }

        [DataMember]
        public string IDMARQUECTR { get; set; }

        [DataMember]
        public string LIBELLEMARQUECOMPTEUR { get; set; }

        [DataMember]
        public System.Guid? IDOWNERSHIP { get; set; }

        [DataMember]
        public DateTime? DATEFABRICATIONCTR { get; set; }

        [DataMember]
        public DateTime? DATEPOSECTR { get; set; }

        [DataMember]
        public int? INDEXPOSECTR { get; set; }

        [DataMember]
        public decimal? DISTANCE { get; set; }

        [DataMember]
        public string OWNERSHIPPROOFID { get; set; }

        [DataMember]
        public int? FK_IDPIECEIDENTITE { get; set; }

        [DataMember]
        public string NUMEROPIECEIDENTITE { get; set; }

        [DataMember]
        public string NUMEROGPS { get; set; }

        [DataMember]
        public string NEARESTROUTE { get; set; }

        [DataMember]
        public bool ISBRACKET { get; set; }

        [DataMember]
        public bool ISSERVICEPOLE { get; set; }

        [DataMember]
        public bool ESTSIMPLIFIE { get; set; }

        [DataMember]
        public bool ESTCOMPLET { get; set; }

        [DataMember]
        public bool ISSUBVENTION { get; set; }

        [DataMember]
        public bool ISEXTENSION { get; set; }

        [DataMember]
        public Guid? IDMANUSCRIT { get; set; }

        [DataMember]
        public string EMPLACEMENTCOMPTEUR { get; set; }

        [DataMember]
        public decimal? MONTANTCOMPLEMENTAIRE { get; set; }

        [DataMember]
        public decimal? MONTANTPARTICIPATION { get; set; }

        [DataMember]
        public decimal? FRAISPOSE { get; set; }

        [DataMember]
        public bool ISNEW { get; set; }

        [DataMember]
        public string LIBELLECENTRE { get; set; }

        [DataMember]
        public string LIBELLETYPEDEVIS { get; set; }

        [DataMember]
        public string LIBELLEPRODUIT { get; set; }

        [DataMember]
        public string LIBELLETACHE { get; set; }

        [DataMember]
        public string LIBELLEFONCTION { get; set; }

        [DataMember]
        public double DELAI { get; set; }

        [DataMember]
        public string ORIGINE { get; set; }

        [DataMember]
        public DateTime? DATELIMITE { get; set; }

        [DataMember]
        public string LIBELLESITE { get; set; }

        [DataMember]
        public string LIBELLETYPEPIECE { get; set; }

        [DataMember]
        public string MATRICULEPIA { get; set; }

        [DataMember]
        public int? MUMETAPE { get; set; }
        [DataMember]
        public int? ISPARTICIPATION { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public int NUMETAPE { get; set; }
        [DataMember]
        public int? DELAIEXECUTIONETAPE { get; set; }
        [DataMember]
        public string NOM { get; set; }
        [DataMember]
        public DateTime? DCAISSE { get; set; }
        [DataMember]
        public string DEMANDE { get; set; }
        [DataMember]
        public string NUMEROCLIENT { get; set; }

    }
}
