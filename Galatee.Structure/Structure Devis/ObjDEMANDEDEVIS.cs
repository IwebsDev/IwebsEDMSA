using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

//Fichier  généré par InovaDTOcreator , - utilitaire WPF  créé par JAB pour la generation des Data  Transfert Object http://beableworld.wordpress.com

namespace Galatee.Structure

{
    [DataContract]
    public class ObjDEMANDEDEVIS
    {
        
       [DataMember] public string NUMDEVIS { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string CLIENT { get; set; }
       [DataMember] public string ORDRECLIENT { get; set; }
       [DataMember] public string DIAMETRE { get; set; }
       [DataMember] public string NOM { get; set; }
       [DataMember] public string TOURNEE { get; set; }
       [DataMember] public string PROFESSION { get; set; }
       [DataMember] public string NUMLOT { get; set; }
       [DataMember] public string PARCELLE { get; set; }
       [DataMember] public string SECTION_PAR { get; set; }
       [DataMember] public string QUARTIER { get; set; }
       [DataMember] public string SECTEUR { get; set; }
       [DataMember] public string NUMTEL { get; set; }
       [DataMember] public string RUE { get; set; }
       [DataMember] public string NUMPORTE { get; set; }
       [DataMember] public string NUMPOTEAUPROCHE { get; set; }
       [DataMember] public string CATEGORY { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEDEMANDE { get; set; }
       [DataMember] public string ADRESSE { get; set; }
       [DataMember] public string COMMUNE { get; set; }
       [DataMember] public string REPEREPROCHE { get; set; }
       [DataMember] public string ORDTOUR { get; set; }
       [DataMember] public string LONGITUDE { get; set; }
       [DataMember] public string LATITUDE { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public int FK_IDDIAMETRECOMPTEUR { get; set; }
       [DataMember] public int FK_IDTOURNEE { get; set; }
       [DataMember] public int FK_IDQUARTIER { get; set; }
       [DataMember] public Nullable<int> FK_IDCATEGORIECLIENT { get; set; }
       [DataMember] public int FK_IDRUE { get; set; }
       [DataMember] public int FK_IDCOMMUNE { get; set; }
       [DataMember] public int FK_IDDEVIS { get; set; }


        [DataMember] public string LIBELLETOURNEE { get; set; }
        [DataMember] public string LIBELLECOMMUNE { get; set; }
        [DataMember] public string LIBELLEQUARTIER { get; set; }
        [DataMember] public string LIBELLERUE { get; set; }
        [DataMember] public string LIBELLEDIAMETRE { get; set; }
        [DataMember] public string LIBELLECATEGORIE { get; set; }
        [DataMember] public string LIBELLETYPEPIECE { get; set; }
        [DataMember] public string REFEM { get; set; }
        [DataMember]public string NDOC { get; set; }
    }
}