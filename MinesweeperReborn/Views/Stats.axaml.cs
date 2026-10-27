using Avalonia.Controls;
using Avalonia.Interactivity;
using MinesweeperCore;
using MinesweeperReborn.Localization;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MinesweeperReborn.Views
{
    public partial class Stats : Window
    {
        private readonly Game game;
        private readonly List<string> statisticsIds = new();
        private readonly Dictionary<string, DifficultySettings> difficulties = new();
        private readonly DifficultyNameManager difficultyNames = new(
            Path.Combine(AppContext.BaseDirectory, "DifficultyNames.json"));
        private string selectedStatisticsId = string.Empty;

        public Stats()
        {
            game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
            InitializeComponent();
            FillDifficulties();
        }

        public Stats(Game game)
        {
            this.game = game;
            InitializeComponent();
            FillDifficulties();
        }

        private void FillDifficulties()
        {
            AddDifficulty(PredefinedDifficulty.Get(Difficulty.Beginner));
            AddDifficulty(PredefinedDifficulty.Get(Difficulty.Intermediate));
            AddDifficulty(PredefinedDifficulty.Get(Difficulty.Expert));

            DifficultySettings currentDifficulty = new(game.Height, game.Width, game.MineCount);
            selectedStatisticsId = game.StatisticsId;
            if (!difficulties.ContainsKey(selectedStatisticsId))
                AddDifficulty(currentDifficulty);

            Difficulties.Items.Clear();
            statisticsIds.Clear();

            foreach (KeyValuePair<string, DifficultySettings> difficulty in difficulties)
            {
                Difficulties.Items.Add(GetDisplayName(difficulty.Value));
                statisticsIds.Add(difficulty.Key);
            }

            Difficulties.SelectedIndex = statisticsIds.IndexOf(selectedStatisticsId);
            RenameMenuItem.IsEnabled = !IsPredefined(currentDifficulty);
        }

        private void AddDifficulty(DifficultySettings settings)
        {
            string id = $"{settings.Height}_{settings.Width}_{settings.Mines}";
            difficulties[id] = settings;
        }

        private string GetDisplayName(DifficultySettings settings)
        {
            if (Matches(settings, PredefinedDifficulty.Get(Difficulty.Beginner))) return Strings.Beginner;
            if (Matches(settings, PredefinedDifficulty.Get(Difficulty.Intermediate))) return Strings.Intermediate;
            if (Matches(settings, PredefinedDifficulty.Get(Difficulty.Expert))) return Strings.Advanced;
            return difficultyNames.GetName(settings);
        }

        private static bool IsPredefined(DifficultySettings settings) =>
            Matches(settings, PredefinedDifficulty.Get(Difficulty.Beginner)) ||
            Matches(settings, PredefinedDifficulty.Get(Difficulty.Intermediate)) ||
            Matches(settings, PredefinedDifficulty.Get(Difficulty.Expert));

        private static bool Matches(DifficultySettings first, DifficultySettings second) =>
            first.Height == second.Height &&
            first.Width == second.Width &&
            first.Mines == second.Mines;

        private void Difficulties_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (Difficulties.SelectedIndex < 0 || Difficulties.SelectedIndex >= statisticsIds.Count)
                return;

            selectedStatisticsId = statisticsIds[Difficulties.SelectedIndex];
            RefreshStatistics();
            RenameMenuItem.IsEnabled = !IsPredefined(difficulties[selectedStatisticsId]);
        }

        private void RefreshStatistics()
        {
            StatsData stats = Statistics.Get(selectedStatisticsId);
            PlayedGames.Text = $"Lejátszott játékok: {stats.PlayedGames}";
            WinnedGames.Text = $"Megnyert játékok: {stats.WinnedGames}";
            WinPercentage.Text = $"Nyerési arány: {stats.WinPercentage}%";
            LongestWinStreak.Text = $"Leghosszabb győzelemsorozat: {stats.LongestWinStreak}";
            LongestLoseStreak.Text = $"Leghosszabb vereségsorozat: {stats.LongestLoseStreak}";
            CurrentStreak.Text = $"Jelenlegi sorozat: {stats.CurrentStreak}";

            Times.Children.Clear();
            Dates.Children.Clear();
            Times.Children.Add(new TextBlock { Text = Strings.StatisticsNoBestTimes });
            Dates.Children.Add(new TextBlock { Text = Strings.StatisticsNoBestTimes });
        }

        private void Close_Click(object? sender, RoutedEventArgs e) => Close();

        private async void Reset_Click(object? sender, RoutedEventArgs e)
        {
            ButtonResult result = await MessageBoxManager.GetMessageBoxStandard(
                Strings.StatisticsReset,
                Strings.StatisticsResetPrompt,
                ButtonEnum.YesNo).ShowWindowDialogAsync(this);

            if (result != ButtonResult.Yes)
                return;

            Statistics.Delete(selectedStatisticsId);
            Statistics.Save(AppSettings.StatisticsPath);
            RefreshStatistics();
        }

        private async void Rename_Click(object? sender, RoutedEventArgs e)
        {
            DifficultySettings settings = difficulties[selectedStatisticsId];
            string currentName = difficultyNames.GetName(settings);
            string? newName = await PromptForName(currentName);
            if (string.IsNullOrWhiteSpace(newName))
                return;

            if (difficultyNames.SaveName(settings, newName.Trim()))
                Difficulties.Items[Difficulties.SelectedIndex] = newName.Trim();
        }

        private async Task<string?> PromptForName(string currentName)
        {
            TextBox nameInput = new() { Text = currentName, MinWidth = 240 };
            Window dialog = new()
            {
                Title = "Név módosítása",
                Width = 300,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            Button saveButton = new() { Content = "Mentés" };
            Button cancelButton = new() { Content = "Mégse" };
            saveButton.Click += (_, _) => dialog.Close(nameInput.Text);
            cancelButton.Click += (_, _) => dialog.Close(null);
            dialog.Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(12),
                Spacing = 8,
                Children =
                {
                    nameInput,
                    new StackPanel
                    {
                        Orientation = Avalonia.Layout.Orientation.Horizontal,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                        Spacing = 8,
                        Children = { cancelButton, saveButton }
                    }
                }
            };

            return await dialog.ShowDialog<string?>(this);
        }
    }
}
