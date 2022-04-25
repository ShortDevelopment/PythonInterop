namespace PythonHost.Host
{
    public abstract class CostumPythonModule
    {
        public abstract string Name { get; }
        public virtual string? DocString { get; }
    }
}
