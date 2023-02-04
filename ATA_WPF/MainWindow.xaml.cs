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


        // retrieve processes and show new form and return back and do all the work
        private void setFirstProcessButton_Click(object sender, RoutedEventArgs e)
        {
            



            ListProcsForm lForm = new ListProcsForm();
            lForm.ShowDialog();

            MessageBox.Show(ProcInstanceClass.selectedProc);

            if (ProcInstanceClass.selectedProc != null && ProcInstanceClass.selectedProc != "")
            {
                processArray[0] = new ProcInstanceClass(ProcInstanceClass.selectedProc);
                ProcInstanceClass.selectedProc = "";

                firstProcessName.Text = processArray[0].ProcName;
                firstProcessUpTime.Text = processArray[0].UpTimeMinutesCurrentSession.ToString() + " mins";
                firstProcessTotalUpTime.Text = processArray[0].TotalUpTime.ToString() + " minutes";
            }
            
        }


    }
}
