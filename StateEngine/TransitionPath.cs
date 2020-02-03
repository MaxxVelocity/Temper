using System;

namespace StateEngine
{
    public class TransitionPath<T> where T : System.Enum
    {
        public T Origin { get; protected set; }

        public T Destination { get; protected set; }

        public static TransitionPath<T> Construct(T origin, T destination)
        {
            return new TransitionPath<T>()
            {
                Origin = origin,
                Destination = destination
            };
        }
    }
}