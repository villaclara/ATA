﻿using ATA_ClassLibrary;
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
            ProcsList.ItemsSource = strProcs;
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(ProcsList.SelectedItem.ToString());

            this.Close();
        }
    }
}
