using System.Collections.Generic;
using MinesweeperCore;

namespace MinesweeperReborn
{
    internal sealed class ThemeConfig
    {
        public string ThemeName { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string ThemeImage { get; set; }
        public string LicenseName { get; set; }
        public string LicenseUrl { get; set; }
        public string TextBoxBackgroundColor { get; set; } = "#31569C";
        public string TextBoxTextColor { get; set; } = "#FFFFFF";
        public double GameBoardCorner { get; set; }
        public double GameBoardMargin { get; set; }
        public string GameBoardButtonColor { get; set; } = "#00FFFFFF";
        public Dictionary<Cell, string> Cells { get; set; } = new();
        public Dictionary<CellStatus, string> Statuses { get; set; } = new();
        public ThemeGui GUI { get; set; } = new();
        public ThemeSounds Sounds { get; set; } = new();
    }

    internal sealed class ThemeGui
    {
        public string Background { get; set; } = string.Empty;
        public string Clock { get; set; } = string.Empty;
        public string Flower { get; set; } = string.Empty;
        public string AboutLogo { get; set; } = string.Empty;
    }

    internal sealed class ThemeSounds
    {
        public string Start { get; set; } = string.Empty;
        public string Click { get; set; } = string.Empty;
        public string FlagPlace { get; set; } = string.Empty;
        public string EveryClick { get; set; } = string.Empty;
    }
}
