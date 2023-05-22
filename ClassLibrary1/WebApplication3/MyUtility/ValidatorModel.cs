/**********************************************************************
 * Author: ThongNT
 * DateCreate: 06-25-2014 
 * Description: ValidatorModel use to validate model manual  
 * ####################################################################
 * Author:......................
 * DateModify: .................
 * Description: ................
 * 
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MyUtility
{
    public class ValidatorModel
    {
        /// <summary>
        /// Author: ThongNT
        /// <para></para>
        /// Validate bussiness rule of model that use DataAnnotations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<string> Validate(object model)
        {
            var type = model.GetType();
            var properties = model.GetType().GetProperties();

            return (from propertyInfo in properties
                    let customAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true)
                    from customAttribute in customAttributes
                    let validationAttribute = (ValidationAttribute)customAttribute
                    let isValid = validationAttribute.IsValid(propertyInfo.GetValue(model, BindingFlags.GetProperty, null, null, null))
                    where !isValid
                    select validationAttribute.ErrorMessage).ToList();
        }

        /// <summary>
        /// <para>Author:TrungLD</para>
        /// <para>DateCreated: 20/04/2015</para>
        /// <para>kiểm tra kiểu số từ chuỗi</para>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(expression), System.Globalization.NumberStyles.Number, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static class Validator
        {
            public static IEnumerable<string> Validate(object o)
            {
                return TypeDescriptor
                    .GetProperties(o.GetType())
                    .Cast<PropertyDescriptor>()
                    .SelectMany(pd => pd.Attributes.OfType<ValidationAttribute>()
                                        .Where(va => !va.IsValid(pd.GetValue(o))))
                                        .Select(xx => xx.ErrorMessage);
            }
        }
    }
}