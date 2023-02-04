using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public static class DifferentFunctions
    {
        

        public static List<Process> GetActiveProcesses()
        {
            return Process.GetProcesses().ToList();
        }


        public static List<string> GetActiveProcessesByNameString()
        {
            List<Process> procs = Process.GetProcesses().ToList();
            List<string> strings = new();
            foreach(var proc in procs)
            {
                strings.Add(proc.ProcessName);
            }
            return strings;
        }

    }
}
