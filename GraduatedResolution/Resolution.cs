using System;
using System.Linq;

namespace GraduatedResolution
{
    public static class Resolution
    {
        public static MaybeResolvedGrade<T> AsGradeIn<T>(this T value, Graduation<T> graduation) where T : IComparable
        {
            if (!graduation.Grades.Any(n => value.IsGreaterThanOrEqualTo(n.Key)))
            {
                return MaybeResolvedGrade<T>.Unresolved();
            }

            return MaybeResolvedGrade<T>.Resolved(
                graduation.Grades
                    .OrderByDescending(n => n.Key)
                    .First(n => value.IsGreaterThanOrEqualTo(n.Key))
                        .Value);
        }

        public static Grade<T> AsGradeIn<T>(this T value, GraduationWithDefault<T> graduation) where T : IComparable
        {
            return graduation.Grades.Any(n => value.IsGreaterThanOrEqualTo(n.Key))
                 ? graduation.Grades
                    .OrderByDescending(n => n.Key)
                    .First(n => value.IsGreaterThanOrEqualTo(n.Key)).Value
                : graduation.Default;
        }

        public static Grade<T> WithDefaultGrade<T>(this MaybeResolvedGrade<T> maybe, Grade<T> defaultGrade) where T : IComparable
        {
            return maybe.HasValue 
                ? maybe.Value 
                : defaultGrade;
        }

        public static GraduationWithDefault<T> WithDefault<T>(this Graduation<T> graduation, Grade<T> defaultGrade) where T : IComparable
        {
            return new GraduationWithDefault<T>(graduation.Grades, defaultGrade);
        }

        private static bool IsGreaterThanOrEqualTo(this IComparable value1, IComparable value2)
        {
            var x = value1.CompareTo(value2) >= 0;
            return x;
        }


        // This algorithm proved to be less performant than simply using the LINQ operators.

        //public static MaybeResolvedGrade<T> AsGradeIn<T>(this T value, Graduation<T> graduation) where T : IComparable
        //{
        //    var selection = MaybeResolvedGrade<T>.Unresolved();
        //    foreach (var grade in graduation.Grades.OrderBy(n => n.Key))
        //    {                                  
        //        var currentGradeMeetsCriteria = (value.CompareTo(grade.Key) >= 0);
        //        if (currentGradeMeetsCriteria) selection = MaybeResolvedGrade<T>.Resolved(grade.Value);
        //        else
        //        {
        //            if (selection.HasValue)
        //            {
        //                return selection;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return selection;
        //}
    }
}