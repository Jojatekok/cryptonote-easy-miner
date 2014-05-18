using System.Collections.Generic;
using System.Configuration;

namespace CryptoNoteMinerGUI
{
    public static class ConfigManager
    {
        public static string ReadString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void WriteStrings(Dictionary<string, string> dictionary)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            foreach (var setting in dictionary) {
                if (settings[setting.Key] == null) {
                    settings.Add(setting.Key, setting.Value);
                } else {
                    settings[setting.Key].Value = setting.Value;
                }
            }

            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public static void WriteString(string key, string value)
        {
            WriteStrings(new Dictionary<string, string>(1) {
                { key, value }
            });
        }

        public static void WriteString(KeyValuePair<string, string> keyValuePair)
        {
            WriteStrings(new Dictionary<string, string>(1) {
                { keyValuePair.Key, keyValuePair.Value }
            });
        }
    }
}
