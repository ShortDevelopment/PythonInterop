using System;

namespace PythonHost.Host
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PythonFastCallMethodAttribute : Attribute
    {
        public string Name = null;
        public string DocString = null;
    }
}
