
using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsClient:CsPrint 
    {
        [DataMember] public string TELEPHONEFIXE { get; set; }
        [DataMember] public string NUMDEM { get; set; }
        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string REFCLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string CODEIDENTIFICATIONNATIONALE { get; set; }
        [DataMember] public string DENABON { get; set; }
        [DataMember] public string NOMABON { get; set; }
        [DataMember] public string DENMAND { get; set; }
        [DataMember] public string NOMMAND { get; set; }
        [DataMember] public string ADRMAND1 { get; set; }
        [DataMember] public string ADRMAND2 { get; set; }
        [DataMember] public string CPOS { get; set; }
        [DataMember] public string BUREAU { get; set; }
        [DataMember] public Nullable<System.DateTime> DINC { get; set; }
        [DataMember] public string MODEPAIEMENT { get; set; }
        [DataMember] public string NOMTIT { get; set; }
        [DataMember] public string BANQUE { get; set; }
        [DataMember] public string GUICHET { get; set; }
        [DataMember] public string COMPTE { get; set; }
        [DataMember] public string RIB { get; set; }
        [DataMember] public string PROPRIO { get; set; }
        [DataMember] public string CODECONSO { get; set; }
        [DataMember] public string CATEGORIE { get; set; }
        [DataMember] public string CODERELANCE { get; set; }
        [DataMember] public string NOMCOD { get; set; }
        [DataMember] public string MOISNAIS { get; set; }
        [DataMember] public string ANNAIS { get; set; }
        [DataMember] public string NOMPERE { get; set; }
        [DataMember] public string NOMMERE { get; set; }
        [DataMember] public string NATIONNALITE { get; set; }
        [DataMember] public string CNI { get; set; }
        [DataMember] public string TELEPHONE { get; set; }
        [DataMember] public string MATRICULE { get; set; }
        [DataMember] public string REGROUPEMENT { get; set; }
        [DataMember] public string REGEDIT { get; set; }
        [DataMember] public string FACTURE { get; set; }
        [DataMember] public Nullable<System.DateTime> DMAJ { get; set; }
        [DataMember] public string NATURE { get; set; }
        [DataMember] public Nullable<int> REFERENCEPUPITRE { get; set; }
        [DataMember] public Nullable<int> PAYEUR { get; set; }
        [DataMember] public string SOUSACTIVITE { get; set; }
        [DataMember] public string AGENTFACTURE { get; set; }
        [DataMember] public string AGENTRECOUVR { get; set; }
        [DataMember] public string AGENTASSAINI { get; set; }
        [DataMember] public string REGROUCONTRAT { get; set; }
        [DataMember] public string INSPECTION { get; set; }
        [DataMember] public string REGLEMENT { get; set; }
        [DataMember] public string DECRET { get; set; }
        [DataMember] public string CONVENTION { get; set; }
        [DataMember] public string REFERENCEATM { get; set; }
        [DataMember] public int PK_ID { get; set; }
        [DataMember] public System.DateTime DATECREATION { get; set; }
        [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        [DataMember] public string USERCREATION { get; set; }
        [DataMember] public string USERMODIFICATION { get; set; }
        [DataMember] public Nullable<int> FK_IDMODEPAIEMENT { get; set; }
        [DataMember] public Nullable<int> FK_IDCODECONSO { get; set; }
        [DataMember] public Nullable<int> FK_IDCATEGORIE { get; set; }
        [DataMember] public Nullable<int> FK_IDRELANCE { get; set; }
        [DataMember] public Nullable<int> FK_IDNATIONALITE { get; set; }
        [DataMember] public Nullable<int> FK_IDNATURECLIENT { get; set; }
        [DataMember] public Nullable<int> FK_IDCENTRE { get; set; }
        [DataMember] public string EMAIL { get; set; }
        [DataMember] public string NUMEROPIECEIDENTITE { get; set; }
        [DataMember] public string NUMPROPRIETE { get; set; }
        [DataMember] public Nullable<bool> ISFACTUREEMAIL { get; set; }
        [DataMember] public Nullable<bool> ISFACTURESMS { get; set; }
        [DataMember] public Nullable<int> FK_IDPAYEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDREGROUPEMENT { get; set; }
        [DataMember] public Nullable<int> FK_IDAG { get; set; }
        [DataMember] public Nullable<int> FK_IDPIECEIDENTITE { get; set; }
        [DataMember] public string MATRICULEAGENT { get; set; }

        [DataMember] public Nullable<int> FK_IDNUMDEM { get; set; }
        [DataMember] public Nullable<int> FK_IDSITE { get; set; }
        [DataMember] public Nullable<int> FK_TYPECLIENT { get; set; }
        [DataMember] public Nullable<int> FK_IDUSAGE { get; set; }
        [DataMember] public string FAX { get; set; }
        [DataMember] public string BOITEPOSTAL { get; set; }

        // AUTRE PROPRIETE DE LA CLASSE
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public int? INDEXEVT { get; set; }
        [DataMember] public decimal? SOLDE { get; set; }
        [DataMember] public decimal? SOLDEDOCUMENT { get; set; }
        [DataMember] public decimal? SOLDEDUE { get; set; }
        [DataMember] public decimal? AVANCE { get; set; }
        [DataMember] public decimal? SOLDENAF { get; set; }
        [DataMember] public string COMPTEUR { get; set; }
        [DataMember] public string NDOC { get; set; }
        [DataMember] public string CRET { get; set; }
        [DataMember] public DateTime? DRES { get; set; }
        [DataMember] public DateTime? EXIGIBILITE { get; set; }
        [DataMember] public string NUMEROIDCLIENT { get; set; }
        [DataMember] public int? TOTALFACUTREDUE { get; set; }
        [DataMember] public string TOURNEE { get; set; }
        [DataMember] public string CODESITE { get; set; }
        [DataMember] public string LIBELLESITE { get; set; }
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public string ORDTOUR { get; set; }
        [DataMember] public int FK_IDTOURNEE { get; set; }
        [DataMember] public string RUE { get; set; }
        [DataMember] public string PORTE { get; set; }
        [DataMember] public int FK_IDPROPRIETAIRE { get; set; }
        [DataMember] public Nullable<int> FK_IDPRODUIT { get; set; }
        [DataMember] public Nullable<int> FK_IDABON{ get; set; }

        [DataMember] public string LIBELLEMODEPAIEMENT { get; set; }
        [DataMember] public string LIBELLECODECONSO { get; set; }
        [DataMember] public string LIBELLECATEGORIE { get; set; }
        [DataMember] public string LIBELLERELANCE { get; set; }
        [DataMember] public string LIBELLENATIONALITE { get; set; }
        [DataMember] public string LIBELLENATURECLIENT { get; set; }
        [DataMember] public string LIBELLEPAYEUR { get; set; }
        [DataMember] public string LIBELLETYPEPIECE { get; set; }
        [DataMember] public string LIBELLEUSAGE { get; set; }
        [DataMember] public string LIBELLEREGCLI { get; set; }
        [DataMember] public string TYPEDEMANDE { get; set; }
        [DataMember] public string LIBELLEDENOMINATION { get; set; }

        [DataMember] public Nullable<int> FK_IDCOMMUNE { get; set; }
        [DataMember] public Nullable<int> FK_IDQUARTIER { get; set; }
        [DataMember] public Nullable<int> FK_IDRUE { get; set; }
        [DataMember] public Nullable<int> FK_IDSECTEUR { get; set; }

        [DataMember] public string PERIODE { get; set; }
        [DataMember] public bool ISMODIFIER { get; set; }


    }
}









