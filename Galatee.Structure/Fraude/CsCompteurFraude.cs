using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
     [DataContract]
   public class CsCompteurFraude 
    {

         [DataMember] public int PK_ID { get; set; }
         [DataMember] public string NumeroCompteur { get; set; }
         [DataMember] public Nullable<int> IndexCompteur { get; set; }
         [DataMember] public string RefPlombCompteur { get; set; }
         [DataMember] public string NumeroPince { get; set; }
         [DataMember] public string CertificatPlombage { get; set; }
         [DataMember] public string RefPlombCacheBorne { get; set; }
         [DataMember] public string RefPlombCoffretFusible { get; set; }
         [DataMember] public string RefPlombCoffretSecurite { get; set; }
         [DataMember] public string Bordereau { get; set; }
         [DataMember] public bool IsEcartIndex { get; set; }
         [DataMember] public Nullable<int> FK_IDCLIENTFRAUDE { get; set; }
         [DataMember] public Nullable<int> FK_IDCONTROLE { get; set; }
        [DataMember] public Nullable<int> FK_IDPRODUIT { get; set; }
        [DataMember] public Nullable<int> FK_IDMARQUECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDTYPECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDANOMALIECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDANOMALIECACHEBORNE { get; set; }
        [DataMember] public Nullable<int> FK_IDANOMALIEBRANCHEMENT { get; set; }
        [DataMember] public Nullable<int> FK_IDPHASECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDCALIBRECOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDREGLAGE { get; set; }
        [DataMember]  public Nullable<int> FK_IDACTIONSURCOMPTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDUSAGEPRODUIT { get; set; }
        [DataMember] public Nullable<int> FK_IDTYPEDISJONCTEUR { get; set; }
        [DataMember] public Nullable<int> FK_IDMARQUEDISJONCTEUR { get; set; }
        [DataMember] public string PRODUIT { get; set; }
        [DataMember] public string TYPECOMPTEUR { get; set; }
        [DataMember] public string MARQUE { get; set; }
        [DataMember] public string USAGEPRODUIT { get; set; }

        [DataMember] public string libelle_Produit { get; set; }
        [DataMember] public string libelle_usage { get; set; }
        [DataMember] public string libelle_AnnomalieBranchement{ get; set; }
        [DataMember] public string libelle_AnnomalieCompteur{ get; set; }
        [DataMember] public string libelle_AutreAnnomalie{ get; set; }
        [DataMember] public string libelle_Calibre{ get; set; }


         
    }
}
