using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using ATA_ClassLibrary.Tools;

namespace ATA_ClassLibrary.Models
{

    // class for instance of each different process
    public class ProcInstanceClass
    {
        //////////////////////
        // FIELDS
        //////////////////////

        public static string? SelectedProc;

        // bool if the process is currently running
        private bool _isRunning;

        // bool if the process was running previous minute
        private bool _isPreviousRunning;

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
        private List<UpTime> _upTimes = new List<UpTime>();


		//////////////////////
		// PROPERTIES
		//////////////////////


		public List<UpTime> UpTimes
        {
            get
            {
                _upTimes = RetrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);
                return _upTimes;
            }
            set => _upTimes = RetrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);
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
                _upTimeMinutesCurrentSession = CalculateUpTimeCurrentSession(); 
                return _upTimeMinutesCurrentSession; 
            }
        }

        public long TotalUpTime
        {
            get
            {
                _totalUpTime = CalculateTotalUpTime();
                return _totalUpTime;
            }
        }

        // if the process is currently running
        public bool IsRunning
        {
            get
            {
                _isRunning = DifferentFunctions.CheckIfProcessIsRunningWithStringName(_procName);
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                process = GetProcByName(ProcName);
            }
        }

        public bool IsPreviousRunning
        {
            get => _isPreviousRunning;
            set => _isPreviousRunning = value;
        }


        public int UpMinutesFromStartingProgram { get; set; }

        public DateOnly TodaysDateForStartingProgram { get; set; }


        //////////////////////
        // METHODS
        //////////////////////


        // constructor 1
        public ProcInstanceClass(string procName)
        {
            process = GetProcByName(procName);
            _procName = procName;
            fileNameToWriteInfo = Path.Combine(DifferentFunctions.BaseDir, procName + ".txt");
            //fileNameToWriteInfo = Path.Combine(Directory.GetCurrentDirectory(), procName + ".txt");
            

            IsRunning = true;
            IsPreviousRunning = false;

            // creating the file when calling the constructor for the given process
            if (!File.Exists(fileNameToWriteInfo))
            {
                // used FileStream because FILE returns a FileStream object and it should be closed
                FileStream fileStream = File.Create(fileNameToWriteInfo);
                fileStream.Close();
            }
            //_upTimes = RetrieveListOfUpTimesForCurrentProcess(fileNameToWriteInfo);

            SelectedProc = "";

            UpMinutesFromStartingProgram = 0;
            TodaysDateForStartingProgram = DateOnly.FromDateTime(DateTime.Now);


            process!.EnableRaisingEvents = true;

        }



        // parameterless constructor 0
        public ProcInstanceClass()
        {
            process = null;
            SelectedProc = "";
            _procName = "";
            _upTimes = new List<UpTime>();
            IsRunning = false;
            IsPreviousRunning = false;

        }

        // needed when the process is not running and APP is running
        public ProcInstanceClass(string name, bool isRunning)
        {
            _procName = name;

            IsRunning = isRunning;
            IsPreviousRunning = false;
            process = null;

            fileNameToWriteInfo = Path.Combine(DifferentFunctions.BaseDir, name + ".txt");

            //fileNameToWriteInfo = Path.Combine(Directory.GetCurrentDirectory(), name + ".txt");



        }

        // retrieve the process by the name and assing it to the calling instance
        public static Process? GetProcByName(string givenName)
        {
            Process[] procs = Process.GetProcessesByName(givenName);
            foreach (var p in procs)
            {
                if (p.ProcessName == givenName)
                {
                    return p;
                }
            }
            return null;
        }


        public void SetIsPreviousRunning()
        {
            if (IsRunning == true && IsPreviousRunning == false)
            {
                IsPreviousRunning = true;
            }

            if (IsRunning == false && IsPreviousRunning == true)
            {
                IsPreviousRunning = false;
            }

        }


        // calculate current UpTIme
        // Subtracts Time.Now - from StartTime.
        // is used in UpTimeMinutesCurrentSession property
        public int CalculateUpTimeCurrentSession()
        {
            if (!IsRunning)
                return 0;

            TimeSpan upDate = new TimeSpan();
            if (process != null)
            {
                upDate = DateTime.Now - process.StartTime;
            }

            // rounds to the closest bigger integer 
            return (int)Math.Ceiling(upDate.TotalMinutes); ;

        }

        //
        //
        // return list of total uptimes for previous sessions read from the file
        // 
        public List<UpTime> RetrieveListOfUpTimesForCurrentProcess(string fileNameToWrite)
        {
            _upTimes = new List<UpTime>();

            string lines = WorkerWithFileClass.ReadFromFileWithGivenName(fileNameToWrite);
            if (lines == null || lines == "")
            {
                _upTimes.Add(new UpTime(CalculateUpTimeCurrentSession(), DateOnly.FromDateTime(DateTime.Now)));
              
            }

            string[] allStrings = lines!.Split(',');

            for (int i = 0; i <= allStrings.Length - 2; i += 2)
            {
                UpTime up = new(Convert.ToInt64(allStrings[i + 1]), DateOnly.FromDateTime(Convert.ToDateTime(allStrings[i])));
                _upTimes.Add(up);     
            }

          
            if (!CheckIfTodayDateWasAddedToUpTimesList() && IsRunning == true)
            {
                _upTimes.Add(new UpTime(CalculateUpTimeCurrentSession(), DateOnly.FromDateTime(DateTime.Now))); 
            }


            return _upTimes;
        }


        // true if today's date is in the List of Uptimes
        public bool CheckIfTodayDateWasAddedToUpTimesList()
        {
			if (_upTimes.Last().UpDate == DateOnly.FromDateTime(DateTime.Now))
			{
				return true;
			}
			return false;
		}

     
        // 
        // calculate total uptime from file and current session
        private long CalculateTotalUpTime()
        {

			if (UpTimes == null)
				return UpTimeMinutesCurrentSession;

			long sum = 0;
			foreach (var item in UpTimes)
			{
				sum += item.UpMinutes;
			}
			return sum;
		}

    }
}
