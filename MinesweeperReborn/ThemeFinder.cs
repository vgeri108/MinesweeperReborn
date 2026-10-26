using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MinesweeperReborn
{
    internal class ThemeFinder
    {
        private static readonly Uri _themesUri = new("avares://MinesweeperReborn/Assets/Themes/");

        public static List<string> GetThemeList()
        {
            return AssetLoader.GetAssets(_themesUri, null)
                              .Where(asset => asset.AbsolutePath.EndsWith("/ThemeConfig.json", StringComparison.OrdinalIgnoreCase))
                              .Select(asset => asset.Segments[asset.Segments.Length - 2].TrimEnd('/'))
                              .Distinct()
                              .ToList();
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

        public static Uri GetThemeImageUri(string themeFolder)
        {
            return new Uri($"avares://MinesweeperReborn/Assets/Themes/{Uri.EscapeDataString(themeFolder)}/{GetThemeImageFromJson(themeFolder)}");
        }

        private static ThemeConfig GetThemeConfig(string themeFolder)
        {
            Uri configUri = new($"avares://MinesweeperReborn/Assets/Themes/{Uri.EscapeDataString(themeFolder)}/ThemeConfig.json");
            using Stream configStream = AssetLoader.Open(configUri);
            ThemeConfig? config = JsonSerializer.Deserialize<ThemeConfig>(configStream);

            if (config == null)
            {
                throw new InvalidDataException($"A(z) '{themeFolder}' téma konfigurációja üres.");
            }

            return config;
        }
    }
}
