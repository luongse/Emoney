using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using MyUtility.Extensions;

namespace MyUtility
{
    public static class DateTimeExtension
    {
        #region Variables

        public enum DateFormat
        {
            [Description("dd/MM/yyyy")]
            NgayThangNam = 1,

            [Description("yyyy/MM/dd")]
            NamThangNgay = 2,

            [Description("MM/dd/yyyy")]
            ThangNgayNam = 3,
        }

        public enum MonthFormat
        {
            [Description("MM/yyyy")]
            MonthYear = 1,
        }

        public enum TimeFormat
        {
            [Description("H:mm:ss")]
            Hmmss,

            [Description("HH:mm:ss")]
            HHmmss,

            [Description("H:mm:ss.fff")]
            Hmmssfff,

            [Description("HH:mm:ss.fff")]
            HHmmssfff,
        }

        public enum SpecifierFormat
        {
            [Description("R")]
            RFC1123,     // ddd, dd MMM yyyy HH':'mm':'ss 'GMT' (*)

            [Description("yyyy-MM-dd'T'HH:mm:ssZ")]
            ISO8601,
        }

        #endregion

        #region Chuyển từ chuỗi sang thời gian

        /// <summary>
        /// Chuyển từ chuỗi sang ngày tháng
        /// </summary>
        /// <param name="datetimeString"></param>
        /// <param name="dateFormat"></param>
        /// <param name="timeFormat"></param>
        /// <param name="isGetDateIfError">Nếu chuyển sang dạng thời gian bị lỗi thì tự động chuyển sang dạng ngày</param>
        /// <returns></returns>
        public static DateTime ParseExact(string datetimeString, DateFormat dateFormat, TimeFormat timeFormat, bool isGetDateIfError = false)
        {
            if (isGetDateIfError)
            {
                try
                {
                    return ParseExactGeneral(datetimeString, dateFormat, timeFormat);
                }
                catch (Exception)
                {
                    return ParseExact(datetimeString, dateFormat);
                }
            }
            return ParseExactGeneral(datetimeString, dateFormat, timeFormat);
        }

