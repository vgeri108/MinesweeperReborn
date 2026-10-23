using Avalonia.Controls;
using MinesweeperCore;

namespace MinesweeperReborn;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Game game = new Game(9, 9, 10);
        game.Dig(1, 1);
        Draw draw = new Draw(game, GameBoard);
    }
}