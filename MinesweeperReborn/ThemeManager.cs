using Avalonia.Media;
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
        public static ThemeSounds Sounds { get; private set; } = new();
        public static Color TextBoxBackgroundColor { get; private set; } = Color.Parse("#31569C");
        public static Color TextBoxTextColor { get; private set; } = Color.Parse("#FFFFFF");
        public static double GameBoardCorner { get; private set; }
        public static double GameBoardMargin { get; private set; }
        public static Color GameBoardButtonColor { get; private set; } = Color.Parse("#00FFFFFF");

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
                Background = BuildImagePath(themeName, config.GUI.Background),
                Clock = BuildImagePath(themeName, config.GUI.Clock),
                Flower = BuildImagePath(themeName, config.GUI.Flower),
                AboutLogo = BuildImagePath(themeName, config.GUI.AboutLogo)
            };
            ThemeSounds sounds = new()
            {
                Start = BuildSoundPath(themeName, config.Sounds.Start),
                Click = BuildSoundPath(themeName, config.Sounds.Click),
                FlagPlace = BuildSoundPath(themeName, config.Sounds.FlagPlace),
                EveryClick = BuildSoundPath(themeName, config.Sounds.EveryClick)
            };
            Color textBoxBackgroundColor = Color.Parse(config.TextBoxBackgroundColor);
            Color textBoxTextColor = Color.Parse(config.TextBoxTextColor);
            Color gameBoardButtonColor = Color.Parse(config.GameBoardButtonColor);

            CurrentTheme = themeName;
            CellImage = cellImages;
            CellStatusImage = statusImages;
            Gui = guiImages;
            Sounds = sounds;
            TextBoxBackgroundColor = textBoxBackgroundColor;
            TextBoxTextColor = textBoxTextColor;
            GameBoardCorner = config.GameBoardCorner;
            GameBoardMargin = config.GameBoardMargin;
            GameBoardButtonColor = gameBoardButtonColor;
        }

        private static string BuildSoundPath(string themeName, string soundPath)
        {
            return string.IsNullOrWhiteSpace(soundPath) ? string.Empty : AssetPaths.GetPath("Themes", themeName, soundPath);
        }

        private static string BuildImagePath(string themeName, string imagePath)
        {
            return AssetPaths.GetPath("Themes", themeName, imagePath);
        }
    }
}
