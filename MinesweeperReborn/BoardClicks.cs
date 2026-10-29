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
        private readonly Action gameOver;
        private bool gameStarted;
        private bool gameOverHandled;

        public ButtonClicks(Game game, Action drawBoard, Action gameOver)
        {
            this.game = game;
            this.drawBoard = drawBoard;
            this.gameOver = gameOver;
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
            HandleGameOver();

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
            HandleGameOver();

            e.Handled = true;
        }

        public void Button_DoubleClick(object? sender, TappedEventArgs e)
        {
            if (sender is not Button { Tag: Point point })
                return;

            game.EasyMine((int)point.X, (int)point.Y);
            drawBoard();
            HandleGameOver();

            e.Handled = true;
        }

        private void HandleGameOver()
        {
            if (!game.IsGameOver || gameOverHandled)
                return;

            game.SaveStatistics(AppSettings.StatisticsPath);
            gameOverHandled = true;
            gameOver();
        }
    }
}
