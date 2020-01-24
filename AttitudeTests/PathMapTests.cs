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
        [TestMethod]
        public void TransitionPathTest()
        {
            var path = TransitionPath<Life>.Construct(Life.Alive, Life.Dead);

            Assert.IsNotNull(path);
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
        public void NowImFeelingZombified()
        {
            var arthas = DeathKnight.IsBorn();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Alive));

            arthas.LifeUpdate();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Dead));

            arthas.SummonedByLichKing();

            arthas.LifeUpdate();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Undead));
        }

        private class DeathKnight
        {
            public void LifeUpdate()
            {
                Life.Update();
            }

            public void SummonedByLichKing()
            {
                summoned = true;
            }

            public bool HasBeenSummoned() { return summoned; }

            private bool summoned = false;

            public MappedState<Life> Life { get; private set; }

            public static DeathKnight IsBorn()
            {              
                var @this = new DeathKnight();

                @this.Life = MappedState<Life>.Construct(PathMapTests.Life.Alive)
                    .PathOf(
                        PathMapTests.Life.Alive
                            .ExpiresTo(PathMapTests.Life.Dead, 1));

                Func<bool> summonDelegate = ()=> @this.HasBeenSummoned();

                @this.Life
                    .PathOf(
                        PathMapTests.Life.Dead
                            .LeadsTo(PathMapTests.Life.Undead)
                            .When2(() => { return @this.summoned == true; }));

                return @this;
            }
        }

        [TestMethod]
        public void DeathBeforeDishonor()
        {

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