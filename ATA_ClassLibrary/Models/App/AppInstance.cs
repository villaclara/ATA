using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATA_ClassLibrary.Models.Interfaces;
using ATA_ClassLibrary.Tools;

namespace ATA_ClassLibrary.Models.App
{
	public class AppInstance : AppInstanceBase, IFileWritable
    {

        // string AppName  
        // int ActiveAps 
        // --- is inherited by AppInstanceBase


        public AppUpTime AppUpTime { get; set; }
		

		#region IfileToWrite interface implementation

		// implementaion IFILETOWRITE
		public string FileToWrite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void CreateFileToWrite()
		{
			throw new NotImplementedException();
		}

		public void WriteUptimeToFile()
        {
            throw new NotImplementedException();
        }

		#endregion

	}
}
