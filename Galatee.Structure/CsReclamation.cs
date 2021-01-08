using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Galatee.Structure
{
    [DataContract]
    public class CsReclamation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs (property));
        }

         [DataMember]
        public System.Guid PK_ID { get; set; }
         [DataMember]
        public int ORDRERECLAMATION { get; set; }
         [DataMember]
        public string NUMEROFACTURE { get; set; }
         [DataMember]
        public string NUMERORECLAMATION { get; set; }
         [DataMember]
        public int IDTYPERECLAMATION { get; set; }
         [DataMember]
        public string NUMEROCOMPTEUR { get; set; }
         [DataMember]
        public string ZONE { get; set; }
         [DataMember]
        public string IDCENTRE { get; set; }
         [DataMember]
        public string NOMPROPRIETAIRE { get; set; }
         [DataMember]
        public string NOMCLIENT { get; set; }
         [DataMember]
        public string ORDRE { get; set; }
         [DataMember]
        public string IDCLIENT { get; set; }
         [DataMember]
        public System.DateTime DATEOUVERTURE { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATEVERIFICATIONBILLING { get; set; }
         [DataMember]
        public string AGENTEMETTEUR { get; set; }
         [DataMember]
        public string AGENTSERVICECONCERNE { get; set; }
         [DataMember]
        public string AGENTSUPERVISOR { get; set; }
         [DataMember]
        public string AGENTBILLING { get; set; }
         [DataMember]
        public string AGENTVALIDATION { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATESUPERVISORCHECKING { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATETRANSMISSIONBILLING { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATEMANAGERCHECKING { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATETRANSMISSIONMANAGER { get; set; }
         [DataMember]
        public string OBSERVATIONBILLING { get; set; }
         [DataMember]
        public string OBSERVATIONSERVICETECH { get; set; }
         [DataMember]
        public string ADRESSE { get; set; }
         [DataMember]
        public string EMAIL { get; set; }
         [DataMember]
        public string NUMEROTELEPHONEPORTABLE { get; set; }
         [DataMember]
        public string NUMEROTELEPHONEFIXE { get; set; }
         [DataMember]
        public string OBJETRECLAMATION { get; set; }
         [DataMember]
        public string ACTIONMENEES { get; set; }
         [DataMember]
        public Nullable<int> IDVALIDATION { get; set; }
         [DataMember]
        public int IDETAPERECLAMATION { get; set; }
         [DataMember]
        public int IDMODERECEPTION { get; set; }
         [DataMember]
        public int IDSTATUTRECLAMATION { get; set; }
         [DataMember]
        public string MOTIFREPRISE { get; set; }
         [DataMember]
        public Nullable<bool> NONCONFORMITE { get; set; }
         [DataMember]
        public string LETTREREPONSE { get; set; }
         [DataMember]
        public Nullable<System.DateTime> DATEVALIDATION { get; set; }
         [DataMember]
        public string MOTIFREJETMANAGER { get; set; }
         [DataMember]
        public string MOTIFREJETBILLING { get; set; }
         [DataMember]
        public string IDSITE { get; set; }
         [DataMember]
        public string CASHPOWER { get; set; }
         [DataMember]
        public string BRANCH { get; set; }
         [DataMember]
        public string TYPEELECTRICITE { get; set; }
         [DataMember]
        public string TYPEEAU { get; set; }
         [DataMember]
        public string IDPRODUIT { get; set; }
         [DataMember]
        public string NAMEOFBO { get; set; }
         [DataMember]
        public string ACCURATEREADINGELECTRICITY { get; set; }
         [DataMember]
        public string ACCURATEREADINGWATER { get; set; }
         [DataMember]
        public string WRONGREADINGELECTRICITY { get; set; }
         [DataMember]
        public string WRONGREADINGWATER { get; set; }
         [DataMember]
        public Nullable<System.DateTime> WRONGREADINGWATERDATE { get; set; }
         [DataMember]
        public Nullable<System.DateTime> WRONGREADINGELECTRICITYDATE { get; set; }
         [DataMember]
        public Nullable<System.DateTime> ACCURATEREADINGELECTRICITYDATE { get; set; }
         [DataMember]
        public Nullable<System.DateTime> ACCURATEREADINGWATERDATE { get; set; }
         
        private string libelleTypeReclamation;
        private string libelleEtapeReclamation;
        private string libelleStatutReclamation;

         [DataMember]
         public string LibelleTypeReclamation
         {
             get { return libelleTypeReclamation; }
             set
             {
                 libelleTypeReclamation = value;
                 OnPropertyChanged("LibelleTypeReclamation");
             }
         }

         [DataMember]
         public string LibelleEtapeReclamation
         {
             get { return libelleEtapeReclamation; }
             set
             {
                 libelleEtapeReclamation = value;
                 OnPropertyChanged("LibelleEtapeReclamation");
             }
         }

         [DataMember]
         public string LibelleStatutReclamation
         {
             get { return libelleStatutReclamation; }
             set
             {
                 libelleStatutReclamation = value;
                 OnPropertyChanged("LibelleStatutReclamation");
             }
         }



        //private Guid   _Id;
        //private string _NumeroReclamation;
        //private string _IdCentre;
        //private string _IdClient;
        //private string _Adresse;
        //private string _Email;
        //private string _NumeroTelephonePortable;
        //private string _NumeroTelephoneFixe;
        //private int    _IdTypeReclamation;
        //private int    _IdEtapeReclamation;
        //private int    _IdStatutReclamation;
        //private string _LibelleCentre;
        //private string _NomPrenomsClient;
        //private string _LibelleTypeReclamation;
        //private string _LibelleStatutReclamation;
        //private string _LibelleEtapeReclamation;

        // [DataMember]
        //public Guid Id
        //{
        //    get { return _Id; }
        //    set { _Id = value; }
        //}

        // [DataMember]
        //public string NumeroReclamation
        //{
        //    get { return _NumeroReclamation; }
        //    set { _NumeroReclamation = value; }
        //}

        // [DataMember]
        //public string IdCentre
        //{
        //    get { return _IdCentre; }
        //    set { _IdCentre = value; }
        //}

        // [DataMember]
        //public string IdClient
        //{
        //    get { return _IdClient; }
        //    set { _IdClient = value; }
        //}

        // [DataMember]
        //public string Adresse
        //{
        //    get { return _Adresse; }
        //    set { _Adresse = value; }
        //}

        // [DataMember]
        //public string Email
        //{
        //    get { return _Email; }
        //    set { _Email = value; }
        //}

        // [DataMember]
        //public string NumeroTelephonePortable
        //{
        //    get { return _NumeroTelephonePortable; }
        //    set { _NumeroTelephonePortable = value; }
        //}

        // [DataMember]
        //public string NumeroTelephoneFixe
        //{
        //    get { return _NumeroTelephoneFixe; }
        //    set { _NumeroTelephoneFixe = value; }
        //}

        // [DataMember]
        //public int IdTypeReclamation
        //{
        //    get { return _IdTypeReclamation; }
        //    set { _IdTypeReclamation = value; }
        //}

        // [DataMember]
        //public int IdEtapeReclamation
        //{
        //    get { return _IdEtapeReclamation; }
        //    set { _IdEtapeReclamation = value; }
        //}

        // [DataMember]
        //public int IdStatutReclamation
        //{
        //    get { return _IdStatutReclamation; }
        //    set { _IdStatutReclamation = value; }
        //}

        // [DataMember]
        //public string LibelleCentre
        //{
        //    get { return _LibelleCentre; }
        //    set { _LibelleCentre = value; }
        //}

        // [DataMember]
        //public string NomPrenomsClient
        //{
        //    get { return _NomPrenomsClient; }
        //    set { _NomPrenomsClient = value; }
        //}

        // [DataMember]
        //public string LibelleTypeReclamation
        //{
        //    get { return _LibelleTypeReclamation; }
        //    set 
        //    { 
        //        _LibelleTypeReclamation = value;
        //        OnPropertyChanged("LibelleTypeReclamation");
        //    }
        //}

        // [DataMember]
        //public string LibelleEtapeReclamation
        //{
        //    get { return _LibelleEtapeReclamation; }
        //    set 
        //    { 
        //        _LibelleEtapeReclamation = value;
        //        OnPropertyChanged("LibelleEtapeReclamation");
        //    }
        //}

        // [DataMember]
        //public string LibelleStatutReclamation
        //{
        //    get { return _LibelleStatutReclamation; }
        //    set 
        //    { 
        //        _LibelleStatutReclamation = value;
        //        OnPropertyChanged("LibelleStatutReclamation");
        //    }
        //}

        ////ajout ola 02/03/2010
        // [DataMember]
        //public Byte OrdreReclamation { get; set; }
         
        //[DataMember]
        //public DateTime? DateOuverture { get; set; }

        // [DataMember]
        //public DateTime? DateVerificationBranchOfficer { get; set; }

        ////public DateTime? WaterReadingDate { get; set; }
        ////public DateTime? ElectricityReadingDate { get; set; }

        // [DataMember]
        //public DateTime? DateVerificationBilling { get; set; }

        // [DataMember]
        //public string NumeroFacture { get; set; }

        // [DataMember]
        //public string NumeroCompteur { get; set; }

        // [DataMember]
        //public string Zone { get; set; }

        // [DataMember]
        //public string AgentEmetteur { get; set; }

        // [DataMember]
        //public string AgentServiceConcerne { get; set; }

        // [DataMember]
        //public string AgentSupervisor { get; set; }

        // [DataMember]
        //public string AgentValidation { get; set; }

        // [DataMember]
        //public string AgentBilling { get; set; }

        // [DataMember]
        //public DateTime? DateSupervisorChecking { get; set; }

        // [DataMember]
        //public DateTime? DateTransmissionBilling { get; set; }

        // [DataMember]
        //public DateTime? DateTransmissionManager { get; set; }

        // [DataMember]
        //public DateTime? DateManagerChecking { get; set; }

        // [DataMember]
        //public DateTime? DateValidation { get; set; }

        // [DataMember]
        //public string ObservationBilling { get; set; }

        // [DataMember]
        //public string ObservationServiceTech { get; set; }

        // [DataMember]
        //public Byte IdModeReception { get; set; }

        // [DataMember]
        //public string ObjetReclamation { get; set; }

        // [DataMember]
        //public string ActionMenees { get; set; }

        // [DataMember]
        //public int IdValidation { get; set; }

        // [DataMember]
        //public string MotifReprise { get; set; }

        // [DataMember]
        //public bool NonConformite { get; set; }

        // [DataMember]
        //public string libelleModeReception { get; set; }

        // [DataMember]
        //public string LibelleValidation { get; set; }

        // [DataMember]
        //public string LibelleEtape { get; set; }

        // [DataMember]
        //public string LibelleTournee { get; set; }

        // [DataMember]
        //public string Code { get; set; }

        // [DataMember]
        //public string IdSite { get; set; }

        // [DataMember]
        //public string CashPower { get; set; }

        // [DataMember]
        //public string Branch { get; set; }

        // [DataMember]
        //public string TypeEau { get; set; }

        // [DataMember]
        //public string TypeElectricite { get; set; }

        // [DataMember]
        //public string IdProduit { get; set; }

        // [DataMember]
        //public string LettreReponse { get; set; }

        // [DataMember]
        //public string NameOfBO { get; set; }

        // [DataMember]
        //public string NomClient { get; set; }

        // [DataMember]
        //public string MotifRejetManager { get; set; }

        // [DataMember]
        //public string MotifRejetBilling { get; set; }

        // [DataMember]
        //public string Recepteur { get; set; }

        // [DataMember]
        //public string Structure { get; set; }

        // [DataMember]
        //public string NomAgentEmetteur { get; set; }

        // [DataMember]
        //public DateTime? DateDebut { get; set; }

        // [DataMember]
        //public DateTime? DateFin { get; set; }

        // [DataMember]
        //public int NombreReclamationParAgent { get; set; }

        // [DataMember]
        //public int NombreTotal { get; set; }

        // [DataMember]
        //public int DelaiTraitement { get; set; }

        // [DataMember]
        //public string LibelleTypeCentre { get; set; }

        // [DataMember]
        //public string LibelleFonction { get; set; }

        // [DataMember]
        //public string NomProprietaire { get; set; }

        // [DataMember]
        //public string AccurateReadingElectricity { get; set; }

        // [DataMember]
        //public string AccurateReadingWater { get; set; }

        // [DataMember]
        //public string WrongReadingElectricity { get; set; }

        // [DataMember]
        //public string WrongReadingWater { get; set; }

        // [DataMember]
        //public string Ordre { get; set; }

        // [DataMember]
        //public DateTime? WrongReadingWaterDate { get; set; }

        // [DataMember]
        //public DateTime? WrongReadingElectricityDate { get; set; }

        // [DataMember]
        //public DateTime? AccurateReadingElectricityDate { get; set; }

        // [DataMember]
        //public DateTime? AccurateReadingWaterDate { get; set; }
    }
}
