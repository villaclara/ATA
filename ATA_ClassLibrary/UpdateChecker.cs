﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ATA_ClassLibrary
{    
    
    // UPDATE FLOW
     // 0 - gets and initializes current version in constructor
     // 1 - gets into google drive folder and downloads all the files from it
     // 2 - grabs the downloaded version.json and retrieves the version of latest build
     // 3 - compares two versions between them
     // 4 - if the version is higher then asks if the user want to update the app
     // 5 - if the user wants to update then replaces all the files from update folder into main folder
     // 6 - the current files go to backup folder 
     // 7 - the app is asked to be restarted


    public class UpdateChecker
    {
        // members for versions
        private Version _currentV;
        private Version _latestV;

        // list of files to be downloaded when updating
        List<FileToDownloadModel> files = new List<FileToDownloadModel>();

        // default location where the file version.json is 
        private readonly string _location = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        // directory of backup and update folders
        private string _backupLoc = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\backup";
        private string _updateLoc = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\update";

        private readonly string _googleDriveLoc = @"https://drive.google.com/drive/u/0/folders/11owhMTSElIkavHKBC517PUpdQU0m2nqn";

        private readonly HttpClient hclient = new();

        public string IsUpdateNeeded;
        public bool IsUpd = false;
        public bool Restart = false;

        // ctor
        // gets the backup and update directories to be created
        // initializes the current version
        public UpdateChecker()
        {


            // check if folders is not created then creates it
            FilesFoldersInitializer initializer = new();
            initializer.InitializeEverything();

            // retrieve and assing current version
            // assing latest version new object
            _currentV = GetVersion(_location);
            _latestV = new Version();

            IsUpdateNeeded = $"No update is needed. The App is launching now. cur - {_currentV}, new - {_latestV}";

        }



    


        // !!!!!!!!!!!!!!!!!!!!!!!!!
        // SHOULD START HERE
        // !!!!!!!!!!!!!!!!!!!!!!!!!
        public async Task StartCheckingUpdate()
        {
            // first clears the update folder
            DeleteFilesFromUpdateFolder(_updateLoc);

            // downloads all the needed files to the update folder
            await DownloadFilesToUpdateFolderAsync();

            // assign the latest version 
            _latestV = GetVersion(_updateLoc);

            


        }


        // return true if the latest is bigger
        // false if latest is lower or equal current
        public bool CompareVersion() => _currentV < _latestV;


        // gets the Current version string from file and returns it
        // is called for current and for the downloaded in update folder
        // folder location is passed as parameter
        private Version GetVersion(string directoryPath)
        {
            (string cur, string lat) = WorkerWithFileClass.getVersionFromFile(directoryPath + "\\version.json");
            return new Version(cur);
        }



        // is called at first
        // connects to the google api
        // and downloads the all files to the update folder
        // update folder id is - 11owhMTSElIkavHKBC517PUpdQU0m2nqn 
        //                       11owhMTSElIkavHKBC517PUpdQU0m2nqn
        // google api key - AIzaSyA2RGmLbJIiNRVhuKNMaa9VYDBB7sKoknk
        private async Task DownloadFilesToUpdateFolderAsync()
        {            
                    
            // initializes new httpclient and folder ID and google drive api key
            var httpClient = new HttpClient();
            var publicFolderId = "11owhMTSElIkavHKBC517PUpdQU0m2nqn";
            var googleDriveApiKey = "AIzaSyA2RGmLbJIiNRVhuKNMaa9VYDBB7sKoknk";
            var nextPageToken = "";
            
            // retrieving all files inside folder and adding new FileToDownloadModel to the list of it
            do
            {
                var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{publicFolderId}'+in+parents&key={googleDriveApiKey}";
                if (!String.IsNullOrEmpty(nextPageToken))
                {
                    folderContentsUri += $"&pageToken={nextPageToken}";
                }
                var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
                var contents = (JObject)JsonConvert.DeserializeObject(contentsJson);
                nextPageToken = (string)contents["nextPageToken"];
                foreach (var file in (JArray)contents["files"])
                {
                    var id = (string)file["id"];
                    var name = (string)file["name"];
                    var linkk = @"https://drive.google.com/uc?export=download&id=" + id;
                    files.Add(new FileToDownloadModel(name, linkk));
                }
            } while (!String.IsNullOrEmpty(nextPageToken));


            // new list of tasks to be performed at the same time
            // adding and running each task
            List<Task> tasks = new();

            foreach(var f in files)
            {
                tasks.Add(Task.Run(() => DownloadFile(f)));
            }

            // waiting for all tasks to be done
            await Task.WhenAll(tasks);
        }

        // download the file function
        // return FileToDownload object
        private async Task DownloadFile(FileToDownloadModel f)
        {
            try
            {
                var downloaded = await hclient.GetByteArrayAsync(f.Link);
                File.WriteAllBytes(".\\update\\" + f.Name, downloaded);
            }
            catch
            {
                IsUpdateNeeded = "error when trying to check update.";
                return;
            }

        }



        // deletes the files from update folder 
        // to prevent for stacking files
        private void DeleteFilesFromUpdateFolder(string folder)
        {
            var files = Directory.GetFiles(folder);
            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }


        // gets all the files in given folder
        // and tries to move them into destination folder
        private void GetAndMoveFilesFromTo(string sourceDirectory, string  destinationDirectory)
        {
            var files = Directory.EnumerateFiles(sourceDirectory);

            foreach (var file in files)
            { 
                FileInfo f = new FileInfo(file);

                if (f.Extension == ".txt")
                    continue;

                if (f.Exists)
                {
                    f.MoveTo(Path.Combine(destinationDirectory, f.Name));
                }

            }
        }


        // one async method for all the moving stuff
        // 1. deletes files from backup folder
        // 2. moves files from _location -> _backup
        // 3. moves files _update -> _location
        public async Task PerformMovingFiles()
        {
            await Task.Run(new Action (() => DeleteFilesFromUpdateFolder(_backupLoc)));
            
            await Task.Run(() => GetAndMoveFilesFromTo(_location, _backupLoc));

            await Task.Run(new Action(() => GetAndMoveFilesFromTo(_updateLoc, _location)));

            await Task.Delay(1000);
        }
    }
}
