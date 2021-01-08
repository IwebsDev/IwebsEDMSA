using System;
using System.Collections.Generic;
using System.Text;
using Inova.Tools.Utilities;
using System.Runtime.Serialization;

namespace Galatee.Structure
{
    [DataContract]
    public class CsDemCout
    {
        [DataMember]
         public string   CENTRE { get; set; }
         [DataMember]
         public string   PRODUIT { get; set; }
         [DataMember]
         public string   TDEM { get; set; }
         [DataMember]
         public string   CATEGORIE { get; set; }
         [DataMember]
         public string   COPER1 { get; set; }
         [DataMember]
         public string NATURE1 { get; set; }
         [DataMember]
         public string   OBLI1 { get; set; }
         [DataMember]
         public string   AUTO1 { get; set; }
         [DataMember]
         public decimal ?   MONTANT1 { get; set; }
         [DataMember]
         public string TAXE1 { get; set; }
         [DataMember]
         public string   LIBELLE1 { get; set; }
         [DataMember]
         public string LIBELLETAXE { get; set; }
         [DataMember]
         public decimal? MONTANTTAXE { get; set; }

         [DataMember]
         public string     COPER2 { get; set; }
         [DataMember]
         public string NATURE2 { get; set; }
         [DataMember]
         public string     OBLI2 { get; set; }
         [DataMember]
         public string     AUTO2 { get; set; }
         [DataMember]
         public decimal ?     MONTANT2 { get; set; }
         [DataMember]
         public string     TAXE2 { get; set; }
         [DataMember]
         public string LIBELLE2 { get; set; }


         [DataMember]
         public string     COPER3 { get; set; }
         [DataMember]
         public string NATURE3 { get; set; }
         [DataMember]
         public string     OBLI3 { get; set; }
         [DataMember]
         public string     AUTO3 { get; set; }
         [DataMember]
         public decimal ?     MONTANT3 { get; set; }
         [DataMember]
         public string     TAXE3 { get; set; }
         [DataMember]
         public string LIBELLE3 { get; set; }

         [DataMember]
         public string     COPER4 { get; set; }
         [DataMember]
         public string NATURE4 { get; set; }
         [DataMember]
         public string     OBLI4 { get; set; }
         [DataMember]
         public string     AUTO4 { get; set; }
         [DataMember]
         public decimal ?  MONTANT4 { get; set; }
         [DataMember]
         public string     TAXE4 { get; set; }
         [DataMember]
         public string LIBELLE4 { get; set; }

         [DataMember]
         public string     COPER5 { get; set; }
         [DataMember]
         public string NATURE5 { get; set; }
         [DataMember]
         public string     OBLI5 { get; set; }
         [DataMember]
         public string     AUTO5 { get; set; }
         [DataMember]
         public decimal ?  MONTANT5 { get; set; }
         [DataMember]
         public string     TAXE5 { get; set; }
         [DataMember]
         public string LIBELLE5 { get; set; }

         public CsDemCout GetCoutCoper1(CsDemCout LesCouts)
         {
             CsDemCout LeCoutCoper1 = new CsDemCout()
             {

                 CENTRE = LesCouts.CENTRE ,
                 PRODUIT = LesCouts.PRODUIT ,
                 COPER1 = LesCouts.COPER1,
                 NATURE1 = LesCouts.NATURE1,
                 OBLI1 = LesCouts.OBLI1 ,
                 AUTO1 = LesCouts.AUTO1 ,
                 MONTANT1 = LesCouts.MONTANT1 ,
                 TAXE1 =LesCouts.TAXE1,
                 LIBELLE1 = LesCouts.LIBELLE1 
             };
             return LeCoutCoper1;
         }
         public CsDemCout GetCoutCoper2(CsDemCout LesCouts)
         {
             CsDemCout LeCoutCoper2 = new CsDemCout()
             {

                 CENTRE = LesCouts.CENTRE,
                 PRODUIT = LesCouts.PRODUIT,
                 COPER1 = LesCouts.COPER2,
                 NATURE1 = LesCouts.NATURE2,

                 OBLI1 = LesCouts.OBLI2,
                 AUTO1 = LesCouts.AUTO2,
                 MONTANT1 = LesCouts.MONTANT2,
                 TAXE1 = LesCouts.TAXE2,
                 LIBELLE1 = LesCouts.LIBELLE2
             };
             return LeCoutCoper2;
         }
         public CsDemCout GetCoutCoper3(CsDemCout LesCouts)
         {
             CsDemCout LeCoutCoper3 = new CsDemCout()
             {

                 CENTRE = LesCouts.CENTRE,
                 PRODUIT = LesCouts.PRODUIT,
                 COPER1 = LesCouts.COPER3,
                 NATURE1 = LesCouts.NATURE3,

                 OBLI1 = LesCouts.OBLI3,
                 AUTO1 = LesCouts.AUTO3,
                 MONTANT1 = LesCouts.MONTANT3,
                 TAXE1 = LesCouts.TAXE3,
                 LIBELLE1 = LesCouts.LIBELLE3
             };
             return LeCoutCoper3;
         }

         public CsDemCout GetCoutCoper4(CsDemCout LesCouts)
         {
             CsDemCout LeCoutCoper4 = new CsDemCout()
             {

                 CENTRE = LesCouts.CENTRE,
                 PRODUIT = LesCouts.PRODUIT,
                 COPER1 = LesCouts.COPER4,
                 NATURE1 = LesCouts.NATURE4,

                 OBLI1 = LesCouts.OBLI4,
                 AUTO1 = LesCouts.AUTO4,
                 MONTANT1 = LesCouts.MONTANT4,
                 TAXE1 = LesCouts.TAXE4,
                 LIBELLE1 = LesCouts.LIBELLE4
             };
             return LeCoutCoper4;
         }

         public CsDemCout GetCoutCoper5(CsDemCout LesCouts)
         {
             CsDemCout LeCoutCoper5 = new CsDemCout()
             {
                 CENTRE = LesCouts.CENTRE,
                 PRODUIT = LesCouts.PRODUIT,
                 COPER1 = LesCouts.COPER5,
                 NATURE1 = LesCouts.NATURE5,

                 OBLI1 = LesCouts.OBLI5,
                 AUTO1 = LesCouts.AUTO5,
                 MONTANT1 = LesCouts.MONTANT5,
                 TAXE1 = LesCouts.TAXE5,
                 LIBELLE1 = LesCouts.LIBELLE5
             };
             return LeCoutCoper5;
         }




    }

}









