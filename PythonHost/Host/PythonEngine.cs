using PythonHost.Host.Registration;
using System;
using System.Runtime.InteropServices;

namespace PythonHost.Host
{
    public sealed class PythonEngine : IDisposable
    {
        #region Initialize
        public PythonEngine() => Py_Initialize();

        [DllImport("python39")]
        private static extern void Py_Initialize();
        #endregion

        public void AddCostumModule(ICostumPythonModule module) => ModuleRegistration.RegisterCustomModule(module);

        #region Execute
        [DllImport("python39")]
        private static extern int PyRun_SimpleString([MarshalAs(UnmanagedType.LPStr)] string code);

        public void ExecuteString(string code) => PyRun_SimpleString(code);
        #endregion

        #region Dispose
        public void Dispose() => Py_Finalize();

        [DllImport("python39")]
        private static extern void Py_Finalize();
        #endregion
    }
}
