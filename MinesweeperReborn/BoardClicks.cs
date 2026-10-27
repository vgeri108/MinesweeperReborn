using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MinesweeperCore;
using System;

namespace MinesweeperReborn
{
    internal class ButtonClicks
    {
        private readonly Game game;
        private readonly Action drawBoard;
        private bool gameStarted;

        public ButtonClicks(Game game, Action drawBoard)
        {
            this.game = game;
            this.drawBoard = drawBoard;
        }

        public void Button_LeftClick(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind != PointerUpdateKind.LeftButtonPressed)
                return;

            if (sender is not Button { Tag: Point point })
                return;

            if (!gameStarted)
            {
                Sounds.PlayClick();
                gameStarted = true;
            }
            else
            {
                Sounds.PlayEveryClick();
            }
            
            game.Dig((int)point.X, (int)point.Y);
            drawBoard();
            if (game.IsGameOver)
                game.SaveStatistics(GameDetails.StatisticsPath);

            e.Handled = true;
        }

        public void Button_RightClick(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind != PointerUpdateKind.RightButtonPressed)
                return;

            if (sender is not Button { Tag: Point point })
                return;

            Sounds.PlayFlag();

            int row = (int)point.X;
            int column = (int)point.Y;
            game.Cycle(row, column);
            if (!AppSettings.EnableQuestionMarks && game.Status[row, column] == CellStatus.Questioned)
                game.Cycle(row, column);

            drawBoard();

            e.Handled = true;
        }

        public void Button_DoubleClick(object? sender, TappedEventArgs e)
        {
            if (sender is not Button { Tag: Point point })
                return;

            game.EasyMine((int)point.X, (int)point.Y);
            drawBoard();
            if (game.IsGameOver)
                game.SaveStatistics(GameDetails.StatisticsPath);

            e.Handled = true;
        }
    }
}
