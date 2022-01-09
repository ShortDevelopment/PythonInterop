using System;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public static class Python
    {
        [DllImport("python39")]
        public static extern IntPtr PyLong_FromLong(long value);

        [StructLayout(LayoutKind.Sequential)]
        public struct PyObject
        {
            public int ob_refcnt;
            public IntPtr ob_type;
        }
    }
}
