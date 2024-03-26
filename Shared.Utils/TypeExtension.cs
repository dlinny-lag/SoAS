using System;

namespace Shared.Utils
{
    public static class TypeExtension
    {
        public static bool ExtractGenericArguments(this Type t, Type genericDefinition, out Type[] genericArguments)
        {
            Type curType = t;
            do
            {
                if (curType.IsGenericType && curType.GetGenericTypeDefinition() == genericDefinition)
                {
                    genericArguments = curType.GetGenericArguments();
                    return true;
                }

                curType = curType.BaseType;
            } while (curType != null);

            genericArguments = null;
            return false;
        }
    }
}