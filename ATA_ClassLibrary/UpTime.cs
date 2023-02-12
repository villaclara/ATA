using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    public class UpTime
    {
        public long UpMinutes { get; set; }
        public DateOnly UpDate { get; set; }

        public UpTime(long UpMinutes, DateOnly UpDate)
        {
            this.UpMinutes = UpMinutes;
            this.UpDate = UpDate;
        }
    }
}
