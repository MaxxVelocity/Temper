using System.Collections.Generic;
using System.Linq;

namespace StateEngine
{
    /// <summary>
    /// Represents an instance of a status within a defined map.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedState<T> where T : System.Enum
    {
        public T Status { get; protected set; }

        private PathMap<T> paths { get; set; }

        private List<ConditionalPath<T>> PathsFromCurrentStatus { get; set; }

        private PathsFromCurrentState Current;

        private uint UpdateTicks = 0;

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


        public void Update()
        {
            UpdateTicks = UpdateTicks++;

            this.Current = this.Current ?? PathsFromCurrentState.Construct(this.Status, this.paths);

            // Evaluate transition to conditional paths
            foreach (var path in Current.ConditionalPaths.OrderBy(n => n.Priority))
            {
                if (path.Condition.Invoke())
                {
                    this.Status = path.Destination;

                    return;
                }
            }

            // Evaluate transition to expiry paths
            if(this.Current.ExpiryPath.Countdown >= this.UpdateTicks)
            {
                this.Status = this.Current.ExpiryPath.Destination;
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
                    ExpiryPath = map.ExpiryPaths?.Single(n => n.Origin.Equals(state))
                };
            }
        }
    }
}