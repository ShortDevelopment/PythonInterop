using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public sealed class PythonObject : ICollection<PythonObject>
    {
        public IntPtr Ptr { get; }

        public PythonObject(IntPtr ptr)
        {
            this.Ptr = ptr;
        }

        #region ICollection
        public int Count
            => Python.PyObject_Length(this);

        public void Add(PythonObject item)
            => this[Count + 1] = item;

        public bool Remove(PythonObject item)
        {
            Python.PyObject_DelItem(this, item);
            return true;
        }

        public IEnumerator<PythonObject> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();


        [Obsolete("Not implemented!")]
        public void CopyTo(PythonObject[] array, int arrayIndex)
            => throw new NotImplementedException();

        [Obsolete("Not implemented!")]
        public void Clear()
            => throw new NotImplementedException();

        [Obsolete("Not implemented!")]
        public bool IsReadOnly
            => throw new NotImplementedException();

        [Obsolete("Not implemented!")]
        public bool Contains(PythonObject item)
            => throw new NotImplementedException();
        #endregion

        #region Indexer
        public PythonObject this[int i]
        {
            get => this[(PythonObject)i];
            set => this[(PythonObject)i] = value;
        }

        public PythonObject this[PythonObject key]
        {
            get => new(Python.PyObject_GetItem(this, key));
            set => Python.PyObject_SetItem(this, key, value);
        }
        #endregion

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

        public static explicit operator bool(PythonObject obj)
            => Python.PyObject_IsTrue(obj);


        public static explicit operator PythonObject(string value)
            => new(Python.PyUnicode_FromString(value));

        public static explicit operator string(PythonObject value)
        {
            var ptr = Python.PyUnicode_AsUTF8AndSize(value, out var length);
            return Marshal.PtrToStringUTF8(ptr, length);
        }


        public static implicit operator IntPtr(PythonObject obj)
            => obj.Ptr;

        public override string ToString()
            => (string)new PythonObject(Python.PyObject_Str(this));
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
