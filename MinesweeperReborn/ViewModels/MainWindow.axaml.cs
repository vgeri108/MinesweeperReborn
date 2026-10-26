using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MinesweeperCore;
using MinesweeperReborn.Views;
using System;
using System.Diagnostics;
using System.IO;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    private Game game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
    public Action NewGameEvent => () => NewGame_Click(null, new RoutedEventArgs());
    public Action LoadGameEvent => LoadGame;
    private bool ClosePromptOpen;
    private bool AllowClose;
    public MainWindow()
    {
        InitializeComponent();
        ApplyThemeImages();
        Opened += OnOpened;
        new Draw(game, GameBoard);
    }

    private void ApplyThemeImages()
    {
        RootGrid.Background = new ImageBrush(AssetPaths.LoadBitmap(ThemeManager.Gui.Background))
        {
            Stretch = Stretch.UniformToFill
        };
        ClockImage.Source = AssetPaths.LoadBitmap(ThemeManager.Gui.Clock);
        FlowerImage.Source = AssetPaths.LoadBitmap(ThemeManager.Gui.Flower);
        TimerBox.Background = new SolidColorBrush(ThemeManager.TextBoxBackgroundColor);
        MineCounterBox.Background = new SolidColorBrush(ThemeManager.TextBoxBackgroundColor);
        Timer.Foreground = new SolidColorBrush(ThemeManager.TextBoxTextColor);
        MineCounter.Foreground = new SolidColorBrush(ThemeManager.TextBoxTextColor);
    }
    private async void OnOpened(object? sender, EventArgs e)
    {
        FirstStartDifficulty firstStartDifficulty = new FirstStartDifficulty();
        await firstStartDifficulty.ShowDialog(this);

        game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
        new Draw(game, GameBoard);

        if (SaveGameHelper.IsThereAnySavedGame()) AskGameLoad();
    }

    public void NewGame_Click(object? sender, RoutedEventArgs e)
    {
        game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
        new Draw(game, GameBoard);
    }

    public void LoadGame()
    {
        game = Game.Load(AppSettings.SaveFileOnExit);
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
        ThemeSelect themeSelect = new ThemeSelect();
        themeSelect.Closed += (_, _) =>
        {
            ApplyThemeImages();
            new Draw(game, GameBoard);
        };
        themeSelect.Show();
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
    private async void AskGameLoad()
    {
        if (!AppSettings.AlwaysContinueSavedGame)
        {
            LoadGame_OpenDialog loadGame_OpenDialog = new LoadGame_OpenDialog(game, LoadGameEvent);
            await loadGame_OpenDialog.ShowDialog(this);
        }
        else
        {
            LoadGame();
        }
        
    }
}