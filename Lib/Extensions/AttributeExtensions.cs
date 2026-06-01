using System;
using System.Linq;

namespace SoundEffect.Lib.Extensions
{
    internal static class AttributeExtensions
    {
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            return enumValue.GetType()
                .GetField(enumValue.ToString())
                ?.GetCustomAttributes(typeof(T), false)
                .FirstOrDefault() as T;
        }
    }
}
