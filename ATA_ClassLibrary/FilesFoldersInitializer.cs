using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATA_ClassLibrary
{
    // class is used to create if needed all files and folders
    // is only called at launch, checks everything and thats it
    // has 1 public and all private methods
    public class FilesFoldersInitializer
    {

        // main exe location
        private readonly string _locationExe = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        //private readonly string _locationExe = System.AppDomain.CurrentDomain.BaseDirectory;


        // main method that should call all private methods
        public void InitializeEverything()
        {
            // to set the starting directory as ATA/
            // not C:/windows/system32
            // idk if this worked or from UpdateChecker class
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            CreateBackupUpdateFolders();
            //CreateLogFile();

            // WRITES THE CURRENT VERSION INTO FILE
            WorkerWithFileClass.WriteVersionFile("0.1.4", "0.1.4");

        }

        private void CreateBackupUpdateFolders()
        {
            // create backup directory
            if (!Directory.Exists(_locationExe + "\\backup"))
                Directory.CreateDirectory(_locationExe + "\\backup");

            // create update directory
            if (!Directory.Exists(_locationExe + "\\update"))
                Directory.CreateDirectory(_locationExe + "\\update");
        }

        private void CreateLogFile()
        {
            if (!File.Exists(_locationExe + "\\log.txt"))
                File.Create(_locationExe + "\\log.txt");
        }


        // deletes the files from the given path to folder
        public void DeleteFilesFromFolder(string folderLoc)
        {
            var files = Directory.GetFiles(folderLoc);
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                   
                    // logging
                    LoggerService.Log($"Deleted file {file} from {folderLoc}.");
                }
            }
        }

        
        
               
    }
}
