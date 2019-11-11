using System;
using System.Collections.Generic;
using System.Text;

namespace StateEngine
{
    public abstract class Node
    {
        public Guid UniqueId { get; }

        public int CyclesElapsed { get; private set; }

        public string Description { get; }

        public List<ConditionalAttitude> PotentialTransitions { get; }

        public Node(string description)
        {
            Description = description;

            UniqueId = Guid.NewGuid();
        }

        public Node Update()
        {
            CyclesElapsed++;
            return this;
        }
    }

    public class PotentialTransition
    {
        public Node Origin { get; }

        public Node Destination { get; }

        public Func<bool> Condition { get; }

        public PotentialTransition(Node origin, Node destination)
        {
            Origin = origin;
            Destination = destination;
        }
    }

    public class ConditionalAttitude
    {
        public Node Behavior { get; }

        public Func<Node, bool> Condition { get; }

        public ConditionalAttitude(Node behavior, Func<Node, bool> condition)
        {
            Behavior = behavior;
            Condition = condition;
        }
    }

    public static class Conditions
    {
        public static Func<bool> CurrentAttitudeExpired(Node currentAttidue)
        {
            return new Func<bool>(() => currentAttidue.CyclesElapsed >= 30);
        }
    }
}