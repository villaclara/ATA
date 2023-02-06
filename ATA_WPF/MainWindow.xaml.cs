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
            
            
           
           
        }




        // retrieve processes and show new form and return back and do all the work
        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {

            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();

            
            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[0] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(2000);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

            }
            
        }

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

        

        private void setSecondProcess_Click(object sender, RoutedEventArgs e)
        {
            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();


            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[1] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(2005);
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
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(2010);
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
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(2015);
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
                ProcInstanceClass.selectedProc = "";

                var timer = new System.Timers.Timer(2020);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();

            }
        }
    }
}
