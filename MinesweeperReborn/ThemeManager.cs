using Avalonia.Platform;
using MinesweeperCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MinesweeperReborn
{
    internal class ThemeManager
    {
        public static string CurrentTheme { get; private set; } = "Default";
        public static Dictionary<Cell, Uri> CellImage { get; private set; }
        public static Dictionary<CellStatus, Uri> CellStatusImage { get; private set; }

        public static void LoadTheme(string themeName)
        {
            Uri configUri = new($"avares://MinesweeperReborn/Assets/Themes/{Uri.EscapeDataString(themeName)}/ThemeConfig.json");
            using Stream configStream = AssetLoader.Open(configUri);
            ThemeConfig? config = JsonSerializer.Deserialize<ThemeConfig>(configStream);

            if (config == null)
            {
                throw new InvalidDataException($"A(z) '{themeName}' téma konfigurációja üres.");
            }

            Dictionary<Cell, Uri> cellImages = new();
            foreach (var image in config.Cells)
            {
                cellImages.Add(image.Key, BuildImageUri(themeName, image.Value));
            }

            Dictionary<CellStatus, Uri> statusImages = new();
            foreach (var image in config.Statuses)
            {
                statusImages.Add(image.Key, BuildImageUri(themeName, image.Value));
            }

            CurrentTheme = themeName;
            CellImage = cellImages;
            CellStatusImage = statusImages;
        }

        private static Uri BuildImageUri(string themeName, string imagePath)
        {
            return new Uri($"avares://MinesweeperReborn/Assets/Themes/{Uri.EscapeDataString(themeName)}/{imagePath}");
        }
    }
}
