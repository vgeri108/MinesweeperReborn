/*
 * Copyright (C) 2026 vgeri108
 *
 * This file is part of MinesweeperReborn.
 *
 * MinesweeperReborn is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 only.
 *
 * MinesweeperReborn is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with MinesweeperReborn. If not, see <https://www.gnu.org/licenses/>.
 */

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
                File.Delete(AppSettings.SaveFileOnExit);
            }
            catch { }
        }
    }
}
