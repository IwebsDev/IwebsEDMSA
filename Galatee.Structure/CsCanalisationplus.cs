using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsCanalisationplus
    {

        # region Generale
        [DataMember]
        public string CENTRE { get; set; }
        [DataMember]
        public string CLIENT { get; set; }
        [DataMember]
        public string ORDRE { get; set; }
        [DataMember]
        public string PRODUIT { get; set; }
        #endregion

        #region CsCanalisationplus
        [DataMember]
        public string NUMDEM { get; set; }

        [DataMember]
        public int  POINT { get; set; }
        [DataMember]
        public string COMMUNE { get; set; }
        [DataMember]
        public string QUARTIER { get; set; }
        [DataMember]
        public string SECTEUR { get; set; }
        [DataMember]
        public string NOMRUE { get; set; }
        [DataMember]
        public string NUMRUE { get; set; }
        [DataMember]
        public string CODERUE { get; set; }
        [DataMember]
        public string COMPRUE { get; set; }
        [DataMember]
        public string ETAGE { get; set; }
        [DataMember]
        public string PORTE { get; set; }
        [DataMember]
        public string CADR { get; set; }
        [DataMember]
        public string CPARC { get; set; }
        [DataMember]
        public string CPOS { get; set; }
        [DataMember]
        public string ENTRETIEN { get; set; }
        [DataMember]
        public string DIAMETRE { get; set; }
        [DataMember]
        public string NATURE { get; set; }
        [DataMember]
        public string USAGE { get; set; }
        [DataMember]
        public string CARTOGRAPHIE { get; set; }
        [DataMember]
        public string QUESTION { get; set; }
        [DataMember]
        public string COMMENTAIREADR { get; set; }
        [DataMember]
        public string COMMENTAIRECPT { get; set; }
        [DataMember]
        public string COMMENTAIREINS { get; set; }
        [DataMember]
        public string COMMENTAIRETEC { get; set; }
        [DataMember]
        public string INTERLOCUTEUR1 { get; set; }
        [DataMember]
        public string TELEPHONE1 { get; set; }
        [DataMember]
        public string INTERLOCUTEUR2 { get; set; }
        [DataMember]
        public string TELEPHONE2 { get; set; }
        [DataMember]
        public string INTERLOCUTEUR3 { get; set; }
        [DataMember]
        public string TELEPHONE3 { get; set; }
        [DataMember]
        public string INTERLOCUTEUR4 { get; set; }
        [DataMember]
        public string TELEPHONE4 { get; set; }
        #endregion





    }

}









