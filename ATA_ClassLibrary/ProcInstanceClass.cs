using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{

    // class for instance of each different process
    public class ProcInstanceClass
    {
        public Process? process;

        private string _procName;
        public string ProcName
        {
            get => _procName;
            set => _procName = value;
        }

        // constructor 1
        public ProcInstanceClass(Process _process)
        {
            process = _process;
            _procName = process.ProcessName;
        }

        // parameterless constructor 0
        public ProcInstanceClass() 
        {
            _procName = "";
            process = null;
        }

        // 
        public Process? getProcByName(string givenName)
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

        public void calculateUpTime()
        { 
        
        }

        public void WriteInfoToFile()
        {

        }


    }
}
