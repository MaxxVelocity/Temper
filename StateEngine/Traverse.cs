using System;
using System.Collections.Generic;

namespace StateEngine
{
    public class Traverse<T> where T : System.Enum
    { 
        public Map<T> Map { get; private set; }

        public IEnumerable<T> Destinations => Map.DestinationsFor(Location);

        public T Location { get; set; }

        public static Traverse<T> Construct()
        {
            return new Traverse<T>();
        }

        public static Traverse<T> ForMap(Map<T> map)
        {
            var construct = new Traverse<T>() { Map = map };
            return construct;
        }

        public Traverse<T> StartingAt(T location)
        {
            Location = location;
            return this;
        }
    }
}