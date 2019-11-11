using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttitudeTests
{
    [TestClass]
    public class Node2Tests
    {
        
        [TestMethod]
        public void FactoryTest()
        {
            var factory = new NodeFactory<Life>();

            var instance = factory.Construct(Life.Alive);

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void UnidirectionalPathTest()
        {
            var path = UnidirectionalPath<Life>.Construct(Life.Alive, Life.Dead);

            Assert.IsNotNull(path);
        }

        [TestMethod]
        public void PathMapTest()
        {
            var map = PathMap<Life>.Construct();

            map.AddPath(Life.Alive, Life.Dead);

            Assert.IsNotNull(map);

            var destinations = map.Paths
                .Where(p => p.Origin == Life.Alive)
                .Select(p => p.Destination);
        }

        [TestMethod]
        public void ToLiveIsToDie()
        {
            var courseOfLife = PathMap<Life>.Construct();

            courseOfLife.Path(Life.Alive.LeadsTo(Life.Dead));

            Assert.IsTrue(courseOfLife
                    .DestinationsFor(Life.Alive)
                    .CanOnlyLeadTo(Life.Dead));
        }

        [TestMethod]
        public void MyTimeShallCome()
        {
            var me = Child.IsBorn();

            me.Life.PathOf(
                Life.Alive
                    .LeadsTo(Life.Dead)
                    .When(me.TimeHasCome));

            Assert.IsTrue(
                me.Life.Status.Equals(Life.Alive));

            me.GetsOld().LifeUpdate();

            Assert.IsTrue(
                me.Life.Status.Equals(Life.Dead));           
        }

        private class Child
        {
            private bool dying;

            public void LifeUpdate() => Life.Update();

            public MappedState<Life> Life { get; private set; }

            public static Child IsBorn()
            {              
                return new Child
                {
                    Life = MappedState<Life>.Construct(),
                    dying = false,
                };
            }

            public Child GetsOld()
            {
                dying = true;
                return this;
            }

            public bool TimeHasCome() { return dying; }
        }

        public enum Life
        {
            Alive,
            Dead,
        }
    }


}