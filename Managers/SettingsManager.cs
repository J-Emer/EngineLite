using Newtonsoft.Json;
using System;
using System.IO;

namespace EngineLite.Engine.Managers
{
    public class SettingsManager
    {
        public static SettingsManager Instance { get; private set; }

        public Settings Settings { get; private set; }

        private readonly string _settingsFilePath;

        public SettingsManager(string settingsFilePath)
        {
            _settingsFilePath = settingsFilePath ?? throw new ArgumentNullException(nameof(settingsFilePath));

            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    using var sr = new StreamReader(_settingsFilePath);
                    Settings = JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd()) ?? new Settings();
                }
                catch
                {
                    Settings = new Settings(); // fallback
                }
            }
            else
            {
                Settings = new Settings(); // create default
                Push(); // save default to file
            }

            Instance = this;
        }

        /// <summary>
        /// Save current settings to JSON file
        /// </summary>
        public void Push()
        {
            using var sw = new StreamWriter(_settingsFilePath);
            sw.Write(JsonConvert.SerializeObject(Settings, Formatting.Indented));
        }
    }
}
