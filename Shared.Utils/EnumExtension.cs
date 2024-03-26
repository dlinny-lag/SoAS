using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shared.Utils
{
    public static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, bool> enumFlagsCache = new ConcurrentDictionary<Type, bool>();

        private static bool IsFlagEnum<T>()
            where T:Enum
        {
            return enumFlagsCache.GetOrAdd(typeof(T), t => t.IsDefined(typeof(FlagsAttribute), false));
        }

        private static class EnumHandler<T>
            where T: Enum
        {
            public static readonly Func<T, bool> IsNone;
            static EnumHandler()
            {
                IsNone = GetIsNone();
            }
            private static Func<T, bool> GetIsNone()
            {
                var arg = Expression.Parameter(typeof(T));
                Type intType = Enum.GetUnderlyingType(typeof(T));
                var argConvert = Expression.Convert(arg, intType);
                var eqZero = Expression.Equal(argConvert, Expression.Constant(0, intType));
                return Expression.Lambda<Func<T, bool>>(eqZero, arg).Compile();
            }
        }

        private static class FlagEnumHandler<T>
            where T: Enum
        {
            public static readonly Func<T, T, T> Or;
            public static readonly Func<T, bool> IsNone;
            public static readonly Func<T, T, bool> HasAnyFlag;

            static FlagEnumHandler()
            {
                if (!typeof(T).IsEnum || !typeof(T).IsDefined(typeof(FlagsAttribute), false))
                    throw new InvalidOperationException($"{typeof(T).FullName} must be a flag enum");
                Or = GetOr();
                IsNone = EnumHandler<T>.IsNone;
                HasAnyFlag = GetHasAnyFlag();
            }

            static Func<T, T, T> GetOr()
            {
                var aArg = Expression.Parameter(typeof(T));
                var bArg = Expression.Parameter(typeof(T));
                Type intType = Enum.GetUnderlyingType(typeof(T));
                var aConvert = Expression.Convert(aArg, intType);
                var bConvert = Expression.Convert(bArg, intType);
                var orRes = Expression.Or(aConvert, bConvert);
                var retConvert = Expression.Convert(orRes, typeof(T));
                return Expression.Lambda<Func<T, T, T>>(retConvert, aArg, bArg).Compile();
            }
           

            private static Func<T, T, bool> GetHasAnyFlag()
            {
                var valueArg = Expression.Parameter(typeof(T));
                var flagArg = Expression.Parameter(typeof(T));
                Type intType = Enum.GetUnderlyingType(typeof(T));
                var intValue = Expression.Convert(valueArg, intType);
                var intFlag = Expression.Convert(flagArg, intType);
                var and = Expression.And(intValue, intFlag);
                var greaterZero = Expression.GreaterThan(and, Expression.Constant(0, intType));

                return Expression.Lambda<Func<T, T, bool>>(greaterZero, valueArg, flagArg).Compile();
            }
        }

        public static T Or<T>(this T value, T anotherValue)
            where T:Enum
        {
            return FlagEnumHandler<T>.Or(value, anotherValue);
        }

        public static bool IsNone<T>(this T value)
            where T:Enum
        {
             return EnumHandler<T>.IsNone(value);
        }

        public static bool HasAnyOfFlags<T>(this T value, params T[] flags)
            where T:Enum
        {
            if (value.IsNone())
                return false;
            if (flags.Length == 0)
                return false;
            T allFlags = default(T);
            for (int i = 0; i < flags.Length; i++)
                allFlags = allFlags.Or(flags[i]);

            return FlagEnumHandler<T>.HasAnyFlag(value, allFlags);
        }
    }
}