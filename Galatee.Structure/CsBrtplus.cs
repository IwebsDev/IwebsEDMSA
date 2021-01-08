using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsBrtplus
    {

          [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public string NUMTT { get; set; }
       [DataMember] public string MARQUETT { get; set; }
       [DataMember] public string ANFABTT { get; set; }
       [DataMember] public decimal? RAPPORTTT1 { get; set; }
       [DataMember] public decimal? RAPPORTTT2 { get; set; }
       [DataMember] public string NUMTC { get; set; }
       [DataMember] public string MARQUETC { get; set; }
       [DataMember] public string ANFABTC { get; set; }
       [DataMember] public decimal? RAPPORTTC1 { get; set; }
       [DataMember] public decimal? RAPPORTTC2 { get; set; }
       [DataMember] public string NUMCH { get; set; }
       [DataMember] public string MARQUECH { get; set; }
       [DataMember] public string ANFABCH { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public System.DateTime? DATEMODIFICATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDPRODUIT { get; set; }
       [DataMember] public int FK_IDNUMDEM { get; set; }
       [DataMember] public int FK_IDBRT { get; set; }
       [DataMember] public string NUMDEM { get; set; }
    }

}









