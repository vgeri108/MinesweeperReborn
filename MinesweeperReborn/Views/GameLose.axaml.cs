using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperCore;
using System;
using System.Diagnostics;

namespace MinesweeperReborn.Views
{
    public partial class GameLose : Window
    {
        private readonly Action startNewGame;
        private readonly Action replayGame;
        private readonly Action closeApplication;

        public GameLose(Game game, Action startNewGame, Action replayGame, Action closeApplication)
        {
            InitializeComponent();
            this.startNewGame = startNewGame;
            this.replayGame = replayGame;
            this.closeApplication = closeApplication;

            StatsData stats = Statistics.Get(game.StatisticsId);
            string bestTime = stats.BestWinTimes.Count == 0
                ? "Még nincs eredmény"
                : $"{(int)stats.BestWinTimes[0].TimeSeconds} másodperc";

            TimeText.Text = $"Idő: {(int)game.ElapsedSeconds} másodperc";
            BestTime.Text = $"Legjobb idő: {bestTime}";
            Date.Text = "Dátum: " + DateTime.Now.ToString("yyyy/MM/dd");
            PlayedGames.Text = $"Lejátszott játékok: {stats.PlayedGames}";
            WinnedGames.Text = $"Megnyert játékok: {stats.WinnedGames}";
            PercentageRating.Text = $"Százalékos értékelés: {stats.WinPercentage}%";
        }

        private void MoreGames_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/vgeri108",
                UseShellExecute = true
            });
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

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            replayGame();
            Close();
        }
    }
}