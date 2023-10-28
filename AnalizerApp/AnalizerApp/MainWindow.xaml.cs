using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalizerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        //public ObservableCollection<string> FilesList { get; set; }

        private Timer processUpdateTimer;
        private DispatcherTimer progressTimer;

        private string symbolsCount;
        public string SymbolsCount
        {
            get => symbolsCount;
            set => this.PropertyChangeMethod(out symbolsCount, value);

        }

        private string wordsCount;
        public string WordsCount
        {
            get => wordsCount;
            set => this.PropertyChangeMethod(out wordsCount, value);

        }

        private string sentencesCount;
        public string SentencesCount
        {
            get => sentencesCount;
            set => this.PropertyChangeMethod(out sentencesCount, value);

        }

        private double progressBarValue;
        public double ProgressBarValue
        {
            get => progressBarValue;
            set => this.PropertyChangeMethod(out progressBarValue, value);

        }

        //public double ProgressBarValue { get; set; }
        public MainWindow()
        {
            
            InitializeComponent();
            this.DataContext = this;
            
            //FilesList = new ObservableCollection<ProcessInfo>();
            this.filesListView.Items.Add("test.json");
            this.filesListView.Items.Add("bigfile.txt");
            this.filesListView.Items.Add("one.bin");
            this.filesListView.Items.Add("smallfile.txt");


            progressTimer = new DispatcherTimer();
            progressTimer.Interval = TimeSpan.FromMilliseconds(100); // Update every 100 milliseconds
            progressTimer.Tick += ProgressTimer_Tick;

        }


        public Thread TaskBarAsync(CancellationToken? token = null)
        {
            var thread = new Thread(() =>
            {
                Predicate<CancellationToken?> IsTokenCanceled = (tkn) => tkn != null && tkn.Value.IsCancellationRequested;

                if (IsTokenCanceled(token))
                    return;
                for (int i = 0; i <= 10; i++)
                {
                    this.ProgressBarValue = i * 10;
                    Thread.Sleep(100);
                }

                // Now that the task is done, stop the progress timer
                progressTimer.Stop();

                //Thread.Sleep(1000);


            });

            thread.Start();

            return thread;
        }


        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            // The progress timer tick handler can be empty, as the progress is updated within TaskBarAsync
        }
        private void PropertyChangeMethod<T>(out T field, T value, [CallerMemberName] string propName = "")
        {
            field = value;

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ProgressBarValue = 0;
            switch (this.filesListView.SelectedIndex)
            {
                case 0:
                {

                    var read = File.ReadAllText("Assets/test.json");
                    ProgressBarValue = 0;
                    progressTimer.Start();
                    var thread = new Thread(() =>
                    {
                        Predicate<CancellationToken?> IsTokenCanceled = (tkn) => tkn != null && tkn.Value.IsCancellationRequested;

                        //if (IsTokenCanceled(token))
                        //    return;
                        for (int i = 0; i <= 10; i++)
                        {
                            this.ProgressBarValue = i * 10;
                            Thread.Sleep(100);
                        }

                    });

                    thread.Start();
                    //thread.Join();
                    this.SymbolsCount = $"symbols count: {read.Length}";
                    this.WordsCount = $"Words count: {CountWords(read)}";
                    this.SentencesCount = $"Sentences count: {CountSentences(read)}";

                    progressTimer.Stop();
                        return;
                }
                case 1:
                {

                    var read = File.ReadAllText("Assets/bigfile.txt");
                    ProgressBarValue = 0;
                    progressTimer.Start();
                    var thread = new Thread(() =>
                    {
                        Predicate<CancellationToken?> IsTokenCanceled = (tkn) => tkn != null && tkn.Value.IsCancellationRequested;

                        //if (IsTokenCanceled(token))
                        //    return;
                        for (int i = 0; i <= 10; i++)
                        {
                            this.ProgressBarValue = i * 10;
                            Thread.Sleep(300);
                        }

                    });

                    thread.Start();
                    thread.Join();
                       
                    this.SymbolsCount = $"symbols count: {read.Length}";
                    this.WordsCount = $"Words count: {CountWords(read)}";
                    this.SentencesCount = $"Sentences count: {CountSentences(read)}";
                    progressTimer.Stop();
                    return;
                }
                    return;
                case 2:
                {

                    var read = File.ReadAllText("Assets/one.bin");
                    ProgressBarValue = 0;
                    progressTimer.Start();
                    var thread = new Thread(() =>
                    {
                        Predicate<CancellationToken?> IsTokenCanceled = (tkn) => tkn != null && tkn.Value.IsCancellationRequested;

                        //if (IsTokenCanceled(token))
                        //    return;
                        for (int i = 0; i <= 10; i++)
                        {
                            this.ProgressBarValue = i * 10;
                            Thread.Sleep(10);
                        }

                    });

                    thread.Start();
                    thread.Join();

                    this.SymbolsCount = $"symbols count: {read.Length}";
                    this.WordsCount = $"Words count: {CountWords(read)}";
                    this.SentencesCount = $"Sentences count: {CountSentences(read)}";
                    progressTimer.Stop();
                    return;
                }
                    return;
                case 3:
                {

                    var read = File.ReadAllText("Assets/smallfile.txt");
                    ProgressBarValue = 0;
                    progressTimer.Start();
                    var thread = new Thread(() =>
                    {
                        Predicate<CancellationToken?> IsTokenCanceled = (tkn) => tkn != null && tkn.Value.IsCancellationRequested;

                        //if (IsTokenCanceled(token))
                        //    return;
                        for (int i = 0; i <= 10; i++)
                        {
                            this.ProgressBarValue = i * 10;
                            Thread.Sleep(50);
                        }

                    });

                    thread.Start();
                    thread.Join();

                    this.SymbolsCount = $"symbols count: {read.Length}";
                    this.WordsCount = $"Words count: {CountWords(read)}";
                    this.SentencesCount = $"Sentences count: {CountSentences(read)}";
                    progressTimer.Stop();
                    return;
                }
                    return;
                default:
                    return;
            }

        }
        private int CountWords(string text)
        {

            string[] words = text.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        private int CountSentences(string text)
        {
            
            string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            return sentences.Length;
        }

        
    }
}
