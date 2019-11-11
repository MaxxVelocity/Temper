using System;
using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    public class Graph
    {
        public List<Node> Nodes { get; }

        public List<PotentialTransition> Relations { get; }

        public Graph()
        {
            Nodes = new List<Node>();
            Relations = new List<PotentialTransition>();
        }

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void AddRelation(Node source, Node target)
        {
            if(!(Nodes.Contains(source) 
                && Nodes.Contains(target)))
            {
                throw new ArgumentException("Cannot relate nodes which are not already added to the graph.");
            }

            Relations.Add(new PotentialTransition(source, target));
        }

        public void AddRelation(PotentialTransition potentialTransition)
        {
            Relations.Add(potentialTransition);
        }

        public IEnumerable<Node> GetTransitionTargets(Node node)
        {
            var relations = Relations
                .Where(n => n.Origin.Equals(node));

            return relations
                .Select(n => n.Destination);
        }
    }

    public static class Extensions
    {
        public static PotentialTransition Becomes(this Node node, Node destination)
        {
            return new PotentialTransition(node, destination);
        }
    }
}