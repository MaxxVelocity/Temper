using System;
using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    public class StatusMapNode<T>
    {
        private readonly T Enum;

        public StatusMapNode(T @type)
        {
            Enum = type;
        }
    }

    public class NodeFactory<T> where T : System.Enum
    {
        public Type Enum { get; set; }

        public NodeFactory()
        {
            this.Enum = typeof(T);
        }

        public StatusMapNode<T> Construct(T type)
        {
            var product = new StatusMapNode<T>(type);

            return product;
        }
    }



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

    public static class Node2Extensions
    {
        public static UnidirectionalPath<T> LeadsTo<T>(this T origin, T destination) where T : System.Enum
        {
            return UnidirectionalPath<T>.Construct(origin, destination);
        }

        public static bool CanOnlyLeadTo<T>(this IEnumerable<T> paths, T destination) where T : System.Enum
        {
            return paths.Where(p => p.Equals(destination)).Count() == 1;
        }

        public static UnidirectionalPath<T> When<T>(this UnidirectionalPath<T> path, Func<bool> condition) where T : System.Enum
        {
            path.Condition = condition;
            return path;
        }
    }
}