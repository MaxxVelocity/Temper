using GraduatedResolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraduatedResolutionTests
{
    [TestClass]
    public class SchoolGradeDemo
    {
        [TestMethod]
        public void ScoreOf75IsAGradeOfC()
        {
            var GradeF = Grade<int>.WithLowerThreshold(0);

            var gradeScale = Graduation<int>.ConsistingOf(
                SchoolGradeScale.A,
                SchoolGradeScale.B,
                SchoolGradeScale.C,
                SchoolGradeScale.D);

            var myGrade = 75
                .AsGradeIn(gradeScale)
                .WithDefaultGrade(GradeF);

            Assert.AreEqual(myGrade, SchoolGradeScale.C);
        }

        private static class SchoolGradeScale
        {
            public static Grade<int> A => Grade<int>.WithLowerThreshold(90);

            public static Grade<int> B => Grade<int>.WithLowerThreshold(80);

            public static Grade<int> C => Grade<int>.WithLowerThreshold(70);

            public static Grade<int> D => Grade<int>.WithLowerThreshold(60);
        }
    }
}