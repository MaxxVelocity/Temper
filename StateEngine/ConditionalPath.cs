using System;
using System.Collections.Generic;
using System.Text;

namespace StateEngine
{
    public class ConditionalPath<T> : TransitionPath<T>, IPrioritizedTransitionPath where T : Enum
    {
        public Func<bool> Condition { get; internal protected set; }

        public uint Priority { get; protected set; }

        public static ConditionalPath<T> Construct(T origin, T destination, uint priority = 1, Func<bool> condition = null)
        {
            return new ConditionalPath<T>
            {
                Condition = condition,
                Priority = priority,
                Origin = origin,
                Destination = destination
            };
        }
    }
}
