using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMapperCampagne
    {
        public string IDCOUPURE { get; set; }
        public string CENTRE { get; set; }
        public Nullable<decimal> MONTANT { get; set; }
        public string MATRICULEPIA { get; set; }
        public string PERIODE_RELANCABLE { get; set; }
        public Nullable<System.DateTime> DATE_EXIGIBILITE { get; set; }
        public string PREMIERE_TOURNEE { get; set; }
        public string DERNIERE_TOURNEE { get; set; }
        public string DEBUT_ORDTOUR { get; set; }
        public string FIN_ORDTOUR { get; set; }
        public Nullable<decimal> MONTANT_RELANCABLE { get; set; }
        public string DEBUT_CATEGORIE { get; set; }
        public string FIN_CATEGORIE { get; set; }
        public string NOMBRE_CLIENT { get; set; }
        public string NOMBRE_FACTURE { get; set; }
        public string DEBUT_AG { get; set; }
        public string FIN_AG { get; set; }
        public int PK_ID { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDMATRICULE { get; set; }
    }
}
