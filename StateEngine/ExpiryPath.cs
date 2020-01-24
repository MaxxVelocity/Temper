using System;
using System.Collections.Generic;

namespace StateEngine
{
    public class ExpiryPath<T> : TransitionPath<T> where T:Enum
    {
        public uint Countdown { get; protected set; }

        public static ExpiryPath<T> Construct(T origin, T destination, uint countdown)
        {
            return new ExpiryPath<T>
            {
                Countdown = countdown,
                Origin = origin,
                Destination = destination
            };
        }
    }
}
