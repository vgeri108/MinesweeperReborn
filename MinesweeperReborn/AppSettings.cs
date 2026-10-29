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
        public static readonly string StatisticsPath = Path.Combine(AppContext.BaseDirectory, "Statistics.json");
        public static string CurrentTheme = "Default";

        public static bool Sounds = true;
        public static bool Tips = true;
        public static bool AlwaysContinueSavedGame = false;
        public static bool AlwaysSaveGameOnExit = false;
        public static bool EnableQuestionMarks = true;

        public static bool AutomaticUpdateSearch = true;
    }
}
