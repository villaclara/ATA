using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATA_ClassLibrary
{
    // class for logging info
    public static class LoggerService
    {
        private readonly static string _loggerFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\log.txt";

        //static LoggerService()
        //{
        //    File.WriteAllText(_loggerFile, "");
        //}

        public static void Log(string message)
        {
            using StreamWriter sw = new StreamWriter(_loggerFile, true);
            string now = DateTime.Now.ToString();
            sw.WriteLine(now + " - " + message);
        }

        public static void ClearLogFile()
        {
            if (File.Exists(_loggerFile))
            {
                File.WriteAllText(_loggerFile, string.Empty);
            }
        }
    }
}
