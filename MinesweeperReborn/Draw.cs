using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MinesweeperCore;
using System;
using System.Collections.Generic;

namespace MinesweeperReborn
{
    internal class Draw
    {
        public Game game;
        public UniformGrid grid;
        private readonly List<Button> buttonPool = new();
        private readonly ButtonClicks buttonClicks;
        private IBrush buttonBackground = Brushes.Transparent;

        private Dictionary<object, Bitmap> ImageCache = new Dictionary<object, Bitmap>();

        public Draw(Game game, UniformGrid grid)
        {
            this.game = game;
            this.grid = grid;
            buttonClicks = new ButtonClicks(game, DrawBoard);

            CacheImages();
            InitBoard();
            DrawBoard();
        }

        private void CacheImages()
        {
            foreach (var item in Enum.GetValues<Cell>())
            {
                if (!ImageCache.ContainsKey(item)) ImageCache.Add(item, AssetPaths.LoadBitmap(ThemeManager.CellImage[item]));
            }
            foreach (var item in Enum.GetValues<CellStatus>())
            {
                if (item != CellStatus.Uncovered)
                {
                    if (!ImageCache.ContainsKey(item)) ImageCache.Add(item, AssetPaths.LoadBitmap(ThemeManager.CellStatusImage[item]));
                }
            }
        }

        private void InitBoard()
        {
            grid.Children.Clear();
            buttonPool.Clear();
            grid.Rows = game.Height;
            grid.Columns = game.Width;
            buttonBackground = new SolidColorBrush(ThemeManager.GameBoardButtonColor);

            for (int x = 0; x < game.Height; x++)
            {
                for (int y = 0; y < game.Width; y++)
                {
                    Button btn = new Button
                    {
                        Tag = new Point(x, y),
                        Margin = new Thickness(ThemeManager.GameBoardMargin),
                        Padding = new Thickness(0),
                        BorderThickness = new Thickness(0),
                        BorderBrush = Brushes.Transparent,
                        CornerRadius = new CornerRadius(0),
                        Background = Brushes.Transparent,
                    };

                    btn.AddHandler(InputElement.PointerPressedEvent, buttonClicks.Button_LeftClick, RoutingStrategies.Bubble, handledEventsToo: true);
                    btn.AddHandler(InputElement.PointerPressedEvent, buttonClicks.Button_RightClick, RoutingStrategies.Bubble, handledEventsToo: true);
                    btn.AddHandler(InputElement.DoubleTappedEvent, buttonClicks.Button_DoubleClick, RoutingStrategies.Bubble, handledEventsToo: true);

                    btn.Content = CreateCellContent(CellStatus.Covered);

                    buttonPool.Add(btn);
                    grid.Children.Add(btn);
                }
            }
        }

        private void DrawBoard()
        {
            for (int x = 0; x < game.Height; x++)
            {
                for (int y = 0; y < game.Width; y++)
                {
                    int index = x * game.Width + y;

                    object sourceCell = Cell.Empty;

                    switch (game.Status[x, y])
                    {
                        case CellStatus.Uncovered:
                            sourceCell = game.GetCell(x, y);
                            break;
                        case CellStatus.Covered:
                            sourceCell = game.Status[x, y];
                            break;
                        case CellStatus.Flagged:
                            sourceCell = game.Status[x, y];
                            break;
                        case CellStatus.Questioned:
                            sourceCell = game.Status[x, y];
                            break;
                    }

                    buttonPool[index].Content = CreateCellContent(sourceCell);
                }
            }
        }

        private Border CreateCellContent(object sourceCell)
        {
            return new Border
            {
                CornerRadius = new CornerRadius(ThemeManager.GameBoardCorner),
                ClipToBounds = true,
                Background = buttonBackground,
                Child = new Image
                {
                    Source = ImageCache[sourceCell],
                    Stretch = Avalonia.Media.Stretch.UniformToFill,
                    Margin = new Thickness(0),
                }
            };
        }
    }
}
