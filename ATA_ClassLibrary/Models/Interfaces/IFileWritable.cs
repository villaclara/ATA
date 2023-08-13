using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary.Models.Interfaces
{
    public interface IFileWritable
    {
        string FileToWrite { get; set; }
        void WriteUptimeToFile();

        void CreateFileToWrite();
    }
}
