using System;

namespace StateEngine
{
    public class UnidirectionalPath<T> where T : System.Enum
    {
        public T Origin { get; private set; }

        public T Destination { get; private set; }

        public Func<bool> Condition { get; internal protected set; }

        public int Priority { get; private set; }

        public static UnidirectionalPath<T> Construct(T origin, T destination, int priority = 1, Func<bool> condition = null)
        {
            return new UnidirectionalPath<T>()
            {
                Origin = origin,
                Destination = destination,
                Condition = condition,
                Priority = priority
            };
        }
    }
}