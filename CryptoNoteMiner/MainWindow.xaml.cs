using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace CryptoNoteMiner
{
    public partial class MainWindow
    {
        internal string BasePath { get; private set; }

        internal const string RelativePathWalletData = "wallet";
        internal const string RelativePathWalletAddress = "wallet.address.txt";

        internal string RelativePathResources { get; private set; }
        internal const string RelativePathResourceSimpleminer = "simpleminer.exe";
        internal const string RelativePathResourceSimplewallet = "simplewallet.exe";

        internal string WalletAddress { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            RelativePathResources = Environment.Is64BitOperatingSystem ?
                                    @"Resources\64-bit\" :
                                    @"Resources\32-bit\";


            var currentFile = BasePath + RelativePathResources + RelativePathResourceSimpleminer;
            if (!File.Exists(currentFile)) MessageManager.ShowError("Missing file: " + currentFile);

            currentFile = BasePath + RelativePathResources + RelativePathResourceSimplewallet;
            if (!File.Exists(currentFile)) MessageManager.ShowError("Missing file: " + currentFile);

            currentFile = BasePath + RelativePathResources + RelativePathWalletAddress;
            if (File.Exists(currentFile)) {
                ReadWalletAddress();
            } else {
                MessageManager.ShowInformation("Generating new wallet with password: x");
                GenerateWallet();
            }

            var coresAvailable = Environment.ProcessorCount;
            for (var i = coresAvailable - 1; i >= 0; i--) {
                ComboBoxCpuCores.Items.Add(i + 1);
            }

            //var coresConfig = INI.Value("cores");
            //int coresInt = comboBoxCores.Items.Count - 1;
            //if (coresConfig != "")
            //{
            //    int coresParsed;
            //    var parsed = int.TryParse(coresConfig, out coresParsed);
            //    if (parsed) coresInt = coresParsed - 1;
            //    if (coresInt + 1 > coresAvailable) coresInt = coresAvailable - 1;

            //}
            //comboBoxCores.SelectedIndex = coresInt;

            //var poolHost = INI.Value("pool_host");
            //if (poolHost != "")
            //{
            //    textBoxPoolHost.Text = poolHost;
            //}
            //var poolPort = INI.Value("pool_port");
            //if (poolPort != "")
            //{
            //    textBoxPoolPort.Text = poolPort;
            //}

            //Application.ApplicationExit += (s, e) => killMiners();
        }

        private void ReadWalletAddress()
        {
            WalletAddress = File.ReadAllText(BasePath + RelativePathWalletAddress);
            TextBoxWalletAddress.Text = WalletAddress;
        }

        private void GenerateWallet()
        {
            var arguments = new[] {
                "--generate-new-wallet=\"" + BasePath + RelativePathWalletData + "\"",
                "--password=x"
            };

            var process = new Process {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(BasePath + RelativePathResources + RelativePathResourceSimplewallet, string.Join(" ", arguments)) {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            process.Exited += (sender, e) => {
                if (!File.Exists(BasePath + RelativePathWalletAddress)) {
                    Dispatcher.Invoke(() => MessageManager.ShowError("Failed to generate new wallet."));
                } else {
                    Dispatcher.Invoke(ReadWalletAddress);
                }
            };

            process.Start();
        }
    }
}
