using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMapperTransfert  
    {
        public string CENTRE { get; set; }
        public string CLIENT { get; set; }
        public string PRODUIT { get; set; }
        public int POINT { get; set; }
        public int NUMEVENEMENT { get; set; }
        public string ORDRE { get; set; }
        public string COMPTEUR { get; set; }
        public Nullable<System.DateTime> DATEEVT { get; set; }
        public string PERIODE { get; set; }
        public string CODEEVT { get; set; }
        public Nullable<int> INDEXEVT { get; set; }
        public string CAS { get; set; }
        public string ENQUETE { get; set; }
        public Nullable<int> CONSO { get; set; }
        public string LOTRI { get; set; }
        public string FACTURE { get; set; }
        public Nullable<int> STATUS { get; set; }
        public string REGLAGECOMPTEUR { get; set; }
        public string TYPETARIF { get; set; }
        public string FORFAIT { get; set; }
        public string CATEGORIE { get; set; }
        public string CODECONSO { get; set; }
        public string STATUTCOMPTEUR { get; set; }
        public string MATRICULE { get; set; }
        public Nullable<int> QTEAREG { get; set; }
        public Nullable<int> CONSOFAC { get; set; }
        public Nullable<decimal> COEFLECT { get; set; }
        public Nullable<int> COEFCOMPTAGE { get; set; }
        public Nullable<decimal> PUISSANCE { get; set; }
        public string TYPECOMPTAGE { get; set; }
        public string TYPECOMPTEUR { get; set; }
        public Nullable<decimal> COEFK1 { get; set; }
        public Nullable<decimal> COEFK2 { get; set; }
        public Nullable<int> COEFFAC { get; set; }
        public string USERCREATION { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDPRODUIT { get; set; }
        public int FK_IDSTATUTTRANSFERT { get; set; }
        public Nullable<bool> ESTCONSORELEVEE { get; set; }
        public string COMMENTAIRE { get; set; }
        public string NOUVEAUCOMPTEUR { get; set; }
        public string NOUVELLEMARQUE { get; set; }
        public string NOUVEAUPOINT { get; set; }
    }
}
