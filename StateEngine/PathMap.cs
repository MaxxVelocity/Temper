using System;
using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    public class PathMap<T> where T : System.Enum
    {
        public List<UnidirectionalPath<T>> Paths { get; }

        private PathMap()
        {
            Paths = new List<UnidirectionalPath<T>>();
        }

        public static PathMap<T> Construct()
        {
            return new PathMap<T>();
        }

        public void AddPath(T origin, T destination, Func<bool> condition = null)
        {
            Paths.Add(UnidirectionalPath<T>.Construct(origin, destination));
        }

        public void Path(UnidirectionalPath<T> path)
        {
            Paths.Add(path);
        }

        public IEnumerable<T> DestinationsFor(T origin)
        {
            return Paths
                .Where(p => p.Origin.Equals(origin))
                .Select(p => p.Destination);
        }
    }
}
