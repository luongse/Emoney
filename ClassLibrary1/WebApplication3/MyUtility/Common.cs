/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014 
 * Description: Common define common static function 
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace MyUtility
{
    public class Common
    {
        #region ma hoa

        #region md5
        /// <summary>
        /// Author: ThongNT
        /// <para></para>
        /// Md5 Encrypt
        /// </summary>
        /// <param name="signOrginal"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string signOrginal)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(signOrginal));
                string hashString = ConvertByteToString(data);
                return hashString;
            }
        }

        public static string MD5_encode(string str_encode)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string MD5_encode_utf16(string str_encode)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();
            using (var md5Hash = System.Security.Cryptography.MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(new System.Text.UnicodeEncoding().GetBytes(str_encode));
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #endregion

        #region HMAC SHA 256

        /// <summary>
        /// Author: ThongNT
        /// <para></para>
        /// Convert byte array to hexa string
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private static string ConvertByteToString(IEnumerable<byte> buff)
        {
            return buff.Aggregate("", (current, t) => current + t.ToString("X2"));
        }

        /// <summary>
        /// Author: ThongNT
        /// <para></para>
        /// SHA256 Encrypt
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="password"></param>
        /// <param name="isUpperCase"></param>
        /// <returns></returns>
        public static string GetHashHmac(string stringToHash, string password, bool isUpperCase = true)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha256 = new HMACSHA256(pass))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                var resultString = ConvertByteToString(hmacsha256.Hash);
                return isUpperCase ? resultString.ToUpper() : resultString.ToLower();
            }
        }

        public static string GetHashHmacToBase64(string stringToHash, string password)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha256 = new HMACSHA256(pass))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return Convert.ToBase64String(hmacsha256.Hash);
            }
        }

        public static string GetHashHmac512(string stringToHash, string password, bool isUpperCase = true)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha512 = new HMACSHA512(pass))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                var resultString = ConvertByteToString(hmacsha512.Hash);
                return isUpperCase ? resultString.ToUpper() : resultString.ToLower();
            }
        }

        public static string GetHashHmac512ToBase64(string stringToHash, string password)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha512 = new HMACSHA512(pass))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return Convert.ToBase64String(hmacsha512.Hash);
            }
        }

        /// <summary>
        /// <para>Author: TrungLD</para>
        /// <para>DateCreated: 18/12/2014</para>
        /// <para>mã hóa sha256</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String sha256_hash(String value)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        public static string Sha1Hash(string stringToHash, bool isUpper = false)
        {
            var sb = new StringBuilder();
            using (var hash = SHA1.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(stringToHash));

                foreach (var b in result)
                    sb.Append(b.ToString(isUpper ? "X2" : "x2"));
            }

            return sb.ToString();
        }

        #endregion

        private static readonly byte[] Sbox = new byte[255];
        private static readonly byte[] MyKey = new byte[255];

        /// <summary>
        /// Mã hóa mật khẩu (mã hóa 1 chiều)
        /// </summary>
        /// <param name="cleanString">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string Encrypt(string cleanString)
        {
            Byte[] clearBytes = new System.Text.UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(hashedBytes);

            var tmp = MD5.Create().ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes);
        }

        /// <summary>
        /// Mã hóa mật khẩu với SHA1 (mã hóa 1 chiều)
        /// </summary>
        /// <param name="cleanString">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string GetMd5C(string cleanString)
        {
            //Byte[] clearBytes = cleanString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();
            Byte[] clearBytes = null;
            var convertByte = StrToByteArray(cleanString, ref clearBytes);
            if (!convertByte || clearBytes == null)
            {
                return string.Empty;
            }
            //Byte[] hashedBytes = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            Byte[] hashedBytes = ((System.Security.Cryptography.HashAlgorithm)System.Security.Cryptography.CryptoConfig.CreateFromName("SHA1")).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes);
        }

        private static bool StrToByteArray(string hexString, ref byte[] HexAsBytes)
        {
            try
            {
                HexAsBytes = new byte[hexString.Length / 2];
                for (int index = 0; index < HexAsBytes.Length; index++)
                {
                    string byteValue = hexString.Substring(index * 2, 2);
                    HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private static string pr_Encrypt(string src, string key)
        {
            var mtxt = Encoding.ASCII.GetBytes(src);
            var mkey = Encoding.ASCII.GetBytes(key);
            var result = Calculate(mtxt, mkey);
            return CharsToHex(result);
        }
        private static string CharsToHex(IList<byte> chars)
        {
            var result = string.Empty;
            var len = chars.Count;
            for (var i = 0; i < len; i++)
            {
                result += String.Format("{0:x2}", chars[i]);
            }
            return result;
        }
        private static byte[] Calculate(IList<byte> plaintxt, IList<byte> psw)
        {
            Initialize(psw);
            byte i = 0;
            byte j = 0;
            var len = plaintxt.Count;
            var cipher = new byte[len];
            for (var a = 0; a < len; a++)
            {
                i = (byte)((i + 1) % 255);
                j = (byte)((j + Sbox[i]) % 255);

                var temp = Sbox[i];
                Sbox[i] = Sbox[j];
                Sbox[j] = temp;

                var idx = (byte)((Sbox[i] + Sbox[j]) % 255);
                int k = Sbox[idx];

                var cipherby = (byte)(plaintxt[a] ^ k);
                cipher[a] = cipherby;
            }
            return cipher;
        }



        private static void Initialize(IList<byte> pwd)
        {
            byte b = 0;
            var intLength = pwd.Count;

            for (byte a = 0; a < 255; a++)
            {
                MyKey[a] = pwd[(a % intLength)];
                Sbox[a] = a;
            }

            for (byte a = 0; a < 255; a++)
            {
                b = (byte)((b + Sbox[a] + MyKey[a]) % 255);
                var tempSwap = Sbox[a];
                Sbox[a] = Sbox[b];
                Sbox[b] = tempSwap;
            }
        }

        public static TimeSpan ConvetDateTimeToTimeSpan(DateTime dateTime)
        {
            TimeSpan dateBetween = DateTime.Now - dateTime;
            return dateBetween;
        }

        public static double ConvetDateTimeToUnixTimeSpan(DateTime value)
        {
            return Convert.ToDouble(value.ToString("yyyyMMddHHmmss"));
        }

        public static long ConvetDateTimeToUnixTimeSpanLong(DateTime value)
        {
            try
            {
                return Convert.ToInt64(value.ToString("yyyyMMddHHmmss"));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string Encrypt(string cipher, string key, bool isencrypt)
        {
            return !isencrypt ? cipher : pr_Encrypt(cipher, key);
        }

        #endregion

        #region RANDOM_STRING
        /// <summary>
        /// Trả về chuỗi random
        /// </summary>
        /// <param name="size">độ dài của chuỗi</param>
        /// <param name="lowerCase">viết hoa hay thường.True:Viết hoa,Flase:Viết thường</param>
        /// <returns>Chuỗi sau khi random</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;

            Random rndint = new Random();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                int so = rnd.Next(0, 2);
                if (so == 1)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                }
                else
                {
                    ch = Convert.ToChar(rndint.Next(0, 9).ToString());
                }

                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        #endregion

        #region AES 256 - To base64 string

        public static string Aes256Decrypt(string str, string key, string iv)
        {
            key = Aes256Md5(key);
            iv = Aes256Md5(iv).Substring(0, 16);
            return Aes256DecryptStringFromBytes(Convert.FromBase64String(str), Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
        }

        public static string Aes256Encrypt(string str, string key, string iv)
        {
            key = Aes256Md5(key);
            iv = Aes256Md5(iv).Substring(0, 16);
            var bytes = Aes256EncryptStringToBytes(str, Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
            return Convert.ToBase64String(bytes);
        }

        internal static byte[] Aes256EncryptData(string data)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var encoder = new UTF8Encoding();
            var hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        internal static string Aes256Md5(string data)
        {
            return BitConverter.ToString(Aes256EncryptData(data)).Replace("-", "").ToLower();
        }

        internal static byte[] Aes256EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.KeySize = 256;
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        internal static string Aes256DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");

            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }

        #endregion AES 256 - To base64 string

        #region AES 256 - To hexa string




        #endregion AES 256 - To hexa string

        public static string ConvertFromBase64(string base64String)
        {
            try
            {
                var byteArray = Convert.FromBase64String(base64String.Replace(" ", "+"));
                return Encoding.UTF8.GetString(byteArray);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string ConvertToBase64(string value)
        {
            try
            {
                var toEncodeAsBytes = Encoding.UTF8.GetBytes(value);
                var returnValue = Convert.ToBase64String(toEncodeAsBytes);
                return returnValue;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Giải mã chuỗi Json thành Object và bỏ qua Catch
        /// </summary>
        /// <typeparam name="T">Kiểu đối tượng muốn chuyển đổi thành</typeparam>
        /// <param name="value">Giá trị chuỗi Json</param>
        /// <returns></returns>
        public static T TryDeserializeObject<T>(string value)
        {
            T retValue = default(T);
            try
            {
                retValue = JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                //throw ex;                
            }
            return retValue;
        }

        /// <summary>
        /// Convert Object thành chuỗi Json
        /// </summary>
        /// <param name="obj">Đối tượng muốn convert</param>
        /// <returns></returns>
        public static string TrySerializeObject(object obj)
        {
            string retValue = null;
            try
            {
                retValue = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                //throw ex;                
            }

            return retValue;
        }

        public static long ConvetDateTimeToTotalMilliseconds(DateTime value)
        {
            return Convert.ToInt64(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }

        #region time valid

        /// <summary>
        /// kiểm tra thời gian hợp lệ
        /// </summary>
        /// <param name="timeStart"></param>
        /// <param name="timeEnd"></param>
        /// <returns></returns>
        public static bool CheckTimeString(string timeStart, string timeEnd)
        {
            if (string.IsNullOrEmpty(timeStart) && string.IsNullOrEmpty(timeEnd))
                return true;

            if (string.IsNullOrEmpty(timeStart))
                timeStart = "00:00:00";

            if (string.IsNullOrEmpty(timeEnd))
                timeStart = "23:59:59";

            if (timeStart == "00:00:00" && timeEnd == "23:59:59")
                return true;

            DateTime now = DateTime.Now;
            DateTime dateFrom = DateTime.ParseExact(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), timeStart), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(string.Format("{0} {1}", now.ToString("yyyy-MM-dd"), timeEnd), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            // data đẹp
            if (dateFrom < dateTo)
            {
                if (now >= dateFrom && now < dateTo)
                    return true;

                return false;
            }

            // từ 23h -> 1h
            if (dateFrom > dateTo && now >= dateFrom && now < dateTo.AddDays(1))
                return true;

            // từ 3h -> 1h
            if (now >= dateFrom.AddDays(-1) && now < dateTo)
                return true;

            return false;
        }

        #endregion

        public static bool IsNumberic(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            double myNum = 0;
            return Double.TryParse(value, out myNum);
        }

        public static bool IsAllNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.All(char.IsNumber);
        }
    }
}