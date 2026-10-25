using Avalonia.Controls;
using Avalonia.Interactivity;
using MinesweeperCore;
using MinesweeperReborn.Views;
using System;
using System.Diagnostics;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    private Game game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
    public Action NewGameEvent => () => NewGame_Click(null, new RoutedEventArgs());
    private bool ClosePromptOpen;
    private bool AllowClose;
    public MainWindow()
    {
        InitializeComponent();
        Opened += OnOpened;
        new Draw(game, GameBoard);
    }
    private async void OnOpened(object? sender, EventArgs e)
    {
        FirstStartDifficulty firstStartDifficulty = new FirstStartDifficulty();
        await firstStartDifficulty.ShowDialog(this);

        game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
        new Draw(game, GameBoard);
    }

    public void NewGame_Click(object? sender, RoutedEventArgs e)
    {
        game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
        new Draw(game, GameBoard);
    }

    public void Stats_Click(object? sender, RoutedEventArgs e)
    {

    }

    public void Settings_Click(object? sender, RoutedEventArgs e)
    {
        Settings settings = new Settings(game, NewGameEvent);
        settings.Show();
    }

    public void AppearanceMenu_Click(object? sender, RoutedEventArgs e)
    {

    }

    public void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    public void CheckForUpdates_Click(object? sender, RoutedEventArgs e)
    {

    }

    public void Help_Click(object? sender, RoutedEventArgs e)
    {
        NewGame_SettingsDialog newGame_SettingsDialog = new NewGame_SettingsDialog();
        newGame_SettingsDialog.Show();
    }

    public void About_Click(object? sender, RoutedEventArgs e)
    {

    }
    public void MoreGames_Click(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/vgeri108",
            UseShellExecute = true
        });
    }

    private async void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (AllowClose) return;

        e.Cancel = true;
        if (ClosePromptOpen) return;

        ClosePromptOpen = true;
        SaveGame_CloseDialog saveGame_CloseDialog = new SaveGame_CloseDialog(game);
        try
        {
            await saveGame_CloseDialog.ShowDialog(this);
            if (!saveGame_CloseDialog.IsCanceled)
            {
                AllowClose = true;
                Close();
            }
        }
        finally
        {
            ClosePromptOpen = false;
        }
    }
}