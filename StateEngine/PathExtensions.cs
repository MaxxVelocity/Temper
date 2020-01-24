using System;
using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    public static class PathExtensions
    {
        public static ExpiryPath<T> ExpiresTo<T>(this T origin, T destination, uint countdown) where T : System.Enum
        {
            return ExpiryPath<T>.Construct(origin, destination, countdown);
        }

        public static ConditionalPath<T> LeadsTo<T>(this T origin, T destination) where T : System.Enum
        {
            return ConditionalPath<T>.Construct(origin, destination);
        }

        public static ConditionalPath<T> When<T>(this ConditionalPath<T> path, Func<bool> condition) where T : System.Enum
        {
            path.Condition = condition;
            return path;
        }

        public static ConditionalPath<T> When2<T>(this ConditionalPath<T> path, Func<bool> condition) where T : System.Enum
        {
            path.Condition = condition;
            return path;
        }

        public static bool CanOnlyLeadTo<T>(this IEnumerable<T> paths, T destination) where T : System.Enum
        {
            return paths.Where(p => p.Equals(destination)).Count() == 1;
        }
    }
}