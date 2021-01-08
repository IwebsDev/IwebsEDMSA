using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;
using System.Linq;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDemande
    {
        #region  collection object
        [DataMember] public List<CsEvenement> LstEvenement { get; set; }
        [DataMember] public List<CsLclient> LstFacture { get; set; }
        [DataMember] public List<CsCanalisation> LstCanalistion { get; set; }
        [DataMember] public List<CsDemandeDetailCout> LstCoutDemande { get; set; }
        [DataMember] public List<CsFraixParticipation> LstFraixParticipation { get; set; }
        [DataMember] public List<CsOrganeScelleDemande> LstOrganeScelleDemande { get; set; }
        [DataMember] public List<CsElementAchatTimbre> LstEltTimbre { get; set; }
        [DataMember]  public List<ObjAPPAREILSDEVIS> AppareilDevis { get; set; }
        [DataMember]  public List<ObjELEMENTDEVIS> EltDevis { get; set; }
        [DataMember]  public List<ObjDOCUMENTSCANNE> ObjetScanne { get; set; }
        [DataMember]  public List<CsControleTravaux> LstControleTvx { get; set; }
        [DataMember]  public List<CsCommentaireRejet > LstCommentaire { get; set; }
        [DataMember]  public List<CsAnnotation>   AnnotationDemande { get; set; }
        [DataMember] public List<CsLclient >  leCompteClient { get; set; }

        //[DataMember] public ObjDOCUMENTSCANNE LeDocumentScanne{ get; set; }
        #endregion
        #region Object
        [DataMember]  public CsAbon Abonne { get; set; }
        [DataMember] public CsPersonePhysique PersonePhysique { get; set; }
        [DataMember] public CsSocietePrive SocietePrives { get; set; }
        [DataMember] public CsAdministration_Institut AdministrationInstitut { get; set; }
        [DataMember] public CsInfoProprietaire InfoProprietaire_ { get; set; }
        [DataMember]  public CsBrt Branchement { get; set; }
        [DataMember]  public CsAg Ag { get; set; }
        [DataMember]  public CsClient LeClient { get; set; }
        [DataMember]  public CsDemandeBase LaDemande { get; set; }
        [DataMember]  public CsLotri LeLotri { get; set; }
        [DataMember]  public ObjTRAVAUXDEVIS TravauxDevis { get; set; }
        [DataMember]  public ObjDOCUMENTSCANNE LeDocumentScanne { get; set; }
        [DataMember]  public CsInfoDemandeWorkflow InfoDemande { get; set; }
        [DataMember]  public CsOrdreTravail  OrdreTravail { get; set; }
        [DataMember]  public ObjSUIVIDEVIS  SuivisDemande { get; set; }
        [DataMember]  public CsDtransfert  Transfert { get; set; }
        [DataMember]  public CsDepannage  Depannage { get; set; }
        [DataMember]  public CsProgarmmation   Programmation { get; set; }

        

        
        #endregion

    }

}









