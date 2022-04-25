using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PythonHost.Host.Registration
{
    internal class ModuleRegistration
    {
        public unsafe static void RegisterCustomModule(CostumPythonModule module)
        {
            IntPtr hModule = PyImport_AddModule(module.Name);
            if (module.DocString != null && !string.IsNullOrEmpty(module.DocString))
                PyModule_SetDocString(hModule, module.DocString);

            #region Functions
            List<PyMethodDef> methodDefinitions = new();
            foreach (MethodInfo method in module.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                PythonFastCallMethodAttribute? data = method.GetCustomAttribute<PythonFastCallMethodAttribute>();
                if (data == null)
                    continue;

                string name = method.Name;
                if (!string.IsNullOrEmpty(data.Name))
                    name = data.Name;

                string docString = "";
                if (!string.IsNullOrEmpty(data.DocString))
                    docString = data.DocString;

                PyCFunctionFast cfunction = method.CreateDelegate<PyCFunctionFast>();

                methodDefinitions.Add(new()
                {
                    Name = (char*)Marshal.StringToHGlobalAnsi(name),
                    Pointer = (void*)Marshal.GetFunctionPointerForDelegate(cfunction),
                    Flags = PyMethodFlags.FastCall,
                    DocString = (char*)Marshal.StringToHGlobalAnsi(docString)
                });
            }

            methodDefinitions.Add(new()
            {
                Name = (char*)Marshal.StringToHGlobalUni(null)
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

        /// <summary>
        /// <see href="https://docs.python.org/3/c-api/structures.html#c.PyMethodDef"/>
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private unsafe struct PyMethodDef
        {
            public char* Name;
            public void* Pointer;
            public PyMethodFlags Flags;
            public char* DocString;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr PyCFunction(IntPtr self, IntPtr args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr PyCFunctionFast(IntPtr self, IntPtr args, int nargs);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr PyCFunctionFastWithKeywords(IntPtr self, IntPtr args, int nargs, IntPtr kwnames);

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
