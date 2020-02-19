using System;
using GraduatedResolution;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraduationTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ScoreOf75IsAGradeOfC()
        {
            var GradeF = Grade.WithLowerThreshold(0);

            var gradeScale = Graduation.Construct(
                SchoolGradeScale.A,
                SchoolGradeScale.B,
                SchoolGradeScale.C,
                SchoolGradeScale.D);

            var myGrade = 75
                .AsGradeIn2(gradeScale)
                .WithDefaultGrade(GradeF);

            Assert.AreEqual(myGrade, SchoolGradeScale.C);
        }

        private static class SchoolGradeScale
        {
            public static Grade A => Grade.WithLowerThreshold(90);

            public static Grade B => Grade.WithLowerThreshold(80);

            public static Grade C => Grade.WithLowerThreshold(70);

            public static Grade D => Grade.WithLowerThreshold(60);
        }
    }
}