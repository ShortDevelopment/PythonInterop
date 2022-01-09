using System;

namespace PythonHost.Host
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PythonMethodAttribute : Attribute
    {
        public string Name = null;
        public string DocString = null;
    }
}
