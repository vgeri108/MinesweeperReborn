using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MinesweeperReborn
{
    internal class ThemeFinder
    {
        public static List<string> GetThemeList()
        {
            List<string> themes = new();
            foreach (string themeDirectory in Directory.GetDirectories(AssetPaths.GetPath("Themes")))
            {
                if (File.Exists(Path.Combine(themeDirectory, "ThemeConfig.json")))
                {
                    themes.Add(Path.GetFileName(themeDirectory));
                }
            }

            return themes;
        }

        public static string GetThemeNameFromJson(string themeFolder)
        {
            ThemeConfig config = GetThemeConfig(themeFolder);
            return string.IsNullOrWhiteSpace(config.ThemeName) ? themeFolder : config.ThemeName;
        }

        public static string GetThemeCreatorFromJson(string themeFolder)
        {
            ThemeConfig config = GetThemeConfig(themeFolder);
            return config.Creator ?? string.Empty;
        }

        public static string GetThemeDescriptionFromJson(string themeFolder)
        {
            ThemeConfig config = GetThemeConfig(themeFolder);
            return config.Description ?? string.Empty;
        }

        public static string GetThemeImageFromJson(string themeFolder)
        {
            ThemeConfig config = GetThemeConfig(themeFolder);
            return string.IsNullOrWhiteSpace(config.ThemeImage) ? config.Statuses[MinesweeperCore.CellStatus.Covered] : config.ThemeImage;
        }

        public static string GetThemeImagePath(string themeFolder)
        {
            return AssetPaths.GetPath("Themes", themeFolder, GetThemeImageFromJson(themeFolder));
        }

        private static ThemeConfig GetThemeConfig(string themeFolder)
        {
            using Stream configStream = File.OpenRead(AssetPaths.GetPath("Themes", themeFolder, "ThemeConfig.json"));
            ThemeConfig? config = JsonSerializer.Deserialize<ThemeConfig>(configStream);

            if (config == null)
            {
                throw new InvalidDataException($"A(z) '{themeFolder}' téma konfigurációja üres.");
            }

            return config;
        }
    }
}
