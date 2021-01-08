using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{


    [DataContract]
    public class CsTarifUnitaire
    {
          [DataMember] public int PK_ID { get; set; }
          [DataMember] public string PRODUIT { get; set; }
          [DataMember] public string REDEVANCE { get; set; }
          [DataMember] public string REGION { get; set; }
          [DataMember] public string SREGION { get; set; }
          [DataMember] public string CENTRE { get; set; }
          [DataMember] public string COMMUNE { get; set; }
          [DataMember] public string RECHERCHETARIF { get; set; }
          [DataMember] public string CTARCOMP { get; set; }
          [DataMember] public Nullable<System.DateTime> DAPP { get; set; }
          [DataMember] public string PERDEB { get; set; }
          [DataMember] public string PERFIN { get; set; }
          [DataMember] public string LIBRED { get; set; }
          [DataMember] public string FEUILLE { get; set; }
          [DataMember] public string ACTUA { get; set; }
          [DataMember] public Nullable<int> FORFAIT { get; set; }
          [DataMember] public string CTAX { get; set; }
          [DataMember] public string UNITE { get; set; }
          [DataMember] public Nullable<decimal> MONTANT { get; set; }
          [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
          [DataMember] public string TRANS { get; set; }
          [DataMember] public string FEUILLERED { get; set; }
          [DataMember] public Nullable<int> FK_IDPRODUIT { get; set; }
          [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }

          /*Autre */
          [DataMember] public string ORDRERED { get; set; }
          [DataMember] public string MODECALCUL { get; set; }
        


    }

}
