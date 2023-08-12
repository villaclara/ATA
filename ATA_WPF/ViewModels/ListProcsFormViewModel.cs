using ATA_ClassLibrary.Models;
using ATA_ClassLibrary.Tools;
using ATA_WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ATA_WPF.ViewModels
{
    public class ListProcsFormViewModel : INotifyPropertyChanged
    {

		public event PropertyChangedEventHandler? PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "") => 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


		private string? _selectedItem;
		public string? SelectedItem
		{
			get => _selectedItem; 
			set
			{
				_selectedItem = value;
				SetApplicationToTrackCMD.OnCanExecuteChanged();
			}
		}

		public MyCommand SetApplicationToTrackCMD { get; set; }
		

		private void SetAppToTrack(object parameter)
		{
			ProcInstanceClass.SelectedProc = SelectedItem!.ToString();
			Process[] pr = Process.GetProcessesByName(ProcInstanceClass.SelectedProc);
			MessageBox.Show($"proc - {ProcInstanceClass.SelectedProc}, o - {parameter}");
			
			Window? wnd = parameter as Window;
			wnd?.Close();
		}




		public List<string> ActiveProcsNames { get; set; }
        
		public ListProcsFormViewModel()
		{

			//ActiveProcsNames = DifferentFunctions.GetActiveProcessesByNameString();
			ActiveProcsNames = MakeOnlyUniqueProcessToDisplay();
			ActiveProcsNames.Sort();
			SetApplicationToTrackCMD = new MyCommand(SetAppToTrack, o => SelectedItem != null);
		}

		private List<string> MakeOnlyUniqueProcessToDisplay()
		{
			var allProcs = DifferentFunctions.GetActiveProcessesByNameString().ToList();
			Dictionary<string, int> uniqueProcs = new Dictionary<string, int>();
			foreach(var proc in allProcs)
			{
				if (uniqueProcs.ContainsKey(proc))
				{
					var count = uniqueProcs[proc];
					uniqueProcs[proc] = ++count;
				}
				else
					uniqueProcs[proc] = 1;
			}

			List<string> newlist = new List<string>();
			foreach(var p in uniqueProcs)
			{
				newlist.Add($"{p.Key} - {p.Value}");
			}

			return newlist;
		}

	}

}
