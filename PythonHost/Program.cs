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
                    "print(MyTestModule.TestMethod())\n");
            }
            Console.ReadLine();
        }

        class MyTestModule : ICostumPythonModule
        {
            public string Name => "MyTestModule";

            public string DocString => "Test Doc";

            [PythonMethod(DocString = "Hallo!")]
            public static IntPtr TestMethod(IntPtr self, IntPtr args)
            {
                return Python.PyLong_FromLong(2022);
            }
        }
    }
}