        /// <summary>
        /// Chuyển từ chuỗi sang ngày
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime ParseExact(string dateTimeString, DateFormat format)
        {
            try { return DateTime.ParseExact(dateTimeString, format.Text(), null); }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Chuyển từ chuỗi sang ngày và thời gian
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="dateFormat"></param>
        /// <param name="timeFormat"></param>
        /// <returns></returns>
        public static DateTime ParseExactGeneral(string dateTimeString, DateFormat dateFormat, TimeFormat timeFormat)
        {
            var formatString = dateFormat.Text() + " " + timeFormat.Text();
            try { return DateTime.ParseExact(dateTimeString, formatString, null); }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Format thời gian

        /// <summary>
        /// TanPVD: 2015/01/07
        /// </summary>
        /// <param name="dt"></param>
        /// format datetime
        /// <returns></returns>
        public static string FormatDateTime(object dt)
        {
            if (dt == null)
                return string.Empty;
            var datetime = dt.ToString();
            TimeSpan dateBetween = DateTime.Now - DateTime.Parse(datetime);
            if (dateBetween.Days < 1)
            {
                return string.Format("{0} giờ {1} phút", dateBetween.Hours, dateBetween.Minutes);
            }
            return (DateTime.Parse(datetime)).ToString(DateFormat.NgayThangNam.Text());
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated: 20/01/2015</para>
        /// chuyển giờ thành định dạng giờ-phút
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string ConvertHouseToHouseMinute(double minute)
        {
            return (
                        minute >= 60 ? String.Format(@"{0} Giờ {1} Phút", Math.Floor(minute / 60), Math.Floor(minute % 60))
                        : (String.Format(@"{0} Phút", Math.Floor(minute % 60)))
                    );
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated:02/02/2015</para>
        /// <para>Description: lấy totalSeconds tính đến thời điểm hiện tại</para>
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double ConvertTimesToTotalSeconds(DateTime date)
        {
            var span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return Math.Round(span.TotalSeconds, 0);
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated:02/02/2015</para>
        /// <para>Description: tính độ lệch thời gian so với hiện tại</para>
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="eslapedMinutes"></param>
        /// <returns></returns>
        public static bool GetEslapedMinutes(double stime, ref double eslapedMinutes)
        {
            try
            {
                var timeZone = TimeZone.CurrentTimeZone;
                var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                var senderTotalSeconds = stime + timeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
                var senderTotalSecondsTimeSpan = TimeSpan.FromSeconds(senderTotalSeconds);

                var receiverDiffTimeSpan = DateTime.UtcNow - origin;
                var tsResult = receiverDiffTimeSpan - senderTotalSecondsTimeSpan;
                eslapedMinutes = tsResult.TotalMinutes;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Extension

        /// <summary>
        /// Lấy format ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetVnDateFormat(this DateTime? date)
        {
            return date.HasValue ? GetVnDateFormat(date.Value) : string.Empty;
        }

        /// <summary>
        /// Lấy format ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetVnDateTimeFormat(this DateTime? date)
        {
            return date.HasValue ? GetVnDateTimeFormat(date.Value) : string.Empty;
        }

        /// <summary>
        /// Lấy format ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetVnDateFormat(this DateTime date)
        {
            return date.ToString(DateFormat.NgayThangNam.Text());
        }

        /// <summary>
        /// Lấy format ngày tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetVnDateTimeFormat(this DateTime date)
        {
            var formatString = DateFormat.NgayThangNam.Text() + " " + TimeFormat.HHmmss.Text();
            return date.ToString(formatString);
        }

        /// <summary>
        /// Lấy format tháng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetMonthFormat(this DateTime date)
        {
            return date.ToString(MonthFormat.MonthYear.Text());
        }

        /// <summary>
        /// Lấy Unix time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Lấy Unix time
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Lấy Utc time từ chuỗi Unix
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime? UnixTimeStampToUtcDateTime(this string unixTimeStamp)
        {
            if (string.IsNullOrEmpty(unixTimeStamp))
                return null;

            int result = 0;
            if (int.TryParse(unixTimeStamp, out result) == false)
                return null;

            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(result).ToUniversalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Lấy Local time từ chuỗi Unix
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime? UnixTimeStampToLocalDateTime(this string unixTimeStamp)
        {
            if (string.IsNullOrEmpty(unixTimeStamp))
                return null;

            int result = 0;
            if (int.TryParse(unixTimeStamp, out result) == false)
                return null;

            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(result).ToLocalTime();
            return dtDateTime;
        }

        public static double GetUnixTimeStamp(this DateTime date)
        {
            var span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return Math.Round(span.TotalSeconds, 0);
        }

        public static long GetLongUnixTimeStamp(this DateTime date)
        {
            var span = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return Convert.ToInt64(Math.Round(span.TotalMilliseconds, 0));

            //var span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return Convert.ToInt64(Math.Round(span.TotalSeconds, 0));
        }

        public static DateTime ConvertSeccondToDateTime(this double data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }
        public static DateTime ConvertSeccondToDateTime(this int data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }
        public static DateTime ConvertSeccondToDateTime(this long data)
        {
            var tempDate = DateTime.Now;
            try
            {
                tempDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data);
            }
            catch (Exception)
            {
            }

            return tempDate;
        }
        public static double ConvertDateTimeToSeccond(this DateTime data)
        {
            var seccond = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            try
            {
                seccond = (data - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            catch (Exception)
            {

            }
            return seccond;
        }
        public static double ConvertDateTimeToSeccond(this DateTime? data)
        {
            DateTime datas = data ?? DateTime.Now;
            var seccond = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            try
            {
                seccond = (datas - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            }
            catch (Exception)
            {

            }
            return seccond;
        }


        //private static double GetUnixTimeStamp(string date)
        //{
        //    var d = DateTime.Parse(date);
        //    return GetUnixTimeStamp(d);
        //}

        //private static DateTime GetUnixTimeStamp(double ts)
        //{
        //    var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    var tt = origin.AddSeconds(ts).ToLocalTime();
        //    return tt;
        //}

        /// <summary>
        /// Lay ngay dau tien trong tuan cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDateOfWeek(this DateTime date)
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            DayOfWeek dOfWeek = info.Calendar.GetDayOfWeek(date);
            var h = new Hashtable();
            h["Sunday"] = 6;
            h["Monday"] = 0;
            h["Tuesday"] = 1;
            h["Wednesday"] = 2;
            h["Thursday"] = 3;
            h["Friday"] = 4;
            h["Saturday"] = 5;
            double indexOfday = double.Parse(h[dOfWeek.ToString()].ToString());
            var tmpDate = date.AddDays(-indexOfday);
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
        }

        /// <summary>
        /// Lay ngay cuoi cung trong tuan cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDateOfWeek(this DateTime date)
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            DayOfWeek dOfWeek = info.Calendar.GetDayOfWeek(date);
            var h = new Hashtable();
            h["Sunday"] = 6;
            h["Monday"] = 0;
            h["Tuesday"] = 1;
            h["Wednesday"] = 2;
            h["Thursday"] = 3;
            h["Friday"] = 4;
            h["Saturday"] = 5;
            double indexOfday = double.Parse(h[dOfWeek.ToString()].ToString());
            var tmpDate = date.AddDays(6 - indexOfday);
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, 23, 59, 59);
        }

        /// <summary>
        /// Ngay dau thang cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Ngay cuoi thang cua mot ngay bat ky
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime date)
        {
            var tmpDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, 23, 59, 59);
        }

        public static List<int> MonthOfYear()
        {
            return new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day);
        }

        public static DateTime EndOfDate(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated: 21/03/2015</para>
        /// <para>Lấy số tuần hiện tại theo DateTime</para>
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekNumber(this DateTime dt)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                dt = dt.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            //return weekNo;
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated: 21/03/2015</para>
        /// <para>Lấy danh sách số tuần theo năm</para>
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<Week> GetWeeksOfTheYear(this int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var beginningDayOfWeek = firstDayOfYear.AddDays(-1 * Convert.ToInt32(firstDayOfYear.DayOfWeek));
            var endingDayOfWeek = beginningDayOfWeek.AddDays(6);
            var weekOfYear = 1;
            var weeksOfTheYear = new List<Week>();

            while (beginningDayOfWeek.Year < year + 1)
            {
                var week = new Week { Number = weekOfYear, BeginningOfWeek = beginningDayOfWeek };
                weeksOfTheYear.Add(week);

                beginningDayOfWeek = beginningDayOfWeek.AddDays(7);
                weekOfYear++;
            }

            return weeksOfTheYear;
        }

        public class Week
        {
            public DateTime BeginningOfWeek { get; set; }
            public DateTime EndOfWeek { get { return this.BeginningOfWeek.AddDays(6); } }
            public int Number { get; set; }
            public string Text { get { return this.ToString(); } }

            public override string ToString()
            {
                return DateTime.Now > BeginningOfWeek && DateTime.Now < EndOfWeek
                    ? String.Format(
                        "Week {0} or current week: {1} - {2}",
                        this.Number,
                        this.BeginningOfWeek.ToShortDateString(),
                        this.EndOfWeek.ToShortDateString())
                    : String.Format(
                        "Week {0}: {1} - {2}",
                        this.Number,
                        this.BeginningOfWeek.ToShortDateString(),
                        this.EndOfWeek.ToShortDateString());
            }
        }


        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {

            var jan1 = new DateTime(year, 1, 1);
            if (weekOfYear == 1)
                return jan1;
            CultureInfo _culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            CultureInfo _uiculture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

            _culture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;
            _uiculture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

            Thread.CurrentThread.CurrentCulture = _culture;
            Thread.CurrentThread.CurrentUICulture = _uiculture;
            var daysOffset = (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            var firstMonday = jan1.AddDays(daysOffset);

            var firstWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated: 10/04/2015</para>
        /// <para>Gets the 12:00:00 instance of a DateTime</para>
        /// </summary>
        public static DateTime GetBeginOfDay(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// <para>Author: </para>
        /// <para>DateCreated: 10/04/2015</para>
        /// <para>Gets the 11:59:59 instance of a DateTime</para>
        /// </summary>
        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return GetBeginOfDay(dateTime).AddDays(1).AddSeconds(-1);
        }
        #endregion

        public static string CountDownTime(this DateTime? date, string formatHours = "h", string formatMinute = "")
        {
            if (!date.HasValue)
                return "";
            TimeSpan remaindate = date.Value - DateTime.Now;
            if (remaindate.TotalHours < 24)
            {
                return string.Format("{0}{1} {2}{3}", remaindate.Hours, formatHours, remaindate.ToString("mm"), formatMinute); // time.ToString(@"hhhmm");
            }
            else
            {
                return Math.Floor(remaindate.TotalDays).ToString(CultureInfo.InvariantCulture) + " ngày";
            }
        }

        public static DateTime GetDate19700101
        {
            get { return DateTime.Parse("1970/01/01"); }
        }

        public static DateTime ToDateTimeParseExact(this string text, string format = "dd/MM/yyyy HH:mm:ss")
        {
            return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
        }
        public static DateTime ToDateTimeParseExactEn(this string text, string format = "MM/dd/yyyy HH:mm:ss")
        {
            return DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
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
    }
}
