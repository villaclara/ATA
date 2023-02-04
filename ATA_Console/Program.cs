
using ATA_ClassLibrary;
using ATA_WPF;
using System;
using System.Diagnostics;


namespace ATA_Console
{
    internal class Program
    {
        static string equalString = "=====================================";

        static void Main(string[] args)
        {
            ////ProcInstanceClass selectedProcess = new();
            //bool wantExit = false;

            //Console.WriteLine("Enter name -> ");
            //string? name = Console.ReadLine();

            //Console.WriteLine("nane {0}", name);
            //ProcInstanceClass selectedProcess = new ProcInstanceClass(name);
            //    //selectedProcess.ProcName = selectedProcess.process.ProcessName;

            //while (!wantExit)
            //{

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

            int sum = 0;
            var procs = Process.GetProcesses();
            foreach (var proc in procs)
            {
                try
                {
                    Console.WriteLine(proc.MainModule.ModuleName);
                    sum++;
                }
                catch(Exception ex) { 
                    //Console.WriteLine(ex.ToString());
                    }
            }


            Console.WriteLine($"sum - {sum}, amount - {procs.Length}");
            var p = Process.GetProcessesByName("steam");
            Console.WriteLine(p[0].Modules + p[0].Handle.ToString());
            

        }
    }
}