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
        /// Author: ThongNT
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

        /// <summary>
        /// Author: ThongNT
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
        /// Author: ThongNT
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
        public static decimal ToDecimal(this string str, decimal _default = 0)
        {
            try
            {
                return decimal.Parse(str);
            }
            catch
            {
                return _default;
            }
        }
        public static long ToLong(this string str)
        {
            return long.Parse(str);
        }
        public static string TextId(this string str)
        {
            return StringCommon.GetTextID(str, 100);
        }

        /// <summary>
        /// Author: ThongNT
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

        /// <summary>
        /// ThongNT : Loai bo dau tieng viet
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

        public static string FormatCoin(long? pCoin)
        {
            string StrRet = String.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin.HasValue ? pCoin.Value : 0);
            if (StrRet == "00") { StrRet = "0"; }
            return StrRet;
        }

        public static string FormatCoin(double? pCoin)
        {
            string StrRet = String.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin.HasValue ? pCoin.Value : 0);
            if (StrRet == "00") { StrRet = "0"; }
            return StrRet;
        }

        public static string FormatCoin(int? pCoin)
        {
            var pCoinValue = pCoin.GetValueOrDefault(0);

            if (pCoinValue < 10) return pCoinValue.ToString();
            string StrRet = String.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoinValue);
            if (StrRet == "00") { StrRet = "0"; }
            return StrRet;

            //string StrRet = String.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin.HasValue ? pCoin.Value : 0);
            //if (StrRet == "00") { StrRet = "0"; }
            //return StrRet;
        }

        public static string FormatCoin(decimal? pCoin)
        {
            string StrRet = String.Format(new CultureInfo("vi-VN"), "{0:0,0}", pCoin.HasValue ? pCoin.Value : 0);
            if (StrRet == "00") { StrRet = "0"; }
            return StrRet;
        }

        /// <summary>
        /// Author:TrungLD
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

        public static string RemoveSpecialCharacters_v3(this string str)
        {
            try
            {
                return Regex.Replace(str, "[^0-9a-zA-Z]+", string.Empty); ;
            }
            catch
            {
                return str;
            }
        }

        /// <summary>
        /// dinh dang so dien thoai theo chuan quoc te
        /// <para>vd : 0909123456 => 84909123456</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToStandardPhone(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (str.StartsWith("0"))
            {
                str = "84" + str.Remove(0, 1);
            }
            return str;
        }

        /// <summary>
        /// dinh dang so dien thoai
        /// <para>vd : 84909123456 => 0909123456</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReStandardPhone(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (str.StartsWith("84"))
            {
                str = "0" + str.Remove(0, 2);
            }
            return str;
        }

        /// <summary>
        /// replace new line
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringReplace"></param>
        /// <returns></returns>
        public static string ReplaceNewLine(this string str, string stringReplace)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            return str
                .Replace("\r\n", stringReplace)
                .Replace("\n", stringReplace)
                .Replace(Environment.NewLine, stringReplace);
        }

        /// <summary>
        /// FullImgPath
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="imageHost"></param>
        /// <param name="defaultAvatar"></param>
        /// <param name="isSecureRequest"></param>
        /// <returns></returns>
        public static string FullImgPath(this string imgPath, string imageHost, string defaultAvatar, bool isSecureRequest = false)
        {
            imgPath = string.IsNullOrEmpty(imgPath) ? defaultAvatar : imgPath;

            if (imgPath.ToLower().Contains("http://") || imgPath.ToLower().Contains("https://"))
                return imgPath;

            var result = imageHost + imgPath;

            if (isSecureRequest)
                result = result.Replace("http://", "https://");

            return result;
        }

        /// <summary>
        /// giấu ký tự
        /// <para>0: abcdwxyz -> ***dwxyz</para>
        /// <para>1: abcdwxyz -> abcdw***</para>
        /// <para>2: abcdwxyz -> abc*****</para>
        /// <para>3: abcdwxyz -> *****xyz</para>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position">0: left hide; 1: right hide; 2: left show; 3: right show</param>
        /// <param name="replaceString"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string HiddenText(this string input, int position = 0, string replaceString = "*", int length = 3)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var _replaceString = new String(Convert.ToChar(replaceString), length);

            if (input.Length <= length)
            {
                switch (position)
                {
                    case 1:
                    case 2:
                        return string.Concat(input, _replaceString);
                    default:
                        return string.Concat(_replaceString, input);
                }
            }

            switch (position)
            {
                case 1:
                    return string.Concat(input.Substring(0, input.Length - length), _replaceString);
                case 2:
                    return string.Concat(input.Substring(0, length), new String(Convert.ToChar(replaceString), input.Length - length));
                case 3:
                    return string.Concat(new String(Convert.ToChar(replaceString), input.Length - length), input.Substring(input.Length - length, length));
                default:
                    return string.Concat(_replaceString, input.Substring(length, input.Length - length));
            }
        }


        private static readonly Random MyRand = new Random();
        public static string RandomImei()
        {
            const int imeiLength = 15;
            var str = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var sum = 0;

            // Fill in the first two values of the string based with the specified prefix.
            // Reporting Body Identifier list: http://en.wikipedia.org/wiki/Reporting_Body_Identifier
            var rbi = new[] { "01", "10", "30", "33", "35", "44", "45", "49", "50", "51", "52", "53", "54", "86", "91", "98", "99" };
            var arr = rbi[MyRand.Next(rbi.Length)];
            str[0] = Convert.ToInt32(arr[0]);
            str[1] = Convert.ToInt32(arr[1]);
            var pos = 2;

            // Fill all the remaining numbers except for the last one with random values.
            while (pos < imeiLength - 1)
            {
                str[pos++] = MyRand.Next(10) % 10;
            }

            // Calculate the Luhn checksum of the values thus far.
            const int lenOffset = (imeiLength + 1) % 2;
            for (pos = 0; pos < imeiLength - 1; pos++)
            {
                if ((pos + lenOffset) % 2 == 1)
                {
                    var t = str[pos] * 2;
                    if (t > 9)
                        t -= 9;

                    sum += t;
                }
                else
                {
                    sum += str[pos];
                }
            }

            // Choose the last digit so that it causes the entire string to pass the checksum.
            var finalDigit = (10 - (sum % 10)) % 10;
            str[imeiLength - 1] = finalDigit;

            // Output the IMEI value.
            var imeiOutput = string.Join("", str);
            imeiOutput = imeiOutput.Substring(0, imeiLength);
            return imeiOutput;
        }
    }
}
