using Avalonia.Controls;
using MinesweeperCore;
using Avalonia.Interactivity;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    private readonly MenuClicks menuClicks = new();

    public MainWindow()
    {
        InitializeComponent();
        Game game = new Game(9, 9, 10);
        game.Dig(1, 1);
        Draw draw = new Draw(game, GameBoard);
    }

    private void NewGame_Click(object? sender, RoutedEventArgs e) => menuClicks.NewGame_Click(sender, e);
    private void Stats_Click(object? sender, RoutedEventArgs e) => menuClicks.Stats_Click(sender, e);
    private void Settings_Click(object? sender, RoutedEventArgs e) => menuClicks.Settings_Click(sender, e);
    private void AppearanceMenu_Click(object? sender, RoutedEventArgs e) => menuClicks.AppearanceMenu_Click(sender, e);
    private void Exit_Click(object? sender, RoutedEventArgs e) => menuClicks.Exit_Click(sender, e);
    private void CheckForUpdates_Click(object? sender, RoutedEventArgs e) => menuClicks.CheckForUpdates_Click(sender, e);
    private void Help_Click(object? sender, RoutedEventArgs e) => menuClicks.Help_Click(sender, e);
    private void About_Click(object? sender, RoutedEventArgs e) => menuClicks.About_Click(sender, e);
    private void MoreGames_Click(object? sender, RoutedEventArgs e) => menuClicks.MoreGames_Click(sender, e);
}