using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsInformationsDevis
    {
        [DataMember]
        public ObjDEVIS Devis { get; set; }
        [DataMember]
        public ObjDEMANDEDEVIS DemandeDevis { get; set; }
        [DataMember]
        public ObjDEPOSIT Deposit { get; set; }
        [DataMember]
        public List<ObjAPPAREILSDEVIS> ListAppareilsDevis { get; set; }
        [DataMember]
        public ObjDOCUMENTSCANNE DocumentAutorisation { get; set; }
        [DataMember]
        public ObjDOCUMENTSCANNE DocumentPreuvePropriete { get; set; }
        [DataMember]
        public ObjDOCUMENTSCANNE DocumentSchema { get; set; }
        [DataMember]
        public ObjDOCUMENTSCANNE DocumentManuscrit{ get; set; }
        [DataMember]
        public List<ObjELEMENTDEVIS> ListElementsDevis { get; set; }
        [DataMember]
        public List<ObjELEMENTDEVIS> ListDetailsElementsDevis { get; set; }
        [DataMember]
        public List<ObjELEMENTDEVIS> ListElementsDevisForValidationRemiseStock { get; set; }
        [DataMember]
        public List<ObjTRAVAUXDEVIS> ListTravauxDevis { get; set; }
        [DataMember]
        public ObjETAPEDEVIS EtapeDevisSuivante { get; set; }
        [DataMember]
        public ObjETAPEDEVIS EtapeCourante { get; set; }
        [DataMember]
        public ObjETAPEDEVIS EtapeRejet { get; set; }
        [DataMember]
        public ObjETAPEDEVIS EtapeIntermediaire { get; set; }
        [DataMember]
        public ObjTRAVAUXDEVIS TravauxDevis { get; set; }
        [DataMember]
        public CsControleTravaux ControleTravaux { get; set; }
        [DataMember]
        public Decimal? AmountPaid { get; set; }
        [DataMember]
        public CsUtilisateur ChefEquipe { get; set; }
    }
}
