using System;
using System.Security.Cryptography;
using System.Text;

namespace POV.Seguridad.Utils
{
    public class EncryptHash
    {
        public EncryptHash()
        {
        }

        #region MD5
        public static byte[] MD5encrypt(string phrase)
        {
            return MD5encrypt(1, phrase);
        }
        public static byte[] MD5encrypt(int cycles, string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes = encoder.GetBytes(phrase);

            for (int i = 0; i < cycles; i++)
                hashedDataBytes = md5hasher.ComputeHash(hashedDataBytes);

            return hashedDataBytes;
        } 
        #endregion

        #region SHA1
        public static byte[] SHA1encrypt(string phrase)
        {
            return SHA1encrypt(1, phrase);
        }
        public static byte[] SHA1encrypt(int cycles, string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1CryptoServiceProvider sha1hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = encoder.GetBytes(phrase);

            for (int i = 0; i < cycles; i++)
                hashedDataBytes = sha1hasher.ComputeHash(hashedDataBytes);

            return hashedDataBytes;
        }
        #endregion

        #region SHA256
        public static byte[] SHA256encrypt(string phrase)
        {
            return SHA256encrypt(1, phrase);
        }
        public static byte[] SHA256encrypt(int cycles, string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = encoder.GetBytes(phrase);

            for (int i = 0; i < cycles; i++)
                hashedDataBytes = sha256hasher.ComputeHash(hashedDataBytes);

            return hashedDataBytes;
        } 
        #endregion

        #region SHA384
        public static byte[] SHA384encrypt(string phrase)
        {
            return SHA384encrypt(1, phrase);
        }
        public static byte[] SHA384encrypt(int cycles, string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA384Managed sha384hasher = new SHA384Managed();
            byte[] hashedDataBytes = encoder.GetBytes(phrase);

            for (int i = 0; i < cycles; i++)
                hashedDataBytes = sha384hasher.ComputeHash(hashedDataBytes);

            return hashedDataBytes;
        } 
        #endregion

        #region SHA512
        public static byte[] SHA512encrypt(string phrase)
        {
            return SHA512encrypt(1, phrase);
        }
        public static byte[] SHA512encrypt(int cycles, string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA512Managed sha512hasher = new SHA512Managed();
            byte[] hashedDataBytes = encoder.GetBytes(phrase);

            for (int i = 0; i < cycles; i++)
                hashedDataBytes = sha512hasher.ComputeHash(hashedDataBytes);

            return hashedDataBytes;
        } 
        #endregion

        public static bool compareByteArray(byte[] arrayOne, byte[] arrayTwo)
        {
            if (arrayOne.Length != arrayTwo.Length)
                return false;
            for (int i = 0; i < arrayOne.Length; i++)
            {
                if (arrayOne[i] != arrayTwo[i])
                    return false;
            }
            return true;
        }
        public static string byteArrayToStringHex(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
        public static string byteArrayToStringBase64(byte[] inputArray)
        {
            return Convert.ToBase64String(inputArray);
        }
    }
}
