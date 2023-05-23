﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace MyUtility
{
    public static class ObjectExtension
    {
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(anonymousObject.GetType()))
                expando.Add(property.Name, property.GetValue(anonymousObject));
            return expando as ExpandoObject;
        }

        public static NameValueCollection ToNameValueCollection<T>(this T dynamicObject)
        {
            var nameValueCollection = new NameValueCollection();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynamicObject))
            {
                string value = propertyDescriptor.GetValue(dynamicObject) == null ? string.Empty : propertyDescriptor.GetValue(dynamicObject).ToString();
                nameValueCollection.Add(propertyDescriptor.Name, value);
            }
            return nameValueCollection;
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
