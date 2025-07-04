using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace BreathControl.Settings
{
    public static class ConfigStorage
    {
        private static readonly string ConfigFileName = string.Concat(BreathControlPlugin.Name, ".cfg");
        private static string ConfigFilePath => Path.Combine(MyFileSystem.UserDataPath, "Storage", ConfigFileName);

        public static void Save(Config config)
        {
            var path = ConfigFilePath;
            using (var text = File.CreateText(path))
                new XmlSerializer(typeof(Config)).Serialize(text, config);
        }

        public static Config Load()
        {
            var path = ConfigFilePath;
            if (!File.Exists(path))
            {
                return Config.Default;
            }

            var xmlSerializer = new XmlSerializer(typeof(Config));
            try
            {
                using (var streamReader = File.OpenText(path))
                    return (Config)xmlSerializer.Deserialize(streamReader) ?? Config.Default;
            }
            catch (Exception)
            {
                MyLog.Default.Warning($"{ConfigFileName}: Failed to read config file: {ConfigFilePath}");
            }
            
            return Config.Default;
        }
        
    }
}