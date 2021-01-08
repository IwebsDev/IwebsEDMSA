using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class CsLotri
    {

       [DataMember] public string NUMLOTRI { get; set; }
       [DataMember] public string JET { get; set; }
       [DataMember] public string PERIODE { get; set; }
       [DataMember] public string CENTRE { get; set; }
       [DataMember] public string PRODUIT { get; set; }
       [DataMember] public string CATEGORIECLIENT { get; set; }
       [DataMember] public string PERIODICITE { get; set; }
       [DataMember] public Nullable<int> EXIG { get; set; }
       [DataMember] public Nullable<System.DateTime> DFAC { get; set; }
       [DataMember] public string ETATFAC1 { get; set; }
       [DataMember] public string ETATFAC2 { get; set; }
       [DataMember] public string ETATFAC3 { get; set; }
       [DataMember] public string ETATFAC4 { get; set; }
       [DataMember] public string ETATFAC5 { get; set; }
       [DataMember] public string ETATFAC6 { get; set; }
       [DataMember] public string ETATFAC7 { get; set; }
       [DataMember] public string ETATFAC8 { get; set; }
       [DataMember] public string ETATFAC9 { get; set; }
       [DataMember] public string ETATFAC10 { get; set; }
       [DataMember] public string TOURNEE { get; set; }
       [DataMember] public string RELEVEUR { get; set; }
       [DataMember] public string BASE { get; set; }
       [DataMember] public int PK_ID { get; set; }
       [DataMember] public System.DateTime DATECREATION { get; set; }
       [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
       [DataMember] public string USERCREATION { get; set; }
       [DataMember] public string USERMODIFICATION { get; set; }
       [DataMember] public Nullable<int> FK_IDPRODUIT { get; set; }
       [DataMember] public Nullable<int> FK_IDCATEGORIECLIENT { get; set; }
       [DataMember] public Nullable<int> FK_IDRELEVEUR { get; set; }
       [DataMember] public Nullable<int> FK_IDTOURNEE { get; set; }
       [DataMember] public int FK_IDCENTRE { get; set; }
       [DataMember] public string UserCalcul { get; set; }




/* Autre*/
        
        [DataMember] public Nullable<int> FK_IDPERIODICITE { get; set; }
        [DataMember] public bool  LETTRAGE { get; set; }
        [DataMember] public bool  MISEAJOUR { get; set; }
        [DataMember] public string   MOISCOMPTA { get; set; }
        [DataMember] public bool  IsSelect { get; set; }
        [DataMember] public bool IsDejaFacturer { get; set; }
        [DataMember] public string STATUS { get; set; }
        [DataMember] public string NOMUSER { get; set; }
        [DataMember] public string LIBELLECENTRE { get; set; }
        [DataMember] public DateTime ? DATEEXIG { get; set; }
        [DataMember] public string MATRICULE { get; set; }
        [DataMember] public string CODE { get; set; }
        [DataMember] public string LIBELLE { get; set; }
        [DataMember] public string IDOBJET { get; set; }
        [DataMember] public string FACTURE { get; set; }
        [DataMember] public string TOPMAJ { get; set; }

    }

}









