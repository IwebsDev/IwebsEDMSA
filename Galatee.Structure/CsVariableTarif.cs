using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsVariableTarif
    {
        [DataMember]
        public string REDEVANCE { get; set; }
        [DataMember]
        public string REGION { get; set; }
        [DataMember]
        public string SREGION { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string COMMUNE { get; set; }
        [DataMember]
        public Nullable<byte> ORDREEDITION { get; set; }
        [DataMember]
        public System.DateTime DATEAPPLICATION { get; set; }
        [DataMember]
        public string RECHERCHETARIF { get; set; }
        [DataMember]
        public string MODECALCUL { get; set; }
        [DataMember]
        public string MODEAPPLICATION { get; set; }
        [DataMember]
        public string LIBELLECOMPTABLE { get; set; }
        [DataMember]
        public string COMPTECOMPTABLE { get; set; }
        [DataMember]
        public Nullable<bool> ESTANALYTIQUE { get; set; }
        [DataMember]
        public Nullable<bool> GENERATIONANOMALIE { get; set; }
        [DataMember]
        public string FORMULE { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public int FK_IDREDEVANCE { get; set; }
        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public int FK_IDMODEAPPLICATION { get; set; }
        [DataMember]
        public int FK_IDMODECALCUL { get; set; }
        [DataMember]
        public int FK_IDRECHERCHETARIF { get; set; }
        [DataMember]
        public virtual CsCentre CENTRE1 { get; set; }
        [DataMember]
        public virtual CsModeApplicationTarif MODEAPPLICATIONTARIF { get; set; }
        [DataMember]
        public virtual CsModeCalcul MODECALCUL1 { get; set; }
        [DataMember]
        public virtual CsRechercheTarif RECHERCHETARIF1 { get; set; }
        [DataMember]
        public virtual CsRedevance REDEVANCE1 { get; set; }
    }
}
