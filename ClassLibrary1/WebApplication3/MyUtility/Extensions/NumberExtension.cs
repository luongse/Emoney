/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014 
 * Description: CommonLogger   
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/

using System;
using System.Globalization;

namespace MyUtility.Extensions
{
    public static class NumberExtension
    {

        //        public static string ToCurrencyString(this int number, string unit = "")
        //        {
        //            return ConvertUtility.FormatCurrency(number, unit);
        //        }

        public static string ToCurrencyString(this decimal number, bool enableShorten = false, bool showUnit = true, bool enableRound = true)
        {
            var unit = "đ";
            var format = "N00";

            if (enableRound == false)
            {
                if (showUnit)
                {
                    return number.ToString("#,##0.00") + unit;
                }
                return number.ToString("#,##0.00");
            }

            if (enableShorten)
            {
                if (number >= 1000000)
                {
                    number = number / (decimal)1000000;
                    unit = "tr";
                    format = "N01";
                }
                else if (number >= 1000)
                {
                    number = number / (decimal)1000;
                    unit = "k";
                    format = "N00";
                }
            }

            var currency = number.ToString(format, new CultureInfo("vi-VN"));

            return showUnit == false ? currency : string.Format("{0}{1}", currency, unit);
        }



        public static string ToCurrencyString(this decimal? number, bool enableShorten = false, bool showUnit = true, bool enableRound = true)
        {
            return ToCurrencyString(number.GetValueOrDefault(), enableShorten, showUnit, enableRound);
        }

        public static string ToCurrencyString(this int number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyString(this int? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        public static string ToCurrencyString(this long? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyString((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        /// <summary>
        /// format tiền tệ xu ThinhQHT
        /// </summary>
        /// <param name="number"></param>
        /// <param name="enableShorten"> </param>
        /// <param name="showUnit">= false để ko có chữ VND</param>
        /// <param name="enableRound"> = true để ko  có .00</param>
        /// <returns></returns>
        public static string ToCurrencyStringXu(this decimal number, bool enableShorten = false, bool showUnit = true, bool enableRound = true)
        {
            var unit = "";
            var format = "N00";

            if (enableRound == false)
            {
                if (showUnit)
                {
                    return number.ToString("#,##0.00") + unit;
                }
                return number.ToString("#,##0.00");
            }

            if (enableShorten)
            {
                if (number >= 1000000)
                {
                    number = number / (decimal)1000000;
                    unit = "tr";
                    format = "N01";
                }
                else if (number >= 1000)
                {
                    number = number / (decimal)1000;
                    unit = "k";
                    format = "N00";
                }
            }

            var currency = number.ToString(format, new CultureInfo("vi-VN"));

            return showUnit == false ? currency : string.Format("{0}{1}", currency, unit);
        }
        public static string ToCurrencyStringXu(this decimal? number, bool enableShorten = false, bool showUnit = true, bool enableRound = true)
        {
            return ToCurrencyStringXu(number.GetValueOrDefault(), enableShorten, showUnit, enableRound);
        }

        public static string ToCurrencyStringXu(this int number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this long number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number, enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this int? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        public static string ToCurrencyStringXu(this long? number, bool enableShorten = false, bool showUnit = true)
        {
            return ToCurrencyStringXu((decimal)number.GetValueOrDefault(0), enableShorten, showUnit);
        }

        #region format theo culture

        public static string FormatCoinCultureUs(this decimal myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureVN(this int myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureVN(this double myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureVN(this decimal myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureVN(this float myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureVN(this long myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureUs(this float myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
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
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }

        public static string FormatCoinCultureUs(this double myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-VN"), format, myCoin).Replace("₫", "").Trim();
        }
        public static string FormatCoinCultureUs(this long myCoin, int round = -1)
        {
            string format = "{0:C}";

            if (round >= 0)
                format = "{0:C" + round + "}";

            //return string.Format(CultureInfo.GetCultureInfo("en-us"), format, myCoin).Replace("$", "");
            return string.Format(CultureInfo.GetCultureInfo("vi-vn"), format, myCoin).Replace("₫", "").Trim();
        }
        #endregion


        public static bool IsNumber(string input)
        {
            try
            {
                return !string.IsNullOrEmpty(input) && !input.Contains(" ") && char.IsNumber(input, 0);
            }
            catch
            {
                return false;
            }
        }

        public static int ConvertStringToInt(string valueString, int radix)
        {
            var valueWithRadix = 0;
            
            foreach (var value in valueString)
            {
                valueWithRadix = valueWithRadix * radix;

                switch (value)
                {
                    case '0':
                        valueWithRadix += 0;
                        break;

                    case '1':
                        valueWithRadix += 1;
                        break;

                    case '2':
                        valueWithRadix += 2;
                        break;

                    case '3':
                        valueWithRadix += 3;
                        break;

                    case '4':
                        valueWithRadix +=4;
                        break;

                    case '5':
                        valueWithRadix += 5;
                        break;

                    case '6':
                        valueWithRadix += 6;
                        break;

                    case '7':
                        valueWithRadix += 7;
                        break;

                    case '8':
                        valueWithRadix += 8;
                        break;

                    case '9':
                        valueWithRadix += 9;
                        break;

                    case 'a':
                    case 'A':
                        valueWithRadix += 10;
                        break;

                    case 'b':
                    case 'B':
                        valueWithRadix += 11;
                        break;

                    case 'c':
                    case 'C':
                        valueWithRadix += 12;
                        break;

                    case 'd':
                    case 'D':
                        valueWithRadix += 13;
                        break;

                    case 'e':
                    case 'E':
                        valueWithRadix += 14;
                        break;

                    case 'f':
                    case 'F':
                        valueWithRadix += 15;
                        break;

                    default:
                        valueWithRadix += 0;
                        break;
                }
            }

            return valueWithRadix;
        }
    }
}
