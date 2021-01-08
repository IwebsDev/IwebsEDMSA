using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsComptabilisation:CsPrint
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string ORIGINE { get; set; }
        [DataMember] public string COPER { get; set; }
        [DataMember] public string COPERINITAL { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public string LIBCOURT { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string COMPTGENE { get; set; }
        [DataMember] public string DC { get; set; }
        [DataMember] public string CTRAIT { get; set; }
        [DataMember] public string CAISSE { get; set; }
        [DataMember] public System.DateTime? DMAJ { get; set; }
        [DataMember] public string TRANS { get; set; }
        [DataMember] public string COMPTEANNEXE1 { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime? DATECREATION { get; set; }
        [DataMember] public System.DateTime? DATECAISSE { get; set; }
        [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public decimal MONTANT{ get; set; }
        [DataMember] public decimal MONTANTDEBIT{ get; set; }
        [DataMember] public decimal MONTANTCREDIT{ get; set; }
        [DataMember] public decimal MONTANTHT { get; set; }
        [DataMember] public decimal MONTANTTAXE { get; set; }
        [DataMember] public decimal MONTANTTTC { get; set; }
        [DataMember] public string  REDEVANCE { get; set; }
        [DataMember] public string  NDOC { get; set; }
        [DataMember] public string  PRODUIT { get; set; }
        [DataMember] public bool  IsDebit { get; set; }
        [DataMember] public int  FK_IDCENTRE { get; set; }
        [DataMember] public int  FK_IDCAISSE { get; set; }
        [DataMember] public int  FK_IDCENTRECAISSE { get; set; }

        #region Interface comptable Sylla
        [DataMember] public string LIBELLECATEGORIE { get; set; }
        [DataMember] public string CENTREIMPUTATION { get; set; }
        [DataMember] public string CATEGORIE { get; set; }
        [DataMember] public string NUMEROMANDATEMENT { get; set; }
        [DataMember] public string NUMEROAVISCREDIT { get; set; }
        [DataMember] public string MODEREG { get; set; }
        [DataMember] public bool ISFACTURE { get; set; }
        [DataMember] public bool ISMANDATEMENT { get; set; }
        #endregion
    }
}
