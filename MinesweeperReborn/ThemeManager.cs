using MinesweeperCore;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MinesweeperReborn
{
    internal class ThemeManager
    {
        public static string CurrentTheme { get; private set; } = "Default";
        public static Dictionary<Cell, string> CellImage { get; private set; } = new();
        public static Dictionary<CellStatus, string> CellStatusImage { get; private set; } = new();
        public static ThemeGui Gui { get; private set; } = new();

        public static void LoadTheme(string themeName)
        {
            using Stream configStream = File.OpenRead(AssetPaths.GetPath("Themes", themeName, "ThemeConfig.json"));
            ThemeConfig? config = JsonSerializer.Deserialize<ThemeConfig>(configStream);

            if (config == null)
            {
                throw new InvalidDataException($"A(z) '{themeName}' téma konfigurációja üres.");
            }

            Dictionary<Cell, string> cellImages = new();
            foreach (var image in config.Cells)
            {
                cellImages.Add(image.Key, BuildImagePath(themeName, image.Value));
            }

            Dictionary<CellStatus, string> statusImages = new();
            foreach (var image in config.Statuses)
            {
                statusImages.Add(image.Key, BuildImagePath(themeName, image.Value));
            }

            ThemeGui guiImages = new()
            {
                Background = BuildImagePath(themeName, Path.Combine("GUI", config.GUI.Background)),
                Clock = BuildImagePath(themeName, Path.Combine("GUI", config.GUI.Clock)),
                Flower = BuildImagePath(themeName, Path.Combine("GUI", config.GUI.Flower))
            };

            CurrentTheme = themeName;
            CellImage = cellImages;
            CellStatusImage = statusImages;
            Gui = guiImages;
        }

        private static string BuildImagePath(string themeName, string imagePath)
        {
            return AssetPaths.GetPath("Themes", themeName, imagePath);
        }
    }
}
