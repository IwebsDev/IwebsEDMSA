using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;


namespace Galatee.Silverlight.Security
{
    public class Cryptage
    {
        public static string Encrypt(string input)
        {
            //byte[] _SaltByte = new byte[30] {2,32,123,70,80,21, 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
            byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45,  208, 63, 58, 174, 243, 66, 82, 207, 159}; // La clé est codée sur 192 bits
            //byte[] _SaltByte = new byte[24] { 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
            string Salt = Convert.ToBase64String(_SaltByte);
            byte[] utfData = UTF8Encoding.UTF8.GetBytes(input);
            byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
            string encryptedString = string.Empty;
            using (AesManaged aes = new AesManaged())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                {
                    using (MemoryStream encryptedStream = new MemoryStream())
                    {
                        using (CryptoStream encryptor =
                            new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                        {
                            encryptor.Write(utfData, 0, utfData.Length);
                            encryptor.Flush();
                            encryptor.Close();

                            byte[] encryptBytes = encryptedStream.ToArray();
                            encryptedString = Convert.ToBase64String(encryptBytes);
                        }
                    }
                }
            }
            return encryptedString;
        }

        public static string Decrypt(string input)
        {

            byte[] _SaltByte = new byte[15] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159 }; // La clé est codée sur 192 bits.//new byte[24] { 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
            string Salt = Convert.ToBase64String(_SaltByte);
            byte[] encryptedBytes = Convert.FromBase64String(input);
            byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
            string decryptedString = string.Empty;
            using (var aes = new AesManaged())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                {
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        CryptoStream decryptor =
                            new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                        decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                        decryptor.Flush();
                        decryptor.Close();

                        byte[] decryptBytes = decryptedStream.ToArray();
                        decryptedString =
                            UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                    }
                }
            }

            return decryptedString;
        }

        public static string GetPasswordToBeSaved(CsStrategieSecurite StrategieSecuriteActif, string UserDisplayName, string password)
        {
            //verifier que le password repond à la strategie (longueur minimale, complexité)
            //verifier que la longueur du password >= longueur minimale exigée
            if (password.Length < StrategieSecuriteActif.LONGUEURMINIMALEPASSWORD)
                throw new Exception(string.Format(Langue.MsgLongPwd, StrategieSecuriteActif.LONGUEURMINIMALEPASSWORD));

            Regex regex;
            //verifier que le password contient le Nombre minimal de Caractères majuscules anglais (A à Z)
            regex = new Regex(@"[A-Z]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERESMAJUSCULES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd, StrategieSecuriteActif.NOMBREMINIMALCARACTERESMAJUSCULES));

            //verifier que le password contient le Nombre minimal de Caractères minuscules anglais (a à z)
            regex = new Regex(@"[a-z]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NombreMinimalCaracteresMinuscules)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd2, StrategieSecuriteActif.NombreMinimalCaracteresMinuscules));

            //verifier que le password contient le Nombre minimal de chiffres
            regex = new Regex(@"[0-9]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERESCHIFFRES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd3, StrategieSecuriteActif.NOMBREMINIMALCARACTERESCHIFFRES));

            //verifier que le password contient le Nombre minimal de Caractères non alphanumeric
            regex = new Regex(@"\W{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERENONALPHABETIQUES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd4, StrategieSecuriteActif.NOMBREMINIMALCARACTERENONALPHABETIQUES));

            //Ne pas contenir le nom de compte de l’utilisateur ou des parties du nom complet de l’utilisateur 
            //comptant plus de trois caractères successifs
            if (StrategieSecuriteActif.NEPASCONTENIRNOMCOMPTE)
            {
                string[] PartiesNom = UserDisplayName.Split(' ');
                //string[] PartiesNom = UserDisplayName.Split(char.Parse(" "));
                foreach (string _nom in PartiesNom)
                {
                    //if (_nom.Length >= 4)
                    //{
                    //    string nom = _nom.ToLower();
                    //    for (int i = 0; i <= nom.Length - 4; i++)
                    //    {
                    //        regex = new Regex(nom.Substring(i, 4) + "{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                    //        if (regex.IsMatch(password.ToLower()))
                    //            throw new Exception(Langue.MsgPwdCpt);
                    //    }
                    //}

                    if (_nom.Length > 0)
                    {
                        if (password.ToLower().Contains(_nom.ToLower()))
                            throw new Exception(Langue.MsgPwdCpt);
                    }
                }
            }
            return Encrypt(password);
        }

        public static void ChangePasswordAdmin(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new Exception(Langue.MsgErrConfPwd);
        }

        public static string DecryptDate(string input)
        {

            byte[] _SaltByte = new byte[21] { 9, 244, 48, 109, 95, 45, 208, 63, 58, 174, 243, 66, 82, 207, 159, 243, 102, 244, 48, 109, 95 }; // La clé est codée su 192 bits.//new byte[214] { 9, 244, 48, 109, 95, 77, 45, 87, 208, 49, 98, 63, 58, 23, 174, 248, 121, 243, 66, 82, 93, 207, 159, 76 }; // La clé est codée sur 192 bits
            string Salt = Convert.ToBase64String(_SaltByte);
            byte[] encryptedBytes = Convert.FromBase64String(input);
            byte[] saltBytes = Encoding.UTF8.GetBytes(Salt);
            string decryptedString = string.Empty;
            using (var aes = new AesManaged())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(Salt, saltBytes);
                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                {
                    using (MemoryStream decryptedStream = new MemoryStream())
                    {
                        CryptoStream decryptor =
                            new CryptoStream(decryptedStream, decryptTransform, CryptoStreamMode.Write);
                        decryptor.Write(encryptedBytes, 0, encryptedBytes.Length);
                        decryptor.Flush();
                        decryptor.Close();

                        byte[] decryptBytes = decryptedStream.ToArray();
                        decryptedString =
                            UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                    }
                }
            }

            return decryptedString;
        }
        public static void StrategieDeMotDePasse(CsStrategieSecurite StrategieSecuriteActif, string UserDisplayName, string password)
        {
            //verifier que le password repond à la strategie (longueur minimale, complexité)
            //verifier que la longueur du password >= longueur minimale exigée
            if (password.Length < StrategieSecuriteActif.LONGUEURMINIMALEPASSWORD)
                throw new Exception(string.Format(Langue.MsgLongPwd, StrategieSecuriteActif.LONGUEURMINIMALEPASSWORD));

            Regex regex;
            //verifier que le password contient le Nombre minimal de Caractères majuscules anglais (A à Z)
            regex = new Regex(@"[A-Z]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERESMAJUSCULES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd, StrategieSecuriteActif.NOMBREMINIMALCARACTERESMAJUSCULES));

            //verifier que le password contient le Nombre minimal de Caractères minuscules anglais (a à z)
            regex = new Regex(@"[a-z]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NombreMinimalCaracteresMinuscules)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd2, StrategieSecuriteActif.NombreMinimalCaracteresMinuscules));

            //verifier que le password contient le Nombre minimal de chiffres
            regex = new Regex(@"[0-9]{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERESCHIFFRES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd3, StrategieSecuriteActif.NOMBREMINIMALCARACTERESCHIFFRES));

            //verifier que le password contient le Nombre minimal de Caractères non alphanumeric
            regex = new Regex(@"\W{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (regex.Matches(password).Count < StrategieSecuriteActif.NOMBREMINIMALCARACTERENONALPHABETIQUES)
                throw new Exception(string.Format(Langue.MsgNbrMiniPwd4, StrategieSecuriteActif.NOMBREMINIMALCARACTERENONALPHABETIQUES));

            //Ne pas contenir le nom de compte de l’utilisateur ou des parties du nom complet de l’utilisateur 
            //comptant plus de trois caractères successifs
            if (StrategieSecuriteActif.NEPASCONTENIRNOMCOMPTE)
            {
                string[] PartiesNom = UserDisplayName.Split(' ');
                //string[] PartiesNom = UserDisplayName.Split(char.Parse(" "));
                foreach (string _nom in PartiesNom)
                {
                    if (_nom.Length >= 4)
                    {
                        for (int i = 0; i <= _nom.Length - 4; i++)
                        {
                            regex = new Regex(_nom.Substring(i, 4) + "{1}", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                            if (regex.IsMatch(password))
                                throw new Exception(Langue.MsgPwdCpt);
                        }
                    }
                }
            }
        }

    }
}
