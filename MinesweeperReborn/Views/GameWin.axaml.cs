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
using MinesweeperReborn.Localization;
using System;
using System.Diagnostics;

namespace MinesweeperReborn.Views
{
    public partial class GameWin : Window
    {
        private readonly Action startNewGame;
        private readonly Action closeApplication;

        public GameWin(Game game, Action startNewGame, Action closeApplication)
        {
            InitializeComponent();
            this.startNewGame = startNewGame;
            this.closeApplication = closeApplication;

            StatsData stats = Statistics.Get(game.StatisticsId);
            string bestTime = stats.BestWinTimes.Count == 0
                ? Strings.StatisticsNoBestTimes
                : string.Format(Strings.GameResultSeconds, (int)stats.BestWinTimes[0].TimeSeconds);

            TimeText.Text = string.Format(Strings.GameResultTime, (int)game.ElapsedSeconds);
            BestTime.Text = string.Format(Strings.GameResultBestTime, bestTime);
            Date.Text = string.Format(Strings.GameResultDate, DateTime.Now.ToString("d", Strings.Culture));
            PlayedGames.Text = string.Format(Strings.StatisticsPlayedGames, stats.PlayedGames);
            WinnedGames.Text = string.Format(Strings.StatisticsWonGames, stats.WinnedGames);
            PercentageRating.Text = string.Format(Strings.StatisticsWinPercentage, stats.WinPercentage);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            closeApplication();
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            startNewGame();
            Close();
        }

        private void MoreGames_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/vgeri108",
                UseShellExecute = true
            });
        }
    }
}