using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public static class WorkerWithFileClass
    {
        public static void writeToFileWithGivenName(string fileNameToWrite, ProcInstanceClass procInstance)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            FileStream fileStream = File.Open(Path.Combine(Directory.GetCurrentDirectory(), fileNameToWrite), FileMode.Append);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.WriteLine(procInstance.UpTimeMinutesCurrentSession);
            writer.Flush();
            writer.Close();
            fileStream.Close();
            
            Console.WriteLine("File was written.");

        }
    }
}
