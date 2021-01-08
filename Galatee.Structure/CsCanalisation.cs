using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsCanalisation:CsPrint 
    {
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string PROPRIO { get; set; }
        [DataMember] public int POINT { get; set; }
        [DataMember] public string BRANCHEMENT { get; set; }
        [DataMember] public Nullable<int> SURFACTURATION { get; set; }
        [DataMember] public Nullable<int> DEBITANNUEL { get; set; }
        [DataMember] public string REPCOMPT { get; set; }
        [DataMember] public Nullable<System.DateTime> POSE { get; set; }
        [DataMember] public Nullable<System.DateTime> DEPOSE { get; set; }
        [DataMember] public Nullable<int> ORDRERELEVE { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public int FK_IDABON { get; set; }
        [DataMember] public int FK_IDCENTRE { get; set; }
        [DataMember] public int? FK_IDCOMPTEUR { get; set; }
        [DataMember] public int? FK_IDTYPECOMPTAGE { get; set; }
        [DataMember] public int FK_IDPRODUIT { get; set; }
        [DataMember] public int FK_IDPROPRIETAIRE { get; set; }
        [DataMember] public Nullable<bool> ESTACTIF { get; set; }
        [DataMember] public Nullable<int> FK_IDREGLAGECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDTYPECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDMARQUECOMPTEUR { get; set; }
        [DataMember] public string NUMERO { get; set; }
        [DataMember] public string TYPECOMPTEUR { get; set; }
        [DataMember] public string TYPECOMPTAGE { get; set; }
        [DataMember] public string REGLAGECOMPTEUR { get; set; }
        [DataMember]  public string MARQUE { get; set; }
        [DataMember] public Nullable<decimal> COEFLECT { get; set; }
        [DataMember] public Nullable<int> COEFCOMPTAGE { get; set; }
        [DataMember] public Nullable<byte> CADRAN { get; set; }
        [DataMember] public string ANNEEFAB { get; set; }
        [DataMember] public string FONCTIONNEMENT { get; set; }
        [DataMember] public string PLOMBAGE { get; set; }
        [DataMember] public string ANCCOMPTEUR { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string NUMDEM { get; set; }
        [DataMember] public Nullable<int> FK_IDDEMANDE { get; set; }
        [DataMember] public Nullable<int> FK_IDMAGAZINVIRTUEL { get; set; }
        [DataMember] public string REPERAGECOMPTEUR { get; set; }

        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }
        [DataMember] public string LIBELLETYPECOMPTEUR { get; set; }
        [DataMember] public string LIBELLETYPECOMPTAGE { get; set; }
        [DataMember] public string LIBELLEREGLAGECOMPTEUR { get; set; }
        [DataMember] public string LIBELLEMARQUE { get; set; }
        [DataMember] public string ETATDUCOMPTEUR { get; set; }

        [DataMember] public string LIBELLEETATCOMPTEUR { get; set; }
        [DataMember] public string INFOCOMPTEUR { get; set; }
        [DataMember] public string COMMENTAIRE { get; set; }
        [DataMember] public int? INDEXEVT { get; set; }
        [DataMember] public int? CONSO { get; set; }
        [DataMember] public Nullable<bool> ISRECU { get; set; }
        [DataMember] public Nullable<bool> ISLIVRE { get; set; }
        [DataMember] public bool IsSelect { get; set; }
        [DataMember] public string CAS { get; set; }
        [DataMember] public string PERIODE { get; set; }
        [DataMember] public byte ORDREAFFICHAGE { get; set; }

        [DataMember] public Nullable<int> FK_IDCALIBRE { get; set; }
        [DataMember] public string NOMCLIENT { get; set; }
        [DataMember] public int FK_IDSTATUTCOMPTEUR { get; set; }
        [DataMember] public int FK_IDETATCOMPTEUR { get; set; }
        [DataMember] public string CODECALIBRECOMPTEUR { get; set; }

        [DataMember] public string LIBELLEMATERIEL { get; set; }
        [DataMember] public int  QUANTITE { get; set; }
        [DataMember] public int FK_IDMATERIELDEVIS { get; set; }
        [DataMember] public bool ISMODIFIER { get; set; }
        

    }
}