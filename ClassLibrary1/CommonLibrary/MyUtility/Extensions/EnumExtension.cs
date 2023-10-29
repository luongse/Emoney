/**********************************************************************
 * Author:  
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
using System.Linq;
using System.Reflection;

namespace MyUtility.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Author:  
        /// <para>Lay ten Enum hoac Description cua Enum</para>
        /// </summary>
        /// <param name="eEnum"></param>
        /// <returns></returns>
        public static string Text(this Enum eEnum)
        {
            var fi = eEnum.GetType().GetField(eEnum.ToString());

            if (fi == null)
                return eEnum.Value().ToString();

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Any() ? attributes[0].Description : eEnum.ToString();
        }

        /// <summary>
        /// Author:  
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
        /// Author:  
        /// <para>Covert String to Enum object</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this object enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString.ToString());
        }

        public static T ToEnum<T>(this int enumValue, T defaultValue)
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

        public static T ToEnumDesc<T>(this string enumDescriptionString)
        {
            var memInfo = typeof(T)
                .GetMembers()
                .FirstOrDefault(m => m.GetCustomAttribute<DescriptionAttribute>() != null
                                     && m.GetCustomAttribute<DescriptionAttribute>()
                                         .Description.Equals(enumDescriptionString, StringComparison.CurrentCultureIgnoreCase));
            return memInfo != null && !string.IsNullOrEmpty(memInfo.Name)
                ? memInfo.Name.ToEnum<T>()
                : default(T);
        }

        /// <summary>
        /// Author:  
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

        public static List<EnumToList<T>> ToListWithCustomAttribute<T>(this Type eNum) where T : Attribute
        {
            var enumValues = Enum.GetValues(eNum).Cast<Enum>();

            var items = (from enumValue in enumValues
                         select new EnumToList<T>
                         {
                             Key = enumValue.Value(),
                             Value = enumValue.Text(),
                             Attribute = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttribute<T>(),
                             Attributes = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes<T>().ToList()
                         }).ToList();

            return items;
        }
    }

    public class EnumToList
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class EnumToList<T>
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public T Attribute { get; set; }
        public List<T> Attributes { get; set; }
    }
}