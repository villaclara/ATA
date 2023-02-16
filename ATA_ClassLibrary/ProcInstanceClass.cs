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

        public static string? selectedProc;

        // bool if the process is currently running
        private bool _isRunning;

        // private string Name for process
        // should have separate name as the Process process could not be running so it will return an error
        private string _procName;

        // process instance of Process
        public Process? process;
       
        //field for process uptime
        private int _upTimeMinutesCurrentSession;
       
        // field for name of related file to write
        public string fileNameToWriteInfo;

        // field for keeping total uptime for the all time
        private long _totalUpTime;

        // field for all uptimes
        private List<UpTime> _upTimes;


        //////////////////////
        // PROPERTIES
        //////////////////////


        public List<UpTime> UpTimes
        {
            get
            {
                _upTimes = retrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);
                return _upTimes;
            }
            set => _upTimes = retrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);
        }
        
        public string ProcName
        {
            get => _procName;
            set => _procName = value;
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

        // if the process is currently running
        public bool IsRunning
        {
            get
            {
                _isRunning = DifferentFunctions.checkIfProcessIsRunningWithStringName(_procName);
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                process = getProcByName(ProcName);
            }
        }

        // for the event when process has started
        public event EventHandler ProcessStartedEvent;

        public int UpMinutesFromStartingProgram { get; set; }

        public DateOnly TodaysDateForStartingProgram { get; set; }


        //////////////////////
        // METHODS
        //////////////////////


        // constructor 1
        public ProcInstanceClass(string procName)
        {
            process = getProcByName(procName);
            _procName = procName;
            fileNameToWriteInfo = Path.Combine(Directory.GetCurrentDirectory(), procName + ".txt");
            //fileNameToWriteInfo = procName + ".txt";

            IsRunning = true;

            // creating the file when calling the constructor for the given process
            if (!File.Exists(fileNameToWriteInfo))
            { 
                // used FileStream because FILE returns a FileStream object and it should be closed
               FileStream fileStream = File.Create(fileNameToWriteInfo);
                fileStream.Close();
            }
            //_upTimes = retrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);

            selectedProc = "";

            UpMinutesFromStartingProgram = 0;
            TodaysDateForStartingProgram = DateOnly.FromDateTime(DateTime.Now);

            //process.Exited += OnProcessStarted;

        }

        

        // parameterless constructor 0
        public ProcInstanceClass() 
        {
            process = null;
            selectedProc = "";
            _procName = "";
            _upTimes = new List<UpTime>();
            IsRunning = false;
             
        }

        // needed when the process is not running and APP is running
        public ProcInstanceClass(string name, bool isRunning)
        {
            _procName = name;
           
            IsRunning = isRunning;
            process = getProcByName(name);
            //fileNameToWriteInfo = name + ".txt";
            fileNameToWriteInfo = Path.Combine(Directory.GetCurrentDirectory(), name + ".txt");

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
            if (!IsRunning)
                return 0;

            TimeSpan upDate = DateTime.Now - process.StartTime;

            // rounds to the closest bigger integer 
            return (int)Math.Ceiling(upDate.TotalMinutes); ;
            
        }

        //
        //
        // return list of total uptimes for previous sessions read from the file
        // 
        public List<UpTime> retrieveListOfUpTimesForCurrentProcess(string fileNameToWrite)
        {
            _upTimes = new List<UpTime>();

            string lines = WorkerWithFileClass.ReadFromFileWithGivenName(fileNameToWrite);
            if (lines == null || lines == "")
            {
                _upTimes.Add(new UpTime(calculateUpTimeCurrentSession(), DateOnly.FromDateTime(DateTime.Now)));
              
            }

            string[] allStrings = lines.Split(',');

            for (int i = 0; i <= allStrings.Length - 2; i += 2)
            {
                UpTime up = new UpTime(Convert.ToInt64(allStrings[i + 1]), DateOnly.FromDateTime(Convert.ToDateTime(allStrings[i])));
                _upTimes.Add(up);     
            }

          
            if (!checkIfTodayDateWasAddedToUpTimesList() && IsRunning == true)
            {
                _upTimes.Add(new UpTime(calculateUpTimeCurrentSession(), DateOnly.FromDateTime(DateTime.Now))); 
            }
            

            return _upTimes;
        }


        // true if today's date is in the List of Uptimes
        public bool checkIfTodayDateWasAddedToUpTimesList()
        {
           if (_upTimes.Last().UpDate == DateOnly.FromDateTime(DateTime.Now))
            { 
                return true;
            }
            return false;
        }

     


        // PROBABLY OK
        // 
        // 
        // calculate total uptime from file and current session
        private long calculateTotalUpTime()
        {
            long sum = 0;
            if (UpTimes == null)
                return UpTimeMinutesCurrentSession;
           
            foreach(var item in UpTimes)
            {
                sum += item.UpMinutes;
            }
            return sum;
        }



    }
}
