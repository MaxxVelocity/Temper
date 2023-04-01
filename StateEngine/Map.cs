using System;
using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    //TODO: make possible to assign same map to multiple object instances.

    public class Map<T> where T : System.Enum
    {
        public List<ConditionalPath<T>> Paths { get; }

        public List<ExpiryPath<T>> ExpiryPaths { get; }

        private Map()
        {
            Paths = new List<ConditionalPath<T>>();
            ExpiryPaths = new List<ExpiryPath<T>>();
        }

        public static Map<T> Construct()
        {
            return new Map<T>();
        }

        public Map<T> PathOf(ConditionalPath<T> path)
        {
            Path(path);
            return this;
        }

        public Map<T> PathOf(ExpiryPath<T> path)
        {
            Path(path);
            return this;
        }

        public void Path(ConditionalPath<T> path)
        {
            Paths.Add(path);
        }

        public void Path(ExpiryPath<T> path)
        {
            EnforceUniqueExpiry(path.Origin);
            ExpiryPaths.Add(path);
        }

        public IEnumerable<T> DestinationsFor(T origin)
        {
            return Paths
                .Where(p => p.Origin.Equals(origin))
                .Select(p => p.Destination);
        }

        private void EnforceUniqueExpiry(T origin)
        {
            if (ExpiryPaths.Any(n => n.Origin.Equals(origin)))
            {
                throw new ArgumentException($"The map for type {typeof(T)} already includes a expiry path from {origin}. A map can only have one expiry per status node.");
            }
        }

        // Priority is being abandoned for now. Conditions should be broad enough to include competing factors in the evaluation criteria. 
        private void EnforceUniquePriority(T origin, uint priority)
        {
              if (Paths.Any(n => n.Origin.Equals(origin) && n.Priority.Equals(priority)))
            {
                throw new ArgumentException($"The map for type {typeof(T)} already includes a path from {origin} with a priority of {priority}.");
            }
        }
    }
}