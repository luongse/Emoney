using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MyUtility.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Author:  
        /// <para>Format chuoi theo nhom VD: 434.345.334</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numberOfPart"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string FormatAsGroup(this string str, int numberOfPart = 4, string separator = ".")
        {
            str = Regex.Replace(str, string.Format("(?!^).{{{0}}}", numberOfPart), string.Format("{0}$0", separator), RegexOptions.RightToLeft);
            return str;

        }
        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        /// <summary>
        /// Author:  
        /// <para>Cat chuoi theo length quy dinh</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string MakeSortString(this string str, int length)
        {
            if (str.Length < length)
                length = str.Length;
            string s = str.Substring(0, length);
            if (s.Length < str.Length && s.LastIndexOf(' ') > 0)
                s = str.Substring(0, s.LastIndexOf(' '));
            if (str.Length > s.Length)
                s += "...";
            return s;
        }

        /// <summary>
        /// Author:  
        /// <para>Chuyen chuoi thanh int number</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultInt = 0)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultInt;
            }
        }
        //public static decimal ToDecimal(this string str, decimal _default = 0)
        //{
        //    try
        //    {
        //        return decimal.Parse(str);
        //    }
        //    catch
        //    {
        //        return _default;
        //    }
        //}
        public static long ToLong(this string str)
        {
            return long.Parse(str);
        }
        public static string TextId(this string str, int length = 100)
        {
            return StringCommon.GetTextID(str, length);
        }

        /// <summary>
        /// Author:  
        /// <para>
        /// Chuyen so thanh chu
        /// </para>
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConverToNumberToString(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            var str = doc.ToCharArray();
            var result = "";
            for (int l = 0; l < str.Length; l++)
            {
                if (l == 0)
                {
                    result += str[l].ToString().ToUpper();
                }
                else
                {
                    result += str[l];
                }
            }
            return result;
        }

        public static string FormatDateTime(this string datetime)
        {
            TimeSpan dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1)
            {
                return string.Format("{0} giờ {1} phút", dateBetween.Hours, dateBetween.Minutes);
            }
            return (DateTime.Parse(datetime)).ToString("dd/MM/yyyy");
        }

        public static DateTime ToDateTimeParseExact(this string text, DateTime defaultError, string format = "dd/MM/yyyy HH:mm:ss")
        {
            try
            {
                return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultError;
            }
        }

        public static string ToVNdate(this DateTime? date, string format = "dd/MM/yyyy")
        {
            if (date.HasValue)
                return date.Value.ToString(format);
            return "";
        }

        public static string FormatDateTimeHhmmss(this string datetime)
        {
            TimeSpan dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1)
            {
                return string.Format("{0} giờ {1} phút", dateBetween.Hours, dateBetween.Minutes);
            }
            return (DateTime.Parse(datetime)).ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        ///   : Loai bo dau tieng viet
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToUnSign(string s)
        {
            s = s.Replace("\"", " ");
            s = Regex.Replace(s, @"\s+", " ");
            s = s.Replace(@"/", " ");
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = s.Normalize(NormalizationForm.FormD);
            var str = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
            return str.Trim().Replace(" ", "-");
        }

        public static string CleanUpWordHtml(string html)
        {
            html = MakeSafe(html);
            html = FixEntityCharacters(html);

            NameValueCollection nvc = new NameValueCollection();

            nvc.Add(@"<!--(\w|\W)+?-->", string.Empty);
            nvc.Add(@"<title>(\w|\W)+?</title>", string.Empty);
            nvc.Add(@"<style>(\w|\W)+?</style>", string.Empty);
            nvc.Add(@"<script\s[^>]*>.+</script>", string.Empty);
            //#Begin Hoang them
            nvc.Add(@"<script (\w|\W)+?\><script>", string.Empty);
            nvc.Add(@"<object.*>.*<object>", string.Empty);
            //nvc.Add(@"<embed (\w|\W)+?>", string.Empty);
            nvc.Add(@"\[flash\](\w|\W)+?\[/flash\]", string.Empty);
            nvc.Add(@"\[music\](\w|\W)+?\[/music\]", string.Empty);
            nvc.Add(@"\[media\](\w|\W)+?\[/media\]", string.Empty);
            nvc.Add(@"<video>(\w|\W)+?</video>", string.Empty);
            nvc.Add(@"<form>(\w|\W)+?</form>", string.Empty);
            nvc.Add(@"<iframe (\w|\W)+?>(\w|\W)+?</iframe>", string.Empty);
            //nvc.Add(@"<div .*>.*<img (\w|\W)* />", "<div>");
            nvc.Add(@"<input.*/>", string.Empty);
            nvc.Add(@"<input.*></input>", string.Empty);
            nvc.Add(@"<div.*style=.*absolute.*>", "<div");
            //nvc.Add(@"<? background", string.Empty);
            //#End Hoang them
            nvc.Add(@"\s?class=\w+", string.Empty);
            nvc.Add(@"(font-family:[^&gt;]*[;'])|(font-size:[^&gt;]*[;'])|(line-height:[^&gt;]*[;'])|(MsoNormal)|(&lt;!--\[if.*?&lt;!\[endif\]--&gt;)", string.Empty);

            nvc.Add(@"<(meta|link|/?o:|/?st\d|/?head|/?html|body|/?body|!\[)[^>]*?>", string.Empty);
            nvc.Add(@"(<[^>]+>)+&nbsp;(</\w+>)+", string.Empty);
            nvc.Add(@"\s+v:\w+=""[^""]+""", string.Empty);
            nvc.Add(@"(\n\r){2,}", string.Empty);

            nvc.Add(@"<[/]?(xml|del|ins|[ovwxp]:\w+)[^>]*?>", string.Empty);
            nvc.Add(@"<([^>]*)(?:lang|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>");

            foreach (string key in nvc.Keys)
                html = Regex.Replace(html, key, nvc[key], RegexOptions.None);

            return html.Trim();
        }

        public static string MakeSafe(string value)
        {
            StringBuilder v = new StringBuilder(value);

            v
                .Replace("&Agrave;", "À")
                .Replace("&Aacute;", "Á")
                .Replace("&Acirc;", "Â")
                .Replace("&Atilde;", "Ã")
                .Replace("&Ccedil;", "Ç")
                .Replace("&Egrave;", "È")
                .Replace("&Eacute;", "É")
                .Replace("&Ecirc;", "Ê")
                .Replace("&Igrave;", "Ì")
                .Replace("&Iacute;", "Í")
                .Replace("&Icirc;", "Î")
                .Replace("&ETH;", "Ð")
                .Replace("&Ograve;", "Ò")
                .Replace("&Oacute;", "Ó")
                .Replace("&Ocirc;", "Ô")
                .Replace("&Otilde;", "Õ")
                .Replace("&Ugrave;", "Ù")
                .Replace("&Uacute;", "Ú")
                .Replace("&Yacute;", "Ý")
                .Replace("&agrave;", "à")
                .Replace("&aacute;", "á")
                .Replace("&acirc;", "â")
                .Replace("&atilde;", "ã")
                .Replace("&aring;", "å")
                .Replace("&ccedil;", "ç")
                .Replace("&egrave;", "è")
                .Replace("&eacute;", "é")
                .Replace("&ecirc;", "ê")
                .Replace("&igrave;", "ì")
                .Replace("&iacute;", "í")
                .Replace("&icirc;", "î")
                .Replace("&ograve;", "ò")
                .Replace("&oacute;", "ó")
                .Replace("&ocirc;", "ô")
                .Replace("&otilde;", "õ")
                .Replace("&ugrave;", "ù")
                .Replace("&uacute;", "ú")
                .Replace("&ucirc;", "û")
                .Replace("&yacute;", "ý");

            return v.ToString();
        }

        private static string FixEntityCharacters(string html)
        {
            NameValueCollection nvc = new NameValueCollection();

            nvc.Add("“", "&ldquo;");
            nvc.Add("”", "&rdquo;");
            nvc.Add("–", "&mdash;");

            foreach (string key in nvc.Keys)
                html = html.Replace(key, nvc[key]);

            return html;
        }

        public const string BRTag = "<br />";


        public static string RemoveAndReplaceAttribute(string input, bool isReplaceBrTag)
        {
            string strRet = input;

            if (isReplaceBrTag)
            {
                strRet = strRet.Replace(BRTag + "\r\n", BRTag).Replace("\r\n", BRTag).Replace("\r", BRTag).Replace("\n", BRTag);
            }


            strRet = ReplaceHtmlTag(strRet); // xoa cac the nguy hiem

            return strRet;
        }


        /// <summary>
        /// xoa cac the html khong cho phep va chan ky tu xau
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ReplaceHtmlTag(string input)
        {
            string strRet = input;

            strRet = CleanUpWordHtml(strRet.Trim());

            // replace the html
            strRet = ReplaceHtmlTag(strRet);
            if (strRet == string.Empty)
                return string.Empty;

            return strRet;
        }

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

        private static readonly Random Rand = new Random();
        public static string RandomStringNumber(int length)
        {
            const string chars = "0123456789";
            var charArray = Enumerable.Repeat(chars, length).Select(s => s[Rand.Next(s.Length)]).ToArray();
            return new string(charArray);
        }

        public static string FormatCoin(double pCoin)
        {
            if (pCoin < 10D)
                return pCoin.ToString(new CultureInfo("vi-VN"));

            var strRet = string.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin);
            if (strRet == "00") { strRet = "0"; }
            return strRet;
        }

        /// <summary>
        /// Author: 
        /// tạo chuỗi bất kỳ
        /// </summary>
        /// <returns></returns>
        public static string GenarateRequestId()
        {
            return DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-03-17</para>
        /// <para>Description: Cat chuoi neu chuoi qua dai</para>
        /// </summary>
        /// <returns></returns>
        public static string CutNick(string stringToCut, int lengthCut, string stringReplace)
        {
            var lengthString = stringToCut.Length;

            if (lengthString > 0 && lengthString > lengthCut)
            {
                var stringTemp = stringToCut.Substring(0, lengthCut - stringReplace.Length) + stringReplace;
                return stringTemp;
            }

            return stringToCut;
        }


        public static string RemoveSpecialCharacters(this string value)
        {
            const string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[~`+=" + "\"";
            char[] specialCharactersArray = specialCharacters.ToCharArray();
            return new String(value.Except(specialCharactersArray).ToArray());
        }

        public static string RemoveSpecialCharacters_v2(this string str)
        {
            return Regex.Replace(str, @"[^a-zA-Z0-9\-ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ_{1}\s]", string.Empty);
        }

        public static string ReplaceEndString(this string text, int length = 3, string charStringReplace = "*")
        {
            if (string.IsNullOrEmpty(text)) return "";

            if (text.Length > length)
                text = text.Substring(0, text.Length - length);
            else
            {
                length = text.Length;
                text = "";
            }

            for (var i = 0; i < length; i++)
            {
                text += charStringReplace;
            }
            return text;
        }

        /// <summary>
        /// <para>Author: TrungTT</para>
        /// <para>Date: 2015-07-15</para>
        /// <para>Description: Chuyen doi ti le cuoc bong da</para>
        /// </summary>
        /// <returns></returns>
        public static string ConvertToRatioFootball(double betRate)
        {
            var betrateTemp = betRate;
            if (betRate < 0)
            {
                var betrateSplit = betRate.ToString().Split(new[] { '-' });
                betRate = double.Parse(betrateSplit[1]);
            }
            var strBetRate = "";
            var temp = betRate.ToString().Split(new[] { '.' });
            if (temp.Length < 2)
                strBetRate = betRate.ToString();
            else
            {
                if (betRate < 1)
                {
                    if (betRate > 0 && betRate < 0.5)
                        strBetRate = "1/4";
                    if (betRate == 0.5)
                        strBetRate = "1/2";
                    if (betRate > 0.5 && betRate < 1)
                        strBetRate = "3/4";
                }
                else
                {
                    var strConvert = "0." + temp[1];
                    if (double.Parse(strConvert) > 0 && double.Parse(strConvert) < 0.5)
                        strBetRate = temp[0] + " 1/4";
                    if (double.Parse(strConvert) == 0.5)
                        strBetRate = temp[0] + " 1/2";
                    if (double.Parse(strConvert) > 0.5 && double.Parse(strConvert) < 1)
                        strBetRate = temp[0] + " 3/4";
                }
            }
            if (betrateTemp < 0)
                return "-" + strBetRate;
            return strBetRate;
        }

        public static string ConvertToBase64(this string text)
        {
            var val = SecurityCommon.EncryptBase64(text);
            return string.IsNullOrEmpty(val) ? val : val.EncodeBase64Url();
        }

        public static string ConvertFromBase64(this string text, bool isDecrypt = false)
        {
            if (!isDecrypt) return text;

            var val = SecurityCommon.DecryptBase64(text);
            return string.IsNullOrEmpty(val) ? val : val.DecodeBase64Url();
        }

        #region format theo culture

        public static string FormatCoinCultureUs(this decimal myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this int myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this double myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this decimal myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this float myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this long myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this float myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        /// <summary>
        /// <para>-1: sẽ có 2 số sau dấu .</para>
        /// <para>0: sẽ ko có số sau dấu .</para>
        /// <para>>0: sẽ có >0 số sau dấu .</para>
        /// </summary>
        /// <param name="myCoin"></param>
        /// <param name="round"></param>
        /// <returns></returns>
        public static string FormatCoinCultureUs(this int myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this double myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this long myCoin, int round = -1)
        {
            var format = "{0:C}";
            if (round >= 0)
                format = "{0:C" + round + "}";
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }

        #endregion
    }
}
