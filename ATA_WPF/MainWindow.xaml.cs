﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using ATA_ClassLibrary;

namespace ATA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // initializing second form

        ProcInstanceClass[] processArray = new ProcInstanceClass[5];

        public MainWindow()
        {
            InitializeComponent();

            createIfNeededStartingFile();

            initializeProcesses();

            DisplayStartingInfo();
            //WorkerWithFileClass.AddProcessNameToFile(DifferentFunctions.fileWithProcesses, processArray);

            
            
        }

        // initializes array of processes
        private void initializeProcesses()
        {
            
            for (int i = 0; i < 5; i++)
            {

                string name = WorkerWithFileClass.getProcessFromFileWithGivenIndex(i);
                if (name == "empty")
                {
                    return;
                }
                if (!DifferentFunctions.checkIfProcessIsRunningWithStringName(name))
                {
                    processArray[i] = new ProcInstanceClass(name, DifferentFunctions.checkIfProcessIsRunningWithStringName(name));
                }
                else { processArray[i] = new ProcInstanceClass(name); }
            }
            
        }
    


    // checks if the startring file - "processes.txt" is created and if not created then creates and fills it with 'empty' names
    // made separate function for the ctor to be clear
        private void createIfNeededStartingFile()
        {
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
        }


        // one function for all info
        private void DisplayAllInfo()
        {
            DisplayInfo(processArray[0], 0, pName: firstProcessName, pUpTime: firstProcessUpTime, pTotalUpTime: firstProcessTotalUpTime, isRun: firstProcessIsRun);
            DisplayInfo(processArray[1], 1, pName: secondProcessName, pUpTime: secondProcessUpTime, pTotalUpTime: secondProcessTotalUpTime, isRun: secondProcessIsRun);
            DisplayInfo(processArray[2], 2, pName: thirdProcessName, pUpTime: thirdProcessUpTime, pTotalUpTime: thirdProcessTotalUpTime, isRun: thirdProcessIsRun);
            DisplayInfo(processArray[3], 3, pName: fourthProcessName, pUpTime: fourthProcessUpTime, pTotalUpTime: fourthProcessTotalUpTime, isRun: fourthProcessIsRun);
            DisplayInfo(processArray[4], 4, pName: fifthProcessName, pUpTime: fifthProcessUpTime, pTotalUpTime: fifthProcessTotalUpTime, isRun: fifthProcessIsRun);
        }


        // is called at MainWindow ctor
        // displaying info + starting timer
        private void DisplayStartingInfo()
        {
            DisplayAllInfo();

            var timer = new System.Timers.Timer(10000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }




        // event handler for timer when time elapsed
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            

            this.Dispatcher.Invoke(new Action(() =>
            {
                // one call for all 5 procs
                DisplayAllInfo();

                // WorkerWithFileClass.writeToFileWithGivenName(proc.fileNameToWriteInfo, proc, proc.UpTimes);
                WorkerWithFileClass.writeToFileInfoAboutProcs(ref processArray);
            }));
        }


        // display info for separate process
        private void DisplayInfoProcess (ProcInstanceClass procInstance, TextBlock pName, TextBlock pUptime, TextBlock pTotalUptime, TextBlock pIsRun)
        {

            pIsRun.Text = procInstance.IsRunning.ToString();
            pName.Text = procInstance.ProcName;
            pUptime.Text = procInstance.UpTimeMinutesCurrentSession + " mins";
            pTotalUptime.Text = procInstance.TotalUpTime.ToString() + " minutes";

            
        }


        // checks the process and calls the displayinfoprocess
        public void DisplayInfo (ProcInstanceClass proc, int procIndex, TextBlock pName, TextBlock pUpTime, TextBlock pTotalUpTime, TextBlock isRun)
        {
            string name = WorkerWithFileClass.getProcessFromFileWithGivenIndex(procIndex);
            if (name == "empty")
            {
                return;
            }

            if (proc.process == null && DifferentFunctions.checkIfProcessIsRunningWithStringName(name) == true)
            {
                //proc = new ProcInstanceClass(name);

                proc.process = ProcInstanceClass.getProcByName(name);
            }

            //if (!DifferentFunctions.checkIfProcessIsRunningWithStringName(name))
            //{
            //    proc = new ProcInstanceClass(name, DifferentFunctions.checkIfProcessIsRunningWithStringName(name));
            //}
            //else { proc = new ProcInstanceClass(name); }

            //WorkerWithFileClass.writeToFileWithGivenName(proc.fileNameToWriteInfo, proc, proc.UpTimes);

            DisplayInfoProcess(proc,  pName,  pUpTime,  pTotalUpTime, isRun);

            MessageBox.Show(proc.UpTimes.Count.ToString());
        }



        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {

            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[0] = new ProcInstanceClass(ProcInstanceClass.selectedProc);

                WorkerWithFileClass.AddProcessNameToFile(processArray[0].ProcName, processArray, 0);

                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[0], firstProcessName, firstProcessUpTime, firstProcessTotalUpTime, firstProcessIsRun);

            }

        }

        private void setSecondProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[1] = new ProcInstanceClass(ProcInstanceClass.selectedProc);

                WorkerWithFileClass.AddProcessNameToFile(processArray[1].ProcName, processArray, 1);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[1], secondProcessName, secondProcessUpTime, secondProcessTotalUpTime, secondProcessIsRun);


            }
        }

        private void setThirdProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[2] = new ProcInstanceClass(ProcInstanceClass.selectedProc);

                WorkerWithFileClass.AddProcessNameToFile(processArray[2].ProcName, processArray, 2);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[2], thirdProcessName, thirdProcessUpTime, thirdProcessTotalUpTime, thirdProcessIsRun);



            }
        }

        private void setFourthProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[3] = new ProcInstanceClass(ProcInstanceClass.selectedProc);

                WorkerWithFileClass.AddProcessNameToFile(processArray[3].ProcName, processArray, 3);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[3], fourthProcessName, fourthProcessUpTime, fourthProcessTotalUpTime, fourthProcessIsRun);



            }
        }

        private void setFifthProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();

            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[4] = new ProcInstanceClass(ProcInstanceClass.selectedProc);

                WorkerWithFileClass.AddProcessNameToFile(processArray[4].ProcName, processArray, 4);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[4], fifthProcessName, fifthProcessUpTime, fifthProcessTotalUpTime, fifthProcessIsRun);



            }
        }



    }
}
