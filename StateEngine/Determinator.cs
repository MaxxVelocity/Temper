using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    /// <summary>
    /// Represents an instance of a status within a defined map. Normally such instance would be an attribute property of an entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Determinator<T> where T : System.Enum
    {
        public T Status { get; protected set; }

        private Map<T> paths { get; set; }

        private PotentialTransitions CurrentPotential;

        private uint currentStateTicks = 0;

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        public static Determinator<T> Construct(T initialState)
        {
            return new Determinator<T>(initialState);
        }

        /// <summary>
        /// Constructs new instance.
        /// </summary>
        public static Determinator<T> StartsAs(T initialState)
        {
            return Construct(initialState);
        }

        /// <summary>
        /// Constructs a new path in the underlying map.
        /// </summary>
        public Determinator<T> PathOf(ConditionalPath<T> path)
        {
            paths.Path(path);
            return this;
        }

        /// <summary>
        /// Constructs a new path in the underlying map.
        /// </summary>
        public Determinator<T> PathOf(ExpiryPath<T> path)
        {
            paths.Path(path);
            return this;
        }

        /// <summary>
        /// Evaluates transition paths from the current state and assigns a new current state if a path condition is met. 
        /// The order of resolution is Durable > Conditional > Expiry.
        /// </summary>
        public void Update()
        {
            currentStateTicks++;

            // Construct the current potential transition aggregate upon the first update
            this.CurrentPotential = this.CurrentPotential 
                ?? PotentialTransitions.Construct(this.Status, this.paths);

            // Evaluate transition to conditional paths
            foreach (var path in CurrentPotential.ConditionalPaths.OrderBy(n => n.Priority))
            {
                // Attempt to resolve as a durable condition
                var conditionDurationPath = path as ConditionDurationPath<T>;

                if(!(conditionDurationPath is null))
                {
                    if (currentStateTicks >= conditionDurationPath.DurationThreshold
                        && conditionDurationPath.Condition.Invoke())
                    {
                        DoTransition(path);
                        return;
                    }
                }

                // Resolve as simple condition
                else
                {
                    if (path.Condition.Invoke())
                    {
                        DoTransition(path);
                        return;
                    }
                }
            }

            // Evaluate transition to expiry paths
            if(this.currentStateTicks >= this.CurrentPotential.ExpiryPath?.Countdown)
            {
                this.Status = this.CurrentPotential.ExpiryPath.Destination;
                this.currentStateTicks = 0;

                //TODO: point to a specific construct instead of NULL
                this.CurrentPotential = null;
            }
        }

        protected Determinator(T initialState)
        {
            paths = Map<T>.Construct();
            Status = initialState;
        }

        private void DoTransition(ConditionalPath<T> path)
        {
            this.Status = path.Destination;
            this.CurrentPotential = null;
            this.currentStateTicks = 0;
        }

        private interface IPotentialTransitions { };

        private struct  UnresolvedPotential : IPotentialTransitions { }

        private class PotentialTransitions : IPotentialTransitions
        {
            public List<ConditionalPath<T>> ConditionalPaths { get; private set; }

            public ExpiryPath<T> ExpiryPath { get; private set; }

            public static PotentialTransitions Construct(T state, Map<T> map)
            {
                return new PotentialTransitions()
                {
                    ConditionalPaths = map.Paths?.Where(n => n.Origin.Equals(state))?.ToList(),
                    ExpiryPath = map.ExpiryPaths?.SingleOrDefault(n => n.Origin.Equals(state))
                };
            }
        }
    }
}