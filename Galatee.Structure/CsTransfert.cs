using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Galatee.Structure
{
    [DataContract]
    public class CsTransfert:CsPrint
    {
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        [DataMember]
        public int POINT { get; set; }
        [DataMember]
        public int NUMEVENEMENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string COMPTEUR { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEEVT { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public string CODEEVT { get; set; }
        [DataMember]
        public Nullable<int> INDEXEVT { get; set; }
        [DataMember]
        public string CAS { get; set; }
        [DataMember]
        public string ENQUETE { get; set; }
        [DataMember]
        public Nullable<int> CONSO { get; set; }
        [DataMember]
        public string LOTRI { get; set; }
        [DataMember]
        public string FACTURE { get; set; }
        [DataMember]
        public Nullable<int> STATUS { get; set; }
        [DataMember]
        public string REGLAGECOMPTEUR { get; set; }
        [DataMember]
        public string TYPETARIF { get; set; }
        [DataMember]
        public string FORFAIT { get; set; }
        [DataMember]
        public string CATEGORIE { get; set; }
        [DataMember]
        public string CODECONSO { get; set; }
        [DataMember]
        public string STATUTCOMPTEUR { get; set; }
        [DataMember]
        public string MATRICULE { get; set; }
        [DataMember]
        public Nullable<int> QTEAREG { get; set; }
        [DataMember]
        public Nullable<int> CONSOFAC { get; set; }
        [DataMember]
        public Nullable<decimal> COEFLECT { get; set; }
        [DataMember]
        public Nullable<int> COEFCOMPTAGE { get; set; }
        [DataMember]
        public Nullable<decimal> PUISSANCE { get; set; }
        [DataMember]
        public string TYPECOMPTAGE { get; set; }
        [DataMember]
        public string TYPECOMPTEUR { get; set; }
        [DataMember]
        public Nullable<decimal> COEFK1 { get; set; }
        [DataMember]
        public Nullable<decimal> COEFK2 { get; set; }
        [DataMember]
        public Nullable<int> COEFFAC { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public System.DateTime DATECREATION { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
        [DataMember]
        public int PK_ID { get; set; }
        [DataMember]
        public int FK_IDCENTRE { get; set; }
        [DataMember]
        public int FK_IDPRODUIT { get; set; }
        [DataMember]
        public int FK_IDSTATUTTRANSFERT { get; set; }
        [DataMember]
        public Nullable<bool> ESTCONSORELEVEE { get; set; }
        [DataMember]
        public string COMMENTAIRE { get; set; }
        [DataMember]
        public string NOUVEAUCOMPTEUR { get; set; }
        [DataMember]
        public string NOUVELLEMARQUE { get; set; }
        [DataMember]
        public string NOUVEAUPOINT { get; set; }
    }
}
