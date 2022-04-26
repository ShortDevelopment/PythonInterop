namespace PythonHost.Host
{
    /// <summary>
    /// Base class for a custom module available to python. <br/>
    /// Add static methods with <see cref="PythonFastCallMethodAttribute"/>
    /// </summary>
    public abstract class CostumPythonModule
    {
        public abstract string Name { get; }
        public virtual string? DocString { get; }
    }
}
