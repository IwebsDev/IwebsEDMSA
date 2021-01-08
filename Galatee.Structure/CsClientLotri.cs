using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsClientLotri
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public int POINT { get; set; }
        [DataMember] public string TOURNEE { get; set; }
        [DataMember] public string ORDTOUR { get; set; }
        [DataMember] public string NUMERO { get; set; }
        [DataMember] public string DIAMETRE { get; set; }
        [DataMember] public string ETATCOMPT { get; set; }
        [DataMember] public string DABON { get; set; }
        [DataMember] public DateTime? DRES { get; set; }
        [DataMember] public decimal? COEFK1 { get; set; }
        [DataMember] public decimal? COEFK2 { get; set; }
        [DataMember] public int? COEFFAC { get; set; }
        [DataMember] public string PERFAC { get; set; }
        [DataMember] public string CATEGORIECLIENT { get; set; }
        [DataMember] public int? COEFCOMPTAGE { get; set; }
        [DataMember] public int  MAXEVENT { get; set; }
        [DataMember] public int PK_EVENEMENT { get; set; }
        [DataMember] public int? NUMEVENEMENT { get; set; }
        [DataMember] public string TYPECOMPTAGE { get; set; }
        [DataMember] public string TYPECOMPTEUR { get; set; }
        [DataMember] public string TCOMPT { get; set; }
        [DataMember] public string MOISFAC { get; set; }
        [DataMember] public string CAS { get; set; }
        [DataMember] public string CODEEVT { get; set; }
        [DataMember] public string PERIODE { get; set; }
        [DataMember] public string CODEV { get; set; }
        [DataMember] public string MATRICULE { get; set; }
        [DataMember] public int? STATUS { get; set; }
        [DataMember] public decimal?   COEFLECT  { get; set; }
        [DataMember] public string TOPEDIT  { get; set; }
        [DataMember] public string LOTRI   { get; set; }
        [DataMember] public int? TYPECONSO { get; set; }
        [DataMember] public string TYPETARIF   { get; set; }
        [DataMember] public string FORFAIT   { get; set; }
        [DataMember] public string CODECONSO   { get; set; }
        [DataMember] public string FACPER { get; set; }
        [DataMember] public int? QTEAREG { get; set; }
        [DataMember] public string DERPERF { get; set; }
        [DataMember] public string DERPERFN { get; set; }
        [DataMember] public DateTime? DATECREATION { get; set; }
        [DataMember] public DateTime? DATEMODIFICATION { get; set; }
        [DataMember] public int? CONSONONFACTUREE { get; set; }
        [DataMember] public string  PROPRIETAIRE { get; set; }
        [DataMember] public int? REGCONSO { get; set; }
        [DataMember] public decimal? PUISSANCE { get; set; }
        [DataMember] public decimal PUISSANCEINSTALLEE { get; set; }
        [DataMember] public int? REGIMPUTE { get; set; }
        [DataMember] public int? FK_IDEVENEMENT { get; set; }
        [DataMember] public int? SURFACTURATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }


        // autres champs foreign key
         [DataMember] public int FK_IDCANALISATION { get; set; }
         [DataMember] public int FK_IDPRODUIT { get; set; }
         [DataMember] public int FK_IDCENTRE { get; set; }
         [DataMember] public int FK_IDCLIENT { get; set; }
         [DataMember] public int FK_IDCATEGORIE { get; set; }
         [DataMember] public int FK_IDRELEVEUR { get; set; }
         [DataMember] public int FK_IDABON { get; set; }
         [DataMember]  public int FK_DIAMETRE { get; set; }
         [DataMember] public int FK_IDTOURNEE { get; set; }
         [DataMember] public int FK_IDCOMPTEUR { get; set; }
        
    }

}









