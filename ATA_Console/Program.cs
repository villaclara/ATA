﻿using System.Timers;

namespace ATA_Console
{
    internal class Program
    {

        [STAThread]
        static async Task Main(string[] args)
        {
            ////ProcInstanceClass selectedProcess = new();
            //bool wantExit = false;

            //Console.WriteLine("Enter name -> ");
            //string? name = Console.ReadLine();

            //Console.WriteLine("nane {0}", name);
            //ProcInstanceClass selectedProcess = new ProcInstanceClass(name);
            //selectedProcess.process.Exited += ProcEnded;
            ////selectedProcess.ProcName = selectedProcess.process.ProcessName;

            //while (!wantExit)
            //{
            //    var timer = new System.Timers.Timer(2000);
            //    timer.Elapsed += Timer_Elapsed;
            //    timer.AutoReset = true;
            //    timer.Enabled= true;

            //    Console_Output_Class.DisplayOptions();

            //    ConsoleKeyInfo pressedKey = Console.ReadKey();
            //    Console.WriteLine(Environment.NewLine);

            //    switch (pressedKey.Key)
            //    {
            //        case ConsoleKey.R:
            //            if (selectedProcess.ProcName == "")
            //            {
            //                Console.WriteLine("Process is missing.");
            //                break;
            //            }
            //            Console.WriteLine($"Process - {selectedProcess.process.ProcessName} - StartTime - {DateTime.Now - selectedProcess.process.StartTime} - Uptime - {selectedProcess.UpTimeMinutesCurrentSession}");
            //            Console.WriteLine(selectedProcess.CalculateUpTimeCurrentSession());

            //            //selectedProcess.RetrieveListOfUpTimesForCurrentProcess(Path.Combine(Directory.GetCurrentDirectory(), "process1.txt"));

            //            Console.WriteLine($"TOTAL UPTIME - {selectedProcess.TotalUpTime}");

            //            break;

            //        case ConsoleKey.N:
            //            if (WorkerWithFileClass.writeToFileWithGivenName("process1.txt", selectedProcess))
            //                Console_Output_Class.DisplayMessage("The time was written.");
            //            else Console_Output_Class.DisplayMessage("Time was NOT written.");
            //            break;


            //        case ConsoleKey.X:
            //            wantExit = true;
            //            break;

            //        case ConsoleKey.C:
            //            selectedProcess.RetrieveListOfUpTimesForCurrentProcess("process1.txt");

            //            break;

            //        default:
            //            Console_Output_Class.DisplayMessage("Please enter correct.");
            //            Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()));
            //            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            //            break;
            //    }


            //}

            ////Console.WriteLine("HELO");
            //await Console.Out.WriteLineAsync("Starting new app.");
            //Application.Current.Shutdown();
            

            //await Console.Out.WriteLineAsync("App started.");
            //System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);

            //Console.WriteLine("Closing this app");
            //Console.WriteLine("HELO");
            //var client = new HttpClient();
            //string page = await client.GetStringAsync(@"http://ataidentifier.great-site.net/index.html");

            //Console.WriteLine(page);
            //Console.ReadLine();

            //WebBrowser browser = new WebBrowser();
            //browser.Navigate("http://ataident.byethost11.com/index.html");
            //var aa = browser.Document;
            //string ss = aa.ToString();
            //Console.WriteLine(ss);


            // IMBA
            //var httpClient = new HttpClient();
            //var publicFolderId = "11owhMTSElIkavHKBC517PUpdQU0m2nqn";
            //var googleDriveApiKey = "AIzaSyA2RGmLbJIiNRVhuKNMaa9VYDBB7sKoknk";
            //var nextPageToken = "";


            //var fileId = "";
            //var client = new WebClient();


            //do
            //{
            //    var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{publicFolderId}'+in+parents&key={googleDriveApiKey}";
            //    if (!String.IsNullOrEmpty(nextPageToken))
            //    {
            //        folderContentsUri += $"&pageToken={nextPageToken}";
            //    }
            //    var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
            //    var contents = (JObject)JsonConvert.DeserializeObject(contentsJson);
            //    nextPageToken = (string)contents["nextPageToken"];
            //    foreach (var file in (JArray)contents["files"])
            //    {
            //        var id = (string)file["id"];
            //        fileId = (string)file["id"];
            //        var name = (string)file["name"];
            //        Console.WriteLine($"{id}:{name}");
            //        var linkk = @"https://drive.google.com/uc?export=download&id=" + fileId;
            //        client.DownloadFile(linkk, "upd\\" + name);
            //    }
            //} while (!String.IsNullOrEmpty(nextPageToken));


            
            // TRY TO DOWNLOAD ATA_WPF NORMALLY USING HTTPCLIENT
            var client = new HttpClient();
            // https://drive.google.com/file/d/1HVKbLeXrlo6IiurhzCTPEq-lPPRgmk1r/view?usp=share_link
            //var link = @"https://drive.google.com/uc?export=download&id=1HVKbLeXrlo6IiurhzCTPEq-lPPRgmk1r";

            // https://www.googleapis.com/drive/v3/files/FileID?alt=media&key=APIKey
            var link2 = @"https://www.googleapis.com/drive/v3/files/1HVKbLeXrlo6IiurhzCTPEq-lPPRgmk1r?alt=media&key=AIzaSyA2RGmLbJIiNRVhuKNMaa9VYDBB7sKoknk";
            var downloaded = await client.GetByteArrayAsync(link2);
            System.IO.File.WriteAllBytes(".\\upd\\ATA_WPF.txt", downloaded);

        }

        private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {

            Console.WriteLine("Timer elapsed");
        }

        public static void ProcEnded(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Ended");
        }
    }
}