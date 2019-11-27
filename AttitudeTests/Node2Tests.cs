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
        public void NowImFeelingZombified()
        {
            var me = Child.IsBorn();

            Assert.IsTrue(
                me.Life.Status.Equals(Life.Alive));

            me.LifeUpdate();

            Assert.IsTrue(
                me.Life.Status.Equals(Life.Dead));

            me.SummonedByLichKing = true;

            me.LifeUpdate();

            Assert.IsTrue(
                me.Life.Status.Equals(Life.Undead));
        }

        private interface IExpires
        {
            bool Expired();
        }

        private class Child
        {
            private int UpdateTicks = 0;

            public void LifeUpdate()
            {
                UpdateTicks++;
                Life.Update();
            }

            public bool SummonedByLichKing { get; set; }

            public MappedState<Life> Life { get; private set; }

            public static Child IsBorn()
            {              
                var @this = new Child();

                @this.Life = MappedState<Life>.Construct(Node2Tests.Life.Alive)
                    .PathOf(
                        Node2Tests.Life.Alive
                            .LeadsTo(Node2Tests.Life.Dead)
                            .When(@this.Expired))
                    .PathOf(
                        Node2Tests.Life.Dead
                            .LeadsTo(Node2Tests.Life.Undead)
                            .When(() => @this.SummonedByLichKing));

                return @this;
            }

            public bool Expired() { return UpdateTicks > 0; }
        }

        public enum Life
        {
            Alive,
            Dead,
            Undead
        }
    }
}