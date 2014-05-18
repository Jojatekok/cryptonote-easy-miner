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

namespace CryptoNoteMinerGUI
{
    public partial class MainWindow
    {
        internal string WalletAddress { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            var currentFile = Paths.ResourceSimpleminer;
            if (!File.Exists(currentFile)) MessageManager.ShowError("Missing file: " + currentFile);

            currentFile = Paths.ResourceSimplewallet;
            if (!File.Exists(currentFile)) MessageManager.ShowError("Missing file: " + currentFile);

            currentFile = Paths.FileWalletAddress;
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

            var cpuCoresSelectionIndex = 0;
            var cpuCoresConfig = ConfigManager.ReadString("CpuCores");
            if (cpuCoresConfig != null) {
                int cpuCoresParsed;
                if (int.TryParse(cpuCoresConfig, out cpuCoresParsed)) {
                    cpuCoresSelectionIndex = ComboBoxCpuCores.Items.IndexOf(cpuCoresParsed);
                    if (cpuCoresSelectionIndex == -1) cpuCoresSelectionIndex = 0;
                }
            }
            ComboBoxCpuCores.SelectedIndex = cpuCoresSelectionIndex;

            var poolHost = ConfigManager.ReadString("PoolHost");
            if (poolHost != null) TextBoxPoolHost.Text = poolHost;

            var poolPort = ConfigManager.ReadString("PoolPort");
            if (poolPort != null) TextBoxPoolPort.Text = poolPort;

            //Application.ApplicationExit += (s, e) => killMiners();
        }

        private void ReadWalletAddress()
        {
            WalletAddress = File.ReadAllText(Paths.FileWalletAddress);
            TextBoxWalletAddress.Text = WalletAddress;
        }

        private void GenerateWallet()
        {
            var arguments = new[] {
                "--generate-new-wallet=\"" + Paths.FileWalletData + "\"",
                "--password=x"
            };

            var process = new Process {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(Paths.ResourceSimplewallet, string.Join(" ", arguments)) {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                }
            };

            process.Exited += (sender, e) => {
                if (!File.Exists(Paths.FileWalletAddress)) {
                    Dispatcher.Invoke(() => MessageManager.ShowError("Failed to generate new wallet."));
                } else {
                    Dispatcher.Invoke(ReadWalletAddress);
                }
            };

            if (!Directory.Exists(Paths.DirectoryWalletData)) {
                Directory.CreateDirectory(Paths.DirectoryWalletData);
            }

            process.Start();
        }
    }
}
