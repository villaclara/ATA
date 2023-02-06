using ATA_ClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ListProcsForm.xaml
    /// </summary>
    public partial class ListProcsForm : Window   
    {
        public ListProcsForm()
        {
            InitializeComponent();

            List<string> strProcs = DifferentFunctions.GetActiveProcessesByNameString();
            strProcs.Sort();
            ProcsList.ItemsSource = strProcs;
        }


        // sets the SELECTEDPROCESS static variable as selected and the MainWindow reads it
        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            // checks if the item was selected
            if (ProcsList.SelectedItem != null)
            {

                // try catch is needed for the check if the process is system 
                // if the process is system - the time is not available
                try
                {
                    ProcInstanceClass.selectedProc = ProcsList.SelectedItem.ToString();
                    Process[] pr = Process.GetProcessesByName(ProcInstanceClass.selectedProc);
                    var a = pr[0].MainModule.FileName;                      // if this gives an error then we catch and refresh SELECTEDPROCESS var
                    this.Close();
                }
                catch (Exception)
                {
                    ProcInstanceClass.selectedProc = "";
                    MessageBox.Show("The process could not be selected. Please select another.");
                }
            }
        }
    }
}
