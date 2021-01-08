using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure 
{
    [DataContract]
    public class CStruct
    {
        private int? _id;
        private string _libelle;

        public CStruct()
        {
        }

        [DataMember]
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }
        [DataMember]
        public string Libelle
        {
            get { return _libelle; }
            set { _libelle = value; }
        }

        public override string ToString()
        {
            return this.Libelle;
        }

    }

    [DataContract]
    public class aCategory
    {
        private string _code;
        private string _libelle;

        public aCategory()
        {
        }
        [DataMember]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        [DataMember]
        public string Libelle
        {
            get { return _libelle; }
            set { _libelle = value; }
        }

        public override string ToString()
        {
            return this.Libelle;
        }

    }

    [DataContract]
    public class aRepartition
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int IdCategory { get; set; }
        [DataMember]
        public int IdProduit { get; set; }
        [DataMember]
        public string Libelle { get; set; }
        [DataMember]
        public string LibProduit { get; set; }
        [DataMember]
        public int Annee { get; set; }
        [DataMember]
        public int Mois { get; set; }
        [DataMember]
        public string Produit { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string LibCategory { get; set; }
        [DataMember]
        public int Nombre { get; set; }
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public bool? IsCreated { get; set; }


        [DataMember]
        public int NombreJanvier { get; set; }
        [DataMember]
        public int NombreFevrier { get; set; }
        [DataMember]
        public int NombreMars { get; set; }
        [DataMember]
        public int NombreAvril { get; set; }
        [DataMember]
        public int NombreMai { get; set; }
        [DataMember]
        public int NombreJuin { get; set; }
        [DataMember]
        public int NombreJuillet { get; set; }
        [DataMember]
        public int NombreAout { get; set; }
        [DataMember]
        public int NombreSeptembre { get; set; }
        [DataMember]
        public int NombreOctobre { get; set; }
        [DataMember]
        public int NombreNovembre { get; set; }
        [DataMember]
        public int NombreDecembre { get; set; }

        //public aRepartition()
        //{
        //    Id = 0;
        //    Libelle = string.Empty;
        //    LibProduit = string.Empty;
        //    Annee = string.Empty;
        //    Mois = string.Empty;
        //    Produit = string.Empty;
        //    Category = string.Empty;
        //    LibCategory = string.Empty;
        //    Nombre = 0;
        //    Total = 0;
        //}
    }

    [DataContract]
    public class aMoisReference
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Libelle { get; set; }

        //public aMoisReference()
        //{
        //    Code = string.Empty;
        //    Libelle = string.Empty;
        //}
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
