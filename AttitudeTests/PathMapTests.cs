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
    public class PathMapTests
    {
        
        //[TestMethod]
        //public void FactoryTest()
        //{
        //    var factory = new NodeFactory<Life>();

        //    var instance = factory.Construct(Life.Alive);

        //    Assert.IsNotNull(instance);
        //}

        [TestMethod]
        public void UnidirectionalPathTest()
        {
            var path = UnidirectionalPath<Life>.Construct(Life.Alive, Life.Dead);

            Assert.IsNotNull(path);
        }

        //[TestMethod]
        //public void PathMapTest()
        //{
        //    var map = PathMap<Life>.Construct();

        //    map.AddPath(Life.Alive, Life.Dead);

        //    Assert.IsNotNull(map);

        //    var destinations = map.Paths
        //        .Where(p => p.Origin == Life.Alive)
        //        .Select(p => p.Destination);
        //}

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThereCanBeOnlyOneTopPriority()
        {
            MappedState<Life>.Construct(PathMapTests.Life.Alive)
                .PathOf(
                    PathMapTests.Life.Alive
                        .LeadsTo(PathMapTests.Life.Dead)
                        .When(() => true))
                .PathOf(
                    PathMapTests.Life.Alive
                        .LeadsTo(PathMapTests.Life.Undead)
                        .When(() => true));
        }

        [TestMethod]
        public void DeathBeforeDishonor()
        {

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

                @this.Life = MappedState<Life>.Construct(PathMapTests.Life.Alive)
                    .PathOf(
                        PathMapTests.Life.Alive
                            .LeadsTo(PathMapTests.Life.Dead)
                            .When(@this.Expired))
                    .PathOf(
                        PathMapTests.Life.Dead
                            .LeadsTo(PathMapTests.Life.Undead)
                            .When(() => @this.SummonedByLichKing));

                return @this;
            }

            public bool Expired() { return UpdateTicks > 0; }
        }

        private class RomanSoldier
        {
            public MappedState<SoldiersLife> Life { get; private set; }

            private int Health = 1;

            public static RomanSoldier IsBorn()
            {
                var @this = new RomanSoldier();

                @this.Life = MappedState<SoldiersLife>.Construct(PathMapTests.SoldiersLife.Alive)
                    .PathOf(
                        SoldiersLife.Alive
                            .LeadsTo(SoldiersLife.Dead)
                            .When(() => @this.Health < 1))
                    .PathOf(
                        SoldiersLife.Alive
                            .LeadsTo(SoldiersLife.Dishonored)
                            .When(() => @this.Health < 1));

                return @this;
            }
        }

        public enum SoldiersLife
        {
            Alive,
            Dead,
            Dishonored
        }

        public enum Life
        {
            Alive,
            Dead,
            Undead
        }
    }
}