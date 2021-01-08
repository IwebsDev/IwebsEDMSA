using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Galatee.Silverlight.Rpnt.ViewsModels
{
    public class CsConsultationClientBTA: INotifyPropertyChanged
    {

        private string _CLIENT_REFCLIENT;
        public string CLIENT_REFCLIEN
        {
            get { return _CLIENT_REFCLIENT; }
            set
            {
                _CLIENT_REFCLIENT = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CLIENT_REFCLIENT"));
            }
        }

        private string _CLIENT_NOM;
        public string CLIENT_NOM
        {
            get { return _CLIENT_NOM; }
            set
            {
                _CLIENT_NOM = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CLIENT_NOM"));
            }
        }

        private string _CLIENT_PRENOMS;
        public string CLIENT_PRENOMS
        {
            get { return _CLIENT_PRENOMS; }
            set
            {
                _CLIENT_PRENOMS = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CLIENT_PRENOMS"));
            }
        }

        private string _CONTRAT_STATUTCONTRAT;
        public string CONTRAT_STATUTCONTRAT
        {
            get { return _CONTRAT_STATUTCONTRAT; }
            set
            {
                _CONTRAT_STATUTCONTRAT = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONTRAT_STATUTCONTRAT"));
            }
        }

        private string _CONTRAT_TYPE;
        public string CONTRAT_TYPE
        {
            get { return _CONTRAT_TYPE; }
            set
            {
                _CONTRAT_TYPE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONTRAT_TYPE"));
            }
        }  
        
        private string _CONTRAT_DATEDERNIERSTATUT;
        public string CONTRAT_DATEDERNIERSTATUT
        {
            get { return _CONTRAT_DATEDERNIERSTATUT; }
            set
            {
                _CONTRAT_DATEDERNIERSTATUT = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONTRAT_DATEDERNIERSTATUT"));
            }
        }

        private string _CONTRAT_PUISSANCESOUSCRITE;
        public string CONTRAT_PUISSANCESOUSCRITE
        {
            get { return _CONTRAT_PUISSANCESOUSCRITE; }
            set
            {
                _CONTRAT_PUISSANCESOUSCRITE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONTRAT_PUISSANCESOUSCRITE"));
            }
        }

        private string _BRT_REF;
        public string BRT_REF
        {
            get { return _BRT_REF; }
            set
            {
                _BRT_REF = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BRT_REF"));
            }
        }

        private string _BRT_COMPTEUR;
        public string BRT_COMPTEUR
        {
            get { return _BRT_COMPTEUR; }
            set
            {
                _BRT_COMPTEUR = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BRT_COMPTEUR"));
            }
        }

        private string _MTH_LIBELLE;
        public string MTH_LIBELLE
        {
            get { return _MTH_LIBELLE; }
            set
            {
                _MTH_LIBELLE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MTH_LIBELLE"));
            }
        }

        private string _MTH_PERIODE;
        public string MTH_PERIODE
        {
            get { return _MTH_PERIODE; }
            set
            {
                _MTH_PERIODE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MTH_PERIODE"));
            }
        }

        private string _CONSO_CHUTE;
        public string CONSO_CHUTE
        {
            get { return _CONSO_CHUTE; }
            set
            {
                _CONSO_CHUTE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONSO_CHUTE"));
            }
        }

        private string _CONSO_PRECEDENTE;
        public string CONSO_PRECEDENTE
        {
            get { return _CONSO_PRECEDENTE; }
            set
            {
                _CONSO_PRECEDENTE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONSO_PRECEDENTE"));
            }
        }

        private string _CONSO_PERIODE_CHUTE;
        public string CONSO_PERIODE_CHUTE
        {
            get { return _CONSO_PERIODE_CHUTE; }
            set
            {
                _CONSO_PERIODE_CHUTE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONSO_PERIODE_CHUTE"));
            }
        }

        private string _CONSO_PERIODE_PRECEDENTE;
        public string CONSO_PERIODE_PRECEDENTE
        {
            get { return _CONSO_PERIODE_PRECEDENTE; }
            set
            {
                _CONSO_PERIODE_PRECEDENTE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONSO_PERIODE_PRECEDENTE"));
            }
        }

        private string _CONSO_DIFFERENECE;
        public string CONSO_DIFFERENECE
        {
            get { return _CONSO_DIFFERENECE; }
            set
            {
                _CONSO_DIFFERENECE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CONSO_DIFFERENECE"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
