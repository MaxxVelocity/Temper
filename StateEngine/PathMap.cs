using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void AddPath(T origin, T destination, Func<bool> condition = null, int priority = 1)
        {
            EnforceUniquePriority(origin, priority);

            Paths.Add(UnidirectionalPath<T>.Construct(origin, destination, priority));
        }

        public void Path(UnidirectionalPath<T> path)
        {
            EnforceUniquePriority(path.Origin, path.Priority);
            Paths.Add(path);
        }

        private void EnforceUniquePriority(T origin, int priority)
        {
            if (Paths.Any(n => n.Origin.Equals(origin) && n.Priority.Equals(priority)))
            {
                throw new ArgumentException($"The map for type {typeof(T)} already includes a path from {origin} with a priority of {priority}.");
            }
        }

        public IEnumerable<T> DestinationsFor(T origin)
        {
            return Paths
                .Where(p => p.Origin.Equals(origin))
                .Select(p => p.Destination);
        }
    }
}
