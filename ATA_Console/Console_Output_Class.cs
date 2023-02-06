using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_Console
{
    // STATIC class
    // need to write functions that will write messages to console
    // it only uses in the Console version of application
    public static class Console_Output_Class
    {
        public static void DisplayMessage(string message) => Console.WriteLine(message);

        public static void DisplayOptions()
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("R to refresh \n N to set new \n X to exit \n -> ");

            Console.WriteLine(value: "Hello");
        }
    }
}
