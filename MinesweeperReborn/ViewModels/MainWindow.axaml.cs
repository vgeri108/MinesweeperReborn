using Avalonia.Controls;
using Avalonia.Interactivity;
using MinesweeperCore;
using MinesweeperReborn.Views;
using System;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    Game game;
    public MainWindow()
    {
        InitializeComponent();
    }

    public void NewGame_Click(object? sender, RoutedEventArgs e)
    {
        game = new Game(16, 16, 10);
        game.Dig(1, 1);
        Draw draw = new Draw(game, GameBoard);
    }

    public void Stats_Click(object? sender, RoutedEventArgs e)
    {

    }

    public void Settings_Click(object? sender, RoutedEventArgs e)
    {
        Settings settings = new Settings(game);
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

    }

    public void About_Click(object? sender, RoutedEventArgs e)
    {

    }
    public void MoreGames_Click(object? sender, RoutedEventArgs e)
    {

    }
}