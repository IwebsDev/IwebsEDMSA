using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDetailCampagne:CsPrint 
    {

            [DataMember] public string IDCOUPURE { get; set; }
            [DataMember] public int PK_ID { get; set; }
            [DataMember] public string CENTRE { get; set; }
            [DataMember] public string CLIENT { get; set; }
            [DataMember] public string ORDRE { get; set; }
            [DataMember] public string REFEM { get; set; }
            [DataMember] public string NDOC { get; set; }
            [DataMember] public string COPER { get; set; }
            [DataMember] public Nullable<decimal> MONTANT { get; set; }
            [DataMember] public Nullable<System.DateTime> EXIGIBILITE { get; set; }
            [DataMember] public string TOURNEE { get; set; }
            [DataMember] public string ORDTOUR { get; set; }
            [DataMember] public string CATEGORIECLIENT { get; set; }
            [DataMember] public Nullable<decimal> SOLDEDUE { get; set; }
            [DataMember] public Nullable<int> NOMBREFACTURE { get; set; }
            [DataMember] public Nullable<decimal> SOLDECLIENT { get; set; }
            [DataMember] public Nullable<decimal> SOLDEFACTURE { get; set; }
            [DataMember] public string COMPTEUR { get; set; }
            [DataMember] public Nullable<bool> ISAUTORISER { get; set; }
            [DataMember] public string MOTIFAUTORISATION { get; set; }
            [DataMember] public Nullable<decimal> FRAIS { get; set; }
            [DataMember] public Nullable<bool> ISANNULATIONFRAIS { get; set; }
            [DataMember] public string MOTIFANNULATION { get; set; }
            [DataMember] public Nullable<System.DateTime> DATERDV { get; set; }
            [DataMember] public string USERCREATION { get; set; }
            [DataMember] public Nullable<System.DateTime> DATECREATION { get; set; }
            [DataMember] public Nullable<System.DateTime> DATEMODIFICATION { get; set; }
            [DataMember] public string USERMODIFICATION { get; set; }
            [DataMember] public int FK_IDLCLIENT { get; set; }
            [DataMember] public int FK_IDCENTRE { get; set; }
            [DataMember] public int FK_IDCLIENT { get; set; }
            [DataMember] public int FK_IDTOURNEE { get; set; }
            [DataMember] public int FK_IDCATEGORIECLIENT { get; set; }
            [DataMember] public int FK_IDCAMPAGNE { get; set; }
            [DataMember] public int RELANCE { get; set; }
        

            [DataMember]   public int? FK_IDOBSERVATION{ get; set; }
            [DataMember]   public int? FK_TYPECOUPURE{ get; set; }
            [DataMember]   public string LIBELLECOUPURE { get; set; }
            [DataMember]   public string LIBELLERELANCE { get; set; }
            [DataMember]   public string TYPECOUPURE { get; set; }
            [DataMember]   public decimal ? MONTANTFRAIS { get; set; }
            [DataMember]   public string MATRICULE { get; set; }
            [DataMember]   public int? INDEX { get; set; }
            [DataMember]   public System.DateTime ? DATECOUPURE { get; set; }
            [DataMember]   public int FK_IDLIBELLETOP{ get; set; }
            [DataMember]   public int FK_IDNATURE{ get; set; }
            [DataMember]   public int FK_IDCOPER{ get; set; }
            [DataMember]   public int FK_IDADMUTILISATEUR{ get; set; }
            [DataMember]   public string NOMABON { get; set; }
            [DataMember]   public string ADRESSE { get; set; }
            [DataMember]   public string OBSERVATION { get; set; }
            [DataMember]   public System.DateTime? DATERENDEZVOUS { get; set; }
            [DataMember]   public string RUE { get; set; }
            [DataMember]   public string PORTE { get; set; }
            [DataMember]   public string QUARTIER { get; set; }
            [DataMember]   public string DATERDVCLIENT { get; set; }
            [DataMember]   public string LIBELLEMOTIF { get; set; }
            [DataMember]   public string LIBELLEBANQUE { get; set; }
            [DataMember]   public string NUMCHEQ { get; set; }
            [DataMember]   public string LIBELLECENTRE { get; set; }
            [DataMember]   public Nullable<bool>  ISNONENCAISSABLE { get; set; }
            [DataMember]   public string NOMAGENT { get; set; }
            [DataMember]   public string DEBUTORDTOURNEE { get; set; }
            [DataMember]   public string FINORDTOURNEE { get; set; }
            [DataMember]   public string DEBUTTOURNEE { get; set; }
            [DataMember]   public string FINTOURNEE { get; set; }

            [DataMember]   public decimal  MONTANTEREGLE { get; set; }
            [DataMember]   public int NOMBREFACTUREREGLE { get; set; }
            [DataMember]   public System.DateTime ? DATEREGLEMENT { get; set; }

            [DataMember]   public int NOMBREAVISEMIS { get; set; }
            [DataMember]   public int NOMBREAVISCOUPE { get; set; }
            [DataMember]   public decimal  POURCENTAGE { get; set; }
            [DataMember]   public Nullable<int>  FK_IDTYPECOUPURE { get; set; }
            [DataMember]   public int? INDEXCOMPTEUR { get; set; }

    
        
            [DataMember]   public bool  IsSelect { get; set; }
            [DataMember]   public bool  IsFraisDejaSaisie { get; set; }



        


        



    }

}









