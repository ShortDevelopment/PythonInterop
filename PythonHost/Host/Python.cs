using System;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public static class Python
    {
        const string PythonLibName = "python39";

        #region Value Types
        [DllImport(PythonLibName)]
        public static extern IntPtr PyLong_FromLong(long value);

        [DllImport(PythonLibName)]
        public static extern long PyLong_AsLong(IntPtr value);


        [DllImport(PythonLibName)]
        public static extern IntPtr PyFloat_FromDouble(double value);

        [DllImport(PythonLibName)]
        public static extern double PyFloat_AsDouble(IntPtr obj);


        [DllImport(PythonLibName)]
        public static extern IntPtr PyBool_FromLong(bool value);

        [DllImport(PythonLibName)]
        public static extern bool PyObject_IsTrue(IntPtr obj);


        [DllImport(PythonLibName)]
        public static extern IntPtr PyUnicode_FromString([MarshalAs(UnmanagedType.LPUTF8Str)] string value);

        [DllImport(PythonLibName)]
        public static extern IntPtr PyUnicode_AsUTF8AndSize(IntPtr obj, out int size);
        #endregion

        #region Tuples
        [DllImport(PythonLibName)]
        public static extern IntPtr PyTuple_New(int size);

        [DllImport(PythonLibName)]
        public static extern int PyTuple_Size(IntPtr tuple);

        [DllImport(PythonLibName)]
        public static extern IntPtr PyTuple_GetItem(IntPtr tuple, int index);

        [DllImport(PythonLibName)]
        public static extern int PyTuple_SetItem(IntPtr tuple, int index, IntPtr obj);
        #endregion

        #region Objects
        [DllImport(PythonLibName)]
        public static extern IntPtr PyObject_Str(IntPtr obj);

        [DllImport(PythonLibName)]
        public static extern IntPtr PyObject_GetItem(IntPtr obj, IntPtr key);

        [DllImport(PythonLibName)]
        public static extern int PyObject_SetItem(IntPtr obj, IntPtr key, IntPtr value);

        [DllImport(PythonLibName)]
        public static extern int PyObject_DelItem(IntPtr obj, IntPtr key);

        [DllImport(PythonLibName)]
        public static extern int PyObject_Length(IntPtr obj);
        #endregion

        public static unsafe PythonObject[] GetArgs(IntPtr args, int nargs)
        {
            PythonObject[] result = new PythonObject[nargs];
            for (int i = 0; i < nargs; i++)
                result[i] = new(((IntPtr*)args)[i]);
            return result;
        }
    }
}
