using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDirecteur
    {
        public string CENTRE { get; set; }
        public string LIBELLE { get; set; }
        public string DR { get; set; }
        public string SITE { get; set; }
        public string ADR1 { get; set; }
        public string ADR2 { get; set; }
        public string CPOS { get; set; }
        public string BUREAU { get; set; }
        public string TELRENS { get; set; }
        public string TELDEP { get; set; }
        public string PRODUIT1 { get; set; }
        public string PRODUIT2 { get; set; }
        public string PRODUIT3 { get; set; }
        public string PRODUIT4 { get; set; }
        public string PRODUIT5 { get; set; }
        public string PRODUIT6 { get; set; }
        public string PRODUIT7 { get; set; }
        public string PRODUIT8 { get; set; }
        public string PRODUIT9 { get; set; }
        public string PRODUIT10 { get; set; }
        public string PRODOPTION1 { get; set; }
        public string PRODOPTION2 { get; set; }
        public string PRODOPTION3 { get; set; }
        public string PRODOPTION4 { get; set; }
        public string PRODOPTION5 { get; set; }
        public string PRODOPTION6 { get; set; }
        public string PRODOPTION7 { get; set; }
        public string PRODOPTION8 { get; set; }
        public string PRODOPTION9 { get; set; }
        public string PRODOPTION10 { get; set; }
        public string NUMID { get; set; }
        public string NUMAUT { get; set; }
        public string AVAUTO { get; set; }
        public string FRAISAUTO { get; set; }
        public string FACTURE { get; set; }
        public Nullable<System.DateTime> DMAJ { get; set; }
        public string TRANS { get; set; }
        public string USERCREATION { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int PK_ID { get; set; }
        public int FK_IDSITE { get; set; }
        public int FK_IDCENTRE { get; set; }


    }
}
