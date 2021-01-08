using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsStrategieSecurite 
    {
        [DataMember]
        public Guid PK_IDSTRATEGIESECURITE { get; set; }
        [DataMember]
        public Guid PK_ID { get; set; }
        [DataMember]
        public string LIBELLE { get; set; }
        [DataMember]
        public bool ACTIF { get; set; }
        [DataMember]
        public int? HISTORIQUENOMBREPASSWORD { get; set; }
        [DataMember]
        public int? DUREEMINIMALEPASSWORD { get; set; }
        [DataMember]
        public int? DUREEMAXIMALEPASSWORD { get; set; }
        [DataMember]
        public int? LONGUEURMINIMALEPASSWORD { get; set; }
        [DataMember]
        public bool CHIFFREMENTREVERSIBLEPASSWORD { get; set; }
        [DataMember]
        public string TOUCHEVERROUILLAGESESSION { get; set; }
        [DataMember]
        public int? NOMBREMAXIMALECHECSOUVERTURESESSION { get; set; }
        [DataMember]
        public int? DUREEVEUILLESESSION { get; set; }
        //public short SeuilVerrouillageCompte { get; set; }
        [DataMember]
        public int? DUREEVERROUILLAGECOMPTE { get; set; }
        [DataMember]
        public int? REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES { get; set; }
        [DataMember]
        public bool NEPASCONTENIRNOMCOMPTE { get; set; }
        [DataMember]
        public int? NOMBREMINIMALCARACTERESMAJUSCULES { get; set; }
        [DataMember]
        public int? NOMBREMINIMALCARACTERESMINISCULES { get; set; }
        [DataMember]
        public int? NOMBREMINIMALCARACTERESCHIFFRES { get; set; }
        [DataMember]
        public int? NOMBREMINIMALCARACTERENONALPHABETIQUES { get; set; }
        [DataMember]
        public DateTime? DATECREATION { get; set; }
        [DataMember]
        public DateTime? DATEMODIFICATION { get; set; }
        [DataMember]
        public string USERCREATION { get; set; }
        [DataMember]
        public string USERMODIFICATION { get; set; }
    }

}









