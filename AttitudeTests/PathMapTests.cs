using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;
using System;

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
        public void ConditionDurationPathTest()
        {
            var toggle = false;

            var path = ConditionalPath<Life>.Construct(Life.Alive, Life.Dead);
            var path2 = ConditionDurationPath<Life>.Construct(Life.Alive, Life.Dead, ()=> toggle, 2);

            TransitionPath<Life> temp = path;

            var resolvedType = temp as ConditionalPath<Life>;

            Assert.IsNotNull(resolvedType);
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

            // Both have died from old age
            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Dead));
            Assert.IsTrue(
                uther.Life.Status.Equals(Life.Dead));

            arthas.SummonedByLichKing();
            // Uther is not summoned by the Lich King because he's not a jerk

            arthas.Life.Update();
            uther.Life.Update();

            Assert.IsTrue(
                arthas.Life.Status.Equals(Life.Undead));
            Assert.IsTrue(
                uther.Life.Status.Equals(Life.Dead));
        }


        private class PotentialDeathKnight
        {
            public Determinator<Life> Life { get; private set; }

            public bool IsReanimated() => summoned;

            private bool summoned = false;

            public static PotentialDeathKnight IsBorn()
            {              
                var @this = new PotentialDeathKnight();
                @this.Life = DeathKnightLifeStateMap(@this);
                return @this;
            }

            public void SummonedByLichKing()
            {
                summoned = true;
            }

            public static Determinator<Life> DeathKnightLifeStateMap(PotentialDeathKnight subject)
            {
                return Determinator<Life>.StartsAs(PathMapTests.Life.Alive)
                    .PathOf(
                        PathMapTests.Life.Alive
                            .ExpiresTo(PathMapTests.Life.Dead, 1))
                    .PathOf(
                        PathMapTests.Life.Dead
                            .LeadsTo(PathMapTests.Life.Undead)
                            .When(subject.IsReanimated));
            }
        }

        public enum Life
        {
            Alive,
            Dead,
            Undead
        }
    }
}