using System;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public sealed class PythonObject
    {
        public bool IsPrimitive { get; private set; }
        public IntPtr Ptr { get; }

        public PythonObject(IntPtr ptr)
        {
            this.Ptr = ptr;
        }

        public static explicit operator PythonObject(long value)
            => new(Python.PyLong_FromLong(value));

        public static explicit operator long(PythonObject obj)
            => Python.PyLong_AsLong(obj);


        public static explicit operator PythonObject(double value)
            => new(Python.PyFloat_FromDouble(value));

        public static explicit operator double(PythonObject obj)
            => Python.PyFloat_AsDouble(obj);


        public static explicit operator PythonObject(bool value)
            => new(Python.PyBool_FromLong(value));


        public static explicit operator PythonObject(string value)
            => new(Python.PyUnicode_FromString(value));

        public static explicit operator string(PythonObject value)
        {
            var ptr = Python.PyUnicode_AsUTF8AndSize(value, out var length);
            return Marshal.PtrToStringUTF8(ptr, length);
        }


        public static implicit operator IntPtr(PythonObject obj)
            => obj.Ptr;

        //public override string ToString()
        //    => (string)this;
    }

    public sealed class PythonObjectMarshaler : ICustomMarshaler
    {
        public void CleanUpManagedData(object ManagedObj) { }

        public void CleanUpNativeData(IntPtr pNativeData) { }

        public int GetNativeDataSize()
            => Marshal.SizeOf<IntPtr>();

        public IntPtr MarshalManagedToNative(object ManagedObj)
            => ((PythonObject)ManagedObj).Ptr;

        public object MarshalNativeToManaged(IntPtr pNativeData)
            => new PythonObject(pNativeData);
    }
}
