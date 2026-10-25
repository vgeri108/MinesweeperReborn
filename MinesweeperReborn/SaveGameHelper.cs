using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinesweeperReborn
{
    internal class SaveGameHelper
    {
        public static bool IsThereAnySavedGame()
        {
            string[] files = Directory.GetFiles("saves", "*.mine");
            if (files.Length == 0) return false;
            return true;
        }
        public static void DeleteSave()
        {
            try
            {
                File.Delete(Path.Combine(AppContext.BaseDirectory, "saves", "exit.mine"));
            }
            catch { }
        }
    }
}
