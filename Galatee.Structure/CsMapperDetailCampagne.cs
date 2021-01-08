using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMapperDetailCampagne
    {
        public string IDCOUPURE { get; set; }
        public int PK_ID { get; set; }
        public string CENTRE { get; set; }
        public string CLIENT { get; set; }
        public string ORDRE { get; set; }
        public string REFEM { get; set; }
        public string NDOC { get; set; }
        public string COPER { get; set; }
        public Nullable<decimal> MONTANT { get; set; }
        public Nullable<System.DateTime> EXIGIBILITE { get; set; }
        public string TOURNEE { get; set; }
        public string ORDTOUR { get; set; }
        public string CATEGORIECLIENT { get; set; }
        public Nullable<decimal> SOLDEDUE { get; set; }
        public Nullable<int> NOMBREFACTURE { get; set; }
        public Nullable<decimal> SOLDECLIENT { get; set; }
        public Nullable<decimal> SOLDEFACTURE { get; set; }
        public string COMPTEUR { get; set; }
        public Nullable<bool> ISAUTORISER { get; set; }
        public string MOTIFAUTORISATION { get; set; }
        public Nullable<decimal> FRAIS { get; set; }
        public Nullable<bool> ISANNULATIONFRAIS { get; set; }
        public string MOTIFANNULATION { get; set; }
        public Nullable<System.DateTime> DATERDV { get; set; }
        public string USERCREATION { get; set; }
        public Nullable<System.DateTime> DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int FK_IDLCLIENT { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDCLIENT { get; set; }
        public int FK_IDTOURNEE { get; set; }
        public int FK_IDCATEGORIECLIENT { get; set; }
        public int FK_IDCAMPAGNE { get; set; }
        public Nullable<int> RELANCE { get; set; }
        public Nullable<int> FK_IDTYPECOUPURE { get; set; }
    }
}
