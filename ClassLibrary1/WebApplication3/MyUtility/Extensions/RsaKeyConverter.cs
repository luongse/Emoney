using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace MyUtility.Extensions
{
    public static class RsaKeyConverter
    {
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static AsymmetricCipherKeyPair GetKeyPair(this RSA rsa)
        {
            try
            {
                return DotNetUtilities.GetRsaKeyPair(rsa);
            }
            catch
            {
                return null;
            }
        }

        public static RsaKeyParameters GetPublicKey(this RSA rsa)
        {
            try
            {
                return DotNetUtilities.GetRsaPublicKey(rsa);
            }
            catch
            {
                return null;
            }
        }

        public static string XmlToPem(string xml)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromXmlString(xml);

                var keyPair = rsa.GetKeyPair(); // try get private and public key pair
                if (keyPair != null) // if XML RSA key contains private key
                {
                    var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                    return FormatPem(privateKeyInfo.GetEncoded().ToBase64(), "RSA PRIVATE KEY");
                }

                var publicKey = rsa.GetPublicKey(); // try get public key
                if (publicKey == null) throw new InvalidKeyException("Invalid RSA Xml Key");

                var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
                return FormatPem(publicKeyInfo.GetEncoded().ToBase64(), "PUBLIC KEY");
            }
        }

        //public static async Task<string> XmlToPemAsync(string xml)
        //{
        //    return await Task.Run(() => XmlToPem(xml));
        //}

        private static string FormatPem(string pem, string keyType)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("-----BEGIN {0}-----\n", keyType);

            int line = 1, width = 64;

            while ((line - 1) * width < pem.Length)
            {
                var startIndex = (line - 1) * width;
                var len = line * width > pem.Length
                              ? pem.Length - startIndex
                              : width;
                sb.AppendFormat("{0}\n", pem.Substring(startIndex, len));
                line++;
            }

            sb.AppendFormat("-----END {0}-----\n", keyType);
            return sb.ToString();
        }

        public static string PemToXml(string pem)
        {
            if (pem.StartsWith("-----BEGIN RSA PRIVATE KEY-----")
                || pem.StartsWith("-----BEGIN PRIVATE KEY-----"))
            {
                return GetXmlRsaKey(pem, obj =>
                {
                    if ((obj as RsaPrivateCrtKeyParameters) != null)
                        return DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)obj);
                    var keyPair = (AsymmetricCipherKeyPair)obj;
                    return DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters)keyPair.Private);
                }, rsa => rsa.ToXmlString(true));
            }

            if (pem.StartsWith("-----BEGIN PUBLIC KEY-----"))
            {
                return GetXmlRsaKey(pem, obj =>
                {
                    var publicKey = (RsaKeyParameters)obj;
                    return DotNetUtilities.ToRSA(publicKey);
                }, rsa => rsa.ToXmlString(false));
            }

            throw new InvalidKeyException("Unsupported PEM format...");
        }

        //public static async Task<string> PemToXmlAsync(string pem)
        //{
        //    return await Task.Run(() => PemToXml(pem));
        //}

        private static string GetXmlRsaKey(string pem, Func<object, RSA> getRsa, Func<RSA, string> getKey)
        {
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms))
            using (var sr = new StreamReader(ms))
            {
                sw.Write(pem);
                sw.Flush();
                ms.Position = 0;
                var pr = new PemReader(sr);
                var keyPair = pr.ReadObject();
                using (var rsa = getRsa(keyPair))
                {
                    var xml = getKey(rsa);
                    return xml;
                }
            }
        }

        public static string PemToXml(string pem, bool isPrivateKey, bool autoAppend = false)
        {
            if (!autoAppend) return PemToXml(pem);

            var pemWithFormat = isPrivateKey
                ? "-----BEGIN RSA PRIVATE KEY-----" + Environment.NewLine + pem + Environment.NewLine + "-----END RSA PRIVATE KEY-----"
                : "-----BEGIN PUBLIC KEY-----" + Environment.NewLine + pem + Environment.NewLine + "-----END PUBLIC KEY-----";

            if (!isPrivateKey) return PemToXml(pemWithFormat);

            try
            {
                return PemToXml(pemWithFormat);
            }
            catch
            {
                pemWithFormat = "-----BEGIN PRIVATE KEY-----" + Environment.NewLine + pem + Environment.NewLine + "-----END PRIVATE KEY-----";
                return PemToXml(pemWithFormat);
            }
        }
    }
}
