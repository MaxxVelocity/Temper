using GraduatedResolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace GraduatedResolutionTests
{
    [TestClass]
    public class TemperatureDemo
    {
        [TestMethod]
        public void ImSoHotForHer()
        {
            var myTemp = -100m;

            var myTempZone = 
                myTemp
                    .AsGradeIn(Centigrade.Scale)
                    .WithDefaultGrade(Centigrade.AbsoluteZero);

            Assert.AreEqual(
                Centigrade.Freezing,
                myTempZone);

            myTemp = 50m;

            Assert.AreEqual(
                Centigrade.Melting,
                myTemp
                    .AsGradeIn(Centigrade.Scale)
                    .WithDefaultGrade(Centigrade.AbsoluteZero));

            myTemp = 150m;

            Assert.AreEqual(
                Centigrade.Boiling,
                myTemp.AsGradeIn(Centigrade.Scale)
                    .WithDefaultGrade(Centigrade.AbsoluteZero));
        }

        public static class Centigrade
        {
            public static Grade<decimal> Default => Centigrade.AbsoluteZero;

            public static Grade<decimal> AbsoluteZero => Grade<decimal>.WithLowerThreshold(default);

            public static Grade<decimal> Freezing => Grade<decimal>.WithLowerThreshold(-273.16m);

            public static Grade<decimal> Melting => Grade<decimal>.WithLowerThreshold(0.1m);

            public static Grade<decimal> Boiling => Grade<decimal>.WithLowerThreshold(100m);

            public static Graduation<decimal> Scale => 
                Graduation<decimal>
                .ConsistingOf(
                    Freezing,
                    Melting,
                    Boiling)
                .WithDefault(Default);
        }
    }
}
