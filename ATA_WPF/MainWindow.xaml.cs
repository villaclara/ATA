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

        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {
            List<Process> procs = DifferentFunctions.GetActiveProcesses();

            ListProcsForm lForm = new ListProcsForm();

            lForm.ShowDialog();
        }
    }
}
