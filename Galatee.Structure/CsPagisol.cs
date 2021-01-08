using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsPagisol

    {
        [DataMember] public string LOTRI { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string TOURNEE { get; set; }
        [DataMember] public string ORDTOUR { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public int POINT { get; set; }
        [DataMember] public string CAS { get; set; }
        [DataMember] public string STATUT { get; set; }
        [DataMember] public string TOPEDIT { get; set; }
        [DataMember] public string CATEGORIECLIENT { get; set; }
        [DataMember] public string FREQUENCE { get; set; }
        [DataMember] public string TFAC { get; set; }
        [DataMember] public Nullable<System.DateTime> DDEB { get; set; }
        [DataMember] public Nullable<System.DateTime> DFIN { get; set; }
        [DataMember] public Nullable<int> QTEFAC { get; set; }
        [DataMember] public Nullable<int> AIED { get; set; }
        [DataMember] public Nullable<int> NIED { get; set; }
        [DataMember] public string PERFAC { get; set; }
        [DataMember] public string FACTURE { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDEVENEMENT { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int FK_IDCATEGORIECLIENT { get; set; }
        [DataMember] public int FK_IDABON { get; set; }
        [DataMember] public int FK_IDTOURNEE { get; set; }
        [DataMember] public bool ISCONSOSEUL { get; set; }
        
       [DataMember]  public string LIBELLEPRODUIT { get; set; }

        

    }

}