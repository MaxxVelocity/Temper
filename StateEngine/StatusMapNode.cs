﻿using System;
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




    public static class UnidirectionalPathExtensions
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