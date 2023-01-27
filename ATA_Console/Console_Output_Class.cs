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
        public static void WriteInfo(string message) => Console.WriteLine(message);
    }
}
