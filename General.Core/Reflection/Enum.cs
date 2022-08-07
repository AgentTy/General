using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Reflection
{
    public class EnumDisplayAttribute : Attribute
    {
        public readonly string DisplayName;
        public readonly bool IsDefaultValue;
        public readonly bool IsDropDownListOption;

        public EnumDisplayAttribute(string DisplayName, bool IsDefaultValue, bool IsDropDownListOption) { this.DisplayName = DisplayName; this.IsDefaultValue = IsDefaultValue; this.IsDropDownListOption = IsDropDownListOption; }
        public override string ToString()
        {
            return "DisplayName = " + DisplayName.ToString() + "\r\n" +
                "IsDefaultValue = " + IsDefaultValue.ToString() +
                "IsDropDownListOption = " + IsDropDownListOption.ToString();
        }
    }


    public class EnumSerializer
    {

        #region Serialize
        /// <summary>
        /// written by Ty Hansen 1/22/2015    
        /// These methods will populate a DropDownList with the values of any C# Enumeration, it lets you use custom attributes to tell the DropDownList what to say and what Default Value to select.
        /// Here is a sample Enumeration
        /*
        public enum DataSharingModeEnum
        {
            [EnumDisplayAttribute("Unspecified", true)]
            [Description("Default Value: Data sharing preference has not been assigned")]
            Unspecified = 0,

            [EnumDisplayAttribute("MSA 2.5 YES", false)]
            [Description("MSA 2.5 YES, Organization agrees to share data")]
            MSA_25_YES = 1,

            [EnumDisplayAttribute("MSA 2.5 NO", false)]
            [Description("MSA 2.5 NO, Organization declines to share data")]
            MSA_25_NO = 2
        }
        */
        /// </summary>
        /// <typeparam name="TEnum">Any enumeration, with or without custom attributes.</typeparam>
        /// <param name="inDropdown">An ASP.Net DropDownList that you want to populate from the enumeration</param>
        public static List<EnumItem> Serialize<TEnum>()
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new
                         {
                             Obj = e,
                             Name = e.ToString(),
                             Value = Convert.ToInt32(e),
                             EnumDisplayAttribute = ((EnumDisplayAttribute)e.GetType().GetMember(e.ToString()).Where(member => member.MemberType == System.Reflection.MemberTypes.Field).FirstOrDefault().GetCustomAttributes(typeof(EnumDisplayAttribute), false).FirstOrDefault()),
                             DescriptionAttribute = ((System.ComponentModel.DescriptionAttribute)e.GetType().GetMember(e.ToString()).Where(member => member.MemberType == System.Reflection.MemberTypes.Field).FirstOrDefault().GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false).FirstOrDefault())
                         };

            return (from e in values select new EnumItem
                         {
                             Name = e.Name,
                             Value = e.Value,
                             DisplayName = e.EnumDisplayAttribute != null ? e.EnumDisplayAttribute.DisplayName : e.Name,
                             IsDefaultValue = e.EnumDisplayAttribute != null ? e.EnumDisplayAttribute.IsDefaultValue : false,
                             IsDropDownListOption = e.EnumDisplayAttribute != null ? e.EnumDisplayAttribute.IsDropDownListOption : true,
                             Description = e.DescriptionAttribute != null ? e.DescriptionAttribute.Description : ""
                         }).ToList();
        }

        public static string GetDisplayName<TEnum>(TEnum e)
        {
            var attr = ((EnumDisplayAttribute)e.GetType().GetMember(e.ToString()).Where(member => member.MemberType == System.Reflection.MemberTypes.Field).FirstOrDefault().GetCustomAttributes(typeof(EnumDisplayAttribute), false).FirstOrDefault());
            if (attr != null && !String.IsNullOrWhiteSpace(attr.DisplayName))
                return attr.DisplayName;
            return e.ToString();
        }

        public struct EnumItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public string DisplayName;
            public bool IsDefaultValue;
            public bool IsDropDownListOption;
            public string Description { get; set; }
        }
        #endregion
    }
}
