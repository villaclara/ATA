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
        //////////////////////
        // FIELDS
        //////////////////////

        //public string boundFileName;

        // process instance of Process
        public Process? process;
       
        //field for process uptime
        private int _upTimeMinutesCurrentSession;
       
        // field for name of related file to write
        public string _fileNameToWriteInfo;

        // field for keeping total uptime for the all time
        private long _totalUpTime;

        // field for all uptimes
        private List<UpTime> _upTimes;


        //////////////////////
        // PROPERTIES
        //////////////////////


        public List<UpTime> UpTimes
        {
            get => _upTimes;
            set => _upTimes = retrieveListOfUpTimesForCurrentProcess(_fileNameToWriteInfo);
        }
        
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

        public long TotalUpTime
        {
            get
            {
                _totalUpTime = calculateTotalUpTime();
                return _totalUpTime;
            }
        }



        //////////////////////
        // METHODS
        //////////////////////


        // constructor 1
        public ProcInstanceClass(string procName)
        {
            process = getProcByName(procName);
            _fileNameToWriteInfo = Path.Combine(Directory.GetCurrentDirectory(), procName + ".txt");
            
            // creating the file when calling the constructor for the given process
            if (!File.Exists(_fileNameToWriteInfo))
            { 
                // used FileStream because FILE returns a FileStream object and it should be closed
               FileStream fileStream = File.Create(_fileNameToWriteInfo);
                fileStream.Close();
            }
                  
            _upTimes = retrieveListOfUpTimesForCurrentProcess(_fileNameToWriteInfo);
        }

        // parameterless constructor 0
        public ProcInstanceClass() 
        {
            process = null;
        }


        // retrieve the process by the name and assing it to the calling instance
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


        // calculate current UpTIme
        // Subtracts Time.Now - from StartTime.
        // is used in UpTimeMinutesCurrentSession property
        public int calculateUpTimeCurrentSession()
        {    
            TimeSpan upDate = DateTime.Now - process.StartTime;
            return (int)upDate.TotalMinutes;
        }


        // return list of total uptimes for previous sessions read from the file
        public List<UpTime> retrieveListOfUpTimesForCurrentProcess(string fileNameToWrite)
        {
            string lines = WorkerWithFileClass.ReadFromFileWithGivenName(fileNameToWrite, this);
            string[] allStrings = lines.Split(',');
            _upTimes = new List<UpTime>();

            for (int i = 0; i < allStrings.Length - 2; i += 2)
            {
                UpTime up = new UpTime(Convert.ToInt64(allStrings[i + 1]), DateOnly.FromDateTime(Convert.ToDateTime(allStrings[i])));
                Console.WriteLine($"up min - {up.UpMinutes} at {up.UpDate}");
                _upTimes.Add(up);     
            }
            return _upTimes;
        }


        // calculate total uptime from file and current session
        private long calculateTotalUpTime()
        {
            long sum = 0;
            foreach(var item in UpTimes)
            {
                sum += item.UpMinutes;
            }
            return sum + UpTimeMinutesCurrentSession;
        }



    }
}
