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
using Avalonia.Styling;
using MinesweeperCore;

namespace MinesweeperReborn.Views
{
    public partial class FirstStartDifficulty : Window
    {
        public FirstStartDifficulty()
        {
            InitializeComponent();
        }
        private void NewBeginner(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Beginner).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Beginner).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Beginner).Mines;
            Close();
        }
        private void NewIntermediate(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Intermediate).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Intermediate).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Intermediate).Mines;
            Close();
        }
        private void NewAdvanced(object sender, RoutedEventArgs e)
        {
            GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Expert).Height;
            GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Expert).Width;
            GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Expert).Mines;
            Close();
        }
    }
}