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
    public partial class SaveGame_CloseDialog : Window
    {
        public bool IsCanceled { get; private set; } =true;
        private Game game;
        public SaveGame_CloseDialog(Game _game)
        {
            InitializeComponent();
            game = _game;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            game.Save(AppSettings.SaveFileOnExit);
            Close();
        }
        private void DontSave_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = false;
            SaveGameHelper.DeleteSave();
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsCanceled = true;
            Close();
        }
    }
}