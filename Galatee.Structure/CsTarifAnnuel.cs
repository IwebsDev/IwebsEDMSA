using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class  CsTarifAnnuel
    {
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public string REDEVANCE { get; set; }
       [DataMember] public string REGION { get; set; }
       [DataMember] public string SREGION { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string COMMUNE { get; set; }
       [DataMember] public string RECHERCHETARIF { get; set; }
       [DataMember] public string CTARCOMP { get; set; }
       [DataMember] public System.DateTime DAPP { get; set; }
       [DataMember] public string PERDEB { get; set; }
       [DataMember] public string PERFIN { get; set; }
       [DataMember] public string LIBRED { get; set; }
       [DataMember] public string FEUILLE { get; set; }
       [DataMember] public string ACTUA { get; set; }
       [DataMember] public int? MINIMUM { get; set; }
       [DataMember] public string CTAX { get; set; }
       [DataMember] public string UNITE { get; set; }
       [DataMember] public int? BARVOL1 { get; set; }
       [DataMember] public int? BARVOL2 { get; set; }
       [DataMember] public int? BARVOL3 { get; set; }
       [DataMember] public int? BARVOL4 { get; set; }
       [DataMember] public int? BARVOL5 { get; set; }
       [DataMember] public int? BARVOL6 { get; set; }
       [DataMember] public int? BARVOL7 { get; set; }
       [DataMember] public int? BARVOL8 { get; set; }
       [DataMember] public int? BARVOL9 { get; set; }
       [DataMember] public decimal? BARPRIX1 { get; set; }
       [DataMember] public decimal? BARPRIX2 { get; set; }
       [DataMember] public decimal? BARPRIX3 { get; set; }
       [DataMember] public decimal? BARPRIX4 { get; set; }
       [DataMember] public decimal? BARPRIX5 { get; set; }
       [DataMember] public decimal? BARPRIX6 { get; set; }
       [DataMember] public decimal? BARPRIX7 { get; set; }
       [DataMember] public decimal? BARPRIX8 { get; set; }
       [DataMember] public decimal? BARPRIX9 { get; set; }
       [DataMember] public string TRANS { get; set; }
       [DataMember] public int? MINIVOL { get; set; }
       [DataMember] public decimal? MINIVAL { get; set; }
       [DataMember] public int? FORFVOL { get; set; }
       [DataMember] public decimal? FORFVAL { get; set; }
       [DataMember] public string FEUILLERED { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }

        /*Autre */
        [DataMember]
       public string ORDRERED { get; set; }
       public string MODECALCUL { get; set; }

    }

}
