using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Galatee.Silverlight.Security
{
    public class CsStrategieSecurite 
    {
        public bool ACTIF { get; set; }
        public bool CHIFFREMENTREVERSIBLEPASSWORD { get; set; }
        public DateTime? DATECREATION { get; set; }
        public DateTime? DATEMODIFICATION { get; set; }
        public int? DUREEMAXIMALEPASSWORD { get; set; }
        public int? DUREEMINIMALEPASSWORD { get; set; }
        public int? DUREEVERROUILLAGECOMPTE { get; set; }
        public int? DUREEVEUILLESESSION { get; set; }
        public int? HISTORIQUENOMBREPASSWORD { get; set; }
        public string LIBELLE { get; set; }
        public int? LONGUEURMINIMALEPASSWORD { get; set; }
        public bool NEPASCONTENIRNOMCOMPTE { get; set; }
        public int? NOMBREMAXIMALECHECSOUVERTURESESSION { get; set; }
        public int? NOMBREMINIMALCARACTERENONALPHABETIQUES { get; set; }
        public int? NOMBREMINIMALCARACTERESCHIFFRES { get; set; }
        public int? NOMBREMINIMALCARACTERESMAJUSCULES { get; set; }
        public int? NombreMinimalCaracteresMinuscules { get; set; }
        public Guid PK_ID { get; set; }
        public Guid PK_IDSTRATEGIESECURITE { get; set; }
        public int? REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES { get; set; }
        public string TOUCHEVERROUILLAGESESSION { get; set; }
        public string USERCREATION { get; set; }
        public string USERMODIFICATION { get; set; }
    }

}









