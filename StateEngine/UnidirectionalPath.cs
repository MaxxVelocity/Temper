using System;

namespace StateEngine
{
    public class UnidirectionalPath<T> where T : System.Enum
    {
        public T Origin { get; private set; }

        public T Destination { get; private set; }

        public Func<bool> Condition { get; internal protected set; }

        public static UnidirectionalPath<T> Construct(T origin, T destination, Func<bool> conditon = null)
        {
            return new UnidirectionalPath<T>()
            {
                Origin = origin,
                Destination = destination,
                Condition = conditon
            };
        }
    }
}