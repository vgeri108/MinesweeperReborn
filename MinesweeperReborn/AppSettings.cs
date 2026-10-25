using Avalonia.Controls.Documents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MinesweeperReborn
{
    internal class AppSettings
    {
        public static string SaveFolder = Path.Combine(AppContext.BaseDirectory, "saves");
        public static string SaveFileOnExit = Path.Combine(SaveFolder, "exit.mine");

        public static bool Sounds = true;
        public static bool Tips = true;
        public static bool AlwaysContinueSavedGame = false;
        public static bool AlwaysSaveGameOnExit = false;
        public static bool EnableQuestionMarks = true;
    }
}
