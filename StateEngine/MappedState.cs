using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    /// <summary>
    /// Represents an instance of a status within a defined map. Normally such instance would be an attribute property of an entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedState<T> where T : System.Enum
    {
        public T Status { get; protected set; }

        private PathMap<T> paths { get; set; }

        private PathsFromCurrentState Current;

        private uint lifetimeTicks = 0;

        private uint currentStateTicks = 0;

        public static MappedState<T> Construct(T initialState)
        {
            return new MappedState<T>(initialState);
        }

        public MappedState<T> PathOf(ConditionalPath<T> path)
        {
            paths.Path(path);
            return this;
        }

        public MappedState<T> PathOf(ExpiryPath<T> path)
        {
            paths.Path(path);
            return this;
        }

        /// <summary>
        /// Evaluates transition paths from the current state and assigns a new current state if a path condition is met
        /// </summary>
        public void Update()
        {
            lifetimeTicks++;
            currentStateTicks++;

            this.Current = this.Current ?? PathsFromCurrentState.Construct(this.Status, this.paths);

            // Evaluate transition to conditional paths
            foreach (var path in Current.ConditionalPaths.OrderBy(n => n.Priority))
            {
                if (path.Condition.Invoke())
                {
                    this.Status = path.Destination;
                    this.Current = null;
                    this.currentStateTicks = 0;
                    return;
                }
            }

            //TODO: consider flexibility that would allow an expiry transition to take priority

            // Evaluate transition to expiry paths
            if(this.Current.ExpiryPath?.Countdown >= this.currentStateTicks)
            {
                this.Status = this.Current.ExpiryPath.Destination;
                this.currentStateTicks = 0;
                this.Current = null;
            }
        }

        protected MappedState(T initialState)
        {
            paths = PathMap<T>.Construct();
            Status = initialState;
        }

        private class PathsFromCurrentState
        {
            public List<ConditionalPath<T>> ConditionalPaths { get; private set; }

            public ExpiryPath<T> ExpiryPath { get; private set; }

            public static PathsFromCurrentState Construct(T state, PathMap<T> map)
            {
                return new PathsFromCurrentState()
                {
                    ConditionalPaths = map.Paths?.Where(n => n.Origin.Equals(state))?.ToList(),
                    ExpiryPath = map.ExpiryPaths?.SingleOrDefault(n => n.Origin.Equals(state))
                };
            }
        }
    }
}