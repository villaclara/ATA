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
        public Process process;

        private string _procName;
        public string ProcName
        {
            get => _procName;
            set => _procName = value;
        }

        public ProcInstanceClass(Process _process)
        {
            process = _process;
        }

        public ProcInstanceClass() 
        {
            _procName = null;
        }
    }
}
