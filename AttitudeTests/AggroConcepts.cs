using StateEngine;
using System;
using System.Windows;

namespace AttitudeTests
{
    public static class AggroTemperment
    {
        public enum State
        {
            Unaware,
            Suspicious,
            Alerted,
            Searching,
            Fleeing
        }

        public interface ITarget { };

        public interface IProximityTarget : ITarget
        { 
            Point Location { get; } 
        };

        public struct NoTarget : ITarget { }

        //TODO: make this apply to something more abstract than the AggroActor concretion
        public static void HasSimpleAggroEngine(this AggroActor actor)
        {
            actor.AggroStatus = Determinator<State>
                .StartsAs(State.Unaware)

                .PathOf(
                    State.Unaware
                        .LeadsTo(State.Suspicious)
                        .When(actor.TargetIsTracked)
                        .AfterDurationOf(2))

                .PathOf(
                    State.Suspicious
                        .ExpiresTo(State.Unaware, 2))
                .PathOf(
                    State.Suspicious
                        .LeadsTo(State.Alerted)
                        .When(actor.TargetIsTracked)
                        .AfterDurationOf(2))

                .PathOf(
                    State.Alerted
                        .LeadsTo(State.Searching)
                        .When(() => !actor.TargetIsTracked()))

                .PathOf(
                    State.Searching
                        .ExpiresTo(State.Searching, 4))
                .PathOf(
                    State.Searching
                        .LeadsTo(State.Alerted)
                        .When(actor.TargetIsTracked));
        }
    }

    static class MathExtensions
    {
        public static int DistanceTo(this Point origin, Point destination)
        {
            var xDistance = (Math.Abs(origin.X) - Math.Abs(destination.X));
            var yDistance = (Math.Abs(origin.Y) - Math.Abs(destination.Y));
            var xx = (Math.Pow(xDistance, 2));
            var yy = (Math.Pow(yDistance, 2));
            return (int)Math.Sqrt(xx + yy);
        }
    }
}