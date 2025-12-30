using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieExplorer
{
    public static class SettingsManager
    {
        private static readonly string settingsFilePath =
        Path.Combine(FileSystem.AppDataDirectory, "settings.json");

        // Loads settings from the JSON file. If the file doesn't exist or 
        // there's an error, returns default settings.
        public static AppSettings LoadSettings()
        {
            try
            {
                // Check if settings file exists
                if (File.Exists(settingsFilePath))
                {
                    // Read the JSON content from the file
                    string json = File.ReadAllText(settingsFilePath);

                    // Deserialize JSON back into AppSettings object
                    // The ?? operator returns a new AppSettings if deserialization returns null
                    return JsonSerializer.Deserialize<AppSettings>(json)
                           ?? new AppSettings();
                }
                // File doesn't exist yet, return default settings
                return new AppSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
                return new AppSettings();

            }
        }//end LoadSettings

        // Saves the settings object to a JSON file in the app data directory.
        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                // Serialize the settings object to JSON
                // WriteIndented makes the JSON human-readable (easier to debug)
                string json = JsonSerializer.Serialize(settings,
                    new JsonSerializerOptions { WriteIndented = true });

                // Write the JSON string to the file
                File.WriteAllText(settingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }

        }

    }
}
