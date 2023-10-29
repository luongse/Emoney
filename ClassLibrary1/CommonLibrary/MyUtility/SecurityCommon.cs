/**********************************************************************
 * Author:  
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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyUtility
{
    public class SecurityCommon
    {
        #region Strong password

        /// <summary>
        /// https://stackoverflow.com/questions/5859632/regular-expression-for-password-validation/5859963
        /// </summary>
        /// <returns></returns>
        public static bool IsStrongPassword(string rawPassword, int minPasswordLength = 6, int maxPasswordLength = 30,
            bool isRequireSpecialCharacter = false)
        {
            if (minPasswordLength < 1) return false;
            if (minPasswordLength > maxPasswordLength) return false;

            var regexNoRequireSpecialChar = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{" + minPasswordLength + @"," + maxPasswordLength + @"}$";
            var regexRequireSpecialChar = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{" + minPasswordLength + @"," + maxPasswordLength + @"}$";

            var regex = isRequireSpecialCharacter
                ? new System.Text.RegularExpressions.Regex(regexRequireSpecialChar)
                : new System.Text.RegularExpressions.Regex(regexNoRequireSpecialChar);

            return regex.IsMatch(rawPassword);
        }

        #endregion

        #region ma hoa

        #region md5

        /// <summary>
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

        #endregion

        #region HMAC SHA 256

        /// <summary>
        /// Author:  
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
        /// Author:  
        /// <para></para>
        /// SHA256 Encrypt
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetHashHmac(string stringToHash, string password)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha256 = new HMACSHA256(pass))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return ConvertByteToString(hmacsha256.Hash);
            }
        }

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2017-09-27</para>
        /// <para>Description: HMAC SHA1 Encrypt</para>
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetHashHmacSha1(string stringToHash, string password)
        {
            var pass = Encoding.UTF8.GetBytes(password);
            using (var hmacsha = new HMACSHA1(pass))
            {
                hmacsha.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return ConvertByteToString(hmacsha.Hash);
            }
        }

        /// <summary>
        /// <para>Author:  </para>
        /// <para>DateCreated: 18/12/2014</para>
        /// <para>mã hóa sha256</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SHA256Hash(string value)
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

        #endregion

        #region TripDES

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-07-16</para>
        /// <para>Description: Ma hoa TripDES</para>
        /// </summary>
        /// <returns></returns>
        public static string EncryptTripDes(string stringToEncrypt, string securityKey, bool isUseHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            var key = securityKey;

            if (isUseHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,
                //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,
                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7,
            };

            var cTransform = tdes.CreateEncryptor();

            //transform the specified region of bytes array to resultArray
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-07-16</para>
        /// <para>Description: Giai ma TripDES</para>
        /// </summary>
        /// <returns></returns>
        public static string DescryptTripDes(string stringToDecrypt, string securityKey, bool isUseHashing = true)
        {
            byte[] keyArray;
            var toEncryptArray = Convert.FromBase64String(stringToDecrypt);
            var key = securityKey;

            if (isUseHashing)
            {
                //if hashing was used get the hash code with regards to your key
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));

                //release any resource held by the MD5CryptoServiceProvider
                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = Encoding.UTF8.GetBytes(key);
            }

            var tdes = new TripleDESCryptoServiceProvider
            {
                //set the secret key for the tripleDES algorithm
                Key = keyArray,

                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                Mode = CipherMode.ECB,

                //padding mode(if any extra byte added)
                Padding = PaddingMode.PKCS7,
            };

            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);

            //Release resources held by TripleDes Encryptor
            tdes.Clear();

            //return the Clear decrypted TEXT
            return Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        /// <summary>
        /// Mã hóa mật khẩu (mã hóa 1 chiều)
        /// </summary>
        /// <param name="cleanString">Chuỗi cần mã hóa</param>
        /// <returns>Chuỗi sau khi giải mã</returns>
        public static string Encrypt(string cleanString)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(hashedBytes);

            return BitConverter.ToString(hashedBytes);
        }

        public static TimeSpan ConvetDateTimeToTimeSpan(DateTime dateTime)
        {
            TimeSpan dateBetween = DateTime.Now - dateTime;
            return dateBetween;
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

        #region Triple Des
        public static string EncryptTripleDes(string toEncrypt, string key, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //// Get the key from config file

            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //Always release the resources and flush data
                //of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string DecryptTripleDes(string cipherString, string key, bool useHashing = true)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            ////Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            //set the secret key for the tripleDES algorithm
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)

            //padding mode(if any extra byte added)

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock
                    (toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region ma hoa base64

        public static string EncryptBase64(string toEncrypt)
        {
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            return Convert.ToBase64String(toEncryptArray, 0, toEncryptArray.Length);
        }

        public static string DecryptBase64(string cipherString)
        {
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            return UTF8Encoding.UTF8.GetString(toEncryptArray);
        }
        #endregion

        #region Encrypt

        private static readonly byte[] Sbox = new byte[255];
        private static readonly byte[] MyKey = new byte[255];

        private static string pr_Encrypt(string src, string key)
        {
            var mtxt = Encoding.ASCII.GetBytes(src);
            var mkey = Encoding.ASCII.GetBytes(key);
            var result = Calculate(mtxt, mkey);
            return CharsToHex(result);
        }

        private static string pr_Decrypt(string src, string key)
        {
            var mtxt = HexToChars(src);
            var mkey = Encoding.ASCII.GetBytes(key);
            var result = Calculate(mtxt, mkey);
            return Encoding.ASCII.GetString(result);
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

        private static byte[] HexToChars(string hex)
        {
            var len = hex.Length;
            var codes = new byte[len / 2];
            for (var i = (hex.Substring(0, 2) == "0x") ? 2 : 0; i < len; i += 2)
            {
                codes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return codes;
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

        public static string Encrypt(string cipher, string key, bool isencrypt)
        {
            return !isencrypt ? cipher : pr_Encrypt(cipher, key);
        }

        public static string Decrypt(string cipher, string key, bool isencrypt)
        {
            return !isencrypt ? cipher : pr_Decrypt(cipher, key);
        }

        #endregion

        #region AES 256 - To hexa string

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

        #endregion AES 256 - To hexa string

        #region Jwt - https://stackoverflow.com/questions/10055158/is-there-any-json-web-token-jwt-example-in-c

        public enum JwtHashAlgorithm
        {
            Rs256,
            Hs256,
            Hs384,
            Hs512
        }

        private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms =
            new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                { JwtHashAlgorithm.Rs256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.Hs256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.Hs384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.Hs512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };

        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwtHashAlgorithm.Rs256;
                case "HS256": return JwtHashAlgorithm.Hs256;
                case "HS384": return JwtHashAlgorithm.Hs384;
                case "HS512": return JwtHashAlgorithm.Hs512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }

        public static string[] JwtEncode(object header, object payload, string keySecret, JwtHashAlgorithm algorithm)
        {
            var segments = new List<string>();

            var headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            var payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());
            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);
            var keyBytes = Encoding.UTF8.GetBytes(keySecret);

            var signature = HashAlgorithms[algorithm](keyBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return segments.ToArray();
        }

        public static string JwtDecode(string token, string key, bool verify)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);

            if (verify)
            {
                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var algorithm = (string)headerData["alg"];

                var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                var decodedCrypto = Convert.ToBase64String(crypto);
                var decodedSignature = Convert.ToBase64String(signature);

                if (decodedCrypto != decodedSignature)
                {
                    throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                }
            }

            return payloadData.ToString();
        }

        public static T JwtDecode<T>(string token, string key, bool verify)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);

            if (!verify)
            {
                return string.IsNullOrEmpty(payloadData.ToString())
                    ? default(T)
                    : JsonConvert.DeserializeObject<T>(payloadData.ToString());
            }

            var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var algorithm = (string)headerData["alg"];

            var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
            var decodedCrypto = Convert.ToBase64String(crypto);
            var decodedSignature = Convert.ToBase64String(signature);

            if (decodedCrypto != decodedSignature)
            {
                throw new ApplicationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
            }

            return string.IsNullOrEmpty(payloadData.ToString())
                ? default(T)
                : JsonConvert.DeserializeObject<T>(payloadData.ToString());
        }

        #endregion


        #region RSA - https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET | https://superdry.apphb.com/tools/online-rsa-key-converter | https://8gwifi.org/RSAFunctionality?keysize=2048

        public static string RsaEncrypt(string plainText, string xmlPublicKey, int dwKeySize = 2048)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);
            using (var rsa = new RSACryptoServiceProvider(dwKeySize))
            {
                try
                {
                    rsa.FromXmlString(xmlPublicKey);
                    var encryptedData = rsa.Encrypt(bytesToEncrypt, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static string RsaDecrypt(string cypherText, string xmlPrivateKey, int dwKeySize = 2048)
        {
            using (var rsa = new RSACryptoServiceProvider(dwKeySize))
            {
                try
                {

                    // server decrypting data with private key                    
                    rsa.FromXmlString(xmlPrivateKey);

                    var resultBytes = Convert.FromBase64String(cypherText);
                    var decryptedBytes = rsa.Decrypt(resultBytes, false);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        #endregion


        #region AES 256 - Random

        internal class SecurityCommonAesHelperUtils
        {
            public static readonly string DefaultAlphabet = "_-0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public static readonly Random DefaultNumberGenerator = new Random();
            public static readonly int DefaultSize = 21;

            public static string RandomNanoId()
            {
                return RandomNanoId(DefaultNumberGenerator, DefaultAlphabet, DefaultSize);
            }

            public static string RandomNanoId(Random random, string cArr, int i)
            {
                if (random == null)
                    throw new ArgumentNullException(nameof(random), "Random cannot be null.");

                if (cArr == null)
                    throw new ArgumentNullException(nameof(cArr), "alphabet cannot be null.");

                if (cArr.Length == 0 || cArr.Length >= 256)
                    throw new ArgumentException("alphabet must contain between 1 and 255 symbols.", nameof(cArr));

                if (i <= 0)
                    throw new ArgumentException("size must be greater than zero.", nameof(i));

                var floor = (2 << ((int)Math.Floor(Math.Log((double)(cArr.Length - 1)) / Math.Log(2.0d)))) - 1;
                var d = (double)floor;
                var d2 = (double)i;
                var d3 = d * 1.6d * d2;
                var length = cArr.Length;
                var ceil = (int)Math.Ceiling(d3 / length);
                var sb = new StringBuilder();
                while (true)
                {
                    var bArr = new byte[ceil];
                    random.NextBytes(bArr);
                    var i2 = 0;
                    while (true)
                    {
                        if (i2 >= ceil) continue;

                        var i3 = bArr[i2] & floor;
                        if (i3 < cArr.Length)
                        {
                            sb.Append(cArr[i3]);
                            if (sb.Length == i)
                                return sb.ToString();
                        }
                        i2++;
                    }
                }
            }
        }

        public static Random SecurityCommonRandom = new Random();
        public class SecurityCommonAesHelper
        {
            private const string NanoCharArray = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~!@#$%^&*()-_+={[]}<>";

            public static string Generate(int length)
            {
                var randomNanoId = SecurityCommonAesHelperUtils.RandomNanoId(SecurityCommonRandom, NanoCharArray, length);
                return randomNanoId;
            }

            public static string RandomSecretKey()
            {
                return Generate(32);
            }

            public static string RandomInitializationVector()
            {
                return Generate(16);
            }
        }

        #endregion
    }
}