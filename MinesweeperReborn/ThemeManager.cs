using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;
using MinesweeperCore;

namespace MinesweeperReborn
{
    internal class ThemeManager
    {
        public static string CurrentTheme = "Default";

        public static Dictionary<Cell, Uri> CellImage = new Dictionary<Cell, Uri>()
        {
            { Cell.Empty, UriBuilder("Empty.png") },
            { Cell.Mine, UriBuilder("Mine.png") },
            { Cell.One, UriBuilder("1.png")},
            { Cell.Two, UriBuilder("2.png")},
            { Cell.Three, UriBuilder("3.png")},
            { Cell.Four, UriBuilder("4.png")},
            { Cell.Five, UriBuilder("5.png")},
            { Cell.Six, UriBuilder("6.png")},
            { Cell.Seven, UriBuilder("7.png")},
            { Cell.Eight, UriBuilder("8.png")},
        };

        public static Dictionary<CellStatus, Uri> CellStatusImage = new Dictionary<CellStatus, Uri>()
        {
            { CellStatus.Covered, UriBuilder("Covered.png") },
            { CellStatus.Flagged, UriBuilder("Flagged.png") },
            { CellStatus.Questioned, UriBuilder("Question.png") },
        };

        private static Uri UriBuilder(string imageName)
        {
            return new($"avares://MinesweeperReborn/Assets/Themes/{CurrentTheme}/{imageName}", UriKind.Absolute);
        }
    }
}
