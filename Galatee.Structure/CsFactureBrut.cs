using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsFactureBrut : CsPrint
    {
        [DataMember] public int NOMBRECALCULE { get; set; }
        [DataMember] public decimal  MONTANT { get; set; }
        [DataMember] public int CONSOTOTAL { get; set; }
        [DataMember] public int REJETE { get; set; }
        [DataMember] public int  FK_IDCENTRE { get; set; }
        [DataMember] public string  LOTRI { get; set; }
        [DataMember] public string  JET { get; set; }
        [DataMember] public string  CENTRE { get; set; }
        [DataMember] public string  CLIENT { get; set; }
        [DataMember] public string  ORDRE { get; set; }
        [DataMember] public string  DENABON { get; set; }
        [DataMember] public string  NOMABON { get; set; }
        [DataMember] public string  DENMAND { get; set; }
        [DataMember] public string  ADRMAND1 { get; set; }
        [DataMember] public string  ADRMAND2 { get; set; }
        [DataMember] public string  CODCONSO { get; set; }
        [DataMember] public string  CATEGORIECLIENT { get; set; }
        [DataMember] public string  REGROUPEMENT { get; set; }
        [DataMember] public string  AG { get; set; }
        [DataMember] public string  COMMUNE { get; set; }
        [DataMember] public string  QUARTIER { get; set; }
        [DataMember] public string  RUE { get; set; }
        [DataMember] public string  PORTE { get; set; }
        [DataMember] public string  TOURNEE { get; set; }
        [DataMember] public string  ORDTOUR { get; set; }
        [DataMember] public string  FACTURE { get; set; }
        [DataMember] public decimal   TOTFHT { get; set; }
        [DataMember] public decimal   TOTFTAX { get; set; }
        [DataMember] public decimal   TOTFTTC { get; set; }
        [DataMember] public string  PERIODE { get; set; }
        [DataMember] public int  EXIG { get; set; }
        [DataMember] public string  COPER { get; set; }
        [DataMember] public string  MODEPAIEMENT { get; set; }
        [DataMember] public DateTime?   DRESABON { get; set; }
        [DataMember] public DateTime   DFAC { get; set; }
        [DataMember] public DateTime  DATECREATION { get; set; }
        [DataMember] public string  USERCREATION { get; set; }
        [DataMember] public int  FK_IDCATEGORIECLIENT { get; set; }
        [DataMember] public int  FK_IDTOURNEE { get; set; }
        [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
        [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }

        [DataMember] public int  FK_IDCLIENT { get; set; }
        [DataMember] public bool  ISFACTUREEMAIL { get; set; }
        [DataMember] public bool  ISFACTURESMS { get; set; }
        [DataMember] public string  EMAIL { get; set; }
        [DataMember] public string  TELEPHONE { get; set; }
        [DataMember] public string  TYPECOMPTAGE { get; set; }
        [DataMember] public string  COMPTEUR { get; set; }
        [DataMember] public string  DIAMETRE { get; set; }
        [DataMember] public string  REGLAGECOMPTEUR { get; set; }
        [DataMember] public decimal   COEFLECT { get; set; }
        [DataMember] public int  POINT { get; set; }
        [DataMember] public decimal   PUISSANCE { get; set; }
        [DataMember] public string  DERPERF { get; set; }
        [DataMember] public string  DERPERFN { get; set; }
        [DataMember] public int  REGCONSO { get; set; }
        [DataMember] public int  REGFAC { get; set; }
        [DataMember] public string  TFAC { get; set; }
        [DataMember] public int   CONSOFAC { get; set; }
        [DataMember] public DateTime?   DEV { get; set; }
        [DataMember] public DateTime?   DATEEVT { get; set; }
        [DataMember] public int  AINDEX { get; set; }
        [DataMember] public int  NINDEX { get; set; }
        [DataMember] public string  CAS { get; set; }
        [DataMember] public int  CONSO { get; set; }
        [DataMember] public decimal   TOTPROHT { get; set; }
        [DataMember] public decimal    TOTPROTAX { get; set; }
        [DataMember] public decimal   TOTPROTTC { get; set; }
        [DataMember] public string  ADERPERF { get; set; }
        [DataMember] public string  ADERPERFN { get; set; }
        [DataMember] public int  REGIMPUTE { get; set; }
        [DataMember] public string  TYPECOMPTEUR { get; set; }
        [DataMember] public DateTime?   DEVPRE { get; set; }
        [DataMember] public int   EVENEMENT { get; set; }
        [DataMember] public decimal   PUISSANCEINSTALLEE { get; set; }
        [DataMember] public int  COEFCOMPTAGE { get; set; }
        [DataMember] public decimal   COEFK1 { get; set; }
        [DataMember] public decimal   COEFK2 { get; set; }
        [DataMember] public decimal   PERTESACTIVES { get; set; }
        [DataMember] public decimal   PERTESREACTIVES { get; set; }
        [DataMember] public int   COEFFAC { get; set; }
        [DataMember] public int   FK_IDEVENEMENT { get; set; }
        [DataMember] public int   FK_IDTYPECOMPTAGE { get; set; }
        [DataMember] public int   FK_IDABON { get; set; }
        [DataMember] public int   FK_IDCAS { get; set; }
        [DataMember] public int   FK_IDPRODUIT { get; set; }
        [DataMember] public string  PRODUIT { get; set; }
        [DataMember] public string  REDEVANCE { get; set; }
        [DataMember] public string  TRANCHE { get; set; }
        [DataMember] public int  QUANTITE { get; set; }
        [DataMember] public string  UNITE { get; set; }
        [DataMember] public decimal   BARPRIX { get; set; }
        [DataMember] public decimal   TAXE { get; set; }
        [DataMember] public string  CTAX { get; set; }
        [DataMember] public decimal   TOTREDHT { get; set; }
        [DataMember] public decimal   TOTREDTAX { get; set; }
        [DataMember] public decimal   TOTREDTTC { get; set; }
        [DataMember] public string  PARAM6 { get; set; }
        [DataMember] public int  NBJOUR { get; set; }
        [DataMember] public DateTime?   DEBUTAPPLICATION { get; set; }
        [DataMember] public string    FORMULE { get; set; }
        [DataMember] public int     QUANTITEMAXIMALE { get; set; }
        [DataMember] public string    ORDRED { get; set; }
        [DataMember] public string    NATURE { get; set; }
        [DataMember] public bool   IsSimuler { get; set; }

        [DataMember] public string MATRICULE { get; set; }

        
    }

}









