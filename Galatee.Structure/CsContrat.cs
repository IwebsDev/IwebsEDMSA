using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.ServiceModel;
using System.Runtime.Serialization;


namespace Galatee.Structure
{
    [DataContract]
    public class CsContrat:CsPrint 
    {
        [DataMember] public string AGENCE { get; set; }
        [DataMember] public string NUMDEMANDE { get; set; }
        [DataMember] public string LIBELLEPRODUIT { get; set; }

        [DataMember] public string NOMPROPRIETAIRE { get; set; }
        [DataMember] public string QUARTIER { get; set; }
        [DataMember] public string RUE { get; set; }
        [DataMember] public string COMMUNE { get; set; }
        [DataMember] public string LOT { get; set; }
        [DataMember] public string PORTE { get; set; }
        [DataMember] public string USAGE { get; set; }
        [DataMember] public string CODEGEOGRAPHIQUE { get; set; }

        [DataMember] public string CENTRE { get; set; }
        [DataMember] public string CLIENT { get; set; }
        [DataMember] public string ORDRE { get; set; }
        [DataMember] public string NOMCLIENT { get; set; }
        [DataMember] public string QUARTIERCLIENT { get; set; }
        [DataMember] public string TELEPHONE { get; set; }
        [DataMember] public string RUECLIENT { get; set; }
        [DataMember] public string PORTECLIENT { get; set; }
        [DataMember] public string BOITEPOSTALE { get; set; }
        [DataMember] public string TYPEDEPIECE { get; set; }
        [DataMember] public string NUMEROPIECE { get; set; }

        [DataMember] public string TAXE { get; set; }
        [DataMember] public string  TAUXTAXE { get; set; }
        [DataMember] public string  MONTANTDEVIS { get; set; }
        [DataMember] public string  MONTANTPARTICAPATION { get; set; }
        [DataMember] public string  TOTAL1 { get; set; }
        [DataMember] public string  AVANCE { get; set; }
        [DataMember] public string  FRAISPOLICE { get; set; }
        [DataMember] public string  FRAISTIMBRE { get; set; }
        [DataMember] public string  TOTAL2 { get; set; }
        [DataMember] public string  TOTALGENERAL { get; set; }

        [DataMember] public string  TYPECOMPTEUR { get; set; }
        [DataMember] public string  NUMEROCOMPTEUR { get; set; }
        [DataMember] public string  ANNEE { get; set; }
        [DataMember] public string  CALIBRE { get; set; }
        [DataMember] public string  NBREFILS { get; set; }

        [DataMember] public string  NATUREBRANCHEMENT { get; set; }
        [DataMember] public string  LONGUEURBRANCHEMENT { get; set; }
        [DataMember] public string  CODETARIF { get; set; }
        [DataMember] public string  CODCONSOMATEUR { get; set; }
        [DataMember] public string  CATEGORIE { get; set; }
        [DataMember] public string  PUISSANCESOUSCRITE { get; set; }
        [DataMember] public string  REGLAGEDISJONCTEUR { get; set; }

        [DataMember] public string  APPAREIL { get; set; }
        [DataMember] public int  QUANTITE { get; set; }

        [DataMember] public string  REDEVANCE { get; set; }
        [DataMember] public string  TRANCHE { get; set; }
        [DataMember] public decimal   MONTANT { get; set; }

        [DataMember] public string  RUBRIQUE { get; set; }
        [DataMember] public decimal   MONTANTRUBIQE { get; set; }

        [DataMember] public string  OPTIONEDITION { get; set; }
        [DataMember] public string  AGENTEDM { get; set; }
        [DataMember] public string  MONTANTAVANCE { get; set; }
        [DataMember] public string  CHAMPVIDE { get; set; }
        [DataMember] public string  REGROUPEMENT { get; set; }



    }
}









