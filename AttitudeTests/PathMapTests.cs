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
            var arthas = PotentialDeathKnight.IsBorn();
            var uther = PotentialDeathKnight.IsBorn();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Alive));
            Assert.IsTrue(
                uther.Life.Status.Equals(Life.Alive));

            arthas.Life.Update();
            uther.Life.Update();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Dead));
            Assert.IsTrue(
                uther.Life.Status.Equals(Life.Dead));

            arthas.SummonedByLichKing();
            // Uther is not summoned by the Lich King because he's not a douche

            arthas.Life.Update();
            uther.Life.Update();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Undead));
            Assert.IsTrue(
                uther.Life.Status.Equals(Life.Dead));
        }

        private class PotentialDeathKnight
        {
            public bool HasBeenSummoned() => summoned;

            private bool summoned = false;

            public MappedState<Life> Life { get; private set; }

            public static PotentialDeathKnight IsBorn()
            {              
                var @this = new PotentialDeathKnight();
                @this.Life = DeathKnightLifeStateMap(@this);
                return @this;
            }

            public void LifeUpdate()
            {
                Life.Update();
            }

            public void SummonedByLichKing()
            {
                summoned = true;
            }

            public static MappedState<Life> DeathKnightLifeStateMap(PotentialDeathKnight subject)
            {
                return MappedState<Life>.Construct(PathMapTests.Life.Alive)
                    .PathOf(
                        PathMapTests.Life.Alive
                            .ExpiresTo(PathMapTests.Life.Dead, 1))
                    .PathOf(
                        PathMapTests.Life.Dead
                            .LeadsTo(PathMapTests.Life.Undead)
                            .When(subject.HasBeenSummoned));
            }
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