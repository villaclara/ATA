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

namespace ATA_WPF
{
    /// <summary>
    /// Interaction logic for TryStartupWindow.xaml
    /// </summary>
    public partial class TryStartupWindow : Window
    {
        private int intForCheckingTextBlock;
        private readonly System.Timers.Timer timer = new System.Timers.Timer(1000);
        private readonly UpdateChecker updater = new UpdateChecker();


        public TryStartupWindow()
        {
            InitializeComponent();

            WorkerWithFileClass.writeVersionFile("0.1.3", "0.1.3");

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            intForCheckingTextBlock = 0;
            // TextBlock1 IsEnabled is False by default
            // chaning to trigger and UPD() to start updating
            TextBlock1.IsEnabledChanged += TextBlock1_IsEnabledChanged;
            TextBlock1.IsEnabled = true;

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


            // after the all files are downloaded and versions are initializes
            // gets the result of Comparing current and Latest versions
            // if TRUE - we need to update
            if (updater.CompareVersion())
            {
                ChekingTextBlock.Text = $"Update is available. Downloading now";

                // waiting for updater to move files from location -> backup
                // and update -> location
                await updater.PerformMovingFiles();

                ChekingTextBlock.Text = "Update Completed. The app will be restarted now.";

                // casually waiting 1 sec to display the window
                await Task.Delay(1000);


                // now the app should be restarted
                // BUT IT DOES NOT WORK
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }

            // if the result of comparing is FALSE - we do not need update
            // so just will show main window
            else
            {
                ChekingTextBlock.Text = $"The App is up to date.";

                //casually waiting 1 sec
                await Task.Delay(1000);

                ShowMainWindow();
            }
        }


        // update function that creates the new UpdateChecker object and starts doing things
        private async Task Upd()
        {
            try
            {
                await updater.StartCheckingUpdate();
                


                await Task.Delay(1000);

            }
            catch (Exception ex)
            {
                timer.Stop();
                ChekingTextBlock.Text = "Error when updating. Try again later.";
            }
            

            //if (updater.Restart)
            //{
            //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            //    Application.Current.Shutdown();
            //}
        }

        private void ShowMainWindow()
        {
            Task.Delay(1000);
            MainWindow main = new MainWindow();
            this.Close();
            Task.Delay(500);
            main.Show();
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
                    ChekingTextBlock.Text = TextToDisplay(intForCheckingTextBlock);
                    intForCheckingTextBlock++;
                });
        }

        // switch on i % 2 and return text to display in ChekingTextBlock
        // used pattern matching in it
        private string TextToDisplay (int i)
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
