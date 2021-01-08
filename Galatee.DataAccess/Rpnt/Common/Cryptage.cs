using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Galatee.DataAccess.Common
{
    public class Crypteur : MarshalByRefObject, ISF.DEPLOIEMENT.COMMON.ICrypteurInova
    {
        private static SymmetricAlgorithm _mCSP = new TripleDESCryptoServiceProvider();

        //Ces valeurs ne doivent pas �tre modifi�es sinon les �l�ments crypt�s avec la cl� et le vecteur ci-dessous ne seront pas d�cryptables
        //Les valeurs ci-dessous sont d�finies de mani�res arbitraires
        private static byte[] _Cle = new byte[24] { 10, 244, 49, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La cl� est cod�e sur 192 bits
        private static byte[] _Vecteur = new byte[8] { 130, 206, 101, 99, 35, 128, 72, 225 };

        #region Cryptage de Chaine

        //Si on n'a pas besoin de decrypter la donn�e crypt�e alors on utilise le hachage
        public static string HasherText(string TextAHasher) 
        {
            try
            {
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider(); ;

                // Convertit le tring en tableau de Bytes
                byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(TextAHasher);

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

        //Si on a besoin de decrypter la donn�e crypt�e alors on utilise le cryptage      
        public static string CrypterText(string DonneeACrypter)
        {
            try
            {
				if (DonneeACrypter == null)
					DonneeACrypter = "";

                ICryptoTransform ct; //Une interface utilis�e pour pouvoir appeler la m�thode CreateEncryptor sur les fournisseurs de services, qui renverront un objet encryptor 
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;

                ct = _mCSP.CreateEncryptor(_Cle, _Vecteur);

                byt = Encoding.UTF8.GetBytes(DonneeACrypter); //convertir la cha�ne originale en un tableau d'octets

                ms = new MemoryStream(); //la cr�ation d'un flux dans lequel �crire les octets crypt�s
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write); //mode dans lequel vous voulez cr�er cette classe (lecture, �criture, etc.). 
                cs.Write(byt, 0, byt.Length); //�crire les donn�es dans le flux de m�moire en utilisant la m�thode Write de l'objet CryptoStream. C'est elle qui ex�cute concr�tement le cryptage et, � mesure que chaque bloc de donn�es est crypt�, les informations sont �crites dans l'objet MemoryStream.
                cs.FlushFinalBlock(); //pour v�rifier que toutes les donn�es ont �t� �crites dans l'objet MemoryStream

                cs.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
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

        //M�thode d'origine
        #region Cryptage de fichiers
        
        public bool CrypterFichier(string unfichier)
		{
			if (!File.Exists(unfichier)) 
			{
				return false;
			}
			//Fichier � crypter : en entr�e
			//Encoding leCode;
			StreamReader sr = new StreamReader (unfichier,Encoding.Default);
			//string ligne=sr1.ReadLine();


			// Fichier crypt�e : en sortie
			//FileInfo fi = new FileInfo(unfichier+".cry");  // Ouverture du fichier Crypt�
			FileInfo fi = new FileInfo(nouveauNomDuFichierCrypte(unfichier));  // Ouverture du fichier Crypt�

			StreamWriter sw = fi.CreateText();

			//			FileStream fs = fi.Create();

			string ligne = string.Empty;
			string resultat = string.Empty;
			while ((ligne=sr.ReadLine())!=null) 
			{
			
				//Utilisation de la m�thode Crypter() :
				resultat = Crypteur.CrypterText(ligne);
				//�crire dans le nouveau fichier crypt�
				//				EcrireLigne( resultat,ref fs );
				EcrireLigne( resultat,ref sw );
				ligne =  string.Empty;
				resultat = string.Empty;
			}
			sr.Close(); // Fichier � crypter
			//			fs.Flush(); fs.Close();// Fichier crypt�
			sw.Flush(); sw.Close();// Fichier crypt�

			return true;
		}
		public  bool DecrypterFichier(string unfichier)
		{
			if (!File.Exists(unfichier)) 
			{
				return false;
			}
			//Fichier � crypter : en entr�e
			StreamReader sr = File.OpenText(unfichier);
            //CrypteurSYGES cryptage = new CrypteurSYGES();
	
			// Fichier crypt�e : en sortie
			//FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier d�crypt�
			string NewNomFichier = this.NomDuFichierDecrypte(unfichier);
			if (NewNomFichier == "")
				return false;
			//FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier d�crypt�
			
			//StreamWriter sw = fi.CreateText();
			StreamWriter sw = new StreamWriter(NewNomFichier, false, Encoding.Default);
			
			//			FileStream fs = fi.Create();

			string ligne = string.Empty;
			string resultat = string.Empty;
			while ((ligne=sr.ReadLine())!=null) 
			{
				//Utilisation de la m�thode Crypter() :
				resultat = Crypteur.DecrypterText(ligne);
				//�crire dans le nouveau fichier crypt�
				EcrireLigne( resultat,ref sw );
				ligne =  string.Empty;
				resultat = string.Empty;
			}
			sr.Close(); // Fichier � crypter
			//			fs.Flush(); fs.Close();// Fichier crypt�
			sw.Flush(); sw.Close();// Fichier crypt�
			return true;
		}
		public  bool DecrypterFichier(string unfichier, string pathResult)
		{
			if (!File.Exists(unfichier)) 
			{
				return false;
			}
			//Fichier � crypter : en entr�e
			StreamReader sr = File.OpenText(unfichier);
			//StreamReader sr = new StreamReader(unfichier, Encoding.Default);
			//Creation d'un objet de la classe pour utiliser les m�thodes.
            //CrypteurSYGES cryptage = new CrypteurSYGES();
	
			// Fichier crypt�e : en sortie
			//FileInfo fi = new FileInfo(unfichier+".dry");  // Ouverture du fichier d�crypt�
			string NewNomFichier = this.NomDuFichierDecrypte(unfichier);
			if (NewNomFichier == "")
				return false;

			if (!Directory.Exists(pathResult))
				return false;
            NewNomFichier = Path.GetDirectoryName(pathResult) + Path.DirectorySeparatorChar + NewNomFichier;
			
			//FileInfo fi = new FileInfo(NewNomFichier);  // Ouverture du fichier d�crypt�
			
			//StreamWriter sw = fi.CreateText();

			StreamWriter sw = new StreamWriter(NewNomFichier, false, Encoding.Default);
			
			string ligne = string.Empty;
			string resultat = string.Empty;
			while ((ligne=sr.ReadLine())!=null) 
			{
				//Utilisation de la m�thode Crypter() :
				resultat = Crypteur.DecrypterText(ligne);
				//�crire dans le nouveau fichier crypt�
				EcrireLigne( resultat,ref sw );
				ligne =  string.Empty;
				resultat = string.Empty;
			}
			sr.Close(); // Fichier � crypter
			//			fs.Flush(); fs.Close();// Fichier crypt�
			sw.Flush(); sw.Close();// Fichier crypt�
			return true;
        }
		public  void EcrireLigne(string Ligne,ref StreamWriter sw ) 
		{
			sw.WriteLine (Ligne);
		}

        #region "Gestion de l'extension des fichiers crypt�s"
        public string ExtensionDeFichierCrypte()
		{
			return ".cry";
		}
        public bool EstUnFichierCrypte (string FileName)
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
		public bool PossedeExtensionDeFichierCrypte (string FileNameToTest)
		{
			string CryptedExtansion = ExtensionDeFichierCrypte();

			//Contr�le des variables en entr�e
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
			if (!this.PossedeExtensionDeFichierCrypte(CryptedFileName))
				return "";
			
			string NewName = CryptedFileName.Replace(this.ExtensionDeFichierCrypte(), "");

			if ((NewName == CryptedFileName) || (NewName == ""))
				return "";
			else
				return NewName;

		}
		#endregion

        #endregion

        #region ICrypteurInova Membres

        public string HashText_Irreversible(string pTexteAHascher)
        {
            return HasherText(pTexteAHascher);
        }

        public string CryptReversibleText(string pTexte)
        {
            return CrypterText(pTexte);
        }

        public string DecryptText(string pTexte)
        {
            return DecrypterText(pTexte);
        }

        #endregion
    }
}
