using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MinesweeperCore;
using MinesweeperReborn.Localization;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MinesweeperReborn.Views
{
    public partial class Settings : Window
    {
        public bool NeedNewGame { get; private set; } = false;
        public bool ContinueTimer { get; private set; } = false;
        private int MaxMines;
        private Game game;
        private readonly Action StartNewGame;
        public Settings(Game _game, Action startNewGame)
        {
            InitializeComponent();
            game = _game;

            Sounds.IsChecked = AppSettings.Sounds;
            Tips.IsChecked = AppSettings.Tips;
            ContinueSaved.IsChecked = AppSettings.AlwaysContinueSavedGame;
            SaveExit.IsChecked = AppSettings.AlwaysSaveGameOnExit;
            QuestionMarks.IsChecked = AppSettings.EnableQuestionMarks;
            tbHeight.Text = game.Height.ToString();
            tbWidth.Text = game.Width.ToString();
            tbMines.Text = game.MineCount.ToString();

            if (game.Height == PredefinedDifficulty.Get(Difficulty.Beginner).Height &&
                game.Width == PredefinedDifficulty.Get(Difficulty.Beginner).Width &&
                game.MineCount == PredefinedDifficulty.Get(Difficulty.Beginner).Mines)
            {
                Easy.IsChecked = true;
            }
            else if (game.Height == PredefinedDifficulty.Get(Difficulty.Intermediate).Height &&
                game.Width == PredefinedDifficulty.Get(Difficulty.Intermediate).Width &&
                game.MineCount == PredefinedDifficulty.Get(Difficulty.Intermediate).Mines)
            {
                Intermediate.IsChecked = true;
            }
            else if (game.Height == PredefinedDifficulty.Get(Difficulty.Expert).Height &&
                game.Width == PredefinedDifficulty.Get(Difficulty.Expert).Width &&
                game.MineCount == PredefinedDifficulty.Get(Difficulty.Expert).Mines)
            {
                Advanced.IsChecked = true;
            }
            else
            {
                rbCustom.IsChecked = true;
            }
            CalcMaxMines();
            StartNewGame = startNewGame;
        }

        private void CalcMaxMines()
        {
            MaxMines = (int)Math.Round((Convert.ToInt32(tbHeight.Text) * Convert.ToInt32(tbWidth.Text)) * 0.80);
            MineCountText.Text = string.Format(Strings.MinesRange, 10, MaxMines);
        }
        private void tbHeight_TextChanged(object? sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbHeight.Text, out value);
            if ((!Valid || tbHeight.Text.Contains(' ')) && tbHeight.Text.Length > 0)
            {
                tbHeight.Text = tbHeight.Text.Remove(tbHeight.Text.Length - 1);
                tbHeight.CaretIndex = tbHeight.Text.Length;
            }
        }

        private void tbHeight_LostFocus(object? sender, RoutedEventArgs e)
        {
            if (tbHeight.Text.Length == 0) tbHeight.Text = "9";
            if (Convert.ToInt32(tbHeight.Text) < 9) tbHeight.Text = "9";
            if (Convert.ToInt32(tbHeight.Text) > 24) tbHeight.Text = "24";
            CalcMaxMines();
        }

        private void tbWidth_TextChanged(object? sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbWidth.Text, out value);
            if ((!Valid || tbWidth.Text.Contains(' ')) && tbWidth.Text.Length > 0)
            {
                tbWidth.Text = tbWidth.Text.Remove(tbWidth.Text.Length - 1);
                tbWidth.CaretIndex = tbWidth.Text.Length;
            }
        }

        private void tbWidth_LostFocus(object? sender, RoutedEventArgs e)
        {
            if (tbWidth.Text.Length == 0) tbWidth.Text = "9";
            if (Convert.ToInt32(tbWidth.Text) < 9) tbWidth.Text = "9";
            if (Convert.ToInt32(tbWidth.Text) > 30) tbWidth.Text = "30";
            CalcMaxMines();
        }

        private void tbMines_TextChanged(object? sender, TextChangedEventArgs e)
        {
            int value;
            bool Valid = int.TryParse(tbMines.Text, out value);
            if ((!Valid || tbMines.Text.Contains(' ')) && tbMines.Text.Length > 0)
            {
                tbMines.Text = tbMines.Text.Remove(tbHeight.Text.Length - 1);
                tbMines.CaretIndex = tbHeight.Text.Length;
            }
        }

        private void tbMines_LostFocus(object? sender, RoutedEventArgs e)
        {
            if (tbMines.Text.Length == 0) tbMines.Text = "10";
            if (Convert.ToInt32(tbMines.Text) < 9) tbMines.Text = "10";
            if (Convert.ToInt32(tbMines.Text) > MaxMines) tbMines.Text = MaxMines.ToString();
        }

        private async void OK_Click(object? sender, RoutedEventArgs e)
        {
            if (rbCustom.IsChecked == false)
            {
                if (Easy.IsChecked == true)
                {
                    GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Beginner).Height;
                    GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Beginner).Width;
                    GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Beginner).Mines;
                }

                if (Intermediate.IsChecked == true)
                {
                    GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Intermediate).Height;
                    GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Intermediate).Width;
                    GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Intermediate).Mines;
                }

                if (Advanced.IsChecked == true)
                {
                    GameDetails.Height = PredefinedDifficulty.Get(Difficulty.Expert).Height;
                    GameDetails.Width = PredefinedDifficulty.Get(Difficulty.Expert).Width;
                    GameDetails.MineCount = PredefinedDifficulty.Get(Difficulty.Expert).Mines;
                }
            }
            else
            {
                try
                {
                    GameDetails.Height = int.Parse(tbHeight.Text);
                    GameDetails.Width = int.Parse(tbWidth.Text);
                    GameDetails.MineCount = int.Parse(tbMines.Text);

                    GameDetails.Height = int.Parse(tbHeight.Text);
                    GameDetails.Width = int.Parse(tbWidth.Text);
                    GameDetails.MineCount = int.Parse(tbMines.Text);
                }
                catch (Exception error)
                {
                    await MessageBoxManager.GetMessageBoxStandard("Hiba", error.Message, ButtonEnum.Ok).ShowAsync();
                }
            }

            //Configuration.Animations = (bool)Animations.IsChecked;
            AppSettings.Sounds = (bool)Sounds.IsChecked;
            AppSettings.Tips = (bool)Tips.IsChecked;
            AppSettings.AlwaysContinueSavedGame = (bool)ContinueSaved.IsChecked;
            AppSettings.AlwaysSaveGameOnExit = (bool)SaveExit.IsChecked;
            AppSettings.EnableQuestionMarks = (bool)QuestionMarks.IsChecked;



            if (game.Height != GameDetails.Height || game.Width != GameDetails.Width || game.MineCount != GameDetails.MineCount) NeedNewGame = true;


            if (NeedNewGame && !game.IsAllCellsAreHidden)
            {
                NewGame_SettingsDialog newGame_SettingsDialog = new NewGame_SettingsDialog();
                await newGame_SettingsDialog.ShowDialog(this);

                if (newGame_SettingsDialog.Selected == "Cancel")
                {
                    ContinueTimer = true;
                }
                if (newGame_SettingsDialog.Selected == "OnNextGame")
                {
                    ContinueTimer = true;
                }
                if (newGame_SettingsDialog.Selected == "StartNew")
                {
                    StartNewGame();
                }
            }
            else
            {
                if (game.IsAllCellsAreHidden) StartNewGame();
            }
            JsonManager.Save();
            Close();
        }

        private void Cancel_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}