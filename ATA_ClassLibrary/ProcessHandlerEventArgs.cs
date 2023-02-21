using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public class ProcessHandlerEventArgs : EventArgs
    {
        private int _procIndex;

        public int ProcIndex { get => _procIndex; }

       // public Process process;

        

        public ProcessHandlerEventArgs(int procIndex)
        {
            _procIndex = procIndex;
        }


    }
}
