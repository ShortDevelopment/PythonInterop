using System;

namespace PythonHost.Host
{
    public sealed class PythonObject
    {
        public bool IsPrimitive { get; private set; }
        public IntPtr Ptr;

        public PythonObject(IntPtr ptr)
        {
            this.Ptr = ptr;
        }

        public static explicit operator PythonObject(long value)
            => new(Python.PyLong_FromLong(value));

        public static explicit operator PythonObject(double value)
            => new(Python.PyFloat_FromDouble(value));

        public static explicit operator PythonObject(bool value)
            => new(Python.PyBool_FromLong(value));

        public static implicit operator IntPtr(PythonObject obj)
            => obj.Ptr;
    }
}
