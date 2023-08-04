using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using ATA_ClassLibrary.Models;
using ATA_ClassLibrary.Tools;
using Microsoft.Win32;

namespace ATA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ProcInstanceClass[] processArray = new ProcInstanceClass[5];

        public static string BaseDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        
        public MainWindow()
        {
            InitializeComponent();

            this.Width = 765;
            this.Height = 320;

            createIfNeededStartingFile();

            initializeProcesses();

            DisplayStartingInfo();

            setupTrayIcon();


#if DEBUG

            System.Windows.MessageBox.Show("Debug mode");



            
            

#else
           

            //// adding registry key to launch app on windows startup
            //RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //reg.SetValue("ATA", Process.GetCurrentProcess().MainModule.FileName.ToString());

#endif
        }


        // event for idk
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
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

        // initializes and setups the TRAY ICON
        // 
        private void setupTrayIcon()
        {
            // Icon and delegate for displaying minimized icon in the taskbar
            // delegate == event
            System.Windows.Forms.NotifyIcon nico = new System.Windows.Forms.NotifyIcon();
            //nico.Icon = new System.Drawing.Icon("icon2_super.ico");

            nico.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BaseDir, "icon2_super.ico"));
            nico.Visible = true;
            nico.DoubleClick += delegate (object? sender, EventArgs args)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            };

            ToolStripMenuItem openItem = new ToolStripMenuItem("Open");
            ToolStripMenuItem checkUpdateItem = new ToolStripMenuItem("Check update");
            ToolStripMenuItem closeItem = new ToolStripMenuItem("Exit");
            nico.ContextMenuStrip = new ContextMenuStrip();
            nico.ContextMenuStrip.Items.AddRange(new[] { openItem, checkUpdateItem, closeItem });


            nico.Text = "ATA";

            closeItem.Click += delegate (object? sender, EventArgs args)
            {
                nico.Visible = false;
                this.Close();
            };

            openItem.Click += delegate (object? sender, EventArgs args)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            };

            checkUpdateItem.Click += CheckUpdateItem_Click;

        }


        private void CheckUpdateItem_Click(object? sender, EventArgs e)
        {
            
        }

        // one function for all info
        private void DisplayAllInfo()
        {
            DisplayInfo(processArray[0], 0, pName: firstProcessName, pUpTime: firstProcessUpTime, pTotalUpTime: firstProcessTotalUpTime, isRun: firstProcessIsRun, resetFirstButton, showFirstProcessAllTimes, deleteFirstButton);
            DisplayInfo(processArray[1], 1, pName: secondProcessName, pUpTime: secondProcessUpTime, pTotalUpTime: secondProcessTotalUpTime, isRun: secondProcessIsRun, resetSecondButton, showSecondProcessAllTimes, deleteSecondButton);
            DisplayInfo(processArray[2], 2, pName: thirdProcessName, pUpTime: thirdProcessUpTime, pTotalUpTime: thirdProcessTotalUpTime, isRun: thirdProcessIsRun, resetThirdButton, showThirdProcessAllTimes, deleteThirdButton);
            DisplayInfo(processArray[3], 3, pName: fourthProcessName, pUpTime: fourthProcessUpTime, pTotalUpTime: fourthProcessTotalUpTime, isRun: fourthProcessIsRun, resetFourthButton, showFourthProcessAllTimes, deleteFourthButton);
            DisplayInfo(processArray[4], 4, pName: fifthProcessName, pUpTime: fifthProcessUpTime, pTotalUpTime: fifthProcessTotalUpTime, isRun: fifthProcessIsRun, resetFifthButton, showFifthProcessAllTimes, deleteFifthButton);
        }


        // is called at MainWindow ctor
        // displaying info + starting timer
        private void DisplayStartingInfo()
        {
            DisplayAllInfo();

            var timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }




        // event handler for timer when time elapsed
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {   
            // each time the time has elaped -- the below functions are called
            // writing updated info to files
            // displaying info

            this.Dispatcher.Invoke(new Action(() =>
            { 
                WorkerWithFileClass.writeToFileInfoAboutProcs(processArray);

                // one call for all 5 procs
                DisplayAllInfo();
            }));
        }


        // display info for separate process
        private void DisplayInfoProcess (ProcInstanceClass procInstance, TextBlock pName, TextBlock pUptime, TextBlock pTotalUptime, TextBlock pIsRun)
        {
            pIsRun.Text = procInstance.IsRunning.ToString();
            if (procInstance.IsRunning)
            {
                pIsRun.Foreground = System.Windows.Media.Brushes.Green;
            }
            else pIsRun.Foreground = System.Windows.Media.Brushes.Red;
            pName.Text = procInstance.ProcName;
            pUptime.Text = procInstance.UpTimeMinutesCurrentSession + " minutes";
            pTotalUptime.Text = procInstance.TotalUpTime.ToString() + " mins -- from " + procInstance.UpTimes.First().UpDate.ToString();         
        }


        // checks the process and calls the displayinfoprocess
        public void DisplayInfo (ProcInstanceClass proc, int procIndex, TextBlock pName, TextBlock pUpTime, TextBlock pTotalUpTime, TextBlock isRun, System.Windows.Controls.Button restart, System.Windows.Controls.Button details, System.Windows.Controls.Button delete)
        {
            // get name of searched process to display info
            string name = WorkerWithFileClass.getProcessFromFileWithGivenIndex(procIndex);
            if (name == "empty")
            {

                return;
            }

            // check if process was not asigned when the app is not running but the name was already addedd to file
            // done to update process.StartTime to correct value
            if (proc.process == null && DifferentFunctions.checkIfProcessIsRunningWithStringName(name) == true)
            {
                proc.process = ProcInstanceClass.getProcByName(name);

            }

            proc.setIsPreviousRunning();

            // check if the app has been closed then assigning process to null
            // done to process.StartTime to update next time the process is launched
            if (DifferentFunctions.checkIfProcessIsRunningWithStringName(name) == false)
            {
                proc.process = null;
               
            }

            // enabling / disabling buttons on the rigth side
            setRightButtonsIsEnabled(restart, details, delete, proc);


            // display info
            DisplayInfoProcess(proc,  pName,  pUpTime,  pTotalUpTime, isRun);

        }

       

        // enables or disables right buttons - restart, details and delete
        // checks the procinstance.procName variable
        private void setRightButtonsIsEnabled(System.Windows.Controls.Button restart, System.Windows.Controls.Button details, System.Windows.Controls.Button delete, ProcInstanceClass proc)
        {
            if (proc.ProcName == "")
            {
                restart.IsEnabled = false;
                details.IsEnabled = false;
                delete.IsEnabled = false;
            }
            else
            {
                restart.IsEnabled = true;
                details.IsEnabled = true;
                delete.IsEnabled = true;
            }
        }



        #region PROCESS EVENTS REGION

       

        #endregion END PROCESS EVENTS REGION

        private void showMessagebox (string m, string t, MessageBoxButton mB) => System.Windows.MessageBox.Show(m, t, mB);




        #region SET BUTTONS
        //////////////////////////////////////////////////////////////////////////////////
        //
        // SECTION FOR SET BUTTONS 
        //

        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {

            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[0] = new ProcInstanceClass(ProcInstanceClass.selectedProc);                
                WorkerWithFileClass.AddProcessNameToFile(processArray[0].ProcName, processArray, 0);
                WorkerWithFileClass.writeToFileInfoAboutOneProcFromSetProcButton(processArray[0]);
                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[0], firstProcessName, firstProcessUpTime, firstProcessTotalUpTime, firstProcessIsRun);
                resetFirstButton.IsEnabled = true;
                showFirstProcessAllTimes.IsEnabled = true;
                deleteFirstButton.IsEnabled = true;
            }
        }

        private void setSecondProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[1] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                //processArray[1].process.Exited += SecondProcExited;

                WorkerWithFileClass.AddProcessNameToFile(processArray[1].ProcName, processArray, 1);
                WorkerWithFileClass.writeToFileInfoAboutOneProcFromSetProcButton(processArray[1]);

                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[1], secondProcessName, secondProcessUpTime, secondProcessTotalUpTime, secondProcessIsRun);


                resetSecondButton.IsEnabled = true;
                showSecondProcessAllTimes.IsEnabled = true;
                deleteSecondButton.IsEnabled = true;
            }
        }

        private void setThirdProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[2] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
               // processArray[2].process.Exited += ThirdProcExited;

                WorkerWithFileClass.AddProcessNameToFile(processArray[2].ProcName, processArray, 2);
                WorkerWithFileClass.writeToFileInfoAboutOneProcFromSetProcButton(processArray[2]);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[2], thirdProcessName, thirdProcessUpTime, thirdProcessTotalUpTime, thirdProcessIsRun);


                resetThirdButton.IsEnabled = true;
                showThirdProcessAllTimes.IsEnabled = true;
                deleteThirdButton.IsEnabled = true;
            }
        }

        private void setFourthProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[3] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
               // processArray[3].process.Exited += FourthProcExited;

                WorkerWithFileClass.AddProcessNameToFile(processArray[3].ProcName, processArray, 3);
                WorkerWithFileClass.writeToFileInfoAboutOneProcFromSetProcButton(processArray[3]);


                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[3], fourthProcessName, fourthProcessUpTime, fourthProcessTotalUpTime, fourthProcessIsRun);


                resetFourthButton.IsEnabled = true;
                showFourthProcessAllTimes.IsEnabled = true;
                deleteFourthButton.IsEnabled = true;
            }
        }

        private void setFifthProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();

            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[4] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                //processArray[4].process.Exited += FifthProcExited;

                WorkerWithFileClass.AddProcessNameToFile(processArray[4].ProcName, processArray, 4);

                WorkerWithFileClass.writeToFileInfoAboutOneProcFromSetProcButton(processArray[4]);
                

                ProcInstanceClass.selectedProc = "";
                DisplayInfoProcess(processArray[4], fifthProcessName, fifthProcessUpTime, fifthProcessTotalUpTime, fifthProcessIsRun);


                resetFifthButton.IsEnabled = true;
                showFifthProcessAllTimes.IsEnabled = true;
                deleteFifthButton.IsEnabled = true;
                
            }
        }



        //
        // END OF SET BUTTONS
        //
        //////////////////////////////////////////////////////////////////////////////////

        #endregion SET BUTTONS

        //
        //////////////////////////////////////////////////////////////////////////////////
        //
        // ANOTHER BUTTONS


        // clik on button from the topbar menu
        // Reset All
        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? All will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            ProcInstanceClass procInstanceNull = new();
            for (int i = 0; i < processArray.Length; i++)
            {
                processArray[i] = procInstanceNull;
            }


            File.WriteAllText(DifferentFunctions.fileWithProcesses, "");
            WorkerWithFileClass.createDefault();
            DisplayAllInfo();


        }


        #region RESET BUTTONS REGION

        // click on Reset all and delete button from topbar menu
        private void ResetAndDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? EVERYTHING will be deleted", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            ProcInstanceClass procInstanceNull = new();
            for (int i = 0; i < 5; i++)
            {
                processArray[i] = procInstanceNull;
                WorkerWithFileClass.writeProcessToFileAtIndex(i, "empty");
            }
            DisplayAllInfo();


        }

        private void resetFirstButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            // trying to reset buttons and if could not do then show messsage
            if (!DifferentFunctions.resetTotalUptime(processArray[0]))
            {
                showMessagebox(m: "Application is currently running. Please close the app and try again.", t: "NOT DONE CLOWN", MessageBoxButton.OK);
            }
            
            else
            {
                DisplayInfo(processArray[0], 0, pName: firstProcessName, pUpTime: firstProcessUpTime, pTotalUpTime: firstProcessTotalUpTime, isRun: firstProcessIsRun, resetFirstButton, showFirstProcessAllTimes, deleteFirstButton);
            }

        }

        private void resetSecondButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            // trying to reset buttons and if could not do then show messsage
            if (!DifferentFunctions.resetTotalUptime(processArray[1]))
            {
                showMessagebox(m: "Application is currently running. Please close the app and try again.", t: "NOT DONE CLOWN", MessageBoxButton.OK);
            }

            else
            {
                DisplayInfo(processArray[1], 1, pName: secondProcessName, pUpTime: secondProcessUpTime, pTotalUpTime: secondProcessTotalUpTime, isRun: secondProcessIsRun, resetSecondButton, showSecondProcessAllTimes, deleteSecondButton);
            }

        }

        private void resetThirdButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            // trying to reset buttons and if could not do then show messsage
            if (!DifferentFunctions.resetTotalUptime(processArray[2]))
            {
                showMessagebox(m: "Application is currently running. Please close the app and try again.", t: "NOT DONE CLOWN", MessageBoxButton.OK);
            }

            else
            {
                DisplayInfo(processArray[2], 2, pName: thirdProcessName, pUpTime: thirdProcessUpTime, pTotalUpTime: thirdProcessTotalUpTime, isRun: thirdProcessIsRun, resetThirdButton, showThirdProcessAllTimes, deleteThirdButton);
            }

        }

        private void resetFourthButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }
            // trying to reset buttons and if could not do then show messsage
            if (!DifferentFunctions.resetTotalUptime(processArray[3]))
            {
                showMessagebox(m: "Application is currently running. Please close the app and try again.", t: "NOT DONE CLOWN", MessageBoxButton.OK);
            }

            else
            {
                DisplayInfo(processArray[3], 3, pName: fourthProcessName, pUpTime: fourthProcessUpTime, pTotalUpTime: fourthProcessTotalUpTime, isRun: fourthProcessIsRun, resetFourthButton, showFourthProcessAllTimes, deleteFourthButton);
            }

        }

        private void resetFifthtButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be restarted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            // trying to reset buttons and if could not do then show messsage
            if (!DifferentFunctions.resetTotalUptime(processArray[4]))
            {
                showMessagebox(m: "Application is currently running. Please close the app and try again.", t: "NOT DONE CLOWN", MessageBoxButton.OK);
            }

            else
            {
                DisplayInfo(processArray[4], 4, pName: fifthProcessName, pUpTime: fifthProcessUpTime, pTotalUpTime: fifthProcessTotalUpTime, isRun: fifthProcessIsRun, resetFifthButton, showFifthProcessAllTimes, deleteFifthButton);
            }

        }

        #endregion RESET BUTTONS END REGION


        #region SHOW ALL TIME BUTTONS CLICK


        private bool isClickedFirst = false;
        private bool isClickedSecond = false;
        private bool isClickedThird = false;
        private bool isClickedFourth = false;
        private bool isClickedFifth = false;

        // firstbutton show all time
        // increases window size and then displaying meessage
        // or if clicked again then just sets the normal window
        private void showFirstProcessAllTimes_Click(object sender, RoutedEventArgs e)
        {
            setWidthWindowWithBooleanAndShowDetails(isClickedFirst, 0);
        }

        private void showSecondProcessAllTimes_Click(object sender, RoutedEventArgs e)
        {
            setWidthWindowWithBooleanAndShowDetails(isClickedSecond, 1);
        }

        private void showThirdProcessAllTimes_Click(object sender, RoutedEventArgs e)
        {
            setWidthWindowWithBooleanAndShowDetails(isClickedThird, 2);
        }

        private void showFourthProcessAllTimes_Click(object sender, RoutedEventArgs e)
        {
            setWidthWindowWithBooleanAndShowDetails(isClickedFourth, 3);
        }

        private void showFifthProcessAllTimes_Click(object sender, RoutedEventArgs e)
        {
            setWidthWindowWithBooleanAndShowDetails(isClickedFifth, 4);
        }






        #endregion SHOW ALL TIME BUTTONS END

        private bool deleteFile (string filename)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure? The time will be permanently deleted.", "WTF", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return false;
            }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            return true;
        }

        #region DELETE BUTTONS REGION



        // ALL buttons do the same behavior
        //
        // 1. calls function that shows messagebox with confirmation on delete
        // 2. if the answer is YES then deletes the file and returns true and go to step 4
        // 3. if the answer is NO then return back
        // 4. assigning new empty variable to appropriate member of array of procInstances
        // 5. writing 'empty' word to the fileWithProcesses text file - procs.txt
        // 6. displaying updated info
       
        private void deleteFirstButton_Click(object sender, RoutedEventArgs e)
        {
            if (!deleteFile(processArray[0].fileNameToWriteInfo))
            {
                return;
            }


            ProcInstanceClass procInstanceNull = new();
            processArray[0] = procInstanceNull;
            WorkerWithFileClass.writeProcessToFileAtIndex(0, "empty");
            DisplayInfo(processArray[0],
                        0,
                        firstProcessName,
                        firstProcessUpTime,
                        firstProcessTotalUpTime,
                        firstProcessIsRun,
                        resetFirstButton,
                        showFirstProcessAllTimes,
                        deleteFirstButton);


            this.Width = 765;
            isClickedFirst = false;


        }

        private void deleteSecondButton_Click(object sender, RoutedEventArgs e)
        {

            if (!deleteFile(processArray[1].fileNameToWriteInfo))
            {
                return;
            }

            ProcInstanceClass procInstanceNull = new();
            processArray[1] = procInstanceNull;
            WorkerWithFileClass.writeProcessToFileAtIndex(1, "empty");
            DisplayInfo(processArray[1],
                        1,
                        secondProcessName,
                        secondProcessUpTime,
                        secondProcessTotalUpTime,
                        secondProcessIsRun,
                        resetSecondButton,
                        showSecondProcessAllTimes,
                        deleteSecondButton);

            this.Width = 765;
            isClickedFirst = false;


        }


        private void deleteThirdButton_Click(object sender, RoutedEventArgs e)
        {
            if (!deleteFile(processArray[2].fileNameToWriteInfo))
            {
                return;
            }

            ProcInstanceClass procInstanceNull = new();
            processArray[2] = procInstanceNull;
            WorkerWithFileClass.writeProcessToFileAtIndex(2, "empty");
            DisplayInfo(processArray[2],
                2,
               thirdProcessName,
               thirdProcessUpTime,
               thirdProcessTotalUpTime,
               thirdProcessIsRun,
               resetThirdButton,
               showThirdProcessAllTimes,
               deleteThirdButton);

            this.Width = 765;
            isClickedFirst = false;


        }

        private void deleteFourthButton_Click(object sender, RoutedEventArgs e)
        {
            if (!deleteFile(processArray[3].fileNameToWriteInfo))
            {
                return;
            }

            ProcInstanceClass procInstanceNull = new();
            processArray[3] = procInstanceNull;
            WorkerWithFileClass.writeProcessToFileAtIndex(3, "empty");
            DisplayInfo(processArray[3],
                3,
                fourthProcessName,
                fourthProcessUpTime,
                fourthProcessTotalUpTime,
                fourthProcessIsRun,
                resetFourthButton,
                showFourthProcessAllTimes,
                deleteFourthButton);

            this.Width = 765;
            isClickedFirst = false;


        }

        private void deleteFifthButton_Click(object sender, RoutedEventArgs e)
        {

            if (!deleteFile(processArray[4].fileNameToWriteInfo))
            {
                return;
            }

            ProcInstanceClass procInstanceNull = new();
            processArray[4] = procInstanceNull;
            WorkerWithFileClass.writeProcessToFileAtIndex(4, "empty");
            DisplayInfo(processArray[4],
                4,
                fifthProcessName,
                fifthProcessUpTime,
                fifthProcessTotalUpTime,
                fifthProcessIsRun,
                resetFifthButton,
                showFifthProcessAllTimes,
                deleteFifthButton);
            this.Width = 765;
            isClickedFirst = false;


        }

        #endregion DELETE BUTTONS END REGION

        // when choosing the Info in the TopBarMenuStrip
        // shows new window InfoWindow with its content
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow infoWindow = new InfoWindow();
            infoWindow.Show();
        }

        // is used to set and reset window width when asking to show details
        private void setWidthWindowWithBooleanAndShowDetails (bool isClicked, int index)
        {
            if (!isClickedFirst)
            {
                this.Width = 985;
                isClickedFirst = true;

                showDetailsWithGivenProcessIndex(index);
            }

            else
            {
                this.Width = 765;
                isClickedFirst = false;
            }
        }

        // show detailed time about process with given index
        private void showDetailsWithGivenProcessIndex(int index)
        {
            IEnumerable<UpTime> upTimes = processArray[index].retrieveListOfUpTimesForCurrentProcess(processArray[index].fileNameToWriteInfo);

            detailsDataGrid.DataContext = upTimes;

        }

       
    }
}
