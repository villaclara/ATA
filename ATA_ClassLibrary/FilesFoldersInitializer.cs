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
        private readonly string LocationExe = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        // main method that should call all private methods
        public void InitializeEverything()
        {
            CreateBackupUpdateFolders();
            CreateLogFile();
        }

        private void CreateBackupUpdateFolders()
        {
            // create backup directory
            if (!Directory.Exists(LocationExe + "\\backup"))
                Directory.CreateDirectory(LocationExe + "\\backup");

            // create update directory
            if (!Directory.Exists(LocationExe + "\\update"))
                Directory.CreateDirectory(LocationExe + "\\update");

        }

        private void CreateLogFile()
        {
            if (!File.Exists(LocationExe + "\\log.txt"))
                File.Create(LocationExe + "\\log.txt");
        }


        // deletes the files from the given path to folder
        public void DeleteFilesFromFolder(string _folderLoc)
        {
            var files = Directory.GetFiles(_folderLoc);
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                   
                    // logging
                    LoggerService.Log($"Deleted file {file} from {_folderLoc}.");
                }
            }
        }

        
        
               
    }
}
