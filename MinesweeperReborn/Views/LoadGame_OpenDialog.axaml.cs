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
            Close();
        }
    }
}