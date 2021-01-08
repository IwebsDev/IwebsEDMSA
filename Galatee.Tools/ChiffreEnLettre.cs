using System;
using System.Collections.Generic;
using System.Text;

namespace Galatee.Tools
{

    public static class ChiffresEnLettres
    {

        static bool dizaine = false;
        private static string Thousand(int pos, string culture)
        {
            switch (pos)
            {
                case 4:
                    return (culture == "fr-FR") ? "Mille " : "Thousand ";
                case 7:
                    return (culture == "fr-FR") ? "Million(s) " : "Million ";
                case 10:
                    return (culture == "fr-FR") ? "Milliard(s) " : "Billion ";
                default:
                    return "";
            }
        }



        private static string Hundred(string wch, string culture)
        {
            switch (wch)
            {
                case "1":
                    return (culture == "fr-FR") ? "Cent " : "One Hundred ";
                case "2":
                    return (culture == "fr-FR") ? "Deux Cents " : "Two Hundred ";
                case "3":
                    return (culture == "fr-FR") ? "Trois Cents " : "Three Hundred ";
                case "4":
                    return (culture == "fr-FR") ? "Quatre Cents " : "Four Hundred ";
                case "5":
                    return (culture == "fr-FR") ? "Cinq Cents " : "Five Hundred ";
                case "6":
                    return (culture == "fr-FR") ? "Six Cents " : "Six Hundred ";
                case "7":
                    return (culture == "fr-FR") ? "Sept Cents " : "Seven Hundred ";
                case "8":
                    return (culture == "fr-FR") ? "Huit Cents " : "Eight Hundred ";
                case "9":
                    return (culture == "fr-FR") ? "Neuf Cents " : "Nine Hundred ";
                default:
                    return "";
            }
        }


        private static string Twenty(string wch, string culture)
        {
            dizaine = false;
            switch (wch)
            {
                case "2":
                    return (culture == "fr-FR") ? "Vingt " : "Twenty ";
                case "3":
                    return (culture == "fr-FR") ? "Trente " : "Thirty ";
                case "4":
                    return (culture == "fr-FR") ? "Quarante " : "Forty ";
                case "5":
                    return (culture == "fr-FR") ? "Cinquante " : "Fifty ";
                case "6":
                    return (culture == "fr-FR") ? "Soixante " : "Sixty ";
                case "7":
                    dizaine = true;
                    return (culture == "fr-FR") ? "Soixante " : "Seventy ";
                case "8":
                    return (culture == "fr-FR") ? "Quatre-vingt " : "Eighty ";
                case "9":
                    dizaine = true;
                    return (culture == "fr-FR") ? "Quatre-vingt " : "Ninety ";
                default:
                    return "";
            }
        }


        private static string Eleven(string wch, string culture)
        {
            switch (wch)
            {
                case "0":
                    return (culture == "fr-FR") ? "Dix " : "Ten ";
                case "1":
                    return (culture == "fr-FR") ? "Onze " : "Eleven ";
                case "2":
                    return (culture == "fr-FR") ? "Douze " : "Twelve ";
                case "3":
                    return (culture == "fr-FR") ? "Treize " : "Thirteen ";
                case "4":
                    return (culture == "fr-FR") ? "Quatorze " : "Forteen ";
                case "5":
                    return (culture == "fr-FR") ? "Quinze " : "Fifteen ";
                case "6":
                    return (culture == "fr-FR") ? "Seize " : "Sixteen ";
                case "7":
                    return (culture == "fr-FR") ? "Dix-sept " : "Seventeen ";
                case "8":
                    return (culture == "fr-FR") ? "Dix-huit " : "Eighteen ";
                case "9":
                    return (culture == "fr-FR") ? "Dix-neuf " : "Nineteen ";
                default:
                    return "";
            }
        }


        private static string Unit(string wch, string culture)
        {
            switch (wch)
            {
                case "1":
                    return (culture == "fr-FR") ? "Un " : "One ";
                case "2":
                    return (culture == "fr-FR") ? "Deux " : "Two ";
                case "3":
                    return (culture == "fr-FR") ? "Trois " : "Three ";
                case "4":
                    return (culture == "fr-FR") ? "Quatre " : "Four ";
                case "5":
                    return (culture == "fr-FR") ? "Cinq " : "Five ";
                case "6":
                    return "Six ";
                case "7":
                    return (culture == "fr-FR") ? "Sept " : "Seven ";
                case "8":
                    return (culture == "fr-FR") ? "Huit " : "Eight ";
                case "9":
                    return (culture == "fr-FR") ? "Neuf " : "Nine ";
                default:
                    return "";
            }
        }


        private static string[] ExtraireParties(string montant, string separateur)
        {
            return montant.ToString().Split(new char[] { separateur[0] });
        }


        public static string AmountInWords(double Amount, string Devise, string Centime, string culture)
        {
            int k;
            int size = 0;
            string strWords = "";
            string strAmount;
            string partieEntiere = "";
            string partieDecimale = "";
            string wch = "";

            string separateur = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;

            strAmount = String.Format("{0:F}", Amount);

            if (Amount == 0)
                return strWords;

            string[] montant = ExtraireParties(strAmount, separateur);

            partieEntiere = montant[0];

            if (montant.Length > 1)
                partieDecimale = montant[1];


            size = partieEntiere.Length;

            for (int i = 0; i <= partieEntiere.Length - 1; i++)
            {
                wch = partieEntiere.Substring(i, 1);
                k = size % 3;

                switch (k)
                {
                    case 0:
                        strWords += Hundred(wch, culture);
                        break;
                    case 1:
                        if (dizaine)
                            strWords += Eleven(wch, culture);
                        else
                            strWords += Unit(wch, culture);
                        //if (wch != "0") 
                        strWords += Thousand(size, culture);
                        break;
                    case 2:
                        if (wch == "1")
                        {
                            i++;
                            wch = partieEntiere.Substring(i, 1); //grab second character
                            strWords += Eleven(wch, culture);
                            size--;
                            strWords += Thousand(size, culture);
                        }
                        else
                            strWords += Twenty(wch, culture);
                        break;
                }
                size--;
            }

            if (Amount >= 1)
                strWords += Devise + " ";





            if (montant.Length > 1 && partieDecimale.Length > 0)
            {
                //if (strUnit != "00")
                //{
                //size = 2; //'the 2 decimal places
                size = partieDecimale.Length; //'the decimal places
                for (int i = 0; i <= 1; i++)
                {
                    wch = partieDecimale.Substring(i, 1);
                    k = size % 3;

                    switch (k)
                    {
                        case 1:
                            if (dizaine)
                                strWords += Eleven(wch, culture);
                            else
                                strWords += Unit(wch, culture);
                            break;
                        case 2:
                            if (wch == "1")
                            {
                                i++;
                                wch = partieDecimale.Substring(i, 1); //grab second character
                                strWords += Eleven(wch, culture);
                                size--;
                            }
                            else
                                strWords += Twenty(wch, culture);
                            break;

                    }
                    size--;
                }
                strWords += Centime;
                //}
            }
            dizaine = false;
            return strWords.ToUpper();
        }

    }


}
