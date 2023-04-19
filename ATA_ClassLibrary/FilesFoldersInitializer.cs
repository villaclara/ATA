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


               
    }
}
