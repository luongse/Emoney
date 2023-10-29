using System.Collections.Generic;
using System.Linq;

namespace MyUtility.Extensions
{
    public static class SafeBase64UrlEncoder
    {
        private const string Plus = "+";
        private const string Minus = "-";
        private const string Slash = "/";
        private const string Underscore = "_";
        private const string EqualSign = "=";
        private const string Pipe = "|";
        private static readonly IDictionary<string, string> Mapper;

        static SafeBase64UrlEncoder()
        {
            Mapper = new Dictionary<string, string> { { Plus, Minus }, { Slash, Underscore }, { EqualSign, Pipe } };
        }

        /// <summary>
        /// Encode the base64 to safeUrl64
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string EncodeBase64Url(this string base64Str)
        {
            return string.IsNullOrEmpty(base64Str)
                ? base64Str
                : Mapper.Aggregate(base64Str, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

        /// <summary>
        /// Decode the Url back to original base64
        /// </summary>
        /// <param name="safe64Url"></param>
        /// <returns></returns>
        public static string DecodeBase64Url(this string safe64Url)
        {
            return string.IsNullOrEmpty(safe64Url)
                ? safe64Url
                : Mapper.Aggregate(safe64Url, (current, pair) => current.Replace(pair.Value, pair.Key));
        }
    }
}
