using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           //using (FileStream fileStream = File.OpenWrite(DifferentFunctions.fileWithProcesses))
            //{
            //    using (StreamWriter writer = new StreamWriter(fileStream))
            //    {
            //        writer.Write(returnString);
            //    }
            //}

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

        // create default procs file and fill it with the empty names
        public static void createDefault()
        {
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(DifferentFunctions.fileWithProcesses)))
            {
                writer.Write("empty,empty,empty,empty,empty");
            }
        }

        // writes to file with given FileName and process instance
        public static bool writeToFileWithGivenName(string fileNameToWrite, ProcInstanceClass procInstance)
        {

          
            using (FileStream fileStream = File.OpenWrite(Path.Combine(Directory.GetCurrentDirectory(), fileNameToWrite)))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    // calculate current session to record proper value to the file
                    procInstance.calculateUpTimeCurrentSession();
                    if (!checkIfTodayWasAlreadyWritten("", procInstance))
                    {


                        writer.Write(DateOnly.FromDateTime(DateTime.Now) + "," + procInstance.UpTimeMinutesCurrentSession + ",");
                        return true;

                    }


                    return true;
                    
                }
            }
        }


        // reads the file and return all lines as string
        public static string ReadFromFileWithGivenName(string fileNameToRead)
        {
            if (File.Exists(fileNameToRead))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), fileNameToRead)))
                {
                    return reader.ReadToEnd();
                }
            }
            return "";
        }


        // NOT DONE
        // 
        //
        public static bool checkIfTodayWasAlreadyWritten (string readed, ProcInstanceClass procInstance)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            string[] allStrings = readed.Split(',');

            List<DateOnly> dates = new List<DateOnly>();

            //for (int i = 0; i < allStrings.Length - 2; i += 2)
            //{
            //    UpTime up = new UpTime(Convert.ToInt64(allStrings[i + 1]), DateOnly.FromDateTime(Convert.ToDateTime(allStrings[i])));
            //    Console.WriteLine($"up min - {up.UpMinutes} at {up.UpDate}");
                
            //}

           foreach (var time in procInstance.UpTimes)
            {
                if (time.UpDate == today)
                    return true;
            }

            return false;
        }
    }
}
