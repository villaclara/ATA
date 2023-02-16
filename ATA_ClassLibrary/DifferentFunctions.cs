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
        // path for the procs file with names of all selected processes
        // it will be - Default path to the folder of project and procs.txt name
        public static string fileWithProcesses = Path.Combine(Directory.GetCurrentDirectory() + "\\procs.txt");

        // gets list of active processes
        public static List<Process> GetActiveProcesses()
        {
            return Process.GetProcesses().ToList();
        }

        // gets list of active process names
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

        // retrieving active procs list and then checks if the given procName is running
        public static bool checkIfProcessIsRunningWithStringName (string procName)
        {
            List<string> procList = GetActiveProcessesByNameString();
            foreach(var proc in procList)
            {
                if (proc == procName)
                    return true;
            }
            return false;
        }

        // NOT USED YET
        public static bool checkIfProcessIsRunningWithProcessVar (Process process)
        {
            List<Process> procs = GetActiveProcesses();
            foreach(var p in procs)
            {
                if (p.ProcessName == process.ProcessName)
                    return true;
            }
            return false;
        }



        // NOT USED ANYWHERE
        // 
        // the realisation of this method was moved to 
        // WorkerWithFileClass.writeToFileInfoAboutProcs(ProcInstance[] procs)
        public static void AddMinuteToTodaysTimeIfTheProcessIsRunning(ProcInstanceClass proc)
        {
            if (proc.IsRunning && proc.checkIfTodayDateWasAddedToUpTimesList())
            {
                proc.UpTimes.Last().UpMinutes += 1;    
            }           
        }

    }
}
