using PythonHost.Host;
using System;

namespace PythonHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (PythonEngine engine = new())
            {
                engine.AddCostumModule(new MyTestModule());
                engine.ExecuteString("from time import time,ctime\n" +
                    "print('Today is', ctime(time()))\n");

                engine.ExecuteString("import MyTestModule\n" +
                    "print(MyTestModule.__doc__)\n" +
                    "print(MyTestModule.TestMethod.__doc__)\n" +
                    "print(MyTestModule.TestMethod(123456, 'test'))\n");
            }
            Console.ReadLine();
        }


        class MyTestModule : CostumPythonModule
        {
            public override string Name
                => "MyTestModule";

            public override string DocString
                => "Test Doc";

            [PythonFastCallMethod(DocString = "Hallo!")]
            public unsafe static IntPtr TestMethod(IntPtr self, IntPtr argPtrs, int nargs)
            {
                var args = Python.GetArgs(argPtrs, nargs);
                IntPtr tuple = Python.PyTuple_New(3);
                Python.PyTuple_SetItem(tuple, 0, (PythonObject)"Das ist ein test");
                Python.PyTuple_SetItem(tuple, 1, (PythonObject)$"Hallo! {(int)args[0]}");
                Python.PyTuple_SetItem(tuple, 2, (PythonObject)$"Hallo! {(string)args[1]}");
                return tuple;
            }
        }
    }
}
