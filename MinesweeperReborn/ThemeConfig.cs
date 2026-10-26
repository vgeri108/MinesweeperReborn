using System.Collections.Generic;
using MinesweeperCore;

namespace MinesweeperReborn
{
    internal sealed class ThemeConfig
    {
        public Dictionary<Cell, string> Cells { get; set; } = new();
        public Dictionary<CellStatus, string> Statuses { get; set; } = new();
    }
}
