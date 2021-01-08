﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsMapperRedFac
    {
        public string LOTRI { get; set; }
        public string JET { get; set; }
        public string CENTRE { get; set; }
        public string CLIENT { get; set; }
        public string ORDRE { get; set; }
        public string FACTURE { get; set; }
        public Nullable<int> LIENRED { get; set; }
        public string REDEVANCE { get; set; }
        public string TRANCHE { get; set; }
        public string ORDRED { get; set; }
        public Nullable<int> QUANTITE { get; set; }
        public string UNITE { get; set; }
        public Nullable<decimal> BARPRIX { get; set; }
        public Nullable<decimal> TAXE { get; set; }
        public string CTAX { get; set; }
        public Nullable<System.DateTime> DAPP { get; set; }
        public string CRITERE { get; set; }
        public Nullable<int> VARIANTE { get; set; }
        public Nullable<decimal> TOTREDHT { get; set; }
        public Nullable<decimal> TOTREDTAX { get; set; }
        public Nullable<decimal> TOTREDTTC { get; set; }
        public string PARAM1 { get; set; }
        public string PARAM2 { get; set; }
        public string PARAM3 { get; set; }
        public string PARAM4 { get; set; }
        public string PARAM5 { get; set; }
        public string PARAM6 { get; set; }
        public Nullable<int> NBJOUR { get; set; }
        public Nullable<System.DateTime> DEBUTAPPLICATION { get; set; }
        public Nullable<System.DateTime> FINAPPLICATION { get; set; }
        public string LIENFAC { get; set; }
        public string TOPMAJ { get; set; }
        public string PERIODE { get; set; }
        public string PRODUIT { get; set; }
        public string FORMULE { get; set; }
        public Nullable<int> TOPANNUL { get; set; }
        public Nullable<int> BARBORNEDEBUT { get; set; }
        public Nullable<int> BARBORNEFIN { get; set; }
        public Nullable<int> QUANTITEMAXIMALE { get; set; }
        public int PK_ID { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public Nullable<int> FK_IDENTFAC { get; set; }
        public int FK_IDPRODUIT { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int FK_IDABON { get; set; }
    }
}
