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
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MinesweeperReborn
{
    internal class JsonManager
    {
        private static readonly string _path = Path.Combine(AppContext.BaseDirectory, "AppSettings.json");

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            var data = new AppSettingsData
            {
                SaveFolder = AppSettings.SaveFolder,
                SaveFileOnExit = AppSettings.SaveFileOnExit,
                CurrentTheme = AppSettings.CurrentTheme,
                Sounds = AppSettings.Sounds,
                Tips = AppSettings.Tips,
                AlwaysContinueSavedGame = AppSettings.AlwaysContinueSavedGame,
                AlwaysSaveGameOnExit = AppSettings.AlwaysSaveGameOnExit,
                EnableQuestionMarks = AppSettings.EnableQuestionMarks,
                AutomaticUpdateSearch = AppSettings.AutomaticUpdateSearch,
                ThemePreference = AppSettings.ThemePreference
            };

            string json = JsonSerializer.Serialize(data, _jsonOptions);
            File.WriteAllText(_path, json);
        }

        public static AppSettingsData Load()
        {
            if (!File.Exists(_path))
            {
                return new AppSettingsData
                {
                    SaveFolder = AppSettings.SaveFolder,
                    SaveFileOnExit = AppSettings.SaveFileOnExit,
                    CurrentTheme = AppSettings.CurrentTheme,
                    Sounds = AppSettings.Sounds,
                    Tips = AppSettings.Tips,
                    AlwaysContinueSavedGame = AppSettings.AlwaysContinueSavedGame,
                    AlwaysSaveGameOnExit = AppSettings.AlwaysSaveGameOnExit,
                    EnableQuestionMarks = AppSettings.EnableQuestionMarks,
                    AutomaticUpdateSearch = AppSettings.AutomaticUpdateSearch,
                    ThemePreference = AppSettings.ThemePreference
                };
            }

            string json = File.ReadAllText(_path);
            AppSettingsData? data = JsonSerializer.Deserialize<AppSettingsData>(json, _jsonOptions);

            if (data == null) throw new InvalidDataException("Hiba a fájl olvasása közben.");

            AppSettings.SaveFolder = data.SaveFolder;
            AppSettings.SaveFileOnExit = data.SaveFileOnExit;
            AppSettings.CurrentTheme = data.CurrentTheme;
            AppSettings.Sounds = data.Sounds;
            AppSettings.Tips = data.Tips;
            AppSettings.AlwaysContinueSavedGame = data.AlwaysContinueSavedGame;
            AppSettings.AlwaysSaveGameOnExit = data.AlwaysSaveGameOnExit;
            AppSettings.EnableQuestionMarks = data.EnableQuestionMarks;
            AppSettings.AutomaticUpdateSearch = data.AutomaticUpdateSearch;
            AppSettings.ThemePreference = data.ThemePreference;

            return data;
        }
    }
}
