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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperCore;
using System;
using System.IO;

namespace MinesweeperReborn.Views
{
    public partial class LoadGame_OpenDialog : Window
    {
        Game game;
        private readonly Action LoadGame;
        public LoadGame_OpenDialog(Game _game, Action loadGame)
        {
            InitializeComponent();
            game = _game;
            LoadGame = loadGame;
            AlwaysContinue.IsChecked = AppSettings.AlwaysContinueSavedGame;
            JsonManager.Save();
        }

        private void AlwaysContinue_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AppSettings.AlwaysContinueSavedGame = AlwaysContinue.IsCancel;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            LoadGame();
            Close();
        }

        private void DontContinue_Click(object sender, RoutedEventArgs e)
        {
            SaveGameHelper.DeleteSave();
            Close();
        }
    }
}