using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperReborn
{
    internal class AppSettingsData
    {
        public string SaveFolder {  get; set; }
        public string SaveFileOnExit { get; set; }
        public string CurrentTheme { get; set; } = "Default";
        public bool Sounds { get; set; }
        public bool Tips { get; set; }
        public bool AlwaysContinueSavedGame { get; set; }
        public bool AlwaysSaveGameOnExit { get; set; }
        public bool EnableQuestionMarks { get; set; }
        public bool AutomaticUpdateSearch { get; set; } = true;
    }
}
