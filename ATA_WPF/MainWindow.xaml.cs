using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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


            DisplayInfo(processArray[0], pName: firstProcessName, pUpTime: firstProcessUpTime, pTotalUpTime: firstProcessTotalUpTime);
            //WorkerWithFileClass.AddProcessNameToFile(DifferentFunctions.fileWithProcesses, processArray);
           
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


        // retrieve processes and show new form and return back and do all the work
        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {

            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();

            
            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[0] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                
                WorkerWithFileClass.AddProcessNameToFile(processArray[0].ProcName, processArray, 0);
                
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(60000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

            }
            
        }


        // event handler for timer when time elapsed
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {

                if (processArray[0] != null)
                {

                    DisplayInfoProcess(processArray[0], firstProcessName, firstProcessUpTime, firstProcessTotalUpTime);
                }

                if (processArray[1] != null)
                {

                    DisplayInfoProcess(processArray[1], secondProcessName, secondProcessUpTime, secondProcessTotalUpTime);
                }
                if (processArray[2] != null)
                {

                    DisplayInfoProcess(processArray[2], thirdProcessName, thirdProcessUpTime, thirdProcessTotalUpTime);
                }
                if (processArray[3] != null)
                {

                    DisplayInfoProcess(processArray[3], fourthProcessName, fourthProcessUpTime, fourthProcessTotalUpTime);
                }
                if (processArray[4] != null)
                {

                    DisplayInfoProcess(processArray[4], fifthProcessName, fifthProcessUpTime, fifthProcessTotalUpTime);
                }
            }));
        }


        private void DisplayInfoProcess (ProcInstanceClass procInstance, TextBlock pName, TextBlock pUptime, TextBlock pTotalUptime)
        {
            

            pName.Text = procInstance.ProcName;
            pUptime.Text = procInstance.UpTimeMinutesCurrentSession + " mins";
            pTotalUptime.Text = procInstance.TotalUpTime.ToString() + " minutes";

            
        }

        public void DisplayInfo (ProcInstanceClass proc, TextBlock pName, TextBlock pUpTime, TextBlock pTotalUpTime)
        {
            string name = WorkerWithFileClass.getProcessFromFileWithGivenIndex(0);
            if (name == "empty")
            {
                return;
            }

            if (DifferentFunctions.checkIfProcessIsRunningWithStringName(name))
            {
                proc = new ProcInstanceClass(name, DifferentFunctions.checkIfProcessIsRunningWithStringName(name));
            }

            DisplayInfoProcess(proc,  pName,  pUpTime,  pTotalUpTime);


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

                var timer = new System.Timers.Timer(58000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

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

                var timer = new System.Timers.Timer(61000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

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

                var timer = new System.Timers.Timer(62000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

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

                var timer = new System.Timers.Timer(59000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

            }
        }
    }
}
