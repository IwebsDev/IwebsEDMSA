using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Silverlight.Security
{
    public  class CsUtilisateur
    {
        public string CENTRE { get; set; }
        public string MATRICULE { get; set; }
        public string LIBELLE { get; set; }
        public string PASSE { get; set; }
        public string FONCTION { get; set; }
        public string CFONCT { get; set; }
        public string CODEHIER { get; set; }
        public Nullable<System.DateTime> DMAJ { get; set; }
        public string TRANS { get; set; }
        public string LOGINNAME { get; set; }
        public string E_MAIL { get; set; }
        public Nullable<System.DateTime> DATEDERNIEREMODIFICATION { get; set; }
        public Nullable<System.DateTime> DATEDEBUTVALIDITE { get; set; }
        public Nullable<System.DateTime> DATEFINVALIDITE { get; set; }
        public Nullable<int> STATUSCOMPTE { get; set; }
        public Nullable<System.DateTime> DATEDERNIEREMODIFICATIONPASSWORD { get; set; }
        public Nullable<bool> INITUSERPASSWORD { get; set; }
        public Nullable<int> NOMBREECHECSOUVERTURESESSION { get; set; }
        public Nullable<System.DateTime> DATEDERNIERECONNEXION { get; set; }
        public Nullable<bool> DERNIERECONNEXIONREUSSIE { get; set; }
        public Nullable<System.DateTime> DATEDERNIERVERROUILLAGE { get; set; }
        public string BRANCHE { get; set; }
        public byte PERIMETREACTION { get; set; }
        public Nullable<bool> ESTSUPPRIMER { get; set; }
        public string USERCREATION { get; set; }
        public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
        public string USERMODIFICATION { get; set; }
        public System.DateTime DATECREATION { get; set; }
        public int FK_IDFONCTION { get; set; }
        public int FK_IDCENTRE { get; set; }
        public int PK_ID { get; set; }
        public byte FK_IDSTATUS { get; set; }
        public string COMPTEWINDOW { get; set; }

        public string LIBELLESTATUSCOMPTE { get; set; }
        public string LIBELLEFONCTION { get; set; }
        public string LIBELLECENTRE { get; set; }
        public string LIBELLEPERIMETREACTION { get; set; }
        public bool IsSELECT { get; set; }
        public bool EsADMIN { get; set; }
        public string CENTREAFFICHER { get; set; }
        public string NOM { get; set; }
    }
}
