using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsClientRechercher :CsPrint 
    {
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRE { get; set; }
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public int  POINT { get; set; }
       [DataMember] public string NOMABON { get; set; }
       [DataMember] public decimal AVANCE { get; set; }
       [DataMember] public string TOURNEE { get; set; }
       [DataMember] public string SEQUENCE { get; set; }
       [DataMember] public string NUMCOMPTEUR { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDABON{ get; set; }
       [DataMember] public int OptionRecherche { get; set; }
       [DataMember] public string LONGITUDE { get; set; }
       [DataMember] public string LATITUDE { get; set; }

       [DataMember] public string ADRESSEELECTRIQUE { get; set; }
       [DataMember] public string PORTE { get; set; }
       [DataMember] public string RUE { get; set; }
       [DataMember] public string LOT { get; set; }
       [DataMember] public string PERIODE { get; set; }
       [DataMember] public string FACTURE { get; set; }

    }

}









