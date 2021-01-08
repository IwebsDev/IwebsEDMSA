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
//using Galatee.Silverlight.ServiceRpnt;
using System.Collections.ObjectModel;

namespace Galatee.Silverlight.Rpnt.ViewsModels
{

    public class CsTBCAMPAGNECONTROLEBTA : INotifyPropertyChanged
    {

        private int? _METHODE_ID;
        public int? METHODE_ID
        {
            get { return _METHODE_ID; }
            set
            {
                _METHODE_ID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("METHODE_ID"));
            }
        }

        private Guid _CAMPAGNE_ID;
        public Guid CAMPAGNE_ID
        {
            get { return _CAMPAGNE_ID; }
            set
            {
                _CAMPAGNE_ID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CAMPAGNE_ID"));
            }
        }

        private String _LIBELLE_CAMPAGNE;
        public String LIBELLE_CAMPAGNE
        {
            get { return _LIBELLE_CAMPAGNE; }
            set
            {
                _LIBELLE_CAMPAGNE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LIBELLE_CAMPAGNE"));
            }
        }

        private String _CODEEXPLOITATION;
        public String CODEEXPLOITATION
        {
            get { return _CODEEXPLOITATION; }
            set
            {
                _CODEEXPLOITATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CODEEXPLOITATION"));
            }
        }

        private String _LIBELLECENTRE;
        public String LIBELLECENTRE
        {
            get { return _LIBELLECENTRE; }
            set
            {
                _LIBELLECENTRE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LIBELLECENTRE"));
            }
        }

        private String _CODECENTRE;
        public String CODECENTRE
        {
            get { return _CODECENTRE; }
            set
            {
                _CODECENTRE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CODECENTRE"));
            }
        }

        private String _LIBELLEEXPLOITATION;
        public String LIBELLEEXPLOITATION
        {
            get { return _LIBELLEEXPLOITATION; }
            set
            {
                _LIBELLEEXPLOITATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LIBELLEEXPLOITATION"));
            }
        }


        private Int32 _STATUT_ID;
        public Int32 STATUT_ID
        {
            get { return _STATUT_ID; }
            set
            {
                _STATUT_ID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("STATUT_ID"));
            }
        }


        private String _STATUT;
        public String STATUT
        {
            get { return _STATUT; }
            set
            {
                _STATUT = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("STATUT"));
            }
        }


        private String _MATRICULEAGENTCREATION;
        public String MATRICULEAGENTCREATION
        {
            get { return _MATRICULEAGENTCREATION; }
            set
            {
                _MATRICULEAGENTCREATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MATRICULEAGENTCREATION"));
            }
        }


        private DateTime _DATECREATION;
        public DateTime DATECREATION
        {
            get { return _DATECREATION; }
            set
            {
                _DATECREATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DATECREATION"));
            }
        }


        private Int32 _NBREELEMENTS;
        public Int32 NBREELEMENTS
        {
            get { return _NBREELEMENTS; }
            set
            {
                _NBREELEMENTS = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NBREELEMENTS"));
            }
        }


        private String _MATRICULEAGENTDERNIEREMODIFICATION;
        public String MATRICULEAGENTDERNIEREMODIFICATION
        {
            get { return _MATRICULEAGENTDERNIEREMODIFICATION; }
            set
            {
                _MATRICULEAGENTDERNIEREMODIFICATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("MATRICULEAGENTDERNIEREMODIFICATION"));
            }
        }


        private DateTime? _DATEMODIFICATION;
        public DateTime? DATEMODIFICATION
        {
            get { return _DATEMODIFICATION; }
            set
            {
                _DATEMODIFICATION = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DATEMODIFICATION"));
            }
        }


        private DateTime _DATEDEBUTCONTROLES;
        public DateTime DATEDEBUTCONTROLES
        {
            get { return _DATEDEBUTCONTROLES; }
            set
            {
                _DATEDEBUTCONTROLES = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DATEDEBUTCONTROLES"));
            }
        }


        private DateTime _DATEFINPREVUE;
        public DateTime DATEFINPREVUE
        {
            get { return _DATEFINPREVUE; }
            set
            {
                _DATEFINPREVUE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DATEFINPREVUE"));
            }
        }


        private int _NBRLOTS;
        public int NBRLOTS
        {
            get { return _NBRLOTS; }
            set
            {
                _NBRLOTS = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("NBRLOTS"));
            }
        }

        private Int32 _POULATIONNONAFFECTES;
        public Int32 POULATIONNONAFFECTES
        {
            get { return _POULATIONNONAFFECTES; }
            set
            {
                _POULATIONNONAFFECTES = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("POULATIONNONAFFECTES"));
            }
        }

        //private ObservableCollection<CsBrt> _LISTEBRANCHEMENT;
        //public ObservableCollection<CsBrt> LISTEBRANCHEMENT
        //{
        //    get { return _LISTEBRANCHEMENT; }
        //    set
        //    {
        //        _LISTEBRANCHEMENT = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("LISTEBRANCHEMENT"));
        //    }
        //}

        //private ObservableCollection<CsTBLOTDECONTROLEBTA> _LISTELOT;
        //public ObservableCollection<CsTBLOTDECONTROLEBTA> LISTELOT
        //{
        //    get { return _LISTELOT; }
        //    set
        //    {
        //        _LISTELOT = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("LISTELOT"));
        //    }
        //}


        //private CsREFMETHODEDEDETECTIONCLIENTSBTA _METHODE;
        //public CsREFMETHODEDEDETECTIONCLIENTSBTA METHODE
        //{
        //    get { return _METHODE; }
        //    set
        //    {
        //        _METHODE = value;
        //        if (PropertyChanged != null)
        //            PropertyChanged(this, new PropertyChangedEventArgs("METHODE"));
        //    }
        //}
        private string _PERIODE;
        public string PERIODE
        {
            get { return _PERIODE; }
            set
            {
                _PERIODE = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("PERIODE"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
