using System;
using System.Diagnostics;
using System.Windows;
using Ingescape;
using System.IO;

namespace WPFIngescape
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        igs_observeCallback callbackLabelString;

        public MainWindow()
        {
            InitializeComponent();
            InitIngescape();
        }

        void CallbackString(iop_t iopType, string name, iopType_t valueType, IntPtr value, int valueSize, IntPtr myData)
        {
            string txt = Igs.readInputAsString(name.ToString());

            Debug.WriteLine("Texte : ", txt);

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.labelString.Content = txt;
            });
        }

        private void InitIngescape()
        {
            Igs.setAgentName("WpfAgent");

            Igs.createInput("stringEntry", iopType_t.IGS_STRING_T, IntPtr.Zero, 0);

            callbackLabelString = CallbackString;

            Igs.observeInput("stringEntry", callbackLabelString, IntPtr.Zero);

            string json = File.ReadAllText("mapping.json");
            Igs.loadMapping(json);
            Debug.WriteLine(Igs.getMappingName());

            Igs.startWithDevice("Wi-Fi", 5670);
        }
        
    }
}
