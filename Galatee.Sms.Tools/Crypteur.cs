using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Galatee.Sms.Tools
{
    public class Crypteur
    {
        private static readonly SymmetricAlgorithm _mCSP = new TripleDESCryptoServiceProvider();

        //Ces valeurs ne doivent pas être modifiées sinon les éléments cryptés avec la clé et le vecteur ci-dessous ne seront pas décryptables
        //Les valeurs ci-dessous sont définies de manières arbitraires
        private static readonly byte[] _Cle = new byte[24]
                                                  {
                                                      9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248
                                                      ,
                                                      121, 243, 66, 82, 93, 207, 159, 76
                                                  }; // La clé est codée sur 192 bits

        private static readonly byte[] _Vecteur = new byte[8] {130, 206, 100, 99, 35, 128, 72, 225};

        #region Cryptage de Chaine

        //Si on n'a pas besoin de decrypter la donnée cryptée alors on utilise le hachage
        public static string HasherText(string TextAHasher)
        {
            try
            {
                var SHA1 = new SHA1CryptoServiceProvider();
                ;

                // Convertit le tring en tableau de Bytes
                byte[] bytValue = Encoding.UTF8.GetBytes(TextAHasher);

                // Execute le hachage, retourne un tableau de bytes
                byte[] bytHash = SHA1.ComputeHash(bytValue);

                SHA1.Clear();

                // Return a base 64 encoded string of the Hash value
                return Convert.ToBase64String(bytHash);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Si on a besoin de decrypter la donnée cryptée alors on utilise le cryptage      
        public static string CrypterText(string DonneeACrypter)
        {
            try
            {
                if (DonneeACrypter == null)
                    DonneeACrypter = "";

                ICryptoTransform ct;
                //Une interface utilisée pour pouvoir appeler la méthode CreateEncryptor sur les fournisseurs de services, qui renverront un objet encryptor 
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;

                ct = _mCSP.CreateEncryptor(_Cle, _Vecteur);

                byt = Encoding.UTF8.GetBytes(DonneeACrypter); //convertir la chaîne originale en un tableau d'octets

                ms = new MemoryStream(); //la création d'un flux dans lequel écrire les octets cryptés
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                //mode dans lequel vous voulez créer cette classe (lecture, écriture, etc.). 
                cs.Write(byt, 0, byt.Length);
                //écrire les données dans le flux de mémoire en utilisant la méthode Write de l'objet CryptoStream. C'est elle qui exécute concrètement le cryptage et, à mesure que chaque bloc de données est crypté, les informations sont écrites dans l'objet MemoryStream.
                cs.FlushFinalBlock(); //pour vérifier que toutes les données ont été écrites dans l'objet MemoryStream

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Decrypter chaine de connexion 
        public static string DecrypterText(string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Value))
                    return string.Empty;

                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;

                ct = _mCSP.CreateDecryptor(_Cle, _Vecteur);

                byt = Convert.FromBase64String(Value);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Cryptage de fichiers

        public bool CrypterFichier(string unfichier)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            //Encoding leCode;
            var sr = new StreamReader(unfichier, Encoding.Default);
            //string ligne=sr1.ReadLine();


            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".cry");  // Ouverture du fichier Crypté
            var fi = new FileInfo(nouveauNomDuFichierCrypte(unfichier)); // Ouverture du fichier Crypté

            StreamWriter sw = fi.CreateText();

            //			FileStream fs = fi.Create();

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {
                //Utilisation de la méthode Crypter() :
                resultat = CrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                //				EcrireLigne( resultat,ref fs );
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush();
            sw.Close(); // Fichier crypté

            return true;
        }

        public bool DecrypterFichier(string unfichier)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            StreamReader sr = File.OpenText(unfichier);
            //CrypteurSYGES cryptage = new CrypteurSYGES();

            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier décrypté
            string NewNomFichier = NomDuFichierDecrypte(unfichier);
            if (NewNomFichier == "")
                return false;
            //FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier décrypté

            //StreamWriter sw = fi.CreateText();
            var sw = new StreamWriter(NewNomFichier, false, Encoding.Default);

            //			FileStream fs = fi.Create();

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {
                //Utilisation de la méthode Crypter() :
                resultat = DecrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush();
            sw.Close(); // Fichier crypté
            return true;
        }

        public bool DecrypterFichier(string unfichier, string pathResult)
        {
            if (!File.Exists(unfichier))
            {
                return false;
            }
            //Fichier à crypter : en entrée
            StreamReader sr = File.OpenText(unfichier);
            //StreamReader sr = new StreamReader(unfichier, Encoding.Default);
            //Creation d'un objet de la classe pour utiliser les méthodes.
            //CrypteurSYGES cryptage = new CrypteurSYGES();

            // Fichier cryptée : en sortie
            //FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier décrypté
            string NewNomFichier = NomDuFichierDecrypte(unfichier);
            if (NewNomFichier == "")
                return false;

            if (!Directory.Exists(pathResult))
                return false;
            NewNomFichier = Path.GetDirectoryName(pathResult) + Path.DirectorySeparatorChar + NewNomFichier;

            //FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier décrypté

            //StreamWriter sw = fi.CreateText();

            var sw = new StreamWriter(NewNomFichier, false, Encoding.Default);

            string ligne = string.Empty;
            string resultat = string.Empty;
            while ((ligne = sr.ReadLine()) != null)
            {
                //Utilisation de la méthode Crypter() :
                resultat = DecrypterText(ligne);
                //écrire dans le nouveau fichier crypté
                EcrireLigne(resultat, ref sw);
                ligne = string.Empty;
                resultat = string.Empty;
            }
            sr.Close(); // Fichier à crypter
            //			fs.Flush(); fs.Close();// Fichier crypté
            sw.Flush();
            sw.Close(); // Fichier crypté
            return true;
        }

        public void EcrireLigne(string Ligne, ref StreamWriter sw)
        {
            sw.WriteLine(Ligne);
        }

        #region "Gestion de l'extension des fichiers cryptés"

        public string ExtensionDeFichierCrypte()
        {
            return ".cry";
        }

        public bool EstUnFichierCrypte(string FileName)
        {
            return true;
        }

        public string nouveauNomDuFichierCrypte(string ActualFileName)
        {
            return ActualFileName + ExtensionDeFichierCrypte();
        }

        public string AjoutExtensionFichierCrypte(string ActualFileName)
        {
            return nouveauNomDuFichierCrypte(ActualFileName);
        }

        public bool PossedeExtensionDeFichierCrypte(string FileNameToTest)
        {
            string CryptedExtansion = ExtensionDeFichierCrypte();

            //Contrôle des variables en entrée
            if ((FileNameToTest == null) || (FileNameToTest.Length == 0))
                return false;

            //Traitements
            string TestName = FileNameToTest.ToUpper();
            if (TestName.IndexOf(CryptedExtansion.ToUpper()) < (TestName.Length - CryptedExtansion.Length))
                //if (FileNameToTest.IndexOf(CryptedExtansion) < 0)
                return false;

            //Fin de la fonction
            return true;
        }

        public string NomDuFichierDecrypte(string CryptedFileName)
        {
            if (CryptedFileName == null)
                return "";
            if (!PossedeExtensionDeFichierCrypte(CryptedFileName))
                return "";

            string NewName = CryptedFileName.Replace(ExtensionDeFichierCrypte(), "");

            if ((NewName == CryptedFileName) || (NewName == ""))
                return "";
            else
                return NewName;
        }

        #endregion

        #endregion

        public static String CryptePassV8(String sPassToCrypt)
        {
            if (sPassToCrypt.Length <= 2)
                return sPassToCrypt;

            String sFirstPart = "", sSecondPart = "";
            int iPos = (sPassToCrypt.Length - 1)/2;

            sFirstPart = sPassToCrypt.Substring(0, iPos);
            sSecondPart = sPassToCrypt.Substring(iPos, sPassToCrypt.Length - sFirstPart.Length);
            char[] sRev = sFirstPart.ToCharArray();
            Array.Reverse(sRev);
            sFirstPart = new String(sRev);
            return sSecondPart + sFirstPart;
        }

        public static String DecryptePassV8(String sPassToCrypt)
        {
            if (sPassToCrypt.Length <= 2)
                return sPassToCrypt;

            String sFirstPart = "", sSecondPart = "";
            int iPos = (sPassToCrypt.Length - 1)/2;

            sFirstPart = sPassToCrypt.Substring(0, sPassToCrypt.Length - iPos);
            sSecondPart = sPassToCrypt.Substring(sFirstPart.Length, iPos);
            char[] sRev = sSecondPart.ToCharArray();
            Array.Reverse(sRev);
            sSecondPart = new String(sRev);
            return sSecondPart + sFirstPart;
        }
    }
}
