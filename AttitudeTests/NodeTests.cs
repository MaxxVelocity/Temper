using StateEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AttitudeTests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void EverythingDies()
        {
            var alive = new Alive1();
            var dead = new Dead1();

            var graph = new Graph();
            graph.AddNode(alive);
            graph.AddNode(dead);
            graph.AddRelation(alive.Becomes(dead));

            var transitionTargets = graph.GetTransitionTargets(alive);
            Assert.IsTrue(transitionTargets.Contains(dead));
        }
    }

    public class Alive1 : Node
    {
        public Alive1() : base("I'm not dead yet.")
        {
        }
    }

    public class Dead1 : Node
    {
        public Dead1() : base("Sorry, we're dead. Come back later.")
        {
        }
    }
}