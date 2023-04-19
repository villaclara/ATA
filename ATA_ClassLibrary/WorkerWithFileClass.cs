using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ATA_ClassLibrary
{
    public static class WorkerWithFileClass
    {

        // write set process name to the file
        // then re-writes the file with the proper names of processes
        // file - "procs.txt" in the default folder of APP
        public static void AddProcessNameToFile (string processName, ProcInstanceClass[] arrProcs, int indexOfProcessToAddToFile)
        {
            string[] processesFromFile = ReadFromFileWithGivenName(DifferentFunctions.fileWithProcesses).Split(',');
            
            // set the appropriate processname at index
            processesFromFile[indexOfProcessToAddToFile] = processName;

            string returnString = "";
            // i < 5 -- because maximum 5 processes
            // for the update upTime file 'i' should be another
            for (int i = 0; i < 5; i++)
            {
                returnString += processesFromFile[i] + ','; 
            }

            File.WriteAllText(DifferentFunctions.fileWithProcesses, returnString);

        }

        // gets the string of process name from the file
        // index - digit of the textblock to display
        public static string getProcessFromFileWithGivenIndex (int index)
        {
            string[] procs = ReadFromFileWithGivenName(DifferentFunctions.fileWithProcesses).Split(',');
            if (procs[index] != "empty")
                return procs[index];

            return "";

        }


        // writes the processes list into default 'procs.txt' file
        // is at DELETE process button
        public static void writeProcessToFileAtIndex (int index, string name)
        {
            string[] procs = ReadFromFileWithGivenName(DifferentFunctions.fileWithProcesses).Split(',');
            procs[index] = name;

            string fullNames = "";
            for(int i = 0; i < 5; i++)
            {
                fullNames += procs[i] + ',';
            }

            File.WriteAllText(DifferentFunctions.fileWithProcesses, fullNames);
        }

        // create default procs file and fill it with the empty names
        // or when pressed 'RESET ALL' button
        public static void createDefault()
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(DifferentFunctions.fileWithProcesses)))
            {
                writer.Write("empty,empty,empty,empty,empty");
            }
        }


        // writes list of each process UPTIMES
        // to file with given array of processes
        public static void writeToFileInfoAboutProcs(ProcInstanceClass[] procs)
        {
            foreach (var proc in procs)
            {

                if (proc.ProcName == "")
                    return;

                // retrieve the list of uptimes 
                // it works because we are in the foreach 
                // so got another list of uptimes
                List<UpTime> ups = proc.retrieveListOfUpTimesForCurrentProcess(proc.fileNameToWriteInfo);
                
                // 
                // +1 minute if process is running
                // did not moved it into separate function because it was not working
                // probably because of copy of instance is given as parameter
                if (proc.IsRunning && proc.checkIfTodayDateWasAddedToUpTimesList())
                {
                    ups.Last().UpMinutes += 1;
                }

                // clears the file
                File.WriteAllText(proc.fileNameToWriteInfo, "");
                
                // writing each element of list
                foreach (var upTime in ups)
                {
                    using (FileStream fileStream = new FileStream(proc.fileNameToWriteInfo, FileMode.Append))
                    {
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        {
                            writer.Write(upTime.UpDate.ToString() + ',' + upTime.UpMinutes.ToString() + ',');
                        }
                    }
                }
            }
            
        }


        // writes the first time info to the file about process
        // after user SETS the process
        public static void writeToFileInfoAboutOneProcFromSetProcButton(ProcInstanceClass proc)
        {
            using (FileStream fileStream = new FileStream(proc.fileNameToWriteInfo, FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(DateOnly.FromDateTime(DateTime.Now).ToString() + ',' + proc.UpTimeMinutesCurrentSession.ToString() + ',');
                }
            }
        }


        // reads the file and return all lines as string
        public static string ReadFromFileWithGivenName(string fileNameToRead)
        {
            if (File.Exists(fileNameToRead))
            {
                using (StreamReader reader = new StreamReader(fileNameToRead))
                {
                    return reader.ReadToEnd();
                }
            }
            else return "";
        }


        // clears the file
        // 
        // used in Restart time button
        public static bool writeTimeForRestartButton (ProcInstanceClass proc)
        {
            File.WriteAllText(proc.fileNameToWriteInfo, DateOnly.FromDateTime(DateTime.Now) + ",0,");
            return true;
        }


        // writes the versions to a file 'version.json'
        public static void writeVersionFile (string currentVersion, string latestVersion)
        {
            // creating anonymous type and then serializing into json
            // anonymous type is needed to write CurrentVersion : x.x.x instead of Item1 : x.x.x
            string output = JsonConvert.SerializeObject(new { currentVersion, latestVersion}, Formatting.Indented);
            try
            {
                using StreamWriter sw = new(DifferentFunctions.BaseDir + $"\\version.json", false);
                sw.Write(output);
            }
            // need to think how to handle and where to display exception message
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // gets the values of current and latest versions from file 'version.json'
        public static (string, string) getVersionFromFile (string fileName)
        {
            string result = ReadFromFileWithGivenName(fileName);

            if (result == "")
                return ("undefined", "undefined");
            
            
            // creating anonymous type for deserialization
            var versions = new { currentVersion = "", latestVersion = "" };
            var re = JsonConvert.DeserializeAnonymousType(result, versions);

            // converting anonymous type fields into valuetuple
            (string, string) r = (re.currentVersion, re.latestVersion);
            return r;



        }
    }
}
