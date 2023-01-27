
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
            ProcInstanceClass selectedProcess = new();
            bool wantExit = false;
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
                        Console.WriteLine($"Process - {selectedProcess.process.ProcessName} - UpTime - {DateTime.Now - selectedProcess.process.StartTime}");
                        break;

                    case ConsoleKey.N:
                        Console.WriteLine("Enter name -> ");
                        string name = Console.ReadLine();
                        if (name != null)
                        {
                            Console.WriteLine("nane {0}", name);
                            selectedProcess.process = selectedProcess.getProcByName(name);
                            selectedProcess.ProcName = selectedProcess.process.ProcessName;
                        }

                        break;

                    case ConsoleKey.X:
                        wantExit = true;
                        break;

                    default:
                        Console_Output_Class.DisplayMessage("Please enter correct.");
                        break;
                }

               
            }

            

        }
    }
}