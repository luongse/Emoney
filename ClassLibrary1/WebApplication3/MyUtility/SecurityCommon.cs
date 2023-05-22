using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MyUtility
{
    public class SecurityCommon
    {
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
            var crypto = Base64UrlDecode(parts[2]);

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
        /*
      public static string JwtDecodeWithRSA(string jwtToken, string privateKeyXmlFormat)
      {
          using (var rsa = RSA.Create())
          {
              rsa.FromXmlString(privateKeyXmlFormat);
              using (var csp = new RSACryptoServiceProvider(rsa.KeySize))
              {
                  csp.FromXmlString(privateKeyXmlFormat);
                  var jwtDecoded = Jose.JWT.Decode(jwtToken, csp);

                  var jweDecoded = Jose.JWE.Decrypt(jwtDecoded, csp);
                  return JwtDecode(jweDecoded.Plaintext, "", false);
              }
          }
      }

       */

        #endregion


        public string Byte2Hex(byte[] b)
        {
            var str = "";
            for (var i = 0; i < b.Length; i++)
            {
                var str2 = Convert.ToString(b[i] & 0xff, 0x10);
                if (str2.Length == 1)
                {
                    str = str + "0" + str2;
                }
                else
                {
                    str = str + str2;
                }
                if (i < (b.Length - 1))
                {
                    str = str ?? "";
                }
            }
            return str.ToUpper();
        }

        public string Byte2Hex_24(byte[] b)
        {
            var str = "";
            for (var i = b.Length - 0x20; i < (b.Length - 8); i++)
            {
                var str2 = Convert.ToString(b[i] & 0xff, 0x10);
                if (str2.Length == 1)
                {
                    str = str + "0" + str2;
                }
                else
                {
                    str = str + str2;
                }
                if (i < (b.Length - 1))
                {
                    str = str ?? "";
                }
            }
            return str.ToUpper();
        }

        public string Byte2Hex_24_en(byte[] b)
        {
            var str = "";
            for (var i = b.Length - 0x18; i < b.Length; i++)
            {
                var str2 = Convert.ToString(b[i] & 0xff, 0x10);
                if (str2.Length == 1)
                {
                    str = str + "0" + str2;
                }
                else
                {
                    str = str + str2;
                }
                if (i < (b.Length - 1))
                {
                    str = str ?? "";
                }
            }
            return str.ToUpper();
        }

        public string Byte2Hex1(byte[] b)
        {
            var str = "";
            for (var i = b.Length - 0x10; i < (b.Length - 8); i++)
            {
                var str2 = Convert.ToString(b[i] & 0xff, 0x10);
                if (str2.Length == 1)
                {
                    str = str + "0" + str2;
                }
                else
                {
                    str = str + str2;
                }
                if (i < (b.Length - 1))
                {
                    str = str ?? "";
                }
            }
            return str.ToUpper();
        }

        public string Decrypt(string msg, string key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes("hywebpg5");
            byte[] data = Hex2Byte(msg);
            byte[] buffer3 = Encoding.UTF8.GetBytes(key);
            byte[] buffer4 = Decrypt(data, buffer3, bytes);
            return Encoding.UTF8.GetString(buffer4);
        }

        public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            try
            {
                var provider = new DESCryptoServiceProvider
                {
                    Key = key,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    IV = iv
                };
                var stream = new MemoryStream(data);
                var stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Read);
                var buffer = new byte[data.Length];
                stream2.Read(buffer, 0, buffer.Length);
                return buffer.Where(num => num != 0).ToArray();
            }
            catch (CryptographicException exception)
            {
                //logger.ErrorFormat("A Cryptographic error occurred: {0}, Data={1}, Length={2}", new object[] { exception.Message, data.Length, data.Length });
                return null;
            }
            catch (Exception exception2)
            {
                //logger.ErrorFormat("A exception occurred: {0}", new object[] { exception2.Message });
                return null;
            }
        }

        public string Des3Decrypt(string data, string key)
        {
            var encoding = new ASCIIEncoding();
            var buffer2 = Hex2Byte(data);
            var bytes = encoding.GetBytes(key);
            var buffer5 = RemovePadding(Des3Decrypt(buffer2, bytes, null));
            return encoding.GetString(buffer5);
        }

        public byte[] Des3Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            try
            {
                var edes = TripleDES.Create();
                if (iv != null)
                {
                    edes.IV = iv;
                }
                edes.Key = key;
                edes.Mode = CipherMode.ECB;
                edes.Padding = PaddingMode.None;
                return edes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
            }
            catch (CryptographicException exception)
            {
                //logger.ErrorFormat("A Cryptographic error occurred: {0}, Data={1}, Length={2}", new object[] { exception.Message, data.Length, data.Length });
                return null;
            }
            catch (Exception exception2)
            {
                //logger.ErrorFormat("A exception occurred: {0}", new object[] { exception2.Message });
                return null;
            }
        }

        public string Des3Encrypt(string data, string key)
        {
            var bytes = Encoding.UTF8.GetBytes(Padding(data));
            var buffer3 = Encoding.UTF8.GetBytes(key);
            var b = Des3Encrypt(bytes, buffer3, null);
            return Byte2Hex(b);
        }

        public byte[] Des3Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            var buffer = new byte[0];
            try
            {
                var edes = TripleDES.Create();
                if (iv != null)
                {
                    edes.IV = iv;
                }
                edes.Key = key;
                edes.Mode = CipherMode.ECB;
                edes.Padding = PaddingMode.None;
                return edes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
            }
            catch (CryptographicException ex)
            {
                //logger.ErrorFormat("A Cryptographic error occurred: {0}", new object[] { exception.Message });
            }
            return buffer;
        }

        public string DESEDEMAC_24(string msg, string key)
        {
            var num = msg.Length % 8;
            if (num != 0)
            {
                for (var i = 0; i < (8 - num); i++)
                {
                    msg = msg + "0";
                }
            }
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes("hywebpg5");
            var data = encoding.GetBytes(msg);
            var buffer3 = encoding.GetBytes(key);
            var b = Des3Encrypt(data, buffer3, bytes);
            return Byte2Hex1(b);
        }

        public string Desmac(string msg, string key)
        {
            var bytes = Encoding.UTF8.GetBytes(msg);
            var buffer3 = Encoding.UTF8.GetBytes(key);
            var hashedBytes = new SHA1CryptoServiceProvider().ComputeHash(bytes);
            var data = Padding(hashedBytes);
            var b = Des3Encrypt(data, buffer3, null);
            return Byte2Hex(b);
        }

        public string Encrypt(string msg, string key)
        {
            var bytes = Encoding.UTF8.GetBytes("hywebpg5");
            var data = Encoding.UTF8.GetBytes(msg);
            var buffer3 = Encoding.UTF8.GetBytes(key);
            var b = Encrypt(data, buffer3, bytes);
            return Byte2Hex(b);
        }

        public byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            try
            {
                var stream = new MemoryStream();
                var provider = new DESCryptoServiceProvider
                {
                    Key = key,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    IV = iv
                };
                var stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                var buffer = data;
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                var buffer2 = stream.ToArray();
                stream2.Close();
                stream.Close();
                return buffer2;
            }
            catch (CryptographicException exception)
            {
                //logger.ErrorFormat("A Cryptographic error occurred: {0}", new object[] { exception.Message });
                return null;
            }
        }

        public byte[] Hex2Byte(string hex)
        {
            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException();
            }
            var chArray = hex.ToCharArray();
            var buffer = new byte[hex.Length / 2];
            var index = 0;
            var num2 = 0;
            var length = hex.Length;
            while (index < length)
            {
                var num4 = Convert.ToInt16("" + chArray[index++] + chArray[index], 0x10) & 0xff;
                buffer[num2] = Convert.ToByte(num4);
                index++;
                num2++;
            }
            return buffer;
        }

        private byte[] Padding(byte[] hashedBytes)
        {
            try
            {
                var sourceArray = hashedBytes;
                var num = 8 - (sourceArray.Length % 8);
                var destinationArray = new byte[sourceArray.Length + num];
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
                for (var i = sourceArray.Length; i < destinationArray.Length; i++)
                {
                    destinationArray[i] = 0;
                }
                return destinationArray;
            }
            catch (Exception)
            {
                //logger.Error("Crypter.padding UnsupportedEncodingException", exception);
            }
            return null;
        }

        public static string Padding(string str)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(str);
                var num = 8 - (bytes.Length % 8);
                var destinationArray = new byte[bytes.Length + num];
                Array.Copy(bytes, destinationArray, bytes.Length);
                for (var i = bytes.Length; i < destinationArray.Length; i++)
                {
                    destinationArray[i] = 0;
                }
                return Encoding.UTF8.GetString(destinationArray);
            }
            catch (Exception exception)
            {
                //logger.Error("Crypter.padding UnsupportedEncodingException", exception);
            }
            return null;
        }

        public static byte[] RemovePadding(byte[] oldByteArray)
        {
            var num = 0;
            for (var i = oldByteArray.Length; i >= 0; i--)
            {
                if (oldByteArray[i - 1] == 0) continue;
                num = oldByteArray.Length - i;
                break;
            }
            var destinationArray = new byte[oldByteArray.Length - num];
            Array.Copy(oldByteArray, destinationArray, destinationArray.Length);
            return destinationArray;
        }

        public string Decryption(string cypherText, string key)
        {
            var encoding = new ASCIIEncoding();
            var b = Convert.FromBase64String(cypherText);
            var des = CreateTripleDES(key);

            //ICryptoTransform ct = des.CreateDecryptor();
            var output = des.CreateDecryptor().TransformFinalBlock(b, 0, b.Length);
            //byte[] output = ct.TransformFinalBlock(b, 0, b.Length);

            return Encoding.ASCII.GetString(output);// Encoding.ASCII.GetString(output);
        }

        public TripleDES CreateTripleDES(string key)
        {
            var keyByte = Encoding.ASCII.GetBytes(Md5(key).Substring(0, 24));
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = keyByte;
            des.IV = Encoding.ASCII.GetBytes(Md5(key).Substring(0, 8));
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.None;
            return des;
        }
        public string Des3DecryptTopup(string data, string key)
        {
            var encoding = new ASCIIEncoding();
            var buffer2 = Convert.FromBase64String(data);
            var bytes = encoding.GetBytes(Md5(key).Substring(0, 24));
            var buffer5 = RemovePadding(Des3Decrypt(buffer2, bytes, null));
            return encoding.GetString(buffer5);
        }
        public string Md5(String data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(data));
            //get hash result after compute it
            var result = md5.Hash;
            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }
            return strBuilder.ToString();
        }

        #region AES

        public static string EncryptDataAES(string toEncrypt, string aesKey, string aesIv, bool toBase64 = true)
        {
            try
            {
                var _toEncrypt = Encoding.UTF8.GetBytes(toEncrypt);
                var key = Encoding.UTF8.GetBytes(aesKey);
                byte[] encryptedData;
                byte[] iv = Encoding.UTF8.GetBytes(aesIv);

                using (SymmetricAlgorithm aes = SymmetricAlgorithm.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform encryptor = aes.CreateEncryptor();
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                        {
                            cStream.Write(_toEncrypt, 0, _toEncrypt.Length);
                            cStream.FlushFinalBlock();
                            encryptedData = mStream.ToArray();
                        }
                    }
                }

                return toBase64
                    ? Convert.ToBase64String(encryptedData)
                    : encryptedData.Aggregate("", (current, next) => current + next.ToString("x2"));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string DecryptDataAES(string cipherText, string aesKey, string aesIv, bool isBase64 = true)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(aesKey);
                var _cipherText = isBase64 ? Convert.FromBase64String(cipherText) : StringHexToByteArray(cipherText);
                byte[] iv = Encoding.UTF8.GetBytes(aesIv);

                string plaintext = null;

                using (Rijndael rijAlg = Rijndael.Create())
                {
                    rijAlg.Key = key;
                    rijAlg.IV = iv;

                    rijAlg.Mode = CipherMode.CBC;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(_cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion


        #region RSA -  https://www.codeproject.com/Articles/10877/Public-Key-RSA-Encryption-in-C-NET | https://superdry.apphb.com/tools/online-rsa-key-converter | https://8gwifi.org/RSAFunctionality?keysize=2048 | https://cryptotools.net/rsagen

        /// <summary>
        /// Export xml rsa key
        /// </summary>
        /// <param name="xmlPrivateKeyString"></param>
        /// <param name="dwKeySize"></param>
        /// <param name="includePrivateKey"></param>
        /// <returns></returns>
        public static string RsaGetPublicKeyFromPrivateKey(string xmlPrivateKeyString, int dwKeySize, bool includePrivateKey = false)
        {
            using (var csp = new RSACryptoServiceProvider(dwKeySize))
            {
                csp.FromXmlString(xmlPrivateKeyString);
                return csp.ToXmlString(includePrivateKey);
            }
        }

        public static string RsaEncryptPkc1Padding(string plainText, string xmlPublicKey, int dwKeySize = 2048)
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

        public static string RsaDecryptPkc1Padding(string cypherText, string xmlPrivateKey, int dwKeySize = 2048)
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

        /*
        public static string GetXmlPrivateKey(string pemPrivateKey)
        {
            var pemPrivateKeyWithFormat = "-----BEGIN PRIVATE KEY-----" + Environment.NewLine + pemPrivateKey + Environment.NewLine + "-----END PRIVATE KEY-----";
            return MyUtility.Extensions.RsaKeyConverter.PemToXml(pemPrivateKeyWithFormat);
        }
         */

        #endregion

        #region SHA256WithRSA

        public static string SHA256WithRSASignData(string inputString, string privateKeyXmlString)
        {
            var signDataBytes = Encoding.UTF8.GetBytes(inputString);
            return SHA256WithRSASignData(signDataBytes, privateKeyXmlString);
        }

        public static string SHA256WithRSASignData(byte[] inputBytes, string privateKeyXmlString, int dwKeySize = 2048, bool toBase64 = true)
        {
            var xmlString = privateKeyXmlString;
            using (var rsa = new RSACryptoServiceProvider(dwKeySize))
            {
                var dataForSignAsBytes = inputBytes;
                rsa.FromXmlString(xmlString);

                var hashData = SHA256.Create().ComputeHash(dataForSignAsBytes);
                var sha256WithRsa = rsa.SignHash(hashData, CryptoConfig.MapNameToOID("SHA256"));
                return toBase64 ? Convert.ToBase64String(sha256WithRsa) : sha256WithRsa.Aggregate("", (current, t) => current + t.ToString("x2"));
            }
        }

        public static bool SHA256WithRSAVerifyData(string inputString, string signToVerification, string publicKeyXmlString, int dwKeySize = 2048, bool toBase64 = true)
        {
            var xmlString = publicKeyXmlString;
            var bytes = Encoding.UTF8.GetBytes(inputString);
            var byteArray = toBase64 ? Convert.FromBase64String(signToVerification) : StringHexToByteArray(signToVerification);
            using (var cryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize))
            {
                cryptoServiceProvider.FromXmlString(xmlString);
                var hashBytes = SHA256.Create().ComputeHash(bytes);
                var num = cryptoServiceProvider.VerifyHash(hashBytes, CryptoConfig.MapNameToOID("SHA256"), byteArray) ? 1 : 0;
                Console.WriteLine(num != 0);
                return num != 0;
            }
        }

        private static byte[] StringHexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }

        #endregion
    }


    public static class MaHoa
    {
        public static string Encrypt(string key, string data)
        {
            int keySize = key.Length;
            int dataSize = data.Length;
            string ret = "";
            for (int i = 0; i < dataSize; i++)
            {
                int nTemp = data[i] ^ key[i % keySize];
                string hexTemp = string.Format("{0:x4}", nTemp);
                ret += hexTemp;
            }
            return ret;
        }

        public static string Decrypt(string key, string data)
        {
            int keySize = key.Length;
            int dataSize = data.Length / 4;
            string ret = "";

            if (data.Length % 4 == 0)
            {
                for (int i = 0; i < dataSize; i++)
                {
                    string sub = data.Substring(i * 4, 4);
                    int value = Convert(sub);
                    int nTemp = value ^ key[i % keySize];
                    ret += (char)nTemp;
                }
            }

            return ret;
        }

        static sbyte[] unhex_table =
      { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
       ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
       ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
       , 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,-1,-1,-1,-1,-1,-1
       ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
       ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
       ,-1,10,11,12,13,14,15,-1,-1,-1,-1,-1,-1,-1,-1,-1
       ,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
      };

        private static int Convert(string hexNumber)
        {
            int decValue = unhex_table[(byte)hexNumber[0]];
            for (int i = 1; i < hexNumber.Length; i++)
            {
                decValue *= 16;
                decValue += unhex_table[(byte)hexNumber[i]];
            }
            return decValue;
        }
    }


    public static class RC4
    {
        public static string Encrypt(string key, string data)
        {
            var hex = StringToHex(data);
            var en = EnDeCrypt(key, hex);
            return StringToHex(en);
        }

        public static string Decrypt(string key, string data)
        {
            // not use.
            return data;
        }

        static private int N = 256;
        static private int[] sbox;
        static private string password;
        static private string text;

        public static string EnDeCrypt(string pass, string input)
        {
            password = pass;
            text = input;

            RC4Initialize();

            int i = 0, j = 0, k = 0;
            StringBuilder cipher = new StringBuilder();
            for (int a = 0; a < text.Length; a++)
            {
                i = (i + 1) % N;
                j = (j + sbox[i]) % N;
                int tempSwap = sbox[i];
                sbox[i] = sbox[j];
                sbox[j] = tempSwap;

                k = sbox[(sbox[i] + sbox[j]) % N];
                int cipherBy = ((int)text[a]) ^ k;  //xor operation
                cipher.Append(Convert.ToChar(cipherBy));
            }
            return cipher.ToString();
        }

        static private void RC4Initialize()
        {
            sbox = new int[N];
            int[] key = new int[N];
            int n = password.Length;
            for (int a = 0; a < N; a++)
            {
                key[a] = (int)password[a % n];
                sbox[a] = a;
            }

            int b = 0;
            for (int a = 0; a < N; a++)
            {
                b = (b + sbox[a] + key[a]) % N;
                int tempSwap = sbox[a];
                sbox[a] = sbox[b];
                sbox[b] = tempSwap;
            }
        }

        public static string StringToHex(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hex = BitConverter.ToString(bytes);
            var hex2 = hex.Replace("-", "");
            return hex2;
        }
    }
}