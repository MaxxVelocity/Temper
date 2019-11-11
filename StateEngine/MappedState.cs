namespace StateEngine
{
    public class MappedState<T> where T : System.Enum
    {
        public void PathOf(UnidirectionalPath<T> path) => paths.Path(path);

        public T Status { get; protected set; }

        private PathMap<T> paths { get; set; }

        public static MappedState<T> Construct()
        {
            return new MappedState<T>();
        }

        private MappedState()
        {
            paths = PathMap<T>.Construct();
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
