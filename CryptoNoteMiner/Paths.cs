using System;

namespace CryptoNoteMinerGUI
{
    static class Paths
    {
        static Paths()
        {
            ConfigBasePath = AppDomain.CurrentDomain.BaseDirectory;
            ConfigRelativePathResources = Environment.Is64BitOperatingSystem ?
                                          @"Resources\64-bit\" :
                                          @"Resources\32-bit\";
        }

        private static string ConfigBasePath { get; set; }

        private const string ConfigRelativePathWalletData = @"WalletData\";
        private const string RelativePathFileWalletData = "wallet.bin";
        private const string RelativePathFileWalletAddress = RelativePathFileWalletData + ".address.txt";

        private static string ConfigRelativePathResources { get; set; }
        private const string RelativePathResourceSimpleminer = "simpleminer.exe";
        private const string RelativePathResourceSimplewallet = "simplewallet.exe";

        internal static string DirectoryWalletData { get { return ConfigBasePath + ConfigRelativePathWalletData; } }
        internal static string FileWalletData { get { return ConfigBasePath + ConfigRelativePathWalletData + RelativePathFileWalletData; } }
        internal static string FileWalletAddress { get { return ConfigBasePath + ConfigRelativePathWalletData + RelativePathFileWalletAddress; } }

        internal static string ResourceSimpleminer { get { return ConfigBasePath + ConfigRelativePathResources + RelativePathResourceSimpleminer; } }
        internal static string ResourceSimplewallet { get { return ConfigBasePath + ConfigRelativePathResources + RelativePathResourceSimplewallet; } }
    }
}
