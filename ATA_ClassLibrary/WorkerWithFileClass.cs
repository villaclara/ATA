﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public static class WorkerWithFileClass
    {
        // writes to file with given FileName and process instance
        public static bool writeToFileWithGivenName(string fileNameToWrite, ProcInstanceClass procInstance)
        {
            //
            // TO DELETE AFTER MOVING TO WPF
            //
            Console.WriteLine(Directory.GetCurrentDirectory());
          
            using (FileStream fileStream = File.Open(Path.Combine(Directory.GetCurrentDirectory(), fileNameToWrite), FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    // calculate current session to record proper value to the file
                    procInstance.calculateUpTimeCurrentSession();
                    writer.Write(DateOnly.FromDateTime(DateTime.Now) + "," + procInstance.UpTimeMinutesCurrentSession + ",");
                    return true;

                    
                }
            }
        }


        // reads the file and return all lines as string
        public static string ReadFromFileWithGivenName(string fileNameToRead, ProcInstanceClass procInstance)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), fileNameToRead)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
