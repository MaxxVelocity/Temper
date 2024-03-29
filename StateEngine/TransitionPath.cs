﻿using System;

namespace StateEngine
{
    public class TransitionPath<T> where T : System.Enum
    {
        public T Origin { get; internal set; }

        public T Destination { get; internal set; }

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