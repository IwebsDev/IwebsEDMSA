//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galatee.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ADMSTRATEGIESECURITE
    {
        public System.Guid PK_ID { get; set; }
        public string LIBELLE { get; set; }
        public bool ACTIF { get; set; }
        public int HISTORIQUENOMBREPASSWORD { get; set; }
        public int DUREEMINIMALEPASSWORD { get; set; }
        public int DUREEMAXIMALEPASSWORD { get; set; }
        public int LONGUEURMINIMALEPASSWORD { get; set; }
        public bool CHIFFREMENTREVERSIBLEPASSWORD { get; set; }
        public string TOUCHEVERROUILLAGESESSION { get; set; }
        public int NOMBREMAXIMALECHECSOUVERTURESESSION { get; set; }
        public int DUREEVEUILLESESSION { get; set; }
        public int DUREEVERROUILLAGECOMPTE { get; set; }
        public int REINITIALISERCOMPTEURVERROUILLAGECOMPTEAPRES { get; set; }
        public bool NEPASCONTENIRNOMCOMPTE { get; set; }
        public int NOMBREMINIMALCARACTERESMAJUSCULES { get; set; }
        public int NOMBREMINIMALCARACTERESMINISCULES { get; set; }
        public int NOMBREMINIMALCARACTERESCHIFFRES { get; set; }
        public int NOMBREMINIMALCARACTERENONALPHABETIQUES { get; set; }
        public string USERCREATION { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
    }
}
