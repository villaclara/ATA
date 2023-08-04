using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary.Models
{
    public class FileToDownloadModel
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public FileToDownloadModel(string name, string link)
        {
            Name = name;
            Link = link;
        }
    }
}
