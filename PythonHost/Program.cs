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
                    "print(MyTestModule.TestMethod(1))\n");
            }
            Console.ReadLine();
        }


        class MyTestModule : CostumPythonModule
        {
            public override string Name
                => "MyTestModule";

            public override string DocString
                => "Test Doc";

            [PythonMethod(DocString = "Hallo!")]
            public static IntPtr TestMethod(IntPtr self, IntPtr args)
            {
                return (PythonObject)2022;
            }
        }
    }
}
