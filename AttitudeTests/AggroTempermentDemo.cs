using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateEngine;
using System;
using System.Windows;

namespace AttitudeTests
{
    /// <summary>
    /// These tests demonstrates that states can be designed to perist for a number of ticks,
    /// then automatically transition to a new state upon reaching a threshold of duration.
    /// </summary>
    [TestClass]
    public class AggroTempermentDemo
    {
        [TestMethod]
        public void WhoGoesThere_TargetGetsLostWhileSuspicious()
        {
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            // In a practical application, the AquiresTarget concept could be a delegate which measures distance or line-of-sight
            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            // In a practical application, the LoosesTrackOfTarget concept could be a negation of the criteria involved in aquiring
            guard.LoosesTrackOfTarget(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);
        }

        [TestMethod]
        public void WhoGoesThere_TargetCausesAlarm()
        {
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.State.Alerted, guard.AggroStatus.Status);
        }

        [TestMethod]
        public void WhoGoesThere_LostTargetCausesSearch()
        {
            var guard = AggroActor.Spawn();
            var intruder = Intruder.Spawn();

            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            guard.AcquiresTarget(intruder); // This method consumes one tick of duration
            Assert.AreEqual(AggroTemperment.State.Unaware, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes a second tick of duration
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.State.Suspicious, guard.AggroStatus.Status);

            guard.AggroStatus.Update(); // Consumes second tick
            Assert.AreEqual(AggroTemperment.State.Alerted, guard.AggroStatus.Status);

            guard.LoosesTrackOfTarget(); // Consumes one tick
            Assert.AreEqual(AggroTemperment.State.Searching, guard.AggroStatus.Status);
        }

        [TestMethod]
        public void WhoGoesThere_ProximityCausesAggro()
        {
            var coord1 = new Point(0, 0);
            var coord2 = new Point(10, 10);

            var distance = coord1.DistanceTo(coord2);
            Assert.AreEqual(14, distance);
        }

        private class Intruder : AggroTemperment.ITarget
        {
            public static Intruder Spawn()
            {
                return new Intruder();
            }
        }

        private interface IProximityTarget
        {
            Point Location { get; }
        };
    }
}