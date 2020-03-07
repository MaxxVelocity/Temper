using System;
using System.Collections.Generic;

namespace GraduatedResolution
{
    /// <summary>
    /// A qualification based on an IComparable type.
    /// </summary>
    /// <typeparam name="T">Any IComparable type, typically a numerical value.</typeparam>
    public struct Grade<T> where T : IComparable
    {
        public T Threshold { get; private set; }

        public static Grade<T> WithLowerThreshold(T threshold) 
        {
            return new Grade<T>() { Threshold = threshold };      
        }
    }

    /// <summary>
    /// A qualification based on an integer threshold.
    /// </summary>
    public struct Grade
    {
        public int Threshold { get; private set; }

        public static Grade WithLowerThreshold(int threshold)
        {
            return new Grade() { Threshold = threshold };
        }
    }

    /// <summary>
    /// A result which may or may not have been resolved to a specific grade.
    /// </summary>
    /// <typeparam name="T">The type of the grade metric, typically numerical.</typeparam>
    public struct MaybeResolvedGrade<T> where T : IComparable
    {
        public bool HasValue { get; private set; }

        public Grade<T> Value { get; private set; }

        public static MaybeResolvedGrade<T> Unresolved()
        {
            return new MaybeResolvedGrade<T>() { HasValue = false };
        }

        public static MaybeResolvedGrade<T> Resolved(Grade<T> value)
        {
            return new MaybeResolvedGrade<T>() { HasValue = true, Value = value };
        }
    }

    /// <summary>
    /// A set of grades which form a scale.
    /// </summary>
    /// <typeparam name="T">An IComparable value type used in the grade metric.</typeparam>
    public class GraduationWithDefault<T> : Graduation<T> where T : IComparable
    {
        public Grade<T> Default { get; private set; }

        internal GraduationWithDefault(SortedList<T, Grade<T>> grades, Grade<T> defaultGrade) : base(grades)
        {
            this.Default = defaultGrade;
        }
    }

    public class Graduation<T> where T : IComparable
    {
        public readonly SortedList<T, Grade<T>> Grades;

        public static Graduation<T> ConsistingOf(params Grade<T>[] grades)
        {
            var graduation = new Graduation<T>();

            Array.ForEach(grades, n => graduation.Grades.Add(n.Threshold, n));

            //foreach (var grade in grades)
            //{
            //    graduation.Grades.Add(grade.Threshold, grade);
            //}

            return graduation;
        }

        private Graduation()
        {
            this.Grades = new SortedList<T, Grade<T>>();
        }

        protected Graduation(SortedList<T, Grade<T>> grades)
        {
            this.Grades = grades;
        }
    }
}
