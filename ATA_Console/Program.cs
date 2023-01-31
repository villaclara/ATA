
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
            //ProcInstanceClass selectedProcess = new();
            bool wantExit = false;

            Console.WriteLine("Enter name -> ");
            string? name = Console.ReadLine();

                Console.WriteLine("nane {0}", name);
                ProcInstanceClass selectedProcess = new ProcInstanceClass(name);
                //selectedProcess.ProcName = selectedProcess.process.ProcessName;
           
            while (!wantExit)
            {

                Console_Output_Class.DisplayOptions();

                ConsoleKeyInfo pressedKey = Console.ReadKey();
                Console.WriteLine(Environment.NewLine);
               
                switch (pressedKey.Key)
                {
                    case ConsoleKey.R:
                        if (selectedProcess.ProcName == "")
                        {
                            Console.WriteLine("Process is missing.");
                            break;
                        }
                        Console.WriteLine($"Process - {selectedProcess.process.ProcessName} - StartTime - {DateTime.Now - selectedProcess.process.StartTime} - Uptime - {selectedProcess.UpTimeMinutesCurrentSession}");
                        Console.WriteLine(selectedProcess.calculateUpTimeCurrentSession());
                        
                        break;

                    case ConsoleKey.N:
                        WorkerWithFileClass.writeToFileWithGivenName("process1.txt", selectedProcess);
                        break;


                    case ConsoleKey.X:
                        wantExit = true;
                        break;

                    default:
                        Console_Output_Class.DisplayMessage("Please enter correct.");
                        Console.WriteLine(Directory.GetParent(Directory.GetCurrentDirectory()));
                        Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                        break;
                }

               
            }

            

        }
    }
}