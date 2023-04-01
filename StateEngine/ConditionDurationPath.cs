using System;

namespace StateEngine
{
    public class ConditionDurationPath<T> : ConditionalPath<T> where T : Enum
    {
        public uint DurationThreshold { get; internal set; }

        public static ConditionDurationPath<T> Construct(T origin, T destination, Func<bool> condition, uint duration)
        {
            return new ConditionDurationPath<T>
            {
                Condition = condition,
                DurationThreshold = duration,
                Origin = origin,
                Destination = destination
            };
        }
    }

    public static class ConditionExtensions
    {
        public static ConditionDurationPath<T> AfterDurationOf<T>(this ConditionalPath<T> path, uint ticks) where T : Enum
        {
            return ConditionDurationPath<T>.Construct(      
                condition : path.Condition,
                destination : path.Destination,
                duration : ticks,
                origin : path.Origin             
            );
        }
    }
}
