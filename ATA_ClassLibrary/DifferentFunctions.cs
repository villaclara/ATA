﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public static class DifferentFunctions
    {
        public static string BaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;

        // path for the procs file with names of all selected processes
        // it will be - Default path to the folder of project and procs.txt name
        public static string fileWithProcesses = BaseDir + "\\procs.txt";

        // gets list of active processes
        public static List<Process> GetActiveProcesses()
        {
            return Process.GetProcesses().ToList();
        }

        // gets list of active process names
        public static List<string> GetActiveProcessesByNameString()
        {
            List<Process> procs = GetActiveProcesses();
            List<string> strings = new();
            
            foreach(var proc in procs)
            {
                strings.Add(proc.ProcessName);
            }
            return strings;
        }

        // retrieving active procs list and then checks if the given procName is running
        public static bool CheckIfProcessIsRunningWithStringName (string procName)
        {
            List<string> procList = GetActiveProcessesByNameString();
            foreach(var proc in procList)
            {
                if (proc == procName)
                    return true;
            }
            return false;
        }


       //
       // resets total uptime of the process 
       // and calls the function from WorkerWithFileClass to delete array from file
       public static bool ResetTotalUptime(ProcInstanceClass procInstance)
        {
            // if the process is running then we return false and show the messagebox which requires to close the process
            if (CheckIfProcessIsRunningWithStringName(procInstance.ProcName))
            {
                return false;
            }

            // if the file is not written then false is returned
            if (!WorkerWithFileClass.WriteTimeForRestartButton(procInstance))
            {
                return false;
            }

            // if everything is ok - we return true
            return true;          
        }

    }
}
