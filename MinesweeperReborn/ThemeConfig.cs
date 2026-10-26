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
        public Dictionary<Cell, string> Cells { get; set; } = new();
        public Dictionary<CellStatus, string> Statuses { get; set; } = new();
        public ThemeGui GUI { get; set; } = new();
    }

    internal sealed class ThemeGui
    {
        public string Background { get; set; } = string.Empty;
        public string Clock { get; set; } = string.Empty;
        public string Flower { get; set; } = string.Empty;
    }
}
