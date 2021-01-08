using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsVariableDeTarification:CsPrint
    {

        #region Champs du nouveau model        
        [DataMember] public string REDEVANCE { get; set; }
        [DataMember] public string REGION { get; set; }
        [DataMember] public string SREGION { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string COMMUNE { get; set; }
        [DataMember] public Nullable<byte> ORDREEDITION { get; set; }
        [DataMember] public System.DateTime DATEAPPLICATION { get; set; }
        [DataMember] public string RECHERCHETARIF { get; set; }
        [DataMember] public string MODECALCUL { get; set; }
        [DataMember] public string MODEAPPLICATION { get; set; }
        [DataMember] public string LIBELLECOMPTABLE { get; set; }
        [DataMember] public string COMPTECOMPTABLE { get; set; }
        [DataMember] public Nullable<bool> ESTANALYTIQUE { get; set; }
        [DataMember] public Nullable<bool> GENERATIONANOMALIE { get; set; }
        [DataMember] public string FORMULE { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDREDEVANCE { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDMODEAPPLICATION { get; set; }
        [DataMember] public int FK_IDMODECALCUL { get; set; }
        [DataMember] public int FK_IDRECHERCHETARIF { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public string  PRODUIT { get; set; }
        [DataMember] public string  CTARCOMP { get; set; }

        #endregion
        [DataMember] public string REDEVANCE_RECHERCHE { get; set; }
        [DataMember] public string CODEREDEVENCE { get; set; }
        [DataMember] public string CODERECHERCHE { get; set; }
    }

}
