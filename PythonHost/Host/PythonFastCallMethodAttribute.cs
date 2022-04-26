using System;

namespace PythonHost.Host
{
    /// <summary>
    /// Marks a <see langword="public"/> <see langword="static"/> method, that is available in a <see cref="CostumPythonModule"/>. <br/>
    /// Signature must match <see href="https://docs.python.org/3/c-api/structures.html#c._PyCFunctionFast">PyCFunctionFast</see>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PythonFastCallMethodAttribute : Attribute
    {
        public string Name = null;
        public string DocString = null;
    }
}
