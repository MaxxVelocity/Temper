using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;

namespace AttitudeTests
{
    [TestClass]
    public class AggroTempermentDemo
    {
        [TestMethod]
        public void WhoGoesThere_TargetGetsLostWhileSuspicious()
        {
            // This test asserts that the conditional transitions take 2 ticks of duration to realize
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.LoosesTrackOfTarget(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);
        }

        [TestMethod]
        public void WhoGoesThere_TargetCausesAlarm()
        {
            // This test asserts that the conditional transitions take 2 ticks of duration to realize
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.Alerted, guard.Aggro.Status);
        }

        [TestMethod]
        public void WhoGoesThere_LostTargetCausesSearch()
        {
            // This test asserts that the conditional transitions take 2 ticks of duration to realize
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.Unaware, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.Suspicious, guard.Aggro.Status);

            guard.Aggro.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.Alerted, guard.Aggro.Status);

            guard.LoosesTrackOfTarget(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.Searching, guard.Aggro.Status);
        }

        private class AggroActor
        {
            public MappedState<AggroTemperment> Aggro { get; private set; }

            public bool TargetIsTracked() => targetIsBeingTracked;

            public bool TargetIsNotTracked() => !targetIsBeingTracked;

            public IAggroTarget AggroTarget = null;

            private bool targetIsBeingTracked = false;

            public void AcquiresTarget(IAggroTarget target)
            {
                this.AggroTarget = target;
                this.Aggro.Update();
                this.targetIsBeingTracked = true;
            }

            public void LoosesTrackOfTarget()
            {
                // The target is still in the actors memory, but is not being tracked
                targetIsBeingTracked = false;
                this.Aggro.Update();
            }

            public static AggroActor Spawn()
            {
                return new AggroActor();
            }

            private AggroActor()
            {
                this.Aggro = AggroStateEngine();
            }

            private MappedState<AggroTemperment> AggroStateEngine()
            {
                return MappedState<AggroTemperment>
                    .StartsAs(AggroTemperment.Unaware)

                    .PathOf(
                        AggroTemperment.Unaware
                            .LeadsTo(AggroTemperment.Suspicious)
                            .When(this.TargetIsTracked)
                            .AfterDurationOf(2))

                    .PathOf(
                        AggroTemperment.Suspicious
                            .ExpiresTo(AggroTemperment.Unaware, 2))
                    .PathOf(
                        AggroTemperment.Suspicious
                            .LeadsTo(AggroTemperment.Alerted)
                            .When(this.TargetIsTracked)
                            .AfterDurationOf(2))

                    .PathOf(AggroTemperment.Alerted
                            .LeadsTo(AggroTemperment.Searching)
                            .When(() => !this.TargetIsTracked()));
            }
        }

        private enum AggroTemperment
        {
            Unaware,
            Suspicious,
            Alerted,
            Searching,
            Fleeing
        }

        private class Intruder : IAggroTarget
        {
            public static Intruder Spawn()
            {
                return new Intruder();
            }
        }

        private interface IAggroTarget { };
    }
}
