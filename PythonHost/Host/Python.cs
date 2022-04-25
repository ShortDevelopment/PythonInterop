using System;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public static class Python
    {
        const string PythonLibName = "python39";

        [DllImport(PythonLibName)]
        public static extern IntPtr PyLong_FromLong(long value);

        [DllImport(PythonLibName)]
        public static extern IntPtr PyFloat_FromDouble(double value);
        [DllImport(PythonLibName)]
        public static extern double PyFloat_AsDouble(IntPtr obj);

        [DllImport(PythonLibName)]
        public static extern IntPtr PyBool_FromLong(bool value);

        [StructLayout(LayoutKind.Sequential)]
        public struct PyObject
        {
            public int ob_refcnt;
            public IntPtr ob_type;
        }
    }
}
