using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Msar_Gded.Helpers
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GetSelectList<TEnum>() where TEnum : struct, Enum
        {
            var list = new List<SelectListItem>();

            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var field = typeof(TEnum).GetField(value.ToString());
                var displayAttribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                    .FirstOrDefault() as DisplayAttribute;

                list.Add(new SelectListItem
                {
                    Value = ((int)value).ToString(),
                    Text = displayAttribute != null ? displayAttribute.Name : value.ToString()
                });
            }

            return list;
        }


        public static string GetDisplayName(this Enum enumValue)
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString());

            if (memberInfo.Length > 0)
            {
                var displayAttr = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                    return displayAttr.Name;
            }

            return enumValue.ToString();
        }
    }
}
