using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StateEngine
{
    public class PathMap<T> where T : System.Enum
    {
        public List<ConditionalPath<T>> Paths { get; }

        public List<ExpiryPath<T>> ExpiryPaths { get; }

        private PathMap()
        {
            Paths = new List<ConditionalPath<T>>();
            ExpiryPaths = new List<ExpiryPath<T>>();
        }

        public static PathMap<T> Construct()
        {
            return new PathMap<T>();
        }

        public void AddPath(T origin, T destination, Func<bool> condition = null)
        {
            Paths.Add(ConditionalPath<T>.Construct(origin, destination));
        }

        public void Path(ConditionalPath<T> path)
        {
            EnforceUniquePriority(path.Origin, path.Priority);
            Paths.Add(path);
        }

        public void Path(ExpiryPath<T> path)
        {
            ExpiryPaths.Add(path);
        }

        private void EnforceUniquePriority(T origin, uint priority)
        {
              if (Paths.Any(n => n.Origin.Equals(origin) && n.Priority.Equals(priority)))
            {
                throw new ArgumentException($"The map for type {typeof(T)} already includes a path from {origin} with a priority of {priority}.");
            }
        }

        private void EnforceUniqueExpiry(T origin)
        {
            if (ExpiryPaths.Any(n => n.Origin.Equals(origin)))
            {
                throw new ArgumentException($"The map for type {typeof(T)} already includes a expiry path from {origin}. A map can only have one expiry per status node.");
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