using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATA_ClassLibrary.Tools
{
    // class for logging info
    public static class LoggerService
    {
        private readonly static string _loggerFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\log.txt";

        public static void Log(string message)
        {
            using var sw = new StreamWriter(_loggerFile, true);
            string now = DateTime.Now.ToString();
            sw.WriteLine(now + " - " + message);
        }

        public static void ClearLogFile()
        {
            using StreamWriter sw = new(_loggerFile, false);
            sw.Write("");
        }
    }
}
