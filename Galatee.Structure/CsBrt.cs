using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsBrt
    {
           [DataMember] public string CENTRE { get; set; }
           [DataMember] public string CLIENT { get; set; }
           [DataMember] public string NUMDEM { get; set; }
           [DataMember] public string PRODUIT { get; set; }
           [DataMember] public Nullable<System.DateTime> DRAC { get; set; }
           [DataMember] public Nullable<System.DateTime> DRES { get; set; }
           [DataMember] public string SERVICE { get; set; }
           [DataMember] public string CATBRT { get; set; }
           [DataMember] public string DIAMBRT { get; set; }
           [DataMember] public Nullable<decimal> LONGBRT { get; set; }
           [DataMember] public Nullable<decimal> LONGEXTENSION { get; set; }
           [DataMember] public string NATBRT { get; set; }
           [DataMember] public Nullable<int> NBPOINT { get; set; }
           [DataMember] public string RESEAU { get; set; }
           [DataMember] public string TRONCON { get; set; }
           [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
           [DataMember] public string TRANSFORMATEUR { get; set; }
           [DataMember] public Nullable<decimal> PUISSANCEINSTALLEE { get; set; }
           [DataMember] public Nullable<decimal> PERTES { get; set; }
           [DataMember] public Nullable<decimal> COEFPERTES { get; set; }
           [DataMember] public string TYPECOMPTAGE { get; set; }
           [DataMember] public string APPTRANSFO { get; set; }
           [DataMember] public string CODEBRT { get; set; }
           [DataMember] public string CODEPOSTE { get; set; }
           [DataMember] public string MARQUETRANSFO { get; set; }
           [DataMember] public string ANFAB { get; set; }
           [DataMember] public string LONGITUDE { get; set; }
           [DataMember] public string LATITUDE { get; set; }
           [DataMember] public string ADRESSERESEAU { get; set; }
           [DataMember] public string USERCREATION { get; set; }
           [DataMember] public System.DateTime DATECREATION { get; set; }
           [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
           [DataMember] public string USERMODIFICATION { get; set; }
           [DataMember] public int PK_ID { get; set; }
           [DataMember] public int FK_IDCENTRE { get; set; }
           [DataMember] public int FK_IDPRODUIT { get; set; }
           [DataMember] public Nullable<int> FK_IDTYPEBRANCHEMENT { get; set; }
           [DataMember] public Nullable<int> FK_IDTYPECOMPTAGE { get; set; }
           [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
           [DataMember] public Nullable<bool> ISBORNEPOSTE { get; set; }
           [DataMember] public Nullable<int> NOMBRETRANSFORMATEUR { get; set; }
           [DataMember] public Nullable<int> FK_IDPOSTESOURCE { get; set; }
           [DataMember] public Nullable<int> FK_IDDEPARTHTA { get; set; }
           [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
           [DataMember] public Nullable<int> FK_IDPOSTETRANSFORMATION { get; set; }
           [DataMember] public string DEPARTBT { get; set; }
           [DataMember] public string NEOUDFINAL { get; set; }
           [DataMember] public Nullable<int> FK_IDAG { get; set; }

        

           [DataMember] public string TOURNEE { get; set; }
           [DataMember] public string ORDTOUR { get; set; }
           [DataMember] public Nullable<int> FK_IDTOURNEE { get; set; }
           [DataMember] public string NOMABON { get; set; }
        

        [DataMember] public string CODETYPEBRANCHEMENT { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLECENTRE{ get; set; }
        [DataMember] public string LIBELLETOURNEE{ get; set; }
        [DataMember] public string LIBELLETYPEBRANCHEMENT { get; set; }
        [DataMember] public string LIBELLETYPECOMPTAGE { get; set; }

        [DataMember] public string CODEPOSTESOURCE { get; set; }
        [DataMember] public string CODEDEPARTHTA { get; set; }
        [DataMember] public string CODETRANSFORMATEUR { get; set; }
        [DataMember] public string CODEQUARTIER { get; set; }

        [DataMember] public string LIBELLEPOSTESOURCE { get; set; }
        [DataMember] public string LIBELLEDEPARTHTA  { get; set; }
        [DataMember] public string LIBELLETRANSFORMATEUR { get; set; }
        [DataMember] public string LIBELLEQUARTIER { get; set; }
        [DataMember] public bool ISMODIFIER { get; set; }


        

        
    
    }

}









