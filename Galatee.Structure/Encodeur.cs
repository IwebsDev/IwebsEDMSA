using System;
using System.Collections.Generic;
using System.Text;

namespace Galatee.Structure
{
    public class Encodeur
    {
        static long Gn__MicGui;

        public Encodeur()
        {
        }

        /********************************************/
        /* Fonction RandomMicGui                    */
        /* G‚nŠre un nombre al‚atoire entre N1 et N2*/
        /* Utilise l'accumulateur Gn__MicGui        */
        /********************************************/

        private static int RandomMicGui(int iN1, int iN2)
        {
            Gn__MicGui = ((92 * Gn__MicGui) + 2731) % 31231;

            if (Gn__MicGui < 0)
                Gn__MicGui *= -1;

            return (int)(iN1 + ((Gn__MicGui * (iN2 - iN1)) / 31231));
        }

        /********************************************/
        /* Fonction CodePassWord                    */
        /* Code le mot de passe passé en paramètre  */
        /* Utilise la fonction RanDomMicGui         */
        /* pour générer des filtres.                */
        /********************************************/

        public static string CodePassWord(string szStrArg)
        {
            int i;
            string szStrResult = "";

            char[] szStr = new char[8];
            for (i = 0; i < 8; i++)
                szStr[i] = (i < szStrArg.Length ? szStrArg[i] : '\0');

            Gn__MicGui = szStrArg.Length * szStr[0];
            for (i = 0; i < szStrArg.Length; i++)
                szStr[i] |= (char)RandomMicGui(5, 50);
            for (i = szStrArg.Length; i < 8; i++)
                szStr[i] = (char)RandomMicGui(33, 127);

            for (i = 0; i < szStr.Length; i++)
                szStrResult += "" + szStr[i];
            return szStrResult;
        }
    }
}
