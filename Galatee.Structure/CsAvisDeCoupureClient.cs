using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsAvisDeCoupureClient : CsPrint
    {
        [DataMember] public string  IDCOUPURE { get; set; }
        [DataMember] public string  CENTRE { get; set; }
        [DataMember] public string  CLIENT { get; set; }
        [DataMember] public string  ORDRE { get; set; }
        [DataMember] public string  RUE { get; set; }
        [DataMember] public string  PORTE { get; set; }
        [DataMember] public string  TOURNEE { get; set; }
        [DataMember] public string  ORDTOUR { get; set; }
        [DataMember] public string  COMPTEUR { get; set; }
        [DataMember] public string  NOMABON { get; set; }
        [DataMember] public string  CATEGORIE { get; set; }
        [DataMember] public string  CODECONSO { get; set; }
        
        [DataMember] public int  FK_IDLCLIENT { get; set; }
        [DataMember] public int  FK_IDCENTRE { get; set; }
        [DataMember] public int  FK_IDCLIENT { get; set; }
        [DataMember] public int  FK_IDABON { get; set; }
        [DataMember] public int  FK_IDCOMPTEUR { get; set; }
        [DataMember] public int  FK_IDTOURNEE { get; set; }
        [DataMember] public int  FK_IDCATEGORIECLIENT { get; set; }
        [DataMember] public int   IDFACTURE { get; set; }
        [DataMember] public string   NDOC { get; set; }
        [DataMember] public string   REFEM { get; set; }
        [DataMember] public string   COPER { get; set; }
        [DataMember] public string   LIBELLECOPER { get; set; }
        [DataMember] public decimal    MONTANT { get; set; }
        [DataMember] public decimal    PAIEMENT { get; set; }
        [DataMember] public decimal    SOLDEFACTURE { get; set; }
        [DataMember] public decimal    SOLDEDUE{ get; set; }
        [DataMember] public int   NOMBREFACTURE { get; set; }
        [DataMember] public int   NOMBRECLIENTTROUVE { get; set; }
        [DataMember] public string  NOMCONTROLER{ get; set; }
        [DataMember] public string  ADRESSE{ get; set; }
        [DataMember] public DateTime EXIGIBILITE { get; set; }

        

    }
}
