using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsConnexion : CsPrint
    {
       
        private string zone;
        [DataMember]
        public string Zone
        {
            get { return zone; }
            set { zone = value; }
        }
        
        private string conso;
        [DataMember]
        public string Conso
        {
            get { return conso; }
            set { conso = value; }
        }
       
        private int cumul;
        [DataMember]
        public int Cumul
        {
            get { return cumul; }
            set { cumul = value; }
        }
       
        private int water;
        [DataMember]
        public int Water
        {
            get { return water; }
            set { water = value; }
        }
     
        private int electricity;
        [DataMember]
        public int Electricity
        {
            get { return electricity; }
            set { electricity = value; }
        }
     
        private int combine;
        [DataMember]
        public int Combine
        {
            get { return combine; }
            set { combine = value; }
        }
        
        private int sewerage;
        [DataMember]
        public int Sewerage
        {
            get { return sewerage; }
            set { sewerage = value; }
        }
        
        private string periode;
        [DataMember]
        public string Periode
        {
            get { return periode; }
            set { periode = value; }
        }

        public int SetPeriode
        {
            //get { return periode; }
            set { periode = value.ToString(); }
        }

        private int Nummois;
        [DataMember]
        public int NumMois
        {
            get { return Nummois; }
            set { Nummois = value; }
        }

        private string mois;
        [DataMember]
        public string Mois
        {
            get { return mois; }
            set { mois = value; }
        }
       
        private int connection;
        [DataMember]
        public int Connection
        {
            get { return connection; }
            set { connection = value; }
        }
        
        private int connection01;
        [DataMember]
        public int Connection01
        {
            get { return connection01; }
            set { connection01 = value; }
        }
        
        private int connection02;
        [DataMember]
        public int Connection02
        {
            get { return connection02; }
            set { connection02 = value; }
        }
       
        private int connection03;
        [DataMember]
        public int Connection03
        {
            get { return connection03; }
            set { connection03 = value; }
        }
        
        private int connection04;
        [DataMember]
        public int Connection04
        {
            get { return connection04; }
            set { connection04 = value; }
        }
       
        private int connection05;
        [DataMember]
        public int Connection05
        {
            get { return connection05; }
            set { connection05 = value; }
        }
       
        private int connection06;
        [DataMember]
        public int Connection06
        {
            get { return connection06; }
            set { connection06 = value; }
        }
      
        private int connection07;
         [DataMember]
        public int Connection07
        {
            get { return connection07; }
            set { connection07 = value; }
        }
       
        private int connection08;
        [DataMember]
        public int Connection08
        {
            get { return connection08; }
            set { connection08 = value; }
        }
       
        private int connection09;
        [DataMember]
        public int Connection09
        {
            get { return connection09; }
            set { connection09 = value; }
        }
      
        private int connection10;
        [DataMember]
        public int Connection10
        {
            get { return connection10; }
            set { connection10 = value; }
        }
       
        private int connection11;
        [DataMember]
        public int Connection11
        {
            get { return connection11; }
            set { connection11 = value; }
        }
      
        private int connection12;
        [DataMember]
        public int Connection12
        {
            get { return connection12; }
            set { connection12 = value; }
        }
      
        private string diametre;
        [DataMember]
        public string Diametre
        {
            get { return diametre; }
            set { diametre = value; }
        }
       
        private string libelle;
        [DataMember]
        public string Libelle
        {
            get { return libelle; }
            set { libelle = value; }
        }
       

        private string centre;
         [DataMember]
        public string Centre
        {
            get { return centre; }
            set { centre = value; }
        }
        
        private string client;
        [DataMember]
        public string Client
        {
            get { return client; }
            set { client = value; }
        }
        
        private string ordre;
        [DataMember]
        public string Ordre
        {
            get { return ordre; }
            set { ordre = value; }
        }
        
        private string nom;
        [DataMember]
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        
        private string addresse;
        [DataMember]
        public string Addresse
        {
            get { return addresse; }
            set { addresse = value; }
        }
      
        private string categorie;
        [DataMember]
        public string Categorie
        {
            get { return categorie; }
            set { categorie = value; }
        }
        
        private DateTime? dateAbonnement;
        [DataMember]
        public DateTime? DateAbonnement
        {
            get { return dateAbonnement; }
            set { dateAbonnement = value; }
        }
        
        private DateTime? dateCreation;
        [DataMember]
        public DateTime? DateCreation
        {
            get { return dateCreation; }
            set { dateCreation = value; }
        }
        
        private DateTime? dateResiliation;
        [DataMember]
        public DateTime? DateResiliation
        {
            get { return dateResiliation; }
            set { dateResiliation = value; }
        }
        
        private int moisInt;
        [DataMember]
        public int MoisInt
        {
            get { return moisInt; }
            set { moisInt = value; }
        }
        
        private string annee;
        [DataMember]
        public string Annee
        {
            get { return annee; }
            set { annee = value; }
        }
        
        private string produit;
        [DataMember]
        public string Produit
        {
            get { return produit; }
            set { produit = value; }
        }
        
        private string tarif;
        [DataMember]
        public string Tarif
        {
            get { return tarif; }
            set { tarif = value; }
        }
    }
}
