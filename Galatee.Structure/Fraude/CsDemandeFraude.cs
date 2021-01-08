using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
     [DataContract]
    public class CsDemandeFraude
    {
        #region  collection object
        [DataMember] public List<CsAppareilRecenseFrd> AppareilRecenseFrd { get; set; }
        [DataMember] public List<CsAppareilUtiliserFrd> AppareilUtiliserFrd { get; set; }
        [DataMember] public List<CsDetailParPresentationEdm> DetailParPresentationEdm { get; set; }
        [DataMember] public List<CsDETAILparPRESTATIONREMBOURSABLE> DETAILparPRESTATIONREMBOURSABLE { get; set; }
        [DataMember] public List<CsDETAILparTRANCHE> DETAILparTRANCHE { get; set; }
        [DataMember] public List<CsDETAILparREGULARISATION> DETAILparREGULARISATION { get; set; }
        [DataMember] public List<CsMoisDejaFactures> MoisDejaFactures { get; set; }
        [DataMember] public List<CsPrestastionEdm> PrestastionEdm { get; set; }
        [DataMember] public List<CsPrestationRemboursable> PrestationRemboursable { get; set; }
        [DataMember] public List<CsRegularisation> Regularisation { get; set; }
        [DataMember] public List<CsTrancheFraude> TrancheFraude { get; set; }
        [DataMember] public List<CsEditionFactureFd > factureFraudeEdition { get; set; }
       // [DataMember] public List<CsConsommationFrd> ConsommationFrd { get; set; }
        #endregion
        #region Object
        [DataMember] public CsFraude Fraude { get; set; } 
        [DataMember] public CsClientFraude ClientFraude { get; set; }
        [DataMember] public CsDenonciateur Denonciateur { get; set; }
        [DataMember] public CsControle Controle { get; set; }
        [DataMember] public CsControleur Controleur { get; set; }
        [DataMember] public CsCompteurFraude CompteurFraude { get; set; }
        [DataMember] public CsAuditionFraude AuditionFraude { get; set; }
        [DataMember] public CsDemandeBase LaDemande { get; set; }
        [DataMember] public CsCtax Ctax { get; set; }
        [DataMember] public CsProduit Produit { get; set; }
        [DataMember] public CsCalibreCompteur CalibreCompteur { get; set; }
        [DataMember] public CsConsommationFrd ConsommationFrd { get; set; }
        [DataMember] public CsAppareilUtiliserFrd sAppareilUtiliserFrd { get; set; }
        [DataMember] public CsFactureFraude FactureFraude { get; set; }
        [DataMember] public CsCoper Coper { get; set; }
        [DataMember] public CsAg Ag { get; set; }
        [DataMember]  public CsCanalisation Canalisation { get; set; }
        [DataMember] public CsAbon Abon { get; set; }


        #endregion
    }
}
