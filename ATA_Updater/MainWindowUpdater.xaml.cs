using ATA_ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace ATA_Updater
{
    /// <summary>
    /// Interaction logic for TryStartupWindow.xaml
    /// </summary>
    public partial class MainWindowUpdater : Window
    {
        private int intForCheckingTextBlock;
        private readonly System.Timers.Timer timer = new(1000);
        private readonly UpdateChecker updater;

        private bool _error403Occured = false;

        


        public MainWindowUpdater()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            
            updater = new UpdateChecker();

            
            // WRITES THE CURRENT VERSION INTO VERSION FILE 
            WorkerWithFileClass.writeVersionFile("0.1.3", "0.1.3");



  

            intForCheckingTextBlock = 0;
            // TextBlock1 IsEnabled is False by default
            // chaning to trigger and UPD() to start updating
            TextBlock2.IsEnabledChanged += TextBlock1_IsEnabledChanged;
            TextBlock2.IsEnabled = true;

        }


        // function to start updating
        // is called when event 'TextBlock1.IsEnabledChanged' is raised at constructor
        // needed to be here to AWAIT the UPD function
        private async void TextBlock1_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // starting animating textblock
            AnimateCheckingTextBlock();

            // starting update function
            await Upd();

            // casually waiting 5 sec
            await Task.Delay(5000);


            // stopping the timer to stop updating the text "checking for update .."
            timer.Stop();



            // checking whether 403 error when downloading from google drive happenned
            // and if true -> then just show program and close this window
            if (_error403Occured)
            {
                updater.DoActionsWhenNoUpdateNeeded();
                ShowMainWindow();

                return;
            }


            // after the all files are downloaded and versions are initializes
            // gets the result of Comparing current and Latest versions
            // if TRUE - we need to update
            if (updater.CompareVersion())
            {
                CheckingTextBlock.Text = $"Update is available. Downloading now";

                // waiting for updater to move files from location -> backup
                // and update -> location
                await updater.PerformMovingFiles();

                CheckingTextBlock.Text = "Update Completed.";

                // casually waiting 1 sec to display the window
                await Task.Delay(2000);


                // now the app should be restarted
                // BUT IT DOES NOT WORK
                //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                //Application.Current.Shutdown();

                //Application.Current.Shutdown();
                //System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
            }



            // if the result of comparing is FALSE - we do not need update
            // so just will show main window
            // and delete files from update folder
            else
            {
                CheckingTextBlock.Text = $"The App is up to date.";
                updater.DoActionsWhenNoUpdateNeeded();
                //casually waiting 2 sec
                await Task.Delay(2000);


            }

            ShowMainWindow();

        }


        // update function that creates the new UpdateChecker object and starts doing things
        // starting function
        private async Task Upd()
        {
            try
            {
                LoggerService.Log($"Started updating from MainWindowUpdater.xaml.");

                // starting updating
                // deleting old files
                // downloading new
                await updater.StartCheckingUpdate();

                await Task.Delay(5000);

            }

            // sometimes google may throw 403 exception
            // when downloading files
            // so it is covered by this exception
            catch (Exception e)
            {                
                timer.Stop();
                CheckingTextBlock.Text = "Error when updating. Try again later.";
                _error403Occured = true;
            }
        }

        // shows main window
        // closes current
        private void ShowMainWindow()
        {
            //MainWindow main = new();
            //this.Close();
            //Task.Delay(500);
            //main.Show();

            System.Diagnostics.Process.Start("ATA_WPF.exe");
            Application.Current.Shutdown();


        }

        // timer to count seconds when animating the checkingtextblock
        private void AnimateCheckingTextBlock()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
        }

        // function when timer elapsed
        // updates the CheckingTextBlock textblock
        // and increases the integer by 1
        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                CheckingTextBlock.Text = TextToDisplay(intForCheckingTextBlock);
                intForCheckingTextBlock++;
            });
        }

        // switch on i % 2 and return text to display in ChekingTextBlock
        // used pattern matching in it
        private string TextToDisplay(int i)
        {
            var res = i % 2;
            return res switch
            {
                0 => "Checking for update..",
                1 => "Checking for update...",
                _ => "Checking for update."
            };
        }


        private void MoveExeFile()
        {
            string backupLoc = @"C:\fls\projects\ATA\ATA_WPF\bin\Debug\net6.0-windows\backup";
            string current = @"C:\fls\projects\ATA\ATA_WPF\bin\Debug\net6.0-windows\ATA_WPF.exe";
            File.Move(current, backupLoc + "\\ATA_WPF.exe");

            string update = @"C:\fls\projects\ATA\ATA_WPF\bin\Debug\net6.0-windows\update\ATA_WPF.exe";
            File.Move(update, current);
        }


        // allowing to move the window is the mouse is pressed anywhere at the window
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
