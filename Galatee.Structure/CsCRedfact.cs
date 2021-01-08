using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCRedfact : CsPrint
    {
        [DataMember]
        public string LOTRI { get; set; }
        [DataMember]
        public string JET { get; set; }
        [DataMember]
        public string DR { get; set; }
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string DENABON { get; set; }
        [DataMember]
        public string NOMABON { get; set; }
        [DataMember]
        public string DENMAND { get; set; }
        [DataMember]
        public string NOMMAND { get; set; }
        [DataMember]
        public string ADRMAND1 { get; set; }
        [DataMember]
        public string ADRMAND2 { get; set; }
        [DataMember]
        public string CPOS { get; set; }
        [DataMember]
        public string BUREAU { get; set; }
        [DataMember]
        public string BANQUE { get; set; }
        [DataMember]
        public string GUICHET { get; set; }
        [DataMember]
        public string COMPTE { get; set; }
        [DataMember]
        public string RIB { get; set; }
        [DataMember]
        public string CODCONSO { get; set; }
        [DataMember]
        public string CATCLI { get; set; }
        [DataMember]
        public string REGCLI { get; set; }
        [DataMember]
        public string REGEDIT { get; set; }
        [DataMember]
        public string AG { get; set; }
        [DataMember]
        public string COMMUNE { get; set; }
        [DataMember]
        public string QUARTIER { get; set; }
        [DataMember]
        public string RUE { get; set; }
        [DataMember]
        public string NOMRUE { get; set; }
        [DataMember]
        public string NUMRUE { get; set; }
        [DataMember]
        public string COMPRUE { get; set; }
        [DataMember]
        public string ETAGE { get; set; }
        [DataMember]
        public string PORTE { get; set; }
        [DataMember]
        public string CADR { get; set; }
        [DataMember]
        public string TOURNEE { get; set; }
        [DataMember]
        public string ORDTOUR { get; set; }
        [DataMember]
        public string NBFAC { get; set; }
        [DataMember]
        public string FACTURE { get; set; }
        [DataMember]
        public string DFAC { get; set; }
        [DataMember]
        public string MES { get; set; }
        [DataMember]
        public decimal  TOTFHT { get; set; }
        [DataMember]
        public decimal  TOTFTAX { get; set; }
        [DataMember]
        public decimal  TOTFTTC { get; set; }
        [DataMember]
        public string NATURE { get; set; }
        [DataMember]
        public string TOPE { get; set; }
        [DataMember]
        public string PERIODE { get; set; }
        [DataMember]
        public int   EXIG { get; set; }
        [DataMember]
        public decimal TOTAJU { get; set; }
        [DataMember]
        public int     DELAJU { get; set; }
        [DataMember]
        public string     COPER { get; set; }
        [DataMember]
        public string     MODEP { get; set; }
        [DataMember]
        public string     ANCIENREPORT { get; set; }
        [DataMember]
        public string     TOTALNONARRONDI { get; set; }
        [DataMember]
        public string     LIENFAC { get; set; }
        [DataMember]
        public string     TOPMAJ { get; set; }
        [DataMember]
        public string     NATURECLIENT { get; set; }
        [DataMember]
        public string     SECTEUR { get; set; }
        [DataMember]
        public string     DRESABON { get; set; }
        [DataMember]
        public string     REFERENCEATM { get; set; }
        [DataMember]
        public string     CODEPROFIL { get; set; }
    }
}
