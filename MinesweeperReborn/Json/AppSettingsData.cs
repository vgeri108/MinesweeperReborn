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
