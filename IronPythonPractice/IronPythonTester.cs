using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IronPython.Hosting;

namespace IronPythonPractice
{
    class IronPythonTester
    {
        public IronPythonTester()
        {
            Console.WriteLine("Press enter to execute the python script!");
            Console.ReadLine();

            //SimpleTest();
            DynamicTest();
            //ReturnTest();

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Retrieve a return value from function in script
        /// </summary>
        private void ReturnTest()
        {
            var py = Python.CreateRuntime();
            try
            {
                dynamic test = py.UseFile("script3.py");
                int instance = test.foo();
                Console.WriteLine("Foo returned {0}", instance);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Execute Single function in script
        /// </summary>
        private void DynamicTest()
        {
            var py = Python.CreateRuntime();
            try
            {
                dynamic test = py.UseFile("script2.py");
                test.foo();
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        /// <summary>
        /// Execute entire script
        /// </summary>
        private void SimpleTest()
        {
            var py = Python.CreateEngine();
            try
            {
                py.ExecuteFile("script.py");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
    }
}
