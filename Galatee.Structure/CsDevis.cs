using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsDevis
    {

        # region Generale

        [DataMember]
        public ObjDEVIS  LeDevis { get; set; }
        [DataMember]
        public ObjDEMANDEDEVIS  LaDemandeDevis { get; set; }

        //[DataMember]
        //public string  NUMDEVIS { get; set; }
        //[DataMember]
        //public string  CENTRE { get; set; }
        //[DataMember]
        //public string  SITE { get; set; }
        //[DataMember]
        //public string NOM { get; set; }
        //[DataMember]
        //public string  CODEPRODUIT { get; set; }
        //[DataMember]
        //public string  IDTYPEDEVIS { get; set; }
        //[DataMember]
        //public DateTime ?  DATECREATION { get; set; }
        //[DataMember]
        //public DateTime ?  DATEETAPE { get; set; }
        //[DataMember]
        //public int   IDETAPEDEVIS { get; set; }
        //[DataMember]
        //public decimal MONTANTHT   { get; set; }  
        //[DataMember]
        //public decimal MONTANTTTC   { get; set; }
        //[DataMember]
        //public decimal MONTANTTVA { get; set; } 
        //[DataMember]
        //public decimal MONTANTTOUTORDRE   { get; set; }                
        //[DataMember]
        //public string  NUMEROCTR  { get; set; }                
        //[DataMember]
        //public string  MOTIFREJET  { get; set; }                
        //[DataMember]
        //public string  DATEREGLEMENT  { get; set; }                
        //[DataMember]
        //public string   MATRICULECAISSE   { get; set; }
        //[DataMember]
        //public string  ORDRE  { get; set; }        
        //[DataMember]
        //public bool   ISFOURNITURE  { get; set; }        
        //[DataMember]
        //public bool   ISDEPOSE  { get; set; }        
        //[DataMember]
        //public string  ISANALYSED  { get; set; }
        //[DataMember]
        //public string ISCREATED { get; set; }
        //[DataMember]
        //public string ISSUBVENTION { get; set; }
        //[DataMember]
        //public decimal  PUISSANCESOUSCRITE   { get; set; }
        //[DataMember]
        //public int ?     IDTYPECTR  { get; set; }
        //[DataMember]
        //public string  IDMARQUECRT { get; set; }
        //[DataMember]
        //public string   IDCARACTERISTIQUE { get; set; }
        //[DataMember]
        //public string TOURNEE { get; set; }

        //[DataMember]
        //public string QUARTIER { get; set; }
        //[DataMember]
        //public string  NUMTEL { get; set; }
        //[DataMember]
        //public string NUMRUE { get; set; }
        //[DataMember]
        //public string NUMPORTE { get; set; }
        //[DataMember]
        //public string  DATEFABRICATIONCRT { get; set; }
        //[DataMember]
        //public Int32 ?  INDEXPOSECTR { get; set; }
        //[DataMember]
        //public DateTime ?  DATEPOSECRT  { get; set; }
        //[DataMember]
        //public decimal DISTANCEBRT  { get; set; }
        //[DataMember]
        //public string  CATEGORY { get; set; }
        //[DataMember]
        //public string COMMUNE { get; set; } 
        #endregion

	
    }

}









