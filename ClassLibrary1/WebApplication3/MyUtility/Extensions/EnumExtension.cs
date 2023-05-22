/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014 
 * Description: EnumExtension   
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MyUtility.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Author: ThongNT
        /// <para>Lay ten Enum hoac Description cua Enum</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static string Text(this Enum eEnum)
        {
            try
            {
                var fi = eEnum.GetType().GetField(eEnum.ToString());

                if (fi != null)
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return attributes.Any() ? attributes[0].Description : eEnum.ToString();
                }
                return eEnum.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// Author: ThongTM
        /// <para>Lay ten Enum hoac Description cua Enum theo thứ tự indexOfAttribute</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <param name="indexOfAttribute"></param>
        /// <returns></returns>
        public static string Text(this Enum eEnum, int indexOfAttribute)
        {
            try
            {
                var fi = eEnum.GetType().GetField(eEnum.ToString());

                if (fi != null)
                {
                    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return attributes.Any() ? attributes[indexOfAttribute].Description : eEnum.ToString();
                }
                return eEnum.ToString();
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// Author: ThongNT
        /// <para>Lay value cua Enum</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static int Value(this Enum eEnum)
        {
            var changeType = Convert.ChangeType(eEnum, eEnum.GetTypeCode());
            if (changeType != null)
                return (int)changeType;
            return -9999;
        }

        /// <summary>
        /// Author: ThongNT
        /// <para>Covert String to Enum object</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object enumString, bool ignoreCase = false)
        {
            return (T)Enum.Parse(typeof(T), enumString.ToString(), ignoreCase);
        }

        public static T ToEnumFromDescription<T>(this object enumString, bool isIgnoreCase = false)
        {
            try
            {
                var descriptionAttribute = isIgnoreCase
                    ? typeof(T).GetMembers()
                        .Where(m => m.GetCustomAttribute<DescriptionAttribute>() != null &&
                                    m.GetCustomAttribute<DescriptionAttribute>().Description.Equals((string)enumString, StringComparison.OrdinalIgnoreCase))
                        .Select(m => m)
                        .FirstOrDefault()
                    : typeof(T).GetMembers()
                        .Where(m => m.GetCustomAttribute<DescriptionAttribute>() != null &&
                                    m.GetCustomAttribute<DescriptionAttribute>().Description == (string)enumString)
                        .Select(m => m)
                        .FirstOrDefault();

                return descriptionAttribute != null
                    ? ToEnum<T>(descriptionAttribute.Name, isIgnoreCase)
                    : ToEnum<T>(enumString.ToString(), isIgnoreCase);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Author: ThongNT
        /// <para>Convert number value to Enum</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static T NumberToEnum<T>(this int enumValue)
        {
            return (T)Enum.ToObject(typeof(T), enumValue);
        }

        public static T NumberToEnum<T>(this int enumValue, T defaultValue)
        {
            try
            {
                return (T)Enum.ToObject(typeof(T), enumValue);
            }
            catch
            {
                return defaultValue;
            }
        }
        public static List<EnumToList> ToList(this Type eNum)
        {
            var enumValues = Enum.GetValues(eNum).Cast<Enum>();

            var items = (from enumValue in enumValues
                         select new EnumToList
                         {
                             Key = enumValue.Value(),
                             Value = enumValue.Text()
                         }).ToList();

            return items;
        }
    }

    public class EnumToList
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
