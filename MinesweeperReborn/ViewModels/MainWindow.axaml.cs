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

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using MinesweeperCore;
using MinesweeperReborn.Views;
using System;
using System.IO;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    private Game game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
    public Action StartNewGameEvent => StartNewGame;
    public Action LoadGameEvent => LoadGame;
    private bool ClosePromptOpen;
    private bool AllowClose;
    private bool RestartOnClose;
    private bool timerPaused;
    private double pausedTimerSeconds;
    private readonly DispatcherTimer uiTimer = new() { Interval = TimeSpan.FromMilliseconds(100) };
    private dynamic? modLoader;
    private Draw? boardDraw;
    public MainWindow()
    {
        InitializeComponent();
        ApplyThemeImages();
        uiTimer.Tick += (_, _) =>
        {
            if (!timerPaused)
                Timer.Text = ((int)Math.Max(0, game.ElapsedSeconds - pausedTimerSeconds)).ToString();
        };
        uiTimer.Start();
        Opened += OnOpened;
        boardDraw = new Draw(game, GameBoard, MineCounter, OnGameOver);
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
        Game.LoadStatistics(AppSettings.StatisticsPath);

        if (!File.Exists(Path.Combine(AppContext.BaseDirectory, "AppSettings.json")))
        {
            Directory.CreateDirectory("saves");
            FirstStartDifficulty firstStartDifficulty = new FirstStartDifficulty();
            await ShowDialogWithPausedTimer(firstStartDifficulty);
            JsonManager.Save();
        }

        StartNewGame();

        if (SaveGameHelper.IsThereAnySavedGame()) await AskGameLoad();

        string modsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MinesweeperReborn",
            "Mods");

        modLoader = ModCompatible.LoadModLoader(modsDirectory, game, RequestDraw, RequestHandleGameOver, RequestLoadGame);

        if (AppSettings.AutomaticUpdateSearch)
        {
            _ = Update.CheckAndPrompt(this, automatic: true);
        }
    }

    public async void NewGame_Click(object? sender, RoutedEventArgs e)
    {
        if (!game.IsAllCellsAreHidden)
        {
            NewGame_MenuDialog newGame_MenuDialog = new NewGame_MenuDialog();
            await ShowDialogWithPausedTimer(newGame_MenuDialog);

            if (newGame_MenuDialog.Selected == "StartNew")
            {
                StartNewGame();
            }
        }
    }

    private void StartNewGame()
    {
        Sounds.PlayStart();
        game = new Game(GameDetails.Height, GameDetails.Width, GameDetails.MineCount, GameDetails.Seed);
        ((dynamic?)modLoader)?.UpdateGame(game);
        pausedTimerSeconds = 0;
        boardDraw = new Draw(game, GameBoard, MineCounter, OnGameOver);
    }

    public void LoadGame()
    {
        LoadGame(AppSettings.SaveFileOnExit);
    }

    private void LoadGame(string path)
    {
        game = Game.Load(path);
        ((dynamic?)modLoader)?.UpdateGame(game);
        pausedTimerSeconds = 0;
        boardDraw = new Draw(game, GameBoard, MineCounter, OnGameOver);
    }

    private void RequestLoadGame(string path)
    {
        Dispatcher.UIThread.Post(() => LoadGame(path));
    }

    private async void OnGameOver()
    {
        Window dialog = game.Result == Game.GameStatus.Win
            ? new GameWin(game, StartNewGameEvent, CloseApplication)
            : new GameLose(game, StartNewGameEvent, ReplayGame, CloseApplication);
        await ShowDialogWithPausedTimer(dialog);
    }

    private void ReplayGame()
    {
        string replayPath = Path.Combine(Path.GetTempPath(), $"MinesweeperReplay-{Guid.NewGuid():N}.mine");
        try
        {
            game.Save(replayPath);
            JsonObject replayData = JsonNode.Parse(File.ReadAllText(replayPath))!.AsObject();
            replayData["TimerSeconds"] = 0;

            foreach (JsonArray row in replayData["Status"]!.AsArray())
            {
                for (int column = 0; column < row.Count; column++)
                    row[column] = 0;
            }

            File.WriteAllText(replayPath, replayData.ToJsonString());
            game = Game.Load(replayPath);
        }
        finally
        {
            if (File.Exists(replayPath))
                File.Delete(replayPath);
        }

        ((dynamic?)modLoader)?.UpdateGame(game);
        pausedTimerSeconds = 0;
        boardDraw = new Draw(game, GameBoard, MineCounter, OnGameOver);
    }

    private void RequestDraw()
    {
        Dispatcher.UIThread.Post(() => boardDraw?.DrawBoard());
    }

    private void RequestHandleGameOver()
    {
        Dispatcher.UIThread.Post(() => boardDraw?.HandleGameOver());
    }

    public async void Stats_Click(object? sender, RoutedEventArgs e)
    {
        await ShowDialogWithPausedTimer(new Stats(game));
    }

    public async void Settings_Click(object? sender, RoutedEventArgs e)
    {
        Settings settings = new Settings(game, StartNewGameEvent);
        await ShowDialogWithPausedTimer(settings);
    }

    private async void ApplicationSettings_Click(object? sender, RoutedEventArgs e)
    {
        AppSettingsMenu appSettingsMenu = new AppSettingsMenu();
        await ShowDialogWithPausedTimer(appSettingsMenu);
        if (appSettingsMenu.RestartRequested)
        {
            RestartOnClose = true;
            Close();
        }
    }

    public async void AppearanceMenu_Click(object? sender, RoutedEventArgs e)
    {
        ThemeSelect themeSelect = new ThemeSelect();
        await ShowDialogWithPausedTimer(themeSelect);
        ApplyThemeImages();
        boardDraw = new Draw(game, GameBoard, MineCounter, OnGameOver);
    }

    public void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    public async void CheckForUpdates_Click(object? sender, RoutedEventArgs e)
    {
        await Update.CheckAndPrompt(this);
    }

    public async void Help_Click(object? sender, RoutedEventArgs e)
    {
        await ShowDialogWithPausedTimer(new Help());
    }

    private async Task ShowDialogWithPausedTimer(Window dialog)
    {
        Game currentGame = game;
        Stopwatch pauseDuration = Stopwatch.StartNew();
        timerPaused = true;
        try
        {
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            await dialog.ShowDialog(this);
        }
        finally
        {
            pauseDuration.Stop();
            if (ReferenceEquals(currentGame, game) && !currentGame.IsGameOver)
                pausedTimerSeconds += pauseDuration.Elapsed.TotalSeconds;
            timerPaused = false;
        }
    }

    private void CloseApplication() => Close();

    public async void About_Click(object? sender, RoutedEventArgs e)
    {
        await ShowDialogWithPausedTimer(new About());
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

        if (game.IsAllCellsAreHidden || game.IsGameOver)
        {
            FinishClosing();
            return;
        }

        ClosePromptOpen = true;
        SaveGame_CloseDialog saveGame_CloseDialog = new SaveGame_CloseDialog(game);
        try
        {
            await ShowDialogWithPausedTimer(saveGame_CloseDialog);
            if (!saveGame_CloseDialog.IsCanceled)
            {
                FinishClosing();
            }
            else
            {
                RestartOnClose = false;
            }
        }
        finally
        {
            ClosePromptOpen = false;
        }
    }

    private void FinishClosing()
    {
        ((dynamic?)modLoader)?.UnloadAll();
        modLoader = null;
        if (RestartOnClose)
            Process.Start(Environment.ProcessPath);

        AllowClose = true;
        Close();
    }

    private async Task AskGameLoad()
    {
        if (!AppSettings.AlwaysContinueSavedGame)
        {
            LoadGame_OpenDialog loadGame_OpenDialog = new LoadGame_OpenDialog(game, LoadGameEvent);
            await ShowDialogWithPausedTimer(loadGame_OpenDialog);
        }
        else
        {
            LoadGame();
        }

    }
}