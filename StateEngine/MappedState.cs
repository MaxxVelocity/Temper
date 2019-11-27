namespace StateEngine
{
    public class MappedState<T> where T : System.Enum
    {
        public MappedState<T> PathOf(UnidirectionalPath<T> path)
        {
            paths.Path(path);
            return this;
        }

        public T Status { get; protected set; }

        private PathMap<T> paths { get; set; }

        public static MappedState<T> Construct(T initialState)
        {
            return new MappedState<T>(initialState);
        }

        private MappedState(T initialState)
        {
            paths = PathMap<T>.Construct();
            Status = initialState;
        }

        public void Update()
        {
            foreach (var path in this.paths.Paths)
            {
                if (path.Condition.Invoke())
                {
                    this.Status = path.Destination;
                    return;
                }
            }
        }
    }
}