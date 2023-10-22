using System.Threading;

namespace TaskManager;


using System;
using System.Diagnostics;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>




public class ProcessInfo
{
    public string Name { get; set; }
    public float CpuUsage { get; set; }
    
    public long MemoryUsage { get; set; }
    public double MemoryUsageInMB
    {
        get { return MemoryUsage / (1024.0 * 1024.0); }
    }

     
}



public partial class MainWindow : Window, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private Timer processUpdateTimer;
    public ObservableCollection<ProcessInfo> ProcessesList { get; set; }
    private string percent;
    public string Percent {
        get => percent;
        set => this.PropertyChangeMethod(out percent, value);

    }

    private void PropertyChangeMethod<T>(out T field, T value, [CallerMemberName] string propName = "")
    {
        field = value;

        if (this.PropertyChanged != null)
        {
            this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
    public MainWindow()
    {
        InitializeComponent();

        

        this.DataContext = this;
        ProcessesList = new ObservableCollection<ProcessInfo>();
       
        processUpdateTimer = new Timer((obj) =>
        {
            long perc = 0;
            int i = 0;
            double totalMemoryGB = 8;
            Dispatcher.Invoke(() =>
            {
                ProcessesList.Clear();

                foreach (Process process in Process.GetProcesses())
                {
                    ProcessesList.Add(new ProcessInfo
                    {
                        Name = process.ProcessName,
                        CpuUsage = 0,
                        MemoryUsage = process.WorkingSet64
                    });
                    perc += process.WorkingSet64;
                    i++;
                }
                perc /= 1073741824;
                this.Percent = Convert.ToString($@"{(int)(perc * 100 / 8)} %
Memory");
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Percent));
                
            });
        }, null, 0, 6000);
       

    }


    private void EndTask_Click(object sender, RoutedEventArgs e)
    {
        if (processListView.SelectedItem is ProcessInfo selectedProcess)
        {
            try
            {
                Process process = Process.GetProcesses().FirstOrDefault(p => p.ProcessName == selectedProcess.Name);
                if (process != null)
                {
                    process.Kill(); 
                    ProcessesList.Remove(selectedProcess);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ending the task: {ex.Message}");
            }
        }
        else
        {
            MessageBox.Show("Select a process to end.");
        }
    }



}