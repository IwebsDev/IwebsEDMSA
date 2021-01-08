//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Galatee.Entity.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbCampagnesControleBTA
    {
        public tbCampagnesControleBTA()
        {
            this.tbLotsDeControleBTA = new HashSet<tbLotsDeControleBTA>();
        }
    
        public System.Guid Campagne_ID { get; set; }
        public string Libelle_Campagne { get; set; }
        public string CodeExploitation { get; set; }
        public int Statut_ID { get; set; }
        public string MatriculeAgentCreation { get; set; }
        public System.DateTime DateCreation { get; set; }
        public int NbreElements { get; set; }
        public string MatriculeAgentDerniereModification { get; set; }
        public Nullable<System.DateTime> DateModification { get; set; }
        public System.DateTime DateDebutControles { get; set; }
        public System.DateTime DateFinPrevue { get; set; }
        public Nullable<int> FK_IDCENTRE { get; set; }
        public Nullable<int> Methode_ID { get; set; }
        public Nullable<int> Critere_TypeClient { get; set; }
        public Nullable<int> Critere_GroupeDeFacturation { get; set; }
        public Nullable<int> Critere_TypeTarif { get; set; }
        public Nullable<int> Critere_TypeCompteur { get; set; }
        public Nullable<int> Critere_AgentZone { get; set; }
        public Nullable<int> Critere_Periode { get; set; }
    
        public virtual RefMethodesDeDetectionClientsBTA RefMethodesDeDetectionClientsBTA { get; set; }
        public virtual ICollection<tbLotsDeControleBTA> tbLotsDeControleBTA { get; set; }
    }
}
