using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class aCritere
    {
        private DateTime _exig;
        private string _client;
        private string _mois;
        private string _origine;
        private int _nbre;
        private int _idcentre;
        private string _numero;
        public aCritere()
        {

        }
        [DataMember]
        public int idcentre
        {
            get { return _idcentre; }
            set { _idcentre = value; }
        }
        [DataMember]
        public DateTime Exigibilite
        {
            get { return _exig; }
            set { _exig = value; }
        }

        [DataMember]
        public string Client
        {
            get { return _client; }
            set { _client = value; }
        }
        [DataMember]
        public string MoisReference
        {
            get { return _mois; }
            set { _mois = value; }
        }
        [DataMember]
        public int OccurrenceImpayes
        {
            get { return _nbre; }
            set { _nbre = value; }
        }
        [DataMember]
        public string Origine
        {
            get { return _origine; }
            set { _origine = value; }
        }

        [DataMember]
        public string Campagne
        {
            get { return _numero; }
            set { _numero = value; }
        }

        private string _tourneeDebut;

        [DataMember]
        public string TourneeDebut
        {
            get { return _tourneeDebut; }
            set { _tourneeDebut = value; }
        }
        [DataMember]
        private string _tourneeFin;

        public string TourneeFin
        {
            get { return _tourneeFin; }
            set { _tourneeFin = value; }
        }
        private string _ordreTourneeDebut;
        [DataMember]
        public string OrdreTourneeDebut
        {
            get { return _ordreTourneeDebut; }
            set { _ordreTourneeDebut = value; }
        }
        private string _ordreTourneeFin;
        [DataMember]
        public string OrdreTourneeFin
        {
            get { return _ordreTourneeFin; }
            set { _ordreTourneeFin = value; }
        }
    }
}
