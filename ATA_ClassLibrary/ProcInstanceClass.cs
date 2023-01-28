using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{

    // class for instance of each different process
    public class ProcInstanceClass
    {

        // process instance of Process
        public Process? process;

        // separate field for Name
        private string _procName;
        //field for process uptime
        private int _upTimeMinutesCurrentSession;
        // field for name of related file to write
        private string _fileNameToWriteInfo;
        private DateTime startedTime;
        
        public string ProcName
        {
            get => process.ProcessName;
        }

        public int UpTimeMinutesCurrentSession
        {
            get 
            { 
                _upTimeMinutesCurrentSession = calculateUpTimeCurrentSession(); 
                return _upTimeMinutesCurrentSession; 
            }
        }


        // constructor 1
        public ProcInstanceClass(string procName)
        {
            process = getProcByName(procName);
            startedTime = process.StartTime;
            //_procName = process.ProcessName;
        }

        // parameterless constructor 0
        public ProcInstanceClass() 
        {
            //_procName = "";
            process = null;
        }

        // 
        public static Process? getProcByName(string givenName)
        {
            Process[] procs = Process.GetProcessesByName(givenName);
            foreach(var p in procs)
            {
                if (p.ProcessName == givenName)
                {
                    return p;
                }
            }

            return null;
        }

        public int calculateUpTimeCurrentSession()
        {    
            TimeSpan upDate = DateTime.Now - process.StartTime;
            return (int)upDate.TotalMinutes;
        }

        public void WriteInfoToFile(string fileNameToWrite)
        {

        }


    }
}
