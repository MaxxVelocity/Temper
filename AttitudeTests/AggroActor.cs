using StateEngine;
using System.Windows;
using static AttitudeTests.AggroTemperment;

namespace AttitudeTests
{
    public class AggroActor : IProximityTarget
    {
        public Point Location { get; private set; }

        public Determinator<State> AggroStatus { get; internal set; }

        public ITarget AggroTarget { get => aggroTarget; }

        public bool TargetIsTracked() => targetIsBeingTracked;

        private AggroTemperment.ITarget aggroTarget = new AggroTemperment.NoTarget();

        private bool targetIsBeingTracked = false;

        public void AcquiresTarget(ITarget target)
        {
            this.aggroTarget = target;
            this.AggroStatus.Update();
            this.targetIsBeingTracked = true;
        }

        public void LoosesTrackOfTarget()
        {
            // The target is still in the actors memory, but is not being tracked
            targetIsBeingTracked = false;
            this.AggroStatus.Update();
        }

        public static AggroActor Spawn()
        {
            return new AggroActor();
        }

        private AggroActor()
        {
            this.HasSimpleAggroEngine();
        }
    }
}