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
            


            intForCheckingTextBlock = 0;
            // TextBlock1 IsEnabled is False by default
            // chaning to trigger and UPD() to start updating
            TextBlock2.IsEnabledChanged += TextBlock1_IsEnabledChanged;
            TextBlock2.IsEnabled = true;


#if DEBUG

#else
            // adding registry key to launch app on windows startup
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("ATA", Process.GetCurrentProcess().MainModule.FileName.ToString());

#endif
        }


        // function to start updating
        // is called when event 'TextBlock1.IsEnabledChanged' is raised at constructor
        // needed to be here to AWAIT the UPD function because ctor can not be called ASYNC
        private async void TextBlock1_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // starting animating textblock
            AnimateCheckingTextBlock();

            // starting update function
            await Upd();

            // casually waiting 5 sec
            await Task.Delay(2000);

            // stopping the timer to stop updating the text "checking for update .."
            timer.Stop();


            // checking whether 403 error when downloading from google drive happenned
            // and if true -> then just show program and close this window
            if (_error403Occured)
            {
                LoggerService.Log("Error 403 occurred and this logged from TextBlock1_IsEnabledChanged.");
                updater.DoActionsWhenNoUpdateNeeded();
                ShowMainWindow();

                return;
            }



            // after the all files are downloaded and versions are initializes
            // gets the result of Comparing current and Latest versions
            // if TRUE - we need to update
            if (updater.CompareVersion())
            {
                CheckingTextBlock.Text = $"Update is available. The files will be updated automatically.";
               
                //logging
                LoggerService.Log($"Update available. Current version is lower than newest.");
                
                // waiting for updater to move files from location -> backup
                // and update -> location
                await updater.PerformMovingFiles();

                CheckingTextBlock.Text = "Update Completed.";
               
                //logging
                LoggerService.Log("Update completed.");
                // casually waiting 1 sec to display the window
                await Task.Delay(2000);

            }


            // if the result of comparing is FALSE - we do not need update
            // so just will show main window
            // and delete files from update folder
            else
            {
                CheckingTextBlock.Text = $"The App is up to date.";
                LoggerService.Log("Update unavailable. Current version is higher or equals newest.");
                updater.DoActionsWhenNoUpdateNeeded();
                //casually waiting 2 sec
                await Task.Delay(1000);

            }

            // starting ATA_WPF.exe app
            ShowMainWindow();

        }


        // update function that creates the new UpdateChecker object and starts doing things
        // starting function
        private async Task Upd()
        {
            LoggerService.Log($"Started updating from MainWindowUpdater.xaml.");

            try
            {

                // starting updating
                // deleting old files
                // downloading new
                await updater.StartCheckingUpdate();

                await Task.Delay(2000);

            }

            // sometimes google may throw 403 exception
            // when downloading files
            // so it is covered by this exception
            catch (Exception)
            {                
                timer.Stop();
                CheckingTextBlock.Text = "Error when updating. Try again later.";
                _error403Occured = true;
            }
        }

        // shows main window
        // closes current
        private async Task ShowMainWindow()
        {
            // logging
            LoggerService.Log($"Starting App ATA_WPF.exe.");
            CheckingTextBlock.Text = "Starting App.";
            await Task.Delay(1000);

            try
            {

                System.Diagnostics.Process.Start("ATA_WPF.exe");
  
            }
            catch (Exception ex)
            {
                CheckingTextBlock.Text = "Error when starting. Starting App from backup.";
                await Task.Delay(1000);
                LoggerService.Log($"Error when starting ATA_WPF.exe - {ex.Message} \n Restoring ATA_WPF from backup.");
                updater.RestoreFromBackup();

                LoggerService.Log($"Startint ATA_WPF.exe");
                System.Diagnostics.Process.Start("ATA_WPF.exe");
            }

            finally
            {
                // logging
                LoggerService.Log($"Closing current ATA_Updater.exe app.");
                Application.Current.Shutdown();
            }

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
        private static string TextToDisplay(int i)
        {
            var res = i % 2;
            return res switch
            {
                0 => "Checking for update..",
                1 => "Checking for update...",
                _ => "Checking for update."
            };
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
