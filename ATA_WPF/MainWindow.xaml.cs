using System;
using System.Collections.Generic;
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
        ProcInstanceClass proces1 = new ProcInstanceClass("steam");

        public MainWindow()
        {
            InitializeComponent();

            firstProcessName.Text = proces1.ProcName;
            firstProcessUpTime.Text = "UpTime - " + proces1.UpTimeMinutesCurrentSession.ToString();
            firstProcessTotalUpTime.Text = "Total UpTime - " + proces1.TotalUpTime.ToString();
        }


    }
}
