using System;

namespace StateEngine
{
    public static class PathExtensions
    {
        /// <summary>
        /// Constructs an expiry path
        /// </summary>
        public static ExpiryPath<T> ExpiresTo<T>(this T origin, T destination, uint countdown) where T : System.Enum
        {
            return ExpiryPath<T>.Construct(origin, destination, countdown);
        }

        /// <summary>
        /// Constructs a conditional path. The Condition will not yet be populated upon construction.
        /// </summary>
        public static ConditionalPath<T> LeadsTo<T>(this T origin, T destination) where T : System.Enum
        {
            return ConditionalPath<T>.Construct(origin, destination);
        }

        /// <summary>
        /// Mutates a constructed ConditionalPath, populating the Condition via internal property setter. 
        /// </summary>
        public static ConditionalPath<T> When<T>(this ConditionalPath<T> path, Func<bool> condition) where T : System.Enum
        {
            path.Condition = condition;
            return path;
        }
    }
}