
using ATA_ClassLibrary;
using ATA_WPF;
using System;
using System.Diagnostics;
using System.Timers;

namespace ATA_Console
{
    internal class Program
    {

        static void Main(string[] args)
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
            //            Console.WriteLine(selectedProcess.calculateUpTimeCurrentSession());

            //            //selectedProcess.retrieveListOfUpTimesForCurrentProcess(Path.Combine(Directory.GetCurrentDirectory(), "process1.txt"));

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
            //            selectedProcess.retrieveListOfUpTimesForCurrentProcess("process1.txt");

            //            break;

            //        default:
            //            Console_Output_Class.DisplayMessage("Please enter correct.");
            //            Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()));
            //            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            //            break;
            //    }


            //}

            bool toCreate = true;
            string[] strs = WorkerWithFileClass.ReadFromFileWithGivenName(DifferentFunctions.fileWithProcesses).Split(',');
            foreach (string str in strs)
            {
                if (str != "empty" && str != "")
                    toCreate = false;
            }
            if (toCreate)
            {
                WorkerWithFileClass.createDefault();
            }


            ProcInstanceClass[] processArray = new ProcInstanceClass[5];
            processArray[1] = new ProcInstanceClass("devenv");
            WorkerWithFileClass.AddProcessNameToFile(processArray[1].ProcName, processArray, 1);


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