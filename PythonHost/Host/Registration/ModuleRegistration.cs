using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PythonHost.Host.Registration
{
    internal class ModuleRegistration
    {
        public static void RegisterCustomModule(ICostumPythonModule module)
        {
            IntPtr hModule = PyImport_AddModule(module.Name);
            PyModule_SetDocString(hModule, module.DocString);

            #region Functions
            List<PyMethodDef> methodDefinitions = new();
            foreach (MethodInfo method in module.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                PythonMethodAttribute? data = method.GetCustomAttribute<PythonMethodAttribute>();
                if (data == null)
                    continue;

                string name = method.Name;
                if (!string.IsNullOrEmpty(data.Name))
                    name = data.Name;

                string docString = "";
                if (!string.IsNullOrEmpty(data.DocString))
                    docString = data.DocString;

                PyCFunction cfunction = method.CreateDelegate<PyCFunction>();

                methodDefinitions.Add(new PyMethodDef()
                {
                    ml_name = Marshal.StringToHGlobalAnsi(name),
                    ml_meth = Marshal.GetFunctionPointerForDelegate(cfunction),
                    ml_flags = PyMethodFlags.NoArgs,
                    ml_doc = Marshal.StringToHGlobalAnsi(docString)
                });
            }

            methodDefinitions.Add(new PyMethodDef()
            {
                ml_name = Marshal.StringToHGlobalUni(null)
            });

            if (PyModule_AddFunctions(hModule, methodDefinitions.ToArray()) != 0)
                throw new Exception("\"PyModule_AddFunctions\" failed");
            #endregion

            PyModule_AddStringConstant(hModule, "food", "spam");
        }

        [DllImport("python39", CharSet = CharSet.Unicode)]
        private static extern IntPtr PyImport_AddModule([MarshalAs(UnmanagedType.LPStr)] string moduleName);

        [DllImport("python39", CharSet = CharSet.Unicode)]
        private static extern int PyModule_SetDocString(IntPtr module, [MarshalAs(UnmanagedType.LPStr)] string docString);

        #region Consts
        [DllImport("python39", CharSet = CharSet.Unicode)]
        private static extern int PyModule_AddStringConstant(IntPtr module, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string value);

        [DllImport("python39", CharSet = CharSet.Unicode)]
        private static extern int PyModule_AddIntConstant(IntPtr module, [MarshalAs(UnmanagedType.LPStr)] string name, int value);
        #endregion

        #region Functions
        [DllImport("python39")]
        private static extern int PyModule_AddFunctions(IntPtr module, PyMethodDef[] functions);

        [StructLayout(LayoutKind.Sequential)]
        private struct PyMethodDef
        {
            // [MarshalAs(UnmanagedType.LPStr)]
            public IntPtr ml_name;
            // [MarshalAs(UnmanagedType.FunctionPtr)]
            public IntPtr ml_meth;
            public PyMethodFlags ml_flags;
            // [MarshalAs(UnmanagedType.LPStr)]
            public IntPtr ml_doc;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr PyCFunction(IntPtr self, IntPtr args);

        [Flags]
        private enum PyMethodFlags : int
        {
            [Obsolete]
            OLDARGS = 0,
            VarArgs = 1,
            Keywords = 2,
            NoArgs = 4,
            O = 8,

            Class = 0x10,
            Static = 0x20,

            /// <summary>
            /// Allows a method to be entered even though a slot has
            /// already filled the entry.  When defined, the flag allows a separate
            /// method, "__contains__" for example, to coexist with a defined
            /// slot like sq_contains.
            /// </summary>
            Coexist = 0x40,

            /// <remarks>3.10+</remarks>
            FastCall = 0x80,

            /// <summary>
            /// The function stores an
            /// additional reference to the class that defines it;
            /// both self and class are passed to it.
            /// It uses PyCMethodObject instead of PyCFunctionObject.
            /// May not be combined with METH_NOARGS, METH_O, METH_CLASS or METH_STATIC.
            /// </summary>
            /// <remarks>3.9+</remarks>
            Method = 0x0200,
        }

        #endregion
    }
}
